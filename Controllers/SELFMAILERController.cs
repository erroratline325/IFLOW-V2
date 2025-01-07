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
using GridMvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Numeric;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;

[MvcAppV2.FilterConfig.UserSessionActionFilter]
public class SELFMAILERController : Controller
{

    public ActionResult ManageSM(string product, string set, string pageNumber, string LogTagNo)
    {

        List<string> AIP = new List<string>();

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
                //command.CommandText = @"SELECT JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.ProductName, JobAuditTrailDetail.JobClass,
                //                            JobAuditTrailDetail.JobType,JobAuditTrailDetail.JobSheetNo, SUM(CAST(JobAuditTrailDetail.AccQty AS INT)) as AccQty, SUM(CAST(JobAuditTrailDetail.ImpQty AS INT)) as ImpQty, SUM(CAST(JobAuditTrailDetail.PageQty AS INT)) as PageQty,JobAuditTrailDetail.PrintSlip,JobAuditTrailDetail.LogTagNo,JobAuditTrailDetail.Id,JobAuditTrailDetail.SMSlip,MAX(Hist_ProductionSlip.CreatedOn) as CreatedOn
                //                        FROM [JobAuditTrailDetail] FULL JOIN Hist_ProductionSlip ON JobAuditTrailDetail.LogTagNo = Hist_ProductionSlip.LogTagNo 
                //                        WHERE JobAuditTrailDetail.Status = 'SELFMAILER' AND JobAuditTrailDetail.LogTagNo LIKE @LogTagNoSearch
                //                        GROUP BY JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.ProductName, JobAuditTrailDetail.JobClass,JobAuditTrailDetail.JobType,JobAuditTrailDetail.JobSheetNo,JobAuditTrailDetail.PrintSlip,JobAuditTrailDetail.LogTagNo,JobAuditTrailDetail.Id,JobAuditTrailDetail.SMSlip
                //                        ORDER BY (SELECT NULL)
                //                        OFFSET 0 ROWS
                //                        FETCH NEXT 100 ROWS ONLY";

                //command.CommandText = @"SELECT JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.ProductName, JobAuditTrailDetail.JobClass,
                //                            JobAuditTrailDetail.JobType,JobAuditTrailDetail.JobSheetNo, JobAuditTrailDetail.AccQty as AccQty,JobAuditTrailDetail.ImpQty  as ImpQty, JobAuditTrailDetail.PageQty as PageQty,JobAuditTrailDetail.PrintSlip,JobAuditTrailDetail.LogTagNo,JobAuditTrailDetail.Id,JobAuditTrailDetail.SMSlip,MAX(Hist_ProductionSlip.CreatedOn) as CreatedOn
                //                        FROM [JobAuditTrailDetail] FULL JOIN Hist_ProductionSlip ON JobAuditTrailDetail.LogTagNo = Hist_ProductionSlip.LogTagNo 
                //                        WHERE JobAuditTrailDetail.Status = 'SELFMAILER' AND JobAuditTrailDetail.LogTagNo LIKE @LogTagNoSearch
                //                        GROUP BY JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.ProductName, JobAuditTrailDetail.JobClass,JobAuditTrailDetail.JobType,JobAuditTrailDetail.JobSheetNo,JobAuditTrailDetail.PrintSlip,JobAuditTrailDetail.LogTagNo,JobAuditTrailDetail.Id,JobAuditTrailDetail.SMSlip,JobAuditTrailDetail.AccQty,JobAuditTrailDetail.ImpQty,JobAuditTrailDetail.PageQty";

                //      command.CommandText = @"SELECT MAX(JobAuditTrailDetail.Customer_Name), MAX(JobAuditTrailDetail.ProductName), MAX(JobAuditTrailDetail.JobClass),
                //                                  MAX(JobAuditTrailDetail.JobType),MAX(JobAuditTrailDetail.JobSheetNo), SUM(CAST(JobAuditTrailDetail.AccQty AS INT)) as AccQty, SUM(CAST(JobAuditTrailDetail.ImpQty AS INT)) as ImpQty, SUM(CAST(JobAuditTrailDetail.PageQty AS INT)) as PageQty,MAX(JobAuditTrailDetail.PrintSlip),JobAuditTrailDetail.LogTagNo,MAX(JobAuditTrailDetail.SMSlip),MAX(Hist_ProductionSlip.CreatedOn) as CreatedOn
                //                              FROM [JobAuditTrailDetail] FULL JOIN Hist_ProductionSlip ON JobAuditTrailDetail.LogTagNo = Hist_ProductionSlip.LogTagNo 
                //                              WHERE JobAuditTrailDetail.Status = 'SELFMAILER' AND JobAuditTrailDetail.LogTagNo LIKE @LogTagNoSearch
                //GROUP BY JobAuditTrailDetail.LogTagNo 
                //ORDER BY MAX(JobAuditTrailDetail.SMSlip) DESC";

                command.CommandText = @"SELECT 
                                            MAX(JobAuditTrailDetail.Customer_Name) AS Customer_Name, 
                                            MAX(JobAuditTrailDetail.ProductName) AS ProductName, 
                                            MAX(JobAuditTrailDetail.JobClass) AS JobClass,
                                            MAX(JobAuditTrailDetail.JobType) AS JobType,
                                            MAX(JobAuditTrailDetail.JobSheetNo) AS JobSheetNo,
                                            SUM(DISTINCT CAST(JobAuditTrailDetail.AccQty AS INT)) AS AccQty, 
                                            SUM(DISTINCT CAST(JobAuditTrailDetail.ImpQty AS INT)) AS ImpQty, 
                                            SUM(DISTINCT CAST(JobAuditTrailDetail.PageQty AS INT)) AS PageQty,
                                            MAX(JobAuditTrailDetail.PrintSlip) AS PrintSlip,
                                            MAX(JobAuditTrailDetail.LogTagNo) AS LogTagNo,
                                            MAX(JobAuditTrailDetail.InsertSlip) AS InsertSlip,
                                            MAX(Hist_ProductionSlip.CreatedOn) AS CreatedOn
                                        FROM 
                                            JobAuditTrailDetail 
                                        FULL JOIN 
                                            Hist_ProductionSlip 
                                        ON 
                                            JobAuditTrailDetail.LogTagNo = Hist_ProductionSlip.LogTagNo 
                                        WHERE 
                                            JobAuditTrailDetail.Status = 'SELFMAILER' AND JobAuditTrailDetail.LogTagNo LIKE @LogTagNoSearch
                                        GROUP BY 
                                            JobAuditTrailDetail.LogTagNo
                                        ORDER BY 
                                            MAX(JobAuditTrailDetail.SMSlip) DESC;";

                command.Parameters.AddWithValue("@LogTagNoSearch", "%" + LogTagNo + "%");
            }

