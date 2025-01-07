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

namespace MvcAppV2.Controllers
{
    public class PrintJIController : Controller
    {
        //
        // GET: /PrintJI/

        public ActionResult Index(FormCollection formCollection)
        {
            string JobSheetNo = formCollection["JobSheetNo"];
            List<string> InsertingInstruction = new List<string>();
            List<string> ManualType = new List<string>();
            List<string> AddressLabelling = new List<string>();
            List<string> FinishingType = new List<string>();

            Debug.WriteLine("Job Sheet No :" + JobSheetNo);//for display walaupun benda tu takde
            //JobSheetNo = "BATCH/2024/0000276";
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Customer_Name, ProductName, ServiceLevel, JobClass, CONVERT(VARCHAR, CAST(JobRequest AS DATE), 105) as JobRequest,QuotationRef, ContractRef, JobType, NewMR, " +
                    "CONVERT(VARCHAR, CAST(ExpectedDateCompletionToGPO AS DATE), 105) as ExpectedDateCompletionToGPO , Contact_Person, DeliveryChannel, AccountsQty, ImpressionQty, PagesQty, " +
                    "CONVERT(VARCHAR, CAST(CycleTerm AS DATE), 105) as CycleTerm, CONVERT(VARCHAR, CAST(MailingDate AS DATE), 105) as MailingDate, JoiningFiles, TotalRecord, InputFileName, OutputFileName, " +
                    "Sorting, SortingMode, Other, DataPrintingRemark, ArtworkStatus, PaperStock, Paper, Grammage, PaperType, PaperSize, MaterialColour, EnvelopeStock, EnvelopeSize, EnvelopeColour, EnvWindowOpaque, " +
                    "EnvelopeType, EnvelopeGrammage, EnvelopeWindow, LabelStock, LabelCutSheet, PlasticStock, PlasticSize, PlasticType, PlasticThickness, OthersStock, BalancedMaterial, PrintingType, GpoList, RegisterMail, " +
                    "AdditionalPrintingMark, PrintingInstr, SortingInstr, PrintingOrientation, OtherList, SortingCriteria, Letter, Brochures_Leaflets, ReplyEnvelope, ImgOnStatement, Booklet, NumberOfInsert, Magezine, Brochure, " +
                    "CarrierSheet, Newsletter, Statement, Booklet1, CommentManualType, FinishingFormat, FoldingType, Sealing, Tearing, BarcodeLabel, Cutting, StickingOf, AddLabel, Sticker, Chesire, Tuck_In, Bursting, Sealed, " +
                    "Folding, Unsealed, Letter1, FinishingInst, IT_SysNotes, Produc_PlanningNotes, PurchasingNotes, ArtworkNotes, EngineeringNotes, Acc_BillingNotes, DCPNotes, PostingInfo FROM JobInstruction WHERE JobSheetNo=@JobSheetNo ", cn);

                cmd.Parameters.AddWithValue("@JobSheetNo", JobSheetNo);

                SqlDataReader rn = cmd.ExecuteReader();

