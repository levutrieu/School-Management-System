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
using System.Windows.Shapes;
using CustomMessage;
using DATN.TTS.BUS;
using DATN.TTS.BUS.Resource;
using DevExpress.XtraEditors;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_MonHoc_popup.xaml
    /// </summary>
    public partial class frm_MonHoc_popup : Window
    {
        bus_MonHoc bus=new bus_MonHoc();

        private DataTable iDataSource = null;

        public frm_MonHoc_popup()
        {
            InitializeComponent();
            this.iDataSource = TableChelmabinding();
            this.DataContext = this.iDataSource;
            this.iDataSource.Rows[0]["USER"] = UserCommon.UserName;
            Load_combo();
        }

        private DataTable TableChelmabinding()
        {
            DataTable dtaTable = null;
            try
            {
                dtaTable = frm_MonHoc.idatasource;
                dtaTable.Columns.Add("USER", typeof (string));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtaTable;
        }

        private void Load_combo()
        {
            try
            {
                bus_bomon bm = new bus_bomon();
                DataTable xdtbm = bm.GetAllBoMon();
                cboboMon.ItemsSource = xdtbm;
                //DataTable table = ((DataView) cboboMon.ItemsSource).Table;
                //DataRow row = table.NewRow();
                //row[cboboMon.DisplayMember] = "--Chọn--";
                //row[cboboMon.ValueMember] = 0;
                //table.Rows.InsertAt(row, 0);
                ////table.AcceptChanges();
                if (string.IsNullOrEmpty(iDataSource.Rows[0]["ID_BOMON"].ToString()))
                {
                    iDataSource.Rows[0]["ID_BOMON"] = 0;
                }
                bus_loaimonhoc lmh = new bus_loaimonhoc();
                DataTable xdtlmh = lmh.GetAll();
                cboLoaimonhoc.ItemsSource = xdtlmh;
                if (string.IsNullOrEmpty(iDataSource.Rows[0]["ID_LOAI_MONHOC"].ToString()))
                {
                    iDataSource.Rows[0]["ID_LOAI_MONHOC"] = 0;
                }
                bus_MonHoc mh=new bus_MonHoc();
                DataTable xdtmh = mh.GetAllMonHoc();
                cboMHsonghanh.ItemsSource = xdtmh;
                if (string.IsNullOrEmpty(iDataSource.Rows[0]["ID_MONHOC_SONGHANH"].ToString()))
                {
                    iDataSource.Rows[0]["ID_MONHOC_SONGHANH"] = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Btnsave_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (!string.IsNullOrEmpty(iDataSource.Rows[0]["ID_MONHOC"].ToString()))
                {
                    int i = bus.UpdateObject(iDataSource);
                    if (i != 0)
                    {
                        Mouse.OverrideCursor = Cursors.Arrow;
                        CTMessagebox.Show("Thành công", "Sửa", "", CTICON.Information, CTBUTTON.OK);
                        this.Close();
                    }
                }
                else
                {
                    int i = bus.InsertObject(iDataSource);
                    if (i != 0)
                    {
                        Mouse.OverrideCursor = Cursors.Arrow;
                        CTMessagebox.Show("Thành công", "Thêm mới", "", CTICON.Information, CTBUTTON.OK);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Information, CTBUTTON.OK);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }
    }
}
