using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
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
using DATN.TTS.TVMH.Resource;
using DevExpress.Utils;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using Microsoft.Win32;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_NhapDiemSV.xaml
    /// </summary>
    public partial class frm_NhapDiemSV : Page
    {
        bus_SinhVien sv = new bus_SinhVien();
        bus_NhapDiemSV diem = new bus_NhapDiemSV();
        bus_molophocphan lhp = new bus_molophocphan();
        bus_LapKeHoachDaoTaoKhoa kehoach = new bus_LapKeHoachDaoTaoKhoa();
        bus_DangKyHocPhan dangkyhocphan = new bus_DangKyHocPhan();
        private DataTable iDataSource = null;
        private DataTable iGridDataSource = null;
        public frm_NhapDiemSV()
        {
            InitializeComponent();
            this.iDataSource = TableSchemaBinding();
            this.DataContext = this.iDataSource;
            iDataSource.Rows[0]["USER"] = UserCommon.UserName;
            SetCombo();
            Init_Grid();
        }

        void SetCombo()
        {
            DataTable dt = kehoach.GetAllHDT();
            if (dt.Rows.Count > 0)
            {
                ComboBoxUtil.SetComboBoxEdit(cboHDT, "TEN_HE_DAOTAO", "ID_HE_DAOTAO", dt, SelectionTypeEnum.Native);
                this.iDataSource.Rows[0]["ID_HE_DAOTAO"] = cboHDT.GetKeyValue(0);
            }
        }

        DataTable TableSchemaBinding()
        {
            try
            {
                DataTable dt = new DataTable();
                Dictionary<string, Type> dic = new Dictionary<string, Type>();
                dic.Add("USER", typeof(string));
                dic.Add("id_sinhvien_s", typeof(int));
                dic.Add("ID_HE_DAOTAO", typeof(int));
                dic.Add("ID_KHOAHOC_NGANH", typeof(int));
                dic.Add("ID_KHOAHOC", typeof(int));
                dic.Add("ID_NGANH", typeof(int));
                dic.Add("HOCKY", typeof(int));
                dic.Add("ID_MONHOC", typeof(int));
                dic.Add("ID_LOPHOCPHAN", typeof(int));
                dic.Add("ID_DANGKY", typeof(int));
                dt = TableUtil.ConvertToTable(dic);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        DataTable TableSchemaBinding_GridDiem()
        {
            Dictionary<string, Type> dic = new Dictionary<string, Type>();
            dic.Add("ID_KETQUA", typeof(int));
            dic.Add("ID_SINHVIEN", typeof(int));
            dic.Add("ID_LOPHOCPHAN", typeof(int));
            dic.Add("ID_KHOAHOC", typeof(int));
            dic.Add("ID_HOCKY", typeof(int));
            dic.Add("MA_SINHVIEN", typeof(string));
            dic.Add("TEN_SINHVIEN", typeof(string));
            dic.Add("TEN_LOP", typeof(string));
            dic.Add("MA_MONHOC", typeof(string));
            dic.Add("TEN_MONHOC", typeof(string));
            dic.Add("DIEM_BT", typeof(double));
            dic.Add("DIEM_GK", typeof(double));
            dic.Add("DIEM_CK", typeof(double));
            dic.Add("DIEM_TONG", typeof(double));
            dic.Add("DIEM_HE4", typeof(double));
            dic.Add("DIEM_CHU", typeof(string));
            dic.Add("CACH_TINHDIEM", typeof(string));
            dic.Add("ID_DANGKY", typeof(int));
            DataTable dt = TableUtil.ConvertDictionaryToTable(dic, false);
            return dt;
        }

        private void Init_Grid()
        {
            try
            {
                GridColumn col;

                #region Hide
                #region 1
                col = new GridColumn();
                col.FieldName = "ID_KETQUA";
                col.Visible = false;
                grd.Columns.Add(col);
                #endregion

                #region 2
                col = new GridColumn();
                col.FieldName = "ID_SINHVIEN";
                col.Visible = false;
                grd.Columns.Add(col);
                #endregion

                #region 3
                col = new GridColumn();
                col.FieldName = "ID_LOPHOCPHAN";
                col.Visible = false;
                grd.Columns.Add(col);
                #endregion

                #region 4
                col = new GridColumn();
                col.FieldName = "ID_KHOAHOC";
                col.Visible = false;
                grd.Columns.Add(col);
                #endregion

                #region 5
                col = new GridColumn();
                col.FieldName = "ID_HOCKY";
                col.Visible = false;
                grd.Columns.Add(col);
                #endregion

                #region 5.1
                col = new GridColumn();
                col.FieldName = "ID_DANGKY";
                col.Visible = false;
                col.Width = 10;
                grd.Columns.Add(col);
                #endregion

                #region 6
                col = new GridColumn();
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.FieldName = "MA_SINHVIEN";
                col.Header = "Mã sinh viên";
                col.Width = 70;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grd.Columns.Add(col);
                #endregion

                #region 7
                col = new GridColumn();
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.FieldName = "TEN_SINHVIEN";
                col.Header = "Tên sinh viên";
                col.Width = 120;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Left;
                grd.Columns.Add(col);
                #endregion;

                #region 8
                //col = new GridColumn();
                //col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                //col.FieldName = "TEN_LOP";
                //col.Header = "Lớp";
                //col.Width = 90;
                //col.AllowEditing = DefaultBoolean.False;
                //col.Visible = true;
                //col.EditSettings = new TextEditSettings();
                //col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Left;
                //grd.Columns.Add(col); 
                #endregion

                #region 9
                col = new GridColumn();
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.FieldName = "MA_MONHOC";
                col.Header = "Mã môn học";
                col.Width = 70;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grd.Columns.Add(col); 
                #endregion

                #region 10
                col = new GridColumn();
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.FieldName = "TEN_MONHOC";
                col.Header = "Tên môn học";
                col.Width = 100;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Left;
                grd.Columns.Add(col); 
                #endregion
                #endregion

                #region 11
                col = new GridColumn();
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.FieldName = "CACH_TINHDIEM";
                col.Header = "% Điểm";
                col.Width = 45;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grd.Columns.Add(col); 
                #endregion

                #region 12
                col = new GridColumn();
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.FieldName = "DIEM_BT";
                col.Header = "% BT";
                col.Width = 35;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                TextEditSettings txtDiemBT = new TextEditSettings();
                txtDiemBT.MaskType = MaskType.Numeric;
                txtDiemBT.Mask = "##.#";
                txtDiemBT.MaxLength = 4;
                txtDiemBT.DisplayFormat = "0.0";
                col.EditSettings = txtDiemBT;
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grd.Columns.Add(col); 
                #endregion

                #region 13
                col = new GridColumn() ;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.FieldName = "DIEM_GK";
                col.Header = "% GK";
                col.Width = 35;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                TextEditSettings txtDiemGK = new TextEditSettings();
                txtDiemGK.MaskType = MaskType.Numeric;
                txtDiemGK.Mask = "##.#";
                txtDiemGK.MaxLength = 4;
                txtDiemGK.DisplayFormat = "0.0";
                col.EditSettings = txtDiemGK;
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grd.Columns.Add(col); 
                #endregion

                #region 14
                col = new GridColumn();
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.FieldName = "DIEM_CK";
                col.Header = "% CK";
                col.Width = 35;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                TextEditSettings txtDiemCK = new TextEditSettings();
                txtDiemCK.MaskType = MaskType.Numeric;
                txtDiemCK.Mask = "##.#";
                txtDiemCK.MaxLength = 4;
                txtDiemCK.DisplayFormat = "0.0";
                col.EditSettings = txtDiemCK;
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grd.Columns.Add(col); 
                #endregion

                #region 15
                col = new GridColumn();
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.FieldName = "DIEM_TONG";
                col.Header = "TK(10/100)";
                col.Width = 45;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                TextEditSettings txtDiemTong = new TextEditSettings();
                col.EditSettings = txtDiemTong;
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                grd.Columns.Add(col); 
                #endregion

                #region 16
                col = new GridColumn();
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.FieldName = "DIEM_HE4";
                col.Header = "TK(4/10)";
                col.Width = 45;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                TextEditSettings txtDiem_HE4 = new TextEditSettings();
                col.EditSettings = txtDiem_HE4;
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                grd.Columns.Add(col); 
                #endregion

                #region 17
                col = new GridColumn();
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.FieldName = "DIEM_CHU";
                col.Header = "Điểm chữ";
                col.Width = 45;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                grd.Columns.Add(col); 
                #endregion

                grdView.AutoWidth = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //void LoadHocKy()
        //{
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("HOCKY", typeof(int));
        //    dt.Columns.Add("HOCKY_NAME", typeof(string));
        //    for (int i = 1; i <= 8; i++)
        //    {
        //        DataRow r = dt.NewRow();
        //        r["HOCKY"] = i;
        //        r["HOCKY_NAME"] = i;
        //        dt.Rows.Add(r);
        //    }
        //    ComboBoxUtil.SetComboBoxEdit(cboHocKy, "HOCKY_NAME", "HOCKY", dt, SelectionTypeEnum.Native);
        //    ComboBoxUtil.InsertItem(cboHocKy, "----------------------------Tất cả--------------------------", "0", 0, false);
        //    this.iDataSource.Rows[0]["HOCKY"] = cboHocKy.GetKeyValue(0);
        //}

        private void CboHDT_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                CboKhoa.ItemsSource = null;
                cboNganh.ItemsSource = null;
                cboMonHoc.ItemsSource = null;
                cboLopHocPhan.ItemsSource = null;

                iGridDataSource = null;

                if (this.iDataSource.Rows[0]["ID_HE_DAOTAO"].ToString() != string.Empty)
                {
                    int pid = Convert.ToInt32(this.iDataSource.Rows[0]["ID_HE_DAOTAO"].ToString());
                    DataTable dt = kehoach.GetAllKhoaHoc(pid);
                    if (dt.Rows.Count > 0)
                    {
                        ComboBoxUtil.SetComboBoxEdit(CboKhoa, "TEN_KHOAHOC", "ID_KHOAHOC", dt, SelectionTypeEnum.Native);
                        this.iDataSource.Rows[0]["ID_KHOAHOC"] = CboKhoa.GetKeyValue(0);
                    }
                    this.grd.ItemsSource = iGridDataSource;
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

        private void CboKhoa_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                cboNganh.ItemsSource = null;
                cboMonHoc.ItemsSource = null;
                cboLopHocPhan.ItemsSource = null;

                iGridDataSource = null;

                if (this.iDataSource.Rows[0]["ID_KHOAHOC"].ToString() != string.Empty)
                {
                    int pid = Convert.ToInt32(this.iDataSource.Rows[0]["ID_KHOAHOC"].ToString());
                    int idkhoahoc = pid;
                    DataTable dt = diem.GetAllKhoaNganh_ForDiem(idkhoahoc);
                    if (dt.Rows.Count > 0)
                    {
                        ComboBoxUtil.SetComboBoxEdit(cboNganh, "TEN_NGANH", "ID_KHOAHOC_NGANH", dt, SelectionTypeEnum.Native);
                        //ComboBoxUtil.InsertItem(cboNganh, "---------------------Tất cả--------------------", "0", 0, false);
                        this.iDataSource.Rows[0]["ID_KHOAHOC_NGANH"] = cboNganh.GetKeyValue(0);
                    }
                    this.grd.ItemsSource = iGridDataSource;
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

        private void CboNganh_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                cboMonHoc.ItemsSource = null;
                cboLopHocPhan.ItemsSource = null;

                iGridDataSource = null;

                if (!string.IsNullOrEmpty(this.iDataSource.Rows[0]["ID_KHOAHOC_NGANH"].ToString()))
                {
                    int idkhoanganh = Convert.ToInt32(this.iDataSource.Rows[0]["ID_KHOAHOC_NGANH"].ToString());
                    DataTable dt = diem.GetMonHocWhereKhoaNganh(idkhoanganh);
                    if (dt.Rows.Count > 0)
                    {
                        ComboBoxUtil.SetComboBoxEdit(cboMonHoc, "TEN_MONHOC", "ID_MONHOC", dt, SelectionTypeEnum.Native);
                        ComboBoxUtil.InsertItem(cboMonHoc, "---------------------Tất cả------------------------", "0", 0, false);
                        this.iDataSource.Rows[0]["ID_MONHOC"] = cboMonHoc.GetKeyValue(0);
                    }
                    this.grd.ItemsSource = iGridDataSource;
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

        private void CboMonHoc_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                cboLopHocPhan.ItemsSource = null;

                iGridDataSource = null;

                if (!string.IsNullOrEmpty(this.iDataSource.Rows[0]["ID_MONHOC"].ToString()))
                {
                    int idmonhoc = Convert.ToInt32(this.iDataSource.Rows[0]["ID_MONHOC"].ToString());
                    DataTable dt = diem.GetLopHocPhanWhereIdMonHoc(idmonhoc);
                    if (dt.Rows.Count > 0)
                    {
                        ComboBoxUtil.SetComboBoxEdit(cboLopHocPhan, "TEN_LOP_HOCPHAN", "ID_LOPHOCPHAN", dt, SelectionTypeEnum.Native);
                        this.iDataSource.Rows[0]["ID_LOPHOCPHAN"] = cboLopHocPhan.GetKeyValue(0);
                    }
                    this.grd.ItemsSource = iGridDataSource;
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

        private void CboLopHocPhan_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                iGridDataSource = null;

                if (!string.IsNullOrEmpty(this.iDataSource.Rows[0]["ID_LOPHOCPHAN"].ToString()))
                {
                    
                    int idlophocphan = Convert.ToInt32(this.iDataSource.Rows[0]["ID_LOPHOCPHAN"].ToString());
                    DataTable dt = diem.GetDanhSachSinhVienDK(idlophocphan);// lay danh sach sinh vien da dang ky lop hoc phan
                    #region Load mới khi chưa nhập dòng recore
                        if (dt.Rows.Count > 0)
                        {
                            iGridDataSource = TableSchemaBinding_GridDiem();
                            
                            foreach (DataRow r in dt.Rows)
                            {
                                string cachtinhdiem = r["CACH_TINHDIEM"].ToString();
                                DataRow dr = iGridDataSource.NewRow();
                                #region Bỏ
                                if (cachtinhdiem == "20-30-50")
                                {
                                    grd.Columns["DIEM_BT"].AllowEditing = DefaultBoolean.True;
                                    grd.Columns["DIEM_GK"].AllowEditing = DefaultBoolean.True;
                                    grd.Columns["DIEM_CK"].AllowEditing = DefaultBoolean.True;
                                    dr["DIEM_BT"] = (r["DIEM_BT"].ToString() == "" ? "0.0" : r["DIEM_BT"].ToString());
                                    dr["DIEM_GK"] = (r["DIEM_GK"].ToString() == "" ? "0.0" : r["DIEM_GK"].ToString());
                                    dr["DIEM_CK"] = (r["DIEM_CK"].ToString() == "" ? "0.0" : r["DIEM_CK"].ToString());
                                }
                                if (cachtinhdiem == "0-30-70")
                                {
                                    grd.Columns["DIEM_BT"].AllowEditing = DefaultBoolean.False;
                                    grd.Columns["DIEM_GK"].AllowEditing = DefaultBoolean.True;
                                    grd.Columns["DIEM_CK"].AllowEditing = DefaultBoolean.True;
                                    dr["DIEM_BT"] = DBNull.Value;
                                    dr["DIEM_GK"] = (r["DIEM_GK"].ToString() == "" ? "0.0" : r["DIEM_GK"].ToString());
                                    dr["DIEM_CK"] = (r["DIEM_CK"].ToString() == "" ? "0.0" : r["DIEM_CK"].ToString());
                                }
                                if (cachtinhdiem == "100")
                                {
                                    grd.Columns["DIEM_BT"].AllowEditing = DefaultBoolean.False;
                                    grd.Columns["DIEM_GK"].AllowEditing = DefaultBoolean.False;
                                    grd.Columns["DIEM_CK"].AllowEditing = DefaultBoolean.True;
                                    dr["DIEM_BT"] = DBNull.Value;
                                    dr["DIEM_GK"] = DBNull.Value;
                                    dr["DIEM_CK"] = (r["DIEM_CK"].ToString() == "" ? "0.0" : r["DIEM_CK"].ToString());
                                }
                                #endregion
                                
                                dr["DIEM_TONG"] = r["DIEM_TONG"];
                                dr["DIEM_HE4"] = r["DIEM_HE4"];
                                dr["DIEM_CHU"] = r["DIEM_CHU"];

                                dr["ID_KETQUA"] = r["ID_KETQUA"];
                                dr["ID_SINHVIEN"] = r["ID_SINHVIEN"];
                                dr["ID_LOPHOCPHAN"] = r["ID_LOPHOCPHAN"];
                                dr["ID_KHOAHOC"] = this.iDataSource.Rows[0]["ID_KHOAHOC"];
                                dr["ID_HOCKY"] = dangkyhocphan.GetHocKy(UserCommon.IdNamhocHkyHtai);
                                dr["MA_SINHVIEN"] = r["MA_SINHVIEN"];
                                dr["TEN_SINHVIEN"] = r["TEN_SINHVIEN"];
                                dr["TEN_LOP"] = r["TEN_LOP"];
                                dr["MA_MONHOC"] = r["MA_MONHOC"];
                                dr["TEN_MONHOC"] = r["TEN_MONHOC"];
                                dr["CACH_TINHDIEM"] = r["CACH_TINHDIEM"];
                                dr["ID_DANGKY"] = r["ID_DANGKY"];

                                iGridDataSource.Rows.Add(dr);
                                iGridDataSource.AcceptChanges();
                            }
                            this.grd.ItemsSource = iGridDataSource;
                        }
                        #endregion
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

        double TinhDiemTong(double bt, double gk, double ck, string cachtinh)
        {
            double result = 0.0;
            if (cachtinh == "20-30-50")
            {
                result = Math.Round((double)((bt*0.2) + (gk*0.3) + (ck*0.5)), 2);
            }
            if (cachtinh == "0-30-70")
            {
                result = Math.Round((double)((gk * 0.3) + (ck * 0.7)), 2);
            }
            if (cachtinh == "100")
            {
                result = Math.Round((double)(ck * 1), 2);
            }
            return result;
        }

        string QuiDoiDiemHe4(double diemtong)
        {
            string result = string.Empty;
            if (diemtong < 4.0)
            {
                result = "F";
            }
            else
            {
                if (diemtong < 5)
                {
                    result = "D";
                }
                else
                {
                    if (diemtong < 5.5)
                    {
                        result = "D+";
                    }
                    else
                    {
                        if (diemtong < 6.5)
                        {
                            result = "C";
                        }
                        else
                        {
                            if (diemtong < 7)
                            {
                                result = "C+";
                            }
                            else
                            {
                                if (diemtong < 9)
                                {
                                    result = "B";
                                }
                                else
                                {
                                    if (diemtong < 8.5)
                                    {
                                        result = "B+";
                                    }
                                    else
                                    {
                                        result = "A";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        double QuiDiemChu(string diemhe4)
        {
            double res = 0.0;
            if (diemhe4 == "A")
                res = 4;
            if (diemhe4 == "B+")
                res = 3.5;
            if (diemhe4 == "B")
                res = 3;
            if (diemhe4 == "C+")
                res = 2.5;
            if (diemhe4 == "C")
                res = 2;
            if (diemhe4 == "D+")
                res = 1.5;
            if (diemhe4 == "D")
                res = 1;
            if (diemhe4 == "F")
                res = 0;
            return res;
        }

        private void GrdView_OnCellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (this.grd.GetFocusedRow() == null)
                    return;
                int index = this.grdView.FocusedRowHandle;
                DataRow r = ((DataRowView)this.grd.GetFocusedRow()).Row;
               string cachtinh = r["CACH_TINHDIEM"].ToString();
                if (cachtinh == "20-30-50")
                {
                    if (Convert.ToDouble(r["DIEM_BT"].ToString()) < 0 || Convert.ToDouble(r["DIEM_BT"].ToString()) > 10)
                    {
                        CTMessagebox.Show("Điểm bài tập phải nằm trong khoảng 0->10", "Thông báo", "", CTICON.Error, CTBUTTON.OK);
                        grd.GetFocusedRow();
                        return;
                    }
                    if (Convert.ToDouble(r["DIEM_GK"].ToString()) < 0 || Convert.ToDouble(r["DIEM_GK"].ToString()) > 10)
                    {
                        CTMessagebox.Show("Điểm bài giữa kỳ phải nằm trong khoảng 0->10", "Thông báo", "", CTICON.Error, CTBUTTON.OK);
                        grd.GetFocusedRow();
                        return;
                    }
                    if (Convert.ToDouble(r["DIEM_CK"].ToString()) < 0 || Convert.ToDouble(r["DIEM_CK"].ToString()) > 10)
                    {
                        CTMessagebox.Show("Điểm bài cuối kỳ phải nằm trong khoảng 0->10", "Thông báo", "", CTICON.Error, CTBUTTON.OK);
                        grd.GetFocusedRow();
                        return;
                    }
                    this.iGridDataSource.Rows[index]["DIEM_TONG"] =
                    TinhDiemTong(Convert.ToDouble(r["DIEM_BT"].ToString()), Convert.ToDouble(r["DIEM_GK"].ToString()), Convert.ToDouble(r["DIEM_CK"].ToString()), cachtinh);
                    if (this.iGridDataSource.Rows[index]["DIEM_TONG"].ToString() != string.Empty)
                    {
                        this.iGridDataSource.Rows[index]["DIEM_CHU"] = QuiDoiDiemHe4(Convert.ToDouble(this.iGridDataSource.Rows[index]["DIEM_TONG"]));
                    }
                    this.iGridDataSource.Rows[index]["DIEM_HE4"] = QuiDiemChu(r["DIEM_CHU"].ToString());
                }
                if (cachtinh == "0-30-70")
                {
                    if (Convert.ToDouble(r["DIEM_GK"].ToString()) < 0 || Convert.ToDouble(r["DIEM_GK"].ToString()) > 10)
                    {
                        CTMessagebox.Show("Điểm bài giữa kỳ phải nằm trong khoảng 0->10", "Thông báo", "", CTICON.Error, CTBUTTON.OK);
                        grd.GetFocusedRow();
                        return;
                    }
                    if (Convert.ToDouble(r["DIEM_CK"].ToString()) < 0 || Convert.ToDouble(r["DIEM_CK"].ToString()) > 10)
                    {
                        CTMessagebox.Show("Điểm bài cuối kỳ phải nằm trong khoảng 0->10", "Thông báo", "", CTICON.Error, CTBUTTON.OK);
                        grd.GetFocusedRow();
                        return;
                    }
                    this.iGridDataSource.Rows[index]["DIEM_TONG"] =
                   TinhDiemTong(0, Convert.ToDouble(r["DIEM_GK"].ToString()), Convert.ToDouble(r["DIEM_CK"].ToString()), cachtinh);
                    if (this.iGridDataSource.Rows[index]["DIEM_TONG"].ToString() != string.Empty)
                    {
                        this.iGridDataSource.Rows[index]["DIEM_CHU"] = QuiDoiDiemHe4(Convert.ToDouble(this.iGridDataSource.Rows[index]["DIEM_TONG"]));
                    }
                    this.iGridDataSource.Rows[index]["DIEM_HE4"] = QuiDiemChu(r["DIEM_CHU"].ToString());
                }
                if (cachtinh == "100")
                {
                    if (Convert.ToDouble(r["DIEM_CK"].ToString()) < 0 || Convert.ToDouble(r["DIEM_CK"].ToString()) > 10)
                    {
                        CTMessagebox.Show("Điểm bài cuối kỳ phải nằm trong khoảng 0->10", "Thông báo", "", CTICON.Error, CTBUTTON.OK);
                        grd.GetFocusedRow();
                        return;
                    }
                    this.iGridDataSource.Rows[index]["DIEM_TONG"] = TinhDiemTong(0, 0, Convert.ToDouble(r["DIEM_CK"].ToString()), cachtinh);
                    if (this.iGridDataSource.Rows[index]["DIEM_TONG"].ToString() != string.Empty)
                    {
                        this.iGridDataSource.Rows[index]["DIEM_CHU"] = QuiDoiDiemHe4(Convert.ToDouble(this.iGridDataSource.Rows[index]["DIEM_TONG"]));
                    }
                    this.iGridDataSource.Rows[index]["DIEM_HE4"] = QuiDiemChu(r["DIEM_CHU"].ToString());
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

        private void BtnLuu_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (this.iGridDataSource.Rows.Count > 0)
                {
                    bool res = diem.InsertDiemSinhVien(iGridDataSource, UserCommon.UserName);
                    if (res)
                    {
                        CTMessagebox.Show("Nhập điểm thành công", "Nhập điểm", "", CTICON.Information, CTBUTTON.OK);
                        CboLopHocPhan_OnEditValueChanged(sender, null);
                    }
                    else
                    {
                        CTMessagebox.Show("Nhập điểm không thành công", "Nhập điểm", "", CTICON.Error, CTBUTTON.OK);
                        return;
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

        #region Trieu
        #region Bỏ
        //private void Load_data()
        //{
        //    try
        //    {
        //        iGridDataSource = diem.GetAll_DiemSV();
        //        grd.ItemsSource = iGridDataSource;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        #endregion
        private void Btnimport_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".xls";
            dlg.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                DataTable dtExcel = exceldata(filename);
                dtExcel.Columns.Add("ID_SINHVIEN", typeof(decimal));
                dtExcel.Columns.Add("ID_LOPHOCPHAN", typeof(decimal));

                DataTable dtSV = sv.GetAllSinhVien();
                DataTable dtDiemSV = diem.GetAll_DiemSV();
                DataTable dtlhp = lhp.Getall_lopHocPhan();
                //if (dtDiemSV.Rows.Count == 0)
                //{
                    //dtDiemSV.Columns.Add("ID_SINHVIEN", typeof(int));
                    //dtDiemSV.Columns.Add("ID_LOPHOCPHAN", typeof(int));
                //}
                DataTable dtNewInsert = dtExcel.Clone();
                foreach (DataRow dr in dtExcel.Rows)
                {
                    string id_sinhvien =
                        dtSV.Select("MA_SINHVIEN = '" + dr["f_masv"].ToString() + "'")[0]["ID_SINHVIEN"].ToString();
                    string id_lhp = dtlhp.Select("MA_LOP_HOCPHAN = '" + dr["f_mamhhtd"].ToString() + "'")[0]["ID_LOPHOCPHAN"].ToString();
                    if (IsCheck(dtDiemSV, id_sinhvien, id_lhp))
                    {
                        dr["ID_SINHVIEN"] = Convert.ToDecimal(id_sinhvien);
                        dr["ID_LOPHOCPHAN"] = Convert.ToDecimal(id_lhp);
                        dtNewInsert.ImportRow(dr);
                        DataRow m = dtDiemSV.NewRow();
                        m["ID_SINHVIEN"] = Convert.ToInt32(dr["ID_SINHVIEN"].ToString());
                        m["ID_LOPHOCPHAN"] = Convert.ToInt32(dr["f_mamhhtd"].ToString());
                        dtDiemSV.Rows.Add(m);
                    }
                }
                int isInsert = 0;
                if (dtNewInsert != null && dtNewInsert.Rows.Count > 0)
                {
                    isInsert = diem.InsertObject_Excel(dtNewInsert, iDataSource.Rows[0]["USER"].ToString());
                    if (isInsert != 0)
                    {
                        CTMessagebox.Show("Thành công", "Nhập từ Excel", "", CTICON.Information, CTBUTTON.OK);
                    }
                    else
                    {
                        CTMessagebox.Show("Lỗi", "Nhập từ Excel", "", CTICON.Information, CTBUTTON.OK);
                    }
                }
                else
                {
                    CTMessagebox.Show("Thành công", "Nhập từ Excel", "", CTICON.Information, CTBUTTON.OK);
                }
            }
        }

        private bool IsCheck(DataTable dtDiem, string id_sinhvien, string id_lophocphan)
        {
            if (dtDiem != null && dtDiem.Rows.Count > 0)
            {
                DataRow[] xcheck = dtDiem.Select("ID_SINHVIEN = " + id_sinhvien + " and ID_LOPHOCPHAN = " + id_lophocphan);
                if (xcheck.Count() > 0)
                    return false;
            }
            return true;
        }

        public DataTable exceldata(string filePath)
        {
            DataTable dtexcel = new DataTable();
            bool hasHeaders = false;
            string HDR = hasHeaders ? "Yes" : "No";
            string strConn;
            if (filePath.Substring(filePath.LastIndexOf('.')).ToLower() == ".xlsx")
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=0\"";
            else
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\"";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            foreach (DataRow schemaRow in schemaTable.Rows)
            {
                string sheet = schemaRow["TABLE_NAME"].ToString();
                if (!sheet.EndsWith("_"))
                {
                    string query = "SELECT f_mamhhtd,f_masv,f_diembt,f_diem1,f_diem2,f_diemtk1,f_diemch1,f_diemstk1 FROM [" + sheet + "] where f_mamhhtd in ('1200022-1-11-1','1201023-1-11-1', '1201023-1-11-2','1200001-1-11-1','1201002-1-11-1','18200013-1-11-1','19200004-1-11-1','1200003-1-11-1','19200001-1-11-1','18200001-1-11-1','1200006-2-11-1')";
                    OleDbDataAdapter daexcel = new OleDbDataAdapter(query, conn);
                    dtexcel.Locale = CultureInfo.CurrentCulture;
                    daexcel.Fill(dtexcel);
                }
            }
            conn.Close();
            return dtexcel;
        }

        private void BtnRefresh_OnClick(object sender, RoutedEventArgs e)
        {
        }
        #endregion
    }
}
