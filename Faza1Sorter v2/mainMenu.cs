using MaterialSkin;
using MaterialSkin.Controls;

namespace Faza1Sorter_v2
{
    public partial class mainMenu : MaterialForm
    {
        public mainMenu()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            //this button will open Form1.cs
            Form1 form1 = new Form1();
            form1.Show();
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            //this button will open Sorter.cs
            Sorter sorter = new Sorter();
            sorter.Show();
        }

        private void materialButton4_Click(object sender, EventArgs e)
        {
            //this button will open Tasak.cs
            Tasak tasak = new Tasak();
            tasak.Show();
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            //this button will open asplex.cs
            asplex asplex = new asplex();
            asplex.Show();
        }

        private void materialButton5_Click(object sender, EventArgs e)
        {
            //this button will open PDFtoXLSX.cs
            PDFtoXLSX pdf = new PDFtoXLSX();
            pdf.Show();
        }
    }
}
