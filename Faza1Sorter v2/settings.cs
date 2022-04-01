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
            //this button will save values put in textboxes in C:\SORTER\settings.txt file in new lines. If already exists, it will be overwritten.
            string path = @"C:\SORTER\settings.txt";
            string[] lines = { textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text };
            System.IO.File.WriteAllLines(path, lines);
            this.Close();
        }
    }
}
