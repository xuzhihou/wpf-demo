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

        private double? lastX = null;
        private double? lastY = null;

        // 按钮相关变量
        private double btnX, btnY, btnDx = -50, btnDy = -50;
        private double? btnLastX = null, btnLastY = null;

        private DispatcherTimer greenDotTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            labelX = 0;
            labelY = 0;
            MoveLabel();

            // 按钮初始位置在右下角
            mainCanvas.Loaded += (s, e) =>
            {
                btnX = mainCanvas.ActualWidth - movingButton.Width;
                btnY = mainCanvas.ActualHeight - movingButton.Height;
                MoveButton();
            };

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            // 绿色圆点定时器
            greenDotTimer.Interval = TimeSpan.FromSeconds(1);
            greenDotTimer.Tick += GreenDotTimer_Tick;
            greenDotTimer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // 标签轨迹
            double prevX = labelX;
            double prevY = labelY;

            double angle = rand.NextDouble() * 2 * Math.PI;
            dx = 50 * Math.Cos(angle);
            dy = 50 * Math.Sin(angle);

            labelX += dx;
            labelY += dy;

            double maxX = mainCanvas.ActualWidth - movingLabel.ActualWidth;
            double maxY = mainCanvas.ActualHeight - movingLabel.ActualHeight;

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

            DrawTrail(prevX, prevY, labelX, labelY, movingLabel, Brushes.Red, ref lastX, ref lastY);

            MoveLabel();

            // 按钮轨迹
            double prevBtnX = btnX;
            double prevBtnY = btnY;

            double btnAngle = rand.NextDouble() * 2 * Math.PI;
            btnDx = -50 * Math.Cos(btnAngle);
            btnDy = -50 * Math.Sin(btnAngle);

            btnX += btnDx;
            btnY += btnDy;

            double maxBtnX = mainCanvas.ActualWidth - movingButton.Width;
            double maxBtnY = mainCanvas.ActualHeight - movingButton.Height;

            if (btnX < 0)
            {
                btnX = 0;
                btnDx = -btnDx;
                btnX += btnDx;
            }
            else if (btnX > maxBtnX)
            {
                btnX = maxBtnX;
                btnDx = -btnDx;
                btnX += btnDx;
            }
            if (btnY < 0)
            {
                btnY = 0;
                btnDy = -btnDy;
                btnY += btnDy;
            }
            else if (btnY > maxBtnY)
            {
                btnY = maxBtnY;
                btnDy = -btnDy;
                btnY += btnDy;
            }

            if (btnX < 0) btnX = 0;
            if (btnX > maxBtnX) btnX = maxBtnX;
            if (btnY < 0) btnY = 0;
            if (btnY > maxBtnY) btnY = maxBtnY;

            DrawTrail(prevBtnX, prevBtnY, btnX, btnY, movingButton, Brushes.Black, ref btnLastX, ref btnLastY);

            MoveButton();
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
        }

        private void DrawTrail(double prevX, double prevY, double currX, double currY, FrameworkElement element, Brush color, ref double? lastTrailX, ref double? lastTrailY)
        {
            double centerX = prevX + element.ActualWidth / 2;
            double centerY = prevY + element.ActualHeight / 2;

            Ellipse dot = new Ellipse
            {
                Width = 4,
                Height = 4,
                Fill = color
            };
            Canvas.SetLeft(dot, centerX - 2);
            Canvas.SetTop(dot, centerY - 2);
            mainCanvas.Children.Add(dot);

            if (lastTrailX.HasValue && lastTrailY.HasValue)
            {
                Line line = new Line
                {
                    X1 = lastTrailX.Value,
                    Y1 = lastTrailY.Value,
                    X2 = centerX,
                    Y2 = centerY,
                    Stroke = color,
                    StrokeThickness = 1
                };
                mainCanvas.Children.Add(line);
            }

            lastTrailX = centerX;
            lastTrailY = centerY;
        }

        private void MoveLabel()
        {
            Canvas.SetLeft(movingLabel, labelX);
            Canvas.SetTop(movingLabel, labelY);
        }

        private void MoveButton()
        {
            Canvas.SetLeft(movingButton, btnX);
            Canvas.SetTop(movingButton, btnY);
        }
    }
}
