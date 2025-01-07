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
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using iTextSharp.text.io;
using Microsoft.Office.Interop.Excel;

[MvcAppV2.FilterConfig.UserSessionActionFilter]
public class PrintingController : Controller
{

    public ActionResult BackToITO(Hist_ProductionSlip Hist_ProductionSlip, Hist_ProductionSlip get, ProductionSlip ProductionSlip, string line, string set, string ProductionSlipId, string JobRequest,
                                       string Status, string PlanDatePostOn, string ItSubmitOn, string CreateUser,
                                       string LogTagNo, string Customer_Name, string ProductName, string JobClass, string JobType, string Frequency,
                                       string AccQty, string ImpQty, string PageQty, string PrintSlipNo,
                                       string NotesByIT, string NotesByProduction, string NotesByPurchasing, string NotesByEngineering, string NotesByArtwork, string NotesByFinance, string NotesByDCP, string Id, List<JobInstruction> selectedRows, string TextareaContent)
    {

        var IdentityName = @Session["Fullname"];
        Session["Id"] = Id;
        string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
        string StatusConcat = "Correction: " + TextareaContent; // Create the concatenated string

        if (set == "BlastBackToITO")
        {
            Session["Id"] = Id;
            foreach (var row in selectedRows)
            {
                string idAsString = row.Id.ToString();
                string textareaAsString = row.TextareaContent;

                string StatusConcat1 = "Correction: " + textareaAsString; // Create the concatenated string


                using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn1.Open();
                    SqlCommand command1;
                    command1 = new SqlCommand("UPDATE [JobInstruction] SET STATUS=@StatusConcat, PrintSlip = NULL WHERE Id=@Id", cn1);
                    command1.Parameters.AddWithValue("@Id", idAsString);
                    command1.Parameters.AddWithValue("@StatusConcat", StatusConcat1);
                    command1.ExecuteNonQuery();
                    cn1.Close();
                }

                using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn1.Open();
                    SqlCommand command1;
                    command1 = new SqlCommand("DELETE FROM [Hist_ProductionSlip] WHERE Id=@Id", cn1);
                    command1.Parameters.AddWithValue("@Id", idAsString);
                    command1.ExecuteNonQuery();
                    cn1.Close();
                }

            }

