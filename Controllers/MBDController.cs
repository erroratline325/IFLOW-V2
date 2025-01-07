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
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Web.Mail;
using Document = MigraDoc.DocumentObjectModel.Document;
using Style = MigraDoc.DocumentObjectModel.Style;
using Paragraph = MigraDoc.DocumentObjectModel.Paragraph;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using System.EnterpriseServices;

[MvcAppV2.FilterConfig.UserSessionActionFilter]
public class MBDController : Controller
{

    string PathSource = System.Configuration.ConfigurationManager.AppSettings["SourceFile"];
    string IpSMtp_ = System.Configuration.ConfigurationManager.AppSettings["IpSMtp"];
    string PortSmtp_ = System.Configuration.ConfigurationManager.AppSettings["PortSmtp"];
    string PathSource2 = System.Configuration.ConfigurationManager.AppSettings["logfilelocation"];






    List<CustomerContract> CustomerContractlist = new List<CustomerContract>();
    public ActionResult ManageCustomerContract(string Id, string Customer_Name, string set)
    {
        if (set == "search") //ini kalu user search product
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Id, Customer_Name, Contract_Name, ContractNo, StrtContractDate, EndContractDate,  AccountManager
                                     FROM [IflowSeed].[dbo].[CustomerContract]
                                     WHERE Customer_Name LIKE @Customer_Name";
                command.Parameters.AddWithValue("@Customer_Name", "%" + Customer_Name + "%");

                //command.Parameters.AddWithValue("@Customer_Name", "%" + Customer_Name + "%");
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CustomerContract model = new CustomerContract();
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
                            model.ContractName = reader.GetString(2);
                        }
                        if (reader.IsDBNull(3) == false)
                        {
                            model.ContractNo = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            //model.StrtContractDateTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(4));
                            model.StrtContractDateTxt = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            //model.EndContractDateTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(5));
                            model.EndContractDateTxt = reader.GetString(5);

                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.SalesExecutiveBy = reader.GetString(6);
                        }

                    }
                    CustomerContractlist.Add(model);
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
                command.CommandText = @"SELECT Id, Customer_Name, Contract_Name, ContractNo, StrtContractDate, EndContractDate,  AccountManager
                                     FROM [IflowSeed].[dbo].[CustomerContract]
                                     ORDER BY Customer_Name";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CustomerContract model = new CustomerContract();
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
                            model.ContractName = reader.GetString(2);
                        }
                        if (reader.IsDBNull(3) == false)
                        {
                            model.ContractNo = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            //model.StrtContractDateTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(4));
                            model.StrtContractDateTxt = reader.GetString(4);

                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            //model.EndContractDateTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(5));
                            model.EndContractDateTxt = reader.GetString(5);

                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.SalesExecutiveBy = reader.GetString(6);
                        }

                    }
                    CustomerContractlist.Add(model);
                }
                cn.Close();
            }
        }
        return View(CustomerContractlist); //hntr data ke ui
    }

    public ActionResult CreateCustomerContract(CustomerContract customerContract, string Id, string Customer_Name, string ContractName, string ContractNo, string StrtContractDate, string EndContractDate, string SalesExecutiveBy, string set)
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
                CustomerContract model = new CustomerContract();
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

        if (string.IsNullOrEmpty(Id))
        {
            ViewBag.DataSet = "Save";
        }
        else
        {
            ViewBag.DataSet = "update";
        }

        if (string.IsNullOrEmpty(Id) && Customer_Name != "Please Select" && !string.IsNullOrEmpty(Customer_Name) && !string.IsNullOrEmpty(ContractName) && !string.IsNullOrEmpty(ContractNo) && !string.IsNullOrEmpty(StrtContractDate) && !string.IsNullOrEmpty(EndContractDate))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                Guid Idx = Guid.NewGuid();
                string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                string ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                var StrtContractDateTxt = Convert.ToDateTime(customerContract.StrtContractDate);
                string strtContractDate = StrtContractDateTxt.ToString("dd/MM/yyyy");

                var EndContractDateTxt = Convert.ToDateTime(customerContract.EndContractDate);
                string endContractDate = EndContractDateTxt.ToString("dd/MM/yyyy");

                cn.Open();
                SqlCommand command;
                command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[CustomerContract] (Id, CreatedOn, ModifiedOn, Customer_Name, Contract_Name, ContractNo, StrtContractDate, EndContractDate, AccountManager) values (@Id, @CreatedOn, @ModifiedOn, @Customer_Name, @Contract_Name, @ContractNo, @StrtContractDate, @EndContractDate, @AccountManager)", cn);
                command.Parameters.AddWithValue("@Id", Idx);
                command.Parameters.AddWithValue("@CreatedOn", createdOn);
                command.Parameters.AddWithValue("@ModifiedOn", ModifiedOn);
                command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                command.Parameters.AddWithValue("@Contract_Name", ContractName);
                command.Parameters.AddWithValue("@ContractNo", ContractNo);
                command.Parameters.AddWithValue("@StrtContractDate", customerContract.StrtContractDate);
                command.Parameters.AddWithValue("@EndContractDate", customerContract.EndContractDate);
                command.Parameters.AddWithValue("@AccountManager", IdentityName.ToString());
                command.ExecuteNonQuery();
                cn.Close();
            }
            return RedirectToAction("ManageCustomerContract", "MBD");
        }

        if (!string.IsNullOrEmpty(Id) && set == "update")
        {
            //update
            string ModifiedOn = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt");

            //var StrtContractDt = Convert.ToDateTime(customerContract.StrtContractDate);
            var StrtContractDt = Convert.ToDateTime(StrtContractDate);
            string strtContractDate = StrtContractDt.ToString("dd/MM/yyyy");
            
            //var EndContractDt = Convert.ToDateTime(customerContract.EndContractDate);
            var EndContractDt = Convert.ToDateTime(EndContractDate);
            string endContractDate = EndContractDt.ToString("dd/MM/yyyy");


            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("UPDATE [IflowSeed].[dbo].[CustomerContract]  SET ModifiedOn=@ModifiedOn, Customer_Name=@Customer_Name, Contract_Name=@ContractName, ContractNo=@ContractNo, StrtContractDate=@StrtContractDate, EndContractDate=@EndContractDate, AccountManager=@SalesExecutiveBy  WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@ModifiedOn", ModifiedOn);
                command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                command.Parameters.AddWithValue("@ContractName", ContractName);
                command.Parameters.AddWithValue("@ContractNo", ContractNo);
                command.Parameters.AddWithValue("@StrtContractDate",StrtContractDate);
                command.Parameters.AddWithValue("@EndContractDate", EndContractDate);
                command.Parameters.AddWithValue("@SalesExecutiveBy", IdentityName.ToString());
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn.Close();
            }

            TempData["msg"] = "<script>alert('Successfully Updated')</script>";
        }

        if (!string.IsNullOrEmpty(Id))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Id, Customer_Name, Contract_Name, ContractNo, FORMAT(CONVERT(date, StrtContractDate), 'yyyy-MM-dd') as StrtContractDate , FORMAT(CONVERT(date, EndContractDate), 'yyyy-MM-dd') as EndContractDate , AccountManager
                                       FROM [IflowSeed].[dbo].[CustomerContract]                              
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
                        ViewBag.ContractName = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        ViewBag.ContractNo = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        //ViewBag.StrtContractDate = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(4));
                        ViewBag.StrtContractDate = reader.GetString(4);

                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        //ViewBag.EndContractDate = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(5));
                        ViewBag.EndContractDate = reader.GetString(5);

                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        ViewBag.SalesExecutiveBy = reader.GetString(6);
                    }

                }
                cn.Close();
            }
        }



        return View();
    }


    public ActionResult DeleteCustomerContract(string Id)
    {
        if (!string.IsNullOrEmpty(Id))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("DELETE [IflowSeed].[dbo].[CustomerContract] WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn.Close();
            }
        }
        return RedirectToAction("ManageCustomerContract", "MBD");
    }

    List<JobInstruction> JobInstructionlist = new List<JobInstruction>();


    List<JobInstruction> JIHistory = new List<JobInstruction>();
    public ActionResult ManageJIHistory(string Id, string ProductName, string set, JobAuditTrail get, string Customer_Name, string JobClass, string JobSheetNo, string JobRequest, string JobType, string Status,
                                            string AccountsQty, string ImpressionQty, string PagesQty, string Frequency, string JobInstructionId,
                                            string NotesByIT, string NotesByProduction, string NotesByPurchasing, string NotesByEngineering,
                                            string NotesByArtwork, string NotesByFinance, string NotesByDCP, string IT_SysNotes, string Produc_PlanningNotes,
                                            string PurchasingNotes, string EngineeringNotes, string ArtworkNotes, string Acc_BillingNotes, string DCPNotes, string product, string PostingInfo, string customer)
    {
        if (set == "search") //ini kalu user search product
        {

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Id,Customer_Name,ProductName,JobSheetNo,Status,
                                           ServiceLevel,IsSlaCreaditCard,JobClass,IsSetPaper,JobRequest,
                                           ExpectedDateCompletionToGpo,QuotationRef,
                                           JobType,DeliveryChannel,AccountsQty,ImpressionQty,
                                           PagesQty,CycleTerm,MailingDate,
                                           JoiningFiles,TotalRecord,InputFileName,OutputFileName,Sorting,
                                           SortingMode,Other,DataPrintingRemark,
                                           ArtworkStatus,PaperStock,
                                           Grammage,MaterialColour,EnvelopeStock,EnvelopeType,EnvelopeSize,
                                           EnvelopeGrammage,EnvelopeColour,EnvelopeWindow,EnvWindowOpaque,
                                           LabelStock,LabelCutsheet,OthersStock,BalancedMaterial,
                                           PlasticStock,PlasticType,PlasticSize,PlasticThickness,
                                           PrintingType,PrintingOrientation,GpoList,RegisterMail,
                                           OtherList,BaseStockType,FinishingSize,AdditionalPrintingMark,
                                           SortingCriteria,PrintingInstr,SortingInstr,Letter,Brochures_Leaflets,
                                           ReplyEnvelope,ImgOnStatement,Booklet,
                                           NumberOfInsert,FinishingInst,
                                           IT_SysNotes,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes,
                                           ArtworkNotes, Acc_BillingNotes, DCPNotes, PostingInfo,ActiveSts
                                       FROM [IflowSeed].[dbo].[JobBatchInfo]
                                     WHERE  Customer_Name LIKE @Customer_Name  ";
                command.Parameters.AddWithValue("@Customer_Name", "%" + customer + "%");

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
                            model.JobSheetNo = reader.GetString(3);
                        }

                        if (reader.IsDBNull(4) == false)
                        {
                            model.Status = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            model.ServiceLevel = reader.GetString(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.SlaCreaditCard = reader.GetBoolean(6);

                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.JobClass = reader.GetString(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.IsSetPaper = reader.GetBoolean(8);

                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            model.JobRequest = reader.GetDateTime(9);
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            model.ExpectedDateCompletionToGpo = reader.GetDateTime(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            model.QuotationRef = reader.GetString(11);
                        }


                        if (reader.IsDBNull(12) == false)
                        {
                            model.JobType = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            model.DeliveryChannel = reader.GetString(13);
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            model.AccountsQty = reader.GetString(14);
                        }
                        if (reader.IsDBNull(15) == false)
                        {
                            model.ImpressionQty = reader.GetString(15);
                        }
                        if (reader.IsDBNull(16) == false)
                        {
                            model.PagesQty = reader.GetString(16);
                        }
                        if (reader.IsDBNull(17) == false)
                        {
                            model.CycleTerm = reader.GetDateTime(17);
                        }
                        if (reader.IsDBNull(18) == false)
                        {
                            model.MailingDate = (DateTime)reader.GetDateTime(18);
                        }

                        if (reader.IsDBNull(19) == false)
                        {
                            model.JoiningFiles = reader.GetString(19);
                        }
                        if (reader.IsDBNull(20) == false)
                        {
                            model.TotalRecord = reader.GetString(20);
                        }
                        if (reader.IsDBNull(21) == false)
                        {
                            model.InputFileName = reader.GetString(21);
                        }
                        if (reader.IsDBNull(22) == false)
                        {
                            model.OutputFileName = reader.GetString(22);
                        }
                        if (reader.IsDBNull(23) == false)
                        {
                            model.Sorting = reader.GetString(23);
                        }
                        if (reader.IsDBNull(24) == false)
                        {
                            model.SortingMode = reader.GetString(24);
                        }
                        if (reader.IsDBNull(25) == false)
                        {
                            model.Other = reader.GetString(25);
                        }
                        if (reader.IsDBNull(26) == false)
                        {
                            model.DataPrintingRemark = reader.GetString(26);
                        }
                        if (reader.IsDBNull(27) == false)
                        {
                            model.ArtworkStatus = reader.GetString(27);
                        }
                        if (reader.IsDBNull(28) == false)
                        {
                            model.PaperStock = reader.GetString(28);
                        }


                        if (reader.IsDBNull(29) == false)
                        {
                            model.Grammage = reader.GetString(29);
                        }
                        if (reader.IsDBNull(30) == false)
                        {
                            model.MaterialColour = reader.GetString(30);
                        }
                        if (reader.IsDBNull(31) == false)
                        {
                            model.EnvelopeStock = reader.GetString(31);
                        }
                        if (reader.IsDBNull(32) == false)
                        {
                            model.EnvelopeType = reader.GetString(32);
                        }
                        if (reader.IsDBNull(33) == false)
                        {
                            model.EnvelopeSize = reader.GetString(33);
                        }
                        if (reader.IsDBNull(34) == false)
                        {
                            model.EnvelopeGrammage = reader.GetString(34);
                        }
                        if (reader.IsDBNull(35) == false)
                        {
                            model.EnvelopeColour = reader.GetString(35);
                        }
                        if (reader.IsDBNull(36) == false)
                        {
                            model.EnvelopeWindow = reader.GetString(36);
                        }
                        if (reader.IsDBNull(37) == false)
                        {
                            model.EnvWindowOpaque = reader.GetString(37);
                        }
                        if (reader.IsDBNull(38) == false)
                        {
                            model.LabelStock = reader.GetString(38);
                        }
                        if (reader.IsDBNull(39) == false)
                        {
                            model.LabelCutsheet = reader.GetString(39);
                        }
                        if (reader.IsDBNull(40) == false)
                        {
                            model.OthersStock = reader.GetString(40);
                        }
                        if (reader.IsDBNull(41) == false)
                        {
                            model.BalancedMaterial = reader.GetString(41);
                        }
                        if (reader.IsDBNull(42) == false)
                        {
                            model.PlasticStock = reader.GetString(42);
                        }
                        if (reader.IsDBNull(43) == false)
                        {
                            model.PlasticType = reader.GetString(43);
                        }
                        if (reader.IsDBNull(44) == false)
                        {
                            model.PlasticSize = reader.GetString(44);
                        }
                        if (reader.IsDBNull(45) == false)
                        {
                            model.PlasticThickness = reader.GetString(45);
                        }
                        if (reader.IsDBNull(46) == false)
                        {
                            model.PrintingType = reader.GetString(46);
                        }
                        if (reader.IsDBNull(47) == false)
                        {
                            model.PrintingOrientation = reader.GetString(47);
                        }
                        if (reader.IsDBNull(48) == false)
                        {
                            model.GpoList = reader.GetBoolean(48);
                        }
                        if (reader.IsDBNull(49) == false)
                        {
                            model.RegisterMail = reader.GetBoolean(49);
                        }
                        if (reader.IsDBNull(50) == false)
                        {
                            model.OtherList = reader.GetString(50);
                        }
                        if (reader.IsDBNull(51) == false)
                        {
                            model.BaseStockType = reader.GetString(51);
                        }
                        if (reader.IsDBNull(52) == false)
                        {
                            model.FinishingSize = reader.GetString(52);
                        }
                        if (reader.IsDBNull(53) == false)
                        {
                            model.AdditionalPrintingMark = reader.GetString(53);
                        }
                        if (reader.IsDBNull(54) == false)
                        {
                            model.SortingCriteria = reader.GetString(54);
                        }
                        if (reader.IsDBNull(55) == false)
                        {
                            model.PrintingInstr = reader.GetString(55);
                        }
                        if (reader.IsDBNull(56) == false)
                        {
                            model.SortingInstr = reader.GetString(56);
                        }
                        if (reader.IsDBNull(57) == false)
                        {
                            model.Letter = reader.GetBoolean(57);
                        }
                        if (reader.IsDBNull(58) == false)
                        {
                            model.Brochures_Leaflets = reader.GetBoolean(58);
                        }
                        if (reader.IsDBNull(59) == false)
                        {
                            model.ReplyEnvelope = reader.GetBoolean(59);
                        }
                        if (reader.IsDBNull(60) == false)
                        {
                            model.ImgOnStatement = reader.GetBoolean(60);
                        }
                        if (reader.IsDBNull(61) == false)
                        {
                            model.Booklet = reader.GetBoolean(61);
                        }
                        if (reader.IsDBNull(62) == false)
                        {
                            model.NumberOfInsert = reader.GetString(62);
                        }


                        if (reader.IsDBNull(63) == false)
                        {
                            model.FinishingInst = reader.GetString(63);
                        }
                        if (reader.IsDBNull(64) == false)
                        {
                            model.IT_SysNotes = reader.GetString(64);
                        }
                        if (reader.IsDBNull(65) == false)
                        {
                            model.Produc_PlanningNotes = reader.GetString(65);
                        }
                        if (reader.IsDBNull(66) == false)
                        {
                            model.PurchasingNotes = reader.GetString(66);
                        }
                        if (reader.IsDBNull(67) == false)
                        {
                            model.EngineeringNotes = reader.GetString(67);
                        }
                        if (reader.IsDBNull(68) == false)
                        {
                            model.ArtworkNotes = reader.GetString(68);
                        }
                        if (reader.IsDBNull(69) == false)
                        {
                            model.Acc_BillingNotes = reader.GetString(69);
                        }
                        if (reader.IsDBNull(70) == false)
                        {
                            model.DCPNotes = reader.GetString(70);
                        }
                        if (reader.IsDBNull(71) == false)
                        {
                            model.PostingInfo = reader.GetString(71);
                        }
                        //if (reader.IsDBNull(72) == false)
                        //{
                        //    model.ActiveSts = reader.GetString(72);
                        //}

                    }
                    JIHistory.Add(model);
                }
                cn.Close();
            }
        }

        if (set == "search2") //ini kalu user search product
        {

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Id,Customer_Name,ProductName,JobSheetNo,SalesExecutiveBy,Status,
                                           ServiceLevel,IsSlaCreaditCard,JobClass,IsSetPaper,JobRequest,
                                           ExpectedDateCompletionToGpo,QuotationRef,ContractName,
                                           Contact_Person,JobType,DeliveryChannel,AccountsQty,ImpressionQty,
                                           PagesQty,CycleTerm,MailingDate,
                                           JoiningFiles,TotalRecord,InputFileName,OutputFileName,Sorting,
                                           SortingMode,Other,DataPrintingRemark,
                                           ArtworkStatus,PaperStock,TypeCode,Paper,PaperSize,
                                           Grammage,MaterialColour,EnvelopeStock,EnvelopeType,EnvelopeSize,
                                           EnvelopeGrammage,EnvelopeColour,EnvelopeWindow,EnvWindowOpaque,
                                           LabelStock,LabelCutsheet,OthersStock,BalancedMaterial,
                                           PlasticStock,PlasticType,PlasticSize,PlasticThickness,
                                           PrintingType,PrintingOrientation,GpoList,RegisterMail,
                                           OtherList,BaseStockType,FinishingSize,AdditionalPrintingMark,
                                           SortingCriteria,PrintingInstr,SortingInstr,Letter,Brochures_Leaflets,
                                           ReplyEnvelope,ImgOnStatement,Booklet,
                                           NumberOfInsert,Magezine1,Brochure1,CarrierSheet1,Newsletter1,
                                           Statement1,Booklet1,CommentManualType,FinishingFormat,
                                           FoldingType,Sealing1,Tearing1,BarcodeLabel1,Cutting1,
                                           StickingOf1,AddLabel1,Sticker1,Chesire1,Tuck_In1,
                                           Bursting1,Sealed1,Folding1,Unsealed1,Letter1,FinishingInst,
                                           IT_SysNotes,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes,
                                           ArtworkNotes, Acc_BillingNotes, DCPNotes, PostingInfo,ProgrammerBy,NewMR,Confrm100
                                       FROM [IflowSeed].[dbo].[JobInstruction] WHERE Status != 'New' AND (JobSheetNo LIKE @JobSheetNo OR Customer_Name LIKE @JobSheetNo OR ProductName LIKE @JobSheetNo)";
                command.Parameters.AddWithValue("@JobSheetNo", "%" + JobSheetNo + "%");
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
                            model.JobSheetNo = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            model.SalesExecutiveBy = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            model.Status = reader.GetString(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.ServiceLevel = reader.GetString(6);
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.SlaCreaditCard = reader.GetBoolean(7);

                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.JobClass = reader.GetString(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            model.IsSetPaper = reader.GetBoolean(9);

                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            model.JobRequest = reader.GetDateTime(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            model.ExpectedDateCompletionToGpo = reader.GetDateTime(11);
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            model.QuotationRef = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            model.ContractName = reader.GetString(13);
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            model.Contact_Person = reader.GetString(14);
                        }
                        if (reader.IsDBNull(15) == false)
                        {
                            model.JobType = reader.GetString(15);
                        }
                        if (reader.IsDBNull(16) == false)
                        {
                            model.DeliveryChannel = reader.GetString(16);
                        }
                        if (reader.IsDBNull(17) == false)
                        {
                            model.AccountsQty = reader.GetString(17);
                        }
                        if (reader.IsDBNull(18) == false)
                        {
                            model.ImpressionQty = reader.GetString(18);
                        }
                        if (reader.IsDBNull(19) == false)
                        {
                            model.PagesQty = reader.GetString(19);
                        }
                        if (reader.IsDBNull(20) == false)
                        {
                            model.CycleTerm = reader.GetDateTime(20);
                        }
                        if (reader.IsDBNull(21) == false)
                        {
                            model.MailingDate = (DateTime)reader.GetDateTime(21);
                        }

                        if (reader.IsDBNull(22) == false)
                        {
                            model.JoiningFiles = reader.GetString(22);
                        }
                        if (reader.IsDBNull(23) == false)
                        {
                            model.TotalRecord = reader.GetString(23);
                        }
                        if (reader.IsDBNull(24) == false)
                        {
                            model.InputFileName = reader.GetString(24);
                        }
                        if (reader.IsDBNull(25) == false)
                        {
                            model.OutputFileName = reader.GetString(25);
                        }
                        if (reader.IsDBNull(26) == false)
                        {
                            model.Sorting = reader.GetString(26);
                        }
                        if (reader.IsDBNull(27) == false)
                        {
                            model.SortingMode = reader.GetString(27);
                        }
                        if (reader.IsDBNull(28) == false)
                        {
                            model.Other = reader.GetString(28);
                        }
                        if (reader.IsDBNull(29) == false)
                        {
                            model.DataPrintingRemark = reader.GetString(29);
                        }
                        if (reader.IsDBNull(30) == false)
                        {
                            model.ArtworkStatus = reader.GetString(30);
                        }
                        if (reader.IsDBNull(31) == false)
                        {
                            model.PaperStock = reader.GetString(31);
                        }
                        if (reader.IsDBNull(32) == false)
                        {
                            model.TypeCode = reader.GetString(32);
                        }
                        if (reader.IsDBNull(33) == false)
                        {
                            model.Paper = reader.GetString(33);
                        }
                        if (reader.IsDBNull(34) == false)
                        {
                            model.PaperSize = reader.GetString(34);
                        }
                        if (reader.IsDBNull(35) == false)
                        {
                            model.Grammage = reader.GetString(35);
                        }
                        if (reader.IsDBNull(36) == false)
                        {
                            model.MaterialColour = reader.GetString(36);
                        }
                        if (reader.IsDBNull(37) == false)
                        {
                            model.EnvelopeStock = reader.GetString(37);
                        }
                        if (reader.IsDBNull(38) == false)
                        {
                            model.EnvelopeType = reader.GetString(38);
                        }
                        if (reader.IsDBNull(39) == false)
                        {
                            model.EnvelopeSize = reader.GetString(39);
                        }
                        if (reader.IsDBNull(40) == false)
                        {
                            model.EnvelopeGrammage = reader.GetString(40);
                        }
                        if (reader.IsDBNull(41) == false)
                        {
                            model.EnvelopeColour = reader.GetString(41);
                        }
                        if (reader.IsDBNull(42) == false)
                        {
                            model.EnvelopeWindow = reader.GetString(42);
                        }
                        if (reader.IsDBNull(43) == false)
                        {
                            model.EnvWindowOpaque = reader.GetString(43);
                        }
                        if (reader.IsDBNull(44) == false)
                        {
                            model.LabelStock = reader.GetString(44);
                        }
                        if (reader.IsDBNull(45) == false)
                        {
                            model.LabelCutsheet = reader.GetString(45);
                        }
                        if (reader.IsDBNull(46) == false)
                        {
                            model.OthersStock = reader.GetString(46);
                        }
                        if (reader.IsDBNull(47) == false)
                        {
                            model.BalancedMaterial = reader.GetString(47);
                        }
                        if (reader.IsDBNull(48) == false)
                        {
                            model.PlasticStock = reader.GetString(48);
                        }
                        if (reader.IsDBNull(49) == false)
                        {
                            model.PlasticType = reader.GetString(49);
                        }
                        if (reader.IsDBNull(50) == false)
                        {
                            model.PlasticSize = reader.GetString(50);
                        }
                        if (reader.IsDBNull(51) == false)
                        {
                            model.PlasticThickness = reader.GetString(51);
                        }
                        if (reader.IsDBNull(52) == false)
                        {
                            model.PrintingType = reader.GetString(52);
                        }
                        if (reader.IsDBNull(53) == false)
                        {
                            model.PrintingOrientation = reader.GetString(53);
                        }
                        if (reader.IsDBNull(54) == false)
                        {
                            model.GpoList = reader.GetBoolean(54);
                        }
                        if (reader.IsDBNull(55) == false)
                        {
                            model.RegisterMail = reader.GetBoolean(55);
                        }
                        if (reader.IsDBNull(56) == false)
                        {
                            model.OtherList = reader.GetString(56);
                        }
                        if (reader.IsDBNull(57) == false)
                        {
                            model.BaseStockType = reader.GetString(57);
                        }
                        if (reader.IsDBNull(58) == false)
                        {
                            model.FinishingSize = reader.GetString(58);
                        }
                        if (reader.IsDBNull(59) == false)
                        {
                            model.AdditionalPrintingMark = reader.GetString(59);
                        }
                        if (reader.IsDBNull(60) == false)
                        {
                            model.SortingCriteria = reader.GetString(60);
                        }
                        if (reader.IsDBNull(61) == false)
                        {
                            model.PrintingInstr = reader.GetString(61);
                        }
                        if (reader.IsDBNull(62) == false)
                        {
                            model.SortingInstr = reader.GetString(62);
                        }
                        if (reader.IsDBNull(63) == false)
                        {
                            model.Letter = reader.GetBoolean(63);
                        }
                        if (reader.IsDBNull(64) == false)
                        {
                            model.Brochures_Leaflets = reader.GetBoolean(64);
                        }
                        if (reader.IsDBNull(65) == false)
                        {
                            model.ReplyEnvelope = reader.GetBoolean(65);
                        }
                        if (reader.IsDBNull(66) == false)
                        {
                            model.ImgOnStatement = reader.GetBoolean(66);
                        }
                        if (reader.IsDBNull(67) == false)
                        {
                            model.Booklet = reader.GetBoolean(67);
                        }
                        if (reader.IsDBNull(68) == false)
                        {
                            model.NumberOfInsert = reader.GetString(68);
                        }
                        if (reader.IsDBNull(69) == false)
                        {
                            model.Magezine1 = reader.GetBoolean(69);
                        }
                        if (reader.IsDBNull(70) == false)
                        {
                            model.Brochure1 = reader.GetBoolean(70);
                        }
                        if (reader.IsDBNull(71) == false)
                        {
                            model.CarrierSheet1 = reader.GetBoolean(71);
                        }
                        if (reader.IsDBNull(72) == false)
                        {
                            model.Newsletter1 = reader.GetBoolean(72);
                        }
                        if (reader.IsDBNull(73) == false)
                        {
                            model.Statement1 = reader.GetBoolean(73);
                        }
                        if (reader.IsDBNull(74) == false)
                        {
                            model.Booklet1 = reader.GetBoolean(74);
                        }
                        if (reader.IsDBNull(75) == false)
                        {
                            model.CommentManualType = reader.GetString(75);
                        }
                        if (reader.IsDBNull(76) == false)
                        {
                            model.FinishingFormat = reader.GetString(76);
                        }
                        if (reader.IsDBNull(77) == false)
                        {
                            model.FoldingType = reader.GetString(77);
                        }
                        if (reader.IsDBNull(78) == false)
                        {
                            model.Sealing1 = reader.GetBoolean(78);
                        }
                        if (reader.IsDBNull(79) == false)
                        {
                            model.Tearing1 = reader.GetBoolean(79);
                        }
                        if (reader.IsDBNull(80) == false)
                        {
                            model.BarcodeLabel1 = reader.GetBoolean(80);
                        }
                        if (reader.IsDBNull(81) == false)
                        {
                            model.Cutting1 = reader.GetBoolean(81);
                        }
                        if (reader.IsDBNull(82) == false)
                        {
                            model.StickingOf1 = reader.GetString(82);
                        }
                        if (reader.IsDBNull(83) == false)
                        {
                            model.AddLabel1 = reader.GetBoolean(83);
                        }
                        if (reader.IsDBNull(84) == false)
                        {
                            model.Sticker1 = reader.GetBoolean(84);
                        }
                        if (reader.IsDBNull(85) == false)
                        {
                            model.Chesire1 = reader.GetBoolean(85);
                        }
                        if (reader.IsDBNull(86) == false)
                        {
                            model.Tuck_In1 = reader.GetBoolean(86);
                        }
                        if (reader.IsDBNull(87) == false)
                        {
                            model.Bursting1 = reader.GetBoolean(87);
                        }
                        if (reader.IsDBNull(88) == false)
                        {
                            model.Sealed1 = reader.GetBoolean(88);
                        }
                        if (reader.IsDBNull(89) == false)
                        {
                            model.Folding1 = reader.GetBoolean(89);
                        }
                        if (reader.IsDBNull(90) == false)
                        {
                            model.Unsealed1 = reader.GetBoolean(90);
                        }
                        if (reader.IsDBNull(91) == false)
                        {
                            model.Letter1 = reader.GetBoolean(91);
                        }
                        if (reader.IsDBNull(92) == false)
                        {
                            model.FinishingInst = reader.GetString(92);
                        }
                        if (reader.IsDBNull(93) == false)
                        {
                            model.IT_SysNotes = reader.GetString(93);
                        }
                        if (reader.IsDBNull(94) == false)
                        {
                            model.Produc_PlanningNotes = reader.GetString(94);
                        }
                        if (reader.IsDBNull(95) == false)
                        {
                            model.PurchasingNotes = reader.GetString(95);
                        }
                        if (reader.IsDBNull(96) == false)
                        {
                            model.EngineeringNotes = reader.GetString(96);
                        }
                        if (reader.IsDBNull(97) == false)
                        {
                            model.ArtworkNotes = reader.GetString(97);
                        }
                        if (reader.IsDBNull(98) == false)
                        {
                            model.Acc_BillingNotes = reader.GetString(98);
                        }
                        if (reader.IsDBNull(99) == false)
                        {
                            model.DCPNotes = reader.GetString(99);
                        }
                        if (reader.IsDBNull(100) == false)
                        {
                            model.PostingInfo = reader.GetString(100);
                        }
                        if (reader["NewMR"] != null)
                        {
                            model.NewMR = reader["NewMR"].ToString();
                        }
                        if (reader["Confrm100"] != null)
                        {
                            model.Confrm100 = reader["Confrm100"].ToString();
                        }
                    }
                    JIHistory.Add(model);

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
                command.CommandText = @"SELECT Id,Customer_Name,ProductName,JobSheetNo,SalesExecutiveBy,Status,
                                           ServiceLevel,IsSlaCreaditCard,JobClass,IsSetPaper,JobRequest,
                                           ExpectedDateCompletionToGpo,QuotationRef,ContractName,
                                           Contact_Person,JobType,DeliveryChannel,AccountsQty,ImpressionQty,
                                           PagesQty,CycleTerm,MailingDate,
                                           JoiningFiles,TotalRecord,InputFileName,OutputFileName,Sorting,
                                           SortingMode,Other,DataPrintingRemark,
                                           ArtworkStatus,PaperStock,TypeCode,Paper,PaperSize,
                                           Grammage,MaterialColour,EnvelopeStock,EnvelopeType,EnvelopeSize,
                                           EnvelopeGrammage,EnvelopeColour,EnvelopeWindow,EnvWindowOpaque,
                                           LabelStock,LabelCutsheet,OthersStock,BalancedMaterial,
                                           PlasticStock,PlasticType,PlasticSize,PlasticThickness,
                                           PrintingType,PrintingOrientation,GpoList,RegisterMail,
                                           OtherList,BaseStockType,FinishingSize,AdditionalPrintingMark,
                                           SortingCriteria,PrintingInstr,SortingInstr,Letter,Brochures_Leaflets,
                                           ReplyEnvelope,ImgOnStatement,Booklet,
                                           NumberOfInsert,Magezine1,Brochure1,CarrierSheet1,Newsletter1,
                                           Statement1,Booklet1,CommentManualType,FinishingFormat,
                                           FoldingType,Sealing1,Tearing1,BarcodeLabel1,Cutting1,
                                           StickingOf1,AddLabel1,Sticker1,Chesire1,Tuck_In1,
                                           Bursting1,Sealed1,Folding1,Unsealed1,Letter1,FinishingInst,
                                           IT_SysNotes,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes,
                                           ArtworkNotes, Acc_BillingNotes, DCPNotes, PostingInfo,ProgrammerBy,NewMR,Confrm100
                                       FROM [IflowSeed].[dbo].[JobInstruction] WHERE Status != 'New' ";
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
                            model.JobSheetNo = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            model.SalesExecutiveBy = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            model.Status = reader.GetString(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.ServiceLevel = reader.GetString(6);
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.SlaCreaditCard = reader.GetBoolean(7);

                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.JobClass = reader.GetString(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            model.IsSetPaper = reader.GetBoolean(9);

                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            model.JobRequest = reader.GetDateTime(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            model.ExpectedDateCompletionToGpo = reader.GetDateTime(11);
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            model.QuotationRef = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            model.ContractName = reader.GetString(13);
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            model.Contact_Person = reader.GetString(14);
                        }
                        if (reader.IsDBNull(15) == false)
                        {
                            model.JobType = reader.GetString(15);
                        }
                        if (reader.IsDBNull(16) == false)
                        {
                            model.DeliveryChannel = reader.GetString(16);
                        }
                        if (reader.IsDBNull(17) == false)
                        {
                            model.AccountsQty = reader.GetString(17);
                        }
                        if (reader.IsDBNull(18) == false)
                        {
                            model.ImpressionQty = reader.GetString(18);
                        }
                        if (reader.IsDBNull(19) == false)
                        {
                            model.PagesQty = reader.GetString(19);
                        }
                        if (reader.IsDBNull(20) == false)
                        {
                            model.CycleTerm = reader.GetDateTime(20);
                        }
                        if (reader.IsDBNull(21) == false)
                        {
                            model.MailingDate = (DateTime)reader.GetDateTime(21);
                        }

                        if (reader.IsDBNull(22) == false)
                        {
                            model.JoiningFiles = reader.GetString(22);
                        }
                        if (reader.IsDBNull(23) == false)
                        {
                            model.TotalRecord = reader.GetString(23);
                        }
                        if (reader.IsDBNull(24) == false)
                        {
                            model.InputFileName = reader.GetString(24);
                        }
                        if (reader.IsDBNull(25) == false)
                        {
                            model.OutputFileName = reader.GetString(25);
                        }
                        if (reader.IsDBNull(26) == false)
                        {
                            model.Sorting = reader.GetString(26);
                        }
                        if (reader.IsDBNull(27) == false)
                        {
                            model.SortingMode = reader.GetString(27);
                        }
                        if (reader.IsDBNull(28) == false)
                        {
                            model.Other = reader.GetString(28);
                        }
                        if (reader.IsDBNull(29) == false)
                        {
                            model.DataPrintingRemark = reader.GetString(29);
                        }
                        if (reader.IsDBNull(30) == false)
                        {
                            model.ArtworkStatus = reader.GetString(30);
                        }
                        if (reader.IsDBNull(31) == false)
                        {
                            model.PaperStock = reader.GetString(31);
                        }
                        if (reader.IsDBNull(32) == false)
                        {
                            model.TypeCode = reader.GetString(32);
                        }
                        if (reader.IsDBNull(33) == false)
                        {
                            model.Paper = reader.GetString(33);
                        }
                        if (reader.IsDBNull(34) == false)
                        {
                            model.PaperSize = reader.GetString(34);
                        }
                        if (reader.IsDBNull(35) == false)
                        {
                            model.Grammage = reader.GetString(35);
                        }
                        if (reader.IsDBNull(36) == false)
                        {
                            model.MaterialColour = reader.GetString(36);
                        }
                        if (reader.IsDBNull(37) == false)
                        {
                            model.EnvelopeStock = reader.GetString(37);
                        }
                        if (reader.IsDBNull(38) == false)
                        {
                            model.EnvelopeType = reader.GetString(38);
                        }
                        if (reader.IsDBNull(39) == false)
                        {
                            model.EnvelopeSize = reader.GetString(39);
                        }
                        if (reader.IsDBNull(40) == false)
                        {
                            model.EnvelopeGrammage = reader.GetString(40);
                        }
                        if (reader.IsDBNull(41) == false)
                        {
                            model.EnvelopeColour = reader.GetString(41);
                        }
                        if (reader.IsDBNull(42) == false)
                        {
                            model.EnvelopeWindow = reader.GetString(42);
                        }
                        if (reader.IsDBNull(43) == false)
                        {
                            model.EnvWindowOpaque = reader.GetString(43);
                        }
                        if (reader.IsDBNull(44) == false)
                        {
                            model.LabelStock = reader.GetString(44);
                        }
                        if (reader.IsDBNull(45) == false)
                        {
                            model.LabelCutsheet = reader.GetString(45);
                        }
                        if (reader.IsDBNull(46) == false)
                        {
                            model.OthersStock = reader.GetString(46);
                        }
                        if (reader.IsDBNull(47) == false)
                        {
                            model.BalancedMaterial = reader.GetString(47);
                        }
                        if (reader.IsDBNull(48) == false)
                        {
                            model.PlasticStock = reader.GetString(48);
                        }
                        if (reader.IsDBNull(49) == false)
                        {
                            model.PlasticType = reader.GetString(49);
                        }
                        if (reader.IsDBNull(50) == false)
                        {
                            model.PlasticSize = reader.GetString(50);
                        }
                        if (reader.IsDBNull(51) == false)
                        {
                            model.PlasticThickness = reader.GetString(51);
                        }
                        if (reader.IsDBNull(52) == false)
                        {
                            model.PrintingType = reader.GetString(52);
                        }
                        if (reader.IsDBNull(53) == false)
                        {
                            model.PrintingOrientation = reader.GetString(53);
                        }
                        if (reader.IsDBNull(54) == false)
                        {
                            model.GpoList = reader.GetBoolean(54);
                        }
                        if (reader.IsDBNull(55) == false)
                        {
                            model.RegisterMail = reader.GetBoolean(55);
                        }
                        if (reader.IsDBNull(56) == false)
                        {
                            model.OtherList = reader.GetString(56);
                        }
                        if (reader.IsDBNull(57) == false)
                        {
                            model.BaseStockType = reader.GetString(57);
                        }
                        if (reader.IsDBNull(58) == false)
                        {
                            model.FinishingSize = reader.GetString(58);
                        }
                        if (reader.IsDBNull(59) == false)
                        {
                            model.AdditionalPrintingMark = reader.GetString(59);
                        }
                        if (reader.IsDBNull(60) == false)
                        {
                            model.SortingCriteria = reader.GetString(60);
                        }
                        if (reader.IsDBNull(61) == false)
                        {
                            model.PrintingInstr = reader.GetString(61);
                        }
                        if (reader.IsDBNull(62) == false)
                        {
                            model.SortingInstr = reader.GetString(62);
                        }
                        if (reader.IsDBNull(63) == false)
                        {
                            model.Letter = reader.GetBoolean(63);
                        }
                        if (reader.IsDBNull(64) == false)
                        {
                            model.Brochures_Leaflets = reader.GetBoolean(64);
                        }
                        if (reader.IsDBNull(65) == false)
                        {
                            model.ReplyEnvelope = reader.GetBoolean(65);
                        }
                        if (reader.IsDBNull(66) == false)
                        {
                            model.ImgOnStatement = reader.GetBoolean(66);
                        }
                        if (reader.IsDBNull(67) == false)
                        {
                            model.Booklet = reader.GetBoolean(67);
                        }
                        if (reader.IsDBNull(68) == false)
                        {
                            model.NumberOfInsert = reader.GetString(68);
                        }
                        if (reader.IsDBNull(69) == false)
                        {
                            model.Magezine1 = reader.GetBoolean(69);
                        }
                        if (reader.IsDBNull(70) == false)
                        {
                            model.Brochure1 = reader.GetBoolean(70);
                        }
                        if (reader.IsDBNull(71) == false)
                        {
                            model.CarrierSheet1 = reader.GetBoolean(71);
                        }
                        if (reader.IsDBNull(72) == false)
                        {
                            model.Newsletter1 = reader.GetBoolean(72);
                        }
                        if (reader.IsDBNull(73) == false)
                        {
                            model.Statement1 = reader.GetBoolean(73);
                        }
                        if (reader.IsDBNull(74) == false)
                        {
                            model.Booklet1 = reader.GetBoolean(74);
                        }
                        if (reader.IsDBNull(75) == false)
                        {
                            model.CommentManualType = reader.GetString(75);
                        }
                        if (reader.IsDBNull(76) == false)
                        {
                            model.FinishingFormat = reader.GetString(76);
                        }
                        if (reader.IsDBNull(77) == false)
                        {
                            model.FoldingType = reader.GetString(77);
                        }
                        if (reader.IsDBNull(78) == false)
                        {
                            model.Sealing1 = reader.GetBoolean(78);
                        }
                        if (reader.IsDBNull(79) == false)
                        {
                            model.Tearing1 = reader.GetBoolean(79);
                        }
                        if (reader.IsDBNull(80) == false)
                        {
                            model.BarcodeLabel1 = reader.GetBoolean(80);
                        }
                        if (reader.IsDBNull(81) == false)
                        {
                            model.Cutting1 = reader.GetBoolean(81);
                        }
                        if (reader.IsDBNull(82) == false)
                        {
                            model.StickingOf1 = reader.GetString(82);
                        }
                        if (reader.IsDBNull(83) == false)
                        {
                            model.AddLabel1 = reader.GetBoolean(83);
                        }
                        if (reader.IsDBNull(84) == false)
                        {
                            model.Sticker1 = reader.GetBoolean(84);
                        }
                        if (reader.IsDBNull(85) == false)
                        {
                            model.Chesire1 = reader.GetBoolean(85);
                        }
                        if (reader.IsDBNull(86) == false)
                        {
                            model.Tuck_In1 = reader.GetBoolean(86);
                        }
                        if (reader.IsDBNull(87) == false)
                        {
                            model.Bursting1 = reader.GetBoolean(87);
                        }
                        if (reader.IsDBNull(88) == false)
                        {
                            model.Sealed1 = reader.GetBoolean(88);
                        }
                        if (reader.IsDBNull(89) == false)
                        {
                            model.Folding1 = reader.GetBoolean(89);
                        }
                        if (reader.IsDBNull(90) == false)
                        {
                            model.Unsealed1 = reader.GetBoolean(90);
                        }
                        if (reader.IsDBNull(91) == false)
                        {
                            model.Letter1 = reader.GetBoolean(91);
                        }
                        if (reader.IsDBNull(92) == false)
                        {
                            model.FinishingInst = reader.GetString(92);
                        }
                        if (reader.IsDBNull(93) == false)
                        {
                            model.IT_SysNotes = reader.GetString(93);
                        }
                        if (reader.IsDBNull(94) == false)
                        {
                            model.Produc_PlanningNotes = reader.GetString(94);
                        }
                        if (reader.IsDBNull(95) == false)
                        {
                            model.PurchasingNotes = reader.GetString(95);
                        }
                        if (reader.IsDBNull(96) == false)
                        {
                            model.EngineeringNotes = reader.GetString(96);
                        }
                        if (reader.IsDBNull(97) == false)
                        {
                            model.ArtworkNotes = reader.GetString(97);
                        }
                        if (reader.IsDBNull(98) == false)
                        {
                            model.Acc_BillingNotes = reader.GetString(98);
                        }
                        if (reader.IsDBNull(99) == false)
                        {
                            model.DCPNotes = reader.GetString(99);
                        }
                        if (reader.IsDBNull(100) == false)
                        {
                            model.PostingInfo = reader.GetString(100);
                        }
                        if (reader["NewMR"] != null)
                        {
                            model.NewMR = reader["NewMR"].ToString();
                        }
                        if (reader["Confrm100"] != null)
                        {
                            model.Confrm100 = reader["Confrm100"].ToString();
                        }
                    }
                    JIHistory.Add(model);

                }
                cn.Close();
            }


            

        }
        return View(JIHistory); //hntr data ke ui
    }


    //List<JobInstruction> JobInstructionlist = new List<JobInstruction>();
    public ActionResult ManageJobInstruction(string Id, string ProductName, string set, JobAuditTrail get, string Customer_Name, string JobClass, string JobSheetNo, string JobRequest, string JobType, string Status,
                                            string AccountsQty, string ImpressionQty, string PagesQty, string Frequency, string JobInstructionId,
                                            string NotesByIT, string NotesByProduction, string NotesByPurchasing, string NotesByEngineering,
                                            string NotesByArtwork, string NotesByFinance, string NotesByDCP, string IT_SysNotes, string Produc_PlanningNotes,
                                            string PurchasingNotes, string EngineeringNotes, string ArtworkNotes, string Acc_BillingNotes, string DCPNotes, string product, string PostingInfo, string NewMR)
    {
        List<string> StatusCorrection = new List<string>();

        if (set == "search") //ini kalu user search product
        {

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Id,Customer_Name,ProductName,JobSheetNo,SalesExecutiveBy,Status,
                                           ServiceLevel,IsSlaCreaditCard,JobClass,IsSetPaper,JobRequest,
                                           ExpectedDateCompletionToGpo,QuotationRef,ContractName,
                                           Contact_Person,JobType,DeliveryChannel,AccountsQty,ImpressionQty,
                                           PagesQty,CycleTerm,MailingDate,
                                           JoiningFiles,TotalRecord,InputFileName,OutputFileName,Sorting,
                                           SortingMode,Other,DataPrintingRemark,
                                           ArtworkStatus,PaperStock,TypeCode,Paper,PaperSize,
                                           Grammage,MaterialColour,EnvelopeStock,EnvelopeType,EnvelopeSize,
                                           EnvelopeGrammage,EnvelopeColour,EnvelopeWindow,EnvWindowOpaque,
                                           LabelStock,LabelCutsheet,OthersStock,BalancedMaterial,
                                           PlasticStock,PlasticType,PlasticSize,PlasticThickness,
                                           PrintingType,PrintingOrientation,GpoList,RegisterMail,
                                           OtherList,BaseStockType,FinishingSize,AdditionalPrintingMark,
                                           SortingCriteria,PrintingInstr,SortingInstr,Letter,Brochures_Leaflets,
                                           ReplyEnvelope,ImgOnStatement,Booklet,
                                           NumberOfInsert,Magezine1,Brochure1,CarrierSheet1,Newsletter1,
                                           Statement1,Booklet1,CommentManualType,FinishingFormat,
                                           FoldingType,Sealing1,Tearing1,BarcodeLabel1,Cutting1,
                                           StickingOf1,AddLabel1,Sticker1,Chesire1,Tuck_In1,
                                           Bursting1,Sealed1,Folding1,Unsealed1,Letter1,FinishingInst,
                                           IT_SysNotes,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes,
                                           ArtworkNotes, Acc_BillingNotes, DCPNotes , PostingInfo,ProgrammerBy,NewMR,Confrm100
                                     FROM [IflowSeed].[dbo].[JobInstruction]
                                     WHERE  ProductName LIKE @ProductName OR Customer_Name=@ProductName OR SalesExecutiveBy=@ProductName OR ProgrammerBy=@ProductName
                                     AND Status = 'New' OR Status ='Waiting to Assign Programme' OR Status ='Development Process' OR Status ='Development Complete' OR Status='Need correction from MBD'
                                     ORDER BY Customer_Name ASC";
                command.Parameters.AddWithValue("@ProductName", "%" + product + "%");
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
                            model.JobSheetNo = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            model.SalesExecutiveBy = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            model.Status = reader.GetString(5);
                            StatusCorrection.Add(reader.GetString(5));
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.ServiceLevel = reader.GetString(6);
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.SlaCreaditCard = reader.GetBoolean(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.JobClass = reader.GetString(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            model.IsSetPaper = reader.GetBoolean(9);
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            model.JobRequest = reader.GetDateTime(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            model.ExpectedDateCompletionToGpo = reader.GetDateTime(11);
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            model.QuotationRef = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            model.ContractName = reader.GetString(13);
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            model.Contact_Person = reader.GetString(14);
                        }
                        if (reader.IsDBNull(15) == false)
                        {
                            model.JobType = reader.GetString(15);
                        }
                        if (reader.IsDBNull(16) == false)
                        {
                            model.DeliveryChannel = reader.GetString(16);
                        }
                        if (reader.IsDBNull(17) == false)
                        {
                            model.AccountsQty = reader.GetString(17);
                        }
                        if (reader.IsDBNull(18) == false)
                        {
                            model.ImpressionQty = reader.GetString(18);
                        }
                        if (reader.IsDBNull(19) == false)
                        {
                            model.PagesQty = reader.GetString(19);
                        }
                        if (reader.IsDBNull(20) == false)
                        {
                            model.CycleTerm = reader.GetDateTime(20);
                        }
                        if (reader.IsDBNull(21) == false)
                        {
                            model.MailingDate = reader.GetDateTime(21);
                        }
                        if (reader.IsDBNull(22) == false)
                        {
                            model.JoiningFiles = reader.GetString(22);
                        }
                        if (reader.IsDBNull(23) == false)
                        {
                            model.TotalRecord = reader.GetString(23);
                        }
                        if (reader.IsDBNull(24) == false)
                        {
                            model.InputFileName = reader.GetString(24);
                        }
                        if (reader.IsDBNull(25) == false)
                        {
                            model.OutputFileName = reader.GetString(25);
                        }
                        if (reader.IsDBNull(26) == false)
                        {
                            model.Sorting = reader.GetString(26);
                        }
                        if (reader.IsDBNull(27) == false)
                        {
                            model.SortingMode = reader.GetString(27);
                        }
                        if (reader.IsDBNull(28) == false)
                        {
                            model.Other = reader.GetString(28);
                        }
                        if (reader.IsDBNull(29) == false)
                        {
                            model.DataPrintingRemark = reader.GetString(29);
                        }
                        if (reader.IsDBNull(30) == false)
                        {
                            model.ArtworkStatus = reader.GetString(30);
                        }
                        if (reader.IsDBNull(31) == false)
                        {
                            model.PaperStock = reader.GetString(31);
                        }
                        if (reader.IsDBNull(32) == false)
                        {
                            model.TypeCode = reader.GetString(32);
                        }
                        if (reader.IsDBNull(33) == false)
                        {
                            model.Paper = reader.GetString(33);
                        }
                        if (reader.IsDBNull(34) == false)
                        {
                            model.PaperSize = reader.GetString(34);
                        }
                        if (reader.IsDBNull(35) == false)
                        {
                            model.Grammage = reader.GetString(35);
                        }
                        if (reader.IsDBNull(36) == false)
                        {
                            model.MaterialColour = reader.GetString(36);
                        }
                        if (reader.IsDBNull(37) == false)
                        {
                            model.EnvelopeStock = reader.GetString(37);
                        }
                        if (reader.IsDBNull(38) == false)
                        {
                            model.EnvelopeType = reader.GetString(38);
                        }
                        if (reader.IsDBNull(39) == false)
                        {
                            model.EnvelopeSize = reader.GetString(39);
                        }
                        if (reader.IsDBNull(40) == false)
                        {
                            model.EnvelopeGrammage = reader.GetString(40);
                        }
                        if (reader.IsDBNull(41) == false)
                        {
                            model.EnvelopeColour = reader.GetString(41);
                        }
                        if (reader.IsDBNull(42) == false)
                        {
                            model.EnvelopeWindow = reader.GetString(42);
                        }
                        if (reader.IsDBNull(43) == false)
                        {
                            model.EnvWindowOpaque = reader.GetString(43);
                        }
                        if (reader.IsDBNull(44) == false)
                        {
                            model.LabelStock = reader.GetString(44);
                        }
                        if (reader.IsDBNull(45) == false)
                        {
                            model.LabelCutsheet = reader.GetString(45);
                        }
                        if (reader.IsDBNull(46) == false)
                        {
                            model.OthersStock = reader.GetString(46);
                        }
                        if (reader.IsDBNull(47) == false)
                        {
                            model.BalancedMaterial = reader.GetString(47);
                        }
                        if (reader.IsDBNull(48) == false)
                        {
                            model.PlasticStock = reader.GetString(48);
                        }
                        if (reader.IsDBNull(49) == false)
                        {
                            model.PlasticType = reader.GetString(49);
                        }
                        if (reader.IsDBNull(50) == false)
                        {
                            model.PlasticSize = reader.GetString(50);
                        }
                        if (reader.IsDBNull(51) == false)
                        {
                            model.PlasticThickness = reader.GetString(51);
                        }
                        if (reader.IsDBNull(52) == false)
                        {
                            model.PrintingType = reader.GetString(52);
                        }
                        if (reader.IsDBNull(53) == false)
                        {
                            model.PrintingOrientation = reader.GetString(53);
                        }
                        if (reader.IsDBNull(54) == false)
                        {
                            model.GpoList = reader.GetBoolean(54);
                        }
                        if (reader.IsDBNull(55) == false)
                        {
                            model.RegisterMail = reader.GetBoolean(55);
                        }
                        if (reader.IsDBNull(56) == false)
                        {
                            model.OtherList = reader.GetString(56);
                        }
                        if (reader.IsDBNull(57) == false)
                        {
                            model.BaseStockType = reader.GetString(57);
                        }
                        if (reader.IsDBNull(58) == false)
                        {
                            model.FinishingSize = reader.GetString(58);
                        }
                        if (reader.IsDBNull(59) == false)
                        {
                            model.AdditionalPrintingMark = reader.GetString(59);
                        }
                        if (reader.IsDBNull(60) == false)
                        {
                            model.SortingCriteria = reader.GetString(60);
                        }
                        if (reader.IsDBNull(61) == false)
                        {
                            model.PrintingInstr = reader.GetString(61);
                        }
                        if (reader.IsDBNull(62) == false)
                        {
                            model.SortingInstr = reader.GetString(62);
                        }
                        if (reader.IsDBNull(63) == false)
                        {
                            model.Letter = reader.GetBoolean(63);
                        }
                        if (reader.IsDBNull(64) == false)
                        {
                            model.Brochures_Leaflets = reader.GetBoolean(64);
                        }
                        if (reader.IsDBNull(65) == false)
                        {
                            model.ReplyEnvelope = reader.GetBoolean(65);
                        }
                        if (reader.IsDBNull(66) == false)
                        {
                            model.ImgOnStatement = reader.GetBoolean(66);
                        }
                        if (reader.IsDBNull(67) == false)
                        {
                            model.Booklet = reader.GetBoolean(67);
                        }
                        if (reader.IsDBNull(68) == false)
                        {
                            model.NumberOfInsert = reader.GetString(68);
                        }
                        if (reader.IsDBNull(69) == false)
                        {
                            model.Magezine1 = reader.GetBoolean(69);
                        }
                        if (reader.IsDBNull(70) == false)
                        {
                            model.Brochure1 = reader.GetBoolean(70);
                        }
                        if (reader.IsDBNull(71) == false)
                        {
                            model.CarrierSheet1 = reader.GetBoolean(71);
                        }
                        if (reader.IsDBNull(72) == false)
                        {
                            model.Newsletter1 = reader.GetBoolean(72);
                        }
                        if (reader.IsDBNull(73) == false)
                        {
                            model.Statement1 = reader.GetBoolean(73);
                        }
                        if (reader.IsDBNull(74) == false)
                        {
                            model.Booklet1 = reader.GetBoolean(74);
                        }
                        if (reader.IsDBNull(75) == false)
                        {
                            model.CommentManualType = reader.GetString(75);
                        }
                        if (reader.IsDBNull(76) == false)
                        {
                            model.FinishingFormat = reader.GetString(76);
                        }
                        if (reader.IsDBNull(77) == false)
                        {
                            model.FoldingType = reader.GetString(77);
                        }
                        if (reader.IsDBNull(78) == false)
                        {
                            model.Sealing1 = reader.GetBoolean(78);
                        }
                        if (reader.IsDBNull(79) == false)
                        {
                            model.Tearing1 = reader.GetBoolean(79);
                        }
                        if (reader.IsDBNull(80) == false)
                        {
                            model.BarcodeLabel1 = reader.GetBoolean(80);
                        }
                        if (reader.IsDBNull(81) == false)
                        {
                            model.Cutting1 = reader.GetBoolean(81);
                        }
                        if (reader.IsDBNull(82) == false)
                        {
                            model.StickingOf1 = reader.GetString(82);
                        }
                        if (reader.IsDBNull(83) == false)
                        {
                            model.AddLabel1 = reader.GetBoolean(83);
                        }
                        if (reader.IsDBNull(84) == false)
                        {
                            model.Sticker1 = reader.GetBoolean(84);
                        }
                        if (reader.IsDBNull(85) == false)
                        {
                            model.Chesire1 = reader.GetBoolean(85);
                        }
                        if (reader.IsDBNull(86) == false)
                        {
                            model.Tuck_In1 = reader.GetBoolean(86);
                        }
                        if (reader.IsDBNull(87) == false)
                        {
                            model.Bursting1 = reader.GetBoolean(87);
                        }
                        if (reader.IsDBNull(88) == false)
                        {
                            model.Sealed1 = reader.GetBoolean(88);
                        }
                        if (reader.IsDBNull(89) == false)
                        {
                            model.Folding1 = reader.GetBoolean(89);
                        }
                        if (reader.IsDBNull(90) == false)
                        {
                            model.Unsealed1 = reader.GetBoolean(90);
                        }
                        if (reader.IsDBNull(91) == false)
                        {
                            model.Letter1 = reader.GetBoolean(91);
                        }
                        if (reader.IsDBNull(92) == false)
                        {
                            model.FinishingInst = reader.GetString(92);
                        }
                        if (reader.IsDBNull(93) == false)
                        {
                            model.IT_SysNotes = reader.GetString(93);
                        }
                        if (reader.IsDBNull(94) == false)
                        {
                            model.Produc_PlanningNotes = reader.GetString(94);
                        }
                        if (reader.IsDBNull(95) == false)
                        {
                            model.PurchasingNotes = reader.GetString(95);
                        }
                        if (reader.IsDBNull(96) == false)
                        {
                            model.EngineeringNotes = reader.GetString(96);
                        }
                        if (reader.IsDBNull(97) == false)
                        {
                            model.ArtworkNotes = reader.GetString(97);
                        }
                        if (reader.IsDBNull(98) == false)
                        {
                            model.Acc_BillingNotes = reader.GetString(98);
                        }
                        if (reader.IsDBNull(99) == false)
                        {
                            model.DCPNotes = reader.GetString(99);
                        }
                        if (reader.IsDBNull(100) == false)
                        {
                            model.PostingInfo = reader.GetString(100);
                        }
                        if (reader.IsDBNull(101) == false)
                        {
                            model.NewMR = reader.GetString(101);
                        }
                        if (reader.IsDBNull(102) == false)
                        {
                            model.Confrm100 = reader.GetString(102);
                        }
                    }
                    JobInstructionlist.Add(model);
                }
                ViewBag.Status = StatusCorrection;
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
                command.CommandText = @"SELECT Id,Customer_Name,ProductName,JobSheetNo,SalesExecutiveBy,Status,
                                           ServiceLevel,IsSlaCreaditCard,JobClass,IsSetPaper,JobRequest,
                                           ExpectedDateCompletionToGpo,QuotationRef,ContractName,
                                           Contact_Person,JobType,DeliveryChannel,AccountsQty,ImpressionQty,
                                           PagesQty,CycleTerm,MailingDate,
                                           JoiningFiles,TotalRecord,InputFileName,OutputFileName,Sorting,
                                           SortingMode,Other,DataPrintingRemark,
                                           ArtworkStatus,PaperStock,TypeCode,Paper,PaperSize,
                                           Grammage,MaterialColour,EnvelopeStock,EnvelopeType,EnvelopeSize,
                                           EnvelopeGrammage,EnvelopeColour,EnvelopeWindow,EnvWindowOpaque,
                                           LabelStock,LabelCutsheet,OthersStock,BalancedMaterial,
                                           PlasticStock,PlasticType,PlasticSize,PlasticThickness,
                                           PrintingType,PrintingOrientation,GpoList,RegisterMail,
                                           OtherList,BaseStockType,FinishingSize,AdditionalPrintingMark,
                                           SortingCriteria,PrintingInstr,SortingInstr,Letter,Brochures_Leaflets,
                                           ReplyEnvelope,ImgOnStatement,Booklet,
                                           NumberOfInsert,Magezine1,Brochure1,CarrierSheet1,Newsletter1,
                                           Statement1,Booklet1,CommentManualType,FinishingFormat,
                                           FoldingType,Sealing1,Tearing1,BarcodeLabel1,Cutting1,
                                           StickingOf1,AddLabel1,Sticker1,Chesire1,Tuck_In1,
                                           Bursting1,Sealed1,Folding1,Unsealed1,Letter1,FinishingInst,
                                           IT_SysNotes,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes,
                                           ArtworkNotes, Acc_BillingNotes, DCPNotes, PostingInfo,ProgrammerBy,NewMR,Confrm100
                                       FROM [IflowSeed].[dbo].[JobInstruction]
                                        WHERE( Status='New' OR Status ='Waiting to Assign Programmer') OR Status ='Development Process' OR Status ='Development Complete' OR Status='QM : Need correction from MBD'
                                                 ORDER BY CreatedOn DESC";
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
                            model.JobSheetNo = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            model.SalesExecutiveBy = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            model.Status = reader.GetString(5);
                            StatusCorrection.Add(reader.GetString(5));
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.ServiceLevel = reader.GetString(6);
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.SlaCreaditCard = reader.GetBoolean(7);

                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.JobClass = reader.GetString(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            model.IsSetPaper = reader.GetBoolean(9);

                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            model.JobRequest = reader.GetDateTime(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            model.ExpectedDateCompletionToGpo = reader.GetDateTime(11);
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            model.QuotationRef = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            model.ContractName = reader.GetString(13);
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            model.Contact_Person = reader.GetString(14);
                        }
                        if (reader.IsDBNull(15) == false)
                        {
                            model.JobType = reader.GetString(15);
                        }
                        if (reader.IsDBNull(16) == false)
                        {
                            model.DeliveryChannel = reader.GetString(16);
                        }
                        if (reader.IsDBNull(17) == false)
                        {
                            model.AccountsQty = reader.GetString(17);
                        }
                        if (reader.IsDBNull(18) == false)
                        {
                            model.ImpressionQty = reader.GetString(18);
                        }
                        if (reader.IsDBNull(19) == false)
                        {
                            model.PagesQty = reader.GetString(19);
                        }
                        if (reader.IsDBNull(20) == false)
                        {
                            model.CycleTerm = reader.GetDateTime(20);
                        }
                        if (reader.IsDBNull(21) == false)
                        {
                            model.MailingDate = (DateTime)reader.GetDateTime(21);
                        }

                        if (reader.IsDBNull(22) == false)
                        {
                            model.JoiningFiles = reader.GetString(22);
                        }
                        if (reader.IsDBNull(23) == false)
                        {
                            model.TotalRecord = reader.GetString(23);
                        }
                        if (reader.IsDBNull(24) == false)
                        {
                            model.InputFileName = reader.GetString(24);
                        }
                        if (reader.IsDBNull(25) == false)
                        {
                            model.OutputFileName = reader.GetString(25);
                        }
                        if (reader.IsDBNull(26) == false)
                        {
                            model.Sorting = reader.GetString(26);
                        }
                        if (reader.IsDBNull(27) == false)
                        {
                            model.SortingMode = reader.GetString(27);
                        }
                        if (reader.IsDBNull(28) == false)
                        {
                            model.Other = reader.GetString(28);
                        }
                        if (reader.IsDBNull(29) == false)
                        {
                            model.DataPrintingRemark = reader.GetString(29);
                        }
                        if (reader.IsDBNull(30) == false)
                        {
                            model.ArtworkStatus = reader.GetString(30);
                        }
                        if (reader.IsDBNull(31) == false)
                        {
                            model.PaperStock = reader.GetString(31);
                        }
                        if (reader.IsDBNull(32) == false)
                        {
                            model.TypeCode = reader.GetString(32);
                        }
                        if (reader.IsDBNull(33) == false)
                        {
                            model.Paper = reader.GetString(33);
                        }
                        if (reader.IsDBNull(34) == false)
                        {
                            model.PaperSize = reader.GetString(34);
                        }
                        if (reader.IsDBNull(35) == false)
                        {
                            model.Grammage = reader.GetString(35);
                        }
                        if (reader.IsDBNull(36) == false)
                        {
                            model.MaterialColour = reader.GetString(36);
                        }
                        if (reader.IsDBNull(37) == false)
                        {
                            model.EnvelopeStock = reader.GetString(37);
                        }
                        if (reader.IsDBNull(38) == false)
                        {
                            model.EnvelopeType = reader.GetString(38);
                        }
                        if (reader.IsDBNull(39) == false)
                        {
                            model.EnvelopeSize = reader.GetString(39);
                        }
                        if (reader.IsDBNull(40) == false)
                        {
                            model.EnvelopeGrammage = reader.GetString(40);
                        }
                        if (reader.IsDBNull(41) == false)
                        {
                            model.EnvelopeColour = reader.GetString(41);
                        }
                        if (reader.IsDBNull(42) == false)
                        {
                            model.EnvelopeWindow = reader.GetString(42);
                        }
                        if (reader.IsDBNull(43) == false)
                        {
                            model.EnvWindowOpaque = reader.GetString(43);
                        }
                        if (reader.IsDBNull(44) == false)
                        {
                            model.LabelStock = reader.GetString(44);
                        }
                        if (reader.IsDBNull(45) == false)
                        {
                            model.LabelCutsheet = reader.GetString(45);
                        }
                        if (reader.IsDBNull(46) == false)
                        {
                            model.OthersStock = reader.GetString(46);
                        }
                        if (reader.IsDBNull(47) == false)
                        {
                            model.BalancedMaterial = reader.GetString(47);
                        }
                        if (reader.IsDBNull(48) == false)
                        {
                            model.PlasticStock = reader.GetString(48);
                        }
                        if (reader.IsDBNull(49) == false)
                        {
                            model.PlasticType = reader.GetString(49);
                        }
                        if (reader.IsDBNull(50) == false)
                        {
                            model.PlasticSize = reader.GetString(50);
                        }
                        if (reader.IsDBNull(51) == false)
                        {
                            model.PlasticThickness = reader.GetString(51);
                        }
                        if (reader.IsDBNull(52) == false)
                        {
                            model.PrintingType = reader.GetString(52);
                        }
                        if (reader.IsDBNull(53) == false)
                        {
                            model.PrintingOrientation = reader.GetString(53);
                        }
                        if (reader.IsDBNull(54) == false)
                        {
                            model.GpoList = reader.GetBoolean(54);
                        }
                        if (reader.IsDBNull(55) == false)
                        {
                            model.RegisterMail = reader.GetBoolean(55);
                        }
                        if (reader.IsDBNull(56) == false)
                        {
                            model.OtherList = reader.GetString(56);
                        }
                        if (reader.IsDBNull(57) == false)
                        {
                            model.BaseStockType = reader.GetString(57);
                        }
                        if (reader.IsDBNull(58) == false)
                        {
                            model.FinishingSize = reader.GetString(58);
                        }
                        if (reader.IsDBNull(59) == false)
                        {
                            model.AdditionalPrintingMark = reader.GetString(59);
                        }
                        if (reader.IsDBNull(60) == false)
                        {
                            model.SortingCriteria = reader.GetString(60);
                        }
                        if (reader.IsDBNull(61) == false)
                        {
                            model.PrintingInstr = reader.GetString(61);
                        }
                        if (reader.IsDBNull(62) == false)
                        {
                            model.SortingInstr = reader.GetString(62);
                        }
                        if (reader.IsDBNull(63) == false)
                        {
                            model.Letter = reader.GetBoolean(63);
                        }
                        if (reader.IsDBNull(64) == false)
                        {
                            model.Brochures_Leaflets = reader.GetBoolean(64);
                        }
                        if (reader.IsDBNull(65) == false)
                        {
                            model.ReplyEnvelope = reader.GetBoolean(65);
                        }
                        if (reader.IsDBNull(66) == false)
                        {
                            model.ImgOnStatement = reader.GetBoolean(66);
                        }
                        if (reader.IsDBNull(67) == false)
                        {
                            model.Booklet = reader.GetBoolean(67);
                        }
                        if (reader.IsDBNull(68) == false)
                        {
                            model.NumberOfInsert = reader.GetString(68);
                        }
                        if (reader.IsDBNull(69) == false)
                        {
                            model.Magezine1 = reader.GetBoolean(69);
                        }
                        if (reader.IsDBNull(70) == false)
                        {
                            model.Brochure1 = reader.GetBoolean(70);
                        }
                        if (reader.IsDBNull(71) == false)
                        {
                            model.CarrierSheet1 = reader.GetBoolean(71);
                        }
                        if (reader.IsDBNull(72) == false)
                        {
                            model.Newsletter1 = reader.GetBoolean(72);
                        }
                        if (reader.IsDBNull(73) == false)
                        {
                            model.Statement1 = reader.GetBoolean(73);
                        }
                        if (reader.IsDBNull(74) == false)
                        {
                            model.Booklet1 = reader.GetBoolean(74);
                        }
                        if (reader.IsDBNull(75) == false)
                        {
                            model.CommentManualType = reader.GetString(75);
                        }
                        if (reader.IsDBNull(76) == false)
                        {
                            model.FinishingFormat = reader.GetString(76);
                        }
                        if (reader.IsDBNull(77) == false)
                        {
                            model.FoldingType = reader.GetString(77);
                        }
                        if (reader.IsDBNull(78) == false)
                        {
                            model.Sealing1 = reader.GetBoolean(78);
                        }
                        if (reader.IsDBNull(79) == false)
                        {
                            model.Tearing1 = reader.GetBoolean(79);
                        }
                        if (reader.IsDBNull(80) == false)
                        {
                            model.BarcodeLabel1 = reader.GetBoolean(80);
                        }
                        if (reader.IsDBNull(81) == false)
                        {
                            model.Cutting1 = reader.GetBoolean(81);
                        }
                        if (reader.IsDBNull(82) == false)
                        {
                            model.StickingOf1 = reader.GetString(82);
                        }
                        if (reader.IsDBNull(83) == false)
                        {
                            model.AddLabel1 = reader.GetBoolean(83);
                        }
                        if (reader.IsDBNull(84) == false)
                        {
                            model.Sticker1 = reader.GetBoolean(84);
                        }
                        if (reader.IsDBNull(85) == false)
                        {
                            model.Chesire1 = reader.GetBoolean(85);
                        }
                        if (reader.IsDBNull(86) == false)
                        {
                            model.Tuck_In1 = reader.GetBoolean(86);
                        }
                        if (reader.IsDBNull(87) == false)
                        {
                            model.Bursting1 = reader.GetBoolean(87);
                        }
                        if (reader.IsDBNull(88) == false)
                        {
                            model.Sealed1 = reader.GetBoolean(88);
                        }
                        if (reader.IsDBNull(89) == false)
                        {
                            model.Folding1 = reader.GetBoolean(89);
                        }
                        if (reader.IsDBNull(90) == false)
                        {
                            model.Unsealed1 = reader.GetBoolean(90);
                        }
                        if (reader.IsDBNull(91) == false)
                        {
                            model.Letter1 = reader.GetBoolean(91);
                        }
                        if (reader.IsDBNull(92) == false)
                        {
                            model.FinishingInst = reader.GetString(92);
                        }
                        if (reader.IsDBNull(93) == false)
                        {
                            model.IT_SysNotes = reader.GetString(93);
                        }
                        if (reader.IsDBNull(94) == false)
                        {
                            model.Produc_PlanningNotes = reader.GetString(94);
                        }
                        if (reader.IsDBNull(95) == false)
                        {
                            model.PurchasingNotes = reader.GetString(95);
                        }
                        if (reader.IsDBNull(96) == false)
                        {
                            model.EngineeringNotes = reader.GetString(96);
                        }
                        if (reader.IsDBNull(97) == false)
                        {
                            model.ArtworkNotes = reader.GetString(97);
                        }
                        if (reader.IsDBNull(98) == false)
                        {
                            model.Acc_BillingNotes = reader.GetString(98);
                        }
                        if (reader.IsDBNull(99) == false)
                        {
                            model.DCPNotes = reader.GetString(99);
                        }
                        if (reader.IsDBNull(100) == false)
                        {
                            model.PostingInfo = reader.GetString(100);
                        }
                        if (reader["NewMR"]!=null)
                        {
                            model.NewMR = reader["NewMR"].ToString();
                        }
                        if (reader["Confrm100"] != null)
                        {
                            model.Confrm100 = reader["Confrm100"].ToString();
                        }
                    }
                    JobInstructionlist.Add(model);
                    
                }
                ViewBag.Status = StatusCorrection;
                cn.Close();
            }

        }

        if (set == "SubmitLIVE")
        {
            if (Status == "Development Complete")
            {
                using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                    cn3.Open();
                    SqlCommand command1;
                    command1 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET ModifiedOn=@ModifiedOn, STATUS='ITO' WHERE Id=@Id", cn3);
                    command1.Parameters.AddWithValue("@ModifiedOn", createdOn);
                    command1.Parameters.AddWithValue("@Id", Id);
                    command1.ExecuteNonQuery();
                    TempData["msg"] = "<script>alert('JI  SUCCESSFULLY LIVE !');</script>";

                    return RedirectToAction("ManageJobInstruction", "MBD");
                }


            }
            else if (JobType == "MMP")
            {
                if(NewMR=="YES")
                {
                    using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                        cn3.Open();
                        SqlCommand command1;
                        command1 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET ModifiedOn=@ModifiedOn, STATUS='MMP' WHERE Id=@Id", cn3);
                        command1.Parameters.AddWithValue("@ModifiedOn", createdOn);
                        command1.Parameters.AddWithValue("@Id", Id);
                        command1.ExecuteNonQuery();
                        TempData["msg"] = "<script>alert('JI  SUCCESSFULLY LIVE !');</script>";

                        return RedirectToAction("ManageJobInstruction", "MBD");
                    }
                }
                else
                {
                    using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                        cn3.Open();
                        SqlCommand command1;
                        command1 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET ModifiedOn=@ModifiedOn, STATUS='MMP' WHERE Id=@Id", cn3);
                        command1.Parameters.AddWithValue("@ModifiedOn", createdOn);
                        command1.Parameters.AddWithValue("@Id", Id);
                        command1.ExecuteNonQuery();
                        TempData["msg"] = "<script>alert('JI  SUCCESSFULLY LIVE !');</script>";

                        return RedirectToAction("ManageJobInstruction", "MBD");
                    }

                }
            }

            else
            {
                TempData["msg"] = "<script>alert('THIS ACTION CANNOT BE PROCESS !');</script>";
            }

            return RedirectToAction("ManageJobInstruction", "MBD");

        }

        if (set == "LIVE")
        {

            List<JIHistory> JIHistory = new List<JIHistory>();


            using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                cn3.Open();
                SqlCommand command1;
                //command1 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET ModifiedOn=@ModifiedOn, STATUS='Waiting To Assign Programmer' WHERE Id=@Id", cn3);
                command1 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET ModifiedOn=@ModifiedOn, STATUS='ITO' WHERE Id=@Id", cn3);
                command1.Parameters.AddWithValue("@ModifiedOn", createdOn);
                command1.Parameters.AddWithValue("@Id", Id);
                command1.ExecuteNonQuery();
                TempData["msg"] = "<script>alert('JI  SUCCESSFULLY LIVE !');</script>";



                //SqlCommand sqlgetinfo = new SqlCommand("SELECT Customer_Name,ProductName,JobSheetNo,Status, ServiceLevel,JobClass,IsSetPaper,JobRequest, ExpectedDateCompletionToGpo,QuotationRef, " +
                //    "JobType,DeliveryChannel,AccountsQty,ImpressionQty, PagesQty,CycleTerm,MailingDate, JoiningFiles,TotalRecord,InputFileName,OutputFileName,Sorting, SortingMode,Other,DataPrintingRemark, ArtworkStatus," +
                //    "PaperStock, Grammage,MaterialColour,EnvelopeStock,EnvelopeType,EnvelopeSize, EnvelopeGrammage,EnvelopeColour,EnvelopeWindow,EnvWindowOpaque, LabelStock,LabelCutsheet,OthersStock,BalancedMaterial, " +
                //    "PlasticStock,PlasticType,PlasticSize,PlasticThickness, PrintingType,PrintingOrientation,GpoList,RegisterMail, OtherList,BaseStockType,FinishingSize,AdditionalPrintingMark, SortingCriteria,PrintingInstr," +
                //    "SortingInstr,Letter,Brochures_Leaflets, ReplyEnvelope,ImgOnStatement,Booklet, NumberOfInsert,FinishingInst, IT_SysNotes,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes, ArtworkNotes, Acc_BillingNotes, " +
                //    "DCPNotes, PostingInfo,Cust_Department FROM [IflowSeed].[dbo].[JobInstruction] WHERE Id=@Id", cn3);
                //sqlgetinfo.Parameters.AddWithValue("@Id", Id);
                //SqlDataReader rm = sqlgetinfo.ExecuteReader();

                //var x = new JIHistory() { };

                //while (rm.Read())
                //{
                //    //Id = 
                //    if (rm.IsDBNull(0) == false)
                //    {
                //        x.Customer_Name = rm.GetString(0);
                //    }

                //    if (rm.IsDBNull(1) == false)
                //    {
                //        x.ProductName = rm.GetString(1);
                //    }

                //    if (rm.IsDBNull(2) == false)
                //    {
                //        x.JobSheetNo = rm.GetString(2);
                //    }

                //    if (rm.IsDBNull(3) == false)
                //    {
                //        x.Status = rm.GetString(3);
                //    }

                //    if (rm.IsDBNull(4) == false)
                //    {
                //        x.ServiceLevel = rm.GetString(4);
                //    }

                //    if (rm.IsDBNull(5) == false)
                //    {
                //        x.JobClass = rm.GetString(5);
                //    }

                //    if (rm.IsDBNull(6) == false)
                //    {
                //        x.IsSetPaper = rm.GetBoolean(6);
                //    }

                //    if (rm.IsDBNull(7) == false)
                //    {
                //        x.JobRequest = rm.GetDateTime(7);
                //    }

                //    if (rm.IsDBNull(8) == false)
                //    {
                //        x.ExpectedDateCompletionToGpo = rm.GetDateTime(8);
                //    }

                //    if (rm.IsDBNull(9) == false)
                //    {
                //        x.QuotationRef = rm.GetString(9);
                //    }

                //    if (rm.IsDBNull(10) == false)
                //    {
                //        x.JobType = rm.GetString(10);
                //    }

                //    if (rm.IsDBNull(11) == false)
                //    {
                //        x.DeliveryChannel = rm.GetString(11);
                //    }

                //    if (rm.IsDBNull(12) == false)
                //    {
                //        x.AccountsQty = rm.GetString(12);
                //    }

                //    if (rm.IsDBNull(13) == false)
                //    {
                //        x.ImpressionQty = rm.GetString(13);
                //    }

                //    if (rm.IsDBNull(14) == false)
                //    {
                //        x.PagesQty = rm.GetString(14);
                //    }

                //    if (rm.IsDBNull(15) == false)
                //    {
                //        x.CycleTerm = rm.GetDateTime(15);
                //    }

                //    if (rm.IsDBNull(16) == false)
                //    {
                //        x.MailingDate = rm.GetDateTime(16);
                //    }

                //    if (rm.IsDBNull(17) == false)
                //    {
                //        x.JoiningFiles = rm.GetString(17);
                //    }

                //    if (rm.IsDBNull(18) == false)
                //    {
                //        x.TotalRecord = rm.GetString(18);
                //    }

                //    if (rm.IsDBNull(19) == false)
                //    {
                //        x.InputFileName = rm.GetString(19);
                //    }

                //    if (rm.IsDBNull(20) == false)
                //    {
                //        x.OutputFileName = rm.GetString(20);
                //    }

                //    if (rm.IsDBNull(21) == false)
                //    {
                //        x.Sorting = rm.GetString(21);
                //    }

                //    if (rm.IsDBNull(22) == false)
                //    {
                //        x.SortingMode = rm.GetString(22);
                //    }

                //    if (rm.IsDBNull(23) == false)
                //    {
                //        x.Other = rm.GetString(23);
                //    }

                //    if (rm.IsDBNull(24) == false)
                //    {
                //        x.DataPrintingRemark = rm.GetString(24);
                //    }

                //    if (rm.IsDBNull(25) == false)
                //    {
                //        x.ArtworkStatus = rm.GetString(25);
                //    }

                //    if (rm.IsDBNull(26) == false)
                //    {
                //        x.PaperStock = rm.GetString(26);
                //    }

                //    if (rm.IsDBNull(27) == false)
                //    {
                //        x.Grammage = rm.GetString(27);
                //    }

                //    if (rm.IsDBNull(28) == false)
                //    {
                //        x.MaterialColour = rm.GetString(28);
                //    }

                //    if (rm.IsDBNull(29) == false)
                //    {
                //        x.EnvelopeStock = rm.GetString(29);
                //    }

                //    if (rm.IsDBNull(30) == false)
                //    {
                //        x.EnvelopeType = rm.GetString(30);
                //    }

                //    if (rm.IsDBNull(31) == false)
                //    {
                //        x.EnvelopeSize = rm.GetString(31);
                //    }

                //    if (rm.IsDBNull(32) == false)
                //    {
                //        x.EnvelopeGrammage = rm.GetString(32);
                //    }

                //    if (rm.IsDBNull(33) == false)
                //    {
                //        x.EnvelopeColour = rm.GetString(33);
                //    }

                //    if (rm.IsDBNull(34) == false)
                //    {
                //        x.EnvelopeWindow = rm.GetString(34);
                //    }

                //    if (rm.IsDBNull(35) == false)
                //    {
                //        x.EnvWindowOpaque = rm.GetString(35);
                //    }

                //    if (rm.IsDBNull(36) == false)
                //    {
                //        x.LabelStock = rm.GetString(36);
                //    }

                //    if (rm.IsDBNull(37) == false)
                //    {
                //        x.LabelCutsheet = rm.GetString(37);
                //    }

                //    if (rm.IsDBNull(38) == false)
                //    {
                //        x.OthersStock = rm.GetString(38);
                //    }

                //    if (rm.IsDBNull(39) == false)
                //    {
                //        x.BalancedMaterial = rm.GetString(39);
                //    }

                //    if (rm.IsDBNull(40) == false)
                //    {
                //        x.PlasticStock = rm.GetString(40);
                //    }

                //    if (rm.IsDBNull(41) == false)
                //    {
                //        x.PlasticType = rm.GetString(41);
                //    }

                //    if (rm.IsDBNull(42) == false)
                //    {
                //        x.PlasticSize = rm.GetString(42);
                //    }

                //    if (rm.IsDBNull(43) == false)
                //    {
                //        x.PlasticThickness = rm.GetString(43);
                //    }

                //    if (rm.IsDBNull(44) == false)
                //    {
                //        x.PrintingType = rm.GetString(44);
                //    }

                //    if (rm.IsDBNull(45) == false)
                //    {
                //        x.PrintingOrientation = rm.GetString(45);
                //    }

                //    if (rm.IsDBNull(46) == false)
                //    {
                //        x.GpoList = rm.GetBoolean(46);
                //    }

                //    if (rm.IsDBNull(47) == false)
                //    {
                //        x.RegisterMail = rm.GetBoolean(47);
                //    }

                //    if (rm.IsDBNull(48) == false)
                //    {
                //        x.OtherList = rm.GetString(48);
                //    }

                //    if (rm.IsDBNull(49) == false)
                //    {
                //        x.BaseStockType = rm.GetString(49);
                //    }

                //    if (rm.IsDBNull(50) == false)
                //    {
                //        x.FinishingSize = rm.GetString(50);
                //    }

                //    if (rm.IsDBNull(51) == false)
                //    {
                //        x.AdditionalPrintingMark = rm.GetString(51);
                //    }

                //    if (rm.IsDBNull(52) == false)
                //    {
                //        x.SortingCriteria = rm.GetString(52);
                //    }

                //    if (rm.IsDBNull(53) == false)
                //    {
                //        x.PrintingInstr = rm.GetString(53);
                //    }

                //    if (rm.IsDBNull(54) == false)
                //    {
                //        x.SortingInstr = rm.GetString(54);
                //    }

                //    if (rm.IsDBNull(55) == false)
                //    {
                //        x.Letter = rm.GetBoolean(55);
                //    }

                //    if (rm.IsDBNull(56) == false)
                //    {
                //        x.Brochures_Leaflets = rm.GetBoolean(56);
                //    }

                //    if (rm.IsDBNull(57) == false)
                //    {
                //        x.ReplyEnvelope = rm.GetBoolean(57);
                //    }

                //    if (rm.IsDBNull(58) == false)
                //    {
                //        x.ImgOnStatement = rm.GetBoolean(58);
                //    }

                //    if (rm.IsDBNull(59) == false)
                //    {
                //        x.Booklet = rm.GetBoolean(59);
                //    }

                //    if (rm.IsDBNull(60) == false)
                //    {
                //        x.NumberOfInsert = rm.GetString(60);
                //    }

                //    if (rm.IsDBNull(61) == false)
                //    {
                //        x.FinishingInst = rm.GetString(61);
                //    }

                //    if (rm.IsDBNull(62) == false)
                //    {
                //        x.IT_SysNotes = rm.GetString(62);
                //    }

                //    if (rm.IsDBNull(63) == false)
                //    {
                //        x.Produc_PlanningNotes = rm.GetString(63);
                //    }

                //    if (rm.IsDBNull(64) == false)
                //    {
                //        x.PurchasingNotes = rm.GetString(64);
                //    }

                //    if (rm.IsDBNull(65) == false)
                //    {
                //        x.EngineeringNotes = rm.GetString(65);
                //    }

                //    if (rm.IsDBNull(66) == false)
                //    {
                //        x.ArtworkNotes = rm.GetString(66);
                //    }

                //    if (rm.IsDBNull(67) == false)
                //    {
                //        x.Acc_BillingNotes = rm.GetString(67);
                //    }

                //    if (rm.IsDBNull(68) == false)
                //    {
                //        x.DCPNotes = rm.GetString(68);
                //    }

                //    if (rm.IsDBNull(69) == false)
                //    {
                //        x.PostingInfo = rm.GetString(69);
                //    }

                //    if (rm.IsDBNull(70) == false)
                //    {
                //        x.Cust_Department = rm.GetString(70);
                //    }

                //    //if (rm.IsDBNull(71) == false)
                //    //{
                //    //    x.ActiveSts = rm.GetString(71);
                //    //}

                //    //JIHistory.Add(x);


                //    //foreach (var a in JIHistory)
                //    //{
                //    //    Debug.WriteLine("Data :" + a);
                //    //}
                //    //IsSlaCreaditCard = rm.get(5);

                //}
                //rm.Close();



                ////foreach(var a in JIHistory)
                ////{
                ////    Debug.WriteLine("DATA :"+a.);
                ////}

                //Guid Idx = Guid.NewGuid();

                //SqlCommand command2 = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[JobBatchInfo] (Id,Customer_Name,ProductName,JobSheetNo,Status,ServiceLevel,JobClass,IsSetPaper,JobRequest,ExpectedDateCompletionToGpo,QuotationRef,JobType,DeliveryChannel,AccountsQty,ImpressionQty,PagesQty,CycleTerm,MailingDate,JoiningFiles,TotalRecord,InputFileName,OutputFileName,Sorting," +
                //    "SortingMode,Other,DataPrintingRemark,ArtworkStatus,PaperStock,Grammage,MaterialColour,EnvelopeStock,EnvelopeType,EnvelopeSize,EnvelopeGrammage,EnvelopeColour,EnvelopeWindow,EnvWindowOpaque,LabelStock,LabelCutsheet,OthersStock," +
                //    "BalancedMaterial,PlasticStock,PlasticType,PlasticSize,PlasticThickness,PrintingType,PrintingOrientation,GpoList,RegisterMail,OtherList,BaseStockType,FinishingSize,AdditionalPrintingMark,SortingCriteria,PrintingInstr,SortingInstr,Letter,Brochures_Leaflets," +
                //    "ReplyEnvelope,ImgOnStatement,Booklet,NumberOfInsert,FinishingInst,IT_SysNotes,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes,ArtworkNotes, Acc_BillingNotes, DCPNotes, PostingInfo,Cust_Department,ActiveSts)" +
                //    "VALUES (@Id2,@Customer_Name,@ProductName,@JobSheetNo,@Status, @ServiceLevel,@JobClass,@IsSetPaper,@JobRequest, @ExpectedDateCompletionToGpo,@QuotationRef, @JobType,@DeliveryChannel,@AccountsQty,@ImpressionQty, " +
                //    "@PagesQty,@CycleTerm,@MailingDate, @JoiningFiles,@TotalRecord,@InputFileName,@OutputFileName,@Sorting, @SortingMode,@Other,@DataPrintingRemark, @ArtworkStatus,@PaperStock, @Grammage,@MaterialColour,@EnvelopeStock,@EnvelopeType,@EnvelopeSize, " +
                //    "@EnvelopeGrammage,@EnvelopeColour,@EnvelopeWindow,@EnvWindowOpaque, @LabelStock,@LabelCutsheet,@OthersStock,@BalancedMaterial, @PlasticStock,@PlasticType,@PlasticSize,@PlasticThickness, @PrintingType,@PrintingOrientation,@GpoList," +
                //    "@RegisterMail, @OtherList,@BaseStockType,@FinishingSize,@AdditionalPrintingMark, @SortingCriteria,@PrintingInstr,@SortingInstr,@Letter,@Brochures_Leaflets, @ReplyEnvelope,@ImgOnStatement,@Booklet, @NumberOfInsert,@FinishingInst, " +
                //    "@IT_SysNotes,@Produc_PlanningNotes,@PurchasingNotes,@EngineeringNotes, @ArtworkNotes, @Acc_BillingNotes, @DCPNotes, @PostingInfo,@Cust_Department,@ActiveSts)", cn3);



                //command2.Parameters.AddWithValue("@Id2", Idx);

                //if (x.Customer_Name != null)
                //{
                //    command2.Parameters.AddWithValue("@Customer_Name", x.Customer_Name);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@Customer_Name", DBNull.Value);

                //}

                //if (x.ProductName != null)
                //{
                //    command2.Parameters.AddWithValue("@ProductName", x.ProductName);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@ProductName", DBNull.Value);

                //}

                //if (x.ProductName != null)
                //{
                //    command2.Parameters.AddWithValue("@JobSheetNo", x.JobSheetNo);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@JobSheetNo", DBNull.Value);

                //}

                //if (x.Status != null)
                //{
                //    command2.Parameters.AddWithValue("@Status", x.Status);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@Status", DBNull.Value);

                //}

                //if (x.ServiceLevel != null)
                //{
                //    command2.Parameters.AddWithValue("@ServiceLevel", x.ServiceLevel);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@ServiceLevel", DBNull.Value);

                //}

                ////if (x.IsSlaCreaditCard != null)
                ////{
                ////    command2.Parameters.AddWithValue("@IsSlaCreaditCard", x.IsSlaCreaditCard);

                ////}
                ////else
                ////{

                ////    command2.Parameters.AddWithValue("@IsSlaCreaditCard", DBNull.Value);

                ////}

                //if (x.JobClass != null)
                //{
                //    command2.Parameters.AddWithValue("@JobClass", x.JobClass);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@JobClass", DBNull.Value);

                //}

                //if (x.IsSetPaper != null)
                //{
                //    command2.Parameters.AddWithValue("@IsSetPaper", x.IsSetPaper);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@IsSetPaper", DBNull.Value);

                //}

                //if (x.JobRequest != null)
                //{
                //    command2.Parameters.AddWithValue("@JobRequest", x.JobRequest);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@JobRequest", DBNull.Value);

                //}

                //if (x.ExpectedDateCompletionToGpo != null)
                //{
                //    command2.Parameters.AddWithValue("@ExpectedDateCompletionToGpo", x.ExpectedDateCompletionToGpo);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@ExpectedDateCompletionToGpo", DBNull.Value);

                //}

                //if (x.QuotationRef != null)
                //{
                //    command2.Parameters.AddWithValue("@QuotationRef", x.QuotationRef);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@QuotationRef", DBNull.Value);

                //}

                //if (x.JobType != null)
                //{
                //    command2.Parameters.AddWithValue("@JobType", x.JobType);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@JobType", DBNull.Value);

                //}

                //if (x.DeliveryChannel != null)
                //{
                //    command2.Parameters.AddWithValue("@DeliveryChannel", x.DeliveryChannel);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@DeliveryChannel", DBNull.Value);

                //}

                //if (x.AccountsQty != null)
                //{
                //    command2.Parameters.AddWithValue("@AccountsQty", x.AccountsQty);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@AccountsQty", DBNull.Value);

                //}

                //if (x.ImpressionQty != null)
                //{
                //    command2.Parameters.AddWithValue("@ImpressionQty", x.ImpressionQty);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@ImpressionQty", DBNull.Value);

                //}

                //if (x.PagesQty != null)
                //{
                //    command2.Parameters.AddWithValue("@PagesQty", x.PagesQty);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@PagesQty", DBNull.Value);

                //}
                //if (x.CycleTerm != null)
                //{
                //    command2.Parameters.AddWithValue("@CycleTerm", x.CycleTerm);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@CycleTerm", DBNull.Value);

                //}

                //if (x.MailingDate != null)
                //{
                //    command2.Parameters.AddWithValue("@MailingDate", x.MailingDate);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@MailingDate", DBNull.Value);

                //}

                //if (x.JoiningFiles != null)
                //{
                //    command2.Parameters.AddWithValue("@JoiningFiles", x.JoiningFiles);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@JoiningFiles", DBNull.Value);

                //}

                //if (x.TotalRecord != null)
                //{
                //    command2.Parameters.AddWithValue("@TotalRecord", x.TotalRecord);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@TotalRecord", DBNull.Value);

                //}

                //if (x.InputFileName != null)
                //{
                //    command2.Parameters.AddWithValue("@InputFileName", x.InputFileName);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@InputFileName", DBNull.Value);

                //}

                //if (x.OutputFileName != null)
                //{
                //    command2.Parameters.AddWithValue("@OutputFileName", x.OutputFileName);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@OutputFileName", DBNull.Value);

                //}

                //if (x.Sorting != null)
                //{
                //    command2.Parameters.AddWithValue("@Sorting", x.Sorting);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@Sorting", DBNull.Value);

                //}

                //if (x.SortingMode != null)
                //{
                //    command2.Parameters.AddWithValue("@SortingMode", x.SortingMode);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@SortingMode", DBNull.Value);

                //}

                //if (x.Other != null)
                //{
                //    command2.Parameters.AddWithValue("@Other", x.Other);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@Other", DBNull.Value);

                //}

                //if (x.DataPrintingRemark != null)
                //{
                //    command2.Parameters.AddWithValue("@DataPrintingRemark", x.DataPrintingRemark);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@DataPrintingRemark", DBNull.Value);

                //}

                //if (x.ArtworkStatus != null)
                //{
                //    command2.Parameters.AddWithValue("@ArtworkStatus", x.ArtworkStatus);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@ArtworkStatus", DBNull.Value);

                //}

                //if (x.PaperStock != null)
                //{
                //    command2.Parameters.AddWithValue("@PaperStock", x.PaperStock);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@PaperStock", DBNull.Value);

                //}

                //if (x.Grammage != null)
                //{
                //    command2.Parameters.AddWithValue("@Grammage", x.Grammage);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@Grammage", DBNull.Value);

                //}

                //if (x.MaterialColour != null)
                //{
                //    command2.Parameters.AddWithValue("@MaterialColour", x.MaterialColour);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@MaterialColour", DBNull.Value);

                //}

                //if (x.EnvelopeStock != null)
                //{
                //    command2.Parameters.AddWithValue("@EnvelopeStock", x.EnvelopeStock);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@EnvelopeStock", DBNull.Value);

                //}

                //if (x.EnvelopeType != null)
                //{
                //    command2.Parameters.AddWithValue("@EnvelopeType", x.EnvelopeType);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@EnvelopeType", DBNull.Value);

                //}

                //if (x.EnvelopeSize != null)
                //{
                //    command2.Parameters.AddWithValue("@EnvelopeSize", x.EnvelopeSize);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@EnvelopeSize", DBNull.Value);

                //}

                //if (x.EnvelopeGrammage != null)
                //{
                //    command2.Parameters.AddWithValue("@EnvelopeGrammage", x.EnvelopeGrammage);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@EnvelopeGrammage", DBNull.Value);

                //}

                //if (x.EnvelopeColour != null)
                //{
                //    command2.Parameters.AddWithValue("@EnvelopeColour", x.EnvelopeColour);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@EnvelopeColour", DBNull.Value);

                //}

                //if (x.EnvelopeWindow != null)
                //{
                //    command2.Parameters.AddWithValue("@EnvelopeWindow", x.EnvelopeWindow);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@EnvelopeWindow", DBNull.Value);

                //}

                //if (x.EnvWindowOpaque != null)
                //{
                //    command2.Parameters.AddWithValue("@EnvWindowOpaque", x.EnvWindowOpaque);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@EnvWindowOpaque", DBNull.Value);

                //}

                //if (x.LabelStock != null)
                //{
                //    command2.Parameters.AddWithValue("@LabelStock", x.LabelStock);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@LabelStock", DBNull.Value);

                //}

                //if (x.LabelCutsheet != null)
                //{
                //    command2.Parameters.AddWithValue("@LabelCutsheet", x.LabelCutsheet);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@LabelCutsheet", DBNull.Value);

                //}

                //if (x.OthersStock != null)
                //{
                //    command2.Parameters.AddWithValue("@OthersStock", x.OthersStock);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@OthersStock", DBNull.Value);

                //}

                //if (x.BalancedMaterial != null)
                //{
                //    command2.Parameters.AddWithValue("@BalancedMaterial", x.BalancedMaterial);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@BalancedMaterial", DBNull.Value);

                //}

                //if (x.PlasticStock != null)
                //{
                //    command2.Parameters.AddWithValue("@PlasticStock", x.PlasticStock);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@PlasticStock", DBNull.Value);

                //}

                //if (x.PlasticType != null)
                //{
                //    command2.Parameters.AddWithValue("@PlasticType", x.PlasticType);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@PlasticType", DBNull.Value);

                //}

                //if (x.PlasticSize != null)
                //{
                //    command2.Parameters.AddWithValue("@PlasticSize", x.PlasticSize);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@PlasticSize", DBNull.Value);

                //}

                //if (x.PlasticThickness != null)
                //{
                //    command2.Parameters.AddWithValue("@PlasticThickness", x.PlasticThickness);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@PlasticThickness", DBNull.Value);

                //}

                //if (x.PrintingType != null)
                //{
                //    command2.Parameters.AddWithValue("@PrintingType", x.PrintingType);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@PrintingType", DBNull.Value);

                //}

                //if (x.PrintingOrientation != null)
                //{
                //    command2.Parameters.AddWithValue("@PrintingOrientation", x.PrintingOrientation);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@PrintingOrientation", DBNull.Value);

                //}

                //if (x.GpoList != null)
                //{
                //    command2.Parameters.AddWithValue("@GpoList", x.GpoList);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@GpoList", DBNull.Value);

                //}

                //if (x.RegisterMail != null)
                //{
                //    command2.Parameters.AddWithValue("@RegisterMail", x.RegisterMail);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@RegisterMail", DBNull.Value);

                //}

                //if (x.OtherList != null)
                //{
                //    command2.Parameters.AddWithValue("@OtherList", x.OtherList);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@OtherList", DBNull.Value);

                //}

                //if (x.BaseStockType != null)
                //{
                //    command2.Parameters.AddWithValue("@BaseStockType", x.BaseStockType);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@BaseStockType", DBNull.Value);

                //}

                //if (x.FinishingSize != null)
                //{
                //    command2.Parameters.AddWithValue("@FinishingSize", x.FinishingSize);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@FinishingSize", DBNull.Value);

                //}

                //if (x.AdditionalPrintingMark != null)
                //{
                //    command2.Parameters.AddWithValue("@AdditionalPrintingMark", x.AdditionalPrintingMark);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@AdditionalPrintingMark", DBNull.Value);

                //}

                //if (x.SortingCriteria != null)
                //{
                //    command2.Parameters.AddWithValue("@SortingCriteria", x.SortingCriteria);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@SortingCriteria", DBNull.Value);

                //}

                //if (x.PrintingInstr != null)
                //{
                //    command2.Parameters.AddWithValue("@PrintingInstr", x.FinishingSize);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@PrintingInstr", DBNull.Value);

                //}
                //if (x.SortingInstr != null)
                //{
                //    command2.Parameters.AddWithValue("@SortingInstr", x.SortingInstr);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@SortingInstr", DBNull.Value);

                //}

                //if (x.Letter != null)
                //{
                //    command2.Parameters.AddWithValue("@Letter", x.Letter);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@Letter", DBNull.Value);

                //}

                //if (x.Brochures_Leaflets != null)
                //{
                //    command2.Parameters.AddWithValue("@Brochures_Leaflets", x.Brochures_Leaflets);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@Brochures_Leaflets", DBNull.Value);

                //}

                //if (x.ReplyEnvelope != null)
                //{
                //    command2.Parameters.AddWithValue("@ReplyEnvelope", x.ReplyEnvelope);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@ReplyEnvelope", DBNull.Value);

                //}

                //if (x.ImgOnStatement != null)
                //{
                //    command2.Parameters.AddWithValue("@ImgOnStatement", x.ImgOnStatement);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@ImgOnStatement", DBNull.Value);

                //}

                //if (x.Booklet != null)
                //{
                //    command2.Parameters.AddWithValue("@Booklet", x.Booklet);
                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@Booklet", DBNull.Value);

                //}

                //if (x.NumberOfInsert != null)
                //{
                //    command2.Parameters.AddWithValue("@NumberOfInsert", x.NumberOfInsert);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@NumberOfInsert", DBNull.Value);

                //}

                //if (x.FinishingInst != null)
                //{
                //    command2.Parameters.AddWithValue("@FinishingInst", x.FinishingInst);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@FinishingInst", DBNull.Value);

                //}

                //if (x.IT_SysNotes != null)
                //{
                //    command2.Parameters.AddWithValue("@IT_SysNotes", x.IT_SysNotes);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@IT_SysNotes", DBNull.Value);

                //}

                //if (x.Produc_PlanningNotes != null)
                //{
                //    command2.Parameters.AddWithValue("@Produc_PlanningNotes", x.Produc_PlanningNotes);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@Produc_PlanningNotes", DBNull.Value);

                //}

                //if (x.PurchasingNotes != null)
                //{
                //    command2.Parameters.AddWithValue("@PurchasingNotes", x.PurchasingNotes);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@PurchasingNotes", DBNull.Value);

                //}

                //if (x.EngineeringNotes != null)
                //{
                //    command2.Parameters.AddWithValue("@EngineeringNotes", x.EngineeringNotes);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@EngineeringNotes", DBNull.Value);

                //}

                //if (x.ArtworkNotes != null)
                //{
                //    command2.Parameters.AddWithValue("@ArtworkNotes", x.ArtworkNotes);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@ArtworkNotes", DBNull.Value);

                //}

                //if (x.Acc_BillingNotes != null)
                //{
                //    command2.Parameters.AddWithValue("@Acc_BillingNotes", x.Acc_BillingNotes);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@Acc_BillingNotes", DBNull.Value);

                //}

                //if (x.DCPNotes != null)
                //{
                //    command2.Parameters.AddWithValue("@DCPNotes", x.DCPNotes);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@DCPNotes", DBNull.Value);

                //}

                //if (x.PostingInfo != null)
                //{
                //    command2.Parameters.AddWithValue("@PostingInfo", x.PostingInfo);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@PostingInfo", DBNull.Value);

                //}

                //if (x.Cust_Department != null)
                //{
                //    command2.Parameters.AddWithValue("@Cust_Department", x.Cust_Department);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@Cust_Department", DBNull.Value);

                //}

                //if (x.ActiveSts != null)
                //{
                //    command2.Parameters.AddWithValue("@ActiveSts", x.ActiveSts);

                //}
                //else
                //{

                //    command2.Parameters.AddWithValue("@ActiveSts", DBNull.Value);

                //}

                //command2.ExecuteNonQuery();

            }

            return RedirectToAction("ManageJobInstruction", "MBD");

        }

        if (set == "AddNew")
        {
            if (JobClass == "DAILY")
            {

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    int _bil = 1;
                    cn.Open();
                    command.CommandText = @"SELECT Id,Customer_Name,ProductName,JobSheetNo,JobClass,JobType,
                                                   AccountsQty,ImpressionQty,PagesQty,
                                                   IT_SysNotes,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes,
                                                   ArtworkNotes,Acc_BillingNotes,DCPNotes,PostingInfo
                                                   FROM [IflowSeed].[dbo].[JobInstruction] 
                                                   WHERE Id=@Id";
                    command.Parameters.AddWithValue("@Id", Id);
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
                                model.JobSheetNo = reader.GetString(3);
                            }
                            if (reader.IsDBNull(4) == false)
                            {
                                model.JobClass = reader.GetString(4);
                            }
                            if (reader.IsDBNull(5) == false)
                            {
                                model.JobType = reader.GetString(5);
                            }
                            if (reader.IsDBNull(6) == false)
                            {
                                model.AccountsQty = reader.GetString(6);
                            }
                            if (reader.IsDBNull(7) == false)
                            {
                                model.ImpressionQty = reader.GetString(7);
                            }
                            if (reader.IsDBNull(8) == false)
                            {
                                model.PagesQty = reader.GetString(8);
                            }
                            if (reader.IsDBNull(9) == false)
                            {
                                model.IT_SysNotes = reader.GetString(9);
                            }
                            if (reader.IsDBNull(10) == false)
                            {
                                model.Produc_PlanningNotes = reader.GetString(10);
                            }
                            if (reader.IsDBNull(11) == false)
                            {
                                model.PurchasingNotes = reader.GetString(11);
                            }
                            if (reader.IsDBNull(12) == false)
                            {
                                model.EngineeringNotes = reader.GetString(12);
                            }
                            if (reader.IsDBNull(13) == false)
                            {
                                model.ArtworkNotes = reader.GetString(13);
                            }
                            if (reader.IsDBNull(14) == false)
                            {
                                model.Acc_BillingNotes = reader.GetString(14);
                            }
                            if (reader.IsDBNull(15) == false)
                            {
                                model.DCPNotes = reader.GetString(15);
                            }
                            if (reader.IsDBNull(16) == false)
                            {
                                model.PostingInfo = reader.GetString(16);
                            }


                        }



                        using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            Guid JobAuditTrailId = Guid.NewGuid();
                            ViewBag.Id = Id;
                            string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                            cn1.Open();
                            SqlCommand command1;
                            command1 = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[JobAuditTrail](Id, CreatedOn, Customer_Name, ProductName, JobSheetNo, JobClass, JobType, Status, AccountsQty, ImpressionQty, PagesQty, NotesByIT, NotesByProduction, NotesByPurchasing, NotesByEngineering, NotesByArtwork, NotesByFinance, NotesByDCP, PostingInfo, JobInstructionId) values (@Id, @CreatedOn, @Customer_Name, @ProductName, @JobSheetNo, @JobClass, @JobType, @Status, @AccountsQty, @ImpressionQty, @PagesQty,  @NotesByIT, @NotesByProduction, @NotesByPurchasing, @NotesByEngineering, @NotesByArtwork, @NotesByFinance, @NotesByDCP, @PostingInfo, @JobInstructionId)", cn1);
                            command1.Parameters.AddWithValue("@Id", JobAuditTrailId);
                            command1.Parameters.AddWithValue("@CreatedOn", createdOn);
                            if (model.Customer_Name != null)
                            {
                                command1.Parameters.AddWithValue("@Customer_Name", model.Customer_Name);
                            }
                            else
                            {
                                command1.Parameters.AddWithValue("@Customer_Name", DBNull.Value);
                            }
                            if (model.ProductName != null)
                            {
                                command1.Parameters.AddWithValue("@ProductName", model.ProductName);
                            }
                            else
                            {
                                command1.Parameters.AddWithValue("@ProductName", DBNull.Value);
                            }
                            if (model.JobSheetNo != null)
                            {
                                command1.Parameters.AddWithValue("@JobSheetNo", model.JobSheetNo);
                            }
                            else
                            {
                                command1.Parameters.AddWithValue("@JobSheetNo", DBNull.Value);
                            }
                            if (model.JobClass != null)
                            {
                                command1.Parameters.AddWithValue("@JobClass", model.JobClass);
                            }
                            else
                            {
                                command1.Parameters.AddWithValue("@JobClass", DBNull.Value);
                            }
                            if (model.JobType != null)
                            {
                                command1.Parameters.AddWithValue("@JobType", model.JobType);
                            }
                            else
                            {
                                command1.Parameters.AddWithValue("@JobType", DBNull.Value);
                            }
                            command1.Parameters.AddWithValue("@Status", "New");
                            if (model.AccountsQty != null)
                            {
                                command1.Parameters.AddWithValue("@AccountsQty", model.AccountsQty);
                            }
                            else
                            {
                                command1.Parameters.AddWithValue("@AccountsQty", DBNull.Value);
                            }
                            if (model.ImpressionQty != null)
                            {
                                command1.Parameters.AddWithValue("@ImpressionQty", model.ImpressionQty);
                            }
                            else
                            {
                                command1.Parameters.AddWithValue("@ImpressionQty", DBNull.Value);
                            }
                            if (PagesQty != null)
                            {
                                command1.Parameters.AddWithValue("@PagesQty", model.PagesQty);
                            }
                            else
                            {
                                command1.Parameters.AddWithValue("@PagesQty", DBNull.Value);
                            }
                            if (model.IT_SysNotes != null)
                            {
                                command1.Parameters.AddWithValue("@NotesByIT", model.IT_SysNotes);
                            }
                            else
                            {
                                command1.Parameters.AddWithValue("@NotesByIT", DBNull.Value);
                            }
                            if (model.Produc_PlanningNotes != null)
                            {
                                command1.Parameters.AddWithValue("@NotesByProduction", model.Produc_PlanningNotes);
                            }
                            else
                            {
                                command1.Parameters.AddWithValue("@NotesByProduction", DBNull.Value);
                            }
                            if (model.PurchasingNotes != null)
                            {
                                command1.Parameters.AddWithValue("@NotesByPurchasing", model.PurchasingNotes);
                            }
                            else
                            {
                                command1.Parameters.AddWithValue("@NotesByPurchasing", DBNull.Value);
                            }
                            if (model.EngineeringNotes != null)
                            {
                                command1.Parameters.AddWithValue("@NotesByEngineering", model.EngineeringNotes);
                            }
                            else
                            {
                                command1.Parameters.AddWithValue("@NotesByEngineering", DBNull.Value);
                            }
                            if (model.ArtworkNotes != null)
                            {
                                command1.Parameters.AddWithValue("@NotesByArtwork", model.ArtworkNotes);
                            }
                            else
                            {
                                command1.Parameters.AddWithValue("@NotesByArtwork", DBNull.Value);
                            }
                            if (model.Acc_BillingNotes != null)
                            {
                                command1.Parameters.AddWithValue("@NotesByFinance", model.Acc_BillingNotes);
                            }
                            else
                            {
                                command1.Parameters.AddWithValue("@NotesByFinance", DBNull.Value);
                            }
                            if (model.DCPNotes != null)
                            {
                                command1.Parameters.AddWithValue("@NotesByDCP", model.DCPNotes);
                            }
                            else
                            {
                                command1.Parameters.AddWithValue("@NotesByDCP", DBNull.Value);
                            }
                            if (model.PostingInfo != null)
                            {
                                command1.Parameters.AddWithValue("@PostingInfo", model.PostingInfo);
                            }
                            else
                            {
                                command1.Parameters.AddWithValue("@PostingInfo", DBNull.Value);
                            }
                            command1.Parameters.AddWithValue("@JobInstructionId", Id);
                            command1.ExecuteNonQuery();
                            cn1.Close();
                            TempData["msg"] = "<script>alert('DAILY JOB ALREADY SENT !');</script>";

                        }
                    }
                }
            }

            else
            {
                TempData["msg"] = "<script>alert('THIS ACTION CANNOT BE PROCESS !');</script>";
            }

            return RedirectToAction("ManageDailyJob", "ITO");


        }



        return View(JobInstructionlist); //hntr data ke ui
    }

    public  ActionResult test()
    {
        string id = "testing1";

        ViewBag.id = id;

        return View();
    }

    public ActionResult CreateBatchJI(string Id, string Customer_Name, string ProductName, string JobSheetNo, string SalesExecutiveBy, string Cust_Department, string ReffSub, string set)
    {

        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];


        ViewBag.IsDepart = @Session["Department"];
        ViewBag.AccountManager = IdentityName.ToString();

        //if (!string.IsNullOrEmpty(Customer_Name) && !string.IsNullOrEmpty(Cust_Department) && !string.IsNullOrEmpty(ProductName))



        if (set=="Submit")
        {

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                Guid Idx = Guid.NewGuid();
                string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                string ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                var No_ = new NoCounterModel();


                cn.Open();
                SqlCommand command;
                command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[JobInstruction] (Id,  Customer_Name, Cust_Department, ProductName, CreatedOn, ModifiedOn, SalesExecutiveBy, Status,reffSub,Iscompleted,JobSheetNo,QuotationRef,CreateUser) values (@Id,  @Customer_Name, @Cust_Department, @ProductName, @CreatedOn,@ModifiedOn, @SalesExecutiveBy, @Status,@reffSub,@Iscompleted,@JobSheetNo,@QuotationRef,@CreateUser)", cn);
                command.Parameters.AddWithValue("@Id", Idx);

                command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                command.Parameters.AddWithValue("@Cust_Department", Cust_Department);
                command.Parameters.AddWithValue("@ProductName", ProductName);
                command.Parameters.AddWithValue("@CreatedOn", createdOn);
                command.Parameters.AddWithValue("@ModifiedOn", ModifiedOn);
                command.Parameters.AddWithValue("@SalesExecutiveBy", IdentityName.ToString());
                command.Parameters.AddWithValue("@Status", "New");
                command.Parameters.AddWithValue("@CreateUser", IdentityName.ToString());

                if (!string.IsNullOrEmpty(ReffSub))
                {
                    Debug.WriteLine("reffSub Value :" + ReffSub);
                    command.Parameters.AddWithValue("@reffSub", ReffSub);
                }
                else
                {
                    Debug.WriteLine("below value should be empty :");
                    Debug.WriteLine("reffSub Value :" + ReffSub);
                    command.Parameters.AddWithValue("@reffSub", DBNull.Value);
                }
                command.Parameters.AddWithValue("@Iscompleted", "0");
                command.Parameters.AddWithValue("@JobSheetNo", No_.RefNo);
                if (!string.IsNullOrEmpty(ReffSub))
                {
                    Debug.WriteLine("Quotation Ref Value :" + ReffSub);
                    command.Parameters.AddWithValue("@QuotationRef", ReffSub);
                }
                else
                {
                    Debug.WriteLine("below value should be empty :");
                    Debug.WriteLine("Quotation Ref Value :" + ReffSub);
                    command.Parameters.AddWithValue("@QuotationRef", DBNull.Value);
                }


                command.ExecuteNonQuery();
                cn.Close();
            }
            //bila save akn gi kt managecustomer
            return RedirectToAction("ManageJobInstruction", "MBD");


        }





        int _bil = 1;
        List<SelectListItem> li = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Customer_Name FROM [IflowSeed].[dbo].[CustomerProduct]                          
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

        if (!string.IsNullOrEmpty(Customer_Name))
        {
            int _bil2 = 1;
            List<SelectListItem> li2 = new List<SelectListItem>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT DISTINCT Cust_Department FROM [IflowSeed].[dbo].[CustomerDetails]    
                                     WHERE Customer_Name=@Customer_Name                            
                                     ORDER BY Cust_Department";
                command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JobInstruction model = new JobInstruction();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            model.Cust_Department = reader.GetString(0);
                        }
                    }
                    int i = _bil2++;
                    if (i == 1)
                    {
                        li2.Add(new SelectListItem { Text = "Please Select" });
                    }
                    li2.Add(new SelectListItem { Text = model.Cust_Department });
                }
                cn.Close();
            }
            ViewData["Cust_Department_"] = li2;
        }
        else
        {
            List<SelectListItem> li2 = new List<SelectListItem>();
            li2.Add(new SelectListItem { Text = "Please Select" });
            ViewData["Cust_Department_"] = li2;
        }


        if (!string.IsNullOrEmpty(Customer_Name))
        {
            int _bil2 = 1;
            List<SelectListItem> li3 = new List<SelectListItem>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT DISTINCT ProductName FROM [IflowSeed].[dbo].[CustomerProduct]    
                                     WHERE Customer_Name=@Customer_Name                            
                                     ORDER BY ProductName";
                command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JobInstruction model = new JobInstruction();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            model.ProductName = reader.GetString(0);
                        }
                    }
                    int i = _bil2++;
                    if (i == 1)
                    {
                        li3.Add(new SelectListItem { Text = "Please Select" });
                    }
                    li3.Add(new SelectListItem { Text = model.ProductName });
                }
                cn.Close();
            }
            ViewData["Product_"] = li3;
        }
        else
        {
            List<SelectListItem> li3 = new List<SelectListItem>();
            li3.Add(new SelectListItem { Text = "Please Select" });
            ViewData["Product_"] = li3;
        }

        if (!string.IsNullOrEmpty(Customer_Name))
        {
            int _bil2 = 1;
            List<SelectListItem> li4 = new List<SelectListItem>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT DISTINCT reffSub FROM [IflowSeed].[dbo].[Quotation]   
                                     WHERE Customer_Name=@Customer_Name AND Approve_Fin=1 AND Approve_MBD=1
                                         ORDER BY reffSub";
                command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    QuotationModel model = new QuotationModel();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            model.ReffSub = reader.GetString(0);
                        }
                    }
                    int i = _bil2++;
                    if (i == 1)
                    {
                        li4.Add(new SelectListItem { Text = "Please Select" });
                    }

                    li4.Add(new SelectListItem { Text = model.ReffSub });
                }
                cn.Close();
            }
            ViewData["ReffSub_"] = li4;
        }
        else
        {
            List<SelectListItem> li4 = new List<SelectListItem>();
            li4.Add(new SelectListItem { Text = "Please Select" });
            ViewData["ReffSub_"] = li4;
        }



        return View();
    }


    public ActionResult DeleteJI(string Id)
    {
        if (!string.IsNullOrEmpty(Id))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("DELETE [IflowSeed].[dbo].[JobInstruction] WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn.Close();
            }
        }
        return RedirectToAction("ManageJobInstruction", "MBD");
    }

    [ValidateInput(false)]
    public ActionResult CreateNewJI(string Id, string set, string JobInstructionId, string tabs, string Customer_Name, string ProductName, string JobSheetNo,
                                      string SalesExecutiveBy, string Status,
                                      string ServiceLevel, string IsSlaCreaditCard, string JobClass, string IsSetPaper,
                                      string JobRequest, string ExpectedDateCompletionToGpo, string QuotationRef, string Contract_Name,
                                      string Contact_Person, string JobType, string DeliveryChannel, string AccountsQty, string ImpressionQty,
                                      string PagesQty, string CycleTerm, string MailingDate,
                                      string JoiningFiles, string TotalRecord, string InputFileName, string OutputFileName, string Sorting,
                                      string SortingMode, string Other, string DataPrintingRemark,
                                      string ArtworkStatus, string PaperStock, string TypeCode, string Paper, string PaperSize,
                                      string Grammage, string MaterialColour, string EnvelopeStock, string EnvelopeType, string EnvelopeSize,
                                      string EnvelopeGrammage, string EnvelopeColour, string EnvelopeWindow, string EnvWindowOpaque,
                                      string LabelStock, string LabelCutsheet, string OthersStock, string BalancedMaterial,
                                      string PlasticStock, string PlasticType, string PlasticSize, string PlasticThickness,
                                      string PrintingType, string PrintingOrientation, string GpoList, string RegisterMail,
                                      string OtherList, string BaseStockType, string FinishingSize, string AdditionalPrintingMark,
                                      string SortingCriteria, string PrintingInstr, string SortingInstr, string JobInstruction,
                                      string Picture_FileId, string Picture_Extension, string Letter, string Brochures_Leaflets,
                                      string ReplyEnvelope, string ImgOnStatement, string Booklet,
                                      string NumberOfInsert, string Magezine1, string Brochure1, string CarrierSheet1, string Newsletter1,
                                      string Statement1, string Booklet1, string CommentManualType, string FinishingFormat,
                                      string FoldingType, string Sealing1, string Tearing1, string BarcodeLabel1, string Cutting1,
                                      string StickingOf1, string AddLabel1, string Sticker1, string Chesire1, string Tuck_In1,
                                      string Bursting1, string Sealed1, string Folding1, string Unsealed1, string Letter1, string FinishingInst,
                                      string IT_SysNotes, string Produc_PlanningNotes, string PurchasingNotes, string EngineeringNotes,
                                      string ArtworkNotes, string Acc_BillingNotes, string DCPNotes, string PostingInfo,
                                      string RTMix, string RTCourierChanges, string RTChargeFranking, string RTSelfMailer, string RTPostage, string RTDeliveryCharges, string RTFranking, string RTImprest,
                                      string Other1, string Other2, string Other3, string Other4, string Other5, string RTServiceChanges,
                                      string PrintingDuplex, string Inserting1, string Inserting2, string Inserting3, string Inserting4, string BrochureInsert, string MailBelow_5K, string Handling, string PI_Tearing, string PI_Sealing, string PI_Folding, string Sticking, string Labelling, string Matching, string CDArchiving, string Npc,
                                      string PI_EnvelopeType, string PI_EnvelopePrice, string PI_Paper, string PI_PaperPrice, string SM_PrintingSM, string SM_SM_Material, string SM_MailBelow_5K, string SM_Handling,
                                      string SM_Paper, string SM_PaperPrice, string SM_Paper2, string SM_PaperPrice2, string MMP_PrintingDuplex, string MMP_FirstInsert, string MMP_SecondInsert, string MMP_InsertingMMP, string MMP_BrochureInsert, string MMP_MailBelow_5K, string MMP_Handling, string MMP_Sealing, string MMP_Tearing, string MMP_Folding, string MMP_Sticking, string MMP_Labelling,
                                      string MMP_Matching, string MMP_EnvelopeType, string MMP_CDArchiving, string MMP_EnvelopePrice, string MMP_Paper, string MMP_PaperPrice, string DCP_FoldingCharges, string DCP_SupplyPrintLabel, string DCP_PrintingDuplex, string DCP_FirstInsert, string DCP_SecondInsert, string DCP_BrochureInsert, string DCP_MailBelow_5K, string DCP_Handling, string DCP_Sealing, string DCP_Tearing,
                                      string DCP_Folding, string DCP_Sticking, string DCP_Labelling, string DCP_Matching, string DCP_CDArchiving, string DCP_EnvelopeType, string DCP_EnvelopePrice, string DCP_Paper, string DCP_PaperPrice, string RM_Printing, string RM_Selfmailer, string RM_MailBelow_5K, string RM_Handling,
                                      string RM_LabellingRegsterMails, string RM_Paper, string RM_PaperPrice, string PrintingDuplex2, string RM_PaperPrice2, string RM_Paper2,
                                      string LBPrintingDuplex, string LBInserting1, string LBInserting2, string LBInserting3, string LBInserting4, string LBBrochureInsert, string LBMailBelow_5K, string LBHandling, string LBPI_Sealing, string LBPI_Tearing, string LBPI_Folding, string LBSticking, string LBLabelling, string LBMatching, string LBCDArchiving, string LBNpc, string LBPI_EnvelopeType, string LBPI_EnvelopePrice, string LBPI_Paper, string LBPI_PaperPrice, string LBSM_PrintingSM, string LBSM_SM_Material, string LBSM_MailBelow_5K, string LBSM_Handling, string LBSM_Paper, string LBSM_PaperPrice, string LBSM_Paper2, string LBSM_PaperPrice2,
                                      string LBMMP_PrintingDuplex, string LBMMP_FirstInsert, string LBMMP_SecondInsert, string LBMMP_InsertingMMP, string LBMMP_BrochureInsert, string LBMMP_MailBelow_5K, string LBMMP_Handling, string LBMMP_Sealing, string LBMMP_Tearing, string LBMMP_Folding, string LBMMP_Sticking, string LBMMP_Labelling, string LBMMP_Matching, string LBMMP_CDArchiving, string LBMMP_EnvelopeType, string LBMMP_EnvelopePrice, string LBMMP_Paper, string LBMMP_PaperPrice,
                                      string LBDCP_FoldingCharges, string LBDCP_SupplyPrintLabel, string LBDCP_PrintingDuplex, string LBDCP_FirstInsert, string LBDCP_SecondInsert, string LBDCP_BrochureInsert, string LBDCP_MailBelow_5K, string LBDCP_Handling, string LBDCP_Sealing, string LBDCP_Tearing, string LBDCP_Folding, string LBDCP_Sticking, string LBDCP_Labelling, string LBDCP_Matching, string LBDCP_CDArchiving, string LBDCP_EnvelopeType, string LBDCP_EnvelopePrice, string LBDCP_Paper,
                                      string LBDCP_PaperPrice, string LBRM_Printing, string LBRM_Selfmailer, string LBRM_MailBelow_5K, string LBRM_Handling, string LBRM_LabellingRegsterMails, string LBRM_Paper, string LBRM_PaperPrice, string LBPrintingDuplex2, string LBRM_Paper2, string LBRM_PaperPrice2, string NewMR, string set2,

                                      JobInstruction get)
    {
        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];
        ViewBag.IsDepart = @Session["Department"];
        string dept = @Session["Department"].ToString();
        Session["JobInstructionId"] = Id;
        Session["Id"] = Id;
        Session["Customer_Name"] = Customer_Name;
        ViewBag.JobSheetNo = JobSheetNo;
        ViewBag.Customer_Name = Customer_Name;
        ViewBag.Id = Id;

        List<SelectListItem> listPrintingType = new List<SelectListItem>();

        listPrintingType.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listPrintingType.Add(new SelectListItem { Text = "SIMPLEX", Value = "SIMPLEX" });
        listPrintingType.Add(new SelectListItem { Text = "DUPLEX", Value = "DUPLEX" });

        ViewData["PrintingType_"] = listPrintingType;


        List<SelectListItem> listPrintingOrientation = new List<SelectListItem>();

        listPrintingOrientation.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listPrintingOrientation.Add(new SelectListItem { Text = "LANDSCAPE", Value = "LANDSCAPE" });
        listPrintingOrientation.Add(new SelectListItem { Text = "PORTRAIT", Value = "PORTRAIT" });

        ViewData["PrintingOrientation_"] = listPrintingOrientation;

        List<SelectListItem> listBaseStockType = new List<SelectListItem>();

        listBaseStockType.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listBaseStockType.Add(new SelectListItem { Text = "CONTINUES", Value = "CONTINUES" });
        listBaseStockType.Add(new SelectListItem { Text = "CUT SHEET", Value = "CUT SHEET" });
        listBaseStockType.Add(new SelectListItem { Text = "CUT SHEET OR CONTINUES", Value = "CUT SHEET OR CONTINUES" });
        listBaseStockType.Add(new SelectListItem { Text = "N/A", Value = "N/A" });

        ViewData["BaseStockType_"] = listBaseStockType;

        List<SelectListItem> listAdditionalPrintingMark = new List<SelectListItem>();

        listAdditionalPrintingMark.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listAdditionalPrintingMark.Add(new SelectListItem { Text = "OMR", Value = "OMR" });
        listAdditionalPrintingMark.Add(new SelectListItem { Text = "OMS", Value = "OMS" });
        listAdditionalPrintingMark.Add(new SelectListItem { Text = "N/A", Value = "N/A" });

        ViewData["AdditionalPrintingMark_"] = listAdditionalPrintingMark;

        List<SelectListItem> listSortingInstr = new List<SelectListItem>();

        listSortingInstr.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listSortingInstr.Add(new SelectListItem { Text = "DEDUP", Value = "DEDUP" });
        listSortingInstr.Add(new SelectListItem { Text = "YES", Value = "YES" });
        listSortingInstr.Add(new SelectListItem { Text = "NO", Value = "NO" });
        listSortingInstr.Add(new SelectListItem { Text = "OVERSEA", Value = "OVERSEA" });

        ViewData["SortingInstr_"] = listSortingInstr;

        List<SelectListItem> listFinishingFormat = new List<SelectListItem>();

        listFinishingFormat.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listFinishingFormat.Add(new SelectListItem { Text = "SELF MAILER / TONER SEAL", Value = "SELF MAILER / TONER SEAL" });
        listFinishingFormat.Add(new SelectListItem { Text = "PRESSURE SEAL", Value = "PRESSURE SEAL" });

        ViewData["FinishingFormat_"] = listFinishingFormat;

        if (!string.IsNullOrEmpty(Customer_Name))
        {
            int _bil3 = 1;
            List<SelectListItem> li3 = new List<SelectListItem>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT DISTINCT Contract_Name FROM [IflowSeed].[dbo].[CustomerContract]    
                                            WHERE Customer_Name = @Customer_Name";
                command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JobInstruction model = new JobInstruction();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            model.Contract_Name = reader.GetString(0);
                        }
                    }
                    int i = _bil3++;
                    if (i == 1)
                    {
                        li3.Add(new SelectListItem { Text = "Please Select" });
                    }
                    li3.Add(new SelectListItem { Text = model.Contract_Name });
                }
                cn.Close();
            }
            ViewData["ContractName_"] = li3;
        }
        else
        {
            List<SelectListItem> li3 = new List<SelectListItem>();
            li3.Add(new SelectListItem { Text = "Please Select" });
            ViewData["ContractName_"] = li3;
        }


        int _bil = 1;
        List<SelectListItem> li = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Contact_Person FROM [IflowSeed].[dbo].[CustomerDetails]          
                                     WHERE Customer_Name = @Customer_Name";
            command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.Contact_Person = reader.GetString(0);
                    }
                }
                int i = _bil++;
                if (i == 1)
                {
                    li.Add(new SelectListItem { Text = "Please Select" });
                    li.Add(new SelectListItem { Text = model.Contact_Person });
                }
                else
                {
                    li.Add(new SelectListItem { Text = model.Contact_Person });
                }
            }
            cn.Close();
        }
        ViewData["ContactPerson_"] = li;



        int _bil1 = 1;
        List<SelectListItem> li1 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT TypeCode FROM [IflowSeed].[dbo].[PaperInfo]                          
                                     ORDER BY TypeCode";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.TypeCode = reader.GetString(0);
                    }
                }
                int i = _bil1++;
                if (i == 1)
                {
                    li1.Add(new SelectListItem { Text = "Please Select" });
                    li1.Add(new SelectListItem { Text = model.TypeCode });

                }
                else
                {
                    li1.Add(new SelectListItem { Text = model.TypeCode });
                }
            }
            cn.Close();
        }
        ViewData["TypeCode_"] = li1;

        int _bil2 = 1;
        List<SelectListItem> li2 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Paper FROM [IflowSeed].[dbo].[PaperInfo]                          
                                     ORDER BY Paper";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.Paper = reader.GetString(0);
                    }
                }
                int i = _bil2++;
                if (i == 1)
                {
                    li2.Add(new SelectListItem { Text = "Please Select" });
                    li2.Add(new SelectListItem { Text = model.Paper });

                }
                else
                {
                    li2.Add(new SelectListItem { Text = model.Paper });
                }
            }
            cn.Close();
        }
        ViewData["Paper_"] = li2;




        int _bil4 = 1;
        List<SelectListItem> li4 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Type FROM [IflowSeed].[dbo].[JobClass]          
                                     ORDER BY Type";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.JobClass = reader.GetString(0);
                    }
                }
                int i = _bil4++;
                if (i == 1)
                {
                    li4.Add(new SelectListItem { Text = "Please Select" });
                    li4.Add(new SelectListItem { Text = model.JobClass });

                }
                else
                {
                    li4.Add(new SelectListItem { Text = model.JobClass });
                }
            }
            cn.Close();
        }
        ViewData["JobClass_"] = li4;

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

        int _bil6 = 1;
        List<SelectListItem> li6 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Channel FROM [IflowSeed].[dbo].[DeliveryChannel]          
                                     ORDER BY Channel ASC";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.DeliveryChannel = reader.GetString(0);
                    }
                }
                int i = _bil6++;
                if (i == 1)
                {
                    li6.Add(new SelectListItem { Text = "Please Select" });
                    li6.Add(new SelectListItem { Text = model.DeliveryChannel });

                }
                else
                {
                    li6.Add(new SelectListItem { Text = model.DeliveryChannel });
                }
            }
            cn.Close();
        }
        ViewData["DeliveryChannel_"] = li6;

        int _bil7 = 1;
        List<SelectListItem> li7 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Description FROM [IflowSeed].[dbo].[MaterialCharges]         
                                    WHERE MaterialType='Envelope' ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.EnvelopeType = reader.GetString(0);
                    }
                }
                int i = _bil7++;
                if (i == 1)
                {
                    li7.Add(new SelectListItem { Text = "Please Select" });
                    li7.Add(new SelectListItem { Text = model.EnvelopeType });

                }
                else
                {
                    li7.Add(new SelectListItem { Text = model.EnvelopeType });
                }
            }
            cn.Close();
        }
        ViewData["EnvelopeType_"] = li7;

        int _bil8 = 1;
        List<SelectListItem> li8 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Size FROM [IflowSeed].[dbo].[EnvelopeSize]          
                                    ORDER BY Size ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.EnvelopeSize = reader.GetString(0);
                    }
                }
                int i = _bil8++;
                if (i == 1)
                {
                    li8.Add(new SelectListItem { Text = "Please Select" });
                    li8.Add(new SelectListItem { Text = model.EnvelopeSize });

                }
                else
                {
                    li8.Add(new SelectListItem { Text = model.EnvelopeSize });
                }
            }
            cn.Close();
        }
        ViewData["EnvelopeSize_"] = li8;

        int _bil9 = 1;
        List<SelectListItem> li9 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Size FROM [IflowSeed].[dbo].[PaperSize]          
                                    ORDER BY Size ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.PaperSize = reader.GetString(0);
                    }
                }
                int i = _bil9++;
                if (i == 1)
                {
                    li9.Add(new SelectListItem { Text = "Please Select" });
                    li9.Add(new SelectListItem { Text = model.PaperSize });

                }
                else
                {
                    li9.Add(new SelectListItem { Text = model.PaperSize });
                }
            }
            cn.Close();
        }
        ViewData["PaperSize_"] = li9;

        int _bil10 = 1;
        List<SelectListItem> li10 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Type FROM [IflowSeed].[dbo].[Grammage]          
                                    ORDER BY Type ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.Grammage = reader.GetString(0);
                    }
                }
                int i = _bil10++;
                if (i == 1)
                {
                    li10.Add(new SelectListItem { Text = "Please Select" });
                    li10.Add(new SelectListItem { Text = model.Grammage });

                }
                else
                {
                    li10.Add(new SelectListItem { Text = model.Grammage });
                }
            }
            cn.Close();
        }
        ViewData["Grammage_"] = li10;

        int _bil11 = 1;
        List<SelectListItem> li11 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Size FROM [IflowSeed].[dbo].[FinishingSize]          
                                    ORDER BY Size ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.FinishingSize = reader.GetString(0);
                    }
                }
                int i = _bil11++;
                if (i == 1)
                {
                    li11.Add(new SelectListItem { Text = "Please Select" });
                    li11.Add(new SelectListItem { Text = model.FinishingSize });

                }
                else
                {
                    li11.Add(new SelectListItem { Text = model.FinishingSize });
                }
            }
            cn.Close();
        }
        ViewData["FinishingSize_"] = li11;

        int _bil12 = 1;
        List<SelectListItem> li12 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Mode FROM [IflowSeed].[dbo].[SortingMode]          
                                    ORDER BY Mode ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.SortingMode = reader.GetString(0);
                    }
                }
                int i = _bil12++;
                if (i == 1)
                {
                    li12.Add(new SelectListItem { Text = "Please Select" });
                    li12.Add(new SelectListItem { Text = model.SortingMode });

                }
                else
                {
                    li12.Add(new SelectListItem { Text = model.SortingMode });
                }
            }
            cn.Close();
        }
        ViewData["SortingMode_"] = li12;

        int _bil13 = 1;
        List<SelectListItem> li13 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Status FROM [IflowSeed].[dbo].[ArtworkStatus]          
                                    ORDER BY Status ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.ArtworkStatus = reader.GetString(0);
                    }
                }
                int i = _bil13++;
                if (i == 1)
                {
                    li13.Add(new SelectListItem { Text = "Please Select" });
                    li13.Add(new SelectListItem { Text = model.ArtworkStatus });

                }
                else
                {
                    li13.Add(new SelectListItem { Text = model.ArtworkStatus });
                }
            }
            cn.Close();
        }
        ViewData["ArtworkStatus_"] = li13;

        int _bil14 = 1;
        List<SelectListItem> li14 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Stock FROM [IflowSeed].[dbo].[EnvelopeStock]          
                                    ORDER BY Stock ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.EnvelopeStock = reader.GetString(0);
                    }
                }
                int i = _bil14++;
                if (i == 1)
                {
                    li14.Add(new SelectListItem { Text = "Please Select" });
                    li14.Add(new SelectListItem { Text = model.EnvelopeStock });

                }
                else
                {
                    li14.Add(new SelectListItem { Text = model.EnvelopeStock });
                }
            }
            cn.Close();
        }
        ViewData["EnvelopeStock_"] = li14;

        int _bil15 = 1;
        List<SelectListItem> li15 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT Window FROM [IflowSeed].[dbo].[EnvelopeWindow]          
                                    ORDER BY Window ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.EnvelopeWindow = reader.GetString(0);
                    }
                }
                int i = _bil15++;
                if (i == 1)
                {
                    li15.Add(new SelectListItem { Text = "Please Select" });
                    li15.Add(new SelectListItem { Text = model.EnvelopeWindow });

                }
                else
                {
                    li15.Add(new SelectListItem { Text = model.EnvelopeWindow });
                }
            }
            cn.Close();
        }
        ViewData["EnvelopeWindow_"] = li15;

        int _bil16 = 1;
        List<SelectListItem> li16 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT Opaque FROM [IflowSeed].[dbo].[EnvWindowOpaque]          
                                    ORDER BY Opaque ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.EnvWindowOpaque = reader.GetString(0);
                    }
                }
                int i = _bil16++;
                if (i == 1)
                {
                    li16.Add(new SelectListItem { Text = "Please Select" });
                    li16.Add(new SelectListItem { Text = model.EnvWindowOpaque });

                }
                else
                {
                    li16.Add(new SelectListItem { Text = model.EnvWindowOpaque });
                }
            }
            cn.Close();
        }
        ViewData["EnvWindowOpaque_"] = li16;

        int _bil17 = 1;
        List<SelectListItem> li17 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Stock FROM [IflowSeed].[dbo].[PlasticStock]          
                                    ORDER BY Stock ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.PlasticStock = reader.GetString(0);
                    }
                }
                int i = _bil17++;
                if (i == 1)
                {
                    li17.Add(new SelectListItem { Text = "Please Select" });
                    li17.Add(new SelectListItem { Text = model.PlasticStock });

                }
                else
                {
                    li17.Add(new SelectListItem { Text = model.PlasticStock });
                }
            }
            cn.Close();
        }
        ViewData["PlasticStock_"] = li17;

        int _bil18 = 1;
        List<SelectListItem> li18 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Label FROM [IflowSeed].[dbo].[LabelCutsheet]          
                                    ORDER BY Label ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.LabelCutsheet = reader.GetString(0);
                    }
                }
                int i = _bil18++;
                if (i == 1)
                {
                    li18.Add(new SelectListItem { Text = "Please Select" });
                    li18.Add(new SelectListItem { Text = model.LabelCutsheet });

                }
                else
                {
                    li18.Add(new SelectListItem { Text = model.LabelCutsheet });
                }
            }
            cn.Close();
        }
        ViewData["LabelCutsheet_"] = li18;

        int _bil19 = 1;
        List<SelectListItem> li19 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Action FROM [IflowSeed].[dbo].[BalancedMaterial]          
                                    ORDER BY Action ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.BalancedMaterial = reader.GetString(0);
                    }
                }
                int i = _bil19++;
                if (i == 1)
                {
                    li19.Add(new SelectListItem { Text = "Please Select" });
                    li19.Add(new SelectListItem { Text = model.BalancedMaterial });

                }
                else
                {
                    li19.Add(new SelectListItem { Text = model.BalancedMaterial });
                }
            }
            cn.Close();
        }
        ViewData["BalancedMaterial_"] = li19;

        using (SqlConnection cnNew = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        {
            cnNew.Open();

            if(!string.IsNullOrEmpty(Id))
            {
                SqlCommand cmd0 = new SqlCommand("SELECT * FROM JobInstruction WHERE Id=@IdNew", cnNew);
                cmd0.Parameters.AddWithValue("@IdNew", Id);
                SqlDataReader rm0 = cmd0.ExecuteReader();

                while(rm0.Read())
                {
                    JobSheetNo = rm0["JobSheetNo"].ToString();
                }
            }
            

            SqlCommand cmd1 = new SqlCommand("SELECT * FROM JobInstruction WHERE JobSheetNo=@JobSheetNo",cnNew);
            cmd1.Parameters.AddWithValue("@JobSheetNo", JobSheetNo);
            SqlDataReader rm1 = cmd1.ExecuteReader();

            while(rm1.Read())
            {
                Id = rm1["Id"].ToString();
                Customer_Name = rm1["Customer_Name"].ToString();
                ViewBag.JobsheetNo= rm1["JobSheetNo"].ToString();
                Session["Id"]=Id;
            }

            cnNew.Close();
        }


        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT Id,Customer_Name,ProductName,ServiceLevel,IsSlaCreaditCard,JobClass,IsSetPaper,JobRequest,ExpectedDateCompletionToGpo,QuotationRef,Contract_Name,ContactPerson,JobType,DeliveryChannel,AccountsQty,ImpressionQty,PagesQty,CycleTerm,MailingDate,
                                    JoiningFiles,TotalRecord,InputFileName,OutputFileName,Sorting,SortingMode,Other,DataPrintingRemark,
                                    ArtworkStatus,PaperStock,TypeCode,Paper,PaperSize,Grammage,MaterialColour,EnvelopeStock,EnvelopeType,EnvelopeSize,EnvelopeGrammage,EnvelopeColour,EnvelopeWindow,EnvWindowOpaque,LabelStock,LabelCutsheet,OthersStock,BalancedMaterial,PlasticStock,PlasticType,PlasticSize,PlasticThickness,
                                    PrintingType,PrintingOrientation,GpoList,RegisterMail,OtherList,BaseStockType,FinishingSize,AdditionalPrintingMark,SortingCriteria,PrintingInstr,SortingInstr,Letter,Brochures_Leaflets,ReplyEnvelope,ImgOnStatement,Booklet,
                                    NumberOfInsert,Magezine1,Brochure1,CarrierSheet1,Newsletter1,Statement1,Booklet1,CommentManualType,FinishingFormat,FoldingType,Sealing1,Tearing1,BarcodeLabel1,Cutting1,StickingOf1,AddLabel1,Sticker1,Chesire1,Tuck_In1,Bursting1,Sealed1,Folding1,Unsealed1,Letter1,FinishingInst,
                                    IT_SysNotes,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes,ArtworkNotes,Acc_BillingNotes,DCPNotes,PostingInfo,PrintingDuplex, Inserting1, Inserting2, Inserting3, Inserting4, BrochureInsert,MailBelow_5K,Handling, PI_Tearing, PI_Sealing, PI_Folding, Sticking, Labelling, Matching, CDArchiving, Npc, 
                                    PI_EnvelopeType, PI_EnvelopePrice, PI_Paper, PI_PaperPrice, SM_PrintingSM, SM_SM_Material, SM_MailBelow_5K, SM_Handling, 
                                    SM_Paper, SM_PaperPrice,SM_Paper2, SM_PaperPrice2, MMP_PrintingDuplex, MMP_FirstInsert, MMP_SecondInsert, MMP_InsertingMMP,MMP_BrochureInsert, MMP_MailBelow_5K, MMP_Handling, MMP_Sealing, MMP_Tearing, MMP_Folding, MMP_Sticking, MMP_Labelling, 
                                    MMP_Matching,MMP_EnvelopeType, MMP_CDArchiving, MMP_EnvelopePrice, MMP_Paper, MMP_PaperPrice, DCP_FoldingCharges, DCP_SupplyPrintLabel, DCP_PrintingDuplex, DCP_FirstInsert, DCP_SecondInsert, DCP_BrochureInsert, DCP_MailBelow_5K, DCP_Handling, DCP_Sealing, DCP_Tearing, 
                                    DCP_Folding, DCP_Sticking, DCP_Labelling, DCP_Matching, DCP_CDArchiving,DCP_EnvelopeType, DCP_EnvelopePrice, DCP_Paper, DCP_PaperPrice, RM_Printing, RM_Selfmailer, RM_MailBelow_5K, RM_Handling,
                                    RM_LabellingRegsterMails,RM_Paper,RM_PaperPrice,RM_PaperPrice2,PrintingDuplex2,RM_Paper2,NewMR
                                    FROM [IflowSeed].[dbo].[JobInstruction]
                                    WHERE Id=@Id";
            command.Parameters.AddWithValue("@Id", Id.ToString());
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
                    ViewBag.ServiceLevel = reader.GetString(3);
                }
                if (reader.IsDBNull(4) == false)
                {
                    bool getIsSlaCreaditCard = reader.GetBoolean(4);
                    if (getIsSlaCreaditCard == false)
                    {
                        ViewBag.IsSlaCreaditCard = "";
                    }
                    else
                    {
                        ViewBag.IsSlaCreaditCard = "checked";
                    }
                }
                if (reader.IsDBNull(5) == false)
                {
                    ViewBag.JobClass = reader.GetString(5);
                }
                if (reader.IsDBNull(6) == false)
                {
                    bool getIsSetPaper = reader.GetBoolean(6);
                    if (getIsSetPaper == false)
                    {
                        ViewBag.IsSetPaper = "";
                    }
                    else
                    {
                        ViewBag.IsSetPaper = "checked";
                    }
                }
                if (reader.IsDBNull(7) == false)
                {
                    ViewBag.JobRequest = String.Format("{0:dd/MM/yyyy}", reader.GetDateTime(7));
                }
                if (reader.IsDBNull(8) == false)
                {
                    ViewBag.ExpectedDateCompletionToGpo = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(8));
                }
                if (reader.IsDBNull(9) == false)
                {
                    ViewBag.QuotationRef = reader.GetString(9);
                }
                if (reader.IsDBNull(10) == false)
                {
                    ViewBag.ContractName = reader.GetString(10);
                }
                if (reader.IsDBNull(11) == false)
                {
                    ViewBag.Contact_Person = reader.GetString(11);
                }
                if (reader.IsDBNull(12) == false)
                {
                    ViewBag.JobType = reader.GetString(12);
                }
                if (reader.IsDBNull(13) == false)
                {
                    ViewBag.DeliveryChannel = reader.GetString(13);
                }
                if (reader.IsDBNull(14) == false)
                {
                    ViewBag.AccountsQty = reader.GetString(14);
                }
                if (reader.IsDBNull(15) == false)
                {
                    ViewBag.ImpressionQty = reader.GetString(15);
                }
                if (reader.IsDBNull(16) == false)
                {
                    ViewBag.PagesQty = reader.GetString(16);
                }
                if (reader.IsDBNull(17) == false)
                {
                    ViewBag.CycleTerm = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(17));
                }
                if (reader.IsDBNull(18) == false)
                {
                    ViewBag.MailingDate = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(18));
                }

                if (reader.IsDBNull(19) == false)
                {
                    ViewBag.JoiningFiles = reader.GetString(19);
                }
                if (reader.IsDBNull(20) == false)
                {
                    ViewBag.TotalRecord = reader.GetString(20);
                }
                if (reader.IsDBNull(21) == false)
                {
                    ViewBag.InputFileName = reader.GetString(21);
                }
                if (reader.IsDBNull(22) == false)
                {
                    ViewBag.OutputFileName = reader.GetString(22);
                }
                if (reader.IsDBNull(23) == false)
                {
                    ViewBag.Sorting = reader.GetString(23);
                }
                if (reader.IsDBNull(24) == false)
                {
                    ViewBag.SortingMode = reader.GetString(24);
                }
                if (reader.IsDBNull(25) == false)
                {
                    ViewBag.Other = reader.GetString(25);
                }
                if (reader.IsDBNull(26) == false)
                {
                    ViewBag.DataPrintingRemark = reader.GetString(26);
                }
                if (reader.IsDBNull(27) == false)
                {
                    ViewBag.ArtworkStatus = reader.GetString(27);
                }
                if (reader.IsDBNull(28) == false)
                {
                    ViewBag.PaperStock = reader.GetString(28);
                }
                if (reader.IsDBNull(29) == false)
                {
                    ViewBag.TypeCode = reader.GetString(29);
                }
                if (reader.IsDBNull(30) == false)
                {
                    ViewBag.Paper = reader.GetString(30);
                }
                if (reader.IsDBNull(31) == false)
                {
                    ViewBag.PaperSize = reader.GetString(31);
                }
                if (reader.IsDBNull(32) == false)
                {
                    ViewBag.Grammage = reader.GetString(32);
                }
                if (reader.IsDBNull(33) == false)
                {
                    ViewBag.MaterialColour = reader.GetString(33);
                }
                if (reader.IsDBNull(34) == false)
                {
                    ViewBag.EnvelopeStock = reader.GetString(34);
                }
                if (reader.IsDBNull(35) == false)
                {
                    ViewBag.EnvelopeType = reader.GetString(35);
                }
                if (reader.IsDBNull(36) == false)
                {
                    ViewBag.EnvelopeSize = reader.GetString(36);
                }
                if (reader.IsDBNull(37) == false)
                {
                    ViewBag.EnvelopeGrammage = reader.GetString(37);
                }
                if (reader.IsDBNull(38) == false)
                {
                    ViewBag.EnvelopeColour = reader.GetString(38);
                }
                if (reader.IsDBNull(39) == false)
                {
                    ViewBag.EnvelopeWindow = reader.GetString(39);
                }
                if (reader.IsDBNull(40) == false)
                {
                    ViewBag.EnvWindowOpaque = reader.GetString(40);
                }
                if (reader.IsDBNull(41) == false)
                {
                    ViewBag.LabelStock = reader.GetString(41);
                }
                if (reader.IsDBNull(42) == false)
                {
                    ViewBag.LabelCutsheet = reader.GetString(42);
                }
                if (reader.IsDBNull(43) == false)
                {
                    ViewBag.OthersStock = reader.GetString(43);
                }
                if (reader.IsDBNull(44) == false)
                {
                    ViewBag.BalancedMaterial = reader.GetString(44);
                }
                if (reader.IsDBNull(45) == false)
                {
                    ViewBag.PlasticStock = reader.GetString(45);
                }
                if (reader.IsDBNull(46) == false)
                {
                    ViewBag.PlasticType = reader.GetString(46);
                }
                if (reader.IsDBNull(47) == false)
                {
                    ViewBag.PlasticSize = reader.GetString(47);
                }
                if (reader.IsDBNull(48) == false)
                {
                    ViewBag.PlasticThickness = reader.GetString(48);
                }
                if (reader.IsDBNull(49) == false)
                {
                    ViewBag.PrintingType = reader.GetString(49);
                }
                if (reader.IsDBNull(50) == false)
                {
                    ViewBag.PrintingOrientation = reader.GetString(50);
                }
                if (reader.IsDBNull(51) == false)
                {
                    bool getGpoList = reader.GetBoolean(51);
                    if (getGpoList == false)
                    {
                        ViewBag.GpoList = "";
                    }
                    else
                    {
                        ViewBag.GpoList = "checked";
                    }
                }
                if (reader.IsDBNull(52) == false)
                {
                    bool getRegisterMail = reader.GetBoolean(52);
                    if (getRegisterMail == false)
                    {
                        ViewBag.RegisterMail = "";
                    }
                    else
                    {
                        ViewBag.RegisterMail = "checked";
                    }
                }
                if (reader.IsDBNull(53) == false)
                {
                    ViewBag.OtherList = reader.GetString(53);
                }
                if (reader.IsDBNull(54) == false)
                {
                    ViewBag.BaseStockType = reader.GetString(54);
                }
                if (reader.IsDBNull(55) == false)
                {
                    ViewBag.FinishingSize = reader.GetString(55);
                }
                if (reader.IsDBNull(56) == false)
                {
                    ViewBag.AdditionalPrintingMark = reader.GetString(56);
                }
                if (reader.IsDBNull(57) == false)
                {
                    ViewBag.SortingCriteria = reader.GetString(57);
                }
                if (reader.IsDBNull(58) == false)
                {
                    ViewBag.PrintingInstr = reader.GetString(58);
                }
                if (reader.IsDBNull(59) == false)
                {
                    ViewBag.SortingInstr = reader.GetString(59);
                }
                if (reader.IsDBNull(60) == false)
                {
                    bool getLetter = reader.GetBoolean(60);
                    if (getLetter == false)
                    {
                        ViewBag.Letter = "";
                    }
                    else
                    {
                        ViewBag.Letter = "checked";
                    }
                }
                if (reader.IsDBNull(61) == false)
                {
                    bool getBrochures_Leaflets = reader.GetBoolean(61);
                    if (getBrochures_Leaflets == false)
                    {
                        ViewBag.Brochures_Leaflets = "";
                    }
                    else
                    {
                        ViewBag.Brochures_Leaflets = "checked";
                    }
                }
                if (reader.IsDBNull(62) == false)
                {
                    bool getReplyEnvelope = reader.GetBoolean(62);
                    if (getReplyEnvelope == false)
                    {
                        ViewBag.ReplyEnvelope = "";
                    }
                    else
                    {
                        ViewBag.ReplyEnvelope = "checked";
                    }
                }
                if (reader.IsDBNull(63) == false)
                {
                    bool getImgOnStatement = reader.GetBoolean(63);
                    if (getImgOnStatement == false)
                    {
                        ViewBag.ImgOnStatement = "";
                    }
                    else
                    {
                        ViewBag.ImgOnStatement = "checked";
                    }
                }
                if (reader.IsDBNull(64) == false)
                {
                    bool getBooklet = reader.GetBoolean(64);
                    if (getBooklet == false)
                    {
                        ViewBag.Booklet = "";
                    }
                    else
                    {
                        ViewBag.Booklet = "checked";
                    }
                }
                if (reader.IsDBNull(65) == false)
                {
                    ViewBag.NumberOfInsert = reader.GetString(65);
                }
                if (reader.IsDBNull(66) == false)
                {
                    bool getMagezine1 = reader.GetBoolean(66);
                    if (getMagezine1 == false)
                    {
                        ViewBag.Magezine1 = "";
                    }
                    else
                    {
                        ViewBag.Magezine1 = "checked";
                    }
                }
                if (reader.IsDBNull(67) == false)
                {
                    bool getBrochure1 = reader.GetBoolean(67);
                    if (getBrochure1 == false)
                    {
                        ViewBag.Brochure1 = "";
                    }
                    else
                    {
                        ViewBag.Brochure1 = "checked";
                    }
                }
                if (reader.IsDBNull(68) == false)
                {
                    bool getCarrierSheet1 = reader.GetBoolean(68);
                    if (getCarrierSheet1 == false)
                    {
                        ViewBag.CarrierSheet1 = "";
                    }
                    else
                    {
                        ViewBag.CarrierSheet1 = "checked";
                    }
                }
                if (reader.IsDBNull(69) == false)
                {
                    bool getNewsletter1 = reader.GetBoolean(69);
                    if (getNewsletter1 == false)
                    {
                        ViewBag.Newsletter1 = "";
                    }
                    else
                    {
                        ViewBag.Newsletter1 = "checked";
                    }
                }
                if (reader.IsDBNull(70) == false)
                {
                    bool getStatement1 = reader.GetBoolean(70);
                    if (getStatement1 == false)
                    {
                        ViewBag.Statement1 = "";
                    }
                    else
                    {
                        ViewBag.Statement1 = "checked";
                    }
                }
                if (reader.IsDBNull(71) == false)
                {
                    bool getBooklet1 = reader.GetBoolean(71);
                    if (getBooklet1 == false)
                    {
                        ViewBag.Booklet1 = "";
                    }
                    else
                    {
                        ViewBag.Booklet1 = "checked";
                    }
                }
                if (reader.IsDBNull(72) == false)
                {
                    ViewBag.CommentManualType = reader.GetString(72);
                }
                if (reader.IsDBNull(73) == false)
                {
                    ViewBag.FinishingFormat = reader.GetString(73);
                }
                if (reader.IsDBNull(74) == false)
                {
                    ViewBag.FoldingType = reader.GetString(74);
                }
                if (reader.IsDBNull(75) == false)
                {
                    bool getSealing1 = reader.GetBoolean(75);
                    if (getSealing1 == false)
                    {
                        ViewBag.Sealing1 = "";
                    }
                    else
                    {
                        ViewBag.Sealing1 = "checked";
                    }
                }
                if (reader.IsDBNull(76) == false)
                {
                    bool getTearing1 = reader.GetBoolean(76);
                    if (getTearing1 == false)
                    {
                        ViewBag.Tearing1 = "";
                    }
                    else
                    {
                        ViewBag.Tearing1 = "checked";
                    }
                }
                if (reader.IsDBNull(77) == false)
                {
                    bool getBarcodeLabel1 = reader.GetBoolean(77);
                    if (getBarcodeLabel1 == false)
                    {
                        ViewBag.BarcodeLabel1 = "";
                    }
                    else
                    {
                        ViewBag.BarcodeLabel1 = "checked";
                    }
                }
                if (reader.IsDBNull(78) == false)
                {
                    bool getCutting1 = reader.GetBoolean(78);
                    if (getCutting1 == false)
                    {
                        ViewBag.Cutting1 = "";
                    }
                    else
                    {
                        ViewBag.Cutting1 = "checked";
                    }
                }
                if (reader.IsDBNull(79) == false)
                {
                    ViewBag.StickingOf1 = reader.GetString(79);
                }
                if (reader.IsDBNull(80) == false)
                {
                    bool getAddLabel1 = reader.GetBoolean(80);
                    if (getAddLabel1 == false)
                    {
                        ViewBag.AddLabel1 = "";
                    }
                    else
                    {
                        ViewBag.AddLabel1 = "checked";
                    }
                }
                if (reader.IsDBNull(81) == false)
                {
                    bool getSticker1 = reader.GetBoolean(81);
                    if (getSticker1 == false)
                    {
                        ViewBag.Sticker1 = "";
                    }
                    else
                    {
                        ViewBag.Sticker1 = "checked";
                    }
                }
                if (reader.IsDBNull(82) == false)
                {
                    bool getChesire1 = reader.GetBoolean(82);
                    if (getChesire1 == false)
                    {
                        ViewBag.Chesire1 = "";
                    }
                    else
                    {
                        ViewBag.Chesire1 = "checked";
                    }
                }
                if (reader.IsDBNull(83) == false)
                {
                    bool getTuck_In1 = reader.GetBoolean(83);
                    if (getTuck_In1 == false)
                    {
                        ViewBag.Tuck_In1 = "";
                    }
                    else
                    {
                        ViewBag.Tuck_In1 = "checked";
                    }
                }
                if (reader.IsDBNull(84) == false)
                {
                    bool getBursting1 = reader.GetBoolean(84);
                    if (getBursting1 == false)
                    {
                        ViewBag.Bursting1 = "";
                    }
                    else
                    {
                        ViewBag.Bursting1 = "checked";
                    }
                }
                if (reader.IsDBNull(85) == false)
                {
                    bool getSealed1 = reader.GetBoolean(85);
                    if (getSealed1 == false)
                    {
                        ViewBag.Sealed1 = "";
                    }
                    else
                    {
                        ViewBag.Sealed1 = "checked";
                    }
                }
                if (reader.IsDBNull(86) == false)
                {
                    bool getFolding1 = reader.GetBoolean(86);
                    if (getFolding1 == false)
                    {
                        ViewBag.Folding1 = "";
                    }
                    else
                    {
                        ViewBag.Folding1 = "checked";
                    }
                }
                if (reader.IsDBNull(87) == false)
                {
                    bool getUnsealed1 = reader.GetBoolean(87);
                    if (getUnsealed1 == false)
                    {
                        ViewBag.Unsealed1 = "";
                    }
                    else
                    {
                        ViewBag.Unsealed1 = "checked";
                    }
                }
                if (reader.IsDBNull(88) == false)
                {
                    bool getLetter1 = reader.GetBoolean(88);
                    if (getLetter1 == false)
                    {
                        ViewBag.Letter1 = "";
                    }
                    else
                    {
                        ViewBag.Letter1 = "checked";
                    }
                }
                if (reader.IsDBNull(89) == false)
                {
                    ViewBag.FinishingInst = reader.GetString(89);
                }
                if (reader.IsDBNull(90) == false)
                {
                    ViewBag.IT_SysNotes = reader.GetString(90);
                }
                if (reader.IsDBNull(91) == false)
                {
                    ViewBag.Produc_PlanningNotes = reader.GetString(91);
                }
                if (reader.IsDBNull(92) == false)
                {
                    ViewBag.PurchasingNotes = reader.GetString(92);
                }
                if (reader.IsDBNull(93) == false)
                {
                    ViewBag.EngineeringNotes = reader.GetString(93);
                }
                if (reader.IsDBNull(94) == false)
                {
                    ViewBag.ArtworkNotes = reader.GetString(94);
                }
                if (reader.IsDBNull(95) == false)
                {
                    ViewBag.Acc_BillingNotes = reader.GetString(95);
                }
                if (reader.IsDBNull(96) == false)
                {
                    ViewBag.DCPNotes = reader.GetString(96);
                }
                if (reader.IsDBNull(97) == false)
                {
                    ViewBag.PostingInfo = reader.GetString(97);
                }
                if (reader.IsDBNull(98) == false)
                {
                    ViewBag.PrintingDuplex = reader.GetString(98);
                }
                if (reader.IsDBNull(99) == false)
                {
                    ViewBag.Inserting1 = reader.GetString(99);
                }
                if (reader.IsDBNull(100) == false)
                {
                    ViewBag.Inserting2 = reader.GetString(100);
                }
                if (reader.IsDBNull(102) == false)
                {
                    ViewBag.Inserting3 = reader.GetString(102);
                }
                if (reader.IsDBNull(103) == false)
                {
                    ViewBag.Inserting4 = reader.GetString(103);
                }
                if (reader.IsDBNull(104) == false)
                {
                    ViewBag.BrochureInsert = reader.GetString(104);
                }
                if (reader.IsDBNull(105) == false)
                {
                    ViewBag.MailBelow_5K = reader.GetString(105);
                }
                if (reader.IsDBNull(106) == false)
                {
                    ViewBag.Handling = reader.GetString(106);
                }
                if (reader.IsDBNull(107) == false)
                {
                    ViewBag.PI_Sealing = reader.GetString(107);
                }
                if (reader.IsDBNull(108) == false)
                {
                    ViewBag.PI_Tearing = reader.GetString(108);
                }
                if (reader.IsDBNull(109) == false)
                {
                    ViewBag.PI_Folding = reader.GetString(109);
                }
                if (reader.IsDBNull(110) == false)
                {
                    ViewBag.Sticking = reader.GetString(110);
                }
                if (reader.IsDBNull(111) == false)
                {
                    ViewBag.Labelling = reader.GetString(111);
                }
                if (reader.IsDBNull(112) == false)
                {
                    ViewBag.Matching = reader.GetString(112);
                }
                if (reader.IsDBNull(113) == false)
                {
                    ViewBag.CDArchiving = reader.GetString(113);
                }
                if (reader.IsDBNull(114) == false)
                {
                    ViewBag.Npc = reader.GetString(114);
                }
                if (reader.IsDBNull(115) == false)
                {
                    ViewBag.PI_EnvelopeType = reader.GetString(115);
                }
                if (reader.IsDBNull(116) == false)
                {
                    ViewBag.PI_EnvelopePrice = reader.GetString(116);
                }
                if (reader.IsDBNull(117) == false)
                {
                    ViewBag.PI_Paper = reader.GetString(117);
                }
                if (reader.IsDBNull(118) == false)
                {
                    ViewBag.PI_PaperPrice = reader.GetString(118);
                }
                if (reader.IsDBNull(119) == false)
                {
                    ViewBag.SM_PrintingSM = reader.GetString(119);
                }
                if (reader.IsDBNull(120) == false)
                {
                    ViewBag.SM_SM_Material = reader.GetString(120);
                }
                if (reader.IsDBNull(121) == false)
                {
                    ViewBag.SM_MailBelow_5K = reader.GetString(121);
                }
                //if (reader.IsDBNull(122) == false)
                //{
                //    ViewBag.SM_Handling = reader.GetString(122);
                //}
                if (reader.IsDBNull(122) == false)
                {
                    ViewBag.SM_Paper = reader.GetString(122);
                }
                if (reader.IsDBNull(123) == false)
                {
                    ViewBag.SM_Handling = reader.GetString(123);
                }
                if (reader.IsDBNull(124) == false)
                {
                    ViewBag.SM_PaperPrice = reader.GetString(124);
                }
                if (reader.IsDBNull(125) == false)
                {
                    ViewBag.SM_Paper2 = reader.GetString(125);
                }
                if (reader.IsDBNull(126) == false)
                {
                    ViewBag.SM_PaperPrice2 = reader.GetString(126);
                }
                if (reader.IsDBNull(127) == false)
                {
                    ViewBag.MMP_PrintingDuplex = reader.GetString(127);
                }
                if (reader.IsDBNull(128) == false)
                {
                    ViewBag.MMP_FirstInsert = reader.GetString(128);
                }
                if (reader.IsDBNull(129) == false)
                {
                    ViewBag.MMP_SecondInsert = reader.GetString(129);
                }
                if (reader.IsDBNull(130) == false)
                {
                    ViewBag.MMP_InsertingMMP = reader.GetString(130);
                }
                if (reader.IsDBNull(131) == false)
                {
                    ViewBag.MMP_BrochureInsert = reader.GetString(131);
                }
                if (reader.IsDBNull(132) == false)
                {
                    ViewBag.MMP_MailBelow_5K = reader.GetString(132);
                }
                if (reader.IsDBNull(133) == false)
                {
                    ViewBag.MMP_Handling = reader.GetString(133);
                }
                if (reader.IsDBNull(134) == false)
                {
                    ViewBag.MMP_Sealing = reader.GetString(134);
                }
                if (reader.IsDBNull(135) == false)
                {
                    ViewBag.MMP_CDArchiving = reader.GetString(135);
                }
                if (reader.IsDBNull(136) == false)
                {
                    ViewBag.MMP_EnvelopeType = reader.GetString(136);
                }
                if (reader.IsDBNull(137) == false)
                {
                    ViewBag.MMP_EnvelopePrice = reader.GetString(137);
                }
                if (reader.IsDBNull(138) == false)
                {
                    ViewBag.MMP_Paper = reader.GetString(138);
                }
                if (reader.IsDBNull(139) == false)
                {
                    ViewBag.MMP_PaperPrice = reader.GetString(139);
                }
                if (reader.IsDBNull(140) == false)
                {
                    ViewBag.DCP_FoldingCharges = reader.GetString(140);
                }
                if (reader.IsDBNull(141) == false)
                {
                    ViewBag.DCP_PrintingDuplex = reader.GetString(141);
                }
                if (reader.IsDBNull(142) == false)
                {
                    ViewBag.DCP_FirstInsert = reader.GetString(142);
                }
                if (reader.IsDBNull(143) == false)
                {
                    ViewBag.DCP_SecondInsert = reader.GetString(143);
                }
                if (reader.IsDBNull(144) == false)
                {
                    ViewBag.DCP_BrochureInsert = reader.GetString(144);
                }
                if (reader.IsDBNull(145) == false)
                {
                    ViewBag.DCP_MailBelow_5K = reader.GetString(145);
                }
                if (reader.IsDBNull(146) == false)
                {
                    ViewBag.DCP_Handling = reader.GetString(146);
                }
                if (reader.IsDBNull(147) == false)
                {
                    ViewBag.DCP_Sealing = reader.GetString(147);
                }
                if (reader.IsDBNull(148) == false)
                {
                    ViewBag.DCP_Tearing = reader.GetString(148);
                }
                if (reader.IsDBNull(149) == false)
                {
                    ViewBag.DCP_Folding = reader.GetString(149);
                }
                if (reader.IsDBNull(150) == false)
                {
                    ViewBag.DCP_Sticking = reader.GetString(150);
                }
                if (reader.IsDBNull(151) == false)
                {
                    ViewBag.MMP_Tearing = reader.GetString(151);
                }
                if (reader.IsDBNull(152) == false)
                {
                    ViewBag.MMP_Folding = reader.GetString(152);
                }
                if (reader.IsDBNull(153) == false)
                {
                    ViewBag.MMP_Sticking = reader.GetString(153);
                }
                if (reader.IsDBNull(154) == false)
                {
                    ViewBag.MMP_Labelling = reader.GetString(154);
                }
                if (reader.IsDBNull(155) == false)
                {
                    ViewBag.MMP_Matching = reader.GetString(155);
                }
                if (reader.IsDBNull(156) == false)
                {
                    ViewBag.DCP_SupplyPrintLabel = reader.GetString(156);
                }
                if (reader.IsDBNull(157) == false)
                {
                    ViewBag.DCP_Labelling = reader.GetString(157);
                }
                if (reader.IsDBNull(158) == false)
                {
                    ViewBag.DCP_Matching = reader.GetString(158);
                }
                if (reader.IsDBNull(159) == false)
                {
                    ViewBag.DCP_CDArchiving = reader.GetString(159);
                }
                if (reader.IsDBNull(160) == false)
                {
                    ViewBag.DCP_EnvelopeType = reader.GetString(160);
                }
                if (reader.IsDBNull(161) == false)
                {
                    ViewBag.DCP_EnvelopePrice = reader.GetString(161);
                }
                if (reader.IsDBNull(162) == false)
                {
                    ViewBag.DCP_Paper = reader.GetString(162);
                }
                if (reader.IsDBNull(163) == false)
                {
                    ViewBag.DCP_PaperPrice = reader.GetString(163);
                }
                if (reader.IsDBNull(164) == false)
                {
                    ViewBag.RM_Printing = reader.GetString(164);
                }
                if (reader.IsDBNull(165) == false)
                {
                    ViewBag.RM_Selfmailer = reader.GetString(165);
                }
                if (reader.IsDBNull(166) == false)
                {
                    ViewBag.RM_MailBelow_5K = reader.GetString(166);
                }
                if (reader.IsDBNull(167) == false)
                {
                    ViewBag.RM_Handling = reader.GetString(167);
                }
                if (reader["RM_LabellingRegsterMails"].ToString()!=null)
                {
                    //ViewBag.RM_LabellingRegsterMails = reader.GetString(168);
                    ViewBag.RM_LabellingRegsterMails = reader["RM_LabellingRegsterMails"].ToString();

                }
                if (reader.IsDBNull(169) == false)
                {
                    ViewBag.RM_Paper = reader.GetString(169);
                }
                if (reader.IsDBNull(170) == false)
                {
                    ViewBag.RM_PaperPrice2 = reader.GetString(170);
                }
                if (reader.IsDBNull(171) == false)
                {
                    ViewBag.PrintingDuplex2 = reader.GetString(171);
                }
                if (reader.IsDBNull(172) == false)
                {
                    ViewBag.RM_Paper2 = reader.GetString(172);
                }
                if (reader.IsDBNull(101) == false)
                {
                    ViewBag.RM_PaperPrice = reader.GetString(101);
                }
                if (reader.IsDBNull(173) == false)
                {
                    ViewBag.NewMR = reader.GetString(173);
                }
                //if (reader.IsDBNull(98) == false)
                //{
                //    ViewBag.RTMix = reader.GetString(98);
                //}
                //if (reader.IsDBNull(99) == false)
                //{
                //    ViewBag.RTCourierChanges = reader.GetString(99);
                //}
                //if (reader.IsDBNull(100) == false)
                //{
                //    ViewBag.RTChargeFranking = reader.GetString(100);
                //}
                //if (reader.IsDBNull(101) == false)
                //{
                //    ViewBag.RTSelfMailer = reader.GetString(101);
                //}
                //if (reader.IsDBNull(102) == false)
                //{
                //    ViewBag.RTPostage = reader.GetString(102);
                //}
                //if (reader.IsDBNull(103) == false)
                //{
                //    ViewBag.RTDeliveryCharges = reader.GetString(103);
                //}
                //if (reader.IsDBNull(104) == false)
                //{
                //    ViewBag.RTFranking = reader.GetString(104);
                //}
                //if (reader.IsDBNull(105) == false)
                //{
                //    ViewBag.RTImprest = reader.GetString(105);
                //}
                //if (reader.IsDBNull(106) == false)
                //{
                //    ViewBag.Other1 = reader.GetString(106);
                //}
                //if (reader.IsDBNull(107) == false)
                //{
                //    ViewBag.Other2 = reader.GetString(107);
                //}
                //if (reader.IsDBNull(108) == false)
                //{
                //    ViewBag.Other3 = reader.GetString(108);
                //}
                //if (reader.IsDBNull(109) == false)
                //{
                //    ViewBag.Other4 = reader.GetString(109);
                //}
                //if (reader.IsDBNull(110) == false)
                //{
                //    ViewBag.Other5 = reader.GetString(110);
                //}
                //if (reader.IsDBNull(111) == false)
                //{
                //    ViewBag.RTServiceChanges = reader.GetString(111);
                //}

            }
            cn.Close();

        }

        List<JobInstruction> viewprofile = new List<JobInstruction>();

        List<JobInstruction> viewData = new List<JobInstruction>();
        List<JobInstruction> viewMaterial = new List<JobInstruction>();
        List<JobInstruction> ViewProduction = new List<JobInstruction>();
        List<JobInstruction> ViewFinishing = new List<JobInstruction>();
        List<JobInstruction> viewImportant = new List<JobInstruction>();
        
        if (set == "ProfileJI")
        {
            //if (!string.IsNullOrEmpty(Id) && JobClass != "Please Select" && JobType != "Please Select" && DeliveryChannel != "Please Select" && !string.IsNullOrEmpty(ServiceLevel) && !string.IsNullOrEmpty(JobClass) && !string.IsNullOrEmpty(JobRequest) && !string.IsNullOrEmpty(JobType) && !string.IsNullOrEmpty(DeliveryChannel) && NewMR != "Please Select")

            if (set2=="Submit")
            {
                if(dept=="IT"||dept=="MBD")
                {
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                    string ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                    get.JobRequest = Convert.ToDateTime(get.JobRequestTxt);
                    get.ExpectedDateCompletionToGpo = Convert.ToDateTime(get.ExpectedDateCompletionToGpoTxt);
                    get.CycleTerm = Convert.ToDateTime(get.CycleTermTxt);
                    get.MailingDate = Convert.ToDateTime(get.MailingDateTxt);


                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        cn.Open();
                        SqlCommand command;
                        command = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET ServiceLevel=@ServiceLevel, IsSlaCreaditCard=@IsSlaCreaditCard, JobClass=@JobClass, IsSetPaper=@IsSetPaper, JobRequest=@JobRequest, ExpectedDateCompletionToGpo=@ExpectedDateCompletionToGpo, QuotationRef=@QuotationRef, Contract_Name=@Contract_Name, ContactPerson=@Contact_Person, JobType=@JobType, DeliveryChannel=@DeliveryChannel, AccountsQty=@AccountsQty, ImpressionQty=@ImpressionQty, PagesQty=@PagesQty, CycleTerm=@CycleTerm, MailingDate=@MailingDate,SalesExecutiveBy=@SalesExecutiveBy, NewMR=@NewMR, NMRStatus=@NMRStatus WHERE Id =@Id", cn);
                        command.Parameters.AddWithValue("@ServiceLevel", ServiceLevel);
                        if (IsSlaCreaditCard == "on")
                        {
                            command.Parameters.AddWithValue("@IsSlaCreaditCard", true);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@IsSlaCreaditCard", false);
                        }
                        if (!string.IsNullOrEmpty(JobClass))
                        {
                            command.Parameters.AddWithValue("@JobClass", JobClass);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@JobClass", DBNull.Value);
                        }
                        if (IsSetPaper == "on")
                        {
                            command.Parameters.AddWithValue("@IsSetPaper", true);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@IsSetPaper", false);
                        }

                        if (!string.IsNullOrEmpty(JobRequest))
                        {
                            string ddd = Convert.ToDateTime(JobRequest).ToString("yyyy-MM-dd");
                            command.Parameters.AddWithValue("@JobRequest", ddd);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@JobRequest", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(ExpectedDateCompletionToGpo))
                        {
                            string ddd1 = Convert.ToDateTime(ExpectedDateCompletionToGpo).ToString("yyyy-MM-dd");
                            command.Parameters.AddWithValue("@ExpectedDateCompletionToGpo", ddd1);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@ExpectedDateCompletionToGpo", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(QuotationRef))
                        {
                            command.Parameters.AddWithValue("@QuotationRef", QuotationRef);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@QuotationRef", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(Contract_Name))
                        {
                            command.Parameters.AddWithValue("@Contract_Name", Contract_Name);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Contract_Name", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(Contact_Person))
                        {
                            command.Parameters.AddWithValue("@Contact_Person", Contact_Person);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Contact_Person", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(JobType))
                        {
                            command.Parameters.AddWithValue("@JobType", JobType);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@JobType", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(DeliveryChannel))
                        {
                            command.Parameters.AddWithValue("@DeliveryChannel", DeliveryChannel);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@DeliveryChannel", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(AccountsQty))
                        {
                            command.Parameters.AddWithValue("@AccountsQty", AccountsQty);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@AccountsQty", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(ImpressionQty))
                        {
                            command.Parameters.AddWithValue("@ImpressionQty", ImpressionQty);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@ImpressionQty", DBNull.Value);
                        }
                        command.Parameters.AddWithValue("@PagesQty", PagesQty);

                        if (!string.IsNullOrEmpty(CycleTerm))
                        {
                            string ddd2 = Convert.ToDateTime(CycleTerm).ToString("yyyy-MM-dd");
                            command.Parameters.AddWithValue("@CycleTerm", ddd2);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@CycleTerm", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(MailingDate))
                        {
                            string ddd3 = Convert.ToDateTime(MailingDate).ToString("yyyy-MM-dd");
                            command.Parameters.AddWithValue("@MailingDate", ddd3);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@MailingDate", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(NewMR))
                        {
                            command.Parameters.AddWithValue("@NewMR", NewMR);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@NewMR", DBNull.Value);
                        }

                        command.Parameters.AddWithValue("@SalesExecutiveBy", IdentityName);
                        command.Parameters.AddWithValue("@NMRStatus", "Modified");


                        command.Parameters.AddWithValue("@Id", Id);


                        command.ExecuteNonQuery();

                        SqlCommand updateNMR = new SqlCommand("UPDATE JobInstruction SET NMRStatus='Modified' WHERE Id=@Id2", cn);
                        updateNMR.Parameters.AddWithValue("@Id2", Id);
                        updateNMR.ExecuteNonQuery();
                        cn.Close();
                    }
                }
               
            }

            return RedirectToAction("ManageJobInstruction", "MBD");
        }
        else if (set == "DataProcess")
        {
            if (!string.IsNullOrEmpty(Id))
            {
                if(dept=="IT"||dept=="MBD")
                {
                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        cn.Open();
                        SqlCommand command;
                        command = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET JoiningFiles=@JoiningFiles, TotalRecord=@TotalRecord, InputFileName=@InputFileName, OutputFileName=@OutputFileName, Sorting=@Sorting, SortingMode=@SortingMode, Other=@Other, DataPrintingRemark=@DataPrintingRemark, NMRStatus=@NMRStatus WHERE Id =@Id", cn);
                        if (!string.IsNullOrEmpty(JoiningFiles))
                        {
                            command.Parameters.AddWithValue("@JoiningFiles", JoiningFiles);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@JoiningFiles", DBNull.Value);
                        }

                        command.Parameters.AddWithValue("@NMRStatus", "Modified");


                        if (!string.IsNullOrEmpty(TotalRecord))
                        {
                            command.Parameters.AddWithValue("@TotalRecord", TotalRecord);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@TotalRecord", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(InputFileName))
                        {
                            command.Parameters.AddWithValue("@InputFileName", InputFileName);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@InputFileName", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(OutputFileName))
                        {
                            command.Parameters.AddWithValue("@OutputFileName", OutputFileName);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@OutputFileName", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(Sorting))
                        {
                            command.Parameters.AddWithValue("@Sorting", Sorting);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Sorting", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(SortingMode))
                        {
                            command.Parameters.AddWithValue("@SortingMode", SortingMode);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@SortingMode", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(Other))
                        {
                            command.Parameters.AddWithValue("@Other", Other);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Other", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(DataPrintingRemark))
                        {
                            command.Parameters.AddWithValue("@DataPrintingRemark", DataPrintingRemark);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@DataPrintingRemark", DBNull.Value);
                        }
                        command.Parameters.AddWithValue("@Id", Id);
                        command.ExecuteNonQuery();

                        SqlCommand updateNMR = new SqlCommand("UPDATE JobInstruction SET NMRStatus='Modified' WHERE Id=@Id2", cn);
                        updateNMR.Parameters.AddWithValue("@Id2", Id);
                        updateNMR.ExecuteNonQuery();

                        cn.Close();
                    }

                }
            }

            return View();
        }
        else if (set == "MaterialInfo")
        {
            if(dept=="IT"||dept=="MBD")
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        cn.Open();
                        SqlCommand command;
                        command = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET ArtworkStatus = @ArtworkStatus, PaperStock = @PaperStock, TypeCode = @TypeCode, Paper = @Paper, PaperSize= @PaperSize, Grammage =@Grammage, MaterialColour = @MaterialColour, EnvelopeStock =@EnvelopeStock, EnvelopeType = @EnvelopeType, EnvelopeSize = @EnvelopeSize, EnvelopeGrammage = @EnvelopeGrammage, EnvelopeColour = @EnvelopeColour, EnvelopeWindow = @EnvelopeWindow, EnvWindowOpaque = @EnvWindowOpaque, LabelStock = @LabelStock, LabelCutsheet = @LabelCutsheet, OthersStock = @OthersStock, BalancedMaterial = @BalancedMaterial, PlasticStock = @PlasticStock, PlasticType = @PlasticType, PlasticSize = @PlasticSize, PlasticThickness = @PlasticThickness, NMRStatus=@NMRStatus  WHERE Id =@Id", cn);
                        command.Parameters.AddWithValue("@ArtworkStatus", ArtworkStatus);
                        command.Parameters.AddWithValue("@PaperStock", PaperStock);
                        command.Parameters.AddWithValue("@TypeCode", TypeCode);
                        if (!string.IsNullOrEmpty(Paper))
                        {
                            command.Parameters.AddWithValue("@Paper", Paper);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Paper", DBNull.Value);
                        }

                        command.Parameters.AddWithValue("@NMRStatus", "Modified");


                        if (!string.IsNullOrEmpty(PaperSize))
                        {
                            command.Parameters.AddWithValue("@PaperSize", PaperSize);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PaperSize", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(Grammage))
                        {
                            command.Parameters.AddWithValue("@Grammage", Grammage);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Grammage", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(MaterialColour))
                        {
                            command.Parameters.AddWithValue("@MaterialColour", MaterialColour);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@MaterialColour", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(EnvelopeStock))
                        {
                            command.Parameters.AddWithValue("@EnvelopeStock", EnvelopeStock);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@EnvelopeStock", DBNull.Value);

                        }
                        if (!string.IsNullOrEmpty(EnvelopeType))
                        {
                            command.Parameters.AddWithValue("@EnvelopeType", EnvelopeType);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@EnvelopeType", DBNull.Value);

                        }
                        if (!string.IsNullOrEmpty(EnvelopeSize))
                        {
                            command.Parameters.AddWithValue("@EnvelopeSize", EnvelopeSize);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@EnvelopeSize", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(EnvelopeGrammage))
                        {
                            command.Parameters.AddWithValue("@EnvelopeGrammage", EnvelopeGrammage);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@EnvelopeGrammage", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(EnvelopeColour))
                        {
                            command.Parameters.AddWithValue("@EnvelopeColour", EnvelopeColour);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@EnvelopeColour", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(EnvelopeColour))
                        {
                            command.Parameters.AddWithValue("@EnvelopeWindow", EnvelopeWindow);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@EnvelopeWindow", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(EnvWindowOpaque))
                        {
                            command.Parameters.AddWithValue("@EnvWindowOpaque", EnvWindowOpaque);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@EnvWindowOpaque", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(LabelStock))
                        {
                            command.Parameters.AddWithValue("@LabelStock", LabelStock);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LabelStock", DBNull.Value);

                        }
                        if (!string.IsNullOrEmpty(LabelCutsheet))
                        {
                            command.Parameters.AddWithValue("@LabelCutsheet", LabelCutsheet);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LabelCutsheet", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(OthersStock))
                        {
                            command.Parameters.AddWithValue("@OthersStock", OthersStock);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@OthersStock", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(BalancedMaterial))
                        {
                            command.Parameters.AddWithValue("@BalancedMaterial", BalancedMaterial);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@BalancedMaterial", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(PlasticStock))
                        {
                            command.Parameters.AddWithValue("@PlasticStock", PlasticStock);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PlasticStock", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(PlasticType))
                        {
                            command.Parameters.AddWithValue("@PlasticType", PlasticType);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PlasticType", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(PlasticSize))
                        {
                            command.Parameters.AddWithValue("@PlasticSize", PlasticSize);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PlasticSize", DBNull.Value);

                        }
                        if (!string.IsNullOrEmpty(PlasticSize))
                        {
                            command.Parameters.AddWithValue("@PlasticThickness", PlasticThickness);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PlasticThickness", DBNull.Value);
                        }
                        command.Parameters.AddWithValue("@Id", Id);
                        command.ExecuteNonQuery();

                        SqlCommand updateNMR = new SqlCommand("UPDATE JobInstruction SET NMRStatus='Modified' WHERE Id=@Id2", cn);
                        updateNMR.Parameters.AddWithValue("@Id2", Id);
                        updateNMR.ExecuteNonQuery();
                        cn.Close();
                    }
                }

            }

            return View();
        }
        else if (set == "ProductionList")
        {
            if(dept=="IT"||dept=="MBD")
            {
                if (!string.IsNullOrEmpty(Id))
                {

                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        cn.Open();
                        SqlCommand command;
                        command = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET PrintingType=@PrintingType,PrintingOrientation=@PrintingOrientation,GpoList=@GpoList,RegisterMail=@RegisterMail,OtherList=@OtherList,BaseStockType=@BaseStockType,FinishingSize=@FinishingSize,AdditionalPrintingMark=@AdditionalPrintingMark,SortingCriteria=@SortingCriteria,PrintingInstr=@PrintingInstr,SortingInstr=@SortingInstr," +
                                                 "Letter=@Letter,Brochures_Leaflets=@Brochures_Leaflets,ReplyEnvelope=@ReplyEnvelope,ImgOnStatement=@ImgOnStatement,Booklet=@Booklet, NMRStatus=@NMRStatus WHERE Id=@Id", cn);

                        if (!string.IsNullOrEmpty(PrintingType))
                        {
                            command.Parameters.AddWithValue("@PrintingType", PrintingType);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PrintingType", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(PrintingOrientation))
                        {
                            command.Parameters.AddWithValue("@PrintingOrientation", PrintingOrientation);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PrintingOrientation", DBNull.Value);
                        }


                        if (GpoList == "on")
                        {
                            command.Parameters.AddWithValue("@GpoList", true);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@GpoList", false);
                        }
                        if (RegisterMail == "on")
                        {
                            command.Parameters.AddWithValue("@RegisterMail", true);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@RegisterMail", false);
                        }

                        if (!string.IsNullOrEmpty(OtherList))
                        {
                            command.Parameters.AddWithValue("@OtherList", OtherList);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@OtherList", DBNull.Value);

                        }

                        command.Parameters.AddWithValue("@NMRStatus", "Modified");

                        if (!string.IsNullOrEmpty(BaseStockType))
                        {
                            command.Parameters.AddWithValue("@BaseStockType", BaseStockType);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@BaseStockType", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(FinishingSize))
                        {
                            command.Parameters.AddWithValue("@FinishingSize", FinishingSize);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@FinishingSize", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(AdditionalPrintingMark))
                        {
                            command.Parameters.AddWithValue("@AdditionalPrintingMark", AdditionalPrintingMark);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@AdditionalPrintingMark", DBNull.Value);
                        }


                        if (!string.IsNullOrEmpty(SortingCriteria))
                        {

                            command.Parameters.AddWithValue("@SortingCriteria", false);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@SortingCriteria", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(PrintingInstr))
                        {

                            command.Parameters.AddWithValue("@PrintingInstr", PrintingInstr);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PrintingInstr", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(SortingInstr))
                        {
                            command.Parameters.AddWithValue("@SortingInstr", SortingInstr);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@SortingInstr", DBNull.Value);
                        }
                        if (Letter == "on")
                        {
                            command.Parameters.AddWithValue("@Letter", true);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Letter", false);
                        }
                        if (Brochures_Leaflets == "on")
                        {
                            command.Parameters.AddWithValue("@Brochures_Leaflets", true);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Brochures_Leaflets", false);
                        }
                        if (ReplyEnvelope == "on")
                        {
                            command.Parameters.AddWithValue("@ReplyEnvelope", true);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@ReplyEnvelope", false);
                        }
                        if (ImgOnStatement == "on")
                        {
                            command.Parameters.AddWithValue("@ImgOnStatement", true);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@ImgOnStatement", false);
                        }
                        if (Booklet == "on")
                        {
                            command.Parameters.AddWithValue("@Booklet", true);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Booklet", false);
                        }
                        command.Parameters.AddWithValue("@Id", Id);
                        command.ExecuteNonQuery();

                        SqlCommand updateNMR = new SqlCommand("UPDATE JobInstruction SET NMRStatus='Modified' WHERE Id=@Id2", cn);
                        updateNMR.Parameters.AddWithValue("@Id2", Id);
                        updateNMR.ExecuteNonQuery();

                        cn.Close();
                    }
                }

            }

            return View();
        }
        else if (set == "FinishingInst")
        {
            if(dept=="MBD"||dept=="IT")
            {
                if (!string.IsNullOrEmpty(Id) && FinishingFormat != "Please Select" && !string.IsNullOrEmpty(NumberOfInsert) && !string.IsNullOrEmpty(CommentManualType) && !string.IsNullOrEmpty(FinishingFormat) && !string.IsNullOrEmpty(FoldingType) && !string.IsNullOrEmpty(StickingOf1) && !string.IsNullOrEmpty(FinishingInst))
                {

                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        cn.Open();
                        SqlCommand command;
                        command = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET NumberOfInsert=@NumberOfInsert,Magezine1=@Magezine1,Brochure1=@Brochure1,CarrierSheet1=@CarrierSheet1,Newsletter1=@Newsletter1,Statement1=@Statement1,Booklet1=@Booklet1,CommentManualType=@CommentManualType,FinishingFormat=@FinishingFormat,FoldingType=@FoldingType,Sealing1=@Sealing1,Tearing1=@Tearing1,BarcodeLabel1=@BarcodeLabel1,Cutting1=@Cutting1,StickingOf1=@StickingOf1,AddLabel1=@AddLabel1,Sticker1=@Sticker1,Chesire1=@Chesire1,Tuck_In1=@Tuck_In1,Bursting1=@Bursting1,Sealed1=@Sealed1,Folding1=@Folding1,Unsealed1=@Unsealed1,Letter1=@Letter1,FinishingInst=@FinishingInst, NMRStatus=@NMRStatus WHERE Id=@Id", cn);
                        command.Parameters.AddWithValue("@NumberOfInsert", NumberOfInsert);
                        if (Magezine1 == "on")
                        {
                            command.Parameters.AddWithValue("@Magezine1", true);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Magezine1", false);
                        }

                        command.Parameters.AddWithValue("@NMRStatus", "Modified");


                        if (Brochure1 == "on")
                        {
                            command.Parameters.AddWithValue("@Brochure1", true);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Brochure1", false);
                        }
                        if (CarrierSheet1 == "on")
                        {
                            command.Parameters.AddWithValue("@CarrierSheet1", true);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@CarrierSheet1", false);
                        }
                        if (Newsletter1 == "on")
                        {
                            command.Parameters.AddWithValue("@Newsletter1", true);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Newsletter1", false);
                        }
                        if (Statement1 == "on")
                        {
                            command.Parameters.AddWithValue("@Statement1", true);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Statement1", false);
                        }
                        if (Booklet1 == "on")
                        {
                            command.Parameters.AddWithValue("@Booklet1", true);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Booklet1", false);
                        }
                        if (!string.IsNullOrEmpty(CommentManualType))
                        {
                            command.Parameters.AddWithValue("@CommentManualType", CommentManualType);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@CommentManualType", DBNull.Value);

                        }

                        if (!string.IsNullOrEmpty(FinishingFormat))
                        {
                            command.Parameters.AddWithValue("@FinishingFormat", FinishingFormat);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@FinishingFormat", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(FoldingType))
                        {
                            command.Parameters.AddWithValue("@FoldingType", FoldingType);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@FoldingType", DBNull.Value);
                        }

                        if (Sealing1 == "on")
                        {
                            command.Parameters.AddWithValue("@Sealing1", true);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Sealing1", false);
                        }
                        if (Tearing1 == "on")
                        {
                            command.Parameters.AddWithValue("@Tearing1", true);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Tearing1", false);
                        }
                        if (BarcodeLabel1 == "on")
                        {
                            command.Parameters.AddWithValue("@BarcodeLabel1", true);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@BarcodeLabel1", false);
                        }
                        if (Cutting1 == "on")
                        {
                            command.Parameters.AddWithValue("@Cutting1", true);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Cutting1", false);
                        }
                        if (!string.IsNullOrEmpty(StickingOf1))
                        {
                            command.Parameters.AddWithValue("@StickingOf1", StickingOf1);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@StickingOf1", DBNull.Value);
                        }


                        if (AddLabel1 == "on")
                        {
                            command.Parameters.AddWithValue("@AddLabel1", true);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@AddLabel1", false);
                        }

                        if (Sticker1 == "on")
                        {
                            command.Parameters.AddWithValue("@Sticker1", true);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Sticker1", false);
                        }


                        if (Chesire1 == "on")
                        {
                            command.Parameters.AddWithValue("@Chesire1", true);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Chesire1", false);
                        }


                        if (Tuck_In1 == "on")
                        {
                            command.Parameters.AddWithValue("@Tuck_In1", true);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Tuck_In1", false);
                        }


                        if (Bursting1 == "on")
                        {
                            command.Parameters.AddWithValue("@Bursting1", true);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Bursting1", false);
                        }


                        if (Sealed1 == "on")
                        {
                            command.Parameters.AddWithValue("@Sealed1", true);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Sealed1", false);
                        }



                        if (Folding1 == "on")
                        {
                            command.Parameters.AddWithValue("@Folding1", true);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Folding1", false);
                        }


                        if (Unsealed1 == "on")
                        {
                            command.Parameters.AddWithValue("@Unsealed1", true);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Unsealed1", false);
                        }


                        if (Letter1 == "on")
                        {
                            command.Parameters.AddWithValue("@Letter1", true);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Letter1", false);
                        }

                        if (!string.IsNullOrEmpty(FinishingInst))
                        {
                            command.Parameters.AddWithValue("@FinishingInst", FinishingInst);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@FinishingInst", DBNull.Value);
                        }
                        command.Parameters.AddWithValue("@Id", Id);
                        command.ExecuteNonQuery();

                        SqlCommand updateNMR = new SqlCommand("UPDATE JobInstruction SET NMRStatus='Modified' WHERE Id=@Id2", cn);
                        updateNMR.Parameters.AddWithValue("@Id2", Id);
                        updateNMR.ExecuteNonQuery();
                        cn.Close();
                    }
                }

            }

            return View();
        }
        else if (set == "ImportantNotes")
        {
            if(dept=="MBD"||dept=="IT")
            {
                if (!string.IsNullOrEmpty(Id))
                {

                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        cn.Open();
                        SqlCommand command;
                        command = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET IT_SysNotes=@IT_SysNotes, Produc_PlanningNotes=@Produc_PlanningNotes, PurchasingNotes=@PurchasingNotes,  EngineeringNotes=@EngineeringNotes, ArtworkNotes=@ArtworkNotes, Acc_BillingNotes=@Acc_BillingNotes, DCPNotes=@DCPNotes, PostingInfo=@PostingInfo, NMRStatus=@NMRStatus WHERE Id=@Id", cn);
                        command.Parameters.AddWithValue("@IT_SysNotes", IT_SysNotes);
                        command.Parameters.AddWithValue("@Produc_PlanningNotes", Produc_PlanningNotes);
                        command.Parameters.AddWithValue("@PurchasingNotes", PurchasingNotes);
                        command.Parameters.AddWithValue("@EngineeringNotes", EngineeringNotes);
                        command.Parameters.AddWithValue("@NMRStatus", "Modified");

                        if (!string.IsNullOrEmpty(ArtworkNotes))
                        {
                            command.Parameters.AddWithValue("@ArtworkNotes", ArtworkNotes);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@ArtworkNotes", DBNull.Value);
                        }


                        if (!string.IsNullOrEmpty(Acc_BillingNotes))
                        {
                            command.Parameters.AddWithValue("@Acc_BillingNotes", Acc_BillingNotes);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Acc_BillingNotes", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(DCPNotes))
                        {
                            command.Parameters.AddWithValue("@DCPNotes", DCPNotes);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@DCPNotes", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(PostingInfo))
                        {
                            command.Parameters.AddWithValue("@PostingInfo", PostingInfo);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PostingInfo", DBNull.Value);
                        }
                        command.Parameters.AddWithValue("@Id", Id);
                        command.ExecuteNonQuery();
                        cn.Close();
                    }
                }

            }

        }
        else if (set == "AddRates")
        {
            if(dept=="MBD"||dept=="IT")
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        cn.Open();
                        SqlCommand command;
                        command = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET RTMix=@RTMix, RTCourierChanges=@RTCourierChanges, RTChargeFranking=@RTChargeFranking, RTSelfMailer=@RTSelfMailer, RTPostage=@RTPostage, RTDeliveryCharges=@RTDeliveryCharges, RTImprest=@RTImprest, RTFranking=@RTFranking, Other1=@Other1, Other2=@Other2, Other3=@Other3, Other4=@Other4, Other5=@Other5, RTServiceChanges=@RTServiceChanges, NMRStatus=@NMRStatus WHERE Id =@Id", cn);
                        command.Parameters.AddWithValue("@RTMix", RTMix);
                        command.Parameters.AddWithValue("@RTCourierChanges", RTCourierChanges);
                        command.Parameters.AddWithValue("@RTChargeFranking", RTChargeFranking);
                        command.Parameters.AddWithValue("@RTSelfMailer", RTSelfMailer);
                        command.Parameters.AddWithValue("@RTPostage", RTPostage);
                        command.Parameters.AddWithValue("@RTDeliveryCharges", RTDeliveryCharges);
                        command.Parameters.AddWithValue("@RTFranking", RTFranking);
                        command.Parameters.AddWithValue("@RTImprest", RTImprest);
                        command.Parameters.AddWithValue("@Other1", Other1);
                        command.Parameters.AddWithValue("@Other2", Other2);
                        command.Parameters.AddWithValue("@Other3", Other3);
                        command.Parameters.AddWithValue("@Other4", Other4);
                        command.Parameters.AddWithValue("@Other5", Other5);
                        command.Parameters.AddWithValue("@RTServiceChanges", RTServiceChanges);

                        command.Parameters.AddWithValue("@NMRStatus", "Modified");


                        command.Parameters.AddWithValue("@Id", Id);
                        command.ExecuteNonQuery();

                        SqlCommand updateNMR = new SqlCommand("UPDATE JobInstruction SET NMRStatus='Modified' WHERE Id=@Id2", cn);
                        updateNMR.Parameters.AddWithValue("@Id2", Id);
                        updateNMR.ExecuteNonQuery();

                        cn.Close();
                    }
                }

            }

            return View();
        }
        else if (set == "PrintInsert")
        {
            if(dept=="MBD"||dept=="IT")
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        cn.Open();
                        SqlCommand command;
                        command = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET PrintingDuplex=@PrintingDuplex, PrintingDuplex2=@PrintingDuplex2, Inserting1=@Inserting1, Inserting2=@Inserting2, Inserting3=@Inserting3, Inserting4=@Inserting4, BrochureInsert=@BrochureInsert, MailBelow_5K=@MailBelow_5K, Handling=@Handling, PI_Sealing=@PI_Sealing, PI_Tearing=@PI_Tearing, PI_Folding=@PI_Folding, Sticking=@Sticking, Matching=@Matching, CDArchiving=@CDArchiving, Npc=@Npc, PI_EnvelopeType=@PI_EnvelopeType, PI_EnvelopePrice=@PI_EnvelopePrice, PI_Paper=@PI_Paper, PI_PaperPrice=@PI_PaperPrice, LBPrintingDuplex=@LBPrintingDuplex, LBPrintingDuplex2=@LBPrintingDuplex2, LBInserting1=@LBInserting1, LBInserting2=@LBInserting2, LBInserting3=@LBInserting3, LBInserting4=@LBInserting4, LBBrochureInsert=@LBBrochureInsert, LBMailBelow_5K=@LBMailBelow_5K, LBHandling=@LBHandling, LBPI_Sealing=@LBPI_Sealing, LBPI_Tearing=@LBPI_Tearing, LBPI_Folding=@LBPI_Folding, LBSticking=@LBSticking, LBMatching=@LBMatching, LBCDArchiving=@LBCDArchiving, LBNpc=@LBNpc, LBPI_EnvelopeType=@LBPI_EnvelopeType, LBPI_EnvelopePrice=@LBPI_EnvelopePrice, LBPI_Paper=@LBPI_Paper, LBPI_PaperPrice=@LBPI_PaperPrice, NMRStatus=@NMRStatus WHERE Id =@Id", cn);

                        command.Parameters.AddWithValue("@NMRStatus", "Modified");


                        if (!string.IsNullOrEmpty(PrintingDuplex))
                        {
                            command.Parameters.AddWithValue("@PrintingDuplex", PrintingDuplex);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PrintingDuplex", DBNull.Value);

                        }
                        if (!string.IsNullOrEmpty(PrintingDuplex2))
                        {
                            command.Parameters.AddWithValue("@PrintingDuplex2", PrintingDuplex2);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PrintingDuplex2", DBNull.Value);

                        }
                        if (!string.IsNullOrEmpty(Inserting1))
                        {
                            command.Parameters.AddWithValue("@Inserting1", Inserting1);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Inserting1", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(Inserting2))
                        {
                            command.Parameters.AddWithValue("@Inserting2", Inserting2);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Inserting2", DBNull.Value);

                        }
                        if (!string.IsNullOrEmpty(Inserting3))
                        {
                            command.Parameters.AddWithValue("@Inserting3", Inserting3);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Inserting3", DBNull.Value);

                        }
                        if (!string.IsNullOrEmpty(Inserting4))
                        {
                            command.Parameters.AddWithValue("@Inserting4", Inserting4);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Inserting4", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(BrochureInsert))
                        {
                            command.Parameters.AddWithValue("@BrochureInsert", BrochureInsert);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@BrochureInsert", DBNull.Value);

                        }
                        if (!string.IsNullOrEmpty(MailBelow_5K))
                        {
                            command.Parameters.AddWithValue("@MailBelow_5K", MailBelow_5K);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@MailBelow_5K", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(Handling))
                        {
                            command.Parameters.AddWithValue("@Handling", Handling);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Handling", DBNull.Value);

                        }
                        if (!string.IsNullOrEmpty(PI_Sealing))
                        {
                            command.Parameters.AddWithValue("@PI_Sealing", PI_Sealing);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PI_Sealing", DBNull.Value);

                        }
                        if (!string.IsNullOrEmpty(PI_Tearing))
                        {
                            command.Parameters.AddWithValue("@PI_Tearing", PI_Tearing);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PI_Tearing", DBNull.Value);

                        }
                        if (!string.IsNullOrEmpty(PI_Folding))
                        {
                            command.Parameters.AddWithValue("@PI_Folding", PI_Folding);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PI_Folding", DBNull.Value);

                        }
                        if (!string.IsNullOrEmpty(Sticking))
                        {
                            command.Parameters.AddWithValue("@Sticking", Sticking);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Sticking", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(Labelling))
                        {
                            command.Parameters.AddWithValue("@Labelling", Labelling);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Labelling", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(Matching))
                        {
                            command.Parameters.AddWithValue("@Matching", Matching);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Matching", DBNull.Value);

                        }
                        if (!string.IsNullOrEmpty(CDArchiving))
                        {
                            command.Parameters.AddWithValue("@CDArchiving", CDArchiving);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@CDArchiving", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(Npc))
                        {
                            command.Parameters.AddWithValue("@Npc", Npc);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Npc", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(PI_EnvelopeType))
                        {
                            command.Parameters.AddWithValue("@PI_EnvelopeType", PI_EnvelopeType);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PI_EnvelopeType", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(PI_EnvelopePrice))
                        {
                            command.Parameters.AddWithValue("@PI_EnvelopePrice", PI_EnvelopePrice);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PI_EnvelopePrice", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(PI_Paper))
                        {
                            command.Parameters.AddWithValue("@PI_Paper", PI_Paper);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PI_Paper", DBNull.Value);

                        }
                        if (!string.IsNullOrEmpty(PI_PaperPrice))
                        {
                            command.Parameters.AddWithValue("@PI_PaperPrice", PI_PaperPrice);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PI_PaperPrice", DBNull.Value);
                        }



                        //Label
                        if (!string.IsNullOrEmpty(LBPrintingDuplex))
                        {
                            command.Parameters.AddWithValue("@LBPrintingDuplex", LBPrintingDuplex);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBPrintingDuplex", "Printing Duplex");

                        }
                        if (!string.IsNullOrEmpty(LBPrintingDuplex2))
                        {
                            command.Parameters.AddWithValue("@LBPrintingDuplex2", LBPrintingDuplex2);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBPrintingDuplex2", "Printing Duplex2");

                        }
                        if (!string.IsNullOrEmpty(LBInserting1))
                        {
                            command.Parameters.AddWithValue("@LBInserting1", LBInserting1);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBInserting1", "Inserting1");
                        }

                        if (!string.IsNullOrEmpty(LBInserting2))
                        {
                            command.Parameters.AddWithValue("@LBInserting2", LBInserting2);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBInserting2", "Inserting2");

                        }
                        if (!string.IsNullOrEmpty(LBInserting3))
                        {
                            command.Parameters.AddWithValue("@LBInserting3", LBInserting3);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBInserting3", "Inserting3");

                        }
                        if (!string.IsNullOrEmpty(LBInserting4))
                        {
                            command.Parameters.AddWithValue("@LBInserting4", LBInserting4);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBInserting4", "Inserting4");
                        }

                        if (!string.IsNullOrEmpty(LBBrochureInsert))
                        {
                            command.Parameters.AddWithValue("@LBBrochureInsert", LBBrochureInsert);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBBrochureInsert", "Brochure Insert");

                        }
                        if (!string.IsNullOrEmpty(LBMailBelow_5K))
                        {
                            command.Parameters.AddWithValue("@LBMailBelow_5K", LBMailBelow_5K);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBMailBelow_5K", "MailBelow 5K");
                        }
                        if (!string.IsNullOrEmpty(LBHandling))
                        {
                            command.Parameters.AddWithValue("@LBHandling", LBHandling);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBHandling", "Handling");

                        }
                        if (!string.IsNullOrEmpty(LBPI_Sealing))
                        {
                            command.Parameters.AddWithValue("@LBPI_Sealing", LBPI_Sealing);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBPI_Sealing", "Sealing");

                        }
                        if (!string.IsNullOrEmpty(LBPI_Tearing))
                        {
                            command.Parameters.AddWithValue("@LBPI_Tearing", LBPI_Tearing);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBPI_Tearing", "Tearing");

                        }
                        if (!string.IsNullOrEmpty(LBPI_Folding))
                        {
                            command.Parameters.AddWithValue("@LBPI_Folding", LBPI_Folding);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBPI_Folding", "Folding");

                        }
                        if (!string.IsNullOrEmpty(LBSticking))
                        {
                            command.Parameters.AddWithValue("@LBSticking", LBSticking);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBSticking", "Sticking");
                        }

                        if (!string.IsNullOrEmpty(LBSticking))
                        {
                            command.Parameters.AddWithValue("@LBLabelling", LBLabelling);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBLabelling", "Labelling");
                        }
                        if (!string.IsNullOrEmpty(LBMatching))
                        {
                            command.Parameters.AddWithValue("@LBMatching", LBMatching);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBMatching", "Matching");

                        }
                        if (!string.IsNullOrEmpty(LBCDArchiving))
                        {
                            command.Parameters.AddWithValue("@LBCDArchiving", LBCDArchiving);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBCDArchiving", "CD Archiving");
                        }
                        if (!string.IsNullOrEmpty(LBNpc))
                        {
                            command.Parameters.AddWithValue("@LBNpc", LBNpc);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBNpc", "Npc");
                        }
                        if (!string.IsNullOrEmpty(LBPI_EnvelopeType))
                        {
                            command.Parameters.AddWithValue("@LBPI_EnvelopeType", LBPI_EnvelopeType);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBPI_EnvelopeType", "Envelope Type");
                        }
                        if (!string.IsNullOrEmpty(LBPI_EnvelopePrice))
                        {
                            command.Parameters.AddWithValue("@LBPI_EnvelopePrice", LBPI_EnvelopePrice);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBPI_EnvelopePrice", "Envelope Price");
                        }
                        if (!string.IsNullOrEmpty(LBPI_Paper))
                        {
                            command.Parameters.AddWithValue("@LBPI_Paper", LBPI_Paper);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBPI_Paper", "PI_Paper");

                        }
                        if (!string.IsNullOrEmpty(LBPI_PaperPrice))
                        {
                            command.Parameters.AddWithValue("@LBPI_PaperPrice", LBPI_PaperPrice);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBPI_PaperPrice", "Paper Price");
                        }
                        command.Parameters.AddWithValue("@Id", Id);
                        command.ExecuteNonQuery();

                        SqlCommand updateNMR = new SqlCommand("UPDATE JobInstruction SET NMRStatus='Modified' WHERE Id=@Id2", cn);
                        updateNMR.Parameters.AddWithValue("@Id2", Id);
                        updateNMR.ExecuteNonQuery();

                        cn.Close();
                    }
                }

            }

        }
        else if (set == "SelfMailer")
        {
            if(dept=="MBD"||dept=="IT")
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        cn.Open();
                        SqlCommand command;
                        command = new SqlCommand("UPDATE[IflowSeed].[dbo].[JobInstruction] SET SM_PrintingSM = @SM_PrintingSM, SM_SM_Material = @SM_SM_Material, SM_MailBelow_5K = @SM_MailBelow_5K, SM_Handling = @SM_Handling, SM_Paper = @SM_Paper, SM_PaperPrice = @SM_PaperPrice, SM_Paper2 = @SM_Paper2, SM_PaperPrice2 = @SM_PaperPrice2, LBSM_PrintingSM = @LBSM_PrintingSM, LBSM_SM_Material = @LBSM_SM_Material, LBSM_MailBelow_5K = @LBSM_MailBelow_5K, LBSM_Handling = @LBSM_Handling, LBSM_Paper = @LBSM_Paper, LBSM_PaperPrice = @LBSM_PaperPrice, LBSM_Paper2 = @LBSM_Paper2, LBSM_PaperPrice2 = @LBSM_PaperPrice2, NMRStatus=@NMRStatus WHERE Id = @Id", cn);
                        if (!string.IsNullOrEmpty(SM_PrintingSM))
                        {
                            command.Parameters.AddWithValue("@SM_PrintingSM", SM_PrintingSM);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@SM_PrintingSM", DBNull.Value);
                        }

                        command.Parameters.AddWithValue("@NMRStatus", "Modified");

                        if (!string.IsNullOrEmpty(SM_SM_Material))
                        {
                            command.Parameters.AddWithValue("@SM_SM_Material", SM_SM_Material);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@SM_SM_Material", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(SM_MailBelow_5K))
                        {
                            command.Parameters.AddWithValue("@SM_MailBelow_5K", SM_MailBelow_5K);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@SM_MailBelow_5K", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(SM_Handling))
                        {
                            command.Parameters.AddWithValue("@SM_Handling", SM_Handling);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@SM_Handling", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(SM_Paper))
                        {
                            command.Parameters.AddWithValue("@SM_Paper", SM_Paper);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@SM_Paper", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(SM_PaperPrice))
                        {
                            command.Parameters.AddWithValue("@SM_PaperPrice", SM_PaperPrice);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@SM_PaperPrice", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(SM_Paper2))
                        {
                            command.Parameters.AddWithValue("@SM_Paper2", SM_Paper2);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@SM_Paper2", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(SM_PaperPrice2))
                        {
                            command.Parameters.AddWithValue("@SM_PaperPrice2", SM_PaperPrice2);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@SM_PaperPrice2", DBNull.Value);
                        }



                        //LABEL
                        if (!string.IsNullOrEmpty(LBSM_PrintingSM))
                        {
                            command.Parameters.AddWithValue("@LBSM_PrintingSM", LBSM_PrintingSM);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBSM_PrintingSM", "Printing Self Mailer");
                        }
                        if (!string.IsNullOrEmpty(LBSM_SM_Material))
                        {
                            command.Parameters.AddWithValue("@LBSM_SM_Material", LBSM_SM_Material);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBSM_SM_Material", "Self Mailer & Material");
                        }

                        if (!string.IsNullOrEmpty(LBSM_MailBelow_5K))
                        {
                            command.Parameters.AddWithValue("@LBSM_MailBelow_5K", LBSM_MailBelow_5K);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBSM_MailBelow_5K", "Mail Below 5K");
                        }

                        if (!string.IsNullOrEmpty(LBSM_Handling))
                        {
                            command.Parameters.AddWithValue("@LBSM_Handling", LBSM_Handling);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBSM_Handling", "Handling");
                        }
                        if (!string.IsNullOrEmpty(LBSM_Paper))
                        {
                            command.Parameters.AddWithValue("@LBSM_Paper", LBSM_Paper);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBSM_Paper", "Paper");
                        }
                        if (!string.IsNullOrEmpty(LBSM_PaperPrice))
                        {
                            command.Parameters.AddWithValue("@LBSM_PaperPrice", LBSM_PaperPrice);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBSM_PaperPrice", "Paper Price");
                        }
                        if (!string.IsNullOrEmpty(LBSM_Paper2))
                        {
                            command.Parameters.AddWithValue("@LBSM_Paper2", LBSM_Paper2);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBSM_Paper2", "Paper2");
                        }
                        if (!string.IsNullOrEmpty(LBSM_PaperPrice2))
                        {
                            command.Parameters.AddWithValue("@LBSM_PaperPrice2", LBSM_PaperPrice2);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBSM_PaperPrice2", "Paper Price2");
                        }
                        command.Parameters.AddWithValue("@Id", Id);
                        command.ExecuteNonQuery();

                        SqlCommand updateNMR = new SqlCommand("UPDATE JobInstruction SET NMRStatus='Modified' WHERE Id=@Id2", cn);
                        updateNMR.Parameters.AddWithValue("@Id2", Id);
                        updateNMR.ExecuteNonQuery();

                        cn.Close();
                    }
                }

            }
        }
        else if (set == "MMP")
        {
            if(dept=="MBD"||dept=="IT")
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        cn.Open();
                        SqlCommand command;
                        command = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET MMP_PrintingDuplex=@MMP_PrintingDuplex, MMP_InsertingMMP=@MMP_InsertingMMP ,MMP_FirstInsert=@MMP_FirstInsert, MMP_SecondInsert=@MMP_SecondInsert,MMP_BrochureInsert=@MMP_BrochureInsert, MMP_MailBelow_5K=@MMP_MailBelow_5K, MMP_Handling=@MMP_Handling, MMP_Sealing=@MMP_Sealing, MMP_Tearing=@MMP_Tearing ,MMP_Folding=@MMP_Folding, MMP_Sticking=@MMP_Sticking, MMP_Labelling=@MMP_Labelling, MMP_Matching=@MMP_Matching, MMP_CDArchiving=@MMP_CDArchiving, MMP_EnvelopeType=@MMP_EnvelopeType, MMP_EnvelopePrice=@MMP_EnvelopePrice, MMP_Paper=@MMP_Paper,MMP_PaperPrice=@MMP_PaperPrice, LBMMP_PrintingDuplex=@LBMMP_PrintingDuplex, LBMMP_InsertingMMP=@LBMMP_InsertingMMP ,LBMMP_FirstInsert=@LBMMP_FirstInsert, LBMMP_SecondInsert=@LBMMP_SecondInsert,LBMMP_BrochureInsert=@LBMMP_BrochureInsert, LBMMP_MailBelow_5K=@LBMMP_MailBelow_5K, LBMMP_Handling=@LBMMP_Handling, LBMMP_Sealing=@LBMMP_Sealing, LBMMP_Tearing=@LBMMP_Tearing ,LBMMP_Folding=@LBMMP_Folding, LBMMP_Sticking=@LBMMP_Sticking, LBMMP_Labelling=@LBMMP_Labelling, LBMMP_Matching=@LBMMP_Matching, LBMMP_CDArchiving=@LBMMP_CDArchiving, LBMMP_EnvelopeType=@LBMMP_EnvelopeType, LBMMP_EnvelopePrice=@LBMMP_EnvelopePrice, LBMMP_Paper=@LBMMP_Paper,LBMMP_PaperPrice=@LBMMP_PaperPrice, NMRStatus=@NMRStatus WHERE Id =@Id", cn);

                        command.Parameters.AddWithValue("@NMRStatus", "Modified");


                        if (!string.IsNullOrEmpty(MMP_PrintingDuplex))
                        {
                            command.Parameters.AddWithValue("@MMP_PrintingDuplex", MMP_PrintingDuplex);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@MMP_PrintingDuplex", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(MMP_InsertingMMP))
                        {
                            command.Parameters.AddWithValue("@MMP_InsertingMMP", MMP_InsertingMMP);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@MMP_InsertingMMP", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(MMP_FirstInsert))
                        {
                            command.Parameters.AddWithValue("@MMP_FirstInsert", MMP_FirstInsert);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@MMP_FirstInsert", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(MMP_SecondInsert))
                        {
                            command.Parameters.AddWithValue("@MMP_SecondInsert", MMP_SecondInsert);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@MMP_SecondInsert", DBNull.Value);

                        }
                        if (!string.IsNullOrEmpty(MMP_BrochureInsert))
                        {
                            command.Parameters.AddWithValue("@MMP_BrochureInsert", MMP_BrochureInsert);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@MMP_BrochureInsert", DBNull.Value);

                        }
                        if (!string.IsNullOrEmpty(MMP_MailBelow_5K))
                        {
                            command.Parameters.AddWithValue("@MMP_MailBelow_5K", MMP_MailBelow_5K);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@MMP_MailBelow_5K", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(MMP_Handling))
                        {
                            command.Parameters.AddWithValue("@MMP_Handling", MMP_Handling);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@MMP_Handling", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(MMP_Sealing))
                        {
                            command.Parameters.AddWithValue("@MMP_Sealing", MMP_Sealing);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@MMP_Sealing", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(MMP_Tearing))
                        {
                            command.Parameters.AddWithValue("@MMP_Tearing", MMP_Tearing);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@MMP_Tearing", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(MMP_Folding))
                        {
                            command.Parameters.AddWithValue("@MMP_Folding", MMP_Folding);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@MMP_Folding", DBNull.Value);

                        }
                        if (!string.IsNullOrEmpty(MMP_Sticking))
                        {
                            command.Parameters.AddWithValue("@MMP_Sticking", MMP_Sticking);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@MMP_Sticking", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(MMP_Labelling))
                        {
                            command.Parameters.AddWithValue("@MMP_Labelling", MMP_Labelling);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@MMP_Labelling", DBNull.Value);

                        }
                        if (!string.IsNullOrEmpty(MMP_Matching))
                        {
                            command.Parameters.AddWithValue("@MMP_Matching", MMP_Matching);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@MMP_Matching", DBNull.Value);

                        }

                        if (!string.IsNullOrEmpty(MMP_CDArchiving))
                        {
                            command.Parameters.AddWithValue("@MMP_CDArchiving", MMP_CDArchiving);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@MMP_CDArchiving", DBNull.Value);

                        }
                        if (!string.IsNullOrEmpty(MMP_EnvelopeType))
                        {
                            command.Parameters.AddWithValue("@MMP_EnvelopeType", MMP_EnvelopeType);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@MMP_EnvelopeType", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(MMP_EnvelopePrice))
                        {
                            command.Parameters.AddWithValue("@MMP_EnvelopePrice", MMP_EnvelopePrice);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@MMP_EnvelopePrice", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(MMP_Paper))
                        {
                            command.Parameters.AddWithValue("@MMP_Paper", MMP_Paper);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@MMP_Paper", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(MMP_PaperPrice))
                        {
                            command.Parameters.AddWithValue("@MMP_PaperPrice", MMP_PaperPrice);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@MMP_PaperPrice", DBNull.Value);
                        }



                        //Label
                        if (!string.IsNullOrEmpty(LBMMP_PrintingDuplex))
                        {
                            command.Parameters.AddWithValue("@LBMMP_PrintingDuplex", LBMMP_PrintingDuplex);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBMMP_PrintingDuplex", "Printing Duplex");
                        }
                        if (!string.IsNullOrEmpty(LBMMP_InsertingMMP))
                        {
                            command.Parameters.AddWithValue("@LBMMP_InsertingMMP", LBMMP_InsertingMMP);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBMMP_InsertingMMP", "Printing Duplex");
                        }
                        if (!string.IsNullOrEmpty(LBMMP_FirstInsert))
                        {
                            command.Parameters.AddWithValue("@LBMMP_FirstInsert", LBMMP_FirstInsert);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBMMP_FirstInsert", "First Insert");
                        }
                        if (!string.IsNullOrEmpty(LBMMP_SecondInsert))
                        {
                            command.Parameters.AddWithValue("@LBMMP_SecondInsert", LBMMP_SecondInsert);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBMMP_SecondInsert", "Second Insert");

                        }
                        if (!string.IsNullOrEmpty(LBMMP_BrochureInsert))
                        {
                            command.Parameters.AddWithValue("@LBMMP_BrochureInsert", LBMMP_BrochureInsert);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBMMP_BrochureInsert", "Brochure Insert");

                        }
                        if (!string.IsNullOrEmpty(LBMMP_MailBelow_5K))
                        {
                            command.Parameters.AddWithValue("@LBMMP_MailBelow_5K", LBMMP_MailBelow_5K);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBMMP_MailBelow_5K", "MailBelow 5K");
                        }
                        if (!string.IsNullOrEmpty(LBMMP_Handling))
                        {
                            command.Parameters.AddWithValue("@LBMMP_Handling", LBMMP_Handling);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBMMP_Handling", "Handling");
                        }
                        if (!string.IsNullOrEmpty(LBMMP_Sealing))
                        {
                            command.Parameters.AddWithValue("@LBMMP_Sealing", LBMMP_Sealing);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBMMP_Sealing", "Sealing");
                        }
                        if (!string.IsNullOrEmpty(LBMMP_Tearing))
                        {
                            command.Parameters.AddWithValue("@LBMMP_Tearing", LBMMP_Tearing);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBMMP_Tearing", "Tearing");
                        }
                        if (!string.IsNullOrEmpty(LBMMP_Folding))
                        {
                            command.Parameters.AddWithValue("@LBMMP_Folding", LBMMP_Folding);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBMMP_Folding", "LBFolding");

                        }
                        if (!string.IsNullOrEmpty(LBMMP_Sticking))
                        {
                            command.Parameters.AddWithValue("@LBMMP_Sticking", LBMMP_Sticking);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBMMP_Sticking", "LBSticking");
                        }

                        if (!string.IsNullOrEmpty(LBMMP_Labelling))
                        {
                            command.Parameters.AddWithValue("@LBMMP_Labelling", LBMMP_Labelling);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBMMP_Labelling", "LBLabelling");

                        }
                        if (!string.IsNullOrEmpty(LBMMP_Matching))
                        {
                            command.Parameters.AddWithValue("@LBMMP_Matching", LBMMP_Matching);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBMMP_Matching", "Matching");

                        }

                        if (!string.IsNullOrEmpty(LBMMP_CDArchiving))
                        {
                            command.Parameters.AddWithValue("@LBMMP_CDArchiving", LBMMP_CDArchiving);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBMMP_CDArchiving", "CD Archiving");

                        }
                        if (!string.IsNullOrEmpty(LBMMP_EnvelopeType))
                        {
                            command.Parameters.AddWithValue("@LBMMP_EnvelopeType", LBMMP_EnvelopeType);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBMMP_EnvelopeType", "Envelope Type");
                        }
                        if (!string.IsNullOrEmpty(LBMMP_EnvelopePrice))
                        {
                            command.Parameters.AddWithValue("@LBMMP_EnvelopePrice", LBMMP_EnvelopePrice);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBMMP_EnvelopePrice", "Envelope Price");
                        }
                        if (!string.IsNullOrEmpty(LBMMP_Paper))
                        {
                            command.Parameters.AddWithValue("@LBMMP_Paper", LBMMP_Paper);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBMMP_Paper", "Paper");
                        }
                        if (!string.IsNullOrEmpty(LBMMP_PaperPrice))
                        {
                            command.Parameters.AddWithValue("@LBMMP_PaperPrice", LBMMP_PaperPrice);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBMMP_PaperPrice", "Paper Price");
                        }

                        command.Parameters.AddWithValue("@Id", Id);
                        command.ExecuteNonQuery();

                        SqlCommand updateNMR = new SqlCommand("UPDATE JobInstruction SET NMRStatus='Modified' WHERE Id=@Id2", cn);
                        updateNMR.Parameters.AddWithValue("@Id2", Id);
                        updateNMR.ExecuteNonQuery();

                        cn.Close();
                    }
                }

            }
        }

        else if (set == "DCP")
        {
            if(dept=="MBD"||dept=="IT")
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        cn.Open();
                        SqlCommand command;
                        command = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET DCP_FoldingCharges=@DCP_FoldingCharges, DCP_SupplyPrintLabel=@DCP_SupplyPrintLabel, DCP_PrintingDuplex=@DCP_PrintingDuplex, DCP_FirstInsert=@DCP_FirstInsert, DCP_SecondInsert=@DCP_SecondInsert, DCP_BrochureInsert=@DCP_BrochureInsert, DCP_MailBelow_5K=@DCP_MailBelow_5K, DCP_Handling=@DCP_Handling, DCP_Sealing=@DCP_Sealing, DCP_Tearing=@DCP_Tearing, DCP_Folding=@DCP_Folding, DCP_Sticking=@DCP_Sticking, DCP_Labelling=@DCP_Labelling, DCP_Matching=@DCP_Matching, DCP_CDArchiving=@DCP_CDArchiving, DCP_EnvelopeType=@DCP_EnvelopeType, DCP_EnvelopePrice=@DCP_EnvelopePrice, DCP_Paper=@DCP_Paper, DCP_PaperPrice=@DCP_PaperPrice, LBDCP_FoldingCharges=@LBDCP_FoldingCharges, LBDCP_SupplyPrintLabel=@LBDCP_SupplyPrintLabel, LBDCP_PrintingDuplex=@LBDCP_PrintingDuplex, LBDCP_FirstInsert=@LBDCP_FirstInsert, LBDCP_SecondInsert=@LBDCP_SecondInsert, LBDCP_BrochureInsert=@LBDCP_BrochureInsert, LBDCP_MailBelow_5K=@LBDCP_MailBelow_5K, LBDCP_Handling=@LBDCP_Handling, LBDCP_Sealing=@LBDCP_Sealing, LBDCP_Tearing=@LBDCP_Tearing, LBDCP_Folding=@LBDCP_Folding, LBDCP_Sticking=@LBDCP_Sticking, LBDCP_Labelling=@LBDCP_Labelling, LBDCP_Matching=@LBDCP_Matching, LBDCP_CDArchiving=@LBDCP_CDArchiving, LBDCP_EnvelopeType=@LBDCP_EnvelopeType, LBDCP_EnvelopePrice=@LBDCP_EnvelopePrice, LBDCP_Paper=@LBDCP_Paper, LBDCP_PaperPrice=@LBDCP_PaperPrice, NMRStatus=@NMRStatus WHERE Id =@Id", cn);

                        if (!string.IsNullOrEmpty(DCP_FoldingCharges))
                        {
                            command.Parameters.AddWithValue("@DCP_FoldingCharges", DCP_FoldingCharges);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@DCP_FoldingCharges", DBNull.Value);
                        }

                        command.Parameters.AddWithValue("@NMRStatus", "Modified");

                        if (!string.IsNullOrEmpty(DCP_SupplyPrintLabel))
                        {
                            command.Parameters.AddWithValue("@DCP_SupplyPrintLabel", DCP_SupplyPrintLabel);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@DCP_SupplyPrintLabel", DBNull.Value);

                        }
                        if (!string.IsNullOrEmpty(DCP_PrintingDuplex))
                        {
                            command.Parameters.AddWithValue("@DCP_PrintingDuplex", DCP_PrintingDuplex);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@DCP_PrintingDuplex", DBNull.Value);

                        }

                        if (!string.IsNullOrEmpty(DCP_FirstInsert))
                        {
                            command.Parameters.AddWithValue("@DCP_FirstInsert", DCP_FirstInsert);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@DCP_FirstInsert", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(DCP_SecondInsert))
                        {
                            command.Parameters.AddWithValue("@DCP_SecondInsert", DCP_SecondInsert);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@DCP_SecondInsert", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(DCP_BrochureInsert))
                        {

                            command.Parameters.AddWithValue("@DCP_BrochureInsert", DCP_BrochureInsert);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@DCP_BrochureInsert", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(DCP_MailBelow_5K))
                        {
                            command.Parameters.AddWithValue("@DCP_MailBelow_5K", DCP_MailBelow_5K);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@DCP_MailBelow_5K", DBNull.Value);

                        }
                        if (!string.IsNullOrEmpty(DCP_Handling))
                        {
                            command.Parameters.AddWithValue("@DCP_Handling", DCP_Handling);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@DCP_Handling", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(DCP_Sealing))
                        {
                            command.Parameters.AddWithValue("@DCP_Sealing", DCP_Sealing);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@DCP_Sealing", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(DCP_Sealing))
                        {
                            command.Parameters.AddWithValue("@DCP_Tearing", DCP_Tearing);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@DCP_Tearing", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(DCP_Folding))
                        {
                            command.Parameters.AddWithValue("@DCP_Folding", DCP_Folding);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@DCP_Folding", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(DCP_Sticking))
                        {
                            command.Parameters.AddWithValue("@DCP_Sticking", DCP_Sticking);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@DCP_Sticking", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(DCP_Labelling))
                        {
                            command.Parameters.AddWithValue("@DCP_Labelling", DCP_Labelling);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@DCP_Labelling", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(DCP_Matching))
                        {
                            command.Parameters.AddWithValue("@DCP_Matching", DCP_Matching);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@DCP_Matching", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(DCP_CDArchiving))
                        {
                            command.Parameters.AddWithValue("@DCP_CDArchiving", DCP_CDArchiving);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@DCP_CDArchiving", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(DCP_EnvelopeType))
                        {

                            command.Parameters.AddWithValue("@DCP_EnvelopeType", DCP_EnvelopeType);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@DCP_EnvelopeType", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(DCP_EnvelopePrice))
                        {
                            command.Parameters.AddWithValue("@DCP_EnvelopePrice", DCP_EnvelopePrice);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@DCP_EnvelopePrice", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(DCP_EnvelopePrice))
                        {
                            command.Parameters.AddWithValue("@DCP_Paper", DCP_Paper);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@DCP_Paper", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(DCP_EnvelopePrice))
                        {
                            command.Parameters.AddWithValue("@DCP_PaperPrice", DCP_PaperPrice);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@DCP_PaperPrice", DBNull.Value);
                        }





                        //Label
                        if (!string.IsNullOrEmpty(LBDCP_FoldingCharges))
                        {
                            command.Parameters.AddWithValue("@LBDCP_FoldingCharges", LBDCP_FoldingCharges);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBDCP_FoldingCharges", "Folding/Insertion/Sealing/Handling Charges");
                        }
                        if (!string.IsNullOrEmpty(LBDCP_SupplyPrintLabel))
                        {
                            command.Parameters.AddWithValue("@LBDCP_SupplyPrintLabel", LBDCP_SupplyPrintLabel);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBDCP_SupplyPrintLabel", "Supply & Print Address label");

                        }
                        if (!string.IsNullOrEmpty(LBDCP_PrintingDuplex))
                        {
                            command.Parameters.AddWithValue("@LBDCP_PrintingDuplex", LBDCP_PrintingDuplex);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBDCP_PrintingDuplex", "PrintingDuplex");

                        }

                        if (!string.IsNullOrEmpty(LBDCP_FirstInsert))
                        {
                            command.Parameters.AddWithValue("@LBDCP_FirstInsert", LBDCP_FirstInsert);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBDCP_FirstInsert", "First Insert");
                        }
                        if (!string.IsNullOrEmpty(LBDCP_SecondInsert))
                        {
                            command.Parameters.AddWithValue("@LBDCP_SecondInsert", LBDCP_SecondInsert);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBDCP_SecondInsert", "Second Insert");
                        }
                        if (!string.IsNullOrEmpty(LBDCP_BrochureInsert))
                        {

                            command.Parameters.AddWithValue("@LBDCP_BrochureInsert", LBDCP_BrochureInsert);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBDCP_BrochureInsert", "Brochure Insert");
                        }
                        if (!string.IsNullOrEmpty(LBDCP_MailBelow_5K))
                        {
                            command.Parameters.AddWithValue("@LBDCP_MailBelow_5K", LBDCP_MailBelow_5K);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBDCP_MailBelow_5K", "MailBelow 5K");

                        }
                        if (!string.IsNullOrEmpty(LBDCP_Handling))
                        {
                            command.Parameters.AddWithValue("@LBDCP_Handling", LBDCP_Handling);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBDCP_Handling", "Handling");
                        }
                        if (!string.IsNullOrEmpty(LBDCP_Sealing))
                        {
                            command.Parameters.AddWithValue("@LBDCP_Sealing", LBDCP_Sealing);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBDCP_Sealing", "Sealing");
                        }
                        if (!string.IsNullOrEmpty(LBDCP_Sealing))
                        {
                            command.Parameters.AddWithValue("@LBDCP_Tearing", LBDCP_Tearing);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBDCP_Tearing", "Tearing");
                        }
                        if (!string.IsNullOrEmpty(LBDCP_Folding))
                        {
                            command.Parameters.AddWithValue("@LBDCP_Folding", LBDCP_Folding);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBDCP_Folding", "Folding");
                        }
                        if (!string.IsNullOrEmpty(LBDCP_Sticking))
                        {
                            command.Parameters.AddWithValue("@LBDCP_Sticking", LBDCP_Sticking);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBDCP_Sticking", "Sticking");
                        }
                        if (!string.IsNullOrEmpty(LBDCP_Labelling))
                        {
                            command.Parameters.AddWithValue("@LBDCP_Labelling", LBDCP_Labelling);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBDCP_Labelling", "Labelling");
                        }
                        if (!string.IsNullOrEmpty(LBDCP_Matching))
                        {
                            command.Parameters.AddWithValue("@LBDCP_Matching", LBDCP_Matching);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBDCP_Matching", "Matching");
                        }
                        if (!string.IsNullOrEmpty(LBDCP_CDArchiving))
                        {
                            command.Parameters.AddWithValue("@LBDCP_CDArchiving", LBDCP_CDArchiving);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBDCP_CDArchiving", "CD Archiving");
                        }
                        if (!string.IsNullOrEmpty(LBDCP_EnvelopeType))
                        {

                            command.Parameters.AddWithValue("@LBDCP_EnvelopeType", LBDCP_EnvelopeType);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBDCP_EnvelopeType", "Envelope Type");
                        }
                        if (!string.IsNullOrEmpty(LBDCP_EnvelopePrice))
                        {
                            command.Parameters.AddWithValue("@LBDCP_EnvelopePrice", LBDCP_EnvelopePrice);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBDCP_EnvelopePrice", "Envelope Price");
                        }
                        if (!string.IsNullOrEmpty(LBDCP_EnvelopePrice))
                        {
                            command.Parameters.AddWithValue("@LBDCP_Paper", LBDCP_Paper);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBDCP_Paper", "Paper");
                        }

                        if (!string.IsNullOrEmpty(LBDCP_EnvelopePrice))
                        {
                            command.Parameters.AddWithValue("@LBDCP_PaperPrice", LBDCP_PaperPrice);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBDCP_PaperPrice", "Paper Price");
                        }

                        command.Parameters.AddWithValue("@Id", Id);
                        command.ExecuteNonQuery();

                        SqlCommand updateNMR = new SqlCommand("UPDATE JobInstruction SET NMRStatus='Modified' WHERE Id=@Id2", cn);
                        updateNMR.Parameters.AddWithValue("@Id2", Id);
                        updateNMR.ExecuteNonQuery();
                        cn.Close();
                    }
                }

            }

        }
        else if (set == "RegisterMail")
        {
            if(dept=="MBD"||dept=="IT")
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        cn.Open();
                        SqlCommand command;
                        command = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET RM_Printing=@RM_Printing, RM_Selfmailer=@RM_Selfmailer, RM_MailBelow_5K=@RM_MailBelow_5K, RM_Handling=@RM_Handling, RM_LabellingRegsterMails=@RM_LabellingRegsterMails, RM_Paper=@RM_Paper, RM_Paper2=@RM_Paper2, RM_PaperPrice=@RM_PaperPrice, RM_PaperPrice2=@RM_PaperPrice2, LBRM_Printing=@LBRM_Printing, LBRM_Selfmailer=@LBRM_Selfmailer, LBRM_MailBelow_5K=@LBRM_MailBelow_5K, LBRM_Handling=@LBRM_Handling, LBRM_LabellingRegsterMails=@LBRM_LabellingRegsterMails, LBRM_Paper=@LBRM_Paper, LBRM_Paper2=@LBRM_Paper2, LBRM_PaperPrice=@LBRM_PaperPrice, LBRM_PaperPrice2=@LBRM_PaperPrice2, NMRStatus=@NMRStatus WHERE Id =@Id", cn);

                        command.Parameters.AddWithValue("@NMRStatus", "Modified");


                        if (!string.IsNullOrEmpty(RM_Printing))
                        {
                            command.Parameters.AddWithValue("@RM_Printing", RM_Printing);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@RM_Printing", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(RM_Selfmailer))
                        {
                            command.Parameters.AddWithValue("@RM_Selfmailer", RM_Selfmailer);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@RM_Selfmailer", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(RM_MailBelow_5K))
                        {
                            command.Parameters.AddWithValue("@RM_MailBelow_5K", RM_MailBelow_5K);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@RM_MailBelow_5K", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(RM_Handling))
                        {
                            command.Parameters.AddWithValue("@RM_Handling", RM_Handling);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@RM_Handling", DBNull.Value);
                        }


                        if (!string.IsNullOrEmpty(RM_LabellingRegsterMails))
                        {
                            command.Parameters.AddWithValue("@RM_LabellingRegsterMails", RM_LabellingRegsterMails);

                        }
                        else
                        {

                            command.Parameters.AddWithValue("@RM_LabellingRegsterMails", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(RM_Paper))
                        {
                            command.Parameters.AddWithValue("@RM_Paper", RM_Paper);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@RM_Paper", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(RM_PaperPrice))
                        {
                            command.Parameters.AddWithValue("@RM_PaperPrice", RM_PaperPrice);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@RM_PaperPrice", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(RM_Paper2))
                        {
                            command.Parameters.AddWithValue("@RM_Paper2", RM_Paper2);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@RM_Paper2", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(RM_PaperPrice2))
                        {
                            command.Parameters.AddWithValue("@RM_PaperPrice2", RM_PaperPrice2);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@RM_PaperPrice2", DBNull.Value);
                        }



                        //Label

                        if (!string.IsNullOrEmpty(LBRM_Printing))
                        {
                            command.Parameters.AddWithValue("@LBRM_Printing", LBRM_Printing);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBRM_Printing", "Printing");
                        }
                        if (!string.IsNullOrEmpty(LBRM_Selfmailer))
                        {
                            command.Parameters.AddWithValue("@LBRM_Selfmailer", LBRM_Selfmailer);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBRM_Selfmailer", "Selfmailer");
                        }

                        if (!string.IsNullOrEmpty(LBRM_MailBelow_5K))
                        {
                            command.Parameters.AddWithValue("@LBRM_MailBelow_5K", LBRM_MailBelow_5K);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBRM_MailBelow_5K", "MailBelow 5K");
                        }
                        if (!string.IsNullOrEmpty(LBRM_Handling))
                        {
                            command.Parameters.AddWithValue("@LBRM_Handling", LBRM_Handling);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBRM_Handling", "Handling");
                        }


                        if (!string.IsNullOrEmpty(LBRM_LabellingRegsterMails))
                        {
                            command.Parameters.AddWithValue("@LBRM_LabellingRegsterMails", LBRM_LabellingRegsterMails);

                        }
                        else
                        {

                            command.Parameters.AddWithValue("@LBRM_LabellingRegsterMails", "Labelling Register Mails");
                        }

                        if (!string.IsNullOrEmpty(LBRM_Paper))
                        {
                            command.Parameters.AddWithValue("@LBRM_Paper", LBRM_Paper);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBRM_Paper", "Paper");
                        }
                        if (!string.IsNullOrEmpty(LBRM_PaperPrice))
                        {
                            command.Parameters.AddWithValue("@LBRM_PaperPrice", LBRM_PaperPrice);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBRM_PaperPrice", "Paper Price");
                        }

                        if (!string.IsNullOrEmpty(LBRM_Paper2))
                        {
                            command.Parameters.AddWithValue("@LBRM_Paper2", LBRM_Paper2);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBRM_Paper2", "Paper2");
                        }
                        if (!string.IsNullOrEmpty(LBRM_PaperPrice2))
                        {
                            command.Parameters.AddWithValue("@LBRM_PaperPrice2", LBRM_PaperPrice2);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@LBRM_PaperPrice2", "Paper Price2");
                        }

                        command.Parameters.AddWithValue("@Id", Id);
                        command.ExecuteNonQuery();

                        SqlCommand updateNMR = new SqlCommand("UPDATE JobInstruction SET NMRStatus='Modified' WHERE Id=@Id2", cn);
                        updateNMR.Parameters.AddWithValue("@Id2", Id);
                        updateNMR.ExecuteNonQuery();
                        cn.Close();
                    }
                }

            }

        }

        else
        {

            return View();
        }



        if (set == "update")
        {
            if(dept=="MBD"||dept=="IT")
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    cn.Open();
                    command.CommandText = @"SELECT Customer_Name,Cust_Department,ProductName,Status
                                     FROM [IflowSeed].[dbo].[JobInstruction] 
                                     WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", Id);
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
                                model.Cust_Department = reader.GetString(1);
                            }
                            if (reader.IsDBNull(2) == false)
                            {
                                model.ProductName = reader.GetString(2);

                            }
                            if (reader.IsDBNull(3) == false)
                            {
                                model.Status = reader.GetString(3);

                            }

                        }

                        string Str = "<html>";
                        Str += "<head>";
                        Str += "<title></title>";
                        Str += "<style type=text/css>p.MsoNormal{margin-bottom:.0001pt;font-size:11.0pt;font-family:Calibri,sans-serif; margin-left: 0cm;margin-right: 0cm;margin-top: 0cm;}.style1{ width: 246px;}.style2{width: 599px;}.style3{ width: 246px; height: 23px;}.style4 {width: 599px;height: 23px;}table, th, td {border: 1px solid black;</style>";
                        Str += "</head>";
                        Str += "<body>";
                        Str += "<p>There is an </p>";
                        Str += "</br>";
                        Str += "<table style=width:100%>";
                        Str += "<tr>";
                        Str += "<td class=style1>CUSTOMER NAME : </td>";
                        Str += "<td class=style2>" + model.Customer_Name.ToUpper() + "</td>";
                        Str += "</tr>";
                        Str += "<tr>";
                        Str += "<td class=style1>DEPARTMENT : </td>";
                        Str += "<td class=style2>" + model.Cust_Department.ToUpper() + "</td>";
                        Str += "</tr>";
                        Str += "<tr>";
                        Str += "<td class=style1>PRODUCT NAME: </td>";
                        Str += "<td class=style2>" + model.ProductName + "</td>";
                        Str += "</tr>";
                        Str += "<tr>";
                        Str += "<td class=style1>STATUS : </td>";
                        Str += "<td class=style2>" + model.Status + "</td>";
                        Str += "</tr>";
                        Str += "</table>";
                        Str += "</body>";
                        Str += "</html>";

                        bool isEmailSendSuccessfully = false;

                        try
                        {
                            // mailer.Send(mailMessage);
                            string smtpServer = IpSMtp_;
                            //string userName = "m.rizalramli@intercity.com.my";
                            //string password = "Abcd123$";
                            int cdoBasic = 1;
                            int cdoSendUsingPort = 2;
                            System.Web.Mail.MailMessage msg = new System.Web.Mail.MailMessage();
                            msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", smtpServer);
                            msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", PortSmtp_);
                            msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", cdoSendUsingPort);
                            msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", cdoBasic);
                            //msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", userName);
                            //msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", password);
                            msg.To = "mfirdaus@intercity.com.my";
                            //msg.Cc = EmailBy.ToString();
                            msg.From = "mfirdaus@intercity.com.my";
                            msg.Subject = "NOTIFICATION FOR NEW/UPDATE JI";
                            msg.Body = Str;
                            msg.BodyFormat = MailFormat.Html;
                            SmtpMail.SmtpServer = smtpServer;
                            SmtpMail.Send(msg);

                            isEmailSendSuccessfully = true;
                        }
                        catch
                        {
                            isEmailSendSuccessfully = false;
                        }

                    }
                    cn.Close();
                }

            }

        }

        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT Id,Customer_Name,ProductName,ServiceLevel,IsSlaCreaditCard,JobClass,IsSetPaper,JobRequest,ExpectedDateCompletionToGpo,QuotationRef,ContractName,ContactPerson,JobType,DeliveryChannel,AccountsQty,ImpressionQty,PagesQty,CycleTerm,MailingDate,
                                    JoiningFiles,TotalRecord,InputFileName,OutputFileName,Sorting,SortingMode,Other,DataPrintingRemark,
                                    ArtworkStatus,PaperStock,TypeCode,Paper,PaperSize,Grammage,MaterialColour,EnvelopeStock,EnvelopeType,EnvelopeSize,EnvelopeGrammage,EnvelopeColour,EnvelopeWindow,EnvWindowOpaque,LabelStock,LabelCutsheet,OthersStock,BalancedMaterial,PlasticStock,PlasticType,PlasticSize,PlasticThickness,
                                    PrintingType,PrintingOrientation,GpoList,RegisterMail,OtherList,BaseStockType,FinishingSize,AdditionalPrintingMark,SortingCriteria,PrintingInstr,SortingInstr,Letter,Brochures_Leaflets,ReplyEnvelope,ImgOnStatement,Booklet,
                                    NumberOfInsert,Magezine1,Brochure1,CarrierSheet1,Newsletter1,Statement1,Booklet1,CommentManualType,FinishingFormat,FoldingType,Sealing1,Tearing1,BarcodeLabel1,Cutting1,StickingOf1,AddLabel1,Sticker1,Chesire1,Tuck_In1,Bursting1,Sealed1,Folding1,Unsealed1,Letter1,FinishingInst,
                                    IT_SysNotes,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes,ArtworkNotes,Acc_BillingNotes,DCPNotes,PostingInfo
                                    FROM [IflowSeed].[dbo].[JobInstruction]
                                    WHERE Id=@Id";
            command.Parameters.AddWithValue("@Id", Id.ToString());
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
                    ViewBag.ServiceLevel = reader.GetString(3);
                }
                if (reader.IsDBNull(4) == false)
                {
                    bool getIsSlaCreaditCard = reader.GetBoolean(4);
                    if (getIsSlaCreaditCard == false)
                    {
                        ViewBag.IsSlaCreaditCard = "";
                    }
                    else
                    {
                        ViewBag.IsSlaCreaditCard = "checked";
                    }
                }
                if (reader.IsDBNull(5) == false)
                {
                    ViewBag.JobClass = reader.GetString(5);
                }
                if (reader.IsDBNull(6) == false)
                {
                    bool getIsSetPaper = reader.GetBoolean(6);
                    if (getIsSetPaper == false)
                    {
                        ViewBag.IsSetPaper = "";
                    }
                    else
                    {
                        ViewBag.IsSetPaper = "checked";
                    }
                }
                if (reader.IsDBNull(7) == false)
                {
                    ViewBag.JobRequest = String.Format("{0:dd/MM/yyyy}", reader.GetDateTime(7));
                }
                if (reader.IsDBNull(8) == false)
                {
                    ViewBag.ExpectedDateCompletionToGpo = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(8));
                }
                if (reader.IsDBNull(9) == false)
                {
                    ViewBag.QuotationRef = reader.GetString(9);
                }
                if (reader.IsDBNull(10) == false)
                {
                    ViewBag.ContractName = reader.GetString(10);
                }
                if (reader.IsDBNull(11) == false)
                {
                    ViewBag.Contact_Person = reader.GetString(11);
                }
                if (reader.IsDBNull(12) == false)
                {
                    ViewBag.JobType = reader.GetString(12);
                }
                if (reader.IsDBNull(13) == false)
                {
                    ViewBag.DeliveryChannel = reader.GetString(13);
                }
                if (reader.IsDBNull(14) == false)
                {
                    ViewBag.AccountsQty = reader.GetString(14);
                }
                if (reader.IsDBNull(15) == false)
                {
                    ViewBag.ImpressionQty = reader.GetString(15);
                }
                if (reader.IsDBNull(16) == false)
                {
                    ViewBag.PagesQty = reader.GetString(16);
                }
                if (reader.IsDBNull(17) == false)
                {
                    ViewBag.CycleTerm = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(17));
                }
                if (reader.IsDBNull(18) == false)
                {
                    ViewBag.MailingDate = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(18));
                }

                if (reader.IsDBNull(19) == false)
                {
                    ViewBag.JoiningFiles = reader.GetString(19);
                }
                if (reader.IsDBNull(20) == false)
                {
                    ViewBag.TotalRecord = reader.GetString(20);
                }
                if (reader.IsDBNull(21) == false)
                {
                    ViewBag.InputFileName = reader.GetString(21);
                }
                if (reader.IsDBNull(22) == false)
                {
                    ViewBag.OutputFileName = reader.GetString(22);
                }
                if (reader.IsDBNull(23) == false)
                {
                    ViewBag.Sorting = reader.GetString(23);
                }
                if (reader.IsDBNull(24) == false)
                {
                    ViewBag.SortingMode = reader.GetString(24);
                }
                if (reader.IsDBNull(25) == false)
                {
                    ViewBag.Other = reader.GetString(25);
                }
                if (reader.IsDBNull(26) == false)
                {
                    ViewBag.DataPrintingRemark = reader.GetString(26);
                }
                if (reader.IsDBNull(27) == false)
                {
                    ViewBag.ArtworkStatus = reader.GetString(27);
                }
                if (reader.IsDBNull(28) == false)
                {
                    ViewBag.PaperStock = reader.GetString(28);
                }
                if (reader.IsDBNull(29) == false)
                {
                    ViewBag.TypeCode = reader.GetString(29);
                }
                if (reader.IsDBNull(30) == false)
                {
                    ViewBag.Paper = reader.GetString(30);
                }
                if (reader.IsDBNull(31) == false)
                {
                    ViewBag.PaperSize = reader.GetString(31);
                }
                if (reader.IsDBNull(32) == false)
                {
                    ViewBag.Grammage = reader.GetString(32);
                }
                if (reader.IsDBNull(33) == false)
                {
                    ViewBag.MaterialColour = reader.GetString(33);
                }
                if (reader.IsDBNull(34) == false)
                {
                    ViewBag.EnvelopeStock = reader.GetString(34);
                }
                if (reader.IsDBNull(35) == false)
                {
                    ViewBag.EnvelopeType = reader.GetString(35);
                }
                if (reader.IsDBNull(36) == false)
                {
                    ViewBag.EnvelopeSize = reader.GetString(36);
                }
                if (reader.IsDBNull(37) == false)
                {
                    ViewBag.EnvelopeGrammage = reader.GetString(37);
                }
                if (reader.IsDBNull(38) == false)
                {
                    ViewBag.EnvelopeColour = reader.GetString(38);
                }
                if (reader.IsDBNull(39) == false)
                {
                    ViewBag.EnvelopeWindow = reader.GetString(39);
                }
                if (reader.IsDBNull(40) == false)
                {
                    ViewBag.EnvWindowOpaque = reader.GetString(40);
                }
                if (reader.IsDBNull(41) == false)
                {
                    ViewBag.LabelStock = reader.GetString(41);
                }
                if (reader.IsDBNull(42) == false)
                {
                    ViewBag.LabelCutsheet = reader.GetString(42);
                }
                if (reader.IsDBNull(43) == false)
                {
                    ViewBag.OthersStock = reader.GetString(43);
                }
                if (reader.IsDBNull(44) == false)
                {
                    ViewBag.BalancedMaterial = reader.GetString(44);
                }
                if (reader.IsDBNull(45) == false)
                {
                    ViewBag.PlasticStock = reader.GetString(45);
                }
                if (reader.IsDBNull(46) == false)
                {
                    ViewBag.PlasticType = reader.GetString(46);
                }
                if (reader.IsDBNull(47) == false)
                {
                    ViewBag.PlasticSize = reader.GetString(47);
                }
                if (reader.IsDBNull(48) == false)
                {
                    ViewBag.PlasticThickness = reader.GetString(48);
                }
                if (reader.IsDBNull(49) == false)
                {
                    ViewBag.PrintingType = reader.GetString(49);
                }
                if (reader.IsDBNull(50) == false)
                {
                    ViewBag.PrintingOrientation = reader.GetString(50);
                }
                if (reader.IsDBNull(51) == false)
                {
                    bool getGpoList = reader.GetBoolean(51);
                    if (getGpoList == false)
                    {
                        ViewBag.GpoList = "";
                    }
                    else
                    {
                        ViewBag.GpoList = "checked";
                    }
                }
                if (reader.IsDBNull(52) == false)
                {
                    bool getRegisterMail = reader.GetBoolean(52);
                    if (getRegisterMail == false)
                    {
                        ViewBag.RegisterMail = "";
                    }
                    else
                    {
                        ViewBag.RegisterMail = "checked";
                    }
                }
                if (reader.IsDBNull(53) == false)
                {
                    ViewBag.OtherList = reader.GetString(53);
                }
                if (reader.IsDBNull(54) == false)
                {
                    ViewBag.BaseStockType = reader.GetString(54);
                }
                if (reader.IsDBNull(55) == false)
                {
                    ViewBag.FinishingSize = reader.GetString(55);
                }
                if (reader.IsDBNull(56) == false)
                {
                    ViewBag.AdditionalPrintingMark = reader.GetString(56);
                }
                if (reader.IsDBNull(57) == false)
                {
                    ViewBag.SortingCriteria = reader.GetString(57);
                }
                if (reader.IsDBNull(58) == false)
                {
                    ViewBag.PrintingInstr = reader.GetString(58);
                }
                if (reader.IsDBNull(59) == false)
                {
                    ViewBag.SortingInstr = reader.GetString(59);
                }
                if (reader.IsDBNull(60) == false)
                {
                    bool getLetter = reader.GetBoolean(60);
                    if (getLetter == false)
                    {
                        ViewBag.Letter = "";
                    }
                    else
                    {
                        ViewBag.Letter = "checked";
                    }
                }
                if (reader.IsDBNull(61) == false)
                {
                    bool getBrochures_Leaflets = reader.GetBoolean(61);
                    if (getBrochures_Leaflets == false)
                    {
                        ViewBag.Brochures_Leaflets = "";
                    }
                    else
                    {
                        ViewBag.Brochures_Leaflets = "checked";
                    }
                }
                if (reader.IsDBNull(62) == false)
                {
                    bool getReplyEnvelope = reader.GetBoolean(62);
                    if (getReplyEnvelope == false)
                    {
                        ViewBag.ReplyEnvelope = "";
                    }
                    else
                    {
                        ViewBag.ReplyEnvelope = "checked";
                    }
                }
                if (reader.IsDBNull(63) == false)
                {
                    bool getImgOnStatement = reader.GetBoolean(63);
                    if (getImgOnStatement == false)
                    {
                        ViewBag.ImgOnStatement = "";
                    }
                    else
                    {
                        ViewBag.ImgOnStatement = "checked";
                    }
                }
                if (reader.IsDBNull(64) == false)
                {
                    bool getBooklet = reader.GetBoolean(64);
                    if (getBooklet == false)
                    {
                        ViewBag.Booklet = "";
                    }
                    else
                    {
                        ViewBag.Booklet = "checked";
                    }
                }
                if (reader.IsDBNull(65) == false)
                {
                    ViewBag.NumberOfInsert = reader.GetString(65);
                }
                if (reader.IsDBNull(66) == false)
                {
                    bool getMagezine1 = reader.GetBoolean(66);
                    if (getMagezine1 == false)
                    {
                        ViewBag.Magezine1 = "";
                    }
                    else
                    {
                        ViewBag.Magezine1 = "checked";
                    }
                }
                if (reader.IsDBNull(67) == false)
                {
                    bool getBrochure1 = reader.GetBoolean(67);
                    if (getBrochure1 == false)
                    {
                        ViewBag.Brochure1 = "";
                    }
                    else
                    {
                        ViewBag.Brochure1 = "checked";
                    }
                }
                if (reader.IsDBNull(68) == false)
                {
                    bool getCarrierSheet1 = reader.GetBoolean(68);
                    if (getCarrierSheet1 == false)
                    {
                        ViewBag.CarrierSheet1 = "";
                    }
                    else
                    {
                        ViewBag.CarrierSheet1 = "checked";
                    }
                }
                if (reader.IsDBNull(69) == false)
                {
                    bool getNewsletter1 = reader.GetBoolean(69);
                    if (getNewsletter1 == false)
                    {
                        ViewBag.Newsletter1 = "";
                    }
                    else
                    {
                        ViewBag.Newsletter1 = "checked";
                    }
                }
                if (reader.IsDBNull(70) == false)
                {
                    bool getStatement1 = reader.GetBoolean(70);
                    if (getStatement1 == false)
                    {
                        ViewBag.Statement1 = "";
                    }
                    else
                    {
                        ViewBag.Statement1 = "checked";
                    }
                }
                if (reader.IsDBNull(71) == false)
                {
                    bool getBooklet1 = reader.GetBoolean(71);
                    if (getBooklet1 == false)
                    {
                        ViewBag.Booklet1 = "";
                    }
                    else
                    {
                        ViewBag.Booklet1 = "checked";
                    }
                }
                if (reader.IsDBNull(72) == false)
                {
                    ViewBag.CommentManualType = reader.GetString(72);
                }
                if (reader.IsDBNull(73) == false)
                {
                    ViewBag.FinishingFormat = reader.GetString(73);
                }
                if (reader.IsDBNull(74) == false)
                {
                    ViewBag.FoldingType = reader.GetString(74);
                }
                if (reader.IsDBNull(75) == false)
                {
                    bool getSealing1 = reader.GetBoolean(75);
                    if (getSealing1 == false)
                    {
                        ViewBag.Sealing1 = "";
                    }
                    else
                    {
                        ViewBag.Sealing1 = "checked";
                    }
                }
                if (reader.IsDBNull(76) == false)
                {
                    bool getTearing1 = reader.GetBoolean(76);
                    if (getTearing1 == false)
                    {
                        ViewBag.Tearing1 = "";
                    }
                    else
                    {
                        ViewBag.Tearing1 = "checked";
                    }
                }
                if (reader.IsDBNull(77) == false)
                {
                    bool getBarcodeLabel1 = reader.GetBoolean(77);
                    if (getBarcodeLabel1 == false)
                    {
                        ViewBag.BarcodeLabel1 = "";
                    }
                    else
                    {
                        ViewBag.BarcodeLabel1 = "checked";
                    }
                }
                if (reader.IsDBNull(78) == false)
                {
                    bool getCutting1 = reader.GetBoolean(78);
                    if (getCutting1 == false)
                    {
                        ViewBag.Cutting1 = "";
                    }
                    else
                    {
                        ViewBag.Cutting1 = "checked";
                    }
                }
                if (reader.IsDBNull(79) == false)
                {
                    ViewBag.StickingOf1 = reader.GetString(79);
                }
                if (reader.IsDBNull(80) == false)
                {
                    bool getAddLabel1 = reader.GetBoolean(80);
                    if (getAddLabel1 == false)
                    {
                        ViewBag.AddLabel1 = "";
                    }
                    else
                    {
                        ViewBag.AddLabel1 = "checked";
                    }
                }
                if (reader.IsDBNull(81) == false)
                {
                    bool getSticker1 = reader.GetBoolean(81);
                    if (getSticker1 == false)
                    {
                        ViewBag.Sticker1 = "";
                    }
                    else
                    {
                        ViewBag.Sticker1 = "checked";
                    }
                }
                if (reader.IsDBNull(82) == false)
                {
                    bool getChesire1 = reader.GetBoolean(82);
                    if (getChesire1 == false)
                    {
                        ViewBag.Chesire1 = "";
                    }
                    else
                    {
                        ViewBag.Chesire1 = "checked";
                    }
                }
                if (reader.IsDBNull(83) == false)
                {
                    bool getTuck_In1 = reader.GetBoolean(83);
                    if (getTuck_In1 == false)
                    {
                        ViewBag.Tuck_In1 = "";
                    }
                    else
                    {
                        ViewBag.Tuck_In1 = "checked";
                    }
                }
                if (reader.IsDBNull(84) == false)
                {
                    bool getBursting1 = reader.GetBoolean(84);
                    if (getBursting1 == false)
                    {
                        ViewBag.Bursting1 = "";
                    }
                    else
                    {
                        ViewBag.Bursting1 = "checked";
                    }
                }
                if (reader.IsDBNull(85) == false)
                {
                    bool getSealed1 = reader.GetBoolean(85);
                    if (getSealed1 == false)
                    {
                        ViewBag.Sealed1 = "";
                    }
                    else
                    {
                        ViewBag.Sealed1 = "checked";
                    }
                }
                if (reader.IsDBNull(86) == false)
                {
                    bool getFolding1 = reader.GetBoolean(86);
                    if (getFolding1 == false)
                    {
                        ViewBag.Folding1 = "";
                    }
                    else
                    {
                        ViewBag.Folding1 = "checked";
                    }
                }
                if (reader.IsDBNull(87) == false)
                {
                    bool getUnsealed1 = reader.GetBoolean(87);
                    if (getUnsealed1 == false)
                    {
                        ViewBag.Unsealed1 = "";
                    }
                    else
                    {
                        ViewBag.Unsealed1 = "checked";
                    }
                }
                if (reader.IsDBNull(88) == false)
                {
                    bool getLetter1 = reader.GetBoolean(88);
                    if (getLetter1 == false)
                    {
                        ViewBag.Letter1 = "";
                    }
                    else
                    {
                        ViewBag.Letter1 = "checked";
                    }
                }
                if (reader.IsDBNull(89) == false)
                {
                    ViewBag.FinishingInst = reader.GetString(89);
                }
                if (reader.IsDBNull(90) == false)
                {
                    ViewBag.IT_SysNotes = reader.GetString(90);
                }
                if (reader.IsDBNull(91) == false)
                {
                    ViewBag.Produc_PlanningNotes = reader.GetString(91);
                }
                if (reader.IsDBNull(92) == false)
                {
                    ViewBag.PurchasingNotes = reader.GetString(92);
                }
                if (reader.IsDBNull(93) == false)
                {
                    ViewBag.EngineeringNotes = reader.GetString(93);
                }
                if (reader.IsDBNull(94) == false)
                {
                    ViewBag.ArtworkNotes = reader.GetString(94);
                }
                if (reader.IsDBNull(95) == false)
                {
                    ViewBag.Acc_BillingNotes = reader.GetString(95);
                }
                if (reader.IsDBNull(96) == false)
                {
                    ViewBag.DCPNotes = reader.GetString(96);
                }
                if (reader.IsDBNull(97) == false)
                {
                    ViewBag.PostingInfo = reader.GetString(97);
                }

            }
            cn.Close();
            return View();

        }
    }









    [ValidateInput(false)]
    public ActionResult CreateViewJI(string Id, string set, string JobInstructionId, string tabs, string Customer_Name, string ProductName, string JobSheetNo,
                                    string SalesExecutiveBy, string Status,
                                    string ServiceLevel, string IsSlaCreaditCard, string JobClass, string IsSetPaper,
                                    string JobRequest, string ExpectedDateCompletionToGpo, string QuotationRef, string Contract_Name,
                                    string Contact_Person, string JobType, string DeliveryChannel, string AccountsQty, string ImpressionQty,
                                    string PagesQty, string CycleTerm, string MailingDate,
                                    string JoiningFiles, string TotalRecord, string InputFileName, string OutputFileName, string Sorting,
                                    string SortingMode, string Other, string DataPrintingRemark,
                                    string ArtworkStatus, string PaperStock, string TypeCode, string Paper, string PaperSize,
                                    string Grammage, string MaterialColour, string EnvelopeStock, string EnvelopeType, string EnvelopeSize,
                                    string EnvelopeGrammage, string EnvelopeColour, string EnvelopeWindow, string EnvWindowOpaque,
                                    string LabelStock, string LabelCutsheet, string OthersStock, string BalancedMaterial,
                                    string PlasticStock, string PlasticType, string PlasticSize, string PlasticThickness,
                                    string PrintingType, string PrintingOrientation, string GpoList, string RegisterMail,
                                    string OtherList, string BaseStockType, string FinishingSize, string AdditionalPrintingMark,
                                    string SortingCriteria, string PrintingInstr, string SortingInstr, string JobInstruction,
                                    string Picture_FileId, string Picture_Extension, string Letter, string Brochures_Leaflets,
                                    string ReplyEnvelope, string ImgOnStatement, string Booklet,
                                    string NumberOfInsert, string Magezine1, string Brochure1, string CarrierSheet1, string Newsletter1,
                                    string Statement1, string Booklet1, string CommentManualType, string FinishingFormat,
                                    string FoldingType, string Sealing1, string Tearing1, string BarcodeLabel1, string Cutting1,
                                    string StickingOf1, string AddLabel1, string Sticker1, string Chesire1, string Tuck_In1,
                                    string Bursting1, string Sealed1, string Folding1, string Unsealed1, string Letter1, string FinishingInst,
                                    string IT_SysNotes, string Produc_PlanningNotes, string PurchasingNotes, string EngineeringNotes,
                                    string ArtworkNotes, string Acc_BillingNotes, string DCPNotes, string PostingInfo, JobInstruction get)
    {
        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];
        ViewBag.IsDepart = @Session["Department"];
        Session["JobInstructionId"] = Id;
        Session["Id"] = Id;
        Session["Customer_Name"] = Customer_Name;
        ViewBag.JobSheetNo = JobSheetNo;
        ViewBag.Customer_Name = Customer_Name;
        ViewBag.Id = Id;

        List<SelectListItem> listPrintingType = new List<SelectListItem>();

        listPrintingType.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listPrintingType.Add(new SelectListItem { Text = "SIMPLEX", Value = "SIMPLEX" });
        listPrintingType.Add(new SelectListItem { Text = "DUPLEX", Value = "DUPLEX" });

        ViewData["PrintingType_"] = listPrintingType;


        List<SelectListItem> listPrintingOrientation = new List<SelectListItem>();

        listPrintingOrientation.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listPrintingOrientation.Add(new SelectListItem { Text = "LANDSCAPE", Value = "LANDSCAPE" });
        listPrintingOrientation.Add(new SelectListItem { Text = "ORIENTATION", Value = "ORIENTATION" });

        ViewData["PrintingOrientation_"] = listPrintingOrientation;

        List<SelectListItem> listBaseStockType = new List<SelectListItem>();

        listBaseStockType.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listBaseStockType.Add(new SelectListItem { Text = "CONTINUES", Value = "CONTINUES" });
        listBaseStockType.Add(new SelectListItem { Text = "CUT SHEET", Value = "CUT SHEET" });
        listBaseStockType.Add(new SelectListItem { Text = "CUT SHEET OR CONTINUES", Value = "CUT SHEET OR CONTINUES" });
        listBaseStockType.Add(new SelectListItem { Text = "N/A", Value = "N/A" });

        ViewData["BaseStockType_"] = listBaseStockType;

        List<SelectListItem> listAdditionalPrintingMark = new List<SelectListItem>();

        listAdditionalPrintingMark.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listAdditionalPrintingMark.Add(new SelectListItem { Text = "OMR", Value = "OMR" });
        listAdditionalPrintingMark.Add(new SelectListItem { Text = "OMS", Value = "OMS" });
        listAdditionalPrintingMark.Add(new SelectListItem { Text = "N/A", Value = "N/A" });

        ViewData["AdditionalPrintingMark_"] = listAdditionalPrintingMark;

        List<SelectListItem> listSortingInstr = new List<SelectListItem>();

        listSortingInstr.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listSortingInstr.Add(new SelectListItem { Text = "DEDUP", Value = "DEDUP" });
        listSortingInstr.Add(new SelectListItem { Text = "YES", Value = "YES" });
        listSortingInstr.Add(new SelectListItem { Text = "NO", Value = "NO" });
        listSortingInstr.Add(new SelectListItem { Text = "OVERSEA", Value = "OVERSEA" });

        ViewData["SortingInstr_"] = listSortingInstr;

        List<SelectListItem> listFinishingFormat = new List<SelectListItem>();

        listFinishingFormat.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listFinishingFormat.Add(new SelectListItem { Text = "SELF MAILER / TONER SEAL", Value = "SELF MAILER / TONER SEAL" });
        listFinishingFormat.Add(new SelectListItem { Text = "PRESSURE SEAL", Value = "PRESSURE SEAL" });

        ViewData["FinishingFormat_"] = listFinishingFormat;

        if (!string.IsNullOrEmpty(Customer_Name))
        {
            int _bil3 = 1;
            List<SelectListItem> li3 = new List<SelectListItem>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT DISTINCT Contract_Name FROM [IflowSeed].[dbo].[CustomerContract]    
                                            WHERE Customer_Name = @Customer_Name";
                command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JobInstruction model = new JobInstruction();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            model.Contract_Name = reader.GetString(0);
                        }
                    }
                    int i = _bil3++;
                    if (i == 1)
                    {
                        li3.Add(new SelectListItem { Text = "Please Select" });
                    }
                    li3.Add(new SelectListItem { Text = model.Contract_Name });
                }
                cn.Close();
            }
            ViewData["ContractName_"] = li3;
        }
        else
        {
            List<SelectListItem> li3 = new List<SelectListItem>();
            li3.Add(new SelectListItem { Text = "Please Select" });
            ViewData["ContractName_"] = li3;
        }


        int _bil = 1;
        List<SelectListItem> li = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Contact_Person FROM [IflowSeed].[dbo].[CustomerDetails]          
                                     WHERE Customer_Name = @Customer_Name";
            command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.Contact_Person = reader.GetString(0);
                    }
                }
                int i = _bil++;
                if (i == 1)
                {
                    li.Add(new SelectListItem { Text = "Please Select" });
                    li.Add(new SelectListItem { Text = model.Contact_Person });
                }
                else
                {
                    li.Add(new SelectListItem { Text = model.Contact_Person });
                }
            }
            cn.Close();
        }
        ViewData["ContactPerson_"] = li;



        int _bil1 = 1;
        List<SelectListItem> li1 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT TypeCode FROM [IflowSeed].[dbo].[PaperInfo]                          
                                     ORDER BY TypeCode";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.TypeCode = reader.GetString(0);
                    }
                }
                int i = _bil1++;
                if (i == 1)
                {
                    li1.Add(new SelectListItem { Text = "Please Select" });
                    li1.Add(new SelectListItem { Text = model.TypeCode });

                }
                else
                {
                    li1.Add(new SelectListItem { Text = model.TypeCode });
                }
            }
            cn.Close();
        }
        ViewData["TypeCode_"] = li1;

        int _bil2 = 1;
        List<SelectListItem> li2 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Paper FROM [IflowSeed].[dbo].[PaperInfo]                          
                                     ORDER BY Paper";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.Paper = reader.GetString(0);
                    }
                }
                int i = _bil2++;
                if (i == 1)
                {
                    li2.Add(new SelectListItem { Text = "Please Select" });
                    li2.Add(new SelectListItem { Text = model.Paper });

                }
                else
                {
                    li2.Add(new SelectListItem { Text = model.Paper });
                }
            }
            cn.Close();
        }
        ViewData["Paper_"] = li2;




        int _bil4 = 1;
        List<SelectListItem> li4 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Type FROM [IflowSeed].[dbo].[JobClass]          
                                     ORDER BY Type";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.JobClass = reader.GetString(0);
                    }
                }
                int i = _bil4++;
                if (i == 1)
                {
                    li4.Add(new SelectListItem { Text = "Please Select" });
                    li4.Add(new SelectListItem { Text = model.JobClass });

                }
                else
                {
                    li4.Add(new SelectListItem { Text = model.JobClass });
                }
            }
            cn.Close();
        }
        ViewData["JobClass_"] = li4;

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

        int _bil6 = 1;
        List<SelectListItem> li6 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Channel FROM [IflowSeed].[dbo].[DeliveryChannel]          
                                     ORDER BY Channel";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.DeliveryChannel = reader.GetString(0);
                    }
                }
                int i = _bil6++;
                if (i == 1)
                {
                    li6.Add(new SelectListItem { Text = "Please Select" });
                    li6.Add(new SelectListItem { Text = model.DeliveryChannel });

                }
                else
                {
                    li6.Add(new SelectListItem { Text = model.DeliveryChannel });
                }
            }
            cn.Close();
        }
        ViewData["DeliveryChannel_"] = li6;

        int _bil7 = 1;
        List<SelectListItem> li7 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Description FROM [IflowSeed].[dbo].[MaterialCharges]          
                                    WHERE MaterialType='Envelope' ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.EnvelopeType = reader.GetString(0);
                    }
                }
                int i = _bil7++;
                if (i == 1)
                {
                    li7.Add(new SelectListItem { Text = "Please Select" });
                    li7.Add(new SelectListItem { Text = model.EnvelopeType });

                }
                else
                {
                    li7.Add(new SelectListItem { Text = model.EnvelopeType });
                }
            }
            cn.Close();
        }
        ViewData["EnvelopeType_"] = li7;

        int _bil8 = 1;
        List<SelectListItem> li8 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Size FROM [IflowSeed].[dbo].[EnvelopeSize]          
                                    ORDER BY Size ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.EnvelopeSize = reader.GetString(0);
                    }
                }
                int i = _bil8++;
                if (i == 1)
                {
                    li8.Add(new SelectListItem { Text = "Please Select" });
                    li8.Add(new SelectListItem { Text = model.EnvelopeSize });

                }
                else
                {
                    li8.Add(new SelectListItem { Text = model.EnvelopeSize });
                }
            }
            cn.Close();
        }
        ViewData["EnvelopeSize_"] = li8;

        int _bil9 = 1;
        List<SelectListItem> li9 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Size FROM [IflowSeed].[dbo].[PaperSize]          
                                    ORDER BY Size ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.PaperSize = reader.GetString(0);
                    }
                }
                int i = _bil9++;
                if (i == 1)
                {
                    li9.Add(new SelectListItem { Text = "Please Select" });
                    li9.Add(new SelectListItem { Text = model.PaperSize });

                }
                else
                {
                    li9.Add(new SelectListItem { Text = model.PaperSize });
                }
            }
            cn.Close();
        }
        ViewData["PaperSize_"] = li9;

        int _bil10 = 1;
        List<SelectListItem> li10 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Type FROM [IflowSeed].[dbo].[Grammage]          
                                    ORDER BY Type ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.Grammage = reader.GetString(0);
                    }
                }
                int i = _bil10++;
                if (i == 1)
                {
                    li10.Add(new SelectListItem { Text = "Please Select" });
                    li10.Add(new SelectListItem { Text = model.Grammage });

                }
                else
                {
                    li10.Add(new SelectListItem { Text = model.Grammage });
                }
            }
            cn.Close();
        }
        ViewData["Grammage_"] = li10;

        int _bil11 = 1;
        List<SelectListItem> li11 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Size FROM [IflowSeed].[dbo].[FinishingSize]          
                                    ORDER BY Size ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.FinishingSize = reader.GetString(0);
                    }
                }
                int i = _bil11++;
                if (i == 1)
                {
                    li11.Add(new SelectListItem { Text = "Please Select" });
                    li11.Add(new SelectListItem { Text = model.FinishingSize });

                }
                else
                {
                    li11.Add(new SelectListItem { Text = model.FinishingSize });
                }
            }
            cn.Close();
        }
        ViewData["FinishingSize_"] = li11;

        int _bil12 = 1;
        List<SelectListItem> li12 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Mode FROM [IflowSeed].[dbo].[SortingMode]          
                                    ORDER BY Mode ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.SortingMode = reader.GetString(0);
                    }
                }
                int i = _bil12++;
                if (i == 1)
                {
                    li12.Add(new SelectListItem { Text = "Please Select" });
                    li12.Add(new SelectListItem { Text = model.SortingMode });

                }
                else
                {
                    li12.Add(new SelectListItem { Text = model.SortingMode });
                }
            }
            cn.Close();
        }
        ViewData["SortingMode_"] = li12;

        int _bil13 = 1;
        List<SelectListItem> li13 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Status FROM [IflowSeed].[dbo].[ArtworkStatus]          
                                    ORDER BY Status ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.ArtworkStatus = reader.GetString(0);
                    }
                }
                int i = _bil13++;
                if (i == 1)
                {
                    li13.Add(new SelectListItem { Text = "Please Select" });
                    li13.Add(new SelectListItem { Text = model.ArtworkStatus });

                }
                else
                {
                    li13.Add(new SelectListItem { Text = model.ArtworkStatus });
                }
            }
            cn.Close();
        }
        ViewData["ArtworkStatus_"] = li13;

        int _bil14 = 1;
        List<SelectListItem> li14 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Stock FROM [IflowSeed].[dbo].[EnvelopeStock]          
                                    ORDER BY Stock ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.EnvelopeStock = reader.GetString(0);
                    }
                }
                int i = _bil14++;
                if (i == 1)
                {
                    li14.Add(new SelectListItem { Text = "Please Select" });
                    li14.Add(new SelectListItem { Text = model.EnvelopeStock });

                }
                else
                {
                    li14.Add(new SelectListItem { Text = model.EnvelopeStock });
                }
            }
            cn.Close();
        }
        ViewData["EnvelopeStock_"] = li14;

        int _bil15 = 1;
        List<SelectListItem> li15 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT Window FROM [IflowSeed].[dbo].[EnvelopeWindow]          
                                    ORDER BY Window ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.EnvelopeWindow = reader.GetString(0);
                    }
                }
                int i = _bil15++;
                if (i == 1)
                {
                    li15.Add(new SelectListItem { Text = "Please Select" });
                    li15.Add(new SelectListItem { Text = model.EnvelopeWindow });

                }
                else
                {
                    li15.Add(new SelectListItem { Text = model.EnvelopeWindow });
                }
            }
            cn.Close();
        }
        ViewData["EnvelopeWindow_"] = li15;

        int _bil16 = 1;
        List<SelectListItem> li16 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT Opaque FROM [IflowSeed].[dbo].[EnvWindowOpaque]          
                                    ORDER BY Opaque ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.EnvWindowOpaque = reader.GetString(0);
                    }
                }
                int i = _bil16++;
                if (i == 1)
                {
                    li16.Add(new SelectListItem { Text = "Please Select" });
                    li16.Add(new SelectListItem { Text = model.EnvWindowOpaque });

                }
                else
                {
                    li16.Add(new SelectListItem { Text = model.EnvWindowOpaque });
                }
            }
            cn.Close();
        }
        ViewData["EnvWindowOpaque_"] = li16;

        int _bil17 = 1;
        List<SelectListItem> li17 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Stock FROM [IflowSeed].[dbo].[PlasticStock]          
                                    ORDER BY Stock ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.PlasticStock = reader.GetString(0);
                    }
                }
                int i = _bil17++;
                if (i == 1)
                {
                    li17.Add(new SelectListItem { Text = "Please Select" });
                    li17.Add(new SelectListItem { Text = model.PlasticStock });

                }
                else
                {
                    li17.Add(new SelectListItem { Text = model.PlasticStock });
                }
            }
            cn.Close();
        }
        ViewData["PlasticStock_"] = li17;

        int _bil18 = 1;
        List<SelectListItem> li18 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Label FROM [IflowSeed].[dbo].[LabelCutsheet]          
                                    ORDER BY Label ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.LabelCutsheet = reader.GetString(0);
                    }
                }
                int i = _bil18++;
                if (i == 1)
                {
                    li18.Add(new SelectListItem { Text = "Please Select" });
                    li18.Add(new SelectListItem { Text = model.LabelCutsheet });

                }
                else
                {
                    li18.Add(new SelectListItem { Text = model.LabelCutsheet });
                }
            }
            cn.Close();
        }
        ViewData["LabelCutsheet_"] = li18;

        int _bil19 = 1;
        List<SelectListItem> li19 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Action FROM [IflowSeed].[dbo].[BalancedMaterial]          
                                    ORDER BY Action ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.BalancedMaterial = reader.GetString(0);
                    }
                }
                int i = _bil19++;
                if (i == 1)
                {
                    li19.Add(new SelectListItem { Text = "Please Select" });
                    li19.Add(new SelectListItem { Text = model.BalancedMaterial });

                }
                else
                {
                    li19.Add(new SelectListItem { Text = model.BalancedMaterial });
                }
            }
            cn.Close();
        }
        ViewData["BalancedMaterial_"] = li19;

        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT Id,Customer_Name,ProductName,JobSheetNo,Status,
                                           ServiceLevel,IsSlaCreaditCard,JobClass,IsSetPaper,JobRequest,
                                           ExpectedDateCompletionToGpo,QuotationRef,
                                           JobType,DeliveryChannel,AccountsQty,ImpressionQty,
                                           PagesQty,CycleTerm,MailingDate,
                                           JoiningFiles,TotalRecord,InputFileName,OutputFileName,Sorting,
                                           SortingMode,Other,DataPrintingRemark,
                                           ArtworkStatus,PaperStock,
                                           Grammage,MaterialColour,EnvelopeStock,EnvelopeType,EnvelopeSize,
                                           EnvelopeGrammage,EnvelopeColour,EnvelopeWindow,EnvWindowOpaque,
                                           LabelStock,LabelCutsheet,OthersStock,BalancedMaterial,
                                           PlasticStock,PlasticType,PlasticSize,PlasticThickness,
                                           PrintingType,PrintingOrientation,GpoList,RegisterMail,
                                           OtherList,BaseStockType,FinishingSize,AdditionalPrintingMark,
                                           SortingCriteria,PrintingInstr,SortingInstr,Letter,Brochures_Leaflets,
                                           ReplyEnvelope,ImgOnStatement,Booklet,
                                           NumberOfInsert,FinishingInst,
                                           IT_SysNotes,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes,
                                           ArtworkNotes, Acc_BillingNotes, DCPNotes, PostingInfo
                                       FROM [IflowSeed].[dbo].[JobBatchInfo]
                                    WHERE Id=@Id";
            command.Parameters.AddWithValue("@Id", Id.ToString());
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
                        ViewBag.JobSheetNo = reader.GetString(3);
                    }

                    if (reader.IsDBNull(4) == false)
                    {
                        ViewBag.Status = reader.GetString(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        ViewBag.ServiceLevel = reader.GetString(5);
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        ViewBag.SlaCreaditCard = reader.GetBoolean(6);

                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        ViewBag.JobClass = reader.GetString(7);
                    }
                    if (reader.IsDBNull(8) == false)
                    {
                        ViewBag.IsSetPaper = reader.GetBoolean(8);

                    }
                    if (reader.IsDBNull(9) == false)
                    {
                        ViewBag.JobRequest = reader.GetDateTime(9);
                    }
                    if (reader.IsDBNull(10) == false)
                    {
                        ViewBag.ExpectedDateCompletionToGpo = reader.GetDateTime(10);
                    }
                    if (reader.IsDBNull(11) == false)
                    {
                        ViewBag.QuotationRef = reader.GetString(11);
                    }


                    if (reader.IsDBNull(12) == false)
                    {
                        ViewBag.JobType = reader.GetString(12);
                    }
                    if (reader.IsDBNull(13) == false)
                    {
                        ViewBag.DeliveryChannel = reader.GetString(13);
                    }
                    if (reader.IsDBNull(14) == false)
                    {
                        ViewBag.AccountsQty = reader.GetString(14);
                    }
                    if (reader.IsDBNull(15) == false)
                    {
                        ViewBag.ImpressionQty = reader.GetString(15);
                    }
                    if (reader.IsDBNull(16) == false)
                    {
                        ViewBag.PagesQty = reader.GetString(16);
                    }
                    if (reader.IsDBNull(17) == false)
                    {
                        ViewBag.CycleTerm = reader.GetDateTime(17);
                    }
                    if (reader.IsDBNull(18) == false)
                    {
                        ViewBag.MailingDate = (DateTime)reader.GetDateTime(18);
                    }

                    if (reader.IsDBNull(19) == false)
                    {
                        ViewBag.JoiningFiles = reader.GetString(19);
                    }
                    if (reader.IsDBNull(20) == false)
                    {
                        ViewBag.TotalRecord = reader.GetString(20);
                    }
                    if (reader.IsDBNull(21) == false)
                    {
                        ViewBag.InputFileName = reader.GetString(21);
                    }
                    if (reader.IsDBNull(22) == false)
                    {
                        ViewBag.OutputFileName = reader.GetString(22);
                    }
                    if (reader.IsDBNull(23) == false)
                    {
                        ViewBag.Sorting = reader.GetString(23);
                    }
                    if (reader.IsDBNull(24) == false)
                    {
                        ViewBag.SortingMode = reader.GetString(24);
                    }
                    if (reader.IsDBNull(25) == false)
                    {
                        ViewBag.Other = reader.GetString(25);
                    }
                    if (reader.IsDBNull(26) == false)
                    {
                        ViewBag.DataPrintingRemark = reader.GetString(26);
                    }
                    if (reader.IsDBNull(27) == false)
                    {
                        ViewBag.ArtworkStatus = reader.GetString(27);
                    }
                    if (reader.IsDBNull(28) == false)
                    {
                        ViewBag.PaperStock = reader.GetString(28);
                    }


                    if (reader.IsDBNull(29) == false)
                    {
                        ViewBag.Grammage = reader.GetString(29);
                    }
                    if (reader.IsDBNull(30) == false)
                    {
                        ViewBag.MaterialColour = reader.GetString(30);
                    }
                    if (reader.IsDBNull(31) == false)
                    {
                        ViewBag.EnvelopeStock = reader.GetString(31);
                    }
                    if (reader.IsDBNull(32) == false)
                    {
                        ViewBag.EnvelopeType = reader.GetString(32);
                    }
                    if (reader.IsDBNull(33) == false)
                    {
                        ViewBag.EnvelopeSize = reader.GetString(33);
                    }
                    if (reader.IsDBNull(34) == false)
                    {
                        ViewBag.EnvelopeGrammage = reader.GetString(34);
                    }
                    if (reader.IsDBNull(35) == false)
                    {
                        ViewBag.EnvelopeColour = reader.GetString(35);
                    }
                    if (reader.IsDBNull(36) == false)
                    {
                        ViewBag.EnvelopeWindow = reader.GetString(36);
                    }
                    if (reader.IsDBNull(37) == false)
                    {
                        ViewBag.EnvWindowOpaque = reader.GetString(37);
                    }
                    if (reader.IsDBNull(38) == false)
                    {
                        ViewBag.LabelStock = reader.GetString(38);
                    }
                    if (reader.IsDBNull(39) == false)
                    {
                        ViewBag.LabelCutsheet = reader.GetString(39);
                    }
                    if (reader.IsDBNull(40) == false)
                    {
                        ViewBag.OthersStock = reader.GetString(40);
                    }
                    if (reader.IsDBNull(41) == false)
                    {
                        ViewBag.BalancedMaterial = reader.GetString(41);
                    }
                    if (reader.IsDBNull(42) == false)
                    {
                        ViewBag.PlasticStock = reader.GetString(42);
                    }
                    if (reader.IsDBNull(43) == false)
                    {
                        ViewBag.PlasticType = reader.GetString(43);
                    }
                    if (reader.IsDBNull(44) == false)
                    {
                        ViewBag.PlasticSize = reader.GetString(44);
                    }
                    if (reader.IsDBNull(45) == false)
                    {
                        ViewBag.PlasticThickness = reader.GetString(45);
                    }
                    if (reader.IsDBNull(46) == false)
                    {
                        ViewBag.PrintingType = reader.GetString(46);
                    }
                    if (reader.IsDBNull(47) == false)
                    {
                        ViewBag.PrintingOrientation = reader.GetString(47);
                    }
                    if (reader.IsDBNull(48) == false)
                    {
                        ViewBag.GpoList = reader.GetBoolean(48);
                    }
                    if (reader.IsDBNull(49) == false)
                    {
                        ViewBag.RegisterMail = reader.GetBoolean(49);
                    }
                    if (reader.IsDBNull(50) == false)
                    {
                        ViewBag.OtherList = reader.GetString(50);
                    }
                    if (reader.IsDBNull(51) == false)
                    {
                        ViewBag.BaseStockType = reader.GetString(51);
                    }
                    if (reader.IsDBNull(52) == false)
                    {
                        ViewBag.FinishingSize = reader.GetString(52);
                    }
                    if (reader.IsDBNull(53) == false)
                    {
                        ViewBag.AdditionalPrintingMark = reader.GetString(53);
                    }
                    if (reader.IsDBNull(54) == false)
                    {
                        ViewBag.SortingCriteria = reader.GetString(54);
                    }
                    if (reader.IsDBNull(55) == false)
                    {
                        ViewBag.PrintingInstr = reader.GetString(55);
                    }
                    if (reader.IsDBNull(56) == false)
                    {
                        ViewBag.SortingInstr = reader.GetString(56);
                    }
                    if (reader.IsDBNull(57) == false)
                    {
                        ViewBag.Letter = reader.GetBoolean(57);
                    }
                    if (reader.IsDBNull(58) == false)
                    {
                        ViewBag.Brochures_Leaflets = reader.GetBoolean(58);
                    }
                    if (reader.IsDBNull(59) == false)
                    {
                        ViewBag.ReplyEnvelope = reader.GetBoolean(59);
                    }
                    if (reader.IsDBNull(60) == false)
                    {
                        ViewBag.ImgOnStatement = reader.GetBoolean(60);
                    }
                    if (reader.IsDBNull(61) == false)
                    {
                        ViewBag.Booklet = reader.GetBoolean(61);
                    }
                    if (reader.IsDBNull(62) == false)
                    {
                        ViewBag.NumberOfInsert = reader.GetString(62);
                    }


                    if (reader.IsDBNull(63) == false)
                    {
                        ViewBag.FinishingInst = reader.GetString(63);
                    }
                    if (reader.IsDBNull(64) == false)
                    {
                        ViewBag.IT_SysNotes = reader.GetString(64);
                    }
                    if (reader.IsDBNull(65) == false)
                    {
                        ViewBag.Produc_PlanningNotes = reader.GetString(65);
                    }
                    if (reader.IsDBNull(66) == false)
                    {
                        ViewBag.PurchasingNotes = reader.GetString(66);
                    }
                    if (reader.IsDBNull(67) == false)
                    {
                        ViewBag.EngineeringNotes = reader.GetString(67);
                    }
                    if (reader.IsDBNull(68) == false)
                    {
                        ViewBag.ArtworkNotes = reader.GetString(68);
                    }
                    if (reader.IsDBNull(69) == false)
                    {
                        ViewBag.Acc_BillingNotes = reader.GetString(69);
                    }
                    if (reader.IsDBNull(70) == false)
                    {
                        ViewBag.DCPNotes = reader.GetString(70);
                    }
                    if (reader.IsDBNull(71) == false)
                    {
                        ViewBag.PostingInfo = reader.GetString(71);
                    }

                }
                JIHistory.Add(model);
            }
            cn.Close();

        }

        List<JobInstruction> viewprofile = new List<JobInstruction>();

        List<JobInstruction> viewData = new List<JobInstruction>();
        List<JobInstruction> viewMaterial = new List<JobInstruction>();
        List<JobInstruction> ViewProduction = new List<JobInstruction>();
        List<JobInstruction> ViewFinishing = new List<JobInstruction>();
        List<JobInstruction> viewImportant = new List<JobInstruction>();

        if (set == "ProfileJI")
        {
            if (!string.IsNullOrEmpty(Id) /*&& JobClass != "Please Select" && JobType != "Please Select" && DeliveryChannel != "Please Select" && !string.IsNullOrEmpty(ServiceLevel) && !string.IsNullOrEmpty(JobClass) && !string.IsNullOrEmpty(JobRequest) && !string.IsNullOrEmpty(JobType) && !string.IsNullOrEmpty(DeliveryChannel) && !string.IsNullOrEmpty(AccountsQty)*/)
            {
                string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                string ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                get.JobRequest = Convert.ToDateTime(get.JobRequestTxt);
                get.ExpectedDateCompletionToGpo = Convert.ToDateTime(get.ExpectedDateCompletionToGpoTxt);
                get.CycleTerm = Convert.ToDateTime(get.CycleTermTxt);
                get.MailingDate = Convert.ToDateTime(get.MailingDateTxt);


                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn.Open();
                    SqlCommand command;
                    command = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET ServiceLevel=@ServiceLevel, IsSlaCreaditCard=@IsSlaCreaditCard, JobClass=@JobClass, IsSetPaper=@IsSetPaper, JobRequest=@JobRequest, ExpectedDateCompletionToGpo=@ExpectedDateCompletionToGpo, QuotationRef=@QuotationRef, Contract_Name=@Contract_Name, Contact_Person=@Contact_Person, JobType=@JobType, DeliveryChannel=@DeliveryChannel, AccountsQty=@AccountsQty, ImpressionQty=@ImpressionQty, PagesQty=@PagesQty, CycleTerm=@CycleTerm, MailingDate=@MailingDate  WHERE Id =@Id", cn);
                    command.Parameters.AddWithValue("@ServiceLevel", ServiceLevel);
                    if (IsSlaCreaditCard == "on")
                    {
                        command.Parameters.AddWithValue("@IsSlaCreaditCard", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@IsSlaCreaditCard", false);
                    }
                    if (!string.IsNullOrEmpty(JobClass))
                    {
                        command.Parameters.AddWithValue("@JobClass", JobClass);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@JobClass", DBNull.Value);
                    }
                    if (IsSetPaper == "on")
                    {
                        command.Parameters.AddWithValue("@IsSetPaper", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@IsSetPaper", false);
                    }

                    if (!string.IsNullOrEmpty(JobRequest))
                    {
                        string ddd = Convert.ToDateTime(JobRequest).ToString("yyyy-MM-dd");
                        command.Parameters.AddWithValue("@JobRequest", ddd);

                    }
                    else
                    {
                        command.Parameters.AddWithValue("@JobRequest", null);
                    }
                    if (!string.IsNullOrEmpty(ExpectedDateCompletionToGpo))
                    {
                        string ddd1 = Convert.ToDateTime(ExpectedDateCompletionToGpo).ToString("yyyy-MM-dd");
                        command.Parameters.AddWithValue("@ExpectedDateCompletionToGpo", ddd1);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ExpectedDateCompletionToGpo", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(QuotationRef))
                    {
                        command.Parameters.AddWithValue("@QuotationRef", QuotationRef);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@QuotationRef", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(Contract_Name))
                    {
                        command.Parameters.AddWithValue("@Contract_Name", Contract_Name);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Contract_Name", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(Contact_Person))
                    {
                        command.Parameters.AddWithValue("@Contact_Person", Contact_Person);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Contact_Person", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(JobType))
                    {
                        command.Parameters.AddWithValue("@JobType", JobType);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@JobType", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(DeliveryChannel))
                    {
                        command.Parameters.AddWithValue("@DeliveryChannel", DeliveryChannel);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@DeliveryChannel", DBNull.Value);
                    }
                    command.Parameters.AddWithValue("@AccountsQty", AccountsQty);
                    command.Parameters.AddWithValue("@ImpressionQty", ImpressionQty);
                    command.Parameters.AddWithValue("@PagesQty", PagesQty);

                    if (!string.IsNullOrEmpty(CycleTerm))
                    {
                        string ddd2 = Convert.ToDateTime(CycleTerm).ToString("yyyy-MM-dd");
                        command.Parameters.AddWithValue("@CycleTerm", ddd2);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@CycleTerm", null);
                    }
                    if (!string.IsNullOrEmpty(MailingDate))
                    {
                        string ddd3 = Convert.ToDateTime(MailingDate).ToString("yyyy-MM-dd");
                        command.Parameters.AddWithValue("@MailingDate", ddd3);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@MailingDate", null);
                    }

                    command.Parameters.AddWithValue("@Id", Id);


                    command.ExecuteNonQuery();
                    cn.Close();
                }
            }

            return RedirectToAction("ManageJobInstruction", "MBD");
        }

        else if (set == "DataProcess")
        {
            if (!string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(JoiningFiles) && !string.IsNullOrEmpty(TotalRecord) && !string.IsNullOrEmpty(InputFileName) && !string.IsNullOrEmpty(OutputFileName) && !string.IsNullOrEmpty(Sorting) && !string.IsNullOrEmpty(SortingMode))
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn.Open();
                    SqlCommand command;
                    command = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET JoiningFiles=@JoiningFiles, TotalRecord=@TotalRecord, InputFileName=@InputFileName, OutputFileName=@OutputFileName, Sorting=@Sorting, SortingMode=@SortingMode, Other=@Other, DataPrintingRemark=@DataPrintingRemark WHERE Id =@Id", cn);
                    command.Parameters.AddWithValue("@JoiningFiles", JoiningFiles);
                    command.Parameters.AddWithValue("@TotalRecord", TotalRecord);
                    command.Parameters.AddWithValue("@InputFileName", InputFileName);
                    command.Parameters.AddWithValue("@OutputFileName", OutputFileName);
                    command.Parameters.AddWithValue("@Sorting", Sorting);
                    command.Parameters.AddWithValue("@SortingMode", SortingMode);
                    if (!string.IsNullOrEmpty(Other))
                    {
                        command.Parameters.AddWithValue("@Other", Other);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Other", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(DataPrintingRemark))
                    {
                        command.Parameters.AddWithValue("@DataPrintingRemark", DataPrintingRemark);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@DataPrintingRemark", DBNull.Value);
                    }
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                    cn.Close();
                }
            }

            return View();
        }
        else if (set == "MaterialInfo")
        {
            if (!string.IsNullOrEmpty(Id) && PaperStock != "Please Select" && !string.IsNullOrEmpty(ArtworkStatus))
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn.Open();
                    SqlCommand command;
                    command = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET ArtworkStatus = @ArtworkStatus, PaperStock = @PaperStock, TypeCode = @TypeCode, Paper = @Paper, PaperSize= @PaperSize, Grammage =@Grammage, MaterialColour = @MaterialColour, EnvelopeStock =@EnvelopeStock, EnvelopeType = @EnvelopeType, EnvelopeSize = @EnvelopeSize, EnvelopeGrammage = @EnvelopeGrammage, EnvelopeColour = @EnvelopeColour, EnvelopeWindow = @EnvelopeWindow, EnvWindowOpaque = @EnvWindowOpaque, LabelStock = @LabelStock, LabelCutsheet = @LabelCutsheet, OthersStock = @OthersStock, BalancedMaterial = @BalancedMaterial, PlasticStock = @PlasticStock, PlasticType = @PlasticType, PlasticSize = @PlasticSize, PlasticThickness = @PlasticThickness  WHERE Id =@Id", cn);
                    command.Parameters.AddWithValue("@ArtworkStatus", ArtworkStatus);
                    command.Parameters.AddWithValue("@PaperStock", PaperStock);
                    command.Parameters.AddWithValue("@TypeCode", TypeCode);
                    if (!string.IsNullOrEmpty(Paper))
                    {
                        command.Parameters.AddWithValue("@Paper", Paper);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Paper", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(PaperSize))
                    {
                        command.Parameters.AddWithValue("@PaperSize", PaperSize);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@PaperSize", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(Grammage))
                    {
                        command.Parameters.AddWithValue("@Grammage", Grammage);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Grammage", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(MaterialColour))
                    {
                        command.Parameters.AddWithValue("@MaterialColour", MaterialColour);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@MaterialColour", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(EnvelopeStock))
                    {
                        command.Parameters.AddWithValue("@EnvelopeStock", EnvelopeStock);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@EnvelopeStock", DBNull.Value);

                    }
                    if (!string.IsNullOrEmpty(EnvelopeType))
                    {
                        command.Parameters.AddWithValue("@EnvelopeType", EnvelopeType);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@EnvelopeType", DBNull.Value);

                    }
                    if (!string.IsNullOrEmpty(EnvelopeSize))
                    {
                        command.Parameters.AddWithValue("@EnvelopeSize", EnvelopeSize);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@EnvelopeSize", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(EnvelopeGrammage))
                    {
                        command.Parameters.AddWithValue("@EnvelopeGrammage", EnvelopeGrammage);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@EnvelopeGrammage", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(EnvelopeColour))
                    {
                        command.Parameters.AddWithValue("@EnvelopeColour", EnvelopeColour);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@EnvelopeColour", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(EnvelopeColour))
                    {
                        command.Parameters.AddWithValue("@EnvelopeWindow", EnvelopeWindow);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@EnvelopeWindow", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(EnvWindowOpaque))
                    {
                        command.Parameters.AddWithValue("@EnvWindowOpaque", EnvWindowOpaque);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@EnvWindowOpaque", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(LabelStock))
                    {
                        command.Parameters.AddWithValue("@LabelStock", LabelStock);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@LabelStock", DBNull.Value);

                    }
                    if (!string.IsNullOrEmpty(LabelCutsheet))
                    {
                        command.Parameters.AddWithValue("@LabelCutsheet", LabelCutsheet);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@LabelCutsheet", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(OthersStock))
                    {
                        command.Parameters.AddWithValue("@OthersStock", OthersStock);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@OthersStock", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(BalancedMaterial))
                    {
                        command.Parameters.AddWithValue("@BalancedMaterial", BalancedMaterial);

                    }
                    else
                    {
                        command.Parameters.AddWithValue("@BalancedMaterial", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(PlasticStock))
                    {
                        command.Parameters.AddWithValue("@PlasticStock", PlasticStock);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@PlasticStock", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(PlasticType))
                    {
                        command.Parameters.AddWithValue("@PlasticType", PlasticType);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@PlasticType", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(PlasticSize))
                    {
                        command.Parameters.AddWithValue("@PlasticSize", PlasticSize);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@PlasticSize", DBNull.Value);

                    }
                    if (!string.IsNullOrEmpty(PlasticSize))
                    {
                        command.Parameters.AddWithValue("@PlasticThickness", PlasticThickness);

                    }
                    else
                    {
                        command.Parameters.AddWithValue("@PlasticThickness", DBNull.Value);
                    }
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                    cn.Close();
                }
            }

            return View();
        }
        else if (set == "ProductionList")
        {
            if (!string.IsNullOrEmpty(Id) && PrintingType != "Please Select" && PrintingOrientation != "Please Select" && BaseStockType != "Please Select" && !string.IsNullOrEmpty(PrintingType) && !string.IsNullOrEmpty(PrintingOrientation))
            {

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn.Open();
                    SqlCommand command;
                    command = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET PrintingType=@PrintingType,PrintingOrientation=@PrintingOrientation,GpoList=@GpoList,RegisterMail=@RegisterMail,OtherList=@OtherList,BaseStockType=@BaseStockType,FinishingSize=@FinishingSize,AdditionalPrintingMark=@AdditionalPrintingMark,SortingCriteria=@SortingCriteria,PrintingInstr=@PrintingInstr,SortingInstr=@SortingInstr," +
                                             "Letter=@Letter,Brochures_Leaflets=@Brochures_Leaflets,ReplyEnvelope=@ReplyEnvelope,ImgOnStatement=@ImgOnStatement,Booklet=@Booklet WHERE Id=@Id", cn);

                    if (!string.IsNullOrEmpty(PrintingType))
                    {
                        command.Parameters.AddWithValue("@PrintingType", PrintingType);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@PrintingType", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(PrintingOrientation))
                    {
                        command.Parameters.AddWithValue("@PrintingOrientation", PrintingOrientation);

                    }
                    else
                    {
                        command.Parameters.AddWithValue("@PrintingOrientation", DBNull.Value);
                    }


                    if (GpoList == "on")
                    {
                        command.Parameters.AddWithValue("@GpoList", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@GpoList", false);
                    }
                    if (RegisterMail == "on")
                    {
                        command.Parameters.AddWithValue("@RegisterMail", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RegisterMail", false);
                    }

                    if (!string.IsNullOrEmpty(OtherList))
                    {
                        command.Parameters.AddWithValue("@OtherList", OtherList);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@OtherList", DBNull.Value);

                    }
                    if (!string.IsNullOrEmpty(BaseStockType))
                    {
                        command.Parameters.AddWithValue("@BaseStockType", BaseStockType);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@BaseStockType", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(FinishingSize))
                    {
                        command.Parameters.AddWithValue("@FinishingSize", FinishingSize);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@FinishingSize", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(AdditionalPrintingMark))
                    {
                        command.Parameters.AddWithValue("@AdditionalPrintingMark", AdditionalPrintingMark);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@AdditionalPrintingMark", DBNull.Value);
                    }


                    if (!string.IsNullOrEmpty(SortingCriteria))
                    {

                        command.Parameters.AddWithValue("@SortingCriteria", false);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@SortingCriteria", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(PrintingInstr))
                    {

                        command.Parameters.AddWithValue("@PrintingInstr", PrintingInstr);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@PrintingInstr", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(SortingInstr))
                    {
                        command.Parameters.AddWithValue("@SortingInstr", SortingInstr);

                    }
                    else
                    {
                        command.Parameters.AddWithValue("@SortingInstr", DBNull.Value);
                    }
                    if (Letter == "on")
                    {
                        command.Parameters.AddWithValue("@Letter", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Letter", false);
                    }
                    if (Brochures_Leaflets == "on")
                    {
                        command.Parameters.AddWithValue("@Brochures_Leaflets", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Brochures_Leaflets", false);
                    }
                    if (ReplyEnvelope == "on")
                    {
                        command.Parameters.AddWithValue("@ReplyEnvelope", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ReplyEnvelope", false);
                    }
                    if (ImgOnStatement == "on")
                    {
                        command.Parameters.AddWithValue("@ImgOnStatement", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ImgOnStatement", false);
                    }
                    if (Booklet == "on")
                    {
                        command.Parameters.AddWithValue("@Booklet", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Booklet", false);
                    }
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                    cn.Close();
                }
            }

            return View();
        }
        else if (set == "FinishingInst")
        {
            if (!string.IsNullOrEmpty(Id) && FinishingFormat != "Please Select" && !string.IsNullOrEmpty(NumberOfInsert) && !string.IsNullOrEmpty(CommentManualType) && !string.IsNullOrEmpty(FinishingFormat) && !string.IsNullOrEmpty(FoldingType) && !string.IsNullOrEmpty(StickingOf1) && !string.IsNullOrEmpty(FinishingInst))
            {

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn.Open();
                    SqlCommand command;
                    command = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET NumberOfInsert=@NumberOfInsert,Magezine1=@Magezine1,Brochure1=@Brochure1,CarrierSheet1=@CarrierSheet1,Newsletter1=@Newsletter1,Statement1=@Statement1,Booklet1=@Booklet1,CommentManualType=@CommentManualType,FinishingFormat=@FinishingFormat,FoldingType=@FoldingType,Sealing1=@Sealing1,Tearing1=@Tearing1,BarcodeLabel1=@BarcodeLabel1,Cutting1=@Cutting1,StickingOf1=@StickingOf1,AddLabel1=@AddLabel1,Sticker1=@Sticker1,Chesire1=@Chesire1,Tuck_In1=@Tuck_In1,Bursting1=@Bursting1,Sealed1=@Sealed1,Folding1=@Folding1,Unsealed1=@Unsealed1,Letter1=@Letter1,FinishingInst=@FinishingInst WHERE Id=@Id", cn);
                    command.Parameters.AddWithValue("@NumberOfInsert", NumberOfInsert);
                    if (Magezine1 == "on")
                    {
                        command.Parameters.AddWithValue("@Magezine1", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Magezine1", false);
                    }
                    if (Brochure1 == "on")
                    {
                        command.Parameters.AddWithValue("@Brochure1", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Brochure1", false);
                    }
                    if (CarrierSheet1 == "on")
                    {
                        command.Parameters.AddWithValue("@CarrierSheet1", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@CarrierSheet1", false);
                    }
                    if (Newsletter1 == "on")
                    {
                        command.Parameters.AddWithValue("@Newsletter1", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Newsletter1", false);
                    }
                    if (Statement1 == "on")
                    {
                        command.Parameters.AddWithValue("@Statement1", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Statement1", false);
                    }
                    if (Booklet1 == "on")
                    {
                        command.Parameters.AddWithValue("@Booklet1", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Booklet1", false);
                    }
                    if (!string.IsNullOrEmpty(CommentManualType))
                    {
                        command.Parameters.AddWithValue("@CommentManualType", CommentManualType);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@CommentManualType", DBNull.Value);

                    }

                    if (!string.IsNullOrEmpty(FinishingFormat))
                    {
                        command.Parameters.AddWithValue("@FinishingFormat", FinishingFormat);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@FinishingFormat", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(FoldingType))
                    {
                        command.Parameters.AddWithValue("@FoldingType", FoldingType);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@FoldingType", DBNull.Value);
                    }

                    if (Sealing1 == "on")
                    {
                        command.Parameters.AddWithValue("@Sealing1", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Sealing1", false);
                    }
                    if (Tearing1 == "on")
                    {
                        command.Parameters.AddWithValue("@Tearing1", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Tearing1", false);
                    }
                    if (BarcodeLabel1 == "on")
                    {
                        command.Parameters.AddWithValue("@BarcodeLabel1", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@BarcodeLabel1", false);
                    }
                    if (Cutting1 == "on")
                    {
                        command.Parameters.AddWithValue("@Cutting1", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Cutting1", false);
                    }
                    if (!string.IsNullOrEmpty(StickingOf1))
                    {
                        command.Parameters.AddWithValue("@StickingOf1", StickingOf1);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@StickingOf1", DBNull.Value);
                    }


                    if (AddLabel1 == "on")
                    {
                        command.Parameters.AddWithValue("@AddLabel1", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@AddLabel1", false);
                    }

                    if (Sticker1 == "on")
                    {
                        command.Parameters.AddWithValue("@Sticker1", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Sticker1", false);
                    }


                    if (Chesire1 == "on")
                    {
                        command.Parameters.AddWithValue("@Chesire1", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Chesire1", false);
                    }


                    if (Tuck_In1 == "on")
                    {
                        command.Parameters.AddWithValue("@Tuck_In1", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Tuck_In1", false);
                    }


                    if (Bursting1 == "on")
                    {
                        command.Parameters.AddWithValue("@Bursting1", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Bursting1", false);
                    }


                    if (Sealed1 == "on")
                    {
                        command.Parameters.AddWithValue("@Sealed1", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Sealed1", false);
                    }



                    if (Folding1 == "on")
                    {
                        command.Parameters.AddWithValue("@Folding1", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Folding1", false);
                    }


                    if (Unsealed1 == "on")
                    {
                        command.Parameters.AddWithValue("@Unsealed1", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Unsealed1", false);
                    }


                    if (Letter1 == "on")
                    {
                        command.Parameters.AddWithValue("@Letter1", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Letter1", false);
                    }

                    if (!string.IsNullOrEmpty(FinishingInst))
                    {
                        command.Parameters.AddWithValue("@FinishingInst", FinishingInst);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@FinishingInst", DBNull.Value);
                    }
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                    cn.Close();
                }
            }

            return View();
        }
        else if (set == "ImportantNotes")
        {
            if (!string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(IT_SysNotes) && !string.IsNullOrEmpty(Produc_PlanningNotes) && !string.IsNullOrEmpty(PurchasingNotes))
            {

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn.Open();
                    SqlCommand command;
                    command = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET IT_SysNotes=@IT_SysNotes, Produc_PlanningNotes=@Produc_PlanningNotes, PurchasingNotes=@PurchasingNotes,  EngineeringNotes=@EngineeringNotes, ArtworkNotes=@ArtworkNotes, Acc_BillingNotes=@Acc_BillingNotes, DCPNotes=@DCPNotes, PostingInfo=@PostingInfo ,Status=@Status WHERE Id=@Id", cn);
                    command.Parameters.AddWithValue("@IT_SysNotes", IT_SysNotes);
                    command.Parameters.AddWithValue("@Produc_PlanningNotes", Produc_PlanningNotes);
                    command.Parameters.AddWithValue("@PurchasingNotes", PurchasingNotes);
                    command.Parameters.AddWithValue("@EngineeringNotes", EngineeringNotes);
                    if (!string.IsNullOrEmpty(ArtworkNotes))
                    {
                        command.Parameters.AddWithValue("@ArtworkNotes", ArtworkNotes);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ArtworkNotes", DBNull.Value);
                    }


                    if (!string.IsNullOrEmpty(Acc_BillingNotes))
                    {
                        command.Parameters.AddWithValue("@Acc_BillingNotes", Acc_BillingNotes);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Acc_BillingNotes", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(DCPNotes))
                    {
                        command.Parameters.AddWithValue("@DCPNotes", DCPNotes);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@DCPNotes", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(PostingInfo))
                    {
                        command.Parameters.AddWithValue("@PostingInfo", PostingInfo);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@PostingInfo", DBNull.Value);
                    }
                    command.Parameters.AddWithValue("@Status", "New");
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                    cn.Close();
                }
            }

        }
        else
        {

            return View();



        }



        if (set == "update")
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Customer_Name,Cust_Department,ProductName,Status
                                     FROM [IflowSeed].[dbo].[JobInstruction] 
                                     WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", Id);
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
                            model.Cust_Department = reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            model.ProductName = reader.GetString(2);

                        }
                        if (reader.IsDBNull(3) == false)
                        {
                            model.Status = reader.GetString(3);

                        }

                    }

                    string Str = "<html>";
                    Str += "<head>";
                    Str += "<title></title>";
                    Str += "<style type=text/css>p.MsoNormal{margin-bottom:.0001pt;font-size:11.0pt;font-family:Calibri,sans-serif; margin-left: 0cm;margin-right: 0cm;margin-top: 0cm;}.style1{ width: 246px;}.style2{width: 599px;}.style3{ width: 246px; height: 23px;}.style4 {width: 599px;height: 23px;}table, th, td {border: 1px solid black;</style>";
                    Str += "</head>";
                    Str += "<body>";
                    Str += "<p>There is an </p>";
                    Str += "</br>";
                    Str += "<table style=width:100%>";
                    Str += "<tr>";
                    Str += "<td class=style1>CUSTOMER NAME : </td>";
                    Str += "<td class=style2>" + model.Customer_Name.ToUpper() + "</td>";
                    Str += "</tr>";
                    Str += "<tr>";
                    Str += "<td class=style1>DEPARTMENT : </td>";
                    Str += "<td class=style2>" + model.Cust_Department.ToUpper() + "</td>";
                    Str += "</tr>";
                    Str += "<tr>";
                    Str += "<td class=style1>PRODUCT NAME: </td>";
                    Str += "<td class=style2>" + model.ProductName + "</td>";
                    Str += "</tr>";
                    Str += "<tr>";
                    Str += "<td class=style1>STATUS : </td>";
                    Str += "<td class=style2>" + model.Status + "</td>";
                    Str += "</tr>";
                    Str += "</table>";
                    Str += "</body>";
                    Str += "</html>";

                    bool isEmailSendSuccessfully = false;

                    try
                    {
                        // mailer.Send(mailMessage);
                        string smtpServer = IpSMtp_;
                        //string userName = "m.rizalramli@intercity.com.my";
                        //string password = "Abcd123$";
                        int cdoBasic = 1;
                        int cdoSendUsingPort = 2;
                        System.Web.Mail.MailMessage msg = new System.Web.Mail.MailMessage();
                        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", smtpServer);
                        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", PortSmtp_);
                        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", cdoSendUsingPort);
                        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", cdoBasic);
                        //msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", userName);
                        //msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", password);
                        msg.To = "norfariza@intercity.com.my";
                        //msg.Cc = EmailBy.ToString();
                        msg.From = "i-flow@intercity.com.my";
                        msg.Subject = "NOTIFICATION FOR NEW/UPDATE JI";
                        msg.Body = Str;
                        msg.BodyFormat = MailFormat.Html;
                        SmtpMail.SmtpServer = smtpServer;
                        SmtpMail.Send(msg);

                        isEmailSendSuccessfully = true;
                    }
                    catch
                    {
                        isEmailSendSuccessfully = false;
                    }

                }
                cn.Close();
            }

        }




        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT Id,Customer_Name,ProductName,ServiceLevel,IsSlaCreaditCard,JobClass,IsSetPaper,JobRequest,ExpectedDateCompletionToGpo,QuotationRef,ContractName,Contact_Person,,DeliveryChannel,AccountsQty,ImpressionQty,PagesQty,CycleTerm,MailingDate,
                                    JoiningFiles,TotalRecord,InputFileName,OutputFileName,Sorting,SortingMode,Other,DataPrintingRemark,
                                    ArtworkStatus,PaperStock,TypeCode,Paper,PaperSize,Grammage,MaterialColour,EnvelopeStock,EnvelopeType,EnvelopeSize,EnvelopeGrammage,EnvelopeColour,EnvelopeWindow,EnvWindowOpaque,LabelStock,LabelCutsheet,OthersStock,BalancedMaterial,PlasticStock,PlasticType,PlasticSize,PlasticThickness,
                                    PrintingType,PrintingOrientation,GpoList,RegisterMail,OtherList,BaseStockType,FinishingSize,AdditionalPrintingMark,SortingCriteria,PrintingInstr,SortingInstr,Letter,Brochures_Leaflets,ReplyEnvelope,ImgOnStatement,Booklet,
                                    NumberOfInsert,Magezine1,Brochure1,CarrierSheet1,Newsletter1,Statement1,Booklet1,CommentManualType,FinishingFormat,FoldingType,Sealing1,Tearing1,BarcodeLabel1,Cutting1,StickingOf1,AddLabel1,Sticker1,Chesire1,Tuck_In1,Bursting1,Sealed1,Folding1,Unsealed1,Letter1,FinishingInst,
                                    IT_SysNotes,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes,ArtworkNotes,Acc_BillingNotes,DCPNotes,PostingInfo
                                    FROM [IflowSeed].[dbo].[JobInstruction]
                                    WHERE Id=@Id";
            command.Parameters.AddWithValue("@Id", Id.ToString());
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
                    ViewBag.ServiceLevel = reader.GetString(3);
                }
                if (reader.IsDBNull(4) == false)
                {
                    bool getIsSlaCreaditCard = reader.GetBoolean(4);
                    if (getIsSlaCreaditCard == false)
                    {
                        ViewBag.IsSlaCreaditCard = "";
                    }
                    else
                    {
                        ViewBag.IsSlaCreaditCard = "checked";
                    }
                }
                if (reader.IsDBNull(5) == false)
                {
                    ViewBag.JobClass = reader.GetString(5);
                }
                if (reader.IsDBNull(6) == false)
                {
                    bool getIsSetPaper = reader.GetBoolean(6);
                    if (getIsSetPaper == false)
                    {
                        ViewBag.IsSetPaper = "";
                    }
                    else
                    {
                        ViewBag.IsSetPaper = "checked";
                    }
                }
                if (reader.IsDBNull(7) == false)
                {
                    ViewBag.JobRequest = String.Format("{0:dd/MM/yyyy}", reader.GetDateTime(7));
                }
                if (reader.IsDBNull(8) == false)
                {
                    ViewBag.ExpectedDateCompletionToGpo = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(8));
                }
                if (reader.IsDBNull(9) == false)
                {
                    ViewBag.QuotationRef = reader.GetString(9);
                }
                if (reader.IsDBNull(10) == false)
                {
                    ViewBag.ContractName = reader.GetString(10);
                }
                if (reader.IsDBNull(11) == false)
                {
                    ViewBag.Contact_Person = reader.GetString(11);
                }
                if (reader.IsDBNull(12) == false)
                {
                    ViewBag.JobType = reader.GetString(12);
                }
                if (reader.IsDBNull(13) == false)
                {
                    ViewBag.DeliveryChannel = reader.GetString(13);
                }
                if (reader.IsDBNull(14) == false)
                {
                    ViewBag.AccountsQty = reader.GetString(14);
                }
                if (reader.IsDBNull(15) == false)
                {
                    ViewBag.ImpressionQty = reader.GetString(15);
                }
                if (reader.IsDBNull(16) == false)
                {
                    ViewBag.PagesQty = reader.GetString(16);
                }
                if (reader.IsDBNull(17) == false)
                {
                    ViewBag.CycleTerm = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(17));
                }
                if (reader.IsDBNull(18) == false)
                {
                    ViewBag.MailingDate = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(18));
                }

                if (reader.IsDBNull(19) == false)
                {
                    ViewBag.JoiningFiles = reader.GetString(19);
                }
                if (reader.IsDBNull(20) == false)
                {
                    ViewBag.TotalRecord = reader.GetString(20);
                }
                if (reader.IsDBNull(21) == false)
                {
                    ViewBag.InputFileName = reader.GetString(21);
                }
                if (reader.IsDBNull(22) == false)
                {
                    ViewBag.OutputFileName = reader.GetString(22);
                }
                if (reader.IsDBNull(23) == false)
                {
                    ViewBag.Sorting = reader.GetString(23);
                }
                if (reader.IsDBNull(24) == false)
                {
                    ViewBag.SortingMode = reader.GetString(24);
                }
                if (reader.IsDBNull(25) == false)
                {
                    ViewBag.Other = reader.GetString(25);
                }
                if (reader.IsDBNull(26) == false)
                {
                    ViewBag.DataPrintingRemark = reader.GetString(26);
                }
                if (reader.IsDBNull(27) == false)
                {
                    ViewBag.ArtworkStatus = reader.GetString(27);
                }
                if (reader.IsDBNull(28) == false)
                {
                    ViewBag.PaperStock = reader.GetString(28);
                }
                if (reader.IsDBNull(29) == false)
                {
                    ViewBag.TypeCode = reader.GetString(29);
                }
                if (reader.IsDBNull(30) == false)
                {
                    ViewBag.Paper = reader.GetString(30);
                }
                if (reader.IsDBNull(31) == false)
                {
                    ViewBag.PaperSize = reader.GetString(31);
                }
                if (reader.IsDBNull(32) == false)
                {
                    ViewBag.Grammage = reader.GetString(32);
                }
                if (reader.IsDBNull(33) == false)
                {
                    ViewBag.MaterialColour = reader.GetString(33);
                }
                if (reader.IsDBNull(34) == false)
                {
                    ViewBag.EnvelopeStock = reader.GetString(34);
                }
                if (reader.IsDBNull(35) == false)
                {
                    ViewBag.EnvelopeType = reader.GetString(35);
                }
                if (reader.IsDBNull(36) == false)
                {
                    ViewBag.EnvelopeSize = reader.GetString(36);
                }
                if (reader.IsDBNull(37) == false)
                {
                    ViewBag.EnvelopeGrammage = reader.GetString(37);
                }
                if (reader.IsDBNull(38) == false)
                {
                    ViewBag.EnvelopeColour = reader.GetString(38);
                }
                if (reader.IsDBNull(39) == false)
                {
                    ViewBag.EnvelopeWindow = reader.GetString(39);
                }
                if (reader.IsDBNull(40) == false)
                {
                    ViewBag.EnvWindowOpaque = reader.GetString(40);
                }
                if (reader.IsDBNull(41) == false)
                {
                    ViewBag.LabelStock = reader.GetString(41);
                }
                if (reader.IsDBNull(42) == false)
                {
                    ViewBag.LabelCutsheet = reader.GetString(42);
                }
                if (reader.IsDBNull(43) == false)
                {
                    ViewBag.OthersStock = reader.GetString(43);
                }
                if (reader.IsDBNull(44) == false)
                {
                    ViewBag.BalancedMaterial = reader.GetString(44);
                }
                if (reader.IsDBNull(45) == false)
                {
                    ViewBag.PlasticStock = reader.GetString(45);
                }
                if (reader.IsDBNull(46) == false)
                {
                    ViewBag.PlasticType = reader.GetString(46);
                }
                if (reader.IsDBNull(47) == false)
                {
                    ViewBag.PlasticSize = reader.GetString(47);
                }
                if (reader.IsDBNull(48) == false)
                {
                    ViewBag.PlasticThickness = reader.GetString(48);
                }
                if (reader.IsDBNull(49) == false)
                {
                    ViewBag.PrintingType = reader.GetString(49);
                }
                if (reader.IsDBNull(50) == false)
                {
                    ViewBag.PrintingOrientation = reader.GetString(50);
                }
                if (reader.IsDBNull(51) == false)
                {
                    bool getGpoList = reader.GetBoolean(51);
                    if (getGpoList == false)
                    {
                        ViewBag.GpoList = "";
                    }
                    else
                    {
                        ViewBag.GpoList = "checked";
                    }
                }
                if (reader.IsDBNull(52) == false)
                {
                    bool getRegisterMail = reader.GetBoolean(52);
                    if (getRegisterMail == false)
                    {
                        ViewBag.RegisterMail = "";
                    }
                    else
                    {
                        ViewBag.RegisterMail = "checked";
                    }
                }
                if (reader.IsDBNull(53) == false)
                {
                    ViewBag.OtherList = reader.GetString(53);
                }
                if (reader.IsDBNull(54) == false)
                {
                    ViewBag.BaseStockType = reader.GetString(54);
                }
                if (reader.IsDBNull(55) == false)
                {
                    ViewBag.FinishingSize = reader.GetString(55);
                }
                if (reader.IsDBNull(56) == false)
                {
                    ViewBag.AdditionalPrintingMark = reader.GetString(56);
                }
                if (reader.IsDBNull(57) == false)
                {
                    ViewBag.SortingCriteria = reader.GetString(57);
                }
                if (reader.IsDBNull(58) == false)
                {
                    ViewBag.PrintingInstr = reader.GetString(58);
                }
                if (reader.IsDBNull(59) == false)
                {
                    ViewBag.SortingInstr = reader.GetString(59);
                }
                if (reader.IsDBNull(60) == false)
                {
                    bool getLetter = reader.GetBoolean(60);
                    if (getLetter == false)
                    {
                        ViewBag.Letter = "";
                    }
                    else
                    {
                        ViewBag.Letter = "checked";
                    }
                }
                if (reader.IsDBNull(61) == false)
                {
                    bool getBrochures_Leaflets = reader.GetBoolean(61);
                    if (getBrochures_Leaflets == false)
                    {
                        ViewBag.Brochures_Leaflets = "";
                    }
                    else
                    {
                        ViewBag.Brochures_Leaflets = "checked";
                    }
                }
                if (reader.IsDBNull(62) == false)
                {
                    bool getReplyEnvelope = reader.GetBoolean(62);
                    if (getReplyEnvelope == false)
                    {
                        ViewBag.ReplyEnvelope = "";
                    }
                    else
                    {
                        ViewBag.ReplyEnvelope = "checked";
                    }
                }
                if (reader.IsDBNull(63) == false)
                {
                    bool getImgOnStatement = reader.GetBoolean(63);
                    if (getImgOnStatement == false)
                    {
                        ViewBag.ImgOnStatement = "";
                    }
                    else
                    {
                        ViewBag.ImgOnStatement = "checked";
                    }
                }
                if (reader.IsDBNull(64) == false)
                {
                    bool getBooklet = reader.GetBoolean(64);
                    if (getBooklet == false)
                    {
                        ViewBag.Booklet = "";
                    }
                    else
                    {
                        ViewBag.Booklet = "checked";
                    }
                }
                if (reader.IsDBNull(65) == false)
                {
                    ViewBag.NumberOfInsert = reader.GetString(65);
                }
                if (reader.IsDBNull(66) == false)
                {
                    bool getMagezine1 = reader.GetBoolean(66);
                    if (getMagezine1 == false)
                    {
                        ViewBag.Magezine1 = "";
                    }
                    else
                    {
                        ViewBag.Magezine1 = "checked";
                    }
                }
                if (reader.IsDBNull(67) == false)
                {
                    bool getBrochure1 = reader.GetBoolean(67);
                    if (getBrochure1 == false)
                    {
                        ViewBag.Brochure1 = "";
                    }
                    else
                    {
                        ViewBag.Brochure1 = "checked";
                    }
                }
                if (reader.IsDBNull(68) == false)
                {
                    bool getCarrierSheet1 = reader.GetBoolean(68);
                    if (getCarrierSheet1 == false)
                    {
                        ViewBag.CarrierSheet1 = "";
                    }
                    else
                    {
                        ViewBag.CarrierSheet1 = "checked";
                    }
                }
                if (reader.IsDBNull(69) == false)
                {
                    bool getNewsletter1 = reader.GetBoolean(69);
                    if (getNewsletter1 == false)
                    {
                        ViewBag.Newsletter1 = "";
                    }
                    else
                    {
                        ViewBag.Newsletter1 = "checked";
                    }
                }
                if (reader.IsDBNull(70) == false)
                {
                    bool getStatement1 = reader.GetBoolean(70);
                    if (getStatement1 == false)
                    {
                        ViewBag.Statement1 = "";
                    }
                    else
                    {
                        ViewBag.Statement1 = "checked";
                    }
                }
                if (reader.IsDBNull(71) == false)
                {
                    bool getBooklet1 = reader.GetBoolean(71);
                    if (getBooklet1 == false)
                    {
                        ViewBag.Booklet1 = "";
                    }
                    else
                    {
                        ViewBag.Booklet1 = "checked";
                    }
                }
                if (reader.IsDBNull(72) == false)
                {
                    ViewBag.CommentManualType = reader.GetString(72);
                }
                if (reader.IsDBNull(73) == false)
                {
                    ViewBag.FinishingFormat = reader.GetString(73);
                }
                if (reader.IsDBNull(74) == false)
                {
                    ViewBag.FoldingType = reader.GetString(74);
                }
                if (reader.IsDBNull(75) == false)
                {
                    bool getSealing1 = reader.GetBoolean(75);
                    if (getSealing1 == false)
                    {
                        ViewBag.Sealing1 = "";
                    }
                    else
                    {
                        ViewBag.Sealing1 = "checked";
                    }
                }
                if (reader.IsDBNull(76) == false)
                {
                    bool getTearing1 = reader.GetBoolean(76);
                    if (getTearing1 == false)
                    {
                        ViewBag.Tearing1 = "";
                    }
                    else
                    {
                        ViewBag.Tearing1 = "checked";
                    }
                }
                if (reader.IsDBNull(77) == false)
                {
                    bool getBarcodeLabel1 = reader.GetBoolean(77);
                    if (getBarcodeLabel1 == false)
                    {
                        ViewBag.BarcodeLabel1 = "";
                    }
                    else
                    {
                        ViewBag.BarcodeLabel1 = "checked";
                    }
                }
                if (reader.IsDBNull(78) == false)
                {
                    bool getCutting1 = reader.GetBoolean(78);
                    if (getCutting1 == false)
                    {
                        ViewBag.Cutting1 = "";
                    }
                    else
                    {
                        ViewBag.Cutting1 = "checked";
                    }
                }
                if (reader.IsDBNull(79) == false)
                {
                    ViewBag.StickingOf1 = reader.GetString(79);
                }
                if (reader.IsDBNull(80) == false)
                {
                    bool getAddLabel1 = reader.GetBoolean(80);
                    if (getAddLabel1 == false)
                    {
                        ViewBag.AddLabel1 = "";
                    }
                    else
                    {
                        ViewBag.AddLabel1 = "checked";
                    }
                }
                if (reader.IsDBNull(81) == false)
                {
                    bool getSticker1 = reader.GetBoolean(81);
                    if (getSticker1 == false)
                    {
                        ViewBag.Sticker1 = "";
                    }
                    else
                    {
                        ViewBag.Sticker1 = "checked";
                    }
                }
                if (reader.IsDBNull(82) == false)
                {
                    bool getChesire1 = reader.GetBoolean(82);
                    if (getChesire1 == false)
                    {
                        ViewBag.Chesire1 = "";
                    }
                    else
                    {
                        ViewBag.Chesire1 = "checked";
                    }
                }
                if (reader.IsDBNull(83) == false)
                {
                    bool getTuck_In1 = reader.GetBoolean(83);
                    if (getTuck_In1 == false)
                    {
                        ViewBag.Tuck_In1 = "";
                    }
                    else
                    {
                        ViewBag.Tuck_In1 = "checked";
                    }
                }
                if (reader.IsDBNull(84) == false)
                {
                    bool getBursting1 = reader.GetBoolean(84);
                    if (getBursting1 == false)
                    {
                        ViewBag.Bursting1 = "";
                    }
                    else
                    {
                        ViewBag.Bursting1 = "checked";
                    }
                }
                if (reader.IsDBNull(85) == false)
                {
                    bool getSealed1 = reader.GetBoolean(85);
                    if (getSealed1 == false)
                    {
                        ViewBag.Sealed1 = "";
                    }
                    else
                    {
                        ViewBag.Sealed1 = "checked";
                    }
                }
                if (reader.IsDBNull(86) == false)
                {
                    bool getFolding1 = reader.GetBoolean(86);
                    if (getFolding1 == false)
                    {
                        ViewBag.Folding1 = "";
                    }
                    else
                    {
                        ViewBag.Folding1 = "checked";
                    }
                }
                if (reader.IsDBNull(87) == false)
                {
                    bool getUnsealed1 = reader.GetBoolean(87);
                    if (getUnsealed1 == false)
                    {
                        ViewBag.Unsealed1 = "";
                    }
                    else
                    {
                        ViewBag.Unsealed1 = "checked";
                    }
                }
                if (reader.IsDBNull(88) == false)
                {
                    bool getLetter1 = reader.GetBoolean(88);
                    if (getLetter1 == false)
                    {
                        ViewBag.Letter1 = "";
                    }
                    else
                    {
                        ViewBag.Letter1 = "checked";
                    }
                }
                if (reader.IsDBNull(89) == false)
                {
                    ViewBag.FinishingInst = reader.GetString(89);
                }
                if (reader.IsDBNull(90) == false)
                {
                    ViewBag.IT_SysNotes = reader.GetString(90);
                }
                if (reader.IsDBNull(91) == false)
                {
                    ViewBag.Produc_PlanningNotes = reader.GetString(91);
                }
                if (reader.IsDBNull(92) == false)
                {
                    ViewBag.PurchasingNotes = reader.GetString(92);
                }
                if (reader.IsDBNull(93) == false)
                {
                    ViewBag.EngineeringNotes = reader.GetString(93);
                }
                if (reader.IsDBNull(94) == false)
                {
                    ViewBag.ArtworkNotes = reader.GetString(94);
                }
                if (reader.IsDBNull(95) == false)
                {
                    ViewBag.Acc_BillingNotes = reader.GetString(95);
                }
                if (reader.IsDBNull(96) == false)
                {
                    ViewBag.DCPNotes = reader.GetString(96);
                }
                if (reader.IsDBNull(97) == false)
                {
                    ViewBag.PostingInfo = reader.GetString(97);
                }

            }
            cn.Close();
            return View();

        }
    }


    //submitActive


    [ValidateInput(false)]
    public ActionResult SubmitToActive(JIHistory JIHistory, string Id, string set, string tabs, string Customer_Name, string ProductName, string JobSheetNo,
                                             string SalesExecutiveBy, string Status,
                                             string ServiceLevel, string IsSlaCreaditCard, string JobClass, string IsSetPaper,
                                             string JobRequest, string ExpectedDateCompletionToGpoTxt, string QuotationRef, string ContractName,
                                             string Contact_Person, string JobType, string DeliveryChannel, string AccountsQty, string ImpressionQty,
                                             string PagesQty, string CycleTermTxt, string MailingDateTxt,
                                             string JoiningFiles, string TotalRecord, string InputFileName, string OutputFileName, string Sorting,
                                             string SortingMode, string Other, string DataPrintingRemark,
                                             string ArtworkStatus, string PaperStock, string TypeCode, string Paper, string PaperSize,
                                             string Grammage, string MaterialColour, string EnvelopeStock, string EnvelopeType, string EnvelopeSize,
                                             string EnvelopeGrammage, string EnvelopeColour, string EnvelopeWindow, string EnvWindowOpaque,
                                             string LabelStock, string LabelCutsheet, string OthersStock, string BalancedMaterial,
                                             string PlasticStock, string PlasticType, string PlasticSize, string PlasticThickness,
                                             string PrintingType, string PrintingOrientation, string GpoList, string RegisterMail,
                                             string OtherList, string BaseStockType, string FinishingSize, string AdditionalPrintingMark,
                                             string SortingCriteria, string PrintingInstr, string SortingInstr,
                                             string Letter, string Brochures_Leaflets, string ExpectedDateCompletionToGpo,
                                             string ReplyEnvelope, string ImgOnStatement, string Booklet,
                                             string NumberOfInsert, string Magezine1, string Brochure1, string CarrierSheet1, string Newsletter1,
                                             string Statement1, string Booklet1, string CommentManualType, string FinishingFormat,
                                             string FoldingType, string Sealing1, string Tearing1, string BarcodeLabel1, string Cutting1,
                                             string StickingOf1, string AddLabel1, string Sticker1, string Chesire1, string Tuck_In1,
                                             string Bursting1, string Sealed1, string Folding1, string Unsealed1, string Letter1, string FinishingInst,
                                             string IT_SysNotes, string Produc_PlanningNotes, string PurchasingNotes, string EngineeringNotes, string Cust_Department, string ReffSub, string ArtworkNotes, string Acc_BillingNotes,
                                             string DCPNotes, string PostingInfo)
    {



        if (set == "submit")
        {

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Id,Customer_Name,ProductName,JobSheetNo,Status,
                                           ServiceLevel,IsSlaCreaditCard,JobClass,IsSetPaper,JobRequest,
                                           ExpectedDateCompletionToGpo,QuotationRef,
                                           JobType,DeliveryChannel,AccountsQty,ImpressionQty,
                                           PagesQty,CycleTerm,MailingDate,
                                           JoiningFiles,TotalRecord,InputFileName,OutputFileName,Sorting,
                                           SortingMode,Other,DataPrintingRemark,
                                           ArtworkStatus,PaperStock,
                                           Grammage,MaterialColour,EnvelopeStock,EnvelopeType,EnvelopeSize,
                                           EnvelopeGrammage,EnvelopeColour,EnvelopeWindow,EnvWindowOpaque,
                                           LabelStock,LabelCutsheet,OthersStock,BalancedMaterial,
                                           PlasticStock,PlasticType,PlasticSize,PlasticThickness,
                                           PrintingType,PrintingOrientation,GpoList,RegisterMail,
                                           OtherList,BaseStockType,FinishingSize,AdditionalPrintingMark,
                                           SortingCriteria,PrintingInstr,SortingInstr,Letter,Brochures_Leaflets,
                                           ReplyEnvelope,ImgOnStatement,Booklet,
                                           NumberOfInsert,FinishingInst,
                                           IT_SysNotes,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes,
                                           ArtworkNotes, Acc_BillingNotes, DCPNotes, PostingInfo,Cust_Department,ActiveSts
                                       FROM [IflowSeed].[dbo].[JobBatchInfo]
                                    WHERE Id=@Id";


                
                command.Parameters.AddWithValue("@Id", Id.ToString());
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JIHistory model = new JIHistory();
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
                            ViewBag.JobSheetNo = reader.GetString(3);
                        }

                        if (reader.IsDBNull(4) == false)
                        {
                            ViewBag.Status = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            ViewBag.ServiceLevel = reader.GetString(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            ViewBag.SlaCreaditCard = reader.GetBoolean(6);

                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            ViewBag.JobClass = reader.GetString(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            ViewBag.IsSetPaper = reader.GetBoolean(8);

                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            ViewBag.JobRequest = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(9));
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            ViewBag.ExpectedDateCompletionToGpo = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(10));
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            ViewBag.QuotationRef = reader.GetString(11);
                        }


                        if (reader.IsDBNull(12) == false)
                        {
                            ViewBag.JobType = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            ViewBag.DeliveryChannel = reader.GetString(13);
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            ViewBag.AccountsQty = reader.GetString(14);
                        }
                        if (reader.IsDBNull(15) == false)
                        {
                            ViewBag.ImpressionQty = reader.GetString(15);
                        }
                        if (reader.IsDBNull(16) == false)
                        {
                            ViewBag.PagesQty = reader.GetString(16);
                        }
                        if (reader.IsDBNull(17) == false)
                        {
                            ViewBag.CycleTerm = reader.GetDateTime(17);
                        }
                        if (reader.IsDBNull(18) == false)
                        {
                            ViewBag.MailingDate = (DateTime)reader.GetDateTime(18);
                        }

                        if (reader.IsDBNull(19) == false)
                        {
                            ViewBag.JoiningFiles = reader.GetString(19);
                        }
                        if (reader.IsDBNull(20) == false)
                        {
                            ViewBag.TotalRecord = reader.GetString(20);
                        }
                        if (reader.IsDBNull(21) == false)
                        {
                            ViewBag.InputFileName = reader.GetString(21);
                        }
                        if (reader.IsDBNull(22) == false)
                        {
                            ViewBag.OutputFileName = reader.GetString(22);
                        }
                        if (reader.IsDBNull(23) == false)
                        {
                            ViewBag.Sorting = reader.GetString(23);
                        }
                        if (reader.IsDBNull(24) == false)
                        {
                            ViewBag.SortingMode = reader.GetString(24);
                        }
                        if (reader.IsDBNull(25) == false)
                        {
                            ViewBag.Other = reader.GetString(25);
                        }
                        if (reader.IsDBNull(26) == false)
                        {
                            ViewBag.DataPrintingRemark = reader.GetString(26);
                        }
                        if (reader.IsDBNull(27) == false)
                        {
                            ViewBag.ArtworkStatus = reader.GetString(27);
                        }
                        if (reader.IsDBNull(28) == false)
                        {
                            ViewBag.PaperStock = reader.GetString(28);
                        }


                        if (reader.IsDBNull(29) == false)
                        {
                            ViewBag.Grammage = reader.GetString(29);
                        }
                        if (reader.IsDBNull(30) == false)
                        {
                            ViewBag.MaterialColour = reader.GetString(30);
                        }
                        if (reader.IsDBNull(31) == false)
                        {
                            ViewBag.EnvelopeStock = reader.GetString(31);
                        }
                        if (reader.IsDBNull(32) == false)
                        {
                            ViewBag.EnvelopeType = reader.GetString(32);
                        }
                        if (reader.IsDBNull(33) == false)
                        {
                            ViewBag.EnvelopeSize = reader.GetString(33);
                        }
                        if (reader.IsDBNull(34) == false)
                        {
                            ViewBag.EnvelopeGrammage = reader.GetString(34);
                        }
                        if (reader.IsDBNull(35) == false)
                        {
                            ViewBag.EnvelopeColour = reader.GetString(35);
                        }
                        if (reader.IsDBNull(36) == false)
                        {
                            ViewBag.EnvelopeWindow = reader.GetString(36);
                        }
                        if (reader.IsDBNull(37) == false)
                        {
                            ViewBag.EnvWindowOpaque = reader.GetString(37);
                        }
                        if (reader.IsDBNull(38) == false)
                        {
                            ViewBag.LabelStock = reader.GetString(38);
                        }
                        if (reader.IsDBNull(39) == false)
                        {
                            ViewBag.LabelCutsheet = reader.GetString(39);
                        }
                        if (reader.IsDBNull(40) == false)
                        {
                            ViewBag.OthersStock = reader.GetString(40);
                        }
                        if (reader.IsDBNull(41) == false)
                        {
                            ViewBag.BalancedMaterial = reader.GetString(41);
                        }
                        if (reader.IsDBNull(42) == false)
                        {
                            ViewBag.PlasticStock = reader.GetString(42);
                        }
                        if (reader.IsDBNull(43) == false)
                        {
                            ViewBag.PlasticType = reader.GetString(43);
                        }
                        if (reader.IsDBNull(44) == false)
                        {
                            ViewBag.PlasticSize = reader.GetString(44);
                        }
                        if (reader.IsDBNull(45) == false)
                        {
                            ViewBag.PlasticThickness = reader.GetString(45);
                        }
                        if (reader.IsDBNull(46) == false)
                        {
                            ViewBag.PrintingType = reader.GetString(46);
                        }
                        if (reader.IsDBNull(47) == false)
                        {
                            ViewBag.PrintingOrientation = reader.GetString(47);
                        }
                        if (reader.IsDBNull(48) == false)
                        {
                            ViewBag.GpoList = reader.GetBoolean(48);
                        }
                        if (reader.IsDBNull(49) == false)
                        {
                            ViewBag.RegisterMail = reader.GetBoolean(49);
                        }
                        if (reader.IsDBNull(50) == false)
                        {
                            ViewBag.OtherList = reader.GetString(50);
                        }
                        if (reader.IsDBNull(51) == false)
                        {
                            ViewBag.BaseStockType = reader.GetString(51);
                        }
                        if (reader.IsDBNull(52) == false)
                        {
                            ViewBag.FinishingSize = reader.GetString(52);
                        }
                        if (reader.IsDBNull(53) == false)
                        {
                            ViewBag.AdditionalPrintingMark = reader.GetString(53);
                        }
                        if (reader.IsDBNull(54) == false)
                        {
                            ViewBag.SortingCriteria = reader.GetString(54);
                        }
                        if (reader.IsDBNull(55) == false)
                        {
                            ViewBag.PrintingInstr = reader.GetString(55);
                        }
                        if (reader.IsDBNull(56) == false)
                        {
                            ViewBag.SortingInstr = reader.GetString(56);
                        }
                        if (reader.IsDBNull(57) == false)
                        {
                            ViewBag.Letter = reader.GetBoolean(57);
                        }
                        if (reader.IsDBNull(58) == false)
                        {
                            ViewBag.Brochures_Leaflets = reader.GetBoolean(58);
                        }
                        if (reader.IsDBNull(59) == false)
                        {
                            ViewBag.ReplyEnvelope = reader.GetBoolean(59);
                        }
                        if (reader.IsDBNull(60) == false)
                        {
                            ViewBag.ImgOnStatement = reader.GetBoolean(60);
                        }
                        if (reader.IsDBNull(61) == false)
                        {
                            ViewBag.Booklet = reader.GetBoolean(61);
                        }
                        if (reader.IsDBNull(62) == false)
                        {
                            ViewBag.NumberOfInsert = reader.GetString(62);
                        }


                        if (reader.IsDBNull(63) == false)
                        {
                            ViewBag.FinishingInst = reader.GetString(63);
                        }
                        if (reader.IsDBNull(64) == false)
                        {
                            ViewBag.IT_SysNotes = reader.GetString(64);
                        }
                        if (reader.IsDBNull(65) == false)
                        {
                            ViewBag.Produc_PlanningNotes = reader.GetString(65);
                        }
                        if (reader.IsDBNull(66) == false)
                        {
                            ViewBag.PurchasingNotes = reader.GetString(66);
                        }
                        if (reader.IsDBNull(67) == false)
                        {
                            ViewBag.EngineeringNotes = reader.GetString(67);
                        }
                        if (reader.IsDBNull(68) == false)
                        {
                            ViewBag.ArtworkNotes = reader.GetString(68);
                        }
                        if (reader.IsDBNull(69) == false)
                        {
                            ViewBag.Acc_BillingNotes = reader.GetString(69);
                        }
                        if (reader.IsDBNull(70) == false)
                        {
                            ViewBag.DCPNotes = reader.GetString(70);
                        }
                        if (reader.IsDBNull(71) == false)
                        {
                            ViewBag.PostingInfo = reader.GetString(71);
                        }
                        if (reader.IsDBNull(72) == false)
                        {
                            ViewBag.Cust_Department = reader.GetString(72);
                        }
                        if (reader.IsDBNull(73) == false)
                        {
                            ViewBag.ActiveSts = reader.GetString(73);
                        }
                    }

                }
                cn.Close();

            }

            if (!string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(JobClass) && !string.IsNullOrEmpty(JobType) && !string.IsNullOrEmpty(DeliveryChannel) && !string.IsNullOrEmpty(ServiceLevel) && set == "submit")

            {

                string ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    Guid Idx = Guid.NewGuid();
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                    cn.Open();
                    SqlCommand command;
                    command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[JobInstruction] (Id,  Customer_Name, Cust_Department, ProductName, CreatedOn, ModifiedOn, Status,reffSub,Iscompleted,JobSheetNo,ServiceLevel," +
                        "JobClass ,JobRequest ,ExpectedDateCompletionToGpo,QuotationRef ,JobType ,AccountsQty,ImpressionQty,PagesQty ,JoiningFiles,TotalRecord ,InputFileName,OutputFileName,Sorting ,SortingMode,Other ," +
                        "DataPrintingRemark,PrintingType ,PrintingOrientation ,GpoList ,OtherList,BaseStockType ,FinishingSize,AdditionalPrintingMark,SortingCriteria,PrintingInstr,SortingInstr,Letter,Brochures_Leaflets," +
                        "ReplyEnvelope,ImgOnStatement,Booklet,IT_SysNotes,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes,ArtworkNotes,Acc_BillingNotes,DCPNotes, PostingInfo ) values (@Id,  @Customer_Name, @Cust_Department, @ProductName, @CreatedOn,@ModifiedOn, @Status,@reffSub,@Iscompleted,@JobSheetNo,@ServiceLevel,@JobClass ,@JobRequest ,@ExpectedDateCompletionToGpo,@QuotationRef ,@JobType ,@AccountsQty,@ImpressionQty,@PagesQty ,@JoiningFiles,@TotalRecord ,@InputFileName,@OutputFileName,@Sorting ,@SortingMode,@Other ,@DataPrintingRemark,@PrintingType ,@PrintingOrientation ,@GpoList ,@OtherList,@BaseStockType ,@FinishingSize,@AdditionalPrintingMark,@SortingCriteria,@PrintingInstr,@SortingInstr,@Letter,@Brochures_Leaflets,@ReplyEnvelope,@ImgOnStatement,@Booklet,@IT_SysNotes,@Produc_PlanningNotes,@PurchasingNotes,@EngineeringNotes,@ArtworkNotes,@Acc_BillingNotes,@DCPNotes,@PostingInfo)", cn);
                    command.Parameters.AddWithValue("@Id", Idx);
                    command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                    if (!string.IsNullOrEmpty(Cust_Department))
                    {
                        command.Parameters.AddWithValue("@Cust_Department", Cust_Department);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Cust_Department", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(ProductName))
                    {
                        command.Parameters.AddWithValue("@ProductName", ProductName);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ProductName", DBNull.Value);
                    }
                    command.Parameters.AddWithValue("@CreatedOn", createdOn);
                    command.Parameters.AddWithValue("@ModifiedOn", ModifiedOn);
                    command.Parameters.AddWithValue("@Status", "New");
                    if (!string.IsNullOrEmpty(ReffSub))
                    {
                        command.Parameters.AddWithValue("@reffSub", ReffSub);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@reffSub", DBNull.Value);
                    }

                    command.Parameters.AddWithValue("@Iscompleted", "0");
                    command.Parameters.AddWithValue("@JobSheetNo", JobSheetNo);
                    if (!string.IsNullOrEmpty(ServiceLevel))
                    {
                        command.Parameters.AddWithValue("@ServiceLevel", ServiceLevel);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ServiceLevel", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(JobClass))
                    {
                        command.Parameters.AddWithValue("@JobClass", JobClass);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@JobClass", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(JobRequest))
                    {
                        command.Parameters.AddWithValue("@JobRequest", JobRequest);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@JobRequest", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(ExpectedDateCompletionToGpo))
                    {

                        command.Parameters.AddWithValue("@ExpectedDateCompletionToGpo", ExpectedDateCompletionToGpo);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ExpectedDateCompletionToGpo", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(QuotationRef))
                    {
                        command.Parameters.AddWithValue("@QuotationRef", QuotationRef);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@QuotationRef", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(JobType))
                    {
                        command.Parameters.AddWithValue("@JobType", JobType);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@JobType", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(AccountsQty))
                    {
                        command.Parameters.AddWithValue("@AccountsQty", AccountsQty);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@AccountsQty", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(ImpressionQty))
                    {
                        command.Parameters.AddWithValue("@ImpressionQty", ImpressionQty);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ImpressionQty", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(PagesQty))
                    {
                        command.Parameters.AddWithValue("@PagesQty", PagesQty);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@PagesQty", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(JoiningFiles))
                    {
                        command.Parameters.AddWithValue("@JoiningFiles", JoiningFiles);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@JoiningFiles", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(TotalRecord))
                    {
                        command.Parameters.AddWithValue("@TotalRecord", TotalRecord);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@TotalRecord", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(InputFileName))
                    {
                        command.Parameters.AddWithValue("@InputFileName", InputFileName);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@InputFileName", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(OutputFileName))
                    {
                        command.Parameters.AddWithValue("@OutputFileName", OutputFileName);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@OutputFileName", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(Sorting))
                    {
                        command.Parameters.AddWithValue("@Sorting", Sorting);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Sorting", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(SortingMode))
                    {
                        command.Parameters.AddWithValue("@SortingMode", SortingMode);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@SortingMode", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(Other))
                    {
                        command.Parameters.AddWithValue("@Other", Other);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Other", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(DataPrintingRemark))
                    {
                        command.Parameters.AddWithValue("@DataPrintingRemark", DataPrintingRemark);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@DataPrintingRemark", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(PrintingType))
                    {
                        command.Parameters.AddWithValue("@PrintingType", PrintingType);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@PrintingType", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(PrintingOrientation))
                    {
                        command.Parameters.AddWithValue("@PrintingOrientation", PrintingOrientation);

                    }
                    else
                    {
                        command.Parameters.AddWithValue("@PrintingOrientation", DBNull.Value);
                    }


                    if (GpoList == "on")
                    {
                        command.Parameters.AddWithValue("@GpoList", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@GpoList", false);
                    }
                    if (RegisterMail == "on")
                    {
                        command.Parameters.AddWithValue("@RegisterMail", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RegisterMail", false);
                    }

                    if (!string.IsNullOrEmpty(OtherList))
                    {
                        command.Parameters.AddWithValue("@OtherList", OtherList);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@OtherList", DBNull.Value);

                    }
                    if (!string.IsNullOrEmpty(BaseStockType))
                    {
                        command.Parameters.AddWithValue("@BaseStockType", BaseStockType);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@BaseStockType", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(FinishingSize))
                    {
                        command.Parameters.AddWithValue("@FinishingSize", FinishingSize);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@FinishingSize", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(AdditionalPrintingMark))
                    {
                        command.Parameters.AddWithValue("@AdditionalPrintingMark", AdditionalPrintingMark);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@AdditionalPrintingMark", DBNull.Value);
                    }


                    if (!string.IsNullOrEmpty(SortingCriteria))
                    {

                        command.Parameters.AddWithValue("@SortingCriteria", false);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@SortingCriteria", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(PrintingInstr))
                    {

                        command.Parameters.AddWithValue("@PrintingInstr", PrintingInstr);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@PrintingInstr", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(SortingInstr))
                    {
                        command.Parameters.AddWithValue("@SortingInstr", SortingInstr);

                    }
                    else
                    {
                        command.Parameters.AddWithValue("@SortingInstr", DBNull.Value);
                    }
                    if (Letter == "on")
                    {
                        command.Parameters.AddWithValue("@Letter", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Letter", false);
                    }
                    if (Brochures_Leaflets == "on")
                    {
                        command.Parameters.AddWithValue("@Brochures_Leaflets", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Brochures_Leaflets", false);
                    }
                    if (ReplyEnvelope == "on")
                    {
                        command.Parameters.AddWithValue("@ReplyEnvelope", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ReplyEnvelope", false);
                    }
                    if (ImgOnStatement == "on")
                    {
                        command.Parameters.AddWithValue("@ImgOnStatement", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ImgOnStatement", false);
                    }
                    if (Booklet == "on")
                    {
                        command.Parameters.AddWithValue("@Booklet", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Booklet", false);
                    }
                    if (!string.IsNullOrEmpty(IT_SysNotes))
                    {
                        command.Parameters.AddWithValue("@IT_SysNotes", IT_SysNotes);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@IT_SysNotes", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(Produc_PlanningNotes))
                    {
                        command.Parameters.AddWithValue("@Produc_PlanningNotes", Produc_PlanningNotes);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Produc_PlanningNotes", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(PurchasingNotes))
                    {
                        command.Parameters.AddWithValue("@PurchasingNotes", PurchasingNotes);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@PurchasingNotes", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(EngineeringNotes))
                    {
                        command.Parameters.AddWithValue("@EngineeringNotes", EngineeringNotes);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@EngineeringNotes", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(ArtworkNotes))
                    {
                        command.Parameters.AddWithValue("@ArtworkNotes", ArtworkNotes);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ArtworkNotes", DBNull.Value);
                    }


                    if (!string.IsNullOrEmpty(Acc_BillingNotes))
                    {
                        command.Parameters.AddWithValue("@Acc_BillingNotes", Acc_BillingNotes);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Acc_BillingNotes", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(DCPNotes))
                    {
                        command.Parameters.AddWithValue("@DCPNotes", DCPNotes);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@DCPNotes", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(PostingInfo))
                    {
                        command.Parameters.AddWithValue("@PostingInfo", PostingInfo);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@PostingInfo", DBNull.Value);
                    }


                    command.ExecuteNonQuery();


                    SqlCommand comm = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobBatchInfo] SET ModifiedOn=@ModifiedOn,ActiveSts=@ActiveSts WHERE Id=@Id", cn);
                    comm.Parameters.AddWithValue("@ModifiedOn", ModifiedOn);
                    comm.Parameters.AddWithValue("@ActiveSts", "ACTIVE");
                    comm.Parameters.AddWithValue("@Id", Id);
                    comm.ExecuteNonQuery();
                    cn.Close();
                }

            }

            else
            {
                TempData["msg"] = "<script>alert('PLEASE CHECK YOUR JI  DETAILS!');</script>";
            }

        }
        return RedirectToAction("ManageJIHistory", "MBD", new { Id = Id.ToString() });
    }


    public ActionResult ViewJI(string Id, string set, string JobInstructionId, string tabs, string Customer_Name, string ProductName, string JobSheetNo,
                           string SalesExecutiveBy, string Status,
                           string ServiceLevel, string IsSlaCreaditCard, string JobClass, string IsSetPaper,
                           string JobRequest, string ExpectedDateCompletionToGpo, string QuotationRef, string ContractName,
                           string Contact_Person, string JobType, string DeliveryChannel, string AccountsQty, string ImpressionQty,
                           string PagesQty, string CycleTerm, string MailingDate,
                           string JoiningFiles, string TotalRecord, string InputFileName, string OutputFileName, string Sorting,
                           string SortingMode, string Other, string DataPrintingRemark,
                           string ArtworkStatus, string PaperStock, string TypeCode, string Paper, string PaperSize,
                           string Grammage, string MaterialColour, string EnvelopeStock, string EnvelopeType, string EnvelopeSize,
                           string EnvelopeGrammage, string EnvelopeColour, string EnvelopeWindow, string EnvWindowOpaque,
                           string LabelStock, string LabelCutsheet, string OthersStock, string BalancedMaterial,
                           string PlasticStock, string PlasticType, string PlasticSize, string PlasticThickness,
                           string PrintingType, string PrintingOrientation, string GpoList, string RegisterMail,
                           string OtherList, string BaseStockType, string FinishingSize, string AdditionalPrintingMark,
                           string SortingCriteria, string PrintingInstr, string SortingInstr, string JobInstruction,
                           string Picture_FileId, string Picture_Extension, string Letter, string Brochures_Leaflets,
                           string ReplyEnvelope, string ImgOnStatement, string Booklet,
                           string NumberOfInsert, string Magezine1, string Brochure1, string CarrierSheet1, string Newsletter1,
                           string Statement1, string Booklet1, string CommentManualType, string FinishingFormat,
                           string FoldingType, string Sealing1, string Tearing1, string BarcodeLabel1, string Cutting1,
                           string StickingOf1, string AddLabel1, string Sticker1, string Chesire1, string Tuck_In1,
                           string Bursting1, string Sealed1, string Folding1, string Unsealed1, string Letter1, string FinishingInst,
                           string IT_SysNotes, string Produc_PlanningNotes, string PurchasingNotes, string EngineeringNotes,
                           string ArtworkNotes, string Acc_BillingNotes, string DCPNotes, string PostingInfo, JobInstruction get)
    {
        var IdentityName = @Session["Fullname"];
        ViewBag.IdentityName = @Session["Fullname"];
        ViewBag.IsDepart = @Session["Department"];
        var IsDepart = @Session["Department"];
        var Role = @Session["Role"];
        var Username = @Session["Username"];
        ViewBag.Username = @Session["Username"];
        Session["Id"] = Id;


        //display data from table 
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT Id,Customer_Name,ProductName,ServiceLevel,IsSlaCreaditCard,JobClass,IsSetPaper,JobRequest,ExpectedDateCompletionToGpo,QuotationRef,ContractName,Contact_Person,JobType,DeliveryChannel,AccountsQty,ImpressionQty,PagesQty,CycleTerm,MailingDate,
                                    JoiningFiles,TotalRecord,InputFileName,OutputFileName,Sorting,SortingMode,Other,DataPrintingRemark,
                                    ArtworkStatus,PaperStock,TypeCode,Paper,PaperSize,Grammage,MaterialColour,EnvelopeStock,EnvelopeType,EnvelopeSize,EnvelopeGrammage,EnvelopeColour,EnvelopeWindow,EnvWindowOpaque,LabelStock,LabelCutsheet,OthersStock,BalancedMaterial,PlasticStock,PlasticType,PlasticSize,PlasticThickness,
                                    PrintingType,PrintingOrientation,GpoList,RegisterMail,OtherList,BaseStockType,FinishingSize,AdditionalPrintingMark,SortingCriteria,PrintingInstr,SortingInstr,Letter,Brochures_Leaflets,ReplyEnvelope,ImgOnStatement,Booklet,
                                    NumberOfInsert,Magezine1,Brochure1,CarrierSheet1,Newsletter1,Statement1,Booklet1,CommentManualType,FinishingFormat,FoldingType,Sealing1,Tearing1,BarcodeLabel1,Cutting1,StickingOf1,AddLabel1,Sticker1,Chesire1,Tuck_In1,Bursting1,Sealed1,Folding1,Unsealed1,Letter1,FinishingInst,
                                    IT_SysNotes,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes,ArtworkNotes,Acc_BillingNotes,DCPNotes,PostingInfo
                                    FROM [IflowSeed].[dbo].[JobInstruction]
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
                    ViewBag.ServiceLevel = reader.GetString(3);
                }
                if (reader.IsDBNull(4) == false)
                {
                    bool getIsSlaCreaditCard = reader.GetBoolean(4);
                    if (getIsSlaCreaditCard == false)
                    {
                        ViewBag.IsSlaCreaditCard = "";
                    }
                    else
                    {
                        ViewBag.IsSlaCreaditCard = "checked";
                    }
                }
                if (reader.IsDBNull(5) == false)
                {
                    ViewBag.JobClass = reader.GetString(5);
                }
                if (reader.IsDBNull(6) == false)
                {
                    bool getIsSetPaper = reader.GetBoolean(6);
                    if (getIsSetPaper == false)
                    {
                        ViewBag.IsSetPaper = "";
                    }
                    else
                    {
                        ViewBag.IsSetPaper = "checked";
                    }
                }
                if (reader.IsDBNull(7) == false)
                {
                    ViewBag.JobRequest = String.Format("{0:dd/MM/yyyy}", reader.GetDateTime(7));
                }
                if (reader.IsDBNull(8) == false)
                {
                    ViewBag.ExpectedDateCompletionToGpo = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(8));
                }
                if (reader.IsDBNull(9) == false)
                {
                    ViewBag.QuotationRef = reader.GetString(9);
                }
                if (reader.IsDBNull(10) == false)
                {
                    ViewBag.ContractName = reader.GetString(10);
                }
                if (reader.IsDBNull(11) == false)
                {
                    ViewBag.Contact_Person = reader.GetString(11);
                }
                if (reader.IsDBNull(12) == false)
                {
                    ViewBag.JobType = reader.GetString(12);
                }
                if (reader.IsDBNull(13) == false)
                {
                    ViewBag.DeliveryChannel = reader.GetString(13);
                }
                if (reader.IsDBNull(14) == false)
                {
                    ViewBag.AccountsQty = reader.GetString(14);
                }
                if (reader.IsDBNull(15) == false)
                {
                    ViewBag.ImpressionQty = reader.GetString(15);
                }
                if (reader.IsDBNull(16) == false)
                {
                    ViewBag.PagesQty = reader.GetString(16);
                }
                if (reader.IsDBNull(17) == false)
                {
                    ViewBag.CycleTerm = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(17));
                }
                if (reader.IsDBNull(18) == false)
                {
                    ViewBag.MailingDate = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(18));
                }

                if (reader.IsDBNull(19) == false)
                {
                    ViewBag.JoiningFiles = reader.GetString(19);
                }
                if (reader.IsDBNull(20) == false)
                {
                    ViewBag.TotalRecord = reader.GetString(20);
                }
                if (reader.IsDBNull(21) == false)
                {
                    ViewBag.InputFileName = reader.GetString(21);
                }
                if (reader.IsDBNull(22) == false)
                {
                    ViewBag.OutputFileName = reader.GetString(22);
                }
                if (reader.IsDBNull(23) == false)
                {
                    ViewBag.Sorting = reader.GetString(23);
                }
                if (reader.IsDBNull(24) == false)
                {
                    ViewBag.SortingMode = reader.GetString(24);
                }
                if (reader.IsDBNull(25) == false)
                {
                    ViewBag.Other = reader.GetString(25);
                }
                if (reader.IsDBNull(26) == false)
                {
                    ViewBag.DataPrintingRemark = reader.GetString(26);
                }
                if (reader.IsDBNull(27) == false)
                {
                    ViewBag.ArtworkStatus = reader.GetString(27);
                }
                if (reader.IsDBNull(28) == false)
                {
                    ViewBag.PaperStock = reader.GetString(28);
                }
                if (reader.IsDBNull(29) == false)
                {
                    ViewBag.TypeCode = reader.GetString(29);
                }
                if (reader.IsDBNull(30) == false)
                {
                    ViewBag.Paper = reader.GetString(30);
                }
                if (reader.IsDBNull(31) == false)
                {
                    ViewBag.PaperSize = reader.GetString(31);
                }
                if (reader.IsDBNull(32) == false)
                {
                    ViewBag.Grammage = reader.GetString(32);
                }
                if (reader.IsDBNull(33) == false)
                {
                    ViewBag.MaterialColour = reader.GetString(33);
                }
                if (reader.IsDBNull(34) == false)
                {
                    ViewBag.EnvelopeStock = reader.GetString(34);
                }
                if (reader.IsDBNull(35) == false)
                {
                    ViewBag.EnvelopeType = reader.GetString(35);
                }
                if (reader.IsDBNull(36) == false)
                {
                    ViewBag.EnvelopeSize = reader.GetString(36);
                }
                if (reader.IsDBNull(37) == false)
                {
                    ViewBag.EnvelopeGrammage = reader.GetString(37);
                }
                if (reader.IsDBNull(38) == false)
                {
                    ViewBag.EnvelopeColour = reader.GetString(38);
                }
                if (reader.IsDBNull(39) == false)
                {
                    ViewBag.EnvelopeWindow = reader.GetString(39);
                }
                if (reader.IsDBNull(40) == false)
                {
                    ViewBag.EnvWindowOpaque = reader.GetString(40);
                }
                if (reader.IsDBNull(41) == false)
                {
                    ViewBag.LabelStock = reader.GetString(41);
                }
                if (reader.IsDBNull(42) == false)
                {
                    ViewBag.LabelCutsheet = reader.GetString(42);
                }
                if (reader.IsDBNull(43) == false)
                {
                    ViewBag.OthersStock = reader.GetString(43);
                }
                if (reader.IsDBNull(44) == false)
                {
                    ViewBag.BalancedMaterial = reader.GetString(44);
                }
                if (reader.IsDBNull(45) == false)
                {
                    ViewBag.PlasticStock = reader.GetString(45);
                }
                if (reader.IsDBNull(46) == false)
                {
                    ViewBag.PlasticType = reader.GetString(46);
                }
                if (reader.IsDBNull(47) == false)
                {
                    ViewBag.PlasticSize = reader.GetString(47);
                }
                if (reader.IsDBNull(48) == false)
                {
                    ViewBag.PlasticThickness = reader.GetString(48);
                }
                if (reader.IsDBNull(49) == false)
                {
                    ViewBag.PrintingType = reader.GetString(49);
                }
                if (reader.IsDBNull(50) == false)
                {
                    ViewBag.PrintingOrientation = reader.GetString(50);
                }
                if (reader.IsDBNull(51) == false)
                {
                    bool getGpoList = reader.GetBoolean(51);
                    if (getGpoList == false)
                    {
                        ViewBag.GpoList = "";
                    }
                    else
                    {
                        ViewBag.GpoList = "checked";
                    }
                }
                if (reader.IsDBNull(52) == false)
                {
                    bool getRegisterMail = reader.GetBoolean(52);
                    if (getRegisterMail == false)
                    {
                        ViewBag.RegisterMail = "";
                    }
                    else
                    {
                        ViewBag.RegisterMail = "checked";
                    }
                }
                if (reader.IsDBNull(53) == false)
                {
                    ViewBag.OtherList = reader.GetString(53);
                }
                if (reader.IsDBNull(54) == false)
                {
                    ViewBag.BaseStockType = reader.GetString(54);
                }
                if (reader.IsDBNull(55) == false)
                {
                    ViewBag.FinishingSize = reader.GetString(55);
                }
                if (reader.IsDBNull(56) == false)
                {
                    ViewBag.AdditionalPrintingMark = reader.GetString(56);
                }
                if (reader.IsDBNull(57) == false)
                {
                    ViewBag.SortingCriteria = reader.GetString(57);
                }
                if (reader.IsDBNull(58) == false)
                {
                    ViewBag.PrintingInstr = reader.GetString(58);
                }
                if (reader.IsDBNull(59) == false)
                {
                    ViewBag.SortingInstr = reader.GetString(59);
                }
                if (reader.IsDBNull(60) == false)
                {
                    bool getLetter = reader.GetBoolean(60);
                    if (getLetter == false)
                    {
                        ViewBag.Letter = "";
                    }
                    else
                    {
                        ViewBag.Letter = "checked";
                    }
                }
                if (reader.IsDBNull(61) == false)
                {
                    bool getBrochures_Leaflets = reader.GetBoolean(61);
                    if (getBrochures_Leaflets == false)
                    {
                        ViewBag.Brochures_Leaflets = "";
                    }
                    else
                    {
                        ViewBag.Brochures_Leaflets = "checked";
                    }
                }
                if (reader.IsDBNull(62) == false)
                {
                    bool getReplyEnvelope = reader.GetBoolean(62);
                    if (getReplyEnvelope == false)
                    {
                        ViewBag.ReplyEnvelope = "";
                    }
                    else
                    {
                        ViewBag.ReplyEnvelope = "checked";
                    }
                }
                if (reader.IsDBNull(63) == false)
                {
                    bool getImgOnStatement = reader.GetBoolean(63);
                    if (getImgOnStatement == false)
                    {
                        ViewBag.ImgOnStatement = "";
                    }
                    else
                    {
                        ViewBag.ImgOnStatement = "checked";
                    }
                }
                if (reader.IsDBNull(64) == false)
                {
                    bool getBooklet = reader.GetBoolean(64);
                    if (getBooklet == false)
                    {
                        ViewBag.Booklet = "";
                    }
                    else
                    {
                        ViewBag.Booklet = "checked";
                    }
                }
                if (reader.IsDBNull(65) == false)
                {
                    ViewBag.NumberOfInsert = reader.GetString(65);
                }
                if (reader.IsDBNull(66) == false)
                {
                    bool getMagezine1 = reader.GetBoolean(66);
                    if (getMagezine1 == false)
                    {
                        ViewBag.Magezine1 = "";
                    }
                    else
                    {
                        ViewBag.Magezine1 = "checked";
                    }
                }
                if (reader.IsDBNull(67) == false)
                {
                    bool getBrochure1 = reader.GetBoolean(67);
                    if (getBrochure1 == false)
                    {
                        ViewBag.Brochure1 = "";
                    }
                    else
                    {
                        ViewBag.Brochure1 = "checked";
                    }
                }
                if (reader.IsDBNull(68) == false)
                {
                    bool getCarrierSheet1 = reader.GetBoolean(68);
                    if (getCarrierSheet1 == false)
                    {
                        ViewBag.CarrierSheet1 = "";
                    }
                    else
                    {
                        ViewBag.CarrierSheet1 = "checked";
                    }
                }
                if (reader.IsDBNull(69) == false)
                {
                    bool getNewsletter1 = reader.GetBoolean(69);
                    if (getNewsletter1 == false)
                    {
                        ViewBag.Newsletter1 = "";
                    }
                    else
                    {
                        ViewBag.Newsletter1 = "checked";
                    }
                }
                if (reader.IsDBNull(70) == false)
                {
                    bool getStatement1 = reader.GetBoolean(70);
                    if (getStatement1 == false)
                    {
                        ViewBag.Statement1 = "";
                    }
                    else
                    {
                        ViewBag.Statement1 = "checked";
                    }
                }
                if (reader.IsDBNull(71) == false)
                {
                    bool getBooklet1 = reader.GetBoolean(71);
                    if (getBooklet1 == false)
                    {
                        ViewBag.Booklet1 = "";
                    }
                    else
                    {
                        ViewBag.Booklet1 = "checked";
                    }
                }
                if (reader.IsDBNull(72) == false)
                {
                    ViewBag.CommentManualType = reader.GetString(72);
                }
                if (reader.IsDBNull(73) == false)
                {
                    ViewBag.FinishingFormat = reader.GetString(73);
                }
                if (reader.IsDBNull(74) == false)
                {
                    ViewBag.FoldingType = reader.GetString(74);
                }
                if (reader.IsDBNull(75) == false)
                {
                    bool getSealing1 = reader.GetBoolean(75);
                    if (getSealing1 == false)
                    {
                        ViewBag.Sealing1 = "";
                    }
                    else
                    {
                        ViewBag.Sealing1 = "checked";
                    }
                }
                if (reader.IsDBNull(76) == false)
                {
                    bool getTearing1 = reader.GetBoolean(76);
                    if (getTearing1 == false)
                    {
                        ViewBag.Tearing1 = "";
                    }
                    else
                    {
                        ViewBag.Tearing1 = "checked";
                    }
                }
                if (reader.IsDBNull(77) == false)
                {
                    bool getBarcodeLabel1 = reader.GetBoolean(77);
                    if (getBarcodeLabel1 == false)
                    {
                        ViewBag.BarcodeLabel1 = "";
                    }
                    else
                    {
                        ViewBag.BarcodeLabel1 = "checked";
                    }
                }
                if (reader.IsDBNull(78) == false)
                {
                    bool getCutting1 = reader.GetBoolean(78);
                    if (getCutting1 == false)
                    {
                        ViewBag.Cutting1 = "";
                    }
                    else
                    {
                        ViewBag.Cutting1 = "checked";
                    }
                }
                if (reader.IsDBNull(79) == false)
                {
                    ViewBag.StickingOf1 = reader.GetString(79);
                }
                if (reader.IsDBNull(80) == false)
                {
                    bool getAddLabel1 = reader.GetBoolean(80);
                    if (getAddLabel1 == false)
                    {
                        ViewBag.AddLabel1 = "";
                    }
                    else
                    {
                        ViewBag.AddLabel1 = "checked";
                    }
                }
                if (reader.IsDBNull(81) == false)
                {
                    bool getSticker1 = reader.GetBoolean(81);
                    if (getSticker1 == false)
                    {
                        ViewBag.Sticker1 = "";
                    }
                    else
                    {
                        ViewBag.Sticker1 = "checked";
                    }
                }
                if (reader.IsDBNull(82) == false)
                {
                    bool getChesire1 = reader.GetBoolean(82);
                    if (getChesire1 == false)
                    {
                        ViewBag.Chesire1 = "";
                    }
                    else
                    {
                        ViewBag.Chesire1 = "checked";
                    }
                }
                if (reader.IsDBNull(83) == false)
                {
                    bool getTuck_In1 = reader.GetBoolean(83);
                    if (getTuck_In1 == false)
                    {
                        ViewBag.Tuck_In1 = "";
                    }
                    else
                    {
                        ViewBag.Tuck_In1 = "checked";
                    }
                }
                if (reader.IsDBNull(84) == false)
                {
                    bool getBursting1 = reader.GetBoolean(84);
                    if (getBursting1 == false)
                    {
                        ViewBag.Bursting1 = "";
                    }
                    else
                    {
                        ViewBag.Bursting1 = "checked";
                    }
                }
                if (reader.IsDBNull(85) == false)
                {
                    bool getSealed1 = reader.GetBoolean(85);
                    if (getSealed1 == false)
                    {
                        ViewBag.Sealed1 = "";
                    }
                    else
                    {
                        ViewBag.Sealed1 = "checked";
                    }
                }
                if (reader.IsDBNull(86) == false)
                {
                    bool getFolding1 = reader.GetBoolean(86);
                    if (getFolding1 == false)
                    {
                        ViewBag.Folding1 = "";
                    }
                    else
                    {
                        ViewBag.Folding1 = "checked";
                    }
                }
                if (reader.IsDBNull(87) == false)
                {
                    bool getUnsealed1 = reader.GetBoolean(87);
                    if (getUnsealed1 == false)
                    {
                        ViewBag.Unsealed1 = "";
                    }
                    else
                    {
                        ViewBag.Unsealed1 = "checked";
                    }
                }
                if (reader.IsDBNull(88) == false)
                {
                    bool getLetter1 = reader.GetBoolean(88);
                    if (getLetter1 == false)
                    {
                        ViewBag.Letter1 = "";
                    }
                    else
                    {
                        ViewBag.Letter1 = "checked";
                    }
                }
                if (reader.IsDBNull(89) == false)
                {
                    ViewBag.FinishingInst = reader.GetString(89);
                }
                if (reader.IsDBNull(90) == false)
                {
                    ViewBag.IT_SysNotes = reader.GetString(90);
                }
                if (reader.IsDBNull(91) == false)
                {
                    ViewBag.Produc_PlanningNotes = reader.GetString(91);
                }
                if (reader.IsDBNull(92) == false)
                {
                    ViewBag.PurchasingNotes = reader.GetString(92);
                }
                if (reader.IsDBNull(93) == false)
                {
                    ViewBag.EngineeringNotes = reader.GetString(93);
                }
                if (reader.IsDBNull(94) == false)
                {
                    ViewBag.ArtworkNotes = reader.GetString(94);
                }
                if (reader.IsDBNull(95) == false)
                {
                    ViewBag.Acc_BillingNotes = reader.GetString(95);
                }
                if (reader.IsDBNull(96) == false)
                {
                    ViewBag.DCPNotes = reader.GetString(96);
                }
                if (reader.IsDBNull(97) == false)
                {
                    ViewBag.PostingInfo = reader.GetString(97);
                }

            }
            cn.Close();
        }

        //call table

        List<JobInstruction> viewJIList = new List<JobInstruction>();
        using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn2))
        {
            int _bil = 1;
            cn2.Open();
            command.CommandText = @"SELECT Id,Customer_Name,ProductName,JobSheetNo,SalesExecutiveBy,Status,
                                           ServiceLevel,IsSlaCreaditCard,JobClass,IsSetPaper,JobRequest,
                                           ExpectedDateCompletionToGpo,QuotationRef,ContractName,
                                           Contact_Person,JobType,DeliveryChannel,AccountsQty,ImpressionQty,
                                           PagesQty,CycleTerm,MailingDate,
                                           JoiningFiles,TotalRecord,InputFileName,OutputFileName,Sorting,
                                           SortingMode,Other,DataPrintingRemark,
                                           ArtworkStatus,PaperStock,TypeCode,Paper,PaperSize,
                                           Grammage,MaterialColour,EnvelopeStock,EnvelopeType,EnvelopeSize,
                                           EnvelopeGrammage,EnvelopeColour,EnvelopeWindow,EnvWindowOpaque,
                                           LabelStock,LabelCutsheet,OthersStock,BalancedMaterial,
                                           PlasticStock,PlasticType,PlasticSize,PlasticThickness,
                                           PrintingType,PrintingOrientation,GpoList,RegisterMail,
                                           OtherList,BaseStockType,FinishingSize,AdditionalPrintingMark,
                                           SortingCriteria,PrintingInstr,SortingInstr,Letter,Brochures_Leaflets,
                                           ReplyEnvelope,ImgOnStatement,Booklet,
                                           NumberOfInsert,Magezine1,Brochure1,CarrierSheet1,Newsletter1,
                                           Statement1,Booklet1,CommentManualType,FinishingFormat,
                                           FoldingType,Sealing1,Tearing1,BarcodeLabel1,Cutting1,
                                           StickingOf1,AddLabel1,Sticker1,Chesire1,Tuck_In1,
                                           Bursting1,Sealed1,Folding1,Unsealed1,Letter1,FinishingInst,
                                           IT_SysNotes,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes,
                                           ArtworkNotes, Acc_BillingNotes, DCPNotes, PostingInfo
                                           FROM [IflowSeed].[dbo].[JobInstruction]
                                           WHERE Id=@Id";
            command.Parameters.AddWithValue("@Id", Session["Id"].ToString());
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
                        model.JobSheetNo = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        model.SalesExecutiveBy = reader.GetString(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        model.Status = reader.GetString(5);
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        model.ServiceLevel = reader.GetString(6);
                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        model.SlaCreaditCard = reader.GetBoolean(7);

                    }
                    if (reader.IsDBNull(8) == false)
                    {
                        model.JobClass = reader.GetString(8);
                    }
                    if (reader.IsDBNull(9) == false)
                    {
                        model.IsSetPaper = reader.GetBoolean(9);

                    }
                    if (reader.IsDBNull(10) == false)
                    {
                        model.JobRequest = reader.GetDateTime(10);
                    }
                    if (reader.IsDBNull(11) == false)
                    {
                        model.ExpectedDateCompletionToGpo = reader.GetDateTime(11);
                    }
                    if (reader.IsDBNull(12) == false)
                    {
                        model.QuotationRef = reader.GetString(12);
                    }
                    if (reader.IsDBNull(13) == false)
                    {
                        model.ContractName = reader.GetString(13);
                    }
                    if (reader.IsDBNull(14) == false)
                    {
                        model.Contact_Person = reader.GetString(14);
                    }
                    if (reader.IsDBNull(15) == false)
                    {
                        model.JobType = reader.GetString(15);
                    }
                    if (reader.IsDBNull(16) == false)
                    {
                        model.DeliveryChannel = reader.GetString(16);
                    }
                    if (reader.IsDBNull(17) == false)
                    {
                        model.AccountsQty = reader.GetString(17);
                    }
                    if (reader.IsDBNull(18) == false)
                    {
                        model.ImpressionQty = reader.GetString(18);
                    }
                    if (reader.IsDBNull(19) == false)
                    {
                        model.PagesQty = reader.GetString(19);
                    }
                    if (reader.IsDBNull(20) == false)
                    {
                        model.CycleTerm = reader.GetDateTime(20);
                    }
                    if (reader.IsDBNull(21) == false)
                    {
                        model.MailingDate = (DateTime)reader.GetDateTime(21);
                    }

                    if (reader.IsDBNull(22) == false)
                    {
                        model.JoiningFiles = reader.GetString(22);
                    }
                    if (reader.IsDBNull(23) == false)
                    {
                        model.TotalRecord = reader.GetString(23);
                    }
                    if (reader.IsDBNull(24) == false)
                    {
                        model.InputFileName = reader.GetString(24);
                    }
                    if (reader.IsDBNull(25) == false)
                    {
                        model.OutputFileName = reader.GetString(25);
                    }
                    if (reader.IsDBNull(26) == false)
                    {
                        model.Sorting = reader.GetString(26);
                    }
                    if (reader.IsDBNull(27) == false)
                    {
                        model.SortingMode = reader.GetString(27);
                    }
                    if (reader.IsDBNull(28) == false)
                    {
                        model.Other = reader.GetString(28);
                    }
                    if (reader.IsDBNull(29) == false)
                    {
                        model.DataPrintingRemark = reader.GetString(29);
                    }
                    if (reader.IsDBNull(30) == false)
                    {
                        model.ArtworkStatus = reader.GetString(30);
                    }
                    if (reader.IsDBNull(31) == false)
                    {
                        model.PaperStock = reader.GetString(31);
                    }
                    if (reader.IsDBNull(32) == false)
                    {
                        model.TypeCode = reader.GetString(32);
                    }
                    if (reader.IsDBNull(33) == false)
                    {
                        model.Paper = reader.GetString(33);
                    }
                    if (reader.IsDBNull(34) == false)
                    {
                        model.PaperSize = reader.GetString(34);
                    }
                    if (reader.IsDBNull(35) == false)
                    {
                        model.Grammage = reader.GetString(35);
                    }
                    if (reader.IsDBNull(36) == false)
                    {
                        model.MaterialColour = reader.GetString(36);
                    }
                    if (reader.IsDBNull(37) == false)
                    {
                        model.EnvelopeStock = reader.GetString(37);
                    }
                    if (reader.IsDBNull(38) == false)
                    {
                        model.EnvelopeType = reader.GetString(38);
                    }
                    if (reader.IsDBNull(39) == false)
                    {
                        model.EnvelopeSize = reader.GetString(39);
                    }
                    if (reader.IsDBNull(40) == false)
                    {
                        model.EnvelopeGrammage = reader.GetString(40);
                    }
                    if (reader.IsDBNull(41) == false)
                    {
                        model.EnvelopeColour = reader.GetString(41);
                    }
                    if (reader.IsDBNull(42) == false)
                    {
                        model.EnvelopeWindow = reader.GetString(42);
                    }
                    if (reader.IsDBNull(43) == false)
                    {
                        model.EnvWindowOpaque = reader.GetString(43);
                    }
                    if (reader.IsDBNull(44) == false)
                    {
                        model.LabelStock = reader.GetString(44);
                    }
                    if (reader.IsDBNull(45) == false)
                    {
                        model.LabelCutsheet = reader.GetString(45);
                    }
                    if (reader.IsDBNull(46) == false)
                    {
                        model.OthersStock = reader.GetString(46);
                    }
                    if (reader.IsDBNull(47) == false)
                    {
                        model.BalancedMaterial = reader.GetString(47);
                    }
                    if (reader.IsDBNull(48) == false)
                    {
                        model.PlasticStock = reader.GetString(48);
                    }
                    if (reader.IsDBNull(49) == false)
                    {
                        model.PlasticType = reader.GetString(49);
                    }
                    if (reader.IsDBNull(50) == false)
                    {
                        model.PlasticSize = reader.GetString(50);
                    }
                    if (reader.IsDBNull(51) == false)
                    {
                        model.PlasticThickness = reader.GetString(51);
                    }
                    if (reader.IsDBNull(52) == false)
                    {
                        model.PrintingType = reader.GetString(52);
                    }
                    if (reader.IsDBNull(53) == false)
                    {
                        model.PrintingOrientation = reader.GetString(53);
                    }
                    if (reader.IsDBNull(54) == false)
                    {
                        model.GpoList = reader.GetBoolean(54);
                    }
                    if (reader.IsDBNull(55) == false)
                    {
                        model.RegisterMail = reader.GetBoolean(55);
                    }
                    if (reader.IsDBNull(56) == false)
                    {
                        model.OtherList = reader.GetString(56);
                    }
                    if (reader.IsDBNull(57) == false)
                    {
                        model.BaseStockType = reader.GetString(57);
                    }
                    if (reader.IsDBNull(58) == false)
                    {
                        model.FinishingSize = reader.GetString(58);
                    }
                    if (reader.IsDBNull(59) == false)
                    {
                        model.AdditionalPrintingMark = reader.GetString(59);
                    }
                    if (reader.IsDBNull(60) == false)
                    {
                        model.SortingCriteria = reader.GetString(60);
                    }
                    if (reader.IsDBNull(61) == false)
                    {
                        model.PrintingInstr = reader.GetString(61);
                    }
                    if (reader.IsDBNull(62) == false)
                    {
                        model.SortingInstr = reader.GetString(62);
                    }
                    if (reader.IsDBNull(63) == false)
                    {
                        model.Letter = reader.GetBoolean(63);
                    }
                    if (reader.IsDBNull(64) == false)
                    {
                        model.Brochures_Leaflets = reader.GetBoolean(64);
                    }
                    if (reader.IsDBNull(65) == false)
                    {
                        model.ReplyEnvelope = reader.GetBoolean(65);
                    }
                    if (reader.IsDBNull(66) == false)
                    {
                        model.ImgOnStatement = reader.GetBoolean(66);
                    }
                    if (reader.IsDBNull(67) == false)
                    {
                        model.Booklet = reader.GetBoolean(67);
                    }
                    if (reader.IsDBNull(68) == false)
                    {
                        model.NumberOfInsert = reader.GetString(68);
                    }
                    if (reader.IsDBNull(69) == false)
                    {
                        model.Magezine1 = reader.GetBoolean(69);
                    }
                    if (reader.IsDBNull(70) == false)
                    {
                        model.Brochure1 = reader.GetBoolean(70);
                    }
                    if (reader.IsDBNull(71) == false)
                    {
                        model.CarrierSheet1 = reader.GetBoolean(71);
                    }
                    if (reader.IsDBNull(72) == false)
                    {
                        model.Newsletter1 = reader.GetBoolean(72);
                    }
                    if (reader.IsDBNull(73) == false)
                    {
                        model.Statement1 = reader.GetBoolean(73);
                    }
                    if (reader.IsDBNull(74) == false)
                    {
                        model.Booklet1 = reader.GetBoolean(74);
                    }
                    if (reader.IsDBNull(75) == false)
                    {
                        model.CommentManualType = reader.GetString(75);
                    }
                    if (reader.IsDBNull(76) == false)
                    {
                        model.FinishingFormat = reader.GetString(76);
                    }
                    if (reader.IsDBNull(77) == false)
                    {
                        model.FoldingType = reader.GetString(77);
                    }
                    if (reader.IsDBNull(78) == false)
                    {
                        model.Sealing1 = reader.GetBoolean(78);
                    }
                    if (reader.IsDBNull(79) == false)
                    {
                        model.Tearing1 = reader.GetBoolean(79);
                    }
                    if (reader.IsDBNull(80) == false)
                    {
                        model.BarcodeLabel1 = reader.GetBoolean(80);
                    }
                    if (reader.IsDBNull(81) == false)
                    {
                        model.Cutting1 = reader.GetBoolean(81);
                    }
                    if (reader.IsDBNull(82) == false)
                    {
                        model.StickingOf1 = reader.GetString(82);
                    }
                    if (reader.IsDBNull(83) == false)
                    {
                        model.AddLabel1 = reader.GetBoolean(83);
                    }
                    if (reader.IsDBNull(84) == false)
                    {
                        model.Sticker1 = reader.GetBoolean(84);
                    }
                    if (reader.IsDBNull(85) == false)
                    {
                        model.Chesire1 = reader.GetBoolean(85);
                    }
                    if (reader.IsDBNull(86) == false)
                    {
                        model.Tuck_In1 = reader.GetBoolean(86);
                    }
                    if (reader.IsDBNull(87) == false)
                    {
                        model.Bursting1 = reader.GetBoolean(87);
                    }
                    if (reader.IsDBNull(88) == false)
                    {
                        model.Sealed1 = reader.GetBoolean(88);
                    }
                    if (reader.IsDBNull(89) == false)
                    {
                        model.Folding1 = reader.GetBoolean(89);
                    }
                    if (reader.IsDBNull(90) == false)
                    {
                        model.Unsealed1 = reader.GetBoolean(90);
                    }
                    if (reader.IsDBNull(91) == false)
                    {
                        model.Letter1 = reader.GetBoolean(91);
                    }
                    if (reader.IsDBNull(92) == false)
                    {
                        model.FinishingInst = reader.GetString(92);
                    }
                    if (reader.IsDBNull(93) == false)
                    {
                        model.IT_SysNotes = reader.GetString(93);
                    }
                    if (reader.IsDBNull(94) == false)
                    {
                        model.Produc_PlanningNotes = reader.GetString(94);
                    }
                    if (reader.IsDBNull(95) == false)
                    {
                        model.PurchasingNotes = reader.GetString(95);
                    }
                    if (reader.IsDBNull(96) == false)
                    {
                        model.EngineeringNotes = reader.GetString(96);
                    }
                    if (reader.IsDBNull(97) == false)
                    {
                        model.ArtworkNotes = reader.GetString(97);
                    }
                    if (reader.IsDBNull(98) == false)
                    {
                        model.Acc_BillingNotes = reader.GetString(98);
                    }
                    if (reader.IsDBNull(99) == false)
                    {
                        model.DCPNotes = reader.GetString(99);
                    }
                    if (reader.IsDBNull(100) == false)
                    {
                        model.PostingInfo = reader.GetString(100);
                    }
                }
                viewJIList.Add(model);
            }
            cn2.Close();

        }







        //-----------------------------------------

        ReloadJIList(Id);

        return new Rotativa.ViewAsPdf("ViewJI", viewJIList)
        {
            // FileName = flightPlan.ListingItemDetailsModel.FlightDetails + ".pdf",
            PageMargins = new Rotativa.Options.Margins(5, 5, 5, 5),
            PageOrientation = Rotativa.Options.Orientation.Portrait,
            //PageWidth = 210,
            //PageHeight = 297
        };
    }

    List<JobInstruction> viewJobInstructionList = new List<JobInstruction>();
    private void ReloadJIList(string Id)
    {
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {

            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT Id,Customer_Name,ProductName,JobSheetNo,SalesExecutiveBy,Status,
                                           ServiceLevel,IsSlaCreaditCard,JobClass,IsSetPaper,JobRequest,
                                           ExpectedDateCompletionToGpo,QuotationRef,ContractName,
                                           Contact_Person,JobType,DeliveryChannel,AccountsQty,ImpressionQty,
                                           PagesQty,CycleTerm,MailingDate,
                                           JoiningFiles,TotalRecord,InputFileName,OutputFileName,Sorting,
                                           SortingMode,Other,DataPrintingRemark,
                                           ArtworkStatus,PaperStock,TypeCode,Paper,PaperSize,
                                           Grammage,MaterialColour,EnvelopeStock,EnvelopeType,EnvelopeSize,
                                           EnvelopeGrammage,EnvelopeColour,EnvelopeWindow,EnvWindowOpaque,
                                           LabelStock,LabelCutsheet,OthersStock,BalancedMaterial,
                                           PlasticStock,PlasticType,PlasticSize,PlasticThickness,
                                           PrintingType,PrintingOrientation,GpoList,RegisterMail,
                                           OtherList,BaseStockType,FinishingSize,AdditionalPrintingMark,
                                           SortingCriteria,PrintingInstr,SortingInstr,Letter,Brochures_Leaflets,
                                           ReplyEnvelope,ImgOnStatement,Booklet,
                                           NumberOfInsert,Magezine1,Brochure1,CarrierSheet1,Newsletter1,
                                           Statement1,Booklet1,CommentManualType,FinishingFormat,
                                           FoldingType,Sealing1,Tearing1,BarcodeLabel1,Cutting1,
                                           StickingOf1,AddLabel1,Sticker1,Chesire1,Tuck_In1,
                                           Bursting1,Sealed1,Folding1,Unsealed1,Letter1,FinishingInst,
                                           IT_SysNotes,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes,
                                           ArtworkNotes, Acc_BillingNotes, DCPNotes, PostingInfo
                                           FROM [IflowSeed].[dbo].[JobInstruction]
                                           WHERE Id=@Id ";
            command.Parameters.AddWithValue("@Id", Id);
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
                        model.JobSheetNo = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        model.SalesExecutiveBy = reader.GetString(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        model.Status = reader.GetString(5);
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        model.ServiceLevel = reader.GetString(6);
                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        model.SlaCreaditCard = reader.GetBoolean(7);

                    }
                    if (reader.IsDBNull(8) == false)
                    {
                        model.JobClass = reader.GetString(8);
                    }
                    if (reader.IsDBNull(9) == false)
                    {
                        model.IsSetPaper = reader.GetBoolean(9);

                    }
                    if (reader.IsDBNull(10) == false)
                    {
                        model.JobRequest = reader.GetDateTime(10);
                    }
                    if (reader.IsDBNull(11) == false)
                    {
                        model.ExpectedDateCompletionToGpo = reader.GetDateTime(11);
                    }
                    if (reader.IsDBNull(12) == false)
                    {
                        model.QuotationRef = reader.GetString(12);
                    }
                    if (reader.IsDBNull(13) == false)
                    {
                        model.ContractName = reader.GetString(13);
                    }
                    if (reader.IsDBNull(14) == false)
                    {
                        model.Contact_Person = reader.GetString(14);
                    }
                    if (reader.IsDBNull(15) == false)
                    {
                        model.JobType = reader.GetString(15);
                    }
                    if (reader.IsDBNull(16) == false)
                    {
                        model.DeliveryChannel = reader.GetString(16);
                    }
                    if (reader.IsDBNull(17) == false)
                    {
                        model.AccountsQty = reader.GetString(17);
                    }
                    if (reader.IsDBNull(18) == false)
                    {
                        model.ImpressionQty = reader.GetString(18);
                    }
                    if (reader.IsDBNull(19) == false)
                    {
                        model.PagesQty = reader.GetString(19);
                    }
                    if (reader.IsDBNull(20) == false)
                    {
                        model.CycleTerm = reader.GetDateTime(20);
                    }
                    if (reader.IsDBNull(21) == false)
                    {
                        model.MailingDate = (DateTime)reader.GetDateTime(21);
                    }

                    if (reader.IsDBNull(22) == false)
                    {
                        model.JoiningFiles = reader.GetString(22);
                    }
                    if (reader.IsDBNull(23) == false)
                    {
                        model.TotalRecord = reader.GetString(23);
                    }
                    if (reader.IsDBNull(24) == false)
                    {
                        model.InputFileName = reader.GetString(24);
                    }
                    if (reader.IsDBNull(25) == false)
                    {
                        model.OutputFileName = reader.GetString(25);
                    }
                    if (reader.IsDBNull(26) == false)
                    {
                        model.Sorting = reader.GetString(26);
                    }
                    if (reader.IsDBNull(27) == false)
                    {
                        model.SortingMode = reader.GetString(27);
                    }
                    if (reader.IsDBNull(28) == false)
                    {
                        model.Other = reader.GetString(28);
                    }
                    if (reader.IsDBNull(29) == false)
                    {
                        model.DataPrintingRemark = reader.GetString(29);
                    }
                    if (reader.IsDBNull(30) == false)
                    {
                        model.ArtworkStatus = reader.GetString(30);
                    }
                    if (reader.IsDBNull(31) == false)
                    {
                        model.PaperStock = reader.GetString(31);
                    }
                    if (reader.IsDBNull(32) == false)
                    {
                        model.TypeCode = reader.GetString(32);
                    }
                    if (reader.IsDBNull(33) == false)
                    {
                        model.Paper = reader.GetString(33);
                    }
                    if (reader.IsDBNull(34) == false)
                    {
                        model.PaperSize = reader.GetString(34);
                    }
                    if (reader.IsDBNull(35) == false)
                    {
                        model.Grammage = reader.GetString(35);
                    }
                    if (reader.IsDBNull(36) == false)
                    {
                        model.MaterialColour = reader.GetString(36);
                    }
                    if (reader.IsDBNull(37) == false)
                    {
                        model.EnvelopeStock = reader.GetString(37);
                    }
                    if (reader.IsDBNull(38) == false)
                    {
                        model.EnvelopeType = reader.GetString(38);
                    }
                    if (reader.IsDBNull(39) == false)
                    {
                        model.EnvelopeSize = reader.GetString(39);
                    }
                    if (reader.IsDBNull(40) == false)
                    {
                        model.EnvelopeGrammage = reader.GetString(40);
                    }
                    if (reader.IsDBNull(41) == false)
                    {
                        model.EnvelopeColour = reader.GetString(41);
                    }
                    if (reader.IsDBNull(42) == false)
                    {
                        model.EnvelopeWindow = reader.GetString(42);
                    }
                    if (reader.IsDBNull(43) == false)
                    {
                        model.EnvWindowOpaque = reader.GetString(43);
                    }
                    if (reader.IsDBNull(44) == false)
                    {
                        model.LabelStock = reader.GetString(44);
                    }
                    if (reader.IsDBNull(45) == false)
                    {
                        model.LabelCutsheet = reader.GetString(45);
                    }
                    if (reader.IsDBNull(46) == false)
                    {
                        model.OthersStock = reader.GetString(46);
                    }
                    if (reader.IsDBNull(47) == false)
                    {
                        model.BalancedMaterial = reader.GetString(47);
                    }
                    if (reader.IsDBNull(48) == false)
                    {
                        model.PlasticStock = reader.GetString(48);
                    }
                    if (reader.IsDBNull(49) == false)
                    {
                        model.PlasticType = reader.GetString(49);
                    }
                    if (reader.IsDBNull(50) == false)
                    {
                        model.PlasticSize = reader.GetString(50);
                    }
                    if (reader.IsDBNull(51) == false)
                    {
                        model.PlasticThickness = reader.GetString(51);
                    }
                    if (reader.IsDBNull(52) == false)
                    {
                        model.PrintingType = reader.GetString(52);
                    }
                    if (reader.IsDBNull(53) == false)
                    {
                        model.PrintingOrientation = reader.GetString(53);
                    }
                    if (reader.IsDBNull(54) == false)
                    {
                        model.GpoList = reader.GetBoolean(54);
                    }
                    if (reader.IsDBNull(55) == false)
                    {
                        model.RegisterMail = reader.GetBoolean(55);
                    }
                    if (reader.IsDBNull(56) == false)
                    {
                        model.OtherList = reader.GetString(56);
                    }
                    if (reader.IsDBNull(57) == false)
                    {
                        model.BaseStockType = reader.GetString(57);
                    }
                    if (reader.IsDBNull(58) == false)
                    {
                        model.FinishingSize = reader.GetString(58);
                    }
                    if (reader.IsDBNull(59) == false)
                    {
                        model.AdditionalPrintingMark = reader.GetString(59);
                    }
                    if (reader.IsDBNull(60) == false)
                    {
                        model.SortingCriteria = reader.GetString(60);
                    }
                    if (reader.IsDBNull(61) == false)
                    {
                        model.PrintingInstr = reader.GetString(61);
                    }
                    if (reader.IsDBNull(62) == false)
                    {
                        model.SortingInstr = reader.GetString(62);
                    }
                    if (reader.IsDBNull(63) == false)
                    {
                        model.Letter = reader.GetBoolean(63);
                    }
                    if (reader.IsDBNull(64) == false)
                    {
                        model.Brochures_Leaflets = reader.GetBoolean(64);
                    }
                    if (reader.IsDBNull(65) == false)
                    {
                        model.ReplyEnvelope = reader.GetBoolean(65);
                    }
                    if (reader.IsDBNull(66) == false)
                    {
                        model.ImgOnStatement = reader.GetBoolean(66);
                    }
                    if (reader.IsDBNull(67) == false)
                    {
                        model.Booklet = reader.GetBoolean(67);
                    }
                    if (reader.IsDBNull(68) == false)
                    {
                        model.NumberOfInsert = reader.GetString(68);
                    }
                    if (reader.IsDBNull(69) == false)
                    {
                        model.Magezine1 = reader.GetBoolean(69);
                    }
                    if (reader.IsDBNull(70) == false)
                    {
                        model.Brochure1 = reader.GetBoolean(70);
                    }
                    if (reader.IsDBNull(71) == false)
                    {
                        model.CarrierSheet1 = reader.GetBoolean(71);
                    }
                    if (reader.IsDBNull(72) == false)
                    {
                        model.Newsletter1 = reader.GetBoolean(72);
                    }
                    if (reader.IsDBNull(73) == false)
                    {
                        model.Statement1 = reader.GetBoolean(73);
                    }
                    if (reader.IsDBNull(74) == false)
                    {
                        model.Booklet1 = reader.GetBoolean(74);
                    }
                    if (reader.IsDBNull(75) == false)
                    {
                        model.CommentManualType = reader.GetString(75);
                    }
                    if (reader.IsDBNull(76) == false)
                    {
                        model.FinishingFormat = reader.GetString(76);
                    }
                    if (reader.IsDBNull(77) == false)
                    {
                        model.FoldingType = reader.GetString(77);
                    }
                    if (reader.IsDBNull(78) == false)
                    {
                        model.Sealing1 = reader.GetBoolean(78);
                    }
                    if (reader.IsDBNull(79) == false)
                    {
                        model.Tearing1 = reader.GetBoolean(79);
                    }
                    if (reader.IsDBNull(80) == false)
                    {
                        model.BarcodeLabel1 = reader.GetBoolean(80);
                    }
                    if (reader.IsDBNull(81) == false)
                    {
                        model.Cutting1 = reader.GetBoolean(81);
                    }
                    if (reader.IsDBNull(82) == false)
                    {
                        model.StickingOf1 = reader.GetString(82);
                    }
                    if (reader.IsDBNull(83) == false)
                    {
                        model.AddLabel1 = reader.GetBoolean(83);
                    }
                    if (reader.IsDBNull(84) == false)
                    {
                        model.Sticker1 = reader.GetBoolean(84);
                    }
                    if (reader.IsDBNull(85) == false)
                    {
                        model.Chesire1 = reader.GetBoolean(85);
                    }
                    if (reader.IsDBNull(86) == false)
                    {
                        model.Tuck_In1 = reader.GetBoolean(86);
                    }
                    if (reader.IsDBNull(87) == false)
                    {
                        model.Bursting1 = reader.GetBoolean(87);
                    }
                    if (reader.IsDBNull(88) == false)
                    {
                        model.Sealed1 = reader.GetBoolean(88);
                    }
                    if (reader.IsDBNull(89) == false)
                    {
                        model.Folding1 = reader.GetBoolean(89);
                    }
                    if (reader.IsDBNull(90) == false)
                    {
                        model.Unsealed1 = reader.GetBoolean(90);
                    }
                    if (reader.IsDBNull(91) == false)
                    {
                        model.Letter1 = reader.GetBoolean(91);
                    }
                    if (reader.IsDBNull(92) == false)
                    {
                        model.FinishingInst = reader.GetString(92);
                    }
                    if (reader.IsDBNull(93) == false)
                    {
                        model.IT_SysNotes = reader.GetString(93);
                    }
                    if (reader.IsDBNull(94) == false)
                    {
                        model.Produc_PlanningNotes = reader.GetString(94);
                    }
                    if (reader.IsDBNull(95) == false)
                    {
                        model.PurchasingNotes = reader.GetString(95);
                    }
                    if (reader.IsDBNull(96) == false)
                    {
                        model.EngineeringNotes = reader.GetString(96);
                    }
                    if (reader.IsDBNull(97) == false)
                    {
                        model.ArtworkNotes = reader.GetString(97);
                    }
                    if (reader.IsDBNull(98) == false)
                    {
                        model.Acc_BillingNotes = reader.GetString(98);
                    }
                    if (reader.IsDBNull(99) == false)
                    {
                        model.DCPNotes = reader.GetString(99);
                    }
                    if (reader.IsDBNull(100) == false)
                    {
                        model.PostingInfo = reader.GetString(100);
                    }
                }

                viewJobInstructionList.Add(model);
            }
            cn.Close();
        }
    }

    [ValidateInput(false)]
    public ActionResult ManageKPI(string set)
    {
        string tempId;

        string CreatedOnDate; string LastWeekFocus; string ThisWeekFocus; string HelpNeeded; string Comment;
        string StartDate; string EndDate; Guid kpiID;

        List<ManageKPI> listTemp = new List<ManageKPI>();

        var PIC = @Session["Fullname"];
        var Role = @Session["Role"];

        if (Role != "Head of Marketing ")
        {
            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn2))
            {

                cn2.Open();

                command.CommandText = @"SELECT CreatedOnDate,LastWeekFocus,ThisWeekFocus,HelpNeeded,Comment,StartDate,EndDate,kpiID
                                   FROM [IflowSeed].[dbo].[ManageKPI] WHERE PIC = @PIC";

                command.Parameters.AddWithValue("@PIC", PIC.ToString());

                var reader = command.ExecuteReader();
                QuotationModel model = new QuotationModel();
                {
                    while (reader.Read())
                    {

                        CreatedOnDate = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(0));
                        LastWeekFocus = reader.GetString(1);
                        ThisWeekFocus = reader.GetString(2);
                        HelpNeeded = reader.GetString(3);
                        Comment = reader.GetString(4);
                        StartDate = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(5));
                        EndDate = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(6));
                        kpiID = reader.GetGuid(7);
                        ViewBag.kpiID = reader.GetGuid(7);
                        //tempId = reader.GetGuid(7);
                        listTemp.Add(new ManageKPI { CreatedOnDate = CreatedOnDate, LastWeekFocus = LastWeekFocus, ThisWeekFocus = ThisWeekFocus, HelpNeeded = HelpNeeded, Comment = Comment, StartDate = StartDate, EndDate = EndDate, kpiID = kpiID });

                    }
                    cn2.Close();

                }
            }
            ViewBag.TbleKPI = listTemp;

            //return View(model);

        }
        if (Role == "Head of Marketing ")
        {
            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn2))
            {

                cn2.Open();

                command.CommandText = @"SELECT CreatedOnDate,LastWeekFocus,ThisWeekFocus,HelpNeeded,Comment,StartDate,EndDate,kpiID
                                   FROM [IflowSeed].[dbo].[ManageKPI] WHERE PIC = @PIC";

                command.Parameters.AddWithValue("@PIC", PIC.ToString());

                var reader = command.ExecuteReader();
                QuotationModel model = new QuotationModel();
                {
                    while (reader.Read())
                    {

                        CreatedOnDate = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(0));
                        LastWeekFocus = reader.GetString(1);
                        ThisWeekFocus = reader.GetString(2);
                        HelpNeeded = reader.GetString(3);
                        Comment = reader.GetString(4);
                        StartDate = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(5));
                        EndDate = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(6));
                        kpiID = reader.GetGuid(7);
                        ViewBag.kpiID = reader.GetGuid(7);
                        //tempId = reader.GetGuid(7);
                        listTemp.Add(new ManageKPI { CreatedOnDate = CreatedOnDate, LastWeekFocus = LastWeekFocus, ThisWeekFocus = ThisWeekFocus, HelpNeeded = HelpNeeded, Comment = Comment, StartDate = StartDate, EndDate = EndDate, kpiID = kpiID });

                    }
                    cn2.Close();

                }
            }
            ViewBag.TbleKPI = listTemp;

            //return View(model);

        }



        return View();

    }
    [ValidateInput(false)]
    public ActionResult StoreKPI(string set, string LastWeekFocus, string ThisWeekFocus, string HelpNeeded, string comment, string StartDate, string EndDate)
    {
        var PIC = @Session["Fullname"];
        using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command2 = new SqlCommand("", cn2))
        {
            cn2.Open();
            command2.CommandText = @"SELECT ThisWeekFocus FROM [IflowSeed].[dbo].[ManageKPI] WHERE PIC = @PIC ORDER BY CreatedOnDate";
            command2.Parameters.AddWithValue("@PIC", PIC.ToString());

            var reader = command2.ExecuteReader();
            while (reader.Read())
            {
                if (reader.IsDBNull(0) == false)
                {
                    //ThisWeekFocus = reader.GetString(0);
                    ViewBag.LastWeekFocus = reader.GetString(0);
                }
            }
            cn2.Close();
        }

        if (set == "newAdd")
        {

            string CreatedOnDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            //get.CreatedOn = Convert.ToDateTime(createdDate);
            //get.dateCreated = Convert.ToDateTime(createdDate);

            Guid kpiID = Guid.NewGuid();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"INSERT INTO [IflowSeed].[dbo].[ManageKPI] (kpiID, PIC, LastWeekFocus, CreatedOnDate, ThisWeekFocus, HelpNeeded, comment, StartDate, EndDate) VALUES (@kpiID, @PIC, @LastWeekFocus, @CreatedOnDate, @ThisWeekFocus, @HelpNeeded, @comment, @StartDate, @EndDate)";

                //ManageKPI model = new ManageKPI();
                command.Parameters.AddWithValue("@kpiID", kpiID);
                command.Parameters.AddWithValue("@PIC", PIC);
                command.Parameters.AddWithValue("@LastWeekFocus", LastWeekFocus);
                command.Parameters.AddWithValue("@CreatedOnDate", CreatedOnDate);
                command.Parameters.AddWithValue("@ThisWeekFocus", ThisWeekFocus);
                command.Parameters.AddWithValue("@HelpNeeded", HelpNeeded);
                command.Parameters.AddWithValue("@comment", comment);
                if (!string.IsNullOrEmpty(StartDate))
                {
                    string aaa = Convert.ToDateTime(StartDate).ToString("yyyy-MM-dd");
                    command.Parameters.AddWithValue("@StartDate", aaa);
                }
                else
                {
                    command.Parameters.AddWithValue("@StartDate", CreatedOnDate);

                }
                if (!string.IsNullOrEmpty(EndDate))
                {
                    string bbb = Convert.ToDateTime(EndDate).ToString("yyyy-MM-dd");
                    command.Parameters.AddWithValue("@EndDate", bbb);
                }
                else
                {
                    command.Parameters.AddWithValue("@EndDate", CreatedOnDate);

                }
                command.ExecuteNonQuery();
                cn.Close();
            }
            return RedirectToAction("ManageKPI", "MBD");
        }


        return View();
    }
    [ValidateInput(false)]
    public ActionResult EditKPI(string set, string id, string kpiID, string PIC, string LastWeekFocus, string CreatedOnDate, string ThisWeekFocus, string HelpNeeded, string Comment, string StartDate, string EndDate)
    {
        if (set != "update")
        {
            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn2))
            {
                cn2.Open();
                command.CommandText = @"SELECT kpiID, PIC, LastWeekFocus, CreatedOnDate, ThisWeekFocus, HelpNeeded, Comment, StartDate, EndDate FROM [iflowSeed].[dbo].[ManageKPI]                               
                                          WHERE kpiID=@id";
                command.Parameters.AddWithValue("@id", id.ToString());
                var reader = command.ExecuteReader();
                while (reader.Read())
                {

                    if (reader.IsDBNull(1) == false)
                    {
                        PIC = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        LastWeekFocus = reader.GetString(2);
                        ViewBag.LastWeekFocus = LastWeekFocus;
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        CreatedOnDate = Convert.ToDateTime(reader.GetDateTime(3)).ToString("yyyy-MM-dd");
                        ViewBag.CreatedOnDate = CreatedOnDate;
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        ThisWeekFocus = reader.GetString(4);
                        ViewBag.ThisWeekFocus = ThisWeekFocus;
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        HelpNeeded = reader.GetString(5);
                        ViewBag.HelpNeeded = HelpNeeded;
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        Comment = reader.GetString(6);
                        ViewBag.Comment = Comment;
                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        StartDate = Convert.ToDateTime(reader.GetDateTime(7)).ToString("yyyy-MM-dd");
                        ViewBag.StartDate = StartDate;
                    }
                    if (reader.IsDBNull(8) == false)
                    {
                        EndDate = Convert.ToDateTime(reader.GetDateTime(8)).ToString("yyyy-MM-dd");
                        ViewBag.EndDate = EndDate;
                    }
                }
                cn2.Close();
                ViewBag.id = id;
            }
        }

        if (set == "update")
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"UPDATE  [IflowSeed].[dbo].[ManageKPI]
                                            SET LastWeekFocus = @LastWeekFocus ,ThisWeekFocus = @ThisWeekFocus, HelpNeeded = @HelpNeeded, Comment = @Comment, StartDate = @StartDate, EndDate = @EndDate
                                            WHERE kpiID=@id";
                command.Parameters.AddWithValue("@id", id.ToString());

                if (LastWeekFocus == null)
                {
                    command.Parameters.Add(new SqlParameter { ParameterName = "@LastWeekFocus", Value = DBNull.Value });
                }
                else
                {
                    command.Parameters.AddWithValue("@LastWeekFocus", LastWeekFocus);
                }
                if (ThisWeekFocus == null)
                {
                    command.Parameters.Add(new SqlParameter { ParameterName = "@ThisWeekFocus", Value = DBNull.Value });
                }
                else
                {
                    command.Parameters.AddWithValue("@ThisWeekFocus", ThisWeekFocus);
                }
                if (HelpNeeded == null)
                {
                    command.Parameters.Add(new SqlParameter { ParameterName = "@HelpNeeded", Value = DBNull.Value });
                }
                else
                {
                    command.Parameters.AddWithValue("@HelpNeeded", HelpNeeded);
                }
                if (Comment == null)
                {
                    command.Parameters.Add(new SqlParameter { ParameterName = "@Comment", Value = DBNull.Value });
                }
                else
                {
                    command.Parameters.AddWithValue("@Comment", Comment);
                }
                if (StartDate == null)
                {
                    command.Parameters.Add(new SqlParameter { ParameterName = "@StartDate", Value = DBNull.Value });
                }
                else
                {
                    command.Parameters.AddWithValue("@StartDate", StartDate);
                }
                if (EndDate == null)
                {
                    command.Parameters.Add(new SqlParameter { ParameterName = "@EndDate", Value = DBNull.Value });
                }
                else
                {
                    command.Parameters.AddWithValue("@EndDate", EndDate);
                }
                command.ExecuteNonQuery();
                cn.Close();
            }

            return RedirectToAction("ManageKPi", "MBD");
        }

        return View();
    }
    [ValidateInput(false)]
    public ActionResult DeleteKPI(String kpiID)
    {
        try
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"DELETE [IflowSeed].[dbo].[ManageKPI]                          
                                      WHERE kpiID = @kpiID";
                command.Parameters.AddWithValue("@kpiID", kpiID.ToString());
                command.ExecuteNonQuery();
                cn.Close();
                return Json(new { code = 0 });
            }
        }
        catch (Exception e)
        {
            return Json(new { code = 1 });
#pragma warning disable CS0162 // Unreachable code detected
            Console.WriteLine($"Generic Exception Handler: {e}");
#pragma warning restore CS0162 // Unreachable code detected
        }
    }
    //create customer id
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

    public ActionResult ReloadAttachment()
    {
        var Id = Session["Id"];

        List<SampleProduct> viewFileStore = new List<SampleProduct>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT Picture_FileId,Id
                                      FROM [IflowSeed].[dbo].[SampleProduct]  
                                      WHERE JobInstruction=@Id                                   
                                      ORDER BY Picture_FileId DESC";
            command.Parameters.AddWithValue("@Id", Id);
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
                        model.Id = reader.GetGuid(1);
                    }

                }
                viewFileStore.Add(model);
            }
            cn.Close();
            //return Json(new { data = viewFileStore }, JsonRequestBehavior.AllowGet);
            return Json(viewFileStore);
        }
    }


    public ActionResult UploadAttachment(SampleProduct ModelSample, string JobInstruction, string JobSheetNo)
    {
        var IdentityName = @Session["Fullname"];
        var Id = Session["Id"];
        var Customer_Name = Session["Customer_Name"];
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.IsRole = @Session["Role"];
        string Deptment = @Session["Department"].ToString();

        ViewBag.JobSheetNo = JobSheetNo;


        if (ModelSample.FileUploadFile != null && Id.ToString() != null && ModelSample.Set == "save")
        {
            var fileName = Path.GetFileName(ModelSample.FileUploadFile.FileName);
            var path = Path.Combine(Server.MapPath("~/FileStore"), fileName);
            ModelSample.FileUploadFile.SaveAs(path);

            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                Guid guidId = Guid.NewGuid();
                string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                cn2.Open();
                SqlCommand command;
                command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[SampleProduct] (Id,CreatedOn,Picture_FileId,JobInstruction,Picture_Extension,Code,CreateBy,JobSheetNo) values (@Id,@CreatedOn,@Picture_FileId,@JobInstruction,@Picture_Extension,@Code,@CreateBy,@JobSheetNo)", cn2);
                command.Parameters.AddWithValue("@Id", guidId);
                command.Parameters.AddWithValue("@CreatedOn", createdOn);
                command.Parameters.AddWithValue("@Picture_FileId", fileName.Trim());
                command.Parameters.AddWithValue("@JobInstruction", Id);
                command.Parameters.AddWithValue("@Picture_Extension", ModelSample.FileUploadFile.ContentType);
                command.Parameters.AddWithValue("@Code", "JI");
                command.Parameters.AddWithValue("@CreateBy", IdentityName.ToString());
                command.Parameters.AddWithValue("@JobSheetNo", JobSheetNo);

                command.ExecuteNonQuery();
                cn2.Close();

            }

            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn2.Open();
                SqlCommand command;
                command = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET JobInstructionId=@JobInstructionId WHERE Id=@Id", cn2);
                command.Parameters.AddWithValue("@JobInstructionId", Id);
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn2.Close();

            }


            return RedirectToAction("CreateNewJI", "MBD", new { Id = Id.ToString(), Customer_Name = Customer_Name.ToString() });
        }

        if (ModelSample.Set == "back")
        {
            return RedirectToAction("CreateNewJI", "MBD", new { Id = Id.ToString(), Customer_Name = Customer_Name.ToString() });
        }

        return View();
    }

    public ActionResult DeleteAttachment(string Id, string JobInstruction)
    {
        Guid SampleProductId = Guid.Empty;

        if (Id != null)
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Picture_FileId,JobInstruction
                                          FROM [IflowSeed].[dbo].[SampleProduct]
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
                            command3 = new SqlCommand("DELETE [IflowSeed].[dbo].[SampleProduct] WHERE Id=@Id", cn3);
                            command3.Parameters.AddWithValue("@Id", Id);
                            command3.ExecuteNonQuery();
                            cn3.Close();

                        }

                    }

                    if (reader.IsDBNull(1) == false)
                    {
                        SampleProductId = reader.GetGuid(1);
                        return RedirectToAction("CreateNewJI", "MBD", new { Id = Session["Id"].ToString() });
                    }
                }
                cn.Close();
            }
        }

        return RedirectToAction("CreateNewJI", "MBD", new { Id = Session["Id"].ToString() });
    }

    public ActionResult DownloadAttachment(string Id)
    {
        Guid SampleProductId = Guid.Empty;

        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT Picture_FileId,Picture_Extension,Id
                                      FROM [IflowSeed].[dbo].[SampleProduct]
                                      WHERE Id=@Id";
            command.Parameters.AddWithValue("@Id", Id.ToString());
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader.IsDBNull(0) == false)
                {
                    var Picture_FileId = reader.GetString(0);
                    var Picture_Extension = reader.GetString(1);
                    SampleProductId = reader.GetGuid(2);
                    var path = PathSource + Picture_FileId;
                    string contentType = Picture_Extension.ToString();
                    return File(path, contentType, Picture_FileId);
                }
            }
        }

        return RedirectToAction("CreateNewJI", "MBD", new { Id = Id.ToString() });
    }

    [ValidateInput(false)]
    public ActionResult SubmitToDeveloper(JobInstruction JobInstruction, string Id, string set, string tabs, string Customer_Name, string ProductName, string JobSheetNo,
                                             string SalesExecutiveBy, string Status,
                                             string ServiceLevel, string IsSlaCreaditCard, string JobClass, string IsSetPaper,
                                             string JobRequest, string ExpectedDateCompletionToGpoTxt, string QuotationRef, string ContractName,
                                             string Contact_Person, string JobType, string DeliveryChannel, string AccountsQty, string ImpressionQty,
                                             string PagesQty, string CycleTermTxt, string MailingDateTxt,
                                             string JoiningFiles, string TotalRecord, string InputFileName, string OutputFileName, string Sorting,
                                             string SortingMode, string Other, string DataPrintingRemark,
                                             string ArtworkStatus, string PaperStock, string TypeCode, string Paper, string PaperSize,
                                             string Grammage, string MaterialColour, string EnvelopeStock, string EnvelopeType, string EnvelopeSize,
                                             string EnvelopeGrammage, string EnvelopeColour, string EnvelopeWindow, string EnvWindowOpaque,
                                             string LabelStock, string LabelCutsheet, string OthersStock, string BalancedMaterial,
                                             string PlasticStock, string PlasticType, string PlasticSize, string PlasticThickness,
                                             string PrintingType, string PrintingOrientation, string GpoList, string RegisterMail,
                                             string OtherList, string BaseStockType, string FinishingSize, string AdditionalPrintingMark,
                                             string SortingCriteria, string PrintingInstr, string SortingInstr,
                                             string Letter, string Brochures_Leaflets,
                                             string ReplyEnvelope, string ImgOnStatement, string Booklet,
                                             string NumberOfInsert, string Magezine1, string Brochure1, string CarrierSheet1, string Newsletter1,
                                             string Statement1, string Booklet1, string CommentManualType, string FinishingFormat,
                                             string FoldingType, string Sealing1, string Tearing1, string BarcodeLabel1, string Cutting1,
                                             string StickingOf1, string AddLabel1, string Sticker1, string Chesire1, string Tuck_In1,
                                             string Bursting1, string Sealed1, string Folding1, string Unsealed1, string Letter1, string FinishingInst,
                                             string IT_SysNotes, string Produc_PlanningNotes, string PurchasingNotes, string EngineeringNotes)
    {
        if (set == "submit")
        {
            if (!string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(JobClass) && !string.IsNullOrEmpty(JobType) && !string.IsNullOrEmpty(DeliveryChannel) && !string.IsNullOrEmpty(ServiceLevel) && set == "submit")

            {
                string ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn.Open();
                    SqlCommand command;
                    command = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET STATUS='Waiting to Assign Programmer',ModifiedOn=@ModifiedOn WHERE Id=@Id", cn);
                    command.Parameters.AddWithValue("@ModifiedOn", ModifiedOn);
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                    cn.Close();

                    TempData["msg"] = "<script>alert('SUCCESSFULLY SUBMIT JI !');</script>";

                    return RedirectToAction("ManageJobInstruction", "MBD");


                }

            }
            else
            {
                TempData["msg"] = "<script>alert('PLEASE COMPLETE THE FORM DETAILS!');</script>";
            }

        }

        return RedirectToAction("ManageJobInstruction", "MBD");

    }





    List<JobInstruction> JobInstructionlist1 = new List<JobInstruction>();



    public string CycleDateOnTxt { get; private set; }
    public string CycleEndOnTxt { get; private set; }
    public Guid ScheduleHighlightId { get; private set; }

    public ActionResult ManageJIUnderDev(string Id, string ProductName, string product, string set, string Status)
    {
        if (set == "search") //ini kalu user search product
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass, JobType, JobSheetNo, JobRequest, SalesExecutiveBy, AssignByLeader, ProgrammerBy, StartDevDate, EndDevDate, Status, Complexity,
                                         AccountsQty,ImpressionQty,PagesQty     
                                         FROM [IflowSeed].[dbo].[JobInstruction]                                    
                                         WHERE ProductName LIKE @ProductName
                                         AND (Status = 'Waiting to Assign Programmer') OR(Status = 'Development Process') OR(Status = 'Development Complete')
                                         ORDER BY CreatedOn desc ";
                command.Parameters.AddWithValue("@ProductName", "%" + product + "%");
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
                            model.JobRequest = reader.GetDateTime(6);
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.SalesExecutiveBy = reader.GetString(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.AssignByLeader = reader.GetString(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            model.ProgrammerBy = reader.GetString(9);
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            model.StartDevDate = reader.GetDateTime(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            model.EndDevDate = reader.GetDateTime(11);
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            model.Status = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            model.Complexity = reader.GetString(13);
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            model.AccountsQty = reader.GetString(14);
                        }
                        if (reader.IsDBNull(15) == false)
                        {
                            model.ImpressionQty = reader.GetString(15);
                        }
                        if (reader.IsDBNull(16) == false)
                        {
                            model.PagesQty = reader.GetString(16);
                        }
                    }
                    JobInstructionlist1.Add(model);
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
                command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass, JobType, JobSheetNo, JobRequest, SalesExecutiveBy, AssignByLeader, ProgrammerBy, StartDevDate, EndDevDate, Status, Complexity,
                                        AccountsQty,ImpressionQty,PagesQty
                                        FROM [IflowSeed].[dbo].[JobInstruction]
                                        WHERE (Status = 'Waiting to Assign Programmer') OR (Status ='Development Process') OR (Status ='Development Complete')";
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
                            model.JobRequest = reader.GetDateTime(6);
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.SalesExecutiveBy = reader.GetString(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.AssignByLeader = reader.GetString(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            model.ProgrammerBy = reader.GetString(9);
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            model.StartDevDate = reader.GetDateTime(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            model.EndDevDate = reader.GetDateTime(11);
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            model.Status = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            model.Complexity = reader.GetString(13);
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            model.AccountsQty = reader.GetString(14);
                        }
                        if (reader.IsDBNull(15) == false)
                        {
                            model.ImpressionQty = reader.GetString(15);
                        }
                        if (reader.IsDBNull(16) == false)
                        {
                            model.PagesQty = reader.GetString(16);
                        }

                    }
                    JobInstructionlist1.Add(model);
                }
                cn.Close();
            }
        }

        if (set == "SubmitLIVE")
        {
            if (Status == "Development Complete")
            {


                using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                    cn3.Open();
                    SqlCommand command1;
                    command1 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET ModifiedOn=@ModifiedOn, STATUS='ITO' WHERE Id=@Id", cn3);
                    command1.Parameters.AddWithValue("@ModifiedOn", createdOn);
                    command1.Parameters.AddWithValue("@Id", Id);
                    command1.ExecuteNonQuery();
                    TempData["msg"] = "<script>alert('JI  SUCCESSFULLY LIVE !');</script>";

                    return RedirectToAction("ManageJIUnderDev", "MBD");
                }


            }

            else
            {
                TempData["msg"] = "<script>alert('THIS ACTION CANNOT BE PROCESS !');</script>";
            }

            return RedirectToAction("ManageJIUnderDev", "MBD");

        }
        return View(JobInstructionlist1); //hntr data ke ui

    }

    public ActionResult getExcelJI(JobInstruction get, string line, string Id, string set, string JobInstructionId, string tabs, string Customer_Name, string ProductName, string JobSheetNo,
                                     string SalesExecutiveBy, string Status,
                                     string ServiceLevel, string IsSlaCreaditCard, string JobClass, string IsSetPaper,
                                     string JobRequest, string ExpectedDateCompletionToGpo, string QuotationRef, string ContractName,
                                     string Contact_Person, string JobType, string DeliveryChannel, string AccountsQty, string ImpressionQty,
                                     string PagesQty, string CycleTerm, string MailingDate,
                                     string JoiningFiles, string TotalRecord, string InputFileName, string OutputFileName, string Sorting,
                                     string SortingMode, string Other, string DataPrintingRemark,
                                     string ArtworkStatus, string PaperStock, string TypeCode, string Paper, string PaperSize,
                                     string Grammage, string MaterialColour, string EnvelopeStock, string EnvelopeType, string EnvelopeSize,
                                     string EnvelopeGrammage, string EnvelopeColour, string EnvelopeWindow, string EnvWindowOpaque,
                                     string LabelStock, string LabelCutsheet, string OthersStock, string BalancedMaterial,
                                     string PlasticStock, string PlasticType, string PlasticSize, string PlasticThickness,
                                     string PrintingType, string PrintingOrientation, string GpoList, string RegisterMail,
                                     string OtherList, string BaseStockType, string FinishingSize, string AdditionalPrintingMark,
                                     string SortingCriteria, string PrintingInstr, string SortingInstr, string JobInstruction,
                                     string Picture_FileId, string Picture_Extension, string Letter, string Brochures_Leaflets,
                                     string ReplyEnvelope, string ImgOnStatement, string Booklet,
                                     string NumberOfInsert, string Magezine1, string Brochure1, string CarrierSheet1, string Newsletter1,
                                     string Statement1, string Booklet1, string CommentManualType, string FinishingFormat,
                                     string FoldingType, string Sealing1, string Tearing1, string BarcodeLabel1, string Cutting1,
                                     string StickingOf1, string AddLabel1, string Sticker1, string Chesire1, string Tuck_In1,
                                     string Bursting1, string Sealed1, string Folding1, string Unsealed1, string Letter1, string FinishingInst,
                                     string IT_SysNotes, string Produc_PlanningNotes, string PurchasingNotes, string EngineeringNotes,
                                     string ArtworkNotes, string Acc_BillingNotes, string DCPNotes, string PostingInfo)
    {
        //string[] parts = line.Split('|');
        //string Id = parts[0].Trim();
        //string ProcessType = parts[1].Trim();

        ViewBag.IsDepart = @Session["Department"];
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        {

            List<JobInstruction> gotlist = new List<JobInstruction>();
            cn.Open();
            SqlCommand command;
            command = new SqlCommand(@"SELECT Id,Customer_Name,ProductName,ServiceLevel,IsSlaCreaditCard,JobClass,IsSetPaper,JobRequest,ExpectedDateCompletionToGpo,QuotationRef,ContractName,Contact_Person,JobType,DeliveryChannel,AccountsQty,ImpressionQty,PagesQty,CycleTerm,MailingDate,
                                       JoiningFiles,TotalRecord,InputFileName,OutputFileName,Sorting,SortingMode,Other,DataPrintingRemark,
                                       ArtworkStatus,PaperStock,TypeCode,Paper,PaperSize,Grammage,MaterialColour,EnvelopeStock,EnvelopeType,EnvelopeSize,EnvelopeGrammage,EnvelopeColour,EnvelopeWindow,EnvWindowOpaque,LabelStock,LabelCutsheet,OthersStock,BalancedMaterial,PlasticStock,PlasticType,PlasticSize,PlasticThickness,
                                       PrintingType,PrintingOrientation,GpoList,RegisterMail,OtherList,BaseStockType,FinishingSize,AdditionalPrintingMark,SortingCriteria,PrintingInstr,SortingInstr,Letter,Brochures_Leaflets,ReplyEnvelope,ImgOnStatement,Booklet,
                                       NumberOfInsert,Magezine1,Brochure1,CarrierSheet1,Newsletter1,Statement1,Booklet1,CommentManualType,FinishingFormat,FoldingType,Sealing1,Tearing1,BarcodeLabel1,Cutting1,StickingOf1,AddLabel1,Sticker1,Chesire1,Tuck_In1,Bursting1,Sealed1,Folding1,Unsealed1,Letter1,FinishingInst,
                                       IT_SysNotes,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes,ArtworkNotes,Acc_BillingNotes,DCPNotes,PostingInfo,JobSheetNo
                                       FROM [IflowSeed].[dbo].[JobInstruction]
                                       WHERE Id = @Id", cn);
            command.Parameters.AddWithValue("@Id", Id.ToString());
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction list = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        list.Id = reader.GetGuid(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        list.Customer_Name = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        list.ProductName = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        list.ServiceLevel = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        list.IsSlaCreaditCard = reader.GetBoolean(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        list.JobClass = reader.GetString(5);
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        list.IsSetPaper = reader.GetBoolean(6);

                    }
                    if (reader.IsDBNull(7) == false)
                    {

                        list.JobRequest = reader.GetDateTime(7);
                    }
                    if (reader.IsDBNull(8) == false)
                    {
                        list.ExpectedDateCompletionToGpo = reader.GetDateTime(8);
                    }
                    if (reader.IsDBNull(9) == false)
                    {
                        list.QuotationRef = reader.GetString(9);
                    }
                    if (reader.IsDBNull(10) == false)
                    {
                        list.ContractName = reader.GetString(10);
                    }
                    if (reader.IsDBNull(11) == false)
                    {
                        list.Contact_Person = reader.GetString(11);
                    }
                    if (reader.IsDBNull(12) == false)
                    {
                        list.JobType = reader.GetString(12);
                    }
                    if (reader.IsDBNull(13) == false)
                    {
                        list.DeliveryChannel = reader.GetString(13);
                    }
                    if (reader.IsDBNull(14) == false)
                    {
                        list.AccountsQty = reader.GetString(14);
                    }
                    if (reader.IsDBNull(15) == false)
                    {
                        list.ImpressionQty = reader.GetString(15);
                    }
                    if (reader.IsDBNull(16) == false)
                    {
                        list.PagesQty = reader.GetString(16);
                    }
                    if (reader.IsDBNull(17) == false)
                    {
                        list.CycleTerm = reader.GetDateTime(17);
                    }
                    if (reader.IsDBNull(18) == false)
                    {
                        list.MailingDate = reader.GetDateTime(18);
                    }

                    if (reader.IsDBNull(19) == false)
                    {
                        list.JoiningFiles = reader.GetString(19);
                    }
                    if (reader.IsDBNull(20) == false)
                    {
                        list.TotalRecord = reader.GetString(20);
                    }
                    if (reader.IsDBNull(21) == false)
                    {
                        list.InputFileName = reader.GetString(21);
                    }
                    if (reader.IsDBNull(22) == false)
                    {
                        list.OutputFileName = reader.GetString(22);
                    }
                    if (reader.IsDBNull(23) == false)
                    {
                        list.Sorting = reader.GetString(23);
                    }
                    if (reader.IsDBNull(24) == false)
                    {
                        list.SortingMode = reader.GetString(24);
                    }
                    if (reader.IsDBNull(25) == false)
                    {
                        list.Other = reader.GetString(25);
                    }
                    if (reader.IsDBNull(26) == false)
                    {
                        list.DataPrintingRemark = reader.GetString(26);
                    }
                    if (reader.IsDBNull(27) == false)
                    {
                        list.ArtworkStatus = reader.GetString(27);
                    }
                    if (reader.IsDBNull(28) == false)
                    {
                        list.PaperStock = reader.GetString(28);
                    }
                    if (reader.IsDBNull(29) == false)
                    {
                        list.TypeCode = reader.GetString(29);
                    }
                    if (reader.IsDBNull(30) == false)
                    {
                        list.Paper = reader.GetString(30);
                    }
                    if (reader.IsDBNull(31) == false)
                    {
                        list.PaperSize = reader.GetString(31);
                    }
                    if (reader.IsDBNull(32) == false)
                    {
                        list.Grammage = reader.GetString(32);
                    }
                    if (reader.IsDBNull(33) == false)
                    {
                        list.MaterialColour = reader.GetString(33);
                    }
                    if (reader.IsDBNull(34) == false)
                    {
                        list.EnvelopeStock = reader.GetString(34);
                    }
                    if (reader.IsDBNull(35) == false)
                    {
                        list.EnvelopeType = reader.GetString(35);
                    }
                    if (reader.IsDBNull(36) == false)
                    {
                        list.EnvelopeSize = reader.GetString(36);
                    }
                    if (reader.IsDBNull(37) == false)
                    {
                        list.EnvelopeGrammage = reader.GetString(37);
                    }
                    if (reader.IsDBNull(38) == false)
                    {
                        list.EnvelopeColour = reader.GetString(38);
                    }
                    if (reader.IsDBNull(39) == false)
                    {
                        list.EnvelopeWindow = reader.GetString(39);
                    }
                    if (reader.IsDBNull(40) == false)
                    {
                        list.EnvWindowOpaque = reader.GetString(40);
                    }
                    if (reader.IsDBNull(41) == false)
                    {
                        list.LabelStock = reader.GetString(41);
                    }
                    if (reader.IsDBNull(42) == false)
                    {
                        list.LabelCutsheet = reader.GetString(42);
                    }
                    if (reader.IsDBNull(43) == false)
                    {
                        list.OthersStock = reader.GetString(43);
                    }
                    if (reader.IsDBNull(44) == false)
                    {
                        list.BalancedMaterial = reader.GetString(44);
                    }
                    if (reader.IsDBNull(45) == false)
                    {
                        list.PlasticStock = reader.GetString(45);
                    }
                    if (reader.IsDBNull(46) == false)
                    {
                        list.PlasticType = reader.GetString(46);
                    }
                    if (reader.IsDBNull(47) == false)
                    {
                        list.PlasticSize = reader.GetString(47);
                    }
                    if (reader.IsDBNull(48) == false)
                    {
                        list.PlasticThickness = reader.GetString(48);
                    }
                    if (reader.IsDBNull(49) == false)
                    {
                        list.PrintingType = reader.GetString(49);
                    }
                    if (reader.IsDBNull(50) == false)
                    {
                        list.PrintingOrientation = reader.GetString(50);
                    }
                    if (reader.IsDBNull(51) == false)
                    {
                        list.GpoList = reader.GetBoolean(51);

                    }
                    if (reader.IsDBNull(52) == false)
                    {
                        list.RegisterMail = reader.GetBoolean(52);

                    }
                    if (reader.IsDBNull(53) == false)
                    {
                        list.OtherList = reader.GetString(53);
                    }
                    if (reader.IsDBNull(54) == false)
                    {
                        list.BaseStockType = reader.GetString(54);
                    }
                    if (reader.IsDBNull(55) == false)
                    {
                        list.FinishingSize = reader.GetString(55);
                    }
                    if (reader.IsDBNull(56) == false)
                    {
                        list.AdditionalPrintingMark = reader.GetString(56);
                    }
                    if (reader.IsDBNull(57) == false)
                    {
                        list.SortingCriteria = reader.GetString(57);
                    }
                    if (reader.IsDBNull(58) == false)
                    {
                        list.PrintingInstr = reader.GetString(58);
                    }
                    if (reader.IsDBNull(59) == false)
                    {
                        list.SortingInstr = reader.GetString(59);
                    }
                    if (reader.IsDBNull(60) == false)
                    {
                        list.Letter = reader.GetBoolean(60);

                    }
                    if (reader.IsDBNull(61) == false)
                    {
                        list.Brochures_Leaflets = reader.GetBoolean(61);

                    }
                    if (reader.IsDBNull(62) == false)
                    {
                        list.ReplyEnvelope = reader.GetBoolean(62);

                    }
                    if (reader.IsDBNull(63) == false)
                    {
                        list.ImgOnStatement = reader.GetBoolean(63);

                    }
                    if (reader.IsDBNull(64) == false)
                    {
                        list.Booklet = reader.GetBoolean(64);

                    }
                    if (reader.IsDBNull(65) == false)
                    {
                        list.NumberOfInsert = reader.GetString(65);
                    }
                    if (reader.IsDBNull(66) == false)
                    {
                        list.Magezine1 = reader.GetBoolean(66);

                    }
                    if (reader.IsDBNull(67) == false)
                    {
                        list.Brochure1 = reader.GetBoolean(67);

                    }
                    if (reader.IsDBNull(68) == false)
                    {
                        list.CarrierSheet1 = reader.GetBoolean(68);

                    }
                    if (reader.IsDBNull(69) == false)
                    {
                        list.Newsletter1 = reader.GetBoolean(69);

                    }
                    if (reader.IsDBNull(70) == false)
                    {
                        list.Statement1 = reader.GetBoolean(70);

                    }
                    if (reader.IsDBNull(71) == false)
                    {
                        list.Booklet1 = reader.GetBoolean(71);

                    }
                    if (reader.IsDBNull(72) == false)
                    {
                        list.CommentManualType = reader.GetString(72);
                    }
                    if (reader.IsDBNull(73) == false)
                    {
                        list.FinishingFormat = reader.GetString(73);
                    }
                    if (reader.IsDBNull(74) == false)
                    {
                        list.FoldingType = reader.GetString(74);
                    }
                    if (reader.IsDBNull(75) == false)
                    {
                        list.Sealing1 = reader.GetBoolean(75);

                    }
                    if (reader.IsDBNull(76) == false)
                    {
                        list.Tearing1 = reader.GetBoolean(76);

                    }
                    if (reader.IsDBNull(77) == false)
                    {
                        list.BarcodeLabel1 = reader.GetBoolean(77);

                    }
                    if (reader.IsDBNull(78) == false)
                    {
                        list.Cutting1 = reader.GetBoolean(78);

                    }
                    if (reader.IsDBNull(79) == false)
                    {
                        list.StickingOf1 = reader.GetString(79);
                    }
                    if (reader.IsDBNull(80) == false)
                    {
                        list.AddLabel1 = reader.GetBoolean(80);

                    }
                    if (reader.IsDBNull(81) == false)
                    {
                        list.Sticker1 = reader.GetBoolean(81);

                    }
                    if (reader.IsDBNull(82) == false)
                    {
                        list.Chesire1 = reader.GetBoolean(82);

                    }
                    if (reader.IsDBNull(83) == false)
                    {
                        list.Tuck_In1 = reader.GetBoolean(83);

                    }
                    if (reader.IsDBNull(84) == false)
                    {
                        list.Bursting1 = reader.GetBoolean(84);

                    }
                    if (reader.IsDBNull(85) == false)
                    {
                        list.Sealed1 = reader.GetBoolean(85);

                    }
                    if (reader.IsDBNull(86) == false)
                    {
                        list.Folding1 = reader.GetBoolean(86);

                    }
                    if (reader.IsDBNull(87) == false)
                    {
                        list.Unsealed1 = reader.GetBoolean(87);

                    }
                    if (reader.IsDBNull(88) == false)
                    {
                        list.Letter1 = reader.GetBoolean(4);

                    }
                    if (reader.IsDBNull(89) == false)
                    {
                        list.FinishingInst = reader.GetString(89);
                    }
                    if (reader.IsDBNull(90) == false)
                    {
                        list.IT_SysNotes = reader.GetString(90);
                    }
                    if (reader.IsDBNull(91) == false)
                    {
                        list.Produc_PlanningNotes = reader.GetString(91);
                    }
                    if (reader.IsDBNull(92) == false)
                    {
                        list.PurchasingNotes = reader.GetString(92);
                    }
                    if (reader.IsDBNull(93) == false)
                    {
                        list.EngineeringNotes = reader.GetString(93);
                    }
                    if (reader.IsDBNull(94) == false)
                    {
                        list.ArtworkNotes = reader.GetString(94);
                    }
                    if (reader.IsDBNull(95) == false)
                    {
                        list.Acc_BillingNotes = reader.GetString(95);
                    }
                    if (reader.IsDBNull(96) == false)
                    {
                        list.DCPNotes = reader.GetString(96);
                    }
                    if (reader.IsDBNull(97) == false)
                    {
                        list.PostingInfo = reader.GetString(97);
                    }
                    if (reader.IsDBNull(98) == false)
                    {
                        list.JobSheetNo = reader.GetString(98);
                    }

                }
                gotlist.Add(list);

            }
            cn.Close();
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("JOB INSTRUCTION");
            workSheet.TabColor = System.Drawing.Color.Black;

            workSheet.DefaultRowHeight = 12;
            workSheet.Row(1).Height = 20;
            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(1).Style.Font.Bold = true;
            workSheet.Cells[1, 1].Value = "JOBSHEET NO";
            workSheet.Cells[1, 2].Value = "SERVICE LEVEL";
            workSheet.Cells[1, 3].Value = "IS SLA CREDIT CARD";
            workSheet.Cells[1, 4].Value = "IS SET PAPER";
            workSheet.Cells[1, 5].Value = "JOB CLASS";
            workSheet.Cells[1, 6].Value = "JOB REQ DATE";
            workSheet.Cells[1, 7].Value = "EXP COMPLETION GPO";
            workSheet.Cells[1, 8].Value = "QUOTATION REFF";
            workSheet.Cells[1, 9].Value = "CONTRACT REFF";
            workSheet.Cells[1, 10].Value = "CONTACT PERSON";
            workSheet.Cells[1, 11].Value = "JOB TYPE";
            workSheet.Cells[1, 12].Value = "DELIVERY CHANNEL";
            workSheet.Cells[1, 13].Value = "ACCOUNTS QTY";
            workSheet.Cells[1, 14].Value = "IMPRESSION QTY";
            workSheet.Cells[1, 15].Value = "PAGES QTY";
            workSheet.Cells[1, 16].Value = "CYCLE TERM";
            workSheet.Cells[1, 17].Value = "MAILING DATE";

            int recordIndex = 2;
            foreach (var CLM in gotlist)
            {
                workSheet.Cells[recordIndex, 1].Value = CLM.JobSheetNo;
                workSheet.Cells[recordIndex, 2].Value = CLM.ServiceLevel;
                workSheet.Cells[recordIndex, 3].Value = CLM.IsSlaCreaditCard;
                workSheet.Cells[recordIndex, 4].Value = CLM.IsSetPaper;
                workSheet.Cells[recordIndex, 5].Value = CLM.JobClass;
                workSheet.Cells[recordIndex, 6].Value = CLM.JobRequest;
                workSheet.Cells[recordIndex, 7].Value = CLM.ExpectedDateCompletionToGpo;
                workSheet.Cells[recordIndex, 8].Value = CLM.QuotationRef;
                workSheet.Cells[recordIndex, 9].Value = CLM.ContractName;
                workSheet.Cells[recordIndex, 10].Value = CLM.Contact_Person;
                workSheet.Cells[recordIndex, 11].Value = CLM.JobType;
                workSheet.Cells[recordIndex, 12].Value = CLM.DeliveryChannel;
                workSheet.Cells[recordIndex, 13].Value = CLM.AccountsQty;
                workSheet.Cells[recordIndex, 14].Value = CLM.ImpressionQty;
                workSheet.Cells[recordIndex, 15].Value = CLM.PagesQty;
                workSheet.Cells[recordIndex, 16].Value = CLM.CycleTerm;
                workSheet.Cells[recordIndex, 17].Value = CLM.MailingDate;

                recordIndex++;
            }
            workSheet.Column(1).AutoFit();
            workSheet.Column(2).AutoFit();
            workSheet.Column(3).AutoFit();
            workSheet.Column(4).AutoFit();
            workSheet.Column(5).AutoFit();
            workSheet.Column(6).AutoFit();
            workSheet.Column(7).AutoFit();
            workSheet.Column(8).AutoFit();
            workSheet.Column(9).AutoFit();
            workSheet.Column(10).AutoFit();
            workSheet.Column(11).AutoFit();
            workSheet.Column(12).AutoFit();
            workSheet.Column(13).AutoFit();
            workSheet.Column(14).AutoFit();
            workSheet.Column(15).AutoFit();
            workSheet.Column(16).AutoFit();
            workSheet.Column(17).AutoFit();

            var workSheet2 = excel.Workbook.Worksheets.Add("DATA PROCESS");
            workSheet2.TabColor = System.Drawing.Color.Black;

            workSheet2.DefaultRowHeight = 12;
            workSheet2.Row(1).Height = 20;
            workSheet2.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet2.Row(1).Style.Font.Bold = true;
            workSheet2.Cells[1, 1].Value = "JOINING FILES";
            workSheet2.Cells[1, 2].Value = "TOTAL RECORDS";
            workSheet2.Cells[1, 3].Value = "INPUT FILE";
            workSheet2.Cells[1, 4].Value = "OUTPUT FILE";
            workSheet2.Cells[1, 5].Value = "SORTING";
            workSheet2.Cells[1, 6].Value = "SORTING MODE";
            workSheet2.Cells[1, 7].Value = "OTHER";
            workSheet2.Cells[1, 8].Value = "DATA PRINT REMARK";


            int recordIndex2 = 2;
            foreach (var CLM in gotlist)
            {
                workSheet2.Cells[recordIndex, 1].Value = CLM.JoiningFiles;
                workSheet2.Cells[recordIndex, 2].Value = CLM.TotalRecord;
                workSheet2.Cells[recordIndex, 3].Value = CLM.InputFileName;
                workSheet2.Cells[recordIndex, 4].Value = CLM.OutputFileName;
                workSheet2.Cells[recordIndex, 5].Value = CLM.Sorting;
                workSheet2.Cells[recordIndex, 6].Value = CLM.SortingMode;
                workSheet2.Cells[recordIndex, 7].Value = CLM.Other;
                workSheet2.Cells[recordIndex, 8].Value = CLM.DataPrintingRemark;


                recordIndex2++;
            }
            workSheet2.Column(1).AutoFit();
            workSheet2.Column(2).AutoFit();
            workSheet2.Column(3).AutoFit();
            workSheet2.Column(4).AutoFit();
            workSheet2.Column(5).AutoFit();
            workSheet2.Column(6).AutoFit();
            workSheet2.Column(7).AutoFit();
            workSheet2.Column(8).AutoFit();

            var workSheet3 = excel.Workbook.Worksheets.Add("MATERIAL INFO");
            workSheet3.TabColor = System.Drawing.Color.Black;

            workSheet3.DefaultRowHeight = 12;
            workSheet3.Row(1).Height = 20;
            workSheet3.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet3.Row(1).Style.Font.Bold = true;
            workSheet3.Cells[1, 1].Value = "ARTWORK STATUS";
            workSheet3.Cells[1, 2].Value = "PAPER STOCK";
            workSheet3.Cells[1, 3].Value = "PAPER TYPE";
            workSheet3.Cells[1, 4].Value = "PAPER NAME";
            workSheet3.Cells[1, 5].Value = "PAPER SIZE";
            workSheet3.Cells[1, 6].Value = "GRAMMAGE";
            workSheet3.Cells[1, 7].Value = "MATERIAL COLOR";
            workSheet3.Cells[1, 8].Value = "ENVELOPE STOCK";
            workSheet3.Cells[1, 9].Value = "ENVELOPE TYPE";
            workSheet3.Cells[1, 10].Value = "ENVELOPE SIZE";
            workSheet3.Cells[1, 11].Value = "ENVELOPE GRAMMAGE";
            workSheet3.Cells[1, 12].Value = "ENVELOPE COLOR";
            workSheet3.Cells[1, 13].Value = "WINDOW";
            workSheet3.Cells[1, 14].Value = "OTHER OPAQUE";
            workSheet3.Cells[1, 15].Value = "LABEL STOCK";
            workSheet3.Cells[1, 16].Value = "LABEL CUT SHEET";
            workSheet3.Cells[1, 17].Value = "PILLOW CASE STOCK";
            workSheet3.Cells[1, 18].Value = "PILLOW CASE TYPE";
            workSheet3.Cells[1, 19].Value = "PILLOW CASE SIZE";
            workSheet3.Cells[1, 20].Value = "PILLOW CASE THCKNESS";
            workSheet3.Cells[1, 21].Value = "OTHER STOCK";
            workSheet3.Cells[1, 22].Value = "BALANCE MATERIAL";


            int recordIndex3 = 2;
            foreach (var CLM in gotlist)
            {
                workSheet3.Cells[recordIndex, 1].Value = CLM.ArtworkStatus;
                workSheet3.Cells[recordIndex, 2].Value = CLM.PaperStock;
                workSheet3.Cells[recordIndex, 3].Value = CLM.TypeCode;
                workSheet3.Cells[recordIndex, 4].Value = CLM.Paper;
                workSheet3.Cells[recordIndex, 5].Value = CLM.PaperSize;
                workSheet3.Cells[recordIndex, 6].Value = CLM.Grammage;
                workSheet3.Cells[recordIndex, 7].Value = CLM.MaterialColour;
                workSheet3.Cells[recordIndex, 8].Value = CLM.EnvelopeStock;
                workSheet3.Cells[recordIndex, 9].Value = CLM.EnvelopeType;
                workSheet3.Cells[recordIndex, 10].Value = CLM.EnvelopeSize;
                workSheet3.Cells[recordIndex, 11].Value = CLM.EnvelopeGrammage;
                workSheet3.Cells[recordIndex, 12].Value = CLM.EnvelopeColour;
                workSheet3.Cells[recordIndex, 13].Value = CLM.EnvelopeWindow;
                workSheet3.Cells[recordIndex, 14].Value = CLM.EnvWindowOpaque;
                workSheet3.Cells[recordIndex, 15].Value = CLM.LabelStock;
                workSheet3.Cells[recordIndex, 16].Value = CLM.LabelCutsheet;
                workSheet3.Cells[recordIndex, 17].Value = CLM.PlasticStock;
                workSheet3.Cells[recordIndex, 18].Value = CLM.PlasticType;
                workSheet3.Cells[recordIndex, 19].Value = CLM.PlasticSize;
                workSheet3.Cells[recordIndex, 20].Value = CLM.PlasticThickness;
                workSheet3.Cells[recordIndex, 21].Value = CLM.OthersStock;
                workSheet3.Cells[recordIndex, 22].Value = CLM.BalancedMaterial;


                recordIndex3++;
            }
            workSheet3.Column(1).AutoFit();
            workSheet3.Column(2).AutoFit();
            workSheet3.Column(3).AutoFit();
            workSheet3.Column(4).AutoFit();
            workSheet3.Column(5).AutoFit();
            workSheet3.Column(6).AutoFit();
            workSheet3.Column(7).AutoFit();
            workSheet3.Column(8).AutoFit();
            workSheet3.Column(9).AutoFit();
            workSheet3.Column(10).AutoFit();
            workSheet3.Column(11).AutoFit();
            workSheet3.Column(12).AutoFit();
            workSheet3.Column(13).AutoFit();
            workSheet3.Column(14).AutoFit();
            workSheet3.Column(15).AutoFit();
            workSheet3.Column(16).AutoFit();
            workSheet3.Column(17).AutoFit();
            workSheet3.Column(18).AutoFit();
            workSheet3.Column(19).AutoFit();
            workSheet3.Column(20).AutoFit();
            workSheet3.Column(21).AutoFit();
            workSheet3.Column(22).AutoFit();

            var workSheet4 = excel.Workbook.Worksheets.Add("PRODUCTION LIST");
            workSheet4.TabColor = System.Drawing.Color.Black;

            workSheet4.DefaultRowHeight = 12;
            workSheet4.Row(1).Height = 20;
            workSheet4.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet4.Row(1).Style.Font.Bold = true;
            workSheet4.Cells[1, 1].Value = "PRINTING TYPE";
            workSheet4.Cells[1, 2].Value = "PRINTING ORIENTATION";
            workSheet4.Cells[1, 3].Value = "GPO LIST";
            workSheet4.Cells[1, 4].Value = "REGISTER MAIL";
            workSheet4.Cells[1, 5].Value = "OTHER LIST";
            workSheet4.Cells[1, 6].Value = "BASE STOCK TYPE";
            workSheet4.Cells[1, 7].Value = "FINISHING SIZE";
            workSheet4.Cells[1, 8].Value = "ADDITIONAL PRINTING REMARK";
            workSheet4.Cells[1, 9].Value = "SORTING CRITERIA";
            workSheet4.Cells[1, 10].Value = "PRINTING INSTRUCTION";
            workSheet4.Cells[1, 11].Value = "SORTING INSTRUCTION";
            workSheet4.Cells[1, 12].Value = "LABEL";
            workSheet4.Cells[1, 13].Value = "BROCHURE/LEAFLETS";
            workSheet4.Cells[1, 14].Value = "REPLY ENVELOPE";
            workSheet4.Cells[1, 15].Value = "IMG ON STATEMENT";
            workSheet4.Cells[1, 16].Value = "BOOKLET";


            int recordIndex4 = 2;
            foreach (var CLM in gotlist)
            {
                workSheet4.Cells[recordIndex, 1].Value = CLM.PrintingType;
                workSheet4.Cells[recordIndex, 2].Value = CLM.PrintingOrientation;
                workSheet4.Cells[recordIndex, 3].Value = CLM.GpoList;
                workSheet4.Cells[recordIndex, 4].Value = CLM.RegisterMail;
                workSheet4.Cells[recordIndex, 5].Value = CLM.OtherList;
                workSheet4.Cells[recordIndex, 6].Value = CLM.BaseStockType;
                workSheet4.Cells[recordIndex, 7].Value = CLM.FinishingSize;
                workSheet4.Cells[recordIndex, 8].Value = CLM.AdditionalPrintingMark;
                workSheet4.Cells[recordIndex, 9].Value = CLM.SortingCriteria;
                workSheet4.Cells[recordIndex, 10].Value = CLM.PrintingInstr;
                workSheet4.Cells[recordIndex, 11].Value = CLM.SortingInstr;
                workSheet4.Cells[recordIndex, 12].Value = CLM.Letter;
                workSheet4.Cells[recordIndex, 13].Value = CLM.Brochures_Leaflets;
                workSheet4.Cells[recordIndex, 14].Value = CLM.ReplyEnvelope;
                workSheet4.Cells[recordIndex, 15].Value = CLM.ImgOnStatement;
                workSheet4.Cells[recordIndex, 16].Value = CLM.Booklet;


                recordIndex4++;
            }
            workSheet4.Column(1).AutoFit();
            workSheet4.Column(2).AutoFit();
            workSheet4.Column(3).AutoFit();
            workSheet4.Column(4).AutoFit();
            workSheet4.Column(5).AutoFit();
            workSheet4.Column(6).AutoFit();
            workSheet4.Column(7).AutoFit();
            workSheet4.Column(8).AutoFit();
            workSheet4.Column(9).AutoFit();
            workSheet4.Column(10).AutoFit();
            workSheet4.Column(11).AutoFit();
            workSheet4.Column(12).AutoFit();
            workSheet4.Column(13).AutoFit();
            workSheet4.Column(14).AutoFit();
            workSheet4.Column(15).AutoFit();
            workSheet4.Column(16).AutoFit();

            var workSheet5 = excel.Workbook.Worksheets.Add("FINISHING INST.");
            workSheet5.TabColor = System.Drawing.Color.Black;

            workSheet5.DefaultRowHeight = 12;
            workSheet5.Row(1).Height = 20;
            workSheet5.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet5.Row(1).Style.Font.Bold = true;
            workSheet5.Cells[1, 1].Value = "NUMBER OF INSERT";
            workSheet5.Cells[1, 2].Value = "MAGAZINE";
            workSheet5.Cells[1, 3].Value = "BROCHURE";
            workSheet5.Cells[1, 4].Value = "CARRIER SHEET";
            workSheet5.Cells[1, 5].Value = "NEWSLETTER";
            workSheet5.Cells[1, 6].Value = "STATEMENT";
            workSheet5.Cells[1, 7].Value = "BOOKLET";
            workSheet5.Cells[1, 8].Value = "COMMENT";
            workSheet5.Cells[1, 9].Value = "FINISHING FORMAT";
            workSheet5.Cells[1, 10].Value = "FOLDING TYPE";
            workSheet5.Cells[1, 11].Value = "SEALING";
            workSheet5.Cells[1, 12].Value = "TEARING";
            workSheet5.Cells[1, 13].Value = "BARCODE LABEL";
            workSheet5.Cells[1, 14].Value = "CUTTING";
            workSheet5.Cells[1, 15].Value = "STICKING OF";
            workSheet5.Cells[1, 16].Value = "ADD LABEL";
            workSheet5.Cells[1, 17].Value = "STICKER";
            workSheet5.Cells[1, 18].Value = "CHESIRE";
            workSheet5.Cells[1, 19].Value = "BROCHURE";
            workSheet5.Cells[1, 20].Value = "TUCK IN";
            workSheet5.Cells[1, 21].Value = "BURSTING";
            workSheet5.Cells[1, 22].Value = "SEALED";
            workSheet5.Cells[1, 23].Value = "FOLDING";
            workSheet5.Cells[1, 24].Value = "UNSEALED";
            workSheet5.Cells[1, 25].Value = "LETTER";
            workSheet5.Cells[1, 26].Value = "FINISHING INSTRUCTION";


            int recordIndex5 = 2;
            foreach (var CLM in gotlist)
            {
                workSheet5.Cells[recordIndex, 1].Value = CLM.NumberOfInsert;
                workSheet5.Cells[recordIndex, 2].Value = CLM.Magezine1;
                workSheet5.Cells[recordIndex, 3].Value = CLM.Brochure1;
                workSheet5.Cells[recordIndex, 4].Value = CLM.CarrierSheet1;
                workSheet5.Cells[recordIndex, 5].Value = CLM.Newsletter1;
                workSheet5.Cells[recordIndex, 6].Value = CLM.Statement1;
                workSheet5.Cells[recordIndex, 7].Value = CLM.Booklet1;
                workSheet5.Cells[recordIndex, 8].Value = CLM.CommentManualType;
                workSheet5.Cells[recordIndex, 9].Value = CLM.FinishingFormat;
                workSheet5.Cells[recordIndex, 10].Value = CLM.FoldingType;
                workSheet5.Cells[recordIndex, 11].Value = CLM.Sealing1;
                workSheet5.Cells[recordIndex, 12].Value = CLM.Tearing1;
                workSheet5.Cells[recordIndex, 13].Value = CLM.BarcodeLabel1;
                workSheet5.Cells[recordIndex, 14].Value = CLM.Cutting1;
                workSheet5.Cells[recordIndex, 15].Value = CLM.StickingOf1;
                workSheet5.Cells[recordIndex, 16].Value = CLM.AddLabel1;
                workSheet5.Cells[recordIndex, 18].Value = CLM.Sticker1;
                workSheet5.Cells[recordIndex, 19].Value = CLM.Chesire1;
                workSheet5.Cells[recordIndex, 20].Value = CLM.Tuck_In1;
                workSheet5.Cells[recordIndex, 21].Value = CLM.Bursting1;
                workSheet5.Cells[recordIndex, 22].Value = CLM.Sealed1;
                workSheet5.Cells[recordIndex, 23].Value = CLM.Folding1;
                workSheet5.Cells[recordIndex, 24].Value = CLM.Unsealed1;
                workSheet5.Cells[recordIndex, 25].Value = CLM.Letter1;
                workSheet5.Cells[recordIndex, 26].Value = CLM.FinishingInst;

                recordIndex5++;
            }

            workSheet5.Column(1).AutoFit();
            workSheet5.Column(2).AutoFit();
            workSheet5.Column(3).AutoFit();
            workSheet5.Column(4).AutoFit();
            workSheet5.Column(5).AutoFit();
            workSheet5.Column(6).AutoFit();
            workSheet5.Column(7).AutoFit();
            workSheet5.Column(8).AutoFit();
            workSheet5.Column(9).AutoFit();
            workSheet5.Column(10).AutoFit();
            workSheet5.Column(11).AutoFit();
            workSheet5.Column(12).AutoFit();
            workSheet5.Column(13).AutoFit();
            workSheet5.Column(14).AutoFit();
            workSheet5.Column(15).AutoFit();
            workSheet5.Column(16).AutoFit();
            workSheet5.Column(17).AutoFit();
            workSheet5.Column(18).AutoFit();
            workSheet5.Column(19).AutoFit();
            workSheet5.Column(20).AutoFit();
            workSheet5.Column(21).AutoFit();
            workSheet5.Column(22).AutoFit();
            workSheet5.Column(23).AutoFit();
            workSheet5.Column(24).AutoFit();
            workSheet5.Column(25).AutoFit();
            workSheet5.Column(26).AutoFit();


            var workSheet6 = excel.Workbook.Worksheets.Add("NOTES");
            workSheet6.TabColor = System.Drawing.Color.Black;

            workSheet6.DefaultRowHeight = 12;
            workSheet6.Row(1).Height = 20;
            workSheet6.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet6.Row(1).Style.Font.Bold = true;
            workSheet6.Cells[1, 1].Value = "IT & SYSTEM";
            workSheet6.Cells[1, 2].Value = "PROD & PLANNING";
            workSheet6.Cells[1, 3].Value = "PURCHASING";
            workSheet6.Cells[1, 4].Value = "ENGINEERING";
            workSheet6.Cells[1, 5].Value = "ARTWORK";
            workSheet6.Cells[1, 6].Value = "ACC & BILLLING";
            workSheet6.Cells[1, 7].Value = "DCP";
            workSheet6.Cells[1, 8].Value = "POSTING INFO";

            int recordIndex6 = 2;
            foreach (var CLM in gotlist)
            {
                workSheet6.Cells[recordIndex, 1].Value = CLM.IT_SysNotes;
                workSheet6.Cells[recordIndex, 2].Value = CLM.Produc_PlanningNotes;
                workSheet6.Cells[recordIndex, 3].Value = CLM.PurchasingNotes;
                workSheet6.Cells[recordIndex, 4].Value = CLM.EngineeringNotes;
                workSheet6.Cells[recordIndex, 5].Value = CLM.ArtworkNotes;
                workSheet6.Cells[recordIndex, 6].Value = CLM.Acc_BillingNotes;
                workSheet6.Cells[recordIndex, 7].Value = CLM.DCPNotes;
                workSheet6.Cells[recordIndex, 8].Value = CLM.PostingInfo;


                recordIndex6++;
            }

            workSheet6.Column(1).AutoFit();
            workSheet6.Column(2).AutoFit();
            workSheet6.Column(3).AutoFit();
            workSheet6.Column(4).AutoFit();
            workSheet6.Column(5).AutoFit();
            workSheet6.Column(6).AutoFit();
            workSheet6.Column(7).AutoFit();
            workSheet6.Column(8).AutoFit();

            string excelName = "Job Instruction -" + ProductName;
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }

        }


        return RedirectToAction("ManageJIUnderDev", "MBD");
    }


    List<SchedulerHighlight> viewSchedulerHighlightlist = new List<SchedulerHighlight>();
    public ActionResult ManageSchedulerHighlightList(string Id, string Customer_Name, string ProductName, string set)
    {
        ViewBag.Customer_Name = Customer_Name;
        ViewBag.ProductName = ProductName;

        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT Id,BatchVisual,Customer_Name,Cust_Department,Name,CreatedBy, BatchVisual                                         
                                           FROM [IflowSeed].[dbo].[SchedulerHighlight]";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                SchedulerHighlight model = new SchedulerHighlight();
                {
                    model.Bil = _bil++;
                    if (reader.IsDBNull(0) == false)
                    {
                        model.Id = reader.GetGuid(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        model.BatchVisual = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        model.Customer_Name = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        model.Cust_Department = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        model.Name = reader.GetString(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        model.CreatedBy = reader.GetString(5);
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        model.BatchVisual = reader.GetString(6);
                    }
                }
                viewSchedulerHighlightlist.Add(model);
            }
            cn.Close();
        }


        return View(viewSchedulerHighlightlist);

    }

    public ActionResult DeleteCreateSchedulerHighlight(string Id)
    {
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        {
            cn.Open();
            SqlCommand command = new SqlCommand("DELETE FROM [IflowSeed].[dbo].[SchedulerHighlight] WHERE Id=@Id", cn);
            command.Parameters.AddWithValue("@Id", Id);
            var rm =command.ExecuteNonQuery();

            cn.Close();
        }
        return RedirectToAction("ManageSchedulerHighlightList", "MBD");
    }

    public ActionResult CreateBatchScheduler(string Id, string Customer_Name, string Cust_Department, string BatchVisual, string Name, string CreatedBy)
    {

        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];
        Session["Id"] = Id;

        ViewBag.IsDepart = @Session["Department"];
        ViewBag.AccountManager = IdentityName.ToString();

        int _bil = 1;
        List<SelectListItem> li = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Customer_Name FROM [IflowSeed].[dbo].[JobInstruction]                          
                                    ORDER BY Customer_Name";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                SchedulerHighlight model = new SchedulerHighlight();
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


        if (!string.IsNullOrEmpty(Customer_Name))
        {
            int _bil2 = 1;
            List<SelectListItem> li2 = new List<SelectListItem>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT DISTINCT Cust_Department FROM [IflowSeed].[dbo].[CustomerProduct]    
                                        WHERE Customer_Name=@Customer_Name                            
                                        ORDER BY Cust_Department";
                command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    SchedulerHighlight model = new SchedulerHighlight();

                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            model.Cust_Department = reader.GetString(0);
                        }
                    }
                    int i = _bil2++;
                    if (i == 1)
                    {
                        li2.Add(new SelectListItem { Text = "Please Select" });
                    }
                    li2.Add(new SelectListItem { Text = model.Cust_Department });
                }
                cn.Close();
            }
            ViewData["CustDepartment_"] = li2;
        }
        else
        {
            List<SelectListItem> li2 = new List<SelectListItem>();
            li2.Add(new SelectListItem { Text = "Please Select" });
            ViewData["CustDepartment_"] = li2;
        }


        if (string.IsNullOrEmpty(Id) && Customer_Name != "Please Select" && !string.IsNullOrEmpty(Customer_Name) && !string.IsNullOrEmpty(Cust_Department) && !string.IsNullOrEmpty(Name))
        {
            var No_ = new NoCounterModel();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                Guid Idx = Guid.NewGuid();
                string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                string ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                cn.Open();
                SqlCommand command;
                command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[SchedulerHighlight] (Id, CreatedOn, Customer_Name, Cust_Department, Name, CreatedBy,BatchVisual) values (@Id, @CreatedOn, @Customer_Name, @Cust_Department, @Name, @CreatedBy,@BatchVisual)", cn);
                command.Parameters.AddWithValue("@Id", Idx);
                command.Parameters.AddWithValue("@CreatedOn", createdOn);
                command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                command.Parameters.AddWithValue("@Cust_Department", Cust_Department);
                command.Parameters.AddWithValue("@Name", Name);
                command.Parameters.AddWithValue("@BatchVisual", No_.RefNo);
                command.Parameters.AddWithValue("@CreatedBy", IdentityName.ToString());
                command.ExecuteNonQuery();
                cn.Close();
            }
            //bila save akn gi kt managecustomer
            return RedirectToAction("ManageSchedulerHighlightList", "MBD");
        }



        if (!string.IsNullOrEmpty(Id))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Id, Customer_Name, Cust_Department, Name
                                       FROM [IflowSeed].[dbo].[SchedulerHighlight]                              
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
                        ViewBag.Cust_Department = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        ViewBag.Name = reader.GetString(3);
                    }


                }
                cn.Close();
            }
        }



        return View();
    }

    public ActionResult DeleteFileStoreUploaded(string Id,string JobId,string Customer_Name, string BatchNo)
    {
        if (Id != null)
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Picture_FileId
                                      FROM [IflowSeed].[dbo].[SchedulerHighlightDetails]
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
                            command3 = new SqlCommand("DELETE [IflowSeed].[dbo].[SchedulerHighlightDetails] WHERE Id=@Id", cn3);
                            command3.Parameters.AddWithValue("@Id", Id);
                            command3.ExecuteNonQuery();
                            cn3.Close();
                        }
                    }
                }
                cn.Close();
            }
        }

        return RedirectToAction("CreateSchedulerHighlightDetail", "MBD", new { Id = JobId, Customer_Name = Customer_Name, BatchNo = BatchNo });
    }


    public ActionResult DeleteSchedulerHighlight(string Id)
    {
        if (!string.IsNullOrEmpty(Id))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("DELETE [IflowSeed].[dbo].[SchedulerHighlight] WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn.Close();
            }
        }
        return RedirectToAction("ManageSchedulerHighlight", "MBD");
    }

    public ActionResult ReloadSH()
    {
        List<SchedulerHighlightDetails> viewSH = new List<SchedulerHighlightDetails>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT Id,ProductName,VisualName,Type,Channel,Volume,CycleDateOn,CycleEndOn,Remark,Customer_Name
                                      FROM [IflowSeed].[dbo].[SchedulerHighlightDetails]  
                                      WHERE ScheduleHighlightId=@Id                                   
                                      ORDER BY CycleDateOn DESC";
            command.Parameters.AddWithValue("@Id", Session["Id"].ToString());
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                SchedulerHighlightDetails model = new SchedulerHighlightDetails();
                {
                    model.Bil = _bil++;
                    if (reader.IsDBNull(0) == false)
                    {
                        model.Id = reader.GetGuid(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        model.ProductName = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        model.VisualName = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        model.Type = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        model.Channel = reader.GetString(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        model.Volume = reader.GetString(5);
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        model.CycleDateOnTxt = String.Format("{0:dd/MM/yyyy}", reader.GetDateTime(6));
                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        model.CycleEndOnTxt = String.Format("{0:dd/MM/yyyy}", reader.GetDateTime(7));
                    }
                    if (reader.IsDBNull(8) == false)
                    {
                        model.Remark = reader.GetString(8);
                    }


                    if (reader.IsDBNull(9) == false)
                    {
                        model.Customer_Name = reader.GetString(9);
                    }


                }
                viewSH.Add(model);
            }
            cn.Close();
            //return Json(new { data = viewFileStore }, JsonRequestBehavior.AllowGet);
            return Json(viewSH);
        }
    }

    [ValidateInput(false)]
    public ActionResult CreateSH(string Id, SchedulerHighlightDetails get, string ScheduleHighlightId, string Customer_Name, string ProductName, string BatchVisual, string Name, string CreatedBy, string Status,
                                  string VisualName, string Type, string CycleDateOn, string CycleEndOn, string Channel, string Remark, string Volume)
    {
        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];
        ViewBag.IsDepart = @Session["Department"];
        Session["Id"] = Id;
        Session["Customer_Name"] = Customer_Name;
        Session["ScheduleHighlightId"] = Id;
        ViewBag.BatchVisual = BatchVisual;
        ViewBag.Customer_Name = Customer_Name;


        return View();

    }






    public ActionResult CreateSchedulerHighlightDetail(FileStore FileUploadLocation,SchedulerHighlightDetails ModelSample, SchedulerHighlightDetails get, string Id, string ScheduleHighlightId,
                                                 string VisualName, string Type, string Channel, string Volume, string CycleDateOn, string CycleEndOn,
                                                 string Remark, string set, string Customer_Name, string BatchNo, string ProductName,string SHId)
    {
        Session["Id"] = Id;
        ViewBag.Id = Id;
        ViewBag.BatchNo = BatchNo;
        ViewBag.Customer_Name = Customer_Name;

        //var Customer_Name = @Session["Customer_Name"];
        var IdentityName = @Session["Fullname"];
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.IsRole = @Session["Role"];
        string Deptment = @Session["Department"].ToString();

        List<SchedulerHighlightDetails> SHD = new List<SchedulerHighlightDetails>();
        List<CustomerProduct> Product = new List<CustomerProduct>();


        if (!string.IsNullOrEmpty(Id))
        {

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    command.CommandText = @"SELECT VisualName, CONVERT(VARCHAR(10),CAST(CycleDateOn AS DATETIME), 120) AS CycleDateOn ,  CONVERT(VARCHAR(10),CAST(CycleEndOn AS DATETIME), 120) AS CycleEndOn  , Remark, Type, Volume, Channel
                                    FROM SchedulerHighlightDetails WHERE Id=@Id";
                    command.Parameters.AddWithValue("@Id", Id);
                    var reader = command.ExecuteReader();

                    if(reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(0) == false)
                            {
                                ViewBag.VisualName = reader.GetString(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                //string[] cycledate = reader.GetString(1).Split(' ');
                                //ViewBag.CycleDateOn = cycledate[0];
                                //ViewBag.CycleDateOn = String.Format("{0:dd/MM/yyyy}", reader.GetDateTime(1));
                                ViewBag.CycleDateOn = reader["CycleDateOn"].ToString();
                            }
                            if (reader.IsDBNull(2) == false)
                            {
                                //string[] cycleEnd = reader.GetString(2).Split(' ');

                                //ViewBag.CycleEndOn = cycleEnd[0];

                                //ViewBag.CycleEndOn = String.Format("{dd/MM/yyyy}", reader.GetString(2));

                                ViewBag.CycleEndOn = reader["CycleEndOn"].ToString();

                            }
                            if (reader.IsDBNull(3) == false)
                            {
                                ViewBag.Remark = reader.GetString(3);
                            }
                            if (reader.IsDBNull(4) == false)
                            {
                                ViewBag.Type = reader.GetString(4);
                            }
                            if (reader.IsDBNull(5) == false)
                            {
                                ViewBag.Volume = reader.GetString(5);
                            }

                            if (reader.IsDBNull(6) == false)
                            {
                                ViewBag.Channel = reader.GetString(6);
                            }


                        }

                        ViewBag.Updatw = "Update";

                    }
                }




                int _Bil = 1;
                SqlCommand cmd = new SqlCommand(@"SELECT DISTINCT ProductName FROM SchedulerHighlightProduct WHERE BatchNo = @Cust_Name ORDER BY ProductName ASC", cn);
                cmd.Parameters.AddWithValue("@Cust_Name", BatchNo);
                SqlDataReader rm = cmd.ExecuteReader();

                if(rm.HasRows)
                {
                    while(rm.Read())
                    {
                        CustomerProduct model = new CustomerProduct();
                        {
                            model.Bil = _Bil++;
                            if (!rm.IsDBNull(0))
                            {
                                model.ProductName = rm.GetString(0);
                            }

                            //if (!rm.IsDBNull(1))
                            //{
                            //    model.CreateUser = rm["CreatedOn"].ToString();
                            //}
                        }

                        Product.Add(model);
                    }
                }

                List<CustomerProduct> prodList = new List<CustomerProduct>();

                SqlCommand cmd2 = new SqlCommand(@"SELECT ProductName FROM CustomerProduct WHERE Customer_Name = @Cust_Name AND (ProductName LIKE '%Credit Card%' OR ProductName LIKE '%Charge Card%') ORDER BY ProductName ASC", cn);
                cmd2.Parameters.AddWithValue("@Cust_Name", Customer_Name);
                SqlDataReader rm2 = cmd2.ExecuteReader();

                if (rm2.HasRows)
                {
                    while (rm2.Read())
                    {
                        CustomerProduct model = new CustomerProduct();
                        {
                            model.Bil = _Bil++;
                            if (!rm2.IsDBNull(0))
                            {
                                model.ProductName = rm2.GetString(0);
                            }
                        }

                        prodList.Add(model);
                    }
                }

                cn.Close();

                ViewBag.ProductList = prodList;
            }
        }

        if (set == "AddNew")
        {
            var fileName = Path.GetFileName(FileUploadLocation.FileUploadFile.FileName);
            var path = Path.Combine(Server.MapPath("~/FileStore"), fileName);
            FileUploadLocation.FileUploadFile.SaveAs(path);
            //var fileName = Path.GetFileName(ModelSample.FileUploadFile.FileName);
            //var path = Path.Combine(Server.MapPath("~/FileStore"), fileName);
            //ModelSample.FileUploadFile.SaveAs(path);


            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                Guid guidId = Guid.NewGuid();
                string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                //get.CycleDateOn = Convert.ToDateTime(get.CycleDateOnTxt);
                //get.CycleEndOn = Convert.ToDateTime(get.CycleEndOnTxt);
                cn2.Open();
                SqlCommand command;
                command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[SchedulerHighlightDetails] (Id,CreatedOn,VisualName,Type,Channel,Volume,CycleDateOn,CycleEndOn,Remark,ScheduleHighlightId,CreatedBy,Picture_FileId,Picture_Extension,BatchVisual) values (@Id,@CreatedOn,@VisualName,@Type,@Channel,@Volume,@CycleDateOn,@CycleEndOn,@Remark,@ScheduleHighlightId,@CreatedBy,@Picture_FileId,@Picture_Extension,@BatchVisual)", cn2);
                command.Parameters.AddWithValue("@Id", guidId);
                command.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
                command.Parameters.AddWithValue("@VisualName", VisualName);
                command.Parameters.AddWithValue("@Type", Type);
                command.Parameters.AddWithValue("@Channel", Channel);
                command.Parameters.AddWithValue("@Volume", Volume);
                command.Parameters.AddWithValue("@CycleDateOn", CycleDateOn);
                command.Parameters.AddWithValue("@CycleEndOn", CycleEndOn);
                command.Parameters.AddWithValue("@Remark", Remark);
                command.Parameters.AddWithValue("@ScheduleHighlightId", Id);
                command.Parameters.AddWithValue("@CreatedBy", IdentityName.ToString());
                command.Parameters.AddWithValue("@Picture_FileId", fileName.Trim());
                command.Parameters.AddWithValue("@Picture_Extension", FileUploadLocation.FileUploadFile.ContentType);
                command.Parameters.AddWithValue("@BatchVisual", BatchNo);


                command.ExecuteNonQuery();
                cn2.Close();

            }
            return RedirectToAction("CreateSchedulerHighlightDetail", "MBD", new { Id = Id, Customer_Name = Customer_Name, BatchNo = BatchNo });

            //return RedirectToAction("ManageSchedulerHighlightList", "MBD");
        }

        if(set=="Update")
        {
            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn2.Open();

                SqlCommand cmd = new SqlCommand(@"UPDATE SchedulerHighlightDetails SET ModifiedOn=@ModifiedOn, VisualName = @VisualName, CycleDateOn = @CycleDateOn, CycleEndOn = @CycleEndOn, Remark = @Remark, Type=@Type, Volume=@Volume, Channel = @Channel WHERE Id=@Id", cn2);
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.Parameters.AddWithValue("@ModifiedOn", DateTime.Now);
                cmd.Parameters.AddWithValue("@VisualName", VisualName);
                cmd.Parameters.AddWithValue("@Type", Type);
                cmd.Parameters.AddWithValue("@Channel", Channel);
                cmd.Parameters.AddWithValue("@Volume", Volume);
                cmd.Parameters.AddWithValue("@CycleDateOn", CycleDateOn);
                cmd.Parameters.AddWithValue("@CycleEndOn", CycleEndOn);
                cmd.Parameters.AddWithValue("@Remark", Remark);

                cmd.ExecuteNonQuery();

                cn2.Close();

                return RedirectToAction("CreateSchedulerHighlightDetail", "MBD", new { Id = Id, Customer_Name = Customer_Name, BatchNo = BatchNo });


            }


        }
        
        if(set== "AddProduct")
        {
            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn2.Open();
                SqlCommand Inscmd = new SqlCommand("INSERT INTO SchedulerHighlightProduct(Id,CreatedOn,ProductName,BatchNo) VALUES(@Id,@CreatedOn,@ProductName,@BatchNo)", cn2);
                Inscmd.Parameters.AddWithValue("@Id", Guid.NewGuid());
                Inscmd.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
                Inscmd.Parameters.AddWithValue("@ProductName", ProductName);
                Inscmd.Parameters.AddWithValue("@BatchNo", BatchNo);
                Inscmd.ExecuteNonQuery();
                cn2.Close();
            }
            return RedirectToAction("CreateSchedulerHighlightDetail", "MBD", new {Id=Id, Customer_Name = Customer_Name, BatchNo = BatchNo });
        }

        if (set == "DeleteProduct")
        {
            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn2.Open();
                SqlCommand Deletecmd = new SqlCommand("DELETE from SchedulerHighlightProduct WHERE ProductName = @ProductName", cn2);
                Deletecmd.Parameters.AddWithValue("@ProductName", ProductName);
                Deletecmd.ExecuteNonQuery();
                cn2.Close();
            }
            return RedirectToAction("CreateSchedulerHighlightDetail", "MBD", new { Id = Id, Customer_Name = Customer_Name, BatchNo = BatchNo });
        }
        return View(Product);
    }
    public ActionResult DeleteSH(string Id, string ScheduleHighlightId)
    {
        Guid ScheduleId = Guid.Empty;
        if (Id != null)
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Picture_FileId,ScheduleHighlightId
                                          FROM [IflowSeed].[dbo].[SchedulerHighlightDetails]
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
                            command3 = new SqlCommand("DELETE [IflowSeed].[dbo].[SchedulerHighlightDetails] WHERE Id=@Id", cn3);
                            command3.Parameters.AddWithValue("@Id", Id);
                            command3.ExecuteNonQuery();
                            cn3.Close();

                        }

                    }

                    if (reader.IsDBNull(1) == false)
                    {
                        ScheduleId = reader.GetGuid(1);
                        return RedirectToAction("CreateSH", "MBD", new { Id = Session["Id"].ToString() });
                    }
                }
                cn.Close();
            }
        }

        return RedirectToAction("CreateSH", "MBD", new { Id = Session["Id"].ToString() });
    }



    public ActionResult FileStoreSCH(string BatchNo)
    {
        var Id = Session["Id"];
        ViewBag.Id = Id;

        List<SchedulerHighlightDetails> Sch_Model = new List<SchedulerHighlightDetails>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT SchedulerHighlightDetails.Volume, SchedulerHighlightDetails.Channel
                                   FROM  SchedulerHighlight INNER JOIN
                                   SchedulerHighlightDetails ON SchedulerHighlight.Id = SchedulerHighlightDetails.ScheduleHighlightId ";
            command.Parameters.AddWithValue("@Id", Id);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                SchedulerHighlightDetails model = new SchedulerHighlightDetails();
                {
                    model.Bil = _bil++;
                    if (reader.IsDBNull(0) == false)
                    {
                        ViewBag.Volume = reader.GetString(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        ViewBag.Channel = reader.GetString(1);
                    }


                }
                Sch_Model.Add(model);
            }

        }



        List<SchedulerHighlightDetails> viewFileStore = new List<SchedulerHighlightDetails>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))

        {

            int _bil = 1;
            cn.Open();
            //command.CommandText = @"SELECT   [Picture_FileId],[Id]
            //                         FROM [IflowSeed].[dbo].[FileStore]
            //                         WHERE ScheduleHighlightId=@Id ";
            //command.Parameters.AddWithValue("@Id", Id);

            //var reader = command.ExecuteReader();
            //while (reader.Read())
            //{
            //    FileStore model = new FileStore();
            //    {
            //        model.Bil = _bil++;
            //        if (reader.IsDBNull(0) == false)
            //        {
            //            model.Picture_FileId = reader.GetString(0);
            //        }
            //        if (reader.IsDBNull(1) == false)
            //        {
            //            model.Id = reader.GetGuid(1);
            //        }

            //    }
            //    viewFileStore.Add(model);
            //}

            command.CommandText = @"SELECT BatchVisual, VisualName, Channel, [Picture_FileId],[Id], CONVERT(VARCHAR(10),CAST(CycleDateOn AS DATETIME), 120) AS CycleDateOn , CONVERT(VARCHAR(10),CAST(CycleEndOn AS DATETIME), 120) AS CycleEndOn ,Remark,Volume
                                     FROM [IflowSeed].[dbo].[SchedulerHighlightDetails]
                                     WHERE BatchVisual=@Id AND CAST(CycleEndOn AS date) >= CAST(@currentDate AS date)";
            command.Parameters.AddWithValue("@Id", BatchNo);
            command.Parameters.AddWithValue("@currentDate", DateTime.Now.ToString("yyyy-MM-dd"));

            Debug.WriteLine("Current Date : "+ DateTime.Now.ToString("yyyy-MM-dd"));


            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                SchedulerHighlightDetails model = new SchedulerHighlightDetails();
                {
                    model.Bil = _bil++;
                    if (reader.IsDBNull(0) == false)
                    {
                        model.BatchVisual = reader.GetString(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        model.VisualName = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        model.Channel = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        model.Customer_Name= reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        model.Id = reader.GetGuid(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        model.CycleDateOn = reader["CycleDateOn"].ToString();
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        model.CycleEndOn = reader["CycleEndOn"].ToString();
                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        model.Remark = reader.GetString(7);
                    }
                    if (reader.IsDBNull(8) == false)
                    {
                        model.Volume = reader.GetString(8);
                    }

                }
                viewFileStore.Add(model);
            }
            cn.Close();
            //return Json(new { data = viewFileStore }, JsonRequestBehavior.AllowGet);
            return Json(viewFileStore);
        }
    }


    public ActionResult UploadFileStore(FileStore FileUploadLocation, string Customer_Name,string BatchNo,string JobId)
    {
        var Id = Session["Id"];
        var Status = Session["Status"];
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.IsRole = @Session["Role"];
        string Deptment = @Session["Department"].ToString();
        ViewBag.Customer_Name = Customer_Name;
        ViewBag.BatchNo=BatchNo;
        ViewBag.Id = JobId;

        Debug.WriteLine("Job ID : " + JobId);

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
                command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[FileStore] (Id,CreatedOn,Picture_FileId,ScheduleHighlightId,UrgencySts,Picture_Extension,Department) values (@Id,@CreatedOn,@Picture_FileId,@ScheduleHighlightId,@UrgencySts,@Picture_Extension,@Department)", cn2);
                command.Parameters.AddWithValue("@Id", guidId);
                command.Parameters.AddWithValue("@CreatedOn", createdOn);
                command.Parameters.AddWithValue("@Picture_FileId", fileName.Trim());
                command.Parameters.AddWithValue("@ScheduleHighlightId", Id);
                command.Parameters.AddWithValue("@UrgencySts", 0);
                command.Parameters.AddWithValue("@Picture_Extension", FileUploadLocation.FileUploadFile.ContentType);
                command.Parameters.AddWithValue("@Department", Deptment);
                command.ExecuteNonQuery();
                cn2.Close();

                return RedirectToAction("CreateSchedulerHighlightDetail", "MBD", new { Id = JobId, Customer_Name = Customer_Name, BatchNo = BatchNo });
            }
        }

        if (FileUploadLocation.set == "back")
        {
            return RedirectToAction("CreateSchedulerHighlightDetail", "MBD", new { Id = JobId, Customer_Name = Customer_Name, BatchNo = BatchNo });
        }

        return View();
    }

    public ActionResult DownloadFileStore0(string Id)
    {
        Guid IdPartner = Guid.Empty;


        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT Picture_FileId,Picture_Extension,ScheduleHighlightId
                                      FROM [IflowSeed].[dbo].[FileStore]
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

        return RedirectToAction("ManageSchedulerHighlight", "MBD", new { Id = IdPartner });
    }

    public ActionResult DownloadFileStore1(string Id, string SHId, string Customer_Name, string BatchNo)
    {
        Guid IdPartner = Guid.Empty;


        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT Picture_FileId,Picture_Extension
                                      FROM [IflowSeed].[dbo].[SchedulerHighlightDetails]
                                      WHERE Id=@Id";
            command.Parameters.AddWithValue("@Id", Id.ToString());
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader.IsDBNull(0) == false)
                {
                    var Picture_FileId = reader.GetString(0);
                    var Picture_Extension = reader.GetString(1);
                    //IdPartner = reader.GetGuid(2);
                    var path = PathSource + Picture_FileId;
                    string contentType = Picture_Extension.ToString();
                    return File(path, contentType, Picture_FileId);
                }
            }
        }

        //return RedirectToAction("ManageSchedulerHighlight", "MBD", new { Id = IdPartner });
        //return RedirectToAction("CreateSchedulerHighlightDetail", "MBD", new { Id = IdPartner });
        return RedirectToAction("CreateSchedulerHighlightDetail", "MBD", new { Id = SHId, Customer_Name = Customer_Name, BatchNo = BatchNo });
    }




    public ActionResult DownloadVisual(string Id)
    {
        Guid ScheduleId = Guid.Empty;

        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT Picture_FileId,Picture_Extension,Id
                                      FROM [IflowSeed].[dbo].[SchedulerHighlightDetails]
                                      WHERE Id=@Id";
            command.Parameters.AddWithValue("@Id", Id.ToString());
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader.IsDBNull(0) == false)
                {
                    var Picture_FileId = reader.GetString(0);
                    var Picture_Extension = reader.GetString(1);
                    ScheduleId = reader.GetGuid(2);
                    var path = PathSource + Picture_FileId;
                    string contentType = Picture_Extension.ToString();
                    return File(path, contentType, Picture_FileId);
                }
            }
        }

        return RedirectToAction("CreateSH", "MBD", new { Id = Session["Id"].ToString() });
    }


    public ActionResult getExcelForm(SchedulerHighlightDetails get, string line, string Id, string Customer_Name, string ProductName, string CycleDateOn, string CycleEndOn)
    {
        //string[] parts = line.Split('|');
        //string Id = parts[0].Trim();
        //string ProcessType = parts[1].Trim();

        ViewBag.IsDepart = @Session["Department"];
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        {

            List<SchedulerHighlightDetails> gotlist = new List<SchedulerHighlightDetails>();
            cn.Open();
            SqlCommand command;
            command = new SqlCommand(@"SELECT b.Id,a.Customer_Name,/* ProductName,*/ b.VisualName,b.Type,b.Channel,b.Volume,b.CycleDateOn,b.CycleEndOn,b.Remark
                                       FROM [IflowSeed].[dbo].[SchedulerHighlight] a, [IflowSeed].[dbo].[SchedulerHighlightDetails] b
                                       WHERE a.Id=b.ScheduleHighlightId AND b.Id=@Id", cn);
            command.Parameters.AddWithValue("@Id", Id);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                SchedulerHighlightDetails list = new SchedulerHighlightDetails();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        list.Id = reader.GetGuid(0);

                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        list.Customer_Name = reader.GetString(1);

                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        list.VisualName = reader.GetString(2);

                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        list.Type = reader.GetString(3);

                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        list.Channel = reader.GetString(4);

                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        list.Volume = reader.GetString(5);

                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        list.CycleDateOnTxt = String.Format("{0:dd/MM/yyyy}", reader.GetDateTime(6));

                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        list.CycleEndOnTxt = String.Format("{0:dd/MM/yyyy}", reader.GetDateTime(7));

                    }
                    if (reader.IsDBNull(8) == false)
                    {
                        list.Remark = reader.GetString(8);

                    }


                }
                gotlist.Add(list);

            }
            cn.Close();
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;

            workSheet.DefaultRowHeight = 12;
            workSheet.Row(1).Height = 20;
            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(1).Style.Font.Bold = true;
            workSheet.Cells[1, 1].Value = "NO.";
            workSheet.Cells[1, 2].Value = "CUSTOMER";
            workSheet.Cells[1, 3].Value = "VISUAL NAME";
            workSheet.Cells[1, 4].Value = "TYPE";
            workSheet.Cells[1, 5].Value = "CHANNEL";
            workSheet.Cells[1, 6].Value = "VOLUME";
            workSheet.Cells[1, 7].Value = "CYCLE DATE";
            workSheet.Cells[1, 8].Value = "CYCLE END";
            workSheet.Cells[1, 9].Value = "REMARK";




            int recordIndex = 2;
            foreach (var CLM in gotlist)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
                workSheet.Cells[recordIndex, 2].Value = CLM.Customer_Name;
                workSheet.Cells[recordIndex, 3].Value = CLM.VisualName;
                workSheet.Cells[recordIndex, 4].Value = CLM.Type;
                workSheet.Cells[recordIndex, 5].Value = CLM.Channel;
                workSheet.Cells[recordIndex, 6].Value = CLM.Volume;
                workSheet.Cells[recordIndex, 7].Value = CLM.CycleDateOnTxt;
                workSheet.Cells[recordIndex, 8].Value = CLM.CycleEndOnTxt;
                workSheet.Cells[recordIndex, 9].Value = CLM.Remark;



                recordIndex++;
            }
            workSheet.Column(1).AutoFit();
            workSheet.Column(2).AutoFit();
            workSheet.Column(3).AutoFit();
            workSheet.Column(5).AutoFit();
            workSheet.Column(6).AutoFit();
            workSheet.Column(7).AutoFit();
            workSheet.Column(8).AutoFit();
            workSheet.Column(9).AutoFit();
            workSheet.Column(10).AutoFit();


            string excelName = "VISUAL HIGHLIGHT-" + Customer_Name + ":" + CycleDateOn + "-" + CycleEndOn;
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }

        }


        return RedirectToAction("CreateSH", "MBD", new { Id = Session["Id"].ToString() });
    }

    public ActionResult PrintJI (string JobSheetNo)
    {
        return View();
    }

    public ActionResult UploadFileStoreMBD(FileStoreUploaded FileUploadLocation, string Category,string JobSheetNo)
    {
        Debug.WriteLine("MASUK CONTROLLER");
        string CustomerName = "";
        var Id = Session["Id"];
        var CreateBy = Session["Fullname"];
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
                command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[SampleProduct] (Id,CreatedOn,Picture_FileId,Picture_Extension,Code,JobInstruction,CreateBy,JobSheetNo) values (@Id,@CreatedOn,@Picture_FileId,@Picture_Extension,@Code,@JobInstruction,@CreateBy,@JobSheetNo)", cn2);
                command.Parameters.AddWithValue("@Id", guidId);
                command.Parameters.AddWithValue("@CreatedOn", createdOn);
                command.Parameters.AddWithValue("@Picture_FileId", fileName.Trim());
                command.Parameters.AddWithValue("@Picture_Extension", FileUploadLocation.FileUploadFile.ContentType);
                command.Parameters.AddWithValue("@Code", "JI");
                command.Parameters.AddWithValue("@JobInstruction", Id);
                command.Parameters.AddWithValue("@CreateBy", CreateBy);
                command.Parameters.AddWithValue("@JobSheetNo", JobSheetNo);


                command.ExecuteNonQuery();

                Debug.WriteLine("Lepas Insert");


                SqlCommand cmd1 = new SqlCommand("SELECT Customer_Name,JobSheetNo FROM JobInstruction WHERE Id=@Id", cn2);
                cmd1.Parameters.AddWithValue("@Id",Id);
                SqlDataReader rm1 = cmd1.ExecuteReader();

                if(rm1.HasRows)
                {
                    while(rm1.Read())
                    {
                        CustomerName = rm1.GetString(0);
                        JobSheetNo = rm1.GetString(1);
                    }
                }
                cn2.Close();

                return RedirectToAction("CreateNewJI", "MBD", new { Id = Id,JobSheetNo=JobSheetNo,Customer_Name=CustomerName });
            }
        }
        if (FileUploadLocation.set == "back")
        {
            return RedirectToAction("CreateNewJI", "MBD", new { Id = Id, JobSheetNo = JobSheetNo, Customer_Name = CustomerName });
        }

        return View();
    }

    public ActionResult ReloadFileStoreMBD(string JobSheetNo)
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
            command.CommandText = @"SELECT Picture_FileId,Id,Code,JobInstruction
                                      FROM [IflowSeed].[dbo].[SampleProduct]
                                      WHERE JobSheetNo=@Id And Code='JI'
                                     ORDER BY CreatedOn DESC";
            command.Parameters.AddWithValue("@Id", JobSheetNo);
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
                    if (reader.IsDBNull(3) == false)
                    {
                        model.set = reader["JobInstruction"].ToString();
                    }
                }
                viewFileStore.Add(model);
            }
            cn.Close();
            //return Json(new { data = viewFileStore }, JsonRequestBehavior.AllowGet);
            return Json(viewFileStore);
        }
    }

    public ActionResult DownloadFileMBD(string Id, string CustomerName, string JobSheetNo)
    {
        Guid IdPartner = Guid.Empty;


        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        {
            cn.Open();

            using (SqlCommand command = new SqlCommand("", cn))
            {
                command.CommandText = @"SELECT Picture_FileId,Picture_Extension,JobInstruction
                                      FROM [IflowSeed].[dbo].[SampleProduct]
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

            SqlCommand cmd1 = new SqlCommand("SELECT Customer_Name,JobSheetNo FROM JobInstruction WHERE Id=@Id", cn);
            cmd1.Parameters.AddWithValue("@Id", Id);
            SqlDataReader rm1 = cmd1.ExecuteReader();

            if (rm1.HasRows)
            {
                while (rm1.Read())
                {
                    CustomerName = rm1.GetString(0);
                    JobSheetNo = rm1.GetString(1);
                }
            }

            cn.Close();
        }


        return RedirectToAction("CreateNewJI", "MBD", new { Id = Id, JobSheetNo = JobSheetNo, Customer_Name = CustomerName });
    }

    public ActionResult RedirectTo(string Id, string Customer_Name, string BatchNo)
    {
        return RedirectToAction("CreateSchedulerHighlightDetail", "MBD",new { Id = Id, Customer_Name = Customer_Name, BatchNo = BatchNo });
    }

    public ActionResult DeleteFileStore(string Id,string JI, string CustomerName, string JobSheetNo)
    {
        Guid QMId = Guid.Empty;

        if (Id != null)
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand cmd1 = new SqlCommand("SELECT Customer_Name,JobSheetNo FROM JobInstruction WHERE Id=@Id", cn);
                cmd1.Parameters.AddWithValue("@Id", JI);
                SqlDataReader rm1 = cmd1.ExecuteReader();

                if (rm1.HasRows)
                {
                    while (rm1.Read())
                    {
                        CustomerName = rm1.GetString(0);
                        JobSheetNo = rm1.GetString(1);
                    }
                }

                using (SqlCommand command = new SqlCommand("", cn))
                {
                    command.CommandText = @"SELECT Picture_FileId,JobInstruction
                                      FROM [IflowSeed].[dbo].[SampleProduct]
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
                                command3 = new SqlCommand("DELETE [IflowSeed].[dbo].[SampleProduct]  WHERE Id=@Id", cn3);
                                command3.Parameters.AddWithValue("@Id", Id);
                                command3.ExecuteNonQuery();
                                cn3.Close();
                            }
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            QMId = reader.GetGuid(1);
                            Session["Id"] = QMId;

                            return RedirectToAction("CreateNewJI", "MBD", new { Id = JI, JobSheetNo = JobSheetNo, Customer_Name = CustomerName });
                        }
                    }


                }
                cn.Close();


            }
        }

        return RedirectToAction("CreateNewJI", "MBD", new { Id = JI, JobSheetNo = JobSheetNo, Customer_Name = CustomerName });
    }




}





internal class TabModel
{
    internal string TargetControlId;
}