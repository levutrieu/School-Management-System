using System;
using System.Collections.Generic;
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
using System.Data;
using DATN.TTS.BUS;
using DATN.TTS.BUS.Resource;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using CustomMessage;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_KhoaHoc.xaml
    /// </summary>
    public partial class frm_KhoaHoc : Page
    {
        private DataTable iDataSoure = null;
        private DataTable iGridDataSoure = null;
        bus_KhoaHoc client = new bus_KhoaHoc();
        private bool FlagSave = true;

        public frm_KhoaHoc()
        {
            InitializeComponent();
            this.iDataSoure = TableChelmabinding();
            this.DataContext = iDataSoure;
            this.iDataSoure.Rows[0]["USER"] = UserCommon.UserName;
            InitGrid();
            SetCombox();
        }

        private void SetCombox()
        {
            cbbHDT.ItemsSource = client.GetAllHeDaoTao();
        }

        private DataTable TableChelmabinding()
        {
            try
            {
                DataTable dt = null;
                Dictionary<string, Type> dic = new Dictionary<string, Type>();
                dic.Add("ID_KHOAHOC", typeof(Int32));
                dic.Add("ID_HE_DAOTAO", typeof(Int32));
                dic.Add("MA_KHOAHOC",typeof(string));
                dic.Add("TEN_KHOAHOC", typeof(string));
                dic.Add("NAM_BD", typeof(Decimal));
                dic.Add("NAM_KT", typeof(Decimal));
                dic.Add("SO_HKY_1NAM", typeof(Decimal));
                dic.Add("SO_HKY", typeof(Decimal));
                dic.Add("KYHIEU", typeof(string));
                dic.Add("TRANGTHAI", typeof(string));
                dic.Add("USER",typeof(string));
                dt = TableUtil.ConvertToTable(dic);
                return dt;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void GetGrid()
        {
            iGridDataSoure = client.GetAllKhoaHoc();

            grd.ItemsSource = iGridDataSoure;
        }

        private void InitGrid()
        {
            GridColumn col;

            col = new GridColumn();
            col.FieldName = "ID_KHOAHOC";
            col.Header = "UserName";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "ID_HE_DAOTAO";
            col.Header = "UserName";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "TEN_HE_DAOTAO";
            col.Header = "Hệ đào tạo";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "MA_KHOAHOC";
            col.Header = "Mã khóa học";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.EditSettings = new TextEditSettings();
            col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
            grd.Columns.Add(col);


            col = new GridColumn();
            col.FieldName = "KYHIEU";
            col.Header = "Ký hiệu";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.EditSettings = new TextEditSettings();
            col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "TEN_KHOAHOC";
            col.Header = "Khóa học";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "NAM_BD";
            col.Header = "Năm bắt đầu";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.EditSettings = new TextEditSettings();
            col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "NAM_KT";
            col.Header = "Năm kết thúc";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.EditSettings = new TextEditSettings();
            col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "SO_HKY_1NAM";
            col.Header = "Học kỳ/Năm";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.EditSettings = new TextEditSettings();
            col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "SO_HKY";
            col.Header = "Số học kỳ";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.EditSettings = new TextEditSettings();
            col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "TRANGTHAI";
            col.Header = "Trạng thái";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            grd.Columns.Add(col);

            GetGrid();
        }

        private bool ValiDate()
        {
            if (this.iDataSoure.Rows[0]["ID_HE_DAOTAO"].ToString() == string.Empty)
            {
                CTMessagebox.Show("Vui lòng chọn hệ đào tạo!", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                cbbHDT.Focus();
                return false;
            }
            if (this.iDataSoure.Rows[0]["MA_KHOAHOC"].ToString() == string.Empty)
            {
                CTMessagebox.Show("Vui lòng nhập mã khóa học", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                txtMaKH.Focus();
                return false;
            }
            if (this.iDataSoure.Rows[0]["TEN_KHOAHOC"].ToString() == string.Empty)
            {
                CTMessagebox.Show("Vui lòng nhập tên khóa học", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                txtTenKH.Focus();
                return false;
            }
            if (this.iDataSoure.Rows[0]["NAM_BD"].ToString() == string.Empty)
            {
                CTMessagebox.Show("VUi lòng nhập năm bắt đầu", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                txtNamBD.Focus();
                return false;
            }
            if (this.iDataSoure.Rows[0]["NAM_KT"].ToString() == string.Empty)
            {
                CTMessagebox.Show("Vui lòng nhập năm kết thúc", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                txtNamKT.Focus();;
                return false;
            }
            if (this.iDataSoure.Rows[0]["SO_HKY_1NAM"].ToString() == string.Empty)
            {
                CTMessagebox.Show("Vui lòng nhập số học kỳ của năm", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                txtHocKyNam.Focus();
                return false;
            }
            if (this.iDataSoure.Rows[0]["SO_HKY"].ToString() == string.Empty)
            {
                CTMessagebox.Show("Vui lòng nhập tổng số học kỳ", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                txtSoHK.Focus();
                return false;
            }
            if (this.iDataSoure.Rows[0]["KYHIEU"].ToString() == string.Empty)
            {
                CTMessagebox.Show("Bạn có muốn xóa không?", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                txtKyHieu.Focus();
                return false;
            }
            if (this.iDataSoure.Rows[0]["TRANGTHAI"].ToString() == string.Empty)
            {
                CTMessagebox.Show("Bạn có muốn xóa không?", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                txtTrangThai.Focus();
                return false;
            }
            return true;
        }

        private void SetNullValue()
        {
            try
            {
                this.iDataSoure.Rows[0]["ID_HE_DAOTAO"] = "0";
                this.iDataSoure.Rows[0]["MA_KHOAHOC"] = string.Empty;
                this.iDataSoure.Rows[0]["TEN_KHOAHOC"] = string.Empty;
                this.iDataSoure.Rows[0]["NAM_BD"] = 0;
                this.iDataSoure.Rows[0]["NAM_KT"] = 0;
                this.iDataSoure.Rows[0]["SO_HKY_1NAM"] = 0;
                this.iDataSoure.Rows[0]["SO_HKY"] = 0;
                this.iDataSoure.Rows[0]["KYHIEU"] = string.Empty;
                this.iDataSoure.Rows[0]["TRANGTHAI"] = string.Empty;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void btnAddNew_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                SetNullValue();
                txtMaKH.Focus();
                FlagSave = true;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void btnSave_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                //this.iDataSoure.Rows[0]["ID_HE_DAOTAO"] = "1";
                if (ValiDate())
                {
                    if (FlagSave)
                    {
                        bool res = client.Insert_KhoaHoc(this.iDataSoure.Copy());
                        if (!res)
                        {
                            CTMessagebox.Show("Thêm mới không thành công", "Thêm mới", "", CTICON.Information,
                                CTBUTTON.YesNo);
                        }
                        SetNullValue();
                    }
                    else
                    {
                        client.Update_KhoaHoc(this.iDataSoure.Copy());
                    }
                }
                GetGrid();
            }
            catch (Exception)
            {
                
                throw;
            }

        }

        private void btnDelete_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CTMessagebox.Show("Bạn có muốn xóa không?","Xóa","", CTICON.Information, CTBUTTON.YesNo) == CTRESPONSE.Yes)
                {
                    client.Delete_KhoaHoc(int.Parse(this.iDataSoure.Rows[0]["ID_KHOAHOC"].ToString()), this.iDataSoure.Rows[0]["USER"].ToString());

                    GetGrid();
                    SetNullValue();
                }
            }
            catch (Exception err)
            {
                
                throw(err);
            }
        }

        private void btnRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                SetNullValue();
                GetGrid();
                txtMaKH.Focus();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void GrdViewNDung_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void GrdViewNDung_OnFocusedRowChanged(object sender, DevExpress.Xpf.Grid.FocusedRowChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                DataRow row = null;
                if (this.grd.GetFocusedRow() == null)
                    return;
                row = ((DataRowView)this.grd.GetFocusedRow()).Row;
                this.iDataSoure.Rows[0]["ID_KHOAHOC"] = row["ID_KHOAHOC"];
                this.iDataSoure.Rows[0]["ID_HE_DAOTAO"] = row["ID_HE_DAOTAO"];
                this.iDataSoure.Rows[0]["MA_KHOAHOC"] = row["MA_KHOAHOC"];
                this.iDataSoure.Rows[0]["TEN_KHOAHOC"] = row["TEN_KHOAHOC"];
                this.iDataSoure.Rows[0]["NAM_BD"] = row["NAM_BD"];
                this.iDataSoure.Rows[0]["NAM_KT"] = row["NAM_KT"];
                this.iDataSoure.Rows[0]["SO_HKY_1NAM"] = row["SO_HKY_1NAM"];
                this.iDataSoure.Rows[0]["SO_HKY"] = row["SO_HKY"];
                this.iDataSoure.Rows[0]["KYHIEU"] = row["KYHIEU"];
                this.iDataSoure.Rows[0]["TRANGTHAI"] = row["TRANGTHAI"];

                FlagSave = false;
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
