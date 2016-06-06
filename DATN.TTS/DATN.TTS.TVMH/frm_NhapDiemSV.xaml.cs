﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
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
using DATN.TTS.BUS.Resource;
using DevExpress.Utils;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using Microsoft.Win32;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_NhapDiemSV.xaml
    /// </summary>
    public partial class frm_NhapDiemSV : Page
    {
        bus_SinhVien sv = new bus_SinhVien();
        bus_NhapDiemSV diem = new bus_NhapDiemSV();
        bus_molophocphan lhp = new bus_molophocphan();

        private DataTable iDataSource = null;
        private DataTable iGridDataSource = null;

        public frm_NhapDiemSV()
        {
            InitializeComponent();
            this.iDataSource = TableSchemaBinding();
            this.DataContext = this.iDataSource;
            iDataSource.Rows[0]["USER"] = UserCommon.UserName;
            Load_cbo();
            Init_Grid();
            Load_data();
        }

        DataTable TableSchemaBinding()
        {
            try
            {
                DataTable dt = new DataTable();
                Dictionary<string, Type> dic = new Dictionary<string, Type>();
                dic.Add("USER", typeof(string));
                dic.Add("id_sinhvien_s", typeof(int));
                dt = TableUtil.ConvertToTable(dic);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Init_Grid()
        {
            try
            {
                GridColumn xcolumn;

                xcolumn = new GridColumn();
                xcolumn.FieldName = "ID_KETQUA";
                xcolumn.Visible = false;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "ID_SINHVIEN";
                xcolumn.Visible = false;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "ID_LOPHOCPHAN";
                xcolumn.Visible = false;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.AllowCellMerge = true;
                xcolumn.FieldName = "TEN_SINHVIEN";
                xcolumn.Header = "Tên sinh viên";
                xcolumn.Width = 150;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "MA_LOP_HOCPHAN";
                xcolumn.Header = "Mã học phần";
                xcolumn.AllowCellMerge = false;
                xcolumn.Width = 90;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "TEN_LOP_HOCPHAN";
                xcolumn.Header = "Tên học phần";
                xcolumn.AllowCellMerge = false;
                xcolumn.Width = 150;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "TEN_KHOAHOC";
                xcolumn.Header = "Khóa";
                xcolumn.AllowCellMerge = false;
                xcolumn.Width = 90;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "DIEM_BT";
                xcolumn.Header = "Điểm BT";
                xcolumn.AllowCellMerge = false;
                xcolumn.Width = 90;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "DIEM_GK";
                xcolumn.Header = "Điểm giữa kỳ";
                xcolumn.AllowCellMerge = false;
                xcolumn.Width = 90;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "DIEM_CK";
                xcolumn.Header = "Điểm cuối kỳ";
                xcolumn.AllowCellMerge = false;
                xcolumn.Width = 90;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "DIEM_TONG";
                xcolumn.Header = "Điểm tổng kết";
                xcolumn.AllowCellMerge = false;
                xcolumn.Width = 90;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "DIEM_CHU";
                xcolumn.Header = "Điểm hệ 4(chữ)";
                xcolumn.AllowCellMerge = false;
                xcolumn.Width = 90;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "DIEM_HE4";
                xcolumn.Header = "Điểm hệ 4";
                xcolumn.AllowCellMerge = false;
                xcolumn.Width = 90;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                grdView.AutoWidth = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void Load_data()
        {
            try
            {
                iGridDataSource = diem.GetAll_DiemSV();
                grd.ItemsSource = iGridDataSource;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Load_cbo()
        {
            try
            {
                DataTable idatasv = sv.GetAllSinhVien();
                cboSinhVien.ItemsSource = idatasv;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Btnimport_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".xls";
            dlg.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                DataTable dtExcel = exceldata(filename);
                dtExcel.Columns.Add("ID_SINHVIEN", typeof(decimal));
                dtExcel.Columns.Add("ID_LOPHOCPHAN", typeof(decimal));

                DataTable dtSV = sv.GetAllSinhVien();
                DataTable dtDiemSV = diem.GetAll_DiemSV();
                DataTable dtlhp = lhp.Getall_lopHocPhan();
                if (dtDiemSV.Rows.Count == 0)
                {
                    dtDiemSV.Columns.Add("ID_SINHVIEN", typeof(int));
                    dtDiemSV.Columns.Add("ID_LOPHOCPHAN", typeof(int));
                }
                DataTable dtNewInsert = dtExcel.Clone();
                foreach (DataRow dr in dtExcel.Rows)
                {
                    string id_sinhvien =
                        dtSV.Select("MA_SINHVIEN = '" + dr["f_masv"].ToString() + "'")[0]["ID_SINHVIEN"].ToString();
                    string id_lhp = dtlhp.Select("MA_LOP_HOCPHAN = '" + dr["f_mamhhtd"].ToString() + "'")[0]["ID_LOPHOCPHAN"].ToString();
                    if (IsCheck(dtDiemSV, id_sinhvien, id_lhp))
                    {
                        dr["ID_SINHVIEN"] = Convert.ToDecimal(id_sinhvien);
                        dr["ID_LOPHOCPHAN"] = Convert.ToDecimal(id_lhp);
                        dtNewInsert.ImportRow(dr);
                        DataRow m = dtDiemSV.NewRow();
                        m["ID_SINHVIEN"] = Convert.ToInt32(dr["ID_SINHVIEN"].ToString());
                        m["ID_LOPHOCPHAN"] =Convert.ToInt32(dr["f_mamhhtd"].ToString());
                        dtDiemSV.Rows.Add(m);
                    }
                }
                int isInsert = 0;
                if (dtNewInsert != null && dtNewInsert.Rows.Count > 0)
                {
                    isInsert = diem.InsertObject_Excel(dtNewInsert, iDataSource.Rows[0]["USER"].ToString());
                    if (isInsert != 0)
                    {
                        CTMessagebox.Show("Thành công", "Nhập từ Excel", "", CTICON.Information, CTBUTTON.OK);
                    }
                    else
                    {
                        CTMessagebox.Show("Lỗi", "Nhập từ Excel", "", CTICON.Information, CTBUTTON.OK);
                    }
                }
                else
                {
                    CTMessagebox.Show("Thành công", "Nhập từ Excel", "", CTICON.Information, CTBUTTON.OK);
                }
            }
        }

        private bool IsCheck(DataTable dtDiem, string id_sinhvien, string id_lophocphan)
        {
            if (dtDiem != null && dtDiem.Rows.Count > 0)
            {
                DataRow[] xcheck = dtDiem.Select("ID_SINHVIEN = " + id_sinhvien + " and ID_LOPHOCPHAN = " + id_lophocphan);
                if (xcheck.Count() > 0)
                    return false;
            }
            return true;
        }

        public DataTable exceldata(string filePath)
        {
            DataTable dtexcel = new DataTable();
            bool hasHeaders = false;
            string HDR = hasHeaders ? "Yes" : "No";
            string strConn;
            if (filePath.Substring(filePath.LastIndexOf('.')).ToLower() == ".xlsx")
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=0\"";
            else
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\"";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            foreach (DataRow schemaRow in schemaTable.Rows)
            {
                string sheet = schemaRow["TABLE_NAME"].ToString();
                if (!sheet.EndsWith("_"))
                {
                    string query = "SELECT f_mamhhtd,f_masv,f_diembt,f_diem1,f_diem2,f_diemtk1,f_diemch1,f_diemstk1 FROM [" + sheet + "]";
                    OleDbDataAdapter daexcel = new OleDbDataAdapter(query, conn);
                    dtexcel.Locale = CultureInfo.CurrentCulture;
                    daexcel.Fill(dtexcel);
                }
            }
            conn.Close();
            return dtexcel;
        }

        private void BtnRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            Load_data();
        }

        private void CboSinhVien_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                DataTable idatasearch = null;
                DataRow[] xcheck = iGridDataSource.Select("ID_SINHVIEN = " + iDataSource.Rows[0]["id_sinhvien_s"].ToString());
                idatasearch = xcheck.CopyToDataTable();
                grd.ItemsSource = idatasearch;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
