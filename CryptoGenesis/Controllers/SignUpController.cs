using CryptoGenesis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CryptoGenesis.Controllers
{
    public class SignUpController : Controller
    {
        // GET: SignUp
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SignUp(SignUpRequest request)
        {

            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.Username))
                return Json(Helpers.StandarError("Not allowed null fields."));

            //  Validate email format
            if (new EmailAddressAttribute().IsValid(request.Email) == false)
                return Json(Helpers.StandarError("Invalid Email."));

            try
            {
                using (var entity = new CryptoGenesisEntities())
                {
                    User usr = new User
                    {
                        Username = request.Username,
                        Email = request.Email,
                        Password = request.Password,
                        EmailVerified = false,
                        Enabled = true
                    };
                    int affiliateBy = -1;

                    if (!string.IsNullOrEmpty(request.AffiliatedBy))
                        affiliateBy = (from u in entity.User
                                       where u.Username == request.AffiliatedBy
                                       select u.UserId).FirstOrDefault();

                    if (affiliateBy > 0) usr.AffiliatedBy = affiliateBy;

                    entity.User.Add(usr);
                    entity.SaveChanges();

                    usr = (from u in entity.User where u.Username == usr.Username select u).FirstOrDefault();

                    

                    entity.Wallet.AddRange(DefaultWalletsCreator(usr.UserId));

                    entity.SaveChanges();

                    return Json(new StandartResponse { OperationSuccessfully = true, response = "" });

                }
            }
            catch (Exception)
            {
                return Json(Helpers.ServerError());
            }
        }

        public class SignUpRequest
        {
           public string Username { get; set; }
           public string Email { get; set; }
           public string Password { get; set; }
           public string AffiliatedBy { get; set; }
        }

        private List<Wallet> DefaultWalletsCreator(int pUserId)
        {
            List<Wallet> wallets = new List<Wallet>();
            wallets.Add(new Wallet
            {
                UserId = pUserId,
                Balance = 0,
                Credit = 0,
                FreezeBalance = 0,
                CurrencyId = "USDT"
            });

            wallets.Add(new Wallet
            {
                UserId = pUserId,
                Balance = 0,
                Credit = 0,
                FreezeBalance = 0,
                CurrencyId = "DLC"
            });

            return wallets;
        }
        
    }
}