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
using CustomMessage;
using DATN.TTS.BUS;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_taotudonghocphan.xaml
    /// </summary>
    public partial class frm_taotudonghocphan : Window
    {
        bus_molophocphan bus = new bus_molophocphan();

        private DataTable iDataSource = null;
        private DataTable iDataSource_Tuan = null;

        public frm_taotudonghocphan()
        {
            InitializeComponent();
            this.iDataSource = TableChelmabinding();
            this.DataContext = this.iDataSource;
            Load_combo();
            
            iDataSource.Rows[0]["SOLOP"] = 1;
            if (frm_molophocphan.iSua == 1)
            {
                SolopEdit.IsReadOnly = true;
                rdbMaMon.IsEnabled = false;
                rdbddTenMon.IsEnabled = false;
                iDataSource.Rows[0]["IS_MA"] = true;
                iDataSource.Rows[0]["IS_TEN"] = true;
                NhapMaTextEdit.IsReadOnly = true;
                btnsave.Content = "Lưu thay đổi";
                iDataSource.Rows[0]["MA_HP"] = iDataSource.Rows[0]["MA_LOP_HOCPHAN"];
                iDataSource.Rows[0]["TEN_HP"] = iDataSource.Rows[0]["TEN_LOP_HOCPHAN"];
            }
            else
            {
                iDataSource.Rows[0]["IS_MA_TD"] = true;
                iDataSource.Rows[0]["IS_TEN_TD"] = true;
                iDataSource.Rows[0]["IS_TEN"] = false;
                iDataSource.Rows[0]["IS_MA"] = false;
                iDataSource.Rows[0]["TUAN_BD"] = 0;
                iDataSource.Rows[0]["TUAN_KT"] = 0;
                iDataSource.Rows[0]["SOLUONG"] = 0;
            }
            Load_Tuan();

            }

        private DataTable TableChelmabinding()
        {
            DataTable dtaTable = null;
            try
            {
                dtaTable = frm_molophocphan.idata_send.Copy();
                dtaTable.Columns.Add("SOLOP", typeof(decimal));
                dtaTable.Columns.Add("MA_THEM", typeof(string));
                dtaTable.Columns.Add("TEN_THEM", typeof(string));
                dtaTable.Columns.Add("MA_HP", typeof(string));
                dtaTable.Columns.Add("TEN_HP", typeof(string));

                dtaTable.Columns.Add("IS_MA_TD", typeof(bool));
                dtaTable.Columns.Add("IS_MA", typeof(bool));
                dtaTable.Columns.Add("IS_TEN_TD", typeof(bool));
                dtaTable.Columns.Add("IS_TEN", typeof(bool));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return dtaTable;
        }

        private void Load_Tuan()
        {
            try
            {
                iDataSource_Tuan = bus.GetAll_TUANHOC();
                if (iDataSource_Tuan != null && iDataSource_Tuan.Rows.Count > 0)
                {
                    TuanBDEdit.MinValue = Convert.ToInt32(iDataSource_Tuan.Rows[0]["TUAN_BDAU_HKY"]);
                    TuanBDEdit.MaxValue = Convert.ToInt32(iDataSource_Tuan.Rows[0]["TUAN_BDAU_HKY"]) + Convert.ToInt32(iDataSource_Tuan.Rows[0]["SO_TUAN"]) -1;
                    TuanKTEdit.MinValue = Convert.ToInt32(iDataSource_Tuan.Rows[0]["TUAN_BDAU_HKY"]);
                    TuanKTEdit.MaxValue = Convert.ToInt32(iDataSource_Tuan.Rows[0]["TUAN_BDAU_HKY"]) + Convert.ToInt32(iDataSource_Tuan.Rows[0]["SO_TUAN"]) -1;

                    iDataSource.Rows[0]["TUAN_BD"] = Convert.ToInt32(iDataSource_Tuan.Rows[0]["TUAN_BDAU_HKY"]);
                    iDataSource.Rows[0]["TUAN_KT"] = Convert.ToInt32(iDataSource_Tuan.Rows[0]["TUAN_BDAU_HKY"]) + Convert.ToInt32(iDataSource_Tuan.Rows[0]["SO_TUAN"]) -1 ;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Load_combo()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("CACHTINH", typeof(string));
                dt.Columns.Add("CACHTINH_NAME", typeof(string));

                DataRow dr =null;
                dr = dt.NewRow();
                dr["CACHTINH"] = "0";
                dr["CACHTINH_NAME"] = "-----Chọn-----";
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["CACHTINH"] = "20-30-50";
                dr["CACHTINH_NAME"] = "20% - 30% - 50%";
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["CACHTINH"] = "30-70";
                dr["CACHTINH_NAME"] = "30% - 70%";
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["CACHTINH"] = "100";
                dr["CACHTINH_NAME"] = "100%";
                dt.Rows.Add(dr);
                dt.AcceptChanges();
                
                cboCanhtinh.ItemsSource = dt;
                iDataSource.Rows[0]["CACH_TINHDIEM"] = "0";

                DataTable xdt = bus.GetAll_NAMHOC();
                cboNamhoc.ItemsSource = xdt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Btnsave_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (frm_molophocphan.iSua == 0)
                {
                    #region Them moi

                    int soluong = Convert.ToInt32(iDataSource.Rows[0]["SOLOP"]);
                    DataTable idata = frm_molophocphan.idata_send.Clone();

                    int ts = bus.Get_Thso_MaHP(Convert.ToInt32(iDataSource.Rows[0]["ID_MONHOC"]),
                        Convert.ToInt32(iDataSource.Rows[0]["ID_NAMHOC_HKY_HTAI"]));
                    if (soluong > 1)
                    {
                        #region Insert nhieu lop hoc phan

                        for (int i = 0; i < soluong; i++)
                        {
                            idata.Rows.Add(idata.NewRow());

                            foreach (DataColumn item in frm_molophocphan.idata_send.Columns)
                            {
                                if (this.iDataSource.Columns[item.ColumnName] != null)
                                {
                                    idata.Rows[i][item.ColumnName] =
                                        this.iDataSource.Rows[0][item.ColumnName];
                                }
                            }

                            if ((bool) iDataSource.Rows[0]["IS_MA_TD"] == true)
                            {
                                ts += 1;
                                idata.Rows[i]["MA_LOP_HOCPHAN"] =
                                    this.iDataSource.Rows[0]["MA_MONHOC"].ToString().Trim() +
                                    "-" +
                                    this.iDataSource.Rows[0]["HOCKY"].ToString().Trim() + "-" +
                                    this.iDataSource.Rows[0]["NAMHOC"].ToString().Substring(2, 2).Trim() + "-" +
                                    ts.ToString();
                                while (bus.Check_MaHocPhan(iDataSource.Rows[0]["MA_LOP_HOCPHAN"].ToString()))
                                {
                                    ts += 1;
                                    idata.Rows[i]["MA_LOP_HOCPHAN"] =
                                        this.iDataSource.Rows[0]["MA_MONHOC"].ToString().Trim() +
                                        "-" +
                                        this.iDataSource.Rows[0]["HOCKY"].ToString().Trim() + "-" +
                                        this.iDataSource.Rows[0]["NAMHOC"].ToString().Substring(2, 2).Trim() + "-" +
                                        ts.ToString();
                                }
                            }
                            if ((bool) iDataSource.Rows[0]["IS_TEN_TD"] == true)
                            {
                                idata.Rows[i]["TEN_LOP_HOCPHAN"] =
                                    this.iDataSource.Rows[0]["TEN_MONHOC"].ToString() +
                                    "-" +
                                    this.iDataSource.Rows[0]["HOCKY"].ToString().Trim() +
                                    "-" +
                                    this.iDataSource.Rows[0]["NAMHOC"].ToString()
                                        .Substring(2, 2)
                                        .Trim() + "- lớp " + ts.ToString();
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        #region Insert 1 lop hoc phan

                        idata.Rows.Add(idata.NewRow());

                        foreach (DataColumn item in frm_molophocphan.idata_send.Columns)
                        {
                            if (this.iDataSource.Columns[item.ColumnName] != null)
                            {
                                idata.Rows[0][item.ColumnName] = this.iDataSource.Rows[0][item.ColumnName];
                            }
                        }

                        if ((bool) iDataSource.Rows[0]["IS_MA_TD"] == true)
                        {
                            ts += 1;
                            idata.Rows[0]["MA_LOP_HOCPHAN"] =
                                this.iDataSource.Rows[0]["MA_MONHOC"].ToString().Trim() +
                                "-" +
                                this.iDataSource.Rows[0]["HOCKY"].ToString().Trim() + "-" +
                                this.iDataSource.Rows[0]["NAMHOC"].ToString().Substring(2, 2).Trim() + "-" +
                                ts.ToString();
                            while (bus.Check_MaHocPhan(idata.Rows[0]["MA_LOP_HOCPHAN"].ToString()))
                            {
                                ts += 1;
                                idata.Rows[0]["MA_LOP_HOCPHAN"] =
                                    this.iDataSource.Rows[0]["MA_MONHOC"].ToString().Trim() +
                                    "-" +
                                    this.iDataSource.Rows[0]["HOCKY"].ToString().Trim() + "-" +
                                    this.iDataSource.Rows[0]["NAMHOC"].ToString().Substring(2, 2).Trim() + "-" +
                                    ts.ToString();
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(iDataSource.Rows[0]["MA_HP"].ToString()))
                            {
                                CTMessagebox.Show("Mã học phần không được để trống", "Thông báo", "");
                                return;
                            }
                            else
                            {
                                idata.Rows[0]["MA_LOP_HOCPHAN"] = this.iDataSource.Rows[0]["MA_HP"].ToString();
                            }
                        }
                        if (bus.Check_MaHocPhan(idata.Rows[0]["MA_LOP_HOCPHAN"].ToString()))
                        {
                            CTMessagebox.Show("Mã học phần bị trùng", "Thông báo", "");
                            return;
                        }
                        if ((bool) iDataSource.Rows[0]["IS_TEN_TD"] == true)
                        {
                            idata.Rows[0]["TEN_LOP_HOCPHAN"] =
                                this.iDataSource.Rows[0]["TEN_MONHOC"].ToString() +
                                "-" +
                                this.iDataSource.Rows[0]["HOCKY"].ToString().Trim() +
                                "-" +
                                this.iDataSource.Rows[0]["NAMHOC"].ToString()
                                    .Substring(2, 2)
                                    .Trim() + "- lớp " + ts.ToString();
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(iDataSource.Rows[0]["TEN_HP"].ToString()))
                            {
                                CTMessagebox.Show("Tên học phần không được để trống", "Thông báo", "");
                                return;
                            }
                            else
                            {
                                idata.Rows[0]["TEN_LOP_HOCPHAN"] = this.iDataSource.Rows[0]["TEN_HP"].ToString();
                            }
                        }

                        #endregion

                    }

                    int tmp = 0;
                    tmp = bus.InsertObject(idata);
                    if (tmp != 0)
                    {
                        CTMessagebox.Show("Thành công", "Lưu", "", CTICON.Information, CTBUTTON.OK);
                    }

                    #endregion
                }
                else
                {
                    #region Sua

                    int tmp = 0;
                    iDataSource.Rows[0]["TEN_LOP_HOCPHAN"] = iDataSource.Rows[0]["TEN_HP"];
                    tmp = bus.UpdateObject(iDataSource);
                    if (tmp != 0)
                    {
                        CTMessagebox.Show("Thành công", "Lưu", "", CTICON.Information, CTBUTTON.OK);
                    }

                    #endregion
                }

                this.Close();
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lưu", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
        }

        private void CboNamhoc_OnSelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable xdt = bus.GetAll_HOCKY_ByNH(Convert.ToInt32(iDataSource.Rows[0]["NAMHOC"]));
                cbohocky.ItemsSource = xdt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Cbohocky_OnSelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable xdt = bus.GetAll_HKYHT(Convert.ToInt32(iDataSource.Rows[0]["NAMHOC"]), Convert.ToInt32(iDataSource.Rows[0]["HOCKY"]));
                iDataSource.Rows[0]["ID_NAMHOC_HKY_HTAI"] = xdt.Rows[0]["ID_NAMHOC_HKY_HTAI"];

                if (xdt != null && xdt.Rows.Count > 0)
                {
                    TuanBDEdit.MinValue = Convert.ToInt32(xdt.Rows[0]["TUAN_BDAU_HKY"]);
                    TuanBDEdit.MaxValue = Convert.ToInt32(xdt.Rows[0]["TUAN_BDAU_HKY"]) + Convert.ToInt32(xdt.Rows[0]["SO_TUAN"]) -1;
                    TuanKTEdit.MinValue = Convert.ToInt32(xdt.Rows[0]["TUAN_BDAU_HKY"]);
                    TuanKTEdit.MaxValue = Convert.ToInt32(xdt.Rows[0]["TUAN_BDAU_HKY"]) + Convert.ToInt32(xdt.Rows[0]["SO_TUAN"]) -1;

                    iDataSource.Rows[0]["TUAN_BD"] = Convert.ToInt32(xdt.Rows[0]["TUAN_BDAU_HKY"]);
                    iDataSource.Rows[0]["TUAN_KT"] = Convert.ToInt32(xdt.Rows[0]["TUAN_BDAU_HKY"]) + Convert.ToInt32(xdt.Rows[0]["SO_TUAN"]) -1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Btncancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
