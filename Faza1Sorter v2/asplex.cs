using MaterialSkin;
using MaterialSkin.Controls;
using Excel = Microsoft.Office.Interop.Excel;

namespace Faza1Sorter_v2
{
    public partial class asplex : MaterialForm
    {
        public asplex()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            //this button will open filebrowserdialog, allow multiselect. Import all .xls files to listview and add them to list
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter = "Excel files (*.xls)|*.xls";
            openFileDialog1.Title = "Select a file";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in openFileDialog1.FileNames)
                {
                    listView1.Items.Add(file);
                }
            }
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            //this button will convert all files from .xls files in listview1 to .xlsx format. Save them with original names in the same folder
            //if listview is empty show messagebox
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("Brak plików do konwersji");
            }
            else
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    Excel.Application xlApp = new Excel.Application();
                    Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(item.Text);
                    xlWorkbook.SaveAs(item.Text.Replace(".xls", ".xlsx"), Excel.XlFileFormat.xlOpenXMLWorkbook);
                    xlWorkbook.Close();
                    xlApp.Quit();
                }
            }
        }
    }
}
