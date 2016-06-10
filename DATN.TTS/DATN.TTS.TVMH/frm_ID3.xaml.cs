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
using DATN.ID3;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_ID3.xaml
    /// </summary>
    public partial class frm_ID3 : Page
    {
        
        List<DATN.ID3.TTS_ID3> iData_AllTree = new List<TTS_ID3>(); // Danh sach tat ca cac tree sau khi thuc hien thuat toan
        

        public frm_ID3()
        {
            InitializeComponent();
        }

        private void Btnimport_OnClick(object sender, RoutedEventArgs e)
        {
            #region Du lieu test

            //DataTable iGridDataSource = new DataTable();
            //iGridDataSource.Columns.Add("MH1", typeof(double));
            //iGridDataSource.Columns.Add("MH2", typeof(double));
            //iGridDataSource.Columns.Add("MH3", typeof(double));
            //iGridDataSource.Columns.Add("MH4", typeof(double));
            //iGridDataSource.Columns.Add("MH5", typeof(double));
            //DataRow dr = null;
            //dr = iGridDataSource.NewRow();
            //dr["MH1"] = 3;
            //dr["MH2"] = 4.5;
            //dr["MH3"] = 8.6;
            //dr["MH4"] = 8.5;
            //dr["MH5"] = 0;
            //iGridDataSource.Rows.Add(dr);

            //dr = iGridDataSource.NewRow();
            //dr["MH1"] = 3;
            //dr["MH2"] = 7;
            //dr["MH3"] = 5.5;
            //dr["MH4"] = 8.5;
            //dr["MH5"] = 0;
            //iGridDataSource.Rows.Add(dr);

            //dr = iGridDataSource.NewRow();
            //dr["MH1"] = 6;
            //dr["MH2"] = 7;
            //dr["MH3"] = 5.5;
            //dr["MH4"] = 8.5;
            //dr["MH5"] = 10;
            //iGridDataSource.Rows.Add(dr);

            //dr = iGridDataSource.NewRow();
            //dr["MH1"] = 10;
            //dr["MH2"] = 4.5;
            //dr["MH3"] = 5.5;
            //dr["MH4"] = 8.5;
            //dr["MH5"] = 10;
            //iGridDataSource.Rows.Add(dr);

            //dr = iGridDataSource.NewRow();
            //dr["MH1"] = 6;
            //dr["MH2"] = 9.5;
            //dr["MH3"] = 8.6;
            //dr["MH4"] = 8;
            //dr["MH5"] = 10;
            //iGridDataSource.Rows.Add(dr);

            //dr = iGridDataSource.NewRow();
            //dr["MH1"] = 3;
            //dr["MH2"] = 4.5;
            //dr["MH3"] = 8.6;
            //dr["MH4"] = 8;
            //dr["MH5"] = 10;
            //iGridDataSource.Rows.Add(dr);

            //dr = iGridDataSource.NewRow();
            //dr["MH1"] = 10;
            //dr["MH2"] = 7;
            //dr["MH3"] = 5.5;
            //dr["MH4"] = 8;
            //dr["MH5"] = 10;
            //iGridDataSource.Rows.Add(dr);

            //dr = iGridDataSource.NewRow();
            //dr["MH1"] = 3;
            //dr["MH2"] = 7;
            //dr["MH3"] = 5.5;
            //dr["MH4"] = 8;
            //dr["MH5"] = 0;
            //iGridDataSource.Rows.Add(dr);

            //dr = iGridDataSource.NewRow();
            //dr["MH1"] = 10;
            //dr["MH2"] = 9.5;
            //dr["MH3"] = 8.6;
            //dr["MH4"] = 8;
            //dr["MH5"] = 0;
            //iGridDataSource.Rows.Add(dr);

            //dr = iGridDataSource.NewRow();
            //dr["MH1"] = 3;
            //dr["MH2"] = 4.5;
            //dr["MH3"] = 5.5;
            //dr["MH4"] = 8;
            //dr["MH5"] = 10;
            //iGridDataSource.Rows.Add(dr);

            //dr = iGridDataSource.NewRow();
            //dr["MH1"] = 6;
            //dr["MH2"] = 9.5;
            //dr["MH3"] = 5.5;
            //dr["MH4"] = 8;
            //dr["MH5"] = 0;
            //iGridDataSource.Rows.Add(dr);

            //dr = iGridDataSource.NewRow();
            //dr["MH1"] = 6;
            //dr["MH2"] = 9.5;
            //dr["MH3"] = 5.5;
            //dr["MH4"] = 8;
            //dr["MH5"] = 5;
            //iGridDataSource.Rows.Add(dr);

            //dr = iGridDataSource.NewRow();
            //dr["MH1"] = 10;
            //dr["MH2"] = 7;
            //dr["MH3"] = 5.5;
            //dr["MH4"] = 8;
            //dr["MH5"] = 8;
            //iGridDataSource.Rows.Add(dr); 
            #endregion
            bus_ID3 bus = new bus_ID3();

            DataTable iGridDataSource = bus.GetDiemMau();

            
            #region Bo
            //var filteredRowsz = from row in iGridDataSource.AsEnumerable()
            //                   where row.Field<double>("417") == 5.8 || row.Field<double>("417") == 7.2
            //                   select row;
            //DataRow[] xcheckz = filteredRowsz.ToArray();
            //DataTable zzz = xcheckz.CopyToDataTable();
            //string bvvv = "";
            //for (int i = 0; i < zzz.Rows.Count; i++)
            //{
            //    for (int j = 0; j < zzz.Columns.Count; j++)
            //    {
            //        bvvv = bvvv + (" " + zzz.Rows[i][j].ToString());
            //    }
            //    bvvv = bvvv + "\n";
            //}


            //DATN.ID3.Attribute x1 = new DATN.ID3.Attribute();
            //x1.Name = "MH1";
            //Attributes.Add(x1);
            //DATN.ID3.Attribute x2 = new DATN.ID3.Attribute();
            //x2.Name = "MH2";
            //Attributes.Add(x2);
            //DATN.ID3.Attribute x3 = new DATN.ID3.Attribute();
            //x3.Name = "MH3";
            //Attributes.Add(x3);
            //DATN.ID3.Attribute x4 = new DATN.ID3.Attribute();
            //x4.Name = "MH4";
            //Attributes.Add(x4);  
            #endregion
            

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

            #region Tao tree

            for (int id = 0; id < ds_diemTK.Count - 1; id += 2)
            {
                #region Tao Attribute
                
                List<DATN.ID3.Attribute> Attributes = new List<DATN.ID3.Attribute>(); // danh sach thuoc tinh
                for (int i = 0; i < iGridDataSource.Columns.Count - 1; i++)
                {
                    DATN.ID3.Attribute x = new DATN.ID3.Attribute();
                    x.Name = iGridDataSource.Columns[i].ColumnName;
                    Attributes.Add(x);
                }

                #endregion

                #region Duyet 1 lan 2 gia tri diem TK de tao tree
                DataTable iDataXet = null;
                var filteredRows = from row in iGridDataSource.AsEnumerable()
                                   where row.Field<double>(xcolumn) == ds_diemTK[id] || row.Field<double>(xcolumn) == ds_diemTK[id+1]
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

                List<DATN.ID3.Attribute> at = new List<DATN.ID3.Attribute>();
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
                DATN.ID3.TTS_ID3 DTID3 = new TTS_ID3(Examples, at, ds_diemTK[id], ds_diemTK[id + 1]);
                DTID3.GetTree();
                iData_AllTree.Add(DTID3);
                #endregion
            }

            #endregion

            #region Duyet tree lay gia tri

            DataTable xxx = iGridDataSource.Clone();
            xxx.Rows.Add(xxx.NewRow());
            xxx.Rows[0][0] = 9;
            xxx.Rows[0][1] = 9.3;
            xxx.Rows[0][2] = 5.6;
            xxx.Rows[0][3] = 4.8;
            xxx.Rows[0][4] = 8.7;
            xxx.Rows[0][5] = 5.7;
            xxx.Rows[0][6] = 4.6;
            xxx.Rows[0][7] = 6.1;
            xxx.Rows[0][8] = 6.6;

            List<double> result = new List<double>();

            foreach (DATN.ID3.TTS_ID3 d in iData_AllTree)
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
            CTMessagebox.Show("Danh sách điểm có thể đạt được: "+ds_dudoan +"\n\n Điểm dự đoán: "+diem_dudoan.ToString(), "Dự đoán", "", CTICON.Information, CTBUTTON.OK);
            #endregion
        }

    }
}
