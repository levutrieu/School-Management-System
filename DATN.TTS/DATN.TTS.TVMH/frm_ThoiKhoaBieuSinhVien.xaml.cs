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
using System.Windows.Shapes;
using DATN.TTS.BUS;
using DATN.TTS.TVMH.Resource;
using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Functions;
using DevExpress.Utils;
using DevExpress.XtraSpreadsheet;
using Color = System.Drawing.Color;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_ThoiKhoaBieuSinhVien.xaml
    /// </summary>
    public partial class frm_ThoiKhoaBieuSinhVien : Page
    {
        bus_DangKyHocPhan client = new bus_DangKyHocPhan();
        private IWorkbook workbook = null;
        private Worksheet worksheet = null;
        private Range range = null;
        private DataTable iDataSoure = null;
        public frm_ThoiKhoaBieuSinhVien()
        {
            InitializeComponent();
            #region Excels
            XepTKB.ShowTabSelector = false;
            XepTKB.Options.Behavior.Selection.AllowExtendSelection = true;
            XepTKB.Options.Behavior.Selection.AllowMultiSelection = true;
            this.XepTKB.Options.Behavior.ShowPopupMenu = DocumentCapability.Hidden;
            XepTKB.ActiveWorksheet.Rows.Hide(15, 1048576 - 1);
            XepTKB.ActiveWorksheet.Columns.Hide(9, 16384 - 1);
            XepTKB.AllowDrop = false;
            XepTKB.Focusable = false; 
            #endregion
            LoadHeader();
            this.iDataSoure = TableSchemaBinding();
            this.DataContext = iDataSoure;
            SetComBobox();
            SetComboTuanTheoNamHoc();
        }

        private void LoadHeader()
        {
            try
            {
                #region Create Header
                XepTKB.BeginUpdate();
                workbook = XepTKB.Document;
                worksheet = workbook.Worksheets[0];
                workbook.Worksheets[0].ActiveView.ShowHeadings = false;
                XepTKB.ReadOnly = true;
                worksheet.Columns[0].Width = 70;
                worksheet.Columns[1].Width = 90;
                worksheet.Columns[2].Width = 140;
                worksheet.Columns[3].Width = 140;
                worksheet.Columns[4].Width = 140;
                worksheet.Columns[5].Width = 140;
                worksheet.Columns[6].Width = 140;
                worksheet.Columns[7].Width = 140;
                worksheet.Columns[8].Width = 140;

                range = worksheet.Range["A1:A1"];
                range.Value = "Ca";
                range.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                range.Font.Bold = true;
                range.Font.Color = DXColor.Red;
                range.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

                range = worksheet.Range["A2:A6"];
                range.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                range.Font.Bold = true;
                range.Font.Color = DXColor.Red;
                range.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                range.Merge();
                range.Value = "S";

                range = worksheet.Range["A7:A11"];
                range.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                range.Font.Bold = true;
                range.Font.Color = DXColor.Red;
                range.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                range.Merge();
                range.Value = "C";

                range = worksheet.Range["A12:A15"];
                range.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                range.Font.Bold = true;
                range.Font.Color = DXColor.Red;
                range.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                range.Merge();
                range.Value = "T";

                #region Tiet hoc

                range = worksheet.Range["B1:B1"];
                range.Value = "Tiết";

                range = worksheet.Range["B2:B2"];
                range.Value = "1";
                range.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                range.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;

                range = worksheet.Range["B3:B3"];
                range.Value = "2";
                range.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                range.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;

                range = worksheet.Range["B4:B4"];
                range.Value = "3";
                range.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                range.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;

                range = worksheet.Range["B5:B5"];
                range.Value = "4";
                range.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                range.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;

                range = worksheet.Range["B6:B6"];
                range.Value = "5";
                range.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                range.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;

                range = worksheet.Range["B7:B7"];
                range.Value = "6";
                range.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                range.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;

                range = worksheet.Range["B8:B8"];
                range.Value = "7";
                range.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                range.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;

                range = worksheet.Range["B9:B9"];
                range.Value = "8";
                range.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                range.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;

                range = worksheet.Range["B10:B10"];
                range.Value = "9";
                range.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                range.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;

                range = worksheet.Range["B11:B11"];
                range.Value = "10";
                range.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                range.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;

                range = worksheet.Range["B12:B12"];
                range.Value = "11";
                range.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                range.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;

                range = worksheet.Range["B13:B13"];
                range.Value = "12";
                range.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                range.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;

                range = worksheet.Range["B14:B14"];
                range.Value = "13";
                range.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                range.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;

                range = worksheet.Range["B15:B15"];
                range.Value = "14";
                range.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                range.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;

                #endregion

                range = worksheet.Range["C1:C1"];
                range.Value = "Thứ 2";

                range = worksheet.Range["D1:D1"];
                range.Value = "Thứ 3";

                range = worksheet.Range["E1:E1"];
                range.Value = "Thứ 4";

                range = worksheet.Range["F1:F1"];
                range.Value = "Thứ 5";

                range = worksheet.Range["G1:G1"];
                range.Value = "Thứ 6";

                range = worksheet.Range["H1:H1"];
                range.Value = "Thứ 7";

                range = worksheet.Range["I1:I1"];
                range.Value = "Chủ nhật";

                #region Format
                //range = worksheet.Range["A1:B15"];

                Formatting rangeFormatting = range.BeginUpdateFormatting();

                rangeFormatting.Font.Color = Color.Black;
                rangeFormatting.Font.Size = 10;
                rangeFormatting.Font.FontStyle = SpreadsheetFontStyle.Regular;

                rangeFormatting.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                rangeFormatting.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;

                range = worksheet.Range["C1:I1"];
                range.ColumnWidth = 480;
                range = worksheet.Range["C2:C15"];
                range.RowHeight = 120;

                range = worksheet.Range["A1:I15"];

                range.EndUpdateFormatting(rangeFormatting);

                range = worksheet.Range["A1:I1"];

                rangeFormatting = range.BeginUpdateFormatting();

                rangeFormatting.Font.Color = Color.Black;
                rangeFormatting.Font.Size = 12;
                rangeFormatting.Font.FontStyle = SpreadsheetFontStyle.Regular;

                rangeFormatting.Fill.BackgroundColor = Color.Silver;


                rangeFormatting.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                rangeFormatting.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;

                range.EndUpdateFormatting(rangeFormatting);
                XepTKB.EndUpdate();


                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        DataTable TableSchemaBinding()
        {
            try
            {
                Dictionary<string, Type> dic = new Dictionary<string, Type>();
                dic.Add("ID_NAMHOC_HKY_HTAI", typeof(int));
                dic.Add("NAME_TUAN", typeof(string));
                dic.Add("TUAN", typeof(int));

                DataTable dt = null;
                dt = TableUtil.ConvertToTable(dic);
                return dt;
            }
            catch (Exception err)
            {
               throw err;
            }
        }

        void SetComBobox()
        {
            DataTable dt = client.GetHocKyNamHoc();
            if (dt.Rows.Count > 0)
            {
                ComboBoxUtil.SetComboBoxEdit(cboXemTKB_HK, "TEN_HOKY_NH", "ID_NAMHOC_HKY_HTAI", dt, SelectionTypeEnum.Native);
                this.iDataSoure.Rows[0]["ID_NAMHOC_HKY_HTAI"] = cboXemTKB_HK.GetKeyValue(0);
            }
        }

        void SetComboTuanTheoNamHoc()
        {
            try
            {
                DataTable dt = client.GetNgay_SoTuan();
                DataTable xdt = new DataTable();
                xdt.Columns.Add("TUAN", typeof (int));
                xdt.Columns.Add("NAME_TUAN", typeof (string));
                int count = 0;
                int sotuan = Convert.ToInt32(dt.Rows[0]["SO_TUAN"].ToString());
                DateTime ngay = Convert.ToDateTime(dt.Rows[0]["NGAY_BATDAU"].ToString());
                string datetemp = ngay.ToShortDateString();
                for (int i = 1; i <= 5; i++)
                {
                    count++;
                    DateTime addday = ngay.AddDays(count*7);
                    DataRow r = xdt.NewRow();
                    r["TUAN"] = count;
                    r["NAME_TUAN"] = "Tuần " + count + " [Từ " + datetemp + "  Đến " + addday.ToShortDateString() +"]";
                    datetemp = addday.AddDays(1).ToShortDateString();

                    xdt.Rows.Add(r);
                    xdt.AcceptChanges();
                }
                if (xdt.Rows.Count > 0)
                {
                    ComboBoxUtil.SetComboBoxEdit(this.cboXemTuan, "NAME_TUAN", "TUAN", xdt, SelectionTypeEnum.Native);
                    this.iDataSoure.Rows[0]["TUAN"] = cboXemTuan.GetKeyValue(0);
                }
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}
