/****************************************
FILE NAME: som cluster
VERSION: Ver_1.0  
PROGRAMER:공영재
PROJECT: som cluser
GROUP: https://hunter95001.github.io/crystalfox.github.io/
CLASS: 
        2Layer Fuzzy SOM

EXPLANATION:     
        Self Organizing Map [통칭 SOM]
       
        Helsinki Technology University 의 Teuvo Kohonen에 의해 제안
        Self-Organizing(자기조직화)란 자기 스스로 학습할 수 있는 능력을 Map으로 형성

        특징
        사람의 대뇌피질의 감각기능을 모델로 만든 인공지능 기법 
        고차원 데이터를 축소하여 가시화 하는 방법
        SOM의 경우 비정형적인 데이터에 대해서 좋은 성능을 보여준다

        한계
        학습률과 이웃 반경 크기를 정적 생성
        최종 연결 강도는 입력 데이터 열에 종속적
        가중치 초기값에 크게 의존
        초기 학습률과 학습률의 감소비율은 직관적으로 설정

        Fuzzy
        특징
        퍼지 집합 이론은 학습된 정보로 인간이 이해할 수 있는 형태로 제시할 수 있는 신경 퍼지 시스템
        퍼지 멤버쉽 값은 퍼지 c-평균 알고리즘으로 계산된다. 
        퍼지 멤버쉽 값을 통해서 승자노드 선출

        Step #1 퍼지 집합의 중심 초기화
        Step #2 이웃 반경 크기 , 맵, 학습률 초기 설정
        Step #3 Update center of fuzzy sets
        Step #4 Calculate output 
        Step #5 Update center of Fuzzy Sets
        Step #6 Update output of the winner rule
        Step #7 Test stop condition

****************************************/
using som_cluster.Main;
using som_cluster.Processing.AI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace som_cluster
{
    public partial class Form1 : Form
    {
        Bitmap OriginalBitmap;      //원본사진
        Bitmap obser;               //결과사진
        Bitmap bitmap;              //Layer1
        Bitmap bitmpaLayer;         //Layer2

        public Form1()
        {
            InitializeComponent();
        }
        private void RunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                bitmap = new Bitmap(OriginalBitmap);

                Fuzzsom fuzzsom = new Fuzzsom(bitmap);
                fuzzsom.Run();
                fuzzsom.Layer2();
                fuzzsom.Layer_Run();
                bitmap = fuzzsom.getBitmap(); //단층
                bitmpaLayer = fuzzsom.getLayer_Bitmap(); //다층

                Labeling label = new Labeling(bitmap);
                label.AIRun();
                obser = label.GetBitmap();

                #region 원본 사진에 라벨링 사진 넣는 구문
                for (int x = 0; x < bitmap.Width; x++)
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        if (obser.GetPixel(x, y) == Color.FromArgb(255, 0, 0))
                        {
                            obser.SetPixel(x, y, Color.FromArgb(255, 0, 0));
                        }
                        else
                        {
                            obser.SetPixel(x, y, OriginalBitmap.GetPixel(x, y));
                        }
                    }
                #endregion

                //Part #3 이미지 View, Lookup Table 
                pictureBox1.Image = OriginalBitmap;
                pictureBox2.Image = obser;
                pictureBox3.Image = bitmap;
                pictureBox4.Image = bitmpaLayer;
            }
            catch (Exception)
            {
                MessageBox.Show("open에서 이미지를 선택하세요");
            }

        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name = "All Files(*.*)|*.*|Bitmap File(*.bmp)|*.bmp|";
            name = name + "Gif File(*.gif)|*.gif|jpeg File(*.jpg)|*.jpg";
            ofdOpen.Title = "타이틀";
            ofdOpen.Filter = name;
            if (ofdOpen.ShowDialog() == DialogResult.OK)
            {
                string strName = ofdOpen.FileName;
                OriginalBitmap = new Bitmap(Image.FromFile(strName));
            }
            pictureBox1.Image = OriginalBitmap;
        }

        #region Save 저장하기
        private void saveFile(Image image)
        {
            string savestrFilename;

            saveFileDialog1.Title = "이미지 파일저장...";
            saveFileDialog1.OverwritePrompt = true;
            saveFileDialog1.Filter = "JPEG File(*.jpg)|*.jpg|Bitmap File(*.bmp)|*.bmp";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                savestrFilename = saveFileDialog1.FileName;
                image.Save(savestrFilename);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile(pictureBox1.Image);
        }
        private void pictureBox2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile(pictureBox2.Image);
        }
        private void pictureBox3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile(pictureBox3.Image);
        }
        private void pictureBox4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile(pictureBox4.Image);
        }
        #endregion
    }
}
