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
                dic.Add("TRANGTHAI", typeof(string));
                dic.Add("USER",typeof(string));
                dt = TableUtil.ConvertToTable(dic);
                return dt;
            }
            catch (Exception)
            {
                
                throw;
            }
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
                    MessageBox.Show("Vui lòng nhập mã bậc", "Thông báo");
                    txtMaHDT.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["TEN_BAC_DAOTAO"] == string.Empty)
                {
                    MessageBox.Show("Vui lòng nhập tên bậc", "Thông báo");
                    txtTenHDT.Focus();
                    return false;
                }
                return true;
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
            col.FieldName = "ID_BAC_DAOTAO";
            col.Header = string.Empty;
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "MA_BAC_DAOTAO";
            col.Header = "Mã bậc đào tạo";
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

            //col = new GridColumn();
            //col.FieldName = "TRANGTHAI";
            //col.Header = "Trạng thái";
            //col.Width = 50;
            //col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            //col.AllowEditing = DefaultBoolean.False;
            //col.Visible = true;
            //col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            //grd.Columns.Add(col);

            GetGrid();
        }

        private void SetIsNull()
        {
            this.iDataSoure.Rows[0]["MA_BAC_DAOTAO"] = string.Empty;
            this.iDataSoure.Rows[0]["TEN_BAC_DAOTAO"] = string.Empty;
            this.iDataSoure.Rows[0]["TRANGTHAI"] = string.Empty;
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
                        if (client.KiemTratrungMa(this.iDataSoure.Rows[0]["MA_BAC_DAOTAO"].ToString()))
                        {
                            bool res = client.Insert_BacDaotao(this.iDataSoure.Copy());
                            if (!res)
                            {
                                MessageBox.Show("Thêm mới không thành công", "Thêm mới");
                                return;
                            }
                            
                            GetGrid();
                            SetIsNull();
                        }
                    }
                    else
                    {
                        client.Update_BacDaoTao(this.iDataSoure.Copy());
                        GetGrid();
                        SetIsNull();
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
                if (MessageBox.Show("Bạn có muốn xóa", "Xóa", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    client.Delete_BacDaoTao(int.Parse(this.iDataSoure.Rows[0]["ID_BAC_DAOTAO"].ToString()), this.iDataSoure.Rows[0]["USER"].ToString());
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
                txtMaHDT.Focus();
                SetIsNull();
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

                DataRow row = null;
                if (this.grd.GetFocusedRow() == null)
                    return;
                row = ((DataRowView) this.grd.GetFocusedRow()).Row;
                this.iDataSoure.Rows[0]["ID_BAC_DAOTAO"] = row["ID_BAC_DAOTAO"];
                this.iDataSoure.Rows[0]["MA_BAC_DAOTAO"] = row["MA_BAC_DAOTAO"];
                this.iDataSoure.Rows[0]["TEN_BAC_DAOTAO"] = row["TEN_BAC_DAOTAO"];
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
    }
}
