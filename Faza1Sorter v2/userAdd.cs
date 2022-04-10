using MaterialSkin;
using MaterialSkin.Controls;
using System.Data.SqlClient;
using System.Linq;

namespace Faza1Sorter_v2
{
    public partial class userAdd : MaterialForm
    {
        string[] files;
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
            if (textBox3.Text != "" && textBox2.Text != "" && numericUpDown1.Value != 0)
            {
                string name = textBox3.Text;
                string folder = textBox2.Text;
                int limit = (int)numericUpDown1.Value;
                int NumOfWork = 0;

                //add those values to sql using sql client. ID will be auto generated like 1, 2, 3,4 etc.
                SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mkzz\source\repos\Faza1Sorter v4\Faza1Sorter v2\database.mdf;Integrated Security=True");
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[Workers] ([Name],[FolderLocation],[Limit],[NumOfWork]) VALUES ('" + name + "','" + folder + "','" + limit + "','" + NumOfWork + "')", con);
                cmd.ExecuteNonQuery();
                con.Close();

                //clear the textboxes
                textBox3.Text = "";
                textBox2.Text = "";
                numericUpDown1.Value = 0;

                //show a messagebox
                MessageBox.Show("Dodano pracownika");

            }
        }
    }
}
