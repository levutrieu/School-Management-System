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
        private DataTable iDataSource = null;

        public frm_taotudonghocphan()
        {
            InitializeComponent();
            this.iDataSource = TableChelmabinding();
            this.DataContext = this.iDataSource;
            Load_combo();

            iDataSource.Rows[0]["IS_MA_TD"] = true;
            iDataSource.Rows[0]["IS_TEN_TD"] = true;
            iDataSource.Rows[0]["IS_TEN"] = false;
            iDataSource.Rows[0]["IS_MA"] = false;
            iDataSource.Rows[0]["TUAN_BD"] = 0;
            iDataSource.Rows[0]["TUAN_KT"] = 0;
            iDataSource.Rows[0]["SOTIET"] = 0;
            iDataSource.Rows[0]["SOLUONG"] = 0;
        }

        private DataTable TableChelmabinding()
        {
            DataTable dtaTable = null;
            try
            {
                dtaTable = frm_molophocphan.idata_send.Copy();
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Btnsave_OnClick(object sender, RoutedEventArgs e)
        {
            bus_molophocphan bus = new bus_molophocphan();

            if ((bool) iDataSource.Rows[0]["IS_MA_TD"] == true)
            {
                int ts = bus.Get_Thso_MaHP(Convert.ToInt32(iDataSource.Rows[0]["ID_MONHOC"]),
                    Convert.ToInt32(iDataSource.Rows[0]["ID_NAMHOC_HKY_HTAI"]));
                this.iDataSource.Rows[0]["MA_LOP_HOCPHAN"] = this.iDataSource.Rows[0]["MA_MONHOC"].ToString().Trim() +
                                                             "-" +
                                                             this.iDataSource.Rows[0]["HOCKY"].ToString().Trim() + "-" +
                                                             this.iDataSource.Rows[0]["NAMHOC"].ToString().Substring(2, 2).Trim() + "-" +
                                                             (ts + 1).ToString();
                while(bus.Check_MaHocPhan(iDataSource.Rows[0]["MA_LOP_HOCPHAN"].ToString()))
                {
                    ts += 1;
                    this.iDataSource.Rows[0]["MA_LOP_HOCPHAN"] = this.iDataSource.Rows[0]["MA_MONHOC"].ToString().Trim() +
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
                    this.iDataSource.Rows[0]["MA_LOP_HOCPHAN"] = this.iDataSource.Rows[0]["MA_HP"].ToString();
                }
            }
            if (bus.Check_MaHocPhan(iDataSource.Rows[0]["MA_LOP_HOCPHAN"].ToString()))
            {
                CTMessagebox.Show("Mã học phần bị trùng", "Thông báo", "");
                return;
            }
            if ((bool) iDataSource.Rows[0]["IS_TEN_TD"] == true)
            {
                if (string.IsNullOrEmpty(iDataSource.Rows[0]["TEN_THEM"].ToString()))
                {
                    this.iDataSource.Rows[0]["TEN_LOP_HOCPHAN"] = this.iDataSource.Rows[0]["TEN_MONHOC"].ToString() +
                                                                  "-" +
                                                                  this.iDataSource.Rows[0]["HOCKY"].ToString().Trim() +
                                                                  "-" +
                                                                  this.iDataSource.Rows[0]["NAMHOC"].ToString()
                                                                      .Substring(2, 2)
                                                                      .Trim();
                }
                else
                {
                    this.iDataSource.Rows[0]["TEN_LOP_HOCPHAN"] = this.iDataSource.Rows[0]["TEN_MONHOC"].ToString() +
                                                                  "-" +
                                                                  this.iDataSource.Rows[0]["HOCKY"].ToString().Trim() +
                                                                  "-" +
                                                                  this.iDataSource.Rows[0]["NAMHOC"].ToString()
                                                                      .Substring(2, 2)
                                                                      .Trim() + "-" +
                                                                  this.iDataSource.Rows[0]["TEN_THEM"].ToString().Trim();
                }
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
                    this.iDataSource.Rows[0]["TEN_LOP_HOCPHAN"] = this.iDataSource.Rows[0]["TEN_HP"].ToString();
                }
            }

            foreach (DataColumn item in frm_molophocphan.idata_send.Columns)
            {
                if (this.iDataSource.Columns[item.ColumnName] != null)
                {
                    frm_molophocphan.idata_send.Rows[0][item.ColumnName] = this.iDataSource.Rows[0][item.ColumnName];
                }
            }
            int tmp = 0;
            tmp = bus.InsertObject(frm_molophocphan.idata_send);
            if (tmp != 0)
            {
                CTMessagebox.Show("Thành công", "Lưu", "", CTICON.Information, CTBUTTON.OK);
            }
            
            this.Close();
        }
    }
}
