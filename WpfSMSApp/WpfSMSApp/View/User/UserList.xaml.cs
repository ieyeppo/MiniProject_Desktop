﻿using iTextSharp.text;
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
                    iTextSharp.text.Font f = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12);
                    string pdfFilePath = saveDialog.FileName;

                    Document pdfDoc = new Document(PageSize.A4);

                    //1. PDF 생성
                    PdfPTable pdfTable = new PdfPTable(GrdData.Columns.Count());

                    //2. PDF 내용 만들기
                    string nanumttf = Path.Combine(Environment.SystemDirectory, @"나눔고딕코딩.ttf");
                    BaseFont nanumBase = BaseFont.CreateFont(nanumttf, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    var nanumFont = new iTextSharp.text.Font(nanumBase, 16f);

                    Paragraph title = new Paragraph($"혀나 Stoke Management System : {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}", nanumFont);
                    
                    //PDF 파일 생성
                    using(FileStream stream = new FileStream(pdfFilePath, FileMode.OpenOrCreate))
                    {
                        PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();

                        //2번에서 만들 내용 추가
                        pdfDoc.Add(title);
                        //pdfDoc.Add();

                        pdfDoc.Close();
                        stream.Close(); //option
                    }
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
