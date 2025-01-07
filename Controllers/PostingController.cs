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
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Diagnostics;

[MvcAppV2.FilterConfig.UserSessionActionFilter]
public class PostingController : Controller
{
    string PathSource = System.Configuration.ConfigurationManager.AppSettings["SourceFile"];

    public Document doc { get; private set; }

    List<JobInstruction> viewPosting = new List<JobInstruction>();
    public ActionResult ManagePosting(string product, string set, string pageNumber, string LogTagNoSearch, string msg)
    {
        if (pageNumber == null)
        {
            pageNumber = "0";
        }

        Debug.WriteLine("Set : " + set);

        ViewBag.msg = msg;
        TempData["msg"] = "<script>alert('" + msg + "')</script>";

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
                //                            EngineeringNotes ,ArtworkNotes,Acc_BillingNotes,PostingSlip
                //                         FROM [JobInstruction]                                    
                //                         WHERE ProductName LIKE @ProductName OR JobSheetNo LIKE @ProductName
                //                         AND Status = 'POSTING'
                //                         ORDER BY CreatedOn DESC ";

                command.CommandText = @"SELECT Customer_Name, ProductName, JobClass,
                                            JobType,JobSheetNo, SUM(CAST(AccQty AS INT)) as AccQty, SUM(CAST(ImpQty AS INT)) as ImpQty, SUM(CAST(PageQty AS INT)) as PageQty,PrintSlip,LogTagNo,Status,PostingSlip, MAX(CreatedOn) AS CreatedOn, RemarkPosting
                                        FROM [JobAuditTrailDetail]
                                        WHERE Status = 'POSTING' AND LogTagNo LIKE @LogTagNo
                                        GROUP BY Customer_Name, ProductName, JobClass,JobType,JobSheetNo,PrintSlip,LogTagNo,Status,PostingSlip,RemarkPosting
                                        ORDER BY MAX(CreatedOn) ASC ";

                command.Parameters.AddWithValue("@LogTagNo", "%"+LogTagNoSearch+"%");
            }

            else if (set == "GoTo")
            {
                if (pageNumber == "0")
                {

                    command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass
                                            , JobType,JobSheetNo, StartDevDate, EndDevDate
                                            ,AccountsQty,ImpressionQty, PagesQty,IT_SysNotes
                                            ,Produc_PlanningNotes,PurchasingNotes,EngineeringNotes
                                         ,ArtworkNotes,Acc_BillingNotes,PostingSlip
                                        FROM [JobInstruction]
                                        WHERE Status = 'POSTING'
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
                                         ,ArtworkNotes,Acc_BillingNotes,PostingSlip
                                        FROM [JobInstruction]
                                        WHERE Status = 'POSTING'
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
                //                         ,ArtworkNotes,Acc_BillingNotes,PostingSlip
                //                        FROM [JobInstruction]
                //                        WHERE Status = 'POSTING'
                //                        ORDER BY (SELECT NULL)
                //                        OFFSET 0 ROWS
                //                        FETCH NEXT 100 ROWS ONLY";

                command.CommandText = @"SELECT Customer_Name, ProductName, JobClass,
                                            JobType,JobSheetNo, SUM(CAST(AccQty AS INT)) as AccQty, SUM(CAST(ImpQty AS INT)) as ImpQty, SUM(CAST(PageQty AS INT)) as PageQty,PrintSlip,LogTagNo,Status,PostingSlip, MAX(CreatedOn) AS CreatedOn,RemarkPosting
                                        FROM [JobAuditTrailDetail]
                                        WHERE Status = 'POSTING'
                                        GROUP BY Customer_Name, ProductName, JobClass,JobType,JobSheetNo,PrintSlip,LogTagNo,Status,PostingSlip,RemarkPosting
                                        ORDER BY MAX(CreatedOn) ASC ";
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
                    if (!reader.IsDBNull(10))
                    {
                        model.Status = reader.GetString(10);
                    }
                    if (!reader.IsDBNull(11))
                    {
                        model.PostingSlip = reader.GetString(11);
                    }
                    else
                    {
                        model.PostingSlip = "PENDING";

                    }
                    //if (reader.IsDBNull(11) == false)
                    //{
                    //    model.Id = reader.GetGuid(11);
                    //}
                    if (!reader.IsDBNull(12))
                    {
                        model.CreatedOn = reader["CreatedOn"].ToString();
                    }

                    if (!reader.IsDBNull(13))
                    {
                        model.Remarks = reader["RemarkPosting"].ToString();
                    }



                }
                JobInstructionlist1.Add(model);

            }


            //SqlCommand cmd1 = new SqlCommand("SELECT Customer_Name, ProductName, JobClass,JobType,JobSheetNo, SUM(CAST(AccQty AS INT)) as AccQty, SUM(CAST(ImpQty AS INT)) as ImpQty, SUM(CAST(PageQty AS INT)) as PageQty,PrintSlip,LogTagNo,Status " +
            //    "FROM [JobAuditTrailDetail] WHERE Status = 'POSTING' WHERE PrintSlip='CREATED' OR InsertSlip='CREATED' OR SMSlip = 'CREATED' OR MMPSlip = 'CREATED' GROUP BY Customer_Name, ProductName, JobClass,JobType,JobSheetNo,PrintSlip,LogTagNo,Status ORDER BY (SELECT NULL) " +
            //    "OFFSET 0 ROWS FETCH NEXT 100 ROWS ONLY", cn);
            //SqlDataReader rm1 = cmd1.ExecuteReader();

