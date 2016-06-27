using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
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
using DevExpress.Utils;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.GroupRowLayout;
using Microsoft.Win32;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_MonHoc.xaml
    /// </summary>
    public partial class frm_MonHoc : Page
    {
        bus_MonHoc bus = new bus_MonHoc();

        public static DataTable idatasource = null;
        private DataTable iGridDataSource = null;

        private DataTable iData = null;

        public frm_MonHoc()
        {
            InitializeComponent();
            this.iData = TableChelmabinding();
            //this.iData.Rows[0]["USER"] = UserCommon.UserName.ToString();
            this.iData.Rows[0]["USER"] = UserCommon.UserName.ToString();
            InitGrid();
            Load_data();
        }

        private DataTable TableChelmabinding()
        {
            DataTable dtaTable = null;
            try
            {
                Dictionary<string, Type> xDicUser = new Dictionary<string, Type>();
                xDicUser.Add("USER", typeof(string));
                xDicUser.Add("ID_MONHOC", typeof(int));
                dtaTable = TableUtil.ConvertToTable(xDicUser);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return dtaTable;
        }

        private void InitGrid()
        {
            try
            {
                GridColumn col = null;

                col = new GridColumn();
                col.FieldName = "ID_MONHOC";
                col.Visible = false;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "CACH_TINHDIEM";
                col.Visible = false;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "ID_HE_DAOTAO";
                col.Visible = false;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "ID_MONHOC_SONGHANH";
                col.Visible = false;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "MA_MONHOC";
                col.Header = "Mã môn học";
                col.AllowCellMerge = true;
                col.AllowEditing = DefaultBoolean.False;
                col.Width = 90;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TEN_MONHOC";
                col.Header = "Tên môn học";
                col.AllowCellMerge = true;
                col.AllowEditing = DefaultBoolean.False;
                col.Width =150;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "SO_TC";
                col.Header = "STC";
                col.AllowCellMerge = false;
                col.AllowEditing = DefaultBoolean.False;
                col.Width = 50;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "KIEUHOC";
                col.Header = "Loại môn học";
                col.AllowCellMerge = false;
                col.AllowEditing = DefaultBoolean.False;
                col.Width = 90;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TEN_HE_DAOTAO";
                col.Header = "Hệ đào tạo";
                col.AllowCellMerge = false;
                col.AllowEditing = DefaultBoolean.False;
                col.Width = 150;
                grd.Columns.Add(col); 

                col = new GridColumn();
                col.FieldName = "SOTIET";
                col.Header = "Số tiết";
                col.AllowCellMerge = false;
                col.AllowEditing = DefaultBoolean.False;
                col.Width = 50;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "CACH_TINHDIEM_CHU";
                col.Header = "Cách tính điểm";
                col.AllowCellMerge = false;
                col.AllowEditing = DefaultBoolean.False;
                col.Width = 120;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TEN_BOMON";
                col.Header = "Bộ môn";
                col.AllowCellMerge = false;
                col.AllowEditing = DefaultBoolean.False;
                col.Width = 90;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TEN_MONHOC_SONGHANH";
                col.Header = "Môn học song hành";
                col.AllowCellMerge = false;
                col.Visible = true;
                col.AllowEditing = DefaultBoolean.False;
                col.Width = 150;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "GHICHU";
                col.Header = "Ghi chú";
                col.AllowCellMerge = false;
                col.AllowEditing = DefaultBoolean.False;
                col.Width = 150;
                grd.Columns.Add(col);

                grdView.AutoWidth = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Load_data()
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                DataTable xdt = null;
                xdt = bus.GetAllMonHoc();
                xdt.Columns.Add("KIEUHOC", typeof (string));
                xdt.Columns.Add("CACH_TINHDIEM_CHU", typeof (string));
                foreach (DataRow dr in xdt.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["IS_THUCHANH"].ToString()) && Convert.ToInt32(dr["IS_THUCHANH"]) == 1)
                    {
                        dr["KIEUHOC"] = "Thực hành";
                    }
                    if (!string.IsNullOrEmpty(dr["IS_LYTHUYET"].ToString()) && Convert.ToInt32(dr["IS_LYTHUYET"]) == 1)
                    {
                        dr["KIEUHOC"] = "Lý thuyết";
                    }
                    if (!string.IsNullOrEmpty(dr["CACH_TINHDIEM"].ToString()))
                    {
                        if (dr["CACH_TINHDIEM"].ToString().Trim().Equals("20-30-50"))
                        {
                            dr["CACH_TINHDIEM_CHU"] = "20% - 30% - 50%";
                        }
                        if (dr["CACH_TINHDIEM"].ToString().Trim().Equals("30-70"))
                        {
                            dr["CACH_TINHDIEM_CHU"] = "30% - 70%";
                        }
                        if (dr["CACH_TINHDIEM"].ToString().Trim().Equals("100"))
                        {
                            dr["CACH_TINHDIEM_CHU"] = "100%";
                        }
                    }
                }
                grd.ItemsSource = xdt;
                this.iGridDataSource = xdt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void GrdView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DataRow RowSelGb = null;
                if (this.grd.GetFocusedRow() == null) return;
                RowSelGb = ((DataRowView)this.grd.GetFocusedRow()).Row;
                idatasource = iGridDataSource.Clone();
                idatasource.ImportRow(RowSelGb);
                frm_MonHoc_popup popup = new frm_MonHoc_popup();
                popup.ShowDialog();
                Load_data();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                idatasource = iGridDataSource.Clone();
                idatasource.Rows.Add(idatasource.NewRow());
                frm_MonHoc_popup popup = new frm_MonHoc_popup();
                popup.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Btnrefresh_OnClick(object sender, RoutedEventArgs e)
        {
            Load_data();
        }

        private void Btndelete_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (!string.IsNullOrEmpty(this.iData.Rows[0]["ID_MONHOC"].ToString()))
                {
                    if (CTMessagebox.Show("Bạn muốn xóa môn học này ?", "Xóa", "",
                        CTICON.Question,
                        CTBUTTON.YesNo) == CTRESPONSE.Yes)
                    {
                        int i = bus.DeleteObject(Convert.ToInt32(this.iData.Rows[0]["ID_MONHOC"].ToString()),
                            this.iData.Rows[0]["USER"].ToString());
                        if (i != 0)
                        {
                            CTMessagebox.Show("Thành công", "Xóa", "", CTICON.Information, CTBUTTON.OK);
                            Load_data();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void GrdView_OnFocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                DataRow RowSelGb = null;
                if (this.grd.GetFocusedRow() == null) return;
                RowSelGb = ((DataRowView) this.grd.GetFocusedRow()).Row;
                this.iData.Rows[0]["ID_MONHOC"] = RowSelGb["ID_MONHOC"];
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private void BtnImport_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".xls";
            dlg.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                DataTable dtExcel = exceldata(filename);

                DataTable dtCheck = bus.GetAllMonHoc();
                DataTable dtNewInsert = dtExcel.Clone();
                foreach (DataRow dr in dtExcel.Rows)
                {
                    if (IsCheck(dtCheck, dr["f_mamh"].ToString()))
                    {
                        dtNewInsert.ImportRow(dr);
                        DataRow m = dtCheck.NewRow();
                        m["MA_MONHOC"] = dr["f_mamh"].ToString();
                        dtCheck.Rows.Add(m);
                    }
                }
                int isInsert = 0;
                if (dtNewInsert != null && dtNewInsert.Rows.Count > 0)
                {
                    isInsert = bus.InsertObject_Excel(dtNewInsert, iData.Rows[0]["USER"].ToString());
                    if (isInsert != 0)
                    {
                        CTMessagebox.Show("Thành công", "Nhập từ Excel", "", CTICON.Information, CTBUTTON.OK);
                    }
                    else
                    {
                        CTMessagebox.Show("Lỗi", "Nhập từ Excel", "", CTICON.Information, CTBUTTON.OK);                        
                    }
                    Load_data();
                }
            }
        }

        private bool IsCheck(DataTable dt, string pMaMH)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] xcheck = dt.Select("MA_MONHOC = '" + pMaMH + "'");
                if (xcheck.Count() > 0)
                    return false;
            }
            return true;
        }

        public static DataTable exceldata(string filePath)
        {
            DataTable dtexcel = new DataTable();
            bool hasHeaders = false;
            string HDR = hasHeaders ? "Yes" : "No";
            string strConn;
            if (filePath.Substring(filePath.LastIndexOf('.')).ToLower() == ".xlsx")
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=0\"";
            else
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\"";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            // Load nhieu sheet trong 1 file
            foreach (DataRow schemaRow in schemaTable.Rows)
            {
                string sheet = schemaRow["TABLE_NAME"].ToString(); //ten sheet
                if (!sheet.EndsWith("_"))
                {
                    //Lay all column
                    //string query = "SELECT  * FROM [" + sheet + "]";

                    //lay column theo y muon
                    string query = "SELECT f_mamhtght,f_mamh,f_mamhhtd,f_dvht,f_tenmhvn FROM [" + sheet + "]";
                    OleDbDataAdapter daexcel = new OleDbDataAdapter(query, conn);
                    dtexcel.Locale = CultureInfo.CurrentCulture;
                    daexcel.Fill(dtexcel);
                }
            }
            conn.Close();
            return dtexcel;
        }
    }
}
