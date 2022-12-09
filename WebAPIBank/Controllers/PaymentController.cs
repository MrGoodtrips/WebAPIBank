using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPIBank.DesignPatterns.SingletonPattern;
using WebAPIBank.DTOClasses;
using WebAPIBank.Models.Context;
using WebAPIBank.Models.Entities;

namespace WebAPIBank.Controllers
{
    public class PaymentController : ApiController
    {
        MyContext _db;

        public PaymentController()
        {
            _db = DBTool.DbInstance;
        }

        //Aşağıdaki Action test içindir... API canlıya çıktığı zaman kesinlikle açık bırakılmamalıdır...
        //[HttpGet]
        //public List<PaymentDTO> GetAll()
        //{
        //    return _db.Cards.Select(x => new PaymentDTO
        //    {
        //        CardUserName = x.CardUserName,
        //        CardNumber = x.CardNumber,
        //        CardExpiryYear = x.CardExpiryYear,
        //        CardExpiryMonth= x.CardExpiryMonth,
        //        SecurityNumber = x.SecurityNumber,
        //        ID = x.ID
        //    }).ToList();
        //}

        private void SetBalance(PaymentDTO item, CardInformation ci)
        {
            ci.Balance -= item.ShoppingPrice;
            _db.SaveChanges();
        }

        public IHttpActionResult ReceivePayment(PaymentDTO item)
        {
            CardInformation ci = _db.Cards.FirstOrDefault(x => x.CardNumber == item.CardNumber && x.CardExpiryMonth == item.CardExpiryMonth && x.CardExpiryYear == item.CardExpiryYear && x.CardUserName == item.CardUserName && x.SecurityNumber == item.SecurityNumber);

            if(ci != null)
            {
                if(ci.CardExpiryYear < DateTime.Now.Year)
                {
                    return BadRequest("Expired Card");
                }
                else if(ci.CardExpiryYear == DateTime.Now.Year)
                {
                    if(ci.CardExpiryMonth < DateTime.Now.Month)
                    {
                        return BadRequest("Expired Card");
                    }

                    if (ci.Balance >= item.ShoppingPrice)
                    {
                        SetBalance(item, ci);
                        return Ok();
                    }
                    else return BadRequest("Balance Exceeded");

                }

                if(ci.Balance >= item.ShoppingPrice)
                {
                    SetBalance(item, ci);
                    return Ok();
                }
                return BadRequest("Balance Exceeded");

            }

            return BadRequest("Card not found");

        }

    }
}
