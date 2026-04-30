using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfAppTest
{
    public partial class MainWindow : Window
    {
        private double labelX = 0;
        private double labelY = 0;
        private double dx = 50;
        private double dy = 50;
        private Random rand = new Random();
        private DispatcherTimer timer;

        // 记录上一个点
        private double? lastX = null;
        private double? lastY = null;

        public MainWindow()
        {
            InitializeComponent();

            labelX = 0;
            labelY = 0;
            MoveLabel();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // 记录当前位置
            double prevX = labelX;
            double prevY = labelY;

            // 随机方向
            double angle = rand.NextDouble() * 2 * Math.PI;
            dx = 50 * Math.Cos(angle);
            dy = 50 * Math.Sin(angle);

            labelX += dx;
            labelY += dy;

            double maxX = this.ActualWidth - movingLabel.ActualWidth - 40;
            double maxY = this.ActualHeight - movingLabel.ActualHeight - 60;

            if (labelX < 0)
            {
                labelX = 0;
                dx = -dx;
                labelX += dx;
            }
            else if (labelX > maxX)
            {
                labelX = maxX;
                dx = -dx;
                labelX += dx;
            }
            if (labelY < 0)
            {
                labelY = 0;
                dy = -dy;
                labelY += dy;
            }
            else if (labelY > maxY)
            {
                labelY = maxY;
                dy = -dy;
                labelY += dy;
            }

            if (labelX < 0) labelX = 0;
            if (labelX > maxX) labelX = maxX;
            if (labelY < 0) labelY = 0;
            if (labelY > maxY) labelY = maxY;

            // 绘制轨迹
            DrawTrail(prevX, prevY, labelX, labelY);

            MoveLabel();
        }

        private void DrawTrail(double prevX, double prevY, double currX, double currY)
        {
            // 圆心坐标为标签左上角+标签宽高一半
            double centerX = prevX + movingLabel.ActualWidth / 2;
            double centerY = prevY + movingLabel.ActualHeight / 2;

            // 画圆
            Ellipse dot = new Ellipse
            {
                Width = 4,
                Height = 4,
                Fill = Brushes.Red
            };
            Canvas.SetLeft(dot, centerX - 2);
            Canvas.SetTop(dot, centerY - 2);
            mainCanvas.Children.Add(dot);

            // 画线
            if (lastX.HasValue && lastY.HasValue)
            {
                Line line = new Line
                {
                    X1 = lastX.Value,
                    Y1 = lastY.Value,
                    X2 = centerX,
                    Y2 = centerY,
                    Stroke = Brushes.Red,
                    StrokeThickness = 1
                };
                mainCanvas.Children.Add(line);
            }

            lastX = centerX;
            lastY = centerY;
        }

        private void MoveLabel()
        {
            Canvas.SetLeft(movingLabel, labelX);
            Canvas.SetTop(movingLabel, labelY);
        }
    }
}
