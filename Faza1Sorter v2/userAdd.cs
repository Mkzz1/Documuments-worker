using MaterialSkin;
using MaterialSkin.Controls;
using System.Data.SQLite;

namespace Faza1Sorter_v2
{
    public partial class userAdd : MaterialForm
    {
        string line = "C:\\SORTER\\database.mdf";
        public userAdd()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            //this button will add a new user to the database. Textbox1 as Name, Textbox2 as FolderLocation, numericupdown as limit
            if (textBox3.Text != "" && textBox2.Text != "")
            {
                string name = textBox3.Text;
                string folder = textBox2.Text;
                int NumOfWork = 0;
                int LimitOfWork = 0;
                //save numericUpDown1 as int LimitOfWork
                if (numericUpDown1.Value != 0)
                {
                    LimitOfWork = (int)numericUpDown1.Value;
                }
                //add those values to sql using sql client. ID will be auto generated like 1, 2, 3,4 etc.
                SQLiteConnection con = new SQLiteConnection(@"Data Source=" + line + ";Integrated Security=True");
                con.Open();
                SQLiteCommand cmd = new SQLiteCommand("INSERT INTO [Workers] ([Name],[FolderLocation],[LimitOfWork],[NumOfWork]) VALUES ('" + name + "','" + folder + "','" + LimitOfWork + "','" + NumOfWork + "')", con);
                cmd.ExecuteNonQuery();
                con.Close();

                //clear the textboxes
                textBox3.Text = "";
                textBox2.Text = "";

                //show a messagebox
                MessageBox.Show("Dodano pracownika");
            }
            else
            {
                MessageBox.Show("Wypełnij wszystkie pola");
            }
        }
    }
}
