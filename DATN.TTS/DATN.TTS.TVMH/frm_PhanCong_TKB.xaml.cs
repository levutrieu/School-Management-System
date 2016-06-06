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
using CustomMessage;
using DATN.TTS.BUS;
using DATN.TTS.BUS.Resource;
using DevExpress.Spreadsheet;
using DevExpress.Utils;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using DevExpress.XtraSpreadsheet;
using Color = System.Drawing.Color;
using Range = DevExpress.Spreadsheet.Range;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_PhanCong_TKB.xaml
    /// </summary>
    public partial class frm_PhanCong_TKB : Page
    {
        bus_PhanCong_TKB client = new bus_PhanCong_TKB();
        private IWorkbook workbook = null;
        private Worksheet worksheet = null;
        private Range range = null;
        private Range rangeClear = null;
        private DataTable iDataSoure = null;
        private DataTable iGridDataSoure = null;
        private DataTable iDataSoureHPCT = null;
        private DataTable iDataXepTKB = null;
        public bool flagsave = true;
        //private DataTable iDataComBo = null;
        private int pID_LOP_HOCPHAN_CT = 0;

        public frm_PhanCong_TKB()
        {
            InitializeComponent();
            #region Load default
            this.iDataSoure = TableSchemaBinding();
            DataContext = iDataSoure;
            this.iDataSoure.Rows[0]["USER"] = UserCommon.UserName;
            #endregion

            #region Load Function
            LoadHeader();
            SetComBo();
            InitGrid();
            #endregion

            #region worksheets
            range = worksheet.GetUsedRange();
            XepTKB.ShowTabSelector = false;
            XepTKB.Options.Behavior.Selection.AllowExtendSelection = true;
            XepTKB.Options.Behavior.Selection.AllowMultiSelection = true;
            this.XepTKB.Options.Behavior.ShowPopupMenu = DocumentCapability.Hidden;
            XepTKB.ActiveWorksheet.Rows.Hide(15, 1048576 - 1);
            XepTKB.ActiveWorksheet.Columns.Hide(9, 16384 - 1);
            XepTKB.ResetLayout();
            #endregion

            #region Create Table for select range in worksheets
            iDataXepTKB = new DataTable();
            iDataXepTKB.Columns.Add("THU");
            iDataXepTKB.Columns.Add("TIET_BD");
            iDataXepTKB.Columns.Add("TIET_KT");
            iDataXepTKB.Columns.Add("SO_TIET");
            #endregion
        }
        //tao header cho tkb
        private void LoadHeader()
        {
            try
            {
                #region Create Header

                XepTKB.BeginUpdate();
                workbook = XepTKB.Document;
                worksheet = workbook.Worksheets[0];
                workbook.Worksheets[0].ActiveView.ShowHeadings = false;

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
                range.ColumnWidth = 385;
                range = worksheet.Range["C2:C15"];
                range.RowHeight = 110;

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
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        void SetComBo()
        {
            DataTable dt = client.GetAllHDT();
            cboHeDT.ItemsSource = dt;
            if (dt.Rows.Count > 0)
                this.iDataSoure.Rows[0]["ID_HE_DAOTAO"] = cboHeDT.GetKeyValue(0);
        }
        //Binding du lieu va datacontex
        DataTable TableSchemaBinding()
        {
            try
            {
                DataTable dt = new DataTable();
                Dictionary<string, Type>dic = new Dictionary<string, Type>();
                dic.Add("ID_KHOAHOC", typeof(int));
                dic.Add("ID_HE_DAOTAO", typeof(int));
                dic.Add("ID_LOPHOCPHAN", typeof(int));
                dic.Add("USER", typeof(string));
                dic.Add("SO_TUAN", typeof(int));
                dic.Add("TIET_BD", typeof(int));
                dic.Add("TIET_KT", typeof(int));
                dic.Add("SO_TIET", typeof(int));
                dic.Add("ID_PHONG", typeof(int));
                dic.Add("THU", typeof(int));
                dic.Add("ID_LOP_HOCPHAN_CTIET", typeof(int));
                dic.Add("SO_TIET_TONG", typeof(int));
                dic.Add("SO_TIET_DASEP", typeof(int));
                dic.Add("SO_TIET_CONLAI", typeof(int));
                dic.Add("SO_TIET_TUAN", typeof(int));
                dt = TableUtil.ConvertToTable(dic);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        void InitGrid()
        {
            GridColumn col = null;
            col = new GridColumn();
            col.FieldName = "ID_LOPHOCPHAN";
            col.Header = string.Empty;
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as System.Windows.Style;
            col.AllowCellMerge = false;
            grdLHP.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "TEN_LOP_HOCPHAN";
            col.Header = "Tên lớp";
            col.Width = 230;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as System.Windows.Style;
            grdLHP.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "MA_LOP_HOCPHAN";
            col.Header = "Mã lớp học phần";
            col.Width = 100;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as System.Windows.Style;
            grdLHP.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "ID_MONHOC";
            col.Header = "ID môn học";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as System.Windows.Style;
            col.AllowCellMerge = false;
            grdLHP.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "MA_MONHOC";
            col.Header = "Mã môn học";
            col.Width = 80;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as System.Windows.Style;
            col.AllowCellMerge = false;
            grdLHP.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "TEN_MONHOC";
            col.Header = "Môn học";
            col.Width = 80;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as System.Windows.Style;
            col.AllowCellMerge = false;
            grdLHP.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "SO_TC";
            col.Header = "STC";
            col.Width = 80;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as System.Windows.Style;
            col.AllowCellMerge = false;
            grdLHP.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "SOTIET";
            col.Header = "Số tiết";
            col.Width = 80;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as System.Windows.Style;
            col.AllowCellMerge = false;
            grdLHP.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "NGAY_BD";
            col.Header = "Ngày BD";
            col.Width = 80;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as System.Windows.Style;
            col.AllowCellMerge = false;
            grdLHP.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "NGAY_KT";
            col.Header = "Ngày KT";
            col.Width = 80;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as System.Windows.Style;
            col.AllowCellMerge = false;
            grdLHP.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "SOLUONG";
            col.Header = "Số lượng SV";
            col.Width = 80;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as System.Windows.Style;
            col.AllowCellMerge = false;
            grdLHP.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "TUAN_BD";
            col.Header = "Tuần BD";
            col.Width = 80;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as System.Windows.Style;
            col.AllowCellMerge = false;
            grdLHP.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "TUAN_KT";
            col.Header = "Tuần KT";
            col.Width = 80;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as System.Windows.Style;
            col.AllowCellMerge = false;
            grdLHP.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "SO_TUAN";
            col.Header = "Số tuần";
            col.Width = 80;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as System.Windows.Style;
            col.AllowCellMerge = false;
            grdLHP.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "SOTIET_LT";
            col.Header = "Số tiết LT";
            col.Width = 80;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as System.Windows.Style;
            col.AllowCellMerge = false;
            grdLHP.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "SOTIET_TH";
            col.Header = "Số tiết TH";
            col.Width = 80;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as System.Windows.Style;
            col.AllowCellMerge = false;
            grdLHP.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "ID_GIANGVIEN";
            col.Header = "ID GV";
            col.Width = 80;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as System.Windows.Style;
            col.AllowCellMerge = false;
            grdLHP.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "MA_GIANGVIEN";
            col.Header = "Mã GV";
            col.Width = 80;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as System.Windows.Style;
            col.AllowCellMerge = false;
            grdLHP.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "TEN_GIANGVIEN";
            col.Header = "Tên GV";
            col.Width = 150;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as System.Windows.Style;
            col.AllowCellMerge = false;
            grdLHP.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "ID_KHOAHOC_NGANH_CTIET";
            col.Header = "ID_KHOAHOC_NGANH_CTIET";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as System.Windows.Style;
            col.AllowCellMerge = false;
            grdLHP.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "ID_HOCKY_NAMHOC";
            col.Header = "ID_HOCKY_NAMHOC";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as System.Windows.Style;
            col.AllowCellMerge = false;
            grdLHP.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "ID_HEDAOTAO";
            col.Header = "ID_HEDAOTAO";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as System.Windows.Style;
            col.AllowCellMerge = false;
            grdLHP.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "ID_LOPHOC";
            col.Header = "ID_LOPHOC";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as System.Windows.Style;
            col.AllowCellMerge = false;
            grdLHP.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "KY_HIEU";
            col.Header = "Ký hiệu MH";
            col.Width = 80;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as System.Windows.Style;
            col.AllowCellMerge = false;
            grdLHP.Columns.Add(col);

            //grdViewLHP.AutoWidth = true;
        }

        private void ClearFormat()
        {
            XepTKB.BeginUpdate();
            rangeClear = worksheet.Range["C2:I15"];
            worksheet.ClearContents(rangeClear);
            worksheet.UnMergeCells(rangeClear);
            rangeClear.ClearFormats();
            XepTKB.EndUpdate();
        }

        #region Chưa dùng đến
        private bool SetMerge(string temp)
        {
            bool res = true;
            List<string> lst = new List<string>();
            lst = Duyet();
            string[] current = temp.Split(':');

            string str = current[0].Substring(0, 1);
            int tx = int.Parse(current[0].Substring(1));
            List<string> list = new List<string>();
            for (int i = 0; i < lst.Count; i++)
            {
                string skip = lst[i].Substring(0, 1);
                if (skip.Equals(str))
                {
                    int c = int.Parse(lst[i].Substring(1));
                    if (c > tx - 1)
                    {
                        list.Add(lst[i].ToString());
                    }
                }
            }

            int count = 0;
            int x = 0;//tetst

            for (int i = 0; i < list.Count; i++)
            {
                int c = int.Parse(list[i].Substring(1).ToString());
                int d = int.Parse(list[i + 1].Substring(1).ToString());
                if ((d - c) == 1)
                {
                    count++;
                }
                if (x - 1 == count)
                {
                    res = true;
                    break;
                }
                if ((d - c) != 1)
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                    CTMessagebox.Show("Số tiết vừa nhập không đủ");
                    res = false;
                    break;
                }
            }

            if (x - 1 > count)
            {
                // base.ShowMessage(MessageType.Information, "Số tiết vừa nhập không đủ");
                res = false;
            }
            XepTKB.EndUpdate();
            return res;
        }

        private string SetColumnRange(string strtemp)
        {
            string res = string.Empty;
            try
            {
                if (strtemp.Contains("2"))
                {
                    res = "C";
                }
                if (strtemp.Contains("3"))
                {
                    res = "D";
                }
                if (strtemp.Contains("4"))
                {
                    res = "E";
                }
                if (strtemp.Contains("5"))
                {
                    res = "F";
                }
                if (strtemp.Contains("6"))
                {
                    res = "G";
                }
                if (strtemp.Contains("7"))
                {
                    res = "H";
                }
                if (strtemp.Contains("8"))
                {
                    res = "I";
                }
                return res;
            }
            catch (Exception ex)
            {
                throw;

            }
        }

        private List<string> Duyet()
        {
            List<string> lst = new List<string>();
            if (range != null)
            {
                range = worksheet.Range["C2:I15"];
            }
            string temp = string.Empty;
            for (int i = 0; i < range.RowCount; i++)
            {
                for (int j = 0; j < range.ColumnCount; j++)
                {
                    if (string.IsNullOrEmpty(range[i, j].Value.ToString()))
                    {
                        temp = range[i, j].GetReferenceA1();
                        lst.Add(temp);
                    }
                }
            }
            return lst;
        }

        private bool KiemTraDuLieu()
        {
            bool res = true;
            foreach (DataRow dr in iDataXepTKB.Rows)
            {
                foreach (DataRow r in iDataSoureHPCT.Rows)
                {
                    if (dr["THU"].ToString() == r["THU"].ToString() && dr["TIET_BD"].ToString() == r["TIET_BD"].ToString() && dr["TIET_KT"].ToString() == r["TIET_KT"].ToString())
                    {
                        res = false;
                        break;
                    }
                }
            }
            return res;
        }
        #endregion

        //get du lieu len excels
        private void SetColumnGrid(string HeadColumn, int BatDau, int KetThuc, string noidung)
        {
            XepTKB.BeginUpdate();
            range = worksheet.Range[HeadColumn + BatDau + ":" + HeadColumn + KetThuc];
            range.Font.Size = 9F;
            range.Merge();
            System.Drawing.Color background = System.Drawing.Color.FromArgb(0,176,240);
            range.AutoFitColumns();
            range.FillColor = background;
            range.SetValue(noidung);
            range = worksheet.Range["C2:I15"];
            range.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
            range.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
            range.Alignment.WrapText = true;
            XepTKB.EndUpdate();
        }

        private string BoolColumn(string strtemp)
        {
            string res = string.Empty;
            try
            {
                if (strtemp.Contains("C"))
                {
                    res = "2";
                }
                if (strtemp.Contains("D"))
                {
                    res = "3";
                }
                if (strtemp.Contains("E"))
                {
                    res = "4";
                }
                if (strtemp.Contains("F"))
                {
                    res = "5";
                }
                if (strtemp.Contains("G"))
                {
                    res = "6";
                }
                if (strtemp.Contains("H"))
                {
                    res = "7";
                }
                if (strtemp.Contains("I"))
                {
                    res = "8";
                }
                return res;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //get du lieu vao Datatable
        private void CreateDataTKB(string[] iRanges)
        {
            DataRow dr = iDataXepTKB.NewRow();
            int bdau = 0;
            int kthuc = 0;
            #region
            dr["THU"] = BoolColumn(iRanges[0].Substring(0, 1));
            if (iRanges.Length == 2)
            {
                int a = int.Parse(iRanges[0].Substring(1)) - 1;
                int b = int.Parse(iRanges[1].Substring(1)) - 1;
                bdau = a;
                kthuc = b;
            }
            else
            {
                int a = int.Parse(iRanges[0].Substring(1)) - 1;
                bdau = a;
                kthuc = a;
            }
            dr["TIET_BD"] = bdau;
            dr["TIET_KT"] = kthuc;
            dr["SO_TIET"] = (kthuc - bdau) + 1;
            iDataXepTKB.Rows.Add(dr);
            iDataXepTKB.AcceptChanges();
            #endregion
        }

        //kiem tra so tiet co the xep tiep theo
        private int KiemTraSoTiet(int sotuan, int sotietchuaxep, int sotiettuan, int sodaxep)
        {
            int kq = (sotietchuaxep/sotuan);
            if (kq + sodaxep == sotiettuan)
            {
                return kq;
            }
            return -1;
        }

        //lay du lieu tu DataTable de get len excels
        private void Load_TKBHPCT(DataTable dt)
        {
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        if (Convert.ToInt32(r["THU"].ToString()) == 2)
                        {
                            string HeadColumn = "C";
                            int TietBD = int.Parse(r["TIET_BD"].ToString());
                            int TietKT = int.Parse(r["TIET_KT"].ToString());

                            //DateTime x = Convert.ToDateTime(r["NGAY_BD"].ToString());
                            //string ngay_bd = x.ToShortDateString();

                            //DateTime x1 = Convert.ToDateTime(r["NGAY_KT"].ToString());
                            //string ngay_kt = x1.ToShortDateString();

                            string temp = r["TEN_LOP_HOCPHAN"].ToString();

                            string str =

                                temp + "\n"

                                + "Tiết" + " " + r["TIET_BD"].ToString() + "-" + r["TIET_KT"].ToString() + "\n"

                                + "Phòng:" + " " + r["TEN_PHONG"].ToString() + "\n"

                                + r["TEN_GIANGVIEN"].ToString() + "\n"
                                + "Tuần" + " " + r["TUAN_BD"] + " Đến" + " " + r["TUAN_KT"].ToString();

                                       //+ "(" + ngay_bd + "-" + ngay_kt + ")";

                            //string name = r["ID_LOP_HOCPHAN_CTIET"].ToString();

                            SetColumnGrid(HeadColumn, TietBD + 1, TietKT + 1, str);
                        }
                        if (Convert.ToInt32(r["THU"].ToString()) == 3)
                        {
                            string HeadColumn = "D";
                            int TietBD = int.Parse(r["TIET_BD"].ToString());
                            int TietKT = int.Parse(r["TIET_KT"].ToString());

                            //DateTime x = Convert.ToDateTime(r["NGAY_BD"].ToString());
                            //string ngay_bd = x.ToShortDateString();

                            //DateTime x1 = Convert.ToDateTime(r["NGAY_KT"].ToString());
                            //string ngay_kt = x1.ToShortDateString();

                            string temp = r["TEN_LOP_HOCPHAN"].ToString();

                            string str =

                                temp + "\n"

                                + "Tiết" + " " + r["TIET_BD"].ToString() + "-" + r["TIET_KT"].ToString() + "\n"

                                + "Phòng:" + " " + r["TEN_PHONG"].ToString() + "\n"

                                + r["TEN_GIANGVIEN"].ToString() + "\n"
                                + "Tuần" + " " + r["TUAN_BD"] + " Đến" + " " + r["TUAN_KT"].ToString();

                                       //+ "(" + ngay_bd + "-" + ngay_kt + ")";

                            //string name = r["ID_LOP_HOCPHAN_CTIET"].ToString();

                            SetColumnGrid(HeadColumn, TietBD + 1, TietKT + 1, str);
                        }
                        if (Convert.ToInt32(r["THU"].ToString()) == 4)
                        {
                            string HeadColumn = "E";
                            int TietBD = int.Parse(r["TIET_BD"].ToString());
                            int TietKT = int.Parse(r["TIET_KT"].ToString());

                            //DateTime x = Convert.ToDateTime(r["NGAY_BD"].ToString());
                            //string ngay_bd = x.ToShortDateString();

                            //DateTime x1 = Convert.ToDateTime(r["NGAY_KT"].ToString());
                            //string ngay_kt = x1.ToShortDateString();

                            string temp = r["TEN_LOP_HOCPHAN"].ToString();

                            string str =

                                temp + "\n"

                                + "Tiết" + " " + r["TIET_BD"].ToString() + "-" + r["TIET_KT"].ToString() + "\n"

                                + "Phòng:" + " " + r["TEN_PHONG"].ToString() + "\n"

                                + r["TEN_GIANGVIEN"].ToString() + "\n"

                                + "Tuần" + " " + r["TUAN_BD"] + " Đến" + " " + r["TUAN_KT"].ToString();

                                       //+ "(" + ngay_bd + "-" + ngay_kt + ")";

                            //string name = r["ID_LOP_HOCPHAN_CTIET"].ToString();

                            SetColumnGrid(HeadColumn, TietBD + 1, TietKT + 1, str);
                        }
                        if (Convert.ToInt32(r["THU"].ToString()) == 5)
                        {
                            string HeadColumn = "F";
                            int TietBD = int.Parse(r["TIET_BD"].ToString());
                            int TietKT = int.Parse(r["TIET_KT"].ToString());

                            //DateTime x = Convert.ToDateTime(r["NGAY_BD"].ToString());
                            //string ngay_bd = x.ToShortDateString();

                            //DateTime x1 = Convert.ToDateTime(r["NGAY_KT"].ToString());
                            //string ngay_kt = x1.ToShortDateString();

                            string temp = r["TEN_LOP_HOCPHAN"].ToString();

                            string str =

                                temp + "\n"

                                + "Tiết" + " " + r["TIET_BD"].ToString() + "-" + r["TIET_KT"].ToString() + "\n"

                                + "Phòng:" + " " + r["TEN_PHONG"].ToString() + "\n"

                                + r["TEN_GIANGVIEN"].ToString() + "\n"

                                + "Tuần" + " " + r["TUAN_BD"] + " Đến" + " " + r["TUAN_KT"].ToString();

                                       //+ "(" + ngay_bd + "-" + ngay_kt + ")";

                            //string name = r["ID_LOP_HOCPHAN_CTIET"].ToString();

                            SetColumnGrid(HeadColumn, TietBD + 1, TietKT + 1, str);
                        }
                        if (Convert.ToInt32(r["THU"].ToString()) == 6)
                        {
                            string HeadColumn = "G";
                            int TietBD = int.Parse(r["TIET_BD"].ToString());
                            int TietKT = int.Parse(r["TIET_KT"].ToString());

                            //DateTime x = Convert.ToDateTime(r["NGAY_BD"].ToString());
                            //string ngay_bd = x.ToShortDateString();

                            //DateTime x1 = Convert.ToDateTime(r["NGAY_KT"].ToString());
                            //string ngay_kt = x1.ToShortDateString();

                            string temp = r["TEN_LOP_HOCPHAN"].ToString();

                            string str =

                                temp + "\n"

                                + "Tiết" + " " + r["TIET_BD"].ToString() + "-" + r["TIET_KT"].ToString() + "\n"

                                + "Phòng:" + " " + r["TEN_PHONG"].ToString() + "\n"

                                + r["TEN_GIANGVIEN"].ToString() + "\n"

                                + "Tuần" + " " + r["TUAN_BD"] + " Đến" + " " + r["TUAN_KT"].ToString();

                                       //+ "(" + ngay_bd + "-" + ngay_kt + ")";

                            //string name = r["ID_LOP_HOCPHAN_CTIET"].ToString();

                            SetColumnGrid(HeadColumn, TietBD + 1, TietKT + 1, str);
                        }
                        if (Convert.ToInt32(r["THU"].ToString()) == 7)
                        {
                            string HeadColumn = "H";
                            int TietBD = int.Parse(r["TIET_BD"].ToString());
                            int TietKT = int.Parse(r["TIET_KT"].ToString());

                            //DateTime x = Convert.ToDateTime(r["NGAY_BD"].ToString());
                            //string ngay_bd = x.ToShortDateString();

                            //DateTime x1 = Convert.ToDateTime(r["NGAY_KT"].ToString());
                            //string ngay_kt = x1.ToShortDateString();

                            string temp = r["TEN_LOP_HOCPHAN"].ToString();

                            string str =

                                temp + "\n"

                                + "Tiết" + " " + r["TIET_BD"].ToString() + "-" + r["TIET_KT"].ToString() + "\n"

                                + "Phòng:" + " " + r["TEN_PHONG"].ToString() + "\n"

                                + r["TEN_GIANGVIEN"].ToString() + "\n"

                                + "Tuần" + " " + r["TUAN_BD"] + " Đến" + " " + r["TUAN_KT"].ToString();

                                        //+ "(" + ngay_bd + "-" + ngay_kt + ")";

                            //string name = r["ID_LOP_HOCPHAN_CTIET"].ToString();

                            SetColumnGrid(HeadColumn, TietBD + 1, TietKT + 1, str);
                        }
                        if (Convert.ToInt32(r["THU"].ToString()) == 8)
                        {
                            string HeadColumn = "I";
                            int TietBD = int.Parse(r["TIET_BD"].ToString());
                            int TietKT = int.Parse(r["TIET_KT"].ToString());

                            //DateTime x = Convert.ToDateTime(r["NGAY_BD"].ToString());
                            //string ngay_bd = x.ToShortDateString();

                            //DateTime x1 = Convert.ToDateTime(r["NGAY_KT"].ToString());
                            //string ngay_kt = x1.ToShortDateString();

                            string temp = r["TEN_LOP_HOCPHAN"].ToString();

                            string str =

                                temp + "\n"

                                + "Tiết" + " " + r["TIET_BD"].ToString() + "-" + r["TIET_KT"].ToString() + "\n"

                                + "Phòng:" + " " + r["TEN_PHONG"].ToString() + "\n"

                                + r["TEN_GIANGVIEN"].ToString() + "\n"

                                + "Tuần" + " " + r["TUAN_BD"] + " Đến" + " " + r["TUAN_KT"].ToString();

                                        //+ "(" + ngay_bd + "-" + ngay_kt + ")";

                            //string name = r["ID_LOP_HOCPHAN_CTIET"].ToString();

                            SetColumnGrid(HeadColumn, TietBD + 1, TietKT + 1, str);
                        }
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        //tao tkb
        private void btnMerge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.iDataSoure.Rows[0]["ID_LOPHOCPHAN"].ToString()))
                {
                    CTMessagebox.Show("Chưa chọn lớp học phần để xếp lịch", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                    return;
                }
                //Mouse.OverrideCursor = Cursors.Wait;
                XepTKB.BeginUpdate();
                iDataXepTKB.Clear();
                Range currentSelection = XepTKB.Selection;
                currentSelection.Select();
                if (currentSelection != null)
                {
                    string rangeSelect = currentSelection.GetReferenceA1();
                    string[] strRange = rangeSelect.Split(':');
                    #region Không cho chọn 2 ngày liên tục
                    if (currentSelection.ColumnCount != 1)
                    {
                        Mouse.OverrideCursor = Cursors.Arrow;
                        CTMessagebox.Show("Bạn chỉ được xếp trong 1 ngày", "Thông báo", "", CTICON.Information,CTBUTTON.OK);
                        XepTKB.EndUpdate();
                        XepTKB.ResetLayout();
                        return;
                    }
                    #endregion
                    else
                    {
                        #region Không cho chọn trên header
                        for (int temp = 0; temp < strRange.Length; temp++)
                        {
                            if (strRange[temp] == "A1" || strRange[temp] == "B1" || strRange[temp] == "C1" ||
                                strRange[temp] == "D1" || strRange[temp] == "E1" || strRange[temp] == "F1" ||
                                strRange[temp] == "G1" || strRange[temp] == "H1" || strRange[temp] == "I1")
                            {
                                Mouse.OverrideCursor = Cursors.Arrow;
                                CTMessagebox.Show("Bạn không thể xếp lịch tại A1_B1_C1_D1_E1_F1_G1_H1_I1", "Lỗi", "",CTICON.Information, CTBUTTON.OK);
                                XepTKB.ResetLayout();
                                XepTKB.EndUpdate();
                                return;
                            }
                        }
                        #endregion
                        #region Set data
                        CreateDataTKB(strRange);
                        DataRow r = iDataXepTKB.Rows[0];
                        this.iDataSoure.Rows[0]["THU"] = r["THU"];
                        this.iDataSoure.Rows[0]["TIET_BD"] = r["TIET_BD"];
                        this.iDataSoure.Rows[0]["TIET_KT"] = r["TIET_KT"];
                        this.iDataSoure.Rows[0]["SO_TIET"] = r["SO_TIET"];
                        #endregion
                        #region Kiểm tra số tiết
                        //int tiet = KiemTraSoTiet(Convert.ToInt32(this.iDataSoure.Rows[0]["SO_TUAN"].ToString()),
                        //    Convert.ToInt32(this.iDataSoure.Rows[0]["SO_TIET_CONLAI"].ToString()),
                        //    Convert.ToInt32(this.iDataSoure.Rows[0]["SO_TIET_TUAN"].ToString()),
                        //    Convert.ToInt32(this.iDataSoure.Rows[0]["SO_TIET_DASEP"].ToString()));
                        #endregion

                        bool check = true;
                            //check = (tiet == (Convert.ToInt32(this.iDataSoure.Rows[0]["SO_TIET"].ToString()))) ? true : false;
                        #region Lưu Chi tiết lớp học phần
                        if (check)
                        {
                            flagsave = true;
                            frm_TimPhongTrong frm = new frm_TimPhongTrong(this.iDataSoure.Copy(), flagsave);
                            frm.Owner = Window.GetWindow(this);
                            frm.ShowDialog();
                            if (frm.res == true)
                            {
                                GrdViewLHP_OnFocusedRowChanged(sender, null);
                                worksheet.Range[rangeSelect].Merge();
                            }
                        }
                        else
                        {
                            CTMessagebox.Show("Số tiết vừa chọn vượt quá số tiết cho phép. Vui lòng kiểm tra", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                            XepTKB.ResetLayout();
                            XepTKB.EndUpdate();
                            return;
                        }
                        #endregion
                    }
                }

                XepTKB.ResetLayout();
                XepTKB.EndUpdate();
            }
            catch 
            {
                return;
            }
            //finally
            //{
            //    Mouse.OverrideCursor = Cursors.Arrow;
            //}
        }
        //cap nhat tkb chưa xử lý
        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.iDataSoure.Rows[0]["ID_LOPHOCPHAN"].ToString()))
                {
                    CTMessagebox.Show("Chưa chọn lớp học phần để xếp lịch", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                    return;
                }
                //Mouse.OverrideCursor = Cursors.Wait;
                iDataXepTKB.Clear();
                XepTKB.BeginUpdate();
                Range currentSelect = XepTKB.Selection;
                string sRang = currentSelect.GetReferenceA1();
                string[] strRaange = sRang.Split(':');
                #region Không cho chọn trên header
                for (int temp = 0; temp < strRaange.Length; temp++)
                {
                    if (strRaange[temp] == "A1" || strRaange[temp] == "B1" || strRaange[temp] == "C1" ||
                        strRaange[temp] == "D1" || strRaange[temp] == "E1" || strRaange[temp] == "F1" ||
                        strRaange[temp] == "G1" || strRaange[temp] == "H1" || strRaange[temp] == "I1")
                    {
                        CTMessagebox.Show("Bạn không thể xếp lịch tại A1_B1_C1_D1_E1_F1_G1_H1_I1", "Lỗi", "", CTICON.Information, CTBUTTON.OK);
                        XepTKB.EndUpdate();
                        XepTKB.ResetLayout();
                        return;
                    }
                }
                #endregion
                CreateDataTKB(strRaange);
                DataRow dr = iDataXepTKB.Rows[0];
                this.iDataSoure.Rows[0]["THU"] = dr["THU"];
                this.iDataSoure.Rows[0]["TIET_BD"] = dr["TIET_BD"];
                this.iDataSoure.Rows[0]["TIET_KT"] = dr["TIET_KT"];
                this.iDataSoure.Rows[0]["SO_TIET"] = dr["SO_TIET"];
                foreach (DataRow r in iDataSoureHPCT.Rows)
                {
                    if (r["THU"].ToString().Equals(dr["THU"].ToString()) && r["TIET_BD"].ToString().Equals(dr["TIET_BD"].ToString()) && r["TIET_KT"].ToString().Equals(dr["TIET_KT"].ToString()))
                    {
                        flagsave = false;
                        this.iDataSoure.Rows[0]["ID_PHONG"] = r["ID_PHONG"];
                        this.iDataSoure.Rows[0]["ID_LOP_HOCPHAN_CTIET"] = r["ID_LOP_HOCPHAN_CTIET"];
                        frm_TimPhongTrong frm = new frm_TimPhongTrong(this.iDataSoure, flagsave);
                        frm.Owner = Window.GetWindow(this);
                        frm.MaxWidth = 1200;
                        frm.MinHeight = 600;
                        frm.ShowDialog();
                        if (frm.res == true)
                        {
                            GrdViewLHP_OnFocusedRowChanged(sender, null);
                        }
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }
        //Huy tkb
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.iDataSoure.Rows[0]["ID_LOPHOCPHAN"].ToString()))
                {
                    CTMessagebox.Show("Chưa chọn lớp học phần để xếp lịch", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                    return;
                }
                Mouse.OverrideCursor = Cursors.Wait;
                iDataXepTKB.Clear();
                XepTKB.BeginUpdate();
                Range currentSelect = XepTKB.Selection;
                string sRang = currentSelect.GetReferenceA1();
                string[] strRaange = sRang.Split(':');
                #region Không cho chọn trên header
                for (int temp = 0; temp < strRaange.Length; temp++)
                {
                    if (strRaange[temp] == "A1" || strRaange[temp] == "B1" || strRaange[temp] == "C1" ||
                        strRaange[temp] == "D1" || strRaange[temp] == "E1" || strRaange[temp] == "F1" ||
                        strRaange[temp] == "G1" || strRaange[temp] == "H1" || strRaange[temp] == "I1")
                    {
                        CTMessagebox.Show("Bạn không thể xếp lịch tại A1_B1_C1_D1_E1_F1_G1_H1_I1", "Lỗi", "",CTICON.Information, CTBUTTON.OK);
                        XepTKB.EndUpdate();
                        XepTKB.ResetLayout();
                        return;
                    }
                }
                #endregion
                #region Begin delete
                if (CTMessagebox.Show("Bạn muốn xóa?", "Xóa", "", CTICON.Information, CTBUTTON.YesNo) == CTRESPONSE.Yes)
                {
                    CreateDataTKB(strRaange);
                    DataRow dr = iDataXepTKB.Rows[0];
                    foreach (DataRow r in iDataSoureHPCT.Rows)
                    {
                        if (r["THU"].ToString().Equals(dr["THU"].ToString()) && r["TIET_BD"].ToString().Equals(dr["TIET_BD"].ToString()) && r["TIET_KT"].ToString().Equals(dr["TIET_KT"].ToString()))
                        {
                            this.iDataSoure.Rows[0]["ID_LOP_HOCPHAN_CTIET"] = r["ID_LOP_HOCPHAN_CTIET"];
                            int pID_LOP_HOCPHAN_CTIET = Convert.ToInt32(this.iDataSoure.Rows[0]["ID_LOP_HOCPHAN_CTIET"].ToString());
                            bool res = client.Delete_LopHocPhan_CT(pID_LOP_HOCPHAN_CTIET, UserCommon.UserName);
                            if (!res)
                            {
                                CTMessagebox.Show("Xóa không thành công", "Xóa", "", CTICON.Information, CTBUTTON.YesNo);
                            }
                            else
                            {
                                GrdViewLHP_OnFocusedRowChanged(sender, null);
                                worksheet.UnMergeCells(currentSelect);
                            }
                        }
                    }
                }
                #endregion
                XepTKB.EndUpdate();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
        }

        private void CboHeDT_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                if (this.iDataSoure.Rows[0]["ID_HE_DAOTAO"].ToString() != string.Empty)
                {
                    DataTable dt = null;
                    dt = client.GetKhoaHoc(Convert.ToInt32(this.iDataSoure.Rows[0]["ID_HE_DAOTAO"].ToString()));
                    cboKhoaHoc.ItemsSource = dt;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        this.iDataSoure.Rows[0]["ID_KHOAHOC"] = cboKhoaHoc.GetKeyValue(0);
                    }
                    if (dt.Rows.Count == 0)
                    {
                        this.iGridDataSoure = null;
                        this.grdLHP.ItemsSource = iGridDataSoure;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CboKhoaHoc_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                this.iGridDataSoure = null;
                if (!this.iDataSoure.Rows[0]["ID_KHOAHOC"].ToString().Equals(string.Empty) && !this.iDataSoure.Rows[0]["ID_HE_DAOTAO"].ToString().Equals(string.Empty))
                {
                    DataTable dt = client.GetAllLopForKhoaHoc(Convert.ToInt32(this.iDataSoure.Rows[0]["ID_HE_DAOTAO"].ToString()), Convert.ToInt32(this.iDataSoure.Rows[0]["ID_KHOAHOC"].ToString()));
                    if (dt.Rows.Count > 0)
                    {
                        this.iGridDataSoure = dt;
                    }
                }
                grdLHP.ItemsSource = this.iGridDataSoure;

            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
            
        }

        private void GrdViewLHP_OnFocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                this.iDataSoure.Rows[0]["SO_TIET_TONG"] = "0";
                this.iDataSoure.Rows[0]["SO_TUAN"] = "0";
                this.iDataSoure.Rows[0]["SO_TIET_TUAN"] = "0";
                this.iDataSoure.Rows[0]["SO_TIET_DASEP"] = "0";
                
                ClearFormat();
                //DataTable iDataSoureHPCT = null;
                Mouse.OverrideCursor = Cursors.Wait;
                DataRow r = null;
                if (this.grdLHP.GetFocusedRow() == null)
                    return;
                r = ((DataRowView)this.grdLHP.GetFocusedRow()).Row;
                this.iDataSoure.Rows[0]["ID_LOPHOCPHAN"] = r["ID_LOPHOCPHAN"];
                if (r["TUAN_KT"].ToString() != string.Empty && r["TUAN_BD"].ToString() != string.Empty)
                {
                    this.iDataSoure.Rows[0]["SO_TUAN"] = (Convert.ToInt32(r["TUAN_KT"].ToString()) - Convert.ToInt32(r["TUAN_BD"].ToString())) + 1;
                }
                //this.iDataSoure.Rows[0]["SO_TUAN"] = (Convert.ToInt32(r["TUAN_KT"].ToString()) - Convert.ToInt32(r["TUAN_BD"].ToString())) + 1;
                this.iDataSoure.Rows[0]["SO_TIET_TONG"] = r["SOTIET"];
                if (!this.iDataSoure.Rows[0]["ID_LOPHOCPHAN"].Equals(string.Empty))
                {
                    iDataSoureHPCT = client.GetHPCTByLHP(Convert.ToInt32(this.iDataSoure.Rows[0]["ID_LOPHOCPHAN"].ToString()));

                    if (this.iDataSoure.Rows[0]["SO_TIET_TONG"].ToString() != string.Empty && this.iDataSoure.Rows[0]["SO_TUAN"].ToString() != string.Empty)
                    {
                        this.iDataSoure.Rows[0]["SO_TIET_TUAN"] = Convert.ToInt32(this.iDataSoure.Rows[0]["SO_TIET_TONG"].ToString()) / Convert.ToInt32(this.iDataSoure.Rows[0]["SO_TUAN"].ToString());
                    
                    }
                    this.iDataSoure.Rows[0]["SO_TIET_DASEP"] = client.SoTietDaSep(Convert.ToInt32(this.iDataSoure.Rows[0]["ID_LOPHOCPHAN"].ToString()));
                }
                if (iDataSoureHPCT != null)
                {
                    Load_TKBHPCT(iDataSoureHPCT.Copy());
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void XepTKB_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
        }

        private void TxtSoTietDaSep_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.iDataSoure.Rows[0]["SO_TIET_TONG"].ToString()) &&
                    !string.IsNullOrEmpty(this.iDataSoure.Rows[0]["SO_TIET_DASEP"].ToString()))
                {
                    this.iDataSoure.Rows[0]["SO_TIET_CONLAI"] =
                        Convert.ToInt32(this.iDataSoure.Rows[0]["SO_TIET_TONG"].ToString()) - (Convert.ToInt32(this.iDataSoure.Rows[0]["SO_TIET_DASEP"].ToString()) * Convert.ToInt32(this.iDataSoure.Rows[0]["SO_TUAN"].ToString()));
                }
            }
            catch
            {
                return;
            }
        }

        private void BtnTimPhong_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                frm_TimPhongTrong frm = new frm_TimPhongTrong(null, null);
                frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                frm.MaxWidth = 1200;
                frm.MinHeight = 600;
                frm.ShowDialog();
            }
            catch
            {
                return;
            }
        }

        private void XepTKB_OnCellBeginEdit(object sender, SpreadsheetCellCancelEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                #region Begin update but there not use
                iDataXepTKB.Clear();
                XepTKB.BeginUpdate();
                Range currentSelection = XepTKB.Selection;
                string sRangs = currentSelection.GetReferenceA1();
                string[] iRanges = sRangs.Split(':');
                #endregion
                #region Không cho chọn trên header
                for (int temp = 0; temp < iRanges.Length; temp++)
                {
                    if (iRanges[temp] == "A1" || iRanges[temp] == "B1" || iRanges[temp] == "C1" ||
                        iRanges[temp] == "D1" || iRanges[temp] == "E1" || iRanges[temp] == "F1" ||
                        iRanges[temp] == "G1" || iRanges[temp] == "H1" || iRanges[temp] == "I1")
                    {
                        CTMessagebox.Show("Bạn không thể cập nhật lịch vào A1_B1_C1_D1_E1_F1_G1_H1_I1", "Lỗi", "", CTICON.Information, CTBUTTON.OK);
                        worksheet.Clear(range);
                        XepTKB.EndUpdate();
                        return;
                    }
                }
                #endregion
                #region Set datat
                CreateDataTKB(iRanges);
                DataRow r = iDataXepTKB.Rows[0];
                this.iDataSoure.Rows[0]["THU"] = r["THU"];
                this.iDataSoure.Rows[0]["TIET_BD"] = r["TIET_BD"];
                this.iDataSoure.Rows[0]["TIET_KT"] = r["TIET_KT"];
                this.iDataSoure.Rows[0]["SO_TIET"] = r["SO_TIET"];
                #endregion
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }
    }
}
