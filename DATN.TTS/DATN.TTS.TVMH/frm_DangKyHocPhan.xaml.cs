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
using DATN.TTS.BUS.Resource;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;

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
        bus_LapKeHoachDaoTaoKhoa client = new bus_LapKeHoachDaoTaoKhoa();
        bus_DangKyHocPhan DangKyHocPhan = new bus_DangKyHocPhan();
        public frm_DangKyHocPhan()
        {
            InitializeComponent();
            this.iDataSoure = TableSchemaBinding();
            this.DataContext = iDataSoure;

            this.iDataSoure.Rows[0]["USER"] = UserCommon.UserName;
            //this.iDataSoure.Rows[0]["ID_HE_DAOTAO"] = "1";
            SetComBoThu();
            InitGrid_LopHP();
            InitGrid_HPDangKy();
            SetComboHDT();
            SetComboMonHoc();
        }

        private DataTable GetNgayHoc()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("THU", typeof(Decimal));
            dt.Columns.Add("NGAY", typeof(string));
            DataRow dr = dt.NewRow();
            dr["THU"] = 0;
            dr["NGAY"] = "----------------Chọn-----------------";
            dt.Rows.Add(dr);
            for (int i = 2; i <= 8; i++)
            {
                DataRow r = dt.NewRow();
                r["THU"] = i;
                if (i == 8)
                {
                    r["NGAY"] = "Chủ nhật";
                }
                else
                {
                    r["NGAY"] = "Thứ" + " " + i;
                }
                dt.Rows.Add(r);
                dt.AcceptChanges();
            }
            return dt;
        }

        private void SetComBoThu()
        {
            cboThu.ItemsSource = GetNgayHoc();
            this.iDataSoure.Rows[0]["THU"] = cboThu.GetKeyValue(0);
        }

        private void SetComboMonHoc()
        {
            try
            {
                DataTable dt = DangKyHocPhan.GetMonHoc();
                cboMonHoc.ItemsSource = dt;
                if (dt.Rows.Count > 0)
                    this.iDataSoure.Rows[0]["ID_MONHOC"] = cboMonHoc.GetKeyValue(0);
            }
            catch (Exception err)
            {
               throw err;
            }
        }

        private DataTable TableSchemaBinding()
        {
            try
            {
                DataTable dt = null;
                Dictionary<string, Type>dic = new Dictionary<string, Type>();
                dic.Add("THU", typeof(Decimal));
                dic.Add("NGAY", typeof(string));
                dic.Add("USER", typeof(string));
                dic.Add("ID_HE_DAOTAO",typeof(int));
                dic.Add("ID_KHOAHOC", typeof(int));
                dic.Add("ID_NGANH", typeof(int));
                dic.Add("ID_LOPHOC", typeof(int));
                dic.Add("ID_MONHOC", typeof(Decimal));
                dt = TableUtil.ConvertToTable(dic);
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            } 
        }

        void InitGrid_LopHP()
        {
            try
            {
                GridColumn col;

                col = new GridColumn();
                col.FieldName = "CHK";
                col.Header = "DK";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new CheckEditSettings();
                col.UnboundType = UnboundColumnType.Boolean;
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdLopHP.Columns.Add(col);

                #region Hide
                col = new GridColumn();
                col.FieldName = "ID_LOPHOCPHAN";
                col.Header = string.Empty;
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grdLopHP.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "ID_KHOAHOC_NGANH_CTIET";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grdLopHP.Columns.Add(col);


                col = new GridColumn();
                col.FieldName = "ID_NAMHOC_HKY_HTAI";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grdLopHP.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "ID_HEDAOTAO";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grdLopHP.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "ID_MONHOC";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grdLopHP.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "ID_LOPHOC";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grdLopHP.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "ID_GIANGVIEN";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grdLopHP.Columns.Add(col);

                #endregion

                #region Hide HP
                col = new GridColumn();
                col.FieldName = "MA_LOP_HOCPHAN";
                col.Header = "Mã học phần";
                col.Width = 100;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdLopHP.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TEN_LOP_HOCPHAN";
                col.Header = "Tên học phần";
                col.Width = 200;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grdLopHP.Columns.Add(col);

                #endregion

                col = new GridColumn();
                col.FieldName = "MA_MONHOC";
                col.Header = "Mã môn học";
                col.Width = 150;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdLopHP.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TEN_MONHOC";
                col.Header = "Tên môn học";
                col.Width = 250;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grdLopHP.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "SO_TC";
                col.Header = "Số TC";
                col.Width = 60;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdLopHP.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "SOLUONG";
                col.Header = "Sỉ số";
                col.Width = 60;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdLopHP.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "CONLAI";
                col.Header = "Còn lại";
                col.Width = 60;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdLopHP.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "THU";
                col.Header = "Thứ";
                col.Width = 60;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdLopHP.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TIET_BD";
                col.Header = "Tiết BD";
                col.Width = 70;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdLopHP.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "SO_TIET";
                col.Header = "Số tiết";
                col.Width = 70;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdLopHP.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TEN_PHONG";
                col.Header = "Phòng";
                col.Width = 100;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdLopHP.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TUAN_BD";
                col.Header = "Tuần BD";
                col.Width = 70;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdLopHP.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TUAN_KT";
                col.Header = "Tuần KT";
                col.Width = 70;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdLopHP.Columns.Add(col);
            }
            catch (Exception er)
            {
                throw er;
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
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grDanhSachDK.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "ID_SINHVIEN";
                col.Header = string.Empty;
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grDanhSachDK.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "MA_MONHOC";
                col.Header = "Mã môn học";
                col.Width = 150;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
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
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grDanhSachDK.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "SO_TC";
                col.Header = "Số TC";
                col.Width = 80;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grDanhSachDK.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "DON_GIA";
                col.Header = "Học phí";
                col.Width = 130;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grDanhSachDK.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "THANH_TIEN";
                col.Header = "Phải đóng";
                col.Width = 150;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grDanhSachDK.Columns.Add(col);


                col = new GridColumn();
                col.FieldName = "NGAY_DANGKY";
                col.Header = "Ngày DK";
                col.Width = 100;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grDanhSachDK.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TRANGTHAI";
                col.Header = "Trạng thái chọn môn học";
                col.Width = 170;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
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
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new CheckEditSettings();
                col.UnboundType = UnboundColumnType.Boolean;
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grDanhSachDK.Columns.Add(col);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        void SetComboHDT()
        {
            try
            {
                DataTable dt = client.GetAllHDT();
                cboHDT.ItemsSource = dt;
                if (dt.Rows.Count > 0)
                    this.iDataSoure.Rows[0]["ID_HE_DAOTAO"] = cboHDT.GetKeyValue(0);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        private void CboHDT_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (this.iDataSoure.Rows[0]["ID_HE_DAOTAO"].ToString() != string.Empty)
                {
                    int pid = Convert.ToInt32(this.iDataSoure.Rows[0]["ID_HE_DAOTAO"].ToString());
                    DataTable dt = client.GetAllKhoaHoc(pid);
                    CboKhoa.ItemsSource = dt;
                    if (dt.Rows.Count > 0)
                        this.iDataSoure.Rows[0]["ID_KHOAHOC"] = CboKhoa.GetKeyValue(0);
                }
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

        private void CboKhoa_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (this.iDataSoure.Rows[0]["ID_KHOAHOC"].ToString() != string.Empty)
                {
                    int pid = Convert.ToInt32(this.iDataSoure.Rows[0]["ID_KHOAHOC"].ToString());
                    DataTable dt = client.GetAllKhoaNganh(pid);
                    cboNganh.ItemsSource = dt;
                    if (dt.Rows.Count > 0)
                        this.iDataSoure.Rows[0]["ID_NGANH"] = cboNganh.GetKeyValue(0);
                }
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

        private void CboNganh_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (this.iDataSoure.Rows[0]["ID_KHOAHOC"].ToString() != string.Empty && this.iDataSoure.Rows[0]["ID_NGANH"].ToString() !=string.Empty)
                {
                    int pidkhoa = Convert.ToInt32(this.iDataSoure.Rows[0]["ID_KHOAHOC"].ToString());
                    int pidnganh = Convert.ToInt32(this.iDataSoure.Rows[0]["ID_NGANH"].ToString());
                    DataTable dt = DangKyHocPhan.GetLopHoc(pidkhoa, pidnganh);
                    cboLopHoc.ItemsSource = dt;
                    if (dt.Rows.Count > 0)
                        this.iDataSoure.Rows[0]["ID_LOPHOC"] = cboLopHoc.GetKeyValue(0);
                }
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

        private void CboMonHoc_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            
        }

        private void BtnHoTroID3_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                frm_ID3 frm=new frm_ID3();
                frm.Owner = Window.GetWindow(this);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BtnHoTroC45_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                frm_C45 frm = new frm_C45();
                frm.Owner = Window.GetWindow(this);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
