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
using DevExpress.Utils;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_CauHinhHocPhi.xaml
    /// </summary>
    public partial class frm_CauHinhHocPhi : Page
    {
        private DataTable iDataSource = null;
        private DataTable iGridDataSource = null;
        private DataTable iGridDataSource_Search = null;
        private DataTable iData_HDT = null;
        private DataTable iData_HKy = null;
        private DataTable iData_NamHoc = null;
        private int SoHky = 0;


        bus_Cauhinhhocphi hp = new bus_Cauhinhhocphi();


        public frm_CauHinhHocPhi()
        {
            InitializeComponent();
            iDataSource = TableChelmabinding();
            this.DataContext = iDataSource;
            Init_Grid();
            iDataSource.Rows[0]["USER"] = UserCommon.UserName;
            Load_cbo();
        }
        private DataTable TableChelmabinding()
        {
            DataTable dtaTable = null;
            try
            {
                Dictionary<string, Type> xDicUser = new Dictionary<string, Type>();
                xDicUser.Add("USER", typeof(string));
                xDicUser.Add("ID_NAMHOC_HIENTAI", typeof(int));
                xDicUser.Add("ID_NAMHOC_HKY_HTAI", typeof(int));
                xDicUser.Add("ID_HE_DAOTAO", typeof(int));
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

                iData_NamHoc = hp.GetAll_Namhoc();
                cboNH.ItemsSource = iData_NamHoc;
                DataTable xdt = hp.GetAll_NhocHientai();
                if (xdt.Rows.Count > 0)
                {
                    iDataSource.Rows[0]["ID_NAMHOC_HIENTAI"] = xdt.Rows[0]["ID_NAMHOC_HIENTAI"];
                }

                #endregion

                #region Load he dao tao

                bus_molophocphan bus = new bus_molophocphan();
                iData_HDT = bus.GetAll_HDT();
                DataRow dr = iData_HDT.NewRow();
                dr["ID_HE_DAOTAO"] = 0;
                dr["TEN_HE_DAOTAO"] = "---------Chọn---------";
                iData_HDT.Rows.InsertAt(dr, 0);
                cboHDT.ItemsSource = iData_HDT;
                iDataSource.Rows[0]["ID_HE_DAOTAO"] = 0;

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

        private void Init_Grid()
        {
            try
            {
                GridColumn xcolumn;

                xcolumn = new GridColumn();
                xcolumn.FieldName = "ID_CAUHINH_HOCPHI";
                xcolumn.Visible = false;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "ID_HE_DAOTAO";
                xcolumn.Visible = false;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "TEN_HE_DAOTAO";
                xcolumn.Header = "Hệ đào tạo";
                xcolumn.AllowCellMerge = true;
                xcolumn.Width = 150;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "HOCKY";
                xcolumn.Header = "Học kỳ";
                xcolumn.AllowCellMerge = true;
                xcolumn.Width = 25;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "KIEUHOC";
                xcolumn.Header = "Loại tín chỉ";
                xcolumn.Width = 150;
                xcolumn.AllowCellMerge = false;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);


                TextEditSettings edt = new TextEditSettings();
                edt.MaskType = MaskType.Numeric;
                edt.Mask = "#,##,##,##,##,##,##,##,##,##,##,##,##,##0 VND;";
                edt.DisplayFormat = "#,##,##,##,##,##,##,##,##,##,##,##,##,##0 VND;";


                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "DON_GIA";
                xcolumn.Header = "Đơn giá ( 1 tín chỉ )";
                xcolumn.Width = 150;
                xcolumn.AllowEditing = DefaultBoolean.True;
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

        private void CboNH_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                #region Load hoc ky

                iData_HKy = hp.GetAll_Namhoc_HKY(Convert.ToInt32(iDataSource.Rows[0]["ID_NAMHOC_HIENTAI"]));
                DataRow xdr = iData_HKy.NewRow();
                xdr["ID_NAMHOC_HKY_HTAI"] = 0;
                xdr["HOCKY"] = "---Chọn---";
                iData_HKy.Rows.InsertAt(xdr, 0);
                cboHK.ItemsSource = iData_HKy;

                #endregion

                #region Load cau hinh hoc phi

                iGridDataSource = hp.GetAll_HP(Convert.ToInt32(iDataSource.Rows[0]["ID_NAMHOC_HIENTAI"]));
                iGridDataSource.Columns.Add("KIEUHOC", typeof(string));
                foreach (DataRow dr in iGridDataSource.Rows)
                {
                    if (Convert.ToInt32(dr["IS_LYTHUYET"]) == 1)
                    {
                        dr["KIEUHOC"] = "Lý thuyết";
                    }
                    else
                    {
                        dr["KIEUHOC"] = "Thực hành";
                    }
                }

                foreach (DataRow dr in iData_HDT.Rows)
                {
                    if (Convert.ToInt32(dr["ID_HE_DAOTAO"]) != 0)
                    {
                        foreach (DataRow drHky in iData_HKy.Rows)
                        {

                            if (Convert.ToInt32(drHky["ID_NAMHOC_HKY_HTAI"]) != 0)
                            {
                                DataRow[] xcheck = (from x in
                                                        iGridDataSource.AsEnumerable()
                                                            .Where(
                                                                d => d.Field<int>("ID_HE_DAOTAO") ==
                                                                     Convert.ToInt32(dr["ID_HE_DAOTAO"])
                                                                     && d.Field<int>("ID_NAMHOC_HKY_HTAI") ==
                                                                     Convert.ToInt32(drHky["ID_NAMHOC_HKY_HTAI"])
                                                            )
                                                    select x).ToArray();
                                if (xcheck.Count() < 1)//Neu chua co trong data thi them vao
                                {
                                    for (int i = 0; i < 2; i++)
                                    {
                                        DataRow drNew = iGridDataSource.NewRow();
                                        drNew["ID_CAUHINH_HOCPHI"] = 0;
                                        drNew["ID_HE_DAOTAO"] = dr["ID_HE_DAOTAO"];
                                        drNew["TEN_HE_DAOTAO"] = dr["TEN_HE_DAOTAO"];
                                        drNew["ID_NAMHOC_HKY_HTAI"] = drHky["ID_NAMHOC_HKY_HTAI"];
                                        drNew["HOCKY"] = drHky["HOCKY"];
                                        drNew["IS_LYTHUYET"] = i;
                                        if (i == 0)
                                        {
                                            drNew["KIEUHOC"] = "Thực hành";
                                        }
                                        else
                                        {
                                            drNew["KIEUHOC"] = "Lý thuyết";
                                        }
                                        drNew["DON_GIA"] = 0;
                                        iGridDataSource.Rows.Add(drNew);
                                    }
                                }
                            }
                        }

                    }
                }

                grd.ItemsSource = iGridDataSource;

                #endregion

                #region Load hoc ky hien tai

                DataTable xdt = hp.GetAll_HKyHientai(Convert.ToInt32(iDataSource.Rows[0]["ID_NAMHOC_HIENTAI"]));
                if (xdt.Rows.Count > 0)
                {
                    iDataSource.Rows[0]["ID_NAMHOC_HKY_HTAI"] = xdt.Rows[0]["ID_NAMHOC_HKY_HTAI"];
                }
                else
                {
                    iDataSource.Rows[0]["ID_NAMHOC_HKY_HTAI"] = 0;
                }
                iDataSource.Rows[0]["ID_HE_DAOTAO"] = 0;
                #endregion
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void CboHK_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(iDataSource.Rows[0]["ID_NAMHOC_HKY_HTAI"]) == 0)
                {
                    iGridDataSource_Search = iGridDataSource.Copy();
                }
                else
                {
                    DataRow[] xcheck = (from x in
                                            iGridDataSource.AsEnumerable()
                                                .Where(
                                                    d =>
                                                        d.Field<int>("ID_NAMHOC_HKY_HTAI") ==
                                                        Convert.ToInt32(iDataSource.Rows[0]["ID_NAMHOC_HKY_HTAI"].ToString())).OrderBy(m => m.Field<int>("HOCKY"))
                                        select x).ToArray();
                    if (xcheck.Count() > 0)
                    {
                        iGridDataSource_Search = xcheck.CopyToDataTable();

                    }
                    else
                    {
                        iGridDataSource_Search = iGridDataSource.Clone();
                    }
                }
                grd.ItemsSource = iGridDataSource_Search;
                iDataSource.Rows[0]["ID_HE_DAOTAO"] = 0;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void CboHDT_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                if (iGridDataSource != null)
                {
                    if (Convert.ToInt32(iDataSource.Rows[0]["ID_HE_DAOTAO"]) == 0)
                    {
                        CboHK_OnEditValueChanged(null, null);
                    }
                    else
                    {
                        if (Convert.ToInt32(iDataSource.Rows[0]["ID_NAMHOC_HKY_HTAI"].ToString()) != 0)
                        {
                            DataRow[] xcheck = (from x in
                                                    iGridDataSource.AsEnumerable()
                                                        .Where(
                                                            d => d.Field<int>("ID_HE_DAOTAO") ==
                                                                 Convert.ToInt32(iDataSource.Rows[0]["ID_HE_DAOTAO"].ToString())
                                                                 && d.Field<int>("ID_NAMHOC_HKY_HTAI") ==
                                                                 Convert.ToInt32(iDataSource.Rows[0]["ID_NAMHOC_HKY_HTAI"].ToString())
                                                        ).OrderBy(m => m.Field<int>("HOCKY"))
                                                select x).ToArray();
                            if (xcheck.Count() > 0)
                            {
                                iGridDataSource_Search = xcheck.CopyToDataTable();

                            }
                        }
                        else
                        {
                            DataRow[] xcheck = (from x in
                                                    iGridDataSource.AsEnumerable()
                                                        .Where(
                                                            d => d.Field<int>("ID_HE_DAOTAO") ==
                                                                 Convert.ToInt32(iDataSource.Rows[0]["ID_HE_DAOTAO"].ToString())).OrderBy(m => m.Field<int>("HOCKY"))
                                                select x).ToArray();
                            if (xcheck.Count() > 0)
                            {
                                iGridDataSource_Search = xcheck.CopyToDataTable();

                            }
                        }
                    }
                    grd.ItemsSource = iGridDataSource_Search;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void BtnRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            iDataSource.Rows[0]["ID_NAMHOC_HIENTAI"] = 0;
            iDataSource.Rows[0]["ID_NAMHOC_HKY_HTAI"] = 0;
            iDataSource.Rows[0]["ID_HE_DAOTAO"] = 0;

            Load_cbo();
        }

        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                int i = hp.SaveObject(iGridDataSource_Search, iDataSource.Rows[0]["USER"].ToString());
                if (i > 0)
                {
                    CTMessagebox.Show("Thành công", "Lưu", "", CTICON.Information, CTBUTTON.OK);
                }
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lưu", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
        }
    }
}
