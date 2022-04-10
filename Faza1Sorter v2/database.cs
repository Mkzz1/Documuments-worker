using MaterialSkin;
using MaterialSkin.Controls;

namespace Faza1Sorter_v2
{
    public partial class database : MaterialForm
    {
        public database()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        } 
        
        private void materialButton1_Click(object sender, EventArgs e)
        {
            string path = textBox1.Text + "\\" + "database.mdf";
            lineChanger(path, @"C:\SORTER\settings.txt", 6);
            this.Close();
        }

        static void lineChanger(string newText, string fileName, int line_to_edit)
        {
            string[] arrLine = File.ReadAllLines(fileName);
            arrLine[6] = newText;
            File.WriteAllLines(fileName, arrLine);
        }
    }
}