            else if (set == "GoTo")
            {
                if (pageNumber == "0")
                {

                    command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass
                                            , JobType,JobSheetNo, StartDevDate, EndDevDate
                                            ,AccountsQty,ImpressionQty, PagesQty,IT_SysNotes
                                            ,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes
                                         ,ArtworkNotes,Acc_BillingNotes,SMSlip
                                        FROM [JobInstruction]
                                        WHERE Status = 'SELFMAILER'
                                        ORDER BY (SELECT NULL)
                                        OFFSET @PageNumber ROWS
                                        FETCH NEXT 100 ROWS ONLY";

                    command.Parameters.AddWithValue("@PageNumber", pageNumberInt);

                }
                else
                {
                    command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass
                                            , JobType,JobSheetNo, StartDevDate, EndDevDate
                                            ,AccountsQty,ImpressionQty, PagesQty,IT_SysNotes
                                            ,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes
                                         ,ArtworkNotes,Acc_BillingNotes,SMSlip
                                        FROM [JobInstruction]
                                        WHERE Status = 'SELFMAILER'
                                        ORDER BY (SELECT NULL)
                                        OFFSET @PageNumber100 ROWS
                                        FETCH NEXT 100 ROWS ONLY";

                    command.Parameters.AddWithValue("@PageNumber100", PageNumber100);

                }
            }

