using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureMagnifier
{
    //该类与所有的图片处理函数有关
    public static class PicFunction
    {
        public static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, Size size)
        {
            //获取图片宽度
            int sourceWidth = imgToResize.Width;
            //获取图片高度
            int sourceHeight = imgToResize.Height;

            float nPercentW = 0;
            float nPercentH = 0;
            //计算宽度的缩放比例
            nPercentW = ((float)size.Width / (float)sourceWidth);
            //计算高度的缩放比例
            nPercentH = ((float)size.Height / (float)sourceHeight);

            //按照填充来干
            //期望的宽度
            int destWidth = (int)(sourceWidth * nPercentW);
            //期望的高度
            int destHeight = (int)(sourceHeight * nPercentH);

            //将图片转移
            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);

            g.InterpolationMode = InterpolationMode.HighQualityBilinear;

            //释放内存
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            GC.Collect();
            return (System.Drawing.Image)b;
        }
    }
}
