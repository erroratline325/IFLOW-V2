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


[MvcAppV2.FilterConfig.UserSessionActionFilter]
public class BSSController : Controller
{
    string PathSource = System.Configuration.ConfigurationManager.AppSettings["SourceFile"];
    string IpSMtp_ = System.Configuration.ConfigurationManager.AppSettings["IpSMtp"];
    string PortSmtp_ = System.Configuration.ConfigurationManager.AppSettings["PortSmtp"];


    List<JobInstruction> JobInstructionlist1 = new List<JobInstruction>();
    public Document doc { get; private set; }
  
   
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
                                        FROM [IflowSeed].[dbo].[ProgDevWorksheet] 
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
                                        FROM [IflowSeed].[dbo].[ProgDevWorksheet] 
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
    public ActionResult CreateProgDevWorksheet(string Id, string JobInstructionId, string Set, string Customer_Name, string ProductName, string JobClass, string JobSheetNo, string JobRequest, string SalesExecutiveBy, string Status, string Complexity,
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
                command = new SqlCommand("UPDATE [IflowSeed].[dbo].[ProgDevWorksheet] SET ModifiedOn=@ModifiedOn,up_1=@up_1,up_2=@up_2,MainProgramId=@MainProgramId,ProgramId=@ProgramId,ProgramDesc=@ProgramDesc,TypeOfData=@TypeOfData,StartDevOn=@StartDevOn,CompleteDevOn=@CompleteDevOn,ReasonDev=@ReasonDev,IsDedup=@IsDedup,Dedup=@Dedup,IsSplitting=@IsSplitting,Splitting=@Splitting,IsRestructuring=@IsRestructuring,Restructuring=@Restructuring,Charges=@Charges,TotalCharges=@TotalCharges,Status=@Status,CreateUser=@CreateUser,IsReviseTemplate=@IsReviseTemplate,ReviseTemplate=@ReviseTemplate,IsReviseContent=@IsReviseContent,ReviseContent=@ReviseContent,IsReviseDataStructure=@IsReviseDataStructure,ReviseDataStructure=@ReviseDataStructure,Field_1until10=@Field_1until10,Field_11until20=@Field_11until20,Field_21until30=@Field_21until30,AmendmentCharges=@AmendmentCharges WHERE Id=@Id", cn);
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
                command.Parameters.AddWithValue("@Dedup", Dedup);
                if (IsSplitting == "on")
                {
                    command.Parameters.AddWithValue("@IsSplitting", true);
                }
                else
                {
                    command.Parameters.AddWithValue("@IsSplitting", false);
                }
                command.Parameters.AddWithValue("@Splitting", Splitting);
                if (IsRestructuring == "on")
                {
                    command.Parameters.AddWithValue("@IsRestructuring", true);
                }
                else
                {
                    command.Parameters.AddWithValue("@IsRestructuring", false);
                }
                command.Parameters.AddWithValue("@Restructuring", Restructuring);
                command.Parameters.AddWithValue("@Charges", Charges);
                command.Parameters.AddWithValue("@TotalCharges", TotalCharges);
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
                command = new SqlCommand("UPDATE [IflowSeed].[dbo].[ProgDevWorksheet] SET IsReviseTemplate=@IsReviseTemplate,ReviseTemplate=@ReviseTemplate,IsReviseContent=@IsReviseContent,ReviseContent=@ReviseContent,IsReviseDataStructure=@IsReviseDataStructure,ReviseDataStructure=@ReviseDataStructure,Field_1until10=@Field_1until10,Field_11until20=@Field_11until20,Field_21until30=@Field_21until30,AmendmentCharges=@AmendmentCharges,ProgramType=@ProgramType WHERE Id=@Id", cn);
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
                command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[ITO_NewProgram] (Id,ProgDevWorksheetId,Activities,Duration,Charges) values (@Id,@ProgDevWorksheetId,@Activities,@Duration,@Charges)", cn2);
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
                command = new SqlCommand("UPDATE [IflowSeed].[dbo].[ProgDevWorksheet] SET ProgramType=@ProgramType WHERE Id=@Id", cn);
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
                                      FROM [IflowSeed].[dbo].[ITO_NewProgram]  
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
                                          FROM [IflowSeed].[dbo].[ITO_NewProgram]
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
                            command3 = new SqlCommand("DELETE [IflowSeed].[dbo].[ITO_NewProgram] WHERE Id=@Id", cn3);
                            command3.Parameters.AddWithValue("@Id", Id);
                            command3.ExecuteNonQuery();
                            cn3.Close();

                        }

                    }

                    if (reader.IsDBNull(1) == false)
                    {
                        ITO_NewProgramId = reader.GetGuid(1);
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
                    command = new SqlCommand("UPDATE [IflowSeed].[dbo].[ProgDevWorksheet]  SET STATUS='Development Complete' WHERE Id=@Id", cn);
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                    cn.Close();

                    using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        ViewBag.JobInstructionId = JobInstructionId;
                        cn3.Open();
                        SqlCommand command1;
                        command1 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET STATUS='Development Complete' WHERE Id=@Id", cn3);
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
                                        FROM [IflowSeed].[dbo].[ProgDevWorksheet] 
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
                                        FROM [IflowSeed].[dbo].[ProgDevWorksheet] 
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
                                              FROM [IflowSeed].[dbo].[ProgDevWorksheet]
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
                                            FROM [IflowSeed].[dbo].[ITO_NewProgram]
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
                                   FROM [IflowSeed].[dbo].[ProgDevWorksheet]    
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
                                           FROM [IflowSeed].[dbo].[ProgDevWorksheet]    
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
                                           FROM [IflowSeed].[dbo].[ITO_NewProgram]
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
        using (SqlCommand command = new SqlCommand("", cn4))
        {
            int _bil = 1;
            cn4.Open();
            command.CommandText = @"SELECT Activities,Duration,Charges, ProgDevWorksheetId
                                           FROM [IflowSeed].[dbo].[ITO_NewProgram] 
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
                cn4.Close();

            }
        }

        //-----------------------------------------

        ReloadWorksheetList(Id);

        return new Rotativa.ViewAsPdf("PrintProgDevWorkSheet", viewProgDevWorksheet)
        {
            // FileName = flightPlan.ListingItemDetailsModel.FlightDetails + ".pdf",
            PageMargins = new Rotativa.Options.Margins(5, 5, 5, 5),
            PageOrientation = Rotativa.Options.Orientation.Portrait,
            //PageWidth = 210,
            //PageHeight = 297
        };
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
                                           FROM[IflowSeed].[dbo].[ProgDevWorksheet] b, [IflowSeed].[dbo].[ITO_NewProgram] a
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


    List<JobInstruction> viewDailyJob = new List<JobInstruction>();
    public ActionResult ManageDailyJob(JobAuditTrail get, string Id, string Customer_Name, string ProductName, string JobClass, string JobSheetNo, string JobRequest, string JobType, string set, string Status,
                                       string AccountsQty, string ImpressionQty, string PagesQty, string Frequency, string JobInstructionId)
    {
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.IsRole = @Session["Role"];
        var IdentityName = @Session["Fullname"];



        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT Id, ModifiedOn, Customer_Name, Cust_Department, ProductName,JobClass, 
                                               JobType,Status, AccountsQty,ImpressionQty, PagesQty,
                                               IT_SysNotes,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes,
                                               ArtworkNotes, Acc_BillingNotes, DCPNotes,JobSheetNo
                                               FROM [IflowSeed].[dbo].[JobInstruction] 
                                               WHERE (Status = 'ITO') AND (JobClass='DAILY') OR (Status = 'EXISTING JI') AND (JobClass='DAILY')";
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
                        model.JobClass = reader.GetString(5);
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        model.JobType = reader.GetString(6);
                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        model.Status = reader.GetString(7);
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
                        model.JobSheetNo = reader.GetString(18);
                    }


                }
                viewDailyJob.Add(model);
            }
            cn.Close();
        }

        return View(viewDailyJob);

    }


    List<JobInstruction> viewSchedulerJob = new List<JobInstruction>();
    public ActionResult ManageSchedulerJob(string set, string ProductName)
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
                command.CommandText = @"SELECT Id, ModifiedOn, JobSheetNo, Customer_Name, ProductName, JobClass, 
                                               JobType,  AccountsQty, ImpressionQty, PagesQty, Status,
                                               IT_SysNotes,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes,
                                               ArtworkNotes, Acc_BillingNotes, DCPNotes
                                               FROM [IflowSeed].[dbo].[JobInstruction] 
                                               WHERE (Status = 'ITO') AND (JobClass !='DAILY') OR (Status = 'EXISTING JI') AND (JobClass !='DAILY')
                                               OR (Status = 'PLANNER')
                                               AND ProductName LIKE @ProductName";
                command.Parameters.AddWithValue("@ProductName", "%" + ProductName + "%");
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
                    }
                    viewSchedulerJob.Add(model);
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
                command.CommandText = @"SELECT Id, ModifiedOn, JobSheetNo, Customer_Name, ProductName, JobClass, 
                                               JobType,  AccountsQty, ImpressionQty, PagesQty, Status,
                                               IT_SysNotes,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes,
                                               ArtworkNotes, Acc_BillingNotes, DCPNotes, SalesExecutiveBy
                                               FROM [IflowSeed].[dbo].[JobInstruction] 
                                               WHERE (Status = 'ITO') AND (JobClass !='DAILY') OR (Status = 'EXISTING JI') AND (JobClass !='DAILY')
                                               OR (Status = 'PLANNER')";
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
                                      FROM [IflowSeed].[dbo].[SchedulerJob]  
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

        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.AccountManager = IdentityName.ToString();
        Session["JobInstructionId"] = Id;
        Session["Id"] = Id;
        ViewBag.Id = Id;
        ViewBag.JobInstructionId = JobInstructionId;





        return View();

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
                                                JobType, SalesExecutiveBy
                                      FROM [IflowSeed].[dbo].[JobInstruction]  
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
                    }



                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        get.JobRequest = Convert.ToDateTime(get.JobRequestTxt);
                        Guid Idx = Guid.NewGuid();
                        cn.Open();
                        SqlCommand command;
                        command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[SchedulerJob] (Id, Customer_Name, Frequency, JobRequest, ProductName, JobClass, JobType, SalesExecutiveBy, JobInstructionId) values (@Id, @Customer_Name, @Frequency, @JobRequest, @ProductName, @JobClass, @JobType, @SalesExecutiveBy, @JobInstructionId)", cn);
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
                        command.Parameters.AddWithValue("@SalesExecutiveBy", model.SalesExecutiveBy);
                        command.Parameters.AddWithValue("@JobInstructionId", model.Id);
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
                command = new SqlCommand("DELETE [IflowSeed].[dbo].[SchedulerJob] WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn.Close();
            }


        }
        return RedirectToAction("ViewSchedulerJob", "ITO", new { Id = Session["Id"].ToString() });
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
                                           FROM [IflowSeed].[dbo].[JobInstruction] a, [IflowSeed].[dbo].[SchedulerJob] b
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
                        model.Id = reader.GetGuid(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        model.JobRequest = reader.GetDateTime(1);
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
                        model.Frequency = reader.GetString(7);
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
                        model.JobInstructionId = reader.GetGuid(11);
                    }
                    if (reader.IsDBNull(12) == false)
                    {
                        model.IT_SysNotes = reader.GetString(12);
                    }
                    if (reader.IsDBNull(13) == false)
                    {
                        model.Produc_PlanningNotes = reader.GetString(13);
                    }
                    if (reader.IsDBNull(14) == false)
                    {
                        model.PurchasingNotes = reader.GetString(14);
                    }
                    if (reader.IsDBNull(15) == false)
                    {
                        model.EngineeringNotes = reader.GetString(15);
                    }
                    if (reader.IsDBNull(16) == false)
                    {
                        model.ArtworkNotes = reader.GetString(16);
                    }
                    if (reader.IsDBNull(17) == false)
                    {
                        model.Acc_BillingNotes = reader.GetString(17);
                    }
                    if (reader.IsDBNull(18) == false)
                    {
                        model.DCPNotes = reader.GetString(18);
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
                    command1 = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[JobAuditTrail](Id, CreatedOn, Customer_Name, Frequency, JobRequest, JobSheetNo, ProductName, JobClass, JobType, Status, AccountsQty, ImpressionQty, PagesQty, NotesByIT, NotesByProduction, NotesByPurchasing, NotesByEngineering, NotesByArtwork, NotesByFinance, NotesByDCP, JobInstructionId) values (@Id, @CreatedOn, @Customer_Name, @Frequency, @JobRequest, @JobSheetNo, @ProductName, @JobClass, @JobType, @Status, @AccountsQty, @ImpressionQty, @PagesQty,  @NotesByIT, @NotesByProduction, @NotesByPurchasing, @NotesByEngineering, @NotesByArtwork, @NotesByFinance, @NotesByDCP, @JobInstructionId)", cn1);
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





    List<JobAuditTrail> viewJobAuditTrail = new List<JobAuditTrail>();
    public ActionResult ManageJobAuditTrail(string set, string ProductName)
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
                command.CommandText = @"SELECT Id,ModifiedOn, Customer_Name, ProductName, JobSheetNo, JobClass, 
                                               Frequency, JobType, LogTagNo, AccountsQty, ImpressionQty, PagesQty, 
                                               TotalAuditTrail, Remarks, CreateByIT, Status,
                                               ModeLog, Path,JobNameIT, JobId, ProgramId,FileId,RevStrtDateOn,
                                               RevStrtTime,DateProcessItOn,TimeProcessIt,DateApproveOn,DateApproveTime,
                                               FirstRecord,LastRecord, Type, CreatedOn,JobInstructionId,CreateByIT,
                                               NotesByIT, NotesByProduction, NotesByPurchasing, NotesByEngineering, NotesByArtwork,NotesByFinance,NotesByDCP,
                                               ImageInDateOn,ImageInTime,RevisedInDateOn
                                        FROM [IflowSeed].[dbo].[JobAuditTrail] 
                                        WHERE (Status = 'New') OR (Status ='Waiting Approval')
                                        AND ProductName LIKE @ProductName
                                        ORDER BY CreatedOn desc";
                command.Parameters.AddWithValue("@ProductName", "%" + ProductName + "%");
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
                            model.Remarks = reader.GetString(13);
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            model.CreateByIT = reader.GetString(14);
                        }
                        if (reader.IsDBNull(15) == false)
                        {
                            model.Status = reader.GetString(15);
                        }
                        if (reader.IsDBNull(16) == false)
                        {
                            model.ModeLog = reader.GetString(16);
                        }
                        if (reader.IsDBNull(17) == false)
                        {
                            model.Path = reader.GetString(17);
                        }
                        if (reader.IsDBNull(18) == false)
                        {
                            model.JobNameIT = reader.GetString(18);
                        }
                        if (reader.IsDBNull(19) == false)
                        {
                            model.JobId = reader.GetString(19);
                        }
                        if (reader.IsDBNull(20) == false)
                        {
                            model.ProgramId = reader.GetString(20);
                        }
                        if (reader.IsDBNull(21) == false)
                        {
                            model.FileId = reader.GetString(21);
                        }
                        if (reader.IsDBNull(22) == false)
                        {
                            model.RevStrtDateOn = reader.GetDateTime(22);
                        }
                        if (reader.IsDBNull(23) == false)
                        {
                            model.RevStrtTime = reader.GetString(23);
                        }
                        if (reader.IsDBNull(24) == false)
                        {
                            model.DateProcessItOn = reader.GetDateTime(24);
                        }
                        if (reader.IsDBNull(25) == false)
                        {
                            model.TimeProcessIt = reader.GetString(25);
                        }
                        if (reader.IsDBNull(26) == false)
                        {
                            model.DateApproveOn = reader.GetDateTime(26);
                        }
                        if (reader.IsDBNull(27) == false)
                        {
                            model.DateApproveTime = reader.GetString(27);
                        }
                        if (reader.IsDBNull(28) == false)
                        {
                            model.FirstRecord = reader.GetString(28);
                        }
                        if (reader.IsDBNull(29) == false)
                        {
                            model.LastRecord = reader.GetString(29);
                        }
                        if (reader.IsDBNull(30) == false)
                        {
                            model.Type = reader.GetString(30);
                        }
                        if (reader.IsDBNull(31) == false)
                        {
                            model.CreatedOn = reader.GetDateTime(31);
                        }
                        if (reader.IsDBNull(32) == false)
                        {
                            model.JobInstructionId = reader.GetGuid(32);
                        }
                        if (reader.IsDBNull(33) == false)
                        {
                            model.CreateByIT = reader.GetString(33);
                        }
                        if (reader.IsDBNull(34) == false)
                        {
                            model.NotesByIT = reader.GetString(34);
                        }
                        if (reader.IsDBNull(35) == false)
                        {
                            model.NotesByProduction = reader.GetString(35);
                        }
                        if (reader.IsDBNull(36) == false)
                        {
                            model.NotesByPurchasing = reader.GetString(36);
                        }
                        if (reader.IsDBNull(37) == false)
                        {
                            model.NotesByEngineering = reader.GetString(37);
                        }
                        if (reader.IsDBNull(38) == false)
                        {
                            model.NotesByArtwork = reader.GetString(38);
                        }
                        if (reader.IsDBNull(39) == false)
                        {
                            model.NotesByFinance = reader.GetString(39);
                        }
                        if (reader.IsDBNull(40) == false)
                        {
                            model.NotesByDCP = reader.GetString(40);
                        }
                        if (reader.IsDBNull(41) == false)
                        {
                            model.ImageInDateOn = reader.GetDateTime(41);
                        }
                        if (reader.IsDBNull(42) == false)
                        {
                            model.ImageInTime = reader.GetString(42);
                        }
                        if (reader.IsDBNull(43) == false)
                        {
                            model.RevisedInDateOn = reader.GetDateTime(43);
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
                                               Frequency, JobType, LogTagNo, AccountsQty, ImpressionQty, PagesQty, 
                                               TotalAuditTrail, Remarks, CreateByIT, Status,
                                               ModeLog, Path,JobNameIT, JobId, ProgramId,FileId,RevStrtDateOn,
                                               RevStrtTime,DateProcessItOn,TimeProcessIt,DateApproveOn,DateApproveTime,
                                               FirstRecord,LastRecord, Type, CreatedOn,JobInstructionId,CreateByIT,
                                               NotesByIT, NotesByProduction, NotesByPurchasing, NotesByEngineering, NotesByArtwork,NotesByFinance,NotesByDCP,
                                               ImageInDateOn,ImageInTime,RevisedInDateOn
                                        FROM [IflowSeed].[dbo].[JobAuditTrail] 
                                        WHERE (Status = 'New') OR (Status ='Waiting Approval')
                                        ORDER BY CreatedOn desc";
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
                            model.Remarks = reader.GetString(13);
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            model.CreateByIT = reader.GetString(14);
                        }
                        if (reader.IsDBNull(15) == false)
                        {
                            model.Status = reader.GetString(15);
                        }
                        if (reader.IsDBNull(16) == false)
                        {
                            model.ModeLog = reader.GetString(16);
                        }
                        if (reader.IsDBNull(17) == false)
                        {
                            model.Path = reader.GetString(17);
                        }
                        if (reader.IsDBNull(18) == false)
                        {
                            model.JobNameIT = reader.GetString(18);
                        }
                        if (reader.IsDBNull(19) == false)
                        {
                            model.JobId = reader.GetString(19);
                        }
                        if (reader.IsDBNull(20) == false)
                        {
                            model.ProgramId = reader.GetString(20);
                        }
                        if (reader.IsDBNull(21) == false)
                        {
                            model.FileId = reader.GetString(21);
                        }
                        if (reader.IsDBNull(22) == false)
                        {
                            model.RevStrtDateOn = reader.GetDateTime(22);
                        }
                        if (reader.IsDBNull(23) == false)
                        {
                            model.RevStrtTime = reader.GetString(23);
                        }
                        if (reader.IsDBNull(24) == false)
                        {
                            model.DateProcessItOn = reader.GetDateTime(24);
                        }
                        if (reader.IsDBNull(25) == false)
                        {
                            model.TimeProcessIt = reader.GetString(25);
                        }
                        if (reader.IsDBNull(26) == false)
                        {
                            model.DateApproveOn = reader.GetDateTime(26);
                        }
                        if (reader.IsDBNull(27) == false)
                        {
                            model.DateApproveTime = reader.GetString(27);
                        }
                        if (reader.IsDBNull(28) == false)
                        {
                            model.FirstRecord = reader.GetString(28);
                        }
                        if (reader.IsDBNull(29) == false)
                        {
                            model.LastRecord = reader.GetString(29);
                        }
                        if (reader.IsDBNull(30) == false)
                        {
                            model.Type = reader.GetString(30);
                        }
                        if (reader.IsDBNull(31) == false)
                        {
                            model.CreatedOn = reader.GetDateTime(31);
                        }
                        if (reader.IsDBNull(32) == false)
                        {
                            model.JobInstructionId = reader.GetGuid(32);
                        }
                        if (reader.IsDBNull(33) == false)
                        {
                            model.CreateByIT = reader.GetString(33);
                        }
                        if (reader.IsDBNull(34) == false)
                        {
                            model.NotesByIT = reader.GetString(34);
                        }
                        if (reader.IsDBNull(35) == false)
                        {
                            model.NotesByProduction = reader.GetString(35);
                        }
                        if (reader.IsDBNull(36) == false)
                        {
                            model.NotesByPurchasing = reader.GetString(36);
                        }
                        if (reader.IsDBNull(37) == false)
                        {
                            model.NotesByEngineering = reader.GetString(37);
                        }
                        if (reader.IsDBNull(38) == false)
                        {
                            model.NotesByArtwork = reader.GetString(38);
                        }
                        if (reader.IsDBNull(39) == false)
                        {
                            model.NotesByFinance = reader.GetString(39);
                        }
                        if (reader.IsDBNull(40) == false)
                        {
                            model.NotesByDCP = reader.GetString(40);
                        }
                        if (reader.IsDBNull(41) == false)
                        {
                            model.ImageInDateOn = reader.GetDateTime(41);
                        }
                        if (reader.IsDBNull(42) == false)
                        {
                            model.ImageInTime = reader.GetString(42);
                        }
                        if (reader.IsDBNull(43) == false)
                        {
                            model.RevisedInDateOn = reader.GetDateTime(43);
                        }

                    }
                    viewJobAuditTrail.Add(model);
                }
                cn.Close();
            }
        }



        return View(viewJobAuditTrail);

    }




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
                    command = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobAuditTrail] SET ModifiedOn=@ModifiedOn, LogTagNo=@LogTagNo, CreateByIt=@CreateByIt WHERE Id=@Id", cn);
                    command.Parameters.AddWithValue("@ModifiedOn", createdOn);
                    command.Parameters.AddWithValue("@LogTagNo", No_.RefNo);
                    command.Parameters.AddWithValue("@CreateByIt", IdentityName.ToString());
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                    cn.Close();
                }
                //bila save akn gi kt managecustomer
                return RedirectToAction("ManageJobAuditTrail", "ITO");

            }
            else
            {
                TempData["msg"] = "<script>alert('LOG TAG NO ALREADY CREATED !');</script>";
                return RedirectToAction("ManageJobAuditTrail", "ITO");
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
                return RedirectToAction("ManageJobAuditTrail", "ITO");
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


        if (set == "Manual")
        {

            if (string.IsNullOrEmpty(LogTagNo))
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    cn.Open();
                    command.CommandText = @"SELECT Id,LogTagNo
                                    FROM [IflowSeed].[dbo].[JobAuditTrail]
                                    WHERE Id=@Id ";
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
                    command = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobAuditTrail] SET ModifiedOn=@ModifiedOn, LogTagNo=@LogTagNo, CreateByIt=@CreateByIt WHERE Id=@Id", cn);
                    command.Parameters.AddWithValue("@ModifiedOn", createdOn);
                    command.Parameters.AddWithValue("@LogTagNo", LogTagNo);
                    command.Parameters.AddWithValue("@CreateByIt", IdentityName.ToString());
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                    cn.Close();
                }
                //bila save akn gi kt managecustomer
                return RedirectToAction("ManageJobAuditTrail", "ITO");
            }
            else
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    cn.Open();
                    command.CommandText = @"SELECT Id,LogTagNo
                                    FROM [IflowSeed].[dbo].[JobAuditTrail]
                                    WHERE Id=@Id ";
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

        }



    }

    public ActionResult DeleteJobAuditTrail(string Id)
    {
        if (!string.IsNullOrEmpty(Id))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("DELETE [IflowSeed].[dbo].[JobAuditTrail] WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn.Close();
            }

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("DELETE [IflowSeed].[dbo].[JobAuditTrailDetail] WHERE JobAuditTrailId=@JobAuditTrailId", cn);
                command.Parameters.AddWithValue("@JobAuditTrailId", Id);
                command.ExecuteNonQuery();
                cn.Close();
            }
        }
        return RedirectToAction("ManageJobAuditTrail", "ITO");
    }

    public ActionResult CreateJobAuditTrail(JobAuditTrailDetail ModelSample, JobAuditTrailDetail get, string Id, string JobAuditTrailId, string set, string Set,
                                            string JobRequest, string Customer_Name, string ProductName, string JobSheetNo, string JobClass,
                                            string Frequency, string JobType, string LogTagNo, string TotalAuditTrail, string CreateByIT, string Status,
                                            string ModeLog, string Path, string JobNameIT, string JobId, string ProgramId, string FileId,
                                            string RevStrtDateOn, string RevStrtTime, string DateProcessItOn, string TimeProcessIt,
                                            string DateApproveOn, string DateApproveTime, string AccountsQty, string ImpressionQty, string PagesQty,
                                            string FirstRecord, string LastRecord, string Remarks, string Type,
                                            string NotesByIT, string NotesByProduction, string NotesByPurchasing,
                                            string NotesByEngineering, string NotesByArtwork, string NotesByFinance, string NotesByDCP, string JobInstructionId,
                                            string ImageInDateOn, string ImageInTime, string RevisedInDateOn,
                                            string AccQty, string ImpQty, string PageQty)

    {
        var IdentityName = @Session["Fullname"];
        Session["Id"] = Id;
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.IsRole = @Session["Role"];
        string Deptment = @Session["Department"].ToString();
        ViewBag.Id = Id;
        ViewBag.JobAuditTrailId = JobAuditTrailId;
        ViewBag.LogTagNo = LogTagNo;




        List<SelectListItem> li = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Mode FROM [IflowSeed].[dbo].[ModeLog]";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobAuditTrailDetail model = new JobAuditTrailDetail();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.ModeLog = reader.GetString(0);
                    }
                }
                int i = _bil++;
                if (i == 1)
                {
                    li.Add(new SelectListItem { Text = "Please Select" });

                }
                li.Add(new SelectListItem { Text = model.ModeLog });
            }
            cn.Close();
        }
        ViewData["ModeLog_"] = li;


        List<SelectListItem> li2 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Type FROM [IflowSeed].[dbo].[TypeList] ";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobAuditTrailDetail model = new JobAuditTrailDetail();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.Type = reader.GetString(0);
                    }
                }
                int i2 = _bil++;
                if (i2 == 1)
                {
                    li2.Add(new SelectListItem { Text = "Please Select" });

                }
                li2.Add(new SelectListItem { Text = model.Type });
            }
            cn.Close();
        }
        ViewData["Type_"] = li2;

        if (set == "AddNew")
        {
            List<JobAuditTrailDetail> viewJobAuditTrailDetail = new List<JobAuditTrailDetail>();

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Id,JobRequest, Customer_Name, ProductName, JobClass, 
                                               Frequency, JobType, LogTagNo, 
                                               NotesByIT, NotesByProduction, NotesByPurchasing, NotesByEngineering, 
                                               NotesByArtwork,NotesByFinance,NotesByDCP, JobInstructionId,
                                               AccountsQty,ImpressionQty,PagesQty
                                               FROM [IflowSeed].[dbo].[JobAuditTrail] 
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
                            model.Id = reader.GetGuid(0);
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            model.JobRequest = reader.GetDateTime(1);
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
                            model.JobClass = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            model.Frequency = reader.GetString(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.JobType = reader.GetString(6);
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.LogTagNo = reader.GetString(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.NotesByIT = reader.GetString(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            model.NotesByProduction = reader.GetString(9);
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            model.NotesByPurchasing = reader.GetString(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            model.NotesByEngineering = reader.GetString(11);
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            model.NotesByArtwork = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            model.NotesByFinance = reader.GetString(13);
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            model.NotesByDCP = reader.GetString(14);
                        }
                        if (reader.IsDBNull(15) == false)
                        {
                            model.JobInstructionId = reader.GetGuid(15);
                        }
                        if (reader.IsDBNull(16) == false)
                        {
                            model.AccountsQty = reader.GetString(16);
                        }
                        if (reader.IsDBNull(17) == false)
                        {
                            model.ImpressionQty = reader.GetString(17);
                        }
                        if (reader.IsDBNull(18) == false)
                        {
                            model.PagesQty = reader.GetString(18);
                        }

                    }
                    viewJobAuditTrailDetail.Add(model);


                    using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {

                        Guid Idx = Guid.NewGuid();
                        get.JobRequest = Convert.ToDateTime(get.JobRequestTxt);

                        cn1.Open();
                        SqlCommand command1;
                        command1 = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[JobAuditTrailDetail](Id, JobRequest, Customer_Name, ProductName, JobClass, Frequency, JobType, LogTagNo, NotesByIT, NotesByProduction, NotesByPurchasing, NotesByEngineering, NotesByArtwork, NotesByFinance, NotesByDCP, JobInstructionId, JobAuditTrailId, AccountsQty, ImpressionQty, PagesQty) values (@Id, @JobRequest, @Customer_Name, @ProductName, @JobClass, @Frequency, @JobType, @LogTagNo, @NotesByIT, @NotesByProduction, @NotesByPurchasing, @NotesByEngineering, @NotesByArtwork, @NotesByFinance, @NotesByDCP, @JobInstructionId, @JobAuditTrailId, @AccountsQty, @ImpressionQty, @PagesQty)", cn1);
                        command1.Parameters.AddWithValue("@Id", Idx);
                        if (model.JobRequest != null)
                        {
                            command1.Parameters.AddWithValue("@JobRequest", model.JobRequest);
                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@JobRequest", DBNull.Value);
                        }
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
                        if (model.JobClass != null)
                        {
                            command1.Parameters.AddWithValue("@JobClass", model.JobClass);
                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@JobClass", DBNull.Value);
                        }
                        if (model.Frequency != null)
                        {
                            command1.Parameters.AddWithValue("@Frequency", model.Frequency);
                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@Frequency", DBNull.Value);

                        }
                        if (model.JobType != null)
                        {
                            command1.Parameters.AddWithValue("@JobType", model.JobType);
                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@JobType", DBNull.Value);
                        }
                        if (model.LogTagNo != null)
                        {
                            command1.Parameters.AddWithValue("@LogTagNo", model.LogTagNo);
                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@LogTagNo", DBNull.Value);
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
                        if (model.JobInstructionId != null)
                        {
                            command1.Parameters.AddWithValue("@JobInstructionId", model.JobInstructionId);
                        }
                        else
                        {
                            command1.Parameters.AddWithValue("@JobInstructionId", DBNull.Value);

                        }
                        command1.Parameters.AddWithValue("@JobAuditTrailId", Id);
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
                        command1.ExecuteNonQuery();
                        cn1.Close();

                        return RedirectToAction("CreateJobAuditTrail", "ITO", new { Id = Idx.ToString() });

                    }

                }
                cn.Close();
            }
        }
        else
        {

        }





        if (set == "CreateAuditTrail")
        {

        }

        else if (set == "Attachment")
        {

        }


        else if (set == "ImportantNotes")
        {


        }
        else if (Set == "save")
        {


            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                get.RevStrtDateOn = Convert.ToDateTime(get.RevStrtDateOnTxt);
                get.DateProcessItOn = Convert.ToDateTime(get.DateProcessItOnTxt);
                get.DateApproveOn = Convert.ToDateTime(get.DateApproveOnTxt);
                get.ImageInDateOn = Convert.ToDateTime(get.ImageInDateOnTxt);
                get.RevisedInDateOn = Convert.ToDateTime(get.RevisedInDateOnTxt);


                cn2.Open();
                SqlCommand command;
                command = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobAuditTrailDetail] SET CreatedOn=@CreatedOn,ModeLog=@ModeLog,Path=@Path,JobNameIT=@JobNameIT,JobId=@JobId,ProgramId=@ProgramId,FileId=@FileId,RevStrtDateOn=@RevStrtDateOn,RevStrtTime=@RevStrtTime,DateProcessItOn=@DateProcessItOn,TimeProcessIt=@TimeProcessIt,DateApproveOn=@DateApproveOn,DateApproveTime=@DateApproveTime,AccQty=@AccQty,ImpQty=@ImpQty,PageQty=@PageQty,FirstRecord=@FirstRecord,LastRecord=@LastRecord,Remarks=@Remarks,Type=@Type,Status=@Status,CreateByIt=@CreateByIt,ImageInDateOn=@ImageInDateOn,ImageInTime=@ImageInTime,RevisedInDateOn=@RevisedInDateOn WHERE Id=@Id", cn2);
                command.Parameters.AddWithValue("@CreatedOn", createdOn);
                command.Parameters.AddWithValue("@ModeLog", ModeLog);
                command.Parameters.AddWithValue("@Path", Path);
                command.Parameters.AddWithValue("@JobNameIT", JobNameIT);
                command.Parameters.AddWithValue("@JobId", JobId);
                command.Parameters.AddWithValue("@ProgramId", ProgramId);
                command.Parameters.AddWithValue("@FileId", FileId);
                if (!string.IsNullOrEmpty(RevStrtDateOn))
                {

                    string ffff = Convert.ToDateTime(RevStrtDateOn).ToString("yyyy-MM-dd");
                    command.Parameters.AddWithValue("@RevStrtDateOn", ffff);

                }
                else
                {
                    command.Parameters.AddWithValue("@RevStrtDateOn", null);
                }
                command.Parameters.AddWithValue("@RevStrtTime", RevStrtTime);
                if (!string.IsNullOrEmpty(DateProcessItOn))
                {

                    string gggg = Convert.ToDateTime(DateProcessItOn).ToString("yyyy-MM-dd");
                    command.Parameters.AddWithValue("@DateProcessItOn", gggg);
                }
                else
                {
                    command.Parameters.AddWithValue("@DateProcessItOn", null);
                }
                command.Parameters.AddWithValue("@TimeProcessIt", TimeProcessIt);
                if (!string.IsNullOrEmpty(DateApproveOn))
                {
                    string hhhh = Convert.ToDateTime(DateApproveOn).ToString("yyyy-MM-dd");
                    command.Parameters.AddWithValue("@DateApproveOn", hhhh);
                }
                else
                {
                    command.Parameters.AddWithValue("@DateApproveOn", null);
                }
                command.Parameters.AddWithValue("@DateApproveTime", DateApproveTime);
                command.Parameters.AddWithValue("@AccQty", AccQty);
                command.Parameters.AddWithValue("@ImpQty", ImpQty);
                command.Parameters.AddWithValue("@PageQty", PageQty);
                command.Parameters.AddWithValue("@FirstRecord", FirstRecord);
                command.Parameters.AddWithValue("@LastRecord", LastRecord);
                command.Parameters.AddWithValue("@Remarks", Remarks);
                command.Parameters.AddWithValue("@Type", Type);
                command.Parameters.AddWithValue("@Status", "Waiting Approval");
                command.Parameters.AddWithValue("@CreateByIt", IdentityName.ToString());
                if (!string.IsNullOrEmpty(ImageInDateOn))
                {

                    string iiii = Convert.ToDateTime(ImageInDateOn).ToString("yyyy-MM-dd");
                    command.Parameters.AddWithValue("@ImageInDateOn", iiii);
                }
                else
                {
                    command.Parameters.AddWithValue("@ImageInDateOn", null);
                }
                command.Parameters.AddWithValue("@ImageInTime", ImageInTime);
                if (!string.IsNullOrEmpty(RevisedInDateOn))
                {
                    string jjjj = Convert.ToDateTime(RevisedInDateOn).ToString("yyyy-MM-dd");
                    command.Parameters.AddWithValue("@RevisedInDateOn", jjjj);
                }
                else
                {
                    command.Parameters.AddWithValue("@RevisedInDateOn", null);
                }
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn2.Close();

            }

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand comm = new SqlCommand("SELECT COUNT(*) FROM [IflowSeed].[dbo].[JobAuditTrailDetail] WHERE JobAuditTrailId=@JobAuditTrailId AND Status='Waiting Approval' ", cn);
                comm.Parameters.AddWithValue("@JobAuditTrailId", JobAuditTrailId);
                Int32 count = (Int32)comm.ExecuteScalar();
                string TotalAT = count.ToString();

                using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn1.Open();
                    SqlCommand comm1 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobAuditTrail] SET TotalAuditTrail=@TotalAuditTrail, Status=@Status WHERE Id=@Id", cn1);
                    comm1.Parameters.AddWithValue("@TotalAuditTrail", TotalAT);
                    comm1.Parameters.AddWithValue("@Status", "Waiting Approval");
                    comm1.Parameters.AddWithValue("@Id", JobAuditTrailId);
                    comm1.ExecuteNonQuery();
                    cn1.Close();
                }

                //using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                //{
                //    cn1.Open();
                //    SqlCommand comm1 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET TotalAuditTrail=@TotalAuditTrail, Status=@Status WHERE Id=@Id", cn1);
                //    comm1.Parameters.AddWithValue("@TotalAuditTrail", TotalAT);
                //    comm1.Parameters.AddWithValue("@Status", "Waiting Approval");
                //    comm1.Parameters.AddWithValue("@Id", JobAuditTrailId);
                //    comm1.ExecuteNonQuery();
                //    cn1.Close();
                //}

                cn.Close();
            }



            return RedirectToAction("CreateLogTagNo", "ITO", new { Id = JobAuditTrailId.ToString() });


        }


        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT Id,ModeLog,Path,JobNameIT,JobId,ProgramId,FileId,RevStrtDateOn,RevStrtTime,
                                           DateProcessItOn,TimeProcessIt,DateApproveOn,DateApproveTime,AccQty,
                                           ImpQty,PageQty,FirstRecord,LastRecord,Remarks,Type,LogTagNo,
                                           NotesByIT, NotesByProduction, NotesByPurchasing,
                                           NotesByEngineering, NotesByArtwork, NotesByFinance, NotesByDCP,JobAuditTrailId,
                                           ImageInDateOn,ImageInTime,RevisedInDateOn
                                    FROM [IflowSeed].[dbo].[JobAuditTrailDetail]
                                    WHERE Id=@JobAuditTrailId ";
            command.Parameters.AddWithValue("@JobAuditTrailId", Id);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader.IsDBNull(0) == false)
                {
                    ViewBag.Id = reader.GetGuid(0);
                }
                if (reader.IsDBNull(1) == false)
                {
                    ViewBag.ModeLog = reader.GetString(1);
                }
                if (reader.IsDBNull(2) == false)
                {
                    ViewBag.Path = reader.GetString(2);
                }
                if (reader.IsDBNull(3) == false)
                {
                    ViewBag.JobNameIT = reader.GetString(3);
                }
                if (reader.IsDBNull(4) == false)
                {
                    ViewBag.JobId = reader.GetString(4);
                }
                if (reader.IsDBNull(5) == false)
                {
                    ViewBag.ProgramId = reader.GetString(5);
                }
                if (reader.IsDBNull(6) == false)
                {
                    ViewBag.FileId = reader.GetString(6);
                }
                if (reader.IsDBNull(7) == false)
                {
                    ViewBag.RevStrtDateOn = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(7));
                }
                if (reader.IsDBNull(8) == false)
                {
                    ViewBag.RevStrtTime = reader.GetString(8);
                }
                if (reader.IsDBNull(9) == false)
                {
                    ViewBag.DateProcessItOn = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(9));
                }
                if (reader.IsDBNull(10) == false)
                {
                    ViewBag.TimeProcessIt = reader.GetString(10);
                }
                if (reader.IsDBNull(11) == false)
                {
                    ViewBag.DateApproveOn = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(11));
                }
                if (reader.IsDBNull(12) == false)
                {
                    ViewBag.DateApproveTime = reader.GetString(12);
                }
                if (reader.IsDBNull(13) == false)
                {
                    ViewBag.AccQty = reader.GetString(13);
                }
                if (reader.IsDBNull(14) == false)
                {
                    ViewBag.ImpQty = reader.GetString(14);
                }
                if (reader.IsDBNull(15) == false)
                {
                    ViewBag.PageQty = reader.GetString(15);
                }
                if (reader.IsDBNull(16) == false)
                {
                    ViewBag.FirstRecord = reader.GetString(16);
                }
                if (reader.IsDBNull(17) == false)
                {
                    ViewBag.LastRecord = reader.GetString(17);
                }
                if (reader.IsDBNull(18) == false)
                {
                    ViewBag.Remarks = reader.GetString(18);
                }
                if (reader.IsDBNull(19) == false)
                {
                    ViewBag.Type = reader.GetString(19);
                }
                if (reader.IsDBNull(20) == false)
                {
                    ViewBag.LogTagNo = reader.GetString(20);
                }
                if (reader.IsDBNull(21) == false)
                {
                    ViewBag.NotesByIT = reader.GetString(21);
                }
                if (reader.IsDBNull(22) == false)
                {
                    ViewBag.NotesByProduction = reader.GetString(22);
                }
                if (reader.IsDBNull(23) == false)
                {
                    ViewBag.NotesByPurchasing = reader.GetString(23);
                }
                if (reader.IsDBNull(24) == false)
                {
                    ViewBag.NotesByEngineering = reader.GetString(24);
                }
                if (reader.IsDBNull(25) == false)
                {
                    ViewBag.NotesByArtwork = reader.GetString(25);
                }
                if (reader.IsDBNull(26) == false)
                {
                    ViewBag.NotesByFinance = reader.GetString(26);
                }
                if (reader.IsDBNull(27) == false)
                {
                    ViewBag.NotesByDCP = reader.GetString(27);
                }
                if (reader.IsDBNull(28) == false)
                {
                    ViewBag.JobAuditTrailId = reader.GetGuid(28);
                }
                if (reader.IsDBNull(29) == false)
                {
                    ViewBag.ImageInDateOn = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(29));
                }
                if (reader.IsDBNull(30) == false)
                {
                    ViewBag.ImageInTime = reader.GetString(30);
                }
                if (reader.IsDBNull(31) == false)
                {
                    ViewBag.RevisedInDateOn = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(31));
                }
            }
            cn.Close();
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
                                           ImpQty,PageQty,FirstRecord,LastRecord,Remarks,Type,JobAuditTrailId
                                      FROM [IflowSeed].[dbo].[JobAuditTrailDetail]  
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
                                          FROM [IflowSeed].[dbo].[JobAuditTrailDetail]
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
                            command3 = new SqlCommand("DELETE [IflowSeed].[dbo].[JobAuditTrailDetail] WHERE Id=@Id", cn3);
                            command3.Parameters.AddWithValue("@Id", Id);
                            command3.ExecuteNonQuery();
                            cn3.Close();

                        }

                        using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            cn2.Open();
                            SqlCommand comm = new SqlCommand("SELECT COUNT(*) FROM [IflowSeed].[dbo].[JobAuditTrailDetail] WHERE JobAuditTrailId=@JobAuditTrailId", cn2);
                            comm.Parameters.AddWithValue("@JobAuditTrailId", Session["Id"].ToString());
                            Int32 count = (Int32)comm.ExecuteScalar();
                            string TotalAT = count.ToString();

                            using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                            {
                                cn1.Open();
                                SqlCommand comm1 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobAuditTrail] SET TotalAuditTrail=@TotalAuditTrail WHERE Id=@Id", cn1);
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
    public ActionResult SubmitJAT(JobAuditTrail JobAuditTrail, JobAuditTrail get, JobAuditTrailDetail JobAuditTrailDetail, string set,
                                  string Id, string JobAuditTrailId, string AuditTrail, string LogTagNo, string JobRequest,
                                  string Customer_Name, string ProductName, string JobSheetNo, string JobClass, string Frequency,
                                  string JobType, string AccountsQty, string ImpressionQty, string PagesQty, string TotalAuditTrail,
                                  string Status, string CreateByIT, string FileId, string JobInstructionId, string PlanDatePostOn, string ItSubmitOn)
    {
        if (Status == "Waiting Approval")
        {
            if (!string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(LogTagNo) && !string.IsNullOrEmpty(AccountsQty) && !string.IsNullOrEmpty(ImpressionQty) && !string.IsNullOrEmpty(PagesQty))
            {

                using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                    cn1.Open();
                    SqlCommand command1;
                    command1 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobAuditTrail] SET STATUS='PLANNER', ItSubmitOn=@ItSubmitOn WHERE Id=@Id", cn1);
                    command1.Parameters.AddWithValue("@ItSubmitOn", createdOn);
                    command1.Parameters.AddWithValue("@Id", Id);
                    command1.ExecuteNonQuery();
                    cn1.Close();
                }

                using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn1.Open();
                    SqlCommand command1;
                    command1 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobInstruction] SET STATUS='PLANNER' WHERE Id=@Id", cn1);
                    command1.Parameters.AddWithValue("@Id", JobInstructionId);
                    command1.ExecuteNonQuery();
                    cn1.Close();
                }

                using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    get.JobRequest = Convert.ToDateTime(get.JobRequestTxt);
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                    cn1.Open();
                    SqlCommand command1;
                    command1 = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobAuditTrailDetail] SET STATUS='PLANNER' WHERE JobAuditTrailId=@JobAuditTrailId", cn1);
                    command1.Parameters.AddWithValue("@JobAuditTrailId", Id);
                    command1.ExecuteNonQuery();
                    cn1.Close();

                    TempData["msg"] = "<script>alert('SUCCESSFULLY SUBMIT TO PLANNER !');</script>";

                    return RedirectToAction("ManageJobAuditTrail", "ITO");
                }

            }
        }

        else
        {
            TempData["msg"] = "<script>alert('PLEASE COMPLETE FORM DETAILS !');</script>";
        }

        return RedirectToAction("ManageJobAuditTrail", "ITO");
    }







    public ActionResult ReloadMedia()
    {
        List<SampleProduct> viewFileStore = new List<SampleProduct>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT Picture_FileId,Id,AuditTrail
                                      FROM [IflowSeed].[dbo].[SampleProduct]  
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

    public ActionResult UploadMedia(SampleProduct ModelSample, string AuditTrail)
    {
        var IdentityName = @Session["Fullname"];
        var Id = Session["Id"];
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.IsRole = @Session["Role"];
        string Deptment = @Session["Department"].ToString();


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
                command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[SampleProduct] (Id,CreatedOn,Picture_FileId,AuditTrail,Picture_Extension,Code,CreateBy) values (@Id,@CreatedOn,@Picture_FileId,@AuditTrail,@Picture_Extension,@Code,@CreateBy)", cn2);
                command.Parameters.AddWithValue("@Id", guidId);
                command.Parameters.AddWithValue("@CreatedOn", createdOn);
                command.Parameters.AddWithValue("@Picture_FileId", fileName.Trim());
                command.Parameters.AddWithValue("@AuditTrail", Id);
                command.Parameters.AddWithValue("@Picture_Extension", ModelSample.FileUploadFile.ContentType);
                command.Parameters.AddWithValue("@Code", "AT");
                command.Parameters.AddWithValue("@CreateBy", IdentityName.ToString());
                command.ExecuteNonQuery();
                cn2.Close();

            }

            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn2.Open();
                SqlCommand command;
                command = new SqlCommand("UPDATE [IflowSeed].[dbo].[JobAuditTrail] SET JobAuditTrailId=@JobAuditTrailId WHERE Id=@Id", cn2);
                command.Parameters.AddWithValue("@JobAuditTrailId", Id);
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn2.Close();

            }


            return RedirectToAction("CreateJobAuditTrail", "ITO", new { Id = Session["Id"].ToString() });
        }

        if (ModelSample.Set == "back")
        {
            return RedirectToAction("CreateJobAuditTrail", "ITO", new { Id = Session["Id"].ToString() });
        }

        return View();
    }

    public ActionResult DeleteMedia(string Id, string AuditTrail)
    {
        Guid SampleProductId = Guid.Empty;

        if (Id != null)
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Picture_FileId,AuditTrail
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
                        return RedirectToAction("CreateJobAuditTrail", "ITO", new { Id = Session["Id"].ToString() });
                    }
                }
                cn.Close();
            }
        }

        return RedirectToAction("CreateJobAuditTrail", "ITO", new { Id = Session["Id"].ToString() });
    }

    public ActionResult DownloadMedia(string Id)
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

        return RedirectToAction("CreateJobAuditTrail", "ITO", new { Id = Id.ToString() });
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
                                           FROM [IflowSeed].[dbo].[JobAuditTrail]
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
        using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn3))
        {
            cn3.Open();
            command.CommandText = @"SELECT Id,ModeLog,Path,JobNameIT,JobId,ProgramId,FileId,
                                    RevStrtDateOn,RevStrtTime,DateProcessItOn,TimeProcessIt,
                                    DateApproveOn,DateProcessItOn,AccQty,ImpQty,PageQty,FirstRecord,
                                    LastRecord,JobAuditTrailId,ImageInDateOn,ImageInTime,RevisedInDateOn
                                    FROM [IflowSeed].[dbo].[JobAuditTrailDetail]
                                    WHERE Id=@JobAuditTrailId";
            command.Parameters.AddWithValue("@JobAuditTrailId", Id);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader.IsDBNull(0) == false)
                {
                    ViewBag.Id = reader.GetGuid(0);
                }
                if (reader.IsDBNull(1) == false)
                {
                    ViewBag.ModeLog = reader.GetString(1);
                }
                if (reader.IsDBNull(2) == false)
                {
                    ViewBag.Path = reader.GetString(2);
                }
                if (reader.IsDBNull(3) == false)
                {
                    ViewBag.JobNameIT = reader.GetString(3);
                }
                if (reader.IsDBNull(4) == false)
                {
                    ViewBag.JobId = reader.GetString(4);
                }
                if (reader.IsDBNull(5) == false)
                {
                    ViewBag.ProgramId = reader.GetString(5);
                }
                if (reader.IsDBNull(6) == false)
                {
                    ViewBag.FileId = reader.GetString(6);
                }
                if (reader.IsDBNull(7) == false)
                {
                    ViewBag.RevStrtDateOn = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(7));
                }
                if (reader.IsDBNull(8) == false)
                {
                    ViewBag.RevStrtTime = reader.GetString(8);
                }
                if (reader.IsDBNull(9) == false)
                {
                    ViewBag.DateProcessItOn = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(9));
                }
                if (reader.IsDBNull(10) == false)
                {
                    ViewBag.TimeProcessIt = reader.GetString(10);
                }
                if (reader.IsDBNull(11) == false)
                {
                    ViewBag.DateApproveOn = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(11));
                }
                if (reader.IsDBNull(12) == false)
                {
                    ViewBag.DateApproveTime = reader.GetString(12);
                }
                if (reader.IsDBNull(13) == false)
                {
                    ViewBag.AccQty = reader.GetString(13);
                }
                if (reader.IsDBNull(14) == false)
                {
                    ViewBag.ImpQty = reader.GetString(14);
                }
                if (reader.IsDBNull(15) == false)
                {
                    ViewBag.PageQty = reader.GetString(15);
                }
                if (reader.IsDBNull(16) == false)
                {
                    ViewBag.FirstRecord = reader.GetString(16);
                }
                if (reader.IsDBNull(17) == false)
                {
                    ViewBag.LastRecord = reader.GetString(17);
                }
                if (reader.IsDBNull(18) == false)
                {
                    ViewBag.JobAuditTrailId = reader.GetGuid(18);
                }
                if (reader.IsDBNull(19) == false)
                {
                    ViewBag.ImageInDateOn = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(19));
                }
                if (reader.IsDBNull(20) == false)
                {
                    ViewBag.ImageInTime = reader.GetString(20);
                }
                if (reader.IsDBNull(21) == false)
                {
                    ViewBag.RevisedInDateOn = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(21));
                }


            }
            cn3.Close();
        }

        //call table

        List<JobAuditTrailDetail> viewJATList1 = new List<JobAuditTrailDetail>();
        using (SqlConnection cn4 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn4))
        {
            int _bil = 1;
            cn4.Open();
            command.CommandText = @"SELECT Id,ModeLog,Path,JobNameIT,JobId,ProgramId,FileId,
                                    RevStrtDateOn,RevStrtTime,DateProcessItOn,TimeProcessIt,
                                    DateApproveOn,DateProcessItOn,AccQty,ImpQty,PageQty,FirstRecord,
                                    LastRecord,JobAuditTrailId,ImageInDateOn,ImageInTime,RevisedInDateOn
                                    FROM [IflowSeed].[dbo].[JobAuditTrailDetail]
                                    WHERE Id=@JobAuditTrailId";
            command.Parameters.AddWithValue("@JobAuditTrailId", Session["Id"].ToString());
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
                        model.RevStrtDateOn = reader.GetDateTime(7);
                    }
                    if (reader.IsDBNull(8) == false)
                    {
                        model.RevStrtTime = reader.GetString(8);
                    }
                    if (reader.IsDBNull(9) == false)
                    {
                        model.DateProcessItOn = reader.GetDateTime(9);
                    }
                    if (reader.IsDBNull(10) == false)
                    {
                        model.TimeProcessIt = reader.GetString(10);
                    }
                    if (reader.IsDBNull(11) == false)
                    {
                        model.DateApproveOn = reader.GetDateTime(1);
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
                        model.JobAuditTrailId = reader.GetGuid(18);
                    }
                    if (reader.IsDBNull(19) == false)
                    {
                        model.ImageInDateOn = reader.GetDateTime(19);
                    }
                    if (reader.IsDBNull(20) == false)
                    {
                        model.ImageInTime = reader.GetString(20);
                    }
                    if (reader.IsDBNull(21) == false)
                    {
                        model.RevisedInDateOn = reader.GetDateTime(21);
                    }

                }
                viewJATList1.Add(model);
            }
            cn4.Close();

        }

        //-----------------------------------------

        ReloadJATList(Id);

        //ReloadJAT(Id);

        return new Rotativa.ViewAsPdf("ViewJAT", viewJATList)
        {
            // FileName = flightPlan.ListingItemDetailsModel.FlightDetails + ".pdf",
            PageMargins = new Rotativa.Options.Margins(5, 5, 5, 5),
            PageOrientation = Rotativa.Options.Orientation.Portrait,
            //PageWidth = 210,
            //PageHeight = 297
        };
    }

    List<JobAuditTrail> viewJATList = new List<JobAuditTrail>();
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
                                           FROM [IflowSeed].[dbo].[JobAuditTrailDetail]a, [IflowSeed].[dbo].[JobAuditTrail]b
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


    //private void ReloadJAT(string Id)
    //{
    //    int _bil = 1;
    //    List<SelectListItem> li = new List<SelectListItem>();
    //    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
    //    using (SqlCommand command = new SqlCommand("", cn))
    //    {
    //        cn.Open();
    //        command.CommandText = @"SELECT DISTINCT Contact_Person FROM [IflowSeed].[dbo].[CustomerDetails]          
    //                                 WHERE Customer_Name = @Customer_Name";
    //        command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
    //        var reader = command.ExecuteReader();
    //        while (reader.Read())
    //        {
    //            JobInstruction model = new JobInstruction();
    //            {
    //                if (reader.IsDBNull(0) == false)
    //                {
    //                    model.ContactPerson = reader.GetString(0);
    //                }
    //            }
    //            int i = _bil++;
    //            if (i == 1)
    //            {
    //                li.Add(new SelectListItem { Text = "Please Select" });
    //                li.Add(new SelectListItem { Text = model.ContactPerson });
    //            }
    //            else
    //            {
    //                li.Add(new SelectListItem { Text = model.ContactPerson });
    //            }
    //        }
    //        cn.Close();
    //    }
    //    ViewData["ContactPerson_"] = li;
    //}


}
