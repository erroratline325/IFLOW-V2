﻿using System;
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
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MigraDoc.DocumentObjectModel;
using Document = MigraDoc.DocumentObjectModel.Document;
using Style = MigraDoc.DocumentObjectModel.Style;
using Paragraph = MigraDoc.DocumentObjectModel.Paragraph;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace MvcAppV2.Controllers
{
    [MvcAppV2.FilterConfig.UserSessionActionFilter]
    public class QMController : Controller
    {

        string PathSource = System.Configuration.ConfigurationManager.AppSettings["SourceFile"];
        string IpSMtp_ = System.Configuration.ConfigurationManager.AppSettings["IpSMtp"];
        string PortSmtp_ = System.Configuration.ConfigurationManager.AppSettings["PortSmtp"];
        string PathSource2 = System.Configuration.ConfigurationManager.AppSettings["logfilelocation"];

        //
        // GET: /QM/

        public ActionResult ManageQM(string Id, string ProductName, string product, string set, string Status)

        {
            var IdentityName = @Session["Fullname"];
            var Role = @Session["Role"];
            var IsDepart = @Session["Department"];
            ViewBag.Dept = Session["Department"].ToString();
            Session["Id"] = Id;
            ViewBag.Id = Id;

            if (IsDepart.ToString() == "QM")
            {
                ViewBag.IsSet = "OpenQM";
            }
            else if (IsDepart.ToString() == "IT")
            {
                ViewBag.IsSet = "OpenITO";
            }
            else
            { 

                ViewBag.IsSet = "OpenMBD";
            }


            List<JobInstruction> JobInstructionlist1 = new List<JobInstruction>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {

                int _bil = 1;
                cn.Open();
                if (set == "search")
                {
                    //command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass, JobType,JobSheetNo, StartDevDate, EndDevDate,StatQM
                    //                     FROM [IflowSeed].[dbo].[JobInstruction]                                    
                    //                     WHERE ProductName LIKE @ProductName
                    //                     AND Status = 'QME' AND JobType='E-BLAST'
                    //                     ORDER BY CreatedOn desc ";

                    command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass, JobType,JobSheetNo, StartDevDate, EndDevDate,StatQM
                                         FROM [IflowSeed].[dbo].[JobInstruction]                                    
                                         WHERE ProductName LIKE @ProductName
                                         AND Status = 'QME' 
                                         ORDER BY CreatedOn desc ";

                    command.Parameters.AddWithValue("@ProductName", "%" + product + "%");
                }

                else
                {


                    //command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass, JobType,JobSheetNo, StartDevDate, EndDevDate,StatQM
                    //                    FROM [IflowSeed].[dbo].[JobInstruction]
                    //                    WHERE Status = 'QME' AND JobType='E-BLAST'";
                    command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass, JobType,JobSheetNo, StartDevDate, EndDevDate,StatQM
                                        FROM [IflowSeed].[dbo].[JobInstruction]
                                        WHERE Status = 'QME' ";

                }

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JobInstruction model = new JobInstruction();
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
                            model.StartDevOn = reader.GetDateTime(6);
                        }

                        if (reader.IsDBNull(7) == false)
                        {
                            model.EndDevDate = reader.GetDateTime(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.StatQM = reader.GetString(8);
                        }
                        else
                        {
                            model.StatQM = "Waiting to be verified";

                        }

                        if (reader.IsDBNull(5) == false)
                        {
                            SqlCommand cmdcheck = new SqlCommand("SELECT RejectQuality, RejectMBD,RejectITO, ApprovalEngineering, PlanningApproval, ApprovalDDPOperation FROM QM WHERE JobSheetNo=@JobSheetNoCheck", cn);
                            cmdcheck.Parameters.AddWithValue("@JobSheetNoCheck", reader.GetString(5));
                            SqlDataReader rmcheck = cmdcheck.ExecuteReader();

                            while(rmcheck.Read())
                            {
                                if (rmcheck.IsDBNull(0) == false)
                                {
                                    if (rmcheck.IsDBNull(1) == false)
                                    {
                                        if (rmcheck.IsDBNull(2) == false)
                                        {
                                            if (rmcheck.IsDBNull(3) == false)
                                            {
                                                if (rmcheck.IsDBNull(4) == false)
                                                {
                                                    if (rmcheck.IsDBNull(5) == false)
                                                    {
                                                        model.QMReady = "Yes";
                                                    }
                                                    else
                                                    {
                                                        model.QMReady = "No";
                                                    }
                                                }
                                                else
                                                {
                                                    model.QMReady = "No";
                                                }
                                            }
                                            else
                                            {
                                                model.QMReady = "No";
                                            }
                                        }
                                        else
                                        {
                                            model.QMReady = "No";
                                        }
                                    }
                                    else
                                    {
                                        model.QMReady = "No";
                                    }
                                }
                                else
                                {
                                    model.QMReady = "No";
                                }
                            }
                        }



                    }
                    Debug.WriteLine("QM Ready : "+model.QMReady);
                    JobInstructionlist1.Add(model);
                }
                cn.Close();
            }

            return View(JobInstructionlist1);
        }

        public ActionResult ListHistoryNMR(string Id, string ProductName, string product, string set, string Status, string JobSheetNo)
        {
            var IdentityName = @Session["Fullname"];
            var Role = @Session["Role"];
            var IsDepart = @Session["Department"];
            ViewBag.Dept = Session["Department"].ToString();
            Session["Id"] = Id;
            ViewBag.Id = Id;

            if (IsDepart.ToString() == "QM")
            {
                ViewBag.IsSet = "OpenQM";
            }
            else if (IsDepart.ToString() == "IT")
            {
                ViewBag.IsSet = "OpenITO";
            }
            else
            {

                ViewBag.IsSet = "OpenMBD";
            }


            List<JobInstruction> JobInstructionlist1 = new List<JobInstruction>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {

                int _bil = 1;
                cn.Open();
                if (set == "search")
                {
                    //command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass, JobType,JobSheetNo, StartDevDate, EndDevDate,StatQM
                    //                     FROM [IflowSeed].[dbo].[JobInstruction]                                    
                    //                     WHERE ProductName LIKE @ProductName
                    //                     AND Status = 'QME' AND JobType='E-BLAST'
                    //                     ORDER BY CreatedOn desc ";

                    command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass, JobType,JobSheetNo, StartDevDate, EndDevDate,StatQM
                                         FROM [IflowSeed].[dbo].[JobInstruction]                                    
                                         WHERE ProductName LIKE @ProductName
                                         AND NMRStatus='COMPLETED'
                                         ORDER BY CreatedOn desc "
                    ;

                    command.Parameters.AddWithValue("@ProductName", "%" + product + "%");
                }

                else
                {


                    //command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass, JobType,JobSheetNo, StartDevDate, EndDevDate,StatQM
                    //                    FROM [IflowSeed].[dbo].[JobInstruction]
                    //                    WHERE Status = 'QME' AND JobType='E-BLAST'";
                    command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass, JobType,JobSheetNo, StartDevDate, EndDevDate,StatQM
                                        FROM [IflowSeed].[dbo].[JobInstruction] WHERE NMRStatus='COMPLETED'";
                                        

                }

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JobInstruction model = new JobInstruction();
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
                            model.StartDevOn = reader.GetDateTime(6);
                        }

                        if (reader.IsDBNull(7) == false)
                        {
                            model.EndDevDate = reader.GetDateTime(7);
                        }
                        //if (reader.IsDBNull(8) == false)
                        //{
                        //    model.StatQM = reader.GetString(8);
                        //}
                        //else
                        //{
                        //    model.StatQM = "Waiting to be verified";

                        //}

                        if (reader.IsDBNull(5) == false)
                        {
                            SqlCommand cmdcheck = new SqlCommand("SELECT RejectQuality, RejectMBD,RejectITO, ApprovalEngineering, PlanningApproval, ApprovalDDPOperation FROM QM WHERE JobSheetNo=@JobSheetNoCheck", cn);
                            cmdcheck.Parameters.AddWithValue("@JobSheetNoCheck", reader.GetString(5));
                            SqlDataReader rmcheck = cmdcheck.ExecuteReader();

                            while (rmcheck.Read())
                            {
                                if (rmcheck.IsDBNull(0) == false)
                                {
                                    if (rmcheck.IsDBNull(1) == false)
                                    {
                                        if (rmcheck.IsDBNull(2) == false)
                                        {
                                            if (rmcheck.IsDBNull(3) == false)
                                            {
                                                if (rmcheck.IsDBNull(4) == false)
                                                {
                                                    if (rmcheck.IsDBNull(5) == false)
                                                    {
                                                        model.QMReady = "Yes";
                                                    }
                                                    else
                                                    {
                                                        model.QMReady = "No";
                                                    }
                                                }
                                                else
                                                {
                                                    model.QMReady = "No";
                                                }
                                            }
                                            else
                                            {
                                                model.QMReady = "No";
                                            }
                                        }
                                        else
                                        {
                                            model.QMReady = "No";
                                        }
                                    }
                                    else
                                    {
                                        model.QMReady = "No";
                                    }
                                }
                                else
                                {
                                    model.QMReady = "No";
                                }
                            }
                        }



                    }
                    Debug.WriteLine("QM Ready : " + model.QMReady);
                    JobInstructionlist1.Add(model);
                }
                cn.Close();
            }

            return View(JobInstructionlist1);
        }


        public ActionResult HistoryNMR(string Id, string Customer_Name, string AttachEncry, string Templayout, string Corret, string CorretInfo, string Compages, string GoodQuality
           , string RejectQuality, string NameQ, string JobSheetNo, string CommentQ, string RejectITO, string NameITO, string CommentITO, string dateITO, string RejectMBD, string NameMBD, string CommentMBD, string dateMBD
           , string JobType, string RemarkAttachEncry, string CusApp, string RemarkCusApp, string RemarkTemplayout, string RemarkCorret, string RemarkCompages, string RemarkGoodQuality, string dateQ, string set, string ProductName, string RemarkCorretInfo, string set2
           , string JobInstructionId, JobInstruction get, string QMStatus, FormCollection formCollection, string PaperSizeInput, string ApprovalPaperSize, string RemarkPaperSize, string PaperWeightInput, string ApprovalPaperWeight, string RemarkPaperWeight, string PaperTypeInput
            , string ApprovalPaperType, string RemarkPaperType, string EnvelopeTypeInput, string ApprovalEnvelopeType, string RemarkEnvelopeType, string AuditTrailReportInput, string AccQty, string ImpQty, string PageQty, string ApprovalAuditTrailReport, string RemarkAuditTrailReport
            , string ApprovalOMRMarker, string RemarkOMRMarker, string ApprovalCompleteAddress, string RemarkCompleteAddress, string StatementDateReferInput, string ApprovalStatementDateRefer
            , string RemarkStatementDateRefer, string AccNoInput, string ApprovalAccNo, string RemarkAccNo, string ApprovalBarcodeComplete, string RemarkBarcodeComplete, string ApprovalJIAvailable, string RemarkJIAvailable, string ApprovalTestingResultEng, string RemarkTestingResultEng
            , string TestingResultITApproval, string TestingResultITRemark, string Comment, string CorrectiveAction, string PaperTypeInput2, string ApprovalEngineering, string ApprovalITODoc, string PlanningApproval, string ApprovalDDPOperation
            , string NameEngineering, string NameITODoc, string NamePlanning, string NameDDPOperation, string CommentEngineering, string CommentITODoc, string CommentPlanning, string CommentDDPOperation, string dateEngineering, string dateITODoc, string datePlanning, string dateDDPOperation
            , string ProcessPrinting, string ProcessInserting, string ProcessSelfMailer, string ProcessMMP)
        {
            Debug.WriteLine("Set Value : " + set);
            var IdentityName = @Session["Fullname"];
            var Role = @Session["Role"];
            ViewBag.IsDepart = @Session["Department"];
            ViewBag.FullName = @Session["Fullname"];
            ViewBag.Department = @Session["Department"].ToString();
            string dept = @Session["Department"].ToString();
            ViewBag.Role = @Session["Role"];
            string role = @Session["Role"].ToString();
            string Department = Session["Department"].ToString();

            Debug.WriteLine("Role : " + @Session["Role"]);
            Debug.WriteLine("Department : " + @Session["Department"]);


            Session["Id"] = Id;
            ViewBag.Id = Id;

            List<SelectListItem> listproduct = new List<SelectListItem>();

            listproduct.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
            listproduct.Add(new SelectListItem { Text = "DDP", Value = "DDP" });
            listproduct.Add(new SelectListItem { Text = "RMS", Value = "RMS" });
            listproduct.Add(new SelectListItem { Text = "DOW", Value = "DOW" });
            listproduct.Add(new SelectListItem { Text = "OTHERS", Value = "OTHERS" });

            ViewData["ProductType_"] = listproduct;

            int _bil5 = 1;
            List<SelectListItem> li5 = new List<SelectListItem>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();

                command.CommandText = @"SELECT DISTINCT Type FROM [IflowSeed].[dbo].[JobType]          
                                     ORDER BY Type";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JobInstruction model = new JobInstruction();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            model.JobType = reader.GetString(0);
                        }
                    }
                    int i = _bil5++;
                    if (i == 1)
                    {
                        li5.Add(new SelectListItem { Text = "Please Select" });
                        li5.Add(new SelectListItem { Text = model.JobType });

                    }
                    else
                    {
                        li5.Add(new SelectListItem { Text = model.JobType });
                    }
                }
                cn.Close();
            }
            ViewData["JobType_"] = li5;

            List<JobInstruction> JobInstructionlist1 = new List<JobInstruction>();

            string ProductName2 = "";
            string JobSheetNo2 = "";
            string JobType2 = null;
            string JobInstructionId2 = "";
            string QMStatus2 = "";
            string Customer_Name2 = "";

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass, JobType,JobSheetNo, StartDevDate, EndDevDate,StatQM
                                        FROM [IflowSeed].[dbo].[JobInstruction]
                                        WHERE Id =@Id";

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
                            Customer_Name2 = reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            ViewBag.ProductName = reader.GetString(2);
                            ProductName2 = reader.GetString(2);

                        }
                        if (reader.IsDBNull(3) == false)
                        {
                            ViewBag.JobClass = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            ViewBag.JobType = reader.GetString(4);
                            JobType2 = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            ViewBag.JobSheetNo = reader.GetString(5);
                            JobSheetNo2 = reader.GetString(5);

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
                            ViewBag.StatQM = reader.GetString(8);
                        }



                    }
                    JobInstructionlist1.Add(model);
                }
                cn.Close();

            }

            //using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            //using (SqlCommand command = new SqlCommand("", cn2))
            //{
            //    int _bil2 = 1;
            //    cn2.Open();
            //    command.CommandText = @"SELECT QMITO.RejectITO as RejectITO, QMITO.CommentITO, QMITO.dateITO, QMITO.NameITO
            //                          FROM  JobInstruction INNER JOIN
            //                         QMITO ON JobInstruction.Id = QMITO.JobInstructionId
            //                         where   JobInstruction.Id=@Id";

            //    command.Parameters.AddWithValue("@Id", Id);
            //    var reader = command.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        QM_Model model = new QM_Model();
            //        {
            //            model.Bil = _bil2++;
            //            if (reader.IsDBNull(0) == false)
            //            {
            //                 Boolean ITORejectStatus= reader.GetBoolean(0);

            //                if (ITORejectStatus == false)
            //                {
            //                    ViewBag.RejectITO = "";
            //                }
            //                else
            //                {
            //                    ViewBag.RejectITO = "checked";
            //                }
            //                //ViewBag.RejectITO = reader["RejectITO"].ToString();

            //            }
            //            if (reader.IsDBNull(1) == false)
            //            {
            //                ViewBag.CommentITO = reader.GetString(1);
            //            }
            //            if (reader.IsDBNull(2) == false)
            //            {
            //                ViewBag.dateITO = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(2));
            //            }

            //            if (reader.IsDBNull(3) == false)
            //            {
            //                ViewBag.NameITO = reader.GetString(3);
            //            }
            //        }
            //    }
            //}

            List<QM_Model> QM_Model = new List<QM_Model>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT JobInstruction.Id,  QM.Customer_Name, QM.ProductName, QM.JobSheetNo, QM.JobType, QM.AttachEncry, QM.RemarkAttachEncry, QM.CusApp, QM.RemarkCusApp, QM.Templayout, QM.RemarkTemplayout, QM.Corret, QM.RemarkCorret, QM.CorretInfo, QM.RemarkCorretInfo, QM.Compages, QM.RemarkCompages, QM.GoodQuality, QM.RemarkGoodQuality, QM.RejectQuality, QM.NameQ, QM.CommentQ, QM.RejectITO, QM.NameITO, QM.CommentITO, QM.RejectMBD, QM.NameMBD, QM.CommentMBD, QM.dateQ, QM.dateITO,
                                            QM.dateMBD,QM.QMStatus,QM.Id
                                  FROM  JobInstruction INNER JOIN
                                  QM ON JobInstruction.Id = QM.JobInstructionId
                                     where   JobInstruction.Id=@Id";

                command.Parameters.AddWithValue("@Id", Id);


                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    QM_Model model = new QM_Model();
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
                            ViewBag.JobSheetNo = reader.GetString(3);
                        }


                        if (reader.IsDBNull(4) == false)
                        {
                            ViewBag.JobType = reader.GetString(4);
                        }


                    }
                    QM_Model.Add(model);
                }

                reader.Close();

                SqlCommand cmd = new SqlCommand("select PaperSizeInput,ApprovalPaperSize,RemarkPaperSize,PaperWeightInput,ApprovalPaperWeight,RemarkPaperWeight,PaperTypeInput,ApprovalPaperType,RemarkPaperType,EnvelopeTypeInput," +
                    "ApprovalEnvelopeType,RemarkEnvelopeType,AuditTrailReportInput,AccQty,ImpQty,PageQty,ApprovalAuditTrailReport,RemarkAuditTrailReport,CusApp,RemarkCusApp,Templayout,RemarkTemplayout,ApprovalOMRMarker,RemarkOMRMarker," +
                    "ApprovalCompleteAddress,RemarkCompleteAddress,StatementDateReferInput,ApprovalStatementDateRefer,RemarkStatementDateRefer,AccNoInput,ApprovalAccNo,RemarkAccNo,ApprovalBarcodeComplete,RemarkBarcodeComplete," +
                    "ApprovalJIAvailable,RemarkJIAvailable,ApprovalTestingResultEng,RemarkTestingResultEng,TestingResultITApproval,TestingResultITRemark,Comment,CorrectiveAction,PaperTypeInput2 " +
                    "FROM QM WHERE JobInstructionId=@JobInstructionId;", cn);

                cmd.Parameters.AddWithValue("@JobInstructionId", Id);
                SqlDataReader rm = cmd.ExecuteReader();

                while (rm.Read())
                {
                    if (!rm.IsDBNull(0))
                    {
                        ViewBag.PaperSizeInput = rm.GetString(0);
                    }

                    if (!rm.IsDBNull(1))
                    {
                        ViewBag.ApprovalPaperSize = "Checked";
                        //ViewBag.ApprovalPaperSize = rm.GetBoolean(1);
                    }

                    if (!rm.IsDBNull(2))
                    {
                        ViewBag.RemarkPaperSize = rm.GetString(2);
                    }

                    if (!rm.IsDBNull(3))
                    {
                        ViewBag.PaperWeightInput = rm.GetString(3);
                    }

                    if (!rm.IsDBNull(4))
                    {
                        ViewBag.ApprovalPaperWeight = "Checked";
                    }

                    if (!rm.IsDBNull(5))
                    {
                        ViewBag.RemarkPaperWeight = rm.GetString(5);
                    }

                    if (!rm.IsDBNull(6))
                    {
                        ViewBag.PaperTypeInput = rm.GetString(6);
                    }

                    if (!rm.IsDBNull(7))
                    {
                        ViewBag.ApprovalPaperType = "Checked";
                    }

                    if (!rm.IsDBNull(8))
                    {
                        ViewBag.RemarkPaperType = rm.GetString(8);
                    }

                    if (!rm.IsDBNull(9))
                    {
                        ViewBag.EnvelopeTypeInput = rm.GetString(9);
                    }

                    if (!rm.IsDBNull(10))
                    {
                        ViewBag.ApprovalEnvelopeType = "Checked";
                    }

                    if (!rm.IsDBNull(11))
                    {
                        ViewBag.RemarkEnvelopeType = rm.GetString(11);
                    }

                    if (!rm.IsDBNull(12))
                    {
                        ViewBag.AuditTrailReportInput = rm.GetString(12);
                    }

                    if (!rm.IsDBNull(13))
                    {
                        ViewBag.AccQty = rm.GetString(13);
                    }

                    if (!rm.IsDBNull(14))
                    {
                        ViewBag.ImpQty = rm.GetString(14);
                    }

                    if (!rm.IsDBNull(15))
                    {
                        ViewBag.PageQty = rm.GetString(15);
                    }

                    if (!rm.IsDBNull(16))
                    {
                        ViewBag.ApprovalAuditTrailReport = "Checked";
                    }

                    if (!rm.IsDBNull(17))
                    {
                        ViewBag.RemarkAuditTrailReport = rm.GetString(17);
                    }

                    if (!rm.IsDBNull(18))
                    {
                        ViewBag.CusApp = "Checked";
                    }

                    if (!rm.IsDBNull(19))
                    {
                        ViewBag.RemarkCusApp = rm.GetString(19);
                    }

                    if (!rm.IsDBNull(20))
                    {
                        ViewBag.Templayout = "Checked";
                    }

                    if (!rm.IsDBNull(21))
                    {
                        ViewBag.RemarkTemplayout = rm.GetString(21);
                    }

                    if (!rm.IsDBNull(22))
                    {
                        ViewBag.ApprovalOMRMarker = "Checked";
                    }

                    if (!rm.IsDBNull(23))
                    {
                        ViewBag.RemarkOMRMarker = rm.GetString(23);
                    }

                    if (!rm.IsDBNull(24))
                    {
                        ViewBag.ApprovalCompleteAddress = "Checked";
                    }

                    if (!rm.IsDBNull(25))
                    {
                        ViewBag.RemarkCompleteAddress = rm.GetString(25);
                    }

                    if (!rm.IsDBNull(26))
                    {
                        ViewBag.StatementDateReferInput = rm.GetString(26);
                    }

                    if (!rm.IsDBNull(27))
                    {
                        ViewBag.ApprovalStatementDateRefer = "Checked";
                    }

                    if (!rm.IsDBNull(28))
                    {
                        ViewBag.RemarkStatementDateRefer = rm.GetString(28);
                    }

                    if (!rm.IsDBNull(29))
                    {
                        ViewBag.AccNoInput = rm.GetString(29);
                    }

                    if (!rm.IsDBNull(30))
                    {
                        ViewBag.ApprovalAccNo = "Checked";
                    }

                    if (!rm.IsDBNull(31))
                    {
                        ViewBag.RemarkAccNo = rm.GetString(31);
                    }

                    if (!rm.IsDBNull(32))
                    {
                        ViewBag.ApprovalBarcodeComplete = "Checked";
                    }

                    if (!rm.IsDBNull(33))
                    {
                        ViewBag.RemarkBarcodeComplete = rm.GetString(33);
                    }

                    if (!rm.IsDBNull(34))
                    {
                        ViewBag.ApprovalJIAvailable = "Checked";
                    }

                    if (!rm.IsDBNull(35))
                    {
                        ViewBag.RemarkJIAvailable = rm.GetString(35);
                    }

                    if (!rm.IsDBNull(36))
                    {
                        ViewBag.ApprovalTestingResultEng = "Checked";
                    }

                    if (!rm.IsDBNull(37))
                    {
                        ViewBag.RemarkTestingResultEng = rm.GetString(37);
                    }

                    if (!rm.IsDBNull(38))
                    {
                        ViewBag.TestingResultITApproval = "Checked";
                    }

                    if (!rm.IsDBNull(39))
                    {
                        ViewBag.TestingResultITRemark = rm.GetString(39);
                    }

                    if (!rm.IsDBNull(40))
                    {
                        ViewBag.Comment = rm.GetString(40);
                    }

                    if (!rm.IsDBNull(41))
                    {
                        ViewBag.CorrectiveAction = rm.GetString(41);
                    }

                    if (!rm.IsDBNull(42))
                    {
                        ViewBag.PaperTypeInput2 = rm.GetString(42);
                    }


                }

                rm.Close();

                SqlCommand cmd2 = new SqlCommand("select RejectMBD,RejectQuality,RejectITO,ApprovalEngineering,ApprovalITODoc,PlanningApproval,ApprovalDDPOperation,NameMBD,NameITO,NameQ,NameEngineering,NameITODoc," +
                    "NamePlanning,DDPOperationName,CommentMBD,CommentITO,CommentQ,CommentEngineering,CommentITODoc,CommentPlanning,CommentDDPOperation, ProcessPrinting, ProcessInserting, ProcessSelfMailer, ProcessMMP from QM WHERE JobInstructionId=@JobInstructionId;", cn);
                cmd2.Parameters.AddWithValue("@JobInstructionId", Id);
                SqlDataReader rm2 = cmd2.ExecuteReader();

                while (rm2.Read())
                {
                    if (!rm2.IsDBNull(0))
                    {
                        ViewBag.RejectMBD = "Checked";
                    }
                    else
                    {
                        if (dept != "MBD")
                        {
                            ViewBag.RejectMBD = "disabled";
                        }

                    }

                    if (!rm2.IsDBNull(1))
                    {
                        ViewBag.RejectQuality = "Checked";
                    }
                    else
                    {
                        if (dept != "QM")
                        {
                            ViewBag.RejectQuality = "disabled";
                        }
                    }

                    if (!rm2.IsDBNull(2))
                    {
                        ViewBag.RejectITO = "Checked";
                    }
                    else
                    {
                        if (dept != "IT")
                        {
                            ViewBag.RejectITO = "disabled";
                        }

                    }

                    if (!rm2.IsDBNull(3))
                    {
                        ViewBag.ApprovalEngineering = "Checked";
                    }
                    else
                    {
                        if (dept != "PRODUCTION" && role != "Engineering")
                        {
                            ViewBag.ApprovalEngineering = "disabled";
                        }


                    }

                    if (!rm2.IsDBNull(4))
                    {
                        ViewBag.ApprovalITODoc = "Checked";
                    }
                    else
                    {

                        if (dept != "IT")
                        {
                            ViewBag.ApprovalITODoc = "disabled";
                        }
                    }

                    if (!rm2.IsDBNull(5))
                    {
                        ViewBag.PlanningApproval = "Checked";
                    }
                    else
                    {
                        if (dept != "PRODUCTION" && role != "Planner")
                        {
                            ViewBag.PlanningApproval = "disabled";

                        }

                    }

                    if (!rm2.IsDBNull(6))
                    {
                        ViewBag.ApprovalDDPOperation = "Checked";
                    }
                    else
                    {
                        if (dept != "PRODUCTION" && role != "Super Admin")
                        {
                            ViewBag.ApprovalDDPOperation = "disabled";
                        }

                    }

                    if (!rm2.IsDBNull(7))
                    {
                        ViewBag.NameMBD = rm2.GetString(7);
                    }


                    if (!rm2.IsDBNull(8))
                    {
                        ViewBag.NameITO = rm2.GetString(8);
                    }

                    if (!rm2.IsDBNull(9))
                    {
                        ViewBag.NameQ = rm2.GetString(9);
                    }

                    if (!rm2.IsDBNull(10))
                    {
                        ViewBag.NameEngineering = rm2.GetString(10);
                    }

                    if (!rm2.IsDBNull(11))
                    {
                        ViewBag.NameITODoc = rm2.GetString(11);
                    }

                    if (!rm2.IsDBNull(12))
                    {
                        ViewBag.NamePlanning = rm2.GetString(12);
                    }

                    if (!rm2.IsDBNull(13))
                    {
                        ViewBag.NameDDPOperation = rm2.GetString(13);
                    }

                    if (!rm2.IsDBNull(14))
                    {
                        ViewBag.CommentMBD = rm2.GetString(14);
                    }

                    if (!rm2.IsDBNull(15))
                    {
                        ViewBag.CommentITO = rm2.GetString(15);
                    }

                    if (!rm2.IsDBNull(16))
                    {
                        ViewBag.CommentQ = rm2.GetString(16);
                    }

                    if (!rm2.IsDBNull(17))
                    {
                        ViewBag.CommentEngineering = rm2.GetString(17);
                    }

                    if (!rm2.IsDBNull(18))
                    {
                        ViewBag.CommentITODoc = rm2.GetString(18);
                    }

                    if (!rm2.IsDBNull(19))
                    {
                        ViewBag.CommentPlanning = rm2.GetString(19);
                    }

                    if (!rm2.IsDBNull(20))
                    {
                        ViewBag.CommentDDPOperation = rm2.GetString(20);
                    }

                    if (!rm2.IsDBNull(21))
                    {
                        ViewBag.ProcessPrinting = "Checked";
                    }
                    else
                    {
                        ViewBag.ProcessPrinting = "disabled";
                    }

                    if (!rm2.IsDBNull(22))
                    {
                        ViewBag.ProcessInserting = "Checked";
                    }
                    else
                    {
                        ViewBag.ProcessInserting = "disabled";

                    }

                    if (!rm2.IsDBNull(23))
                    {
                        ViewBag.ProcessSelfMailer = "Checked";
                    }
                    else
                    {
                        ViewBag.ProcessSelfMailer = "disabled";
                    }

                    if (!rm2.IsDBNull(24))
                    {
                        ViewBag.ProcessMMP = "Checked";
                    }
                    else
                    {
                        ViewBag.ProcessMMP = "disabled";

                    }

                }

                rm2.Close();

                SqlCommand cmd3 = new SqlCommand("SELECT dateEngineering,dateITODoc,datePlanning,dateDDPOperation,dateMBD,dateITO,dateQ FROM QM WHERE JobInstructionId=@JobInstructionIdcmd3", cn);

                cmd3.Parameters.AddWithValue("@JobInstructionIdcmd3", Id);

                SqlDataReader rm3 = cmd3.ExecuteReader();

                while (rm3.Read())
                {
                    if (!rm3.IsDBNull(0))
                    {
                        ViewBag.dateEngineering = rm3.GetString(0);
                    }
                    else
                    {
                        ViewBag.dateEngineering = null;
                    }

                    if (!rm3.IsDBNull(1))
                    {
                        ViewBag.dateITODoc = rm3.GetString(1);
                    }
                    else
                    {
                        ViewBag.dateITODoc = null;
                    }

                    if (!rm3.IsDBNull(2))
                    {
                        ViewBag.datePlanning = rm3.GetString(2);
                    }
                    else
                    {
                        ViewBag.datePlanning = null;
                    }

                    if (!rm3.IsDBNull(3))
                    {
                        ViewBag.dateDDPOperation = rm3.GetString(3);
                    }
                    else
                    {
                        ViewBag.dateDDPOperation = null;
                    }

                    if (!rm3.IsDBNull(4))
                    {
                        ViewBag.dateMBD = rm3.GetString(4);
                    }
                    else
                    {
                        ViewBag.dateMBD = null;
                    }

                    if (!rm3.IsDBNull(5))
                    {
                        ViewBag.dateITO = rm3.GetString(5);
                    }
                    else
                    {
                        ViewBag.dateITO = null;
                    }

                    if (!rm3.IsDBNull(6))
                    {
                        ViewBag.dateQ = rm3.GetString(6);
                    }
                    else
                    {
                        ViewBag.dateQ = null;
                    }

                }

                rm3.Close();

                SqlCommand cmdlogtagno = new SqlCommand("SELECT LogTagNo FROM JobAuditTrailDetail WHERE JobSheetNo=@JobSheetNocmdlogtagno", cn);

                cmdlogtagno.Parameters.AddWithValue("@JobSheetNocmdlogtagno", JobSheetNo2);

                SqlDataReader rmlogtagno = cmdlogtagno.ExecuteReader();

                int bil = 1;

                while (rmlogtagno.Read())
                {
                    //var model = new JobAuditTrailDetail();
                    //{
                    //    model.Bil = bil;
                    //    model.LogTagNo = rmlogtagno.GetString(0);
                    //}
                    //bil++;
                    //logtagno.Add(model);

                    ViewBag.LogTagNo = rmlogtagno.GetString(0);
                }


                cn.Close();


            }

            return View(logtagno);

        }

        public ActionResult AddeChannel(string Id, string Customer_Name, string AttachEncry, string Templayout, string Corret, string CorretInfo, string Compages, string GoodQuality
            , string RejectQuality, string NameQ, string JobSheetNo, string CommentQ, string RejectITO, string NameITO, string CommentITO, string dateITO, string RejectMBD, string NameMBD, string CommentMBD, string dateMBD
            , string JobType, string RemarkAttachEncry, string CusApp, string RemarkCusApp, string RemarkTemplayout, string RemarkCorret, string RemarkCompages, string RemarkGoodQuality, string dateQ, string set, string ProductName, string RemarkCorretInfo, string set2
            , string JobInstructionId, JobInstruction get, string QMStatus)
        {

            var IdentityName = @Session["Fullname"];
            var Role = @Session["Role"];
            ViewBag.IsDepart = @Session["Department"];


            Session["Id"] = Id;
            ViewBag.Id = Id;

            



            List<SelectListItem> listproduct = new List<SelectListItem>();

            listproduct.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
            listproduct.Add(new SelectListItem { Text = "DDP", Value = "DDP" });
            listproduct.Add(new SelectListItem { Text = "RMS", Value = "RMS" });
            listproduct.Add(new SelectListItem { Text = "DOW", Value = "DOW" });
            listproduct.Add(new SelectListItem { Text = "OTHERS", Value = "OTHERS" });

            ViewData["ProductType_"] = listproduct;


            int _bil5 = 1;
            List<SelectListItem> li5 = new List<SelectListItem>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT DISTINCT Type FROM [IflowSeed].[dbo].[JobType]          
                                     ORDER BY Type";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JobInstruction model = new JobInstruction();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            model.JobType = reader.GetString(0);
                        }
                    }
                    int i = _bil5++;
                    if (i == 1)
                    {
                        li5.Add(new SelectListItem { Text = "Please Select" });
                        li5.Add(new SelectListItem { Text = model.JobType });

                    }
                    else
                    {
                        li5.Add(new SelectListItem { Text = model.JobType });
                    }
                }
                cn.Close();
            }
            ViewData["JobType_"] = li5;




            List<JobInstruction> JobInstructionlist1 = new List<JobInstruction>(); ;
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass, JobType,JobSheetNo, StartDevDate, EndDevDate
                                        FROM [IflowSeed].[dbo].[JobInstruction]
                                        WHERE Id =@Id";

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


                    }
                    JobInstructionlist1.Add(model);
                }
                cn.Close();

            }

            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn2))
            {
                int _bil2 = 1;
                cn2.Open();
                command.CommandText = @"SELECT QMITO.RejectITO, QMITO.CommentITO, QMITO.dateITO, QMITO.NameITO
                                  FROM  JobInstruction INNER JOIN
                                 QMITO ON JobInstruction.Id = QMITO.JobInstructionId
                                 where   JobInstruction.Id=@Id";

                command.Parameters.AddWithValue("@Id", Id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    QM_Model model = new QM_Model();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            ViewBag.RejectITO = reader.GetBoolean(0);
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            ViewBag.CommentITO = reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            ViewBag.dateITO = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(2));
                        }

                        if (reader.IsDBNull(3) == false)
                        {
                            ViewBag.NameITO = reader.GetString(3);
                        }
                    }
                }
            }




            List<QM_Model> QM_Model = new List<QM_Model>(); ;
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT JobInstruction.Id,  QM.Customer_Name, QM.ProductName, QM.JobSheetNo, QM.JobType, QM.AttachEncry, QM.RemarkAttachEncry, QM.CusApp, QM.RemarkCusApp, QM.Templayout, QM.RemarkTemplayout, QM.Corret, QM.RemarkCorret, QM.CorretInfo, QM.RemarkCorretInfo, QM.Compages, QM.RemarkCompages, QM.GoodQuality, QM.RemarkGoodQuality, QM.RejectQuality, QM.NameQ, QM.CommentQ, QM.RejectITO, QM.NameITO, QM.CommentITO, QM.RejectMBD, QM.NameMBD, QM.CommentMBD, QM.dateQ, QM.dateITO,
                                            QM.dateMBD,QM.QMStatus,QM.Id
                                  FROM  JobInstruction INNER JOIN
                                  QM ON JobInstruction.Id = QM.JobInstructionId
                                     where   JobInstruction.Id=@Id";

                command.Parameters.AddWithValue("@Id", Id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    QM_Model model = new QM_Model();
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
                            ViewBag.JobSheetNo = reader.GetString(3);
                        }


                        if (reader.IsDBNull(4) == false)
                        {
                            ViewBag.JobType = reader.GetString(4);
                        }


                        if (reader.IsDBNull(5) == false)
                        {
                            bool getAttachEncry = reader.GetBoolean(5);
                            if (getAttachEncry == false)
                            {
                                ViewBag.AttachEncry = reader.GetBoolean(5);
                            }
                            else
                            {
                                ViewBag.AttachEncry = "checked";
                            }
                        }

                        if (reader.IsDBNull(6) == false)
                        {
                            ViewBag.RemarkAttachEncry = reader.GetString(6);
                        }


                        if (reader.IsDBNull(7) == false)
                        {
                            bool getCusApp = reader.GetBoolean(7);
                            if (getCusApp == false)
                            {
                                ViewBag.CusApp = "";
                            }
                            else
                            {
                                ViewBag.CusApp = "checked";
                            }
                        }

                        if (reader.IsDBNull(8) == false)
                        {
                            ViewBag.RemarkCusApp = reader.GetString(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            bool getTemplayout = reader.GetBoolean(9);
                            if (getTemplayout == false)
                            {
                                ViewBag.Templayout = "";
                            }
                            else
                            {
                                ViewBag.Templayout = "checked";
                            }
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            ViewBag.RemarkTemplayout = reader.GetString(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            bool getCorret = reader.GetBoolean(11);
                            if (getCorret == false)
                            {
                                ViewBag.Corret = "";
                            }
                            else
                            {
                                ViewBag.Corret = "checked";
                            }
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            ViewBag.RemarkCorret = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            bool getCorretInfo = reader.GetBoolean(13);
                            if (getCorretInfo == false)
                            {
                                ViewBag.CorretInfo = "";
                            }
                            else
                            {
                                ViewBag.CorretInfo = "checked";
                            }
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            ViewBag.RemarkCorretInfo = reader.GetString(14);
                        }

                        if (reader.IsDBNull(15) == false)
                        {
                            bool getCompages = reader.GetBoolean(15);
                            if (getCompages == false)
                            {
                                ViewBag.Compages = "";
                            }
                            else
                            {
                                ViewBag.Compages = "checked";
                            }
                        }
                        if (reader.IsDBNull(16) == false)
                        {
                            ViewBag.RemarkCompages = reader.GetString(16);
                        }

                        if (reader.IsDBNull(17) == false)
                        {
                            bool getGoodQuality = reader.GetBoolean(17);
                            if (getGoodQuality == false)
                            {
                                ViewBag.GoodQuality = "";
                            }
                            else
                            {
                                ViewBag.GoodQuality = "checked";
                            }
                        }
                        if (reader.IsDBNull(18) == false)
                        {
                            ViewBag.RemarkGoodQuality = reader.GetString(18);
                        }
                        if (reader.IsDBNull(19) == false)
                        {
                            bool getRejectQuality = reader.GetBoolean(19);
                            if (getRejectQuality == false)
                            {
                                ViewBag.RejectQuality = "";
                            }
                            else
                            {
                                ViewBag.RejectQuality = "checked";
                            }
                        }
                        if (reader.IsDBNull(20) == false)
                        {
                            ViewBag.NameQ = reader.GetString(20);
                        }
                        if (reader.IsDBNull(21) == false)
                        {
                            ViewBag.CommentQ = reader.GetString(21);
                        }

                        if (reader.IsDBNull(22) == false)
                        {
                            bool getRejectITO = reader.GetBoolean(22);
                            if (getRejectITO == false)
                            {
                                ViewBag.RejectITO = "";
                            }
                            else
                            {
                                ViewBag.RejectITO = "checked";
                            }
                        }
                        if (reader.IsDBNull(23) == false)
                        {
                            ViewBag.NameITO = reader.GetString(23);
                        }
                        if (reader.IsDBNull(24) == false)
                        {
                            ViewBag.CommentITO = reader.GetString(24);
                        }
                        if (reader.IsDBNull(25) == false)
                        {
                            bool getRejectMBD = reader.GetBoolean(25);
                            if (getRejectMBD == false)
                            {
                                ViewBag.RejectMBD = "";
                            }
                            else
                            {
                                ViewBag.RejectMBD = "checked";
                            }
                        }
                        if (reader.IsDBNull(26) == false)
                        {
                            ViewBag.NameMBD = reader.GetString(26);
                        }
                        if (reader.IsDBNull(27) == false)
                        {
                            ViewBag.CommentMBD = reader.GetString(27);
                        }
                        if (reader.IsDBNull(28) == false)
                        {
                            ViewBag.dateQ = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(28));
                            //model.DateQTxt = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(28));
                        }

                        if (reader.IsDBNull(29) == false)
                        {
                            ViewBag.dateITO = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(29));
                        }

                        if (reader.IsDBNull(30) == false)
                        {
                            ViewBag.dateMBD = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(30));
                        }
                        if (reader.IsDBNull(31) == false)
                        {
                            ViewBag.QMStatus = reader.GetString(31);
                        }
                    }
                    QM_Model.Add(model);
                }
                cn.Close();
            }


            if (set == "AddNew")
            {
                Session["Id"] = Id;
                ViewBag.Id = Id;
                ViewBag.Customer_Name = Customer_Name;
                ViewBag.ProductName = ProductName;

                //get.DateQ = Convert.ToDateTime(get.DateQtxt);
                //get.DateITO = Convert.ToDateTime(get.DateITOtxt);
                //get.DateMBD = Convert.ToDateTime(get.DateMBDtxt);




                string DateQ = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt ");
                string DateITO = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt ");
                string DateMBD = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt ");


                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    Guid guidId = Guid.NewGuid();
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                    cn2.Open();
                    SqlCommand command;
                    command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[QM] ([Id],[CreateOn],[Customer_Name],[ProductName],[JobSheetNo],[JobType],[AttachEncry],[RemarkAttachEncry] ,[CusApp] ,[RemarkCusApp],[Templayout],[RemarkTemplayout] ,[Corret],[RemarkCorret],[CorretInfo],[RemarkCorretInfo],[Compages],[RemarkCompages] ,[GoodQuality] ,[RemarkGoodQuality],[RejectQuality],[NameQ],[CommentQ],[dateQ],[JobInstructionId],[QMStatus])" +
                                             "VALUES (@Id,@CreateOn,@Customer_Name,@ProductName,@JobSheetNo,@JobType,@AttachEncry,@RemarkAttachEncry,@CusApp,@RemarkCusApp,@Templayout,@RemarkTemplayout,@Corret,@RemarkCorret,@CorretInfo,@RemarkCorretInfo,@Compages,@RemarkCompages,@GoodQuality,@RemarkGoodQuality,@RejectQuality,@NameQ,@CommentQ,@dateQ,@JobInstructionId,@QMStatus)", cn2);
                    command.Parameters.AddWithValue("@Id", guidId);
                    command.Parameters.AddWithValue("@CreateOn", createdOn);
                    command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                    command.Parameters.AddWithValue("@ProductName", ProductName);
                    command.Parameters.AddWithValue("@JobSheetNo", JobSheetNo);
                    command.Parameters.AddWithValue("@JobType", JobType);
                    if (!string.IsNullOrEmpty(AttachEncry))
                    {
                        command.Parameters.AddWithValue("@AttachEncry", AttachEncry);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@AttachEncry", DBNull.Value);
                    }


                    if (!string.IsNullOrEmpty(RemarkAttachEncry))
                    {
                        command.Parameters.AddWithValue("@RemarkAttachEncry", RemarkAttachEncry);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RemarkAttachEncry", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(RemarkCusApp))
                    {
                        command.Parameters.AddWithValue("@CusApp", CusApp);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@CusApp", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(RemarkCusApp))
                    {
                        command.Parameters.AddWithValue("@RemarkCusApp", RemarkCusApp);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RemarkCusApp", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(Templayout))
                    {
                        command.Parameters.AddWithValue("@Templayout", Templayout);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Templayout", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(RemarkTemplayout))
                    {
                        command.Parameters.AddWithValue("@RemarkTemplayout", RemarkTemplayout);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RemarkTemplayout", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(Corret))
                    {
                        command.Parameters.AddWithValue("@Corret", Corret);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Corret", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(RemarkCorret))
                    {
                        command.Parameters.AddWithValue("@RemarkCorret", RemarkCorret);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RemarkCorret", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(CorretInfo))
                    {
                        command.Parameters.AddWithValue("@CorretInfo", CorretInfo);

                    }
                    else
                    {
                        command.Parameters.AddWithValue("@CorretInfo", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(RemarkCorretInfo))
                    {

                        command.Parameters.AddWithValue("@RemarkCorretInfo", RemarkCorretInfo);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RemarkCorretInfo", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(Compages))
                    {
                        command.Parameters.AddWithValue("@Compages", Compages);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Compages", DBNull.Value);
                    }


                    if (!string.IsNullOrEmpty(RemarkCompages))
                    {
                        command.Parameters.AddWithValue("@RemarkCompages", RemarkCompages);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RemarkCompages", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(GoodQuality))
                    {
                        command.Parameters.AddWithValue("@GoodQuality", GoodQuality);

                    }
                    else
                    {
                        command.Parameters.AddWithValue("@GoodQuality", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(RemarkGoodQuality))
                    {
                        command.Parameters.AddWithValue("@RemarkGoodQuality", RemarkGoodQuality);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RemarkGoodQuality", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(RejectQuality))
                    {
                        command.Parameters.AddWithValue("@RejectQuality", RejectQuality);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RejectQuality", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(NameQ))
                    {
                        command.Parameters.AddWithValue("@NameQ", NameQ);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@NameQ", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(NameQ))
                    {
                        command.Parameters.AddWithValue("@CommentQ", CommentQ);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@CommentQ", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(DateQ))
                    {


                        command.Parameters.AddWithValue("@dateQ", DateQ);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@dateQ", DBNull.Value);
                    }

                   

                    command.Parameters.AddWithValue("@JobInstructionId", Id);
                    command.Parameters.AddWithValue("@QMStatus", "Progress");
                    command.ExecuteNonQuery();
                    cn2.Close();
                }
                using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                    cn3.Open();
                    SqlCommand command2;
                    command2 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction]  SET ModifiedOn=@ModifiedOn,StatQM=@StatQM WHERE Id=@Id", cn3);

                    command2.Parameters.AddWithValue("@ModifiedOn", createdOn);
                    command2.Parameters.AddWithValue("@Id", get.Id);
                    command2.Parameters.AddWithValue("@StatQM", "Verified");
                    command2.ExecuteNonQuery();
                    cn3.Close();



                }



                return RedirectToAction("ManageQM", "QM");

            }



            if (set == "Verify")
            {
                Session["Id"] = Id;
                ViewBag.Id = Id;
                ViewBag.Customer_Name = Customer_Name;
                ViewBag.ProductName = ProductName;


                //string DateQ2 = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                //string DateITO2 = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                //string DateMBD2 = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");



                using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn1.Open();
                    SqlCommand command1;
                    command1 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET ModifiedOn=@ModifiedOn WHERE Id=@Id", cn1);
                    command1.Parameters.AddWithValue("@Id", Id);
                    command1.Parameters.AddWithValue("@ModifiedOn", createdOn);
                    command1.ExecuteNonQuery();
                    cn1.Close();
                }



                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {

                    cn2.Open();
                    SqlCommand command;
                    command = new SqlCommand("UPDATE [IflowSeed].[dbo].[QM]  SET ModifiedOn=@ModifiedOn,QMStatus=@QMStatus,RejectITO=@RejectITO,NameITO=@NameITO,CommentITO=@CommentITO,dateITO=@dateITO WHERE Id=@JobInstructionId", cn2);
                    command.Parameters.AddWithValue("@ModifiedOn", createdOn);
                    command.Parameters.AddWithValue("@JobInstructionId", get.Id);
                    command.Parameters.AddWithValue("@QMStatus", "Progress");
                    command.Parameters.AddWithValue("@RejectITO", RejectITO);
                    command.Parameters.AddWithValue("@NameITO", NameITO);
                    command.Parameters.AddWithValue("@CommentITO", CommentITO);
                    command.Parameters.AddWithValue("@dateITO", dateITO);


                    command.ExecuteNonQuery();
                    cn2.Close();

                    return RedirectToAction("ManageQM", "QM");

                }
            }


            return View();

        }


        List<JobAuditTrailDetail> logtagno=new List<JobAuditTrailDetail>();
        public ActionResult AddeChannel2(string Id, string Customer_Name, string AttachEncry, string Templayout, string Corret, string CorretInfo, string Compages, string GoodQuality
           , string RejectQuality, string NameQ, string JobSheetNo, string CommentQ, string RejectITO, string NameITO, string CommentITO, string dateITO, string RejectMBD, string NameMBD, string CommentMBD, string dateMBD
           , string JobType, string RemarkAttachEncry, string CusApp, string RemarkCusApp, string RemarkTemplayout, string RemarkCorret, string RemarkCompages, string RemarkGoodQuality, string dateQ, string set, string ProductName, string RemarkCorretInfo, string set2
           , string JobInstructionId, JobInstruction get, string QMStatus, FormCollection formCollection, string PaperSizeInput, string ApprovalPaperSize, string RemarkPaperSize, string PaperWeightInput, string ApprovalPaperWeight, string RemarkPaperWeight, string PaperTypeInput
            , string ApprovalPaperType, string RemarkPaperType, string EnvelopeTypeInput, string ApprovalEnvelopeType, string RemarkEnvelopeType, string AuditTrailReportInput, string AccQty, string ImpQty, string PageQty, string ApprovalAuditTrailReport, string RemarkAuditTrailReport
            ,string ApprovalOMRMarker, string RemarkOMRMarker, string ApprovalCompleteAddress, string RemarkCompleteAddress, string StatementDateReferInput, string ApprovalStatementDateRefer
            , string RemarkStatementDateRefer, string AccNoInput, string ApprovalAccNo, string RemarkAccNo, string ApprovalBarcodeComplete, string RemarkBarcodeComplete, string ApprovalJIAvailable, string RemarkJIAvailable, string ApprovalTestingResultEng, string RemarkTestingResultEng
            , string TestingResultITApproval, string TestingResultITRemark, string Comment, string CorrectiveAction,string PaperTypeInput2, string ApprovalEngineering, string ApprovalITODoc, string PlanningApproval, string ApprovalDDPOperation
            , string NameEngineering, string NameITODoc, string NamePlanning, string NameDDPOperation, string CommentEngineering, string CommentITODoc, string CommentPlanning, string CommentDDPOperation, string dateEngineering, string dateITODoc, string datePlanning, string dateDDPOperation
            ,string ProcessPrinting, string ProcessInserting, string ProcessSelfMailer, string ProcessMMP,string CommentEngineer)
        {
            Debug.WriteLine("Set Value : " + set);
            var IdentityName = @Session["Fullname"];
            var Role = @Session["Role"];
            ViewBag.IsDepart = @Session["Department"];
            ViewBag.FullName= @Session["Fullname"];
            ViewBag.Department= @Session["Department"].ToString();
            string dept = @Session["Department"].ToString();
            ViewBag.Role = @Session["Role"];
            string role = @Session["Role"].ToString();
            string Department = Session["Department"].ToString();

            Debug.WriteLine("Role : " + @Session["Role"]);
            Debug.WriteLine("Department : " + @Session["Department"]);


            Session["Id"] = Id;
            ViewBag.Id = Id;

            List<SelectListItem> listproduct = new List<SelectListItem>();

            listproduct.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
            listproduct.Add(new SelectListItem { Text = "DDP", Value = "DDP" });
            listproduct.Add(new SelectListItem { Text = "RMS", Value = "RMS" });
            listproduct.Add(new SelectListItem { Text = "DOW", Value = "DOW" });
            listproduct.Add(new SelectListItem { Text = "OTHERS", Value = "OTHERS" });

            ViewData["ProductType_"] = listproduct;

            int _bil5 = 1;
            List<SelectListItem> li5 = new List<SelectListItem>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();

                command.CommandText = @"SELECT DISTINCT Type FROM [IflowSeed].[dbo].[JobType]          
                                     ORDER BY Type";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JobInstruction model = new JobInstruction();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            model.JobType = reader.GetString(0);
                        }
                    }
                    int i = _bil5++;
                    if (i == 1)
                    {
                        li5.Add(new SelectListItem { Text = "Please Select" });
                        li5.Add(new SelectListItem { Text = model.JobType });

                    }
                    else
                    {
                        li5.Add(new SelectListItem { Text = model.JobType });
                    }
                }
                cn.Close();
            }
            ViewData["JobType_"] = li5;

            List<JobInstruction> JobInstructionlist1 = new List<JobInstruction>();

            string ProductName2="";
            string JobSheetNo2= "";
            string JobType2 = null;
            string JobInstructionId2 = "";
            string QMStatus2 = "";
            string Customer_Name2 = "";

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass, JobType,JobSheetNo, StartDevDate, EndDevDate,StatQM
                                        FROM [IflowSeed].[dbo].[JobInstruction]
                                        WHERE Id =@Id";

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
                            Customer_Name2= reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            ViewBag.ProductName = reader.GetString(2);
                            ProductName2 = reader.GetString(2);

                        }
                        if (reader.IsDBNull(3) == false)
                        {
                            ViewBag.JobClass = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            ViewBag.JobType = reader.GetString(4);
                            JobType2 = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            ViewBag.JobSheetNo = reader.GetString(5);
                            JobSheetNo2 = reader.GetString(5);

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
                            ViewBag.StatQM = reader.GetString(8);
                        }
                        
                        

                    }
                    JobInstructionlist1.Add(model);
                }
                cn.Close();

            }

            

             List<QM_Model> QM_Model = new List<QM_Model>(); 
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT JobInstruction.Id,  QM.Customer_Name, QM.ProductName, QM.JobSheetNo, QM.JobType, QM.AttachEncry, QM.RemarkAttachEncry, QM.CusApp, QM.RemarkCusApp, QM.Templayout, QM.RemarkTemplayout, QM.Corret, QM.RemarkCorret, QM.CorretInfo, QM.RemarkCorretInfo, QM.Compages, QM.RemarkCompages, QM.GoodQuality, QM.RemarkGoodQuality, QM.RejectQuality, QM.NameQ, QM.CommentQ, QM.RejectITO, QM.NameITO, QM.CommentITO, QM.RejectMBD, QM.NameMBD, QM.CommentMBD, QM.dateQ, QM.dateITO,
                                            QM.dateMBD,QM.QMStatus,QM.Id
                                  FROM  JobInstruction INNER JOIN
                                  QM ON JobInstruction.Id = QM.JobInstructionId
                                     where   JobInstruction.Id=@Id";

                command.Parameters.AddWithValue("@Id", Id);


                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    QM_Model model = new QM_Model();
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
                            ViewBag.JobSheetNo = reader.GetString(3);
                        }


                        if (reader.IsDBNull(4) == false)
                        {
                            ViewBag.JobType = reader.GetString(4);
                        }


                        //if (reader.IsDBNull(5) == false)
                        //{
                        //    bool getAttachEncry = reader.GetBoolean(5);
                        //    if (getAttachEncry == false)
                        //    {
                        //        ViewBag.AttachEncry = reader.GetBoolean(5);
                        //    }
                        //    else
                        //    {
                        //        ViewBag.AttachEncry = "checked";
                        //    }
                        //}

                        //if (reader.IsDBNull(6) == false)
                        //{
                        //    ViewBag.RemarkAttachEncry = reader.GetString(6);
                        //}


                        //if (reader.IsDBNull(7) == false)
                        //{
                        //    bool getCusApp = reader.GetBoolean(7);
                        //    if (getCusApp == false)
                        //    {
                        //        ViewBag.CusApp = "";
                        //    }
                        //    else
                        //    {
                        //        ViewBag.CusApp = "checked";
                        //    }
                        //}

                        //if (reader.IsDBNull(8) == false)
                        //{
                        //    ViewBag.RemarkCusApp = reader.GetString(8);
                        //}
                        //if (reader.IsDBNull(9) == false)
                        //{
                        //    bool getTemplayout = reader.GetBoolean(9);
                        //    if (getTemplayout == false)
                        //    {
                        //        ViewBag.Templayout = "";
                        //    }
                        //    else
                        //    {
                        //        ViewBag.Templayout = "checked";
                        //    }
                        //}
                        //if (reader.IsDBNull(10) == false)
                        //{
                        //    ViewBag.RemarkTemplayout = reader.GetString(10);
                        //}
                        //if (reader.IsDBNull(11) == false)
                        //{
                        //    bool getCorret = reader.GetBoolean(11);
                        //    if (getCorret == false)
                        //    {
                        //        ViewBag.Corret = "";
                        //    }
                        //    else
                        //    {
                        //        ViewBag.Corret = "checked";
                        //    }
                        //}
                        //if (reader.IsDBNull(12) == false)
                        //{
                        //    ViewBag.RemarkCorret = reader.GetString(12);
                        //}
                        //if (reader.IsDBNull(13) == false)
                        //{
                        //    bool getCorretInfo = reader.GetBoolean(13);
                        //    if (getCorretInfo == false)
                        //    {
                        //        ViewBag.CorretInfo = "";
                        //    }
                        //    else
                        //    {
                        //        ViewBag.CorretInfo = "checked";
                        //    }
                        //}
                        //if (reader.IsDBNull(14) == false)
                        //{
                        //    ViewBag.RemarkCorretInfo = reader.GetString(14);
                        //}

                        //if (reader.IsDBNull(15) == false)
                        //{
                        //    bool getCompages = reader.GetBoolean(15);
                        //    if (getCompages == false)
                        //    {
                        //        ViewBag.Compages = "";
                        //    }
                        //    else
                        //    {
                        //        ViewBag.Compages = "checked";
                        //    }
                        //}
                        //if (reader.IsDBNull(16) == false)
                        //{
                        //    ViewBag.RemarkCompages = reader.GetString(16);
                        //}

                        //if (reader.IsDBNull(17) == false)
                        //{
                        //    bool getGoodQuality = reader.GetBoolean(17);
                        //    if (getGoodQuality == false)
                        //    {
                        //        ViewBag.GoodQuality = "";
                        //    }
                        //    else
                        //    {
                        //        ViewBag.GoodQuality = "checked";
                        //    }
                        //}
                        //if (reader.IsDBNull(18) == false)
                        //{
                        //    ViewBag.RemarkGoodQuality = reader.GetString(18);
                        //}
                        //if (reader.IsDBNull(19) == false)
                        //{
                        //    bool getRejectQuality = reader.GetBoolean(19);
                        //    if (getRejectQuality == false)
                        //    {
                        //        ViewBag.RejectQuality = "";
                        //    }
                        //    else
                        //    {
                        //        ViewBag.RejectQuality = "checked";
                        //    }
                        //}
                        //if (reader.IsDBNull(20) == false)
                        //{
                        //    ViewBag.NameQ = reader.GetString(20);
                        //}
                        //if (reader.IsDBNull(21) == false)
                        //{
                        //    ViewBag.CommentQ = reader.GetString(21);
                        //}

                        //if (reader.IsDBNull(22) == false)
                        //{
                        //    bool getRejectITO = reader.GetBoolean(22);
                        //    if (getRejectITO == false)
                        //    {
                        //        ViewBag.RejectITO = "";
                        //    }
                        //    else
                        //    {
                        //        ViewBag.RejectITO = "checked";
                        //    }
                        //}
                        //if (reader.IsDBNull(23) == false)
                        //{
                        //    ViewBag.NameITO = reader.GetString(23);
                        //}
                        //if (reader.IsDBNull(24) == false)
                        //{
                        //    ViewBag.CommentITO = reader.GetString(24);
                        //}
                        //if (reader.IsDBNull(25) == false)
                        //{
                        //    bool getRejectMBD = reader.GetBoolean(25);
                        //    if (getRejectMBD == false)
                        //    {
                        //        ViewBag.RejectMBD = "";
                        //    }
                        //    else
                        //    {
                        //        ViewBag.RejectMBD = "checked";
                        //    }
                        //}
                        //if (reader.IsDBNull(26) == false)
                        //{
                        //    ViewBag.NameMBD = reader.GetString(26);
                        //}
                        //if (reader.IsDBNull(27) == false)
                        //{
                        //    ViewBag.CommentMBD = reader.GetString(27);
                        //}
                        //if (reader.IsDBNull(28) == false)
                        //{
                        //    ViewBag.dateQ = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(28));
                        //    //model.DateQTxt = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(28));
                        //}

                        //if (reader.IsDBNull(29) == false)
                        //{
                        //    ViewBag.dateITO = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(29));
                        //}

                        //if (reader.IsDBNull(30) == false)
                        //{
                        //    ViewBag.dateMBD = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(30));
                        //}
                        //if (reader.IsDBNull(31) == false)
                        //{
                        //    ViewBag.QMStatus = reader.GetString(31);
                        //}
                    }
                    QM_Model.Add(model);
                }

                reader.Close();

                SqlCommand cmd = new SqlCommand("select PaperSizeInput,ApprovalPaperSize,RemarkPaperSize,PaperWeightInput,ApprovalPaperWeight,RemarkPaperWeight,PaperTypeInput,ApprovalPaperType,RemarkPaperType,EnvelopeTypeInput," +
                    "ApprovalEnvelopeType,RemarkEnvelopeType,AuditTrailReportInput,AccQty,ImpQty,PageQty,ApprovalAuditTrailReport,RemarkAuditTrailReport,CusApp,RemarkCusApp,Templayout,RemarkTemplayout,ApprovalOMRMarker,RemarkOMRMarker," +
                    "ApprovalCompleteAddress,RemarkCompleteAddress,StatementDateReferInput,ApprovalStatementDateRefer,RemarkStatementDateRefer,AccNoInput,ApprovalAccNo,RemarkAccNo,ApprovalBarcodeComplete,RemarkBarcodeComplete," +
                    "ApprovalJIAvailable,RemarkJIAvailable,ApprovalTestingResultEng,RemarkTestingResultEng,TestingResultITApproval,TestingResultITRemark,Comment,CorrectiveAction,PaperTypeInput2 " +
                    "FROM QM WHERE JobInstructionId=@JobInstructionId;", cn);

                cmd.Parameters.AddWithValue("@JobInstructionId", Id);
                SqlDataReader rm = cmd.ExecuteReader();

                while(rm.Read())
                {
                    if (!rm.IsDBNull(0))
                    {
                        ViewBag.PaperSizeInput = rm.GetString(0);
                    }

                    if (!rm.IsDBNull(1))
                    {
                        ViewBag.ApprovalPaperSize = "Checked";
                        //ViewBag.ApprovalPaperSize = rm.GetBoolean(1);
                    }

                    if (!rm.IsDBNull(2))
                    {
                        ViewBag.RemarkPaperSize = rm.GetString(2);
                    }

                    if (!rm.IsDBNull(3))
                    {
                        ViewBag.PaperWeightInput = rm.GetString(3);
                    }

                    if (!rm.IsDBNull(4))
                    {
                        ViewBag.ApprovalPaperWeight = "Checked";
                    }

                    if (!rm.IsDBNull(5))
                    {
                        ViewBag.RemarkPaperWeight = rm.GetString(5);
                    }

                    if (!rm.IsDBNull(6))
                    {
                        ViewBag.PaperTypeInput = rm.GetString(6);
                    }

                    if (!rm.IsDBNull(7))
                    {
                        ViewBag.ApprovalPaperType = "Checked";
                    }

                    if (!rm.IsDBNull(8))
                    {
                        ViewBag.RemarkPaperType = rm.GetString(8);
                    }

                    if (!rm.IsDBNull(9))
                    {
                        ViewBag.EnvelopeTypeInput = rm.GetString(9);
                    }

                    if (!rm.IsDBNull(10))
                    {
                        ViewBag.ApprovalEnvelopeType = "Checked";
                    }

                    if (!rm.IsDBNull(11))
                    {
                        ViewBag.RemarkEnvelopeType = rm.GetString(11);
                    }

                    if (!rm.IsDBNull(12))
                    {
                        ViewBag.AuditTrailReportInput = rm.GetString(12);
                    }

                    if (!rm.IsDBNull(13))
                    {
                        ViewBag.AccQty = rm.GetString(13);
                    }

                    if (!rm.IsDBNull(14))
                    {
                        ViewBag.ImpQty = rm.GetString(14);
                    }

                    if (!rm.IsDBNull(15))
                    {
                        ViewBag.PageQty = rm.GetString(15);
                    }

                    if (!rm.IsDBNull(16))
                    {
                        ViewBag.ApprovalAuditTrailReport = "Checked";
                    }

                    if (!rm.IsDBNull(17))
                    {
                        ViewBag.RemarkAuditTrailReport = rm.GetString(17);
                    }

                    if (!rm.IsDBNull(18))
                    {
                        ViewBag.CusApp = "Checked";
                    }

                    if (!rm.IsDBNull(19))
                    {
                        ViewBag.RemarkCusApp = rm.GetString(19);
                    }

                    if (!rm.IsDBNull(20))
                    {
                        ViewBag.Templayout = "Checked";
                    }

                    if (!rm.IsDBNull(21))
                    {
                        ViewBag.RemarkTemplayout = rm.GetString(21);
                    }

                    if (!rm.IsDBNull(22))
                    {
                        ViewBag.ApprovalOMRMarker = "Checked";
                    }

                    if (!rm.IsDBNull(23))
                    {
                        ViewBag.RemarkOMRMarker = rm.GetString(23);
                    }

                    if (!rm.IsDBNull(24))
                    {
                        ViewBag.ApprovalCompleteAddress = "Checked";
                    }

                    if (!rm.IsDBNull(25))
                    {
                        ViewBag.RemarkCompleteAddress = rm.GetString(25);
                    }

                    if (!rm.IsDBNull(26))
                    {
                        ViewBag.StatementDateReferInput = rm.GetString(26);
                    }

                    if (!rm.IsDBNull(27))
                    {
                        ViewBag.ApprovalStatementDateRefer = "Checked";
                    }

                    if (!rm.IsDBNull(28))
                    {
                        ViewBag.RemarkStatementDateRefer = rm.GetString(28);
                    }

                    if (!rm.IsDBNull(29))
                    {
                        ViewBag.AccNoInput = rm.GetString(29);
                    }

                    if (!rm.IsDBNull(30))
                    {
                        ViewBag.ApprovalAccNo = "Checked";
                    }

                    if (!rm.IsDBNull(31))
                    {
                        ViewBag.RemarkAccNo = rm.GetString(31);
                    }

                    if (!rm.IsDBNull(32))
                    {
                        ViewBag.ApprovalBarcodeComplete = "Checked";
                    }

                    if (!rm.IsDBNull(33))
                    {
                        ViewBag.RemarkBarcodeComplete = rm.GetString(33);
                    }

                    if (!rm.IsDBNull(34))
                    {
                        ViewBag.ApprovalJIAvailable = "Checked";
                    }

                    if (!rm.IsDBNull(35))
                    {
                        ViewBag.RemarkJIAvailable = rm.GetString(35);
                    }

                    if (!rm.IsDBNull(36))
                    {
                        ViewBag.ApprovalTestingResultEng = "Checked";
                    }

                    if (!rm.IsDBNull(37))
                    {
                        ViewBag.RemarkTestingResultEng = rm.GetString(37);
                    }

                    if (!rm.IsDBNull(38))
                    {
                        ViewBag.TestingResultITApproval = "Checked";
                    }

                    if (!rm.IsDBNull(39))
                    {
                        ViewBag.TestingResultITRemark = rm.GetString(39);
                    }

                    if (!rm.IsDBNull(40))
                    {
                        ViewBag.Comment = rm.GetString(40);
                    }

                    if (!rm.IsDBNull(41))
                    {
                        ViewBag.CorrectiveAction = rm.GetString(41);
                    }

                    if (!rm.IsDBNull(42))
                    {
                        ViewBag.PaperTypeInput2 = rm.GetString(42);
                    }
                    

                }

                rm.Close();

                SqlCommand cmd2 = new SqlCommand("select RejectMBD,RejectQuality,RejectITO,ApprovalEngineering,ApprovalITODoc,PlanningApproval,ApprovalDDPOperation,NameMBD,NameITO,NameQ,NameEngineering,NameITODoc," +
                    "NamePlanning,DDPOperationName,CommentMBD,CommentITO,CommentQ,CommentEngineering,CommentITODoc,CommentPlanning,CommentDDPOperation, ProcessPrinting, ProcessInserting, ProcessSelfMailer, ProcessMMP from QM WHERE JobInstructionId=@JobInstructionId;", cn);
                cmd2.Parameters.AddWithValue("@JobInstructionId", Id);
                SqlDataReader rm2= cmd2.ExecuteReader();

                while(rm2.Read())
                {
                    if (!rm2.IsDBNull(0))
                    {
                        ViewBag.RejectMBD = "Checked";
                    }
                    else
                    {
                        if(dept!="MBD")
                        {
                            ViewBag.RejectMBD = "disabled";
                        }

                    }

                    if (!rm2.IsDBNull(1))
                    {
                        ViewBag.RejectQuality = "Checked";
                    }
                    else
                    {
                        if(dept!="QM")
                        {
                            ViewBag.RejectQuality = "disabled";
                        }
                    }

                    if (!rm2.IsDBNull(2))
                    {
                        ViewBag.RejectITO = "Checked";
                    }
                    else
                    {
                        if (dept!="IT")
                        {
                            ViewBag.RejectITO = "disabled";
                        }

                    }

                    if (!rm2.IsDBNull(3))
                    {
                        ViewBag.ApprovalEngineering = "Checked";
                    }
                    else
                    {
                        if (dept != "PRODUCTION" && role!="Engineering")
                        {
                            ViewBag.ApprovalEngineering = "disabled";
                        }


                    }

                    if (!rm2.IsDBNull(4))
                    {
                        ViewBag.ApprovalITODoc = "Checked";
                    }
                    else
                    {

                        if (dept != "IT")
                        {
                            ViewBag.ApprovalITODoc = "disabled";
                        }
                    }

                    if (!rm2.IsDBNull(5))
                    {
                        ViewBag.PlanningApproval = "Checked";
                    }
                    else
                    {
                        if (dept != "PRODUCTION" && role!="Planner")
                        {
                            ViewBag.PlanningApproval = "disabled";

                        }

                    }

                    if (!rm2.IsDBNull(6))
                    {
                        ViewBag.ApprovalDDPOperation = "Checked";
                    }
                    else
                    {
                        if (dept != "PRODUCTION" && role != "Super Admin")
                        {
                            ViewBag.ApprovalDDPOperation = "disabled";
                        }

                    }

                    if (!rm2.IsDBNull(7))
                    {
                        ViewBag.NameMBD = rm2.GetString(7);
                    }
                   

                    if (!rm2.IsDBNull(8))
                    {
                        ViewBag.NameITO = rm2.GetString(8);
                    }

                    if (!rm2.IsDBNull(9))
                    {
                        ViewBag.NameQ = rm2.GetString(9);
                    }

                    if (!rm2.IsDBNull(10))
                    {
                        ViewBag.NameEngineering = rm2.GetString(10);
                    }

                    if (!rm2.IsDBNull(11))
                    {
                        ViewBag.NameITODoc = rm2.GetString(11);
                    }

                    if (!rm2.IsDBNull(12))
                    {
                        ViewBag.NamePlanning = rm2.GetString(12);
                    }

                    if (!rm2.IsDBNull(13))
                    {
                        ViewBag.NameDDPOperation = rm2.GetString(13);
                    }

                    if (!rm2.IsDBNull(14))
                    {           
                        ViewBag.CommentMBD = rm2.GetString(14);
                    }

                    if (!rm2.IsDBNull(15))
                    {
                        ViewBag.CommentITO = rm2.GetString(15);
                    }

                    if (!rm2.IsDBNull(16))
                    {
                        ViewBag.CommentQ = rm2.GetString(16);
                    }

                    if (!rm2.IsDBNull(17))
                    {
                        ViewBag.CommentEngineer = rm2.GetString(17);
                    }

                    if (!rm2.IsDBNull(18))
                    {
                        ViewBag.CommentITODoc = rm2.GetString(18);
                    }

                    if (!rm2.IsDBNull(19))
                    {
                        ViewBag.CommentPlanning = rm2.GetString(19);
                    }

                    if (!rm2.IsDBNull(20))
                    {
                        ViewBag.CommentDDPOperation = rm2.GetString(20);
                    }

                    if (!rm2.IsDBNull(21))
                    {
                        ViewBag.ProcessPrinting = "Checked";
                    }
                    else
                    {
                        ViewBag.ProcessPrinting = "disabled";
                    }

                    if (!rm2.IsDBNull(22))
                    {
                        ViewBag.ProcessInserting = "Checked";
                    }
                    else
                    {
                        ViewBag.ProcessInserting = "disabled";

                    }

                    if (!rm2.IsDBNull(23))
                    {
                        ViewBag.ProcessSelfMailer = "Checked";
                    }
                    else
                    {
                        ViewBag.ProcessSelfMailer = "disabled";
                    }

                    if (!rm2.IsDBNull(24))
                    {
                        ViewBag.ProcessMMP = "Checked";
                    }
                    else
                    {
                        ViewBag.ProcessMMP = "disabled";

                    }

                }

                rm2.Close();

                SqlCommand cmd3 = new SqlCommand("SELECT dateEngineering,dateITODoc,datePlanning,dateDDPOperation,dateMBD,dateITO,dateQ FROM QM WHERE JobInstructionId=@JobInstructionIdcmd3", cn);

                cmd3.Parameters.AddWithValue("@JobInstructionIdcmd3", Id);

                SqlDataReader rm3 =cmd3.ExecuteReader();

                while(rm3.Read())
                {
                    if (!rm3.IsDBNull(0))
                    {
                        ViewBag.dateEngineering = rm3.GetString(0);
                    }
                    else
                    {
                        ViewBag.dateEngineering = null;
                    }

                    if (!rm3.IsDBNull(1))
                    {
                        ViewBag.dateITODoc = rm3.GetString(1);
                    }
                    else
                    {
                        ViewBag.dateITODoc = null;
                    }

                    if (!rm3.IsDBNull(2))
                    {
                        ViewBag.datePlanning = rm3.GetString(2);
                    }
                    else
                    {
                        ViewBag.datePlanning = null;
                    }

                    if (!rm3.IsDBNull(3))
                    {
                        ViewBag.dateDDPOperation = rm3.GetString(3);
                    }
                    else
                    {
                        ViewBag.dateDDPOperation = null;
                    }

                    if (!rm3.IsDBNull(4))
                    {
                        ViewBag.dateMBD = rm3.GetString(4);
                    }
                    else
                    {
                        ViewBag.dateMBD = null;
                    }

                    if (!rm3.IsDBNull(5))
                    {
                        ViewBag.dateITO = rm3.GetString(5);
                    }
                    else
                    {
                        ViewBag.dateITO = null;
                    }

                    if (!rm3.IsDBNull(6))
                    {
                        ViewBag.dateQ = rm3.GetString(6);
                    }
                    else
                    {
                        ViewBag.dateQ = null;
                    }

                }

                rm3.Close();

                SqlCommand cmdcheck = new SqlCommand("select * from QM WHERE JobInstructionid=@JobInstructionIdCheck", cn);
                cmdcheck.Parameters.AddWithValue("@JobInstructionIdCheck", Id);
                SqlDataReader rmcheck = cmdcheck.ExecuteReader();

                if (!rmcheck.HasRows)
                {
                    SqlCommand cmdInsert = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[QM] ([Id],[CreateOn],[Customer_Name],[ProductName],[JobSheetNo],[JobType],[JobInstructionId],[QMStatus])" +
                                                "VALUES (@Id,@CreateOn,@Customer_Name,@ProductName,@JobSheetNo,@JobType,@JobInstructionId,@QMStatus)", cn);
                    Guid guidx = Guid.NewGuid();
                    cmdInsert.Parameters.AddWithValue("@Id", guidx);
                    cmdInsert.Parameters.AddWithValue("@CreateOn", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt "));
                    cmdInsert.Parameters.AddWithValue("@Customer_Name", Customer_Name2);
                    cmdInsert.Parameters.AddWithValue("@ProductName", ProductName2);
                    cmdInsert.Parameters.AddWithValue("@JobSheetNo", JobSheetNo2);
                    cmdInsert.Parameters.AddWithValue("@JobType", JobType2);
                    cmdInsert.Parameters.AddWithValue("@JobInstructionId", Id);
                    cmdInsert.Parameters.AddWithValue("@QMStatus", "Progress");

                    cmdInsert.ExecuteNonQuery();
                }

                rmcheck.Close();

                SqlCommand cmdlogtagno = new SqlCommand("SELECT LogTagNo FROM JobAuditTrailDetail WHERE JobSheetNo=@JobSheetNocmdlogtagno", cn);

                cmdlogtagno.Parameters.AddWithValue("@JobSheetNocmdlogtagno", JobSheetNo2);

                SqlDataReader rmlogtagno = cmdlogtagno.ExecuteReader();

                int bil = 1;

                while(rmlogtagno.Read())
                {
                    //var model = new JobAuditTrailDetail();
                    //{
                    //    model.Bil = bil;
                    //    model.LogTagNo = rmlogtagno.GetString(0);
                    //}
                    //bil++;
                    //logtagno.Add(model);

                    ViewBag.LogTagNo=rmlogtagno.GetString(0);
                }


                cn.Close();


            }

           

            if (set == "Verify")
            {
                Session["Id"] = Id;
                ViewBag.Id = Id;
                ViewBag.Customer_Name = Customer_Name;
                ViewBag.ProductName = ProductName;

                //get.DateQ = Convert.ToDateTime(get.DateQtxt);
                //get.DateITO = Convert.ToDateTime(get.DateITOtxt);
                //get.DateMBD = Convert.ToDateTime(get.DateMBDtxt);

                //string DateQ = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt ");
                //string DateITO = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt ");
                //string DateMBD = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt ");


                //using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                //{
                //    Guid guidId = Guid.NewGuid();
                //    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                //    cn2.Open();
                //    SqlCommand command;
                //    command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[QMITO] ([Id],[ModifiedOn],[RejectITO],[CommentITO],[dateITO],[JobInstructionId],[QMStatus],[NameITO])" +
                //                             "VALUES (@Id,@ModifiedOn,@RejectITO,@CommentITO,@dateITO,@JobInstructionId,@QMStatus,@NameITO)", cn2);
                //    command.Parameters.AddWithValue("@Id", guidId);                   
                //    command.Parameters.AddWithValue("@ModifiedOn", createdOn);
                //    if (!string.IsNullOrEmpty(RejectITO))
                //    {
                //        command.Parameters.AddWithValue("@RejectITO", RejectITO);
                //    }
                //    else
                //    {
                //        command.Parameters.AddWithValue("@RejectITO", DBNull.Value);

                //    }
                //    command.Parameters.AddWithValue("@CommentITO", CommentITO);
                //    command.Parameters.AddWithValue("@dateITO", dateITO);
                //    command.Parameters.AddWithValue("@JobInstructionId", Id);
                //    command.Parameters.AddWithValue("@QMStatus", "VerifyITO");
                //    command.Parameters.AddWithValue("@NameITO", NameITO);

                //    command.ExecuteNonQuery();
                //    cn2.Close();
                //}

                //using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                //{
                //    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                //    cn3.Open();
                //    SqlCommand command2;
                //    command2 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction]  SET ModifiedOn=@ModifiedOn,StatQM=@StatQM WHERE Id=@Id", cn3);

                //    command2.Parameters.AddWithValue("@ModifiedOn", createdOn);
                //    command2.Parameters.AddWithValue("@Id", get.Id);
                //    command2.Parameters.AddWithValue("@StatQM", "Verify");
                //    command2.ExecuteNonQuery();
                //    cn3.Close();
                //}

                //using (SqlConnection cn4 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                //{
                //    Guid guidId = Guid.NewGuid();
                //    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                //    cn4.Open();
                //    SqlCommand command;
                //    command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[QM] ([Id],[CreateOn],[Customer_Name],[ProductName],[JobSheetNo],[JobType],[JobInstructionId],[QMStatus])" +
                //                             "VALUES (@Id,@CreateOn,@Customer_Name,@ProductName,@JobSheetNo,@JobType,@JobInstructionId,@QMStatus)", cn4);

                //    //command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[QM] ([Id],[CreateOn],[Customer_Name],[ProductName],[JobSheetNo],[JobType],[AttachEncry],[RemarkAttachEncry] ,[CusApp] ,[RemarkCusApp],[Templayout],[RemarkTemplayout] ,[Corret],[RemarkCorret],[CorretInfo],[RemarkCorretInfo],[Compages],[RemarkCompages] ,[GoodQuality] ,[RemarkGoodQuality],[RejectQuality],[NameQ],[CommentQ],[dateQ],[JobInstructionId],[QMStatus])" +
                //    //     "VALUES (@Id,@CreateOn,@Customer_Name,@ProductName,@JobSheetNo,@JobType,@AttachEncry,@RemarkAttachEncry,@CusApp,@RemarkCusApp,@Templayout,@RemarkTemplayout,@Corret,@RemarkCorret,@CorretInfo,@RemarkCorretInfo,@Compages,@RemarkCompages,@GoodQuality,@RemarkGoodQuality,@RejectQuality,@NameQ,@CommentQ,@dateQ,@JobInstructionId,@QMStatus)", cn4);

                //    command.Parameters.AddWithValue("@Id", guidId);
                //    command.Parameters.AddWithValue("@CreateOn", createdOn);
                //    command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                //    command.Parameters.AddWithValue("@ProductName", ProductName);
                //    command.Parameters.AddWithValue("@JobSheetNo", JobSheetNo);
                //    command.Parameters.AddWithValue("@JobType", JobType);
                //    //if (!string.IsNullOrEmpty(AttachEncry))
                //    //{
                //    //    command.Parameters.AddWithValue("@AttachEncry", AttachEncry);
                //    //}
                //    //else
                //    //{
                //    //    command.Parameters.AddWithValue("@AttachEncry", DBNull.Value);
                //    //}


                //    //if (!string.IsNullOrEmpty(RemarkAttachEncry))
                //    //{
                //    //    command.Parameters.AddWithValue("@RemarkAttachEncry", RemarkAttachEncry);
                //    //}
                //    //else
                //    //{
                //    //    command.Parameters.AddWithValue("@RemarkAttachEncry", DBNull.Value);
                //    //}
                //    //if (!string.IsNullOrEmpty(RemarkCusApp))
                //    //{
                //    //    command.Parameters.AddWithValue("@CusApp", CusApp);
                //    //}
                //    //else
                //    //{
                //    //    command.Parameters.AddWithValue("@CusApp", DBNull.Value);
                //    //}

                //    //if (!string.IsNullOrEmpty(RemarkCusApp))
                //    //{
                //    //    command.Parameters.AddWithValue("@RemarkCusApp", RemarkCusApp);
                //    //}
                //    //else
                //    //{
                //    //    command.Parameters.AddWithValue("@RemarkCusApp", DBNull.Value);
                //    //}
                //    //if (!string.IsNullOrEmpty(Templayout))
                //    //{
                //    //    command.Parameters.AddWithValue("@Templayout", Templayout);
                //    //}
                //    //else
                //    //{
                //    //    command.Parameters.AddWithValue("@Templayout", DBNull.Value);
                //    //}

                //    //if (!string.IsNullOrEmpty(RemarkTemplayout))
                //    //{
                //    //    command.Parameters.AddWithValue("@RemarkTemplayout", RemarkTemplayout);
                //    //}
                //    //else
                //    //{
                //    //    command.Parameters.AddWithValue("@RemarkTemplayout", DBNull.Value);
                //    //}

                //    //if (!string.IsNullOrEmpty(Corret))
                //    //{
                //    //    command.Parameters.AddWithValue("@Corret", Corret);
                //    //}
                //    //else
                //    //{
                //    //    command.Parameters.AddWithValue("@Corret", DBNull.Value);
                //    //}

                //    //if (!string.IsNullOrEmpty(RemarkCorret))
                //    //{
                //    //    command.Parameters.AddWithValue("@RemarkCorret", RemarkCorret);
                //    //}
                //    //else
                //    //{
                //    //    command.Parameters.AddWithValue("@RemarkCorret", DBNull.Value);
                //    //}
                //    //if (!string.IsNullOrEmpty(CorretInfo))
                //    //{
                //    //    command.Parameters.AddWithValue("@CorretInfo", CorretInfo);

                //    //}
                //    //else
                //    //{
                //    //    command.Parameters.AddWithValue("@CorretInfo", DBNull.Value);
                //    //}
                //    //if (!string.IsNullOrEmpty(RemarkCorretInfo))
                //    //{

                //    //    command.Parameters.AddWithValue("@RemarkCorretInfo", RemarkCorretInfo);
                //    //}
                //    //else
                //    //{
                //    //    command.Parameters.AddWithValue("@RemarkCorretInfo", DBNull.Value);
                //    //}

                //    //if (!string.IsNullOrEmpty(Compages))
                //    //{
                //    //    command.Parameters.AddWithValue("@Compages", Compages);
                //    //}
                //    //else
                //    //{
                //    //    command.Parameters.AddWithValue("@Compages", DBNull.Value);
                //    //}


                //    //if (!string.IsNullOrEmpty(RemarkCompages))
                //    //{
                //    //    command.Parameters.AddWithValue("@RemarkCompages", RemarkCompages);
                //    //}
                //    //else
                //    //{
                //    //    command.Parameters.AddWithValue("@RemarkCompages", DBNull.Value);
                //    //}

                //    //if (!string.IsNullOrEmpty(GoodQuality))
                //    //{
                //    //    command.Parameters.AddWithValue("@GoodQuality", GoodQuality);

                //    //}
                //    //else
                //    //{
                //    //    command.Parameters.AddWithValue("@GoodQuality", DBNull.Value);
                //    //}

                //    //if (!string.IsNullOrEmpty(RemarkGoodQuality))
                //    //{
                //    //    command.Parameters.AddWithValue("@RemarkGoodQuality", RemarkGoodQuality);
                //    //}
                //    //else
                //    //{
                //    //    command.Parameters.AddWithValue("@RemarkGoodQuality", DBNull.Value);
                //    //}
                //    //if (!string.IsNullOrEmpty(RejectQuality))
                //    //{
                //    //    command.Parameters.AddWithValue("@RejectQuality", RejectQuality);
                //    //}
                //    //else
                //    //{
                //    //    command.Parameters.AddWithValue("@RejectQuality", DBNull.Value);
                //    //}
                //    //if (!string.IsNullOrEmpty(NameQ))
                //    //{
                //    //    command.Parameters.AddWithValue("@NameQ", NameQ);
                //    //}
                //    //else
                //    //{
                //    //    command.Parameters.AddWithValue("@NameQ", DBNull.Value);
                //    //}
                //    //if (!string.IsNullOrEmpty(NameQ))
                //    //{
                //    //    command.Parameters.AddWithValue("@CommentQ", CommentQ);
                //    //}
                //    //else
                //    //{
                //    //    command.Parameters.AddWithValue("@CommentQ", DBNull.Value);
                //    //}

                //    //if (!string.IsNullOrEmpty(dateQ))
                //    //{
                //    //    command.Parameters.AddWithValue("@dateQ", dateQ);
                //    //}
                //    //else
                //    //{
                //    //    command.Parameters.AddWithValue("@dateQ", DBNull.Value);
                //    //}

                //    //command.Parameters.AddWithValue("@JobInstructionId", Id);
                //    //command.Parameters.AddWithValue("@QMStatus", "Progress");
                //    //command.ExecuteNonQuery();
                //    cn4.Close();
                //}



                return RedirectToAction("ManageQM", "QM");

            }

            

            if (set == "Save")
            {
                Session["Id"] = Id;
                ViewBag.Id = Id;
                ViewBag.Customer_Name = Customer_Name;
                ViewBag.ProductName = ProductName;
                Debug.WriteLine("Verify Button Pressed");


                //string DateQ2 = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                //string DateITO2 = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                //string DateMBD2 = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                //string DateITO = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt ");

                //using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                //{
                //    cn1.Open();
                //    SqlCommand command1;
                //    command1 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET ModifiedOn=@ModifiedOn WHERE Id=@Id", cn1);
                //    command1.Parameters.AddWithValue("@Id", Id);
                //    command1.Parameters.AddWithValue("@ModifiedOn", createdOn);
                //    command1.ExecuteNonQuery();
                //    cn1.Close();
                //}

                using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn1.Open();
                    SqlCommand command2;
                    command2 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction]  SET ModifiedOn=@ModifiedOn,StatQM=@StatQM WHERE Id=@Id", cn1);

                    command2.Parameters.AddWithValue("@ModifiedOn", createdOn);
                    command2.Parameters.AddWithValue("@Id", Id);
                    command2.Parameters.AddWithValue("@StatQM", "Partially Verified");
                    command2.ExecuteNonQuery();
                    cn1.Close();
                }



                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn2.Open();

                    if(Department=="QM")
                    {
                        SqlCommand command;
                        //command = new SqlCommand("UPDATE [IflowSeed].[dbo].[QM]  SET ModifiedOn=@ModifiedOn,QMStatus=@QMStatus,RejectITO=@RejectITO,NameITO=@NameITO,CommentITO=@CommentITO,dateITO=@dateITO WHERE Id=@JobInstructionId", cn2);
                        //command = new SqlCommand("UPDATE [IflowSeed].[dbo].[QM]  SET CusApp=@CusApp, AttachEncry=@AttachEncry, Templayout=@Templayout, Corret=@Corret, CorretInfo=@CorretInfo, Compages=@Compages, GoodQuality=@GoodQuality, RejectQuality=@RejectQuality, QMStatus=@QMStatus,RejectITO=@RejectITO,NameITO=@NameITO,CommentITO=@CommentITO,dateITO=@dateITO WHERE JobInstructionId=@JobInstructionId", cn2);
                        command = new SqlCommand("UPDATE QM Set PaperSizeInput=@PaperSizeInput,ApprovalPaperSize=@ApprovalPaperSize,RemarkPaperSize=@RemarkPaperSize,PaperWeightInput=@PaperWeightInput,ApprovalPaperWeight=@ApprovalPaperWeight,RemarkPaperWeight=@RemarkPaperWeight," +
                        "PaperTypeInput=@PaperTypeInput,ApprovalPaperType=@ApprovalPaperType,RemarkPaperType=@RemarkPaperType,EnvelopeTypeInput=@EnvelopeTypeInput,ApprovalEnvelopeType=@ApprovalEnvelopeType,RemarkEnvelopeType=@RemarkEnvelopeType,AuditTrailReportInput=@AuditTrailReportInput," +
                        "AccQty=@AccQty,ImpQty=@ImpQty,PageQty=@PageQty,ApprovalAuditTrailReport=@ApprovalAuditTrailReport,RemarkAuditTrailReport=@RemarkAuditTrailReport,CusApp=@CusApp,RemarkCusApp=@RemarkCusApp,Templayout=@Templayout,RemarkTemplayout=@RemarkTemplayout," +
                        "ApprovalOMRMarker=@ApprovalOMRMarker,RemarkOMRMarker=@RemarkOMRMarker, ApprovalCompleteAddress=@ApprovalCompleteAddress, RemarkCompleteAddress=@RemarkCompleteAddress,StatementDateReferInput=@StatementDateReferInput,ApprovalStatementDateRefer=@ApprovalStatementDateRefer," +
                        "RemarkStatementDateRefer=@RemarkStatementDateRefer,AccNoInput=@AccNoInput,ApprovalAccNo=@ApprovalAccNo,RemarkAccNo=@RemarkAccNo,ApprovalBarcodeComplete=@ApprovalBarcodeComplete,RemarkBarcodeComplete=@RemarkBarcodeComplete,ApprovalJIAvailable=@ApprovalJIAvailable," +
                        "RemarkJIAvailable=@RemarkJIAvailable,ApprovalTestingResultEng=@ApprovalTestingResultEng,RemarkTestingResultEng=@RemarkTestingResultEng,TestingResultITApproval=@TestingResultITApproval,TestingResultITRemark=@TestingResultITRemark,Comment=@Comment," +
                        "CorrectiveAction=@CorrectiveAction,PaperTypeInput2=@PaperTypeInput2 " +
                        "WHERE JobInstructionId=@JobInstructionId", cn2);

                        //command.Parameters.AddWithValue("@ModifiedOn", createdOn);
                        command.Parameters.AddWithValue("@JobInstructionId", Id);


                        if (String.IsNullOrEmpty(PaperSizeInput))
                        {
                            command.Parameters.AddWithValue("@PaperSizeInput", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PaperSizeInput", PaperSizeInput);
                        }

                        if (ApprovalPaperSize == null)
                        {
                            command.Parameters.AddWithValue("@ApprovalPaperSize", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@ApprovalPaperSize", ApprovalPaperSize);
                        }

                        if (RemarkPaperSize == null)
                        {
                            command.Parameters.AddWithValue("@RemarkPaperSize", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@RemarkPaperSize", RemarkPaperSize);
                        }

                        if (PaperWeightInput == null)
                        {
                            command.Parameters.AddWithValue("@PaperWeightInput", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PaperWeightInput", PaperWeightInput);
                        }

                        if (ApprovalPaperWeight == null)
                        {
                            command.Parameters.AddWithValue("@ApprovalPaperWeight", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@ApprovalPaperWeight", ApprovalPaperWeight);
                        }

                        if (RemarkPaperWeight == null)
                        {
                            command.Parameters.AddWithValue("@RemarkPaperWeight", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@RemarkPaperWeight", RemarkPaperWeight);
                        }

                        if (PaperTypeInput == null)
                        {
                            command.Parameters.AddWithValue("@PaperTypeInput", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PaperTypeInput", PaperTypeInput);
                        }

                        if (ApprovalPaperType == null)
                        {
                            command.Parameters.AddWithValue("@ApprovalPaperType", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@ApprovalPaperType", ApprovalPaperType);
                        }

                        if (RemarkPaperType == null)
                        {
                            command.Parameters.AddWithValue("@RemarkPaperType", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@RemarkPaperType", RemarkPaperType);
                        }

                        if (EnvelopeTypeInput == null)
                        {
                            command.Parameters.AddWithValue("@EnvelopeTypeInput", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@EnvelopeTypeInput", EnvelopeTypeInput);
                        }

                        if (ApprovalEnvelopeType == null)
                        {
                            command.Parameters.AddWithValue("@ApprovalEnvelopeType", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@ApprovalEnvelopeType", ApprovalEnvelopeType);
                        }

                        if (RemarkEnvelopeType == null)
                        {
                            command.Parameters.AddWithValue("@RemarkEnvelopeType", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@RemarkEnvelopeType", RemarkEnvelopeType);
                        }

                        if (AuditTrailReportInput == null)
                        {
                            command.Parameters.AddWithValue("@AuditTrailReportInput", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@AuditTrailReportInput", AuditTrailReportInput);
                        }

                        if (AccQty == null)
                        {
                            command.Parameters.AddWithValue("@AccQty", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@AccQty", AccQty);
                        }

                        if (ImpQty == null)
                        {
                            command.Parameters.AddWithValue("@ImpQty", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@ImpQty", ImpQty);
                        }

                        if (PageQty == null)
                        {
                            command.Parameters.AddWithValue("@PageQty", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PageQty", PageQty);
                        }

                        if (ApprovalAuditTrailReport == null)
                        {
                            command.Parameters.AddWithValue("@ApprovalAuditTrailReport", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@ApprovalAuditTrailReport", ApprovalAuditTrailReport);
                        }

                        if (RemarkAuditTrailReport == null)
                        {
                            command.Parameters.AddWithValue("@RemarkAuditTrailReport", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@RemarkAuditTrailReport", RemarkAuditTrailReport);
                        }

                        if (CusApp == null)
                        {
                            command.Parameters.AddWithValue("@CusApp", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@CusApp", CusApp);
                        }

                        if (RemarkCusApp == null)
                        {
                            command.Parameters.AddWithValue("@RemarkCusApp", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@RemarkCusApp", RemarkCusApp);
                        }

                        if (Templayout == null)
                        {
                            command.Parameters.AddWithValue("@Templayout", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Templayout", Templayout);
                        }

                        if (RemarkTemplayout == null)
                        {
                            command.Parameters.AddWithValue("@RemarkTemplayout", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@RemarkTemplayout", RemarkTemplayout);
                        }

                        if (ApprovalOMRMarker == null)
                        {
                            command.Parameters.AddWithValue("@ApprovalOMRMarker", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@ApprovalOMRMarker", ApprovalOMRMarker);
                        }

                        if (RemarkOMRMarker == null)
                        {
                            command.Parameters.AddWithValue("@RemarkOMRMarker", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@RemarkOMRMarker", RemarkOMRMarker);
                        }

                        if (ApprovalCompleteAddress == null)
                        {
                            command.Parameters.AddWithValue("@ApprovalCompleteAddress", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@ApprovalCompleteAddress", ApprovalCompleteAddress);
                        }

                        if (RemarkCompleteAddress == null)
                        {
                            command.Parameters.AddWithValue("@RemarkCompleteAddress", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@RemarkCompleteAddress", RemarkCompleteAddress);
                        }

                        if (StatementDateReferInput == null)
                        {
                            command.Parameters.AddWithValue("@StatementDateReferInput", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@StatementDateReferInput", StatementDateReferInput);
                        }

                        if (ApprovalStatementDateRefer == null)
                        {
                            command.Parameters.AddWithValue("@ApprovalStatementDateRefer", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@ApprovalStatementDateRefer", ApprovalStatementDateRefer);
                        }

                        if (RemarkStatementDateRefer == null)
                        {
                            command.Parameters.AddWithValue("@RemarkStatementDateRefer", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@RemarkStatementDateRefer", RemarkStatementDateRefer);
                        }

                        if (AccNoInput == null)
                        {
                            command.Parameters.AddWithValue("@AccNoInput", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@AccNoInput", AccNoInput);
                        }

                        if (ApprovalAccNo == null)
                        {
                            command.Parameters.AddWithValue("@ApprovalAccNo", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@ApprovalAccNo", ApprovalAccNo);
                        }

                        if (RemarkAccNo == null)
                        {
                            command.Parameters.AddWithValue("@RemarkAccNo", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@RemarkAccNo", RemarkAccNo);
                        }

                        if (ApprovalBarcodeComplete == null)
                        {
                            command.Parameters.AddWithValue("@ApprovalBarcodeComplete", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@ApprovalBarcodeComplete", ApprovalBarcodeComplete);
                        }

                        if (RemarkBarcodeComplete == null)
                        {
                            command.Parameters.AddWithValue("@RemarkBarcodeComplete", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@RemarkBarcodeComplete", RemarkBarcodeComplete);
                        }

                        if (ApprovalJIAvailable == null)
                        {
                            command.Parameters.AddWithValue("@ApprovalJIAvailable", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@ApprovalJIAvailable", ApprovalJIAvailable);
                        }

                        if (RemarkJIAvailable == null)
                        {
                            command.Parameters.AddWithValue("@RemarkJIAvailable", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@RemarkJIAvailable", RemarkJIAvailable);
                        }

                        if (ApprovalTestingResultEng == null)
                        {
                            command.Parameters.AddWithValue("@ApprovalTestingResultEng", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@ApprovalTestingResultEng", ApprovalTestingResultEng);
                        }

                        if (RemarkTestingResultEng == null)
                        {
                            command.Parameters.AddWithValue("@RemarkTestingResultEng", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@RemarkTestingResultEng", RemarkTestingResultEng);
                        }

                        if (TestingResultITApproval == null)
                        {
                            command.Parameters.AddWithValue("@TestingResultITApproval", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@TestingResultITApproval", TestingResultITApproval);
                        }

                        if (TestingResultITRemark == null)
                        {
                            command.Parameters.AddWithValue("@TestingResultITRemark", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@TestingResultITRemark", TestingResultITRemark);
                        }

                        if (Comment == null)
                        {
                            command.Parameters.AddWithValue("@Comment", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Comment", Comment);
                        }

                        if (CorrectiveAction == null)
                        {
                            command.Parameters.AddWithValue("@CorrectiveAction", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@CorrectiveAction", CorrectiveAction);
                        }

                        if (PaperTypeInput2 == null)
                        {
                            command.Parameters.AddWithValue("@PaperTypeInput2", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PaperTypeInput2", PaperTypeInput2);
                        }


                        command.ExecuteNonQuery();

                    }

                    //===================================================================================================================================================================================================================================

                    SqlCommand cmd = new SqlCommand("UPDATE QM SET RejectMBD=@RejectMBD1, RejectQuality=@RejectQuality1, RejectITO=@RejectITO1, ApprovalEngineering=@ApprovalEngineering, ApprovalITODoc=@ApprovalITODoc," +
                        "PlanningApproval=@PlanningApproval, ApprovalDDPOperation=@ApprovalDDPOperation,NameMBD=@NameMBD, NameITO=@NameITO, NameQ=@NameQ, NameEngineering=@NameEngineering, NameITODoc=@NameITODoc, " +
                        "NamePlanning=@NamePlanning, DDPOperationName=@DDPOperationName, CommentMBD=@CommentMBD, CommentITO=@CommentITO, CommentQ=@CommentQ, CommentEngineering=@CommentEngineering, CommentITODoc=@CommentITODoc, " +
                        "CommentPlanning=@CommentPlanning, CommentDDPOperation=@CommentDDPOperation WHERE JobInstructionId=@JobInstructionId1;", cn2);

                    cmd.Parameters.AddWithValue("@JobInstructionId1", Id);

                    if (String.IsNullOrEmpty(RejectMBD))
                    {
                        cmd.Parameters.AddWithValue("@RejectMBD1", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@RejectMBD1", RejectMBD);
                    }

                    if (String.IsNullOrEmpty(RejectQuality) )
                    {
                        cmd.Parameters.AddWithValue("@RejectQuality1", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@RejectQuality1", RejectQuality);
                    }

                    if (String.IsNullOrEmpty(RejectITO))
                    {
                        cmd.Parameters.AddWithValue("@RejectITO1", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@RejectITO1", RejectITO);
                    }

                    if (String.IsNullOrEmpty(ApprovalEngineering))
                    {
                        cmd.Parameters.AddWithValue("@ApprovalEngineering", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ApprovalEngineering", ApprovalEngineering);
                    }

                    if (String.IsNullOrEmpty(ApprovalITODoc))
                    {
                        cmd.Parameters.AddWithValue("@ApprovalITODoc", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ApprovalITODoc", ApprovalITODoc);
                    }

                    if (String.IsNullOrEmpty(PlanningApproval))
                    {
                        cmd.Parameters.AddWithValue("@PlanningApproval", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@PlanningApproval", PlanningApproval);
                    }

                    if (String.IsNullOrEmpty(ApprovalDDPOperation))
                    {
                        cmd.Parameters.AddWithValue("@ApprovalDDPOperation", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ApprovalDDPOperation", ApprovalDDPOperation);
                    }

                    if (NameMBD == null)
                    {
                        cmd.Parameters.AddWithValue("@NameMBD", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@NameMBD", NameMBD);
                    }

                    if (NameITO == null)
                    {
                        cmd.Parameters.AddWithValue("@NameITO", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@NameITO", NameITO);
                    }

                    if (NameQ == null)
                    {
                        cmd.Parameters.AddWithValue("@NameQ", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@NameQ", NameQ);
                    }

                    if (NameEngineering == null)
                    {
                        cmd.Parameters.AddWithValue("@NameEngineering", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@NameEngineering", NameEngineering);
                    }

                    if (NameITODoc == null)
                    {
                        cmd.Parameters.AddWithValue("@NameITODoc", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@NameITODoc", NameITODoc);
                    }

                    if (NamePlanning == null)
                    {
                        cmd.Parameters.AddWithValue("@NamePlanning", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@NamePlanning", NamePlanning);
                    }

                    if (NameDDPOperation == null)
                    {
                        cmd.Parameters.AddWithValue("@DDPOperationName", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@DDPOperationName", NameDDPOperation);
                    }

                    if (CommentMBD == null)
                    {
                        cmd.Parameters.AddWithValue("@CommentMBD", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CommentMBD", CommentMBD);
                    }

                    if (CommentITO == null)
                    {
                        cmd.Parameters.AddWithValue("@CommentITO", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CommentITO", CommentITO);
                    }

                    if (CommentQ == null)
                    {
                        cmd.Parameters.AddWithValue("@CommentQ", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CommentQ", CommentQ);
                    }

                    if (CommentEngineer == null)
                    {
                        cmd.Parameters.AddWithValue("@CommentEngineering", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CommentEngineering", CommentEngineer);
                    }

                    if (CommentITODoc == null)
                    {
                        cmd.Parameters.AddWithValue("@CommentITODoc", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CommentITODoc", CommentITODoc);
                    }

                    if (CommentPlanning == null)
                    {
                        cmd.Parameters.AddWithValue("@CommentPlanning", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CommentPlanning", CommentPlanning);
                    }

                    if (CommentDDPOperation == null)
                    {
                        cmd.Parameters.AddWithValue("@CommentDDPOperation", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CommentDDPOperation", CommentDDPOperation);
                    }

                    if (JobInstructionId == null)
                    {
                        cmd.Parameters.AddWithValue("@JobInstructionId", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@JobInstructionId", JobInstructionId);
                    }
                    cmd.ExecuteNonQuery();


                    SqlCommand cmd2 = new SqlCommand("UPDATE QM SET dateEngineering=@dateEngineering,dateITODoc=@dateITODoc,datePlanning=@datePlanning,dateDDPOperation=@dateDDPOperation,dateMBD=@dateMBD,dateITO=@dateITO,dateQ=@dateQ" +
                        ", ProcessPrinting=@ProcessPrinting, ProcessInserting=@ProcessInserting, ProcessSelfMailer=@ProcessSelfMailer, ProcessMMP=@ProcessMMP from QM WHERE JobInstructionId=@JobInstructionId2", cn2);

                    cmd2.Parameters.AddWithValue("@JobInstructionId2", Id);

                    if (dateEngineering == null)
                    {
                        cmd2.Parameters.AddWithValue("@dateEngineering", DBNull.Value);
                    }
                    else
                    {
                        cmd2.Parameters.AddWithValue("@dateEngineering", dateEngineering);
                    }

                    if (dateITODoc == null)
                    {
                        cmd2.Parameters.AddWithValue("@dateITODoc", DBNull.Value);
                    }
                    else
                    {
                        cmd2.Parameters.AddWithValue("@dateITODoc", dateITODoc);
                    }

                    if (datePlanning == null)
                    {
                        cmd2.Parameters.AddWithValue("@datePlanning", DBNull.Value);
                    }
                    else
                    {
                        cmd2.Parameters.AddWithValue("@datePlanning", datePlanning);
                    }

                    if (dateDDPOperation == null)
                    {
                        cmd2.Parameters.AddWithValue("@dateDDPOperation", DBNull.Value);
                    }
                    else
                    {
                        cmd2.Parameters.AddWithValue("@dateDDPOperation", dateDDPOperation);
                    }

                    if (dateMBD == null)
                    {
                        cmd2.Parameters.AddWithValue("@dateMBD", DBNull.Value);
                    }
                    else
                    {
                        cmd2.Parameters.AddWithValue("@dateMBD", dateMBD);
                    }

                    if (dateITO == null)
                    {
                        cmd2.Parameters.AddWithValue("@dateITO", DBNull.Value);
                    }
                    else
                    {
                        cmd2.Parameters.AddWithValue("@dateITO", dateITO);
                    }

                    if (dateQ == null)
                    {
                        cmd2.Parameters.AddWithValue("@dateQ", DBNull.Value);
                    }
                    else
                    {
                        cmd2.Parameters.AddWithValue("@dateQ", dateQ);
                    }

                    if (ProcessPrinting == null)
                    {
                        cmd2.Parameters.AddWithValue("@ProcessPrinting", DBNull.Value);
                    }
                    else
                    {
                        cmd2.Parameters.AddWithValue("@ProcessPrinting", ProcessPrinting);
                    }

                    if (ProcessInserting == null)
                    {
                        cmd2.Parameters.AddWithValue("@ProcessInserting", DBNull.Value);
                    }
                    else
                    {
                        cmd2.Parameters.AddWithValue("@ProcessInserting", ProcessInserting);
                    }

                    if (ProcessSelfMailer == null)
                    {
                        cmd2.Parameters.AddWithValue("@ProcessSelfMailer", DBNull.Value);
                    }
                    else
                    {
                        cmd2.Parameters.AddWithValue("@ProcessSelfMailer", ProcessSelfMailer);
                    }

                    if (ProcessMMP == null)
                    {
                        cmd2.Parameters.AddWithValue("@ProcessMMP", DBNull.Value);
                    }
                    else
                    {
                        cmd2.Parameters.AddWithValue("@ProcessMMP", ProcessMMP);
                    }

                    cmd2.ExecuteNonQuery();


                    //Debug.WriteLine("Check all status");
                    //Debug.WriteLine("Id : "+Id);

                    SqlCommand cmd3 = new SqlCommand("SELECT RejectMBD, RejectITO, RejectQuality, ApprovalEngineering, ApprovalITODoc, PlanningApproval, ApprovalDDPOperation FROM QM WHERE JobInstructionId=@JobInstructionId2", cn2);
                    cmd3.Parameters.AddWithValue("@JobInstructionId2", Id);
                    SqlDataReader rm3 = cmd3.ExecuteReader();

                    while (rm3.Read())
                    {
                        Debug.WriteLine("Read");
                        if (rm3.HasRows)
                        {
                            Debug.WriteLine("has row");
                            if (!rm3.IsDBNull(0))
                            {
                                Debug.WriteLine("not null 0");

                                if (!rm3.IsDBNull(1))
                                {
                                    Debug.WriteLine("not null 1");

                                    if (!rm3.IsDBNull(2))
                                    {
                                        Debug.WriteLine("not null 2");

                                        if (!rm3.IsDBNull(3))
                                        {
                                            Debug.WriteLine("not null 3");

                                            if (!rm3.IsDBNull(4))
                                            {
                                                Debug.WriteLine("not null 4");

                                                if (!rm3.IsDBNull(5))
                                                {
                                                    Debug.WriteLine("not null 5");

                                                    if (!rm3.IsDBNull(6))
                                                    {
                                                        Debug.WriteLine("not null 6 and execute");

                                                        SqlCommand cmd4 = new SqlCommand("UPDATE JobInstruction SET StatQM ='Verified', NMRStatus='COMPLETED' WHERE Id =@JobInstructionId3", cn2);
                                                        cmd4.Parameters.AddWithValue("@JobInstructionId3", Id);
                                                        cmd4.ExecuteNonQuery();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }


                    cn2.Close();

                    return RedirectToAction("ManageQM", "QM");

                }

                //using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                //{


                //    cn2.Open();
                //    SqlCommand command;
                //    //command = new SqlCommand("UPDATE [IflowSeed].[dbo].[QM]  SET ModifiedOn=@ModifiedOn,QMStatus=@QMStatus,RejectITO=@RejectITO,NameITO=@NameITO,CommentITO=@CommentITO,dateITO=@dateITO WHERE Id=@JobInstructionId", cn2);
                //    command = new SqlCommand("UPDATE [IflowSeed].[dbo].[QM]  SET QMStatus=@QMStatus,RejectITO=@RejectITO,NameITO=@NameITO,CommentITO=@CommentITO,dateITO=@dateITO WHERE Id=@JobInstructionId", cn2);

                //    //command.Parameters.AddWithValue("@ModifiedOn", createdOn);
                //    command.Parameters.AddWithValue("@JobInstructionId", get.Id);
                //    command.Parameters.AddWithValue("@QMStatus", "Progress");
                //    if (RejectITO == null)
                //    {
                //        command.Parameters.AddWithValue("@RejectITO", RejectITO != null ? (object)RejectITO : DBNull.Value);
                //    }
                //    else
                //    {
                //        command.Parameters.AddWithValue("@RejectITO", RejectITO);
                //    }
                //    command.Parameters.AddWithValue("@NameITO", NameITO);
                //    command.Parameters.AddWithValue("@CommentITO", CommentITO);
                //    command.Parameters.AddWithValue("@dateITO", DateITO);


                //    command.ExecuteNonQuery();
                //    cn2.Close();

                //    return RedirectToAction("ManageQM", "QM");

                //}
            }


            return View(logtagno);

        }



        public ActionResult AddeChannel3(string Id, string Customer_Name, string AttachEncry, string Templayout, string Corret, string CorretInfo, string Compages, string GoodQuality
           , string RejectQuality, string NameQ, string JobSheetNo, string CommentQ, string RejectITO, string NameITO, string CommentITO, string dateITO, string RejectMBD, string NameMBD, string CommentMBD, string dateMBD
           , string JobType, string RemarkAttachEncry, string CusApp, string RemarkCusApp, string RemarkTemplayout, string RemarkCorret, string RemarkCompages, string RemarkGoodQuality, string dateQ, string set, string ProductName, string RemarkCorretInfo, string set2
           , string JobInstructionId, JobInstruction get, string QMStatus)
        {

            var IdentityName = @Session["Fullname"];
            var Role = @Session["Role"];
            ViewBag.IsDepart = @Session["Department"];


            Session["Id"] = Id;
            ViewBag.Id = Id;





            List<SelectListItem> listproduct = new List<SelectListItem>();

            listproduct.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
            listproduct.Add(new SelectListItem { Text = "DDP", Value = "DDP" });
            listproduct.Add(new SelectListItem { Text = "RMS", Value = "RMS" });
            listproduct.Add(new SelectListItem { Text = "DOW", Value = "DOW" });
            listproduct.Add(new SelectListItem { Text = "OTHERS", Value = "OTHERS" });

            ViewData["ProductType_"] = listproduct;


            int _bil5 = 1;
            List<SelectListItem> li5 = new List<SelectListItem>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT DISTINCT Type FROM [IflowSeed].[dbo].[JobType]          
                                     ORDER BY Type";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JobInstruction model = new JobInstruction();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            model.JobType = reader.GetString(0);
                        }
                    }
                    int i = _bil5++;
                    if (i == 1)
                    {
                        li5.Add(new SelectListItem { Text = "Please Select" });
                        li5.Add(new SelectListItem { Text = model.JobType });

                    }
                    else
                    {
                        li5.Add(new SelectListItem { Text = model.JobType });
                    }
                }
                cn.Close();
            }
            ViewData["JobType_"] = li5;




            List<JobInstruction> JobInstructionlist1 = new List<JobInstruction>(); ;
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass, JobType,JobSheetNo, StartDevDate, EndDevDate
                                        FROM [IflowSeed].[dbo].[JobInstruction]
                                        WHERE Id =@Id";

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


                    }
                    JobInstructionlist1.Add(model);
                }
                cn.Close();

            }

            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn2))
            {
                int _bil2 = 1;
                cn2.Open();
                command.CommandText = @"SELECT QMITO.RejectITO, QMITO.CommentITO, QMITO.dateITO, QMITO.NameITO
                                      FROM  JobInstruction INNER JOIN
                                     QMITO ON JobInstruction.Id = QMITO.JobInstructionId
                                     where   JobInstruction.Id=@Id";

                command.Parameters.AddWithValue("@Id", Id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    QM_Model model = new QM_Model();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            ViewBag.RejectITO = reader.GetBoolean(0);
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            ViewBag.CommentITO = reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            ViewBag.dateITO = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(2));
                        }

                        if (reader.IsDBNull(3) == false)
                        {
                            ViewBag.NameITO = reader.GetString(3);
                        }
                    }
                }
            }

            //using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            //using (SqlCommand command = new SqlCommand("", cn2))
            //{
            //    int _bil2 = 1;
            //    cn2.Open();
            //    command.CommandText = @"SELECT QMMBD.RejectMBD, QMMBD.CommentMBD, QMMBD.dateMBD, QMMBD.NameMBD
            //                          FROM  JobInstruction INNER JOIN
            //                         QMMBD ON JobInstruction.Id = QMMBD.JobInstructionId
            //                         where   JobInstruction.Id=@Id";

            //    command.Parameters.AddWithValue("@Id", Id);
            //    var reader = command.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        QM_Model model = new QM_Model();
            //        {
            //            if (reader.IsDBNull(0) == false)
            //            {
            //                ViewBag.RejectMBD = reader.GetString(0);
            //            }
            //            if (reader.IsDBNull(1) == false)
            //            {
            //                ViewBag.CommentMBD = reader.GetString(1);
            //            }
            //            if (reader.IsDBNull(2) == false)
            //            {
            //                ViewBag.dateMBD = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(2));
            //            }

            //            if (reader.IsDBNull(3) == false)
            //            {
            //                ViewBag.NameMBD = reader.GetString(3);
            //            }
            //        }
            //    }
            //}






            List<QM_Model> QM_Model = new List<QM_Model>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT JobInstruction.Id,  QM.Customer_Name, QM.ProductName, QM.JobSheetNo, QM.JobType, QM.AttachEncry, QM.RemarkAttachEncry, QM.CusApp, QM.RemarkCusApp, QM.Templayout, QM.RemarkTemplayout, QM.Corret, QM.RemarkCorret, QM.CorretInfo, QM.RemarkCorretInfo, QM.Compages, QM.RemarkCompages, QM.GoodQuality, QM.RemarkGoodQuality, QM.RejectQuality, QM.NameQ, QM.CommentQ, QM.RejectITO, QM.NameITO, QM.CommentITO, QM.RejectMBD, QM.NameMBD, QM.CommentMBD, QM.dateQ, QM.dateITO,
                                            QM.dateMBD,QM.QMStatus,QM.Id
                                  FROM  JobInstruction INNER JOIN
                                  QM ON JobInstruction.Id = QM.JobInstructionId
                                     where   JobInstruction.Id=@Id";

                command.Parameters.AddWithValue("@Id", Id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    QM_Model model = new QM_Model();
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
                            ViewBag.JobSheetNo = reader.GetString(3);
                        }


                        if (reader.IsDBNull(4) == false)
                        {
                            ViewBag.JobType = reader.GetString(4);
                        }


                        if (reader.IsDBNull(5) == false)
                        {
                            bool getAttachEncry = reader.GetBoolean(5);
                            if (getAttachEncry == false)
                            {
                                ViewBag.AttachEncry = reader.GetBoolean(5);
                            }
                            else
                            {
                                ViewBag.AttachEncry = "checked";
                            }
                        }

                        if (reader.IsDBNull(6) == false)
                        {
                            ViewBag.RemarkAttachEncry = reader.GetString(6);
                        }


                        if (reader.IsDBNull(7) == false)
                        {
                            bool getCusApp = reader.GetBoolean(7);
                            if (getCusApp == false)
                            {
                                ViewBag.CusApp = "";
                            }
                            else
                            {
                                ViewBag.CusApp = "checked";
                            }
                        }

                        if (reader.IsDBNull(8) == false)
                        {
                            ViewBag.RemarkCusApp = reader.GetString(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            bool getTemplayout = reader.GetBoolean(9);
                            if (getTemplayout == false)
                            {
                                ViewBag.Templayout = "";
                            }
                            else
                            {
                                ViewBag.Templayout = "checked";
                            }
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            ViewBag.RemarkTemplayout = reader.GetString(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            bool getCorret = reader.GetBoolean(11);
                            if (getCorret == false)
                            {
                                ViewBag.Corret = "";
                            }
                            else
                            {
                                ViewBag.Corret = "checked";
                            }
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            ViewBag.RemarkCorret = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            bool getCorretInfo = reader.GetBoolean(13);
                            if (getCorretInfo == false)
                            {
                                ViewBag.CorretInfo = "";
                            }
                            else
                            {
                                ViewBag.CorretInfo = "checked";
                            }
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            ViewBag.RemarkCorretInfo = reader.GetString(14);
                        }

                        if (reader.IsDBNull(15) == false)
                        {
                            bool getCompages = reader.GetBoolean(15);
                            if (getCompages == false)
                            {
                                ViewBag.Compages = "";
                            }
                            else
                            {
                                ViewBag.Compages = "checked";
                            }
                        }
                        if (reader.IsDBNull(16) == false)
                        {
                            ViewBag.RemarkCompages = reader.GetString(16);
                        }

                        if (reader.IsDBNull(17) == false)
                        {
                            bool getGoodQuality = reader.GetBoolean(17);
                            if (getGoodQuality == false)
                            {
                                ViewBag.GoodQuality = "";
                            }
                            else
                            {
                                ViewBag.GoodQuality = "checked";
                            }
                        }
                        if (reader.IsDBNull(18) == false)
                        {
                            ViewBag.RemarkGoodQuality = reader.GetString(18);
                        }
                        if (reader.IsDBNull(19) == false)
                        {
                            bool getRejectQuality = reader.GetBoolean(19);
                            if (getRejectQuality == false)
                            {
                                ViewBag.RejectQuality = "";
                            }
                            else
                            {
                                ViewBag.RejectQuality = "checked";
                            }
                        }
                        if (reader.IsDBNull(20) == false)
                        {
                            ViewBag.NameQ = reader.GetString(20);
                        }
                        if (reader.IsDBNull(21) == false)
                        {
                            ViewBag.CommentQ = reader.GetString(21);
                        }

                        if (reader.IsDBNull(22) == false)
                        {
                            bool getRejectITO = reader.GetBoolean(22);
                            if (getRejectITO == false)
                            {
                                ViewBag.RejectITO = "";
                            }
                            else
                            {
                                ViewBag.RejectITO = "checked";
                            }
                        }
                        if (reader.IsDBNull(23) == false)
                        {
                            ViewBag.NameITO = reader.GetString(23);
                        }
                        if (reader.IsDBNull(24) == false)
                        {
                            ViewBag.CommentITO = reader.GetString(24);
                        }
                        if (reader.IsDBNull(25) == false)
                        {
                            bool getRejectMBD = reader.GetBoolean(25);
                            if (getRejectMBD == false)
                            {
                                ViewBag.RejectMBD = "";
                            }
                            else
                            {
                                ViewBag.RejectMBD = "checked";
                            }
                        }
                        if (reader.IsDBNull(26) == false)
                        {
                            ViewBag.NameMBD = reader.GetString(26);
                        }
                        if (reader.IsDBNull(27) == false)
                        {
                            ViewBag.CommentMBD = reader.GetString(27);
                        }
                        if (reader.IsDBNull(28) == false)
                        {
                            ViewBag.dateQ = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(28));
                            //model.DateQTxt = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(28));
                        }

                        if (reader.IsDBNull(29) == false)
                        {
                            ViewBag.dateITO = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(29));
                        }

                        if (reader.IsDBNull(30) == false)
                        {
                            ViewBag.dateMBD = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(30));
                        }
                        if (reader.IsDBNull(31) == false)
                        {
                            ViewBag.QMStatus = reader.GetString(31);
                        }
                    }
                    QM_Model.Add(model);
                }
                cn.Close();
            }


            if (set == "Verify")
            {
                Session["Id"] = Id;
                ViewBag.Id = Id;
                ViewBag.Customer_Name = Customer_Name;
                ViewBag.ProductName = ProductName;

                //get.DateQ = Convert.ToDateTime(get.DateQtxt);
                //get.DateITO = Convert.ToDateTime(get.DateITOtxt);
                //get.DateMBD = Convert.ToDateTime(get.DateMBDtxt);




                string DateQ = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt ");
                string DateITO = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt ");
                string DateMBD = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt ");


                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {


                    Guid guidId = Guid.NewGuid();
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                    cn2.Open();
                    SqlCommand command;
                    command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[QMMBD] ([Id],[ModifiedOn],[RejectMBD],[CommentMBD],[dateMBD],[JobInstructionId],[QMStatus],[NameMBD])" +
                                             "VALUES (@Id,@ModifiedOn,@RejectMBD,@CommentMBD,@dateMBD,@JobInstructionId,@QMStatus,@NameMBD)", cn2);
                    command.Parameters.AddWithValue("@Id", guidId);
                    command.Parameters.AddWithValue("@ModifiedOn", createdOn);
                    if (!string.IsNullOrEmpty(RejectITO))
                    {
                        command.Parameters.AddWithValue("@RejectMBD", RejectMBD);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RejectMBD", DBNull.Value);

                    }
                    command.Parameters.AddWithValue("@CommentMBD", CommentMBD);
                    command.Parameters.AddWithValue("@dateMBD", DateMBD);
                    command.Parameters.AddWithValue("@JobInstructionId", Id);
                    command.Parameters.AddWithValue("@QMStatus", "QMStatus");
                    command.Parameters.AddWithValue("@NameMBD", NameMBD);

                    command.ExecuteNonQuery();
                    cn2.Close();
                }
                using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                    cn3.Open();
                    SqlCommand command2;
                    command2 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction]  SET ModifiedOn=@ModifiedOn,StatQM=@StatQM WHERE Id=@Id", cn3);

                    command2.Parameters.AddWithValue("@ModifiedOn", createdOn);
                    command2.Parameters.AddWithValue("@Id", get.Id);
                    command2.Parameters.AddWithValue("@StatQM", "Success Verify");
                    command2.ExecuteNonQuery();
                    cn3.Close();



                }


                return RedirectToAction("ManageQM", "QM");

            }



            if (set == "Verify")
            {
                Session["Id"] = Id;
                ViewBag.Id = Id;
                ViewBag.Customer_Name = Customer_Name;
                ViewBag.ProductName = ProductName;


                //string DateQ2 = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                //string DateITO2 = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                //string DateMBD2 = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");



                using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn1.Open();
                    SqlCommand command1;
                    command1 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET ModifiedOn=@ModifiedOn WHERE Id=@Id", cn1);
                    command1.Parameters.AddWithValue("@Id", Id);
                    command1.Parameters.AddWithValue("@ModifiedOn", createdOn);
                    command1.ExecuteNonQuery();
                    cn1.Close();
                }



                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {


                    cn2.Open();
                    SqlCommand command;
                    command = new SqlCommand("UPDATE [IflowSeed].[dbo].[QM]  SET ModifiedOn=@ModifiedOn,QMStatus=@QMStatus,RejectITO=@RejectITO,NameITO=@NameITO,CommentITO=@CommentITO,dateITO=@dateITO WHERE Id=@JobInstructionId", cn2);
                    command.Parameters.AddWithValue("@ModifiedOn", createdOn);
                    command.Parameters.AddWithValue("@JobInstructionId", get.Id);
                    command.Parameters.AddWithValue("@QMStatus", "Progress");
                    command.Parameters.AddWithValue("@RejectITO", RejectITO);
                    command.Parameters.AddWithValue("@NameITO", NameITO);
                    command.Parameters.AddWithValue("@CommentITO", CommentITO);
                    command.Parameters.AddWithValue("@dateITO", dateITO);


                    command.ExecuteNonQuery();
                    cn2.Close();

                    return RedirectToAction("ManageQM", "QM");

                }
            }


            return View();

        }



        //public ActionResult ViewQM(string Id)
        //{
        //    Session["Id"] = Id;
        //    ViewBag.Id = Id;

        //    List<QM_Model> QM_Model = new List<QM_Model>(); ;
        //    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        //    using (SqlCommand command = new SqlCommand("", cn))
        //    {
        //        int _bil = 1;
        //        cn.Open();
        //        command.CommandText = @"SELECT JobInstruction.Id,  QM.Customer_Name, QM.ProductName, QM.JobSheetNo, QM.JobType, QM.AttachEncry, QM.RemarkAttachEncry, QM.CusApp, QM.RemarkCusApp, QM.Templayout, QM.RemarkTemplayout, QM.Corret, QM.RemarkCorret, QM.CorretInfo, QM.RemarkCorretInfo, QM.Compages, QM.RemarkCompages, QM.GoodQuality, QM.RemarkGoodQuality, QM.RejectQuality, QM.NameQ, QM.CommentQ, QM.RejectITO, QM.NameITO, QM.CommentITO, QM.RejectMBD, QM.NameMBD, QM.CommentMBD, QM.dateQ, QM.dateITO,
        //                                    QM.dateMBD,QM.QMStatus,QM.Id
        //                          FROM  JobInstruction INNER JOIN
        //                          QM ON JobInstruction.Id = QM.JobInstructionId
        //                          where   QM.JobInstructionId=@Id";

        //        command.Parameters.AddWithValue("@Id", Id);
        //        var reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            QM_Model model = new QM_Model();
        //            {
        //                model.Bil = _bil++;
        //                if (reader.IsDBNull(0) == false)
        //                {
        //                    ViewBag.Id = reader.GetGuid(0);
        //                }
        //                if (reader.IsDBNull(1) == false)
        //                {
        //                    ViewBag.Customer_Name = reader.GetString(1);
        //                }
        //                if (reader.IsDBNull(2) == false)
        //                {
        //                    ViewBag.ProductName = reader.GetString(2);
        //                }
        //                if (reader.IsDBNull(3) == false)
        //                {
        //                    ViewBag.JobSheetNo = reader.GetString(3);
        //                }


        //                if (reader.IsDBNull(4) == false)
        //                {
        //                    ViewBag.JobType = reader.GetString(4);
        //                }


        //                if (reader.IsDBNull(5) == false)
        //                {
        //                    bool getAttachEncry = reader.GetBoolean(7);
        //                    if (getAttachEncry == false)
        //                    {
        //                        ViewBag.AttachEncry = reader.GetBoolean(7);
        //                    }
        //                    else
        //                    {
        //                        ViewBag.AttachEncry = "checked";
        //                    }
        //                }

        //                if (reader.IsDBNull(6) == false)
        //                {
        //                    ViewBag.RemarkAttachEncry = reader.GetString(6);
        //                }


        //                if (reader.IsDBNull(7) == false)
        //                {
        //                    bool getCusApp = reader.GetBoolean(7);
        //                    if (getCusApp == false)
        //                    {
        //                        ViewBag.CusApp = "";
        //                    }
        //                    else
        //                    {
        //                        ViewBag.CusApp = "checked";
        //                    }
        //                }

        //                if (reader.IsDBNull(8) == false)
        //                {
        //                    ViewBag.RemarkCusApp = reader.GetString(8);
        //                }
        //                if (reader.IsDBNull(9) == false)
        //                {
        //                    bool getTemplayout = reader.GetBoolean(9);
        //                    if (getTemplayout == false)
        //                    {
        //                        ViewBag.Templayout = "";
        //                    }
        //                    else
        //                    {
        //                        ViewBag.Templayout = "checked";
        //                    }
        //                }
        //                if (reader.IsDBNull(10) == false)
        //                {
        //                    ViewBag.RemarkTemplayout = reader.GetString(10);
        //                }
        //                if (reader.IsDBNull(11) == false)
        //                {
        //                    bool getCorret = reader.GetBoolean(11);
        //                    if (getCorret == false)
        //                    {
        //                        ViewBag.Corret = "";
        //                    }
        //                    else
        //                    {
        //                        ViewBag.Corret = "checked";
        //                    }
        //                }
        //                if (reader.IsDBNull(12) == false)
        //                {
        //                    ViewBag.RemarkCorret = reader.GetString(12);
        //                }
        //                if (reader.IsDBNull(13) == false)
        //                {
        //                    bool getCorretInfo = reader.GetBoolean(13);
        //                    if (getCorretInfo == false)
        //                    {
        //                        ViewBag.CorretInfo = "";
        //                    }
        //                    else
        //                    {
        //                        ViewBag.CorretInfo = "checked";
        //                    }
        //                }
        //                if (reader.IsDBNull(14) == false)
        //                {
        //                    ViewBag.RemarkCorretInfo = reader.GetString(14);
        //                }

        //                if (reader.IsDBNull(15) == false)
        //                {
        //                    bool getCompages = reader.GetBoolean(15);
        //                    if (getCompages == false)
        //                    {
        //                        ViewBag.Compages = "";
        //                    }
        //                    else
        //                    {
        //                        ViewBag.Compages = "checked";
        //                    }
        //                }
        //                if (reader.IsDBNull(16) == false)
        //                {
        //                    ViewBag.RemarkCompages = reader.GetString(16);
        //                }

        //                if (reader.IsDBNull(17) == false)
        //                {
        //                    bool getGoodQuality = reader.GetBoolean(17);
        //                    if (getGoodQuality == false)
        //                    {
        //                        ViewBag.GoodQuality = "";
        //                    }
        //                    else
        //                    {
        //                        ViewBag.GoodQuality = "checked";
        //                    }
        //                }
        //                if (reader.IsDBNull(18) == false)
        //                {
        //                    ViewBag.RemarkGoodQuality = reader.GetString(18);
        //                }
        //                if (reader.IsDBNull(19) == false)
        //                {
        //                    bool getRejectQuality = reader.GetBoolean(19);
        //                    if (getRejectQuality == false)
        //                    {
        //                        ViewBag.RejectQuality = "";
        //                    }
        //                    else
        //                    {
        //                        ViewBag.RejectQuality = "checked";
        //                    }
        //                }
        //                if (reader.IsDBNull(20) == false)
        //                {
        //                    ViewBag.NameQ = reader.GetString(20);
        //                }
        //                if (reader.IsDBNull(21) == false)
        //                {
        //                    ViewBag.CommentQ = reader.GetString(21);
        //                }

        //                if (reader.IsDBNull(22) == false)
        //                {
        //                    bool getRejectITO = reader.GetBoolean(22);
        //                    if (getRejectITO == false)
        //                    {
        //                        ViewBag.RejectITO = "";
        //                    }
        //                    else
        //                    {
        //                        ViewBag.RejectITO = "checked";
        //                    }
        //                }
        //                if (reader.IsDBNull(23) == false)
        //                {
        //                    ViewBag.NameITO = reader.GetString(23);
        //                }
        //                if (reader.IsDBNull(24) == false)
        //                {
        //                    ViewBag.CommentITO = reader.GetString(24);
        //                }
        //                if (reader.IsDBNull(25) == false)
        //                {
        //                    bool getRejectMBD = reader.GetBoolean(25);
        //                    if (getRejectMBD == false)
        //                    {
        //                        ViewBag.RejectMBD = "";
        //                    }
        //                    else
        //                    {
        //                        ViewBag.RejectMBD = "checked";
        //                    }
        //                }
        //                if (reader.IsDBNull(26) == false)
        //                {
        //                    ViewBag.NameMBD = reader.GetString(26);
        //                }
        //                if (reader.IsDBNull(27) == false)
        //                {
        //                    ViewBag.CommentMBD = reader.GetString(27);
        //                }
        //                if (reader.IsDBNull(28) == false)
        //                {
        //                    ViewBag.dateQ = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(28));
        //                    //model.DateQTxt = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(28));
        //                }

        //                if (reader.IsDBNull(29) == false)
        //                {
        //                    ViewBag.dateITO = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(29));
        //                }

        //                if (reader.IsDBNull(30) == false)
        //                {
        //                    ViewBag.dateMBD = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(30));
        //                }
        //                if (reader.IsDBNull(31) == false)
        //                {
        //                    ViewBag.QMStatus = reader.GetString(31);
        //                }
        //            }
        //            QM_Model.Add(model);
        //        }
        //        cn.Close();


        //        return View();
        //    }



        //}


        public ActionResult SubmitPlanner(string Id, string JobInstructionId, string JobType)

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
                    command1 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET STATUS='PLANNER' WHERE Id=@Id", cn1);
                    command1.Parameters.AddWithValue("@Id", Id);
                    command1.ExecuteNonQuery();
                    cn1.Close();
                }

                using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn1.Open();
                    SqlCommand command1;
                    command1 = new SqlCommand("UPDATE [IflowSeed].[dbo].[QM] SET QMSTATUS='PRODUCTION' WHERE JobInstructionId=@JobInstructionId", cn1);
                    command1.Parameters.AddWithValue("@JobInstructionId", Id);
                    command1.ExecuteNonQuery();
                    cn1.Close();
                }
            }
            return RedirectToAction("ManageQM", "QM", new { Id = Session["Id"].ToString() });

        }

        public ActionResult SubmitITO(string Id, string JobInstructionId, string JobType)

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
                    command1 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET STATUS='PLANNER' WHERE Id=@Id", cn1);
                    command1.Parameters.AddWithValue("@Id", Id);
                    command1.ExecuteNonQuery();
                    cn1.Close();
                }

                using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn1.Open();
                    SqlCommand command1;
                    command1 = new SqlCommand("UPDATE [IflowSeed].[dbo].[QM] SET QMSTATUS='PRODUCTION' WHERE JobInstructionId=@JobInstructionId", cn1);
                    command1.Parameters.AddWithValue("@JobInstructionId", Id);
                    command1.ExecuteNonQuery();
                    cn1.Close();
                }
            }
            return RedirectToAction("ManageQM", "QM", new { Id = Session["Id"].ToString() });

        }

        public ActionResult ReloadFileStoreQME(string JobSheetNo)
        {
            var Id = Session["Id"];
            ViewBag.Id = Id;


            List<QM_Model> QM_Model = new List<QM_Model>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT JobInstruction.Id,  QM.Customer_Name, QM.ProductName, QM.JobSheetNo, QM.JobType
                                  FROM  JobInstruction INNER JOIN
                                  QM ON JobInstruction.Id = QM.JobInstructionId";
                command.Parameters.AddWithValue("@Id", Id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    QM_Model model = new QM_Model();
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
                            ViewBag.JobSheetNo = reader.GetString(3);
                            //JobSheetNo = reader.GetString(3);
                        }

                        if (reader.IsDBNull(4) == false)
                        {
                            ViewBag.JobType = reader.GetString(4);
                        }

                    }
                    QM_Model.Add(model);
                }

            }


            List<FileLoadQM> viewFileStore = new List<FileLoadQM>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {


                int _bil = 1;
                cn.Open();



                //command.CommandText = @"SELECT
                //                        SampleProduct.Picture_FileId AS SampleProduct_Picture_FileId,
                //                        QMFileStore.Id AS QMFileStore_Id,
                //                        QMFileStore.Type,
                //                        SampleProduct.JobSheetNo
                //                        FROM
                //                        [IflowSeed].[dbo].[QMFileStore]
                //                        INNER JOIN
                //                        SampleProduct
                //                        ON
                //                        SampleProduct.JobSheetNo = QMFileStore.JobSheetNo
                //                        WHERE
                //                        SampleProduct.JobSheetNo = @reloadJobSheetNo";


                command.CommandText = @"SELECT Picture_FileId,Id,Type
                                      FROM [IflowSeed].[dbo].[QMFileStore] 
                                      WHERE JobSheetNo = @reloadJobSheetNo";
                //command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@reloadJobSheetNo", JobSheetNo);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    FileLoadQM model = new FileLoadQM();
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
                        if (reader.IsDBNull(2) == false)
                        {
                            model.Type = reader.GetString(2);
                        }
                    }
                    viewFileStore.Add(model);
                }
                cn.Close();
                //return Json(new { data = viewFileStore }, JsonRequestBehavior.AllowGet);
                return Json(viewFileStore);
            }
        }


        public ActionResult ReloadFileStoreDDP()
        {
            var Id = Session["Id"];
            ViewBag.Id = Id;


            List<QM_Model> QM_Model = new List<QM_Model>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT JobInstruction.Id,  QM.Customer_Name, QM.ProductName, QM.JobSheetNo, QM.JobType
                                  FROM  JobInstruction INNER JOIN
                                  QM ON JobInstruction.Id = QM.JobInstructionId";
                command.Parameters.AddWithValue("@Id", Id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    QM_Model model = new QM_Model();
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
                            ViewBag.JobSheetNo = reader.GetString(3);
                        }

                        if (reader.IsDBNull(4) == false)
                        {
                            ViewBag.JobType = reader.GetString(4);
                        }

                    }
                    QM_Model.Add(model);
                }

            }


            List<FileLoadQM> viewFileStore = new List<FileLoadQM>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {


                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Picture_FileId,Id,Type
                                      FROM [IflowSeed].[dbo].[QMFileStore]
                                      WHERE QM=@Id And Type='DDP'
                                     ORDER BY CreatedOn DESC";
                command.Parameters.AddWithValue("@Id", Id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    FileLoadQM model = new FileLoadQM();
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
                        if (reader.IsDBNull(2) == false)
                        {
                            model.Type = reader.GetString(2);
                        }
                    }
                    viewFileStore.Add(model);
                }
                cn.Close();
                //return Json(new { data = viewFileStore }, JsonRequestBehavior.AllowGet);
                return Json(viewFileStore);
            }
        }



        public ActionResult DownloadFileQME(string Id)
        {
            Guid IdPartner = Guid.Empty;


            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Picture_FileId,Picture_Extension,QM
                                      FROM [IflowSeed].[dbo].[QMFileStore]
                                      WHERE Id=@Id";
                command.Parameters.AddWithValue("@Id", Id.ToString());
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        var Picture_FileId = reader.GetString(0);
                        var Picture_Extension = reader.GetString(1);
                        IdPartner = reader.GetGuid(2);
                        var path = PathSource + Picture_FileId;
                        string contentType = Picture_Extension.ToString();
                        return File(path, contentType, Picture_FileId);
                    }
                }
            }

            return RedirectToAction("AddeChannel", "QM", new { Id = IdPartner, status = "New" });
        }

        public ActionResult DownloadFileDDP(string Id)
        {
            Guid IdPartner = Guid.Empty;


            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Picture_FileId,Picture_Extension,QM
                                      FROM [IflowSeed].[dbo].[QMFileStore]
                                      WHERE Id=@Id";
                command.Parameters.AddWithValue("@Id", Id.ToString());
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        var Picture_FileId = reader.GetString(0);
                        var Picture_Extension = reader.GetString(1);
                        IdPartner = reader.GetGuid(2);
                        var path = PathSource + Picture_FileId;
                        string contentType = Picture_Extension.ToString();
                        return File(path, contentType, Picture_FileId);
                    }
                }
            }

            return RedirectToAction("AddDDP", "QM", new { Id = IdPartner, status = "New" });
        }


        public ActionResult UploadFileStoreQME(FileStoreUploaded FileUploadLocation, string Category, string JobSheetNo)
        {

            var Id = Session["Id"];
            ViewBag.Id = Id;


            ViewBag.IsDepart = @Session["Department"];
            ViewBag.IsRole = @Session["Role"];
            ViewBag.JobSheetNo = JobSheetNo;



            if (FileUploadLocation.FileUploadFile != null && FileUploadLocation.set == "save")
            {
                var fileName = Path.GetFileName(FileUploadLocation.FileUploadFile.FileName);
                var path = Path.Combine(Server.MapPath("~/FileStore"), fileName);
                FileUploadLocation.FileUploadFile.SaveAs(path);

                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    Guid guidId = Guid.NewGuid();
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                    cn2.Open();

                    SqlCommand command;
                    command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[QMFileStore] (Id,CreatedOn,Picture_FileId,Picture_Extension,Type,QM,JobSheetNo) values (@Id,@CreatedOn,@Picture_FileId,@Picture_Extension,@Type,@QM,@JobSheetNo)", cn2);
                    command.Parameters.AddWithValue("@Id", guidId);
                    command.Parameters.AddWithValue("@CreatedOn", createdOn);
                    command.Parameters.AddWithValue("@Picture_FileId", fileName.Trim());

                    command.Parameters.AddWithValue("@Picture_Extension", FileUploadLocation.FileUploadFile.ContentType);
                    command.Parameters.AddWithValue("@Type", "QME");
                    command.Parameters.AddWithValue("@QM", Id);
                    command.Parameters.AddWithValue("@JobSheetNo", JobSheetNo);

                    command.ExecuteNonQuery();
                    cn2.Close();

                    return RedirectToAction("AddeChannel2", "QM", new { Id = Id });
                }
            }
            if (FileUploadLocation.set == "back")
            {
                return RedirectToAction("AddeChannel2", "QM", new { Id = Id });
            }

            return View();
        }

        public ActionResult UploadFileStoreDDP(FileStoreUploaded FileUploadLocation, string Category)
        {

            var Id = Session["Id"];
            ViewBag.Id = Id;


            ViewBag.IsDepart = @Session["Department"];
            ViewBag.IsRole = @Session["Role"];



            if (FileUploadLocation.FileUploadFile != null && FileUploadLocation.set == "save")
            {
                var fileName = Path.GetFileName(FileUploadLocation.FileUploadFile.FileName);
                var path = Path.Combine(Server.MapPath("~/FileStore"), fileName);
                FileUploadLocation.FileUploadFile.SaveAs(path);

                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    Guid guidId = Guid.NewGuid();
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                    cn2.Open();

                    SqlCommand command;
                    command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[QMFileStore] (Id,CreatedOn,Picture_FileId,Picture_Extension,Type,QM) values (@Id,@CreatedOn,@Picture_FileId,@Picture_Extension,@Type,@QM)", cn2);
                    command.Parameters.AddWithValue("@Id", guidId);
                    command.Parameters.AddWithValue("@CreatedOn", createdOn);
                    command.Parameters.AddWithValue("@Picture_FileId", fileName.Trim());

                    command.Parameters.AddWithValue("@Picture_Extension", FileUploadLocation.FileUploadFile.ContentType);
                    command.Parameters.AddWithValue("@Type", "DDP");
                    command.Parameters.AddWithValue("@QM", Id);
                    command.ExecuteNonQuery();
                    cn2.Close();

                    return RedirectToAction("AddDDP", "QM", new { Id = Id });
                }
            }
            if (FileUploadLocation.set == "back")
            {
                return RedirectToAction("AddDDP", "QM", new { Id = Id });
            }

            return View();
        }



        public ActionResult DeleteFileStore(string Id)
        {
            Guid QMId = Guid.Empty;

            if (Id != null)
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    cn.Open();
                    command.CommandText = @"SELECT Picture_FileId,QM
                                      FROM [IflowSeed].[dbo].[QMFileStore]
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
                                command3 = new SqlCommand("DELETE [IflowSeed].[dbo].[QMFileStore]  WHERE Id=@Id", cn3);
                                command3.Parameters.AddWithValue("@Id", Id);
                                command3.ExecuteNonQuery();
                                cn3.Close();
                            }
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            QMId = reader.GetGuid(1);
                            Session["Id"] = QMId;

                            return RedirectToAction("ManageQM", "QM", new { Id = QMId });
                        }
                    }
                    cn.Close();
                }
            }

            return RedirectToAction("ManageQM", "QM", new { Id = QMId });
        }

        public ActionResult ViewQME(string JobInstructionId, string Id)
        {


            int _bil5 = 1;
            List<SelectListItem> li5 = new List<SelectListItem>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT DISTINCT Type FROM [IflowSeed].[dbo].[JobType]          
                                     ORDER BY Type";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JobInstruction model = new JobInstruction();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            model.JobType = reader.GetString(0);
                        }
                    }
                    int i = _bil5++;
                    if (i == 1)
                    {
                        li5.Add(new SelectListItem { Text = "Please Select" });
                        li5.Add(new SelectListItem { Text = model.JobType });

                    }
                    else
                    {
                        li5.Add(new SelectListItem { Text = model.JobType });
                    }
                }
                cn.Close();
            }
            ViewData["JobType_"] = li5;

            if (!string.IsNullOrEmpty(Id))
            {

                List<QM_Model> QM_Model = new List<QM_Model>(); ;
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    int _bil = 1;
                    cn.Open();
                    command.CommandText = @"SELECT JobInstruction.Id,  QM.Customer_Name, QM.ProductName, QM.JobSheetNo, QM.JobType, QM.AttachEncry, QM.RemarkAttachEncry, QM.CusApp, QM.RemarkCusApp, QM.Templayout, QM.RemarkTemplayout, QM.Corret, QM.RemarkCorret, QM.CorretInfo, QM.RemarkCorretInfo, QM.Compages, QM.RemarkCompages, QM.GoodQuality, QM.RemarkGoodQuality, QM.RejectQuality, QM.NameQ, QM.CommentQ, QM.RejectITO, QM.NameITO, QM.CommentITO, QM.RejectMBD, QM.NameMBD, QM.CommentMBD, QM.dateQ, QM.dateITO, 
                                            QM.dateMBD,QM.Id
                                  FROM  JobInstruction INNER JOIN
                                  QM ON JobInstruction.Id = QM.JobInstructionId
                                  where JobInstruction.JobType='E-BLAST'";
                    command.Parameters.AddWithValue("@Id", Id);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        QM_Model model = new QM_Model();
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
                                ViewBag.JobSheetNo = reader.GetString(3);
                            }



                            if (reader.IsDBNull(4) == false)
                            {
                                ViewBag.JobType = reader.GetString(4);
                            }


                            if (reader.IsDBNull(5) == false)
                            {
                                bool getAttachEncry = reader.GetBoolean(7);
                                if (getAttachEncry == false)
                                {
                                    ViewBag.AttachEncry = reader.GetBoolean(7);
                                }
                                else
                                {
                                    ViewBag.AttachEncry = "checked";
                                }
                            }

                            if (reader.IsDBNull(6) == false)
                            {
                                ViewBag.RemarkAttachEncry = reader.GetString(6);
                            }


                            if (reader.IsDBNull(7) == false)
                            {
                                bool getCusApp = reader.GetBoolean(7);
                                if (getCusApp == false)
                                {
                                    ViewBag.CusApp = "";
                                }
                                else
                                {
                                    ViewBag.CusApp = "checked";
                                }
                            }

                            if (reader.IsDBNull(8) == false)
                            {
                                ViewBag.RemarkCusApp = reader.GetString(8);
                            }
                            if (reader.IsDBNull(9) == false)
                            {
                                bool getTemplayout = reader.GetBoolean(9);
                                if (getTemplayout == false)
                                {
                                    ViewBag.Templayout = "";
                                }
                                else
                                {
                                    ViewBag.Templayout = "checked";
                                }
                            }
                            if (reader.IsDBNull(10) == false)
                            {
                                ViewBag.RemarkTemplayout = reader.GetString(10);
                            }
                            if (reader.IsDBNull(11) == false)
                            {
                                bool getCorret = reader.GetBoolean(11);
                                if (getCorret == false)
                                {
                                    ViewBag.Corret = "";
                                }
                                else
                                {
                                    ViewBag.Corret = "checked";
                                }
                            }
                            if (reader.IsDBNull(12) == false)
                            {
                                ViewBag.RemarkCorret = reader.GetString(12);
                            }
                            if (reader.IsDBNull(13) == false)
                            {
                                bool getCorretInfo = reader.GetBoolean(13);
                                if (getCorretInfo == false)
                                {
                                    ViewBag.CorretInfo = "";
                                }
                                else
                                {
                                    ViewBag.CorretInfo = "checked";
                                }
                            }
                            if (reader.IsDBNull(14) == false)
                            {
                                ViewBag.RemarkCorretInfo = reader.GetString(14);
                            }

                            if (reader.IsDBNull(15) == false)
                            {
                                bool getCompages = reader.GetBoolean(15);
                                if (getCompages == false)
                                {
                                    ViewBag.Compages = "";
                                }
                                else
                                {
                                    ViewBag.Compages = "checked";
                                }
                            }
                            if (reader.IsDBNull(16) == false)
                            {
                                ViewBag.RemarkCompages = reader.GetString(16);
                            }

                            if (reader.IsDBNull(17) == false)
                            {
                                bool getGoodQuality = reader.GetBoolean(17);
                                if (getGoodQuality == false)
                                {
                                    ViewBag.GoodQuality = "";
                                }
                                else
                                {
                                    ViewBag.GoodQuality = "checked";
                                }
                            }
                            if (reader.IsDBNull(18) == false)
                            {
                                ViewBag.RemarkGoodQuality = reader.GetString(18);
                            }
                            if (reader.IsDBNull(19) == false)
                            {
                                bool getRejectQuality = reader.GetBoolean(19);
                                if (getRejectQuality == false)
                                {
                                    ViewBag.RejectQuality = "";
                                }
                                else
                                {
                                    ViewBag.RejectQuality = "checked";
                                }
                            }
                            if (reader.IsDBNull(20) == false)
                            {
                                ViewBag.NameQ = reader.GetString(20);
                            }
                            if (reader.IsDBNull(21) == false)
                            {
                                ViewBag.CommentQ = reader.GetString(21);
                            }

                            if (reader.IsDBNull(22) == false)
                            {
                                bool getRejectITO = reader.GetBoolean(22);
                                if (getRejectITO == false)
                                {
                                    ViewBag.RejectITO = "";
                                }
                                else
                                {
                                    ViewBag.RejectITO = "checked";
                                }
                            }
                            if (reader.IsDBNull(23) == false)
                            {
                                ViewBag.NameITO = reader.GetString(23);
                            }
                            if (reader.IsDBNull(24) == false)
                            {
                                ViewBag.CommentITO = reader.GetString(24);
                            }
                            if (reader.IsDBNull(25) == false)
                            {
                                bool getRejectMBD = reader.GetBoolean(25);
                                if (getRejectMBD == false)
                                {
                                    ViewBag.RejectMBD = "";
                                }
                                else
                                {
                                    ViewBag.RejectMBD = "checked";
                                }
                            }
                            if (reader.IsDBNull(26) == false)
                            {
                                ViewBag.NameMBD = reader.GetString(26);
                            }
                            if (reader.IsDBNull(27) == false)
                            {
                                ViewBag.CommentMBD = reader.GetString(27);
                            }
                            if (reader.IsDBNull(28) == false)
                            {
                                ViewBag.dateQ = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(28));
                                //model.DateQTxt = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(28));
                            }

                            if (reader.IsDBNull(29) == false)
                            {
                                ViewBag.dateITO = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(29));
                            }

                            if (reader.IsDBNull(30) == false)
                            {
                                ViewBag.dateMBD = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(30));
                            }
                        }
                        QM_Model.Add(model);
                    }
                    cn.Close();



                }

            }
            return View();


        }

        public ActionResult ManageQMDDP(string Id, string ProductName, string product, string set, string Status)
        {
            var IdentityName = @Session["Fullname"];
            var Role = @Session["Role"];
            var IsDepart = @Session["Department"];
            Session["Id"] = Id;
            ViewBag.Id = Id;

            if (IsDepart.ToString() == "QM")
            {
                ViewBag.IsSet = "OpenQM";
            }
            else if (IsDepart.ToString() == "IT")
            {
                ViewBag.IsSet = "OpenITO";
            }
            else
            {
                ViewBag.IsSet = "OpenMBD";
            }


            List<JobInstruction> JobInstructionlist1 = new List<JobInstruction>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {

                int _bil = 1;
                cn.Open();
                if (set == "search")
                {
                    command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass, JobType,JobSheetNo, StartDevDate, EndDevDate,StatQM
                                         FROM [IflowSeed].[dbo].[JobInstruction]                                    
                                         WHERE ProductName LIKE @ProductName
                                         AND Status = 'QME' AND JobType !='E-Blast AND JobType!='PDF GENERATOR'
                                         ORDER BY CreatedOn desc ";

                    command.Parameters.AddWithValue("@ProductName", "%" + product + "%");
                }

                else
                {
                    command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass, JobType,JobSheetNo, StartDevDate, EndDevDate,StatQM
                                        FROM [IflowSeed].[dbo].[JobInstruction]
                                        WHERE Status = 'QME' AND JobType !='E-Blast' AND JobType!='PDF GENERATOR'";
                }

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JobInstruction model = new JobInstruction();
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
                            model.StartDevOn = reader.GetDateTime(6);
                        }

                        if (reader.IsDBNull(7) == false)
                        {
                            model.EndDevDate = reader.GetDateTime(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.StatQM = reader.GetString(8);
                        }
                        else
                        {
                            model.StatQM = "Waiting to be verified";

                        }
                    }
                    JobInstructionlist1.Add(model);
                }
                cn.Close();
            }

            return View(JobInstructionlist1);
        }


        public ActionResult AddDDP(string Id, string Customer_Name, string AttachEncry, string Templayout, string Corret, string CorretInfo, string Compages, string GoodQuality
           , string RejectQuality, string NameQ, string JobSheetNo, string CommentQ, string RejectITO, string NameITO, string CommentITO, string dateITO, string RejectMBD, string NameMBD, string CommentMBD, string dateMBD
           , string JobType, string RemarkAttachEncry, string CusApp, string RemarkCusApp, string RemarkTemplayout, string RemarkCorret, string RemarkCompages, string RemarkGoodQuality, string dateQ, string set, string ProductName, string RemarkCorretInfo
           , string JobInstructionId, string DateQ2, string DateITO2, string DateMBD2, JobInstruction get)
        {


            var IdentityName = @Session["Fullname"];
            var Role = @Session["Role"];
            ViewBag.IsDepart = @Session["Department"];


            Session["Id"] = Id;
            ViewBag.Id = Id;

            List<SelectListItem> listproduct = new List<SelectListItem>();

            listproduct.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
            listproduct.Add(new SelectListItem { Text = "DDP", Value = "DDP" });
            listproduct.Add(new SelectListItem { Text = "RMS", Value = "RMS" });
            listproduct.Add(new SelectListItem { Text = "DOW", Value = "DOW" });
            listproduct.Add(new SelectListItem { Text = "OTHERS", Value = "OTHERS" });

            ViewData["ProductType_"] = listproduct;


            int _bil5 = 1;
            List<SelectListItem> li5 = new List<SelectListItem>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT DISTINCT Type FROM [IflowSeed].[dbo].[JobType]          
                                     ORDER BY Type";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JobInstruction model = new JobInstruction();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            model.JobType = reader.GetString(0);
                        }
                    }
                    int i = _bil5++;
                    if (i == 1)
                    {
                        li5.Add(new SelectListItem { Text = "Please Select" });
                        li5.Add(new SelectListItem { Text = model.JobType });

                    }
                    else
                    {
                        li5.Add(new SelectListItem { Text = model.JobType });
                    }
                }
                cn.Close();
            }
            ViewData["JobType_"] = li5;




            List<JobInstruction> JobInstructionlist1 = new List<JobInstruction>(); ;
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass, JobType,JobSheetNo, StartDevDate, EndDevDate
                                        FROM [IflowSeed].[dbo].[JobInstruction]
                                        WHERE Id =@Id";

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


                    }
                    JobInstructionlist1.Add(model);
                }
                cn.Close();



            }


            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn2))
            {
                int _bil2 = 1;
                cn2.Open();
                command.CommandText = @"SELECT QMITO.RejectITO, QMITO.CommentITO, QMITO.dateITO, QMITO.NameITO
                                      FROM  JobInstruction INNER JOIN
                                     QMITO ON JobInstruction.Id = QMITO.JobInstructionId
                                     where   JobInstruction.Id=@Id";

                command.Parameters.AddWithValue("@Id", Id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    QM_Model model = new QM_Model();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            ViewBag.RejectITO = reader.GetBoolean(0);
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            ViewBag.CommentITO = reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            ViewBag.dateITO = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(2));
                        }

                        if (reader.IsDBNull(3) == false)
                        {
                            ViewBag.NameITO = reader.GetString(3);
                        }
                    }
                }
            }




            List<QM_Model> QM_Model = new List<QM_Model>(); ;
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT JobInstruction.Id,  QM.Customer_Name, QM.ProductName, QM.JobSheetNo, QM.JobType, QM.AttachEncry, QM.RemarkAttachEncry, QM.CusApp, QM.RemarkCusApp, QM.Templayout, QM.RemarkTemplayout, QM.Corret, QM.RemarkCorret, QM.CorretInfo, QM.RemarkCorretInfo, QM.Compages, QM.RemarkCompages, QM.GoodQuality, QM.RemarkGoodQuality, QM.RejectQuality, QM.NameQ, QM.CommentQ, QM.RejectITO, QM.NameITO, QM.CommentITO, QM.RejectMBD, QM.NameMBD, QM.CommentMBD, QM.dateQ, QM.dateITO,
                                            QM.dateMBD,QM.QMStatus,QM.Id
                                  FROM  JobInstruction INNER JOIN
                                  QM ON JobInstruction.Id = QM.JobInstructionId
                                     where   JobInstruction.Id=@Id";

                command.Parameters.AddWithValue("@Id", Id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    QM_Model model = new QM_Model();
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
                            ViewBag.JobSheetNo = reader.GetString(3);
                        }


                        if (reader.IsDBNull(4) == false)
                        {
                            ViewBag.JobType = reader.GetString(4);
                        }


                        if (reader.IsDBNull(5) == false)
                        {
                            bool getAttachEncry = reader.GetBoolean(7);
                            if (getAttachEncry == false)
                            {
                                ViewBag.AttachEncry = reader.GetBoolean(7);
                            }
                            else
                            {
                                ViewBag.AttachEncry = "checked";
                            }
                        }

                        if (reader.IsDBNull(6) == false)
                        {
                            ViewBag.RemarkAttachEncry = reader.GetString(6);
                        }


                        if (reader.IsDBNull(7) == false)
                        {
                            bool getCusApp = reader.GetBoolean(7);
                            if (getCusApp == false)
                            {
                                ViewBag.CusApp = "";
                            }
                            else
                            {
                                ViewBag.CusApp = "checked";
                            }
                        }

                        if (reader.IsDBNull(8) == false)
                        {
                            ViewBag.RemarkCusApp = reader.GetString(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            bool getTemplayout = reader.GetBoolean(9);
                            if (getTemplayout == false)
                            {
                                ViewBag.Templayout = "";
                            }
                            else
                            {
                                ViewBag.Templayout = "checked";
                            }
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            ViewBag.RemarkTemplayout = reader.GetString(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            bool getCorret = reader.GetBoolean(11);
                            if (getCorret == false)
                            {
                                ViewBag.Corret = "";
                            }
                            else
                            {
                                ViewBag.Corret = "checked";
                            }
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            ViewBag.RemarkCorret = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            bool getCorretInfo = reader.GetBoolean(13);
                            if (getCorretInfo == false)
                            {
                                ViewBag.CorretInfo = "";
                            }
                            else
                            {
                                ViewBag.CorretInfo = "checked";
                            }
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            ViewBag.RemarkCorretInfo = reader.GetString(14);
                        }

                        if (reader.IsDBNull(15) == false)
                        {
                            bool getCompages = reader.GetBoolean(15);
                            if (getCompages == false)
                            {
                                ViewBag.Compages = "";
                            }
                            else
                            {
                                ViewBag.Compages = "checked";
                            }
                        }
                        if (reader.IsDBNull(16) == false)
                        {
                            ViewBag.RemarkCompages = reader.GetString(16);
                        }

                        if (reader.IsDBNull(17) == false)
                        {
                            bool getGoodQuality = reader.GetBoolean(17);
                            if (getGoodQuality == false)
                            {
                                ViewBag.GoodQuality = "";
                            }
                            else
                            {
                                ViewBag.GoodQuality = "checked";
                            }
                        }
                        if (reader.IsDBNull(18) == false)
                        {
                            ViewBag.RemarkGoodQuality = reader.GetString(18);
                        }
                        if (reader.IsDBNull(19) == false)
                        {
                            bool getRejectQuality = reader.GetBoolean(19);
                            if (getRejectQuality == false)
                            {
                                ViewBag.RejectQuality = "";
                            }
                            else
                            {
                                ViewBag.RejectQuality = "checked";
                            }
                        }
                        if (reader.IsDBNull(20) == false)
                        {
                            ViewBag.NameQ = reader.GetString(20);
                        }
                        if (reader.IsDBNull(21) == false)
                        {
                            ViewBag.CommentQ = reader.GetString(21);
                        }

                        if (reader.IsDBNull(22) == false)
                        {
                            bool getRejectITO = reader.GetBoolean(22);
                            if (getRejectITO == false)
                            {
                                ViewBag.RejectITO = "";
                            }
                            else
                            {
                                ViewBag.RejectITO = "checked";
                            }
                        }
                        if (reader.IsDBNull(23) == false)
                        {
                            ViewBag.NameITO = reader.GetString(23);
                        }
                        if (reader.IsDBNull(24) == false)
                        {
                            ViewBag.CommentITO = reader.GetString(24);
                        }
                        if (reader.IsDBNull(25) == false)
                        {
                            bool getRejectMBD = reader.GetBoolean(25);
                            if (getRejectMBD == false)
                            {
                                ViewBag.RejectMBD = "";
                            }
                            else
                            {
                                ViewBag.RejectMBD = "checked";
                            }
                        }
                        if (reader.IsDBNull(26) == false)
                        {
                            ViewBag.NameMBD = reader.GetString(26);
                        }
                        if (reader.IsDBNull(27) == false)
                        {
                            ViewBag.CommentMBD = reader.GetString(27);
                        }
                        if (reader.IsDBNull(28) == false)
                        {
                            ViewBag.dateQ = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(28));
                            //model.DateQTxt = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(28));
                        }

                        if (reader.IsDBNull(29) == false)
                        {
                            ViewBag.dateITO = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(29));
                        }

                        if (reader.IsDBNull(30) == false)
                        {
                            ViewBag.dateMBD = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(30));
                        }
                        if (reader.IsDBNull(31) == false)
                        {
                            ViewBag.QMStatus = reader.GetString(31);
                        }
                    }
                    QM_Model.Add(model);
                }
                cn.Close();
            }


            if (set == "AddNew")
            {
                Session["Id"] = Id;
                ViewBag.Id = Id;
                ViewBag.Customer_Name = Customer_Name;
                ViewBag.ProductName = ProductName;
                ViewBag.IsDepart = @Session["Department"];

                //get.DateQ = Convert.ToDateTime(get.DateQtxt);
                //get.DateITO = Convert.ToDateTime(get.DateITOtxt);
                //get.DateMBD = Convert.ToDateTime(get.DateMBDtxt);
                //string DateQ2;
                //string DateITO2;
                //string DateMBD2;

                string DateQ = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt ");
                string DateITO = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt ");
                string DateMBD = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt ");


              



                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    Guid guidId = Guid.NewGuid();
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                    cn2.Open();
                    SqlCommand command;
                    command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[QM] ([Id],[Customer_Name],[ProductName],[JobSheetNo],[JobType],[AttachEncry],[RemarkAttachEncry] ,[CusApp] ,[RemarkCusApp],[Templayout],[RemarkTemplayout] ,[Corret],[RemarkCorret],[CorretInfo],[RemarkCorretInfo],[Compages],[RemarkCompages] ,[GoodQuality] ,[RemarkGoodQuality],[RejectQuality],[NameQ],[CommentQ],[dateQ],[JobInstructionId],[QMStatus])" +
                                             "VALUES (@Id,@Customer_Name,@ProductName,@JobSheetNo,@JobType,@AttachEncry,@RemarkAttachEncry,@CusApp,@RemarkCusApp,@Templayout,@RemarkTemplayout,@Corret,@RemarkCorret,@CorretInfo,@RemarkCorretInfo,@Compages,@RemarkCompages,@GoodQuality,@RemarkGoodQuality,@RejectQuality,@NameQ,@CommentQ,@dateQ,@JobInstructionId,@QMStatus)", cn2);
                    command.Parameters.AddWithValue("@Id", guidId);
                    command.Parameters.AddWithValue("@CreateOn", createdOn);
                    command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                    command.Parameters.AddWithValue("@ProductName", ProductName);
                    command.Parameters.AddWithValue("@JobSheetNo", JobSheetNo);
                    command.Parameters.AddWithValue("@JobType", JobType);
                    if (!string.IsNullOrEmpty(AttachEncry))
                    {
                        command.Parameters.AddWithValue("@AttachEncry", AttachEncry);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@AttachEncry", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(RemarkAttachEncry))
                    {
                        command.Parameters.AddWithValue("@RemarkAttachEncry", RemarkAttachEncry);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RemarkAttachEncry", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(CusApp))
                    {
                        command.Parameters.AddWithValue("@CusApp", CusApp);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@CusApp", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(RemarkCusApp))
                    {
                        command.Parameters.AddWithValue("@RemarkCusApp", RemarkCusApp);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RemarkCusApp", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(Templayout))
                    {
                        command.Parameters.AddWithValue("@Templayout", Templayout);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Templayout", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(RemarkTemplayout))
                    {
                        command.Parameters.AddWithValue("@RemarkTemplayout", RemarkTemplayout);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RemarkTemplayout", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(Corret))
                    {
                        command.Parameters.AddWithValue("@Corret", Corret);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Corret", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(RemarkCorret))
                    {
                        command.Parameters.AddWithValue("@RemarkCorret", RemarkCorret);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RemarkCorret", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(CorretInfo))
                    {
                        command.Parameters.AddWithValue("@CorretInfo", CorretInfo);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@CorretInfo", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(RemarkCorretInfo))
                    {

                        command.Parameters.AddWithValue("@RemarkCorretInfo", RemarkCorretInfo);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RemarkCorretInfo", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(Compages))
                    {
                        command.Parameters.AddWithValue("@Compages", Compages);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Compages", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(RemarkCompages))
                    {
                        command.Parameters.AddWithValue("@RemarkCompages", RemarkCompages);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RemarkCompages", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(GoodQuality))
                    {
                        command.Parameters.AddWithValue("@GoodQuality", GoodQuality);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@GoodQuality", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(RemarkGoodQuality))
                    {
                        command.Parameters.AddWithValue("@RemarkGoodQuality", RemarkGoodQuality);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RemarkGoodQuality", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(RejectQuality))
                    {
                        command.Parameters.AddWithValue("@RejectQuality", RejectQuality);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RejectQuality", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(NameQ))
                    {
                        command.Parameters.AddWithValue("@NameQ", NameQ);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@NameQ", DBNull.Value);

                    }
                    if (!string.IsNullOrEmpty(CommentQ))
                    {
                        command.Parameters.AddWithValue("@CommentQ", CommentQ);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@CommentQ", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(DateQ))
                    {

                        command.Parameters.AddWithValue("@dateQ", DateQ);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@dateQ", DBNull.Value);
                    }                                

                   

                    command.Parameters.AddWithValue("@JobInstructionId", Id);
                    command.Parameters.AddWithValue("@QMStatus", "Progress");
                    command.ExecuteNonQuery();
                    cn2.Close();

                }

                using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                    cn3.Open();
                    SqlCommand command2;
                    command2 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction]  SET ModifiedOn=@ModifiedOn,StatQM=@StatQM WHERE Id=@Id", cn3);

                    command2.Parameters.AddWithValue("@ModifiedOn", createdOn);
                    command2.Parameters.AddWithValue("@Id", get.Id);
                    command2.Parameters.AddWithValue("@StatQM", "Verify");
                    command2.ExecuteNonQuery();
                    cn3.Close();



                }


                return RedirectToAction("ManageQMDDP", "QM");

            }
            return View();


        }



        public ActionResult AddDDP2(string Id, string Customer_Name, string AttachEncry, string Templayout, string Corret, string CorretInfo, string Compages, string GoodQuality
          , string RejectQuality, string NameQ, string JobSheetNo, string CommentQ, string RejectITO, string NameITO, string CommentITO, string dateITO, string RejectMBD, string NameMBD, string CommentMBD, string dateMBD
          , string JobType, string RemarkAttachEncry, string CusApp, string RemarkCusApp, string RemarkTemplayout, string RemarkCorret, string RemarkCompages, string RemarkGoodQuality, string dateQ, string set, string ProductName, string RemarkCorretInfo, string set2
          , string JobInstructionId, JobInstruction get, string QMStatus)
        {

            var IdentityName = @Session["Fullname"];
            var Role = @Session["Role"];
            ViewBag.IsDepart = @Session["Department"];


            Session["Id"] = Id;
            ViewBag.Id = Id;





            List<SelectListItem> listproduct = new List<SelectListItem>();

            listproduct.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
            listproduct.Add(new SelectListItem { Text = "DDP", Value = "DDP" });
            listproduct.Add(new SelectListItem { Text = "RMS", Value = "RMS" });
            listproduct.Add(new SelectListItem { Text = "DOW", Value = "DOW" });
            listproduct.Add(new SelectListItem { Text = "OTHERS", Value = "OTHERS" });

            ViewData["ProductType_"] = listproduct;


            int _bil5 = 1;
            List<SelectListItem> li5 = new List<SelectListItem>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT DISTINCT Type FROM [IflowSeed].[dbo].[JobType]          
                                     ORDER BY Type";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JobInstruction model = new JobInstruction();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            model.JobType = reader.GetString(0);
                        }
                    }
                    int i = _bil5++;
                    if (i == 1)
                    {
                        li5.Add(new SelectListItem { Text = "Please Select" });
                        li5.Add(new SelectListItem { Text = model.JobType });

                    }
                    else
                    {
                        li5.Add(new SelectListItem { Text = model.JobType });
                    }
                }
                cn.Close();
            }
            ViewData["JobType_"] = li5;




            List<JobInstruction> JobInstructionlist1 = new List<JobInstruction>(); ;
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass, JobType,JobSheetNo, StartDevDate, EndDevDate
                                        FROM [IflowSeed].[dbo].[JobInstruction]
                                        WHERE Id =@Id";

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


                    }
                    JobInstructionlist1.Add(model);
                }
                cn.Close();

            }

            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn2))
            {
                int _bil2 = 1;
                cn2.Open();
                command.CommandText = @"SELECT QMITO.RejectITO, QMITO.CommentITO, QMITO.dateITO, QMITO.NameITO
                                      FROM  JobInstruction INNER JOIN
                                     QMITO ON JobInstruction.Id = QMITO.JobInstructionId
                                     where   JobInstruction.Id=@Id";

                command.Parameters.AddWithValue("@Id", Id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    QM_Model model = new QM_Model();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            ViewBag.RejectITO = reader.GetString(0);
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            ViewBag.CommentITO = reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            ViewBag.dateITO = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(2));
                        }

                        if (reader.IsDBNull(3) == false)
                        {
                            ViewBag.NameITO = reader.GetString(3);
                        }
                    }
                }
            }







            List<QM_Model> QM_Model = new List<QM_Model>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT JobInstruction.Id,  QM.Customer_Name, QM.ProductName, QM.JobSheetNo, QM.JobType, QM.AttachEncry, QM.RemarkAttachEncry, QM.CusApp, QM.RemarkCusApp, QM.Templayout, QM.RemarkTemplayout, QM.Corret, QM.RemarkCorret, QM.CorretInfo, QM.RemarkCorretInfo, QM.Compages, QM.RemarkCompages, QM.GoodQuality, QM.RemarkGoodQuality, QM.RejectQuality, QM.NameQ, QM.CommentQ, QM.RejectITO, QM.NameITO, QM.CommentITO, QM.RejectMBD, QM.NameMBD, QM.CommentMBD, QM.dateQ, QM.dateITO,
                                            QM.dateMBD,QM.QMStatus,QM.Id
                                  FROM  JobInstruction INNER JOIN
                                  QM ON JobInstruction.Id = QM.JobInstructionId
                                     where   JobInstruction.Id=@Id";

                command.Parameters.AddWithValue("@Id", Id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    QM_Model model = new QM_Model();
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
                            ViewBag.JobSheetNo = reader.GetString(3);
                        }


                        if (reader.IsDBNull(4) == false)
                        {
                            ViewBag.JobType = reader.GetString(4);
                        }


                        if (reader.IsDBNull(5) == false)
                        {
                            bool getAttachEncry = reader.GetBoolean(7);
                            if (getAttachEncry == false)
                            {
                                ViewBag.AttachEncry = reader.GetBoolean(7);
                            }
                            else
                            {
                                ViewBag.AttachEncry = "checked";
                            }
                        }

                        if (reader.IsDBNull(6) == false)
                        {
                            ViewBag.RemarkAttachEncry = reader.GetString(6);
                        }


                        if (reader.IsDBNull(7) == false)
                        {
                            bool getCusApp = reader.GetBoolean(7);
                            if (getCusApp == false)
                            {
                                ViewBag.CusApp = "";
                            }
                            else
                            {
                                ViewBag.CusApp = "checked";
                            }
                        }

                        if (reader.IsDBNull(8) == false)
                        {
                            ViewBag.RemarkCusApp = reader.GetString(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            bool getTemplayout = reader.GetBoolean(9);
                            if (getTemplayout == false)
                            {
                                ViewBag.Templayout = "";
                            }
                            else
                            {
                                ViewBag.Templayout = "checked";
                            }
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            ViewBag.RemarkTemplayout = reader.GetString(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            bool getCorret = reader.GetBoolean(11);
                            if (getCorret == false)
                            {
                                ViewBag.Corret = "";
                            }
                            else
                            {
                                ViewBag.Corret = "checked";
                            }
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            ViewBag.RemarkCorret = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            bool getCorretInfo = reader.GetBoolean(13);
                            if (getCorretInfo == false)
                            {
                                ViewBag.CorretInfo = "";
                            }
                            else
                            {
                                ViewBag.CorretInfo = "checked";
                            }
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            ViewBag.RemarkCorretInfo = reader.GetString(14);
                        }

                        if (reader.IsDBNull(15) == false)
                        {
                            bool getCompages = reader.GetBoolean(15);
                            if (getCompages == false)
                            {
                                ViewBag.Compages = "";
                            }
                            else
                            {
                                ViewBag.Compages = "checked";
                            }
                        }
                        if (reader.IsDBNull(16) == false)
                        {
                            ViewBag.RemarkCompages = reader.GetString(16);
                        }

                        if (reader.IsDBNull(17) == false)
                        {
                            bool getGoodQuality = reader.GetBoolean(17);
                            if (getGoodQuality == false)
                            {
                                ViewBag.GoodQuality = "";
                            }
                            else
                            {
                                ViewBag.GoodQuality = "checked";
                            }
                        }
                        if (reader.IsDBNull(18) == false)
                        {
                            ViewBag.RemarkGoodQuality = reader.GetString(18);
                        }
                        if (reader.IsDBNull(19) == false)
                        {
                            bool getRejectQuality = reader.GetBoolean(19);
                            if (getRejectQuality == false)
                            {
                                ViewBag.RejectQuality = "";
                            }
                            else
                            {
                                ViewBag.RejectQuality = "checked";
                            }
                        }
                        if (reader.IsDBNull(20) == false)
                        {
                            ViewBag.NameQ = reader.GetString(20);
                        }
                        if (reader.IsDBNull(21) == false)
                        {
                            ViewBag.CommentQ = reader.GetString(21);
                        }

                        if (reader.IsDBNull(22) == false)
                        {
                            bool getRejectITO = reader.GetBoolean(22);
                            if (getRejectITO == false)
                            {
                                ViewBag.RejectITO = "";
                            }
                            else
                            {
                                ViewBag.RejectITO = "checked";
                            }
                        }
                        if (reader.IsDBNull(23) == false)
                        {
                            ViewBag.NameITO = reader.GetString(23);
                        }
                        if (reader.IsDBNull(24) == false)
                        {
                            ViewBag.CommentITO = reader.GetString(24);
                        }
                        if (reader.IsDBNull(25) == false)
                        {
                            bool getRejectMBD = reader.GetBoolean(25);
                            if (getRejectMBD == false)
                            {
                                ViewBag.RejectMBD = "";
                            }
                            else
                            {
                                ViewBag.RejectMBD = "checked";
                            }
                        }
                        if (reader.IsDBNull(26) == false)
                        {
                            ViewBag.NameMBD = reader.GetString(26);
                        }
                        if (reader.IsDBNull(27) == false)
                        {
                            ViewBag.CommentMBD = reader.GetString(27);
                        }
                        if (reader.IsDBNull(28) == false)
                        {
                            ViewBag.dateQ = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(28));
                            //model.DateQTxt = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(28));
                        }

                        if (reader.IsDBNull(29) == false)
                        {
                            ViewBag.dateITO = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(29));
                        }

                        if (reader.IsDBNull(30) == false)
                        {
                            ViewBag.dateMBD = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(30));
                        }
                        if (reader.IsDBNull(31) == false)
                        {
                            ViewBag.QMStatus = reader.GetString(31);
                        }
                    }
                    QM_Model.Add(model);
                }
                cn.Close();
            }


            if (set == "Verify")
            {
                Session["Id"] = Id;
                ViewBag.Id = Id;
                ViewBag.Customer_Name = Customer_Name;
                ViewBag.ProductName = ProductName;

                //get.DateQ = Convert.ToDateTime(get.DateQtxt);
                //get.DateITO = Convert.ToDateTime(get.DateITOtxt);
                //get.DateMBD = Convert.ToDateTime(get.DateMBDtxt);




                string DateQ = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt ");
                string DateITO = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt ");
                string DateMBD = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt ");


                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {


                    Guid guidId = Guid.NewGuid();
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                    cn2.Open();
                    SqlCommand command;
                    command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[QMITO] ([Id],[ModifiedOn],[RejectITO],[CommentITO],[dateITO],[JobInstructionId],[QMStatus],[NameITO])" +
                                             "VALUES (@Id,@ModifiedOn,@RejectITO,@CommentITO,@dateITO,@JobInstructionId,@QMStatus,@NameITO)", cn2);
                    command.Parameters.AddWithValue("@Id", guidId);
                    command.Parameters.AddWithValue("@ModifiedOn", createdOn);
                    if (!string.IsNullOrEmpty(RejectITO))
                    {
                        command.Parameters.AddWithValue("@RejectITO", RejectITO);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RejectITO", DBNull.Value);

                    }
                    command.Parameters.AddWithValue("@CommentITO", CommentITO);
                    command.Parameters.AddWithValue("@dateITO", DateITO);
                    command.Parameters.AddWithValue("@JobInstructionId", Id);
                    command.Parameters.AddWithValue("@QMStatus", "VerifyITO");
                    command.Parameters.AddWithValue("@NameITO", NameITO);

                    command.ExecuteNonQuery();
                    cn2.Close();
                }
                using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                    cn3.Open();
                    SqlCommand command2;
                    command2 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction]  SET ModifiedOn=@ModifiedOn,StatQM=@StatQM WHERE Id=@Id", cn3);

                    command2.Parameters.AddWithValue("@ModifiedOn", createdOn);
                    command2.Parameters.AddWithValue("@Id", get.Id);
                    command2.Parameters.AddWithValue("@StatQM", "Verify");
                    command2.ExecuteNonQuery();
                    cn3.Close();



                }


                return RedirectToAction("ManageQMDDP", "QM");

            }



            if (set == "Verify")
            {
                Session["Id"] = Id;
                ViewBag.Id = Id;
                ViewBag.Customer_Name = Customer_Name;
                ViewBag.ProductName = ProductName;


                //string DateQ2 = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                //string DateITO2 = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                //string DateMBD2 = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");



                using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn1.Open();
                    SqlCommand command1;
                    command1 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET ModifiedOn=@ModifiedOn WHERE Id=@Id", cn1);
                    command1.Parameters.AddWithValue("@Id", Id);
                    command1.Parameters.AddWithValue("@ModifiedOn", createdOn);
                    command1.ExecuteNonQuery();
                    cn1.Close();
                }



                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {


                    cn2.Open();
                    SqlCommand command;
                    command = new SqlCommand("UPDATE [IflowSeed].[dbo].[QM]  SET ModifiedOn=@ModifiedOn,QMStatus=@QMStatus,RejectITO=@RejectITO,NameITO=@NameITO,CommentITO=@CommentITO,dateITO=@dateITO WHERE Id=@JobInstructionId", cn2);
                    command.Parameters.AddWithValue("@ModifiedOn", createdOn);
                    command.Parameters.AddWithValue("@JobInstructionId", get.Id);
                    command.Parameters.AddWithValue("@QMStatus", "Progress");
                    command.Parameters.AddWithValue("@RejectITO", RejectITO);
                    command.Parameters.AddWithValue("@NameITO", NameITO);
                    command.Parameters.AddWithValue("@CommentITO", CommentITO);
                    command.Parameters.AddWithValue("@dateITO", dateITO);


                    command.ExecuteNonQuery();
                    cn2.Close();

                    return RedirectToAction("ManageQMDDP", "QM");

                }
            }


            return View();

        }


        public ActionResult AddDDP3(string Id, string Customer_Name, string AttachEncry, string Templayout, string Corret, string CorretInfo, string Compages, string GoodQuality
          , string RejectQuality, string NameQ, string JobSheetNo, string CommentQ, string RejectITO, string NameITO, string CommentITO, string dateITO, string RejectMBD, string NameMBD, string CommentMBD, string dateMBD
          , string JobType, string RemarkAttachEncry, string CusApp, string RemarkCusApp, string RemarkTemplayout, string RemarkCorret, string RemarkCompages, string RemarkGoodQuality, string dateQ, string set, string ProductName, string RemarkCorretInfo, string set2
          , string JobInstructionId, JobInstruction get, string QMStatus)
        {

            var IdentityName = @Session["Fullname"];
            var Role = @Session["Role"];
            ViewBag.IsDepart = @Session["Department"];


            Session["Id"] = Id;
            ViewBag.Id = Id;





            List<SelectListItem> listproduct = new List<SelectListItem>();

            listproduct.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
            listproduct.Add(new SelectListItem { Text = "DDP", Value = "DDP" });
            listproduct.Add(new SelectListItem { Text = "RMS", Value = "RMS" });
            listproduct.Add(new SelectListItem { Text = "DOW", Value = "DOW" });
            listproduct.Add(new SelectListItem { Text = "OTHERS", Value = "OTHERS" });

            ViewData["ProductType_"] = listproduct;


            int _bil5 = 1;
            List<SelectListItem> li5 = new List<SelectListItem>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT DISTINCT Type FROM [IflowSeed].[dbo].[JobType]          
                                     ORDER BY Type";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JobInstruction model = new JobInstruction();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            model.JobType = reader.GetString(0);
                        }
                    }
                    int i = _bil5++;
                    if (i == 1)
                    {
                        li5.Add(new SelectListItem { Text = "Please Select" });
                        li5.Add(new SelectListItem { Text = model.JobType });

                    }
                    else
                    {
                        li5.Add(new SelectListItem { Text = model.JobType });
                    }
                }
                cn.Close();
            }
            ViewData["JobType_"] = li5;




            List<JobInstruction> JobInstructionlist1 = new List<JobInstruction>(); ;
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass, JobType,JobSheetNo, StartDevDate, EndDevDate
                                        FROM [IflowSeed].[dbo].[JobInstruction]
                                        WHERE Id =@Id";

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


                    }
                    JobInstructionlist1.Add(model);
                }
                cn.Close();

            }

            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn2))
            {
                int _bil2 = 1;
                cn2.Open();
                command.CommandText = @"SELECT QMITO.RejectITO, QMITO.CommentITO, QMITO.dateITO, QMITO.NameITO
                                      FROM  JobInstruction INNER JOIN
                                     QMITO ON JobInstruction.Id = QMITO.JobInstructionId
                                     where   JobInstruction.Id=@Id";

                command.Parameters.AddWithValue("@Id", Id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    QM_Model model = new QM_Model();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            ViewBag.RejectITO = reader.GetString(0);
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            ViewBag.CommentITO = reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            ViewBag.dateITO = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(2));
                        }

                        if (reader.IsDBNull(3) == false)
                        {
                            ViewBag.NameITO = reader.GetString(3);
                        }
                    }
                }
            }

            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn2))
            {
                int _bil2 = 1;
                cn2.Open();
                command.CommandText = @"SELECT QMMBD.RejectMBD, QMMBD.CommentMBD, QMMBD.dateMBD, QMMBD.NameMBD
                                      FROM  JobInstruction INNER JOIN
                                     QMMBD ON JobInstruction.Id = QMMBD.JobInstructionId
                                     where   JobInstruction.Id=@Id";

                command.Parameters.AddWithValue("@Id", Id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    QM_Model model = new QM_Model();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            ViewBag.RejectMBD = reader.GetString(0);
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            ViewBag.CommentMBD = reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            ViewBag.dateMBD = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(2));
                        }

                        if (reader.IsDBNull(3) == false)
                        {
                            ViewBag.NameMBD = reader.GetString(3);
                        }
                    }
                }
            }






            List<QM_Model> QM_Model = new List<QM_Model>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT JobInstruction.Id,  QM.Customer_Name, QM.ProductName, QM.JobSheetNo, QM.JobType, QM.AttachEncry, QM.RemarkAttachEncry, QM.CusApp, QM.RemarkCusApp, QM.Templayout, QM.RemarkTemplayout, QM.Corret, QM.RemarkCorret, QM.CorretInfo, QM.RemarkCorretInfo, QM.Compages, QM.RemarkCompages, QM.GoodQuality, QM.RemarkGoodQuality, QM.RejectQuality, QM.NameQ, QM.CommentQ, QM.RejectITO, QM.NameITO, QM.CommentITO, QM.RejectMBD, QM.NameMBD, QM.CommentMBD, QM.dateQ, QM.dateITO,
                                            QM.dateMBD,QM.QMStatus,QM.Id
                                  FROM  JobInstruction INNER JOIN
                                  QM ON JobInstruction.Id = QM.JobInstructionId
                                     where   JobInstruction.Id=@Id";

                command.Parameters.AddWithValue("@Id", Id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    QM_Model model = new QM_Model();
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
                            ViewBag.JobSheetNo = reader.GetString(3);
                        }


                        if (reader.IsDBNull(4) == false)
                        {
                            ViewBag.JobType = reader.GetString(4);
                        }


                        if (reader.IsDBNull(5) == false)
                        {
                            bool getAttachEncry = reader.GetBoolean(7);
                            if (getAttachEncry == false)
                            {
                                ViewBag.AttachEncry = reader.GetBoolean(7);
                            }
                            else
                            {
                                ViewBag.AttachEncry = "checked";
                            }
                        }

                        if (reader.IsDBNull(6) == false)
                        {
                            ViewBag.RemarkAttachEncry = reader.GetString(6);
                        }


                        if (reader.IsDBNull(7) == false)
                        {
                            bool getCusApp = reader.GetBoolean(7);
                            if (getCusApp == false)
                            {
                                ViewBag.CusApp = "";
                            }
                            else
                            {
                                ViewBag.CusApp = "checked";
                            }
                        }

                        if (reader.IsDBNull(8) == false)
                        {
                            ViewBag.RemarkCusApp = reader.GetString(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            bool getTemplayout = reader.GetBoolean(9);
                            if (getTemplayout == false)
                            {
                                ViewBag.Templayout = "";
                            }
                            else
                            {
                                ViewBag.Templayout = "checked";
                            }
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            ViewBag.RemarkTemplayout = reader.GetString(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            bool getCorret = reader.GetBoolean(11);
                            if (getCorret == false)
                            {
                                ViewBag.Corret = "";
                            }
                            else
                            {
                                ViewBag.Corret = "checked";
                            }
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            ViewBag.RemarkCorret = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            bool getCorretInfo = reader.GetBoolean(13);
                            if (getCorretInfo == false)
                            {
                                ViewBag.CorretInfo = "";
                            }
                            else
                            {
                                ViewBag.CorretInfo = "checked";
                            }
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            ViewBag.RemarkCorretInfo = reader.GetString(14);
                        }

                        if (reader.IsDBNull(15) == false)
                        {
                            bool getCompages = reader.GetBoolean(15);
                            if (getCompages == false)
                            {
                                ViewBag.Compages = "";
                            }
                            else
                            {
                                ViewBag.Compages = "checked";
                            }
                        }
                        if (reader.IsDBNull(16) == false)
                        {
                            ViewBag.RemarkCompages = reader.GetString(16);
                        }

                        if (reader.IsDBNull(17) == false)
                        {
                            bool getGoodQuality = reader.GetBoolean(17);
                            if (getGoodQuality == false)
                            {
                                ViewBag.GoodQuality = "";
                            }
                            else
                            {
                                ViewBag.GoodQuality = "checked";
                            }
                        }
                        if (reader.IsDBNull(18) == false)
                        {
                            ViewBag.RemarkGoodQuality = reader.GetString(18);
                        }
                        if (reader.IsDBNull(19) == false)
                        {
                            bool getRejectQuality = reader.GetBoolean(19);
                            if (getRejectQuality == false)
                            {
                                ViewBag.RejectQuality = "";
                            }
                            else
                            {
                                ViewBag.RejectQuality = "checked";
                            }
                        }
                        if (reader.IsDBNull(20) == false)
                        {
                            ViewBag.NameQ = reader.GetString(20);
                        }
                        if (reader.IsDBNull(21) == false)
                        {
                            ViewBag.CommentQ = reader.GetString(21);
                        }

                        if (reader.IsDBNull(22) == false)
                        {
                            bool getRejectITO = reader.GetBoolean(22);
                            if (getRejectITO == false)
                            {
                                ViewBag.RejectITO = "";
                            }
                            else
                            {
                                ViewBag.RejectITO = "checked";
                            }
                        }
                        if (reader.IsDBNull(23) == false)
                        {
                            ViewBag.NameITO = reader.GetString(23);
                        }
                        if (reader.IsDBNull(24) == false)
                        {
                            ViewBag.CommentITO = reader.GetString(24);
                        }
                        if (reader.IsDBNull(25) == false)
                        {
                            bool getRejectMBD = reader.GetBoolean(25);
                            if (getRejectMBD == false)
                            {
                                ViewBag.RejectMBD = "";
                            }
                            else
                            {
                                ViewBag.RejectMBD = "checked";
                            }
                        }
                        if (reader.IsDBNull(26) == false)
                        {
                            ViewBag.NameMBD = reader.GetString(26);
                        }
                        if (reader.IsDBNull(27) == false)
                        {
                            ViewBag.CommentMBD = reader.GetString(27);
                        }
                        if (reader.IsDBNull(28) == false)
                        {
                            ViewBag.dateQ = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(28));
                            //model.DateQTxt = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(28));
                        }

                        if (reader.IsDBNull(29) == false)
                        {
                            ViewBag.dateITO = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(29));
                        }

                        if (reader.IsDBNull(30) == false)
                        {
                            ViewBag.dateMBD = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(30));
                        }
                        if (reader.IsDBNull(31) == false)
                        {
                            ViewBag.QMStatus = reader.GetString(31);
                        }
                    }
                    QM_Model.Add(model);
                }
                cn.Close();
            }


            if (set == "Verify")
            {
                Session["Id"] = Id;
                ViewBag.Id = Id;
                ViewBag.Customer_Name = Customer_Name;
                ViewBag.ProductName = ProductName;

                //get.DateQ = Convert.ToDateTime(get.DateQtxt);
                //get.DateITO = Convert.ToDateTime(get.DateITOtxt);
                //get.DateMBD = Convert.ToDateTime(get.DateMBDtxt);




                string DateQ = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt ");
                string DateITO = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt ");
                string DateMBD = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt ");


                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {


                    Guid guidId = Guid.NewGuid();
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                    cn2.Open();
                    SqlCommand command;
                    command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[QMMBD] ([Id],[ModifiedOn],[RejectMBD],[CommentMBD],[dateMBD],[JobInstructionId],[QMStatus],[NameMBD])" +
                                             "VALUES (@Id,@ModifiedOn,@RejectMBD,@CommentMBD,@dateMBD,@JobInstructionId,@QMStatus,@NameMBD)", cn2);
                    command.Parameters.AddWithValue("@Id", guidId);
                    command.Parameters.AddWithValue("@ModifiedOn", createdOn);
                    if (!string.IsNullOrEmpty(RejectITO))
                    {
                        command.Parameters.AddWithValue("@RejectMBD", RejectMBD);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RejectMBD", DBNull.Value);

                    }
                    command.Parameters.AddWithValue("@CommentMBD", CommentMBD);
                    command.Parameters.AddWithValue("@dateMBD", DateMBD);
                    command.Parameters.AddWithValue("@JobInstructionId", Id);
                    command.Parameters.AddWithValue("@QMStatus", "QMStatus");
                    command.Parameters.AddWithValue("@NameMBD", NameMBD);

                    command.ExecuteNonQuery();
                    cn2.Close();
                }
                using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                    cn3.Open();
                    SqlCommand command2;
                    command2 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction]  SET ModifiedOn=@ModifiedOn,StatQM=@StatQM WHERE Id=@Id", cn3);

                    command2.Parameters.AddWithValue("@ModifiedOn", createdOn);
                    command2.Parameters.AddWithValue("@Id", get.Id);
                    command2.Parameters.AddWithValue("@StatQM", "Success Verify");
                    command2.ExecuteNonQuery();
                    cn3.Close();



                }


                return RedirectToAction("ManageQMDDP", "QM");

            }



            if (set == "Verify")
            {
                Session["Id"] = Id;
                ViewBag.Id = Id;
                ViewBag.Customer_Name = Customer_Name;
                ViewBag.ProductName = ProductName;


                //string DateQ2 = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                //string DateITO2 = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                //string DateMBD2 = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");



                using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn1.Open();
                    SqlCommand command1;
                    command1 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET ModifiedOn=@ModifiedOn WHERE Id=@Id", cn1);
                    command1.Parameters.AddWithValue("@Id", Id);
                    command1.Parameters.AddWithValue("@ModifiedOn", createdOn);
                    command1.ExecuteNonQuery();
                    cn1.Close();
                }



                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {


                    cn2.Open();
                    SqlCommand command;
                    command = new SqlCommand("UPDATE [IflowSeed].[dbo].[QM]  SET ModifiedOn=@ModifiedOn,QMStatus=@QMStatus,RejectITO=@RejectITO,NameITO=@NameITO,CommentITO=@CommentITO,dateITO=@dateITO WHERE Id=@JobInstructionId", cn2);
                    command.Parameters.AddWithValue("@ModifiedOn", createdOn);
                    command.Parameters.AddWithValue("@JobInstructionId", get.Id);
                    command.Parameters.AddWithValue("@QMStatus", "Progress");
                    command.Parameters.AddWithValue("@RejectITO", RejectITO);
                    command.Parameters.AddWithValue("@NameITO", NameITO);
                    command.Parameters.AddWithValue("@CommentITO", CommentITO);
                    command.Parameters.AddWithValue("@dateITO", dateITO);


                    command.ExecuteNonQuery();
                    cn2.Close();

                    return RedirectToAction("ManageQMDDP", "QM");

                }
            }


            return View();

        }




        //public ActionResult AddDDP2(string Id, string Customer_Name, string AttachEncry, string Templayout, string Corret, string CorretInfo, string Compages, string GoodQuality
        //   , string RejectQuality, string NameQ, string JobSheetNo, string CommentQ, string RejectITO, string NameITO, string CommentITO, string dateITO, string RejectMBD, string NameMBD, string CommentMBD, string dateMBD
        //   , string JobType, string RemarkAttachEncry, string CusApp, string RemarkCusApp, string RemarkTemplayout, string RemarkCorret, string RemarkCompages, string RemarkGoodQuality, string dateQ, string set, string ProductName, string RemarkCorretInfo, string set2
        //   , string JobInstructionId, JobInstruction get, string DateQ, string DateITO, string DateMBD)
        //{

        //    var IdentityName = @Session["Fullname"];
        //    var Role = @Session["Role"];
        //    ViewBag.IsDepart = @Session["Department"];


        //    Session["Id"] = Id;
        //    ViewBag.Id = Id;

        //    List<SelectListItem> listproduct = new List<SelectListItem>();

        //    listproduct.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        //    listproduct.Add(new SelectListItem { Text = "DDP", Value = "DDP" });
        //    listproduct.Add(new SelectListItem { Text = "RMS", Value = "RMS" });
        //    listproduct.Add(new SelectListItem { Text = "DOW", Value = "DOW" });
        //    listproduct.Add(new SelectListItem { Text = "OTHERS", Value = "OTHERS" });

        //    ViewData["ProductType_"] = listproduct;


        //    int _bil5 = 1;
        //    List<SelectListItem> li5 = new List<SelectListItem>();
        //    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        //    using (SqlCommand command = new SqlCommand("", cn))
        //    {
        //        cn.Open();
        //        command.CommandText = @"SELECT DISTINCT Type FROM [IflowSeed].[dbo].[JobType]          
        //                             ORDER BY Type";
        //        var reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            JobInstruction model = new JobInstruction();
        //            {
        //                if (reader.IsDBNull(0) == false)
        //                {
        //                    model.JobType = reader.GetString(0);
        //                }
        //            }
        //            int i = _bil5++;
        //            if (i == 1)
        //            {
        //                li5.Add(new SelectListItem { Text = "Please Select" });
        //                li5.Add(new SelectListItem { Text = model.JobType });

        //            }
        //            else
        //            {
        //                li5.Add(new SelectListItem { Text = model.JobType });
        //            }
        //        }
        //        cn.Close();
        //    }
        //    ViewData["JobType_"] = li5;

        //    List<JobInstruction> JobInstructionlist1 = new List<JobInstruction>(); ;
        //    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        //    using (SqlCommand command = new SqlCommand("", cn))
        //    {
        //        int _bil = 1;
        //        cn.Open();
        //        command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass, JobType,JobSheetNo, StartDevDate, EndDevDate
        //                                FROM [IflowSeed].[dbo].[JobInstruction]
        //                                WHERE Id =@Id";

        //        command.Parameters.AddWithValue("@Id", Id);
        //        var reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            JobInstruction model = new JobInstruction();
        //            {
        //                model.Bil = _bil++;
        //                if (reader.IsDBNull(0) == false)
        //                {
        //                    ViewBag.Id = reader.GetGuid(0);
        //                }
        //                if (reader.IsDBNull(1) == false)
        //                {
        //                    ViewBag.Customer_Name = reader.GetString(1);
        //                }
        //                if (reader.IsDBNull(2) == false)
        //                {
        //                    ViewBag.ProductName = reader.GetString(2);
        //                }
        //                if (reader.IsDBNull(3) == false)
        //                {
        //                    ViewBag.JobClass = reader.GetString(3);
        //                }
        //                if (reader.IsDBNull(4) == false)
        //                {
        //                    ViewBag.JobType = reader.GetString(4);
        //                }
        //                if (reader.IsDBNull(5) == false)
        //                {
        //                    ViewBag.JobSheetNo = reader.GetString(5);
        //                }
        //                if (reader.IsDBNull(6) == false)
        //                {
        //                    ViewBag.StartDevOn = reader.GetDateTime(6);
        //                }

        //                if (reader.IsDBNull(7) == false)
        //                {
        //                    ViewBag.EndDevDate = reader.GetDateTime(7);
        //                }


        //            }
        //            JobInstructionlist1.Add(model);
        //        }
        //        cn.Close();

        //    }


        //    using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        //    using (SqlCommand command = new SqlCommand("", cn2))
        //    {
        //        int _bil2 = 1;
        //        cn2.Open();
        //        command.CommandText = @"SELECT QMITO.RejectITO, QMITO.CommentITO, QMITO.dateITO, QMITO.NameITO
        //                              FROM  JobInstruction INNER JOIN
        //                             QMITO ON JobInstruction.Id = QMITO.JobInstructionId
        //                             where   JobInstruction.Id=@Id";

        //        command.Parameters.AddWithValue("@Id", Id);
        //        var reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            QM_Model model = new QM_Model();
        //            {
        //                if (reader.IsDBNull(0) == false)
        //                {
        //                    ViewBag.RejectITO = reader.GetString(0);
        //                }
        //                if (reader.IsDBNull(1) == false)
        //                {
        //                    ViewBag.CommentITO = reader.GetString(1);
        //                }
        //                if (reader.IsDBNull(2) == false)
        //                {
        //                    ViewBag.dateITO = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(2));
        //                }

        //                if (reader.IsDBNull(3) == false)
        //                {
        //                    ViewBag.NameITO = reader.GetString(3);
        //                }
        //            }
        //        }
        //    }




        //    List<QM_Model> QM_Model = new List<QM_Model>(); ;
        //    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        //    using (SqlCommand command = new SqlCommand("", cn))
        //    {
        //        int _bil = 1;
        //        cn.Open();
        //        command.CommandText = @"SELECT JobInstruction.Id,  QM.Customer_Name, QM.ProductName, QM.JobSheetNo, QM.JobType, QM.AttachEncry, QM.RemarkAttachEncry, QM.CusApp, QM.RemarkCusApp, QM.Templayout, QM.RemarkTemplayout, QM.Corret, QM.RemarkCorret, QM.CorretInfo, QM.RemarkCorretInfo, QM.Compages, QM.RemarkCompages, QM.GoodQuality, QM.RemarkGoodQuality, QM.RejectQuality, QM.NameQ, QM.CommentQ, QM.RejectITO, QM.NameITO, QM.CommentITO, QM.RejectMBD, QM.NameMBD, QM.CommentMBD, QM.dateQ, QM.dateITO,
        //                                    QM.dateMBD,QM.QMStatus,QM.Id
        //                          FROM  JobInstruction INNER JOIN
        //                          QM ON JobInstruction.Id = QM.JobInstructionId
        //                             where   QM.JobInstructionId=@Id";

        //        command.Parameters.AddWithValue("@Id", Id);
        //        var reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            QM_Model model = new QM_Model();
        //            {
        //                model.Bil = _bil++;
        //                if (reader.IsDBNull(0) == false)
        //                {
        //                    ViewBag.Id = reader.GetGuid(0);
        //                }
        //                if (reader.IsDBNull(1) == false)
        //                {
        //                    ViewBag.Customer_Name = reader.GetString(1);
        //                }
        //                if (reader.IsDBNull(2) == false)
        //                {
        //                    ViewBag.ProductName = reader.GetString(2);
        //                }
        //                if (reader.IsDBNull(3) == false)
        //                {
        //                    ViewBag.JobSheetNo = reader.GetString(3);
        //                }


        //                if (reader.IsDBNull(4) == false)
        //                {
        //                    ViewBag.JobType = reader.GetString(4);
        //                }


        //                if (reader.IsDBNull(5) == false)
        //                {
        //                    bool getAttachEncry = reader.GetBoolean(7);
        //                    if (getAttachEncry == false)
        //                    {
        //                        ViewBag.AttachEncry = reader.GetBoolean(7);
        //                    }
        //                    else
        //                    {
        //                        ViewBag.AttachEncry = "checked";
        //                    }
        //                }

        //                if (reader.IsDBNull(6) == false)
        //                {
        //                    ViewBag.RemarkAttachEncry = reader.GetString(6);
        //                }


        //                if (reader.IsDBNull(7) == false)
        //                {
        //                    bool getCusApp = reader.GetBoolean(7);
        //                    if (getCusApp == false)
        //                    {
        //                        ViewBag.CusApp = "";
        //                    }
        //                    else
        //                    {
        //                        ViewBag.CusApp = "checked";
        //                    }
        //                }

        //                if (reader.IsDBNull(8) == false)
        //                {
        //                    ViewBag.RemarkCusApp = reader.GetString(8);
        //                }
        //                if (reader.IsDBNull(9) == false)
        //                {
        //                    bool getTemplayout = reader.GetBoolean(9);
        //                    if (getTemplayout == false)
        //                    {
        //                        ViewBag.Templayout = "";
        //                    }
        //                    else
        //                    {
        //                        ViewBag.Templayout = "checked";
        //                    }
        //                }
        //                if (reader.IsDBNull(10) == false)
        //                {
        //                    ViewBag.RemarkTemplayout = reader.GetString(10);
        //                }
        //                if (reader.IsDBNull(11) == false)
        //                {
        //                    bool getCorret = reader.GetBoolean(11);
        //                    if (getCorret == false)
        //                    {
        //                        ViewBag.Corret = "";
        //                    }
        //                    else
        //                    {
        //                        ViewBag.Corret = "checked";
        //                    }
        //                }
        //                if (reader.IsDBNull(12) == false)
        //                {
        //                    ViewBag.RemarkCorret = reader.GetString(12);
        //                }
        //                if (reader.IsDBNull(13) == false)
        //                {
        //                    bool getCorretInfo = reader.GetBoolean(13);
        //                    if (getCorretInfo == false)
        //                    {
        //                        ViewBag.CorretInfo = "";
        //                    }
        //                    else
        //                    {
        //                        ViewBag.CorretInfo = "checked";
        //                    }
        //                }
        //                if (reader.IsDBNull(14) == false)
        //                {
        //                    ViewBag.RemarkCorretInfo = reader.GetString(14);
        //                }

        //                if (reader.IsDBNull(15) == false)
        //                {
        //                    bool getCompages = reader.GetBoolean(15);
        //                    if (getCompages == false)
        //                    {
        //                        ViewBag.Compages = "";
        //                    }
        //                    else
        //                    {
        //                        ViewBag.Compages = "checked";
        //                    }
        //                }
        //                if (reader.IsDBNull(16) == false)
        //                {
        //                    ViewBag.RemarkCompages = reader.GetString(16);
        //                }

        //                if (reader.IsDBNull(17) == false)
        //                {
        //                    bool getGoodQuality = reader.GetBoolean(17);
        //                    if (getGoodQuality == false)
        //                    {
        //                        ViewBag.GoodQuality = "";
        //                    }
        //                    else
        //                    {
        //                        ViewBag.GoodQuality = "checked";
        //                    }
        //                }
        //                if (reader.IsDBNull(18) == false)
        //                {
        //                    ViewBag.RemarkGoodQuality = reader.GetString(18);
        //                }
        //                if (reader.IsDBNull(19) == false)
        //                {
        //                    bool getRejectQuality = reader.GetBoolean(19);
        //                    if (getRejectQuality == false)
        //                    {
        //                        ViewBag.RejectQuality = "";
        //                    }
        //                    else
        //                    {
        //                        ViewBag.RejectQuality = "checked";
        //                    }
        //                }
        //                if (reader.IsDBNull(20) == false)
        //                {
        //                    ViewBag.NameQ = reader.GetString(20);
        //                }
        //                if (reader.IsDBNull(21) == false)
        //                {
        //                    ViewBag.CommentQ = reader.GetString(21);
        //                }

        //                if (reader.IsDBNull(22) == false)
        //                {
        //                    bool getRejectITO = reader.GetBoolean(22);
        //                    if (getRejectITO == false)
        //                    {
        //                        ViewBag.RejectITO = "";
        //                    }
        //                    else
        //                    {
        //                        ViewBag.RejectITO = "checked";
        //                    }
        //                }
        //                if (reader.IsDBNull(23) == false)
        //                {
        //                    ViewBag.NameITO = reader.GetString(23);
        //                }
        //                if (reader.IsDBNull(24) == false)
        //                {
        //                    ViewBag.CommentITO = reader.GetString(24);
        //                }
        //                if (reader.IsDBNull(25) == false)
        //                {
        //                    bool getRejectMBD = reader.GetBoolean(25);
        //                    if (getRejectMBD == false)
        //                    {
        //                        ViewBag.RejectMBD = "";
        //                    }
        //                    else
        //                    {
        //                        ViewBag.RejectMBD = "checked";
        //                    }
        //                }
        //                if (reader.IsDBNull(26) == false)
        //                {
        //                    ViewBag.NameMBD = reader.GetString(26);
        //                }
        //                if (reader.IsDBNull(27) == false)
        //                {
        //                    ViewBag.CommentMBD = reader.GetString(27);
        //                }
        //                if (reader.IsDBNull(28) == false)
        //                {
        //                    ViewBag.dateQ = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(28));
        //                    //model.DateQTxt = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(28));
        //                }

        //                if (reader.IsDBNull(29) == false)
        //                {
        //                    ViewBag.dateITO = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(29));
        //                }

        //                if (reader.IsDBNull(30) == false)
        //                {
        //                    ViewBag.dateMBD = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(30));
        //                }
        //                if (reader.IsDBNull(31) == false)
        //                {
        //                    ViewBag.QMStatus = reader.GetString(31);
        //                }
        //            }
        //            QM_Model.Add(model);
        //        }
        //        cn.Close();
        //    }


        //    if (set == "Verify")
        //    {
        //        Session["Id"] = Id;
        //        ViewBag.Id = Id;
        //        ViewBag.Customer_Name = Customer_Name;
        //        ViewBag.ProductName = ProductName;

        //        //get.DateQ = Convert.ToDateTime(get.DateQtxt);
        //        //get.DateITO = Convert.ToDateTime(get.DateITOtxt);
        //        //get.DateMBD = Convert.ToDateTime(get.DateMBDtxt);




        //        string DateQ2 = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt ");
        //        string DateITO2 = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt ");
        //        string DateMBD2 = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt ");



        //        using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        //        {


        //            Guid guidId = Guid.NewGuid();
        //            string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

        //            cn2.Open();
        //            SqlCommand command;
        //            command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[QMITO] ([Id],[ModifiedOn],[RejectITO],[CommentITO],[dateITO],[JobInstructionId],[QMStatus],[NameITO])" +
        //                                     "VALUES (@Id,@ModifiedOn,@RejectITO,@CommentITO,@dateITO,@JobInstructionId,@QMStatus,@NameITO)", cn2);
        //            command.Parameters.AddWithValue("@Id", guidId);
        //            command.Parameters.AddWithValue("@ModifiedOn", createdOn);
        //            if (!string.IsNullOrEmpty(RejectITO))
        //            {
        //                command.Parameters.AddWithValue("@RejectITO", RejectITO);
        //            }
        //            else
        //            {
        //                command.Parameters.AddWithValue("@RejectITO", DBNull.Value);

        //            }
        //            command.Parameters.AddWithValue("@CommentITO", CommentITO);
        //            command.Parameters.AddWithValue("@dateITO", DateITO);
        //            command.Parameters.AddWithValue("@JobInstructionId", Id);
        //            command.Parameters.AddWithValue("@QMStatus", "VerifyITO");
        //            command.Parameters.AddWithValue("@NameITO", NameITO);

        //            command.ExecuteNonQuery();
        //            cn2.Close();
        //        }



        //        using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        //        {

        //            string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

        //            cn2.Open();
        //            SqlCommand command;
        //            command = new SqlCommand("UPDATE [IflowSeed].[dbo].[QM]  SET ModifiedOn=@ModifiedOn,QMStatus=@QMStatus WHERE JobInstructionId=@JobInstructionId", cn2);

        //            command.Parameters.AddWithValue("@ModifiedOn", createdOn);
        //            command.Parameters.AddWithValue("@JobInstructionId", get.Id);
        //            command.Parameters.AddWithValue("@QMStatus", "Verify");
        //            command.ExecuteNonQuery();
        //            cn2.Close();

        //            return RedirectToAction("ManageQMDDP", "QM");

        //        }
        //    }


        //    return View();

        //}


        public ActionResult ViewQMDDP(string JobInstructionId, string Id)
        {


            int _bil5 = 1;
            List<SelectListItem> li5 = new List<SelectListItem>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT DISTINCT Type FROM [IflowSeed].[dbo].[JobType]          
                                     ORDER BY Type";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JobInstruction model = new JobInstruction();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            model.JobType = reader.GetString(0);
                        }
                    }
                    int i = _bil5++;
                    if (i == 1)
                    {
                        li5.Add(new SelectListItem { Text = "Please Select" });
                        li5.Add(new SelectListItem { Text = model.JobType });

                    }
                    else
                    {
                        li5.Add(new SelectListItem { Text = model.JobType });
                    }
                }
                cn.Close();
            }
            ViewData["JobType_"] = li5;

            if (!string.IsNullOrEmpty(Id))
            {

                List<QM_Model> QM_Model = new List<QM_Model>(); ;
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    int _bil = 1;
                    cn.Open();
                    command.CommandText = @"SELECT JobInstruction.Id,  QM.Customer_Name, QM.ProductName, QM.JobSheetNo, QM.JobType, QM.AttachEncry, QM.RemarkAttachEncry, QM.CusApp, QM.RemarkCusApp, QM.Templayout, QM.RemarkTemplayout, QM.Corret, QM.RemarkCorret, QM.CorretInfo, QM.RemarkCorretInfo, QM.Compages, QM.RemarkCompages, QM.GoodQuality, QM.RemarkGoodQuality, QM.RejectQuality, QM.NameQ, QM.CommentQ, QM.RejectITO, QM.NameITO, QM.CommentITO, QM.RejectMBD, QM.NameMBD, QM.CommentMBD, QM.dateQ, QM.dateITO, 
                                            QM.dateMBD,QM.Id
                                  FROM  JobInstruction INNER JOIN
                                  QM ON JobInstruction.Id = QM.JobInstructionId
                                  where JobInstruction.JobType= 'DCP'";
                    command.Parameters.AddWithValue("@Id", Id);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        QM_Model model = new QM_Model();
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
                                ViewBag.JobSheetNo = reader.GetString(3);
                            }



                            if (reader.IsDBNull(4) == false)
                            {
                                ViewBag.JobType = reader.GetString(4);
                            }


                            if (reader.IsDBNull(5) == false)
                            {
                                bool getAttachEncry = reader.GetBoolean(7);
                                if (getAttachEncry == false)
                                {
                                    ViewBag.AttachEncry = reader.GetBoolean(7);
                                }
                                else
                                {
                                    ViewBag.AttachEncry = "checked";
                                }
                            }

                            if (reader.IsDBNull(6) == false)
                            {
                                ViewBag.RemarkAttachEncry = reader.GetString(6);
                            }


                            if (reader.IsDBNull(7) == false)
                            {
                                bool getCusApp = reader.GetBoolean(7);
                                if (getCusApp == false)
                                {
                                    ViewBag.CusApp = "";
                                }
                                else
                                {
                                    ViewBag.CusApp = "checked";
                                }
                            }

                            if (reader.IsDBNull(8) == false)
                            {
                                ViewBag.RemarkCusApp = reader.GetString(8);
                            }
                            if (reader.IsDBNull(9) == false)
                            {
                                bool getTemplayout = reader.GetBoolean(9);
                                if (getTemplayout == false)
                                {
                                    ViewBag.Templayout = "";
                                }
                                else
                                {
                                    ViewBag.Templayout = "checked";
                                }
                            }
                            if (reader.IsDBNull(10) == false)
                            {
                                ViewBag.RemarkTemplayout = reader.GetString(10);
                            }
                            if (reader.IsDBNull(11) == false)
                            {
                                bool getCorret = reader.GetBoolean(11);
                                if (getCorret == false)
                                {
                                    ViewBag.Corret = "";
                                }
                                else
                                {
                                    ViewBag.Corret = "checked";
                                }
                            }
                            if (reader.IsDBNull(12) == false)
                            {
                                ViewBag.RemarkCorret = reader.GetString(12);
                            }
                            if (reader.IsDBNull(13) == false)
                            {
                                bool getCorretInfo = reader.GetBoolean(13);
                                if (getCorretInfo == false)
                                {
                                    ViewBag.CorretInfo = "";
                                }
                                else
                                {
                                    ViewBag.CorretInfo = "checked";
                                }
                            }
                            if (reader.IsDBNull(14) == false)
                            {
                                ViewBag.RemarkCorretInfo = reader.GetString(14);
                            }

                            if (reader.IsDBNull(15) == false)
                            {
                                bool getCompages = reader.GetBoolean(15);
                                if (getCompages == false)
                                {
                                    ViewBag.Compages = "";
                                }
                                else
                                {
                                    ViewBag.Compages = "checked";
                                }
                            }
                            if (reader.IsDBNull(16) == false)
                            {
                                ViewBag.RemarkCompages = reader.GetString(16);
                            }

                            if (reader.IsDBNull(17) == false)
                            {
                                bool getGoodQuality = reader.GetBoolean(17);
                                if (getGoodQuality == false)
                                {
                                    ViewBag.GoodQuality = "";
                                }
                                else
                                {
                                    ViewBag.GoodQuality = "checked";
                                }
                            }
                            if (reader.IsDBNull(18) == false)
                            {
                                ViewBag.RemarkGoodQuality = reader.GetString(18);
                            }
                            if (reader.IsDBNull(19) == false)
                            {
                                bool getRejectQuality = reader.GetBoolean(19);
                                if (getRejectQuality == false)
                                {
                                    ViewBag.RejectQuality = "";
                                }
                                else
                                {
                                    ViewBag.RejectQuality = "checked";
                                }
                            }
                            if (reader.IsDBNull(20) == false)
                            {
                                ViewBag.NameQ = reader.GetString(20);
                            }
                            if (reader.IsDBNull(21) == false)
                            {
                                ViewBag.CommentQ = reader.GetString(21);
                            }

                            if (reader.IsDBNull(22) == false)
                            {
                                bool getRejectITO = reader.GetBoolean(22);
                                if (getRejectITO == false)
                                {
                                    ViewBag.RejectITO = "";
                                }
                                else
                                {
                                    ViewBag.RejectITO = "checked";
                                }
                            }
                            if (reader.IsDBNull(23) == false)
                            {
                                ViewBag.NameITO = reader.GetString(23);
                            }
                            if (reader.IsDBNull(24) == false)
                            {
                                ViewBag.CommentITO = reader.GetString(24);
                            }
                            if (reader.IsDBNull(25) == false)
                            {
                                bool getRejectMBD = reader.GetBoolean(25);
                                if (getRejectMBD == false)
                                {
                                    ViewBag.RejectMBD = "";
                                }
                                else
                                {
                                    ViewBag.RejectMBD = "checked";
                                }
                            }
                            if (reader.IsDBNull(26) == false)
                            {
                                ViewBag.NameMBD = reader.GetString(26);
                            }
                            if (reader.IsDBNull(27) == false)
                            {
                                ViewBag.CommentMBD = reader.GetString(27);
                            }
                            if (reader.IsDBNull(28) == false)
                            {
                                ViewBag.dateQ = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(28));
                                //model.DateQTxt = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(28));
                            }

                            if (reader.IsDBNull(29) == false)
                            {
                                ViewBag.dateITO = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(29));
                            }

                            if (reader.IsDBNull(30) == false)
                            {
                                ViewBag.dateMBD = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader.GetDateTime(30));
                            }
                        }
                        QM_Model.Add(model);
                    }
                    cn.Close();



                }

            }
            return View();


        }


        public ActionResult QMSubmit(string Id, string JobSheetNo)
        {
            Debug.WriteLine("Id : " + Id);
            using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn1.Open();
                SqlCommand command2;
                command2 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET NMRStatus=@NMRStatus,Status=@Status WHERE Id=@Id", cn1);

                command2.Parameters.AddWithValue("@NMRStatus", "COMPLETED");
                command2.Parameters.AddWithValue("@Id", Id);
                command2.Parameters.AddWithValue("@Status", "PRODUCTION");
                command2.ExecuteNonQuery();

                SqlCommand cmdQMStatus = new SqlCommand("UPDATE QM Set QMStatus=@QMStatus WHERE JobInstructionId=@JobInstructionIdqmstatus", cn1);

                cmdQMStatus.Parameters.AddWithValue("@QMStatus", "Complete");
                cmdQMStatus.Parameters.AddWithValue("JobInstructionIdqmstatus", Id);

                cmdQMStatus.ExecuteNonQuery();

                SqlCommand cmdUpdateAT = new SqlCommand("UPDATE JobAuditTrailDetail Set Status=@StatusAT WHERE JobSheetNo=@JobSheetNo", cn1);
                cmdUpdateAT.Parameters.AddWithValue("@JobSheetNo", JobSheetNo);
                cmdUpdateAT.Parameters.AddWithValue("@StatusAT", "PRODUCTION");

                cmdUpdateAT.ExecuteNonQuery();

                cn1.Close();

                return RedirectToAction("ManageQM", "QM");
            }
        }



    }

}
