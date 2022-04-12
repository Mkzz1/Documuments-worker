using MaterialSkin;
using MaterialSkin.Controls;
using System.Data.SQLite;

namespace Faza1Sorter_v2
{
    public partial class Sorter : MaterialForm
    {
        string[] files;
        string line = "C:\\SORTER\\database.mdf";
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
        }

        private void materialButton6_Click(object sender, EventArgs e)
        {
            //open userAdd.cs
            userAdd userAdd = new userAdd();
            userAdd.Show();
        }

        public void materialButton2_Click(object sender, EventArgs e)
        {
            //this button will sort checkedlistbox1 and show only files whose names start with 10 characters specified in textbox.

            if (materialMultiLineTextBox21.ToString() == "")
            {
                MessageBox.Show("Wprowadź nazwę pliku");
            }
            if(textBox1.Text == String.Empty)
            {
                MessageBox.Show("Załaduj pliki");
            }
            else
            {
                //remove last new line from textbox
                string[] characters = materialMultiLineTextBox21.Text.Split("\r\n");
                checkedListBox1.Items.Clear();
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    foreach (string character in characters)
                    {
                        if (fileName.StartsWith(character))
                        {
                            checkedListBox1.Items.Add(fileName);
                        }
                    }
                }
            }
        }

        private void materialButton4_Click(object sender, EventArgs e)
        {
            //this button will move checked in checkedlistbox1 files to FolderLocation from database
            //checkedlistbox2 show Name from database, find FolderLocation assigned to Name
            //move files to FolderLocation
            //remove files from checkedlistbox1
            if (checkedListBox2.CheckedItems.Count > 1)
            {
                MessageBox.Show("Wybierz tylko jednego pracownika");
            }
            else if (checkedListBox1.CheckedItems.Count > 1)
            {
                MessageBox.Show("Nie wybrano plików");
            }
            else 
            {
                //get checked files from checkedlistbox1 in textbox1 folder
                string location = textBox1.Text;
                string[] files = new string[checkedListBox1.CheckedItems.Count];
                for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
                {
                    files[i] = checkedListBox1.CheckedItems[i].ToString();
                }
                //get FolderLocation from database
                SQLiteConnection con = new SQLiteConnection(@"Data Source=" + line + ";Integrated Security=True");
                con.Open();
                //get checked Name as string from database
                string checkedName = "";
                for (int i = 0; i < checkedListBox2.CheckedItems.Count; i++)
                {
                    checkedName += checkedListBox2.CheckedItems[i].ToString();
                }
                //get FolderLocation from database
                string query = "SELECT FolderLocation FROM Workers WHERE Name = '" + checkedName + "'";
                SQLiteCommand cmd = new SQLiteCommand(query, con);
                SQLiteDataReader reader = cmd.ExecuteReader();
                string folderLocation = "";
                while (reader.Read())
                {
                    folderLocation = reader["FolderLocation"].ToString();
                }
                reader.Close();
                //move files to FolderLocation
                for (int i = 0; i < files.Length; i++)
                {
                    string fileName = Path.GetFileName(files[i]);
                    string newFileName = Path.Combine(folderLocation, fileName);
                    File.Move(location + "\\" + files[i], newFileName);
                }
                //Remove moved files from checkedlistbox1
                for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
                {
                    checkedListBox1.Items.Remove(checkedListBox1.CheckedItems[i]);
                }
                //count how many files in folder has the same first ten charachters in their name. save number of files as int
                List<string> list = new List<string>();
                foreach (string file in Directory.GetFiles(folderLocation))
                {
                    string fileName = Path.GetFileName(file);
                    list.Add(fileName.Substring(0, 10));
                }
                list = list.Distinct().ToList();
                //count how many items are in list
                int count = list.Count;
                //add count to database in NumOfWork
                query = "UPDATE Workers SET NumOfWork = NumOfWork + " + count + " WHERE Name = '" + checkedName + "'";
                cmd = new SQLiteCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Pliki zostały przeniesione");
            }
        }

        private void materialButton5_Click(object sender, EventArgs e)
        {
            //open editUser.cs
            editUser editUser = new editUser();
            editUser.Show();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Add first ten charachters from checked files names from checkedlistbox1 to materialMultiLineTextBox21. Separate them by new line.
            //if name duplicate in materialMultiLineTextBox21 add only once
            materialMultiLineTextBox21.Text = "";
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    string fileName = Path.GetFileName(files[i]);
                    if (materialMultiLineTextBox21.Text.Contains(fileName.Substring(0, 10)))
                    {
                        continue;
                    }
                    else
                    {
                        materialMultiLineTextBox21.Text += fileName.Substring(0, 10) + "\r\n";
                    }
                }
            }
        }

        private void ImportWorkersToList()
        {
            SQLiteConnection con = new SQLiteConnection(@"Data Source=" + line + ";Integrated Security=True");
            con.Open();
            SQLiteCommand command = new SQLiteCommand("SELECT Name FROM Workers", con);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                checkedListBox2.Items.Add(reader["Name"].ToString());
            }
            reader.Close();
            con.Close();
        }

        private void Sorter_Load(object sender, EventArgs e)
        {
            CheckSettings();
        }

        private void CheckSettings()
        {
            //this function will check if database.mdf is created in C:\SORTER path. If not, it will create it. With table Workers, columns Name, FolderLocation, Limit, NumOfWork
            //if database.mdf is created, it will check if table Workers is created. If not, it will create it.
            if (!Directory.Exists(@"C:\\SORTER"))
            {
                Directory.CreateDirectory(@"C:\\SORTER");
            }
            //check if database.mdf is created
            if (!File.Exists(line))
            {
                SQLiteConnection.CreateFile(line);
                SQLiteConnection con = new SQLiteConnection(@"Data Source=" + line + ";Integrated Security=True");
                con.Open();
                string query = "CREATE TABLE Workers (Name VARCHAR(50), FolderLocation VARCHAR(550), LimitOfWork INT, NumOfWork INT)";
                SQLiteCommand cmd = new SQLiteCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            else
            {
                ImportWorkersToList();
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

        private void materialButton3_Click(object sender, EventArgs e)
        {
            //if checkedlistbox1 no items checked, show messagebox
            if (checkedListBox1.CheckedItems.Count == 0)
            {
                MessageBox.Show("Nie wybrano żadnych plików");
            }
            else if (checkedListBox2.CheckedItems.Count == 0)
            {
                MessageBox.Show("Nie wybrano folderów");
            }
            else
            {
                //import checked files name from checkedlistbox1 to string list. Import only first ten charachters from checked files name.
                List<string> list = new List<string>();
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemChecked(i))
                    {
                        string fileName = Path.GetFileName(files[i]);
                        list.Add(fileName.Substring(0, 10));
                    }
                }
                list = list.Distinct().ToList();
                //get files from textBox1 folder
                string[] files1 = Directory.GetFiles(textBox1.Text);
                //get checked Names in checkedlistbox 1 as string list
                List<string> checkedNames = new List<string>();
                for (int b = 0; b < checkedListBox2.Items.Count; b++)
                {
                    if (checkedListBox2.GetItemChecked(b))
                    {
                        checkedNames.Add(checkedListBox2.Items[b].ToString());
                    }
                }
                for (int c = 0; c < checkedListBox2.CheckedItems.Count; c++)
                {
                    checkedNames[c] = checkedListBox2.CheckedItems[c].ToString();
                }
                //in database find all FolderLocation assigned to checkednames and save them to string list
                List<string> folderLocations = new List<string>();
                SQLiteConnection con = new SQLiteConnection(@"Data Source=" + line + ";Integrated Security=True");
                con.Open();
                for (int d = 0; d < checkedNames.Count; d++)
                {
                    string query = "SELECT FolderLocation FROM Workers WHERE Name = '" + checkedNames[d] + "'";
                    SQLiteCommand cmd = new SQLiteCommand(query, con);
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        folderLocations.Add(reader["FolderLocation"].ToString());
                    }
                }
                con.Close();
                //check if file1 contain fileName from list. If yes, move all files that contain first item to list to first folder, second item to second folder and so on. If folderLocations list comes to an end, start from begining until list ist empty.
                //if multiple folders selected, every folder will get same amount of files.
                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = 0; j < files1.Length; j++)
                    {
                        if (Path.GetFileName(files1[j]).Contains(list[i]))
                        {
                            if (folderLocations.Count == 0)
                            {
                                folderLocations.Add(folderLocations[0]);
                            }
                            //move file to folder
                            string source = files1[j];
                            string destination = folderLocations[i % folderLocations.Count];
                            File.Move(source, destination + "\\" + Path.GetFileName(source));
                            //add +1 to NumOfWork column in database assigned to folderLocation each time a file is moved
                            SQLiteConnection con1 = new SQLiteConnection(@"Data Source=" + line + ";Integrated Security=True");
                            con1.Open();
                            string query = "UPDATE Workers SET NumOfWork = NumOfWork + 1 WHERE FolderLocation = '" + folderLocations[i % folderLocations.Count] + "'";
                            SQLiteCommand cmd = new SQLiteCommand(query, con1);
                            cmd.ExecuteNonQuery();
                            con1.Close();
                        }
                    }
                }
                //if all files are moved, show messagebox
                MessageBox.Show("Wszystkie pliki zostały przeniesione");
                //clear every checkedlistbox1 items
                checkedListBox1.Items.Clear();
            }
        }
    }
}
