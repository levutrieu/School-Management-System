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
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_HeDaoTao.xaml
    /// </summary>
    public partial class frm_HeDaoTao : Page
    {
        private DataTable iDataSoure = null;
        private DataTable iGridDataSoure = null;
        private bool flagsave = true;
        bus_HeDaoTao client = new bus_HeDaoTao();
        public frm_HeDaoTao()
        {
            InitializeComponent();
            this.iDataSoure = TableSchemabinding();
            this.DataContext = iDataSoure;
            this.iDataSoure.Rows[0]["USER"] = UserCommon.UserName;
            InitGrid();
            SetComBo();
        }

        DataTable TableSchemabinding()
        {
            try
            {
                DataTable dt = null;
                Dictionary<string, Type> dic = new Dictionary<string, Type>();
                dic.Add("ID_HE_DAOTAO", typeof(int));
                dic.Add("ID_BAC_DAOTAO", typeof(int));
                dic.Add("ID_LOAIHINH_DTAO", typeof(int));
                dic.Add("MA_HE_DAOTAO", typeof(string));
                dic.Add("TEN_HE_DAOTAO", typeof(string));
                dic.Add("SO_NAMHOC", typeof(Decimal));
                dic.Add("TEN_BAC_DAOTAO", typeof(string));
                dic.Add("TEN_LOAIHINH_DTAO", typeof(string));
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

        private void InitGrid()
        {
            GridColumn col;

            col = new GridColumn();
            col.FieldName = "ID_HE_DAOTAO";
            col.Header = string.Empty;
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "ID_BAC_DAOTAO";
            col.Header = string.Empty;
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "ID_LOAIHINH_DTAO";
            col.Header = string.Empty;
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "MA_HE_DAOTAO";
            col.Header = "Mã hệ đào tạo";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            col.EditSettings = new TextEditSettings();
            col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "TEN_HE_DAOTAO";
            col.Header = "Hệ đào tạo";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "SO_NAMHOC";
            col.Header = "Số năm học";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            col.EditSettings = new TextEditSettings();
            col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "TEN_BAC_DAOTAO";
            col.Header = "Bậc đào tạo";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "TEN_LOAIHINH_DTAO";
            col.Header = "Loại hình đào tạo";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            grd.Columns.Add(col);      

            GetGrid();
        }

        private void GetGrid()
        {
            iGridDataSoure = client.GetAllHeDaoTao();
            grd.ItemsSource = iGridDataSoure.Copy();
        }

        private void SetComBo()
        {
            try
            {
                DataTable dt = client.GetAllBacDaoTao();
                if (dt.Rows.Count > 0)
                {
                    ComboBoxUtil.SetComboBoxEdit(cbbbacdt, "TEN_BAC_DAOTAO", "ID_BAC_DAOTAO", dt, SelectionTypeEnum.Native);
                    this.iDataSoure.Rows[0]["ID_BAC_DAOTAO"] = cbbbacdt.GetKeyValue(0);
                }
                DataTable xdt = client.GetAllLoaiHinhDaoTao();
                if (xdt.Rows.Count > 0)
                {
                    ComboBoxUtil.SetComboBoxEdit(cbbloaihinhdt, "TEN_LOAIHINH_DTAO", "ID_LOAIHINH_DTAO", xdt, SelectionTypeEnum.Native);
                    this.iDataSoure.Rows[0]["ID_LOAIHINH_DTAO"] = cbbloaihinhdt.GetKeyValue(0);
                }
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
        }

        private void SetIsNull()
        {
            try
            {
                this.iDataSoure.Rows[0]["ID_BAC_DAOTAO"] = cbbbacdt.GetKeyValue(0);
                this.iDataSoure.Rows[0]["ID_LOAIHINH_DTAO"] =  cbbloaihinhdt.GetKeyValue(0);
                this.iDataSoure.Rows[0]["MA_HE_DAOTAO"] = string.Empty;
                this.iDataSoure.Rows[0]["TEN_HE_DAOTAO"] = string.Empty;
                this.iDataSoure.Rows[0]["SO_NAMHOC"] = 0;
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
        }

        private bool ValiDate()
        {
            if (this.iDataSoure.Rows[0]["ID_BAC_DAOTAO"].ToString() == "0")
            {
                CTMessagebox.Show("Vui lòng chọn bậc đào tạo", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                cbbbacdt.Focus();
                return false;
            }
            if (this.iDataSoure.Rows[0]["ID_LOAIHINH_DTAO"].ToString() == "0")
            {
                CTMessagebox.Show("Vui lòng chọn loại hình đào tạo", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                cbbloaihinhdt.Focus();
                return false;
            }
            if (this.iDataSoure.Rows[0]["MA_HE_DAOTAO"].ToString() == string.Empty)
            {
                CTMessagebox.Show("Vui lòng nhập mã hệ đào tạo", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                txtMaHDT.Focus();
                return false;
            }
            if (this.iDataSoure.Rows[0]["TEN_HE_DAOTAO"].ToString() == string.Empty)
            {
                CTMessagebox.Show("Vui lòng nhập tên hệ đào tạo", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                txtTenHDT.Focus();
                return false;
            }
            if (this.iDataSoure.Rows[0]["SO_NAMHOC"].ToString() == string.Empty)
            {
                CTMessagebox.Show("Vui lòng nhập số năm học", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                txtSonam.Focus();
                return false;
            }
            return true;
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
                this.iDataSoure.Rows[0]["ID_HE_DAOTAO"] = row["ID_HE_DAOTAO"];
                this.iDataSoure.Rows[0]["ID_BAC_DAOTAO"] = row["ID_BAC_DAOTAO"];
                this.iDataSoure.Rows[0]["ID_LOAIHINH_DTAO"] = row["ID_LOAIHINH_DTAO"];
                this.iDataSoure.Rows[0]["MA_HE_DAOTAO"] = row["MA_HE_DAOTAO"];
                this.iDataSoure.Rows[0]["TEN_HE_DAOTAO"] = row["TEN_HE_DAOTAO"];
                this.iDataSoure.Rows[0]["SO_NAMHOC"] = row["SO_NAMHOC"];
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

        private void btnAddNew_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                flagsave = true;
                SetIsNull();
                txtMaHDT.Focus();
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
                if (ValiDate())
                {
                    if (flagsave)
                    {
                        if (client.KiemTrungMa(this.iDataSoure.Rows[0]["MA_HE_DAOTAO"].ToString()))
                        {
                            bool res = client.Insert_HeDaoTao(this.iDataSoure.Copy());
                            if (!res)
                            {
                                CTMessagebox.Show("Thêm mới không thành công.", "Thêm mới", "", CTICON.Error,
                                    CTBUTTON.OK);
                                return;
                            }
                            else
                            {
                                CTMessagebox.Show("Thêm mới thành công.", "Thêm mới", "", CTICON.Information,
                                    CTBUTTON.OK);
                                SetIsNull();
                                GetGrid();
                            }

                        }
                        else
                        {
                            CTMessagebox.Show("Trùng mã.", "Thêm mới", "", CTICON.Error, CTBUTTON.OK);
                            txtMaHDT.Focus();
                        }
                    }
                    else
                    {
                        bool res = client.Update_HeDaoTao(this.iDataSoure.Copy());
                        if (!res)
                        {
                            CTMessagebox.Show("Cập nhật không thành công.", "Cập nhật", "", CTICON.Error, CTBUTTON.OK);
                            return;
                        }
                        else
                        {
                            CTMessagebox.Show("Cập nhật thành công.", "Cập nhật", "", CTICON.Information, CTBUTTON.OK);
                            SetIsNull();
                            GetGrid();
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

        private void btnRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                GetGrid();
                SetIsNull();
                txtMaHDT.Focus();
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
                if (CTMessagebox.Show("Bạn có muốn xóa", "Xóa", "", CTICON.Information, CTBUTTON.OK) == CTRESPONSE.OK)
                {
                    bool res = client.Delete_HeDaoTao(int.Parse(this.iDataSoure.Rows[0]["ID_HE_DAOTAO"].ToString()), this.iDataSoure.Rows[0]["USER"].ToString());
                    if (!res)
                    {
                        CTMessagebox.Show("Xóa không thành công.", "Xóa", "", CTICON.Error, CTBUTTON.OK);
                        return;
                    }
                    else
                    {
                        CTMessagebox.Show("Xóa thành công.", "Xóa", "", CTICON.Information, CTBUTTON.OK);
                        SetIsNull();
                        GetGrid();
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
    }
}
