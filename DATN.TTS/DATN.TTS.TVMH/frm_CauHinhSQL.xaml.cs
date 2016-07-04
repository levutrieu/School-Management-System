using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
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
using Microsoft.Win32;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_CauHinhSQL.xaml
    /// </summary>
    public partial class frm_CauHinhSQL : Window
    {
        bus_login bus = new bus_login();
        private DataTable iDataSource = null;


        public frm_CauHinhSQL()
        {
            InitializeComponent();
            this.iDataSource = TableSchemaDefineBingding();
            this.DataContext = this.iDataSource;
            Load_cbo();
        }

        private DataTable TableSchemaDefineBingding()
        {
            DataTable xDt = new DataTable();
            try
            {
                xDt.Columns.Add("SERVER_NAME", typeof(string));
                xDt.Columns.Add("USER", typeof(string));
                xDt.Columns.Add("PASS", typeof(string));
                xDt.Columns.Add("DATABASE", typeof(string));
                DataRow xdr = xDt.NewRow();
                xDt.Rows.InsertAt(xdr, 0);
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            return xDt;
        }

        void Load_cbo()
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                DataTable cbo_data = null;
                cbo_data = GetSerVerName_ByRegistry();
                cbo_servername.ItemsSource = cbo_data;
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

        #region Get Servername

        public DataTable GetSerVerName()
        {
            try
            {
                DataTable table = System.Data.Sql.SqlDataSourceEnumerator.Instance.GetDataSources();
                return table;
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            return null;
        }
        public static DataTable GetSerVerName_ByRegistry()
        {
            RegistryValueDataReader registryValueDataReader = new RegistryValueDataReader();

            string[] instances64Bit = registryValueDataReader.ReadRegistryValueData(RegistryHive.Wow64,
                                                                                    Registry.LocalMachine,
                                                                                    @"SOFTWARE\Microsoft\Microsoft SQL Server",
                                                                                    "InstalledInstances");

            string[] instances32Bit = registryValueDataReader.ReadRegistryValueData(RegistryHive.Wow6432,
                                                                                    Registry.LocalMachine,
                                                                                    @"SOFTWARE\Microsoft\Microsoft SQL Server",
                                                                                    "InstalledInstances");
            string name_com = System.Environment.MachineName;
            DataTable xdt = new DataTable();
            xdt.Columns.Add("SERVER_NAME", typeof(string));
            xdt.Columns.Add("SERVER_NAME_FULL", typeof(string));
            foreach (string name in instances32Bit)
            {
                DataRow dr = xdt.NewRow();
                dr[0] = name;
                dr[1] = name_com + "\\" + name;
                xdt.Rows.Add(dr);
            }
            foreach (string name in instances64Bit)
            {
                DataRow dr = xdt.NewRow();
                dr[0] = name;
                dr[1] = name_com + "\\" + name;
                xdt.Rows.Add(dr);
            }
            return xdt;
        }
        public enum RegistryHive
        {
            Wow64,
            Wow6432
        }
        public class RegistryValueDataReader
        {
            private static readonly int KEY_WOW64_32KEY = 0x200;
            private static readonly int KEY_WOW64_64KEY = 0x100;

            private static readonly UIntPtr HKEY_LOCAL_MACHINE = (UIntPtr)0x80000002;

            private static readonly int KEY_QUERY_VALUE = 0x1;

            [DllImport("advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "RegOpenKeyEx")]
            static extern int RegOpenKeyEx(
                        UIntPtr hKey,
                        string subKey,
                        uint options,
                        int sam,
                        out IntPtr phkResult);


            [DllImport("advapi32.dll", SetLastError = true)]
            static extern int RegQueryValueEx(
                        IntPtr hKey,
                        string lpValueName,
                        int lpReserved,
                        out uint lpType,
                        IntPtr lpData,
                        ref uint lpcbData);

            private static int GetRegistryHiveKey(RegistryHive registryHive)
            {
                return registryHive == RegistryHive.Wow64 ? KEY_WOW64_64KEY : KEY_WOW64_32KEY;
            }

            private static UIntPtr GetRegistryKeyUIntPtr(RegistryKey registry)
            {
                if (registry == Registry.LocalMachine)
                {
                    return HKEY_LOCAL_MACHINE;
                }

                return UIntPtr.Zero;
            }

            public string[] ReadRegistryValueData(RegistryHive registryHive, RegistryKey registryKey, string subKey, string valueName)
            {
                string[] instanceNames = new string[0];

                int key = GetRegistryHiveKey(registryHive);
                UIntPtr registryKeyUIntPtr = GetRegistryKeyUIntPtr(registryKey);

                IntPtr hResult;

                int res = RegOpenKeyEx(registryKeyUIntPtr, subKey, 0, KEY_QUERY_VALUE | key, out hResult);

                if (res == 0)
                {
                    uint type;
                    uint dataLen = 0;

                    RegQueryValueEx(hResult, valueName, 0, out type, IntPtr.Zero, ref dataLen);

                    byte[] databuff = new byte[dataLen];
                    byte[] temp = new byte[dataLen];

                    List<String> values = new List<string>();

                    GCHandle handle = GCHandle.Alloc(databuff, GCHandleType.Pinned);
                    try
                    {
                        RegQueryValueEx(hResult, valueName, 0, out type, handle.AddrOfPinnedObject(), ref dataLen);
                    }
                    finally
                    {
                        handle.Free();
                    }

                    int i = 0;
                    int j = 0;

                    while (i < databuff.Length)
                    {
                        if (databuff[i] == '\0')
                        {
                            j = 0;
                            string str = Encoding.Default.GetString(temp).Trim('\0');

                            if (!string.IsNullOrEmpty(str))
                            {
                                values.Add(str);
                            }

                            temp = new byte[dataLen];
                        }
                        else
                        {
                            temp[j++] = databuff[i];
                        }

                        ++i;
                    }

                    instanceNames = new string[values.Count];
                    values.CopyTo(instanceNames);
                }

                return instanceNames;
            }
        } 

        #endregion

        #region Get Datatable

        public DataTable GetDatabase(string pServerName, string pUser, string pPass)
        {
            try
            {
                DataTable dt = new DataTable();
                if (!string.IsNullOrEmpty(pUser))
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT name FROM sys.databases",
                        "Data Source=" + pServerName + ";Initial Catalog=master;User ID=" + pUser + ";Password=" + pPass +
                        "");
                    da.Fill(dt);
                }
                else
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT name FROM sys.databases",
                        "Data Source=" + pServerName + ";Initial Catalog=master;Integrated Security=True;");
                    da.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            return null;
        }

        #endregion

        private void BtnOK_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                bool xcheck = bus.ChangeConnectionString(iDataSource.Rows[0]["SERVER_NAME"].ToString(),
                    iDataSource.Rows[0]["DATABASE"].ToString(),
                    iDataSource.Rows[0]["USER"].ToString(), iDataSource.Rows[0]["PASS"].ToString());
                if (xcheck)
                {
                    CTMessagebox.Show("Thành công!", "Cấu hình", "");
                    this.Close();
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

        private void Cbo_database_OnDefaultButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (!string.IsNullOrEmpty(this.iDataSource.Rows[0]["SERVER_NAME"].ToString()))
                {
                    DataTable xdt = null;
                    xdt = GetDatabase(iDataSource.Rows[0]["SERVER_NAME"].ToString(),
                        iDataSource.Rows[0]["USER"].ToString(),
                        iDataSource.Rows[0]["PASS"].ToString());
                    cbo_database.ItemsSource = xdt;
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

        private void BtnRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            Load_cbo();
        }

        private void btnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}
