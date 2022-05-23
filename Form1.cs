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
        bool flag = true;
        readonly int side = 20;
        readonly int width = 30;
        readonly int height = 30;
        readonly State[,] Map;
        Point currentPoint;
        readonly Stack<Point> stack;
        readonly Random rnd;
        bool builded = false;
        Bitmap player = Resource1.IO;

        public enum State
        {
            Cell,
            Wall,
            Visited,
            Light
        }

        public Form1()
        {
            InitializeComponent();
            SetStyle( ControlStyles.OptimizedDoubleBuffer | 
                ControlStyles.AllPaintingInWmPaint | 
                ControlStyles.UserPaint, true);
            UpdateStyles();

            this.KeyDown += new KeyEventHandler(Move);

            panel1.Location = new Point(1, 1);
            panel1.Size = new Size(width * side, height * side);
            stack = new Stack<Point>();
            rnd = new Random();
            Map = new State[width, height];
            InitMap();
            currentPoint = new Point(1, 1);
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
            if (flag)
            {
                for (int x = 0; x < width; x++)
                    for (int y = 0; y < height; y++)
                    {
                        switch (Map[x, y])
                        {
                            case State.Cell:
                                DrawBox(x, y, Brushes.White, e.Graphics);
                                break;
                            case State.Wall:
                                DrawBox(x, y, Brushes.Black, e.Graphics);
                                break;
                            case State.Visited:
                                DrawBox(x, y, Brushes.Red, e.Graphics);
                                break;
                        }
                    }
                flag = false;
            }
        }

        private void DrawBox(int x, int y, Brush brush, Graphics g)
        {
            g.FillRectangle(brush, x * side, y * side, side, side);
        }

        private void Form1_Timer(object sender, EventArgs e)
        {
            Refresh();
        }

        private new void Move(object sender, KeyEventArgs e)
        {
            Graphics g = this.CreateGraphics();
            if (e.KeyCode == Keys.W && pictureBox1.Location.Y > side && Map[pictureBox1.Location.X / side, (pictureBox1.Location.Y - side) / side] != State.Wall)
            {
                pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y - side);
                LightMove(Keys.W);
            }
            if (e.KeyCode == Keys.S && pictureBox1.Location.Y <= side * (height - 3) && Map[pictureBox1.Location.X / side, (pictureBox1.Location.Y + side) / side] != State.Wall)
            {
                pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y + side);
                LightMove(Keys.S);
            }
            if (e.KeyCode == Keys.A && pictureBox1.Location.X > side && Map[(pictureBox1.Location.X - side) / side, pictureBox1.Location.Y / side] != State.Wall)
            {
                pictureBox1.Location = new Point(pictureBox1.Location.X - side, pictureBox1.Location.Y);
                LightMove(Keys.A);
            }
            if (e.KeyCode == Keys.D && pictureBox1.Location.X <= side * (width - 3) && Map[(pictureBox1.Location.X + side) / side, pictureBox1.Location.Y / side] != State.Wall)
            {                   
                pictureBox1.Location = new Point(pictureBox1.Location.X + side, pictureBox1.Location.Y);
                LightMove(Keys.D);
            }
        }

        private void LightMove(Keys key)
        {
            Graphics g = this.CreateGraphics();

            pictureBox2.Visible = false;
            pictureBox2.Location = pictureBox1.Location;

            if(key == Keys.W)
            {
                if (Map[pictureBox1.Location.X / side, (pictureBox1.Location.Y - side) / side] != State.Wall)
                {
                    while (pictureBox2.Location.Y > side && Map[pictureBox2.Location.X / side, (pictureBox2.Location.Y - side) / side] != State.Wall)
                    {
                        pictureBox2.Location = new Point(pictureBox2.Location.X, pictureBox2.Location.Y - side);
                        if (!pictureBox2.Visible) pictureBox2.Visible = true;                      
                    }
                    pictureBox2.Visible = false;
                }
            }

            if (key == Keys.S)
            {
                if (Map[pictureBox1.Location.X / side, (pictureBox1.Location.Y + side) / side] != State.Wall)
                {
                    while (pictureBox2.Location.Y <= side * (height - 3) && Map[pictureBox2.Location.X / side, (pictureBox2.Location.Y + side) / side] != State.Wall)
                    {
                        pictureBox2.Location = new Point(pictureBox2.Location.X, pictureBox2.Location.Y + side);
                        if (!pictureBox2.Visible) pictureBox2.Visible = true;
                    }
                    pictureBox2.Visible = false;
                }
            }

            if (key == Keys.A)
            {
                if (Map[(pictureBox1.Location.X - side) / side, pictureBox1.Location.Y / side] != State.Wall)
                {
                    while (pictureBox2.Location.X > side && Map[(pictureBox2.Location.X - side) / side, pictureBox2.Location.Y / side] != State.Wall)
                    {
                        pictureBox2.Location = new Point(pictureBox2.Location.X - side, pictureBox2.Location.Y);
                        if (!pictureBox2.Visible) pictureBox2.Visible = true;
                    }
                    pictureBox2.Visible = false;
                }
            }

            if (key == Keys.D)
            {
                if (Map[(pictureBox1.Location.X + side) / side, pictureBox1.Location.Y / side] != State.Wall)
                {
                    while (pictureBox2.Location.X <= side * (width - 3) && Map[(pictureBox2.Location.X + side) / side, pictureBox2.Location.Y / side] != State.Wall)
                    {
                        pictureBox2.Location = new Point(pictureBox2.Location.X + side, pictureBox2.Location.Y);
                        if (!pictureBox2.Visible) pictureBox2.Visible = true;
                    }
                    pictureBox2.Visible = false;
                }
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
            BuildMap();
            PrepareAfterBuildMap();
            builded = true;
            panel1.Invalidate();
        }
    }
}