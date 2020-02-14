using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TEST
{
    public partial class Form1 : Form
    {
        Bitmap orginal = new Bitmap("00.ROI.JPG");
        Bitmap bitmap;
        Bitmap obs;
        public Form1()
        {
            InitializeComponent();
        }

        private void RUNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConvexHull convexHull = new ConvexHull(bitmap); //그래픽 객체를 사용하기 때문에 Bitmap을 사용해야함.
            bitmap = convexHull.GetBitmap();
            obs = new Bitmap(bitmap.Width,bitmap.Height);
            for (int x = 0; x < bitmap.Width; x++)
                for (int y = 0; y < bitmap.Height; y++)
                {
                    if (bitmap.GetPixel(x, y).R == 255)
                    {
                        obs.SetPixel(x, y, orginal.GetPixel(x, y));
                    }
                    else {
                        obs.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                    }

                }
            
            pictureBox2.Image = obs;
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image image = null;
            string name = "All Files(*.*)|*.*|Bitmap File(*.bmp)|*.bmp|";
            name = name + "Gif File(*.gif)|*.gif|jpeg File(*.jpg)|*.jpg";
            openFileDialog1.Title = "타이틀";
            openFileDialog1.Filter = name;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string strName = openFileDialog1.FileName;
                image = Image.FromFile(strName);
            }
            pictureBox1.Image = image;
            bitmap = new Bitmap(pictureBox1.Image);
            for (int x = 0; x < bitmap.Width; x++)
                for (int y = 0; y < bitmap.Height; y++)
                {
                    int R = bitmap.GetPixel(x, y).R < 125 ? 0 : 255;
                    bitmap.SetPixel(x, y, Color.FromArgb(R, R, R));
                }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.Image.Save("C:\\Users\\KYJ\\Desktop\\100.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        private void EndToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
