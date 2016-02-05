using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using Castle.Core.Internal;

namespace Castle.Windsor.DependencyDigram
{
    public static class VertexExtensions
    {
        public static string Name(this IVertex v)
        {
            var model = (ComponentModel)v;
            var serviceName = GetServiceName(model);

            return string.Format("{0}\n{1}", model.Implementation.GetFriendlyName(serviceName), serviceName);
        }

        public static ComponentInfo GetComponentInfo(this IVertex v,bool load)
        {
            var model = (ComponentModel)v;
            var serviceName = GetServiceName(model);
            var x = new ComponentInfo()
            {
                FriendlyName = model.Implementation.GetFriendlyName(serviceName),
                Name = model.Implementation.FullName,
                Services = model.Services.Select(GetFriendlyName).ToList(),
                Dependents =!load? null:model.Dependents.Select(c => GetComponentInfo(c,false)).ToList()
            };
            return x;
        }

        private static string GetServiceName(ComponentModel model)
        {
            var names = model.Services.Select(d => d.GetFriendlyName()).ToList();
            var counter = 0;
            var nameList = new List<string>();
            const int listSize = 4;
            do
            {
                var list = names.OrderBy(c => c).Skip(counter).Take(listSize);
                counter += listSize;
                nameList.Add(string.Join(",", list));
            } while (model.Services.Count() > counter);

            return string.Join("\n", nameList);
        }

        //public static string Name(this IVertex v)
        //{
        //    var model = (ComponentModel)v;
        //    var x= string.Format("{0}/{1}", model.Implementation.GetFriendlyName(), string.Join(",", model.Services.Select(d => d.GetFriendlyName())));
        //    return x;
        //}
        public static string GetFriendlyName(this Type type, string serviceName)
        {
            if (type == typeof(LateBoundComponent))
                return "Lazy <" + serviceName + ">";
            return GetFriendlyName(type);
        }

        public static string GetFriendlyName(this Type type)
        {

            if (type == typeof(int))
                return "int";
            if (type == typeof(short))
                return "short";
            if (type == typeof(byte))
                return "byte";
            if (type == typeof(bool))
                return "bool";
            if (type == typeof(long))
                return "long";
            if (type == typeof(float))
                return "float";
            if (type == typeof(double))
                return "double";
            if (type == typeof(decimal))
                return "decimal";
            if (type == typeof(string))
                return "string";

            if (type.IsGenericType)
                return type.Name.Split('`')[0] + "<" + string.Join(", ", type.GetGenericArguments().Select(GetFriendlyName).ToArray()) + ">";

            return type.Name;
        }
    }
}