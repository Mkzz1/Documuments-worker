using MaterialSkin;
using MaterialSkin.Controls;

namespace Faza1Sorter_v2
{
    public partial class buttons : MaterialForm
    {
        
        public buttons()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "")
            {
                MessageBox.Show("Wypełnij wszystkie pola!");
            }
            else
            {
                //this button will save values put in textboxes in C:\SORTER\buttons.txt file in new lines. If already exists, it will be overwritten.
                string path = @"C:\SORTER\buttons.txt";
                string[] lines = { textBox1.Text, textBox2.Text, textBox3.Text, textBox6.Text, textBox5.Text, textBox4.Text, };
                System.IO.File.WriteAllLines(path, lines);
            }
        }
    }
}
