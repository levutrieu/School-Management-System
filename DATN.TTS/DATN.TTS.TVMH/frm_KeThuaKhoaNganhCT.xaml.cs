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
using DATN.TTS.BUS;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.LookUp;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_KeThuaKhoaNganhCT.xaml
    /// </summary>
    public partial class frm_KeThuaKhoaNganhCT : Window
    {
        private DataTable iGridDataSoure = null;
        public DataTable iDataReturn = null;
        private DataTable iDataSoure = null;
        private bus_LapKeHoachDaoTaoKhoa client = new bus_LapKeHoachDaoTaoKhoa();

        public frm_KeThuaKhoaNganhCT()
        {
            InitializeComponent();
            this.iDataSoure = TableSchemaBinding();
            this.DataContext = iDataSoure;
            InitGridKhoaNganhCT();
            SetComboHDT();
            SetComboNganh();
        }

        DataTable TableSchemaBinding()
        {
            try
            {
                DataTable dt = null;
                Dictionary<string, Type> dic = new Dictionary<string, Type>();
                dic.Add("ID_KHOAHOC", typeof(Decimal));
                dic.Add("TEN_KHOAHOC", typeof(string));
                dic.Add("ID_HE_DAOTAO", typeof(Decimal));
                dic.Add("TEN_HE_DAOTAO", typeof(string));
                dic.Add("ID_NGANH", typeof(Decimal));
                dic.Add("TEN_NGANH", typeof(string));
                dt = TableUtil.ConvertToTable(dic);

                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        private void InitGridKhoaNganhCT()
        {
            try
            {
                GridColumn col;
                col = new GridColumn();
                col.FieldName = "CHK";
                col.Header = "Chọn";
                col.Width = 100;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new CheckEditSettings();
                col.UnboundType = UnboundColumnType.Boolean;
                col.EditSettings.HorizontalContentAlignment =
                    DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdKhoaNganhCT.Columns.Add(col);

                #region Properties hide

                col = new GridColumn();
                col.FieldName = "ID_KHOAHOC_NGANH_CTIET";
                col.Header = string.Empty;
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grdKhoaNganhCT.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "ID_MONHOC";
                col.Header = "ID_MONHOC";
                col.Width = 150;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment =
                    DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdKhoaNganhCT.Columns.Add(col);



                col = new GridColumn();
                col.FieldName = "ID_HE_DAOTAO";
                col.Header = "ID_HE_DAOTAO";
                col.Width = 250;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grdKhoaNganhCT.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "ID_KHOAHOC_NGANH";
                col.Header = "ID_KHOAHOC_NGANH";
                col.Width = 80;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment =
                    DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdKhoaNganhCT.Columns.Add(col);

                #endregion

                col = new GridColumn();
                col.FieldName = "TEN_MONHOC";
                col.Header = "Tên môn học";
                col.Width = 250;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grdKhoaNganhCT.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "SO_TC";
                col.Header = "Số TC";
                col.Width = 80;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment =
                    DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdKhoaNganhCT.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "HOCKY";
                col.Header = "Học kỳ";
                col.Width = 80;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment =
                    DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdKhoaNganhCT.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "SOTIET_LT";
                col.Header = "Số tiết LT";
                col.Width = 80;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment =
                    DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdKhoaNganhCT.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "SOTIET_TH";
                col.Header = "Số tiết TH";
                col.Width = 80;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment =
                    DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdKhoaNganhCT.Columns.Add(col);

                col = new GridColumn();
                col.Header = "Môn học trước";
                col.FieldName = "ID_MONHOC_TRUOC";
                col.Width = 130;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;

                LookUpEditSettings cboMonHocTruoc = new LookUpEditSettings();
                col.EditSettings = cboMonHocTruoc;
                cboMonHocTruoc.ItemsSource = client.GetMonHocTruoc();
                cboMonHocTruoc.DisplayMember = "TEN_MONHOC";
                cboMonHocTruoc.ValueMember = "ID_MONHOC";
                col.EditSettings.HorizontalContentAlignment =
                    DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdKhoaNganhCT.Columns.Add(col);

                col = new GridColumn();
                col.Header = "Môn học song hành";
                col.FieldName = "ID_MONHOC_TRUOC";
                col.Width = 150;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;

                LookUpEditSettings cbbMonHocSongHanh = new LookUpEditSettings();
                col.EditSettings = cbbMonHocSongHanh;
                cbbMonHocSongHanh.ItemsSource = client.GetMonHocSongHanh();
                cbbMonHocSongHanh.DisplayMember = "TEN_MONHOC";
                cbbMonHocSongHanh.ValueMember = "ID_MONHOC";

                col.EditSettings.HorizontalContentAlignment =
                    DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdKhoaNganhCT.Columns.Add(col);

                col = new GridColumn();
                col.Header = "Môn học tiên quyết";
                col.FieldName = "MONHOC_TIENQUYET";
                col.Width = 150;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;

                LookUpEditSettings cbbMonHocTienQuyet = new LookUpEditSettings();
                col.EditSettings = cbbMonHocTienQuyet;
                cbbMonHocTienQuyet.ItemsSource = client.GetMonHocTienQuyet();
                cbbMonHocTienQuyet.DisplayMember = "TEN_MONHOC";
                cbbMonHocTienQuyet.ValueMember = "ID_MONHOC";
                col.EditSettings.HorizontalContentAlignment =
                    DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdKhoaNganhCT.Columns.Add(col);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        private void SetComboHDT()
        {
            try
            {
                DataTable iDataCombo = new DataTable();
                iDataCombo.Columns.Add("ID_HE_DAOTAO", typeof (Decimal));
                iDataCombo.Columns.Add("TEN_HE_DAOTAO", typeof (string));
                DataTable dt = client.GetAllHDT();
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow ir = iDataCombo.NewRow();
                    ir["ID_HE_DAOTAO"] = dr["ID_HE_DAOTAO"];
                    ir["TEN_HE_DAOTAO"] = dr["TEN_HE_DAOTAO"];

                    iDataCombo.Rows.Add(ir);
                    iDataCombo.AcceptChanges();
                }
                cboHDT.ItemsSource = iDataCombo;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        void SetComboKhoa(int IdHedaoTao)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ID_KHOAHOC", typeof(Decimal));
                dt.Columns.Add("TEN_KHOAHOC", typeof(string));
                DataTable xdt = client.GetKhoaWhereHDT(IdHedaoTao);
                foreach (DataRow r in xdt.Rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["ID_KHOAHOC"] = r["ID_KHOAHOC"];
                    dr["TEN_KHOAHOC"] = r["TEN_KHOAHOC"];

                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                }
                CboKhoa.ItemsSource = dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        void SetComboNganh()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ID_NGANH", typeof(Decimal));
                dt.Columns.Add("TEN_NGANH", typeof(string));
                DataTable xdt = client.GetNganhWhereHDT(0);
                foreach (DataRow dr in xdt.Rows)
                {
                    DataRow ir = dt.NewRow();
                    ir["ID_NGANH"] = dr["ID_NGANH"];
                    ir["TEN_NGANH"] = dr["TEN_NGANH"];

                    dt.Rows.Add(ir);
                    dt.AcceptChanges();
                }
                cboNganh.ItemsSource = dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        private void BtnDong_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                this.Close();
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void BtnChon_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                DataRow[] check = (from temp in iGridDataSoure.AsEnumerable() where temp.Field<string>("CHK") == "True" select temp).ToArray();
                if (check.Count() > 0)
                {
                    iDataReturn = check.CopyToDataTable();
                }
                this.Close();
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void CboHDT_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                int IdHedaoTao = Convert.ToInt32(this.iDataSoure.Rows[0]["ID_HE_DAOTAO"].ToString());
                SetComboKhoa(IdHedaoTao);
            }
            catch (Exception err)
            {

                throw err;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void CboKhoa_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void CboNganh_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (this.iDataSoure.Rows[0]["ID_NGANH"].ToString() != string.Empty)
                {
                    int res = client.GetIDKhoaNganh(Convert.ToInt32(this.iDataSoure.Rows[0]["ID_KHOAHOC"].ToString()), Convert.ToInt32(this.iDataSoure.Rows[0]["ID_NGANH"].ToString()));
                    if (res != 0 || !res.ToString().Equals(string.Empty))
                    {
                        iGridDataSoure = client.GetAllKhoaNganhCT(res);
                        iGridDataSoure.Columns.Add("CHK");
                        grdKhoaNganhCT.ItemsSource = iGridDataSoure;
                    }
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }
    }
}
