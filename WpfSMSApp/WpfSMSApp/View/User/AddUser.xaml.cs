using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using MahApps.Metro.Controls.Dialogs;

namespace WpfSMSApp.View.User
{
    /// <summary>
    /// MyAccount.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AddUser : Page
    {
        public AddUser()
        {
            InitializeComponent();
        }




        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LblUserIdentityNumber.Visibility = LblUserSurname.Visibility =
                    LblUserName.Visibility = LblUserEmail.Visibility
                    = LblUserPassword.Visibility = LblUserAdmin.Visibility
                    = LblUserActivated.Visibility = Visibility.Hidden;


                //콤보박스 초기화
                List<string> comboValues = new List<string>
                {
                    "False", // index값 : 0
                    "True" // index값 : 1
                };
                CboUserAdmin.ItemsSource = comboValues;
                CboUserActivated.ItemsSource = comboValues;

                TxtUserID.Text = "";
            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 AddUser Loaded : {ex}");
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

            LblUserIdentityNumber.Visibility = LblUserSurname.Visibility =
                   LblUserName.Visibility = LblUserEmail.Visibility
                   = LblUserPassword.Visibility = LblUserAdmin.Visibility
                   = LblUserActivated.Visibility = Visibility.Hidden;

            var user = new Model.User();

            if (string.IsNullOrEmpty(TxtUserIdentityNumber.Text))
            {
                LblUserIdentityNumber.Visibility = Visibility.Visible;
                LblUserIdentityNumber.Text = "사번을 입력하세요";
                isValid = false;
            }
            else if (!IdentityNumberCheck(TxtUserIdentityNumber.Text))
            {
                LblUserIdentityNumber.Visibility = Visibility.Visible;
                LblUserIdentityNumber.Text = "중복된 사번 입니다..";
                isValid = false;
            }
            if (string.IsNullOrEmpty(TxtUserSurname.Text))
            {
                LblUserSurname.Visibility = Visibility.Visible;
                LblUserSurname.Text = "이름(성)을 입력하세요";
                isValid = false;
            }

            if (string.IsNullOrEmpty(TxtUserName.Text))
            {
                LblUserName.Visibility = Visibility.Visible;
                LblUserName.Text = "이름을 입력하세요";
                isValid = false;
            }
            if (string.IsNullOrEmpty(TxtUserEmail.Text))
            {
                LblUserEmail.Visibility = Visibility.Visible;
                LblUserEmail.Text = "이메일을 입력하세요";
                isValid = false;
            }
            else if (!Commons.IsValidEmail(TxtUserEmail.Text))
            {
                LblUserEmail.Visibility = Visibility.Visible;
                LblUserEmail.Text = "이메일 형식이 올바르지 않습니다.";
                isValid = false;
            }
            else if (!EmailCheck(TxtUserEmail.Text))
            {
                LblUserEmail.Visibility = Visibility.Visible;
                LblUserEmail.Text = "중복된 이메일 입니다..";
                isValid = false;
            }
            if (string.IsNullOrEmpty(TxtUserPassword.Password))
            {
                LblUserPassword.Visibility = Visibility.Visible;
                LblUserPassword.Text = "패스워드를 입력하세요";
                isValid = false;
            }
            if (CboUserAdmin.SelectedIndex < 0)
            {
                LblUserAdmin.Visibility = Visibility.Visible;
                LblUserAdmin.Text = "관리자여부를 선택하세요.";
                isValid = false;
            }
            if (CboUserActivated.SelectedIndex < 0)
            {
                LblUserActivated.Visibility = Visibility.Visible;
                LblUserActivated.Text = "활성여부를 선택하세요.";
                isValid = false;
            }
            

            if (isValid)
            {
                user.UserIdentityNumber = TxtUserIdentityNumber.Text;
                user.UserSurname = TxtUserSurname.Text;
                user.UserName = TxtUserName.Text;
                user.UserEmail = TxtUserEmail.Text;
                user.UserPassword = TxtUserPassword.Password;
                user.UserAdmin = bool.Parse(CboUserAdmin.SelectedValue.ToString());
                user.UserActivated = bool.Parse(CboUserActivated.SelectedValue.ToString());


                try
                {
                    var mdHash = MD5.Create();
                    user.UserPassword = Commons.GetMd5Hash(mdHash, user.UserPassword);

                    var result = Logic.DataAccess.SetUser(user);

                    if(result == 0)
                    {
                        LblResult.Text = "사용자 추가에 문제가 발생했습니다. 관리자에게 문의 바랍니다.";
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

        private bool EmailCheck(string email)
        {
            var cnt = Logic.DataAccess.GetUsers().Where(u => u.UserEmail.Equals(email)).Count();
            if (cnt > 0)
            {
                return false;
            }
            else return true;
        }

        private bool IdentityNumberCheck(string identityNumber)
        {
            var cnt = Logic.DataAccess.GetUsers().Where(u => u.UserIdentityNumber.Equals(identityNumber)).Count();
            if (cnt > 0)
            {
                return false;
            }
            else return true;
        }
    }
}