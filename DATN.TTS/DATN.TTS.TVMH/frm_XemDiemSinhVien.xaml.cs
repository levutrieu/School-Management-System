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
using DevExpress.Utils;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_XemDiemSinhVien.xaml
    /// </summary>
    public partial class frm_XemDiemSinhVien : Page
    {
        bus_DangKyHocPhan client = new bus_DangKyHocPhan();
        private DataTable iDataSoure = null;
        private DataTable iGridDataSoure = null;
        public frm_XemDiemSinhVien()
        {
            InitializeComponent();
            this.iDataSoure = TableSchemaBinding();
            this.DataContext = iDataSoure;
            //this.iDataSoure.Rows[0]["MA_SINHVIEN"] = "2001120021";
            InitDiem();
        }

        DataTable TableSchemaBinding()
        {
            try
            {
                Dictionary<string, Type> dic = new Dictionary<string, Type>();
                dic.Add("ID_NAMHOC_HKY_HTAI", typeof(int));
                dic.Add("NAME_TUAN", typeof(string));
                dic.Add("TUAN", typeof(int));
                dic.Add("ID_SINHVIEN", typeof(Decimal));
                dic.Add("MA_SINHVIEN", typeof(string));
                dic.Add("TEN_SINHVIEN", typeof(string));
                dic.Add("TEN_NGANH", typeof(string));
                dic.Add("TEN_KHOAHOC", typeof(string));
                dic.Add("TEN_HE_DAOTAO", typeof(string));
                dic.Add("KHOAHOC", typeof(string));
                dic.Add("TEN_LOP", typeof(string));
                DataTable dt = null;
                dt = TableUtil.ConvertToTable(dic);
                return dt;
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            return null;
        }

        void InitDiem()
        {
            try
            {
                TreeListColumn col;

                #region
                col = new TreeListColumn();
                col.FieldName = "ID";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = false;
                
                GridDiem.Columns.Add(col);

                col = new TreeListColumn();
                col.Header = "Tên môn học";
                col.FieldName = "NAME";
                col.Width = 200;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                
                GridDiem.Columns.Add(col); 

                col = new TreeListColumn();
                col.FieldName = "MA_MONHOC";
                col.Header = "Mã môn học";
                col.Width = 70;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                
                GridDiem.Columns.Add(col);

               
                #endregion

                col = new TreeListColumn();
                col.FieldName = "SO_TC";
                col.Header = "Số TC";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                GridDiem.Columns.Add(col);

                col = new TreeListColumn();
                col.FieldName = "CACH_TINHDIEM";
                col.Header = "Tính điểm";
                col.Width = 70;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                GridDiem.Columns.Add(col);

                col = new TreeListColumn();
                col.FieldName = "DIEM_BT";
                col.Header = "% Bài tập";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                GridDiem.Columns.Add(col);

                col = new TreeListColumn();
                col.FieldName = "DIEM_GK";
                col.Header = "% Giữa kỳ";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                GridDiem.Columns.Add(col);

                col = new TreeListColumn();
                col.FieldName = "DIEM_CK";
                col.Header = "% Cuối kỳ";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                GridDiem.Columns.Add(col);

                col = new TreeListColumn();
                col.FieldName = "DIEM_TONG";
                col.Header = "Điểm tổng";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                GridDiem.Columns.Add(col);

                col = new TreeListColumn();
                col.FieldName = "DIEM_CHU";
                col.Header = "Điểm chữ";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                GridDiem.Columns.Add(col);


                col = new TreeListColumn();
                col.FieldName = "DIEM_HE4";
                col.Header = "Điểm hệ 4";
                col.Width = 50;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                GridDiem.Columns.Add(col);

                GridViewDiem.AutoWidth = true;
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
        }

        double TinhDiemHe10(DataTable xdt)
        {
            try
            {
                double res = 0;
                int stc = 0;
                foreach (DataRow r in xdt.Rows)
                {
                    res += Convert.ToDouble(r["DIEM_TONG"].ToString())*Convert.ToInt32(r["SO_TC"].ToString());
                    stc += Convert.ToInt32(r["SO_TC"].ToString());
                }
                return Math.Round(((double)res / stc), 2);
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            return double.NaN;
        }

        double TinhDiemTong(DataTable xdt)
        {
            double res = 0;
            int stc = 0;
            foreach (DataRow r in xdt.Rows)
            {
                string temp = r["DIEM_CHU"].ToString().ToUpper().Trim();
                if (temp.Equals("A"))
                {
                    res += Convert.ToDouble(r["SO_TC"].ToString()) * 4;
                    stc += Convert.ToInt32(r["SO_TC"].ToString());
                }
                if (temp.Equals("B+"))
                {
                    res += Convert.ToDouble(r["SO_TC"].ToString()) * 3.5;
                    stc += Convert.ToInt32(r["SO_TC"].ToString());
                }
                if (temp.Equals("B"))
                {
                    res += Convert.ToDouble(r["SO_TC"].ToString()) * 3;
                    stc += Convert.ToInt32(r["SO_TC"].ToString());
                }
                if (temp.Equals("C+"))
                {
                    res += Convert.ToDouble(r["SO_TC"].ToString()) * 2.5;
                    stc += Convert.ToInt32(r["SO_TC"].ToString());
                }
                if (temp.Equals("C"))
                {
                    res += Convert.ToDouble(r["SO_TC"].ToString()) * 2;
                    stc += Convert.ToInt32(r["SO_TC"].ToString());
                }
                if (temp.Equals("D+"))
                {
                    res += Convert.ToDouble(r["SO_TC"].ToString()) * 1.5;
                    stc += Convert.ToInt32(r["SO_TC"].ToString());
                }
                if (temp.Equals("D"))
                {
                    res += Convert.ToDouble(r["SO_TC"].ToString()) * 1;
                    stc += Convert.ToInt32(r["SO_TC"].ToString());
                }
                if (temp.Equals("F"))
                {
                    res += Convert.ToDouble(r["SO_TC"].ToString()) * 0;
                    stc += Convert.ToInt32(r["SO_TC"].ToString());
                }
            }
            return Math.Round(((double)res / stc), 2);
        }

        int TinhSOTCDat(DataTable xdt)
        {
            int stc = 0;
            try
            {
                
                foreach (DataRow r in xdt.Rows)
                {
                    string temp = r["DIEM_CHU"].ToString().ToUpper().Trim();
                    if (!temp.Equals("F"))
                    {
                        stc += Convert.ToInt32(r["SO_TC"].ToString());
                    }

                }
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            return stc;
        }

        void LoadDiemToGrid(int idsinhvien)
        {
            try
            {
                DataTable dt = client.GetDanhSachDiem(idsinhvien);
                DataTable xdt = dt.Clone();
                #region 
                foreach (DataRow r in dt.Rows)
                {
                    if (r["ID_PARENT"].ToString() == "")
                    {
                        r["MA_MONHOC"] = "";
                        r["SO_TC"] = DBNull.Value;
                        r["DIEM_BT"] = DBNull.Value;
                        r["DIEM_GK"] = DBNull.Value;
                        r["DIEM_CK"] = DBNull.Value;
                        r["DIEM_TONG"] = DBNull.Value;
                        r["DIEM_HE4"] = DBNull.Value;
                        r["DIEM_CHU"] = DBNull.Value;
                        r["ID_PARENT"] = DBNull.Value;

                        xdt.ImportRow(r);
                    }
                    else
                    {
                        xdt.ImportRow(r);
                    }
                } 
                #endregion
                DataTable dtcopy = xdt.Copy();
                int count = 1;
                foreach (DataRow xdr in xdt.Rows)
                {
                    if (xdr["ID_PARENT"].ToString() == "")
                    {
                        DataTable zdt = new DataTable();
                        string x = (xdr["ID"].ToString());
                        DataRow[] row = (from temp in xdt.AsEnumerable()
                                    .Where(t => t.Field<string>("ID_PARENT") == x)
                                         select temp).ToArray();
                        if (row.Count() > 0)
                        {
                            zdt = row.CopyToDataTable();
                            double res = TinhDiemTong(zdt);
                            double diemtong = TinhDiemHe10(zdt);

                            DataRow r = null;

                            #region 
                            r = dtcopy.NewRow();
                            r["ID"] = x + r["ID"].ToString() + (count + 12121);
                            r["MA_MONHOC"] = "";
                            r["SO_TC"] = DBNull.Value;
                            r["DIEM_BT"] = DBNull.Value;
                            r["DIEM_GK"] = DBNull.Value;
                            r["DIEM_CK"] = DBNull.Value;
                            r["DIEM_TONG"] = DBNull.Value;
                            r["DIEM_HE4"] = DBNull.Value;
                            r["DIEM_CHU"] = DBNull.Value;
                            r["ID_PARENT"] = x;
                            r["NAME"] = "";
                            dtcopy.Rows.Add(r);

                            r = dtcopy.NewRow();
                            r["ID"] = x + r["ID"].ToString() + (count + 11222);
                            r["MA_MONHOC"] = "";
                            r["SO_TC"] = DBNull.Value;
                            r["DIEM_BT"] = DBNull.Value;
                            r["DIEM_GK"] = DBNull.Value;
                            r["DIEM_CK"] = DBNull.Value;
                            r["DIEM_TONG"] = DBNull.Value;
                            r["DIEM_HE4"] = DBNull.Value;
                            r["DIEM_CHU"] = DBNull.Value;
                            r["ID_PARENT"] = x;
                            r["NAME"] = "";
                            dtcopy.Rows.Add(r); 
                            #endregion

                            r = dtcopy.NewRow();
                            r["ID"] = x + r["ID"].ToString() + (count + 9999);
                            r["MA_MONHOC"] = "";
                            r["SO_TC"] = DBNull.Value;
                            r["DIEM_BT"] = DBNull.Value;
                            r["DIEM_GK"] = DBNull.Value;
                            r["DIEM_CK"] = DBNull.Value;
                            r["DIEM_TONG"] = DBNull.Value;
                            r["DIEM_HE4"] = DBNull.Value;
                            r["DIEM_CHU"] = DBNull.Value;
                            r["ID_PARENT"] = x;
                            r["NAME"] = "Điểm tổng hệ 4:   " + res;
                            dtcopy.Rows.Add(r);

                            r = dtcopy.NewRow();
                            r["ID"] = x + r["ID"].ToString() + (count + 7173183);
                            r["MA_MONHOC"] = "";
                            r["SO_TC"] = DBNull.Value;
                            r["DIEM_BT"] = DBNull.Value;
                            r["DIEM_GK"] = DBNull.Value;
                            r["DIEM_CK"] = DBNull.Value;
                            r["DIEM_TONG"] = DBNull.Value;
                            r["DIEM_HE4"] = DBNull.Value;
                            r["DIEM_CHU"] = DBNull.Value;
                            r["ID_PARENT"] = x;
                            r["NAME"] = "Điểm tổng hệ 10:   " + diemtong;
                            dtcopy.Rows.Add(r);

                            r = dtcopy.NewRow();
                            r["ID"] = x + r["ID"].ToString() + (count + 453453);
                            r["MA_MONHOC"] = "";
                            r["SO_TC"] = DBNull.Value;
                            r["DIEM_BT"] = DBNull.Value;
                            r["DIEM_GK"] = DBNull.Value;
                            r["DIEM_CK"] = DBNull.Value;
                            r["DIEM_TONG"] = DBNull.Value;
                            r["DIEM_HE4"] = DBNull.Value;
                            r["DIEM_CHU"] = DBNull.Value;
                            r["ID_PARENT"] = x;
                            r["NAME"] = "Số TC đã đạt:   " + TinhSOTCDat(zdt);
                            dtcopy.Rows.Add(r);

                            r = dtcopy.NewRow();
                            #region 
                            r["ID"] = x + r["ID"].ToString() + (count + 1000);
                            r["MA_MONHOC"] = "";
                            r["SO_TC"] = DBNull.Value;
                            r["DIEM_BT"] = DBNull.Value;
                            r["DIEM_GK"] = DBNull.Value;
                            r["DIEM_CK"] = DBNull.Value;
                            r["DIEM_TONG"] = DBNull.Value;
                            r["DIEM_HE4"] = DBNull.Value;
                            r["DIEM_CHU"] = DBNull.Value;
                            r["ID_PARENT"] = x;
                            r["NAME"] = "";
                            dtcopy.Rows.Add(r); 
                            #endregion
                        }
                        
                        
                    }
                    count++;
                }
                iGridDataSoure = dtcopy.Copy();
                GridDiem.ItemsSource = iGridDataSoure;
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
        }

        private void BtnXemDiem_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (this.iDataSoure.Rows[0]["MA_SINHVIEN"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập mã sinh viên để thực hiện các thao tác tiếp theo!!", "Thông báo",
                        "", CTICON.Information, CTBUTTON.YesNo);
                    txtMASV.Focus();
                    return;
                }
                int idsinhvien = client.GetIDSinhVien(this.iDataSoure.Rows[0]["MA_SINHVIEN"].ToString().Trim());
                if (idsinhvien == 0)
                {
                    CTMessagebox.Show("Không tìm thấy sinh viên này trong hệ thống!!" + "\n" + "Vui lòng thử lại.!!",
                        "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                    txtMASV.Focus();
                    return;
                }
                this.iDataSoure.Rows[0]["ID_SINHVIEN"] = idsinhvien;

                DataTable iDataThongTin = client.GetThongTinSinhVien(idsinhvien);
                if (iDataThongTin.Rows.Count > 0)
                {
                    this.iDataSoure.Rows[0]["MA_SINHVIEN"] = iDataThongTin.Rows[0]["MA_SINHVIEN"];
                    this.iDataSoure.Rows[0]["TEN_SINHVIEN"] = iDataThongTin.Rows[0]["TEN_SINHVIEN"];
                    this.iDataSoure.Rows[0]["TEN_NGANH"] = iDataThongTin.Rows[0]["TEN_NGANH"];
                    this.iDataSoure.Rows[0]["TEN_KHOAHOC"] = iDataThongTin.Rows[0]["TEN_KHOAHOC"];
                    this.iDataSoure.Rows[0]["TEN_HE_DAOTAO"] = iDataThongTin.Rows[0]["TEN_HE_DAOTAO"];
                    this.iDataSoure.Rows[0]["KHOAHOC"] = iDataThongTin.Rows[0]["KHOAHOC"];
                    this.iDataSoure.Rows[0]["TEN_LOP"] = iDataThongTin.Rows[0]["TEN_LOP"];
                }
                LoadDiemToGrid(idsinhvien);
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void BtnXuatDiem_OnClick(object sender, RoutedEventArgs e)
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
                ((TreeListView)GridDiem.View).ExportToXls(@"D:\DataExport\DiemSinhVien.xls", options);
                sw.Stop();
                CTMessagebox.Show("File đã được lưu trên D:DataExport");
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
        }
    }
}
