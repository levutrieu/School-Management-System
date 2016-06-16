using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpf.Editors;
namespace DATN.TTS.TVMH.Resource
{
    public enum SelectionTypeEnum
    {
        Native,
        Checked,
        Radio,
    }
    public class ComboBoxUtil
    {
        public static void InsertItem(ComboBoxEdit comboBoxEdit, string displayData, string valueData, int positionIndex, bool isSelected)
        {
            DataTable table = ((DataView)comboBoxEdit.ItemsSource).Table;
            DataRow row = table.NewRow();
            row[comboBoxEdit.DisplayMember] = (object)displayData;
            row[comboBoxEdit.ValueMember] = (object)valueData;
            table.Rows.InsertAt(row, positionIndex);
            table.AcceptChanges();
            if (!isSelected)
                return;
            comboBoxEdit.SelectedIndex = positionIndex;
        }
        private static DataTable CopyColumn(DataTable srcTable, DataTable dstTable, string srcColName, string dstColName)
        {
            try
            {
                dstTable.Columns.Add(dstColName);
                dstTable.Columns.Add(srcColName);
                foreach (DataRow dataRow in (InternalDataCollectionBase)srcTable.Rows)
                {
                    DataRow row = dstTable.NewRow();
                    row[dstColName] = dataRow[srcColName];
                    row[srcColName] = dataRow[srcColName];
                    dstTable.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dstTable;
        }
        public static void SetComboBoxEdit(ComboBoxEdit comboBoxEdit, string displayName, string valueName, DataTable dataSource, SelectionTypeEnum selectionTypeEnum)
        {
            if (selectionTypeEnum == SelectionTypeEnum.Checked)
                comboBoxEdit.StyleSettings = (BaseEditStyleSettings)new CheckedComboBoxStyleSettings();
            else if (selectionTypeEnum == SelectionTypeEnum.Radio)
                comboBoxEdit.StyleSettings = (BaseEditStyleSettings)new RadioComboBoxStyleSettings();
            string srcColName = displayName;
            string dstColName = valueName;
            if (srcColName == dstColName)
            {
                dstColName += "_";
                comboBoxEdit.DisplayMember = srcColName;
                comboBoxEdit.ValueMember = dstColName;
                dataSource = ComboBoxUtil.CopyColumn(dataSource, new DataTable("Table"), srcColName, dstColName);
            }
            else
            {
                comboBoxEdit.DisplayMember = srcColName;
                comboBoxEdit.ValueMember = dstColName;
            }
            comboBoxEdit.ItemsSource = (object)dataSource.DefaultView.ToTable(1 != 0, new string[2]{srcColName,dstColName}).DefaultView;
        }
    }
}
