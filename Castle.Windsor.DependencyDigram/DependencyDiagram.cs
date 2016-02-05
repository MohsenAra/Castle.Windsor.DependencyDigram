using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using QuickGraph;

namespace Castle.Windsor.DependencyDigram
{
    public class DependencyDiagram
    {

        public static void SaveGraphNodesInJsonFile(IWindsorContainer container,string filePath)
        {
            var dependencyDict = ExtractNodes(container);
            var x = JsonConvert.SerializeObject(dependencyDict);
            File.WriteAllText(filePath, x);
        }

        private static List<ComponentInfo> ExtractNodes(IWindsorContainer container)
        {
            var dependencyDict = container.Kernel.GraphNodes
                .Select(c => c.GetComponentInfo(true)).ToList();
            return dependencyDict;
        }

        public static List<ComponentInfo> LoadGraphNodesFromJsonFile(string filePath)
        {
            var x = File.ReadAllText(filePath);
            var nodes = JsonConvert.DeserializeObject<List<ComponentInfo>>(x);
            nodes=nodes.Distinct(new EqComparer<ComponentInfo>((a, b) => a.FriendlyName == b.FriendlyName,c=>c.FriendlyName.GetHashCode())).ToList();
            return nodes;
        }

        public static void CreateBigPicture(IWindsorContainer container, string path, Func<KeyValuePair<string, IEnumerable<string>>, bool> func)
        {
            var infoes  = ExtractNodes(container);
            var depends = getDepends(infoes, func);

            ExportToPng(infoes, depends, path);

        }

        public static void CreatePictureByRootHead(IWindsorContainer container, string path)
        {
            var x = ExtractNodes(container);
            CreatePictureByRootHead(x,path);

        }
        public static void CreatePictureByRootHead(List<ComponentInfo> infoes,string path)
        {
            var list = getOrphanDepends(infoes);
            foreach (var name in list)
            {
                var depends = getDepends(infoes, pair => pair.Key == name);

               var x= name.Replace("\n", "-").Replace("<", "_").Replace(">", "_");
                if (x.Length > 200)
                    x= name.Remove(200);
                ExportToPng(infoes, depends, path+x+".png");
            }
        }

        private static void ExportToPng(List<ComponentInfo> infoes, List<string> depends, string path)
        {

            var dependencyDict = GetNodeDictionary2(infoes, depends);
            var graph =
                dependencyDict.ToVertexAndEdgeListGraph(
                    kv => kv.Value.Select(n => new SEquatableEdge<string>(kv.Key, n)));

            var bi = graph.ToGleeGraph().ToBitmap();
            if (File.Exists(path))
                File.Delete(path);
            bi.Save(path);
        }


        private static List<string> getOrphanDepends(List<ComponentInfo> infoes)
        {
            var nodes = GetNodeDictionary2(infoes,null);
            var childs = nodes.SelectMany(c => c.Value).ToList();
            var list = new List<string>();
            foreach (var node in nodes)
            {
                if (childs.All(c => c != node.Key))
                    list.Add(node.Key);
            }
            return list;
        }

       
        private static Dictionary<string, IEnumerable<string>> GetNodeDictionary2(List<ComponentInfo> infoes, List<string> depends)
        {
            return 
                (depends == null ? infoes : infoes.Where(c => depends.Contains(c.FriendlyName)))
                    .ToDictionary(n => n.FriendlyName, n => n.Dependents.Select(c=>c.FriendlyName));

        }

        private static List<string> getDepends(List<ComponentInfo> infoes, Func<KeyValuePair<string, IEnumerable<string>>, bool> func)
        {
            var nodes = GetNodeDictionary2(infoes,null);
            var list = new List<string>();
            var filteredNodes = nodes.Where(func);
            foreach (var node in filteredNodes)
            {
                list.Add(node.Key);
                foreach (var subNode in node.Value)
                {
                    list.AddRange(GetSubNodes(nodes, subNode));
                }
            }
            return list;
        }

        private static List<string> GetSubNodes(Dictionary<string, IEnumerable<string>> nodes, string key)
        {
            var list = new List<string>();
            foreach (var node in nodes.Where(c => c.Key == key))
            {
                list.Add(node.Key);
                foreach (var subNode in node.Value)
                {
                    list.AddRange(GetSubNodes(nodes, subNode));
                }
            }
            return list;
        }
    }
}