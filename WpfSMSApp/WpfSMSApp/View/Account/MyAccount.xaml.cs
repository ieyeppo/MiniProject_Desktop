using System;
using System.Collections.Generic;
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

namespace WpfSMSApp.View.Account
{
    /// <summary>
    /// MyAccount.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MyAccount : Page
    {
        public MyAccount()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var userID = Commons.LOGINED_USER.UserID.ToString();
                var UserIdentityNumber = Commons.LOGINED_USER.UserIdentityNumber.ToString();
                var UserSurName = Commons.LOGINED_USER.UserSurname.ToString();
                var UserName = Commons.LOGINED_USER.UserName.ToString();
                var UserEmail = Commons.LOGINED_USER.UserEmail.ToString();
                var UserAdmin = Commons.LOGINED_USER.UserAdmin.ToString();
                var UserActivated = Commons.LOGINED_USER.UserActivated.ToString();

                TxtUserID.Text = userID;
                TxtUserIdentityNumber.Text = UserIdentityNumber;
                TxtUserSurName.Text = UserSurName;
                TxtUserName.Text = UserName;
                TxtUserEmail.Text = UserEmail;
                TxtUserAdmin.Text = UserAdmin;
                TxtUserActivated.Text = UserActivated;
            }
            catch(Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 MyAccount Page_Loaded : {ex}");
                throw ex;
            }
        }

        private void BtnEditMyAccount_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
