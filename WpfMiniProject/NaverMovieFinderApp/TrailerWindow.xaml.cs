using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using MahApps.Metro.Controls;
using NaverMovieFinderApp.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace NaverMovieFinderApp
{
    /// <summary>
    /// TrailerWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TrailerWindow : MetroWindow
    {
        //유튜브 API 검색결과
        List<YoutubeItem> youtubes;

        public TrailerWindow()
        {
            InitializeComponent();
        }

        public TrailerWindow(string movieName) :this()
        {
            youtubes = new List<YoutubeItem>(); //초기화
            LblMovieName.Content = $"{movieName} 예고편";
            ProcSearchYoutubeApi(movieName);
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //유튜브 API로 검색



        }

        private async void ProcSearchYoutubeApi(string movieName)
        {
            await LoadDataCollection();
            LsvYoutubeSearch.ItemsSource = youtubes;
        }

        private async Task LoadDataCollection()
        {
            var youtubeService = new YouTubeService(
                new BaseClientService.Initializer()
                {
                    ApiKey = "AIzaSyBmqxwW9MdSyvekv-fYYQ_5uq0bGWrvu90",//
                    ApplicationName = this.GetType().ToString()
                });

            var request = youtubeService.Search.List("snippet");
            request.Q = LblMovieName.Content.ToString();    //{영화이름} 예고편
            request.MaxResults = 10;                        //사이즈가 크면 멈춰버림

            var response = await request.ExecuteAsync();    //검색 시작
            foreach (var item in response.Items)
            {
                if (item.Id.Kind.Equals("youtube#video"))
                {
                    YoutubeItem youtube = new YoutubeItem()
                    {
                        Title = item.Snippet.Title,
                        Author = item.Snippet.ChannelTitle,
                        URL = $"https://www.youtube.com/watch?v={item.Id.VideoId}"
                    };
                    //썸네일 이미지
                    youtube.Thumbnail = new BitmapImage(new Uri(item.Snippet.Thumbnails.Default__.Url, UriKind.RelativeOrAbsolute));

                    youtubes.Add(youtube);
                }
            }
        }

        private void LsvYoutubeSearch_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(LsvYoutubeSearch.SelectedItems.Count == 0)
            {
                Commons.ShowMessageAsync("유튜브영화", "영화를 선택하세요.");
                return;
            }
            if (LsvYoutubeSearch.SelectedItems.Count > 1)
            {
                Commons.ShowMessageAsync("유튜브영화", "영화를 하나만 선택하세요.");
                return;
            }

            if(LsvYoutubeSearch.SelectedItem is YoutubeItem)
            {
                var video = LsvYoutubeSearch.SelectedItem as YoutubeItem;
                BrwYoutubeWatch.Source = new Uri(video.URL, UriKind.RelativeOrAbsolute);
            }
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            BrwYoutubeWatch.Source = null;  //해제
            BrwYoutubeWatch.Dispose();      //즉시해제
        }
    }
}
