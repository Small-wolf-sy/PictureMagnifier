using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PictureMagnifier
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Thread timeshow = new Thread(TimeStart);
            timeshow.Start();
            ButtonEvent();
            RefreshEvent();
            BarEvent();
        }

        #region 相关内部属性
        private PicProperty origin_Pic=null;//原始图像，即使图片在软件内显示时效果较差，也要保证放大的效果无误
        private float nPercentW = 0;//原图片与压缩后的宽比例，用于映射原图
        private float nPercentH = 0;//原图片与压缩后的高比例，用于映射原图
        private int Mouse_X = 0;
        private int Mouse_Y = 0;
        #endregion 

        /// <summary>
        /// 存储所有button事件的声明
        /// </summary>
        void ButtonEvent()
        {
            ReadButton.Click += ReadButton_Click;
            HelpButton.Click += HelpButton_Click;
            copyrightbutton.Click += Copyrightbutton_Click;
            this.FormClosed += Form1_FormClosed;
        }

        /// <summary>
        /// 窗体关闭时终止所有线程，包括计时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        /// <summary>
        /// 鼠标在图片上移动时的事件
        /// </summary>
        void RefreshEvent()
        {
            oriPicBox.MouseMove += OriPicBox_MouseMove;
        }

        /// <summary>
        /// 滑动条拖动时间
        /// </summary>
        void BarEvent()
        {
            magnivalue.ValueChanged += Magnivalue_ValueChanged;
        }

        /// <summary>
        /// 改变放大倍率时的放大区域自调整，为了提高精度，插件设置范围是10-100，对应1-10倍，这样可以0.1倍精度调整
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Magnivalue_ValueChanged(object sender, EventArgs e)
        {
            //直接根据放大区域的图的中心找到左上角
            float target_X = Mouse_X - magniBox.Width / (2 * (magnivalue.Value/10));
            float target_Y = Mouse_Y - magniBox.Height / (2 * (magnivalue.Value/10));

            #region 提取根据放大倍数得到的区域图片
            //建立新的图片
            Bitmap b = new Bitmap(magniBox.Width / (magnivalue.Value/10), magniBox.Height / (magnivalue.Value/10));
            //建立画布，在画布上建立图形
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);

            g.InterpolationMode = InterpolationMode.HighQualityBilinear;

            RectangleF srcRect = new RectangleF(target_X, target_Y, target_X + magniBox.Width / (magnivalue.Value/10),
                target_Y + magniBox.Height / (magnivalue.Value/10));//显示图像那一部分
            GraphicsUnit units = GraphicsUnit.Pixel;//源矩形的度量单位设置为像素
            //绘制图像，可用的方法有很多
            g.DrawImage(origin_Pic.GetImage, 0, 0, srcRect, units);
            //释放内存
            g.Dispose();
            GC.Collect();
            #endregion

            //将整个区域的图调整大小
            magniBox.Image = PicFunction.resizeImage((System.Drawing.Image)b, magniBox.Size);
        }

        /// <summary>
        /// //获取鼠标所在的位置并提取图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>鼠标数据，X、Y从中提取
        private void OriPicBox_MouseMove(object sender, MouseEventArgs e)
        {
            //无图片时不提取
            if(origin_Pic==null)
            {
                return;
            }
            Mouse_X = e.X;
            Mouse_Y = e.Y;
            //根据鼠标位置反推计算原图上对应的位置，并根据放大倍数来确定对应矩形的左上角顶点，
            float target_X = e.X / nPercentW - magniBox.Width / (2 * (magnivalue.Value/10));
            float target_Y = e.Y / nPercentH - magniBox.Height / (2 * (magnivalue.Value/10));

            #region 提取根据放大倍数得到的区域图片
            //建立新的图片
            Bitmap b = new Bitmap(magniBox.Width/ (magnivalue.Value/10), magniBox.Height / (magnivalue.Value/10));
            //建立画布，在画布上建立图形
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);

            g.InterpolationMode = InterpolationMode.HighQualityBilinear;

            RectangleF srcRect = new RectangleF(target_X, target_Y, target_X+ magniBox.Width / (magnivalue.Value/10),
                target_Y+ magniBox.Height / (magnivalue.Value/10));//显示图像那一部分
            GraphicsUnit units = GraphicsUnit.Pixel;//源矩形的度量单位设置为像素
            //绘制图像，可用的方法有很多
            g.DrawImage(origin_Pic.GetImage, 0,0, srcRect, units);
            //释放内存
            g.Dispose();
            GC.Collect();
            #endregion

            //将整个区域的图调整大小
            magniBox.Image = PicFunction.resizeImage((System.Drawing.Image)b,magniBox.Size);
        }

        /// <summary>
        /// 版权说明
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Copyrightbutton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 帮助说明
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelpButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 读取图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadButton_Click(object sender, EventArgs e)
        {
            #region 读取图片
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "图像文件|*.jpg;*.bmp;*.png;*.jpeg";
            openFileDialog1.Title = "打开图像";
            if(openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                //以前用的Image img,然后origin_Image=img
                //并在最后img.dipose释放后，因为指向的同一个对象，此时origin_Image也没有东西了
                //为了提高内存使用率，就不再单独声明一个Image，如果一定要生命的话，就必须使用Clone
                //Image img = (Image)origin_Image.Clone();
                origin_Pic = new PicProperty(openFileDialog1.FileName, Image.FromFile(openFileDialog1.FileName, true));
                oriPicBox.Image = PicFunction.resizeImage(origin_Pic.GetImage, oriPicBox.Size);
                //计算宽度的缩放比例
                nPercentW = oriPicBox.Width / (float)origin_Pic.Width;
                //计算高度的缩放比例
                nPercentH = oriPicBox.Height / (float)origin_Pic.Height;
                //显示数据
                PicPropertyGrid.SelectedObject = origin_Pic;
            }
            #endregion
        }

        /// <summary>
        /// 实现程序左下角的时间同步刷新
        /// </summary>
        void TimeStart()
        {
            while (true)
            {
                //避免线程刷新太多
                ShowTime.Text = DateTime.Now.ToLocalTime().ToString();
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
