using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace som_cluster.Main
{
    public partial class Lookup : Form
    {
        int[] array = new int[256];
        public Lookup(Bitmap origanlBitmap)
        {
            InitializeComponent();
            for (int x = 0; x < origanlBitmap.Width; x++)
                for (int y = 0; y < origanlBitmap.Height; y++)
                {
                    int color = origanlBitmap.GetPixel(x, y).R;
                    array[color]++;
                  
                }
            

            chart1.Titles.Add("Lookup Table");
            chart1.Series.Add("Color");
            for(int i=0; i<255; i++)
            chart1.Series["Color"].Points.AddXY(i.ToString(), array[i]);
        }
    }
}
