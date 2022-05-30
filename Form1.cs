using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace Game
{
    public partial class Form1 : Form
    {
        readonly int side = 20;
        readonly int width = 30;
        readonly int height = 30;
        readonly State[,] Map;
        Point currentPoint;
        readonly Stack<Point> stack;
        readonly Random rnd;
        bool builded = false;
        bool inTheEnd = false;
        bool isBuilded = true;
        int treasureCount;

        public enum State
        {
            Cell,
            Wall,
            Visited
        }

        public Form1()
        {
            InitializeComponent();
            SetStyle( ControlStyles.OptimizedDoubleBuffer | 
                ControlStyles.AllPaintingInWmPaint | 
                ControlStyles.UserPaint, true);
            UpdateStyles();

            this.KeyDown += new KeyEventHandler(Move);

            pictureBox1.BackgroundImage = new Bitmap(Resource1.IO);
            pictureBox2.BackgroundImage = new Bitmap(Resource1.light);
            pictureBox3.BackgroundImage = new Bitmap(Resource1.Treasure);
            pictureBox4.BackgroundImage = new Bitmap(Resource1.Treasure);

            panel1.Location = new Point(1, 1);
            panel1.Size = new Size(width * side, height * side);

            timer2.Interval = 1;
            timer2.Tick += new EventHandler(Update1);
            timer2.Start();

            timer3.Interval = 1000;
            timer3.Tick += new EventHandler(Update2);
            timer3.Start();

            stack = new Stack<Point>();
            rnd = new Random();
            Map = new State[width, height];
            InitMap();
            currentPoint = new Point(1, 1);
            Treasure();
        }

        private void Update1(object sender, EventArgs e)
        {
            if (pictureBox1.Location == pictureBox2.Location)
                pictureBox2.Visible = false;
        }

        private void Update2(object sender, EventArgs e)
        {
            if (inTheEnd)
            {
                pictureBox2.Visible = false;
            }
            timer2.Stop();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            panel1_Click(panel1, new EventArgs());
            var width = panel1.Bounds.Size.Width;
            var height = panel1.Bounds.Size.Height + menuStrip1.Height;
            ClientSize = new Size(width, height);
            CenterToScreen();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (isBuilded)
            {
                for (int x = 0; x < width; x++)
                    for (int y = 0; y < height; y++)
                    {
                        switch (Map[x, y])
                        {
                            case State.Cell:
                                DrawBox(x, y, Brushes.Black, e.Graphics);
                                break;
                            case State.Wall:
                                DrawBox(x, y, Brushes.Black, e.Graphics);
                                break;
                        }
                    }
                isBuilded = false;
            }
        }

        private void Treasure()
        {
            Graphics g = this.CreateGraphics();
            var rnd = new Random();
            var treasureLocationX = (int)rnd.Next(0, width);
            var treasureLocationY = (int)rnd.Next(0, height);

            if (treasureCount == 3)
            {
                MessageBox.Show("Игра окончена, Вы нашли все сундуки! Нажмите на ПКЛ или ЛКМ для повторной игры.");
                pictureBox3.Visible = false;
            }

            while (treasureCount < 3)
            {
                if (Map[treasureLocationX, treasureLocationY] != State.Wall)
                {
                    if (treasureCount != 0) MessageBox.Show("Вы нашли сундук!");
                    treasureCount += 1;
                    pictureBox3.Visible = false;
                    pictureBox3.Location = new Point(treasureLocationX * side + 1, treasureLocationY * side + 1);
                    break;
                }
                else
                {
                    treasureLocationX = (int)rnd.Next(0, width);
                    treasureLocationY = (int)rnd.Next(0, height);
                }
            }
        }

        private void DrawBox(int x, int y, Brush brush, Graphics g)
        {
            g.FillRectangle(brush, x * side, y * side, side, side);
        }

        private new void Move(object sender, KeyEventArgs e)
        {
            timer2.Start();
            pictureBox2.Visible = true;
            inTheEnd = false;
            Graphics g = this.CreateGraphics();

            if (e.KeyCode == Keys.W && pictureBox1.Location.Y > side && Map[pictureBox1.Location.X / side, (pictureBox1.Location.Y - side) / side] != State.Wall)
            {             
                pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y - side);
                Light(Keys.W);
            }
            if (e.KeyCode == Keys.S && pictureBox1.Location.Y <= side * (height - 3) && Map[pictureBox1.Location.X / side, (pictureBox1.Location.Y + side) / side] != State.Wall)
            {
                pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y + side);
                Light(Keys.S);
            }
            if (e.KeyCode == Keys.A && pictureBox1.Location.X > side && Map[(pictureBox1.Location.X - side) / side, pictureBox1.Location.Y / side] != State.Wall)
            {
                pictureBox1.Location = new Point(pictureBox1.Location.X - side, pictureBox1.Location.Y);
                Light(Keys.A);
            }
            if (e.KeyCode == Keys.D && pictureBox1.Location.X <= side * (width - 3) && Map[(pictureBox1.Location.X + side) / side, pictureBox1.Location.Y / side] != State.Wall)
            {
                pictureBox1.Location = new Point(pictureBox1.Location.X + side, pictureBox1.Location.Y);
                Light(Keys.D);
            }
            if (e.KeyCode == Keys.Space)
                if (pictureBox1.Location == pictureBox3.Location)
                { 
                    Treasure();
                    DrawBox(pictureBox1.Location.X, pictureBox1.Location.Y, Brushes.White, g);
                }
        }

        private void Light(Keys key)
        {
            pictureBox2.Location = pictureBox1.Location;

            if (key == Keys.W)
                if (Map[pictureBox1.Location.X / side, (pictureBox1.Location.Y - side) / side] != State.Wall)
                {
                    while (pictureBox2.Location.Y > side && Map[pictureBox2.Location.X / side, (pictureBox2.Location.Y - side) / side] != State.Wall)
                    {
                        pictureBox2.Location = new Point(pictureBox2.Location.X, pictureBox2.Location.Y - side);
                        if (pictureBox2.Location == pictureBox3.Location)
                            pictureBox3.Visible = true;
                    }
                    inTheEnd = true;
                }

            if (key == Keys.S)
                if (Map[pictureBox1.Location.X / side, (pictureBox1.Location.Y + side) / side] != State.Wall)
                {
                    while (pictureBox2.Location.Y <= side * (height - 3) && Map[pictureBox2.Location.X / side, (pictureBox2.Location.Y + side) / side] != State.Wall)
                    {
                        pictureBox2.Location = new Point(pictureBox2.Location.X, pictureBox2.Location.Y + side);
                        if (pictureBox2.Location == pictureBox3.Location)
                            pictureBox3.Visible = true;
                    }
                    inTheEnd = true;
                }

            if (key == Keys.A)
                if (Map[(pictureBox1.Location.X - side) / side, pictureBox1.Location.Y / side] != State.Wall)
                {
                    while (pictureBox2.Location.X > side && Map[(pictureBox2.Location.X - side) / side, pictureBox2.Location.Y / side] != State.Wall)
                    {
                        pictureBox2.Location = new Point(pictureBox2.Location.X - side, pictureBox2.Location.Y);
                        if (pictureBox2.Location == pictureBox3.Location)
                            pictureBox3.Visible = true;
                    }
                    inTheEnd = true;
                }

            if (key == Keys.D)
                if (Map[(pictureBox1.Location.X + side) / side, pictureBox1.Location.Y / side] != State.Wall)
                {
                    while (pictureBox2.Location.X <= side * (width - 3) && Map[(pictureBox2.Location.X + side) / side, pictureBox2.Location.Y / side] != State.Wall)
                    {
                        pictureBox2.Location = new Point(pictureBox2.Location.X + side, pictureBox2.Location.Y);
                        if (pictureBox2.Location == pictureBox3.Location)
                            pictureBox3.Visible = true;
                    }
                    inTheEnd = true;
                }
        }

        private void InitMap()
        {
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    if(i % 2 != 0 && j % 2 != 0 &&         
                    i < width - 1 && j < height - 1)    
                    Map[i, j] = State.Cell;             
                    else Map[i, j] = State.Wall;
                }
            }
        }

        private void BuildMap()
        {
            while (true)
            {
                var neighbours = GetNeighbours(currentPoint);
                if (neighbours.Length != 0)
                {
                    var randNum = rnd.Next(neighbours.Length);
                    var neighbourCell = neighbours[randNum];
                    if (neighbours.Length > 1)
                        stack.Push(currentPoint); 
                    RemoveWall(currentPoint, neighbourCell);
                    Map[currentPoint.X, currentPoint.Y] = State.Visited;
                    currentPoint = neighbourCell;
                }
                else if (stack.Count > 0) 
                {
                    Map[currentPoint.X, currentPoint.Y] = State.Visited;
                    currentPoint = stack.Pop();
                }
                else break;
            }
        }

        private void RemoveWall(Point first, Point second)
        {
            var x = second.X - first.X;
            var y = second.Y - first.Y;
            int addX, addY;
            var wall = new Point();
            if (x != 0)
                addX = (x / Math.Abs(x));
            else addX = 0;
            if (y != 0)
                addY = (y / Math.Abs(y));
            else addY = 0;
            wall.X = first.X + addX;
            wall.Y = first.Y + addY;
            Map[wall.X, wall.Y] = State.Visited;
        }

        private Point[] GetNeighbours(Point c)
        {
            const int distance = 2;
            var points = new List<Point>();
            var x = c.X;
            var y = c.Y;
            var up = new Point(x, y - distance);
            var right = new Point(x + distance, y);
            var down = new Point(x, y + distance);
            var left = new Point(x - distance, y);
            var neighbours = new Point[] { down, right, up, left };
            foreach (var point in neighbours)
            {
                if (point.X > 0 && point.X < width && point.Y > 0 && point.Y < height)
                {

                    if (Map[point.X, point.Y] != State.Wall && Map[point.X, point.Y] != State.Visited)
                        points.Add(point); 
                }
            }
            return points.ToArray();
        }

        private void PrepareAfterBuildMap()
        {
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    if (Map[i, j] == State.Visited)
                        Map[i, j] = State.Cell;
                }
            }
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            if (builded)
            {
                InitMap();
                currentPoint = new Point(1, 1);
            }
            isBuilded = true;
            pictureBox1.Location = new Point(side + 1, side + 1);
            pictureBox2.Location = new Point(side + 1, side + 1);
            BuildMap();
            PrepareAfterBuildMap();
            builded = true;
            panel1.Invalidate();       
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            pictureBox3.Visible = true;
            Wait(5);
            pictureBox3.Visible = false;
            isBuilded = true;
        }

        private void Wait(double seconds)
        {
            int ticks = System.Environment.TickCount + (int)Math.Round(seconds * 1000.0);
            while (System.Environment.TickCount < ticks)
            {
                Application.DoEvents();
            }
        }
    }
}