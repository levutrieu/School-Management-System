using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using DevExpress.Xpf.Grid.GroupRowLayout;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_MonHoc.xaml
    /// </summary>
    public partial class frm_MonHoc : Page
    {
        bus_MonHoc bus = new bus_MonHoc();

        public static DataTable idatasource = null;
        private DataTable iGridDataSource = null;

        private DataTable iData = null;

        public frm_MonHoc()
        {
            InitializeComponent();
            this.iData = TableChelmabinding();
            this.iData.Rows[0]["USER"] = UserCommon.IdNhanVien.ToString();
            InitGrid();
            Load_data();
        }

        private DataTable TableChelmabinding()
        {
            DataTable dtaTable = null;
            try
            {
                Dictionary<string, Type> xDicUser = new Dictionary<string, Type>();
                xDicUser.Add("USER", typeof(string));
                xDicUser.Add("ID_MONHOC", typeof(int));
                dtaTable = TableUtil.ConvertToTable(xDicUser);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return dtaTable;
        }

        private void InitGrid()
        {
            try
            {
                GridColumn col = null;

                col = new GridColumn();
                col.FieldName = "ID_MONHOC";
                col.Visible = false;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "MA_MONHOC";
                col.Header = "Mã môn học";
                col.AllowCellMerge = false;
                col.AllowEditing = DefaultBoolean.False;
                col.Width = 90;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TEN_MONHOC";
                col.Header = "Tên môn học";
                col.AllowCellMerge = false;
                col.AllowEditing = DefaultBoolean.False;
                col.Width =150;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "SO_TC";
                col.Header = "Số tín chỉ";
                col.AllowCellMerge = false;
                col.AllowEditing = DefaultBoolean.False;
                col.Width = 50;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TEN_LOAIMH";
                col.Header = "Loại môn học";
                col.AllowCellMerge = false;
                col.AllowEditing = DefaultBoolean.False;
                col.Width = 90;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TEN_BOMON";
                col.Header = "Bộ môn";
                col.AllowCellMerge = false;
                col.AllowEditing = DefaultBoolean.False;
                col.Width = 90;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TEN_MONHOC_TQ";
                col.Header = "Môn học tiên quyết";
                col.AllowCellMerge = false;
                col.Visible = false;
                col.AllowEditing = DefaultBoolean.False;
                col.Width = 90;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "GHICHU";
                col.Header = "Ghi chú";
                col.AllowCellMerge = false;
                col.AllowEditing = DefaultBoolean.False;
                col.Width = 150;
                grd.Columns.Add(col);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Load_data()
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                DataTable xdt = null;
                xdt = bus.GetAllMonHoc();
                grd.ItemsSource = xdt;
                this.iGridDataSource = xdt;
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

        private void GrdView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DataRow RowSelGb = null;
                if (this.grd.GetFocusedRow() == null) return;
                RowSelGb = ((DataRowView)this.grd.GetFocusedRow()).Row;
                idatasource = iGridDataSource.Clone();
                idatasource.ImportRow(RowSelGb);
                frm_MonHoc_popup popup = new frm_MonHoc_popup();
                popup.ShowDialog();
                Load_data();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                idatasource = iGridDataSource.Clone();
                idatasource.Rows.Add(idatasource.NewRow());
                frm_MonHoc_popup popup = new frm_MonHoc_popup();
                popup.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Btnrefresh_OnClick(object sender, RoutedEventArgs e)
        {
            Load_data();
        }

        private void Btndelete_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (!string.IsNullOrEmpty(this.iData.Rows[0]["ID_MONHOC"].ToString()))
                {
                    if (CTMessagebox.Show("Bạn muốn xóa môn học này ?", "Xóa", "",
                        CTICON.Question,
                        CTBUTTON.YesNo) == CTRESPONSE.Yes)
                    {
                        int i = bus.DeleteObject(Convert.ToInt32(this.iData.Rows[0]["ID_MONHOC"].ToString()),
                            this.iData.Rows[0]["USER"].ToString());
                        if (i != 0)
                        {
                            CTMessagebox.Show("Thành công", "Xóa", "", CTICON.Information, CTBUTTON.OK);
                            Load_data();
                        }
                    }
                }

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

        private void GrdView_OnFocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                DataRow RowSelGb = null;
                if (this.grd.GetFocusedRow() == null) return;
                RowSelGb = ((DataRowView) this.grd.GetFocusedRow()).Row;
                this.iData.Rows[0]["ID_MONHOC"] = RowSelGb["ID_MONHOC"];
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
    }
}
