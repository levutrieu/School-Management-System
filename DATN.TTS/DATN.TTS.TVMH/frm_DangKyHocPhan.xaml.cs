using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
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
using DATN.TTS.TVMH.Resource;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using DevExpress.XtraSpreadsheet.Commands.Internal;
using DevExpress.XtraSpreadsheet.Model;
using Excel = Microsoft.Office.Interop.Excel;
namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_DangKyHocPhan.xaml
    /// </summary>
    public partial class frm_DangKyHocPhan : Page
    {
        private DataTable iDataSoure = null;
        private DataTable iGridDataSoureHP = null;
        private DataTable iGridDataSoureHPDK = null;
        List<int> lstIDLopHocPhanDK = new List<int>();// get cac lop da dang ky
        List<string> LstMonHocCheckTrung = new List<string>();// get ma mon hoc da dang ky
        bus_LapKeHoachDaoTaoKhoa client = new bus_LapKeHoachDaoTaoKhoa();
        bus_DangKyHocPhan DangKyHocPhan = new bus_DangKyHocPhan();

        private int idthu;
        private int idLopHoc = 0;
        private int idnganh=0;
        private int idkhoanganh=0;
        private int idkhoahoc=0;
        private int idmonhoc = 0;
        public static string Masinhvien = "";
        public static DataTable iDataID3 = null;
        public static DataTable iDataC45 = null;
        private string _MaSinhVien = "";
        private string _SinhVien = "";
        private string _Lop = "";
        private string _Nganh = "";
        private string _HeDaoTao = "";
        private string _KhoaHoc = "";
        public frm_DangKyHocPhan()
        {
            InitializeComponent();
            this.iDataSoure = TableSchemaBinding();
            this.DataContext = iDataSoure;

            this.iDataSoure.Rows[0]["USER"] = UserCommon.UserName;
            this.iDataSoure.Rows[0]["MA_SINHVIEN"] = "2001120008";
            InitGrid_LopHP();
            InitGrid_HPDangKy();
        }

        private DataTable TableSchemaBinding()
        {
            try
            {
                DataTable dt = null;
                Dictionary<string, Type> dic = new Dictionary<string, Type>();
                dic.Add("THU", typeof(Decimal));
                dic.Add("NGAY", typeof(string));
                dic.Add("USER", typeof(string));
                dic.Add("ID_HE_DAOTAO", typeof(int));
                dic.Add("ID_KHOAHOC", typeof(int));
                dic.Add("ID_NGANH", typeof(int));
                dic.Add("ID_LOPHOC", typeof(int));
                dic.Add("ID_MONHOC", typeof(int));
                dic.Add("ID_SINHVIEN", typeof(Decimal));
                dic.Add("MA_SINHVIEN", typeof(string));
                dt = TableUtil.ConvertToTable(dic);
                return dt;
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            return null;
        }

        private DataTable TableSchemaBindingGridHPDK()
        {
            try
            {
                DataTable dt = new DataTable();
                Dictionary<string, Type> dic = new Dictionary<string, Type>();
                dic.Add("ID_DANGKY", typeof(Decimal));
                dic.Add("MA_MONHOC", typeof(string));
                dic.Add("TEN_MONHOC", typeof(string));
                dic.Add("ID_LOPHOCPHAN", typeof(int));
                dic.Add("MA_LOP_HOCPHAN", typeof(string));
                dic.Add("TEN_LOP_HOCPHAN", typeof(string));
                dic.Add("ID_THAMSO", typeof(Decimal));
                dic.Add("ID_SINHVIEN", typeof(Decimal));
                dic.Add("NGAY_DANGKY", typeof(DateTime));
                dic.Add("GIO_DANGKY", typeof(string));
                dic.Add("DON_GIA", typeof(Decimal));
                dic.Add("THANH_TIEN", typeof(Decimal));
                dic.Add("TRANGTHAI", typeof(string));
                dic.Add("SO_TC", typeof(Decimal));
                dt = TableUtil.ConvertDictionaryToTable(dic, false);
                return dt;
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            return null;
        }

        void InitGrid_LopHP()
        {
            try
            {
                GridColumn col;
                col = new GridColumn();
                col.FieldName = "CHK";
                col.Header = "DK";
                col.Width = 40;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                CheckEditSettings chkcheck = new CheckEditSettings();
                col.EditSettings = chkcheck;
                col.UnboundType = UnboundColumnType.Boolean;
                grdLopHP.Columns.Add(col);

                #region Hide
                col = new GridColumn();
                col.FieldName = "ID_LOPHOCPHAN";
                col.Header = string.Empty;
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;

                grdLopHP.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "ID_KHOAHOC_NGANH_CTIET";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;

                grdLopHP.Columns.Add(col);


                col = new GridColumn();
                col.FieldName = "ID_NAMHOC_HKY_HTAI";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;

                grdLopHP.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "ID_HEDAOTAO";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;

                grdLopHP.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "ID_MONHOC";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;

                grdLopHP.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "ID_LOPHOC";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;

                grdLopHP.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "ID_GIANGVIEN";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;

                grdLopHP.Columns.Add(col);

                #endregion

                #region Hide HP
                col = new GridColumn();
                col.FieldName = "MA_LOP_HOCPHAN";
                col.Header = "Mã học phần";
                col.Width = 120;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdLopHP.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TEN_LOP_HOCPHAN";
                col.Header = "Tên học phần";
                col.Width = 140;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                grdLopHP.Columns.Add(col);

                //col = new GridColumn();
                //col.FieldName = "TEN_LOP";
                //col.Header = "Lớp QL";
                //col.Width = 80;
                //col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                //col.AllowEditing = DefaultBoolean.False;
                //col.Visible = false;
                //col.EditSettings = new TextEditSettings();
                //col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                //grdLopHP.Columns.Add(col);

                #endregion

                #region Attribiute
                col = new GridColumn();
                col.FieldName = "MA_MONHOC";
                col.Header = "Mã môn học";
                col.Width = 80;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdLopHP.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TEN_MONHOC";
                col.Header = "Tên môn học";
                col.Width = 250;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                grdLopHP.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "SO_TC";
                col.Header = "STC";
                col.Width = 35;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdLopHP.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "SOLUONG";
                col.Header = "Sỉ số";
                col.Width = 35;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdLopHP.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "SOSVDKY";
                col.Header = "Đã DK";
                col.Width = 35;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdLopHP.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TEN_GIANGVIEN";
                col.Header = "Giảng viên";
                col.Width = 90;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdLopHP.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "THOIKHOABIEU";
                col.Header = "Thời khóa biểu";
                col.Width = 150;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                TextEditSettings txtTKB = new TextEditSettings();
                txtTKB.TextWrapping = TextWrapping.Wrap;
                col.EditSettings = txtTKB;
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdLopHP.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TUAN_BD";
                col.Header = "Tuần BD";
                col.Width = 35;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;

                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdLopHP.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TUAN_KT";
                col.Header = "Tuần KT";
                col.Width = 35;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdLopHP.Columns.Add(col);
                #endregion
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
        }

        void InitGrid_HPDangKy()
        {
            try
            {
                GridColumn col;
                col = new GridColumn();
                col.FieldName = "ID_DANGKY";
                col.Header = string.Empty;
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;

                grDanhSachDK.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "ID_SINHVIEN";
                col.Header = string.Empty;
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;

                grDanhSachDK.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "ID_LOPHOCPHAN";
                col.Header = "Lớp học phần";
                col.Width = 90;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                grDanhSachDK.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "MA_MONHOC";
                col.Header = "Mã môn học";
                col.Width = 150;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grDanhSachDK.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TEN_MONHOC";
                col.Header = "Tên môn học";
                col.Width = 250;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                grDanhSachDK.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "MA_LOP_HOCPHAN";
                col.Header = "Mã học phần";
                col.Width = 150;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grDanhSachDK.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TEN_LOP_HOCPHAN";
                col.Header = "Tên học phần";
                col.Width = 250;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                grDanhSachDK.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "SO_TC";
                col.Header = "Số TC";
                col.Width = 80;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;

                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grDanhSachDK.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "DON_GIA";
                col.Header = "Học phí";
                col.Width = 130;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;

                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grDanhSachDK.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "THANH_TIEN";
                col.Header = "Phải đóng";
                col.Width = 150;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;

                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grDanhSachDK.Columns.Add(col);


                col = new GridColumn();
                col.FieldName = "NGAY_DANGKY";
                col.Header = "Ngày DK";
                col.Width = 100;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;

                //col.EditSettings = new TextEditSettings();
                //col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grDanhSachDK.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TRANGTHAI";
                col.Header = "Trạng thái chọn môn học";
                col.Width = 170;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;

                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grDanhSachDK.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "CHK";
                col.Header = "Xóa";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;

                col.EditSettings = new CheckEditSettings();
                col.UnboundType = UnboundColumnType.Boolean;
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grDanhSachDK.Columns.Add(col);
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
        }

        void LoadGridLopHocPhan()
        {
            try
            {
                iGridDataSoureHP = DangKyHocPhan.GetDanhSachLopHocPhan(idkhoahoc, idnganh);
                iGridDataSoureHP.Columns.Add("CHK");
                foreach (DataRow row in iGridDataSoureHP.Rows)
                {
                    row["CHK"] = "False";
                }
                this.grdLopHP.ItemsSource = iGridDataSoureHP;
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
        }

        void SetComBoboxKhoa()
        {
            int pid = Convert.ToInt32(this.iDataSoure.Rows[0]["ID_HE_DAOTAO"].ToString());
            DataTable dt = client.GetAllKhoaHoc(pid);
            if (dt.Rows.Count > 0)
            {
                ComboBoxUtil.SetComboBoxEdit(CboKhoa, "TEN_KHOAHOC", "ID_KHOAHOC", dt, SelectionTypeEnum.Native);
            }
        }

        void SetComBoNganh(int idkhoahoc)
        {
            DataTable dt = DangKyHocPhan.GetNganhWhereKhoaHoc(idkhoahoc);
            if (dt.Rows.Count > 0)
            {
                ComboBoxUtil.SetComboBoxEdit(cboNganh,"TEN_NGANH" ,"ID_NGANH", dt, SelectionTypeEnum.Native);
                //ComboBoxUtil.InsertItem(cboNganh, "-------------------------------------------------Tất cả--------------------------------------------", "0", 0, false);
                this.iDataSoure.Rows[0]["ID_NGANH"] = cboNganh.GetKeyValue(0);
            }
        }

        //void SetComBoLophoc(int _idkhoahoc, int _idnganh)
        //{
        //    DataTable dt = DangKyHocPhan.GetLopHoc(_idkhoahoc, _idnganh);
        //    if (dt.Rows.Count > 0)
        //    {
        //        ComboBoxUtil.SetComboBoxEdit(cboLopHoc,"TEN_LOP", "ID_LOPHOC", dt, SelectionTypeEnum.Native);
        //        ComboBoxUtil.InsertItem(cboLopHoc,"---------------------------------------------Tất cả-------------------------------------------","0",0, false);
        //        this.iDataSoure.Rows[0]["ID_LOPHOC"] = cboLopHoc.GetKeyValue(0);
        //    }
        //}

        private void SetComboMonHoc(int idkhoahoc, int idnganh)
        {
            try
            {
                DataTable dt = DangKyHocPhan.GetMonHoc(idkhoahoc, idnganh);
                if (dt.Rows.Count > 0)
                {
                    ComboBoxUtil.SetComboBoxEdit(cboMonHoc, "TEN_MONHOC", "ID_MONHOC", dt, SelectionTypeEnum.Native);
                    ComboBoxUtil.InsertItem(cboMonHoc, "---------------------------------------------Tất cả-------------------------------------------", "0", 0, false);
                    this.iDataSoure.Rows[0]["ID_MONHOC"] = cboMonHoc.GetKeyValue(0);
                }
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
        }

        private void BtnHoTroID3_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Masinhvien = this.iDataSoure.Rows[0]["MA_SINHVIEN"].ToString();
                if (iGridDataSoureHP != null && iGridDataSoureHP.Rows.Count > 0)
                {
                    DataRow[] xDataSearch = (from x in
                                                 iGridDataSoureHP.AsEnumerable()
                                                     .Where(
                                                         d =>
                                                             d.Field<int>("ISBATBUOC") == 0)
                                             select x).ToArray();
                    if (xDataSearch.Count() > 0)
                    {
                        iDataID3 = xDataSearch.CopyToDataTable();

                    }
                    else
                    {
                        iDataID3 = iGridDataSoureHP.Clone();
                    }
                }
                frm_ID3 frm = new frm_ID3();
                frm.Owner = Window.GetWindow(this);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
        }

        private void BtnHoTroC45_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Masinhvien = this.iDataSoure.Rows[0]["MA_SINHVIEN"].ToString();
                if (iGridDataSoureHP != null && iGridDataSoureHP.Rows.Count > 0)
                {
                    DataRow[] xDataSearch = (from x in
                                                 iGridDataSoureHP.AsEnumerable()
                                                     .Where(
                                                         d =>
                                                             d.Field<int>("ISBATBUOC") == 0)
                                             select x).ToArray();
                    if (xDataSearch.Count() > 0)
                    {
                        iDataID3 = xDataSearch.CopyToDataTable();

                    }
                    else
                    {
                        iDataID3 = iGridDataSoureHP.Clone();
                    }
                }
                frm_C45 frm = new frm_C45();
                frm.Owner = Window.GetWindow(this);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
        }

        bool CheckTrungLopHocPhan(int ID_LOPHOCPHAN)
        {
            int count = 0;
            for (int i = 0; i < lstIDLopHocPhanDK.Count; i++)
            {
                if (lstIDLopHocPhanDK[i] == ID_LOPHOCPHAN)
                    return false;
            }
            return true;
        }

        private void BtnXemHocPhanDK_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                #region Kiểm tra thong tin nhập vào là mã sinh viên
                lstIDLopHocPhanDK.Clear();
                LstMonHocCheckTrung.Clear();
                if (this.iDataSoure.Rows[0]["MA_SINHVIEN"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập mã sinh viên để thực hiện các thao tác tiếp theo!!", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                    txtMASV.Focus();
                    return;
                }
                int idsinhvien = DangKyHocPhan.GetIDSinhVien(this.iDataSoure.Rows[0]["MA_SINHVIEN"].ToString().Trim());
                if (idsinhvien == 0)
                {
                    CTMessagebox.Show("Không tìm thấy sinh viên này trong hệ thống!!" + "\n" + "Vui lòng thử lại.!!", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                    txtMASV.Focus();
                    return;
                }
                #endregion
                this.iDataSoure.Rows[0]["ID_SINHVIEN"] = idsinhvien;
                DataTable thongtin = DangKyHocPhan.GetThongTinSinhVien(idsinhvien);
                if (thongtin.Rows.Count > 0)
                {
                    _MaSinhVien = thongtin.Rows[0]["MA_SINHVIEN"].ToString();
                    _SinhVien = thongtin.Rows[0]["TEN_SINHVIEN"].ToString();
                    _Nganh = thongtin.Rows[0]["TEN_NGANH"].ToString();
                    _KhoaHoc = thongtin.Rows[0]["TEN_KHOAHOC"].ToString();
                    _Lop = thongtin.Rows[0]["TEN_LOP"].ToString();
                    _HeDaoTao = thongtin.Rows[0]["TEN_HE_DAOTAO"].ToString();
                    this.iDataSoure.Rows[0]["ID_HE_DAOTAO"] = thongtin.Rows[0]["ID_HE_DAOTAO"];
                    this.iDataSoure.Rows[0]["ID_KHOAHOC"] = thongtin.Rows[0]["ID_KHOAHOC"];
                    this.iDataSoure.Rows[0]["ID_NGANH"] = thongtin.Rows[0]["ID_NGANH"];
                }
                #region
                iGridDataSoureHPDK = DangKyHocPhan.GetLopHPDK(idsinhvien);
                iGridDataSoureHPDK.Columns.Add("CHK");
                this.grDanhSachDK.ItemsSource = iGridDataSoureHPDK;

                LoadGridLopHocPhan();
                SetComBoboxKhoa();

                if (iGridDataSoureHPDK.Rows.Count > 0)
                {
                    foreach (DataRow r in iGridDataSoureHPDK.Copy().Rows)
                    {
                        lstIDLopHocPhanDK.Add(Convert.ToInt32(r["ID_LOPHOCPHAN"]));
                        LstMonHocCheckTrung.Add(r["MA_MONHOC"].ToString());
                    }
                }
                if (lstIDLopHocPhanDK.Count > 0)
                {
                    for (int i = 0; i < lstIDLopHocPhanDK.Count; i++)
                    {
                        int temp = lstIDLopHocPhanDK[i];
                        for (int j = 0; j < iGridDataSoureHP.Rows.Count; j++)
                        {
                            if (temp == Convert.ToInt32(iGridDataSoureHP.Rows[j]["ID_LOPHOCPHAN"].ToString()))
                            {
                                iGridDataSoureHP.Rows[j]["CHK"] = "True";
                            }
                        }
                    }
                }
                #endregion
                grdLopHP.ItemsSource = iGridDataSoureHP;

            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void BtnLuuDK_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (this.iDataSoure.Rows[0]["MA_SINHVIEN"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập mã sinh viên. Và bấm xem học phần đã đăng ký." + "\n" + "Rồi thực hiện đăng đăng ký học phần", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                    txtMASV.Focus();
                    return;
                }
                if (this.iDataSoure.Rows[0]["ID_SINHVIEN"].ToString() == "0" || this.iDataSoure.Rows[0]["ID_SINHVIEN"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Không tìm thấy sinh viên có mã này trong hệ thống." + "\n" + "Vui lòng kiểm tra lại.!", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                    txtMASV.Focus();
                    return;
                }
                if (iGridDataSoureHPDK.Rows.Count > 0)
                {
                    DataTable dt = null;
                    DataRow[] check = (from temp in iGridDataSoureHPDK.AsEnumerable().Where(t => t.Field<string>("CHK") != "True") select temp).ToArray();
                    if (check.Count() > 0)
                    {
                        dt = check.CopyToDataTable();
                    }
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        bool res = DangKyHocPhan.Insert_HocPhanDK(dt.Copy(), UserCommon.UserName);
                        if (res)
                        {
                            CTMessagebox.Show("Đăng ký học phần thành công", "Đăng ký học phần", "", CTICON.Information, CTBUTTON.OK);
                            BtnXemHocPhanDK_OnClick(sender, e);
                        }
                        else
                        {
                            CTMessagebox.Show("Đăng ký học phần không thành công", "Đăng ký học phần", "", CTICON.Error, CTBUTTON.OK);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void BtnHuyDK_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (this.iDataSoure.Rows[0]["MA_SINHVIEN"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập mã sinh viên. Và bấm xem học phần đã đăng ký." + "\n" + "Rồi thực hiện đăng đăng ký học phần", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                    txtMASV.Focus();
                    return;
                }
                if (this.iDataSoure.Rows[0]["ID_SINHVIEN"].ToString() == "0" || this.iDataSoure.Rows[0]["ID_SINHVIEN"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Không tìm thấy sinh viên có mã này trong hệ thống." + "\n" + "Vui lòng kiểm tra lại.!", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                    txtMASV.Focus();
                    return;
                }
                if (iGridDataSoureHPDK.Rows.Count > 0)
                {
                    DataTable dt = null;
                    DataRow[] check = (from temp in iGridDataSoureHPDK.AsEnumerable().Where(t => t.Field<string>("CHK") == "True") select temp).ToArray();
                    if (check.Count() > 0)
                    {
                        dt = check.CopyToDataTable();
                    }
                    if (dt != null && dt.Rows.Count > 0)
                    {

                        bool res = DangKyHocPhan.Insert_DangKyHuy(dt.Copy(), UserCommon.UserName);
                        if (res)
                        {
                            CTMessagebox.Show("Hủy đăng ký học phần thành công", "Hủy đăng ký học phần", "", CTICON.Information, CTBUTTON.OK);
                            BtnXemHocPhanDK_OnClick(sender, e);
                        }
                        else
                        {
                            CTMessagebox.Show("Hủy đăng ký học phần không thành công", "Hủy đăng ký học phần", "", CTICON.Information, CTBUTTON.OK);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        DataTable UncheckGrid(int idlophocphan, DataTable dtTable)
        {
            DataTable dt = dtTable.Clone();
            if (dtTable.Rows.Count > 0)
            {
                foreach (DataRow r in dtTable.Rows)
                {
                    if (r["ID_DANGKY"].ToString() == "0")
                    {
                        if (idlophocphan != Convert.ToInt32(r["ID_LOPHOCPHAN"].ToString()))
                        {
                            dt.ImportRow(r);
                        }
                    }
                    if (r["ID_DANGKY"].ToString() != "0" || r["ID_DANGKY"].ToString() != string.Empty)
                    {
                        if (idlophocphan == Convert.ToInt32(r["ID_LOPHOCPHAN"].ToString()))
                        {
                            r["CHK"] = "True";
                            dt.ImportRow(r);
                        }
                        else
                        {
                            r["CHK"] = "";
                            dt.ImportRow(r);
                        }
                    }
                }
            }
            return dt;
        }

        private void GrdViewLopHP_OnCellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            #region CheckGrid
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                int index = this.grdViewLopHP.FocusedRowHandle;
                if (iGridDataSoureHPDK != null && iGridDataSoureHPDK.Rows.Count > 0)
                {
                    #region Neu row tại index = true
                    if (iGridDataSoureHP.Rows[index]["CHK"].ToString() == "True")
                    {
                        int SoLuongTongSV = Convert.ToInt32(iGridDataSoureHP.Rows[index]["SOLUONG"].ToString());
                        int SoLuongDaDK = Convert.ToInt32(iGridDataSoureHP.Rows[index]["SOSVDKY"].ToString());
                        if (SoLuongTongSV > SoLuongDaDK)
                        {
                            #region Xu ly khi truong hop tai index = true
                            bool res = CheckTrungLopHocPhan(Convert.ToInt32(iGridDataSoureHP.Rows[index]["ID_LOPHOCPHAN"].ToString()));
                            if (res)
                            {

                                #region Check trung học phần
                                foreach (DataRow dr in iGridDataSoureHPDK.Rows)
                                {
                                    string temp = iGridDataSoureHP.Rows[index]["MA_MONHOC"].ToString().Trim();
                                    if (dr["MA_MONHOC"].ToString().Trim().Equals(temp))
                                    {
                                        CTMessagebox.Show("Môn học này đã được đăng ký." + "\n" + "Bạn có thể hủy môn học đã đăng ký trước đó để đăng ký mới", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                                        iGridDataSoureHP.Rows[index]["CHK"] = "";
                                        grdLopHP.ItemsSource = iGridDataSoureHP;
                                        return;
                                    }
                                }
                                #endregion
                                DataRow r = iGridDataSoureHPDK.NewRow();
                                r["ID_DANGKY"] = 0;
                                r["MA_MONHOC"] = iGridDataSoureHP.Rows[index]["MA_MONHOC"];
                                r["TEN_MONHOC"] = iGridDataSoureHP.Rows[index]["TEN_MONHOC"];
                                r["ID_LOPHOCPHAN"] = iGridDataSoureHP.Rows[index]["ID_LOPHOCPHAN"];

                                r["MA_LOP_HOCPHAN"] = iGridDataSoureHP.Rows[index]["MA_LOP_HOCPHAN"];
                                r["TEN_LOP_HOCPHAN"] = iGridDataSoureHP.Rows[index]["TEN_LOP_HOCPHAN"];

                                r["ID_THAMSO"] = 0;
                                r["ID_SINHVIEN"] = this.iDataSoure.Rows[0]["ID_SINHVIEN"];
                                r["NGAY_DANGKY"] = System.DateTime.Now;
                                r["GIO_DANGKY"] = DateTime.Now.ToString("HH:mm:ss");
                                r["SO_TC"] = iGridDataSoureHP.Rows[index]["SO_TC"];
                                r["DON_GIA"] = DangKyHocPhan.GetHocPhi_LT(Convert.ToInt32(this.iDataSoure.Rows[0]["ID_HE_DAOTAO"].ToString()), Convert.ToInt32(iGridDataSoureHP.Rows[index]["IS_LYTHUYET"].ToString()));
                                r["THANH_TIEN"] = (double)Convert.ToDouble(r["SO_TC"].ToString()) * Convert.ToDouble(r["DON_GIA"].ToString());
                                r["TRANGTHAI"] = "Chưa lưu";
                                iGridDataSoureHPDK.Rows.Add(r);

                                iGridDataSoureHP.Rows[index]["SOSVDKY"] = SoLuongDaDK + 1;
                            }
                            else
                            {
                                CTMessagebox.Show("Học phần này đã được đăng ký." + "\n" + "Bạn có thể hủy lớp đã đăng ký trước đó để đăng ký mới", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                                iGridDataSoureHP.Rows[index]["CHK"] = "";
                                grdLopHP.ItemsSource = iGridDataSoureHP;
                                return;
                            }
                            #endregion
                        }
                        else
                        {
                            CTMessagebox.Show("Lớp đã đủ số lượng.!", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                            iGridDataSoureHP.Rows[index]["CHK"] = "";
                        }
                    }
                    #endregion

                    else
                    {
                        #region Neu row tai index = false
                        if (iGridDataSoureHP.Rows[index]["CHK"].ToString() == "False")
                        {
                            DataTable zdt = UncheckGrid(Convert.ToInt32(iGridDataSoureHP.Rows[index]["ID_LOPHOCPHAN"].ToString()), iGridDataSoureHPDK.Copy());
                            if (CTMessagebox.Show("Bạn vừa bỏ chọn một lớp đã đăng ký." + "\n" + "Bạn có muốn hủy đăng ký không", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo) == CTRESPONSE.Yes)
                            {
                                //iGridDataSoureHPDK = UncheckGrid(Convert.ToInt32(iGridDataSoureHP.Rows[index]["ID_LOPHOCPHAN"].ToString()), iGridDataSoureHPDK.Copy());
                                //iGridDataSoureHPDK = zdt.Copy();
                                var data = (from temp in zdt.AsEnumerable().Where(t => t.Field<string>("CHK") == "True") select temp).ToArray();
                                if (data.Count() > 0)
                                {
                                    DataTable dt = data.CopyToDataTable();
                                    if (Convert.ToInt32(dt.Rows[0]["ID_DANGKY"].ToString()) == 0)
                                    {
                                        int xxx = Convert.ToInt32(dt.Rows[0]["ID_LOPHOCPHAN"].ToString());
                                        var tempdata =
                                            (from tempz in iGridDataSoureHPDK.AsEnumerable()
                                                    .Where(t => t.Field<int>("ID_LOPHOCPHAN") != xxx)
                                             select tempz).ToArray();
                                        if (tempdata.Count() > 0)
                                        {
                                            iGridDataSoureHPDK = tempdata.CopyToDataTable();
                                            grDanhSachDK.ItemsSource = iGridDataSoureHPDK;
                                        }
                                    }
                                    else
                                    {
                                        bool res = DangKyHocPhan.Insert_DangKyHuy(dt.Copy(), UserCommon.UserName);
                                        if (res)
                                        {
                                            //CTMessagebox.Show("Đã hủy thành công", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                                            DataTable xdt = (from temp in zdt.AsEnumerable().Where(t => t.Field<string>("CHK") != "True") select temp).CopyToDataTable();

                                            iGridDataSoureHPDK = xdt.Copy();
                                            grDanhSachDK.ItemsSource = iGridDataSoureHPDK;

                                            int pID_LOPHOCPHAN = Convert.ToInt32(iGridDataSoureHP.Rows[index]["ID_LOPHOCPHAN"].ToString());

                                            iGridDataSoureHP.Rows[index]["SOSVDKY"] = DangKyHocPhan.GetSSDK(pID_LOPHOCPHAN);

                                            for (int i = 0; i < lstIDLopHocPhanDK.Count; i++)
                                            {
                                                if (lstIDLopHocPhanDK[i] == pID_LOPHOCPHAN)
                                                    lstIDLopHocPhanDK.RemoveAt(i);
                                            }
                                        }
                                        else
                                        {
                                            CTMessagebox.Show("Lỗi", "Thông báo", "", CTICON.Error, CTBUTTON.OK);
                                        }
                                    }

                                }
                            }
                            else
                            {
                                iGridDataSoureHP.Rows[index]["CHK"] = "True";
                                grdLopHP.ItemsSource = iGridDataSoureHP;
                            }
                        }
                        #endregion
                    }
                }
                else
                {
                    #region Add row khi iGridDataSoureHPDK = null
                    if (iGridDataSoureHP.Rows[index]["CHK"].ToString() == "True")
                    {
                        int SoLuongTongSV = Convert.ToInt32(iGridDataSoureHP.Rows[index]["SOLUONG"].ToString());
                        int SoLuongDaDK = Convert.ToInt32(iGridDataSoureHP.Rows[index]["SOSVDKY"].ToString());
                        if (SoLuongTongSV > SoLuongDaDK)
                        {
                            iGridDataSoureHPDK = TableSchemaBindingGridHPDK();
                            DataRow r = iGridDataSoureHPDK.NewRow();
                            r["ID_DANGKY"] = 0;
                            r["MA_MONHOC"] = iGridDataSoureHP.Rows[index]["MA_MONHOC"];
                            r["TEN_MONHOC"] = iGridDataSoureHP.Rows[index]["TEN_MONHOC"];
                            r["ID_LOPHOCPHAN"] = iGridDataSoureHP.Rows[index]["ID_LOPHOCPHAN"];

                            r["MA_LOP_HOCPHAN"] = iGridDataSoureHP.Rows[index]["MA_LOP_HOCPHAN"];
                            r["TEN_LOP_HOCPHAN"] = iGridDataSoureHP.Rows[index]["TEN_LOP_HOCPHAN"];

                            r["ID_THAMSO"] = 0;
                            r["ID_SINHVIEN"] = this.iDataSoure.Rows[0]["ID_SINHVIEN"];
                            r["NGAY_DANGKY"] = System.DateTime.Now;
                            r["GIO_DANGKY"] = DateTime.Now.ToString("HH:mm:ss");
                            r["SO_TC"] = iGridDataSoureHP.Rows[index]["SO_TC"];
                            r["DON_GIA"] = DangKyHocPhan.GetHocPhi_LT(Convert.ToInt32(this.iDataSoure.Rows[0]["ID_HE_DAOTAO"].ToString()), Convert.ToInt32(iGridDataSoureHP.Rows[index]["IS_LYTHUYET"].ToString()));
                            r["THANH_TIEN"] = (double)Convert.ToDouble(r["SO_TC"].ToString()) * Convert.ToDouble(r["DON_GIA"].ToString());
                            r["TRANGTHAI"] = "Chưa lưu";
                            iGridDataSoureHPDK.Rows.Add(r);
                            iGridDataSoureHPDK.Columns.Add("CHK");
                            this.grDanhSachDK.ItemsSource = iGridDataSoureHPDK;

                            iGridDataSoureHP.Rows[index]["SOSVDKY"] = SoLuongDaDK + 1;
                        }
                        else
                        {
                            CTMessagebox.Show("Lớp đã đủ số lượng.!", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                            iGridDataSoureHP.Rows[index]["CHK"] = "";
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
            #endregion
        }

        private void CboKhoa_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                cboNganh.ItemsSource = null;
                cboMonHoc.ItemsSource = null;
                if (this.iDataSoure.Rows[0]["ID_KHOAHOC"].ToString() != string.Empty)
                {
                    idkhoahoc = Convert.ToInt32(this.iDataSoure.Rows[0]["ID_KHOAHOC"].ToString());
                    SetComBoNganh(idkhoahoc);
                    CboNganh_OnEditValueChanged(null, null);
                    LoadGridLopHocPhan();
                }
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void CboNganh_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                cboMonHoc.ItemsSource = null;
                if (this.iDataSoure.Rows[0]["ID_KHOAHOC"].ToString() != string.Empty &&
                    this.iDataSoure.Rows[0]["ID_NGANH"].ToString() != string.Empty)
                {
                    idkhoahoc = Convert.ToInt32(this.iDataSoure.Rows[0]["ID_KHOAHOC"].ToString());
                    idnganh = Convert.ToInt32(this.iDataSoure.Rows[0]["ID_NGANH"].ToString());
                }
                SetComboMonHoc(idkhoahoc, idnganh);
                LoadGridLopHocPhan();
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void CboMonHoc_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (this.iDataSoure.Rows[0]["ID_MONHOC"].ToString() != string.Empty)
                {
                    if (iGridDataSoureHP != null && iGridDataSoureHP.Rows.Count > 0)
                    {
                        idmonhoc = Convert.ToInt32(this.iDataSoure.Rows[0]["ID_MONHOC"].ToString());
                        if (iGridDataSoureHP.Rows.Count > 0)
                        {
                            if (idmonhoc != 0)
                            {
                                var temp =
                                    (from data in
                                        iGridDataSoureHP.AsEnumerable()
                                            .Where(t => t.Field<int>("ID_MONHOC") == idmonhoc)
                                        select data).ToList();
                                if (temp.Count > 0)
                                {
                                    DataTable dt = temp.CopyToDataTable();
                                    grdLopHP.ItemsSource = dt;
                                }
                            }
                            else
                            {
                                grdLopHP.ItemsSource = iGridDataSoureHP;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void CboLopHoc_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (this.iDataSoure.Rows[0]["ID_LOPHOC"].ToString() != string.Empty)
                {
                    idLopHoc = Convert.ToInt32(this.iDataSoure.Rows[0]["ID_LOPHOC"].ToString());
                }
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }
        
        private void BtnXuatPhieu_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                DataTable hocdk = null;
                DataRow[] check = (from temp in iGridDataSoureHPDK.AsEnumerable().Where(t => t.Field<string>("CHK") != "True") select temp).ToArray();
                if (check.Count() > 0)
                {
                    hocdk = check.CopyToDataTable();
                }
                DataTable dt = DangKyHocPhan.GetALLSinhVien();
                foreach (DataRow r in dt.Rows)
                {
                    bool res = DangKyHocPhan.Insert_HocPhanDK_All(hocdk.Copy(), r["MA_SINHVIEN"].ToString(), Convert.ToInt32(r["ID_SINHVIEN"].ToString()));
                    if (res)
                    {
                        CTMessagebox.Show("Thanh cong");
                    }
                    else
                    {
                        CTMessagebox.Show("Loi");
                    }
                }
                //XuatThoiKhoaBieu();
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        void XuatThoiKhoaBieu()
        {
            Excel.Application excels = new Microsoft.Office.Interop.Excel.Application();
            Excel.Workbooks workbook = null;
            Excel.Workbook oBook = null;
            Excel.Worksheet worksheet = null;
            Excel.Borders boders = null;
            System.Drawing.Color background = System.Drawing.Color.FromArgb(0, 176, 240);
            excels.Application.SheetsInNewWorkbook = 1;
            workbook = excels.Workbooks;
            oBook = (Excel.Workbook)(excels.Workbooks.Add(Type.Missing));
            worksheet = oBook.Worksheets[1];
            worksheet.Name = "ThoiKhoaBieu";
            worksheet.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
            Excel.Range headRange = worksheet.get_Range("B1", "D1");

            #region Title
            headRange.MergeCells = true;
            headRange.Resize.RowHeight = 25;
            headRange.ColumnWidth = 35;
            headRange.Font.Name = "Times New Roman";
            headRange.Value = "BỘ CÔNG THƯƠNG";
            headRange.Font.Size = 11;

            headRange = worksheet.get_Range("B2", "D2");
            headRange.MergeCells = true;
            headRange.Resize.RowHeight = 35;
            headRange.ColumnWidth = 43;
            headRange.Font.Name = "Times New Roman";
            headRange.Value = "TRƯỜNG ĐẠI HỌC CÔNG NGHIỆP THỰC PHẨM";
            headRange.Font.Size = 10;
            headRange.Font.Bold = true;

            headRange = worksheet.get_Range("E1", "K1");
            headRange.MergeCells = true;
            headRange.Resize.RowHeight = 25;
            headRange.Font.Name = "Times New Roman";
            headRange.Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
            headRange.Font.Size = 11;

            headRange = worksheet.get_Range("E2", "K2");
            headRange.MergeCells = true;
            headRange.Resize.RowHeight = 25;
            headRange.Font.Name = "Times New Roman";
            headRange.Value = "Độc lập - Tự do - Hạnh phúc";
            headRange.Font.Size = 11;
            headRange.Font.Bold = true;


            headRange = worksheet.get_Range("E4", "H4");
            headRange.MergeCells = true;
            headRange.Resize.RowHeight = 25;
            headRange.Font.Name = "Times New Roman";
            headRange.Value = "PHIẾU ĐĂNG KÝ MÔN HỌC";
            headRange.Font.Size = 11;
            headRange.Font.Bold = true;

            headRange = worksheet.get_Range("C5", "D5");
            headRange.MergeCells = true;
            headRange.Resize.RowHeight = 18;
            //headRange.ColumnWidth = 25;
            headRange.Font.Name = "Times New Roman";
            headRange.Value = "MSSV:  " + _MaSinhVien;
            headRange.Font.Size = 11;
            headRange.Font.Bold = true;

            headRange = worksheet.get_Range("E5", "F5");
            headRange.MergeCells = true;
            headRange.Resize.RowHeight = 18;
            //headRange.ColumnWidth = 25;
            headRange.Font.Name = "Times New Roman";
            headRange.Value = "Sinh viên:  " + _SinhVien;
            headRange.Font.Size = 11;
            headRange.Font.Bold = true;

            headRange = worksheet.get_Range("G5", "H5");
            headRange.MergeCells = true;
            headRange.Resize.RowHeight = 18;
            //headRange.ColumnWidth = 30;
            headRange.Font.Name = "Times New Roman";
            headRange.Value = "Lớp:   " + _Lop;
            headRange.Font.Size = 11;
            headRange.Font.Bold = true;

            headRange = worksheet.get_Range("I5", "J5");
            headRange.MergeCells = true;
            headRange.Resize.RowHeight = 18;
            headRange.ColumnWidth = 25;
            headRange.Font.Name = "Times New Roman";
            headRange.Value = "Hệ đào tạo:   " + _HeDaoTao;
            headRange.Font.Size = 11;
            headRange.Font.Bold = true;

            headRange = worksheet.get_Range("D6", "E6");
            headRange.MergeCells = true;
            headRange.Resize.RowHeight = 18;
            headRange.ColumnWidth = 25;
            headRange.Font.Name = "Times New Roman";
            headRange.Value = "Khóa học:   " + _KhoaHoc;
            headRange.Font.Size = 11;
            headRange.Font.Bold = true;

            headRange = worksheet.get_Range("G6", "I6");
            headRange.MergeCells = true;
            headRange.Resize.RowHeight = 18;
            headRange.ColumnWidth = 25;
            headRange.Font.Name = "Times New Roman";
            headRange.Value = "Ngành học:   " + _Nganh;
            headRange.Font.Size = 11;
            headRange.Font.Bold = true;
            #endregion

            #region Header

            worksheet.Range["B8:K9"].Interior.Color = background;
            headRange = worksheet.get_Range("B8", "K9");
            headRange.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            headRange.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
            headRange.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
            headRange.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            headRange.Borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.XlLineStyle.xlContinuous;
            headRange.Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlContinuous;

            headRange = worksheet.get_Range("B8", "B9");
            headRange.MergeCells = true;
            headRange.ColumnWidth = 3;
            headRange.Value = "STT";
            headRange.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            headRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            headRange = worksheet.get_Range("C8", "C9");
            headRange.MergeCells = true;
            headRange.ColumnWidth = 15;
            headRange.Value = "Mã môn học";
            headRange.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            headRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            headRange = worksheet.get_Range("D8", "D9");
            headRange.MergeCells = true;
            headRange.ColumnWidth = 20;
            headRange.Value = "Tên môn học";
            headRange.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            headRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            headRange = worksheet.get_Range("E8", "E9");
            headRange.MergeCells = true;
            headRange.ColumnWidth = 15;
            headRange.Value = "Mã học phần";
            headRange.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            headRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            headRange = worksheet.get_Range("F8", "F9");
            headRange.MergeCells = true;
            headRange.ColumnWidth = 20;
            headRange.Value = "Tên học phần";
            headRange.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            headRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            headRange = worksheet.get_Range("G8", "G9");
            headRange.MergeCells = true;
            headRange.ColumnWidth = 15;
            headRange.Value = "STC";
            headRange.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            headRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            headRange = worksheet.get_Range("H8", "H9");
            headRange.MergeCells = true;
            headRange.ColumnWidth = 25;
            headRange.Value = "Học phí/TC";
            headRange.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            headRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            headRange = worksheet.get_Range("I8", "I9");
            headRange.MergeCells = true;
            headRange.ColumnWidth = 20;
            headRange.Value = "Học phí";
            headRange.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            headRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            headRange = worksheet.get_Range("J8", "J9");
            headRange.MergeCells = true;
            headRange.ColumnWidth = 20;
            headRange.Value = "Ngày đăng ký";
            headRange.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            headRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            headRange = worksheet.get_Range("K8", "K9");
            headRange.MergeCells = true;
            headRange.ColumnWidth = 15;
            headRange.Value = "Ghi chú";
            headRange.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            headRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            #endregion

            #region design table

            headRange = worksheet.get_Range("A1", "Z6");
            headRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            headRange.VerticalAlignment = Excel.XlHAlign.xlHAlignCenter;

            headRange = worksheet.get_Range("A5", "Z6");
            headRange.WrapText = true;
            //headRange.Font.Size = 10;
            headRange.Font.Bold = true;

            headRange = worksheet.get_Range("K6", "X6");
            headRange.Font.Bold = false;

            #endregion

            #region
            if (iGridDataSoureHPDK != null && iGridDataSoureHPDK.Rows.Count > 0)
            {
                int row = 0;
                int col = 0;
                for (int i = 0; i < iGridDataSoureHPDK.Rows.Count; i++)
                {
                    worksheet.Cells[(i + 10), 2] = i + 1;
                    row = i + 10;
                    for (int j = 0; j < iGridDataSoureHPDK.Columns.Count - 7; j++)
                    {
                        worksheet.Cells[(i + 10), (j + 3)] = iGridDataSoureHPDK.Rows[i][j];
                        if ((i + 10) < (iGridDataSoureHPDK.Rows.Count + 9))
                        {
                            headRange = worksheet.get_Range("B" + row, "K" + row);
                            headRange.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                            headRange.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                            headRange.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                            headRange.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                        }
                    }
                }
                if (row != 0)
                {
                    headRange = worksheet.get_Range("B10", "K" + row);
                    headRange.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                    headRange.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                    headRange.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                }
            }
            #endregion
            excels.Application.Visible = true;
        }

     }
}
