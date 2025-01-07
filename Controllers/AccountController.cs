using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using MvcAppV2.Filters;
using MvcAppV2.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace IflowSeed.Controllers
{

    public class AccountController : Controller
    {
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            ModelState.Clear();

            ViewBag.IsDepart = @Session["Department"];      
            if (ModelState.IsValid & model.UserName != null && model.Password!= null)
            {
                byte[] Password = Encrypt(model.Password);

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    cn.Open();
                    command.CommandText = @"SELECT [Fullname],[Role],[Department],[Id],[Email],[MobileNo],[Username],[CreatedOn] FROM [dbo].[User]                                    
                                     WHERE [Username]=@UserId AND [Password]=@Password";
                    command.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = model.UserName;
                    command.Parameters.Add("@Password", SqlDbType.VarBinary).Value = Password;
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string fullname = reader.GetString(0);
                        string role = reader.GetString(1);
                        Session["Fullname"] = fullname.ToString();
                        Session["Role"] = role.ToString();
                        Session["Department"] = reader.GetString(2);
                        Session["Idx"] = reader.GetGuid(3);
                        Session["Email"] = reader.GetString(4);
                        Session["MobileNo"] = reader.GetString(5);
                        Session["StaffId"] = reader.GetString(6);

                        Guid Idx = reader.GetGuid(3);
                        DateTime UpDate = reader.GetDateTime(7);
               
                        if (DateTime.Now > UpDate)
                        {                
                             ModelState.AddModelError("", "Password has expired,Please update your password!!");
                             return RedirectToAction("UpdatePassword3month", "Home", new { Id = Idx.ToString() });
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }                       
                    }
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                    cn.Close();
                    
                }  
            }
            // If we got this far, something failed, redisplay form         
            return View(model);
        }

        //
        // POST: /Account/LogOff

       
        
        [HttpPost]
        public ActionResult LogOff()
        {
            Session.RemoveAll();
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register(RegisterModel model, string role_)
        {
            ViewBag.IsDepart = @Session["Department"];      
            if (ModelState.IsValid && model.Role != "-" && model.Department != "-")
            {
                try
                {
                    //WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    //WebSecurity.Login(model.UserName, model.Password);

                   byte[] Password = Encrypt(model.Password);

                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        Guid Id = Guid.NewGuid();
                        string createdOn = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt");
                        cn.Open();
                        SqlCommand command;
                        command = new SqlCommand("INSERT INTO [dbo].[User] (Id,CreatedOn,Username,Password,Fullname,Email,Role,Department,MobileNo) values (@Id,@CreatedOn,@Username,@Password,@Fullname,@Email,@Role,@Department,@MobileNo)", cn);
                        command.Parameters.AddWithValue("@Id", Id);
                        command.Parameters.AddWithValue("@CreatedOn", createdOn);
                        command.Parameters.AddWithValue("@Username", model.UserName);
                        command.Parameters.AddWithValue("@Password", Password);
                        command.Parameters.AddWithValue("@Fullname", model.Fullname);
                        command.Parameters.AddWithValue("@Email", model.Email);
                        command.Parameters.AddWithValue("@Role", model.Role);
                        command.Parameters.AddWithValue("@Department", model.Department);
                        command.Parameters.AddWithValue("@MobileNo", model.MobileNo);
                        command.ExecuteNonQuery();
                        cn.Close();
                    }

                    Session["Fullname"] = model.Fullname.ToString();
                    return RedirectToAction("ManageUser", "Account"); 
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            List<SelectListItem> listrole = new List<SelectListItem>();
            listrole.Add(new SelectListItem { Text = "Select", Value = "-" });
            listrole.Add(new SelectListItem { Text = "Account Manager", Value = "Account Manager" });
            listrole.Add(new SelectListItem { Text = "Sales Executive", Value = "Sales Executive" });
            listrole.Add(new SelectListItem { Text = "IT Assistant", Value = "IT Assistant" });
            listrole.Add(new SelectListItem { Text = "Graphic Designer", Value = "Graphic Designer" });
            listrole.Add(new SelectListItem { Text = "QM Manager", Value = "QM Manager" });
            listrole.Add(new SelectListItem { Text = "QA Inspector", Value = "QA Inspector" });
            listrole.Add(new SelectListItem { Text = "QM Executive", Value = "QM Executive" });
            listrole.Add(new SelectListItem { Text = "Account Assistant", Value = "Account Assistant" });
            listrole.Add(new SelectListItem { Text = "Asst. Account", Value = "Asst.Account" });
            listrole.Add(new SelectListItem { Text = "HOD Sale", Value = "HOD Sale" });

            ViewData["role_"] = listrole;

            List<SelectListItem> listDept = new List<SelectListItem>();
            listDept.Add(new SelectListItem { Text = "Select", Value = "-" });
            listDept.Add(new SelectListItem { Text = "MBD", Value = "MBD" });
            listDept.Add(new SelectListItem { Text = "TMO", Value = "TMO" });
            listDept.Add(new SelectListItem { Text = "QM", Value = "QM" });
            listDept.Add(new SelectListItem { Text = "FINANCE", Value = "FINANCE" });
            listDept.Add(new SelectListItem { Text = "PRODUCTION", Value = "PRODUCTION" });
            ViewData["Department"] = listDept;

            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register2(RegisterModel model, string role_)
        {
            ViewBag.IsDepart = @Session["Department"];      
            if (ModelState.IsValid && model.Role != "-")
            {
                // Attempt to register the user
                try
                {
                    //WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    //WebSecurity.Login(model.UserName, model.Password);

                   byte[] Password = Encrypt(model.Password);

                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        Guid Id = Guid.NewGuid();
                        string createdOn = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt");
                        cn.Open();
                        SqlCommand command;
                        command = new SqlCommand("INSERT INTO [dbo].[User] (Id,CreatedOn,Username,Password,Fullname,Email,Role,Department,MobileNo) values (@Id,@CreatedOn,@Username,@Password,@Fullname,@Email,@Role,@Department,@MobileNo)", cn);
                        command.Parameters.AddWithValue("@Id", Id);
                        command.Parameters.AddWithValue("@CreatedOn", createdOn);
                        command.Parameters.AddWithValue("@Username", model.UserName);
                        command.Parameters.AddWithValue("@Password", Password);
                        command.Parameters.AddWithValue("@Fullname", model.Fullname);
                        command.Parameters.AddWithValue("@Email", model.Email);
                        command.Parameters.AddWithValue("@Role", model.Role);
                        command.Parameters.AddWithValue("@Department", model.Department);
                        command.Parameters.AddWithValue("@MobileNo", model.MobileNo);
                        command.ExecuteNonQuery();
                        cn.Close();
                    }

                    Session["Fullname"] = model.Fullname.ToString();
                    return RedirectToAction("Register", "Account"); 
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            List<SelectListItem> listrole = new List<SelectListItem>();
            listrole.Add(new SelectListItem { Text = "Select", Value = "-" });
            listrole.Add(new SelectListItem { Text = "Account Manager", Value = "Account Manager" });
            listrole.Add(new SelectListItem { Text = "Sales Executive", Value = "Sales Executive" });
            listrole.Add(new SelectListItem { Text = "IT Assistant", Value = "IT Assistant" });
            listrole.Add(new SelectListItem { Text = "Graphic Designer", Value = "Graphic Designer" });
            listrole.Add(new SelectListItem { Text = "QM Manager", Value = "QM Manager" });
            listrole.Add(new SelectListItem { Text = "QA Inspector", Value = "QA Inspector" });
            listrole.Add(new SelectListItem { Text = "QM Executive", Value = "QM Executive" });
            listrole.Add(new SelectListItem { Text = "Account Assistant", Value = "Account Assistant" });
            listrole.Add(new SelectListItem { Text = "Asst. Account", Value = "Asst.Account" });
            ViewData["role_"] = listrole;

            return View(model);
        }           

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (UsersContext db = new UsersContext())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion

        protected static SymmetricAlgorithm algorithm = BuildAlgorithm();
        protected byte[] Encrypt(string clearText)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                CryptoStream cs = new CryptoStream(ms,
                                                    algorithm.CreateEncryptor(),
                                                    CryptoStreamMode.Write);

                using (StreamWriter sw = new StreamWriter(cs))
                {
                    sw.Write(clearText);
                    sw.Close();
                    return ms.ToArray();
                }
            }
        }

        private static SymmetricAlgorithm BuildAlgorithm()
        {
            var algo = RijndaelManaged.Create();

            //TODO: Change key
            algo.Key = Encoding.UTF8.GetBytes("YasAlunakaAniruh");
            algo.IV = Encoding.UTF8.GetBytes("RuuhMinAmriRabbi");

            Array.Reverse(algo.Key);
            Array.Reverse(algo.IV);

            return algo;
        }

        protected string Decrypt(byte[] cipherText)
        {
            using (MemoryStream ms = new MemoryStream(cipherText))
            {
                CryptoStream cs = new CryptoStream(ms,
                                            algorithm.CreateDecryptor(),
                                            CryptoStreamMode.Read);
                using (StreamReader sr = new StreamReader(cs))
                {
                    string val = sr.ReadToEnd();
                    sr.Close();
                    return val;
                }
            }
        }      
    }
}
