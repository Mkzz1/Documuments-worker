﻿using MaterialSkin;
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
            if (textBox3.Text == String.Empty)
            {
                MessageBox.Show("Wypełnij pole imię!");
            }
            else
            {
                //Update worker wih new values
                SQLiteConnection con = new SQLiteConnection(@"Data Source=" + line + ";Integrated Security=True");
                con.Open();
                SQLiteCommand cmd = new SQLiteCommand("UPDATE Workers SET Name = '" + textBox3.Text + "', FolderLocation = '" + textBox2.Text + "', NumOfWork = '" + numericUpDown2.Text + "' WHERE Name = '" + textBox3.Text + "'", con);
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
    }
}
