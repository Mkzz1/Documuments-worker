using MaterialSkin;
using MaterialSkin.Controls;

namespace Faza1Sorter_v2
{
    public partial class Form1 : MaterialForm
    {
        int xPos;
        int yPos;
        bool Dragging;
        public Form1()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
            this.pictureBox1.MouseWheel += PictureBox1_MouseWheel;
        }

        private void PictureBox1_MouseWheel(object? sender, MouseEventArgs e)
        {
            //this will zoom in and out the picture in picturebox
            if (e.Delta > 0)
            {
                pictureBox1.Width += 50;
                pictureBox1.Height += 50;
            }
            else
            {
                pictureBox1.Width -= 50;
                pictureBox1.Height -= 50;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //this button will open folder browser dialog and select the folder. Import all files from that folder and sort them by name. Files will be shown in listview
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            string path = folderBrowserDialog.SelectedPath;
            string[] files = Directory.GetFiles(path);
            listView1.Items.Clear();
            foreach (string file in files)
            {
                string[] fileName = file.Split('\\');
                ListViewItem item = new ListViewItem(fileName[fileName.Length - 1]);
                item.SubItems.Add(file);
                listView1.Items.Add(item);
            }
            //selected picture in listview will be shown in picturebox
            listView1.SelectedIndexChanged += (s, args) =>
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    pictureBox1.ImageLocation = listView1.SelectedItems[0].SubItems[1].Text;
                }
            };
            //first ten charachers of file name showedin picturebox will be shown in textbox
            listView1.SelectedIndexChanged += (s, args) =>
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    string[] fileName = listView1.SelectedItems[0].SubItems[1].Text.Split('\\');
                    materialTextBox21.Text = fileName[fileName.Length - 1].Substring(0, 10);
                }
            };
            //first item in listview will be selected
            listView1.Items[0].Selected = true;
        }
        
        private void button6_Click(object sender, EventArgs e)
        {
            int index = listView1.SelectedIndices[0] + 1;

            // In case we're in the last row
            if (index >= listView1.Items.Count)
                return;

            listView1.Items[index].Selected = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int index = listView1.SelectedIndices[0] - 1;

            // In case we're in the first row
            if (index < 0)
                return;

            listView1.Items[index].Selected = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //this button will move selected tif file in the listview to the C:\Users\Mkzz\Desktop\doki\Test2 folder with the same name as original file.
            if (listView1.SelectedItems.Count > 0)
            {
                string fileName = listView1.SelectedItems[0].SubItems[1].Text;
                string[] fileName2 = fileName.Split('\\');
                string newFileName = fileName2[fileName2.Length - 1];
                string newPath = @"L:\Odprawa_celna\POZ\GCG\...FAZA1_DO_WYS£ANIA_POZ\...PPWK tam gdzie mo¿na wys³aæ @\" + newFileName;
                File.Move(fileName, newPath);
                listView1.Items.Remove(listView1.SelectedItems[0]);
            }
            //all files wchich contain selected text in textbox will be moved to the C:\Users\Mkzz\Desktop\doki\Test2 folder with the same name as original file.
            if (materialTextBox21.Text != "")
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    string fileName = item.SubItems[1].Text;
                    string[] fileName2 = fileName.Split('\\');
                    string newFileName = fileName2[fileName2.Length - 1];
                    string newPath = @"L:\Odprawa_celna\POZ\GCG\...FAZA1_DO_WYS£ANIA_POZ\...PPWK tam gdzie mo¿na wys³aæ @\" + newFileName;
                    if (newFileName.Contains(materialTextBox21.Text))
                    {
                        File.Move(fileName, newPath);
                        listView1.Items.Remove(item);
                    }
                }
            }
            //next item in listview will be selected
            if (listView1.Items.Count > 0)
            {
                listView1.Items[0].Selected = true;
                listView1.Items[0].EnsureVisible();
            }
        }

        private void materialSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            //this switch will change the color of the form to dark mode.
            if (materialSwitch1.Checked)
            {
                var materialSkinManager = MaterialSkinManager.Instance;
                materialSkinManager.AddFormToManage(this);
                materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
                materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
            }
            else
            {
                var materialSkinManager = MaterialSkinManager.Instance;
                materialSkinManager.AddFormToManage(this);
                materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
                materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //this button will rotate the picture in picturebox 90 degrees clockwise.
                pictureBox1.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                pictureBox1.Refresh();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //this button will rotate the picture in picturebox 90 degrees counter clockwise.
            pictureBox1.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            pictureBox1.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //this button will move selected tif file in the listview to the C:\Users\Mkzz\Desktop\doki\Test2 folder with the same name as original file.
            if (listView1.SelectedItems.Count > 0)
            {
                string fileName = listView1.SelectedItems[0].SubItems[1].Text;
                string[] fileName2 = fileName.Split('\\');
                string newFileName = fileName2[fileName2.Length - 1];
                string newPath = @"L:\Odprawa_celna\POZ\GCG\...FAZA1_DO_WYS£ANIA_POZ\...TelefonSMS\" + newFileName;
                File.Move(fileName, newPath);
                listView1.Items.Remove(listView1.SelectedItems[0]);
            }
            //all files wchich contain selected text in textbox will be moved to the C:\Users\Mkzz\Desktop\doki\Test2 folder with the same name as original file.
            if (materialTextBox21.Text != "")
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    string fileName = item.SubItems[1].Text;
                    string[] fileName2 = fileName.Split('\\');
                    string newFileName = fileName2[fileName2.Length - 1];
                    string newPath = @"L:\Odprawa_celna\POZ\GCG\...FAZA1_DO_WYS£ANIA_POZ\...TelefonSMS\" + newFileName;
                    if (newFileName.Contains(materialTextBox21.Text))
                    {
                        File.Move(fileName, newPath);
                        listView1.Items.Remove(item);
                    }
                }
            }
            //next item in listview will be selected
            if (listView1.Items.Count > 0)
            {
                listView1.Items[0].Selected = true;
                listView1.Items[0].EnsureVisible();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //this button will move selected tif file in the listview to the C:\Users\Mkzz\Desktop\doki\Test2 folder with the same name as original file.
            if (listView1.SelectedItems.Count > 0)
            {
                string fileName = listView1.SelectedItems[0].SubItems[1].Text;
                string[] fileName2 = fileName.Split('\\');
                string newFileName = fileName2[fileName2.Length - 1];
                string newPath = @"L:\Odprawa_celna\POZ\GCG\..NOWE DO AWIZACJI\6110\" + newFileName;
                File.Move(fileName, newPath);
                listView1.Items.Remove(listView1.SelectedItems[0]);
            }
            //all files wchich contain selected text in textbox will be moved to the C:\Users\Mkzz\Desktop\doki\Test2 folder with the same name as original file.
            if (materialTextBox21.Text != "")
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    string fileName = item.SubItems[1].Text;
                    string[] fileName2 = fileName.Split('\\');
                    string newFileName = fileName2[fileName2.Length - 1];
                    string newPath = @"L:\Odprawa_celna\POZ\GCG\..NOWE DO AWIZACJI\6110\" + newFileName;
                    if (newFileName.Contains(materialTextBox21.Text))
                    {
                        File.Move(fileName, newPath);
                        listView1.Items.Remove(item);
                    }
                }
            }
            //next item in listview will be selected
            if (listView1.Items.Count > 0)
            {
                listView1.Items[0].Selected = true;
                listView1.Items[0].EnsureVisible();
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //this function will let user move picture in picturebox.
            Control c = sender as Control;
            if (Dragging && c != null)
            {
                c.Top = e.Y + c.Top - yPos;
                c.Left = e.X + c.Left - xPos;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //this function will let user move picture in picturebox.
                xPos = e.X;
                yPos = e.Y;
                Dragging = true;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            Dragging = false;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //this button will move selected tif file in the listview to the C:\Users\Mkzz\Desktop\doki\Test2 folder with the same name as original file.
            if (listView1.SelectedItems.Count > 0)
            {
                string fileName = listView1.SelectedItems[0].SubItems[1].Text;
                string[] fileName2 = fileName.Split('\\');
                string newFileName = fileName2[fileName2.Length - 1];
                string newPath = @"L:\Odprawa_celna\POZ\GCG\..NOWE DO AWIZACJI\TRANZYT\" + newFileName;
                File.Move(fileName, newPath);
                listView1.Items.Remove(listView1.SelectedItems[0]);
            }
            //all files wchich contain selected text in textbox will be moved to the C:\Users\Mkzz\Desktop\doki\Test2 folder with the same name as original file.
            if (materialTextBox21.Text != "")
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    string fileName = item.SubItems[1].Text;
                    string[] fileName2 = fileName.Split('\\');
                    string newFileName = fileName2[fileName2.Length - 1];
                    string newPath = @"L:\Odprawa_celna\POZ\GCG\..NOWE DO AWIZACJI\TRANZYT\" + newFileName;
                    if (newFileName.Contains(materialTextBox21.Text))
                    {
                        File.Move(fileName, newPath);
                        listView1.Items.Remove(item);
                    }
                }
            }
            //next item in listview will be selected
            if (listView1.Items.Count > 0)
            {
                listView1.Items[0].Selected = true;
                listView1.Items[0].EnsureVisible();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //this button will move selected tif file in the listview to the C:\Users\Mkzz\Desktop\doki\Test2 folder with the same name as original file.
            if (listView1.SelectedItems.Count > 0)
            {
                string fileName = listView1.SelectedItems[0].SubItems[1].Text;
                string[] fileName2 = fileName.Split('\\');
                string newFileName = fileName2[fileName2.Length - 1];
                string newPath = @"L:\Odprawa_celna\POZ\GCG\..NOWE DO AWIZACJI\BK\" + newFileName;
                File.Move(fileName, newPath);
                listView1.Items.Remove(listView1.SelectedItems[0]);
            }
            //all files wchich contain selected text in textbox will be moved to the C:\Users\Mkzz\Desktop\doki\Test2 folder with the same name as original file.
            if (materialTextBox21.Text != "")
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    string fileName = item.SubItems[1].Text;
                    string[] fileName2 = fileName.Split('\\');
                    string newFileName = fileName2[fileName2.Length - 1];
                    string newPath = @"L:\Odprawa_celna\POZ\GCG\..NOWE DO AWIZACJI\BK\" + newFileName;
                    if (newFileName.Contains(materialTextBox21.Text))
                    {
                        File.Move(fileName, newPath);
                        listView1.Items.Remove(item);
                    }
                }
            }
            //next item in listview will be selected
            if (listView1.Items.Count > 0)
            {
                listView1.Items[0].Selected = true;
                listView1.Items[0].EnsureVisible();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //this button will move selected tif file in the listview to the C:\Users\Mkzz\Desktop\doki\Test2 folder with the same name as original file.
            if (listView1.SelectedItems.Count > 0)
            {
                string fileName = listView1.SelectedItems[0].SubItems[1].Text;
                string[] fileName2 = fileName.Split('\\');
                string newFileName = fileName2[fileName2.Length - 1];
                string newPath = @"L:\Odprawa_celna\POZ\GCG\..NOWE DO AWIZACJI\INNE\" + newFileName;
                File.Move(fileName, newPath);
                listView1.Items.Remove(listView1.SelectedItems[0]);
            }
            //all files wchich contain selected text in textbox will be moved to the C:\Users\Mkzz\Desktop\doki\Test2 folder with the same name as original file.
            if (materialTextBox21.Text != "")
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    string fileName = item.SubItems[1].Text;
                    string[] fileName2 = fileName.Split('\\');
                    string newFileName = fileName2[fileName2.Length - 1];
                    string newPath = @"L:\Odprawa_celna\POZ\GCG\..NOWE DO AWIZACJI\INNE\" + newFileName;
                    if (newFileName.Contains(materialTextBox21.Text))
                    {
                        File.Move(fileName, newPath);
                        listView1.Items.Remove(item);
                    }
                }
            }
            //next item in listview will be selected
            if (listView1.Items.Count > 0)
            {
                listView1.Items[0].Selected = true;
                listView1.Items[0].EnsureVisible();
            }
        }
    }
}
