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
using DevExpress.Mvvm.UI;
using DevExpress.Utils;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_LoaiMonHoc.xaml
    /// </summary>
    public partial class frm_LoaiMonHoc : Page
    {
        bus_loaimonhoc bus =new bus_loaimonhoc();
        private DataTable iDataSource = null;
        private DataTable iGridDataSource = null;

        public frm_LoaiMonHoc()
        {
            InitializeComponent();
            this.iDataSource = TableChelmabinding();
            this.DataContext = this.iDataSource;
            this.iDataSource.Rows[0]["USER"] = UserCommon.UserName;
            this.iDataSource.Rows[0]["ID_LOAI"] = 0;
            Initialize_Grid();
            Load_data();
        }

        private DataTable TableChelmabinding()
        {
            DataTable dtaTable = null;
            try
            {
                Dictionary<string, Type> xDicUser = new Dictionary<string, Type>();
                xDicUser.Add("USER", typeof(string));
                xDicUser.Add("MA_LOAI", typeof(string));
                xDicUser.Add("TENLOAI", typeof(string));
                xDicUser.Add("TRANGTHAI", typeof(decimal));
                xDicUser.Add("ID_LOAI", typeof(decimal));
                dtaTable = TableUtil.ConvertToTable(xDicUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtaTable;
        }

        private void Initialize_Grid()
        {
            try
            {
                GridColumn col = null;

                col = new GridColumn();
                col.FieldName = "ID_LOAI_MONHOC";
                col.Header = "Mã loại môn học";
                col.AllowEditing = DefaultBoolean.False;
                col.Width = 100;
                col.Visible = false;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "MA_LOAI_MONHOC";
                col.Header = "Mã loại môn học";
                col.AllowEditing = DefaultBoolean.False;
                col.Width = 100;
                col.Visible = true;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TEN_LOAI_MONHOC";
                col.Header = "Tên loại môn học";
                col.AllowEditing = DefaultBoolean.False;
                col.Width = 100;
                col.Visible = true;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TRANGTHAI";
                col.Header = "Trạng thái";
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Binding = new Binding("TRANGTHAI") { Converter = new StringToBooleanConverter(), ConverterParameter = "0:1", Mode = BindingMode.TwoWay };
                col.Visible = true;
                col.EditSettings = new CheckEditSettings();
                col.Width = 50;
                grd.Columns.Add(col);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(this.iDataSource.Rows[0]["ID_LOAI"]) == 0)
                {
                    int xrt = bus.InsertObject(iDataSource);

                    if (xrt != 0)
                    {
                        CTMessagebox.Show("Thành công", "Thêm mới", "", CTICON.Information,
                        CTBUTTON.OK);
                    }
                }
                else
                {
                    int xrt = bus.UpdateObject(iDataSource);

                    if (xrt != 0)
                    {
                        CTMessagebox.Show("Thành công", "Sửa", "", CTICON.Information,
                        CTBUTTON.OK);
                    }
                }
                Load_data();
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lưu", ex.Message, CTICON.Information,
                        CTBUTTON.OK);
            }
        }

        void Load_data()
        {
            iGridDataSource = bus.GetAll();
            grd.ItemsSource = iGridDataSource;
        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            this.iDataSource.Rows[0]["MA_LOAI"] = "";
            this.iDataSource.Rows[0]["TENLOAI"] = "";
            this.iDataSource.Rows[0]["TRANGTHAI"] = 0;
            this.iDataSource.Rows[0]["ID_LOAI"] = 0;
            Txtmaloai.Focus();
        }

        private void BtnDelete_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(this.iDataSource.Rows[0]["ID_LOAI"]) != 0)
                {
                    int xrt = bus.DeleteObject(Convert.ToInt32(this.iDataSource.Rows[0]["ID_LOAI"]), this.iDataSource.Rows[0]["USER"].ToString());
                    if (xrt != 0)
                    {
                        CTMessagebox.Show("Thành công", "Xóa", "", CTICON.Information,
                        CTBUTTON.OK);
                    }
                    Load_data();
                }
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Xóa", "", CTICON.Information,
                        CTBUTTON.OK);
            }
        }

        private void BtnRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            Load_data();
        }

        private void GrdView_OnFocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                DataRow RowSelGb = null;
                if (this.grd.GetFocusedRow() == null) return;
                RowSelGb = ((DataRowView)this.grd.GetFocusedRow()).Row;
                this.iDataSource.Rows[0]["ID_LOAI"] = Convert.ToInt32(RowSelGb["ID_LOAI_MONHOC"]);
                this.iDataSource.Rows[0]["MA_LOAI"] = RowSelGb["MA_LOAI_MONHOC"].ToString();
                this.iDataSource.Rows[0]["TENLOAI"] = RowSelGb["TEN_LOAI_MONHOC"].ToString();
                this.iDataSource.Rows[0]["TRANGTHAI"] = Convert.ToInt32(RowSelGb["TRANGTHAI"]);
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
    }
}
