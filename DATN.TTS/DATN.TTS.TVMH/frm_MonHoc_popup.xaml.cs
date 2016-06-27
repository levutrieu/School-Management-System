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
                if (string.IsNullOrEmpty(dtaTable.Rows[0]["ID_MONHOC"].ToString()))
                {
                    dtaTable.Rows[0]["SOTIET"] = 1;
                    dtaTable.Rows[0]["SO_TC"] = 1;
                    dtaTable.Rows[0]["IS_THUCHANH"] = 0;
                    dtaTable.Rows[0]["IS_LYTHUYET"] = 1;
                    dtaTable.Rows[0]["IS_TINHDIEM"] = 1;
                    dtaTable.Rows[0]["ISBATBUOC"] = 1;
                    dtaTable.Rows[0]["IS_THUHOCPHI"] = 1;
                    dtaTable.Rows[0]["TRANGTHAI"] = 1;
                }
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
                if (string.IsNullOrEmpty(iDataSource.Rows[0]["ID_BOMON"].ToString()))
                {
                    iDataSource.Rows[0]["ID_BOMON"] = 0;
                }
                //bus_loaimonhoc lmh = new bus_loaimonhoc();
                //DataTable xdtlmh = lmh.GetAll();
                //cboLoaimonhoc.ItemsSource = xdtlmh;
                //if (string.IsNullOrEmpty(iDataSource.Rows[0]["ID_LOAI_MONHOC"].ToString()))
                //{
                //    iDataSource.Rows[0]["ID_LOAI_MONHOC"] = 0;
                //}
                
                if (string.IsNullOrEmpty(iDataSource.Rows[0]["ID_MONHOC_SONGHANH"].ToString()))
                {
                    iDataSource.Rows[0]["ID_MONHOC_SONGHANH"] = 0;
                }

                #region Load He dao tao

                bus_molophocphan bus = new bus_molophocphan();
                DataTable xdt_hdt = bus.GetAll_HDT();
                cbohedaotao.ItemsSource = xdt_hdt;
                cbohedaotao.ItemsSource = xdt_hdt;

                #endregion

                #region Load cach tinh diem

                DataTable dt = new DataTable();
                dt.Columns.Add("CACHTINH", typeof(string));
                dt.Columns.Add("CACHTINH_NAME", typeof(string));

                DataRow dr = null;
                dr = dt.NewRow();
                dr["CACHTINH"] = "0";
                dr["CACHTINH_NAME"] = "-----Chọn-----";
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["CACHTINH"] = "20-30-50";
                dr["CACHTINH_NAME"] = "20% - 30% - 50%";
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["CACHTINH"] = "30-70";
                dr["CACHTINH_NAME"] = "30% - 70%";
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["CACHTINH"] = "100";
                dr["CACHTINH_NAME"] = "100%";
                dt.Rows.Add(dr);
                dt.AcceptChanges();

                cboCachTinhDiem.ItemsSource = dt;

                #endregion
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

        private void Cbohedaotao_OnSelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(iDataSource.Rows[0]["ID_HE_DAOTAO"].ToString()))
                {
                    bus_MonHoc mh = new bus_MonHoc();
                    DataTable xdtmh = mh.GetAllMonHoc_ByHDT(Convert.ToInt32(iDataSource.Rows[0]["ID_HE_DAOTAO"]));
                    cboMHsonghanh.ItemsSource = xdtmh;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
