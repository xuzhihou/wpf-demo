using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfAppTest
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer greenDotTimer = new DispatcherTimer();
        private Random rand = new Random();

        private class GreenDot
        {
            public Ellipse Ellipse { get; set; }
            public Point Center { get; set; }
            public int Index { get; set; }
        }
        private List<GreenDot> greenDots = new List<GreenDot>();
        private List<Line> greenLines = new List<Line>();

        public MainWindow()
        {
            InitializeComponent();

            greenDotTimer.Interval = TimeSpan.FromSeconds(1);
            greenDotTimer.Tick += GreenDotTimer_Tick;
            greenDotTimer.Start();
        }

        private void GreenDotTimer_Tick(object sender, EventArgs e)
        {
            if (mainCanvas.ActualWidth < 4 || mainCanvas.ActualHeight < 4) return;

            double x = rand.NextDouble() * (mainCanvas.ActualWidth - 4);
            double y = rand.NextDouble() * (mainCanvas.ActualHeight - 4);

            Ellipse dot = new Ellipse
            {
                Width = 4,
                Height = 4,
                Fill = Brushes.Green
            };
            Canvas.SetLeft(dot, x);
            Canvas.SetTop(dot, y);
            mainCanvas.Children.Add(dot);

            var newDot = new GreenDot
            {
                Ellipse = dot,
                Center = new Point(x + 2, y + 2),
                Index = greenDots.Count
            };
            greenDots.Add(newDot);

            // 移除所有旧连线
            foreach (var line in greenLines)
            {
                mainCanvas.Children.Remove(line);
            }
            greenLines.Clear();

            // 记录每个点的连接数
            int n = greenDots.Count;
            int[] connectCount = new int[n];
            var connections = new HashSet<(int, int)>();

            // 计算所有点对距离
            var pairs = new List<(int i, int j, double dist)>();
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    double dist = Distance(greenDots[i].Center, greenDots[j].Center);
                    pairs.Add((i, j, dist));
                }
            }
            // 按距离升序排列
            pairs = pairs.OrderBy(p => p.dist).ToList();

            // 贪心地为每个点连接最近的点，且每个点最多2条线
            foreach (var (i, j, dist) in pairs)
            {
                if (connectCount[i] < 2 && connectCount[j] < 2 && !connections.Contains((i, j)))
                {
                    Line line = new Line
                    {
                        X1 = greenDots[i].Center.X,
                        Y1 = greenDots[i].Center.Y,
                        X2 = greenDots[j].Center.X,
                        Y2 = greenDots[j].Center.Y,
                        Stroke = Brushes.Green,
                        StrokeThickness = 1
                    };
                    mainCanvas.Children.Add(line);
                    greenLines.Add(line);
                    connections.Add((i, j));
                    connectCount[i]++;
                    connectCount[j]++;
                }
            }
        }

        private double Distance(Point a, Point b)
        {
            double dx = a.X - b.X;
            double dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
