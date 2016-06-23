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
    /// Interaction logic for frm_Khoa.xaml
    /// </summary>
    public partial class frm_Khoa : Page
    {
        bus_Khoa client = new bus_Khoa();
        private DataTable iDataSoure = null;
        private DataTable iGridDataSoure = null;
        private bool flagsave = true;
        public frm_Khoa()
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
            col.FieldName = "ID_KHOA";
            col.Header = string.Empty;
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "MA_KHOA";
            col.Header = "Mã khoa";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.EditSettings = new TextEditSettings();
            col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "TEN_KHOA";
            col.Header = "Tên khoa";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "DIENTHOAI";
            col.Header = "Điện thoại";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "EMAIL";
            col.Header = "Email";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "GHICHU";
            col.Header = "Ghi chú";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            grd.Columns.Add(col);
            GetGrid();
        }

        private void GetGrid()
        {
            this.iGridDataSoure = client.GetAllKhoa();
            grd.ItemsSource = iGridDataSoure;
        }

        private DataTable TableSchemaBinding()
        {
            try
            {
                DataTable dt = null;
                Dictionary<string, Type> dic = new Dictionary<string, Type>();
                dic.Add("ID_KHOA", typeof(Decimal));
                dic.Add("MA_KHOA", typeof(string));
                dic.Add("TEN_KHOA", typeof(string));
                dic.Add("DIENTHOAI", typeof(string));
                dic.Add("EMAIL", typeof(string));
                dic.Add("GHICHU", typeof(string));
                dic.Add("USER", typeof(string));
                dt = TableUtil.ConvertToTable(dic);
                return dt;
            }
            catch (Exception err)
            {
                
                throw err;
            }
        }

        private void SetIsNull()
        {
            try
            {
                this.iDataSoure.Rows[0]["MA_KHOA"] = string.Empty;
                this.iDataSoure.Rows[0]["TEN_KHOA"] = string.Empty;
                this.iDataSoure.Rows[0]["DIENTHOAI"] = string.Empty;
                this.iDataSoure.Rows[0]["EMAIL"] = string.Empty;
                this.iDataSoure.Rows[0]["GHICHU"] = string.Empty;
            }
            catch (Exception err)
            {

                throw err;
            }
        }

        private bool ValiDate()
        {
            try
            {
                if (this.iDataSoure.Rows[0]["MA_KHOA"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập mã khoa","Thông báo","", CTICON.Information, CTBUTTON.OK);
                    txtMaKhoa.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["TEN_KHOA"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập tên khoa", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                    txtTenKhoa.Focus();
                    return false;
                }
                return true;
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
                if(this.grd.GetFocusedRow() == null)
                    return;
                r = ((DataRowView) grd.GetFocusedRow()).Row;
                this.iDataSoure.Rows[0]["ID_KHOA"] = r["ID_KHOA"];
                this.iDataSoure.Rows[0]["MA_KHOA"] = r["MA_KHOA"];
                this.iDataSoure.Rows[0]["TEN_KHOA"] = r["TEN_KHOA"];
                this.iDataSoure.Rows[0]["DIENTHOAI"] = r["DIENTHOAI"];
                this.iDataSoure.Rows[0]["EMAIL"] = r["EMAIL"];
                this.iDataSoure.Rows[0]["GHICHU"] = r["GHICHU"];

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
                GetGrid();
                SetIsNull();
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
                if (ValiDate())
                {
                    if (flagsave)
                    {
                        bool res = client.Insert_Khoa(this.iDataSoure.Copy());
                        if (!res)
                        {
                            CTMessagebox.Show("Thêm mới không thành công", "Thêm mới", "", CTICON.Information,CTBUTTON.OK);
                        }
                        GetGrid();
                        SetIsNull();
                    }
                    else
                    {
                        client.Update_Khoa(this.iDataSoure.Copy());
                        GetGrid();
                        SetIsNull();
                    }
                }
            }
            catch(Exception err)
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
                if (CTMessagebox.Show("Bạn có muốn xóa không?", "Xóa","",CTICON.Information, CTBUTTON.YesNo) == CTRESPONSE.Yes)
                {
                    client.Delete_Khoa(this.iDataSoure.Copy());
                    GetGrid();
                    SetIsNull();
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
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                GetGrid();
                SetIsNull();
                txtMaKhoa.Focus();
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
    }
}
