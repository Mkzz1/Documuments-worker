using SautinSoft;
using MaterialSkin;
using MaterialSkin.Controls;

namespace Faza1Sorter_v2
{
    public partial class PDFtoXLSX : MaterialForm
    {
        public PDFtoXLSX()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            //this button will open the file explorer and allow the user to select a multiple files. Add the files with their location to the listview. Allow only PDF.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Multiselect = false;
            openFileDialog1.Filter = "PDF Files|*.pdf";
            openFileDialog1.Title = "Select a file";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //add the files to the textbox
                materialTextBox1.Text = openFileDialog1.FileName;
            }
        }
        
        private void materialButton2_Click(object sender, EventArgs e)
        {
            string path = materialTextBox1.Text;
            //get default Downloads folder as string
            string downloadsPath = Environment.ExpandEnvironmentVariables("%userprofile%/downloads/");

            if(materialTextBox1.Text == string.Empty)
            {
                MessageBox.Show("Wybierz plik PDF");
            }
            else
            {
                PdfFocus f = new PdfFocus();
                f.OpenPdf(path);
                //get file name of the pdf
                string fileName = System.IO.Path.GetFileName(path);
                f.ToExcel(downloadsPath + "\\" + fileName + ".xls");
            }
        }
    }
}
