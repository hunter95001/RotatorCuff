/****************************************
PROGRAMER:공영재
PROJECT: ConvexHull
GROUP: https://hunter95001.github.io/crystalfox.github.io/
EXPLANATION: 

ConvexHull
외각의 점들을 이어서 도형을 만드는 알고리즘

A quick-sort like algorithm for convex hull
x점 좌표를 기준으로 소팅이 되어있습니다
0번 배열이 가장 낮은 x값으로 ~배열의 마지막 번호가 x축 좌표가 가장 큽니다.
가장 좌측의 상단과 가장 우측의 하단을 연결한 후 
y축의 가장 높은곳과 가장 낮은 곳을 기준으로 삼각형을 그려줍니다
convex hull의 조건상 삼각형의 영역에 있는 좌표들은 convex hull의 조건에 만족하지 못합니다.

****************************************/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST
{
    class ConvexHull
    {
        Bitmap ObserveBitmap; //보여줄 비트맵 변수
        private const int colornum = 255; //선의 색상

        Graphics g; //선을 이을때 사용할 Graphics 객체
        Queue<A> queue; //큐 타입
        Pen P; //Pen 컬러 색
        A pre; //A컬렉션 타입
        Point StartPoint; //시작좌표
        Point nextpoint; //다음좌표
        Point topPoint;     //최고점
        int[] pointx; //x점
        int[] pointy; //y점
        int Bottomright = 0, Leftmost = 0, maxpoint = 0, minpoint = 0, rowpoint = 0;
        //  하단우측        좌측상단          최상(단가장 높은점)      최하단(가장 낮은점)

        int size = 0; //탐색으로 나온 점의 좌표개수
        int queuecount = 0; //큐배열의 크기.
        double angnum; //각도 변수
        double min;

        public ConvexHull(Bitmap bitmap)
        {

            Console.WriteLine("컨백스 시작");
            this.ObserveBitmap = new Bitmap(bitmap);
            g = Graphics.FromImage(ObserveBitmap);
            P = new Pen(Color.FromArgb(255, 255, 255));
            SavePoint(ObserveBitmap); //원본사진에서 colornum의 색상점을 찾는 역할을 합니다.
            QuickHull();
            Paint();
        }

        #region QuickHull ConvexHull의 성능을 개선시킨것.

        public class A
        {
            //queue에 넣을 컬렉션 타입 
            //점의 좌표와
            //각도를 저장합니다
            Point p;
            double angle;
            public A(Point p, double angle) { this.p = p; this.angle = angle; }
            public double Getangle() { return angle; }
            public Point GetPoint() { return p; }
        }

        private void QuickHull()
        {
            queue = new Queue<A>(); //queue변수를 할당 받음
            StartPoint = new Point(pointx[0], pointy[0]);
            //x축 좌표를기준으로 소팅되어있기 때문에 0,0이 좌측 최하단 좌표임

            Quickinit();
            Console.WriteLine("퀵이닛 끝");
            //Step #1 Left Bottom [좌측 최상단점]에서 Right Bottom [우측 최상단점]을 탐색합니다.
            //반시계방향 - 제일 좌측 좌표 [pointx[0],pointy[0]]입니다 
            //제일 우측좌표에서는 더이상 반시 계방향으로 이동할수가 없습니다.
            //제일 우측 좌표를 탐색할떄까지 선을 이어줍니다.
            CCWinit();
            while (nextpoint != new Point(pointx[size - 1], Bottomright))
            {
                CCW(); //반시계 방향
            }

            //Step #2 Right Bottom [우측 최상단점]에서 Top Point [높이가 가장 낮은점]을 탐색합니다
            //반시계방향 - [우측 하단점] 시작 합니다.
            //초기좌표에서는 더이상 시계방향으로 이동할수가 없습니다.
            //초기좌표를 탐색할떄까지 선을 이어줍니다.
            CCWTopInit();
            while (nextpoint.Y != rowpoint)
            {
                CCWTop();
            }

            //Step #3 Left Bottom [좌측 최하단점]에서 Top Point [높이가 가장 낮은점]을 탐색합니다
            //시계방향 - 제일 좌측 좌표 [pointx[0],pointy[0]]입니다 
            //제일 높은점 좌표를 탐색할떄까지 선을 이어줍니다.
            topPoint = StartPoint;
            CWInit();

            while (nextpoint != topPoint)
            {
                CW();
            }

        }

        private void Quickinit()
        {
            Bottomright = 10000; Leftmost = 0; maxpoint = 0; minpoint = 0;
            double min = 10000, max = 0;
            for (int i = 0; i < size; i++)
            {
                if (pointx[0] == pointx[i])
                {
                    Leftmost = pointy[i];
                }

                if (pointx[size - 1] == pointx[i] && Bottomright > pointy[i])
                {
                    Bottomright = pointy[i];

                }

                if (pointy[i] < min)
                {
                    min = pointy[i];
                    minpoint = i;
                    rowpoint = pointy[i];
                }

                if (pointy[i] > max)
                {
                    max = pointy[i];
                    maxpoint = i;
                }
            }

            //삼각형을 만들어 줍니다
            Triangle(new Point(pointx[0], Leftmost),
                     new Point(pointx[maxpoint], pointy[maxpoint]),
                     new Point(pointx[size - 1], Bottomright));

            Triangle(new Point(pointx[0], Leftmost),
                     new Point(pointx[minpoint], pointy[minpoint]),
                     new Point(pointx[size - 1], Bottomright));

            //두 삼각형 안에 들어가는 점들은 convex hull의 범위에 들어가지 않기 때문에 점을 지워줍니다.
            //Console.WriteLine(pointx[maxpoint] + " " + pointy[maxpoint] + " " + pointx[minpoint] + " " + pointy[minpoint]);
            //Console.WriteLine("하단우측 " + Bottomright + " 좌측상단 " + Leftmost + " 최상(단가장 높은점) " + maxpoint + " 최하단(가장 낮은점) " + minpoint);
            SavePoint(ObserveBitmap);
        }

        #region Step#1 CCW Right Bottom 반시계방향 
        private void CCWinit()
        {
            angnum = 0; //각도값을 0으로 초기화
            for (int i = 0; i < size; i++)
            {
                angnum = Angle(new Point(pointx[0], pointy[0]), new Point(pointx[i], pointy[i]));//각도계산
                if (angnum > 0)//시계방향이기 때문에 각도값이 양수
                    queue.Enqueue(new A(new Point(pointx[i], pointy[i]), angnum)); //값 저장
            }
            min = 10000;
            queuecount = queue.Count;//for문안에 적어줄 경우 큐 값이 dequeue되면서 예상치못한 에러를 유발함
            for (int i = 0; i < queuecount; i++) //큐에 잇는 배열만큼 카운팅함. 
            {
                pre = queue.Dequeue(); //큐에서 빼옴
                if (0 < pre.Getangle() && pre.Getangle() < 180) //180의 좌표선상에서 빼옴 <-360도중에 180~360는 음수이기때문
                {
                    if (min > pre.Getangle()) //최소각을 구함
                    {
                        min = pre.Getangle(); // 최소각 저장
                        nextpoint = pre.GetPoint(); //최소좌표 저장
                    }
                    queue.Enqueue(new A(pre.GetPoint(), pre.Getangle())); //양수값에 대해서 큐에 저장
                }

            }

            g.DrawLine(P, new Point(pointx[0], pointy[0]), nextpoint); //선그림
            StartPoint = nextpoint;//시작좌표를 그렸던 좌표로 저장합니다.
        }
        private void CCW()
        {
            //반시계방향.
            angnum = 0; //각도 초기화
            queuecount = queue.Count;//for문안에 적어줄 경우 큐 값이 dequeue되면서 예상치못한 에러를 유발함
            for (int i = 0; i < queuecount; i++) //queue의 배열크기만큼 시작
            {
                pre = queue.Dequeue();
                angnum = Angle(StartPoint, pre.GetPoint()); //시작 좌표에서 queue의 좌표들의 각을 구함

                if (angnum > 0) //반시계방향은 양수값을 저장함
                    queue.Enqueue(new A(pre.GetPoint(), angnum)); //큐에 저장
            }
            min = 10000;
            queuecount = queue.Count;//for문안에 적어줄 경우 큐 값이 dequeue되면서 예상치못한 에러를 유발함
            for (int i = 0; i < queuecount; i++) //queue의 크기만큼 시작
            {
                pre = queue.Dequeue(); //배열값 가져옴
                //Console.WriteLine("각도" + pre.Getangle() + " 좌표 " + pre.GetPoint());
                if (0 < pre.Getangle() && pre.Getangle() < 360)//180의 좌표선상에서 빼옴 <-360도중에 180~360는 음수이기때문
                {
                    if (min > pre.Getangle())//최소각을 구함
                    {
                        min = pre.Getangle(); //최소각을 저장
                        nextpoint = pre.GetPoint(); //최소좌표 저장

                    }
                    queue.Enqueue(new A(pre.GetPoint(), pre.Getangle()));//양수값에 대해서 큐에 저장
                }

            }
            g.DrawLine(P, StartPoint, nextpoint); //선그림
            StartPoint = nextpoint;//시작좌표를 그렸던 좌표로 저장합니다.
        }


        #endregion

        #region Step#2 CCW TOP 반시계방향
        private void CCWTopInit()
        {
            angnum = 0;//각도 변수를 =0으로 초기화
            for (int i = 0; i < size; i++)
            {
                angnum = Angle(new Point(pointx[size - 1], Bottomright), new Point(pointx[i], pointy[i]));
                //Console.WriteLine("각도 값" + angnum+" ("+ pointx[i] + ","+ pointy[i] + ")");
                if (angnum < 0) //반시계방향이라서 음수값을 가지고옴
                    queue.Enqueue(new A(new Point(pointx[i], pointy[i]), angnum));
            }

            min = 10000; //소팅하기위해 큰 값을 가지고잇음
            queuecount = queue.Count;
            for (int i = 0; i < queuecount; i++)
            {
                pre = queue.Dequeue(); //값을 가지고옴.
                if (-360 < pre.Getangle() && pre.Getangle() < 0) //각도값이 -180~90사이
                {

                    if (min > pre.Getangle())
                    {
                        min = pre.Getangle();
                        nextpoint = pre.GetPoint();

                    }
                    queue.Enqueue(new A(pre.GetPoint(), pre.Getangle())); //queue에 저장
                }
            }
            g.DrawLine(P, new Point(pointx[size - 1], Bottomright), nextpoint);//선을 그림
            StartPoint = nextpoint;//시작 좌표를 다음 좌표로 이동

        }

        private void CCWTop()
        {
            //시계방향.
            angnum = 0;
            queuecount = queue.Count; //for문안에 적어줄 경우 큐 값이 dequeue되면서 예상치못한 에러를 유발함
            for (int i = 0; i < queuecount; i++)
            {
                pre = queue.Dequeue();
                angnum = Angle(StartPoint, pre.GetPoint());
                if (angnum < 0)
                    queue.Enqueue(new A(pre.GetPoint(), angnum));
            }

            min = 10000;
            queuecount = queue.Count;//for문안에 적어줄 경우 큐 값이 dequeue되면서 예상치못한 에러를 유발함
            for (int i = 0; i < queuecount; i++)
            {
                pre = queue.Dequeue();
                if (-180 < pre.Getangle() && pre.Getangle() < -90)
                {
                    if (min > pre.Getangle())
                    {
                        min = pre.Getangle();
                        nextpoint = pre.GetPoint();
                    }
                    queue.Enqueue(new A(pre.GetPoint(), pre.Getangle()));
                }
            }
            if (min == 10000) //queue가 비었을경우
                nextpoint = new Point(pointx[0], Leftmost);//다음좌표를 시작점으로 바꾸고

            g.DrawLine(P, StartPoint, nextpoint); //선그림  
            StartPoint = nextpoint;

        }
        #endregion

        #region Step#3 CW 시계 방향
        private void CWInit()
        {

            angnum = 0; //각도값을 0으로 초기화
            for (int i = 0; i < size; i++)
            {
                angnum = Angle(new Point(pointx[0], pointy[0]), new Point(pointx[i], pointy[i]));//각도계산
                if (angnum > 0)//시계방향이기 때문에 각도값이 양수
                    queue.Enqueue(new A(new Point(pointx[i], pointy[i]), angnum)); //값 저장
            }
            min = 0;
            queuecount = queue.Count;//for문안에 적어줄 경우 큐 값이 dequeue되면서 예상치못한 에러를 유발함
            for (int i = 0; i < queuecount; i++) //큐에 잇는 배열만큼 카운팅함. 
            {
                pre = queue.Dequeue(); //큐에서 빼옴

                if (90 < pre.Getangle() && pre.Getangle() < 180) //180의 좌표선상에서 빼옴 <-360도중에 180~360는 음수이기때문
                {
                    if (min < pre.Getangle()) //최소각을 구함
                    {
                        min = pre.Getangle(); // 최소각 저장
                        nextpoint = pre.GetPoint(); //최소좌표 저장
                    }
                    queue.Enqueue(new A(pre.GetPoint(), pre.Getangle())); //양수값에 대해서 큐에 저장
                }

            }
            g.DrawLine(P, new Point(pointx[0], pointy[0]), nextpoint); //선그림
            StartPoint = nextpoint;//시작좌표를 그렸던 좌표로 저장합니다.
        }
        private void CW()
        {
            //반시계방향.
            angnum = 0; //각도 초기화
            queuecount = queue.Count;//for문안에 적어줄 경우 큐 값이 dequeue되면서 예상치못한 에러를 유발함
            for (int i = 0; i < queuecount; i++) //queue의 배열크기만큼 시작
            {
                pre = queue.Dequeue();
                angnum = Angle(StartPoint, pre.GetPoint()); //시작 좌표에서 queue의 좌표들의 각을 구함

                if (angnum > 0) //반시계방향은 양수값을 저장함
                    queue.Enqueue(new A(pre.GetPoint(), angnum)); //큐에 저장
            }
            min = 0;
            queuecount = queue.Count;//for문안에 적어줄 경우 큐 값이 dequeue되면서 예상치못한 에러를 유발함
            for (int i = 0; i < queuecount; i++) //큐에 잇는 배열만큼 카운팅함. 
            {
                pre = queue.Dequeue(); //큐에서 빼옴
                if (89 < pre.Getangle() && pre.Getangle() < 180) //y축의 높이가 같을때 x축이 다면 90도로 이어줘야 하기 때문에
                {
                    if (min < pre.Getangle()) //최소각을 구함
                    {
                        min = pre.Getangle(); // 최소각 저장
                        nextpoint = pre.GetPoint(); //최소좌표 저장

                    }
                    queue.Enqueue(new A(pre.GetPoint(), pre.Getangle())); //양수값에 대해서 큐에 저장
                }

            }
            g.DrawLine(P, StartPoint, nextpoint); //선그림
            StartPoint = nextpoint;//시작좌표를 그렸던 좌표로 저장합니다.
        }
        #endregion

        #region 각도구함.
        private double Angle(Point Start, Point End)
        {
            double ang = 180 * Math.Atan2(End.X - Start.X, End.Y - Start.Y) / Math.PI;
            return ang;
        }
        #endregion

        #region 삼각형 그리기
        private void Triangle(Point StartPoint, Point MidPoint, Point EndPoint)
        {
            PointF[] points = { StartPoint, MidPoint, EndPoint }; //3점을 배열로 받음
            g.FillPolygon(new SolidBrush(Color.FromArgb(0, 0, 0)), points);
            //FillPolygon= 배열에있는 점들을 전부 색칠함

            /*
            삼각형안에 들어있는 점들은 전부 탐색에 제외되기 때문에 검은색으로 색칠
            밑에 3개는 절대 지우면 안됨 탐색 할때 점이 사라지기 때문에 탐색할 점이 없어 무한루프에 빠지게 됩니다.
            그러기 때문에 점을 다시 찍어줘야 합니다. 
            [점의 색상은 탐색하는 색상으로 바꿔 줘야합니다]
            ex 검정색 탐색 -> 0,0,0 빨간색 탐색 -> 255,0,0
            */
            ObserveBitmap.SetPixel(StartPoint.X, StartPoint.Y, Color.FromArgb(255, 255, 255));
            ObserveBitmap.SetPixel(MidPoint.X, MidPoint.Y, Color.FromArgb(255, 255, 255));
            ObserveBitmap.SetPixel(EndPoint.X, EndPoint.Y, Color.FromArgb(255, 255, 255));

        }
        #endregion

        #region 좌표 저장
        private void SavePoint(Bitmap SeeBitmap)
        {
            for (int x = 0; x < SeeBitmap.Width; x++)
                for (int y = 0; y < SeeBitmap.Height; y++)
                    if (SeeBitmap.GetPixel(x, y).R == 255)
                        size++;

            Point[] Points = new Point[size + 1];
            pointx = new int[size + 1];
            pointy = new int[size + 1];
            size = 0;
            for (int x = 0; x < SeeBitmap.Width; x++)
                for (int y = 0; y < SeeBitmap.Height; y++)
                    if (SeeBitmap.GetPixel(x, y).R == 255)
                    {
                        Points[size] = new Point(x, y);
                        size++;
                    }

            for (int i = 0; i < size; i++)
            {
                pointx[i] = Points[i].X;
                pointy[i] = Points[i].Y;
            }
        }
        #endregion

        #endregion

        #region Paint 내부 모형을 색칠 해줍니다.
        /*
         선이 이어진 곳을 찾아서 색칠해주는 함수 
         초록색으로 선을 이어주기 때문에 .G가 255일 경우 색칠함.
        */
        private void Paint()
        {
            Point startpt = new Point(0, 0);
            Point endpt = new Point(0, 0);

            for (int y = 0; y < ObserveBitmap.Height; y++)
            {
                //왼쪽에서 좌표를 읽어오는 for문
                for (int x = 0; x < ObserveBitmap.Width; x++)
                    if (ObserveBitmap.GetPixel(x, y).G == colornum)
                    {
                        startpt = new Point(x, y);
                        break;
                    }

                //오른쪽에서 좌표를 읽어오는 for문
                for (int x = ObserveBitmap.Width - 1; x >= 0; x--)
                    if (ObserveBitmap.GetPixel(x, y).G == colornum)
                    {
                        endpt = new Point(x, y);
                        break;
                    }

                //색칠
                if (startpt != endpt)
                {
                    g.DrawLine(P, startpt, endpt);
                }
            }
        }
        #endregion

        public Bitmap GetBitmap()
        {
            return ObserveBitmap;
        }
    }
}