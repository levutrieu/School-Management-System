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
using DevExpress.Utils;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_NhanVien.xaml
    /// </summary>
    public partial class frm_NhanVien : Window
    {
        bus_NhanSu client = new bus_NhanSu();
        private DataTable iDataSoure = null;
        private bool flgsave = true;

        public frm_NhanVien()
        {
            InitializeComponent();
            iDataSoure = TableSchemaBinding();
            this.DataContext = iDataSoure;
            InitGrid();
        }

        #region

        private DataTable TableSchemaBinding()
        {
            DataTable dt = null;
            try
            {
                Dictionary<string, Type> xDicUser = new Dictionary<string, Type>();
                xDicUser.Add("ID_NHANVIEN", typeof(int));
                xDicUser.Add("MA_NHANVIEN", typeof(string));
                xDicUser.Add("HOTEN", typeof(string));
                xDicUser.Add("GIOI_TINH", typeof(Decimal));
                xDicUser.Add("NGAY_SINH", typeof(DateTime));
                xDicUser.Add("DIACHI", typeof(string));
                xDicUser.Add("CMND", typeof(string));
                xDicUser.Add("NGAYCAP", typeof(DateTime));
                xDicUser.Add("NOICAP", typeof(string));
                xDicUser.Add("DIENTHOAI", typeof(string));
                xDicUser.Add("EMAIL", typeof(string));
                xDicUser.Add("TRANGTHAI", typeof(string));
                xDicUser.Add("NAM_LAMVIEC", typeof(Decimal));
                xDicUser.Add("ID_HE_SOLUONG", typeof(int));
                xDicUser.Add("CHOOHIENTAI", typeof(string));
                xDicUser.Add("LUONGCB", typeof(Decimal));
                xDicUser.Add("USER", typeof(string));

                dt = TableUtil.ConvertToTable(xDicUser);
                return dt;

            }
            catch (Exception)
            {
                
                throw;
            }
            
        }

        private void ValiCheck()
        {
            if (chkGioiTinh.IsChecked == true)
            {
                this.iDataSoure.Rows[0]["GIOI_TINH"] = 1;
            }
            else
            {
                this.iDataSoure.Rows[0]["GIOI_TINH"] = 0;
            }
        }

        private bool ValiDate()
        {
            try
            {
                if (this.iDataSoure.Rows[0]["MA_NHANVIEN"].ToString() == String.Empty)
                {
                    MessageBox.Show("Vui lòng nhập", "Thông báo");
                    txtMaNV.Focus();
                    return false;
                    
                }
                if (this.iDataSoure.Rows[0]["HOTEN"].ToString() == String.Empty)
                {
                    MessageBox.Show("Vui lòng nhập", "Thông báo");
                    txtHoTen.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["GIOI_TINH"].ToString() == String.Empty)
                {
                    MessageBox.Show("Vui lòng nhập", "Thông báo");
                    chkGioiTinh.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["NGAY_SINH"].ToString() == String.Empty)
                {
                    MessageBox.Show("Vui lòng nhập", "Thông báo");
                    txtNgaySinh.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["DIACHI"].ToString() == String.Empty)
                {
                    MessageBox.Show("Vui lòng nhập", "Thông báo");
                    txtDiaChi.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["CMND"].ToString() == String.Empty)
                {
                    MessageBox.Show("Vui lòng nhập", "Thông báo");
                    txtCMND.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["NGAYCAP"].ToString() == String.Empty)
                {
                    MessageBox.Show("Vui lòng nhập", "Thông báo");
                    txtNgayCap.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["NOICAP"].ToString() == String.Empty)
                {
                    MessageBox.Show("Vui lòng nhập", "Thông báo");
                    txtNoiCap.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["DIENTHOAI"].ToString() == String.Empty)
                {
                    MessageBox.Show("Vui lòng nhập", "Thông báo");
                    txtDT.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["EMAIL"].ToString() == String.Empty)
                {
                    MessageBox.Show("Vui lòng nhập", "Thông báo");
                    txtEmail.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["TRANGTHAI"].ToString() == String.Empty)
                {
                    MessageBox.Show("Vui lòng nhập", "Thông báo");
                    txtTrangThai.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["NAM_LAMVIEC"].ToString() == String.Empty)
                {
                    MessageBox.Show("Vui lòng nhập", "Thông báo");
                    txtNamLV.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["ID_HE_SOLUONG"].ToString() == String.Empty)
                {
                    MessageBox.Show("Vui lòng nhập", "Thông báo");
                    txtHSl.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["CHOOHIENTAI"].ToString() == String.Empty)
                {
                    MessageBox.Show("Vui lòng nhập", "Thông báo");
                    txtChoHT.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["LUONGCB"].ToString() == String.Empty)
                {
                    MessageBox.Show("Vui lòng nhập", "Thông báo");
                    txtLCB.Focus();
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SetNullValue()
        {
            try
            {
                this.iDataSoure.Rows[0]["MA_NHANVIEN"] = String.Empty;
                this.iDataSoure.Rows[0]["HOTEN"] = String.Empty;
                this.iDataSoure.Rows[0]["GIOI_TINH"] = 0;
                this.iDataSoure.Rows[0]["NGAY_SINH"] = DateTime.Today;
                this.iDataSoure.Rows[0]["DIACHI"] = String.Empty;
                this.iDataSoure.Rows[0]["CMND"] = String.Empty;
                this.iDataSoure.Rows[0]["NGAYCAP"] = DateTime.Today;
                this.iDataSoure.Rows[0]["NOICAP"] = String.Empty;
                this.iDataSoure.Rows[0]["DIENTHOAI"] = String.Empty;
                this.iDataSoure.Rows[0]["EMAIL"] = String.Empty;
                this.iDataSoure.Rows[0]["TRANGTHAI"] = String.Empty;
                this.iDataSoure.Rows[0]["NAM_LAMVIEC"] = 0;
                this.iDataSoure.Rows[0]["ID_HE_SOLUONG"] = 0;
                this.iDataSoure.Rows[0]["CHOOHIENTAI"] = 0;
                this.iDataSoure.Rows[0]["LUONGCB"] = 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void LoadAll()
        {
            try
            {
                grd.ItemsSource = client.GetAllNhanSu();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void InitGrid()
        {
            try
            {
                GridColumn col = null;

                col = new GridColumn();
                col.FieldName = "ID_NHANVIEN";
                col.Header = "ID";
                col.Visible = false;
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "MA_NHANVIEN";
                col.Header = "Mã nhân viên";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "HOTEN";
                col.Header = "Họ tên";
                col.AutoFilterValue = true;
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "GIOI_TINH";
                col.Header = "Giới tính";
                col.EditSettings = new CheckEditSettings();
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "NGAY_SINH";
                col.Header = "Ngày sinh";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.DisplayFormat = "dd/MM/yyyy";
                col.Visible = true;
                
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "DIACHI";
                col.Header = "Địa chỉ";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "CMND";
                col.Header = "CMND";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "DIENTHOAI";
                col.Header = "Điện thoại";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "EMAIL";
                col.Header = "Email";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TRANGTHAI";
                col.Header = "Trạng thái";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "NAM_LAMVIEC";
                col.Header = "Năm làm việc";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "LUONGCB";
                col.Header = "Lương cơ bản";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "ID_HE_SOLUONG";
                col.Header = "Lương cơ bản";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "NGAYCAP";
                col.Header = "Lương cơ bản";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "NOICAP";
                col.Header = "Lương cơ bản";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                
                grd.Columns.Add(col);

                LoadAll();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        private void btnAddNew_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                SetNullValue();
                flgsave = true;
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
                ValiCheck();
                if (ValiDate())
                {
                    if (flgsave)
                    {
                        bool res = client.Insert_NhanSu(iDataSoure.Copy());
                        if (res)
                        {
                            MessageBox.Show("Thêm mới thành công");
                            SetNullValue();
                        }
                        else
                        {
                            MessageBox.Show("Thêm mới không thành công");
                        }
                    }
                    else
                    {
                        client.Update_NhanSu(iDataSoure.Copy());
                        SetNullValue();
                    }
                   
                }
                
                LoadAll();
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
                if (MessageBox.Show("Bạn có muốn xóa không", "Thông báo", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    client.Delete_NhanSu(iDataSoure.Copy());
                }
                LoadAll();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void btnRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadAll();
                SetNullValue();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void Grd_OnFocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                DataRow row = null;
                if (this.grd.GetFocusedRow() == null)
                    return;
                row = ((DataRowView)this.grd.GetFocusedRow()).Row;
                this.iDataSoure.Rows[0]["ID_NHANVIEN"] = row["ID_NHANVIEN"];
                this.iDataSoure.Rows[0]["MA_NHANVIEN"] = row["MA_NHANVIEN"];
                this.iDataSoure.Rows[0]["HOTEN"] = row["HOTEN"];
                this.iDataSoure.Rows[0]["GIOI_TINH"] = row["GIOI_TINH"];
                this.iDataSoure.Rows[0]["NGAY_SINH"] = row["NGAY_SINH"];
                this.iDataSoure.Rows[0]["DIACHI"] = row["DIACHI"];
                this.iDataSoure.Rows[0]["CMND"] = row["CMND"];
                this.iDataSoure.Rows[0]["NGAYCAP"] = row["NGAYCAP"];
                this.iDataSoure.Rows[0]["NOICAP"] = row["NOICAP"];
                this.iDataSoure.Rows[0]["DIENTHOAI"] = row["DIENTHOAI"];
                this.iDataSoure.Rows[0]["EMAIL"] = row["EMAIL"];
                this.iDataSoure.Rows[0]["TRANGTHAI"] = row["TRANGTHAI"];
                this.iDataSoure.Rows[0]["NAM_LAMVIEC"] = row["NAM_LAMVIEC"];
                this.iDataSoure.Rows[0]["ID_HE_SOLUONG"] = row["ID_HE_SOLUONG"];
                this.iDataSoure.Rows[0]["CHOOHIENTAI"] = row["CHOOHIENTAI"];
                this.iDataSoure.Rows[0]["LUONGCB"] = row["LUONGCB"];
                flgsave = false;
            }
            catch (Exception ex)
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
