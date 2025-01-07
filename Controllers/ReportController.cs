using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcAppV2.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using MvcAppV2.Controllers;
using System.Web.Helpers;
using System.IO;
using Rotativa.Options;
using System.Text.RegularExpressions;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.Diagnostics;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Data.Metadata.Edm;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System.Security.Cryptography;
using OfficeOpenXml.ConditionalFormatting;
using System.Reflection;
using Microsoft.SqlServer.Server;

namespace MvcAppV2.Controllers
{
    public class ReportController : Controller
    {
        string PathSource = System.Configuration.ConfigurationManager.AppSettings["SourceFile"];
        string IpSMtp_ = System.Configuration.ConfigurationManager.AppSettings["IpSMtp"];
        string PortSmtp_ = System.Configuration.ConfigurationManager.AppSettings["PortSmtp"];

        //
        // GET: /Report/
        List<JobAuditTrailDetail> viewJI = new List<JobAuditTrailDetail>();
        public ActionResult ListReportFin(string set, string Customer_Name)
        {
            ViewBag.IsDepart = @Session["Department"];
            ViewBag.IsRole = @Session["Role"];
            var Role = @Session["Role"];

            if (set == "search")
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn.Open();

                    using (SqlCommand command = new SqlCommand("", cn))
                    {
                        int _bil = 1;
                        command.CommandText = @"SELECT JobInstruction.Customer_Name, JobInstruction.ProductName, JobInstruction.Status, JobAuditTrailDetail.AccQty, JobAuditTrailDetail.ImpQty, JobAuditTrailDetail.PageQty, JobAuditTrailDetail.ModeLog, JobAuditTrailDetail.JobClass,JobAuditTrailDetail.LogTagNo
                                            FROM  JobInstruction INNER JOIN
                                            JobAuditTrailDetail ON JobInstruction.Id = JobAuditTrailDetail.JobAuditTrailId";
                        command.Parameters.AddWithValue("@Customer_Name", "%" + Customer_Name + "%");
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
                                    model.Status = reader.GetString(2);
                                }
                                if (reader.IsDBNull(3) == false)
                                {
                                    model.AccQty = reader.GetString(3);
                                }
                                if (reader.IsDBNull(4) == false)
                                {
                                    model.ImpQty = reader.GetString(4);
                                }
                                if (reader.IsDBNull(5) == false)
                                {
                                    model.PageQty = reader.GetString(5);
                                }
                                if (reader.IsDBNull(6) == false)
                                {
                                    model.ModeLog = reader.GetString(6);
                                }
                                if (reader.IsDBNull(7) == false)
                                {
                                    model.JobClass = reader.GetString(7);
                                }
                                if (reader.IsDBNull(8) == false)
                                {
                                    model.LogTagNo = reader.GetString(8);
                                }


                            }
                            viewJI.Add(model);
                        }
                    }




                }
            }

            else if (set == "search2")
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    int _bil = 1;
                    cn.Open();
                    command.CommandText = @"SELECT JobInstruction.Customer_Name, JobInstruction.ProductName, JobInstruction.Status, JobAuditTrailDetail.AccQty, JobAuditTrailDetail.ImpQty, JobAuditTrailDetail.PageQty, JobAuditTrailDetail.ModeLog, JobAuditTrailDetail.JobClass,JobAuditTrailDetail.LogTagNo
                                            FROM  JobInstruction INNER JOIN
                                            JobAuditTrailDetail ON JobInstruction.Id = JobAuditTrailDetail.JobAuditTrailId
                                            WHERE JobInstruction.Customer_Name LIKE @Customer_Name AND JobInstruction.Status='FINANCE'";
                    command.Parameters.AddWithValue("@Customer_Name", "%" + Customer_Name + "%");
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
                                model.Status = reader.GetString(2);
                            }
                            if (reader.IsDBNull(3) == false)
                            {
                                model.AccQty = reader.GetString(3);
                            }
                            if (reader.IsDBNull(4) == false)
                            {
                                model.ImpQty = reader.GetString(4);
                            }
                            if (reader.IsDBNull(5) == false)
                            {
                                model.PageQty = reader.GetString(5);
                            }
                            if (reader.IsDBNull(6) == false)
                            {
                                model.ModeLog = reader.GetString(6);
                            }
                            if (reader.IsDBNull(7) == false)
                            {
                                model.JobClass = reader.GetString(7);
                            }
                            if (reader.IsDBNull(8) == false)
                            {
                                model.LogTagNo = reader.GetString(8);
                            }
                        }
                        viewJI.Add(model);
                    }
                    cn.Close();
                }
            }

            else
            {
                //ALL
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn.Open();
                    using (SqlCommand command = new SqlCommand("", cn))
                    {
                        int _bil = 1;
                        command.CommandText = @"SELECT JobInstruction.Customer_Name, JobInstruction.ProductName, JobInstruction.Status, JobAuditTrailDetail.AccQty, JobAuditTrailDetail.ImpQty, JobAuditTrailDetail.PageQty, JobAuditTrailDetail.ModeLog, JobAuditTrailDetail.JobClass,JobAuditTrailDetail.LogTagNo
                                            FROM  JobInstruction INNER JOIN
                                            JobAuditTrailDetail ON JobInstruction.Id = JobAuditTrailDetail.JobAuditTrailId
                                             WHERE JobInstruction.Status='FINANCE'";
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
                                    model.Status = reader.GetString(2);
                                }
                                if (reader.IsDBNull(3) == false)
                                {
                                    model.AccQty = reader.GetString(3);
                                }
                                if (reader.IsDBNull(4) == false)
                                {
                                    model.ImpQty = reader.GetString(4);
                                }
                                if (reader.IsDBNull(5) == false)
                                {
                                    model.PageQty = reader.GetString(5);
                                }
                                if (reader.IsDBNull(6) == false)
                                {
                                    model.ModeLog = reader.GetString(6);
                                }
                                if (reader.IsDBNull(7) == false)
                                {
                                    model.JobClass = reader.GetString(7);
                                }
                                if (reader.IsDBNull(8) == false)
                                {
                                    model.LogTagNo = reader.GetString(8);
                                }
                            }
                            viewJI.Add(model);
                        }
                    }

                    List<string> CustomerList = new List<string>();

                    SqlCommand GetCustomer = new SqlCommand("SELECT DISTINCT Customer_Name FROM CustomerDetails", cn);
                    SqlDataReader rmGetCustomer = GetCustomer.ExecuteReader();

                    if (rmGetCustomer.HasRows)
                    {
                        while (rmGetCustomer.Read())
                        {
                            CustomerList.Add(rmGetCustomer.GetString(0));
                        }
                    }

                    ViewBag.CustomerList = CustomerList;

                    cn.Close();

                }



            }

            return View(viewJI);
        }

        [HttpPost]
        public ActionResult getReport(string id, string DateStartTxt, string DateEndTxt, string Customer_Name, string ProductName)
        {
            ViewBag.IsDepart = @Session["Department"];
            ViewBag.IsRole = @Session["Role"];


            if (!string.IsNullOrEmpty(Customer_Name))
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn.Open();
                    if (!string.IsNullOrEmpty(DateStartTxt))
                    {
                        DateTime DateStart = Convert.ToDateTime(DateStartTxt);
                        var DateStart1 = DateStart.ToString("yyyy-MM-dd");
                        //DateStartTxt = DateStart.ToString("ddMMyyyy");
                    }

                    if (!string.IsNullOrEmpty(DateEndTxt))
                    {
                        DateTime DateEnd = Convert.ToDateTime(DateEndTxt);
                        var DateEnd1 = DateEnd.ToString("yyyy-MM-dd");
                        //DateEndTxt = DateEnd.ToString("ddMMyyyy");
                    }

                    List<Finance> ViewList = new List<Finance>();
                    string command = "";

                    if (ProductName != "Please Select")
                    {
                        //command = "SELECT A.Customer_Name, A.ProductName, A.LogTagNo, SUM(CAST(A.AccQty AS INT)) as AccQty, SUM(CAST(A.ImpQty AS INT)) as ImpQty, SUM(CAST(A.PageQty AS INT)) as PageQty, MAX(A.RevStrtDateOn), MAX(B.PostingDateOn), MAX(A.RevStrtTime), MAX(D.StartDateOn), MAX(E.PostingDateOn) " +
                        //"FROM JobAuditTrailDetail as A " +
                        //"LEFT JOIN PostingManifest as B ON A.JobSheetNo=B.JobSheetNo " +
                        //"INNER JOIN BillingMPR as C ON C.JobSheetNo=A.LogTagNo " +
                        //"LEFT JOIN ProductionSlip as D ON D.LogTagNo = A.LogTagNo " +
                        //"LEFT JOIN PostingManifest as E ON E.LogTagNo = A.LogTagNo " +
                        //"WHERE A.Customer_Name=@custName AND A.ProductName=@ProdName AND " +
                        //"LEFT( CONVERT(varchar, A.CreatedOn, 120), 10) >= @dateStart AND LEFT( CONVERT(varchar, C.CreatedOn, 120), 10) <= @dateEnd " +
                        //"GROUP BY A.Customer_Name, A.ProductName, A.LogTagNo " +
                        //"ORDER BY LogTagNo DESC";

                        command = @"SELECT A.Customer_Name, A.ProductName, A.LogTagNo, SUM(CAST(A.AccQty AS INT)) as AccQty, SUM(CAST(A.ImpQty AS INT)) as ImpQty, SUM(CAST(A.PageQty AS INT)) as PageQty, MAX(A.RevStrtDateOn), MAX(B.PostingDateOn), MAX(A.RevStrtTime), MAX(D.StartDateOn), MAX(E.PostingDateOn) 
                                    FROM JobAuditTrailDetail as A 
                                    FULL JOIN PostingManifest as B ON A.JobSheetNo=B.JobSheetNo 
                                    FULL JOIN BillingMPR as C ON C.JobSheetNo=A.LogTagNo 
                                    FULL JOIN ProductionSlip as D ON D.LogTagNo = A.LogTagNo 
                                    FULL JOIN PostingManifest as E ON E.LogTagNo = A.LogTagNo 
                                    WHERE A.Customer_Name= @CustName AND A.ProductName LIKE @ProdName AND (A.CreatedOn BETWEEN @dateStart AND @dateEnd) AND A.Status != 'PROCESSING'
                                    GROUP BY A.Customer_Name, A.ProductName, A.LogTagNo 
                                    ORDER BY A.LogTagNo DESC";


                    }
                    else
                    {
                        //command = "SELECT A.Customer_Name, A.ProductName, A.LogTagNo, SUM(CAST(A.AccQty AS INT)) as AccQty, SUM(CAST(A.ImpQty AS INT)) as ImpQty, SUM(CAST(A.PageQty AS INT)) as PageQty, MAX(A.RevStrtDateOn), MAX(B.PostingDateOn), MAX(A.RevStrtTime), MAX(D.StartDateOn), MAX(E.PostingDateOn) " +
                        //"FROM JobAuditTrailDetail as A " +
                        //"LEFT JOIN PostingManifest as B ON A.JobSheetNo=B.JobSheetNo " +
                        //"INNER JOIN BillingMPR as C ON C.JobSheetNo=A.LogTagNo " +
                        //"LEFT JOIN ProductionSlip as D ON D.LogTagNo = A.LogTagNo " +
                        //"LEFT JOIN PostingManifest as E ON E.LogTagNo = A.LogTagNo " +
                        //"WHERE A.Customer_Name=@custName AND " +
                        //"LEFT( CONVERT(varchar, A.CreatedOn, 120), 10) >= @dateStart AND LEFT( CONVERT(varchar, C.CreatedOn, 120), 10) <= @dateEnd " +
                        //"GROUP BY A.Customer_Name, A.ProductName, A.LogTagNo " +
                        //"ORDER BY A.LogTagNo DESC";

                        command = @"SELECT A.Customer_Name, A.ProductName, A.LogTagNo, SUM(CAST(A.AccQty AS INT)) as AccQty, SUM(CAST(A.ImpQty AS INT)) as ImpQty, SUM(CAST(A.PageQty AS INT)) as PageQty, MAX(A.RevStrtDateOn), MAX(B.PostingDateOn), MAX(A.RevStrtTime), MAX(D.StartDateOn), MAX(E.PostingDateOn) 
                                    FROM JobAuditTrailDetail as A 
                                    FULL JOIN PostingManifest as B ON A.JobSheetNo=B.JobSheetNo 
                                    FULL JOIN BillingMPR as C ON C.JobSheetNo=A.LogTagNo 
                                    FULL JOIN ProductionSlip as D ON D.LogTagNo = A.LogTagNo 
                                    FULL JOIN PostingManifest as E ON E.LogTagNo = A.LogTagNo 
                                    WHERE A.Customer_Name= @CustName AND (A.CreatedOn BETWEEN @dateStart AND @dateEnd) AND A.Status != 'PROCESSING'
                                    GROUP BY A.Customer_Name, A.ProductName, A.LogTagNo 
                                    ORDER BY A.LogTagNo DESC";
                    }

                    SqlCommand cmd1 = new SqlCommand(command, cn);
                    //Debug.WriteLine("Product Name : " + ProductName);
                    cmd1.Parameters.AddWithValue("@CustName", Customer_Name);
                    if (ProductName != "Please Select")
                    {
                        Debug.WriteLine("productname ada");
                        cmd1.Parameters.AddWithValue("@ProdName", "%"+ProductName+"%");
                    }
                    //Debug.WriteLine("Date Start : " + DateStartTxt);
                    //Debug.WriteLine("Date End : " + DateEndTxt);
                    cmd1.Parameters.AddWithValue("@dateStart", SqlDbType.DateTime).Value = DateStartTxt;
                    cmd1.Parameters.AddWithValue("@dateEnd", SqlDbType.DateTime).Value = DateEndTxt;

                    SqlDataReader rm1 = cmd1.ExecuteReader();

                    string CollectionDate = "";
                    string CollectionTime = "";
                    string PostingDate = "";

                    while (rm1.Read())
                    {
                        List<double> ServiceCharges = getServiceCharges(rm1.GetString(2));
                        List<string> PrintedPost = getPrintedPostDate(rm1.GetString(2));

                        if (!rm1.IsDBNull(6))
                        {
                            CollectionDate = rm1.GetDateTime(6).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            CollectionDate = "-";
                        }

                        if (!rm1.IsDBNull(7))
                        {
                            PostingDate = rm1.GetDateTime(7).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            PostingDate = "-";
                        }

                        if (!rm1.IsDBNull(8))
                        {
                            CollectionTime = rm1.GetString(8);
                        }
                        else
                        {
                            CollectionTime = "-";
                        }

                        List<string> MateriaCharges = getMaterialCharges(rm1.GetString(2));
                        Finance List1 = new Finance();
                        {

                            List1.Customer_Name = rm1.GetString(0);
                            List1.ProductName = rm1.GetString(1);
                            List1.LogTagNo = rm1.GetString(2);
                            List1.PageQty = rm1["PageQty"].ToString();
                            List1.ImpQty = rm1["ImpQty"].ToString();
                            List1.AccQty = rm1["AccQty"].ToString();
                            List1.RevStrtDateOnTxt = CollectionDate;
                            List1.PostingDateOnTxt = PostingDate;
                            List1.RevStrtTime = CollectionTime;
                            if (!string.IsNullOrEmpty(MateriaCharges[0]))
                            {
                                List1.Paper = MateriaCharges[0];
                            }
                            else
                            {
                                List1.Paper = "0.00";

                            }

                            if (!string.IsNullOrEmpty(MateriaCharges[1]) )
                            {
                                List1.Env = MateriaCharges[1];
                            }
                            else
                            {
                                List1.Env = "0.00";

                            }
                            List1.Postage = getPostage(rm1.GetString(2));
                            List1.RegistredMails = getRegisteredMails(rm1.GetString(2));
                            List1.Franking = getFranking(rm1.GetString(2));
                            if(!string.IsNullOrEmpty(getProgrammingCharges(rm1.GetString(2))))
                            {
                                List1.TotalAmountO3 = getProgrammingCharges(rm1.GetString(2));
                            }
                            else
                            {
                                List1.TotalAmountO3 = "0.00";
                            }

                            if(!string.IsNullOrEmpty(getRebate(rm1.GetString(2))))
                            {
                                List1.Rebate = getRebate(rm1.GetString(2));
                            }
                            else
                            {
                                List1.Rebate = "0.00";

                            }
                            List1.DatePrintedTxt = PrintedPost[0];
                            List1.DatePostingTxt = PrintedPost[1];
                            List1.Persent10 = get10Percent(rm1.GetString(2));

                            if (!rm1.IsDBNull(9))
                            {
                                List1.StartDateOnTxt = rm1.GetDateTime(9).ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                List1.StartDateOnTxt = "-";
                            }

                            if (!rm1.IsDBNull(10))
                            {
                                List1.PostingDateOnTxt = rm1.GetDateTime(10).ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                List1.PostingDateOnTxt = "-";
                            }

                            List1.ServiceChargesDouble = ServiceCharges.Take(ServiceCharges.Count-1).Sum();
                            List1.Sst = ServiceCharges.Last().ToString();

                        }

                        ViewList.Add(List1);
                    }

                    //foreach(var x in ViewList)
                    //{
                    //    Debug.WriteLine(x.Customer_Name);
                    //    Debug.WriteLine(x.ProductName);
                    //    Debug.WriteLine(x.LogTagNo);
                    //    Debug.WriteLine(x.PageQty);
                    //    Debug.WriteLine(x.ImpQty);
                    //    Debug.WriteLine(x.AccQty);
                    //    Debug.WriteLine(x.RevStrtDateOnTxt);
                    //    Debug.WriteLine(x.PostingDateOnTxt);
                    //    Debug.WriteLine(x.Paper);
                    //    Debug.WriteLine(x.Env);
                    //    Debug.WriteLine(x.Postage);
                    //    Debug.WriteLine(x.RegistredMails);
                    //    Debug.WriteLine(x.Franking);
                    //    Debug.WriteLine(x.TotalAmountO3);



                    //}


                    cn.Close();
                    ExcelPackage excel = new ExcelPackage();
                    var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                    workSheet.TabColor = System.Drawing.Color.Black;

                    workSheet.DefaultRowHeight = 12;
                    workSheet.Row(1).Height = 20;
                    workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(1).Style.Font.Bold = true;

                    workSheet.Cells[1, 1].Value = "CUSTOMER";
                    workSheet.Cells[1, 2].Value = "PRODUCTNAME";
                    workSheet.Cells[1, 3].Value = "LOGTAGNO";
                    workSheet.Cells[1, 4].Value = "PAPER";
                    workSheet.Cells[1, 5].Value = "IMPRESSION";
                    workSheet.Cells[1, 6].Value = "ACCOUNT";
                    workSheet.Cells[1, 7].Value = "COLLECTION DATE";
                    workSheet.Cells[1, 8].Value = "COLLECTION TIME";
                    workSheet.Cells[1, 9].Value = "PRINTED DATE";
                    workSheet.Cells[1, 10].Value = "POSTING DATE";
                    workSheet.Cells[1, 11].Value = "PAPER";
                    workSheet.Cells[1, 12].Value = "ENV";
                    workSheet.Cells[1, 13].Value = "SERVICE CHARGES";
                    workSheet.Cells[1, 14].Value = "POSTAGE";
                    workSheet.Cells[1, 15].Value = "REGISTERED MAILS";
                    workSheet.Cells[1, 16].Value = "REBATE";
                    workSheet.Cells[1, 17].Value = "FRANKING";
                    workSheet.Cells[1, 18].Value = "10%";
                    workSheet.Cells[1, 19].Value = "SST 8%";
                    workSheet.Cells[1, 20].Value = "PROGRAMMING CHARGE";
                    workSheet.Cells[1, 21].Value = "TOTAL";
                    workSheet.Cells[1, 22].Value = "NET INVOICE";


                    int recordIndex = 2;

                    foreach (var CLM in ViewList)
                    {
                        workSheet.Cells[recordIndex, 1].Value = CLM.Customer_Name;
                        workSheet.Cells[recordIndex, 2].Value = CLM.ProductName;
                        workSheet.Cells[recordIndex, 3].Value = CLM.LogTagNo;
                        workSheet.Cells[recordIndex, 4].Value = CLM.PageQty;
                        workSheet.Cells[recordIndex, 5].Value = CLM.ImpQty;
                        workSheet.Cells[recordIndex, 6].Value = CLM.AccQty;
                        workSheet.Cells[recordIndex, 7].Value = CLM.RevStrtDateOnTxt;
                        workSheet.Cells[recordIndex, 8].Value = CLM.RevStrtTime;
                        workSheet.Cells[recordIndex, 9].Value = CLM.DatePrintedTxt;
                        workSheet.Cells[recordIndex, 10].Value = CLM.PostingDateOnTxt;
                        workSheet.Cells[recordIndex, 11].Value = CLM.Paper;
                        workSheet.Cells[recordIndex, 12].Value = CLM.Env;

                        //workSheet.Cells[recordIndex, 13].Value = CLM.ServiceChargesDouble;
                        workSheet.Cells[recordIndex, 13].Formula = "="+CLM.ServiceChargesDouble.ToString()+"-N" + recordIndex + "-O" + recordIndex + "-P"+recordIndex+"-Q"+recordIndex+"-T"+recordIndex;

                        workSheet.Cells[recordIndex, 14].Value = CLM.Postage;
                        workSheet.Cells[recordIndex, 15].Value = CLM.RegistredMails;
                        workSheet.Cells[recordIndex, 16].Value = CLM.Rebate;
                        workSheet.Cells[recordIndex, 17].Value = CLM.Franking;
                        workSheet.Cells[recordIndex, 18].Value = CLM.Persent10;
                        //workSheet.Cells[recordIndex, 17].Value = CLM.TotalAmountO2;
                        //workSheet.Cells[recordIndex, 19].Formula = "=(M" + recordIndex + "+N" + recordIndex + "+O" + recordIndex + "+P" + recordIndex + "+Q" + recordIndex + ")*0.08"; /*CLM.Sst*/;
                        workSheet.Cells[recordIndex, 19].Value =CLM.Sst ; /*CLM.Sst*/;

                        workSheet.Cells[recordIndex, 20].Value = CLM.TotalAmountO3;
                        //workSheet.Cells[recordIndex, 20].Value = CLM.TotalAmountx;
                        workSheet.Cells[recordIndex, 21].Formula = "=TEXT(K" + recordIndex + "+L" + recordIndex + "+M"+recordIndex+"+N"+recordIndex+"+O"+recordIndex+"+P"+recordIndex+"+Q"+recordIndex+"+R"+recordIndex+"+S"+recordIndex+"+T"+recordIndex+",\"RM0.00\")";
                        workSheet.Cells[recordIndex, 22].Formula = "=TEXT(U" + recordIndex + "-N" + recordIndex + "-O" + recordIndex + ",\"RM0.00\")";


                        workSheet.Row(recordIndex).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

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

                    string excelName = " REPORT-" + DateStartTxt + "-" + DateEndTxt;
                    using (var memoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                        excel.SaveAs(memoryStream);
                        memoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }

                    cn.Close();
                }
            }
            else
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn.Open();
                    DateTime DateStart = Convert.ToDateTime(DateStartTxt);
                    DateTime DateEnd = Convert.ToDateTime(DateEndTxt);

                    var DateStart1 = DateStart.ToString("yyyy-MM-dd");
                    var DateEnd1 = DateEnd.ToString("yyyy-MM-dd");

                    DateStartTxt = DateStart.ToString("ddMMyyyy");
                    DateEndTxt = DateEnd.ToString("ddMMyyyy");

                    List<Finance> ViewList = new List<Finance>();
                    using (SqlCommand command2 = new SqlCommand("", cn))
                    {
                        cn.Open();
                        command2.Parameters.Clear();
                        command2.CommandText = @"SELECT DISTINCT JobInstruction.Customer_Name, JobInstruction.ProductName,JobAuditTrailDetail.LogTagNo, JobAuditTrailDetail.PageQty, JobAuditTrailDetail.ImpQty, JobAuditTrailDetail.AccQty, JobAuditTrailDetail.RevStrtDateOn, JobAuditTrailDetail.RevStrtTime, Hist_ProductionSlip.StartDateOn, Hist_ProductionSlip.StartTime,  PostingManifest.PostingDateOn, MailFrankingPosting.Total, TblBilling.TotalAmountService, TblMaterials.DescriptionMaterials, TblBilling.Sst,TblMaterials.Paper,TblMaterials.Env,TblBilling.TotalAmountPostage,TblBilling.Process,TblBilling.TotalAmountO,TblBilling.TotalAmountO2,TblBilling.TotalAmountO3,TblBilling.TotalAmountF, TblMaterials.TotalAmountPaper,TblMaterials.TotalAmountEnv
                                               FROM  JobInstruction INNER JOIN
                                               JobAuditTrailDetail ON JobInstruction.Id = JobAuditTrailDetail.JobAuditTrailId INNER JOIN
                                               PostingManifest ON JobInstruction.Id = PostingManifest.JobInstructionId INNER JOIN
                                               MailFrankingPosting ON JobInstruction.Id = MailFrankingPosting.JobInstructionId INNER JOIN
                                               Hist_ProductionSlip ON JobInstruction.Id = Hist_ProductionSlip.ProductionSlipId INNER JOIN
                                               TblBilling ON JobInstruction.Id = TblBilling.JobInstructionId INNER JOIN
                                               TblMaterials ON JobInstruction.Id = TblMaterials.JobInstructionId
                                              
                                            WHERE JobInstruction.Customer_Name LIKE @Customer_Name AND 
                                         LEFT( CONVERT(varchar, JobInstruction.ModifiedOn, 120), 10) >= @dateStart
                                          AND LEFT( CONVERT(varchar, TblBilling.CreatedOn, 120), 10) <= @dateEnd 
                                           
                                          ORDER BY LogTagNo DESC";
                        command2.Parameters.Add("@dateStart", SqlDbType.DateTime).Value = DateStart1;
                        command2.Parameters.Add("@dateEnd", SqlDbType.DateTime).Value = DateEnd1;
                        command2.Parameters.AddWithValue("@Customer_Name", "%" + Customer_Name + "%");
                        var reader = command2.ExecuteReader();
                        while (reader.Read())
                        {
                            Finance list = new Finance();
                            {

                                if (reader.IsDBNull(0) == false)
                                {
                                    list.Customer_Name = reader.GetString(0);
                                }
                                if (reader.IsDBNull(1) == false)
                                {
                                    list.ProductName = reader.GetString(1);
                                }
                                if (reader.IsDBNull(2) == false)
                                {
                                    list.LogTagNo = reader.GetString(2);
                                }
                                if (reader.IsDBNull(3) == false)
                                {
                                    list.PageQty = reader.GetString(3);
                                }
                                if (reader.IsDBNull(4) == false)
                                {
                                    list.ImpQty = reader.GetString(4);
                                }
                                if (reader.IsDBNull(5) == false)
                                {
                                    list.AccQty = reader.GetString(5);
                                }
                                if (reader.IsDBNull(6) == false)
                                {
                                    list.RevStrtDateOnTxt = reader.GetDateTime(6).ToString("dd/MM/yyyy");
                                }
                                if (reader.IsDBNull(7) == false)
                                {
                                    list.RevStrtTime = reader.GetString(7);
                                }
                                if (reader.IsDBNull(8) == false)
                                {
                                    list.StartDateOnTxt = reader.GetDateTime(8).ToString("dd/MM/yyyy");
                                }
                                if (reader.IsDBNull(9) == false)
                                {
                                    list.StartTime = reader.GetString(9);
                                }

                                if (reader.IsDBNull(10) == false)
                                {
                                    list.PostingDateOnTxt = reader.GetDateTime(10).ToString("dd/MM/yyyy");
                                }
                                if (reader.IsDBNull(11) == false)
                                {
                                    list.Total = reader.GetString(11);
                                }
                                if (reader.IsDBNull(12) == false)
                                {
                                    list.TotalAmountService = reader.GetString(12);
                                }
                                if (reader.IsDBNull(13) == false)
                                {
                                    list.DescriptionMaterials = reader.GetString(13);
                                }
                                if (reader.IsDBNull(14) == false)
                                {
                                    list.Sst = reader.GetString(14);
                                }
                                if (reader.IsDBNull(15) == false)
                                {
                                    list.Paper = reader.GetString(15);
                                }
                                if (reader.IsDBNull(16) == false)
                                {
                                    list.Env = reader.GetString(16);
                                }
                                if (reader.IsDBNull(17) == false)
                                {
                                    list.TotalAmountPostage = reader.GetString(17);
                                }

                                if (reader.IsDBNull(18) == false)
                                {
                                    list.Process = reader.GetString(18);
                                }

                                if (reader.IsDBNull(19) == false)
                                {
                                    list.TotalAmountO = reader.GetString(19);
                                }
                                if (reader.IsDBNull(20) == false)
                                {
                                    list.TotalAmountO2 = reader.GetString(20);
                                }

                                if (reader.IsDBNull(21) == false)
                                {
                                    list.TotalAmountO3 = reader.GetString(21);
                                }
                                if (reader.IsDBNull(22) == false)
                                {
                                    list.TotalAmountF = reader.GetString(22);
                                }

                                if (reader.IsDBNull(23) == false)
                                {
                                    list.TotalAmountPaper = reader.GetString(23);
                                }
                                if (reader.IsDBNull(24) == false)
                                {
                                    list.TotalAmountEnv = reader.GetString(24);
                                }

                            }
                            ViewList.Add(list);
                        }
                        cn.Close();
                    }

                    cn.Close();
                    ExcelPackage excel = new ExcelPackage();
                    var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                    workSheet.TabColor = System.Drawing.Color.Black;

                    workSheet.DefaultRowHeight = 12;
                    workSheet.Row(1).Height = 20;
                    workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(1).Style.Font.Bold = true;

                    workSheet.Cells[1, 1].Value = "CUSTOMER";
                    workSheet.Cells[1, 2].Value = "PRODUCTNAME";
                    workSheet.Cells[1, 3].Value = "LOGTAGNO";
                    workSheet.Cells[1, 4].Value = "PAPER";
                    workSheet.Cells[1, 5].Value = "IMPRESSION";
                    workSheet.Cells[1, 6].Value = "ACCOUNT";
                    workSheet.Cells[1, 7].Value = "COLLECTION DATE";
                    workSheet.Cells[1, 8].Value = "COLLECTION TIME";
                    workSheet.Cells[1, 9].Value = "PRINTED DATE";
                    workSheet.Cells[1, 10].Value = "POSTING DATE";
                    workSheet.Cells[1, 11].Value = "PAPER";
                    workSheet.Cells[1, 12].Value = "ENV";
                    workSheet.Cells[1, 13].Value = "SERVICE CHARGES";
                    workSheet.Cells[1, 14].Value = "POSTAGE";
                    workSheet.Cells[1, 15].Value = "REGISTERED MAILS";
                    workSheet.Cells[1, 16].Value = "FRANKING";
                    workSheet.Cells[1, 17].Value = "10%";
                    workSheet.Cells[1, 18].Value = "SST 6%";
                    workSheet.Cells[1, 19].Value = "PROGRAMMING CHARGE";
                    workSheet.Cells[1, 20].Value = "TOTAL";




                    int recordIndex = 2;

                    foreach (var CLM in ViewList)
                    {
                        workSheet.Cells[recordIndex, 1].Value = CLM.Customer_Name;
                        workSheet.Cells[recordIndex, 2].Value = CLM.ProductName;
                        workSheet.Cells[recordIndex, 3].Value = CLM.LogTagNo;
                        workSheet.Cells[recordIndex, 4].Value = CLM.PageQty;
                        workSheet.Cells[recordIndex, 5].Value = CLM.ImpQty;
                        workSheet.Cells[recordIndex, 6].Value = CLM.AccQty;
                        workSheet.Cells[recordIndex, 7].Value = CLM.RevStrtDateOnTxt;
                        workSheet.Cells[recordIndex, 8].Value = CLM.RevStrtTime;
                        workSheet.Cells[recordIndex, 9].Value = CLM.StartDateOnTxt;
                        workSheet.Cells[recordIndex, 10].Value = CLM.PostingDateOnTxt;
                        workSheet.Cells[recordIndex, 11].Value = CLM.TotalAmountPaper;
                        workSheet.Cells[recordIndex, 12].Value = CLM.TotalAmountEnv;
                        workSheet.Cells[recordIndex, 13].Value = CLM.TotalAmountService;
                        workSheet.Cells[recordIndex, 14].Value = CLM.TotalAmountPostage;
                        workSheet.Cells[recordIndex, 15].Value = CLM.TotalAmountO;
                        workSheet.Cells[recordIndex, 16].Value = CLM.TotalAmountF;
                        workSheet.Cells[recordIndex, 17].Value = CLM.Persent10;
                        workSheet.Cells[recordIndex, 18].Value = CLM.Sst;
                        workSheet.Cells[recordIndex, 19].Value = CLM.TotalAmountO3;
                        workSheet.Cells[recordIndex, 20].Value = CLM.TotalAmountx;



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

                    string excelName = "REPORT-" + DateStartTxt + "-" + DateEndTxt;
                    using (var memoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                        excel.SaveAs(memoryStream);
                        memoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                    cn.Close();
                }
            }

            return RedirectToAction("ListReportFin", "Report");
        }




        //[HttpPost]
        //public ActionResult getReport2(string id, string DateStartTxt, string DateEndTxt, string Customer_Name,string ProductName)
        //{
        //    ViewBag.IsDepart = @Session["Department"];
        //    ViewBag.IsRole = @Session["Role"];

        //    if (!string.IsNullOrEmpty(DateStartTxt) && !string.IsNullOrEmpty(DateEndTxt))
        //    {
        //        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        //        {
        //            DateTime DateStart = Convert.ToDateTime(DateStartTxt);
        //            DateTime DateEnd = Convert.ToDateTime(DateEndTxt);

        //            var DateStart1 = DateStart.ToString("yyyy-MM-dd");
        //            var DateEnd1 = DateEnd.ToString("yyyy-MM-dd");

        //            DateStartTxt = DateStart.ToString("ddMMyyyy");
        //            DateEndTxt = DateEnd.ToString("ddMMyyyy");

        //            List<Hist_ProductionSlip> gotlist = new List<Hist_ProductionSlip>();
        //            cn.Open();
        //            SqlCommand command;
        //            command = new SqlCommand(@"SELECT JobInstruction.Customer_Name, JobInstruction.ProductName, Hist_ProductionSlip.PageQty, Hist_ProductionSlip.ImpQty, Hist_ProductionSlip.AccQty, Hist_ProductionSlip.Status, JobInstruction.JobSheetNo,TblBilling.CreatedOn,TblBilling.Process,TblBilling.TotalAmount,TblBilling.GrandTotal
        //                                      FROM  JobInstruction INNER JOIN
        //                                       Hist_ProductionSlip ON JobInstruction.Id = Hist_ProductionSlip.ProductionSlipId INNER JOIN
        //                                      TblBilling ON JobInstruction.Id = TblBilling.JobInstructionId
        //                                   WHERE JobInstruction.Customer_Name like @Customer_Name AND
        //                                  LEFT( CONVERT(varchar, JobInstruction.CreatedOn, 120), 10) >= @dateStart
        //                                   AND LEFT( CONVERT(varchar, JobInstruction.CreatedOn, 120), 10) <= @dateEnd  

        //                                   ORDER BY JobInstruction.ModifiedOn", cn);
        //            command.Parameters.Add("@dateStart", SqlDbType.DateTime).Value = DateStart1;
        //            command.Parameters.Add("@dateEnd", SqlDbType.DateTime).Value = DateEnd1;
        //            command.Parameters.AddWithValue("@Customer_Name", "%" + ProductName + "%");
        //            var reader = command.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                Hist_ProductionSlip list = new Hist_ProductionSlip();
        //                {
        //                    if (reader.IsDBNull(0) == false)
        //                    {
        //                        list.Customer_Name = reader.GetString(0);
        //                    }
        //                    if (reader.IsDBNull(1) == false)
        //                    {
        //                        list.ProductName = reader.GetString(1);
        //                    }

        //                    if (reader.IsDBNull(2) == false)
        //                    {
        //                        list.PageQty = reader.GetString(2);
        //                    }
        //                    if (reader.IsDBNull(3) == false)
        //                    {
        //                        list.ImpQty = reader.GetString(3);
        //                    }
        //                    if (reader.IsDBNull(4) == false)
        //                    {
        //                        list.AccQty = reader.GetString(4);
        //                    }

        //                    if (reader.IsDBNull(5) == false)
        //                    {
        //                        list.Status = reader.GetString(5);
        //                    }
        //                    if (reader.IsDBNull(6) == false)
        //                    {
        //                        list.JobSheetNo = reader.GetString(6);
        //                    }
        //                    if (reader.IsDBNull(7) == false)
        //                    {
        //                        list.CreatedOnTxt = String.Format("{0:dd/MM/yyyy}", (DateTime)reader.GetDateTime(7));
        //                    }
        //                    if (reader.IsDBNull(8) == false)
        //                    {
        //                        list.Process = reader.GetString(8);
        //                    }
        //                    if (reader.IsDBNull(9) == false)
        //                    {
        //                        list.TotalAmount = reader.GetString(9);
        //                    }
        //                    if (reader.IsDBNull(10) == false)
        //                    {
        //                        list.GrandTotal = reader.GetString(10);
        //                    }
        //                }
        //                gotlist.Add(list);
        //            }
        //            cn.Close();
        //            ExcelPackage excel = new ExcelPackage();
        //            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
        //            workSheet.TabColor = System.Drawing.Color.Black;

        //            workSheet.DefaultRowHeight = 12;
        //            workSheet.Row(1).Height = 20;
        //            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            workSheet.Row(1).Style.Font.Bold = true;
        //            workSheet.Cells[1, 1].Value = "NO.";
        //            workSheet.Cells[1, 2].Value = "CUSTOMER NAME";
        //            workSheet.Cells[1, 3].Value = "PRODUCT NAME";
        //            workSheet.Cells[1, 4].Value = "CUS";
        //            workSheet.Cells[1, 5].Value = "PAPER";
        //            workSheet.Cells[1, 6].Value = "IMPRESSION";
        //            workSheet.Cells[1, 7].Value = "ACCOUNTS QTY";
        //            workSheet.Cells[1, 8].Value = " ACTUAL AUDIT TRAIL";
        //            workSheet.Cells[1, 9].Value = "DATE INV";
        //            workSheet.Cells[1, 10].Value = "PROCESS";
        //            workSheet.Cells[1, 11].Value = "AMOUNT";
        //            workSheet.Cells[1, 12].Value = "GRAND TOTAL";




        //            int recordIndex = 2;
        //            foreach (var CLM in gotlist)
        //            {
        //                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
        //                workSheet.Cells[recordIndex, 2].Value = CLM.Customer_Name;
        //                workSheet.Cells[recordIndex, 3].Value = CLM.ProductName;
        //                workSheet.Cells[recordIndex, 4].Value = CLM.Cust_Department;
        //                workSheet.Cells[recordIndex, 5].Value = CLM.PageQty;
        //                workSheet.Cells[recordIndex, 6].Value = CLM.ImpQty;
        //                workSheet.Cells[recordIndex, 7].Value = CLM.AccQty;
        //                workSheet.Cells[recordIndex, 8].Value = CLM.Status;
        //                workSheet.Cells[recordIndex, 9].Value = CLM.CreatedOnTxt;
        //                workSheet.Cells[recordIndex, 10].Value = CLM.Process;
        //                workSheet.Cells[recordIndex, 11].Value = CLM.TotalAmount;
        //                workSheet.Cells[recordIndex, 12].Value = CLM.GrandTotal;

        //                recordIndex++;
        //            }
        //            workSheet.Column(1).AutoFit();
        //            workSheet.Column(2).AutoFit();
        //            workSheet.Column(3).AutoFit();
        //            workSheet.Column(4).AutoFit();
        //            workSheet.Column(5).AutoFit();
        //            workSheet.Column(6).AutoFit();
        //            workSheet.Column(7).AutoFit();
        //            workSheet.Column(8).AutoFit();
        //            workSheet.Column(9).AutoFit();
        //            workSheet.Column(10).AutoFit();
        //            workSheet.Column(11).AutoFit();
        //            workSheet.Column(12).AutoFit();



        //            string excelName = "REPORT-" + DateStartTxt + "-" + DateEndTxt;
        //            using (var memoryStream = new MemoryStream())
        //            {
        //                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
        //                excel.SaveAs(memoryStream);
        //                memoryStream.WriteTo(Response.OutputStream);
        //                Response.Flush();
        //                Response.End();
        //            }

        //        }
        //    }

        //    return RedirectToAction("ListReportFin", "Report");
        //}



        List<ViewAuditTrail> viewJTD = new List<ViewAuditTrail>();

        public int _bil { get; set; }

        public ActionResult ListReportTracking(string set, string Customer_Name)
        {
            ViewBag.IsDepart = @Session["Department"];
            ViewBag.IsRole = @Session["Role"];
            var Role = @Session["Role"];

            if (set == "search")
            {

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    int _bil = 1;
                    cn.Open();
                    command.CommandText = @"SELECT JobAuditTrailDetail.CreateByIt, JobAuditTrailDetail.TimeProcessIt, JobAuditTrailDetail.Customer_Name, JobInstruction.ProductName, JobAuditTrailDetail.LogTagNo, JobAuditTrailDetail.AccQty, JobAuditTrailDetail.ImpQty, JobAuditTrailDetail.PageQty, JobAuditTrailDetail.StartDate, JobAuditTrailDetail.RevStrtDateOn, JobInstruction.SalesExecutiveBy, PostingManifest.CreatedOn, PostingManifest.PostingDateOn, PostingManifest.PostingTime
                                           FROM  PostingManifest INNER JOIN
                                           JobAuditTrailDetail ON PostingManifest.JobInstructionId = JobAuditTrailDetail.JobInstructionId INNER JOIN
                                           JobInstruction ON JobAuditTrailDetail.JobAuditTrailId = JobInstruction.Id
                                           WHERE (PostingManifest.Status = 'POSTING')
                                          WHERE Customer_Name LIKE @Customer_Name";
                    command.Parameters.AddWithValue("@Customer_Name", "%" + Customer_Name + "%");
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ViewAuditTrail model = new ViewAuditTrail();
                        {
                            model.Bil = _bil++;

                            if (reader.IsDBNull(0) == false)
                            {
                                model.CreateByIt = reader.GetString(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                model.TimeProcessIt = reader.GetString(1);
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
                                model.LogTagNo = reader.GetString(4);
                            }
                            if (reader.IsDBNull(5) == false)
                            {
                                model.AccQty = reader.GetString(5);
                            }
                            if (reader.IsDBNull(6) == false)
                            {
                                model.ImpQty = reader.GetString(6);
                            }
                            if (reader.IsDBNull(7) == false)
                            {
                                model.PageQty = reader.GetString(7);
                            }

                            if (reader.IsDBNull(8) == false)
                            {
                                model.StartDateTxt = String.Format("{0:dd/MM/yyyy }", (DateTime)reader.GetDateTime(8));
                            }
                            if (reader.IsDBNull(9) == false)
                            {
                                model.RevStrtDateOnTxt = String.Format("{0:dd/MM/yyyy }", (DateTime)reader.GetDateTime(9));
                            }

                            if (reader.IsDBNull(10) == false)
                            {
                                model.SalesExecutiveBy = reader.GetString(10);
                            }
                            if (reader.IsDBNull(11) == false)
                            {
                                model.CreatedOnTxt = String.Format("{0:dd/MM/yyyy }", (DateTime)reader.GetDateTime(11));
                            }
                            if (reader.IsDBNull(12) == false)
                            {
                                model.PostingDateOnTxt = String.Format("{0:dd/MM/yyyy }", (DateTime)reader.GetDateTime(12));
                            }
                            if (reader.IsDBNull(13) == false)
                            {
                                model.PostingTime = reader.GetString(13);
                            }
                        }
                        viewJTD.Add(model);
                    }
                    cn.Close();
                }
            }

            else
            {
                //ALL
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    int _bil = 1;
                    cn.Open();
                    command.CommandText = @"SELECT JobAuditTrailDetail.CreateByIt, JobAuditTrailDetail.TimeProcessIt, JobAuditTrailDetail.Customer_Name, JobInstruction.ProductName, JobAuditTrailDetail.LogTagNo, JobAuditTrailDetail.AccQty, JobAuditTrailDetail.ImpQty, JobAuditTrailDetail.PageQty, JobAuditTrailDetail.StartDate, JobAuditTrailDetail.RevStrtDateOn, JobInstruction.SalesExecutiveBy, PostingManifest.CreatedOn, PostingManifest.PostingDateOn, PostingManifest.PostingTime
                                            FROM  PostingManifest INNER JOIN
                                            JobAuditTrailDetail ON PostingManifest.JobInstructionId = JobAuditTrailDetail.JobInstructionId INNER JOIN
                                            JobInstruction ON JobAuditTrailDetail.JobAuditTrailId = JobInstruction.Id
                                            WHERE (PostingManifest.Status = 'POSTING')";
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ViewAuditTrail model = new ViewAuditTrail();
                        {
                            model.Bil = _bil++;


                            if (reader.IsDBNull(0) == false)
                            {
                                model.CreateByIt = reader.GetString(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                model.TimeProcessIt = reader.GetString(1);
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
                                model.LogTagNo = reader.GetString(4);
                            }
                            if (reader.IsDBNull(5) == false)
                            {
                                model.AccQty = reader.GetString(5);
                            }
                            if (reader.IsDBNull(6) == false)
                            {
                                model.ImpQty = reader.GetString(6);
                            }
                            if (reader.IsDBNull(7) == false)
                            {
                                model.PageQty = reader.GetString(7);
                            }

                            if (reader.IsDBNull(8) == false)
                            {
                                model.StartDateTxt = String.Format("{0:dd/MM/yyyy }", (DateTime)reader.GetDateTime(8));
                            }
                            if (reader.IsDBNull(9) == false)
                            {
                                model.RevStrtDateOnTxt = String.Format("{0:dd/MM/yyyy }", (DateTime)reader.GetDateTime(9));
                            }

                            if (reader.IsDBNull(10) == false)
                            {
                                model.SalesExecutiveBy = reader.GetString(10);
                            }
                            if (reader.IsDBNull(11) == false)
                            {
                                model.CreatedOnTxt = String.Format("{0:dd/MM/yyyy }", (DateTime)reader.GetDateTime(11));
                            }
                            if (reader.IsDBNull(12) == false)
                            {
                                model.PostingDateOnTxt = String.Format("{0:dd/MM/yyyy }", (DateTime)reader.GetDateTime(12));
                            }
                            if (reader.IsDBNull(13) == false)
                            {
                                model.PostingTime = reader.GetString(13);
                            }
                        }
                        viewJTD.Add(model);
                    }
                    cn.Close();
                }
            }

            return View(viewJTD);
        }

        [HttpPost]
        public ActionResult getReportTracking(JobInstruction get, string id, string DateStartTxt, string DateEndTxt)
        {
            ViewBag.IsDepart = @Session["Department"];
            ViewBag.IsRole = @Session["Role"];

            if (!string.IsNullOrEmpty(DateStartTxt) && !string.IsNullOrEmpty(DateEndTxt))
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    DateTime DateStart = Convert.ToDateTime(DateStartTxt);
                    DateTime DateEnd = Convert.ToDateTime(DateEndTxt);

                    var DateStart1 = DateStart.ToString("yyyy-MM-dd");
                    var DateEnd1 = DateEnd.ToString("yyyy-MM-dd");

                    DateStartTxt = DateStart.ToString("ddMMyyyy");
                    DateEndTxt = DateEnd.ToString("ddMMyyyy");

                    List<ViewAuditTrail> viewJTD = new List<ViewAuditTrail>();
                    cn.Open();
                    SqlCommand command;
                    command = new SqlCommand(@"SELECT JobAuditTrailDetail.CreateByIt, JobAuditTrailDetail.TimeProcessIt, JobAuditTrailDetail.Customer_Name, JobInstruction.ProductName, JobAuditTrailDetail.LogTagNo, JobAuditTrailDetail.AccQty, JobAuditTrailDetail.ImpQty, JobAuditTrailDetail.PageQty, JobAuditTrailDetail.StartDate, JobAuditTrailDetail.RevStrtDateOn, JobInstruction.SalesExecutiveBy, PostingManifest.CreatedOn, PostingManifest.PostingDateOn, PostingManifest.PostingTime
                                               FROM  JobInstruction INNER JOIN
                                             JobAuditTrailDetail ON JobInstruction.Id = JobAuditTrailDetail.JobInstructionId INNER JOIN
                                            PostingManifest ON JobAuditTrailDetail.JobInstructionId = PostingManifest.JobInstructionId
                                           WHERE LEFT( CONVERT(varchar, JobInstruction.ModifiedOn, 120), 10) >= @dateStart
                                           AND LEFT( CONVERT(varchar, PostingManifest.CreatedOn, 120), 10) <= @dateEnd                                          
                                           ORDER BY JobInstruction.ModifiedOn", cn);
                    command.Parameters.Add("@dateStart", SqlDbType.DateTime).Value = DateStart1;
                    command.Parameters.Add("@dateEnd", SqlDbType.DateTime).Value = DateEnd1;
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ViewAuditTrail model = new ViewAuditTrail();
                        {
                            model.Bil = _bil++;

                            if (reader.IsDBNull(0) == false)
                            {
                                model.CreateByIt = reader.GetString(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                model.TimeProcessIt = reader.GetString(1);
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
                                model.LogTagNo = reader.GetString(4);
                            }
                            if (reader.IsDBNull(5) == false)
                            {
                                model.AccQty = reader.GetString(5);
                            }
                            if (reader.IsDBNull(6) == false)
                            {
                                model.ImpQty = reader.GetString(6);
                            }
                            if (reader.IsDBNull(7) == false)
                            {
                                model.PageQty = reader.GetString(7);
                            }

                            if (reader.IsDBNull(8) == false)
                            {
                                model.StartDateTxt = String.Format("{0:dd/MM/yyyy }", (DateTime)reader.GetDateTime(8));
                            }
                            if (reader.IsDBNull(9) == false)
                            {
                                model.RevStrtDateOnTxt = String.Format("{0:dd/MM/yyyy }", (DateTime)reader.GetDateTime(9));
                            }

                            if (reader.IsDBNull(10) == false)
                            {
                                model.SalesExecutiveBy = reader.GetString(10);
                            }
                            if (reader.IsDBNull(11) == false)
                            {
                                model.CreatedOnTxt = String.Format("{0:dd/MM/yyyy }", (DateTime)reader.GetDateTime(11));
                            }
                            if (reader.IsDBNull(12) == false)
                            {
                                model.PostingDateOnTxt = String.Format("{0:dd/MM/yyyy }", (DateTime)reader.GetDateTime(12));
                            }
                            if (reader.IsDBNull(13) == false)
                            {
                                model.PostingTime = reader.GetString(13);
                            }
                        }
                        viewJTD.Add(model);
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
                    workSheet.Cells[1, 2].Value = "It Submit Date ";
                    workSheet.Cells[1, 3].Value = "It Submit Time";
                    workSheet.Cells[1, 4].Value = "Customer";
                    workSheet.Cells[1, 5].Value = "Product";
                    workSheet.Cells[1, 6].Value = "Log Tag No.";
                    workSheet.Cells[1, 7].Value = "Acc Qty.";
                    workSheet.Cells[1, 8].Value = "Imp Qty";
                    workSheet.Cells[1, 9].Value = "Pages Qty";
                    workSheet.Cells[1, 10].Value = "StartIn Date";
                    workSheet.Cells[1, 11].Value = "StartIn Time";
                    workSheet.Cells[1, 12].Value = "PIC";
                    workSheet.Cells[1, 13].Value = "Date Post";
                    workSheet.Cells[1, 14].Value = "Process Start Date";
                    workSheet.Cells[1, 15].Value = "Process Start Time";


                    int recordIndex = 2;
                    foreach (var CLM in viewJTD)
                    {
                        workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
                        workSheet.Cells[recordIndex, 2].Value = CLM.CreateByIt;
                        workSheet.Cells[recordIndex, 3].Value = CLM.TimeProcessIt;
                        workSheet.Cells[recordIndex, 4].Value = CLM.Customer_Name;
                        workSheet.Cells[recordIndex, 5].Value = CLM.ProductName;
                        workSheet.Cells[recordIndex, 6].Value = CLM.LogTagNo;
                        workSheet.Cells[recordIndex, 7].Value = CLM.AccQty;
                        workSheet.Cells[recordIndex, 8].Value = CLM.ImpQty;
                        workSheet.Cells[recordIndex, 9].Value = CLM.PageQty;
                        workSheet.Cells[recordIndex, 10].Value = CLM.StartDateTxt;
                        workSheet.Cells[recordIndex, 11].Value = CLM.RevStrtDateOnTxt;
                        workSheet.Cells[recordIndex, 12].Value = CLM.SalesExecutiveBy;
                        workSheet.Cells[recordIndex, 13].Value = CLM.CreatedOnTxt;
                        workSheet.Cells[recordIndex, 14].Value = CLM.PostingDateOnTxt;
                        workSheet.Cells[recordIndex, 15].Value = CLM.PostingTime;

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


                    string excelName = "REPORT TRACKING-" + DateStartTxt + "-" + DateEndTxt;
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
            }

            return RedirectToAction("ListReportTracking", "Report");
        }


        List<ProductionSlip> Product = new List<ProductionSlip>();
        public ActionResult ManageProduct(string set, string ProductName, string product, string Customer_Name, string customer)
        {


            if (set == "search")

            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    int _bil = 1;
                    cn.Open();
                    command.CommandText = @"SELECT Planner.ModifiedOn, Planner.StartProductionDateOn, Planner.PlanDatePostOn, Planner.PlanReturn_CourierOn, Planner.CreateByPlanner, Planner.MachineInsert, Planner.Machine, Planner.PlanShift, JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.ProductName, JobAuditTrailDetail.AccQty, JobAuditTrailDetail.ImpQty, JobAuditTrailDetail.PageQty, JobAuditTrailDetail.ModeLog, JobInstruction.JobSheetNo, JobInstruction.JobClass, JobInstruction.JobType, JobAuditTrailDetail.RevStrtDateOn
                                            FROM  JobInstruction INNER JOIN
                                             JobAuditTrailDetail ON JobInstruction.Id = JobAuditTrailDetail.JobAuditTrailId INNER JOIN
                                             Planner ON JobAuditTrailDetail.JobAuditTrailId = Planner.JobInstructionId
                                             AND JobAuditTrailDetail.ProductName LIKE @ProductName";

                    command.Parameters.AddWithValue("@ProductName", "%" + product + "%");

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ProductionSlip model = new ProductionSlip();
                        {
                            model.Bil = _bil++;
                            if (reader.IsDBNull(0) == false)
                            {
                                model.ModifiedOn = reader.GetDateTime(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                model.StartProductionDateOn = reader.GetDateTime(1);
                            }
                            if (reader.IsDBNull(2) == false)
                            {
                                model.PlanDatePostOn = reader.GetDateTime(2);
                            }
                            if (reader.IsDBNull(3) == false)
                            {
                                model.PlanReturn_CourierOn = reader.GetDateTime(3);
                            }
                            if (reader.IsDBNull(4) == false)
                            {
                                model.CreateByPlanner = reader.GetString(4);
                            }

                            if (reader.IsDBNull(5) == false)
                            {
                                model.MachineInsert = reader.GetString(5);
                            }
                            if (reader.IsDBNull(6) == false)
                            {
                                model.Machine = reader.GetString(6);
                            }
                            if (reader.IsDBNull(7) == false)
                            {
                                model.PlanShift = reader.GetString(7);
                            }
                            if (reader.IsDBNull(8) == false)
                            {
                                model.Customer_Name = reader.GetString(8);
                            }
                            if (reader.IsDBNull(9) == false)
                            {
                                model.ProductName = reader.GetString(9);
                            }
                            if (reader.IsDBNull(10) == false)
                            {
                                model.AccQty = reader.GetString(10);
                            }
                            if (reader.IsDBNull(11) == false)
                            {
                                model.ImpQty = reader.GetString(11);
                            }
                            if (reader.IsDBNull(12) == false)
                            {
                                model.PageQty = reader.GetString(12);
                            }
                            if (reader.IsDBNull(13) == false)
                            {
                                model.ModeLog = reader.GetString(13);
                            }
                            if (reader.IsDBNull(14) == false)
                            {
                                model.JobSheetNo = reader.GetString(14);
                            }
                            if (reader.IsDBNull(15) == false)
                            {
                                model.JobClass = reader.GetString(15);
                            }
                            if (reader.IsDBNull(16) == false)
                            {
                                model.JobType = reader.GetString(16);
                            }
                            if (reader.IsDBNull(17) == false)
                            {
                                model.RevStrtDateOn = reader.GetDateTime(17);
                            }
                        }
                        Product.Add(model);
                    }
                    cn.Close();
                }
            }

            else if (set == "search2")

            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    int _bil = 1;
                    cn.Open();
                    command.CommandText = @"SELECT Planner.ModifiedOn, Planner.StartProductionDateOn, Planner.PlanDatePostOn, Planner.PlanReturn_CourierOn, Planner.CreateByPlanner, Planner.MachineInsert, Planner.Machine, Planner.PlanShift, JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.ProductName, JobAuditTrailDetail.AccQty, JobAuditTrailDetail.ImpQty, JobAuditTrailDetail.PageQty, JobAuditTrailDetail.ModeLog, JobInstruction.JobSheetNo, JobInstruction.JobClass, JobInstruction.JobType, JobAuditTrailDetail.RevStrtDateOn
                                            FROM  JobInstruction INNER JOIN
                                             JobAuditTrailDetail ON JobInstruction.Id = JobAuditTrailDetail.JobAuditTrailId INNER JOIN
                                             Planner ON JobAuditTrailDetail.JobAuditTrailId = Planner.JobInstructionId
                                             AND JobAuditTrailDetail.Customer_Name LIKE @Customer_Name";

                    command.Parameters.AddWithValue("@Customer_Name", "%" + customer + "%");

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ProductionSlip model = new ProductionSlip();
                        {
                            model.Bil = _bil++;
                            if (reader.IsDBNull(0) == false)
                            {
                                model.ModifiedOn = reader.GetDateTime(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                model.StartProductionDateOn = reader.GetDateTime(1);
                            }
                            if (reader.IsDBNull(2) == false)
                            {
                                model.PlanDatePostOn = reader.GetDateTime(2);
                            }
                            if (reader.IsDBNull(3) == false)
                            {
                                model.PlanReturn_CourierOn = reader.GetDateTime(3);
                            }
                            if (reader.IsDBNull(4) == false)
                            {
                                model.CreateByPlanner = reader.GetString(4);
                            }

                            if (reader.IsDBNull(5) == false)
                            {
                                model.MachineInsert = reader.GetString(5);
                            }
                            if (reader.IsDBNull(6) == false)
                            {
                                model.Machine = reader.GetString(6);
                            }
                            if (reader.IsDBNull(7) == false)
                            {
                                model.PlanShift = reader.GetString(7);
                            }
                            if (reader.IsDBNull(8) == false)
                            {
                                model.Customer_Name = reader.GetString(8);
                            }
                            if (reader.IsDBNull(9) == false)
                            {
                                model.ProductName = reader.GetString(9);
                            }
                            if (reader.IsDBNull(10) == false)
                            {
                                model.AccQty = reader.GetString(10);
                            }
                            if (reader.IsDBNull(11) == false)
                            {
                                model.ImpQty = reader.GetString(11);
                            }
                            if (reader.IsDBNull(12) == false)
                            {
                                model.PageQty = reader.GetString(12);
                            }
                            if (reader.IsDBNull(13) == false)
                            {
                                model.ModeLog = reader.GetString(13);
                            }
                            if (reader.IsDBNull(14) == false)
                            {
                                model.JobSheetNo = reader.GetString(14);
                            }
                            if (reader.IsDBNull(15) == false)
                            {
                                model.JobClass = reader.GetString(15);
                            }
                            if (reader.IsDBNull(16) == false)
                            {
                                model.JobType = reader.GetString(16);
                            }
                            if (reader.IsDBNull(17) == false)
                            {
                                model.RevStrtDateOn = reader.GetDateTime(17);
                            }
                        }
                        Product.Add(model);
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
                    command.CommandText = @"SELECT Planner.ModifiedOn, Planner.StartProductionDateOn, Planner.PlanDatePostOn, Planner.PlanReturn_CourierOn, Planner.CreateByPlanner, Planner.MachineInsert, Planner.Machine, Planner.PlanShift, JobAuditTrailDetail.Customer_Name, JobAuditTrailDetail.ProductName, JobAuditTrailDetail.AccQty, JobAuditTrailDetail.ImpQty, JobAuditTrailDetail.PageQty, JobAuditTrailDetail.ModeLog, JobInstruction.JobSheetNo, JobInstruction.JobClass, JobInstruction.JobType, JobAuditTrailDetail.RevStrtDateOn
                                  FROM  JobInstruction INNER JOIN
                                 JobAuditTrailDetail ON JobInstruction.Id = JobAuditTrailDetail.JobAuditTrailId INNER JOIN
                                  Planner ON JobAuditTrailDetail.JobAuditTrailId = Planner.JobInstructionId";
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ProductionSlip model = new ProductionSlip();
                        {
                            model.Bil = _bil++;
                            if (reader.IsDBNull(0) == false)
                            {
                                model.ModifiedOn = reader.GetDateTime(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                model.StartProductionDateOn = reader.GetDateTime(1);
                            }
                            if (reader.IsDBNull(2) == false)
                            {
                                model.PlanDatePostOn = reader.GetDateTime(2);
                            }
                            if (reader.IsDBNull(3) == false)
                            {
                                model.PlanReturn_CourierOn = reader.GetDateTime(3);
                            }
                            if (reader.IsDBNull(4) == false)
                            {
                                model.CreateByPlanner = reader.GetString(4);
                            }

                            if (reader.IsDBNull(5) == false)
                            {
                                model.MachineInsert = reader.GetString(5);
                            }
                            if (reader.IsDBNull(6) == false)
                            {
                                model.Machine = reader.GetString(6);
                            }
                            if (reader.IsDBNull(7) == false)
                            {
                                model.PlanShift = reader.GetString(7);
                            }
                            if (reader.IsDBNull(8) == false)
                            {
                                model.Customer_Name = reader.GetString(8);
                            }
                            if (reader.IsDBNull(9) == false)
                            {
                                model.ProductName = reader.GetString(9);
                            }
                            if (reader.IsDBNull(10) == false)
                            {
                                model.AccQty = reader.GetString(10);
                            }
                            if (reader.IsDBNull(11) == false)
                            {
                                model.ImpQty = reader.GetString(11);
                            }
                            if (reader.IsDBNull(12) == false)
                            {
                                model.PageQty = reader.GetString(12);
                            }
                            if (reader.IsDBNull(13) == false)
                            {
                                model.ModeLog = reader.GetString(13);
                            }
                            if (reader.IsDBNull(14) == false)
                            {
                                model.JobSheetNo = reader.GetString(14);
                            }
                            if (reader.IsDBNull(15) == false)
                            {
                                model.JobClass = reader.GetString(15);
                            }
                            if (reader.IsDBNull(16) == false)
                            {
                                model.JobType = reader.GetString(16);
                            }
                            if (reader.IsDBNull(17) == false)
                            {
                                model.RevStrtDateOn = reader.GetDateTime(17);
                            }

                        }
                        Product.Add(model);
                    }
                    cn.Close();
                }
            }
            return View(Product); //hntr data ke ui
        }

        public string get10Percent(string LogTagNo)
        {
            Decimal Total = 0;
            Decimal Franking = 0;
            Decimal Airmail = 0;
            Decimal Spore = 0;
            Decimal Tletter = 0;
            Decimal NPC = 0;
            Decimal Mix = 0;
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();



                SqlCommand cmd1 = new SqlCommand("SELECT CAST(Franking10Charges as DECIMAL(9,2)) as Franking10Charges, CAST(AirmailCharges as DECIMAL(9,2)) as AirmailCharges, CAST(SporeCharges as DECIMAL(9,2)) as SporeCharges, CAST(TLetterCharges as DECIMAL(9,2)) as TLetterCharges, CAST(NPCCharges as DECIMAL(9,2)) as NPCCharges, CAST(Mix10Charges as DECIMAL(9,2)) as Mix10Charges FROM BillingMPR WHERE JobSheetNo=@LogTagNo", cn);
                cmd1.Parameters.AddWithValue("@LogTagNo", LogTagNo);
                SqlDataReader rm1 = cmd1.ExecuteReader();

                if (rm1.HasRows)
                {
                    while (rm1.Read())
                    {
                        if (!rm1.IsDBNull(0))
                        {
                            Franking = rm1.GetDecimal(0);
                        }
                        else
                        {
                            Franking = 0;
                        }

                        if (!rm1.IsDBNull(1))
                        {
                            Airmail = rm1.GetDecimal(1);
                        }
                        else
                        {
                            Airmail = 0;
                        }

                        if (!rm1.IsDBNull(2))
                        {
                            Spore = rm1.GetDecimal(2);
                        }
                        else
                        {
                            Spore = 0;
                        }

                        if (!rm1.IsDBNull(3))
                        {
                            Tletter = rm1.GetDecimal(3);
                        }
                        else
                        {
                            Tletter = 0;
                        }

                        if (!rm1.IsDBNull(4))
                        {
                            NPC = rm1.GetDecimal(4);
                        }
                        else
                        {
                            NPC = 0;
                        }

                        if (!rm1.IsDBNull(5))
                        {
                            Mix = rm1.GetDecimal(5);
                        }
                        else
                        {
                            Mix = 0;
                        }

                    }
                }

                cn.Close();
            }

            Total = Franking + Airmail + Spore + Tletter + NPC + Mix;

            return Total.ToString();
        }

        public List<string> getPrintedPostDate(string LogTagNo)
        {
            List<string> PrintedPost = new List<string>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();

                SqlCommand cmd1 = new SqlCommand("SELECT FORMAT(CONVERT(date, Hist_ProductionSlip.StartDateOn), 'yyyy-MM-dd') as StartDateOn, FORMAT(CONVERT(date, PostingManifest.PostingDateOn), 'yyyy-MM-dd') as PostingDateOn FROM Hist_ProductionSlip INNER JOIN PostingManifest ON Hist_ProductionSlip.LogTagNo=PostingManifest.LogTagNo WHERE Hist_ProductionSlip.LogTagNo=@LogTagNo", cn);
                cmd1.Parameters.AddWithValue("@LogTagNo", LogTagNo);
                SqlDataReader rm1 = cmd1.ExecuteReader();

                if (rm1.HasRows)
                {
                    while (rm1.Read())
                    {
                        if (!rm1.IsDBNull(0))
                        {
                            PrintedPost.Add(rm1["StartDateOn"].ToString());
                        }
                        else
                        {
                            PrintedPost.Add("-");
                        }

                        if (!rm1.IsDBNull(1))
                        {
                            PrintedPost.Add(rm1["PostingDateOn"].ToString());
                        }
                        else
                        {
                            PrintedPost.Add("-");
                        }
                    }
                }
                else
                {
                    PrintedPost.Add("-");
                    PrintedPost.Add("-");
                }

                cn.Close();
            }

            return PrintedPost;
        }
        public string getRebate(string LogTagno)
        {
            string RebateCharges = "";
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();

                SqlCommand cmd1 = new SqlCommand("SELECT RebateCharges FROM BillingMPR WHERE JobSheetNo = @JobSheetNo1", cn);
                cmd1.Parameters.AddWithValue("@JobSheetNo1", LogTagno);
                SqlDataReader rm1 = cmd1.ExecuteReader();

                while (rm1.Read())
                {
                    if (!rm1.IsDBNull(0))
                    {
                        RebateCharges = rm1.GetString(0);
                    }
                    else
                    {
                        RebateCharges = "0.00";

                    }
                }

                cn.Close();
            }
            return RebateCharges;
        }

        public string getPostage(string LogTagNo)
        {
            List<double> PostageTotal = new List<double>();
            List<string> ItemList = new List<string>();
            double PostageTotalAmount = 0;
            string PostageTotalAmountStr = "";
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                for (int index = 1; index <= 8; index++)
                {
                    SqlCommand cmd1 = new SqlCommand("SELECT Postage" + index + "Charges FROM BillingMPR WHERE JobSheetNo=@LogTagNo1", cn);
                    cmd1.Parameters.AddWithValue("@LogTagNo1", LogTagNo);
                    SqlDataReader rm1 = cmd1.ExecuteReader();

                    while (rm1.Read())
                    {
                        if (!string.IsNullOrEmpty(rm1["Postage" + index + "Charges"].ToString()))
                        {
                            ItemList.Add("Postage" + index + "Charges");

                        }
                    }

                }

                foreach (var items in ItemList)
                {
                    Debug.WriteLine(items);
                }

                foreach (var item in ItemList)
                {
                    SqlCommand cmd2 = new SqlCommand("SELECT " + item + " FROM BillingMPR WHERE JobSheetNo=@LogTagNo2", cn);
                    cmd2.Parameters.AddWithValue("@LogTagNo2", LogTagNo);
                    SqlDataReader rm2 = cmd2.ExecuteReader();

                    while (rm2.Read())
                    {
                        if (rm2.HasRows)
                        {
                            double postageValue = double.Parse(rm2.GetString(0));
                            PostageTotal.Add(postageValue);
                        }
                    }

                }


                if (PostageTotal.Count > 0)
                {
                    PostageTotalAmount = PostageTotal.Sum();
                    PostageTotalAmountStr = PostageTotalAmount.ToString();
                }
                else
                {
                    PostageTotalAmountStr = "0.00";
                }

                cn.Close();

            }
            return PostageTotalAmountStr;
        }

        public string getRegisteredMails(string LogTagNo)
        {
            List<double> RegisteredMailsTotal = new List<double>();
            string TotalString = "";
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();

                SqlCommand cmd1 = new SqlCommand("SELECT RegisteredMailsCharges, RegisteredMails2Charges FROM BillingMPR WHERE JobSheetNo=@LogTagNo1", cn);
                cmd1.Parameters.AddWithValue("@LogTagNo1", LogTagNo);
                SqlDataReader rm1 = cmd1.ExecuteReader();
                while (rm1.Read())
                {
                    if (!string.IsNullOrEmpty(rm1["RegisteredMailsCharges"].ToString()))
                    {

                        double DBValueDouble = double.Parse(rm1.GetString(0));

                        RegisteredMailsTotal.Add(DBValueDouble);

                    }

                    if (!string.IsNullOrEmpty(rm1["RegisteredMails2Charges"].ToString()))
                    {

                        double DBValueDouble = double.Parse(rm1.GetString(1));

                        RegisteredMailsTotal.Add(DBValueDouble);

                    }


                }

                if (RegisteredMailsTotal.Count > 0)
                {
                    TotalString = RegisteredMailsTotal.Sum().ToString();
                }
                else
                {
                    TotalString = "0.00";
                }

                cn.Close();
            }

            return TotalString;
        }

        public string getFranking(string LogTagNo)
        {
            List<double> FrankingTotal = new List<double>();
            string TotalAmount = "";

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand cmd1 = new SqlCommand("SELECT FrankingCharges FROM BillingMPR WHERE JobSheetNo = @LogTagNo1", cn);
                cmd1.Parameters.AddWithValue("@LogTagNo1", LogTagNo);
                SqlDataReader rm1 = cmd1.ExecuteReader();

                while (rm1.Read())
                {
                    if (!string.IsNullOrEmpty(rm1["FrankingCharges"].ToString()))
                    {
                        double DBValueDDouble = double.Parse(rm1.GetString(0));
                        FrankingTotal.Add(DBValueDDouble);
                    }
                }


                if (FrankingTotal.Count > 0)
                {
                    TotalAmount = FrankingTotal.Sum().ToString();
                }
                else
                {
                    TotalAmount = "0.00";
                }

                cn.Close();
            }

            return TotalAmount;
        }

        public string getProgrammingCharges(string LogTagNo)
        {
            List<double> TotalCharges = new List<double>();
            string DBValue = "";
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();

                SqlCommand cmd1 = new SqlCommand("SELECT ProgrammingCharges FROM BillingMPR WHERE JobSheetNo = @LogTagNo1", cn);
                cmd1.Parameters.AddWithValue("@LogTagNo1", LogTagNo);
                SqlDataReader rm1 = cmd1.ExecuteReader();

                while (rm1.Read())
                {
                    //if (!string.IsNullOrEmpty(rm1.GetString(0)))
                    //{
                    //    if(!rm1.IsDBNull(0))
                    //    {
                    //        //double DBDoubleValue = rm1.GetDouble(0);
                    //        //DBValue = DBDoubleValue.ToString();

                    //        DBValue = rm1.GetString(0);
                    //    }
                    //    else
                    //    {
                    //        DBValue = "0.00";
                    //    }

                    //}
                    //else
                    //{
                    //    DBValue = "0.00";
                    //}

                    if (!rm1.IsDBNull(0))
                    {
                        //double DBDoubleValue = rm1.GetDouble(0);
                        //DBValue = DBDoubleValue.ToString();

                        DBValue = rm1.GetString(0);
                    }
                    else
                    {
                        DBValue = "0.00";
                    }
                }

                cn.Close();
            }

            return DBValue;
        }

        [HttpPost]
        public ActionResult getReportMaster(Finance get, string id, string DateStartTxt, string DateEndTxt, string Customer_Name, string ServiceChange, string Postage)
        {
            ViewBag.IsDepart = @Session["Department"];
            ViewBag.IsRole = @Session["Role"];

            if (!string.IsNullOrEmpty(Customer_Name))
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    DateTime DateStart = Convert.ToDateTime(DateStartTxt);
                    DateTime DateEnd = Convert.ToDateTime(DateEndTxt);

                    var DateStart1 = DateStart.ToString("yyyy-MM-dd");
                    var DateEnd1 = DateEnd.ToString("yyyy-MM-dd");

                    DateStartTxt = DateStart.ToString("ddMMyyyy");
                    DateEndTxt = DateEnd.ToString("ddMMyyyy");



                    //List<Finance> ViewList = new List<Finance>();
                    //using (SqlCommand command2 = new SqlCommand("", cn))
                    //{
                    //    cn.Open();
                    //    command2.Parameters.Clear();
                    //    command2.CommandText = @"SELECT DISTINCT JobInstruction.Customer_Name, JobInstruction.ProductName,JobAuditTrailDetail.LogTagNo, JobAuditTrailDetail.PageQty, JobAuditTrailDetail.ImpQty, JobAuditTrailDetail.AccQty, JobAuditTrailDetail.RevStrtDateOn, JobAuditTrailDetail.RevStrtTime, Hist_ProductionSlip.StartDateOn, Hist_ProductionSlip.StartTime,  PostingManifest.PostingDateOn, MailFrankingPosting.Total, TblBilling.TotalAmountService, TblMaterials.DescriptionMaterials, TblBilling.Sst,TblMaterials.Paper,TblMaterials.Env,TblBilling.TotalAmountPostage,TblBilling.ServiceChange,TblBilling.Postage
                    //                           FROM  JobInstruction INNER JOIN
                    //                           JobAuditTrailDetail ON JobInstruction.Id = JobAuditTrailDetail.JobAuditTrailId INNER JOIN
                    //                           PostingManifest ON JobInstruction.Id = PostingManifest.JobInstructionId INNER JOIN
                    //                           MailFrankingPosting ON JobInstruction.Id = MailFrankingPosting.JobInstructionId INNER JOIN
                    //                           Hist_ProductionSlip ON JobInstruction.Id = Hist_ProductionSlip.ProductionSlipId INNER JOIN
                    //                           TblBilling ON JobInstruction.Id = TblBilling.JobInstructionId INNER JOIN
                    //                           TblMaterials ON JobInstruction.Id = TblMaterials.JobInstructionId

                    //                       WHERE JobInstruction.Customer_Name LIKE @Customer_Name AND 
                    //                       LEFT( CONVERT(varchar, JobInstruction.ModifiedOn, 120), 10) >= @dateStart
                    //                       AND LEFT( CONVERT(varchar, TblBilling.CreatedOn, 120), 10) <= @dateEnd  

                    //                      ORDER BY LogTagNo DESC";
                    //    command2.Parameters.Add("@dateStart", SqlDbType.DateTime).Value = DateStart1;
                    //    command2.Parameters.Add("@dateEnd", SqlDbType.DateTime).Value = DateEnd1;
                    //    command2.Parameters.AddWithValue("@Customer_Name", "%" + Customer_Name + "%");


                    //    var reader = command2.ExecuteReader();
                    //    while (reader.Read())
                    //    {
                    //        Finance list = new Finance();
                    //        {

                    //            if (reader.IsDBNull(0) == false)
                    //            {
                    //                list.Customer_Name = reader.GetString(0);
                    //            }
                    //            if (reader.IsDBNull(1) == false)
                    //            {
                    //                list.ProductName = reader.GetString(1);
                    //            }
                    //            if (reader.IsDBNull(2) == false)
                    //            {
                    //                list.LogTagNo = reader.GetString(2);
                    //            }
                    //            if (reader.IsDBNull(3) == false)
                    //            {
                    //                list.PageQty = reader.GetString(3);
                    //            }
                    //            if (reader.IsDBNull(4) == false)
                    //            {
                    //                list.ImpQty = reader.GetString(4);
                    //            }
                    //            if (reader.IsDBNull(5) == false)
                    //            {
                    //                list.AccQty = reader.GetString(5);
                    //            }
                    //            if (reader.IsDBNull(6) == false)
                    //            {
                    //                list.RevStrtDateOnTxt = reader.GetDateTime(6).ToString("dd/MM/yyyy");
                    //            }
                    //            if (reader.IsDBNull(7) == false)
                    //            {
                    //                list.RevStrtTime = reader.GetString(7);
                    //            }
                    //            if (reader.IsDBNull(8) == false)
                    //            {
                    //                list.StartDateOnTxt = reader.GetDateTime(8).ToString("dd/MM/yyyy"); 
                    //            }

                    //            if (reader.IsDBNull(9) == false)
                    //            {
                    //                list.StartTime = reader.GetString(9);
                    //            }
                    //            if (reader.IsDBNull(10) == false)
                    //            {
                    //                list.PostingDateOnTxt= reader.GetDateTime(10).ToString("dd/MM/yyyy");
                    //            }


                    //            if (reader.IsDBNull(11) == false)
                    //            {
                    //                list.Total = reader.GetString(11);
                    //            }


                    //            if (reader.IsDBNull(12) == false)
                    //            {
                    //                list.TotalAmountService = reader.GetString(12);
                    //            }
                    //            if (reader.IsDBNull(13) == false)
                    //            {
                    //                list.DescriptionMaterials = reader.GetString(13);
                    //            }

                    //            if (reader.IsDBNull(14) == false)
                    //            {
                    //                list.Sst = reader.GetString(14);
                    //            }
                    //            if (reader.IsDBNull(15) == false)
                    //            {
                    //                list.Paper = reader.GetString(15);
                    //            }
                    //            if (reader.IsDBNull(16) == false)
                    //            {
                    //                list.Env = reader.GetString(16);
                    //            }
                    //            if (reader.IsDBNull(17) == false)
                    //            {
                    //                list.TotalAmountPostage = reader.GetString(17);
                    //            }
                    //            if (reader.IsDBNull(18) == false)
                    //            {
                    //                list.Process = reader.GetString(18);
                    //            }
                    //        }
                    //        ViewList.Add(list);
                    //    }
                    //    cn.Close();
                    //}

                    //cn.Close();
                    //ExcelPackage excel = new ExcelPackage();
                    //var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                    //workSheet.TabColor = System.Drawing.Color.Black;

                    //workSheet.DefaultRowHeight = 12;
                    //workSheet.Row(1).Height = 20;
                    //workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //workSheet.Row(1).Style.Font.Bold = true;

                    //workSheet.Cells[1, 1].Value = "CUSTOMER";
                    //workSheet.Cells[1, 2].Value = "PRODUCTNAME";
                    //workSheet.Cells[1, 3].Value = "LOGTAGNO";
                    //workSheet.Cells[1, 4].Value = "PAPER";
                    //workSheet.Cells[1, 5].Value = "IMPRESSION";
                    //workSheet.Cells[1, 6].Value = "ACCOUNT";
                    //workSheet.Cells[1, 7].Value = "COLLECTION DATE";
                    //workSheet.Cells[1, 8].Value = "COLLECTION TIME";
                    //workSheet.Cells[1, 9].Value = "PRINTED DATE";
                    //workSheet.Cells[1, 10].Value = "POSTING DATE";
                    //workSheet.Cells[1, 11].Value = "PAPER";
                    //workSheet.Cells[1, 12].Value = "ENV";
                    //workSheet.Cells[1, 13].Value = "SERVICE CHARGES";
                    //workSheet.Cells[1, 14].Value = "POSTAGE";
                    //workSheet.Cells[1, 15].Value = "FRANKING";
                    //workSheet.Cells[1, 16].Value = "SST 6%";
                    //workSheet.Cells[1, 17].Value = "TOTAL";                   




                    //int recordIndex = 2;

                    //foreach (var CLM in ViewList)
                    //{
                    //    workSheet.Cells[recordIndex, 1].Value = CLM.Customer_Name;
                    //    workSheet.Cells[recordIndex, 2].Value = CLM.ProductName;
                    //    workSheet.Cells[recordIndex, 3].Value = CLM.LogTagNo;
                    //    workSheet.Cells[recordIndex, 4].Value = CLM.PageQty;
                    //    workSheet.Cells[recordIndex, 5].Value = CLM.ImpQty;
                    //    workSheet.Cells[recordIndex, 6].Value = CLM.AccQty;
                    //    workSheet.Cells[recordIndex, 7].Value = CLM.RevStrtDateOnTxt;
                    //    workSheet.Cells[recordIndex, 8].Value = CLM.RevStrtTime;
                    //    workSheet.Cells[recordIndex, 9].Value = CLM.StartDateOnTxt;
                    //    workSheet.Cells[recordIndex, 10].Value = CLM.PostingDateOnTxt;
                    //    workSheet.Cells[recordIndex, 11].Value = CLM.Paper;
                    //    workSheet.Cells[recordIndex, 12].Value = CLM.Env;
                    //    workSheet.Cells[recordIndex, 13].Value = CLM.TotalAmountService;
                    //    workSheet.Cells[recordIndex, 14].Value = CLM.TotalAmountPostage;
                    //    workSheet.Cells[recordIndex, 15].Value = CLM.Sst;
                    //    workSheet.Cells[recordIndex, 16].Value = CLM.TotalAmountx;



                    //    recordIndex++;
                    //}



                    //workSheet.Column(1).AutoFit();
                    //workSheet.Column(2).AutoFit();
                    //workSheet.Column(3).AutoFit();
                    //workSheet.Column(4).AutoFit();
                    //workSheet.Column(5).AutoFit();
                    //workSheet.Column(6).AutoFit();
                    //workSheet.Column(7).AutoFit();
                    //workSheet.Column(8).AutoFit();
                    //workSheet.Column(9).AutoFit();
                    //workSheet.Column(10).AutoFit();
                    //workSheet.Column(11).AutoFit();
                    //workSheet.Column(12).AutoFit();
                    //workSheet.Column(13).AutoFit();
                    //workSheet.Column(14).AutoFit();
                    //workSheet.Column(15).AutoFit();

                    List<Finance> ViewList = new List<Finance>();
                    using (SqlCommand command2 = new SqlCommand("", cn))
                    {
                        cn.Open();
                        command2.Parameters.Clear();
                        command2.CommandText = @"SELECT DISTINCT JobInstruction.Customer_Name, JobInstruction.ProductName,JobAuditTrailDetail.LogTagNo, JobAuditTrailDetail.PageQty, JobAuditTrailDetail.ImpQty, JobAuditTrailDetail.AccQty, JobAuditTrailDetail.RevStrtDateOn, JobAuditTrailDetail.RevStrtTime, Hist_ProductionSlip.StartDateOn, Hist_ProductionSlip.StartTime,  PostingManifest.PostingDateOn, MailFrankingPosting.Total, TblBilling.TotalAmountService, TblMaterials.DescriptionMaterials, TblBilling.Sst,TblMaterials.Paper,TblMaterials.Env,TblBilling.TotalAmountPostage,TblBilling.Process,TblBilling.TotalAmountO,TblBilling.TotalAmountO2,TblBilling.TotalAmountO3,TblBilling.TotalAmountF, TblMaterials.TotalAmountPaper,TblMaterials.TotalAmountEnv
                                               FROM  JobInstruction INNER JOIN
                                               JobAuditTrailDetail ON JobInstruction.Id = JobAuditTrailDetail.JobAuditTrailId INNER JOIN
                                               PostingManifest ON JobInstruction.Id = PostingManifest.JobInstructionId INNER JOIN
                                               MailFrankingPosting ON JobInstruction.Id = MailFrankingPosting.JobInstructionId INNER JOIN
                                               Hist_ProductionSlip ON JobInstruction.Id = Hist_ProductionSlip.ProductionSlipId INNER JOIN
                                               TblBilling ON JobInstruction.Id = TblBilling.JobInstructionId INNER JOIN
                                               TblMaterials ON JobInstruction.Id = TblMaterials.JobInstructionId
                                              
                                            WHERE JobInstruction.Customer_Name LIKE @Customer_Name AND 
                                         LEFT( CONVERT(varchar, JobInstruction.ModifiedOn, 120), 10) >= @dateStart
                                          AND LEFT( CONVERT(varchar, TblBilling.CreatedOn, 120), 10) <= @dateEnd 
                                           
                                          ORDER BY LogTagNo DESC";
                        command2.Parameters.Add("@dateStart", SqlDbType.DateTime).Value = DateStart1;
                        command2.Parameters.Add("@dateEnd", SqlDbType.DateTime).Value = DateEnd1;
                        command2.Parameters.AddWithValue("@Customer_Name", "%" + Customer_Name + "%");
                        var reader = command2.ExecuteReader();
                        while (reader.Read())
                        {
                            Finance list = new Finance();
                            {

                                if (reader.IsDBNull(0) == false)
                                {
                                    list.Customer_Name = reader.GetString(0);
                                }
                                if (reader.IsDBNull(1) == false)
                                {
                                    list.ProductName = reader.GetString(1);
                                }
                                if (reader.IsDBNull(2) == false)
                                {
                                    list.LogTagNo = reader.GetString(2);
                                }
                                if (reader.IsDBNull(3) == false)
                                {
                                    list.PageQty = reader.GetString(3);
                                }
                                if (reader.IsDBNull(4) == false)
                                {
                                    list.ImpQty = reader.GetString(4);
                                }
                                if (reader.IsDBNull(5) == false)
                                {
                                    list.AccQty = reader.GetString(5);
                                }
                                if (reader.IsDBNull(6) == false)
                                {
                                    list.RevStrtDateOnTxt = reader.GetDateTime(6).ToString("dd/MM/yyyy");
                                }
                                if (reader.IsDBNull(7) == false)
                                {
                                    list.RevStrtTime = reader.GetString(7);
                                }
                                if (reader.IsDBNull(8) == false)
                                {
                                    list.StartDateOnTxt = reader.GetDateTime(8).ToString("dd/MM/yyyy");
                                }
                                if (reader.IsDBNull(9) == false)
                                {
                                    list.StartTime = reader.GetString(9);
                                }

                                if (reader.IsDBNull(10) == false)
                                {
                                    list.PostingDateOnTxt = reader.GetDateTime(10).ToString("dd/MM/yyyy");
                                }
                                if (reader.IsDBNull(11) == false)
                                {
                                    list.Total = reader.GetString(11);
                                }
                                if (reader.IsDBNull(12) == false)
                                {
                                    list.TotalAmountService = reader.GetString(12);
                                }
                                if (reader.IsDBNull(13) == false)
                                {
                                    list.DescriptionMaterials = reader.GetString(13);
                                }
                                if (reader.IsDBNull(14) == false)
                                {
                                    list.Sst = reader.GetString(14);
                                }
                                if (reader.IsDBNull(15) == false)
                                {
                                    list.Paper = reader.GetString(15);
                                }
                                if (reader.IsDBNull(16) == false)
                                {
                                    list.Env = reader.GetString(16);
                                }
                                if (reader.IsDBNull(17) == false)
                                {
                                    list.TotalAmountPostage = reader.GetString(17);
                                }

                                if (reader.IsDBNull(18) == false)
                                {
                                    list.Process = reader.GetString(18);
                                }

                                if (reader.IsDBNull(19) == false)
                                {
                                    list.TotalAmountO = reader.GetString(19);
                                }
                                if (reader.IsDBNull(20) == false)
                                {
                                    list.TotalAmountO2 = reader.GetString(20);
                                }

                                if (reader.IsDBNull(21) == false)
                                {
                                    list.TotalAmountO3 = reader.GetString(21);
                                }
                                if (reader.IsDBNull(22) == false)
                                {
                                    list.TotalAmountF = reader.GetString(22);
                                }

                                if (reader.IsDBNull(23) == false)
                                {
                                    list.TotalAmountPaper = reader.GetString(23);
                                }
                                if (reader.IsDBNull(24) == false)
                                {
                                    list.TotalAmountEnv = reader.GetString(24);
                                }

                            }
                            ViewList.Add(list);
                        }
                        cn.Close();
                    }

                    cn.Close();
                    ExcelPackage excel = new ExcelPackage();
                    var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                    workSheet.TabColor = System.Drawing.Color.Black;

                    workSheet.DefaultRowHeight = 12;
                    workSheet.Row(1).Height = 20;
                    workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(1).Style.Font.Bold = true;

                    workSheet.Cells[1, 1].Value = "CUSTOMER";
                    workSheet.Cells[1, 2].Value = "PRODUCTNAME";
                    workSheet.Cells[1, 3].Value = "LOGTAGNO";
                    workSheet.Cells[1, 4].Value = "PAPER";
                    workSheet.Cells[1, 5].Value = "IMPRESSION";
                    workSheet.Cells[1, 6].Value = "ACCOUNT";
                    workSheet.Cells[1, 7].Value = "COLLECTION DATE";
                    workSheet.Cells[1, 8].Value = "COLLECTION TIME";
                    workSheet.Cells[1, 9].Value = "PRINTED DATE";
                    workSheet.Cells[1, 10].Value = "POSTING DATE";
                    workSheet.Cells[1, 11].Value = "PAPER";
                    workSheet.Cells[1, 12].Value = "ENV";
                    workSheet.Cells[1, 13].Value = "SERVICE CHARGES";
                    workSheet.Cells[1, 14].Value = "POSTAGE";
                    workSheet.Cells[1, 15].Value = "REGISTERED MAILS";
                    workSheet.Cells[1, 16].Value = "FRANKING";
                    workSheet.Cells[1, 17].Value = "SERVICE CHARGES 10%";
                    workSheet.Cells[1, 18].Value = "SST 6%";
                    workSheet.Cells[1, 19].Value = "PROGRAMMING CHARGE";
                    workSheet.Cells[1, 20].Value = "TOTAL";




                    int recordIndex = 2;

                    foreach (var CLM in ViewList)
                    {
                        workSheet.Cells[recordIndex, 1].Value = CLM.Customer_Name;
                        workSheet.Cells[recordIndex, 2].Value = CLM.ProductName;
                        workSheet.Cells[recordIndex, 3].Value = CLM.LogTagNo;
                        workSheet.Cells[recordIndex, 4].Value = CLM.PageQty;
                        workSheet.Cells[recordIndex, 5].Value = CLM.ImpQty;
                        workSheet.Cells[recordIndex, 6].Value = CLM.AccQty;
                        workSheet.Cells[recordIndex, 7].Value = CLM.RevStrtDateOnTxt;
                        workSheet.Cells[recordIndex, 8].Value = CLM.RevStrtTime;
                        workSheet.Cells[recordIndex, 9].Value = CLM.StartDateOnTxt;
                        workSheet.Cells[recordIndex, 10].Value = CLM.PostingDateOnTxt;
                        workSheet.Cells[recordIndex, 11].Value = CLM.TotalAmountPaper;
                        workSheet.Cells[recordIndex, 12].Value = CLM.TotalAmountEnv;
                        workSheet.Cells[recordIndex, 13].Value = CLM.TotalAmountService;
                        workSheet.Cells[recordIndex, 14].Value = CLM.TotalAmountPostage;
                        workSheet.Cells[recordIndex, 15].Value = CLM.TotalAmountO;
                        workSheet.Cells[recordIndex, 16].Value = CLM.TotalAmountF;
                        workSheet.Cells[recordIndex, 17].Value = CLM.TotalAmountO2;
                        workSheet.Cells[recordIndex, 18].Value = CLM.Sst;
                        workSheet.Cells[recordIndex, 19].Value = CLM.TotalAmountO3;
                        workSheet.Cells[recordIndex, 20].Value = CLM.TotalAmountx;



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

                    string excelName = " MASTER REPORT-" + DateStartTxt + "-" + DateEndTxt;
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
            }

            return RedirectToAction("ListMasterReportFin", "Report");
        }


        //[HttpPost]
        //public ActionResult getReportMaster(JobInstruction get, string id, string DateStartTxt, string DateEndTxt,string Customer_Name)
        //{
        //    ViewBag.IsDepart = @Session["Department"];
        //    ViewBag.IsRole = @Session["Role"];

        //    if (!string.IsNullOrEmpty(Customer_Name) )
        //    {
        //        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        //        {
        //            DateTime DateStart = Convert.ToDateTime(DateStartTxt);
        //            DateTime DateEnd = Convert.ToDateTime(DateEndTxt);

        //            var DateStart1 = DateStart.ToString("yyyy-MM-dd");
        //            var DateEnd1 = DateEnd.ToString("yyyy-MM-dd");

        //            DateStartTxt = DateStart.ToString("ddMMyyyy");
        //            DateEndTxt = DateEnd.ToString("ddMMyyyy");

        //            List<Finance> gotlist = new List<Finance>();
        //            cn.Open();
        //            SqlCommand command;
        //            command = new SqlCommand(@"SELECT JobInstruction.Customer_Name, JobInstruction.ProductName, TblBilling.Process, TblBilling.GrandTotal, TblBilling.TotalAmount, PostingManifest.PostingDateOn, PostingManifest.PostingTime, Finance.Cust_Department, JobAuditTrailDetail.AccQty, JobAuditTrailDetail.ImpQty, JobAuditTrailDetail.PageQty, Finance.Contact_Person, Finance.InvoiceNo, Finance.CreatedOn
        //                          FROM  JobInstruction INNER JOIN
        //                     Finance ON JobInstruction.Id = Finance.JobInstructionId INNER JOIN
        //                       PostingManifest ON JobInstruction.Id = PostingManifest.JobInstructionId INNER JOIN
        //                     JobAuditTrailDetail ON JobInstruction.Id = JobAuditTrailDetail.JobAuditTrailId INNER JOIN
        //                        TblBilling ON Finance.JobInstructionId = TblBilling.JobInstructionId
        //                                   WHERE JobInstruction.Customer_Name=@Customer_Name AND
        //                                   LEFT( CONVERT(varchar, JobInstruction.ModifiedOn, 120), 10) >= @dateStart
        //                                   AND LEFT( CONVERT(varchar, TblBilling.CreatedOn, 120), 10) <= @dateEnd  

        //                                   ORDER BY JobInstruction.ModifiedOn", cn);
        //            command.Parameters.Add("@dateStart", SqlDbType.DateTime).Value = DateStart1;
        //            command.Parameters.Add("@dateEnd", SqlDbType.DateTime).Value = DateEnd1;
        //            command.Parameters.AddWithValue("@Customer_Name", "%" + Customer_Name + "%");
        //            var reader = command.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                Finance list = new Finance();
        //                {

        //                    if (reader.IsDBNull(0) == false)
        //                    {
        //                        list.Customer_Name = reader.GetString(0);
        //                    }
        //                    if (reader.IsDBNull(1) == false)
        //                    {
        //                        list.ProductName = reader.GetString(1);
        //                    }
        //                    if (reader.IsDBNull(2) == false)
        //                    {
        //                        list.Process = reader.GetString(2);
        //                    }
        //                    if (reader.IsDBNull(3) == false)
        //                    {
        //                        list.GrandTotal = reader.GetString(3);
        //                    }
        //                    if (reader.IsDBNull(4) == false)
        //                    {
        //                        list.TotalAmount = reader.GetString(4);
        //                    }
        //                    if (reader.IsDBNull(5) == false)
        //                    {
        //                        list.PostingDateOn = reader.GetDateTime(5);
        //                    }
        //                    if (reader.IsDBNull(6) == false)
        //                    {
        //                        list.PostingTime = reader.GetString(6);
        //                    }
        //                    if (reader.IsDBNull(7) == false)
        //                    {
        //                        list.Cust_Department = reader.GetString(7);
        //                    }
        //                    if (reader.IsDBNull(8) == false)
        //                    {
        //                        list.AccQty = reader.GetString(8);
        //                    }
        //                    if (reader.IsDBNull(9) == false)
        //                    {
        //                        list.ImpQty = reader.GetString(9);
        //                    }
        //                    if (reader.IsDBNull(10) == false)
        //                    {
        //                        list.PageQty = reader.GetString(10);
        //                    }
        //                    if (reader.IsDBNull(11) == false)
        //                    {
        //                        list.Contact_Person = reader.GetString(11);
        //                    }
        //                    if (reader.IsDBNull(12) == false)
        //                    {
        //                        list.InvoiceNo = reader.GetString(12);
        //                    }
        //                    if (reader.IsDBNull(13) == false)
        //                    {
        //                        list.InvoiceNo = reader.GetString(13);
        //                    }
        //                }
        //                ListJI.Add(list);
        //            }
        //            cn.Close();
        //            ExcelPackage excel = new ExcelPackage();
        //            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
        //            workSheet.TabColor = System.Drawing.Color.Black;

        //            workSheet.DefaultRowHeight = 12;
        //            workSheet.Row(1).Height = 20;
        //            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            workSheet.Row(1).Style.Font.Bold = true;
        //            workSheet.Cells[1, 1].Value = "NO.";
        //            workSheet.Cells[1, 2].Value = "INV NO";
        //            workSheet.Cells[1, 3].Value = "DATE INV";
        //            workSheet.Cells[1, 4].Value = "JOB NAME";
        //            workSheet.Cells[1, 5].Value = "PERSONAL CONTACT";
        //            workSheet.Cells[1, 6].Value = "DATA RECEIVED";
        //            workSheet.Cells[1, 7].Value = "TIME RECEIVED";
        //            workSheet.Cells[1, 8].Value = "PAPER";
        //            workSheet.Cells[1, 9].Value = "IMPRESSION";
        //            workSheet.Cells[1, 10].Value = "ACCOUNT";
        //            workSheet.Cells[1, 11].Value = "PROCESS AUDIT TRAIL";
        //            workSheet.Cells[1, 12].Value = "TOTAL CHARGES";
        //            workSheet.Cells[1, 13].Value = "GRAND TOTAL";





        //            int recordIndex = 2;
        //            foreach (var CLM in gotlist)
        //            {
        //                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
        //                workSheet.Cells[recordIndex, 2].Value = CLM.InvoiceNo;
        //                workSheet.Cells[recordIndex, 3].Value = CLM.CreatedOn;
        //                workSheet.Cells[recordIndex, 4].Value = CLM.ProductName;
        //                workSheet.Cells[recordIndex, 5].Value = CLM.Contact_Person;
        //                workSheet.Cells[recordIndex, 6].Value = CLM.PostingDateOn;
        //                workSheet.Cells[recordIndex, 7].Value = CLM.PostingTime;
        //                workSheet.Cells[recordIndex, 8].Value = CLM.PageQty;
        //                workSheet.Cells[recordIndex, 9].Value = CLM.ImpQty;
        //                workSheet.Cells[recordIndex, 10].Value = CLM.AccQty;
        //                workSheet.Cells[recordIndex, 11].Value = CLM.Process;
        //                workSheet.Cells[recordIndex, 12].Value = CLM.TotalAmount;
        //                workSheet.Cells[recordIndex, 13].Value = CLM.GrandTotal;

        //                recordIndex++;
        //            }
        //            workSheet.Column(1).AutoFit();
        //            workSheet.Column(2).AutoFit();
        //            workSheet.Column(3).AutoFit();
        //            workSheet.Column(4).AutoFit();
        //            workSheet.Column(5).AutoFit();
        //            workSheet.Column(6).AutoFit();
        //            workSheet.Column(7).AutoFit();
        //            workSheet.Column(8).AutoFit();
        //            workSheet.Column(9).AutoFit();
        //            workSheet.Column(10).AutoFit();
        //            workSheet.Column(11).AutoFit();
        //            workSheet.Column(12).AutoFit();
        //            workSheet.Column(13).AutoFit();

        //            string excelName = " MASTER REPORT-" + DateStartTxt + "-" + DateEndTxt;
        //            using (var memoryStream = new MemoryStream())
        //            {
        //                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
        //                excel.SaveAs(memoryStream);
        //                memoryStream.WriteTo(Response.OutputStream);
        //                Response.Flush();
        //                Response.End();
        //            }

        //        }
        //    }

        //    return RedirectToAction("ListMasterReportFin", "Report");
        //}




        [HttpPost]
        public ActionResult getReportMasterByCompany(string Customer_Name)
        {
            ViewBag.IsDepart = @Session["Department"];
            ViewBag.IsRole = @Session["Role"];

            if (!string.IsNullOrEmpty(Customer_Name))
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {


                    List<Finance> gotlist2 = new List<Finance>();
                    cn.Open();
                    SqlCommand command;
                    command = new SqlCommand(@"SELECT Finance.Contact_Person, Finance.Cust_Department, Finance.InvoiceNo, Finance.CreatedOn, Finance.Customer_Name, Finance.ProductName, JobAuditTrailDetail.AccQty, JobAuditTrailDetail.ImpQty, JobAuditTrailDetail.PageQty, PostingManifest.PostingDateOn, PostingManifest.PostingTime, PostingManifest.Re_turn
                                           FROM  JobInstruction INNER JOIN
                                            JobAuditTrailDetail ON JobInstruction.Id = JobAuditTrailDetail.JobInstructionId INNER JOIN
                                           Finance ON JobInstruction.Id = Finance.JobInstructionId INNER JOIN
                                         PostingManifest ON JobInstruction.Id = PostingManifest.JobInstructionId
                                           WHERE JobInstruction.Customer_Name =@Customer_Name                                      
                                           ORDER BY JobInstruction.ModifiedOn", cn);
                    command.Parameters.AddWithValue("@Customer_Name", "%" + Customer_Name + "%");

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Finance listx = new Finance();
                        {
                            if (reader.IsDBNull(0) == false)
                            {
                                listx.Contact_Person = reader.GetString(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                listx.Cust_Department = reader.GetString(1);
                            }
                            if (reader.IsDBNull(2) == false)
                            {
                                listx.InvoiceNo = reader.GetString(2);
                            }
                            if (reader.IsDBNull(3) == false)
                            {
                                listx.CreatedOn = reader.GetDateTime(3);
                            }
                            if (reader.IsDBNull(4) == false)
                            {
                                listx.Customer_Name = reader.GetString(4);
                            }
                            if (reader.IsDBNull(5) == false)
                            {
                                listx.ProductName = reader.GetString(5);
                            }

                            if (reader.IsDBNull(6) == false)
                            {
                                listx.AccQty = reader.GetString(6);
                            }
                            if (reader.IsDBNull(7) == false)
                            {
                                listx.ImpQty = reader.GetString(7);
                            }
                            if (reader.IsDBNull(8) == false)
                            {
                                listx.Process = reader.GetString(8);
                            }
                            if (reader.IsDBNull(9) == false)
                            {
                                listx.PostingDateOn = reader.GetDateTime(9);
                            }

                            if (reader.IsDBNull(10) == false)
                            {
                                listx.PostingTime = reader.GetString(10);
                            }

                            if (reader.IsDBNull(11) == false)
                            {
                                listx.Re_turn = reader.GetString(11);
                            }
                        }

                        gotlist2.Add(listx);

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
                    workSheet.Cells[1, 2].Value = "CONTACT PERSON";
                    workSheet.Cells[1, 3].Value = "CUSTOMER DEPARTMENT";
                    workSheet.Cells[1, 4].Value = "INVOICE NO";
                    workSheet.Cells[1, 5].Value = "CREATE DATE";
                    workSheet.Cells[1, 6].Value = "CUSTOMER NAME";
                    workSheet.Cells[1, 7].Value = "PRODUCT NAME";
                    workSheet.Cells[1, 8].Value = "ACCOUNTS Qty";
                    workSheet.Cells[1, 9].Value = "IMP Qty";
                    workSheet.Cells[1, 10].Value = "PAGE Qty";



                    int recordIndex = 2;
                    foreach (var CLM in gotlist2)
                    {
                        workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
                        workSheet.Cells[recordIndex, 2].Value = CLM.Contact_Person;
                        workSheet.Cells[recordIndex, 3].Value = CLM.Cust_Department;
                        workSheet.Cells[recordIndex, 4].Value = CLM.InvoiceNo;
                        workSheet.Cells[recordIndex, 5].Value = CLM.CreatedOn;
                        workSheet.Cells[recordIndex, 6].Value = CLM.Customer_Name;
                        workSheet.Cells[recordIndex, 7].Value = CLM.ProductName;
                        workSheet.Cells[recordIndex, 8].Value = CLM.AccQty;
                        workSheet.Cells[recordIndex, 9].Value = CLM.ImpQty;
                        workSheet.Cells[recordIndex, 10].Value = CLM.Process;

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


                    string excelName = "REPORT-" + "-" + Customer_Name;
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
            }
            return RedirectToAction("ListReportFin", "Report");


        }

        List<Finance> ListJI = new List<Finance>();
        public ActionResult ListMasterReportFin(string set, string Customer_Name, FormCollection formcollection)
        {
            ViewBag.IsDepart = @Session["Department"];
            ViewBag.IsRole = @Session["Role"];
            var Role = @Session["Role"];

            set = formcollection["set"];

            if (set == "search")
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    int _bil = 1;
                    cn.Open();
                    command.CommandText = @"SELECT Finance.Contact_Person, JobInstruction.ProductName, Finance.InvoiceNo, Finance.CreatedOn, Finance.Customer_Name, Finance.ProductName, JobAuditTrailDetail.AccQty, JobAuditTrailDetail.ImpQty, JobAuditTrailDetail.PageQty, PostingManifest.PostingDateOn, PostingManifest.PostingTime, Finance.Cust_Department
                                           FROM  JobInstruction INNER JOIN
                                            JobAuditTrailDetail ON JobInstruction.Id = JobAuditTrailDetail.JobInstructionId INNER JOIN
                                           Finance ON JobInstruction.Id = Finance.JobInstructionId INNER JOIN
                                         PostingManifest ON JobInstruction.Id = PostingManifest.JobInstructionId";
                    command.Parameters.AddWithValue("@Customer_Name", "%" + Customer_Name + "%");
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Finance model = new Finance();
                        {
                            model.Bil = _bil++;

                            if (reader.IsDBNull(0) == false)
                            {
                                model.InvoiceNo = reader.GetString(0);
                            }
                            if (reader.IsDBNull(1) == false)
                            {
                                model.CreatedOn = reader.GetDateTime(1);
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
                                model.Process = reader.GetString(4);
                            }
                            if (reader.IsDBNull(5) == false)
                            {
                                model.GrandTotal = reader.GetString(5);
                            }
                            if (reader.IsDBNull(6) == false)
                            {
                                model.TotalAmount = reader.GetString(6);
                            }
                            if (reader.IsDBNull(7) == false)
                            {
                                model.PostingDateOn = reader.GetDateTime(7);
                            }
                            if (reader.IsDBNull(8) == false)
                            {
                                model.PostingTime = reader.GetString(8);
                            }
                            if (reader.IsDBNull(9) == false)
                            {
                                model.Cust_Department = reader.GetString(9);
                            }
                        }
                        ListJI.Add(model);
                    }
                    cn.Close();
                }
            }

            else if (set == "search2")
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    int _bil = 1;
                    cn.Open();
                    command.CommandText = @"SELECT JobInstruction.Customer_Name, JobInstruction.ProductName,JobAuditTrailDetail.LogTagNo, JobAuditTrailDetail.PageQty, JobAuditTrailDetail.ImpQty, JobAuditTrailDetail.AccQty, JobAuditTrailDetail.RevStrtDateOn, JobAuditTrailDetail.RevStrtTime, Hist_ProductionSlip.StartDateOn, Hist_ProductionSlip.StartTime,  PostingManifest.PostingDateOn, MailFrankingPosting.Total, TblBilling.Process, TblBilling.TotalAmount, TblMaterials.DescriptionMaterials, TblMaterials.TotalAmount
                                               FROM  JobInstruction INNER JOIN
                                               JobAuditTrailDetail ON JobInstruction.Id = JobAuditTrailDetail.JobAuditTrailId INNER JOIN
                                               PostingManifest ON JobInstruction.Id = PostingManifest.JobInstructionId INNER JOIN
                                               MailFrankingPosting ON JobInstruction.Id = MailFrankingPosting.JobInstructionId INNER JOIN
                                               Hist_ProductionSlip ON JobInstruction.Id = Hist_ProductionSlip.ProductionSlipId INNER JOIN
                                               TblBilling ON JobInstruction.Id = TblBilling.JobInstructionId INNER JOIN
                                               TblMaterials ON JobInstruction.Id = TblMaterials.JobInstructionId
                                          WHERE JobInstruction.Customer_Name LIKE @Customer_Name AND JobInstruction.Status='FINANCE'";
                    command.Parameters.AddWithValue("@Customer_Name", "%" + Customer_Name + "%");
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Finance model = new Finance();
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
                                model.PageQty = reader.GetString(3);
                            }

                            if (reader.IsDBNull(4) == false)
                            {
                                model.ImpQty = reader.GetString(4);
                            }
                            if (reader.IsDBNull(5) == false)
                            {
                                model.AccQty = reader.GetString(5);
                            }
                            if (reader.IsDBNull(6) == false)
                            {
                                model.RevStrtDateOn = reader.GetDateTime(6);
                            }
                            if (reader.IsDBNull(7) == false)
                            {
                                model.RevStrtTime = reader.GetString(7);
                            }
                            if (reader.IsDBNull(8) == false)
                            {
                                model.StartDateOn = reader.GetDateTime(8);
                            }

                            if (reader.IsDBNull(9) == false)
                            {
                                model.StartTime = reader.GetString(9);
                            }
                            if (reader.IsDBNull(10) == false)
                            {
                                model.PostingDateOn = reader.GetDateTime(10);
                            }


                            if (reader.IsDBNull(11) == false)
                            {
                                model.Total = reader.GetString(11);
                            }
                            if (reader.IsDBNull(12) == false)
                            {
                                model.Process = reader.GetString(12);
                            }

                            if (reader.IsDBNull(13) == false)
                            {
                                model.TotalAmount = reader.GetString(13);
                            }
                            if (reader.IsDBNull(14) == false)
                            {
                                model.DescriptionMaterials = reader.GetString(14);
                            }
                            if (reader.IsDBNull(15) == false)
                            {
                                model.TotalAmount = reader.GetString(15);
                            }
                        }
                        ListJI.Add(model);
                    }
                    cn.Close();
                }
            }

            else
            {
                //ALL
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    int _bil = 1;
                    cn.Open();
                    command.CommandText = @"SELECT distinct JobInstruction.Customer_Name, JobInstruction.ProductName,JobAuditTrailDetail.LogTagNo, JobAuditTrailDetail.PageQty, JobAuditTrailDetail.ImpQty, JobAuditTrailDetail.AccQty, JobAuditTrailDetail.RevStrtDateOn, JobAuditTrailDetail.RevStrtTime
                                               FROM  JobInstruction INNER JOIN
                                               JobAuditTrailDetail ON JobInstruction.Id = JobAuditTrailDetail.JobAuditTrailId INNER JOIN
                                               PostingManifest ON JobInstruction.Id = PostingManifest.JobInstructionId INNER JOIN
                                               MailFrankingPosting ON JobInstruction.Id = MailFrankingPosting.JobInstructionId INNER JOIN
                                               Hist_ProductionSlip ON JobInstruction.Id = Hist_ProductionSlip.ProductionSlipId INNER JOIN
                                               TblBilling ON JobInstruction.Id = TblBilling.JobInstructionId INNER JOIN
                                               TblMaterials ON JobInstruction.Id = TblMaterials.JobInstructionId 
											   ORDER BY JobAuditTrailDetail.LogTagNo DESC ";
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Finance model = new Finance();
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
                                model.PageQty = reader.GetString(3);
                            }

                            if (reader.IsDBNull(4) == false)
                            {
                                model.ImpQty = reader.GetString(4);
                            }
                            if (reader.IsDBNull(5) == false)
                            {
                                model.AccQty = reader.GetString(5);
                            }
                            if (reader.IsDBNull(6) == false)
                            {
                                model.RevStrtDateOn = reader.GetDateTime(6);
                            }
                            if (reader.IsDBNull(7) == false)
                            {
                                model.RevStrtTime = reader.GetString(7);
                            }

                        }
                        ListJI.Add(model);
                    }
                    cn.Close();

                }
            }

            return View(ListJI);
        }

        List<DailyTracking> dailytrackingreport = new List<DailyTracking>();
        public ActionResult DailyTrackingReport(DailyTracking get)
        {
            List<string> customer = new List<string>();
            List<string> JobClass = new List<string>();
            List<string> Company = new List<string>();
            List<int> TotalAcc = new List<int>();
            List<int> TotalImp = new List<int>();
            List<int> TotalPages = new List<int>();



            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (SqlCommand command = new SqlCommand("", cn))
                {
                    cn.Open();
                    command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id from DailyTracking ORDER BY CreatedOn ASC";
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        DailyTracking model = new DailyTracking();
                        {
                            model.CreatedOn = reader.GetString(0);
                            model.JobClass = reader["JobClass"].ToString();
                            model.Customer_Name = reader["Customer_Name"].ToString();
                            model.ProductName = reader["ProductName"].ToString();
                            model.LogTagNo = reader["LogTagNo"].ToString();
                            model.Id = reader.GetGuid(5);

                        };

                        dailytrackingreport.Add(model);

                    }

                    reader.Close();

                }

                SqlCommand getcustomer = new SqlCommand("SELECT DISTINCT Customer_Name FROM DailyTracking EXCEPT select Customer_Name from DailyTracking WHERE Customer_Name IS NULL", cn);
                SqlDataReader rm = getcustomer.ExecuteReader();

                while (rm.Read())
                {
                    customer.Add(rm["Customer_Name"].ToString());
                }

                //foreach(var a in customer)
                //{
                //    Debug.WriteLine("customer list :" +a);
                //}

                ViewBag.customer = customer;

                rm.Close();

                SqlCommand getJobClass = new SqlCommand(" SELECT DISTINCT JobClass FROM DailyTracking EXCEPT select JobClass from DailyTracking WHERE JobClass IS NULL", cn);
                SqlDataReader rmjobclass = getJobClass.ExecuteReader();

                while (rmjobclass.Read())
                {
                    JobClass.Add(rmjobclass["JobClass"].ToString());
                }

                ViewBag.JobClass = JobClass;

                rmjobclass.Close();

                SqlCommand getcompany = new SqlCommand("SELECT DISTINCT Company FROM DailyTracking EXCEPT select Company from DailyTracking WHERE Company IS NULL", cn);
                SqlDataReader rmcompany = getcompany.ExecuteReader();

                while (rmcompany.Read())
                {
                    Company.Add(rmcompany["Company"].ToString());
                }

                Company.Add("None");
                ViewBag.Company = Company;

                rmcompany.Close();

                SqlCommand getTotal = new SqlCommand("SELECT count(Id) as totalid FROM DailyTracking", cn);
                SqlDataReader rmTotal = getTotal.ExecuteReader();

                while (rmTotal.Read())
                {
                    string totalstr = rmTotal["totalid"].ToString();

                    ViewBag.Total = totalstr;
                }


                rmTotal.Close();

                SqlCommand getTotalrecords = new SqlCommand(" SELECT AccountsQty, ImpressionQty, PagesQty from DailyTracking", cn);
                SqlDataReader rmTotalrecords = getTotalrecords.ExecuteReader();

                while (rmTotalrecords.Read())
                {
                    string totalAccstr = rmTotalrecords["AccountsQty"].ToString();
                    string totalImpstr = rmTotalrecords["ImpressionQty"].ToString();
                    string totalPagesstr = rmTotalrecords["PagesQty"].ToString();

                    //Debug.WriteLine("totalAccstr : " + totalAccstr);
                    //Debug.WriteLine("totalImpstr : " + totalImpstr);
                    //Debug.WriteLine("totalPagesstr : " + totalPagesstr);



                    try
                    {
                        int totalAcc = Convert.ToInt32(totalAccstr);
                        TotalAcc.Add(totalAcc);
                    }
                    catch
                    {
                        int totalAcc = 0;
                        TotalAcc.Add(totalAcc);
                    }

                    try
                    {
                        int totalImp = Convert.ToInt32(totalImpstr);
                        TotalImp.Add(totalImp);
                    }
                    catch
                    {
                        int totalImp = 0;
                        TotalImp.Add(totalImp);
                    }

                    try
                    {
                        int totalPages = Convert.ToInt32(totalPagesstr);
                        TotalPages.Add(totalPages);
                    }
                    catch
                    {
                        int totalPages = 0;
                        TotalPages.Add(totalPages);
                    }

                    //if (totalAccstr!=null|| totalAccstr != " ")
                    //{
                    //    int totalAcc = Convert.ToInt32(totalAccstr);
                    //    TotalAcc.Add(totalAcc);
                    //}
                    //else
                    //{
                    //    int totalAcc = 0;
                    //    TotalAcc.Add(totalAcc);
                    //}

                    //if (totalImpstr != null|| totalImpstr != " ")
                    //{
                    //    int totalImp = Convert.ToInt32(totalImpstr);
                    //    TotalImp.Add(totalImp);
                    //}
                    //else
                    //{
                    //    int totalImp = 0;
                    //    TotalImp.Add(totalImp);
                    //}

                    //if (totalPagesstr != null || totalPagesstr != " ")
                    //{
                    //    int totalPages = Convert.ToInt32(totalPagesstr);
                    //    TotalPages.Add(totalPages);
                    //}
                    //else
                    //{
                    //    int totalPages = 0;
                    //    TotalPages.Add(totalPages);
                    //}

                }

                ViewBag.TotalAcc = TotalAcc.Sum();
                ViewBag.TotalImp = TotalImp.Sum();
                ViewBag.TotalPages = TotalPages.Sum();

                rmTotalrecords.Close();
            }

            return View(dailytrackingreport);
        }

        [HttpPost]
        public async Task<ActionResult> DailyTrackingReport(DailyTracking get, FormCollection formcollection)
        {
            try
            {
                string LogTagNo = "";
                string CompanyForm = "";
                string CustomerForm = "";
                string JobClassForm = "";
                string StartDateForm = "";
                string EndDateForm = "";

                try
                {
                    LogTagNo = formcollection["LogTagNo"];
                }
                catch
                {
                    LogTagNo = "";
                }

                try
                {
                    CompanyForm = formcollection["Company"];
                }
                catch
                {
                    CompanyForm = "";
                }

                try
                {
                    CustomerForm = formcollection["Customer"];
                }
                catch
                {
                    CustomerForm = "";
                }

                try
                {
                    JobClassForm = formcollection["JobClass"];
                }
                catch
                {
                    JobClassForm = "";
                }

                try
                {
                    CustomerForm = formcollection["Customer"];
                }
                catch
                {
                    CustomerForm = "";
                }

                try
                {
                    StartDateForm = formcollection["StartDate"];
                }
                catch
                {
                    StartDateForm = "";
                }

                try
                {
                    EndDateForm = formcollection["EndDate"];
                }
                catch
                {
                    EndDateForm = "";
                }

                Debug.WriteLine("Start Method");
                Debug.WriteLine("JobClass :" + JobClassForm);
                Debug.WriteLine("Company :" + CompanyForm);
                Debug.WriteLine("LogTag :" + LogTagNo);
                Debug.WriteLine("Customer_Name :" + CustomerForm);
                Debug.WriteLine("StartDate :" + StartDateForm);
                Debug.WriteLine("EndDate :" + EndDateForm);

                List<string> customer = new List<string>();
                List<string> JobClass = new List<string>();
                List<string> Company = new List<string>();
                List<int> TotalAcc = new List<int>();
                List<int> TotalImp = new List<int>();
                List<int> TotalPages = new List<int>();

                string StartDate = StartDateForm.Replace("/", "-");
                string EndDate = EndDateForm.Replace("/", "-");

                Debug.WriteLine("Start Date : " + StartDate);
                Debug.WriteLine("End Date : " + EndDate);

                //command.Parameters.AddWithValue("@JobClass", JobClassForm);
                //command.Parameters.AddWithValue("@Company", CompanyForm);
                //command.Parameters.AddWithValue("@LogTagNo", LogTagNo);
                //command.Parameters.AddWithValue("@Customer_Name", CustomerForm);
                //command.Parameters.AddWithValue("@StartDate", StartDate);
                //command.Parameters.AddWithValue("@EndDate", EndDate);

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (SqlCommand command = new SqlCommand("", cn))
                    {
                        cn.Open();

                        //search by logtag only
                        if (LogTagNo != "" && CompanyForm == null && CustomerForm == null && JobClassForm == null)
                        {
                            Debug.WriteLine("Masuk Query logtag only");
                            command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking where LogTagNo=@LogTagNo ORDER BY CreatedOn ASC";

                        }
                        //search by company only
                        if (CompanyForm != null && LogTagNo == "" && CustomerForm == null && JobClassForm == null)
                        {
                            Debug.WriteLine("Masuk Query Company Only");
                            command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking where Company=@Company ORDER BY CreatedOn ASC";
                        }

                        //search by customer only
                        if (LogTagNo == "" && CompanyForm == null && CustomerForm != null && JobClassForm == null)
                        {
                            Debug.WriteLine("Masuk Query Company Only");
                            command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking where Customer_Name=@Customer_Name ORDER BY CreatedOn ASC";
                        }

                        //search by customer and job class
                        if (LogTagNo == "" && CompanyForm == null && CustomerForm != null && JobClassForm != null)
                        {
                            if (JobClassForm != "All")
                            {
                                command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking where JobClass=@JobClass AND Customer_Name=@Customer_Name ORDER BY CreatedOn ASC";
                            }
                            else
                            {
                                command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking where Customer_Name=@Customer_Name ORDER BY CreatedOn ASC";
                            }
                        }

                        //search by job class only
                        if (LogTagNo == "" && CompanyForm == null && CustomerForm == null && JobClassForm != null)
                        {
                            if (JobClassForm != "All")
                            {
                                Debug.WriteLine("Masuk Query job class only");

                                command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking where JobClass=@JobClass ORDER BY CreatedOn ASC";
                            }
                            else
                            {
                                Debug.WriteLine("Masuk Query job class only");

                                command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking ORDER BY CreatedOn ASC";
                            }
                        }

                        //search by lpgtag an company
                        if (LogTagNo != "" && CompanyForm != null && CustomerForm == null && JobClassForm == null)
                        {
                            Debug.WriteLine("Masuk Query lpgtag an company");
                            command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking where LogTagNo=@LogTagNo AND Company=@Company ORDER BY CreatedOn ASC";
                        }

                        //search by logtag and job class
                        if (LogTagNo != "" && CompanyForm == null && CustomerForm == null && JobClassForm != null)
                        {
                            if (JobClassForm != "All")
                            {
                                command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking where LogTagNo=@LogTagNo AND JobClass=@JobClass ORDER BY CreatedOn ASC";
                            }
                            else
                            {
                                command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking where LogTagNo=@LogTagNo ORDER BY CreatedOn ASC";
                            }
                        }

                        //search by logtag and customer
                        if (LogTagNo != "" && CompanyForm == null && CustomerForm != null && JobClassForm == null)
                        {
                            Debug.WriteLine("Masuk Query logtag and customer");

                            command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking where LogTagNo=@LogTagNo AND Customer_Name=@Customer_Name ORDER BY CreatedOn ASC";
                        }

                        //search by company and customer
                        if (LogTagNo == "" && CompanyForm != null && CustomerForm != null && JobClassForm == null)
                        {
                            Debug.WriteLine("Masuk Query company and customer");
                            command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking where Company=@Company AND Customer_Name=@Customer_Name ORDER BY CreatedOn ASC";

                        }

                        //search by company and jobclass
                        if (LogTagNo == "" && CompanyForm != null && CustomerForm == null && JobClassForm != null)
                        {
                            if (JobClassForm != "All")
                            {
                                Debug.WriteLine("Masuk Query company and jobclass");
                                command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking where Company=@Company AND JobClass=@JobClass ORDER BY CreatedOn ASC";
                            }
                            else
                            {
                                Debug.WriteLine("Masuk Query company and jobclass");
                                command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking where Company=@Company ORDER BY CreatedOn ASC";
                            }


                        }
                        //search all except logtag
                        if (LogTagNo == "" && CompanyForm != null && CustomerForm != null && JobClassForm != null)
                        {
                            command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking where JobClass=@JobClass AND Company=@Company AND Customer_Name=@Customer_Name AND CONVERT(VARCHAR, CreatedOn, 23)BETWEEN @StartDate AND @EndDate ORDER BY CreatedOn ASC";

                        }
                        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                        if (LogTagNo != "" && CompanyForm == null && CustomerForm == null && JobClassForm == null && StartDateForm != "" && EndDateForm != "")
                        {
                            Debug.WriteLine("Masuk Query logtag only");
                            command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking where LogTagNo=@LogTagNo1 AND CONVERT(VARCHAR, CreatedOn, 23) Between @StartDate AND @EndDate ORDER BY CreatedOn ASC";

                        }

                        //search by company only
                        if (CompanyForm != null && LogTagNo == "" && CustomerForm == null && JobClassForm == null && StartDateForm != null && EndDateForm != null)
                        {
                            Debug.WriteLine("Masuk Query Company Only");
                            command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking where Company=@Company1 AND CreatedOn Between @StartDate AND @EndDate ORDER BY CreatedOn ASC";
                        }

                        //search by customer only
                        if (LogTagNo == "" && CompanyForm == null && CustomerForm != null && JobClassForm == null && StartDateForm != null && EndDateForm != null)
                        {
                            Debug.WriteLine("Masuk Query Company Only");
                            command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking where Customer_Name=@Customer_Name1 AND CreatedOn Between @StartDate AND @EndDate ORDER BY CreatedOn ASC";

                        }

                        //search by customer and job class
                        if (LogTagNo == "" && CompanyForm == null && CustomerForm != null && JobClassForm != null && StartDateForm != null && EndDateForm != null)
                        {
                            if (JobClassForm != "All")
                            {
                                command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking where JobClass=@JobClass1 AND Customer_Name=@Customer_Name1 AND CreatedOn Between @StartDate AND @EndDate ORDER BY CreatedOn ASC";
                            }
                            else
                            {
                                command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking where Customer_Name=@Customer_Name1 AND CreatedOn Between @StartDate AND @EndDate ORDER BY CreatedOn ASC";
                            }
                        }

                        //search by job class only
                        if (LogTagNo == "" && CompanyForm == null && CustomerForm == null && JobClassForm != null && StartDateForm != null && EndDateForm != null)
                        {
                            if (JobClassForm != "All")
                            {
                                Debug.WriteLine("Masuk Query job class only");

                                command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking where JobClass=@JobClass1 AND CreatedOn Between @StartDate AND @EndDate ORDER BY CreatedOn ASC";
                            }
                            else
                            {
                                Debug.WriteLine("Masuk Query job class only");

                                command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking WHERE CreatedOn Between @StartDate AND @EndDate ORDER BY CreatedOn ASC";
                            }


                        }

                        //search by lpgtag an company
                        if (LogTagNo != "" && CompanyForm != null && CustomerForm == null && JobClassForm == null && StartDateForm != null && EndDateForm != null)
                        {
                            Debug.WriteLine("Masuk Query lpgtag an company");
                            command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking where LogTagNo=@LogTagNo1 AND Company=@Company1 AND CreatedOn Between @StartDate AND @EndDate ORDER BY CreatedOn ASC";

                        }

                        //search by logtag and job class
                        if (LogTagNo != "" && CompanyForm == null && CustomerForm == null && JobClassForm != null && StartDateForm != null && EndDateForm != null)
                        {
                            if (JobClassForm != "All")
                            {
                                command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking where LogTagNo=@LogTagNo1 AND JobClass=@JobClass1 AND CreatedOn Between @StartDate AND @EndDate ORDER BY CreatedOn ASC";
                            }
                            else
                            {
                                command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking where LogTagNo=@LogTagNo1 AND CreatedOn Between @StartDate AND @EndDate ORDER BY CreatedOn ASC";
                            }
                        }

                        //search by logtag and customer
                        if (LogTagNo != "" && CompanyForm == null && CustomerForm != null && JobClassForm == null && StartDateForm != null && EndDateForm != null)
                        {
                            Debug.WriteLine("Masuk Query logtag and customer");

                            command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking where LogTagNo=@LogTagNo AND Customer_Name=@Customer_Name AND CreatedOn Between @StartDate AND @EndDate ORDER BY CreatedOn ASC";

                        }

                        //search by company and customer
                        if (LogTagNo == "" && CompanyForm != null && CustomerForm != null && JobClassForm == null && StartDateForm != null && EndDateForm != null)
                        {
                            Debug.WriteLine("Masuk Query company and customer");
                            command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking where Company=@Company AND Customer_Name=@Customer_Name AND CreatedOn Between @StartDate AND @EndDate ORDER BY CreatedOn ASC";
                        }

                        //search by company and jobclass
                        if (LogTagNo == "" && CompanyForm != null && CustomerForm == null && JobClassForm != null && StartDateForm != null && EndDateForm != null)
                        {
                            if (JobClassForm != "All")
                            {
                                Debug.WriteLine("Masuk Query company and jobclass");
                                command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking,Id where Company=@Company1 AND JobClass=@JobClass1 AND CreatedOn Between @StartDate AND @EndDate ORDER BY CreatedOn ASC";
                            }
                            else
                            {
                                Debug.WriteLine("Masuk Query company and jobclass");
                                command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking,Id where Company=@Company AND CreatedOn Between @StartDate AND @EndDate ORDER BY CreatedOn ASC";
                            }


                        }
                        //search all except logtag
                        if (LogTagNo == "" && CompanyForm != null && CustomerForm != null && JobClassForm != null && StartDateForm != null && EndDateForm != null)
                        {
                            command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking where JobClass=@JobClass AND Company=@Company AND Customer_Name=@Customer_Name AND CONVERT(VARCHAR, CreatedOn, 23)BETWEEN @StartDate AND @EndDate ORDER BY CreatedOn ASC";
                        }

                        if (LogTagNo == "" && CompanyForm == null && CustomerForm == null && JobClassForm == null && StartDateForm != null && EndDateForm != null)
                        {
                            Debug.WriteLine("Masuk Query company and jobclass");
                            command.CommandText = @"select LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id,AccountsQty, ImpressionQty, PagesQty from DailyTracking where CONVERT(VARCHAR, CreatedOn, 23) Between @StartDate AND @EndDate ORDER BY CreatedOn ASC";

                        }

                        if(!string.IsNullOrEmpty(LogTagNo))
                        {
                            command.Parameters.AddWithValue("@LogTagNo", LogTagNo);
                            command.Parameters.AddWithValue("@LogTagNo1", LogTagNo);
                        }

                        if (!string.IsNullOrEmpty(JobClassForm))
                        {
                            command.Parameters.AddWithValue("@JobClass", JobClassForm);
                        }

                        if (!string.IsNullOrEmpty(CompanyForm))
                        {
                            command.Parameters.AddWithValue("@Company", CompanyForm);
                        }

                        if (!string.IsNullOrEmpty(CustomerForm))
                        {
                            command.Parameters.AddWithValue("@Customer_Name", CustomerForm);
                        }

                        if (!string.IsNullOrEmpty(StartDate))
                        {
                            command.Parameters.AddWithValue("@StartDate", StartDate);
                        }

                        if (!string.IsNullOrEmpty(EndDate))
                        {
                            command.Parameters.AddWithValue("@EndDate", EndDate);
                        }



                        //Debug.WriteLine("JobClass :" + JobClassForm);
                        //Debug.WriteLine("Company :" + CompanyForm);
                        //Debug.WriteLine("Customer_Name :" + CustomerForm);
                        //Debug.WriteLine("StartDate :" + StartDate);
                        //Debug.WriteLine("EndDate :" + EndDate);

                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            DailyTracking model = new DailyTracking();
                            {
                                model.CreatedOn = reader.GetString(0);
                                model.JobClass = reader["JobClass"].ToString();
                                model.Customer_Name = reader["Customer_Name"].ToString();
                                model.ProductName = reader["ProductName"].ToString();
                                model.LogTagNo = reader["LogTagNo"].ToString();
                                model.Id = reader.GetGuid(5);

                            };

                            dailytrackingreport.Add(model);

                            string totalAccstr = reader["AccountsQty"].ToString();
                            string totalImpstr = reader["ImpressionQty"].ToString();
                            string totalPagesstr = reader["PagesQty"].ToString();

                            try
                            {
                                int totalAcc = Convert.ToInt32(totalAccstr);
                                TotalAcc.Add(totalAcc);
                            }
                            catch
                            {
                                int totalAcc = 0;
                                TotalAcc.Add(totalAcc);
                            }

                            try
                            {
                                int totalImp = Convert.ToInt32(totalImpstr);
                                TotalImp.Add(totalImp);
                            }
                            catch
                            {
                                int totalImp = 0;
                                TotalImp.Add(totalImp);
                            }

                            try
                            {
                                int totalPages = Convert.ToInt32(totalPagesstr);
                                TotalPages.Add(totalPages);
                            }
                            catch
                            {
                                int totalPages = 0;
                                TotalPages.Add(totalPages);
                            }



                        }

                        ViewBag.TotalAcc = TotalAcc.Sum();
                        ViewBag.TotalImp = TotalImp.Sum();
                        ViewBag.TotalPages = TotalPages.Sum();

                        reader.Close();



                    }

                    Debug.WriteLine("Execute Customer Post");
                    SqlCommand getcustomer = new SqlCommand("SELECT DISTINCT Customer_Name FROM DailyTracking EXCEPT select Customer_Name from DailyTracking WHERE Customer_Name IS NULL", cn);
                    SqlDataReader rm = getcustomer.ExecuteReader();

                    while (rm.Read())
                    {
                        customer.Add(rm["Customer_Name"].ToString());
                    }

                    ViewBag.customer = customer;

                    rm.Close();

                    Debug.WriteLine("Execute JobClass Post");

                    SqlCommand getJobClass = new SqlCommand(" SELECT DISTINCT JobClass FROM DailyTracking EXCEPT select JobClass from DailyTracking WHERE JobClass IS NULL", cn);
                    SqlDataReader rmjobclass = getJobClass.ExecuteReader();

                    while (rmjobclass.Read())
                    {
                        JobClass.Add(rmjobclass["JobClass"].ToString());
                    }

                    ViewBag.JobClass = JobClass;

                    rmjobclass.Close();

                    Debug.WriteLine("Execute Company Post");

                    SqlCommand getcompany = new SqlCommand("SELECT DISTINCT Company FROM DailyTracking EXCEPT select Company from DailyTracking WHERE Company IS NULL", cn);
                    SqlDataReader rmcompany = getcompany.ExecuteReader();

                    while (rmcompany.Read())
                    {
                        Company.Add(rmcompany["Company"].ToString());
                    }

                    Company.Add("None");
                    ViewBag.Company = Company;

                    rmcompany.Close();

                    SqlCommand getTotal = new SqlCommand("SELECT count(Id) as totalid FROM DailyTracking", cn);
                    SqlDataReader rmTotal = getTotal.ExecuteReader();

                    while (rmTotal.Read())
                    {
                        string totalstr = rmTotal["totalid"].ToString();

                        ViewBag.Total = totalstr;
                    }


                    rmTotal.Close();

                    //SqlCommand getTotalrecords = new SqlCommand(" SELECT AccountsQty, ImpressionQty, PagesQty from DailyTracking", cn);
                    //SqlDataReader rmTotalrecords = getTotalrecords.ExecuteReader();

                    //while (rmTotalrecords.Read())
                    //{
                    //    string totalAccstr = rmTotalrecords["AccountsQty"].ToString();
                    //    string totalImpstr = rmTotalrecords["ImpressionQty"].ToString();
                    //    string totalPagesstr = rmTotalrecords["PagesQty"].ToString();

                    //    try
                    //    {
                    //        int totalAcc = Convert.ToInt32(totalAccstr);
                    //        TotalAcc.Add(totalAcc);
                    //    }
                    //    catch
                    //    {
                    //        int totalAcc = 0;
                    //        TotalAcc.Add(totalAcc);
                    //    }

                    //    try
                    //    {
                    //        int totalImp = Convert.ToInt32(totalImpstr);
                    //        TotalImp.Add(totalImp);
                    //    }
                    //    catch
                    //    {
                    //        int totalImp = 0;
                    //        TotalImp.Add(totalImp);
                    //    }

                    //    try
                    //    {
                    //        int totalPages = Convert.ToInt32(totalPagesstr);
                    //        TotalPages.Add(totalPages);
                    //    }
                    //    catch
                    //    {
                    //        int totalPages = 0;
                    //        TotalPages.Add(totalPages);
                    //    }

                    //    //if (totalAccstr!=null|| totalAccstr != " ")
                    //    //{
                    //    //    int totalAcc = Convert.ToInt32(totalAccstr);
                    //    //    TotalAcc.Add(totalAcc);
                    //    //}
                    //    //else
                    //    //{
                    //    //    int totalAcc = 0;
                    //    //    TotalAcc.Add(totalAcc);
                    //    //}

                    //    //if (totalImpstr != null|| totalImpstr != " ")
                    //    //{
                    //    //    int totalImp = Convert.ToInt32(totalImpstr);
                    //    //    TotalImp.Add(totalImp);
                    //    //}
                    //    //else
                    //    //{
                    //    //    int totalImp = 0;
                    //    //    TotalImp.Add(totalImp);
                    //    //}

                    //    //if (totalPagesstr != null || totalPagesstr != " ")
                    //    //{
                    //    //    int totalPages = Convert.ToInt32(totalPagesstr);
                    //    //    TotalPages.Add(totalPages);
                    //    //}
                    //    //else
                    //    //{
                    //    //    int totalPages = 0;
                    //    //    TotalPages.Add(totalPages);
                    //    //}

                    //}

                    //ViewBag.TotalAcc = TotalAcc.Sum();
                    //ViewBag.TotalImp = TotalImp.Sum();
                    //ViewBag.TotalPages = TotalPages.Sum();

                    //rmTotalrecords.Close();
                }

                return View(dailytrackingreport);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error :" + ex);
                //ViewBag.Error = "<script>alert('ERROR BITCH')</script>";
                return View(dailytrackingreport);
            }



        }

        List<DailyTracking> exportdailytracking = new List<DailyTracking>();

        public ActionResult ExportExcelDailyTrackingReport(string ids)
        {
            // Split the string 'ids' into individual IDs
            Debug.WriteLine("List :" + ids);
            var idList = ids.Split('|').ToList();

            // Process the list of IDs here
            // You can access the IDs in the 'idList' variable
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");


            //foreach (var a in idList)
            //{
            //    Debug.WriteLine("ID :" + a);
            //}

            foreach (var id in idList)
            {
                // Process each ID as needed
                //Debug.WriteLine("ID :" + id);
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn.Open();

                    SqlCommand command = new SqlCommand("SELECT FORMAT(MAX(A.CreatedOn), 'dd-MM-yyyy') as CreatedOn ,MAX(A.JobClass),MAX(A.Customer_Name),MAX(A.ProductName),A.LogTagNo,SUM(CAST(B.AccQty AS INT)) AS AccQty ,SUM(CAST(B.PageQty AS INT)) AS PageQty,SUM(CAST(B.ImpQty AS INT)) AS ImpQty,LEFT( CONVERT(varchar, MAX(A.StartDateOn), 120), 10)," +
                        "MAX(A.StartTime),LEFT( CONVERT(varchar, MAX(A.EndDateOn), 120), 10),MAX(A.EndTime),MAX(A.PIC),LEFT( CONVERT(varchar, MAX(A.ProcessStartDateOn), 120), 10),MAX(A.ProcessStartTime),LEFT( CONVERT(varchar, MAX(A.ProcessEndDateOn), 120), 10),MAX(A.ProcessEndTime),MAX(A.TimeTaken),MAX(A.DatePostOn),MAX(A.DatePostTime),LEFT( CONVERT(varchar, MAX(A.DateApproveOn), 120), 10),MAX(A.DateApproveTime)," +
                        "MAX(A.ItSubmitOn) as ItSubmitOn,MAX(A.DateDeletionOn),MAX(A.PaperType),MAX(B.JobType),MAX(A.PaperType),MAX(B.JobClass),MAX(B.CreatedOn) as CreatedOn2 FROM DailyTracking as A LEFT JOIN JobAuditTrailDetail AS B ON A.LogTagNo=B.LogTagNo WHERE A.Id=@Id GROUP BY A.LogTagNo ORDER BY MAX(A.CreatedOn) ASC", cn);
                    command.Parameters.AddWithValue("@Id", id);

                    SqlDataReader rm = command.ExecuteReader();

                    while (rm.Read())
                    {
                        Debug.WriteLine("LogTagNo : " + rm.GetString(4));
                        List<string> PostingInformation = getPostingInfo(rm.GetString(4));

                        var model = new DailyTracking();
                        {
                            //ada 24 
                            if (rm.IsDBNull(0) == false)
                            {
                                model.CreatedOn = rm["CreatedOn"].ToString();
                            }

                            if (rm.IsDBNull(1) == false)
                            {
                                //int position = rm.GetString(1).IndexOf('-');
                                //string JobClass = (rm.GetString(1)).Substring(0, position - 1);
                                //model.JobClass = JobClass;

                                if (rm.GetString(1).Contains('-'))
                                {
                                    int position = rm.GetString(1).IndexOf('-');
                                    string JobClass = rm.GetString(1).Substring(0, position);
                                    Debug.WriteLine("JobClass Modified: " + JobClass);
                                    model.JobClass = JobClass;
                                }
                                else
                                {
                                    model.JobClass = rm.GetString(1);
                                }

                                //model.JobClass = rm.GetString(1);

                            }

                            if (rm.IsDBNull(2) == false)
                            {
                                model.Customer_Name = rm.GetString(2);
                            }

                            if (rm.IsDBNull(3) == false)
                            {
                                model.ProductName = rm.GetString(3);
                            }

                            if (rm.IsDBNull(4) == false)
                            {
                                model.LogTagNo = rm.GetString(4);
                            }

                            if (rm.IsDBNull(5) == false)
                            {
                                model.AccountsQty = rm["AccQty"].ToString();
                            }

                            if (rm.IsDBNull(6) == false)
                            {
                                model.PagesQty = rm["PageQty"].ToString();
                            }

                            if (rm.IsDBNull(7) == false)
                            {
                                model.ImpressionQty = rm["ImpQty"].ToString();
                            }

                            if (rm.IsDBNull(8) == false)
                            {
                                model.StartDateOn = rm.GetString(8);
                            }

                            if (rm.IsDBNull(9) == false)
                            {
                                model.StartTime = rm.GetString(9);
                            }
                            else
                            {
                                model.StartTime = "-";

                            }

                            if (rm.IsDBNull(10) == false)
                            {
                                model.EndDateOn = rm.GetString(10);
                            }

                            if (rm.IsDBNull(11) == false)
                            {
                                model.EndTime = rm.GetString(11);
                            }
                            else
                            {
                                model.EndTime = "-";

                            }

                            if (rm.IsDBNull(12) == false)
                            {
                                model.PIC = rm.GetString(12);
                            }

                            if (rm.IsDBNull(13) == false)
                            {
                                model.ProcessStartDateOn = rm.GetString(13);
                            }

                            if (rm.IsDBNull(14) == false)
                            {
                                model.ProcessStartTime = rm.GetString(14);
                            }
                            else
                            {
                                model.ProcessStartTime = "-";

                            }

                            if (rm.IsDBNull(15) == false)
                            {
                                model.ProcessEndDateOnTxt = rm.GetString(15);
                            }

                            if (rm.IsDBNull(16) == false)
                            {
                                model.ProcessEndTime = rm.GetString(16);
                            }
                            else
                            {
                                model.ProcessEndTime = "-";

                            }

                            if (rm.IsDBNull(17) == false)
                            {
                                model.TimeTaken = rm.GetString(17);
                            }

                            if (rm.IsDBNull(18) == false)
                            {
                                model.DatePostOnTxt = rm.GetString(18);
                            }

                            if (rm.IsDBNull(19) == false)
                            {
                                model.DatePostTime = rm.GetString(19);
                            }
                            else
                            {
                                model.DatePostTime = "-";
                            }

                            if (rm.IsDBNull(20) == false)
                            {
                                model.DateApproveOn = rm.GetString(20);
                            }
                            else
                            {

                                model.DateApproveOn = "-";

                            }

                            if (rm.IsDBNull(21) == false)
                            {
                                model.DateApproveTime = rm.GetString(21);
                            }
                            else
                            {

                                model.DateApproveTime = "-";

                            }

                            if (rm.IsDBNull(22) == false)
                            {
                                model.ItSubmitOnTxt = rm["ITSubmitOn"].ToString();
                            }

                            if (rm.IsDBNull(23) == false)
                            {
                                model.DateDeletionOn = rm.GetDateTime(23);
                            }

                            if (rm.IsDBNull(24) == false)
                            {
                                model.PaperType = rm.GetString(24);
                            }
                            else
                            {
                                model.PaperType = "-";
                            }

                            if (rm.IsDBNull(25) == false)
                            {
                                model.JobType = rm.GetString(25);
                            }
                            else
                            {
                                model.JobType = "-";
                            }

                            //if (rm.IsDBNull(27) == false)
                            //{
                            //    model.JobClass = rm.GetString(27);
                            //}
                            if (rm.IsDBNull(28) == false)
                            {
                                // use CreateByIT to capture timestamp on CreatedOn column to save time
                                var TimeCreated = rm["CreatedOn2"].ToString().Split(' ');
                                var TimeCreated2 = TimeCreated[1].Substring(0, 5).Replace(":","");
                                model.CreateByIT = TimeCreated2;

                            }

                            model.DatePostOn = getDatePost(rm.GetString(4));
                            model.DatePostTime = getTimePost(rm.GetString(4));
                            model.DateDeletionOnTxt = getDeletionDate(rm.GetString(4));
                            model.DatePostOnTxt = PostingInformation[0];
                            model.DatePostTime = PostingInformation[1];

                            if (PostingInformation[0] != "-")
                            {
                                DateTime date = DateTime.Parse(PostingInformation[0]);

                                DateTime newDate = date.AddDays(-2);

                                model.DateDeletionOnTxt = newDate.ToString();
                            }
                            else
                            {
                                model.DateDeletionOnTxt = "-";
                            }
                            //model.ServiceCharges = ServiceCharges.Sum();
                            //model.DatePostOn = PostingInfo[0];
                            //model.DatePostTime = PostingInfo[1];

                        };

                        exportdailytracking.Add(model);
                    }


                    //try
                    //{
                    //    SqlCommand command = new SqlCommand("SELECT LEFT( CONVERT(varchar, A.CreatedOn, 120), 10),A.JobClass,A.Customer_Name,A.ProductName,A.LogTagNo,A.AccountsQty,A.PagesQty,A.ImpressionQty,LEFT( CONVERT(varchar, A.StartDateOn, 120), 10)," +
                    //    "StartTime,LEFT( CONVERT(varchar, A.EndDateOn, 120), 10),A.EndTime,PIC,LEFT( CONVERT(varchar, A.ProcessStartDateOn, 120), 10),A.ProcessStartTime,A.TimeTaken,A.DatePostOn,A.DatePostTime,LEFT( CONVERT(varchar, A.DateApproveOn, 120), 10),A.DateApproveTime," +
                    //    "A.ItSubmitOn,A.DateDeletionOn,A.PaperType,A.JobType,B.PaperType,B.JobClass FROM DailyTracking as A INNER JOIN JobInstruction AS B ON A.JobSheetNo=B.JobSheetNo WHERE A.Id=@Id", cn);
                    //    command.Parameters.AddWithValue("@Id", id);

                    //    SqlDataReader rm = command.ExecuteReader();

                    //    while (rm.Read())
                    //    {
                    //        List<string> PostingInformation = getPostingInfo(rm.GetString(4));

                    //        foreach (var x in PostingInformation)
                    //        {
                    //            Debug.WriteLine("Content : " + x);
                    //            Debug.WriteLine("================================");
                    //        }

                    //        var model = new DailyTracking();
                    //        {
                    //            //ada 24 
                    //            if (rm.IsDBNull(0) == false)
                    //            {
                    //                model.CreatedOn = rm.GetString(0);
                    //            }

                    //            if (rm.IsDBNull(1) == false)
                    //            {
                    //                model.JobClass = rm.GetString(1);
                    //            }

                    //            if (rm.IsDBNull(2) == false)
                    //            {
                    //                model.Customer_Name = rm.GetString(2);
                    //            }

                    //            if (rm.IsDBNull(3) == false)
                    //            {
                    //                model.ProductName = rm.GetString(3);
                    //            }

                    //            if (rm.IsDBNull(4) == false)
                    //            {
                    //                model.LogTagNo = rm.GetString(4);
                    //            }

                    //            if (rm.IsDBNull(5) == false)
                    //            {
                    //                model.AccountsQty = rm.GetString(5);
                    //            }

                    //            if (rm.IsDBNull(6) == false)
                    //            {
                    //                model.PagesQty = rm.GetString(6);
                    //            }

                    //            if (rm.IsDBNull(7) == false)
                    //            {
                    //                model.ImpressionQty = rm.GetString(7);
                    //            }

                    //            if (rm.IsDBNull(8) == false)
                    //            {
                    //                model.StartDateOn = rm.GetString(8);
                    //            }

                    //            if (rm.IsDBNull(9) == false)
                    //            {
                    //                model.StartTime = rm.GetString(9);
                    //            }

                    //            if (rm.IsDBNull(10) == false)
                    //            {
                    //                model.EndDateOn = rm.GetString(10);
                    //            }

                    //            if (rm.IsDBNull(11) == false)
                    //            {
                    //                model.EndTime = rm.GetString(11);
                    //            }

                    //            if (rm.IsDBNull(12) == false)
                    //            {
                    //                model.PIC = rm.GetString(12);
                    //            }

                    //            if (rm.IsDBNull(13) == false)
                    //            {
                    //                model.ProcessStartDateOn = rm.GetString(13);
                    //            }

                    //            if (rm.IsDBNull(14) == false)
                    //            {
                    //                model.ProcessStartTime = rm.GetString(14);
                    //            }

                    //            if (rm.IsDBNull(15) == false)
                    //            {
                    //                model.TimeTaken = rm.GetString(15);
                    //            }

                    //            //if (rm.IsDBNull(16) == false)
                    //            //{
                    //            //    model.DatePostOn = rm.GetDateTime(16);
                    //            //}

                    //            if (rm.IsDBNull(17) == false)
                    //            {
                    //                model.DatePostTime = rm.GetString(17);
                    //            }

                    //            if (rm.IsDBNull(18) == false)
                    //            {
                    //                model.DateApproveOn = rm.GetString(18);
                    //            }

                    //            if (rm.IsDBNull(19) == false)
                    //            {
                    //                model.DateApproveTime = rm.GetString(19);
                    //            }

                    //            if (rm.IsDBNull(20) == false)
                    //            {
                    //                model.ItSubmitOn = rm.GetDateTime(20);
                    //            }

                    //            if (rm.IsDBNull(21) == false)
                    //            {
                    //                model.DateDeletionOn = rm.GetDateTime(21);
                    //            }

                    //            if (rm.IsDBNull(22) == false)
                    //            {
                    //                model.PaperType = rm.GetString(22);
                    //            }
                    //            else
                    //            {
                    //                model.PaperType = "-";
                    //            }

                    //            if (rm.IsDBNull(23) == false)
                    //            {
                    //                model.JobType = rm.GetString(23);
                    //            }
                    //            else
                    //            {
                    //                model.JobType = "-";
                    //            }

                    //            if (rm.IsDBNull(24) == false)
                    //            {
                    //                model.JobClass = rm.GetString(24);
                    //            }

                    //            model.DatePostOn = getDatePost(rm.GetString(4));
                    //            model.DatePostTime = getTimePost(rm.GetString(4));
                    //            model.DateDeletionOnTxt = getDeletionDate(rm.GetString(4));
                    //            model.DatePostOnTxt = PostingInformation[0];
                    //            model.DatePostTime = PostingInformation[1];

                    //            if (PostingInformation[0] !="-") 
                    //            {
                    //                DateTime date = DateTime.Parse(PostingInformation[0]);

                    //                DateTime newDate = date.AddDays(-2);

                    //                model.DateDeletionOnTxt = newDate.ToString();
                    //            }
                    //            else
                    //            {
                    //                model.DateDeletionOnTxt = "-";
                    //            }
                    //            //model.ServiceCharges = ServiceCharges.Sum();
                    //            //model.DatePostOn = PostingInfo[0];
                    //            //model.DatePostTime = PostingInfo[1];

                    //        };

                    //        exportdailytracking.Add(model);
                    //    }

                    //    //string excelName = " REPORT-" + DateStartTxt + "-" + DateEndTxt;

                    //    //using (var memoryStream = new MemoryStream())
                    //    //{
                    //    //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //    //    Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                    //    //    excel.SaveAs(memoryStream);
                    //    //    memoryStream.WriteTo(Response.OutputStream);
                    //    //    Response.Flush();
                    //    //    Response.End();
                    //    //}
                    //}
                    //catch(Exception ex)
                    //{
                    //    Debug.WriteLine("Error : " + ex);
                    //    TempData["Error"] = "<script>alert('No Data Available');</script>";
                    //    return RedirectToAction("DailyTrackingReport", "Report");
                    //}


                }


                string excelName = "EXCEL SAMPLE";

                workSheet.TabColor = System.Drawing.Color.Black;

                workSheet.DefaultRowHeight = 12;
                workSheet.Row(1).Height = 20;
                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(1).Style.Font.Bold = true;

                workSheet.Cells[1, 1].Value = "IT Submit Date";
                workSheet.Cells[1, 2].Value = "IT Submit Time";
                workSheet.Cells[1, 3].Value = "Job Class";
                workSheet.Cells[1, 4].Value = "Customer Name";
                workSheet.Cells[1, 5].Value = "Product Name";
                workSheet.Cells[1, 6].Value = "Log Tag No";
                workSheet.Cells[1, 7].Value = "Accounts Qty";
                workSheet.Cells[1, 8].Value = "Pages Qty";
                workSheet.Cells[1, 9].Value = "Impression Qty";
                workSheet.Cells[1, 10].Value = "Start In Date";
                workSheet.Cells[1, 11].Value = "Start In Time";
                workSheet.Cells[1, 12].Value = "End In Date";
                workSheet.Cells[1, 13].Value = "End In Time";
                workSheet.Cells[1, 14].Value = "PIC";
                workSheet.Cells[1, 15].Value = "Process Start Date";
                workSheet.Cells[1, 16].Value = "Process Start Time";
                workSheet.Cells[1, 17].Value = "Process End Date";
                workSheet.Cells[1, 18].Value = "Process End Time";
                workSheet.Cells[1, 19].Value = "Time Taken";
                workSheet.Cells[1, 20].Value = "Date Post";
                workSheet.Cells[1, 21].Value = "Post Time";
                workSheet.Cells[1, 22].Value = "Date Approve";
                workSheet.Cells[1, 23].Value = "Approve Time";
                workSheet.Cells[1, 24].Value = "Date Deletion";
                workSheet.Cells[1, 25].Value = "Paper Type";
                workSheet.Cells[1, 26].Value = "Job Type";

                int recordIndex = 2;

                foreach (var x in exportdailytracking)
                {
                    //Debug.WriteLine("Created On: " + x.CreatedOn);
                    //Debug.WriteLine("Job Class: " + x.JobClass);
                    //Debug.WriteLine("Customer Name: " + x.Customer_Name);
                    //Debug.WriteLine("Product Name: " + x.ProductName);
                    //Debug.WriteLine("Log Tag No: " + x.LogTagNo);
                    //Debug.WriteLine("Accounts Qty: " + x.AccountsQty);
                    //Debug.WriteLine("Pages Qty: " + x.PagesQty);
                    //Debug.WriteLine("Impression Qty: " + x.ImpressionQty);
                    //Debug.WriteLine("Start Date On: " + x.StartDateOn);
                    //Debug.WriteLine("Start Time: " + x.StartTime);
                    //Debug.WriteLine("End Date On: " + x.EndDateOn);
                    //Debug.WriteLine("End Time: " + x.EndTime);
                    //Debug.WriteLine("PIC: " + x.PIC);
                    //Debug.WriteLine("Process Start Date On: " + x.ProcessStartDateOn);
                    //Debug.WriteLine("Process Start Time: " + x.ProcessStartTime);
                    //Debug.WriteLine("Time Taken: " + x.TimeTaken);
                    //Debug.WriteLine("Date Post On: " + x.DatePostOn);
                    //Debug.WriteLine("Date Post Time: " + x.DatePostTime);
                    //Debug.WriteLine("Date Approve On: " + x.DateApproveOn);
                    //Debug.WriteLine("Date Approve Time: " + x.DateApproveTime);
                    //Debug.WriteLine("IT Submit On: " + x.ItSubmitOn);
                    //Debug.WriteLine("Date Deletion On: " + x.DateDeletionOn);
                    //Debug.WriteLine("Paper Type: " + x.PaperType);
                    //Debug.WriteLine("Job Type: " + x.JobType);
                    //Debug.WriteLine("=======================================================================================");

                    workSheet.Cells[recordIndex, 1].Value = x.CreatedOn;
                    workSheet.Cells[recordIndex, 2].Value = x.CreateByIT;
                    workSheet.Cells[recordIndex, 3].Value = x.JobClass;
                    workSheet.Cells[recordIndex, 4].Value = x.Customer_Name;
                    workSheet.Cells[recordIndex, 5].Value = x.ProductName;
                    workSheet.Cells[recordIndex, 6].Value = x.LogTagNo;
                    workSheet.Cells[recordIndex, 7].Value = x.AccountsQty;
                    workSheet.Cells[recordIndex, 8].Value = x.PagesQty;
                    workSheet.Cells[recordIndex, 9].Value = x.ImpressionQty;
                    workSheet.Cells[recordIndex, 10].Value = x.StartDateOn;
                    if(x.StartTime!="-")
                    {
                        workSheet.Cells[recordIndex, 11].Value = (x.StartTime).Replace(":", "");
                    }
                    else
                    {
                        workSheet.Cells[recordIndex, 11].Value = x.StartTime;

                    }
                    workSheet.Cells[recordIndex, 12].Value = x.EndDateOn;

                    if (x.EndTime != "-")
                    {
                       workSheet.Cells[recordIndex, 13].Value = (x.EndTime).Replace(":", "");

                    }
                    else
                    {
                        workSheet.Cells[recordIndex, 13].Value = x.EndTime;

                    }
                    workSheet.Cells[recordIndex, 14].Value = x.PIC;
                    workSheet.Cells[recordIndex, 15].Value = x.ProcessStartDateOn;

                    if (x.ProcessStartTime != "-")
                    {
                        workSheet.Cells[recordIndex, 16].Value = (x.ProcessStartTime).Replace(":", "");

                    }
                    else
                    {
                        workSheet.Cells[recordIndex, 16].Value = x.ProcessStartTime;

                    }
                    workSheet.Cells[recordIndex, 17].Value = x.ProcessEndDateOnTxt;

                    if (x.ProcessEndTime != "-")
                    {
                        workSheet.Cells[recordIndex, 18].Value = (x.ProcessEndTime).Replace(":", "");

                    }
                    else
                    {
                        workSheet.Cells[recordIndex, 18].Value = x.ProcessEndTime;

                    }
                    workSheet.Cells[recordIndex, 19].Value = x.TimeTaken;
                    workSheet.Cells[recordIndex, 20].Value = x.DatePostOnTxt;

                    if (x.DatePostTime != "-")
                    {
                        workSheet.Cells[recordIndex, 21].Value = (x.DatePostTime).Replace(":", "");

                    }
                    else
                    {
                        workSheet.Cells[recordIndex, 21].Value = x.DatePostTime;

                    }
                    workSheet.Cells[recordIndex, 22].Value = x.DateApproveOn;


                    if (x.DateApproveTime != "-")
                    {
                        workSheet.Cells[recordIndex, 23].Value = (x.DateApproveTime).Replace(":", "");

                    }
                    else
                    {
                        workSheet.Cells[recordIndex, 23].Value = x.DateApproveTime;

                    }
                    workSheet.Cells[recordIndex, 24].Value = x.DateDeletionOnTxt;
                    workSheet.Cells[recordIndex, 25].Value = x.PaperType;
                    workSheet.Cells[recordIndex, 26].Value = x.JobType;
                    recordIndex++;
                }

                //foreach (var CLM in exportdailytracking)
                //{
                //    workSheet.Cells[recordIndex, 1].Value = CLM.Customer_Name;
                //    workSheet.Cells[recordIndex, 2].Value = CLM.ProductName;
                //    workSheet.Cells[recordIndex, 3].Value = CLM.LogTagNo;
                //    workSheet.Cells[recordIndex, 4].Value = CLM.PageQty;
                //    workSheet.Cells[recordIndex, 5].Value = CLM.ImpQty;
                //    workSheet.Cells[recordIndex, 6].Value = CLM.AccQty;
                //    workSheet.Cells[recordIndex, 7].Value = CLM.RevStrtDateOnTxt;
                //    workSheet.Cells[recordIndex, 8].Value = CLM.RevStrtTime;
                //    workSheet.Cells[recordIndex, 9].Value = CLM.StartDateOnTxt;
                //    workSheet.Cells[recordIndex, 10].Value = CLM.PostingDateOnTxt;
                //    workSheet.Cells[recordIndex, 11].Value = CLM.TotalAmountPaper;
                //    workSheet.Cells[recordIndex, 12].Value = CLM.TotalAmountEnv;
                //    workSheet.Cells[recordIndex, 13].Value = CLM.TotalAmountService;
                //    workSheet.Cells[recordIndex, 14].Value = CLM.TotalAmountPostage;
                //    workSheet.Cells[recordIndex, 15].Value = CLM.TotalAmountO;
                //    workSheet.Cells[recordIndex, 16].Value = CLM.TotalAmountF;
                //    workSheet.Cells[recordIndex, 17].Value = CLM.TotalAmountO2;
                //    workSheet.Cells[recordIndex, 18].Value = CLM.Sst;
                //    workSheet.Cells[recordIndex, 19].Value = CLM.TotalAmountO3;
                //    workSheet.Cells[recordIndex, 20].Value = CLM.TotalAmountx;


                //    recordIndex++;
                //}


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
            workSheet.Column(23).AutoFit();
            workSheet.Column(24).AutoFit();
            workSheet.Column(25).AutoFit();
            workSheet.Column(26).AutoFit();
            workSheet.Column(27).AutoFit();


            var memoryStream = new MemoryStream();
            excel.SaveAs(memoryStream);

            memoryStream.Position = 0;

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string filename = "DailyTracking.xlsx";
            //return File(filePath, contentType, downloadName);
            return File(memoryStream, contentType, filename);

        }



        public List<string> getPostingInfo(string LogTagNo)
        {
            List<string> PostingInfo = new List<string>();

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();

                SqlCommand cmd1 = new SqlCommand("SELECT PostingDateOn, PostingTime FROM PostingManifest WHERE LogTagNo = @LogTagNo1", cn);
                cmd1.Parameters.AddWithValue("@LogTagNo1", LogTagNo);
                SqlDataReader rm1 = cmd1.ExecuteReader();

                if (rm1.HasRows)
                {
                    while (rm1.Read())
                    {
                        if (!rm1.IsDBNull(0))
                        {
                            PostingInfo.Add(rm1["PostingDateOn"].ToString());
                        }
                        else
                        {
                            PostingInfo.Add("-");
                        }

                        if (!rm1.IsDBNull(1))
                        {
                            PostingInfo.Add(rm1.GetString(1));
                        }
                        else
                        {
                            PostingInfo.Add("-");
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 2; i++)
                    {
                        PostingInfo.Add("-");
                    }
                }


                //foreach (var x in PostingInfo)
                //{
                //    Debug.WriteLine("Content : " + x);
                //    Debug.WriteLine("================================");
                //}

                cn.Close();
            }

            return PostingInfo;
        }
        public List<double> getServiceCharges(string LogTagNo)
        {
            List<int> Vol = new List<int>();
            List<double> Charges = new List<double>();
            List<string> Rate = new List<string>();
            List<string> Desc = new List<string>();
            List<string> Process = new List<string>();
            List<double> SST8 = new List<double>();

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();

                int postingCounter = 2;
                int InsertionCounter = 4;
                int BrochureCounter = 5;
                int PostageCounter = 8;
                int ImprestCounter = 8;

                List<string> SCColumn = new List<string>();



                //printing
                for (int counter = 1; counter <= postingCounter; counter++)
                {
                    SqlCommand PostingCheck = new SqlCommand("SELECT Posting" + counter + "Charges FROM BillingMPR WHERE JobSheetNo=@LogTagNoPosting", cn);
                    PostingCheck.Parameters.AddWithValue("@LogTagNoPosting", LogTagNo);
                    SqlDataReader rmPostCheck = PostingCheck.ExecuteReader();

                    while (rmPostCheck.Read())
                    {
                        //if (rmPostCheck.HasRows)
                        if (!rmPostCheck.IsDBNull(0))
                        {
                            SCColumn.Add("Posting" + counter);
                        }
                    }

                    rmPostCheck.Close();
                }

                Debug.WriteLine("Masuk process table F");

                //insertion

                for (int counter = 1; counter <= InsertionCounter; counter++)
                {
                    SqlCommand InsertionCheck = new SqlCommand("SELECT Insertion" + counter + "Charges FROM BillingMPR WHERE JobSheetNo=@LogTagNoInsertion", cn);
                    InsertionCheck.Parameters.AddWithValue("@LogTagNoInsertion", LogTagNo);
                    SqlDataReader rmInsertionCheck = InsertionCheck.ExecuteReader();

                    while (rmInsertionCheck.Read())
                    {
                        if (!rmInsertionCheck.IsDBNull(0))
                        {
                            SCColumn.Add("Insertion" + counter);
                        }
                    }

                    rmInsertionCheck.Close();
                }

                //mix
                SqlCommand MixCheck = new SqlCommand("SELECT MixCharges FROM BillingMPR WHERE JobSheetNo=@LogTagNoMix", cn);
                MixCheck.Parameters.AddWithValue("@LogTagNoMix", LogTagNo);
                SqlDataReader rmMixCheck = MixCheck.ExecuteReader();

                while (rmMixCheck.Read())
                {
                    if (!rmMixCheck.IsDBNull(0))
                    {
                        SCColumn.Add("Mix");
                    }
                }

                rmMixCheck.Close();

                //statement
                SqlCommand StatementCheck = new SqlCommand("SELECT StatementCharges FROM BillingMPR WHERE JobSheetNo=@LogTagNoStatement", cn);
                StatementCheck.Parameters.AddWithValue("@LogTagNoStatement", LogTagNo);
                SqlDataReader rmStatementCheck = StatementCheck.ExecuteReader();

                while (rmStatementCheck.Read())
                {
                    if (!rmStatementCheck.IsDBNull(0))
                    {
                        SCColumn.Add("Statement");
                    }
                }

                rmStatementCheck.Close();

                //brochure
                for (int counter = 1; counter <= BrochureCounter; counter++)
                {
                    SqlCommand BrochureCheck = new SqlCommand("SELECT Brochure" + counter + "Charges FROM BillingMPR WHERE JobSheetNo=@LogTagNoBrochure", cn);
                    BrochureCheck.Parameters.AddWithValue("@LogTagNoBrochure", LogTagNo);
                    SqlDataReader rmBrochureCheck = BrochureCheck.ExecuteReader();

                    while (rmBrochureCheck.Read())
                    {
                        if (!rmBrochureCheck.IsDBNull(0))
                        {
                            SCColumn.Add("Brochure" + counter);
                        }
                    }

                    rmBrochureCheck.Close();
                }

                //MB5K until Discount

                SqlCommand BulkCheck = new SqlCommand("SELECT MB5KCharges, InsertingCharges, HandlingCharges, SealingCharges, TearingCharges, FoldingCharges, StockingCharges, LabellingCharges, SelfMailerCharges,SelfMaterialCharges, " +
                    "OvertimeCharges, CDCharges, CourierCharges, DeliveryCharges, MatchingCharges, LabellingRMCharges, IPDSCharges, ProgrammingCharges, ReturnMailCharges, RetainerCharges, RubberStampingCharges, GlueingCharges, " +
                    "CuttingCharges, MergingCharges, DiscountCharges, DataSFTPCharges FROM BillingMPR WHERE JobSheetNo=@LogTagNoBulk", cn);
                BulkCheck.Parameters.AddWithValue("@LogTagNoBulk", LogTagNo);
                SqlDataReader rmBulkCheck = BulkCheck.ExecuteReader();

                while (rmBulkCheck.Read())
                {
                    if (!rmBulkCheck.IsDBNull(0))
                    {
                        SCColumn.Add("MB5K");
                    }
                    if (!rmBulkCheck.IsDBNull(1))
                    {
                        SCColumn.Add("Inserting");
                    }
                    if (!rmBulkCheck.IsDBNull(2))
                    {
                        SCColumn.Add("Handling");
                    }
                    if (!rmBulkCheck.IsDBNull(3))
                    {
                        SCColumn.Add("Sealing");
                    }
                    if (!rmBulkCheck.IsDBNull(4))
                    {
                        SCColumn.Add("Tearing");
                    }
                    if (!rmBulkCheck.IsDBNull(5))
                    {
                        SCColumn.Add("Folding");
                    }
                    if (!rmBulkCheck.IsDBNull(6))
                    {
                        SCColumn.Add("Sticking");
                    }
                    if (!rmBulkCheck.IsDBNull(7))
                    {
                        SCColumn.Add("Labelling");
                    }
                    if (!rmBulkCheck.IsDBNull(8))
                    {
                        SCColumn.Add("SelfMailer");
                    }
                    if (!rmBulkCheck.IsDBNull(9))
                    {
                        SCColumn.Add("SelfMaterial");
                    }
                    if (!rmBulkCheck.IsDBNull(10))
                    {
                        SCColumn.Add("OverTime");
                    }
                    if (!rmBulkCheck.IsDBNull(11))
                    {
                        SCColumn.Add("CD");
                    }
                    if (!rmBulkCheck.IsDBNull(12))
                    {
                        SCColumn.Add("Courier");
                    }
                    if (!rmBulkCheck.IsDBNull(13))
                    {
                        SCColumn.Add("Delivery");
                    }
                    if (!rmBulkCheck.IsDBNull(14))
                    {
                        SCColumn.Add("Matching");
                    }
                    if (!rmBulkCheck.IsDBNull(15))
                    {
                        SCColumn.Add("LabellingRM");
                    }
                    if (!rmBulkCheck.IsDBNull(16))
                    {
                        SCColumn.Add("IPDS");
                    }
                    if (!rmBulkCheck.IsDBNull(17))
                    {
                        SCColumn.Add("Programming");
                    }
                    if (!rmBulkCheck.IsDBNull(18))
                    {
                        SCColumn.Add("ReturnMail");
                    }
                    if (!rmBulkCheck.IsDBNull(19))
                    {
                        SCColumn.Add("Retainer");
                    }
                    if (!rmBulkCheck.IsDBNull(20))
                    {
                        SCColumn.Add("RubberStamping");
                    }
                    if (!rmBulkCheck.IsDBNull(21))
                    {
                        SCColumn.Add("Glueing");
                    }
                    if (!rmBulkCheck.IsDBNull(22))
                    {
                        SCColumn.Add("Cutting");
                    }
                    if (!rmBulkCheck.IsDBNull(23))
                    {
                        SCColumn.Add("Merging");
                    }
                    if (!rmBulkCheck.IsDBNull(24))
                    {
                        SCColumn.Add("Discount");
                    }
                    if (!rmBulkCheck.IsDBNull(25))
                    {
                        SCColumn.Add("DataSFTP");
                    }

                }

                rmBulkCheck.Close();

                //postage and imprest
                for (int counter = 1; counter <= PostageCounter; counter++)
                {
                    //SqlCommand PostImpCheck = new SqlCommand("SELECT Postage" + counter + "Charges, Imprest" + counter + "Charges FROM BillingMPR WHERE JobSheetNo=@LogTagNoPostImp", cn);

                    //removed Imprest
                    SqlCommand PostImpCheck = new SqlCommand("SELECT Postage" + counter + "Charges FROM BillingMPR WHERE JobSheetNo=@LogTagNoPostImp", cn);

                    PostImpCheck.Parameters.AddWithValue("@LogTagNoPostImp", LogTagNo);
                    SqlDataReader rmPostImpCheck = PostImpCheck.ExecuteReader();

                    while (rmPostImpCheck.Read())
                    {
                        if (!rmPostImpCheck.IsDBNull(0))
                        {
                            SCColumn.Add("Postage" + counter);
                        }

                        //if (!rmPostImpCheck.IsDBNull(1))
                        //{
                        //    SCColumn.Add("Imprest" + counter);
                        //}
                    }

                    rmPostImpCheck.Close();
                }

                //SqlCommand BulkCheck2 = new SqlCommand("SELECT RebateCharges, FrankingCharges, Franking10Charges, AirmailCharges, SporeCharges, TLetterCharges, " +
                //"NPCCharges, Mix10Charges, RegisteredMailsCharges, ImprestRMCharges, RegisteredMails2Charges, ImprestRM2Charges FROM BillingMPR WHERE JobSheetNo=@JSBC2", cn);

                //removed ImprestRM and ImprestRM2
                SqlCommand BulkCheck2 = new SqlCommand("SELECT RebateCharges, FrankingCharges, Franking10Charges, AirmailCharges, SporeCharges, TLetterCharges, " +
                "NPCCharges, Mix10Charges, RegisteredMailsCharges, RegisteredMails2Charges FROM BillingMPR WHERE JobSheetNo=@JSBC2", cn);

                //SqlCommand BulkCheck2 = new SqlCommand("SELECT RebateCharges, FrankingCharges, Franking10Charges, AirmailCharges, SporeCharges, TLetterCharges, " +
                //    "NPCCharges, Mix10Charges, RegisteredMailsCharges, ImprestRMCharges, RegisteredMails2Charges, ImprestRM2Charges FROM BillingMPR WHERE JobSheetNo=@JSBC2", cn);
                BulkCheck2.Parameters.AddWithValue("@JSBC2", LogTagNo);
                SqlDataReader rmBC2 = BulkCheck2.ExecuteReader();

                while (rmBC2.Read())
                {
                    if (!rmBC2.IsDBNull(0))
                    {
                        SCColumn.Add("Rebate");
                    }
                    if (!rmBC2.IsDBNull(1))
                    {
                        SCColumn.Add("Franking");
                    }
                    if (!rmBC2.IsDBNull(2))
                    {
                        SCColumn.Add("Franking10");
                    }
                    if (!rmBC2.IsDBNull(3))
                    {
                        SCColumn.Add("Airmail");
                    }
                    if (!rmBC2.IsDBNull(4))
                    {
                        SCColumn.Add("Spore");
                    }
                    if (!rmBC2.IsDBNull(5))
                    {
                        SCColumn.Add("TLetter");
                    }
                    if (!rmBC2.IsDBNull(6))
                    {
                        SCColumn.Add("NPC");
                    }
                    if (!rmBC2.IsDBNull(7))
                    {
                        SCColumn.Add("Mix10");
                    }
                    if (!rmBC2.IsDBNull(8))
                    {
                        SCColumn.Add("RegisteredMails");
                    }
                    //if (!rmBC2.IsDBNull(9))
                    //{
                    //    SCColumn.Add("ImprestRM");
                    //}
                    if (!rmBC2.IsDBNull(9))
                    {
                        SCColumn.Add("RegisteredMails2");
                    }
                    //if (!rmBC2.IsDBNull(10))
                    //{
                    //    SCColumn.Add("ImprestRM2");
                    //}
                }

                //foreach (var x in SCColumn)
                //{
                //    Debug.WriteLine("item : " + x);
                //}

                double tax = 0;

                //table f
                Debug.WriteLine("LogTagNo : " + LogTagNo);

                foreach (var item in SCColumn)
                {
                    Debug.WriteLine(item);
                    SqlCommand TableF = new SqlCommand("SELECT " + item + "Charges FROM BillingMPR WHERE JobSheetNo=@JSTF", cn);
                    TableF.Parameters.AddWithValue("@JSTF", LogTagNo);
                    SqlDataReader rmTableF = TableF.ExecuteReader();

                    while (rmTableF.Read())
                    {
                        if (!rmTableF.IsDBNull(0))
                        {
                            Charges.Add(double.Parse(rmTableF.GetString(0)));
                            if (item == "CDArchiving" || item == "Programming" || item == "DataSFTP")
                            {
                                tax = double.Parse(rmTableF.GetString(0)) * 0.08;
                                SST8.Add(tax);
                            }
                        }

                        Process.Add(item);
                    }
                }


                cn.Close();
            }
            Debug.WriteLine("Total Tax : " + SST8.Sum());
            Charges.Add(SST8.Sum());

            return Charges;
        }

        public List<string> getMaterialCharges(string LogTagNo)
        {
            List<string> MaterialCharges = new List<string>();

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();

                SqlCommand cmd1 = new SqlCommand("SELECT SUM(CAST (Total_Charges as decimal(18,2))) as TotalCharges FROM MaterialDescriptionMail WHERE MaterialType='Paper' AND LogTagNo=@LogTagNo1", cn);
                cmd1.Parameters.AddWithValue("@LogTagNo1", LogTagNo);
                SqlDataReader rm1 = cmd1.ExecuteReader();

                while (rm1.Read())
                {
                    if (!rm1.IsDBNull(0) )
                    {
                        MaterialCharges.Add(rm1["TotalCharges"].ToString());
                    }
                    else
                    {
                        MaterialCharges.Add("0.00");

                    }
                }

                SqlCommand cmd2 = new SqlCommand("SELECT SUM(CAST (Total_Charges as decimal(18,2))) as TotalCharges FROM MaterialDescriptionMail WHERE MaterialType='Envelope' AND LogTagNo=@LogTagNo1", cn);
                cmd2.Parameters.AddWithValue("@LogTagNo1", LogTagNo);
                SqlDataReader rm2 = cmd2.ExecuteReader();

                while (rm2.Read())
                {
                    if (!rm2.IsDBNull(0) )
                    {
                        MaterialCharges.Add(rm2["TotalCharges"].ToString());

                    }
                    else
                    {
                        MaterialCharges.Add("0.00");

                    }
                }

                cn.Close();
            }

            return MaterialCharges;
        }

        //public string getEnvelopeCharges(string LogTagNo)
        //{
        //    string EnvelopeCharges = "";

        //    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        //    {
        //        cn.Open();
        //        SqlCommand cmd1 = new SqlCommand("SELECT Total_Charges FROM MaterialDescriptionMail WHERE MaterialType='Envelope' AND LogTagNo=@LogTagNo1", cn);
        //        cmd1.Parameters.AddWithValue("@LogTagNo1", LogTagNo);
        //        SqlDataReader rm1 = cmd1.ExecuteReader();

        //        while (rm1.Read())
        //        {
        //            if (!string.IsNullOrEmpty(rm1.GetString(0)))
        //            {
        //                EnvelopeCharges = rm1.GetString(0);

        //            }
        //            else
        //            {
        //                EnvelopeCharges = "0.00";

        //            }
        //        }

        //        cn.Open();
        //    }

        //    return EnvelopeCharges;
        //}

        public string getDatePost(string LogTagNo)
        {
            string DatePosting = "";

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();

                SqlCommand cmd1 = new SqlCommand("SELECT LEFT( CONVERT(varchar, PostingDateOn, 120), 10), PostingTime FROM PostingManifest WHERE LogTagNo = @LogTagNo", cn);
                cmd1.Parameters.AddWithValue("@LogTagNo", LogTagNo);
                SqlDataReader rm1 = cmd1.ExecuteReader();

                while (rm1.Read())
                {
                    if (!rm1.IsDBNull(0))
                    {
                        if (!string.IsNullOrEmpty(rm1.GetString(0)))
                        {
                            DatePosting = rm1.GetString(0);
                        }
                        else
                        {
                            DatePosting = "";
                        }
                    }
                    else
                    {
                        DatePosting = "";

                    }

                }

                cn.Close();
            }

            return DatePosting;
        }

        public string getTimePost(string LogTagNo)
        {
            string PostingTime = "";

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();


                SqlCommand cmd1 = new SqlCommand("SELECT PostingTime FROM PostingManifest WHERE LogTagNo=@LogTagNo1", cn);
                cmd1.Parameters.AddWithValue("@LogTagNo1", LogTagNo);
                SqlDataReader rm1 = cmd1.ExecuteReader();

                if(rm1.HasRows)
                {
                    while (rm1.Read())
                    {
                        if (!rm1.IsDBNull(0))
                        {
                            if (!string.IsNullOrEmpty(rm1.GetString(0)))
                            {
                                PostingTime = rm1.GetString(0);
                            }
                            else
                            {
                                PostingTime = "";
                            }
                        }
                        else
                        {
                            PostingTime = "";
                        }

                    }
                }
                else
                {
                    PostingTime = "";
                }


                cn.Close();
            }

            return PostingTime;
        }

        public string getDeletionDate(string LogTagNo)
        {
            DateTime postdate = DateTime.Now;
            DateTime DeletionDate = DateTime.Now;
            string postdatestr = "";
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();

                SqlCommand cmd1 = new SqlCommand("SELECT PostingDateOn FROM PostingManifest WHERE LogTagNo=@LogTagNo1", cn);
                cmd1.Parameters.AddWithValue("LogTagNo1", LogTagNo);
                SqlDataReader rm1 = cmd1.ExecuteReader();

                while (rm1.Read())
                {
                    if (!rm1.IsDBNull(0))
                    {
                        postdate = rm1.GetDateTime(0);
                        DeletionDate = postdate.AddDays(3);
                        postdatestr = DeletionDate.ToString("yyyy-MM-dd");

                        Debug.WriteLine("postdate : " + DeletionDate);
                        Debug.WriteLine("postdatestr : " + postdatestr);
                    }
                    else
                    {
                        postdatestr = "";
                    }
                }

                cn.Close();
            }

            return postdatestr;
        }

        public ActionResult ManageLogTagStatus(string set, string LogTagNo)
        {
            string role = Session["Role"] as string;

            ViewBag.Role = role;

            List<JobAuditTrailDetail> auditTrail = new List<JobAuditTrailDetail>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();

                if (set == "search")
                {
                    SqlCommand cmd = new SqlCommand("SELECT DISTINCT LogTagNo, JobSheetNo, Customer_Name, ProductName,Status,JobClass,Remarks FROM JobAuditTrailDetail WHERE LogTagNo LIKE @LogTagNo1", cn);
                    cmd.Parameters.AddWithValue("@LogTagNo1", "%" + LogTagNo + "%");
                    SqlDataReader rm = cmd.ExecuteReader();

                    int bil = 1;
                    if (rm.HasRows)
                    {
                        while (rm.Read())
                        {
                            var model = new JobAuditTrailDetail();
                            {
                                model.Bil = bil;
                                if (!rm.IsDBNull(0))
                                {
                                    model.LogTagNo = rm.GetString(0);

                                }

                                if (!rm.IsDBNull(1))
                                {
                                    model.JobSheetNo = rm.GetString(1);

                                }

                                if (!rm.IsDBNull(2))
                                {
                                    model.Customer_Name = rm.GetString(2);

                                }

                                if (!rm.IsDBNull(3))
                                {
                                    model.ProductName = rm.GetString(3);

                                }

                                if (!rm.IsDBNull(4))
                                {
                                    model.Status = rm.GetString(4);
                                }

                                if (!rm.IsDBNull(5))
                                {
                                    model.JobClass = rm.GetString(5);

                                }

                                if (!rm.IsDBNull(6))
                                {
                                    model.Remark = rm.GetString(6);

                                }



                                //model.Id = rm.GetGuid(5);
                            }
                            bil++;
                            auditTrail.Add(model);
                        }
                    }


                }
                else
                {
                    SqlCommand cmd = new SqlCommand("SELECT DISTINCT LogTagNo, JobSheetNo, Customer_Name, ProductName,Status,JobClass,Remarks FROM JobAuditTrailDetail", cn);
                    SqlDataReader rm = cmd.ExecuteReader();

                    int bil = 1;
                    while (rm.Read())
                    {
                        var model = new JobAuditTrailDetail();
                        {
                            model.Bil = bil;
                            if (!rm.IsDBNull(0))
                            {
                                model.LogTagNo = rm.GetString(0);

                            }

                            if (!rm.IsDBNull(1))
                            {
                                model.JobSheetNo = rm.GetString(1);

                            }

                            if (!rm.IsDBNull(2))
                            {
                                model.Customer_Name = rm.GetString(2);

                            }

                            if (!rm.IsDBNull(3))
                            {
                                model.ProductName = rm.GetString(3);

                            }

                            if (!rm.IsDBNull(4))
                            {
                                model.Status = rm.GetString(4);
                            }

                            if (!rm.IsDBNull(5))
                            {
                                model.JobClass = rm.GetString(5);

                            }

                            if (!rm.IsDBNull(6))
                            {
                                model.Remark = rm.GetString(6);

                            }
                            //model.Id = rm.GetGuid(5);
                        }
                        bil++;
                        auditTrail.Add(model);
                    }

                }

                cn.Close();
            }
            return View(auditTrail);
        }

        public ActionResult ManageProductTransaction(string set, string search)
        {
            string parameterizedSearch = "%" + search + "%";
            ViewBag.Role = Session["Role"].ToString();

            List<JobInstruction> jobInstructions = new List<JobInstruction>();

            if (set == "search")
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT Id,Company,Customer_Name,ProductName,JobClass,JobType,JobSheetNo,FORMAT(CONVERT(date, CreatedOn), 'dd-MM-yyy') as CreatedOn, CreateUser FROM [IflowSeed].[dbo].[JobInstruction] " +
                        "WHERE Company LIKE @search OR Customer_Name LIKE @search or ProductName LIKE @search OR JobClass LIKE @search OR JobType LIKE @search OR JobSheetNo LIKE @search " +
                        "ORDER BY Customer_Name ASC, ProductName ASC", cn);

                    cmd.Parameters.AddWithValue("@search", parameterizedSearch);

                    SqlDataReader rm = cmd.ExecuteReader();

                    int bil = 1;

                    try
                    {

                        while (rm.Read())
                        {
                            var model = new JobInstruction();
                            {
                                model.Bil = bil;
                                if (rm.IsDBNull(0) == false)
                                {
                                    model.Id = rm.GetGuid(0);
                                }
                                if (rm.IsDBNull(1) == false)
                                {
                                    model.Company = rm.GetString(1);
                                }
                                if (rm.IsDBNull(2) == false)
                                {
                                    model.Customer_Name = rm.GetString(2);
                                }
                                if (rm.IsDBNull(3) == false)
                                {
                                    model.ProductName = rm.GetString(3);
                                }
                                if (rm.IsDBNull(4) == false)
                                {
                                    model.JobClass = rm.GetString(4);
                                }
                                if (rm.IsDBNull(5) == false)
                                {
                                    model.JobType = rm.GetString(5);
                                }
                                if (rm.IsDBNull(6) == false)
                                {
                                    model.JobSheetNo = rm.GetString(6);
                                }
                                if (rm.IsDBNull(7) == false)
                                {
                                    model.CreatedOn = rm["CreatedOn"].ToString();
                                }
                                if (rm.IsDBNull(8) == false)
                                {
                                    model.CreateUser = rm.GetString(8);
                                }

                            }

                            bil++;
                            jobInstructions.Add(model);

                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Error : " + ex);
                    }

                    rm.Close();

                    cn.Close();
                }

                return View(jobInstructions);
            }
            else
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT Id,Company,Customer_Name,ProductName,JobClass,JobType,JobSheetNo, FORMAT(CONVERT(date, CreatedOn), 'dd/MM/yyy') as CreatedOn, CreateUser FROM [IflowSeed].[dbo].[JobInstruction] ORDER BY Customer_Name ASC, ProductName ASC", cn);
                    SqlDataReader rm = cmd.ExecuteReader();

                    int bil = 1;
                    while (rm.Read())
                    {
                        var model = new JobInstruction();
                        {
                            model.Bil = bil;
                            if (rm.IsDBNull(0) == false)
                            {
                                model.Id = rm.GetGuid(0);
                            }
                            if (rm.IsDBNull(1) == false)
                            {
                                model.Company = rm.GetString(1);
                            }
                            if (rm.IsDBNull(2) == false)
                            {
                                model.Customer_Name = rm.GetString(2);
                            }
                            if (rm.IsDBNull(3) == false)
                            {
                                model.ProductName = rm.GetString(3);
                            }
                            if (rm.IsDBNull(4) == false)
                            {
                                model.JobClass = rm.GetString(4);
                            }
                            if (rm.IsDBNull(5) == false)
                            {
                                model.JobType = rm.GetString(5);
                            }
                            if (rm.IsDBNull(6) == false)
                            {
                                model.JobSheetNo = rm.GetString(6);
                            }
                            if (rm.IsDBNull(7) == false)
                            {
                                model.CreatedOn = rm["CreatedOn"].ToString();
                            }
                            if (rm.IsDBNull(8) == false)
                            {
                                model.CreateUser = rm.GetString(8);
                            }


                        }
                        bil++;
                        jobInstructions.Add(model);
                    }

                    rm.Close();

                    cn.Close();
                }

                return View(jobInstructions);

            }

        }

        public ActionResult ProductTransaction(string JobsheetNo)
        {
            List<View_TransactionJob> productTransaction = new List<View_TransactionJob>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand("  SELECT JobSheetNo,Customer_Name,Cust_Department,ProductName,JobClass FROM JobInstruction WHERE JobSheetNo=@JobSheetNo", cn);
                cmd.Parameters.AddWithValue("@JobsheetNo", JobsheetNo);
                SqlDataReader rm = cmd.ExecuteReader();

                if (rm.Read())
                {
                    if (rm.IsDBNull(0) == false)
                    {
                        ViewBag.JobSheetNo = rm.GetString(0);
                    }

                    if (rm.IsDBNull(1) == false)
                    {
                        ViewBag.Customer_Name = rm.GetString(1);
                    }

                    if (rm.IsDBNull(2) == false)
                    {
                        ViewBag.Cust_Department = rm.GetString(2);
                    }

                    if (rm.IsDBNull(3) == false)
                    {
                        ViewBag.ProductName = rm.GetString(3);
                    }

                    if (rm.IsDBNull(4) == false)
                    {
                        ViewBag.JobClass = rm.GetString(4);
                    }

                    rm.Close();


                    //SqlCommand cmd2 = new SqlCommand("SELECT  Status, LogTagNo, JobClass, AccountsQty, ImpressionQty, PagesQty, TotalAuditTrail, JobType, DateCollectedOn FROM View_TransactionJob WHERE JobSheetNo=@JobSheetNo1", cn);


                    SqlCommand cmd2= new SqlCommand("SELECT JobAuditTrailDetail.JobSheetNo, JobAuditTrailDetail.LogTagNo, MAX(JobAuditTrailDetail.RevStrtDateOn) AS CollectedDate, MAX(JobAuditTrailDetail.JobClass) AS JobClass,MAX(JobAuditTrailDetail.Status) AS Status, " +
                        "MAX(JobAuditTrailDetail.JobType) AS JobType, MAX(JobAuditTrailDetail.Id),(SELECT COUNT(DISTINCT LogTagNo) FROM JobAuditTrailDetail WHERE JobSheetNo = @JobSheetNo1) as TotalAT FROM JobInstruction INNER JOIN JobAuditTrailDetail ON JobInstruction.JobSheetNo = JobAuditTrailDetail.JobSheetNo " +
                        "WHERE JobAuditTrailDetail.JobSheetNo = @JobSheetNo1 GROUP BY JobAuditTrailDetail.LogTagNo, JobAuditTrailDetail.JobSheetNo ORDER BY MAX(JobAuditTrailDetail.RevStrtDateOn) DESC ", cn);

                    //SqlCommand cmd2 = new SqlCommand("SELECT JobAuditTrailDetail.JobSheetNo, JobAuditTrailDetail.LogTagNo, MAX(JobAuditTrailDetail.RevStrtDateOn) AS CollectedDate, MAX(JobAuditTrailDetail.JobClass) AS JobClass, " +
                    //    "MAX(JobAuditTrailDetail.Status) AS Status, MAX(JobAuditTrailDetail.JobType) AS JobType, MAX(JobAuditTrailDetail.Id) FROM JobInstruction INNER JOIN JobAuditTrailDetail ON JobInstruction.JobSheetNo = JobAuditTrailDetail.JobSheetNo " +
                    //    "WHERE JobAuditTrailDetail.JobSheetNo =@JobSheetNo1 GROUP BY JobAuditTrailDetail.LogTagNo, JobAuditTrailDetail.JobSheetNo ORDER BY JobAuditTrailDetail.LogTagNo;", cn);


                    //SqlCommand cmd2 = new SqlCommand("SELECT Status, LogTagNo, JobClass, AccQty, ImpQty, PageQty, COUNT(LogTagNo), JobType, RevStrtDateOn, Customer_Name, ProductName, Id FROM JobAuditTrailDetail WHERE JobSheetNo=@JobSheetNo1 GROUP BY Status, LogTagNo, JobClass, AccQty, ImpQty, PageQty, JobType, RevStrtDateOn, Customer_Name,ProductName, Id", cn);

                    cmd2.Parameters.AddWithValue("@JobsheetNo1", JobsheetNo);
                    SqlDataReader rm2 = cmd2.ExecuteReader();

                    int bil = 1;
                    while (rm2.Read())
                    {
                        var model2 = new View_TransactionJob();
                        {
                            model2.Bil = bil;

                            if (rm2.IsDBNull(1) == false)
                            {
                                model2.LogTagNo = rm2.GetString(1);
                            }

                            if (rm2.IsDBNull(2) == false)
                            {
                                model2.DateCollectedOn = rm2.GetDateTime(2);
                            }

                            if (rm2.IsDBNull(3) == false)
                            {
                                model2.JobClass = rm2.GetString(3);
                            }

                            if (rm2.IsDBNull(4) == false)
                            {
                                model2.Status = rm2.GetString(4);
                            }

                            if (rm2.IsDBNull(5) == false)
                            {
                                model2.JobType = rm2.GetString(5);
                            }

                            if (rm2.IsDBNull(6) == false)
                            {
                                model2.Id = rm2.GetGuid(6);
                            }

                            List<string> AIP = getTotalAIP(rm2.GetString(1));

                            model2.AccountQty = AIP[0];
                            model2.ImpressionQty = AIP[1];
                            model2.PagesQty = AIP[2];
                            model2.TotalAuditTrail = rm2.GetInt32(7);
                            ViewBag.TotalAuditTrail = rm2.GetInt32(7);

                        }

                        bil++;
                        productTransaction.Add(model2);
                    }

                    rm2.Close();

                    //SqlCommand cmd3 = new SqlCommand("SELECT count(*) FROM  View_TransactionJob WHERE JobSheetNo=@JobSheetNo2", cn);
                    //cmd3.Parameters.AddWithValue("@JobsheetNo2", JobsheetNo);
                    //SqlDataReader rm3 = cmd3.ExecuteReader();

                    //while (rm3.Read())
                    //{
                    //    if (rm3.IsDBNull(0) == false)
                    //    {
                    //        ViewBag.TotalAuditTrail = rm3.GetInt32(0);
                    //    }
                    //}

                }

                cn.Close();
            }


            return View(productTransaction);
        }

        public List<string> getTotalAIP(string LogTagNo)
        {
            List<string> AIP = new List<string>();

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand cmd1 = new SqlCommand("SELECT SUM(CAST(AccQty AS INT)) as AccQty, SUM(CAST(ImpQty AS INT)) as ImpQty, SUM(CAST(PageQty AS INT)) as PageQty FROM JobAuditTrailDetail WHERE LogTagNo=@LogTagNo1", cn);
                cmd1.Parameters.AddWithValue("@LogTagNo1", LogTagNo);
                SqlDataReader rm1 = cmd1.ExecuteReader();

                while (rm1.Read())
                {
                    if (rm1.GetInt32(0) != 0)
                    {
                        AIP.Add(rm1["AccQty"].ToString());
                    }
                    else
                    {
                        AIP.Add("0");
                    }

                    if (rm1.GetInt32(1) != 0)
                    {
                        AIP.Add(rm1["ImpQty"].ToString());
                    }
                    else
                    {
                        AIP.Add("0");
                    }

                    if (rm1.GetInt32(2) != 0)
                    {
                        AIP.Add(rm1["PageQty"].ToString());
                    }
                    else
                    {
                        AIP.Add("0");
                    }
                }

                SqlCommand cmd2 = new SqlCommand("SELECT COUNT(LogTagNo) as TotalAT FROM JobAuditTrailDetail WHERE LogTagNo=@LogTagNo2", cn);
                cmd2.Parameters.AddWithValue("@LogTagNo2", LogTagNo);
                SqlDataReader rm2 = cmd2.ExecuteReader();

                while (rm2.Read())
                {
                    if (rm2.GetInt32(0) != 0)
                    {
                        AIP.Add(rm2["TotalAT"].ToString());
                    }
                    else
                    {
                        AIP.Add("0");
                    }
                }

                cn.Close();

            }


            return AIP;
        }


        public ActionResult CreditCardReport()
        {
            List<DailyTracking> creditCardList = new List<DailyTracking>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();

                int bil = 1;

                SqlCommand cmd1 = new SqlCommand("SELECT LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id FROM DailyTracking WHERE ProductName LIKE '%Credit Card%' OR ProductName LIKE '%Charge Card%' OR ProductName LIKE '%AMEX Krysflyer Platinum%'", cn);
                SqlDataReader rm1 = cmd1.ExecuteReader();

                while (rm1.Read())
                {
                    if (!string.IsNullOrEmpty(rm1.GetString(4)))
                    {
                        DailyTracking model = new DailyTracking();
                        {
                            model.Bil = bil++;
                            model.CreatedOn = rm1.GetString(0);
                            model.JobClass = rm1["JobClass"].ToString();
                            model.Customer_Name = rm1["Customer_Name"].ToString();
                            model.ProductName = rm1["ProductName"].ToString();
                            model.LogTagNo = rm1["LogTagNo"].ToString();
                            model.Id = rm1.GetGuid(5);
                        };

                        creditCardList.Add(model);
                    }
                    else
                    {
                        return View();
                    }
                }

                SqlCommand getTotal = new SqlCommand("SELECT count(Id) as totalid FROM DailyTracking WHERE ProductName LIKE '%Credit Card%' OR ProductName LIKE '%Charge Card%' OR ProductName LIKE '%AMEX Krysflyer Platinum%'", cn);
                SqlDataReader rmTotal = getTotal.ExecuteReader();

                while (rmTotal.Read())
                {
                    string totalstr = rmTotal["totalid"].ToString();

                    if (!string.IsNullOrEmpty(totalstr))
                    {
                        ViewBag.Total = totalstr;
                    }
                    else
                    {
                        ViewBag.Total = "0";
                    }

                }


                rmTotal.Close();

                List<int> TotalAcc = new List<int>();
                List<int> TotalImp = new List<int>();
                List<int> TotalPages = new List<int>();


                SqlCommand getTotalrecords = new SqlCommand(" SELECT AccountsQty, ImpressionQty, PagesQty from DailyTracking WHERE ProductName LIKE '%Credit Card%' OR ProductName LIKE '%Charge Card%' OR ProductName LIKE '%AMEX Krysflyer Platinum%'", cn);
                SqlDataReader rmTotalrecords = getTotalrecords.ExecuteReader();

                while (rmTotalrecords.Read())
                {
                    string totalAccstr = rmTotalrecords["AccountsQty"].ToString();
                    string totalImpstr = rmTotalrecords["ImpressionQty"].ToString();
                    string totalPagesstr = rmTotalrecords["PagesQty"].ToString();

                    //Debug.WriteLine("totalAccstr : " + totalAccstr);
                    //Debug.WriteLine("totalImpstr : " + totalImpstr);
                    //Debug.WriteLine("totalPagesstr : " + totalPagesstr);



                    try
                    {
                        int totalAcc = Convert.ToInt32(totalAccstr);
                        TotalAcc.Add(totalAcc);
                    }
                    catch
                    {
                        int totalAcc = 0;
                        TotalAcc.Add(totalAcc);
                    }

                    try
                    {
                        int totalImp = Convert.ToInt32(totalImpstr);
                        TotalImp.Add(totalImp);
                    }
                    catch
                    {
                        int totalImp = 0;
                        TotalImp.Add(totalImp);
                    }

                    try
                    {
                        int totalPages = Convert.ToInt32(totalPagesstr);
                        TotalPages.Add(totalPages);
                    }
                    catch
                    {
                        int totalPages = 0;
                        TotalPages.Add(totalPages);
                    }

                    //if (totalAccstr!=null|| totalAccstr != " ")
                    //{
                    //    int totalAcc = Convert.ToInt32(totalAccstr);
                    //    TotalAcc.Add(totalAcc);
                    //}
                    //else
                    //{
                    //    int totalAcc = 0;
                    //    TotalAcc.Add(totalAcc);
                    //}

                    //if (totalImpstr != null|| totalImpstr != " ")
                    //{
                    //    int totalImp = Convert.ToInt32(totalImpstr);
                    //    TotalImp.Add(totalImp);
                    //}
                    //else
                    //{
                    //    int totalImp = 0;
                    //    TotalImp.Add(totalImp);
                    //}

                    //if (totalPagesstr != null || totalPagesstr != " ")
                    //{
                    //    int totalPages = Convert.ToInt32(totalPagesstr);
                    //    TotalPages.Add(totalPages);
                    //}
                    //else
                    //{
                    //    int totalPages = 0;
                    //    TotalPages.Add(totalPages);
                    //}

                }

                ViewBag.TotalAcc = TotalAcc.Sum();
                ViewBag.TotalImp = TotalImp.Sum();
                ViewBag.TotalPages = TotalPages.Sum();

                rmTotalrecords.Close();

                cn.Close();
            }

            return View(creditCardList);
        }

        [HttpPost]
        public async Task<ActionResult> CreditCardReport(FormCollection formCollection)
        {
            string StartDate = formCollection["StartDate"];
            string EndDate = formCollection["EndDate"];

            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;

            //Debug.WriteLine("StartDate : " + StartDate);
            //Debug.WriteLine("EndDate : " + EndDate);


            List<DailyTracking> creditCardList = new List<DailyTracking>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();

                int bil = 1;

                SqlCommand cmd1 = new SqlCommand("SELECT LEFT( CONVERT(varchar, CreatedOn, 120), 10),JobClass,Customer_Name,ProductName,LogTagNo,Id FROM DailyTracking WHERE ProductName LIKE '%Credit Card%' OR ProductName LIKE '%Charge Card%' AND CONVERT(VARCHAR, CreatedOn, 23)BETWEEN @StartDate AND @EndDate", cn);
                cmd1.Parameters.AddWithValue("@StartDate", StartDate);
                cmd1.Parameters.AddWithValue("@EndDate", EndDate);

                SqlDataReader rm1 = cmd1.ExecuteReader();

                while (rm1.Read())
                {
                    if (!string.IsNullOrEmpty(rm1.GetString(4)))
                    {
                        DailyTracking model = new DailyTracking();
                        {
                            model.Bil = bil++;
                            model.CreatedOn = rm1.GetString(0);
                            model.JobClass = rm1["JobClass"].ToString();
                            model.Customer_Name = rm1["Customer_Name"].ToString();
                            model.ProductName = rm1["ProductName"].ToString();
                            model.LogTagNo = rm1["LogTagNo"].ToString();
                            model.Id = rm1.GetGuid(5);
                        };

                        creditCardList.Add(model);
                    }
                    else
                    {
                        return View();
                    }
                }

                SqlCommand getTotal = new SqlCommand("SELECT count(Id) as totalid FROM DailyTracking WHERE ProductName LIKE '%Credit Card%' OR ProductName LIKE '%Charge Card%' OR ProductName LIKE '%AMEX Krysflyer Platinum%'", cn);
                SqlDataReader rmTotal = getTotal.ExecuteReader();

                while (rmTotal.Read())
                {
                    string totalstr = rmTotal["totalid"].ToString();

                    if (!string.IsNullOrEmpty(totalstr))
                    {
                        ViewBag.Total = totalstr;
                    }
                    else
                    {
                        ViewBag.Total = "0";
                    }

                }


                rmTotal.Close();

                List<int> TotalAcc = new List<int>();
                List<int> TotalImp = new List<int>();
                List<int> TotalPages = new List<int>();


                SqlCommand getTotalrecords = new SqlCommand(" SELECT AccountsQty, ImpressionQty, PagesQty from DailyTracking WHERE ProductName LIKE '%Credit Card%' OR ProductName LIKE '%Charge Card%' OR ProductName LIKE '%AMEX Krysflyer Platinum%'", cn);
                SqlDataReader rmTotalrecords = getTotalrecords.ExecuteReader();

                while (rmTotalrecords.Read())
                {
                    string totalAccstr = rmTotalrecords["AccountsQty"].ToString();
                    string totalImpstr = rmTotalrecords["ImpressionQty"].ToString();
                    string totalPagesstr = rmTotalrecords["PagesQty"].ToString();

                    //Debug.WriteLine("totalAccstr : " + totalAccstr);
                    //Debug.WriteLine("totalImpstr : " + totalImpstr);
                    //Debug.WriteLine("totalPagesstr : " + totalPagesstr);



                    try
                    {
                        int totalAcc = Convert.ToInt32(totalAccstr);
                        TotalAcc.Add(totalAcc);
                    }
                    catch
                    {
                        int totalAcc = 0;
                        TotalAcc.Add(totalAcc);
                    }

                    try
                    {
                        int totalImp = Convert.ToInt32(totalImpstr);
                        TotalImp.Add(totalImp);
                    }
                    catch
                    {
                        int totalImp = 0;
                        TotalImp.Add(totalImp);
                    }

                    try
                    {
                        int totalPages = Convert.ToInt32(totalPagesstr);
                        TotalPages.Add(totalPages);
                    }
                    catch
                    {
                        int totalPages = 0;
                        TotalPages.Add(totalPages);
                    }

                    //if (totalAccstr!=null|| totalAccstr != " ")
                    //{
                    //    int totalAcc = Convert.ToInt32(totalAccstr);
                    //    TotalAcc.Add(totalAcc);
                    //}
                    //else
                    //{
                    //    int totalAcc = 0;
                    //    TotalAcc.Add(totalAcc);
                    //}

                    //if (totalImpstr != null|| totalImpstr != " ")
                    //{
                    //    int totalImp = Convert.ToInt32(totalImpstr);
                    //    TotalImp.Add(totalImp);
                    //}
                    //else
                    //{
                    //    int totalImp = 0;
                    //    TotalImp.Add(totalImp);
                    //}

                    //if (totalPagesstr != null || totalPagesstr != " ")
                    //{
                    //    int totalPages = Convert.ToInt32(totalPagesstr);
                    //    TotalPages.Add(totalPages);
                    //}
                    //else
                    //{
                    //    int totalPages = 0;
                    //    TotalPages.Add(totalPages);
                    //}

                }

                ViewBag.TotalAcc = TotalAcc.Sum();
                ViewBag.TotalImp = TotalImp.Sum();
                ViewBag.TotalPages = TotalPages.Sum();

                rmTotalrecords.Close();

                cn.Close();
            }

            return View(creditCardList);
        }

        public ActionResult ExportExcelCreditCardReport(string ids, string StartDate, string EndDate)
        {
            // Split the string 'ids' into individual IDs
            Debug.WriteLine("List :" + ids);
            var idList = ids.Split(',').ToList();
            List<string> PostingInfo = new List<string>();

            Debug.WriteLine("Start Date Excel : " + StartDate);
            Debug.WriteLine("End Date Excel : " + EndDate);

            string LogTagNo = "";


            // Process the list of IDs here
            // You can access the IDs in the 'idList' variable
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");

            //foreach (var a in idList)
            //{
            //    Debug.WriteLine("ID :" + a);
            //}

            foreach (var id in idList)
            {
                // Process each ID as needed
                Debug.WriteLine("ID :" + id);
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn.Open();

                    SqlCommand cmd1 = new SqlCommand("SELECT LogTagNo FROM DailyTracking WHERE Id=@Id1", cn);
                    cmd1.Parameters.AddWithValue("@Id1", id);
                    SqlDataReader rm1 = cmd1.ExecuteReader();

                    while (rm1.Read())
                    {
                        LogTagNo = rm1.GetString(0);
                        Debug.WriteLine("LogTagNo : " + LogTagNo);
                    }



                    SqlCommand command = new SqlCommand("SELECT LEFT( CONVERT(varchar, A.CreatedOn, 120), 10),A.Customer_Name,A.ProductName,A.LogTagNo,A.AccountsQty,A.PagesQty,A.ImpressionQty,LEFT( CONVERT(varchar, B.StartDateOn, 120), 10),B.StartTime,LEFT( CONVERT(varchar, B.EndDateOn, 120), 10)," +
                        "B.EndTime,LEFT( CONVERT(varchar, B.Ins_StartDateOn, 120), 10),B.Ins_StartTime,LEFT( CONVERT(varchar, B.Ins_EndDateOn, 120), 10),B.Ins_EndTime FROM DailyTracking AS A FULL JOIN ProductionSlip AS B ON A.LogTagNo=B.LogTagNo WHERE A.LogTagNo=@LogTagNo1", cn);
                    command.Parameters.AddWithValue("@LogTagNo1", LogTagNo);

                    //SqlCommand command = new SqlCommand("SELECT LEFT( CONVERT(varchar, A.CreatedOn, 120), 10),A.JobClass,A.Customer_Name,A.ProductName,A.LogTagNo,A.AccountsQty,A.PagesQty,A.ImpressionQty,LEFT( CONVERT(varchar, A.StartDateOn, 120), 10)," +
                    //"StartTime,LEFT( CONVERT(varchar, A.EndDateOn, 120), 10),A.EndTime,PIC,LEFT( CONVERT(varchar, A.ProcessStartDateOn, 120), 10),A.ProcessStartTime,A.TimeTaken,A.DatePostOn,A.DatePostTime,LEFT( CONVERT(varchar, A.DateApproveOn, 120), 10),A.DateApproveTime," +
                    //"A.ItSubmitOn,A.DateDeletionOn,A.PaperType,A.JobType,B.PaperType,B.JobClass FROM DailyTracking as A INNER JOIN JobInstruction AS B ON A.JobSheetNo=B.JobSheetNo WHERE A.Id=@Id", cn);
                    //command.Parameters.AddWithValue("@Id", id);

                    SqlDataReader rm = command.ExecuteReader();

                    while (rm.Read())
                    {
                        var model = new DailyTracking();
                        {
                            //ada 24 
                            if (rm.IsDBNull(0) == false)
                            {
                                model.CreatedOn = rm.GetString(0);
                            }
                            else
                            {
                                model.CreatedOn = "";

                            }

                            if (rm.IsDBNull(1) == false)
                            {
                                model.Customer_Name = rm.GetString(1);
                            }
                            else
                            {
                                model.Customer_Name = "";

                            }

                            if (rm.IsDBNull(2) == false)
                            {
                                model.ProductName = rm.GetString(2);
                            }
                            else
                            {
                                model.ProductName = "";
                            }

                            if (rm.IsDBNull(3) == false)
                            {
                                model.LogTagNo = rm.GetString(3);
                            }
                            else
                            {
                                model.LogTagNo = "";
                            }

                            if (rm.IsDBNull(4) == false)
                            {
                                model.AccountsQty = rm.GetString(4);
                            }
                            else
                            {
                                model.AccountsQty = "";
                            }

                            if (rm.IsDBNull(5) == false)
                            {
                                model.PagesQty = rm.GetString(5);
                            }
                            else
                            {
                                model.PagesQty = "";
                            }

                            if (rm.IsDBNull(6) == false)
                            {
                                model.ImpressionQty = rm.GetString(6);
                            }
                            else
                            {
                                model.ImpressionQty = "";
                            }

                            if (rm.IsDBNull(7) == false)
                            {
                                model.StartDateOn = rm.GetString(7);
                            }
                            else
                            {
                                model.StartDateOn = "-";
                            }

                            if (rm.IsDBNull(8) == false)
                            {
                                model.StartTime = rm.GetString(8);
                            }
                            else
                            {
                                model.StartTime = "-";
                            }

                            if (rm.IsDBNull(9) == false)
                            {
                                model.EndDateOn = rm.GetString(9);
                            }
                            else
                            {
                                model.EndDateOn = "-";
                            }

                            if (rm.IsDBNull(10) == false)
                            {
                                model.EndTime = rm.GetString(10);
                            }
                            else
                            {
                                model.EndTime = "-";
                            }

                            if (rm.IsDBNull(11) == false)
                            {
                                model.Ins_StartDateOn = rm.GetString(11);
                            }
                            else
                            {
                                model.Ins_StartDateOn = "-";
                            }

                            if (rm.IsDBNull(12) == false)
                            {
                                model.Ins_StartTime = rm.GetString(12);
                            }
                            else
                            {
                                model.Ins_StartTime = "-";
                            }

                            if (rm.IsDBNull(13) == false)
                            {
                                model.Ins_EndDateOn = rm.GetString(13);
                            }
                            else
                            {
                                model.Ins_EndDateOn = "-";
                            }

                            if (rm.IsDBNull(14) == false)
                            {
                                model.Ins_EndTime = rm.GetString(14);
                            }
                            else
                            {
                                model.Ins_EndTime = "-";
                            }


                        };

                        exportdailytracking.Add(model);
                    }

                    //string excelName = " REPORT-" + DateStartTxt + "-" + DateEndTxt;

                    //using (var memoryStream = new MemoryStream())
                    //{
                    //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //    Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                    //    excel.SaveAs(memoryStream);
                    //    memoryStream.WriteTo(Response.OutputStream);
                    //    Response.Flush();
                    //    Response.End();
                    //}

                    //try
                    //{

                    //}
                    //catch 
                    //{
                    //    TempData["Error"] = "<script>alert('No Data Available');</script>";
                    //    return RedirectToAction("CreditCardReport", "Report");
                    //}




                }

                string excelName = "EXCEL SAMPLE";

                workSheet.TabColor = System.Drawing.Color.Black;

                workSheet.DefaultRowHeight = 12;
                workSheet.Row(1).Height = 20;
                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(1).Style.Font.Bold = true;

                workSheet.Cells[1, 1].Value = "Collection Date";
                workSheet.Cells[1, 2].Value = "Customer";
                workSheet.Cells[1, 3].Value = "Product";
                workSheet.Cells[1, 4].Value = "LogTagNo";
                workSheet.Cells[1, 5].Value = "Pages Qty";
                workSheet.Cells[1, 6].Value = "Impression Qty";
                workSheet.Cells[1, 7].Value = "Account Qty";
                workSheet.Cells[1, 8].Value = "Start Print Date";
                workSheet.Cells[1, 9].Value = "Start Print Time";
                workSheet.Cells[1, 10].Value = "End Print Date";
                workSheet.Cells[1, 11].Value = "End Print Time";
                workSheet.Cells[1, 12].Value = "Ins Start Date";
                workSheet.Cells[1, 13].Value = "Ins Start Time";
                workSheet.Cells[1, 14].Value = "Ins End Date";
                workSheet.Cells[1, 15].Value = "Ins End Time";
                //workSheet.Cells[1, 16].Value = "Time Taken";
                //workSheet.Cells[1, 17].Value = "Date Post On";
                //workSheet.Cells[1, 18].Value = "Date Post Time";
                //workSheet.Cells[1, 19].Value = "Date Approve On";
                //workSheet.Cells[1, 20].Value = "Date Approve Time";
                //workSheet.Cells[1, 21].Value = "It Submit On";
                //workSheet.Cells[1, 22].Value = "Date Deletion On";
                //workSheet.Cells[1, 23].Value = "Paper Type";
                //workSheet.Cells[1, 24].Value = "Job Type";

                int recordIndex = 2;

                foreach (var x in exportdailytracking)
                {
                    //Debug.WriteLine("Created On: " + x.CreatedOn);
                    //Debug.WriteLine("Job Class: " + x.JobClass);
                    //Debug.WriteLine("Customer Name: " + x.Customer_Name);
                    //Debug.WriteLine("Product Name: " + x.ProductName);
                    //Debug.WriteLine("Log Tag No: " + x.LogTagNo);
                    //Debug.WriteLine("Accounts Qty: " + x.AccountsQty);
                    //Debug.WriteLine("Pages Qty: " + x.PagesQty);
                    //Debug.WriteLine("Impression Qty: " + x.ImpressionQty);
                    //Debug.WriteLine("Start Date On: " + x.StartDateOn);
                    //Debug.WriteLine("Start Time: " + x.StartTime);
                    //Debug.WriteLine("End Date On: " + x.EndDateOn);
                    //Debug.WriteLine("End Time: " + x.EndTime);
                    //Debug.WriteLine("PIC: " + x.PIC);
                    //Debug.WriteLine("Process Start Date On: " + x.ProcessStartDateOn);
                    //Debug.WriteLine("Process Start Time: " + x.ProcessStartTime);
                    //Debug.WriteLine("Time Taken: " + x.TimeTaken);
                    //Debug.WriteLine("Date Post On: " + x.DatePostOn);
                    //Debug.WriteLine("Date Post Time: " + x.DatePostTime);
                    //Debug.WriteLine("Date Approve On: " + x.DateApproveOn);
                    //Debug.WriteLine("Date Approve Time: " + x.DateApproveTime);
                    //Debug.WriteLine("IT Submit On: " + x.ItSubmitOn);
                    //Debug.WriteLine("Date Deletion On: " + x.DateDeletionOn);
                    //Debug.WriteLine("Paper Type: " + x.PaperType);
                    //Debug.WriteLine("Job Type: " + x.JobType);
                    //Debug.WriteLine("=======================================================================================");

                    workSheet.Cells[recordIndex, 1].Value = x.CreatedOn;
                    workSheet.Cells[recordIndex, 2].Value = x.Customer_Name;
                    workSheet.Cells[recordIndex, 3].Value = x.ProductName;
                    workSheet.Cells[recordIndex, 4].Value = x.LogTagNo;
                    workSheet.Cells[recordIndex, 5].Value = x.PagesQty;
                    workSheet.Cells[recordIndex, 6].Value = x.ImpressionQty;
                    workSheet.Cells[recordIndex, 7].Value = x.AccountsQty;
                    workSheet.Cells[recordIndex, 8].Value = x.StartDateOn;
                    workSheet.Cells[recordIndex, 9].Value = x.StartTime;
                    workSheet.Cells[recordIndex, 10].Value = x.EndDateOn;
                    workSheet.Cells[recordIndex, 11].Value = x.EndTime;
                    workSheet.Cells[recordIndex, 12].Value = x.Ins_StartDateOn;
                    workSheet.Cells[recordIndex, 13].Value = x.Ins_StartTime;
                    workSheet.Cells[recordIndex, 14].Value = x.Ins_EndDateOn;
                    workSheet.Cells[recordIndex, 15].Value = x.Ins_EndTime;
                    //workSheet.Cells[recordIndex, 16].Value = x.TimeTaken;
                    //workSheet.Cells[recordIndex, 17].Value = x.DatePostOn;
                    //workSheet.Cells[recordIndex, 18].Value = x.DatePostTime;
                    //workSheet.Cells[recordIndex, 19].Value = x.DateApproveOn;
                    //workSheet.Cells[recordIndex, 20].Value = x.DateApproveTime;
                    //workSheet.Cells[recordIndex, 21].Value = x.CreatedOn;
                    //workSheet.Cells[recordIndex, 22].Value = x.DateDeletionOnTxt;
                    //workSheet.Cells[recordIndex, 23].Value = x.PaperType;
                    //workSheet.Cells[recordIndex, 24].Value = x.JobType;
                    recordIndex++;
                }

                //foreach (var CLM in exportdailytracking)
                //{
                //    workSheet.Cells[recordIndex, 1].Value = CLM.Customer_Name;
                //    workSheet.Cells[recordIndex, 2].Value = CLM.ProductName;
                //    workSheet.Cells[recordIndex, 3].Value = CLM.LogTagNo;
                //    workSheet.Cells[recordIndex, 4].Value = CLM.PageQty;
                //    workSheet.Cells[recordIndex, 5].Value = CLM.ImpQty;
                //    workSheet.Cells[recordIndex, 6].Value = CLM.AccQty;
                //    workSheet.Cells[recordIndex, 7].Value = CLM.RevStrtDateOnTxt;
                //    workSheet.Cells[recordIndex, 8].Value = CLM.RevStrtTime;
                //    workSheet.Cells[recordIndex, 9].Value = CLM.StartDateOnTxt;
                //    workSheet.Cells[recordIndex, 10].Value = CLM.PostingDateOnTxt;
                //    workSheet.Cells[recordIndex, 11].Value = CLM.TotalAmountPaper;
                //    workSheet.Cells[recordIndex, 12].Value = CLM.TotalAmountEnv;
                //    workSheet.Cells[recordIndex, 13].Value = CLM.TotalAmountService;
                //    workSheet.Cells[recordIndex, 14].Value = CLM.TotalAmountPostage;
                //    workSheet.Cells[recordIndex, 15].Value = CLM.TotalAmountO;
                //    workSheet.Cells[recordIndex, 16].Value = CLM.TotalAmountF;
                //    workSheet.Cells[recordIndex, 17].Value = CLM.TotalAmountO2;
                //    workSheet.Cells[recordIndex, 18].Value = CLM.Sst;
                //    workSheet.Cells[recordIndex, 19].Value = CLM.TotalAmountO3;
                //    workSheet.Cells[recordIndex, 20].Value = CLM.TotalAmountx;


                //    recordIndex++;
                //}


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
            //workSheet.Column(16).AutoFit();
            //workSheet.Column(17).AutoFit();
            //workSheet.Column(18).AutoFit();
            //workSheet.Column(19).AutoFit();
            //workSheet.Column(20).AutoFit();
            //workSheet.Column(21).AutoFit();
            //workSheet.Column(22).AutoFit();
            //workSheet.Column(23).AutoFit();
            //workSheet.Column(24).AutoFit();

            var memoryStream = new MemoryStream();
            excel.SaveAs(memoryStream);

            memoryStream.Position = 0;

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate))
            {
                string filename = "Credit Card Report " + StartDate + " to " + EndDate + ".xlsx";
                return File(memoryStream, contentType, filename);


            }
            else
            {
                string filename = "Credit Card Report.xlsx";
                return File(memoryStream, contentType, filename);


            }
            //return File(filePath, contentType, downloadName);

        }



        public ActionResult ProduList(string Customer_Name)
        {
            Debug.WriteLine("Customer NAme Value : " + Customer_Name);
            String temp = "0";
            int _bildd = 1;
            List<string> ProdList = new List<string>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT DISTINCT ProductName FROM [dbo].[CustomerProduct]                          
                                      WHERE Customer_Name = @Customer_Name";
                command.Parameters.AddWithValue("@Customer_Name", Customer_Name.ToString());

                //
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        Debug.WriteLine("DB Result : " + reader.GetString(0));

                        int i = _bildd++;
                        if (i == 1)
                        {
                            ProdList.Add("Please Select");
                            ProdList.Add(reader.GetString(0));

                        }
                        else
                        {
                            ProdList.Add(reader.GetString(0));
                        }
                        ViewBag.ProdList = reader.GetString(0);
                        temp = reader.GetString(0);
                    }
                    else
                    {
                        ProdList.Add("Please Select");
                    }

                }



                foreach (var x in ProdList)
                {
                    Debug.WriteLine("ProdList : " + x);
                }

                cn.Close();
            }
            return Json(new { data = ProdList });
        }

        public ActionResult UpdateDailyTracking(string LogTagNo,string set, string CreateByIT,string CreatedOn, string DateApproveOn, string DateApproveTime, string DateDeletionOn, string DatePostOn, string DatePostTime, string EndDateOn, string EndTime,
            string ProcessEndDateOn, string ProcessEndTime, string ProcessStartDate, string ProcessStartTime, string StartDateOn, string StartTime, string TimeTaken,string ITSubmitOn, string ProcessStartDateOn)
        {

            Debug.WriteLine("Time Taken : " + TimeTaken);

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();

                SqlCommand command = new SqlCommand("SELECT FORMAT(MAX(A.CreatedOn), 'yyyy-MM-dd') as CreatedOn ,MAX(A.JobClass),MAX(A.Customer_Name),MAX(A.ProductName),A.LogTagNo,SUM(CAST(B.AccQty AS INT)) AS AccQty ,SUM(CAST(B.PageQty AS INT)) AS PageQty,SUM(CAST(B.ImpQty AS INT)) AS ImpQty,LEFT( CONVERT(varchar, MAX(A.StartDateOn), 120), 10)," +
                       "MAX(A.StartTime),LEFT( CONVERT(varchar, MAX(A.EndDateOn), 120), 10),MAX(A.EndTime),MAX(A.PIC),LEFT( CONVERT(varchar, MAX(A.ProcessStartDateOn), 120), 10),MAX(A.ProcessStartTime),LEFT( CONVERT(varchar, MAX(A.ProcessEndDateOn), 120), 10),MAX(A.ProcessEndTime),MAX(A.TimeTaken),MAX(A.DatePostOn),MAX(A.DatePostTime),LEFT( CONVERT(varchar, MAX(A.DateApproveOn), 120), 10),MAX(A.DateApproveTime)," +
                       "MAX(A.ItSubmitOn) as ItSubmitOn,MAX(A.DateDeletionOn),MAX(A.PaperType),MAX(B.JobType),MAX(A.PaperType),MAX(B.JobClass),MAX(B.CreatedOn) as CreatedOn2 FROM DailyTracking as A LEFT JOIN JobAuditTrailDetail AS B ON A.LogTagNo=B.LogTagNo WHERE A.LogTagNo=@LogTagNo GROUP BY A.LogTagNo ORDER BY MAX(A.CreatedOn) ASC", cn);
                command.Parameters.AddWithValue("@LogTagNo", LogTagNo);
                SqlDataReader rm = command.ExecuteReader();

                while (rm.Read())
                {
                    Debug.WriteLine("LogTagNo : " + rm.GetString(4));
                    List<string> PostingInformation = getPostingInfo(rm.GetString(4));

                    //ada 24 
                    if (rm.IsDBNull(0) == false)
                    {
                        ViewBag.ITSubmitOn = rm["CreatedOn"].ToString(); 
                    }

                    if (rm.IsDBNull(1) == false)
                    {
                        ViewBag.JobClass = rm.GetString(1);
                    }

                    if (rm.IsDBNull(2) == false)
                    {
                        ViewBag.Customer_Name = rm.GetString(2);
                    }

                    if (rm.IsDBNull(3) == false)
                    {
                        ViewBag.ProductName = rm.GetString(3);
                    }

                    if (rm.IsDBNull(4) == false)
                    {
                        ViewBag.LogTagNo = rm.GetString(4);
                        LogTagNo = rm.GetString(4);
                    }

                    if (rm.IsDBNull(5) == false)
                    {
                        ViewBag.AccQty = rm["AccQty"].ToString();
                    }

                    if (rm.IsDBNull(6) == false)
                    {
                        ViewBag.PageQty = rm["PageQty"].ToString();
                    }

                    if (rm.IsDBNull(7) == false)
                    {
                        ViewBag.ImpQty = rm["ImpQty"].ToString();
                    }

                    if (rm.IsDBNull(8) == false)
                    {
                        ViewBag.StartDateOn = rm.GetString(8);
                    }

                    if (rm.IsDBNull(9) == false)
                    {
                        ViewBag.StartTime = rm.GetString(9);
                    }
                    else
                    {
                        ViewBag.StartTime = "-";

                    }

                    if (rm.IsDBNull(10) == false)
                    {
                        ViewBag.EndDateOn = rm.GetString(10);
                    }

                    if (rm.IsDBNull(11) == false)
                    {
                        ViewBag.EndTime = rm.GetString(11);
                    }
                    else
                    {
                        ViewBag.EndTime = "-";

                    }

                    if (rm.IsDBNull(12) == false)
                    {
                        ViewBag.PIC = rm.GetString(12);
                    }

                    if (rm.IsDBNull(13) == false)
                    {
                        ViewBag.ProcessStartDateOn = rm.GetString(13);
                    }

                    if (rm.IsDBNull(14) == false)
                    {
                        ViewBag.ProcessStartTime = rm.GetString(14);
                        Debug.WriteLine("Process Start Time : " + rm.GetString(14));
                    }
                    else
                    {
                        ViewBag.ProcessStartTime = "-";

                    }

                    if (rm.IsDBNull(15) == false)
                    {
                        ViewBag.ProcessEndDateOn = rm.GetString(15);
                    }

                    if (rm.IsDBNull(16) == false)
                    {
                        ViewBag.ProcessEndTime = rm.GetString(16);
                        Debug.WriteLine("Process End Time : " + rm.GetString(16));

                    }
                    else
                    {
                        ViewBag.ProcessEndTime = "-";

                    }

                    if (rm.IsDBNull(17) == false)
                    {
                        ViewBag.TimeTaken = rm.GetString(17);
                    }

                    if (rm.IsDBNull(18) == false)
                    {
                        ViewBag.DatePostOnTxt = rm.GetString(18);
                    }

                    if (rm.IsDBNull(19) == false)
                    {
                        ViewBag.DatePostTime = rm.GetString(19);
                    }
                    else
                    {
                        ViewBag.DatePostTime = "-";
                    }

                    if (rm.IsDBNull(20) == false)
                    {
                        ViewBag.DateApproveOn = rm.GetString(20);
                    }
                    else
                    {

                        ViewBag.DateApproveOn = "-";

                    }

                    if (rm.IsDBNull(21) == false)
                    {
                        ViewBag.DateApproveTime = rm.GetString(21);
                    }
                    else
                    {

                        ViewBag.DateApproveTime = "-";

                    }

                    if (rm.IsDBNull(22) == false)
                    {
                        ViewBag.ItSubmitOnTxt = rm["ITSubmitOn"].ToString();
                    }

                    if (rm.IsDBNull(23) == false)
                    {
                        ViewBag.DateDeletionOn = rm.GetDateTime(23);
                    }

                    if (rm.IsDBNull(24) == false)
                    {
                        ViewBag.PaperType = rm.GetString(24);
                    }
                    else
                    {
                        ViewBag.PaperType = "-";
                    }

                    if (rm.IsDBNull(25) == false)
                    {
                        ViewBag.JobType = rm.GetString(25);
                    }
                    else
                    {
                        ViewBag.JobType = "-";
                    }

                    if (rm.IsDBNull(27) == false)
                    {
                        ViewBag.JobClass = rm.GetString(27);
                    }
                    if (rm.IsDBNull(28) == false)
                    {
                        // use CreateByIT to capture timestamp on CreatedOn column to save time
                        var TimeCreated = rm["CreatedOn2"].ToString().Split(' ');
                        var TimeCreated2 = TimeCreated[1].Substring(0, 5).Replace(":", "");
                        //ViewBag.ITSubmitTime = TimeCreated2;
                        ViewBag.ITSubmitTime = TimeCreated[1];

                    }

                    ViewBag.DatePostOn = getDatePost(rm.GetString(4));
                    ViewBag.DatePostTime = getTimePost(rm.GetString(4));
                    ViewBag.DateDeletionOnTxt = getDeletionDate(rm.GetString(4));
                    ViewBag.DatePostOnTxt = PostingInformation[0];
                    ViewBag.DatePostTime = PostingInformation[1];

                    if (PostingInformation[0] != "-")
                    {
                        DateTime date = DateTime.Parse(PostingInformation[0]);

                        DateTime newDate = date.AddDays(-2);

                        ViewBag.DateDeletionOnTxt = newDate.ToString();
                    }
                    else
                    {
                        ViewBag.DateDeletionOnTxt = "-";
                    }

                }

                cn.Close();
            }

            if(set=="UPDATE")
            {
                using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn2.Open();

                    SqlCommand cmd = new SqlCommand(@"UPDATE DailyTracking SET CreatedOn = @CreatedOn, ITSubmitOn=@ITSubmitOn, StartDateOn=@StartDateOn, StartTime=@StartTime, EndDateOn=@EndDateOn, EndTime=@EndTime,
                                                      ProcessStartDateOn=@ProcessStartDateOn, ProcessStartTime=@ProcessStartTime, ProcessEndDateOn=@ProcessEndDateOn, ProcessEndTime=@ProcessEndTime, DatePostOn=@DatePostOn, 
                                                      DatePostTime=@DatePostTime, DateDeletionOn=@DateDeletionOn, DateApproveOn=@DateApproveOn, DateApproveTime=@DateApproveTime, TimeTaken=@TimeTaken 
                                                      WHERE LogTagNo=@LogTagNo", cn2);

                    cmd.Parameters.AddWithValue("@LogTagNo", LogTagNo);


                    if (!string.IsNullOrEmpty(CreatedOn))
                    {
                        cmd.Parameters.AddWithValue("@CreatedOn", CreatedOn);

                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CreatedOn", DBNull.Value);

                    }

                    if (!string.IsNullOrEmpty(ITSubmitOn))
                    {
                        cmd.Parameters.AddWithValue("@ITSubmitOn", ITSubmitOn);
                       
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ITSubmitOn", DBNull.Value);

                    }
                    if (!string.IsNullOrEmpty(StartDateOn))
                    {
                        cmd.Parameters.AddWithValue("@StartDateOn", StartDateOn);
                       
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@StartDateOn", DBNull.Value);

                    }

                    if (!string.IsNullOrEmpty(StartTime))
                    {
                        cmd.Parameters.AddWithValue("@StartTime", StartTime);
                        
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@StartTime", DBNull.Value);

                    }

                    if (!string.IsNullOrEmpty(EndDateOn))
                    {
                        cmd.Parameters.AddWithValue("@EndDateOn", EndDateOn);
                       
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@EndDateOn", DBNull.Value);

                    }

                    if (!string.IsNullOrEmpty(EndTime))
                    {
                        cmd.Parameters.AddWithValue("@EndTime", EndTime);
                       
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@EndTime", DBNull.Value);

                    }

                    if (!string.IsNullOrEmpty(ProcessStartDateOn))
                    {
                        cmd.Parameters.AddWithValue("@ProcessStartDateOn", ProcessStartDateOn);
                       
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ProcessStartDateOn", DBNull.Value);

                    }

                    if (!string.IsNullOrEmpty(ProcessStartTime))
                    {
                        cmd.Parameters.AddWithValue("@ProcessStartTime", ProcessStartTime);
                       
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ProcessStartTime", DBNull.Value);

                    }

                    if (!string.IsNullOrEmpty(ProcessEndDateOn))
                    {
                        cmd.Parameters.AddWithValue("@ProcessEndDateOn", ProcessEndDateOn);
                       
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ProcessEndDateOn", DBNull.Value);

                    }

                    if (!string.IsNullOrEmpty(ProcessEndTime))
                    {
                        cmd.Parameters.AddWithValue("@ProcessEndTime", ProcessEndTime);
                       
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ProcessEndTime", DBNull.Value);

                    }

                    if (!string.IsNullOrEmpty(DatePostOn))
                    {
                        cmd.Parameters.AddWithValue("@DatePostOn", DatePostOn);
                       
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@DatePostOn", DBNull.Value);

                    }

                    if (!string.IsNullOrEmpty(DatePostTime))
                    {
                        cmd.Parameters.AddWithValue("@DatePostTime", DatePostTime);
                       
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@DatePostTime", DBNull.Value);

                    }

                    if (!string.IsNullOrEmpty(DateDeletionOn))
                    {
                        cmd.Parameters.AddWithValue("@DateDeletionOn", DateDeletionOn);
                        
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@DateDeletionOn", DBNull.Value);

                    }

                    if (!string.IsNullOrEmpty(DateApproveOn))
                    {
                        cmd.Parameters.AddWithValue("@DateApproveOn", DateApproveOn);
                      
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@DateApproveOn", DBNull.Value);

                    }

                    if (!string.IsNullOrEmpty(DateApproveTime))
                    {
                        cmd.Parameters.AddWithValue("@DateApproveTime", DateApproveTime);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@DateApproveTime", DBNull.Value);

                    }

                    if (!string.IsNullOrEmpty(TimeTaken))
                    {
                        cmd.Parameters.AddWithValue("@TimeTaken", TimeTaken);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@TimeTaken", DBNull.Value);

                    }



                    cmd.ExecuteNonQuery();

                    cn2.Close();

                    return RedirectToAction("UpdateDailyTracking", "Report", new { LogTagNo = LogTagNo });
                }
            }
                
            return View();
        }


        public ActionResult UpdateLogTagStatus(string LogTagNo)
        {
            JobAuditTrailDetail model = null;

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("SELECT DISTINCT LogTagNo, JobSheetNo, Customer_Name, ProductName, Status FROM JobAuditTrailDetail WHERE LogTagNo=@LogTagNo", cn);
                cmd.Parameters.AddWithValue("@LogTagNo", LogTagNo);

                SqlDataReader rm = cmd.ExecuteReader();

                if (rm.Read())
                {
                    model = new JobAuditTrailDetail
                    {
                        LogTagNo = rm["LogTagNo"].ToString(),
                        JobSheetNo = rm["JobSheetNo"].ToString(),
                        Customer_Name = rm["Customer_Name"].ToString(),
                        ProductName = rm["ProductName"].ToString(),
                        Status = rm["Status"].ToString()
                    };
                }

                rm.Close();
            }
            if (TempData.ContainsKey("Message"))
            {
                ViewBag.Message = TempData["Message"];
            }
            return View(model);

        }

        public ActionResult UpdateLogTagStatus2(string LogTagNo, String Status)
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE JobAuditTrailDetail SET Status=@Status WHERE LogTagNo=@LogTagNo", cn);
                cmd.Parameters.AddWithValue("@Status", Status);
                cmd.Parameters.AddWithValue("@LogTagNo", LogTagNo);

                cmd.ExecuteNonQuery();
            }
            TempData["Message"] = $"Status LogTag Successfully Updated to {Status}";
            return RedirectToAction("UpdateLogTagStatus", new { LogTagNo = LogTagNo });
        }



    }







}



