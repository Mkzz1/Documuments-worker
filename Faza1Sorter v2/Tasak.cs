using MaterialSkin;
using MaterialSkin.Controls;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;

namespace Faza1Sorter_v2
{
    public partial class Tasak : MaterialForm
    {
        public Tasak()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            //this button will load .pdf file and display its location and name it in the materialTextBox1 
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "PDF Files|*.pdf";
            openFileDialog1.Title = "Select a PDF File";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                materialTextBox1.Text = openFileDialog1.FileName;
            }
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            if(materialTextBox1.ToString() == string.Empty)
            {
                MessageBox.Show("Wybierz plik");
            }
            else
            {
                //this button will split every page of the .pdf file from materialTextBox1 into new file and save it in same folder as original file with original file name + _page_number.pdf
                string fileName = materialTextBox1.Text;
                PdfDocument document = PdfReader.Open(fileName, PdfDocumentOpenMode.Import);
                int pageCount = document.PageCount;
                for (int i = 0; i < pageCount; i++)
                {
                    PdfPage page = document.Pages[i];
                    PdfDocument outputDocument = new PdfDocument();
                    outputDocument.AddPage(page);
                    string outputFileName = fileName.Substring(0, fileName.Length - 4) + "_page_" + i.ToString() + ".pdf";
                    outputDocument.Save(outputFileName);
                }
            }
        }
    }
}