                while (rn.Read()) // error handling
                {

                    string Customer_Name = rn["Customer_Name"].ToString();
                    Debug.WriteLine("Customer Name string :" + Customer_Name);
                    ViewBag.Customer_Name = Customer_Name;

                    string ProductName = rn["ProductName"].ToString();
                    Debug.WriteLine("Product Name string :" + ProductName);
                    ViewBag.ProductName = ProductName;

                    ViewBag.JobSheetNo = JobSheetNo;

                    //PROFILE JI

                    string ServiceLevel = rn["ServiceLevel"].ToString();

                    //int ServiceLevelInt = Int32.Parse(ServiceLevel); [untuk convert string ke int]

                    Debug.WriteLine("Service Level string :" + ServiceLevel);
                    //Debug.WriteLine("Service Level int :" + ServiceLevelInt);

                    ViewBag.ServiceLevel = ServiceLevel; //ada dalam cache (boleh guna dekat view dan controller)
                    //TempData["ServiceLevel"] = ServiceLevel; (tak ada dalam cache

                    string JobClass = rn["JobClass"].ToString();
                    Debug.WriteLine("Job Class string :" + JobClass);
                    ViewBag.JobClass = JobClass;

                    string JobRequest = rn["JobRequest"].ToString();
                    Debug.WriteLine("Job Request Date string :" + JobRequest);
                    ViewBag.JobRequest = JobRequest;

                    string QuotationRef = rn["QuotationRef"].ToString();
                    Debug.WriteLine("Quotation Ref string :" + QuotationRef);
                    ViewBag.QuotationRef = QuotationRef;

                    string ContractRef = rn["ContractRef"].ToString();
                    Debug.WriteLine("Contract Ref string :" + ContractRef);
                    ViewBag.ContractRef = ContractRef;

                    string JobType = rn["JobType"].ToString();
                    Debug.WriteLine("Job Type string :" + JobType);
                    ViewBag.JobType = JobType;

                    string NewMR = rn["NewMR"].ToString();
                    Debug.WriteLine("New Model Report string :" + NewMR);
                    ViewBag.NewMR = NewMR;

                    string ExpectedDateCompletionToGPO = rn["ExpectedDateCompletionToGPO"].ToString();
                    Debug.WriteLine("Expected Date Completion To GPO string :" + ExpectedDateCompletionToGPO);
                    ViewBag.ExpectedDateCompletionToGPO = ExpectedDateCompletionToGPO;

                    string Contact_Person = rn["Contact_Person"].ToString();
                    Debug.WriteLine("Contact Person string :" + Contact_Person);
                    ViewBag.Contact_Person = Contact_Person;

                    string DeliveryChannel = rn["DeliveryChannel"].ToString();
                    Debug.WriteLine("Delivery Channel string :" + DeliveryChannel);
                    ViewBag.DeliveryChannel = DeliveryChannel;

                    //ORDER QUANTITY

                    string AccountsQty = rn["AccountsQty"].ToString();
                    Debug.WriteLine("Account Qty string :" + AccountsQty);
                    ViewBag.AccountsQty = AccountsQty;

                    string ImpressionQty = rn["ImpressionQty"].ToString();
                    Debug.WriteLine(" Impression Qty string :" + ImpressionQty);
                    ViewBag.ImpressionQty = ImpressionQty;

                    string PagesQty = rn["PagesQty"].ToString();
                    Debug.WriteLine("Pages Qty string :" + PagesQty);
                    ViewBag.PagesQty = PagesQty;

                    string CycleTerm = rn["CycleTerm"].ToString();
                    Debug.WriteLine("Cycle Term string :" + CycleTerm);
                    ViewBag.CycleTerm = CycleTerm;

                    string MailingDate = rn["MailingDate"].ToString();
                    Debug.WriteLine("Mailing Date string :" + MailingDate);
                    ViewBag.MailingDate = MailingDate;

                    //DATA PROCESS

                    string JoiningFiles = rn["JoiningFiles"].ToString();
                    Debug.WriteLine("Joining Files string :" + JoiningFiles);
                    ViewBag.JoiningFiles = JoiningFiles;

                    string TotalRecord = rn["TotalRecord"].ToString();
                    Debug.WriteLine("Total Record string :" + TotalRecord);
                    ViewBag.TotalRecord = TotalRecord;

                    string InputFileName = rn["InputFileName"].ToString();
                    Debug.WriteLine("Input File Name string :" + InputFileName);
                    ViewBag.InputFileName = InputFileName;

                    string OutputFileName = rn["OutputFileName"].ToString();
                    Debug.WriteLine("Output File Name string :" + OutputFileName);
                    ViewBag.OutputFileName = OutputFileName;

                    string Sorting = rn["Sorting"].ToString();
                    Debug.WriteLine("Sorting string :" + Sorting);
                    ViewBag.Sorting = Sorting;

                    string SortingMode = rn["SortingMode"].ToString();
                    Debug.WriteLine("Sorting Mode string :" + SortingMode);
                    ViewBag.SortingMode = SortingMode;

                    string Other = rn["Other"].ToString();
                    Debug.WriteLine("Other string :" + Other);
                    ViewBag.Other = Other;

                    string DataPrintingRemark = rn["DataPrintingRemark"].ToString();
                    Debug.WriteLine("Data Printing Remark string :" + DataPrintingRemark);
                    ViewBag.DataPrintingRemark = DataPrintingRemark;

                    //MATERIAL INFO

                    string ArtworkStatus = rn["ArtworkStatus"].ToString();
                    Debug.WriteLine("Artwork Status string :" + ArtworkStatus);
                    ViewBag.DataPrintingRemark = ArtworkStatus;

                    //Paper Information

                    string PaperStock = rn["PaperStock"].ToString();
                    Debug.WriteLine("Paper Stock string :" + PaperStock);
                    ViewBag.PaperStock = PaperStock;

                    string Paper = rn["Paper"].ToString();
                    Debug.WriteLine("Paper Name string :" + Paper);
                    ViewBag.Paper = Paper;

                    string Grammage = rn["Grammage"].ToString();
                    Debug.WriteLine("Grammage string :" + Grammage);
                    ViewBag.Grammage = Grammage;

                    string PaperType = rn["PaperType"].ToString();
                    Debug.WriteLine("Paper Type string :" + PaperType);
                    ViewBag.PaperType = PaperType;

                    string PaperSize = rn["PaperSize"].ToString();
                    Debug.WriteLine("Paper Size string :" + PaperSize);
                    ViewBag.PaperSize = PaperSize;

                    string MaterialColour = rn["MaterialColour"].ToString();
                    Debug.WriteLine("Material Colour string :" + MaterialColour);
                    ViewBag.MaterialColour = MaterialColour;

                    //Envelope Information

                    string EnvelopeStock = rn["EnvelopeStock"].ToString();
                    Debug.WriteLine("Envelope Stock string :" + EnvelopeStock);
                    ViewBag.EnvelopeStock = EnvelopeStock;

                    string EnvelopeSize = rn["EnvelopeSize"].ToString();
                    Debug.WriteLine("Envelope Size string :" + EnvelopeSize);
                    ViewBag.EnvelopeSize = EnvelopeSize;

                    string EnvelopeColour = rn["EnvelopeColour"].ToString();
                    Debug.WriteLine("Envelope Colour string :" + EnvelopeColour);
                    ViewBag.EnvelopeColour = EnvelopeColour;

                    string EnvWindowOpaque = rn["EnvWindowOpaque"].ToString();
                    Debug.WriteLine("Others Opaque string :" + EnvWindowOpaque);
                    ViewBag.EnvWindowOpaque = EnvWindowOpaque;

                    string EnvelopeType = rn["EnvelopeType"].ToString();
                    Debug.WriteLine("Envelope Type string :" + EnvelopeType);
                    ViewBag.EnvelopeType = EnvelopeType;

                    string EnvelopeGrammage = rn["EnvelopeGrammage"].ToString();
                    Debug.WriteLine("Envelope Grammage string :" + EnvelopeGrammage);
                    ViewBag.EnvelopeGrammage = EnvelopeGrammage;

                    string EnvelopeWindow = rn["EnvelopeWindow"].ToString();
                    Debug.WriteLine("Envelope Window string :" + EnvelopeWindow);
                    ViewBag.EnvelopeWindow = EnvelopeWindow;

                    //Label Information
                    string LabelStock = rn["LabelStock"].ToString();
                    Debug.WriteLine("Label Stock string :" + LabelStock);
                    ViewBag.LabelStock = LabelStock;

                    string LabelCutSheet = rn["LabelCutSheet"].ToString();
                    Debug.WriteLine("Label Cut Sheet string :" + LabelCutSheet);
                    ViewBag.LabelCutSheet = LabelCutSheet;

                    string PlasticStock = rn["PlasticStock"].ToString();
                    Debug.WriteLine("Plastic Stock string :" + PlasticStock);
                    ViewBag.PlasticStock = PlasticStock;

                    string PlasticSize = rn["PlasticSize"].ToString();
                    Debug.WriteLine("Plastic Size string :" + PlasticSize);
                    ViewBag.PlasticSize = PlasticSize;

                    string PlasticType = rn["PlasticType"].ToString();
                    Debug.WriteLine("Plastic Type string :" + PlasticType);
                    ViewBag.PlasticType = PlasticType;

                    string PlasticThickness = rn["PlasticThickness"].ToString();
                    Debug.WriteLine("Plastic Thickness string :" + PlasticThickness);
                    ViewBag.PlasticThickness = PlasticThickness;

                    //Other Information
                    string OthersStock = rn["OthersStock"].ToString();
                    Debug.WriteLine("Others Stock string :" + OthersStock);
                    ViewBag.OthersStock = OthersStock;

                    string BalancedMaterial = rn["BalancedMaterial"].ToString();
                    Debug.WriteLine("Balanced Material string :" + BalancedMaterial);
                    ViewBag.BalancedMaterial = BalancedMaterial;

                    //PRODUCTION LIST

                    //Priniting Instruction

                    string PrintingType = rn["PrintingType"].ToString();
                    Debug.WriteLine("Printing Type string :" + PrintingType);
                    ViewBag.PrintingType = PrintingType;

                    string GpoList = rn["GpoList"].ToString();
                    Debug.WriteLine("Gpo List string :" + GpoList);
                    ViewBag.GpoList = GpoList;

                    string RegisterMail = rn["RegisterMail"].ToString();
                    Debug.WriteLine("Register Mail string :" + RegisterMail);
                    ViewBag.RegisterMail = RegisterMail;

                    string AdditionalPrintingMark = rn["AdditionalPrintingMark"].ToString();
                    Debug.WriteLine("Additional Printing Mark string :" + AdditionalPrintingMark);
                    ViewBag.AdditionalPrintingMark = AdditionalPrintingMark;

                    string PrintingInstr = rn["PrintingInstr"].ToString();
                    Debug.WriteLine("Printing Instruction string :" + PrintingInstr);
                    ViewBag.PrintingInstr = PrintingInstr;

                    string SortingInstr = rn["SortingInstr"].ToString();
                    Debug.WriteLine("Sorting Instruction string :" + SortingInstr);
                    ViewBag.SortingInstr = SortingInstr;

                    string PrintingOrientation = rn["PrintingOrientation"].ToString();
                    Debug.WriteLine("Printing Orientation string :" + PrintingOrientation);
                    ViewBag.PrintingOrientation = PrintingOrientation;

                    string OtherList = rn["OtherList"].ToString();
                    Debug.WriteLine("Other List string :" + OtherList);
                    ViewBag.OtherList = OtherList;

                    string SortingCriteria = rn["SortingCriteria"].ToString();
                    Debug.WriteLine("Sorting Criteria string :" + SortingCriteria);
                    ViewBag.SortingCriteria = SortingCriteria;

                    //Inserting Instruction
                    if (!string.IsNullOrEmpty(rn["Letter"].ToString()))
                    {
                        InsertingInstruction.Add("Letter");
                    }

                    if (!string.IsNullOrEmpty(rn["Brochures_Leaflets"].ToString()))
                    {
                        InsertingInstruction.Add("Brochures/Leaflets");
                    }

                    if (!string.IsNullOrEmpty(rn["ReplyEnvelope"].ToString()))
                    {
                        InsertingInstruction.Add("Reply Envelope");
                    }

                    if (!string.IsNullOrEmpty(rn["ImgOnStatement"].ToString()))
                    {
                        InsertingInstruction.Add("Image On Statement");
                    }

                    if (!string.IsNullOrEmpty(rn["Booklet"].ToString()))
                    {
                        InsertingInstruction.Add("Booklet");
                    }

                    ViewBag.InsertInstruction = InsertingInstruction;

                    //FINISHING INSTRUCTION

                    string NumberOfInsert = rn["NumberOfInsert"].ToString();
                    Debug.WriteLine("Number Of Insert string :" + NumberOfInsert);
                    ViewBag.NumberOfInsert = NumberOfInsert;

                    //Manual Type
                    if (!string.IsNullOrEmpty(rn["Magezine"].ToString()))
                    {
                        ManualType.Add("Magezine");
                    }

                    if (!string.IsNullOrEmpty(rn["Brochure"].ToString()))
                    {
                        ManualType.Add("Brochure");
                    }

                    if (!string.IsNullOrEmpty(rn["CarrierSheet"].ToString()))
                    {
                        ManualType.Add("Carrier Sheet");
                    }

                    if (!string.IsNullOrEmpty(rn["Newsletter"].ToString()))
                    {
                        ManualType.Add("Newsletter");
                    }

                    if (!string.IsNullOrEmpty(rn["Statement"].ToString()))
                    {
                        ManualType.Add("Statement");
                    }

                    if (!string.IsNullOrEmpty(rn["Booklet1"].ToString()))
                    {
                        ManualType.Add("Booklet");
                    }

                    ViewBag.ManualType = ManualType;

                    string CommentManualType = rn["CommentManualType"].ToString();
                    Debug.WriteLine("Comment Manual Type string :" + CommentManualType);
                    ViewBag.CommentManualType = CommentManualType;

                    string FinishingFormat = rn["FinishingFormat"].ToString();
                    Debug.WriteLine("Finishing Format string :" + FinishingFormat);
                    ViewBag.FinishingFormat = FinishingFormat;

                    //Folding and Labelling Instruction

                    string FoldingType = rn["FoldingType"].ToString();
                    Debug.WriteLine("Folding Type string :" + FoldingType);
                    ViewBag.FoldingType = FoldingType;

                    //Address Labelling
                    if (!string.IsNullOrEmpty(rn["Sealing"].ToString()))
                    {
                        AddressLabelling.Add("Sealing");
                    }

                    if (!string.IsNullOrEmpty(rn["Tearing"].ToString()))
                    {
                        AddressLabelling.Add("Tearing");
                    }

                    if (!string.IsNullOrEmpty(rn["BarcodeLabel"].ToString()))
                    {
                        AddressLabelling.Add("Barcode Label");
                    }

                    if (!string.IsNullOrEmpty(rn["Cutting"].ToString()))
                    {
                        AddressLabelling.Add("Cutting");
                    }

                    ViewBag.AddressLabelling = AddressLabelling;

                    string StickingOf = rn["StickingOf"].ToString();
                    Debug.WriteLine("Sticking Of string :" + StickingOf);
                    ViewBag.StickingOf = StickingOf;

                    //Finishing Type
                    if (!string.IsNullOrEmpty(rn["AddLabel"].ToString()))
                    {
                        FinishingType.Add("Add Label");
                    }

                    if (!string.IsNullOrEmpty(rn["Sticker"].ToString()))
                    {
                        FinishingType.Add("Sticker");
                    }

                    if (!string.IsNullOrEmpty(rn["Chesire"].ToString()))
                    {
                        FinishingType.Add("Chesire");
                    }

                    if (!string.IsNullOrEmpty(rn["Tuck_In"].ToString()))
                    {
                        FinishingType.Add("Tuck In");
                    }
                    if (!string.IsNullOrEmpty(rn["Bursting"].ToString()))
                    {
                        FinishingType.Add("Bursting");
                    }

                    if (!string.IsNullOrEmpty(rn["Sealed"].ToString()))
                    {
                        FinishingType.Add("Sealed");
                    }

                    if (!string.IsNullOrEmpty(rn["Folding"].ToString()))
                    {
                        FinishingType.Add("Folding");
                    }

                    if (!string.IsNullOrEmpty(rn["Unsealed"].ToString()))
                    {
                        FinishingType.Add("Unsealed");

                    }

                    if (!string.IsNullOrEmpty(rn["Letter1"].ToString()))
                    {
                        FinishingType.Add("Letter");

                    }


                    ViewBag.FinishingType = FinishingType;


                    string IT_SysNotes = rn["IT_SysNotes"].ToString();
                    Debug.WriteLine("IT System Notes string :" + IT_SysNotes);
                    ViewBag.IT_SysNotes = IT_SysNotes;

                    string Produc_PlanningNotes = rn["Produc_PlanningNotes"].ToString();
                    Debug.WriteLine("Produc_PlanningNotes string :" + Produc_PlanningNotes);
                    ViewBag.Produc_PlanningNotes = Produc_PlanningNotes;

                    string PurchasingNotes = rn["PurchasingNotes"].ToString();
                    Debug.WriteLine("PurchasingNotes string :" + PurchasingNotes);
                    ViewBag.PurchasingNotes = PurchasingNotes;

                    string EngineeringNotes = rn["EngineeringNotes"].ToString();
                    Debug.WriteLine("EngineeringNotes string :" + EngineeringNotes);
                    ViewBag.EngineeringNotes = EngineeringNotes;

                    string ArtworkNotes = rn["ArtworkNotes"].ToString();
                    Debug.WriteLine("ArtworkNotes string :" + ArtworkNotes);
                    ViewBag.ArtworkNotes = ArtworkNotes;

                    string Acc_BillingNotes = rn["Acc_BillingNotes"].ToString();
                    Debug.WriteLine("Acc_BillingNotes string :" + Acc_BillingNotes);
                    ViewBag.Acc_BillingNotes = Acc_BillingNotes;

                    string DCPNotes = rn["DCPNotes"].ToString();
                    Debug.WriteLine("DCPNotes string :" + DCPNotes);
                    ViewBag.DCPNotes = DCPNotes;

                    string PostingInfo = rn["PostingInfo"].ToString();
                    Debug.WriteLine("PostingInfo string :" + PostingInfo);
                    ViewBag.PostingInfo = PostingInfo;


                }
                    cn.Close();
                }

            //return new Rotativa.ViewAsPdf("Index")
            //{
            //    // FileName = flightPlan.ListingItemDetailsModel.FlightDetails + ".pdf",
            //    FileName = "JI.pdf",
            //    PageMargins = new Rotativa.Options.Margins(12, 12, 12, 12),
            //    //PageOrientation = Rotativa.Options.Orientation.Landscape,
            //    PageOrientation = Rotativa.Options.Orientation.Portrait,
            //    //PageWidth = 210,
            //    //PageHeight = 297,
            //    PageSize = Rotativa.Options.Size.A4 // Ensure the PDF size is set to A4
            //};
            return View();
        }

        }
    }
