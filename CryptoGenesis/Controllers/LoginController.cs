using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using CryptoGenesis.Models;
using System.Security.Cryptography;

namespace CryptoGenesis.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Login(string pUser, string pPassword, string pKey)
        {

            try
            {
                using (var entity = new CryptoGenesisEntities())
                {
                    var allowedVersion = (from c in entity.ConfigGeneral select c).FirstOrDefault().AllowedVersion;


                    if (pKey != Helpers.ComputeHash386(pPassword + allowedVersion)) return Json(new LoginResponse
                    {
                        successfully = false,
                        response = "Please, update the application.",
                        token = null
                    });

                    var pswHash = Helpers.ComputeHash386(pPassword);

                    var user = (from u in entity.User
                                where (u.Email == pUser || u.Username == pUser) && u.Password == pswHash
                                select u).FirstOrDefault();

                    if (user == null) return Json(new LoginResponse
                    {
                        successfully = false,
                        response = "Wrong username or password.",
                        token = null
                    });

                    var token = Helpers.GenerateToken();

                    entity.SessionToken.Add(new SessionToken
                    {
                        CreationDate = DateTime.Now,
                        TokenString = token,
                        UserId = user.UserId
                    });

                    entity.SaveChanges();

                    return Json(new LoginResponse
                    {
                        successfully = true,
                        response = "",
                        token = token
                    });
                }
            }

            catch(Exception)
            {
                return Json(new LoginResponse
                {
                    successfully = false,
                    response = "Internal server error.",
                    token = null
                });
            }
        }

        private class LoginResponse 
        { 
            public bool successfully { get; set; }
            public string response { get; set; }
            public string token { get; set; }
        }
    }
}