﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NileCapitalCruisesBEAPI;
using NileCapitalCruisesBEAPI.DTOs.BookingWedget;
using NileCapitalCruisesBEAPI.DTOs.Step_GetCabins;
using NileCapitalCruisesBEAPI.DTOs.Step_Order;
using NileCapitalCruisesBEAPI.Models;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Azure.Core;
using Azure;
using Microsoft.AspNetCore.Authorization;
//using Newtonsoft.Json;


namespace BookingNile.API.Controllers.CMS
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly NileCapitalCruisesBeopdbContext _dbContextOp;
        private readonly NileCapitalCruisesBedbContext _dbContext;
        private readonly IEmailSender _emailSender;

        private readonly HttpClient _httpClient;

        public OrderController(NileCapitalCruisesBeopdbContext dbContextOp, NileCapitalCruisesBedbContext dbContext, IHttpClientFactory httpClientFactory, IEmailSender emailSender)
        {
            _dbContextOp = dbContextOp;
            _dbContext = dbContext;
            _httpClient = httpClientFactory.CreateClient();
            _emailSender = emailSender;
        }


        [HttpPost("CreateOrdernew")]

        public async Task<ActionResult> CreateOrdernewAsync(CLS_Order_Request request)
        {
            string format = "yyyy,MM,dd"; // Specify the format
            DateTime date = DateTime.ParseExact(request.str_OperationDate, format, null); // Parse the string to DateTime

            var cabin = _dbContext.TblCabins.Where(c => c.CabinId == request.CabinId).FirstOrDefault();
            var cruiseItinerary = _dbContext.VwCruisesItineraries.Where(c => c.ItineraryId == request.ItineraryId).FirstOrDefault();
            var nationality = _dbContextOp.TblCountriesNationalities.Where(n => n.NationalityId == request.CustomerNationalityId).FirstOrDefault();
            var operationDateID = _dbContext.TblOperationDates.Where(op => op.OperationDate == date).FirstOrDefault()?.OperationDateId;

            var vwRatesPrices = _dbContext.VwRatesPrices
                                .Where(vw => vw.ItineraryId == request.ItineraryId
                                    && vw.CabinMaxAdults >= request.AdultsNo
                                    && vw.CabinMaxChild >= request.ChildNo
                                    && vw.CabinMaxOccupancy >= request.AdultsNo + request.ChildNo
                                    && vw.DateStart <= date
                                    && vw.DateEnd >= date
                                    && vw.OperationDateId == operationDateID).ToList();

            var getRatePriceStructures = GetRatePriceStructures();

            double? priceInfant = (request.ChildAge1 >= 0 && request.ChildAge1 <= 1.99) || (request.ChildAge2 >= 0 && request.ChildAge2 <= 1.99) ? CalculatePrice(getRatePriceStructures.Infant, request.PriceAdultBasic) : 0;
            double? priceChild6 = (request.ChildAge1 >= 2 && request.ChildAge1 <= 5.99) || (request.ChildAge2 >= 2 && request.ChildAge2 <= 5.99) ? CalculatePrice(getRatePriceStructures.Child6, request.PriceAdultBasic) : 0;
            double? priceChild12 = (request.ChildAge1 >= 6 && request.ChildAge1 <= 11.99) || (request.ChildAge2 >= 6 && request.ChildAge2 <= 11.99) ? CalculatePrice(getRatePriceStructures.Child12, request.PriceAdultBasic) : 0;

            double? priceChildren = (request.ChildNo > 0 ? (request.ChildAge1 == request.ChildAge2 ?
                                          request.ChildNo * priceInfant + request.ChildNo * priceChild6 + request.ChildNo * priceChild12 :
                                          priceInfant + priceChild6 + priceChild12) : 0);

            double? priceAdults = (request.AdultsNo == 1 ? request.PriceAdultBasic : request.AdultsNo * request.PriceAdultBasic);
            double? netPriceNightTotal = priceAdults + priceChildren;


            double? netPriceTotal = netPriceNightTotal * vwRatesPrices.FirstOrDefault()?.DurationNights;

            var newOreder = new TblOrder()
            {
                AdultsNo = request.AdultsNo,
                ChildNo = request.ChildNo,
                OperationDateId = operationDateID,
                CabinId = request.CabinId,
                CurrencyId = 1,
                OrderStatusId = 1,
                ItineraryId = request.ItineraryId,
                CustomerFirstName = request.CustomerFirstName,
                CustomerLastName = request.CustomerLastName,
                CustomerEmail = request.CustomerEmail,
                CustomerPhone = request.CustomerPhone,
                CustomerNationalityId = request.CustomerNationalityId,
                OrderSpecialRequest = request.OrderSpecialRequest,
                PriceAdultRate = 0,
                PriceChildRate = 0,
                OrderTotalPrice = netPriceTotal
            };


            try
            {
                _dbContextOp.TblOrders.Add(newOreder);

                _dbContextOp.SaveChanges();
            }

            catch (Exception ex)
            {

                Console.WriteLine($"An error occurred: {ex.Message}");
                return BadRequest(ex.Message);
            }

            int OrderID = newOreder.OrderId;
            Random rand = new Random();
            int RandomNum = rand.Next(3);
            string RandomNumber = RandomNum.ToString();
            string OrderNumber = "M-" + RandomNumber + OrderID;
            if (true)
            {

                string Ovar_hash = Createhash(OrderNumber, newOreder.OrderTotalPrice);
                string var_merchantId = "?merchantId=MID-23933-421";
                //string var_merchantId = "?merchantId=MID-18872-595";
                string var_orderId = "&orderId=" + OrderNumber;
                string var_amount = "&amount=" + (newOreder.OrderTotalPrice).ToString();
                string var_currency = "&currency=EGP";
                string var_hash = "&hash=" + Ovar_hash;
                //string var_mode = "&mode=test";
                string var_mode = "&mode=live";
                string var_merchantRedirect = "&merchantRedirect=https://apibe.nilecapitalcruises.com/" + "api/order/PaymentBack";
                string var_display = "&display=en";
                string var_allowedMethods = "&allowedMethods=card";
                string var_redirectMethod = "&redirectMethod=get";
                var Order = _dbContextOp.TblOrders.Where(x => x.OrderId == newOreder.OrderId).FirstOrDefault();
                Order.OrderConfirmationNumber = OrderNumber;
                _dbContextOp.SaveChanges();

                return Ok(new
                {
                    OrderID = newOreder.OrderId,
                    linkToCashier = "https://checkout.kashier.io/" + var_merchantId + var_orderId + var_amount + var_currency + var_hash + var_mode + var_merchantRedirect + var_display + var_allowedMethods + var_redirectMethod
                });
                //return Ok("https://checkout.kashier.io/" + var_merchantId + var_orderId + var_amount + var_currency + var_hash + var_mode + var_merchantRedirect + var_display + var_allowedMethods + var_redirectMethod);

            }



        }


        [HttpPost("CreateOrder")]

        public async Task<ActionResult> CreateOrderAsync(CLS_Order_Request request)
        {
            string format = "yyyy,MM,dd"; // Specify the format
            DateTime date = DateTime.ParseExact(request.str_OperationDate, format, null); // Parse the string to DateTime

            var cabin = _dbContext.TblCabins.Where(c => c.CabinId == request.CabinId).FirstOrDefault();
            var cruiseItinerary = _dbContext.VwCruisesItineraries.Where(c => c.ItineraryId == request.ItineraryId).FirstOrDefault();
            var nationality = _dbContextOp.TblCountriesNationalities.Where(n => n.NationalityId == request.CustomerNationalityId).FirstOrDefault();
            var operationDateID = _dbContext.TblOperationDates.Where(op => op.OperationDate == date).FirstOrDefault()?.OperationDateId;

            var vwRatesPrices = _dbContext.VwRatesPrices
                                .Where(vw => vw.ItineraryId == request.ItineraryId
                                    && vw.CabinMaxAdults >= request.AdultsNo
                                    && vw.CabinMaxChild >= request.ChildNo
                                    && vw.CabinMaxOccupancy >= request.AdultsNo + request.ChildNo
                                    && vw.DateStart <= date
                                    && vw.DateEnd >= date
                                    && vw.OperationDateId == operationDateID).ToList();

            var getRatePriceStructures = GetRatePriceStructures();

            double? priceInfant = (request.ChildAge1 >= 0 && request.ChildAge1 <= 1.99) || (request.ChildAge2 >= 0 && request.ChildAge2 <= 1.99) ? CalculatePrice(getRatePriceStructures.Infant, request.PriceAdultBasic) : 0;
            double? priceChild6 = (request.ChildAge1 >= 2 && request.ChildAge1 <= 5.99) || (request.ChildAge2 >= 2 && request.ChildAge2 <= 5.99) ? CalculatePrice(getRatePriceStructures.Child6, request.PriceAdultBasic) : 0;
            double? priceChild12 = (request.ChildAge1 >= 6 && request.ChildAge1 <= 11.99) || (request.ChildAge2 >= 6 && request.ChildAge2 <= 11.99) ? CalculatePrice(getRatePriceStructures.Child12, request.PriceAdultBasic) : 0;

            double? priceChildren = (request.ChildNo > 0 ? (request.ChildAge1 == request.ChildAge2 ?
                                          request.ChildNo * priceInfant + request.ChildNo * priceChild6 + request.ChildNo * priceChild12 :
                                          priceInfant + priceChild6 + priceChild12) : 0);

            double? priceAdults = (request.AdultsNo == 1 ? request.PriceAdultBasic : request.AdultsNo * request.PriceAdultBasic);
            double? netPriceNightTotal = priceAdults + priceChildren;


            double? netPriceTotal = netPriceNightTotal * vwRatesPrices.FirstOrDefault()?.DurationNights;

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
                PriceAdultRate = 0,
                PriceChildRate = 0,
                OrderTotalPrice = netPriceTotal
            };


            try
            {
                _dbContextOp.TblOrders.Add(newOreder);

                _dbContextOp.SaveChanges();

                string bodyForCompany =
            @"<table  style='border:1px solid #ccc;text-align: left;border-collapse: collapse;width: 100%;'>
                           <tbody>";

                bodyForCompany +=
                @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                           <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>First Name: </td>
                           <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + newOreder.CustomerFirstName + @" </td>
                           </tr>";

                bodyForCompany +=
                @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                           <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Last Name: </td>
                           <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + newOreder.CustomerLastName + @" </td>
                           </tr>";

                bodyForCompany +=
               @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                           <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Email: </td>
                           <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + newOreder.CustomerEmail + @" </td>
                           </tr>";

                bodyForCompany +=
                @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                           <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Mobile Number: </td>
                           <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + newOreder.CustomerPhone + @" </td>
                           </tr>";

                bodyForCompany +=
                @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                           <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Nationality: </td>
                           <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + nationality?.NationalityName + @" </td>
                           </tr>";

                bodyForCompany +=
                @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                           <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Cruise Name: </td>
                           <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + cruiseItinerary?.CruiseNameSys + @" </td>
                           </tr>";

                bodyForCompany +=
                @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                           <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Cabin Name: </td>
                           <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + cabin?.CabinNameSys + @" </td>
                           </tr>";

                bodyForCompany +=
            @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                           <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Itinerary Name: </td>
                           <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + cruiseItinerary?.ItineraryNameSys + @" </td>
                           </tr>";

                bodyForCompany +=
              @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                           <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Operation Date: </td>
                           <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + date.ToString("dddd dd, MMMM , yyyy") + @" </td>
                           </tr>";


                bodyForCompany +=
                @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                           <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Adults No: </td>
                           <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + newOreder.AdultsNo + @" </td>
                           </tr>";


                bodyForCompany +=
                @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                           <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Child No: </td>
                           <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + newOreder.ChildNo + @" </td>
                           </tr>";




                bodyForCompany +=
               @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                           <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Total Price: </td>
                           <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + newOreder.OrderTotalPrice + "$" + @"  </td>
                           </tr>";


                bodyForCompany +=
                @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                           <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Special Request: </td>
                           <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + newOreder.OrderSpecialRequest + @" </td>
                           </tr>";
                bodyForCompany += @"</tbody></table> ";

                SendMail("do-not-reply@nilecapitalcruises.com", "info@nilecapitalcruises.com", "Nile Capital Cruises Booking Request", bodyForCompany);
                SendMail("do-not-reply@nilecapitalcruises.com", "ahmed.taha@titegypt.com", "Nile Capital Cruises Booking Request", bodyForCompany);

                if (cruiseItinerary?.CruiseId == 1)
                {
                    string bodyForCustomer = @"<p style='font-size: 14px;'>Thanks! Your booking at Nile Capital Cruise is confirmed. We're thrilled to host you and are preparing to make your visit memorable.</p> <p style='font-weight: bold; font-size: 14px;'>Your Booking:</p>";
                    bodyForCustomer += bodyForCompany;
                    bodyForCustomer += @"<br><span style='font-weight: bold; font-size: 14px;'>Nile Capital Cruises sales team:</span><br>";
                    bodyForCustomer += @"<span style='font-size: 14px;'> <b>Email:</b> info@nilecapitalcruises.com</span><br>";
                    bodyForCustomer += @"<span style='font-size: 14px;'> <b>Mobil:</b> +20 111 111 8733</span><br>";
                    bodyForCustomer += @"<span style='font-size: 14px;'> <b>Address:</b> 12 Ismail Zohdi, Al Hadiqah Ad Dawleyah, Nasr City, Cairo Governorate</span><br>";

                    SendMail("do-not-reply@nilecapitalcruises.com", request.CustomerEmail, "Booking Request", bodyForCustomer);
                    SendMail("do-not-reply@nilecapitalcruises.com", "ahmed.taha@titegypt.com", "Booking Request", bodyForCustomer);
                }
                else
                {
                    string bodyForCustomer = @"<p style='font-size: 14px;'>Thanks! Your booking at Agatha Christie Dahabiya is confirmed. We can't wait to provide you with an exceptional experience.</p> <p style='font-weight: bold; font-size: 14px;'>Your Booking:</p>";
                    bodyForCustomer += bodyForCompany;
                    bodyForCustomer += @"<br><span style='font-weight: bold; font-size: 14px;'>Nile Capital Cruises sales team:</span><br>";
                    bodyForCustomer += @"<span style='font-size: 14px;'> <b>Email:</b> info@nilecapitalcruises.com</span><br>";
                    bodyForCustomer += @"<span style='font-size: 14px;'> <b>Mobil:</b> +20 111 111 8733</span><br>";
                    bodyForCustomer += @"<span style='font-size: 14px;'> <b>Address:</b> 12 Ismail Zohdi, Al Hadiqah Ad Dawleyah, Nasr City, Cairo Governorate</span><br>";

                    SendMail("do-not-reply@nilecapitalcruises.com", request.CustomerEmail, "Booking Request", bodyForCustomer);
                    SendMail("do-not-reply@nilecapitalcruises.com", "ahmed.taha@titegypt.com", "Booking Request", bodyForCustomer);
                }

            }

            catch (Exception ex)
            {

                Console.WriteLine($"An error occurred: {ex.Message}");
                return BadRequest(ex.Message);
            }
            //SendMail("do-not-reply@nilecapitalcruises.com", "abdallah.abdelnasser@titegypt.com", "Nile Capital Cruise Booking Mail", "hi");
            //await _emailSender.SendEmailAsync("dd");

            var orderResponse = new CLS_Order_Response()
            {
                CustomerFirstName = newOreder.CustomerFirstName,
                CustomerLastName = newOreder.CustomerLastName,
                CustomerEmail = newOreder.CustomerEmail,
                CustomerPhone = newOreder.CustomerPhone,
                NationalityName = nationality?.NationalityName,
                CruiseNameSys = cruiseItinerary?.CruiseNameSys,
                CabinNameSys = cabin?.CabinNameSys,
                ItineraryNameSys = cruiseItinerary?.ItineraryNameSys,
                str_OperationDate = date.ToString("dddd dd, MMMM , yyyy"),
                AdultsNo = newOreder.AdultsNo,
                ChildNo = newOreder.ChildNo,
                OrderTotalPrice = newOreder.OrderTotalPrice,
                OrderSpecialRequest = newOreder.OrderSpecialRequest

            };


            return Ok(orderResponse);

        }



        //[HttpPost("CreateOrder")]

        //public async Task<ActionResult> CreateOrderAsync(CLS_Order_Request request)
        //{
        //    string format = "yyyy,MM,dd"; // Specify the format
        //    DateTime date = DateTime.ParseExact(request.str_OperationDate, format, null); // Parse the string to DateTime

        //    var operationDateID = _dbContext.TblOperationDates.Where(op => op.OperationDate == date).FirstOrDefault()?.OperationDateId;
        //    var PriceChild = request.PriceAdultRate / 2;
        //    var TotalPrice = (request.AdultsNo * request.PriceAdultRate) + (request.ChildNo * PriceChild);

        //    var newOreder = new TblOrder()
        //    {
        //        AdultsNo = request.AdultsNo,
        //        ChildNo = request.ChildNo,
        //        OperationDateId = operationDateID,
        //        CabinId = request.CabinId,
        //        CurrencyId = 1,
        //        ItineraryId = request.ItineraryId,
        //        CustomerFirstName = request.CustomerFirstName,
        //        CustomerLastName = request.CustomerLastName,
        //        CustomerEmail = request.CustomerEmail,
        //        CustomerPhone = request.CustomerPhone,
        //        CustomerNationalityId = request.CustomerNationalityId,
        //        OrderSpecialRequest = request.OrderSpecialRequest,
        //        PriceAdultRate = request.PriceAdultRate,
        //        PriceChildRate = request.PriceAdultRate / 2,
        //        OrderTotalPrice= TotalPrice
        //    };


        //    try
        //    {
        //        _dbContextOp.TblOrders.Add(newOreder);

        //        _dbContextOp.SaveChanges();

        //    }

        //    catch (Exception ex)
        //    {

        //        Console.WriteLine($"An error occurred: {ex.Message}");
        //    }
        //    int OrderID = newOreder.OrderId;
        //    Random rand = new Random();
        //    int RandomNum = rand.Next(3);
        //    string RandomNumber = RandomNum.ToString();
        //    string OrderNumber = "M-" + RandomNumber + OrderID;
        //    if (true)
        //    {

        //        string Ovar_hash = Createhash(OrderNumber, newOreder.OrderTotalPrice);
        //        string var_merchantId = "?merchantId=MID-23933-421";
        //        string var_orderId = "&orderId=" + OrderNumber;
        //        string var_amount = "&amount=" + (newOreder.OrderTotalPrice).ToString();
        //        string var_currency = "&currency=EGP";
        //        string var_hash = "&hash=" + Ovar_hash;
        //        string var_mode = "&mode=live";
        //        string var_merchantRedirect = "&merchantRedirect=https://apibe.nilecapitalcruises.com/" + "api/order/PaymentBack";
        //        string var_display = "&display=en";
        //        string var_allowedMethods = "&allowedMethods=card";
        //        string var_redirectMethod = "&redirectMethod=get";
        //        var Order = _dbContextOp.TblOrders.Where(x => x.OrderId == newOreder.OrderId).FirstOrDefault();
        //        Order.OrderConfirmationNumber = OrderNumber;
        //        _dbContextOp.SaveChanges();
        //        //Response.Redirect("https://checkout.kashier.io/" + var_merchantId + var_orderId + var_amount + var_currency + var_hash + var_mode + var_merchantRedirect + var_display + var_allowedMethods + var_redirectMethod);

        //        //return Redirect("https://checkout.kashier.io/" + var_merchantId + var_orderId + var_amount + var_currency + var_hash + var_mode + var_merchantRedirect + var_display + var_allowedMethods + var_redirectMethod);


        //        //return Redirect("https://checkout.kashier.io/\" + var_merchantId + var_orderId + var_amount + var_currency + var_hash + var_mode + var_merchantRedirect + var_display + var_allowedMethods + var_redirectMethod");



        //        return Ok("https://checkout.kashier.io/" + var_merchantId + var_orderId + var_amount + var_currency + var_hash + var_mode + var_merchantRedirect + var_display + var_allowedMethods + var_redirectMethod);

        //        // Use _httpClient to make HTTP requests
        //        //HttpResponseMessage response = await _httpClient.GetAsync("https://checkout.kashier.io/" + var_merchantId + var_orderId + var_amount + var_currency + var_hash + var_mode + var_merchantRedirect + var_display + var_allowedMethods + var_redirectMethod);

        //        //if (response.IsSuccessStatusCode)
        //        //{
        //        //    // Process successful response
        //        //    string responseBody = await response.Content.ReadAsStringAsync();
        //        //    return Ok(responseBody);
        //        //}
        //        //else
        //        //{
        //        //    // Handle error response
        //        //    return StatusCode((int)response.StatusCode);
        //        //}

        //        // Construct the URL to the external page
        //        //string externalUrl = "\"https://checkout.kashier.io/\" + var_merchantId + var_orderId + var_amount + var_currency + var_hash + var_mode + var_merchantRedirect + var_display + var_allowedMethods + var_redirectMethod";

        //        //string script = $@"
        //        //<script type='text/javascript'>
        //        //    window.location.href = '{externalUrl}';
        //        //</script>";

        //        //return Content(script, "text/html");
        //    }





        //}


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

            try
            {
               

                var cabin = _dbContext.TblCabins.Where(c => c.CabinId == Order.CabinId).FirstOrDefault();
                var cruiseItinerary = _dbContext.VwCruisesItineraries.Where(c => c.ItineraryId == Order.ItineraryId).FirstOrDefault();
                var nationality = _dbContextOp.TblCountriesNationalities.Where(n => n.NationalityId == Order.CustomerNationalityId).FirstOrDefault();
                var operationDate = _dbContext.TblOperationDates.Where(op => op.OperationDateId == Order.OperationDateId).FirstOrDefault()?.OperationDate;

                string bodyForCompany =
                @"<table  style='border:1px solid #ccc;text-align: left;border-collapse: collapse;width: 100%;'>
                           <tbody>";

                bodyForCompany +=
                @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>First Name: </td>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + Order.CustomerFirstName + @" </td>
                               </tr>";

                bodyForCompany +=
                @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Last Name: </td>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + Order.CustomerLastName + @" </td>
                               </tr>";

                bodyForCompany +=
               @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Email: </td>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + Order.CustomerEmail + @" </td>
                               </tr>";

                bodyForCompany +=
                @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Mobile Number: </td>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + Order.CustomerPhone + @" </td>
                               </tr>";

                bodyForCompany +=
                @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Nationality: </td>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + nationality?.NationalityName + @" </td>
                               </tr>";

                bodyForCompany +=
                @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Cruise Name: </td>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + cruiseItinerary?.CruiseNameSys + @" </td>
                               </tr>";

                bodyForCompany +=
                @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Cabin Name: </td>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + cabin?.CabinNameSys + @" </td>
                               </tr>";

                bodyForCompany +=
            @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Itinerary Name: </td>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + cruiseItinerary?.ItineraryNameSys + @" </td>
                               </tr>";

                bodyForCompany +=
              @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Operation Date: </td>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + operationDate?.ToString("dddd dd, MMMM , yyyy") + @" </td>
                               </tr>";


                bodyForCompany +=
                @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Adults No: </td>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + Order.AdultsNo + @" </td>
                               </tr>";


                bodyForCompany +=
                @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Child No: </td>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + Order.ChildNo + @" </td>
                               </tr>";

                bodyForCompany +=
               @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Order Confirmation Number: </td>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + Order.OrderConfirmationNumber + @" </td>
                               </tr>";

                bodyForCompany +=
              @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Payment Status: </td>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + Order.PmpaymentStatus + @" </td>
                               </tr>";

                bodyForCompany +=
            @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Card: </td>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + Order.PmmaskedCard + @" </td>
                               </tr>";

                bodyForCompany +=
@"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                 <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Order Reference: </td>
                 <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + Order.PmorderReference + @" </td>
                 </tr>";

                bodyForCompany +=
@"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                 <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Card Brand: </td>
                 <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + Order.PmcardBrand + @" </td>
                 </tr>";

                bodyForCompany +=
@"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                 <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Currency: </td>
                 <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + Order.Pmcurrency + @" </td>
                 </tr>";
               

                bodyForCompany +=
               @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Total Price: </td>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + Order.OrderTotalPrice + "$" + @"  </td>
                               </tr>";


                bodyForCompany +=
                @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Special Request: </td>
                               <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + Order.OrderSpecialRequest + @" </td>
                               </tr>";
                bodyForCompany += @"</tbody></table> ";

                SendMail("do-not-reply@nilecapitalcruises.com", "info@nilecapitalcruises.com", "Nile Capital Cruises Booking Request", bodyForCompany);
                SendMail("do-not-reply@nilecapitalcruises.com", "ahmed.taha@titegypt.com", "Nile Capital Cruises Booking Request", bodyForCompany);

                if (cruiseItinerary?.CruiseId == 1)
                {
                    string bodyForCustomer = @"<p style='font-size: 14px;'>Thanks! Your booking at Nile Capital Cruise is confirmed. We're thrilled to host you and are preparing to make your visit memorable.</p> <p style='font-weight: bold; font-size: 14px;'>Your Booking:</p>";
                    bodyForCustomer += bodyForCompany;
                    bodyForCustomer += @"<br><span style='font-weight: bold; font-size: 14px;'>Nile Capital Cruises sales team:</span><br>";
                    bodyForCustomer += @"<span style='font-size: 14px;'> <b>Email:</b> info@nilecapitalcruises.com</span><br>";
                    bodyForCustomer += @"<span style='font-size: 14px;'> <b>Mobile:</b> +20 111 111 8733</span><br>";
                    bodyForCustomer += @"<span style='font-size: 14px;'> <b>Address:</b> 12 Ismail Zohdi, Al Hadiqah Ad Dawleyah, Nasr City, Cairo Governorate</span><br>";

                    SendMail("do-not-reply@nilecapitalcruises.com", Order.CustomerEmail, "Booking Request", bodyForCustomer);
                    SendMail("do-not-reply@nilecapitalcruises.com", "ahmed.taha@titegypt.com", "Booking Request", bodyForCustomer);
                }
                else
                {
                    string bodyForCustomer = @"<p style='font-size: 14px;'>Thanks! Your booking at Agatha Christie Dahabiya is confirmed. We can't wait to provide you with an exceptional experience.</p> <p style='font-weight: bold; font-size: 14px;'>Your Booking:</p>";
                    bodyForCustomer += bodyForCompany;
                    bodyForCustomer += @"<br><span style='font-weight: bold; font-size: 14px;'>Nile Capital Cruises sales team:</span><br>";
                    bodyForCustomer += @"<span style='font-size: 14px;'> <b>Email:</b> info@nilecapitalcruises.com</span><br>";
                    bodyForCustomer += @"<span style='font-size: 14px;'> <b>Mobile:</b> +20 111 111 8733</span><br>";
                    bodyForCustomer += @"<span style='font-size: 14px;'> <b>Address:</b> 12 Ismail Zohdi, Al Hadiqah Ad Dawleyah, Nasr City, Cairo Governorate</span><br>";

                    SendMail("do-not-reply@nilecapitalcruises.com", Order.CustomerEmail, "Booking Request", bodyForCustomer);
                    SendMail("do-not-reply@nilecapitalcruises.com", "ahmed.taha@titegypt.com", "Booking Request", bodyForCustomer);
                }

                var orderResponse = new CLS_Order_Response()
                {
                    CustomerFirstName = Order.CustomerFirstName,
                    CustomerLastName = Order.CustomerLastName,
                    CustomerEmail = Order.CustomerEmail,
                    CustomerPhone = Order.CustomerPhone,
                    NationalityName = nationality?.NationalityName,
                    CruiseNameSys = cruiseItinerary?.CruiseNameSys,
                    CabinNameSys = cabin?.CabinNameSys,
                    ItineraryNameSys = cruiseItinerary?.ItineraryNameSys,
                    str_OperationDate = operationDate?.ToString("dddd dd, MMMM , yyyy"),
                    AdultsNo = Order.AdultsNo,
                    ChildNo = Order.ChildNo,
                    OrderTotalPrice = Order.OrderTotalPrice,
                    OrderSpecialRequest = Order.OrderSpecialRequest

                };

                //var queryString = $"?data={orderResponse}";
                //var jsonOrderResponse = JsonConvert.SerializeObject(orderResponse);
                //var queryString = $"?data={jsonOrderResponse}";

                //return Redirect("https://ws.nilecapitalcruises.com/en/cabinsbooking/1/thank-you" + queryString);
                return Redirect("https://nilecapitalcruises.com/en/thank-you");
            }
            catch (Exception ex)
            {

                Console.WriteLine($"An error occurred: {ex.Message}");
                return BadRequest(ex.Message);
            }

            

            }



            public static string Createhash(string OrderNumber, double? TotalAmount)
            {
            //string mid = "MID-18872-595";
            string mid = "MID-23933-421";
            string amount = TotalAmount.ToString();
                string currency = "EGP";
                string orderId = OrderNumber;
            //string secret = "856db08a-0429-4b82-8d92-9ba850e17cf1";
            string secret = "1de29d75-0e3d-428f-ac3e-bca58e90beee";
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
            public ActionResult OrderDetails(int OrderId)
            {
                var order = _dbContextOp.TblOrders.Where(x => x.OrderId == OrderId).FirstOrDefault();
                if (order == null)
                {
                    return NotFound();
                }


            //if (order.OrderStatusId != 2)
            //{
            //    return BadRequest("The coustomer not paiad");
            //}
            var operationDate = _dbContext.TblOperationDates.Where(x => x.OperationDateId == order.OperationDateId).FirstOrDefault()?.OperationDate;
                var cabin = _dbContext.TblCabins.Where(c => c.CabinId == order.CabinId).FirstOrDefault();
                var cruiseItinerary = _dbContext.VwCruisesItineraries.Where(c => c.ItineraryId == order.ItineraryId).FirstOrDefault();
                var nationality = _dbContextOp.TblCountriesNationalities.Where(n => n.NationalityId == order.CustomerNationalityId).FirstOrDefault();
            var Obj_GetOrder = new CLS_Order_Response()
                {
                    OrderId = order.OrderId,
                    CustomerFirstName = order.CustomerFirstName,
                    CustomerLastName = order.CustomerLastName,
                    CustomerEmail = order.CustomerEmail,
                    CustomerPhone = order.CustomerPhone,
                    NationalityName = nationality?.NationalityName,
                    CruiseNameSys = cruiseItinerary?.CruiseNameSys,
                    CabinNameSys = cabin?.CabinNameSys,
                    ItineraryNameSys = cruiseItinerary?.ItineraryNameSys,
                    str_OperationDate = operationDate?.ToString("dddd dd, MMMM , yyyy"),
                    AdultsNo = order.AdultsNo,
                    ChildNo = order.ChildNo,
                    OrderTotalPrice = order.OrderTotalPrice,
                    OrderSpecialRequest = order.OrderSpecialRequest,
                    OrderConfirmationNumber = order.OrderConfirmationNumber,
                    PaymentStatus = order.PmpaymentStatus,
                    Card = order.PmmaskedCard,
                    OrderReference = order.PmorderReference,
                    CardBrand = order.PmcardBrand,
                    Currency = order.Pmcurrency,
                    Str_OrderDate = order.OrderDate?.ToString("dddd dd, MMMM , yyyy"),

            };
            return Ok(Obj_GetOrder);
            }



        [Authorize(Roles = "Admin")]
        [HttpGet("cms/getOrders")]

        public ActionResult GetOrders()
        {
            var items = _dbContextOp.VwOrders.Where(o => o.IsDeleted == false).ToList();

            var operationDates = _dbContext.TblOperationDates.ToList();
            var cabins = _dbContext.TblCabins.ToList();
            var cruiseItineraries = _dbContext.VwCruisesItineraries.ToList();

            if (items.Count < 1)
                return NotFound();

            var List_Orders = items.Select(o => new CLS_Order_Response()
            {
                OrderId = o.OrderId,
                OrderStatus = o.OrderStatus,
                CustomerFirstName = o.CustomerFirstName,
                CustomerLastName = o.CustomerLastName,
                CustomerEmail = o.CustomerEmail,
                CustomerPhone = o.CustomerPhone,
                NationalityName = o.NationalityName,
                CruiseNameSys = cruiseItineraries.Where(c => c.ItineraryId == o.ItineraryId).FirstOrDefault()?.CruiseNameSys,
                CabinNameSys = cabins.Where(c => c.CabinId == o.CabinId).FirstOrDefault()?.CabinNameSys,
                ItineraryNameSys = cruiseItineraries.Where(c => c.ItineraryId == o.ItineraryId).FirstOrDefault()?.ItineraryNameSys,
                str_OperationDate = operationDates.Where(x => x.OperationDateId == o.OperationDateId).FirstOrDefault()?.OperationDate?.ToString("dddd dd, MMMM , yyyy"),
                AdultsNo = o.AdultsNo,
                ChildNo = o.ChildNo,
                OrderTotalPrice = o.OrderTotalPrice,
                OrderSpecialRequest = o.OrderSpecialRequest,
                OrderConfirmationNumber = o.OrderConfirmationNumber,
                PaymentStatus = o.PmpaymentStatus,
                Card = o.PmmaskedCard,
                OrderReference = o.PmorderReference,
                CardBrand = o.PmcardBrand,
                Currency = o.Pmcurrency,
                Str_OrderDate = o.OrderDate?.ToString("dddd dd, MMMM , yyyy"),

            });

            return Ok(List_Orders);
        }

        [HttpPost("deleteOrder/{orderId}")]
        
        public async Task<IActionResult> DeleteOrder([FromRoute] int orderId)
        {

            var item = await _dbContextOp.TblOrders.FindAsync(orderId);

            if (item == null)
            {
                return NotFound();
            }

            item.IsDeleted = true;

            await _dbContextOp.SaveChangesAsync();

            return Ok();
        }
        private CLS_RatePriceStructure GetRatePriceStructures()
            {
                var ratesPricesStructures = _dbContext.TblRatesPricesStructures.ToList();
                var infant = ratesPricesStructures.FirstOrDefault(r => r.Description == "Infant");
                var child6 = ratesPricesStructures.FirstOrDefault(r => r.Description == "Child6");
                var child12 = ratesPricesStructures.FirstOrDefault(r => r.Description == "Child12");
                var singleAdult = ratesPricesStructures.FirstOrDefault(r => r.Description == "SingleAdult");

                return new CLS_RatePriceStructure()
                {
                    Infant = infant,
                    Child6 = child6,
                    Child12 = child12,
                    SingleAdult = singleAdult,

                };
            }

            private double? CalculatePrice(TblRatesPricesStructure ratesPrice, double? price)
            {
                return (ratesPrice.IsPercentage ?? false) ?
                            ((ratesPrice.IsPositiveSign ?? false) ? price + ((ratesPrice.Value / 100) * price) : price - ((ratesPrice.Value / 100) * price)) :
                            ((ratesPrice.IsPositiveSign ?? false) ? price + ratesPrice.Value : price - ratesPrice.Value);
            }



            private void SendMail(string fromAddress, string toAddress, string mailSubject, string mailBody)
            {

                MailMessage mail = new MailMessage();
                MailMessage mailMsg = new MailMessage(fromAddress, toAddress, mailSubject, mailBody);
                mailMsg.IsBodyHtml = true;
                mailMsg.Priority = MailPriority.Normal;
                SmtpClient smtp = new SmtpClient();

                smtp.EnableSsl = true;

                smtp.Credentials = new NetworkCredential("do-not-reply@nilecapitalcruises.com", "Uhll38^86_Ak81r7q9");
                smtp.Host = "red.specialservers.com";
                smtp.Port = 587;

                smtp.Send(mailMsg);

            }



            

        }
    }





//Thanks! Your booking at Nile Capital Cruise is confirmed. We're thrilled to host you and are preparing to make your visit memorable.


//Thanks! Your booking is confirmed at Agatha Christie Dahabiya. We can't wait to provide you with an exceptional experience.