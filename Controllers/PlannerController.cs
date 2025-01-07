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
public class PlannerController : Controller
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
                    command1 = new SqlCommand("UPDATE [JobAuditTrailDetail] SET STATUS=@StatusConcat WHERE JobSheetNo=@Id", cn1);
                    command1.Parameters.AddWithValue("@Id", idAsString);
                    command1.Parameters.AddWithValue("@StatusConcat", StatusConcat1);
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
            command1 = new SqlCommand("UPDATE [JobAuditTrailDetail]SET STATUS=@StatusConcat WHERE JobSheetNo=@Id", cn1);
            command1.Parameters.AddWithValue("@Id", Id);
            command1.Parameters.AddWithValue("@StatusConcat", StatusConcat);
            command1.ExecuteNonQuery();
            cn1.Close();
        }

        return RedirectToAction("ManagePrint", "Printing", new { Id = Session["Id"].ToString() });
    }


    public ActionResult ManagePlanner(string product, string set, string pageNumber)
    {

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
                //                            EngineeringNotes ,ArtworkNotes,Acc_BillingNotes
                //                         FROM [JobInstruction]                                    
                //                         WHERE ProductName LIKE @ProductName
                //                         AND Status = 'PLANNER'
                //                         ORDER BY CreatedOn DESC ";

                //command.CommandText = @"SELECT Customer_Name, ProductName, JobClass,
                //JobType, JobSheetNo, SUM(CAST(AccQty AS INT)) as AccQty, SUM(CAST(ImpQty AS INT)) as ImpQty, SUM(CAST(PageQty AS INT)) as PageQty,PrintSlip,LogTagNo,MAX(FORMAT(CONVERT(date, RevStrtDateOn), 'yyyy/MM/dd')) as RevStrtDateOn, MAX(RevStrtTime), MAX(FORMAT(CONVERT(date, ProcessDate), 'yyyy/MM/dd')) as ProcessDate,MAX(TimeProcessIt)
                //                        FROM[JobAuditTrailDetail]
                //                        WHERE Status = 'PLANNER' AND (LogTagNo LIKE @ProductName OR ProductName LIKE @ProductName)
                //                        GROUP BY Customer_Name, ProductName, JobClass,JobType,JobSheetNo,PrintSlip,LogTagNo ORDER BY MAX(TimeProcessIt) ASC";

                command.CommandText = @"SELECT JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.ProductName, JobAuditTrailDetail.JobClass,
                JobAuditTrailDetail.JobType, JobAuditTrailDetail.JobSheetNo, SUM(CAST(JobAuditTrailDetail.AccQty AS INT)) as AccQty, SUM(CAST(JobAuditTrailDetail.ImpQty AS INT)) as ImpQty, SUM(CAST(JobAuditTrailDetail.PageQty AS INT)) as PageQty,JobAuditTrailDetail.PrintSlip,JobAuditTrailDetail.LogTagNo,MAX(FORMAT(CONVERT(date, JobAuditTrailDetail.RevStrtDateOn), 'yyyy/MM/dd')) as RevStrtDateOn, 
                MAX(JobAuditTrailDetail.RevStrtTime), MAX(FORMAT(CONVERT(date, JobAuditTrailDetail.ProcessDate), 'yyyy/MM/dd')) as ProcessDate,MAX(JobAuditTrailDetail.TimeProcessIt), JobInstruction.NMRStatus
                                        FROM [JobAuditTrailDetail] FULL JOIN JobInstruction ON JobAuditTrailDetail.JobSheetNo = JobInstruction.JobSheetNo
                                        WHERE JobAuditTrailDetail.Status = 'PLANNER'  AND (JobAuditTrailDetail.LogTagNo LIKE @ProductName OR JobAuditTrailDetail.ProductName LIKE @ProductName)
                                        GROUP BY JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.ProductName, JobAuditTrailDetail.JobClass,JobAuditTrailDetail.JobType,JobAuditTrailDetail.JobSheetNo,JobAuditTrailDetail.PrintSlip,JobAuditTrailDetail.LogTagNo, JobInstruction.NMRStatus,JobAuditTrailDetail.ProcessDate ORDER BY JobAuditTrailDetail.ProcessDate ASC,MAX(JobAuditTrailDetail.TimeProcessIt) ASC, JobInstruction.NMRStatus DESC";


                //command.CommandText = @"SELECT Customer_Name, ProductName, JobClass,
                //JobType, JobSheetNo, SUM(CAST(AccQty AS INT)) as AccQty, SUM(CAST(ImpQty AS INT)) as ImpQty, SUM(CAST(PageQty AS INT)) as PageQty,PrintSlip,LogTagNo,MAX(FORMAT(CONVERT(date, RevStrtDateOn), 'yyyy/MM/dd')) as RevStrtDateOn, MAX(RevStrtTime), MAX(FORMAT(CONVERT(date, ProcessDate), 'yyyy/MM/dd')) as ProcessDate,MAX(TimeProcessIt)
                //                        FROM[JobAuditTrailDetail]
                //                        WHERE Status = 'PLANNER' AND (LogTagNo LIKE @ProductName OR ProductName LIKE @ProductName)
                //                        GROUP BY Customer_Name, ProductName, JobClass,JobType,JobSheetNo,PrintSlip,LogTagNo ORDER BY ProcessDate ASC,MAX(TimeProcessIt) ASC";

                command.Parameters.AddWithValue("@ProductName", "%" + product + "%");
            }
            else
            {
                //command.CommandText = @"SELECT Customer_Name, ProductName, JobClass,
                //JobType, JobSheetNo, SUM(CAST(AccQty AS INT)) as AccQty, SUM(CAST(ImpQty AS INT)) as ImpQty, SUM(CAST(PageQty AS INT)) as PageQty,PrintSlip,LogTagNo,MAX(FORMAT(CONVERT(date, RevStrtDateOn), 'yyyy/MM/dd')) as RevStrtDateOn, MAX(RevStrtTime), MAX(FORMAT(CONVERT(date, ProcessDate), 'yyyy/MM/dd')) as ProcessDate,MAX(TimeProcessIt)
                //                        FROM[JobAuditTrailDetail]
                //                        WHERE Status = 'PLANNER' 
                //                        GROUP BY Customer_Name, ProductName, JobClass,JobType,JobSheetNo,PrintSlip,LogTagNo ORDER BY MAX(TimeProcessIt) ASC";

                command.CommandText = @"SELECT JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.ProductName, JobAuditTrailDetail.JobClass,
                JobAuditTrailDetail.JobType, JobAuditTrailDetail.JobSheetNo, SUM(CAST(JobAuditTrailDetail.AccQty AS INT)) as AccQty, SUM(CAST(JobAuditTrailDetail.ImpQty AS INT)) as ImpQty, SUM(CAST(JobAuditTrailDetail.PageQty AS INT)) as PageQty,JobAuditTrailDetail.PrintSlip,JobAuditTrailDetail.LogTagNo,MAX(FORMAT(CONVERT(date, JobAuditTrailDetail.RevStrtDateOn), 'yyyy/MM/dd')) as RevStrtDateOn, 
                MAX(JobAuditTrailDetail.RevStrtTime), MAX(FORMAT(CONVERT(date, JobAuditTrailDetail.ProcessDate), 'yyyy/MM/dd')) as ProcessDate,MAX(JobAuditTrailDetail.TimeProcessIt), JobInstruction.NMRStatus
                                        FROM [JobAuditTrailDetail] FULL JOIN JobInstruction ON JobAuditTrailDetail.JobSheetNo = JobInstruction.JobSheetNo
                                        WHERE JobAuditTrailDetail.Status = 'PLANNER' 
                                        GROUP BY JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.ProductName, JobAuditTrailDetail.JobClass,JobAuditTrailDetail.JobType,JobAuditTrailDetail.JobSheetNo,JobAuditTrailDetail.PrintSlip,JobAuditTrailDetail.LogTagNo, JobInstruction.NMRStatus,JobAuditTrailDetail.ProcessDate ORDER BY JobAuditTrailDetail.ProcessDate ASC,MAX(JobAuditTrailDetail.TimeProcessIt) ASC, JobInstruction.NMRStatus DESC";

                //command.CommandText = @"SELECT Customer_Name, ProductName, JobClass,
                //                            JobType,JobSheetNo, SUM(CAST(AccQty AS INT)) as AccQty, SUM(CAST(ImpQty AS INT)) as ImpQty, SUM(CAST(PageQty AS INT)) as PageQty,PrintSlip,LogTagNo,FORMAT(CONVERT(date, RevStrtDateOn), 'yyyy/MM/dd') as RevStrtDateOn, RevStrtTime, FORMAT(CONVERT(date, ProcessDate), 'yyyy/MM/dd') as ProcessDate,MAX(TimeProcessIt)
                //                        FROM [JobAuditTrailDetail]
                //                        WHERE Status = 'PLANNER'
                //                        GROUP BY Customer_Name, ProductName, JobClass,JobType,JobSheetNo,PrintSlip,LogTagNo,RevStrtDateOn,RevStrtTime,ProcessDate
                //                        ORDER BY (SELECT NULL)
                //                        OFFSET 0 ROWS
                //                        FETCH NEXT 100 ROWS ONLY";

            }

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    model.Bil = _bil++;
                    //if (reader.IsDBNull(0) == false)
                    //{
                    //    model.Id = reader.GetGuid(0);
                    //}
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
                        model.PrintSlip = reader.GetString(8);
                    }
                    if (!reader.IsDBNull(9))
                    {
                        model.LogTagNo = reader.GetString(9);
                    }
                    if (!reader.IsDBNull(10))
                    {
                        model.CollectedDate = reader["RevStrtDateOn"].ToString();
                    }
                    else
                    {
                        model.CollectedDate = "";
                    }
                    if (!reader.IsDBNull(11))
                    {
                        model.CollectedTime = reader.GetString(11);
                    }
                    else
                    {
                        model.CollectedTime = "";

                    }
                    if (!reader.IsDBNull(12))
                    {
                        model.ItSubmitDateOn = reader["ProcessDate"].ToString();
                    }
                    else
                    {
                        model.ItSubmitDateOn = "";

                    }
                    if (!reader.IsDBNull(13))
                    {
                        model.ItSubmitTimeOn = reader.GetString(13);
                    }
                    else
                    {
                        model.ItSubmitTimeOn = "";

                    }
                    //repurposed confrm100 as placeholder for NMR Status, to save time
                    if (!reader.IsDBNull(14))
                    {
                        if(reader.GetString(14)=="COMPLETED")
                        {
                            model.Confrm100 = "NO";

                        }
                        else
                        {
                            model.Confrm100 = "YES";

                        }
                    }
                    else
                    {
                        model.Confrm100 = "YES";

                    }
                    model.CombineCollected = model.CollectedDate + " " + model.CollectedTime;
                    model.CombineItSubmit = model.ItSubmitDateOn + " " + model.ItSubmitTimeOn;

                }
                JobInstructionlist1.Add(model);

            }
            cn.Close();
        }

        return View(JobInstructionlist1);
    }




    //List<JobAuditTrail> viewJobAuditTrail = new List<JobAuditTrail>();

    //public ActionResult ManagePreSchedule(string Id, string set, string LogTagNo, string AccountsQty, string ImpressionQty, string PagesQty,
    //                                      string JobRequest, string Customer_Name, string ProductName, string ProgramId,
    //                                      string FileId, string JobId, string JobNameIT, string RevStrtDateOn, string RevStrtTime,
    //                                      string DateProcessItOn, string TimeProcessIt, string FirstRecord, string LastRecord,
    //                                      string JobClass, string JobType, string Frequency, string PlanDatePostOn, string ItSubmitOn,
    //                                      string JobInstructionId, string JobAuditTrailId, string StartProductionDateOn,
    //                                      string Machine, string MachineInsert, string PlanShift, string PlanReturn_CourierOn, string CreateByPlanner,
    //                                      string NotesByIT, string NotesByProduction, string NotesByPurchasing, string NotesByEngineering, string NotesByArtwork, string NotesByFinance, string NotesByDCP)

    //{


    //    if (set == "search") //ini kalu user search product
    //    {
    //        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
    //        using (SqlCommand command = new SqlCommand("", cn))
    //        {
    //            int _bil = 1;
    //            cn.Open();
    //            command.CommandText = @"SELECT Id, ModifiedOn, ItSubmitOn, Customer_Name, ProductName, LogTagNo, JobType, JobClass,
    //                                   Frequency, AccountsQty,PagesQty,ImpressionQty,JobSheetNo,TotalAuditTrail,FileId,
    //                                   StartProductionDateOn,Machine,MachineInsert,PlanShift,PlanDatePostOn,PlanReturn_CourierOn,
    //                                   NotesByIT,NotesByProduction,NotesByPurchasing,NotesByEngineering,NotesByArtwork,NotesByFinance,NotesByDCP,
    //                                   CreateByPlanner,Status,ProgramId,JobId,JobNameIT,RevStrtDateOn,RevStrtTime,DateProcessItOn,TimeProcessIt,
    //                                   FirstRecord,LastRecord,JobInstructionId,JobAuditTrailId
    //                                   FROM [[JobAuditTrailDetail]] 
    //                                   WHERE (Status = 'PLANNER')
    //                                   AND ProductName LIKE @ProductName";
    //            command.Parameters.AddWithValue("@ProductName", "%" + ProductName + "%"); var reader = command.ExecuteReader();
    //            while (reader.Read())
    //            {
    //                JobAuditTrail model = new JobAuditTrail();
    //                {
    //                    model.Bil = _bil++;
    //                    if (reader.IsDBNull(0) == false)
    //                    {
    //                        model.Id = reader.GetGuid(0);
    //                    }
    //                    if (reader.IsDBNull(1) == false)
    //                    {
    //                        model.ModifiedOn = reader.GetDateTime(1);
    //                    }
    //                    if (reader.IsDBNull(2) == false)
    //                    {
    //                        model.ItSubmitOn = reader.GetDateTime(2);
    //                    }
    //                    if (reader.IsDBNull(3) == false)
    //                    {
    //                        model.Customer_Name = reader.GetString(3);
    //                    }
    //                    if (reader.IsDBNull(4) == false)
    //                    {
    //                        model.ProductName = reader.GetString(4);
    //                    }
    //                    if (reader.IsDBNull(5) == false)
    //                    {
    //                        model.LogTagNo = reader.GetString(5);
    //                    }
    //                    if (reader.IsDBNull(6) == false)
    //                    {
    //                        model.JobType = reader.GetString(6);
    //                    }
    //                    if (reader.IsDBNull(7) == false)
    //                    {
    //                        model.JobClass = reader.GetString(7);
    //                    }
    //                    if (reader.IsDBNull(8) == false)
    //                    {
    //                        model.Frequency = reader.GetString(8);
    //                    }
    //                    if (reader.IsDBNull(9) == false)
    //                    {
    //                        model.AccountsQty = reader.GetString(9);
    //                    }
    //                    if (reader.IsDBNull(10) == false)
    //                    {
    //                        model.PagesQty = reader.GetString(10);
    //                    }
    //                    if (reader.IsDBNull(11) == false)
    //                    {
    //                        model.ImpressionQty = reader.GetString(11);
    //                    }
    //                    if (reader.IsDBNull(12) == false)
    //                    {
    //                        model.JobSheetNo = reader.GetString(12);
    //                    }
    //                    if (reader.IsDBNull(13) == false)
    //                    {
    //                        model.TotalAuditTrail = reader.GetString(13);
    //                    }
    //                    if (reader.IsDBNull(14) == false)
    //                    {
    //                        model.FileId = reader.GetString(14);
    //                    }
    //                    if (reader.IsDBNull(15) == false)
    //                    {
    //                        model.StartProductionDateOn = reader.GetDateTime(15);
    //                    }
    //                    if (reader.IsDBNull(16) == false)
    //                    {
    //                        model.Machine = reader.GetString(16);
    //                    }
    //                    if (reader.IsDBNull(17) == false)
    //                    {
    //                        model.MachineInsert = reader.GetString(17);
    //                    }
    //                    if (reader.IsDBNull(18) == false)
    //                    {
    //                        model.PlanShift = reader.GetString(18);
    //                    }
    //                    if (reader.IsDBNull(19) == false)
    //                    {
    //                        model.PlanDatePostOn = reader.GetDateTime(19);
    //                    }
    //                    if (reader.IsDBNull(20) == false)
    //                    {
    //                        model.PlanReturn_CourierOn = reader.GetDateTime(20);
    //                    }
    //                    if (reader.IsDBNull(21) == false)
    //                    {
    //                        model.NotesByIT = reader.GetString(21);
    //                    }
    //                    if (reader.IsDBNull(22) == false)
    //                    {
    //                        model.NotesByProduction = reader.GetString(22);
    //                    }
    //                    if (reader.IsDBNull(23) == false)
    //                    {
    //                        model.NotesByPurchasing = reader.GetString(23);
    //                    }
    //                    if (reader.IsDBNull(24) == false)
    //                    {
    //                        model.NotesByEngineering = reader.GetString(24);
    //                    }
    //                    if (reader.IsDBNull(25) == false)
    //                    {
    //                        model.NotesByArtwork = reader.GetString(25);
    //                    }
    //                    if (reader.IsDBNull(26) == false)
    //                    {
    //                        model.NotesByFinance = reader.GetString(26);
    //                    }
    //                    if (reader.IsDBNull(27) == false)
    //                    {
    //                        model.NotesByDCP = reader.GetString(27);
    //                    }
    //                    if (reader.IsDBNull(28) == false)
    //                    {
    //                        model.CreateByPlanner = reader.GetString(28);
    //                    }
    //                    if (reader.IsDBNull(29) == false)
    //                    {
    //                        model.Status = reader.GetString(29);
    //                    }
    //                    if (reader.IsDBNull(30) == false)
    //                    {
    //                        model.ProgramId = reader.GetString(30);
    //                    }
    //                    if (reader.IsDBNull(31) == false)
    //                    {
    //                        model.JobId = reader.GetString(31);
    //                    }
    //                    if (reader.IsDBNull(32) == false)
    //                    {
    //                        model.JobNameIT = reader.GetString(32);
    //                    }
    //                    if (reader.IsDBNull(33) == false)
    //                    {
    //                        model.RevStrtDateOn = reader.GetDateTime(33);
    //                    }
    //                    if (reader.IsDBNull(34) == false)
    //                    {
    //                        model.RevStrtTime = reader.GetString(34);
    //                    }
    //                    if (reader.IsDBNull(35) == false)
    //                    {
    //                        model.DateProcessItOn = reader.GetDateTime(35);
    //                    }
    //                    if (reader.IsDBNull(36) == false)
    //                    {
    //                        model.TimeProcessIt = reader.GetString(36);
    //                    }
    //                    if (reader.IsDBNull(37) == false)
    //                    {
    //                        model.FirstRecord = reader.GetString(37);
    //                    }
    //                    if (reader.IsDBNull(38) == false)
    //                    {
    //                        model.LastRecord = reader.GetString(38);
    //                    }
    //                    if (reader.IsDBNull(39) == false)
    //                    {
    //                        model.JobInstructionId = reader.GetGuid(39);
    //                    }
    //                    if (reader.IsDBNull(40) == false)
    //                    {
    //                        model.JobAuditTrailId = reader.GetGuid(40);
    //                    }

    //                }
    //                viewJobAuditTrail.Add(model);
    //            }
    //            cn.Close();
    //        }
    //    }
    //    else
    //    {
    //        //ALL firt masuk
    //        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
    //        using (SqlCommand command = new SqlCommand("", cn))
    //        {
    //            int _bil = 1;
    //            cn.Open();
    //            command.CommandText = @"SELECT Id, ModifiedOn, ItSubmitOn, Customer_Name, ProductName, LogTagNo, JobType, JobClass,
    //                                   Frequency, AccountsQty,PagesQty,ImpressionQty,JobSheetNo,TotalAuditTrail,FileId,
    //                                   StartProductionDateOn,Machine,MachineInsert,PlanShift,PlanDatePostOn,PlanReturn_CourierOn,
    //                                   NotesByIT,NotesByProduction,NotesByPurchasing,NotesByEngineering,NotesByArtwork,NotesByFinance,NotesByDCP,
    //                                   CreateByPlanner,Status,ProgramId,JobId,JobNameIT,RevStrtDateOn,RevStrtTime,DateProcessItOn,TimeProcessIt,
    //                                   FirstRecord,LastRecord,JobInstructionId,JobAuditTrailId
    //                                   FROM [JobAuditTrailDetail]
    //                                   WHERE (Status = 'PLANNER')";
    //            var reader = command.ExecuteReader();
    //            while (reader.Read())
    //            {
    //                JobAuditTrail model = new JobAuditTrail();
    //                {
    //                    model.Bil = _bil++;
    //                    if (reader.IsDBNull(0) == false)
    //                    {
    //                        model.Id = reader.GetGuid(0);
    //                    }
    //                    if (reader.IsDBNull(1) == false)
    //                    {
    //                        model.ModifiedOn = reader.GetDateTime(1);
    //                    }
    //                    if (reader.IsDBNull(2) == false)
    //                    {
    //                        model.ItSubmitOn = reader.GetDateTime(2);
    //                    }
    //                    if (reader.IsDBNull(3) == false)
    //                    {
    //                        model.Customer_Name = reader.GetString(3);
    //                    }
    //                    if (reader.IsDBNull(4) == false)
    //                    {
    //                        model.ProductName = reader.GetString(4);
    //                    }
    //                    if (reader.IsDBNull(5) == false)
    //                    {
    //                        model.LogTagNo = reader.GetString(5);
    //                    }
    //                    if (reader.IsDBNull(6) == false)
    //                    {
    //                        model.JobType = reader.GetString(6);
    //                    }
    //                    if (reader.IsDBNull(7) == false)
    //                    {
    //                        model.JobClass = reader.GetString(7);
    //                    }
    //                    if (reader.IsDBNull(8) == false)
    //                    {
    //                        model.Frequency = reader.GetString(8);
    //                    }
    //                    if (reader.IsDBNull(9) == false)
    //                    {
    //                        model.AccountsQty = reader.GetString(9);
    //                    }
    //                    if (reader.IsDBNull(10) == false)
    //                    {
    //                        model.PagesQty = reader.GetString(10);
    //                    }
    //                    if (reader.IsDBNull(11) == false)
    //                    {
    //                        model.ImpressionQty = reader.GetString(11);
    //                    }
    //                    if (reader.IsDBNull(12) == false)
    //                    {
    //                        model.JobSheetNo = reader.GetString(12);
    //                    }
    //                    if (reader.IsDBNull(13) == false)
    //                    {
    //                        model.TotalAuditTrail = reader.GetString(13);
    //                    }
    //                    if (reader.IsDBNull(14) == false)
    //                    {
    //                        model.FileId = reader.GetString(14);
    //                    }
    //                    if (reader.IsDBNull(15) == false)
    //                    {
    //                        model.StartProductionDateOn = reader.GetDateTime(15);
    //                    }
    //                    if (reader.IsDBNull(16) == false)
    //                    {
    //                        model.Machine = reader.GetString(16);
    //                    }
    //                    if (reader.IsDBNull(17) == false)
    //                    {
    //                        model.MachineInsert = reader.GetString(17);
    //                    }
    //                    if (reader.IsDBNull(18) == false)
    //                    {
    //                        model.PlanShift = reader.GetString(18);
    //                    }
    //                    if (reader.IsDBNull(19) == false)
    //                    {
    //                        model.PlanDatePostOn = reader.GetDateTime(19);
    //                    }
    //                    if (reader.IsDBNull(20) == false)
    //                    {
    //                        model.PlanReturn_CourierOn = reader.GetDateTime(20);
    //                    }
    //                    if (reader.IsDBNull(21) == false)
    //                    {
    //                        model.NotesByIT = reader.GetString(21);
    //                    }
    //                    if (reader.IsDBNull(22) == false)
    //                    {
    //                        model.NotesByProduction = reader.GetString(22);
    //                    }
    //                    if (reader.IsDBNull(23) == false)
    //                    {
    //                        model.NotesByPurchasing = reader.GetString(23);
    //                    }
    //                    if (reader.IsDBNull(24) == false)
    //                    {
    //                        model.NotesByEngineering = reader.GetString(24);
    //                    }
    //                    if (reader.IsDBNull(25) == false)
    //                    {
    //                        model.NotesByArtwork = reader.GetString(25);
    //                    }
    //                    if (reader.IsDBNull(26) == false)
    //                    {
    //                        model.NotesByFinance = reader.GetString(26);
    //                    }
    //                    if (reader.IsDBNull(27) == false)
    //                    {
    //                        model.NotesByDCP = reader.GetString(27);
    //                    }
    //                    if (reader.IsDBNull(28) == false)
    //                    {
    //                        model.CreateByPlanner = reader.GetString(28);
    //                    }
    //                    if (reader.IsDBNull(29) == false)
    //                    {
    //                        model.Status = reader.GetString(29);
    //                    }
    //                    if (reader.IsDBNull(30) == false)
    //                    {
    //                        model.ProgramId = reader.GetString(30);
    //                    }
    //                    if (reader.IsDBNull(31) == false)
    //                    {
    //                        model.JobId = reader.GetString(31);
    //                    }
    //                    if (reader.IsDBNull(32) == false)
    //                    {
    //                        model.JobNameIT = reader.GetString(32);
    //                    }
    //                    if (reader.IsDBNull(33) == false)
    //                    {
    //                        model.RevStrtDateOn = reader.GetDateTime(33);
    //                    }
    //                    if (reader.IsDBNull(34) == false)
    //                    {
    //                        model.RevStrtTime = reader.GetString(34);
    //                    }
    //                    if (reader.IsDBNull(35) == false)
    //                    {
    //                        model.DateProcessItOn = reader.GetDateTime(35);
    //                    }
    //                    if (reader.IsDBNull(36) == false)
    //                    {
    //                        model.TimeProcessIt = reader.GetString(36);
    //                    }
    //                    if (reader.IsDBNull(37) == false)
    //                    {
    //                        model.FirstRecord = reader.GetString(37);
    //                    }
    //                    if (reader.IsDBNull(38) == false)
    //                    {
    //                        model.LastRecord = reader.GetString(38);
    //                    }
    //                    if (reader.IsDBNull(39) == false)
    //                    {
    //                        model.JobInstructionId = reader.GetGuid(39);
    //                    }
    //                    if (reader.IsDBNull(40) == false)
    //                    {
    //                        model.JobAuditTrailId = reader.GetGuid(40);
    //                    }
    //                }
    //                viewJobAuditTrail.Add(model);
    //            }
    //            cn.Close();
    //        }


    //    }



    //    return View(viewJobAuditTrail); //hntr data ke ui
    //}

    public ActionResult ViewPlanSchedule(JobAuditTrail JobAuditTrail, JobAuditTrail get, JobAuditTrailDetail JobAuditTrailDetail, string set, string Id, string JobRequest, string Customer_Name, string ProductName, string LogTagNo,
                                           string JobType, string JobClass, string Frequency, string AccountsQty, string PagesQty, string ImpressionQty,
                                           string JobSheetNo, string TotalAuditTrail, string FileId, string ReadyBySTORE, string ReadyByENGINEER, string ReadyByGRAPHIC,
                                           string StartProductionDateOn, string Machine, string MachineInsert, string PlanShift, string PlanDatePostOn, string PlanReturn_CourierOn, string CreateByPlanner, string Status,
                                           string NotesByIT, string NotesByProduction, string NotesByPurchasing, string NotesByEngineering, string NotesByArtwork, string NotesByFinance, string NotesByDCP,
                                           string JobInstructionId, string JobAuditTrailId, string ItSubmitOn, string AccQty, string ImpQty, string PageQty, string MachineType)

    {

        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.AccountManager = IdentityName.ToString();
        ViewBag.Customer_Name = Customer_Name;
        ViewBag.ProductName = ProductName;
        ViewBag.JobRequest = JobRequest;
        ViewBag.LogTagNo = LogTagNo;
        ViewBag.JobType = JobType;
        ViewBag.JobClass = JobClass;
        ViewBag.Frequency = Frequency;
        ViewBag.AccountsQty = AccountsQty;
        ViewBag.PagesQty = PagesQty;
        ViewBag.ImpressionQty = ImpressionQty;
        ViewBag.TotalAuditTrail = TotalAuditTrail;
        ViewBag.Id = Id;

        List<JobInstruction> JobInstructionlist1 = new List<JobInstruction>(); ;
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT JobInstruction.Id, JobInstruction.Customer_Name, JobInstruction.ProductName, JobInstruction.JobClass, JobInstruction.JobType, JobInstruction.JobSheetNo, JobInstruction.StartDevDate, JobInstruction.EndDevDate, JobInstruction.JobRequest, JobInstruction.AccountsQty, JobInstruction.ImpressionQty, JobInstruction.PagesQty, JobAuditTrailDetail.AccQty, JobAuditTrailDetail.ImpQty, JobAuditTrailDetail.PageQty,JobAuditTrailDetail.CreatedOn
                                        FROM  JobInstruction INNER JOIN
                                        JobAuditTrailDetail ON JobInstruction.Id = JobAuditTrailDetail.JobAuditTrailId
                                        WHERE JobInstruction.Id =@Id";

            command.Parameters.AddWithValue("@Id", Id);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    model.Bil = _bil++;
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
                        ViewBag.JobRequest = String.Format("{0:dd/MM/yyyy }", (DateTime)reader.GetDateTime(8));
                    }
                    if (reader.IsDBNull(9) == false)
                    {
                        ViewBag.AccountsQty = reader.GetString(9);
                    }
                    if (reader.IsDBNull(10) == false)
                    {
                        ViewBag.ImpressionQty = reader.GetString(10);
                    }
                    if (reader.IsDBNull(11) == false)
                    {
                        ViewBag.PagesQty = reader.GetString(11);
                    }
                    if (reader.IsDBNull(12) == false)
                    {
                        ViewBag.AccQty = reader.GetString(12);
                    }
                    if (reader.IsDBNull(13) == false)
                    {
                        ViewBag.ImpQty = reader.GetString(13);
                    }
                    if (reader.IsDBNull(14) == false)
                    {
                        ViewBag.PageQty = reader.GetString(14);
                    }
                    if (reader.IsDBNull(15) == false)
                    {
                        ViewBag.CreatedOn = String.Format("{0:dd/MM/yyyy }", (DateTime)reader.GetDateTime(15));
                    }
                }
                JobInstructionlist1.Add(model);
            }
            cn.Close();



        }

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT StartProductionDateOn,PlanDatePostOn,PlanReturn_CourierOn,
                                        MachineInsert,Machine,PlanShift
                                        FROM  Planner 
                                        WHERE Planner.Id =@Id";
                command.Parameters.AddWithValue("@Id", Id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    

                        if (reader.IsDBNull(0) == false)
                        {
                            ViewBag.StartProductionDateOn = String.Format("{0:dd/MM/yyyy}", reader.GetDateTime(0));
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            //model.PlanDatePostOn = String.Format("{0:dd/MM/yyyy}", reader.GetDateTime(1));
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            //model.PlanReturn_CourierOn = String.Format("{0:dd/MM/yyyy}", reader.GetDateTime(2));
                        }
                        if (reader.IsDBNull(3) == false)
                        {
                            ViewBag.MachineInsert = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            ViewBag.Machine = reader.GetString(3);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            //model.PlanShift = reader.GetString(5);
                        }
                    
            }
            cn.Close();
            }

        return View();
        //return RedirectToAction("CreatePlanSchedule", "Planner");
    }

    [ValidateInput(false)]
    public ActionResult CreatePlanSchedule(JobAuditTrail JobAuditTrail, JobAuditTrail get, JobAuditTrailDetail JobAuditTrailDetail, string set, string Id, string JobRequest, string Customer_Name, string ProductName, string LogTagNo,
                                           string JobType, string JobClass, string Frequency, string AccountsQty, string PagesQty, string ImpressionQty,
                                           string JobSheetNo, string TotalAuditTrail, string FileId, string ReadyBySTORE, string ReadyByENGINEER, string ReadyByGRAPHIC,
                                           string StartProductionDateOn, string Machine, string MachineInsert, string PlanShift, string PlanDatePostOn, string PlanReturn_CourierOn, string CreateByPlanner, string Status,
                                           string NotesByIT, string NotesByProduction, string NotesByPurchasing, string NotesByEngineering, string NotesByArtwork, string NotesByFinance, string NotesByDCP,
                                           string JobInstructionId, string JobAuditTrailId, string ItSubmitOn, string AccQty, string ImpQty, string PageQty, string MachineType,string NMRStatus)

    {

        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.AccountManager = IdentityName.ToString();
        ViewBag.Customer_Name = Customer_Name;
        ViewBag.ProductName = ProductName;
        ViewBag.JobRequest = JobRequest;
        ViewBag.LogTagNo = LogTagNo;
        ViewBag.JobType = JobType;
        ViewBag.JobClass = JobClass;
        ViewBag.Frequency = Frequency;
        ViewBag.AccountsQty = AccountsQty;
        ViewBag.PagesQty = PagesQty;
        ViewBag.ImpressionQty = ImpressionQty;
        ViewBag.TotalAuditTrail = TotalAuditTrail;
        ViewBag.Id = Id;
        ViewBag.JobSheetNo = JobSheetNo;




        List<SelectListItem> List3 = new List<SelectListItem>();

        List3.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        List3.Add(new SelectListItem { Text = "INSERTING", Value = "INSERTING" });
        List3.Add(new SelectListItem { Text = "SELFMAILER", Value = "SELFMAILER" });
        List3.Add(new SelectListItem { Text = "MMP", Value = "MMP" });
        //List3.Add(new SelectListItem { Text = "MS", Value = "MS" });

        ViewData["MachineInsert_"] = List3;



        List<SelectListItem> List2 = new List<SelectListItem>();

        List2.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        List2.Add(new SelectListItem { Text = "CUT SHEET", Value = "CUT SHEET" });
        List2.Add(new SelectListItem { Text = "COMPUTER FORM", Value = "COMPUTER FORM" });
        List2.Add(new SelectListItem { Text = "DCP", Value = "DCP" });

        ViewData["Machine_"] = List2;


        List<SelectListItem> list4 = new List<SelectListItem>();

        list4.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        list4.Add(new SelectListItem { Text = "Morning", Value = "Morning" });
        list4.Add(new SelectListItem { Text = "Night", Value = "Night" });
        list4.Add(new SelectListItem { Text = "Both", Value = "Both" });

        ViewData["PlanShift_"] = list4;


        if (!string.IsNullOrEmpty(LogTagNo))
        {

            List<JobInstruction> JobInstructionlist1 = new List<JobInstruction>(); ;
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.ProductName, JobAuditTrailDetail.JobClass, JobAuditTrailDetail.JobType, JobAuditTrailDetail.JobSheetNo, SUM(CAST(JobAuditTrailDetail.AccQty AS INT)) as AccQty, SUM(CAST(JobAuditTrailDetail.ImpQty AS INT)) as ImpQty, SUM(CAST(JobAuditTrailDetail.PageQty AS INT)) as PageQty,MAX(JobAuditTrailDetail.CreatedOn), FORMAT(CONVERT(date, Planner.StartProductionDateOn), 'yyyy-MM-dd') as StartProductionDateOn, Planner.Machine, Planner.MachineInsert, Planner.PlanShift, FORMAT(CONVERT(date, Planner.PlanDatePostOn), 'yyyy-MM-dd') as PlanDatePostOn , FORMAT(CONVERT(date, Planner.PlanReturn_CourierOn), 'yyyy-MM-dd') as PlanReturn_CourierOn,
                                        JobInstruction.IT_SysNotes, JobInstruction.Produc_PlanningNotes,JobInstruction.PurchasingNotes,JobInstruction.EngineeringNotes,JobInstruction.ArtworkNotes,JobInstruction.Acc_BillingNotes, MAX(JobAuditTrailDetail.Cust_Department) 
                                        FROM JobAuditTrailDetail FULL JOIN Planner ON JobAuditTrailDetail.LogTagNo=Planner.LogTagNo FULL JOIN JobInstruction ON JobAuditTrailDetail.JobSheetNo=JobInstruction.JobSheetNo 
                                        WHERE JobAuditTrailDetail.LogTagNo =@LogTagNo GROUP BY JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.ProductName, JobAuditTrailDetail.JobClass, JobAuditTrailDetail.JobType, JobAuditTrailDetail.JobSheetNo, Planner.Machine, Planner.MachineInsert,Planner.StartProductionDateOn, Planner.PlanDatePostOn, Planner.PlanReturn_CourierOn, Planner.PlanShift,JobInstruction.IT_SysNotes, JobInstruction.Produc_PlanningNotes,JobInstruction.PurchasingNotes,JobInstruction.EngineeringNotes,JobInstruction.ArtworkNotes,JobInstruction.Acc_BillingNotes";

                command.Parameters.AddWithValue("@LogTagNo", LogTagNo);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JobInstruction model = new JobInstruction();
                    {
                        model.Bil = _bil++;
                        
                        if (reader.IsDBNull(0) == false)
                        {
                            ViewBag.Customer_Name = reader.GetString(0);
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            ViewBag.ProductName = reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            ViewBag.JobClass = reader.GetString(2);
                        }
                        if (reader.IsDBNull(3) == false)
                        {
                            ViewBag.JobType = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            ViewBag.JobSheetNo = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            ViewBag.AccQty = reader["AccQty"].ToString();
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            ViewBag.ImpQty = reader["ImpQty"].ToString();
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            ViewBag.PageQty = reader["PageQty"].ToString();
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            ViewBag.CreatedOn = String.Format("{0:dd/MM/yyyy }", (DateTime)reader.GetDateTime(8));
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            ViewBag.StartProductionDateOn = reader["StartProductionDateOn"].ToString();
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            ViewBag.Machine = reader.GetString(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            ViewBag.MachineInsert = reader.GetString(11);
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            ViewBag.PlanShift = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            ViewBag.PlanDatePostOn = reader["PlanDatePostOn"].ToString();
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            ViewBag.PlanReturn_CourierOn = reader["PlanReturn_CourierOn"].ToString();
                        }
                        if (reader.IsDBNull(15) == false)
                        {
                            ViewBag.IT_SysNotes = reader.GetString(15);
                        }
                        if (reader.IsDBNull(16) == false)
                        {
                            ViewBag.Produc_PlanningNotes = reader.GetString(16);
                        }
                        if (reader.IsDBNull(17) == false)
                        {
                            ViewBag.PurchasingNotes = reader.GetString(17);
                        }
                        if (reader.IsDBNull(18) == false)
                        {
                            ViewBag.EngineeringNotes = reader.GetString(18);
                        }
                        if (reader.IsDBNull(19) == false)
                        {
                            ViewBag.ArtworkNotes = reader.GetString(19);
                        }
                        if (reader.IsDBNull(20) == false)
                        {
                            ViewBag.Acc_BillingNotes = reader.GetString(20);
                        }
                        if (reader.IsDBNull(21) == false)
                        {
                            if(reader.GetString(21)=="Please Select"|| reader.GetString(21) == "")
                            {
                                ViewBag.Cust_Department = "-";

                            }
                            else
                            {
                                ViewBag.Cust_Department = reader.GetString(21);
                            }
                        }
                        else
                        {
                            ViewBag.Cust_Department = "-";

                        }
                    }
                    JobInstructionlist1.Add(model);
                }
                cn.Close();



            }
            //afif 1

            if (set == "CreatePlanSchedule")
            {
                string PlannerStatus = null;

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    int _bil = 1;
                    cn.Open();
                    command.CommandText = @"SELECT Status
                                        FROM  JobAuditTrailDetail
                                        WHERE LogTagNo = @LogTagNo";

                    command.Parameters.AddWithValue("@LogTagNo", LogTagNo);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            PlannerStatus = reader.GetString(0);
                        }
                    }
                        
                    cn.Close();



                }

                if (!string.IsNullOrEmpty(LogTagNo) && !string.IsNullOrEmpty(StartProductionDateOn) && PlannerStatus == "PLANNER")
                {
                    Guid Idx = Guid.NewGuid();
                    string modifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                    get.StartProductionDateOn = Convert.ToDateTime(get.StartProductionDateOnTxt);
                    get.PlanDatePostOn = Convert.ToDateTime(get.PlanDatePostOnTxt);
                    get.PlanReturn_CourierOn = Convert.ToDateTime(get.PlanReturn_CourierOnTxt);
                    string test = JobSheetNo;


                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        cn.Open();
                        SqlCommand command;
                        command = new SqlCommand("INSERT INTO [Planner] ( Id,[ModifiedOn],[StartProductionDateOn],[PlanDatePostOn],[PlanReturn_CourierOn],[CreateByPlanner],[StatusPlanner],[MachineInsert],[Machine],[PlanShift],[NotesByIT],[NotesByProduction],[NotesByPurchasing],[NotesByEngineering],[NotesByArtwork],[NotesByFinance],[NotesByDCP], [JobSheetNo], [LogTagNo])" +
                                      "VALUES (@Id,@ModifiedOn,@StartProductionDateOn,@PlanDatePostOn,@PlanReturn_CourierOn,@CreateByPlanner,@StatusPlanner,@MachineInsert,@Machine,@PlanShift,@NotesByIT,@NotesByProduction,@NotesByPurchasing,@NotesByEngineering,@NotesByArtwork,@NotesByFinance,@NotesByDCP,@JobSheetNo,@LogTagNo)", cn);

                        command.Parameters.AddWithValue("@ModifiedOn", modifiedOn);
                        command.Parameters.AddWithValue("@LogTagNo", LogTagNo);

                        if (!string.IsNullOrEmpty(StartProductionDateOn))
                        {
                            string dddd = Convert.ToDateTime(StartProductionDateOn).ToString("yyyy-MM-dd");
                            command.Parameters.AddWithValue("@StartProductionDateOn", dddd);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@StartProductionDateOn", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(PlanDatePostOn))
                        {
                            string eeee = Convert.ToDateTime(PlanDatePostOn).ToString("yyyy-MM-dd");
                            command.Parameters.AddWithValue("@PlanDatePostOn", eeee);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PlanDatePostOn", DBNull.Value);
                        //    command.Parameters.AddWithValue("@PlanDatePostOn", null);
                        }
                        if (!string.IsNullOrEmpty(PlanReturn_CourierOn))
                        {
                            string ffff = Convert.ToDateTime(PlanReturn_CourierOn).ToString("yyyy-MM-dd");
                            command.Parameters.AddWithValue("@PlanReturn_CourierOn", ffff);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PlanReturn_CourierOn", DBNull.Value);
                            //command.Parameters.AddWithValue("@PlanReturn_CourierOn", null);
                        }

                        if(!string.IsNullOrEmpty(MachineInsert))
                        {
                            command.Parameters.AddWithValue("@MachineInsert", MachineInsert);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@MachineInsert", DBNull.Value);

                        }

                        command.Parameters.AddWithValue("@CreateByPlanner", IdentityName.ToString());
                        command.Parameters.AddWithValue("@Id", Idx);
                        //command.Parameters.AddWithValue("@JobInstructionId", Id);
                        command.Parameters.AddWithValue("@StatusPlanner", "PRODUCTION");

                        if (!string.IsNullOrEmpty(Machine))
                        {
                            command.Parameters.AddWithValue("@Machine", Machine);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Machine", DBNull.Value);

                        }

                        if (!string.IsNullOrEmpty(PlanShift))
                        {
                            command.Parameters.AddWithValue("@PlanShift", PlanShift);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PlanShift", DBNull.Value);

                        }

                        if (!string.IsNullOrEmpty(NotesByIT))
                        {
                            command.Parameters.AddWithValue("@NotesByIT", NotesByIT);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@NotesByIT", DBNull.Value);

                        }

                        if (!string.IsNullOrEmpty(NotesByProduction))
                        {
                            command.Parameters.AddWithValue("@NotesByProduction", NotesByProduction);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@NotesByProduction", DBNull.Value);

                        }

                        if (!string.IsNullOrEmpty(NotesByPurchasing))
                        {
                            command.Parameters.AddWithValue("@NotesByPurchasing", NotesByPurchasing);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@NotesByPurchasing", DBNull.Value);

                        }

                        if (!string.IsNullOrEmpty(NotesByEngineering))
                        {
                            command.Parameters.AddWithValue("@NotesByEngineering", NotesByEngineering);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@NotesByEngineering", DBNull.Value);

                        }

                        if (!string.IsNullOrEmpty(NotesByArtwork))
                        {
                            command.Parameters.AddWithValue("@NotesByArtwork", NotesByArtwork);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@NotesByArtwork", DBNull.Value);

                        }

                        if (!string.IsNullOrEmpty(NotesByFinance))
                        {
                            command.Parameters.AddWithValue("@NotesByFinance", NotesByFinance);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@NotesByFinance", DBNull.Value);

                        }

                        if (!string.IsNullOrEmpty(NotesByDCP))
                        {
                            command.Parameters.AddWithValue("@NotesByDCP", NotesByDCP);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@NotesByDCP", DBNull.Value);

                        }

                        command.Parameters.AddWithValue("@JobSheetNo", JobSheetNo);
                        command.ExecuteNonQuery();

                        cn.Close();
                    }

                    //using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    //{

                    //    cn1.Open();
                    //    SqlCommand command1;
                    //    command1 = new SqlCommand("UPDATE [JobAuditTrail] SET STATUS='PRODUCTION' WHERE Id=@Id", cn1);
                    //    command1.Parameters.AddWithValue("@Id", Id);
                    //    command1.ExecuteNonQuery();
                    //    cn1.Close();
                    //}
                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    using (SqlCommand command = new SqlCommand("", cn))
                    {
                        int _bil = 1;
                        cn.Open();
                        command.CommandText = @"SELECT Id, NMRStatus
                                        FROM  JobInstruction
                                        WHERE JobSheetNo = @JobSheetNo";

                        command.Parameters.AddWithValue("@JobSheetNo", JobSheetNo);
                        var reader = command.ExecuteReader();
                        if (reader.Read()) // Check if there are rows returned
                        {
                            NMRStatus = reader["NMRStatus"].ToString(); // Assign the retrieved NMRStatus value to the NMRStatus variable
                        }
                        else
                        {
                            // Reader has no content
                            // Handle this case as needed
                        }
                        cn.Close();

                    }

                    if (NMRStatus == "COMPLETED")
                    {
                        using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            cn1.Open();
                            SqlCommand command1;
                            command1 = new SqlCommand("UPDATE [JobAuditTrailDetail] SET STATUS='PRODUCTION' WHERE LogTagNo=@JobSheetNo", cn1);
                            command1.Parameters.AddWithValue("@JobSheetNo", LogTagNo);
                            command1.ExecuteNonQuery();
                            cn1.Close();
                        }

                        using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            cn1.Open();
                            SqlCommand command1;
                            command1 = new SqlCommand("UPDATE [JobInstruction] SET STATUS='PRODUCTION', PrintSlip = 'PENDING' WHERE JobSheetNo=@Id", cn1);
                            command1.Parameters.AddWithValue("@Id", JobSheetNo);
                            command1.ExecuteNonQuery();
                            cn1.Close();
                        }
                    }
                    else if (NMRStatus=="Modified")
                    {
                        using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            cn1.Open();
                            SqlCommand command1;
                            command1 = new SqlCommand("UPDATE [JobAuditTrailDetail] SET STATUS='QME' WHERE LogTagNo=@JobSheetNo", cn1);
                            command1.Parameters.AddWithValue("@JobSheetNo", LogTagNo);
                            command1.ExecuteNonQuery();
                            cn1.Close();
                        }

                        using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            cn1.Open();
                            SqlCommand command1;
                            command1 = new SqlCommand("UPDATE [JobInstruction] SET STATUS='QME' WHERE JobSheetNo=@Id", cn1);
                            command1.Parameters.AddWithValue("@Id", JobSheetNo);
                            command1.ExecuteNonQuery();
                            cn1.Close();
                        }
                    }
                    else if (string.IsNullOrEmpty(NMRStatus))
                    {
                        using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            cn1.Open();
                            SqlCommand command1;
                            command1 = new SqlCommand("UPDATE [JobAuditTrailDetail] SET STATUS='QME' WHERE LogTagNo=@JobSheetNo", cn1);
                            command1.Parameters.AddWithValue("@JobSheetNo", LogTagNo);
                            command1.ExecuteNonQuery();
                            cn1.Close();
                        }

                        using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            cn1.Open();
                            SqlCommand command1;
                            command1 = new SqlCommand("UPDATE [JobInstruction] SET STATUS='QME' WHERE JobSheetNo=@Id", cn1);
                            command1.Parameters.AddWithValue("@Id", JobSheetNo);
                            command1.ExecuteNonQuery();
                            cn1.Close();
                        }
                    }
                    //afif
                    //return View("~/Views/Shared/CloseWindow.cshtml");
                    return RedirectToAction("ManagePlanner", "Planner");

                }
                else
                {
                    TempData["msgplanner"] = "<script>alert('Insert unsuccessful. Either Start Prod Date empty or Id Status is not Planner');</script>";
                    //return View("~/Views/Shared/CloseWindow.cshtml");
                    return View();
                }



            }


        }
        return View();
        //return RedirectToAction("CreatePlanSchedule", "Planner");



    }


    List<JobAuditTrailDetail> JobAuditTrailDetail = new List<JobAuditTrailDetail>();
    public ActionResult ViewAT(string ProductName, string LogTagNo,string set,string Id,string Customer_Name)
    {
        var IdentityName = @Session["Fullname"];
        ViewBag.IdentityName = @Session["Fullname"];
        ViewBag.IsDepart = @Session["Department"];
        var IsDepart = @Session["Department"];
        var Role = @Session["Role"];
        var Username = @Session["Username"];
        ViewBag.Username = @Session["Username"];
        Session["Id"] = Id;

        if (!string.IsNullOrEmpty(Id))
        {

            //ALL firt masuk
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT DISTINCT Customer_Name, ProductName,  LogTagNo 
                                        FROM [JobAuditTrailDetail]										
                                        where Customer_Name=@Customer_Name 
                                       ORDER BY Customer_Name";
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


        }

        return View(JobAuditTrailDetail);

    }



    public ActionResult CloseWindow()
    {
        return View("CloseWindow");
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




    List<JobAuditTrailDetail> liEB = new List<JobAuditTrailDetail>();
    private double totalamount;



}
   


