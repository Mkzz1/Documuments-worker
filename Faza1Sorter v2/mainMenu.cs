using MaterialSkin;
using MaterialSkin.Controls;
using PdfSharp.Pdf;
using PdfSharp.Drawing;

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
        void DrawImage(XGraphics gfx, string jpegSamplePath, int x, int y, int width, int height)
        {
            XImage image = XImage.FromFile(jpegSamplePath);
            gfx.DrawImage(image, x, y, width, height);
        }
        
        private void materialButton6_Click(object sender, EventArgs e)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            //this button will convert all images from listview to PDF and save them in the same folder as the original images using PdfSharp
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "JPEG files (*.jpg)|*.jpg|PNG files (*.png)|*.png|BMP files (*.bmp)|*.bmp|All files (*.*)|*.*";
            fileDialog.Multiselect = true;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                PdfDocument document = new PdfDocument();
                document.Info.Title = "Skonwertowany plik";

                foreach (string fileSpec in fileDialog.FileNames)
                {
                    PdfPage page = document.AddPage();
                    XGraphics gfx = XGraphics.FromPdfPage(page);
                    DrawImage(gfx, fileSpec, 0, 0, (int)page.Width, (int)page.Height);
                }
                string downloadsPath = Environment.ExpandEnvironmentVariables("%userprofile%/downloads/");
                if (document.PageCount > 0) document.Save(downloadsPath + "SkonwertowanyPlik.pdf");
            }
        }
    }
}
