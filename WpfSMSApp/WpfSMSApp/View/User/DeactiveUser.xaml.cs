using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using MahApps.Metro.Controls;

namespace WpfSMSApp.View.User
{
    /// <summary>
    /// MyAccount.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DeactiveUser : Page
    {
        public DeactiveUser()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();

            try
            {
                

            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 EditAccount Loaded : {ex}");
                throw ex;
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private async void BtnDeactive_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = GrdData.SelectedItem as Model.User;

                if (GrdData.SelectedItem == null || GrdData.SelectedIndex >= Logic.DataAccess.GetUsers().Count())
                {
                    await Commons.ShowMessageAsync("오류", "비활성화할 사용자를 선택하세요.");
                    return;
                }
                
                user.UserActivated = false;
                Logic.DataAccess.SetUser(user);
                LoadData();
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 BtnDeactive_Click : {ex}");
                throw ex;
            }

        }

        private void LoadData()
        {
            this.DataContext = Logic.DataAccess.GetUsers();
        }
    }
}