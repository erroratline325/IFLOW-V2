using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using MvcAppV2.Models;
using System.IO;
using Rotativa.Options;
using iTextSharp.text;

namespace MvcAppV2.Models
{
    public class ManageAttachmentController : Controller
    {

        //
        // GET: /ManageAttachment/

        public ActionResult ManageAttachmentList(string Picture_FileId)
        {


            if (Picture_FileId != null)
            {
                ViewBag.IsView = "setview";
                ViewBag.fileid = Picture_FileId;
            }

            List<SelectListItem> li = new List<SelectListItem>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT DISTINCT Customer_Name FROM [IflowSeed].[dbo].[JobAuditTrail]                          
                                        ORDER BY Customer_Name";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ViewAuditTrail model = new ViewAuditTrail();
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


            List<ViewAuditTrail> ViewAuditTrail = new List<ViewAuditTrail>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @" SELECT [Picture_FileId],[Customer_Name],[ProductName],[CreatedOn],[CreateBy]
                                         FROM [IflowSeed].[dbo].[SampleProduct]
                                         WHERE Customer_Name = Customer_Name
                                         AND Code = 'AT'
                                         ORDER BY [CreatedOn] DESC";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ViewAuditTrail model = new ViewAuditTrail();
                    {
                        model.Bil = _bil++;
                        if (reader.IsDBNull(0) == false)
                        {
                            model.Picture_FileId = reader.GetString(0);
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
                            DateTime dateupload = reader.GetDateTime(3);
                            model.CreatedOn = dateupload.ToString("dd/MM/yyyy");
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            model.CreateBy = reader.GetString(4);
                        }
                    }
                    ViewAuditTrail.Add(model);
                }
                cn.Close();
            }
            return View(ViewAuditTrail);
        }

        public ActionResult UploadFile(ViewAuditTrail ModelSample)
        {
            var IdentityName = @Session["Fullname"];
            var Role = @Session["Role"];
            ViewBag.IsDepart = @Session["Department"];

            List<SelectListItem> li2 = new List<SelectListItem>();
            li2.Add(new SelectListItem { Text = "Select", Value = "0" });
            li2.Add(new SelectListItem { Text = "Bank Rakyat", Value = "Bank Rakyat" });
            li2.Add(new SelectListItem { Text = "Majlis Perbandaran Selayang", Value = "Majlis Perbandaran Selayang" });
            li2.Add(new SelectListItem { Text = "Malayan Banking Berhad", Value = "Malayan Banking Berhad" });

            ViewData["customer"] = li2;

            List<SelectListItem> li3 = new List<SelectListItem>();
            li3.Add(new SelectListItem { Text = "Select", Value = "0" });
            ViewData["ProductName"] = li3;

            if (ModelSample.Customer_Name != null)
            {
                int _bil = 1;
                List<SelectListItem> li = new List<SelectListItem>();
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    cn.Open();
                    command.CommandText = @"SELECT [ProductName] FROM [IflowSeed].[dbo].[JobInstruction]     
                                          WHERE Customer_Name=@Customer_Name                  
                                          ORDER BY [ProductName]";
                    command.Parameters.AddWithValue("@Customer_Name", ModelSample.Customer_Name);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ViewAuditTrail model = new ViewAuditTrail();
                        {
                            if (reader.IsDBNull(0) == false)
                            {
                                model.ProductName = reader.GetString(0);
                            }
                        }
                        int i = _bil++;
                        if (i == 1)
                        {
                            li.Add(new SelectListItem { Text = "Please Select" });
                        }
                        li.Add(new SelectListItem { Text = model.ProductName });
                    }
                    cn.Close();
                }
                ViewData["ProductName"] = li;
            }

            if (ModelSample.Customer_Name != null && ModelSample.ProductName != null && ModelSample.FileUploadFile != null)
            {

                var fileName = Path.GetFileName(ModelSample.FileUploadFile.FileName);
                var path = Path.Combine(Server.MapPath("~/FileStore"), fileName);
                ModelSample.FileUploadFile.SaveAs(path);

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    Guid Id = Guid.NewGuid();
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                    cn.Open();
                    SqlCommand command;
                    command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[SampleProduct] (Id,CreatedOn,Picture_FileId,Picture_Extension,Customer_Name,ProductName,CreateBy,Code) values (@Id,@CreatedOn,@Picture_FileId,@Picture_Extension,@Customer_Name,@ProductName,@CreateBy,@Code)", cn);
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@CreatedOn", createdOn);
                    command.Parameters.AddWithValue("@Picture_FileId", fileName.Trim());
                    command.Parameters.AddWithValue("@Picture_Extension", ModelSample.FileUploadFile.ContentType);
                    command.Parameters.AddWithValue("@Customer_Name", ModelSample.Customer_Name);
                    command.Parameters.AddWithValue("@ProductName", ModelSample.ProductName);
                    command.Parameters.AddWithValue("@CreateBy", IdentityName.ToString());
                    command.Parameters.AddWithValue("@Code", "AT");
                    command.ExecuteNonQuery();
                    cn.Close();

                    return RedirectToAction("ManageAttachmentList", "ManageAttachment");
                }
            }

            return View();
        }

        public ActionResult ManageSheduleHighlightVisual(string Id, string Picture_FileId, string Customer_Name, string set)
        {


            if (Picture_FileId != null)
            {
                ViewBag.IsView = "setview";
                ViewBag.fileid = Picture_FileId;
            }

            
            List<SelectListItem> li = new List<SelectListItem>();

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();

                command.CommandText = @"SELECT DISTINCT (Customer_Name) 
                                        FROM SampleProduct 
                                        WHERE Code = 'SH' ";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    SampleProduct model = new SampleProduct();
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

            ViewData["BNO"] = li;
            ViewBag.Display = 1;


            List<SampleProduct> ViewScheduleHighlight = new List<SampleProduct>();

            if (!string.IsNullOrEmpty(Customer_Name))
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    int _bil = 1;
                    cn.Open();
                    command.CommandText = @" SELECT [Picture_FileId],[Customer_Name],[ProductName],[CreatedOn],[CreateBy]
                                         FROM [IflowSeed].[dbo].[SampleProduct]
                                         WHERE Customer_Name=@BNO                                        
                                         AND Code = 'SH'
                                         ORDER BY [CreatedOn] DESC";
                    command.Parameters.Add(new SqlParameter("@BNO", Customer_Name.ToString()));
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        SampleProduct model = new SampleProduct();
                        {
                            model.Bil = _bil++;
                            if (reader.IsDBNull(0) == false)
                            {
                                model.Picture_FileId = reader.GetString(0);
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
                                DateTime dateupload = reader.GetDateTime(3);
                                model.CreatedOn = dateupload.ToString("dd/MM/yyyy");
                            }
                            if (reader.IsDBNull(4) == false)
                            {
                                model.CreateBy = reader.GetString(4);
                            }
                        }
                        ViewScheduleHighlight.Add(model);
                    }
                    cn.Close();
                }
            }
            else
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    int _bil = 1;
                    cn.Open();
                    command.CommandText = @" SELECT [Picture_FileId],[Customer_Name],[ProductName],[CreatedOn],[CreateBy]
                                         FROM [IflowSeed].[dbo].[SampleProduct]
                                         WHERE Customer_Name = Customer_Name
                                         AND Code = 'SH'
                                         ORDER BY [CreatedOn] DESC";
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        SampleProduct model = new SampleProduct();
                        {
                            model.Bil = _bil++;
                            if (reader.IsDBNull(0) == false)
                            {
                                model.Picture_FileId = reader.GetString(0);
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
                                DateTime dateupload = reader.GetDateTime(3);
                                model.CreatedOn = dateupload.ToString("dd/MM/yyyy");
                            }
                            if (reader.IsDBNull(4) == false)
                            {
                                model.CreateBy = reader.GetString(4);
                            }
                        }
                        ViewScheduleHighlight.Add(model);
                    }
                    cn.Close();
                }
            }

           

            return View(ViewScheduleHighlight);
        }



        public ActionResult UploadVisual(SampleProduct ModelSample, string Id)

        { 
            var IdentityName = @Session["Fullname"];
            var Role = @Session["Role"];
            ViewBag.IsDepart = @Session["Department"];
            Session["Id"] = Id;
            ViewBag.Id = Id;

            List<SelectListItem> li2 = new List<SelectListItem>();
            li2.Add(new SelectListItem { Text = "Select", Value = "0" });
            li2.Add(new SelectListItem { Text = "Bank Rakyat", Value = "Bank Rakyat" });
            li2.Add(new SelectListItem { Text = "Majlis Perbandaran Selayang", Value = "Majlis Perbandaran Selayang" });
            li2.Add(new SelectListItem { Text = "Malayan Banking Berhad", Value = "Malayan Banking Berhad" });

            ViewData["customer"] = li2;

            List<SelectListItem> li3 = new List<SelectListItem>();
            li3.Add(new SelectListItem { Text = "Select", Value = "0" });
            ViewData["ProductName"] = li3;

            if (ModelSample.Customer_Name != null)
            {
                int _bil = 1;
                List<SelectListItem> li = new List<SelectListItem>();
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    cn.Open();
                    command.CommandText = @"SELECT [ProductName] FROM [IflowSeed].[dbo].[SchedulerHighlight]     
                                          WHERE Customer_Name=@Customer_Name                  
                                          ORDER BY [ProductName]";
                    command.Parameters.AddWithValue("@Customer_Name", ModelSample.Customer_Name);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        SampleProduct model = new SampleProduct();
                        {
                            if (reader.IsDBNull(0) == false)
                            {
                                model.ProductName = reader.GetString(0);
                            }
                        }
                        int i = _bil++;
                        if (i == 1)
                        {
                            li.Add(new SelectListItem { Text = "Please Select" });
                        }
                        li.Add(new SelectListItem { Text = model.ProductName });
                    }
                    cn.Close();
                }
                ViewData["ProductName"] = li;
            }

           
                if (ModelSample.Customer_Name != null && ModelSample.ProductName != null && ModelSample.FileUploadFile != null)
                {

                    var fileName = Path.GetFileName(ModelSample.FileUploadFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/FileStore"), fileName);
                    ModelSample.FileUploadFile.SaveAs(path);

                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                        cn.Open();
                        SqlCommand command;                    
                        command = new SqlCommand("UPDATE [IflowSeed].[dbo].[SampleProduct] SET CreatedOn=@CreatedOn,Picture_FileId=@Picture_FileId,Picture_Extension=@Picture_Extension,Customer_Name=@Customer_Name,ProductName=@ProductName,CreateBy=@CreateBy,Code=@Code WHERE Id=@Id", cn);
                        command.Parameters.AddWithValue("@CreatedOn", createdOn);
                        command.Parameters.AddWithValue("@Picture_FileId", fileName.Trim());
                        command.Parameters.AddWithValue("@Picture_Extension", ModelSample.FileUploadFile.ContentType);
                        command.Parameters.AddWithValue("@Customer_Name", ModelSample.Customer_Name);
                        command.Parameters.AddWithValue("@ProductName", ModelSample.ProductName);
                        command.Parameters.AddWithValue("@CreateBy", IdentityName.ToString());
                        command.Parameters.AddWithValue("@Code", "SH");
                        command.Parameters.AddWithValue("@Id", ModelSample.Id);
                        command.ExecuteNonQuery();
                        cn.Close();

                        return RedirectToAction("ManageSheduleHighlightVisual", "ManageAttachment");
                    }
                }
            

            return View();
        }

    }
}
