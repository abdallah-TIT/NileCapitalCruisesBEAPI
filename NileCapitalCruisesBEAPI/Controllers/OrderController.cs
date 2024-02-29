using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NileCapitalCruisesBEAPI.DTOs.BookingWedget;
using NileCapitalCruisesBEAPI.DTOs.Step_GetCabins;
using NileCapitalCruisesBEAPI.DTOs.Step_Order;
using NileCapitalCruisesBEAPI.Models;
using System.Security.Cryptography;

namespace BookingNile.API.Controllers.CMS
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly NileCapitalCruisesBeopdbContext _dbContextOp;
        private readonly NileCapitalCruisesBedbContext _dbContext;

        private readonly HttpClient _httpClient;

        public OrderController(NileCapitalCruisesBeopdbContext dbContextOp, NileCapitalCruisesBedbContext dbContext, IHttpClientFactory httpClientFactory)
        {
            _dbContextOp = dbContextOp;
            _dbContext = dbContext;
            _httpClient = httpClientFactory.CreateClient();
        }




        [HttpPost("CreateOrder")]

        public async Task<ActionResult> CreateOrderAsync(CLS_Order_Request request)
        {
            string format = "yyyy,MM,dd"; // Specify the format
            DateTime date = DateTime.ParseExact(request.str_OperationDate, format, null); // Parse the string to DateTime

            var operationDateID = _dbContext.TblOperationDates.Where(op => op.OperationDate == date).FirstOrDefault()?.OperationDateId;
            var PriceChild = request.PriceAdultRate / 2;
            var TotalPrice = (request.AdultsNo * request.PriceAdultRate) + (request.ChildNo * PriceChild);

            var newOreder = new TblOrder()
            {
                AdultsNo = request.AdultsNo,
                ChildNo = request.ChildNo,
                OperationDateId = operationDateID,
                CabinId = request.CabinId,
                CurrencyId = 1,
                ItineraryId = request.ItineraryId,
                CustomerFirstName = request.CustomerFirstName,
                CustomerLastName = request.CustomerLastName,
                CustomerEmail = request.CustomerEmail,
                CustomerPhone = request.CustomerPhone,
                CustomerNationalityId = request.CustomerNationalityId,
                OrderSpecialRequest = request.OrderSpecialRequest,
                PriceAdultRate = request.PriceAdultRate,
                PriceChildRate = request.PriceAdultRate / 2,
                OrderTotalPrice= TotalPrice
            };


            try
            {
                _dbContextOp.TblOrders.Add(newOreder);

                _dbContextOp.SaveChanges();
                
            }

            catch (Exception ex)
            {
                
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            int OrderID = newOreder.OrderId;
            Random rand = new Random();
            int RandomNum = rand.Next(3);
            string RandomNumber = RandomNum.ToString();
            string OrderNumber = "M-" + RandomNumber + OrderID;
            if (true)
            {

                string Ovar_hash = Createhash(OrderNumber, newOreder.OrderTotalPrice);
                string var_merchantId = "?merchantId=MID-18872-595";
                string var_orderId = "&orderId=" + OrderNumber;
                string var_amount = "&amount=" + (newOreder.OrderTotalPrice).ToString();
                string var_currency = "&currency=EGP";
                string var_hash = "&hash=" + Ovar_hash;
                string var_mode = "&mode=test";
                string var_merchantRedirect = "&merchantRedirect=https://apibe.nilecapitalcruises.com/" + "api/order/PaymentBack";
                string var_display = "&display=en";
                string var_allowedMethods = "&allowedMethods=card";
                string var_redirectMethod = "&redirectMethod=get";
                var Order = _dbContextOp.TblOrders.Where(x => x.OrderId == newOreder.OrderId).FirstOrDefault();
                Order.OrderConfirmationNumber = OrderNumber;
                _dbContextOp.SaveChanges();
                //Response.Redirect("https://checkout.kashier.io/" + var_merchantId + var_orderId + var_amount + var_currency + var_hash + var_mode + var_merchantRedirect + var_display + var_allowedMethods + var_redirectMethod);

                //return Redirect("https://checkout.kashier.io/" + var_merchantId + var_orderId + var_amount + var_currency + var_hash + var_mode + var_merchantRedirect + var_display + var_allowedMethods + var_redirectMethod);


                //return Redirect("https://checkout.kashier.io/\" + var_merchantId + var_orderId + var_amount + var_currency + var_hash + var_mode + var_merchantRedirect + var_display + var_allowedMethods + var_redirectMethod");



                return Ok("https://checkout.kashier.io/" + var_merchantId + var_orderId + var_amount + var_currency + var_hash + var_mode + var_merchantRedirect + var_display + var_allowedMethods + var_redirectMethod);

                // Use _httpClient to make HTTP requests
                //HttpResponseMessage response = await _httpClient.GetAsync("https://checkout.kashier.io/" + var_merchantId + var_orderId + var_amount + var_currency + var_hash + var_mode + var_merchantRedirect + var_display + var_allowedMethods + var_redirectMethod);

                //if (response.IsSuccessStatusCode)
                //{
                //    // Process successful response
                //    string responseBody = await response.Content.ReadAsStringAsync();
                //    return Ok(responseBody);
                //}
                //else
                //{
                //    // Handle error response
                //    return StatusCode((int)response.StatusCode);
                //}

                // Construct the URL to the external page
                //string externalUrl = "\"https://checkout.kashier.io/\" + var_merchantId + var_orderId + var_amount + var_currency + var_hash + var_mode + var_merchantRedirect + var_display + var_allowedMethods + var_redirectMethod";

                //string script = $@"
                //<script type='text/javascript'>
                //    window.location.href = '{externalUrl}';
                //</script>";

                //return Content(script, "text/html");
            }

            



        }


        [HttpGet("paymentBack")]
        public ActionResult PaymentBack(string paymentStatus, string cardDataToken, string maskedCard, string merchantOrderId, string orderId, string cardBrand, string orderReference, string transactionId, string currency, string mode, string signature)
        {

            var Order = _dbContextOp.TblOrders.Where(x => x.OrderConfirmationNumber == merchantOrderId).FirstOrDefault();

            //var Package = _dbContextOp.tbl_Dahabeyat_Packages.Where(x => x.PackageID == Order.PackageID).FirstOrDefault();
            var FirstName = Order.CustomerFirstName;

            var LastName = Order.CustomerLastName;

            var txt_CustomerName = FirstName + " " + LastName;


            var txt_CustomerEmail = Order.CustomerEmail;
            var txt_CustomerPhone = Order.CustomerPhone;
            if (paymentStatus == "SUCCESS")
            {


                #region Save result back from payment
                Order.OrderStatusId = 2;
                Order.PmpaymentStatus = paymentStatus;
                Order.PmcardDataToken = cardDataToken;
                Order.PmmaskedCard = maskedCard;
                Order.PmmerchantOrderId = merchantOrderId;
                Order.PmorderId = orderId;
                Order.PmcardBrand = cardBrand;
                Order.PmorderReference = orderReference;
                Order.PmtransactionId = transactionId;
                Order.Pmcurrency = currency;
                Order.Pmmode = mode;
                Order.Pmsignature = signature;
                _dbContextOp.SaveChanges();

                #endregion



               
            }



            var orderConfirmationNumber = Order.OrderConfirmationNumber;

            return Redirect("https://ws.nilecapitalcruises.com/en/cabinsbooking/1/thank-you");

        }



        public static string Createhash(string OrderNumber, double? TotalAmount)
        {
            string mid = "MID-18872-595";
            string amount = TotalAmount.ToString();
            string currency = "EGP";
            string orderId = OrderNumber;
            string secret = "856db08a-0429-4b82-8d92-9ba850e17cf1";
            string path = "/?payment=" + mid + "." + orderId + "." + amount + "." + currency;
            string message;
            string key;
            key = secret;
            message = path;
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(key);
            byte[] messageBytes = encoding.GetBytes(message);
            HMACSHA256 hmacmd256 = new HMACSHA256(keyByte);
            byte[] hashmessage = hmacmd256.ComputeHash(messageBytes);
            return ByteToString(hashmessage).ToLower();
        }

        public static string ByteToString(byte[] buff)
        {
            string sbinary = "";
            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }


        [HttpGet("OrderDetails")]
        public ActionResult OrderDetails(string? orderConfirmationNumber)
        {
            var order = _dbContextOp.TblOrders.Where(x => x.OrderConfirmationNumber == orderConfirmationNumber).FirstOrDefault();
            if (order == null)
            {
                return NotFound();
            }
            var operationDate = _dbContext.TblOperationDates.Where(x => x.OperationDateId == order.OperationDateId).FirstOrDefault()?.OperationDate;
            var cabinNameSys = _dbContext.TblCabins.Where(x => x.CabinId == order.CabinId).FirstOrDefault()?.CabinNameSys;
            var itineraryNameSys = _dbContext.TblItineraries.Where(x => x.ItineraryId == order.ItineraryId).FirstOrDefault()?.ItineraryNameSys;
            var Obj_GetOrder = new CLS_Order()
            {
                OrderConfirmationNumber = order.OrderConfirmationNumber,
                OrderTotalPrice = order.OrderTotalPrice,
                AdultsNo = order.AdultsNo,
                ChildNo = order.ChildNo,
                str_OperationDate = $"{operationDate.Value.Year:D4},{operationDate.Value.Month:D2},{operationDate.Value.Day:D2}",
                CabinNameSys = cabinNameSys,
                ItineraryNameSys = itineraryNameSys,
                CustomerFirstName = order.CustomerFirstName,
                CustomerLastName = order.CustomerLastName,
                CustomerEmail = order.CustomerEmail,
                CustomerPhone = order.CustomerPhone,
            };
            return Ok(Obj_GetOrder);
        }

    }
}
