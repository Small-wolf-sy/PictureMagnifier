using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            Thread timeshow=new Thread(TimeStart);
            timeshow.Start();
            ButtonEvent();
        }

        #region 相关内部属性
        Image origin_Image;//原始图像，即使图片在软件内显示时效果较差，也要保证放大的效果无误
        #endregion 

        /// <summary>
        /// 存储所有button事件的声明
        /// </summary>
        void ButtonEvent()
        {
            ReadButton.Click += ReadButton_Click;
            HelpButton.Click += HelpButton_Click;
            copyrightbutton.Click += Copyrightbutton_Click;
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
                Image img = Image.FromFile(openFileDialog1.FileName, true);
                origin_Image = img;
                oriPicBox.Image = PicFunction.resizeImage(img, oriPicBox.Size);
                img.Dispose();
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
