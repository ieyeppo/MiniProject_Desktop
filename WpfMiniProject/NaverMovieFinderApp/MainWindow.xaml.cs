﻿using MahApps.Metro.Controls;
using System.Windows.Input;

namespace NaverMovieFinderApp
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

        private void MetroWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            StsResult.Content = "";
            TxtMovieName.Focus();
        }

        private void BtnSearch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            StsResult.Content = "";

            if (string.IsNullOrEmpty(TxtMovieName.Text))
            {
                StsResult.Content = "검색할 영화명을 입력 후, 검색버튼을 눌러주세요.";
                return;
            }

            Commons.ShowMessageAsync("결과", $"{TxtMovieName.Text}");
        }

        private void TxtMovieName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter) BtnSearch_Click(sender, e);
            else if (e.Key == Key.Escape) TxtMovieName.Text = "";
        }
    }
}
