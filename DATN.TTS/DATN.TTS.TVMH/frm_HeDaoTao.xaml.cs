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
                dic.Add("ID_HE_DAOTAO", typeof(Int32));
                dic.Add("ID_BAC_DAOTAO", typeof(Int32));
                dic.Add("ID_LOAIHINH_DTAO", typeof(Int32));
                dic.Add("MA_HE_DAOTAO", typeof(string));
                dic.Add("TEN_HE_DAOTAO", typeof(string));
                dic.Add("SO_NAMHOC", typeof(Decimal));
                dic.Add("TRANGTHAI", typeof(string));
                dic.Add("TEN_BAC_DAOTAO", typeof(string));
                dic.Add("TEN_LOAIHINH_DTAO", typeof(string));
                dic.Add("USER", typeof(string));
                dt = TableUtil.ConvertToTable(dic);
                return dt;
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
            col.FieldName = "ID_HE_DAOTAO";
            col.Header = string.Empty;
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "ID_BAC_DAOTAO";
            col.Header = string.Empty;
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            grd.Columns.Add(col);

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
            col.FieldName = "MA_HE_DAOTAO";
            col.Header = "Mã hệ đào tạo";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
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
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "SO_NAMHOC";
            col.Header = "Số năm học";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
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
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
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
                cbbbacdt.ItemsSource = client.GetAllBacDaoTao();
                cbbloaihinhdt.ItemsSource = client.GetAllLoaiHinhDaoTao();
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
                //this.iDataSoure.Rows[0]["ID_HE_DAOTAO"] = "0";
                this.iDataSoure.Rows[0]["ID_BAC_DAOTAO"] = "0";
                this.iDataSoure.Rows[0]["ID_LOAIHINH_DTAO"] = "0";
                this.iDataSoure.Rows[0]["MA_HE_DAOTAO"] = string.Empty;
                this.iDataSoure.Rows[0]["TEN_HE_DAOTAO"] = string.Empty;
                this.iDataSoure.Rows[0]["SO_NAMHOC"] = 0;
                this.iDataSoure.Rows[0]["TRANGTHAI"] = string.Empty;
                //this.iDataSoure.Rows[0]["TEN_LOAIHINH_DTAO"] = string.Empty;

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private bool ValiDate()
        {
            if (this.iDataSoure.Rows[0]["ID_BAC_DAOTAO"].ToString() == "0")
            {
                MessageBox.Show("Vui lòng chọn bậc đào tạo");
                cbbbacdt.Focus();
                return false;
            }
            if (this.iDataSoure.Rows[0]["ID_LOAIHINH_DTAO"].ToString() == "0")
            {
                MessageBox.Show("Vui lòng chọn loại hình đào tạo");
                cbbloaihinhdt.Focus();
                return false;
            }
            if (this.iDataSoure.Rows[0]["MA_HE_DAOTAO"].ToString() == string.Empty)
            {
                MessageBox.Show("Vui lòng nhập mã hệ đào tạo");
                txtMaHDT.Focus();
                return false;
            }
            if (this.iDataSoure.Rows[0]["TEN_HE_DAOTAO"].ToString() == string.Empty)
            {
                MessageBox.Show("Vui lòng nhập tên hệ đào tạo");
                txtTenHDT.Focus();
                return false;
            }
            if (this.iDataSoure.Rows[0]["SO_NAMHOC"].ToString() == string.Empty)
            {
                MessageBox.Show("Vui lòng nhập số năm học");
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
                this.iDataSoure.Rows[0]["TRANGTHAI"] = row["TRANGTHAI"];
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

        private void btnAddNew_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                flagsave = true;
                SetIsNull();
                txtMaHDT.Focus();
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
                        if (client.KiemTrungMa(this.iDataSoure.Rows[0]["MA_HE_DAOTAO"].ToString()))
                        {
                            bool res = client.Insert_HeDaoTao(this.iDataSoure.Copy());
                            if (!res)
                            {
                                MessageBox.Show("Thêm mới không thành công", "Thêm mới");
                                return;
                            }
                            SetIsNull();
                            GetGrid();
                        }
                        else
                        {
                            MessageBox.Show("Mã bị trùng. Vui lòng nhập lại", "Thông báo");
                            txtMaHDT.Focus();
                        }
                    }
                    else
                    {
                        client.Update_HeDaoTao(this.iDataSoure.Copy());
                        SetIsNull();
                        GetGrid();
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
                GetGrid();
                SetIsNull();
                txtMaHDT.Focus();
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
                if (MessageBox.Show("Bạn có muốn xóa", "Xóa", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    client.Delete_HeDaoTao(int.Parse(this.iDataSoure.Rows[0]["ID_HE_DAOTAO"].ToString()), this.iDataSoure.Rows[0]["USER"].ToString());
                    GetGrid();
                    SetIsNull();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Xóa Không thành công", "Xóa", MessageBoxButton.OKCancel);
            }
        }
    }
}
