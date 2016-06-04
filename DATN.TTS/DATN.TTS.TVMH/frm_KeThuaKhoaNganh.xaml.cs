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
using DevExpress.Xpf.Grid.Native;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_KeThuaKhoaNganh.xaml
    /// </summary>
    public partial class frm_KeThuaKhoaNganh : Window
    {
        private DataTable iGridDataSoure = null;
        public DataTable iDataReturn = null;
        private DataTable iDataSoure = null;
        bus_LapKeHoachDaoTaoKhoa client = new bus_LapKeHoachDaoTaoKhoa();
        public frm_KeThuaKhoaNganh()
        {
            InitializeComponent();
            this.iDataSoure = TableSchemaBinding();
            this.DataContext = iDataSoure;
            InitGrid();
            SetCombo();
            this.iDataSoure.Rows[0]["ID_HE_DAOTAO"] = "0";
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
                dt = TableUtil.ConvertToTable(dic);

                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        void InitGrid()
        {
            try
            {
                GridColumn col;
                #region Hide
                col = new GridColumn();
                col.FieldName = "ID_KHOAHOC_NGANH";
                col.Header = string.Empty;
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "ID_KHOAHOC";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grd.Columns.Add(col);


                col = new GridColumn();
                col.FieldName = "ID_NGANH";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grd.Columns.Add(col);

                #endregion

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
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TEN_KHOAHOC";
                col.Header = "Khóa";
                col.Width = 100;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TEN_NGANH";
                col.Header = "Ngành";
                col.Width = 200;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "SO_HKY";
                col.Header = "Số học kỳ";
                col.Width = 100;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "HOCKY_TRONGKHOA";
                col.Header = "Thứ tự học kỳ";
                col.Width = 150;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "SO_SINHVIEN_DK";
                col.Header = "Số sinh viên dự kiến";
                col.Width = 180;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "SO_LOP";
                col.Header = "Số lớp";
                col.Width = 100;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "GHICHU";
                col.Header = "Ghi chú";
                col.Width = 200;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = false;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grd.Columns.Add(col);
            }
            catch (Exception er)
            {
                throw er;
            }
        }

        void SetCombo()
        {
            try
            {
                DataTable iDataCombo = new DataTable();
                iDataCombo.Columns.Add("ID_HE_DAOTAO", typeof(Decimal));
                iDataCombo.Columns.Add("TEN_HE_DAOTAO", typeof(string));
                DataRow r = iDataCombo.NewRow();
                r["ID_HE_DAOTAO"] = 0;
                r["TEN_HE_DAOTAO"] = "----------------Chọn----------------";
                iDataCombo.Rows.Add(r);
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

        private void BtnChon_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                iDataReturn = (from temp in iGridDataSoure.AsEnumerable() where temp.Field<string>("CHK") == "True" select temp).CopyToDataTable();
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
                iGridDataSoure = client.GetAllKhoaNganh(Convert.ToInt32(this.iDataSoure.Rows[0]["ID_KHOAHOC"].ToString()));
                iGridDataSoure.Columns.Add("CHK");
                this.grd.ItemsSource = iGridDataSoure;
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
