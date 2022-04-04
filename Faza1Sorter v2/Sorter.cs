using MaterialSkin;
using MaterialSkin.Controls;
using System.Linq;

namespace Faza1Sorter_v2
{
    public partial class Sorter : MaterialForm
    {
        string[] files;
        public Sorter()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }
        public void materialButton1_Click(object sender, EventArgs e)
        {
            //this button will open folderbrowserdialog and select the folder where the files are tif located. Files will be sorted by their names. Files will be shown in the checkedlistbox. Only file name will be shown in the checkedlistbox. File name will be shorten to first 10 characters.
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.ShowNewFolderButton = false;
            folderBrowserDialog1.Description = "Wybierz folder z plikami";
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string folderPath = folderBrowserDialog1.SelectedPath;
                files = Directory.GetFiles(folderPath, "*.TIF");
                checkedListBox1.Items.Clear();
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    checkedListBox1.Items.Add(fileName);
                }
            }

            //if duplicate names are found, remove from checkedlistbox
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                for (int j = i + 1; j < checkedListBox1.Items.Count; j++)
                {
                    if (checkedListBox1.Items[i].ToString() == checkedListBox1.Items[j].ToString())
                    {
                        checkedListBox1.Items.RemoveAt(j);
                    }
                }
            }
            //function will add path to the textbox
            textBox1.Text = folderBrowserDialog1.SelectedPath;
        }

        private void materialCheckbox1_CheckedChanged(object sender, EventArgs e)
        {
            //this checkbox will select all the files in the materialCheckedListBox1
            if (materialCheckbox1.Checked == true)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, true);
                }
            }
            else
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, false);
                }
            }
            //all selected names will be shown in textbox3 separated by comma without spaces
            textBox3.Text = "";
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i) == true)
                {
                    textBox3.Text += checkedListBox1.Items[i].ToString() + ",";
                }
            }
            //remove last comma
            textBox3.Text = textBox3.Text.Remove(textBox3.Text.Length - 1);
        }

        private void materialButton6_Click(object sender, EventArgs e)
        {
            //This button will create a folder with the name of the employee in path from textBox4 with name from textBox2.
            if (textBox4.Text == string.Empty)
            {
                MessageBox.Show("Wskaż foldery pracowników");
            }
            else
            {
                string path = textBox4.Text;
                string folderName = textBox2.Text;
                string folderPath = path + "\\" + folderName;
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                //load all folders from path to checkedlisbox2
                checkedListBox2.Items.Clear();
                string[] folders = Directory.GetDirectories(path);
                foreach (string folder in folders)
                {
                    string folderName2 = Path.GetFileName(folder);
                    checkedListBox2.Items.Add(folderName2);
                }
            }
        }

        public void materialButton2_Click(object sender, EventArgs e)
        {
            //this button will sort the files in the checkedlistbox1 and show only the files that contain characters from materialMultiLineTextBox21 in their name. Comma will be separator
            string[] characters = materialMultiLineTextBox21.Text.Split("\r\n");
            checkedListBox1.Items.Clear();
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                foreach (string character in characters)
                {
                    if (fileName.Contains(character))
                    {
                        checkedListBox1.Items.Add(fileName);
                    }
                }
            }
        }

        private void materialButton7_Click(object sender, EventArgs e)
        {
            //this button will remove all selected folders in checkedlistbox2. Remove also from harddrive
            for (int i = checkedListBox2.Items.Count - 1; i >= 0; i--)
            {
                if (checkedListBox2.GetItemChecked(i))
                {
                    string folderName = checkedListBox2.Items[i].ToString();
                    string folderPath = textBox4.Text + "\\" + folderName;
                    Directory.Delete(folderPath, true);
                    checkedListBox2.Items.RemoveAt(i);
                }
            }
        }

        private void materialButton8_Click(object sender, EventArgs e)
        {
            //this button will open folderbroweserdialog. Load all folders from the selected folder and add them to the chechlist
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.ShowNewFolderButton = false;
            folderBrowserDialog1.Description = "Wybierz folder z pracownikami";
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string folderPath = folderBrowserDialog1.SelectedPath;
                string[] folders = Directory.GetDirectories(folderPath);
                foreach (string folder in folders)
                {
                    checkedListBox2.Items.Add(Path.GetFileName(folder));
                }
            }
            //folder path will be added to the textbox
            textBox4.Text = folderBrowserDialog1.SelectedPath;
        }

        private void materialButton4_Click(object sender, EventArgs e)
        {
            //this button will move checked files to the selected folder. If no folder is selected, a messagebox will pop up. If more than one folder selected a messagebox will pop up.
            if (checkedListBox1.CheckedItems.Count == 0)
            {
                MessageBox.Show("Nie wybrano plików");
            }
            else if (checkedListBox2.CheckedItems.Count > 1)
            {
                MessageBox.Show("Wybrano więcej niż jeden folder");
            }
            else if(checkedListBox2.CheckedItems.Count == 0)
            {
                MessageBox.Show("Nie wybrano folderu");
            }
            else
            {
                string folderPath = textBox1.Text;
                string workerFolder = textBox4.Text;
                string folderName = checkedListBox2.SelectedItem.ToString();
                string folderPath2 = workerFolder + "\\" + folderName;
                foreach (string file in checkedListBox1.CheckedItems)
                {
                    string filePath = folderPath + "\\" + file;
                    string filePath2 = folderPath2 + "\\" + file;
                    File.Move(filePath, filePath2);
                }
                //moved files will be removed from the checkedlistbox
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemChecked(i))
                    {
                        checkedListBox1.Items.RemoveAt(i);
                    }
                }
            }
        }

        private void materialButton10_Click(object sender, EventArgs e)
        {
            //this button will open mainMenu.cs
            mainMenu mainMenu = new mainMenu();
            mainMenu.Show();
            this.Hide();
        }

        private void materialCheckbox2_CheckedChanged_1(object sender, EventArgs e)
        {
            //this checkbox will check all values in checkedlistbox2
            for (int i = 0; i < checkedListBox2.Items.Count; i++)
            {
                checkedListBox2.SetItemChecked(i, true);
            }
        }

        private void materialButton5_Click(object sender, EventArgs e)
        {
            //this button will change name of checked folder in checkedlistbox2 to name from textBox2
            if(textBox2.Text == string.Empty)
            {
                MessageBox.Show("Nie wpisano nazwy");
            }
            else if (textBox4.Text == string.Empty)
            {
                MessageBox.Show("Nie wybrano ściezki folderów");
            }
            else if (checkedListBox2.CheckedItems.Count == 0)
            {
                MessageBox.Show("Nie wybrano folderów");
            }
            else if (checkedListBox2.CheckedItems.Count > 1)
            {
                MessageBox.Show("Wybrano więcej niż jeden folder");
            }
            else
            {
                string folderPath = textBox4.Text;
                string folderName = checkedListBox2.SelectedItem.ToString();
                string folderPath2 = folderPath + "\\" + folderName;
                string newFolderName = textBox2.Text;
                string newFolderPath = folderPath + "\\" + newFolderName;
                Directory.Move(folderPath2, newFolderPath);
                checkedListBox2.Items.Remove(folderName);
                checkedListBox2.Items.Add(newFolderName);
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if item checked in checkbox1 it will print selected name in textBox3
            //checked items will be separated by a comma
            if (checkedListBox1.CheckedItems.Count == 0)
            {
                textBox3.Text = "";
            }
            else if (checkedListBox1.CheckedItems.Count > 1)
            {
                textBox3.Text = "";
                for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
                {
                    textBox3.Text += checkedListBox1.CheckedItems[i].ToString() + ",";
                }
            }
            else
            {
                textBox3.Text = checkedListBox1.CheckedItems[0].ToString();
            }
            //if selected more than one item in checkbox1 remove last comma
            if (textBox3.Text.Length > 10)
            {
                textBox3.Text = textBox3.Text.Remove(textBox3.Text.Length - 1);
            }
        }

        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if item checked in checkbox1 it will print selected name in textBox3
            //checked items will be separated by a comma
            if (checkedListBox2.CheckedItems.Count == 0)
            {
                textBox5.Text = "";
            }
            else if (checkedListBox2.CheckedItems.Count > 1)
            {
                textBox5.Text = "";
                for (int i = 0; i < checkedListBox2.CheckedItems.Count; i++)
                {
                    textBox5.Text += checkedListBox2.CheckedItems[i].ToString() + ",";
                }
            }
            else
            {
                textBox5.Text = checkedListBox2.CheckedItems[0].ToString();
            }
            //remove comma at the end of textbox5
            if (textBox5.Text.Length > 1)
            {
                textBox5.Text = textBox5.Text.Remove(textBox5.Text.Length - 1);
            }
        }

        private void materialButton9_Click(object sender, EventArgs e)
        {
            //This button will split documents checked in checkedlistbox1 that contains selected charachters in their names between folders in checkedlistbox2. Each folder will get an equal number of files. Each folder will get different files.
            if (checkedListBox1.CheckedItems.Count == 0)
            {
                MessageBox.Show("Nie wybrano plików");
            }
            else if (checkedListBox2.CheckedItems.Count == 0)
            {
                MessageBox.Show("Nie wybrano folderów");
            }
            else
            {
                string folderPath = textBox1.Text;
                string workerFolder = textBox4.Text;
                string folderName = checkedListBox2.CheckedItems.ToString();
                string folderPath2 = workerFolder + "\\" + folderName;
                int filesPerFolder = checkedListBox1.CheckedItems.Count / checkedListBox2.CheckedItems.Count;
                int filesLeft = checkedListBox1.CheckedItems.Count % checkedListBox2.CheckedItems.Count;
                int fileIndex = 0;
                foreach (string folder in checkedListBox2.CheckedItems)
                {
                    string folderPath3 = workerFolder + "\\" + folder;
                    for (int i = 0; i < filesPerFolder; i++)
                    {
                        string filePath = folderPath + "\\" + checkedListBox1.CheckedItems[fileIndex];
                        string filePath2 = folderPath3 + "\\" + checkedListBox1.CheckedItems[fileIndex];
                        File.Move(filePath, filePath2);
                        fileIndex++;
                    }
                }
                for (int i = 0; i < filesLeft; i++)
                {
                    string filePath = folderPath + "\\" + checkedListBox1.CheckedItems[fileIndex];
                    string filePath2 = folderPath2 + "\\" + checkedListBox1.CheckedItems[fileIndex];
                    File.Move(filePath, filePath2);
                    fileIndex++;
                }
                //moved files will be removed from the checkedlistbox
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemChecked(i))
                    {
                        checkedListBox1.Items.RemoveAt(i);
                    }
                }            }
            MessageBox.Show("Zakończono");
        }
    }
}
