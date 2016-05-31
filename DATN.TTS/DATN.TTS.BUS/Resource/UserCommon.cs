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
    }
}
