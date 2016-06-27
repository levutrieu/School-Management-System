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
using DATN.TTS.TVMH.Resource;
using DevExpress.Utils;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_QLyNgayDauNamHoc_HocKy.xaml
    /// </summary>
    public partial class frm_QLyNgayDauNamHoc_HocKy : Page
    {
        bus_namhoc client = new bus_namhoc();
        private DataTable iDataSoure = null;
        private DataTable iGridDataSoure = null;

        public frm_QLyNgayDauNamHoc_HocKy()
        {
            InitializeComponent();
            this.iDataSoure = TableSchemabinding();
            this.DataContext = iDataSoure;
            this.iDataSoure.Rows[0]["USER"] = UserCommon.UserName;
            this.iDataSoure.Rows[0]["HOCKY"] = "0";
            this.iDataSoure.Rows[0]["ID_NAMHOC_HIENTAI"] = "0";
            SetComboNamHoc();
            InitGrid();
        }

        void SetComboNamHoc()
        {
            DataTable dt = client.GetAllNamHoc();
            cboNamHoc.ItemsSource = dt;
            if (dt.Rows.Count > 0)
                this.iDataSoure.Rows[0]["ID_NAMHOC_HIENTAI"] = cboNamHoc.GetKeyValue(0);
        }

        DataTable TableSchemabinding()
        {
            try
            {
                DataTable dt = new DataTable();
                Dictionary<string, Type> dic = new Dictionary<string, Type>();
                dic.Add("ID_NAMHOC_HKY_HTAI", typeof(Decimal));
                dic.Add("ID_NAMHOC_HIENTAI", typeof(Decimal));
                dic.Add("HOCKY", typeof(int));
                dic.Add("HOCKY_NAME", typeof(string));
                dic.Add("IS_HIENTAI", typeof(Decimal));
                dic.Add("TUAN_BD_HKY", typeof(Decimal));
                dic.Add("USER", typeof(string));
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
                GridColumn col = null;

                col = new GridColumn();
                col.FieldName = "ID_NAMHOC_HKY_HTAI";
                col.Header = string.Empty;
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "ID_NAMHOC_HIENTAI";
                col.Header = "";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "NAMHOC_TU";
                col.Header = "Năm bắt đầu";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "NAMHOC";
                col.Header = "Năm học";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "HOCKY";
                col.Header = "Học kỳ";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grd.Columns.Add(col); 

                col = new GridColumn();
                col.FieldName = "TUAN_BD_HKY";
                col.Header = "Tuần bắt đầu";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "IS_HIENTAI";
                col.Header = "Học kỳ hiện tại";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                
                col.AllowEditing = DefaultBoolean.False;
                col.EditSettings = new CheckEditSettings();
                col.Visible = true;
                grd.Columns.Add(col);

                GetGrid();
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        private void GetGrid()
        {
            this.iGridDataSoure = client.GetData(Convert.ToInt32(this.iDataSoure.Rows[0]["ID_NAMHOC_HIENTAI"].ToString()));

            grd.ItemsSource = this.iGridDataSoure;
        }

        private void btnRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                //this.iDataSoure.Rows[0]["HOCKY"] = "0";
                //this.iDataSoure.Rows[0]["ID_NAMHOC_HIENTAI"] = "0";
                SetComboNamHoc();
                GetGrid();
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

        private void GrdViewNDung_OnFocusedRowChanged(object sender, DevExpress.Xpf.Grid.FocusedRowChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                DataRow r = null;
                if (this.grd.GetFocusedRow() == null)
                    return;
                r = ((DataRowView)grd.GetFocusedRow()).Row;
                this.iDataSoure.Rows[0]["ID_NAMHOC_HIENTAI"] = r["ID_NAMHOC_HIENTAI"];
                this.iDataSoure.Rows[0]["HOCKY"] = r["HOCKY"];
                this.iDataSoure.Rows[0]["TUAN_BD_HKY"] = r["TUAN_BD_HKY"];
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

        private void CboNamHoc_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                //if (!string.IsNullOrEmpty(this.iDataSoure.Rows[0]["ID_NAMHOC_HIENTAI"].ToString()))
                //{
                //    int idNamhocHientai = Convert.ToInt32(this.iDataSoure.Rows[0]["ID_NAMHOC_HIENTAI"].ToString());
                //    SetComboHK(idNamhocHientai);
                //}
                //GetGrid();
            }
            catch (Exception er)
            {
                throw er;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void CboHocKy_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (Convert.ToInt32(this.iDataSoure.Rows[0]["HOCKY"].ToString()) == 1)
                {
                    this.iDataSoure.Rows[0]["TUAN_BD_HKY"] = 1;
                }
                if (Convert.ToInt32(this.iDataSoure.Rows[0]["HOCKY"].ToString()) == 2)
                {
                    this.iDataSoure.Rows[0]["TUAN_BD_HKY"] = 24;
                }
                if (Convert.ToInt32(this.iDataSoure.Rows[0]["HOCKY"].ToString()) == 3)
                {
                    this.iDataSoure.Rows[0]["TUAN_BD_HKY"] = 48;
                }
                GetGrid();
            }
            catch (Exception er)
            {
                throw er;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void BtnSetNamHoc_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                int pID_NAMHOC_HIENTAI = Convert.ToInt32(this.iDataSoure.Rows[0]["ID_NAMHOC_HIENTAI"].ToString());
                int pHOCKY = Convert.ToInt32(this.iDataSoure.Rows[0]["HOCKY"].ToString());
                if (pID_NAMHOC_HIENTAI != 0 && pHOCKY != 0)
                {
                    bool res = client.CheckTrungHocKyNamHoc(pID_NAMHOC_HIENTAI, pHOCKY);
                    if (res)
                    {
                        bool resultInse = client.Insert_HocKyNamHoc(pID_NAMHOC_HIENTAI, pHOCKY, Convert.ToInt32(this.iDataSoure.Rows[0]["TUAN_BD_HKY"].ToString()), UserCommon.UserName);
                        if (resultInse)
                        {
                            CTMessagebox.Show("Thiết lập học kỳ hiện tại thành công", "Thiết lập mới", "", CTICON.Information,CTBUTTON.OK);
                            GetGrid();
                        }
                        else
                        {
                            CTMessagebox.Show("Thiết lập học kỳ hiện tại không thành công", "Thiết lập mới", "", CTICON.Information, CTBUTTON.OK);
                        }
                    }
                    else
                    {
                        bool resultUp = client.Update_IsHienTai(pID_NAMHOC_HIENTAI, pHOCKY, UserCommon.UserName);
                        if (resultUp)
                        {
                            CTMessagebox.Show("Thiết lập học kỳ hiện tại thành công", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                            GetGrid();
                        }
                        else
                        {
                            CTMessagebox.Show("Thiết lập học kỳ hiện tại không thành công", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                        }
                    }
                }
                else
                {
                    CTMessagebox.Show("Vui lòng chọn năm học và học kỳ", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                    cboNamHoc.Focus();
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
