using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfSMSApp.View.Store
{
    public partial class StoreList : Page
    {
        public StoreList()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Model.Store> stores = new List<Model.Store>();
                List<Model.StockStore> stockStores = new List<Model.StockStore>();
                List<Model.Stock> stocks = new List<Model.Stock>();
                stores = Logic.DataAccess.GetStores();
                stocks = Logic.DataAccess.GetStocks();

                // stores 데이터를 stockStores로 옮김
                foreach(Model.Store item  in stores)
                {
                    stockStores.Add(new Model.StockStore() {
                        StoreID = item.StoreID,
                        StoreName = item.StoreName,
                        StoreLocation = item.StoreLocation,
                        ItemStatus = item.ItemStatus,
                        TagID = item.TagID,
                        BarcodeID = item.BarcodeID,
                        StockQuantity = 0
                    });
                }

                this.DataContext = stockStores;
            }
            catch(Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 UserList Loaded : {ex}");
                throw ex;
            }
        }


        private void BtnAddStore_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new AddStore());
            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 BtnAddStore_Click : {ex}");
                throw ex;
            }
        }

        private void BtnEditStore_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new EditStore());
            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 BtnEditStore_Click : {ex}");
                throw ex;
            }
        }

        private void BtnExcelPdf_Click(object sender, RoutedEventArgs e)
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

                    Commons.ShowMessageAsync("Excel 변환", "Excel 익스포트 성공했습니다.");
                }
                catch(Exception ex)
                {
                    Commons.LOGGER.Error($"예외발생 BtnExcelPdf_Click : {ex}");
                    throw ex;
                }
            }
        }
    }
}
