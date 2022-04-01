using MaterialSkin;
using MaterialSkin.Controls;

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
            LoadDB();
        }

        //This function will create a database that will record the employee's name and folder location
        private void createDatabase()
        {
            string path = @"C:\Users\Public\Documents\Faza1Sorter.txt";
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("");
                }
            }
        }
        private void Sorter_Load(object sender, EventArgs e)
        {
            
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
                    checkedListBox1.Items.Add(fileName.Substring(0, 10));
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
        }

        private void materialButton6_Click(object sender, EventArgs e)
        {
            //This button will create a folder with the name of the employee in path from textbox4 and show it in the checkedlistbox2
            string path = textBox4.Text;
            string folderName = textBox3.Text;
            string folderPath = path + "\\" + folderName;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            checkedListBox2.Items.Clear();
            checkedListBox2.Items.Add(folderName);
        }

        public void materialButton2_Click(object sender, EventArgs e)
        {
            //this button will sort the files in the checkedlistbox1 and show only the files that contain characters from materialMultiLineTextBox21 in their name. Comma will be separator
            string[] characters = materialMultiLineTextBox21.Text.Split(',');
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
            //this button will remove the selected worker from the database and remove them from the checkedListBox
            string path = @"settings.txt";
            string[] lines = File.ReadAllLines(path);
            string[] newLines = new string[lines.Length - 1];
            int index = 0;
            foreach (string line in lines)
            {
                if (line != checkedListBox2.SelectedItem.ToString())
                {
                    newLines[index] = line;
                    index++;
                }
            }
            File.WriteAllLines(path, newLines);
            checkedListBox2.Items.Remove(checkedListBox2.SelectedItem);
        }
        //create a function that will read database on startup and load workers into the checkedListBox
        private void LoadDB()
        {
            string path = @"settings.txt";
            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                checkedListBox2.Items.Add(line);
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

        private void materialButton9_Click(object sender, EventArgs e)
        {
            //this button will remove new lines and change them to commas
            string search = materialMultiLineTextBox21.Text;
            string[] searchArray = search.Split('\n');
            string newSearch = "";
            foreach (string word in searchArray)
            {
                newSearch += word + ",";
            }
            materialMultiLineTextBox21.Text = newSearch;
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

        private void materialButton3_Click(object sender, EventArgs e)
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
                }
                //count how many files are in each folder and show in listview.
                //foreach (string folder in checkedListBox2.CheckedItems)
                //{
                //    string folderPath3 = workerFolder + "\\" + folder;
                //    int filesInFolder = Directory.GetFiles(folderPath3).Length;
                //    ListViewItem item = new ListViewItem(folder);
                //    item.SubItems.Add(filesInFolder.ToString());
                //    materialListView1.Items.Add(item);
                //}
            }
            MessageBox.Show("Zakończono");
        }

        private void materialCheckbox2_CheckedChanged(object sender, EventArgs e)
        {
            //this will check all values in checkedlistbox
            for (int i = 0; i < checkedListBox2.Items.Count; i++)
            {
                checkedListBox2.SetItemChecked(i, true);
            }
        }
    }
}
