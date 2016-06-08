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
        List<DATN.ID3.Attribute> Attributes = new List<DATN.ID3.Attribute>(); // danh sach thuoc tinh
        DATN.ID3.TTS_ID3 DTID3; //Khoi tao 1 tree
        List<DATN.ID3.TTS_ID3> iData_AllTree = new List<TTS_ID3>(); // Danh sach tat ca cac tree sau khi thuc hien thuat toan
        List<List<double>> Examples = new List<List<double>>(); // danh sach dieu kien

        public frm_ID3()
        {
            InitializeComponent();
        }

        private void Btnimport_OnClick(object sender, RoutedEventArgs e)
        {
            #region Du lieu test

            DataTable iGridDataSource = new DataTable();
            iGridDataSource.Columns.Add("MH1", typeof(double));
            iGridDataSource.Columns.Add("MH2", typeof(double));
            iGridDataSource.Columns.Add("MH3", typeof(double));
            iGridDataSource.Columns.Add("MH4", typeof(double));
            iGridDataSource.Columns.Add("MH5", typeof(double));
            DataRow dr = null;
            dr = iGridDataSource.NewRow();
            dr["MH1"] = 3;
            dr["MH2"] = 4.5;
            dr["MH3"] = 8.6;
            dr["MH4"] = 8.5;
            dr["MH5"] = 0;
            iGridDataSource.Rows.Add(dr);

            dr = iGridDataSource.NewRow();
            dr["MH1"] = 3;
            dr["MH2"] = 7;
            dr["MH3"] = 5.5;
            dr["MH4"] = 8.5;
            dr["MH5"] = 0;
            iGridDataSource.Rows.Add(dr);

            dr = iGridDataSource.NewRow();
            dr["MH1"] = 6;
            dr["MH2"] = 7;
            dr["MH3"] = 5.5;
            dr["MH4"] = 8.5;
            dr["MH5"] = 10;
            iGridDataSource.Rows.Add(dr);

            dr = iGridDataSource.NewRow();
            dr["MH1"] = 10;
            dr["MH2"] = 4.5;
            dr["MH3"] = 5.5;
            dr["MH4"] = 8.5;
            dr["MH5"] = 10;
            iGridDataSource.Rows.Add(dr);

            dr = iGridDataSource.NewRow();
            dr["MH1"] = 6;
            dr["MH2"] = 9.5;
            dr["MH3"] = 8.6;
            dr["MH4"] = 8;
            dr["MH5"] = 10;
            iGridDataSource.Rows.Add(dr);

            dr = iGridDataSource.NewRow();
            dr["MH1"] = 3;
            dr["MH2"] = 4.5;
            dr["MH3"] = 8.6;
            dr["MH4"] = 8;
            dr["MH5"] = 10;
            iGridDataSource.Rows.Add(dr);

            dr = iGridDataSource.NewRow();
            dr["MH1"] = 10;
            dr["MH2"] = 7;
            dr["MH3"] = 5.5;
            dr["MH4"] = 8;
            dr["MH5"] = 10;
            iGridDataSource.Rows.Add(dr);

            dr = iGridDataSource.NewRow();
            dr["MH1"] = 3;
            dr["MH2"] = 7;
            dr["MH3"] = 5.5;
            dr["MH4"] = 8;
            dr["MH5"] = 0;
            iGridDataSource.Rows.Add(dr);

            dr = iGridDataSource.NewRow();
            dr["MH1"] = 10;
            dr["MH2"] = 9.5;
            dr["MH3"] = 8.6;
            dr["MH4"] = 8;
            dr["MH5"] = 0;
            iGridDataSource.Rows.Add(dr);

            dr = iGridDataSource.NewRow();
            dr["MH1"] = 3;
            dr["MH2"] = 4.5;
            dr["MH3"] = 5.5;
            dr["MH4"] = 8;
            dr["MH5"] = 10;
            iGridDataSource.Rows.Add(dr);

            dr = iGridDataSource.NewRow();
            dr["MH1"] = 6;
            dr["MH2"] = 9.5;
            dr["MH3"] = 5.5;
            dr["MH4"] = 8;
            dr["MH5"] = 0;
            iGridDataSource.Rows.Add(dr);

            dr = iGridDataSource.NewRow();
            dr["MH1"] = 6;
            dr["MH2"] = 9.5;
            dr["MH3"] = 5.5;
            dr["MH4"] = 8;
            dr["MH5"] = 5;
            iGridDataSource.Rows.Add(dr);

            dr = iGridDataSource.NewRow();
            dr["MH1"] = 10;
            dr["MH2"] = 7;
            dr["MH3"] = 5.5;
            dr["MH4"] = 8;
            dr["MH5"] = 8;
            iGridDataSource.Rows.Add(dr); 
            #endregion

            #region Tao Attribute

            DATN.ID3.Attribute x1 = new DATN.ID3.Attribute();
            x1.Name = "MH1";
            Attributes.Add(x1);
            DATN.ID3.Attribute x2 = new DATN.ID3.Attribute();
            x2.Name = "MH2";
            Attributes.Add(x2);
            DATN.ID3.Attribute x3 = new DATN.ID3.Attribute();
            x3.Name = "MH3";
            Attributes.Add(x3);
            DATN.ID3.Attribute x4 = new DATN.ID3.Attribute();
            x4.Name = "MH4";
            Attributes.Add(x4); 

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

            DataTable iDataXet = null;
            string xcolumn = iGridDataSource.Columns[iGridDataSource.Columns.Count - 1].ColumnName; // Ten cot diem TK mon dang xet

            #region Tao tree

            for (int id = 0; id < ds_diemTK.Count - 1; id += 2)
            {
                #region Duyet 1 lan 2 gia tri diem TK de tao tree

                DataRow[] xcheck = iGridDataSource.Select(xcolumn + "= " + ds_diemTK[id] + " or " + xcolumn + "= " + ds_diemTK[id + 1]);
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

                Examples.Clear();
                foreach (DataRow xdr in iDataXet.Rows)
                {
                    List<double> example = new List<double>();
                    for (int i = 0; i < iDataXet.Columns.Count; i++)
                    {
                        example.Add(Convert.ToDouble(xdr[i]));
                    }
                    Examples.Add(example);
                }
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
                DTID3 = new TTS_ID3(Examples, at, ds_diemTK[id], ds_diemTK[id+1]);
                DTID3.GetTree();
                iData_AllTree.Add(DTID3);
                #endregion
            }

            #endregion

            #region Duyet tree lay gia tri

            DataTable xxx = iGridDataSource.Clone();
            xxx.Rows.Add(xxx.NewRow());
            xxx.Rows[0][0] = 10;
            xxx.Rows[0][1] = 4;
            xxx.Rows[0][2] = 8;
            xxx.Rows[0][3] = 8;

            List<double> result =new List<double>();

            foreach (DATN.ID3.TTS_ID3 d in iData_AllTree)
            {
                result = d.SearchTree(d.Tree, xxx, result,false);
            }
            double tong = 0;
            foreach (double b in result)
            {
                tong += b;
            }
            double diem_dudoan = (double)tong/result.Count;
            CTMessagebox.Show(diem_dudoan.ToString(), "Dự đoán", "", CTICON.Information, CTBUTTON.OK);
            #endregion
        }

    }
}
