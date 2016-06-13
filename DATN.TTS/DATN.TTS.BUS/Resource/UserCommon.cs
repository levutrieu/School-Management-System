using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.TTS.BUS.Resource
{
    public static class UserCommon
    {
        private static int ID_NhanVien;

        private static int _ID_NAMHOC_HIENTAI;

        private static int _ID_NAMHOC_HKY_HTAI;

        public static int IdNhanVien
        {
            get { return ID_NhanVien; }
            set { ID_NhanVien = value; }
        }

        private static string Ten_NhanVien;

        public static string TenNhanVien
        {
            get { return Ten_NhanVien; }
            set { Ten_NhanVien = value; }
        }

        private static string User_Name;

        public static string UserName
        {
            get { return User_Name; }
            set { User_Name = value; }
        }

        public static int IdNamhocHientai
        {
            get { return _ID_NAMHOC_HIENTAI; }
            set { _ID_NAMHOC_HIENTAI = value; }
        }

        public static int IdNamhocHkyHtai
        {
            get { return _ID_NAMHOC_HKY_HTAI; }
            set { _ID_NAMHOC_HKY_HTAI = value; }
        }
    }
}
