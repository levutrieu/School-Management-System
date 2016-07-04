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
using Microsoft.Win32;
using DATN.TTS.BUS;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class frm_Login : Window
    {
        bus_login bus = new bus_login();
        private DataTable iDataSource = null;

        public frm_Login()
        {
            InitializeComponent();
            this.iDataSource = TableSchemaDefineBingding();
            this.DataContext = this.iDataSource;
            this.iDataSource.Rows[0]["savecheck"] = false;
            DocDuLieuRegistry();
        }

        private DataTable TableSchemaDefineBingding()
        {
            DataTable xDt = new DataTable();
            try
            {
                xDt.Columns.Add("account", typeof(string));
                xDt.Columns.Add("password", typeof(string));
                xDt.Columns.Add("savecheck", typeof(bool));
                DataRow xdr = xDt.NewRow();
                xDt.Rows.InsertAt(xdr, 0);
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            return xDt;
        }

        private void BtnDangNhap_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                #region Kiem tra nhap vao

                if (string.IsNullOrEmpty(iDataSource.Rows[0]["account"].ToString()) ||
                    string.IsNullOrEmpty(iDataSource.Rows[0]["password"].ToString()))
                {
                    CTMessagebox.Show("Thiếu tài khoản hoặc mật khẩu!", "Đăng nhập", "");
                    return;
                }

                #endregion

                #region Kiem tra cau hinh SQL

                bus.Check_Config();
                if (bus.Check_Config() == 1 || bus.Check_Config() == 2)
                {
                    if (CTMessagebox.Show("Chuỗi cấu hình sai, bạn muốn cấu hình hệ thống không ?", "Đăng nhập", "",
                        CTICON.Question,
                        CTBUTTON.YesNo) == CTRESPONSE.Yes)
                    {
                        BtnCauHinh_OnClick(null, null);
                    }
                    else
                    {
                        return;
                    }

                }
                else
                {
                    #region Check tai khoan

                    int xcheck = bus.CheckLogin(iDataSource.Rows[0]["account"].ToString(),
                        iDataSource.Rows[0]["password"].ToString());
                    if (xcheck == 0)
                    {
                        CTMessagebox.Show("Sai tài khoản hoặc mật khẩu!", "Đăng nhập", "");
                        this.iDataSource.Rows[0]["password"] = "";
                        AccountTextEdit.Focus();
                    }
                    if (xcheck == 2)
                    {
                        CTMessagebox.Show("Tài khoản này đã bị khóa", "Đăng nhập", "");
                        this.iDataSource.Rows[0]["password"] = "";
                        AccountTextEdit.Focus();
                    }
                    if (xcheck == 1)
                    {
                        #region Luu mat khau

                        if (Convert.ToBoolean(this.iDataSource.Rows[0]["savecheck"]) == true)
                        {
                            Luu_MatKhau_Registry(iDataSource.Rows[0]["account"].ToString(),
                                iDataSource.Rows[0]["password"].ToString(), "true");
                        }
                        else
                        {
                            Luu_MatKhau_Registry("", "", "false");
                        }

                        #endregion

                        #region Mo form main

                        Menu main = new Menu();
                        main.Show();
                        this.Close();

                        #endregion
                    }

                    #endregion
                }

                #endregion

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

        private void BtnThoat_OnClick(object sender, RoutedEventArgs e)
        {
            //this.Close();


        }

        public void Luu_MatKhau_Registry(string pUser, string pPass, string pCheck)
        {
            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey("Software\\DATN\\TTS");
            regKey.SetValue("User", pUser);
            regKey.SetValue("Password", pPass);
            regKey.SetValue("Check", pCheck);
            regKey.Close();
        }

        public void DocDuLieuRegistry()
        {
            try
            {
                string pUser = "", pPass = "", pCheck = "";
                RegistryKey regKey = Registry.CurrentUser;
                regKey = regKey.CreateSubKey("Software\\DATN\\TTS");
                //đọc vào dữ liệu rồi gán cho biến tình trạng
                pCheck = regKey.GetValue("Check").ToString();
                if (pCheck == "true")
                {
                    LuuMK.IsChecked = true;
                    this.iDataSource.Rows[0]["savecheck"] = true;
                    this.iDataSource.Rows[0]["account"] = regKey.GetValue("User").ToString();
                    this.iDataSource.Rows[0]["password"] = regKey.GetValue("Password").ToString();
                    return;
                }
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
        }

        private void LuuMK_OnUnchecked(object sender, RoutedEventArgs e)
        {
            this.iDataSource.Rows[0]["savecheck"] = false;
        }

        private void LuuMK_OnChecked(object sender, RoutedEventArgs e)
        {
            this.iDataSource.Rows[0]["savecheck"] = true;
        }

        private void BtnCauHinh_OnClick(object sender, RoutedEventArgs e)
        {
            this.Visibility=Visibility.Hidden;
            frm_CauHinhSQL cauhinh = new frm_CauHinhSQL();
            cauhinh.ShowDialog();
            this.Visibility = Visibility.Visible;
        }

        private void BtnDangNhap_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnDangNhap_OnClick(sender, e);
            }
        }
    }
}
