using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Painter
{
    public partial class Form1 : Form
    {
        private Color color = Color.White;
        private Color[] colors;
        private Rectangle rect = new Rectangle(250, 250, 150, 150);
        private Bitmap canvas;

        public Form1()
        {
            this.Width = 700;
            this.Height = 500;
            colors = new Color[] { Color.Red, Color.Green, Color.Gray, Color.Pink, Color.Violet };
            Paint += new PaintEventHandler(Painter);
            MouseClick += new MouseEventHandler(OnMouseClick);
            CreateButtons();
            canvas = new Bitmap(this.Width, this.Height);
            this.BackgroundImage = new Bitmap("D:\\Veronika\\Painter\\Canvas2.png");
            Graphics g = Graphics.FromImage(canvas);
            g.FillRectangle(Brushes.White, rect);
            g.DrawRectangle(Pens.Black, rect);
            g.FillEllipse(Brushes.White, rect);
            g.DrawEllipse(Pens.Black, rect);
            g.DrawLine(Pens.Black, rect.Left, rect.Top + rect.Top / 2, rect.Right, rect.Top + rect.Top / 2);
            //createControl(0, 0, 0, "D:\\Veronika\\Painter\\canvas.bmp");
            //createControl(1, 0, 700, "D:\\Veronika\\Painter\\palette.bmp");
            //createControl(2, 900, 700, "D:\\Veronika\\Painter\\clear.bmp");
            //createControl(3, 1000, 700, "D:\\Veronika\\Painter\\save.bmp");
            //createControl(4, 1100, 700, "D:\\Veronika\\Painter\\exit.bmp");
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (rect.Contains(e.Location))
            {
                FlooeFill(canvas, e.Location.X, e.Location.Y, color);
                this.Invalidate();
            }
        }

        private void FlooeFill(Bitmap bmp, int x, int y, Color fillColor)
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

        private void Painter(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(canvas, Point.Empty);
        }

        private void CreateButtons()
        {
            int count = 6;
            int posX = 0;

            for (int i = 0; i < count; i++)
            {
                Button button = new Button()
                {
                    Size = new Size(100, 50),
                    Text = "",
                    Location = new Point(posX, 10),
                };
                posX += button.Width;

                if (i > 4)
                {
                    button.BackColor = Color.White;
                    button.Image = new Bitmap("D:\\Veronika\\Painter\\save.bmp");
                    button.Click += new EventHandler(OnExitClick);
                }

                else
                {
                    button.BackColor = colors[i];
                }
                button.Click += new EventHandler(SetColor);
                this.Controls.Add(button);
            }
        }

        private void SetColor(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                color = button.BackColor;
            }
        }

        private void OnExitClick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
