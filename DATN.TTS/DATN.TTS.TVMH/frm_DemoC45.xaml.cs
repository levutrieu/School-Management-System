using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DATN.TTS.BUS;
using DevExpress.Utils;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_DemoC45.xaml
    /// </summary>
    public partial class frm_DemoC45 : Page
    {
        private DataTable iDataSoure = null;
        private DataTable iGridDataSoure = null;
        bus_C45 client = new bus_C45();
        public frm_DemoC45()
        {
            InitializeComponent();
            InitGrid();
        }

        private void InitGrid()
        {
            GridColumn col;

            col = new GridColumn();

            col = new GridColumn();
            col.FieldName = "MonHoc";
            col.Header = "Môn";
            col.Width = 250;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.EditSettings = new TextEditSettings();
            col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
            grdMonHoc.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "Diem";
            col.Header = "Diem";
            col.Width = 250;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            grdMonHoc.Columns.Add(col);
        }

        private void BtnThuHien_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void BtnLoadDuLieu_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable sinhvien = client.GetSinhVien();
                DataTable xdt = client.GetMonHocForC45();
                DataTable dt = new DataTable();
                foreach (DataRow r in xdt.Rows)
                {
                    dt.Columns.Add(r[0].ToString());
                }
                for(int j = 1; j <=xdt.Columns.Count - 1; j++)
                {
                    DataRow r = dt.NewRow();
                    for (int i = 0; i <= xdt.Rows.Count - 1; i++)
                    {
                        r[i] = xdt.Rows[i][j].ToString();
                    }
                    dt.Rows.Add(r);
                }
                
            }
            catch (Exception)
            {
                throw;
            }
        }


        DataTable ReverDataTable(DataTable idata)
        {
            DataTable dt = new DataTable();




            return dt;
        }

        private DataTable GenerateTransposedTable(DataTable inputTable)
        {
            DataTable outputTable = new DataTable();

            foreach (DataRow inRow in inputTable.Rows)
            {
                string newColName = inRow[0].ToString();
                  outputTable.Columns.Add(newColName);
            }
       
            for (int rCount = 1; rCount <= inputTable.Columns.Count - 1; rCount++)
            {
                DataRow newRow = outputTable.NewRow();
                for (int cCount = 0; cCount <= inputTable.Rows.Count - 1; cCount++)
                {
                    string colValue = inputTable.Rows[cCount][rCount].ToString();
                    newRow[cCount] = colValue;
                }
                outputTable.Rows.Add(newRow);
            }

            return outputTable;
        }
    }
}
