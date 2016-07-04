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
using DevExpress.Utils;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_TietHoc.xaml
    /// </summary>
    public partial class frm_TietHoc : Page
    {
        bus_TietHoc client = new bus_TietHoc();
        private DataTable iDataSoure = null;
        private DataTable iGridDataSoure = null;
        private bool flagsave = true;
        public frm_TietHoc()
        {
            InitializeComponent();
            this.iDataSoure = TableSchemaBinding();
            this.DataContext = iDataSoure;

            this.iDataSoure.Rows[0]["USER"] = UserCommon.UserName;


            InitGrid();
        }

        public bool KiemTraGioHopLe(string GioBd, string GioKT)
        {
            try
            {
                //string[] mangBD = GioBd.Split(':');
                //string[] mangKT = GioKT.Split(':');

                DateTime tBegin = Convert.ToDateTime(GioBd);
                DateTime tEnd = Convert.ToDateTime(GioKT);
                TimeSpan ts = new TimeSpan();
                ts = (TimeSpan)(tEnd - tBegin);

                string temp = ts.ToString();
                string[] mang = temp.Split(':');

                int phut = int.Parse(mang[1]);
                if (phut == 45)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            return false;
        }

        private void InitGrid()
        {
            GridColumn col;

            col = new GridColumn();
            col.FieldName = "ID_TIETHOC";
            col.Header = string.Empty;
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "TEN_TIETHOC";
            col.Header = "Tiết";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "GIO_BD";
            col.Header = "Giờ bắt đầu";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "GIO_KT";
            col.Header = "Giờ kết thúc";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "CA";
            col.Header = "Ca";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            grd.Columns.Add(col);

            GetGrid();
        }

        private void GetGrid()
        {
            this.iGridDataSoure = client.GetAll();
            grd.ItemsSource = iGridDataSoure;
        }

        private DataTable TableSchemaBinding()
        {
            try
            {
                DataTable dt = null;
                Dictionary<string, Type> dic = new Dictionary<string, Type>();
                dic.Add("ID_TIETHOC", typeof(string));
                dic.Add("TEN_TIETHOC", typeof(string));
                dic.Add("GIO_BD", typeof(string));
                dic.Add("GIO_KT", typeof(string));
                dic.Add("CA", typeof(string));
                dic.Add("USER", typeof(string));
                dt = TableUtil.ConvertToTable(dic);
                return dt;
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            return null;
        }

        private void SetIsNull()
        {
            try
            {
                this.iDataSoure.Rows[0]["TEN_TIETHOC"] = string.Empty;
                this.iDataSoure.Rows[0]["GIO_BD"] = string.Empty;
                this.iDataSoure.Rows[0]["GIO_KT"] = string.Empty;
                this.iDataSoure.Rows[0]["CA"] = string.Empty;
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
        }

        private bool ValiDate()
        {
            try
            {
                if (this.iDataSoure.Rows[0]["TEN_TIETHOC"].ToString() == string.Empty)
                {
                    MessageBox.Show("Vui lòng nhập");
                    txtTietHoc.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["GIO_BD"].ToString() == string.Empty)
                {
                    MessageBox.Show("Vui lòng nhập");
                    txtGioBD.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["GIO_KT"].ToString() == string.Empty)
                {
                    MessageBox.Show("Vui lòng nhập");
                    txtGioKT.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["CA"].ToString() == string.Empty)
                {
                    MessageBox.Show("Vui lòng nhập");
                    cbbCa.Focus();
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            return false;
        }

        private void btnAddNew_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                GetGrid();
                SetIsNull();
                this.iDataSoure.Rows[0]["GIO_BD"] = "07:00:00";
                flagsave = true;
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
        }

        private void btnSave_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (ValiDate())
                {
                    if (flagsave)
                    {
                        bool res = client.Insert_Khoa(this.iDataSoure.Copy());
                        if (!res)
                        {
                            MessageBox.Show("Thêm mới không thành công", "Thêm mới");
                        }
                        GetGrid();
                        SetIsNull();
                    }
                    else
                    {
                        client.Update_Khoa(this.iDataSoure.Copy());
                        GetGrid();
                        SetIsNull();
                    }
                }
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

        private void btnDelete_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (MessageBox.Show("Bạn có muốn xóa không?", "Xóa", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    client.Delete_Khoa(this.iDataSoure.Copy());
                    GetGrid();
                    SetIsNull();
                }
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

        private void btnRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                KiemTraGioHopLe(this.iDataSoure.Rows[0]["GIO_BD"].ToString(), this.iDataSoure.Rows[0]["GIO_KT"].ToString());

                GetGrid();
                SetIsNull();
                txtTietHoc.Focus();
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

        private void GrdViewNDung_OnFocusedRowChanged(object sender, DevExpress.Xpf.Grid.FocusedRowChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                DataRow r = null;
                if (this.grd.GetFocusedRow() == null)
                    return;
                r = ((DataRowView)grd.GetFocusedRow()).Row;
                this.iDataSoure.Rows[0]["ID_TIETHOC"] = r["ID_TIETHOC"];
                this.iDataSoure.Rows[0]["TEN_TIETHOC"] = r["TEN_TIETHOC"];
                this.iDataSoure.Rows[0]["GIO_BD"] = r["GIO_BD"];
                this.iDataSoure.Rows[0]["GIO_KT"] = r["GIO_KT"];
                this.iDataSoure.Rows[0]["CA"] = r["CA"];

                flagsave = false;
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

        private void TxtGioBD_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            this.iDataSoure.Rows[0]["GIO_KT"] = "07:45:00";
        }
    }
}
