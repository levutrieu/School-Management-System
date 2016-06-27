using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using DATN.TTS.BUS.Resource;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_TimPhongTrong.xaml
    /// </summary>
    public partial class frm_TimPhongTrong : Window
    {
        public class BoolStringClass
        {
            public int Id { get; set; }
            public string TheText { get; set; }
            public bool IsSelected { get; set; }
        }

        bus_PhanCong_TKB client = new bus_PhanCong_TKB();

        public bool flagsave = true;

        public bool res;

        DataTable iDataSource = null;

        public DataTable iDataReturn = new DataTable();

        DataTable iGridDataSource = null;
        
        public ObservableCollection<BoolStringClass> TheList { get; set; }

        public frm_TimPhongTrong(params object[] oParams)
        {
            InitializeComponent();
            this.MinHeight = 600;
            this.MaxWidth = 1200;
            this.iDataSource = TableSchemaDefineBingding();
            this.DataContext = this.iDataSource;
            this.iDataSource.Rows[0]["USER"] = UserCommon.UserName;

            #region Load check thứ
            TheList = new ObservableCollection<BoolStringClass>();
            TheList.Add(new BoolStringClass { TheText = "Thứ 2", IsSelected = false, Id = 2 });
            TheList.Add(new BoolStringClass { TheText = "Thứ 3", IsSelected = false, Id = 3 });
            TheList.Add(new BoolStringClass { TheText = "Thứ 4", IsSelected = false, Id = 4 });
            TheList.Add(new BoolStringClass { TheText = "Thứ 5", IsSelected = false, Id = 5 });
            TheList.Add(new BoolStringClass { TheText = "Thứ 6", IsSelected = false, Id = 6 });
            TheList.Add(new BoolStringClass { TheText = "Thứ 7", IsSelected = false, Id = 7 });
            TheList.Add(new BoolStringClass { TheText = "Chủ Nhật", IsSelected = false, Id = 8 });
            #endregion

            lstThu.ItemsSource = TheList;
            InitGrid();

            #region OnPopUp
            if (oParams[0] != null)
            {
                DataTable dt = (DataTable) oParams[0];
                flagsave = (bool) oParams[1];
                DataRow r = dt.Rows[0];
                this.iDataSource.Rows[0]["ID_LOPHOCPHAN"] = r["ID_LOPHOCPHAN"];
                this.iDataSource.Rows[0]["TIET_BD"] = r["TIET_BD"];
                this.iDataSource.Rows[0]["TIET_KT"] = r["TIET_KT"];
                this.iDataSource.Rows[0]["SO_TIET"] = r["SO_TIET"];
                this.iDataSource.Rows[0]["THU"] = r["THU"];
                this.iDataSource.Rows[0]["ID_LOP_HOCPHAN_CTIET"] = r["ID_LOP_HOCPHAN_CTIET"];
                this.iDataSource.Rows[0]["ID_PHONG"] = r["ID_PHONG"];
                if (flagsave)
                {
                    CheckChonThu.IsReadOnly = true;
                    CheckChonTiet.IsReadOnly = true;
                    int tietbd = Convert.ToInt32(r["TIET_BD"].ToString());
                    int tietkt = Convert.ToInt32(r["TIET_KT"].ToString());
                    int thu = Convert.ToInt32(r["THU"].ToString());

                    DataTable xdt = client.GetPhongWhereThuTiet(thu, tietbd, tietkt);
                    xdt.Columns.Add("IsNew");
                    iGridDataSource = xdt.Copy();
                    grd.ItemsSource = iGridDataSource;
                }
                else
                {
                    int tietbd = Convert.ToInt32(r["TIET_BD"].ToString());
                    int tietkt = Convert.ToInt32(r["TIET_KT"].ToString());
                    int thu = Convert.ToInt32(r["THU"].ToString());

                    DataTable xdt = client.GetPhongWhereThuTiet(thu, tietbd, tietkt);
                    xdt.Columns.Add("IsNew");
                    iGridDataSource = xdt.Copy();
                    grd.ItemsSource = iGridDataSource;
                }
            #endregion
            }
            if (oParams[1] == null)
            {
                btnSave.IsEnabled = false;
            }
        }

        void InitGrid()
        {
            GridColumn col = null;

            col = new GridColumn();
            col.FieldName = "IsNew";
            col.Header = string.Empty;
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.True;
            col.UnboundType = UnboundColumnType.Boolean;
            col.Visible = true;
            col.Width = 10;
            col.EditSettings = new CheckEditSettings();
            
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "ID_PHONG";
            col.Header = string.Empty;
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = false;
            
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "MA_PHONG";
            col.Header = "Mã phòng";
            col.Width = 50;
            col.AutoFilterValue = true;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "TEN_PHONG";
            col.Header = "Phòng";
            col.Width = 80;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "SUCCHUA";
            col.Header = "Sức chứa";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "LOAIPHONG";
            col.Header = "Loại phòng";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "DAY";
            col.Header = "Dãy";
            col.Width = 40;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.EditSettings = new TextEditSettings { DisplayFormat = "dd/MM/yyyy" };
            col.Visible = true;
            
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "TANG";
            col.Header = "Tầng";
            col.Width = 40;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            grd.Columns.Add(col);

            col = new GridColumn();
            col.FieldName = "GHICHU";
            col.Header = "Ghi chú";
            col.Width = 50;
            col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            col.AllowEditing = DefaultBoolean.False;
            col.Visible = true;
            
            grd.Columns.Add(col);
        }

        private DataTable TableSchemaDefineBingding()
        {
            DataTable xDt = null;
            try
            {
                Dictionary<string, Type> dic = new Dictionary<string, Type>();
                dic.Add("USER", typeof(string));

                dic.Add("ID_LOP_HOCPHAN_CTIET", typeof(Decimal));
                dic.Add("ID_LOPHOCPHAN", typeof(Decimal));
                dic.Add("TUAN_BD", typeof(Decimal));
                dic.Add("TUAN_KT", typeof(Decimal));
                dic.Add("SO_TIET", typeof(Decimal));
                //dic.Add("pNgayBatDau", typeof(string));
                //dic.Add("pNgayKetThuc", typeof(string));
                dic.Add("TIET_BD", typeof(Decimal));
                dic.Add("TIET_KT", typeof(Decimal));
                dic.Add("THU", typeof(Decimal));
                dic.Add("ID_PHONG", typeof(Decimal));
                xDt = TableUtil.ConvertToTable(dic);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return xDt;

        }

        private void BtnAddNew_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void GrdView_OnFocusedRowChanged(object sender, DevExpress.Xpf.Grid.FocusedRowChangedEventArgs e)
        {

        }

        private void CheckChonTiet_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                BorChonTiet.IsEnabled = true;  
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CheckChonTiet_Unchecked(object sender, RoutedEventArgs e)
        {
            BorChonTiet.IsEnabled = false;
            spTietBatDau.Value = 0;
            spTietKetThuc.Value = 0;
        }

        private void CheckChonThu_Checked(object sender, RoutedEventArgs e)
        {
            bor_ChonThu.IsEnabled = true;
        }

        private void CheckChonThu_Unchecked(object sender, RoutedEventArgs e)
        {
            bor_ChonThu.IsEnabled = false;
        }

        private void BtnTim_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                for (int i = 0; i < TheList.Count; i++)
                {
                    if (TheList[i].IsSelected == true)
                    {
                        this.iDataSource.Rows[0]["THU"] = TheList[i].Id;
                    }
                }
                iGridDataSource = client.GetPhongWhereThuTiet(Convert.ToInt32(this.iDataSource.Rows[0]["THU"].ToString()), Convert.ToInt32(this.iDataSource.Rows[0]["TIET_BD"].ToString()),
                        Convert.ToInt32(this.iDataSource.Rows[0]["TIET_KT"].ToString()));
                iGridDataSource.Columns.Add("IsNew");
                grd.ItemsSource = iGridDataSource.Copy();
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

        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                //foreach (DataRow r in iGridDataSource.Rows)
                //{
                //    if (r["IsNew"].Equals("True"))
                //    {
                //        this.iDataSource.Rows[0]["ID_PHONG"] = r["ID_PHONG"];
                //    }
                //}
                DataTable dt = (from temp in iGridDataSource.AsEnumerable() where (temp.Field<string>("IsNew") == "True")select temp).CopyToDataTable();
                this.iDataSource.Rows[0]["ID_PHONG"] = dt.Rows[0]["ID_PHONG"];
                #region thực hiện insert
                if (flagsave)
                {
                    res = client.Insert_LopHocPhan_CT(this.iDataSource.Copy());
                    if (!res)
                    {
                        CTMessagebox.Show("Thêm mới không thành công", "Thêm mới", "", CTICON.Information, CTBUTTON.YesNo);
                    }
                    else
                    {
                        CTMessagebox.Show("Thêm mới thành công", "Thêm mới", "", CTICON.Information, CTBUTTON.YesNo);
                        this.Close();
                    }
                }
                #endregion
                else
                {
                    res = client.Update_LopHocPhan_CT(this.iDataSource.Copy());
                    if (!res)
                    {
                        CTMessagebox.Show("Cập nhật không thành công", "Cập nhật", "", CTICON.Information, CTBUTTON.YesNo);
                    }
                    else
                    {
                        CTMessagebox.Show("Cập nhật thành công", "Cập nhật", "", CTICON.Information, CTBUTTON.YesNo);
                        this.Close();
                    }
                    
                }
                
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

        private void GrdView_OnCellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                int index = this.grdView.FocusedRowHandle;
                for (int i = 0; i < this.iGridDataSource.Rows.Count; i++)
                {
                    if (i != index)
                    {
                        this.iGridDataSource.Rows[i]["IsNew"] = "False";
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void GrdView_OnCellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                int index = this.grdView.FocusedRowHandle;
                for (int i = 0; i < this.iGridDataSource.Rows.Count; i++)
                {
                    if (i != index)
                    {
                        this.iGridDataSource.Rows[i]["IsNew"] = "False";
                    }
                }
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
    }
}
