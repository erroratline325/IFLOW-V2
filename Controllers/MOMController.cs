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
using static MvcAppV2.Models.NoCounterModel;
using System.Diagnostics;

namespace MvcAppV2.Controllers
{
    public class MOMController : Controller
    {

        //
        // GET: /MOM/
        List<MOMModel> ViewList = new List<MOMModel>();
        public ActionResult ManageMOM(string set, string Subject, string Department)
        {
            ViewBag.IsDepart = @Session["Department"];
            Department = ViewBag.IsDepart;
            var Dept = @Session["Department"];
            
            ViewBag.IsRole = @Session["Role"];
            var Role = @Session["Role"];

            List<MOMModel> ViewList = new List<MOMModel>();
            if (Role.ToString() != "Head Of Sales")
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    cn.Open();

                    if (set == "search")
                    {
                        command.CommandText = @"SELECT DateTxt,MGNo,AppReq,CreatedBy,gID,Status,Department
                                        FROM [IflowSeed].[dbo].[MOM]
                                        WHERE Subject LIKE @Subject OR MGNo LIKE @ReferenceNo AND Department=@Department AND IsJobCompleted=0 ORDER BY DateTxt desc";
                        command.Parameters.AddWithValue("@Subject", "%" + Subject + "%");
                        command.Parameters.AddWithValue("@Department", Dept);
                    }
                    else
                    {
                        command.CommandText = @"SELECT DateTxt,MGNo,Subject,CreatedBy,gID,Status,Department
                                        FROM [IflowSeed].[dbo].[MOM] WHERE IsJobCompleted=0 AND Department=@Department  ORDER BY DateTxt desc";
                        command.Parameters.AddWithValue("@Department", Dept);
                    }
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        MOMModel model = new MOMModel();
                        {
                            if (reader.IsDBNull(0) == false)
                            {
                                model.DateTxtT = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(0));
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                model.MGMNo = reader.GetString(1);
                            }
                            if (reader.IsDBNull(2) == false)
                            {
                                model.Subject = reader.GetString(2);
                            }
                            if (reader.IsDBNull(3) == false)
                            {
                                model.CreatedBy = reader.GetString(3);
                            }
                            if (reader.IsDBNull(4) == false)
                            {
                                model.gID = reader.GetGuid(4);
                            }
                            if (reader.IsDBNull(5) == false)
                            {
                                model.Status = reader.GetString(5);
                            }
                            if (reader.IsDBNull(6) == false)
                            {
                                model.Department = reader.GetString(6);
                            }
                        }
                        ViewList.Add(model);
                    }
                    cn.Close();
                }
            }
            else
            {

                //FOR HEAD FOR SALES
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    cn.Open();

                    if (set == "search")
                    {
                        command.CommandText = @"SELECT DateTxt,MGNo,Subject,CreatedBy,gID,Status,Department
                                        FROM [iflow3].[dbo].[ManagementApproval]
                                        WHERE (Subject LIKE @Subject OR MGNo LIKE @ReferenceNo) AND IsJobCompleted=0 AND Status='New'  AND ( Department='SALES' OR Department='MARKETING') ORDER BY DateTxt desc";
                        command.Parameters.AddWithValue("@Subject", "%" + Subject + "%");
                    }
                    else
                    {
                        command.CommandText = @"SELECT DateTxt,MGNo,Subject,CreatedBy,gID,Status,Department
                                        FROM [iflow3].[dbo].[ManagementApproval] WHERE IsJobCompleted=0  AND Status='New' AND (Department='SALES' OR Department='MARKETING') ORDER BY DateTxt desc";
                    }
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        MOMModel model = new MOMModel();
                        {
                            if (reader.IsDBNull(0) == false)
                            {
                                model.DateTxtT = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(0));
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                model.MGMNo = reader.GetString(1);
                            }
                            if (reader.IsDBNull(2) == false)
                            {
                                model.Subject = reader.GetString(2);
                            }
                            if (reader.IsDBNull(3) == false)
                            {
                                model.CreatedBy = reader.GetString(3);
                            }
                            if (reader.IsDBNull(4) == false)
                            {
                                model.gID = reader.GetGuid(4);
                            }
                            if (reader.IsDBNull(5) == false)
                            {
                                model.Status = reader.GetString(5);
                            }
                            if (reader.IsDBNull(6) == false)
                            {
                                model.Department = reader.GetString(6);
                            }
                        }
                        ViewList.Add(model);
                    }
                    cn.Close();
                }
            }

            return View(ViewList);
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet]

        public ActionResult AddMOM(string id, string Status, MOMModel get, string set, string ARNo, string Department, string CompanyName)
        { 
            
                ViewBag.IsDepart = @Session["Department"];
                Department = ViewBag.IsDepart;
                ViewBag.IsRole = @Session["Role"];
                ViewBag.IdentityName = @Session["Fullname"];
                var IdentityName = @Session["Fullname"];
                Session["FileUploadID"] = "";

                //Viewbag status to pass data to UI
                ViewBag.Status = Status;
                ViewBag.Status = @Session["Status"];
                //Viewbag gID to pass data to UI
                //declare @session["id"] sebagai id (id pass dr mgmtApp)
                @Session["Id"] = id;
                ViewBag.id = id;

                if (set == "Close" && id != null)
                {

                    using (SqlConnection cn5 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        cn5.Open();
                        SqlCommand comm = new SqlCommand("SELECT COUNT(*) FROM [iflow3].[dbo].[FileStoreManagement]" +
                                                    "WHERE MGNo=@Id", cn5);
                        comm.Parameters.AddWithValue("@Id", id);
                        Int32 count = (Int32)comm.ExecuteScalar();
                        if (count > 0)
                        {
                            using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                            {
                                cn3.Open();
                                SqlCommand command3;
                                command3 = new SqlCommand("UPDATE [IflowSeed].[dbo].[MOM] SET Status='Closed',IsJobCompleted=@IsJobCompleted WHERE gID=@Id", cn3);
                                command3.Parameters.AddWithValue("@IsJobCompleted", true);
                                command3.Parameters.AddWithValue("@Id", id);
                                command3.ExecuteNonQuery();
                                cn3.Close();
                            }
                        }
                        cn5.Close();
                        return RedirectToAction("mgmtApp", "SalesMarketing");
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

                    //SELECT DATA FROM DATABASE TO BE VIEW IN UI
                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    using (SqlCommand command = new SqlCommand("", cn))
                    {
                        cn.Open();
                        command.CommandText = @"SELECT MGNo,Subject,Descr,PreparedBy,PreparedPos,RecommandedBy,RecommandedPos,AgreedBy,AgreedPos,VerifiedBy,VerifiedPos,ApprovedBy,ApprovedPos,MGMDescr,ReviewedBy,ReviewedPos,EndorsedBy,EndorsedPos,ConsultationBy,ConsultationPos,JoinPreparedBy,JoinPreparedPos,RemarkReject,Status
                                            FROM [IflowSeed].[dbo].[MOM]
                                            WHERE gID = @id ";
                        command.Parameters.AddWithValue("@id", id.ToString());

                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            MOMModel gets = new MOMModel();
                            {
                                if (reader.IsDBNull(0) == false)
                                {
                                    gets.MGMNo = reader.GetString(0);
                                    Session["FileUploadID"] = gets.MGMNo;
                                    //enable to view data at layout screen
                                    ViewBag.MGNo = gets.MGMNo;

                                }
                                if (reader.IsDBNull(1) == false)
                                {
                                    gets.Subject = reader.GetString(1);
                                }
                                if (reader.IsDBNull(2) == false)
                                {
                                    gets.Descr = reader.GetString(2);
                                }
                                if (reader.IsDBNull(3) == false)
                                {
                                    gets.PreparedBy = reader.GetString(3);
                                    ViewBag.Prepared = gets.PreparedBy;
                                }
                                if (reader.IsDBNull(4) == false)
                                {
                                    gets.PreparedPos = reader.GetString(4);

                                }
                                if (reader.IsDBNull(5) == false)
                                {
                                    gets.RecommandedBy = reader.GetString(5);
                                    ViewBag.Recommanded = gets.RecommandedBy;
                                }
                                if (reader.IsDBNull(6) == false)
                                {
                                    gets.RecommandedPos = reader.GetString(6);
                                }
                                if (reader.IsDBNull(7) == false)
                                {
                                    gets.AgreedBy = reader.GetString(7);
                                    ViewBag.Agreed = gets.AgreedBy;
                                }
                                if (reader.IsDBNull(8) == false)
                                {
                                    gets.AgreedPos = reader.GetString(8);
                                }
                                if (reader.IsDBNull(9) == false)
                                {
                                    gets.VerifiedBy = reader.GetString(9);
                                    ViewBag.Verified = gets.VerifiedBy;
                                }
                                if (reader.IsDBNull(10) == false)
                                {
                                    gets.VerifiedPos = reader.GetString(10);
                                }
                                if (reader.IsDBNull(11) == false)
                                {
                                    gets.ApprovedBy = reader.GetString(11);
                                    ViewBag.Approved = gets.ApprovedBy;
                                }
                                if (reader.IsDBNull(12) == false)
                                {
                                    gets.ApprovedPos = reader.GetString(12);
                                }
                                if (reader.IsDBNull(13) == false)
                                {
                                    gets.MGMDescr = reader.GetString(13);
                                }
                                if (reader.IsDBNull(14) == false)
                                {
                                    gets.ReviewedBy = reader.GetString(14);
                                    ViewBag.Reviewed = gets.ReviewedBy;
                                }
                                if (reader.IsDBNull(15) == false)
                                {
                                    gets.ReviewedPos = reader.GetString(15);
                                }

                                if (reader.IsDBNull(16) == false)
                                {
                                    gets.EndorsedBy = reader.GetString(16);
                                    ViewBag.Endorsed = gets.EndorsedBy;
                                }
                                if (reader.IsDBNull(17) == false)
                                {
                                    gets.EndorsedPos = reader.GetString(17);
                                }

                                if (reader.IsDBNull(18) == false)
                                {
                                    gets.ConsultationBy = reader.GetString(18);
                                    ViewBag.Consultation = gets.ConsultationBy;
                                }
                                if (reader.IsDBNull(19) == false)
                                {
                                    gets.ConsultationPos = reader.GetString(19);
                                }

                                if (reader.IsDBNull(20) == false)
                                {
                                    gets.JoinPreparedBy = reader.GetString(20);
                                    ViewBag.JoinPrepared = gets.JoinPreparedBy;
                                }
                                if (reader.IsDBNull(21) == false)
                                {
                                    gets.JoinPreparedPos = reader.GetString(21);
                                }
                                if (reader.IsDBNull(22) == false)
                                {
                                    gets.RemarkReject = reader.GetString(22);
                                    ViewBag.RemarkReject = gets.RemarkReject;
                                }
                                if (reader.IsDBNull(23) == false)
                                {
                                    gets.Status = reader.GetString(23);
                                    //enable to view data at layout screen
                                    ViewBag.Status = gets.Status;
                                }

                                ViewBag.MGMStat = gets.MGMDescr;

                            }
                            return View(gets);
                        }
                    }
                }

                //if (ViewBag.TotalTick == ViewBag.TotalStatus)
                //{
                //    using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                //    {
                //        cn3.Open();
                //        SqlCommand command3;
                //        command3 = new SqlCommand("UPDATE [iflow3].[dbo].[ManagementApproval] SET Status='Closed' WHERE gID=@Id", cn3);
                //        command3.Parameters.AddWithValue("@Id", id);
                //        command3.ExecuteNonQuery();
                //        cn3.Close();
                //    }
                //}

                return View();
            }

        public string PreparedPos { get; set; }
        public string PreparedEmail { get; set; }
        public string JoinPreparedPos { get; set; }
        public string JoinPreparedEmail { get; set; }
        public string ConsultationPos { get; set; }
        public string ConsultationEmail { get; set; }
        public string RecommandedPos { get; set; }
        public string RecommandedEmail { get; set; }
        public string VerifiedPos { get; set; }
        public string VerifiedEmail { get; set; }
        public string EndorsedPos { get; set; }
        public string EndorsedEmail { get; set; }
        public string ReviewedPos { get; set; }
        public string ReviewedEmail { get; set; }
        public string AgreedPos { get; set; }
        public string AgreedEmail { get; set; }
        public string ApprovedPos { get; set; }
        public string ApprovedEmail { get; set; }


        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddMOM(MOMModel get, String MGMNo, String Department, String uID, String id)
        {
            ViewBag.IsDepart = @Session["Department"];
            Department = ViewBag.IsDepart;

            //uID
            ViewBag.uID = Session["Idx"].ToString();
            uID = ViewBag.uID;

            //gID
            @Session["Id"] = id;

            ViewBag.IsRole = @Session["Role"];
            ViewBag.IdentityName = @Session["Fullname"];
            var IdentityName = @Session["Fullname"];



            if (get.MGMNo == null)
            {
                Guid gid = Guid.NewGuid();
                get.gID = gid;
                //Random rnd = new Random();
                //int rndnumber = rnd.Next();
                //string Date = DateTime.Now.ToString("ddmmyyyy");
                
                string masa = DateTime.Now.ToString("dd/MM/yyyy");
                string createdDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                get.CreatedON = Convert.ToDateTime(createdDate);
                get.DateTxt = Convert.ToDateTime(masa);
                get.PreparedDate = Convert.ToDateTime(masa);
                get.CreatedBy = Session["FullName"].ToString();

                //Guid gid = Guid.NewGuid();
                //get.gID = gid;
                //var CurrRefno = new MgmtPaperCounterModel();
                //get.MGMNo = CurrRefno.RefNo;
                //string createdDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                //get.CreatedBy = Session["FullName"].ToString();

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

                if (get.JoinPreparedBy != "Please Select" && !string.IsNullOrEmpty(get.JoinPreparedBy))
                {
                    //JOIN PREPARED BY
                    using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    using (SqlCommand command = new SqlCommand("", cn2))
                    {
                        cn2.Open();
                        command.CommandText = @"SELECT Role,Email FROM [IflowSeed].[dbo].[User]                               
                                      WHERE Fullname=@Fullname";
                        command.Parameters.AddWithValue("@Fullname", get.JoinPreparedBy.ToString());
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(0) == false)
                            {
                                JoinPreparedPos = reader.GetString(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                JoinPreparedEmail = reader.GetString(1);
                            }
                        }
                        cn2.Close();
                    }
                }

                if (get.ConsultationBy != "Please Select" && !string.IsNullOrEmpty(get.ConsultationBy))
                {
                    //CONSULTATION BY
                    using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    using (SqlCommand command = new SqlCommand("", cn2))
                    {
                        cn2.Open();
                        command.CommandText = @"SELECT Role,Email FROM [IflowSeed].[dbo].[User]                               
                                      WHERE Fullname=@Fullname";
                        command.Parameters.AddWithValue("@Fullname", get.ConsultationBy.ToString());
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(0) == false)
                            {
                                ConsultationPos = reader.GetString(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                ConsultationEmail = reader.GetString(1);
                            }
                        }
                        cn2.Close();
                    }
                }

                if (get.RecommandedBy != "Please Select" && !string.IsNullOrEmpty(get.RecommandedBy))
                {
                    //RECOMMANDED BY
                    using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    using (SqlCommand command = new SqlCommand("", cn2))
                    {
                        cn2.Open();
                        command.CommandText = @"SELECT Role,Email FROM [IflowSeed].[dbo].[User]                               
                                      WHERE Fullname=@Fullname";
                        command.Parameters.AddWithValue("@Fullname", get.RecommandedBy.ToString());
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(0) == false)
                            {
                                RecommandedPos = reader.GetString(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                RecommandedEmail = reader.GetString(1);
                            }
                        }
                        cn2.Close();
                    }
                }

                if (get.VerifiedBy != "Please Select" && !string.IsNullOrEmpty(get.VerifiedBy))
                {
                    //VERIFIED BY
                    using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    using (SqlCommand command = new SqlCommand("", cn2))
                    {
                        cn2.Open();
                        command.CommandText = @"SELECT Role,Email FROM [IflowSeed].[dbo].[User]                               
                                      WHERE Fullname=@Fullname";
                        command.Parameters.AddWithValue("@Fullname", get.VerifiedBy.ToString());
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(0) == false)
                            {
                                VerifiedPos = reader.GetString(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                VerifiedEmail = reader.GetString(1);
                            }
                        }
                        cn2.Close();
                    }
                }

                if (get.EndorsedBy != "Please Select" && !string.IsNullOrEmpty(get.EndorsedBy))
                {
                    //ENDORSED BY
                    using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    using (SqlCommand command = new SqlCommand("", cn2))
                    {
                        cn2.Open();
                        command.CommandText = @"SELECT Role,Email FROM [IflowSeed].[dbo].[User]                               
                                      WHERE Fullname=@Fullname";
                        command.Parameters.AddWithValue("@Fullname", get.EndorsedBy.ToString());
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(0) == false)
                            {
                                EndorsedPos = reader.GetString(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                EndorsedEmail = reader.GetString(1);
                            }
                        }
                        cn2.Close();
                    }
                }

                if (get.ReviewedBy != "Please Select" && !string.IsNullOrEmpty(get.ReviewedBy))
                {
                    //REVIEWED BY
                    using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    using (SqlCommand command = new SqlCommand("", cn2))
                    {
                        cn2.Open();
                        command.CommandText = @"SELECT Role,Email FROM [IflowSeed].[dbo].[User]                               
                                      WHERE Fullname=@Fullname";
                        command.Parameters.AddWithValue("@Fullname", get.ReviewedBy.ToString());
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(0) == false)
                            {
                                ReviewedPos = reader.GetString(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                ReviewedEmail = reader.GetString(1);
                            }
                        }
                        cn2.Close();
                    }
                }

                if (get.AgreedBy != "Please Select" && !string.IsNullOrEmpty(get.AgreedBy))
                {
                    //AGREED BY
                    using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    using (SqlCommand command = new SqlCommand("", cn2))
                    {
                        cn2.Open();
                        command.CommandText = @"SELECT Role,Email FROM [IflowSeed].[dbo].[User]                               
                                      WHERE Fullname=@Fullname";
                        command.Parameters.AddWithValue("@Fullname", get.AgreedBy.ToString());
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(0) == false)
                            {
                                AgreedPos = reader.GetString(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                AgreedEmail = reader.GetString(1);
                            }
                        }
                        cn2.Close();
                    }
                }

                if (get.ApprovedBy != "Please Select" && !string.IsNullOrEmpty(get.ApprovedBy))
                {
                    //APPROVED BY
                    using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    using (SqlCommand command = new SqlCommand("", cn2))
                    {
                        cn2.Open();
                        command.CommandText = @"SELECT Role,Email FROM [IflowSeed].[dbo].[User]                               
                                      WHERE Fullname=@Fullname";
                        command.Parameters.AddWithValue("@Fullname", get.ApprovedBy.ToString());
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(0) == false)
                            {
                                ApprovedPos = reader.GetString(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                ApprovedEmail = reader.GetString(1);
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

                    ViewBag.JoinPreparedPos = JoinPreparedPos;
                    ViewBag.JoinPreparedEmail = JoinPreparedEmail;

                    ViewBag.ConsultationPos = ConsultationPos;
                    ViewBag.ConsultationEmail = ConsultationEmail;

                    ViewBag.RecommandedPos = RecommandedPos;
                    ViewBag.RecommandedEmail = RecommandedEmail;

                    ViewBag.VerifiedPos = VerifiedPos;
                    ViewBag.VerifiedEmail = VerifiedEmail;

                    ViewBag.EndorsedPos = EndorsedPos;
                    ViewBag.EndorsedEmail = EndorsedEmail;

                    ViewBag.ReviewedPos = ReviewedPos;
                    ViewBag.ReviewedEmail = ReviewedEmail;

                    ViewBag.AgreedPos = AgreedPos;
                    ViewBag.AgreedEmail = AgreedEmail;

                    ViewBag.ApprovedPos = ApprovedPos;
                    ViewBag.ApprovedEmail = ApprovedEmail;

                    //if (get.PreparedBy != "Please Select")
                    //{
                    //    string Str = "<html>";
                    //    Str += "<head>";
                    //    Str += "<title></title>";
                    //    Str += "<style type=text/css>p.MsoNormal{margin-bottom:.0001pt;font-size:11.0pt;font-family:Calibri,sans-serif; margin-left: 0cm;margin-right: 0cm;margin-top: 0cm;}.style1{ width: 246px;}.style2{width: 599px;}.style3{ width: 246px; height: 23px;}.style4 {width: 599px;height: 23px;}table, th, td {border: 1px solid black;</style>";
                    //    Str += "</head>";
                    //    Str += "<body>";
                    //    Str += "<p>Kindly reply and take action on the Management Paper soonest possible.</p>";
                    //    Str += "</br>";
                    //    Str += "<table style=width:100%>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>REF NO : </td>";
                    //    Str += "<td class=style2>" + get.MGMNo.ToUpper() + "</td>";
                    //    Str += "</tr>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>SUBJECT : </td>";
                    //    Str += "<td class=style2>" + get.Subject.ToUpper() + "</td>";
                    //    Str += "</tr>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>CREATE BY : </td>";
                    //    Str += "<td class=style2>" + get.CreatedBy + "</td>";
                    //    Str += "</tr>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>ASSIGN TO : </td>";
                    //    Str += "<td class=style2>" + get.PreparedBy + "</td>";
                    //    Str += "</tr>";
                    //    Str += "</table>";
                    //    Str += "</body>";
                    //    Str += "</html>";

                    //    bool isEmailSendSuccessfully = false;

                    //    try
                    //    {
                    //        // mailer.Send(mailMessage);
                    //        string smtpServer = IpSMtp_;
                    //        //string userName = "m.rizalramli@intercity.com.my";
                    //        //string password = "Abcd123$";
                    //        int cdoBasic = 1;
                    //        int cdoSendUsingPort = 2;
                    //        System.Web.Mail.MailMessage msg = new System.Web.Mail.MailMessage();
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", smtpServer);
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", PortSmtp_);
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", cdoSendUsingPort);
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", cdoBasic); 
                    //        msg.To = PreparedEmail.Trim();
                    //        msg.From = "i-flow_noreply@avis.com.my";
                    //        msg.Subject = "I-FLOW - MANAGEMENT PAPER";
                    //        msg.Body = Str;
                    //        msg.BodyFormat = MailFormat.Html;
                    //        //SmtpMail.SmtpServer = smtpServer;
                    //        //SmtpMail.Send(msg);

                    //        isEmailSendSuccessfully = true;
                    //    }
                    //    catch
                    //    {
                    //        isEmailSendSuccessfully = false;
                    //    }
                    //}

                    //if (get.RecommandedBy != "Please Select")
                    //{
                    //    string Str = "<html>";
                    //    Str += "<head>";
                    //    Str += "<title></title>";
                    //    Str += "<style type=text/css>p.MsoNormal{margin-bottom:.0001pt;font-size:11.0pt;font-family:Calibri,sans-serif; margin-left: 0cm;margin-right: 0cm;margin-top: 0cm;}.style1{ width: 246px;}.style2{width: 599px;}.style3{ width: 246px; height: 23px;}.style4 {width: 599px;height: 23px;}table, th, td {border: 1px solid black;</style>";
                    //    Str += "</head>";
                    //    Str += "<body>";
                    //    Str += "<p>Kindly reply and take action on the Management Paper soonest possible.</p>";
                    //    Str += "</br>";
                    //    Str += "<table style=width:100%>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>REF NO : </td>";
                    //    Str += "<td class=style2>" + get.MGMNo.ToUpper() + "</td>";
                    //    Str += "</tr>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>SUBJECT : </td>";
                    //    Str += "<td class=style2>" + get.Subject.ToUpper() + "</td>";
                    //    Str += "</tr>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>CREATE BY : </td>";
                    //    Str += "<td class=style2>" + get.CreatedBy + "</td>";
                    //    Str += "</tr>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>ASSIGN TO : </td>";
                    //    Str += "<td class=style2>" + get.RecommandedBy + "</td>";
                    //    Str += "</tr>";
                    //    Str += "</table>";
                    //    Str += "</body>";
                    //    Str += "</html>";

                    //    bool isEmailSendSuccessfully = false;

                    //    try
                    //    {
                    //        // mailer.Send(mailMessage);
                    //        string smtpServer = IpSMtp_;
                    //        //string userName = "m.rizalramli@intercity.com.my";
                    //        //string password = "Abcd123$";
                    //        int cdoBasic = 1;
                    //        int cdoSendUsingPort = 2;
                    //        System.Web.Mail.MailMessage msg = new System.Web.Mail.MailMessage();
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", smtpServer);
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", PortSmtp_);
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", cdoSendUsingPort);
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", cdoBasic);
                    //        msg.To = RecommandedEmail.Trim();
                    //        msg.From = "i-flow_noreply@avis.com.my";
                    //        msg.Subject = "I-FLOW - MANAGEMENT PAPER";
                    //        msg.Body = Str;
                    //        msg.BodyFormat = MailFormat.Html;
                    //        //SmtpMail.SmtpServer = smtpServer;
                    //        //SmtpMail.Send(msg);

                    //        isEmailSendSuccessfully = true;
                    //    }
                    //    catch
                    //    {
                    //        isEmailSendSuccessfully = false;
                    //    }
                    //}

                    //if (get.AgreedBy != "Please Select")
                    //{
                    //    string Str = "<html>";
                    //    Str += "<head>";
                    //    Str += "<title></title>";
                    //    Str += "<style type=text/css>p.MsoNormal{margin-bottom:.0001pt;font-size:11.0pt;font-family:Calibri,sans-serif; margin-left: 0cm;margin-right: 0cm;margin-top: 0cm;}.style1{ width: 246px;}.style2{width: 599px;}.style3{ width: 246px; height: 23px;}.style4 {width: 599px;height: 23px;}table, th, td {border: 1px solid black;</style>";
                    //    Str += "</head>";
                    //    Str += "<body>";
                    //    Str += "<p>Kindly reply and take action on the Management Paper soonest possible.</p>";
                    //    Str += "</br>";
                    //    Str += "<table style=width:100%>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>REF NO : </td>";
                    //    Str += "<td class=style2>" + get.MGMNo.ToUpper() + "</td>";
                    //    Str += "</tr>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>SUBJECT : </td>";
                    //    Str += "<td class=style2>" + get.Subject.ToUpper() + "</td>";
                    //    Str += "</tr>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>CREATE BY : </td>";
                    //    Str += "<td class=style2>" + get.CreatedBy + "</td>";
                    //    Str += "</tr>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>ASSIGN TO : </td>";
                    //    Str += "<td class=style2>" + get.AgreedBy + "</td>";
                    //    Str += "</tr>";
                    //    Str += "</table>";
                    //    Str += "</body>";
                    //    Str += "</html>";

                    //    bool isEmailSendSuccessfully = false;

                    //    try
                    //    {
                    //        // mailer.Send(mailMessage);
                    //        string smtpServer = IpSMtp_;
                    //        //string userName = "m.rizalramli@intercity.com.my";
                    //        //string password = "Abcd123$";
                    //        int cdoBasic = 1;
                    //        int cdoSendUsingPort = 2;
                    //        System.Web.Mail.MailMessage msg = new System.Web.Mail.MailMessage();
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", smtpServer);
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", PortSmtp_);
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", cdoSendUsingPort);
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", cdoBasic);
                    //        msg.To = AgreedEmail.Trim();
                    //        msg.From = "i-flow_noreply@avis.com.my";
                    //        msg.Subject = "I-FLOW - MANAGEMENT PAPER";
                    //        msg.Body = Str;
                    //        msg.BodyFormat = MailFormat.Html;
                    //        //SmtpMail.SmtpServer = smtpServer;
                    //        //SmtpMail.Send(msg);

                    //        isEmailSendSuccessfully = true;
                    //    }
                    //    catch
                    //    {
                    //        isEmailSendSuccessfully = false;
                    //    }
                    //}

                    //if (get.VerifiedBy != "Please Select")
                    //{
                    //    string Str = "<html>";
                    //    Str += "<head>";
                    //    Str += "<title></title>";
                    //    Str += "<style type=text/css>p.MsoNormal{margin-bottom:.0001pt;font-size:11.0pt;font-family:Calibri,sans-serif; margin-left: 0cm;margin-right: 0cm;margin-top: 0cm;}.style1{ width: 246px;}.style2{width: 599px;}.style3{ width: 246px; height: 23px;}.style4 {width: 599px;height: 23px;}table, th, td {border: 1px solid black;</style>";
                    //    Str += "</head>";
                    //    Str += "<body>";
                    //    Str += "<p>Kindly reply and take action on the Management Paper soonest possible.</p>";
                    //    Str += "</br>";
                    //    Str += "<table style=width:100%>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>REF NO : </td>";
                    //    Str += "<td class=style2>" + get.MGMNo.ToUpper() + "</td>";
                    //    Str += "</tr>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>SUBJECT : </td>";
                    //    Str += "<td class=style2>" + get.Subject.ToUpper() + "</td>";
                    //    Str += "</tr>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>CREATE BY : </td>";
                    //    Str += "<td class=style2>" + get.CreatedBy + "</td>";
                    //    Str += "</tr>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>ASSIGN TO : </td>";
                    //    Str += "<td class=style2>" + get.VerifiedBy + "</td>";
                    //    Str += "</tr>";
                    //    Str += "</table>";
                    //    Str += "</body>";
                    //    Str += "</html>";

                    //    bool isEmailSendSuccessfully = false;

                    //    try
                    //    {
                    //        // mailer.Send(mailMessage);
                    //        string smtpServer = IpSMtp_;
                    //        //string userName = "m.rizalramli@intercity.com.my";
                    //        //string password = "Abcd123$";
                    //        int cdoBasic = 1;
                    //        int cdoSendUsingPort = 2;
                    //        System.Web.Mail.MailMessage msg = new System.Web.Mail.MailMessage();
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", smtpServer);
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", PortSmtp_);
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", cdoSendUsingPort);
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", cdoBasic);
                    //        msg.To = VerifiedEmail.Trim();
                    //        msg.From = "i-flow_noreply@avis.com.my";
                    //        msg.Subject = "I-FLOW - MANAGEMENT PAPER";
                    //        msg.Body = Str;
                    //        msg.BodyFormat = MailFormat.Html;
                    //        //SmtpMail.SmtpServer = smtpServer;
                    //        //SmtpMail.Send(msg);

                    //        isEmailSendSuccessfully = true;
                    //    }
                    //    catch
                    //    {
                    //        isEmailSendSuccessfully = false;
                    //    }
                    //}


                    //if (get.ApprovedBy != "Please Select")
                    //{
                    //    string Str = "<html>";
                    //    Str += "<head>";
                    //    Str += "<title></title>";
                    //    Str += "<style type=text/css>p.MsoNormal{margin-bottom:.0001pt;font-size:11.0pt;font-family:Calibri,sans-serif; margin-left: 0cm;margin-right: 0cm;margin-top: 0cm;}.style1{ width: 246px;}.style2{width: 599px;}.style3{ width: 246px; height: 23px;}.style4 {width: 599px;height: 23px;}table, th, td {border: 1px solid black;</style>";
                    //    Str += "</head>";
                    //    Str += "<body>";
                    //    Str += "<p>Kindly reply and take action on the Management Paper soonest possible.</p>";
                    //    Str += "</br>";
                    //    Str += "<table style=width:100%>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>REF NO : </td>";
                    //    Str += "<td class=style2>" + get.MGMNo.ToUpper() + "</td>";
                    //    Str += "</tr>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>SUBJECT : </td>";
                    //    Str += "<td class=style2>" + get.Subject.ToUpper() + "</td>";
                    //    Str += "</tr>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>CREATE BY : </td>";
                    //    Str += "<td class=style2>" + get.CreatedBy + "</td>";
                    //    Str += "</tr>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>ASSIGN TO : </td>";
                    //    Str += "<td class=style2>" + get.ApprovedBy + "</td>";
                    //    Str += "</tr>";
                    //    Str += "</table>";
                    //    Str += "</body>";
                    //    Str += "</html>";

                    //    bool isEmailSendSuccessfully = false;

                    //    try
                    //    {
                    //        // mailer.Send(mailMessage);
                    //        string smtpServer = IpSMtp_;
                    //        //string userName = "m.rizalramli@intercity.com.my";
                    //        //string password = "Abcd123$";
                    //        int cdoBasic = 1;
                    //        int cdoSendUsingPort = 2;
                    //        System.Web.Mail.MailMessage msg = new System.Web.Mail.MailMessage();
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", smtpServer);
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", PortSmtp_);
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", cdoSendUsingPort);
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", cdoBasic);
                    //        msg.To = ApprovedEmail.Trim();
                    //        msg.From = "i-flow_noreply@avis.com.my";
                    //        msg.Subject = "I-FLOW - MANAGEMENT PAPER";
                    //        msg.Body = Str;
                    //        msg.BodyFormat = MailFormat.Html;
                    //        //SmtpMail.SmtpServer = smtpServer;
                    //        //SmtpMail.Send(msg);

                    //        isEmailSendSuccessfully = true;
                    //    }
                    //    catch
                    //    {
                    //        isEmailSendSuccessfully = false;
                    //    }
                    //}

                    //if (get.ReviewedBy != "Please Select")
                    //{
                    //    string Str = "<html>";
                    //    Str += "<head>";
                    //    Str += "<title></title>";
                    //    Str += "<style type=text/css>p.MsoNormal{margin-bottom:.0001pt;font-size:11.0pt;font-family:Calibri,sans-serif; margin-left: 0cm;margin-right: 0cm;margin-top: 0cm;}.style1{ width: 246px;}.style2{width: 599px;}.style3{ width: 246px; height: 23px;}.style4 {width: 599px;height: 23px;}table, th, td {border: 1px solid black;</style>";
                    //    Str += "</head>";
                    //    Str += "<body>";
                    //    Str += "<p>Kindly reply and take action on the Management Paper soonest possible.</p>";
                    //    Str += "</br>";
                    //    Str += "<table style=width:100%>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>REF NO : </td>";
                    //    Str += "<td class=style2>" + get.MGMNo.ToUpper() + "</td>";
                    //    Str += "</tr>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>SUBJECT : </td>";
                    //    Str += "<td class=style2>" + get.Subject.ToUpper() + "</td>";
                    //    Str += "</tr>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>CREATE BY : </td>";
                    //    Str += "<td class=style2>" + get.CreatedBy + "</td>";
                    //    Str += "</tr>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>ASSIGN TO : </td>";
                    //    Str += "<td class=style2>" + get.ReviewedBy + "</td>";
                    //    Str += "</tr>";
                    //    Str += "</table>";
                    //    Str += "</body>";
                    //    Str += "</html>";

                    //    bool isEmailSendSuccessfully = false;

                    //    try
                    //    {
                    //        // mailer.Send(mailMessage);
                    //        string smtpServer = IpSMtp_;
                    //        //string userName = "m.rizalramli@intercity.com.my";
                    //        //string password = "Abcd123$";
                    //        int cdoBasic = 1;
                    //        int cdoSendUsingPort = 2;
                    //        System.Web.Mail.MailMessage msg = new System.Web.Mail.MailMessage();
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", smtpServer);
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", PortSmtp_);
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", cdoSendUsingPort);
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", cdoBasic);
                    //        msg.To = ReviewedEmail.Trim();
                    //        msg.From = "i-flow_noreply@avis.com.my";
                    //        msg.Subject = "I-FLOW - MANAGEMENT PAPER";
                    //        msg.Body = Str;
                    //        msg.BodyFormat = MailFormat.Html;
                    //        //SmtpMail.SmtpServer = smtpServer;
                    //        //SmtpMail.Send(msg);

                    //        isEmailSendSuccessfully = true;
                    //    }
                    //    catch
                    //    {
                    //        isEmailSendSuccessfully = false;
                    //    }
                    //}

                    //if (get.EndorsedBy != "Please Select")
                    //{
                    //    string Str = "<html>";
                    //    Str += "<head>";
                    //    Str += "<title></title>";
                    //    Str += "<style type=text/css>p.MsoNormal{margin-bottom:.0001pt;font-size:11.0pt;font-family:Calibri,sans-serif; margin-left: 0cm;margin-right: 0cm;margin-top: 0cm;}.style1{ width: 246px;}.style2{width: 599px;}.style3{ width: 246px; height: 23px;}.style4 {width: 599px;height: 23px;}table, th, td {border: 1px solid black;</style>";
                    //    Str += "</head>";
                    //    Str += "<body>";
                    //    Str += "<p>Kindly reply and take action on the Management Paper soonest possible.</p>";
                    //    Str += "</br>";
                    //    Str += "<table style=width:100%>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>REF NO : </td>";
                    //    Str += "<td class=style2>" + get.MGMNo.ToUpper() + "</td>";
                    //    Str += "</tr>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>SUBJECT : </td>";
                    //    Str += "<td class=style2>" + get.Subject.ToUpper() + "</td>";
                    //    Str += "</tr>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>CREATE BY : </td>";
                    //    Str += "<td class=style2>" + get.CreatedBy + "</td>";
                    //    Str += "</tr>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>ASSIGN TO : </td>";
                    //    Str += "<td class=style2>" + get.EndorsedBy + "</td>";
                    //    Str += "</tr>";
                    //    Str += "</table>";
                    //    Str += "</body>";
                    //    Str += "</html>";

                    //    bool isEmailSendSuccessfully = false;

                    //    try
                    //    {
                    //        // mailer.Send(mailMessage);
                    //        string smtpServer = IpSMtp_;
                    //        //string userName = "m.rizalramli@intercity.com.my";
                    //        //string password = "Abcd123$";
                    //        int cdoBasic = 1;
                    //        int cdoSendUsingPort = 2;
                    //        System.Web.Mail.MailMessage msg = new System.Web.Mail.MailMessage();
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", smtpServer);
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", PortSmtp_);
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", cdoSendUsingPort);
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", cdoBasic);
                    //        msg.To = EndorsedEmail.Trim();
                    //        msg.From = "i-flow_noreply@avis.com.my";
                    //        msg.Subject = "I-FLOW - MANAGEMENT PAPER";
                    //        msg.Body = Str;
                    //        msg.BodyFormat = MailFormat.Html;
                    //        //SmtpMail.SmtpServer = smtpServer;
                    //        //SmtpMail.Send(msg);

                    //        isEmailSendSuccessfully = true;
                    //    }
                    //    catch
                    //    {
                    //        isEmailSendSuccessfully = false;
                    //    }
                    //}

                    //if (get.ConsultationBy != "Please Select")
                    //{
                    //    string Str = "<html>";
                    //    Str += "<head>";
                    //    Str += "<title></title>";
                    //    Str += "<style type=text/css>p.MsoNormal{margin-bottom:.0001pt;font-size:11.0pt;font-family:Calibri,sans-serif; margin-left: 0cm;margin-right: 0cm;margin-top: 0cm;}.style1{ width: 246px;}.style2{width: 599px;}.style3{ width: 246px; height: 23px;}.style4 {width: 599px;height: 23px;}table, th, td {border: 1px solid black;</style>";
                    //    Str += "</head>";
                    //    Str += "<body>";
                    //    Str += "<p>Kindly reply and take action on the Management Paper soonest possible.</p>";
                    //    Str += "</br>";
                    //    Str += "<table style=width:100%>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>REF NO : </td>";
                    //    Str += "<td class=style2>" + get.MGMNo.ToUpper() + "</td>";
                    //    Str += "</tr>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>SUBJECT : </td>";
                    //    Str += "<td class=style2>" + get.Subject.ToUpper() + "</td>";
                    //    Str += "</tr>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>CREATE BY : </td>";
                    //    Str += "<td class=style2>" + get.CreatedBy + "</td>";
                    //    Str += "</tr>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>ASSIGN TO : </td>";
                    //    Str += "<td class=style2>" + get.ConsultationBy + "</td>";
                    //    Str += "</tr>";
                    //    Str += "</table>";
                    //    Str += "</body>";
                    //    Str += "</html>";

                    //    bool isEmailSendSuccessfully = false;

                    //    try
                    //    {
                    //        // mailer.Send(mailMessage);
                    //        string smtpServer = IpSMtp_;
                    //        //string userName = "m.rizalramli@intercity.com.my";
                    //        //string password = "Abcd123$";
                    //        int cdoBasic = 1;
                    //        int cdoSendUsingPort = 2;
                    //        System.Web.Mail.MailMessage msg = new System.Web.Mail.MailMessage();
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", smtpServer);
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", PortSmtp_);
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", cdoSendUsingPort);
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", cdoBasic);
                    //        msg.To = ConsultationEmail.Trim();
                    //        msg.From = "i-flow_noreply@avis.com.my";
                    //        msg.Subject = "I-FLOW - MANAGEMENT PAPER";
                    //        msg.Body = Str;
                    //        msg.BodyFormat = MailFormat.Html;
                    //        //SmtpMail.SmtpServer = smtpServer;
                    //        //SmtpMail.Send(msg);

                    //        isEmailSendSuccessfully = true;
                    //    }
                    //    catch
                    //    {
                    //        isEmailSendSuccessfully = false;
                    //    }
                    //}

                    //if (get.JoinPreparedBy != "Please Select")
                    //{
                    //    string Str = "<html>";
                    //    Str += "<head>";
                    //    Str += "<title></title>";
                    //    Str += "<style type=text/css>p.MsoNormal{margin-bottom:.0001pt;font-size:11.0pt;font-family:Calibri,sans-serif; margin-left: 0cm;margin-right: 0cm;margin-top: 0cm;}.style1{ width: 246px;}.style2{width: 599px;}.style3{ width: 246px; height: 23px;}.style4 {width: 599px;height: 23px;}table, th, td {border: 1px solid black;</style>";
                    //    Str += "</head>";
                    //    Str += "<body>";
                    //    Str += "<p>Kindly reply and take action on the Management Paper soonest possible.</p>";
                    //    Str += "</br>";
                    //    Str += "<table style=width:100%>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>REF NO : </td>";
                    //    Str += "<td class=style2>" + get.MGMNo.ToUpper() + "</td>";
                    //    Str += "</tr>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>SUBJECT : </td>";
                    //    Str += "<td class=style2>" + get.Subject.ToUpper() + "</td>";
                    //    Str += "</tr>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>CREATE BY : </td>";
                    //    Str += "<td class=style2>" + get.CreatedBy + "</td>";
                    //    Str += "</tr>";
                    //    Str += "<tr>";
                    //    Str += "<td class=style1>ASSIGN TO : </td>";
                    //    Str += "<td class=style2>" + get.JoinPreparedBy + "</td>";
                    //    Str += "</tr>";
                    //    Str += "</table>";
                    //    Str += "</body>";
                    //    Str += "</html>";

                    //    bool isEmailSendSuccessfully = false;

                    //    try
                    //    {
                    //        // mailer.Send(mailMessage);
                    //        string smtpServer = IpSMtp_;
                    //        //string userName = "m.rizalramli@intercity.com.my";
                    //        //string password = "Abcd123$";
                    //        int cdoBasic = 1;
                    //        int cdoSendUsingPort = 2;
                    //        System.Web.Mail.MailMessage msg = new System.Web.Mail.MailMessage();
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", smtpServer);
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", PortSmtp_);
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", cdoSendUsingPort);
                    //        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", cdoBasic);
                    //        msg.To = JoinPreparedEmail.Trim();
                    //        msg.From = "i-flow_noreply@avis.com.my";
                    //        msg.Subject = "I-FLOW - MANAGEMENT PAPER";
                    //        msg.Body = Str;
                    //        msg.BodyFormat = MailFormat.Html;
                    //        //SmtpMail.SmtpServer = smtpServer;
                    //        //SmtpMail.Send(msg);

                    //        isEmailSendSuccessfully = true;
                    //    }
                    //    catch
                    //    {
                    //        isEmailSendSuccessfully = false;
                    //    }
                    //}
                    var GetRefNo = new AppMOMModel();
                    string Reff = GetRefNo.RefNo;
                    get.MGMNo = Reff;

                    Guid guidId = Guid.NewGuid();
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                    string DateTxt = DateTime.Now.ToString("yyyy-MM-dd");


                    //INSERT OR UPDATE DATA HERE
                    cn.Open();
                    command.CommandText = @"INSERT INTO [IflowSeed].[dbo].[MOM] (gID,CreatedON,Subject,Descr,DateTxt,CreatedBy,PreparedBy" +
                              ",PreparedPos,PreparedDate,RecommandedBy,RecommandedPos,RecommandedDate,AgreedBy,AgreedPos,AgreedDate,VerifiedBy,VerifiedPos,VerifiedDate,ApprovedBy" +
                              ",ApprovedPos,ApprovedDate,MGMDescr,ReviewedBy,ReviewedPos,EndorsedBy,EndorsedPos,ConsultationBy,ConsultationPos,JoinPreparedBy,JoinPreparedPos" +
                              ",ReviewedDate,EndorsedDate,ConsultationDate,JoinPreparedDate,PreparedEmail,JoinPreparedEmail,ConsultationEmail,RecommandedEmail,VerifiedEmail,EndorsedEmail,ReviewedEmail,AgreedEmail,ApprovedEmail,Status,Department,uID,IsJobCompleted)" +
                               "VALUES (@gID,@CreatedON,@Subject,@Descr,@DateTxt,@CreatedBy,@PreparedBy,@PreparedPos,@PreparedDate,@RecommandedBy,@RecommandedPos,@RecommandedDate,@AgreedBy,@AgreedPos,@AgreedDate,@VerifiedBy,@VerifiedPos,@VerifiedDate,@ApprovedBy,@ApprovedPos,@ApprovedDate,@MGMDescr,@ReviewedBy,@ReviewedPos,@EndorsedBy,@EndorsedPos,@ConsultationBy,@ConsultationPos,@JoinPreparedBy,@JoinPreparedPos,@ReviewedDate,@EndorsedDate,@ConsultationDate,@JoinPreparedDate,@PreparedEmail,@JoinPreparedEmail,@ConsultationEmail,@RecommandedEmail,@VerifiedEmail,@EndorsedEmail,@ReviewedEmail,@AgreedEmail,@ApprovedEmail,@Status,@Department,@uID,@IsJobCompleted)";
                    command.Parameters.AddWithValue("@gID", get.gID);                   
                    command.Parameters.AddWithValue("@CreatedON", createdOn);
                    command.Parameters.AddWithValue("@Subject", get.Subject);
                    command.Parameters.AddWithValue("@Descr", get.Descr);
                    command.Parameters.AddWithValue("@DateTxt", DateTxt);
                    command.Parameters.AddWithValue("@CreatedBy", get.CreatedBy);
                    if (get.PreparedBy == "Please Select")
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@PreparedBy", Value = "Please Select" });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@PreparedPos", Value = DBNull.Value });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@PreparedDate", Value = DBNull.Value });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@PreparedEmail", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@PreparedBy", get.PreparedBy);
                        command.Parameters.AddWithValue("@PreparedPos", PreparedPos);
                        command.Parameters.AddWithValue("@PreparedDate", DateTxt);
                        command.Parameters.AddWithValue("@PreparedEmail", PreparedEmail);
                    }

                    if (get.RecommandedBy == "Please Select")
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@RecommandedBy", Value = "Please Select" });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@RecommandedPos", Value = DBNull.Value });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@RecommandedDate", Value = DBNull.Value });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@RecommandedEmail", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RecommandedBy", get.RecommandedBy);
                        command.Parameters.AddWithValue("@RecommandedPos", RecommandedPos);
                        command.Parameters.AddWithValue("@RecommandedDate", DateTxt);
                        command.Parameters.AddWithValue("@RecommandedEmail", RecommandedEmail);
                    }

                    if (get.AgreedBy == "Please Select")
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@AgreedBy", Value = "Please Select" });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@AgreedPos", Value = DBNull.Value });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@AgreedDate", Value = DBNull.Value });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@AgreedEmail", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@AgreedBy", get.AgreedBy);
                        command.Parameters.AddWithValue("@AgreedPos", AgreedPos);
                        command.Parameters.AddWithValue("@AgreedDate", DateTxt);
                        command.Parameters.AddWithValue("@AgreedEmail", AgreedEmail);
                    }

                    if (get.VerifiedBy == "Please Select")
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@VerifiedBy", Value = "Please Select" });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@VerifiedPos", Value = DBNull.Value });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@VerifiedDate", Value = DBNull.Value });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@VerifiedEmail", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@VerifiedBy", get.VerifiedBy);
                        command.Parameters.AddWithValue("@VerifiedPos", VerifiedPos);
                        command.Parameters.AddWithValue("@VerifiedDate", DateTxt);
                        command.Parameters.AddWithValue("@VerifiedEmail", VerifiedEmail);
                    }

                    if (get.ApprovedBy == "Please Select")
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@ApprovedBy", Value = "Please Select" });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@ApprovedPos", Value = DBNull.Value });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@ApprovedDate", Value = DBNull.Value });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@ApprovedEmail", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ApprovedBy", get.ApprovedBy);
                        command.Parameters.AddWithValue("@ApprovedPos", ApprovedPos);
                        command.Parameters.AddWithValue("@ApprovedDate", DateTxt);
                        command.Parameters.AddWithValue("@ApprovedEmail", ApprovedEmail);
                    }
                    get.MGMDescr = "KeyIn";
                    command.Parameters.AddWithValue("@MGMDescr", get.MGMDescr);

                    if (get.ReviewedBy == "Please Select")
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@ReviewedBy", Value = "Please Select" });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@ReviewedPos", Value = DBNull.Value });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@ReviewedDate", Value = DBNull.Value });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@ReviewedEmail", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ReviewedBy", get.ReviewedBy);
                        command.Parameters.AddWithValue("@ReviewedPos", ReviewedPos);
                        command.Parameters.AddWithValue("@ReviewedDate", DateTxt);
                        command.Parameters.AddWithValue("@ReviewedEmail", ReviewedEmail);
                    }

                    if (get.EndorsedBy == "Please Select")
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@EndorsedBy", Value = "Please Select" });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@EndorsedPos", Value = DBNull.Value });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@EndorsedDate", Value = DBNull.Value });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@EndorsedEmail", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@EndorsedBy", get.EndorsedBy);
                        command.Parameters.AddWithValue("@EndorsedPos", EndorsedPos);
                        command.Parameters.AddWithValue("@EndorsedDate", DateTxt);
                        command.Parameters.AddWithValue("@EndorsedEmail", EndorsedEmail);
                    }

                    if (get.ConsultationBy == "Please Select")
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@ConsultationBy", Value = "Please Select" });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@ConsultationPos", Value = DBNull.Value });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@ConsultationDate", Value = DBNull.Value });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@ConsultationEmail", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ConsultationBy", get.ConsultationBy);
                        command.Parameters.AddWithValue("@ConsultationPos", ConsultationPos);
                        command.Parameters.AddWithValue("@ConsultationDate", DateTxt);
                        command.Parameters.AddWithValue("@ConsultationEmail", ConsultationEmail);
                    }

                    if (get.JoinPreparedBy == "Please Select")
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@JoinPreparedBy", Value = "Please Select" });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@JoinPreparedPos", Value = DBNull.Value });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@JoinPreparedDate", Value = DBNull.Value });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@JoinPreparedEmail", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@JoinPreparedBy", get.JoinPreparedBy);
                        command.Parameters.AddWithValue("@JoinPreparedPos", JoinPreparedPos);
                        command.Parameters.AddWithValue("@JoinPreparedDate", DateTxt);
                        command.Parameters.AddWithValue("@JoinPreparedEmail", JoinPreparedEmail);
                    }
                    command.Parameters.AddWithValue("@Status", "New");
                    command.Parameters.AddWithValue("@Department", Department);
                    command.Parameters.AddWithValue("@uID", uID);
                    command.Parameters.AddWithValue("@IsJobCompleted", false);
                    command.ExecuteNonQuery();
                    cn.Close();
                }
            }
            else
            {
                //string masa = DateTime.Now.ToString("dd/MM/yyyy");
                //get.PreparedDate = Convert.ToDateTime(masa);
                //get.RecommandedDate = get.PreparedDate;
                //get.AgreedDate = get.PreparedDate;
                //get.VerifiedDate = get.PreparedDate;
                //get.ApprovedDate = get.PreparedDate;
                //get.ReviewedDate = get.PreparedDate;
                //get.EndorsedDate = get.PreparedDate;
                //get.ConsultationDate = get.PreparedDate;
                //get.JoinPreparedDate = get.PreparedDate;

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

                if (get.JoinPreparedBy != "Please Select" && !string.IsNullOrEmpty(get.JoinPreparedBy))
                {
                    //JOIN PREPARED BY
                    using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    using (SqlCommand command = new SqlCommand("", cn2))
                    {
                        cn2.Open();
                        command.CommandText = @"SELECT Role,Email FROM [IflowSeed].[dbo].[User]                               
                                      WHERE Fullname=@Fullname";
                        command.Parameters.AddWithValue("@Fullname", get.JoinPreparedBy.ToString());
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(0) == false)
                            {
                                JoinPreparedPos = reader.GetString(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                JoinPreparedEmail = reader.GetString(1);
                            }
                        }
                        cn2.Close();
                    }
                }

                if (get.ConsultationBy != "Please Select" && !string.IsNullOrEmpty(get.ConsultationBy))
                {
                    //CONSULTATION BY
                    using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    using (SqlCommand command = new SqlCommand("", cn2))
                    {
                        cn2.Open();
                        command.CommandText = @"SELECT Role,Email FROM [IflowSeed].[dbo].[User]                               
                                      WHERE Fullname=@Fullname";
                        command.Parameters.AddWithValue("@Fullname", get.ConsultationBy.ToString());
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(0) == false)
                            {
                                ConsultationPos = reader.GetString(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                ConsultationEmail = reader.GetString(1);
                            }
                        }
                        cn2.Close();
                    }
                }

                if (get.RecommandedBy != "Please Select" && !string.IsNullOrEmpty(get.RecommandedBy))
                {
                    //RECOMMANDED BY
                    using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    using (SqlCommand command = new SqlCommand("", cn2))
                    {
                        cn2.Open();
                        command.CommandText = @"SELECT Role,Email FROM [IflowSeed].[dbo].[User]                               
                                      WHERE Fullname=@Fullname";
                        command.Parameters.AddWithValue("@Fullname", get.RecommandedBy.ToString());
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(0) == false)
                            {
                                RecommandedPos = reader.GetString(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                RecommandedEmail = reader.GetString(1);
                            }
                        }
                        cn2.Close();
                    }
                }

                if (get.VerifiedBy != "Please Select" && !string.IsNullOrEmpty(get.VerifiedBy))
                {
                    //VERIFIED BY
                    using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    using (SqlCommand command = new SqlCommand("", cn2))
                    {
                        cn2.Open();
                        command.CommandText = @"SELECT Role,Email FROM [IflowSeed].[dbo].[User]                               
                                      WHERE Fullname=@Fullname";
                        command.Parameters.AddWithValue("@Fullname", get.VerifiedBy.ToString());
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(0) == false)
                            {
                                VerifiedPos = reader.GetString(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                VerifiedEmail = reader.GetString(1);
                            }
                        }
                        cn2.Close();
                    }
                }

                if (get.EndorsedBy != "Please Select" && !string.IsNullOrEmpty(get.EndorsedBy))
                {
                    //ENDORSED BY
                    using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    using (SqlCommand command = new SqlCommand("", cn2))
                    {
                        cn2.Open();
                        command.CommandText = @"SELECT Role,Email FROM [IflowSeed].[dbo].[User]                               
                                      WHERE Fullname=@Fullname";
                        command.Parameters.AddWithValue("@Fullname", get.EndorsedBy.ToString());
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(0) == false)
                            {
                                EndorsedPos = reader.GetString(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                EndorsedEmail = reader.GetString(1);
                            }
                        }
                        cn2.Close();
                    }
                }

                if (get.ReviewedBy != "Please Select" && !string.IsNullOrEmpty(get.ReviewedBy))
                {
                    //REVIEWED BY
                    using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    using (SqlCommand command = new SqlCommand("", cn2))
                    {
                        cn2.Open();
                        command.CommandText = @"SELECT Role,Email FROM [IflowSeed].[dbo].[User]                               
                                      WHERE Fullname=@Fullname";
                        command.Parameters.AddWithValue("@Fullname", get.ReviewedBy.ToString());
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(0) == false)
                            {
                                ReviewedPos = reader.GetString(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                ReviewedEmail = reader.GetString(1);
                            }
                        }
                        cn2.Close();
                    }
                }

                if (get.AgreedBy != "Please Select" && !string.IsNullOrEmpty(get.AgreedBy))
                {
                    //AGREED BY
                    using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    using (SqlCommand command = new SqlCommand("", cn2))
                    {
                        cn2.Open();
                        command.CommandText = @"SELECT Role,Email FROM [IflowSeed].[dbo].[User]                               
                                      WHERE Fullname=@Fullname";
                        command.Parameters.AddWithValue("@Fullname", get.AgreedBy.ToString());
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(0) == false)
                            {
                                AgreedPos = reader.GetString(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                AgreedEmail = reader.GetString(1);
                            }
                        }
                        cn2.Close();
                    }
                }

                if (get.ApprovedBy != "Please Select" && !string.IsNullOrEmpty(get.ApprovedBy))
                {
                    //APPROVED BY
                    using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    using (SqlCommand command = new SqlCommand("", cn2))
                    {
                        cn2.Open();
                        command.CommandText = @"SELECT Role,Email FROM [IflowSeed].[dbo].[User]                               
                                      WHERE Fullname=@Fullname";
                        command.Parameters.AddWithValue("@Fullname", get.ApprovedBy.ToString());
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(0) == false)
                            {
                                ApprovedPos = reader.GetString(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                ApprovedEmail = reader.GetString(1);
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

                    ViewBag.JoinPreparedPos = JoinPreparedPos;
                    ViewBag.JoinPreparedEmail = JoinPreparedEmail;

                    ViewBag.ConsultationPos = ConsultationPos;
                    ViewBag.ConsultationEmail = ConsultationEmail;

                    ViewBag.RecommandedPos = RecommandedPos;
                    ViewBag.RecommandedEmail = RecommandedEmail;

                    ViewBag.VerifiedPos = VerifiedPos;
                    ViewBag.VerifiedEmail = VerifiedEmail;

                    ViewBag.EndorsedPos = EndorsedPos;
                    ViewBag.EndorsedEmail = EndorsedEmail;

                    ViewBag.ReviewedPos = ReviewedPos;
                    ViewBag.ReviewedEmail = ReviewedEmail;

                    ViewBag.AgreedPos = AgreedPos;
                    ViewBag.AgreedEmail = AgreedEmail;

                    ViewBag.ApprovedPos = ApprovedPos;
                    ViewBag.ApprovedEmail = ApprovedEmail;

                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                    string DateTxt = DateTime.Now.ToString("yyyy-MM-dd");

                    cn.Open();
                    command.CommandText = @"UPDATE [IflowSeed].[dbo].[MOM]
                                            SET Subject = @Subject ,Descr = @Descr,PreparedBy = @PreparedBy,PreparedPos = @PreparedPos,PreparedDate = @PreparedDate,RecommandedBy = @RecommandedBy
                                            ,RecommandedPos = @RecommandedPos,RecommandedDate = @RecommandedDate,AgreedBy = @AgreedBy ,AgreedPos = @AgreedPos ,AgreedDate = @AgreedDate,VerifiedBy = @VerifiedBy
                                            ,VerifiedPos = @VerifiedPos,VerifiedDate = @VerifiedDate,ApprovedBy = @ApprovedBy,ApprovedPos = @ApprovedPos,ApprovedDate = @ApprovedDate,ReviewedBy = @ReviewedBy,ReviewedPos = @ReviewedPos,ReviewedDate = @ReviewedDate,EndorsedBy = @EndorsedBy,EndorsedPos = @EndorsedPos,EndorsedDate = @EndorsedDate,ConsultationBy = @ConsultationBy,ConsultationPos = @ConsultationPos,ConsultationDate = @ConsultationDate,JoinPreparedBy = @JoinPreparedBy,JoinPreparedPos = @JoinPreparedPos,JoinPreparedDate = @JoinPreparedDate
                                            WHERE  gID = @gID and uID = @uID";

                    command.Parameters.AddWithValue("@gID", id.ToString());
                    command.Parameters.AddWithValue("@uID", uID);

                    //MGNo = @MGNo
                    //command.Parameters.AddWithValue("@MGNo", get.MGMNo);

                    command.Parameters.AddWithValue("@Subject", get.Subject);
                    command.Parameters.AddWithValue("@Descr", get.Descr);



                    if (get.PreparedBy == "Please Select")
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@PreparedBy", Value = "Please Select" });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@PreparedPos", Value = DBNull.Value });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@PreparedDate", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@PreparedBy", get.PreparedBy);
                        command.Parameters.AddWithValue("@PreparedPos", PreparedPos);
                        command.Parameters.AddWithValue("@PreparedDate", DateTxt);
                    }

                    if (get.RecommandedBy == "Please Select")
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@RecommandedBy", Value = "Please Select" });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@RecommandedPos", Value = DBNull.Value });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@RecommandedDate", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RecommandedBy", get.RecommandedBy);
                        command.Parameters.AddWithValue("@RecommandedPos", RecommandedPos);
                        command.Parameters.AddWithValue("@RecommandedDate", DateTxt);
                    }

                    if (get.AgreedBy == "Please Select")
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@AgreedBy", Value = "Please Select" });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@AgreedPos", Value = DBNull.Value });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@AgreedDate", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@AgreedBy", get.AgreedBy);
                        command.Parameters.AddWithValue("@AgreedPos", AgreedPos);
                        command.Parameters.AddWithValue("@AgreedDate", DateTxt);
                    }

                    if (get.VerifiedBy == "Please Select")
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@VerifiedBy", Value = "Please Select" });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@VerifiedPos", Value = DBNull.Value });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@VerifiedDate", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@VerifiedBy", get.VerifiedBy);
                        command.Parameters.AddWithValue("@VerifiedPos", VerifiedPos);
                        command.Parameters.AddWithValue("@VerifiedDate", DateTxt);
                    }

                    if (get.ApprovedBy == "Please Select")
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@ApprovedBy", Value = "Please Select" });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@ApprovedPos", Value = DBNull.Value });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@ApprovedDate", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ApprovedBy", get.ApprovedBy);
                        command.Parameters.AddWithValue("@ApprovedPos", ApprovedPos);
                        command.Parameters.AddWithValue("@ApprovedDate", DateTxt);
                    }

                    if (get.ReviewedBy == "Please Select")
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@ReviewedBy", Value = "Please Select" });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@ReviewedPos", Value = DBNull.Value });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@ReviewedDate", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ReviewedBy", get.ReviewedBy);
                        command.Parameters.AddWithValue("@ReviewedPos", ReviewedPos);
                        command.Parameters.AddWithValue("@ReviewedDate", DateTxt);
                    }

                    if (get.EndorsedBy == "Please Select")
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@EndorsedBy", Value = "Please Select" });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@EndorsedPos", Value = DBNull.Value });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@EndorsedDate", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@EndorsedBy", get.EndorsedBy);
                        command.Parameters.AddWithValue("@EndorsedPos", EndorsedPos);
                        command.Parameters.AddWithValue("@EndorsedDate", DateTxt);
                    }

                    if (get.ConsultationBy == "Please Select")
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@ConsultationBy", Value = "Please Select" });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@ConsultationPos", Value = DBNull.Value });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@ConsultationDate", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ConsultationBy", get.ConsultationBy);
                        command.Parameters.AddWithValue("@ConsultationPos", ConsultationPos);
                        command.Parameters.AddWithValue("@ConsultationDate", DateTxt);
                    }

                    if (get.JoinPreparedBy == "Please Select")
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@JoinPreparedBy", Value = "Please Select" });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@JoinPreparedPos", Value = DBNull.Value });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@JoinPreparedDate", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@JoinPreparedBy", get.JoinPreparedBy);
                        command.Parameters.AddWithValue("@JoinPreparedPos", JoinPreparedPos);
                        command.Parameters.AddWithValue("@JoinPreparedDate", DateTxt);
                    }
                    command.ExecuteNonQuery();
                    cn.Close();
                }
            }
            return RedirectToAction("ManageMOM", "MOM");
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public ActionResult GetFileMGM()
        {
            //gID
            //Viewbag gID to pass data to UI
            //declare var Id sebagai Session["Id"]
            var Id = Session["Id"];
            List<FileStoreManagement> viewFileStore = new List<FileStoreManagement>();
            if (Id != null)
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    int _bil = 1;
                    cn.Open();
                    command.CommandText = @"SELECT Picture_FileId,Id
                                      FROM [iflowSeed].[dbo].[FileStoreManagement]
                                      WHERE MGNo=@Id
                                     ORDER BY CreatedOn DESC";
                    command.Parameters.AddWithValue("@Id", Id.ToString());
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        FileStoreManagement model = new FileStoreManagement();
                        {
                            model.Bil = _bil++;
                            if (reader.IsDBNull(0) == false)
                            {
                                model.Picture_FileId = reader.GetString(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                model.Id = reader.GetGuid(1);
                            }
                        }
                        viewFileStore.Add(model);
                    }
                    cn.Close();
                }
            }
            return Json(viewFileStore);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public ActionResult UploadMGMFile(FileStoreUploaded FileUploadLocation, String MGMNo, string uID)
        {
            //gID
            //Viewbag gID to pass data to UI
            //declare var Id sebagai Session["Id"]
            var Id = Session["Id"];
            ViewBag.Id = Id;

            //uID
            ViewBag.uID = Session["Idx"].ToString();
            uID = ViewBag.uID;

            //Status
            //declare Status sebagai Session["Status"]
            var Status = Session["Status"];
            ViewBag.status = Status;

            //var Status = Session["Status"];
            ViewBag.IsDepart = @Session["Department"];
            ViewBag.IsRole = @Session["Role"];
            //MGNo = ViewBag.MGNo;
            //MGMNo = ViewBag.MGMNo;

            if (FileUploadLocation.FileUploadFile != null && Id.ToString() != null && FileUploadLocation.set == "save")
            {
                var fileName = Path.GetFileName(FileUploadLocation.FileUploadFile.FileName);
                var path = Path.Combine(Server.MapPath("~/FileStore"), fileName);
                FileUploadLocation.FileUploadFile.SaveAs(path);

                Debug.WriteLine("filepath :" + path);

                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    Guid guidId = Guid.NewGuid();
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                    cn2.Open();
                    SqlCommand command;
                    command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[FileStoreManagement] (Id,CreatedOn,Picture_FileId,Picture_Extension,Type_,UserId,MGNo) values (@Id,@CreatedOn,@Picture_FileId,@Picture_Extension,@Type_,@UserId,@MGNo)", cn2);
                    command.Parameters.AddWithValue("@Id", guidId);
                    command.Parameters.AddWithValue("@CreatedOn", createdOn);
                    command.Parameters.AddWithValue("@Picture_FileId", fileName.Trim());
                    command.Parameters.AddWithValue("@Picture_Extension", FileUploadLocation.FileUploadFile.ContentType);
                    command.Parameters.AddWithValue("@MGNo", Id);
                    command.Parameters.AddWithValue("@Type_", "MANAGEMENT PAPER");
                    command.Parameters.AddWithValue("@UserId", uID);
                    command.ExecuteNonQuery();
                    cn2.Close();

                    return RedirectToAction("AddMOM", "MOM", new { Id = Id });
                }
            }

            if (FileUploadLocation.set == "back")
            {
                return RedirectToAction("AddMOM", "MOM", new { Id = Id });
            }

            return View();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///

        public ActionResult DeleteFileStore(string Id)
        {
            Guid QMId = Guid.Empty;

            if (Id != null)
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    cn.Open();
                    command.CommandText = @"SELECT Picture_FileId
                                      FROM [IflowSeed].[dbo].[FileStoreManagement]
                                      WHERE Id=@Id";
                    command.Parameters.AddWithValue("@Id", Id.ToString());
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            var Picture_FileId = reader.GetString(0);
                            var path = Path.Combine(Server.MapPath("~/FileStore"), Picture_FileId);
                            System.IO.File.Delete(path);

                            using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                            {
                                cn3.Open();
                                SqlCommand command3;
                                command3 = new SqlCommand("DELETE [IflowSeed].[dbo].[FileStoreManagement] WHERE Id=@Id", cn3);
                                command3.Parameters.AddWithValue("@Id", Id);
                                command3.ExecuteNonQuery();
                                cn3.Close();
                            }
                        }

                        if (reader.IsDBNull(0) == false)
                        {
                            //QMId = reader.GetGuid(1);
                            //Session["Id"] = QMId;

                            return RedirectToAction("AddMOM", "MOM", new { Id = Session["Id"] });
                        }
                    }
                    cn.Close();
                }
            }

            return RedirectToAction("ManageQM", "QM", new { Id = QMId });
        }

        public ActionResult RejectMOM(string Id, string Status, string remark)
        {
            ViewBag.Id = Id;
            var IdentityName = @Session["Fullname"];
            var Role = @Session["Role"];
            var IsDepart = @Session["Department"];
            var EmailBy = @Session["Email"];

            if (Id != null && remark != null)
            {
                using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn3.Open();
                    SqlCommand command3;
                    command3 = new SqlCommand("UPDATE [IflowSeed].[dbo].[MOM] SET Status='Rejected',RemarkReject= @MgmtPprRemarkReject WHERE gID=@Id", cn3);
                    command3.Parameters.AddWithValue("@Id", Id);
                    command3.Parameters.AddWithValue("@MgmtPprRemarkReject", remark);
                    command3.ExecuteNonQuery();
                    cn3.Close();
                }
                return RedirectToAction("ManageMOM", "MOM");
            }
            return View();
        }
    }
}

    