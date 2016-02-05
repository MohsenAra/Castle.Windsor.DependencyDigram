using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using Microsoft.Glee.Drawing;
using Microsoft.Glee.GraphViewerGdi;

namespace Castle.Windsor.DependencyDigram
{
    public static class GleeGraphExtensions
    {
        public static Bitmap ToBitmap(this Graph graph)
        {
            var renderer = new GraphRenderer(graph);
            renderer.CalculateLayout();
            var bmp = new Bitmap((int)graph.Width, (int)graph.Height, PixelFormat.Format32bppArgb);
            using (var gr = Graphics.FromImage(bmp))
            {
                gr.CompositingQuality = CompositingQuality.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBilinear;
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                gr.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                renderer.Render(gr, 0, 0, bmp.Width, bmp.Height);
            }
            return bmp;
        }
       
    }

}
