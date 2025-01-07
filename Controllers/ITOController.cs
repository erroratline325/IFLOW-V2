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
using static MvcAppV2.Models.NoCounterModel.NoProductionModel;
using System.Diagnostics;
using MigraDoc.DocumentObjectModel.IO;
using Org.BouncyCastle.Bcpg;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using Org.BouncyCastle.Crypto.Digests;
using Microsoft.Ajax.Utilities;
using System.Web.Razor.Tokenizer.Symbols;

[MvcAppV2.FilterConfig.UserSessionActionFilter]
public class ITOController : Controller
{
    string PathSource = System.Configuration.ConfigurationManager.AppSettings["SourceFile"];
    string IpSMtp_ = System.Configuration.ConfigurationManager.AppSettings["IpSMtp"];
    string PortSmtp_ = System.Configuration.ConfigurationManager.AppSettings["PortSmtp"];


    List<JobInstruction> JobInstructionlist1 = new List<JobInstruction>();
    public Document doc { get; private set; }


    public ActionResult ManageAssignProgrammer(string Id, string ProductName, string AssignByLeader, string product, string set)
    {
        if (set == "search")
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass, JobType, JobSheetNo, JobRequest, SalesExecutiveBy, AssignByLeader, ProgrammerBy, StartDevDate, EndDevDate, Status, Complexity, ModifiedOn
                                        FROM [dbo].[JobInstruction]                                    
                                        WHERE ProductName LIKE @ProductName
                                        AND Status = 'Waiting to Assign Programmer'
                                        ORDER BY ModifiedOn desc";
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
                            model.ModifiedOn = reader.GetDateTime(14);
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
                command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass, JobType, JobSheetNo, JobRequest, SalesExecutiveBy, AssignByLeader, ProgrammerBy, StartDevDate, EndDevDate, Status, Complexity, ModifiedOn
                                        FROM [dbo].[JobInstruction]
                                        WHERE Status = 'Waiting to Assign Programmer' 
                                        ORDER BY ModifiedOn desc";
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
                            model.ModifiedOn = reader.GetDateTime(14);
                        }

                    }
                    JobInstructionlist1.Add(model);
                }
                cn.Close();
            }
        }
        return View(JobInstructionlist1); //hntr data ke ui
    }

    public ActionResult AssignProgrammer(JobInstruction JobInstruction, string Id, string set, string tabs, string Customer_Name, string ProductName, string JobSheetNo,
                                             string SalesExecutiveBy, string Status, JobInstruction get,
                                             string ServiceLevel, string IsSlaCreaditCard, string JobClass, string IsSetPaper,
                                             string JobRequest, string ExpectedDateCompletionToGpo, string QuotationRef, string ContractRef,
                                             string ContactPerson, string JobType, string DeliveryChannel, string AccountsQty, string ImpressionQty,
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
                                             string SortingCriteria, string PrintingInstr, string SortingInstr,
                                             string Picture_FileId, string Picture_Extension, string Letter, string Brochures_Leaflets,
                                             string ReplyEnvelope, string ImgOnStatement, string Booklet,
                                             string NumberOfInsert, string Magezine1, string Brochure1, string CarrierSheet1, string Newsletter1,
                                             string Statement1, string Booklet1, string CommentManualType, string FinishingFormat,
                                             string FoldingType, string Sealing1, string Tearing1, string BarcodeLabel1, string Cutting1,
                                             string StickingOf1, string AddLabel1, string Sticker1, string Chesire1, string Tuck_In1,
                                             string Bursting1, string Sealed1, string Folding1, string Unsealed1, string Letter1, string FinishingInst,
                                             string IT_SysNotes, string Produc_PlanningNotes, string PurchasingNotes, string EngineeringNotes,
                                             string AssignByLeader, string ProgrammerBy, string StartDevDate, string EndDevDate, string Complexity)
    {

        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];
        Session["Id"] = Id;
        ViewBag.Id = Id;

        int _bil19 = 1;
        List<SelectListItem> li19 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT ProgrammerBy FROM [dbo].[ITO_Programmer]          
                                    ORDER BY ProgrammerBy ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.ProgrammerBy = reader.GetString(0);
                    }
                }
                int i = _bil19++;
                if (i == 1)
                {
                    li19.Add(new SelectListItem { Text = "Please Select" });
                    li19.Add(new SelectListItem { Text = model.ProgrammerBy });

                }
                else
                {
                    li19.Add(new SelectListItem { Text = model.ProgrammerBy });
                }
            }
            cn.Close();
        }
        ViewData["ProgrammerBy_"] = li19;



        List<SelectListItem> listComplexity = new List<SelectListItem>();

        listComplexity.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listComplexity.Add(new SelectListItem { Text = "NORMAL", Value = "NORMAL" });
        listComplexity.Add(new SelectListItem { Text = "MEDIUM", Value = "MEDIUM" });
        listComplexity.Add(new SelectListItem { Text = "ADVANCE", Value = "ADVANCE" });

        ViewData["Complexity_"] = listComplexity;

        if (!string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(ProgrammerBy) && !string.IsNullOrEmpty(Complexity))
        {
            string ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
            get.StartDevDate = Convert.ToDateTime(get.StartDevDateTxt);
            get.EndDevDate = Convert.ToDateTime(get.EndDevDateTxt);

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("UPDATE [dbo].[JobInstruction] SET ModifiedOn=@ModifiedOn, AssignByLeader=@AssignByLeader, ProgrammerBy=@ProgrammerBy, StartDevDate=@StartDevDate, EndDevDate=@EndDevDate, Complexity=@Complexity, Status=@Status WHERE Id =@Id", cn);
                command.Parameters.AddWithValue("@ModifiedOn", ModifiedOn);
                command.Parameters.AddWithValue("@AssignByLeader", IdentityName.ToString());
                command.Parameters.AddWithValue("@ProgrammerBy", ProgrammerBy);
                if (!string.IsNullOrEmpty(StartDevDate))
                {
                    string aaa = Convert.ToDateTime(StartDevDate).ToString("yyyy-MM-dd");
                    command.Parameters.AddWithValue("@StartDevDate", aaa);

                }
                else
                {
                    command.Parameters.AddWithValue("@StartDevDate", null);
                }
                if (!string.IsNullOrEmpty(EndDevDate))
                {
                    string aaa1 = Convert.ToDateTime(EndDevDate).ToString("yyyy-MM-dd");
                    command.Parameters.AddWithValue("@EndDevDate", aaa1);

                }
                else
                {
                    command.Parameters.AddWithValue("@EndDevDate", null);
                }

                command.Parameters.AddWithValue("@Complexity", Complexity);
                command.Parameters.AddWithValue("@Status", "Under Development");
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn.Close();

            }

            using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn3))
            {

                cn3.Open();
                command.CommandText = @"SELECT Id,Customer_Name,ProductName,JobClass,JobSheetNo,ModifiedOn,
                                            SalesExecutiveBy,Complexity,JobRequest,ProgrammerBy,Paper,
                                            StartDevDate,EndDevDate
                                            FROM [dbo].[JobInstruction] 
                                            WHERE Id=@Id";
                command.Parameters.AddWithValue("@Id", Id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JobInstruction model = new JobInstruction();
                    {
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
                            model.JobSheetNo = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            model.ModifiedOn = reader.GetDateTime(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.SalesExecutiveBy = reader.GetString(6);
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.Complexity = reader.GetString(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.JobRequest = reader.GetDateTime(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            model.ProgrammerBy = reader.GetString(9);
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            model.Paper = reader.GetString(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            model.StartDevOn = reader.GetDateTime(11);
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            model.EndDevDate = reader.GetDateTime(12);
                        }

                    }

                    using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        Guid WorksheetId = Guid.NewGuid();
                        ViewBag.Id = WorksheetId;
                        string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                        get.JobRequest = Convert.ToDateTime(get.JobRequestTxt);
                        get.StartDevOn = Convert.ToDateTime(get.StartDevOnTxt);
                        get.CompleteDevOn = Convert.ToDateTime(get.CompleteDevOnTxt);

                        cn2.Open();
                        SqlCommand command2;
                        command2 = new SqlCommand("INSERT INTO [dbo].[ProgDevWorksheet] (Id,CreatedOn,Customer_Name,ProductName,JobClass,JobSheetNo,SalesExecutiveBy,Status,Complexity,JobRequest,ProgrammerBy,Paper,StartDevOn,CompleteDevOn,JobInstructionId) values (@Id,@CreatedOn,@Customer_Name,@ProductName,@JobClass,@JobSheetNo,@SalesExecutiveBy,@Status,@Complexity,@JobRequest,@ProgrammerBy,@Paper,@StartDevOn,@CompleteDevOn,@JobInstructionId)", cn2);
                        command2.Parameters.AddWithValue("@Id", WorksheetId);
                        command2.Parameters.AddWithValue("@CreatedOn", createdOn);
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
                        if (model.JobClass != null)
                        {
                            command2.Parameters.AddWithValue("@JobClass", model.JobClass);
                        }
                        else
                        {
                            command2.Parameters.AddWithValue("@JobClass", DBNull.Value);
                        }
                        if (model.JobSheetNo != null)
                        {
                            command2.Parameters.AddWithValue("@JobSheetNo", model.JobSheetNo);
                        }
                        else
                        {
                            command2.Parameters.AddWithValue("@JobSheetNo", DBNull.Value);
                        }
                        if (model.SalesExecutiveBy != null)
                        {
                            command2.Parameters.AddWithValue("@SalesExecutiveBy", model.SalesExecutiveBy);
                        }
                        else
                        {
                            command2.Parameters.AddWithValue("@SalesExecutiveBy", DBNull.Value);
                        }
                        command2.Parameters.AddWithValue("@Status", "New");
                        if (model.Complexity != null)
                        {
                            command2.Parameters.AddWithValue("@Complexity", model.Complexity);
                        }
                        else
                        {
                            command2.Parameters.AddWithValue("@Complexity", DBNull.Value);
                        }
                        if (model.JobRequest != null)
                        {
                            //DateTime.ParseExact("06-13-2012", "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                            command2.Parameters.AddWithValue("@JobRequest", model.JobRequest);
                            Debug.WriteLine("not empty");
                            Debug.WriteLine(model.JobRequest.ToString());
                        }
                        else
                        {
                            Debug.WriteLine("empty");
                            Debug.WriteLine(model.JobRequest.ToString());
                            command2.Parameters.AddWithValue("@JobRequest", null);
                        }
                        if (model.ProgrammerBy != null)
                        {
                            command2.Parameters.AddWithValue("@ProgrammerBy", model.ProgrammerBy);
                        }
                        else
                        {
                            command2.Parameters.AddWithValue("@ProgrammerBy", DBNull.Value);
                        }
                        if (model.Paper != null)
                        {
                            command2.Parameters.AddWithValue("@Paper", model.Paper);
                        }
                        else
                        {
                            command2.Parameters.AddWithValue("@Paper", DBNull.Value);
                        }
                        if (model.StartDevOn != null)
                        {
                            //DateTime.ParseExact("06-13-2012", "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                            command2.Parameters.AddWithValue("@StartDevOn", model.StartDevOn);
                        }
                        else
                        {
                            command2.Parameters.AddWithValue("@StartDevOn", null);
                        }
                        if (model.EndDevDate != null)
                        {
                            //DateTime.ParseExact("06-13-2012", "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                            command2.Parameters.AddWithValue("@CompleteDevOn", model.EndDevDate);
                        }
                        else
                        {
                            command2.Parameters.AddWithValue("@CompleteDevOn", null);
                        }

                        command2.Parameters.AddWithValue("@JobInstructionId", Id);
                        command2.ExecuteNonQuery();
                        cn2.Close();



                        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            cn.Open();
                            SqlCommand command1;
                            command1 = new SqlCommand("UPDATE [dbo].[JobInstruction] SET STATUS='Development Process' WHERE Id=@Id", cn);
                            command1.Parameters.AddWithValue("@Id", Id);
                            command1.ExecuteNonQuery();
                            cn.Close();
                        }


                    }

                }
                cn3.Close();
                TempData["msg"] = "<script>alert('JI ALREADY SENT TO DEVELOPMENT PROCESS!');</script>";


                return RedirectToAction("ManageAssignProgrammer", "ITO");

            }



        }

        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT Id,ProgrammerBy,StartDevDate,EndDevDate,Complexity
                                    FROM [dbo].[JobInstruction]
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
                    ViewBag.ProgrammerBy = reader.GetString(1);
                }
                if (reader.IsDBNull(2) == false)
                {
                    ViewBag.StartDevDate = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(2));
                }
                if (reader.IsDBNull(3) == false)
                {
                    ViewBag.EndDevDate = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(3));
                }
                if (reader.IsDBNull(4) == false)
                {
                    ViewBag.Complexity = reader.GetString(4);
                }


            }
            cn.Close();
        }

        return View();

    }

    List<ProgDevWorksheet> viewProgDevWorksheet = new List<ProgDevWorksheet>();
    public ActionResult ManageProgDevWorksheet(string Id, string product, string Customer_Name, string ProductName, string JobClass, string JobSheetNo, string JobRequest, string set, string SalesExecutiveBy, string Status, string Complexity, ProgDevWorksheet get)

    {
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.IsRole = @Session["Role"];
        var IdentityName = @Session["Fullname"];

        if (set == "search")
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Id,Customer_Name,ProductName,JobClass,JobSheetNo,JobRequest,
                                        SalesExecutiveBy,Status,Complexity,StartDevOn,CompleteDevOn,
                                        Paper,MainProgramId,ProgramId,ProgramDesc,TypeOfData,ReasonDev,
                                        ProgramType,IsDedup,Dedup,IsSplitting,Splitting,IsRestructuring,
                                        Restructuring,Charges,TotalCharges,JobInstructionId,ProgrammerBy,up_1,up_2,CreateUser
                                        FROM [dbo].[ProgDevWorksheet] 
                                        WHERE ProductName LIKE @ProductName
                                        AND (Status = 'New') OR (Status ='Under Development')                                     
                                        ORDER BY CreatedOn desc";
                command.Parameters.AddWithValue("@ProductName", "%" + product + "%");


                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ProgDevWorksheet model = new ProgDevWorksheet();
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
                            model.JobSheetNo = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            model.JobRequest = reader.GetDateTime(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.SalesExecutiveBy = reader.GetString(6);
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.Status = reader.GetString(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.Complexity = reader.GetString(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            model.StartDevOn = reader.GetDateTime(9);
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            model.CompleteDevOn = reader.GetDateTime(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            model.Paper = reader.GetString(11);
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            model.MainProgramId = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            model.ProgramId = reader.GetString(13);
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            model.ProgramDesc = reader.GetString(14);
                        }
                        if (reader.IsDBNull(15) == false)
                        {
                            model.TypeOfData = reader.GetString(15);
                        }
                        if (reader.IsDBNull(16) == false)
                        {
                            model.ReasonDev = reader.GetString(16);
                        }
                        if (reader.IsDBNull(17) == false)
                        {
                            model.ProgramType = reader.GetString(17);
                        }
                        if (reader.IsDBNull(18) == false)
                        {
                            model.IsDedup = reader.GetBoolean(18);
                        }
                        if (reader.IsDBNull(19) == false)
                        {
                            model.Dedup = reader.GetString(19);
                        }
                        if (reader.IsDBNull(20) == false)
                        {
                            model.IsSplitting = reader.GetBoolean(20);
                        }
                        if (reader.IsDBNull(21) == false)
                        {
                            model.Splitting = reader.GetString(21);
                        }
                        if (reader.IsDBNull(22) == false)
                        {
                            model.IsRestructuring = reader.GetBoolean(22);
                        }
                        if (reader.IsDBNull(23) == false)
                        {
                            model.Restructuring = reader.GetString(23);
                        }
                        if (reader.IsDBNull(24) == false)
                        {
                            model.Charges = reader.GetString(24);
                        }
                        if (reader.IsDBNull(25) == false)
                        {
                            model.TotalCharges = reader.GetString(25);
                        }
                        if (reader.IsDBNull(26) == false)
                        {
                            model.JobInstructionId = reader.GetGuid(26);
                        }
                        if (reader.IsDBNull(27) == false)
                        {
                            model.ProgrammerBy = reader.GetString(27);
                        }
                        if (reader.IsDBNull(28) == false)
                        {
                            model.up_1 = reader.GetBoolean(28);
                        }
                        if (reader.IsDBNull(29) == false)
                        {
                            model.up_2 = reader.GetBoolean(29);
                        }
                        if (reader.IsDBNull(30) == false)
                        {
                            model.CreateUser = reader.GetString(30);
                        }

                    }
                    viewProgDevWorksheet.Add(model);
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
                command.CommandText = @"SELECT Id,Customer_Name,ProductName,JobClass,JobSheetNo,JobRequest,
                                        SalesExecutiveBy,Status,Complexity,StartDevOn,CompleteDevOn,
                                        Paper,MainProgramId,ProgramId,ProgramDesc,TypeOfData,ReasonDev,
                                        ProgramType,IsDedup,Dedup,IsSplitting,Splitting,IsRestructuring,
                                        Restructuring,Charges,TotalCharges,JobInstructionId,ProgrammerBy,up_1,up_2,CreateUser
                                        FROM [dbo].[ProgDevWorksheet] 
                                        WHERE (Status = 'New') OR (Status ='Under Development')                                     
                                        ORDER BY CreatedOn desc";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ProgDevWorksheet model = new ProgDevWorksheet();
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
                            model.JobSheetNo = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            model.JobRequest = reader.GetDateTime(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.SalesExecutiveBy = reader.GetString(6);
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.Status = reader.GetString(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.Complexity = reader.GetString(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            model.StartDevOn = reader.GetDateTime(9);
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            model.CompleteDevOn = reader.GetDateTime(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            model.Paper = reader.GetString(11);
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            model.MainProgramId = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            model.ProgramId = reader.GetString(13);
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            model.ProgramDesc = reader.GetString(14);
                        }
                        if (reader.IsDBNull(15) == false)
                        {
                            model.TypeOfData = reader.GetString(15);
                        }
                        if (reader.IsDBNull(16) == false)
                        {
                            model.ReasonDev = reader.GetString(16);
                        }
                        if (reader.IsDBNull(17) == false)
                        {
                            model.ProgramType = reader.GetString(17);
                        }
                        if (reader.IsDBNull(18) == false)
                        {
                            model.IsDedup = reader.GetBoolean(18);
                        }
                        if (reader.IsDBNull(19) == false)
                        {
                            model.Dedup = reader.GetString(19);
                        }
                        if (reader.IsDBNull(20) == false)
                        {
                            model.IsSplitting = reader.GetBoolean(20);
                        }
                        if (reader.IsDBNull(21) == false)
                        {
                            model.Splitting = reader.GetString(21);
                        }
                        if (reader.IsDBNull(22) == false)
                        {
                            model.IsRestructuring = reader.GetBoolean(22);
                        }
                        if (reader.IsDBNull(23) == false)
                        {
                            model.Restructuring = reader.GetString(23);
                        }
                        if (reader.IsDBNull(24) == false)
                        {
                            model.Charges = reader.GetString(24);
                        }
                        if (reader.IsDBNull(25) == false)
                        {
                            model.TotalCharges = reader.GetString(25);
                        }
                        if (reader.IsDBNull(26) == false)
                        {
                            model.JobInstructionId = reader.GetGuid(26);
                        }
                        if (reader.IsDBNull(27) == false)
                        {
                            model.ProgrammerBy = reader.GetString(27);
                        }
                        if (reader.IsDBNull(28) == false)
                        {
                            model.up_1 = reader.GetBoolean(28);
                        }
                        if (reader.IsDBNull(29) == false)
                        {
                            model.up_2 = reader.GetBoolean(29);
                        }
                        if (reader.IsDBNull(30) == false)
                        {
                            model.CreateUser = reader.GetString(30);
                        }
                    }
                    viewProgDevWorksheet.Add(model);
                }
                cn.Close();
            }
        }


        return View(viewProgDevWorksheet);

    }

    [ValidateInput(false)]
    public ActionResult CreateProgDevWorksheet(string Id, string set, string Activities, string JobInstructionId, string Set, string Customer_Name, string ProductName, string JobClass, string JobSheetNo, string JobRequest, string SalesExecutiveBy, string Status, string Complexity,
                                    ProgDevWorksheet get, string Paper, string MainProgramId, string ProgramId, string ProgramDesc, string TypeOfData, string StartDevOn, string CompleteDevOn, string ReasonDev, string CreateUser,
                                    string ProgramType, string IsDedup, string Dedup, string IsSplitting, string Splitting, string IsRestructuring, string Restructuring, string Charges, string TotalCharges, string up_1, string up_2,
                                    string IsReviseTemplate, string ReviseTemplate, string IsReviseContent, string ReviseContent, string IsReviseDataStructure, string ReviseDataStructure, string Field_1until10, string Field_11until20,
                                    string Field_21until30, string AmendmentCharges, string Activites, string Duration)
    {
        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.Id = Id;
        ViewBag.Customer_Name = Customer_Name;
        ViewBag.ProductName = ProductName;
        ViewBag.ProductName = ProductName;
        ViewBag.JobClass = JobClass;
        ViewBag.Complexity = Complexity;
        ViewBag.SalesExecutiveBy = SalesExecutiveBy;
        ViewBag.JobRequest = JobRequest;
        Session["JobInstructionId"] = Id;
        Session["ProgDevWorksheetId"] = Id;
        Session["Id"] = Id;




        List<SelectListItem> listMainProgramId = new List<SelectListItem>();
        listMainProgramId.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listMainProgramId.Add(new SelectListItem { Text = "i2s", Value = "i2s" });
        listMainProgramId.Add(new SelectListItem { Text = "PReS", Value = "PReS" });
        listMainProgramId.Add(new SelectListItem { Text = "Python", Value = "Python" });

        ViewData["MainProgramId_"] = listMainProgramId;

        List<SelectListItem> listTypeOfData = new List<SelectListItem>();
        listTypeOfData.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listTypeOfData.Add(new SelectListItem { Text = "BCIDIC", Value = "BCIDIC" });
        listTypeOfData.Add(new SelectListItem { Text = "ASCII", Value = "ASCII" });
        listTypeOfData.Add(new SelectListItem { Text = "DBF", Value = "DBF" });
        listTypeOfData.Add(new SelectListItem { Text = "OTHERS(csv)", Value = "OTHERS(csv)" });
        ViewData["TypeOfData_"] = listTypeOfData;


        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT Id,Customer_Name,ProductName,JobClass,JobSheetNo,JobRequest,SalesExecutiveBy,Complexity,
                                   up_1,up_2,MainProgramId,ProgramId,ProgramDesc,TypeOfData,StartDevOn,CompleteDevOn,ReasonDev,
                                   ProgramType,IsDedup,Dedup,IsSplitting,Splitting,IsRestructuring,Restructuring,Charges,TotalCharges,
                                   IsReviseTemplate,ReviseTemplate,IsReviseContent,ReviseContent,IsReviseDataStructure,ReviseDataStructure,
                                   Field_1until10,Field_11until20,Field_21until30, AmendmentCharges,
                                   IsReviseTemplate,ReviseTemplate,IsReviseContent,ReviseContent,IsReviseDataStructure,ReviseDataStructure,
                                   Field_1until10,Field_11until20,Field_21until30, AmendmentCharges
                                   FROM [dbo].[ProgDevWorksheet]                                     
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
                    ViewBag.JobClass = reader.GetString(3);
                }
                if (reader.IsDBNull(4) == false)
                {
                    ViewBag.JobSheetNo = reader.GetString(4);
                }
                if (reader.IsDBNull(5) == false)
                {
                    ViewBag.JobRequest = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(5));
                }
                if (reader.IsDBNull(6) == false)
                {
                    ViewBag.SalesExecutiveBy = reader.GetString(6);
                }
                if (reader.IsDBNull(7) == false)
                {
                    ViewBag.Complexity = reader.GetString(7);
                }
                if (reader.IsDBNull(8) == false)
                {
                    bool getup_1 = reader.GetBoolean(8);
                    if (getup_1 == false)
                    {
                        ViewBag.up_1 = "";
                    }
                    else
                    {
                        ViewBag.up_1 = "checked";
                    }
                }
                if (reader.IsDBNull(9) == false)
                {
                    bool getup_2 = reader.GetBoolean(9);
                    if (getup_2 == false)
                    {
                        ViewBag.up_2 = "";
                    }
                    else
                    {
                        ViewBag.up_2 = "checked";
                    }
                }
                if (reader.IsDBNull(10) == false)
                {
                    ViewBag.MainProgramId = reader.GetString(10);
                }
                if (reader.IsDBNull(11) == false)
                {
                    ViewBag.ProgramId = reader.GetString(11);
                }
                if (reader.IsDBNull(12) == false)
                {
                    ViewBag.ProgramDesc = reader.GetString(12);
                }
                if (reader.IsDBNull(13) == false)
                {
                    ViewBag.TypeOfData = reader.GetString(13);
                }
                if (reader.IsDBNull(14) == false)
                {
                    ViewBag.StartDevOn = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(14));
                }
                if (reader.IsDBNull(15) == false)
                {
                    ViewBag.CompleteDevOn = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(15));
                }
                if (reader.IsDBNull(16) == false)
                {
                    ViewBag.ReasonDev = reader.GetString(16);
                }
                if (reader.IsDBNull(17) == false)
                {
                    ViewBag.ProgramType = reader.GetString(17);
                }
                if (reader.IsDBNull(18) == false)
                {
                    bool getIsDedup = reader.GetBoolean(18);
                    if (getIsDedup == false)
                    {
                        ViewBag.IsDedup = "";
                    }
                    else
                    {
                        ViewBag.IsDedup = "checked";
                    }
                }
                if (reader.IsDBNull(19) == false)
                {
                    ViewBag.Dedup = reader.GetString(19);
                }
                if (reader.IsDBNull(20) == false)
                {
                    bool getIsSplitting = reader.GetBoolean(20);
                    if (getIsSplitting == false)
                    {
                        ViewBag.IsSplitting = "";
                    }
                    else
                    {
                        ViewBag.IsSplitting = "checked";
                    }
                }
                if (reader.IsDBNull(21) == false)
                {
                    ViewBag.Splitting = reader.GetString(21);
                }
                if (reader.IsDBNull(22) == false)
                {
                    bool getIsRestructuring = reader.GetBoolean(22);
                    if (getIsRestructuring == false)
                    {
                        ViewBag.IsRestructuring = "";
                    }
                    else
                    {
                        ViewBag.IsRestructuring = "checked";
                    }
                }
                if (reader.IsDBNull(23) == false)
                {
                    ViewBag.Restructuring = reader.GetString(23);
                }
                if (reader.IsDBNull(24) == false)
                {
                    ViewBag.Charges = reader.GetString(24);
                }
                if (reader.IsDBNull(25) == false)
                {
                    ViewBag.TotalCharges = reader.GetString(25);
                }
                if (reader.IsDBNull(26) == false)
                {
                    bool getIsReviseTemplate = reader.GetBoolean(26);
                    if (getIsReviseTemplate == false)
                    {
                        ViewBag.IsReviseTemplate = "";
                    }
                    else
                    {
                        ViewBag.IsReviseTemplate = "checked";
                    }
                }
                if (reader.IsDBNull(27) == false)
                {
                    ViewBag.ReviseTemplate = reader.GetString(27);
                }
                if (reader.IsDBNull(28) == false)
                {
                    bool getIsReviseContent = reader.GetBoolean(28);
                    if (getIsReviseContent == false)
                    {
                        ViewBag.IsReviseContent = "";
                    }
                    else
                    {
                        ViewBag.IsReviseContent = "checked";
                    }
                }
                if (reader.IsDBNull(29) == false)
                {
                    ViewBag.ReviseContent = reader.GetString(29);
                }
                if (reader.IsDBNull(30) == false)
                {
                    bool getIsReviseDataStructure = reader.GetBoolean(30);
                    if (getIsReviseDataStructure == false)
                    {
                        ViewBag.IsReviseDataStructure = "";
                    }
                    else
                    {
                        ViewBag.IsReviseDataStructure = "checked";
                    }
                }
                if (reader.IsDBNull(31) == false)
                {
                    ViewBag.ReviseDataStructure = reader.GetString(31);
                }
                if (reader.IsDBNull(32) == false)
                {
                    bool getField_1until10 = reader.GetBoolean(32);
                    if (getField_1until10 == false)
                    {
                        ViewBag.Field_1until10 = "";
                    }
                    else
                    {
                        ViewBag.Field_1until10 = "checked";
                    }
                }
                if (reader.IsDBNull(33) == false)
                {
                    bool getField_11until20 = reader.GetBoolean(33);
                    if (getField_11until20 == false)
                    {
                        ViewBag.Field_11until20 = "";
                    }
                    else
                    {
                        ViewBag.Field_11until20 = "checked";
                    }
                }
                if (reader.IsDBNull(34) == false)
                {
                    bool getField_21until30 = reader.GetBoolean(34);
                    if (getField_21until30 == false)
                    {
                        ViewBag.Field_21until30 = "";
                    }
                    else
                    {
                        ViewBag.Field_21until30 = "checked";
                    }
                }
                if (reader.IsDBNull(35) == false)
                {
                    ViewBag.AmendmentCharges = reader.GetString(35);
                }


            }

            cn.Close();
        }




        if (set == "Main")
        {
            if (!string.IsNullOrEmpty(Id))
            {

                get.StartDevOn = Convert.ToDateTime(get.StartDevOnTxt);
                get.CompleteDevOn = Convert.ToDateTime(get.CompleteDevOnTxt);

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                    cn.Open();
                    SqlCommand command;
                    command = new SqlCommand("UPDATE [dbo].[ProgDevWorksheet] SET ModifiedOn=@ModifiedOn,up_1=@up_1,up_2=@up_2,MainProgramId=@MainProgramId,ProgramId=@ProgramId,ProgramDesc=@ProgramDesc,TypeOfData=@TypeOfData,StartDevOn=@StartDevOn,CompleteDevOn=@CompleteDevOn,ReasonDev=@ReasonDev,IsDedup=@IsDedup,Dedup=@Dedup,IsSplitting=@IsSplitting,Splitting=@Splitting,IsRestructuring=@IsRestructuring,Restructuring=@Restructuring,Charges=@Charges,TotalCharges=@TotalCharges,Status=@Status,CreateUser=@CreateUser WHERE Id=@Id", cn);
                    command.Parameters.AddWithValue("@ModifiedOn", createdOn);
                    if (up_1 == "on")
                    {
                        command.Parameters.AddWithValue("@up_1", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@up_1", false);
                    }
                    if (up_2 == "on")
                    {
                        command.Parameters.AddWithValue("@up_2", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@up_2", false);
                    }

                    if (!string.IsNullOrEmpty(MainProgramId))
                    {
                        command.Parameters.AddWithValue("@MainProgramId", MainProgramId);

                    }
                    else
                    {
                        command.Parameters.AddWithValue("@MainProgramId", DBNull.Value);

                    }

                    if (!string.IsNullOrEmpty(ProgramId))
                    {
                        command.Parameters.AddWithValue("@ProgramId", ProgramId);

                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ProgramId", DBNull.Value);

                    }

                    if (!string.IsNullOrEmpty(ProgramDesc))
                    {
                        command.Parameters.AddWithValue("@ProgramDesc", ProgramDesc);

                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ProgramDesc", DBNull.Value);

                    }

                    if (!string.IsNullOrEmpty(TypeOfData))
                    {
                        command.Parameters.AddWithValue("@TypeOfData", TypeOfData);

                    }
                    else
                    {
                        command.Parameters.AddWithValue("@TypeOfData", DBNull.Value);

                    }


                    if (!string.IsNullOrEmpty(StartDevOn))
                    {
                        string ccc = Convert.ToDateTime(StartDevOn).ToString("yyyy-MM-dd");
                        command.Parameters.AddWithValue("@StartDevOn", ccc);

                    }
                    else
                    {
                        command.Parameters.AddWithValue("@StartDevOn", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(CompleteDevOn))
                    {
                        string ccc1 = Convert.ToDateTime(CompleteDevOn).ToString("yyyy-MM-dd");
                        command.Parameters.AddWithValue("@CompleteDevOn", ccc1);

                    }
                    else
                    {
                        command.Parameters.AddWithValue("@CompleteDevOn", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(CompleteDevOn))
                    {
                        command.Parameters.AddWithValue("@ReasonDev", ReasonDev);

                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ReasonDev", DBNull.Value);

                    }


                    if (IsDedup == "on")
                    {
                        command.Parameters.AddWithValue("@IsDedup", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@IsDedup", false);
                    }

                    if (!string.IsNullOrEmpty(Dedup))
                    {
                        command.Parameters.AddWithValue("@Dedup", Dedup);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Dedup", DBNull.Value);
                    }

                    if (IsSplitting == "on")
                    {
                        command.Parameters.AddWithValue("@IsSplitting", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@IsSplitting", false);
                    }

                    if (!string.IsNullOrEmpty(Splitting))
                    {
                        command.Parameters.AddWithValue("@Splitting", Splitting);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Splitting", DBNull.Value);
                    }
                    if (IsRestructuring == "on")
                    {
                        command.Parameters.AddWithValue("@IsRestructuring", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@IsRestructuring", false);
                    }
                    if (!string.IsNullOrEmpty(Restructuring))
                    {
                        command.Parameters.AddWithValue("@Restructuring", Restructuring);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Restructuring", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(Charges))
                    {
                        command.Parameters.AddWithValue("@Charges", Charges);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Charges", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(TotalCharges))
                    {
                        command.Parameters.AddWithValue("@TotalCharges", TotalCharges);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@TotalCharges", DBNull.Value);
                    }
                    command.Parameters.AddWithValue("@Status", "Under Development");
                    command.Parameters.AddWithValue("@CreateUser", IdentityName.ToString());
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                    cn.Close();
                }

            }
            return RedirectToAction("ManageProgDevWorksheet", "ITO");

        }
        else if (set == "NewProgram")
        {
            if (!string.IsNullOrEmpty(Id))
            {

                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    Guid guidId = Guid.NewGuid();
                    cn2.Open();
                    SqlCommand command;
                    command = new SqlCommand("INSERT INTO [dbo].[ITO_NewProgram] (Id,ProgDevWorksheetId,Activities,Duration,Charges) values (@Id,@ProgDevWorksheetId,@Activities,@Duration,@Charges)", cn2);
                    command.Parameters.AddWithValue("@Id", guidId);
                    command.Parameters.AddWithValue("@ProgDevWorksheetId", Id);
                    command.Parameters.AddWithValue("@Activities", Activities);
                    command.Parameters.AddWithValue("@Duration", Duration);
                    command.Parameters.AddWithValue("@Charges", Charges);
                    command.ExecuteNonQuery();
                    cn2.Close();

                }

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn.Open();
                    SqlCommand command;
                    command = new SqlCommand("UPDATE [dbo].[ProgDevWorksheet] SET ProgramType=@ProgramType WHERE Id=@Id", cn);
                    command.Parameters.AddWithValue("@ProgramType", "New Program");
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                    cn.Close();
                }

            }
        }

        else if (set == "Amendment")
        {
            if (!string.IsNullOrEmpty(Id))
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn.Open();
                    SqlCommand command;
                    command = new SqlCommand("UPDATE [dbo].[ProgDevWorksheet] SET IsReviseTemplate=@IsReviseTemplate,ReviseTemplate=@ReviseTemplate,IsReviseContent=@IsReviseContent,ReviseContent=@ReviseContent,IsReviseDataStructure=@IsReviseDataStructure,ReviseDataStructure=@ReviseDataStructure,Field_1until10=@Field_1until10,Field_11until20=@Field_11until20,Field_21until30=@Field_21until30,AmendmentCharges=@AmendmentCharges,ProgramType=@ProgramType WHERE Id=@Id", cn);
                    if (IsReviseTemplate == "on")
                    {
                        command.Parameters.AddWithValue("@IsReviseTemplate", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@IsReviseTemplate", false);
                    }
                    command.Parameters.AddWithValue("@ReviseTemplate", ReviseTemplate);
                    if (IsReviseContent == "on")
                    {
                        command.Parameters.AddWithValue("@IsReviseContent", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@IsReviseContent", false);
                    }
                    command.Parameters.AddWithValue("@ReviseContent", ReviseContent);
                    if (IsReviseDataStructure == "on")
                    {
                        command.Parameters.AddWithValue("@IsReviseDataStructure", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@IsReviseDataStructure", false);
                    }
                    command.Parameters.AddWithValue("@ReviseDataStructure", ReviseDataStructure);
                    if (Field_1until10 == "on")
                    {
                        command.Parameters.AddWithValue("@Field_1until10", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Field_1until10", false);
                    }
                    if (Field_11until20 == "on")
                    {
                        command.Parameters.AddWithValue("@Field_11until20", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Field_11until20", false);
                    }
                    if (Field_21until30 == "on")
                    {
                        command.Parameters.AddWithValue("@Field_21until30", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Field_21until30", false);
                    }
                    command.Parameters.AddWithValue("@AmendmentCharges", AmendmentCharges);
                    command.Parameters.AddWithValue("@ProgramType", "Amendment");
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                    cn.Close();
                }


            }
            return RedirectToAction("ManageProgDevWorksheet", "ITO");





        }
        return View();

    }







    [ValidateInput(false)]
    public ActionResult CreateProgDevWorksheet1(string Id, string JobInstructionId, string Set, string Customer_Name, string ProductName, string JobClass, string JobSheetNo, string JobRequest, string SalesExecutiveBy, string Status, string Complexity,
                                               ProgDevWorksheet get, string Paper, string MainProgramId, string ProgramId, string ProgramDesc, string TypeOfData, string StartDevOn, string CompleteDevOn, string ReasonDev, string CreateUser,
                                               string ProgramType, string IsDedup, string Dedup, string IsSplitting, string Splitting, string IsRestructuring, string Restructuring, string Charges, string TotalCharges, string up_1, string up_2,
                                               string IsReviseTemplate, string ReviseTemplate, string IsReviseContent, string ReviseContent, string IsReviseDataStructure, string ReviseDataStructure, string Field_1until10, string Field_11until20,
                                               string Field_21until30, string AmendmentCharges, string Activites, string Duration)
    {





        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.Id = Id;
        Session["JobInstructionId"] = Id;
        Session["ProgDevWorksheetId"] = Id;




        List<SelectListItem> listMainProgramId = new List<SelectListItem>();
        listMainProgramId.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listMainProgramId.Add(new SelectListItem { Text = "i2s", Value = "i2s" });
        listMainProgramId.Add(new SelectListItem { Text = "PReS", Value = "PReS" });
        ViewData["MainProgramId_"] = listMainProgramId;

        List<SelectListItem> listTypeOfData = new List<SelectListItem>();
        listTypeOfData.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listTypeOfData.Add(new SelectListItem { Text = "BCIDIC", Value = "BCIDIC" });
        listTypeOfData.Add(new SelectListItem { Text = "ASCII", Value = "ASCII" });
        listTypeOfData.Add(new SelectListItem { Text = "DBF", Value = "DBF" });
        listTypeOfData.Add(new SelectListItem { Text = "OTHERS(csv)", Value = "OTHERS(csv)" });
        ViewData["TypeOfData_"] = listTypeOfData;

        if (!string.IsNullOrEmpty(Id) && MainProgramId != "Please Select" && TypeOfData != "Please Select" && !string.IsNullOrEmpty(MainProgramId) && !string.IsNullOrEmpty(ProgramId) && !string.IsNullOrEmpty(ProgramDesc) && !string.IsNullOrEmpty(TypeOfData) && !string.IsNullOrEmpty(StartDevOn) && !string.IsNullOrEmpty(CompleteDevOn) && !string.IsNullOrEmpty(ReasonDev) && !string.IsNullOrEmpty(Charges) && !string.IsNullOrEmpty(TotalCharges))
        {

            get.StartDevOn = Convert.ToDateTime(get.StartDevOnTxt);
            get.CompleteDevOn = Convert.ToDateTime(get.CompleteDevOnTxt);

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                cn.Open();
                SqlCommand command;
                command = new SqlCommand("UPDATE [dbo].[ProgDevWorksheet] SET ModifiedOn=@ModifiedOn,up_1=@up_1,up_2=@up_2,MainProgramId=@MainProgramId,ProgramId=@ProgramId,ProgramDesc=@ProgramDesc,TypeOfData=@TypeOfData,StartDevOn=@StartDevOn,CompleteDevOn=@CompleteDevOn,ReasonDev=@ReasonDev,IsDedup=@IsDedup,Dedup=@Dedup,IsSplitting=@IsSplitting,Splitting=@Splitting,IsRestructuring=@IsRestructuring,Restructuring=@Restructuring,Charges=@Charges,TotalCharges=@TotalCharges,Status=@Status,CreateUser=@CreateUser,IsReviseTemplate=@IsReviseTemplate,ReviseTemplate=@ReviseTemplate,IsReviseContent=@IsReviseContent,ReviseContent=@ReviseContent,IsReviseDataStructure=@IsReviseDataStructure,ReviseDataStructure=@ReviseDataStructure,Field_1until10=@Field_1until10,Field_11until20=@Field_11until20,Field_21until30=@Field_21until30,AmendmentCharges=@AmendmentCharges WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@ModifiedOn", createdOn);
                if (up_1 == "on")
                {
                    command.Parameters.AddWithValue("@up_1", true);
                }
                else
                {
                    command.Parameters.AddWithValue("@up_1", false);
                }
                if (up_2 == "on")
                {
                    command.Parameters.AddWithValue("@up_2", true);
                }
                else
                {
                    command.Parameters.AddWithValue("@up_2", false);
                }
                command.Parameters.AddWithValue("@MainProgramId", MainProgramId);
                command.Parameters.AddWithValue("@ProgramId", ProgramId);
                command.Parameters.AddWithValue("@ProgramDesc", ProgramDesc);
                command.Parameters.AddWithValue("@TypeOfData", TypeOfData);
                if (!string.IsNullOrEmpty(StartDevOn))
                {
                    string ccc = Convert.ToDateTime(StartDevOn).ToString("yyyy-MM-dd");
                    command.Parameters.AddWithValue("@StartDevOn", ccc);

                }
                else
                {
                    command.Parameters.AddWithValue("@StartDevOn", null);
                }
                if (!string.IsNullOrEmpty(CompleteDevOn))
                {
                    string ccc1 = Convert.ToDateTime(CompleteDevOn).ToString("yyyy-MM-dd");
                    command.Parameters.AddWithValue("@CompleteDevOn", ccc1);

                }
                else
                {
                    command.Parameters.AddWithValue("@CompleteDevOn", null);
                }
                command.Parameters.AddWithValue("@ReasonDev", ReasonDev);
                if (IsDedup == "on")
                {
                    command.Parameters.AddWithValue("@IsDedup", true);
                }
                else
                {
                    command.Parameters.AddWithValue("@IsDedup", false);
                }

                if (!string.IsNullOrEmpty(Dedup))
                {
                    command.Parameters.AddWithValue("@Dedup", Dedup);
                }
                else
                {
                    command.Parameters.AddWithValue("@Dedup", DBNull.Value);

                }
                if (IsSplitting == "on")
                {
                    command.Parameters.AddWithValue("@IsSplitting", true);
                }
                else
                {
                    command.Parameters.AddWithValue("@IsSplitting", false);
                }
                if (!string.IsNullOrEmpty(Splitting))
                {
                    command.Parameters.AddWithValue("@Splitting", Splitting);
                }
                else
                {
                    command.Parameters.AddWithValue("@Splitting", DBNull.Value);
                }
                if (IsRestructuring == "on")
                {
                    command.Parameters.AddWithValue("@IsRestructuring", true);
                }
                else
                {
                    command.Parameters.AddWithValue("@IsRestructuring", false);
                }
                if (!string.IsNullOrEmpty(Restructuring))
                {
                    command.Parameters.AddWithValue("@Restructuring", Restructuring);
                }
                else
                {
                    command.Parameters.AddWithValue("@Restructuring", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(Charges))
                {
                    command.Parameters.AddWithValue("@Charges", Charges);
                }
                else
                {
                    command.Parameters.AddWithValue("@Charges", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(TotalCharges))
                {
                    command.Parameters.AddWithValue("@TotalCharges", TotalCharges);
                }
                else
                {
                    command.Parameters.AddWithValue("@TotalCharges", DBNull.Value);
                }
                command.Parameters.AddWithValue("@Status", "Under Development");
                command.Parameters.AddWithValue("@CreateUser", IdentityName.ToString());
                if (IsReviseTemplate == "on")
                {
                    command.Parameters.AddWithValue("@IsReviseTemplate", true);
                }
                else
                {
                    command.Parameters.AddWithValue("@IsReviseTemplate", false);
                }
                if (!string.IsNullOrEmpty(ReviseTemplate))
                {
                    command.Parameters.AddWithValue("@ReviseTemplate", ReviseTemplate);
                }
                else
                {
                    command.Parameters.AddWithValue("@ReviseTemplate", DBNull.Value);
                }
                if (IsReviseContent == "on")
                {
                    command.Parameters.AddWithValue("@IsReviseContent", true);
                }
                else
                {
                    command.Parameters.AddWithValue("@IsReviseContent", false);
                }
                if (!string.IsNullOrEmpty(ReviseContent))
                {
                    command.Parameters.AddWithValue("@ReviseContent", ReviseContent);
                }
                else
                {
                    command.Parameters.AddWithValue("@ReviseContent", DBNull.Value);
                }
                if (IsReviseDataStructure == "on")
                {
                    command.Parameters.AddWithValue("@IsReviseDataStructure", true);
                }
                else
                {
                    command.Parameters.AddWithValue("@IsReviseDataStructure", false);
                }
                if (!string.IsNullOrEmpty(ReviseDataStructure))
                {
                    command.Parameters.AddWithValue("@ReviseDataStructure", ReviseDataStructure);
                }
                else
                {
                    command.Parameters.AddWithValue("@ReviseDataStructure", DBNull.Value);
                }
                if (Field_1until10 == "on")
                {
                    command.Parameters.AddWithValue("@Field_1until10", true);
                }
                else
                {
                    command.Parameters.AddWithValue("@Field_1until10", false);
                }
                if (Field_11until20 == "on")
                {
                    command.Parameters.AddWithValue("@Field_11until20", true);
                }
                else
                {
                    command.Parameters.AddWithValue("@Field_11until20", false);
                }
                if (Field_21until30 == "on")
                {
                    command.Parameters.AddWithValue("@Field_21until30", true);
                }
                else
                {
                    command.Parameters.AddWithValue("@Field_21until30", false);
                }
                if (!string.IsNullOrEmpty(AmendmentCharges))
                {
                    command.Parameters.AddWithValue("@AmendmentCharges", AmendmentCharges);
                }
                else
                {
                    command.Parameters.AddWithValue("@AmendmentCharges", DBNull.Value);
                }
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn.Close();
            }

        }

        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT Id,Customer_Name,ProductName,JobClass,JobSheetNo,JobRequest,SalesExecutiveBy,Complexity,
                                   up_1,up_2,MainProgramId,ProgramId,ProgramDesc,TypeOfData,StartDevOn,CompleteDevOn,ReasonDev,
                                   ProgramType,IsDedup,Dedup,IsSplitting,Splitting,IsRestructuring,Restructuring,Charges,TotalCharges,
                                   IsReviseTemplate,ReviseTemplate,IsReviseContent,ReviseContent,IsReviseDataStructure,ReviseDataStructure,
                                   Field_1until10,Field_11until20,Field_21until30, AmendmentCharges,
                                   IsReviseTemplate,ReviseTemplate,IsReviseContent,ReviseContent,IsReviseDataStructure,ReviseDataStructure,
                                   Field_1until10,Field_11until20,Field_21until30, AmendmentCharges
                                   FROM [dbo].[ProgDevWorksheet]                                     
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
                    ViewBag.JobClass = reader.GetString(3);
                }
                if (reader.IsDBNull(4) == false)
                {
                    ViewBag.JobSheetNo = reader.GetString(4);
                }
                if (reader.IsDBNull(5) == false)
                {
                    ViewBag.JobRequest = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(5));
                }
                if (reader.IsDBNull(6) == false)
                {
                    ViewBag.SalesExecutiveBy = reader.GetString(6);
                }
                if (reader.IsDBNull(7) == false)
                {
                    ViewBag.Complexity = reader.GetString(7);
                }
                if (reader.IsDBNull(8) == false)
                {
                    bool getup_1 = reader.GetBoolean(8);
                    if (getup_1 == false)
                    {
                        ViewBag.up_1 = "";
                    }
                    else
                    {
                        ViewBag.up_1 = "checked";
                    }
                }
                if (reader.IsDBNull(9) == false)
                {
                    bool getup_2 = reader.GetBoolean(9);
                    if (getup_2 == false)
                    {
                        ViewBag.up_2 = "";
                    }
                    else
                    {
                        ViewBag.up_2 = "checked";
                    }
                }
                if (reader.IsDBNull(10) == false)
                {
                    ViewBag.MainProgramId = reader.GetString(10);
                }
                if (reader.IsDBNull(11) == false)
                {
                    ViewBag.ProgramId = reader.GetString(11);
                }
                if (reader.IsDBNull(12) == false)
                {
                    ViewBag.ProgramDesc = reader.GetString(12);
                }
                if (reader.IsDBNull(13) == false)
                {
                    ViewBag.TypeOfData = reader.GetString(13);
                }
                if (reader.IsDBNull(14) == false)
                {
                    ViewBag.StartDevOn = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(14));
                }
                if (reader.IsDBNull(15) == false)
                {
                    ViewBag.CompleteDevOn = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(15));
                }
                if (reader.IsDBNull(16) == false)
                {
                    ViewBag.ReasonDev = reader.GetString(16);
                }
                if (reader.IsDBNull(17) == false)
                {
                    ViewBag.ProgramType = reader.GetString(17);
                }
                if (reader.IsDBNull(18) == false)
                {
                    bool getIsDedup = reader.GetBoolean(18);
                    if (getIsDedup == false)
                    {
                        ViewBag.IsDedup = "";
                    }
                    else
                    {
                        ViewBag.IsDedup = "checked";
                    }
                }
                if (reader.IsDBNull(19) == false)
                {
                    ViewBag.Dedup = reader.GetString(19);
                }
                if (reader.IsDBNull(20) == false)
                {
                    bool getIsSplitting = reader.GetBoolean(20);
                    if (getIsSplitting == false)
                    {
                        ViewBag.IsSplitting = "";
                    }
                    else
                    {
                        ViewBag.IsSplitting = "checked";
                    }
                }
                if (reader.IsDBNull(21) == false)
                {
                    ViewBag.Splitting = reader.GetString(21);
                }
                if (reader.IsDBNull(22) == false)
                {
                    bool getIsRestructuring = reader.GetBoolean(22);
                    if (getIsRestructuring == false)
                    {
                        ViewBag.IsRestructuring = "";
                    }
                    else
                    {
                        ViewBag.IsRestructuring = "checked";
                    }
                }
                if (reader.IsDBNull(23) == false)
                {
                    ViewBag.Restructuring = reader.GetString(23);
                }
                if (reader.IsDBNull(24) == false)
                {
                    ViewBag.Charges = reader.GetString(24);
                }
                if (reader.IsDBNull(25) == false)
                {
                    ViewBag.TotalCharges = reader.GetString(25);
                }
                if (reader.IsDBNull(26) == false)
                {
                    bool getIsReviseTemplate = reader.GetBoolean(26);
                    if (getIsReviseTemplate == false)
                    {
                        ViewBag.IsReviseTemplate = "";
                    }
                    else
                    {
                        ViewBag.IsReviseTemplate = "checked";
                    }
                }
                if (reader.IsDBNull(27) == false)
                {
                    ViewBag.ReviseTemplate = reader.GetString(27);
                }
                if (reader.IsDBNull(28) == false)
                {
                    bool getIsReviseContent = reader.GetBoolean(28);
                    if (getIsReviseContent == false)
                    {
                        ViewBag.IsReviseContent = "";
                    }
                    else
                    {
                        ViewBag.IsReviseContent = "checked";
                    }
                }
                if (reader.IsDBNull(29) == false)
                {
                    ViewBag.ReviseContent = reader.GetString(29);
                }
                if (reader.IsDBNull(30) == false)
                {
                    bool getIsReviseDataStructure = reader.GetBoolean(30);
                    if (getIsReviseDataStructure == false)
                    {
                        ViewBag.IsReviseDataStructure = "";
                    }
                    else
                    {
                        ViewBag.IsReviseDataStructure = "checked";
                    }
                }
                if (reader.IsDBNull(31) == false)
                {
                    ViewBag.ReviseDataStructure = reader.GetString(31);
                }
                if (reader.IsDBNull(32) == false)
                {
                    bool getField_1until10 = reader.GetBoolean(32);
                    if (getField_1until10 == false)
                    {
                        ViewBag.Field_1until10 = "";
                    }
                    else
                    {
                        ViewBag.Field_1until10 = "checked";
                    }
                }
                if (reader.IsDBNull(33) == false)
                {
                    bool getField_11until20 = reader.GetBoolean(33);
                    if (getField_11until20 == false)
                    {
                        ViewBag.Field_11until20 = "";
                    }
                    else
                    {
                        ViewBag.Field_11until20 = "checked";
                    }
                }
                if (reader.IsDBNull(34) == false)
                {
                    bool getField_21until30 = reader.GetBoolean(34);
                    if (getField_21until30 == false)
                    {
                        ViewBag.Field_21until30 = "";
                    }
                    else
                    {
                        ViewBag.Field_21until30 = "checked";
                    }
                }
                if (reader.IsDBNull(35) == false)
                {
                    ViewBag.AmendmentCharges = reader.GetString(35);
                }


            }

            cn.Close();
        }
        return View();
    }


    public ActionResult CreateAmendment(ProgDevWorksheet progDevWorksheet, string Id, string Set, string IsReviseTemplate, string ReviseTemplate, string IsReviseContent, string ReviseContent, string IsReviseDataStructure, string ReviseDataStructure, string Field_1until10, string Field_11until20,
                                         string Field_21until30, string AmendmentCharges, string Activites, string Duration)
    {
        var IdentityName = @Session["Fullname"];

        var Customer_Name = Session["Customer_Name"];
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.IsRole = @Session["Role"];
        string Deptment = @Session["Department"].ToString();


        if (!string.IsNullOrEmpty(Id) && Set == "Amendment")
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("UPDATE [dbo].[ProgDevWorksheet] SET IsReviseTemplate=@IsReviseTemplate,ReviseTemplate=@ReviseTemplate,IsReviseContent=@IsReviseContent,ReviseContent=@ReviseContent,IsReviseDataStructure=@IsReviseDataStructure,ReviseDataStructure=@ReviseDataStructure,Field_1until10=@Field_1until10,Field_11until20=@Field_11until20,Field_21until30=@Field_21until30,AmendmentCharges=@AmendmentCharges,ProgramType=@ProgramType WHERE Id=@Id", cn);
                if (IsReviseTemplate == "on")
                {
                    command.Parameters.AddWithValue("@IsReviseTemplate", true);
                }
                else
                {
                    command.Parameters.AddWithValue("@IsReviseTemplate", false);
                }
                command.Parameters.AddWithValue("@ReviseTemplate", ReviseTemplate);
                if (IsReviseContent == "on")
                {
                    command.Parameters.AddWithValue("@IsReviseContent", true);
                }
                else
                {
                    command.Parameters.AddWithValue("@IsReviseContent", false);
                }
                command.Parameters.AddWithValue("@ReviseContent", ReviseContent);
                if (IsReviseDataStructure == "on")
                {
                    command.Parameters.AddWithValue("@IsReviseDataStructure", true);
                }
                else
                {
                    command.Parameters.AddWithValue("@IsReviseDataStructure", false);
                }
                command.Parameters.AddWithValue("@ReviseDataStructure", ReviseDataStructure);
                if (Field_1until10 == "on")
                {
                    command.Parameters.AddWithValue("@Field_1until10", true);
                }
                else
                {
                    command.Parameters.AddWithValue("@Field_1until10", false);
                }
                if (Field_11until20 == "on")
                {
                    command.Parameters.AddWithValue("@Field_11until20", true);
                }
                else
                {
                    command.Parameters.AddWithValue("@Field_11until20", false);
                }
                if (Field_21until30 == "on")
                {
                    command.Parameters.AddWithValue("@Field_21until30", true);
                }
                else
                {
                    command.Parameters.AddWithValue("@Field_21until30", false);
                }
                command.Parameters.AddWithValue("@AmendmentCharges", AmendmentCharges);
                command.Parameters.AddWithValue("@ProgramType", "Amendment");
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn.Close();
            }

        }


        if (Set == "back")
        {
            return RedirectToAction("CreateProgDevWorksheet", "ITO", new { Id = Id.ToString() });
        }

        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT Id,
                                   IsReviseTemplate,ReviseTemplate,IsReviseContent,ReviseContent,IsReviseDataStructure,ReviseDataStructure,
                                   Field_1until10,Field_11until20,Field_21until30, AmendmentCharges
                                   FROM [IflowSeed].[dbo].[ProgDevWorksheet]                                     
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
                    bool getIsReviseTemplate = reader.GetBoolean(1);
                    if (getIsReviseTemplate == false)
                    {
                        ViewBag.IsReviseTemplate = "";
                    }
                    else
                    {
                        ViewBag.IsReviseTemplate = "checked";
                    }
                }
                if (reader.IsDBNull(2) == false)
                {
                    ViewBag.ReviseTemplate = reader.GetString(2);
                }
                if (reader.IsDBNull(3) == false)
                {
                    bool getIsReviseContent = reader.GetBoolean(3);
                    if (getIsReviseContent == false)
                    {
                        ViewBag.IsReviseContent = "";
                    }
                    else
                    {
                        ViewBag.IsReviseContent = "checked";
                    }
                }
                if (reader.IsDBNull(4) == false)
                {
                    ViewBag.ReviseContent = reader.GetString(4);
                }
                if (reader.IsDBNull(5) == false)
                {
                    bool getIsReviseDataStructure = reader.GetBoolean(5);
                    if (getIsReviseDataStructure == false)
                    {
                        ViewBag.IsReviseDataStructure = "";
                    }
                    else
                    {
                        ViewBag.IsReviseDataStructure = "checked";
                    }
                }
                if (reader.IsDBNull(6) == false)
                {
                    ViewBag.ReviseDataStructure = reader.GetString(6);
                }
                if (reader.IsDBNull(7) == false)
                {
                    bool getField_1until10 = reader.GetBoolean(7);
                    if (getField_1until10 == false)
                    {
                        ViewBag.Field_1until10 = "";
                    }
                    else
                    {
                        ViewBag.Field_1until10 = "checked";
                    }
                }
                if (reader.IsDBNull(8) == false)
                {
                    bool getField_11until20 = reader.GetBoolean(8);
                    if (getField_11until20 == false)
                    {
                        ViewBag.Field_11until20 = "";
                    }
                    else
                    {
                        ViewBag.Field_11until20 = "checked";
                    }
                }
                if (reader.IsDBNull(9) == false)
                {
                    bool getField_21until30 = reader.GetBoolean(9);
                    if (getField_21until30 == false)
                    {
                        ViewBag.Field_21until30 = "";
                    }
                    else
                    {
                        ViewBag.Field_21until30 = "checked";
                    }
                }
                if (reader.IsDBNull(10) == false)
                {
                    ViewBag.AmendmentCharges = reader.GetString(10);
                }
            }

            cn.Close();
        }



        return View();
    }









    public ActionResult CreateNewProgram(ITO_NewProgram iTO_NewProgram, string id, string Set, string Activities, string Duration, string Charges, string ProgramType)
    {
        var IdentityName = @Session["Fullname"];

        var Customer_Name = Session["Customer_Name"];
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.IsRole = @Session["Role"];
        string Deptment = @Session["Department"].ToString();
        ViewBag.Id = id;


        if (id.ToString() != null && Set == "save")
        {

            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                Guid guidId = Guid.NewGuid();
                cn2.Open();
                SqlCommand command;
                command = new SqlCommand("INSERT INTO [dbo].[ITO_NewProgram] (Id,ProgDevWorksheetId,Activities,Duration,Charges) values (@Id,@ProgDevWorksheetId,@Activities,@Duration,@Charges)", cn2);
                command.Parameters.AddWithValue("@Id", guidId);
                command.Parameters.AddWithValue("@ProgDevWorksheetId", id);
                command.Parameters.AddWithValue("@Activities", Activities);
                command.Parameters.AddWithValue("@Duration", Duration);
                command.Parameters.AddWithValue("@Charges", Charges);
                command.ExecuteNonQuery();
                cn2.Close();

            }

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("UPDATE [dbo].[ProgDevWorksheet] SET ProgramType=@ProgramType WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@ProgramType", "Amendment");
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
                cn.Close();
            }

            return RedirectToAction("CreateProgDevWorksheet", "ITO", new { Id = id.ToString() });
        }

        if (Set == "back")
        {
            return RedirectToAction("CreateProgDevWorksheet", "ITO", new { Id = id.ToString() });
        }

        return View();
    }

    public ActionResult ReloadNewProgram()
    {
        List<ITO_NewProgram> viewNewProgramList = new List<ITO_NewProgram>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT Activities,Duration,Charges, Id
                                      FROM [dbo].[ITO_NewProgram]  
                                      WHERE ProgDevWorksheetId=@Id";
            command.Parameters.AddWithValue("@Id", Session["ProgDevWorksheetId"].ToString());
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                ITO_NewProgram model = new ITO_NewProgram();
                {
                    model.Bil = _bil++;
                    if (reader.IsDBNull(0) == false)
                    {
                        model.Activities = reader.GetString(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        model.Duration = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        model.Charges = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        model.Id = reader.GetGuid(3);
                    }

                }
                viewNewProgramList.Add(model);
            }
            cn.Close();
            //return Json(new { data = viewFileStore }, JsonRequestBehavior.AllowGet);
            return Json(viewNewProgramList);
        }
    }

    public ActionResult DeleteNewProgram(string Id, string ProgDevWorksheetId)
    {
        Guid ITO_NewProgramId = Guid.Empty;

        if (Id != null)
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Activities,Duration,Charges, Id
                                          FROM [dbo].[ITO_NewProgram]
                                          WHERE Id=@Id";
                command.Parameters.AddWithValue("@Id", Id.ToString());
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.IsDBNull(0) == false)
                    {

                        using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            cn3.Open();
                            SqlCommand command3;
                            command3 = new SqlCommand("DELETE [dbo].[ITO_NewProgram] WHERE Id=@Id", cn3);
                            command3.Parameters.AddWithValue("@Id", Id);
                            command3.ExecuteNonQuery();
                            cn3.Close();

                        }

                    }

                    if (reader.IsDBNull(3) == false)
                    {
                        ITO_NewProgramId = reader.GetGuid(3);
                        return RedirectToAction("CreateProgDevWorksheet", "ITO", new { Id = Session["Id"].ToString() });
                    }
                }
                cn.Close();
            }
        }

        return RedirectToAction("CreateProgDevWorksheet", "ITO", new { Id = Session["Id"].ToString() });
    }



    [ValidateInput(false)]
    public ActionResult DevelopmentComplete(ProgDevWorksheet ProgDevWorksheet, string set, string Id, string JobInstructionId, string Customer_Name, string ProductName, string JobClass, string JobSheetNo, string JobRequest, string SalesExecutiveBy, string Status, string Complexity,
                                            string Paper, string MainProgramId, string ProgramId, string ProgramDesc, string TypeOfData, string StartDevOn, string CompleteDevOn, string ReasonDev,
                                            string ProgramType, string IsDedup, string Dedup, string IsSplitting, string Splitting, string IsRestructuring, string Restructuring, string Charges, string TotalCharges)
    {
        if (set == "submit")
        {
            if (!string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(ProgramId) && !string.IsNullOrEmpty(ProgramDesc) && !string.IsNullOrEmpty(TypeOfData) && !string.IsNullOrEmpty(StartDevOn) && !string.IsNullOrEmpty(CompleteDevOn) && set == "submit")

            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn.Open();
                    SqlCommand command;
                    command = new SqlCommand("UPDATE [dbo].[ProgDevWorksheet]  SET STATUS='Development Complete' WHERE Id=@Id", cn);
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                    cn.Close();

                    using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        ViewBag.JobInstructionId = JobInstructionId;
                        cn3.Open();
                        SqlCommand command1;
                        command1 = new SqlCommand("UPDATE [dbo].[JobInstruction] SET STATUS='Development Complete' WHERE Id=@Id", cn3);
                        command1.Parameters.AddWithValue("@Id", JobInstructionId);
                        command1.ExecuteNonQuery();
                        cn3.Close();

                        TempData["msg"] = "<script>alert('JI SUCCESSFULLY BACK TO MBD !');</script>";
                        return RedirectToAction("ManageProgDevWorksheet", "ITO");

                    }
                }
            }
            else
            {
                TempData["msg"] = "<script>alert('Development Process Not Complete!');</script>";
            }

        }

        return RedirectToAction("ManageProgDevWorksheet", "ITO");

    }

    List<ProgDevWorksheet> ProgDevWorksheetList = new List<ProgDevWorksheet>();
    public ActionResult CompleteDevJI(string Id, string product, string Customer_Name, string ProductName, string JobClass, string JobSheetNo, string JobRequest, string set, string SalesExecutiveBy, string Status, string Complexity, ProgDevWorksheet get,
                                       string IsReviseTemplate, string ReviseTemplate, string IsReviseContent, string ReviseContent, string IsReviseDataStructure, string ReviseDataStructure,
                                       string Field_1until10, string Field_11until20, string Field_21until30, string AmendmentCharges)
    {
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.IsRole = @Session["Role"];
        var IdentityName = @Session["Fullname"];

        if (set == "search")
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Id,Customer_Name,ProductName,JobClass,JobSheetNo,JobRequest,
                                        SalesExecutiveBy,Status,Complexity,StartDevOn,CompleteDevOn,
                                        MainProgramId,ProgramId,ProgramDesc,TypeOfData,ReasonDev,
                                        ProgramType,IsDedup,Dedup,IsSplitting,Splitting,IsRestructuring,
                                        Restructuring,Charges,TotalCharges,JobInstructionId,ProgrammerBy,up_1,up_2,
                                        IsReviseTemplate,ReviseTemplate,IsReviseContent,ReviseContent,IsReviseDataStructure,ReviseDataStructure,
                                        Field_1until10,Field_11until20,Field_21until30, AmendmentCharges
                                        FROM [dbo].[ProgDevWorksheet] 
                                        WHERE ProductName LIKE @ProductName
                                        AND Status = 'DEVELOPMENT COMPLETE'
                                        ORDER BY CreatedOn desc";
                command.Parameters.AddWithValue("@ProductName", "%" + product + "%");
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ProgDevWorksheet model = new ProgDevWorksheet();
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
                            model.JobSheetNo = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            model.JobRequest = reader.GetDateTime(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.SalesExecutiveBy = reader.GetString(6);
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.Status = reader.GetString(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.Complexity = reader.GetString(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            model.StartDevOn = reader.GetDateTime(9);
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            model.CompleteDevOn = reader.GetDateTime(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            model.MainProgramId = reader.GetString(11);
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            model.ProgramId = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            model.ProgramDesc = reader.GetString(13);
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            model.TypeOfData = reader.GetString(14);
                        }
                        if (reader.IsDBNull(15) == false)
                        {
                            model.ReasonDev = reader.GetString(15);
                        }
                        if (reader.IsDBNull(16) == false)
                        {
                            model.ProgramType = reader.GetString(16);
                        }
                        if (reader.IsDBNull(17) == false)
                        {
                            model.IsDedup = reader.GetBoolean(17);
                        }
                        if (reader.IsDBNull(18) == false)
                        {
                            model.Dedup = reader.GetString(18);
                        }
                        if (reader.IsDBNull(19) == false)
                        {
                            model.IsSplitting = reader.GetBoolean(19);
                        }
                        if (reader.IsDBNull(20) == false)
                        {
                            model.Splitting = reader.GetString(20);
                        }
                        if (reader.IsDBNull(21) == false)
                        {
                            model.IsRestructuring = reader.GetBoolean(21);
                        }
                        if (reader.IsDBNull(22) == false)
                        {
                            model.Restructuring = reader.GetString(22);
                        }
                        if (reader.IsDBNull(23) == false)
                        {
                            model.Charges = reader.GetString(23);
                        }
                        if (reader.IsDBNull(24) == false)
                        {
                            model.TotalCharges = reader.GetString(24);
                        }
                        if (reader.IsDBNull(25) == false)
                        {
                            model.JobInstructionId = reader.GetGuid(25);
                        }
                        if (reader.IsDBNull(26) == false)
                        {
                            model.ProgrammerBy = reader.GetString(26);
                        }
                        if (reader.IsDBNull(27) == false)
                        {
                            model.up_1 = reader.GetBoolean(27);
                        }
                        if (reader.IsDBNull(28) == false)
                        {
                            model.up_2 = reader.GetBoolean(28);
                        }
                        if (reader.IsDBNull(29) == false)
                        {
                            model.IsReviseTemplate = reader.GetBoolean(29);
                        }
                        if (reader.IsDBNull(30) == false)
                        {
                            model.ReviseTemplate = reader.GetString(30);
                        }
                        if (reader.IsDBNull(31) == false)
                        {
                            model.IsReviseContent = reader.GetBoolean(31);
                        }
                        if (reader.IsDBNull(32) == false)
                        {
                            model.ReviseContent = reader.GetString(32);
                        }
                        if (reader.IsDBNull(33) == false)
                        {
                            model.IsReviseDataStructure = reader.GetBoolean(33);
                        }
                        if (reader.IsDBNull(34) == false)
                        {
                            model.ReviseDataStructure = reader.GetString(34);
                        }
                        if (reader.IsDBNull(35) == false)
                        {
                            model.Field_1until10 = reader.GetBoolean(35);
                        }
                        if (reader.IsDBNull(36) == false)
                        {
                            model.Field_11until20 = reader.GetBoolean(36);
                        }
                        if (reader.IsDBNull(37) == false)
                        {
                            model.Field_21until30 = reader.GetBoolean(37);
                        }
                        if (reader.IsDBNull(38) == false)
                        {
                            model.AmendmentCharges = reader.GetString(38);
                        }

                    }
                    ProgDevWorksheetList.Add(model);
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
                command.CommandText = @"SELECT Id,Customer_Name,ProductName,JobClass,JobSheetNo,JobRequest,
                                        SalesExecutiveBy,Status,Complexity,StartDevOn,CompleteDevOn,
                                        MainProgramId,ProgramId,ProgramDesc,TypeOfData,ReasonDev,
                                        ProgramType,IsDedup,Dedup,IsSplitting,Splitting,IsRestructuring,
                                        Restructuring,Charges,TotalCharges,JobInstructionId,ProgrammerBy,up_1,up_2,
                                        IsReviseTemplate,ReviseTemplate,IsReviseContent,ReviseContent,IsReviseDataStructure,ReviseDataStructure,
                                        Field_1until10,Field_11until20,Field_21until30, AmendmentCharges
                                        FROM [dbo].[ProgDevWorksheet] 
                                        WHERE Status = 'DEVELOPMENT COMPLETE'
                                        ORDER BY CreatedOn desc";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ProgDevWorksheet model = new ProgDevWorksheet();
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
                            model.JobSheetNo = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            model.JobRequest = reader.GetDateTime(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.SalesExecutiveBy = reader.GetString(6);
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.Status = reader.GetString(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.Complexity = reader.GetString(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            model.StartDevOn = reader.GetDateTime(9);
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            model.CompleteDevOn = reader.GetDateTime(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            model.MainProgramId = reader.GetString(11);
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            model.ProgramId = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            model.ProgramDesc = reader.GetString(13);
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            model.TypeOfData = reader.GetString(14);
                        }
                        if (reader.IsDBNull(15) == false)
                        {
                            model.ReasonDev = reader.GetString(15);
                        }
                        if (reader.IsDBNull(16) == false)
                        {
                            model.ProgramType = reader.GetString(16);
                        }
                        if (reader.IsDBNull(17) == false)
                        {
                            model.IsDedup = reader.GetBoolean(17);
                        }
                        if (reader.IsDBNull(18) == false)
                        {
                            model.Dedup = reader.GetString(18);
                        }
                        if (reader.IsDBNull(19) == false)
                        {
                            model.IsSplitting = reader.GetBoolean(19);
                        }
                        if (reader.IsDBNull(20) == false)
                        {
                            model.Splitting = reader.GetString(20);
                        }
                        if (reader.IsDBNull(21) == false)
                        {
                            model.IsRestructuring = reader.GetBoolean(21);
                        }
                        if (reader.IsDBNull(22) == false)
                        {
                            model.Restructuring = reader.GetString(22);
                        }
                        if (reader.IsDBNull(23) == false)
                        {
                            model.Charges = reader.GetString(23);
                        }
                        if (reader.IsDBNull(24) == false)
                        {
                            model.TotalCharges = reader.GetString(24);
                        }
                        if (reader.IsDBNull(25) == false)
                        {
                            model.JobInstructionId = reader.GetGuid(25);
                        }
                        if (reader.IsDBNull(26) == false)
                        {
                            model.ProgrammerBy = reader.GetString(26);
                        }
                        if (reader.IsDBNull(27) == false)
                        {
                            model.up_1 = reader.GetBoolean(27);
                        }
                        if (reader.IsDBNull(28) == false)
                        {
                            model.up_2 = reader.GetBoolean(28);
                        }
                        if (reader.IsDBNull(29) == false)
                        {
                            model.IsReviseTemplate = reader.GetBoolean(29);
                        }
                        if (reader.IsDBNull(30) == false)
                        {
                            model.ReviseTemplate = reader.GetString(30);
                        }
                        if (reader.IsDBNull(31) == false)
                        {
                            model.IsReviseContent = reader.GetBoolean(31);
                        }
                        if (reader.IsDBNull(32) == false)
                        {
                            model.ReviseContent = reader.GetString(32);
                        }
                        if (reader.IsDBNull(33) == false)
                        {
                            model.IsReviseDataStructure = reader.GetBoolean(33);
                        }
                        if (reader.IsDBNull(34) == false)
                        {
                            model.ReviseDataStructure = reader.GetString(34);
                        }
                        if (reader.IsDBNull(35) == false)
                        {
                            model.Field_1until10 = reader.GetBoolean(35);
                        }
                        if (reader.IsDBNull(36) == false)
                        {
                            model.Field_11until20 = reader.GetBoolean(36);
                        }
                        if (reader.IsDBNull(37) == false)
                        {
                            model.Field_21until30 = reader.GetBoolean(37);
                        }
                        if (reader.IsDBNull(38) == false)
                        {
                            model.AmendmentCharges = reader.GetString(38);
                        }
                    }
                    ProgDevWorksheetList.Add(model);
                }
                cn.Close();
            }
        }


        return View(ProgDevWorksheetList);

    }


    public ActionResult getExcelDevWorksheet(string Id, string JobInstructionId, string set, string Customer_Name, string ProductName, string JobClass, string JobSheetNo, string JobRequest, string SalesExecutiveBy, string Status, string Complexity,
                                             ProgDevWorksheet get, string Paper, string MainProgramId, string ProgramId, string ProgramDesc, string TypeOfData, string StartDevOn, string CompleteDevOn, string ReasonDev,
                                             string ProgramType, string IsDedup, string Dedup, string IsSplitting, string Splitting, string IsRestructuring, string Restructuring, string Charges, string TotalCharges, string up_1, string up_2,
                                             string IsReviseTemplate, string ReviseTemplate, string IsReviseContent, string ReviseContent, string IsReviseDataStructure, string ReviseDataStructure,
                                             string Field_1until10, string Field_11until20, string Field_21until30, string AmendmentCharges)
    {

        ViewBag.IsDepart = @Session["Department"];
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        {

            List<ProgDevWorksheet> gotlist = new List<ProgDevWorksheet>();
            cn.Open();
            SqlCommand command;
            command = new SqlCommand(@"SELECT Id,Customer_Name,ProductName,Paper,up_1,up_2,JobClass, CreateUser, SalesExecutiveBy,
                                              JobRequest,Complexity,StartDevOn,CompleteDevOn,
                                              MainProgramId,ProgramId,ProgramDesc,TypeOfData,ReasonDev,
                                              ProgramType,IsDedup,Dedup,IsSplitting,Splitting,IsRestructuring,
                                              Restructuring,Charges,TotalCharges,ProgrammerBy,
                                              IsReviseTemplate,ReviseTemplate,IsReviseContent,ReviseContent,IsReviseDataStructure,ReviseDataStructure,
                                              Field_1until10,Field_11until20,Field_21until30, AmendmentCharges
                                              FROM [dbo].[ProgDevWorksheet]
                                              WHERE Id = @Id", cn);
            command.Parameters.AddWithValue("@Id", Id.ToString());
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                ProgDevWorksheet list = new ProgDevWorksheet();
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
                        list.Paper = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        list.up_1 = reader.GetBoolean(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        list.up_2 = reader.GetBoolean(5);
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        list.JobClass = reader.GetString(6);
                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        list.CreateUser = reader.GetString(7);
                    }
                    if (reader.IsDBNull(8) == false)
                    {
                        list.SalesExecutiveBy = reader.GetString(8);
                    }
                    if (reader.IsDBNull(9) == false)
                    {
                        list.JobRequest = reader.GetDateTime(9);
                    }
                    if (reader.IsDBNull(10) == false)
                    {
                        list.Complexity = reader.GetString(10);
                    }
                    if (reader.IsDBNull(11) == false)
                    {
                        list.StartDevOn = reader.GetDateTime(11);
                    }
                    if (reader.IsDBNull(12) == false)
                    {
                        list.CompleteDevOn = reader.GetDateTime(12);
                    }
                    if (reader.IsDBNull(13) == false)
                    {
                        list.MainProgramId = reader.GetString(13);
                    }
                    if (reader.IsDBNull(14) == false)
                    {
                        list.ProgramId = reader.GetString(14);
                    }
                    if (reader.IsDBNull(15) == false)
                    {
                        list.ProgramDesc = reader.GetString(15);
                    }
                    if (reader.IsDBNull(16) == false)
                    {
                        list.TypeOfData = reader.GetString(16);
                    }
                    if (reader.IsDBNull(17) == false)
                    {
                        list.ReasonDev = reader.GetString(17);
                    }
                    if (reader.IsDBNull(18) == false)
                    {
                        list.ProgramType = reader.GetString(18);
                    }
                    if (reader.IsDBNull(19) == false)
                    {
                        list.IsDedup = reader.GetBoolean(19);
                    }
                    if (reader.IsDBNull(20) == false)
                    {
                        list.Dedup = reader.GetString(20);
                    }
                    if (reader.IsDBNull(21) == false)
                    {
                        list.IsSplitting = reader.GetBoolean(21);
                    }
                    if (reader.IsDBNull(22) == false)
                    {
                        list.Splitting = reader.GetString(22);
                    }
                    if (reader.IsDBNull(23) == false)
                    {
                        list.IsRestructuring = reader.GetBoolean(23);
                    }
                    if (reader.IsDBNull(24) == false)
                    {
                        list.Restructuring = reader.GetString(24);
                    }
                    if (reader.IsDBNull(25) == false)
                    {
                        list.Charges = reader.GetString(25);
                    }
                    if (reader.IsDBNull(26) == false)
                    {
                        list.TotalCharges = reader.GetString(26);
                    }
                    if (reader.IsDBNull(27) == false)
                    {
                        list.ProgrammerBy = reader.GetString(27);
                    }
                    if (reader.IsDBNull(28) == false)
                    {
                        list.IsReviseTemplate = reader.GetBoolean(28);
                    }
                    if (reader.IsDBNull(29) == false)
                    {
                        list.ReviseTemplate = reader.GetString(29);
                    }
                    if (reader.IsDBNull(30) == false)
                    {
                        list.IsReviseContent = reader.GetBoolean(30);
                    }
                    if (reader.IsDBNull(31) == false)
                    {
                        list.ReviseContent = reader.GetString(31);
                    }
                    if (reader.IsDBNull(32) == false)
                    {
                        list.IsReviseDataStructure = reader.GetBoolean(32);
                    }
                    if (reader.IsDBNull(33) == false)
                    {
                        list.ReviseDataStructure = reader.GetString(33);
                    }
                    if (reader.IsDBNull(34) == false)
                    {
                        list.Field_1until10 = reader.GetBoolean(34);
                    }
                    if (reader.IsDBNull(35) == false)
                    {
                        list.Field_11until20 = reader.GetBoolean(35);
                    }
                    if (reader.IsDBNull(36) == false)
                    {
                        list.Field_21until30 = reader.GetBoolean(36);
                    }
                    if (reader.IsDBNull(37) == false)
                    {
                        list.AmendmentCharges = reader.GetString(37);
                    }

                }
                gotlist.Add(list);

            }
            cn.Close();

            ViewBag.Id = Id;

            List<ITO_NewProgram> gotlist2 = new List<ITO_NewProgram>();
            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn2.Open();
                SqlCommand command2;
                command2 = new SqlCommand(@"SELECT Activities,Duration,Charges, Id
                                            FROM [dbo].[ITO_NewProgram]
                                            WHERE Id = @ProgDevWorksheetId", cn2);
                command2.Parameters.AddWithValue("@ProgDevWorksheetId", Id.ToString());
                var reader2 = command2.ExecuteReader();
                while (reader2.Read())
                {
                    ITO_NewProgram list = new ITO_NewProgram();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            list.Activities = reader.GetString(0);
                        }
                        if (reader2.IsDBNull(1) == false)
                        {
                            list.Duration = reader.GetString(1);
                        }
                        if (reader2.IsDBNull(2) == false)
                        {
                            list.Charges = reader.GetString(2);
                        }
                        if (reader2.IsDBNull(3) == false)
                        {
                            list.Id = reader.GetGuid(3);
                        }

                    }
                    gotlist2.Add(list);
                }
                cn.Close();

            }


            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("WORKSHEET 1");
            workSheet.TabColor = System.Drawing.Color.Black;

            workSheet.DefaultRowHeight = 12;
            workSheet.Row(1).Height = 20;
            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(1).Style.Font.Bold = true;
            workSheet.Cells[1, 1].Value = "CUSTOMER";
            workSheet.Cells[1, 2].Value = "PRODUCT NAME";
            workSheet.Cells[1, 3].Value = "PAPER";
            workSheet.Cells[1, 4].Value = "1 UP";
            workSheet.Cells[1, 5].Value = "2 UP";
            workSheet.Cells[1, 6].Value = "PIC";
            workSheet.Cells[1, 7].Value = "FREQUENCY";
            workSheet.Cells[1, 8].Value = "FROM";
            workSheet.Cells[1, 9].Value = "DATE";
            workSheet.Cells[1, 10].Value = "PROGRAM ID";
            workSheet.Cells[1, 11].Value = "PROGRAM DESC";
            workSheet.Cells[1, 12].Value = "TYPE OF DATA";
            workSheet.Cells[1, 13].Value = "DATE START";
            workSheet.Cells[1, 14].Value = "DATE COMPLETE";
            workSheet.Cells[1, 15].Value = "IS DEDUP";
            workSheet.Cells[1, 16].Value = "DEDUP";
            workSheet.Cells[1, 17].Value = "IS SPLITTING";
            workSheet.Cells[1, 18].Value = "SPLITTING";
            workSheet.Cells[1, 19].Value = "IS RESTRUCTURING";
            workSheet.Cells[1, 20].Value = "RESTRUCTURING";
            workSheet.Cells[1, 21].Value = "CHARGES";
            workSheet.Cells[1, 22].Value = "TOTAL CHARGES";


            int recordIndex = 2;
            foreach (var CLM in gotlist)
            {
                workSheet.Cells[recordIndex, 1].Value = CLM.Customer_Name;
                workSheet.Cells[recordIndex, 2].Value = CLM.ProductName;
                workSheet.Cells[recordIndex, 3].Value = CLM.Paper;
                workSheet.Cells[recordIndex, 4].Value = CLM.up_1;
                workSheet.Cells[recordIndex, 5].Value = CLM.up_2;
                workSheet.Cells[recordIndex, 6].Value = CLM.CreateUser;
                workSheet.Cells[recordIndex, 7].Value = CLM.JobClass;
                workSheet.Cells[recordIndex, 8].Value = CLM.SalesExecutiveBy;
                workSheet.Cells[recordIndex, 9].Value = CLM.JobRequest;
                workSheet.Cells[recordIndex, 10].Value = CLM.ProgramId;
                workSheet.Cells[recordIndex, 11].Value = CLM.ProgramDesc;
                workSheet.Cells[recordIndex, 12].Value = CLM.TypeOfData;
                workSheet.Cells[recordIndex, 13].Value = CLM.StartDevOn;
                workSheet.Cells[recordIndex, 14].Value = CLM.CompleteDevOn;
                workSheet.Cells[recordIndex, 15].Value = CLM.IsDedup;
                workSheet.Cells[recordIndex, 16].Value = CLM.Dedup;
                workSheet.Cells[recordIndex, 17].Value = CLM.IsSplitting;
                workSheet.Cells[recordIndex, 18].Value = CLM.Splitting;
                workSheet.Cells[recordIndex, 19].Value = CLM.IsRestructuring;
                workSheet.Cells[recordIndex, 20].Value = CLM.Restructuring;
                workSheet.Cells[recordIndex, 21].Value = CLM.Charges;
                workSheet.Cells[recordIndex, 22].Value = CLM.TotalCharges;


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
            workSheet.Column(18).AutoFit();
            workSheet.Column(19).AutoFit();
            workSheet.Column(20).AutoFit();
            workSheet.Column(21).AutoFit();
            workSheet.Column(22).AutoFit();



            string excelName = "Program Dev Worksheet  -" + ProductName;
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


        return RedirectToAction("CompleteDevJI", "ITO");
    }



    public ActionResult PrintProgDevWorkSheet(string Id)
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
            command.CommandText = @"SELECT Id,Customer_Name,ProductName,Paper,up_1,up_2,JobClass, CreateUser, SalesExecutiveBy,
                                           JobRequest,Complexity,StartDevOn,CompleteDevOn,
                                           MainProgramId,ProgramId,ProgramDesc,TypeOfData,ReasonDev,
                                           ProgramType,IsDedup,Dedup,IsSplitting,Splitting,IsRestructuring,
                                           Restructuring,Charges,TotalCharges,ProgrammerBy,
                                           IsReviseTemplate,ReviseTemplate,IsReviseContent,ReviseContent,IsReviseDataStructure,ReviseDataStructure,
                                           Field_1until10,Field_11until20,Field_21until30, AmendmentCharges
                                   FROM [dbo].[ProgDevWorksheet]    
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
                    ViewBag.Paper = reader.GetString(3);
                }
                if (reader.IsDBNull(4) == false)
                {
                    ViewBag.up_1 = reader.GetBoolean(4);
                }
                if (reader.IsDBNull(5) == false)
                {
                    ViewBag.up_2 = reader.GetBoolean(5);
                }
                if (reader.IsDBNull(6) == false)
                {
                    ViewBag.JobClass = reader.GetString(6);
                }
                if (reader.IsDBNull(7) == false)
                {
                    ViewBag.CreateUser = reader.GetString(7);
                }
                if (reader.IsDBNull(8) == false)
                {
                    ViewBag.SalesExecutiveBy = reader.GetString(8);
                }
                if (reader.IsDBNull(9) == false)
                {
                    ViewBag.JobRequest = reader.GetDateTime(9);
                }
                if (reader.IsDBNull(10) == false)
                {
                    ViewBag.Complexity = reader.GetString(10);
                }
                if (reader.IsDBNull(11) == false)
                {
                    ViewBag.StartDevOn = reader.GetDateTime(11);
                }
                if (reader.IsDBNull(12) == false)
                {
                    ViewBag.CompleteDevOn = reader.GetDateTime(12);
                }
                if (reader.IsDBNull(13) == false)
                {
                    ViewBag.MainProgramId = reader.GetString(13);
                }
                if (reader.IsDBNull(14) == false)
                {
                    ViewBag.ProgramId = reader.GetString(14);
                }
                if (reader.IsDBNull(15) == false)
                {
                    ViewBag.ProgramDesc = reader.GetString(15);
                }
                if (reader.IsDBNull(16) == false)
                {
                    ViewBag.TypeOfData = reader.GetString(16);
                }
                if (reader.IsDBNull(17) == false)
                {
                    ViewBag.ReasonDev = reader.GetString(17);
                }
                if (reader.IsDBNull(18) == false)
                {
                    ViewBag.ProgramType = reader.GetString(18);
                }
                if (reader.IsDBNull(19) == false)
                {
                    ViewBag.IsDedup = reader.GetBoolean(19);
                }
                if (reader.IsDBNull(20) == false)
                {
                    ViewBag.Dedup = reader.GetString(20);
                }
                if (reader.IsDBNull(21) == false)
                {
                    ViewBag.IsSplitting = reader.GetBoolean(21);
                }
                if (reader.IsDBNull(22) == false)
                {
                    ViewBag.Splitting = reader.GetString(22);
                }
                if (reader.IsDBNull(23) == false)
                {
                    ViewBag.IsRestructuring = reader.GetBoolean(23);
                }
                if (reader.IsDBNull(24) == false)
                {
                    ViewBag.Restructuring = reader.GetString(24);
                }
                if (reader.IsDBNull(25) == false)
                {
                    ViewBag.Charges = reader.GetString(25);
                }
                if (reader.IsDBNull(26) == false)
                {
                    ViewBag.TotalCharges = reader.GetString(26);
                }
                if (reader.IsDBNull(27) == false)
                {
                    ViewBag.ProgrammerBy = reader.GetString(27);
                }
                if (reader.IsDBNull(28) == false)
                {
                    ViewBag.IsReviseTemplate = reader.GetBoolean(28);
                }
                if (reader.IsDBNull(29) == false)
                {
                    ViewBag.ReviseTemplate = reader.GetString(29);
                }
                if (reader.IsDBNull(30) == false)
                {
                    ViewBag.IsReviseContent = reader.GetBoolean(30);
                }
                if (reader.IsDBNull(31) == false)
                {
                    ViewBag.ReviseContent = reader.GetString(31);
                }
                if (reader.IsDBNull(32) == false)
                {
                    ViewBag.IsReviseDataStructure = reader.GetBoolean(32);
                }
                if (reader.IsDBNull(33) == false)
                {
                    ViewBag.ReviseDataStructure = reader.GetString(33);
                }
                if (reader.IsDBNull(34) == false)
                {
                    ViewBag.Field_1until10 = reader.GetBoolean(34);
                }
                if (reader.IsDBNull(35) == false)
                {
                    ViewBag.Field_11until20 = reader.GetBoolean(35);
                }
                if (reader.IsDBNull(36) == false)
                {
                    ViewBag.Field_21until30 = reader.GetBoolean(36);
                }
                if (reader.IsDBNull(37) == false)
                {
                    ViewBag.AmendmentCharges = reader.GetString(37);
                }

            }
            cn.Close();
        }

        //call table

        List<ProgDevWorksheet> viewProgDevWorksheet = new List<ProgDevWorksheet>();
        using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn2))
        {
            int _bil = 1;
            cn2.Open();
            command.CommandText = @"SELECT Id,Customer_Name,ProductName,Paper,up_1,up_2,JobClass, CreateUser, SalesExecutiveBy,
                                           JobRequest,Complexity,StartDevOn,CompleteDevOn,
                                           MainProgramId,ProgramId,ProgramDesc,TypeOfData,ReasonDev,
                                           ProgramType,IsDedup,Dedup,IsSplitting,Splitting,IsRestructuring,
                                           Restructuring,Charges,TotalCharges,ProgrammerBy,
                                           IsReviseTemplate,ReviseTemplate,IsReviseContent,ReviseContent,IsReviseDataStructure,ReviseDataStructure,
                                           Field_1until10,Field_11until20,Field_21until30, AmendmentCharges
                                           FROM [dbo].[ProgDevWorksheet]    
                                           WHERE Id=@Id
                                           ORDER BY CreatedOn";
            command.Parameters.AddWithValue("@Id", Session["Id"].ToString());
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                ProgDevWorksheet model = new ProgDevWorksheet();
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
                        model.Paper = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        model.up_1 = reader.GetBoolean(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        model.up_2 = reader.GetBoolean(5);
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        model.JobClass = reader.GetString(6);
                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        model.CreateUser = reader.GetString(7);
                    }
                    if (reader.IsDBNull(8) == false)
                    {
                        model.SalesExecutiveBy = reader.GetString(8);
                    }
                    if (reader.IsDBNull(9) == false)
                    {
                        model.JobRequest = reader.GetDateTime(9);
                    }
                    if (reader.IsDBNull(10) == false)
                    {
                        model.Complexity = reader.GetString(10);
                    }
                    if (reader.IsDBNull(11) == false)
                    {
                        model.StartDevOn = reader.GetDateTime(11);
                    }
                    if (reader.IsDBNull(12) == false)
                    {
                        model.CompleteDevOn = reader.GetDateTime(12);
                    }
                    if (reader.IsDBNull(13) == false)
                    {
                        model.MainProgramId = reader.GetString(13);
                    }
                    if (reader.IsDBNull(14) == false)
                    {
                        model.ProgramId = reader.GetString(14);
                    }
                    if (reader.IsDBNull(15) == false)
                    {
                        model.ProgramDesc = reader.GetString(15);
                    }
                    if (reader.IsDBNull(16) == false)
                    {
                        model.TypeOfData = reader.GetString(16);
                    }
                    if (reader.IsDBNull(17) == false)
                    {
                        model.ReasonDev = reader.GetString(17);
                    }
                    if (reader.IsDBNull(18) == false)
                    {
                        model.ProgramType = reader.GetString(18);
                    }
                    if (reader.IsDBNull(19) == false)
                    {
                        model.IsDedup = reader.GetBoolean(19);
                    }
                    if (reader.IsDBNull(20) == false)
                    {
                        model.Dedup = reader.GetString(20);
                    }
                    if (reader.IsDBNull(21) == false)
                    {
                        model.IsSplitting = reader.GetBoolean(21);
                    }
                    if (reader.IsDBNull(22) == false)
                    {
                        model.Splitting = reader.GetString(22);
                    }
                    if (reader.IsDBNull(23) == false)
                    {
                        model.IsRestructuring = reader.GetBoolean(23);
                    }
                    if (reader.IsDBNull(24) == false)
                    {
                        model.Restructuring = reader.GetString(24);
                    }
                    if (reader.IsDBNull(25) == false)
                    {
                        model.Charges = reader.GetString(25);
                    }
                    if (reader.IsDBNull(26) == false)
                    {
                        model.TotalCharges = reader.GetString(26);
                    }
                    if (reader.IsDBNull(27) == false)
                    {
                        model.ProgrammerBy = reader.GetString(27);
                    }
                    if (reader.IsDBNull(28) == false)
                    {
                        model.IsReviseTemplate = reader.GetBoolean(28);
                    }
                    if (reader.IsDBNull(29) == false)
                    {
                        model.ReviseTemplate = reader.GetString(29);
                    }
                    if (reader.IsDBNull(30) == false)
                    {
                        model.IsReviseContent = reader.GetBoolean(30);
                    }
                    if (reader.IsDBNull(31) == false)
                    {
                        model.ReviseContent = reader.GetString(31);
                    }
                    if (reader.IsDBNull(32) == false)
                    {
                        model.IsReviseDataStructure = reader.GetBoolean(32);
                    }
                    if (reader.IsDBNull(33) == false)
                    {
                        model.ReviseDataStructure = reader.GetString(33);
                    }
                    if (reader.IsDBNull(34) == false)
                    {
                        model.Field_1until10 = reader.GetBoolean(34);
                    }
                    if (reader.IsDBNull(35) == false)
                    {
                        model.Field_11until20 = reader.GetBoolean(35);
                    }
                    if (reader.IsDBNull(36) == false)
                    {
                        model.Field_21until30 = reader.GetBoolean(36);
                    }
                    if (reader.IsDBNull(37) == false)
                    {
                        model.AmendmentCharges = reader.GetString(37);
                    }
                }
                viewProgDevWorksheet.Add(model);
            }
            cn2.Close();

        }



        using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn3))
        {
            cn3.Open();
            command.CommandText = @"SELECT Activities,Duration,Charges, ProgDevWorksheetId
                                           FROM [dbo].[ITO_NewProgram]
                                    WHERE ProgDevWorksheetId=@ProgDevWorksheetId";
            command.Parameters.AddWithValue("@ProgDevWorksheetId", Id);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader.IsDBNull(0) == false)
                {
                    ViewBag.Activities = reader.GetString(0);
                }
                if (reader.IsDBNull(1) == false)
                {
                    ViewBag.Duration = reader.GetString(1);
                }
                if (reader.IsDBNull(2) == false)
                {
                    ViewBag.Charges = reader.GetString(2);
                }
                if (reader.IsDBNull(3) == false)
                {
                    ViewBag.ProgDevWorksheetId = reader.GetGuid(3);
                }

            }
            cn3.Close();
        }

        //call table

        List<ITO_NewProgram> viewNewProgram = new List<ITO_NewProgram>();
        using (SqlConnection cn4 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        {
            cn4.Open();

            using (SqlCommand command = new SqlCommand("", cn4))
            {
                int _bil = 1;
                command.CommandText = @"SELECT Activities,Duration,Charges, ProgDevWorksheetId
                                           FROM [dbo].[ITO_NewProgram] 
                                           WHERE ProgDevWorksheetId=@ProgDevWorksheetId";
                command.Parameters.AddWithValue("@ProgDevWorksheetId", Session["Id"].ToString());
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ITO_NewProgram model = new ITO_NewProgram();
                    {
                        model.Bil = _bil++;
                        if (reader.IsDBNull(0) == false)
                        {
                            model.Activities = reader.GetString(0);
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            model.Duration = reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            model.Charges = reader.GetString(2);
                        }
                        if (reader.IsDBNull(3) == false)
                        {
                            model.ProgDevWorksheetId = reader.GetGuid(3);
                        }
                        viewNewProgram.Add(model);
                    }

                }
            }
            cn4.Close();


        }

        //-----------------------------------------

        ReloadWorksheetList(Id);

        return View(viewProgDevWorksheet);
        //return new Rotativa.ViewAsPdf("PrintProgDevWorkSheet", viewProgDevWorksheet)
        //{
        //    // FileName = flightPlan.ListingItemDetailsModel.FlightDetails + ".pdf",
        //    PageMargins = new Rotativa.Options.Margins(5, 5, 5, 5),
        //    PageOrientation = Rotativa.Options.Orientation.Portrait,
        //    //PageWidth = 210,
        //    //PageHeight = 297
        //};
    }

    List<ProgDevWorksheet> viewWorksheet = new List<ProgDevWorksheet>();
    private void ReloadWorksheetList(string Id)
    {
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {

            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT b.Id,b.Customer_Name,b.ProductName,b.Paper,b.up_1,b.up_2,b.JobClass, b.CreateUser, b.SalesExecutiveBy,
                                           b.JobRequest,b.Complexity,b.StartDevOn,b.CompleteDevOn,
                                           b.MainProgramId,b.ProgramId,b.ProgramDesc,b.TypeOfData,b.ReasonDev,
                                           b.ProgramType,b.IsDedup,b.Dedup,b.IsSplitting,b.Splitting,b.IsRestructuring,
                                           b.Restructuring,b.Charges,b.TotalCharges,b.ProgrammerBy,
                                           b.IsReviseTemplate,b.ReviseTemplate,b.IsReviseContent,b.ReviseContent,b.IsReviseDataStructure,b.ReviseDataStructure,
                                           b.Field_1until10,b.Field_11until20,b.Field_21until30, b.AmendmentCharges,
                                           a.Activities,a.Duration,a.Charges, a.ProgDevWorksheetId
                                           FROM [dbo].[ProgDevWorksheet] b, [dbo].[ITO_NewProgram] a
                                           WHERE a.ProgDevWorksheetId=b.Id AND b.Id=@Id ";
            command.Parameters.AddWithValue("@Id", Id);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                ProgDevWorksheet model = new ProgDevWorksheet();
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
                        model.Paper = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        model.up_1 = reader.GetBoolean(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        model.up_2 = reader.GetBoolean(5);
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        model.JobClass = reader.GetString(6);
                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        model.CreateUser = reader.GetString(7);
                    }
                    if (reader.IsDBNull(8) == false)
                    {
                        model.SalesExecutiveBy = reader.GetString(8);
                    }
                    if (reader.IsDBNull(9) == false)
                    {
                        model.JobRequest = reader.GetDateTime(9);
                    }
                    if (reader.IsDBNull(10) == false)
                    {
                        model.Complexity = reader.GetString(10);
                    }
                    if (reader.IsDBNull(11) == false)
                    {
                        model.StartDevOn = reader.GetDateTime(11);
                    }
                    if (reader.IsDBNull(12) == false)
                    {
                        model.CompleteDevOn = reader.GetDateTime(12);
                    }
                    if (reader.IsDBNull(13) == false)
                    {
                        model.MainProgramId = reader.GetString(13);
                    }
                    if (reader.IsDBNull(14) == false)
                    {
                        model.ProgramId = reader.GetString(14);
                    }
                    if (reader.IsDBNull(15) == false)
                    {
                        model.ProgramDesc = reader.GetString(15);
                    }
                    if (reader.IsDBNull(16) == false)
                    {
                        model.TypeOfData = reader.GetString(16);
                    }
                    if (reader.IsDBNull(17) == false)
                    {
                        model.ReasonDev = reader.GetString(17);
                    }
                    if (reader.IsDBNull(18) == false)
                    {
                        model.ProgramType = reader.GetString(18);
                    }
                    if (reader.IsDBNull(19) == false)
                    {
                        model.IsDedup = reader.GetBoolean(19);
                    }
                    if (reader.IsDBNull(20) == false)
                    {
                        model.Dedup = reader.GetString(20);
                    }
                    if (reader.IsDBNull(21) == false)
                    {
                        model.IsSplitting = reader.GetBoolean(21);
                    }
                    if (reader.IsDBNull(22) == false)
                    {
                        model.Splitting = reader.GetString(22);
                    }
                    if (reader.IsDBNull(23) == false)
                    {
                        model.IsRestructuring = reader.GetBoolean(23);
                    }
                    if (reader.IsDBNull(24) == false)
                    {
                        model.Restructuring = reader.GetString(24);
                    }
                    if (reader.IsDBNull(25) == false)
                    {
                        model.Charges = reader.GetString(25);
                    }
                    if (reader.IsDBNull(26) == false)
                    {
                        model.TotalCharges = reader.GetString(26);
                    }
                    if (reader.IsDBNull(27) == false)
                    {
                        model.ProgrammerBy = reader.GetString(27);
                    }
                    if (reader.IsDBNull(28) == false)
                    {
                        model.IsReviseTemplate = reader.GetBoolean(28);
                    }
                    if (reader.IsDBNull(29) == false)
                    {
                        model.ReviseTemplate = reader.GetString(29);
                    }
                    if (reader.IsDBNull(30) == false)
                    {
                        model.IsReviseContent = reader.GetBoolean(30);
                    }
                    if (reader.IsDBNull(31) == false)
                    {
                        model.ReviseContent = reader.GetString(31);
                    }
                    if (reader.IsDBNull(32) == false)
                    {
                        model.IsReviseDataStructure = reader.GetBoolean(32);
                    }
                    if (reader.IsDBNull(33) == false)
                    {
                        model.ReviseDataStructure = reader.GetString(33);
                    }
                    if (reader.IsDBNull(34) == false)
                    {
                        model.Field_1until10 = reader.GetBoolean(34);
                    }
                    if (reader.IsDBNull(35) == false)
                    {
                        model.Field_11until20 = reader.GetBoolean(35);
                    }
                    if (reader.IsDBNull(36) == false)
                    {
                        model.Field_21until30 = reader.GetBoolean(36);
                    }
                    if (reader.IsDBNull(37) == false)
                    {
                        model.AmendmentCharges = reader.GetString(37);
                    }
                    if (reader.IsDBNull(38) == false)
                    {
                        model.Activities = reader.GetString(38);
                    }
                    if (reader.IsDBNull(39) == false)
                    {
                        model.Duration = reader.GetString(39);
                    }
                    if (reader.IsDBNull(40) == false)
                    {
                        model.Charges = reader.GetString(40);
                    }
                    if (reader.IsDBNull(41) == false)
                    {
                        model.ProgDevWorksheetId = reader.GetGuid(41);
                    }
                }

                viewWorksheet.Add(model);
            }
            cn.Close();
        }
    }



    //public ActionResult ManageDailyJob(JobAuditTrail get, string Id, string Customer_Name, string ProductName, string JobClass, string JobSheetNo, string JobRequest, string JobType, string set, string Status,
    //                                   string AccountsQty, string ImpressionQty, string PagesQty, string Frequency, string JobInstructionId)
    //{
    //    ViewBag.IsDepart = @Session["Department"];
    //    ViewBag.IsRole = @Session["Role"];
    //    var IdentityName = @Session["Fullname"];



    //    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
    //    using (SqlCommand command = new SqlCommand("", cn))
    //    {
    //        int _bil = 1;
    //        cn.Open();
    //        command.CommandText = @"SELECT Id, ModifiedOn, Customer_Name, Cust_Department, ProductName,JobClass, 
    //                                           JobType,Status, AccountsQty,ImpressionQty, PagesQty,
    //                                           IT_SysNotes,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes,
    //                                           ArtworkNotes, Acc_BillingNotes, DCPNotes,JobSheetNo
    //                                           FROM [dbo].[JobInstruction] 
    //                                           WHERE (Status = 'ITO') AND (JobClass='DAILY') OR (Status = 'EXISTING JI') AND (JobClass='DAILY')";
    //        var reader = command.ExecuteReader();
    //        while (reader.Read())
    //        {
    //            DailyTracking model = new DailyTracking();
    //            {
    //                model.Bil = _bil++;
    //                if (reader.IsDBNull(0) == false)
    //                {
    //                    model.Id = reader.GetGuid(0);
    //                }
    //                if (reader.IsDBNull(1) == false)
    //                {
    //                    model.ModifiedOn = reader.GetDateTime(1);
    //                }
    //                if (reader.IsDBNull(2) == false)
    //                {
    //                    model.Customer_Name = reader.GetString(2);
    //                }
    //                if (reader.IsDBNull(3) == false)
    //                {
    //                    model.Cust_Department = reader.GetString(3);
    //                }
    //                if (reader.IsDBNull(4) == false)
    //                {
    //                    model.ProductName = reader.GetString(4);
    //                }
    //                if (reader.IsDBNull(5) == false)
    //                {
    //                    model.JobClass = reader.GetString(5);
    //                }
    //                if (reader.IsDBNull(6) == false)
    //                {
    //                    model.JobType = reader.GetString(6);
    //                }
    //                if (reader.IsDBNull(7) == false)
    //                {
    //                    model.Status = reader.GetString(7);
    //                }
    //                if (reader.IsDBNull(8) == false)
    //                {
    //                    model.AccountsQty = reader.GetString(8);
    //                }
    //                if (reader.IsDBNull(9) == false)
    //                {
    //                    model.ImpressionQty = reader.GetString(9);
    //                }
    //                if (reader.IsDBNull(10) == false)
    //                {
    //                    model.PagesQty = reader.GetString(10);
    //                }
    //                if (reader.IsDBNull(11) == false)
    //                {
    //                    model.IT_SysNotes = reader.GetString(11);
    //                }
    //                if (reader.IsDBNull(12) == false)
    //                {
    //                    model.Produc_PlanningNotes = reader.GetString(12);
    //                }
    //                if (reader.IsDBNull(13) == false)
    //                {
    //                    model.PurchasingNotes = reader.GetString(13);
    //                }
    //                if (reader.IsDBNull(14) == false)
    //                {
    //                    model.EngineeringNotes = reader.GetString(14);
    //                }
    //                if (reader.IsDBNull(15) == false)
    //                {
    //                    model.ArtworkNotes = reader.GetString(15);
    //                }
    //                if (reader.IsDBNull(16) == false)
    //                {
    //                    model.Acc_BillingNotes = reader.GetString(16);
    //                }
    //                if (reader.IsDBNull(17) == false)
    //                {
    //                    model.DCPNotes = reader.GetString(17);
    //                }
    //                if (reader.IsDBNull(18) == false)
    //                {
    //                    model.JobSheetNo = reader.GetString(18);
    //                }


    //            }
    //            viewDailyJob.Add(model);
    //        }
    //        cn.Close();
    //    }

    //    return View(viewDailyJob);

    //}


    List<JobInstruction> viewSchedulerJob = new List<JobInstruction>();
    public ActionResult ManageSchedulerJob(string set, string ProductName, string JobSheetNo)
    {
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.IsRole = @Session["Role"];
        ViewBag.UserId = Session["Fullname"];
        var IdentityName = @Session["Fullname"];

        if (set == "search")
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();


                command.CommandText = @"SELECT JobInstruction.Id, JobInstruction.CreatedOn as CreatedOn, JobInstruction.JobSheetNo, JobInstruction.Customer_Name, JobInstruction.ProductName, JobInstruction.JobClass, 
                                               JobInstruction.JobType, JobInstruction.AccountsQty, JobInstruction.ImpressionQty, JobInstruction.PagesQty, JobInstruction.Status,
                                               JobInstruction.IT_SysNotes,JobInstruction.Produc_PlanningNotes,JobInstruction.PurchasingNotes,JobInstruction.EngineeringNotes,
                                               JobInstruction.ArtworkNotes, JobInstruction.Acc_BillingNotes, JobInstruction.DCPNotes, JobInstruction.SalesExecutiveBy,JobInstruction.Cust_Department, JobInstruction.ModifiedOn as ReleaseDate
                                               FROM JobInstruction WHERE JobInstruction.Status NOT IN('Waiting to Assign Programmer', 'Development Process', 'Development Complete', 'New') AND ProductName LIKE @ProductNameSearch";
                //command.CommandText = @"SELECT JobInstruction.Id, JobInstruction.CreatedOn as CreatedOn, JobInstruction.JobSheetNo, JobInstruction.Customer_Name, JobInstruction.ProductName, JobInstruction.JobClass, 
                //                               JobInstruction.JobType, JobInstruction.AccountsQty, JobInstruction.ImpressionQty, JobInstruction.PagesQty, JobInstruction.Status,
                //                               JobInstruction.IT_SysNotes,JobInstruction.Produc_PlanningNotes,JobInstruction.PurchasingNotes,JobInstruction.EngineeringNotes,
                //                               JobInstruction.ArtworkNotes, JobInstruction.Acc_BillingNotes, JobInstruction.DCPNotes, JobInstruction.SalesExecutiveBy,JobInstruction.Cust_Department, MAX(JobAuditTrailDetail.CreatedOn) as ReleaseDate
                //                               FROM JobInstruction INNER JOIN JobAuditTrailDetail ON JobInstruction.JobSheetNo = JobAuditTrailDetail.JobSheetNo WHERE (JobInstruction.Status != 'New' OR JobInstruction.Status NOT IN ('Waiting to Assign Programmer','Development Process','Development Complete')) AND JobInstruction.ProductName LIKE @ProductNameSearch
                //                               GROUP BY JobInstruction.Id, JobInstruction.CreatedOn, JobInstruction.JobSheetNo, JobInstruction.Customer_Name, JobInstruction.ProductName, JobInstruction.JobClass, JobInstruction.JobType,  JobInstruction.AccountsQty, JobInstruction.ImpressionQty, JobInstruction.PagesQty, JobInstruction.Status,JobInstruction.IT_SysNotes,
                //                               JobInstruction.Produc_PlanningNotes,JobInstruction.PurchasingNotes,JobInstruction.EngineeringNotes,JobInstruction.ArtworkNotes, JobInstruction.Acc_BillingNotes, JobInstruction.DCPNotes, JobInstruction.SalesExecutiveBy,JobInstruction.Cust_Department";

                //command.CommandText = @"SELECT Id, ModifiedOn, JobSheetNo, Customer_Name, ProductName, JobClass, 
                //                           JobType,  AccountsQty, ImpressionQty, PagesQty, Status,
                //                           IT_SysNotes,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes,
                //                           ArtworkNotes, Acc_BillingNotes, DCPNotes, SalesExecutiveBy,Cust_Department
                //                           FROM [dbo].[JobInstruction] WHERE Status IN ('Waiting to Assign Programmer', 'Development Process', 'Development Complete') OR Status != 'New' AND ProductName LIKE @ProductNameSearch;";
                command.Parameters.AddWithValue("@ProductNameSearch", "%" + ProductName + "%");
                //WHERE Status = 'ITO' OR Status = 'FINANCE'
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
                            model.CreatedOn = reader["CreatedOn"].ToString();
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            model.JobSheetNo = reader.GetString(2);
                        }
                        if (reader.IsDBNull(3) == false)
                        {
                            model.Customer_Name = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            model.ProductName = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            model.JobClass = reader.GetString(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.JobType = reader.GetString(6);
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.AccountsQty = reader.GetString(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.ImpressionQty = reader.GetString(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            model.PagesQty = reader.GetString(9);
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            model.Status = reader.GetString(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            model.IT_SysNotes = reader.GetString(11);
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            model.Produc_PlanningNotes = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            model.PurchasingNotes = reader.GetString(13);
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            model.EngineeringNotes = reader.GetString(14);
                        }
                        if (reader.IsDBNull(15) == false)
                        {
                            model.ArtworkNotes = reader.GetString(15);
                        }
                        if (reader.IsDBNull(16) == false)
                        {
                            model.Acc_BillingNotes = reader.GetString(16);
                        }
                        if (reader.IsDBNull(17) == false)
                        {
                            model.DCPNotes = reader.GetString(17);
                        }
                        if (reader.IsDBNull(18) == false)
                        {
                            model.SalesExecutiveBy = reader.GetString(18);
                        }
                        if (reader.IsDBNull(19) == false)
                        {
                            model.Cust_Department = reader.GetString(19);
                        }
                        if (reader.IsDBNull(20) == false)
                        {
                            model.JobRequestTxt = reader["ReleaseDate"].ToString();
                        }
                    }
                    viewSchedulerJob.Add(model);
                }
                cn.Close();
            }


            //using (SqlCommand command = new SqlCommand("", cn))
            //{
            //    int _bil = 1;
            //    cn.Open();
            //    //command.CommandText = @"SELECT Id, ModifiedOn, JobSheetNo, Customer_Name, ProductName, JobClass, 
            //    //                               JobType,  AccountsQty, ImpressionQty, PagesQty, Status,
            //    //                               IT_SysNotes,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes,
            //    //                               ArtworkNotes, Acc_BillingNotes, DCPNotes
            //    //                               FROM [dbo].[JobInstruction]                                                
            //    //                               AND ProductName LIKE @ProductName";

            //    command.CommandText = @"SELECT Id, ModifiedOn, JobSheetNo, Customer_Name, ProductName, JobClass, 
            //                                   JobType,  AccountsQty, ImpressionQty, PagesQty, Status,
            //                                   IT_SysNotes,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes,
            //                                   ArtworkNotes, Acc_BillingNotes, DCPNotes,Cust_Department
            //                                   FROM [dbo].[JobInstruction]                                                
            //                                   WHERE ProductName LIKE @ProductName";
            //    command.Parameters.AddWithValue("@ProductName", "%" + ProductName + "%");
            //    var reader = command.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        JobInstruction model = new JobInstruction();
            //        {
            //            model.Bil = _bil++;
            //            if (reader.IsDBNull(0) == false)
            //            {
            //                model.Id = reader.GetGuid(0);
            //            }
            //            if (reader.IsDBNull(1) == false)
            //            {
            //                model.ModifiedOn = reader.GetDateTime(1);
            //            }
            //            if (reader.IsDBNull(2) == false)
            //            {
            //                model.JobSheetNo = reader.GetString(2);
            //            }
            //            if (reader.IsDBNull(3) == false)
            //            {
            //                model.Customer_Name = reader.GetString(3);
            //            }
            //            if (reader.IsDBNull(4) == false)
            //            {
            //                model.ProductName = reader.GetString(4);
            //            }
            //            if (reader.IsDBNull(5) == false)
            //            {
            //                model.JobClass = reader.GetString(5);
            //            }
            //            if (reader.IsDBNull(6) == false)
            //            {
            //                model.JobType = reader.GetString(6);
            //            }
            //            if (reader.IsDBNull(7) == false)
            //            {
            //                model.AccountsQty = reader.GetString(7);
            //            }
            //            if (reader.IsDBNull(8) == false)
            //            {
            //                model.ImpressionQty = reader.GetString(8);
            //            }
            //            if (reader.IsDBNull(9) == false)
            //            {
            //                model.PagesQty = reader.GetString(9);
            //            }
            //            if (reader.IsDBNull(10) == false)
            //            {
            //                model.Status = reader.GetString(10);
            //            }
            //            if (reader.IsDBNull(11) == false)
            //            {
            //                model.IT_SysNotes = reader.GetString(11);
            //            }
            //            if (reader.IsDBNull(12) == false)
            //            {
            //                model.Produc_PlanningNotes = reader.GetString(12);
            //            }
            //            if (reader.IsDBNull(13) == false)
            //            {
            //                model.PurchasingNotes = reader.GetString(13);
            //            }
            //            if (reader.IsDBNull(14) == false)
            //            {
            //                model.EngineeringNotes = reader.GetString(14);
            //            }
            //            if (reader.IsDBNull(15) == false)
            //            {
            //                model.ArtworkNotes = reader.GetString(15);
            //            }
            //            if (reader.IsDBNull(16) == false)
            //            {
            //                model.Acc_BillingNotes = reader.GetString(16);
            //            }
            //            if (reader.IsDBNull(17) == false)
            //            {
            //                model.DCPNotes = reader.GetString(17);
            //            }
            //            if (reader.IsDBNull(18) == false)
            //            {
            //                model.Cust_Department = reader.GetString(18);
            //            }
            //        }
            //        viewSchedulerJob.Add(model);
            //    }
            //    cn.Close();
            //}
        }
        else
        {
            //ALL firt masuk
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT JobInstruction.Id, JobInstruction.CreatedOn as CreatedOn, JobInstruction.JobSheetNo, JobInstruction.Customer_Name, JobInstruction.ProductName, JobInstruction.JobClass, 
                                               JobInstruction.JobType, JobInstruction.AccountsQty, JobInstruction.ImpressionQty, JobInstruction.PagesQty, JobInstruction.Status,
                                               JobInstruction.IT_SysNotes,JobInstruction.Produc_PlanningNotes,JobInstruction.PurchasingNotes,JobInstruction.EngineeringNotes,
                                               JobInstruction.ArtworkNotes, JobInstruction.Acc_BillingNotes, JobInstruction.DCPNotes, JobInstruction.SalesExecutiveBy,JobInstruction.Cust_Department, JobInstruction.ModifiedOn as ReleaseDate
                                               FROM JobInstruction WHERE JobInstruction.Status NOT IN ('Waiting to Assign Programmer','Development Process','Development Complete','New')";

                //WHERE Status = 'ITO' OR Status = 'FINANCE'
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
                            model.CreatedOn = reader["CreatedOn"].ToString();
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            model.JobSheetNo = reader.GetString(2);
                        }
                        if (reader.IsDBNull(3) == false)
                        {
                            model.Customer_Name = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            model.ProductName = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            model.JobClass = reader.GetString(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.JobType = reader.GetString(6);
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.AccountsQty = reader.GetString(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.ImpressionQty = reader.GetString(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            model.PagesQty = reader.GetString(9);
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            model.Status = reader.GetString(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            model.IT_SysNotes = reader.GetString(11);
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            model.Produc_PlanningNotes = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            model.PurchasingNotes = reader.GetString(13);
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            model.EngineeringNotes = reader.GetString(14);
                        }
                        if (reader.IsDBNull(15) == false)
                        {
                            model.ArtworkNotes = reader.GetString(15);
                        }
                        if (reader.IsDBNull(16) == false)
                        {
                            model.Acc_BillingNotes = reader.GetString(16);
                        }
                        if (reader.IsDBNull(17) == false)
                        {
                            model.DCPNotes = reader.GetString(17);
                        }
                        if (reader.IsDBNull(18) == false)
                        {
                            model.SalesExecutiveBy = reader.GetString(18);
                        }
                        if (reader.IsDBNull(19) == false)
                        {
                            model.Cust_Department = reader.GetString(19);
                        }
                        if (reader.IsDBNull(20) == false)
                        {
                            model.JobRequestTxt = reader["ReleaseDate"].ToString();
                        }
                    }
                    viewSchedulerJob.Add(model);
                }
                cn.Close();
            }
        }
        return View(viewSchedulerJob);
    }


    public ActionResult ReloadSJ()
    {
        List<SchedulerJob> viewSJ = new List<SchedulerJob>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT Id, Customer_Name, Frequency, JobRequest, ProductName, JobClass, 
                                           JobType, SalesExecutiveBy
                                      FROM [dbo].[SchedulerJob]  
                                      WHERE JobInstructionId=@Id";
            command.Parameters.AddWithValue("@Id", Session["Id"].ToString());
            var reader = command.ExecuteReader();
            while (reader.Read())
            {

                SchedulerJob model = new SchedulerJob();
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
                        model.Frequency = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        model.JobRequestTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(3));
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        model.ProductName = reader.GetString(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        model.JobClass = reader.GetString(5);
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        model.JobType = reader.GetString(6);
                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        model.SalesExecutiveBy = reader.GetString(7);
                    }


                }
                viewSJ.Add(model);
            }
            cn.Close();
            return Json(viewSJ);
        }
    }


    public ActionResult ViewSchedulerJob(string Id, string set, string Customer_Name, string Frequency, string JobRequest, string ProductName, string JobClass,
                                          string JobType, string SalesExecutiveBy, string JobInstructionId, string Datepicker)

    {

        List<SchedulerJob> viewSJ = new List<SchedulerJob>();

        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.AccountManager = IdentityName.ToString();
        Session["JobInstructionId"] = Id;
        Session["Id"] = Id;
        ViewBag.Id = Id;
        ViewBag.JobInstructionId = JobInstructionId;

        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        {
            cn.Open();

            int _bil = 1;
            SqlCommand cmd = new SqlCommand("SELECT Id, Customer_Name, Frequency, JobRequest, ProductName, JobClass, JobType, SalesExecutiveBy, JobSheetNo, CreatedOn, ModifiedOn FROM [dbo].[SchedulerJob] WHERE JobInstructionId=@Id", cn);
            cmd.Parameters.AddWithValue("@Id", Id);
            SqlDataReader rm = cmd.ExecuteReader();

            while(rm.Read())
            {

                SchedulerJob model = new SchedulerJob();
                {
                    model.Bil = _bil++;
                    if (rm.IsDBNull(0) == false)
                    {
                        model.Id = rm.GetGuid(0);
                    }
                    if (rm.IsDBNull(1) == false)
                    {
                        model.Customer_Name = rm.GetString(1);
                    }
                    if (rm.IsDBNull(2) == false)
                    {
                        model.Frequency = rm.GetString(2);
                    }
                    if (rm.IsDBNull(3) == false)
                    {
                        model.JobRequestTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)rm.GetDateTime(3));
                    }
                    if (rm.IsDBNull(4) == false)
                    {
                        model.ProductName = rm.GetString(4);
                    }
                    if (rm.IsDBNull(5) == false)
                    {
                        model.JobClass = rm.GetString(5);
                    }
                    if (rm.IsDBNull(6) == false)
                    {
                        model.JobType = rm.GetString(6);
                    }
                    if (rm.IsDBNull(7) == false)
                    {
                        model.SalesExecutiveBy = rm.GetString(7);
                    }
                    if (rm.IsDBNull(8) == false)
                    {
                        model.JobSheetNo = rm.GetString(8);
                    }
                    if (rm.IsDBNull(9) == false)
                    {
                        model.CreatedOn = rm["CreatedOn"].ToString();
                    }
                    if (rm.IsDBNull(10) == false)
                    {
                        model.ModifiedOn = rm["ModifiedOn"].ToString();
                    }


                }
                viewSJ.Add(model);
            }

            cn.Close();
        }


        return View(viewSJ);

    }

    public ActionResult CreateSchedulerJob(SchedulerJob ModelSample, SchedulerJob get, string Id, string set,
                                           string JobRequest, string Customer_Name, string ProductName, string JobClass,
                                           string JobType, string JobInstructionId, string SalesExecutiveBy)
    {
        var IdentityName = @Session["Fullname"];
        Session["Id"] = Id;
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.IsRole = @Session["Role"];
        string Deptment = @Session["Department"].ToString();
        ViewBag.Id = Id;
        ViewBag.JobRequest = JobRequest;

        if (!string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(JobRequest))
        {


            List<SchedulerJob> ViewSJ = new List<SchedulerJob>();
            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command2 = new SqlCommand("", cn2))
            {
                int _bil = 1;
                cn2.Open();
                command2.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass, 
                                                JobType, SalesExecutiveBy, JobSheetNo, Cust_Department,FORMAT(CreatedOn,'dd-MM-yyyy') as CreatedOn ,FORMAT(ModifiedOn,'dd-MM-yyyy') as ModifiedOn
                                      FROM [dbo].[JobInstruction]  
                                      WHERE Id=@Id";
                command2.Parameters.AddWithValue("@Id", Id);
                var reader = command2.ExecuteReader();
                while (reader.Read())
                {

                    SchedulerJob model = new SchedulerJob();
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
                            model.SalesExecutiveBy = reader.GetString(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.JobSheetNo = reader.GetString(6);
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.Cust_Department = reader.GetString(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.CreatedOn = reader["CreatedOn"].ToString();
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            model.ModifiedOn = reader["ModifiedOn"].ToString();
                        }
                    }



                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        get.JobRequest = Convert.ToDateTime(get.JobRequestTxt);
                        Guid Idx = Guid.NewGuid();


                        cn.Open();
                        SqlCommand command;
                        command = new SqlCommand("INSERT INTO [dbo].[SchedulerJob] (Id, Customer_Name, Frequency, JobRequest, ProductName, JobClass, JobType, SalesExecutiveBy, JobInstructionId,JobSheetNo,Cust_Department,CreatedOn,ModifiedOn) values (@Id, @Customer_Name, @Frequency, @JobRequest, @ProductName, @JobClass, @JobType, @SalesExecutiveBy, @JobInstructionId, @JobSheetNo, @Cust_Department,@CreatedOn,@ModifiedOn)", cn);
                        command.Parameters.AddWithValue("@Id", Idx);
                        command.Parameters.AddWithValue("@Customer_Name", model.Customer_Name);
                        command.Parameters.AddWithValue("@Frequency", model.JobClass + "-" + JobRequest);
                        if (!string.IsNullOrEmpty(JobRequest))
                        {
                            string ccc1 = Convert.ToDateTime(JobRequest).ToString("yyyy-MM-dd");
                            command.Parameters.AddWithValue("@JobRequest", ccc1);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@JobRequest", null);
                        }
                        command.Parameters.AddWithValue("@ProductName", model.ProductName);
                        command.Parameters.AddWithValue("@JobClass", model.JobClass);
                        command.Parameters.AddWithValue("@JobType", model.JobType);

                        command.Parameters.AddWithValue("@SalesExecutiveBy", IdentityName.ToString());
                        command.Parameters.AddWithValue("@JobInstructionId", model.Id);
                        command.Parameters.AddWithValue("@JobSheetNo", model.JobSheetNo);
                        command.Parameters.AddWithValue("@CreatedOn", model.CreatedOn);
                        command.Parameters.AddWithValue("@ModifiedOn", DateTime.Now.ToString());


                        if (!string.IsNullOrEmpty(model.Cust_Department))
                        {
                            command.Parameters.AddWithValue("@Cust_Department", model.Cust_Department);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Cust_Department", DBNull.Value);

                        }

                        command.ExecuteNonQuery();
                        cn.Close();
                    }

                }
                cn2.Close();
            }

            return RedirectToAction("ViewSchedulerJob", "ITO", new { Id = Session["Id"].ToString() });
        }

        return View();
    }

    public ActionResult DeleteSchedulerTask(string Id)
    {
        if (!string.IsNullOrEmpty(Id))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("DELETE [dbo].[SchedulerJob] WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn.Close();
            }


        }
        return RedirectToAction("ViewSchedulerJob", "ITO", new { Id = Session["Id"].ToString() });
    }

    public ActionResult SubmitAudit(string Id, string Customer_Name, string ProductName, string JobClass, string JobsheetNo, string JobType, String Cust_Department)

    {
        var IdentityName = @Session["Fullname"];
        Session["Id"] = Id;
        string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

        Debug.WriteLine("JobType : " + JobType);

        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT Id, ModifiedOn, JobSheetNo, Customer_Name, ProductName, JobClass, 
                                               JobType,  AccountsQty, ImpressionQty, PagesQty, Status, SalesExecutiveBy
                                               FROM [dbo].[JobInstruction] 
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
                        model.Id = reader.GetGuid(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        model.ModifiedOn = reader.GetDateTime(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        model.JobSheetNo = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        model.Customer_Name = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        model.ProductName = reader.GetString(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        model.JobClass = reader.GetString(5);
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        model.JobType = reader.GetString(6);
                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        model.AccountsQty = reader.GetString(7);
                    }
                    if (reader.IsDBNull(8) == false)
                    {
                        model.ImpressionQty = reader.GetString(8);
                    }
                    if (reader.IsDBNull(9) == false)
                    {
                        model.PagesQty = reader.GetString(9);
                    }
                    if (reader.IsDBNull(10) == false)
                    {
                        model.Status = reader.GetString(10);
                    }
                    if (reader.IsDBNull(11) == false)
                    {
                        model.SalesExecutiveBy = reader.GetString(11);
                    }
                }
                viewSchedulerJob.Add(model);
            }
            cn.Close();
        }

        //using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        //{

        //    cn3.Open();
        //    SqlCommand command1;
        //    command1 = new SqlCommand("UPDATE [dbo].[JobInstruction] SET ModifiedOn=@ModifiedOn, STATUS='ITO' WHERE Id=@Id", cn3);
        //    command1.Parameters.AddWithValue("@Id", Id);
        //    command1.Parameters.AddWithValue("@ModifiedOn", createdOn);
        //    command1.ExecuteNonQuery();
        //    cn3.Close();


        //}
        using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        {
            Session["Id"] = Id;
            Guid Idx = Guid.NewGuid();
            var No_ = new ITOLog();

            cn1.Open();
            SqlCommand command1;
            command1 = new SqlCommand("INSERT INTO [dbo].[JobAuditTrailDetail](Id, Customer_Name, ProductName, JobClass, JobType, LogTagNo,CreatedOn,Status,JobSheetNo,Cust_Department,CreateByIt) values (@Id,  @Customer_Name, @ProductName, @JobClass, @JobType, @LogTagNo,@CreatedOn,@Status,@JobSheetNo,@Cust_Department,@CreateByIt)", cn1);
            command1.Parameters.AddWithValue("@Id", Idx);

            if (Customer_Name != null)
            {
                command1.Parameters.AddWithValue("@Customer_Name", Customer_Name);
            }
            else
            {
                command1.Parameters.AddWithValue("@Customer_Name", DBNull.Value);
            }
            if (ProductName != null)
            {
                command1.Parameters.AddWithValue("@ProductName", ProductName);
            }
            else
            {
                command1.Parameters.AddWithValue("@ProductName", DBNull.Value);
            }
            if (JobClass != null)
            {
                command1.Parameters.AddWithValue("@JobClass", JobClass);
            }
            else
            {
                command1.Parameters.AddWithValue("@JobClass", DBNull.Value);
            }

            if (JobType != null)
            {
                command1.Parameters.AddWithValue("@JobType", JobType);
            }
            else
            {
                command1.Parameters.AddWithValue("@JobType", DBNull.Value);
            }
            if (JobsheetNo != null)
            {
                command1.Parameters.AddWithValue("@JobsheetNo", JobsheetNo);
            }
            else
            {
                command1.Parameters.AddWithValue("@JobsheetNo", DBNull.Value);
            }
            if (Cust_Department != null)
            {
                command1.Parameters.AddWithValue("@Cust_Department", Cust_Department);
            }
            else
            {
                command1.Parameters.AddWithValue("@Cust_Department", DBNull.Value);
            }
            if (IdentityName != null)
            {
                command1.Parameters.AddWithValue("@CreateByIt", IdentityName);
            }
            else
            {
                command1.Parameters.AddWithValue("@Cust_Department", DBNull.Value);
            }

            if (JobType == "MMP")
            {
                string MMPLogTag = No_.RefNo.Substring(2, 7);
                MMPLogTag = "M" + MMPLogTag;
                command1.Parameters.AddWithValue("@LogTagNo", MMPLogTag);

            }
            else
            {
                command1.Parameters.AddWithValue("@LogTagNo", No_.RefNo);

            }

            command1.Parameters.AddWithValue("@CreatedOn", createdOn);

            command1.Parameters.AddWithValue("@Status", "PROCESSING");
            //command1.Parameters.AddWithValue("@JobAuditTrailId", Id);

            command1.ExecuteNonQuery();
            cn1.Close();

        }

        return RedirectToAction("ManageSchedulerJob", "ITO", new { Id = Session["Id"].ToString() });

    }


    //addnewLogtag

    public ActionResult AddNewLogTagNo(string Id, string Customer_Name, string ProductName, string JobClass, string Frequency, string JobType, string LogTagNo, string JobAuditTrailId, JobAuditTrailDetail get, string JobSheetNo, string RevStrtDateOn,string RevStrtTime, string RevEndDateOn, string RevEndTimeOn, string ProcessDate, string TimeProcessIt,string ProcessEnd, string TimeEndProcessIt)

    {
        var IdentityName = @Session["Fullname"];

        Session["Id"] = Id;
        string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
        ViewBag.LogTagNo = LogTagNo;

        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT Id, Customer_Name, ProductName, LogTagNo,JobClass,JobAuditTrailId                                              
                                               FROM [dbo].[JobAuditTrailDetail] 
                                               WHERE Customer_Name =@Customer_Name";
            command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
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
                        model.LogTagNo = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        model.JobClass = reader.GetString(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        model.JobAuditTrailId = reader.GetGuid(5);

                    }

                }

            }
            cn.Close();

            cn.Open();
            Guid Idx = Guid.NewGuid();
            ViewBag.LogTagNo = LogTagNo;



            SqlCommand command1;
            command1 = new SqlCommand("INSERT INTO [dbo].[JobAuditTrailDetail](Id, Customer_Name, ProductName, LogTagNo,CreatedOn,Status,JobClass,JobSheetNo, JobType, CreateByIt, RevStrtDateOn, RevStrtTime, RevEndDateOn, RevEndTimeOn, ProcessDate, TimeProcessIt,ProcessEnd, TimeEndProcessIt) " +
                "values (@Id,  @Customer_Name, @ProductName, @LogTagNo,@CreatedOn,@Status,@JobClass,@JobSheetNo,@JobType,@CreateByIt,@RevStrtDateOn, @RevStrtTime, @RevEndDateOn, @RevEndTimeOn, @ProcessDate, @TimeProcessIt, @ProcessEnd, @TimeEndProcessIt)", cn);
            command1.Parameters.AddWithValue("@Id", Idx);

            if (Customer_Name != null)
            {
                command1.Parameters.AddWithValue("@Customer_Name", Customer_Name);
            }
            else
            {
                command1.Parameters.AddWithValue("@Customer_Name", DBNull.Value);
            }
            if (ProductName != null)
            {
                command1.Parameters.AddWithValue("@ProductName", ProductName);
            }
            else
            {
                command1.Parameters.AddWithValue("@ProductName", DBNull.Value);
            }

            if (JobType != null)
            {
                command1.Parameters.AddWithValue("@JobType", JobType);
            }
            else
            {
                command1.Parameters.AddWithValue("@ProductName", DBNull.Value);
            }

            command1.Parameters.AddWithValue("@LogTagNo", LogTagNo);

            command1.Parameters.AddWithValue("@CreatedOn", createdOn);

            command1.Parameters.AddWithValue("@Status", "PROCESSING");
            if (JobClass != null)
            {
                command1.Parameters.AddWithValue("@JobClass", JobClass);
            }
            else
            {
                command1.Parameters.AddWithValue("@JobClass", DBNull.Value);
            }
            if (JobSheetNo != null)
            {
                command1.Parameters.AddWithValue("@JobSheetNo", JobSheetNo);
            }
            else
            {
                command1.Parameters.AddWithValue("@JobSheetNo", DBNull.Value);
            }

            if (IdentityName != null)
            {
                command1.Parameters.AddWithValue("@CreateByIt", IdentityName);
            }
            else
            {
                command1.Parameters.AddWithValue("@CreateByIt", DBNull.Value);
            }

            if(!string.IsNullOrEmpty(RevStrtDateOn))
            {
                command1.Parameters.AddWithValue("@RevStrtDateOn", RevStrtDateOn);

            }
            else
            {
                command1.Parameters.AddWithValue("@RevStrtDateOn", DBNull.Value);

            }

            if (!string.IsNullOrEmpty(RevStrtTime))
            {
                command1.Parameters.AddWithValue("@RevStrtTime", RevStrtTime);

            }
            else
            {
                command1.Parameters.AddWithValue("@RevStrtTime", DBNull.Value);

            }

            if (!string.IsNullOrEmpty(RevEndDateOn))
            {
                command1.Parameters.AddWithValue("@RevEndDateOn", RevEndDateOn);

            }
            else
            {
                command1.Parameters.AddWithValue("@RevEndDateOn", DBNull.Value);

            }

            if (!string.IsNullOrEmpty(RevEndTimeOn))
            {
                command1.Parameters.AddWithValue("@RevEndTimeOn", RevEndTimeOn);

            }
            else
            {
                command1.Parameters.AddWithValue("@RevEndTimeOn", DBNull.Value);

            }

            if (!string.IsNullOrEmpty(ProcessDate))
            {
                command1.Parameters.AddWithValue("@ProcessDate", ProcessDate);

            }
            else
            {
                command1.Parameters.AddWithValue("@ProcessDate", DBNull.Value);

            }

            if (!string.IsNullOrEmpty(TimeProcessIt))
            {
                command1.Parameters.AddWithValue("@TimeProcessIt", TimeProcessIt);

            }
            else
            {
                command1.Parameters.AddWithValue("@TimeProcessIt", DBNull.Value);

            }

            if (!string.IsNullOrEmpty(ProcessEnd))
            {
                command1.Parameters.AddWithValue("@ProcessEnd", ProcessEnd);

            }
            else
            {
                command1.Parameters.AddWithValue("@ProcessEnd", DBNull.Value);

            }

            if (!string.IsNullOrEmpty(TimeEndProcessIt))
            {
                command1.Parameters.AddWithValue("@TimeEndProcessIt", TimeEndProcessIt);

            }
            else
            {
                command1.Parameters.AddWithValue("@TimeEndProcessIt", DBNull.Value);

            }

            //command1.Parameters.AddWithValue("@JobAuditTrailId", Id);

            command1.ExecuteNonQuery();

            SqlCommand cmd = new SqlCommand("SELECT DISTINCT Customer_Name, JobClass, Id FROM JobAuditTrailDetail " +
                "WHERE LogTagNo=@LogTagNo1", cn);
            cmd.Parameters.AddWithValue("@LogTagNo1", LogTagNo);
            SqlDataReader rm = cmd.ExecuteReader();

            while (rm.Read())
            {
                JobAuditTrailId = rm["Id"].ToString();
                JobClass = rm["JobClass"].ToString();
            }

            cn.Close();

            return RedirectToAction("AddJAT", "ITO", new { Id = Idx, Customer_Name = Customer_Name, LogTagNo = LogTagNo, JobClass = JobClass, JobSheetNo = JobSheetNo });

        }

        //Id = model.JobAuditTrailId, Customer_Name = model.Customer_Name, LogTagNo = model.LogTagNo, JobClass = model.JobClass



        //return RedirectToAction("AddJAT", "ITO", new { LogTagNo = Session["LogTagNo"].ToString() });


    }





    public ActionResult SubmitQME(string Id)

    {
        var IdentityName = @Session["Fullname"];
        Session["Id"] = Id;
        string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

        using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        {



            cn3.Open();
            SqlCommand command1;
            command1 = new SqlCommand("UPDATE [dbo].[JobInstruction] SET ModifiedOn=@ModifiedOn, STATUS='QME' WHERE JobInstructionId=@Id", cn3);
            command1.Parameters.AddWithValue("@Id", Id);
            command1.Parameters.AddWithValue("@ModifiedOn", createdOn);
            command1.ExecuteNonQuery();
            cn3.Close();



        }
        return RedirectToAction("ManageJAT", "ITO", new { Id = Session["Id"].ToString() });

    }

    [ValidateInput(false)]
    public ActionResult SubmitSchedulerTask(JobAuditTrail JobAuditTrail, JobAuditTrail get, JobInstruction JobInstruction, string Id, string ProductName, string set,
                                         string Customer_Name, string JobClass, string JobSheetNo, string JobRequest, string JobType, string Status,
                                         string AccountsQty, string ImpressionQty, string PagesQty, string Frequency, string JobInstructionId,
                                         string NotesByIT, string NotesByProduction, string NotesByPurchasing, string NotesByEngineering,
                                         string NotesByArtwork, string NotesByFinance, string NotesByDCP, string IT_SysNotes, string Produc_PlanningNotes,
                                         string PurchasingNotes, string EngineeringNotes, string ArtworkNotes, string Acc_BillingNotes, string DCPNotes)
    {


        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT a.Id,b.JobRequest,a.JobSheetNo,
                                           a.Customer_Name,a.ProductName,a.JobClass,a.JobType,b.Frequency,
                                           a.AccountsQty,a.ImpressionQty,a.PagesQty,b.JobInstructionId,
                                           a.IT_SysNotes,a.Produc_PlanningNotes,a.PurchasingNotes,a.EngineeringNotes,a.ArtworkNotes,a.Acc_BillingNotes,a.DCPNotes
                                           FROM [dbo].[JobInstruction] a, [dbo].[SchedulerJob] b
                                           WHERE a.Id=b.JobInstructionId AND b.Id=@Id";
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
                        ViewBag.JobRequest = reader.GetDateTime(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        ViewBag.JobSheetNo = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        ViewBag.Customer_Name = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        ViewBag.ProductName = reader.GetString(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        ViewBag.JobClass = reader.GetString(5);
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        ViewBag.JobType = reader.GetString(6);
                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        ViewBag.Frequency = reader.GetString(7);
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
                        ViewBag.JobInstructionId = reader.GetGuid(11);

                    }
                    if (reader.IsDBNull(12) == false)
                    {
                        ViewBag.IT_SysNotes = reader.GetString(12);
                    }
                    if (reader.IsDBNull(13) == false)
                    {
                        ViewBag.Produc_PlanningNotes = reader.GetString(13);
                    }
                    if (reader.IsDBNull(14) == false)
                    {
                        ViewBag.PurchasingNotes = reader.GetString(14);
                    }
                    if (reader.IsDBNull(15) == false)
                    {
                        ViewBag.EngineeringNotes = reader.GetString(15);
                    }
                    if (reader.IsDBNull(16) == false)
                    {
                        ViewBag.ArtworkNotes = reader.GetString(16);
                    }
                    if (reader.IsDBNull(17) == false)
                    {
                        ViewBag.Acc_BillingNotes = reader.GetString(17);
                    }
                    if (reader.IsDBNull(18) == false)
                    {
                        ViewBag.DCPNotes = reader.GetString(18);
                    }


                }

                //viewSubmitProcess.Add(model);

                using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    Guid JobAuditTrailId = Guid.NewGuid();
                    ViewBag.Id = Id;
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                    get.JobRequest = Convert.ToDateTime(get.JobRequestTxt);

                    cn1.Open();
                    SqlCommand command1;
                    command1 = new SqlCommand("INSERT INTO [dbo].[JobAuditTrail](Id, CreatedOn, Customer_Name, Frequency, JobRequest, JobSheetNo, ProductName, JobClass, JobType, Status, AccountsQty, ImpressionQty, PagesQty, NotesByIT, NotesByProduction, NotesByPurchasing, NotesByEngineering, NotesByArtwork, NotesByFinance, NotesByDCP, JobInstructionId) values (@Id, @CreatedOn, @Customer_Name, @Frequency, @JobRequest, @JobSheetNo, @ProductName, @JobClass, @JobType, @Status, @AccountsQty, @ImpressionQty, @PagesQty,  @NotesByIT, @NotesByProduction, @NotesByPurchasing, @NotesByEngineering, @NotesByArtwork, @NotesByFinance, @NotesByDCP, @JobInstructionId)", cn1);
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
                    if (model.Frequency != null)
                    {
                        command1.Parameters.AddWithValue("@Frequency", model.Frequency);
                    }
                    else
                    {
                        command1.Parameters.AddWithValue("@Frequency", DBNull.Value);
                    }
                    if (model.JobRequest != null)
                    {
                        command1.Parameters.AddWithValue("@JobRequest", model.JobRequest);
                    }
                    else
                    {
                        command1.Parameters.AddWithValue("@JobRequest", DBNull.Value);
                    }
                    if (model.JobSheetNo != null)
                    {
                        command1.Parameters.AddWithValue("@JobSheetNo", model.JobSheetNo);
                    }
                    else
                    {
                        command1.Parameters.AddWithValue("@JobSheetNo", DBNull.Value);
                    }
                    if (model.ProductName != null)
                    {
                        command1.Parameters.AddWithValue("@ProductName", model.ProductName);
                    }
                    else
                    {
                        command1.Parameters.AddWithValue("@ProductName", DBNull.Value);
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
                    if (model.PagesQty != null)
                    {
                        command1.Parameters.AddWithValue("@PagesQty", model.PagesQty);
                    }
                    else
                    {
                        command1.Parameters.AddWithValue("@PagesQty", DBNull.Value);
                    }
                    if (model.NotesByIT != null)
                    {
                        command1.Parameters.AddWithValue("@NotesByIT", model.NotesByIT);
                    }
                    else
                    {
                        command1.Parameters.AddWithValue("@NotesByIT", DBNull.Value);
                    }
                    if (model.NotesByProduction != null)
                    {
                        command1.Parameters.AddWithValue("@NotesByProduction", model.NotesByProduction);
                    }
                    else
                    {
                        command1.Parameters.AddWithValue("@NotesByProduction", DBNull.Value);
                    }
                    if (model.NotesByPurchasing != null)
                    {
                        command1.Parameters.AddWithValue("@NotesByPurchasing", model.NotesByPurchasing);
                    }
                    else
                    {
                        command1.Parameters.AddWithValue("@NotesByPurchasing", DBNull.Value);
                    }
                    if (model.NotesByEngineering != null)
                    {
                        command1.Parameters.AddWithValue("@NotesByEngineering", model.NotesByEngineering);
                    }
                    else
                    {
                        command1.Parameters.AddWithValue("@NotesByEngineering", DBNull.Value);
                    }
                    if (model.NotesByArtwork != null)
                    {
                        command1.Parameters.AddWithValue("@NotesByArtwork", model.NotesByArtwork);
                    }
                    else
                    {
                        command1.Parameters.AddWithValue("@NotesByArtwork", DBNull.Value);
                    }
                    if (model.NotesByFinance != null)
                    {
                        command1.Parameters.AddWithValue("@NotesByFinance", model.NotesByFinance);
                    }
                    else
                    {
                        command1.Parameters.AddWithValue("@NotesByFinance", DBNull.Value);
                    }
                    if (model.NotesByDCP != null)
                    {
                        command1.Parameters.AddWithValue("@NotesByDCP", model.NotesByDCP);
                    }
                    else
                    {
                        command1.Parameters.AddWithValue("@NotesByDCP", DBNull.Value);
                    }
                    command1.Parameters.AddWithValue("@JobInstructionId", model.Id);
                    command1.ExecuteNonQuery();
                    cn1.Close();
                    TempData["msg"] = "<script>alert('SCHEDULER JOB ALREADY SENT !');</script>";

                }
            }
            cn.Close();

        }

        return RedirectToAction("ViewSchedulerJob", "ITO", new { Id = Session["Id"].ToString() });
    }
    ///
    List<JobAuditTrailDetail> viewJobAuditTrail = new List<JobAuditTrailDetail>();
    public ActionResult ManageJobAuditTrail(string set, string ProductName, string Id, string JobAuditTrailId, string Customer_Name, string LogTagNo)
    {
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.IsRole = @Session["Role"];
        var IdentityName = @Session["Fullname"];
        ViewBag.JobAuditTrailId = JobAuditTrailId;
        Session["Id"] = JobAuditTrailId;
        ViewBag.LogTagNo = LogTagNo;
        Session["LogTagNo"] = LogTagNo;


        if (set == "search")
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Id,ModifiedOn, Customer_Name, ProductName, JobSheetNo, JobClass, 
                                               Frequency, JobType, LogTagNo, AccountsQty, ImpressionQty, PagesQty, 
                                               TotalAuditTrail,  CreateByIT, Status, ModeLog, Path,JobNameIT,JobId
                                        FROM [dbo].[JobAuditTrailDetail] 
                                        WHERE LogTagNo=@LogTagNo
                                        AND ProductName LIKE @ProductName
                                        ORDER BY CreatedOn desc";
                command.Parameters.AddWithValue("@ProductName", "%" + ProductName + "%");
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
                            model.ModifiedOn = reader.GetDateTime(1);
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
                            model.JobSheetNo = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            model.JobClass = reader.GetString(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.Frequency = reader.GetString(6);
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.JobType = reader.GetString(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.LogTagNo = reader.GetString(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            model.AccountsQty = reader.GetString(9);
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            model.ImpressionQty = reader.GetString(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            model.PagesQty = reader.GetString(11);
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            model.TotalAuditTrail = reader.GetString(12);
                        }

                        if (reader.IsDBNull(13) == false)
                        {
                            model.CreateByIT = reader.GetString(13);
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            model.Status = reader.GetString(14);
                        }
                        if (reader.IsDBNull(15) == false)
                        {
                            model.ModeLog = reader.GetString(15);
                        }
                        if (reader.IsDBNull(16) == false)
                        {
                            model.Path = reader.GetString(16);
                        }
                        if (reader.IsDBNull(17) == false)
                        {
                            model.JobNameIT = reader.GetString(17);
                        }
                        if (reader.IsDBNull(18) == false)
                        {
                            model.JobId = reader.GetString(18);
                        }

                    }
                    viewJobAuditTrail.Add(model);
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
                command.CommandText = @"SELECT Id,ModifiedOn, Customer_Name, ProductName, JobSheetNo, JobClass, 
                                               Frequency, JobType, LogTagNo, Status, ModeLog, Path,JobNameIT,JobId,JobAuditTrailId
                                        FROM [dbo].[JobAuditTrailDetail] 
                                        where LogTagNo=@LogTagNo
                                        ORDER BY CreatedOn desc";
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
                            model.ModifiedOn = reader.GetDateTime(1);
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
                            model.JobSheetNo = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            model.JobClass = reader.GetString(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.Frequency = reader.GetString(6);
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.JobType = reader.GetString(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.LogTagNo = reader.GetString(8);
                        }

                        if (reader.IsDBNull(9) == false)
                        {
                            model.Status = reader.GetString(9);
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            model.ModeLog = reader.GetString(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            model.Path = reader.GetString(11);
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            model.JobNameIT = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            model.JobId = reader.GetString(13);
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            model.JobAuditTrailId = reader.GetGuid(14);
                        }

                    }
                    viewJobAuditTrail.Add(model);
                }
                cn.Close();
            }
        }



        return View(viewJobAuditTrail);

    }



    ///

    public List<string> TotalAll(string LogTagNo)
    {
        List<string> TotalAccImpPage = new List<string>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        {
            cn.Open();
            SqlCommand cmd1 = new SqlCommand("SELECT SUM(CAST(AccQty AS INT)) as A, SUM(CAST(PageQty AS INT)) AS B, SUM(CAST(ImpQty AS INT)) AS C FROM JobAuditTrailDetail WHERE LogTagNo = @LogTagNo1", cn);
            cmd1.Parameters.AddWithValue("@LogTagNo1", LogTagNo);
            SqlDataReader rm1 = cmd1.ExecuteReader();

            while (rm1.Read())
            {
                if (!rm1.IsDBNull(0))
                {
                    TotalAccImpPage.Add(rm1["A"].ToString());
                }
                else
                {
                    TotalAccImpPage.Add("0");
                }

                if (!rm1.IsDBNull(1))
                {
                    TotalAccImpPage.Add(rm1["B"].ToString());
                }
                else
                {
                    TotalAccImpPage.Add("0");
                }

                if (!rm1.IsDBNull(2))
                {
                    TotalAccImpPage.Add(rm1["C"].ToString());
                }
                else
                {
                    TotalAccImpPage.Add("0");
                }
            }

            cn.Close();

        }


        return TotalAccImpPage;
    }

    public string TotalAuditTrail(string LogTagNo)
    {
        string TotalFile = "";

        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        {
            cn.Open();

            SqlCommand cmd1 = new SqlCommand("SELECT COUNT(LogTagNo) as A FROM JobAuditTrailDetail WHERE LogTagNo=@LogTagNo1", cn);
            cmd1.Parameters.AddWithValue("LogTagNo1", LogTagNo);
            SqlDataReader rm1 = cmd1.ExecuteReader();

            while (rm1.Read())
            {
                TotalFile = rm1["A"].ToString();
            }

            cn.Close();
        }

        return TotalFile;
    }

    List<JobInstruction> JobInstructionlist2 = new List<JobInstruction>();
    public ActionResult ManageJAT(string Id, string ProductName, string customer, string set, string Status)

    {
        List<JobInstruction> JobInstructionlist1 = new List<JobInstruction>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {

            int _bil = 1;
            cn.Open();
            if (set == "search")
            {
                command.CommandText = @"SELECT 
                                        JobAuditTrailDetail.Customer_Name,
                                        JobAuditTrailDetail.LogTagNo,
                                        MAX(JobAuditTrailDetail.JobClass) AS JobClass,
                                        MAX(JobAuditTrailDetail.ProductName) AS ProductName,
                                        MAX(JobAuditTrailDetail.Status) AS Status,
                                        MAX(JobAuditTrailDetail.JobType) AS JobType,
                                        MAX(JobAuditTrailDetail.CreateByIt) AS CreateByIt,
                                        MAX(JobAuditTrailDetail.CreatedOn) AS CreatedOn,
                                        MAX(JobAuditTrailDetail.JobAuditTrailId) AS JobAuditTrailId,
                                        JobAuditTrailDetail.JobSheetNo,
                                        JobAuditTrailDetail.Remarks,
                                        JobInstruction.PaperType
                                    FROM 
                                        JobInstruction 
                                    INNER JOIN
                                        JobAuditTrailDetail ON JobInstruction.JobSheetNo = JobAuditTrailDetail.JobSheetNo
                                    WHERE 
                                        (JobAuditTrailDetail.JobClass='DAILY' OR JobAuditTrailDetail.JobClass LIKE '%DAILY-%') 
                                        AND (JobAuditTrailDetail.Status IN ('PROCESSING','QM : Need correction from ITO','MBD : Need correction from ITO','Planner : Need correction from ITO','Printing : Need correction from ITO','Inserting : Need correction from ITO','SelfMailer : Need correction from ITO','MMP : Need correction from ITO') ) 
                                        AND JobAuditTrailDetail.ProductName LIKE @Customer_Name
                                    GROUP BY 
                                        JobAuditTrailDetail.LogTagNo, JobAuditTrailDetail.Customer_Name,JobAuditTrailDetail.JobSheetNo,JobAuditTrailDetail.Remarks, JobInstruction.PaperType
                                    ORDER BY 
                                        JobAuditTrailDetail.LogTagNo;";

                command.Parameters.AddWithValue("@Customer_Name", "%" + customer + "%");
            }

            else
            {
                //command.CommandText = @"SELECT DISTINCT JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.LogTagNo, JobAuditTrailDetail.JobClass, JobAuditTrailDetail.ProductName,JobAuditTrailDetail.Status,JobAuditTrailDetail.JobAuditTrailId, JobAuditTrailDetail.JobClass,
                //                         JobAuditTrailDetail.JobType, JobAuditTrailDetail.CreateByIt, JobAuditTrailDetail.CreatedOn AS CreatedOn,JobAuditTrailDetail.JobAuditTrailId
                //                         FROM JobInstruction INNER JOIN
                //                         JobAuditTrailDetail ON JobInstruction.JobSheetNo = JobAuditTrailDetail.JobSheetNo
                //                         WHERE JobAuditTrailDetail.JobClass='DAILY' AND (JobAuditTrailDetail.Status='PROCESSING')
                //                      ORDER BY JobAuditTrailDetail.LogTagNo";

                command.CommandText = @"SELECT 
                                        JobAuditTrailDetail.Customer_Name,
                                        JobAuditTrailDetail.LogTagNo,
                                        MAX(JobAuditTrailDetail.JobClass) AS JobClass,
                                        MAX(JobAuditTrailDetail.ProductName) AS ProductName,
                                        MAX(JobAuditTrailDetail.Status) AS Status,
                                        MAX(JobAuditTrailDetail.JobType) AS JobType,
                                        MAX(JobAuditTrailDetail.CreateByIt) AS CreateByIt,
                                        MAX(JobAuditTrailDetail.CreatedOn) AS CreatedOn,
                                        MAX(JobAuditTrailDetail.JobAuditTrailId) AS JobAuditTrailId,
                                        JobAuditTrailDetail.JobSheetNo,
                                        JobAuditTrailDetail.Remarks,
                                        JobInstruction.PaperType
                                    FROM 
                                        JobInstruction 
                                    INNER JOIN
                                        JobAuditTrailDetail ON JobInstruction.JobSheetNo = JobAuditTrailDetail.JobSheetNo
                                    WHERE 
                                        (JobAuditTrailDetail.JobClass='DAILY' OR JobAuditTrailDetail.JobClass LIKE '%DAILY-%')
                                        AND (JobAuditTrailDetail.Status IN ('PROCESSING','QM : Need correction from ITO','MBD : Need correction from ITO','Planner : Need correction from ITO','Printing : Need correction from ITO','Inserting : Need correction from ITO','SelfMailer : Need correction from ITO','MMP : Need correction from ITO') ) 
                                    GROUP BY 
                                        JobAuditTrailDetail.LogTagNo, JobAuditTrailDetail.Customer_Name,JobAuditTrailDetail.JobSheetNo,JobAuditTrailDetail.Remarks, JobInstruction.PaperType
                                    ORDER BY 
                                        JobAuditTrailDetail.LogTagNo;";

                //OR JobAuditTrailDetail.Status = 'PLANNER' OR JobAuditTrailDetail.Status = 'PRODUCTION'
            }


            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                List<string> totalAll = TotalAll(reader.GetString(1));

                JobInstruction model = new JobInstruction();
                {
                    model.Bil = _bil++;

                    if (reader.IsDBNull(0) == false)
                    {
                        model.Customer_Name = reader.GetString(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        model.LogTagNo = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        model.JobClass = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        model.ProductName = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        model.Status = reader.GetString(4);
                        ViewBag.Status = reader.GetString(4);
                    }
                    //if (reader.IsDBNull(5) == false)
                    //{
                    //    model.JobAuditTrailId = reader.GetGuid(5);
                    //}
                    if (reader.IsDBNull(5) == false)
                    {
                        model.JobType = reader.GetString(5);
                    }

                    if (reader.IsDBNull(6) == false)
                    {
                        model.CreateUser = reader["CreateByIt"].ToString();
                    }
                    model.AccQty = totalAll[0];
                    model.PageQty = totalAll[1];
                    model.ImpQty = totalAll[2];

                    if (reader.IsDBNull(7) == false)
                    {
                        model.CreatedOn = reader["CreatedOn"].ToString();
                    }

                    if (reader.IsDBNull(8) == false)
                    {
                        model.JobAuditTrailId = reader.GetGuid(8);
                    }
                    if (reader.IsDBNull(9) == false)
                    {
                        model.JobSheetNo = reader.GetString(9);
                        ViewBag.JobSheetNo = reader.GetString(9);
                    }
                    if (reader.IsDBNull(10) == false)
                    {
                        model.Remarks = reader.GetString(10);
                    }
                    if (reader.IsDBNull(11) == false)
                    {
                        model.Paper = reader.GetString(11);
                    }



                    //if (reader.IsDBNull(10) == false)
                    //{
                    //    model.JobAuditTrailId = reader.GetGuid(10);
                    //}

                    model.TotalRecord = TotalAuditTrail(reader.GetString(1));

                }
                JobInstructionlist1.Add(model);
            }
            cn.Close();
        }

        return View(JobInstructionlist1);
    }

    public ActionResult ManageJAT2(string Id, string ProductName, string product, string set, string Status, string customer)

    {


        List<JobInstruction> JobInstructionlist1 = new List<JobInstruction>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {

            int _bil = 1;
            cn.Open();
            if (set == "search")
            {
                command.CommandText = @" SELECT 
                                        JobAuditTrailDetail.Customer_Name,
                                        JobAuditTrailDetail.LogTagNo,
                                        MAX(JobAuditTrailDetail.JobClass) AS JobClass,
                                        MAX(JobAuditTrailDetail.ProductName) AS ProductName,
                                        MAX(JobAuditTrailDetail.Status) AS Status,
                                        MAX(JobAuditTrailDetail.JobAuditTrailId) AS JobAuditTrailId,
                                        MAX(JobAuditTrailDetail.JobType) AS JobType,
                                        MAX(JobAuditTrailDetail.CreateByIt) AS CreateByIt,
                                        MAX(JobAuditTrailDetail.CreatedOn) AS CreatedOn,
                                        JobAuditTrailDetail.JobSheetNo,
                                        JobInstruction.PaperType,
                                        JobAuditTrailDetail.Remarks
                                    FROM 
                                        JobInstruction 
                                    INNER JOIN
                                        JobAuditTrailDetail ON JobInstruction.JobSheetNo = JobAuditTrailDetail.JobSheetNo
                                    WHERE 
                                        (JobAuditTrailDetail.JobClass !='DAILY' AND JobAuditTrailDetail.JobClass NOT LIKE '%DAILY-%') 
                                        AND(JobAuditTrailDetail.Status IN('PROCESSING', 'QM : Need correction from ITO', 'MBD : Need correction from ITO', 'Planner : Need correction from ITO', 'Printing : Need correction from ITO', 'Inserting : Need correction from ITO', 'SelfMailer : Need correction from ITO', 'MMP : Need correction from ITO'))
                                        AND JobAuditTrailDetail.ProductName LIKE @Customer_Name 
                                    GROUP BY 
                                        JobAuditTrailDetail.LogTagNo, JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.JobSheetNo,JobInstruction.PaperType,JobAuditTrailDetail.Remarks
                                    ORDER BY 
                                        JobAuditTrailDetail.LogTagNo;";

                //command.CommandText = @" SELECT 
                //                        JobAuditTrailDetail.Customer_Name,
                //                        JobAuditTrailDetail.LogTagNo,
                //                        MAX(JobAuditTrailDetail.JobClass) AS JobClass,
                //                        MAX(JobAuditTrailDetail.ProductName) AS ProductName,
                //                        MAX(JobAuditTrailDetail.Status) AS Status,
                //                        MAX(JobAuditTrailDetail.JobAuditTrailId) AS JobAuditTrailId,
                //                        MAX(JobAuditTrailDetail.JobType) AS JobType,
                //                        MAX(JobAuditTrailDetail.CreateByIt) AS CreateByIt,
                //                        MAX(JobAuditTrailDetail.CreatedOn) AS CreatedOn,
                //                        JobAuditTrailDetail.JobSheetNo,
                //                        JobInstruction.PaperType
                //                    FROM 
                //                        JobInstruction 
                //                    INNER JOIN
                //                        JobAuditTrailDetail ON JobInstruction.JobSheetNo = JobAuditTrailDetail.JobSheetNo
                //                    WHERE 
                //                        (JobAuditTrailDetail.JobClass !='DAILY' AND JobAuditTrailDetail.JobClass NOT LIKE '%DAILY-%') 
                //                        AND (JobAuditTrailDetail.Status='PROCESSING' OR JobAuditTrailDetail.Status='QM : Need correction from ITO') AND JobAuditTrailDetail.ProductName LIKE @Customer_Name 
                //                    GROUP BY 
                //                        JobAuditTrailDetail.LogTagNo, JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.JobSheetNo,JobInstruction.PaperType
                //                    ORDER BY 
                //                        JobAuditTrailDetail.LogTagNo;";

                command.Parameters.AddWithValue("@Customer_Name", "%" + customer + "%");
            }

            else
            {


                //command.CommandText = @" SELECT DISTINCT JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.LogTagNo, JobAuditTrailDetail.JobClass, JobAuditTrailDetail.ProductName,JobAuditTrailDetail.Status,JobInstruction.Id,JobInstruction.JobType
                //                         FROM  JobInstruction INNER JOIN
                //                         JobAuditTrailDetail ON JobInstruction.Id = JobAuditTrailDetail.JobAuditTrailId

                //      			 EXCEPT									                                      
                //                         SELECT DISTINCT JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.LogTagNo, JobAuditTrailDetail.JobClass, JobAuditTrailDetail.ProductName,JobAuditTrailDetail.Status,JobAuditTrailDetail.JobAuditTrailId                                        
                //                         FROM  JobInstruction INNER JOIN
                //                         JobAuditTrailDetail ON JobInstruction.Id = JobAuditTrailDetail.JobAuditTrailId
                //                        WHERE  JobAuditTrailDetail.JobClass='DAILY' AND (JobAuditTrailDetail.Status='PROCESSING' OR JobAuditTrailDetail.Status='PLANNER' OR JobAuditTrailDetail.Status='PRODUCTION')
                //                     ORDER BY JobAuditTrailDetail.LogTagNo ";

                //command.CommandText = @" SELECT DISTINCT JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.LogTagNo, JobAuditTrailDetail.JobClass, JobAuditTrailDetail.ProductName,JobAuditTrailDetail.Status,JobInstruction.Id,JobInstruction.JobType
                //                         FROM  JobInstruction INNER JOIN
                //                         JobAuditTrailDetail ON JobInstruction.Id = JobAuditTrailDetail.JobAuditTrailId                           
                //      			 EXCEPT									                                      
                //                         SELECT DISTINCT JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.LogTagNo, JobAuditTrailDetail.JobClass, JobAuditTrailDetail.ProductName,JobAuditTrailDetail.Status,JobAuditTrailDetail.JobAuditTrailId,JobInstruction.JobType                                        
                //                         FROM  JobInstruction INNER JOIN
                //                         JobAuditTrailDetail ON JobInstruction.Id = JobAuditTrailDetail.JobAuditTrailId
                //                        WHERE  JobAuditTrailDetail.JobClass ='DAILY' AND JobAuditTrailDetail.Status='PROCESSING'
                //                     ORDER BY JobAuditTrailDetail.LogTagNo ";

                //command.CommandText = @" SELECT DISTINCT JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.LogTagNo, JobAuditTrailDetail.JobClass, JobAuditTrailDetail.ProductName,JobAuditTrailDetail.Status,JobInstruction.Id,JobInstruction.JobType, JobAuditTrailDetal.JobAuditTrailId
                //                         FROM  JobInstruction INNER JOIN
                //                         JobAuditTrailDetail ON JobInstruction.JobSheetNo = JobAuditTrailDetail.JobSheetNo

                //                        WHERE JobAuditTrailDetail.JobClass != 'DAILY' AND JobAuditTrailDetail.Status = 'PROCESSING'

                //                        ORDER BY JobAuditTrailDetail.LogTagNo";

                command.CommandText = @" SELECT 
                                        JobAuditTrailDetail.Customer_Name,
                                        JobAuditTrailDetail.LogTagNo,
                                        MAX(JobAuditTrailDetail.JobClass) AS JobClass,
                                        MAX(JobAuditTrailDetail.ProductName) AS ProductName,
                                        MAX(JobAuditTrailDetail.Status) AS Status,
                                        MAX(JobAuditTrailDetail.JobAuditTrailId) AS JobAuditTrailId,
                                        MAX(JobAuditTrailDetail.JobType) AS JobType,
                                        MAX(JobAuditTrailDetail.CreateByIt) AS CreateByIt,
                                        MAX(JobAuditTrailDetail.CreatedOn) AS CreatedOn,
                                        JobAuditTrailDetail.JobSheetNo,
                                        JobInstruction.PaperType,
                                        JobAuditTrailDetail.Remarks
                                    FROM 
                                        JobInstruction 
                                    INNER JOIN
                                        JobAuditTrailDetail ON JobInstruction.JobSheetNo = JobAuditTrailDetail.JobSheetNo
                                    WHERE 
                                        (JobAuditTrailDetail.JobClass !='DAILY' AND JobAuditTrailDetail.JobClass NOT LIKE '%DAILY-%') 
                                        AND(JobAuditTrailDetail.Status IN('PROCESSING', 'QM : Need correction from ITO', 'MBD : Need correction from ITO', 'Planner : Need correction from ITO', 'Printing : Need correction from ITO', 'Inserting : Need correction from ITO', 'SelfMailer : Need correction from ITO', 'MMP : Need correction from ITO'))
                                    GROUP BY 
                                        JobAuditTrailDetail.LogTagNo, JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.JobSheetNo,JobInstruction.PaperType,JobAuditTrailDetail.Remarks
                                    ORDER BY 
                                        JobAuditTrailDetail.LogTagNo;";




                //OR JobAuditTrailDetail.Status = 'PLANNER' OR JobAuditTrailDetail.Status = 'PRODUCTION'
                //WHERE JobAuditTrailDetail.JobClass = 'MONTHLY' OR JobAuditTrailDetail.JobClass = 'YEARLY' AND JobAuditTrailDetail.Status = 'PROCESSING'

            }

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                List<string> totalAll = TotalAll(reader.GetString(1));

                JobInstruction model = new JobInstruction();
                {
                    model.Bil = _bil++;

                    if (reader.IsDBNull(0) == false)
                    {
                        model.Customer_Name = reader.GetString(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        model.LogTagNo = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        model.JobClass = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        model.ProductName = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        model.Status = reader.GetString(4);
                        ViewBag.Status = reader.GetString(4);
                    }
                    //if (reader.IsDBNull(5) == false)
                    //{
                    //    model.JobAuditTrailId = reader.GetGuid(5);
                    //}
                    if (reader.IsDBNull(6) == false)
                    {
                        model.JobType = reader.GetString(6);
                    }

                    if (reader.IsDBNull(7) == false)
                    {
                        model.CreateUser = reader["CreateByIt"].ToString();
                    }
                    model.AccQty = totalAll[0];
                    model.PageQty = totalAll[1];
                    model.ImpQty = totalAll[2];

                    if (reader.IsDBNull(8) == false)
                    {
                        model.CreatedOn = reader["CreatedOn"].ToString();
                    }

                    if (reader.IsDBNull(9) == false)
                    {
                        model.JobSheetNo = reader["JobSheetNo"].ToString();
                        ViewBag.JobSheetNo = reader.GetString(9);
                    }
                    if (reader.IsDBNull(10) == false)
                    {
                        model.Paper = reader.GetString(10);
                    }
                    if (reader.IsDBNull(11) == false)
                    {
                        model.Remarks = reader.GetString(11);
                    }


                    //if (reader.IsDBNull(10) == false)
                    //{
                    //    model.Id = reader.GetGuid(10);
                    //}

                    //if (reader.IsDBNull(10) == false)
                    //{
                    //    model.JobAuditTrailId = reader.GetGuid(10);
                    //}

                    model.TotalRecord = TotalAuditTrail(reader.GetString(1));

                }
                JobInstructionlist1.Add(model);
            }


            //while (reader.Read())
            //{
            //    JobInstruction model = new JobInstruction();
            //    {
            //        model.Bil = _bil++;

            //        if (reader.IsDBNull(0) == false)
            //        {
            //            model.Customer_Name = reader.GetString(0);
            //        }
            //        if (reader.IsDBNull(1) == false)
            //        {
            //            model.LogTagNo = reader.GetString(1);
            //        }
            //        if (reader.IsDBNull(2) == false)
            //        {
            //            model.JobClass = reader.GetString(2);
            //        }
            //        if (reader.IsDBNull(3) == false)
            //        {
            //            model.ProductName = reader.GetString(3);
            //        }

            //        if (reader.IsDBNull(4) == false)
            //        {
            //            model.Status = reader.GetString(4);

            //        }
            //        if (reader.IsDBNull(5)==false)
            //        {
            //            model.Id = reader.GetGuid(5);
            //        }
            //        if (reader.IsDBNull(6) == false)
            //        {
            //            model.JobType = reader.GetString(6);
            //        }
            //        if (reader.IsDBNull(7) == false)
            //        {
            //            model.JobAuditTrailId = reader.GetGuid(7);
            //        }


            //    }
            //    JobInstructionlist1.Add(model);
            //}


            cn.Close();
        }

        return View(JobInstructionlist1);
    }



    //public ActionResult ManageJobAuditTrail2(string set, string ProductName)
    //{
    //    ViewBag.IsDepart = @Session["Department"];
    //    ViewBag.IsRole = @Session["Role"];
    //    var IdentityName = @Session["Fullname"];


    //    List<JobAuditTrailDetail> View2 = new List<JobAuditTrailDetail>();


    //    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
    //    using (SqlCommand command = new SqlCommand("", cn))
    //    {
    //        int _bil = 1;
    //        cn.Open();
    //        if (set == "search")
    //        {
    //            command.CommandText = @"SELECT [Id],[CreatedOn] ,[ModifiedOn] ,[JobSheetNo] ,[Customer_Name] ,[ProductName] ,[CreateUser] ,[Company] ,[Status]
    //                                  ,[LogTagNo] ,[AccountsQty] ,[ImpressionQty],[PagesQty],[ProgramName],[Path],[ModeLog]                                              
    //                                    FROM [dbo].[JobAuditTrailDetail] 
    //                                    ProductName LIKE @ProductName
    //                                    ORDER BY CreatedOn desc";
    //            command.Parameters.AddWithValue("@ProductName", "%" + ProductName + "%");

    //        }

    //        else
    //        {
    //            command.CommandText = @"  SELECT [Id],[CreatedOn] ,[ModifiedOn] ,[JobSheetNo] ,[Customer_Name] ,[ProductName] ,[CreateUser] ,[Company] ,[Status]
    //                                  ,[LogTagNo] ,[AccountsQty] ,[ImpressionQty],[PagesQty],[ProgramName] ,[Path],[ModeLog]                                        
    //                                    FROM [dbo].[JobAuditTrailDetail]                                             
    //                                    WHERE  Status ='Waiting Approval'
    //                                    ORDER BY CreatedOn desc";

    //        }

    //        var reader = command.ExecuteReader();
    //        while (reader.Read())
    //        {
    //            JobAuditTrailDetail model = new JobAuditTrailDetail();
    //            {
    //                model.Bil = _bil++;
    //                if (reader.IsDBNull(0) == false)
    //                {
    //                    model.Id = reader.GetGuid(0);
    //                }
    //                if (reader.IsDBNull(1) == false)
    //                {
    //                    model.CreatedOn = reader.GetDateTime(1);
    //                }
    //                if (reader.IsDBNull(2) == false)
    //                {
    //                    model.ModifiedOn = reader.GetDateTime(2);
    //                }
    //                if (reader.IsDBNull(3) == false)
    //                {
    //                    model.JobSheetNo = reader.GetString(3);
    //                }
    //                if (reader.IsDBNull(4) == false)
    //                {
    //                    model.Customer_Name = reader.GetString(4);
    //                }
    //                if (reader.IsDBNull(5) == false)
    //                {
    //                    model.ProductName = reader.GetString(5);
    //                }
    //                if (reader.IsDBNull(6) == false)
    //                {
    //                    model.CreateUser = reader.GetString(6);
    //                }
    //                if (reader.IsDBNull(7) == false)
    //                {
    //                    model.Company = reader.GetString(7);
    //                }
    //                if (reader.IsDBNull(8) == false)
    //                {
    //                    model.Status = reader.GetString(8);
    //                }
    //                if (reader.IsDBNull(9) == false)
    //                {
    //                    model.LogTagNo = reader.GetString(9);
    //                }
    //                if (reader.IsDBNull(10) == false)
    //                {
    //                    model.AccountsQty = reader.GetString(10);
    //                }
    //                if (reader.IsDBNull(11) == false)
    //                {
    //                    model.ImpressionQty = reader.GetString(11);
    //                }
    //                if (reader.IsDBNull(12) == false)
    //                {
    //                    model.PagesQty = reader.GetString(12);
    //                }

    //                if (reader.IsDBNull(13) == false)
    //                {
    //                    model.ProgramName = reader.GetString(13);
    //                }
    //                if (reader.IsDBNull(14) == false)
    //                {
    //                    model.Path = reader.GetString(14);
    //                }
    //                if (reader.IsDBNull(15) == false)
    //                {
    //                    model.ModeLog = reader.GetString(15);
    //                }

    //            }
    //            View2.Add(model);
    //        }
    //        cn.Close();
    //    }

    //    return View(View2);

    //}



    public ActionResult CreateLogTagNo(string set, string Set, string Id, string JobAuditTrailId, JobAuditTrailDetail get,
                                       string JobRequest, string Customer_Name, string ProductName, string JobSheetNo, string JobClass,
                                       string Frequency, string JobType, string LogTagNo, string TotalAuditTrail, string CreateByIT, string Status,
                                       string ModeLog, string Path, string JobNameIT, string JobId, string ProgramId, string FileId,
                                       string RevStrtDateOn, string RevStrtTime, string DateProcessItOn, string TimeProcessIt,
                                       string DateApproveOn, string DateApproveTime, string AccountsQty, string ImpressionQty, string PagesQty,
                                       string FirstRecord, string LastRecord, string Remarks, string Type,
                                       string NotesByIT, string NotesByProduction, string NotesByPurchasing,
                                       string NotesByEngineering, string NotesByArtwork, string NotesByFinance, string NotesByDCP, string JobInstructionId,
                                       string ImageInDateOn, string ImageInTime, string RevisedInDateOn)

    {

        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.AccountManager = IdentityName.ToString();
        ViewBag.AccountsQty = AccountsQty;
        ViewBag.PagesQty = PagesQty;
        ViewBag.ImpressionQty = ImpressionQty;
        ViewBag.JobAuditTrailId = JobAuditTrailId;
        Session["JobAuditTrailId"] = Id;
        Session["Id"] = Id;
        ViewBag.Id = Id;
        ViewBag.JobInstructionId = JobInstructionId;
        ViewBag.LogTagNo = LogTagNo;
        ViewBag.Customer_Name = Customer_Name;
        ViewBag.ProductName = ProductName;



        if (set == "LogTag")
        {
            if (string.IsNullOrEmpty(LogTagNo))
            {
                var No_ = new NoCounterModel();
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                    cn.Open();
                    SqlCommand command;
                    command = new SqlCommand("UPDATE [dbo].[JobAuditTrail] SET ModifiedOn=@ModifiedOn, LogTagNo=@LogTagNo, CreateByIt=@CreateByIt WHERE Id=@Id", cn);
                    command.Parameters.AddWithValue("@ModifiedOn", createdOn);
                    command.Parameters.AddWithValue("@LogTagNo", No_.RefNo);
                    command.Parameters.AddWithValue("@CreateByIt", IdentityName.ToString());
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                    cn.Close();
                }
                //bila save akn gi kt managecustomer
                return RedirectToAction("ManageJobAuditTrail", "ITO", new { Id = Id });

            }
            else
            {
                TempData["msg"] = "<script>alert('LOG TAG NO ALREADY CREATED !');</script>";
                return RedirectToAction("ManageJobAuditTrail", "ITO", new { Id = Id });
            }
        }



        if (set == "AddNew")
        {
            if (!string.IsNullOrEmpty(LogTagNo))
            {


                return View();

            }
            else
            {
                TempData["msg"] = "<script>alert('THIS ACTION CANNOT BE PROCEED WITHOUT LOG TAG NO !');</script>";
                return RedirectToAction("ManageJobAuditTrail", "ITO", new { Id = Id });
            }
        }

        return View();

    }

    public ActionResult ManualCreateLogTagNo(string set, string Set, string Id, string JobAuditTrailId, JobAuditTrailDetail get,
                                      string JobRequest, string Customer_Name, string ProductName, string JobSheetNo, string JobClass,
                                      string Frequency, string JobType, string LogTagNo, string TotalAuditTrail, string CreateByIT, string Status,
                                      string ModeLog, string Path, string JobNameIT, string JobId, string ProgramId, string FileId,
                                      string RevStrtDateOn, string RevStrtTime, string DateProcessItOn, string TimeProcessIt,
                                      string DateApproveOn, string DateApproveTime, string AccountsQty, string ImpressionQty, string PagesQty,
                                      string FirstRecord, string LastRecord, string Remarks, string Type,
                                      string NotesByIT, string NotesByProduction, string NotesByPurchasing,
                                      string NotesByEngineering, string NotesByArtwork, string NotesByFinance, string NotesByDCP, string JobInstructionId,
                                      string ImageInDateOn, string ImageInTime, string RevisedInDateOn, string Idx)


    {

        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.AccountManager = IdentityName.ToString();
        ViewBag.AccountsQty = AccountsQty;
        ViewBag.PagesQty = PagesQty;
        ViewBag.ImpressionQty = ImpressionQty;
        ViewBag.JobAuditTrailId = JobAuditTrailId;
        Session["JobAuditTrailId"] = Id;
        Session["Id"] = Id;
        ViewBag.Id = Id;
        ViewBag.JobInstructionId = JobInstructionId;
        ViewBag.LogTagNo = LogTagNo;
        ViewBag.Customer_Name = Customer_Name;
        ViewBag.ProductName = ProductName;


        if (set == "Manual")
        {

            if (string.IsNullOrEmpty(LogTagNo))
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    cn.Open();
                    command.CommandText = @"SELECT JobAuditTrailDetail.Id, JobAuditTrailDetail.LogTagNo
                                            FROM  JobInstruction INNER JOIN
                                             JobAuditTrailDetail ON JobInstruction.Id = JobAuditTrailDetail.JobInstructionId ";
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
                            ViewBag.LogTagNo = reader.GetString(1);
                        }

                    }
                    cn.Close();
                }

                return View();

            }
            else
            {

                TempData["msg"] = "<script>alert('LOG TAG NO ALREADY CREATED !');</script>";
                return RedirectToAction("ManageJobAuditTrail", "ITO");

            }

        }
        else
        {
            if (!string.IsNullOrEmpty(LogTagNo))
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                    cn.Open();
                    SqlCommand command;
                    command = new SqlCommand("UPDATE [dbo].[JobAuditTrailDetail] SET ModifiedOn=@ModifiedOn, LogTagNo=@LogTagNo, CreateByIt=@CreateByIt WHERE Id=@Id", cn);
                    command.Parameters.AddWithValue("@ModifiedOn", createdOn);
                    command.Parameters.AddWithValue("@LogTagNo", LogTagNo);
                    command.Parameters.AddWithValue("@CreateByIt", IdentityName.ToString());
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                    cn.Close();
                }
                //bila save akn gi kt managecustomer
                return RedirectToAction("ManageJAT", "ITO");
            }
            else
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    cn.Open();
                    command.CommandText = @"SELECT JobAuditTrailDetail.Id, JobAuditTrailDetail.LogTagNo
                                          FROM  JobInstruction INNER JOIN
                                          JobAuditTrailDetail ON JobInstruction.Id = JobAuditTrailDetail.JobInstructionId
                                    WHERE JobAuditTrailDetail.Id=@Id ";
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
                            ViewBag.LogTagNo = reader.GetString(1);
                        }

                    }
                    cn.Close();
                }
                if (set == "LogTag")
                {
                    if (string.IsNullOrEmpty(LogTagNo))
                    {
                        Session["Id"] = Id;
                        ViewBag.Id = Id;
                        var No_ = new NoCounterModel();
                        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                            cn.Open();
                            SqlCommand command;
                            command = new SqlCommand("UPDATE [dbo].[JobAuditTrailDetail] SET ModifiedOn=@ModifiedOn, LogTagNo=@LogTagNo, CreateByIt=@CreateByIt WHERE Id=@Id", cn);
                            command.Parameters.AddWithValue("@ModifiedOn", createdOn);
                            command.Parameters.AddWithValue("@LogTagNo", No_.RefNo);
                            command.Parameters.AddWithValue("@CreateByIt", IdentityName.ToString());
                            command.Parameters.AddWithValue("@Id", Id);
                            command.ExecuteNonQuery();
                            cn.Close();
                        }
                        //bila save akn gi kt managecustomer
                        return RedirectToAction("ManageJAT", "ITO");

                    }
                    else
                    {
                        TempData["msg"] = "<script>alert('LOG TAG ALREADY CREATED !');</script>";
                        return RedirectToAction("ManageJAT", "ITO");
                    }
                }


                return View();
            }


        }



    }

    public ActionResult DeleteJobAuditTrail(string LogTagNo)
    {
        if (!string.IsNullOrEmpty(LogTagNo))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("DELETE [dbo].[JobAuditTrailDetail] WHERE LogTagNo=@LogTagNo", cn);
                command.Parameters.AddWithValue("@LogTagNo", LogTagNo);
                command.ExecuteNonQuery();
                cn.Close();
            }

            //using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            //{
            //    cn.Open();
            //    SqlCommand command;
            //    command = new SqlCommand("DELETE [dbo].[JobAuditTrailDetail] WHERE JobAuditTrailId=@JobAuditTrailId", cn);
            //    command.Parameters.AddWithValue("@JobAuditTrailId", Id);
            //    command.ExecuteNonQuery();
            //    cn.Close();
            //}
        }
        return RedirectToAction("ManageJAT", "ITO");
    }

    public ActionResult AddJAT(JobAuditTrailDetail ModelSample, DailyTracking get2, JobAuditTrailDetail get, string Id, string JobAuditTrailId, string set, string Set,
                                            string JobRequest, string Customer_Name, string ProductName, string JobSheetNo, string JobClass,
                                            string Frequency, string JobType, string LogTagNo, string TotalAuditTrail, string CreateByIT, string Status,
                                            string Path, string JobNameIT, string JobId, string ProgramId, string FileId,
                                            string RevStrtDateOn, string RevStrtTime, string DateProcessItOn, string TimeProcessIt,
                                            string DateApproveOn, string DateApproveTime, string AccountsQty, string ImpressionQty, string PagesQty,
                                            string FirstRecord, string LastRecord, string Remark, string Type,
                                            string NotesByIT, string NotesByProduction, string NotesByPurchasing,
                                            string NotesByEngineering, string NotesByArtwork, string NotesByFinance, string NotesByDCP, string JobInstructionId,
                                            string ImageInDateOn, string ImageInTime, string RevisedInDateOn, string IdDT,
                                            string AccQty, string ImpQty, string PageQty, string status, string Cust_Department, string timeEndProcessIt, string revStrtDateOn, string dateApproveOn, string imageInDateOn, 
                                            string revisedInDateOn, string processDate, string processEnd, string TimeTaken, string RevEndDateOn, string RevEndTimeOn, string PaperType,string msg)
    {
        Debug.WriteLine("RevEndTimeOn : " + RevEndTimeOn);
        // = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
        // = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
        // = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
        // = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

        // = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
        // = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

        // = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
        // = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
        var IdentityName = @Session["Fullname"];
        Session["Id"] = Id;
        ViewBag.Id = Id;
        ViewBag.JobInstructionId = JobInstructionId;
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.IsRole = @Session["Role"];
        string Deptment = @Session["Department"].ToString();
        ViewBag.Customer_Name = Customer_Name;
        ViewBag.ProductName = ProductName;
        ViewBag.LogTagNo = LogTagNo;

        if(!string.IsNullOrEmpty(msg))
        {
            ViewBag.Msg = "<script>alert('"+ msg + "');</script>";
        }


        List<SelectListItem> li9 = new List<SelectListItem>();
        List<JobAuditTrailDetail> listJob = new List<JobAuditTrailDetail>();


        li9.Add(new SelectListItem { Text = "Please Select", Value = " " });
        li9.Add(new SelectListItem { Text = "LOAN", Value = "LOAN" });
        li9.Add(new SelectListItem { Text = "RETURN TO BRANCH", Value = "RETURN TO BRANCH" });
        li9.Add(new SelectListItem { Text = "REGISTER MAIL", Value = "REGISTER MAIL" });
        li9.Add(new SelectListItem { Text = "POSTING", Value = "POSTING" });
        li9.Add(new SelectListItem { Text = "COLOUR PRINTING", Value = "COLOUR PRINTING" });
        li9.Add(new SelectListItem { Text = "COURIER G-DEX", Value = "COURIER G-DEX" });
        li9.Add(new SelectListItem { Text = "HOME STMT", Value = "HOME STMT" });
        li9.Add(new SelectListItem { Text = "RETURN", Value = "RETURN" });
        li9.Add(new SelectListItem { Text = "HOLD", Value = "HOLD" });
        li9.Add(new SelectListItem { Text = "COURIER", Value = "COURIER" });
        li9.Add(new SelectListItem { Text = "RE-PRINT", Value = "RE-PRINT" });
        li9.Add(new SelectListItem { Text = "MORE > 6 PAGES", Value = "MORE > 6 PAGES" });
        li9.Add(new SelectListItem { Text = "BCP Activity", Value = "BCP Activity" });
        ViewData["Type_"] = li9;

        if (!string.IsNullOrEmpty(Id))
        {

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();

                using (SqlCommand FirstLoad = new SqlCommand("", cn))
                {
                    if (Id == "00000000-0000-0000-0000-000000000000")
                    {
                        FirstLoad.CommandText = "SELECT TOP(1) Id, Customer_Name, ProductName, JobClass, JobType,Cust_Department,LogTagNo,ProgramId,AccQty,ImpQty,PageQty,JobNameIT,FirstRecord,LastRecord," +
                            "ModeLog,Path,JobId,ProgramId,FileId,FORMAT(CONVERT(date, RevStrtDateOn), 'yyyy-MM-dd'),RevStrtTime,FORMAT(CONVERT(date, ProcessDate), 'yyyy-MM-dd'),TimeProcessIt,FORMAT(CONVERT(date, ProcessEnd), 'yyyy-MM-dd'),TimeEndProcessIt,FORMAT(CONVERT(date, DateApproveOn), 'yyyy-MM-dd'),DateApproveTime,FORMAT(CONVERT(date, ImageInDateOn), 'yyyy-MM-dd'),ImageInTime,FORMAT(CONVERT(date, RevisedInDateOn), 'yyyy-MM-dd')," +
                            "Type,Remark,CreatedOn,JobSheetNo, CreateByIT, FORMAT(CONVERT(date, RevEndDateOn), 'yyyy-MM-dd'),RevEndTimeOn FROM [dbo].[JobAuditTrailDetail] WHERE LogTagNo=@LogTagNo1";
                        FirstLoad.Parameters.AddWithValue("LogTagNo1", LogTagNo);



                    }
                    else
                    {
                        //FirstLoad.CommandText = "SELECT Customer_Name, ProductName, LogTagNo,JobNameIT,ModeLog,JobId,JobAuditTrailId, Id, AccQty,ImpQty,PageQty,JobRequest,JobClass, JobSheetNo, JobType FROM [dbo].[JobAuditTrailDetail] WHERE LogTagNo=@LogTagNo1 AND Id=@Id1";
                        FirstLoad.CommandText = "SELECT Id, Customer_Name, ProductName, JobClass, JobType,Cust_Department,LogTagNo,ProgramId,AccQty,ImpQty,PageQty,JobNameIT,FirstRecord,LastRecord," +
                            "ModeLog,Path,JobId,ProgramId,FileId,FORMAT(CONVERT(date, RevStrtDateOn), 'yyyy-MM-dd'),RevStrtTime,FORMAT(CONVERT(date, ProcessDate), 'yyyy-MM-dd'),TimeProcessIt,FORMAT(CONVERT(date, ProcessEnd), 'yyyy-MM-dd'),TimeEndProcessIt,FORMAT(CONVERT(date, DateApproveOn), 'yyyy-MM-dd'),DateApproveTime,FORMAT(CONVERT(date, ImageInDateOn), 'yyyy-MM-dd'),ImageInTime,FORMAT(CONVERT(date, RevisedInDateOn), 'yyyy-MM-dd')," +
                            "Type,Remark,CreatedOn,JobSheetNo, CreateByIT, FORMAT(CONVERT(date, RevEndDateOn), 'yyyy-MM-dd'), RevEndTimeOn FROM [dbo].[JobAuditTrailDetail] WHERE LogTagNo=@LogTagNo1 AND Id=@Id1";

                        FirstLoad.Parameters.AddWithValue("LogTagNo1", LogTagNo);
                        FirstLoad.Parameters.AddWithValue("Id1", Id);

                    }

                    var rmFirstLoad = FirstLoad.ExecuteReader();

                    while (rmFirstLoad.Read())
                    {
                        if (rmFirstLoad.IsDBNull(0) == false)
                        {
                            ViewBag.Id = rmFirstLoad.GetGuid(0);
                        }
                        if (rmFirstLoad.IsDBNull(1) == false)
                        {
                            ViewBag.Customer_Name = rmFirstLoad.GetString(1);
                        }
                        if (rmFirstLoad.IsDBNull(2) == false)
                        {
                            ViewBag.ProductName = rmFirstLoad.GetString(2);
                        }
                        if (rmFirstLoad.IsDBNull(3) == false)
                        {
                            ViewBag.JobClass = rmFirstLoad.GetString(3);
                        }
                        if (rmFirstLoad.IsDBNull(4) == false)
                        {
                            ViewBag.JobType = rmFirstLoad.GetString(4);
                        }

                        if (rmFirstLoad.IsDBNull(5) == false)
                        {
                            ViewBag.Cust_Department = rmFirstLoad.GetString(5);
                        }
                        if (rmFirstLoad.IsDBNull(6) == false)
                        {
                            ViewBag.LogTagNo = rmFirstLoad.GetString(6);
                        }
                        if (rmFirstLoad.IsDBNull(7) == false)
                        {
                            ViewBag.ProgramId = rmFirstLoad.GetString(7);
                        }

                        if (rmFirstLoad.IsDBNull(8) == false)
                        {
                            ViewBag.AccountQty = rmFirstLoad.GetString(8);
                        }

                        if (rmFirstLoad.IsDBNull(9) == false)
                        {
                            ViewBag.ImpressionQty = rmFirstLoad.GetString(9);
                        }
                        if (rmFirstLoad.IsDBNull(10) == false)
                        {
                            ViewBag.PagesQty = rmFirstLoad.GetString(10);
                        }
                        if (rmFirstLoad.IsDBNull(11) == false)
                        {
                            ViewBag.JobNameIT = rmFirstLoad.GetString(11);
                        }

                        if (rmFirstLoad.IsDBNull(12) == false)
                        {
                            ViewBag.FirstRecord = rmFirstLoad.GetString(12);
                        }
                        if (rmFirstLoad.IsDBNull(13) == false)
                        {
                            ViewBag.LastRecord = rmFirstLoad.GetString(13);
                        }
                        if (rmFirstLoad.IsDBNull(14) == false)
                        {
                            ViewBag.ModeLog = rmFirstLoad.GetString(14);
                        }
                        if (rmFirstLoad.IsDBNull(15) == false)
                        {
                            ViewBag.Path = rmFirstLoad.GetString(15);
                        }
                        if (rmFirstLoad.IsDBNull(16) == false)
                        {
                            ViewBag.JobId = rmFirstLoad.GetString(16);
                        }
                        if (rmFirstLoad.IsDBNull(17) == false)
                        {
                            ViewBag.ProgramId = rmFirstLoad.GetString(17);
                        }
                        if (rmFirstLoad.IsDBNull(18) == false)
                        {
                            ViewBag.FileId = rmFirstLoad.GetString(18);
                        }
                        if (rmFirstLoad.IsDBNull(19) == false)
                        {
                            //ViewBag.RevStrtDateOn = String.Format("{0:dd/MM/yyyy }", (DateTime)rmFirstLoad.GetDateTime(19));
                            ViewBag.RevStrtDateOn = rmFirstLoad.GetString(19);
                        }
                        if (rmFirstLoad.IsDBNull(20) == false)
                        {
                            ViewBag.RevStrtTime = rmFirstLoad.GetString(20);
                        }
                        if (rmFirstLoad.IsDBNull(21) == false)
                        {
                            //ViewBag.ProcessDate = String.Format("{0:dd/MM/yyyy }", (DateTime)rmFirstLoad.GetDateTime(21));
                            ViewBag.ProcessDate = rmFirstLoad.GetString(21);
                        }
                        if (rmFirstLoad.IsDBNull(22) == false)
                        {
                            ViewBag.TimeProcessIt = rmFirstLoad.GetString(22);
                        }
                        if (rmFirstLoad.IsDBNull(23) == false)
                        {
                            //ViewBag.ProcessEnd = String.Format("{0:dd/MM/yyyy }", (DateTime)rmFirstLoad.GetDateTime(23));
                            ViewBag.ProcessEnd = rmFirstLoad.GetString(23);
                        }
                        if (rmFirstLoad.IsDBNull(24) == false)
                        {
                            ViewBag.TimeEndProcessIt = rmFirstLoad.GetString(24);
                        }
                        if (rmFirstLoad.IsDBNull(25) == false)
                        {
                            //ViewBag.DateApproveOn = String.Format("{0:dd/MM/yyyy }", (DateTime)rmFirstLoad.GetDateTime(25));
                            ViewBag.DateApproveOn = rmFirstLoad.GetString(25);
                        }
                        if (rmFirstLoad.IsDBNull(26) == false)
                        {
                            ViewBag.DateApproveTime = rmFirstLoad.GetString(26);
                        }
                        if (rmFirstLoad.IsDBNull(27) == false)
                        {
                            ViewBag.ImageInDateOn = rmFirstLoad.GetString(27);

                            //ViewBag.ImageInDateOn = String.Format("{0:dd/MM/yyyy }", (DateTime)rmFirstLoad.GetDateTime(28));
                        }
                        if (rmFirstLoad.IsDBNull(28) == false)
                        {
                            ViewBag.ImageInTime = rmFirstLoad.GetString(28);
                        }
                        if (rmFirstLoad.IsDBNull(29) == false)
                        {
                            //ViewBag.RevisedInDateOn = String.Format("{0:dd/MM/yyyy }", (DateTime)rmFirstLoad.GetDateTime(29));
                            ViewBag.RevisedInDateOn = rmFirstLoad.GetString(29);
                        }
                        if (rmFirstLoad.IsDBNull(30) == false)
                        {
                            ViewBag.Type = rmFirstLoad.GetString(30);
                        }
                        if (rmFirstLoad.IsDBNull(31) == false)
                        {
                            ViewBag.Remark = rmFirstLoad.GetString(31);
                        }
                        if (rmFirstLoad.IsDBNull(32) == false)
                        {
                            ViewBag.CreatedOn = rmFirstLoad.GetDateTime(32);
                        }
                        if (rmFirstLoad.IsDBNull(33) == false)
                        {
                            ViewBag.JobSheetNo = rmFirstLoad.GetString(33);
                            JobSheetNo = rmFirstLoad.GetString(33);
                        }
                        if (rmFirstLoad.IsDBNull(34) == false)
                        {
                            ViewBag.CreateByIT = rmFirstLoad.GetString(34);
                        }
                        if (rmFirstLoad.IsDBNull(35) == false)
                        {
                            ViewBag.RevEndDateOn = rmFirstLoad.GetString(35);
                        }
                        if (rmFirstLoad.IsDBNull(36) == false)
                        {
                            ViewBag.RevEndTimeOn = rmFirstLoad.GetString(36);
                        }

                    }

                    SqlCommand GetPaperType = new SqlCommand("SELECT PaperType FROM JobInstruction WHERE JobSheetNo = @PaperTypeJS", cn);
                    GetPaperType.Parameters.AddWithValue("@PaperTypeJS", JobSheetNo);
                    SqlDataReader rmPaperType = GetPaperType.ExecuteReader();

                    if(rmPaperType.HasRows)
                    {
                        while(rmPaperType.Read())
                        {
                            if(!rmPaperType.IsDBNull(0))
                            {
                                ViewBag.PaperType = rmPaperType.GetString(0);
                                PaperType= rmPaperType.GetString(0);
                            }
                            else
                            {
                                ViewBag.PaperType = "-";
                            }
                        }
                    }



                    SqlCommand LoadTotal = new SqlCommand("SELECT COUNT(LogTagNo) FROM [dbo].[JobAuditTrailDetail] WHERE LogTagNo=@LogTagNoLoadTotal", cn);
                    LoadTotal.Parameters.AddWithValue("LogTagNoLoadTotal", LogTagNo);
                    SqlDataReader rmLoadTotal = LoadTotal.ExecuteReader();

                    while (rmLoadTotal.Read())
                    {
                        if (rmLoadTotal.GetInt32(0) > 1)
                        {
                            SqlCommand LoadTotal2 = new SqlCommand("SELECT SUM(CAST(AccQty AS INT)) AS AccQty, SUM(CAST(ImpQty AS INT)) AS ImpQty, SUM(CAST(PageQty AS INT)) AS PageQty  FROM [dbo].[JobAuditTrailDetail] WHERE LogTagNo=@LogTagNoLoadTotal2", cn);
                            LoadTotal2.Parameters.AddWithValue("LogTagNoLoadTotal2", LogTagNo);
                            SqlDataReader rmLoadTotal2 = LoadTotal2.ExecuteReader();

                            while (rmLoadTotal2.Read())
                            {
                                if (!rmLoadTotal2.IsDBNull(0))
                                {
                                    ViewBag.TotalAccQty = rmLoadTotal2.GetInt32(0);

                                }
                                else
                                {
                                    ViewBag.TotalAccQty = "0";

                                }

                                if (!rmLoadTotal2.IsDBNull(1))
                                {
                                    ViewBag.TotalImpQty = rmLoadTotal2.GetInt32(1);

                                }
                                else
                                {
                                    ViewBag.TotalImpQty = "0";

                                }

                                if (!rmLoadTotal2.IsDBNull(2))
                                {
                                    ViewBag.TotalPageQty = rmLoadTotal2.GetInt32(2);

                                }
                                else
                                {
                                    ViewBag.TotalPageQty = "0";

                                }

                            }

                        }
                    }




                    //while (rmFirstLoad.Read())
                    //{
                    //    if (rmFirstLoad.IsDBNull(0) == false)
                    //    {
                    //        ViewBag.Customer_Name = rmFirstLoad.GetString(0);
                    //    }
                    //    if (rmFirstLoad.IsDBNull(1) == false)
                    //    {
                    //        ViewBag.ProductName = rmFirstLoad.GetString(1);
                    //    }
                    //    if (rmFirstLoad.IsDBNull(2) == false)
                    //    {
                    //        ViewBag.LogTagNo = rmFirstLoad.GetString(2);
                    //    }
                    //    if (rmFirstLoad.IsDBNull(3) == false)
                    //    {
                    //        ViewBag.JobNameIT = rmFirstLoad.GetString(3);
                    //    }
                    //    if (rmFirstLoad.IsDBNull(4) == false)
                    //    {
                    //        ViewBag.ModeLog = rmFirstLoad.GetString(4);
                    //    }
                    //    if (rmFirstLoad.IsDBNull(5) == false)
                    //    {
                    //        ViewBag.JobId = rmFirstLoad.GetString(5);
                    //    }

                    //    if (rmFirstLoad.IsDBNull(8) == false)
                    //    {
                    //        ViewBag.AccQty = rmFirstLoad.GetString(8);
                    //    }
                    //    if (rmFirstLoad.IsDBNull(9) == false)
                    //    {
                    //        ViewBag.ImpQty = rmFirstLoad.GetString(9);
                    //    }
                    //    if (rmFirstLoad.IsDBNull(10) == false)
                    //    {
                    //        ViewBag.PageQty = rmFirstLoad.GetString(10);
                    //    }

                    //}
                }


                using (SqlCommand command = new SqlCommand("", cn))
                {

                    int _bil = 1;

                    //Id,ModifiedOn, Customer_Name, ProductName, JobSheetNo, JobClass, 
                    //                               Frequency, JobType, LogTagNo, AccountsQty, ImpressionQty, PagesQty, 
                    //                               TotalAuditTrail,  CreateByIT, Status, ModeLog, Path,JobNameIT,JobId

                    //command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass, JobType,Cust_Department,LogTagNo,ProgramId,AccQty,ImpQty,PageQty,JobNameIT,FirstRecord,LastRecord,ModeLog,Path,JobNameIT,JobId,ProgramId,FileId,RevStrtDateOn,RevStrtTime,ProcessDate,TimeProcessIt,ProcessEnd,TimeEndProcessIt,DateApproveOn,DateApproveTime
                    //                       ,ImageInDateOn,ImageInTime,RevisedInDateOn,Type,Remark,Customer_Name
                    //                                 FROM [dbo].[JobAuditTrailDetail]                                  
                    //                                 WHERE Id=@Id ";
                    Debug.WriteLine("ID First Load : " + Id);
                    command.CommandText = @"SELECT Customer_Name, ProductName, LogTagNo,JobNameIT,ModeLog,JobId,JobAuditTrailId, Id, AccQty,ImpQty,PageQty,JobRequest,JobClass, JobSheetNo, JobType,FileId
                                                 FROM [dbo].[JobAuditTrailDetail]                                  
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
                            if (reader.IsDBNull(3) == false)
                            {
                                model.JobNameIT = reader.GetString(3);
                            }
                            if (reader.IsDBNull(5) == false)
                            {
                                model.JobId = reader.GetString(5);
                            }
                            //if (reader.IsDBNull(6) == false)
                            //{
                            //    model.JobAuditTrailId = reader.GetGuid(6);
                            //}
                            if (reader.IsDBNull(7) == false)
                            {
                                model.Id = reader.GetGuid(7);
                            }
                            if (reader.IsDBNull(8) == false)
                            {
                                model.AccountsQty = reader.GetString(8);
                            }
                            if (reader.IsDBNull(9) == false)
                            {
                                model.ImpressionQty = reader.GetString(9);
                            }
                            if (reader.IsDBNull(10) == false)
                            {
                                model.PagesQty = reader.GetString(10);
                            }
                            if (reader.IsDBNull(11) == false)
                            {
                                model.JobRequest = reader.GetDateTime(11);
                            }
                            if (reader.IsDBNull(12) == false)
                            {
                                model.JobClass = reader.GetString(12);
                            }
                            if (reader.IsDBNull(14) == false)
                            {
                                model.JobType = reader.GetString(14);
                            }
                            if (reader.IsDBNull(15) == false)
                            {
                                model.FileId = reader.GetString(15);
                            }

                        };

                        listJob.Add(model);

                    }

                    reader.Close();

                }

                List<DailyTracking> dailyTrackings = new List<DailyTracking>();
                using (SqlCommand getdailytracking = new SqlCommand("", cn))
                {
                    //if (Id == "00000000-0000-0000-0000-000000000000")
                    //{
                    //    getdailytracking.CommandText = "SELECT TOP(1) FORMAT(CONVERT(date, StartDateOn), 'yyyy-MM-dd'), FORMAT(CONVERT(date, EndDateOn), 'yyyy-MM-dd'),  FORMAT(CONVERT(date, ProcessStartDateOn), 'yyyy-MM-dd'), FORMAT(CONVERT(date, ProcessEndDateOn), 'yyyy-MM-dd'), " +
                    //    "FORMAT(CONVERT(date, DateApproveOn), 'yyyy-MM-dd'), FORMAT(CONVERT(date, LogTagSendOn), 'yyyy-MM-dd'), JobSheetNo, LogTagNo, Customer_Name, Cust_Department, ProductName, PIC, AccountsQty, ImpressionQty, PagesQty, StartTime, EndTime, ProcessStartTime, " +
                    //    "ProcessEndTime, TimeTaken, DateApproveTime, LogTagSendTime FROM DailyTracking Where LogTagNo=@LogTagNo";
                    //    getdailytracking.Parameters.AddWithValue("LogTagNo", LogTagNo);

                    //}
                    //else
                    //{
                    //    getdailytracking.CommandText = "SELECT TOP(1) FORMAT(CONVERT(date, StartDateOn), 'yyyy-MM-dd'), FORMAT(CONVERT(date, EndDateOn), 'yyyy-MM-dd'),  FORMAT(CONVERT(date, ProcessStartDateOn), 'yyyy-MM-dd'), FORMAT(CONVERT(date, ProcessEndDateOn), 'yyyy-MM-dd'), " +
                    //    "FORMAT(CONVERT(date, DateApproveOn), 'yyyy-MM-dd'), FORMAT(CONVERT(date, LogTagSendOn), 'yyyy-MM-dd'), JobSheetNo, LogTagNo, Customer_Name, Cust_Department, ProductName, PIC, AccountsQty, ImpressionQty, PagesQty, StartTime, EndTime, ProcessStartTime, " +
                    //    "ProcessEndTime, TimeTaken, DateApproveTime, LogTagSendTime FROM DailyTracking Where LogTagNo=@LogTagNo AND JobAuditTrail=@JAT1";
                    //    getdailytracking.Parameters.AddWithValue("LogTagNo", LogTagNo);
                    //    getdailytracking.Parameters.AddWithValue("JAT1", Id);
                    //}

                    getdailytracking.CommandText = "SELECT FORMAT(CONVERT(date, StartDateOn), 'yyyy-MM-dd'), FORMAT(CONVERT(date, EndDateOn), 'yyyy-MM-dd'),  FORMAT(CONVERT(date, ProcessStartDateOn), 'yyyy-MM-dd'), FORMAT(CONVERT(date, ProcessEndDateOn), 'yyyy-MM-dd'), " +
                       "FORMAT(CONVERT(date, DateApproveOn), 'yyyy-MM-dd'), FORMAT(CONVERT(date, LogTagSendOn), 'yyyy-MM-dd'), JobSheetNo, LogTagNo, Customer_Name, Cust_Department, ProductName, PIC, AccountsQty, ImpressionQty, PagesQty, StartTime, EndTime, ProcessStartTime, " +
                       "ProcessEndTime, TimeTaken, DateApproveTime, LogTagSendTime, Id FROM DailyTracking Where LogTagNo=@LogTagNo";
                    getdailytracking.Parameters.AddWithValue("LogTagNo", LogTagNo);
                    getdailytracking.Parameters.AddWithValue("JAT1", Id);

                    var rmdailytracking = getdailytracking.ExecuteReader();

                    while (rmdailytracking.Read())
                    {
                        var DailyTracking = new DailyTracking();
                        {
                            if (rmdailytracking.IsDBNull(0) == false)
                            {
                                //DateTime dateValue = DateTime.ParseExact(rmdailytracking.GetString(0), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                ViewBag.StartDateOn = rmdailytracking.GetString(0);
                                //DailyTracking.StartDateOn = rmdailytracking.GetString(0);
                            }
                            if (rmdailytracking.IsDBNull(1) == false)
                            {
                                ViewBag.EndDateOn = rmdailytracking.GetString(1);

                                //ViewBag.EndDateOn = rmdailytracking.GetDateTime(1); ;
                                //DailyTracking.EndDateOn = rmdailytracking.GetDateTime(1);
                            }
                            if (rmdailytracking.IsDBNull(2) == false)
                            {
                                ViewBag.ProcessStartDateOn = rmdailytracking.GetString(2); ;
                                //DailyTracking.ProcessStartDateOn = rmdailytracking.GetDateTime(2);
                            }
                            if (rmdailytracking.IsDBNull(3) == false)
                            {
                                ViewBag.ProcessEndDateOn = rmdailytracking.GetString(3); ;
                                //DailyTracking.ProcessEndDateOn = rmdailytracking.GetDateTime(3);
                            }
                            if (rmdailytracking.IsDBNull(4) == false)
                            {
                                ViewBag.DateApproveOn = rmdailytracking.GetString(4); ;
                                //DailyTracking.DateApproveOn = rmdailytracking.GetDateTime(4);
                            }
                            if (rmdailytracking.IsDBNull(5) == false)
                            {
                                ViewBag.LogTagSendOn = rmdailytracking.GetString(5); ;
                                //DailyTracking.LogTagSendOn = rmdailytracking.GetDateTime(5);
                            }
                            if (rmdailytracking.IsDBNull(6) == false)
                            {
                                //ViewBag.JobSheetNo = rmdailytracking.GetString(6); ;
                                //DailyTracking.JobSheetNo = rmdailytracking.GetString(6);
                            }
                            if (rmdailytracking.IsDBNull(7) == false)
                            {
                                ViewBag.logTagNo = rmdailytracking.GetString(7); ;
                                //DailyTracking.LogTagNo = rmdailytracking.GetString(7);
                            }
                            if (rmdailytracking.IsDBNull(8) == false)
                            {
                                ViewBag.Customer_Name = rmdailytracking.GetString(8); ;
                                //DailyTracking.Customer_Name = rmdailytracking.GetString(8);
                            }
                            if (rmdailytracking.IsDBNull(9) == false)
                            {
                                ViewBag.Cust_Department = rmdailytracking.GetString(9); ;
                                //DailyTracking.Cust_Department = rmdailytracking.GetString(9);
                            }
                            if (rmdailytracking.IsDBNull(10) == false)
                            {
                                ViewBag.ProductName = rmdailytracking.GetString(10); ;
                                //DailyTracking.ProductName = rmdailytracking.GetString(10);
                            }
                            if (rmdailytracking.IsDBNull(11) == false)
                            {
                                ViewBag.PIC = rmdailytracking.GetString(11); ;
                                //DailyTracking.PIC = rmdailytracking.GetString(11);
                            }
                            //if (rmdailytracking.IsDBNull(12) == false)
                            //{
                            //    ViewBag.AccQty = rmdailytracking.GetString(12); ;
                            //    //DailyTracking.AccountsQty = rmdailytracking.GetString(12);
                            //}
                            //if (rmdailytracking.IsDBNull(13) == false)
                            //{
                            //    ViewBag.ImpQty = rmdailytracking.GetString(13); ;
                            //    //DailyTracking.ImpressionQty = rmdailytracking.GetString(13);
                            //}
                            //if (rmdailytracking.IsDBNull(14) == false)
                            //{
                            //    ViewBag.PageQty = rmdailytracking.GetString(14); ;
                            //    //DailyTracking.PagesQty = rmdailytracking.GetString(14);
                            //}
                            if (rmdailytracking.IsDBNull(15) == false)
                            {
                                ViewBag.StartTime = rmdailytracking.GetString(15); ;
                                //DailyTracking.PagesQty = rmdailytracking.GetString(14);
                            }
                            if (rmdailytracking.IsDBNull(16) == false)
                            {
                                ViewBag.Endtime = rmdailytracking.GetString(16); ;
                                //DailyTracking.PagesQty = rmdailytracking.GetString(14);
                            }
                            if (rmdailytracking.IsDBNull(17) == false)
                            {
                                ViewBag.ProcessStartTime = rmdailytracking.GetString(17); ;
                                //DailyTracking.PagesQty = rmdailytracking.GetString(14);
                            }
                            if (rmdailytracking.IsDBNull(18) == false)
                            {
                                ViewBag.ProcessEndTime = rmdailytracking.GetString(18); ;
                                //DailyTracking.PagesQty = rmdailytracking.GetString(14);
                            }
                            if (rmdailytracking.IsDBNull(19) == false)
                            {
                                ViewBag.TimeTaken = rmdailytracking.GetString(19); ;
                                //DailyTracking.PagesQty = rmdailytracking.GetString(14);
                            }
                            if (rmdailytracking.IsDBNull(20) == false)
                            {
                                ViewBag.DateApproveTime = rmdailytracking.GetString(20); ;
                                //DailyTracking.PagesQty = rmdailytracking.GetString(14);
                            }
                            if (rmdailytracking.IsDBNull(21) == false)
                            {
                                ViewBag.LogTagSendTime = rmdailytracking.GetString(21); ;
                                //DailyTracking.PagesQty = rmdailytracking.GetString(14);
                            }
                            if (rmdailytracking.IsDBNull(22) == false)
                            {
                                ViewBag.IdDT = rmdailytracking.GetGuid(22); ;
                                //DailyTracking.PagesQty = rmdailytracking.GetString(14);
                            }

                        }
                    }

                    rmdailytracking.Close();

                    List<int> TotalAccDT = new List<int>();
                    List<int> TotalImpDT = new List<int>();
                    List<int> TotalPageDT = new List<int>();

                    SqlCommand AIPDT = new SqlCommand("SELECT AccQty, ImpQty, PageQty FROM JobAuditTrailDetail WHERE LogTagNo = @LogTagNoDT", cn);
                    AIPDT.Parameters.AddWithValue("@LogTagNoDT", LogTagNo);
                    SqlDataReader rmAIP = AIPDT.ExecuteReader();

                    if (rmAIP.HasRows)
                    {
                        while (rmAIP.Read())
                        {
                            if (!rmAIP.IsDBNull(0))
                            {

                                try
                                {
                                    TotalAccDT.Add(Int32.Parse(rmAIP.GetString(0)));

                                }
                                catch
                                {
                                    TotalAccDT.Add(0);
                                }
                            }
                            else
                            {
                                TotalAccDT.Add(0);
                            }

                            if (!rmAIP.IsDBNull(1))
                            {

                                try
                                {
                                    TotalImpDT.Add(Int32.Parse(rmAIP.GetString(1)));

                                }
                                catch
                                {
                                    TotalImpDT.Add(0);
                                }

                            }
                            else
                            {
                                TotalImpDT.Add(0);

                            }

                            if (!rmAIP.IsDBNull(2))
                            {

                                try
                                {
                                    TotalPageDT.Add(Int32.Parse(rmAIP.GetString(2)));

                                }
                                catch
                                {
                                    TotalPageDT.Add(2);
                                }

                            }
                            else
                            {
                                TotalPageDT.Add(0);

                            }
                        }
                    }

                    ViewBag.AccountQtyDT = TotalAccDT.Sum().ToString();
                    ViewBag.ImpressionQtyDT = TotalImpDT.Sum().ToString();
                    ViewBag.PagesQtyDT = TotalPageDT.Sum().ToString();


                }

                //SqlCommand getdailytracking = new SqlCommand("SELECT TOP(1) FORMAT(CONVERT(date, StartDateOn), 'yyyy-MM-dd'), FORMAT(CONVERT(date, EndDateOn), 'yyyy-MM-dd'),  FORMAT(CONVERT(date, ProcessStartDateOn), 'yyyy-MM-dd'), " +
                //    "FORMAT(CONVERT(date, ProcessEndDateOn), 'yyyy-MM-dd'), FORMAT(CONVERT(date, DateApproveOn), 'yyyy-MM-dd'), FORMAT(CONVERT(date, LogTagSendOn), 'yyyy-MM-dd'), JobSheetNo, LogTagNo, Customer_Name, " +
                //    "Cust_Department, ProductName, PIC, AccountsQty, ImpressionQty, PagesQty, StartTime, EndTime, ProcessStartTime, ProcessEndTime, TimeTaken, DateApproveTime, LogTagSendTime FROM DailyTracking Where LogTagNo=@LogTagNo", cn);
                //getdailytracking.Parameters.AddWithValue("LogTagNo", LogTagNo);
                //SqlDataReader rmdailytracking = getdailytracking.ExecuteReader();



                if (set == "view")
                {
                    SqlCommand cmd = new SqlCommand("SELECT Id, Customer_Name, ProductName, JobClass, JobType,Cust_Department,LogTagNo,ProgramId,AccQty,ImpQty,PageQty,JobNameIT,FirstRecord,LastRecord," +
                    "ModeLog,Path,JobId,ProgramId,FileId,FORMAT(CONVERT(date, RevStrtDateOn), 'yyyy-MM-dd'),RevStrtTime,FORMAT(CONVERT(date, ProcessDate), 'yyyy-MM-dd'),TimeProcessIt,FORMAT(CONVERT(date, ProcessEnd), 'yyyy-MM-dd'),TimeEndProcessIt,FORMAT(CONVERT(date, DateApproveOn), 'yyyy-MM-dd'),DateApproveTime,FORMAT(CONVERT(date, ImageInDateOn), 'yyyy-MM-dd'),ImageInTime,FORMAT(CONVERT(date, RevisedInDateOn), 'yyyy-MM-dd')," +
                    "Type,Remark,CreatedOn, JobSheetNo, CreateByIT FROM [dbo].[JobAuditTrailDetail] WHERE Id=@Id1 ", cn);
                    cmd.Parameters.AddWithValue("@Id1", Id);

                    SqlDataReader rm = cmd.ExecuteReader();

                    while (rm.Read())
                    {
                        if (rm.IsDBNull(0) == false)
                        {
                            ViewBag.Id = rm.GetGuid(0);
                        }
                        if (rm.IsDBNull(1) == false)
                        {
                            ViewBag.Customer_Name = rm.GetString(1);
                        }
                        if (rm.IsDBNull(2) == false)
                        {
                            ViewBag.ProductName = rm.GetString(2);
                        }
                        if (rm.IsDBNull(3) == false)
                        {
                            ViewBag.JobClass = rm.GetString(3);
                        }
                        if (rm.IsDBNull(4) == false)
                        {
                            ViewBag.JobType = rm.GetString(4);
                        }

                        if (rm.IsDBNull(5) == false)
                        {
                            ViewBag.Cust_Department = rm.GetString(5);
                        }
                        if (rm.IsDBNull(6) == false)
                        {
                            ViewBag.LogTagNo = rm.GetString(6);
                        }
                        if (rm.IsDBNull(7) == false)
                        {
                            ViewBag.ProgramId = rm.GetString(7);
                        }

                        if (rm.IsDBNull(8) == false)
                        {
                            ViewBag.AccQty = rm.GetString(8);
                        }

                        if (rm.IsDBNull(9) == false)
                        {
                            ViewBag.ImpQty = rm.GetString(9);
                        }
                        if (rm.IsDBNull(10) == false)
                        {
                            ViewBag.PageQty = rm.GetString(10);
                        }
                        if (rm.IsDBNull(11) == false)
                        {
                            ViewBag.JobNameIT = rm.GetString(11);
                        }

                        if (rm.IsDBNull(12) == false)
                        {
                            ViewBag.FirstRecord = rm.GetString(12);
                        }
                        if (rm.IsDBNull(13) == false)
                        {
                            ViewBag.LastRecord = rm.GetString(13);
                        }
                        if (rm.IsDBNull(14) == false)
                        {
                            ViewBag.ModeLog = rm.GetString(14);
                        }
                        if (rm.IsDBNull(15) == false)
                        {
                            ViewBag.Path = rm.GetString(15);
                        }
                        if (rm.IsDBNull(16) == false)
                        {
                            ViewBag.JobId = rm.GetString(16);
                        }
                        //if (rm.IsDBNull(17) == false)
                        //{
                        //    ViewBag.ProgramId = rm.GetString(17);
                        //}
                        if (rm.IsDBNull(18) == false)
                        {
                            ViewBag.FileId = rm.GetString(18);
                        }
                        if (rm.IsDBNull(19) == false)
                        {
                            //ViewBag.RevStrtDateOn = String.Format("{0:dd/MM/yyyy }", (DateTime)rm.GetDateTime(19));
                            ViewBag.RevStrtDateOn = rm.GetString(19);

                        }
                        if (rm.IsDBNull(20) == false)
                        {
                            ViewBag.RevStrtTime = rm.GetString(20);
                        }
                        if (rm.IsDBNull(21) == false)
                        {
                            //ViewBag.ProcessDate = String.Format("{0:dd/MM/yyyy }", (DateTime)rm.GetDateTime(21));
                            ViewBag.ProcessDate = rm.GetString(21);

                        }
                        if (rm.IsDBNull(22) == false)
                        {
                            ViewBag.TimeProcessIt = rm.GetString(22);
                        }
                        if (rm.IsDBNull(23) == false)
                        {
                            //ViewBag.ProcessEnd = String.Format("{0:dd/MM/yyyy }", (DateTime)rm.GetDateTime(23));
                            ViewBag.ProcessEnd = rm.GetString(23);

                        }
                        if (rm.IsDBNull(24) == false)
                        {
                            ViewBag.TimeEndProcessIt = rm.GetString(24);
                        }
                        if (rm.IsDBNull(25) == false)
                        {
                            //ViewBag.DateApproveOn = String.Format("{0:dd/MM/yyyy }", (DateTime)rm.GetDateTime(25));
                            ViewBag.DateApproveOn = rm.GetString(25);

                        }
                        if (rm.IsDBNull(26) == false)
                        {
                            ViewBag.DateApproveTime = rm.GetString(26);
                        }
                        if (rm.IsDBNull(27) == false)
                        {
                            ViewBag.ImageInDateOn = rm.GetString(27);

                            //ViewBag.ImageInDateOn = String.Format("{0:dd/MM/yyyy }", (DateTime)rm.GetDateTime(28));
                        }
                        if (rm.IsDBNull(28) == false)
                        {
                            ViewBag.ImageInTime = rm.GetString(28);
                        }
                        if (rm.IsDBNull(29) == false)
                        {
                            //ViewBag.RevisedInDateOn = String.Format("{0:dd/MM/yyyy }", (DateTime)rm.GetDateTime(29));
                            ViewBag.RevisedInDateOn = rm.GetString(29);

                        }
                        if (rm.IsDBNull(30) == false)
                        {
                            ViewBag.Type = rm.GetString(30);
                        }
                        if (rm.IsDBNull(31) == false)
                        {
                            ViewBag.Remark = rm.GetString(31);
                        }
                        if (rm.IsDBNull(32) == false)
                        {
                            ViewBag.CreatedOn = rm.GetDateTime(32);
                        }
                        if (rm.IsDBNull(33) == false)
                        {
                            ViewBag.JobSheetNo = rm.GetString(33);
                        }
                        if (rm.IsDBNull(34) == false)
                        {
                            ViewBag.CreateByIT = rm.GetString(34);
                        }
                    }
                }


                cn.Close();
            }

            if (set == "UpdateDailyTracking")
            {
                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command2 = new SqlCommand("", cn2))
                {
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                    string ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                    Guid newId = Guid.NewGuid();

                    cn2.Open();

                    command2.CommandText =
                        @"IF NOT EXISTS 
                         ( SELECT  1 FROM [dbo].[DailyTracking] 
                         WHERE LogTagNo = @LogTagCheck )
                         BEGIN
                            INSERT INTO [dbo].[DailyTracking] 
                                (Id,StartDateOn,StartTime,EndDateOn,EndTime,ProcessStartDateOn,ProcessStartTime,ProcessEndDateOn,ProcessEndTime,TimeTaken,
                                DateApproveOn,DateApproveTime,LogTagSendOn,LogTagSendTime,CreatedOn, JobSheetNo, LogTagNo, JobClass, JobType, Customer_Name, Cust_Department, ProductName, PIC, AccountsQty,ImpressionQty, PagesQty, Status, JobAuditTrail,PaperType) 
                                VALUES (@Id,@StartDateOn,@StartTime,@EndDateOn,@EndTime,@ProcessStartDateOn,@ProcessStartTime,@ProcessEndDateOn,@ProcessEndTime,@TimeTaken,@DateApproveOn,@DateApproveTime,@LogTagSendOn,@LogTagSendTime,@CreatedOn, @JobSheetNo, 
                                @LogTagNo, @JobClass, @JobType, @Customer_Name, @Cust_Department, @ProductName, @PIC, @AccountsQty, @ImpressionQty, @PagesQty, @Status, @JobAuditTrail,@PaperType) 
                         END
                     ELSE 
                        BEGIN 
                            UPDATE [dbo].[DailyTracking] SET
                            StartDateOn=@StartDateOn, StartTime=@StartTime, EndDateOn=@EndDateOn, EndTime=@EndTime, ProcessStartDateOn=@ProcessStartDateOn ,ProcessStartTime=@ProcessStartTime ,ProcessEndDateOn=@ProcessEndDateOn ,ProcessEndTime=@ProcessEndTime ,TimeTaken=@TimeTaken,
                            DateApproveOn=@DateApproveOn, DateApproveTime=@DateApproveTime, LogTagSendOn=@LogTagSendOn ,LogTagSendTime=@LogTagSendTime, ModifiedOn=@ModifiedOn,  AccountsQty=@AccountsQty, ImpressionQty=@ImpressionQty, PagesQty=@PagesQty WHERE LogTagNo=@LogTagUpdateDT
                        END";

                    command2.Parameters.AddWithValue("@Id", newId);
                    command2.Parameters.AddWithValue("@LogTagCheck", LogTagNo);
                    command2.Parameters.AddWithValue("@LogTagUpdateDT", LogTagNo);



                    if (get2.StartDateOn == null)
                    {
                        command2.Parameters.Add(new SqlParameter { ParameterName = "@StartDateOn", Value = DBNull.Value });
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@StartDateOn", get2.StartDateOn);
                    }

                    if (get2.StartTime == null)
                    {
                        command2.Parameters.Add(new SqlParameter { ParameterName = "@StartTime", Value = DBNull.Value });
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@StartTime", get2.StartTime);
                    }

                    if (get2.EndDateOn == null)
                    {
                        command2.Parameters.Add(new SqlParameter { ParameterName = "@EndDateOn", Value = DBNull.Value });
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@EndDateOn", get2.EndDateOn);
                    }

                    if (get2.EndTime == null)
                    {
                        command2.Parameters.Add(new SqlParameter { ParameterName = "@EndTime", Value = DBNull.Value });
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@EndTime", get2.EndTime);
                    }

                    if (get2.ProcessStartDateOn == null)
                    {
                        command2.Parameters.Add(new SqlParameter { ParameterName = "@ProcessStartDateOn", Value = DBNull.Value });
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@ProcessStartDateOn", get2.ProcessStartDateOn);
                    }

                    if (get2.ProcessStartTime == null)
                    {
                        command2.Parameters.Add(new SqlParameter { ParameterName = "@ProcessStartTime", Value = DBNull.Value });
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@ProcessStartTime", get2.ProcessStartTime);
                    }

                    if (get2.ProcessEndDateOn == null)
                    {
                        command2.Parameters.Add(new SqlParameter { ParameterName = "@ProcessEndDateOn", Value = DBNull.Value });
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@ProcessEndDateOn", get2.ProcessEndDateOn);
                    }

                    if (get2.ProcessEndTime == null)
                    {
                        command2.Parameters.Add(new SqlParameter { ParameterName = "@ProcessEndTime", Value = DBNull.Value });
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@ProcessEndTime", get2.ProcessEndTime);
                    }

                    if (TimeTaken == null)
                    {
                        command2.Parameters.Add(new SqlParameter { ParameterName = "@TimeTaken", Value = DBNull.Value });
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@TimeTaken", TimeTaken);
                    }

                    if (get.DateApproveOn == null)
                    {
                        command2.Parameters.Add(new SqlParameter { ParameterName = "@DateApproveOn", Value = DBNull.Value });
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@DateApproveOn", get.DateApproveOn);
                    }

                    if (get.DateApproveTime == null)
                    {
                        command2.Parameters.Add(new SqlParameter { ParameterName = "@DateApproveTime", Value = DBNull.Value });
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@DateApproveTime", get.DateApproveTime);
                    }

                    if (get2.LogTagSendOn == null)
                    {
                        command2.Parameters.Add(new SqlParameter { ParameterName = "@LogTagSendOn", Value = DBNull.Value });
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@LogTagSendOn", get2.LogTagSendOn);
                    }

                    if (get2.LogTagSendTime == null)
                    {
                        command2.Parameters.Add(new SqlParameter { ParameterName = "@LogTagSendTime", Value = DBNull.Value });
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@LogTagSendTime", get2.LogTagSendTime);
                    }
                    command2.Parameters.AddWithValue("@CreatedOn", createdOn);
                    command2.Parameters.AddWithValue("@ModifiedOn", ModifiedOn);

                    if (JobSheetNo == null)
                    {
                        command2.Parameters.Add(new SqlParameter { ParameterName = "@JobSheetNo", Value = DBNull.Value });
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@JobSheetNo", JobSheetNo);
                    }

                    if (LogTagNo == null)
                    {
                        command2.Parameters.Add(new SqlParameter { ParameterName = "@LogTagNo", Value = DBNull.Value });
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@LogTagNo", LogTagNo);
                    }


                    if (PaperType == null)
                    {
                        command2.Parameters.Add(new SqlParameter { ParameterName = "@PaperType", Value = DBNull.Value });
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@PaperType", PaperType);
                    }

                    if (JobClass == null)
                    {
                        command2.Parameters.Add(new SqlParameter { ParameterName = "@JobClass", Value = DBNull.Value });
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@JobClass", JobClass);
                    }

                    if (JobType == null)
                    {
                        command2.Parameters.Add(new SqlParameter { ParameterName = "@JobType", Value = DBNull.Value });
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@JobType", JobType);
                    }

                    if (Customer_Name == null)
                    {
                        command2.Parameters.Add(new SqlParameter { ParameterName = "@Customer_Name", Value = DBNull.Value });
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                    }

                    if (Cust_Department == null)
                    {
                        command2.Parameters.Add(new SqlParameter { ParameterName = "@Cust_Department", Value = DBNull.Value });
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@Cust_Department", Cust_Department);
                    }

                    if (ProductName == null)
                    {
                        command2.Parameters.Add(new SqlParameter { ParameterName = "@ProductName", Value = DBNull.Value });
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@ProductName", ProductName);
                    }

                    if (CreateByIT == null)
                    {
                        command2.Parameters.Add(new SqlParameter { ParameterName = "@PIC", Value = DBNull.Value });
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@PIC", CreateByIT);
                    }

                    if (AccountsQty == null)
                    {
                        command2.Parameters.Add(new SqlParameter { ParameterName = "@AccountsQty", Value = DBNull.Value });
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@AccountsQty", AccountsQty);
                    }

                    if (ImpressionQty == null)
                    {
                        command2.Parameters.Add(new SqlParameter { ParameterName = "@ImpressionQty", Value = DBNull.Value });
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@ImpressionQty", ImpressionQty);
                    }

                    if (PagesQty == null)
                    {
                        command2.Parameters.Add(new SqlParameter { ParameterName = "@PagesQty", Value = DBNull.Value });
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@PagesQty", PagesQty);
                    }

                    command2.Parameters.AddWithValue("@Status", "Completed");
                    command2.Parameters.AddWithValue("@JobAuditTrail", Id);

                    command2.ExecuteNonQuery();
                    cn2.Close();
                }

                return View(listJob);
                //return RedirectToAction("ManageDailyJobITO", "ITO");
                //return View();
            }


            if (set == "AddUpdate")
            {
                Session["Id"] = Id;
                ViewBag.Id = Id;
                ViewBag.Customer_Name = Customer_Name;
                ViewBag.ProductName = ProductName;

                int fileCount = 0;

                using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn1.Open();
                    SqlCommand checkFileName = new SqlCommand("SELECT COUNT(FileId) FROM JobAuditTrailDetail WHERE FileId = @FileIdCheck", cn1);
                    checkFileName.Parameters.AddWithValue("@FileIdCheck", FileId);
                    SqlDataReader rmFile = checkFileName.ExecuteReader();

                    while (rmFile.Read())
                    {
                        fileCount = rmFile.GetInt32(0);
                    }

                    cn1.Close();
                }

                if(fileCount>1)
                {
                    return RedirectToAction("AddJAT", "ITO", new { Id = Id, Customer_Name = Customer_Name, LogTagNo = LogTagNo, JobClass = JobClass,msg="File name already used" });
                }





                //string revStrtDateOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                string dateProcessItOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                //string dateApproveOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                //string imageInDateOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                //string revisedInDateOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                //string processDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                //string processEnd = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");



                using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    Debug.WriteLine("ID :" + Id);
                    Debug.WriteLine("Created On :" + createdOn);


                    get.JobRequest = Convert.ToDateTime(get.JobRequestTxt);
                    var No_ = new ITOLog();

                    cn1.Open();
                    SqlCommand command1;
                    command1 = new SqlCommand("UPDATE [dbo].[JobAuditTrailDetail] SET JobRequest=@JobRequest, Customer_Name=@Customer_Name, ProductName=@ProductName,  JobType=@JobType, NotesByIT=@NotesByIT, NotesByProduction=@NotesByIT, NotesByPurchasing=@NotesByPurchasing, NotesByEngineering=@NotesByEngineering, NotesByArtwork=@NotesByArtwork, NotesByFinance=@NotesByFinance, NotesByDCP=@NotesByDCP,Path=@Path,JobNameIT=@JobNameIT,JobId=@JobId,ProgramId=@ProgramId,FileId=@FileId,RevStrtDateOn=@RevStrtDateOn,RevStrtTime=@RevStrtTime,DateProcessItOn=@DateProcessItOn,TimeProcessIt=@TimeProcessIt,DateApproveOn=@DateApproveOn,DateApproveTime=@DateApproveTime,AccQty=@AccQty,ImpQty=@ImpQty,PageQty=@PageQty,FirstRecord=@FirstRecord,LastRecord=@LastRecord,Remark=@Remark,Type=@Type,Status=@Status,CreateByIt=@CreateByIt,ImageInDateOn=@ImageInDateOn,ImageInTime=@ImageInTime,RevisedInDateOn=@RevisedInDateOn,Cust_Department=@Cust_Department,ProcessDate=@ProcessDate,ProcessEnd=@ProcessEnd,TimeEndProcessIt=@TimeEndProcessIt, RevEndDateOn=@RevEndDateOn, RevEndTimeOn=@RevEndTimeOn, TimeTaken=@TimeTaken WHERE Id=@Id", cn1);
                    command1.Parameters.AddWithValue("@Id", Id);
                    //Debug.WriteLine("ID again :" + Id);

                    try
                    {
                        if (TimeTaken != null)
                        {
                            command1.Parameters.AddWithValue("@TimeTaken", TimeTaken);
                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@TimeTaken", DBNull.Value);
                        }

                        if (JobRequest != null)
                        {
                            command1.Parameters.AddWithValue("@JobRequest", JobRequest);
                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@JobRequest", DBNull.Value);
                        }

                        if (Customer_Name != null)
                        {
                            command1.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@Customer_Name", DBNull.Value);
                        }

                        if (ProductName != null)
                        {
                            command1.Parameters.AddWithValue("@ProductName", ProductName);
                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@ProductName", DBNull.Value);
                        }


                        if (JobType != null)
                        {
                            command1.Parameters.AddWithValue("@JobType", JobType);
                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@JobType", DBNull.Value);
                        }

                        if (NotesByIT != null)
                        {
                            command1.Parameters.AddWithValue("@NotesByIT", NotesByIT);
                        }
                        else
                        {

                            command1.Parameters.AddWithValue("@NotesByIT", DBNull.Value);

                        }

                        if (NotesByProduction != null)
                        {
                            command1.Parameters.AddWithValue("@NotesByProduction", NotesByProduction);
                        }
                        else
                        {

                            command1.Parameters.AddWithValue("@NotesByProduction", DBNull.Value);
                        }

                        if (NotesByPurchasing != null)
                        {
                            command1.Parameters.AddWithValue("@NotesByPurchasing", NotesByPurchasing);
                        }
                        else
                        {

                            command1.Parameters.AddWithValue("@NotesByPurchasing", DBNull.Value);

                        }

                        if (NotesByEngineering != null)
                        {
                            command1.Parameters.AddWithValue("@NotesByEngineering", NotesByEngineering);
                        }
                        else
                        {

                            command1.Parameters.AddWithValue("@NotesByEngineering", DBNull.Value);

                        }

                        if (NotesByArtwork != null)
                        {
                            command1.Parameters.AddWithValue("@NotesByArtwork", NotesByArtwork);
                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@NotesByArtwork", DBNull.Value);

                        }

                        if (NotesByFinance != null)
                        {
                            command1.Parameters.AddWithValue("@NotesByFinance", NotesByFinance);
                        }
                        else
                        {

                            command1.Parameters.AddWithValue("@NotesByFinance", DBNull.Value);

                        }

                        if (NotesByDCP != null)
                        {
                            command1.Parameters.AddWithValue("@NotesByDCP", NotesByDCP);
                        }
                        else
                        {

                            command1.Parameters.AddWithValue("@NotesByDCP", DBNull.Value);


                        }

                        command1.Parameters.AddWithValue("@CreatedOn", createdOn);

                        if (Path != null)
                        {
                            command1.Parameters.AddWithValue("@Path", Path);
                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@Path", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(JobNameIT))
                        {
                            command1.Parameters.AddWithValue("@JobNameIT", JobNameIT);

                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@JobNameIT", DBNull.Value);

                        }

                        if (!string.IsNullOrEmpty(JobId))
                        {
                            command1.Parameters.AddWithValue("@JobId", JobId);

                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@JobId", DBNull.Value);

                        }

                        if (!string.IsNullOrEmpty(ProgramId))
                        {
                            command1.Parameters.AddWithValue("@ProgramId", ProgramId);
                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@ProgramId", DBNull.Value);

                        }

                        if (!string.IsNullOrEmpty(FileId))
                        {
                            command1.Parameters.AddWithValue("@FileId", FileId);

                        }
                        //else
                        //{
                        //    command1.Parameters.AddWithValue("@FileId", DBNull.Value);

                        //}

                        if (!string.IsNullOrEmpty(revStrtDateOn))
                        {
                            command1.Parameters.AddWithValue("@RevStrtDateOn", revStrtDateOn);

                        }
                        //else
                        //{
                        //    command1.Parameters.AddWithValue("@RevStrtDateOn", DBNull.Value);
                        //}

                        if (!string.IsNullOrEmpty(RevStrtTime))
                        {
                            command1.Parameters.AddWithValue("@RevStrtTime", RevStrtTime);

                        }
                        //else
                        //{
                        //    command1.Parameters.AddWithValue("@RevStrtTime", DBNull.Value);

                        //}

                        if (!string.IsNullOrEmpty(dateProcessItOn))
                        {
                            command1.Parameters.AddWithValue("@DateProcessItOn", dateProcessItOn);
                        }
                        //else
                        //{
                        //    command1.Parameters.AddWithValue("@DateProcessItOn", DBNull.Value);
                        //}

                        if (!string.IsNullOrEmpty(TimeProcessIt))
                        {
                            command1.Parameters.AddWithValue("@TimeProcessIt", TimeProcessIt);

                        }
                        //else
                        //{
                        //    command1.Parameters.AddWithValue("@TimeProcessIt", DBNull.Value);

                        //}

                        if (!string.IsNullOrEmpty(dateApproveOn))
                        {

                            command1.Parameters.AddWithValue("@DateApproveOn", dateApproveOn);
                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@DateApproveOn", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(DateApproveTime))
                        {

                            command1.Parameters.AddWithValue("@DateApproveTime", DateApproveTime);
                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@DateApproveTime", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(AccQty))
                        {

                            command1.Parameters.AddWithValue("@AccQty", AccQty);
                        }
                        //else
                        //{
                        //    command1.Parameters.AddWithValue("@AccQty", DBNull.Value);
                        //}

                        if (!string.IsNullOrEmpty(ImpQty))
                        {

                            command1.Parameters.AddWithValue("@ImpQty", ImpQty);

                        }
                        //else
                        //{
                        //    command1.Parameters.AddWithValue("@ImpQty", DBNull.Value);
                        //}

                        if (!string.IsNullOrEmpty(PageQty))
                        {

                            command1.Parameters.AddWithValue("@PageQty", PageQty);

                        }
                        //else
                        //{
                        //    command1.Parameters.AddWithValue("@PageQty", DBNull.Value);
                        //}

                        if (!string.IsNullOrEmpty(FirstRecord))
                        {

                            command1.Parameters.AddWithValue("@FirstRecord", FirstRecord);

                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@FirstRecord", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(LastRecord))
                        {

                            command1.Parameters.AddWithValue("@LastRecord", LastRecord);

                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@LastRecord", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(Remark))
                        {

                            command1.Parameters.AddWithValue("@Remark", Remark);

                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@Remark", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(Type))
                        {

                            command1.Parameters.AddWithValue("@Type", Type);

                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@Type", DBNull.Value);
                        }

                        command1.Parameters.AddWithValue("@Status", "PROCESSING");

                        if (!string.IsNullOrEmpty(IdentityName.ToString()))
                        {

                            command1.Parameters.AddWithValue("@CreateByIt", IdentityName.ToString());
                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@CreateByIt", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(imageInDateOn))
                        {

                            command1.Parameters.AddWithValue("@ImageInDateOn", imageInDateOn);
                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@ImageInDateOn", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(ImageInTime))
                        {

                            command1.Parameters.AddWithValue("@ImageInTime", ImageInTime);
                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@ImageInTime", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(revisedInDateOn))
                        {

                            command1.Parameters.AddWithValue("@RevisedInDateOn", revisedInDateOn);
                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@RevisedInDateOn", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(Cust_Department))
                        {

                            command1.Parameters.AddWithValue("@Cust_Department", Cust_Department);
                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@Cust_Department", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(processDate))
                        {

                            command1.Parameters.AddWithValue("@ProcessDate", processDate);
                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@ProcessDate", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(processEnd))
                        {

                            command1.Parameters.AddWithValue("@ProcessEnd", processEnd);
                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@ProcessEnd", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(timeEndProcessIt))
                        {

                            command1.Parameters.AddWithValue("@timeEndProcessIt", timeEndProcessIt);
                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@timeEndProcessIt", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(RevEndDateOn))
                        {

                            command1.Parameters.AddWithValue("@RevEndDateOn", RevEndDateOn);
                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@RevEndDateOn", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(RevEndTimeOn))
                        {

                            command1.Parameters.AddWithValue("@RevEndTimeOn", RevEndTimeOn);
                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@RevEndTimeOn", DBNull.Value);
                        }

                        command1.ExecuteNonQuery();

                        return RedirectToAction("AddJAT", "ITO", new { Id = Id, Customer_Name = Customer_Name, LogTagNo = LogTagNo, JobClass = JobClass });

                    }
                    catch
                    {
                        TempData["Error"] = "<script>alert('Please Fill In All Required Field.')</script>";
                        return RedirectToAction("AddJAT", "ITO", new { Id = Id, Customer_Name = Customer_Name, LogTagNo = LogTagNo, JobClass = JobClass });

                    }





                    cn1.Close();


                    //SQL Command below were commented by Firdaus reason being of unknown use

                    //using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    //{
                    //    Debug.WriteLine("ID Update :" + Id);

                    //    cn.Open();
                    //    SqlCommand command2;
                    //    command2 = new SqlCommand("UPDATE [dbo].[JobInstruction] SET STATUS='DAILY AT',ModifiedOn=@ModifiedOn WHERE Id=@Id", cn);
                    //    command2.Parameters.AddWithValue("@ModifiedOn", createdOn);
                    //    command2.Parameters.AddWithValue("@Id", Id);
                    //    command2.ExecuteNonQuery();
                    //    cn.Close();

                    //    TempData["msg"] = "<script>alert('SUCCESSFULLY SUBMIT  !');</script>";



                    //}

                    //return RedirectToAction("AddJAT", "ITO", new { LogTagNo = Session["LogTagNo"].ToString() });
                    //return View(listJob);




                }



            }


        }


        return View(listJob);
    }



    public ActionResult CreateJobAuditTrail(JobAuditTrailDetail ModelSample, JobAuditTrailDetail get, string Id, string JobAuditTrailId, string set, string Set,
                                        string JobRequest, string Customer_Name, string ProductName, string JobSheetNo, string JobClass,
                                        string Frequency, string JobType, string LogTagNo, string TotalAuditTrail, string CreateByIT, string Status,
                                        string ModeLog, string Path, string JobNameIT, string JobId, string ProgramId, string FileId,
                                        string RevStrtDateOn, string RevStrtTime, string DateProcessItOn, string TimeProcessIt,
                                        string DateApproveOn, string DateApproveTime, string AccountsQty, string ImpressionQty, string PagesQty,
                                        string FirstRecord, string LastRecord, string Remark, string Type,
                                        string NotesByIT, string NotesByProduction, string NotesByPurchasing,
                                        string NotesByEngineering, string NotesByArtwork, string NotesByFinance, string NotesByDCP, string JobInstructionId,
                                        string ImageInDateOn, string ImageInTime, string RevisedInDateOn,
                                        string AccQty, string ImpQty, string PageQty)


    {
        var IdentityName = @Session["Fullname"];
        Session["Id"] = Id;
        ViewBag.Id = Id;
        ViewBag.JobInstructionId = JobInstructionId;
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.IsRole = @Session["Role"];
        string Deptment = @Session["Department"].ToString();
        ViewBag.Customer_Name = Customer_Name;
        ViewBag.ProductName = ProductName;

        if (!string.IsNullOrEmpty(Id))
        {

            List<JobInstruction> JobInstructionlist1 = new List<JobInstruction>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {

                int _bil = 1;
                cn.Open();

                command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass, JobType,JobSheetNo, StartDevDate, EndDevDate
                                         FROM [dbo].[JobInstruction]                                    
                                         WHERE Id=@Id
                                         ORDER BY CreatedOn desc ";

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


            if (set == "AddNew")
            {
                Session["Id"] = Id;
                ViewBag.Id = Id;
                ViewBag.Customer_Name = Customer_Name;
                ViewBag.ProductName = ProductName;

                string revStrtDateOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                string dateProcessItOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                string dateApproveOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                string imageInDateOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                string revisedInDateOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");



                using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {

                    Guid Idx = Guid.NewGuid();
                    get.JobRequest = Convert.ToDateTime(get.JobRequestTxt);
                    var No_ = new ITOLog();

                    cn1.Open();
                    SqlCommand command1;
                    command1 = new SqlCommand("INSERT INTO [dbo].[JobAuditTrailDetail](Id, JobRequest, Customer_Name, ProductName, JobClass, Frequency, JobType, LogTagNo, NotesByIT, NotesByProduction, NotesByPurchasing, NotesByEngineering, NotesByArtwork, NotesByFinance, NotesByDCP, JobInstructionId, JobAuditTrailId, AccountsQty, ImpressionQty, PagesQty,CreatedOn,Path,JobNameIT,JobId,ProgramId,FileId,RevStrtDateOn,RevStrtTime,DateProcessItOn,TimeProcessIt,DateApproveOn,DateApproveTime,AccQty,ImpQty,PageQty,FirstRecord,LastRecord,Remark,Type,Status,CreateByIt,ImageInDateOn,ImageInTime,RevisedInDateOn) " +
                        "values (@Id, @JobRequest, @Customer_Name, @ProductName, @JobClass, @Frequency, @JobType, @LogTagNo, @NotesByIT, @NotesByProduction, @NotesByPurchasing, @NotesByEngineering, @NotesByArtwork, @NotesByFinance, @NotesByDCP, @JobInstructionId, @JobAuditTrailId, @AccountsQty, @ImpressionQty, @PagesQty,@CreatedOn,@Path,@JobNameIT,@JobId,@ProgramId,@FileId,@RevStrtDateOn,@RevStrtTime,@DateProcessItOn,@TimeProcessIt,@DateApproveOn,@DateApproveTime,@AccQty,@ImpQty,@PageQty,@FirstRecord,@LastRecord,@Remark,@Type,@Status,@CreateByIt,@ImageInDateOn,@ImageInTime,@RevisedInDateOn)", cn1);
                    command1.Parameters.AddWithValue("@Id", Idx);
                    if (JobRequest != null)
                    {
                        command1.Parameters.AddWithValue("@JobRequest", JobRequest);
                    }
                    else
                    {
                        command1.Parameters.AddWithValue("@JobRequest", DBNull.Value);
                    }
                    if (Customer_Name != null)
                    {
                        command1.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                    }
                    else
                    {
                        command1.Parameters.AddWithValue("@Customer_Name", DBNull.Value);
                    }
                    if (ProductName != null)
                    {
                        command1.Parameters.AddWithValue("@ProductName", ProductName);
                    }
                    else
                    {
                        command1.Parameters.AddWithValue("@ProductName", DBNull.Value);
                    }
                    if (JobClass != null)
                    {
                        command1.Parameters.AddWithValue("@JobClass", JobClass);
                    }
                    else
                    {
                        command1.Parameters.AddWithValue("@JobClass", DBNull.Value);
                    }
                    if (Frequency != null)
                    {
                        command1.Parameters.AddWithValue("@Frequency", Frequency);
                    }
                    else
                    {
                        command1.Parameters.AddWithValue("@Frequency", DBNull.Value);

                    }
                    if (JobType != null)
                    {
                        command1.Parameters.AddWithValue("@JobType", JobType);
                    }
                    else
                    {
                        command1.Parameters.AddWithValue("@JobType", DBNull.Value);
                    }
                    if (LogTagNo != null)
                    {
                        command1.Parameters.AddWithValue("@LogTagNo", LogTagNo);
                    }
                    else
                    {
                        command1.Parameters.AddWithValue("@LogTagNo", DBNull.Value);
                    }
                    if (NotesByIT != null)
                    {
                        command1.Parameters.AddWithValue("@NotesByIT", NotesByIT);
                    }
                    else
                    {

                        command1.Parameters.AddWithValue("@NotesByIT", DBNull.Value);

                    }
                    if (NotesByProduction != null)
                    {
                        command1.Parameters.AddWithValue("@NotesByProduction", NotesByProduction);
                    }
                    else
                    {

                        command1.Parameters.AddWithValue("@NotesByProduction", DBNull.Value);
                    }
                    if (NotesByPurchasing != null)
                    {
                        command1.Parameters.AddWithValue("@NotesByPurchasing", NotesByPurchasing);
                    }
                    else
                    {

                        command1.Parameters.AddWithValue("@NotesByPurchasing", DBNull.Value);

                    }
                    if (NotesByEngineering != null)
                    {
                        command1.Parameters.AddWithValue("@NotesByEngineering", NotesByEngineering);
                    }
                    else
                    {

                        command1.Parameters.AddWithValue("@NotesByEngineering", DBNull.Value);

                    }
                    if (NotesByArtwork != null)
                    {
                        command1.Parameters.AddWithValue("@NotesByArtwork", NotesByArtwork);
                    }
                    else
                    {
                        command1.Parameters.AddWithValue("@NotesByArtwork", DBNull.Value);

                    }
                    if (NotesByFinance != null)
                    {
                        command1.Parameters.AddWithValue("@NotesByFinance", NotesByFinance);
                    }
                    else
                    {

                        command1.Parameters.AddWithValue("@NotesByFinance", DBNull.Value);

                    }
                    if (NotesByDCP != null)
                    {
                        command1.Parameters.AddWithValue("@NotesByDCP", NotesByDCP);
                    }
                    else
                    {

                        command1.Parameters.AddWithValue("@NotesByDCP", DBNull.Value);


                    }
                    if (JobInstructionId != null)
                    {
                        command1.Parameters.AddWithValue("@JobInstructionId", Id);
                    }
                    else
                    {
                        command1.Parameters.AddWithValue("@JobInstructionId", DBNull.Value);

                    }
                    command1.Parameters.AddWithValue("@JobAuditTrailId", Id);
                    if (AccountsQty != null)
                    {
                        command1.Parameters.AddWithValue("@AccountsQty", AccountsQty);
                    }
                    else
                    {
                        command1.Parameters.AddWithValue("@AccountsQty", DBNull.Value);

                    }
                    if (ImpressionQty != null)
                    {
                        command1.Parameters.AddWithValue("@ImpressionQty", ImpressionQty);
                    }
                    else
                    {
                        command1.Parameters.AddWithValue("@ImpressionQty", DBNull.Value);

                    }
                    if (PagesQty != null)
                    {
                        command1.Parameters.AddWithValue("@PagesQty", PagesQty);
                    }
                    else
                    {
                        command1.Parameters.AddWithValue("@PagesQty", DBNull.Value);

                    }
                    command1.Parameters.AddWithValue("@CreatedOn", createdOn);
                    command1.Parameters.AddWithValue("@Path", Path);
                    command1.Parameters.AddWithValue("@JobNameIT", JobNameIT);
                    command1.Parameters.AddWithValue("@JobId", JobId);
                    command1.Parameters.AddWithValue("@ProgramId", ProgramId);
                    command1.Parameters.AddWithValue("@FileId", FileId);

                    if (!string.IsNullOrEmpty(revStrtDateOn))
                    {


                        command1.Parameters.AddWithValue("@RevStrtDateOn", revStrtDateOn);

                    }
                    else
                    {
                        command1.Parameters.AddWithValue("@RevStrtDateOn", null);
                    }
                    command1.Parameters.AddWithValue("@RevStrtTime", RevStrtTime);
                    if (!string.IsNullOrEmpty(dateProcessItOn))
                    {


                        command1.Parameters.AddWithValue("@DateProcessItOn", dateProcessItOn);
                    }
                    else
                    {
                        command1.Parameters.AddWithValue("@DateProcessItOn", null);
                    }
                    command1.Parameters.AddWithValue("@TimeProcessIt", TimeProcessIt);
                    if (!string.IsNullOrEmpty(dateApproveOn))
                    {

                        command1.Parameters.AddWithValue("@DateApproveOn", dateApproveOn);
                    }
                    else
                    {
                        command1.Parameters.AddWithValue("@DateApproveOn", null);
                    }
                    command1.Parameters.AddWithValue("@DateApproveTime", DateApproveTime);
                    command1.Parameters.AddWithValue("@AccQty", AccQty);
                    command1.Parameters.AddWithValue("@ImpQty", ImpQty);
                    command1.Parameters.AddWithValue("@PageQty", PageQty);
                    command1.Parameters.AddWithValue("@FirstRecord", FirstRecord);
                    command1.Parameters.AddWithValue("@LastRecord", LastRecord);
                    command1.Parameters.AddWithValue("@Remark", Remark);
                    command1.Parameters.AddWithValue("@Type", Type);
                    command1.Parameters.AddWithValue("@Status", "Waiting Approval");
                    command1.Parameters.AddWithValue("@CreateByIt", IdentityName.ToString());
                    if (!string.IsNullOrEmpty(imageInDateOn))
                    {


                        command1.Parameters.AddWithValue("@ImageInDateOn", imageInDateOn);
                    }
                    else
                    {
                        command1.Parameters.AddWithValue("@ImageInDateOn", null);
                    }
                    command1.Parameters.AddWithValue("@ImageInTime", ImageInTime);
                    if (!string.IsNullOrEmpty(revisedInDateOn))
                    {

                        command1.Parameters.AddWithValue("@RevisedInDateOn", revisedInDateOn);
                    }
                    else
                    {
                        command1.Parameters.AddWithValue("@RevisedInDateOn", null);
                    }




                    command1.ExecuteNonQuery();
                    cn1.Close();

                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {


                        cn.Open();
                        SqlCommand command;
                        command = new SqlCommand("UPDATE [dbo].[JobInstruction] SET STATUS='Processing JAT',ModifiedOn=@ModifiedOn WHERE Id=@Id", cn);
                        command.Parameters.AddWithValue("@ModifiedOn", createdOn);
                        command.Parameters.AddWithValue("@Id", Id);
                        command.ExecuteNonQuery();
                        cn.Close();

                        TempData["msg"] = "<script>alert('SUCCESSFULLY SUBMIT AUDIT TRAIL !');</script>";

                        return RedirectToAction("ManageJAT", "ITO");


                    }



                }

            }

        }
        using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn2))
        {
            cn2.Open();
            command.CommandText = @"SELECT JobAuditTrailDetail.JobSheetNo, JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.ProductName, JobAuditTrailDetail.Status, JobAuditTrailDetail.RevStrtTime, JobAuditTrailDetail.DateApproveTime, JobAuditTrailDetail.ImageInTime,
                         JobAuditTrailDetail.NotesByIT, JobAuditTrailDetail.NotesByProduction, JobAuditTrailDetail.NotesByPurchasing, JobAuditTrailDetail.NotesByEngineering, JobAuditTrailDetail.NotesByArtwork, JobAuditTrailDetail.NotesByFinance, JobAuditTrailDetail.NotesByDCP, JobAuditTrailDetail.RevStrtDateOn, JobAuditTrailDetail.DateApproveOn, 
                                   JobAuditTrailDetail.ImageInDateOn, JobAuditTrailDetail.RevisedInDateOn, JobAuditTrailDetail.DateProcessItOn, JobAuditTrailDetail.TimeProcessIt, JobAuditTrailDetail.TotalAuditTrail
                                   FROM  JobInstruction INNER JOIN
                                   JobAuditTrailDetail ON JobInstruction.Id = JobAuditTrailDetail.JobInstructionId";
            command.Parameters.AddWithValue("@JobAuditTrailId", Id);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader.IsDBNull(0) == false)
                {
                    ViewBag.JobSheetNo = reader.GetString(0);
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
                    ViewBag.Status = reader.GetString(3);
                }
                if (reader.IsDBNull(4) == false)
                {
                    ViewBag.RevStrtTime = reader.GetString(4);
                }

                if (reader.IsDBNull(5) == false)
                {
                    ViewBag.FileId = reader.GetString(5);
                }
                if (reader.IsDBNull(6) == false)
                {
                    ViewBag.NotesByIT = reader.GetString(6);
                }
                if (reader.IsDBNull(7) == false)
                {
                    ViewBag.NotesByProduction = reader.GetString(7);
                }
                if (reader.IsDBNull(8) == false)
                {
                    ViewBag.DateProcessItOn = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(8));
                }
                if (reader.IsDBNull(9) == false)
                {
                    ViewBag.TimeProcessIt = reader.GetString(9);
                }
                if (reader.IsDBNull(10) == false)
                {
                    ViewBag.DateApproveOn = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(10));
                }
                if (reader.IsDBNull(11) == false)
                {
                    ViewBag.DateApproveTime = reader.GetString(11);
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
                    ViewBag.PageQty = reader.GetDateTime(14);
                }
                if (reader.IsDBNull(15) == false)
                {
                    ViewBag.FirstRecord = reader.GetString(15);
                }
                if (reader.IsDBNull(16) == false)
                {
                    ViewBag.LastRecord = reader.GetString(16);
                }
                if (reader.IsDBNull(17) == false)
                {
                    ViewBag.Remark = reader.GetString(17);
                }
                if (reader.IsDBNull(18) == false)
                {
                    ViewBag.Type = reader.GetString(18);
                }
                if (reader.IsDBNull(19) == false)
                {
                    ViewBag.LogTagNo = reader.GetString(19);
                }

                if (reader.IsDBNull(20) == false)
                {
                    ViewBag.NotesByProduction = reader.GetString(20);
                }
                if (reader.IsDBNull(21) == false)
                {
                    ViewBag.NotesByPurchasing = reader.GetString(21);
                }
                if (reader.IsDBNull(22) == false)
                {
                    ViewBag.NotesByEngineering = reader.GetString(22);
                }
                if (reader.IsDBNull(23) == false)
                {
                    ViewBag.NotesByArtwork = reader.GetString(23);
                }
                if (reader.IsDBNull(24) == false)
                {
                    ViewBag.NotesByFinance = reader.GetString(24);
                }
                if (reader.IsDBNull(25) == false)
                {
                    ViewBag.NotesByDCP = reader.GetString(25);
                }
                if (reader.IsDBNull(26) == false)
                {
                    ViewBag.JobAuditTrailId = reader.GetGuid(26);
                }
                if (reader.IsDBNull(27) == false)
                {
                    ViewBag.ImageInDateOn = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(27));
                }
                if (reader.IsDBNull(28) == false)
                {
                    ViewBag.ImageInTime = reader.GetString(28);
                }
                if (reader.IsDBNull(29) == false)
                {
                    ViewBag.RevisedInDateOn = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(29));
                }
            }
            cn2.Close();
        }




        return View();
    }


    public ActionResult ReloadJobAuditTrail()
    {


        List<JobAuditTrailDetail> viewJobAuditTrail = new List<JobAuditTrailDetail>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT Id,ModeLog,Path,JobNameIT,JobId,ProgramId,FileId,RevStrtDateOn,RevStrtTime,
                                           DateProcessItOn,TimeProcessIt,DateApproveOn,DateApproveTime,AccQty,
                                           ImpQty,PageQty,FirstRecord,LastRecord,Remark,Type,JobAuditTrailId
                                      FROM [dbo].[JobAuditTrailDetail]  
                                      WHERE JobAuditTrailId=@Id AND Status='Waiting Approval'";
            command.Parameters.AddWithValue("@Id", Session["JobAuditTrailId"].ToString());
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
                        model.ModeLog = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        model.Path = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        model.JobNameIT = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        model.JobId = reader.GetString(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        model.ProgramId = reader.GetString(5);
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        model.FileId = reader.GetString(6);
                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        model.RevStrtDateOnTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(7));
                    }
                    if (reader.IsDBNull(8) == false)
                    {
                        model.RevStrtTime = reader.GetString(8);
                    }
                    if (reader.IsDBNull(9) == false)
                    {
                        model.DateProcessItOnTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(9));
                    }
                    if (reader.IsDBNull(10) == false)
                    {
                        model.TimeProcessIt = reader.GetString(10);
                    }
                    if (reader.IsDBNull(11) == false)
                    {
                        model.DateApproveOnTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(11));
                    }
                    if (reader.IsDBNull(12) == false)
                    {
                        model.DateApproveTime = reader.GetString(12);
                    }
                    if (reader.IsDBNull(13) == false)
                    {
                        model.AccQty = reader.GetString(13);
                    }
                    if (reader.IsDBNull(14) == false)
                    {
                        model.ImpQty = reader.GetString(14);
                    }
                    if (reader.IsDBNull(15) == false)
                    {
                        model.PageQty = reader.GetString(15);
                    }
                    if (reader.IsDBNull(16) == false)
                    {
                        model.FirstRecord = reader.GetString(16);
                    }
                    if (reader.IsDBNull(17) == false)
                    {
                        model.LastRecord = reader.GetString(17);
                    }
                    if (reader.IsDBNull(18) == false)
                    {
                        model.Remark = reader.GetString(18);
                    }
                    if (reader.IsDBNull(19) == false)
                    {
                        model.Type = reader.GetString(19);
                    }


                }
                viewJobAuditTrail.Add(model);
            }
            cn.Close();
            return Json(viewJobAuditTrail);
        }
    }

    public ActionResult DeleteJAT(string Id, string JobAuditTrailId, string LogTagNo)
    {

        ViewBag.Id = Id;
        ViewBag.JobAuditTrailId = JobAuditTrailId;
        ViewBag.LogTagNo = LogTagNo;


        if (Id != null)
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT LogTagNo, JobAuditTrailId
                                          FROM [dbo].[JobAuditTrailDetail]
                                          WHERE Id=@Id";
                command.Parameters.AddWithValue("@Id", Id.ToString());
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.IsDBNull(0) == false)
                    {

                        using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            cn3.Open();
                            SqlCommand command3;
                            command3 = new SqlCommand("DELETE [dbo].[JobAuditTrailDetail] WHERE Id=@Id", cn3);
                            command3.Parameters.AddWithValue("@Id", Id);
                            command3.ExecuteNonQuery();
                            cn3.Close();

                        }

                        using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            cn2.Open();
                            SqlCommand comm = new SqlCommand("SELECT COUNT(*) FROM [dbo].[JobAuditTrailDetail] WHERE JobAuditTrailId=@JobAuditTrailId", cn2);
                            comm.Parameters.AddWithValue("@JobAuditTrailId", Session["Id"].ToString());
                            Int32 count = (Int32)comm.ExecuteScalar();
                            string TotalAT = count.ToString();

                            using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                            {
                                cn1.Open();
                                SqlCommand comm1 = new SqlCommand("UPDATE [dbo].[JobAuditTrail] SET TotalAuditTrail=@TotalAuditTrail WHERE Id=@Id", cn1);
                                comm1.Parameters.AddWithValue("@TotalAuditTrail", TotalAT);
                                comm1.Parameters.AddWithValue("@Id", Session["Id"].ToString());
                                comm1.ExecuteNonQuery();
                                cn1.Close();
                            }

                            cn2.Close();
                        }

                    }

                    if (reader.IsDBNull(1) == false)
                    {

                        return RedirectToAction("CreateLogTagNo", "ITO", new { Id = Session["JobAuditTrailId"].ToString() });

                    }
                }
                cn.Close();
            }

        }

        return RedirectToAction("CreateLogTagNo", "ITO", new { Id = Session["JobAuditTrailId"].ToString() });
    }



    [ValidateInput(false)]
    public ActionResult SubmitPlanner(JobAuditTrailDetail get, JobAuditTrailDetail JobAuditTrailDetail, string set,
                                  string Id, string JobAuditTrailId, string AuditTrail, string LogTagNo, string JobRequest,
                                  string Customer_Name, string ProductName, string JobSheetNo, string JobClass, string Frequency,
                                  string JobType, string AccountsQty, string ImpressionQty, string PagesQty, string TotalAuditTrail,
                                  string Status, string CreateByIT, string FileId, string JobInstructionId, string PlanDatePostOn, string ItSubmitOn, string StatusITO,string Cust_Department,
                                  string CreatedOn,string EndTime,string EndDateOn, string DateApproveOn, string DateApproveTime,string ProcessDate, string ProcessEnd,string ProcessEndDateOn, string ProcessEndTime,
                                  string ProcessStartDateOn, string ProcessStartTime, string RevEndDateOn, string RevEndTimeOn, string RevisedInDateOn,string RevStrtDateOn,string RevStrtTime, string StartDateOn, string StartTime,
                                  string TimeEndProcessIt, string TimeProcessIt, string TimeTaken)
    {

        using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        {
            cn1.Open();

            SqlCommand cmd0 = new SqlCommand(@"SELECT MAX(Customer_Name), MAX(ProductName), MAX(JobClass), MAX(JobType),MAX(Cust_Department),SUM(CAST(AccQty AS INT)) AS AccQty,SUM(CAST(ImpQty AS INT)) AS ImpQty,SUM(CAST(PageQty AS INT)) AS PageQty,
                FORMAT(CONVERT(date, MAX(RevStrtDateOn)), 'yyyy-MM-dd') AS RevStrtDateOn,MAX(RevStrtTime),FORMAT(CONVERT(date, MAX(ProcessDate)), 'yyyy-MM-dd') AS ProcessDate,MAX(TimeProcessIt),FORMAT(CONVERT(date, MAX(ProcessEnd)), 'yyyy-MM-dd') AS ProcessEnd,MAX(TimeEndProcessIt),
                FORMAT(CONVERT(date, MAX(DateApproveOn)), 'yyyy-MM-dd') AS DateApproveOn,MAX(DateApproveTime),MAX(CreatedOn) as CreatedOn, MAX(CreateByIT), FORMAT(CONVERT(date, MAX(RevEndDateOn)), 'yyyy-MM-dd') AS RevEndDateOn,
                MAX(RevEndTimeOn), MAX(TimeTaken) AS TimeTaken FROM [dbo].[JobAuditTrailDetail] WHERE LogTagNo=@LogTagNo1 ", cn1);

            //SqlCommand cmd0 =new SqlCommand ("SELECT Customer_Name, ProductName, JobClass, JobType,Cust_Department,SUM(CAST(AccQty AS int) AS AccQty,SUM(CAST(ImpQty AS int) AS ImpQty,SUM(CAST(PageQty AS int) AS PageQty," +
            //           "FORMAT(CONVERT(date, RevStrtDateOn), 'yyyy-MM-dd'),RevStrtTime,FORMAT(CONVERT(date, ProcessDate), 'yyyy-MM-dd'),TimeProcessIt,FORMAT(CONVERT(date, ProcessEnd), 'yyyy-MM-dd'),TimeEndProcessIt,FORMAT(CONVERT(date, DateApproveOn), 'yyyy-MM-dd'),DateApproveTime,PaperType,FORMAT(CONVERT(date, RevisedInDateOn), 'yyyy-MM-dd')," +
            //           "PaperType,CreatedOn, CreateByIT, FORMAT(CONVERT(date, RevEndDateOn), 'yyyy-MM-dd'), RevEndTimeOn FROM [dbo].[JobAuditTrailDetail] WHERE LogTagNo=@LogTagNo1",cn1);

            cmd0.Parameters.AddWithValue("LogTagNo1", LogTagNo);
            SqlDataReader rm0 = cmd0.ExecuteReader();

            while (rm0.Read())
            {

                if (rm0.IsDBNull(0) == false)
                {
                    Customer_Name = rm0.GetString(0);
                }
                if (rm0.IsDBNull(1) == false)
                {
                    ProductName = rm0.GetString(1);
                }
                if (rm0.IsDBNull(2) == false)
                {
                    JobClass = rm0.GetString(2);
                }
                if (rm0.IsDBNull(3) == false)
                {
                    JobType = rm0.GetString(3);
                }

                if (rm0.IsDBNull(4) == false)
                {
                    Cust_Department = rm0.GetString(4);
                }
                if (rm0.IsDBNull(5) == false)
                {
                    AccountsQty = rm0["AccQty"].ToString();
                }

                if (rm0.IsDBNull(6) == false)
                {
                    ImpressionQty = rm0["ImpQty"].ToString();
                }
                if (rm0.IsDBNull(7) == false)
                {
                    PagesQty = rm0["PageQty"].ToString();
                }
                //if (rm0.IsDBNull(9) == false)
                //{
                //    JobNameIT = rm0.GetString(11);
                //}
                if (rm0.IsDBNull(8) == false)
                {
                    //ViewBag.RevStrtDateOn = String.Format("{0:dd/MM/yyyy }", (DateTime)rm0.GetDateTime(19));
                    RevStrtDateOn = rm0["RevStrtDateOn"].ToString();
                }
                if (rm0.IsDBNull(9) == false)
                {
                    RevStrtTime = rm0.GetString(9);
                }
                if (rm0.IsDBNull(10) == false)
                {
                    //ViewBag.ProcessDate = String.Format("{0:dd/MM/yyyy }", (DateTime)rm0.GetDateTime(21));
                    ProcessDate = rm0["ProcessDate"].ToString();
                }
                if (rm0.IsDBNull(11) == false)
                {
                    TimeProcessIt = rm0.GetString(11);
                }
                if (rm0.IsDBNull(12) == false)
                {
                    //ViewBag.ProcessEnd = String.Format("{0:dd/MM/yyyy }", (DateTime)rm0.GetDateTime(23));
                    ProcessEnd = rm0["ProcessEnd"].ToString();
                }
                if (rm0.IsDBNull(13) == false)
                {
                    TimeEndProcessIt = rm0.GetString(13);
                }
                if (rm0.IsDBNull(14) == false)
                {
                    //ViewBag.DateApproveOn = String.Format("{0:dd/MM/yyyy }", (DateTime)rm0.GetDateTime(25));
                    DateApproveOn = rm0["DateApproveOn"].ToString();
                }
                if (rm0.IsDBNull(15) == false)
                {
                    DateApproveTime = rm0.GetString(15);
                }
                //if (rm0.IsDBNull(27) == false)
                //{
                //    ImageInDateOn = rm0.GetString(27);

                //    //ViewBag.ImageInDateOn = String.Format("{0:dd/MM/yyyy }", (DateTime)rm0.GetDateTime(28));
                //}
                //if (rm0.IsDBNull(28) == false)
                //{
                //    ImageInTime = rm0.GetString(28);
                //}

                if (rm0.IsDBNull(16) == false)
                {
                    CreatedOn = rm0["CreatedOn"].ToString();
                }
                if (rm0.IsDBNull(17) == false)
                {
                    CreateByIT = rm0.GetString(17);
                }
                if (rm0.IsDBNull(18) == false)
                {
                    RevEndDateOn = rm0["RevEndDateOn"].ToString();
                }
                if (rm0.IsDBNull(19) == false)
                {
                    RevEndTimeOn = rm0.GetString(19);
                }
                if (rm0.IsDBNull(20) == false)
                {
                    TimeTaken = rm0.GetString(20);
                }

            }


            SqlCommand getPaperType = new SqlCommand("SELECT PaperType FROM JobInstruction WHERE JobSheetNo=@PaperJS", cn1);
            getPaperType.Parameters.AddWithValue("@PaperJS", JobSheetNo);
            SqlDataReader rmPaper = getPaperType.ExecuteReader();

            if(rmPaper.HasRows)
            {
                while(rmPaper.Read())
                {
                    if(!rmPaper.IsDBNull(0))
                    {
                        PaperType = rmPaper.GetString(0);
                    }
                }
            }   



            SqlCommand cmd1 = new SqlCommand("INSERT INTO [dbo].[DailyTracking] (Id,StartDateOn,StartTime,EndDateOn,EndTime,ProcessStartDateOn,ProcessStartTime,ProcessEndDateOn,ProcessEndTime,TimeTaken,DateApproveOn,DateApproveTime,CreatedOn, " +
            "JobSheetNo, LogTagNo, JobClass, JobType, Customer_Name, Cust_Department, ProductName, PIC, AccountsQty,ImpressionQty, PagesQty, Status,PaperType) " +
            "VALUES (@Id,@StartDateOn,@StartTime,@EndDateOn,@EndTime,@ProcessStartDateOn,@ProcessStartTime,@ProcessEndDateOn,@ProcessEndTime,@TimeTaken,@DateApproveOn,@DateApproveTime,@CreatedOn, @JobSheetNo, @LogTagNo, @JobClass, @JobType, @Customer_Name, " +
            "@Cust_Department, @ProductName, @PIC, @AccountsQty, @ImpressionQty, @PagesQty, @Status,@PaperType) ", cn1);
            //Guid newId = Guid.NewGuid();

            cmd1.Parameters.AddWithValue("@Id", Guid.NewGuid());

            if (RevStrtDateOn == null)
            {
                cmd1.Parameters.Add(new SqlParameter { ParameterName = "@StartDateOn", Value = DBNull.Value });
            }
            else
            {
                cmd1.Parameters.AddWithValue("@StartDateOn", RevStrtDateOn);
            }

            if (RevStrtTime == null)
            {
                cmd1.Parameters.Add(new SqlParameter { ParameterName = "@StartTime", Value = DBNull.Value });
            }
            else
            {
                cmd1.Parameters.AddWithValue("@StartTime", RevStrtTime);
            }

            if (RevEndDateOn == null)
            {
                cmd1.Parameters.Add(new SqlParameter { ParameterName = "@EndDateOn", Value = DBNull.Value });
            }
            else
            {
                cmd1.Parameters.AddWithValue("@EndDateOn", RevEndDateOn);
            }

            if (RevEndTimeOn == null)
            {
                cmd1.Parameters.Add(new SqlParameter { ParameterName = "@EndTime", Value = DBNull.Value });
            }
            else
            {
                cmd1.Parameters.AddWithValue("@EndTime", RevEndTimeOn);
            }

            if (ProcessDate == null)
            {
                cmd1.Parameters.Add(new SqlParameter { ParameterName = "@ProcessStartDateOn", Value = DBNull.Value });
            }
            else
            {
                cmd1.Parameters.AddWithValue("@ProcessStartDateOn", ProcessDate);
            }

            if (TimeProcessIt == null)
            {
                cmd1.Parameters.Add(new SqlParameter { ParameterName = "@ProcessStartTime", Value = DBNull.Value });
            }
            else
            {
                cmd1.Parameters.AddWithValue("@ProcessStartTime", TimeProcessIt);
            }

            if (ProcessEnd == null)
            {
                cmd1.Parameters.Add(new SqlParameter { ParameterName = "@ProcessEndDateOn", Value = DBNull.Value });
            }
            else
            {
                cmd1.Parameters.AddWithValue("@ProcessEndDateOn", ProcessEnd);
            }

            if (TimeEndProcessIt == null)
            {
                cmd1.Parameters.Add(new SqlParameter { ParameterName = "@ProcessEndTime", Value = DBNull.Value });
            }
            else
            {
                cmd1.Parameters.AddWithValue("@ProcessEndTime", TimeEndProcessIt);
            }

            if (TimeTaken == null)
            {
                cmd1.Parameters.Add(new SqlParameter { ParameterName = "@TimeTaken", Value = DBNull.Value });
            }
            else
            {
                cmd1.Parameters.AddWithValue("@TimeTaken", TimeTaken);
            }

            if (DateApproveOn == null)
            {
                cmd1.Parameters.Add(new SqlParameter { ParameterName = "@DateApproveOn", Value = DBNull.Value });
            }
            else
            {
                cmd1.Parameters.AddWithValue("@DateApproveOn", DateApproveOn);
            }

            if (DateApproveTime == null)
            {
                cmd1.Parameters.Add(new SqlParameter { ParameterName = "@DateApproveTime", Value = DBNull.Value });
            }
            else
            {
                cmd1.Parameters.AddWithValue("@DateApproveTime", DateApproveTime);
            }

            cmd1.Parameters.AddWithValue("@CreatedOn", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));

            if (JobSheetNo == null)
            {
                cmd1.Parameters.Add(new SqlParameter { ParameterName = "@JobSheetNo", Value = DBNull.Value });
            }
            else
            {
                cmd1.Parameters.AddWithValue("@JobSheetNo", JobSheetNo);
            }

            if (LogTagNo == null)
            {
                cmd1.Parameters.Add(new SqlParameter { ParameterName = "@LogTagNo", Value = DBNull.Value });
            }
            else
            {
                cmd1.Parameters.AddWithValue("@LogTagNo", LogTagNo);
            }


            if (PaperType == null)
            {
                cmd1.Parameters.Add(new SqlParameter { ParameterName = "@PaperType", Value = DBNull.Value });
            }
            else
            {
                cmd1.Parameters.AddWithValue("@PaperType", PaperType);
            }

            if (JobClass == null)
            {
                cmd1.Parameters.Add(new SqlParameter { ParameterName = "@JobClass", Value = DBNull.Value });
            }
            else
            {
                cmd1.Parameters.AddWithValue("@JobClass", JobClass);
            }

            if (JobType == null)
            {
                cmd1.Parameters.Add(new SqlParameter { ParameterName = "@JobType", Value = DBNull.Value });
            }
            else
            {
                cmd1.Parameters.AddWithValue("@JobType", JobType);
            }

            if (Customer_Name == null)
            {
                cmd1.Parameters.Add(new SqlParameter { ParameterName = "@Customer_Name", Value = DBNull.Value });
            }
            else
            {
                cmd1.Parameters.AddWithValue("@Customer_Name", Customer_Name);
            }

            if (Cust_Department == null)
            {
                cmd1.Parameters.Add(new SqlParameter { ParameterName = "@Cust_Department", Value = DBNull.Value });
            }
            else
            {
                cmd1.Parameters.AddWithValue("@Cust_Department", Cust_Department);
            }

            if (ProductName == null)
            {
                cmd1.Parameters.Add(new SqlParameter { ParameterName = "@ProductName", Value = DBNull.Value });
            }
            else
            {
                cmd1.Parameters.AddWithValue("@ProductName", ProductName);
            }

            if (CreateByIT == null)
            {
                cmd1.Parameters.Add(new SqlParameter { ParameterName = "@PIC", Value = DBNull.Value });
            }
            else
            {
                cmd1.Parameters.AddWithValue("@PIC", CreateByIT);
            }

            if (AccountsQty == null)
            {
                cmd1.Parameters.Add(new SqlParameter { ParameterName = "@AccountsQty", Value = DBNull.Value });
            }
            else
            {
                cmd1.Parameters.AddWithValue("@AccountsQty", AccountsQty);
            }

            if (ImpressionQty == null)
            {
                cmd1.Parameters.Add(new SqlParameter { ParameterName = "@ImpressionQty", Value = DBNull.Value });
            }
            else
            {
                cmd1.Parameters.AddWithValue("@ImpressionQty", ImpressionQty);
            }

            if (PagesQty == null)
            {
                cmd1.Parameters.Add(new SqlParameter { ParameterName = "@PagesQty", Value = DBNull.Value });
            }
            else
            {
                cmd1.Parameters.AddWithValue("@PagesQty", PagesQty);
            }

            cmd1.Parameters.AddWithValue("@Status", "Completed");
            //cmd1.Parameters.AddWithValue("@JobAuditTrail", Id);

            cmd1.ExecuteNonQuery();

            cn1.Close();
        }


        if (!string.IsNullOrEmpty(LogTagNo) && JobType == "E-BLAST")
        {
            Debug.WriteLine("status : QME");
            //using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            //{
            //    cn1.Open();
            //    SqlCommand command1;
            //    command1 = new SqlCommand("UPDATE [dbo].[JobInstruction] SET STATUS='PLANNER' WHERE Id=@Id", cn1);
            //    command1.Parameters.AddWithValue("@Id", Id);
            //    command1.ExecuteNonQuery();
            //    cn1.Close();
            //}

            using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                get.JobRequest = Convert.ToDateTime(get.JobRequestTxt);
                string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                cn1.Open();
                SqlCommand command1;
                command1 = new SqlCommand("UPDATE [dbo].[JobAuditTrailDetail] SET STATUS='PLANNER' WHERE LogTagNo=@LogTagNo1", cn1);
                command1.Parameters.AddWithValue("@LogTagNo1", LogTagNo);
                command1.ExecuteNonQuery();

                SqlCommand cmd2 = new SqlCommand("SELECT JobSheetNo FROM JobAuditTrailDetail WHERE LogTagNo=@LogTagNo2", cn1);
                cmd2.Parameters.AddWithValue("@LogTagNo2", LogTagNo);
                SqlDataReader rm2 = cmd2.ExecuteReader();

                while (rm2.Read())
                {
                    JobSheetNo = rm2.GetString(0);
                    Debug.WriteLine("JobSheetNo : " + JobSheetNo);
                }

                //SqlCommand cmd3 = new SqlCommand("UPDATE JobInstruction SET Status='PLANNER' WHERE JobSheetNo=@JobSheetNo1", cn1);
                //cmd3.Parameters.AddWithValue("@JobSheetNo1", JobSheetNo);
                //cmd3.ExecuteNonQuery();

                cn1.Close();

                TempData["msg"] = "<script>alert('SUCCESSFULLY SUBMIT TO QM !');</script>";

                return RedirectToAction("ManageJAT", "ITO");
            }


        }

        else if (!string.IsNullOrEmpty(LogTagNo) && JobType == "DCP")
        {

            Debug.WriteLine("status : QME BUT DCP");


            //using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            //{
            //    cn1.Open();
            //    SqlCommand command1;
            //    command1 = new SqlCommand("UPDATE [dbo].[JobInstruction] SET STATUS='QME' WHERE Id=@Id", cn1);
            //    command1.Parameters.AddWithValue("@Id", Id);
            //    command1.ExecuteNonQuery();
            //    cn1.Close();
            //}

            using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                get.JobRequest = Convert.ToDateTime(get.JobRequestTxt);
                string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                cn1.Open();
                SqlCommand command1;
                command1 = new SqlCommand("UPDATE [dbo].[JobAuditTrailDetail] SET STATUS='PLANNER' WHERE LogTagNo=@LogTagNo1", cn1);
                command1.Parameters.AddWithValue("@LogTagN01", LogTagNo);
                command1.ExecuteNonQuery();

                SqlCommand cmd2 = new SqlCommand("SELECT JobSheetNo FROM JobAuditTrailDetail WHERE LogTagNo=@LogTagNo2", cn1);
                cmd2.Parameters.AddWithValue("@LogTagNo2", LogTagNo);
                SqlDataReader rm2 = cmd2.ExecuteReader();

                while (rm2.Read())
                {
                    JobSheetNo = rm2.GetString(0);
                    Debug.WriteLine("JobSheetNo : " + JobSheetNo);
                }

                //SqlCommand cmd3 = new SqlCommand("UPDATE JobInstruction SET Status='PLANNER' WHERE JobSheetNo=@JobSheetNo1", cn1);
                //cmd3.Parameters.AddWithValue("@JobSheetNo1", JobSheetNo);
                //cmd3.ExecuteNonQuery();

                cn1.Close();

                TempData["msg"] = "<script>alert('SUCCESSFULLY SUBMIT TO QM !');</script>";

                return RedirectToAction("ManageJAT", "ITO");
            }

        }

        else if (!string.IsNullOrEmpty(LogTagNo) && JobClass == "DAILY")
        {

            Debug.WriteLine("status : PLANNER DAILY");


            //using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            //{
            //    cn1.Open();
            //    SqlCommand command1;
            //    command1 = new SqlCommand("UPDATE [dbo].[JobInstruction] SET STATUS='PLANNER' WHERE Id=@Id", cn1);
            //    command1.Parameters.AddWithValue("@Id", Id);
            //    command1.ExecuteNonQuery();
            //    cn1.Close();
            //}

            using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                get.JobRequest = Convert.ToDateTime(get.JobRequestTxt);
                string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                cn1.Open();
                SqlCommand command1;
                command1 = new SqlCommand("UPDATE [dbo].[JobAuditTrailDetail] SET STATUS='PLANNER' WHERE LogTagNo=@LogTagNo1", cn1);
                command1.Parameters.AddWithValue("@LogTagNo1", LogTagNo);
                command1.ExecuteNonQuery();

                SqlCommand cmd2 = new SqlCommand("SELECT JobSheetNo FROM JobAuditTrailDetail WHERE LogTagNo=@LogTagNo2", cn1);
                cmd2.Parameters.AddWithValue("@LogTagNo2", LogTagNo);
                SqlDataReader rm2 = cmd2.ExecuteReader();

                while (rm2.Read())
                {
                    JobSheetNo = rm2.GetString(0);
                    Debug.WriteLine("JobSheetNo : " + JobSheetNo);
                }

                //SqlCommand cmd3 = new SqlCommand("UPDATE JobInstruction SET Status='PLANNER' WHERE JobSheetNo=@JobSheetNo1", cn1);
                //cmd3.Parameters.AddWithValue("@JobSheetNo1", JobSheetNo);
                //cmd3.ExecuteNonQuery();

                cn1.Close();

                TempData["msg"] = "<script>alert('SUCCESSFULLY SUBMIT TO PLANNER !');</script>";

                return RedirectToAction("ManageJAT", "ITO");
            }



        }
        else if (!string.IsNullOrEmpty(LogTagNo) && JobType == "SELF MAILER")
        {

            //using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            //{
            //    cn1.Open();
            //    SqlCommand command1;
            //    command1 = new SqlCommand("UPDATE [dbo].[JobInstruction] SET STATUS='PLANNER' WHERE Id=@Id", cn1);
            //    command1.Parameters.AddWithValue("@Id", Id);
            //    command1.ExecuteNonQuery();
            //    cn1.Close();
            //}

            using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                get.JobRequest = Convert.ToDateTime(get.JobRequestTxt);
                string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                cn1.Open();
                SqlCommand command1;
                command1 = new SqlCommand("UPDATE [dbo].[JobAuditTrailDetail] SET STATUS='PLANNER' WHERE LogTagNo=@LogTagNo1", cn1);
                command1.Parameters.AddWithValue("@LogTagNo1", LogTagNo);
                command1.ExecuteNonQuery();

                SqlCommand cmd2 = new SqlCommand("SELECT JobSheetNo FROM JobAuditTrailDetail WHERE LogTagNo=@LogTagNo2", cn1);
                cmd2.Parameters.AddWithValue("@LogTagNo2", LogTagNo);
                SqlDataReader rm2 = cmd2.ExecuteReader();

                while (rm2.Read())
                {
                    JobSheetNo = rm2.GetString(0);
                    Debug.WriteLine("JobSheetNo : " + JobSheetNo);
                }

                //SqlCommand cmd3 = new SqlCommand("UPDATE JobInstruction SET Status='PLANNER' WHERE JobSheetNo=@JobSheetNo1", cn1);
                //cmd3.Parameters.AddWithValue("@JobSheetNo1", JobSheetNo);
                //cmd3.ExecuteNonQuery();

                cn1.Close();

                TempData["msg"] = "<script>alert('SUCCESSFULLY SUBMIT TO PLANNER !');</script>";

                return RedirectToAction("ManageJAT", "ITO");
            }



        }
        else
        {
            if (!string.IsNullOrEmpty(LogTagNo))
            {
                Debug.WriteLine("status : PLANNER ID NOT EMPTY");


                //using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                //{
                //    cn1.Open();
                //    SqlCommand command1;
                //    command1 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET STATUS='PLANNER' WHERE Id=@Id", cn1);
                //    command1.Parameters.AddWithValue("@Id", Id);
                //    command1.ExecuteNonQuery();
                //    cn1.Close();
                //}

                using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    get.JobRequest = Convert.ToDateTime(get.JobRequestTxt);
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                    cn1.Open();
                    SqlCommand command1;
                    command1 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobAuditTrailDetail] SET STATUS='PLANNER' WHERE LogTagNo=@LogTagNo", cn1);
                    command1.Parameters.AddWithValue("@LogTagNo", LogTagNo);
                    command1.ExecuteNonQuery();

                    SqlCommand cmd2 = new SqlCommand("SELECT JobSheetNo FROM JobAuditTrailDetail WHERE LogTagNo=@LogTagNo2", cn1);
                    cmd2.Parameters.AddWithValue("@LogTagNo2", LogTagNo);
                    SqlDataReader rm2 = cmd2.ExecuteReader();

                    while (rm2.Read())
                    {
                        JobSheetNo = rm2.GetString(0);
                        Debug.WriteLine("JobSheetNo : " + JobSheetNo);
                    }

                    //SqlCommand cmd3 = new SqlCommand("UPDATE JobInstruction SET Status='PLANNER' WHERE JobSheetNo=@JobSheetNo1", cn1);
                    //cmd3.Parameters.AddWithValue("@JobSheetNo1", JobSheetNo);
                    //cmd3.ExecuteNonQuery();


                    cn1.Close();

                    TempData["msg"] = "<script>alert('SUCCESSFULLY SUBMIT TO PLANNER !');</script>";

                    return RedirectToAction("ManageJAT", "ITO");
                }

            }


            List<JobInstruction> JobInstructionlist3 = new List<JobInstruction>();
            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command2 = new SqlCommand("", cn2))
            {
                int _bil = 1;
                cn2.Open();
                command2.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass, 
                                                JobType, SalesExecutiveBy
                                      FROM [IflowSeed].[dbo].[JobInstruction]  
                                      WHERE Id=@Id";
                command2.Parameters.AddWithValue("@Id", Id);
                var reader = command2.ExecuteReader();
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
                            model.SalesExecutiveBy = reader.GetString(5);
                        }
                    }
                    JobInstructionlist3.Add(model);
                }



            }
            return View();


        }

       

    }


    public ActionResult ReloadMedia()
    {
        var Id = Session["Id"];
        ViewBag.Id = Id;

        List<SampleProduct> viewFileStore = new List<SampleProduct>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT Picture_FileId,Id
                                      FROM [IflowSeed].[dbo].[SampleProductAudit]  
                                      WHERE AuditTrail=@Id                                   
                                      ORDER BY Picture_FileId DESC";
            command.Parameters.AddWithValue("@Id", Session["Id"].ToString());
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

    public ActionResult UploadMedia(SampleProductAudit ModelSample, string AuditTrail)
    {
        var IdentityName = @Session["Fullname"];
        var Id = Session["Id"];
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.IsRole = @Session["Role"];
        string Deptment = @Session["Department"].ToString();




        if (ModelSample.FileUploadFile2 != null && Id.ToString() != null && ModelSample.Set == "save")
        {
            var fileName = Path.GetFileName(ModelSample.FileUploadFile2.FileName);
            var path = Path.Combine(Server.MapPath("~/FileStore"), fileName);
            ModelSample.FileUploadFile2.SaveAs(path);

            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                Guid guidId = Guid.NewGuid();
                string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                cn2.Open();
                SqlCommand command;
                command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[SampleProductAudit] (Id,CreatedOn,Picture_FileId,AuditTrail,Picture_Extension,Code,CreateBy) values (@Id,@CreatedOn,@Picture_FileId,@AuditTrail,@Picture_Extension,@Code,@CreateBy)", cn2);
                command.Parameters.AddWithValue("@Id", guidId);
                command.Parameters.AddWithValue("@CreatedOn", createdOn);
                command.Parameters.AddWithValue("@Picture_FileId", fileName.Trim());
                command.Parameters.AddWithValue("@AuditTrail", Id);
                command.Parameters.AddWithValue("@Picture_Extension", ModelSample.FileUploadFile2.ContentType);
                command.Parameters.AddWithValue("@Code", "AT");
                command.Parameters.AddWithValue("@CreateBy", IdentityName.ToString());
                command.ExecuteNonQuery();
                cn2.Close();

            }

            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn2.Open();
                SqlCommand command;
                command = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobAuditTrailDetail] SET JobAuditTrailId=@JobAuditTrailId WHERE Id=@Id", cn2);
                command.Parameters.AddWithValue("@JobAuditTrailId", Id);
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn2.Close();

            }


            return RedirectToAction("ManageJAT", "ITO", new { Id = Session["Id"].ToString() });
        }

        if (ModelSample.Set == "back")
        {
            return RedirectToAction("ManageJAT", "ITO", new { Id = Session["Id"].ToString() });
        }

        return View();
    }


    private string NamaCustomer;
    private string NamaProjek;
    private string JobId;
    private string ProgramID;

    private string FileName;
    private string TotalAcc;
    private string TotalPage;
    private string TotalImpression;
    private string FirstRecord;
    private string LastRecord;
    private string JobName;
    private string ProgramName;

    private string FileId;
    private string StatementType;
    private string RunMode;
    private string DTProcess;
    private string TimeProcess;
    private string DataType;
    private string PaperType;





    public ActionResult ImportData(SampleProductAudit ModelSample, string AuditTrail, string Temp)

    {

        var IdentityName = @Session["Fullname"];
        var Id = Session["Id"];
        ViewBag.Id = Id;
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.IsRole = @Session["Role"];
        string Deptment = @Session["Department"].ToString();


        List<SelectListItem> li10 = new List<SelectListItem>();
        li10.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        li10.Add(new SelectListItem { Text = "i2S", Value = "i2S" });
        li10.Add(new SelectListItem { Text = "MBBCC", Value = "MBBCC" });
        li10.Add(new SelectListItem { Text = "BSN", Value = "BSN" });

        ViewData["Temp_"] = li10;


        if (ModelSample.FileUploadFile3 != null && Id.ToString() != null && ModelSample.Set == "save")
        {


            var fileName = Path.GetFileName(ModelSample.FileUploadFile3.FileName);
            var path2 = Path.Combine(Server.MapPath("~/FileStore"), fileName);
            ModelSample.FileUploadFile3.SaveAs(path2);



            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {

                cn2.Open();
                try
                {
                    //------------insert 
                    if (Temp == "i2S")
                    {

                        int ii = 0;

                        {

                            StreamReader rdr = new StreamReader(path2);
                            string line;
                            while ((line = rdr.ReadLine()) != null)
                            {
                                switch (ii)
                                {
                                    case 0:
                                        NamaCustomer = line;
                                        break;
                                    case 1:
                                        NamaProjek = line;
                                        break;
                                    case 3:
                                        string[] data3 = line.Split(':');
                                        JobId = data3[1].ToString();
                                        break;
                                    case 4:
                                        string[] data4 = line.Split(':');
                                        JobName = data4[1].ToString(); ;
                                        break;
                                    case 6:
                                        string[] data6 = line.Split(':');
                                        ProgramID = data6[1].ToString();
                                        break;
                                    case 7:
                                        string[] data7 = line.Split(':');
                                        FileName = data7[1].ToString();
                                        break;
                                    case 8:
                                        string[] data8 = line.Split(':');
                                        TotalAcc = data8[1].ToString();
                                        break;
                                    case 9:
                                        string[] data9 = line.Split(':');
                                        TotalPage = data9[1].ToString();
                                        break;
                                    case 10:
                                        string[] data10 = line.Split(':');
                                        TotalImpression = data10[1].ToString();
                                        break;
                                    case 12:
                                        string[] data12 = line.Split(':');
                                        FirstRecord = data12[1].ToString();
                                        break;
                                    case 13:
                                        string[] data13 = line.Split(':');
                                        LastRecord = data13[1].ToString();
                                        break;
                                }
                                // add=1;
                                ii = ii + 1;

                            }
                        }

                        Guid guidIdx = Guid.NewGuid();

                        SqlCommand command3;
                        command3 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobAuditTrailDetail] SET Id=@Id,CreatedOn=@CreatedOn,JobId=@JobId,JobNameIT=@JobNameIT,ProgramId=@ProgramId,FileId=@FileId,AccQty=@AccQty,PageQty=@PageQty,ImpQty=@ImpQty,FirstRecord=@FirstRecord,LastRecord=@LastRecord,Customer_Name=@Customer_Name,ProductName=@ProductName WHERE Id=@Id", cn2);
                        command3.Parameters.AddWithValue("@Id", Id);
                        command3.Parameters.AddWithValue("@CreatedOn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        if (!string.IsNullOrEmpty(JobId))
                        {

                            command3.Parameters.AddWithValue("@JobId", JobId.Trim());

                        }
                        else
                        {
                            command3.Parameters.AddWithValue("@JobId", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(JobName))
                        {

                            command3.Parameters.AddWithValue("@JobNameIT", JobName);
                        }
                        else
                        {
                            command3.Parameters.AddWithValue("@JobNameIT", DBNull.Value);


                        }
                        if (!string.IsNullOrEmpty(ProgramID))
                        {

                            command3.Parameters.AddWithValue("@ProgramId", ProgramID);
                        }
                        else
                        {
                            command3.Parameters.AddWithValue("@ProgramId", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(FileId))
                        {
                            command3.Parameters.AddWithValue("@FileId", FileId.Trim());
                        }
                        else
                        {
                            command3.Parameters.AddWithValue("@FileId", DBNull.Value);
                        }


                        if (!string.IsNullOrEmpty(TotalAcc))
                        {
                            command3.Parameters.AddWithValue("@AccQty", TotalAcc.Trim());
                        }
                        else
                        {
                            command3.Parameters.AddWithValue("@AccQty", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(TotalPage))
                        {

                            command3.Parameters.AddWithValue("@PageQty", TotalPage);
                        }
                        else
                        {
                            command3.Parameters.AddWithValue("@PageQty", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(TotalImpression))
                        {

                            command3.Parameters.AddWithValue("@ImpQty", TotalPage);
                        }
                        else
                        {
                            command3.Parameters.AddWithValue("@ImpQty", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(FirstRecord))
                        {
                            command3.Parameters.AddWithValue("@FirstRecord", FirstRecord);
                        }
                        else
                        {
                            command3.Parameters.AddWithValue("@FirstRecord", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(LastRecord))
                        {
                            command3.Parameters.AddWithValue("@LastRecord", LastRecord);
                        }
                        else
                        {
                            command3.Parameters.AddWithValue("@LastRecord", DBNull.Value);
                        }

                        command3.Parameters.AddWithValue("@Customer_Name", NamaCustomer);
                        command3.Parameters.AddWithValue("@ProductName", NamaProjek);
                        command3.ExecuteNonQuery();

                    }
                    else if (Temp == "MBBCC")
                    {

                        int ii = 0;
                        StreamReader rdr = new StreamReader(path2);
                        string line;
                        while ((line = rdr.ReadLine()) != null)
                        {
                            switch (ii)
                            {
                                case 4:
                                    string[] data4 = line.Split(':');
                                    NamaCustomer = data4[1].ToString().Trim();
                                    break;
                                case 5:
                                    string[] data5 = line.Split(':');
                                    ProgramName = data5[1].ToString().Trim();
                                    break;
                                case 6:
                                    string[] data6 = line.Split(':');
                                    FileId = data6[1].ToString().Trim();
                                    break;
                                case 7:
                                    string[] data7 = line.Split(':');
                                    StatementType = data7[1].ToString().Trim();
                                    break;
                                case 8:
                                    string[] data8 = line.Split(':');
                                    RunMode = data8[1].ToString().Trim();
                                    break;
                                case 9:
                                    string[] data9 = line.Split(':');
                                    DTProcess = data9[1].ToString().Trim();
                                    break;
                                case 10:
                                    string[] data10 = line.Split(':');
                                    TimeProcess = data10[1].ToString().Trim() + ":" + data10[2].ToString().Trim() + ":" + data10[3].ToString().Trim();
                                    break;
                                case 12:
                                    string[] data12 = line.Split(':');
                                    TotalPage = data12[1].ToString().Trim();
                                    break;
                                case 13:
                                    string[] data13 = line.Split(':');
                                    TotalImpression = data13[1].ToString().Trim();
                                    break;
                                case 14:
                                    string[] data14 = line.Split(':');
                                    TotalAcc = data14[1].ToString().Trim();
                                    break;

                            }
                            // add=1;
                            ii = ii + 1;

                        }


                        Guid guidIdx = Guid.NewGuid();

                        SqlCommand command3;
                        command3 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobAuditTrailDetail] SET Id=@Id,CreatedOn=@CreatedOn,JobId=@JobId,JobNameIT=@JobNameIT,ProgramId=@ProgramId,FileId=@FileId,AccQty=@AccQty,PageQty=@PageQty,ImpQty=@ImpQty,FirstRecord=@FirstRecord,LastRecord=@LastRecord,Customer_Name=@Customer_Name,ProductName=@ProductName WHERE Id=@Id", cn2);
                        command3.Parameters.AddWithValue("@Id", Id);

                        command3.Parameters.AddWithValue("@CreatedOn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        if (!string.IsNullOrEmpty(ProgramName))
                        {

                            command3.Parameters.AddWithValue("@JobId", ProgramName.Trim());

                        }
                        else
                        {
                            command3.Parameters.AddWithValue("@JobId", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(FileId))
                        {

                            command3.Parameters.AddWithValue("@JobNameIT", FileId);
                        }
                        else
                        {
                            command3.Parameters.AddWithValue("@JobNameIT", DBNull.Value);


                        }
                        if (!string.IsNullOrEmpty(RunMode))
                        {

                            command3.Parameters.AddWithValue("@ProgramId", RunMode);
                        }
                        else
                        {
                            command3.Parameters.AddWithValue("@ProgramId", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(FileId))
                        {
                            command3.Parameters.AddWithValue("@FileId", FileId.Trim());
                        }
                        else
                        {
                            command3.Parameters.AddWithValue("@FileId", DBNull.Value);
                        }


                        if (!string.IsNullOrEmpty(TotalAcc))
                        {
                            command3.Parameters.AddWithValue("@AccQty", TotalAcc.Trim());
                        }
                        else
                        {
                            command3.Parameters.AddWithValue("@AccQty", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(TotalPage))
                        {

                            command3.Parameters.AddWithValue("@PageQty", TotalPage);
                        }
                        else
                        {
                            command3.Parameters.AddWithValue("@PageQty", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(TotalImpression))
                        {

                            command3.Parameters.AddWithValue("@ImpQty", TotalImpression);
                        }
                        else
                        {
                            command3.Parameters.AddWithValue("@ImpQty", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(FirstRecord))
                        {
                            command3.Parameters.AddWithValue("@FirstRecord", FirstRecord);
                        }
                        else
                        {
                            command3.Parameters.AddWithValue("@FirstRecord", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(LastRecord))
                        {
                            command3.Parameters.AddWithValue("@LastRecord", LastRecord);
                        }
                        else
                        {
                            command3.Parameters.AddWithValue("@LastRecord", DBNull.Value);
                        }

                        command3.Parameters.AddWithValue("@Customer_Name", NamaCustomer);

                        if (!string.IsNullOrEmpty(StatementType))
                        {
                            command3.Parameters.AddWithValue("@ProductName", StatementType);
                        }
                        else
                        {
                            command3.Parameters.AddWithValue("@ProductName", DBNull.Value);
                        }
                        command3.ExecuteNonQuery();

                    }


                    else if (Temp == "BSN")
                    {

                        int ii = 0;
                        StreamReader rdr = new StreamReader(path2);
                        string line;
                        while ((line = rdr.ReadLine()) != null)
                        {
                            switch (ii)
                            {
                                case 4:
                                    string[] data4 = line.Split(':');
                                    ProgramID = data4[1].ToString().Trim();
                                    break;
                                case 5:
                                    string[] data5 = line.Split(':');
                                    PaperType = data5[1].ToString().Trim();
                                    break;
                                case 6:
                                    string[] data6 = line.Split(':');
                                    FileName = data6[1].ToString().Trim();
                                    break;
                                case 7:
                                    string[] data7 = line.Split(':');
                                    DataType = data7[1].ToString().Trim();
                                    break;
                                case 8:
                                    string[] data8 = line.Split(':');
                                    TotalAcc = data8[1].ToString().Trim();
                                    break;
                                case 9:
                                    string[] data9 = line.Split(':');
                                    TotalPage = data9[1].ToString().Trim();
                                    break;
                                case 10:
                                    string[] data10 = line.Split(':');
                                    TotalImpression = data10[1].ToString().Trim();
                                    break;
                                case 11:
                                    string[] data11 = line.Split(':');
                                    FirstRecord = data11[1].ToString().Trim();
                                    break;
                                case 12:
                                    string[] data12 = line.Split(':');
                                    LastRecord = data12[1].ToString().Trim();
                                    break;

                                case 14:
                                    string[] data14 = line.Split(':');
                                    NamaCustomer = data14[1].ToString();
                                    break;


                            }
                            // add=1;
                            ii = ii + 1;

                        }


                        Guid guidIdx = Guid.NewGuid();

                        SqlCommand command3;
                        command3 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobAuditTrailDetail] SET Id=@Id,CreatedOn=@CreatedOn,JobId=@JobId,JobNameIT=@JobNameIT,ProgramId=@ProgramId,FileId=@FileId,AccQty=@AccQty,PageQty=@PageQty,ImpQty=@ImpQty,FirstRecord=@FirstRecord,LastRecord=@LastRecord,Customer_Name=@Customer_Name,ProductName=@ProductName WHERE Id=@Id", cn2);
                        command3.Parameters.AddWithValue("@Id", Id);
                        command3.Parameters.AddWithValue("@CreatedOn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        if (!string.IsNullOrEmpty(JobId))
                        {

                            command3.Parameters.AddWithValue("@JobId", JobId.Trim());

                        }
                        else
                        {
                            command3.Parameters.AddWithValue("@JobId", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(FileId))
                        {

                            command3.Parameters.AddWithValue("@JobNameIT", FileId);
                        }
                        else
                        {
                            command3.Parameters.AddWithValue("@JobNameIT", DBNull.Value);


                        }
                        if (!string.IsNullOrEmpty(ProgramID))
                        {

                            command3.Parameters.AddWithValue("@ProgramId", ProgramID);
                        }
                        else
                        {
                            command3.Parameters.AddWithValue("@ProgramId", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(FileId))
                        {
                            command3.Parameters.AddWithValue("@FileId", FileId.Trim());
                        }
                        else
                        {
                            command3.Parameters.AddWithValue("@FileId", DBNull.Value);
                        }


                        if (!string.IsNullOrEmpty(TotalAcc))
                        {
                            command3.Parameters.AddWithValue("@AccQty", TotalAcc.Trim());
                        }
                        else
                        {
                            command3.Parameters.AddWithValue("@AccQty", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(TotalPage))
                        {

                            command3.Parameters.AddWithValue("@PageQty", TotalPage);
                        }
                        else
                        {
                            command3.Parameters.AddWithValue("@PageQty", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(TotalImpression))
                        {

                            command3.Parameters.AddWithValue("@ImpQty", TotalImpression);
                        }
                        else
                        {
                            command3.Parameters.AddWithValue("@ImpQty", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(FirstRecord))
                        {
                            command3.Parameters.AddWithValue("@FirstRecord", FirstRecord);
                        }
                        else
                        {
                            command3.Parameters.AddWithValue("@FirstRecord", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(LastRecord))
                        {
                            command3.Parameters.AddWithValue("@LastRecord", LastRecord);
                        }
                        else
                        {
                            command3.Parameters.AddWithValue("@LastRecord", DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(NamaCustomer))
                        {
                            command3.Parameters.AddWithValue("@Customer_Name", NamaCustomer);

                        }
                        else
                        {
                            command3.Parameters.AddWithValue("@Customer_Name", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(StatementType))
                        {
                            command3.Parameters.AddWithValue("@ProductName", StatementType);
                        }
                        else
                        {
                            command3.Parameters.AddWithValue("@ProductName", DBNull.Value);
                        }
                        command3.ExecuteNonQuery();

                    }




                    Guid guidId = Guid.NewGuid();
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");


                    SqlCommand command;
                    command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[SampleProductAudit] (Id,CreatedOn,Picture_FileId,AuditTrail,Picture_Extension,Code,CreateBy) values (@Id,@CreatedOn,@Picture_FileId,@AuditTrail,@Picture_Extension,@Code,@CreateBy)", cn2);
                    command.Parameters.AddWithValue("@Id", guidId);
                    command.Parameters.AddWithValue("@CreatedOn", createdOn);
                    command.Parameters.AddWithValue("@Picture_FileId", fileName.Trim());
                    command.Parameters.AddWithValue("@AuditTrail", Id);
                    command.Parameters.AddWithValue("@Picture_Extension", ModelSample.FileUploadFile3.ContentType);
                    command.Parameters.AddWithValue("@Code", "AT");
                    command.Parameters.AddWithValue("@CreateBy", IdentityName.ToString());
                    command.ExecuteNonQuery();


                }
                catch (System.Exception err)
                {
                    string strr = err.Message;

                }
                finally
                {
                    cn2.Close();
                }
            }


        }

        return View();

    }


    public ActionResult DataImport(string Id, string Customer_Name, string LogTagNo, string JobClass, string modeLog, HttpPostedFileBase file)
    {
        Debug.WriteLine("File : " + file);

        if (file != null && file.ContentLength > 0)
        {
            var fileName = System.IO.Path.GetFileName(file.FileName);
            var path = Path.Combine(Server.MapPath("~/FileStore"), fileName);
            file.SaveAs(path);
            Debug.WriteLine("File saved successfully");
            Debug.WriteLine("Beginning to read the file to extract data");

            int j = 1;

            if (modeLog == "Maybank Standard")
            {
                int[] Standard = new int[] { 4, 1, 2, 1, 1, 1, 1, 2, 1 };
                using (var fileUpload = new StreamReader(path))
                {
                    foreach (int lineNumber in Standard)
                    {
                        //Debug.WriteLine("Line Number :" + lineNumber);
                        for (int i = 1; i < lineNumber; i++)
                        {
                            //Debug.WriteLine("I Value :" + i);

                            if (fileUpload.ReadLine() == null) throw new ArgumentOutOfRangeException(nameof(lineNumber), "Line number exceeds total number of lines.");
                        }

                        if (j == 1)
                        {
                            string[] result = fileUpload.ReadLine().Split(new string[] { ": " }, StringSplitOptions.None);
                            JobId = result[1];

                        }
                        else if (j == 2)
                        {
                            string[] result = fileUpload.ReadLine().Split(new string[] { ": " }, StringSplitOptions.None);
                            JobName = result[1];

                        }
                        else if (j == 3)
                        {
                            string[] result = fileUpload.ReadLine().Split(new string[] { ": " }, StringSplitOptions.None);
                            ProgramID = result[1];
                        }
                        else if (j == 4)
                        {
                            string[] result = fileUpload.ReadLine().Split(new string[] { ": " }, StringSplitOptions.None);
                            FileName = result[1];

                        }
                        else if (j == 5)
                        {
                            string[] result = fileUpload.ReadLine().Split(new string[] { ": " }, StringSplitOptions.None);
                            string[] result2 = result[1].Split(new string[] { " " }, StringSplitOptions.None);
                            TotalAcc = result2[0].Replace(" ","");

                        }
                        else if (j == 6)
                        {
                            string[] result = fileUpload.ReadLine().Split(new string[] { ": " }, StringSplitOptions.None);
                            string[] result2 = result[1].Split(new string[] { " " }, StringSplitOptions.None);
                            TotalPage = result2[0].Replace(" ", "");

                        }
                        else if (j == 7)
                        {
                            string[] result = fileUpload.ReadLine().Split(new string[] { ": " }, StringSplitOptions.None);
                            string[] result2 = result[1].Split(new string[] { " " }, StringSplitOptions.None);
                            TotalImpression = result2[0].Replace(" ", "");

                        }
                        else if (j == 8)
                        {
                            string[] result = fileUpload.ReadLine().Split(new string[] { ": " }, StringSplitOptions.None);
                            FirstRecord = result[1];

                        }
                        else if (j == 9)
                        {
                            string[] result = fileUpload.ReadLine().Split(new string[] { ": " }, StringSplitOptions.None);
                            LastRecord = result[1];

                        }

                        j++;


                    }

                }

                Debug.WriteLine(JobId);
                Debug.WriteLine(JobName);
                Debug.WriteLine(ProgramID);
                Debug.WriteLine(ProgramName);
                Debug.WriteLine(TotalAcc);
                Debug.WriteLine(TotalPage);
                Debug.WriteLine(TotalImpression);
                Debug.WriteLine(FirstRecord);
                Debug.WriteLine(LastRecord);

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn.Open();

                    SqlCommand cmd1 = new SqlCommand("UPDATE JobAuditTrailDetail SET JobId=@JobId, JobNameIT=@JobName,ProgramId=@ProgramId,FileId=@FileId, AccQty=@AccQty, ImpQty=@ImpQty,PageQty=@PageQty, FirstRecord=@FirstRecord, LastRecord=@LastRecord WHERE Id=@Id", cn);

                    if (!string.IsNullOrEmpty(JobId))
                    {
                        cmd1.Parameters.AddWithValue("@JobId", JobId);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@JobId", "");

                    }

                    if (!string.IsNullOrEmpty(JobName))
                    {
                        cmd1.Parameters.AddWithValue("@JobName", JobName);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@JobName", "");
                    }

                    if (!string.IsNullOrEmpty(ProgramID))
                    {
                        cmd1.Parameters.AddWithValue("@ProgramId", ProgramID);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@ProgramId", "");
                    }

                    if (!string.IsNullOrEmpty(FileName))
                    {
                        cmd1.Parameters.AddWithValue("@FileId", FileName);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@FileId", "");
                    }

                    if (!string.IsNullOrEmpty(TotalAcc))
                    {
                        cmd1.Parameters.AddWithValue("@AccQty", TotalAcc);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@AccQty", "");
                    }

                    if (!string.IsNullOrEmpty(TotalImpression))
                    {
                        cmd1.Parameters.AddWithValue("@ImpQty", TotalImpression);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@ImpQty", "");
                    }

                    if (!string.IsNullOrEmpty(TotalPage))
                    {
                        cmd1.Parameters.AddWithValue("@PageQty", TotalPage);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@PageQty", "");
                    }

                    if (!string.IsNullOrEmpty(FirstRecord))
                    {
                        cmd1.Parameters.AddWithValue("@FirstRecord", FirstRecord);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@FirstRecord", "");
                    }

                    if (!string.IsNullOrEmpty(LastRecord))
                    {
                        cmd1.Parameters.AddWithValue("@LastRecord", LastRecord);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@LastRecord", "");
                    }

                    cmd1.Parameters.AddWithValue("@Id", Id);

                    cmd1.ExecuteNonQuery();

                    cn.Close();
                }


            }
            else if (modeLog == "MBCC")
            {
                int[] Standard = new int[] { 3, 3, 1, 6, 1, 1 };
                using (var fileUpload = new StreamReader(path))
                {
                    foreach (int lineNumber in Standard)
                    {
                        //Debug.WriteLine("Line Number :" + lineNumber);
                        for (int i = 1; i < lineNumber; i++)
                        {
                            //Debug.WriteLine("I Value :" + i);

                            if (fileUpload.ReadLine() == null) throw new ArgumentOutOfRangeException(nameof(lineNumber), "Line number exceeds total number of lines.");
                        }

                        if (j == 1)
                        {
                            //string[] result = fileUpload.ReadLine().Split(new string[] { ": " }, StringSplitOptions.None);
                            JobName = fileUpload.ReadLine();

                        }
                        else if (j == 2)
                        {
                            string[] result = fileUpload.ReadLine().Split(new string[] { "Program  Name             : " }, StringSplitOptions.None);
                            ProgramID = result[1];

                        }
                        else if (j == 3)
                        {
                            string[] result = fileUpload.ReadLine().Split(new string[] { "File ID                   : " }, StringSplitOptions.None);
                            FileId = result[1];
                        }
                        else if (j == 4)
                        {
                            string[] result = fileUpload.ReadLine().Split(new string[] { "Total Number of Pages     :" }, StringSplitOptions.None);
                            TotalPage = result[1].Replace(" ","");

                        }
                        else if (j == 5)
                        {
                            string[] result = fileUpload.ReadLine().Split(new string[] { "Total Number of Impression:" }, StringSplitOptions.None);
                            TotalImpression = result[1].Replace(" ","");

                        }
                        else if (j == 6)
                        {
                            string[] result = fileUpload.ReadLine().Split(new string[] { "Total Number of Accounts  :" }, StringSplitOptions.None);
                            TotalAcc = result[1].Replace(" ","");

                        }
                        //else if (j == 7)
                        //{
                        //    string[] result = fileUpload.ReadLine().Split(new string[] { ": " }, StringSplitOptions.None);
                        //    TotalImpression = result[1];

                        //}
                        //else if (j == 8)
                        //{
                        //    string[] result = fileUpload.ReadLine().Split(new string[] { ": " }, StringSplitOptions.None);
                        //    FirstRecord = result[1];

                        //}
                        //else if (j == 9)
                        //{
                        //    string[] result = fileUpload.ReadLine().Split(new string[] { ": " }, StringSplitOptions.None);
                        //    LastRecord = result[1];

                        //}

                        j++;


                    }

                }

                Debug.WriteLine(JobId);
                Debug.WriteLine(JobName);
                Debug.WriteLine(ProgramID);
                Debug.WriteLine(ProgramName);
                Debug.WriteLine(TotalAcc);
                Debug.WriteLine(TotalPage);
                Debug.WriteLine(TotalImpression);
                Debug.WriteLine(FirstRecord);
                Debug.WriteLine(LastRecord);

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn.Open();

                    SqlCommand cmd1 = new SqlCommand("UPDATE JobAuditTrailDetail SET JobNameIT=@JobName,ProgramId=@ProgramId,FileId=@FileId, AccQty=@AccQty, ImpQty=@ImpQty,PageQty=@PageQty WHERE Id=@Id", cn);

                    if (!string.IsNullOrEmpty(JobName))
                    {
                        cmd1.Parameters.AddWithValue("@JobName", JobName);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@JobName", "");
                    }

                    if (!string.IsNullOrEmpty(ProgramID))
                    {
                        cmd1.Parameters.AddWithValue("@ProgramId", ProgramID);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@ProgramId", "");
                    }

                    if (!string.IsNullOrEmpty(FileId))
                    {
                        cmd1.Parameters.AddWithValue("@FileId", FileId);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@FileId", "");
                    }

                    if (!string.IsNullOrEmpty(TotalAcc))
                    {
                        cmd1.Parameters.AddWithValue("@AccQty", TotalAcc);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@AccQty", "");
                    }

                    if (!string.IsNullOrEmpty(TotalImpression))
                    {
                        cmd1.Parameters.AddWithValue("@ImpQty", TotalImpression);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@ImpQty", "");
                    }

                    if (!string.IsNullOrEmpty(TotalPage))
                    {
                        cmd1.Parameters.AddWithValue("@PageQty", TotalPage);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@PageQty", "");
                    }

                    //if (!string.IsNullOrEmpty(FirstRecord))
                    //{
                    //    cmd1.Parameters.AddWithValue("@FirstRecord", FirstRecord);
                    //}
                    //else
                    //{
                    //    cmd1.Parameters.AddWithValue("@FirstRecord", "");
                    //}

                    //if (!string.IsNullOrEmpty(LastRecord))
                    //{
                    //    cmd1.Parameters.AddWithValue("@LastRecord", LastRecord);
                    //}
                    //else
                    //{
                    //    cmd1.Parameters.AddWithValue("@LastRecord", "");
                    //}

                    cmd1.Parameters.AddWithValue("@Id", Id);

                    cmd1.ExecuteNonQuery();

                    cn.Close();
                }
            }
            else if (modeLog == "Bank Rakyat")
            {
                string result1 = "";
                int[] Standard = new int[] { 5, 1, 2, 1, 1, 2, 1 };
                using (var fileUpload = new StreamReader(path))
                {
                    foreach (int lineNumber in Standard)
                    {
                        //Debug.WriteLine("Line Number :" + lineNumber);
                        for (int i = 1; i < lineNumber; i++)
                        {
                            //Debug.WriteLine("I Value :" + i);

                            if (fileUpload.ReadLine() == null) throw new ArgumentOutOfRangeException(nameof(lineNumber), "Line number exceeds total number of lines.");
                        }

                        //result1 = fileUpload.ReadLine();
                        //Debug.WriteLine(result1);

                        if (j == 1)
                        {
                            string[] result = fileUpload.ReadLine().Split(new string[] { "Program ID           : " }, StringSplitOptions.None);
                            ProgramID = result[1];

                        }
                        else if (j == 2)
                        {
                            string[] result = fileUpload.ReadLine().Split(new string[] { "File Name (PDF)      : " }, StringSplitOptions.None);
                            foreach (var x in result)
                            {
                                Debug.WriteLine("Result : " + x);
                            }
                            FileId = result[1];

                        }
                        else if (j == 3)
                        {
                            string[] result = fileUpload.ReadLine().Split(new string[] { "Total Accounts       : " }, StringSplitOptions.None);
                            TotalAcc = result[1].Replace(" ","");
                        }
                        else if (j == 4)
                        {
                            string[] result = fileUpload.ReadLine().Split(new string[] { " : " }, StringSplitOptions.None);
                            TotalPage = result[1].Replace(" ", "");

                        }
                        else if (j == 5)
                        {
                            string[] result = fileUpload.ReadLine().Split(new string[] { " : " }, StringSplitOptions.None);
                            TotalImpression = result[1].Replace(" ", "");

                        }
                        else if (j == 6)
                        {
                            string[] result = fileUpload.ReadLine().Split(new string[] { "First Record         : " }, StringSplitOptions.None);
                            FirstRecord = result[1];

                        }
                        else if (j == 7)
                        {
                            string[] result = fileUpload.ReadLine().Split(new string[] { " : " }, StringSplitOptions.None);
                            LastRecord = result[1];

                        }
                        //else if (j == 8)
                        //{


                        //}
                        //else if (j == 9)
                        //{
                        //    string[] result = fileUpload.ReadLine().Split(new string[] { ": " }, StringSplitOptions.None);
                        //    LastRecord = result[1];

                        //}

                        j++;


                    }

                }

                //Debug.WriteLine(JobId);
                //Debug.WriteLine(JobName);
                Debug.WriteLine(ProgramID);
                Debug.WriteLine(FileId);
                Debug.WriteLine(TotalAcc);
                Debug.WriteLine(TotalPage);
                Debug.WriteLine(TotalImpression);
                Debug.WriteLine(FirstRecord);
                Debug.WriteLine(LastRecord);

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn.Open();

                    SqlCommand cmd1 = new SqlCommand("UPDATE JobAuditTrailDetail SET ProgramId=@ProgramId,FileId=@FileId, AccQty=@AccQty, ImpQty=@ImpQty,PageQty=@PageQty, FirstRecord=@FirstRecord, LastRecord=@LastRecord WHERE Id=@Id", cn);

                    //if (!string.IsNullOrEmpty(JobId))
                    //{
                    //    cmd1.Parameters.AddWithValue("@JobId", JobId);
                    //}
                    //else
                    //{
                    //    cmd1.Parameters.AddWithValue("@JobId", "");

                    //}

                    //if (!string.IsNullOrEmpty(JobName))
                    //{
                    //    cmd1.Parameters.AddWithValue("@JobName", JobName);
                    //}
                    //else
                    //{
                    //    cmd1.Parameters.AddWithValue("@JobName", "");
                    //}

                    if (!string.IsNullOrEmpty(ProgramID))
                    {
                        cmd1.Parameters.AddWithValue("@ProgramId", ProgramID);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@ProgramId", "");
                    }

                    if (!string.IsNullOrEmpty(FileId))
                    {
                        cmd1.Parameters.AddWithValue("@FileId", FileId);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@FileId", "");
                    }

                    if (!string.IsNullOrEmpty(TotalAcc))
                    {
                        cmd1.Parameters.AddWithValue("@AccQty", TotalAcc);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@AccQty", "");
                    }

                    if (!string.IsNullOrEmpty(TotalImpression))
                    {
                        cmd1.Parameters.AddWithValue("@ImpQty", TotalImpression);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@ImpQty", "");
                    }

                    if (!string.IsNullOrEmpty(TotalPage))
                    {
                        cmd1.Parameters.AddWithValue("@PageQty", TotalPage);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@PageQty", "");
                    }

                    if (!string.IsNullOrEmpty(FirstRecord))
                    {
                        cmd1.Parameters.AddWithValue("@FirstRecord", FirstRecord);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@FirstRecord", "");
                    }

                    if (!string.IsNullOrEmpty(LastRecord))
                    {
                        cmd1.Parameters.AddWithValue("@LastRecord", LastRecord);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@LastRecord", "");
                    }

                    cmd1.Parameters.AddWithValue("@Id", Id);

                    cmd1.ExecuteNonQuery();

                    cn.Close();
                }
            }


        }

        return RedirectToAction("AddJAT", "ITO", new { Id = Id, Customer_Name = Customer_Name, LogTagNo = LogTagNo, JobClass = JobClass });
    }

    public ActionResult DeleteMedia(string Id, string AuditTrail, string JatId, string CustomerName, string LogTagNo, string JobClass)
    {
        Guid SampleProductId = Guid.Empty;

        if (Id != null)
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Picture_FileId,AuditTrail
                                          FROM [IflowSeed].[dbo].[SampleProductAudit]
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
                            command3 = new SqlCommand("DELETE [IflowSeed].[dbo].[SampleProductAudit] WHERE Id=@Id", cn3);
                            command3.Parameters.AddWithValue("@Id", Id);
                            command3.ExecuteNonQuery();
                            cn3.Close();

                        }

                    }

                    if (reader.IsDBNull(1) == false)
                    {
                        SampleProductId = reader.GetGuid(1);
                        //return RedirectToAction("ManageJAT", "ITO");
                        return RedirectToAction("AddJAT", "ITO", new { Id = JatId, CustomerName = CustomerName, LogTagNo = LogTagNo, JobClass = JobClass });
                        //return RedirectToAction("CreateJobAuditTrail", "ITO", new { Id = Session["Id"].ToString() });
                    }
                }
                cn.Close();
            }
        }

        return RedirectToAction("CreateJobAuditTrail", "ITO", new { Id = Session["Id"].ToString() });
    }

    public ActionResult DownloadMedia(string Id, string JatId, string CustomerName, string LogTagNo, string JobClass)
    {
        Guid SampleProductId = Guid.Empty;

        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT Picture_FileId,Picture_Extension,Id
                                      FROM [IflowSeed].[dbo].[SampleProductAudit]
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

        //return RedirectToAction("CreateJobAuditTrail", "ITO", new { Id = Id.ToString() });
        //return RedirectToAction("ManageJAT", "ITO");
        return RedirectToAction("AddJAT", "ITO", new { Id = JatId, CustomerName = CustomerName, LogTagNo = LogTagNo, JobClass = JobClass });

    }

    public ActionResult ViewJAT(string Id, string set, string JobInstructionId, string tabs, string Customer_Name, string ProductName, string JobSheetNo,
                                string SalesExecutiveBy, string Status,
                                string ServiceLevel, string IsSlaCreaditCard, string JobClass, string IsSetPaper,
                                string JobRequest, string ExpectedDateCompletionToGpo, string QuotationRef, string ContractName,
                                string ContactPerson, string JobType, string DeliveryChannel, string AccountsQty, string ImpressionQty,
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
                                string ArtworkNotes, string Acc_BillingNotes, string DCPNotes, string PostingInfo, JobInstruction get,
                                string ImageInDateOn, string ImageInTime, string RevisedInDateOn)
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
            command.CommandText = @"SELECT Id,Customer_Name,ProductName,LogTagNo,
                                    AccountsQty,ImpressionQty,PagesQty, JobType
                                    FROM [IflowSeed].[dbo].[JobAuditTrail]
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
                    ViewBag.LogTagNo = reader.GetString(3);
                }
                if (reader.IsDBNull(4) == false)
                {
                    ViewBag.AccountsQty = reader.GetString(4);
                }
                if (reader.IsDBNull(5) == false)
                {
                    ViewBag.ImpressionQty = reader.GetString(5);
                }
                if (reader.IsDBNull(6) == false)
                {
                    ViewBag.PagesQty = reader.GetString(6);
                }
                if (reader.IsDBNull(7) == false)
                {
                    ViewBag.JobType = reader.GetString(7);
                }


            }
            cn.Close();
        }

        //call table

        List<JobAuditTrail> viewJATList = new List<JobAuditTrail>();
        using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn2))
        {
            int _bil = 1;
            cn2.Open();
            command.CommandText = @"SELECT Id,Customer_Name,ProductName,LogTagNo,
                                           AccountsQty,ImpressionQty,PagesQty, JobType
                                           FROM [dbo].[JobAuditTrailDetail]
                                           WHERE Id=@Id";
            command.Parameters.AddWithValue("@Id", Session["Id"].ToString());
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobAuditTrail model = new JobAuditTrail();
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
                        model.LogTagNo = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        model.AccountsQty = reader.GetString(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        model.ImpressionQty = reader.GetString(5);
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        model.PagesQty = reader.GetString(6);
                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        model.JobType = reader.GetString(7);
                    }

                }
                viewJATList.Add(model);
            }
            cn2.Close();

        }



        //display data from table 
        List<JobAuditTrailDetail> viewJATList1 = new List<JobAuditTrailDetail>();
        using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn3))
        {
            cn3.Open();
            command.CommandText = @"SELECT JobAuditTrailDetail.ModeLog, JobAuditTrailDetail.Path, JobAuditTrailDetail.JobNameIT, JobAuditTrailDetail.JobId, JobAuditTrailDetail.ProgramId, JobAuditTrailDetail.FileId, JobAuditTrailDetail.RevStrtDateOn, JobAuditTrailDetail.RevStrtTime, JobAuditTrailDetail.DateProcessItOn, JobAuditTrailDetail.TimeProcessIt, JobAuditTrailDetail.DateApproveOn, JobAuditTrailDetail.AccQty, JobAuditTrailDetail.ImpQty, JobAuditTrailDetail.PageQty, JobAuditTrailDetail.FirstRecord,
                                      JobAuditTrailDetail.LastRecord, JobAuditTrailDetail.ImageInDateOn, JobAuditTrailDetail.ImageInTime, 
                                    JobAuditTrailDetail.RevisedInDateOn, JobAuditTrailDetail.Customer_Name,JobAuditTrailDetail.DateApproveTime
                                    FROM  JobInstruction INNER JOIN
                                     JobAuditTrailDetail ON JobInstruction.Id = JobAuditTrailDetail.JobInstructionId
                                    WHERE JobInstruction.Id=@JobAuditTrailId";
            command.Parameters.AddWithValue("@JobAuditTrailId", Id);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader.IsDBNull(0) == false)
                {
                    ViewBag.ModeLog = reader.GetString(0);
                }
                if (reader.IsDBNull(1) == false)
                {
                    ViewBag.Path = reader.GetString(1);
                }
                if (reader.IsDBNull(2) == false)
                {
                    ViewBag.JobNameIT = reader.GetString(2);
                }
                if (reader.IsDBNull(3) == false)
                {
                    ViewBag.JobId = reader.GetString(3);
                }
                if (reader.IsDBNull(4) == false)
                {
                    ViewBag.ProgramId = reader.GetString(4);
                }
                if (reader.IsDBNull(5) == false)
                {
                    ViewBag.FileId = reader.GetString(5);
                }
                if (reader.IsDBNull(6) == false)
                {
                    ViewBag.RevStrtDateOn = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(6));
                }
                if (reader.IsDBNull(7) == false)
                {
                    ViewBag.RevStrtTime = reader.GetString(7);
                }
                if (reader.IsDBNull(8) == false)
                {
                    ViewBag.DateProcessItOn = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(8));
                }
                if (reader.IsDBNull(9) == false)
                {
                    ViewBag.TimeProcessIt = reader.GetString(9);
                }
                if (reader.IsDBNull(10) == false)
                {
                    ViewBag.DateApproveOn = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(10));
                }

                if (reader.IsDBNull(11) == false)
                {
                    ViewBag.AccQty = reader.GetString(11);
                }
                if (reader.IsDBNull(12) == false)
                {
                    ViewBag.ImpQty = reader.GetString(12);
                }
                if (reader.IsDBNull(13) == false)
                {
                    ViewBag.PageQty = reader.GetString(13);
                }
                if (reader.IsDBNull(14) == false)
                {
                    ViewBag.FirstRecord = reader.GetString(14);
                }
                if (reader.IsDBNull(15) == false)
                {
                    ViewBag.LastRecord = reader.GetString(15);
                }

                if (reader.IsDBNull(16) == false)
                {
                    ViewBag.ImageInDateOn = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(16));
                }
                if (reader.IsDBNull(17) == false)
                {
                    ViewBag.ImageInTime = reader.GetString(17);
                }
                if (reader.IsDBNull(18) == false)
                {
                    ViewBag.RevisedInDateOn = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(18));
                }
                if (reader.IsDBNull(19) == false)
                {
                    ViewBag.Customer_Name = reader.GetString(19);
                }
                if (reader.IsDBNull(20) == false)
                {
                    ViewBag.DateApproveTime = reader.GetString(20);
                }

            }
            cn3.Close();
        }

        //call table



        //-----------------------------------------

        ReloadJATList(Id);

        //ReloadJAT(Id);

        return new Rotativa.ViewAsPdf("ViewJAT", viewJATList1)
        {
            // FileName = flightPlan.ListingItemDetailsModel.FlightDetails + ".pdf",
            PageMargins = new Rotativa.Options.Margins(5, 5, 5, 5),
            PageOrientation = Rotativa.Options.Orientation.Portrait,
            //PageWidth = 210,
            //PageHeight = 297
        };
    }

    List<JobAuditTrail> viewJATList = new List<JobAuditTrail>();
    private int _bil;
    private JobAuditTrailDetail model;

    private void ReloadJATList(string Id)
    {
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {

            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT b.Id,b.Customer_Name, b.ProductName, b.LogTagNo,
                                           b.AccountsQty,b.ImpressionQty,b.PagesQty,
                                           a.ModeLog,a.Path,a.JobNameIT,a.JobId,a.ProgramId,a.FileId,
                                           a.RevStrtDateOn,a.RevStrtTime,a.DateProcessItOn,a.TimeProcessIt,
                                           a.DateApproveOn,a.DateApproveTime,a.AccQty,a.ImpQty,a.PageQty,a.FirstRecord,
                                           a.LastRecord,a.JobAuditTrailId,a.ImageInDateOn,a.ImageInTime,a.RevisedInDateOn,b.JobType
                                           FROM [dbo].[JobAuditTrailDetail]a, [dbo].[JobAuditTrail]b
                                           WHERE a.JobAuditTrailId=b.Id AND b.Id=@Id ";
            command.Parameters.AddWithValue("@Id", Id);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobAuditTrail model = new JobAuditTrail();
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
                        model.LogTagNo = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        model.AccountsQty = reader.GetString(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        model.ImpressionQty = reader.GetString(5);
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        model.PagesQty = reader.GetString(6);
                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        model.ModeLog = reader.GetString(7);
                    }
                    if (reader.IsDBNull(8) == false)
                    {
                        model.Path = reader.GetString(8);
                    }
                    if (reader.IsDBNull(9) == false)
                    {
                        model.JobNameIT = reader.GetString(9);
                    }
                    if (reader.IsDBNull(10) == false)
                    {
                        model.JobId = reader.GetString(10);
                    }
                    if (reader.IsDBNull(11) == false)
                    {
                        model.ProgramId = reader.GetString(11);
                    }
                    if (reader.IsDBNull(12) == false)
                    {
                        model.FileId = reader.GetString(12);
                    }
                    if (reader.IsDBNull(13) == false)
                    {
                        model.RevStrtDateOn = reader.GetDateTime(13);
                    }
                    if (reader.IsDBNull(14) == false)
                    {
                        model.RevStrtTime = reader.GetString(14);
                    }
                    if (reader.IsDBNull(15) == false)
                    {
                        model.DateProcessItOn = reader.GetDateTime(15);
                    }
                    if (reader.IsDBNull(16) == false)
                    {
                        model.TimeProcessIt = reader.GetString(16);
                    }
                    if (reader.IsDBNull(17) == false)
                    {
                        model.DateApproveOn = reader.GetDateTime(17);
                    }
                    if (reader.IsDBNull(18) == false)
                    {
                        model.DateApproveTime = reader.GetString(18);
                    }
                    if (reader.IsDBNull(19) == false)
                    {
                        model.AccQty = reader.GetString(19);
                    }
                    if (reader.IsDBNull(20) == false)
                    {
                        model.ImpQty = reader.GetString(20);
                    }
                    if (reader.IsDBNull(21) == false)
                    {
                        model.PageQty = reader.GetString(21);
                    }
                    if (reader.IsDBNull(22) == false)
                    {
                        model.FirstRecord = reader.GetString(22);
                    }
                    if (reader.IsDBNull(23) == false)
                    {
                        model.LastRecord = reader.GetString(23);
                    }
                    if (reader.IsDBNull(24) == false)
                    {
                        model.JobAuditTrailId = reader.GetGuid(24);
                    }
                    if (reader.IsDBNull(25) == false)
                    {
                        model.ImageInDateOn = reader.GetDateTime(25);
                    }
                    if (reader.IsDBNull(26) == false)
                    {
                        model.ImageInTime = reader.GetString(26);
                    }
                    if (reader.IsDBNull(27) == false)
                    {
                        model.RevisedInDateOn = reader.GetDateTime(27);
                    }
                    if (reader.IsDBNull(28) == false)
                    {
                        model.JobType = reader.GetString(28);
                    }
                }


                viewJATList.Add(model);
            }
            cn.Close();
        }
    }

    List<DailyTracking> viewDailyJob = new List<DailyTracking>();
    public ActionResult ManageDailyJobITO()
    {
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.IsRole = @Session["Role"];
        var IdentityName = @Session["Fullname"];

        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT Id, DateProcessItOn, Customer_Name, Cust_Department, ProductName,
                                    CreateByIT, AccQty, ImpQty, PageQty, JobSheetNo, JobClass, Frequency
                                    FROM [dbo].[JobAuditTrailDetail]
                                    ORDER BY DateProcessItOn Desc";

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                DailyTracking model = new DailyTracking();
                {
                    model.Bil = _bil++;
                    if (reader.IsDBNull(0) == false)
                    {
                        model.Id = reader.GetGuid(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        model.DateProcessItOnTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(1));
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
                        model.ProductName = reader.GetString(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        model.CreateByIT = reader.GetString(5);
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
                        model.JobSheetNo = reader.GetString(9);
                    }
                    if (reader.IsDBNull(10) == false)
                    {
                        model.JobClass = reader.GetString(10);
                    }
                    if (reader.IsDBNull(11) == false)
                    {
                        model.Frequency = reader.GetString(11);
                    }
                }
                viewDailyJob.Add(model);
            }
            cn.Close();
        }
        return View(viewDailyJob);

    }



    public ActionResult CreateDailyTracking(DailyTracking get, string set, string Id, string TimeTaken, string JobSheetNo, string LogTagNo,
        string JobClass, string JobType, string Customer_Name, string Cust_Department, string ProductName, string CreateByIT, string AccountsQty,
        string ImpressionQty, string PagesQty)
    {
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT Id, JobSheetNo, LogTagNo, JobClass, JobType, Customer_Name, ProductName,
                                    CreateByIT, AccQty, ImpQty, PageQty, Cust_Department
                                    FROM [dbo].[JobAuditTrailDetail] 
                                    WHERE Id=@Id";

            command.Parameters.AddWithValue("@Id", Id);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                DailyTracking model = new DailyTracking();
                {
                    model.Bil = _bil++;
                    if (reader.IsDBNull(0) == false)
                    {
                        model.Id = reader.GetGuid(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        model.JobSheetNo = reader.GetString(1);
                        ViewBag.JobSheetNo = model.JobSheetNo;
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        model.LogTagNo = reader.GetString(2);
                        ViewBag.LogTagNo = model.LogTagNo;
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        model.JobClass = reader.GetString(3);
                        ViewBag.JobClass = model.JobClass;
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        model.JobType = reader.GetString(4);
                        ViewBag.JobType = model.JobType;
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        model.Customer_Name = reader.GetString(5);
                        ViewBag.Customer_Name = model.Customer_Name;
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        model.ProductName = reader.GetString(6);
                        ViewBag.ProductName = model.ProductName;
                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        model.CreateByIT = reader.GetString(7);
                        ViewBag.CreateByIT = model.CreateByIT;
                    }
                    if (reader.IsDBNull(8) == false)
                    {
                        model.AccQty = reader.GetString(8);
                        ViewBag.AccQty = model.AccQty;
                    }
                    if (reader.IsDBNull(9) == false)
                    {
                        model.ImpQty = reader.GetString(9);
                        ViewBag.ImpQty = model.ImpQty;
                    }
                    if (reader.IsDBNull(10) == false)
                    {
                        model.PageQty = reader.GetString(10);
                        ViewBag.PageQty = model.PageQty;
                    }
                    if (reader.IsDBNull(11) == false)
                    {
                        model.Cust_Department = reader.GetString(11);
                        ViewBag.Cust_Department = model.Cust_Department;
                    }
                }
                viewDailyJob.Add(model);
            }
            cn.Close();
        }

        if (set == "UpdateDailyTracking")
        {
            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command2 = new SqlCommand("", cn2))
            {
                string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                string ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                Guid newId = Guid.NewGuid();

                cn2.Open();

                command2.CommandText =
                    @"IF NOT EXISTS 
                         ( SELECT  1 FROM [dbo].[DailyTracking] 
                         WHERE JobAuditTrail = @JobAuditTrail )
                         BEGIN
                            INSERT INTO [dbo].[DailyTracking] 
                                (Id,StartDateOn,StartTime,EndDateOn,EndTime,ProcessStartDateOn,ProcessStartTime,ProcessEndDateOn,ProcessEndTime,TimeTaken,
                                DateApproveOn,DateApproveTime,LogTagSendOn,LogTagSendTime,CreatedOn, JobSheetNo, LogTagNo, JobClass, JobType, Customer_Name, Cust_Department, ProductName, PIC, AccountsQty,ImpressionQty, PagesQty, Status, JobAuditTrail) 
                                VALUES (@Id,@StartDateOn,@StartTime,@EndDateOn,@EndTime,@ProcessStartDateOn,@ProcessStartTime,@ProcessEndDateOn,@ProcessEndTime,@TimeTaken,@DateApproveOn,@DateApproveTime,@LogTagSendOn,@LogTagSendTime,@CreatedOn, @JobSheetNo, 
                                @LogTagNo, @JobClass, @JobType, @Customer_Name, @Cust_Department, @ProductName, @PIC, @AccountsQty, @ImpressionQty, @PagesQty, @Status, @JobAuditTrail) 
                         END
                     ELSE 
                        BEGIN 
                            UPDATE [dbo].[DailyTracking] SET
                            StartDateOn=@StartDateOn, StartTime=@StartTime, EndDateOn=@EndDateOn, EndTime=@EndTime, ProcessStartDateOn=@ProcessStartDateOn ,ProcessStartTime=@ProcessStartTime ,ProcessEndDateOn=@ProcessEndDateOn ,ProcessEndTime=@ProcessEndTime ,TimeTaken=@TimeTaken,
                            DateApproveOn=@DateApproveOn, DateApproveTime=@DateApproveTime, LogTagSendOn=@LogTagSendOn ,LogTagSendTime=@LogTagSendTime, ModifiedOn=@ModifiedOn
                        END";

                command2.Parameters.AddWithValue("@Id", newId);

                if (get.StartDateOn == null)
                {
                    command2.Parameters.Add(new SqlParameter { ParameterName = "@StartDateOn", Value = DBNull.Value });
                }
                else
                {
                    command2.Parameters.AddWithValue("@StartDateOn", get.StartDateOn);
                }

                if (get.StartTime == null)
                {
                    command2.Parameters.Add(new SqlParameter { ParameterName = "@StartTime", Value = DBNull.Value });
                }
                else
                {
                    command2.Parameters.AddWithValue("@StartTime", get.StartTime);
                }

                if (get.EndDateOn == null)
                {
                    command2.Parameters.Add(new SqlParameter { ParameterName = "@EndDateOn", Value = DBNull.Value });
                }
                else
                {
                    command2.Parameters.AddWithValue("@EndDateOn", get.EndDateOn);
                }

                if (get.EndTime == null)
                {
                    command2.Parameters.Add(new SqlParameter { ParameterName = "@EndTime", Value = DBNull.Value });
                }
                else
                {
                    command2.Parameters.AddWithValue("@EndTime", get.EndTime);
                }

                if (get.ProcessStartDateOn == null)
                {
                    command2.Parameters.Add(new SqlParameter { ParameterName = "@ProcessStartDateOn", Value = DBNull.Value });
                }
                else
                {
                    command2.Parameters.AddWithValue("@ProcessStartDateOn", get.ProcessStartDateOn);
                }

                if (get.ProcessStartTime == null)
                {
                    command2.Parameters.Add(new SqlParameter { ParameterName = "@ProcessStartTime", Value = DBNull.Value });
                }
                else
                {
                    command2.Parameters.AddWithValue("@ProcessStartTime", get.ProcessStartTime);
                }

                if (get.ProcessEndDateOn == null)
                {
                    command2.Parameters.Add(new SqlParameter { ParameterName = "@ProcessEndDateOn", Value = DBNull.Value });
                }
                else
                {
                    command2.Parameters.AddWithValue("@ProcessEndDateOn", get.ProcessEndDateOn);
                }

                if (get.ProcessEndTime == null)
                {
                    command2.Parameters.Add(new SqlParameter { ParameterName = "@ProcessEndTime", Value = DBNull.Value });
                }
                else
                {
                    command2.Parameters.AddWithValue("@ProcessEndTime", get.ProcessEndTime);
                }

                if (TimeTaken == null)
                {
                    command2.Parameters.Add(new SqlParameter { ParameterName = "@TimeTaken", Value = DBNull.Value });
                }
                else
                {
                    command2.Parameters.AddWithValue("@TimeTaken", TimeTaken);
                }

                if (get.DateApproveOn == null)
                {
                    command2.Parameters.Add(new SqlParameter { ParameterName = "@DateApproveOn", Value = DBNull.Value });
                }
                else
                {
                    command2.Parameters.AddWithValue("@DateApproveOn", get.DateApproveOn);
                }

                if (get.DateApproveTime == null)
                {
                    command2.Parameters.Add(new SqlParameter { ParameterName = "@DateApproveTime", Value = DBNull.Value });
                }
                else
                {
                    command2.Parameters.AddWithValue("@DateApproveTime", get.DateApproveTime);
                }

                if (get.LogTagSendOn == null)
                {
                    command2.Parameters.Add(new SqlParameter { ParameterName = "@LogTagSendOn", Value = DBNull.Value });
                }
                else
                {
                    command2.Parameters.AddWithValue("@LogTagSendOn", get.LogTagSendOn);
                }

                if (get.LogTagSendTime == null)
                {
                    command2.Parameters.Add(new SqlParameter { ParameterName = "@LogTagSendTime", Value = DBNull.Value });
                }
                else
                {
                    command2.Parameters.AddWithValue("@LogTagSendTime", get.LogTagSendTime);
                }
                command2.Parameters.AddWithValue("@CreatedOn", createdOn);
                command2.Parameters.AddWithValue("@ModifiedOn", ModifiedOn);

                if (JobSheetNo == null)
                {
                    command2.Parameters.Add(new SqlParameter { ParameterName = "@JobSheetNo", Value = DBNull.Value });
                }
                else
                {
                    command2.Parameters.AddWithValue("@JobSheetNo", JobSheetNo);
                }

                if (LogTagNo == null)
                {
                    command2.Parameters.Add(new SqlParameter { ParameterName = "@LogTagNo", Value = DBNull.Value });
                }
                else
                {
                    command2.Parameters.AddWithValue("@LogTagNo", LogTagNo);
                }

                if (JobClass == null)
                {
                    command2.Parameters.Add(new SqlParameter { ParameterName = "@JobClass", Value = DBNull.Value });
                }
                else
                {
                    command2.Parameters.AddWithValue("@JobClass", JobClass);
                }

                if (JobType == null)
                {
                    command2.Parameters.Add(new SqlParameter { ParameterName = "@JobType", Value = DBNull.Value });
                }
                else
                {
                    command2.Parameters.AddWithValue("@JobType", JobType);
                }

                if (Customer_Name == null)
                {
                    command2.Parameters.Add(new SqlParameter { ParameterName = "@Customer_Name", Value = DBNull.Value });
                }
                else
                {
                    command2.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                }

                if (Cust_Department == null)
                {
                    command2.Parameters.Add(new SqlParameter { ParameterName = "@Cust_Department", Value = DBNull.Value });
                }
                else
                {
                    command2.Parameters.AddWithValue("@Cust_Department", Cust_Department);
                }

                if (ProductName == null)
                {
                    command2.Parameters.Add(new SqlParameter { ParameterName = "@ProductName", Value = DBNull.Value });
                }
                else
                {
                    command2.Parameters.AddWithValue("@ProductName", ProductName);
                }

                if (CreateByIT == null)
                {
                    command2.Parameters.Add(new SqlParameter { ParameterName = "@PIC", Value = DBNull.Value });
                }
                else
                {
                    command2.Parameters.AddWithValue("@PIC", CreateByIT);
                }

                if (AccountsQty == null)
                {
                    command2.Parameters.Add(new SqlParameter { ParameterName = "@AccountsQty", Value = DBNull.Value });
                }
                else
                {
                    command2.Parameters.AddWithValue("@AccountsQty", AccountsQty);
                }

                if (ImpressionQty == null)
                {
                    command2.Parameters.Add(new SqlParameter { ParameterName = "@ImpressionQty", Value = DBNull.Value });
                }
                else
                {
                    command2.Parameters.AddWithValue("@ImpressionQty", ImpressionQty);
                }

                if (PagesQty == null)
                {
                    command2.Parameters.Add(new SqlParameter { ParameterName = "@PagesQty", Value = DBNull.Value });
                }
                else
                {
                    command2.Parameters.AddWithValue("@PagesQty", PagesQty);
                }

                command2.Parameters.AddWithValue("@Status", "Completed");
                command2.Parameters.AddWithValue("@JobAuditTrail", Id);

                command2.ExecuteNonQuery();
                cn2.Close();
            }

            return RedirectToAction("AddJAT", "ITO", new { Id = Id, Customer_Name = Customer_Name, LogTagNo = LogTagNo, JobClass = JobClass });
            //return RedirectToAction("ManageDailyJobITO", "ITO");
            //return View();
        }

        return View();
    }

    List<JobAuditTrailDetail> LogtagStatus = new List<JobAuditTrailDetail>();
    public ActionResult ManageLogTagStatus(string set, string LogTagNo, string Id)
    {
        if (set == "search")
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Id, CreatedOn, DateProcessItOn, LogTagNo, Status,
                                               ProductName, JobClass, Frequency, JobType, Customer_Name, AccountsQty, ImpressionQty, 
                                               PagesQty, Remarks, Company
                                               FROM [dbo].[JobAuditTrailDetail]
                                               WHERE LogTagNo LIKE @LogTagNo";

                command.Parameters.AddWithValue("@LogTagNo", "%" + LogTagNo + "%");
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
                            model.CreatedOn = reader.GetDateTime(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            model.DateProcessItOn = reader.GetDateTime(2);
                        }
                        if (reader.IsDBNull(3) == false)
                        {
                            model.LogTagNo = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            model.Status = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            model.ProductName = reader.GetString(5);
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
                            model.JobType = reader.GetString(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            model.Customer_Name = reader.GetString(9);
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            model.AccountsQty = reader.GetString(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            model.ImpressionQty = reader.GetString(11);
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            model.PagesQty = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            model.Remarks = reader.GetString(13);
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            model.Company = reader.GetString(14);
                        }
                    }
                    LogtagStatus.Add(model);
                }
                cn.Close();
            }
            return View(LogtagStatus);
        }

        if (set == "ViewAuditTrail")
        {
            return View();
        }

        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil = 1;
            cn.Open();

            //command.CommandText = @"SELECT Id, ModifiedOn, JobSheetNo, Customer_Name, ProductName, JobClass, 
            //                                   JobType,  AccountsQty, ImpressionQty, PagesQty, Status,
            //                                   IT_SysNotes,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes,
            //                                   ArtworkNotes, Acc_BillingNotes, DCPNotes, SalesExecutiveBy,Cust_Department
            //                                   FROM [dbo].[JobInstruction] WHERE Status != 'New' OR Status='Waiting to Assign Programmer' OR Status='Development Process' OR Status='Development Complete'";

            command.CommandText = @"SELECT JobAuditTrailDetail.Id, JobAuditTrailDetail.CreatedOn, JobAuditTrailDetail.DateProcessItOn, JobAuditTrailDetail.Status, 
                                   JobInstruction.Customer_Name, JobInstruction.ProductName, JobInstruction.Cust_Department, JobAuditTrailDetail.AccQty, JobAuditTrailDetail.ImpQty, JobAuditTrailDetail.PageQty,
                                  JobAuditTrailDetail.ProcessDate, JobAuditTrailDetail.StartDate,JobInstruction.JobClass,JobAuditTrailDetail.LogTagNo
                                 FROM  JobInstruction INNER JOIN
                   JobAuditTrailDetail ON JobInstruction.Id = JobAuditTrailDetail.JobInstructionId";
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
                        model.CreatedOn = reader.GetDateTime(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        model.DateProcessItOn = reader.GetDateTime(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        model.Status = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        model.Customer_Name = reader.GetString(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        model.ProductName = reader.GetString(5);
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        model.Cust_Department = reader.GetString(6);
                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        model.AccQty = reader.GetString(7);
                    }


                    if (reader.IsDBNull(8) == false)
                    {
                        model.ImpQty = reader.GetString(8);
                    }
                    if (reader.IsDBNull(9) == false)
                    {
                        model.PageQty = reader.GetString(9);
                    }
                    if (reader.IsDBNull(10) == false)
                    {
                        model.ProcessDate = reader.GetDateTime(10);
                    }
                    if (reader.IsDBNull(11) == false)
                    {
                        model.StartDate = reader.GetDateTime(1);
                    }
                    if (reader.IsDBNull(12) == false)
                    {
                        model.JobClass = reader.GetString(12);
                    }
                    if (reader.IsDBNull(13) == false)
                    {
                        model.LogTagNo = reader.GetString(13);
                    }
                }
                LogtagStatus.Add(model);
            }
            cn.Close();
        }
        return View(LogtagStatus);
    }

    List<JobAuditTrailDetail> JobAuditTrailDetail = new List<JobAuditTrailDetail>();
    public ActionResult ViewAT(string Id, string set, string JobInstructionId, string tabs, string Customer_Name, string ProductName, string JobSheetNo,
                               string SalesExecutiveBy, string Status,
                               string ServiceLevel, string IsSlaCreaditCard, string JobClass, string IsSetPaper,
                               string JobRequest, string ExpectedDateCompletionToGpo, string QuotationRef, string ContractName,
                               string ContactPerson, string JobType, string DeliveryChannel, string AccountsQty, string ImpressionQty,
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
                               string ArtworkNotes, string Acc_BillingNotes, string DCPNotes, string PostingInfo, JobInstruction get,
                               string ImageInDateOn, string ImageInTime, string RevisedInDateOn)
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







        //display data from table 
        List<JobAuditTrailDetail> JobAuditTrailDetail = new List<JobAuditTrailDetail>();

        int _bil = 1;
        using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn3))
        {


            cn3.Open();
            command.CommandText = @"SELECT ModeLog, Path, JobNameIT,JobId, ProgramId, FileId, RevStrtDateOn, RevStrtTime, DateProcessItOn, TimeProcessIt, DateApproveOn, AccQty, ImpQty, PageQty, FirstRecord,
                                      JobAuditTrailDetail.LastRecord, JobAuditTrailDetail.ImageInDateOn, JobAuditTrailDetail.ImageInTime, 
                                    RevisedInDateOn, Customer_Name,DateApproveTime,LogTagNo
                                      FROM [dbo].[JobAuditTrailDetail]
                                    WHERE Id=@Id ";
            command.Parameters.AddWithValue("@Id", Id);

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobAuditTrailDetail model = new JobAuditTrailDetail();
                {
                    model.Bil = _bil++;
                    if (reader.IsDBNull(0) == false)
                    {
                        ViewBag.ModeLog = reader.GetString(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        ViewBag.Path = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        ViewBag.JobNameIT = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        ViewBag.JobId = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        ViewBag.ProgramId = reader.GetString(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        ViewBag.FileId = reader.GetString(5);
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        ViewBag.RevStrtDateOnTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(6));
                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        ViewBag.RevStrtTime = reader.GetString(7);
                    }
                    if (reader.IsDBNull(8) == false)
                    {
                        ViewBag.DateProcessItOnTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(8));
                    }
                    if (reader.IsDBNull(9) == false)
                    {
                        ViewBag.TimeProcessIt = reader.GetString(9);
                    }
                    if (reader.IsDBNull(10) == false)
                    {
                        ViewBag.DateApproveOnTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(10));
                    }

                    if (reader.IsDBNull(11) == false)
                    {
                        ViewBag.AccQty = reader.GetString(11);
                    }
                    if (reader.IsDBNull(12) == false)
                    {
                        ViewBag.ImpQty = reader.GetString(12);
                    }
                    if (reader.IsDBNull(13) == false)
                    {
                        ViewBag.PageQty = reader.GetString(13);
                    }
                    if (reader.IsDBNull(14) == false)
                    {
                        ViewBag.FirstRecord = reader.GetString(14);
                    }
                    if (reader.IsDBNull(15) == false)
                    {
                        ViewBag.LastRecord = reader.GetString(15);
                    }

                    if (reader.IsDBNull(16) == false)
                    {
                        ViewBag.ImageInDateOnTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(16));
                    }
                    if (reader.IsDBNull(17) == false)
                    {
                        ViewBag.ImageInTime = reader.GetString(17);
                    }
                    if (reader.IsDBNull(18) == false)
                    {
                        ViewBag.RevisedInDateOnTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(18));
                    }
                    if (reader.IsDBNull(19) == false)
                    {
                        ViewBag.Customer_Name = reader.GetString(19);
                    }
                    if (reader.IsDBNull(20) == false)
                    {
                        ViewBag.DateApproveTime = reader.GetString(20);
                    }
                    if (reader.IsDBNull(21) == false)
                    {
                        ViewBag.LogTagNo = reader.GetString(21);
                    }

                }

                ViewBag.JobAuditTrailDetail = JobAuditTrailDetail;

            }

            //call table

        }

        //-----------------------------------------

        ReloadJATList(Id);

        //ReloadJAT(Id);

        return new Rotativa.ViewAsPdf("ViewAT", JobAuditTrailDetail)
        {
            // FileName = flightPlan.ListingItemDetailsModel.FlightDetails + ".pdf",
            PageMargins = new Rotativa.Options.Margins(5, 5, 5, 5),
            PageOrientation = Rotativa.Options.Orientation.Portrait,
            //PageWidth = 210,
            //PageHeight = 297
        };
    }


    //
    public ActionResult ReportJATPDF(string Id, string Customer_Name, string LogTagNo, string AccQty, string RevStrtDateOn, string RevStrtTime, string ProcessDate, string TimeProcessIt, string ProcessEnd, string DateApproveTime, string TimeEndProcessIt)
    {
        string JobClass = "";

        List<int> TotalAccQty = new List<int>();
        List<int> TotalImpQty = new List<int>();
        List<int> TotalPageQty = new List<int>();

        Debug.WriteLine("LogTagNo : " + LogTagNo);
        //string ProcessDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
        if (!string.IsNullOrEmpty(LogTagNo))
        {

            using (SqlConnection cn4 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command2 = new SqlCommand("", cn4))
            {


                cn4.Open();
                //command2.CommandText = @"SELECT
                //                            SUM((CASE WHEN ISNUMERIC(AccQty)=1
                //                            THEN CONVERT(decimal,AccQty) ELSE 0 END))                                           
                //                            AS [TotalAccQty]
                //                            FROM JobAuditTrailDetail                                    
                //                         WHERE LogTagNo=@LogTagNo ";
                command2.CommandText = @"SELECT AccQty FROM JobAuditTrailDetail                      
                                         WHERE LogTagNo=@LogTagNo ";
                command2.Parameters.AddWithValue("@LogTagNo", LogTagNo);
                var reader2 = command2.ExecuteReader();
                while (reader2.Read())
                {

                    try
                    {
                        TotalAccQty.Add(Int32.Parse(reader2.GetString(0)));
                    }
                    catch
                    {
                        TotalAccQty.Add(0);
                    }
                    //if (reader2.IsDBNull(0) == false)
                    //{
                    //    try
                    //    {
                    //        ViewBag.TotalAccQty = reader2.GetInt32(0);
                    //    }
                    //    catch
                    //    {
                    //        ViewBag.TotalAccQty = "-";
                    //    }
                    //}
                    //else
                    //{
                    //    ViewBag.TotalAccQty = "-";
                    //}

                }
                ViewBag.TotalAccQty = TotalAccQty.Sum();

            }

            using (SqlConnection cn4 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command2 = new SqlCommand("", cn4))
            {


                cn4.Open();
                //command2.CommandText = @"SELECT
                //                            SUM((CASE WHEN ISNUMERIC(ImpQty)=1
                //                            THEN CONVERT(decimal,ImpQty) ELSE 0 END))                                           
                //                            AS [TotalImpQty]
                //                            FROM JobAuditTrailDetail                                    
                //                         WHERE LogTagNo=@LogTagNo ";
                command2.CommandText = @"SELECT
                                           ImpQty
                                            FROM JobAuditTrailDetail                                    
                                         WHERE LogTagNo=@LogTagNo ";
                command2.Parameters.AddWithValue("@LogTagNo", LogTagNo);
                var reader2 = command2.ExecuteReader();
                while (reader2.Read())
                {
                    try
                    {
                        TotalImpQty.Add(Int32.Parse(reader2.GetString(0)));
                    }
                    catch
                    {
                        TotalImpQty.Add(0);
                    }
                }


                ViewBag.TotalImpQty = TotalImpQty.Sum();
            }

            using (SqlConnection cn4 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command2 = new SqlCommand("", cn4))
            {


                cn4.Open();
                //command2.CommandText = @"SELECT
                //                            SUM((CASE WHEN ISNUMERIC(PageQty)=1
                //                            THEN CONVERT(decimal,PageQty) ELSE 0 END))                                           
                //                            AS [TotalPageQty]
                //                            FROM JobAuditTrailDetail                                    
                //                         WHERE LogTagNo=@LogTagNo ";
                command2.CommandText = @"SELECT
                                            PageQty
                                            FROM JobAuditTrailDetail                                    
                                         WHERE LogTagNo=@LogTagNo ";
                command2.Parameters.AddWithValue("@LogTagNo", LogTagNo);
                var reader2 = command2.ExecuteReader();
                while (reader2.Read())
                {
                    try
                    {
                        TotalPageQty.Add(Int32.Parse(reader2.GetString(0)));
                    }
                    catch
                    {
                        TotalPageQty.Add(0);
                    }
                }

                ViewBag.TotalPageQty = TotalPageQty.Sum();

            }
        }


        if (!string.IsNullOrEmpty(LogTagNo))
        {

            List<JobAuditTrailDetail> JobAuditTrailDetail = new List<JobAuditTrailDetail>();

            int _bil = 1;
            using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn3))
            {


                cn3.Open();
                command.CommandText = @"SELECT LogTagNo, JobType, AccQty,ImpQty, PageQty, ProductName,Customer_Name, JobClass
                                      FROM [dbo].[JobAuditTrailDetail]
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
                        if (reader.IsDBNull(7) == false)
                        {
                            JobClass = reader.GetString(7);
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
                                if (!rs4.IsDBNull(0))
                                {
                                    eb.Customer_Name = rs4.GetString(0);
                                }
                                else
                                {
                                    eb.Customer_Name = "-";
                                }

                                if (!rs4.IsDBNull(1))
                                {
                                    eb.ProgramId = rs4.GetString(1);
                                }
                                else
                                {
                                    eb.ProgramId = "-";
                                }
                                if (!rs4.IsDBNull(2))
                                {
                                    eb.FileId = rs4.GetString(2);
                                }
                                else
                                {
                                    eb.FileId = "-";
                                }

                                if (!rs4.IsDBNull(3))
                                {
                                    eb.JobId = rs4.GetString(3);
                                }
                                else
                                {
                                    eb.JobId = "-";
                                }

                                if (!rs4.IsDBNull(4))
                                {
                                    eb.JobNameIT = rs4.GetString(4);
                                }
                                else
                                {
                                    eb.JobNameIT = "-";
                                }

                                if (!rs4.IsDBNull(5))
                                {
                                    eb.RevStrtDateOnTxt = rs4.GetDateTime(5).ToString("dd/MM/yyy");
                                }
                                else
                                {
                                    eb.RevStrtDateOnTxt = "-";
                                }

                                if (!rs4.IsDBNull(6))
                                {
                                    eb.RevStrtTime = rs4.GetString(6);
                                }
                                else
                                {
                                    eb.RevStrtTime = "-";
                                }

                                if (!rs4.IsDBNull(7))
                                {
                                    eb.ProcessDateStr = rs4.GetDateTime(7).ToString("dd/MM/yyy");
                                }
                                else
                                {
                                    eb.ProcessDateStr = "-";
                                }

                                if (!rs4.IsDBNull(8))
                                {
                                    eb.TimeProcessIt = rs4.GetString(8);
                                }
                                else
                                {
                                    eb.TimeProcessIt = "-";
                                }

                                if (!rs4.IsDBNull(9))
                                {
                                    eb.ProcessEndTxt = rs4.GetDateTime(9).ToString("dd/MM/yyy");
                                }
                                else
                                {
                                    eb.ProcessEndTxt = "-";
                                }

                                if (!rs4.IsDBNull(10))
                                {
                                    eb.AccQty = rs4.GetString(10);
                                }
                                else
                                {
                                    eb.AccQty = "-";
                                }

                                if (!rs4.IsDBNull(11))
                                {
                                    eb.ImpQty = rs4.GetString(11);
                                }
                                else
                                {
                                    eb.ImpQty = "-";
                                }

                                if (!rs4.IsDBNull(12))
                                {
                                    eb.PageQty = rs4.GetString(12);
                                }
                                else
                                {
                                    eb.PageQty = "-";
                                }

                                if (!rs4.IsDBNull(13))
                                {
                                    eb.FirstRecord = rs4.GetString(13);
                                }
                                else
                                {
                                    eb.FirstRecord = "-";
                                }

                                if (!rs4.IsDBNull(14))
                                {
                                    eb.LastRecord = rs4.GetString(14);
                                }
                                else
                                {
                                    eb.LastRecord = "-";
                                }

                                if (!rs4.IsDBNull(15))
                                {
                                    eb.LogTagNo = rs4.GetString(15);
                                }
                                else
                                {
                                    eb.LogTagNo = "-";
                                }

                                if (!rs4.IsDBNull(16))
                                {
                                    eb.ProductName = rs4.GetString(16);
                                }
                                else
                                {
                                    eb.ProductName = "-";
                                }

                                if (!rs4.IsDBNull(17))
                                {
                                    eb.JobType = rs4.GetString(17);
                                }
                                else
                                {
                                    eb.JobType = "-";
                                }

                                if (!rs4.IsDBNull(18))
                                {
                                    eb.DateApproveOnTxt = rs4.GetDateTime(18).ToString("dd/MM/yyy");
                                }
                                else
                                {
                                    eb.DateApproveOnTxt = "-";
                                }

                                if (!rs4.IsDBNull(19))
                                {
                                    eb.DateApproveTime = rs4.GetString(19);
                                }
                                else
                                {
                                    eb.DateApproveTime = "-";
                                }

                                if (!rs4.IsDBNull(20))
                                {
                                    eb.Type = rs4.GetString(20);
                                }
                                else
                                {
                                    eb.Type = "";
                                }

                                if (!rs4.IsDBNull(21))
                                {
                                    eb.DateProcessItOnTxt = rs4.GetDateTime(21).ToString("dd/MM/yyy");
                                }
                                else
                                {
                                    eb.DateProcessItOnTxt = "-";
                                }


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

        if (liEB.Count > 0)
        {

            return new Rotativa.ViewAsPdf("ReportJATPDF", ViewBag.ListPDF)
            {
                PageMargins = new Rotativa.Options.Margins(5, 5, 5, 5),
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                CustomSwitches = "--disable-smart-shrinking"
            };

            return View(ViewBag.ListPDF);

        }
        else
        {
            if (JobClass == "DAILY")
            {
                TempData["msgError"] = "<script>alert('Please fill in the audit trail detail first to view the audit trail.');</script>";
                return RedirectToAction("ManageJAT", "ITO");
            }
            else
            {
                TempData["msgError"] = "<script>alert('Please fill in the audit trail detail first to view the audit trail.');</script>";
                return RedirectToAction("ManageJAT2", "ITO");
            }

        }
    }




    List<JobAuditTrailDetail> liEB = new List<JobAuditTrailDetail>();
    private double totalamount;

    public ActionResult ViewPosting(string Id)
    {
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT JobAuditTrailId, PostingDateOn, PostingTime, Local, Oversea, Re_Turn, Courier, Doubles, PO_Box, Recovery,
                                    ReturnSts, RemarkRecovery, Status
                                    FROM [dbo].[PostingManifest]
                                    WHERE JobAuditTrailId=@Id
                                    ORDER BY PostingDateOn desc";

            command.Parameters.AddWithValue("@Id", Id);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.Id = reader.GetGuid(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        ViewBag.PostingDateOn = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(1));
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        ViewBag.PostingTime = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        ViewBag.Local = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        ViewBag.Oversea = reader.GetString(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        ViewBag.Re_Turn = reader.GetString(5);
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        ViewBag.Courier = reader.GetString(6);
                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        ViewBag.Doubles = reader.GetString(7);
                    }
                    if (reader.IsDBNull(8) == false)
                    {
                        ViewBag.PO_Box = reader.GetString(8);
                    }
                    if (reader.IsDBNull(9) == false)
                    {
                        ViewBag.Recovery = reader.GetString(9);
                    }
                    if (reader.IsDBNull(10) == false)
                    {
                        ViewBag.ReturnSts = reader.GetString(10);
                    }
                    if (reader.IsDBNull(11) == false)
                    {
                        ViewBag.RemarkRecovery = reader.GetString(11);
                    }
                    if (reader.IsDBNull(12) == false)
                    {
                        ViewBag.Status = reader.GetString(11);
                    }
                }
            }
            cn.Close();
        }
        return View();
    }

    public ActionResult DeleteAuditTrail(string Id, string Customer_Name, string LogTagNo, string JobClass, string PaperType, string JobSheetNo)
    {


        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        {
            cn.Open();

            SqlCommand cmd1 = new SqlCommand("DELETE FROM JobAuditTrailDetail WHERE Id=@Id", cn);
            cmd1.Parameters.AddWithValue("@Id", Id);
            cmd1.ExecuteNonQuery();

            SqlCommand cmd2 = new SqlCommand("SELECT TOP(1) Id FROM [dbo].[JobAuditTrailDetail] WHERE LogTagNo=@LogTagNo1", cn);
            cmd2.Parameters.AddWithValue("LogTagNo1", LogTagNo);
            SqlDataReader rm2 = cmd2.ExecuteReader();

            while(rm2.Read())
            {
                if(!rm2.IsDBNull(0))
                {
                    Id = rm2["Id"].ToString();

                }
                else
                {
                    Id = "00000000-0000-0000-0000-000000000000";

                }
            }
            

            cn.Close();
        }

        return RedirectToAction("AddJAT", "ITO", new { Id = Id, Customer_Name = Customer_Name, LogTagNo = LogTagNo, JobClass = JobClass, PaperType=PaperType, JobSheetNo= JobSheetNo });

    }

    public ActionResult SendBack(string JobSheetNo, string remark, string location, string from, string LogTagNo)
    {
        Debug.WriteLine("Location : " + location);
        Debug.WriteLine("from : " + from);

        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        {
            cn.Open();

            //used confirm100 column for jobinstruction and Remarks column for jobaudittrail  to save time
            if (location == "MBD")
            {
                SqlCommand cmd1 = new SqlCommand("UPDATE JobInstruction SET Confrm100 = @Confrm100, Status=@Status1 WHERE JobSheetNo = @JobSheetNo1", cn);
                cmd1.Parameters.AddWithValue("@Confrm100", remark);
                cmd1.Parameters.AddWithValue("@JobSheetNo1", JobSheetNo);
                cmd1.Parameters.AddWithValue("@Status1", "QM : Need correction from MBD");
                cmd1.ExecuteNonQuery();

                return RedirectToAction("ManageJobInstruction", "MBD");


            }
            else if (location == "ITO")
            {

                if (from == "Printing")
                {
                    SqlCommand cmd2 = new SqlCommand("UPDATE JobAuditTrailDetail SET Remarks = @Remarks4, Status=@Status6 WHERE LogTagNo = @LogTagNo4", cn);
                    cmd2.Parameters.AddWithValue("@Remarks4", remark);
                    cmd2.Parameters.AddWithValue("@LogTagNo4", LogTagNo);
                    cmd2.Parameters.AddWithValue("@Status6", "Printing : Need correction from ITO");
                    cmd2.ExecuteNonQuery();

                    return RedirectToAction("ManagePrint", "Printing");

                }
                else if (from == "Inserting")
                {
                    SqlCommand cmd2 = new SqlCommand("UPDATE JobAuditTrailDetail SET Remarks = @Remarks3, Status=@Status5 WHERE LogTagNo = @LogTagNo3", cn);
                    cmd2.Parameters.AddWithValue("@Remarks3", remark);
                    cmd2.Parameters.AddWithValue("@LogTagNo3", LogTagNo);
                    cmd2.Parameters.AddWithValue("@Status5", "Inserting : Need correction from ITO");
                    cmd2.ExecuteNonQuery();

                    return RedirectToAction("ManageInsert", "Inserting");

                }
                else if (from == "SelfMailer")
                {
                    SqlCommand cmd2 = new SqlCommand("UPDATE JobAuditTrailDetail SET Remarks = @Remarks2, Status=@Status4 WHERE LogTagNo = @LogTagNo2", cn);
                    cmd2.Parameters.AddWithValue("@Remarks2", remark);
                    cmd2.Parameters.AddWithValue("@LogTagNo2", LogTagNo);
                    cmd2.Parameters.AddWithValue("@Status4", "SelfMailer : Need correction from ITO");
                    cmd2.ExecuteNonQuery();

                    return RedirectToAction("ManageSM", "SELFMAILER");

                }
                else if (from == "MMP")
                {
                    SqlCommand cmd2 = new SqlCommand("UPDATE JobAuditTrailDetail SET Remarks = @Remarks, Status=@Status3 WHERE LogTagNo = @LogTagNo", cn);
                    cmd2.Parameters.AddWithValue("@Remarks", remark);
                    cmd2.Parameters.AddWithValue("@LogTagNo", LogTagNo);
                    cmd2.Parameters.AddWithValue("@Status3", "MMP : Need correction from ITO");
                    cmd2.ExecuteNonQuery();

                    return RedirectToAction("ManageMMP", "MMP");

                }
                else if (from == "Planner")
                {
                    SqlCommand cmd2 = new SqlCommand("UPDATE JobAuditTrailDetail SET Remarks = @Remarks, Status=@Status3 WHERE LogTagNo = @LogTagNo", cn);
                    cmd2.Parameters.AddWithValue("@Remarks", remark);
                    cmd2.Parameters.AddWithValue("@LogTagNo", LogTagNo);
                    cmd2.Parameters.AddWithValue("@Status3", "Planner : Need correction from ITO");
                    cmd2.ExecuteNonQuery();

                    return RedirectToAction("ManageMMP", "MMP");

                }
                else
                {
                    SqlCommand cmd1 = new SqlCommand("UPDATE JobInstruction SET Confrm100 = @Confrm100, Status=@Status1 WHERE JobSheetNo = @JobSheetNo1", cn);
                    cmd1.Parameters.AddWithValue("@Confrm100", remark);
                    cmd1.Parameters.AddWithValue("@JobSheetNo1", JobSheetNo);
                    cmd1.Parameters.AddWithValue("@Status1", "QM : Need correction from ITO");
                    cmd1.ExecuteNonQuery();

                    SqlCommand cmd2 = new SqlCommand("UPDATE JobAuditTrailDetail SET Remarks = @Remarks, Status=@Status2 WHERE JobSheetNo = @JobSheetNo2", cn);
                    cmd2.Parameters.AddWithValue("@Remarks", remark);
                    cmd2.Parameters.AddWithValue("@JobSheetNo2", JobSheetNo);
                    cmd2.Parameters.AddWithValue("@Status2", "QM : Need correction from ITO");
                    cmd2.ExecuteNonQuery();

                    return RedirectToAction("ManageQM", "QM");

                }


            }


            cn.Close();

            return RedirectToAction("Index", "Home");

        }



        //if (from=="DDP")
        //{
        //    return RedirectToAction("QM", "ManageQMDDP");
        //}
        //else
        //{
        //    return RedirectToAction("QM", "ManageQM");
        //}
    }

    public ActionResult SubmitCorrection(string JobSheetNo, string LogTagNo, string from)
    {
        string Status = "";
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        {
            cn.Open();

            if (!string.IsNullOrEmpty(LogTagNo))
            {
                SqlCommand cmd1 = new SqlCommand("SELECT Status FROM JobAuditTrailDetail WHERE LogTagNo=@LogTagNo1", cn);
                cmd1.Parameters.AddWithValue("@LogTagNo1", LogTagNo);
                SqlDataReader rm1 = cmd1.ExecuteReader();

                while (rm1.Read())
                {
                    Status = rm1.GetString(0);
                }
            }
            else
            {
                SqlCommand cmd1 = new SqlCommand("SELECT Status FROM JobInstruction WHERE JobSheetNo=@JobSheetNo1", cn);
                cmd1.Parameters.AddWithValue("@JobSheetNo1", JobSheetNo);
                SqlDataReader rm1 = cmd1.ExecuteReader();

                while (rm1.Read())
                {
                    Status = rm1.GetString(0);
                }
            }


            string[] Destination = Status.Split(':');

            Debug.WriteLine("Destination : " + Destination[0]);

            if (Destination[0].Contains("MBD"))
            {
                SqlCommand cmd2 = new SqlCommand("UPDATE JobInstruction SET Status='MBD' WHERE JobSheetNo=@JobSheetNo1", cn);
                cmd2.Parameters.AddWithValue("@JobSheetNo1", JobSheetNo);
                cmd2.ExecuteNonQuery();
            }
            else if (Destination[0].Contains("QM"))
            {
                SqlCommand cmd2 = new SqlCommand("UPDATE JobInstruction SET Status='QME' WHERE JobSheetNo=@JobSheetNo2", cn);
                cmd2.Parameters.AddWithValue("@JobSheetNo2", JobSheetNo);
                cmd2.ExecuteNonQuery();

                SqlCommand cmd3 = new SqlCommand("UPDATE JobAuditTrailDetail SET Status='QME' WHERE JobSheetNo=@JobSheetNo3", cn);
                cmd3.Parameters.AddWithValue("@JobSheetNo3", JobSheetNo);
                cmd3.ExecuteNonQuery();
            }
            else if (Destination[0].Contains("Planner"))
            {
                SqlCommand cmd3 = new SqlCommand("UPDATE JobAuditTrailDetail SET Status='PLANNER' WHERE LogTagNo=@LogTagNo", cn);
                cmd3.Parameters.AddWithValue("@LogTagNo", LogTagNo);
                cmd3.ExecuteNonQuery();
            }
            else if (Destination[0].Contains("Printing"))
            {
                SqlCommand cmd3 = new SqlCommand("UPDATE JobAuditTrailDetail SET Status='PRODUCTION' WHERE LogTagNo=@LogTagNo2", cn);
                cmd3.Parameters.AddWithValue("@LogTagNo2", LogTagNo);
                cmd3.ExecuteNonQuery();
            }
            else if (Destination[0].Contains("SelfMailer"))
            {
                SqlCommand cmd3 = new SqlCommand("UPDATE JobAuditTrailDetail SET Status='SELFMAILER' WHERE LogTagNo=@LogTagNo3", cn);
                cmd3.Parameters.AddWithValue("@LogTagNo3", LogTagNo);
                cmd3.ExecuteNonQuery();
            }
            else if (Destination[0].Contains("MMP"))
            {
                SqlCommand cmd3 = new SqlCommand("UPDATE JobAuditTrailDetail SET Status='MMP' WHERE LogTagNo=@LogTagNo4", cn);
                cmd3.Parameters.AddWithValue("@LogTagNo4", LogTagNo);
                cmd3.ExecuteNonQuery();
            }
            else
            {

            }

            //if(Status=="QM : Need correction from MBD"||Status=="QM : Need correction from ITO"||Status=="Printing : Need Correction from ITO")
            //{
            //    if(Status == "QM : Need correction from MBD")
            //    {
            //        SqlCommand cmd2 = new SqlCommand("UPDATE JobInstruction SET Status='QME' WHERE JobSheetNo=@JobSheetNo2", cn);
            //        cmd2.Parameters.AddWithValue("@JobSheetNo2", JobSheetNo);
            //        cmd2.ExecuteNonQuery();
            //    }
            //    else if(Status == "QM : Need correction from ITO")
            //    {
            //        SqlCommand cmd2 = new SqlCommand("UPDATE JobInstruction SET Status='QME' WHERE JobSheetNo=@JobSheetNo2", cn);
            //        cmd2.Parameters.AddWithValue("@JobSheetNo2", JobSheetNo);
            //        cmd2.ExecuteNonQuery();

            //        SqlCommand cmd3 = new SqlCommand("UPDATE JobAuditTrailDetail SET Status='QME' WHERE JobSheetNo=@JobSheetNo3",cn);
            //        cmd3.Parameters.AddWithValue("@JobSheetNo3", JobSheetNo);
            //        cmd3.ExecuteNonQuery();
            //    }
            //    else if (Status == "Printing : Need correction from ITO")
            //    {
            //        SqlCommand cmd2 = new SqlCommand("UPDATE JobInstruction SET Status='PRODUCTION' WHERE JobSheetNo=@JobSheetNo2", cn);
            //        cmd2.Parameters.AddWithValue("@JobSheetNo2", JobSheetNo);
            //        cmd2.ExecuteNonQuery();

            //        SqlCommand cmd3 = new SqlCommand("UPDATE JobAuditTrailDetail SET Status='PRODUCTION' WHERE JobSheetNo=@JobSheetNo3", cn);
            //        cmd3.Parameters.AddWithValue("@JobSheetNo3", JobSheetNo);
            //        cmd3.ExecuteNonQuery();
            //    }

            //}

            cn.Close();

            if (from.Contains("MBD"))
            {
                return RedirectToAction("ManageJobInstruction", "MBD");
            }
            else if (from.Contains("ITO"))
            {
                return RedirectToAction("ManageJAT", "ITO");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }

        //if (Status == "QM : Need correction from MBD")
        //{
        //    return RedirectToAction("ManageJobInstruction", "MBD");
        //}
        //else
        //{
        //    return RedirectToAction("ManageJAT", "ITO");
        //}


        //return View();
    }





}

