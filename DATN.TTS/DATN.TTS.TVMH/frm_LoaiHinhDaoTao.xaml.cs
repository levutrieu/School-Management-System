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
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_LoaiHinhDaoTao.xaml
    /// </summary>
    public partial class frm_LoaiHinhDaoTao : Page
    {
        bus_LoaiHinhDaoTao client = new bus_LoaiHinhDaoTao();

        private DataTable iDataSoure = null;

        private DataTable iGridDataSoure = null;

        private bool flagsave = true;

        public frm_LoaiHinhDaoTao()
        {
            InitializeComponent();
            this.iDataSoure = TableSchemaBinding();
            this.DataContext = iDataSoure;

            this.iDataSoure.Rows[0]["USER"] = UserCommon.UserName;
            InitGrid();
        }

        private void InitGrid()
        {
            GridColumn col;

            col = new GridColumn();
            col.FieldName = "ID_LOAIHINH_DTAO";
            col.Header = string.Empty;
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "MA_LOAIHINH_DTAO";
            col.Header = "Mã loại";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            col.EditSettings = new TextEditSettings();
            col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "TEN_LOAIHINH_DTAO";
            col.Header = "Loại hình đào tạo";
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

        private void GetGrid()
        {
            this.iGridDataSoure = client.GetAll();
            grd.ItemsSource = iGridDataSoure;
        }

        private DataTable TableSchemaBinding()
        {
            try
            {
                DataTable dt = null;
                Dictionary<string, Type> dic = new Dictionary<string, Type>();
                dic.Add("ID_LOAIHINH_DTAO", typeof(string));
                dic.Add("MA_LOAIHINH_DTAO", typeof(string));
                dic.Add("TEN_LOAIHINH_DTAO", typeof(string));
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

        private void SetIsNull()
        {
            try
            {
                this.iDataSoure.Rows[0]["MA_LOAIHINH_DTAO"] = string.Empty;
                this.iDataSoure.Rows[0]["TEN_LOAIHINH_DTAO"] = string.Empty;
                this.iDataSoure.Rows[0]["GHICHU"] = string.Empty;
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
        }

        private bool ValiDate()
        {
            try
            {
                if (this.iDataSoure.Rows[0]["MA_LOAIHINH_DTAO"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập mã loại hình đào tạo","Thông báo","", CTICON.Information, CTBUTTON.OK);
                    txtMaloai.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["TEN_LOAIHINH_DTAO"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập tên hình đào tạo", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                    txtTenloai.Focus();
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            return false;
        }

        private void btnAddNew_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                GetGrid();
                SetIsNull();
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
                if (ValiDate())
                {
                    if (flagsave)
                    {
                        bool res = client.Insert_LoaiHinhDaoTao(this.iDataSoure.Copy());
                        if (!res)
                        {
                            CTMessagebox.Show("Thêm mới không thành công", "Thêm mới", "", CTICON.Error, CTBUTTON.OK);
                            return;
                        }
                        else
                        {
                            CTMessagebox.Show("Thêm mới thành công", "Thêm mới", "", CTICON.Information, CTBUTTON.OK);
                            GetGrid();
                            SetIsNull();
                        }

                    }
                    else
                    {
                       bool res = client.Update_LoaiHinhDaoTao(this.iDataSoure.Copy());
                       if (!res)
                       {
                           CTMessagebox.Show("Cập nhật không thành công", "Cập nhật", "", CTICON.Error, CTBUTTON.OK);
                           return;
                       }
                       else
                       {
                           CTMessagebox.Show("Cập nhật thành công", "Cập nhật", "", CTICON.Information, CTBUTTON.OK);
                           GetGrid();
                           SetIsNull();
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
                if (CTMessagebox.Show("Bạn có muốn xóa không?", "Xóa","", CTICON.Information, CTBUTTON.YesNo) == CTRESPONSE.Yes)
                {
                    bool res  =client.Delete_LoaiHinhDaoTao(this.iDataSoure.Copy());
                    if (!res)
                    {
                        CTMessagebox.Show("Xóa không thành công", "Xóa", "", CTICON.Error, CTBUTTON.OK);
                        return;
                    }
                    else
                    {
                        CTMessagebox.Show("Xóa thành công", "Xóa", "", CTICON.Information, CTBUTTON.OK);
                        GetGrid();
                        SetIsNull();
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
                txtMaloai.Focus();
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
                r = ((DataRowView)grd.GetFocusedRow()).Row;
                this.iDataSoure.Rows[0]["ID_LOAIHINH_DTAO"] = r["ID_LOAIHINH_DTAO"];
                this.iDataSoure.Rows[0]["MA_LOAIHINH_DTAO"] = r["MA_LOAIHINH_DTAO"];
                this.iDataSoure.Rows[0]["TEN_LOAIHINH_DTAO"] = r["TEN_LOAIHINH_DTAO"];
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
