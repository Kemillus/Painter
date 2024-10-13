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
        private const int WIDTH = 1200;
        private const int HEIGHT = 800;
        private const int FIGURE_X = 600;
        private const int FIGURE_Y = 350;
        private const int FIGURE_SIZE = 300;

        private Controll[] controls = new Controll[5];
        private int controlIndex = 0;

        public Form1()
        {
            this.Width = WIDTH;
            this.Height = HEIGHT;
            this.Text = "Graphics";

            this.Paint += new PaintEventHandler(drawFigure);

            controls[0] = new Controll { Left = 0, Top = 0, Right = 100, Bottom = 100 };
            controls[1] = new Controll { Left = 0, Top = 700, Right = 100, Bottom = 800 };
            controls[2] = new Controll { Left = 900, Top = 700, Right = 1000, Bottom = 800 };
            controls[3] = new Controll { Left = 1000, Top = 700, Right = 1100, Bottom = 800 };
            controls[4] = new Controll { Left = 1100, Top = 700, Right = 1200, Bottom = 800 };

            //createControl(0, 0, 0, "D:\\Veronika\\Painter\\canvas.bmp");
            //createControl(1, 0, 700, "D:\\Veronika\\Painter\\palette.bmp");
            //createControl(2, 900, 700, "D:\\Veronika\\Painter\\clear.bmp");
            //createControl(3, 1000, 700, "D:\\Veronika\\Painter\\save.bmp");
            //createControl(4, 1100, 700, "D:\\Veronika\\Painter\\exit.bmp");
            
            this.BackgroundImage = new Bitmap("D:\\Veronika\\Painter\\canvas.bmp");
            CreateImage(0, 0, 0, "D:\\Veronika\\Painter\\palette.bmp");
            this.Update();
        }

        private void CreateImage(int i,int left, int top, string fileName)
        {
            PictureBox pictureBox = new PictureBox();
            pictureBox.Image = new Bitmap(fileName);
            this.Controls.Add(pictureBox);
        }

        private void createControl(int i, int left, int top, string fileName)
        {
            Bitmap image = (Bitmap)Image.FromFile(fileName);
            this.CreateGraphics().DrawImage(image, left, top);
            controls[i].Left = left;
            controls[i].Top = top;
            controls[i].Right = left + image.Width;
            controls[i].Bottom = top + image.Height;
        }

        private void drawFigure(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillEllipse(Brushes.White, FIGURE_X - FIGURE_SIZE / 2, FIGURE_Y - FIGURE_SIZE / 2, FIGURE_SIZE, FIGURE_SIZE);
            g.DrawEllipse(Pens.White, FIGURE_X - FIGURE_SIZE / 2, FIGURE_Y - FIGURE_SIZE / 2, FIGURE_SIZE, FIGURE_SIZE);
            g.DrawLine(Pens.White, FIGURE_X - FIGURE_SIZE, FIGURE_Y - FIGURE_SIZE / 2, FIGURE_X, FIGURE_Y - FIGURE_SIZE);
            g.DrawLine(Pens.White, FIGURE_X + FIGURE_SIZE, FIGURE_Y - FIGURE_SIZE / 2, FIGURE_X, FIGURE_Y - FIGURE_SIZE);
            g.DrawLine(Pens.White, FIGURE_X - FIGURE_SIZE, FIGURE_Y + FIGURE_SIZE / 2, FIGURE_X, FIGURE_Y + FIGURE_SIZE);
            g.DrawLine(Pens.White, FIGURE_X + FIGURE_SIZE, FIGURE_Y + FIGURE_SIZE / 2, FIGURE_X, FIGURE_Y + FIGURE_SIZE);
        }

        private void fillFigure(object sender, MouseEventArgs e)
        {
            if (e.X >= FIGURE_X - FIGURE_SIZE / 2 && e.X <= FIGURE_X + FIGURE_SIZE / 2 &&
                e.Y >= FIGURE_Y - FIGURE_SIZE / 2 && e.Y <= FIGURE_Y + FIGURE_SIZE / 2)
            {
                this.CreateGraphics().FillEllipse(Brushes.White, FIGURE_X - FIGURE_SIZE / 2, FIGURE_Y - FIGURE_SIZE / 2, FIGURE_SIZE, FIGURE_SIZE);
            }
        }

        private void setColor(object sender, MouseEventArgs e)
        {
            Color color = this.BackColor;
            this.CreateGraphics().FillEllipse(new SolidBrush(color), FIGURE_X - FIGURE_SIZE / 2, FIGURE_Y - FIGURE_SIZE / 2, FIGURE_SIZE, FIGURE_SIZE);
        }

        private void saveImage(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(this.Width, this.Height);
            this.CreateGraphics().DrawImage(bitmap, 0, 0);
            bitmap.Save("output.bmp");
        }

        private void exit(object sender, EventArgs e)
        {
            this.Close();
        }

        private void selectControl(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                if (e.X >= controls[i].Left && e.X <= controls[i].Right &&
                    e.Y >= controls[i].Top && e.Y <= controls[i].Bottom)
                {
                    switch (i)
                    {
                        case 0:
                            fillFigure(sender, e);
                            break;
                        case 1:
                            setColor(sender, e);
                            break;
                        case 2:
                            this.CreateGraphics().FillEllipse(Brushes.White, FIGURE_X - FIGURE_SIZE / 2, FIGURE_Y - FIGURE_SIZE / 2, FIGURE_SIZE, FIGURE_SIZE);
                            break;
                        case 3:
                            saveImage(sender, e);
                            break;
                        case 4:
                            exit(sender, e);
                            break;
                    }
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            selectControl(this, e);
            base.OnMouseDown(e);
        }
    }
}
