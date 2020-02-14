using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using som_cluster.Bean;
using som_cluster.Main;

namespace som_cluster
{
   
    public partial class Form1 : Form
    {
        Bitmap bitmap;

        int testnum = 1;
        public Form1()
        {
            //Part #1 초기화 부분 
            InitializeComponent();
            //String imagePath = "C:\\Users\\KYJ\\Desktop\\Git Hub\\Som Cluster\\som cluster\\som cluster\\Image\\test.jpg";
            String imagePath = "C:\\Users\\KYJ\\Desktop\\Git Hub\\Som Cluster\\som cluster\\som cluster\\Image\\SOM.png";
            String[] testPath = new String[10];
            testPath[0] = "C:\\Users\\KYJ\\Desktop\\Git Hub\\Som Cluster\\som cluster\\som cluster\\Image\\01.png";
            testPath[1] = "C:\\Users\\KYJ\\Desktop\\Git Hub\\Som Cluster\\som cluster\\som cluster\\Image\\02.png";
            testPath[2] = "C:\\Users\\KYJ\\Desktop\\Git Hub\\Som Cluster\\som cluster\\som cluster\\Image\\03.png";
            testPath[3] = "C:\\Users\\KYJ\\Desktop\\Git Hub\\Som Cluster\\som cluster\\som cluster\\Image\\04.png";
            testPath[4] = "C:\\Users\\KYJ\\Desktop\\Git Hub\\Som Cluster\\som cluster\\som cluster\\Image\\05.png";
            testPath[5] = "C:\\Users\\KYJ\\Desktop\\Git Hub\\Som Cluster\\som cluster\\som cluster\\Image\\06.png";
            testPath[6] = "C:\\Users\\KYJ\\Desktop\\Git Hub\\Som Cluster\\som cluster\\som cluster\\Image\\07.png";
            testPath[7] = "C:\\Users\\KYJ\\Desktop\\Git Hub\\Som Cluster\\som cluster\\som cluster\\Image\\08.png";
            testPath[8] = "C:\\Users\\KYJ\\Desktop\\Git Hub\\Som Cluster\\som cluster\\som cluster\\Image\\09.png";
            testPath[9] = "C:\\Users\\KYJ\\Desktop\\Git Hub\\Som Cluster\\som cluster\\som cluster\\Image\\10.png";
            Bitmap[] testBitmap = new Bitmap[10];
            for (int i = 0; i < 10; i++)
            {
                testBitmap[i] = new Bitmap(testPath[i]);
                Console.WriteLine("R "+testBitmap[i].GetPixel(0,0).R+" G "+ testBitmap[i].GetPixel(0, 0).G +" B "+ testBitmap[i].GetPixel(0, 0).B);
            }

            bitmap = new Bitmap(imagePath);

            //Part #2 전처리 부분
            //Pretreatment prtment = new Pretreatment(new Bitmap(bitmap));
            //prtment.Gray();


            //Part #3 A.I 기법 부분.
            //AI aI = new AI(new Bitmap(bitmap));
            //aI.Kmeans();

            //AI som = new AI(new Bitmap(bitmap));
            //som.Som(testBitmap);

            //Lookup lookup = new Lookup(som.getBitmap());
            //lookup.Show();

            //pictureBox1.Image = bitmap;
            //pictureBox2.Image = aI.getBitmap();
            //pictureBox3.Image = som.getBitmap();



           
                test(9);
            
           


        }

        public void test(int i) {
            int num = i;
            String s3 = "C:\\Users\\KYJ\\Desktop\\Git Hub\\Som Cluster\\som cluster\\som cluster\\Image\\의사사진\\"+num+"doctor.jpg";
            String s2 = "C:\\Users\\KYJ\\Desktop\\Git Hub\\Som Cluster\\som cluster\\som cluster\\Image\\어깨사진\\"+num+"origin.jpg";
            String ss = "C:\\Users\\KYJ\\Desktop\\Git Hub\\Som Cluster\\som cluster\\som cluster\\Image\\회전근개 사진\\" + num + "labeling.jpg";
            Bitmap bb = new Bitmap(ss);
            Bitmap b2 = new Bitmap(s2);
            Bitmap b3 = new Bitmap(bb.Width,bb.Height);
            Bitmap b4 = new Bitmap(s3);
            List<int> ll = new List<int>();
            int max = 0;
            for (int x = 0; x < bb.Width; x++)
                for (int y = 0; y < bb.Height; y++)
                {
                    b3.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                }

            for (int x = 0; x < bb.Width; x++)
                for (int y = 0; y < bb.Height; y++) {
                    int R = bb.GetPixel(x, y).R;
                    int G = bb.GetPixel(x, y).G;
                    int B = bb.GetPixel(x, y).B;
                    int R1 = b2.GetPixel(x, y).R;
                    int G1 = b2.GetPixel(x, y).G;
                    int B1 = b2.GetPixel(x, y).B;
                    if (R > G + B &&R1<240)
                    {
                        b3.SetPixel(x, y, Color.FromArgb(R1, 0, 0));
                        max++;
                    }
                }
            Console.WriteLine(max);

            pictureBox1.Image = bb;
            pictureBox2.Image = b2;
            pictureBox3.Image = b4;
            chart1lookup(b2);
            chart2lookup(b3);

        }
     
     
    

        
        public void chart1lookup(Bitmap origanlBitmap) {
            int[] array = new int[256];
            for (int x = 0; x < origanlBitmap.Width; x++)
                    for (int y = 0; y < origanlBitmap.Height; y++)
                    {
                        int color = origanlBitmap.GetPixel(x, y).R;
                        array[color]++;

                    }


                chart1.Titles.Add("Lookup Table");
                chart1.Series.Add("Color");
                for (int i = 0; i < 255; i++)
                    chart1.Series["Color"].Points.AddXY(i.ToString(), array[i]);
            }

        public void chart2lookup(Bitmap origanlBitmap)
        {
            int[] array = new int[256];
            for (int x = 0; x < origanlBitmap.Width; x++)
                for (int y = 0; y < origanlBitmap.Height; y++)
                {
                    int color = origanlBitmap.GetPixel(x, y).R;
                    array[color]++;

                }


            chart2.Titles.Add("Lookup Table");
            chart2.Series.Add("Color");
            for (int i = 0; i < 255; i++)
                chart2.Series["Color"].Points.AddXY(i.ToString(), array[i]);
        }

        private void 캡처ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rectangle rectangle = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
            Bitmap bitmap = new Bitmap(rectangle.Width, rectangle.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(this.Left,this.Top,0,0,this.Size);
            string filename = "C:\\Users\\KYJ\\Desktop\\룩업\\"+ testnum +".jpg";
            bitmap.Save(filename);
        }
    }
}
