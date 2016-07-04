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
using System.Windows.Documents.DocumentStructures;
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
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_LopHoc.xaml
    /// </summary>
    public partial class frm_LopHoc : Page
    {
        bus_LopHoc client = new bus_LopHoc();
        private DataTable iDataSoure = null;
        private DataTable iGridDataSoure = null;
        private bool flagsave = true;
        public frm_LopHoc()
        {
            InitializeComponent();
            this.iDataSoure = TableSchemaBinding();
            this.DataContext = iDataSoure;
            this.iDataSoure.Rows[0]["USER"] = UserCommon.UserName;
            SetComBo();
            InitGrid();

        }

        void InitGrid()
        {
            GridColumn col = null;
            col = new GridColumn();
            col.FieldName = "ID_LOPHOC";
            col.Header = string.Empty;
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "ID_KHOAHOC_NGANH";
            col.Header = string.Empty;
            col.Width = 50;
            col.AutoFilterValue = true;

            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "MA_LOP";
            col.Header = "Mã lớp";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            col.EditSettings = new TextEditSettings();
            col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "TEN_LOP";
            col.Header = "Lớp";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "SOLUONG_SV";
            col.Header = "Sỉ số";
            col.Width = 30;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            col.EditSettings = new TextEditSettings();
            col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "ID_GIANGVIEN_CN";
            col.Header = string.Empty;
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "TEN_GIANGVIEN";
            col.Header = "Chủ nhiệm";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "GHICHU";
            col.Header = "Ghi chú";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            grd.Columns.Add(col);


            GetGrid();
        }

        void SetIsNull()
        {
            this.iDataSoure.Rows[0]["ID_KHOAHOC_NGANH"] = cbbKhoaNganh.GetKeyValue(0);
            this.iDataSoure.Rows[0]["KHOA_NGANH"] = string.Empty;
            this.iDataSoure.Rows[0]["ID_GIANGVIEN"] = cbbGiangVienCN.GetKeyValue(0);
            this.iDataSoure.Rows[0]["MA_LOP"] = string.Empty;
            this.iDataSoure.Rows[0]["TEN_LOP"] = string.Empty;
            this.iDataSoure.Rows[0]["SOLUONG_SV"] = 0;
            this.iDataSoure.Rows[0]["GHICHU"] = string.Empty;
        }

        void SetComBo()
        {
            DataTable dt = null;
            dt = client.GetAllKhoaNganh();
            if (dt.Rows.Count > 0)
            {
                ComboBoxUtil.SetComboBoxEdit(cbbKhoaNganh, "KHOA_NGANH", "ID_KHOAHOC_NGANH", dt, SelectionTypeEnum.Native);
                this.iDataSoure.Rows[0]["ID_KHOAHOC_NGANH"] = cbbKhoaNganh.GetKeyValue(0);
            }
            dt = client.GetGV();
            if (dt.Rows.Count > 0)
            {
                ComboBoxUtil.SetComboBoxEdit(cbbGiangVienCN, "TEN_GIANGVIEN", "ID_GIANGVIEN", dt, SelectionTypeEnum.Native);
                this.iDataSoure.Rows[0]["ID_GIANGVIEN"] = cbbGiangVienCN.GetKeyValue(0);
            }
        }

        void GetGrid()
        {
            this.iGridDataSoure = client.GetAllLop();
            this.grd.ItemsSource = iGridDataSoure.Copy();
        }

        bool Validate()
        {
            if (this.iDataSoure.Rows[0]["MA_LOP"].ToString() == string.Empty)
            {
                CTMessagebox.Show("Vui lòng nhập mã lớp!", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                txtMaLop.Focus();
                return false;
            }
            if (this.iDataSoure.Rows[0]["TEN_LOP"].ToString() == string.Empty)
            {
                CTMessagebox.Show("Vui lòng nhập tên lớp!", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                txtTenLop.Focus();
                return false;
            }
            if (this.iDataSoure.Rows[0]["ID_GIANGVIEN"].ToString() == "0")
            {
                CTMessagebox.Show("Vui lòng chọn giảng viên chủ nhiệm", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                cbbGiangVienCN.Focus();
                return false;
            }
            if (this.iDataSoure.Rows[0]["ID_KHOAHOC_NGANH"].ToString() == "0")
            {
                CTMessagebox.Show("Vui lòng chọn khóa ngành cho lớp", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                cbbKhoaNganh.Focus();
                return false;
            }
            if (this.iDataSoure.Rows[0]["SOLUONG_SV"].ToString() == string.Empty)
            {
                CTMessagebox.Show("Vui lòng nhập số sinh viên của lớp", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                txtSLSV.Focus();
                return false;
            }
            return true;
        }

        DataTable TableSchemaBinding()
        {
            try
            {
                DataTable dt = null;
                Dictionary<string, Type> dic = new Dictionary<string, Type>();
                dic.Add("ID_LOPHOC", typeof(int));
                dic.Add("ID_KHOAHOC_NGANH", typeof(int));
                dic.Add("KHOA_NGANH", typeof(string));
                dic.Add("ID_GIANGVIEN", typeof(int));
                dic.Add("MA_LOP", typeof(string));
                dic.Add("TEN_LOP", typeof(string));
                dic.Add("SOLUONG_SV", typeof(Decimal));
                dic.Add("GHICHU", typeof(string));
                dic.Add("USER", typeof(string));
                dt = TableUtil.ConvertToTable(dic);
                return dt;
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            return null;
        }

        private void btnAddNew_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                GetGrid();
                SetIsNull();
                txtMaLop.Focus();
                flagsave = true;
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

        private void btnSave_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (Validate())
                {
                    if (flagsave)
                    {
                        if (!client.CheckTrungMaLop(this.iDataSoure.Rows[0]["MA_LOP"].ToString()))
                        {
                            CTMessagebox.Show("Trùng mã lớp", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                            txtMaLop.Focus();
                            return;
                        }
                        bool res = client.Insert_Lop(this.iDataSoure.Copy());
                        if (!res)
                        {
                            CTMessagebox.Show("Thêm mới không thành công", "Thêm mới", "", CTICON.Error, CTBUTTON.OK);
                        }
                        else
                        {
                            CTMessagebox.Show("Thêm mới thành công", "Thêm mới", "", CTICON.Information, CTBUTTON.OK);
                            GetGrid();
                            SetIsNull();
                            txtMaLop.Focus();
                        }
                    }
                    else
                    {
                        bool res = client.Update_Lop(this.iDataSoure.Copy());
                        if (!res)
                        {
                            CTMessagebox.Show("Cập nhật không thành công", "Cập nhật", "", CTICON.Error, CTBUTTON.OK);
                        }
                        else
                        {
                            CTMessagebox.Show("Cập nhật thành công", "Cập nhật", "", CTICON.Information, CTBUTTON.OK);
                            GetGrid();
                            SetIsNull();
                            txtMaLop.Focus();
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

        private void btnDelete_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (CTMessagebox.Show("Bạn muốn xóa?", "Xóa", "", CTICON.Information, CTBUTTON.YesNo) == CTRESPONSE.Yes)
                {
                    bool res = client.Delete_Lop(this.iDataSoure.Copy());
                    if (!res)
                    {
                        CTMessagebox.Show("Xóa không thành công", "Xóa", "", CTICON.Error, CTBUTTON.OK);
                    }
                    else
                    {
                        CTMessagebox.Show("Xóa thành công", "Xóa", "", CTICON.Information, CTBUTTON.OK);
                        GetGrid();
                        SetIsNull();
                        txtMaLop.Focus();
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

        private void btnRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                btnAddNew_OnClick(null, null);
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

        private void GrdViewNDung_OnFocusedRowChanged(object sender, DevExpress.Xpf.Grid.FocusedRowChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                DataRow r = null;
                if (this.grd.GetFocusedRow() == null)
                    return;
                r = ((DataRowView)this.grd.GetFocusedRow()).Row;
                this.iDataSoure.Rows[0]["ID_LOPHOC"] = r["ID_LOPHOC"];
                this.iDataSoure.Rows[0]["ID_KHOAHOC_NGANH"] = r["ID_KHOAHOC_NGANH"];
                this.iDataSoure.Rows[0]["ID_GIANGVIEN"] = r["ID_GIANGVIEN_CN"];
                this.iDataSoure.Rows[0]["MA_LOP"] = r["MA_LOP"];
                this.iDataSoure.Rows[0]["TEN_LOP"] = r["TEN_LOP"];
                this.iDataSoure.Rows[0]["SOLUONG_SV"] = r["SOLUONG_SV"];
                this.iDataSoure.Rows[0]["GHICHU"] = r["GHICHU"];
                flagsave = false;
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
    }
}
