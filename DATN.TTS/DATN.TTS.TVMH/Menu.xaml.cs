using System;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Runtime.Remoting;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CustomMessage;
using DATN.TTS.BUS;
using DATN.TTS.BUS.Resource;
using DATN.TTS.TVMH.Resource;
using DevExpress.Mvvm.Native;
using DevExpress.Xpf.NavBar;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        bus_DangKyHocPhan GetNamHoc = new bus_DangKyHocPhan();
        public Menu()
        {
            InitializeComponent();
            this.DataContext = this;
            txtThongTin.Text = UserCommon.TenNhanVien;
            Load_menu();
            UserCommon.IdNamhocHientai = GetNamHoc.GetNamHocHienTai();
            UserCommon.IdNamhocHkyHtai = GetNamHoc.GetHocKyHienTai();
            NavBarItem_OnClick(null, null);
        }

        #region Add item menu

        private void Load_menu()
        {
            var uri = new Uri("pack://application:,,,/Images/item.png");

            bus_login bus = new bus_login();
            DataTable xdt = bus.Load_manhinh(UserCommon.UserName);
            if (xdt != null)
            {
                if (xdt.Rows.Count > 0)
                {
                    for (int i = 0; i < xdt.Rows.Count; i++)
                    {
                        NavBarItem item = new NavBarItem();
                        item.Content = xdt.Rows[i]["TenManHinh"].ToString();
                        item.ImageSource = new BitmapImage(uri);
                        item.SetCurrentValue(ContentStringFormatProperty,
                            xdt.Rows[i]["MaManHinh"].ToString() + "/" + xdt.Rows[i]["TenManHinh"].ToString());
                        item.DisplayMode = DisplayMode.ImageAndText;
                        item.Click += OpenTabClick;
                        MenuGroup.Items.Add(item);
                    }
                }
            }
        }

        private void OpenTabClick(object sender, System.EventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                int check = 0;
                TabItem tmp_focus = null;

                NavBarItem item = (NavBarItem) sender;
                string _form = (string) item.GetValue(ContentStringFormatProperty);
                string[] tmp = _form.Split('/');
                TabItem xtabitem = new TabItem();
                xtabitem.Style = this.FindResource("TabItemStyle") as Style;
                xtabitem.Header = tmp[1];
                Frame xtabFrame = new Frame();
                foreach (TabItem xitem in tabMenu.Items)
                {
                    Frame a = (Frame) xitem.Content;
                    Page c = (Page) a.Content;
                    var m = (Grid) c.Content;
                    string[] n = m.Parent.ToString().Split('.');
                    if (n[3].Trim().Equals(tmp[0].Trim()))
                    {
                        check = 1;
                        tmp_focus = xitem;
                    }
                }
                if (check == 1)
                {
                    tmp_focus.Focus();
                }
                else
                {
                    string windowClass = string.Concat(tmp[0].Trim(), ".xaml");
                    var Xpage = (Page) System.Windows.Application.LoadComponent(new Uri(windowClass, UriKind.Relative));
                    xtabFrame.Content = Xpage;
                    xtabitem.Content = xtabFrame;
                    tabMenu.Items.Add(xtabitem);
                    xtabitem.Focus();
                }

                //Type type = this.GetType((string)string.Concat("DATN.TTS.TVMH",tmp[0].Trim()), false, false);
                //if (type == (Type)null)
                //    return (object)null;
                //var cc = Activator.CreateInstance(type, false, BindingFlags.Instance | BindingFlags.Public, (Binder)null,
                //    (object[]) null, (CultureInfo) null, (object[]) null);

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


        #endregion

        #region CloseCommand

        private RelayCommand _cmdCloseCommand;
        public ICommand CloseCommand
        {
            get
            {
                if (_cmdCloseCommand == null)
                {
                    _cmdCloseCommand = new RelayCommand(
                        param => this.CloseTab_Execute(param),
                        param => this.CloseTab_CanExecute(param)
                        );
                }
                return _cmdCloseCommand;
            }
        }
        private void CloseTab_Execute(object parm)
        {
            TabItem ti = parm as TabItem;
            if (ti != null)
                tabMenu.Items.Remove(parm);
        }
        private bool CloseTab_CanExecute(object parm)
        {
            TabItem ti = parm as TabItem;
            if (ti != null)
                return ti.IsEnabled;
            return false;
        }

        #endregion

        #region Event click change user

        private int check = 0;
        private void BtnUser_OnClick(object sender, RoutedEventArgs e)
        {
            if (check == 1)
            {
                this.MenuChange.Visibility = Visibility.Hidden;
                check = 0;
            }
            else
            {
                this.MenuChange.Visibility = Visibility.Visible;
                check = 1;
            }
        }

        private void MenuChange_OnMouseLeave(object sender, MouseEventArgs e)
        {
            this.MenuChange.Visibility = Visibility.Hidden;
        }

        private void BtnDangXuat_OnClick(object sender, RoutedEventArgs e)
        {
            frm_Login form = new frm_Login();
            form.Show();
            this.Close();
        }

        private void BtnDoiMK_OnClick(object sender, RoutedEventArgs e)
        {
            frm_DoiMatKhau form = new frm_DoiMatKhau();
            form.ShowDialog();
        }

        private void BtnThoat_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        } 

        #endregion

        private void NavBarItem_OnClick(object sender, EventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                var uri = new Uri("pack://application:,,,/Images/item.png");
                NavBarItem item = new NavBarItem();
                item.Content = "Trang chủ";
                item.ImageSource = new BitmapImage(uri);
                item.SetCurrentValue(ContentStringFormatProperty,
                    "Main" + "/" + "Trang chủ");
                item.DisplayMode = DisplayMode.ImageAndText;
                item.Click += OpenTabClick;
                OpenTabClick(item, null);
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Thông báo", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }
    }
}
