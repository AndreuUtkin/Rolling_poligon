using System.Drawing;
using System.Timers;
using static Катящийся_многоугольник.Program;

namespace Катящийся_многоугольник
{
    public class PolygonForm : Form
    {
        private int numberOfSides;
        private int radus;
        private int speed;
        double k;
        private System.Timers.Timer timer;
        Polygon pol;
        int nextPoi;
        float dx;
        float dy;
        float x1;
        float y1;
        double sx;
        bool left;
        int mode;
        Point[] line;
        double b;
        bool moving;

        public PolygonForm(int sides, int radus, int speed, double k, double sx, bool left)
        {

            moving = false;
            mode = 0;
            line = new Point[2];
            this.numberOfSides = sides;
            this.Text = "Катящийся n-угольник";
            // this.Width = 600;
            //this.Height = 600;
            this.k = k;
            this.radus = radus;
            this.speed = speed;
            this.MouseClick += new MouseEventHandler(Form1_MouseClick);
            this.MouseDown += new MouseEventHandler(Form1_MouseDown);
            this.MouseUp += new MouseEventHandler(Form1_MouseUp);
            this.MouseMove += new MouseEventHandler(Form1_MouseMove);
            InitializeComponent();
            pol = new Polygon(this.numberOfSides, this.radus, new Point(0, 350), this.speed);
            pol.refresh();
            MaximizeBox = false;
            //this.nextPoi = pol.maxY(line);
            //this.x1 = (float)sx;
            //this.y1 = (float)k * x1 + (float)b;

            //this.dx = x1 - pol.points[nextPoi].X;
            //this.dy = y1 - pol.points[nextPoi].Y;
            //pol.center = new Point(pol.center.X + (int)dx, pol.center.Y + (int)dy);
            //pol.refresh();
            this.DoubleBuffered = true; // Для уменьшения мерцания
            //this.Paint += new PaintEventHandler(DrawPolygon);
            timer = new System.Timers.Timer(50 - this.speed); // Изменение угла каждые 100 миллисекунд
            timer.Elapsed += OnTimedEvent;
            timer.Start();

            // Подписка на обработчик событий для клавиатуры
            this.KeyDown += new KeyEventHandler(OnKeyDown);
            this.k = k;
            this.sx = trackBar3.Value;
            this.left = left;
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (moving)
            {
                double d1 = Math.Sqrt((e.X - line[0].X) * (e.X - line[0].X) + (e.Y - line[0].Y) * (e.Y - line[0].Y));
                double d2 = Math.Sqrt((e.X - line[1].X) * (e.X - line[1].X) + (e.Y - line[1].Y) * (e.Y - line[1].Y));
                if (d1 < 5)
                {
                    line[0] = new Point(e.X, e.Y);
                    double kd = ((double)line[1].Y - (double)line[0].Y) / ((double)line[1].X - (double)line[0].X);
                    double kb = -((double)line[0].X * (double)line[1].Y - (double)line[1].X * (double)line[0].Y) / ((double)line[1].X - (double)line[0].X);
                    this.k = kd;
                    b = kb;


                }
                else if (d2 < 5)
                {
                    line[1] = new Point(e.X, e.Y);
                    double kd = ((double)line[1].Y - (double)line[0].Y) / ((double)line[1].X - (double)line[0].X);
                    double kb = -((double)line[0].X * (double)line[1].Y - (double)line[1].X * (double)line[0].Y) / ((double)line[1].X - (double)line[0].X);
                    this.k = kd;
                    b = kb;


                }
                if (k > 0)
                {
                    trackBar3.Value = (int)x1;
                    x1 = 600;
                    y1 = 50;
                    this.dx = x1 - pol.points[nextPoi].X;
                    this.dy = y1 - pol.points[nextPoi].Y;
                    pol.center = new Point(pol.center.X + (int)dx, pol.center.Y + (int)dy);
                    pol.refresh();
                    nextPoi = pol.maxY(line);
                    x1 = trackBar3.Value;
                    y1 = (float)k * x1 + (float)b;
                    this.dx = x1 - pol.points[nextPoi].X;
                    this.dy = y1 - pol.points[nextPoi].Y;
                    pol.center = new Point(pol.center.X + (int)dx, pol.center.Y + (int)dy);
                    pol.refresh();
                }
                else
                {
                    float old_x = x1;
                    x1 = 100;
                    y1 = 100;
                    this.dx = x1 - pol.points[nextPoi].X;
                    this.dy = y1 - pol.points[nextPoi].Y;
                    pol.center = new Point(pol.center.X + (int)dx, pol.center.Y + (int)dy);
                    pol.refresh();
                    nextPoi = pol.maxY(line);
                    x1 = old_x;
                    y1 = (float)k * x1 + (float)b;
                    this.dx = x1 - pol.points[nextPoi].X;
                    this.dy = y1 - pol.points[nextPoi].Y;
                    pol.center = new Point(pol.center.X + (int)dx, pol.center.Y + (int)dy);
                    pol.refresh();
                }
            }
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (mode == 2)
                {

                    moving = true;
                }
            }
        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            moving = false;
        }
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            // Проверка, была ли нажата левая кнопка мыши
            if (e.Button == MouseButtons.Left)
            {
                if (mode == 1)
                {
                    line[1] = new Point(e.X, e.Y);
                    mode = 2;
                    double kd = ((double)line[1].Y - (double)line[0].Y) / ((double)line[1].X - (double)line[0].X);
                    double kb = -((double)line[0].X * (double)line[1].Y - (double)line[1].X * (double)line[0].Y) / ((double)line[1].X - (double)line[0].X);
                    this.k = kd;
                    //numericUpDown2.Value = (decimal)kd;
                    b = kb;
                    //************
                    //this.k = (double)numericUpDown2.Value;
                    this.nextPoi = pol.maxY(line);
                    this.x1 = (float)sx;
                    this.y1 = (float)k * x1 + (float)b;
                    //InitializeComponent();
                    this.dx = x1 - pol.points[nextPoi].X;
                    this.dy = y1 - pol.points[nextPoi].Y;
                    pol.center = new Point(pol.center.X + (int)dx, pol.center.Y + (int)dy);
                    pol.refresh();

                }
                if (mode == 0)
                {
                    line[0] = new Point(e.X, e.Y);
                    mode = 1;

                }

                else
                {

                }
            }
        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            // Изменяем координаты квадрата в зависимости от нажатой клавиши
            this.Close();
        }
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            this.Invalidate(); // Перерисовываем форму
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawSquare(e.Graphics);
        }

        private void DrawSquare(Graphics g)
        {

            if (mode == 0)
            {
                g.DrawString("Нажмите ЛКМ для вбора первой точки прямой", Font, Brushes.Red, new PointF(150, 350));
                
                //g.DrawPolygon(pen, center_dot);
            }
            if (mode == 1)
            {
                g.DrawString("Нажмите ЛКМ для вбора второй точки прямой", Font, Brushes.Red, new PointF(150, 350));
                Bitmap pt2 = new Bitmap(4, 4);
                pt2.SetPixel(0, 0, Color.Black);
                pt2.SetPixel(0, 1, Color.Black);
                pt2.SetPixel(0, 2, Color.Black);
                pt2.SetPixel(1, 2, Color.Black);
                pt2.SetPixel(1, 0, Color.Black);
                pt2.SetPixel(1, 1, Color.Black);
                pt2.SetPixel(2, 0, Color.Black);
                pt2.SetPixel(2, 1, Color.Black);
                pt2.SetPixel(2, 2, Color.Black);
                g.DrawImageUnscaled(pt2, line[0].X, line[0].Y);
            }
            if (mode == 2)
            {
                if (trackBar2.Value == 0)
                {
                    pol.radius = trackBar1.Value;
                    pol.refresh();

                    if (k > 0)
                    {
                        trackBar3.Value = (int)x1;
                        x1 = 600;
                        y1 = 50;
                        this.dx = x1 - pol.points[nextPoi].X;
                        this.dy = y1 - pol.points[nextPoi].Y;
                        pol.center = new Point(pol.center.X + (int)dx, pol.center.Y + (int)dy);
                        pol.refresh();
                        nextPoi = pol.maxY(line);
                        x1 = trackBar3.Value;
                        y1 = (float)k * x1 + (float)b;
                        this.dx = x1 - pol.points[nextPoi].X;
                        this.dy = y1 - pol.points[nextPoi].Y;
                        pol.center = new Point(pol.center.X + (int)dx, pol.center.Y + (int)dy);
                        pol.refresh();
                    }
                    else
                    {
                        float old_x = x1;
                        x1 = 100;
                        y1 = 100;
                        this.dx = x1 - pol.points[nextPoi].X;
                        this.dy = y1 - pol.points[nextPoi].Y;
                        pol.center = new Point(pol.center.X + (int)dx, pol.center.Y + (int)dy);
                        pol.refresh();
                        nextPoi = pol.maxY(line);
                        x1 = old_x;
                        y1 = (float)k * x1 + (float)b;
                        this.dx = x1 - pol.points[nextPoi].X;
                        this.dy = y1 - pol.points[nextPoi].Y;
                        pol.center = new Point(pol.center.X + (int)dx, pol.center.Y + (int)dy);
                        pol.refresh();
                    }


                }
                else if (trackBar2.Value < 0)
                {

                    pol.rotation += 0.01 * trackBar2.Value;
                    pol.refresh();
                    dx = x1 - pol.points[nextPoi].X;
                    dy = y1 - pol.points[nextPoi].Y;
                    pol.center = new Point(pol.center.X + (int)dx, pol.center.Y + (int)dy);
                    pol.radius = trackBar1.Value;
                    pol.refresh();

                    int nextP2 = pol.nextP_left(nextPoi);
                    if (pol.isLower(new Point((int)pol.points[nextP2].X, (int)pol.points[nextP2].Y), this.k, (float)b))
                    {
                        nextPoi = nextP2;
                        x1 = pol.points[nextP2].X;
                        y1 = (float)b + x1 * (float)k; y1 = (float)b + x1 * (float)k;
                        // pol.points[nextP2] = new PointF((float)x1, (float)y1);
                    }
                }
                else
                {

                    pol.rotation += trackBar2.Value * 0.01;
                    pol.refresh();
                    dx = x1 - pol.points[nextPoi].X;
                    dy = y1 - pol.points[nextPoi].Y;
                    pol.center = new Point(pol.center.X + (int)dx, pol.center.Y + (int)dy);
                    pol.radius = trackBar1.Value;
                    pol.refresh();

                    int nextP2 = pol.nextP(nextPoi);
                    if (pol.isLower(new Point((int)pol.points[nextP2].X, (int)pol.points[nextP2].Y), this.k, (float)b))
                    {
                        nextPoi = nextP2;
                        x1 = pol.points[nextP2].X;
                        y1 = (float)b + x1 * (float)k;
                        //pol.points[nextP2] = new PointF((float)x1, (float)y1);
                    }
                }


                if (pol.points[0].X > 600 || pol.points[0].X < -50)
                {
                    trackBar2.Value = 0;
                }
                Pen pen = new Pen(Color.Black);
                pen.Width = 1;
                Pen dot_pen = new Pen(Color.Green);
                dot_pen.Width = 4;
                PointF[] points_line = new PointF[2];
                points_line[0] = new PointF(0, (float)b);
                points_line[1] = new PointF(1500, (float)b + 1500 * (float)this.k);
                g.DrawPolygon(dot_pen, points_line);//**********
                PointF[] center_dot = new PointF[2];
                center_dot[0] = new PointF(pol.center.X, pol.center.Y);
                center_dot[1] = new PointF(pol.center.X, pol.center.Y);
                Bitmap pt = new Bitmap(5, 5);
                pt.SetPixel(0, 0, Color.Red);
                pt.SetPixel(0, 1, Color.Red);
                pt.SetPixel(0, 2, Color.Red);
                pt.SetPixel(1, 2, Color.Red);
                pt.SetPixel(1, 0, Color.Red);
                pt.SetPixel(1, 1, Color.Red);
                pt.SetPixel(2, 0, Color.Red);
                pt.SetPixel(2, 1, Color.Red);
                pt.SetPixel(2, 2, Color.Red);
                pt.SetPixel(3, 0, Color.Red);
                pt.SetPixel(3, 1, Color.Red);
                pt.SetPixel(3, 2, Color.Red);
                pt.SetPixel(3, 3, Color.Red);
                pt.SetPixel(0, 3, Color.Red);
                pt.SetPixel(1, 3, Color.Red);
                pt.SetPixel(2, 3, Color.Red);
                
                g.FillPolygon(Brushes.Blue, pol.points);
                g.DrawPolygon(pen, pol.points);
                g.DrawImageUnscaled(pt, pol.center.X, pol.center.Y);
                //g.DrawPolygon(pen, center_dot);
                
                Bitmap pt2 = new Bitmap(4, 4);
                pt2.SetPixel(0, 0, Color.Red);
                pt2.SetPixel(0, 1, Color.Red);
                pt2.SetPixel(0, 2, Color.Red);
                pt2.SetPixel(1, 2, Color.Red);
                pt2.SetPixel(1, 0, Color.Red);
                pt2.SetPixel(1, 1, Color.Red);
                pt2.SetPixel(2, 0, Color.Red);
                pt2.SetPixel(2, 1, Color.Red);
                pt2.SetPixel(2, 2, Color.Red);
                pt2.SetPixel(3, 0, Color.Red);
                pt2.SetPixel(3, 1, Color.Red);
                pt2.SetPixel(3, 2, Color.Red);
                pt2.SetPixel(3, 3, Color.Red);
                pt2.SetPixel(0, 3, Color.Red);
                pt2.SetPixel(1, 3, Color.Red);
                pt2.SetPixel(2, 3, Color.Red);
                
                g.DrawImageUnscaled(pt2, line[0].X, line[0].Y);
                Bitmap pt3 = new Bitmap(4, 4);
                pt3.SetPixel(0, 0, Color.Red);
                pt3.SetPixel(0, 1, Color.Red);
                pt3.SetPixel(0, 2, Color.Red);
                pt3.SetPixel(1, 2, Color.Red);
                pt3.SetPixel(1, 0, Color.Red);
                pt3.SetPixel(1, 1, Color.Red);
                pt3.SetPixel(2, 0, Color.Red);
                pt3.SetPixel(2, 1, Color.Red);
                pt3.SetPixel(2, 2, Color.Red);
                g.DrawImageUnscaled(pt2, line[1].X, line[1].Y);

            }
        }

        private void InitializeComponent()
        {
            trackBar1 = new TrackBar();
            trackBar2 = new TrackBar();
            numericUpDown1 = new NumericUpDown();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            trackBar3 = new TrackBar();
            label4 = new Label();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar3).BeginInit();
            SuspendLayout();
            // 
            // trackBar1
            // 
            trackBar1.Location = new Point(671, 193);
            trackBar1.Maximum = 300;
            trackBar1.Minimum = 20;
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(156, 69);
            trackBar1.TabIndex = 0;
            trackBar1.Value = 100;
            // 
            // trackBar2
            // 
            trackBar2.Location = new Point(671, 313);
            trackBar2.Maximum = 3;
            trackBar2.Minimum = -3;
            trackBar2.Name = "trackBar2";
            trackBar2.Size = new Size(156, 69);
            trackBar2.TabIndex = 1;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(682, 78);
            numericUpDown1.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            numericUpDown1.Minimum = new decimal(new int[] { 3, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(145, 31);
            numericUpDown1.TabIndex = 2;
            numericUpDown1.Value = new decimal(new int[] { 3, 0, 0, 0 });
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(702, 24);
            label1.Name = "label1";
            label1.Size = new Size(125, 25);
            label1.TabIndex = 3;
            label1.Text = "Число сторон";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(624, 141);
            label2.Name = "label2";
            label2.Size = new Size(203, 25);
            label2.TabIndex = 4;
            label2.Text = "Размер многоугольниа";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(633, 274);
            label3.Name = "label3";
            label3.Size = new Size(134, 25);
            label3.TabIndex = 5;
            label3.Text = "Угол поворота";
            // 
            // trackBar3
            // 
            trackBar3.Location = new Point(682, 450);
            trackBar3.Maximum = 600;
            trackBar3.Name = "trackBar3";
            trackBar3.Size = new Size(156, 69);
            trackBar3.TabIndex = 6;
            trackBar3.Scroll += trackBar3_Scroll;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(682, 401);
            label4.Name = "label4";
            label4.Size = new Size(144, 25);
            label4.TabIndex = 7;
            label4.Text = "Расположение x";
            // 
            // button1
            // 
            button1.Location = new Point(714, 561);
            button1.Name = "button1";
            button1.Size = new Size(112, 34);
            button1.TabIndex = 8;
            button1.Text = "Очистить";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_2;
            // 
            // PolygonForm
            // 
            ClientSize = new Size(923, 666);
            Controls.Add(button1);
            Controls.Add(label4);
            Controls.Add(trackBar3);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(numericUpDown1);
            Controls.Add(trackBar2);
            Controls.Add(trackBar1);
            Name = "PolygonForm";
            Load += PolygonForm_Load;
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar2).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar3).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private void PolygonForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private TrackBar trackBar1;
        private TrackBar trackBar2;
        private NumericUpDown numericUpDown1;

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

            pol.center = new Point(pol.center.X + (int)dx, pol.center.Y + (int)dy);
            pol = new Polygon((int)numericUpDown1.Value, trackBar1.Value, new Point(pol.center.X, pol.center.Y), this.speed);
            pol.refresh();
            //MaximizeBox = false;
            if (k > 0)
            {
                trackBar3.Value = (int)x1;
                x1 = 600;
                y1 = 50;
                this.dx = x1 - pol.points[nextPoi].X;
                this.dy = y1 - pol.points[nextPoi].Y;
                pol.center = new Point(pol.center.X + (int)dx, pol.center.Y + (int)dy);
                pol.refresh();
                nextPoi = pol.maxY(line);
                x1 = trackBar3.Value;
                y1 = (float)k * x1 + (float)b;
                this.dx = x1 - pol.points[nextPoi].X;
                this.dy = y1 - pol.points[nextPoi].Y;
                pol.center = new Point(pol.center.X + (int)dx, pol.center.Y + (int)dy);
                pol.refresh();
            }
            else
            {
                float old_x = x1;
                x1 = 100;
                y1 = 100;
                this.dx = x1 - pol.points[0].X;
                this.dy = y1 - pol.points[0].Y;
                pol.center = new Point(pol.center.X + (int)dx, pol.center.Y + (int)dy);
                pol.refresh();
                nextPoi = pol.maxY(line);
                x1 = old_x;
                y1 = (float)k * x1 + (float)b;
                this.dx = x1 - pol.points[nextPoi].X;
                this.dy = y1 - pol.points[nextPoi].Y;
                pol.center = new Point(pol.center.X + (int)dx, pol.center.Y + (int)dy);
                pol.refresh();
            }


        }

        private Label label1;
        private Label label2;

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private Label label3;
        private NumericUpDown numericUpDown2;
        private TrackBar trackBar3;

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            x1 = trackBar3.Value;
            if (k > 0)
            {
                trackBar3.Value = (int)x1;
                x1 = 600;
                y1 = 50;
                this.dx = x1 - pol.points[nextPoi].X;
                this.dy = y1 - pol.points[nextPoi].Y;
                pol.center = new Point(pol.center.X + (int)dx, pol.center.Y + (int)dy);
                pol.refresh();
                nextPoi = pol.maxY(line);
                x1 = trackBar3.Value;
                y1 = (float)k * x1 + (float)b;
                this.dx = x1 - pol.points[nextPoi].X;
                this.dy = y1 - pol.points[nextPoi].Y;
                pol.center = new Point(pol.center.X + (int)dx, pol.center.Y + (int)dy);
                pol.refresh();
            }
            else
            {
                float old_x = x1;
                x1 = 100;
                y1 = 100;
                this.dx = x1 - pol.points[nextPoi].X;
                this.dy = y1 - pol.points[nextPoi].Y;
                pol.center = new Point(pol.center.X + (int)dx, pol.center.Y + (int)dy);
                pol.refresh();
                nextPoi = pol.maxY(line);
                x1 = old_x;
                y1 = (float)k * x1 + (float)b;
                this.dx = x1 - pol.points[nextPoi].X;
                this.dy = y1 - pol.points[nextPoi].Y;
                pol.center = new Point(pol.center.X + (int)dx, pol.center.Y + (int)dy);
                pol.refresh();
            }
        }

        private Label label4;
        private Button button1;

        private void button1_Click_2(object sender, EventArgs e)
        {
            mode = 0;
        }
    }

    public class Polygon
    {
        public PointF[] points;
        public int numberOfSides;
        public double radius;
        public Point center;
        public double rotation;
        public Polygon(int numberOfSides, double radius, Point center, double rotation)
        {
            this.numberOfSides = numberOfSides;
            this.radius = radius;
            this.center = center;
            this.rotation = rotation;
            this.points = new PointF[numberOfSides];
            for (int i = 0; i < this.numberOfSides; i++)
            {
                double angle = 2 * Math.PI * i / this.numberOfSides;
                this.points[i] = new PointF(
                this.center.X + (float)(this.radius * Math.Cos(angle + this.rotation)),
                    this.center.Y + (float)(this.radius * Math.Sin(angle + this.rotation))
                );
            }
        }
        public void refresh()
        {
            for (int i = 0; i < this.numberOfSides; i++)
            {
                double angle = 2 * Math.PI * i / this.numberOfSides;
                this.points[i] = new PointF(
                this.center.X + (float)(this.radius * Math.Cos(angle + this.rotation)),
                    this.center.Y + (float)(this.radius * Math.Sin(angle + this.rotation))
                );
            }
        }
        public int maxY( Point[] line)
        {
            float maxY = 100000;
            int maxN = 100;
            int maxN_2;
            for (int i = 0; i <numberOfSides; i++)
            {
                double dist = minDistance(new Point((int)line[0].X, (int)line[0].Y), new Point((int)line[1].X, (int)line[1].Y), new Point((int)points[i].X, (int)points[i].Y));
                   
                if (dist < maxY )
                {
                    
                        maxN = i;
                        maxY = (float)dist;
                    
                }
               
                
            }
            return maxN;
        }
        public int maxX(double k, double b)
        {
            float maxX = -100;
            int maxN = 100;
            int maxN_2;
            for (int i = 0; i < numberOfSides; i++)
            {
                if (points[i].X > maxX)
                {
                    maxN = i;
                    maxX = points[i].X;
                }
            }
            return maxN;
        }
        public int minX(double k, double b)
        {
            float minX = 10000;
            int maxN = 100;
            int maxN_2;
            for (int i = 0; i < numberOfSides; i++)
            {
                if (points[i].X < minX)
                {
                    maxN = i;
                    minX = points[i].X;
                }
            }
            return maxN;
        }
        public double minDistance(Point A, Point B, Point E)
        {

            // Используем формулу
            float x0 = E.X;
            float y0 = E.Y;
            float x1 = A.X;
            float y1 = A.Y;
            float x2 = B.X;
            float y2 = B.Y;
            double numerator = Math.Abs((y2 - y1) * x0 - (x2 - x1) * y0 + x2 * y1 - y2 * x1);
            double denominator = Math.Sqrt(Math.Pow(y2 - y1, 2) + Math.Pow(x2 - x1, 2));

            // Проверяем, чтобы деление на ноль не произошло
            if (denominator == 0)
            {
                throw new ArgumentException("Две точки, задающие прямую, не могут совпадать.");
            }

            return numerator / denominator;
        }
        public bool isLower(PointF p, double k,float b)
        {
            float ly = (float)k*p.X+b;
            if (ly <= p.Y)
            {
                return true;
            }
            else { return false; }
        }
        public int nextP(int poi)
        {
            if (poi == 0)
                return numberOfSides - 1;
            else return poi - 1;
        }
        public int nextP_left(int poi)
        {
            if (poi == numberOfSides-1)
                return 0;
            else return poi + 1;
        }
    }
    
}
