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
using DevExpress.Utils;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_KhoaHoc_Nganh.xaml
    /// </summary>
    public partial class frm_KhoaHoc_Nganh : Page
    {
        bus_KhoaHoc_Nganh client = new bus_KhoaHoc_Nganh();
        private DataTable iDataSoure = null;
        private DataTable iGridDataSoure = null;
        private bool flagsave = true;
        public frm_KhoaHoc_Nganh()
        {
            InitializeComponent();
            this.iDataSoure = TableSchemaBinding();
            this.DataContext = iDataSoure;

            this.iDataSoure.Rows[0]["USER"] = "admin";
            InitGrid();
            SetComBo();
        }

        void InitGrid()
        {
            GridColumn col;

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
            col.AllowCellMerge = false;
            grd.Columns.Add(col);


            col = new GridColumn();
            col.FieldName = "ID_NGANH";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "TEN_KHOAHOC";
            col.Header = "Khóa";
            col.Width = 30;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "TEN_NGANH";
            col.Header = "Ngành";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "SO_HKY";
            col.AllowCellMerge = true;
            col.Header = "Số học kỳ";
            col.Width = 30;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "SO_SINHVIEN_DK";
            col.AllowCellMerge = true;
            col.Header = "Số sinh viên dự kiến";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "SO_LOP";
            col.Header = "Số lớp";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "HOCKY_TRONGKHOA";
            col.Header = "Các học kỳ";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "GHICHU";
            col.Header = "Ghi chú";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            col.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            col.AllowCellMerge = false;
            grd.Columns.Add(col);

            grdViewNDung.AutoWidth = true;
            GetGrid();
        }

        void SetComBo()
        {
            cbbKhoa.ItemsSource = client.GetAllKhoaHoc();
            cbbNganh.ItemsSource = client.GetAllNganh();
        }

        void SetIsNull()
        {
            this.iDataSoure.Rows[0]["ID_KHOAHOC"] = "0";
            this.iDataSoure.Rows[0]["ID_NGANH"] = "0";
            this.iDataSoure.Rows[0]["SO_HKY"] = 0;
            this.iDataSoure.Rows[0]["SO_SINHVIEN_DK"] = 0;
            this.iDataSoure.Rows[0]["SO_LOP"] = 0;
            this.iDataSoure.Rows[0]["HOCKY_TRONGKHOA"] = string.Empty;
            this.iDataSoure.Rows[0]["GHICHU"] = string.Empty;
        }

        bool ValiDate()
        {
            if (this.iDataSoure.Rows[0]["ID_KHOAHOC"].ToString() == string.Empty)
            {
                CTMessagebox.Show("Vui lòng chọn khóa học!", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                cbbKhoa.Focus();
                return false;
            }
            if (this.iDataSoure.Rows[0]["ID_NGANH"].ToString() == string.Empty)
            {
                CTMessagebox.Show("Vui lòng chọn ngành!", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                cbbNganh.Focus();
                return false;
            }
            if (this.iDataSoure.Rows[0]["SO_HKY"].ToString() == "0")
            {
                CTMessagebox.Show("Vui lòng nhập số học kỳ", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                txtSoHK.Focus();
                return false;
            }
            if (this.iDataSoure.Rows[0]["SO_SINHVIEN_DK"].ToString() == "0")
            {
                CTMessagebox.Show("Vui lòng nhập số sinh viên dự kiến", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                txtSoSVDK.Focus();
                return false;
            }
            if (this.iDataSoure.Rows[0]["SO_LOP"].ToString() == "0")
            {
                CTMessagebox.Show("Vui lòng nhập số lớp dự kiến", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                txtSoLopMo.Focus();
                return false;
            }
            return true;
        }

        void GetGrid()
        {
            this.iGridDataSoure = client.GetAllKhoaNganh();
            grd.ItemsSource = this.iGridDataSoure;
        }

        DataTable TableSchemaBinding()
        {
            try
            {
                DataTable dt = null;
                Dictionary<string, Type> dic = new Dictionary<string, Type>();
                dic.Add("ID_KHOAHOC_NGANH", typeof(Decimal));
                dic.Add("ID_KHOAHOC", typeof(int));
                dic.Add("ID_NGANH", typeof(int));
                dic.Add("SO_HKY", typeof(Decimal));
                dic.Add("SO_SINHVIEN_DK", typeof(Decimal));
                dic.Add("SO_LOP", typeof(Decimal));
                dic.Add("HOCKY_TRONGKHOA", typeof(string));
                dic.Add("GHICHU", typeof(string));
                dic.Add("USER", typeof(string));
                dic.Add("TEN_NGANH", typeof (string));
                dic.Add("TEN_KHOAHOC", typeof(string));
                dt = TableUtil.ConvertToTable(dic);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnAddNew_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                GetGrid();
                SetIsNull();
                cbbKhoa.Focus();
                flagsave = true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnSave_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValiDate())
                {
                    if (flagsave)
                    {
                        bool res = client.Insert_Khoa_Nganh(this.iDataSoure.Copy());
                        if(!res)
                        {
                            CTMessagebox.Show("Thêm mới không thành công", "Thêm mới", "", CTICON.Information, CTBUTTON.YesNo);
                        }
                        GetGrid();
                        SetIsNull();
                        cbbKhoa.Focus();
                    }
                    else
                    {
                        client.Update_Khoa_Nganh(this.iDataSoure.Copy());
                        GetGrid();
                        SetIsNull();
                        cbbKhoa.Focus();
                    }
                }
            }
            catch (Exception err)
            {
                CTMessagebox.Show("Lỗi", "Lưu", "", CTICON.Information, CTBUTTON.YesNo);
            }
        }

        private void btnDelete_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CTMessagebox.Show("Bạn muốn xóa?", "Xóa", "", CTICON.Information, CTBUTTON.YesNo) == CTRESPONSE.Yes)
                {
                    client.Delete_Khoa_Nganh(this.iDataSoure.Copy());
                    GetGrid();
                    SetIsNull();
                    cbbKhoa.Focus();
                }
            }
            catch (Exception)
            {
               throw;
            }
        }

        private void btnRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                GetGrid();
                SetIsNull();
                cbbKhoa.Focus();
            }
            catch (Exception)
            {
                throw;
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
                r = ((DataRowView)this.grd.GetFocusedRow()).Row;
                this.iDataSoure.Rows[0]["ID_KHOAHOC_NGANH"] = r["ID_KHOAHOC_NGANH"];
                this.iDataSoure.Rows[0]["ID_KHOAHOC"] = r["ID_KHOAHOC"];
                this.iDataSoure.Rows[0]["ID_NGANH"] = r["ID_NGANH"];
                this.iDataSoure.Rows[0]["SO_HKY"] = r["SO_HKY"];
                this.iDataSoure.Rows[0]["HOCKY_TRONGKHOA"] = r["HOCKY_TRONGKHOA"];
                this.iDataSoure.Rows[0]["SO_SINHVIEN_DK"] = r["SO_SINHVIEN_DK"];
                this.iDataSoure.Rows[0]["GHICHU"] = r["GHICHU"];
                this.iDataSoure.Rows[0]["SO_LOP"] = r["SO_LOP"];
                flagsave = false;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void TxtSoHK_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                int SO_HKY = Convert.ToInt32(txtSoHK.Text);
                string HOCKY_TRONGKHOA = string.Empty;
                for (int i = 1; i <= SO_HKY; i++)
                {
                    if (i == 8)
                    {
                        HOCKY_TRONGKHOA += i;
                    }
                    else
                    {
                        HOCKY_TRONGKHOA += i + ", ";
                    }
                }
                this.iDataSoure.Rows[0]["HOCKY_TRONGKHOA"] = HOCKY_TRONGKHOA;
                }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}