            return RedirectToAction("ManagePrint", "Printing");
        }

        using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        {
            cn1.Open();
            SqlCommand command1;
            command1 = new SqlCommand("UPDATE [JobInstruction]SET STATUS=@StatusConcat, PrintSlip = NULL WHERE Id=@Id", cn1);
            command1.Parameters.AddWithValue("@Id", Id);
            command1.Parameters.AddWithValue("@StatusConcat", StatusConcat);
            command1.ExecuteNonQuery();
            cn1.Close();
        }

        using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        {
            cn1.Open();
            SqlCommand command1;
            command1 = new SqlCommand("DELETE FROM [Hist_ProductionSlip] WHERE Id=@Id", cn1);
            command1.Parameters.AddWithValue("@Id", Id);
            command1.ExecuteNonQuery();
            cn1.Close();
        }

        return RedirectToAction("ManagePrint", "Printing");
    }

    public ActionResult ManagePrint(string product, string set, string pageNumber,string LogTagSearch, string msg)
    {
        ViewBag.Message = msg;
        ViewBag.Department = Session["Department"].ToString();

        TempData["msg"] = "<script>alert('"+msg+"')</script>";

        if (pageNumber == null)
        {
            pageNumber = "0";
        }

        int pageNumberInt = int.Parse(pageNumber);

        // Use pageNumberInt as an integer variable
        // For example:
        int PageNumber100 = (pageNumberInt - 1) * 100;

        List<JobInstruction> JobInstructionlist1 = new List<JobInstruction>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {

            int _bil = 1;
            cn.Open();
            if (set == "search")
            {
                //command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass, JobType,JobSheetNo, 
                //                            StartDevDate, EndDevDate,AccountsQty,ImpressionQty, 
                //                            PagesQty,IT_SysNotes,Produc_PlanningNotes,PurchasingNotes,
                //                            EngineeringNotes ,ArtworkNotes,Acc_BillingNotes,PrintSlip
                //                         FROM [JobInstruction]                                    
                //                         WHERE ProductName LIKE @ProductName
                //                         AND Status = 'PRODUCTION'
                //                         AND PrintSlip = 'PENDING'
                //                         ORDER BY CreatedOn DESC ";

                command.CommandText = @"  SELECT MAX(JobAuditTrailDetail.Customer_Name) AS Customer_Name, MAX(JobAuditTrailDetail.ProductName) AS ProductName, MAX(JobAuditTrailDetail.JobClass) AS JobClass,
                                            MAX(JobAuditTrailDetail.JobType) AS JobType,MAX(JobAuditTrailDetail.JobSheetNo) AS JobSheetNo, SUM(CAST(JobAuditTrailDetail.AccQty AS INT)) as AccQty, SUM(CAST(JobAuditTrailDetail.ImpQty AS INT)) as ImpQty, SUM(CAST(JobAuditTrailDetail.PageQty AS INT)) as PageQty,JobAuditTrailDetail.LogTagNo AS LogTagNo,MAX(JobAuditTrailDetail.CreatedOn) as LogTagSendOn, MAX(DailyTracking.CreatedOn) AS LogTagSendTime, MAX(JobAuditTrailDetail.PrintSlip)
                                        FROM [JobAuditTrailDetail] FULL JOIN DailyTracking ON JobAuditTrailDetail.LogTagNo=DailyTracking.LogTagNo
                                        WHERE JobAuditTrailDetail.Status = 'PRODUCTION' AND JobAuditTrailDetail.LogTagNo LIKE @LogTagNo
                                        GROUP BY JobAuditTrailDetail.LogTagNo
										ORDER BY JobAuditTrailDetail.LogTagNo";

                command.Parameters.AddWithValue("LogTagNo", "%"+LogTagSearch+"%");
                Debug.WriteLine("LogTag Search : " + LogTagSearch);
            }
            else if (set == "GoTo")
            {
                if (pageNumber == "0")
                {
                    command.CommandText = @"SELECT JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.ProductName, JobAuditTrailDetail.JobClass,
                                            JobAuditTrailDetail.JobType,JobAuditTrailDetail.JobSheetNo, SUM(CAST(JobAuditTrailDetail.AccQty AS INT)) as AccQty, SUM(CAST(JobAuditTrailDetail.ImpQty AS INT)) as ImpQty, SUM(CAST(JobAuditTrailDetail.PageQty AS INT)) as PageQty,JobAuditTrailDetail.LogTagNo,DailyTracking.CreatedOn as LogTagSendOn, DailyTracking.LogTagSendTime
                                        FROM [JobAuditTrailDetail] FULL JOIN DailyTracking ON JobAuditTrailDetail.LogTagNo=DailyTracking.LogTagNo
                                        WHERE JobAuditTrailDetail.Status = 'PRODUCTION' AND JobAuditTrailDetail.LogTagNo=@LogTagNo
                                        GROUP BY JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.ProductName, JobAuditTrailDetail.JobClass,JobAuditTrailDetail.JobType,JobAuditTrailDetail.JobSheetNo,JobAuditTrailDetail.LogTagNo,DailyTracking.CreatedOn,DailyTracking.LogTagSendTime
                                        ORDER BY (SELECT NULL)
                                        OFFSET 0 ROWS
                                        FETCH NEXT 1000 ROWS ONLY";

                    //command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass
                    //                        , JobType,JobSheetNo, StartDevDate, EndDevDate
                    //                        ,AccountsQty,ImpressionQty, PagesQty,IT_SysNotes
                    //                        ,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes
                    //                     ,ArtworkNotes,Acc_BillingNotes,PrintSlip
                    //                    FROM [JobInstruction]
                    //                    WHERE Status = 'PRODUCTION'
                    //                    AND PrintSlip = 'PENDING'
                    //                    ORDER BY (SELECT NULL)
                    //                    OFFSET @PageNumber ROWS
                    //                    FETCH NEXT 100 ROWS ONLY";

                    command.Parameters.AddWithValue("LogTagNo", LogTagSearch);

                    //command.Parameters.AddWithValue("@PageNumber", pageNumberInt);

                }
                else
                {

                    command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass
                                            , JobType,JobSheetNo, StartDevDate, EndDevDate
                                            ,AccountsQty,ImpressionQty, PagesQty,IT_SysNotes
                                            ,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes
                                         ,ArtworkNotes,Acc_BillingNotes,PrintSlip
                                        FROM [JobInstruction]
                                        WHERE Status = 'PRODUCTION'
                                        AND PrintSlip = 'PENDING'";

                    command.Parameters.AddWithValue("@PageNumber100", PageNumber100);
                    //command.Parameters.AddWithValue("LogTagNo", LogTagSearch);

                }
            }
            else
            {

                //command.CommandText = @"SELECT JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.ProductName, JobAuditTrailDetail.JobClass,
                //                            JobAuditTrailDetail.JobType,JobAuditTrailDetail.JobSheetNo, SUM(CAST(JobAuditTrailDetail.AccQty AS INT)) as AccQty, SUM(CAST(JobAuditTrailDetail.ImpQty AS INT)) as ImpQty, SUM(CAST(JobAuditTrailDetail.PageQty AS INT)) as PageQty,JobAuditTrailDetail.LogTagNo,FORMAT(CONVERT(date, DailyTracking.CreatedOn), 'dd-MM-yyyy') as LogTagSendOn, DailyTracking.LogTagSendTime, JobAuditTrailDetail.PrintSlip
                //                        FROM [JobAuditTrailDetail] FULL JOIN DailyTracking ON JobAuditTrailDetail.LogTagNo=DailyTracking.LogTagNo
                //                        WHERE JobAuditTrailDetail.Status = 'PRODUCTION'
                //                        GROUP BY JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.ProductName, JobAuditTrailDetail.JobClass,JobAuditTrailDetail.JobType,JobAuditTrailDetail.JobSheetNo,JobAuditTrailDetail.LogTagNo,DailyTracking.CreatedOn,DailyTracking.LogTagSendTime,JobAuditTrailDetail.PrintSlip
                //                        ORDER BY (SELECT NULL)
                //                        OFFSET 0 ROWS
                //                        FETCH NEXT 1000 ROWS ONLY";

                //command.CommandText = @"SELECT JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.ProductName, JobAuditTrailDetail.JobClass,
                //                            JobAuditTrailDetail.JobType,JobAuditTrailDetail.JobSheetNo, SUM(CAST(JobAuditTrailDetail.AccQty AS INT)) as AccQty, SUM(CAST(JobAuditTrailDetail.ImpQty AS INT)) as ImpQty, SUM(CAST(JobAuditTrailDetail.PageQty AS INT)) as PageQty,JobAuditTrailDetail.LogTagNo,MAX(JobAuditTrailDetail.CreatedOn) as LogTagSendOn, DailyTracking.LogTagSendTime, JobAuditTrailDetail.PrintSlip
                //                        FROM [JobAuditTrailDetail] FULL JOIN DailyTracking ON JobAuditTrailDetail.LogTagNo=DailyTracking.LogTagNo
                //                        WHERE JobAuditTrailDetail.Status = 'PRODUCTION'
                //                        GROUP BY JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.ProductName, JobAuditTrailDetail.JobClass,JobAuditTrailDetail.JobType,JobAuditTrailDetail.JobSheetNo,JobAuditTrailDetail.LogTagNo,DailyTracking.LogTagSendTime,JobAuditTrailDetail.PrintSlip";

                command.CommandText = @"  SELECT MAX(JobAuditTrailDetail.Customer_Name) AS Customer_Name, MAX(JobAuditTrailDetail.ProductName) AS ProductName, MAX(JobAuditTrailDetail.JobClass) AS JobClass,
                                            MAX(JobAuditTrailDetail.JobType) AS JobType,MAX(JobAuditTrailDetail.JobSheetNo) AS JobSheetNo, SUM(CAST(JobAuditTrailDetail.AccQty AS INT)) as AccQty, SUM(CAST(JobAuditTrailDetail.ImpQty AS INT)) as ImpQty, SUM(CAST(JobAuditTrailDetail.PageQty AS INT)) as PageQty,JobAuditTrailDetail.LogTagNo AS LogTagNo,MAX(JobAuditTrailDetail.CreatedOn) as LogTagSendOn, MAX(DailyTracking.CreatedOn) AS LogTagSendTime, MAX(JobAuditTrailDetail.PrintSlip)
                                        FROM [JobAuditTrailDetail] FULL JOIN DailyTracking ON JobAuditTrailDetail.LogTagNo=DailyTracking.LogTagNo
                                        WHERE JobAuditTrailDetail.Status = 'PRODUCTION'
                                        GROUP BY JobAuditTrailDetail.LogTagNo
										ORDER BY JobAuditTrailDetail.LogTagNo";

            }

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    model.Bil = _bil++;

                    if (reader.IsDBNull(0) == false)
                    {
                        model.Customer_Name = reader.GetString(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        model.ProductName = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        model.JobClass = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        model.JobType = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        model.JobSheetNo = reader.GetString(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        model.AccountsQty = reader["AccQty"].ToString();
                    }
                    else
                    {
                        model.AccountsQty = "0";
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        model.ImpressionQty = reader["ImpQty"].ToString();
                    }
                    else
                    {
                        model.ImpressionQty = "0";
                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        model.PagesQty = reader["PageQty"].ToString();
                    }
                    else
                    {
                        model.PagesQty = "0";
                    }
                   
                    if (!reader.IsDBNull(8))
                    {
                        model.LogTagNo = reader.GetString(8);
                    }
                    if (!reader.IsDBNull(9))
                    {
                        model.DateITOtxt = reader["LogTagSendOn"].ToString();
                    }
                    if (!reader.IsDBNull(10))
                    {
                        model.DateMBDtxt = reader["LogTagSendOn"].ToString();
                    }
                    //model.DateITOtxt = model.DateQtxt + " " + model.DateMBDtxt;
                    if (!reader.IsDBNull(11))
                    {
                        model.PrintSlip = reader.GetString(11);
                    }
                    else
                    {
                        model.PrintSlip = "PENDING";
                    }
                    //if (reader.IsDBNull(10) == false)
                    //{
                    //    model.Id = reader.GetGuid(10);
                    //}



                }

                JobInstructionlist1.Add(model);

            }
            cn.Close();
        }

        return View(JobInstructionlist1);
    }

    public ActionResult SubmitInsert(string Id, string ProductionSlipId, string JobType)
    {
        var IdentityName = @Session["Fullname"];
        Session["Id"] = Id;
        string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");



        if (!string.IsNullOrEmpty(Id))
        {
            using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn1.Open();
                SqlCommand command1;
                command1 = new SqlCommand("UPDATE [JobInstruction] SET STATUS='INSERTING' WHERE Id=@Id", cn1);
                command1.Parameters.AddWithValue("@Id", Id);
                command1.ExecuteNonQuery();
                cn1.Close();
            }

            using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn1.Open();
                SqlCommand command1;
                command1 = new SqlCommand("UPDATE [Hist_ProductionSlip] SET STATUS='INSERTING' WHERE Id=@ProductionSlipId", cn1);
                command1.Parameters.AddWithValue("@ProductionSlipId", Id);
                command1.ExecuteNonQuery();
                cn1.Close();
            }
        }
        return RedirectToAction("ManagePrint", "Printing", new { Id = Session["Id"].ToString() });

    }


    List<ProductionSlip> ProductionSliplist = new List<ProductionSlip>();

    public ActionResult ManageProductionSlip(string Id, string ProductName, string set)
    {
        if (set == "search") //ini kalu user search product
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Id, JobRequest, PlanDatePostOn, ItSubmitOn,  Status, LogTagNo, Customer_Name,
                                       ProductName,FileId, JobClass,
                                       Frequency,JobType, AccountsQty, PagesQty, ImpressionQty,ProductionSlipId,
                                       NotesByIT,NotesByProduction,NotesByPurchasing,NotesByEngineering,NotesByArtwork,NotesByFinance,NotesByDCP,
                                       AccQty, PageQty, ImpQty
                                       FROM [ProductionSlip]
                                       ORDER BY Customer_Name";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ProductionSlip model = new ProductionSlip();
                    {
                        model.Bil = _bil++;
                        if (reader.IsDBNull(0) == false)
                        {
                            model.Id = reader.GetGuid(0);
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            model.JobRequest = reader.GetDateTime(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            model.PlanDatePostOn = reader.GetDateTime(2);
                        }
                        if (reader.IsDBNull(3) == false)
                        {
                            model.ItSubmitOn = reader.GetDateTime(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            model.Status = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            model.LogTagNo = reader.GetString(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.Customer_Name = reader.GetString(6);
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.StatusPlanner = reader.GetString(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.FileId = reader.GetString(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            model.JobClass = reader.GetString(9);
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            model.Frequency = reader.GetString(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            model.JobType = reader.GetString(11);
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            model.AccountsQty = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            model.PagesQty = reader.GetString(13);
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            model.ImpressionQty = reader.GetString(14);
                        }
                        if (reader.IsDBNull(15) == false)
                        {
                            model.ProductionSlipId = reader.GetGuid(15);
                        }
                        if (reader.IsDBNull(16) == false)
                        {
                            model.NotesByIT = reader.GetString(16);
                        }
                        if (reader.IsDBNull(17) == false)
                        {
                            model.NotesByProduction = reader.GetString(17);
                        }
                        if (reader.IsDBNull(18) == false)
                        {
                            model.NotesByPurchasing = reader.GetString(18);
                        }
                        if (reader.IsDBNull(19) == false)
                        {
                            model.NotesByEngineering = reader.GetString(19);
                        }
                        if (reader.IsDBNull(20) == false)
                        {
                            model.NotesByArtwork = reader.GetString(20);
                        }
                        if (reader.IsDBNull(21) == false)
                        {
                            model.NotesByFinance = reader.GetString(21);
                        }
                        if (reader.IsDBNull(22) == false)
                        {
                            model.NotesByDCP = reader.GetString(22);
                        }
                        if (reader.IsDBNull(23) == false)
                        {
                            model.AccQty = reader.GetString(23);
                        }
                        if (reader.IsDBNull(24) == false)
                        {
                            model.PageQty = reader.GetString(24);
                        }
                        if (reader.IsDBNull(25) == false)
                        {
                            model.ImpQty = reader.GetString(25);
                        }

                    }
                    ProductionSliplist.Add(model);
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
                command.CommandText = @"SELECT Id, JobRequest, PlanDatePostOn, ItSubmitOn,  Status, LogTagNo, Customer_Name,
                                       ProductName,FileId, JobClass,
                                       Frequency,JobType, AccountsQty, PagesQty, ImpressionQty,ProductionSlipId,
                                       NotesByIT,NotesByProduction,NotesByPurchasing,NotesByEngineering,NotesByArtwork,NotesByFinance,NotesByDCP,
                                       AccQty, PageQty, ImpQty
                                       FROM [ProductionSlip]                                
                                       ORDER BY Customer_Name";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ProductionSlip model = new ProductionSlip();
                    {
                        model.Bil = _bil++;
                        if (reader.IsDBNull(0) == false)
                        {
                            model.Id = reader.GetGuid(0);
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            model.JobRequest = reader.GetDateTime(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            model.PlanDatePostOn = reader.GetDateTime(2);
                        }
                        if (reader.IsDBNull(3) == false)
                        {
                            model.ItSubmitOn = reader.GetDateTime(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            model.Status = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            model.LogTagNo = reader.GetString(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.Customer_Name = reader.GetString(6);
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.StatusPlanner = reader.GetString(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.FileId = reader.GetString(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            model.JobClass = reader.GetString(9);
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            model.Frequency = reader.GetString(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            model.JobType = reader.GetString(11);
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            model.AccountsQty = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            model.PagesQty = reader.GetString(13);
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            model.ImpressionQty = reader.GetString(14);
                        }
                        if (reader.IsDBNull(15) == false)
                        {
                            model.ProductionSlipId = reader.GetGuid(15);
                        }
                        if (reader.IsDBNull(16) == false)
                        {
                            model.NotesByIT = reader.GetString(16);
                        }
                        if (reader.IsDBNull(17) == false)
                        {
                            model.NotesByProduction = reader.GetString(17);
                        }
                        if (reader.IsDBNull(18) == false)
                        {
                            model.NotesByPurchasing = reader.GetString(18);
                        }
                        if (reader.IsDBNull(19) == false)
                        {
                            model.NotesByEngineering = reader.GetString(19);
                        }
                        if (reader.IsDBNull(20) == false)
                        {
                            model.NotesByArtwork = reader.GetString(20);
                        }
                        if (reader.IsDBNull(21) == false)
                        {
                            model.NotesByFinance = reader.GetString(21);
                        }
                        if (reader.IsDBNull(22) == false)
                        {
                            model.NotesByDCP = reader.GetString(22);
                        }
                        if (reader.IsDBNull(22) == false)
                        {
                            model.NotesByDCP = reader.GetString(22);
                        }
                        if (reader.IsDBNull(23) == false)
                        {
                            model.AccQty = reader.GetString(23);
                        }
                        if (reader.IsDBNull(24) == false)
                        {
                            model.PageQty = reader.GetString(24);
                        }
                        if (reader.IsDBNull(25) == false)
                        {
                            model.ImpQty = reader.GetString(25);
                        }


                    }
                    ProductionSliplist.Add(model);
                }
                cn.Close();
            }
        }
        return View(ProductionSliplist); //hntr data ke ui
    }

    [ValidateInput(false)]
    public ActionResult ProductionSlipCreate(Hist_ProductionSlip ModelSample, string Set, Hist_ProductionSlip get, string PrintSlipNo, string StartDateOn, string StartTime, string EndDateOn, string EndTime,
                                             string AccountsQty, string PagesQty, string ImpressionQty,
                                             string Machine, string ProcessType, string SequenceStart, string SequenceEnd, string Recovery,
                                             string AccQty, string PageQty, string ImpQty, string FileName, string ProductionSlipId, string JobType,
                                             string NotesByIT, string NotesByProduction, string NotesByPurchasing, string NotesByEngineering, string NotesByArtwork,
                                             string NotesByFinance, string NotesByDCP, string JobSheetNo, string Page, string LogTagNo, string Id)
    {
        var IdentityName = @Session["Fullname"];
        var Idx = Session["Id"];
        string test = Page;

        ViewBag.IsDepart = @Session["Department"];
        ViewBag.IsRole = @Session["Role"];
        ViewBag.Department = Session["Department"].ToString();
        string Deptment = @Session["Department"].ToString();
        //ViewBag.ProductionSlipId = ProductionSlipId;
        //ViewBag.PrintSlipNo = PrintSlipNo;
        ViewBag.JobSheetNo = JobSheetNo;
        Debug.WriteLine("JobSheetNo : " + JobSheetNo);
        //ViewBag.Id = Id;
        string Page1 = "Done";

        //string status = Request.QueryString["status"];

        List<SelectListItem> List4 = new List<SelectListItem>();

        List4.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        List4.Add(new SelectListItem { Text = "CUT SHEET", Value = "CUT SHEET" });
        List4.Add(new SelectListItem { Text = "COMPUTER FORM", Value = "COMPUTER FORM" });
        List4.Add(new SelectListItem { Text = "DCP", Value = "DCP" });


        ViewData["Machine_"] = List4;


        List<SelectListItem> List3 = new List<SelectListItem>();

        List3.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        List3.Add(new SelectListItem { Text = "INSERTING", Value = "INSERTING" });
        List3.Add(new SelectListItem { Text = "SELFMAILER", Value = "SELFMAILER" });
        List3.Add(new SelectListItem { Text = "MMP", Value = "MMP" });

        ViewData["ProcessType_"] = List3;

        List<JobAuditTrailDetail> JobInstructionlist1 = new List<JobAuditTrailDetail>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        {
            cn.Open();

            //if (string.IsNullOrEmpty(JobSheetNo))
            //{
            //    SqlCommand cmdJobSheet = new SqlCommand("SELECT JobSheetNo FROM JobAuditTrailDetail WHERE Id=@Idx",cn);
            //    cmdJobSheet.Parameters.AddWithValue("@Idx", Guid.Parse(Id));
            //    SqlDataReader rmJS = cmdJobSheet.ExecuteReader();

            //    while (rmJS.Read())
            //    {
            //        JobSheetNo = rmJS.GetString(0);
            //    }
            //}

            using (SqlConnection cn6 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn6.Open();
                if (Id.ToString() == "00000000-0000-0000-0000-000000000000" && !string.IsNullOrEmpty(Id))
                {
                    Debug.WriteLine("Masuk First Load");
                    //SqlCommand cmd1 = new SqlCommand("SELECT TOP (1) AccQty, ImpQty, ImpQty,Status,FileId,JobSheetNo,Id FROM JobAuditTrailDetail WHERE JobSheetNo=@JobSheetNo", cn6);
                    SqlCommand cmd1 = new SqlCommand("SELECT TOP (1) JobAuditTrailDetail.AccQty, JobAuditTrailDetail.ImpQty, JobAuditTrailDetail.PageQty,JobAuditTrailDetail.Status,JobAuditTrailDetail.FileId,JobAuditTrailDetail.JobSheetNo," +
                        "JobAuditTrailDetail.Id,FORMAT(CONVERT(date, Hist_ProductionSlip.StartDateOn), 'yyyy-MM-dd') as StartDateOn, Hist_ProductionSlip.StartTime, FORMAT(CONVERT(date, Hist_ProductionSlip.EndDateOn), 'yyyy-MM-dd') as EndDateOn, " +
                        "Hist_ProductionSlip.EndTime, FORMAT(CONVERT(date, Hist_ProductionSlip.SequenceStart), 'yyyy-MM-dd') as SequenceStart , FORMAT(CONVERT(date, Hist_ProductionSlip.SequenceEnd), 'yyyy-MM-dd') as SequenceEnd , " +
                        "Hist_ProductionSlip.PrintSlipNo,JobAuditTrailDetail.LogTagNo,Hist_ProductionSlip.Machine,Hist_ProductionSlip.ProcessType,Hist_ProductionSlip.Recovery,JobInstruction.IT_SysNotes,JobInstruction.Produc_PlanningNotes," +
                        "JobInstruction.PurchasingNotes,JobInstruction.EngineeringNotes,JobInstruction.ArtworkNotes, JobInstruction.Acc_BillingNotes " +
                        "FROM JobAuditTrailDetail FULL JOIN Hist_ProductionSlip ON JobAuditTrailDetail.Id=Hist_ProductionSlip.ProductionSlipId FULL JOIN JobInstruction ON JobInstruction.JobSheetNo=JobAuditTrailDetail.JobSheetNo " +
                        "WHERE JobAuditTrailDetail.LogTagNo=@JobSheetNo", cn6);
                    cmd1.Parameters.AddWithValue("@JobSheetNo", LogTagNo);
                    SqlDataReader rm1 = cmd1.ExecuteReader();

                    if (rm1.HasRows)
                    {
                        while (rm1.Read())
                        {
                            ViewBag.AccQty = rm1.GetString(0);
                            ViewBag.ImpQty = rm1.GetString(1);
                            ViewBag.PageQty = rm1.GetString(2);
                            ViewBag.Status = rm1.GetString(3);
                            ViewBag.FileId = rm1.GetString(4);
                            ViewBag.JobSheetNo = rm1.GetString(5);
                            ViewBag.Id = rm1.GetGuid(6);

                            if (rm1.IsDBNull(7) == false)
                            {
                                ViewBag.StartDateOn = rm1["StartDateOn"].ToString();
                            }

                            if (rm1.IsDBNull(8) == false)
                            {
                                ViewBag.StartTime = rm1.GetString(8);
                            }

                            if (rm1.IsDBNull(9) == false)
                            {
                                ViewBag.EndDateOn = rm1["EndDateOn"].ToString();
                            }

                            if (rm1.IsDBNull(10) == false)
                            {
                                ViewBag.EndTime = rm1.GetString(10);
                            }
                            if (rm1.IsDBNull(11) == false)
                            {
                                ViewBag.SequenceStart = rm1["SequenceStart"].ToString();
                            }
                            if (rm1.IsDBNull(12) == false)
                            {
                                ViewBag.SequenceEnd = rm1["SequenceEnd"].ToString();
                            }
                            if (rm1.IsDBNull(13) == false)
                            {
                                ViewBag.PrintSlipNo = rm1.GetString(13);
                            }
                            if (rm1.IsDBNull(14) == false)
                            {
                                ViewBag.LogTagNo = rm1.GetString(14);
                            }
                            if (rm1.IsDBNull(15) == false)
                            {
                                ViewBag.Machine = rm1.GetString(15);
                            }
                            if (rm1.IsDBNull(16) == false)
                            {
                                ViewBag.ProcessType = rm1.GetString(16);
                            }
                            if (rm1.IsDBNull(17) == false)
                            {
                                ViewBag.Recovery = rm1.GetString(17);
                            }
                            if (rm1.IsDBNull(18) == false)
                            {
                                ViewBag.IT_SysNotes = rm1.GetString(18);
                            }
                            if (rm1.IsDBNull(19) == false)
                            {
                                ViewBag.Produc_PlanningNotes = rm1.GetString(19);
                            }
                            if (rm1.IsDBNull(20) == false)
                            {
                                ViewBag.PurchasingNotes = rm1.GetString(20);
                            }
                            if (rm1.IsDBNull(21) == false)
                            {
                                ViewBag.EngineeringNotes = rm1.GetString(21);
                            }
                            if (rm1.IsDBNull(22) == false)
                            {
                                ViewBag.ArtworkNotes = rm1.GetString(22);
                            }
                            if (rm1.IsDBNull(23) == false)
                            {
                                ViewBag.Acc_BillingNotes = rm1.GetString(23);
                            }

                        }

                    }
                }
                else
                {
                    Debug.WriteLine("Masuk custom load");
                    SqlCommand cmd1 = new SqlCommand("SELECT JobAuditTrailDetail.AccQty, JobAuditTrailDetail.ImpQty, JobAuditTrailDetail.PageQty,JobAuditTrailDetail.Status,JobAuditTrailDetail.FileId,JobAuditTrailDetail.JobSheetNo,JobAuditTrailDetail.Id,FORMAT(CONVERT(date, Hist_ProductionSlip.StartDateOn), 'yyyy-MM-dd') as StartDateOn, Hist_ProductionSlip.StartTime, FORMAT(CONVERT(date, Hist_ProductionSlip.EndDateOn), 'yyyy-MM-dd') as EndDateOn, Hist_ProductionSlip.EndTime, " +
                        "Hist_ProductionSlip.SequenceStart, Hist_ProductionSlip.SequenceEnd,Hist_ProductionSlip.PrintSlipNo,JobAuditTrailDetail.LogTagNo,Hist_ProductionSlip.Machine,Hist_ProductionSlip.ProcessType,Hist_ProductionSlip.Recovery, JobInstruction.IT_SysNotes,JobInstruction.Produc_PlanningNotes,JobInstruction.PurchasingNotes,JobInstruction.EngineeringNotes,JobInstruction.ArtworkNotes, JobInstruction.Acc_BillingNotes " +
                        "FROM JobAuditTrailDetail FULL JOIN Hist_ProductionSlip ON JobAuditTrailDetail.Id=Hist_ProductionSlip.ProductionSlipId FULL JOIN JobInstruction ON JobInstruction.JobSheetNo=JobAuditTrailDetail.JobSheetNo WHERE JobAuditTrailDetail.Id=@Id2", cn6);

                    cmd1.Parameters.AddWithValue("@Id2", Id);
                    SqlDataReader rm1 = cmd1.ExecuteReader();

                    if (rm1.HasRows)
                    {
                        while (rm1.Read())
                        {
                            ViewBag.AccQty = rm1.GetString(0);
                            ViewBag.ImpQty = rm1.GetString(1);
                            ViewBag.PageQty = rm1.GetString(2);
                            ViewBag.Status = rm1.GetString(3);
                            ViewBag.FileId = rm1.GetString(4);
                            ViewBag.JobSheetNo = rm1.GetString(5);
                            ViewBag.Id = rm1.GetGuid(6);

                            if (rm1.IsDBNull(7) == false)
                            {
                                ViewBag.StartDateOn = rm1["StartDateOn"].ToString();
                            }

                            if (rm1.IsDBNull(8) == false)
                            {
                                ViewBag.StartTime = rm1.GetString(8);
                            }

                            if (rm1.IsDBNull(9) == false)
                            {
                                ViewBag.EndDateOn = rm1["EndDateOn"].ToString();
                            }

                            if (rm1.IsDBNull(10) == false)
                            {
                                ViewBag.EndTime = rm1.GetString(10);
                            }
                            if (rm1.IsDBNull(11) == false)
                            {
                                ViewBag.SequenceStart = rm1.GetString(11);
                            }
                            if (rm1.IsDBNull(12) == false)
                            {
                                ViewBag.SequenceEnd = rm1.GetString(12);
                            }
                            if (rm1.IsDBNull(13) == false)
                            {
                                ViewBag.PrintSlipNo = rm1.GetString(13);
                            }
                            if (rm1.IsDBNull(14) == false)
                            {
                                ViewBag.LogTagNo = rm1.GetString(14);
                            }
                            if (rm1.IsDBNull(15) == false)
                            {
                                ViewBag.Machine = rm1.GetString(15);
                            }
                            if (rm1.IsDBNull(16) == false)
                            {
                                ViewBag.ProcessType = rm1.GetString(16);
                            }
                            if (rm1.IsDBNull(17) == false)
                            {
                                ViewBag.Recovery = rm1.GetString(17);
                            }
                            if (rm1.IsDBNull(18) == false)
                            {
                                ViewBag.IT_SysNotes = rm1.GetString(18);
                            }
                            if (rm1.IsDBNull(19) == false)
                            {
                                ViewBag.Produc_PlanningNotes = rm1.GetString(19);
                            }
                            if (rm1.IsDBNull(20) == false)
                            {
                                ViewBag.PurchasingNotes = rm1.GetString(20);
                            }
                            if (rm1.IsDBNull(21) == false)
                            {
                                ViewBag.EngineeringNotes = rm1.GetString(21);
                            }
                            if (rm1.IsDBNull(22) == false)
                            {
                                ViewBag.ArtworkNotes = rm1.GetString(22);
                            }
                            if (rm1.IsDBNull(23) == false)
                            {
                                ViewBag.Acc_BillingNotes = rm1.GetString(23);
                            }

                        }

                    }
                }
                cn6.Close();


            }


            using (SqlCommand command = new SqlCommand("", cn))
            {

                int _bil = 1;
                command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass, JobType,JobSheetNo, 
                                            AccQty,ImpQty,PageQty,Status,PrintSlip, FileId, LogTagNo
                                        FROM [JobAuditTrailDetail]
                                        WHERE JobSheetNo=@JobSheetNo AND LogTagNo=@LogTagNo";
                command.Parameters.AddWithValue("@JobSheetNo", JobSheetNo);
                command.Parameters.AddWithValue("@LogTagNo", LogTagNo);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JobAuditTrailDetail model = new JobAuditTrailDetail();
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
                            model.JobClass = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            model.JobType = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            model.JobSheetNo = reader.GetString(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.AccQty = reader.GetString(6);
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.ImpQty = reader.GetString(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.PageQty = reader.GetString(8);
                        }

                        if (reader.IsDBNull(9) == false)
                        {
                            model.Status = reader.GetString(9);
                        }

                        if (reader.IsDBNull(10) == false)
                        {
                            model.PrintSlip = reader.GetString(10);
                        }
                        if (!reader.IsDBNull(10) == false)
                        {
                            model.PrintSlip = null;
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            model.FileId = reader.GetString(11);
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            model.LogTagNo = reader.GetString(12);
                        }

                    }

                    JobInstructionlist1.Add(model);

                }

            }

            

            cn.Close();



        }

        if (!string.IsNullOrEmpty(Id) && Set == "save")
        {
            if (string.IsNullOrEmpty(PrintSlipNo))
            {
                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                   
                    string createdOn1 = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                    get.StartDateOn = Convert.ToDateTime(get.StartDtOnTxt);
                    get.EndDateOn = Convert.ToDateTime(get.EndDtOnTxt);

                    /// insert
                    cn2.Open();

                    //get total logtag
                    SqlCommand CountLogTag = new SqlCommand("SELECT COUNT(LogTagNo) FROM JobAuditTrailDetail WHERE LogTagNo=@CountLogTag", cn2);
                    CountLogTag.Parameters.AddWithValue("@CountLogTag",LogTagNo);
                    SqlDataReader rmCount = CountLogTag.ExecuteReader();

                    int TotalLogTag = 0;

                    while(rmCount.Read())
                    {
                        TotalLogTag = rmCount.GetInt32(0);
                    }

                    //get file name & AIP value & Id
                    List<string> FileNames = new List<string>();
                    List<string> AccQtys = new List<string>();
                    List<string> ImpQtys= new List<string>();
                    List<string> PageQtys = new List<string>();
                    List<Guid> Ids = new List<Guid>();


                    SqlCommand GetFileName = new SqlCommand("SELECT FileId, AccQty, ImpQty, PageQty,Id FROM JobAuditTrailDetail WHERE LogTagNo = @FileLogTag", cn2);
                    GetFileName.Parameters.AddWithValue("@FileLogTag", LogTagNo);
                    SqlDataReader rmFile = GetFileName.ExecuteReader();

                    while(rmFile.Read())
                    {
                        FileNames.Add(rmFile.GetString(0));
                        if(rmFile.IsDBNull(1))
                        {
                            AccQtys.Add(rmFile.GetString(1));

                        }
                        else
                        {
                            AccQtys.Add("-");
                        }

                        if (rmFile.IsDBNull(2))
                        {
                            PageQtys.Add(rmFile.GetString(3));

                        }
                        else
                        {
                            PageQtys.Add("-");
                        }

                        if (rmFile.IsDBNull(3))
                        {
                            ImpQtys.Add(rmFile.GetString(3));

                        }
                        else
                        {
                            ImpQtys.Add("-");
                        }
                        Ids.Add(rmFile.GetGuid(4));
                    }

                    for (int i = 0; i<TotalLogTag;i++)
                    {
                        Guid guidId = Guid.NewGuid();
                        var No_ = new NoProductionModel();

                        SqlCommand command4;
                        command4 = new SqlCommand("INSERT INTO [Hist_ProductionSlip] (Id, CreatedOn, StartDateOn, StartTime, EndDateOn, LogTagNo, EndTime, Machine, ProcessType, SequenceStart, SequenceEnd, FileName, Status, PrintSlipNo, ProductionSlipId, CreateUser, AccQty, PageQty, ImpQty, Recovery, NotesByIT, NotesByProduction, NotesByPurchasing, NotesByEngineering, NotesByArtwork, NotesByFinance, NotesByDCP, JobSheetNo) values (@Id, @CreatedOn, @StartDateOn, @StartTime, @EndDateOn,@LogTagNo, @EndTime, @Machine, @ProcessType, @SequenceStart, @SequenceEnd, @FileName, @Status, @PrintSlipNo, @ProductionSlipId, @CreateUser, @AccQty, @PageQty, @ImpQty, @Recovery, @NotesByIT, @NotesByProduction ,@NotesByPurchasing, @NotesByEngineering, @NotesByArtwork, @NotesByFinance, @NotesByDCP, @JobSheetNo)", cn2);
                        command4.Parameters.AddWithValue("@Id", guidId);
                        command4.Parameters.AddWithValue("@CreatedOn", createdOn1);
                        if (!string.IsNullOrEmpty(StartDateOn))
                        {
                            string iiii = Convert.ToDateTime(StartDateOn).ToString("yyyy-MM-dd");
                            command4.Parameters.AddWithValue("@StartDateOn", iiii);
                        }
                        else
                        {
                            command4.Parameters.AddWithValue("@StartDateOn", DBNull.Value);
                        }
                        command4.Parameters.AddWithValue("@StartTime", StartTime);
                        if (!string.IsNullOrEmpty(EndDateOn))
                        {
                            string jjjj = Convert.ToDateTime(EndDateOn).ToString("yyyy-MM-dd");
                            command4.Parameters.AddWithValue("@EndDateOn", jjjj);
                        }
                        else
                        {
                            command4.Parameters.AddWithValue("@EndDateOn", DBNull.Value);
                        }

                        command4.Parameters.AddWithValue("@LogTagNo", LogTagNo);

                        if (!string.IsNullOrEmpty(EndTime))
                        {
                            command4.Parameters.AddWithValue("@EndTime", EndTime);
                        }
                        else
                        {
                            command4.Parameters.AddWithValue("@EndTime", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(Machine))
                        {
                            command4.Parameters.AddWithValue("@Machine", Machine);
                        }
                        else
                        {
                            command4.Parameters.AddWithValue("@Machine", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(ProcessType))
                        {
                            command4.Parameters.AddWithValue("@ProcessType", ProcessType);
                        }
                        else
                        {
                            command4.Parameters.AddWithValue("@ProcessType", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(SequenceStart))
                        {
                            command4.Parameters.AddWithValue("@SequenceStart", SequenceStart);
                        }
                        else
                        {
                            command4.Parameters.AddWithValue("@SequenceStart", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(SequenceEnd))
                        {
                            command4.Parameters.AddWithValue("@SequenceEnd", SequenceEnd);
                        }
                        else
                        {
                            command4.Parameters.AddWithValue("@SequenceEnd", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(FileNames[i]))
                        {
                            command4.Parameters.AddWithValue("@FileName", FileNames[i]);
                        }
                        else
                        {
                            command4.Parameters.AddWithValue("@FileName", DBNull.Value);
                        }


                        if (!string.IsNullOrEmpty(ProcessType))
                        {
                            command4.Parameters.AddWithValue("@Status", ProcessType);
                        }
                        else
                        {
                            command4.Parameters.AddWithValue("@Status", DBNull.Value);
                        }


                        command4.Parameters.AddWithValue("@PrintSlipNo", No_.RefNo);
                        command4.Parameters.AddWithValue("@ProductionSlipId", Ids[i]);
                        command4.Parameters.AddWithValue("@CreateUser", IdentityName.ToString());

                        if (AccQtys[i] !="-")
                        {
                            command4.Parameters.AddWithValue("@AccQty", AccQtys[i]);

                        }
                        else
                        {
                            command4.Parameters.AddWithValue("@AccQty", DBNull.Value);

                        }


                        if (PageQtys[i] != "-")
                        {
                            command4.Parameters.AddWithValue("@PageQty", PageQtys[i]);

                        }
                        else
                        {
                            command4.Parameters.AddWithValue("@PageQty", DBNull.Value);

                        }

                        if (ImpQtys[i] != "-")
                        {
                            command4.Parameters.AddWithValue("@ImpQty", ImpQtys[i]);
                        }
                        else
                        {
                            command4.Parameters.AddWithValue("@ImpQty", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(Recovery))
                        {
                            command4.Parameters.AddWithValue("@Recovery", Recovery);
                        }
                        else
                        {
                            command4.Parameters.AddWithValue("@Recovery", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(NotesByIT))
                        {
                            command4.Parameters.AddWithValue("@NotesByIT", NotesByIT);
                        }
                        else
                        {
                            command4.Parameters.AddWithValue("@NotesByIT", DBNull.Value);

                        }

                        if (!string.IsNullOrEmpty(NotesByProduction))
                        {
                            command4.Parameters.AddWithValue("@NotesByProduction", NotesByProduction);
                        }
                        else
                        {
                            command4.Parameters.AddWithValue("@NotesByProduction", DBNull.Value);

                        }

                        if (!string.IsNullOrEmpty(NotesByPurchasing))
                        {
                            command4.Parameters.AddWithValue("@NotesByPurchasing", NotesByPurchasing);
                        }
                        else
                        {
                            command4.Parameters.AddWithValue("@NotesByPurchasing", DBNull.Value);

                        }

                        if (!string.IsNullOrEmpty(NotesByEngineering))
                        {
                            command4.Parameters.AddWithValue("@NotesByEngineering", NotesByEngineering);
                        }
                        else
                        {
                            command4.Parameters.AddWithValue("@NotesByEngineering", DBNull.Value);

                        }

                        if (!string.IsNullOrEmpty(NotesByArtwork))
                        {
                            command4.Parameters.AddWithValue("@NotesByArtwork", NotesByArtwork);
                        }
                        else
                        {
                            command4.Parameters.AddWithValue("@NotesByArtwork", NotesByArtwork);

                        }

                        if (!string.IsNullOrEmpty(NotesByFinance))
                        {
                            command4.Parameters.AddWithValue("@NotesByFinance", NotesByFinance);
                        }
                        else
                        {
                            command4.Parameters.AddWithValue("@NotesByFinance", DBNull.Value);

                        }

                        if (!string.IsNullOrEmpty(NotesByDCP))
                        {
                            command4.Parameters.AddWithValue("@NotesByDCP", NotesByDCP);
                        }
                        else
                        {
                            command4.Parameters.AddWithValue("@NotesByDCP", DBNull.Value);

                        }

                        command4.Parameters.AddWithValue("@JobSheetNo", JobSheetNo);
                        command4.ExecuteNonQuery();
                    }

                    
                    cn2.Close();
                    ///

                }

                using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn1.Open();
                    SqlCommand command1;
                    command1 = new SqlCommand("UPDATE [JobAuditTrailDetail] SET PrintSlip='CREATED' WHERE LogTagNo=@Id", cn1);
                    command1.Parameters.AddWithValue("@Id", LogTagNo);
                    command1.ExecuteNonQuery();
                    cn1.Close();
                }

            }
            else
            {
                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    Guid guidId = Guid.NewGuid();
                    var No_ = new NoProductionModel();
                    string createdOn1 = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                    get.StartDateOn = Convert.ToDateTime(get.StartDtOnTxt);
                    get.EndDateOn = Convert.ToDateTime(get.EndDtOnTxt);

                    /// insert
                    cn2.Open();

                    SqlCommand command4;
                    command4 = new SqlCommand("UPDATE [Hist_ProductionSlip] SET LogTagNo=@LogTagNo, StartDateOn = @StartDateOn, StartTime = @StartTime, EndDateOn = @EndDateOn, EndTime = @EndTime, Machine = @Machine, ProcessType = @ProcessType, SequenceStart = @SequenceStart, SequenceEnd = @SequenceEnd, FileName = @FileName, Status = @Status, PrintSlipNo = @PrintSlipNo, ProductionSlipId = @ProductionSlipId, CreateUser = @CreateUser, AccQty = @AccQty, PageQty = @PageQty, ImpQty = @ImpQty, Recovery = @Recovery, NotesByIT = @NotesByIT, NotesByProduction = @NotesByProduction, NotesByPurchasing = @NotesByPurchasing, NotesByEngineering = @NotesByEngineering, NotesByArtwork = @NotesByArtwork, NotesByFinance = @NotesByFinance, JobSheetNo = @JobSheetNo WHERE ProductionSlipId = @Id", cn2);
                    command4.Parameters.AddWithValue("@Id", Id);
                    if (!string.IsNullOrEmpty(StartDateOn))
                    {
                        string iiii = Convert.ToDateTime(StartDateOn).ToString("yyyy-MM-dd");
                        command4.Parameters.AddWithValue("@StartDateOn", iiii);
                    }
                    else
                    {
                        command4.Parameters.AddWithValue("@StartDateOn", DBNull.Value);
                    }
                    command4.Parameters.AddWithValue("@StartTime", StartTime);
                    if (!string.IsNullOrEmpty(EndDateOn))
                    {
                        string jjjj = Convert.ToDateTime(EndDateOn).ToString("yyyy-MM-dd");
                        command4.Parameters.AddWithValue("@EndDateOn", jjjj);
                    }
                    else
                    {
                        command4.Parameters.AddWithValue("@EndDateOn", DBNull.Value);
                    }
                    command4.Parameters.AddWithValue("@LogTagNo", LogTagNo);
                    command4.Parameters.AddWithValue("@EndTime", EndTime);
                    command4.Parameters.AddWithValue("@Machine", Machine);
                    command4.Parameters.AddWithValue("@ProcessType", ProcessType);
                    if (!string.IsNullOrEmpty(SequenceStart))
                    {
                        command4.Parameters.AddWithValue("@SequenceStart", SequenceStart);
                    }
                    else
                    {
                        command4.Parameters.AddWithValue("@SequenceStart", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(SequenceEnd))
                    {
                        command4.Parameters.AddWithValue("@SequenceEnd", SequenceEnd);
                    }
                    else
                    {
                        command4.Parameters.AddWithValue("@SequenceEnd", DBNull.Value);
                    }
                    command4.Parameters.AddWithValue("@FileName", FileName);
                    command4.Parameters.AddWithValue("@Status", ProcessType);
                    command4.Parameters.AddWithValue("@PrintSlipNo", No_.RefNo);
                    command4.Parameters.AddWithValue("@ProductionSlipId", Id);
                    command4.Parameters.AddWithValue("@CreateUser", IdentityName.ToString());
                    command4.Parameters.AddWithValue("@AccQty", AccQty);
                    command4.Parameters.AddWithValue("@PageQty", PageQty);
                    command4.Parameters.AddWithValue("@ImpQty", ImpQty);

                    if (!string.IsNullOrEmpty(Recovery))
                    {
                        command4.Parameters.AddWithValue("@Recovery", Recovery);
                    }
                    else
                    {
                        command4.Parameters.AddWithValue("@Recovery", DBNull.Value);

                    }

                    if (!string.IsNullOrEmpty(NotesByIT))
                    {
                        command4.Parameters.AddWithValue("@NotesByIT", NotesByIT);
                    }
                    else
                    {
                        command4.Parameters.AddWithValue("@NotesByIT", DBNull.Value);

                    }

                    if (!string.IsNullOrEmpty(NotesByProduction))
                    {
                        command4.Parameters.AddWithValue("@NotesByProduction", NotesByProduction);
                    }
                    else
                    {
                        command4.Parameters.AddWithValue("@NotesByProduction", DBNull.Value);

                    }

                    if (!string.IsNullOrEmpty(NotesByPurchasing))
                    {
                        command4.Parameters.AddWithValue("@NotesByPurchasing", NotesByPurchasing);
                    }
                    else
                    {
                        command4.Parameters.AddWithValue("@NotesByPurchasing", DBNull.Value);

                    }

                    if (!string.IsNullOrEmpty(NotesByEngineering))
                    {
                        command4.Parameters.AddWithValue("@NotesByEngineering", NotesByEngineering);
                    }
                    else
                    {
                        command4.Parameters.AddWithValue("@NotesByEngineering", DBNull.Value);

                    }

                    if (!string.IsNullOrEmpty(NotesByArtwork))
                    {
                        command4.Parameters.AddWithValue("@NotesByArtwork", NotesByArtwork);
                    }
                    else
                    {
                        command4.Parameters.AddWithValue("@NotesByArtwork", NotesByArtwork);

                    }

                    if (!string.IsNullOrEmpty(NotesByFinance))
                    {
                        command4.Parameters.AddWithValue("@NotesByFinance", NotesByFinance);
                    }
                    else
                    {
                        command4.Parameters.AddWithValue("@NotesByFinance", DBNull.Value);

                    }

                    if (!string.IsNullOrEmpty(NotesByDCP))
                    {
                        command4.Parameters.AddWithValue("@NotesByDCP", NotesByDCP);
                    }
                    else
                    {
                        command4.Parameters.AddWithValue("@NotesByDCP", DBNull.Value);

                    }
                    command4.Parameters.AddWithValue("@JobSheetNo", JobSheetNo);
                    command4.ExecuteNonQuery();
                    cn2.Close();
                    ///

                }
            }

            return RedirectToAction("ProductionSlipCreate", "Printing", new {
                LogTagNo = ViewBag.LogTagNo,
                JobSheetNo = ViewBag.JobSheetNo,
                AccountsQty = ViewBag.AccQty,
                PagesQty = ViewBag.PageQty,
                ImpressionQty = ViewBag.ImpQty,
            });

        }
       
        if (Set == "back")
        {
            return RedirectToAction("ManagePrint", "Printing");
        }

        //if (status == "SubmitFail") 
        //{
        //    Page = "Start";
        //}

        if (Page == "Done")
        {
            return View(JobInstructionlist1);
        }


        return View(JobInstructionlist1);
    }



    public ActionResult ReloadPS()
    {


        List<Hist_ProductionSlip> viewFileStore = new List<Hist_ProductionSlip>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT Id, FileName, AccQty, PageQty, ImpQty, StartDateOn, StartTime, EndDateOn, EndTime,
                                           Machine, ProcessType, SequenceStart, SequenceEnd, PrintSlipNo,ProductionSlipId
                                      FROM [Hist_ProductionSlip]  
                                      WHERE ProductionSlipId=@";
            command.Parameters.AddWithValue("@Id", Session["ProductionSlipId"].ToString());
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Hist_ProductionSlip model = new Hist_ProductionSlip();
                {
                    model.Bil = _bil++;
                    if (reader.IsDBNull(0) == false)
                    {
                        model.Id = reader.GetGuid(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        model.FileName = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        model.AccQty = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        model.PageQty = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        model.ImpQty = reader.GetString(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        model.StartDtOnTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(5));
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        model.StartTime = reader.GetString(6);
                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        model.EndDtOnTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(7));
                    }
                    if (reader.IsDBNull(8) == false)
                    {
                        model.EndTime = reader.GetString(8);
                    }
                    if (reader.IsDBNull(9) == false)
                    {
                        model.Machine = reader.GetString(9);
                    }
                    if (reader.IsDBNull(10) == false)
                    {
                        model.ProcessType = reader.GetString(10);
                    }
                    if (reader.IsDBNull(11) == false)
                    {
                        model.SequenceStart = reader.GetString(11);
                    }
                    if (reader.IsDBNull(12) == false)
                    {
                        model.SequenceEnd = reader.GetString(12);
                    }
                    if (reader.IsDBNull(13) == false)
                    {
                        model.PrintSlipNo = reader.GetString(13);
                    }


                }
                viewFileStore.Add(model);
            }
            cn.Close();
            //return Json(new { data = viewFileStore }, JsonRequestBehavior.AllowGet);
            return Json(viewFileStore);
        }
    }



    List<Hist_ProductionSlip> viewSubmitProcess = new List<Hist_ProductionSlip>();

    //[ValidateInput(false)]
    public ActionResult SubmitProcessType(Hist_ProductionSlip Hist_ProductionSlip, Hist_ProductionSlip get, ProductionSlip ProductionSlip, string line, string set, string ProductionSlipId, string JobRequest,
                                          string Status, string PlanDatePostOn, string ItSubmitOn, string CreateUser,
                                          string LogTagNo, string Customer_Name, string ProductName, string JobClass, string JobType, string Frequency,
                                          string AccQty, string ImpQty, string PageQty, string PrintSlipNo,
                                          string NotesByIT, string NotesByProduction, string NotesByPurchasing, string NotesByEngineering, string NotesByArtwork, string NotesByFinance, string NotesByDCP, string Id, string JobSheetNo, List<JobInstruction> selectedRows)
    {

        var IdentityName = @Session["Fullname"];
        string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
        List<string> ProcessTypes= new List<string>();
        List<string> Ids = new List<string>();



        //if (set == "NoSlip")
        //{
        //    TempData["Message"] = "Slip is not created";
        //    return RedirectToAction("ManagePrint", "Printing");
        //}
        try
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand("SELECT PrintSlip FROM JobAuditTrailDetail WHERE LogTagNo = @LogTagNo ORDER BY PrintSlip ASC",cn);
                cmd.Parameters.AddWithValue("@LogTagNo", LogTagNo);
                SqlDataReader rm = cmd.ExecuteReader();

                if(rm.HasRows)
                {
                    while(rm.Read())
                    {
                        if(rm.IsDBNull(0))
                        {
                           return RedirectToAction("ManagePrint", "Printing", new { msg = "Submit Failed" });
                        }
                       

                    }
                }
                else
                {
                    return RedirectToAction("ManagePrint", "Printing", new { msg = "Submit Failed" });
                }

                cn.Close();
            }


            using (SqlConnection cn6 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command3 = new SqlCommand("", cn6))
            {
                cn6.Open();
                //command3.CommandText = @"SELECT JobAuditTrailDetail.AccountsQty, JobAuditTrailDetail.ImpressionQty, JobAuditTrailDetail.PagesQty, Hist_ProductionSlip.StartDateOn, Hist_ProductionSlip.StartTime, Hist_ProductionSlip.EndDateOn, Hist_ProductionSlip.EndTime, Hist_ProductionSlip.SequenceStart, Hist_ProductionSlip.SequenceEnd, Hist_ProductionSlip.Machine, Hist_ProductionSlip.Recovery, Hist_ProductionSlip.FileName,Hist_ProductionSlip.PrintSlipNo,
                //                         Hist_ProductionSlip.AccQty,Hist_ProductionSlip.ImpQty,Hist_ProductionSlip.PageQty,JobAuditTrailDetail.Status,Hist_ProductionSlip.ProcessType
                //                         FROM  JobAuditTrailDetail INNER JOIN
                //                    Hist_ProductionSlip ON JobAuditTrailDetail.LogTagNo = Hist_ProductionSlip.LogTagNo
                //                    WHERE JobAuditTrailDetail.LogTagNo=@Id";
                int i = 0;
                command3.CommandText = @"SELECT ProcessType, ProductionSlipId FROM Hist_ProductionSlip WHERE LogTagNo=@Id";
                command3.Parameters.AddWithValue("@Id", LogTagNo);
                var reader6 = command3.ExecuteReader();

                while (reader6.Read())
                {
                    ViewBag.ProcessType = reader6.GetString(0);
                    ProcessTypes.Add(reader6.GetString(0));
                    Ids.Add(reader6.GetString(1));
                    i++;

                }
                
                cn6.Close();
            }

            int idcount =0;
            foreach (var ProcessType in ProcessTypes)
            {
                Debug.WriteLine("Process Type : " + ProcessType);
                if (ProcessType == "INSERTING")
                {
                    int InsCheck = 0;
;

                    using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        cn1.Open();
                        SqlCommand command1;
                        command1 = new SqlCommand("UPDATE [Hist_ProductionSlip] SET STATUS='INSERTING', ModifiedOn=@Modified1 WHERE JobInstructionId=@Id", cn1);
                        command1.Parameters.AddWithValue("@Id", Ids[idcount]);
                        command1.Parameters.AddWithValue("@Modified1", DateTime.Now);

                        command1.ExecuteNonQuery();
                        cn1.Close();
                    }

                    using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        cn1.Open();
                        SqlCommand command1;
                        command1 = new SqlCommand("UPDATE [JobAuditTrailDetail] SET STATUS='INSERTING', ModifiedOn=@Modified2 WHERE Id=@Id", cn1);
                        command1.Parameters.AddWithValue("@Id", Ids[idcount]);
                        command1.Parameters.AddWithValue("@Modified2", DateTime.Now);

                        command1.ExecuteNonQuery();
                        cn1.Close();
                    }

                    using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        cn1.Open();
                        SqlCommand command1;
                        command1 = new SqlCommand("UPDATE [JobInstruction] SET InsertSlip='PENDING', PrintSlip = 'SUBMITTED' WHERE JobSheetNo=@JobSheetNo", cn1);
                        command1.Parameters.AddWithValue("@JobSheetNo", JobSheetNo);
                        command1.ExecuteNonQuery();
                        cn1.Close();
                    }

                    //if (InsCheck == 0)
                    //{



                    //    InsCheck++;

                    //}

                }

                if (ProcessType == "MMP")
                {
                    int MMPCheck = 0;
                    string now = DateTime.Now.ToString();


                    Debug.WriteLine("MMP");
                    using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        cn1.Open();
                        SqlCommand command1;
                        command1 = new SqlCommand("UPDATE [Hist_ProductionSlip] SET STATUS='MMP', ModifiedOn=@Modified3 WHERE Id=@Id", cn1);
                        command1.Parameters.AddWithValue("@Id", Ids[idcount]);
                        command1.Parameters.AddWithValue("@Modified3", DateTime.Now);
                        command1.ExecuteNonQuery();
                        cn1.Close();
                    }

                    using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        cn1.Open();
                        SqlCommand command1;
                        command1 = new SqlCommand("UPDATE [JobAuditTrailDetail] SET STATUS='MMP', ModifiedOn=@Modified4 WHERE Id=@Id", cn1);
                        command1.Parameters.AddWithValue("@Id", Ids[idcount]);
                        command1.Parameters.AddWithValue("@Modified4", DateTime.Now);

                        command1.ExecuteNonQuery();
                        cn1.Close();
                    }

                    using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        cn1.Open();
                        SqlCommand command1;
                        command1 = new SqlCommand("UPDATE [JobInstruction] SET MMPSlip='PENDING', PrintSlip = 'SUBMITTED' WHERE JobSheetNo=@JobSheetNo", cn1);
                        command1.Parameters.AddWithValue("@JobSheetNo", JobSheetNo);
                        command1.ExecuteNonQuery();
                        cn1.Close();
                    }

                }

                if (ProcessType == "MMPPickup")
                {
                    Debug.WriteLine("MMP");
                    using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        cn1.Open();
                        SqlCommand command1;
                        command1 = new SqlCommand("UPDATE [Hist_ProductionSlip] SET STATUS='MMPPickup' WHERE LogTagNo=@Id", cn1);
                        command1.Parameters.AddWithValue("@Id", LogTagNo);
                        command1.ExecuteNonQuery();
                        cn1.Close();
                    }

                    using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        cn1.Open();
                        SqlCommand command1;
                        command1 = new SqlCommand("UPDATE [JobAuditTrailDetail] SET STATUS='MMPPickup' WHERE LogTagNo=@Id", cn1);
                        command1.Parameters.AddWithValue("@Id", LogTagNo);
                        command1.ExecuteNonQuery();
                        cn1.Close();
                    }


                }

                if (ProcessType == "PRINT,INSERT")
                {
                    string now = DateTime.Now.ToString();

                    using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        cn1.Open();
                        SqlCommand command1;
                        command1 = new SqlCommand("UPDATE [Hist_ProductionSlip] SET STATUS='PRINT,INSERT AND RETURN', ModifiedOn=@Modified5 WHERE LogTagNo=@Id", cn1);
                        command1.Parameters.AddWithValue("@Id", LogTagNo);
                        command1.Parameters.AddWithValue("@Modified5", DateTime.Now);

                        command1.ExecuteNonQuery();
                        cn1.Close();
                    }

                    using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        cn1.Open();
                        SqlCommand command1;
                        command1 = new SqlCommand("UPDATE [JobAuditTrailDetail] SET STATUS='PRINT,INSERT AND RETURN' WHERE LogTagNo=@Id", cn1);
                        command1.Parameters.AddWithValue("@Id", LogTagNo);
                        command1.ExecuteNonQuery();
                        cn1.Close();
                    }


                }

                if (ProcessType == "SELFMAILER")
                {
                    string now = DateTime.Now.ToString();

                    int SMCheck = 0;

                    using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        cn1.Open();
                        SqlCommand command1;
                        command1 = new SqlCommand("UPDATE [Hist_ProductionSlip] SET STATUS='SELFMAILER', ModifiedOn=@Modified6 WHERE JobInstructionId=@Id", cn1);
                        command1.Parameters.AddWithValue("@Id", Ids[idcount]);
                        command1.Parameters.AddWithValue("@Modified6", DateTime.Now);
                        command1.ExecuteNonQuery();
                        cn1.Close();
                    }

                    using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        cn1.Open();
                        SqlCommand command1;
                        command1 = new SqlCommand("UPDATE [JobAuditTrailDetail] SET STATUS='SELFMAILER', ModifiedOn=@Modified7 WHERE Id=@Id", cn1);
                        command1.Parameters.AddWithValue("@Id", Ids[idcount]);
                        command1.Parameters.AddWithValue("@Modified7", DateTime.Now);
                        command1.ExecuteNonQuery();
                        cn1.Close();
                    }

                    using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        cn1.Open();
                        SqlCommand command1;
                        command1 = new SqlCommand("UPDATE [JobInstruction] SET SMSlip='PENDING', PrintSlip = 'SUBMITTED' WHERE JobSheetNo=@JobSheetNo", cn1);
                        command1.Parameters.AddWithValue("@JobSheetNo", JobSheetNo);
                        command1.ExecuteNonQuery();
                        cn1.Close();
                    }

                }

                idcount++;

            }


            return RedirectToAction("ManagePrint", "Printing", new { msg = "Submit Success" });
        }
        catch(Exception ex)
        {
            Debug.WriteLine(ex.Message);
            TempData["Message"] = "<script>alert('Slip is not created')</script>";

            return RedirectToAction("ManagePrint", "Printing");
        }


        return RedirectToAction("ManagePrint", "Printing", new { msg = "Submit Success" });
    }


    public ActionResult DeletePS(string Id, string ProductionSlipId, string PrintSlipNo, string set, string JobSheetNo, List<JobInstruction> selectedRows)
    {
        Session["Id"] = Id;
        ViewBag.Id = Id;
        ViewBag.ProductionSlipId = ProductionSlipId;
        ViewBag.PrintSlipNo = PrintSlipNo;


        if (set == "BlastDeleteProdSlip")
        {
            foreach (var row in selectedRows)
            {
                string idAsString = row.Id.ToString();

                if (idAsString != null)
                {
                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    using (SqlCommand command = new SqlCommand("", cn))
                    {
                        cn.Open();
                        command.CommandText = @"SELECT Id
                                          FROM [Hist_ProductionSlip]
                                          WHERE Id=@Id";
                        command.Parameters.AddWithValue("@Id", idAsString);
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(0) == false)
                            {

                                using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                                {
                                    cn3.Open();
                                    SqlCommand command3;
                                    command3 = new SqlCommand("DELETE [Hist_ProductionSlip] WHERE Id=@Id", cn3);
                                    command3.Parameters.AddWithValue("@Id", idAsString);
                                    command3.ExecuteNonQuery();
                                    cn3.Close();

                                }

                                using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                                {
                                    cn3.Open();
                                    SqlCommand command3;
                                    command3 = new SqlCommand("UPDATE [JobAuditTrailDetail] SET PrintSlip=NULL WHERE Id=@Id", cn3);
                                    command3.Parameters.AddWithValue("@Id", idAsString);
                                    command3.ExecuteNonQuery();
                                    cn3.Close();

                                }

                            }


                        }
                        cn.Close();
                    }
                }
            }
        }

        else if (Id != null)
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Id
                                          FROM [Hist_ProductionSlip]
                                          WHERE Id=@Id";
                command.Parameters.AddWithValue("@Id", Id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.IsDBNull(0) == false)
                    {

                        using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            cn3.Open();
                            SqlCommand command3;
                            command3 = new SqlCommand("DELETE [Hist_ProductionSlip] WHERE Id=@Id", cn3);
                            command3.Parameters.AddWithValue("@Id", Id);
                            command3.ExecuteNonQuery();
                            cn3.Close();

                        }

                        using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            cn3.Open();
                            SqlCommand command3;
                            command3 = new SqlCommand("UPDATE [JobAuditTrailDetail] SET PrintSlip=NULL WHERE Id=@Id", cn3);
                            command3.Parameters.AddWithValue("@Id", Id);
                            command3.ExecuteNonQuery();
                            cn3.Close();

                        }

                    }


                }
                cn.Close();
            }
        }

        return RedirectToAction("ManagePrint", "Printing");
    }






    [ValidateInput(false)]
    public ActionResult CreateProductionSlip(string set, string Id, string PrintSlipNo, string StartDateOn, string StartTime, string EndDateOn, string EndTime,
                                               string AccountsQty, string PagesQty, string ImpressionQty,
                                               string Machine, string ProcessType, string SequenceStart, string SequenceEnd,
                                               string AccQty, string PageQty, string ImpQty, string FileName, string ProductionSlipId,
                                               string IT_SysNotes, string Produc_PlanningNotes, string PurchasingNotes, string EngineeringNotes, string ArtworkNotes, string Acc_BillingNotes, string DCPNotes, string Confrm100)

    {

        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.AccountManager = IdentityName.ToString();
        ViewBag.AccountsQty = AccountsQty;
        ViewBag.PagesQty = PagesQty;
        ViewBag.ImpressionQty = ImpressionQty;
        ViewBag.ProductionSlipId = ProductionSlipId;
        Session["ProductionSlipId"] = Id;
        Session["Id"] = Id;
        ViewBag.Id = Id;
        ViewBag.PrintSlipNo = PrintSlipNo;
        ViewBag.AccQty = AccQty;
        ViewBag.PageQty = PageQty;
        ViewBag.ImpQty = ImpQty;




        using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn3))
        {
            cn3.Open();
            command.CommandText = command.CommandText = @" SELECT Id, Customer_Name, ProductName, JobClass, JobType,JobSheetNo, StartDevDate, EndDevDate,AccountsQty,ImpressionQty,PagesQty,IT_SysNotes,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes,
                                         ArtworkNotes,Acc_BillingNotes,DCPNotes,[Status]                                         
                                        FROM [JobInstruction]
                                         WHERE Id=@Id";
            command.Parameters.AddWithValue("@Id", Id);

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
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
                        ViewBag.JobClass = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        ViewBag.JobType = reader.GetString(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        ViewBag.JobSheetNo = reader.GetString(5);
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        ViewBag.StartDevOn = reader.GetDateTime(6);
                    }

                    if (reader.IsDBNull(7) == false)
                    {
                        ViewBag.EndDevDate = reader.GetDateTime(7);
                    }
                    if (reader.IsDBNull(8) == false)
                    {
                        ViewBag.AccountsQty = reader.GetString(8);
                    }
                    if (reader.IsDBNull(9) == false)
                    {
                        ViewBag.ImpressionQty = reader.GetString(9);
                    }
                    if (reader.IsDBNull(10) == false)
                    {
                        ViewBag.PagesQty = reader.GetString(10);
                    }
                    if (reader.IsDBNull(11) == false)
                    {
                        ViewBag.IT_SysNotes = reader.GetString(11);
                    }
                    if (reader.IsDBNull(12) == false)
                    {
                        ViewBag.Produc_PlanningNotes = reader.GetString(12);
                    }
                    if (reader.IsDBNull(12) == false)
                    {
                        ViewBag.PurchasingNotes = reader.GetString(12);
                    }
                    if (reader.IsDBNull(13) == false)
                    {
                        ViewBag.EngineeringNotes = reader.GetString(13);
                    }
                    if (reader.IsDBNull(14) == false)
                    {
                        ViewBag.ArtworkNotes = reader.GetString(14);
                    }
                    if (reader.IsDBNull(15) == false)
                    {
                        ViewBag.Acc_BillingNotes = reader.GetString(15);
                    }
                    if (reader.IsDBNull(16) == false)
                    {
                        ViewBag.DCPNotes = reader.GetString(16);
                    }
                    if (reader.IsDBNull(17) == false)
                    {
                        ViewBag.Status = reader.GetString(17);
                    }

                }

            }

            List<SelectListItem> App = new List<SelectListItem>();
            App.Add(new SelectListItem { Text = "PLEASE SELECT", Value = "" });
            App.Add(new SelectListItem { Text = "Yes", Value = "Yes" });
            App.Add(new SelectListItem { Text = "No", Value = "No" });
            ViewData["Confrm100_"] = App;

            if (set == "CreatePrint")
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn.Open();
                    SqlCommand command2;
                    command2 = new SqlCommand("UPDATE [JobInstruction] SET Confrm100=@Confrm100  WHERE Id=@Id", cn);
                    command2.Parameters.AddWithValue("@Confrm100", Confrm100);

                    command2.Parameters.AddWithValue("@Id", Id);
                    command2.ExecuteNonQuery();
                    cn.Close();
                }


            }

        }
        return View();

    }

    List<Hist_ProductionSlip> ViewProductionSliplist = new List<Hist_ProductionSlip>();


    public ActionResult ViewProductionSlip(string Id, string ProductionSlipId, string Set)
    {
        Session["ProductionSlipId"] = ProductionSlipId;
        ViewBag.Id = Id;
        ViewBag.ProductionSlipId = ProductionSlipId;

        if (Set == "back")
        {
            return RedirectToAction("ManageProductionSlip", "Printing");
        }

        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT Id, FileName, AccQty, PageQty, ImpQty, StartDateOn, StartTime, EndDateOn, EndTime,
                                           Machine, ProcessType, SequenceStart, SequenceEnd, PrintSlipNo,Status
                                           FROM [Hist_ProductionSlip]                                    
                                           WHERE ProductionSlipId LIKE @ProductionSlipId";
            command.Parameters.AddWithValue("@ProductionSlipId", "%" + ProductionSlipId + "%");
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Hist_ProductionSlip model = new Hist_ProductionSlip();
                {
                    model.Bil = _bil++;
                    if (reader.IsDBNull(0) == false)
                    {
                        model.Id = reader.GetGuid(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        model.FileName = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        model.AccQty = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        model.PageQty = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        model.ImpQty = reader.GetString(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        model.StartDateOn = reader.GetDateTime(5);
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        model.StartTime = reader.GetString(6);
                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        model.EndDateOn = reader.GetDateTime(7);
                    }
                    if (reader.IsDBNull(8) == false)
                    {
                        model.EndTime = reader.GetString(8);
                    }
                    if (reader.IsDBNull(9) == false)
                    {
                        model.Machine = reader.GetString(9);
                    }
                    if (reader.IsDBNull(10) == false)
                    {
                        model.ProcessType = reader.GetString(10);
                    }
                    if (reader.IsDBNull(11) == false)
                    {
                        model.SequenceStart = reader.GetString(11);
                    }
                    if (reader.IsDBNull(12) == false)
                    {
                        model.SequenceEnd = reader.GetString(12);
                    }
                    if (reader.IsDBNull(13) == false)
                    {
                        model.PrintSlipNo = reader.GetString(13);
                    }
                    if (reader.IsDBNull(14) == false)
                    {
                        model.Status = reader.GetString(14);
                    }



                }
                ViewProductionSliplist.Add(model);
            }
            cn.Close();
        }

        return View(ViewProductionSliplist); //hntr data ke ui
    }

    List<JobAuditTrailDetail> JobAuditTrailDetail = new List<JobAuditTrailDetail>();
    public ActionResult ViewAT(string ProductName, string LogTagNo, string set, string Id, string Customer_Name)
    {
        var IdentityName = @Session["Fullname"];
        ViewBag.IdentityName = @Session["Fullname"];
        ViewBag.IsDepart = @Session["Department"];
        var IsDepart = @Session["Department"];
        var Role = @Session["Role"];
        var Username = @Session["Username"];
        ViewBag.Username = @Session["Username"];
        Session["Id"] = Id;



        //ALL firt masuk
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Customer_Name, ProductName,  LogTagNo 
                                        FROM [JobAuditTrailDetail]										
                                        where Customer_Name=@Customer_Name ";
            command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobAuditTrailDetail model = new JobAuditTrailDetail();
                {
                    model.Bil = _bil++;

                    if (reader.IsDBNull(0) == false)
                    {
                        model.Customer_Name = reader.GetString(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        model.ProductName = reader.GetString(1);
                    }

                    if (reader.IsDBNull(2) == false)
                    {
                        model.LogTagNo = reader.GetString(2);
                    }




                }
                JobAuditTrailDetail.Add(model);
            }
            cn.Close();
        }




        return View(JobAuditTrailDetail);

    }







    public ActionResult ReportJATPDF(string Id, string Customer_Name, string LogTagNo, string AccQty, string RevStrtDateOn, string RevStrtTime, string ProcessDate, string TimeProcessIt, string ProcessEnd, string DateApproveTime, string TimeEndProcessIt)
    {
        //string ProcessDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");



        if (!string.IsNullOrEmpty(Id))
        {

            List<JobAuditTrailDetail> Product = new List<JobAuditTrailDetail>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT JobAuditTrailDetail.AccQty, JobAuditTrailDetail.ImpQty, JobAuditTrailDetail.PageQty, JobAuditTrailDetail.LogTagNo, JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.ProductName
                                       FROM  JobInstruction INNER JOIN
                                     JobAuditTrailDetail ON JobInstruction.Id = JobAuditTrailDetail.JobAuditTrailId
                                      where JobInstruction.Id = @Id ";
                command.Parameters.AddWithValue("@Id", Id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JobAuditTrailDetail model = new JobAuditTrailDetail();
                    {
                        model.Bil = _bil++;
                        if (reader.IsDBNull(0) == false)
                        {
                            model.AccQty = reader.GetString(0);
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            model.ImpQty = reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            model.PageQty = reader.GetString(2);
                        }
                        if (reader.IsDBNull(3) == false)
                        {
                            model.LogTagNo = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            model.Customer_Name = reader.GetString(4);
                        }

                        if (reader.IsDBNull(5) == false)
                        {
                            model.ProductName = reader.GetString(5);
                        }


                    }
                    Product.Add(model);
                }
                cn.Close();
            }



            using (SqlConnection cn4 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command2 = new SqlCommand("", cn4))
            {


                cn4.Open();
                command2.CommandText = @"SELECT
                                            SUM((CASE WHEN ISNUMERIC(AccQty)=1
                                            THEN CONVERT(decimal,AccQty) ELSE 0 END))                                           
                                            AS [TotalAccQty]
                                            FROM JobAuditTrailDetail                                    
                                         WHERE LogTagNo=@LogTagNo ";
                command2.Parameters.AddWithValue("@LogTagNo", LogTagNo);
                var reader2 = command2.ExecuteReader();
                while (reader2.Read())
                {
                    if (reader2.IsDBNull(0) == false)
                    {
                        ViewBag.TotalAccQty = reader2.GetDecimal(0);




                    }
                }



            }

            using (SqlConnection cn4 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command2 = new SqlCommand("", cn4))
            {


                cn4.Open();
                command2.CommandText = @"SELECT
                                            SUM((CASE WHEN ISNUMERIC(ImpQty)=1
                                            THEN CONVERT(decimal,ImpQty) ELSE 0 END))                                           
                                            AS [TotalImpQty]
                                            FROM JobAuditTrailDetail                                    
                                         WHERE LogTagNo=@LogTagNo ";
                command2.Parameters.AddWithValue("@LogTagNo", LogTagNo);
                var reader2 = command2.ExecuteReader();
                while (reader2.Read())
                {
                    if (reader2.IsDBNull(0) == false)
                    {
                        ViewBag.TotalImpQty = reader2.GetDecimal(0);




                    }
                }



            }

            using (SqlConnection cn4 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command2 = new SqlCommand("", cn4))
            {


                cn4.Open();
                command2.CommandText = @"SELECT
                                            SUM((CASE WHEN ISNUMERIC(PageQty)=1
                                            THEN CONVERT(decimal,PageQty) ELSE 0 END))                                           
                                            AS [TotalPageQty]
                                            FROM JobAuditTrailDetail                                    
                                         WHERE LogTagNo=@LogTagNo ";
                command2.Parameters.AddWithValue("@LogTagNo", LogTagNo);
                var reader2 = command2.ExecuteReader();
                while (reader2.Read())
                {
                    if (reader2.IsDBNull(0) == false)
                    {
                        ViewBag.TotalPageQty = reader2.GetDecimal(0);




                    }
                }



            }


        }



        if (!string.IsNullOrEmpty(Id))
        {


            List<JobAuditTrailDetail> JobAuditTrailDetail = new List<JobAuditTrailDetail>();

            int _bil = 1;
            using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn3))
            {


                cn3.Open();
                command.CommandText = @"SELECT LogTagNo, JobType, AccQty,ImpQty, PageQty, ProductName,Customer_Name
                                      FROM [JobAuditTrailDetail]
                                    WHERE LogTagNo=@LogTagNo ";
                command.Parameters.AddWithValue("@LogTagNo", LogTagNo);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JobAuditTrailDetail model = new JobAuditTrailDetail();
                    {
                        model.Bil = _bil++;
                        if (reader.IsDBNull(0) == false)
                        {
                            ViewBag.LogTagNo = reader.GetString(0);
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            ViewBag.JobType = reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            ViewBag.AccQty = reader.GetString(2);
                        }
                        if (reader.IsDBNull(3) == false)
                        {
                            ViewBag.ImpQty = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            ViewBag.PageQty = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            ViewBag.ProductName = reader.GetString(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            ViewBag.Customer_Name = reader.GetString(6);
                        }
                        ViewBag.JobAuditTrailDetail = JobAuditTrailDetail;

                    }
                }








            }






            int ic = 0;
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString()))
            {
                cn.Open();
                try
                {
                    SqlCommand cmd44 = new SqlCommand("Select Customer_Name, ProgramId, FileId,JobId,JobNameIT,RevStrtDateOn,RevStrtTime,ProcessDate,TimeProcessIt,ProcessEnd,AccQty,ImpQty,PageQty,FirstRecord,LastRecord,LogTagNo,ProductName,JobType,DateApproveOn,DateApproveTime,Type,DateProcessItOn  FROM JobAuditTrailDetail  where  LogTagNo=@LogTagNo ", cn);
                    cmd44.Parameters.Add(new SqlParameter("@LogTagNo", LogTagNo));
                    SqlDataReader rs4 = cmd44.ExecuteReader();
                    if (rs4.HasRows)
                    {

                        while (rs4.Read())
                        {
                            ic = ic + 1;
                            JobAuditTrailDetail eb = new JobAuditTrailDetail();
                            {
                                eb.Bil = ic;
                                eb.Customer_Name = rs4.GetString(0);
                                eb.ProgramId = rs4.GetString(1);
                                eb.FileId = rs4.GetString(2);
                                eb.JobId = rs4.GetString(3);
                                eb.JobNameIT = rs4.GetString(4);



                                eb.RevStrtDateOn = rs4.GetDateTime(5);



                                eb.RevStrtTime = rs4.GetString(6);



                                eb.ProcessDate = rs4.GetDateTime(7);


                                eb.TimeProcessIt = rs4.GetString(8);



                                eb.ProcessEnd = rs4.GetDateTime(9);




                                eb.AccQty = rs4.GetString(10);
                                eb.ImpQty = rs4.GetString(11);
                                eb.PageQty = rs4.GetString(12);
                                eb.FirstRecord = rs4.GetString(13);
                                eb.LastRecord = rs4.GetString(14);
                                eb.LogTagNo = rs4.GetString(15);
                                eb.ProductName = rs4.GetString(16);
                                eb.JobType = rs4.GetString(17);
                                eb.DateApproveOn = rs4.GetDateTime(18);
                                eb.DateApproveTime = rs4.GetString(19);
                                eb.Type = rs4.GetString(20);
                                eb.DateProcessItOn = rs4.GetDateTime(21);

                            };
                            liEB.Add(eb);
                        }
                    }
                    rs4.Close();
                    ViewBag.ListPDF = liEB.ToList();
                }
                catch (System.Exception err)
                {
                    TempData["msg"] = "<script>alert('" + err.Message + "');</script>";
                }
                finally
                {
                    cn.Close();
                }
            }
        }
        return new Rotativa.ViewAsPdf("ReportJATPDF", ViewBag.ListPDF)
        {
            PageMargins = new Rotativa.Options.Margins(5, 5, 5, 5),
            PageOrientation = Rotativa.Options.Orientation.Portrait,
        };
    }

    public ActionResult BackToITO2(string LogTagNo, string set, string from)
    {
        Debug.WriteLine("First From : " + from);
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString()))
        {
            cn.Open();

            SqlCommand cmd = new SqlCommand("SELECT ProductName FROM JobAuditTrailDetail WHERE LogTagNo = @LogTagNo",cn);
            cmd.Parameters.AddWithValue("@logTagNo", LogTagNo);
            SqlDataReader rm = cmd.ExecuteReader();

            while(rm.Read())
            {
                ViewBag.ProductName = rm.GetString(0);
            }

            ViewBag.LogTagNo = LogTagNo;
            ViewBag.from = from;

            cn.Close();


        }
        
        return View();
    }




    List<JobAuditTrailDetail> liEB = new List<JobAuditTrailDetail>();
    private double totalamount;



}


