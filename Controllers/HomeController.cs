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
using static DotNetOpenAuth.OpenId.Extensions.AttributeExchange.WellKnownAttributes;
using System.Diagnostics;
using JQChart.Web.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

[MvcAppV2.FilterConfig.UserSessionActionFilter]
public class HomeController : Controller
{

    List<User> viewUserlist = new List<User>();
    public ActionResult ManageStaff(string Id, string FullName, string set)
    {
        if (set == "search")
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Username,Fullname,Email,Role,Department,MobileNo,Id
                                     FROM [IflowSeed].[dbo].[User]
                                     WHERE FullName LIKE @FullName
                                     ORDER BY Department";
                command.Parameters.AddWithValue("@FullName", "%" + FullName + "%");
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    User model = new User();
                    {
                        model.Bil = _bil++;
                        if (reader.IsDBNull(0) == false)
                        {
                            model.Username = reader.GetString(0);
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            model.Fullname = reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            model.Email = reader.GetString(1);
                        }
                        if (reader.IsDBNull(3) == false)
                        {
                            model.Role = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            model.Department = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            model.MobileNo = reader.GetString(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.Id = reader.GetGuid(6);
                        }
                    }
                    viewUserlist.Add(model);
                }
                cn.Close();
            }
        }
        else
        {
            //ALL
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT  Username,Fullname,Email,Role,Department,MobileNo,Id
                                       FROM [IflowSeed].[dbo].[User]
                                      ORDER BY Fullname";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    User model = new User();
                    {
                        model.Bil = _bil++;
                        if (reader.IsDBNull(0) == false)
                        {
                            model.Username = reader.GetString(0);
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            model.Fullname = reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            model.Email = reader.GetString(1);
                        }
                        if (reader.IsDBNull(3) == false)
                        {
                            model.Role = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            model.Department = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            model.MobileNo = reader.GetString(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.Id = reader.GetGuid(6);
                        }
                    }
                    viewUserlist.Add(model);
                }
                cn.Close();
            }
        }
        return View(viewUserlist);
    }


    public JsonResult insertStaff(RegisterStaff vm)
    {


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
        listrole.Add(new SelectListItem { Text = "Planner", Value = "Planner" });
        listrole.Add(new SelectListItem { Text = "Printing", Value = "Printing" });
        listrole.Add(new SelectListItem { Text = "Inserting", Value = "Inserting" });
        listrole.Add(new SelectListItem { Text = "MMP", Value = "MMP" });
        listrole.Add(new SelectListItem { Text = "Posting", Value = "Posting" });
        listrole.Add(new SelectListItem { Text = "Super Admin", Value = "Super Admin" });
        ViewData["role_"] = listrole;


        List<SelectListItem> listDept = new List<SelectListItem>();
        listDept.Add(new SelectListItem { Text = "Select", Value = "-" });
        listDept.Add(new SelectListItem { Text = "MBD", Value = "MBD" });
        listDept.Add(new SelectListItem { Text = "BSS", Value = "BSS" });
        listDept.Add(new SelectListItem { Text = "IT", Value = "IT" });
        listDept.Add(new SelectListItem { Text = "QM", Value = "QM" });
        listDept.Add(new SelectListItem { Text = "FINANCE", Value = "FINANCE" });
        listDept.Add(new SelectListItem { Text = "PRODUCTION", Value = "PRODUCTION" });
        ViewData["Department_"] = listDept;


        if (!string.IsNullOrEmpty(vm.Password) && vm.Password.Length >= 8 && !string.IsNullOrEmpty(vm.UserName) && !string.IsNullOrEmpty(vm.Fullname) && !string.IsNullOrEmpty(vm.Email) && !string.IsNullOrEmpty(vm.Role) && !string.IsNullOrEmpty(vm.Department) && !string.IsNullOrEmpty(vm.MobileNo))
        {
            byte[] EncryptPassword = Encrypt(vm.Password);

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                Guid Id = Guid.NewGuid();
                string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[User] (Id,CreatedOn,Username,Password,Fullname,Email,Role,Department,MobileNo) values (@Id,@CreatedOn,@Username,@Password,@Fullname,@Email,@Role,@Department,@MobileNo)", cn);
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@CreatedOn", createdOn);
                command.Parameters.AddWithValue("@Username", vm.UserName);
                command.Parameters.AddWithValue("@Password", EncryptPassword);
                command.Parameters.AddWithValue("@Fullname", vm.Fullname);
                command.Parameters.AddWithValue("@Email", vm.Email);
                command.Parameters.AddWithValue("@Role", vm.Role);
                command.Parameters.AddWithValue("@Department", vm.Department);
                command.Parameters.AddWithValue("@MobileNo", vm.MobileNo);
                command.ExecuteNonQuery();
                cn.Close();
            }


        }

        var Idx = ViewBag.id;

        return Json(Idx, JsonRequestBehavior.AllowGet);
    }

    public ActionResult RegisterStaff(string submitButton, string Password, string UserName, string Fullname, string Email, string Role, string Department, string MobileNo)

    {


        try
        {
            int _bil1 = 1;
            //List<SelectListItem> li5 = new List<SelectListItem>();
            //using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            //using (SqlCommand command = new SqlCommand("", cn))
            //{
            //    cn.Open();
            //    command.CommandText = @"SELECT DISTINCT type FROM [IflowSeed].[dbo].[Role]          
            //                         ORDER BY type";
            //    var reader = command.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        RegisterStaff model = new RegisterStaff();
            //        {
            //            if (reader.IsDBNull(0) == false)
            //            {
            //                model.RoleType = reader.GetString(0);
            //            }
            //        }
            //        int i = _bil1++;
            //        if (i == 1)
            //        {
            //            li5.Add(new SelectListItem { Text = "Please Select" });
            //            li5.Add(new SelectListItem { Text = model.RoleType });

            //        }
            //        else
            //        {
            //            li5.Add(new SelectListItem { Text = model.RoleType });
            //        }
            //    }
            //    cn.Close();
            //}
            //ViewData["role_"] = li5;


            int _bil2 = 1;
            //List<SelectListItem> li6 = new List<SelectListItem>();
            //using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            //using (SqlCommand command = new SqlCommand("", cn))
            //{
            //    cn.Open();
            //    command.CommandText = @"SELECT DISTINCT List_Dept FROM [IflowSeed].[dbo].[Department]          
            //                         ORDER BY List_Dept";
            //    var reader = command.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        RegisterStaff model = new RegisterStaff();
            //        {
            //            if (reader.IsDBNull(0) == false)
            //            {
            //                model.Department = reader.GetString(0);
            //            }
            //        }
            //        int i = _bil2++;
            //        if (i == 1)
            //        {
            //            li6.Add(new SelectListItem { Text = "Please Select" });
            //            li6.Add(new SelectListItem { Text = model.Department });

            //        }
            //        else
            //        {
            //            li6.Add(new SelectListItem { Text = model.Department });
            //        }
            //    }
            //    cn.Close();
            //}

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
            listrole.Add(new SelectListItem { Text = "Asst Account", Value = "Asst Account" });
            listrole.Add(new SelectListItem { Text = "Planner", Value = "Planner" });
            listrole.Add(new SelectListItem { Text = "Printing", Value = "Printing" });
            listrole.Add(new SelectListItem { Text = "Inserting", Value = "Inserting" });
            listrole.Add(new SelectListItem { Text = "Engineering", Value = "Engineering" });
            listrole.Add(new SelectListItem { Text = "MMP", Value = "MMP" });
            listrole.Add(new SelectListItem { Text = "Posting", Value = "Posting" });
            listrole.Add(new SelectListItem { Text = "Programmer", Value = "Programmer" });
            listrole.Add(new SelectListItem { Text = "Super Admin", Value = "Super Admin" });
            ViewData["role_"] = listrole;

            List<SelectListItem> listDept = new List<SelectListItem>();
            listDept.Add(new SelectListItem { Text = "Select", Value = "-" });
            listDept.Add(new SelectListItem { Text = "MBD", Value = "MBD" });
            listDept.Add(new SelectListItem { Text = "BSS", Value = "BSS" });
            listDept.Add(new SelectListItem { Text = "IT", Value = "IT" });
            listDept.Add(new SelectListItem { Text = "QM", Value = "QM" });
            listDept.Add(new SelectListItem { Text = "FINANCE", Value = "FINANCE" });
            listDept.Add(new SelectListItem { Text = "PRODUCTION", Value = "PRODUCTION" });
            ViewData["Department_"] = listDept;

            //ViewData["department_"] = li6;



            if (!string.IsNullOrEmpty(Fullname))
            {
                byte[] EncryptPassword = Encrypt(Password);

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    Guid Id = Guid.NewGuid();
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                    cn.Open();
                    SqlCommand command;
                    command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[User] (Id,CreatedOn,Username,Password,Fullname,Email,Role,Department,MobileNo) values (@Id,@CreatedOn,@Username,@Password,@Fullname,@Email,@Role,@Department,@MobileNo)", cn);
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@CreatedOn", createdOn);

                    if (!string.IsNullOrEmpty(UserName))
                    {
                        command.Parameters.AddWithValue("@Username", UserName);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Username", DBNull.Value);
                        TempData["msg"] = "<script>alert(' Username is  is reqired.');</script>";
                        return RedirectToAction("RegisterStaff", "Home");

                    }


                    if (!string.IsNullOrEmpty(Password) && Password.Length >= 8)
                    {
                        command.Parameters.AddWithValue("@Password", EncryptPassword);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Password", DBNull.Value);
                        TempData["msg"] = "<script>alert('Password must 8 character. ');</script>";
                        return RedirectToAction("RegisterStaff", "Home");



                    }

                    if (!string.IsNullOrEmpty(Fullname))
                    {
                        command.Parameters.AddWithValue("@Fullname", Fullname);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Fullname", DBNull.Value);
                        TempData["msg"] = "<script>alert(' Full Name is  is reqired.');</script>";
                        return RedirectToAction("RegisterStaff", "Home");

                    }


                    if (!string.IsNullOrEmpty(Email))
                    {
                        command.Parameters.AddWithValue("@Email", Email);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Email", DBNull.Value);
                        TempData["msg"] = "<script>alert(' Email is  is reqired.');</script>";
                        return RedirectToAction("RegisterStaff", "Home");

                    }

                    if (!string.IsNullOrEmpty(Role))
                    {
                        command.Parameters.AddWithValue("@Role", Role);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Role", DBNull.Value);
                        TempData["msg"] = "<script>alert(' Role is  is reqired.');</script>";
                        return RedirectToAction("RegisterStaff", "Home");

                    }

                    if (!string.IsNullOrEmpty(Department))
                    {
                        command.Parameters.AddWithValue("@Department", Department);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Department", DBNull.Value);
                        TempData["msg"] = "<script>alert(' Department is  is reqired.');</script>";
                        return RedirectToAction("RegisterStaff", "Home");

                    }


                    if (!string.IsNullOrEmpty(MobileNo))
                    {
                        command.Parameters.AddWithValue("@MobileNo", MobileNo);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@MobileNo", DBNull.Value);

                    }
                    command.ExecuteNonQuery();
                    cn.Close();
                }

                return RedirectToAction("ManageStaff", "Home");
            }





        }
        catch (MembershipCreateUserException)
        {

        }

        return View();
    }

    public ActionResult ChangePassword(string Id, string set, string Password, string ConfirmPassword)
    {
        var IdentityName = @Session["Fullname"];
        var UserName = @Session["Username"];
        var Role = @Session["Role"];
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.Id = Id;

        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT Username,Fullname,Department FROM [IflowSeed].[dbo].[User]                               
                                     WHERE Id=@Id ";
            command.Parameters.AddWithValue("@Id", Id);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader.IsDBNull(0) == false)
                {
                    ViewBag.Username = reader.GetString(0);
                }
                if (reader.IsDBNull(1) == false)
                {
                    ViewBag.Fullname = reader.GetString(1);
                }
                if (reader.IsDBNull(2) == false)
                {
                    ViewBag.Department = reader.GetString(2);
                }
            }

            cn.Close();
        }

        if (set == "changepassword")
        {
            if (!string.IsNullOrEmpty(Password) && Password.Length >= 8 && !string.IsNullOrEmpty(ConfirmPassword) && Password == ConfirmPassword)
            {
                byte[] EncryptPassword = Encrypt(Password);
                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn2.Open();
                    SqlCommand command;
                    command = new SqlCommand("UPDATE [IflowSeed].[dbo].[User] SET Password=@Password WHERE Id=@Id", cn2);
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@Password", EncryptPassword);
                    command.ExecuteNonQuery();
                    cn2.Close();

                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                TempData["msg"] = "<script>alert('Password Not Sucessfully Changed');</script>";
            }
        }
        else
        {
            TempData["msg"] = "<script>alert('Password Not Sucessfully Changed');</script>";
        }
        return View();
    }

    public ActionResult AssignRole(string Id, string sts)
    {
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.IsRole = @Session["Role"];

        ViewBag.Id = Id;
        ViewBag.sts = sts;

        if (sts == "deleteuser")
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("DELETE [IflowSeed].[dbo].[User] WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn.Close();
            }
        }

        if (sts == "error")
        {
            ViewBag.IsError = "error";
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
        listrole.Add(new SelectListItem { Text = "Assistant Executive Account", Value = "Assistant Executive Account" });
        listrole.Add(new SelectListItem { Text = "Planner", Value = "Planner" });
        listrole.Add(new SelectListItem { Text = "Printing", Value = "Printing" });
        listrole.Add(new SelectListItem { Text = "Inserting", Value = "Inserting" });
        listrole.Add(new SelectListItem { Text = "MMP", Value = "MMP" });
        listrole.Add(new SelectListItem { Text = "Posting", Value = "Posting" });
        listrole.Add(new SelectListItem { Text = "Super Admin", Value = "Super Admin" });
        ViewData["RoleType"] = listrole;

        List<SelectListItem> listDept = new List<SelectListItem>();
        listDept.Add(new SelectListItem { Text = "Select", Value = "-" });
        listDept.Add(new SelectListItem { Text = "MBD", Value = "MBD" });
        listDept.Add(new SelectListItem { Text = "BSS", Value = "BSS" });
        listDept.Add(new SelectListItem { Text = "IT", Value = "IT" });
        listDept.Add(new SelectListItem { Text = "QM", Value = "QM" });
        listDept.Add(new SelectListItem { Text = "FINANCE", Value = "FINANCE" });
        listDept.Add(new SelectListItem { Text = "PRODUCTION", Value = "PRODUCTION" });
        ViewData["Department"] = listDept;

        if (Id != null && Id != "")
        {
            Guid Ids_ = new Guid(Id);

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Id,UserName,Fullname,Email,Role,Department,MobileNo FROM [IflowSeed].[dbo].[User]                                     
                                     WHERE Id=@Id";
                //command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = Id;
                command.Parameters.AddWithValue("@Id", Id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        ViewBag.Id = reader.GetGuid(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        ViewBag.Username = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        ViewBag.Fullname = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        ViewBag.Email = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        ViewBag.Role = reader.GetString(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        ViewBag.Department = reader.GetString(5);
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        ViewBag.MobileNo = reader.GetString(6);
                    }
                }
                cn.Close();
            }
        }

        //            ViewBag.Userlist = GetUser(null);
        return View();
    }

    public ActionResult ViewProfile(string Id, string sts, string UserName)
    {
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.IsRole = @Session["Role"];

        ViewBag.Id = @Session["Id"];
        ViewBag.sts = sts;
        ViewBag.UserName = @Session["UserName"];

        if (sts == "deleteuser")
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("DELETE [IflowSeed].[dbo].[User] WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn.Close();
            }
        }

        if (sts == "error")
        {
            ViewBag.IsError = "error";
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
        listrole.Add(new SelectListItem { Text = "Asst Account", Value = "Asst Account" });
        listrole.Add(new SelectListItem { Text = "Planner", Value = "Planner" });
        listrole.Add(new SelectListItem { Text = "Printing", Value = "Printing" });
        listrole.Add(new SelectListItem { Text = "Inserting", Value = "Inserting" });
        listrole.Add(new SelectListItem { Text = "MMP", Value = "MMP" });
        listrole.Add(new SelectListItem { Text = "Posting", Value = "Posting" });
        listrole.Add(new SelectListItem { Text = "Super Admin", Value = "Super Admin" });
        ViewData["role_"] = listrole;

        List<SelectListItem> listDept = new List<SelectListItem>();
        listDept.Add(new SelectListItem { Text = "Select", Value = "-" });
        listDept.Add(new SelectListItem { Text = "MBD", Value = "MBD" });
        listDept.Add(new SelectListItem { Text = "BSS", Value = "BSS" });
        listDept.Add(new SelectListItem { Text = "IT", Value = "IT" });
        listDept.Add(new SelectListItem { Text = "QM", Value = "QM" });
        listDept.Add(new SelectListItem { Text = "FINANCE", Value = "FINANCE" });
        listDept.Add(new SelectListItem { Text = "PRODUCTION", Value = "PRODUCTION" });
        ViewData["Department_"] = listDept;

        if (Id != null && Id != "")
        {
            Guid Ids_ = new Guid(Id);

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Id,UserName,Fullname,Email,Role,Department,MobileNo FROM [IflowSeed].[dbo].[User]                                     
                                     WHERE Id=@Id";
                //command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = Id;
                command.Parameters.AddWithValue("@Id", Id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        ViewBag.Id = reader.GetGuid(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        ViewBag.Username = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        ViewBag.Fullname = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        ViewBag.Email = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        ViewBag.Role = reader.GetString(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        ViewBag.Department = reader.GetString(5);
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        ViewBag.MobileNo = reader.GetString(6);
                    }
                }
                cn.Close();
            }
        }

        //            ViewBag.Userlist = GetUser(null);
        return View();
    }

    public ActionResult UpdateProfile2(string set, string Id, string UserName, string Password, string Confirmpwd, string Fullname, string Email, string MobileNo, string Role, string Department)
    {
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
        listrole.Add(new SelectListItem { Text = "Assistant Executive Account", Value = "Assistant Executive Account" });
        listrole.Add(new SelectListItem { Text = "Planner", Value = "Planner" });
        listrole.Add(new SelectListItem { Text = "Printing", Value = "Printing" });
        listrole.Add(new SelectListItem { Text = "Inserting", Value = "Inserting" });
        listrole.Add(new SelectListItem { Text = "MMP", Value = "MMP" });
        listrole.Add(new SelectListItem { Text = "Posting", Value = "Posting" });
        listrole.Add(new SelectListItem { Text = "Super Admin", Value = "Super Admin" });
        ViewData["role_"] = listrole;

        List<SelectListItem> listDept = new List<SelectListItem>();
        listDept.Add(new SelectListItem { Text = "Select", Value = "-" });
        listDept.Add(new SelectListItem { Text = "MBD", Value = "MBD" });
        listDept.Add(new SelectListItem { Text = "BSS", Value = "BSS" });
        listDept.Add(new SelectListItem { Text = "IT", Value = "IT" });
        listDept.Add(new SelectListItem { Text = "QM", Value = "QM" });
        listDept.Add(new SelectListItem { Text = "FINANCE", Value = "FINANCE" });
        listDept.Add(new SelectListItem { Text = "PRODUCTION", Value = "PRODUCTION" });
        ViewData["Department_"] = listDept;

        if (!string.IsNullOrEmpty(Id) && Fullname != null && Email != null && Role != "Please Select" && Department != null && MobileNo != null)
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("UPDATE [IflowSeed].[dbo].[User] SET Username=@Username,Fullname=@Fullname,Email=@Email,Role=@Role,Department=@Department,MobileNo=@MobileNo WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@Username", UserName.Trim());
                command.Parameters.AddWithValue("@Fullname", Fullname.Trim());
                command.Parameters.AddWithValue("@Email", Email.Trim());
                command.Parameters.AddWithValue("@Role", Role.Trim());
                command.Parameters.AddWithValue("@Department", Department.Trim());
                command.Parameters.AddWithValue("@MobileNo", MobileNo.Trim());
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn.Close();
            }
        }

        if (!string.IsNullOrEmpty(UserName))
        {

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Id,UserName,Fullname,Email,Role,Department,MobileNo FROM [IflowSeed].[dbo].[User]                                     
                                     WHERE UserName=@UserName";
                //command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = Id;
                command.Parameters.AddWithValue("@UserName", UserName);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        ViewBag.Id = reader.GetGuid(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        ViewBag.Username = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        ViewBag.Fullname = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        ViewBag.Email = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        ViewBag.Role = reader.GetString(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        ViewBag.Department = reader.GetString(5);
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        ViewBag.MobileNo = reader.GetString(6);
                    }
                }
                cn.Close();
            }
        }

        return View();
    }

    List<RegisterModel> viewModelList = new List<RegisterModel>();
    private MultiSelectList GetUser(string[] selectedValues)
    {
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT Id,Fullname FROM [IflowSeed].[dbo].[User]                                
                                     ORDER BY [Fullname] ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                RegisterModel model = new RegisterModel();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.Id = reader.GetGuid(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        model.Fullname = reader.GetString(1);
                    }
                }
                viewModelList.Add(model);
            }
            cn.Close();
        }
        return new MultiSelectList(viewModelList, "Id", "Fullname", selectedValues);
    }

    public ActionResult UpdatePassword(string Id, string set, string Password, string ConfirmPassword)
    {
        ViewBag.Id = Id;

        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT Username,Fullname,Department FROM [IflowSeed].[dbo].[User]                               
                                     WHERE Id=@Id ";
            command.Parameters.AddWithValue("@Id", Id);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader.IsDBNull(0) == false)
                {
                    ViewBag.Username = reader.GetString(0);
                }
                if (reader.IsDBNull(1) == false)
                {
                    ViewBag.Fullname = reader.GetString(1);
                }
                if (reader.IsDBNull(2) == false)
                {
                    ViewBag.Department = reader.GetString(2);
                }
            }

            cn.Close();
        }


        if (!string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(ConfirmPassword) && Password == ConfirmPassword && Password.Length >= 8)
        {
            byte[] EncryptPassword = Encrypt(Password);

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("UPDATE [IflowSeed].[dbo].[User] SET Password=@Password WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@Password", EncryptPassword);
                command.ExecuteNonQuery();
                cn.Close();
                return RedirectToAction("ManageStaff", "Home");
            }
        }

        return View();
    }


    public ActionResult UpdatePassword3month(string Id, string set, string Password, string ConfirmPassword)
    {
        ViewBag.Id = Id;

        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT Username,Fullname,Department FROM [IflowSeed].[dbo].[User]                               
                                     WHERE Id=@Id ";
            command.Parameters.AddWithValue("@Id", Id);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader.IsDBNull(0) == false)
                {
                    ViewBag.Username = reader.GetString(0);
                }
                if (reader.IsDBNull(1) == false)
                {
                    ViewBag.Fullname = reader.GetString(1);
                }
                if (reader.IsDBNull(2) == false)
                {
                    ViewBag.Department = reader.GetString(2);
                }
            }

            cn.Close();
        }


        if (!string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(ConfirmPassword) && Password == ConfirmPassword && Password.Length >= 8)
        {
            byte[] EncryptPassword = Encrypt(Password);

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                DateTime UpDate2 = DateTime.Now.AddMonths(3);
                string createdOn = UpDate2.ToString("yyyy-MM-dd hh:mm:ss tt");

                cn.Open();
                SqlCommand command;
                command = new SqlCommand("UPDATE [IflowSeed].[dbo].[User] SET Password=@Password, CreatedOn=@createdOn WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@Password", EncryptPassword);
                command.Parameters.AddWithValue("@createdOn", createdOn);
                command.ExecuteNonQuery();
                cn.Close();

                //WebSecurity.Logout();
                Session.RemoveAll();
                Session.Clear();
                return RedirectToAction("Index", "Home");
            }
        }

        return View();
    }


    public ActionResult DeleteUser(RegisterModel model)
    {
        if (model.Id != Guid.Empty)
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("DELETE [IflowSeed].[dbo].[User] WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@Id", model.Id);
                command.ExecuteNonQuery();
                cn.Close();
            }
        }
        return RedirectToAction("ManageStaff", "Home");
    }

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

    public ActionResult Index()
    {
        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];
        ViewBag.IsDepart = @Session["Department"];
        List<string> dataset = new List<string>();

        if (IdentityName == null || Role == null)
        {
            return RedirectToAction("Login", "Account");
        }
        else
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand comm = new SqlCommand("SELECT COUNT(*) FROM [IflowSeed].[dbo].[JobInstruction] WHERE (Status = 'New') OR (Status = 'Waiting to Assign Programmer') OR (Status = 'Development Complete') ", cn);
                Int32 count = (Int32)comm.ExecuteScalar();
                ViewBag.IsCreateJI = count.ToString();
                dataset.Add(count.ToString());
                cn.Close();
            }

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand comm = new SqlCommand("SELECT COUNT(*) FROM [IflowSeed].[dbo].[ProgDevWorksheet] WHERE Status='Under Development'", cn);
                comm.Parameters.AddWithValue("@Status", "Under Development");
                Int32 count = (Int32)comm.ExecuteScalar();
                ViewBag.IsUnderDevelopment = count.ToString();
                dataset.Add(count.ToString());
                cn.Close();
            }

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand comm = new SqlCommand("SELECT COUNT(DISTINCT(LogTagNo)) FROM [IflowSeed].[dbo].[JobAuditTrailDetail]  WHERE Status IN ('ITO','PROCESSING')", cn);
                SqlDataReader count = comm.ExecuteReader();

                if(count.HasRows)
                {
                    while(count.Read())
                    {
                        ViewBag.IsDailyJob = count.GetInt32(0);
                        dataset.Add(count.ToString());

                    }
                }
                else
                {
                    ViewBag.IsDailyJob = "0";
                    dataset.Add("0");

                }
                cn.Close();
            }

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand comm = new SqlCommand("SELECT COUNT(JobSheetNo) FROM [IflowSeed].[dbo].[JobInstruction] WHERE Status=@Status", cn);
                comm.Parameters.AddWithValue("@Status", "QME");
                Int32 count = (Int32)comm.ExecuteScalar();
                ViewBag.IsQM = count.ToString();
                dataset.Add(count.ToString());
                cn.Close();
            }

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand comm = new SqlCommand("SELECT COUNT(DISTINCT(LogTagNo)) FROM [IflowSeed].[dbo].[JobAuditTrailDetail] WHERE Status=@Status", cn);
                comm.Parameters.AddWithValue("@Status", "PLANNER");
                Int32 count = (Int32)comm.ExecuteScalar();
                ViewBag.IsPlanner = count.ToString();
                dataset.Add(count.ToString());
                cn.Close();
            }

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand comm = new SqlCommand("SELECT COUNT(DISTINCT(LogTagNo)) FROM [IflowSeed].[dbo].[JobAuditTrailDetail]WHERE Status=@Status ", cn);
                comm.Parameters.AddWithValue("@Status", "PRODUCTION");
                Int32 count = (Int32)comm.ExecuteScalar();
                ViewBag.IsPrinting = count.ToString();
                dataset.Add(count.ToString());
                cn.Close();
            }

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand comm = new SqlCommand("SELECT COUNT(DISTINCT(LogTagNo)) FROM [IflowSeed].[dbo].[JobAuditTrailDetail] WHERE Status=@Status", cn);
                comm.Parameters.AddWithValue("@Status", "INSERTING");
                Int32 count = (Int32)comm.ExecuteScalar();
                ViewBag.IsInserting = count.ToString();
                dataset.Add(count.ToString());
                cn.Close();
            }

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand comm = new SqlCommand("SELECT COUNT(DISTINCT(LogTagNo)) FROM [IflowSeed].[dbo].[JobAuditTrailDetail] WHERE Status=@Status", cn);
                comm.Parameters.AddWithValue("@Status", "MMP");
                Int32 count = (Int32)comm.ExecuteScalar();
                ViewBag.IsMMP = count.ToString();
                dataset.Add(count.ToString());
                cn.Close();
            }

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand comm = new SqlCommand("SELECT COUNT(DISTINCT(LogTagNo)) FROM [IflowSeed].[dbo].[JobAuditTrailDetail] WHERE Status=@Status", cn);
                comm.Parameters.AddWithValue("@Status", "SELFMAILER");
                Int32 count = (Int32)comm.ExecuteScalar();
                ViewBag.IsSM = count.ToString();
                dataset.Add(count.ToString());
                cn.Close();
            }

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand comm = new SqlCommand("SELECT COUNT(DISTINCT(LogTagNo)) FROM [IflowSeed].[dbo].[JobAuditTrailDetail] WHERE  Status=@Status", cn);
                comm.Parameters.AddWithValue("@Status", "POSTING");
                Int32 count = (Int32)comm.ExecuteScalar();
                ViewBag.IsPosting = count.ToString();
                dataset.Add(count.ToString());
                cn.Close();
            }

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand comm = new SqlCommand("SELECT COUNT(DISTINCT(LogTagNo)) FROM [IflowSeed].[dbo].[JobAuditTrailDetail] WHERE Status=@Status", cn);
                comm.Parameters.AddWithValue("@Status", "FINANCE");
                Int32 count = (Int32)comm.ExecuteScalar();
                ViewBag.IsFinance = count.ToString();
                dataset.Add(count.ToString());
                cn.Close();
            }

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand comm = new SqlCommand("SELECT COUNT(JobSheetNo) FROM [IflowSeed].[dbo].[JobInstruction]", cn);
                Int32 count = (Int32)comm.ExecuteScalar();
                ViewBag.IsTotal = count.ToString();
                dataset.Add(count.ToString());
                cn.Close();
            }


            //using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            //{
            //    cn.Open();
            //    SqlCommand comm = new SqlCommand("SELECT COUNT(*) FROM [IflowSeed].[dbo].[SchedulerJob]", cn);
            //    Int32 count = (Int32)comm.ExecuteScalar();
            //    ViewBag.IsSchedulerJob = count.ToString();
            //    dataset.Add(count.ToString());
            //    cn.Close();
            //}

            //using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            //{
            //    cn.Open();
            //    SqlCommand comm = new SqlCommand("SELECT COUNT(*) FROM [IflowSeed].[dbo].[JobAuditTrailDetail] WHERE Status=@Status", cn);
            //    comm.Parameters.AddWithValue("@Status", "PROCESSING");
            //    Int32 count = (Int32)comm.ExecuteScalar();
            //    ViewBag.IsWaitingApproval = count.ToString();
            //    dataset.Add(count.ToString());
            //    cn.Close();
            //}

            //using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            //{
            //    cn.Open();
            //    SqlCommand comm = new SqlCommand("SELECT COUNT(*) FROM [IflowSeed].[dbo].[JobInstruction] WHERE Status=@Status", cn);
            //    comm.Parameters.AddWithValue("@Status", "PRINT,INSERT AND RETURN");
            //    Int32 count = (Int32)comm.ExecuteScalar();
            //    ViewBag.IsPRINT = count.ToString();
            //    dataset.Add(count.ToString());
            //    cn.Close();
            //}

            return View();


        }


    }


    List<CustomerDetails> CustomerDetailslist = new List<CustomerDetails>();
    public ActionResult ManageCustomerDetails(string Id, string Customer_Name, string set)
    {
        if (set == "search") //ini kalu user search product
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Id, Customer_Name, AccountManager, Contact_Person, Address1, Cust_Phone, Cust_FaxNo, Cust_Mobile, Cust_Email, Cust_Web, Cust_Department
                                     FROM [IflowSeed].[dbo].[CustomerDetails]
                                     WHERE Customer_Name LIKE @Customer_Name";
                command.Parameters.AddWithValue("@Customer_Name", "%" + Customer_Name + "%");
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CustomerDetails model = new CustomerDetails();
                    {
                        model.Bil = _bil++;
                        if (reader.IsDBNull(0) == false)
                        {
                            model.Id = reader.GetGuid(0);
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            model.Customer_Name = reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            model.AccountManager = reader.GetString(2);
                        }
                        if (reader.IsDBNull(3) == false)
                        {
                            model.Contact_Person = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            model.Address1 = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            model.Cust_Phone = reader.GetString(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.Cust_FaxNo = reader.GetString(6);
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.Cust_Mobile = reader.GetString(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.Cust_Email = reader.GetString(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            model.Cust_Web = reader.GetString(9);
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            model.Cust_Department = reader.GetString(10);
                        }

                    }
                    CustomerDetailslist.Add(model);
                }
                cn.Close();
            }
        }
        else
        {
            //ALL firt masuk
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Id, Customer_Name, AccountManager, Contact_Person, Address1, Cust_Phone, Cust_FaxNo, Cust_Mobile, Cust_Email, Cust_Web, Cust_Department
                                       FROM [IflowSeed].[dbo].[CustomerDetails]
                                     ORDER BY Customer_Name";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CustomerDetails model = new CustomerDetails();
                    {
                        model.Bil = _bil++;
                        if (reader.IsDBNull(0) == false)
                        {
                            model.Id = reader.GetGuid(0);
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            model.Customer_Name = reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            model.AccountManager = reader.GetString(2);
                        }
                        if (reader.IsDBNull(3) == false)
                        {
                            model.Contact_Person = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            model.Address1 = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            model.Cust_Phone = reader.GetString(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.Cust_FaxNo = reader.GetString(6);
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.Cust_Mobile = reader.GetString(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.Cust_Email = reader.GetString(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            model.Cust_Web = reader.GetString(9);
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            model.Cust_Department = reader.GetString(10);
                        }

                    }
                    CustomerDetailslist.Add(model);
                }
                cn.Close();
            }
        }
        return View(CustomerDetailslist); //hntr data ke ui
    }

    public ActionResult CreateCustomerDetails(string Id, string Cust_Department, string Customer_Name, string AccountManager, string Contact_Person, string Address1, string Address2, string Address3, string Cust_Postcode, string Cust_State, string DeliveryAddress1, string DeliveryAddress2, string DeliveryAddress3, string DeliveryCust_Postcode, string DeliveryCust_State, string Cust_Phone, string Cust_FaxNo, string Cust_Mobile, string Cust_Email, string Cust_Web, string set, string ProductType)
    {
        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];
        ViewBag.IsDepart = @Session["Department"];
        //ViewBag.AccountManager = IdentityName.ToString();

        List<SelectListItem> listState = new List<SelectListItem>();

        listState.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listState.Add(new SelectListItem { Text = "KEDAH", Value = "KEDAH" });
        listState.Add(new SelectListItem { Text = "NEGERI SEMBILAN", Value = "NEGERI SEMBILAN" });
        listState.Add(new SelectListItem { Text = "PAHANG", Value = "PAHANG" });
        listState.Add(new SelectListItem { Text = "KELANTAN", Value = "KELANTAN" });
        listState.Add(new SelectListItem { Text = "JOHOR", Value = "JOHOR" });
        listState.Add(new SelectListItem { Text = "PERAK", Value = "PERAK" });
        listState.Add(new SelectListItem { Text = "PERLIS", Value = "PERLIS" });
        listState.Add(new SelectListItem { Text = "SELANGOR", Value = "SELANGOR" });
        listState.Add(new SelectListItem { Text = "SARAWAK", Value = "SARAWAK" });
        listState.Add(new SelectListItem { Text = "PULAU PINANG", Value = "PULAU PINANG" });
        listState.Add(new SelectListItem { Text = "SABAH", Value = "TERM" });
        listState.Add(new SelectListItem { Text = "MELAKA", Value = "MELAKA" });
        listState.Add(new SelectListItem { Text = "WILAYAH PERSEKUTUAN", Value = "WILAYAH PERSEKUTUAN" });
        listState.Add(new SelectListItem { Text = "TERENGGANU", Value = "TERENGGANU" });

        ViewData["State_"] = listState;


        List<SelectListItem> listproduct = new List<SelectListItem>();

        listproduct.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listproduct.Add(new SelectListItem { Text = "DDP", Value = "DDP" });
        listproduct.Add(new SelectListItem { Text = "RMS", Value = "RMS" });
        listproduct.Add(new SelectListItem { Text = "DOW", Value = "DOW" });
        listproduct.Add(new SelectListItem { Text = "OTHERS", Value = "OTHERS" });

        ViewData["ProductType_"] = listproduct;


        ViewBag.Id = Id;

        if (string.IsNullOrEmpty(Id))
        {
            ViewBag.DataSet = "Save";
        }
        else
        {
            ViewBag.DataSet = "update";
        }


        if (set == "Submit")
        {
            Debug.Write("success");
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                Guid Idx = Guid.NewGuid();
                string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                string ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                cn.Open();
                SqlCommand command;
                command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[CustomerDetails] (Id, CreatedOn,ModifiedOn, Customer_Name, AccountManager, Contact_Person, Address1, Address2, Address3, Cust_Postcode, Cust_State, DeliveryAddress1, DeliveryAddress2, DeliveryAddress3, DeliveryCust_Postcode, DeliveryCust_State, Cust_Phone, Cust_FaxNo, Cust_Mobile, Cust_Email, Cust_Web, CreateUser,ProductType,Cust_Department) values (@Id, @CreatedOn,@ModifiedOn, @Customer_Name, @AccountManager, @Contact_Person, @Address1, @Address2, @Address3, @Cust_Postcode, @Cust_State, @DeliveryAddress1, @DeliveryAddress2, @DeliveryAddress3, @DeliveryCust_Postcode, @DeliveryCust_State, @Cust_Phone, @Cust_FaxNo, @Cust_Mobile, @Cust_Email, @Cust_Web, @CreateUser,@ProductType,@Cust_Department)", cn);
                command.Parameters.AddWithValue("@Id", Idx);
                command.Parameters.AddWithValue("@CreatedOn", createdOn);
                command.Parameters.AddWithValue("@ModifiedOn", ModifiedOn);
                command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                command.Parameters.AddWithValue("@AccountManager", AccountManager);
                command.Parameters.AddWithValue("@Contact_Person", Contact_Person);
                command.Parameters.AddWithValue("@Address1", Address1);
                command.Parameters.AddWithValue("@Address2", Address2);
                command.Parameters.AddWithValue("@Address3", Address3);
                command.Parameters.AddWithValue("@Cust_Postcode", Cust_Postcode);
                command.Parameters.AddWithValue("@Cust_State", Cust_State);
                command.Parameters.AddWithValue("@DeliveryAddress1", DeliveryAddress1);
                command.Parameters.AddWithValue("@DeliveryAddress2", DeliveryAddress2);
                command.Parameters.AddWithValue("@DeliveryAddress3", DeliveryAddress3);
                command.Parameters.AddWithValue("@DeliveryCust_Postcode", DeliveryCust_Postcode);
                command.Parameters.AddWithValue("@DeliveryCust_State", DeliveryCust_State);
                command.Parameters.AddWithValue("@Cust_Phone", Cust_Phone);
                command.Parameters.AddWithValue("@Cust_FaxNo", Cust_FaxNo);
                command.Parameters.AddWithValue("@Cust_Mobile", Cust_Mobile);
                command.Parameters.AddWithValue("@Cust_Email", Cust_Email);
                command.Parameters.AddWithValue("@Cust_Web", Cust_Web);
                command.Parameters.AddWithValue("@CreateUser", IdentityName.ToString());
                command.Parameters.AddWithValue("@ProductType", ProductType);
                command.Parameters.AddWithValue("@Cust_Department", Cust_Department);

                command.ExecuteNonQuery();
                cn.Close();
            }

            Debug.WriteLine("Fail");
            //bila save akn gi kt managecustomer
            return RedirectToAction("ManageCustomerDetails", "Home");
        }

        if (!string.IsNullOrEmpty(Id) && set == "update")
        {
            //update
            string ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("UPDATE [IflowSeed].[dbo].[CustomerDetails]  SET Cust_Department=@Cust_Department, ModifiedOn=@ModifiedOn, Customer_Name=@Customer_Name, AccountManager=@AccountManager, Contact_Person=@Contact_Person,Address1=@Address1, Address2=@Address2,Address3=@Address3, Cust_Postcode=@Cust_Postcode,Cust_State=@Cust_State, DeliveryAddress1=@DeliveryAddress1,DeliveryAddress2=@DeliveryAddress2, DeliveryAddress3=@DeliveryAddress3,DeliveryCust_Postcode=@DeliveryCust_Postcode, DeliveryCust_State=@DeliveryCust_State,Cust_Phone=@Cust_Phone, Cust_FaxNo=@Cust_FaxNo, Cust_Mobile=@Cust_Mobile,Cust_Email=@Cust_Email, Cust_Web=@Cust_Web, CreateUser=@CreateUser,ProductType=@ProductType WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@ModifiedOn", ModifiedOn);
                command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                command.Parameters.AddWithValue("@AccountManager", AccountManager);
                command.Parameters.AddWithValue("@Contact_Person", Contact_Person);
                command.Parameters.AddWithValue("@Address1", Address1);
                command.Parameters.AddWithValue("@Address2", Address2);
                command.Parameters.AddWithValue("@Address3", Address3);
                command.Parameters.AddWithValue("@Cust_Postcode", Cust_Postcode);
                command.Parameters.AddWithValue("@Cust_State", Cust_State);
                command.Parameters.AddWithValue("@DeliveryAddress1", DeliveryAddress1);
                command.Parameters.AddWithValue("@DeliveryAddress2", DeliveryAddress2);
                command.Parameters.AddWithValue("@DeliveryAddress3", DeliveryAddress3);
                command.Parameters.AddWithValue("@DeliveryCust_Postcode", DeliveryCust_Postcode);
                command.Parameters.AddWithValue("@DeliveryCust_State", DeliveryCust_State);
                command.Parameters.AddWithValue("@Cust_Phone", Cust_Phone);
                command.Parameters.AddWithValue("@Cust_FaxNo", Cust_FaxNo);
                command.Parameters.AddWithValue("@Cust_Mobile", Cust_Mobile);
                command.Parameters.AddWithValue("@Cust_Email", Cust_Email);
                command.Parameters.AddWithValue("@Cust_Web", Cust_Web);
                command.Parameters.AddWithValue("@CreateUser", IdentityName.ToString());
                command.Parameters.AddWithValue("@ProductType", ProductType);
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@Cust_Department", Cust_Department);

                command.ExecuteNonQuery();
                cn.Close();
            }
        }

        if (!string.IsNullOrEmpty(Id))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Id, Customer_Name, AccountManager, Contact_Person, Address1, Address2, Address3, Cust_Postcode, Cust_State, DeliveryAddress1, DeliveryAddress2, DeliveryAddress3, DeliveryCust_Postcode, DeliveryCust_State, Cust_Phone, Cust_FaxNo, Cust_Mobile, Cust_Email, Cust_Web, Cust_Department
                                       FROM [IflowSeed].[dbo].[CustomerDetails]                              
                                     WHERE Id=@Id";
                command.Parameters.AddWithValue("@Id", Id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        ViewBag.Id = reader.GetGuid(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        ViewBag.Customer_Name = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        ViewBag.AccountManager = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        ViewBag.Contact_Person = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        ViewBag.Address1 = reader.GetString(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        ViewBag.Address2 = reader.GetString(5);
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        ViewBag.Address3 = reader.GetString(6);
                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        ViewBag.Cust_Postcode = reader.GetString(7);
                    }
                    if (reader.IsDBNull(8) == false)
                    {
                        ViewBag.Cust_State = reader.GetString(8);
                    }
                    if (reader.IsDBNull(9) == false)
                    {
                        ViewBag.DeliveryAddress1 = reader.GetString(9);
                    }
                    if (reader.IsDBNull(10) == false)
                    {
                        ViewBag.DeliveryAddress2 = reader.GetString(10);
                    }
                    if (reader.IsDBNull(11) == false)
                    {
                        ViewBag.DeliveryAddress3 = reader.GetString(11);
                    }
                    if (reader.IsDBNull(12) == false)
                    {
                        ViewBag.DeliveryCust_Postcode = reader.GetString(12);
                    }
                    if (reader.IsDBNull(13) == false)
                    {
                        ViewBag.DeliveryCust_State = reader.GetString(13);
                    }
                    if (reader.IsDBNull(14) == false)
                    {
                        ViewBag.Cust_Phone = reader.GetString(14);
                    }
                    if (reader.IsDBNull(15) == false)
                    {
                        ViewBag.Cust_FaxNo = reader.GetString(15);
                    }
                    if (reader.IsDBNull(16) == false)
                    {
                        ViewBag.Cust_Mobile = reader.GetString(16);
                    }
                    if (reader.IsDBNull(17) == false)
                    {
                        ViewBag.Cust_Email = reader.GetString(17);
                    }
                    if (reader.IsDBNull(18) == false)
                    {
                        ViewBag.Cust_Web = reader.GetString(18);
                    }
                    if (reader.IsDBNull(19) == false)
                    {
                        ViewBag.Cust_Department = reader.GetString(19);
                    }
                }
                cn.Close();
            }
        }



        return View();
    }

    public ActionResult DeleteCustomerDetails(string Id)
    {
        if (!string.IsNullOrEmpty(Id))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("DELETE [IflowSeed].[dbo].[CustomerDetails] WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn.Close();
            }
        }
        return RedirectToAction("ManageCustomerDetails", "Home");
    }

    List<CustomerProduct> CustomerProductlist = new List<CustomerProduct>();
    public ActionResult ManageProduct(string Id, string Customer_Name, string set)
    {
        if (set == "search") //ini kalu user search product
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Id,Customer_Name,ProductName,AccountManager,CreateUser
                                     FROM [IflowSeed].[dbo].[CustomerProduct]
                                     WHERE Customer_Name LIKE @Customer_Name";
                command.Parameters.AddWithValue("@Customer_Name", "%" + Customer_Name + "%");
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CustomerProduct model = new CustomerProduct();
                    {
                        model.Bil = _bil++;
                        if (reader.IsDBNull(0) == false)
                        {
                            model.Id = reader.GetGuid(0);
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            model.Customer_Name = reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            model.ProductName = reader.GetString(2);
                        }
                        if (reader.IsDBNull(3) == false)
                        {
                            model.AccountManager = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            model.CreateUser = reader.GetString(4);
                        }

                    }
                    CustomerProductlist.Add(model);
                }
                cn.Close();
            }
        }
        else
        {
            //ALL firt masuk
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Id,Customer_Name,ProductName,AccountManager,CreateUser
                                       FROM [IflowSeed].[dbo].[CustomerProduct]
                                     ORDER BY Customer_Name";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CustomerProduct model = new CustomerProduct();
                    {
                        model.Bil = _bil++;
                        if (reader.IsDBNull(0) == false)
                        {
                            model.Id = reader.GetGuid(0);
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            model.Customer_Name = reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            model.ProductName = reader.GetString(2);
                        }
                        if (reader.IsDBNull(3) == false)
                        {
                            model.AccountManager = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            model.CreateUser = reader.GetString(4);
                        }

                    }
                    CustomerProductlist.Add(model);
                }
                cn.Close();
            }
        }
        return View(CustomerProductlist); //hntr data ke ui
    }

    public ActionResult CreateNewProduct(string Id, string Customer_Name, string ProductName, string AccountManager, string CreateUser, string set)
    {

        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];


        ViewBag.IsDepart = @Session["Department"];
        ViewBag.AccountManager = IdentityName.ToString();

        int _bil = 1;
        List<SelectListItem> li = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Customer_Name FROM [IflowSeed].[dbo].[CustomerDetails]                          
                                     ORDER BY Customer_Name";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                CustomerProduct model = new CustomerProduct();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.Customer_Name = reader.GetString(0);
                    }
                }
                int i = _bil++;
                if (i == 1)
                {
                    li.Add(new SelectListItem { Text = "Please Select" });
                }
                li.Add(new SelectListItem { Text = model.Customer_Name });
            }
            cn.Close();
        }
        ViewData["Customer_"] = li;



        ViewBag.Customer_Name = Customer_Name;
        ViewBag.Id = Id;

        if (string.IsNullOrEmpty(Id))
        {
            ViewBag.DataSet = "Save";
        }
        else
        {
            ViewBag.DataSet = "update";
        }


        if (string.IsNullOrEmpty(Id) && Customer_Name != "Please Select" && !string.IsNullOrEmpty(Customer_Name) && !string.IsNullOrEmpty(ProductName))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                Guid Idx = Guid.NewGuid();
                string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                string ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                cn.Open();
                SqlCommand command;
                command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[CustomerProduct] (Id, CreatedOn, ModifiedOn, Customer_Name, ProductName, AccountManager, CreateUser) values (@Id, @CreatedOn,@ModifiedOn, @Customer_Name, @ProductName, @AccountManager,  @CreateUser)", cn);
                command.Parameters.AddWithValue("@Id", Idx);
                command.Parameters.AddWithValue("@CreatedOn", createdOn);
                command.Parameters.AddWithValue("@ModifiedOn", ModifiedOn);
                command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                command.Parameters.AddWithValue("@ProductName", ProductName.ToUpper());
                command.Parameters.AddWithValue("@AccountManager", AccountManager);
                command.Parameters.AddWithValue("@CreateUser", IdentityName.ToString());
                command.ExecuteNonQuery();
                cn.Close();
            }
            //bila save akn gi kt managecustomer
            return RedirectToAction("ManageProduct", "Home");
        }

        if (!string.IsNullOrEmpty(Id) && set == "update")
        {
            //update
            string ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("UPDATE [IflowSeed].[dbo].[CustomerProduct]  SET ModifiedOn=@ModifiedOn, Customer_Name=@Customer_Name, ProductName=@ProductName, AccountManager=@AccountManager, CreateUser=@CreateUser WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@ModifiedOn", ModifiedOn);
                command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                command.Parameters.AddWithValue("@ProductName", ProductName.ToUpper());
                command.Parameters.AddWithValue("@AccountManager", AccountManager);
                command.Parameters.AddWithValue("@CreateUser", IdentityName.ToString());
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn.Close();
            }
        }

        if (!string.IsNullOrEmpty(Id))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Id, Customer_Name, ProductName, AccountManager
                                       FROM [IflowSeed].[dbo].[CustomerProduct]                              
                                     WHERE Id=@Id";
                command.Parameters.AddWithValue("@Id", Id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        ViewBag.Id = reader.GetGuid(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        ViewBag.Customer_Name = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        ViewBag.ProductName = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        ViewBag.AccountManager = reader.GetString(3);
                    }

                }
                cn.Close();
            }
        }



        return View();
    }

    public ActionResult DeleteProduct(string Id)
    {
        if (!string.IsNullOrEmpty(Id))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("DELETE [IflowSeed].[dbo].[CustomerProduct] WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn.Close();
            }
        }
        return RedirectToAction("ManageProduct", "Home");
    }


    List<PaperInfo> PaperInfolist = new List<PaperInfo>();
    public ActionResult ManagePaperType(string Id, string TypeCode, string set)
    {
        if (set == "search") //ini kalu user search product
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Id,TypeCode,Paper
                                     FROM [IflowSeed].[dbo].[PaperInfo]
                                     WHERE TypeCode LIKE @TypeCode";
                command.Parameters.AddWithValue("@TypeCode", "%" + TypeCode + "%");
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    PaperInfo model = new PaperInfo();
                    {
                        model.Bil = _bil++;
                        if (reader.IsDBNull(0) == false)
                        {
                            model.Id = reader.GetGuid(0);
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            model.TypeCode = reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            model.Paper = reader.GetString(2);
                        }

                    }
                    PaperInfolist.Add(model);
                }
                cn.Close();
            }
        }
        else
        {
            //ALL firt masuk
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Id,TypeCode,Paper
                                       FROM [IflowSeed].[dbo].[PaperInfo]
                                     ORDER BY TypeCode";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    PaperInfo model = new PaperInfo();
                    {
                        model.Bil = _bil++;
                        if (reader.IsDBNull(0) == false)
                        {
                            model.Id = reader.GetGuid(0);
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            model.TypeCode = reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            model.Paper = reader.GetString(2);
                        }

                    }
                    PaperInfolist.Add(model);
                }
                cn.Close();
            }
        }
        return View(PaperInfolist); //hntr data ke ui
    }

    public ActionResult CreateNewPaperType(string Id, string TypeCode, string Paper, string set)
    {

        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];
        ViewBag.IsDepart = @Session["Department"];

        List<SelectListItem> listTypeCode = new List<SelectListItem>();

        listTypeCode.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listTypeCode.Add(new SelectListItem { Text = "CS", Value = "CS" });
        listTypeCode.Add(new SelectListItem { Text = "CF", Value = "CF" });

        ViewData["TypeCode_"] = listTypeCode;


        ViewBag.Id = Id;

        if (string.IsNullOrEmpty(Id))
        {
            ViewBag.DataSet = "Save";
        }
        else
        {
            ViewBag.DataSet = "update";
        }


        if (string.IsNullOrEmpty(Id) && TypeCode != "Please Select" && !string.IsNullOrEmpty(Paper) && !string.IsNullOrEmpty(TypeCode))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                Guid Idx = Guid.NewGuid();
                string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                string ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                cn.Open();
                SqlCommand command;
                command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[PaperInfo] (Id, CreatedOn, ModifiedOn, TypeCode, Paper) values (@Id, @CreatedOn,@ModifiedOn,  @TypeCode, @Paper)", cn);
                command.Parameters.AddWithValue("@Id", Idx);
                command.Parameters.AddWithValue("@CreatedOn", createdOn);
                command.Parameters.AddWithValue("@ModifiedOn", ModifiedOn);
                command.Parameters.AddWithValue("@TypeCode", TypeCode);
                command.Parameters.AddWithValue("@Paper", Paper);
                command.ExecuteNonQuery();
                cn.Close();
            }
            //bila save akn gi kt managecustomer
            return RedirectToAction("ManagePaperType", "Home");
        }

        if (!string.IsNullOrEmpty(Id) && set == "update")
        {
            //update
            string ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("UPDATE [IflowSeed].[dbo].[PaperInfo]  SET ModifiedOn=@ModifiedOn, TypeCode=@TypeCode, Paper=@Paper WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@ModifiedOn", ModifiedOn);
                command.Parameters.AddWithValue("@TypeCode", TypeCode);
                command.Parameters.AddWithValue("@Paper", Paper);
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn.Close();
            }
        }

        if (!string.IsNullOrEmpty(Id))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Id, TypeCode, Paper
                                       FROM [IflowSeed].[dbo].[PaperInfo]                              
                                     WHERE Id=@Id";
                command.Parameters.AddWithValue("@Id", Id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        ViewBag.Id = reader.GetGuid(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        ViewBag.TypeCode = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        ViewBag.Paper = reader.GetString(2);
                    }

                }
                cn.Close();
            }
        }

        return View();
    }

    public ActionResult DeletePaperType(string Id)
    {
        if (!string.IsNullOrEmpty(Id))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("DELETE [IflowSeed].[dbo].[PaperInfo] WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn.Close();
            }
        }
        return RedirectToAction("ManagePaperType", "Home");
    }

    List<AddProgrammeSystem> listProgramme = new List<AddProgrammeSystem>();
    public ActionResult ManagelistProgramme(string Id, string Code, string Name, string set)
    {
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {

            int _bil = 1;
            cn.Open();

            if (set == "search")
            {
                command.CommandText = @"SELECT Id,Code,Name
                                       FROM [IflowSeed].[dbo].[AddProgrammeSystem]
                                        WHERE Code LIKE @Code ORDER BY CreatedOn desc";
                command.Parameters.AddWithValue("@Code", "%" + Code + "%");
            }

            else
            {
                command.CommandText = @"SELECT Id,Code,Name
                                       FROM [IflowSeed].[dbo].[AddProgrammeSystem]
                                       ORDER BY Name";
            }

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                AddProgrammeSystem model = new AddProgrammeSystem();
                {
                    model.Bil = _bil++;
                    if (reader.IsDBNull(0) == false)
                    {
                        model.Id = reader.GetGuid(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        model.Code = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        model.Name = reader.GetString(2);
                    }

                }
                listProgramme.Add(model);
            }
            cn.Close();
        }

        return View(listProgramme); //hntr data ke ui
    }


    public ActionResult AddNewProgramme(string Id, string Code, string Name, string set)
    {

        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];


        ViewBag.IsDepart = @Session["Department"];
        ViewBag.AccountManager = IdentityName.ToString();

        if (string.IsNullOrEmpty(Id))
        {
            ViewBag.DataSet = "Save";
        }
        else
        {
            ViewBag.DataSet = "update";
        }

        if (string.IsNullOrEmpty(Id) && Code != "Please Select" && Name != "Please Select" && !string.IsNullOrEmpty(Code) && !string.IsNullOrEmpty(Name))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                Guid Idx = Guid.NewGuid();
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[AddProgrammeSystem] (Id, Code, Name) values (@Id, @Code, @Name)", cn);
                command.Parameters.AddWithValue("@Id", Idx);
                command.Parameters.AddWithValue("@Code", Code);
                command.Parameters.AddWithValue("@Name", Name);
                command.ExecuteNonQuery();
                cn.Close();
            }

            return RedirectToAction("ManagelistProgramme", "Home");
        }

        if (!string.IsNullOrEmpty(Id) && set == "update")
        {
            //update
            string ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("UPDATE [IflowSeed].[dbo].[AddProgrammeSystem]  SET ModifiedOn=@ModifiedOn, Code=@Code, Name=@Name WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@ModifiedOn", ModifiedOn);
                command.Parameters.AddWithValue("@Code", Code);
                command.Parameters.AddWithValue("@Name", Name);
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn.Close();
            }
        }


        if (!string.IsNullOrEmpty(Id))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Id, Code, Name
                                       FROM [IflowSeed].[dbo].[AddProgrammeSystem]                              
                                       WHERE Id=@Id";
                command.Parameters.AddWithValue("@Id", Id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        ViewBag.Id = reader.GetGuid(0);
                    }

                    if (reader.IsDBNull(1) == false)
                    {
                        ViewBag.Code = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        ViewBag.Name = reader.GetString(2);
                    }

                }
                cn.Close();
            }
        }
        return View(listProgramme);
    }

    List<DetailsProgram> AddProgrammeSystemlist = new List<DetailsProgram>();
    public ActionResult ManageProgrammeList(string Id, string set, string Name)
    {

        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT Id,Customer_Name,ProductName,Code,Name                                         
                                        FROM [IflowSeed].[dbo].[DetailsProgram]
                                        ORDER BY Code asc ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                DetailsProgram model = new DetailsProgram();
                {
                    model.Bil = _bil++;
                    if (reader.IsDBNull(0) == false)
                    {
                        model.Id = reader.GetGuid(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        model.Customer_Name = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        model.ProductName = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        model.Code = reader.GetString(3);
                    }

                    if (reader.IsDBNull(4) == false)
                    {
                        model.Name = reader.GetString(4);
                    }
                }
                AddProgrammeSystemlist.Add(model);
            }
            cn.Close();
        }
        return View(AddProgrammeSystemlist);
    }



    public ActionResult DeleteProgrammeList(string Id)
    {
        if (!string.IsNullOrEmpty(Id))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("DELETE [IflowSeed].[dbo].[AddProgrammeSystem] WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn.Close();
            }
        }
        return RedirectToAction("ManagelistProgramme", "Home");
    }


    public ActionResult AddDetails(string Id, string Code, string Name, string Customer_Name, string ProductName, string Details, string AddProgrammeSystemId, string set)
    {

        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];


        ViewBag.IsDepart = @Session["Department"];
        ViewBag.AccountManager = IdentityName.ToString();

        if (string.IsNullOrEmpty(Id))
        {
            ViewBag.DataSet = "Save";
        }
        else
        {
            ViewBag.DataSet = "update";
        }


        int _bil = 1;
        List<SelectListItem> li = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT Customer_Name FROM [IflowSeed].[dbo].[CustomerProduct]                          
                                     ORDER BY Customer_Name";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                CustomerProduct model = new CustomerProduct();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.Customer_Name = reader.GetString(0);
                    }
                }
                int i = _bil++;
                if (i == 1)
                {
                    li.Add(new SelectListItem { Text = "Please Select" });
                }
                li.Add(new SelectListItem { Text = model.Customer_Name });
            }
            cn.Close();
        }
        ViewData["Customer_"] = li;


        int _bil2 = 1;
        List<SelectListItem> li2 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT ProductName FROM [IflowSeed].[dbo].[CustomerProduct]    
                                         ORDER BY ProductName";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                CustomerProduct model = new CustomerProduct();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.ProductName = reader.GetString(0);
                    }
                }
                int i = _bil2++;
                if (i == 1)
                {
                    li2.Add(new SelectListItem { Text = "Please Select" });
                }
                li2.Add(new SelectListItem { Text = model.ProductName });
            }
            cn.Close();
        }
        ViewData["Product_"] = li2;



        int _bil3 = 1;
        List<SelectListItem> li3 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT Name FROM [IflowSeed].[dbo].[AddProgrammeSystem]                          
                                     ORDER BY Name";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                AddProgrammeSystem model = new AddProgrammeSystem();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.Name = reader.GetString(0);
                    }
                }
                int i = _bil3++;
                if (i == 1)
                {
                    li3.Add(new SelectListItem { Text = "Please Select" });
                }
                li3.Add(new SelectListItem { Text = model.Name });
            }
            cn.Close();
        }
        ViewData["Name_"] = li3;


        if (string.IsNullOrEmpty(Id) && Name != "Please Select" && Customer_Name != "Please Select" && ProductName != "Please Select" && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Customer_Name) && !string.IsNullOrEmpty(ProductName) && !string.IsNullOrEmpty(Details))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                Guid Idx = Guid.NewGuid();
                string CreatedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[DetailsProgram] (Id,CreatedOn,Name,Customer_Name,ProductName,Details) values (@Id,@CreatedOn,@Name,@Customer_Name,@ProductName,@Details)", cn);
                command.Parameters.AddWithValue("@Id", Idx);
                command.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                command.Parameters.AddWithValue("@Name", Name);
                command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                command.Parameters.AddWithValue("@ProductName", ProductName);
                command.Parameters.AddWithValue("@Details", Details);
                command.ExecuteNonQuery();
                cn.Close();
            }

            return RedirectToAction("ManagelistProgramme", "Home");
        }

        if (!string.IsNullOrEmpty(Id) && set == "update")
        {
            //update
            string ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("UPDATE [IflowSeed].[dbo].[DetailsProgram]  SET ModifiedOn=@ModifiedOn, Name=@Name, Customer_Name=@Customer_Name,ProductName=@ProductName,Details=@Details WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@ModifiedOn", ModifiedOn);
                command.Parameters.AddWithValue("@Name", Name);
                command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                command.Parameters.AddWithValue("@ProductName", ProductName);
                command.Parameters.AddWithValue("@Details", Details);
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn.Close();
            }
        }

        if (!string.IsNullOrEmpty(Id))
        {

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Id,Name,Customer_Name,ProductName,Details
                                       FROM [IflowSeed].[dbo].[DetailsProgram]                              
                                     WHERE Id=@Id";
                command.Parameters.AddWithValue("@Id", Id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        ViewBag.Id = reader.GetGuid(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        ViewBag.Name = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        ViewBag.Customer_Name = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        ViewBag.ProductName = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        ViewBag.Details = reader.GetString(4);
                    }
                }
                cn.Close();
            }
        }

        return View();


    }


    List<PrintingType> PrintingTypelist = new List<PrintingType>();
    public ActionResult ManagePrintingType(string Id, string set, string Name)
    {

        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT Id,Code,Name                                         
                                        FROM [IflowSeed].[dbo].[PrintingType]
                                        ORDER BY Code asc ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                PrintingType model = new PrintingType();
                {
                    model.Bil = _bil++;
                    if (reader.IsDBNull(0) == false)
                    {
                        model.Id = reader.GetGuid(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        model.Code = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        model.Name = reader.GetString(2);
                    }

                }
                PrintingTypelist.Add(model);
            }
            cn.Close();
        }
        return View(PrintingTypelist);


    }

    public ActionResult DeletePrintingTypelist(string Id)
    {
        if (!string.IsNullOrEmpty(Id))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("DELETE [IflowSeed].[dbo].[PrintingType] WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn.Close();
            }
        }
        return RedirectToAction("ManagePrintingType", "Home");
    }


    public ActionResult AddPrintingType(string Id, string Code, string Name, string set)
    {

        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];


        ViewBag.IsDepart = @Session["Department"];
        ViewBag.AccountManager = IdentityName.ToString();

        if (string.IsNullOrEmpty(Id))
        {
            ViewBag.DataSet = "Save";
        }
        else
        {
            ViewBag.DataSet = "update";
        }



        if (string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Code) && !string.IsNullOrEmpty(Name))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                Guid Idx = Guid.NewGuid();
                string CreatedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[PrintingType] (Id,CreatedOn,Code,Name) values (@Id,@CreatedOn,@Code,@Name)", cn);
                command.Parameters.AddWithValue("@Id", Idx);
                command.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                command.Parameters.AddWithValue("@Code", Code);
                command.Parameters.AddWithValue("@Name", Name);
                command.ExecuteNonQuery();
                cn.Close();
            }

            return RedirectToAction("ManagePrintingType", "Home");
        }

        if (!string.IsNullOrEmpty(Id) && set == "update")
        {
            //update
            string ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("UPDATE [IflowSeed].[dbo].[PrintingType]  SET ModifiedOn=@ModifiedOn, Code=@Code, Name=@Name WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@ModifiedOn", ModifiedOn);
                command.Parameters.AddWithValue("@Code", Code);
                command.Parameters.AddWithValue("@Name", Name);
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn.Close();
            }
        }

        if (!string.IsNullOrEmpty(Id))
        {

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Id,Code,Name
                                       FROM [IflowSeed].[dbo].[PrintingType]                              
                                     WHERE Id=@Id";
                command.Parameters.AddWithValue("@Id", Id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        ViewBag.Id = reader.GetGuid(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        ViewBag.Code = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        ViewBag.Name = reader.GetString(2);
                    }

                }
                cn.Close();
            }
        }

        return View();
    }

    List<MaterialCharges> MaterialChargeslist = new List<MaterialCharges>();
    public ActionResult ManageMaterialCharges(string Id, string set, string Name)
    {

        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT Id,ItemCode,MaterialType,Rate,Description,CreatedBy                                         
                                        FROM [IflowSeed].[dbo].[MaterialCharges]
                                        ORDER BY ItemCode asc ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                MaterialCharges model = new MaterialCharges();
                {
                    model.Bil = _bil++;
                    if (reader.IsDBNull(0) == false)
                    {
                        model.Id = reader.GetGuid(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        model.ItemCode = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        model.MaterialType = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        model.Rate = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        model.Description = reader.GetString(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        model.CreatedBy = reader.GetString(5);
                    }

                }
                MaterialChargeslist.Add(model);
            }
            cn.Close();
        }

        return View(MaterialChargeslist);

    }

    public ActionResult DeleteMaterialChargeslist(string Id)
    {
        if (!string.IsNullOrEmpty(Id))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("DELETE [IflowSeed].[dbo].[MaterialCharges] WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn.Close();
            }
        }
        return RedirectToAction("ManageMaterialCharges", "Home");
    }


    public ActionResult AddMaterialCharges(string Id, string ItemCode, string MaterialType, string Rate, string Description, string CreatedBy, string set)
    {

        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];


        ViewBag.IsDepart = @Session["Department"];
        ViewBag.AccountManager = IdentityName.ToString();

        if (string.IsNullOrEmpty(Id))
        {
            ViewBag.DataSet = "Save";
        }
        else
        {
            ViewBag.DataSet = "update";
        }

        List<SelectListItem> listMaterialType = new List<SelectListItem>();

        listMaterialType.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listMaterialType.Add(new SelectListItem { Text = "ENVELOPE", Value = "ENVELOPE" });
        listMaterialType.Add(new SelectListItem { Text = "PAPER", Value = "PAPER" });

        ViewData["MaterialType_"] = listMaterialType;


        if (string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(ItemCode) && !string.IsNullOrEmpty(MaterialType) && !string.IsNullOrEmpty(Rate) && !string.IsNullOrEmpty(Description))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                Guid Idx = Guid.NewGuid();
                string CreatedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[MaterialCharges] (Id,ItemCode,MaterialType,Rate,Description,CreatedBy) values (@Id,@ItemCode,@MaterialType,@Rate,@Description,@CreatedBy)", cn);
                command.Parameters.AddWithValue("@Id", Idx);
                command.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                command.Parameters.AddWithValue("@ItemCode", ItemCode);
                command.Parameters.AddWithValue("@MaterialType", MaterialType);
                command.Parameters.AddWithValue("@Rate", Rate);
                command.Parameters.AddWithValue("@Description", Description);
                command.Parameters.AddWithValue("@CreatedBy", IdentityName.ToString());
                command.ExecuteNonQuery();
                cn.Close();
            }

            return RedirectToAction("ManageMaterialCharges", "Home");
        }

        if (!string.IsNullOrEmpty(Id) && set == "update")
        {
            //update
            string ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("UPDATE [IflowSeed].[dbo].[MaterialCharges]  SET ModifiedOn=@ModifiedOn, ItemCode=@ItemCode, MaterialType=@MaterialType, Rate=@Rate, Description=@Description WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@ModifiedOn", ModifiedOn);
                command.Parameters.AddWithValue("@ItemCode", ItemCode);
                command.Parameters.AddWithValue("@MaterialType", MaterialType);
                command.Parameters.AddWithValue("@Rate", Rate);
                command.Parameters.AddWithValue("@Description", Description);
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn.Close();
            }
        }

        if (!string.IsNullOrEmpty(Id))
        {

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Id,ItemCode,MaterialType,Rate,Description
                                       FROM [IflowSeed].[dbo].[MaterialCharges]                              
                                     WHERE Id=@Id";
                command.Parameters.AddWithValue("@Id", Id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        ViewBag.Id = reader.GetGuid(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        ViewBag.ItemCode = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        ViewBag.MaterialType = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        ViewBag.Rate = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        ViewBag.Description = reader.GetString(4);
                    }

                }
                cn.Close();
            }
        }

        return View();
    }

    public ActionResult ChartData()
    {
        List<string> dataset = new List<string>();

        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand comm = new SqlCommand("SELECT COUNT(*) FROM [IflowSeed].[dbo].[JobInstruction] WHERE (Status = 'New') OR (Status = 'Waiting to Assign Programmer') OR (Status = 'Development Complete') ", cn);
                Int32 count = (Int32)comm.ExecuteScalar();
                ViewBag.IsCreateJI = count.ToString();
                dataset.Add(count.ToString());
                cn.Close();
            }

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand comm = new SqlCommand("SELECT COUNT(*) FROM [IflowSeed].[dbo].[ProgDevWorksheet] WHERE Status='Under Development'", cn);
                comm.Parameters.AddWithValue("@Status", "Under Development");
                Int32 count = (Int32)comm.ExecuteScalar();
                ViewBag.IsUnderDevelopment = count.ToString();
                dataset.Add(count.ToString());
                cn.Close();
            }

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand comm = new SqlCommand("SELECT COUNT(DISTINCT(LogTagNo)) FROM [IflowSeed].[dbo].[JobAuditTrailDetail]  WHERE Status IN ('ITO','PROCESSING')", cn);
                SqlDataReader count = comm.ExecuteReader();

                if(count.HasRows)
                {
                    while(count.Read())
                    {
                        ViewBag.IsDailyJob = count.GetInt32(0);
                        dataset.Add(count.ToString());

                    }
                }
                else
                {
                    ViewBag.IsDailyJob = "0";
                    dataset.Add("0");

                }
                cn.Close();
            }

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand comm = new SqlCommand("SELECT COUNT(JobSheetNo) FROM [IflowSeed].[dbo].[JobInstruction] WHERE Status=@Status", cn);
                comm.Parameters.AddWithValue("@Status", "QME");
                Int32 count = (Int32)comm.ExecuteScalar();
                ViewBag.IsQM = count.ToString();
                dataset.Add(count.ToString());
                cn.Close();
            }

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand comm = new SqlCommand("SELECT COUNT(DISTINCT(LogTagNo)) FROM [IflowSeed].[dbo].[JobAuditTrailDetail] WHERE Status=@Status", cn);
                comm.Parameters.AddWithValue("@Status", "PLANNER");
                Int32 count = (Int32)comm.ExecuteScalar();
                ViewBag.IsPlanner = count.ToString();
                dataset.Add(count.ToString());
                cn.Close();
            }

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand comm = new SqlCommand("SELECT COUNT(DISTINCT(LogTagNo)) FROM [IflowSeed].[dbo].[JobAuditTrailDetail]WHERE Status=@Status ", cn);
                comm.Parameters.AddWithValue("@Status", "PRODUCTION");
                Int32 count = (Int32)comm.ExecuteScalar();
                ViewBag.IsPrinting = count.ToString();
                dataset.Add(count.ToString());
                cn.Close();
            }

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand comm = new SqlCommand("SELECT COUNT(DISTINCT(LogTagNo)) FROM [IflowSeed].[dbo].[JobAuditTrailDetail] WHERE Status=@Status", cn);
                comm.Parameters.AddWithValue("@Status", "INSERTING");
                Int32 count = (Int32)comm.ExecuteScalar();
                ViewBag.IsInserting = count.ToString();
                dataset.Add(count.ToString());
                cn.Close();
            }

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand comm = new SqlCommand("SELECT COUNT(DISTINCT(LogTagNo)) FROM [IflowSeed].[dbo].[JobAuditTrailDetail] WHERE Status=@Status", cn);
                comm.Parameters.AddWithValue("@Status", "MMP");
                Int32 count = (Int32)comm.ExecuteScalar();
                ViewBag.IsMMP = count.ToString();
                dataset.Add(count.ToString());
                cn.Close();
            }

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand comm = new SqlCommand("SELECT COUNT(DISTINCT(LogTagNo)) FROM [IflowSeed].[dbo].[JobAuditTrailDetail] WHERE Status=@Status", cn);
                comm.Parameters.AddWithValue("@Status", "SELFMAILER");
                Int32 count = (Int32)comm.ExecuteScalar();
                ViewBag.IsSM = count.ToString();
                dataset.Add(count.ToString());
                cn.Close();
            }

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand comm = new SqlCommand("SELECT COUNT(DISTINCT(LogTagNo)) FROM [IflowSeed].[dbo].[JobAuditTrailDetail] WHERE  Status=@Status", cn);
                comm.Parameters.AddWithValue("@Status", "POSTING");
                Int32 count = (Int32)comm.ExecuteScalar();
                ViewBag.IsPosting = count.ToString();
                dataset.Add(count.ToString());
                cn.Close();
            }

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand comm = new SqlCommand("SELECT COUNT(DISTINCT(LogTagNo)) FROM [IflowSeed].[dbo].[JobAuditTrailDetail] WHERE Status=@Status", cn);
                comm.Parameters.AddWithValue("@Status", "FINANCE");
                Int32 count = (Int32)comm.ExecuteScalar();
                ViewBag.IsFinance = count.ToString();
                dataset.Add(count.ToString());
                cn.Close();
            }

            //using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            //{
            //    cn.Open();
            //    SqlCommand comm = new SqlCommand("SELECT COUNT(JobSheetNo) FROM [IflowSeed].[dbo].[JobInstruction]", cn);
            //    Int32 count = (Int32)comm.ExecuteScalar();
            //    ViewBag.IsTotal = count.ToString();
            //    dataset.Add(count.ToString());
            //    cn.Close();
            //}

        var labels = new List<string> { "NEW JI", "UNDER DEV", "AUDIT TRAIL", "QM", "PLANNER", "PRINTING", "INSERTING", "MMP", "SELFMAILER", "POSTING", "FINANCE" };
        //var labels = new List<string> { "NEW JI", "UNDER DEV", "AUDIT TRAIL", "QM", "PLANNER", "PRINTING", "INSERTING", "MMP", "SELFMAILER", "POSTING", "FINANCE", "TOTAL JI" };


        var chartData = new
        {
            labels = labels.ToArray(),
            datasets = new[]
            {
                new
                {
                    label = "Total Data",
                    data = dataset.ToArray(),
                    backgroundColor = labels.Select(_ => "steelblue").ToArray(),
                    borderColor = labels.Select(_ => "steelblue").ToArray(),
                    borderWidth = 1
                }
        }
        };

        return Json(chartData, JsonRequestBehavior.AllowGet);
    }



}


