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
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_SinhVien.xaml
    /// </summary>
    public partial class frm_SinhVien : Page
    {
        bus_SinhVien client = new bus_SinhVien();
        private DataTable iDataSoure = null;
        private DataTable iGridDataSoure = null;
        private bool flagsave = true;
        public frm_SinhVien()
        {
            InitializeComponent();
            this.iDataSoure = TableSchemaBinding();
            DataContext = this.iDataSoure;
            this.iDataSoure.Rows[0]["USER"] = "admin";
            InitGrid();
            SetCombo();
            GetComboTrangThai();
        }

        void GetComboTrangThai()
        {
            try
            {
                List<string> lst = new List<string>(){"Đang học", "Bảo lưu", "Đã tốt nghiệp", "Ngừng học", "Chuyển hệ", "Đình chỉ học", "Khác"};
                DataTable dt = new DataTable();
                dt.Columns.Add("TRANGTHAI", typeof (string));
                dt.Columns.Add("NAME_TRANGTHAI", typeof(string));

                for (int i = 0; i <= lst.Count-1; i++)
                {
                    DataRow r = dt.NewRow();
                    r["TRANGTHAI"] = lst[i].ToString();
                    r["NAME_TRANGTHAI"] = lst[i].ToString();

                    dt.Rows.Add(r);
                    dt.AcceptChanges();
                }
                cbbTrangThai.ItemsSource = dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        void SetCombo()
        {
            cbbLop.ItemsSource = client.GetAllLop();
        }

        void SetIsNull()
        {
            this.iDataSoure.Rows[0]["ID_SINHVIEN"] = "0";
            this.iDataSoure.Rows[0]["ID_LOPHOC"] = "0";
            this.iDataSoure.Rows[0]["TRANGTHAI"] = string.Empty;
            this.iDataSoure.Rows[0]["MA_SINHVIEN"] = string.Empty;
            this.iDataSoure.Rows[0]["TEN_SINHVIEN"] = string.Empty;
            this.iDataSoure.Rows[0]["PATH_ANH"] = string.Empty;
            this.iDataSoure.Rows[0]["GIOITINH"] = "False";
            this.iDataSoure.Rows[0]["DIENTHOAI"] = string.Empty;
            this.iDataSoure.Rows[0]["DIENTHOAI_GD"] = string.Empty;
            this.iDataSoure.Rows[0]["DIACHI"] = string.Empty;
            this.iDataSoure.Rows[0]["EMAIL"] = string.Empty;
            this.iDataSoure.Rows[0]["CMND"] = string.Empty;
            this.iDataSoure.Rows[0]["NGAYCAP"] = DateTime.Parse("01/01/1990");
            this.iDataSoure.Rows[0]["NOICAP"] = string.Empty;
            this.iDataSoure.Rows[0]["NGAYSINH"] = DateTime.Parse("01/01/1990");
            this.iDataSoure.Rows[0]["NOISINH"] = string.Empty;
            this.iDataSoure.Rows[0]["THONGTIN_NGOAITRU"] = string.Empty;
            this.iDataSoure.Rows[0]["IS_DOANVIEN"] = "False";
            this.iDataSoure.Rows[0]["NGAY_VAODOAN"] = DateTime.Parse("01/01/2000");
            this.iDataSoure.Rows[0]["NGAY_VAOTRUONG"] = DateTime.Parse("01/01/2000");
            this.iDataSoure.Rows[0]["NGAY_RATRUONG"] = DateTime.Parse("01/01/2000");
            this.iDataSoure.Rows[0]["TEN_LOP"] = string.Empty;
        }

        void InitGrid()
        {
            GridColumn col = null;
            col = new GridColumn();
            col.FieldName = "ID_SINHVIEN";
            col.Header = string.Empty;
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "ID_LOPHOC";
            col.Header = string.Empty;
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "TEN_LOP";
            col.Header = "Lớp";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "PATH_ANH";
            col.Header = "Ảnh";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "MA_SINHVIEN";
            col.Header = "Mã SV";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "TEN_SINHVIEN";
            col.Header = "Sinh viên";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "TRANGTHAI";
            col.Header = "Trạng thái SV";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "GIOITINH";
            col.Header = "Giới tính";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "DIENTHOAI";
            col.Header = "Điện thoại";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "DIACHI";
            col.Header = "Địa chỉ";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "EMAIL";
            col.Header = "Email";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "CMND";
            col.Header = "CMND";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "NGAYCAP";
            col.Header = "Ngày cấp";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "NOICAP";
            col.Header = "Nơi cấp";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "NGAYSINH";
            col.Header = "Ngày sinh";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "NOISINH";
            col.Header = "Nơi sinh";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "THONGTIN_NGOAITRU";
            col.Header = "Ngoại trú";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "IS_DOANVIEN";
            col.Header = "Đoàn viên";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "NGAY_VAODOAN";
            col.Header = "Ngày vào";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "NGAY_VAOTRUONG";
            col.Header = "Ngày nhập học";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "NGAY_RATRUONG";
            col.Header = "Ngày ra trường";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            //grdViewNDung.AutoWidth = true;
            GetGrid();
        }

        void GetGrid()
        {
            try
            {
                this.iGridDataSoure = client.GetAllSinhVien();
                grd.ItemsSource = this.iGridDataSoure;
            }
            catch (Exception)
            {
                throw;
            }   
        }

        bool Validate()
        {
            try
            {
                if (this.iDataSoure.Rows[0]["ID_LOPHOC"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng chọn lớp học!", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                    cbbLop.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["TRANGTHAI"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng chọn trạng thái!", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                    cbbTrangThai.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["MA_SINHVIEN"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập mã sinh viên!", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                    txtMaSV.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["TEN_SINHVIEN"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập tên sinh viên!", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                    txtTenSV.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["DIENTHOAI"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập số điện thoại!", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                    txtDT.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["DIACHI"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập địa chỉ!", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                    txtDCHI.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["CMND"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập CMND!", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                    txtCMND.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["NGAYCAP"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập ngày cấp CMND!", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                    txtNgayCap.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["NOICAP"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập nơi cấp CMND!", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                    txtNoiCap.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["NGAYSINH"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập ngày sinh!", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                    txtNgaySinh.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["NOISINH"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập nơi sinh!", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                    txtNoiSinh.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["THONGTIN_NGOAITRU"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập thông tin cư trú!", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                    txtTTNgoaiTru.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["NGAY_VAOTRUONG"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập ngày nhập học!", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                    txtNgayNhapHoc.Focus();
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        DataTable TableSchemaBinding()
        {
            try
            {
                DataTable dt = new DataTable();
                Dictionary<string, Type> dic = new Dictionary<string, Type>();
                dic.Add("ID_SINHVIEN", typeof(int));
                dic.Add("ID_LOPHOC", typeof(int));
                dic.Add("TRANGTHAI", typeof(string));
                dic.Add("MA_SINHVIEN", typeof(string));
                dic.Add("TEN_SINHVIEN", typeof(string));
                dic.Add("PATH_ANH", typeof(string));
                dic.Add("GIOITINH", typeof(bool));
                dic.Add("DIENTHOAI", typeof(string));
                dic.Add("DIENTHOAI_GD", typeof(string));
                dic.Add("DIACHI", typeof(string));
                dic.Add("EMAIL", typeof(string));
                dic.Add("CMND", typeof(string));
                dic.Add("NGAYCAP", typeof(DateTime));
                dic.Add("NOICAP", typeof(string));
                dic.Add("NGAYSINH", typeof(DateTime));
                dic.Add("NOISINH", typeof(string));
                dic.Add("THONGTIN_NGOAITRU", typeof(string));
                dic.Add("IS_DOANVIEN", typeof(bool));
                dic.Add("NGAY_VAODOAN", typeof(DateTime));
                dic.Add("NGAY_VAOTRUONG", typeof(DateTime));
                dic.Add("NGAY_RATRUONG", typeof(DateTime));
                dic.Add("TEN_LOP", typeof(string));
                dic.Add("USER", typeof(string));
                dt = TableUtil.ConvertToTable(dic);
                return dt;
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
                GetGrid();
                SetIsNull();
                flagsave = true;
                txtMaSV.Focus();
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
                if (Validate())
                {
                    if (flagsave)
                    {
                        if (client.CheckMSSV(this.iDataSoure.Rows[0]["MA_SINHVIEN"].ToString()))
                        {
                            bool res = client.Insert_SinhVien(this.iDataSoure.Copy());
                            if (!res)
                            {
                                CTMessagebox.Show("Thêm mới không thành công", "Thêm mới", "", CTICON.Information, CTBUTTON.YesNo);
                            }
                            GetGrid();
                            SetIsNull();
                            txtMaSV.Focus();
                        }
                        else
                        {
                            CTMessagebox.Show("Mã sinh viên này đã tồn tại", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                            txtMaSV.Focus();
                        }
                        
                    }
                    else
                    {
                        int res = client.Update_SinhVien(this.iDataSoure.Copy());
                        if (res != 0)
                        {
                            CTMessagebox.Show("Cập nhật thành công", "Cập nhật", "", CTICON.Information, CTBUTTON.YesNo);
                        }
                        else
                        {
                            CTMessagebox.Show("Cập nhật không thành công", "Cập nhật", "", CTICON.Information, CTBUTTON.YesNo);
                        }
                        GetGrid();
                        SetIsNull();
                        txtMaSV.Focus();
                    }
                }
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        private void btnDelete_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CTMessagebox.Show("Bạn có muốn xóa", "Xóa", "", CTICON.Information, CTBUTTON.YesNo) == CTRESPONSE.Yes)
                {
                    int res = client.Delete_SinhVien(this.iDataSoure.Copy());
                    if (res != 0)
                    {
                        CTMessagebox.Show("Xóa thành công", "Xóa", "", CTICON.Information, CTBUTTON.YesNo);
                    }
                    else
                    {
                        CTMessagebox.Show("Xóa không thành công", "Xóa", "", CTICON.Information, CTBUTTON.YesNo);
                    }
                }
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
                btnAddNew_OnClick(sender, e);
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
                DataRow r = null;
                if (this.grd.GetFocusedRow() == null)
                    return;
                r = ((DataRowView)this.grd.GetFocusedRow()).Row;

                this.iDataSoure.Rows[0]["ID_SINHVIEN"] = r["ID_SINHVIEN"];
                this.iDataSoure.Rows[0]["ID_LOPHOC"] = r["ID_LOPHOC"];
                this.iDataSoure.Rows[0]["TRANGTHAI"] = r["TRANGTHAI"];
                this.iDataSoure.Rows[0]["MA_SINHVIEN"] = r["MA_SINHVIEN"];
                this.iDataSoure.Rows[0]["TEN_SINHVIEN"] = r["TEN_SINHVIEN"];
                this.iDataSoure.Rows[0]["PATH_ANH"] = r["PATH_ANH"];
                this.iDataSoure.Rows[0]["GIOITINH"] = r["GIOITINH"];
                this.iDataSoure.Rows[0]["DIENTHOAI"] = r["DIENTHOAI"];
                this.iDataSoure.Rows[0]["DIENTHOAI_GD"] = r["DIENTHOAI_GD"];
                this.iDataSoure.Rows[0]["DIACHI"] = r["DIACHI"];
                this.iDataSoure.Rows[0]["EMAIL"] = r["EMAIL"];
                this.iDataSoure.Rows[0]["CMND"] = r["CMND"];
                this.iDataSoure.Rows[0]["NGAYCAP"] = r["NGAYCAP"];
                this.iDataSoure.Rows[0]["NOICAP"] = r["NOICAP"];
                this.iDataSoure.Rows[0]["NGAYSINH"] = r["NGAYSINH"];
                this.iDataSoure.Rows[0]["NOISINH"] = r["NOISINH"];
                this.iDataSoure.Rows[0]["THONGTIN_NGOAITRU"] = r["THONGTIN_NGOAITRU"];
                this.iDataSoure.Rows[0]["IS_DOANVIEN"] = r["IS_DOANVIEN"];
                this.iDataSoure.Rows[0]["NGAY_VAODOAN"] = r["NGAY_VAODOAN"];
                this.iDataSoure.Rows[0]["NGAY_VAOTRUONG"] = r["NGAY_VAOTRUONG"];
                this.iDataSoure.Rows[0]["NGAY_RATRUONG"] = r["NGAY_RATRUONG"];
                this.iDataSoure.Rows[0]["TEN_LOP"] = r["TEN_LOP"];

                flagsave = false;
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
