using MaterialSkin;
using MaterialSkin.Controls;
using System.Data;
using System.Data.SqlClient;

namespace Faza1Sorter_v2
{
    public partial class editUser : MaterialForm
    {
        public editUser()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == String.Empty)
            {
                MessageBox.Show("Wypełnij pole imię!");
            }
            else
            {
                //Update worker wih new values
                SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mkzz\source\repos\Faza1Sorter v4\Faza1Sorter v2\database.mdf;Integrated Security=True");
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Workers SET Name = '" + textBox3.Text + "', FolderLocation = '" + textBox2.Text + "', Limit = '" + numericUpDown1.Text + "', NumOfWork = '" + numericUpDown2.Text + "' WHERE Name = '" + textBox3.Text + "'", con);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Zaktualizowano dane pracownika!");
            }
        }

        private void editUser_Load(object sender, EventArgs e)
        {
            //on load this form will load all data from database.mdf
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mkzz\source\repos\Faza1Sorter v4\Faza1Sorter v2\database.mdf;Integrated Security=True");
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from Workers", con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            if(textBox3.Text == String.Empty)
            {
                MessageBox.Show("Wypełnij pole imię!");
            }
            else
            {
                //this will delete selected worker from database.mdf
                SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mkzz\source\repos\Faza1Sorter v4\Faza1Sorter v2\database.mdf;Integrated Security=True");
                con.Open();
                SqlCommand cmd = new SqlCommand("delete from Workers where Name='" + textBox3.Text + "'", con);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Użytkownik usunięty!");
            }
        }
    }
}
