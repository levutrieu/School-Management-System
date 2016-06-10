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
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_CauHinhNguoiDung.xaml
    /// </summary>
    public partial class frm_CauHinhNguoiDung : Page
    {
        private DataTable iDataSoure = null;

        private DataTable iGridDataSoure = null;

        private DataTable iGridDataSoureND = null;

        private bool flagSave;

        public frm_CauHinhNguoiDung()
        {
            InitializeComponent();
            this.iDataSoure = TableChelmabinding();
            this.DataContext = iDataSoure;
            GetAllNhanVien();
            Init_GridNDung();
            Init_GridNhom();

            
        }

        private bus_CauHinhNguoiDung client = new bus_CauHinhNguoiDung();

        private void GetAllNhanVien()
        {
            cbbNhanVien.ItemsSource = client.GetAllNhanVien();
        }

        private void LoadGridNDung()
        {
            try
            {
                iGridDataSoureND = client.GetAllNguoiDung();
                grd.ItemsSource = iGridDataSoureND.Copy();
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
                xDicUser.Add("UserName", typeof(string));
                xDicUser.Add("Pass", typeof(string));
                xDicUser.Add("HoatDong", typeof(int));
                xDicUser.Add("GhiChu", typeof(string));
                xDicUser.Add("ID_NhanVien", typeof(Int32));
                xDicUser.Add("NhanVien", typeof(string));
                xDicUser.Add("HOTEN", typeof(string));
                xDicUser.Add("ID_NHANVIEN",typeof(Int32));
                dtaTable = TableUtil.ConvertToTable(xDicUser);
            }
            catch (Exception)
            {

                throw;
            }
            return dtaTable;
        }

        private void Init_GridNDung()
        {
            GridColumn xcolumn;


            xcolumn = new GridColumn();
            xcolumn.FieldName = "ID_NHANVIEN";
            xcolumn.Header = "Người dùng";
            xcolumn.Width = 50;
            xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            xcolumn.AllowEditing = DefaultBoolean.False;
            xcolumn.Visible = false;
            xcolumn.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            grd.Columns.Add(xcolumn);

            xcolumn = new GridColumn();
            xcolumn.FieldName = "UserName";
            xcolumn.Header = "UserName";
            xcolumn.Width = 50;
            xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            xcolumn.AllowEditing = DefaultBoolean.False;
            xcolumn.Visible = true;
            xcolumn.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            grd.Columns.Add(xcolumn);

            xcolumn = new GridColumn();
            xcolumn.FieldName = "HoatDong";
            xcolumn.Header = "Hoạt động";
            xcolumn.Width = 50;
            xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            xcolumn.AllowEditing = DefaultBoolean.False;
            xcolumn.Visible = true;
            xcolumn.EditSettings = new CheckEditSettings();
            xcolumn.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            grd.Columns.Add(xcolumn);

            xcolumn = new GridColumn();
            xcolumn.FieldName = "HOTEN";
            xcolumn.Header = "Người dùng";
            xcolumn.Width = 50;
            xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            xcolumn.AllowEditing = DefaultBoolean.False;
            xcolumn.Visible = true;
            xcolumn.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            grd.Columns.Add(xcolumn);

            xcolumn = new GridColumn();
            xcolumn.FieldName = "Pass";
            xcolumn.Header = "Người dùng";
            xcolumn.Width = 50;
            xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            xcolumn.AllowEditing = DefaultBoolean.False;
            xcolumn.Visible = false;
            xcolumn.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            grd.Columns.Add(xcolumn);

            xcolumn = new GridColumn();
            xcolumn.FieldName = "GhiChu";
            xcolumn.Header = "Người dùng";
            xcolumn.Width = 50;
            xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
            xcolumn.AllowEditing = DefaultBoolean.False;
            xcolumn.Visible = false;
            xcolumn.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
            grd.Columns.Add(xcolumn);

            iGridDataSoureND = client.GetAllNguoiDung();

            grd.ItemsSource = iGridDataSoureND.Copy();
            //LoadGridNDung();
        }

        private void Init_GridNhom()
        {
            try
            {
                GridColumn xcolumn;

                xcolumn = new GridColumn();
                xcolumn.FieldName = "IsNew";
                xcolumn.Header = string.Empty;
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
                xcolumn.FieldName = "MaNhomNguoiDung";
                xcolumn.Header = "MaNhomNguoiDung";
                xcolumn.Width = 50;
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = false;
                xcolumn.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grdUI.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "TenNhomNguoiDUng";
                xcolumn.Header = "Nhóm người dùng";
                xcolumn.Width = 50;
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                xcolumn.HeaderStyle = FindResource("ColumnsHeaderStyle") as Style;
                grdUI.Columns.Add(xcolumn);

                iGridDataSoure = client.GetAllNhom();
                grdUI.ItemsSource = iGridDataSoure;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void ValiCheck()
        {
            if (chkHoatDong.IsChecked == true)
            {
                this.iDataSoure.Rows[0]["HoatDong"] = 1;
            }
            else
            {
                this.iDataSoure.Rows[0]["HoatDong"] = 0;
            }
        }

        private void SetNullValue()
        {
            try
            {
                this.iDataSoure.Rows[0]["UserName"] = String.Empty;
                this.iDataSoure.Rows[0]["Pass"] = String.Empty;
                this.iDataSoure.Rows[0]["HoatDong"] = 0;
                this.iDataSoure.Rows[0]["GhiChu"] = String.Empty;
                this.iDataSoure.Rows[0]["ID_NHANVIEN"] = 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool ValiDate()
        {
            try
            {
                if (this.iDataSoure.Rows[0]["UserName"].ToString() == String.Empty)
                {
                    MessageBox.Show("Vui lòng nhập", "Thông báo");
                    txtUser.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["Pass"].ToString() == String.Empty)
                {
                    MessageBox.Show("Vui lòng nhập", "Thông báo");
                    txtPass.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["ID_NhanVien"].ToString() == "0")
                {
                    MessageBox.Show("Vui lòng nhập", "Thông báo");
                    cbbNhanVien.Focus();
                    return false;
                }
                return true;
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
                client.Delete_NhanSu(iDataSoure.Copy());
                LoadGridNDung();
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
                LoadGridNDung();
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
                Mouse.OverrideCursor = Cursors.Wait;
                ValiCheck();
                DataTable dt = iGridDataSoure.Copy();
                if (ValiDate())
                {
                    if (flagSave)
                    {
                        #region Insert new recore
                        bool res = true;
                        #region chưa kiểm tra
                        //foreach (DataRow dr in iGridDataSoureND.Rows)
                        //{
                        //    if (dr["UserName"].ToString().Trim() != this.iDataSoure.Rows[0]["UserName"].ToString().Trim())
                        //    {
                        //        res = client.InsertNguoiDung(iDataSoure.Copy());
                        //        break;
                        //    }
                        //    else
                        //    {
                        //        MessageBox.Show("Tài khoản này đã tồn tại","Thông báo");
                        //        txtUser.Focus();
                        //        return;
                        //    }
                        //}
                        #endregion
                        if (client.KiemTraUse(this.iDataSoure.Rows[0]["UserName"].ToString().Trim()))
                        {
                            res = client.InsertNguoiDung(this.iDataSoure.Copy());
                        }
                        else
                        {
                            CTMessagebox.Show("Tài khoản này đã tồn tại", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                            txtUser.Focus();
                            return;
                        }
                        if (res)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (dr["IsNew"].ToString() == "True")
                                {
                                    client.InsertNDungVaoNhom(this.iDataSoure.Rows[0]["UserName"].ToString().Trim(),dr["MaNhomNguoiDung"].ToString());
                                }
                            }
                            LoadGridNDung();
                        }
                        else
                        {
                            CTMessagebox.Show("Lưu dữ liệu thành công", "Lưu", "", CTICON.Information, CTBUTTON.YesNo);
                        }
                        #endregion
                    }
                    else
                    {
                        #region Update, Delete, Insert new recore for user already
                        client.UpdateNguoiDung(iDataSoure.Copy());        
                        foreach (DataRow dr in dt.Rows)
                        {
                            DataTable idt = client.GetAllNDungNhom(this.iDataSoure.Rows[0]["UserName"].ToString().Trim(),dr["MaNhomNguoiDung"].ToString().Trim());
                            if (dr["IsNew"].ToString() == "True")
                            {
                                if (idt.Rows.Count <= 0)
                                {
                                    try
                                    {
                                        bool res =client.InsertNDungVaoNhom(this.iDataSoure.Rows[0]["UserName"].ToString().Trim(), dr["MaNhomNguoiDung"].ToString().Trim());
                                        if (!res)
                                        {
                                            CTMessagebox.Show("Lưu dữ liệu thất bại", "Lưu", "", CTICON.Information, CTBUTTON.YesNo);
                                        }
                                    }
                                    catch
                                        (Exception)
                                    {
                                        throw;
                                    }
                                }
                            }
                            else
                            {
                                if (dr["IsNew"].ToString() == "False")
                                {
                                    if (idt.Rows.Count > 0)
                                    {
                                        try
                                        {
                                            client.DeleteNDungVaoNhom(this.iDataSoure.Rows[0]["UserName"].ToString().Trim(), dr["MaNhomNguoiDung"].ToString().Trim());
                                        }
                                        catch (Exception)
                                        {
                                            throw;
                                        }
                                    }
                                }
                                else
                                {

                                }
                            }
                        }
                        LoadGridNDung();
                        #endregion
                    }
                }
            }
            catch (Exception err)
            {
                throw;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void btnAddNew_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                SetNullValue();
                //this.iDataSoure.Clear();
                iGridDataSoure = client.GetAllNhom();
                grdUI.ItemsSource = iGridDataSoure;
                flagSave = true;
                txtUser.IsReadOnly = false;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void btnAddNhanSu_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                frm_NhanVien frm = new frm_NhanVien();
                frm.ShowDialog();
                GetAllNhanVien();
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        private void GrdViewNDung_OnFocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                DataRow row = null;
                if (this.grd.GetFocusedRow() == null)
                    return;
                row = ((DataRowView)this.grd.GetFocusedRow()).Row;

                this.iDataSoure.Rows[0]["UserName"] = row["UserName"];
                this.iDataSoure.Rows[0]["Pass"] = row["Pass"];
                this.iDataSoure.Rows[0]["HoatDong"] = row["HoatDong"];
                this.iDataSoure.Rows[0]["GhiChu"] = row["GhiChu"];
                this.iDataSoure.Rows[0]["ID_NHANVIEN"] = row["ID_NHANVIEN"];
                this.iDataSoure.Rows[0]["HOTEN"] = row["HOTEN"];
                
                if (!string.IsNullOrEmpty(this.iDataSoure.Rows[0]["UserName"].ToString()))
                {
                    #region
                    DataTable idt = client.GetAllNhom();
                    DataTable dt = client.GetNhomWhere(row["UserName"].ToString().Trim());
                    #region Create Table
                    DataTable xdt = new DataTable();
                    xdt.Columns.Add("IsNew");
                    xdt.Columns.Add("MaNhomNguoiDung");
                    xdt.Columns.Add("TenNhomNguoiDUng");
                    #endregion

                    if (dt.Rows.Count > 0)
                    {
                        #region Get Check User for Group
                        foreach (DataRow dr in dt.Rows)
                        {
                            foreach (DataRow idr in idt.Rows)
                            {
                                DataRow xdr = xdt.NewRow();

                                if (dr["MaNhomNguoiDung"].ToString().Trim().Contains(idr["MaNhomNguoiDung"].ToString().Trim()))
                                {
                                    xdr["IsNew"] = "True";
                                    xdr["MaNhomNguoiDung"] = idr["MaNhomNguoiDung"];
                                    xdr["TenNhomNguoiDUng"] = idr["TenNhomNguoiDUng"];
                                    xdt.Rows.Add(xdr);
                                }
                                else
                                {
                                    xdr["IsNew"] = "False";
                                    xdr["MaNhomNguoiDung"] = idr["MaNhomNguoiDung"];
                                    xdr["TenNhomNguoiDUng"] = idr["TenNhomNguoiDUng"];
                                    xdt.Rows.Add(xdr);
                                }
                                xdt.AcceptChanges();
                            }
                        }
                        if (xdt.Rows.Count > 0)
                        {
                            iGridDataSoure = xdt.AsEnumerable() .OrderByDescending(a => a.Field<string>("IsNew"))
                                                                .GroupBy(a => a.Field<string>("MaNhomNguoiDung")).Select(group => group.First()).CopyToDataTable();
                        }
                        else
                        {
                            return;
                        }
                        #endregion
                    }
                    else
                    {
                        iGridDataSoure = idt;
                    }
                    #endregion
                }
                flagSave = false;
                txtUser.IsReadOnly = true;
                grdUI.ItemsSource = iGridDataSoure;
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

        private void GrdViewNDung_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
        }
    }
}
