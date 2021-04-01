using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using MahApps.Metro.Controls.Dialogs;

namespace WpfSMSApp.View.Store
{
    /// <summary>
    /// MyAccount.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditStore : Page
    {
        public EditStore()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();

            try
            {
                LblStoreName.Visibility = LblStoreLocation.Visibility =
                    LblItemStatus.Visibility = LblTagID.Visibility
                    = LblBarcodeID.Visibility = LblResult.Visibility = Visibility.Hidden;


                //콤보박스 초기화
                List<string> comboValues = new List<string>
                {
                    "False", // index값 : 0
                    "True" // index값 : 1
                };
                CboItemStatus.ItemsSource = comboValues;

            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 EditStore Loaded : {ex}");
                throw ex;
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true; // 입력된 값이 모두 만족하는지 판별하는 플래그

            LblStoreName.Visibility = LblStoreLocation.Visibility =
                LblItemStatus.Visibility = LblTagID.Visibility
                = LblBarcodeID.Visibility = LblResult.Visibility = Visibility.Hidden;

            var store = new Model.Store();

            if (string.IsNullOrEmpty(TxtStoreName.Text))
            {
                LblStoreName.Visibility = Visibility.Visible;
                LblStoreName.Text = "창고명을 입력하세요";
                isValid = false;
            }
            else if (!StoreNameCheck(TxtStoreName.Text))
            {
                LblStoreName.Visibility = Visibility.Visible;
                LblStoreName.Text = "중복된 창고명 입니다..";
                isValid = false;
            }
            if (string.IsNullOrEmpty(TxtStoreLocation.Text))
            {
                LblStoreLocation.Visibility = Visibility.Visible;
                LblStoreLocation.Text = "창고위치를 입력하세요";
                isValid = false;
            }

            if (CboItemStatus.SelectedIndex < 0)
            {
                LblItemStatus.Visibility = Visibility.Visible;
                LblItemStatus.Text = "재고여부를 입력하세요";
                isValid = false;
            }
            if (string.IsNullOrEmpty(TxtTagID.Text))
            {
                LblTagID.Visibility = Visibility.Visible;
                LblTagID.Text = "태그아이디를 입력하세요";
                isValid = false;
            }
            else if (!TagIDCheck(TxtTagID.Text))
            {
                LblTagID.Visibility = Visibility.Visible;
                LblTagID.Text = "중복된 태그아이디 입니다..";
                isValid = false;
            }
            if (string.IsNullOrEmpty(TxtBarcodeID.Text))
            {
                LblBarcodeID.Visibility = Visibility.Visible;
                LblBarcodeID.Text = "바코드아이디를 입력하세요";
                isValid = false;
            }


            if (isValid)
            {
                store.StoreName = TxtStoreName.Text;
                store.StoreLocation = TxtStoreLocation.Text;
                store.ItemStatus = bool.Parse(CboItemStatus.SelectedValue.ToString());
                store.TagID = int.Parse(TxtTagID.Text);
                store.BarcodeID = int.Parse(TxtBarcodeID.Text);

                try
                {

                    var result = Logic.DataAccess.SetStore(store);

                    if (result == 0)
                    {
                        LblResult.Text = "창고정보 수정에 수정에 문제가 발생했습니다. 관리자에게 문의 바랍니다.";
                        LblResult.Foreground = Brushes.Red;
                    }
                    else
                    {
                        LblResult.Text = "창고정보를 정상적으로 수정했습니다.";
                        LblResult.Foreground = Brushes.DeepSkyBlue;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    Commons.LOGGER.Error($"예외발생 BtnUpdate_Click : {ex}");
                }

            }
        }

        private void GrdData_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            //MessageBox.Show("셀클릭");
            try
            {
                var store = GrdData.SelectedItem as Model.Store;
                TxtStoreID.Text = store.StoreID.ToString();
                TxtStoreName.Text = store.StoreName.ToString();
                TxtStoreLocation.Text = store.StoreLocation.ToString();
                CboItemStatus.SelectedIndex = store.ItemStatus == false ? 0 : 1;
                TxtTagID.Text = store.TagID.ToString();
                TxtBarcodeID.Text = store.BarcodeID.ToString();
            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 GrdData_SelectedCellsChanged : {ex}");
            }
        }

        private void LoadData()
        {
            this.DataContext = Logic.DataAccess.GetStores();
        }

        private bool StoreNameCheck(string name)
        {
            var cnt = Logic.DataAccess.GetStores().Where(s => s.StoreName.Equals(name)).Count();
            if (cnt > 0)
            {
                return false;
            }
            else return true;
        }

        private bool TagIDCheck(string tagID)
        {
            var cnt = Logic.DataAccess.GetStores().Where(s => s.TagID.Equals(tagID)).Count();
            if (cnt > 0)
            {
                return false;
            }
            else return true;
        }
    }
}