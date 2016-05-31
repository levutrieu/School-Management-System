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
using DATN.TTS.BUS;
using DevExpress.Utils;
using DevExpress.Xpf.Grid;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_PhanCongGV_popup.xaml
    /// </summary>
    public partial class frm_PhanCongGV_popup : Window
    {
        private int id = 0;
        private string ten = "";
        DataTable treeListDataSource = null;
        DataTable treeListDataSource_search = null;
        DataTable iDataSource = null;

        public frm_PhanCongGV_popup()
        {
            InitializeComponent();
            this.iDataSource = this.TableSchemaDefineBingding();
            this.DataContext = this.iDataSource;
            InitTreeview();
            Load_data();
        }

        private DataTable TableSchemaDefineBingding()
        {
            DataTable xDt = null;
            try
            {
                Dictionary<string, Type> xDicUser = new Dictionary<string, Type>();
                xDicUser.Add("SEARCHGV", typeof(string));
                xDt = TableUtil.ConvertToTable(xDicUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return xDt;
        }

        private void InitTreeview()
        {
            try
            {
                TreeListColumn xColumn = new TreeListColumn();
                xColumn.FieldName = "ID";
                xColumn.Visible = false;
                xColumn.AllowEditing=DefaultBoolean.False;
                listGV.Columns.Add(xColumn);

                TreeListColumn xxColumn = new TreeListColumn();
                xxColumn.FieldName = "ID_PARENT";
                xxColumn.AllowEditing = DefaultBoolean.False;
                xxColumn.Visible = false;
                listGV.Columns.Add(xxColumn);

                TreeListColumn xxxColumn = new TreeListColumn();
                xxxColumn.FieldName = "MA";
                xxxColumn.AllowEditing=DefaultBoolean.False;
                xxxColumn.Visible = false;
                listGV.Columns.Add(xxxColumn);

                TreeListColumn zColumn = new TreeListColumn();
                zColumn.FieldName = "NAME";
                zColumn.Header = "Tên giảng viên";
                zColumn.AllowEditing = DefaultBoolean.False;
                zColumn.Visible = true;
                listGV.Columns.Add(zColumn);

                //List<TreeListColumn> listcolumn = new List<TreeListColumn>();
                //TreeListColumn xColumn = new TreeListColumn();

                //xColumn.FieldName = "ID";
                //xColumn.Visible = false;
                //listcolumn.Add(xColumn);

                //xColumn.FieldName = "ID_PARENT";
                //xColumn.Visible = false;
                //listcolumn.Add(xColumn);

                //xColumn.FieldName = "MA";
                //xColumn.Visible = false;
                //listcolumn.Add(xColumn);

                //xColumn.FieldName = "NAME";
                //xColumn.Header = "Tên giảng viên";
                //xColumn.Visible = true;
                //listcolumn.Add(xColumn);

                //foreach (TreeListColumn cl in listcolumn)
                //{
                //    listGV.Columns.Add(cl);
                //}

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
                Mouse.OverrideCursor = Cursors.Wait;
                bus_phanconggiaovien bus=new bus_phanconggiaovien();
                treeListDataSource = bus.GetGV_tree();
                listGV.ItemsSource = treeListDataSource;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void TreeListViewMH_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (!e.Device.Target.Focus()) return;
                var hi = treeListViewMH.CalcHitInfo(e.OriginalSource as DependencyObject);
                DataRow row = null;
                if (hi.InRowCell)
                {
                    row = ((DataRowView)treeListViewMH.GetNodeByRowHandle(treeListViewMH.FocusedRowHandle).Content).Row;
                    if (!string.IsNullOrEmpty(row["ID_PARENT"].ToString()) && row["ID"].ToString().Substring(0, 1) == "G")
                    {
                        id = Convert.ToInt32(row["MA"]);
                        ten = row["NAME"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (id != 0)
                {
                    frm_PhanCongGV.id_giaovien = id;
                    frm_PhanCongGV.ten_giaovien = ten;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (!string.IsNullOrEmpty(this.iDataSource.Rows[0]["SEARCHGV"].ToString().Trim()))
                {
                    this.treeListDataSource_search = null;
                    this.treeListDataSource_search = this.treeListDataSource.Clone();
                    foreach (DataRow dr in treeListDataSource.Rows)
                    {
                        if (dr["NAME"].ToString().ToLower().Contains(this.iDataSource.Rows[0]["SEARCHGV"].ToString().Trim().ToLower())
                                && dr["ID"].ToString().Contains("G"))
                        {
                            treeListDataSource_search.ImportRow(dr);
                        }
                    }
                    if (this.treeListDataSource_search != null)
                    {
                        foreach (DataRow dr in treeListDataSource.Rows)
                        {
                            if (!dr["ID"].ToString().Contains("K")) continue;
                            foreach (DataRow drSearch in treeListDataSource_search.Rows)
                            {
                                if (drSearch["ID_PARENT"].ToString().Contains(dr["ID"].ToString()))
                                {
                                    treeListDataSource_search.ImportRow(dr);
                                    break;
                                }
                            }
                        }
                    }
                    listGV.ItemsSource =this.treeListDataSource_search;
                }
                else
                {
                    listGV.ItemsSource = this.treeListDataSource;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }
    }
}
