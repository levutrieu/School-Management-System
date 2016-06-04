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
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_bomon.xaml
    /// </summary>
    public partial class frm_bomon : Page
    {
        private DataTable iDataSource = null;
        private DataTable iGridDataSoure = null;
        bus_bomon bus=new bus_bomon();
        public frm_bomon()
        {
            InitializeComponent();
            this.iDataSource = TableChelmabinding();
            this.DataContext = this.iDataSource;
            this.iDataSource.Rows[0]["ID_BOMON"] = 0;
            this.iDataSource.Rows[0]["USER"] = UserCommon.UserName;
            Init_Grid();
            LoadData();
        }
        private DataTable TableChelmabinding()
        {
            DataTable dtaTable = null;
            try
            {
                Dictionary<string, Type> xDicUser = new Dictionary<string, Type>();
                xDicUser.Add("USER", typeof(string));
                xDicUser.Add("ID_BOMON", typeof(int));
                xDicUser.Add("MA_BM", typeof(string));
                xDicUser.Add("TEN_BM", typeof(string));
                dtaTable = TableUtil.ConvertToTable(xDicUser);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return dtaTable;
        }

        private void Init_Grid()
        {
            try
            {
                GridColumn xcolumn;

                xcolumn = new GridColumn();
                xcolumn.FieldName = "ID_BOMON";
                xcolumn.Header = "Mã bộ môn";
                xcolumn.Width = 50;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = false;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "MA_BM";
                xcolumn.Header = "Mã bộ môn";
                xcolumn.Width = 50;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "TEN_BM";
                xcolumn.Header = "Tên bộ môn";
                xcolumn.Width = 100;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void LoadData()
        {
            try
            {
                iGridDataSoure = bus.GetAllBoMon();
                grd.ItemsSource = iGridDataSoure;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GrdView_OnFocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
             try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                DataRow RowSelGb = null;
                if (this.grd.GetFocusedRow() == null) return;
                RowSelGb = ((DataRowView) this.grd.GetFocusedRow()).Row;
                this.iDataSource.Rows[0]["MA_BM"] = RowSelGb["MA_BM"];
                this.iDataSource.Rows[0]["TEN_BM"] = RowSelGb["TEN_BM"];
                this.iDataSource.Rows[0]["ID_BOMON"] = RowSelGb["ID_BOMON"];
            }
             catch (Exception ex)
             {
                 throw ex;
             }
             finally
             {
                 Mouse.OverrideCursor = Cursors.Arrow;
             }
        }

        private void BtnRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void BtnDelete_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(this.iDataSource.Rows[0]["ID_BOMON"]) != 0)
                {
                    int xrt = bus.DeleteObject(Convert.ToInt32(this.iDataSource.Rows[0]["ID_BOMON"]), this.iDataSource.Rows[0]["USER"].ToString());
                    if (xrt != 0)
                    {
                        CTMessagebox.Show("Thành công", "Xóa", "", CTICON.Information,
                        CTBUTTON.OK);
                    }
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Xóa", "", CTICON.Information,
                        CTBUTTON.OK);
            }
        }

        private void BtnAddNew_OnClick(object sender, RoutedEventArgs e)
        {
            this.iDataSource.Rows[0]["MA_BM"] = "";
            this.iDataSource.Rows[0]["TEN_BM"] = "";
            this.iDataSource.Rows[0]["ID_BOMON"] = 0;
        }

        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(this.iDataSource.Rows[0]["ID_BOMON"]) == 0)
                {
                    int xrt = bus.InsertObject(this.iDataSource.Rows[0]["MA_BM"].ToString(),
                        this.iDataSource.Rows[0]["TEN_BM"].ToString(), this.iDataSource.Rows[0]["USER"].ToString());

                    if (xrt != 0)
                    {
                        CTMessagebox.Show("Thành công", "Thêm mới", "", CTICON.Information,
                        CTBUTTON.OK);
                    }
                }
                else
                {
                    int xrt = bus.UpdateObject(Convert.ToInt32(this.iDataSource.Rows[0]["ID_BOMON"]),
                        this.iDataSource.Rows[0]["TEN_BM"].ToString(), this.iDataSource.Rows[0]["USER"].ToString());

                    if (xrt != 0)
                    {
                        CTMessagebox.Show("Thành công", "Sửa", "", CTICON.Information,
                        CTBUTTON.OK);
                    }
                }
                LoadData();
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lưu", ex.Message, CTICON.Information,
                        CTBUTTON.OK);    
            }
        }
    }
}
