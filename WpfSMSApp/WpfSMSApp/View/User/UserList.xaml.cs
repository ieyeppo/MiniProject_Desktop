using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfSMSApp.View.User
{
    /// <summary>
    /// MyAccount.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserList : Page
    {
        public UserList()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                RdoAll.IsChecked = true;
                RdoAll_Checked(sender, e);


            }
            catch(Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 UserList Loaded : {ex}");
                throw ex;
            }
        }

        private void RdoAll_Checked(object sender, RoutedEventArgs e)
        {
            List<Model.User> users = new List<Model.User>();

            try
            {
                if (RdoAll.IsChecked == true)
                {
                    users = Logic.DataAccess.GetUsers();
                }

                this.DataContext = users;
            }
            catch(Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 : {ex}");
                throw ex;
            }
        }

        private void RdoActive_Checked(object sender, RoutedEventArgs e)
        {
            List<Model.User> users = new List<Model.User>();

            try
            {
                if (RdoActive.IsChecked == true)
                {
                    users = Logic.DataAccess.GetUsers().Where(u => u.UserActivated == true).ToList();
                }

                this.DataContext = users;
            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 : {ex}");
                throw ex;
            }
        }

        private void RdoDeactive_Checked(object sender, RoutedEventArgs e)
        {
            List<Model.User> users = new List<Model.User>();

            try
            {
                if (RdoDeactive.IsChecked == true)
                {
                    users = Logic.DataAccess.GetUsers().Where(u => u.UserActivated == false).ToList();
                }

                this.DataContext = users;
            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 : {ex}");
                throw ex;
            }
        }

        private void BtnAddUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new AddUser());
            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 BtnAddUser : {ex}");
                throw ex;
            }
        }

        private void BtnEditUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new EditUser());
            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 BtnEditUser : {ex}");
                throw ex;
            }
        }

        private void BtnDeactivateuser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new DeactiveUser());
            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 BtnDeactivateuser_Click : {ex}");
                throw ex;
            }
        }

        private void BtnExportPdf_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();

            saveDialog.Filter = "PDF File(*.pdf)|*.pdf";
            saveDialog.FileName = "";
            saveDialog.ShowDialog();

            if(saveDialog.ShowDialog() == true)
            {
                //PDF 변환
                try
                {
                    //0. PDF 사용 폰트 설정
                    string nanumPath = Path.Combine(Environment.CurrentDirectory, @"NanumGothic.ttf");
                    BaseFont nanumBase = BaseFont.CreateFont(nanumPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    var nanumTitle = new iTextSharp.text.Font(nanumBase, 20f);
                    var nanumContent = new iTextSharp.text.Font(nanumBase, 10f);

                    //iTextSharp.text.Font f = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12);
                    string pdfFilePath = saveDialog.FileName;

                    //1. PDF 생성
                    Document pdfDoc = new Document(PageSize.A4);

                    //2. PDF 내용 만들기
                    Paragraph title = new Paragraph("부경대 재고관리 시스템(SMS)\n", nanumTitle);
                    Paragraph subTitle = new Paragraph($"사용자리스트 Exported : {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\n\n\n", nanumContent);

                    PdfPTable pdfTable = new PdfPTable(GrdData.Columns.Count());
                    pdfTable.WidthPercentage = 100;

                    //그리드 헤더 작업
                    foreach(DataGridColumn column in GrdData.Columns)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column.Header.ToString(), nanumContent));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        pdfTable.AddCell(cell);
                    }

                    float[] columsWidth = new float[] { 10f, 20f, 20f, 20f, 50f, 15f, 15f };
                    pdfTable.SetWidths(columsWidth);

                    //그리드 로우 작업
                    foreach(var item in GrdData.Items)
                    {
                        if(item is Model.User)
                        {
                            var temp = item as Model.User;

                            PdfPCell cell = new PdfPCell(new Phrase(temp.UserID.ToString(), nanumContent));
                            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            pdfTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(temp.UserIdentityNumber.ToString(), nanumContent));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(temp.UserSurname.ToString(), nanumContent));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(temp.UserName.ToString(), nanumContent));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(temp.UserEmail.ToString(), nanumContent));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(temp.UserAdmin.ToString(), nanumContent));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(temp.UserActivated.ToString(), nanumContent));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfTable.AddCell(cell);
                        }
                    }

                    //PDF 파일 생성
                    using (FileStream stream = new FileStream(pdfFilePath, FileMode.OpenOrCreate))
                    {
                        PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();

                        //2번에서 만들 내용 추가
                        pdfDoc.Add(title);
                        pdfDoc.Add(subTitle);
                        pdfDoc.Add(pdfTable);

                        pdfDoc.Close();
                        stream.Close(); //option
                    }

                    Commons.ShowMessageAsync("PDF변환", "PDF 익스포트 성공했습니다.");
                }
                catch(Exception ex)
                {
                    Commons.LOGGER.Error($"예외발생 BtnExportPdf_Click : {ex}");
                    throw ex;
                }
            }
        }
    }
}
