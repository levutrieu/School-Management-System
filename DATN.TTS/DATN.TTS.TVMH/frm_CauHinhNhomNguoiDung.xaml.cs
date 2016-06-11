using System;
using System.Collections.Generic;
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
using System.Data;
using CustomMessage;
using DATN.TTS.BUS;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_CauHinhNhomNguoiDung.xaml
    /// </summary>
    public partial class frm_CauHinhNhomNguoiDung : Page
    {
        private DataTable iDataSoure = null;

        private DataTable iGridDataSoureNhom = null;

        private DataTable iGridDataSoureUI = null;

        bus_CauHinhNhom client = new bus_CauHinhNhom();

        private bool flagSave = true;

        public frm_CauHinhNhomNguoiDung()
        {
            InitializeComponent();
            iDataSoure = TableChelmabinding();
            this.DataContext = iDataSoure;
            Init_GridNhom();
            Init_GridUI();

            txtMaManHinh.IsReadOnly = true;
        }

        private void Init_GridNhom()
        {
            try
            {
                GridColumn xcolumn;

                xcolumn = new GridColumn();
                xcolumn.FieldName = "MaNhomNguoiDung";
                xcolumn.Header = "Mã nhóm";
                xcolumn.Width = 50;
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                xcolumn.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "TenNhomNguoiDUng";
                xcolumn.Header = "Nhóm người dùng";
                xcolumn.Width = 50;
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                xcolumn.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grd.Columns.Add(xcolumn);

                //iGridDataSoureNhom = client.GetAllNhom();
                //grd.ItemsSource = iGridDataSoureNhom;
                LoadGridNhom();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void Init_GridUI()
        {
            try
            {
                GridColumn xcolumn;

                xcolumn = new GridColumn();
                xcolumn.FieldName = "IsNew";
                xcolumn.Header = "Chọn";
                xcolumn.Width = 50;
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.AllowEditing = DefaultBoolean.True;
                xcolumn.UnboundType = UnboundColumnType.Boolean;
                xcolumn.Visible = true;
                xcolumn.Width = 10;
                xcolumn.EditSettings = new CheckEditSettings();
                xcolumn.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grdUI.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "CoQuyen";
                xcolumn.Header = "Quyền";
                xcolumn.Width = 50;
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.AllowEditing = DefaultBoolean.True;
                xcolumn.UnboundType = UnboundColumnType.Integer;
                xcolumn.Visible = false;
                xcolumn.Width = 10;
                xcolumn.EditSettings = new CheckEditSettings();
                xcolumn.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grdUI.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "MaManHinh";
                xcolumn.Header = "MaNhomNguoiDung";
                xcolumn.Width = 50;
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = false;
                xcolumn.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                
                grdUI.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "TenManHinh";
                xcolumn.Header = "Màn hình";
                xcolumn.Width = 50;
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                xcolumn.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                xcolumn.EditSettings = new TextEditSettings();
                xcolumn.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Left;
                grdUI.Columns.Add(xcolumn);

                iGridDataSoureUI = client.GetAllManHinh();
                grdUI.ItemsSource = iGridDataSoureUI;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private DataTable TableChelmabinding()
        {
            DataTable dtaTable = null;
            try
            {
                Dictionary<string, Type> xDicUser = new Dictionary<string, Type>();
                xDicUser.Add("MaNhomNguoiDung", typeof(string));
                xDicUser.Add("TenNhomNguoiDUng", typeof(string));
                xDicUser.Add("MaManHinh", typeof(string));
                xDicUser.Add("GhiChu", typeof(string));
                xDicUser.Add("CoQuyen", typeof(Decimal));
                dtaTable = TableUtil.ConvertToTable(xDicUser);
            }
            catch (Exception)
            {

                throw;
            }
            return dtaTable;
        }

        private void SetNullValue()
        {
            this.iDataSoure.Rows[0]["MaNhomNguoiDung"] = string.Empty;
            this.iDataSoure.Rows[0]["TenNhomNguoiDUng"] = string.Empty;
            this.iDataSoure.Rows[0]["GhiChu"] = string.Empty;
        }

        private bool Validate()
        {
            //if (this.iDataSoure.Rows[0]["MaNhomNguoiDung"].ToString().Trim() == string.Empty)
            //{
            //    MessageBox.Show("Vui lòng nhập mã nhóm", "Thông báo");
            //    txtMaManHinh.Focus();
            //    return false;
            //}
            if (this.iDataSoure.Rows[0]["TenNhomNguoiDUng"].ToString().Trim() == string.Empty)
            {
                MessageBox.Show("Vui lòng nhập tên nhóm", "Thông báo");
                txtTenMH.Focus();
                return false;
            }
            return true;
        }

        private void LoadGridNhom()
        {
            iGridDataSoureNhom = client.GetAllNhom();
            grd.ItemsSource = iGridDataSoureNhom;
        }

        private void GrdViewNDung_OnMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void btnRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadGridNhom();
                SetNullValue();
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
                DataTable table = iGridDataSoureUI.Copy();
                if (Validate())
                {
                    if (flagSave)
                    {
                        bool res = client.Insert_Nhom(iDataSoure.Copy());
                        if (!res)
                        {
                            CTMessagebox.Show("Lưu dữ liệu thất bại", "Lưu", "", CTICON.Information, CTBUTTON.YesNo);
                        }
                        else
                        {
                            #region Insert recore in table phân quyền
                            foreach (DataRow dr in table.Rows)
                            {
                                if (dr["IsNew"].ToString() == "True")
                                {
                                    if (dr["CoQuyen"].ToString() == "True")
                                    {
                                        this.iDataSoure.Rows[0]["CoQuyen"] = 1;
                                    }
                                    else
                                    {
                                        this.iDataSoure.Rows[0]["CoQuyen"] = 0;
                                    }
                                    //this.iDataSoure.Rows[0]["MaNhomNguoiDung"] = d;
                                    this.iDataSoure.Rows[0]["MaManHinh"] = dr["MaManHinh"];

                                    res = client.Insert_PhanQuyenUI(this.iDataSoure.Copy());
                                    if (!res)
                                    {
                                        CTMessagebox.Show("Lưu dữ liệu thất bại", "Lưu", "", CTICON.Information, CTBUTTON.YesNo);
                                    }
                                }
                            }
                            #endregion
                        }
                        LoadGridNhom();
                    }
                    else
                    {
                        #region Update Group and insert or delete authorization

                        bool res;
                        client.Update_Nhom(this.iDataSoure.Copy());
                        foreach (DataRow dr in table.Rows)
                        {
                            DataTable dt = client.GetPhanQuyenOne(this.iDataSoure.Rows[0]["MaNhomNguoiDung"].ToString().Trim(), dr["MaManHinh"].ToString().Trim());
                            if (dr["IsNew"].ToString() == "True")
                            {
                                if (dt.Rows.Count <= 0)
                                {
                                    if (dr["CoQuyen"].ToString() == "True")
                                    {
                                        this.iDataSoure.Rows[0]["CoQuyen"] = 1;
                                    }
                                    else
                                    {
                                        this.iDataSoure.Rows[0]["CoQuyen"] = 0;
                                    }
                                    this.iDataSoure.Rows[0]["MaManHinh"] = dr["MaManHinh"];

                                    res = client.Insert_PhanQuyenUI(this.iDataSoure.Copy());
                                    if (!res)
                                    {
                                        CTMessagebox.Show("Lưu dữ liệu thất bại", "Lưu", "", CTICON.Information, CTBUTTON.YesNo);
                                    }
                                }
                            }
                            else
                            {
                                if (dr["IsNew"].ToString() == "False")
                                {
                                    if (dt.Rows.Count > 0)
                                    {
                                        #region note never use
                                        //if (dr["CoQuyen"].ToString() == "True")
                                        //{
                                        //    this.iDataSoure.Rows[0]["CoQuyen"] = 1;
                                        //}
                                        //else
                                        //{
                                        //    this.iDataSoure.Rows[0]["CoQuyen"] = 0;
                                        //}
                                        #endregion
                                        this.iDataSoure.Rows[0]["MaManHinh"] = dr["MaManHinh"];
                                        try
                                        {
                                            client.Delete_PhanQuyenUI(this.iDataSoure.Copy());
                                        }
                                        catch (Exception)
                                        {
                                            throw;
                                        }
                                        
                                    }
                                }
                            }
                        }
                        LoadGridNhom();
                        #endregion
                    }
                }
               
                
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void btnDelete_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if ( CTMessagebox.Show("Bạn có muốn xóa không?", "Xóa", "", CTICON.Information, CTBUTTON.YesNo) == CTRESPONSE.Yes)
                {
                    client.Delete_Nhom(this.iDataSoure.Rows[0]["MaNhomNguoiDung"].ToString().Trim());
                    LoadGridNhom();
                }
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
                txtMaManHinh.IsReadOnly = false;
                flagSave = true;
                SetNullValue();
                txtMaManHinh.Focus();
                iGridDataSoureUI = client.GetAllManHinh();
                grdUI.ItemsSource = iGridDataSoureUI;
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
                
                DataRow row = null;
                if (this.grd.GetFocusedRow() == null)
                    return;
                row = ((DataRowView) this.grd.GetFocusedRow()).Row;

                this.iDataSoure.Rows[0]["MaNhomNguoiDung"] = row["MaNhomNguoiDung"];
                this.iDataSoure.Rows[0]["TenNhomNguoiDUng"] = row["TenNhomNguoiDUng"];
                this.iDataSoure.Rows[0]["GhiChu"] = row["GhiChu"];
                if (!string.IsNullOrEmpty(this.iDataSoure.Rows[0]["MaNhomNguoiDung"].ToString()))
                {
                    DataTable idt = client.GetAllManHinh();
                    DataTable dt = client.GetPhanQuyenByWhere(row["MaNhomNguoiDung"].ToString());

                    DataTable xdt = new DataTable();
                    xdt.Columns.Add("IsNew");
                    xdt.Columns.Add("MaManHinh");
                    xdt.Columns.Add("TenManHinh");
                    xdt.Columns.Add("CoQuyen");
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            foreach (DataRow idr in idt.Rows)
                            {
                                DataRow xdr = xdt.NewRow();
                                if (dr["MaManHinh"].ToString().Trim().Contains(idr["MaManHinh"].ToString().Trim()))
                                {
                                    xdr["IsNew"] = "True";
                                    xdr["MaManHinh"] = idr["MaManHinh"];
                                    xdr["TenManHinh"] = idr["TenManHinh"];
                                    if (dr["CoQuyen"].ToString() == "1")
                                    {
                                        xdr["CoQuyen"] = "True";
                                    }
                                    if (dr["CoQuyen"].ToString() == "0")
                                    {
                                        xdr["CoQuyen"] = "False";
                                    }
                                    xdt.Rows.Add(xdr);
                                }
                                else
                                {
                                    xdr["IsNew"] = "False";
                                    xdr["MaManHinh"] = idr["MaManHinh"];
                                    xdr["TenManHinh"] = idr["TenManHinh"];
                                    xdr["CoQuyen"] = "False";
                                    xdt.Rows.Add(xdr);
                                }
                                xdt.AcceptChanges();
                            }
                        }
                        if (xdt.Rows.Count > 0)
                        {
                            iGridDataSoureUI = xdt.AsEnumerable().OrderByDescending(a => a.Field<string>("IsNew"))
                                                                .GroupBy(a => a.Field<string>("MaManHinh")).Select(group => group.First()).CopyToDataTable();
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        iGridDataSoureUI = idt;
                    }
                }
                flagSave = false;
                txtMaManHinh.IsReadOnly = true;
                grdUI.ItemsSource = iGridDataSoureUI;
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

        private void GrdViewUI_OnCellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                //int index = this.grdViewUI.FocusedRowHandle;
                //for (int i = 0; i < iGridDataSoureUI.Rows.Count; i++)
                //{
                //    if(i == index)
                //}
                //if (this.iGridDataSoureUI.Rows[index]["IsNew"].ToString() == "True")
                //    this.iGridDataSoureUI.Rows[index]["CoQuyen"] = 1;
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

        private void GrdViewUI_OnCellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            //Mouse.OverrideCursor = Cursors.Wait;
            int index = this.grdViewUI.FocusedRowHandle;
            if (this.iGridDataSoureUI.Rows[index]["IsNew"].ToString() == "True")
                this.iGridDataSoureUI.Rows[index]["CoQuyen"] = "True";

            if (this.iGridDataSoureUI.Rows[index]["IsNew"].ToString() == "False")
                this.iGridDataSoureUI.Rows[index]["CoQuyen"] = "False";
        }
    }
}
