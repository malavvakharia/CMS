using Campaign.BL;
using Campaign.Data;
using Campaign.IBL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CampaignManagementSystem.Controllers
{
    public class LoginController : Controller
    {
       
        private ILoginRepository iLoginRepository;
        public LoginController(ILoginRepository _iLoginRepository)
        {
            iLoginRepository = _iLoginRepository;
        }
        [HttpGet]
        public ActionResult Login()
        {
            if (Authantication())
            {
                return RedirectToAction("Login");
            }

            ViewBag.Email = "";
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserLogin u)
        {
            if (Authantication())
            {
                return RedirectToAction("Login");
            }

            ViewBag.LoginError = "";
            if (u.Email == null || u.Password == null)
            {
                ViewBag.LoginError = "Please Enter Email And/Or Password";
                return RedirectToAction("Login");
            }
          
            var userDetail = iLoginRepository.GetUserByEmailPassword(u.Email,u.Password);
            if (userDetail != null)
            {
                if (u.RememberMe)
                {
                    HttpCookie LoginCookie = new HttpCookie("LoginCookie", u.Email);
                    LoginCookie.Expires = DateTime.Now.AddDays(1);
                    Session["Role"] = userDetail.Role;
                    Response.Cookies.Add(LoginCookie);
                }
                Session["Role"] = userDetail.Role;
                Session["Email"] = u.Email;
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                ViewBag.LoginError = "Wrong Username And / Or Password";
                return View();
            }
        }
        [HttpGet]
        public ActionResult AddUser()
        {

            if (!Authorization("ADMIN"))
            {
                return RedirectToAction("Login");
            }


            ViewBag.Email = "";
            ViewBag.Fname = "";
            ViewBag.Lname = "";

            return View();
        }
        [HttpPost]
        public ActionResult AddUser(UserLogin u)
        {
   
            if (!Authorization("ADMIN"))
            {
                return RedirectToAction("Login");
            }


            if (ModelState.IsValid)
            {
                ViewBag.EmailValidationError = "";
                ViewBag.PwdValidationError = "";
                ViewBag.NameValidationError = "";
                ViewBag.FormValidationError = "";

                ViewBag.Email = u.Email;
                ViewBag.Fname = u.FName;
                ViewBag.Lname = u.LName;

                if (u.FName == null || u.LName == null || u.Email == null || u.Password == null || u.Role == null)
                {
                    ViewBag.FormValidationError = "Looks Like You Are Missing Something!!!";
                    return View(u);
                }

                if (!iLoginRepository.CheckEmail(u.Email))
                {
                    ViewBag.EmailValidationError = "Please Enter Valid Email";
                    return View(u);
                }

                if (!iLoginRepository.CheckPassword(u.Password))
                {
                    ViewBag.PwdValidationError = "Password must be at least 8 characters, no more than 16 characters, and must include at least one upper case letter, one lower case letter, and one numeric digit";
                    return View(u);
                }

                if (!iLoginRepository.CheckName(u.FName) || !iLoginRepository.CheckName(u.LName))
                {
                    ViewBag.NameValidationError = "Invalid First Name And / Or Last Name";
                    return View(u);
                }

                iLoginRepository.AddUser(u);
                return RedirectToAction("Login");
            }

            return View(u);
        }
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ForgotPassword(string Email)
        {
            try
            {
                var objUser = iLoginRepository.GetUserByEmail(Email);
                if (objUser != null)
                {
                    string pwd = iLoginRepository.UpdateAndReturnUserPassword(objUser);
                    var body = createEmailBody(Email, pwd);
                    var message = new MailMessage();
                    message.To.Add(new MailAddress(Email));
                    message.From = new MailAddress("campaignmanagement0@gmail.com");
                    message.Subject = "Reset Password";
                    message.Body = body;
                    message.IsBodyHtml = true;

                    using (var smtp = new SmtpClient())
                    {
                        var credential = new NetworkCredential
                        {
                            UserName = "campaignmanagement0@gmail.com",
                            Password = "CMScms123"
                        };
                        smtp.Credentials = credential;
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;

                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(message);

                        ViewBag.Error = "Check Your Email !!!";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Error = "No Such Email Found";
                    return View();
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = "Some Error" + e.ToString();
            }
            return View();
        }

        private string createEmailBody(string Email, string pwd)
        {
            string body = string.Empty;
            //using streamreader for reading my htmltemplate   
            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplate.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{Email}", Email); //replacing the required things  
            body = body.Replace("{ResetPassword}", pwd);

            return body;
        }
        private bool Authantication()
        {
            if (Session["Email"] != null || HttpContext.Request.Cookies["LoginCookie"] != null)
            {
                return true;
            }
            return false;
        }
        private bool Authorization(string UserRole)
        {
            if (Session["Role"] != null)
            {
                string CurrentUserRole = Session["Role"].ToString();
                if (!string.IsNullOrEmpty(UserRole) && CurrentUserRole.Equals(UserRole))
                {
                    return true;
                }
            }
            return false;
        }
    }
}