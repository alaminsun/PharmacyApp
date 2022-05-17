using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using PhramacyApp.Areas.Transaction.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Areas.Dashboard.Models
{
    public class InvoiceReport
    {
        private readonly IWebHostEnvironment _oHostEnvironment;
        public InvoiceReport(IWebHostEnvironment oHostEnvironment)
        {
            _oHostEnvironment = oHostEnvironment;
        }

        #region Declaration
        int _maxColumn = 3;
        Document _document;
        Font _fontStyle;
        PdfPTable _pdfPTable = new PdfPTable(3);
        PdfPCell _pdfCell;
        MemoryStream _memoryStream = new MemoryStream();
        List<PurchaseDetailModel> _ostudents = new List<PurchaseDetailModel>();
        #endregion

        public byte[] Report(List<PurchaseDetailModel> items)
        {
            _ostudents = items;
            _document = new Document();
            _document.SetPageSize(PageSize.A4);
            _document.SetMargins(5f, 5f, 20f, 5f);

            _pdfPTable.WidthPercentage = 100;
            _pdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            PdfWriter docWrite = PdfWriter.GetInstance(_document, _memoryStream);

            _document.Open();

            float[] sizes = new float[_maxColumn];
            for (int i = 0; i < _maxColumn; i++)
            {
                if (i == 0)
                {
                    sizes[i] = 20;
                }
                else
                {
                    sizes[i] = 100;
                }
            }
            _pdfPTable.SetWidths(sizes);

            this.ReportHeader();
            this.EmptyRow(2);
            this.ReportBody();
            _pdfPTable.HeaderRows = 2;
            _document.Add(_pdfPTable);
            _document.Close();

            return _memoryStream.ToArray();
        }

        private void ReportHeader()
        {
            _pdfCell = new PdfPCell(this.AddLogo());
            _pdfCell.Colspan = 1;
            _pdfCell.Border = 0;
            _pdfPTable.AddCell(_pdfCell);
        }

        private PdfPTable AddLogo()
        {
            int maxColumn = 1;
            PdfPTable pdfPTable = new PdfPTable(maxColumn);
            string path = _oHostEnvironment.WebRootPath + "/Images";

            string imgCombine = Path.Combine(path, "BMW.png");
            Image img = Image.GetInstance(imgCombine);

            _pdfCell = new PdfPCell(img);
            _pdfCell.Colspan = maxColumn;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            pdfPTable.AddCell(_pdfCell);

            pdfPTable.CompleteRow();

            return pdfPTable;
        }

        private PdfPTable SetPageTitle()
        {
            int maxColumn = 3;
            PdfPTable pdfPTable = new PdfPTable(maxColumn);

            _fontStyle = FontFactory.GetFont("Tahoma", 18f, 1);
            _pdfCell = new PdfPCell(new Phrase("Scholl Name ", _fontStyle));
            _pdfCell.Colspan = maxColumn;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            pdfPTable.AddCell(_pdfCell);
            pdfPTable.CompleteRow();

            return pdfPTable;
        }

        private void EmptyRow(int nCount)
        {
            for (int i = 0; i <= nCount; i++)
            {
                _fontStyle = FontFactory.GetFont("Tahoma", 18f, 1);
                _pdfCell = new PdfPCell(new Phrase("Scholl Name ", _fontStyle));
                _pdfCell.Colspan = _maxColumn;
                _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _pdfCell.Border = 0;
                _pdfCell.ExtraParagraphSpace = 10;
                _pdfPTable.AddCell(_pdfCell);
                _pdfPTable.CompleteRow();
            }
        }

        private void ReportBody()
        {
            var fontStyleBold = FontFactory.GetFont("Tahoma", 9f, 1);
            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);

            #region Detail Table Header 
            _pdfCell = new PdfPCell(new Phrase("Medicine Id", fontStyleBold));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.Gray;
            _pdfPTable.AddCell(_pdfCell);


            _pdfCell = new PdfPCell(new Phrase("Medicine Name", fontStyleBold));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.Gray;
            _pdfPTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("Batch No", fontStyleBold));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.Gray;
            _pdfPTable.AddCell(_pdfCell);

            _pdfPTable.CompleteRow();
            #endregion

            #region Detail table body

            foreach (var oStudent in _ostudents)
            {
                _pdfCell = new PdfPCell(new Phrase(oStudent.Medicine_Id.ToString(), _fontStyle));
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = BaseColor.White;
                _pdfPTable.AddCell(_pdfCell);


                _pdfCell = new PdfPCell(new Phrase(oStudent.Medicine_Name, fontStyleBold));
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = BaseColor.White;
                _pdfPTable.AddCell(_pdfCell);

                _pdfCell = new PdfPCell(new Phrase(oStudent.Batch_No, fontStyleBold));
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = BaseColor.White;
                _pdfPTable.AddCell(_pdfCell);

                _pdfPTable.CompleteRow();
            }

            #endregion

        }
    }
}
