using Microsoft.Glee.Drawing;
using QuickGraph;
using QuickGraph.Glee;

namespace Castle.Windsor.DependencyDigram
{
    public static class EdgeListGraphExtensions
    {
        public static Graph ToGleeGraph<V, E>(this IEdgeListGraph<V, E> graph) where E : IEdge<V>
        {
            var populator = graph.CreateGleePopulator();
            populator.NodeAdded += (sender, args) =>
            {
                var attr = args.Node.Attr;
                attr.Fillcolor = new Microsoft.Glee.Drawing.Color(241, 241, 241);
                attr.LabelMargin = 5;
                attr.FontName = "Verdana";
                attr.Fontsize = 10;
            };
            populator.Compute();
            return populator.GleeGraph;
        }
    }
}