using MaterialSkin;
using MaterialSkin.Controls;
using System.Data;
using System.Data.SQLite;

namespace Faza1Sorter_v2
{
    public partial class editUser : MaterialForm
    {
        string line = "C:\\SORTER\\database.mdf";
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
            //if textboxes are empty, show message box
            if (textBox3.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Wypełnij wszystkie pola tekstowe!");
            }
            {
                //Update worker wih new values
                SQLiteConnection con = new SQLiteConnection(@"Data Source=" + line + ";Integrated Security=True");
                con.Open();
                SQLiteCommand cmd = new SQLiteCommand("UPDATE Workers SET Name = '" + textBox3.Text + "', FolderLocation = '" + textBox2.Text + "', NumOfWork = '" + numericUpDown2.Text + "', LimitOfWork = '" + numericUpDown1.Text + "' WHERE Name = '" + textBox3.Text + "'", con);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Zaktualizowano dane pracownika!");
            }
        }

        private void editUser_Load(object sender, EventArgs e)
        {
            //on load this form will load all data from database.mdf
            SQLiteConnection con = new SQLiteConnection(@"Data Source=" + line + ";Integrated Security=True");
            con.Open();
            SQLiteCommand cmd = new SQLiteCommand("select * from Workers", con);
            SQLiteDataAdapter sda = new SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
            //change name of columns in datagridview 
            dataGridView1.Columns[0].HeaderText = "Imię";
            dataGridView1.Columns[1].HeaderText = "Folder";
            dataGridView1.Columns[2].HeaderText = "Limit";
            dataGridView1.Columns[3].HeaderText = "Ilość spraw";
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
                SQLiteConnection con = new SQLiteConnection(@"Data Source=" + line + ";Integrated Security=True");
                con.Open();
                SQLiteCommand cmd = new SQLiteCommand("delete from Workers where Name='" + textBox3.Text + "'", con);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Użytkownik usunięty!");
            }
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            //this button will set 0 to NumOfWork to every worker
            SQLiteConnection con = new SQLiteConnection(@"Data Source=" + line + ";Integrated Security=True");
            con.Open();
            SQLiteCommand cmd = new SQLiteCommand("UPDATE Workers SET NumOfWork = '0'", con);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Wyzerowano przydział");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if user click on a cell Name in datagridview, textboxes will load data from selected row
            textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            numericUpDown1.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            numericUpDown2.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            
        }
    }
}
