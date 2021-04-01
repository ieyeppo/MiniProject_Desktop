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
    public partial class AddStore : Page
    {
        public AddStore()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LblStoreName.Visibility = LblStoreLocation.Visibility = LblResult.Visibility = Visibility.Hidden;


                TxtStoreID.Text = "";
            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 AddStore Loaded : {ex}");
                throw ex;
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true; // 입력된 값이 모두 만족하는지 판별하는 플래그

            LblStoreName.Visibility = LblStoreLocation.Visibility = LblResult.Visibility = Visibility.Hidden;

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
            

            if (isValid)
            {
                store.StoreName = TxtStoreName.Text;
                store.StoreLocation = TxtStoreLocation.Text;
/*                store.ItemStatus = bool.Parse(CboItemStatus.SelectedValue.ToString());
                store.TagID = int.Parse(TxtTagID.Text);
                store.BarcodeID = int.Parse(TxtBarcodeID.Text);*/

                try
                {
                    var result = Logic.DataAccess.SetStore(store);

                    if(result == 0)
                    {
                        LblResult.Text = "창고 추가에 문제가 발생했습니다. 관리자에게 문의 바랍니다.";
                        LblResult.Foreground = Brushes.Red;
                    }
                    else
                    {
                        NavigationService.GoBack();
                    }
                }
                catch (Exception ex)
                {
                    Commons.LOGGER.Error($"예외발생 AddUser: {ex}");
                }

            }
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