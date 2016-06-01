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

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_KhungNganhDaoTaoKhoa.xaml
    /// </summary>
    public partial class frm_KhungNganhDaoTaoKhoa : Page
    {
        private DataTable iDataSoure = null;
        private DataTable iGridDataSoureNganh = null;
        private DataTable iGridDataSoureNganhCT = null;
        public frm_KhungNganhDaoTaoKhoa()
        {
            InitializeComponent();
        }
    }
}