            //while()
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
                        command.CommandText = @"SELECT Id
                                          FROM [PostingManifest]
                                          WHERE Id=@Id";
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
                                    command3 = new SqlCommand("DELETE [PostingManifest] WHERE Id=@Id", cn3);
                                    command3.Parameters.AddWithValue("@Id", idAsString);
                                    command3.ExecuteNonQuery();
                                    cn3.Close();

                                }

                                using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                                {
                                    cn3.Open();
                                    SqlCommand command3;
                                    command3 = new SqlCommand("UPDATE [JobInstruction] SET PostingSlip=NULL WHERE Id=@Id", cn3);
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
                command.CommandText = @"SELECT Id
                                          FROM [PostingManifest]
                                          WHERE Id=@Id";
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
                            command3 = new SqlCommand("DELETE [PostingManifest] WHERE Id=@Id", cn3);
                            command3.Parameters.AddWithValue("@Id", Id);
                            command3.ExecuteNonQuery();
                            cn3.Close();

                        }

                        using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            cn3.Open();
                            SqlCommand command3;
                            command3 = new SqlCommand("UPDATE [JobInstruction] SET PostingSlip=NULL WHERE Id=@Id", cn3);
                            command3.Parameters.AddWithValue("@Id", Id);
                            command3.ExecuteNonQuery();
                            cn3.Close();

                        }

                    }


                }
                cn.Close();
            }
        }

        return RedirectToAction("ManagePosting", "Posting");
    }


    public ActionResult PostingDetails(string set, string Id, string Customer_Name, string ProductName, string JobClass,
                                       string AccQty, string PageQty, string ImpQty, string AccountsQty, string PagesQty, string ImpressionQty,
                                       string JobSheetNo, string LogTagNo, string PostingDateOn, string PostingTime, string Local, string Oversea,
                                       string Courier, string Re_turn, string ReturnSts, string Shred, string Hold, string PO_BOX, string Ins_Material,
                                       string InsertMMP, string Recovery, string RemarkRecovery, string RemarkIns_Material, string PostingManifestId)

    {

        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];
        ViewBag.IsDepart = @Session["Department"];
        ViewBag.AccountManager = IdentityName.ToString();
        ViewBag.PostingManifestId = PostingManifestId;
        ViewBag.AccountsQty = AccountsQty;
        ViewBag.PagesQty = PagesQty;
        ViewBag.ImpressionQty = ImpressionQty;
        ViewBag.PostingManifestId = PostingManifestId;
        Session["PostingManifestId"] = Id;
        Session["Id"] = Id;
        ViewBag.Id = Id;
        ViewBag.AccQty = AccQty;
        ViewBag.PageQty = PageQty;
        ViewBag.ImpQty = ImpQty;




        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {

            cn.Open();
            command.CommandText = @"SELECT  Id, Customer_Name, ProductName, JobClass,
                                            AccQty, PageQty, ImpQty, LogTagNo, JobSheetNo
                                            FROM [PostingManifest]                              
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
                    ViewBag.JobClass = reader.GetString(3);
                }
                if (reader.IsDBNull(4) == false)
                {
                    ViewBag.AccQty = reader.GetString(4);
                }
                if (reader.IsDBNull(5) == false)
                {
                    ViewBag.PageQty = reader.GetString(5);
                }
                if (reader.IsDBNull(6) == false)
                {
                    ViewBag.ImpQty = reader.GetString(6);
                }
                if (reader.IsDBNull(7) == false)
                {
                    ViewBag.LogTagNo = reader.GetString(7);
                }
                if (reader.IsDBNull(8) == false)
                {
                    ViewBag.JobSheetNo = reader.GetString(8);
                }


            }
            cn.Close();
        }



        return View();

    }

    public ActionResult DeletePosting(string Id, string LogTagNo, string JobSheetNo)
    {
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        {
            cn.Open();
            SqlCommand cmdDelete = new SqlCommand("DELETE FROM PostingManifest WHERE Id=@Id", cn);
            cmdDelete.Parameters.AddWithValue("@Id", Id);
            cmdDelete.ExecuteNonQuery();

            SqlCommand cmdUpdate = new SqlCommand("UPDATE JobAuditTrailDetail SET PostingSlip ='PENDING' WHERE LogTagNo=@LogTagNo", cn);
            cmdUpdate.Parameters.AddWithValue("LogTagNo", LogTagNo);
            cmdUpdate.ExecuteNonQuery();

            cn.Close();
        }

        return RedirectToAction("CreatePosting", "Posting", new { Id = Id, LogTagNo = LogTagNo, JobSheetNo = JobSheetNo });
    }


    public ActionResult CreatePosting(Hist_PostingManifest ModelSample, string Set, Hist_PostingManifest get, string Id, string Customer_Name, string ProductName, string JobClass,
                                       string AccQty, string PageQty, string ImpQty, string AccountsQty, string PagesQty, string ImpressionQty,
                                       string JobSheetNo, string LogTagNo, string PostingDateOn, string PostingTime, string Local, string Oversea,
                                       string Courier, string Re_turn, string ReturnSts, string Shred, string Hold, string PO_BOX, string Ins_Material,
                                       string InsertMMP, string Recovery, string RemarkRecovery, string RemarkIns_Material, string PostingManifestId, string CreateUser,
                                       string JobInstructionId, string Weight, string Rate, string gID, string NCR, string Balance, string CheckBalance)
    {
        //Debug.WriteLine("LogTagNo : " + LogTagNo);
        var IdentityName = @Session["Fullname"];
        var Ids = Session["Id"];

       

        ViewBag.IsDepart = @Session["Department"];
        ViewBag.IsRole = @Session["Role"];
        string Deptment = @Session["Department"].ToString();
        ViewBag.PostingManifestId = PostingManifestId;
        ViewBag.gID = gID;
        ViewBag.Id = Id;

        List<SelectListItem> listReturnStss = new List<SelectListItem>();

        listReturnStss.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        listReturnStss.Add(new SelectListItem { Text = "Customer Request", Value = "Customer Request" });
        listReturnStss.Add(new SelectListItem { Text = "Incomplete Address", Value = "Incomplete Address" });

        ViewData["ReturnSts_"] = listReturnStss;

        int _bil = 1;
        List<SelectListItem> li = new List<SelectListItem>();
        using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        {
            cn2.Open();




            using (SqlCommand command2 = new SqlCommand("", cn2))
            {
                //command2.CommandText = @"SELECT DISTINCT TypeMail FROM [MailPrice]                          
                //                     ORDER BY TypeMail";
                //var reader = command2.ExecuteReader();
                //while (reader.Read())
                //{
                //    FrankinReport model = new FrankinReport();
                //    {
                //        if (reader.IsDBNull(0) == false)
                //        {
                //            model.Local = reader.GetString(0);
                //        }
                //    }
                //    int i = _bil++;
                //    if (i == 1)
                //    {
                //        li.Add(new SelectListItem { Text = "Please Select" });
                //    }
                //    li.Add(new SelectListItem { Text = model.Local, Value = model.Local });
                //}
            }

            string AccQtyOri = "";
            string ImpQtyOri = "";
            string PageQtyOri = "";

            string AccQtySubmit = "";
            string ImpQtySubmit = "";
            string PageQtySubmit = "";


            SqlCommand cmd3 = new SqlCommand("SELECT SUM(CAST(AccQty AS INT)) as AccQty, SUM(CAST(ImpQty AS INT)) as ImpQty, SUM(CAST(PageQty AS INT)) as PageQty FROM JobAuditTrailDetail WHERE LogTagNo=@LogTagNo1", cn2);
            cmd3.Parameters.AddWithValue("@LogTagNo1", LogTagNo);
            SqlDataReader rm3 = cmd3.ExecuteReader();

            if (rm3.HasRows)
            {
                while (rm3.Read())
                {
                    AccQtyOri = rm3["AccQty"].ToString();
                    ImpQtyOri = rm3["ImpQty"].ToString();
                    PageQtyOri = rm3["PageQty"].ToString();
                }
            }

            SqlCommand cmd4 = new SqlCommand("SELECT SUM(CAST(AccQty AS INT)) as AccQty, SUM(CAST(ImpQty AS INT)) as ImpQty, SUM(CAST(PageQty AS INT)) as PageQty FROM JobAuditTrailDetail WHERE LogTagNo=@LogTagNo2 AND (PrintSlip='CREATED' OR InsertSlip='CREATED' OR SMSlip='CREATED' OR MMPSlip='CREATED') ", cn2);
            cmd4.Parameters.AddWithValue("@LogTagNo2", LogTagNo);
            SqlDataReader rm4 = cmd4.ExecuteReader();

            if (rm4.HasRows)
            {
                while (rm4.Read())
                {
                    AccQtySubmit = rm4["AccQty"].ToString();
                    ImpQtySubmit = rm4["ImpQty"].ToString();
                    PageQtySubmit = rm4["PageQty"].ToString();
                }
            }


            if ((AccQtyOri != AccQtySubmit) && (ImpQtyOri != ImpQtySubmit) && (PageQtyOri != PageQtySubmit))
            {
                TempData["Error"] = "<script>alert('Wait until all file in the logtag has been submitted')</script>";
                return RedirectToAction("ManagePosting", "Posting");
            }

            cn2.Close();
        }

        ViewData["Local_"] = li;



        ViewBag.Update = "Update";

        List<JobInstruction> JobInstructionlist1 = new List<JobInstruction>(); ;
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil3 = 1;
            cn.Open();
            command.CommandText = @"SELECT JobInstruction.Id, JobInstruction.Customer_Name, JobInstruction.ProductName, JobInstruction.JobClass, JobInstruction.JobType, JobInstruction.JobSheetNo, JobInstruction.StartDevDate, JobInstruction.EndDevDate, JobInstruction.JobRequest, SUM(CAST(JobAuditTrailDetail.AccQty AS INT)) as AccQty, SUM(CAST(JobAuditTrailDetail.ImpQty AS INT)) as ImpQty, SUM(CAST(JobAuditTrailDetail.PageQty AS INT)) as PageQty, JobAuditTrailDetail.LogTagNo
                                        FROM  JobInstruction FULL JOIN
                                        JobAuditTrailDetail ON JobInstruction.JobSheetNo = JobAuditTrailDetail.JobSheetNo
                                        WHERE JobAuditTrailDetail.LogTagNo =@Id 
                                        GROUP BY JobInstruction.Id, JobInstruction.Customer_Name, JobInstruction.ProductName, JobInstruction.JobClass, JobInstruction.JobType, JobInstruction.JobSheetNo, JobInstruction.StartDevDate, JobInstruction.EndDevDate, JobInstruction.JobRequest,JobAuditTrailDetail.LogTagNo";

            command.Parameters.AddWithValue("@Id", LogTagNo);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    model.Bil = _bil3++;
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
                        ViewBag.JobRequest = reader.GetDateTime(8);
                    }
                    if (reader.IsDBNull(9) == false)
                    {
                        ViewBag.AccQty = reader["AccQty"].ToString();
                        AccQty = reader["AccQty"].ToString();
                    }
                    if (reader.IsDBNull(10) == false)
                    {
                        ViewBag.ImpQty = reader["ImpQty"].ToString();
                    }
                    if (reader.IsDBNull(11) == false)
                    {
                        ViewBag.PagesQty = reader["PageQty"].ToString();
                    }
                    if (reader.IsDBNull(12) == false)
                    {
                        ViewBag.LogTagNo = reader.GetString(12);
                    }


                }
                JobInstructionlist1.Add(model);
            }
            cn.Close();
        }

        int storeBalance = 0;

        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        {
            cn.Open();

            using (SqlCommand command = new SqlCommand("", cn))
            {
                command.CommandText = @"SELECT PostingSlip,LogTagNo,JobSheetNo,PostingBalance
                            FROM  [JobAuditTrailDetail]
                            WHERE LogTagNo=@Id";

                command.Parameters.AddWithValue("@Id", LogTagNo);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        ViewBag.PostingSlip = reader.GetString(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        ViewBag.LogTagNo = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        ViewBag.JobSheetNo = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        ViewBag.Balance = reader["PostingBalance"].ToString();
                    }

                }
            }

            cn.Close();


        }

        int AccQtyInt = 0;

        try
        {
            AccQtyInt = Int32.Parse(AccQty);
        }
        catch
        {
            AccQtyInt = 0;
        }

        //Debug.WriteLine("Balance : " + (AccQtyInt - storeBalance).ToString());
        //Debug.WriteLine("Acc Qty : " + AccQty);

        List<PostingManifest> PostingMnfst = new List<PostingManifest>();

        //afif
        // modified by firdaus
        int bil = 1;
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT PostingDateOn, PostingTime, Local, Oversea, Re_turn, Courier, Recovery, PO_BOX, InsertMMP, Shred,
                                        Hold, RemarkIns_Material, Ins_Material, RemarkRecovery, Weight, Rate, ReturnSts, Id
                                        FROM  [PostingManifest]
                                        WHERE LogTagNo=@Id";

            command.Parameters.AddWithValue("@Id", LogTagNo);
            var reader = command.ExecuteReader();

            List<int> Total = new List<int>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    PostingManifest model = new PostingManifest();
                    {
                        model.Bil = bil++;
                        if (reader.IsDBNull(0) == false)
                        {
                            ViewBag.PostingDateOn = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(0));
                            model.PostingDateOn = reader.GetDateTime(0);
                        }

                        if (reader.IsDBNull(1) == false)
                        {
                            ViewBag.PostingTime = reader.GetString(1);
                            model.PostingTime = reader.GetString(1);
                        }

                        if (reader.IsDBNull(2) == false)
                        {
                            ViewBag.Local = reader.GetString(2);
                            model.Local = reader.GetString(2);
                            Total.Add(Int32.Parse(reader.GetString(2)));

                        }

                        if (reader.IsDBNull(3) == false)
                        {
                            ViewBag.Oversea = reader.GetString(3);
                            model.Oversea = reader.GetString(3);
                            Total.Add(Int32.Parse(reader.GetString(3)));


                        }

                        if (reader.IsDBNull(4) == false)
                        {
                            ViewBag.Re_turn = reader.GetString(4);
                            model.Re_turn = reader.GetString(4);
                            Total.Add(Int32.Parse(reader.GetString(4)));


                        }

                        if (reader.IsDBNull(5) == false)
                        {
                            ViewBag.Courier = reader.GetString(5);
                            model.Courier = reader.GetString(5);
                            Total.Add(Int32.Parse(reader.GetString(5)));


                        }
                        if (reader.IsDBNull(6) == false)
                        {
                            ViewBag.Recovery = reader.GetString(6);
                            model.Recovery = reader.GetString(6);
                            Total.Add(Int32.Parse(reader.GetString(6)));


                        }
                        if (reader.IsDBNull(7) == false)
                        {
                            ViewBag.PO_BOX = reader.GetString(7);
                            model.PO_BOX = reader.GetString(7);
                            Total.Add(Int32.Parse(reader.GetString(7)));


                        }

                        if (reader.IsDBNull(8) == false)
                        {
                            ViewBag.InsertMMP = reader.GetString(8);
                            model.InsertMMP = reader.GetString(8);
                            Total.Add(Int32.Parse(reader.GetString(8)));


                        }
                        if (reader.IsDBNull(9) == false)
                        {
                            ViewBag.Shred = reader.GetString(9);
                            model.Shred = reader.GetString(9);
                            Total.Add(Int32.Parse(reader.GetString(9)));


                        }
                        if (reader.IsDBNull(10) == false)
                        {
                            ViewBag.Hold = reader.GetString(10);
                            model.Hold = reader.GetString(10);
                            Total.Add(Int32.Parse(reader.GetString(10)));


                        }
                        if (reader.IsDBNull(11) == false)
                        {
                            ViewBag.RemarkIns_Material = reader.GetString(10);
                            model.RemarkIns_Material = reader.GetString(10);

                        }
                        if (reader.IsDBNull(12) == false)
                        {
                            bool getIns_Material = reader.GetBoolean(12);
                            if (getIns_Material == false)
                            {
                                ViewBag.Ins_Material = "";
                                model.Ins_Material = false;
                            }
                            else
                            {
                                ViewBag.Ins_Material = "checked";
                                model.Ins_Material = true;

                            }
                        }
                        if (reader.IsDBNull(13) == false)
                        {
                            ViewBag.RemarkRecovery = reader.GetString(13);
                            model.RemarkRecovery = reader.GetString(13);
                        }
                        if (reader.IsDBNull(14) == false && reader.IsDBNull(15) == false)
                        {
                            ViewBag.Weight = reader.GetString(14) + "," + reader.GetString(15);
                        }
                        if (reader.IsDBNull(16) == false)
                        {
                            ViewBag.ReturnSts = reader.GetString(16);
                            model.ReturnSts = reader.GetString(16);

                        }
                        if (reader.IsDBNull(17) == false)
                        {
                            model.Id = reader.GetGuid(17);

                        }
                    }
                    PostingMnfst.Add(model);

                    if (Total.Count > 0)
                    {
                        int totalBalance = 0;
                        try
                        {
                            totalBalance = Int32.Parse(AccQty) - Total.Sum();
                            ViewBag.Balance = totalBalance.ToString();
                        }
                        catch
                        {
                            AccQty = "0";
                            totalBalance = Int32.Parse(AccQty) - Total.Sum();

                            ViewBag.Balance = totalBalance.ToString();
                        }
                    }
                    else
                    {
                        ViewBag.Balance = AccQty;
                    }
                }

            }
            else
            {
                ViewBag.Balance = AccQty;
            }
            cn.Close();
        }


        if (Set == "CreatePosting")
        {
            string CreatedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
            get.CreatedOn = Convert.ToDateTime(CreatedOn);

            if (!string.IsNullOrEmpty(LogTagNo)/* && !string.IsNullOrEmpty(PostingDateOn) && !string.IsNullOrEmpty(PostingTime) && ViewBag.PostingSlip == null*/)
            {
                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    Guid guidId = Guid.NewGuid();
                    string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                    get.CreatedOn = Convert.ToDateTime(createdOn);

                    get.PostingDateOn = Convert.ToDateTime(get.PostingDateOnTxt);


                    //string createdDate2 = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                    //get.CreatedOn = Convert.ToDateTime(createdDate2);
                    //get.dateCreated = Convert.ToDateTime(createdDate2);

                    string Weight1 = "";
                    string Rate1 = "";

                    // Split the string by the comma
                    if (!string.IsNullOrEmpty(Weight))
                    {
                        string[] splitValues = Weight.Split(',');
                        // Assign the split values to Weight1 and Rate1
                        Weight1 = splitValues[0];
                        Rate1 = splitValues[1];
                    }

                    cn2.Open();
                    SqlCommand command;
                    command = new SqlCommand("INSERT INTO [PostingManifest] (Id, CreatedOn, PostingDateOn, PostingTime, RemarkIns_Material, Ins_Material, Local, Weight, Rate, Oversea, Courier, Re_turn, ReturnSts, Shred, Hold,NCR, InsertMMP, Recovery, RemarkRecovery, CreateUser, Status,LogTagNo) values (@Id, @CreatedOn,@PostingDateOn, @PostingTime, @RemarkIns_Material, @Ins_Material, @Local, @Weight, @Rate, @Oversea, @Courier, @Re_turn, @ReturnSts, @Shred, @Hold,@NCR, @InsertMMP, @Recovery, @RemarkRecovery, @CreateUser, @Status,@LogTagNo)", cn2);
                    command.Parameters.AddWithValue("@Id", guidId);
                    command.Parameters.AddWithValue("@CreatedOn", get.CreatedOn);

                    if (!string.IsNullOrEmpty(PostingTime))
                    {
                        command.Parameters.AddWithValue("@PostingTime", PostingTime);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@PostingTime", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(RemarkIns_Material))
                    {
                        command.Parameters.AddWithValue("@RemarkIns_Material", RemarkIns_Material);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RemarkIns_Material", DBNull.Value);
                    }


                    if (Ins_Material == "on")
                    {
                        command.Parameters.AddWithValue("@Ins_Material", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Ins_Material", false);
                    }
                    if (!string.IsNullOrEmpty(PostingDateOn))
                    {
                        string iiii = Convert.ToDateTime(PostingDateOn).ToString("yyyy-MM-dd");
                        command.Parameters.AddWithValue("@PostingDateOn", iiii);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@PostingDateOn", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(Local))
                    {
                        command.Parameters.AddWithValue("@Local", Local);

                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Local", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(Weight))
                    {
                        command.Parameters.AddWithValue("@Weight", Weight1);

                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Weight", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(Rate1))
                    {
                        command.Parameters.AddWithValue("@Rate", Rate1);

                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Rate", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(ReturnSts))
                    {
                        command.Parameters.AddWithValue("@ReturnSts", ReturnSts);

                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ReturnSts", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(Oversea))
                    {
                        command.Parameters.AddWithValue("@Oversea", Oversea);

                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Oversea", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(Courier))
                    {
                        command.Parameters.AddWithValue("@Courier", Courier);

                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Courier", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(Re_turn))
                    {
                        command.Parameters.AddWithValue("@Re_turn", Re_turn);

                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Re_turn", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(Shred))
                    {
                        command.Parameters.AddWithValue("@Shred", Shred);

                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Shred", DBNull.Value);
                    }


                    if (!string.IsNullOrEmpty(Hold))
                    {
                        command.Parameters.AddWithValue("@Hold", Hold);

                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Hold", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(NCR))
                    {
                        command.Parameters.AddWithValue("@NCR", NCR);

                    }
                    else
                    {
                        command.Parameters.AddWithValue("@NCR", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(InsertMMP))
                    {
                        command.Parameters.AddWithValue("@InsertMMP", InsertMMP);

                    }
                    else
                    {
                        command.Parameters.AddWithValue("@InsertMMP", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(Recovery))
                    {
                        command.Parameters.AddWithValue("@Recovery", Recovery);

                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Recovery", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(RemarkRecovery))
                    {
                        command.Parameters.AddWithValue("@RemarkRecovery", RemarkRecovery);

                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RemarkRecovery", DBNull.Value);
                    }

                    command.Parameters.AddWithValue("@CreateUser", IdentityName.ToString());
                    command.Parameters.AddWithValue("@Status", "Posting Complete");
                    command.Parameters.AddWithValue("@LogTagNo", LogTagNo);
                    command.ExecuteNonQuery();
                    cn2.Close();

                }

                using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn1.Open();

                    SqlCommand cmdUpdate = new SqlCommand("UPDATE JobAuditTrailDetail SET PostingBalance=@Balance WHERE LogTagNo=@LogTagNo1", cn1);
                    cmdUpdate.Parameters.AddWithValue("@Balance", Balance);
                    cmdUpdate.Parameters.AddWithValue("@LogTagNo1", LogTagNo);
                    cmdUpdate.ExecuteNonQuery();

                    SqlCommand cmdCheck = new SqlCommand("SELECT PostingBalance FROM JobAuditTrailDetail WHERE LogTagNo=@LogTagNo2", cn1);
                    cmdCheck.Parameters.AddWithValue("@LogTagNo2", LogTagNo);
                    SqlDataReader rmCheck = cmdCheck.ExecuteReader();

                    if (rmCheck.HasRows)
                    {
                        while (rmCheck.Read())
                        {
                            CheckBalance = rmCheck["PostingBalance"].ToString();
                        }
                    }

                    int checkbalanceint = 0;

                    try
                    {
                        checkbalanceint = Int32.Parse(CheckBalance);
                    }
                    catch
                    {
                        checkbalanceint = 0;
                    }

                    if (CheckBalance == "0" || checkbalanceint <= 0)
                    {
                        SqlCommand command1;
                        command1 = new SqlCommand("UPDATE [JobAuditTrailDetail] SET PostingSlip='CREATED' WHERE LogTagNo=@Id", cn1);
                        command1.Parameters.AddWithValue("@Id", LogTagNo);
                        command1.ExecuteNonQuery();
                    }
                    else
                    {
                        SqlCommand command1;
                        command1 = new SqlCommand("UPDATE [JobAuditTrailDetail] SET PostingSlip='PENDING' WHERE LogTagNo=@Id2", cn1);
                        command1.Parameters.AddWithValue("@Id2", LogTagNo);
                        command1.ExecuteNonQuery();
                    }

                    cn1.Close();
                }

                return RedirectToAction("CreatePosting", "Posting", new { Id = Id, LogTagNo = LogTagNo, JobSheetNo = JobSheetNo });

            }
            //else if (!string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(PostingDateOn) && !string.IsNullOrEmpty(PostingTime) && ViewBag.PostingSlip != null)
            //{
            //    // Split the string by the comma

            //    string Weight1 = "";
            //    string Rate1 = "";

            //    if (!string.IsNullOrEmpty(Weight))
            //    {
            //        string[] splitValues = Weight.Split(',');

            //        // Assign the split values to Weight1 and Rate1
            //        Weight1 = splitValues[0];
            //        Rate1 = splitValues[1];
            //    }


            //    using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            //    {
            //        string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
            //        get.PostingDateOn = Convert.ToDateTime(get.PostingDateOnTxt);

            //        cn2.Open();
            //        SqlCommand command;
            //        command = new SqlCommand("UPDATE [PostingManifest] SET ModifiedOn=@ModifiedOn,PostingDateOn=@PostingDateOn, PostingTime=@PostingTime, RemarkIns_Material=@RemarkIns_Material, Ins_Material=@Ins_Material, Local=@Local, Oversea=@Oversea, Courier=@Courier, Re_turn=@Re_turn, ReturnSts=@ReturnSts, Shred=@Shred, Hold=@Hold, InsertMMP=@InsertMMP, Recovery=@Recovery, RemarkRecovery=@RemarkRecovery, CreateUser=@CreateUser, Weight=@Weight, Rate=@Rate, NCR=@NCR WHERE LogTagNo=@IdNew", cn2);
            //        command.Parameters.AddWithValue("@IdNew", LogTagNo);
            //        command.Parameters.AddWithValue("@ModifiedOn", createdOn);
            //        if (!string.IsNullOrEmpty(PostingDateOn))
            //        {
            //            string iiii = Convert.ToDateTime(PostingDateOn).ToString("yyyy-MM-dd");
            //            command.Parameters.AddWithValue("@PostingDateOn", iiii);
            //        }
            //        else
            //        {
            //            command.Parameters.AddWithValue("@PostingDateOn", DBNull.Value);
            //        }

            //        if (!string.IsNullOrEmpty(PostingTime))
            //        {
            //            command.Parameters.AddWithValue("@PostingTime", PostingTime);

            //        }
            //        else
            //        {
            //            command.Parameters.AddWithValue("@PostingTime", DBNull.Value);
            //        }

            //        if (!string.IsNullOrEmpty(RemarkIns_Material))
            //        {
            //            command.Parameters.AddWithValue("@RemarkIns_Material", RemarkIns_Material);

            //        }
            //        else
            //        {
            //            command.Parameters.AddWithValue("@RemarkIns_Material", DBNull.Value);
            //        }

            //        if (Ins_Material == "on")
            //        {
            //            command.Parameters.AddWithValue("@Ins_Material", true);
            //        }
            //        else
            //        {
            //            command.Parameters.AddWithValue("@Ins_Material", false);
            //        }

            //        if (!string.IsNullOrEmpty(Local))
            //        {
            //            command.Parameters.AddWithValue("@Local", Local);

            //        }
            //        else
            //        {
            //            command.Parameters.AddWithValue("@Local", DBNull.Value);
            //        }

            //        if (!string.IsNullOrEmpty(Oversea))
            //        {
            //            command.Parameters.AddWithValue("@Oversea", Oversea);

            //        }
            //        else
            //        {
            //            command.Parameters.AddWithValue("@Oversea", DBNull.Value);
            //        }

            //        if (!string.IsNullOrEmpty(Courier))
            //        {
            //            command.Parameters.AddWithValue("@Courier", Courier);

            //        }
            //        else
            //        {
            //            command.Parameters.AddWithValue("@Courier", DBNull.Value);
            //        }

            //        if (!string.IsNullOrEmpty(Re_turn))
            //        {
            //            command.Parameters.AddWithValue("@Re_turn", Re_turn);

            //        }
            //        else
            //        {
            //            command.Parameters.AddWithValue("@Re_turn", DBNull.Value);
            //        }

            //        if (!string.IsNullOrEmpty(ReturnSts))
            //        {
            //            command.Parameters.AddWithValue("@ReturnSts", ReturnSts);

            //        }
            //        else
            //        {
            //            command.Parameters.AddWithValue("@ReturnSts", DBNull.Value);
            //        }

            //        if (!string.IsNullOrEmpty(Shred))
            //        {
            //            command.Parameters.AddWithValue("@Shred", Shred);

            //        }
            //        else
            //        {
            //            command.Parameters.AddWithValue("@Shred", DBNull.Value);
            //        }

            //        if (!string.IsNullOrEmpty(Hold))
            //        {
            //            command.Parameters.AddWithValue("@Hold", Hold);

            //        }
            //        else
            //        {
            //            command.Parameters.AddWithValue("@Hold", DBNull.Value);
            //        }

            //        if (!string.IsNullOrEmpty(NCR))
            //        {
            //            command.Parameters.AddWithValue("@NCR", NCR);

            //        }
            //        else
            //        {
            //            command.Parameters.AddWithValue("@NCR", DBNull.Value);
            //        }


            //        if (!string.IsNullOrEmpty(InsertMMP))
            //        {
            //            command.Parameters.AddWithValue("@InsertMMP", InsertMMP);

            //        }
            //        else
            //        {
            //            command.Parameters.AddWithValue("@InsertMMP", DBNull.Value);
            //        }

            //        if (!string.IsNullOrEmpty(Recovery))
            //        {
            //            command.Parameters.AddWithValue("@Recovery", Recovery);

            //        }
            //        else
            //        {
            //            command.Parameters.AddWithValue("@Recovery", DBNull.Value);
            //        }

            //        if (!string.IsNullOrEmpty(RemarkRecovery))
            //        {
            //            command.Parameters.AddWithValue("@RemarkRecovery", RemarkRecovery);

            //        }
            //        else
            //        {
            //            command.Parameters.AddWithValue("@RemarkRecovery", DBNull.Value);
            //        }

            //        command.Parameters.AddWithValue("@CreateUser", IdentityName.ToString());
            //        command.Parameters.AddWithValue("@Id", Id);
            //        command.Parameters.AddWithValue("@Weight", Weight1);
            //        command.Parameters.AddWithValue("@Rate", Rate1);
            //        command.ExecuteNonQuery();
            //        cn2.Close();

            //    }

            //    return RedirectToAction("CreatePosting", "Posting", new { Id = Id, LogTagNo = LogTagNo, JobSheetNo = JobSheetNo });

            //}

        }

        //return RedirectToAction("CreatePosting","Posting", new)
        return View(PostingMnfst);

    }


    List<Hist_ProductionSlip> viewSubmitProcess = new List<Hist_ProductionSlip>();

    public ActionResult ReloadPostingManifest()
    {


        List<Hist_PostingManifest> viewPosting = new List<Hist_PostingManifest>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _bil = 1;
            cn.Open();
            command.CommandText = @"SELECT Id, PostingDateOn, PostingTime, RemarkIns_Material, Ins_Material, Local, 
                                           Oversea, Courier, Re_turn, ReturnSts, Shred, Hold, PO_BOX, InsertMMP, Recovery,
                                           RemarkRecovery
                                      FROM [Hist_PostingManifest]  
                                      WHERE PostingManifestId=@Id AND Status='POSTING'                                  
                                      ORDER BY PostingDateOn";
            command.Parameters.AddWithValue("@Id", Session["PostingManifestId"].ToString());
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Hist_PostingManifest model = new Hist_PostingManifest();
                {
                    model.Bil = _bil++;
                    if (reader.IsDBNull(0) == false)
                    {
                        model.Id = reader.GetGuid(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        model.PostingDateOnTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(1));
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        model.PostingTime = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        model.RemarkIns_Material = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        model.Ins_Material = reader.GetBoolean(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        model.Local = reader.GetString(5);
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        model.Oversea = reader.GetString(6);
                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        model.Courier = reader.GetString(7);
                    }
                    if (reader.IsDBNull(8) == false)
                    {
                        model.Re_turn = reader.GetString(8);
                    }
                    if (reader.IsDBNull(9) == false)
                    {
                        model.ReturnSts = reader.GetString(9);
                    }
                    if (reader.IsDBNull(10) == false)
                    {
                        model.Shred = reader.GetString(10);
                    }
                    if (reader.IsDBNull(11) == false)
                    {
                        model.Hold = reader.GetString(11);
                    }
                    if (reader.IsDBNull(12) == false)
                    {
                        model.PO_BOX = reader.GetString(12);
                    }
                    if (reader.IsDBNull(13) == false)
                    {
                        model.InsertMMP = reader.GetString(13);
                    }
                    if (reader.IsDBNull(14) == false)
                    {
                        model.Recovery = reader.GetString(14);
                    }
                    if (reader.IsDBNull(15) == false)
                    {
                        model.RemarkRecovery = reader.GetString(15);
                    }

                }
                viewPosting.Add(model);
            }
            cn.Close();
            //return Json(new { data = viewFileStore }, JsonRequestBehavior.AllowGet);
            return Json(viewPosting);
        }


    }



    public ActionResult DeletePostingManifest(string Id, string PostingManifestId)
    {
        Session["Id"] = Id;
        ViewBag.Id = Id;
        ViewBag.PostingManifestId = PostingManifestId;


        if (Id != null)
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT Id,PostingManifestId
                                          FROM [Hist_PostingManifest]
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
                            command3 = new SqlCommand("DELETE [Hist_PostingManifest] WHERE Id=@Id", cn3);
                            command3.Parameters.AddWithValue("@Id", Id);
                            command3.ExecuteNonQuery();
                            cn3.Close();

                        }

                    }

                    if (reader.IsDBNull(1) == false)
                    {

                        return RedirectToAction("PostingDetails", "Posting", new { Id = Session["PostingManifestId"].ToString() });

                    }
                }
                cn.Close();
            }
        }

        return RedirectToAction("PostingDetails", "Posting", new { Id = Session["Id"].ToString() });
    }

    public ActionResult ReturnToSender(string Id, string LogTagNo)
    {
        using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        {
            cn1.Open();

            SqlCommand cmdCheck = new SqlCommand("SELECT Id FROM JobAuditTrailDetail WHERE LogTagNo=@LogTagNo1", cn1);
            cmdCheck.Parameters.AddWithValue("@logTagNo1", LogTagNo);
            SqlDataReader rmCheck = cmdCheck.ExecuteReader();

            while (rmCheck.Read())
            {
                SqlCommand cmdCheck2 = new SqlCommand("SELECT ProcessType FROM Hist_ProductionSlip WHERE ProductionSlipId=@Id1", cn1);
                cmdCheck2.Parameters.AddWithValue("@Id1", rmCheck.GetGuid(0));
                SqlDataReader rmCheck2 = cmdCheck2.ExecuteReader();

                while (rmCheck2.Read())
                {
                    SqlCommand cmdUpdate = new SqlCommand("UPDATE JobAuditTrailDetail SET Status=@Status WHERE Id=@Id2", cn1);
                    cmdUpdate.Parameters.AddWithValue("@Status", rmCheck2.GetString(0));
                    cmdUpdate.Parameters.AddWithValue("@Id2", rmCheck.GetGuid(0));
                    cmdUpdate.ExecuteNonQuery();
                }
            }

            cn1.Close();
        }

        return RedirectToAction("ManagePosting", "Posting", new { msg = "Return Success" });
    }


    [ValidateInput(false)]
    public ActionResult SubmitPM(string Id, string JobInstructionId, string JobType, string set, List<JobInstruction> selectedRows, string LogTagNo)
    {

        //if (set == "NoSlip")
        //{
        //    TempData["Message"] = "Slip is not created";
        //    return RedirectToAction("ManagePosting", "Posting");
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
        //            command1 = new SqlCommand("UPDATE [JobInstruction] SET STATUS='FINANCE', IsStatusFin='1' WHERE Id=@Id", cn1);
        //            command1.Parameters.AddWithValue("@Id", idAsString);
        //            command1.ExecuteNonQuery();
        //            cn1.Close();
        //        }

        //        using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        //        {
        //            cn1.Open();
        //            SqlCommand command1;
        //            command1 = new SqlCommand("UPDATE [PostingManifest] SET STATUS='FINANCE' WHERE Id=@Id", cn1);
        //            command1.Parameters.AddWithValue("@Id", idAsString);
        //            command1.ExecuteNonQuery();
        //            cn1.Close();
        //        }
        //    }

        //    return RedirectToAction("ManagePosting", "Posting");

        //}


        using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        {
            cn1.Open();
            //SqlCommand command1;
            //command1 = new SqlCommand("UPDATE [JobInstruction] SET STATUS='FINANCE', IsStatusFin='1' WHERE Id=@Id", cn1);
            //command1.Parameters.AddWithValue("@Id", Id);
            //command1.ExecuteNonQuery();

            SqlCommand cmdCheck1 = new SqlCommand("SELECT COUNT(LogTagNo) FROM JobAuditTrailDetail WHERE LogTagno = @LogTagNo1", cn1);
            cmdCheck1.Parameters.AddWithValue("@LogTagNo1", LogTagNo);
            SqlDataReader rmCheck1 = cmdCheck1.ExecuteReader();

            while (rmCheck1.Read())
            {
                if (rmCheck1.GetInt32(0) > 0)
                {
                    SqlCommand cmdCheck = new SqlCommand("SELECT COUNT(PostingSlip) FROM JobAuditTrailDetail WHERE LogTagNo = @LogTagNo AND PostingSlip='CREATED'", cn1);
                    cmdCheck.Parameters.AddWithValue("@LogTagNo", LogTagNo);
                    SqlDataReader rmCheck = cmdCheck.ExecuteReader();

                    while (rmCheck.Read())
                    {
                        if (rmCheck.GetInt32(0) > 0)
                        {
                            if (rmCheck.GetInt32(0) == rmCheck1.GetInt32(0))
                            {
                                SqlCommand command1;
                                command1 = new SqlCommand("UPDATE [JobAuditTrailDetail] SET STATUS='FINANCE', PostingSlip='CREATED' WHERE LogTagNo=@Id", cn1);
                                command1.Parameters.AddWithValue("@Id", LogTagNo);
                                command1.ExecuteNonQuery();

                                SqlCommand command2;
                                command2 = new SqlCommand("UPDATE [PostingManifest] SET STATUS='FINANCE' WHERE LogTagNo=@Id2", cn1);
                                command2.Parameters.AddWithValue("@Id2", LogTagNo);
                                command2.ExecuteNonQuery();
                            }
                            else
                            {
                                return RedirectToAction("ManagePosting", "Posting", new { msg = "Submit failed, please check all file has been filled before submitting" });
                            }
                        }
                        else
                        {
                            return RedirectToAction("ManagePosting", "Posting", new { msg = "Submit failed, please check all file has been filled before submitting" });
                        }
                    }


                }
                else
                {
                    return RedirectToAction("ManagePosting", "Posting", new { msg = "Submit failed, please check all file has been filled before submitting" });

                }

            }




            cn1.Close();
        }

        //using (SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        //{
        //    cn1.Open();

        //    cn1.Close();
        //}



        string Msg= "SUCCESSFULLY SUBMIT TO FINANCE!";


        return RedirectToAction("ManagePosting", "Posting", new {msg=Msg});
    }


    public ActionResult ViewMailFrankingPostingReport(string Id)
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
                                   FROM [ProgDevWorksheet]    
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
                                           FROM [ProgDevWorksheet]    
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
                                           FROM [ITO_NewProgram]
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
                                           FROM [ITO_NewProgram] 
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

        return new Rotativa.ViewAsPdf("ViewMailFrankingPostingReport", viewProgDevWorksheet)
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
                                           FROM[ProgDevWorksheet] b, [ITO_NewProgram] a
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


    public ActionResult ViewDoketPengeposanMelFrangki(string Id)
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
                                   FROM [ProgDevWorksheet]    
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

        List<ProgDevWorksheet> viewDoketPengeposanMelFrangki = new List<ProgDevWorksheet>();
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
                                           FROM [ProgDevWorksheet]    
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
                viewDoketPengeposanMelFrangki.Add(model);
            }
            cn2.Close();

        }



        using (SqlConnection cn3 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn3))
        {
            cn3.Open();
            command.CommandText = @"SELECT Activities,Duration,Charges, ProgDevWorksheetId
                                           FROM [ITO_NewProgram]
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
                                           FROM [ITO_NewProgram] 
                                           WHERE ProgDevWorksheetId=@ProgDevWorksheetId";
            command.Parameters.AddWithValue("@ProgDevWorksheetId", Id);
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

        List<MelFranki> listMailFranki = new List<MelFranki>();
        using (SqlConnection cn4 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn4))
        {
            int _bil = 1;
            cn4.Open();
            command.CommandText = @"SELECT Customer_Name,Application,MPAO_No,LTT_No,Zone,Class,Weight,Quantity,Rate,SubTotal,Total,JobInstructionId
                                           FROM [MailFrankingPosting] 
                                           WHERE JobInstructionId=@JobInstructionId";
            command.Parameters.AddWithValue("@JobInstructionId", Session["Id"].ToString());
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                listMailFranki.Add(new MelFranki
                {
                    Customer_Name = reader.IsDBNull(0) ? null : reader.GetString(0),
                    Application = reader.IsDBNull(1) ? null : reader.GetString(1),
                    MPAO_No = reader.IsDBNull(2) ? null : reader.GetString(2),
                    LTT_No = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Zone = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Class = reader.IsDBNull(5) ? null : reader.GetString(5),
                    Weight = reader.IsDBNull(6) ? null : reader.GetString(6),
                    Quantity = reader.IsDBNull(7) ? null : reader.GetString(7),
                    Rate = reader.IsDBNull(8) ? null : reader.GetString(8),
                    SubTotal = reader.IsDBNull(9) ? null : reader.GetString(9),
                    Total = reader.IsDBNull(10) ? null : reader.GetString(10),


                });
            }
            ViewBag.listMailFranki = listMailFranki;
            cn4.Close();
        }

        //-----------------------------------------

        ReloadDoketPengeposanMelFrangki(Id);

        return new Rotativa.ViewAsPdf("ViewDoketPengeposanMelFrangki", viewDoketPengeposanMelFrangki)
        {
            // FileName = flightPlan.ListingItemDetailsModel.FlightDetails + ".pdf",
            PageMargins = new Rotativa.Options.Margins(12, 12, 12, 12),
            PageOrientation = Rotativa.Options.Orientation.Landscape,
            //PageWidth = 210,
            //PageHeight = 297
        };
    }

    List<ProgDevWorksheet> viewDoketPengeposanMelFrangki = new List<ProgDevWorksheet>();
    private void ReloadDoketPengeposanMelFrangki(string Id)
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
                                           FROM[ProgDevWorksheet] b, [ITO_NewProgram] a
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

                viewDoketPengeposanMelFrangki.Add(model);
            }
            cn.Close();
        }
    }

    public ActionResult CreateMailFrankingReport(string Id, string Customer_Name, string Application, string MPAO_No, string LTT_No, string Zone, string Class, string set,
                                                 string Quantity, string Rate, string SubTotal, string Total, string Weight)
    {

        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];
        Session["Id"] = Id;
        ViewBag.Id = Id;


        ViewBag.IsDepart = @Session["Department"];
        ViewBag.AccountManager = IdentityName.ToString();

        int _bil = 1;
        List<SelectListItem> li = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT Customer_Name FROM [CustomerDetails]                          
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







        List<JobInstruction> JobInstructionlist1 = new List<JobInstruction>(); ;
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            int _Bil = 1;
            cn.Open();
            command.CommandText = @"SELECT Id, Customer_Name, ProductName, JobClass, JobType,JobSheetNo, StartDevDate, EndDevDate,JobRequest,AccountsQty,ImpressionQty,PagesQty
                                        FROM [JobInstruction]
                                        WHERE Id =@Id";

            command.Parameters.AddWithValue("@Id", Id);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
                {
                    model.Bil = _Bil++;
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
                        ViewBag.JobRequest = reader.GetDateTime(8);
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

                }
                JobInstructionlist1.Add(model);
            }
            cn.Close();

        }


        if (set == "AddNew")
        {



            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                Session["Id"] = Id;
                ViewBag.Id = Id;
                Guid guid = Guid.NewGuid();

                string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");


                cn.Open();
                SqlCommand command;
                command = new SqlCommand("INSERT INTO [MailFrankingPosting] (Id, CreatedOn, ModifiedOn, Customer_Name,Application,MPAO_No,LTT_No,Zone,Class,Weight,Quantity,Rate,SubTotal,Total,JobInstructionId) values (@Id, @CreatedOn, @ModifiedOn, @Customer_Name,@Application,@MPAO_No,@LTT_No,@Zone,@Class,@Weight,@Quantity,@Rate,@SubTotal,@Total,@JobInstructionId)", cn);
                command.Parameters.AddWithValue("@Id", guid);
                command.Parameters.AddWithValue("@CreatedOn", createdOn);
                command.Parameters.AddWithValue("@ModifiedOn", createdOn);
                if (Customer_Name != null)
                {
                    command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                }
                if (Application != null)
                {
                    command.Parameters.AddWithValue("@Application", Application);
                }
                if (MPAO_No != null)
                {
                    command.Parameters.AddWithValue("@MPAO_No", MPAO_No);
                }
                if (LTT_No != null)
                {
                    command.Parameters.AddWithValue("@LTT_No", LTT_No);
                }
                if (Zone != null)
                {
                    command.Parameters.AddWithValue("@Zone", Zone);
                }
                if (Class != null)
                {
                    command.Parameters.AddWithValue("@Class", Class);
                }
                if (Class != Weight)
                {
                    command.Parameters.AddWithValue("@Weight", Weight);
                }
                if (Quantity != null)
                {
                    command.Parameters.AddWithValue("@Quantity", Quantity);
                }
                if (Rate != null)
                {
                    command.Parameters.AddWithValue("@Rate", Rate);
                }
                if (SubTotal != null)
                {
                    command.Parameters.AddWithValue("@SubTotal", SubTotal);
                }
                if (Total != null)
                {
                    command.Parameters.AddWithValue("@Total", Total);
                }

                command.Parameters.AddWithValue("@JobInstructionId", Id);
                command.ExecuteNonQuery();
                cn.Close();

                return RedirectToAction("ManagePosting", "Posting");
            }


        }
        if (!string.IsNullOrEmpty(Id))

        {

            using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn2))
            {
                cn2.Open();
                command.CommandText = @"SELECT MailFrankingPosting.Application, MailFrankingPosting.MPAO_No, MailFrankingPosting.LTT_No, MailFrankingPosting.Zone, MailFrankingPosting.Class, MailFrankingPosting.Weight, MailFrankingPosting.Quantity, MailFrankingPosting.Rate, MailFrankingPosting.SubTotal, MailFrankingPosting.Total
                                        FROM  JobInstruction INNER JOIN
                                    MailFrankingPosting ON JobInstruction.Id = MailFrankingPosting.JobInstructionId
                                     Where MailFrankingPosting.JobInstructionId=@Id";
                command.Parameters.AddWithValue("@Id", Id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JobInstruction model = new JobInstruction();
                    model.Bil = _bil++;
                    if (reader.IsDBNull(0) == false)
                    {
                        ViewBag.Application = reader.GetString(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        ViewBag.MPAO_No = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        ViewBag.LTT_No = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        ViewBag.Zone = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        ViewBag.Class = reader.GetString(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        ViewBag.Weight = reader.GetString(5);
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        ViewBag.Quantity = reader.GetString(6);
                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        ViewBag.Rate = reader.GetString(7);
                    }
                    if (reader.IsDBNull(8) == false)
                    {
                        ViewBag.SubTotal = reader.GetString(8);
                    }
                    if (reader.IsDBNull(9) == false)
                    {
                        ViewBag.Total = reader.GetString(9);
                    }
                }

            }


        }


        return View();

    }

    public ActionResult StorePostagePrice(string set, string MailType, string TypeMail, string Weight, string Rate, string deleteID)
    {

        List<SelectListItem> ListMailType = new List<SelectListItem>();

        ListMailType.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
        ListMailType.Add(new SelectListItem { Text = "STANDARD MAIL", Value = "STANDARD MAIL" });
        ListMailType.Add(new SelectListItem { Text = "NON STANDARD MAIL", Value = "NON STANDARD MAIL" });


        ViewData["MailType_"] = ListMailType;

        if (!string.IsNullOrEmpty(deleteID))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"DELETE [MailPrice]                          
                                      WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", deleteID.ToString());
                command.ExecuteNonQuery();
                cn.Close();

            }

            List<ListPostagePrice> listPrices = new List<ListPostagePrice>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();

                command.CommandText = @"SELECT Id, TypeMail, Weight, Rate FROM [MailPrice]                          
                                    ORDER BY TypeMail asc";

                //cmd.Parameters.AddWithValue("@id", id.ToString());
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    listPrices.Add(new ListPostagePrice
                    {
                        Id = reader.GetGuid(0),
                        MailType = reader.IsDBNull(1) ? null : reader.GetString(1),
                        Weight = reader.IsDBNull(2) ? null : reader.GetString(2),
                        Rate = reader.IsDBNull(3) ? null : reader.GetString(3)
                    });
                }

                cn.Close();
            }
            ViewBag.TbleListPrice = listPrices;
            return View();
        }

        if (set == "AddNew")
        {
            var IdentityName = @Session["Fullname"];
            var Role = @Session["Role"];
            string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
            //ViewBag.IsDepart = @Session["Department"];
            //ViewBag.AccountManager = IdentityName.ToString();

            Guid guidId = Guid.NewGuid();
            ViewBag.MailType = MailType;

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();

                command.CommandText = @"INSERT INTO [MailPrice] (Id, CreatedOn, CreatedBy, TypeMail, Weight, Rate) 
                                    values (@Id, @CreatedOn, @CreatedBy, @TypeMail, @Weight, @Rate)";

                command.Parameters.AddWithValue("@Id", guidId);
                command.Parameters.AddWithValue("@CreatedOn", createdOn);
                command.Parameters.AddWithValue("@CreatedBy", IdentityName);
                command.Parameters.AddWithValue("@TypeMail", MailType);
                command.Parameters.AddWithValue("@Weight", Weight);
                command.Parameters.AddWithValue("@Rate", Rate);

                command.ExecuteNonQuery();
                cn.Close();
            }
        }
        List<ListPostagePrice> listPrice = new List<ListPostagePrice>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();

            command.CommandText = @"SELECT Id, TypeMail, Weight, Rate FROM [MailPrice]                          
                                    ORDER BY TypeMail asc";

            //cmd.Parameters.AddWithValue("@id", id.ToString());
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                listPrice.Add(new ListPostagePrice
                {
                    Id = reader.GetGuid(0),
                    MailType = reader.IsDBNull(1) ? null : reader.GetString(1),
                    Weight = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Rate = reader.IsDBNull(3) ? null : reader.GetString(3)
                });
            }

            cn.Close();
        }
        ViewBag.TbleListPrice = listPrice;

        return View();
    }

    public ActionResult Local(String Local)
    {
        String temp = "0";
        int _bildd = 1;
        string Weight = null, Rate = null;


        List<SelectListItem> li2 = new List<SelectListItem>();
        li2.Clear();
        using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command2 = new SqlCommand("", cn2))
        {
            cn2.Open();
            command2.CommandText = @"SELECT DISTINCT Weight,Rate FROM [MailPrice]                          
                                      WHERE TypeMail = @TypeMail";
            command2.Parameters.AddWithValue("@TypeMail", Local);
            var reader = command2.ExecuteReader();
            while (reader.Read())
            {
                if (reader.IsDBNull(0) == false)
                {
                    Weight = reader.GetString(0);
                    Rate = reader.GetString(1);
                }

                if (_bildd == 1)
                {
                    li2.Add(new SelectListItem { Text = "Please Select" });
                    li2.Add(new SelectListItem { Text = $"{Weight},{Rate}" });
                }
                else
                {
                    li2.Add(new SelectListItem { Text = $"{Weight},{Rate}" });
                }

                _bildd++;
            }
            cn2.Close();
        }
        return Json(new { data = li2 });
    }


    public ActionResult Weight(String Weight)
    {
        String temp = "0";
        int _bildd = 1;
        List<SelectListItem> li2 = new List<SelectListItem>();
        li2.Clear();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT rate FROM [MailPrice]                          
                                      WHERE Weight = @Weight";
            command.Parameters.AddWithValue("@Weight", Weight.ToString());


            //
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                FrankinReport model = new FrankinReport();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.Rate = reader.GetString(0);
                    }
                }
                //int i = _bildd++;
                //if (i == 1)
                //{
                //   // li2.Add(new SelectListItem { Text = "Please Select" });
                //    li2.Add(new SelectListItem { Text = model.Rate });

                //}
                //else
                //{
                //    li2.Add(new SelectListItem { Text = model.Weight });
                //}
                //  ViewBag.Weight = model.Rate;
                temp = model.Rate;
            }
            cn.Close();
        }
        return Json(new { data = temp });
    }

    List<ListManagePostingDetail> ViewManagePostingDetail = new List<ListManagePostingDetail>();
    public ActionResult ManagePostingDetail(ListManagePostingDetail get, string Id, string gID, string set)
    {

        ViewBag.gID = gID;
        ViewBag.Id = Id;


        if (set == "Delete")
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"Delete [PostingManifest]                                         
                                        WHERE Id= @Id";
                command.Parameters.AddWithValue("@Id", gID);
                command.ExecuteNonQuery();
                cn.Close();
            }
        }


        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {

            cn.Open();
            command.CommandText = @"SELECT JobInstructionId,CreatedOn,Oversea,Re_turn,Courier,Recovery, Id, Local, Weight, Rate
                                   FROM [PostingManifest]                                         
                                   WHERE JobInstructionId = @Id
                                   ORDER BY CreatedOn";

            int _bil = 1;
            command.Parameters.AddWithValue("@Id", Id);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                ListManagePostingDetail model = new ListManagePostingDetail();
                {
                    model.Bil = _bil++;
                    if (reader.IsDBNull(0) == false)
                    {
                        model.Id = reader.GetGuid(0);
                    }
                    if (reader.IsDBNull(1) == false)
                    {
                        model.CreatedOnTxt = String.Format("{0:dd/MM/yyyy hh:mm:ss tt}", (DateTime)reader.GetDateTime(1));
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        model.Oversea = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        model.Re_turn = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        model.Courier = reader.GetString(4);
                    }
                    if (reader.IsDBNull(5) == false)
                    {
                        model.Recovery = reader.GetString(5);
                    }
                    if (reader.IsDBNull(6) == false)
                    {
                        model.gID = reader.GetGuid(6);
                    }
                    if (reader.IsDBNull(7) == false)
                    {
                        model.Local = reader.GetString(7);
                    }
                    if (reader.IsDBNull(8) == false)
                    {
                        model.Weight = reader.GetString(8);
                    }
                    if (reader.IsDBNull(9) == false)
                    {
                        model.Rate = reader.GetString(9);
                    }
                }
                ViewManagePostingDetail.Add(model);
            }
            cn.Close();
        }
        return View(ViewManagePostingDetail);
    }

}