            else
            {

                //command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass
                //                            , JobType,JobSheetNo, StartDevDate, EndDevDate
                //                            ,AccountsQty,ImpressionQty, PagesQty,IT_SysNotes
                //                            ,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes
                //                         ,ArtworkNotes,Acc_BillingNotes,SMSlip
                //                        FROM [JobInstruction]
                //                        WHERE Status = 'SELFMAILER'
                //                        ORDER BY (SELECT NULL)
                //                        OFFSET 0 ROWS
                //                        FETCH NEXT 100 ROWS ONLY";

                //command.CommandText = @"SELECT JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.ProductName, JobAuditTrailDetail.JobClass,
                //                            JobAuditTrailDetail.JobType,JobAuditTrailDetail.JobSheetNo, SUM(CAST(JobAuditTrailDetail.AccQty AS INT)) as AccQty, SUM(CAST(JobAuditTrailDetail.ImpQty AS INT)) as ImpQty, SUM(CAST(JobAuditTrailDetail.PageQty AS INT)) as PageQty,JobAuditTrailDetail.PrintSlip,JobAuditTrailDetail.LogTagNo,JobAuditTrailDetail.Id,JobAuditTrailDetail.SMSlip,MAX(Hist_ProductionSlip.CreatedOn) as CreatedOn
                //                        FROM [JobAuditTrailDetail] FULL JOIN Hist_ProductionSlip ON JobAuditTrailDetail.LogTagNo = Hist_ProductionSlip.LogTagNo 
                //                        WHERE JobAuditTrailDetail.Status = 'SELFMAILER'
                //                        GROUP BY JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.ProductName, JobAuditTrailDetail.JobClass,JobAuditTrailDetail.JobType,JobAuditTrailDetail.JobSheetNo,JobAuditTrailDetail.PrintSlip,JobAuditTrailDetail.LogTagNo,JobAuditTrailDetail.Id,JobAuditTrailDetail.SMSlip
                //                        ORDER BY (SELECT NULL)
                //                        OFFSET 0 ROWS
                //                        FETCH NEXT 100 ROWS ONLY";



                //command.CommandText = @"SELECT JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.ProductName, JobAuditTrailDetail.JobClass,
                //                            JobAuditTrailDetail.JobType,JobAuditTrailDetail.JobSheetNo, SUM(CAST(JobAuditTrailDetail.AccQty AS INT)) as AccQty, SUM(CAST(JobAuditTrailDetail.ImpQty AS INT)) as ImpQty, SUM(CAST(JobAuditTrailDetail.PageQty AS INT)) as PageQty,JobAuditTrailDetail.PrintSlip,JobAuditTrailDetail.LogTagNo,JobAuditTrailDetail.Id,JobAuditTrailDetail.SMSlip,MAX(Hist_ProductionSlip.CreatedOn) as CreatedOn
                //                        FROM [JobAuditTrailDetail] FULL JOIN Hist_ProductionSlip ON JobAuditTrailDetail.LogTagNo = Hist_ProductionSlip.LogTagNo 
                //                        WHERE JobAuditTrailDetail.Status = 'SELFMAILER'
                //                        GROUP BY JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.ProductName, JobAuditTrailDetail.JobClass,JobAuditTrailDetail.JobType,JobAuditTrailDetail.JobSheetNo,JobAuditTrailDetail.PrintSlip,JobAuditTrailDetail.LogTagNo,JobAuditTrailDetail.Id,JobAuditTrailDetail.SMSlip,JobAuditTrailDetail.AccQty,JobAuditTrailDetail.ImpQty,JobAuditTrailDetail.PageQty";

                //      command.CommandText = @"SELECT MAX(JobAuditTrailDetail.Customer_Name), MAX(JobAuditTrailDetail.ProductName), MAX(JobAuditTrailDetail.JobClass),
                //                                  MAX(JobAuditTrailDetail.JobType),MAX(JobAuditTrailDetail.JobSheetNo), SUM(CAST(JobAuditTrailDetail.AccQty AS INT)) as AccQty, SUM(CAST(JobAuditTrailDetail.ImpQty AS INT)) as ImpQty, SUM(CAST(JobAuditTrailDetail.PageQty AS INT)) as PageQty,MAX(JobAuditTrailDetail.PrintSlip),JobAuditTrailDetail.LogTagNo,MAX(JobAuditTrailDetail.SMSlip),MAX(Hist_ProductionSlip.CreatedOn) as CreatedOn
                //                              FROM [JobAuditTrailDetail] FULL JOIN Hist_ProductionSlip ON JobAuditTrailDetail.LogTagNo = Hist_ProductionSlip.LogTagNo 
                //                              WHERE JobAuditTrailDetail.Status = 'SELFMAILER'     
                //GROUP BY JobAuditTrailDetail.LogTagNo 
                //ORDER BY MAX(JobAuditTrailDetail.SMSlip) DESC";

                command.CommandText = @"SELECT 
                                            MAX(JobAuditTrailDetail.Customer_Name) AS Customer_Name, 
                                            MAX(JobAuditTrailDetail.ProductName) AS ProductName, 
                                            MAX(JobAuditTrailDetail.JobClass) AS JobClass,
                                            MAX(JobAuditTrailDetail.JobType) AS JobType,
                                            MAX(JobAuditTrailDetail.JobSheetNo) AS JobSheetNo,
                                            SUM(DISTINCT CAST(JobAuditTrailDetail.AccQty AS INT)) AS AccQty, 
                                            SUM(DISTINCT CAST(JobAuditTrailDetail.ImpQty AS INT)) AS ImpQty, 
                                            SUM(DISTINCT CAST(JobAuditTrailDetail.PageQty AS INT)) AS PageQty,
                                            MAX(JobAuditTrailDetail.PrintSlip) AS PrintSlip,
                                            MAX(JobAuditTrailDetail.LogTagNo) AS LogTagNo,
                                            MAX(JobAuditTrailDetail.InsertSlip) AS InsertSlip,
                                            MAX(Hist_ProductionSlip.CreatedOn) AS CreatedOn
                                        FROM 
                                            JobAuditTrailDetail 
                                        FULL JOIN 
                                            Hist_ProductionSlip 
                                        ON 
                                            JobAuditTrailDetail.LogTagNo = Hist_ProductionSlip.LogTagNo 
                                        WHERE 
                                            JobAuditTrailDetail.Status = 'SELFMAILER' 
                                        GROUP BY 
                                            JobAuditTrailDetail.LogTagNo
                                        ORDER BY 
                                            MAX(JobAuditTrailDetail.SMSlip) DESC;";

            }

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    model.Bil = _bil++;
                    AIP = getAIP(reader.GetString(9));

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
                        //model.AccountsQty = reader["AccQty"].ToString();
                        model.AccountsQty = AIP[0];

                    }
                    else
                    {
                        model.AccountsQty = "0";
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        //model.ImpressionQty = reader["ImpQty"].ToString();
                        model.ImpressionQty = AIP[1];

                    }
                    else
                    {
                        model.ImpressionQty = "0";
                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        //model.PagesQty = reader["PageQty"].ToString();
                        model.PagesQty = AIP[2];

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
                    //if (reader.IsDBNull(10) == false)
                    //{
                    //    model.Id = reader.GetGuid(10);
                    //}
                    if (!reader.IsDBNull(10))
                    {
                        model.SMSlip = reader.GetString(10);
                    }
                    else
                    {
                        model.SMSlip = "PENDING";

                    }
                    if (!reader.IsDBNull(11))
                    {
                        model.ModifiedOn = reader.GetDateTime(11);
                    }




                }
                JobInstructionlist1.Add(model);

            }
            cn.Close();
        }

        return View(JobInstructionlist1);
    }

    public ActionResult DeletePS(string Id, string ProductionSlipId, string PrintSlipNo, string set, List<JobInstruction> selectedRows)
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
                        command.CommandText = @"SELECT Guid
                                          FROM [InsInserting]
                                          WHERE Guid=@Id";
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
                                    command3 = new SqlCommand("DELETE [InsInserting] WHERE Guid=@Id", cn3);
                                    command3.Parameters.AddWithValue("@Id", idAsString);
                                    command3.ExecuteNonQuery();
                                    cn3.Close();

                                }

                                using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                                {
                                    cn3.Open();
                                    SqlCommand command3;
                                    command3 = new SqlCommand("UPDATE [JobInstruction] SET SMSlip=NULL WHERE Id=@Id", cn3);
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
                command.CommandText = @"SELECT Guid
                                          FROM [InsInserting]
                                          WHERE Guid=@Id";
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
                            command3 = new SqlCommand("DELETE [InsInserting] WHERE Guid=@Id", cn3);
                            command3.Parameters.AddWithValue("@Id", Id);
                            command3.ExecuteNonQuery();
                            cn3.Close();

                        }

                        using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            cn3.Open();
                            SqlCommand command3;
                            command3 = new SqlCommand("UPDATE [JobInstruction] SET SMSlip=NULL WHERE Id=@Id", cn3);
                            command3.Parameters.AddWithValue("@Id", Id);
                            command3.ExecuteNonQuery();
                            cn3.Close();

                        }

                    }


                }
                cn.Close();
            }
        }

        return RedirectToAction("ManageSM", "SELFMAILER");
    }





    List<Hist_ProductionSlip> viewInsertingProcess = new List<Hist_ProductionSlip>();

    public ActionResult ManageProcessSM(string Id, string ProductName, string set)
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
                                               WHERE (Status = 'SELF MAILER') AND (ProcessType='SELF MAILER') OR (Status = 'READY TO SM') AND (ProcessType='SELF MAILER')
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
                                               WHERE (Status = 'SELF MAILER') AND (ProcessType='SELF MAILER') OR (Status = 'READY TO SM') AND (ProcessType='SELF MAILER')
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


    public ActionResult CreateProdSlip(Hist_ProductionSlip get, string set, string Id, string PrintSlipNo, string Ins_StartDateOn, string Ins_StartTime, string Ins_EndDateOn, string Ins_EndTime, string Ins_Recovery,
                                       string Ins_Machine, string Process, string Sort, string NonSort, string ProcessType, string Ins_CreateUser, string ProductionSlipId, string JobAuditTrailId,
                                       string NotesByIT, string NotesByProduction, string NotesByPurchasing, string NotesByEngineering, string NotesByArtwork, string NotesByFinance, string NotesByDCP, string LogTagNo, string JobSheetNo)

    {
        string InsId = "";
        string PrintSlipCheck = "";
        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.AccountManager = IdentityName.ToString();
        ViewBag.Id = Id;
        ViewBag.ProductionSlipId = ProductionSlipId;
        ViewBag.JobAuditTrailId = JobAuditTrailId;
        List<JobAuditTrailDetail> AT = new List<JobAuditTrailDetail>();


        List<SelectListItem> List4 = new List<SelectListItem>();

        List4.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        List4.Add(new SelectListItem { Text = "CUT SHEET", Value = "CUT SHEET" });
        List4.Add(new SelectListItem { Text = "COMPUTER FORM", Value = "COMPUTER FORM" });
        List4.Add(new SelectListItem { Text = "DCP", Value = "DCP" });


        ViewData["Machine_"] = List4;



        List<SelectListItem> li2 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Name FROM [PrintingType] ";
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

        using (SqlConnection cn6 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        {
            cn6.Open();

            //SqlCommand cmdCheck = new SqlCommand("SELECT PrintSlipNo, LogTagNo, JobSheetNo FROM Hist_ProductionSlip WHERE ProductionSlipId=@IdCheck", cn6);
            //cmdCheck.Parameters.AddWithValue("IdCheck", Id);
            //SqlDataReader rmCheck = cmdCheck.ExecuteReader();

            //if (rmCheck.HasRows)
            //{
            //    if (rmCheck.Read())
            //    {
            //        PrintSlipNo = rmCheck.GetString(0);
            //        LogTagNo = rmCheck.GetString(1);
            //        JobSheetNo = rmCheck.GetString(2);
            //    }
            //}

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();


                if (Id.ToString() == "00000000-0000-0000-0000-000000000000" && !string.IsNullOrEmpty(Id))
                {
                    Debug.WriteLine("Masuk First Load");

                  

                    //SqlCommand cmd1 = new SqlCommand("SELECT TOP (1) AccQty, ImpQty, ImpQty,Status,FileId,JobSheetNo,Id FROM JobAuditTrailDetail WHERE JobSheetNo=@JobSheetNo", cn6);
                    SqlCommand cmd1 = new SqlCommand(@"SELECT TOP(1) 
                                                        SUM(CAST(JobAuditTrailDetail.AccQty AS INT)) AS AccQty, 
                                                        SUM(CAST(JobAuditTrailDetail.ImpQty AS INT)) AS ImpQty, 
                                                        SUM(CAST(JobAuditTrailDetail.PageQty AS INT)) AS PageQty, 
                                                        JobAuditTrailDetail.LogTagNo, 
                                                        JobAuditTrailDetail.JobSheetNo, 
                                                        FORMAT(CONVERT(date, InsInserting.Ins_StartDateOn), 'yyyy-MM-dd') AS Ins_StartDateOn, 
                                                        InsInserting.Ins_StartTime, 
                                                        FORMAT(CONVERT(date, InsInserting.Ins_EndDateOn), 'yyyy-MM-dd') AS Ins_EndDateOn, 
                                                        InsInserting.Ins_EndTime, 
                                                        InsInserting.Ins_Machine, 
                                                        JobAuditTrailDetail.Id AS Id, 
                                                        InsInserting.PrintSlipNo 
                                                    FROM JobAuditTrailDetail 
                                                    FULL JOIN InsInserting 
                                                        ON JobAuditTrailDetail.LogTagNo = InsInserting.LogTagNo 
                                                    WHERE InsInserting.LogTagNo = @LogTagNo 
                                                        AND JobAuditTrailDetail.Status = 'SELFMAILER' 
                                                    GROUP BY 
                                                        JobAuditTrailDetail.LogTagNo, 
                                                        JobAuditTrailDetail.JobSheetNo, 
                                                        FORMAT(CONVERT(date, InsInserting.Ins_StartDateOn), 'yyyy-MM-dd'), 
                                                        InsInserting.Ins_StartTime, 
                                                        FORMAT(CONVERT(date, InsInserting.Ins_EndDateOn), 'yyyy-MM-dd'), 
                                                        InsInserting.Ins_EndTime, 
                                                        InsInserting.Ins_Machine, 
                                                        JobAuditTrailDetail.Id, 
                                                        InsInserting.PrintSlipNo", cn);
                    cmd1.Parameters.AddWithValue("@LogTagNo", LogTagNo);
                    SqlDataReader rm1 = cmd1.ExecuteReader();

                    if (rm1.HasRows)
                    {
                        Debug.WriteLine("ada row");
                        while (rm1.Read())
                        {
                            if (!rm1.IsDBNull(0))
                            {
                                ViewBag.AccQty = rm1["AccQty"].ToString();

                            }
                            if (!rm1.IsDBNull(1))
                            {
                                ViewBag.ImpQty = rm1["ImpQty"].ToString();

                            }
                            if (!rm1.IsDBNull(2))
                            {
                                ViewBag.PageQty = rm1["PageQty"].ToString();

                            }
                            if (!rm1.IsDBNull(3))
                            {
                                ViewBag.LogTagNo = rm1.GetString(3);

                            }
                            if (!rm1.IsDBNull(4))
                            {
                                ViewBag.JobSheetNo = rm1.GetString(4);

                            }
                            if (!rm1.IsDBNull(5))
                            {
                                ViewBag.Ins_StartDateOn = rm1["Ins_StartDateOn"].ToString();

                            }
                            if (!rm1.IsDBNull(6))
                            {
                                ViewBag.Ins_StartTime = rm1.GetString(6);

                            }
                            if (!rm1.IsDBNull(7))
                            {
                                ViewBag.Ins_EndDateOn = rm1["Ins_EndDateOn"].ToString();

                            }
                            if (!rm1.IsDBNull(8))
                            {
                                ViewBag.Ins_EndTime = rm1.GetString(8);

                            }
                            if (!rm1.IsDBNull(9))
                            {
                                ViewBag.Machine = rm1.GetString(9);

                            }
                            if (!rm1.IsDBNull(10))
                            {
                                ViewBag.Id = rm1.GetGuid(10);
                                Id= rm1["Id"].ToString();
                            }
                            if (!rm1.IsDBNull(11))
                            {
                                ViewBag.PrintSlipNo = rm1.GetString(11);
                            }


                        }

                    }
                    else
                    {
                        Debug.WriteLine("Xde row");
                    }

                    SqlCommand cmd0 = new SqlCommand(@"SELECT TOP(1) AccQty, ImpQty, PageQty, LogTagNo, JobSheetNo FROM JobAuditTrailDetail WHERE LogTagNo=@IdFirst", cn);
                    cmd0.Parameters.AddWithValue("@IdFirst", LogTagNo);
                    SqlDataReader rm0 = cmd0.ExecuteReader();

                    while (rm0.Read())
                    {
                        ViewBag.AccQty = rm0.GetString(0);
                        ViewBag.ImpQty = rm0.GetString(1);
                        ViewBag.PageQty = rm0.GetString(2);
                        ViewBag.LogTagNo = rm0.GetString(3);
                        ViewBag.JobSheetNo = rm0.GetString(4);
                    }
                }
                else
                {
                    Debug.WriteLine("Masuk Custom Load");

                   
                    //SqlCommand cmd1 = new SqlCommand("SELECT TOP (1) AccQty, ImpQty, ImpQty,Status,FileId,JobSheetNo,Id FROM JobAuditTrailDetail WHERE JobSheetNo=@JobSheetNo", cn6);
                    SqlCommand cmd1 = new SqlCommand(@"SELECT
                                                        JobAuditTrailDetail.AccQty , 
                                                        JobAuditTrailDetail.ImpQty, 
                                                        JobAuditTrailDetail.PageQty, 
                                                        JobAuditTrailDetail.LogTagNo, 
                                                        JobAuditTrailDetail.JobSheetNo, 
                                                        FORMAT(CONVERT(date, InsInserting.Ins_StartDateOn), 'yyyy-MM-dd') AS Ins_StartDateOn, 
                                                        InsInserting.Ins_StartTime, 
                                                        FORMAT(CONVERT(date, InsInserting.Ins_EndDateOn), 'yyyy-MM-dd') AS Ins_EndDateOn, 
                                                        InsInserting.Ins_EndTime, 
                                                        InsInserting.Ins_Machine, 
                                                        JobAuditTrailDetail.Id AS Id, 
                                                        InsInserting.PrintSlipNo 
                                                    FROM JobAuditTrailDetail 
                                                    FULL JOIN InsInserting 
                                                        ON JobAuditTrailDetail.LogTagNo = InsInserting.LogTagNo 
                                                    WHERE InsInserting.PrintSlipNo = @PrintSlipNo 
                                                        AND JobAuditTrailDetail.Status = 'SELFMAILER' 
                                                    GROUP BY 
                                                        JobAuditTrailDetail.LogTagNo, 
                                                        JobAuditTrailDetail.JobSheetNo, 
                                                        FORMAT(CONVERT(date, InsInserting.Ins_StartDateOn), 'yyyy-MM-dd'), 
                                                        InsInserting.Ins_StartTime, 
                                                        FORMAT(CONVERT(date, InsInserting.Ins_EndDateOn), 'yyyy-MM-dd'), 
                                                        InsInserting.Ins_EndTime,  JobAuditTrailDetail.AccQty , 
                                                        JobAuditTrailDetail.ImpQty, 
                                                        JobAuditTrailDetail.PageQty, 
                                                        InsInserting.Ins_Machine, 
                                                        JobAuditTrailDetail.Id, 
                                                        InsInserting.PrintSlipNo", cn);
                    cmd1.Parameters.AddWithValue("@PrintSlipNo", PrintSlipNo);
                    SqlDataReader rm1 = cmd1.ExecuteReader();

                    if (rm1.HasRows)
                    {
                        Debug.WriteLine("ada row");
                        while (rm1.Read())
                        {
                            //if (!rm1.IsDBNull(0))
                            //{
                            //    ViewBag.AccQty = rm1["AccQty"].ToString();

                            //}
                            //if (!rm1.IsDBNull(1))
                            //{
                            //    ViewBag.ImpQty = rm1["ImpQty"].ToString();

                            //}
                            //if (!rm1.IsDBNull(2))
                            //{
                            //    ViewBag.PageQty = rm1["PageQty"].ToString();

                            //}
                            if (!rm1.IsDBNull(3))
                            {
                                ViewBag.LogTagNo = rm1.GetString(3);

                            }
                            if (!rm1.IsDBNull(4))
                            {
                                ViewBag.JobSheetNo = rm1.GetString(4);

                            }
                            if (!rm1.IsDBNull(5))
                            {
                                ViewBag.Ins_StartDateOn = rm1["Ins_StartDateOn"].ToString();

                            }
                            if (!rm1.IsDBNull(6))
                            {
                                ViewBag.Ins_StartTime = rm1.GetString(6);

                            }
                            if (!rm1.IsDBNull(7))
                            {
                                ViewBag.Ins_EndDateOn = rm1["Ins_EndDateOn"].ToString();

                            }
                            if (!rm1.IsDBNull(8))
                            {
                                ViewBag.Ins_EndTime = rm1.GetString(8);

                            }
                            if (!rm1.IsDBNull(9))
                            {
                                ViewBag.Machine = rm1.GetString(9);

                            }
                            if (!rm1.IsDBNull(10))
                            {
                                ViewBag.Id = rm1.GetGuid(10);
                            }
                            if (!rm1.IsDBNull(11))
                            {
                                ViewBag.PrintSlipNo = rm1.GetString(11);
                            }


                        }

                    }
                    else
                    {
                        Debug.WriteLine("Xde row");
                    }

                    SqlCommand cmd0 = new SqlCommand(@"SELECT TOP(1) AccQty, ImpQty, PageQty, LogTagNo, JobSheetNo FROM JobAuditTrailDetail WHERE Id=@IdFirst", cn);
                    cmd0.Parameters.AddWithValue("@IdFirst", Id);
                    SqlDataReader rm0 = cmd0.ExecuteReader();

                    while (rm0.Read())
                    {
                        ViewBag.AccQty = rm0.GetString(0);
                        Debug.WriteLine("Acc Qty Custom : " + rm0.GetString(0));
                        ViewBag.ImpQty = rm0.GetString(1);
                        Debug.WriteLine("Imp Qty Custom : " + rm0.GetString(1));

                        ViewBag.PageQty = rm0.GetString(2);
                        Debug.WriteLine("Imp Qty Custom : " + rm0.GetString(2));

                        ViewBag.LogTagNo = rm0.GetString(3);
                        ViewBag.JobSheetNo = rm0.GetString(4);
                    }

                   
                }
                cn.Close();


            }


            int _bil = 1;

            //FILE 
            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn2.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT 
                                                    MAX(JobAuditTrailDetail.LogTagNo),
                                                    MAX(JobAuditTrailDetail.Customer_Name) AS Customer_Name,
                                                    MAX(JobAuditTrailDetail.ProductName) AS ProductName,
                                                    MAX(JobAuditTrailDetail.FileId) AS FileId,
                                                    Hist_ProductionSlip.PrintSlipNo AS PrintSlipNo,
	                                                JobAuditTrailDetail.Id
                                                FROM 
                                                    JobAuditTrailDetail
                                                FULL JOIN 
                                                    Hist_ProductionSlip 
                                                ON 
                                                    Hist_ProductionSlip.ProductionSlipId = JobAuditTrailDetail.Id
                                                WHERE 
                                                    Hist_ProductionSlip.ProcessType = 'SELFMAILER' 
                                                    AND Hist_ProductionSlip.LogTagNo = @LogTagNo
                                                GROUP BY 
                                                    Hist_ProductionSlip.PrintSlipNo,JobAuditTrailDetail.Id
                                                ORDER BY Hist_ProductionSlip.PrintSlipNo", cn2);
                cmd.Parameters.AddWithValue("@LogTagNo", LogTagNo);
                SqlDataReader rm = cmd.ExecuteReader();

                while (rm.Read())
                {
                    JobAuditTrailDetail model = new JobAuditTrailDetail();
                    {
                        model.Bil = _bil++;
                        if (!rm.IsDBNull(0))
                        {
                            model.LogTagNo = rm.GetString(0);
                        }
                        if (!rm.IsDBNull(1))
                        {
                            model.Customer_Name = rm.GetString(1);
                        }
                        if (!rm.IsDBNull(2))
                        {
                            model.ProductName = rm.GetString(2);
                        }
                        if (!rm.IsDBNull(3))
                        {
                            model.FileId = rm.GetString(3);
                        }
                        if (!rm.IsDBNull(4))
                        {
                            model.JobSheetNo = rm.GetString(4);
                        }
                        if (!rm.IsDBNull(5))
                        {
                            model.Id = rm.GetGuid(5);
                        }
                    }

                    AT.Add(model);

                }

                cn2.Close();
            }



            //using (SqlCommand command3 = new SqlCommand("", cn6))
            //{
            //    command3.CommandText = @"SELECT JobAuditTrailDetail.AccQty, JobAuditTrailDetail.ImpQty, JobAuditTrailDetail.PageQty, Hist_ProductionSlip.StartDateOn, Hist_ProductionSlip.StartTime, Hist_ProductionSlip.EndDateOn, Hist_ProductionSlip.EndTime, Hist_ProductionSlip.Machine, Hist_
            //    .Recovery,Hist_ProductionSlip.PrintSlipNo, JobAuditTrailDetail.LogTagNo,JobAuditTrailDetail.JobSheetNo
            //                             FROM JobAuditTrailDetail INNER JOIN
            //                        Hist_ProductionSlip ON JobAuditTrailDetail.Id = Hist_ProductionSlip.ProductionSlipId
            //                        WHERE Hist_ProductionSlip.PrintSlipNo =@Id";
            //    command3.Parameters.AddWithValue("@Id", PrintSlipNo);
            //    var reader6 = command3.ExecuteReader();
            //    while (reader6.Read())
            //    {
            //        if (reader6.IsDBNull(0) == false)
            //        {
            //            ViewBag.AccQty = reader6.GetString(0);
            //        }
            //        if (reader6.IsDBNull(1) == false)
            //        {
            //            ViewBag.ImpQty = reader6.GetString(1);
            //        }

            //        if (reader6.IsDBNull(2) == false)
            //        {
            //            ViewBag.PageQty = reader6.GetString(2);
            //        }

            //        if (reader6.IsDBNull(3) == false)
            //        {
            //            ViewBag.StartDateOn = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader6.GetDateTime(3));
            //        }

            //        if (reader6.IsDBNull(4) == false)
            //        {
            //            ViewBag.StartTime = reader6.GetString(4);
            //        }

            //        if (reader6.IsDBNull(5) == false)
            //        {
            //            ViewBag.EndDateOn = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader6.GetDateTime(5));
            //        }

            //        if (reader6.IsDBNull(6) == false)
            //        {
            //            ViewBag.EndTime = reader6.GetString(6);
            //        }

            //        if (reader6.IsDBNull(7) == false)
            //        {
            //            ViewBag.Ins_Machine = reader6.GetString(7);
            //        }
            //        if (reader6.IsDBNull(8) == false)
            //        {
            //            ViewBag.Ins_Recovery = reader6.GetString(8);
            //        }
            //        if (reader6.IsDBNull(9) == false)
            //        {
            //            ViewBag.PrintSlipNo = reader6.GetString(9);
            //        }
            //        if (reader6.IsDBNull(10) == false)
            //        {
            //            ViewBag.LogTagNo = reader6.GetString(10);
            //        }
            //        if (reader6.IsDBNull(11) == false)
            //        {
            //            ViewBag.JobSheetNo = reader6.GetString(11);
            //        }


            //    }
            //}

            cn6.Close();

        }

        using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        {
            cn2.Open();

            //using (SqlCommand command3 = new SqlCommand("", cn2))
            //{

            //    command3.CommandText = @"SELECT InsInserting.Guid,FORMAT(CONVERT(date, InsInserting.Ins_StartDateOn), 'yyyy-MM-dd') as Ins_StartDateOn , InsInserting.Ins_StartTime, FORMAT(CONVERT(date, InsInserting.Ins_EndDateOn), 'yyyy-MM-dd') as Ins_EndDateOn , InsInserting.Ins_EndTime, InsInserting.Ins_Machine, InsInserting.Ins_Recovery, InsInserting.Sort, InsInserting.NonSort, JobAuditTrailDetail.InsertSlip, JobAuditTrailDetail.LogTagNo,JobAuditTrailDetail.JobSheetNo,InsInserting.PrintSlipNo
            //                                 FROM  JobAuditTrailDetail FULL JOIN
            //                                  InsInserting ON JobAuditTrailDetail.Id = InsInserting.Guid                              
            //                                   WHERE InsInserting.PrintSlipNo=@PrintSlipNo";
            //    command3.Parameters.AddWithValue("@PrintSlipNo", PrintSlipNo);
            //    var reader3 = command3.ExecuteReader();
            //    while (reader3.Read())
            //    {
            //        if (reader3.IsDBNull(0) == false)
            //        {
            //            ViewBag.Guid = reader3.GetGuid(0);
            //            InsId = reader3.GetGuid(0).ToString();
            //        }

            //        if (reader3.IsDBNull(1) == false)
            //        {
            //            ViewBag.Ins_StartDateOn = reader3["Ins_StartDateOn"].ToString();
            //        }
            //        if (reader3.IsDBNull(2) == false)
            //        {
            //            ViewBag.Ins_StartTime = reader3.GetString(2);
            //        }
            //        if (reader3.IsDBNull(3) == false)
            //        {
            //            ViewBag.Ins_EndDateOn = reader3["Ins_EndDateOn"].ToString();
            //        }
            //        if (reader3.IsDBNull(4) == false)
            //        {
            //            ViewBag.Ins_EndTime = reader3.GetString(4);
            //        }
            //        if (reader3.IsDBNull(5) == false)
            //        {
            //            ViewBag.Ins_Machine = reader3.GetString(5);
            //        }

            //        if (reader3.IsDBNull(6) == false)
            //        {
            //            ViewBag.Ins_Recovery = reader3.GetString(6);
            //        }

            //        if (reader3.IsDBNull(7) == false)
            //        {
            //            bool getSort = reader3.GetBoolean(7);
            //            if (getSort == false)
            //            {
            //                ViewBag.Sort = "";
            //            }
            //            else
            //            {
            //                ViewBag.Sort = "checked";
            //            }
            //        }
            //        if (reader3.IsDBNull(8) == false)
            //        {
            //            bool getNonSort = reader3.GetBoolean(8);
            //            if (getNonSort == false)
            //            {
            //                ViewBag.NonSort = "";
            //            }
            //            else
            //            {
            //                ViewBag.NonSort = "checked";
            //            }
            //        }
            //        if (reader3.IsDBNull(9) == false)
            //        {
            //            ViewBag.InsertSlip = reader3.GetString(9);
            //        }
            //        if (reader3.IsDBNull(10) == false)
            //        {
            //            ViewBag.LogTagNo = reader3.GetString(10);
            //        }
            //        if (reader3.IsDBNull(11) == false)
            //        {
            //            ViewBag.JobSheetNo = reader3.GetString(11);
            //        }
            //        if (reader3.IsDBNull(12) == false)
            //        {
            //            PrintSlipCheck = reader3.GetString(12);
            //        }




            //    }
            //}

            cn2.Close();


        }

        if (set == "CreateProductionSlip")
        {


            if (string.IsNullOrEmpty(PrintSlipNo))
            {
                Debug.WriteLine("Masuk Create");

                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    Guid guidId = Guid.NewGuid();
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                    get.Ins_StartDateOn = Convert.ToDateTime(get.Ins_StartDateOnTxt);
                    get.Ins_EndDateOn = Convert.ToDateTime(get.Ins_EndDateOnTxt);

                    cn2.Open();


                    List<string> PrintSlipList = new List<string>();
                    List<string> ATId = new List<string>();

                    SqlCommand cmd0 = new SqlCommand("SELECT PrintSlipNo,ProductionSlipId FROM Hist_ProductionSlip WHERE LogTagNo=@LogTagNoCheck", cn2);
                    cmd0.Parameters.AddWithValue("@LogTagNoCheck", LogTagNo);
                    SqlDataReader rm0= cmd0.ExecuteReader();

                    while(rm0.Read())
                    {
                        PrintSlipList.Add(rm0.GetString(0));
                        ATId.Add(rm0.GetString(1));
                    }

                    foreach (var PrintSlip in PrintSlipList)
                    {
                        SqlCommand command2;
                        command2 = new SqlCommand("INSERT INTO [InsInserting] (Guid, ModifiedOn, Ins_StartDateOn, Ins_StartTime, Ins_EndDateOn, Ins_EndTime, Ins_Machine, Ins_Recovery, Sort, Ins_CreateUser, JobSheetNo,NonSort,LogTagNo,JobInstructionId,PrintSlipNo) values (@Guid,@ModifiedOn,@Ins_StartDateOn, @Ins_StartTime,@Ins_EndDateOn,@Ins_EndTime,@Ins_Machine,@Ins_Recovery, @Sort, @Ins_CreateUser,@JobSheetNo,@NonSort,@LogTagNo,@JobInstructionId,@PrintSlipNo)", cn2);
                        command2.Parameters.AddWithValue("@Guid", Guid.NewGuid());
                        command2.Parameters.AddWithValue("@ModifiedOn", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                        command2.Parameters.AddWithValue("@LogTagNo", LogTagNo);

                        if (!string.IsNullOrEmpty(Ins_StartDateOn))
                        {
                            string a3 = Convert.ToDateTime(Ins_StartDateOn).ToString("yyyy-MM-dd");
                            command2.Parameters.AddWithValue("@Ins_StartDateOn", a3);
                        }
                        else
                        {
                            command2.Parameters.AddWithValue("@Ins_StartDateOn", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(Ins_StartTime))
                        {
                            command2.Parameters.AddWithValue("@Ins_StartTime", Ins_StartTime);

                        }
                        else
                        {
                            command2.Parameters.AddWithValue("@Ins_StartTime", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(Ins_EndDateOn))
                        {
                            string a4 = Convert.ToDateTime(Ins_EndDateOn).ToString("yyyy-MM-dd");
                            command2.Parameters.AddWithValue("@Ins_EndDateOn", a4);
                        }
                        else
                        {
                            command2.Parameters.AddWithValue("@Ins_EndDateOn", DBNull.Value);
                        }


                        if (!string.IsNullOrEmpty(Ins_EndTime))
                        {
                            command2.Parameters.AddWithValue("@Ins_EndTime", Ins_EndTime);

                        }
                        else
                        {
                            command2.Parameters.AddWithValue("@Ins_EndTime", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(Ins_Machine))
                        {
                            command2.Parameters.AddWithValue("@Ins_Machine", Ins_Machine);

                        }
                        else
                        {
                            command2.Parameters.AddWithValue("@Ins_Machine", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(Ins_Recovery))
                        {
                            command2.Parameters.AddWithValue("@Ins_Recovery", Ins_Recovery);

                        }
                        else
                        {
                            command2.Parameters.AddWithValue("@Ins_Recovery", DBNull.Value);
                        }


                        if (Sort == "on")
                        {
                            command2.Parameters.AddWithValue("@Sort", true);
                        }
                        else
                        {
                            command2.Parameters.AddWithValue("@Sort", false);
                        }

                        command2.Parameters.AddWithValue("@Ins_CreateUser", IdentityName.ToString());

                        command2.Parameters.AddWithValue("@JobSheetNo", JobSheetNo);
                        command2.Parameters.AddWithValue("@JobInstructionId", Id);

                        if (NonSort == "on")
                        {
                            command2.Parameters.AddWithValue("@NonSort", true);
                        }
                        else
                        {
                            command2.Parameters.AddWithValue("@NonSort", false);
                        }

                        command2.Parameters.AddWithValue("@PrintSlipNo", PrintSlip);

                        command2.ExecuteNonQuery();
                    }



                   
                    cn2.Close();

                }

                using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn1.Open();
                    SqlCommand command1;
                    command1 = new SqlCommand("UPDATE [JobAuditTrailDetail] SET SMSlip='CREATED' WHERE Id=@Id", cn1);
                    command1.Parameters.AddWithValue("@Id", Id);
                    command1.ExecuteNonQuery();


                    SqlCommand cmd0 = new SqlCommand("SELECT TOP (1) PrintSlipNo FROM InsInserting WHERE LogTagNo = @LogTagNoIns", cn1);
                    cmd0.Parameters.AddWithValue("@LogTagNoIns", LogTagNo);
                    SqlDataReader rm0 = cmd0.ExecuteReader();

                    while (rm0.Read())
                    {
                        PrintSlipNo = rm0.GetString(0);
                    }

                    cn1.Close();


                }

                TempData["msg"] = "<script>alert('SUCCESSFULLY SUBMIT TO POSTING !');</script>";


                return RedirectToAction("CreateProdSlip", "SELFMAILER", new { Id = Id, LogTagNo =LogTagNo,JobSheetNo=JobSheetNo,PrintSlipNo=PrintSlipNo}); ;
            }
            else if (!string.IsNullOrEmpty(PrintSlipNo))
            {
                Debug.WriteLine("Masuk update");
                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn2.Open();

                    Guid guidId = Guid.NewGuid();
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                    get.Ins_StartDateOn = Convert.ToDateTime(get.Ins_StartDateOnTxt);
                    get.Ins_EndDateOn = Convert.ToDateTime(get.Ins_EndDateOnTxt);


                    SqlCommand command2;
                    command2 = new SqlCommand("UPDATE [InsInserting] SET Ins_StartDateOn = @Ins_StartDateOn, Ins_StartTime = @Ins_StartTime, Ins_EndDateOn = @Ins_EndDateOn, Ins_EndTime = @Ins_EndTime, Ins_Machine = @Ins_Machine, Ins_Recovery = @Ins_Recovery, Sort = @Sort, Ins_CreateUser = @Ins_CreateUser, NonSort = @NonSort WHERE PrintSlipNo = @Guid", cn2);
                    command2.Parameters.AddWithValue("@Guid", PrintSlipNo);
                    if (!string.IsNullOrEmpty(Ins_StartDateOn))
                    {
                        string a3 = Convert.ToDateTime(Ins_StartDateOn).ToString("yyyy-MM-dd");
                        command2.Parameters.AddWithValue("@Ins_StartDateOn", a3);
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@Ins_StartDateOn", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(Ins_StartTime))
                    {
                        command2.Parameters.AddWithValue("@Ins_StartTime", Ins_StartTime);

                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@Ins_StartTime", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(Ins_EndDateOn))
                    {
                        string a4 = Convert.ToDateTime(Ins_EndDateOn).ToString("yyyy-MM-dd");
                        command2.Parameters.AddWithValue("@Ins_EndDateOn", a4);
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@Ins_EndDateOn", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(Ins_EndTime))
                    {
                        command2.Parameters.AddWithValue("@Ins_EndTime", Ins_EndTime);

                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@Ins_EndTime", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(Ins_Machine))
                    {
                        command2.Parameters.AddWithValue("@Ins_Machine", Ins_Machine);

                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@Ins_Machine", DBNull.Value);
                    }


                    if (!string.IsNullOrEmpty(Ins_Recovery))
                    {
                        command2.Parameters.AddWithValue("@Ins_Recovery", Ins_Recovery);

                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@Ins_Recovery", DBNull.Value);
                    }

                    if (Sort == "on")
                    {
                        command2.Parameters.AddWithValue("@Sort", true);
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@Sort", false);
                    }

                    command2.Parameters.AddWithValue("@Ins_CreateUser", IdentityName.ToString());

                    if (NonSort == "on")
                    {
                        command2.Parameters.AddWithValue("@NonSort", true);
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@NonSort", false);
                    }



                    command2.ExecuteNonQuery();
                    cn2.Close();

                }

                //return RedirectToAction("ManageSM", "SELFMAILER");

                return RedirectToAction("CreateProdSlip", "SELFMAILER", new { Id = Id, LogTagNo = LogTagNo, JobSheetNo = JobSheetNo, PrintSlipNo = PrintSlipNo }); ;


            }

        }



        return View(AT);

    }


    public ActionResult ViewTransProductionSlip(string Set, string Id, string ProductionSlipId)
    {
        ViewBag.Id = Id;

        if (Set == "back")
        {
            return RedirectToAction("ManageProcessSM", "SELFMAILER");
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


            }
            cn1.Close();
        }
        return View();
    }




    public ActionResult SubmitPosting(string Id, string JobInstructionId, string JobType, string set, List<JobInstruction> selectedRows, string LogTagNo)

    {
        string PrintSlipNo = "";
        var IdentityName = @Session["Fullname"];
        string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

        //if (set == "NoSlip")
        //{
        //    TempData["Message"] = "Slip is not created";
        //    return RedirectToAction("ManageSM", "SELFMAILER");
        //}

        //if (set == "BlastProdSlip")
        //{
        //    foreach (var row in selectedRows)
        //    {
        //        string idAsString = row.Id.ToString();

        //        using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        //        {
        //            cn1.Open();
        //            SqlCommand command1;
        //            command1 = new SqlCommand("UPDATE [JobInstruction] SET STATUS='POSTING' WHERE Id=@Id", cn1);
        //            command1.Parameters.AddWithValue("@Id", idAsString);
        //            command1.ExecuteNonQuery();
        //            cn1.Close();
        //        }

        //        using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        //        {
        //            cn1.Open();
        //            SqlCommand command1;
        //            command1 = new SqlCommand("UPDATE [InsInserting] SET InsInserting='POSTING' WHERE JobInstructionId=@JobInstructionId", cn1);
        //            command1.Parameters.AddWithValue("@JobInstructionId", idAsString);
        //            command1.ExecuteNonQuery();
        //            cn1.Close();
        //        }
        //    }
        //}

        using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        {
            cn1.Open();

            //SqlCommand cmdCheck = new SqlCommand("SELECT Hist_ProductionSlip.PrintSlipNo FROM Hist_ProductionSlip FULL JOIN JobAuditTrailDetail ON JobAuditTrailDetail.Id = Hist_ProductionSlip.ProductionSlipId WHERE JobAuditTrailDetail.Id=@Id", cn1);
            //cmdCheck.Parameters.AddWithValue("@Id", Id);
            //SqlDataReader rmCheck = cmdCheck.ExecuteReader();

            //if (rmCheck.HasRows)
            //{
            //    while (rmCheck.Read())
            //    {
            //        PrintSlipNo = rmCheck.GetString(0);
            //    }
            //}
            //else
            //{
            //    //new { Message = "<script>alert('Submit Failed')</script>" })
            //    RedirectToAction("ManageSM", "SELFMAILER");
            //}


            SqlCommand command1;
            command1 = new SqlCommand("UPDATE [JobAuditTrailDetail] SET STATUS='POSTING' WHERE LogTagNo=@Id AND Status='SELFMAILER'", cn1);
            command1.Parameters.AddWithValue("@Id", LogTagNo);
            command1.ExecuteNonQuery();
            cn1.Close();
        }

        using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        {
            cn1.Open();

            List<string> PSNo = new List<string>();

            SqlCommand cmd1 = new SqlCommand(@"SELECT PrintSLipNo 
                                                    FROM Hist_ProductionSlip
                                                    WHERE LogTagNo = @LogTagNo AND ProcessType = 'SELFMAILER'",cn1);
            cmd1.Parameters.AddWithValue("@LogTagNo", LogTagNo);
            SqlDataReader rm1 = cmd1.ExecuteReader();

            while (rm1.Read())
            {
                if (!rm1.IsDBNull(0))
                {
                    PSNo.Add(rm1.GetString(0));
                }

            }

            if (PSNo.Count > 0)
            {
                foreach (var PS in PSNo)
                {
                    SqlCommand command1;
                    command1 = new SqlCommand("UPDATE [InsInserting] SET InsInserting='POSTING' WHERE PrintSLipNo=@PrintSlipNo", cn1);
                    command1.Parameters.AddWithValue("@PrintSlipNo", PS);
                    command1.ExecuteNonQuery();
                }
            }

            cn1.Close();
        }

        return RedirectToAction("ManageSM", "SELFMAILER");

    }

    public List<string> getAIP(string LogTagNo)
    {
        Debug.WriteLine("LogTagNo : " + LogTagNo);
        List<string> AIP = new List<string>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        {
            cn.Open();

            SqlCommand cmdAIP = new SqlCommand("SELECT SUM(CAST(AccQty AS INT)) AS AccQty, SUM(CAST(ImpQty AS INT)) AS ImpQty, SUM(CAST(PageQty AS INT)) AS PageQty FROM JobAuditTrailDetail WHERE LogTagNo = @LogTagNo AND Status='SELFMAILER'", cn);
            cmdAIP.Parameters.AddWithValue("@LogTagNo", LogTagNo);
            SqlDataReader rmAIP = cmdAIP.ExecuteReader();

            while (rmAIP.Read())
            {
                if (!rmAIP.IsDBNull(0))
                {
                    AIP.Add(rmAIP["AccQty"].ToString());
                }
                else
                {
                    AIP.Add("0");
                }

                if (!rmAIP.IsDBNull(1))
                {
                    AIP.Add(rmAIP["ImpQty"].ToString());
                }
                else
                {
                    AIP.Add("0");

                }

                if (!rmAIP.IsDBNull(2))
                {
                    AIP.Add(rmAIP["PageQty"].ToString());
                }
                else
                {
                    AIP.Add("0");
                }
            }

            cn.Close();
        }

        int i = 0;

        foreach (var value in AIP)
        {
            if (i == 0)
            {
                Debug.WriteLine("AccQty : " + value);
            }
            else if (i == 1)
            {
                Debug.WriteLine("ImpQty : " + value);
            }
            else if (i == 2)
            {
                Debug.WriteLine("PageQty : " + value);
                i = 0;
            }

            i++;
        }
        return AIP;
    }




}



