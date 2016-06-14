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
namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_C45.xaml
    /// </summary>
    public partial class frm_C45 : Window
    {
        List<DATN.C45.DecisionTree_C45> iData_AllTree = new List<DATN.C45.DecisionTree_C45>(); // Danh sach tat ca cac tree sau khi thuc hien thuat toan

        bus_ID3 bus = new bus_ID3();

        public frm_C45()
        {
            InitializeComponent();
        }

        private void Btnimport_OnClick(object sender, RoutedEventArgs e)
        {

            bus_ID3 bus = new bus_ID3();

            DataTable iGridDataSource = bus.GetDiemMau();

            List<double> ds_diemTK = new List<double>(); //Danh sach cac bo diem TK co trong bang du lieu

            #region Lay danh sach diem KET QUA MON TU CHON dang xet
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

            DataTable xxx = iGridDataSource.Clone();
            xxx.Rows.Add(xxx.NewRow());
            xxx.Rows[0][0] = 6.3;
            xxx.Rows[0][1] = 9.3;
            xxx.Rows[0][2] = 7.2;
            xxx.Rows[0][3] = 4.8;
            xxx.Rows[0][4] = 8.7;
            //xxx.Rows[0][5] = 8.1;

            List<double> result = new List<double>();

            foreach (DATN.C45.DecisionTree_C45 d in iData_AllTree)
            {
                result = d.SearchTree(d.Tree, xxx, result, false);
            }
            double tong = 0;
            string ds_dudoan = "";
            foreach (double b in result)
            {
                ds_dudoan = ds_dudoan + "\n" + b.ToString();
                tong += b;
            }
            double diem_dudoan = (double)tong / result.Count;
            //CTMessagebox.Show("Danh sách điểm có thể đạt được: "+ds_dudoan +"\n\n Điểm dự đoán: "+diem_dudoan.ToString(), "Dự đoán", "", CTICON.Information, CTBUTTON.OK);
            TXTkq.Text = "Danh sách điểm có thể đạt được: " + ds_dudoan + "\n\n Điểm dự đoán: " + diem_dudoan.ToString();

            #endregion
        }
    }
}
