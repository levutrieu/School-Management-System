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
using DATN.TTS.TVMH.Resource;
using DevExpress.Utils;
using DevExpress.Xpf.Grid;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_GiangVien.xaml
    /// </summary>
    public partial class frm_GiangVien : Page
    {
        bus_GiangVien client = new bus_GiangVien();
        private DataTable iDataSoure = null;
        private DataTable iGridDataSoure = null;
        private bool flagsave = true;
        public frm_GiangVien()
        {
            InitializeComponent();
            this.iDataSoure = TableSchemaBinding();
            DataContext = this.iDataSoure;

            this.iDataSoure.Rows[0]["USER"] = UserCommon.UserName;
            GetComboTrangThai();
            InitGrid();
            SetCombo();
        }

        void GetComboTrangThai()
        {
            try
            {
                List<string> lst = new List<string>() { "Đang làm việc", "Thực tập", "Đang học nâng cấp", "Đã hết hợp đồng", "Chuyển cở sở", "Khác" };
                DataTable dt = new DataTable();
                dt.Columns.Add("TRANGTHAI", typeof(string));
                dt.Columns.Add("NAME_TRANGTHAI", typeof(string));

                for (int i = 0; i <= lst.Count - 1; i++)
                {
                    DataRow r = dt.NewRow();
                    r["TRANGTHAI"] = lst[i].ToString();
                    r["NAME_TRANGTHAI"] = lst[i].ToString();

                    dt.Rows.Add(r);
                    dt.AcceptChanges();
                }
                ComboBoxUtil.SetComboBoxEdit(cbbTrangThai, "NAME_TRANGTHAI", "TRANGTHAI", dt, SelectionTypeEnum.Native);
                this.iDataSoure.Rows[0]["TRANGTHAI"] = cbbTrangThai.GetKeyValue(0);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        void SetCombo()
        {
            //cbbChucVu.ItemsSource = client.GetAllChucVu();
            //cbbKhoa.ItemsSource = client.GetAllKhoa();
            DataTable dt = client.GetAllChucVu();
            if (dt.Rows.Count > 0)
            {
                ComboBoxUtil.SetComboBoxEdit(cbbChucVu, "TEN_CHUCVU", "ID_CHUCVU", dt, SelectionTypeEnum.Native);
                this.iDataSoure.Rows[0]["ID_CHUCVU"] = cbbChucVu.GetKeyValue(0);
            }
            DataTable xdt = client.GetAllKhoa();
            if (xdt.Rows.Count > 0)
            {
                ComboBoxUtil.SetComboBoxEdit(cbbKhoa,"TEN_KHOA", "ID_KHOA", xdt, SelectionTypeEnum.Native);
                this.iDataSoure.Rows[0]["ID_KHOA"] = cbbKhoa.GetKeyValue(0);
            }
        }

        void SetIsNull()
        {
            this.iDataSoure.Rows[0]["ID_GIANGVIEN"] = "0";
            this.iDataSoure.Rows[0]["HE_SOLUONG"] = "0";
            this.iDataSoure.Rows[0]["TRANGTHAI"] = cbbTrangThai.GetKeyValue(0);
            this.iDataSoure.Rows[0]["ID_CHUCVU"] = cbbChucVu.GetKeyValue(0);
            this.iDataSoure.Rows[0]["TEN_GIANGVIEN"] = string.Empty;
            this.iDataSoure.Rows[0]["MA_GIANGVIEN"] = string.Empty;
            this.iDataSoure.Rows[0]["PATH_ANH"] = string.Empty;
            this.iDataSoure.Rows[0]["GIOITINH"] = "False";
            this.iDataSoure.Rows[0]["DIENTHOAI"] = string.Empty;
            this.iDataSoure.Rows[0]["DIACHI"] = string.Empty;
            this.iDataSoure.Rows[0]["EMAIL"] = string.Empty;
            this.iDataSoure.Rows[0]["CMND"] = string.Empty;
            this.iDataSoure.Rows[0]["NGAYCAP"] = DateTime.Parse("01/01/1990");
            this.iDataSoure.Rows[0]["NOICAP"] = string.Empty;
            this.iDataSoure.Rows[0]["NGAYSINH"] = DateTime.Parse("01/01/1990");
            this.iDataSoure.Rows[0]["NOISINH"] = string.Empty;
            this.iDataSoure.Rows[0]["NAM_LAMVIEC"] = DateTime.Today;
            this.iDataSoure.Rows[0]["NAM_KETTHUC"] = DateTime.Today;
            this.iDataSoure.Rows[0]["GHICHU"] = string.Empty;
            this.iDataSoure.Rows[0]["ID_KHOA"] = cbbKhoa.GetKeyValue(0);
        }

        void InitGrid()
        {
            GridColumn col = null;
            col = new GridColumn();
            col.FieldName = "ID_GIANGVIEN";
            col.Header = string.Empty;
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "ID_KHOA";
            col.Header = string.Empty;
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "HE_SOLUONG";
            col.Header = string.Empty;
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "ID_CHUCVU";
            col.Header = string.Empty;
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            
            grd.Columns.Add(col);

            //col = new GridColumn();
            //col.FieldName = "TEN_KHOA";
            //col.Header = "Khoa giảng dạy";
            //col.Width = 50;
            //col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            //col.AllowEditing = DefaultBoolean.False;
            //col.Visible = true;
            //
            //col.AllowCellMerge = false;
            //grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "PATH_ANH";
            col.Header = "Ảnh";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "MA_GIANGVIEN";
            col.Header = "Mã GV";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "TEN_GIANGVIEN";
            col.Header = "Giảng viên";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "TRANGTHAI";
            col.Header = "Trạng thái làm việc";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "TEN_CHUCVU";
            col.Header = "Chức vụ";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "NGAYSINH";
            col.Header = "Ngày sinh";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "NOISINH";
            col.Header = "Nơi sinh";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            
            col.AllowCellMerge = false;
            grd.Columns.Add(col);


            col = new GridColumn();
            col.FieldName = "GIOITINH";
            col.Header = "Giới tính";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "DIENTHOAI";
            col.Header = "Điện thoại";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "DIACHI";
            col.Header = "Địa chỉ";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "EMAIL";
            col.Header = "Email";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "CMND";
            col.Header = "CMND";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "NGAYCAP";
            col.Header = "Ngày cấp";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "NOICAP";
            col.Header = "Nơi cấp";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            col.AllowCellMerge = false;
            grd.Columns.Add(col);
            
            col = new GridColumn();
            col.FieldName = "NAM_LAMVIEC";
            col.Header = "Năm làm việc";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "NAM_KETTHUC";
            col.Header = "Năm hết hợp đồng";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "GHICHU";
            col.Header = "Ghi chú";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            grdViewNDung.AutoWidth = true;
            GetGrid();
        }

        void GetGrid()
        {
            try
            {
                this.iGridDataSoure = client.GetAllGiangVien();
                grd.ItemsSource = this.iGridDataSoure;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        bool Validate()
        {
            try
            {
                if (this.iDataSoure.Rows[0]["ID_KHOA"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng chọn khoa giảng dạy!", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                    cbbKhoa.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["HE_SOLUONG"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập hệ số lương!", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                    txtHSL.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["ID_CHUCVU"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng chọn chức vụ!", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                    cbbChucVu.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["MA_GIANGVIEN"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập mã giảng viên!", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                    txtMaSV.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["TEN_GIANGVIEN"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập tên giảng viên!", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                    txtTenSV.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["NGAYSINH"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập ngày sinh", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                    txtNgaySinh.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["DIENTHOAI"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập số điện thoại!", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                    txtDT.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["DIACHI"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập địa chỉ!", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                    txtDCHI.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["CMND"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập CMND!", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                    txtCMND.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["NGAYCAP"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập ngày cấp CMND!", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                    txtNgayCap.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["NOICAP"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập nơi cấp CMND!", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                    txtNoiCap.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["NGAYSINH"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập ngày sinh!", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                    txtNgaySinh.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["NOISINH"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập nơi sinh!", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                    txtNoiSinh.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["NAM_LAMVIEC"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập năm làm việc!", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                    txtNamVaoLam.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["NAM_KETTHUC"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập năm hết hợp đồng!", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                    txtNamHetHopDong.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["TRANGTHAI"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập năm hết hợp đồng!", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                    cbbTrangThai.Focus();
                    return false;
                }
                return true;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        DataTable TableSchemaBinding()
        {
            try
            {
                DataTable dt = new DataTable();
                Dictionary<string, Type> dic = new Dictionary<string, Type>();
                dic.Add("ID_KHOA", typeof(int));
                dic.Add("ID_GIANGVIEN", typeof(int));
                dic.Add("HE_SOLUONG", typeof(Decimal));
                dic.Add("ID_CHUCVU", typeof(int));
                dic.Add("MA_GIANGVIEN", typeof(string));
                dic.Add("TEN_GIANGVIEN", typeof(string));
                dic.Add("PATH_ANH", typeof(string));
                dic.Add("NGAYSINH", typeof(DateTime));
                dic.Add("GIOITINH", typeof(bool));
                dic.Add("DIACHI", typeof(string));
                dic.Add("DIENTHOAI", typeof(string));
                dic.Add("EMAIL", typeof(string));
                dic.Add("CMND", typeof(string));
                dic.Add("NGAYCAP", typeof(DateTime));
                dic.Add("NOICAP", typeof(string));
                dic.Add("NAM_LAMVIEC", typeof(DateTime));
                dic.Add("NAM_KETTHUC", typeof(DateTime));
                dic.Add("TRANGTHAI", typeof(string));
                dic.Add("GHICHU", typeof(string));
                dic.Add("NOISINH", typeof(string));
                dic.Add("USER", typeof(string));
                dt = TableUtil.ConvertToTable(dic);
                return dt;
            }
            catch (Exception err)
            {
                throw err;
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
                r = ((DataRowView) this.grd.GetFocusedRow()).Row;
                this.iDataSoure.Rows[0]["ID_GIANGVIEN"] = r["ID_GIANGVIEN"];
                this.iDataSoure.Rows[0]["HE_SOLUONG"] = r["HE_SOLUONG"];
                this.iDataSoure.Rows[0]["TRANGTHAI"] = r["TRANGTHAI"];
                this.iDataSoure.Rows[0]["ID_CHUCVU"] = r["ID_CHUCVU"];
                this.iDataSoure.Rows[0]["TEN_GIANGVIEN"] = r["TEN_GIANGVIEN"];
                this.iDataSoure.Rows[0]["MA_GIANGVIEN"] = r["MA_GIANGVIEN"];
                this.iDataSoure.Rows[0]["PATH_ANH"] = r["PATH_ANH"];
                this.iDataSoure.Rows[0]["GIOITINH"] = r["GIOITINH"];
                this.iDataSoure.Rows[0]["DIENTHOAI"] = r["DIENTHOAI"];
                this.iDataSoure.Rows[0]["DIACHI"] = r["DIACHI"];
                this.iDataSoure.Rows[0]["EMAIL"] = r["EMAIL"];
                this.iDataSoure.Rows[0]["CMND"] = r["CMND"];
                this.iDataSoure.Rows[0]["NGAYCAP"] = r["NGAYCAP"];
                this.iDataSoure.Rows[0]["NOICAP"] = r["NOICAP"];
                this.iDataSoure.Rows[0]["NGAYSINH"] = r["NGAYSINH"];
                this.iDataSoure.Rows[0]["NOISINH"] = r["NOISINH"];
                this.iDataSoure.Rows[0]["NAM_LAMVIEC"] = r["NAM_LAMVIEC"];
                this.iDataSoure.Rows[0]["NAM_KETTHUC"] = r["NAM_KETTHUC"];
                this.iDataSoure.Rows[0]["GHICHU"] = r["GHICHU"];
                this.iDataSoure.Rows[0]["ID_KHOA"] = r["ID_KHOA"];
                flagsave = false;
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

        private void btnAddNew_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                SetIsNull();
                txtMaSV.Focus();
                this.iDataSoure.Rows[0]["GIOITINH"] = "False";
                flagsave = true;
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

        private void btnSave_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (Validate())
                {
                    if (flagsave)
                    {
                        bool res = client.Insert_Giangvien(this.iDataSoure.Copy());
                        if (!res)
                        {
                            CTMessagebox.Show("Thêm mới không thành công", "Thêm mới", "", CTICON.Information,
                                CTBUTTON.YesNo);
                        }
                        else
                        {
                            GetGrid();
                            SetIsNull();
                            txtMaSV.Focus();
                        }
                    }
                    else
                    {
                        bool res = client.Update_GiangVien(this.iDataSoure.Copy());
                        if (!res)
                        {
                            CTMessagebox.Show("Thêm mới không thành công", "Thêm mới", "", CTICON.Information,CTBUTTON.YesNo);
                        }
                        else
                        {
                            GetGrid();
                            SetIsNull();
                            txtMaSV.Focus();
                        }
                    }
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

        private void btnDelete_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (CTMessagebox.Show("Bạn có muốn xóa", "Xóa", "", CTICON.Information, CTBUTTON.YesNo) == CTRESPONSE.Yes)
                {
                    bool res = client.Delete_GiangVien(this.iDataSoure.Copy());
                    if (!res)
                    {
                        CTMessagebox.Show("Xóa thành công", "Xóa", "", CTICON.Information, CTBUTTON.YesNo);
                    }
                    else
                    {
                        btnAddNew_OnClick(sender, e);
                    }
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

        private void btnRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            btnAddNew_OnClick(sender, e);
        }

        
    }
}
