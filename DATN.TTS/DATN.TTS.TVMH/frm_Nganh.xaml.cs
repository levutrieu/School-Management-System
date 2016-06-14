using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
using DevExpress.XtraPrinting;
using Page = System.Windows.Controls.Page;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_Nganh.xaml
    /// </summary>
    public partial class frm_Nganh : Page
    {
        private bus_Nganh client = new bus_Nganh();
        private DataTable iDataSoure = null;
        private DataTable iGridDataSoure = null;
        private bool flagsave = true;

        public frm_Nganh()
        {
            InitializeComponent();
            this.iDataSoure = TableSchemaBinding();
            this.DataContext = iDataSoure;

            this.iDataSoure.Rows[0]["USER"] = UserCommon.UserName;
            SetComboBox();
            InitGrid();
        }

        private void SetComboBox()
        {
            //cbbHDT.ItemsSource = client.GetAllHDT();
            cbbKhoa.ItemsSource = client.GetAllKhoa();
        }

        private DataTable TableSchemaBinding()
        {
            try
            {
                DataTable dt = null;
                Dictionary<string, Type> dic = new Dictionary<string, Type>();
                dic.Add("ID_NGANH", typeof (Decimal));
                dic.Add("ID_KHOA", typeof (int));
                dic.Add("ID_HE_DAOTAO", typeof(int));
                dic.Add("MA_NGANH", typeof (string));
                dic.Add("TEN_NGANH", typeof (string));
                dic.Add("KYHIEU", typeof (string));
                dic.Add("GHICHU", typeof (string));
                dic.Add("TRANGTHAI", typeof (string));
                dic.Add("CAP_NGANH", typeof (string));
                dic.Add("USER", typeof (string));
                dic.Add("TEN_KHOA", typeof (string));
                dic.Add("TEN_HE_DAOTAO", typeof (string));
                dt = TableUtil.ConvertToTable(dic);
                return dt;
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
                if (this.iDataSoure.Rows[0]["ID_KHOA"].ToString() == "0")
                {
                    CTMessagebox.Show("Vui lòng chọn khoa");
                    txtMaNganh.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["MA_NGANH"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập mã ngành");
                    txtMaNganh.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["TEN_NGANH"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập tên ngành");
                    txtMaNganh.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["KYHIEU"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập ký hiệu");
                    txtMaNganh.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["CAP_NGANH"].ToString() == string.Empty)
                {
                    CTMessagebox .Show("Vui lòng nhập cấp ngành");
                    txtMaNganh.Focus();
                    return false;
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void SetNull()
        {
            try
            {
                this.iDataSoure.Rows[0]["ID_KHOA"] = "0";
                this.iDataSoure.Rows[0]["MA_NGANH"] = string.Empty;
                this.iDataSoure.Rows[0]["TEN_NGANH"] = string.Empty;
                this.iDataSoure.Rows[0]["KYHIEU"] = string.Empty;
                //this.iDataSoure.Rows[0]["ID_HE_DAOTAO"] = "0";
                this.iDataSoure.Rows[0]["GHICHU"] = string.Empty;
                this.iDataSoure.Rows[0]["TRANGTHAI"] = string.Empty;
                this.iDataSoure.Rows[0]["CAP_NGANH"] = string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void InitGrid()
        {
            GridColumn col;

            col = new GridColumn();
            col.FieldName = "ID_NGANH";
            col.Header = string.Empty;
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "ID_KHOA";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "TEN_KHOA";
            col.Header = "Khoa";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "KYHIEU";
            col.Header = "Ký hiệu";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.EditSettings = new TextEditSettings();
            col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "MA_NGANH";
            col.Header = "Mã Ngành";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.EditSettings = new TextEditSettings();
            col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "TEN_NGANH";
            col.Header = "Ngành";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "CAP_NGANH";
            col.Header = "Cấp ngành";
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
           
            grdViewNDung.AutoWidth = true;
            LoadGrid();
        }

        void LoadGrid()
        {
            try
            {
                this.iGridDataSoure = client.GetAllNganh();
                this.grd.ItemsSource = this.iGridDataSoure;
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
                LoadGrid();
                SetNull();
                txtMaNganh.Focus();
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
                        bool res = client.Insert_Nganh(this.iDataSoure.Copy());
                        if (!res)
                        {
                            CTMessagebox.Show("Thêm mới không thành công", "Thêm mới", "", CTICON.Error, CTBUTTON.YesNo);
                        }
                        LoadGrid();
                        SetNull();
                        txtMaNganh.Focus();
                    }
                    else
                    {
                        client.Update_Nganh(this.iDataSoure.Copy());
                        LoadGrid();
                        SetNull();
                        txtMaNganh.Focus();
                    }
                }
            }
            catch (Exception)
            {
                CTMessagebox.Show("Lỗi", "Lưu", "", CTICON.Information, CTBUTTON.YesNo);
            }
        }

        private void btnDelete_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if( CTMessagebox.Show("Lỗi", "Xóa", "", CTICON.Information, CTBUTTON.YesNo) == CTRESPONSE.Yes)
                {
                    client.Delete_Nganh(this.iDataSoure.Copy());
                    LoadGrid();
                    SetNull();
                    txtMaNganh.Focus();
                }
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        private void btnRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadGrid();
                SetNull();
                txtMaNganh.Focus();
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
                if (this.grd.GetFocusedRow() == null) return;
                r = ((DataRowView) this.grd.GetFocusedRow()).Row;
                this.iDataSoure.Rows[0]["ID_NGANH"] = r["ID_NGANH"];
                this.iDataSoure.Rows[0]["ID_KHOA"] = r["ID_KHOA"];
                this.iDataSoure.Rows[0]["MA_NGANH"] = r["MA_NGANH"];
                this.iDataSoure.Rows[0]["TEN_NGANH"] = r["TEN_NGANH"];
                this.iDataSoure.Rows[0]["KYHIEU"] = r["KYHIEU"];
                this.iDataSoure.Rows[0]["GHICHU"] = r["GHICHU"];
                this.iDataSoure.Rows[0]["TRANGTHAI"] = r["TRANGTHAI"];
                this.iDataSoure.Rows[0]["CAP_NGANH"] = r["CAP_NGANH"];
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
