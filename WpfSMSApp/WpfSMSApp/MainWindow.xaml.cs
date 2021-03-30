using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Windows;
using WpfSMSApp.View;
using WpfSMSApp.View.Account;
using WpfSMSApp.View.User;

namespace WpfSMSApp
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }       

        private void MetroWindow_ContentRendered(object sender, EventArgs e)
        {
            ShowLoginView();
        }

        private void MetroWindow_Activated(object sender, EventArgs e)
        {
            if(Commons.LOGINED_USER != null)
            {
                BtnLoginedId.Content = $"{Commons.LOGINED_USER.UserEmail} ({Commons.LOGINED_USER.UserName})";
            }
        }

        private async void BtnLogOut_Click(object sender, RoutedEventArgs e)
        {
            //TODO : 모든 화면을 해제하고 첫화면으로 돌려놔야함.

            var result = await this.ShowMessageAsync("로그아웃","로그아웃하시겠습니까?", MessageDialogStyle.AffirmativeAndNegative, null);

            if(result == MessageDialogResult.Affirmative)
            {
                //이전 사용자 정보 삭제
                Commons.LOGINED_USER = null;
                ShowLoginView();
            }
        }

        private void ShowLoginView()
        {
            LoginView view = new LoginView();
            view.Owner = this;
            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            view.ShowDialog();
        }

        private async void BtnAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActiveControl.Content = new MyAccount();
            }
            catch(Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 BtnAccount_Click : {ex}");  //계정정보 수정
                await this.ShowMessageAsync("예외", $"예외발생 BtnAccount_Click : {ex}");
            }
        }

        private async void BtnUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActiveControl.Content = new UserList();
            }
            catch (Exception ex)
            {
                //사용자 추가,수정,비활성화,pdf익스포트
                Commons.LOGGER.Error($"예외발생 BtnUser_Click : {ex}");  
                await this.ShowMessageAsync("예외", $"예외발생 BtnUser_Click : {ex}");
            }
        }
    }
}
