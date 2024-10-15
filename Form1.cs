using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Painter
{
    public partial class Form1 : Form
    {
        private Color selectedColor = Color.White;
        private Color[] colors = { Color.Red, Color.Green, Color.Gray, Color.Pink, Color.Violet, Color.Blue, Color.Brown };
        private Rectangle shapeBounds = new Rectangle(250, 250, 450, 450);
        private Bitmap canvas;
        private Bitmap bitmapExit = new Bitmap("D:\\Veronika\\Painter\\Painter\\Resources\\exit.png");
        private Bitmap bitmapSave =new Bitmap("D:\\Veronika\\Painter\\Painter\\Resources\\download.png");
        private Bitmap bitmapClear = new Bitmap("D:\\Veronika\\Painter\\Painter\\Resources\\trash.png");
        private Bitmap bitmapBackground = new Bitmap("D:\\Veronika\\Painter\\Painter\\Resources\\background.jpg");
        private Pen borderPen = new Pen(Color.Black, 2);

        public Form1()
        {
            InitializeComponent();
            SetupForm();
            InitializeCanvas();
            CreateColorButtons();
            CreateFunctionalButtons();
        }

        private void SetupForm()
        {
            this.Width = 1000;
            this.Height = 900;
            this.DoubleBuffered = true;
            this.Paint += new PaintEventHandler(OnPaintCanvas);
            this.MouseClick += new MouseEventHandler(OnCanvasMouseClick);

        }

        private void InitializeCanvas()
        {
            canvas = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(canvas);
            DrawInitialShapes(g);
            this.BackgroundImage = bitmapBackground;
        }

        private void DrawInitialShapes(Graphics g)
        {
            Rectangle mainCircle = shapeBounds;
            g.FillEllipse(Brushes.White, mainCircle);
            g.DrawEllipse(borderPen, mainCircle);

            for (int i = 1; i <= 4; i++)
            {
                int offset = i * 30;
                Rectangle circle = new Rectangle(mainCircle.X + offset, mainCircle.Y + offset, mainCircle.Width - offset * 2, mainCircle.Height - offset * 2);
                g.DrawEllipse(borderPen, circle);
            }

            for (int i = 0; i <= 360; i += 30)
            {
                double angle = i * Math.PI / 180;
                float x = mainCircle.X + mainCircle.Width / 2 + (float)(mainCircle.Width / 2 * Math.Cos(angle));
                float y = mainCircle.Y + mainCircle.Height / 2 + (float)(mainCircle.Height / 2 * Math.Sin(angle));
                g.DrawLine(borderPen, mainCircle.X + mainCircle.Width / 2, mainCircle.Y + mainCircle.Height / 2, x, y);
            }
        }

        private void OnPaintCanvas(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(canvas, Point.Empty);
        }

        private void OnCanvasMouseClick(object sender, MouseEventArgs e)
        {
            if (IsPointInCircle(e.Location, shapeBounds))
            {
                FloodFill(canvas, e.Location.X, e.Location.Y, selectedColor);
                this.Invalidate();
            }
        }

        private bool IsPointInCircle(Point point, Rectangle bounds)
        {
            float centerX = bounds.X + bounds.Width / 2;
            float centerY = bounds.Y + bounds.Height / 2;
            float radius = bounds.Width / 2;

            float dx = point.X - centerX;
            float dy = point.Y - centerY;

            return (dx * dx + dy * dy) <= (radius * radius);
        }

        private void FloodFill(Bitmap bmp, int x, int y, Color fillColor)
        {
            Color targetColor = bmp.GetPixel(x, y);
            if (targetColor.ToArgb() == fillColor.ToArgb() || targetColor.ToArgb() == Color.Black.ToArgb())
                return;

            Stack<Point> pixels = new Stack<Point>();
            pixels.Push(new Point(x, y));

            while (pixels.Count > 0)
            {
                Point pt = pixels.Pop();
                if (pt.X < 0 || pt.Y < 0 || pt.X >= bmp.Width || pt.Y >= bmp.Height)
                    continue;

                Color currentColor = bmp.GetPixel(pt.X, pt.Y);
                if (currentColor.ToArgb() != targetColor.ToArgb() || currentColor.ToArgb() == Color.Black.ToArgb())
                    continue;

                bmp.SetPixel(pt.X, pt.Y, fillColor);
                pixels.Push(new Point(pt.X + 1, pt.Y));
                pixels.Push(new Point(pt.X - 1, pt.Y));
                pixels.Push(new Point(pt.X, pt.Y + 1));
                pixels.Push(new Point(pt.X, pt.Y - 1));
            }
        }

        private void CreateColorButtons()
        {
            int posX = 0;
            Size buttonSize = new Size(100, 50);

            for (int i = 0; i < colors.Length; i++)
            {
                CreateButton($"ButtonColor{i}", colors[i], new Point(posX, 0), buttonSize);
                posX += buttonSize.Width;
            }
        }

        private void CreateFunctionalButtons()
        {
            int posX = colors.Length * 100;
            Size buttonSize = new Size(100, 50);

            CreateButton("ButtonClear", Color.White, new Point(posX, 0), buttonSize, bitmapClear);
            CreateButton("ButtonSave", Color.White, new Point(posX += buttonSize.Width, 0), buttonSize, bitmapSave);
            CreateButton("ButtonExit", Color.White, new Point(posX + buttonSize.Width, 0), buttonSize, bitmapExit);
        }

        private void CreateButton(string name, Color color, Point location, Size size, Bitmap bitmap = null)
        {
            Button button = new Button()
            {
                Name = name,
                BackColor = color,
                Image = bitmap != null ? new Bitmap(bitmap) : null,
                Location = location,
                Size = size
            };
            button.Click += bitmap == null ? new EventHandler(SetColor) : new EventHandler(ExecuteFunctionalButtonClick);
            this.Controls.Add(button);
        }

        private void SetColor(object sender, EventArgs e)
        {
            selectedColor = (sender as Button).BackColor;
        }

        private void ExecuteFunctionalButtonClick(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button.Name == "ButtonExit")
            {
                this.Close();
            }

            else if (button.Name == "ButtonClear")
            {
                Graphics g = Graphics.FromImage(canvas);
                g.DrawImage(bitmapBackground, Point.Empty);
                DrawInitialShapes(g);
                this.Invalidate();
            }

            else if (button.Name == "ButtonSave")
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Bitmap Image|*.bmp"
                };
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    canvas.Save(saveFileDialog.FileName);
                }
            }
        }
    }
}
