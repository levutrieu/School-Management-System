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
using DATN.TTS.TVMH.Resource;
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
        }

        DataTable TableSchemaBinding()
        {
            try
            {
                DataTable dt = null;
                Dictionary<string, Type> dic = new Dictionary<string, Type>();
                dic.Add("ID_KHOAHOC", typeof(int));
                dic.Add("TEN_KHOAHOC", typeof(string));
                dic.Add("ID_HE_DAOTAO", typeof(int));
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
                DataTable dt = client.GetAllHDT();
                if (dt.Rows.Count > 0)
                {
                    ComboBoxUtil.SetComboBoxEdit(cboHDT, "TEN_HE_DAOTAO", "ID_HE_DAOTAO", dt, SelectionTypeEnum.Native);
                    this.iDataSoure.Rows[0]["ID_HE_DAOTAO"] = cboHDT.GetKeyValue(0);
                }
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
                DataTable xdt = client.GetKhoaWhereHDT(IdHedaoTao);
                if (xdt.Rows.Count > 0)
                {
                    ComboBoxUtil.SetComboBoxEdit(CboKhoa, "TEN_KHOAHOC", "ID_KHOAHOC", xdt, SelectionTypeEnum.Native);
                    this.iDataSoure.Rows[0]["ID_KHOAHOC"] = CboKhoa.GetKeyValue(0);
                }
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
                iGridDataSoure = null;
                CboKhoa.ItemsSource = null;
                int IdHedaoTao = Convert.ToInt32(this.iDataSoure.Rows[0]["ID_HE_DAOTAO"].ToString());
                SetComboKhoa(IdHedaoTao);
                grd.ItemsSource = iGridDataSoure;
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
                iGridDataSoure = null;
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

        private void GrdViewKhoaNganh_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (this.grd.GetFocusedRow() == null)
                    return;
                DataRow row = ((DataRowView)this.grd.GetFocusedRow()).Row;
                int index = Convert.ToInt32(row["ID_NGANH"].ToString());
                int vtri = -1;
                for (int i = 0; i < iGridDataSoure.Rows.Count; i++)
                {
                    int idnganh = Convert.ToInt32(iGridDataSoure.Rows[i]["ID_NGANH"].ToString());
                    if (index == idnganh)
                    {
                        vtri = i;
                        break;
                    }
                }
                if (iGridDataSoure.Rows[vtri]["CHK"].ToString() == "True")
                {
                    iGridDataSoure.Rows[vtri]["CHK"] = "False";
                }
                else
                {
                    iGridDataSoure.Rows[vtri]["CHK"] = "True";
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
