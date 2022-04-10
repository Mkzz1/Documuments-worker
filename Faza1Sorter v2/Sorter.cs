using MaterialSkin;
using MaterialSkin.Controls;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Faza1Sorter_v2
{
    public partial class Sorter : MaterialForm
    {
        string[] files;
        string line = File.ReadLines(@"C:\SORTER\settings.txt").Skip(6).Take(1).First();
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
            else if (checkedListBox1.Items.Count == 0)
            {
                MessageBox.Show("Wybierz pliki");
            }
            else
            {
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

        private void materialButton7_Click(object sender, EventArgs e)
        {
            
        }

        private void materialButton4_Click(object sender, EventArgs e)
        {
            //this button will move checked in checkedlistbox1 files to FolderLocation from database
            //checkedlistbox2 show Name from database, find FolderLocation assigned to Name
            //move files to FolderLocation
            //remove files from checkedlistbox1
            if (checkedListBox2.CheckedItems.Count > 1)
            {
                MessageBox.Show("Wybierz tylko jedną pozycję");
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
                SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename="+line+";Integrated Security=True");
                con.Open();
                //get checked Name as string from database
                string checkedName = "";
                for (int i = 0; i < checkedListBox2.CheckedItems.Count; i++)
                {
                    checkedName += checkedListBox2.CheckedItems[i].ToString();
                }
                //get FolderLocation from database
                string query = "SELECT FolderLocation FROM Workers WHERE Name = '" + checkedName + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
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
                int count = 0;
                foreach (string file in Directory.GetFiles(folderLocation))
                {
                    string fileName = Path.GetFileName(file);
                    if (fileName.Substring(0, 10) == fileName.Substring(0, 10))
                    {
                        count++;
                    }
                }
                //Add count to database in NumOfWork
                query = "UPDATE Workers SET NumOfWork = NumOfWork + " + count + " WHERE Name = '" + checkedName + "'";
                cmd = new SqlCommand(query, con);
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

        private void materialButton9_Click(object sender, EventArgs e)
        {
            //This button will move checkedlisbox1 checked files between folders on checkedlistbox2
        }

        private void ImportWorkersToList()
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + line + ";Integrated Security=True");
            con.Open();
            SqlCommand command = new SqlCommand("SELECT Name FROM Workers", con);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                checkedListBox2.Items.Add(reader["Name"].ToString());
            }
            reader.Close();
            con.Close();
        }

        private void Sorter_Load(object sender, EventArgs e)
        {
            ImportWorkersToList();
            CheckSettings();
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            //open database.cs
            database database = new database();
            database.Show();
        }

        private void CheckSettings()
        {
            string path = @"C:\SORTER\settings.txt";
            //check if C:\SORTER\settings.txt exists. If not create it. Write lines on it.
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            //if settings.txt is empty, program will fill display message box with text "Pamiętaj by ustawić ścieżki folderów w ustawieniach!"
            if (File.ReadAllText(path) == "")
            {
                string[] lines = { "1", "2", "3", "4", "5", "6", "1 database" };
                File.WriteAllLines(path, lines);
            }
        }
    }
}
