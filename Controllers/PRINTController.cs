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

[MvcAppV2.FilterConfig.UserSessionActionFilter]
public class PRINTController : Controller
{

    List<Hist_ProductionSlip> viewInsertingProcess = new List<Hist_ProductionSlip>();

    public ActionResult ManageProcessPrint(string Id, string ProductName, string set)
    {
        if (set == "search") //ini kalu user search product
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Id, PrintSlipNo, FileName, ProcessType, Ins_Machine, Ins_StartDateOn, Ins_StartTime, Ins_EndDateOn, Ins_EndTime,
                                               AccQty, PageQty, ImpQty,Status, SequenceStart, SequenceEnd
                                               FROM [IflowSeed].[dbo].[Hist_ProductionSlip]
                                               WHERE (Status = 'PRINT,INSERT AND RETURN') AND (ProcessType='PRINT,INSERT AND RETURN') OR (Status = 'READY TO PRINT') AND (ProcessType='PRINT,INSERT AND RETURN')
                                               ORDER BY Customer_Name";
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
                            model.PrintSlipNo = reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            model.FileName = reader.GetString(2);
                        }
                        if (reader.IsDBNull(3) == false)
                        {
                            model.ProcessType = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            model.Ins_Machine = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            model.Ins_StartDateOn = reader.GetDateTime(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.Ins_StartTime = reader.GetString(6);
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.Ins_EndDateOn = reader.GetDateTime(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.Ins_EndTime = reader.GetString(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            model.AccQty = reader.GetString(9);
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            model.PageQty = reader.GetString(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            model.ImpQty = reader.GetString(11);
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            model.Status = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            model.SequenceStart = reader.GetString(13);
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            model.SequenceEnd = reader.GetString(14);
                        }

                    }
                    viewInsertingProcess.Add(model);
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
                command.CommandText = @"SELECT Id, PrintSlipNo, FileName, ProcessType, Ins_Machine, Ins_StartDateOn, Ins_StartTime, Ins_EndDateOn, Ins_EndTime,
                                               AccQty, PageQty, ImpQty,Status, SequenceStart, SequenceEnd
                                               FROM [IflowSeed].[dbo].[Hist_ProductionSlip] 
                                               WHERE (Status = 'PRINT,INSERT AND RETURN') AND (ProcessType='PRINT,INSERT AND RETURN') OR (Status = 'READY TO PRINT') AND (ProcessType='PRINT,INSERT AND RETURN')
                                               ORDER BY Customer_Name";
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
                            model.PrintSlipNo = reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            model.FileName = reader.GetString(2);
                        }
                        if (reader.IsDBNull(3) == false)
                        {
                            model.ProcessType = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            model.Ins_Machine = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            model.Ins_StartDateOn = reader.GetDateTime(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.Ins_StartTime = reader.GetString(6);
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.Ins_EndDateOn = reader.GetDateTime(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.Ins_EndTime = reader.GetString(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            model.AccQty = reader.GetString(9);
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            model.PageQty = reader.GetString(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            model.ImpQty = reader.GetString(11);
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            model.Status = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            model.SequenceStart = reader.GetString(13);
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            model.SequenceEnd = reader.GetString(14);
                        }

                    }
                    viewInsertingProcess.Add(model);
                }
                cn.Close();
            }
        }
        return View(viewInsertingProcess); //hntr data ke ui
    }


    public ActionResult CreateProdSlip(Hist_ProductionSlip get, string set, string Id, string PrintSlipNo, string Ins_StartDateOn, string Ins_StartTime, string Ins_EndDateOn, string Ins_EndTime,
                                       string Ins_Machine, string Process, string Sort, string NonSort, string ProcessType, string Ins_CreateUser, string Recovery,
                                       string NotesByIT, string NotesByProduction, string NotesByPurchasing, string NotesByEngineering, string NotesByArtwork, string NotesByFinance, string NotesByDCP)

    {

        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.AccountManager = IdentityName.ToString();
        Session["Id"] = Id;
        ViewBag.Id = Id;
        ViewBag.PrintSlipNo = PrintSlipNo;

        List<SelectListItem> li = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Machine FROM [IflowSeed].[dbo].[MachineType]";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Hist_ProductionSlip model = new Hist_ProductionSlip();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.Ins_Machine = reader.GetString(0);
                    }
                }
                int i = _bil++;
                if (i == 1)
                {
                    li.Add(new SelectListItem { Text = "Please Select" });

                }
                li.Add(new SelectListItem { Text = model.Ins_Machine });
            }
            cn.Close();
        }
        ViewData["MachineType_"] = li;


        List<SelectListItem> li2 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Process FROM [IflowSeed].[dbo].[ProcessType] ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Hist_ProductionSlip model = new Hist_ProductionSlip();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.ProcessType = reader.GetString(0);
                    }
                }
                int i2 = _bil++;
                if (i2 == 1)
                {
                    li2.Add(new SelectListItem { Text = "Please Select" });

                }
                li2.Add(new SelectListItem { Text = model.ProcessType });
            }
            cn.Close();
        }
        ViewData["ProcessType_"] = li2;



        if (set == "CreateProductionSlip")
        {
            if (!string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Ins_StartDateOn) && !string.IsNullOrEmpty(Ins_StartTime) && !string.IsNullOrEmpty(Ins_EndDateOn) && !string.IsNullOrEmpty(Ins_EndTime) && !string.IsNullOrEmpty(Ins_Machine))
            {

                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                    get.Ins_StartDateOn = Convert.ToDateTime(get.Ins_StartDateOnTxt);
                    get.Ins_EndDateOn = Convert.ToDateTime(get.Ins_EndDateOnTxt);
                    cn2.Open();
                    SqlCommand command;
                    command = new SqlCommand("UPDATE [IflowSeed].[dbo].[Hist_ProductionSlip] SET ModifiedOn=@ModifiedOn,Ins_StartDateOn=@Ins_StartDateOn,Ins_StartTime=@Ins_StartTime,Ins_EndDateOn=@Ins_EndDateOn,Ins_EndTime=@Ins_EndTime,Ins_Machine=@Ins_Machine, Sort=@Sort, NonSort=@NonSort, /*Process=@Process,*/Ins_CreateUser=@Ins_CreateUser, Status=@Status, Recovery=@Recovery WHERE Id=@Id", cn2);
                    command.Parameters.AddWithValue("@ModifiedOn", createdOn);
                    if (!string.IsNullOrEmpty(Ins_StartDateOn))
                    {
                        string a7 = Convert.ToDateTime(Ins_StartDateOn).ToString("yyyy-MM-dd");
                        command.Parameters.AddWithValue("@Ins_StartDateOn", a7);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Ins_StartDateOn", null);
                    }
                    command.Parameters.AddWithValue("@Ins_StartTime", Ins_StartTime);
                    if (!string.IsNullOrEmpty(Ins_EndDateOn))
                    {
                        string a8 = Convert.ToDateTime(Ins_EndDateOn).ToString("yyyy-MM-dd");
                        command.Parameters.AddWithValue("@Ins_EndDateOn", a8);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Ins_EndDateOn", null);
                    }
                    command.Parameters.AddWithValue("@Ins_EndTime", Ins_EndTime);
                    command.Parameters.AddWithValue("@Ins_Machine", Ins_Machine);
                    //command.Parameters.AddWithValue("@Process", Process);
                    if (Sort == "on")
                    {
                        command.Parameters.AddWithValue("@Sort", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Sort", false);
                    }
                    if (NonSort == "on")
                    {
                        command.Parameters.AddWithValue("@NonSort", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@NonSort", false);
                    }
                    command.Parameters.AddWithValue("@Ins_CreateUser", IdentityName.ToString());
                    command.Parameters.AddWithValue("@Status", "READY TO PRINT");
                    command.Parameters.AddWithValue("@Recovery", Recovery);
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                    cn2.Close();

                }

                //using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                //{
                //    cn1.Open();
                //    SqlCommand command1;
                //    command1 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobAuditTrailDetail] SET STATUS='POSTING' WHERE JobAuditTrailId=@JobAuditTrailId", cn1);
                //    command1.Parameters.AddWithValue("@JobAuditTrailId", JobAuditTrailId);
                //    command1.ExecuteNonQuery();
                //    cn1.Close();
                //}

                using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn3))
                {

                    cn3.Open();
                    command.CommandText = @"SELECT a.Id, b.LogTagNo, b.Customer_Name, b.ProductName, b.Status,
                                            b.JobType, b.JobClass, b.Frequency, b.AccQty, b.PageQty, b.ImpQty, a.TotalAuditTrail,
                                            b.JobInstructionId, b.JobAuditTrailId, a.AccountsQty, a.ImpressionQty, a.PagesQty
                                            FROM [IflowSeed].[dbo].[ProductionSlip] a, [IflowSeed].[dbo].[Hist_ProductionSlip] b
                                            WHERE a.Id=b.ProductionSlipId AND b.Id=@Id";
                    command.Parameters.AddWithValue("@Id", Id);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        PostingManifest model = new PostingManifest();
                        {
                            if (reader.IsDBNull(0) == false)
                            {
                                model.Id = reader.GetGuid(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                model.LogTagNo = reader.GetString(1);
                            }
                            if (reader.IsDBNull(2) == false)
                            {
                                model.Customer_Name = reader.GetString(2);
                            }
                            if (reader.IsDBNull(3) == false)
                            {
                                model.ProductName = reader.GetString(3);
                            }
                            if (reader.IsDBNull(4) == false)
                            {
                                model.Status = reader.GetString(4);
                            }
                            if (reader.IsDBNull(5) == false)
                            {
                                model.JobType = reader.GetString(5);
                            }
                            if (reader.IsDBNull(6) == false)
                            {
                                model.JobClass = reader.GetString(6);
                            }
                            if (reader.IsDBNull(7) == false)
                            {
                                model.Frequency = reader.GetString(7);
                            }
                            if (reader.IsDBNull(8) == false)
                            {
                                model.AccQty = reader.GetString(8);
                            }
                            if (reader.IsDBNull(9) == false)
                            {
                                model.PageQty = reader.GetString(9);
                            }
                            if (reader.IsDBNull(10) == false)
                            {
                                model.ImpQty = reader.GetString(10);
                            }
                            if (reader.IsDBNull(11) == false)
                            {
                                model.TotalAuditTrail = reader.GetString(11);
                            }
                            if (reader.IsDBNull(12) == false)
                            {
                                model.JobInstructionId = reader.GetGuid(12);
                            }
                            if (reader.IsDBNull(13) == false)
                            {
                                model.JobAuditTrailId = reader.GetGuid(13);
                            }
                            if (reader.IsDBNull(14) == false)
                            {
                                model.AccountsQty = reader.GetString(14);
                            }
                            if (reader.IsDBNull(15) == false)
                            {
                                model.PagesQty = reader.GetString(15);
                            }
                            if (reader.IsDBNull(16) == false)
                            {
                                model.ImpressionQty = reader.GetString(16);
                            }


                        }

                        using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            Guid WorksheetId = Guid.NewGuid();
                            ViewBag.Id = WorksheetId;
                            string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                            string ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                            cn2.Open();
                            SqlCommand command2;
                            command2 = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[PostingManifest] (Id, CreatedOn, LogTagNo, Customer_Name, ProductName, Status, JobType, JobClass, Frequency, AccQty, PageQty, ImpQty, TotalAuditTrail, JobInstructionId, JobAuditTrailId, ProductionSlipId, AccountsQty, ImpressionQty, PagesQty) values (@Id, @CreatedOn, @LogTagNo, @Customer_Name, @ProductName, @Status, @JobType, @JobClass, @Frequency, @AccQty, @PageQty, @ImpQty, @TotalAuditTrail, @JobInstructionId, @JobAuditTrailId, @ProductionSlipId, @AccountsQty, @ImpressionQty, @PagesQty)", cn2);
                            command2.Parameters.AddWithValue("@Id", WorksheetId);
                            command2.Parameters.AddWithValue("@CreatedOn", createdOn);
                            if (model.LogTagNo != null)
                            {
                                command2.Parameters.AddWithValue("@LogTagNo", model.LogTagNo);
                            }
                            else
                            {
                                command2.Parameters.AddWithValue("@LogTagNo", DBNull.Value);
                            }
                            if (model.Customer_Name != null)
                            {
                                command2.Parameters.AddWithValue("@Customer_Name", model.Customer_Name);
                            }
                            else
                            {
                                command2.Parameters.AddWithValue("@Customer_Name", DBNull.Value);
                            }
                            if (model.ProductName != null)
                            {
                                command2.Parameters.AddWithValue("@ProductName", model.ProductName);
                            }
                            else
                            {
                                command2.Parameters.AddWithValue("@ProductName", DBNull.Value);
                            }
                            if (model.Status != null)
                            {
                                command2.Parameters.AddWithValue("@Status", "New");
                            }
                            else
                            {
                                command2.Parameters.AddWithValue("@Status", DBNull.Value);
                            }
                            if (model.JobType != null)
                            {
                                command2.Parameters.AddWithValue("@JobType", model.JobType);
                            }
                            else
                            {
                                command2.Parameters.AddWithValue("@JobType", DBNull.Value);
                            }
                            if (model.JobClass != null)
                            {
                                command2.Parameters.AddWithValue("@JobClass", model.JobClass);
                            }
                            else
                            {
                                command2.Parameters.AddWithValue("@JobClass", DBNull.Value);
                            }
                            if (model.Frequency != null)
                            {
                                command2.Parameters.AddWithValue("@Frequency", model.Frequency);
                            }
                            else
                            {
                                command2.Parameters.AddWithValue("@Frequency", DBNull.Value);
                            }

                            if (model.AccQty != null)
                            {
                                command2.Parameters.AddWithValue("@AccQty", model.AccQty);
                            }
                            else
                            {
                                command2.Parameters.AddWithValue("@AccQty", DBNull.Value);
                            }
                            if (model.PageQty != null)
                            {
                                command2.Parameters.AddWithValue("@PageQty", model.PageQty);
                            }
                            else
                            {
                                command2.Parameters.AddWithValue("@PageQty", DBNull.Value);
                            }
                            if (model.ImpQty != null)
                            {
                                command2.Parameters.AddWithValue("@ImpQty", model.ImpQty);
                            }
                            else
                            {
                                command2.Parameters.AddWithValue("@ImpQty", DBNull.Value);
                            }
                            if (model.TotalAuditTrail != null)
                            {
                                command2.Parameters.AddWithValue("@TotalAuditTrail", model.TotalAuditTrail);
                            }
                            else
                            {
                                command2.Parameters.AddWithValue("@TotalAuditTrail", DBNull.Value);
                            }
                            command2.Parameters.AddWithValue("@JobInstructionId", model.JobInstructionId);
                            command2.Parameters.AddWithValue("@JobAuditTrailId", Id);
                            command2.Parameters.AddWithValue("@ProductionSlipId", model.Id);
                            if (model.AccountsQty != null)
                            {
                                command2.Parameters.AddWithValue("@AccountsQty", model.AccountsQty);
                            }
                            else
                            {
                                command2.Parameters.AddWithValue("@AccountsQty", DBNull.Value);
                            }
                            if (model.PagesQty != null)
                            {
                                command2.Parameters.AddWithValue("@PagesQty", model.PagesQty);
                            }
                            else
                            {
                                command2.Parameters.AddWithValue("@PagesQty", DBNull.Value);
                            }
                            if (model.ImpressionQty != null)
                            {
                                command2.Parameters.AddWithValue("@ImpressionQty", model.ImpressionQty);
                            }
                            else
                            {
                                command2.Parameters.AddWithValue("@ImpressionQty", DBNull.Value);
                            }
                            command2.ExecuteNonQuery();
                            cn2.Close();

                        }

                    }
                    cn3.Close();
                    TempData["msg"] = "<script>alert('SUCCESSFULLY SUBMIT TO POSTING !');</script>";


                    return RedirectToAction("ManageProcessPrint", "PRINT");


                }

            }

        }

        else if (set == "ImportantNotes")
        {

        }
        else if (set == "SampleProduct")
        {



        }

        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {

            cn.Open();
            command.CommandText = @"SELECT Id, PrintSlipNo, Ins_StartDateOn, Ins_StartTime, Ins_EndDateOn, Ins_EndTime,
                                           Ins_Machine,Sort, NonSort, 
                                           NotesByIT,NotesByProduction,NotesByPurchasing, NotesByEngineering, NotesByArtwork , NotesByFinance, NotesByDCP, Recovery/*, Process*/
                                           FROM [IflowSeed].[dbo].[Hist_ProductionSlip]                              
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
                    ViewBag.PrintSlipNo = reader.GetString(1);
                }
                if (reader.IsDBNull(2) == false)
                {
                    ViewBag.Ins_StartDateOn = String.Format("{0:dd/MM/yyyy}", reader.GetDateTime(2));
                }
                if (reader.IsDBNull(3) == false)
                {
                    ViewBag.Ins_StartTime = reader.GetString(3);
                }
                if (reader.IsDBNull(4) == false)
                {
                    ViewBag.Ins_EndDateOn = String.Format("{0:dd/MM/yyyy}", reader.GetDateTime(4));
                }
                if (reader.IsDBNull(5) == false)
                {
                    ViewBag.Ins_EndTime = reader.GetString(5);
                }
                if (reader.IsDBNull(6) == false)
                {
                    ViewBag.Ins_Machine = reader.GetString(6);
                }

                if (reader.IsDBNull(7) == false)
                {
                    bool getSort = reader.GetBoolean(7);
                    if (getSort == false)
                    {
                        ViewBag.Sort = "";
                    }
                    else
                    {
                        ViewBag.Sort = "checked";
                    }
                }
                if (reader.IsDBNull(8) == false)
                {
                    bool getNonSort = reader.GetBoolean(8);
                    if (getNonSort == false)
                    {
                        ViewBag.NonSort = "";
                    }
                    else
                    {
                        ViewBag.NonSort = "checked";
                    }
                }
                if (reader.IsDBNull(9) == false)
                {
                    ViewBag.NotesByIT = reader.GetString(9);
                }
                if (reader.IsDBNull(10) == false)
                {
                    ViewBag.NotesByProduction = reader.GetString(10);
                }
                if (reader.IsDBNull(11) == false)
                {
                    ViewBag.NotesByPurchasing = reader.GetString(11);
                }
                if (reader.IsDBNull(12) == false)
                {
                    ViewBag.NotesByEngineering = reader.GetString(12);
                }
                if (reader.IsDBNull(13) == false)
                {
                    ViewBag.NotesByArtwork = reader.GetString(13);
                }
                if (reader.IsDBNull(14) == false)
                {
                    ViewBag.NotesByFinance = reader.GetString(14);
                }
                if (reader.IsDBNull(15) == false)
                {
                    ViewBag.NotesByDCP = reader.GetString(15);
                }
                if (reader.IsDBNull(16) == false)
                {
                    ViewBag.Recovery = reader.GetString(16);
                }
                //if (reader.IsDBNull(16) == false)
                //{
                //    ViewBag.Process = reader.GetString(16);
                //}


            }
            cn.Close();
        }

        return View();

    }



    List<viewHist_ProductionSlip> viewProductionSlip = new List<viewHist_ProductionSlip>();
    List<Hist_ProductionSlip> viewProdSlip = new List<Hist_ProductionSlip>();


    public ActionResult ViewTransProductionSlip(string Set, string Id, string ProductionSlipId)
    {
        ViewBag.Id = Id;

        if (Set == "back")
        {
            return RedirectToAction("ManageProcessPrint", "PRINT");
        }



        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT b.Id, a.ProductionSlipId
                                           FROM[IflowSeed].[dbo].[ProductionSlip] a, [IflowSeed].[dbo].[Hist_ProductionSlip] b
                                           WHERE a.Id=b.ProductionSlipId AND b.Id=@Id ";
            command.Parameters.AddWithValue("@Id", Id);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                viewHist_ProductionSlip model = new viewHist_ProductionSlip();
                {

                    if (reader.IsDBNull(0) == false)
                    {
                        model.Id = reader.GetGuid(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        model.ProductionSlipId = reader.GetGuid(1);
                    }

                }

                Session["ProductionSlipId"] = model.ProductionSlipId;


            }
            cn.Close();

        }

        using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command1 = new SqlCommand("", cn1))
        {
            int _bil = 1;
            cn1.Open();
            command1.CommandText = @"SELECT Id, Status, ProcessType, Customer_Name, LogTagNo, AccQty,PageQty, ImpQty,
                                           Ins_Machine,Ins_StartDateOn,Ins_StartTime,Ins_EndDateOn,Ins_EndTime
                                           FROM [IflowSeed].[dbo].[Hist_ProductionSlip]                                    
                                           WHERE ProductionSlipId LIKE @ProductionSlipId  AND 
                                           (Status = 'INSERTING') OR (Status = 'MMP') OR (Status ='PRINT,INSERT AND RETURN')
                                           OR (Status = 'SELF MAILER') OR (Status = 'READY TO INSERT') 
                                           ORDER BY ProcessType";
            command1.Parameters.AddWithValue("@ProductionSlipId", "%" + Session["ProductionSlipId"] + "%");
            var reader1 = command1.ExecuteReader();
            while (reader1.Read())
            {
                Hist_ProductionSlip model = new Hist_ProductionSlip();
                {

                    model.Bil = _bil++;
                    if (reader1.IsDBNull(0) == false)
                    {
                        model.Id = reader1.GetGuid(0);
                    }
                    if (reader1.IsDBNull(1) == false)
                    {
                        model.Status = reader1.GetString(1);
                    }
                    if (reader1.IsDBNull(2) == false)
                    {
                        model.ProcessType = reader1.GetString(2);
                    }
                    if (reader1.IsDBNull(3) == false)
                    {
                        model.Customer_Name = reader1.GetString(3);
                    }
                    if (reader1.IsDBNull(4) == false)
                    {
                        model.LogTagNo = reader1.GetString(4);
                    }
                    if (reader1.IsDBNull(5) == false)
                    {
                        model.AccQty = reader1.GetString(5);
                    }
                    if (reader1.IsDBNull(6) == false)
                    {
                        model.PageQty = reader1.GetString(6);
                    }
                    if (reader1.IsDBNull(7) == false)
                    {
                        model.ImpQty = reader1.GetString(7);
                    }
                    if (reader1.IsDBNull(8) == false)
                    {
                        model.Ins_Machine = reader1.GetString(8);
                    }
                    if (reader1.IsDBNull(9) == false)
                    {
                        model.Ins_StartDateOn = reader1.GetDateTime(9);
                    }
                    if (reader1.IsDBNull(10) == false)
                    {
                        model.Ins_StartTime = reader1.GetString(10);
                    }
                    if (reader1.IsDBNull(11) == false)
                    {
                        model.Ins_EndDateOn = reader1.GetDateTime(11);
                    }
                    if (reader1.IsDBNull(12) == false)
                    {
                        model.Ins_EndTime = reader1.GetString(12);
                    }

                }

                viewProdSlip.Add(model);
            }
            cn1.Close();
        }




        return View(viewProdSlip); //hntr data ke ui
    }

}


