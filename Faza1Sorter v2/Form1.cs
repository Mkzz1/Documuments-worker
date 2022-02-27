using System.IO;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;

namespace Faza1Sorter_v2
{
    public partial class Form1 : MaterialForm
    {
        private List<Image> LoadedImages { get; set; }
        private int SelectedImageIndex = 0;
        bool Dragging;
        int xPos;
        int yPos;
        public Form1()
        {
            InitializeComponent();
            this.pictureBox1.MouseWheel += PictureBox1_MouseWheel;

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
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

        private void Form1_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
        }
        private void LoadImagesFromFolder(string[] paths)
        {
            LoadedImages = new List<Image>();
            

            foreach(var path in paths)
            { 
                var tempImage = Image.FromFile(path);
                LoadedImages.Add(tempImage);
            }
        }

        public void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FolderBrowser = new FolderBrowserDialog();
            if(FolderBrowser.ShowDialog() == DialogResult.OK)
            {
                var selectedDirectory = FolderBrowser.SelectedPath;
                var imagePaths = Directory.GetFiles(selectedDirectory);

                LoadImagesFromFolder(imagePaths);

                //Inicializacja listy plików
                ImageList images = new ImageList();
                images.ImageSize = new System.Drawing.Size(130, 130);
                foreach (var image in LoadedImages)
                {
                    images.Images.Add(image);
                }
                //ustawianie listview z imagelsit
                listView1.LargeImageList = images;

                for (int itemIndex = 1; itemIndex < LoadedImages.Count; itemIndex++)
                {
                    listView1.Items.Add(new ListViewItem($"{itemIndex}", itemIndex - 1));
                }
            }
        }
        public void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e) { Dragging = false; }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Dragging = true;
                xPos = e.X;
                yPos = e.Y;
            }
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Control? c = sender as Control;
            if (Dragging && c != null)
            {
                c.Top = e.Y + c.Top - yPos;
                c.Left = e.X + c.Left - xPos;
            }
        }
        //przycisk do BK i tranzyt
        public void button2_Click(object sender, EventArgs e)
        {
            
            string startFodler = $@"C:\Users\Mkzz\Desktop\doki";
            string destinyFodler = @"C:\Users\Mkzz\Desktop\doki\Test1\";

            string[] files = Directory.GetFiles(startFodler, "*.tif");

            pictureBox1.Dispose();
            listView1.Dispose();
            
            foreach (string f in files)
            {
                string fName = f.Substring(startFodler.Length +1);
                File.Copy(Path.Combine(startFodler, fName), Path.Combine(destinyFodler, fName), true);

            }
            foreach (string f in files)
            {
                File.Delete(f);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string startFodler = $@"C:\Users\Mkzz\Desktop\doki";
            string destinyFodler = @"C:\Users\Mkzz\Desktop\doki\Test2\";

            string[] files = Directory.GetFiles(startFodler, "*.tif");

            foreach (string f in files)
            {
                string fName = f.Substring(startFodler.Length + 1);
                File.Copy(Path.Combine(startFodler, fName), Path.Combine(destinyFodler, fName), true);

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string startFodler = $@"C:\Users\Mkzz\Desktop\doki";
            string destinyFodler = @"C:\Users\Mkzz\Desktop\doki\Test3\";

            string[] files = Directory.GetFiles(startFodler, "*.tif");

            foreach (string f in files)
            {
                string fName = f.Substring(startFodler.Length + 1);
                File.Copy(Path.Combine(startFodler, fName), Path.Combine(destinyFodler, fName), true);

            }
        }

        private void button_navigation(object sender, EventArgs e)
        {
            var clickedButton = sender as Button;
            if (clickedButton.Text.Contains("Poprzedni"))
            {
                if (SelectedImageIndex > 0)
                {
                    SelectedImageIndex -= 1;
                    Image slectedImg = LoadedImages[SelectedImageIndex];
                    pictureBox1.Image = slectedImg;

                    SelectTheClickedItem(listView1, SelectedImageIndex);
                }
            }
            else
            {
                if (SelectedImageIndex < (LoadedImages.Count -1 ))
                {
                    SelectedImageIndex += 1;
                    Image slectedImg = LoadedImages[SelectedImageIndex];
                    pictureBox1.Image = slectedImg;

                    SelectTheClickedItem(listView1, SelectedImageIndex);
                }
            }
        }

        private void SelectTheClickedItem(ListView list, int index)
        {
            for(int item = 0; item < list.Items.Count; item++)
            {
                if (item == index)
                {
                    list.Items[item].Selected = true;
                }
                else
                {
                    list.Items[item].Selected = false;
                }
            }           
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {
                var selectedIndex = listView1.SelectedIndices[0];
                Image slectedImg = LoadedImages[selectedIndex];
                pictureBox1.Image = slectedImg;
                SelectedImageIndex = selectedIndex;
            }
        }

       MaterialSkinManager TManager = MaterialSkinManager.Instance;

        private void materialSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            if (materialSwitch1.Checked)
            {
                TManager.Theme = MaterialSkinManager.Themes.DARK;
                
            }
            else
            {
                TManager.Theme = MaterialSkinManager.Themes.LIGHT;
            }
        }

        private void materialTextBox21_Click(object sender, EventArgs e)
        {
          //show image file name in textbox
          string fileName = SelectedImageIndex.ToString();
          materialTextBox21.Text = fileName;
        }
    }
}
