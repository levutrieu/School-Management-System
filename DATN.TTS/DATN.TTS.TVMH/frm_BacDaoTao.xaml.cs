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
    /// Interaction logic for frm_BacDaoTao.xaml
    /// </summary>
    public partial class frm_BacDaoTao : Page
    {
        bus_BacDaotao client =new bus_BacDaotao();
        private DataTable iDataSoure = null;
        private DataTable iGridDataSoure = null;
        private bool flagsave = true;
        public frm_BacDaoTao()
        {
            InitializeComponent();
            this.iDataSoure = TableSchemabinding();
            this.DataContext = iDataSoure;

            this.iDataSoure.Rows[0]["USER"] = UserCommon.UserName;
            InitGrid();
        }

        private DataTable TableSchemabinding()
        {
            try
            {
                DataTable dt = null;
                Dictionary<string, Type> dic = new Dictionary<string, Type>();
                dic.Add("ID_BAC_DAOTAO", typeof(Decimal));
                dic.Add("MA_BAC_DAOTAO", typeof(string));
                dic.Add("TEN_BAC_DAOTAO", typeof(string));
                dic.Add("USER",typeof(string));
                dt = TableUtil.ConvertToTable(dic);
                return dt;
            }
            catch (Exception err)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", err.Message, CTICON.Error, CTBUTTON.OK);
            }
            return null;
        }

        private void GetGrid()
        {
            iGridDataSoure = client.GetAllBac();
            grd.ItemsSource = iGridDataSoure;
        }

        private bool ValiDate()
        {
            try
            {
                if (this.iDataSoure.Rows[0]["MA_BAC_DAOTAO"] == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập mã bậc đào tạo", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                    txtMaHDT.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["TEN_BAC_DAOTAO"] == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập tên bậc đào tạo", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                    txtTenHDT.Focus();
                    return false;
                }
                return true;
            }
            catch (Exception err)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", err.Message, CTICON.Error, CTBUTTON.OK);
            }
            return false;
        }

        private void InitGrid()
        {
            GridColumn col;

            col = new GridColumn();
            col.FieldName = "ID_BAC_DAOTAO";
            col.Header = string.Empty;
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "MA_BAC_DAOTAO";
            col.Header = "Mã bậc đào tạo";
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

            GetGrid();
        }

        private void SetIsNull()
        {
            this.iDataSoure.Rows[0]["MA_BAC_DAOTAO"] = string.Empty;
            this.iDataSoure.Rows[0]["TEN_BAC_DAOTAO"] = string.Empty;
        }

        private void btnAddNew_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                txtMaHDT.Focus();
                GetGrid();
                SetIsNull();
                flagsave = true;
            }
            catch (Exception err)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", err.Message, CTICON.Error, CTBUTTON.OK);
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
                        if (client.KiemTratrungMa(this.iDataSoure.Rows[0]["MA_BAC_DAOTAO"].ToString()))
                        {
                            bool res = client.Insert_BacDaotao(this.iDataSoure.Copy());
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
                    }
                    else
                    {
                        bool res  = client.Update_BacDaoTao(this.iDataSoure.Copy());
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
            catch (Exception err)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", err.Message, CTICON.Error, CTBUTTON.OK);
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
                if (CTMessagebox.Show("Bạn có muốn xóa không?", "Xóa","",CTICON.Information, CTBUTTON.OK) == CTRESPONSE.OK)
                {
                    bool res = client.Delete_BacDaoTao(int.Parse(this.iDataSoure.Rows[0]["ID_BAC_DAOTAO"].ToString()), this.iDataSoure.Rows[0]["USER"].ToString());
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
            catch (Exception err)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", err.Message, CTICON.Error, CTBUTTON.OK);
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
                txtMaHDT.Focus();
                SetIsNull();
            }
            catch (Exception err)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", err.Message, CTICON.Error, CTBUTTON.OK);
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
                DataRow row = null;
                if (this.grd.GetFocusedRow() == null)
                    return;
                row = ((DataRowView) this.grd.GetFocusedRow()).Row;
                this.iDataSoure.Rows[0]["ID_BAC_DAOTAO"] = row["ID_BAC_DAOTAO"];
                this.iDataSoure.Rows[0]["MA_BAC_DAOTAO"] = row["MA_BAC_DAOTAO"];
                this.iDataSoure.Rows[0]["TEN_BAC_DAOTAO"] = row["TEN_BAC_DAOTAO"];
                flagsave = false;
            }
            catch (Exception err)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", err.Message, CTICON.Error, CTBUTTON.OK);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }
    }
}
