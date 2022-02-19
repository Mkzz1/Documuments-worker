using System.IO;

namespace Faza1Sorter_v2
{
    public partial class Form1 : Form
    {
        //bool Dragging;
        //int xPos;
        //int yPos;
        public Form1()
        {
            InitializeComponent();
            this.pictureBox1.MouseWheel += PictureBox1_MouseWheel;
        }

        private void PictureBox1_MouseWheel(object? sender, MouseEventArgs e)
        {
            if(e.Delta > 0)
            {
                pictureBox1.Width = pictureBox1.Width + 100;
                pictureBox1.Height = pictureBox1.Height + 100;
            }
            else
            {
                pictureBox1.Width = pictureBox1.Width - 100;
                pictureBox1.Height = pictureBox1.Height - 100;
            }
        }

        Image ZoomPicture(Image img, Size size)
        {
            Bitmap bm = new Bitmap(img, Convert.ToInt32(img.Width * size.Width), Convert.ToInt32(img.Height * size.Height));
            Graphics g = Graphics.FromImage(bm);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            return bm;
        }

        PictureBox org;

        private void Form1_Load(object sender, EventArgs e)
        {
            trackBar1.Minimum = 1;
            trackBar1.Maximum = 6;
            trackBar1.SmallChange = 1;
            trackBar1.LargeChange = 1;
            trackBar1.UseWaitCursor = false;

            this.DoubleBuffered = true;
            //org = new PictureBox();
            //org.Image = pictureBox1.Image;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if(trackBar1.Value != 0)
            {
                pictureBox1.Image = null;
                pictureBox1.Image = ZoomPicture(org.Image, new Size(trackBar1.Value, trackBar1.Value));

            }


        }

        public void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog od = new OpenFileDialog();
            var onlyFileName = System.IO.Path.GetFileName(od.FileName);
            if (od.ShowDialog() == DialogResult.OK)
            {
                org = new PictureBox();
                org.Load(od.FileName);
                pictureBox1.Cursor = Cursors.Hand;
                pictureBox1.Load(od.FileName);
                richTextBox1.Text = Path.GetFileName(od.FileName);
                
            }
            
        }
        //private void pictureBox1_MouseUp(object sender, MouseEventArgs e) { Dragging = false; }
        //private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Left)
        //    {
        //        Dragging = true;
        //        xPos = e.X;
        //        yPos = e.Y;
        //    }
        //}
        //private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        //{
        //    Control? c = sender as Control;
        //    if (Dragging && c != null)
        //    {
        //        c.Top = e.Y + c.Top - yPos;
        //        c.Left = e.X + c.Left - xPos;
        //    }
        //}

        public void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}