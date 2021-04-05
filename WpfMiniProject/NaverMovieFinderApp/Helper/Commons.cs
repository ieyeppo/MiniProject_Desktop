using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NLog;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace NaverMovieFinderApp
{
    public class Commons
    {
        //즐겨찾기 여부 플래그
        public static bool IsFavorite = false;

        //즐겨찾기 삭제 후 보기
        public static bool IsDelete = false;

        // NLog 정적 인스턴스 생성
        public static readonly Logger LOGGER = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// MetroMessageBox 공통메서드
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public static async Task<MessageDialogResult> ShowMessageAsync(string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative)
        {
            return await ((MetroWindow)Application.Current.MainWindow).ShowMessageAsync(title, message, style, null);
        }

        public static string GetOpenApiResult(string openApiUrl, string clientID, string clientSecret)
        {
            var result = "";

            try
            {
                WebRequest request = WebRequest.Create(openApiUrl);
                request.Headers.Add("X-Naver-Client-Id", clientID);
                request.Headers.Add("X-Naver-Client-Secret", clientSecret);

                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);

                result = reader.ReadToEnd();

                reader.Close();
                stream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"예외발생 : {ex}");
            }

            return result;
        }

        /// <summary>
        /// HTML 태그 삭제 메서드
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string StripHtmlTag(string text)
        {
            //HTML Tag 삭제하는 정규 표현식
            return Regex.Replace(text, @"<(.|\n)*?>", "");
        }

        public static string StripPipe(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            return text.Substring(0, text.LastIndexOf("|")).Replace("|", ", ");
        }
    }
}
