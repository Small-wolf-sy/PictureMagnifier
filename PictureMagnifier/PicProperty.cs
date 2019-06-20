using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace PictureMagnifier
{
    public class PicProperty
    {
        private string fileposition = "";//地址
        private Image image;//图片
        private double width;//宽度
        private double height;//高度

        //获取图片
        public Image GetImage { get { return image; } set { image = value; } }
        //获取宽度
        public double Width { get { return width; }}
        //获取高度
        public double Height { get { return height; } }
        //获取路径
        public string getpath { get { return fileposition; } }

        //构造函数
        public PicProperty(string path,Image origin_image)
        {
            this.fileposition = path;
            this.image = origin_image;
            this.width = origin_image.Width;
            this.height = origin_image.Height;
        }
    }
}
