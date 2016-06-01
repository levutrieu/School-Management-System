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
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "MA_LOAIHINH_DTAO";
            col.Header = "Mã loại";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
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
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
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
                dic.Add("TRANGTHAI", typeof(string));
                dic.Add("GHICHU", typeof(string));
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
                this.iDataSoure.Rows[0]["MA_LOAIHINH_DTAO"] = string.Empty;
                this.iDataSoure.Rows[0]["TEN_LOAIHINH_DTAO"] = string.Empty;
                this.iDataSoure.Rows[0]["TRANGTHAI"] = string.Empty;
                this.iDataSoure.Rows[0]["GHICHU"] = string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool ValiDate()
        {
            try
            {
                if (this.iDataSoure.Rows[0]["MA_LOAIHINH_DTAO"].ToString() == string.Empty)
                {
                    MessageBox.Show("Vui lòng nhập");
                    txtMaloai.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["TEN_LOAIHINH_DTAO"].ToString() == string.Empty)
                {
                    MessageBox.Show("Vui lòng nhập");
                    txtTenloai.Focus();
                    return false;
                }
                return true;
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
                if (ValiDate())
                {
                    if (flagsave)
                    {
                        bool res = client.Insert_LoaiHinhDaoTao(this.iDataSoure.Copy());
                        if (!res)
                        {
                            MessageBox.Show("Thêm mới không thành công", "Thêm mới");
                        }
                        GetGrid();
                        SetIsNull();
                    }
                    else
                    {
                        client.Update_LoaiHinhDaoTao(this.iDataSoure.Copy());
                        GetGrid();
                        SetIsNull();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void btnDelete_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Bạn có muốn xóa không?", "Xóa", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    client.Delete_LoaiHinhDaoTao(this.iDataSoure.Copy());
                    GetGrid();
                    SetIsNull();
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
                GetGrid();
                SetIsNull();
                txtMaloai.Focus();
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
                    this.iDataSoure.Rows[0]["TRANGTHAI"] = r["TRANGTHAI"];
                    this.iDataSoure.Rows[0]["GHICHU"] = r["GHICHU"];

                    flagsave = false;
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
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }
    }
}
