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
        public Form1()
        {
            InitializeComponent();
        }

        private void RUNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(pictureBox1.Image);
            Binarization binarization = new Binarization(bitmap);
            //bitmap = binarization.Gonzalez_Horizontal_Vertical();
            //bitmap = binarization.Gonzalez();
            //bitmap = binarization.Vertical();
            //bitmap = binarization.Horizontal();
            pictureBox2.Image = bitmap;
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
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.Image.Save("C:\\Users\\KYJ\\Desktop\\TEST\\100.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        private void EndToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
