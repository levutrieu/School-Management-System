using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
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
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using DATN.C45;
using DevExpress.Data;
using DevExpress.Xpf.Editors.Settings;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_C45.xaml
    /// </summary>
    public partial class frm_C45 : Window
    {
        List<DATN.C45.DecisionTree_C45> iData_AllTree = new List<DATN.C45.DecisionTree_C45>(); // Danh sach tat ca cac tree sau khi thuc hien thuat toan

        bus_ID3 bus = new bus_ID3();

        private DataTable iGridDataSource = null;

        public frm_C45()
        {
            InitializeComponent();
            InitGrid_LopHP();
            this.iGridDataSource = TableSchemaBinding();
            Load_data();
        }

        private DataTable TableSchemaBinding()
        {
            try
            {
                DataTable dt = null;
                Dictionary<string, Type> dic = new Dictionary<string, Type>();
                dic.Add("CHK", typeof(bool));
                dic.Add("ID_MONHOC", typeof(int));
                dic.Add("MA_MONHOC", typeof(string));
                dic.Add("TEN_MONHOC", typeof(string));
                dic.Add("SO_TC", typeof(int));
                dt = TableUtil.ConvertToTable(dic);
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        void InitGrid_LopHP()
        {
            try
            {
                GridColumn col;

                col = new GridColumn();
                col.FieldName = "CHK";
                col.Header = "Chọn";
                col.Width = 30;
                col.AllowEditing = DefaultBoolean.True;
                col.Visible = true;
                col.EditSettings = new CheckEditSettings();
                col.UnboundType = UnboundColumnType.Boolean;
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "ID_MONHOC";
                col.Visible = false;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "MA_MONHOC";
                col.Header = "Mã môn học";
                col.Width = 100;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TEN_MONHOC";
                col.Header = "Tên môn học";
                col.Width = 250;
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "SO_TC";
                col.Header = "Số TC";
                col.Width = 60;
                col.AllowEditing = DefaultBoolean.False;
                
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                col.Visible = true;
                grd.Columns.Add(col);

                grdView.AutoWidth = true;
            }
            catch (Exception er)
            {
                throw er;
            }
        }

        public void Load_data()
        {
            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                iGridDataSource.Clear();
                foreach (DataRow dr in frm_DangKyHocPhan.iDataID3.Rows)
                {
                    if (iGridDataSource != null && iGridDataSource.Rows.Count > 0)
                    {
                        int check = 0;
                        foreach (DataRow d in iGridDataSource.Rows)
                        {
                            if (Convert.ToInt32(d["ID_MONHOC"]) == Convert.ToInt32(dr["ID_MONHOC"]))
                            {
                                check = 1;
                                break;
                            }
                        }
                        if (check == 0)
                        {
                            DataRow xdr = iGridDataSource.NewRow();
                            xdr["CHK"] = false;
                            xdr["ID_MONHOC"] = dr["ID_MONHOC"];
                            xdr["MA_MONHOC"] = dr["MA_MONHOC"];
                            xdr["TEN_MONHOC"] = dr["TEN_MONHOC"];
                            xdr["SO_TC"] = dr["SO_TC"];
                            iGridDataSource.Rows.Add(xdr);
                        }
                    }
                    else
                    {
                        DataRow xdr = iGridDataSource.NewRow();
                        xdr["CHK"] = false;
                        xdr["ID_MONHOC"] = dr["ID_MONHOC"];
                        xdr["MA_MONHOC"] = dr["MA_MONHOC"];
                        xdr["TEN_MONHOC"] = dr["TEN_MONHOC"];
                        xdr["SO_TC"] = dr["SO_TC"];
                        iGridDataSource.Rows.Add(xdr);
                    }
                }
                grd.ItemsSource = iGridDataSource;
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Error", "Error", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            finally
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }
        }

        private void Btnimport_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                if (this.iGridDataSource != null && this.iGridDataSource.Rows.Count > 0)
                {
                    string ketqua = "";
                    foreach (DataRow mdr in this.iGridDataSource.Rows)
                    {
                        if (Convert.ToBoolean(mdr["CHK"]) == true)
                        {
                            #region Thuc hien thuat toan cho tung mon hoc

                            DataTable iGridDataSource = bus.GetDiemMau(frm_DangKyHocPhan.Masinhvien,
                                Convert.ToInt32(mdr["ID_MONHOC"]));
                            DataTable Diem_SV = bus.GetDiem_SV(frm_DangKyHocPhan.Masinhvien,
                                Convert.ToInt32(mdr["ID_MONHOC"]));
                            double diem_dudoan = TuVanMH(iGridDataSource, Diem_SV);

                            if (Double.IsNaN(diem_dudoan))
                            {
                                #region Không tìm được điểm chính xác

                                #region Tao lai du lieu mau

                                foreach (DataRow dr in iGridDataSource.Rows)
                                {
                                    foreach (DataColumn xdc in iGridDataSource.Columns)
                                    {
                                        if (!string.IsNullOrEmpty(dr[xdc.ColumnName].ToString()))
                                        {
                                            if (Convert.ToDouble(dr[xdc.ColumnName]) < (double)4)
                                            {
                                                dr[xdc.ColumnName] = 4; // 0 -> 3.9
                                            }
                                            else
                                            {
                                                if (Convert.ToDouble(dr[xdc.ColumnName]) < (double)5)
                                                {
                                                    dr[xdc.ColumnName] = 5; // 4.0 -> 4.9
                                                }
                                                else
                                                {
                                                    if (Convert.ToDouble(dr[xdc.ColumnName]) < (double)5.5)
                                                    {
                                                        dr[xdc.ColumnName] = 5.5; // 5.0 -> 5.4
                                                    }
                                                    else
                                                    {
                                                        if (Convert.ToDouble(dr[xdc.ColumnName]) < (double)6.5)
                                                        {
                                                            dr[xdc.ColumnName] = 6.5; // 5.5 -> 6.4
                                                        }
                                                        else
                                                        {
                                                            if (Convert.ToDouble(dr[xdc.ColumnName]) <
                                                                (double)7)
                                                            {
                                                                dr[xdc.ColumnName] = 7; // 6.5 -> 6.9
                                                            }
                                                            else
                                                            {
                                                                if (Convert.ToDouble(dr[xdc.ColumnName]) <
                                                                    (double)8)
                                                                {
                                                                    dr[xdc.ColumnName] = 8; // 7.0 -> 7.9
                                                                }
                                                                else
                                                                {
                                                                    if (Convert.ToDouble(dr[xdc.ColumnName]) <
                                                                        (double)8.5)
                                                                    {
                                                                        dr[xdc.ColumnName] = 8.5; // 8.0 -> 8.4
                                                                    }
                                                                    else
                                                                    {
                                                                        dr[xdc.ColumnName] = 10; // 8.5 -> 10
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                #endregion

                                if (iGridDataSource.Rows.Count < 1)
                                {
                                    ketqua = ketqua + "Dự đoán kết quả môn học " + mdr["TEN_MONHOC"].ToString() + " :" +
                                             "\n- Kết quả: Không thể dự đoán \n- Điểm dự đoán: Không có dữ liệu mẫu nên không thể dự đoán được\n\n";
                                }
                                else
                                {
                                    //4-F 5-D 5.5-D+ 6.5-C 7-C+ 8-B 8.5-B+ 10-A
                                    #region Tao lai diem DK

                                    foreach (DataColumn xdc in Diem_SV.Columns)
                                    {
                                        if (string.IsNullOrEmpty(Diem_SV.Rows[0][xdc.ColumnName].ToString()))
                                        {
                                            Diem_SV.Rows[0][xdc.ColumnName] = 4;
                                        }
                                        else
                                        {
                                            if (Convert.ToDouble(Diem_SV.Rows[0][xdc.ColumnName]) < (double)4)
                                            {
                                                Diem_SV.Rows[0][xdc.ColumnName] = 4;
                                            }
                                            else
                                            {
                                                if (Convert.ToDouble(Diem_SV.Rows[0][xdc.ColumnName]) < (double)5)
                                                {
                                                    Diem_SV.Rows[0][xdc.ColumnName] = 5;
                                                }
                                                else
                                                {
                                                    if (Convert.ToDouble(Diem_SV.Rows[0][xdc.ColumnName]) < (double)5.5)
                                                    {
                                                        Diem_SV.Rows[0][xdc.ColumnName] = 5.5;
                                                    }
                                                    else
                                                    {
                                                        if (Convert.ToDouble(Diem_SV.Rows[0][xdc.ColumnName]) <
                                                            (double)6.5)
                                                        {
                                                            Diem_SV.Rows[0][xdc.ColumnName] = 6.5;
                                                        }
                                                        else
                                                        {
                                                            if (Convert.ToDouble(Diem_SV.Rows[0][xdc.ColumnName]) <
                                                                (double)7)
                                                            {
                                                                Diem_SV.Rows[0][xdc.ColumnName] = 7;
                                                            }
                                                            else
                                                            {
                                                                if (Convert.ToDouble(Diem_SV.Rows[0][xdc.ColumnName]) <
                                                                    (double)8)
                                                                {
                                                                    Diem_SV.Rows[0][xdc.ColumnName] = 8;
                                                                }
                                                                else
                                                                {
                                                                    if (
                                                                        Convert.ToDouble(Diem_SV.Rows[0][xdc.ColumnName]) <
                                                                        (double)8.5)
                                                                    {
                                                                        Diem_SV.Rows[0][xdc.ColumnName] = 8.5;
                                                                    }
                                                                    else
                                                                    {
                                                                        Diem_SV.Rows[0][xdc.ColumnName] = 10;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    #endregion

                                    diem_dudoan = TuVanMH(iGridDataSource, Diem_SV);
                                    if (diem_dudoan == 4)
                                    {
                                        ketqua = ketqua + "Dự đoán kết quả môn học " + mdr["TEN_MONHOC"].ToString() +
                                                 " :" +
                                                 "\n- Kết quả: Không đạt \n- Điểm dự đoán: 0 -> 3.9 (F)\n\n";
                                    }
                                    if (diem_dudoan == 5)
                                    {
                                        ketqua = ketqua + "Dự đoán kết quả môn học " + mdr["TEN_MONHOC"].ToString() +
                                                 " :" +
                                                 "\n- Kết quả: Đạt \n- Điểm dự đoán: 4.0 -> 4.9 (D) \n\n";
                                    }
                                    if (diem_dudoan == 5.5)
                                    {
                                        ketqua = ketqua + "Dự đoán kết quả môn học " + mdr["TEN_MONHOC"].ToString() +
                                                 " :" +
                                                 "\n- Kết quả: Đạt \n- Điểm dự đoán: 5.0 -> 5.4 (D+) \n\n";
                                    }
                                    if (diem_dudoan == 6.5)
                                    {
                                        ketqua = ketqua + "Dự đoán kết quả môn học " + mdr["TEN_MONHOC"].ToString() +
                                                 " :" +
                                                 "\n- Kết quả: Đạt \n- Điểm dự đoán: 5.5 -> 6.4 (C) \n\n";
                                    }
                                    if (diem_dudoan == 7)
                                    {
                                        ketqua = ketqua + "Dự đoán kết quả môn học " + mdr["TEN_MONHOC"].ToString() +
                                                 " :" +
                                                 "\n- Kết quả: Đạt \n- Điểm dự đoán: 6.5 -> 6.9 (C+) \n\n";
                                    }
                                    if (diem_dudoan == 8)
                                    {
                                        ketqua = ketqua + "Dự đoán kết quả môn học " + mdr["TEN_MONHOC"].ToString() +
                                                 " :" +
                                                 "\n- Kết quả: Đạt \n- Điểm dự đoán: 7.0 -> 7.9 (B) \n\n";
                                    }
                                    if (diem_dudoan == 8.5)
                                    {
                                        ketqua = ketqua + "Dự đoán kết quả môn học " + mdr["TEN_MONHOC"].ToString() +
                                                 " :" +
                                                 "\n- Kết quả: Đạt \n- Điểm dự đoán: 8.0 -> 8.4 (B+) \n\n";
                                    }
                                    if (diem_dudoan == 10)
                                    {
                                        ketqua = ketqua + "Dự đoán kết quả môn học " + mdr["TEN_MONHOC"].ToString() +
                                                 " :" +
                                                 "\n- Kết quả: Đạt \n- Điểm dự đoán: 8.5 -> 10 (A) \n\n";
                                    }
                                    if (Double.IsNaN(diem_dudoan))
                                    {
                                        #region Xét đậu rớt khi không tìm được điểm

                                        foreach (DataRow dr in iGridDataSource.Rows)
                                        {
                                            if (Convert.ToDouble(dr[mdr["ID_MONHOC"].ToString()]) == (double)4)
                                            {
                                                dr[mdr["ID_MONHOC"].ToString()] = 0;
                                            }
                                            else
                                            {
                                                dr[mdr["ID_MONHOC"].ToString()] = 10;
                                            }
                                        }
                                        diem_dudoan = TuVanMH(iGridDataSource, Diem_SV);
                                        if (diem_dudoan == 10)
                                        {
                                            ketqua = ketqua + "Dự đoán kết quả môn học " + mdr["TEN_MONHOC"].ToString() +
                                                     " :" +
                                                     "\n- Kết quả: Đạt \n- Điểm dự đoán: Dữ liệu quá ít không thể dự đoán được\n\n";
                                        }
                                        else
                                        {
                                            ketqua = ketqua + "Dự đoán kết quả môn học " + mdr["TEN_MONHOC"].ToString() +
                                                     " :" +
                                                     "\n- Kết quả: Không đạt \n- Điểm dự đoán: Dữ liệu quá ít không thể dự đoán được\n\n";
                                        }

                                        #endregion
                                    }
                                }

                                #endregion
                            }
                            else
                            {
                                #region Diem chu

                                string diemchu = "";
                                if (diem_dudoan < (double)4)
                                {
                                    diemchu = " 0 -> 3.9 (F)";
                                }
                                else
                                {
                                    if (diem_dudoan < (double)5)
                                    {
                                        diemchu = " 4.0 -> 4.9 (D)";
                                    }
                                    else
                                    {
                                        if (diem_dudoan < (double)5.5)
                                        {
                                            diemchu = " 5.0 -> 5.4 (D+)";
                                        }
                                        else
                                        {
                                            if (diem_dudoan < (double)6.5)
                                            {
                                                diemchu = " 5.5 -> 6.4 (C)";
                                            }
                                            else
                                            {
                                                if (diem_dudoan < (double)7)
                                                {
                                                    diemchu = " 6.5 -> 6.9 (C+)";
                                                }
                                                else
                                                {
                                                    if (diem_dudoan < (double)8)
                                                    {
                                                        diemchu = " 7.0 -> 7.9 (B)";
                                                    }
                                                    else
                                                    {
                                                        if (diem_dudoan < (double)8.5)
                                                        {
                                                            diemchu = " 8.0 -> 8.4 (B+)";
                                                        }
                                                        else
                                                        {
                                                            diemchu = " 8.5 -> 10 (A)";
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                #endregion

                                if (diem_dudoan >= (double)4)
                                {
                                    ketqua = ketqua + "Dự đoán kết quả môn học " + mdr["TEN_MONHOC"].ToString() + " :" +
                                             "\n- Kết quả: Đạt \n- Điểm dự đoán:" + diemchu + "\n\n";
                                }
                                else
                                {
                                    ketqua = ketqua + "Dự đoán kết quả môn học " + mdr["TEN_MONHOC"].ToString() + " :" +
                                             "\n- Kết quả: Không đạt \n- Điểm dự đoán:" + diemchu +
                                             "\n\n";
                                }
                            }
                            TXTkq.Text = ketqua;

                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Error", "Error", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            finally
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }
        }

        private double TuVanMH(DataTable iData_DiemMau, DataTable iData_DiemDieuKien)
        {
            try
            {
                #region Thuc hien thuat toan cho tung mon hoc

                DataTable iGridDataSource = iData_DiemMau;

                List<double> ds_diemTK = new List<double>(); //Danh sach cac bo diem TK co trong bang du lieu

                #region Lay danh sach diem KET QUA MON DANG TU VAN

                foreach (DataRow drTK in iGridDataSource.Rows)
                {
                    int check = 0;
                    foreach (double kt in ds_diemTK)
                    {
                        if (kt == Convert.ToDouble(drTK[iGridDataSource.Columns.Count - 1]))
                        {
                            check = 1;
                        }
                    }
                    if (check == 0)
                    {
                        ds_diemTK.Add(Convert.ToDouble(drTK[iGridDataSource.Columns.Count - 1]));
                    }
                }
                if (ds_diemTK.Count % 2 != 0)
                {
                    ds_diemTK.Add(ds_diemTK[0]);
                }

                #endregion

                string xcolumn = iGridDataSource.Columns[iGridDataSource.Columns.Count - 1].ColumnName; // Ten cot diem TK mon dang xet
                iData_AllTree.Clear();

                #region Tao tree

                for (int id = 0; id < ds_diemTK.Count - 1; id += 2)
                {
                    #region Tao Attribute

                    List<DATN.C45.Attribute> Attributes = new List<DATN.C45.Attribute>(); // danh sach thuoc tinh
                    for (int i = 0; i < iGridDataSource.Columns.Count - 1; i++)
                    {
                        DATN.C45.Attribute x = new DATN.C45.Attribute();
                        x.Name = iGridDataSource.Columns[i].ColumnName;
                        Attributes.Add(x);
                    }

                    #endregion

                    #region Duyet 1 lan 2 gia tri diem TK de tao tree
                    DataTable iDataXet = null;
                    var filteredRows = from row in iGridDataSource.AsEnumerable()
                                       where row.Field<double>(xcolumn) == ds_diemTK[id] || row.Field<double>(xcolumn) == ds_diemTK[id + 1]
                                       select row;
                    DataRow[] xcheck = filteredRows.ToArray();
                    //DataRow[] xcheck = iGridDataSource.Select("Convert(" + xcolumn + ",'System.Double')= " + ds_diemTK[id].ToString() + " or Convert(" + xcolumn + ",'System.Double')= " + ds_diemTK[id + 1].ToString());
                    iDataXet = xcheck.CopyToDataTable();

                    #endregion

                    #region Tao tree

                    for (int i = 0; i < iDataXet.Rows.Count; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            int check = 0;
                            for (int z = 0; z < Attributes[j].Value.Count; z++)
                            {
                                if (Convert.ToDouble(Attributes[j].Value[z]) == Convert.ToDouble(iDataXet.Rows[i][j]))
                                {
                                    check = 1;
                                }
                            }
                            if (check == 0)
                            {
                                Attributes[j].Value.Add(Convert.ToDouble(iDataXet.Rows[i][j]));
                            }
                        }
                    }

                    #region Tao ds dieu kien mau

                    List<List<double>> Examples = new List<List<double>>();
                    foreach (DataRow xdr in iDataXet.Rows)
                    {
                        List<double> example = new List<double>();
                        for (int i = 0; i < iDataXet.Columns.Count; i++)
                        {
                            example.Add(Convert.ToDouble(xdr[i]));
                        }
                        Examples.Add(example);
                    }

                    #endregion

                    List<DATN.C45.Attribute> at = new List<DATN.C45.Attribute>();
                    for (int i = 0; i < Attributes.Count; i++)
                    {
                        at.Add(Attributes[i]);
                    }
                    List<double> ds_tmp = new List<double>();
                    for (int i = 0; i < Examples.Count; i++)
                    {
                        int check = 0;
                        foreach (double value in ds_tmp)
                        {
                            if (Examples[i][Examples[i].Count - 1] == value)
                            {
                                check = 1;
                            }
                        }
                        if (check == 0)
                            ds_tmp.Add(Examples[i][Examples[i].Count - 1]);
                    }
                    DATN.C45.DecisionTree_C45 DTID3 = new DecisionTree_C45(Examples, at, ds_diemTK[id], ds_diemTK[id + 1]);
                    DTID3.GetTree();
                    iData_AllTree.Add(DTID3);
                    #endregion
                }

                #endregion

                #region Duyet tree lay gia tri

                DataTable Diem_SV = iData_DiemDieuKien;

                List<double> result = new List<double>();

                foreach (DATN.C45.DecisionTree_C45 d in iData_AllTree)
                {
                    result = d.SearchTree(d.Tree, Diem_SV, result, false);
                }
                double tong = 0;
                string ds_dudoan = "";
                foreach (double b in result)
                {
                    ds_dudoan = ds_dudoan + "\n" + b.ToString();
                    tong += b;
                }
                double diem_dudoan = (double)tong / result.Count;

                #endregion

                return diem_dudoan;

                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
