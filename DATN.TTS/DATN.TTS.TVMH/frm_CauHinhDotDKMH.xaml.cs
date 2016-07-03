using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_CauHinhDotDKMH.xaml
    /// </summary>
    public partial class frm_CauHinhDotDKMH : Page
    {
        private DataTable iDataSource = null;
        private DataTable iGridDataSource = null;

        bus_Cauhinhhocphi hp = new bus_Cauhinhhocphi();
        bus_molophocphan bus = new bus_molophocphan();

        public frm_CauHinhDotDKMH()
        {
            InitializeComponent();
            iDataSource = TableChelmabinding();
            this.DataContext = iDataSource;
            iDataSource.Rows[0]["USER"] = UserCommon.UserName;
            Init_Grid();
            Load_cbo();
            Load_data();

        }

        private DataTable TableChelmabinding()
        {
            DataTable dtaTable = null;
            try
            {
                Dictionary<string, Type> xDicUser = new Dictionary<string, Type>();
                xDicUser.Add("USER", typeof(string));
                xDicUser.Add("ID_DOTDK", typeof(int));
                xDicUser.Add("ID_NAMHOC_HIENTAI", typeof(int));
                xDicUser.Add("ID_NAMHOC_HKY_HTAI", typeof(int));
                xDicUser.Add("ID_HE_DAOTAO", typeof(int));
                xDicUser.Add("MA_DOT_DK", typeof(string));
                xDicUser.Add("TEN_DOT_DK", typeof(string));
                xDicUser.Add("NGAY_BDAU", typeof(DateTime));
                xDicUser.Add("NGAY_KTHUC", typeof(DateTime));
                dtaTable = TableUtil.ConvertToTable(xDicUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtaTable;
        }

        private void Load_cbo()
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                #region Load nam hoc

                DataTable iData_NamHoc = hp.GetAll_Namhoc();
                cboNH.ItemsSource = iData_NamHoc;

                #endregion

                #region Load he dao tao


                DataTable iData_HDT = bus.GetAll_HDT();
                DataRow dr = iData_HDT.NewRow();
                dr["ID_HE_DAOTAO"] = 0;
                dr["TEN_HE_DAOTAO"] = "---------Chọn---------";
                iData_HDT.Rows.InsertAt(dr, 0);
                cboHDT.ItemsSource = iData_HDT;

                #endregion
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

        private void Load_data()
        {
            try
            {
                iGridDataSource = hp.GetAll_DOT_DK();

                grd.ItemsSource = iGridDataSource;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void Init_Grid()
        {
            try
            {
                GridColumn xcolumn;

                xcolumn = new GridColumn();
                xcolumn.FieldName = "ID_DOTDK";
                xcolumn.Visible = false;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "ID_NAMHOC_HKY_HTAI";
                xcolumn.Visible = false;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "ID_NAMHOC_HIENTAI";
                xcolumn.Visible = false;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "ID_HE_DAOTAO";
                xcolumn.Visible = false;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "MA_DOT_DK";
                xcolumn.Visible = false;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "NAMHOC";
                xcolumn.Header = "Năm học";
                xcolumn.AllowCellMerge = true;
                xcolumn.Width = 90;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "HOCKY";
                xcolumn.Header = "Học kỳ";
                xcolumn.AllowCellMerge = true;
                xcolumn.Width = 50;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "TEN_DOT_DK";
                xcolumn.Header = "Tên đợt ĐK";
                xcolumn.Width = 150;
                xcolumn.AllowCellMerge = false;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);


                TextEditSettings edt = new TextEditSettings();
                edt.MaskType = MaskType.DateTime;
                edt.Mask = "dd/MM/yyyy hh:mm";
                edt.DisplayFormat = "dd/MM/yyyy hh:mm tt";


                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "NGAY_BDAU";
                xcolumn.Header = "Ngày giờ bắt đầu";
                xcolumn.Width = 150;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.AllowCellMerge = false;
                xcolumn.EditSettings = edt;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "NGAY_KTHUC";
                xcolumn.Header = "Ngày giờ kết thúc";
                xcolumn.Width = 150;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.AllowCellMerge = false;
                xcolumn.EditSettings = edt;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                grdViewNDung.AutoWidth = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void BtnNew_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                iDataSource.Rows[0]["ID_DOTDK"] = 0;
                iDataSource.Rows[0]["ID_HE_DAOTAO"] = 0;
                iDataSource.Rows[0]["MA_DOT_DK"] = "";
                iDataSource.Rows[0]["TEN_DOT_DK"] = "";
                iDataSource.Rows[0]["NGAY_BDAU"] = DBNull.Value;
                iDataSource.Rows[0]["NGAY_KTHUC"] = DBNull.Value;
                iDataSource.Rows[0]["ID_NAMHOC_HIENTAI"] = 0;
                iDataSource.Rows[0]["ID_NAMHOC_HKY_HTAI"] = 0;
                cboNH.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CboNH_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                #region Load hoc ky

                if (iDataSource.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(iDataSource.Rows[0]["ID_NAMHOC_HIENTAI"].ToString()))
                    {
                        DataTable iData_HKy =
                            hp.GetAll_Namhoc_HKY(Convert.ToInt32(iDataSource.Rows[0]["ID_NAMHOC_HIENTAI"]));
                        DataRow xdr = iData_HKy.NewRow();
                        xdr["ID_NAMHOC_HKY_HTAI"] = 0;
                        xdr["HOCKY"] = "---Chọn---";
                        iData_HKy.Rows.InsertAt(xdr, 0);
                        cboHK.ItemsSource = iData_HKy;
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrdViewNDung_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DataRow RowSelGb = null;
                if (this.grd.GetFocusedRow() == null) return;
                RowSelGb = ((DataRowView)this.grd.GetFocusedRow()).Row;

                iDataSource.Rows[0]["ID_DOTDK"] = RowSelGb["ID_DOTDK"];
                iDataSource.Rows[0]["ID_HE_DAOTAO"] = RowSelGb["ID_HE_DAOTAO"];
                iDataSource.Rows[0]["MA_DOT_DK"] = RowSelGb["MA_DOT_DK"];
                iDataSource.Rows[0]["TEN_DOT_DK"] = RowSelGb["TEN_DOT_DK"];
                iDataSource.Rows[0]["NGAY_BDAU"] = RowSelGb["NGAY_BDAU"];
                iDataSource.Rows[0]["NGAY_KTHUC"] = RowSelGb["NGAY_KTHUC"];
                iDataSource.Rows[0]["ID_NAMHOC_HIENTAI"] = RowSelGb["ID_NAMHOC_HIENTAI"];
                iDataSource.Rows[0]["ID_NAMHOC_HKY_HTAI"] = RowSelGb["ID_NAMHOC_HKY_HTAI"];
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
                if (!string.IsNullOrEmpty(iDataSource.Rows[0]["ID_DOTDK"].ToString()))
                {
                    if (Convert.ToInt32(iDataSource.Rows[0]["ID_DOTDK"]) == 0)
                    {
                        #region Insert

                        int i = 0;
                        i=hp.InsertObject(iDataSource);
                        if (i != 0)
                        {
                            CTMessagebox.Show("Thành công", "Lưu", "", CTICON.Information, CTBUTTON.OK);
                        }

                        #endregion
                    }
                    else
                    {
                        #region Update

                        int i = 0;
                        i = hp.UpdateObject(iDataSource);
                        if (i != 0)
                        {
                            CTMessagebox.Show("Thành công", "Lưu", "", CTICON.Information, CTBUTTON.OK);
                        }

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lưu", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
        }
    }
}
