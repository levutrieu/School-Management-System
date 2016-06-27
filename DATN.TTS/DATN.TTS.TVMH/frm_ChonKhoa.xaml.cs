using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
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
using DATN.TTS.TVMH.Resource;
using DevExpress.Utils;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_ChonKhoa.xaml
    /// </summary>
    public partial class frm_ChonKhoa : UserControl
    {
        bus_LapKeHoachDaoTaoKhoa client = new bus_LapKeHoachDaoTaoKhoa();
        public  DataTable iDataSoure = null;

        public DataTable iGridDataSoure = null;

        public frm_ChonKhoa()
        {
            InitializeComponent();
            iDataSoure = TableSchemaBinding();
            this.DataContext = iDataSoure;

            //iDataSoure.Rows[0]["USER"] = UserCommon.UserName;
            this.iDataSoure.Rows[0]["ID_HE_DAOTAO"] = "1";
            SetComBo();
            InitGridKhoa();
        }

        DataTable TableSchemaBinding()
        {
            try
            {
                DataTable dt = new DataTable();
                Dictionary<string, Type> dic = new Dictionary<string, Type>();
                //dic.Add("USER", typeof(string));
                dic.Add("ID_HE_DAOTAO", typeof(int));
                dic.Add("ID_KHOAHOC", typeof(int));
                dic.Add("TEN_HE_DAOTAO", typeof(string));
                dic.Add("TEN_KHOAHOC", typeof(string));
                dic.Add("SO_HKY", typeof(int));
                dt = TableUtil.ConvertToTable(dic);
                return dt;
            }
            catch (Exception er)
            {
                throw er;
            }
        }

        void SetComBo()
        {
           DataTable dt = client.GetAllHDT();
            if (dt.Rows.Count > 0)
            {
                ComboBoxUtil.SetComboBoxEdit(cboHeDT, "TEN_HE_DAOTAO", "ID_HE_DAOTAO", dt, SelectionTypeEnum.Native);
                this.iDataSoure.Rows[0]["ID_HE_DAOTAO"] = cboHeDT.GetKeyValue(0);
            }
        }

        void InitGridKhoa()
        {
            try
            {
                GridColumn col;

                col = new GridColumn();
                col.FieldName = "ID_KHOAHOC";
                col.Header = "UserName";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "ID_HE_DAOTAO";
                col.Header = "UserName";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TEN_HE_DAOTAO";
                col.Header = "Hệ đào tạo";
                col.Width = 150;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "MA_KHOAHOC";
                col.Header = "Mã khóa học";
                col.Width = 80;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grd.Columns.Add(col);


                col = new GridColumn();
                col.FieldName = "KYHIEU";
                col.Header = "Ký hiệu";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TEN_KHOAHOC";
                col.Header = "Khóa học";
                col.Width = 100;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "NAM_BD";
                col.Header = "Năm bắt đầu";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "NAM_KT";
                col.Header = "Năm kết thúc";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "SO_HKY_1NAM";
                col.Header = "Học kỳ/Năm";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "SO_HKY";
                col.Header = "Số học kỳ";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TRANGTHAI";
                col.Header = "Trạng thái";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grd.Columns.Add(col);

                GrdKhoa.AutoWidth = true;
                LoadKhoa();
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        void LoadKhoa()
        {
            try
            {
                iGridDataSoure = client.GetAllKhoaHoc(Convert.ToInt32(iDataSoure.Rows[0]["ID_HE_DAOTAO"].ToString()));
                grd.ItemsSource = iGridDataSoure;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        #region Chưa dùng
        private void BtnChon_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                SetComBo();
                CboHeDT_OnEditValueChanged(sender, null);
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

        private void BtnExcel_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (File.Exists(@"D:\DataExport") == false)
                {
                    Directory.CreateDirectory(@"D:\DataExport");
                }
                Stopwatch sw = new Stopwatch();
                sw.Start();
                DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                options.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Value;
                options.ExportMode = DevExpress.XtraPrinting.XlsExportMode.SingleFile;
                ((TableView)grd.View).ExportToXls(@"D:\DataExport\KhoaHoc.xls", options);
                sw.Stop();
                CTMessagebox.Show("File đã được lưu trên D:DataExport");
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        private void GrdKhoa_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            //try
            //{
            //    Mouse.OverrideCursor = Cursors.Wait;
            //    if (this.grd.GetFocusedRow() == null)
            //        return;
            //    DataRow  row = ((DataRowView)this.grd.GetFocusedRow()).Row;
            //    this.iDataSoure.Rows[0]["ID_KHOAHOC"] = row["ID_KHOAHOC"];
            //    this.iDataSoure.Rows[0]["TEN_KHOAHOC"] = row["TEN_KHOAHOC"];
            //    this.iDataSoure.Rows[0]["TEN_HE_DAOTAO"] = row["TEN_HE_DAOTAO"];
            //}
            //catch (Exception err)
            //{
            //    throw err;
            //}
            //finally
            //{
            //    Mouse.OverrideCursor = Cursors.Arrow;
            //}
        }

        #endregion

        private void CboHeDT_OnEditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                LoadKhoa();
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

        private void GrdKhoa_OnFocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (this.grd.GetFocusedRow() == null)
                    return;
                DataRow row = ((DataRowView)this.grd.GetFocusedRow()).Row;
                this.iDataSoure.Rows[0]["ID_KHOAHOC"] = row["ID_KHOAHOC"];
                this.iDataSoure.Rows[0]["TEN_KHOAHOC"] = row["TEN_KHOAHOC"];
                this.iDataSoure.Rows[0]["TEN_HE_DAOTAO"] = row["TEN_HE_DAOTAO"];
                this.iDataSoure.Rows[0]["SO_HKY"] = row["SO_HKY"];
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
