using MaterialSkin;
using MaterialSkin.Controls;

namespace Faza1Sorter_v2
{
    public partial class settings : MaterialForm
    {
        public settings()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            //if textboxes are empty, show messagebox
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "")
            {
                MessageBox.Show("Wypełnij wszystkie pola!");
            }
            else
            {
                string path = @"C:\SORTER\settings.txt";
                string[] lines = { textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, };
                System.IO.File.WriteAllLines(path, lines);
            }
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            //open buttons.cs
            buttons b = new buttons();
            b.Show();
        }

        private void ChangeLabels()
        {
            //this function will import names from lines in buttons.txt and change labels text to them. Like label1 = first line of buttons.txt and so on
            string path = @"C:\SORTER\buttons.txt";
            string[] lines = System.IO.File.ReadAllLines(path);
            materialLabel1.Text = lines[0];
            materialLabel2.Text = lines[1];
            materialLabel3.Text = lines[3];
            materialLabel4.Text = lines[2];
            materialLabel6.Text = lines[4];
            materialLabel5.Text = lines[5];
        }

        private void settings_Load(object sender, EventArgs e)
        {
            ChangeLabels();
        }
    }
}
