using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
using DevExpress.Utils;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_KhoaNganh.xaml
    /// </summary>
    public partial class frm_KhoaNganh : UserControl
    {
        public bus_LapKeHoachDaoTaoKhoa client = new bus_LapKeHoachDaoTaoKhoa();
        public DataTable iGridDataSoureNganh = null;

        public DataTable iGridDataSoureKhoaNganh = new DataTable();

        public DataTable iDataSoure = null;

        public frm_KhoaNganh()
        {
            InitializeComponent();

            this.iDataSoure = TableSchemBinding();

            //this.iGridDataSoureKhoaNganh = TableSchemaBindings_Grid();

            this.DataContext = iDataSoure;

            this.iDataSoure.Rows[0]["USER"] = UserCommon.UserName;
            
            //SetComBo();
            InitGridNganh();
            InitGridKhoaNganh();
        }

        void InitGridNganh()
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
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "ID_NGANH";
                col.Header = string.Empty;
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
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
                col.FieldName = "MA_NGANH";
                col.Header = "Mã Ngành";
                col.Width = 80;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TEN_NGANH";
                col.Header = "Ngành";
                col.Width = 250;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "CAP_NGANH";
                col.Header = "Cấp ngành";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grd.Columns.Add(col);
            }
            catch (Exception er)
            {
                throw er;
            }
        }

        void InitGridKhoaNganh()
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
                grdKhoaNganh.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "ID_KHOAHOC";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grdKhoaNganh.Columns.Add(col);


                col = new GridColumn();
                col.FieldName = "ID_NGANH";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grdKhoaNganh.Columns.Add(col);

                #endregion

                col = new GridColumn();
                col.FieldName = "CHK";
                col.Header = "Xóa";
                col.Width = 100;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                col.EditSettings = new CheckEditSettings();
                col.UnboundType = UnboundColumnType.Boolean;
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdKhoaNganh.Columns.Add(col);

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
                grdKhoaNganh.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TEN_NGANH";
                col.Header = "Ngành";
                col.Width = 200;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grdKhoaNganh.Columns.Add(col);

                col = new GridColumn();
                TextEditSettings txtSOHK = new TextEditSettings();
                txtSOHK.MaskType = MaskType.Numeric;
                txtSOHK.Mask = "##";
                txtSOHK.MaxLength = 2;
                col.EditSettings = txtSOHK;
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                col.FieldName = "SO_HKY";
                col.Header = "Số học kỳ";
                col.Width = 100;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grdKhoaNganh.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "HOCKY_TRONGKHOA";
                col.Header = "Thứ tự học kỳ";
                col.Width = 150;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grdKhoaNganh.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "SO_SINHVIEN_DK";
                col.Header = "Số sinh viên dự kiến";
                col.Width = 180;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                SpinEditSettings txtSoSV = new SpinEditSettings();
                txtSoSV.MinValue = 0;
                txtSoSV.MaskType = MaskType.Numeric;
                col.EditSettings = txtSoSV;
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdKhoaNganh.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "SO_LOP";
                col.Header = "Số lớp";
                col.Width = 100;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                SpinEditSettings txtSOLOP = new SpinEditSettings();
                txtSOLOP.MinValue = 0;
                txtSoSV.MaskType = MaskType.Numeric;
                col.EditSettings = txtSOLOP;
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grdKhoaNganh.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "GHICHU";
                col.Header = "Ghi chú";
                col.Width = 200;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grdKhoaNganh.Columns.Add(col);
            }
            catch (Exception er)
            {
                throw er;
            }
        }

        public void LoadNganh(DataTable xdt)
        {
            try
            {
                iGridDataSoureNganh = xdt;
                iGridDataSoureNganh.Columns.Add("CHK");
                grd.ItemsSource = iGridDataSoureNganh;
            }
            catch (Exception er)
            {
                throw er; 
            }
        }

        DataTable TableSchemBinding()
        {
            try
            {
                DataTable dt = new DataTable();
                Dictionary<string, Type> dic = new Dictionary<string, Type>();
                dic.Add("USER", typeof(string));
                dic.Add("ID_HE_DAOTAO", typeof(Decimal));
                dic.Add("TEN_HE_DAOTAO", typeof(string));
                dic.Add("TEN_KHOAHOC", typeof(string));
                dic.Add("ID_KHOAHOC", typeof(Decimal));
                dic.Add("ID_NGANH", typeof(Decimal));
                dic.Add("TEN_NGANH", typeof(string));
                dic.Add("SO_LOP", typeof(Decimal));
                //===============================
                dic.Add("KHOAHOC_NGANH", typeof(string));
                dic.Add("ID_KHOAHOC_NGANH", typeof(Decimal));
                dt = TableUtil.ConvertToTable(dic);
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
           
        }

        DataTable TableSchemaBindings_Grid()
        {
            try
            {
                DataTable dt = null;
                Dictionary<string, Type> dic = new Dictionary<string, Type>();
                dic.Add("ID_KHOAHOC_NGANH", typeof(Decimal));
                dic.Add("ID_KHOAHOC", typeof(Decimal));
                dic.Add("ID_NGANH", typeof(Decimal));
                dic.Add("TEN_KHOAHOC", typeof(string));
                dic.Add("TEN_NGANH", typeof(string));
                dic.Add("SO_HKY", typeof(Decimal));
                dic.Add("HOCKY_TRONGKHOA", typeof(string));
                dic.Add("SO_SINHVIEN_DK", typeof(Decimal));
                dic.Add("SO_LOP", typeof(Decimal));
                dic.Add("GHICHU", typeof(string));
                dic.Add("CHK",typeof(bool));
                dt = TableUtil.ConvertDictionaryToTable(dic, false);
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public void LoadKhoaNganh()
        {
            try
            {
                iGridDataSoureKhoaNganh = client.GetAllKhoaNganh(Convert.ToInt32(this.iDataSoure.Rows[0]["ID_KHOAHOC"].ToString()));
                iGridDataSoureKhoaNganh.Columns.Add("CHK");
                grdKhoaNganh.ItemsSource = iGridDataSoureKhoaNganh;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public bool KiemTraThemKhoaNganh(Decimal idNganh, DataTable DataTemp)
        {
            try
            {
                if (DataTemp.Rows.Count == 0)
                    return true;
                foreach (DataRow r in DataTemp.Rows)
                {
                    if (Convert.ToDecimal(r["ID_NGANH"].ToString()) == idNganh)
                        return false;
                }
                return true;

            }
            catch (Exception err)
            {
                throw err;
            }
        }

        private void BtnThem_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (iGridDataSoureKhoaNganh.Rows.Count == 0)
                {
                    iGridDataSoureKhoaNganh = TableSchemaBindings_Grid();
                }
                #region Bỏ
                //if (this.iDataSoure.Rows[0]["ID_NGANH"].ToString() == string.Empty || this.iDataSoure.Rows[0]["ID_NGANH"].ToString() == "0")
                //{
                //    CTMessagebox.Show("Bạn chưa chọn ngành nào để thêm vào khóa", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                //    return;
                //}
                //bool check = KiemTraThemKhoaNganh(Convert.ToDecimal(this.iDataSoure.Rows[0]["ID_NGANH"].ToString()), iGridDataSoureKhoaNganh);
                //if (!check)
                //{
                //    CTMessagebox.Show("Ngành vừa thêm đã có trong khóa!", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                //    return;
                //} 
                #endregion
                DataTable dt = new DataTable();
                if (iGridDataSoureNganh.Rows.Count > 0)
                {
                    dt = (from temp in iGridDataSoureNganh.AsEnumerable().Where(t => t.Field<string>("CHK") == "True") select temp).CopyToDataTable();
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow xr in dt.Rows)
                    {
                        DataRow r = iGridDataSoureKhoaNganh.NewRow();
                        r["ID_KHOAHOC_NGANH"] = "0";
                        r["ID_KHOAHOC"] = this.iDataSoure.Rows[0]["ID_KHOAHOC"];
                        r["TEN_KHOAHOC"] = this.iDataSoure.Rows[0]["TEN_KHOAHOC"];
                        r["ID_NGANH"] = xr["ID_NGANH"];
                        r["TEN_NGANH"] = xr["TEN_NGANH"];
                        r["SO_HKY"] = "0";
                        r["HOCKY_TRONGKHOA"] = "";
                        r["SO_SINHVIEN_DK"] = "0";
                        r["SO_LOP"] = "0";
                        r["GHICHU"] = "";
                        iGridDataSoureKhoaNganh.Rows.Add(r);
                        iGridDataSoureKhoaNganh.AcceptChanges();
                    }
                }
                #region Bỏ
                //DataRow r = iGridDataSoureKhoaNganh.NewRow();
                //r["ID_KHOAHOC_NGANH"] = "0";
                //r["ID_KHOAHOC"] = this.iDataSoure.Rows[0]["ID_KHOAHOC"];
                //r["TEN_KHOAHOC"] = this.iDataSoure.Rows[0]["TEN_KHOAHOC"];
                //r["ID_NGANH"] = this.iDataSoure.Rows[0]["ID_NGANH"];
                //r["TEN_NGANH"] = this.iDataSoure.Rows[0]["TEN_NGANH"];
                //r["SO_HKY"] = "0";
                //r["HOCKY_TRONGKHOA"] = "";
                //r["SO_SINHVIEN_DK"] = "0";
                //r["SO_LOP"] = "0";
                //r["GHICHU"] = "";
                //iGridDataSoureKhoaNganh.Rows.Add(r);
                //iGridDataSoureKhoaNganh.AcceptChanges(); 
                #endregion
                //iGridDataSoureKhoaNganh.Columns.Add("CHK");
                grdKhoaNganh.ItemsSource = iGridDataSoureKhoaNganh;
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

        private void BtnKeThua_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                frm_KeThuaKhoaNganh frmKeThua = new frm_KeThuaKhoaNganh();
                frmKeThua.Owner = System.Windows.Window.GetWindow(this);
                frmKeThua.ShowDialog();
                DataTable xdt = frmKeThua.iDataReturn.Copy();
                if (iGridDataSoureKhoaNganh.Rows.Count == 0)
                {
                    iGridDataSoureKhoaNganh = TableSchemaBindings_Grid();

                    foreach (DataRow dr in xdt.Rows)
                    {
                        DataRow r = iGridDataSoureKhoaNganh.NewRow();
                        r["ID_KHOAHOC_NGANH"] = "0";
                        r["ID_KHOAHOC"] = this.iDataSoure.Rows[0]["ID_KHOAHOC"];
                        r["TEN_KHOAHOC"] = this.iDataSoure.Rows[0]["TEN_KHOAHOC"];
                        r["ID_NGANH"] = dr["ID_NGANH"];
                        r["TEN_NGANH"] = dr["TEN_NGANH"];
                        r["SO_HKY"] = dr["SO_HKY"];
                        r["HOCKY_TRONGKHOA"] = dr["HOCKY_TRONGKHOA"];
                        r["SO_SINHVIEN_DK"] = dr["SO_SINHVIEN_DK"];
                        r["SO_LOP"] = dr["SO_LOP"];
                        r["GHICHU"] = dr["GHICHU"];

                        iGridDataSoureKhoaNganh.Rows.Add(r);
                        iGridDataSoureKhoaNganh.AcceptChanges();
                    }
                }
                else
                {
                    DataTable dt = new DataTable();
                    if (iGridDataSoureKhoaNganh.Rows.Count > 0)
                    {
                        dt = xdt.Clone();
                        foreach (DataRow r in xdt.Rows)
                        {
                            foreach (DataRow xdr in iGridDataSoureKhoaNganh.Rows)
                            {
                                int x = Convert.ToInt32(xdr["ID_NGANH"].ToString());
                                int y = Convert.ToInt32(r["ID_NGANH"].ToString());
                                if (y != x)
                                {
                                    dt.ImportRow(r);
                                }
                            }
                        }
                        foreach (DataRow dr in dt.Rows)
                        {
                            DataRow r = iGridDataSoureKhoaNganh.NewRow();
                            r["ID_KHOAHOC_NGANH"] = "0";
                            r["ID_KHOAHOC"] = this.iDataSoure.Rows[0]["ID_KHOAHOC"];
                            r["TEN_KHOAHOC"] = this.iDataSoure.Rows[0]["TEN_KHOAHOC"];
                            r["ID_NGANH"] = dr["ID_NGANH"];
                            r["TEN_NGANH"] = dr["TEN_NGANH"];
                            r["SO_HKY"] = dr["SO_HKY"];
                            r["HOCKY_TRONGKHOA"] = dr["HOCKY_TRONGKHOA"];
                            r["SO_SINHVIEN_DK"] = dr["SO_SINHVIEN_DK"];
                            r["SO_LOP"] = dr["SO_LOP"];
                            r["GHICHU"] = dr["GHICHU"];

                            iGridDataSoureKhoaNganh.Rows.Add(r);
                            iGridDataSoureKhoaNganh.AcceptChanges();
                        }
                        DataTable xdtTable = iGridDataSoureKhoaNganh.AsEnumerable().GroupBy(a => a.Field<int>("ID_NGANH")).Select(b => b.First()).CopyToDataTable();
                        iGridDataSoureKhoaNganh = xdtTable.Copy(); 
                    }
                }
                #region Bỏ
                //else
                //{
                //    dt = xdt.Copy();
                //}
                //if (dt.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        DataRow r = iGridDataSoureKhoaNganh.NewRow();
                //        r["ID_KHOAHOC_NGANH"] = "0";
                //        r["ID_KHOAHOC"] = this.iDataSoure.Rows[0]["ID_KHOAHOC"];
                //        r["TEN_KHOAHOC"] = this.iDataSoure.Rows[0]["TEN_KHOAHOC"];
                //        r["ID_NGANH"] = dr["ID_NGANH"];
                //        r["TEN_NGANH"] = dr["TEN_NGANH"];
                //        r["SO_HKY"] = dr["SO_HKY"];
                //        r["HOCKY_TRONGKHOA"] = dr["HOCKY_TRONGKHOA"];
                //        r["SO_SINHVIEN_DK"] = dr["SO_SINHVIEN_DK"];
                //        r["SO_LOP"] = dr["SO_LOP"];
                //        r["GHICHU"] = dr["GHICHU"];

                //        iGridDataSoureKhoaNganh.Rows.Add(r);
                //        iGridDataSoureKhoaNganh.AcceptChanges();
                //    }
                //    DataTable xdtTable = iGridDataSoureKhoaNganh.AsEnumerable().GroupBy(a => a.Field<int>("ID_NGANH")).Select(b => b.First()).CopyToDataTable();
                //    iGridDataSoureKhoaNganh = xdtTable.Copy(); 
                //}
                #endregion
                grdKhoaNganh.ItemsSource = iGridDataSoureKhoaNganh;
            }
            catch
            {
                return;
            }
        }

        private void GrdViewKhoaNganh_OnCellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (this.grdKhoaNganh.GetFocusedRow() == null)
                    return;
                int index = this.grdViewKhoaNganh.FocusedRowHandle;
                DataRow r = ((DataRowView)this.grdKhoaNganh.GetFocusedRow()).Row;
                int temp = Convert.ToInt32(r["SO_HKY"].ToString());
                if (temp < 0 || temp > 10)
                {
                    CTMessagebox.Show("Số học kỳ phải nằm trong khoảng 0 -> 10", "Thông báo", "", CTICON.Warning,CTBUTTON.OK);
                    return;
                }
                string HOCKY_TRONGKHOA = string.Empty;
                for (int i = 1; i <= temp; i++)
                {
                    if (i == temp)
                    {
                        HOCKY_TRONGKHOA += i;
                    }
                    else
                    {
                        HOCKY_TRONGKHOA += i + ", ";
                    }
                }
                this.iGridDataSoureKhoaNganh.Rows[index]["HOCKY_TRONGKHOA"] = HOCKY_TRONGKHOA;
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

        public void BtnThemKhoaNganh_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (iGridDataSoureKhoaNganh != null && iGridDataSoureKhoaNganh.Rows.Count > 0)
                {
                    bool res = client.Insert_Khoa_Nganh(this.iGridDataSoureKhoaNganh.Copy(), UserCommon.UserName);
                    if (!res)
                    {
                        CTMessagebox.Show("Lưu khóa ngành không thành công", "Lưu", "", CTICON.Error, CTBUTTON.OK);
                        LoadKhoaNganh();
                        return;
                    }
                    else
                    {
                        CTMessagebox.Show("Lưu khóa ngành thành công", "Lưu", "", CTICON.Information, CTBUTTON.OK);
                        LoadKhoaNganh();
                        iGridDataSoureNganh = client.GetNganhWhereHDT(Convert.ToInt32(iDataSoure.Rows[0]["ID_KHOAHOC"].ToString()));
                        iGridDataSoureNganh.Columns.Add("CHK");
                        grd.ItemsSource = iGridDataSoureNganh;
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

        private void BtnXoa_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                int count = 0;
                foreach (DataRow r in iGridDataSoureKhoaNganh.Rows)
                {
                    if (r["CHK"].ToString() == "True")
                        count++;
                }
                if (count == 0)
                {
                    CTMessagebox.Show("Bạn chưa chọn ngành nào để xóa!", "Thông báo", "", CTICON.Information,CTBUTTON.OK);
                    return;
                }
                DataTable dt = (from temp in iGridDataSoureKhoaNganh.AsEnumerable()where temp.Field<string>("CHK") == "True" select temp).CopyToDataTable();
                if (dt.Rows.Count > 0)
                {
                    bool res = client.Delete_Khoa_Nganh(dt.Copy(), UserCommon.UserName);
                    if (!res)
                    {
                        CTMessagebox.Show("Xóa ngành không thành công", "Xóa", "", CTICON.Error, CTBUTTON.OK);
                        LoadKhoaNganh();
                        return;
                    }
                    else
                    {
                        CTMessagebox.Show("Xóa ngành thành công", "Xóa", "", CTICON.Information, CTBUTTON.OK);
                        LoadKhoaNganh();
                        iGridDataSoureNganh = client.GetNganhWhereHDT(Convert.ToInt32(iDataSoure.Rows[0]["ID_KHOAHOC"].ToString()));
                        iGridDataSoureNganh.Columns.Add("CHK");
                        grd.ItemsSource = iGridDataSoureNganh;
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

        private void BtnCapNhat_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                LoadKhoaNganh();
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

        private void GrdViewKhoaNganh_OnFocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (this.grdKhoaNganh.GetFocusedRow() == null)
                    return;
                DataRow row = ((DataRowView)this.grdKhoaNganh.GetFocusedRow()).Row;
                this.iDataSoure.Rows[0]["ID_KHOAHOC_NGANH"] = row["ID_KHOAHOC_NGANH"];
                this.iDataSoure.Rows[0]["KHOAHOC_NGANH"] = row["TEN_KHOAHOC"] + " Ngành: " + row["TEN_NGANH"];
                this.iDataSoure.Rows[0]["SO_LOP"] = row["SO_LOP"];
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

        private void BtnThemLop_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                frm_MoLopHocPopUp frm = new frm_MoLopHocPopUp(this.iDataSoure.Copy());
                frm.Owner = Window.GetWindow(this);
                frm.ShowDialog();
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
                Stopwatch sw = new Stopwatch();
                sw.Start();
                DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                options.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Value;
                options.ExportMode = DevExpress.XtraPrinting.XlsExportMode.SingleFile;
                ((TableView)grdKhoaNganh.View).ExportToXls(@"C:\Users\MINHTHONG\Desktop\khoanganh.xls", options);
                sw.Stop();
                CTMessagebox.Show(string.Format("Done in {0} sec.", sw.ElapsedMilliseconds / 1000));
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        #region Bỏ
        // chua dung toi
        bool CheckTrungKeThua(DataTable dtTable, int id)
        {
            try
            {
                if (dtTable.Rows.Count > 0)
                {
                    foreach (DataRow r in dtTable.Rows)
                    {
                        if (Convert.ToInt32(r["ID_NGANH"].ToString()) == id)
                            return false;
                    }
                }
                return true;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        private void GrdNganh_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (this.grd.GetFocusedRow() == null)
                    return;
                DataRow row = ((DataRowView)this.grd.GetFocusedRow()).Row;
                int index = Convert.ToInt32(row["ID_NGANH"].ToString());
                int vtri = -1;
                for (int i = 0 ; i <iGridDataSoureNganh.Rows.Count; i++)
                {
                    int idnganh = Convert.ToInt32(iGridDataSoureNganh.Rows[i]["ID_NGANH"].ToString());
                    if (index == idnganh)
                    {
                        vtri = i;
                        break;
                    }
                }
                if (iGridDataSoureNganh.Rows[vtri]["CHK"].ToString() == "True")
                {
                    iGridDataSoureNganh.Rows[vtri]["CHK"]= "False";
                }
                else
                {
                    iGridDataSoureNganh.Rows[vtri]["CHK"] = "True";
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

        private void GrdNganh_OnFocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (this.grd.GetFocusedRow() == null)
                    return;
                DataRow row = ((DataRowView)this.grd.GetFocusedRow()).Row;
                this.iDataSoure.Rows[0]["ID_NGANH"] = row["ID_NGANH"];
                this.iDataSoure.Rows[0]["TEN_NGANH"] = row["TEN_NGANH"];
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
        #endregion
    }
}
