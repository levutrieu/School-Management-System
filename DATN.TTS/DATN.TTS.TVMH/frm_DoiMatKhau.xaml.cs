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
using DATN.TTS.BUS.Resource;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_DoiMatKhau.xaml
    /// </summary>
    public partial class frm_DoiMatKhau : Window
    {
        private DataTable iDataSource = null;

        bus_login bus=new bus_login();
        public frm_DoiMatKhau()
        {
            InitializeComponent();
            iDataSource = TableChelmabinding();
            this.DataContext = iDataSource;
            iDataSource.Rows[0]["USER"] = UserCommon.UserName;
            iDataSource.Rows[0]["account"] = UserCommon.UserName;
            passcbo.Focus();
        }

        private DataTable TableChelmabinding()
        {
            DataTable dtaTable = null;
            try
            {
                Dictionary<string, Type> xDicUser = new Dictionary<string, Type>();
                xDicUser.Add("USER", typeof(string));
                xDicUser.Add("account", typeof(string));
                xDicUser.Add("password", typeof(string));
                xDicUser.Add("passwordnew", typeof(string));
                xDicUser.Add("passwordcheck", typeof(string));
                dtaTable = TableUtil.ConvertToTable(xDicUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtaTable;
        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Btnsave_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(iDataSource.Rows[0]["password"].ToString()) ||
                    string.IsNullOrEmpty(iDataSource.Rows[0]["passwordnew"].ToString()) ||
                    string.IsNullOrEmpty(iDataSource.Rows[0]["passwordcheck"].ToString()))
                {
                    CTMessagebox.Show("Bạn phải nhập đầy đủ thông tin", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                }
                else
                {
                    int i = bus.CheckLogin(iDataSource.Rows[0]["account"].ToString(),
                        iDataSource.Rows[0]["password"].ToString());
                    if (i == 1)
                    {
                        if (
                            !iDataSource.Rows[0]["passwordnew"].ToString()
                                .Trim()
                                .Equals(iDataSource.Rows[0]["passwordcheck"].ToString().Trim()))
                        {
                            CTMessagebox.Show("Mật khẩu xác nhận sai", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                            iDataSource.Rows[0]["password"] = "";
                            iDataSource.Rows[0]["passwordnew"] = "";
                            iDataSource.Rows[0]["passwordcheck"] = "";
                            passcbo.Focus();
                        }
                        else
                        {
                            int xcheck = bus.UpdatePass(iDataSource.Rows[0]["account"].ToString(),
                                iDataSource.Rows[0]["passwordnew"].ToString());
                            if (xcheck == 1)
                            {
                                CTMessagebox.Show("Thành công", "Đổi mật khẩu", "", CTICON.Information, CTBUTTON.OK);
                                this.Close();
                            }
                        }
                    }
                    else
                    {
                        CTMessagebox.Show("Sai mật khẩu", "Thông báo", "", CTICON.Information, CTBUTTON.OK);
                        iDataSource.Rows[0]["password"] = "";
                        iDataSource.Rows[0]["passwordnew"] = "";
                        iDataSource.Rows[0]["passwordcheck"] = "";
                        passcbo.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
    }
}
