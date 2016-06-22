using System;
using System.Collections.Generic;
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
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using Microsoft.Win32;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_molophocphan.xaml
    /// </summary>
    public partial class frm_molophocphan : Page
    {
        private DataTable iDataSource = null;
        private DataTable iGridDataSource = null;
        private DataTable iGridData_dsHP_nganh = null;
        private DataTable iGridData_dsHP = null;
        private DataTable iGridData_TTHK = null;
        public static DataTable idata_send = null;
        public static int iSua = 0;

        public frm_molophocphan()
        {
            InitializeComponent();
            this.iDataSource = TableChelmabinding();
            this.DataContext = this.iDataSource;
            idata_send = TableAuto_Send();
            InitGrid_MH_NGANH();
            InitGrid_HP();
            InitGrid_Lop();
            InitGrid_mh_lop();
            InitGrid_HP_ds();
            this.iDataSource.Rows[0]["USER"] = UserCommon.UserName;
            Load_combo();
        }

        private DataTable TableChelmabinding()
        {
            DataTable dtaTable = null;
            try
            {
                Dictionary<string, Type> xDicUser = new Dictionary<string, Type>();
                xDicUser.Add("ID_LOPHOCPHAN", typeof(decimal));
                xDicUser.Add("TEN_LOP_HOCPHAN", typeof(string));
                xDicUser.Add("USER", typeof(string));
                xDicUser.Add("ID_HE_DAOTAO", typeof(int));
                xDicUser.Add("TEN_HEDAOTAO", typeof(string));
                xDicUser.Add("TEN_HEDAOTAO_LQL", typeof(string));
                xDicUser.Add("TEN_NGANH_LQL", typeof(string));
                xDicUser.Add("TEN_NGANH", typeof(string));
                xDicUser.Add("ID_HEDAOTAO_LQL", typeof(int));
                xDicUser.Add("ID_KHOAHOC", typeof(int));
                xDicUser.Add("ID_KHOAHOC_LQL", typeof(int));
                xDicUser.Add("ID_KHOAHOC_NGANH", typeof(int));
                xDicUser.Add("ID_LOPHOC", typeof(int));
                xDicUser.Add("HOCKY", typeof(int));
                xDicUser.Add("HOCKY_Search", typeof(int));
                xDicUser.Add("NAMHOC_Search", typeof(int));
                xDicUser.Add("HOCKY_Search_ds", typeof(int));
                xDicUser.Add("NAMHOC_Search_ds", typeof(int));
                xDicUser.Add("ID_MONHOC", typeof(int));
                xDicUser.Add("ID_KHOAHOC_NGANH_CTIET", typeof(int));
                dtaTable = TableUtil.ConvertToTable(xDicUser);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return dtaTable;
        }

        private void InitGrid_MH_NGANH()
        {
            try
            {
                GridColumn xcolumn;

                xcolumn = new GridColumn();
                xcolumn.FieldName = "ID_KHOAHOC_NGANH_CTIET";
                xcolumn.Visible = false;
                grd_KN.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "ID_MONHOC";
                xcolumn.Visible = false;
                grd_KN.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "MA_MONHOC";
                xcolumn.Header = "Mã môn học";
                xcolumn.Width = 90;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd_KN.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "TEN_MONHOC";
                xcolumn.Header = "Tên môn học";
                xcolumn.Width = 150;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd_KN.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "SO_TC";
                xcolumn.Header = "STC";
                xcolumn.Width = 50;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd_KN.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "SOTIET";
                xcolumn.Header = "Số tiết";
                xcolumn.Width = 60;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd_KN.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "KIEU_HOC_CHU";
                xcolumn.Header = "Kiểu học";
                xcolumn.Width = 90;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd_KN.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "HOCKY";
                xcolumn.Header = "Học kỳ";
                xcolumn.Width = 50;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd_KN.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "TEN_BM";
                xcolumn.Header = "Bộ môn";
                xcolumn.Width = 150;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd_KN.Columns.Add(xcolumn);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void InitGrid_HP()
        {
            try
            {

                GridColumn xcolumn;

                xcolumn = new GridColumn();
                xcolumn.FieldName = "ID_LOPHOCPHAN";
                xcolumn.Visible = false;
                grd_ct.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "MA_LOP_HOCPHAN";
                xcolumn.Header = "Mã lớp học phần";
                xcolumn.Width = 90;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd_ct.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "TEN_LOP_HOCPHAN";
                xcolumn.Header = "Tên lớp học phần";
                xcolumn.Width = 150;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd_ct.Columns.Add(xcolumn);

                //xcolumn = new GridColumn();
                //xcolumn.FieldName = "SO_TC";
                //xcolumn.Header = "STC";
                //xcolumn.Width = 50;
                //xcolumn.AllowEditing = DefaultBoolean.False;
                //xcolumn.Visible = true;
                //grd_ct.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "SOTIET";
                xcolumn.Header = "Số tiết";
                xcolumn.Width = 50;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd_ct.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "SOLUONG";
                xcolumn.Header = "Số sinh viên";
                xcolumn.Width = 50;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd_ct.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "TUAN_BD";
                xcolumn.Header = "Tuần bắt đầu";
                xcolumn.Width = 150;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd_ct.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "TUAN_KT";
                xcolumn.Header = "Tuần kết thúc";
                xcolumn.Width = 150;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd_ct.Columns.Add(xcolumn);

                //xcolumn = new GridColumn();
                //xcolumn.FieldName = "KIEU_HOC_CHU";
                //xcolumn.Header = "Kiểu học";
                //xcolumn.Width = 90;
                //xcolumn.AllowEditing = DefaultBoolean.False;
                //xcolumn.Visible = true;
                //grd_ct.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "CACH_TINHDIEM";
                xcolumn.Header = "Cách tính điểm";
                xcolumn.Width = 150;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd_ct.Columns.Add(xcolumn);

                grdView_ct.AutoWidth = false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void InitGrid_HP_ds()
        {
            try
            {

                GridColumn xcolumn;

                xcolumn = new GridColumn();
                xcolumn.FieldName = "ID_LOPHOCPHAN";
                xcolumn.Visible = false;
                grd_ds.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "MA_LOP_HOCPHAN";
                xcolumn.Header = "Mã lớp học phần";
                xcolumn.Width = 90;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd_ds.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "TEN_LOP_HOCPHAN";
                xcolumn.Header = "Tên lớp học phần";
                xcolumn.Width = 150;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd_ds.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "SOTIET";
                xcolumn.Header = "Số tiết";
                xcolumn.Width = 50;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd_ds.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "SOLUONG";
                xcolumn.Header = "Số sinh viên";
                xcolumn.Width = 50;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd_ds.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "TUAN_BD";
                xcolumn.Header = "Tuần bắt đầu";
                xcolumn.Width = 150;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd_ds.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "TUAN_KT";
                xcolumn.Header = "Tuần kết thúc";
                xcolumn.Width = 150;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd_ds.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "CACH_TINHDIEM";
                xcolumn.Header = "Cách tính điểm";
                xcolumn.Width = 150;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd_ds.Columns.Add(xcolumn);

                grdView_ds.AutoWidth = false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void InitGrid_Lop()
        {
            try
            {
                GridColumn xcolumn;

                xcolumn = new GridColumn();
                xcolumn.FieldName = "ID_KHOAHOC_NGANH";
                xcolumn.Visible = false;
                grd_lop.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "ID_LOPHOC";
                xcolumn.Visible = false;
                grd_lop.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "MA_LOP";
                xcolumn.Header = "Mã lớp";
                xcolumn.Width = 70;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd_lop.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "TEN_LOP";
                xcolumn.Header = "Tên lớp";
                xcolumn.Width = 150;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd_lop.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "TEN_KHOAHOC";
                xcolumn.Header = "Khóa";
                xcolumn.Width = 90;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd_lop.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "ten_nganh";
                xcolumn.Header = "Ngành";
                xcolumn.Width = 90;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd_lop.Columns.Add(xcolumn);

                grdView_lop.AutoWidth = false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void InitGrid_mh_lop()
        {
            try
            {
                GridColumn col = null;

                col = new GridColumn();
                col.FieldName = "ID_KHOAHOC_NGANH_CTIET";
                col.Visible = false;
                grd_mh_lop.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "ID_MONHOC";
                col.Visible = false;
                grd_mh_lop.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "MA_MONHOC";
                col.Header = "Mã môn học";
                col.AllowCellMerge = false;
                col.AllowEditing = DefaultBoolean.False;
                col.Width = 90;
                grd_mh_lop.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TEN_MONHOC";
                col.Header = "Tên môn học";
                col.AllowCellMerge = false;
                col.AllowEditing = DefaultBoolean.False;
                col.Width = 150;
                grd_mh_lop.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "SO_TC";
                col.Header = "Số tín chỉ";
                col.AllowCellMerge = false;
                col.AllowEditing = DefaultBoolean.False;
                col.Width = 50;
                grd_mh_lop.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "SOTIET";
                col.Header = "Số tiết";
                col.AllowCellMerge = false;
                col.AllowEditing = DefaultBoolean.False;
                col.Width = 50;
                grd_mh_lop.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TEN_LOAIMH";
                col.Header = "Loại môn học";
                col.AllowCellMerge = false;
                col.AllowEditing = DefaultBoolean.False;
                col.Width = 90;
                grd_mh_lop.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TEN_BOMON";
                col.Header = "Bộ môn";
                col.AllowCellMerge = false;
                col.AllowEditing = DefaultBoolean.False;
                col.Width = 90;
                grd_mh_lop.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "KIEU_HOC_CHU";
                col.Header = "Kiểu học";
                col.AllowCellMerge = false;
                col.Visible = false;
                col.AllowEditing = DefaultBoolean.False;
                col.Width = 90;
                grd_mh_lop.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "GHICHU";
                col.Header = "Ghi chú";
                col.AllowCellMerge = false;
                col.AllowEditing = DefaultBoolean.False;
                col.Width = 150;
                grd_mh_lop.Columns.Add(col);

                grdView_mh_lop.AutoWidth = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataTable TableAuto_Send()
        {
            DataTable dtaTable = null;
            try
            {
                Dictionary<string, Type> xDicUser = new Dictionary<string, Type>();
                xDicUser.Add("ID_LOPHOCPHAN", typeof(int));
                xDicUser.Add("ID_LOPHOC", typeof(decimal));
                xDicUser.Add("ID_KHOAHOC_NGANH_CTIET", typeof(decimal));
                xDicUser.Add("TEN_NGANH", typeof(string));
                xDicUser.Add("TEN_HEDAOTAO", typeof(string));
                xDicUser.Add("ID_NAMHOC_HKY_HTAI", typeof(decimal));
                xDicUser.Add("ID_HE_DAOTAO", typeof(decimal));
                xDicUser.Add("ID_MONHOC", typeof(decimal));
                xDicUser.Add("MA_LOP_HOCPHAN", typeof(string));
                xDicUser.Add("TEN_LOP_HOCPHAN", typeof(string));
                xDicUser.Add("TUAN_BD", typeof(decimal));
                xDicUser.Add("TUAN_KT", typeof(decimal));
                xDicUser.Add("SOTIET", typeof(decimal));
                xDicUser.Add("SOLUONG", typeof(decimal));
                xDicUser.Add("CACH_TINHDIEM", typeof(string));

                xDicUser.Add("MA_MONHOC", typeof(string));
                xDicUser.Add("TEN_MONHOC", typeof(string));
                xDicUser.Add("KIEU_HOC_CHU", typeof(string));
                xDicUser.Add("SO_TC", typeof(decimal));
                xDicUser.Add("USER", typeof(string));
                xDicUser.Add("HOCKY", typeof(int));
                xDicUser.Add("NAMHOC", typeof(int));
                xDicUser.Add("HOCKYHT", typeof(decimal));
                xDicUser.Add("NAMHOCHT", typeof(string));
                dtaTable = TableUtil.ConvertToTable(xDicUser);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return dtaTable;
        }

        private void Load_combo()
        {
            try
            {
                bus_molophocphan bus = new bus_molophocphan();
                //Thong tin nam hoc
                iGridData_TTHK = bus.GetAll_Tso_Hocky();
                iDataSource.Rows[0]["HOCKY_Search"] = iGridData_TTHK.Rows[0]["HOCKY"];
                iDataSource.Rows[0]["NAMHOC_Search"] = iGridData_TTHK.Rows[0]["NAMHOC_TU"];
                iDataSource.Rows[0]["HOCKY_Search_ds"] = iGridData_TTHK.Rows[0]["HOCKY"];
                iDataSource.Rows[0]["NAMHOC_Search_ds"] = iGridData_TTHK.Rows[0]["NAMHOC_TU"];

                DataTable xdtbm = bus.GetAll_HDT();
                cboHeDT.ItemsSource = xdtbm;
                cboHeDTLQL.ItemsSource = xdtbm;

                #region LOAD HOC KY
                DataTable dt_hocky = new DataTable();
                dt_hocky.Columns.Add("hocky", typeof(int));
                dt_hocky.Columns.Add("tenhocky", typeof(string));

                DataRow xdr = null;
                xdr = dt_hocky.NewRow();
                xdr["hocky"] = 0;
                xdr["tenhocky"] = "-----Chọn-----";
                dt_hocky.Rows.Add(xdr);
                for (int i = 1; i <= 8; i++)
                {
                    xdr = dt_hocky.NewRow();
                    xdr["hocky"] = i;
                    xdr["tenhocky"] = i.ToString();
                    dt_hocky.Rows.Add(xdr);
                }
                dt_hocky.AcceptChanges();
                cboHocKy.ItemsSource = dt_hocky;
                iDataSource.Rows[0]["HOCKY"] = 0;
                #endregion

                #region Load namhoc search

                DataTable xdt = bus.GetAll_NAMHOC();
                cboNamhoc_search.ItemsSource = xdt;

                #endregion

                #region Load namhoc search ds

                DataTable xdata = bus.GetAll_NAMHOC();
                cboNamhoc_search_ds.ItemsSource = xdt;

                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CboHeDT_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                bus_molophocphan bus = new bus_molophocphan();
                DataTable xdtbm = bus.GetAll_KhoaHoc(Convert.ToInt32(iDataSource.Rows[0]["ID_HE_DAOTAO"]));
                cboKhoa.ItemsSource = xdtbm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CboKhoa_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                bus_molophocphan bus = new bus_molophocphan();
                DataTable xdtbm = bus.GetAll_Khoa_Nganh(Convert.ToInt32(iDataSource.Rows[0]["ID_KHOAHOC"]));
                cboNganh.ItemsSource = xdtbm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CboNganh_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                bus_molophocphan bus = new bus_molophocphan();
                DataTable xdtbm = bus.GetAll_monhoc(Convert.ToInt32(iDataSource.Rows[0]["ID_KHOAHOC_NGANH"]));
                xdtbm.Columns.Add("KIEU_HOC_CHU", typeof(string));
                foreach (DataRow dr in xdtbm.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["IS_THUCHANH"].ToString()) &&
                        !string.IsNullOrEmpty(dr["IS_LYTHUYET"].ToString()))
                    {
                        if (Convert.ToInt32(dr["IS_THUCHANH"]) == 1)
                        {
                            dr["KIEU_HOC_CHU"] = "Thực hành";
                        }
                        if (Convert.ToInt32(dr["IS_LYTHUYET"]) == 1)
                        {
                            dr["KIEU_HOC_CHU"] = "Lý thuyết";
                        }
                    }
                }
                iGridDataSource = xdtbm.Copy();
                grd_KN.ItemsSource = iGridDataSource;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BtnTaoTuDongTheoKhoa_OnClick(object sender, RoutedEventArgs e)
        {
            if (idata_send == null) return;
            if (string.IsNullOrEmpty(idata_send.Rows[0]["ID_MONHOC"].ToString())) return;
            frm_taotudonghocphan frm = new frm_taotudonghocphan();
            frm.Owner = Window.GetWindow(this);
            frm.ShowDialog();
            idata_send.Clear();
            idata_send.Rows.Add(idata_send.NewRow());
        }

        private void CboHeDTLQL_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                bus_molophocphan bus = new bus_molophocphan();
                DataTable xdtbm = bus.GetAll_KhoaHoc(Convert.ToInt32(iDataSource.Rows[0]["ID_HEDAOTAO_LQL"]));
                cboKhoaLQL.ItemsSource = xdtbm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CboKhoaLQL_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                bus_molophocphan bus = new bus_molophocphan();
                DataTable xdtbm = bus.GetAll_Lop(Convert.ToInt32(iDataSource.Rows[0]["ID_KHOAHOC_LQL"]));
                grd_lop.ItemsSource = xdtbm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GrdView_lop_OnFocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                DataRow RowSelGb = null;
                if (this.grd_lop.GetFocusedRow() == null) return;
                RowSelGb = ((DataRowView)this.grd_lop.GetFocusedRow()).Row;


                this.iDataSource.Rows[0]["TEN_NGANH_LQL"] = RowSelGb["ten_nganh"].ToString();
                iDataSource.Rows[0]["ID_LOPHOC"] = RowSelGb["ID_LOPHOC"];

                bus_molophocphan bus = new bus_molophocphan();
                DataTable idata = bus.GetAll_MH_byKhoaHoc(Convert.ToInt32(RowSelGb["ID_KHOAHOC_NGANH"]));
                idata.Columns.Add("KIEU_HOC_CHU", typeof(string));
                foreach (DataRow dr in idata.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["IS_THUCHANH"].ToString()) &&
                       !string.IsNullOrEmpty(dr["IS_LYTHUYET"].ToString()))
                    {
                        if (Convert.ToInt32(dr["IS_THUCHANH"]) == 1)
                        {
                            dr["KIEU_HOC_CHU"] = "Thực hành";
                        }
                        if (Convert.ToInt32(dr["IS_LYTHUYET"]) == 1)
                        {
                            dr["KIEU_HOC_CHU"] = "Lý thuyết";
                        }
                    }
                }
                grd_mh_lop.ItemsSource = idata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BtnTaoTuDongTheoLop_OnClick(object sender, RoutedEventArgs e)
        {
            if (idata_send == null) return;
            if (string.IsNullOrEmpty(idata_send.Rows[0]["ID_MONHOC"].ToString())) return;
            frm_taotudonghocphan frm = new frm_taotudonghocphan();
            frm.Owner = Window.GetWindow(this);
            frm.ShowDialog();
            idata_send.Clear();
            idata_send.Rows.Add(idata_send.NewRow());
        }

        private void GrdView_mh_lop_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DataRow RowSelGb = null;
                if (this.grd_mh_lop.GetFocusedRow() == null) return;
                RowSelGb = ((DataRowView)this.grd_mh_lop.GetFocusedRow()).Row;

                idata_send.Clear();
                idata_send.Rows.Add(idata_send.NewRow());
                idata_send.Rows[0]["ID_LOPHOC"] = iDataSource.Rows[0]["ID_LOPHOC"];
                idata_send.Rows[0]["ID_KHOAHOC_NGANH_CTIET"] = RowSelGb["ID_KHOAHOC_NGANH_CTIET"];
                idata_send.Rows[0]["ID_HE_DAOTAO"] = iDataSource.Rows[0]["ID_HEDAOTAO_LQL"];
                idata_send.Rows[0]["ID_MONHOC"] = RowSelGb["ID_MONHOC"];
                idata_send.Rows[0]["SO_TC"] = RowSelGb["SO_TC"];
                idata_send.Rows[0]["SOTIET"] = RowSelGb["SOTIET"];
                idata_send.Rows[0]["KIEU_HOC_CHU"] = RowSelGb["KIEU_HOC_CHU"];
                idata_send.Rows[0]["TEN_MONHOC"] = RowSelGb["TEN_MONHOC"];
                idata_send.Rows[0]["MA_MONHOC"] = RowSelGb["MA_MONHOC"];
                idata_send.Rows[0]["ID_NAMHOC_HKY_HTAI"] = iGridData_TTHK.Rows[0]["ID_NAMHOC_HKY_HTAI"];
                idata_send.Rows[0]["HOCKYHT"] = iGridData_TTHK.Rows[0]["HOCKY"];
                idata_send.Rows[0]["NAMHOCHT"] = iGridData_TTHK.Rows[0]["NAMHOC_TU"].ToString() + "-" + (Convert.ToInt32(iGridData_TTHK.Rows[0]["NAMHOC_TU"]) + 1).ToString();
                idata_send.Rows[0]["HOCKY"] = iGridData_TTHK.Rows[0]["HOCKY"];
                idata_send.Rows[0]["NAMHOC"] = iGridData_TTHK.Rows[0]["NAMHOC_TU"];
                idata_send.Rows[0]["USER"] = iDataSource.Rows[0]["USER"];
                idata_send.Rows[0]["TEN_HEDAOTAO"] = iDataSource.Rows[0]["TEN_HEDAOTAO_LQL"];
                idata_send.Rows[0]["TEN_NGANH"] = iDataSource.Rows[0]["TEN_NGANH_LQL"];

                iDataSource.Rows[0]["ID_MONHOC"] = RowSelGb["ID_MONHOC"];
                iDataSource.Rows[0]["ID_KHOAHOC_NGANH_CTIET"] = RowSelGb["ID_KHOAHOC_NGANH_CTIET"];
                iDataSource.Rows[0]["HOCKY_Search"] = iGridData_TTHK.Rows[0]["HOCKY"];
                iDataSource.Rows[0]["NAMHOC_Search"] = iGridData_TTHK.Rows[0]["NAMHOC_TU"];
                iDataSource.Rows[0]["HOCKY_Search_ds"] = iGridData_TTHK.Rows[0]["HOCKY"];
                iDataSource.Rows[0]["NAMHOC_Search_ds"] = iGridData_TTHK.Rows[0]["NAMHOC_TU"];

                bus_molophocphan bus = new bus_molophocphan();
                iGridData_dsHP_nganh = bus.GetAll_hocphan_bymh(Convert.ToInt32(RowSelGb["ID_MONHOC"]), Convert.ToInt32(iGridData_TTHK.Rows[0]["ID_NAMHOC_HKY_HTAI"]),
                    Convert.ToInt32(RowSelGb["ID_KHOAHOC_NGANH_CTIET"]));
                grd_ct.ItemsSource = iGridData_dsHP_nganh;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GrdView_KN_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DataRow RowSelGb = null;
                if (this.grd_KN.GetFocusedRow() == null) return;
                RowSelGb = ((DataRowView)this.grd_KN.GetFocusedRow()).Row;

                idata_send.Clear();
                idata_send.Rows.Add(idata_send.NewRow());
                idata_send.Rows[0]["ID_KHOAHOC_NGANH_CTIET"] = RowSelGb["ID_KHOAHOC_NGANH_CTIET"];
                idata_send.Rows[0]["ID_HE_DAOTAO"] = iDataSource.Rows[0]["ID_HE_DAOTAO"];
                idata_send.Rows[0]["ID_MONHOC"] = RowSelGb["ID_MONHOC"];
                idata_send.Rows[0]["SO_TC"] = RowSelGb["SO_TC"];
                idata_send.Rows[0]["SOTIET"] = RowSelGb["SOTIET"];
                idata_send.Rows[0]["KIEU_HOC_CHU"] = RowSelGb["KIEU_HOC_CHU"];
                idata_send.Rows[0]["TEN_MONHOC"] = RowSelGb["TEN_MONHOC"];
                idata_send.Rows[0]["MA_MONHOC"] = RowSelGb["MA_MONHOC"];
                idata_send.Rows[0]["ID_NAMHOC_HKY_HTAI"] = iGridData_TTHK.Rows[0]["ID_NAMHOC_HKY_HTAI"];
                idata_send.Rows[0]["HOCKYHT"] = iGridData_TTHK.Rows[0]["HOCKY"];
                idata_send.Rows[0]["NAMHOCHT"] = iGridData_TTHK.Rows[0]["NAMHOC_TU"].ToString() + "-" + (Convert.ToInt32(iGridData_TTHK.Rows[0]["NAMHOC_TU"]) + 1).ToString();
                idata_send.Rows[0]["HOCKY"] = iGridData_TTHK.Rows[0]["HOCKY"];
                idata_send.Rows[0]["NAMHOC"] = iGridData_TTHK.Rows[0]["NAMHOC_TU"];
                idata_send.Rows[0]["USER"] = iDataSource.Rows[0]["USER"];
                idata_send.Rows[0]["TEN_HEDAOTAO"] = iDataSource.Rows[0]["TEN_HEDAOTAO"];
                idata_send.Rows[0]["TEN_NGANH"] = iDataSource.Rows[0]["TEN_NGANH"];

                iDataSource.Rows[0]["ID_MONHOC"] = RowSelGb["ID_MONHOC"];
                iDataSource.Rows[0]["ID_KHOAHOC_NGANH_CTIET"] = RowSelGb["ID_KHOAHOC_NGANH_CTIET"];
                iDataSource.Rows[0]["HOCKY_Search"] = iGridData_TTHK.Rows[0]["HOCKY"];
                iDataSource.Rows[0]["NAMHOC_Search"] = iGridData_TTHK.Rows[0]["NAMHOC_TU"];
                iDataSource.Rows[0]["HOCKY_Search_ds"] = iGridData_TTHK.Rows[0]["HOCKY"];
                iDataSource.Rows[0]["NAMHOC_Search_ds"] = iGridData_TTHK.Rows[0]["NAMHOC_TU"];

                bus_molophocphan bus = new bus_molophocphan();
                iGridData_dsHP_nganh = bus.GetAll_hocphan_bymh(Convert.ToInt32(RowSelGb["ID_MONHOC"]), Convert.ToInt32(iGridData_TTHK.Rows[0]["ID_NAMHOC_HKY_HTAI"]),
                    Convert.ToInt32(RowSelGb["ID_KHOAHOC_NGANH_CTIET"]));
                grd_ct.ItemsSource = iGridData_dsHP_nganh;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BtnDelete_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(iDataSource.Rows[0]["ID_LOPHOCPHAN"].ToString()))
                {
                    if (CTMessagebox.Show("Bạn muốn xóa học phần " + iDataSource.Rows[0]["TEN_LOP_HOCPHAN"].ToString() + " không?", "Xóa", "", CTICON.Question, CTBUTTON.YesNo) == CTRESPONSE.Yes)
                    {
                        int tmp = 0;
                        bus_molophocphan bus = new bus_molophocphan();

                        tmp = bus.DeleteObject(Convert.ToInt32(iDataSource.Rows[0]["ID_LOPHOCPHAN"]),
                            iDataSource.Rows[0]["USER"].ToString());
                        if (tmp != 0)
                        {
                            CTMessagebox.Show("Thành công", "Xóa", "", CTICON.Information, CTBUTTON.OK);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GrdView_ct_OnFocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                DataRow RowSelGb = null;
                if (this.grd_ct.GetFocusedRow() == null) return;
                RowSelGb = ((DataRowView)this.grd_ct.GetFocusedRow()).Row;
                iDataSource.Rows[0]["ID_LOPHOCPHAN"] = RowSelGb["ID_LOPHOCPHAN"];
                iDataSource.Rows[0]["TEN_LOP_HOCPHAN"] = RowSelGb["TEN_LOP_HOCPHAN"];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CboHeDT_OnSelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                this.iDataSource.Rows[0]["TEN_HEDAOTAO"] = ((DataRowView)cboHeDT.SelectedItem)["TEN_HE_DAOTAO"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CboHeDTLQL_OnSelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                this.iDataSource.Rows[0]["TEN_HEDAOTAO_LQL"] = ((DataRowView)cboHeDTLQL.SelectedItem)["TEN_HE_DAOTAO"].ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CboNganh_OnSelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                this.iDataSource.Rows[0]["TEN_NGANH"] = ((DataRowView)cboNganh.SelectedItem)["TEN_NGANH"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GrdView_ct_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                iSua = 1;
                DataRow RowSelGb = null;
                if (this.grd_ct.GetFocusedRow() == null) return;
                RowSelGb = ((DataRowView)this.grd_ct.GetFocusedRow()).Row;
                DataTable xdt = client.GetAll_TT_byHP(Convert.ToInt32(RowSelGb["ID_LOPHOCPHAN"]));
                foreach (DataColumn item in xdt.Columns)
                {
                    if (idata_send.Columns[item.ColumnName] != null)
                    {
                        idata_send.Rows[0][item.ColumnName] = xdt.Rows[0][item.ColumnName];
                    }
                }
                idata_send.Rows[0]["HOCKYHT"] = iGridData_TTHK.Rows[0]["HOCKY"];
                idata_send.Rows[0]["NAMHOCHT"] = iGridData_TTHK.Rows[0]["NAMHOC_TU"].ToString() + "-" + (Convert.ToInt32(iGridData_TTHK.Rows[0]["NAMHOC_TU"]) + 1).ToString();
                frm_taotudonghocphan frm = new frm_taotudonghocphan();
                frm.Owner = Window.GetWindow(this);
                frm.ShowDialog();
                idata_send.Clear();
                idata_send.Rows.Add(idata_send.NewRow());
                iSua = 0;
                BtnRefresh_OnClick(null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        bus_molophocphan client = new bus_molophocphan();

        private void BtnImport_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.DefaultExt = ".xls";
                dlg.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    string filename = dlg.FileName;
                    DataTable dtExcel = exceldata(filename);

                    #region Add namhoc_hientai

                    DataTable idata_nhoc_ht = client.Getall_NamHocHT();
                    DataTable idata_new_nhht = dtExcel.Clone();
                    foreach (DataRow dr in dtExcel.Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["f_namhoc0"].ToString()))
                        {
                            if (IsCheck_nhht(idata_nhoc_ht, dr["f_namhoc0"].ToString()))
                            {
                                idata_new_nhht.ImportRow(dr);
                                DataRow xdr = idata_nhoc_ht.NewRow();
                                xdr["NAMHOC_TU"] = dr["f_namhoc0"];
                                idata_nhoc_ht.Rows.Add(xdr);
                            }
                        }
                    }
                    if (idata_new_nhht.Rows.Count > 0)
                    {
                        client.Insert_NamHocHT_Excel(idata_new_nhht, iDataSource.Rows[0]["USER"].ToString());
                    }

                    #endregion

                    #region Add hocky_namhoc_hientai

                    DataTable idata_hky_ht = client.Getall_NamHoc_hkyHT();
                    DataTable idata_new_HkyHT = dtExcel.Clone();
                    foreach (DataRow dr in dtExcel.Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["f_namhoc0"].ToString()) &&
                            !string.IsNullOrEmpty(dr["f_hockythu"].ToString()))
                        {
                            if (IsCheck_Hkyht(idata_hky_ht, dr["f_namhoc0"].ToString(), dr["f_hockythu"].ToString()))
                            {
                                idata_new_HkyHT.ImportRow(dr);
                                DataRow xdr = idata_hky_ht.NewRow();
                                xdr["NAMHOC_TU"] = dr["f_namhoc0"];
                                xdr["HOCKY"] = dr["f_hockythu"];
                                idata_hky_ht.Rows.Add(xdr);
                            }
                        }
                    }
                    if (idata_new_HkyHT.Rows.Count > 0)
                    {
                        client.Insert_HkyHT_Excel(idata_new_HkyHT, iDataSource.Rows[0]["USER"].ToString());
                    }

                    #endregion

                    #region Add lophocphan

                    int xcheck = 0;
                    DataTable idata_lophocphan = client.Getall_lopHocPhan();
                    DataTable idata_new_lhp = dtExcel.Clone();
                    foreach (DataRow dr in dtExcel.Rows)
                    {
                        if (IsCheck_lhp(idata_lophocphan, dr["f_mamhhtd"].ToString()))
                        {
                            idata_new_lhp.ImportRow(dr);
                            DataRow xdr = idata_lophocphan.NewRow();
                            xdr["MA_LOP_HOCPHAN"] = dr["f_mamhhtd"];
                            idata_lophocphan.Rows.Add(xdr);
                        }
                    }
                    if (idata_new_lhp.Rows.Count > 0)
                    {
                        xcheck = client.Insert_lhp_Excel(idata_new_lhp, iDataSource.Rows[0]["USER"].ToString());
                        if (xcheck != 0)
                        {
                            CTMessagebox.Show("Thành công", "Nhập từ Excel", "", CTICON.Information, CTBUTTON.OK);
                            BtnRefresh_ds_OnClick(null, null);
                        }
                        else
                        {
                            CTMessagebox.Show("Lỗi", "Nhập từ Excel", "", CTICON.Information, CTBUTTON.OK);
                        }
                    }
                    else
                    {
                        CTMessagebox.Show("Thành công", "Nhập từ Excel", "", CTICON.Information, CTBUTTON.OK);
                        BtnRefresh_ds_OnClick(null, null);
                    }

                    #endregion
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

        private bool IsCheck_nhht(DataTable dt, string namhochientai)
        {
            if (dt != null)
            {
                DataRow[] xcheck = dt.Select("NAMHOC_TU = " + namhochientai + "");
                if (xcheck.Count() > 0)
                    return false;
            }
            return true;
        }

        private bool IsCheck_Hkyht(DataTable dt, string namhochientai, string hocky)
        {
            if (dt != null)
            {
                DataRow[] xcheck = dt.Select("NAMHOC_TU = " + namhochientai + " and HOCKY = " + hocky);
                if (xcheck.Count() > 0)
                    return false;
            }
            return true;
        }

        private bool IsCheck_lhp(DataTable dt, string pid_lophocphan)
        {
            if (dt != null)
            {
                DataRow[] xcheck = dt.Select("MA_LOP_HOCPHAN = '" + pid_lophocphan + "'");
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
            foreach (DataRow schemaRow in schemaTable.Rows)
            {
                string sheet = schemaRow["TABLE_NAME"].ToString();
                if (!sheet.EndsWith("_"))
                {
                    string query = "SELECT f_mamh,f_mamhhtd,f_phtrambt,f_phtramkt,f_namhoc0,f_namthu,f_hockythu,f_tenmhvn FROM [" + sheet + "]";
                    OleDbDataAdapter daexcel = new OleDbDataAdapter(query, conn);
                    dtexcel.Locale = CultureInfo.CurrentCulture;
                    daexcel.Fill(dtexcel);
                }
            }
            conn.Close();
            return dtexcel;
        }

        private void BtnRefresh_ds_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                iDataSource.Rows[0]["HOCKY_Search_ds"] = iGridData_TTHK.Rows[0]["HOCKY"];
                iDataSource.Rows[0]["NAMHOC_Search_ds"] = iGridData_TTHK.Rows[0]["NAMHOC_TU"];
                iGridData_dsHP = client.GetAll_hocphan_ds(Convert.ToInt32(iGridData_TTHK.Rows[0]["ID_NAMHOC_HKY_HTAI"]));
                grd_ds.ItemsSource = iGridData_dsHP;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CboHocKy_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(iDataSource.Rows[0]["HOCKY"]) == 0)
                {
                    grd_KN.ItemsSource = iGridDataSource;
                }
                else
                {
                    if (iGridDataSource != null && iGridDataSource.Rows.Count > 0)
                    {
                        DataTable dt = null;
                        DataRow[] xcheck = (from x in
                                                iGridDataSource.AsEnumerable()
                                                    .Where(
                                                        d =>
                                                            d.Field<int>("HOCKY") ==
                                                            Convert.ToInt32(iDataSource.Rows[0]["HOCKY"].ToString()))
                                            select x).ToArray();
                        if (xcheck.Count() > 0)
                        {
                            dt = xcheck.CopyToDataTable();

                        }
                        else
                        {
                            dt = iGridDataSource.Clone();
                        }
                        grd_KN.ItemsSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GrdView_KN_OnFocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            GrdView_KN_OnMouseDown(null, null);
        }

        private void GrdView_mh_lop_OnFocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            GrdView_mh_lop_OnMouseDown(null, null);
        }

        private void CboNamhoc_search_OnSelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                bus_molophocphan bus = new bus_molophocphan();
                DataTable xdt = bus.GetAll_HOCKY_ByNH(Convert.ToInt32(iDataSource.Rows[0]["NAMHOC_Search"]));
                cbohocky_search.ItemsSource = xdt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Cbohocky_search_OnSelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(iDataSource.Rows[0]["ID_MONHOC"].ToString()))
                {
                    bus_molophocphan bus = new bus_molophocphan();
                    DataTable xdt = bus.GetAll_HKYHT(Convert.ToInt32(iDataSource.Rows[0]["NAMHOC_Search"]),
                        Convert.ToInt32(iDataSource.Rows[0]["HOCKY_Search"]));
                    iGridData_dsHP_nganh = bus.GetAll_hocphan_bymh(Convert.ToInt32(iDataSource.Rows[0]["ID_MONHOC"]),
                        Convert.ToInt32(xdt.Rows[0]["ID_NAMHOC_HKY_HTAI"]),
                        Convert.ToInt32(iDataSource.Rows[0]["ID_KHOAHOC_NGANH_CTIET"]));
                    grd_ct.ItemsSource = iGridData_dsHP_nganh;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CboNamhoc_search_ds_OnSelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                bus_molophocphan bus = new bus_molophocphan();
                DataTable xdt = bus.GetAll_HOCKY_ByNH(Convert.ToInt32(iDataSource.Rows[0]["NAMHOC_Search_ds"]));
                cbohocky_search_ds.ItemsSource = xdt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Cbohocky_search_ds_OnSelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                bus_molophocphan bus = new bus_molophocphan();
                DataTable xdt = bus.GetAll_HKYHT(Convert.ToInt32(iDataSource.Rows[0]["NAMHOC_Search_ds"]),
                    Convert.ToInt32(iDataSource.Rows[0]["HOCKY_Search_ds"]));

                iGridData_dsHP = client.GetAll_hocphan_ds(Convert.ToInt32(xdt.Rows[0]["ID_NAMHOC_HKY_HTAI"]));
                grd_ds.ItemsSource = iGridData_dsHP;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BtnDelete_ds_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(iDataSource.Rows[0]["ID_LOPHOCPHAN"].ToString()))
                {
                    if (CTMessagebox.Show("Bạn muốn xóa học phần " + iDataSource.Rows[0]["TEN_LOP_HOCPHAN"].ToString() + " không?", "Xóa", "", CTICON.Question, CTBUTTON.YesNo) == CTRESPONSE.Yes)
                    {
                        int tmp = 0;
                        bus_molophocphan bus = new bus_molophocphan();

                        tmp = bus.DeleteObject(Convert.ToInt32(iDataSource.Rows[0]["ID_LOPHOCPHAN"]),
                            iDataSource.Rows[0]["USER"].ToString());
                        if (tmp != 0)
                        {
                            CTMessagebox.Show("Thành công", "Xóa", "", CTICON.Information, CTBUTTON.OK);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GrdView_ds_OnFocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                DataRow RowSelGb = null;
                if (this.grd_ds.GetFocusedRow() == null) return;
                RowSelGb = ((DataRowView)this.grd_ds.GetFocusedRow()).Row;
                iDataSource.Rows[0]["ID_LOPHOCPHAN"] = RowSelGb["ID_LOPHOCPHAN"];
                iDataSource.Rows[0]["TEN_LOP_HOCPHAN"] = RowSelGb["TEN_LOP_HOCPHAN"];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    try
        //    {
        //        if (this.tabKHMoLop.IsSelected)
        //        {
        //            if (iGridData_dsHP_nganh != null)
        //            {
        //                iGridData_dsHP_nganh.Clear();
        //            }
        //        }
        //        if (this.tabMLTLQL.IsSelected)
        //        {
        //            if (iGridData_dsHP_nganh != null)
        //            {
        //                iGridData_dsHP_nganh.Clear();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        private void GrdView_ds_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                iSua = 1;
                DataRow RowSelGb = null;
                if (this.grd_ds.GetFocusedRow() == null) return;
                RowSelGb = ((DataRowView)this.grd_ds.GetFocusedRow()).Row;
                DataTable xdt = client.GetAll_TT_byHP(Convert.ToInt32(RowSelGb["ID_LOPHOCPHAN"]));
                foreach (DataColumn item in xdt.Columns)
                {
                    if (idata_send.Columns[item.ColumnName] != null)
                    {
                        idata_send.Rows[0][item.ColumnName] = xdt.Rows[0][item.ColumnName];
                    }
                }
                idata_send.Rows[0]["HOCKYHT"] = iGridData_TTHK.Rows[0]["HOCKY"];
                idata_send.Rows[0]["NAMHOCHT"] = iGridData_TTHK.Rows[0]["NAMHOC_TU"].ToString() + "-" + (Convert.ToInt32(iGridData_TTHK.Rows[0]["NAMHOC_TU"]) + 1).ToString();
                frm_taotudonghocphan frm = new frm_taotudonghocphan();
                frm.Owner = Window.GetWindow(this);
                frm.ShowDialog();
                idata_send.Clear();
                idata_send.Rows.Add(idata_send.NewRow());
                iSua = 0;
                BtnRefresh_ds_OnClick(null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BtnRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(iDataSource.Rows[0]["ID_MONHOC"].ToString()))
                {
                    bus_molophocphan bus = new bus_molophocphan();
                    DataTable xdt = bus.GetAll_HKYHT(Convert.ToInt32(iDataSource.Rows[0]["NAMHOC_Search"]),
                        Convert.ToInt32(iDataSource.Rows[0]["HOCKY_Search"]));
                    iGridData_dsHP_nganh = bus.GetAll_hocphan_bymh(Convert.ToInt32(iDataSource.Rows[0]["ID_MONHOC"]),
                        Convert.ToInt32(xdt.Rows[0]["ID_NAMHOC_HKY_HTAI"]),
                        Convert.ToInt32(iDataSource.Rows[0]["ID_KHOAHOC_NGANH_CTIET"]));
                    grd_ct.ItemsSource = iGridData_dsHP_nganh;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
