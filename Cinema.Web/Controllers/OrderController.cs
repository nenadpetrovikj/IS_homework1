using Cinema.Domain.DomainModels;
using Cinema.Services.Interface;
using ClosedXML.Excel;
using GemBox.Document;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Web.Controllers
{

    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        //private readonly UserManager<EShopApplicationUser> userManager;

        public OrderController(IOrderService orderService/*, UserManager<EShopApplicationUser> userManager*/)
        {
            this._orderService = orderService;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            //this.userManager = userManager;
        }
        public IActionResult Index()
        {
            List<Order> listOrder = this._orderService.getAllOrders();
            return View(listOrder);
        }

        public IActionResult GetDetailsForTicket(BaseEntity model)
        {
            Order order = this._orderService.getOrderDetails(model);
            return View(order);
        }


        public FileContentResult ExportAllOrders()
        {
            string fileName = "Orders.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("All Orders");

                worksheet.Cell(1, 1).Value = "Order Id";
                worksheet.Cell(1, 2).Value = "Costumer Email";


                var result = this._orderService.getAllOrders();

                for (int i = 1; i <= result.Count(); i++)
                {
                    var item = result[i - 1];

                    worksheet.Cell(i + 1, 1).Value = item.Id.ToString();
                    worksheet.Cell(i + 1, 2).Value = item.User.Email;

                    for (int p = 0; p < item.Tickets.Count(); p++)
                    {
                        worksheet.Cell(1, p + 3).Value = "Ticket-" + (p + 1);
                        worksheet.Cell(i + 1, p + 3).Value = item.Tickets.ElementAt(p).SelectedTicket.Title;
                    }
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }

            }
        }

        public FileContentResult CreateInvoice(BaseEntity model)
        {
            var result = this._orderService.getOrderDetails(model);

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
            var document = DocumentModel.Load(templatePath);

            document.Content.Replace("{{OrderNumber}}", result.Id.ToString());
            document.Content.Replace("{{UserName}}", result.User.UserName);

            StringBuilder sb = new StringBuilder();

            var totalPrice = 0.0;

            foreach (var item in result.Tickets)
            {
                totalPrice += item.Quantity * item.SelectedTicket.TicketPrice;
                sb.AppendLine(item.SelectedTicket.Title + " with quantity of: " + item.Quantity + " and price of: " + item.SelectedTicket.TicketPrice + "MKD");
            }


            document.Content.Replace("{{TicketList}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", totalPrice.ToString() + "MKD");


            var stream = new MemoryStream();

            document.Save(stream, new PdfSaveOptions());

            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");


      
        }

    }
}
