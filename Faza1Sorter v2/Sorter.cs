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
                files = Directory.GetFiles(folderPath, "*");
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
            //remove thumbs.db from checkedlistbox
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.Items[i].ToString() == "Thumbs.db")
                {
                    checkedListBox1.Items.RemoveAt(i);
                }
            }
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
            if (textBox1.Text == String.Empty)
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
            else if (checkedListBox1.CheckedItems.Count < 1)
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
                //Remove moved files from checkedlistbox1
                for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
                {
                    checkedListBox1.Items.Remove(checkedListBox1.CheckedItems[i]);
                }
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
                    //if folderLocation NumOfWork is equal or bigger to LimitOfWork from database, remove from list
                    for (int v = 0; v < folderLocations.Count; v++)
                    {
                        string query1 = "SELECT NumOfWork, LimitOfWork FROM Workers WHERE FolderLocation = '" + folderLocations[v] + "'";
                        SQLiteCommand cmd1 = new SQLiteCommand(query1, con);
                        SQLiteDataReader reader1 = cmd1.ExecuteReader();
                        while (reader1.Read())
                        {
                            if (Convert.ToInt32(reader1["NumOfWork"]) >= Convert.ToInt32(reader1["LimitOfWork"]))
                            {
                                folderLocations.Remove(folderLocations[v]);
                            }
                        }
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
                        }
                    }
                }
                //NumOfWork says how much every worker has cases to do. One Case is all files that starts with the same 10 charachters in name. LimitOfWork is how many cases worker can do.
                //If any folder has more cases than LimitOfWork, move cases between other folders untill all folders have less than LimitOfWork cases.
                for (int i = 0; i < folderLocations.Count; i++)
                {
                    string[] files2 = Directory.GetFiles(folderLocations[i]);
                    List<string> list2 = new List<string>();
                    for (int j = 0; j < files2.Length; j++)
                    {
                        string fileName = Path.GetFileName(files2[j]);
                        list2.Add(fileName.Substring(0, 10));
                    }
                    list2 = list2.Distinct().ToList();
                    SQLiteConnection con1 = new SQLiteConnection(@"Data Source=" + line + ";Integrated Security=True");
                    con1.Open();
                    string query = "SELECT LimitOfWork FROM Workers WHERE Name = '" + checkedNames[i] + "'";
                    SQLiteCommand cmd = new SQLiteCommand(query, con1);
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (list2.Count > Convert.ToInt32(reader["LimitOfWork"]))
                        {
                            while (list2.Count > Convert.ToInt32(reader["LimitOfWork"]))
                            {
                                for (int j = 0; j < files2.Length; j++)
                                {
                                    if (Path.GetFileName(files2[j]).Substring(0, 10) == list2[0])
                                    {
                                        string source = files2[j];
                                        string destination = folderLocations[(i + 1) % folderLocations.Count];
                                        File.Move(source, destination + "\\" + Path.GetFileName(source));
                                    }
                                }
                                list2.RemoveAt(0);
                            }
                        }
                    }
                    con1.Close();
                }
                //count how many cases are in each folder. One case is all files that starts with the same name. Update database with new number of cases.
                for (int i = 0; i < folderLocations.Count; i++)
                {
                    //if folder is empty, remove from list
                    if (Directory.GetFiles(folderLocations[i]).Length == 0)
                    {
                        folderLocations.Remove(folderLocations[i]);
                    }
                    else
                    {
                        string[] files2 = Directory.GetFiles(folderLocations[i]);
                        List<string> list2 = new List<string>();
                        for (int j = 0; j < files2.Length; j++)
                        {
                            string fileName = Path.GetFileName(files2[j]);
                            list2.Add(fileName.Substring(0, 10));
                        }
                        list2 = list2.Distinct().ToList();
                        SQLiteConnection con1 = new SQLiteConnection(@"Data Source=" + line + ";Integrated Security=True");
                        con1.Open();
                        string query = "UPDATE Workers SET NumOfWork = NumOfWork +" + list2.Count + " WHERE FolderLocation = '" + folderLocations[i] + "'";
                        SQLiteCommand cmd = new SQLiteCommand(query, con1);
                        cmd.ExecuteNonQuery();
                        con1.Close();
                    }
                }
                //if folders has bigger NumOfWork than LimitOfWork, take the excess cases and move it back to the folder with textbox1. Move that much cases to equal number NumOfWork and LimitOfWork
                for (int i = 0; i < folderLocations.Count; i++)
                {
                    string[] files2 = Directory.GetFiles(folderLocations[i]);
                    List<string> list2 = new List<string>();
                    for (int j = 0; j < files2.Length; j++)
                    {
                        string fileName = Path.GetFileName(files2[j]);
                        list2.Add(fileName.Substring(0, 10));
                    }
                    list2 = list2.Distinct().ToList();
                    SQLiteConnection con1 = new SQLiteConnection(@"Data Source=" + line + ";Integrated Security=True");
                    con1.Open();
                    string query = "SELECT NumOfWork, LimitOfWork FROM Workers WHERE FolderLocation = '" + folderLocations[i] + "'";
                    SQLiteCommand cmd = new SQLiteCommand(query, con1);
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (Convert.ToInt32(reader["NumOfWork"]) > Convert.ToInt32(reader["LimitOfWork"]))
                        {
                            int excess = Convert.ToInt32(reader["NumOfWork"]) - Convert.ToInt32(reader["LimitOfWork"]);
                            for (int j = 0; j < excess; j++)
                            {
                                for (int k = 0; k < files2.Length; k++)
                                {
                                    if (Path.GetFileName(files2[k]).Substring(0, 10) == list2[0])
                                    {
                                        string source = files2[k];
                                        string destination = textBox1.Text;
                                        File.Move(source, destination + "\\" + Path.GetFileName(source));
                                    }
                                }
                                list2.RemoveAt(0);
                            }
                            string query1 = "UPDATE Workers SET NumOfWork = NumOfWork -" + excess + " WHERE FolderLocation = '" + folderLocations[i] + "'";
                            SQLiteCommand cmd1 = new SQLiteCommand(query1, con1);
                            cmd1.ExecuteNonQuery();
                        }
                    }
                    con1.Close();
                }
                MessageBox.Show("Pliki zostały przeniesione");
                //load all files from textbox1 folder into checkedlistbox1. If folder is empty, show messagebox.
                if (Directory.GetFiles(textBox1.Text).Length == 0)
                {
                    MessageBox.Show("Folder z plikami jest pusty");
                }
                else
                {
                    string[] files9 = Directory.GetFiles(textBox1.Text);
                    List<string> list1 = new List<string>();
                    for (int i = 0; i < files9.Length; i++)
                    {
                        string fileName = Path.GetFileName(files9[i]);
                        list1.Add(fileName);
                    }
                    list1 = list1.Distinct().ToList();
                    checkedListBox1.Items.Clear();
                    for (int i = 0; i < list1.Count; i++)
                    {
                        checkedListBox1.Items.Add(list1[i]);
                    }
                }
                //remove thumbs.db from checkedlistbox
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.Items[i].ToString() == "Thumbs.db")
                    {
                        checkedListBox1.Items.RemoveAt(i);
                    }
                }
            }
        }

        private void convertBttn_Click(object sender, EventArgs e)
        {
            //This button will search for all numerical sequences of 10 digits in the materialMultiLineTextBox21 .It will delete other characters or words and save found numerical sequences from the new line in materialmultilinetextbox21.
            //If there are no sequences, it will show messagebox.
            //if materialmultilinetextbox21 is empty, show messagebox
            if (materialMultiLineTextBox21.Text == "")
            {
                MessageBox.Show("Pusty tekst");
            }
            else
            {
                //create list of strings
                List<string> list = new List<string>();
                //create string array
                string[] lines = materialMultiLineTextBox21.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                //for each line in lines array
                for (int i = 0; i < lines.Length; i++)
                {
                    //create string variable
                    string line = lines[i];
                    //if line is not empty
                    if (line != "")
                    {
                        //create string array
                        string[] words = line.Split(' ');
                        //for each word in words array
                        for (int j = 0; j < words.Length; j++)
                        {
                            //create string variable
                            string word = words[j];
                            //if word is not empty
                            if (word != "")
                            {
                                //if word is a sequence of 10 digits
                                if (word.Length == 10 && word.All(char.IsDigit))
                                {
                                    //add word to list
                                    list.Add(word);
                                    //remove duplicates
                                }
                            }
                        }
                    }
                }
                list = list.Distinct().ToList();
                //if list is empty, show messagebox
                if (list.Count == 0)
                {
                    MessageBox.Show("Nie znaleziono numerów");
                }
                else
                {
                    //for each item in list
                    for (int i = 0; i < list.Count; i++)
                    {
                        //delete all characters from list item
                        list[i] = list[i].Replace(" ", "");
                    }
                    //create string variable
                    string line = "";
                    //for each item in list
                    for (int i = 0; i < list.Count; i++)
                    {
                        //add item to string variable
                        line += list[i] + "\r\n";
                    }
                    //clear material
                    materialMultiLineTextBox21.Text = "";
                    //add string variable to material
                    materialMultiLineTextBox21.Text = line;
                    //remove duplicates from list
                }
            }
        }
    }
}