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

[MvcAppV2.FilterConfig.UserSessionActionFilter]
public class RequestFormController : Controller
{
    List<ManagementITform> ManagementITformlist = new List<ManagementITform>();

    public string StartDevDateTxt { get; private set; }
    public string EndDevDateTxt { get; private set; }
    public string ImplementationDateOnTxt { get; private set; }
    public string BeforeDateOnTxt { get; private set; }
    public string AfterDateOnTxt { get; private set; }
    public string TargetImpDateOnTxt { get; private set; }
    public string DateRequestOn { get; private set; }
    public string EstimateDateTxt { get; private set; }

    public ActionResult ManageITform(string Id, string ProductName, string set)
    {
        if (set == "search") //ini kalu user search product
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Id,BatchNo,ProductName,RequestType,RequestName,
                                           Department,ContactNo,ModifiedOn,Company,UnitSystem,Type,
                                           ChangeRequired,ChangeType,IncidentNo,ReasonforChange,Dependencies,
                                           BeforeDateOn,AfterDateOn,TargetImpDateOn,Duration,AfterModificationRemark,
                                           Task,ImplementationDateOn,Deliveables,Who,AnyProcedure,AnyProcedure1,SpecifyTheName,
                                           TimeImplementation,Status,ProgrammerBy,AssignByLeader,UnderLevelDev,
                                           StartAssignDevDate,EndAssignDevDate,StartDevDate,EndDevDate,DevelopCompleteDate.
                                           RequestEmail,Remark
                                     FROM [IflowSeed].[dbo].[ManagementITform]
                                     WHERE ProductName LIKE @ProductName
                                     ORDER BY CreatedOn desc";
                command.Parameters.AddWithValue("@ProductName", "%" + ProductName + "%");
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ManagementITform model = new ManagementITform();
                    {
                        model.Bil = _bil++;
                        if (reader.IsDBNull(0) == false)
                        {
                            model.Id = reader.GetGuid(0);
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            model.BatchNo = reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            model.ProductName = reader.GetString(2);
                        }
                        if (reader.IsDBNull(3) == false)
                        {
                            model.RequestType = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            model.RequestName = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            model.Department = reader.GetString(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.ContactNo = reader.GetString(6);
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.ModifiedOn = reader.GetDateTime(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.Company = reader.GetString(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            model.UnitSystem = reader.GetString(9);
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            model.Type = reader.GetString(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            model.ChangeRequired = reader.GetString(11);
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            model.ChangeType = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            model.IncidentNo = reader.GetString(13);
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            model.ReasonforChange = reader.GetString(14);
                        }
                        if (reader.IsDBNull(15) == false)
                        {
                            model.Dependencies = reader.GetString(15);
                        }
                        if (reader.IsDBNull(16) == false)
                        {
                            model.BeforeDateOn = reader.GetDateTime(16);
                        }
                        if (reader.IsDBNull(17) == false)
                        {
                            model.AfterDateOn = reader.GetDateTime(17);
                        }
                        if (reader.IsDBNull(18) == false)
                        {
                            model.TargetImpDateOn = reader.GetDateTime(18);
                        }
                        if (reader.IsDBNull(19) == false)
                        {
                            model.Duration = reader.GetString(19);
                        }
                        if (reader.IsDBNull(20) == false)
                        {
                            model.AfterModificationRemark = reader.GetString(2021);
                        }
                        if (reader.IsDBNull(21) == false)
                        {
                            model.Task = reader.GetString(21);
                        }
                        if (reader.IsDBNull(22) == false)
                        {
                            model.ImplementationDateOn = reader.GetDateTime(22);
                        }
                        if (reader.IsDBNull(23) == false)
                        {
                            model.Deliveables = reader.GetString(23);
                        }
                        if (reader.IsDBNull(24) == false)
                        {
                            model.Who = reader.GetString(24);
                        }
                        if (reader.IsDBNull(25) == false)
                        {
                            model.AnyProcedure = reader.GetBoolean(25);
                        }
                        if (reader.IsDBNull(26) == false)
                        {
                            model.AnyProcedure1 = reader.GetBoolean(26);
                        }
                        if (reader.IsDBNull(27) == false)
                        {
                            model.SpecifyTheName = reader.GetString(27);
                        }
                        if (reader.IsDBNull(28) == false)
                        {
                            model.TimeImplementation = reader.GetString(28);
                        }
                        if (reader.IsDBNull(29) == false)
                        {
                            model.Status = reader.GetString(29);
                        }
                        if (reader.IsDBNull(30) == false)
                        {
                            model.ProgrammerBy = reader.GetString(30);
                        }
                        if (reader.IsDBNull(31) == false)
                        {
                            model.AssignByLeader = reader.GetString(31);
                        }
                        if (reader.IsDBNull(32) == false)
                        {
                            model.UnderLevelDev = reader.GetString(32);
                        }
                        if (reader.IsDBNull(33) == false)
                        {
                            model.StartAssignDevDate = reader.GetDateTime(33);
                        }
                        if (reader.IsDBNull(34) == false)
                        {
                            model.EndAssignDevDate = reader.GetDateTime(34);
                        }
                        if (reader.IsDBNull(35) == false)
                        {
                            model.StartDevDateTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(35));
                        }
                        if (reader.IsDBNull(36) == false)
                        {
                            model.EndDevDateTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(36));
                        }
                        if (reader.IsDBNull(37) == false)
                        {
                            model.DevelopCompleteDate = reader.GetDateTime(37);
                        }
                        if (reader.IsDBNull(38) == false)
                        {
                            model.RequestEmail = reader.GetString(38);
                        }
                        if (reader.IsDBNull(39) == false)
                        {
                            model.Remark = reader.GetString(39);
                        }

                    }
                    ManagementITformlist.Add(model);
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
                command.CommandText = @"SELECT Id,BatchNo,ProductName,RequestType,RequestName,
                                           Department,ContactNo,ModifiedOn,Company,UnitSystem,Type,
                                           ChangeRequired,ChangeType,IncidentNo,ReasonforChange,Dependencies,
                                           BeforeDateOn,AfterDateOn,TargetImpDateOn,Duration,AfterModificationRemark,
                                           Task,ImplementationDateOn,Deliveables,Who,AnyProcedure,AnyProcedure1,SpecifyTheName,
                                           TimeImplementation,Status,ProgrammerBy,AssignByLeader,UnderLevelDev,
                                           StartAssignDevDate,EndAssignDevDate,StartDevDate,EndDevDate,DevelopCompleteDate,
                                           RequestEmail,Remark
                                     FROM [IflowSeed].[dbo].[ManagementITform]
                                     WHERE ProductName LIKE @ProductName
                                     ORDER BY CreatedOn desc";
                command.Parameters.AddWithValue("@ProductName", "%" + ProductName + "%");
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ManagementITform model = new ManagementITform();
                    {
                        model.Bil = _bil++;
                        if (reader.IsDBNull(0) == false)
                        {
                            model.Id = reader.GetGuid(0);
                        }                      
                        if (reader.IsDBNull(1) == false)
                        {
                            model.BatchNo = reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            model.ProductName = reader.GetString(2);
                        }
                        if (reader.IsDBNull(3) == false)
                        {
                            model.RequestType = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            model.RequestName = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            model.Department = reader.GetString(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.ContactNo = reader.GetString(6);
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.ModifiedOn = reader.GetDateTime(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.Company = reader.GetString(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            model.UnitSystem = reader.GetString(9);
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            model.Type = reader.GetString(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            model.ChangeRequired = reader.GetString(11);
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            model.ChangeType = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            model.IncidentNo = reader.GetString(13);
                        }
                        if (reader.IsDBNull(14) == false)
                        {
                            model.ReasonforChange = reader.GetString(14);
                        }
                        if (reader.IsDBNull(15) == false)
                        {
                            model.Dependencies = reader.GetString(15);
                        }
                        if (reader.IsDBNull(16) == false)
                        {
                            model.BeforeDateOn = reader.GetDateTime(16);
                        }
                        if (reader.IsDBNull(17) == false)
                        {
                            model.AfterDateOn = reader.GetDateTime(17);
                        }
                        if (reader.IsDBNull(18) == false)
                        {
                            model.TargetImpDateOn = reader.GetDateTime(18);
                        }
                        if (reader.IsDBNull(19) == false)
                        {
                            model.Duration = reader.GetString(19);
                        }
                        if (reader.IsDBNull(20) == false)
                        {
                            model.AfterModificationRemark = reader.GetString(20);
                        }
                        if (reader.IsDBNull(21) == false)
                        {
                            model.Task = reader.GetString(21);
                        }
                        if (reader.IsDBNull(22) == false)
                        {
                            model.ImplementationDateOn = reader.GetDateTime(22);
                        }
                        if (reader.IsDBNull(23) == false)
                        {
                            model.Deliveables = reader.GetString(23);
                        }
                        if (reader.IsDBNull(24) == false)
                        {
                            model.Who = reader.GetString(24);
                        }
                        if (reader.IsDBNull(25) == false)
                        {
                            model.AnyProcedure = reader.GetBoolean(25);
                        }
                        if (reader.IsDBNull(26) == false)
                        {
                            model.AnyProcedure1 = reader.GetBoolean(26);
                        }
                        if (reader.IsDBNull(27) == false)
                        {
                            model.SpecifyTheName = reader.GetString(27);
                        }
                        if (reader.IsDBNull(28) == false)
                        {
                            model.TimeImplementation = reader.GetString(28);
                        }
                        if (reader.IsDBNull(29) == false)
                        {
                            model.Status = reader.GetString(29);
                        }
                        if (reader.IsDBNull(30) == false)
                        {
                            model.ProgrammerBy = reader.GetString(30);
                        }
                        if (reader.IsDBNull(31) == false)
                        {
                            model.AssignByLeader = reader.GetString(31);
                        }
                        if (reader.IsDBNull(32) == false)
                        {
                            model.UnderLevelDev = reader.GetString(32);
                        }
                        if (reader.IsDBNull(33) == false)
                        {
                            model.StartAssignDevDate = reader.GetDateTime(33);
                        }
                        if (reader.IsDBNull(34) == false)
                        {
                            model.EndAssignDevDate = reader.GetDateTime(34);
                        }
                        if (reader.IsDBNull(35) == false)
                        {
                            model.StartDevDateTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(35));
                        }
                        if (reader.IsDBNull(36) == false)
                        {
                            model.EndDevDateTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(36));
                        }
                        if (reader.IsDBNull(37) == false)
                        {
                            model.DevelopCompleteDate = reader.GetDateTime(37);
                        }
                        if (reader.IsDBNull(38) == false)
                        {
                            model.RequestEmail = reader.GetString(38);
                        }
                        if (reader.IsDBNull(39) == false)
                        {
                            model.Remark = reader.GetString(39);
                        }

                    }
                    ManagementITformlist.Add(model);
                }
                cn.Close();
            }
        }

        return View(ManagementITformlist); //hntr data ke ui
    }

    public ActionResult CreateBatchManagementITform(string Id, string Customer_Name, string ProductName, string BatchNo, string Status, string set)
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
                command.CommandText = @"SELECT ProductName FROM [IflowSeed].[dbo].[CustomerProduct]    
                                     WHERE Customer_Name=@Customer_Name                            
                                     ORDER BY ProductName";
                command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CustomerProduct model = new CustomerProduct();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            model.ProductName = reader.GetString(0);
                        }
                    }
                    int i = _bil2++;
                    if (i == 1)
                    {
                        li2.Add(new SelectListItem { Text = "Please Select" });
                    }
                    li2.Add(new SelectListItem { Text = model.ProductName });
                }
                cn.Close();
            }
            ViewData["Product_"] = li2;
        }
        else
        {
            List<SelectListItem> li2 = new List<SelectListItem>();
            li2.Add(new SelectListItem { Text = "Please Select" });
            ViewData["Product_"] = li2;
        }



        if (string.IsNullOrEmpty(Id) && Customer_Name != "Please Select" && ProductName != "Please Select" && !string.IsNullOrEmpty(Customer_Name) && !string.IsNullOrEmpty(ProductName))
        {
            var No_ = new NoCounterModel();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                Guid Idx = Guid.NewGuid();
                string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                cn.Open();
                SqlCommand command;
                command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[ManagementITform] (Id, CreatedOn, ProductName, BatchNo, RequestName, Status) values (@Id, @CreatedOn,@ProductName,@BatchNo,@RequestName,@Status)", cn);
                command.Parameters.AddWithValue("@Id", Idx);
                command.Parameters.AddWithValue("@CreatedOn", createdOn);
                command.Parameters.AddWithValue("@ProductName", ProductName);
                command.Parameters.AddWithValue("@BatchNo", No_.RefNo);
                command.Parameters.AddWithValue("@RequestName", IdentityName.ToString());
                command.Parameters.AddWithValue("@Status", "New");
                command.ExecuteNonQuery();
                cn.Close();
            }
            //bila save akn gi kt managecustomer
            return RedirectToAction("ManageITform", "RequestForm");
        }

        return View();
    }


    public ActionResult DeleteBatchManagementITform(string Id)
    {
        if (!string.IsNullOrEmpty(Id))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("DELETE [IflowSeed].[dbo].[ManagementITform] WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn.Close();
            }
        }
        return RedirectToAction("ManageITform", "RequestForm");
    }

    [ValidateInput(false)]
    public ActionResult CreateManagementForm(string Id, string set,string BatchNo, string ProductName, string RequestType, string RequestName,
                                    string Department, string ContactNo, string DateRequestOn, string Company, string UnitSystem, string Type,
                                    string ChangeRequired, string ChangeType, string IncidentNo, string ReasonforChange, string Dependencies,
                                    string BeforeDateOnTxt, string AfterDateOnTxt, string TargetImpDateOnTxt, string Duration, string AfterModificationRemark,
                                    string Task, string ImplementationDateOnTxt, string Deliveables, string Who, string AnyProcedure, string AnyProcedure1, string SpecifyTheName,
                                    string TimeImplementation, string Status, string ProgrammerBy, string AssignByLeader, string UnderLevelDev,
                                    string StartAssignDevDateTxt, string EndAssignDevDateTxt, string StartDevDateTxt, string EndDevDateTxt, string DevelopCompleteDateTxt,
                                    string RequestEmail, string Remark)
    {
        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];
        ViewBag.AccountManager = IdentityName.ToString();
        ViewBag.IsDepart = @Session["Department"];
        Session["Id"] = Id;

        List<SelectListItem> listRequestType = new List<SelectListItem>();

        listRequestType.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listRequestType.Add(new SelectListItem { Text = "NORMAL REQUEST", Value = "NORMAL REQUEST" });
        listRequestType.Add(new SelectListItem { Text = "EMERGENCY REQUEST", Value = "EMERGENCY REQUEST" });
        ViewData["RequestType_"] = listRequestType;

        List<SelectListItem> listDepartment = new List<SelectListItem>();

        listDepartment.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listDepartment.Add(new SelectListItem { Text = "OPERATIONS", Value = "OPERATIONS" });
        listDepartment.Add(new SelectListItem { Text = "QA", Value = "QA" });
        listDepartment.Add(new SelectListItem { Text = "BSS", Value = "BSS" });
        ViewData["Department_"] = listDepartment;

        List<SelectListItem> listType = new List<SelectListItem>();

        listType.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listType.Add(new SelectListItem { Text = "HARDWARE", Value = "HARDWARE" });
        listType.Add(new SelectListItem { Text = "INFRASTRUCTURE", Value = "INFRASTRUCTURE" });
        listType.Add(new SelectListItem { Text = "APPLICATION", Value = "APPLICATION" });
        listType.Add(new SelectListItem { Text = "SOFTWARE", Value = "SOFTWARE" });
        listType.Add(new SelectListItem { Text = "NETWORK", Value = "NETWORK" });
        listType.Add(new SelectListItem { Text = "ENVIRONMENT", Value = "ENVIRONMENT" });

        ViewData["Type_"] = listType;

        List<SelectListItem> listChangeType = new List<SelectListItem>();

        listChangeType.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listChangeType.Add(new SelectListItem { Text = "NEW REQUIREMENT", Value = "NEW REQUIREMENT" });
        listChangeType.Add(new SelectListItem { Text = "CUSTOMIZATION", Value = "CUSTOMIZATION" });
        listChangeType.Add(new SelectListItem { Text = "MAINTENANCE", Value = "MAINTENANCE" });
        ViewData["ChangeType_"] = listChangeType;

        List<SelectListItem> listDependencies = new List<SelectListItem>();

        listDependencies.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listDependencies.Add(new SelectListItem { Text = "PRE-REQUISITE", Value = "PRE-REQUISITE" });
        listDependencies.Add(new SelectListItem { Text = "CO-REQUISITE", Value = "CO-REQUISITE" });
        ViewData["Dependencies_"] = listDependencies;


        if (set == "SectionA")
        {
            if (!string.IsNullOrEmpty(Id) && RequestType != "Please Select" && Department != "Please Select" && !string.IsNullOrEmpty(RequestType) && !string.IsNullOrEmpty(RequestName) && !string.IsNullOrEmpty(RequestEmail) && !string.IsNullOrEmpty(Department) && !string.IsNullOrEmpty(ContactNo))
            {
                string ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
              
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn.Open();
                    SqlCommand command;
                    command = new SqlCommand("UPDATE [IflowSeed].[dbo].[ManagementITform] SET ModifiedOn=@ModifiedOn,RequestType=@RequestType, RequestName=@RequestName, RequestEmail=@RequestEmail, Department=@Department, ContactNo=@ContactNo  WHERE Id =@Id", cn);
                    command.Parameters.AddWithValue("@ModifiedOn", ModifiedOn);
                    command.Parameters.AddWithValue("@RequestType", RequestType);
                    command.Parameters.AddWithValue("@RequestName", RequestName);
                    command.Parameters.AddWithValue("@RequestEmail", RequestEmail);
                    command.Parameters.AddWithValue("@Department", Department);
                    command.Parameters.AddWithValue("@ContactNo", ContactNo);
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                    cn.Close();
                }
            }

        }
        else if (set == "SectionB")
        {
            if (!string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(UnitSystem) && !string.IsNullOrEmpty(Type) && !string.IsNullOrEmpty(ChangeRequired))
            {

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn.Open();
                    SqlCommand command;
                    command = new SqlCommand("UPDATE [IflowSeed].[dbo].[ManagementITform] SET UnitSystem=@UnitSystem, Type=@Type, ChangeRequired=@ChangeRequired WHERE Id =@Id", cn);
                    command.Parameters.AddWithValue("@UnitSystem", UnitSystem);
                    command.Parameters.AddWithValue("@Type", Type);
                    command.Parameters.AddWithValue("@ChangeRequired", ChangeRequired);                 
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }
        else if (set == "SectionC")
        {
            if (!string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(ChangeType) && !string.IsNullOrEmpty(IncidentNo) && !string.IsNullOrEmpty(ReasonforChange) && !string.IsNullOrEmpty(Dependencies) && !string.IsNullOrEmpty(BeforeDateOnTxt) && !string.IsNullOrEmpty(AfterDateOnTxt) && !string.IsNullOrEmpty(TargetImpDateOnTxt) && !string.IsNullOrEmpty(Duration) && !string.IsNullOrEmpty(AfterModificationRemark))
            {
                this.BeforeDateOnTxt = "22/11/2009";
                DateTime BeforeDateOn = DateTime.ParseExact(this.BeforeDateOnTxt, "dd/MM/yyyy", null);
                this.AfterDateOnTxt = "22/11/2009";
                DateTime AfterDateOn = DateTime.ParseExact(this.AfterDateOnTxt, "dd/MM/yyyy", null);
                this.TargetImpDateOnTxt = "22/11/2009";
                DateTime TargetImpDateOn = DateTime.ParseExact(this.TargetImpDateOnTxt, "dd/MM/yyyy", null);

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn.Open();
                    SqlCommand command;
                    command = new SqlCommand("UPDATE [IflowSeed].[dbo].[ManagementITform] SET ChangeType = @ChangeType, IncidentNo = @IncidentNo, ReasonforChange = @ReasonforChange, Dependencies = @Dependencies, BeforeDateOn= @BeforeDateOn, AfterDateOn =@AfterDateOn, TargetImpDateOn = @TargetImpDateOn, Duration =@Duration, AfterModificationRemark = @AfterModificationRemark WHERE Id =@Id", cn);
                    command.Parameters.AddWithValue("@ChangeType", ChangeType);
                    command.Parameters.AddWithValue("@IncidentNo", IncidentNo);
                    command.Parameters.AddWithValue("@ReasonforChange", ReasonforChange);
                    command.Parameters.AddWithValue("@Dependencies", Dependencies);
                    command.Parameters.AddWithValue("@BeforeDateOn", BeforeDateOnTxt);
                    command.Parameters.AddWithValue("@AfterDateOn", AfterDateOnTxt);
                    command.Parameters.AddWithValue("@TargetImpDateOn", TargetImpDateOn);
                    command.Parameters.AddWithValue("@Duration", Duration);
                    command.Parameters.AddWithValue("@AfterModificationRemark", AfterModificationRemark);                  
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }
        else if (set == "SectionD")
        {
            if (!string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(StartDevDateTxt) && !string.IsNullOrEmpty(EndDevDateTxt) && !string.IsNullOrEmpty(Task) && !string.IsNullOrEmpty(ImplementationDateOnTxt) && !string.IsNullOrEmpty(TimeImplementation) && !string.IsNullOrEmpty(Deliveables) && !string.IsNullOrEmpty(Who))
            {
                this.StartDevDateTxt = "22/11/2009";
                DateTime StartDevDate = DateTime.ParseExact(this.StartDevDateTxt, "dd/MM/yyyy", null);
                this.EndDevDateTxt = "22/11/2009";
                DateTime EndDevDate = DateTime.ParseExact(this.EndDevDateTxt, "dd/MM/yyyy", null);
                this.ImplementationDateOnTxt = "22/11/2009";
                DateTime ImplementationDateOn = DateTime.ParseExact(this.ImplementationDateOnTxt, "dd/MM/yyyy", null);

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn.Open();
                    SqlCommand command;
                    command = new SqlCommand("UPDATE [IflowSeed].[dbo].[ManagementITform] SET StartDevDate=@StartDevDate,EndDevDate=@EndDevDate,Task=@Task,ImplementationDateOn=@ImplementationDateOn,TimeImplementation=@TimeImplementation,Deliveables=@Deliveables,Who=@Who,AnyProcedure=@AnyProcedure,AnyProcedure1=@AnyProcedure1,SpecifyTheName=@SpecifyTheName,AssignByLeader=@AssignByLeader WHERE Id=@Id", cn);
                    command.Parameters.AddWithValue("@StartDevDate", StartDevDateTxt);
                    command.Parameters.AddWithValue("@EndDevDate", EndDevDateTxt);               
                    command.Parameters.AddWithValue("@Task", Task);
                    command.Parameters.AddWithValue("@ImplementationDateOn", ImplementationDateOnTxt);
                    command.Parameters.AddWithValue("@TimeImplementation", TimeImplementation);
                    command.Parameters.AddWithValue("@Deliveables", Deliveables);
                    command.Parameters.AddWithValue("@Who", Who);
                    if (AnyProcedure == "on")
                    {
                        command.Parameters.AddWithValue("@AnyProcedure", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@AnyProcedure", false);
                    }
                    if (AnyProcedure1 == "on")
                    {
                        command.Parameters.AddWithValue("@AnyProcedure1", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@AnyProcedure1", false);
                    }
                    command.Parameters.AddWithValue("@SpecifyTheName", SpecifyTheName);
                    command.Parameters.AddWithValue("@AssignByLeader", IdentityName.ToString());
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }
    
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT Id,BatchNo,ProductName,RequestType,RequestName,
                                    RequestEmail,Department,ContactNo,UnitSystem,Type,
                                    ChangeRequired,ChangeType,IncidentNo,ReasonforChange,Dependencies,
                                    BeforeDateOn,AfterDateOn,TargetImpDateOn,Duration,AfterModificationRemark,
                                    StartDevDate,EndDevDate,Task,ImplementationDateOn,TimeImplementation,Deliveables,
                                    Who,AnyProcedure,AnyProcedure1,SpecifyTheName                                   
                                    FROM  [IflowSeed].[dbo].[ManagementITform]
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
                    ViewBag.BatchNo = reader.GetString(1);
                }
                if (reader.IsDBNull(2) == false)
                {
                    ViewBag.ProductName = reader.GetString(2);
                }
                if (reader.IsDBNull(3) == false)
                {
                    ViewBag.RequestType = reader.GetString(3);
                }
                if (reader.IsDBNull(4) == false)
                {
                    ViewBag.RequestName = reader.GetString(4);
                }
                if (reader.IsDBNull(5) == false)
                {
                    ViewBag.RequestEmail = reader.GetString(5);
                }
                if (reader.IsDBNull(6) == false)
                {
                    ViewBag.Department = reader.GetString(6);
                }                
                if (reader.IsDBNull(7) == false)
                {
                    ViewBag.ContactNo = reader.GetString(7);
                }           
                if (reader.IsDBNull(8) == false)
                {
                    ViewBag.UnitSystem = reader.GetString(8);
                }
                if (reader.IsDBNull(9) == false)
                {
                    ViewBag.Type = reader.GetString(9);
                }
                if (reader.IsDBNull(10) == false)
                {
                    ViewBag.ChangeRequired = reader.GetString(10);
                }
                if (reader.IsDBNull(11) == false)
                {
                    ViewBag.ChangeType = reader.GetString(11);
                }
                if (reader.IsDBNull(12) == false)
                {
                    ViewBag.IncidentNo = reader.GetString(12);
                }
                if (reader.IsDBNull(13) == false)
                {
                    ViewBag.ReasonforChange = reader.GetString(13);
                }
                if (reader.IsDBNull(14) == false)
                {
                    ViewBag.Dependencies = reader.GetString(14);
                }
                if (reader.IsDBNull(15) == false)
                {
                    ViewBag.BeforeDateOnTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(15));
                }
                if (reader.IsDBNull(16) == false)
                {
                    ViewBag.AfterDateOnTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(16));
                }
                if (reader.IsDBNull(17) == false)
                {
                    ViewBag.TargetImpDateOnTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(17));
                }
                if (reader.IsDBNull(18) == false)
                {
                    ViewBag.Duration = reader.GetString(18);
                }
                if (reader.IsDBNull(19) == false)
                {
                    ViewBag.AfterModificationRemark = reader.GetString(19);
                }
                if (reader.IsDBNull(20) == false)
                {
                    ViewBag.StartDevDateTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(20));
                }
                if (reader.IsDBNull(21) == false)
                {
                    ViewBag.EndDevDateTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(21));
                }
                if (reader.IsDBNull(22) == false)
                {
                    ViewBag.Task = reader.GetString(22);
                }
                if (reader.IsDBNull(23) == false)
                {
                    ViewBag.ImplementationDateOnTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(23));
                }
                if (reader.IsDBNull(24) == false)
                {
                    ViewBag.TimeImplementation = reader.GetString(24);
                }
                if (reader.IsDBNull(25) == false)
                {
                    ViewBag.Deliveables = reader.GetString(25);
                }
                if (reader.IsDBNull(26) == false)
                {
                    ViewBag.Who = reader.GetString(26);
                }
                if (reader.IsDBNull(27) == false)
                {
                    bool getAnyProcedure = reader.GetBoolean(27);
                    if (getAnyProcedure == false)
                    {
                        ViewBag.AnyProcedure = "";
                    }
                    else
                    {
                        ViewBag.AnyProcedure = "checked";
                    }
                }
                if (reader.IsDBNull(28) == false)
                {
                    bool getAnyProcedure1 = reader.GetBoolean(28);
                    if (getAnyProcedure1 == false)
                    {
                        ViewBag.AnyProcedure1 = "";
                    }
                    else
                    {
                        ViewBag.AnyProcedure1 = "checked";
                    }
                }
                if (reader.IsDBNull(29) == false)
                {
                    ViewBag.SpecifyTheName = reader.GetString(29);
                }
                

            }
            cn.Close();
        }

        return View();

    }

    public ActionResult ReleaseRequest(ManagementITform model)
    {
        if ((model.Id != Guid.Empty))

        {

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                string ItSubmRequestFormn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("UPDATE [IflowSeed].[dbo].[ManagementITform] SET Status=@Status WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@Status", "Request Released");
                command.Parameters.AddWithValue("@Id", model.Id);
                command.ExecuteNonQuery();
                cn.Close();
            }
        }
        else
        {
            TempData["msg"] = "<script>alert('Please complete form details !');</script>";
        }

        return RedirectToAction("ManageITform", "RequestForm");
    }


    List<Nim_IssueJob> Nim_IssueJoblist = new List<Nim_IssueJob>();

    public ActionResult ManageIssueNIM(string Id, string BatchNo, string set)
    {
        if (set == "search") //ini kalu user search product
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Id,BatchNo,Status,Company,Department,
                                           CreateIssueBy,DateIssue,ProblemIssue,DateComplete,IdentifyIssue
                                     FROM [IflowSeed].[dbo].[Nim_IssueJob]
                                     WHERE BatchNo LIKE @BatchNo
                                     ORDER BY CreatedOn desc";
                command.Parameters.AddWithValue("@BatchNo", "%" + BatchNo + "%");
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Nim_IssueJob model = new Nim_IssueJob();
                    {
                        model.Bil = _bil++;
                        if (reader.IsDBNull(0) == false)
                        {
                            model.Id = reader.GetGuid(0);
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            model.BatchNo = reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            model.Status = reader.GetString(2);
                        }
                        if (reader.IsDBNull(3) == false)
                        {
                            model.Company = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            model.Department = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            model.CreateIssueBy = reader.GetString(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.CreatedOn = reader.GetDateTime(6);
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.ProblemIssue = reader.GetString(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.DateCompleteTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(8));
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            model.IdentifyIssue = reader.GetString(9);
                        }
                    }
                    Nim_IssueJoblist.Add(model);
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
                command.CommandText = @"SELECT Id,BatchNo,Status,Company,Department,
                                           CreateIssueBy,CreatedOn,ProblemIssue,DateComplete,IdentifyIssue
                                     FROM [IflowSeed].[dbo].[Nim_IssueJob]
                                     ORDER BY CreatedOn desc";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Nim_IssueJob model = new Nim_IssueJob();
                    {
                        model.Bil = _bil++;
                        if (reader.IsDBNull(0) == false)
                        {
                            model.Id = reader.GetGuid(0);
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            model.BatchNo = reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            model.Status = reader.GetString(2);
                        }
                        if (reader.IsDBNull(3) == false)
                        {
                            model.Company = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            model.Department = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            model.CreateIssueBy = reader.GetString(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.CreatedOn = reader.GetDateTime(6);
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.ProblemIssue = reader.GetString(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.DateCompleteTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(8));
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            model.IdentifyIssue = reader.GetString(9);
                        }
                       
                    }
                    Nim_IssueJoblist.Add(model);
                }
                cn.Close();
            }
        }

        return View(Nim_IssueJoblist); //hntr data ke ui
    }

    public ActionResult CreateIssueNIM(string Id,string BatchNo, string Status, string set,
                                       string Company, string Department, string CreateIssueBy,
                                       string EmailRequested, string ProblemIssue)
    {

        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];


        ViewBag.IsDepart = @Session["Department"];
        ViewBag.AccountManager = IdentityName.ToString();

        List<SelectListItem> listCompany = new List<SelectListItem>();

        listCompany.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listCompany.Add(new SelectListItem { Text = "INTERCITY", Value = "INTERCITY" });
        listCompany.Add(new SelectListItem { Text = "PRO OFFICE", Value = "PRO OFFICE" });
        ViewData["Company_"] = listCompany;

        List<SelectListItem> listDepartment = new List<SelectListItem>();

        listDepartment.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listDepartment.Add(new SelectListItem { Text = "FINANCE", Value = "FINANCE" });
        listDepartment.Add(new SelectListItem { Text = "HR", Value = "HR" });
        listDepartment.Add(new SelectListItem { Text = "IT", Value = "IT" });
        listDepartment.Add(new SelectListItem { Text = "MBD", Value = "MBD" });
        listDepartment.Add(new SelectListItem { Text = "OPERATION", Value = "OPERATION" });
        listDepartment.Add(new SelectListItem { Text = "POSTING", Value = "POSTING" });
        listDepartment.Add(new SelectListItem { Text = "QM", Value = "QM" });
        listDepartment.Add(new SelectListItem { Text = "RMS", Value = "RMS" });
        ViewData["Department_"] = listDepartment;

        if (string.IsNullOrEmpty(Id))
        {
            ViewBag.DataSet = "Save";
        }
        else
        {
            ViewBag.DataSet = "update";
        }

        if (string.IsNullOrEmpty(Id) && Company != "Please Select" && Department != "Please Select" && !string.IsNullOrEmpty(Company) && !string.IsNullOrEmpty(Department) && !string.IsNullOrEmpty(EmailRequested) && !string.IsNullOrEmpty(ProblemIssue))
        {
            var No_ = new NoCounterModel();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                Guid Idx = Guid.NewGuid();
                string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                cn.Open();
                SqlCommand command;
                command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[Nim_IssueJob] (Id, CreatedOn, BatchNo, Status, Company,Department,EmailRequested,ProblemIssue,CreateIssueBy) values (@Id, @CreatedOn, @BatchNo, @Status, @Company, @Department, @EmailRequested, @ProblemIssue, @CreateIssueBy)", cn);
                command.Parameters.AddWithValue("@Id", Idx);
                command.Parameters.AddWithValue("@CreatedOn", createdOn);
                command.Parameters.AddWithValue("@BatchNo", No_.RefNo);
                command.Parameters.AddWithValue("@Status", "New");
                command.Parameters.AddWithValue("@Company", Company);
                command.Parameters.AddWithValue("@Department", Department);
                command.Parameters.AddWithValue("@EmailRequested", EmailRequested);
                command.Parameters.AddWithValue("@ProblemIssue", ProblemIssue);
                command.Parameters.AddWithValue("@CreateIssueBy", IdentityName.ToString());
                command.ExecuteNonQuery();
                cn.Close();
            }
            //bila save akn gi kt managecustomer
            return RedirectToAction("ManageIssueNIM", "RequestForm");
        }

        if (!string.IsNullOrEmpty(Id) && set == "update")
        {
            //update
            string ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("UPDATE [IflowSeed].[dbo].[Nim_IssueJob]  SET ModifiedOn=@ModifiedOn, Company=@Company, Department=@Department, EmailRequested=@EmailRequested, ProblemIssue=@ProblemIssue, EndContractDate=@EndContractDate, CreateIssueBy=@CreateIssueBy  WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@ModifiedOn", ModifiedOn);
                command.Parameters.AddWithValue("@Company", Company);
                command.Parameters.AddWithValue("@Department", Department);
                command.Parameters.AddWithValue("@EmailRequested", EmailRequested);
                command.Parameters.AddWithValue("@ProblemIssue", ProblemIssue);
                command.Parameters.AddWithValue("@CreateIssueBy", IdentityName.ToString());
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn.Close();
            }
        }
        if (!string.IsNullOrEmpty(Id))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Id, Company,Department,
                                       EmailRequested, ProblemIssue
                                       FROM [IflowSeed].[dbo].[Nim_IssueJob]                              
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
                        ViewBag.Company = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        ViewBag.Department = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        ViewBag.EmailRequested = reader.GetString(3);
                    }                                        
                    if (reader.IsDBNull(4) == false)
                    {
                        ViewBag.ProblemIssue = reader.GetString(4);
                    }

                }
                cn.Close();
            }
        }

        return View();
    }


    public ActionResult DeleteIssueNIM(string Id)
    {
        if (!string.IsNullOrEmpty(Id))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("DELETE [IflowSeed].[dbo].[ManagementITform] WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn.Close();
            }
        }
        return RedirectToAction("ManageIssueNIM", "RequestForm");
    }

    List<ChangeRequestBSS> ChangeRequestBSSlist = new List<ChangeRequestBSS>();

    public ActionResult ManageChangeReqForm(string Id, string CRNo, string set, string ProductName,string JobInstructionId)
    {
       
        List<SelectListItem> li = new List<SelectListItem>();

        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil = 1;
            cn.Open();

            command.CommandText = @"select distinct(ProductName) from [IflowSeed].[dbo].[ChangeRequestBSS]
                                    ORDER BY ProductName asc";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                ChangeRequestBSS model = new ChangeRequestBSS();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.ProductName = reader.GetString(0);
                    }
                }
                int i = _bil++;

                if (i == 1)
                {
                    li.Add(new SelectListItem { Text = "Please Select" });
                }

                li.Add(new SelectListItem { Text = model.ProductName });

            }
            cn.Close();

        }

        ViewData["BNO"] = li;

        if (!string.IsNullOrEmpty(ProductName))
        {

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Id,CRNo,ProductName,Department,Status,Company,
                                           TypeOfIssue,JobType,CreatedOn,EstimateDate,PersonInCharge,
                                           RequestFrom,Description,JobInstructionId
                                           FROM [IflowSeed].[dbo].[ChangeRequestBSS]
                                           where ProductName=@BNO 
                                           order by ProductName asc";
                command.Parameters.Add(new SqlParameter("@BNO", ProductName.ToString()));
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ChangeRequestBSS model = new ChangeRequestBSS();
                    {
                        model.Bil = _bil++;
                        if (reader.IsDBNull(0) == false)
                        {
                            model.Id = reader.GetGuid(0);
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            model.CRNo = reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            model.ProductName = reader.GetString(2);
                        }
                        if (reader.IsDBNull(3) == false)
                        {
                            model.Department = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            model.Status = reader.GetString(4);
                        }                       
                        if (reader.IsDBNull(5) == false)
                        {
                            model.Company = reader.GetString(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.TypeOfIssue = reader.GetString(6);
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.JobType = reader.GetString(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.CreatedOn = reader.GetDateTime(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            model.EstimateDateTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(9));
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            model.PersonInCharge = reader.GetString(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            model.RequestFrom = reader.GetString(11);
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            model.Description = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            model.JobInstructionId = reader.GetGuid(13);
                        }

                    }
                    ChangeRequestBSSlist.Add(model);
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
                command.CommandText = @"SELECT Id,CRNo,ProductName,Department,Status,Company,
                                           TypeOfIssue,JobType,CreatedOn,EstimateDate,PersonInCharge,
                                           RequestFrom,Description,JobInstructionId
                                           FROM [IflowSeed].[dbo].[ChangeRequestBSS]
                                           WHERE Status='New'
                                           ORDER BY CreatedOn desc";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ChangeRequestBSS model = new ChangeRequestBSS();
                    {
                        model.Bil = _bil++;
                        if (reader.IsDBNull(0) == false)
                        {
                            model.Id = reader.GetGuid(0);
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            model.CRNo = reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            model.ProductName = reader.GetString(2);
                        }
                        if (reader.IsDBNull(3) == false)
                        {
                            model.Department = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            model.Status = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            model.Company = reader.GetString(5);
                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            model.TypeOfIssue = reader.GetString(6);
                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            model.JobType = reader.GetString(7);
                        }
                        if (reader.IsDBNull(8) == false)
                        {
                            model.CreatedOn = reader.GetDateTime(8);
                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            model.EstimateDateTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(9));
                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            model.PersonInCharge = reader.GetString(10);
                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            model.RequestFrom = reader.GetString(11);
                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            model.Description = reader.GetString(12);
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            model.JobInstructionId = reader.GetGuid(13);
                        }

                    }
                    ChangeRequestBSSlist.Add(model);
                }
                cn.Close();
            }
        }

        return View(ChangeRequestBSSlist); //hntr data ke ui
    }

    public ActionResult CreateChangeReqForm(string Id, string set, string CRNo, string Status, string Department,
                                            string ProductName, string JobName, string TypeOfIssue,
                                            string Company, string EstimateDateTxt, string PersonInCharge,
                                            string RequestFrom, string Email, string JobType, string Description, string JobInstructionId,string ChangeReqBSS)
    {

        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.AccountManager = IdentityName.ToString();

        Session["JobInstructionId"] = Id;
        Session["Id"] = Id;
        Session["ProductName"] = ProductName;




        int _bil = 1;
        List<SelectListItem> li = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT ProductName FROM [IflowSeed].[dbo].[ChangeRequestBSS]                          
                                     ORDER BY ProductName";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                ChangeRequestBSS model = new ChangeRequestBSS();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.ProductName = reader.GetString(0);
                    }
                }
                int i = _bil++;
                if (i == 1)
                {
                    li.Add(new SelectListItem { Text = "Please Select" });
                }
                li.Add(new SelectListItem { Text = model.ProductName });
            }
            cn.Close();
        }
        ViewData["ProductName_"] = li;

        List<SelectListItem> listCompany = new List<SelectListItem>();

        listCompany.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listCompany.Add(new SelectListItem { Text = "INTERCITY", Value = "INTERCITY" });
        listCompany.Add(new SelectListItem { Text = "PRO OFFICE", Value = "PRO OFFICE" });
        ViewData["Company_"] = listCompany;

        List<SelectListItem> listDepartment = new List<SelectListItem>();

        listDepartment.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listDepartment.Add(new SelectListItem { Text = "FINANCE", Value = "FINANCE" });
        listDepartment.Add(new SelectListItem { Text = "HR", Value = "HR" });
        listDepartment.Add(new SelectListItem { Text = "IT", Value = "IT" });
        listDepartment.Add(new SelectListItem { Text = "MBD", Value = "MBD" });
        listDepartment.Add(new SelectListItem { Text = "OPERATION", Value = "OPERATION" });
        listDepartment.Add(new SelectListItem { Text = "POSTING", Value = "POSTING" });
        listDepartment.Add(new SelectListItem { Text = "QM", Value = "QM" });
        listDepartment.Add(new SelectListItem { Text = "RMS", Value = "RMS" });
        listDepartment.Add(new SelectListItem { Text = "PLANNER", Value = "PLANNER" });
        listDepartment.Add(new SelectListItem { Text = "PRODUCTION", Value = "PRODUCTION" });
        listDepartment.Add(new SelectListItem { Text = "ENGINEER", Value = "ENGINEER" });
        listDepartment.Add(new SelectListItem { Text = "STORE", Value = "STORE" });
        listDepartment.Add(new SelectListItem { Text = "FINANCE", Value = "FINANCE" });
        listDepartment.Add(new SelectListItem { Text = "BSS", Value = "BSS" });
        ViewData["Department_"] = listDepartment;

        List<SelectListItem> listPersonInCharge = new List<SelectListItem>();

        listPersonInCharge.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listPersonInCharge.Add(new SelectListItem { Text = "SEAN NG", Value = "SEAN NG" });
        listPersonInCharge.Add(new SelectListItem { Text = "NASIR NOORDIN", Value = "NASIR NOORDIN" });
        listPersonInCharge.Add(new SelectListItem { Text = "HASDURA", Value = "HASDURA" });
        listPersonInCharge.Add(new SelectListItem { Text = "SITI AMINAH", Value = "SITI AMINAH" });
        listPersonInCharge.Add(new SelectListItem { Text = "NORFARIZA", Value = "NORFARIZA" });
        ViewData["PersonInCharge_"] = listPersonInCharge;

        List<SelectListItem> listTypeOfIssue = new List<SelectListItem>();

        listTypeOfIssue.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listTypeOfIssue.Add(new SelectListItem { Text = "NEW SYSTEM", Value = "NEW SYSTEM" });
        listTypeOfIssue.Add(new SelectListItem { Text = "NEW ARTWORK", Value = "NEW ARTWORK" });
        listTypeOfIssue.Add(new SelectListItem { Text = "ENHANCEMENT SYSTEM", Value = "ENHANCEMENT SYSTEM" });
        listTypeOfIssue.Add(new SelectListItem { Text = "ENHANCEMENT ART WORK", Value = "ENHANCEMENT ART WORK" });
        listTypeOfIssue.Add(new SelectListItem { Text = "PROBLEM", Value = "PROBLEM" });
        ViewData["TypeOfIssue_"] = listTypeOfIssue;

        List<SelectListItem> listJobType = new List<SelectListItem>();

        listJobType.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listJobType.Add(new SelectListItem { Text = "NEW", Value = "NEW" });
        listJobType.Add(new SelectListItem { Text = "ART DESIGN", Value = "ART DESIGN" });
        listJobType.Add(new SelectListItem { Text = "iFLOW", Value = "iFLOW" });
        listJobType.Add(new SelectListItem { Text = "SEED", Value = "SEED" });
        listJobType.Add(new SelectListItem { Text = "DOCTRAC", Value = "DOCTRAC" });
        listJobType.Add(new SelectListItem { Text = "E-SMART", Value = "E-SMART" });
        listJobType.Add(new SelectListItem { Text = "i-HELPDESK", Value = "i-HELPDESK" });
        listJobType.Add(new SelectListItem { Text = "RED MAIL", Value = "RED MAIL" });
        listJobType.Add(new SelectListItem { Text = "DESS SYSTEM", Value = "DESS SYSTEM" });
        listJobType.Add(new SelectListItem { Text = "MERGING APP TOOLS", Value = "MERGING APP TOOLS" });      
        ViewData["JobType_"] = listJobType;

        if (!string.IsNullOrEmpty(Id) && Company != "Please Select" && Department != "Please Select" && !string.IsNullOrEmpty(Company) && !string.IsNullOrEmpty(PersonInCharge) && !string.IsNullOrEmpty(Department) && !string.IsNullOrEmpty(ProductName) && !string.IsNullOrEmpty(JobName) && !string.IsNullOrEmpty(TypeOfIssue) && !string.IsNullOrEmpty(Company) && !string.IsNullOrEmpty(EstimateDateTxt) && !string.IsNullOrEmpty(PersonInCharge) && !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(JobType) && !string.IsNullOrEmpty(Description))
        {
           
            var No_ = new NoCounterModel();

            //update
            string ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
            this.EstimateDateTxt = "22/11/2009";
            DateTime EstimateDate = DateTime.ParseExact(this.EstimateDateTxt, "dd/MM/yyyy", null);

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("UPDATE [IflowSeed].[dbo].[ChangeRequestBSS]  SET ModifiedOn=@ModifiedOn,Department=@Department,JobName=@JobName,TypeOfIssue=@TypeOfIssue,Company=@Company,EstimateDate=@EstimateDate,PersonInCharge=@PersonInCharge,RequestFrom=@RequestFrom,Email=@Email,JobType=@JobType,Description=@Description,Status=@Status,CRNo=@CRNo WHERE ProductName=@ProductName AND Id=@Id", cn);
                command.Parameters.AddWithValue("@ModifiedOn", ModifiedOn);
                command.Parameters.AddWithValue("@Department", Department);
                command.Parameters.AddWithValue("@JobName", JobName);
                command.Parameters.AddWithValue("@TypeOfIssue", TypeOfIssue);
                command.Parameters.AddWithValue("@Company", Company);
                command.Parameters.AddWithValue("@EstimateDate", EstimateDateTxt);
                command.Parameters.AddWithValue("@PersonInCharge", PersonInCharge);
                command.Parameters.AddWithValue("@RequestFrom", IdentityName.ToString());
                command.Parameters.AddWithValue("@Email", Email);
                command.Parameters.AddWithValue("@JobType", JobType);
                command.Parameters.AddWithValue("@Description", Description);
                command.Parameters.AddWithValue("@Status", "New");
                command.Parameters.AddWithValue("@CRNo", No_.RefNo);
                command.Parameters.AddWithValue("@ProductName", ProductName);
                command.Parameters.AddWithValue("@Id", Id);


                command.ExecuteNonQuery();
                cn.Close();
            }
        }
        if (!string.IsNullOrEmpty(Id))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Id, Department, ProductName, JobName, TypeOfIssue,
                                               Company, EstimateDate, PersonInCharge,
                                               Email, JobType, Description
                                       FROM [IflowSeed].[dbo].[ChangeRequestBSS]                              
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
                        ViewBag.Department = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        ViewBag.ProductName = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        ViewBag.JobName = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        ViewBag.TypeOfIssue = reader.GetString(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        ViewBag.Company = reader.GetString(5);
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        ViewBag.EstimateDateTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(6));
                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        ViewBag.PersonInCharge = reader.GetString(7);
                    }
                    if (reader.IsDBNull(8) == false)
                    {
                        ViewBag.Email = reader.GetString(8);
                    }
                    if (reader.IsDBNull(9) == false)
                    {
                        ViewBag.JobType = reader.GetString(9);
                    }
                    if (reader.IsDBNull(10) == false)
                    {
                        ViewBag.Description = reader.GetString(10);
                    }

                }
                cn.Close();
            }
        }

        return View();
    }

    public ActionResult ReloadAttachment(string Id, string set)
    {

        if (set == "update")
        {
            List<SampleProduct> viewFileStore = new List<SampleProduct>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Picture_FileId,Id,ChangeReqBSS
                                      FROM [IflowSeed].[dbo].[SampleProduct]  
                                      WHERE Id=@ChangeReqBSS                                   
                                      ORDER BY Picture_FileId DESC";
                command.Parameters.AddWithValue("@ChangeReqBSS", Session["Id"].ToString());
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
        else
        {
            List<SampleProduct> viewFileStore = new List<SampleProduct>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Picture_FileId,Id,ChangeReqBSS
                                      FROM [IflowSeed].[dbo].[SampleProduct]  
                                      WHERE Id=@Id                                   
                                      ORDER BY Picture_FileId DESC";
                command.Parameters.AddWithValue("@Id",Id);
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


       
    }

    public ActionResult UploadAttachment(SampleProduct ModelSample, string ChangeReqBSS)
    {
        var IdentityName = @Session["Fullname"];
        var Id = Session["Id"];
        Session["Id"] = Id;
        Session["Id"] = ChangeReqBSS;

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
                command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[SampleProduct] (Id,CreatedOn,Picture_FileId,ChangeReqBSS,Picture_Extension,Code,CreateBy) values (@Id,@CreatedOn,@Picture_FileId,@ChangeReqBSS,@Picture_Extension,@Code,@CreateBy)", cn2);
                command.Parameters.AddWithValue("@Id", guidId);
                command.Parameters.AddWithValue("@CreatedOn", createdOn);
                command.Parameters.AddWithValue("@Picture_FileId", fileName.Trim());
                command.Parameters.AddWithValue("@ChangeReqBSS", Id);
                command.Parameters.AddWithValue("@Picture_Extension", ModelSample.FileUploadFile.ContentType);
                command.Parameters.AddWithValue("@Code", "ChangeReqBSS");
                command.Parameters.AddWithValue("@CreateBy", IdentityName.ToString());
                command.ExecuteNonQuery();
                cn2.Close();

            }

            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn2.Open();
                SqlCommand command;
                command = new SqlCommand("UPDATE [IflowSeed].[dbo].[ChangeRequestBSS] SET ChangeReqBSS=@ChangeReqBSS WHERE Id=@Id", cn2);
                command.Parameters.AddWithValue("@ChangeReqBSS", Id);
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn2.Close();

            }


            return RedirectToAction("CreateChangeReqForm", "RequestForm", new { Id = Id.ToString() });
        }

        if (ModelSample.Set == "back")
        {
            return RedirectToAction("CreateChangeReqForm", "RequestForm", new { Id = Id.ToString() });
        }

        return View();
    }

    public ActionResult DeleteAttachment(string Id, string ChangeReqBSS)
    {
        Guid SampleProductId = Guid.Empty;

        if (Id != null)
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Picture_FileId,ChangeReqBSS
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
                        return RedirectToAction("CreateChangeReqForm", "RequestForm", new { Id = Session["Id"].ToString() });
                    }
                }
                cn.Close();
            }
        }

        return RedirectToAction("CreateChangeReqForm", "RequestForm", new { Id = Session["Id"].ToString() });
    }

    string PathSource = System.Configuration.ConfigurationManager.AppSettings["SourceFile"];


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

        return RedirectToAction("CreateChangeReqForm", "RequestForm", new { Id = Id.ToString() });
    }

}




