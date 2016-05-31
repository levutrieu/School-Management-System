using System;
using System.Collections;
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
using DevExpress.Utils;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Editors;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_PhanCongGV.xaml
    /// </summary>
    public partial class frm_PhanCongGV : Page
    {
        private DataTable iDataSoure = null;
        private DataTable iGridDataSource = null;
        private ArrayList iDataChange = new ArrayList();

        public static int rowfocus = -1;
        public static int id_giaovien = 0;
        public static string ten_giaovien = "";

        bus_phanconggiaovien bus = new bus_phanconggiaovien();

        public frm_PhanCongGV()
        {
            InitializeComponent();
            iDataSoure = TableChelmabinding();
            this.DataContext = this.iDataSoure;
            this.iDataSoure.Rows[0]["USER"] = UserCommon.IdNhanVien.ToString();
            Init_Grid();
            Load_data();
        }

        private DataTable TableChelmabinding()
        {
            DataTable dtaTable = null;
            try
            {
                Dictionary<string, Type> xDicUser = new Dictionary<string, Type>();
                xDicUser.Add("USER", typeof(decimal));
                xDicUser.Add("ID_BOMON", typeof(int));
                xDicUser.Add("MA_BM", typeof(string));
                xDicUser.Add("TEN_BM", typeof(string));
                dtaTable = TableUtil.ConvertToTable(xDicUser);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return dtaTable;
        }

        private void Init_Grid()
        {
            try
            {
                GridColumn xcolumn;

                xcolumn = new GridColumn();
                xcolumn.FieldName = "ID_LOPHOCPHAN";
                xcolumn.Visible = false;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "ID_GIANGVIEN";
                xcolumn.Visible = false;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn() { CellTemplate = (DataTemplate)this.Resources["btn"] };
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.Header = "Xóa GV";
                xcolumn.Width = 90;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "TEN_GIANGVIEN";
                xcolumn.Header = "Tên giảng viên";
                xcolumn.Width = 150;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "MA_LOP_HOCPHAN";
                xcolumn.Header = "Mã học phần";
                xcolumn.Width = 90;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "TEN_LOP_HOCPHAN";
                xcolumn.Header = "Tên học phần";
                xcolumn.Width = 150;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "TEN_LOP";
                xcolumn.Header = "Lớp kiểm soát";
                xcolumn.Width = 150;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "SOTIET";
                xcolumn.Header = "Số tiết";
                xcolumn.Width = 50;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "SOLUONG";
                xcolumn.Header = "Số sinh viên";
                xcolumn.Width = 50;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "TUAN_BD";
                xcolumn.Header = "Tuần bắt đầu";
                xcolumn.Width = 50;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "TUAN_KT";
                xcolumn.Header = "Tuần kết thúc";
                xcolumn.Width = 50;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "TEN_HE_DAOTAO";
                xcolumn.Header = "Hệ đào tạo";
                xcolumn.Width = 90;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "TEN_NGANH";
                xcolumn.Header = "Ngành";
                xcolumn.Width = 150;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "TEN_BM";
                xcolumn.Header = "Bộ môn";
                xcolumn.Width = 150;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void Load_data()
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                iGridDataSource = bus.GetAll_Hocphan();
                grd.ItemsSource = iGridDataSource;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void btndelete_Click(object sender, RoutedEventArgs e)
        {
            if (rowfocus != -1)
            {
                if (string.IsNullOrEmpty(iGridDataSource.Rows[rowfocus]["ID_GIANGVIEN"].ToString()) ||
                    Convert.ToInt32(iGridDataSource.Rows[rowfocus]["ID_GIANGVIEN"]) == 0) return;
                string gv = iGridDataSource.Rows[rowfocus]["TEN_GIANGVIEN"].ToString();
                if (CTMessagebox.Show("Bạn muốn xóa giảng viên " + gv + " không ?", "Xóa giảng viên", "",
                    CTICON.Question,
                    CTBUTTON.YesNo) == CTRESPONSE.Yes)
                {
                    iGridDataSource.Rows[rowfocus]["TEN_GIANGVIEN"] = "";
                    iGridDataSource.Rows[rowfocus]["ID_GIANGVIEN"] = 0;

                    int xcheck = 0;
                    foreach (object d in iDataChange)
                    {
                        int chk = Convert.ToInt32(d.ToString());
                        if (Convert.ToInt32(this.iGridDataSource.Rows[rowfocus]["ID_LOPHOCPHAN"]) == chk)
                        {
                            xcheck = 1;
                        }
                    }
                    if (xcheck != 1)
                    {
                        iDataChange.Add(Convert.ToInt32(this.iGridDataSource.Rows[rowfocus]["ID_LOPHOCPHAN"]));
                    }
                }
            }
        }

        private void Grd_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int rowHandle = -1;

            if (this.grd.GetFocusedRow() == null) return;
            if (iGridDataSource.Rows.Count == 0) return;
            DataRow RowSelGb = ((DataRowView)this.grd.GetFocusedRow()).Row;
            for (int i = 0; i < iGridDataSource.Rows.Count; i++)
            {
                if (Convert.ToInt32(iGridDataSource.Rows[i]["ID_LOPHOCPHAN"]) ==
                    Convert.ToInt32(RowSelGb["ID_LOPHOCPHAN"]))
                {
                    rowHandle = i;
                    break;
                }
            }
            if (rowHandle != -1)
            {
                frm_PhanCongGV_popup popup = new frm_PhanCongGV_popup();
                popup.ShowDialog();
                if (id_giaovien != 0)
                {
                    this.iGridDataSource.Rows[rowHandle]["ID_GIANGVIEN"] = id_giaovien;
                    this.iGridDataSource.Rows[rowHandle]["TEN_GIANGVIEN"] = ten_giaovien;
                }

                int xcheck = 0;
                foreach (object d in iDataChange)
                {
                    int chk = Convert.ToInt32(d.ToString());
                    if (Convert.ToInt32(this.iGridDataSource.Rows[rowfocus]["ID_LOPHOCPHAN"]) == chk)
                    {
                        xcheck = 1;
                    }
                }
                if (xcheck != 1)
                {
                    iDataChange.Add(Convert.ToInt32(this.iGridDataSource.Rows[rowfocus]["ID_LOPHOCPHAN"]));
                }
            }
        }

        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.iDataChange != null)
                {
                    int kq = 0;

                    foreach (object id in iDataChange)
                    {
                        foreach (DataRow dtr in iGridDataSource.Rows)
                        {
                            int d = Convert.ToInt32(id);
                            if (Convert.ToInt32(dtr["ID_LOPHOCPHAN"]) == d)
                            {
                                kq = bus.UpdateObject(Convert.ToInt32(dtr["ID_LOPHOCPHAN"].ToString()),
                                    Convert.ToInt32(dtr["ID_GIANGVIEN"].ToString()),
                                    iDataSoure.Rows[0]["USER"].ToString());
                            }
                        }
                    }
                    if (kq != 0)
                    {
                        CTMessagebox.Show("Thành công", "Lưu", "", CTICON.Information, CTBUTTON.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GrdView_OnFocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                DataRow RowSelGb = null;
                if (this.grd.GetFocusedRow() == null) return;
                RowSelGb = ((DataRowView)this.grd.GetFocusedRow()).Row;
                for (int i = 0; i < iGridDataSource.Rows.Count; i++)
                {
                    if (string.IsNullOrEmpty(iGridDataSource.Rows[i]["ID_LOPHOCPHAN"].ToString())) continue;
                    if (Convert.ToInt32(iGridDataSource.Rows[i]["ID_LOPHOCPHAN"]) ==
                        Convert.ToInt32(RowSelGb["ID_LOPHOCPHAN"]))
                    {
                        rowfocus = i;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Btnrefresh_OnClick(object sender, RoutedEventArgs e)
        {
            Load_data();
        }
    }
}
