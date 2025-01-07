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
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;

namespace MvcAppV2.Controllers
{
    [MvcAppV2.FilterConfig.UserSessionActionFilter]

    public class QuotationController : Controller
    {
        //
        // GET: /Quotation/            
        public ActionResult ManageQuotation(string id, string set, string CustName, string Customer_Name)
        {

            var IdentityName = @Session["Fullname"];
            var Role = @Session["Role"];
            ViewBag.IsDepart = @Session["Department"];

            if (IdentityName == null || Role == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                if (set == "ApproveMBD")
                {
                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    using (SqlCommand command = new SqlCommand("", cn))

                    {
                        cn.Open();
                        command.CommandText = @"UPDATE  [IflowSeed].[dbo].[Quotation]
                                            SET Approve_MBD = @Approve_MBD
                                            WHERE gID = @id";

                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@Approve_MBD", true);
                        //command.Parameters.AddWithValue("@Status", "Reject");
                        command.ExecuteNonQuery();
                        cn.Close();
                    }
                }

                if (set == "ApproveFin")
                {
                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    using (SqlCommand command = new SqlCommand("", cn))

                    {
                        cn.Open();
                        command.CommandText = @"UPDATE  [IflowSeed].[dbo].[Quotation]
                                            SET Approve_Fin = @Approve_Fin
                                            WHERE gID = @id";

                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@Approve_Fin", true);
                        //command.Parameters.AddWithValue("@Status", "Reject");
                        command.ExecuteNonQuery();
                        cn.Close();
                    }
                }

                if (set == "Reject" && id != null)
                {
                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    using (SqlCommand command = new SqlCommand("", cn))

                    {
                        cn.Open();
                        command.CommandText = @"UPDATE  [IflowSeed].[dbo].[Quotation]
                                            SET RemarkReject = @RemarkReject,  Status = @Status
                                            WHERE gID = @id";

                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@RemarkReject", true);
                        command.Parameters.AddWithValue("@Status", "Rejected");
                        command.ExecuteNonQuery();
                        cn.Close();
                    }
                }
                //Job Close
                if (set == "Close" && id != null)
                {
                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    using (SqlCommand command = new SqlCommand("", cn))

                    {
                        cn.Open();
                        command.CommandText = @"UPDATE  [IflowSeed].[dbo].[Quotation]
                                            SET IsJobCompleted = @IsJobCompleted, Status = @Status 
                                            WHERE gID = @id";

                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@IsJobCompleted", true);
                        command.Parameters.AddWithValue("@Status", "Completed");
                        command.ExecuteNonQuery();
                        cn.Close();
                    }

                    //save newRate in table MaterialCharges and StoreMaterialcharges
                    //List<ListNewRate> ViewPriceRate = new List<ListNewRate>();
                    //using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    //using (SqlCommand command2 = new SqlCommand("", cn2))
                    //{
                    //    int _bil = 0;
                    //    cn2.Open();


                    //    cn2.Close();

                    //}

                    //{
                    //    int _bil = 0;
                    //    cn2.Open();
                    //    command2.CommandText = @"SELECT  ProductType, MaterialType, Description, newRate, Status, Id, ItemCode, CreatedBy, CreatedOn, rateID
                    //                            FROM [IflowSeed].[dbo].[RatePrice]
                    //                            WHERE Id = @id";

                    //    command2.Parameters.AddWithValue("@id", id);
                    //    var reader = command2.ExecuteReader();
                    //    while (reader.Read())
                    //    {
                    //        ListNewRate model = new ListNewRate();
                    //        {
                    //            model.Bil = _bil++;
                    //            if (reader.IsDBNull(0) == false)
                    //            {
                    //                model.ProductType = reader.GetString(0);
                    //            }
                    //            if (reader.IsDBNull(1) == false)
                    //            {
                    //                model.MaterialType = reader.GetString(1);
                    //            }
                    //            if (reader.IsDBNull(2) == false)
                    //            {
                    //                model.Description = reader.GetString(2);
                    //            }
                    //            if (reader.IsDBNull(3) == false)
                    //            {
                    //                model.NewRate = reader.GetString(3);
                    //            }
                    //            if (reader.IsDBNull(4) == false)
                    //            {
                    //                model.Status = reader.GetString(4);
                    //            }
                    //            if (reader.IsDBNull(5) == false)
                    //            {
                    //                model.Id = reader.GetString(5);
                    //            }
                    //            if (reader.IsDBNull(6) == false)
                    //            {
                    //                model.ItemCode = reader.GetString(6);
                    //            }
                    //            if (reader.IsDBNull(7) == false)
                    //            {
                    //                model.CreatedBy = reader.GetString(7);
                    //            }
                    //            if (reader.IsDBNull(8) == false)
                    //            {
                    //                model.CreatedOn = reader.GetDateTime(8);
                    //            }
                    //            if (reader.IsDBNull(9) == false)
                    //            {
                    //                model.rateID = reader.GetGuid(9);
                    //            }
                    //            ViewPriceRate.Add(model);
                    //        }
                    //        using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    //        using (SqlCommand command3 = new SqlCommand("", cn3))
                    //        {
                    //            Guid gID = Guid.NewGuid();
                    //            cn3.Open();


                    //            command3.CommandText = @"IF NOT EXISTS ( SELECT 1 FROM [IflowSeed].[dbo].[MaterialCharges] WHERE [ItemCode] = @ItemCode AND [MaterialType] = @MaterialType AND [Description] = @Description AND [Rate] = @Rate AND id = @id)
                    //                                    BEGIN INSERT INTO [IflowSeed].[dbo].[MaterialCharges] (Id,CreatedOn,ItemCode,MaterialType,Rate,Description,CreatedBy,ProductType) 
                    //                                    VALUES (@Id,@CreatedOn,@ItemCode,@MaterialType,@Rate,@Description,@CreatedBy,@ProductType) END";

                    //            //command3.CommandText = @"INSERT INTO [IflowSeed].[dbo].[MaterialCharges] (Id,CreatedOn,ItemCode,MaterialType,Rate,Description,CreatedBy,ProductType) VALUES (@Id,@CreatedOn,@ItemCode,@MaterialType,@Rate,@Description,@CreatedBy,@ProductType)";
                    //            command3.Parameters.AddWithValue("@Id", gID);
                    //            command3.Parameters.AddWithValue("@CreatedOn", model.CreatedOn);
                    //            command3.Parameters.AddWithValue("@ItemCode", model.ItemCode);
                    //            command3.Parameters.AddWithValue("@MaterialType", model.MaterialType);
                    //            command3.Parameters.AddWithValue("@Rate", model.NewRate);
                    //            command3.Parameters.AddWithValue("@Description", model.Description);
                    //            command3.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    //            command3.Parameters.AddWithValue("@ProductType", model.ProductType);
                    //            command3.ExecuteNonQuery();
                    //            cn3.Close();

                    //        }
                    //        using (SqlConnection cn4 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    //        using (SqlCommand command4 = new SqlCommand("", cn4))
                    //        {
                    //            cn4.Open();
                    //            command4.CommandText = @"IF NOT EXISTS ( SELECT  1 FROM [IflowSeed].[dbo].[StoreMaterialCharges] WHERE [ItemCode] = @ItemCode AND [MaterialType] = @MaterialType AND [Description] = @Description AND [Id] = @Id )
                    //                                    BEGIN INSERT INTO [IflowSeed].[dbo].[StoreMaterialCharges] (gID,MaterialType,ItemCode,Description,Rate,Id) 
                    //                                    VALUES (@gID,@MaterialType,@ItemCode,@Description,@Rate,@Id) END";

                    //            //command4.CommandText = @"INSERT INTO [IflowSeed].[dbo].[StoreMaterialCharges] (gID,MaterialType,ItemCode,Description,Rate,Id) VALUES (@gID,@MaterialType,@ItemCode,@Description,@Rate,@Id)";
                    //            command4.Parameters.AddWithValue("@gID", model.rateID);
                    //            command4.Parameters.AddWithValue("@MaterialType", model.MaterialType);
                    //            command4.Parameters.AddWithValue("@ItemCode", model.ItemCode);
                    //            command4.Parameters.AddWithValue("@Description", model.Description);
                    //            command4.Parameters.AddWithValue("@Rate", model.NewRate);
                    //            command4.Parameters.AddWithValue("@Id", model.Id);
                    //            command4.ExecuteNonQuery();
                    //            cn4.Close();

                    //        }
                    //    }
                    //    cn2.Close();
                    //}

                    return RedirectToAction("ManageQuotation", "Quotation");
                }

                List<QuotationModel> ViewList = new List<QuotationModel>();
                List<ManageQuo> ManageQuo = new List<ManageQuo>();

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    int _bil = 1;
                    int _bil2 = 1;

                    cn.Open();
                    if (set == "search")
                    {

                        command.CommandText = @"SELECT Customer_Name,reffSub,dateCreate,createdBy,reffNo,gID,subject,Status, Approve_Fin, Approve_MBD, IsJobCompleted
                                        FROM [IflowSeed].[dbo].[Quotation]
                                        WHERE Customer_Name LIKE @Customer_Name AND  IsJobCompleted=0 AND Department='SALES' AND RemarkReject = 0 ORDER BY dateCreate desc";
                        command.Parameters.AddWithValue("@Customer_Name", "%" + CustName + "%");

                    }
                    else
                    {

                        command.CommandText = @"SELECT Customer_Name,reffSub,dateCreate,createdBy,reffNo,gID,subject,Status, Approve_Fin, Approve_MBD, IsJobCompleted, RemarkReject
                                   FROM [IflowSeed].[dbo].[Quotation]";

                    }
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.GetString(8) == "0" && reader.GetString(9) == "0")
                        {
                            QuotationModel model = new QuotationModel();
                            {
                                model.Bil = _bil++;
                                if (reader.IsDBNull(0) == false)
                                {
                                    model.Customer_Name = reader.GetString(0);
                                }
                                if (reader.IsDBNull(1) == false)
                                {
                                    model.ReffSub = reader.GetString(1);
                                }
                                if (reader.IsDBNull(2) == false)
                                {
                                    model.dateCreatedTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(2));
                                }
                                if (reader.IsDBNull(3) == false)
                                {
                                    model.createdBy = reader.GetString(3);
                                }
                                if (reader.IsDBNull(4) == false)
                                {
                                    model.ReffNo = reader.GetString(4);
                                }
                                if (reader.IsDBNull(5) == false)
                                {
                                    model.gID = reader.GetGuid(5);
                                }
                                if (reader.IsDBNull(6) == false)
                                {
                                    model.subject = reader.GetString(6);
                                }
                                if (reader.IsDBNull(7) == false)
                                {
                                    model.Status = reader.GetString(7);
                                }
                                if (reader.IsDBNull(8) == false)
                                {
                                    if (reader.GetString(8) == "0")
                                    {
                                        model.Approve_Fin = "No";
                                    }
                                    if (reader.GetString(8) == "1")
                                    {
                                        model.Approve_Fin = "Yes";
                                    }
                                }
                                if (reader.IsDBNull(9) == false)
                                {
                                    if (reader.GetString(9) == "0")
                                    {
                                        model.Approve_MBD = "No";
                                    }
                                    if (reader.GetString(9) == "1")
                                    {
                                        model.Approve_MBD = "Yes";
                                    }
                                }

                            }
                            ViewList.Add(model);
                        }
                        else
                        {
                            ManageQuo model2 = new ManageQuo();
                            {
                                model2.Bil = _bil2++;
                                if (reader.IsDBNull(0) == false)
                                {
                                    model2.Customer_Name = reader.GetString(0);
                                }
                                if (reader.IsDBNull(1) == false)
                                {
                                    model2.ReffSub = reader.GetString(1);
                                }
                                if (reader.IsDBNull(2) == false)
                                {
                                    model2.dateCreatedTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(2));
                                }
                                if (reader.IsDBNull(3) == false)
                                {
                                    model2.createdBy = reader.GetString(3);
                                }
                                if (reader.IsDBNull(4) == false)
                                {
                                    model2.ReffNo = reader.GetString(4);
                                }
                                if (reader.IsDBNull(5) == false)
                                {
                                    model2.gID = reader.GetGuid(5);
                                }
                                if (reader.IsDBNull(6) == false)
                                {
                                    model2.subject = reader.GetString(6);
                                }
                                if (reader.IsDBNull(7) == false)
                                {
                                    model2.Status = reader.GetString(7);
                                }
                                if (reader.IsDBNull(8) == false)
                                {
                                    if (reader.GetString(8) == "0")
                                    {
                                        model2.Approve_Fin = "No";
                                    }
                                    if (reader.GetString(8) == "1")
                                    {
                                        model2.Approve_Fin = "Yes";
                                    }
                                }
                                if (reader.IsDBNull(9) == false)
                                {
                                    if (reader.GetString(9) == "0")
                                    {
                                        model2.Approve_MBD = "No";
                                    }
                                    if (reader.GetString(9) == "1")
                                    {
                                        model2.Approve_MBD = "Yes";
                                    }
                                }
                            }
                            ManageQuo.Add(model2);
                        }

                    }
                    cn.Close();
                    //var temp = ViewList.Where(s => s.IsJobCompleted);
                    //var temp2 = ViewList.Where(s => !s.IsJobCompleted);

                    ViewBag.ManageQuo = ViewList;
                    ViewBag.ManageQuoCompleted = ManageQuo;
                    //foreach(var item in ManageQuo)
                    //{
                    //    Debug.WriteLine("Bil :" + item.Bil);
                    //}
                }

                return View(ViewList);
            }

            
        }

        public ActionResult UploadQuotation()
        {
            List<string> CustomerList = new List<string>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();

                SqlCommand cmd1 = new SqlCommand("SELECT DISTINCT Customer_Name FROM CustomerDetails",cn);
                SqlDataReader rm1 = cmd1.ExecuteReader();

                while (rm1.Read())
                {
                    CustomerList.Add(rm1.GetString(0));   
                }

                cn.Close();
            }    

            ViewBag.CustomerList = CustomerList;

            return View();
        }

        public ActionResult UploadFileStore(FileStore FileUploadLocation,string subject, string Customer_Name)
        {
            var Id = Session["Id"];
            var Status = Session["Status"];
            ViewBag.IsDepart = @Session["Department"];
            ViewBag.IsRole = @Session["Role"];
            string Role = Session["Role"].ToString();
            string Deptment = @Session["Department"].ToString();
            string user = Session["FullName"].ToString();

            var CurrNo = new GeneralLetterModel();

            if (FileUploadLocation.FileUploadFile != null && FileUploadLocation.set == "save")
            {
                var fileName = Path.GetFileName(FileUploadLocation.FileUploadFile.FileName);
                var path = Path.Combine(Server.MapPath("~/FileStore"), fileName);
                FileUploadLocation.FileUploadFile.SaveAs(path);

                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    Guid guidId = Guid.NewGuid();
                    Guid Idx = Guid.NewGuid();
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                    cn2.Open();
                    SqlCommand cmd1 = new SqlCommand("INSERT INTO Quotation (gID,CreatedOn,reffNo,CreatedBy,position,dateCreate,subject,reffSub,Customer_Name,Uploaded,Approve_Fin,Approve_MBD,RemarkReject,Status,IsJobCompleted) VALUES(@Idx,@CreatedOn1,@reffNo,@CreatedBy,@position,@dateCreate,@subject,@reffSub,@Customer_Name,@Uploaded,@Approve_Fin,@Approve_MBD,@RemarkReject,@Status,@IsJobCompleted)", cn2);
                    cmd1.Parameters.AddWithValue("@Idx",Idx);
                    cmd1.Parameters.AddWithValue("@CreatedOn1", createdOn);
                    cmd1.Parameters.AddWithValue("@reffNo", CurrNo.RefNo+"U");
                    cmd1.Parameters.AddWithValue("@CreatedBy", user);
                    cmd1.Parameters.AddWithValue("@position", Role);
                    cmd1.Parameters.AddWithValue("@dateCreate", createdOn);
                    cmd1.Parameters.AddWithValue("@subject", subject);
                    cmd1.Parameters.AddWithValue("@reffSub", CurrNo.RefNo+"U");
                    cmd1.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                    cmd1.Parameters.AddWithValue("@Uploaded", "YES");
                    cmd1.Parameters.AddWithValue("@Approve_Fin", 1);
                    cmd1.Parameters.AddWithValue("@Approve_MBD", 1);
                    cmd1.Parameters.AddWithValue("@RemarkReject", 0);
                    cmd1.Parameters.AddWithValue("@Status", "Completed");
                    cmd1.Parameters.AddWithValue("@IsJobCompleted", 1);



                    cmd1.ExecuteNonQuery();


                    SqlCommand command;
                    command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[QuoFileStore] (Id,CreatedOn,File_Id,File_Extension,QuotationID) values (@Id,@CreatedOn,@Picture_FileId,@Picture_Extension,@QuotationID)", cn2);
                    command.Parameters.AddWithValue("@Id", guidId);
                    command.Parameters.AddWithValue("@CreatedOn", createdOn);
                    command.Parameters.AddWithValue("@Picture_FileId", fileName.Trim());
                    command.Parameters.AddWithValue("@Picture_Extension", FileUploadLocation.FileUploadFile.ContentType);
                    command.Parameters.AddWithValue("@QuotationID", Idx);
                   

                    command.ExecuteNonQuery();

                    cn2.Close();

                    return RedirectToAction("ManageQuotation", "Quotation");
                }
            }

            if (FileUploadLocation.set == "back")
            {
                return RedirectToAction("ManageQuotation", "Quotation");
            }

            return View();
        }

        public ActionResult ViewQuotationPdf(string Id)
        {
            string fileName = "";
            string mimeType = "";
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();

                SqlCommand cmd1 = new SqlCommand(("SELECT File_id, File_Extension FROM QuoFileStore WHERE QuotationID = @Id"), cn);
                cmd1.Parameters.AddWithValue("@Id", Id);
                SqlDataReader rm1 = cmd1.ExecuteReader();

                while (rm1.Read())
                {
                    fileName = rm1.GetString(0);
                    mimeType = rm1.GetString(1);

                }

                cn.Close();

                ViewBag.FileName=fileName;
                ViewBag.mimeType=mimeType;
            }
            return View();
        }

        public FileResult ViewQuoPDF(string fileName,string mimeType)
        {
            
            string filePath = Server.MapPath("~/FileStore/" + fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, mimeType);
        }


        List<CustomerDetails> viewCustomerEnquiry = new List<CustomerDetails>();
        public string PreparedPos { get; set; }
        public string PreparedEmail { get; set; }


        public ActionResult AddQuotationEquiry(string custname, string custenqId, string profile, string phoneno, string email, string avbno, string awdno, string set, string JobRequest, string CompanyName, string Sic)
        {
            ViewBag.IsDepart = @Session["Department"];
            ViewBag.IsRole = @Session["Role"];
            var IdentityName = @Session["Fullname"];


            if (!string.IsNullOrEmpty(custname) && set != "AddNew")
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    int _bil = 1;
                    cn.Open();
                    command.CommandText = @"SELECT Id, Customer_Name, AccountManager, Contact_Person, Address1, Cust_Phone, Cust_FaxNo, Cust_Mobile, Cust_Email, Cust_Web,ProductType,Address2,Address3,Cust_Postcode,Cust_State
                                     FROM [IflowSeed].[dbo].[CustomerDetails]
                                     WHERE Customer_Name LIKE @Customer_Name";
                    command.Parameters.AddWithValue("@Customer_Name", "%" + custname + "%");
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
                                model.ProductType = reader.GetString(10);
                            }
                            if (reader.IsDBNull(11) == false)
                            {
                                model.Address2 = reader.GetString(11);
                            }
                            if (reader.IsDBNull(12) == false)
                            {
                                model.Address3 = reader.GetString(12);
                            }
                            if (reader.IsDBNull(13) == false)
                            {
                                model.Cust_Postcode = reader.GetString(13);
                            }
                            if (reader.IsDBNull(14) == false)
                            {
                                model.Cust_State = reader.GetString(14);
                            }
                        }
                        viewCustomerEnquiry.Add(model);
                    }
                    cn.Close();
                }
            }

            else //display manager
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    int _bil = 1;
                    cn.Open();
                    command.CommandText = @"SELECT Id, Customer_Name, AccountManager, Contact_Person
                                     FROM [IflowSeed].[dbo].[CustomerDetails]";


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
                        }
                        viewCustomerEnquiry.Add(model);
                    }
                    cn.Close();
                }
            }
            return View(viewCustomerEnquiry);
        }

        public string ProductType { get; set; }
        public object CurrNo { get; set; }
        public string Font_arialbd { get; private set; }
        List<SelectListItem> li1 = new List<SelectListItem>();
        List<SelectListItem> li2 = new List<SelectListItem>();
        List<SelectListItem> li3 = new List<SelectListItem>();
        List<SelectListItem> li4 = new List<SelectListItem>();


        List<QuotationModel> ViewList = new List<QuotationModel>();
        [HttpGet]
        public ActionResult ViewQuo(string id, string Status, string set, String ReffSub, string Customer_Name, string ProductType, string Address1, string Address2, string Address3, string CC1_name, string Cust_State, string Cust_Postcode, string get, string Cust_Term, string gID, string MaterialType, string ItemCode, string ProductTerm, string CustomerName)
        {

            ViewBag.IsDepart = @Session["Department"];
            ViewBag.IsRole = @Session["Role"];

            List<ListTable> listTemp = new List<ListTable>();
            ViewBag.TbleMaterial = listTemp;

            List<ListNewRate> listNewRate = new List<ListNewRate>();
            ViewBag.TblePriceRate = listNewRate;

            //Viewbag status to pass data to UI
            ViewBag.Status = Status;
            ViewBag.Customer_Name = Customer_Name;
            ViewBag.Address1 = Address1;
            ViewBag.Address2 = Address2;
            ViewBag.Address3 = Address3;
            ViewBag.Cust_Postcode = Cust_Postcode;
            ViewBag.Cust_State = Cust_State;

            //Viewbag gID to pass data to UI
            //declare @session["id"] sebagai id (id pass dr Gletter)
            @Session["Id"] = id;
            ViewBag.id = id;

            //view pdf quotation
            using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn1.Open();

                SqlCommand cmd1 = new SqlCommand("SELECT Uploaded FROM Quotation WHERE gID = @gID",cn1);
                cmd1.Parameters.AddWithValue("@gID", id);
                SqlDataReader rm1 = cmd1.ExecuteReader();

                while(rm1.Read())
                {
                    if (!string.IsNullOrEmpty(rm1["Uploaded"].ToString()))
                    {
                        return RedirectToAction("ViewQuotationPdf", "Quotation", new { Id = id });
                    }
                }
                

                cn1.Close();
            }

            //Save term into database 
            if (set == "AddList")
            {
                using (SqlConnection cn7 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command7 = new SqlCommand("", cn7))
                {
                    cn7.Open();
                    command7.CommandText = @"INSERT INTO [IflowSeed].[dbo].[QouStoreTerm] (gID,Cust_Term) VALUES (@gID, @Cust_Term)";

                    QuotationModel model = new QuotationModel();
                    command7.Parameters.AddWithValue("@gID", model.gID);
                    command7.Parameters.AddWithValue("@Cust_Term", model.Cust_Term);
                    command7.ExecuteNonQuery();
                    cn7.Close();

                }
            }

            //productType
            int _bild = 1;
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT DISTINCT ProductType FROM [IflowSeed].[dbo].[MaterialCharges]  
                                      WHERE ProductType IS NOT NULL AND ProductType != ' '
                                     ORDER BY ProductType";
                //
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    MaterialCharges model = new MaterialCharges();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            model.ProductType = reader.GetString(0);
                        }
                    }
                    int i = _bild++;

                    if (i == 1)
                    {
                        li1.Add(new SelectListItem { Text = "Please Select" });
                        li1.Add(new SelectListItem { Text = model.ProductType });

                    }
                    else
                    {
                        li1.Add(new SelectListItem { Text = model.ProductType });
                    }
                }
                cn.Close();
            }
            ViewData["ProductType_"] = li1;

            //material
            int _bildd = 1;
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT DISTINCT MaterialType FROM [IflowSeed].[dbo].[MaterialCharges]                          
                                     ORDER BY MaterialType";
                //
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    MaterialCharges model = new MaterialCharges();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            model.MaterialType = reader.GetString(0);
                        }
                    }
                    int i = _bildd++;

                    if (i == 1)
                    {
                        li2.Add(new SelectListItem { Text = "Please Select" });
                        li2.Add(new SelectListItem { Text = model.MaterialType });

                    }
                    else
                    {
                        li2.Add(new SelectListItem { Text = model.MaterialType });
                    }
                }
                cn.Close();
            }
            ViewData["MaterialType_"] = li2;


            //ItemCode
            int _bildd2 = 1;
            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command2 = new SqlCommand("", cn2))
            {
                cn2.Open();
                command2.CommandText = @"SELECT DISTINCT ItemCode FROM [IflowSeed].[dbo].[MaterialCharges]                          
                                     ORDER BY ItemCode";
                //
                var reader2 = command2.ExecuteReader();
                while (reader2.Read())
                {
                    MaterialCharges model = new MaterialCharges();
                    {
                        if (reader2.IsDBNull(0) == false)
                        {
                            model.ItemCode = reader2.GetString(0);
                        }
                    }
                    int i = _bildd2++;
                    if (i == 1)
                    {
                        li3.Add(new SelectListItem { Text = "Please Select" });
                        li3.Add(new SelectListItem { Text = model.ItemCode });

                    }
                    else
                    {
                        li3.Add(new SelectListItem { Text = model.ItemCode });
                    }
                }
                cn2.Close();
            }
            ViewData["ItemCode_"] = li3;
            //end itemCode

            //
            int _bildd4 = 1;
            using (SqlConnection cn4 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn4))
            {
                cn4.Open();
                command.CommandText = @"SELECT DISTINCT ProductTerm FROM [IflowSeed].[dbo].[QouStoreTerm]
                                     WHERE ProductTerm IS NOT NULL AND ProductTerm != ' '
                                     ORDER BY ProductTerm";
                //
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    QuotationModel model = new QuotationModel();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            model.ProductTerm = reader.GetString(0);
                        }
                    }
                    int i = _bildd4++;
                    if (i == 1)
                    {
                        li4.Add(new SelectListItem { Text = "Please Select" });
                        li4.Add(new SelectListItem { Text = model.ProductTerm });

                    }
                    else
                    {
                        li4.Add(new SelectListItem { Text = model.ProductTerm });
                    }
                }
                cn4.Close();
            }
            ViewData["ProductTerm_"] = li4;

            if (set == "Print" && id != null)
            {

                using (SqlConnection cn5 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand comm = new SqlCommand("", cn5))
                {
                    cn5.Open();
                    comm.CommandText = @"SELECT Customer_Name
                                            FROM [IflowSeed].[dbo].[Quotation]
                                            WHERE gID = @id ";
                    comm.Parameters.AddWithValue("@id", id.ToString());
                    //comm.Parameters.AddWithValue("@Customer_Name", Customer_Name.ToString());

                    QuotationModel model = new QuotationModel();

                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {

                        {
                            if (reader.IsDBNull(0) == false)
                            {
                                model.Customer_Name = reader.GetString(0);
                            }

                        }
                        CreatePDF(id, Customer_Name);
                        return View(model);

                    }

                    //CreatePDF(QuoID.ToString());


                    cn5.Close();
                    return RedirectToAction("ManageQuotation", "Quotation");
                }
            }
            //print pdf end

            if (set == "Close" && id != null)
            {

                using (SqlConnection cn5 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn5.Open();
                    SqlCommand comm = new SqlCommand("SELECT COUNT(*) FROM [IflowSeed].[dbo].[FileStoreManagement]" +
                                                "WHERE MGNo=@Id", cn5);
                    comm.Parameters.AddWithValue("@Id", id);
                    Int32 count = (Int32)comm.ExecuteScalar();
                    if (count > 0)
                    {
                        using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            cn3.Open();
                            SqlCommand command3;
                            command3 = new SqlCommand("UPDATE [IflowSeed].[dbo].[Quotation] SET Status='Closed', IsJobCompleted=@IsJobCompleted WHERE gID=@Id", cn3);
                            command3.Parameters.AddWithValue("@Id", id);
                            command3.Parameters.AddWithValue("@IsJobCompleted", true);
                            command3.ExecuteNonQuery();
                            cn3.Close();
                        }
                    }
                    cn5.Close();
                    return RedirectToAction("ViewQuo", "Quotation");
                }
            }

            int _bil = 1;
            List<SelectListItem> li = new List<SelectListItem>();

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Fullname FROM [IflowSeed].[dbo].[User]                               
                                     ORDER BY [Fullname]";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    UserStaff model = new UserStaff();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            model.Fullname = reader.GetString(0);
                        }
                    }
                    int i = _bil++;
                    if (i == 1)
                    {
                        li.Add(new SelectListItem { Text = "Please Select" });
                    }
                    li.Add(new SelectListItem { Text = model.Fullname });
                }
                cn.Close();
            }
            ViewData["Fullname_"] = li;


            //Check if there is Id present or not
            if (id == null)
            {
                Session["FileUploadID"] = "";
                return View();
            }
            else
            {
                ViewBag.MGMT = "UpdateDelete";

                //start table termList

                List<ListTableTerm> listTerm = new List<ListTableTerm>();
                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand cmd = new SqlCommand("", cn2))
                {
                    //model.gID = get.gID;
                    cn2.Open();
                    cmd.CommandText = @"SELECT Cust_Term, ProductTerm, TermID  FROM [IflowSeed].[dbo].[StoreTerms]                          
                                      WHERE Id=@id";


                    //command.Parameters.AddWithValue("@Cust_Term", Cust_Term.ToString());
                    cmd.Parameters.AddWithValue("@id", id.ToString());

                    //

                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        {
                            QuotationModel model = new QuotationModel();
                            model.Cust_Term = reader.GetString(0);
                            listTerm.Add(new ListTableTerm { Cust_Term = reader.GetString(0), ProductTerm = reader.GetString(1), TermID = reader.GetGuid(2) });
                        }
                    }
                    cn2.Close();
                }
                //ViewData["ProductTerm_"] = li4;            
                //return Json(new { data =  });
                ViewBag.Cust_Term = listTerm;
                ViewBag.gID = gID;

                //end



                //start table material

                //List<ListTable> listTemp = new List<ListTable>();
                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn2))
                {
                    string Description;
                    string Rate;

                    cn2.Open();
                    command.CommandText = @"SELECT MaterialType, Description, Rate, gID, Volume, TotalRate FROM [IflowSeed].[dbo].[StoreMaterialCharges]                               
                                      WHERE Id=@id";

                    command.Parameters.AddWithValue("@id", id.ToString());
                    int bil2 = 1;
                    var reader = command.ExecuteReader();
                    QuotationModel model = new QuotationModel();
                    {
                        while (reader.Read())
                        {

                            bil2 = bil2 + 1;
                            MaterialType = reader.GetString(0);
                            Description = reader.GetString(1);
                            Rate = reader.GetString(2);

                            listTemp.Add(new ListTable { MaterialType = MaterialType, Description = Description, Rate = Rate, gID = reader.GetGuid(3), bil2 = bil2 + 1,  Volume = reader.GetInt32(4), TotalRate = reader.GetString(5)});

                        }
                        cn2.Close();

                    }
                    ViewBag.TbleMaterial = listTemp;

                    //return View(model);

                }

                //end table material

                //Display RatePrice               
                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand cmd = new SqlCommand("", cn2))
                {
                    //model.gID = get.gID;

                    cn2.Open();
                    cmd.CommandText = @"SELECT ItemCode, MaterialType, Description, Rate, rateID, CreatedOn, Volume, TotalRate  FROM [IflowSeed].[dbo].[HistoryMaterialCharge]                          
                                          WHERE Id=@id ORDER BY CreatedOn desc";

                    //command.Parameters.AddWithValue("@Cust_Term", Cust_Term.ToString());
                    cmd.Parameters.AddWithValue("@id", id.ToString()); //Id

                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        {
                            ItemCode = reader.GetString(0);
                            listNewRate.Add(new ListNewRate { MaterialType = reader.GetString(1), Description = reader.GetString(2), Rate = reader.GetString(3), rateID = reader.GetGuid(4), CreatedOn = reader.GetDateTime(5), Volume = reader.GetInt32(6), TotalRate = reader.GetString(7)});
                        }
                    }
                    cn2.Close();
                }
                ViewBag.TblePriceRate = listNewRate;
                //end display price rate


                //SELECT DATA FROM DATABASE TO BE VIEW IN UI
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    cn.Open();
                    command.CommandText = @"SELECT ReffNo,Customer_Name,Address1,Address2,Address3,Cust_Postcode,Cust_State,attName,salutation,subject,gID,descr,reffSub,Status,ProductType,CC1_name,CC1_position,CC1_mobile,CC1_email,CC2_name,CC2_position,CC2_mobile,CC2_email,attName                                          
                                            FROM [IflowSeed].[dbo].[Quotation]
                                            WHERE gID = @id ";
                    command.Parameters.AddWithValue("@id", id.ToString());

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        QuotationModel gets = new QuotationModel();
                        {
                            if (reader.IsDBNull(0) == false)
                            {
                                gets.ReffNo = reader.GetString(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                ViewBag.Customer_Name = reader.GetString(1);
                            }
                            if (reader.IsDBNull(2) == false)
                            {
                                gets.Address1 = reader.GetString(2);
                                ViewBag.Address1 = gets.Address1;
                            }
                            if (reader.IsDBNull(3) == false)
                            {
                                gets.Address2 = reader.GetString(3);
                                ViewBag.Address2 = gets.Address2;
                            }
                            if (reader.IsDBNull(4) == false)
                            {
                                gets.Address3 = reader.GetString(4);
                                ViewBag.Address3 = gets.Address3;
                            }
                            if (reader.IsDBNull(5) == false)
                            {
                                gets.Cust_Postcode = reader.GetString(5);
                                ViewBag.Cust_Postcode = gets.Cust_Postcode;
                            }
                            if (reader.IsDBNull(6) == false)
                            {
                                gets.Cust_State = reader.GetString(6);
                                ViewBag.Cust_State = gets.Cust_State;
                            }
                            if (reader.IsDBNull(7) == false)
                            {
                                gets.attName = reader.GetString(7);
                                //ViewBag.attName = gets.attName;
                            }
                            if (reader.IsDBNull(8) == false)
                            {
                                gets.salutation = reader.GetString(8);
                                //ViewBag.salutation = gets.salutation;
                            }
                            if (reader.IsDBNull(9) == false)
                            {
                                gets.subject = reader.GetString(9);
                                ViewBag.subject = gets.subject;
                            }
                            if (reader.IsDBNull(10) == false)
                            {
                                gets.gID = reader.GetGuid(10);
                                ViewBag.gID = gets.gID;
                            }
                            if (reader.IsDBNull(11) == false)
                            {
                                gets.descr = reader.GetString(11);
                                ViewBag.descr = gets.descr;
                            }
                            if (reader.IsDBNull(12) == false)
                            {
                                gets.ReffSub = reader.GetString(12);
                                ViewBag.reffSub = gets.ReffSub;
                            }
                            //if (reader.IsDBNull(13) == false)
                            //{
                            //    gets.RemarkReject = reader.GetString(13);
                            //    ViewBag.RemarkReject = gets.RemarkReject;
                            //}
                            if (reader.IsDBNull(13) == false)
                            {
                                gets.Status = reader.GetString(13);
                                ViewBag.Status = gets.Status;
                            }
                            if (reader.IsDBNull(14) == false)
                            {
                                gets.ProductType = reader.GetString(14);
                                ViewBag.ProductType = gets.ProductType;
                            }
                            if (reader.IsDBNull(15) == false)
                            {
                                gets.CC1_name = reader.GetString(15);
                                ViewBag.CC1_name = gets.CC1_name;
                            }
                            if (reader.IsDBNull(16) == false)
                            {
                                gets.CC1_position = reader.GetString(16);
                                ViewBag.CC1_position = gets.CC1_position;
                            }
                            if (reader.IsDBNull(17) == false)
                            {
                                gets.CC1_mobile = reader.GetString(17);
                                ViewBag.CC1_mobile = gets.CC1_mobile;
                            }
                            if (reader.IsDBNull(18) == false)
                            {
                                gets.CC1_email = reader.GetString(18);
                                ViewBag.CC1_email = gets.CC1_email;
                            }
                            if (reader.IsDBNull(19) == false)
                            {
                                gets.CC2_name = reader.GetString(19);
                                ViewBag.CC2_name = gets.CC2_name;
                            }
                            if (reader.IsDBNull(20) == false)
                            {
                                gets.CC2_position = reader.GetString(20);
                                ViewBag.CC2_position = gets.CC2_position;
                            }
                            if (reader.IsDBNull(21) == false)
                            {
                                gets.CC2_mobile = reader.GetString(21);
                                ViewBag.CC2_mobile = gets.CC2_mobile;
                            }
                            if (reader.IsDBNull(22) == false)
                            {
                                gets.CC2_email = reader.GetString(22);
                                ViewBag.CC2_email = gets.CC2_email;
                            }
                            if (reader.IsDBNull(23) == false)
                            {
                                gets.attName = reader.GetString(23);
                                ViewBag.attName = gets.attName;
                            }
                        }
                        ViewBag.AWDStat = "Update";
                        return View(gets);
                    }
                }
            }

            return View();
        }
        // List<CustomerDetails> viewCustTerm = new List<CustomerDetails>();
        //List<QouStoreTerm> viewCustTerm = new List<QouStoreTerm>();

        public bool IsPostBack { get; }


        [ValidateInput(false)]
        [HttpPost]

        public ActionResult ViewQuo(QuotationModel get, String Department, string deleteID, string deleteID2, String uID, String id, String Id, string ReffSub, string RefNo, string CC2_name, string CC2_position, string CC2_mobile, string CC2_email, string CC1_name, string CC1_position, string CC1_mobile, string CC1_email, string Customer_Name, string Cust_Term, String set, String value, String Status, String Address1, String Address2, String Address3, string Cust_Postcode, string Cust_State, string gID, string MaterialType, string ItemCode, string DescID, string Rate, string Description, string ProductType, string ProductTerm, string cust_Term, string tableMaterial, string NewRate,string Volume)
        {

            String descriptionVal;
            ViewBag.Status = "New";
            ViewBag.Customer_Name = Customer_Name;
            ViewBag.Address1 = Address1;
            ViewBag.Address2 = Address2;
            ViewBag.Address3 = Address3;
            ViewBag.Cust_Postcode = Cust_Postcode;
            ViewBag.Cust_State = Cust_State;
            ViewBag.IsRole = @Session["Role"];
            //List<SelectListItem> li2 = new List<SelectListItem>();

            //ViewData["MaterialType_"] = li2;
            ViewData["ProductTerm_"] = li4;
            ViewData["ItemCode_"] = li3;

                if (set == "newAdd")
            {
                ViewBag.IsDepart = @Session["Department"];
                Department = ViewBag.IsDepart;

                //uID
                ViewBag.uID = Session["Idx"].ToString();
                uID = ViewBag.uID;

                //gID
                @Session["Id"] = id;

                ViewBag.gID = gID.ToString();
                //ViewBag.gID = get.gID;
                @Session["gID"] = ViewBag.gID;
                string createdDate2 = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                get.CreatedOn = Convert.ToDateTime(createdDate2);
                get.dateCreated = Convert.ToDateTime(createdDate2);

                Guid gid = Guid.NewGuid();
                get.gID = gid;

                string user = Session["FullName"].ToString();
                string position = Session["Role"].ToString();
                get.createdBy = user.ToString();
                get.position = position.ToString();

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    //Viewback Each Position & Email
                    ViewBag.PreparedPos = PreparedPos;
                    ViewBag.PreparedEmail = PreparedEmail;

                    string DateTxt = DateTime.Now.ToString("yyyy-MM-dd");

                    var CurrNo = new GeneralLetterModel();

                    cn.Open();
                    command.CommandText = @"INSERT INTO [IflowSeed].[dbo].[Quotation]
                                           (gID,CreatedOn,Customer_Name,reffNo,Address1,Address2,Address3,Cust_Postcode,Cust_State,attName,salutation,subject,descr,createdBy,position,dateCreate,reffSub,Status,Department,IsJobCompleted,CC2_name,CC2_position,CC2_mobile,CC2_email,CC1_name,CC1_position,CC1_mobile,CC1_email,Approve_Fin,Approve_MBD,RemarkReject)" +
                                            "VALUES (@gID,@CreatedOn, @Customer_Name,@reffNo, @Address1, @Address2, @Address3, @Cust_Postcode, @Cust_State, @attName, @salutation, @subject, @descr, @createdBy, @position, @dateCreate,@reffSub,@Status,@Department,@IsJobCompleted,@CC2_name,@CC2_position,@CC2_mobile,@CC2_email,@CC1_name,@CC1_position,@CC1_mobile,@CC1_email,@Approve_Fin,@Approve_MBD,@RemarkReject)";
                    command.Parameters.AddWithValue("@gID", get.gID);
                    command.Parameters.AddWithValue("@CreatedOn", get.CreatedOn);
                    command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                    command.Parameters.AddWithValue("@reffNo", CurrNo.RefNo);
                    if (get.Address1 == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@Address1", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Address1", get.Address1);
                    }

                    if (get.Address2 == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@Address2", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Address2", get.Address2);
                    }

                    if (get.Address3 == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@Address3", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Address3", get.Address3);
                    }

                    if (get.Cust_Postcode == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@Cust_Postcode", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Cust_Postcode", get.Cust_Postcode);
                    }

                    if (get.Cust_State == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@Cust_State", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Cust_State", get.Cust_State);
                    }

                    if (get.attName == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@attName", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@attName", get.attName);
                    }

                    if (get.salutation == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@salutation", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@salutation", get.salutation);
                    }

                    if (get.subject == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@subject", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@subject", get.subject);
                    }

                    if (get.descr == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@descr", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@descr", get.descr);
                    }

                    command.Parameters.AddWithValue("@createdBy", get.createdBy);
                    command.Parameters.AddWithValue("@position", get.position);
                    command.Parameters.AddWithValue("@dateCreate", get.dateCreated);
                    command.Parameters.AddWithValue("@reffSub", CurrNo.RefNo);


                    command.Parameters.AddWithValue("@Status", "New");
                    command.Parameters.AddWithValue("@Department", Department);
                    command.Parameters.AddWithValue("@IsJobCompleted", false);
                    command.Parameters.AddWithValue("@CC2_name", CC2_name);
                    command.Parameters.AddWithValue("@CC2_position", CC2_position);
                    command.Parameters.AddWithValue("@CC2_mobile", CC2_mobile);
                    command.Parameters.AddWithValue("@CC2_email", CC2_email);

                    command.Parameters.AddWithValue("@CC1_name", CC1_name);
                    command.Parameters.AddWithValue("@CC1_position", CC1_position);
                    command.Parameters.AddWithValue("@CC1_mobile", CC1_mobile);
                    command.Parameters.AddWithValue("@CC1_email", CC1_email);
                    command.Parameters.AddWithValue("@Approve_Fin", false);
                    command.Parameters.AddWithValue("@Approve_MBD", false);
                    command.Parameters.AddWithValue("@RemarkReject", false);
                    command.ExecuteNonQuery();
                    cn.Close();


                }
                return RedirectToAction("ManageQuotation", "Quotation");
            }
            //start table ProductType
            int _bild = 1;
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                li1.Clear();
                command.CommandText = @"SELECT DISTINCT ProductType FROM [IflowSeed].[dbo].[MaterialCharges] 
                                     WHERE ProductType IS NOT NULL AND ProductType != ' '
                                     ORDER BY ProductType";
                //
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    MaterialCharges models = new MaterialCharges();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            models.ProductType = reader.GetString(0);
                        }
                    }
                    int i = _bild++;

                    if (i == 1)
                    {
                        li1.Add(new SelectListItem { Text = "Please Select"});
                        li1.Add(new SelectListItem { Text = models.ProductType});

                    }
                    else
                    {
                        li1.Add(new SelectListItem { Text = models.ProductType});
                    }
                }
                cn.Close();
            }
            ViewData["ProductType_"] = li1;

            int _bildd4 = 1;
            using (SqlConnection cn4 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn4))
            {
                cn4.Open();
                command.CommandText = @"SELECT DISTINCT ProductTerm FROM [IflowSeed].[dbo].[QouStoreTerm]    
                                     WHERE ProductTerm IS NOT NULL AND ProductTerm != ' '
                                     ORDER BY ProductTerm";
                //
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    QuotationModel model2 = new QuotationModel();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            model2.ProductTerm = reader.GetString(0);
                        }
                    }
                    int i = _bildd4++;
                    if (i == 1)
                    {
                        li4.Add(new SelectListItem { Text = "Please Select" });
                        li4.Add(new SelectListItem { Text = model2.ProductTerm});

                    }
                    else
                    {
                        li4.Add(new SelectListItem { Text = model2.ProductTerm});
                    }
                }
                cn4.Close();
            }
            ViewData["ProductTerm_"] = li4;



            //insert into database
            QuotationModel model = new QuotationModel();
            Guid gIDs = Guid.NewGuid();
            get.gIDs = gIDs;
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                if (!string.IsNullOrEmpty(ProductTerm) && !string.IsNullOrEmpty(Cust_Term))
                {
                    ViewBag.Id = id;
                    cn.Open();
                    command.CommandText = @"IF NOT EXISTS ( SELECT 1 FROM [IflowSeed].[dbo].[StoreTerms] WHERE [ProductTerm] = @ProductTerm AND [Cust_Term] =@Cust_Term AND [Id] =@Id )
                    BEGIN INSERT INTO [IflowSeed].[dbo].[StoreTerms] (TermID,ProductTerm,Cust_Term,Id) 
                    VALUES (@TermID,@ProductTerm,@Cust_Term,@Id) END";

                    command.Parameters.AddWithValue("@TermID", get.gIDs);
                    if (get.ProductTerm == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@ProductTerm", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ProductTerm", get.ProductTerm);
                    }

                    if (get.Cust_Term == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@Cust_Term", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Cust_Term", get.Cust_Term);
                    }

                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                    cn.Close();

                }
                List<ListTableTerm> listTerm = new List<ListTableTerm>();
                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand cmd = new SqlCommand("", cn2))
                {
                    model.gID = get.gID;
                    cn2.Open();
                    cmd.CommandText = @"SELECT Cust_Term, ProductTerm, TermID  FROM [IflowSeed].[dbo].[StoreTerms]                          
                                      WHERE Id=@gID";


                    //command.Parameters.AddWithValue("@Cust_Term", Cust_Term.ToString());
                    cmd.Parameters.AddWithValue("@gID", gID.ToString()); //Id

                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        {
                            model.Cust_Term = reader.GetString(0);
                            listTerm.Add(new ListTableTerm { Cust_Term = reader.GetString(0), ProductTerm = reader.GetString(1), TermID = reader.GetGuid(2) });
                        }
                    }
                    cn2.Close();
                }
                //ViewData["ProductTerm_"] = li4;            
                //return Json(new { data =  });
                ViewBag.Cust_Term = listTerm;
                ViewBag.gID = gID;

            }


            ////return View(model);
            // }


            //materialTyoe
            if (!string.IsNullOrEmpty(deleteID))
            {
                try
                {
                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    using (SqlCommand command = new SqlCommand("", cn))
                    {
                        cn.Open();
                        command.CommandText = @"DELETE [IflowSeed].[dbo].[StoreMaterialCharges]                          
                                      WHERE gID = @gID";
                        command.Parameters.AddWithValue("@gID", deleteID.ToString());
                        command.ExecuteNonQuery();
                        cn.Close();

                    }
                }
                catch (Exception e)
                {


                    Console.WriteLine($"Generic Exception Handler: {e}");
                }


                return RedirectToAction("ViewQuo", "Quotation");
            }

            if (!string.IsNullOrEmpty(deleteID2))
            {
                try
                {
                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    using (SqlCommand command = new SqlCommand("", cn))
                    {
                        cn.Open();
                        command.CommandText = @"DELETE [IflowSeed].[dbo].[HistoryMaterialCharge]                          
                                            WHERE rateID = @gID";
                        command.Parameters.AddWithValue("@gID", deleteID2.ToString());
                        command.ExecuteNonQuery();
                        cn.Close();
                    }
                }
                catch (Exception e)
                {


                    Console.WriteLine($"Generic Exception Handler: {e}");
                }


                return RedirectToAction("ViewQuo", "Quotation");
            }

            if (string.IsNullOrEmpty(deleteID) || string.IsNullOrEmpty(deleteID2))
            {

                //new id for material type
                Guid gIDs3 = Guid.NewGuid();
                get.gIDMate = gIDs3; //

                string createdDateMaterial = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                get.CreatedOn = Convert.ToDateTime(createdDateMaterial);
                get.dateCreated = Convert.ToDateTime(createdDateMaterial);
                var IdentityName2 = @Session["Fullname"];
                int Tempvolume = 1;
                double tempRate2 = 0.0;
                double TotalRate = 0.0;
                if (Volume != "")
                {
                    Tempvolume = int.Parse(Volume);
                }
            

                string tempRate = "";
                if (NewRate != "")
                {
                    tempRate = NewRate;
                }
                else
                {
                    tempRate = get.Rate;

                }
                if (tempRate != null)
                {
                    tempRate2 = double.Parse(tempRate);
                    TotalRate = Tempvolume * tempRate2;
                }


                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    if (!string.IsNullOrEmpty(ItemCode) && !string.IsNullOrEmpty(MaterialType))
                    {

                        ViewBag.Id = id;
                        cn.Open();

                        command.CommandText = @"SELECT Description FROM [IflowSeed].[dbo].[MaterialCharges]" +
                         "WHERE Id = @IdTemp";

                        command.Parameters.AddWithValue("@IdTemp", DescID);
                        
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            descriptionVal = reader.GetString(0);

                            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                            using (SqlCommand command2 = new SqlCommand("", cn2))
                            {
                                cn2.Open();
                                //command2.CommandText = @"INSERT INTO [IflowSeed].[dbo].[StoreMaterialCharges] (gID,MaterialType,ItemCode,Description,Rate,Id,CreatedOn,CreatedBy) VALUES (@gID,@ItemCode,@MaterialType,@Description,@Rate,@Id,@CreatedOn,@CreatedBy)";
                                command2.CommandText =
                                @"IF NOT EXISTS 
                                    ( SELECT  1 FROM [IflowSeed].[dbo].[StoreMaterialCharges] 
                                    WHERE ItemCode = @ItemCode AND MaterialType = @MaterialType AND Description = @Description AND [Id] =@Id)
                                    BEGIN 
                                        INSERT INTO [IflowSeed].[dbo].[StoreMaterialCharges] (gID,MaterialType,ItemCode,Description,Rate,Id,CreatedOn,CreatedBy,Volume,TotalRate) 
                                        VALUES (@gID,@MaterialType,@ItemCode,@Description,@Rate,@Id,@CreatedOn,@CreatedBy,@Volume,@TotalRate) 
                                    END 
                                ELSE     
                                    BEGIN
                                        UPDATE [IflowSeed].[dbo].[StoreMaterialCharges] SET Rate=@Rate, Volume=@Volume, TotalRate=@TotalRate WHERE Description=@Description AND Id = @Id
                                    END";

                                command2.Parameters.AddWithValue("@gID", get.gIDMate);
                                if (get.MaterialType == null)
                                {
                                    command2.Parameters.Add(new SqlParameter { ParameterName = "@MaterialType", Value = DBNull.Value });
                                }
                                else
                                {
                                    command2.Parameters.AddWithValue("@MaterialType", get.MaterialType);
                                }
                                if (get.ItemCode == null)
                                {
                                    command2.Parameters.Add(new SqlParameter { ParameterName = "@ItemCode", Value = DBNull.Value });
                                }
                                else
                                {
                                    command2.Parameters.AddWithValue("@ItemCode", get.ItemCode);
                                }

                                if (descriptionVal == null)
                                {
                                    command2.Parameters.Add(new SqlParameter { ParameterName = "@Description", Value = DBNull.Value });
                                }
                                else
                                {
                                    command2.Parameters.AddWithValue("@Description", descriptionVal);
                                }

                                if (get.Rate == null)
                                {
                                    command2.Parameters.Add(new SqlParameter { ParameterName = "@Rate", Value = DBNull.Value });
                                }
                                else
                                {
                           
                                    command2.Parameters.AddWithValue("@Rate", tempRate);
                                }
                                command2.Parameters.AddWithValue("@Id", id.ToString());
                                command2.Parameters.AddWithValue("@CreatedOn", get.dateCreated);
                                command2.Parameters.AddWithValue("@CreatedBy", IdentityName2.ToString());
                                command2.Parameters.AddWithValue("@Volume", Tempvolume);
                                command2.Parameters.AddWithValue("@TotalRate", TotalRate.ToString());


                                command2.ExecuteNonQuery();
                                cn2.Close();
                            }

                        }

                        reader.Close();
                        command.ExecuteNonQuery();
                        cn.Close();


                    }
                }


                //update approval from mbd and fin
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    Guid guid = new Guid(gID);
                    cn.Open();
                    command.CommandText = @"UPDATE  [IflowSeed].[dbo].[Quotation]
                                            SET Approve_Fin = @Approve_Fin, Approve_MBD = @Approve_MBD 
                                            WHERE gID = @id";

                    command.Parameters.AddWithValue("@id", guid);
                    command.Parameters.AddWithValue("@Approve_Fin", "0");
                    command.Parameters.AddWithValue("@Approve_MBD", "0");
                    command.ExecuteNonQuery();
                    cn.Close();
                }
                //

                //newPriceRate
                if (!string.IsNullOrEmpty(ItemCode) && !string.IsNullOrEmpty(MaterialType))
                {
                    //String descriptionVal;
                    Guid rateId = Guid.NewGuid();
                    get.rateID = rateId;

                    var IdentityName = @Session["Fullname"];



                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    using (SqlCommand command = new SqlCommand("", cn))
                    {
                        ViewBag.Id = id;
                        cn.Open();


                        command.CommandText = @"SELECT Description FROM [IflowSeed].[dbo].[MaterialCharges]" +
                         "WHERE Id = @IdTemp";

                        command.Parameters.AddWithValue("@IdTemp", DescID);

                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            descriptionVal = reader.GetString(0);

                            using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                            using (SqlCommand command3 = new SqlCommand("", cn3))
                            {

                                string createdDateRate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                                get.CreatedOn = Convert.ToDateTime(createdDateRate);
                                get.dateCreated = Convert.ToDateTime(createdDateRate);

                                cn3.Open();                                
                                command3.CommandText = @"IF NOT EXISTS ( SELECT  1 FROM [IflowSeed].[dbo].[HistoryMaterialCharge] WHERE [ItemCode] = @ItemCode AND [MaterialType] = @MaterialType AND [Description] = @Description AND Id = @Id AND Rate = @Rate)
                                BEGIN INSERT INTO [IflowSeed].[dbo].[HistoryMaterialCharge] (rateID,ProductType,MaterialType,ItemCode,Description,Rate,Status,Id,CreatedBy,CreatedOn,Volume, TotalRate) 
                                VALUES (@rateID,@ProductType,@MaterialType,@ItemCode,@Description,@Rate,@Status,@Id,@CreatedBy,@CreatedOn,@Volume,@TotalRate)END";                          
                                command3.Parameters.AddWithValue("@rateID", get.rateID);

                                if (get.ProductType == null)
                                {
                                    command3.Parameters.Add(new SqlParameter { ParameterName = "@ProductType", Value = DBNull.Value });
                                }
                                else
                                {
                                    command3.Parameters.AddWithValue("@ProductType", get.ProductType);
                                }

                                if (get.MaterialType == null)
                                {
                                    command3.Parameters.Add(new SqlParameter { ParameterName = "@MaterialType", Value = DBNull.Value });
                                }
                                else
                                {
                                    command3.Parameters.AddWithValue("@MaterialType", get.MaterialType);
                                }

                                if (get.ItemCode == null)
                                {
                                    command3.Parameters.Add(new SqlParameter { ParameterName = "@ItemCode", Value = DBNull.Value });
                                }
                                else
                                {
                                    command3.Parameters.AddWithValue("@ItemCode", get.ItemCode);
                                }

                                if (descriptionVal == null)
                                {
                                    command3.Parameters.Add(new SqlParameter { ParameterName = "@Description", Value = DBNull.Value });
                                }
                                else
                                {
                                    command3.Parameters.AddWithValue("@Description", descriptionVal);
                                }
                                
                                command3.Parameters.AddWithValue("@Rate", tempRate);                               
                                command3.Parameters.AddWithValue("@Status", "Pending");
                                command3.Parameters.AddWithValue("@Id", id.ToString());
                                command3.Parameters.AddWithValue("@CreatedBy", IdentityName.ToString());
                                command3.Parameters.AddWithValue("@CreatedOn", get.dateCreated);
                                command3.Parameters.AddWithValue("@Volume", Tempvolume);
                                command3.Parameters.AddWithValue("@TotalRate", TotalRate.ToString());
                                //createdDateRate

                                command3.ExecuteNonQuery();
                                cn3.Close();
                            }
                        }
                        reader.Close();
                        command.ExecuteNonQuery();
                        cn.Close();
                    }

                    List<ListNewRate> listNewRate2 = new List<ListNewRate>();
                    using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    using (SqlCommand cmd = new SqlCommand("", cn2))
                    {
                        model.gID = get.gID;

                        cn2.Open();
                        cmd.CommandText = @"SELECT MaterialType, Description, Rate, rateID, CreatedOn, Volume, TotalRate FROM [IflowSeed].[dbo].[HistoryMaterialCharge]                          
                                          WHERE Id=@gID ORDER BY CreatedOn desc";


                        //command.Parameters.AddWithValue("@Cust_Term", Cust_Term.ToString());
                        cmd.Parameters.AddWithValue("@gID", id.ToString()); //Id

                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            {
                                listNewRate2.Add(new ListNewRate { MaterialType = reader.GetString(0), Description = reader.GetString(1), Rate = reader.GetString(2), rateID = reader.GetGuid(3), CreatedOn = reader.GetDateTime(4), Volume = reader.GetInt32(5), TotalRate = reader.GetString(6)});
                            }
                        }
                        cn2.Close();
                    }


                }

            }
            List<ListNewRate> listNewRate = new List<ListNewRate>();
            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand cmd = new SqlCommand("", cn2))
            {
                model.gID = get.gID;
                cn2.Open();
                cmd.CommandText = @"SELECT MaterialType, Description, Rate, rateID, CreatedOn, Volume, TotalRate FROM [IflowSeed].[dbo].[HistoryMaterialCharge]                          
                                          WHERE Id=@gID ORDER BY CreatedOn desc";


                //command.Parameters.AddWithValue("@Cust_Term", Cust_Term.ToString());
                cmd.Parameters.AddWithValue("@gID", gID.ToString()); //Id

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    {
                        listNewRate.Add(new ListNewRate { MaterialType = reader.GetString(0), Description = reader.GetString(1), Rate = reader.GetString(2), rateID = reader.GetGuid(3), 
                            CreatedOn = reader.GetDateTime(4), Volume = reader.GetInt32(5), TotalRate = reader.GetString(6)
                        });
                    }
                }
                cn2.Close();
            }
            ViewBag.TblePriceRate = listNewRate;

            List<ListTable> listTemp = new List<ListTable>();
            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn2))
            {
                cn2.Open();
                command.CommandText = @"SELECT MaterialType, Description, Rate, gID, Volume, TotalRate FROM [IflowSeed].[dbo].[StoreMaterialCharges]                               
                                           WHERE Id=@id
                                           ORDER BY MaterialType";

                command.Parameters.AddWithValue("@id", gID.ToString());

                var reader = command.ExecuteReader();
                QuotationModel model3 = new QuotationModel();
                {
                    while (reader.Read())
                    {


                        model3.gID = get.gID;
                        //if (reader.IsDBNull(0) == false)
                        //{
                        MaterialType = reader.GetString(0);
                        Description = reader.GetString(1);
                        Rate = reader.GetString(2);
                        listTemp.Add(new ListTable { MaterialType = MaterialType, Description = Description, Rate = Rate, gID = reader.GetGuid(3), Volume = reader.GetInt32(4), TotalRate = reader.GetString(5)});

                    }
                    cn2.Close();

                }
                ViewBag.TbleMaterial = listTemp;
            }

            if (set == "selectAddTerm" && ProductType != "Please Select")
            {
                //create id for cust term
                Guid gIDs2 = Guid.NewGuid();
                get.gID = gIDs2;

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    ViewBag.Id = id;

                    cn.Open();
                    command.CommandText = @"INSERT INTO [IflowSeed].[dbo].[StoreTerms] (gID,ProductTerm,Cust_Term,Id) VALUES (@gID,@ProductTerm @Cust_Term,@Id)";

                    command.Parameters.AddWithValue("@gID", get.gID);
                    if (get.ProductType == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@ProductType", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ProductTerm", get.ProductTerm);
                    }
                    if (get.Terms == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@Cust_Term", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Cust_Term", get.Terms);
                    }
                    command.Parameters.AddWithValue("@Id", id.ToString());
                    command.ExecuteNonQuery();
                    cn.Close();

                }
            }

            ViewBag.IsDepart = @Session["Department"];
            Department = ViewBag.IsDepart;

            //uID
            ViewBag.uID = Session["Idx"].ToString();
            uID = ViewBag.uID;

            //gID
            @Session["Id"] = id;

            ViewBag.gID = gID.ToString();
            //ViewBag.gID = get.gID;
            @Session["gID"] = ViewBag.gID;
            string createdDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            get.CreatedOn = Convert.ToDateTime(createdDate);
            get.dateCreated = Convert.ToDateTime(createdDate);

            ViewBag.Display = 1;

            if (get.gID == Guid.Empty)
            {
                Guid gid = Guid.NewGuid();
                get.gID = gid;


                string user = Session["FullName"].ToString();
                string position = Session["Role"].ToString();
                get.createdBy = user.ToString();
                get.position = position.ToString();

                //upagain:


                /////////////////////////////////////
                if (get.PreparedBy != "Please Select" && !string.IsNullOrEmpty(get.PreparedBy))
                {
                    //PREPARED BY
                    using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    using (SqlCommand command = new SqlCommand("", cn2))
                    {
                        cn2.Open();
                        command.CommandText = @"SELECT Role,Email FROM [IflowSeed].[dbo].[User]                               
                                      WHERE Fullname=@Fullname";
                        command.Parameters.AddWithValue("@Fullname", get.PreparedBy.ToString());
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(0) == false)
                            {
                                PreparedPos = reader.GetString(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                PreparedEmail = reader.GetString(1);
                            }
                        }
                        cn2.Close();
                    }
                }

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    //Viewback Each Position & Email
                    ViewBag.PreparedPos = PreparedPos;
                    ViewBag.PreparedEmail = PreparedEmail;

                    string DateTxt = DateTime.Now.ToString("yyyy-MM-dd");

                    var CurrNo = new GeneralLetterModel();

                    cn.Open();
                    command.CommandText = @"INSERT INTO [IflowSeed].[dbo].[Quotation]
                                           (gID,CreatedOn,Customer_Name,reffNo,Address1,Address2,Address3,Cust_Postcode,Cust_State,attName,salutation,subject,descr,createdBy,position,dateCreate,reffSub,Status,Department,IsJobCompleted,CC2_name,CC2_position,CC2_mobile,CC2_email,CC1_name,CC1_position,CC1_mobile,CC1_email,Approve_Fin,Approve_MBD,RemarkReject)" +
                                            "VALUES (@gID,@CreatedOn, @Customer_Name,@reffNo, @Address1, @Address2, @Address3, @Cust_Postcode, @Cust_State, @attName, @salutation, @subject, @descr, @createdBy, @position, @dateCreate,@reffSub,@Status,@Department,@IsJobCompleted,@CC2_name,@CC2_position,@CC2_mobile,@CC2_email,@CC1_name,@CC1_position,@CC1_mobile,@CC1_email,@Approve_Fin,@Approve_MBD,@RemarkReject)";
                    command.Parameters.AddWithValue("@gID", get.gID);
                    command.Parameters.AddWithValue("@CreatedOn", get.CreatedOn);
                    command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                    command.Parameters.AddWithValue("@reffNo", CurrNo.RefNo);
                    if (get.Address1 == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@Address1", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Address1", get.Address1);
                    }

                    if (get.Address2 == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@Address2", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Address2", get.Address2);
                    }

                    if (get.Address3 == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@Address3", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Address3", get.Address3);
                    }

                    if (get.Cust_Postcode == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@Cust_Postcode", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Cust_Postcode", get.Cust_Postcode);
                    }

                    if (get.Cust_State == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@Cust_State", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Cust_State", get.Cust_State);
                    }

                    if (get.attName == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@attName", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@attName", get.attName);
                    }

                    if (get.salutation == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@salutation", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@salutation", get.salutation);
                    }

                    if (get.subject == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@subject", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@subject", get.subject);
                    }

                    if (get.descr == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@descr", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@descr", get.descr);
                    }

                    command.Parameters.AddWithValue("@createdBy", get.createdBy);
                    command.Parameters.AddWithValue("@position", get.position);
                    command.Parameters.AddWithValue("@dateCreate", get.dateCreated);
                    command.Parameters.AddWithValue("@reffSub", CurrNo.RefNo);


                    command.Parameters.AddWithValue("@Status", "New");
                    command.Parameters.AddWithValue("@Department", Department);
                    command.Parameters.AddWithValue("@IsJobCompleted", false);
                    command.Parameters.AddWithValue("@CC2_name", CC2_name);
                    command.Parameters.AddWithValue("@CC2_position", CC2_position);
                    command.Parameters.AddWithValue("@CC2_mobile", CC2_mobile);
                    command.Parameters.AddWithValue("@CC2_email", CC2_email);

                    command.Parameters.AddWithValue("@CC1_name", CC1_name);
                    command.Parameters.AddWithValue("@CC1_position", CC1_position);
                    command.Parameters.AddWithValue("@CC1_mobile", CC1_mobile);
                    command.Parameters.AddWithValue("@CC1_email", CC1_email);
                    command.Parameters.AddWithValue("@Approve_Fin", false);
                    command.Parameters.AddWithValue("@Approve_MBD", false);
                    command.Parameters.AddWithValue("@RemarkReject", false);                                       
                    command.ExecuteNonQuery();
                    cn.Close();


                }
                return RedirectToAction("ManageQuotation", "Quotation");
            }
            else
            {
                if (get.PreparedBy != "Please Select" && !string.IsNullOrEmpty(get.PreparedBy))
                {
                    //PREPARED BY
                    using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    using (SqlCommand command = new SqlCommand("", cn2))
                    {
                        cn2.Open();
                        command.CommandText = @"SELECT Role,Email FROM [iflowSeed].[dbo].[User]                               
                                      WHERE Fullname=@Fullname";
                        command.Parameters.AddWithValue("@Fullname", get.PreparedBy.ToString());
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(0) == false)
                            {
                                PreparedPos = reader.GetString(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                PreparedEmail = reader.GetString(1);
                            }
                        }
                        cn2.Close();
                    }
                }



                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    //Viewback Each Position & Email
                    ViewBag.PreparedPos = PreparedPos;
                    ViewBag.PreparedEmail = PreparedEmail;

                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                    string DateTxt = DateTime.Now.ToString("yyyy-MM-dd");

                    cn.Open();
                    command.CommandText = @"UPDATE [IflowSeed].[dbo].[Quotation]
                                            SET Customer_Name = @Customer_Name ,Address1 = @Address1, Address2 = @Address2, Address3 = @Address3, Cust_Postcode = @Cust_Postcode, Cust_State = @Cust_State, attName = @attName, subject = @subject, descr = @descr, CC1_name = @CC1_name, CC2_name = @CC2_name, CC1_position = @CC1_position, CC2_position = @CC2_position, CC1_mobile = @CC1_mobile, CC2_mobile = @CC2_mobile, CC1_email = @CC1_email, CC2_email = @CC2_email
                                            WHERE gID = @gID";
                    command.Parameters.AddWithValue("@gID", get.gID);
                    command.Parameters.AddWithValue("@Customer_Name", get.Customer_Name);

                    if (get.Address1 == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@Address1", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Address1", get.Address1);
                        ViewBag.Address1 = get.Address1;
                    }

                    if (get.Address2 == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@Address2", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Address2", get.Address2);
                        ViewBag.Address2 = get.Address2;
                    }

                    if (get.Address3 == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@Address3", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Address3", get.Address3);
                        ViewBag.Address3 = get.Address3;
                    }

                    if (get.Cust_Postcode == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@Cust_Postcode", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Cust_Postcode", get.Cust_Postcode);
                        ViewBag.Cust_Postcode = get.Cust_Postcode;
                    }

                    if (get.Cust_State == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@Cust_State", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Cust_State", get.Cust_State);
                        ViewBag.Cust_State = get.Cust_State;
                    }

                    if (get.attName == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@attName", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@attName", get.attName);
                        ViewBag.attName = get.attName;
                    }

                    if (get.salutation == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@salutation", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@salutation", get.salutation);
                        ViewBag.salutation = get.salutation;
                    }

                    if (get.subject == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@subject", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@subject", get.subject);
                        ViewBag.subject = get.subject;
                    }

                    if (get.descr == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@descr", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@descr", get.descr);
                        ViewBag.descr = get.descr;
                    }
                    if (get.CC1_name == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@CC1_name", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@CC1_name", get.CC1_name);
                        ViewBag.CC1_Name = get.CC1_name;
                    }

                    if (get.CC2_name == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@CC2_name", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@CC2_name", get.CC2_name);
                        ViewBag.CC2_name = get.CC2_name;
                    }

                    if (get.CC1_position == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@CC1_position", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@CC1_position", get.CC1_position);
                        ViewBag.CC1_position = get.CC1_position;
                    }

                    if (get.CC2_position == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@CC2_position", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@CC2_position", get.CC2_position);
                        ViewBag.CC2_position = get.CC2_position;
                    }

                    if (get.CC1_mobile == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@CC1_mobile", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@CC1_mobile", get.CC1_mobile);
                        ViewBag.CC1_mobile = get.CC1_mobile;
                    }

                    if (get.CC2_mobile == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@CC2_mobile", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@CC2_mobile", get.CC2_mobile);
                        ViewBag.CC2_mobile = get.CC2_mobile;
                    }

                    if (get.CC1_email == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@CC1_email", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@CC1_email", get.CC1_email);
                        ViewBag.CC1_email = get.CC1_email;
                    }

                    if (get.CC2_email == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@CC2_email", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@CC2_email", get.CC2_email);
                        ViewBag.CC2_email = get.CC2_email;
                    }
                  

                    command.ExecuteNonQuery();
                    cn.Close();

                    ViewBag.id = gID;
                    ViewBag.Id = gID;
                    
                   
                    ViewBag.AWDStat = "Update";

                }

                //return RedirectToAction("ManageQuotation", "Quotation");
                return View();

            }

        }

        //create pdf
        List<JobAuditTrail> viewQuo = new List<JobAuditTrail>();

        [AllowAnonymous]
        public ActionResult PrintQuotation(String Id, String Customer_Name, String reffSub, String Address1, String Address2, String Address3, String Cust_Postcode, String Cust_State, String subject, String descr)
        {
            //string ItemCode;
            //string Description;
            //string Rate;
            ViewBag.IsDepart = @Session["Department"];
            ViewBag.IsRole = @Session["Role"];
            var IdentityName = @Session["Fullname"];
            int bil2 = 0;

            List<ListTable> listTemp = new List<ListTable>();
            //List<ListTableTerm> listTemp2 = new List<ListTableTerm>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Quotation.gID, Quotation.Customer_Name, Quotation.reffSub, Quotation.Address1, 
                                    Quotation.Address2, Quotation.Address3, Quotation.Cust_Postcode, Quotation.Cust_State, Quotation.subject, 
                                    Quotation.descr, StoreMaterialCharges.MaterialType, StoreMaterialCharges.Description, StoreMaterialCharges.Rate, 
                                    Quotation.CC1_name, Quotation.CC1_position, Quotation.CC1_mobile, Quotation.CC1_email, Quotation.CC2_name, 
                                    Quotation.CC2_position, Quotation.CC2_mobile, Quotation.CC2_email, Quotation.dateCreate, StoreMaterialCharges.Volume, StoreMaterialCharges.TotalRate, Quotation.attName
                                    FROM [IflowSeed].[dbo].[Quotation]
                                    INNER JOIN [IflowSeed].[dbo].[StoreMaterialCharges] ON Quotation.gID = TRY_CONVERT(uniqueidentifier, StoreMaterialCharges.Id)                                        
                                    WHERE Quotation.gID = @Id";

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
                        ViewBag.reffSub = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        ViewBag.Address1 = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        ViewBag.Address2 = reader.GetString(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        ViewBag.Address3 = reader.GetString(5);
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        ViewBag.Cust_Postcode = reader.GetString(6);
                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        ViewBag.Cust_State = reader.GetString(7);
                    }
                    if (reader.IsDBNull(8) == false)
                    {
                        ViewBag.subject = reader.GetString(8);
                    }
                    if (reader.IsDBNull(9) == false)
                    {
                        ViewBag.descr = reader.GetString(9);
                    }
                    if (reader.IsDBNull(10) == false)
                    {
                        ViewBag.MaterialType = reader.GetString(10);
                    }
                    if (reader.IsDBNull(11) == false)
                    {
                        ViewBag.Description = reader.GetString(11);
                    }
                    if (reader.IsDBNull(12) == false)
                    {
                        ViewBag.Rate = reader.GetString(12);
                    }
                    if (reader.IsDBNull(13) == false)
                    {
                        ViewBag.CC1_name = reader.GetString(13);
                    }
                    if (reader.IsDBNull(14) == false)
                    {
                        ViewBag.CC1_position = reader.GetString(14);
                    }
                    if (reader.IsDBNull(15) == false)
                    {
                        ViewBag.CC1_mobile = reader.GetString(15);
                    }
                    if (reader.IsDBNull(16) == false)
                    {
                        ViewBag.CC1_email = reader.GetString(16);
                    }
                    if (reader.IsDBNull(17) == false)
                    {
                        ViewBag.CC2_name = reader.GetString(17);
                    }
                    if (reader.IsDBNull(18) == false)
                    {
                        ViewBag.CC2_position = reader.GetString(18);
                    }
                    if (reader.IsDBNull(19) == false)
                    {
                        ViewBag.CC2_mobile = reader.GetString(19);
                    }
                    if (reader.IsDBNull(20) == false)
                    {
                        ViewBag.CC2_email = reader.GetString(20);
                    }
                    if (reader.IsDBNull(21) == false)
                    {
                        ViewBag.dateCreate = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(21));
                    }
                    if (reader.IsDBNull(22) == false)
                    {
                        ViewBag.Volume = reader.GetInt32(22);
                    }
                    if (reader.IsDBNull(23) == false)
                    {
                        ViewBag.TotalRate = reader.GetString(23);
                    }
                    if (reader.IsDBNull(24) == false)
                    {
                        ViewBag.attName = reader.GetString(24);
                    }

                    bil2 = bil2 + 1;
                    listTemp.Add(new ListTable { MaterialType = reader.GetString(10), Description = reader.GetString(11), Rate = reader.GetString(12), bil2 = bil2, Volume = reader.GetInt32(22), TotalRate = reader.GetString(23) });
                }
                cn.Close();

                ViewBag.Id = Id;

                //
                List<ListTableTerm> listTerm2 = new List<ListTableTerm>();
                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand cmd = new SqlCommand("", cn2))
                {
                    //model.gID = get.gID;
                    cn2.Open();
                    cmd.CommandText = @"SELECT Cust_Term, Id  FROM [IflowSeed].[dbo].[StoreTerms]                          
                                      WHERE Id = @Id";

                    cmd.Parameters.AddWithValue("@Id", Id);
                    var reader2 = cmd.ExecuteReader();
                    while (reader2.Read())
                    {
                        {
                            QuotationModel model = new QuotationModel();
                            model.Cust_Term = reader2.GetString(0);
                            listTerm2.Add(new ListTableTerm { Cust_Term = reader2.GetString(0) });
                        }
                    }
                    cn2.Close();
                }

                ViewBag.TbleMaterial = listTemp;
                ViewBag.Cust_Term = listTerm2;


                //ReloadJAT(Id);
                var header = Server.MapPath("~/Static/Header.html");
                // var footer = Server.MapPath("~/Static/Footer.html#pagetext=Page&oftext=Of");

                string customSwitches = string.Format("--header-html \"{0}\" " +
                    "--header-spacing \"15\" " +
                    //"--footer-html \"{1}\" " +
                    //"--footer-spacing \"10\" " +
                    //"--footer-font-size \"10\" " +
                    "--header-font-size \"10\" ", header);

                return new Rotativa.ViewAsPdf("PrintQuotation", "Quotation")
                {
                    // FileName = flightPlan.ListingItemDetailsModel.FlightDetails + ".pdf",
                  
                    PageMargins = new Rotativa.Options.Margins(20, 5, 20, 5),
                    //CustomSwitches = customSwitches,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageSize = Rotativa.Options.Size.A4,
                    //CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
                    //PageWidth = 210,
                    //PageHeight = 297
                };
            }
        }


        String Gid; Guid gIDs; String custN; String add1; String add2; String add3; String postcode; String custState; String sub; string reffSub; string descr;
        private void CreatePDF(String id, String custName)
        {
            gIDs = Guid.NewGuid();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                try
                {
                    CustomerDetails model = new CustomerDetails();

                    //@"SELECT Customer_Name,reffSub,dateCreate,createdBy,reffNo,gID,subject,Status
                    //FROM[IflowSeed].[dbo].[Quotation]
                    command.CommandText = @"SELECT  gID, Customer_Name, Address1, Address2, Address3, Cust_Postcode, Cust_State, Subject, reffSub, descr FROM [IflowSeed].[dbo].[Quotation]                                     
                                            WHERE gID = @id ";
                    command.Parameters.AddWithValue("@id", id.ToString());


                    //var reader = command.ExecuteReader();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(0) == false)
                            {
                                gIDs = reader.GetGuid(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                custN = reader.GetString(1);
                            }
                            if (reader.IsDBNull(2) == false)
                            {
                                add1 = reader.GetString(2);
                            }
                            if (reader.IsDBNull(3) == false)
                            {
                                add2 = reader.GetString(3);
                            }
                            if (reader.IsDBNull(4) == false)
                            {
                                add3 = reader.GetString(4);
                            }
                            if (reader.IsDBNull(5) == false)
                            {
                                postcode = reader.GetString(5);
                            }
                            if (reader.IsDBNull(6) == false)
                            {
                                custState = reader.GetString(6);
                            }
                            if (reader.IsDBNull(7) == false)
                            {
                                sub = reader.GetString(7);
                            }
                            if (reader.IsDBNull(8) == false)
                            {
                                reffSub = reader.GetString(8);
                            }
                            if (reader.IsDBNull(9) == false)
                            {
                                descr = reader.GetString(9);
                            }
                            CreateConsignPDF(gIDs.ToString(), custN, add1, add2, add3, postcode, custState, sub, reffSub, descr);
                        }

                    }
                    reader.Close();
                }
                catch (System.Exception err)
                {
                    string adaerror = err.Message;
                }
                finally
                {
                    cn.Close();
                }

            }

        }
        public int paksiy;
        private void CreateConsignPDF(String namapdf, string custN, string adds1, string adds2, string adds3, string pCode, string cState, String subs, string reffSub, string descr)
        {

            //string dd = Session["Role"].ToString();
            ViewBag.IsRole = @Session["Role"];

            string fileDir = Path.Combine(Server.MapPath("~/PdfQoutation/"), namapdf.ToString());
            if (System.IO.Directory.Exists(fileDir) == false)
            {
                System.IO.Directory.CreateDirectory(fileDir);
            }
            string CombineAll = Path.Combine(fileDir, namapdf + ".pdf");

            string oldFile = Server.MapPath("~/Images/SampleQR.pdf"); //pdf kosng
            //string filename = Server.MapPath("~/Images/Dolist 2_page-0001.jpg"); //gmbr table(design pdf)

            PdfReader reader = new PdfReader(oldFile);
            iTextSharp.text.Rectangle size = reader.GetPageSizeWithRotation(1);
            Document document = new Document(size);

            FileStream fs = new FileStream(CombineAll, FileMode.Create, FileAccess.Write);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            //var image = iTextSharp.text.Image.GetInstance(filename);
            //image.SetAbsolutePosition(10, 0);
            //image.ScaleAbsoluteHeight(document.PageSize.Height);
            //image.ScaleAbsoluteWidth(document.PageSize.Width);
            //document.Add(image);

            // the pdf content
            //iTextSharp.text.Font.UNDERLINE



            PdfContentByte cb = writer.DirectContent;
            BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb.SetFontAndSize(bf, 12);

            PdfContentByte cb2 = writer.DirectContent;
            BaseFont bf2 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb.SetFontAndSize(bf2, 8);

            PdfContentByte cb3 = writer.DirectContent;
            BaseFont bf3 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb3.SetFontAndSize(bf3, 8);

            PdfContentByte cb4 = writer.DirectContent;
            BaseFont bf4 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb3.SetFontAndSize(bf4, 8);

            BaseFont ArialBoldUnder10 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fontABU10 = new Font(ArialBoldUnder10, 10, Font.UNDERLINE);

            cb.BeginText();
            cb.ShowTextAligned(0, "Our Ref		: " + reffSub, 40, 680, 0);
            cb.EndText();

            cb.BeginText();
            cb.ShowTextAligned(0, "Date		: ", 40, 670, 0);
            cb.EndText();

            //PdfContentByte abc = writer.DirectContent;
            //abc.BeginText();
            //abc.SetFontAndSize(ArialBoldUnder10, 8);               
            //abc.ShowTextAligned(0, "DRB-HICOM EZ-DRIVE SDN BHD (12799-K)", 374, 729, 0);       
            //abc.EndText();


            cb2.BeginText();
            paksiy = 650;
            if (adds1 != null)
            {
                //cb2.BeginText();
                cb2.ShowTextAligned(0, adds1, 40, paksiy, 0);
                //cb2.EndText();
            }

            if (adds2 != null)
            {
                paksiy = paksiy - 10;
                //cb2.BeginText();
                cb2.ShowTextAligned(0, adds2, 40, paksiy, 0);
                //cb2.EndText();
            }

            if (adds3 != null)
            {
                paksiy = paksiy - 10;
                cb2.ShowTextAligned(0, adds3, 40, paksiy, 0);
            }

            if (pCode != null && cState != null)
            {
                paksiy = paksiy - 10;
                cb2.ShowTextAligned(0, pCode + " " + cState, 40, paksiy, 0);
            }
            cb2.EndText();

            cb2.BeginText();
            cb2.ShowTextAligned(0, "Dear Sir/Madam,", 40, 550, 0);
            cb2.EndText();

            cb2.BeginText();
            cb2.ShowTextAligned(0, subs, 40, 520, 0);
            cb2.EndText();

            cb2.BeginText();
            cb2.ShowTextAligned(0, descr, 40, 500, 0);
            cb2.EndText();


            //PdfPTable titleSpace = new PdfPTable(1);
            //titleSpace.SpacingBefore = 2000;
            //titleSpace.TotalWidth = 538;
            //titleSpace.DefaultCell.FixedHeight = 300f;
            //titleSpace.LockedWidth = true;
            //titleSpace.DefaultCell.Border = Rectangle.NO_BORDER;
            //float[] widthsS = new float[] { 14f }; //colom width
            //titleSpace.SetWidths(widthsS);
            //Paragraph ptspace = new Paragraph();
            //Phrase phspce = new Phrase("\n", cb2);
            //ptspace.Add(phspce);
            //titleSpace.AddCell(ptspace);
            //pdfDoc.Add(titleSpace);

            PdfImportedPage page = writer.GetImportedPage(reader, 1);
            cb.AddTemplate(page, 0, 0);

            // close the streams and voilá the file should be changed :)
            document.Close();
            fs.Close();
            writer.Close();
            reader.Close();
        }


        [HttpPost]
        public ActionResult ProType(String ProductType)
        {
            String temp = "0";
            int _bildd = 1;
            List<SelectListItem> li2 = new List<SelectListItem>();
            li2.Clear();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT DISTINCT MaterialType FROM [IflowSeed].[dbo].[MaterialCharges]                          
                                      WHERE ProductType = @ProductType";
                command.Parameters.AddWithValue("@ProductType", ProductType.ToString());


                //
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    QuotationModel model = new QuotationModel();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            model.MaterialType = reader.GetString(0);
                        }
                    }
                    int i = _bildd++;
                    if (i == 1)
                    {
                        li2.Add(new SelectListItem { Text = "Please Select" });
                        li2.Add(new SelectListItem { Text = model.MaterialType });

                    }
                    else
                    {
                        li2.Add(new SelectListItem { Text = model.MaterialType });
                    }
                    ViewBag.ProductType = model.MaterialType;
                    temp = model.MaterialType;
                }
                cn.Close();
            }
            return Json(new { data = li2 });
        }
        public ActionResult test(String MaterialType)
        {
            String temp = "0";
            int _bildd = 1;
            List<SelectListItem> li2 = new List<SelectListItem>();
            li2.Clear();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT DISTINCT ItemCode FROM [IflowSeed].[dbo].[MaterialCharges]                          
                                      WHERE MaterialType = @MaterialType";
                command.Parameters.AddWithValue("@MaterialType", MaterialType.ToString());


                //
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    QuotationModel model = new QuotationModel();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            model.ItemCode = reader.GetString(0);
                        }
                    }
                    int i = _bildd++;
                    if (i == 1)
                    {
                        li2.Add(new SelectListItem { Text = "Please Select" });
                        li2.Add(new SelectListItem { Text = model.ItemCode });

                    }
                    else
                    {
                        li2.Add(new SelectListItem { Text = model.ItemCode });
                    }
                    ViewBag.MaterialType = model.ItemCode;
                    temp = model.ItemCode;
                }
                cn.Close();
            }
            return Json(new { data = li2 });
        }

        [HttpPost]
        public ActionResult test2(String ItemCode, String MaterialType)
        {
            String temp = "0";
            int _bildd = 1;
            List<MaterialCharges> li2 = new List<MaterialCharges>();
            // List<Guid> li2 = new List<Guid>();
            li2.Clear();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Id, Description FROM [IflowSeed].[dbo].[MaterialCharges]                          
                                     WHERE ItemCode = @ItemCode AND MaterialType = @MaterialType";
                command.Parameters.AddWithValue("@ItemCode", ItemCode.ToString());
                command.Parameters.AddWithValue("@MaterialType", MaterialType.ToString());



                //
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    QuotationModel model = new QuotationModel();
                    {
                        //if (reader.IsDBNull(0) == false)
                        //{
                        //model.Description = reader.GetString(0);
                        //model.Rate = reader.GetString(1);
                        //}
                    }
                    int i = _bildd++;
                    if (i == 1)
                    {
                        li2.Add(new MaterialCharges { Description = "Please Select" });
                        li2.Add(new MaterialCharges { Description = reader.GetString(1), Id = reader.GetGuid(0) });

                    }
                    else
                    {
                        li2.Add(new MaterialCharges { Description = reader.GetString(1), Id = reader.GetGuid(0) });
                    }
                    ViewBag.ItemCode = model.ItemCode;
                    temp = model.ItemCode;
                }
                cn.Close();
            }
            //ViewData["MaterialType_"] = li2;            
            return Json(new { data = li2 });
        }
        [HttpPost]
        public ActionResult test3(String Description)
        {
            var temp = "";
            int _bildd = 1;
            List<SelectListItem> li2 = new List<SelectListItem>();

            li2.Clear();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT  Rate FROM [IflowSeed].[dbo].[MaterialCharges]                          
                                     WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", Description.ToString());


                //
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    QuotationModel model = new QuotationModel();
                    {
                        //if (reader.IsDBNull(0) == false)
                        //{
                        model.Description = reader.GetString(0);
                        // model.Rate = reader.GetString(1);
                        //}
                    }


                    temp = reader.GetString(0);


                }
                cn.Close();
            }
            //ViewData["MaterialType_"] = li2;            
            return Json(new { data = temp });
        }
        public ActionResult terms(String ProductTerm)
        {
            String temp = "0";
            int _bildd = 1;
            List<SelectListItem> li2 = new List<SelectListItem>();
            li2.Clear();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT DISTINCT Cust_Term FROM [IflowSeed].[dbo].[QouStoreTerm]                          
                                      WHERE ProductTerm = @ProductTerm";
                command.Parameters.AddWithValue("@ProductTerm", ProductTerm.ToString());


                //
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    QuotationModel model = new QuotationModel();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            model.Cust_Term = reader.GetString(0);
                        }
                    }
                    int i = _bildd++;
                    if (i == 1)
                    {
                        li2.Add(new SelectListItem { Text = "Please Select" });
                        li2.Add(new SelectListItem { Text = model.Cust_Term });

                    }
                    else
                    {
                        li2.Add(new SelectListItem { Text = model.Cust_Term });
                    }
                    //ViewBag.ProductTerm = model.Cust_Term;
                    temp = model.Cust_Term;
                }
                cn.Close();
            }

            return Json(new { data = li2, ProductTerm });
        }

        [HttpPost]
        public ActionResult terms2(String Cust_Term, String ProductTerm)
        {
            String temp = "0";
            int _bildd = 1;
            List<QuotationModel> li2 = new List<QuotationModel>();
            // List<Guid> li2 = new List<Guid>();
            li2.Clear();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Cust_Term FROM [IflowSeed].[dbo].[QouStoreTerm]                          
                                     WHERE ProductTerm = @ProductTerm";
                //command.Parameters.AddWithValue("@Cust_Term", Cust_Term.ToString());
                command.Parameters.AddWithValue("@ProductTerm", ProductTerm.ToString());


                //
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    QuotationModel model = new QuotationModel();
                    {
                        model.Cust_Term = reader.GetString(0);
                    }
                    temp = model.Cust_Term;
                }
                cn.Close();
            }
            //ViewData["MaterialType_"] = li2;            
            //return Json(new { data =  });
            ViewBag.Cust_Term = li2;
            return RedirectToAction("ViewQuo", "Quotation");
            //return View();            
        }
        public ActionResult DeleteRowTable(String gID)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    cn.Open();
                    command.CommandText = @"DELETE [IflowSeed].[dbo].[StoreMaterialCharges]                          
                                      WHERE gID = @gID";
                    command.Parameters.AddWithValue("@gID", gID.ToString());
                    command.ExecuteNonQuery();
                    cn.Close();
                    return Json(new { code = 0 });
                }
            }
            catch (Exception e)
            {
                return Json(new { code = 1 });

                Console.WriteLine($"Generic Exception Handler: {e}");
            }

        }
        public ActionResult DeleteCustTerm(String TempID)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    cn.Open();
                    command.CommandText = @"DELETE [IflowSeed].[dbo].[StoreTerms]                          
                                      WHERE TermID = @gID";
                    command.Parameters.AddWithValue("@gID", TempID.ToString());
                    command.ExecuteNonQuery();
                    cn.Close();
                    return Json(new { code = 0 });
                }
            }
            catch (Exception e)
            {
                return Json(new { code = 1 });
                Console.WriteLine($"Generic Exception Handler: {e}");
            }
        }
        public ActionResult SaveMaterial(String MaterialType, String Rate, String ItemCode, String Description)
        {

            try
            {
                Guid gIDs = Guid.NewGuid();
                var gID = gIDs;
                String descriptionVal = "";

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {


                    cn.Open();

                    command.CommandText = @"SELECT Description FROM [IflowSeed].[dbo].[MaterialCharges]" +
                     "WHERE Id = @IdTemp";
                    command.Parameters.AddWithValue("@IdTemp", Description);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        descriptionVal = reader.GetString(0);

                        using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        using (SqlCommand command2 = new SqlCommand("", cn2))
                        {
                            cn2.Open();
                            command.CommandText = @"INSERT INTO [IflowSeed].[dbo].[StoreMaterialCharges] (gID,MaterialType,ItemCode,Description,Rate,Id) VALUES (@gID,@ItemCode,@MaterialType,@Description,@Rate,@Id)";
                            command.Parameters.AddWithValue("@gID", gID);

                            command.Parameters.AddWithValue("@MaterialType", MaterialType);


                            command.Parameters.AddWithValue("@ItemCode", ItemCode);


                            command.Parameters.AddWithValue("@Description", descriptionVal);


                            command.Parameters.AddWithValue("@Rate", Rate);

                            command.Parameters.AddWithValue("@Id", Description);


                            cn2.Close();
                        }



                    }
                    reader.Close();
                    command.ExecuteNonQuery();
                    cn.Close();

                }
                return Json(new { Code = 0, id = Description, gID = gID });



            }
            catch (Exception e)
            {
                return Json(new { Code = 1 });

            }


        }
        public ActionResult displayTable(String id, Guid gID)
        {
            String idSession = @Session["Id"].ToString();
            String ItemCode, Description, Rate;
            List<ListTable> listTemp = new List<ListTable>();
            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn2))
            {
                cn2.Open();
                command.CommandText = @"SELECT ItemCode, Description, Rate, gID FROM [IflowSeed].[dbo].[StoreMaterialCharges]                               
                                      WHERE Id=@id";

                command.Parameters.AddWithValue("@id", idSession.ToString());

                var reader = command.ExecuteReader();
                QuotationModel model = new QuotationModel();
                {
                    while (reader.Read())
                    {


                        model.gID = gID;
                        // if (reader.IsDBNull(0) == false)
                        //{
                        ItemCode = reader.GetString(0);
                        Description = reader.GetString(1);
                        Rate = reader.GetString(2);

                        listTemp.Add(new ListTable { ItemCode = ItemCode, Description = Description, Rate = Rate, gID = reader.GetGuid(3) });

                    }
                    cn2.Close();

                }
                //ViewBag.TbleMaterial = listTemp;
                //return Json(listTemp);
                return Json(new { Data = listTemp });

            }
        }
        //public ActionResult ViewPrice(Guid rateID, string Description)
        //{

        //    List<ListNewRate> listTemp = new List<ListNewRate>();
        //    using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        //    using (SqlCommand command = new SqlCommand("", cn2))
        //    {
        //        cn2.Open();
        //        command.CommandText = @"SELECT rateID, ProductType, MaterialType, Description, newRate, Status, Id, ItemCode, CreatedBy, CreatedOn FROM [IflowSeed].[dbo].[RatePrice]                               
        //                              WHERE rateID = @id AND Description = @Description";

        //        command.Parameters.AddWithValue("@id", rateID.ToString());
        //        command.Parameters.AddWithValue("@Description", Description.ToString());

        //        //cn.Open();
        //        //command.CommandText = @"SELECT  Rate FROM [IflowSeed].[dbo].[MaterialCharges]                          
        //        //                     WHERE Id = @Id";s
        //        //command.Parameters.AddWithValue("@Id", Description.ToString());

        //        var reader = command.ExecuteReader();
        //        ListNewRate model = new ListNewRate();
        //        {
        //            while (reader.Read())
        //            {
        //                model.ProductType = reader.GetString(1);
        //                model.MaterialType = reader.GetString(2);
        //                model.Description = reader.GetString(3);
        //                model.NewRate = reader.GetString(4);
        //                model.Status = reader.GetString(5);
        //                model.ItemCode = reader.GetString(6);
        //            }
        //            cn2.Close();

        //            listTemp.Add(model);
        //        }

        //        return Json(new { Data = listTemp, code = 0 });

        //    }
        //}

    }
}

