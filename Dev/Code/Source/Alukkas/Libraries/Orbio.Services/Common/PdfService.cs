using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Orbio.Core.Domain.Common;

namespace Orbio.Services.Common
{
    public class PdfService : IPdfService
    {
        /// <summary>
        /// Print packaging slips to PDF
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="shipments">Shipments</param>
        /// <param name="languageId">Language identifier; 0 to use a language used when placing an order</param>
        public virtual void PrintPackagingSlipsToPdf(Stream stream, IList<Shipment> shipments)
        {
            var pageSize = PageSize.A4;

            var doc = new Document(pageSize);
            PdfWriter.GetInstance(doc, stream);
            doc.Open();

            //fonts
            var titleFont = new Font();
            titleFont.SetStyle(Font.BOLD);
            titleFont.Color = BaseColor.BLACK;
            var font = new Font();
            var attributesFont = new Font();
            attributesFont.SetStyle(Font.ITALIC);

            int shipmentCount = shipments.Count;
            int shipmentNum = 0;

            foreach (var shipment in shipments)
            {
                var order = shipment.Order;

                if (order.Address != null)
                {
                    doc.Add(new Paragraph(String.Format("Shipment #{0}", shipment.Id), titleFont));
                    doc.Add(new Paragraph(String.Format("Order #{0}", order.Id), titleFont));

                    doc.Add(new Paragraph(String.Format("Name {0}", order.Address.FirstName + " " + order.Address.LastName), font));

                    doc.Add(new Paragraph(String.Format("Phone {0}", order.Address.PhoneNumber), font));
                    doc.Add(new Paragraph(String.Format("Address {0}", order.Address.Address1), font));

                    if (!String.IsNullOrEmpty(order.Address.Address2))
                        doc.Add(new Paragraph(String.Format("Billing Address {0}", order.Address.Address2), font));
                        doc.Add(new Paragraph(String.Format("{0}, {1} {2}", order.Address.City, order.Address.StateProvince != null ? order.Address.StateProvince.Name : "", order.Address.ZipPostalCode), font));

                    if (order.Address.Country != null)
                    doc.Add(new Paragraph(String.Format("{0}", order.Address.Country != null ? order.Address.Country.Name : ""), font));

                    doc.Add(new Paragraph(" "));

                    doc.Add(new Paragraph(String.Format("Shipping Method {0}", order.ShippingMethod), font));
                    doc.Add(new Paragraph(" "));

                    var productsTable = new PdfPTable(3);
                    productsTable.WidthPercentage = 100f;
                    productsTable.SetWidths(new[] { 60, 20, 20 });

                    //product name
                    var cell = new PdfPCell(new Phrase("Product Name", font));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    productsTable.AddCell(cell);

                    //SKU
                    cell = new PdfPCell(new Phrase("SKU", font));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    productsTable.AddCell(cell);

                    //qty
                    cell = new PdfPCell(new Phrase("Qnty", font));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    productsTable.AddCell(cell);

                   foreach(var oi in shipment.Order.OrderItems)
                    {
                        //product name
                        if (oi == null)
                            continue;

                        var p = oi.Product;
                        string name = p.Name;
                        cell = new PdfPCell();
                        cell.AddElement(new Paragraph(name, font));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        var attributesParagraph = new Paragraph(oi.AttributeDescription, attributesFont);
                        cell.AddElement(attributesParagraph);
                        productsTable.AddCell(cell);

                        //SKU
                        var sku = p.Sku;
                        cell = new PdfPCell(new Phrase(sku ?? String.Empty, font));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        productsTable.AddCell(cell);

                        //qty
                        cell = new PdfPCell(new Phrase(oi.Quantity.ToString(), font));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        productsTable.AddCell(cell);
                    }
                    doc.Add(productsTable);
                }

                shipmentNum++;
                if (shipmentNum < shipmentCount)
                {
                    doc.NewPage();
                }
            }


            doc.Close();
        }
     
    }
}
