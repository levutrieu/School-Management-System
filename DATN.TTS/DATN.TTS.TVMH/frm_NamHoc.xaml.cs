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
using DevExpress.Utils;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_NamHoc.xaml
    /// </summary>
    public partial class frm_NamHoc : Page
    {
        private DataTable iDataSoure = null;
        private DataTable iGridDataSoure = null;
        private bool flagsave;
        bus_namhoc client = new bus_namhoc();
        public frm_NamHoc()
        {
            InitializeComponent();
            this.iDataSoure = TableSchemaBinding();
            this.DataContext = iDataSoure;
            this.iDataSoure.Rows[0]["USER"] = UserCommon.UserName;
            InitGrid();
        }

        private void InitGrid()
        {
            try
            {
                GridColumn col = null;

                col = new GridColumn();
                col.FieldName = "ID_NAMHOC_HIENTAI";
                col.Header = string.Empty;
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "NAMHOC_TU";
                col.Header = "Năm bắt đầu";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.AllowEditing = DefaultBoolean.False;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                col.Visible = true;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "NAMHOC_DEN";
                col.Header = "Năm kết thúc";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.AllowEditing = DefaultBoolean.False;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                col.Visible = true;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "NGAY_BATDAU";
                col.Header = "Ngày bắt đầu";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "SO_TUAN";
                col.Header = "Số tuần";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.AllowEditing = DefaultBoolean.False;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                col.Visible = true;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "SO_HKY_TRONGNAM";
                col.Header = "Số học kỳ trong năm";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.AllowEditing = DefaultBoolean.False;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                col.Visible = true;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "IS_HIENTAI";
                col.Header = "Năm hiện tại";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.AllowEditing = DefaultBoolean.False;
                col.EditSettings = new CheckEditSettings();
                col.Visible = true;
                grd.Columns.Add(col);

                GetGrid();
            }
            catch (Exception er)
            {
                throw er;
            }
        }

        private void GetGrid()
        {
            iGridDataSoure = client.GetAll();
            grd.ItemsSource = iGridDataSoure;
        }

        private DataTable TableSchemaBinding()
        {
            try
            {
                DataTable dt = null;
                Dictionary<string, Type> dic = new Dictionary<string, Type>();
                dic.Add("ID_NAMHOC_HIENTAI", typeof(Decimal));
                dic.Add("NAMHOC_TU", typeof(Decimal));
                dic.Add("NAMHOC_DEN", typeof(Decimal));
                dic.Add("NGAY_BATDAU", typeof(DateTime));
                dic.Add("SO_TUAN", typeof(Decimal));
                dic.Add("SO_HKY_TRONGNAM", typeof(Decimal));
                dic.Add("IS_HIENTAI", typeof(Decimal));
                dic.Add("USER", typeof(string));
                dt = TableUtil.ConvertToTable(dic);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SetIsNull()
        {
            try
            {
                this.iDataSoure.Rows[0]["NAMHOC_TU"] = DateTime.Now.Year;
                this.iDataSoure.Rows[0]["NAMHOC_DEN"] = "0";
                this.iDataSoure.Rows[0]["NGAY_BATDAU"] = DateTime.Now;
                this.iDataSoure.Rows[0]["SO_HKY_TRONGNAM"] = "0";
                this.iDataSoure.Rows[0]["SO_TUAN"] = "0";
            }
            catch (Exception er)
            {
                
                throw er;
            }
        }

        private Boolean CheckNgayBatDau(string ngaybatdau)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                DateTime _NgayBatDau = DateTime.Parse(ngaybatdau);
                if (_NgayBatDau.DayOfWeek != DayOfWeek.Monday)
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                    return false;
                }
                int namhoc = int.Parse(txtNamBD.Text);
                if (namhoc > 0)
                {
                    if (_NgayBatDau.Year != namhoc)
                    {
                        Mouse.OverrideCursor = Cursors.Arrow;
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                CTMessagebox.Show(ex.Message, "Lỗi", "", CTICON.Error, CTBUTTON.YesNo);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
            return true;
        }

        private bool ValiDate()
        {
            try
            {
                if (this.iDataSoure.Rows[0]["NAMHOC_TU"].ToString() == "0")
                {
                    CTMessagebox.Show("Vui lòng chọn năm học bắt đầu!", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                    txtNamBD.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["NAMHOC_DEN"].ToString() == "0")
                {
                    CTMessagebox.Show("Vui lòng chọn năm học kết thúc!", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                    txtNamDen.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["NGAY_BATDAU"].ToString() == "0")
                {
                    CTMessagebox.Show("Vui lòng chọn ngày bắt đầu năm học!", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                    txtNgayBD.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["SO_HKY_TRONGNAM"].ToString() == "0")
                {
                    CTMessagebox.Show("Vui lòng nhập số học kỳ của năm học!", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                    txtHKTrongNam.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["SO_TUAN"].ToString() == "0")
                {
                    CTMessagebox.Show("Vui lòng nhập số tuần học của năm!", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                    txtSotuan.Focus();
                    return false;
                }
                return true;
            }
            catch (Exception er)
            {
                throw er;
            }
        }

        private void btnAddNew_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                GetGrid();
                SetIsNull();
                txtNamBD.Focus();
                flagsave = true;
            }
            catch (Exception er)
            {
                throw er;
            }
        }

        private void btnSave_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValiDate())
                {
                    if (CheckNgayBatDau(this.iDataSoure.Rows[0]["NGAY_BATDAU"].ToString()))
                    {
                        if (flagsave)
                        {
                            bool res = client.Insert_NamHocHienTai(this.iDataSoure.Copy());
                            if (!res)
                            {
                                CTMessagebox.Show("Thêm mới thất bại", "Thêm mới", "", CTICON.Information, CTBUTTON.YesNo);
                            }
                            else
                            {
                                CTMessagebox.Show("Thêm mới thành công", "Thêm mới", "", CTICON.Information, CTBUTTON.YesNo);
                                GetGrid();
                                SetIsNull();
                                txtNamBD.Focus();
                            }
                        }
                        else
                        {
                            bool res = client.Insert_NamHocHienTai(this.iDataSoure.Copy());
                            if (!res)
                            {
                                CTMessagebox.Show("Cập nhật thất bại", "Cập nhật", "", CTICON.Information, CTBUTTON.YesNo);
                            }
                            else
                            {
                                CTMessagebox.Show("Cập nhật thành công", "Cập nhật", "", CTICON.Information, CTBUTTON.YesNo);
                                GetGrid();
                                SetIsNull();
                                txtNamBD.Focus();
                            }
                        }
                    }
                    else
                    {
                        CTMessagebox.Show("Ngày bắt đầu năm học phải là thứ 2", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                        txtNgayBD.Focus();
                    }
                    
                }
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
                try
                {
                    if (CTMessagebox.Show("Bạn muốn xóa?", "Xóa", "", CTICON.Information, CTBUTTON.YesNo) == CTRESPONSE.Yes)
                    {
                        bool res = client.Delete_NamHocHienTai(Convert.ToInt32(this.iDataSoure.Rows[0]["ID_NAMHOC_HIENTAI"].ToString()), UserCommon.UserName);
                        if (!res)
                        {
                            CTMessagebox.Show("Xóa không thành công", "Xóa", "", CTICON.Information, CTBUTTON.YesNo);
                        }
                        else
                        {
                            CTMessagebox.Show("Xóa thành công", "Xóa", "", CTICON.Information, CTBUTTON.YesNo);
                            GetGrid();
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
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
                btnAddNew_OnClick(sender,e);
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
                r = ((DataRowView)grd.GetFocusedRow()).Row;
                this.iDataSoure.Rows[0]["ID_NAMHOC_HIENTAI"] = r["ID_NAMHOC_HIENTAI"];
                this.iDataSoure.Rows[0]["NAMHOC_TU"] = r["NAMHOC_TU"];
                this.iDataSoure.Rows[0]["NAMHOC_DEN"] = r["NAMHOC_DEN"];
                this.iDataSoure.Rows[0]["NGAY_BATDAU"] = r["NGAY_BATDAU"];
                this.iDataSoure.Rows[0]["SO_HKY_TRONGNAM"] = r["SO_HKY_TRONGNAM"];
                this.iDataSoure.Rows[0]["IS_HIENTAI"] = r["IS_HIENTAI"];
                this.iDataSoure.Rows[0]["SO_TUAN"] = r["SO_TUAN"];
            }
            catch (Exception er)
            {
                throw er;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void BtnSetNamHoc_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (!this.iDataSoure.Rows[0]["ID_NAMHOC_HIENTAI"].ToString().Equals(string.Empty))
                {
                    bool res = client.SetNamHocHienTai(Convert.ToInt32(this.iDataSoure.Rows[0]["ID_NAMHOC_HIENTAI"].ToString()));
                    if (!res)
                    {
                        CTMessagebox.Show("Đặt năm học hiện tại thất bại", "Lỗi", "", CTICON.Error, CTBUTTON.YesNo);
                    }
                    else
                    {
                        CTMessagebox.Show("Đặt năm học hiện tại thành công", "Thành công", "", CTICON.Information, CTBUTTON.YesNo);
                        GetGrid();
                    }
                }
            }
            catch (Exception er)
            {
                throw er;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void TxtNamBD_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                if (txtNamBD.Text.Length == 4)
                {
                    int namhoc = int.Parse(txtNamBD.Text);
                    this.iDataSoure.Rows[0]["NAMHOC_DEN"] = namhoc + 1;
                }
            }
            catch (Exception ex)
            {
                CTMessagebox.Show(ex.Message, "Lỗi", "", CTICON.Error, CTBUTTON.YesNo);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }
    }
}
