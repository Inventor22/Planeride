using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Drawing
{

    public class PointTransformer
    {
        public Point EndPoint;
        public Point Home;
        private int step;

        public virtual void Update(int step)
        {
            this.step = step;
            EndPoint = Home;
        }

        public virtual void Update(Point startPoint, int step)
        {
            this.step = step;
            this.EndPoint = startPoint;
        }

        public PointTransformer(Point home)
        {
            this.Home = home;
            this.EndPoint = home;
        }
    }

    public class CircularTransform : PointTransformer
    {
        private int radius;
        private int stepFactor;

        public void UpdateStepFactor(int stepFactor)
        {
            this.stepFactor = stepFactor;
        }

        public CircularTransform(Point home, int radius, int stepFactor) 
            : base(home)
        {
            this.radius = radius;
            this.stepFactor = stepFactor;
        }

        public override void Update(Point startPoint, int step)
        {
            EndPoint = new Point(
                (int)(this.Home.X + this.radius * Math.Cos(step * this.stepFactor * Math.PI / (double)180)),
                (int)(this.Home.Y + this.radius * Math.Sin(step * this.stepFactor * Math.PI / (double)180))
                );
        }

        public override void Update(int step)
        {
            this.Update(this.Home, step);
        }
    }
    public partial class Form1 : Form
    {
        private List<Point> pointList;
        private Pen pen;

        List<PointTransformer> pointTransformers = new List<PointTransformer>();

        public Form1()
        {
            InitializeComponent();

            pen = new Pen(Color.Crimson);

            pointList = new List<Point>();

            home = new Point(this.Width/2, this.Height/2);

            pointTransformers.Add(new CircularTransform(home, 200, 5));
            pointTransformers.Add(new CircularTransform(home, 100, 10));

            timer1.Interval = 250;
            timer1.Tick += Timer1OnTick;
            timer1.Enabled = true;
            timer1.Start();
        }

        private int angle0 = 0;
        private int angle1 = 0;

        private int angle0Velocity = 10;
        private int angle1Velocity = 20;

        private int radius0 = 100;
        private int radius1 = 100;

        private Point home;

        private int step = 0;

        private void Timer1OnTick(object sender, EventArgs eventArgs)
        {
            foreach (var pointTransformer in pointTransformers)
            {
                pointTransformer.Update(step++);
                pointList.Add(new Point(pointTransformer.EndPoint.X, pointTransformer.EndPoint.Y));
            }

            //angle0 += angle0Velocity;
            //angle1 += angle1Velocity;

            //Point p0 = new Point(
            //    (int)(home.X + radius0*Math.Cos(angle0 * Math.PI / (double)180)),
            //    (int)(home.Y + radius0*Math.Sin(angle0 * Math.PI / (double)180))
            //    );
            
            //Point p1 = new Point(
            //    (int)(home.X + radius1 * Math.Cos(angle1 * Math.PI / (double)180)),
            //    (int)(home.Y + radius1 * Math.Sin(angle1 * Math.PI / (double)180))
            //    );

            //pointList.Add(p0);
            //pointList.Add(p1);

            // Force redraw
            this.Canvas.Invalidate();
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = this.Canvas.CreateGraphics();
            if (pointList.Count >= 2)
            {
                g.DrawLines(pen, pointList.ToArray());
            }
        }
    }
}
