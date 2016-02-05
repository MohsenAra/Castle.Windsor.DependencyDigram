using System.Collections.Generic;
using System.Linq;

namespace Castle.Windsor.DependencyDigram
{
    public class ComponentInfo
    {
        public string Name { get; set; }
        public string FriendlyName { get; set; }
        public List<ComponentInfo> Dependents{ get; set; }
        public List<string> Services { get; set; }

        public string Service
        {
            get
            {
                var counter = 0;
                var nameList = new List<string>();
                const int listSize = 4;
                do
                {
                    var list = Services.OrderBy(c => c).Skip(counter).Take(listSize);
                    counter += listSize;
                    nameList.Add(string.Join(",", list));
                } while (Services.Count() > counter);

                return string.Join("\n", nameList);
            }
        }
    }
}