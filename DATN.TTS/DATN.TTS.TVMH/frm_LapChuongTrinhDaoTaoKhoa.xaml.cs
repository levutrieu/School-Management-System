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
using CustomMessage;
using DevExpress.Xpf.WindowsUI.Base;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_LapChuongTrinhDaoTaoKhoa.xaml
    /// </summary>
    public partial class frm_LapChuongTrinhDaoTaoKhoa : Page
    {
        public frm_LapChuongTrinhDaoTaoKhoa()
        {
            InitializeComponent();
        }
        private void SetMoveButtonVisibility()
        {
            btnNext.Visibility = (PageController.SelectedIndex == PageController.Items.Count - 1 ? Visibility.Hidden : Visibility.Visible);
            btnPrev.Visibility = (PageController.SelectedIndex == 0 ? Visibility.Hidden : Visibility.Visible);
        }

        private void BindingUI(int index)
        {
            switch (index)
            {
                case 0:
                    break;
                case 1:
                    DataTable dtKhoa = frm_ChonKhoa.iDataSoure;
                    DataTable dtKhoaNganh = frm_KhoaNganh.iDataSoure;
                    if (dtKhoa.Rows.Count > 0 && dtKhoaNganh.Rows.Count > 0)
                    {
                        dtKhoaNganh.Rows[0]["ID_HE_DAOTAO"] = dtKhoa.Rows[0]["ID_HE_DAOTAO"];
                        dtKhoaNganh.Rows[0]["TEN_HE_DAOTAO"] = dtKhoa.Rows[0]["TEN_HE_DAOTAO"];
                        dtKhoaNganh.Rows[0]["ID_KHOAHOC"] = dtKhoa.Rows[0]["ID_KHOAHOC"];
                        dtKhoaNganh.Rows[0]["TEN_KHOAHOC"] = dtKhoa.Rows[0]["TEN_KHOAHOC"];
                        DataTable dt = frm_KhoaNganh.client.GetNganhWhereHDT(Convert.ToInt32(dtKhoa.Rows[0]["ID_KHOAHOC"].ToString()));
                        frm_KhoaNganh.LoadKhoaNganh();
                        
                        frm_KhoaNganh.LoadNganh(dt);
                    }
                    break;
                case 2:
                    DataTable xdtKhoaNganh = frm_KhoaNganh.iDataSoure;
                    DataTable xdtKhoaNganhCt = frm_KhungNganhDaoTaoKhoa.iDataSoure;
                    if (xdtKhoaNganh.Rows.Count > 0 && xdtKhoaNganhCt.Rows.Count > 0)
                    {
                        xdtKhoaNganhCt.Rows[0]["ID_HE_DAOTAO"] = xdtKhoaNganh.Rows[0]["ID_HE_DAOTAO"];
                        xdtKhoaNganhCt.Rows[0]["TEN_HE_DAOTAO"] = xdtKhoaNganh.Rows[0]["TEN_HE_DAOTAO"];
                        xdtKhoaNganhCt.Rows[0]["ID_KHOAHOC"] = xdtKhoaNganh.Rows[0]["ID_KHOAHOC"];
                        xdtKhoaNganhCt.Rows[0]["TEN_KHOAHOC"] = xdtKhoaNganh.Rows[0]["TEN_KHOAHOC"];
                        xdtKhoaNganhCt.Rows[0]["KHOAHOC_NGANH"] = xdtKhoaNganh.Rows[0]["KHOAHOC_NGANH"];
                        xdtKhoaNganhCt.Rows[0]["ID_KHOAHOC_NGANH"] = xdtKhoaNganh.Rows[0]["ID_KHOAHOC_NGANH"];

                        frm_KhungNganhDaoTaoKhoa.LoadMonHoc();
                        frm_KhungNganhDaoTaoKhoa.LoadKhoaNganhCT();
                    }
                    break;
            }
        }

        private void PageController_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                int index = PageController.SelectedIndex;
                if (index != 0)
                {
                    if (index == 1)
                    {
                        if (frm_ChonKhoa.iDataSoure == null || frm_ChonKhoa.iDataSoure.Rows.Count == 0 || 
                            frm_ChonKhoa.iGridDataSoure == null || frm_ChonKhoa.iGridDataSoure.Rows.Count ==0)
                        {
                            CTMessagebox.Show("Vui lòng chọn khóa học!" + "\n" + " Trước khi bước sang bước tiếp", "Thông báo", "",CTICON.Information, CTBUTTON.YesNo);
                            DataTable dt = new DataTable();
                            frm_KhoaNganh.LoadNganh(dt);
                            return;
                        }
                        SetMoveButtonVisibility();
                        BindingUI(index);
                    }
                    if (index == 2)
                    {
                        if (frm_KhungNganhDaoTaoKhoa.iDataSoure == null || frm_KhungNganhDaoTaoKhoa.iDataSoure.Rows.Count == 0 ||
                            frm_KhoaNganh.iGridDataSoureKhoaNganh == null || frm_KhoaNganh.iGridDataSoureKhoaNganh.Rows.Count == 0)
                        {
                            CTMessagebox.Show("Vui lòng chọn khóa ngành!" +"\n"+" Trước khi bước sang bước tiếp", "Thông báo", "",CTICON.Information, CTBUTTON.YesNo);
                            return;
                        }
                        if (string.IsNullOrEmpty(frm_KhoaNganh.iDataSoure.Rows[0]["ID_KHOAHOC_NGANH"].ToString()))
                        {
                            CTMessagebox.Show("Vui lòng chọn khóa ngành!" + "\n" + " Trước khi bước sang bước tiếp", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo);
                            return;
                        }
                        DataTable xdt = frm_KhoaNganh.iGridDataSoureKhoaNganh;
                        int row = 0;
                        foreach (DataRow dr in xdt.Rows)
                        {
                            if (dr["ID_KHOAHOC_NGANH"].ToString() == "0")
                                row++;
                        }
                        if (row > 0)
                        {
                            if (CTMessagebox.Show("Có " + row + " dòng chưa lưu." + "\n" + "Bạn có muốn lưu không?", "Thông báo", "", CTICON.Information, CTBUTTON.YesNo) == CTRESPONSE.Yes)
                            {
                                frm_KhoaNganh.BtnThemKhoaNganh_OnClick(null, null);
                            }
                        }
                        SetMoveButtonVisibility();
                        BindingUI(index);
                    }
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void btnNext_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int index = PageController.SelectedIndex;
            if (frm_ChonKhoa.iDataSoure == null || frm_ChonKhoa.iDataSoure.Rows.Count == 0)
            {
                CTMessagebox.Show("Vui lòng chọn khóa học trước khi bước sang bước tiếp", "Thông báo", "",
                    CTICON.Information, CTBUTTON.YesNo);
                return;
            }
            if (PageController.SelectedIndex < PageController.Items.Count - 1)
            {
                if (index == 2)
                {
                    
                }
                else
                     PageController.Select((ISelectorItem)PageController.Items[PageController.SelectedIndex + 1]);
            }
            SetMoveButtonVisibility();
        }

        private void btnPrev_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (PageController.SelectedIndex > 0)
                PageController.SelectedIndex--;

            SetMoveButtonVisibility();
        }
    }
}
