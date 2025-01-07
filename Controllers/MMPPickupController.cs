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

[MvcAppV2.FilterConfig.UserSessionActionFilter]
public class MMPPickupController : Controller
{
    public ActionResult ManageMMPPickup(string product, string set, string pageNumber)

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
                command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass, JobType,JobSheetNo, 
                                            StartDevDate, EndDevDate,AccountsQty,ImpressionQty, 
                                            PagesQty,IT_SysNotes,Produc_PlanningNotes,PurchasingNotes,
                                            EngineeringNotes ,ArtworkNotes,Acc_BillingNotes,MMPSlip
                                         FROM [JobInstruction]                                    
                                         WHERE ProductName LIKE @ProductName OR JobSheetNo LIKE @ProductName
                                         AND Status = 'MMPPickup'
                                         ORDER BY CreatedOn DESC ";

                command.Parameters.AddWithValue("@ProductName", "%" + product + "%");
            }

            else if (set == "GoTo")
            {
                if (pageNumber == "0")
                {

                    command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass
                                            , JobType,JobSheetNo, StartDevDate, EndDevDate
                                            ,AccountsQty,ImpressionQty, PagesQty,IT_SysNotes
                                            ,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes
                                         ,ArtworkNotes,Acc_BillingNotes,MMPSlip
                                        FROM [JobInstruction]
                                        WHERE Status = 'MMPPickup'
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
                                         ,ArtworkNotes,Acc_BillingNotes,MMPSlip
                                        FROM [JobInstruction]
                                        WHERE Status = 'MMPPickup'
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
                //                         ,ArtworkNotes,Acc_BillingNotes,MMPSlip
                //                        FROM [JobInstruction]
                //                        WHERE Status = 'MMPPickup'
                //                        ORDER BY (SELECT NULL)
                //                        OFFSET 0 ROWS
                //                        FETCH NEXT 100 ROWS ONLY";

                command.CommandText = @"SELECT Customer_Name, ProductName, JobClass,
                                            JobType,JobSheetNo, SUM(CAST(AccQty AS INT)) as AccQty, SUM(CAST(ImpQty AS INT)) as ImpQty, SUM(CAST(PageQty AS INT)) as PageQty,PrintSlip,LogTagNo,Id
                                        FROM [JobAuditTrailDetail]
                                        WHERE Status = 'MMPPickup'
                                        GROUP BY Customer_Name, ProductName, JobClass,JobType,JobSheetNo,PrintSlip,LogTagNo,Id
                                        ORDER BY (SELECT NULL)
                                        OFFSET 0 ROWS
                                        FETCH NEXT 100 ROWS ONLY";

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
                        model.PrintSlip = reader.GetString(8);
                    }
                    if (!reader.IsDBNull(9))
                    {
                        model.LogTagNo = reader.GetString(9);
                    }
                    if (reader.IsDBNull(10) == false)
                    {
                        model.Id = reader.GetGuid(10);
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
                                    command3 = new SqlCommand("UPDATE [JobInstruction] SET MMPSlip=NULL WHERE Id=@Id", cn3);
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
                            command3 = new SqlCommand("UPDATE [JobInstruction] SET MMPSlip=NULL WHERE Id=@Id", cn3);
                            command3.Parameters.AddWithValue("@Id", Id);
                            command3.ExecuteNonQuery();
                            cn3.Close();

                        }

                    }


                }
                cn.Close();
            }
        }

        return RedirectToAction("ManageMMPPickup", "MMP");
    }

    public ActionResult CreateProdSlipPickup(MMPPickup get, string set, string PrintSlip, string Ins_StartDateOn, string Ins_StartTime, string Ins_EndDateOn, string Ins_EndTime, string Recovery,
                                     string Ins_Machine, string Process, string Sort, string NonSort, string ProcessType, string Ins_CreateUser, string ProductionSlipId, string Machine, string id, string Company_Name)

    {
        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.AccountManager = IdentityName.ToString();
        //Session["Id"] = Id;
        //ViewBag.Id = Id;
        ViewBag.PrintSlipNo = PrintSlip;
        ViewBag.ProductionSlipId = ProductionSlipId;
        //ViewBag.JobAuditTrailId = JobAuditTrailId;

        ViewBag.Ins_StartDateOn = Ins_StartDateOn;
        ViewBag.Ins_StartTime = Ins_StartTime;
        ViewBag.Ins_EndDateOn = Ins_EndDateOn;
        ViewBag.Ins_EndTime = Ins_EndTime;
        ViewBag.Recovery = Recovery;

        ViewBag.Process = Process;
        ViewBag.Sort = Sort;
        ViewBag.NonSort = NonSort;
        ViewBag.Machine = Machine;
        ViewBag.Company_Name = Company_Name;

        ViewBag.set = set;


        string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
        get.Ins_StartDateOn = String.Format("{0:dd/MM/yyyy}", get.Ins_StartDateOn);
        get.Ins_EndDateOn = String.Format("{0:dd/MM/yyyy}", get.Ins_EndDateOn);

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
        ViewBag.Machine = Ins_Machine;

        if (set == "SaveProdSlipPickup")
        {
            var No_ = new NoProductionModel();
            Guid guidId = Guid.NewGuid();
            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command2 = new SqlCommand("", cn2))
            {
                cn2.Open();
                command2.CommandText = @"INSERT INTO [InsInserting] (Guid, ModifiedOn, Ins_StartDateOn, Ins_StartTime, Ins_EndDateOn, Ins_EndTime, Ins_Machine, Ins_Recovery, Sort, Ins_CreateUser,NonSort,MMPType,PrintSlip,Company_Name) 
                                    values (@Guid,@ModifiedOn,@Ins_StartDateOn, @Ins_StartTime,@Ins_EndDateOn,@Ins_EndTime,@Ins_Machine,@Ins_Recovery, @Sort, @Ins_CreateUser,@NonSort,@MMPType,@PrintSlip,@Company_Name) ";

                command2.Parameters.AddWithValue("@Guid", guidId);
                command2.Parameters.AddWithValue("@ModifiedOn", createdOn);
                if (!string.IsNullOrEmpty(Ins_StartDateOn))
                {
                    string a3 = Convert.ToDateTime(Ins_StartDateOn).ToString("yyyy-MM-dd");
                    command2.Parameters.AddWithValue("@Ins_StartDateOn", a3);
                }
                else
                {
                    command2.Parameters.AddWithValue("@Ins_StartDateOn", null);
                }
                command2.Parameters.AddWithValue("@Ins_StartTime", Ins_StartTime);
                if (!string.IsNullOrEmpty(Ins_EndDateOn))
                {
                    string a4 = Convert.ToDateTime(Ins_EndDateOn).ToString("yyyy-MM-dd");
                    command2.Parameters.AddWithValue("@Ins_EndDateOn", a4);
                }
                else
                {
                    command2.Parameters.AddWithValue("@Ins_EndDateOn", null);
                }

                if (!string.IsNullOrEmpty(Ins_EndDateOn))
                {
                    command2.Parameters.AddWithValue("@Ins_EndTime", Ins_EndTime);
                }
                else
                {
                    command2.Parameters.AddWithValue("@Ins_EndTime", null);
                }

                if (!string.IsNullOrEmpty(Machine))
                {
                    command2.Parameters.AddWithValue("@Ins_Machine", Machine);
                }
                else
                {
                    command2.Parameters.AddWithValue("@Machine", null);
                }

                if (!string.IsNullOrEmpty(Recovery))
                {
                    command2.Parameters.AddWithValue("@Ins_Recovery", Recovery);
                }
                else
                {
                    command2.Parameters.AddWithValue("@Machine", null);
                }

                if (Sort == "on")
                {
                    command2.Parameters.AddWithValue("@Sort", true);
                }
                else
                {
                    command2.Parameters.AddWithValue("@Sort", false);
                }
                command2.Parameters.AddWithValue("@PrintSlip", No_.RefNo);

                command2.Parameters.AddWithValue("@Ins_CreateUser", IdentityName.ToString());

                if (NonSort == "on")
                {
                    command2.Parameters.AddWithValue("@NonSort", true);
                }
                else
                {
                    command2.Parameters.AddWithValue("@NonSort", false);
                }

                command2.Parameters.AddWithValue("@MMPType", "Pickup");

                if (!string.IsNullOrEmpty(Company_Name))
                {
                    command2.Parameters.AddWithValue("@Company_Name", Company_Name);
                }
                else
                {
                    command2.Parameters.AddWithValue("@Company_Name", null);
                }

                command2.ExecuteNonQuery();



                cn2.Close();
                return RedirectToAction("ManageMMPPickup", "MMP");

            }
        }

        if (set == "EditProdSlipPickup")
        {
            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command2 = new SqlCommand("", cn2))
            {
                cn2.Open();
                command2.CommandText = @"UPDATE [InsInserting] 
                                    SET ModifiedOn = @ModifiedOn, Ins_StartDateOn = @Ins_StartDateOn, Ins_StartTime = @Ins_StartTime, Ins_EndDateOn = @Ins_EndDateOn, Ins_EndTime = @Ins_EndTime,
                                    Ins_Machine = @Ins_Machine, Ins_Recovery = @Ins_Recovery, Sort = @Sort, NonSort = @NonSort,  Company_Name = @Company_Name";

                command2.Parameters.AddWithValue("@Guid", id);
                command2.Parameters.AddWithValue("@ModifiedOn", createdOn);
                if (!string.IsNullOrEmpty(Ins_StartDateOn))
                {
                    string a3 = Convert.ToDateTime(Ins_StartDateOn).ToString("yyyy-MM-dd");
                    command2.Parameters.AddWithValue("@Ins_StartDateOn", a3);
                }
                else
                {
                    command2.Parameters.AddWithValue("@Ins_StartDateOn", null);
                }
                command2.Parameters.AddWithValue("@Ins_StartTime", Ins_StartTime);
                if (!string.IsNullOrEmpty(Ins_EndDateOn))
                {
                    string a4 = Convert.ToDateTime(Ins_EndDateOn).ToString("yyyy-MM-dd");
                    command2.Parameters.AddWithValue("@Ins_EndDateOn", a4);
                }
                else
                {
                    command2.Parameters.AddWithValue("@Ins_EndDateOn", null);
                }
                command2.Parameters.AddWithValue("@Ins_EndTime", Ins_EndTime);
                command2.Parameters.AddWithValue("@Ins_Machine", Machine);
                command2.Parameters.AddWithValue("@Ins_Recovery", Recovery);
                if (Sort == "on")
                {
                    command2.Parameters.AddWithValue("@Sort", true);
                }
                else
                {
                    command2.Parameters.AddWithValue("@Sort", false);
                }

                if (NonSort == "on")
                {
                    command2.Parameters.AddWithValue("@NonSort", true);
                }
                else
                {
                    command2.Parameters.AddWithValue("@NonSort", false);
                }

                command2.Parameters.AddWithValue("@MMPType", "Pickup");

                command2.Parameters.AddWithValue("@Company_Name", Company_Name);

                command2.ExecuteNonQuery();



                cn2.Close();
                return RedirectToAction("ManageMMPPickup", "MMP");

            }
        }
        return View();
    }

    public ActionResult CreateProdSlip(Hist_ProductionSlip get, string set, string Id, string PrintSlipNo, string Ins_StartDateOn, string Ins_StartTime, string Ins_EndDateOn, string Ins_EndTime, string Ins_Recovery,
                                       string Ins_Machine, string Process, string Sort, string NonSort, string ProcessType, string Ins_CreateUser, string ProductionSlipId, string JobAuditTrailId,
                                       string NotesByIT, string NotesByProduction, string NotesByPurchasing, string NotesByEngineering, string NotesByArtwork, string NotesByFinance, string NotesByDCP, string ReturnO, string CourierO)

    {

        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.AccountManager = IdentityName.ToString();
        ViewBag.Id = Id;
        ViewBag.PrintSlipNo = PrintSlipNo;
        ViewBag.ProductionSlipId = ProductionSlipId;
        ViewBag.JobAuditTrailId = JobAuditTrailId;

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
        using (SqlCommand command3 = new SqlCommand("", cn6))
        {
            cn6.Open();
            command3.CommandText = @"SELECT JobInstruction.AccountsQty, JobInstruction.ImpressionQty, JobInstruction.PagesQty, Hist_ProductionSlip.StartDateOn, Hist_ProductionSlip.StartTime, Hist_ProductionSlip.EndDateOn, Hist_ProductionSlip.EndTime, Hist_ProductionSlip.SequenceStart, Hist_ProductionSlip.SequenceEnd, Hist_ProductionSlip.Machine, Hist_ProductionSlip.Recovery, Hist_ProductionSlip.FileName,Hist_ProductionSlip.PrintSlipNo,
                                         Hist_ProductionSlip.AccQty,Hist_ProductionSlip.ImpQty,Hist_ProductionSlip.PageQty,JobInstruction.Status,Hist_ProductionSlip.ProcessType
                                         FROM  JobInstruction INNER JOIN
                                    Hist_ProductionSlip ON JobInstruction.Id = Hist_ProductionSlip.[Id]
                                    WHERE JobInstruction.Id=@Id";
            command3.Parameters.AddWithValue("@Id", Id);
            var reader6 = command3.ExecuteReader();
            while (reader6.Read())
            {
                if (reader6.IsDBNull(0) == false)
                {
                    ViewBag.AccountsQty = reader6.GetString(0);
                }

                if (reader6.IsDBNull(1) == false)
                {
                    ViewBag.ImpressionQty = reader6.GetString(1);
                }
                if (reader6.IsDBNull(2) == false)
                {
                    ViewBag.PagesQty = reader6.GetString(2);
                }

                if (reader6.IsDBNull(3) == false)
                {
                    ViewBag.StartDateOn = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader6.GetDateTime(3));
                }

                if (reader6.IsDBNull(4) == false)
                {
                    ViewBag.StartTime = reader6.GetString(4);
                }

                if (reader6.IsDBNull(5) == false)
                {
                    ViewBag.EndDateOn = String.Format("{0:dd/MM/yyyy hh:mmtt}", (DateTime)reader6.GetDateTime(5));
                }

                if (reader6.IsDBNull(6) == false)
                {
                    ViewBag.EndTime = reader6.GetString(6);
                }
                if (reader6.IsDBNull(7) == false)
                {
                    ViewBag.SequenceStart = reader6.GetString(7);
                }
                if (reader6.IsDBNull(8) == false)
                {
                    ViewBag.SequenceEnd = reader6.GetString(8);
                }

                if (reader6.IsDBNull(9) == false)
                {
                    ViewBag.Machine = reader6.GetString(9);
                }
                if (reader6.IsDBNull(10) == false)
                {
                    ViewBag.Recovery = reader6.GetString(10);
                }

                if (reader6.IsDBNull(11) == false)
                {
                    ViewBag.FileName = reader6.GetString(11);
                }
                if (reader6.IsDBNull(12) == false)
                {
                    ViewBag.PrintSlipNo = reader6.GetString(12);
                }
                if (reader6.IsDBNull(13) == false)
                {
                    ViewBag.AccQty = reader6.GetString(13);
                }
                if (reader6.IsDBNull(14) == false)
                {
                    ViewBag.ImpQty = reader6.GetString(14);
                }

                if (reader6.IsDBNull(15) == false)
                {
                    ViewBag.PageQty = reader6.GetString(15);
                }
                if (reader6.IsDBNull(16) == false)
                {
                    ViewBag.Status = reader6.GetString(16);
                }
                if (reader6.IsDBNull(17) == false)
                {
                    ViewBag.ProcessType = reader6.GetString(17);
                }



            }
            cn6.Close();
        }

        using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command3 = new SqlCommand("", cn2))
        {

            cn2.Open();
            command3.CommandText = @"SELECT InsInserting.Guid,InsInserting.Ins_StartDateOn, InsInserting.Ins_StartTime, InsInserting.Ins_EndDateOn, InsInserting.Ins_EndTime, InsInserting.Ins_Machine, InsInserting.Ins_Recovery, InsInserting.Sort, InsInserting.NonSort, JobInstruction.MMPSlip, InsInserting.Ins_ReturnO, InsInserting.Ins_CourierO
                                             FROM  JobInstruction INNER JOIN
                                              InsInserting ON JobInstruction.Id = InsInserting.Guid                              
                                               WHERE JobInstruction.Id=@Guid";
            command3.Parameters.AddWithValue("@Guid", Id);
            var reader3 = command3.ExecuteReader();
            while (reader3.Read())
            {
                if (reader3.IsDBNull(0) == false)
                {
                    ViewBag.Guid = reader3.GetGuid(0);
                }

                if (reader3.IsDBNull(1) == false)
                {
                    ViewBag.Ins_StartDateOn = String.Format("{0:dd/MM/yyyy}", reader3.GetDateTime(1));
                }
                if (reader3.IsDBNull(2) == false)
                {
                    ViewBag.Ins_StartTime = reader3.GetString(2);
                }
                if (reader3.IsDBNull(3) == false)
                {
                    ViewBag.Ins_EndDateOn = String.Format("{0:dd/MM/yyyy}", reader3.GetDateTime(3));
                }
                if (reader3.IsDBNull(4) == false)
                {
                    ViewBag.Ins_EndTime = reader3.GetString(4);
                }
                if (reader3.IsDBNull(5) == false)
                {
                    ViewBag.Ins_Machine = reader3.GetString(5);
                }

                if (reader3.IsDBNull(6) == false)
                {
                    ViewBag.Ins_Recovery = reader3.GetString(6);
                }

                if (reader3.IsDBNull(7) == false)
                {
                    bool getSort = reader3.GetBoolean(7);
                    if (getSort == false)
                    {
                        ViewBag.Sort = "";
                    }
                    else
                    {
                        ViewBag.Sort = "checked";
                    }
                }
                if (reader3.IsDBNull(8) == false)
                {
                    bool getNonSort = reader3.GetBoolean(8);
                    if (getNonSort == false)
                    {
                        ViewBag.NonSort = "";
                    }
                    else
                    {
                        ViewBag.NonSort = "checked";
                    }
                }
                if (reader3.IsDBNull(9) == false)
                {
                    ViewBag.MMPSlip = reader3.GetString(9);
                }
                if (reader3.IsDBNull(10) == false)
                {
                    ViewBag.ReturnO = reader3.GetString(10);
                }
                if (reader3.IsDBNull(11) == false)
                {
                    ViewBag.CourierO = reader3.GetString(11);
                }




            }
            cn2.Close();
        }

        if (set == "CreateProductionSlip")
        {


            if (!string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Ins_StartDateOn) && !string.IsNullOrEmpty(Ins_StartTime) && !string.IsNullOrEmpty(Ins_EndDateOn) && !string.IsNullOrEmpty(Ins_EndTime) && !string.IsNullOrEmpty(Ins_Machine) && ViewBag.MMPSlip == null)
            {

                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    Guid guidId = Guid.NewGuid();
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                    get.Ins_StartDateOn = Convert.ToDateTime(get.Ins_StartDateOnTxt);
                    get.Ins_EndDateOn = Convert.ToDateTime(get.Ins_EndDateOnTxt);


                    cn2.Open();
                    SqlCommand command2;
                    command2 = new SqlCommand("INSERT INTO [InsInserting] (Guid, ModifiedOn, Ins_StartDateOn, Ins_StartTime, Ins_EndDateOn, Ins_EndTime, Ins_Machine, Ins_Recovery, Sort, Ins_CreateUser, JobInstructionId,NonSort,Ins_ReturnO,Ins_CourierO) values (@Guid,@ModifiedOn,@Ins_StartDateOn, @Ins_StartTime,@Ins_EndDateOn,@Ins_EndTime,@Ins_Machine,@Ins_Recovery, @Sort, @Ins_CreateUser,@JobInstructionId,@NonSort,@Ins_ReturnO,@Ins_CourierO)", cn2);
                    command2.Parameters.AddWithValue("@Guid", Id);
                    command2.Parameters.AddWithValue("@ModifiedOn", createdOn);
                    if (!string.IsNullOrEmpty(Ins_StartDateOn))
                    {
                        string a3 = Convert.ToDateTime(Ins_StartDateOn).ToString("yyyy-MM-dd");
                        command2.Parameters.AddWithValue("@Ins_StartDateOn", a3);
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@Ins_StartDateOn", null);
                    }
                    command2.Parameters.AddWithValue("@Ins_StartTime", Ins_StartTime);
                    if (!string.IsNullOrEmpty(Ins_EndDateOn))
                    {
                        string a4 = Convert.ToDateTime(Ins_EndDateOn).ToString("yyyy-MM-dd");
                        command2.Parameters.AddWithValue("@Ins_EndDateOn", a4);
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@Ins_EndDateOn", null);
                    }
                    command2.Parameters.AddWithValue("@Ins_EndTime", Ins_EndTime);
                    command2.Parameters.AddWithValue("@Ins_Machine", Ins_Machine);
                    command2.Parameters.AddWithValue("@Ins_Recovery", Ins_Recovery);
                    if (Sort == "on")
                    {
                        command2.Parameters.AddWithValue("@Sort", true);
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@Sort", false);
                    }

                    command2.Parameters.AddWithValue("@Ins_CreateUser", IdentityName.ToString());
                    command2.Parameters.AddWithValue("@JobInstructionId", Id);
                    if (NonSort == "on")
                    {
                        command2.Parameters.AddWithValue("@NonSort", true);
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@NonSort", false);
                    }
                    command2.Parameters.AddWithValue("@Ins_ReturnO", ReturnO);
                    command2.Parameters.AddWithValue("@Ins_CourierO", CourierO);




                    command2.ExecuteNonQuery();
                    cn2.Close();

                }

                using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn1.Open();
                    SqlCommand command1;
                    command1 = new SqlCommand("UPDATE [JobInstruction] SET MMPSlip='CREATED' WHERE Id=@Id", cn1);
                    command1.Parameters.AddWithValue("@Id", Id);
                    command1.ExecuteNonQuery();
                    cn1.Close();
                }


                TempData["msg"] = "<script>alert('SUCCESSFULLY SUBMIT TO POSTING !');</script>";
                return RedirectToAction("CreateProdSlip", "MMP", new { Id = Id });


            }

            else if (!string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Ins_StartDateOn) && !string.IsNullOrEmpty(Ins_StartTime) && !string.IsNullOrEmpty(Ins_EndDateOn) && !string.IsNullOrEmpty(Ins_EndTime) && !string.IsNullOrEmpty(Ins_Machine) && ViewBag.MMPSlip != null)
            {
                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    Guid guidId = Guid.NewGuid();
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                    get.Ins_StartDateOn = Convert.ToDateTime(get.Ins_StartDateOnTxt);
                    get.Ins_EndDateOn = Convert.ToDateTime(get.Ins_EndDateOnTxt);


                    cn2.Open();
                    SqlCommand command2;
                    command2 = new SqlCommand("UPDATE [InsInserting] SET Ins_StartDateOn = @Ins_StartDateOn, Ins_StartTime = @Ins_StartTime, Ins_EndDateOn = @Ins_EndDateOn, Ins_EndTime = @Ins_EndTime, Ins_Machine = @Ins_Machine, Ins_Recovery = @Ins_Recovery, Sort = @Sort, Ins_CreateUser = @Ins_CreateUser, JobInstructionId = @JobInstructionId, NonSort = @NonSort, Ins_ReturnO = @Ins_ReturnO , Ins_CourierO = @Ins_CourierO WHERE Guid = @Guid", cn2);
                    command2.Parameters.AddWithValue("@Guid", Id);
                    if (!string.IsNullOrEmpty(Ins_StartDateOn))
                    {
                        string a3 = Convert.ToDateTime(Ins_StartDateOn).ToString("yyyy-MM-dd");
                        command2.Parameters.AddWithValue("@Ins_StartDateOn", a3);
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@Ins_StartDateOn", null);
                    }
                    command2.Parameters.AddWithValue("@Ins_StartTime", Ins_StartTime);
                    if (!string.IsNullOrEmpty(Ins_EndDateOn))
                    {
                        string a4 = Convert.ToDateTime(Ins_EndDateOn).ToString("yyyy-MM-dd");
                        command2.Parameters.AddWithValue("@Ins_EndDateOn", a4);
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@Ins_EndDateOn", null);
                    }
                    command2.Parameters.AddWithValue("@Ins_EndTime", Ins_EndTime);
                    command2.Parameters.AddWithValue("@Ins_Machine", Ins_Machine);
                    command2.Parameters.AddWithValue("@Ins_Recovery", Ins_Recovery);
                    if (Sort == "on")
                    {
                        command2.Parameters.AddWithValue("@Sort", true);
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@Sort", false);
                    }

                    command2.Parameters.AddWithValue("@Ins_CreateUser", IdentityName.ToString());
                    command2.Parameters.AddWithValue("@JobInstructionId", Id);
                    if (NonSort == "on")
                    {
                        command2.Parameters.AddWithValue("@NonSort", true);
                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@NonSort", false);
                    }
                    command2.Parameters.AddWithValue("@Ins_ReturnO", ReturnO);
                    command2.Parameters.AddWithValue("@Ins_CourierO", CourierO);



                    command2.ExecuteNonQuery();
                    cn2.Close();

                }

                return RedirectToAction("CreateProdSlip", "MMP", new { Id = Id });

            }





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
            return RedirectToAction("ManageProcessMMP", "MMP");
        }



        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT b.Id, a.ProductionSlipId
                                           FROM[ProductionSlip] a, [Hist_ProductionSlip] b
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
                                           FROM [Hist_ProductionSlip]                                    
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


    public ActionResult SubmitPosting(string Id, string JobInstructionId, string JobType, string set, List<JobInstruction> selectedRows)

    {
        var IdentityName = @Session["Fullname"];
        string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

        if (set == "NoSlip")
        {
            TempData["Message"] = "Slip is not created";
            return RedirectToAction("ManageMMPPickup", "MMP");
        }

        if (set == "BlastProdSlip")
        {
            foreach (var row in selectedRows)
            {
                string idAsString = row.Id.ToString();

                using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn1.Open();
                    SqlCommand command1;
                    command1 = new SqlCommand("UPDATE [JobInstruction] SET STATUS='POSTING' WHERE Id=@Id", cn1);
                    command1.Parameters.AddWithValue("@Id", idAsString);
                    command1.ExecuteNonQuery();
                    cn1.Close();
                }

                using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn1.Open();
                    SqlCommand command1;
                    command1 = new SqlCommand("UPDATE [InsInserting] SET InsInserting='POSTING' WHERE JobInstructionId=@JobInstructionId", cn1);
                    command1.Parameters.AddWithValue("@JobInstructionId", idAsString);
                    command1.ExecuteNonQuery();
                    cn1.Close();
                }
            }

            return RedirectToAction("ManageInsert", "Inserting");

        }

        else if (!string.IsNullOrEmpty(Id))
        {
            using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn1.Open();
                SqlCommand command1;
                command1 = new SqlCommand("UPDATE [JobInstruction] SET STATUS='POSTING' WHERE Id=@Id", cn1);
                command1.Parameters.AddWithValue("@Id", Id);
                command1.ExecuteNonQuery();
                cn1.Close();
            }

            using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn1.Open();
                SqlCommand command1;
                command1 = new SqlCommand("UPDATE [InsInserting] SET InsInserting='POSTING' WHERE JobInstructionId=@JobInstructionId", cn1);
                command1.Parameters.AddWithValue("@JobInstructionId", Id);
                command1.ExecuteNonQuery();
                cn1.Close();
            }
        }
        return RedirectToAction("ManageInsert", "Inserting");

    }
}


