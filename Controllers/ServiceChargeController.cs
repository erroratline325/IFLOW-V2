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


public class ServiceChargeController : Controller
{

    public ActionResult ManageMaterialTypes(string MaterialType, string set, string Id)
    {
        if (set == "delete")
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"DELETE [IflowSeed].[dbo].[MaterialCharges]                          
                                      WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", Id.ToString());
                command.ExecuteNonQuery();
                cn.Close();

            }
        }

        int _bil = 1;
        List<SelectListItem> li = new List<SelectListItem>();

        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();

            command.CommandText = @"select distinct(MaterialType) from [IflowSeed].[dbo].[MaterialCharges] 
                                    WHERE MaterialType IS NOT NULL AND MaterialType != ' '
                                    ORDER BY MaterialType Desc";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                MaterialCharges model = new MaterialCharges();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.MaterialType = reader.GetString(0);
                    }
                }
                int i = _bil++;

                if (i == 1)
                {
                    li.Add(new SelectListItem { Text = "Please Select" });
                }

                li.Add(new SelectListItem { Text = model.MaterialType });

            }
            cn.Close();

        }

        ViewData["MaterialType"] = li;
        ViewBag.Display = 1;


        if (!string.IsNullOrEmpty(MaterialType))
        {
            //Session["BNO"] = BatchNumber.ToString();                
            List<MaterialCharges> ListOfData = new List<MaterialCharges>();


            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                try
                {
                    cn.Open();


                    SqlCommand cmd = new SqlCommand("select *  from [IflowSeed].[dbo].[MaterialCharges] where MaterialType=@MaterialType AND ItemCode IS NOT NULL AND ItemCode != ' ' order by MaterialType asc ", cn);
                    cmd.Parameters.Add(new SqlParameter("@MaterialType", MaterialType.ToString()));   // Session["ViewBatchNumber"].ToString()
                    SqlDataReader rs = cmd.ExecuteReader();
                    if (rs.HasRows)
                    {
                        while (rs.Read())
                        {
                            MaterialCharges list = new MaterialCharges();
                            {
                                list.id = rs["Id"].ToString();
                                list.ItemCode = rs["ItemCode"].ToString();
                                list.MaterialType = rs["MaterialType"].ToString();
                                list.Description = rs["Description"].ToString();
                                list.Rate = rs["Rate"].ToString();
                                list.ProductType = rs["ProductType"].ToString();
                            }
                            ListOfData.Add(list);
                        }

                        // return RedirectToAction("ViewReportCycle", "Home");
                        ViewBag.Display = 2;


                    }
                }
                catch (System.Exception err)
                {
                    //TempData["msg"] = "<script>alert('" + err.ToString() + "');</script>";
                    Response.Write("<script > alert('" + err.ToString() + "');</ script >");
                }
                finally
                {
                    cn.Close();
                }
            }

            return View(ListOfData);
        }
        else
        {
            return View();
        }


    }
    
    public ActionResult AddMaterialType(string Id, string ItemCode, string MaterialType, string Rate, string Description, string CreatedBy, string set, string ProductType)
    {

        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];

        if(!string.IsNullOrEmpty(MaterialType))
        {
            ViewBag.MaterialType = MaterialType;
        }

        ViewBag.IsDepart = @Session["Department"];
        ViewBag.AccountManager = IdentityName.ToString();

        if (string.IsNullOrEmpty(Id))
        {
            ViewBag.DataSet = "Save";
        }
        else
        {
            ViewBag.DataSet = "update";
        }

        int _bildd = 1;
        List<SelectListItem> li2 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT MaterialType FROM [IflowSeed].[dbo].[MaterialCharges]
                                   WHERE MaterialType IS NOT NULL AND MaterialType != ' '
                                   ORDER BY MaterialType";
            var reader = command.ExecuteReader();
           

            while (reader.Read())
            {
                MaterialCharges model = new MaterialCharges();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.MaterialType = reader.GetString(0);
                    }
                }
                int i = _bildd++;

                if (i == 1)
                {
                    li2.Add(new SelectListItem { Text = "Please Select" });
                }

                li2.Add(new SelectListItem { Text = model.MaterialType });

            }
            cn.Close();
        }
        ViewData["MaterialType_"] = li2;

        //
        int _bil = 1;
        List<SelectListItem> li3 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT ProductType FROM [IflowSeed].[dbo].[MaterialCharges]
                                    WHERE ProductType IS NOT NULL AND ProductType !=  ' '
                                    ORDER BY ProductType";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                MaterialCharges model = new MaterialCharges();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.ProductType = reader.GetString(0);
                    }
                }
                int i = _bil++;
                if (i == 1)
                {
                    li3.Add(new SelectListItem { Text = "Please Select" });
                    li3.Add(new SelectListItem { Text = model.ProductType });

                }
                else
                {
                    li3.Add(new SelectListItem { Text = model.ProductType });
                }
            }
            cn.Close();
        }
        ViewData["ProductType_"] = li3;
        //

        if (set == "AddNew" || set == "AddNew2")
        {
            if (string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(MaterialType) || !string.IsNullOrEmpty(ProductType))
            {
                if (set == "AddNew2")
                {
                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        Guid Idx = Guid.NewGuid();
                        string CreatedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                        cn.Open();
                        SqlCommand command;
                        command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[MaterialCharges] (Id,CreatedOn,MaterialType,CreatedBy,ProductType) values (@Id,@CreatedOn,@MaterialType,@CreatedBy,@ProductType)", cn);
                        command.Parameters.AddWithValue("@Id", Idx);
                        command.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                        if (MaterialType == null)
                        {
                            command.Parameters.Add(new SqlParameter { ParameterName = "@MaterialType", Value = DBNull.Value });
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@MaterialType", MaterialType);
                        }

                        command.Parameters.AddWithValue("@CreatedBy", IdentityName.ToString());

                        if (ProductType == null)
                        {
                            command.Parameters.Add(new SqlParameter { ParameterName = "@ProductType", Value = DBNull.Value });
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@ProductType", ProductType);
                        }

                        command.ExecuteNonQuery();
                        cn.Close();




                        return RedirectToAction("AddMaterialType", "ServiceCharge");

                    }
                }
                if (set != "AddNew2")
                {
                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        Guid Idx = Guid.NewGuid();
                        string CreatedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                        cn.Open();
                        SqlCommand command;
                        command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[MaterialCharges] (Id,CreatedOn,ItemCode,MaterialType,Description,CreatedBy,Rate,ProductType) values (@Id,@CreatedOn,@ItemCode,@MaterialType,@Description,@CreatedBy,@Rate,@ProductType)", cn);
                        command.Parameters.AddWithValue("@Id", Idx);
                        command.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                        command.Parameters.AddWithValue("@ItemCode", ItemCode);
                        command.Parameters.AddWithValue("@MaterialType", MaterialType);
                        command.Parameters.AddWithValue("@Description", Description);
                        command.Parameters.AddWithValue("@CreatedBy", IdentityName.ToString());
                        command.Parameters.AddWithValue("@Rate", Rate);
                        command.Parameters.AddWithValue("@ProductType", ProductType);
                        command.ExecuteNonQuery();
                        cn.Close();




                        return RedirectToAction("AddMaterialType", "ServiceCharge");

                    }
                }

            }

            if (!string.IsNullOrEmpty(Id) && set == "update")
            {
                //update
                string ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn.Open();
                    SqlCommand command;
                    command = new SqlCommand("UPDATE [IflowSeed].[dbo].[MaterialCharges]  SET ModifiedOn=@ModifiedOn, ItemCode=@ItemCode, MaterialType=@MaterialType, Rate=@Rate, Description=@Description WHERE Id=@Id, ProductType = @ProductType", cn);
                    command.Parameters.AddWithValue("@ModifiedOn", ModifiedOn);
                    command.Parameters.AddWithValue("@ItemCode", ItemCode);
                    command.Parameters.AddWithValue("@MaterialType", MaterialType);
                    command.Parameters.AddWithValue("@Rate", Rate);
                    command.Parameters.AddWithValue("@Description", Description);
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@ProductType", ProductType);
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
                    command.CommandText = @"SELECT Id,ItemCode,MaterialType,Rate,Description,ProductType
                                       FROM [IflowSeed].[dbo].[MaterialCharges]                              
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
                            ViewBag.ItemCode = reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            ViewBag.MaterialType = reader.GetString(2);
                        }
                        if (reader.IsDBNull(3) == false)
                        {
                            ViewBag.Rate = reader.GetString(3);
                        }
                        if (reader.IsDBNull(4) == false)
                        {
                            ViewBag.Description = reader.GetString(4);
                        }
                        if (reader.IsDBNull(5) == false)
                        {
                            ViewBag.ProductType = reader.GetString(5);
                        }

                    }
                    cn.Close();
                }
            }


        }







        if (string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(MaterialType) && !string.IsNullOrEmpty(Description))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                Guid Idx = Guid.NewGuid();
                string CreatedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[MaterialCharges] (Id,CreatedOn,MaterialType,Description,CreatedBy) values (@Id,@CreatedOn,@MaterialType,@Description,@CreatedBy)", cn);
                command.Parameters.AddWithValue("@Id", Idx);
                command.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                command.Parameters.AddWithValue("@MaterialType", MaterialType);
                command.Parameters.AddWithValue("@Description", Description);
                command.Parameters.AddWithValue("@CreatedBy", IdentityName.ToString());
                command.ExecuteNonQuery();
                cn.Close();
            }

        }

        if (!string.IsNullOrEmpty(Id) && set == "update")
        {
            //update
            string ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("UPDATE [IflowSeed].[dbo].[MaterialCharges]  SET ModifiedOn=@ModifiedOn, MaterialType=@MaterialType, Description=@Description WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@ModifiedOn", ModifiedOn);
                command.Parameters.AddWithValue("@MaterialType", MaterialType);
                command.Parameters.AddWithValue("@Description", Description);
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
                command.CommandText = @"SELECT Id,MaterialType,Description,Rate,ItemCode
                                       FROM [IflowSeed].[dbo].[MaterialCharges]                              
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
                        ViewBag.MaterialType = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        ViewBag.Description = reader.GetString(2);
                    }
                    if (reader.IsDBNull(3) == false)
                    {
                        ViewBag.Rate = reader.GetString(3);
                    }
                    if (reader.IsDBNull(4) == false)
                    {
                        ViewBag.ItemCode = reader.GetString(4);
                    }

                }
                cn.Close();
            }
        }

        return View();
    }


    List<ChargesToCustomer> CustomerList = new List<ChargesToCustomer>();
    public ActionResult ManageCustomerList(string Id, string ProductName, string product, string set, string Status)
    {
        if (set == "search") //ini kalu user search product
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT Id, Customer_Name, ProductName, CreatedBy
                                         FROM [IflowSeed].[dbo].[ChargesToCustomer]                                    
                                         WHERE ProductName LIKE @ProductName
                                         ORDER BY CreatedOn desc ";
                command.Parameters.AddWithValue("@ProductName", "%" + product + "%");
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ChargesToCustomer model = new ChargesToCustomer();
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
                            model.CreatedBy = reader.GetString(3);
                        }

                    }
                    CustomerList.Add(model);
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
                command.CommandText = @"SELECT Id, Customer_Name, ProductName,CreatedBy
                                        FROM [IflowSeed].[dbo].[ChargesToCustomer]";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ChargesToCustomer model = new ChargesToCustomer();
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
                            model.CreatedBy = reader.GetString(3);
                        }


                    }
                    CustomerList.Add(model);
                }
                cn.Close();
            }
        }

       
        return View(CustomerList); //hntr data ke ui

    }




    public ActionResult SelectCustomer(string Id, string Customer_Name, string ProductName,string set)
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
            command.CommandText = @"SELECT DISTINCT Customer_Name FROM [IflowSeed].[dbo].[JobInstruction]                          
                                     ORDER BY Customer_Name";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                JobInstruction model = new JobInstruction();
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
            List<SelectListItem> li3 = new List<SelectListItem>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();
                command.CommandText = @"SELECT DISTINCT ProductName FROM [IflowSeed].[dbo].[JobInstruction]    
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


        ViewBag.Customer_Name = Customer_Name;
        ViewBag.Id = Id;

        if (string.IsNullOrEmpty(Id))
        {
            ViewBag.DataSet = "Save";
        }
        else
        {
            ViewBag.DataSet = "update";
        }

        if (string.IsNullOrEmpty(Id) && Customer_Name != "Please Select" && ProductName != "Please Select" && !string.IsNullOrEmpty(Customer_Name) && !string.IsNullOrEmpty(ProductName))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                Guid Idx = Guid.NewGuid();
                string createdOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");

                cn.Open();
                SqlCommand command;
                command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[ChargesToCustomer] (Id, CreatedOn, Customer_Name,ProductName,CreatedBy) values (@Id, @CreatedOn,@Customer_Name,@ProductName,@CreatedBy)", cn);
                command.Parameters.AddWithValue("@Id", Idx);
                command.Parameters.AddWithValue("@CreatedOn", createdOn);
                command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                command.Parameters.AddWithValue("@ProductName", ProductName);
                command.Parameters.AddWithValue("@CreatedBy", IdentityName.ToString());
                command.ExecuteNonQuery();
                cn.Close();
            }
            //bila save akn gi kt managecustomer
            return RedirectToAction("ManageCustomerList", "ServiceCharge");
        }

        if (!string.IsNullOrEmpty(Id) && set == "update")
        {
            //update
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("UPDATE [IflowSeed].[dbo].[ChargesToCustomer]  SET Customer_Name=@Customer_Name,ProductName=@ProductName WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@Customer_Name", Customer_Name);
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
                command.CommandText = @"SELECT Id, Customer_Name, ProductName
                                       FROM [IflowSeed].[dbo].[ChargesToCustomer]                              
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

                }
                cn.Close();
            }
        }



        return View();
    }

    public ActionResult DeleteCustomerList(string Id)
    {
        if (!string.IsNullOrEmpty(Id))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("DELETE [IflowSeed].[dbo].[ChargesToCustomer] WHERE Id=@Id", cn);
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                cn.Close();
            }
        }
        return RedirectToAction("ManageCustomerList", "ServiceCharge");
    }

    List<MaterialChargess> MasterChecklist = new List<MaterialChargess>();
    [ValidateInput(false)]
    public ActionResult SelectedItemCharges(string Id, string Customer_Name, string ProductName, string MaterialType, string set)
    {
        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];
        ViewBag.IsDepart = @Session["Department"];
        Session["JobInstructionId"] = Id;
        Session["Id"] = Id;
        Session["Customer_Name"] = Customer_Name;
        Session["ProductName"] = ProductName;
        ViewBag.Customer_Name = Customer_Name;
        ViewBag.ProductName = ProductName;
        ViewBag.Id = Id;


         
        if (!string.IsNullOrEmpty(MaterialType))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                int _bil = 1;
                cn.Open();
                command.CommandText = @"SELECT DISTINCT MaterialType, Description
                                        FROM  [IflowSeed].[dbo].[MaterialCharges] 
                                        WHERE MaterialType LIKE @MaterialType_";
                command.Parameters.AddWithValue("@MaterialType_", MaterialType.ToString());
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    MaterialChargess models = new MaterialChargess();
                    {
                        models.Bil = _bil++;

                        if (reader.IsDBNull(0) == false)
                        {
                            models.MaterialType = reader.GetString(0);
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            models.Description = reader.GetString(1);
                        }


                    }
                    MasterChecklist.Add(models);
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
                command.CommandText = @"SELECT DISTINCT MaterialType, Description
                                        FROM  [IflowSeed].[dbo].[MaterialCharges] 
                                        ORDER BY MaterialType";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    MaterialChargess model = new MaterialChargess();
                    {
                        model.Bil = _bil++;

                        if (reader.IsDBNull(0) == false)
                        {
                            model.MaterialType = reader.GetString(0);
                        }
                        if (reader.IsDBNull(1) == false)
                        {
                            model.Description = reader.GetString(1);
                        }


                    }
                    MasterChecklist.Add(model);
                }
                cn.Close();
                return View(MasterChecklist);

            }
        }



        if (set == "ProfileJI")
        {

        }
        else if (set == "DataProcess")
        {

        }
        else if (set == "MaterialInfo")
        {
        }
        else if (set == "ProductionList")
        {
          
            
        }
        else if (set == "FinishingInst")
        {
           
        }
        else if (set == "ImportantNotes")
        {
           
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

                  

                }
                cn.Close();
            }

        }




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
        }

        return View();

    }

    public ActionResult ManageTermsAndCondition(string ProductTerm)
    {
        {
            int _bil = 1;
            List<SelectListItem> li = new List<SelectListItem>();

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (SqlCommand command = new SqlCommand("", cn))
            {
                cn.Open();

                command.CommandText = @"SELECT DISTINCT (ProductTerm) from [IflowSeed].[dbo].[QouStoreTerm] 
                                        WHERE ProductTerm IS NOT NULL AND ProductTerm != ' '
                                        ORDER BY ProductTerm Desc";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TermsAndCondition model = new TermsAndCondition();
                    {
                        if (reader.IsDBNull(0) == false)
                        {
                            model.ProductTerm = reader.GetString(0);
                        }
                    }
                    int i = _bil++;

                    if (i == 1)
                    {
                        li.Add(new SelectListItem { Text = "Please Select" });
                    }

                    li.Add(new SelectListItem { Text = model.ProductTerm });

                }
                cn.Close();

            }

            ViewData["ProductTerm_"] = li;
            ViewBag.Display = 1;


            if (!string.IsNullOrEmpty(ProductTerm))
            {
                //Session["BNO"] = BatchNumber.ToString();                
                List<TermsAndCondition> ListOfData = new List<TermsAndCondition>();


                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    try
                    {
                        cn.Open();


                        SqlCommand cmd = new SqlCommand("select *  from [IflowSeed].[dbo].[QouStoreTerm] WHERE ProductTerm=@ProductTerm AND Cust_Term IS NOT NULL AND Cust_Term != ' ' ORDER  by ProductTerm asc ", cn);
                        cmd.Parameters.Add(new SqlParameter("@ProductTerm", ProductTerm.ToString()));
                        SqlDataReader rs = cmd.ExecuteReader();
                        if (rs.HasRows)
                        {
                            while (rs.Read())
                            {
                                TermsAndCondition list = new TermsAndCondition();
                                {
                                    list.id = rs["gID"].ToString();
                                    list.ProductTerm = rs["ProductTerm"].ToString();
                                    list.Cust_Term = rs["Cust_Term"].ToString();

                                }
                                ListOfData.Add(list);
                            }

                            // return RedirectToAction("ViewReportCycle", "Home");
                            ViewBag.Display = 2;


                        }
                    }
                    catch (System.Exception err)
                    {
                        //TempData["msg"] = "<script>alert('" + err.ToString() + "');</script>";
                        Response.Write("<script > alert('" + err.ToString() + "');</ script >");
                    }
                    finally
                    {
                        cn.Close();
                    }
                }

                return View(ListOfData);
            }
            else
            {
                return View();
            }


        }
    }

    public ActionResult AddTermsAndCondition(string Id, string set, string ProductTerm, string Cust_Term, string CreatedBy)
    {
        var IdentityName = @Session["Fullname"];
        var Role = @Session["Role"];


        ViewBag.IsDepart = @Session["Department"];
        ViewBag.AccountManager = IdentityName.ToString();

        if (string.IsNullOrEmpty(Id))
        {
            ViewBag.DataSet = "Save";
        }
        else
        {
            ViewBag.DataSet = "update";
        }

        int _bildd = 1;
        List<SelectListItem> li2 = new List<SelectListItem>();
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        using (SqlCommand command = new SqlCommand("", cn))
        {
            cn.Open();
            command.CommandText = @"SELECT DISTINCT ProductTerm FROM [IflowSeed].[dbo].[QouStoreTerm]  
                                     WHERE ProductTerm IS NOT NULL AND ProductTerm != ' '
                                     ORDER BY ProductTerm";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                TermsAndCondition model = new TermsAndCondition();
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        model.ProductTerm = reader.GetString(0);
                    }
                }
                int i = _bildd++;
                if (i == 1)
                {
                    li2.Add(new SelectListItem { Text = "Please Select" });
                    li2.Add(new SelectListItem { Text = model.ProductTerm });

                }
                else
                {
                    li2.Add(new SelectListItem { Text = model.ProductTerm });
                }
            }
            cn.Close();
        }
        ViewData["ProductTerm_"] = li2;

        if (set == "AddNew" || set == "AddNew2")
        {
            if (string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(ProductTerm))
            {
                if (set == "AddNew")
                {
                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        Guid Idx = Guid.NewGuid();
                        string CreatedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                        cn.Open();
                        SqlCommand command;
                        command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[QouStoreTerm] (gID,CreatedOn,ProductTerm,Cust_Term,CreatedBy) values (@gID,@CreatedOn,@ProductTerm,@Cust_Term,@CreatedBy)", cn);
                        command.Parameters.AddWithValue("@gID", Idx);
                        command.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                        command.Parameters.AddWithValue("@ProductTerm", ProductTerm);
                        command.Parameters.AddWithValue("@Cust_Term", Cust_Term);
                        command.Parameters.AddWithValue("@CreatedBy", IdentityName.ToString());
                        command.ExecuteNonQuery();
                        cn.Close();




                        return RedirectToAction("AddTermsAndCondition", "ServiceCharge");

                    }
                }
                if (set == "AddNew2")
                {
                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        Guid Idx = Guid.NewGuid();
                        string CreatedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                        cn.Open();
                        SqlCommand command;
                        command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[QouStoreTerm] (gID,CreatedOn,ProductTerm,CreatedBy) values (@gID,@CreatedOn,@ProductTerm,@CreatedBy)", cn);
                        command.Parameters.AddWithValue("@gID", Idx);
                        command.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                        command.Parameters.AddWithValue("@ProductTerm", ProductTerm);
                        command.Parameters.AddWithValue("@CreatedBy", IdentityName.ToString());
                        command.ExecuteNonQuery();
                        cn.Close();




                        return RedirectToAction("AddTermsAndCondition", "ServiceCharge");

                    }
                }

            }

            if (!string.IsNullOrEmpty(Id) && set == "update")
            {
                //update
                string ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    cn.Open();
                    SqlCommand command;
                    command = new SqlCommand("UPDATE [IflowSeed].[dbo].[QouStoreTerm]  SET ModifiedOn=@ModifiedOn, ProductTerm=@ProductTerm, Cust_Term=@Cust_Term WHERE Id=@Id", cn);
                    command.Parameters.AddWithValue("@ModifiedOn", ModifiedOn);
                    command.Parameters.AddWithValue("@ProductTerm", ProductTerm);
                    command.Parameters.AddWithValue("@Cust_Term", Cust_Term);
                    command.Parameters.AddWithValue("@gID", Id);
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
                    command.CommandText = @"SELECT Id,ProductTerm,Cust_Term
                                       FROM [IflowSeed].[dbo].[QouStoreTerm]                              
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
                            ViewBag.ProductTerm = reader.GetString(1);
                        }
                        if (reader.IsDBNull(2) == false)
                        {
                            ViewBag.Cust_Term = reader.GetString(2);
                        }
                    }
                    cn.Close();
                }
            }


        }

        if (string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(ProductTerm) && !string.IsNullOrEmpty(Cust_Term))
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                Guid Idx = Guid.NewGuid();
                string CreatedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("INSERT INTO [IflowSeed].[dbo].[QouStoreTerm] (gID,CreatedOn,ProductTerm,Cust_Term,CreatedBy) values (@gID,@CreatedOn,@ProductTerm,@Cust_Term,@CreatedBy)", cn);
                command.Parameters.AddWithValue("@gID", Idx);
                command.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                command.Parameters.AddWithValue("@ProductTerm", ProductTerm);
                command.Parameters.AddWithValue("@Cust_Term", Cust_Term);
                command.Parameters.AddWithValue("@CreatedBy", IdentityName.ToString());
                command.ExecuteNonQuery();
                cn.Close();
            }

        }

        if (!string.IsNullOrEmpty(Id) && set == "update")
        {
            //update
            string ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                cn.Open();
                SqlCommand command;
                command = new SqlCommand("UPDATE [IflowSeed].[dbo].[QouStoreTerm]  SET ModifiedOn=@ModifiedOn, ProductTerm=@ProductTerm, Cust_Term=@Cust_Term WHERE gID=@gID", cn);
                command.Parameters.AddWithValue("@ModifiedOn", ModifiedOn);
                command.Parameters.AddWithValue("@ProductTerm", ProductTerm);
                command.Parameters.AddWithValue("@Cust_Term", Cust_Term);
                command.Parameters.AddWithValue("@gID", Id);
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
                command.CommandText = @"SELECT gID,ProductTerm,Cust_Term
                                       FROM [IflowSeed].[dbo].[QouStoreTerm]                              
                                     WHERE gID=@gID";
                command.Parameters.AddWithValue("@gID", Id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ViewBag.gID = Id;

                    if (reader.IsDBNull(1) == false)
                    {
                        ViewBag.ProductTerm = reader.GetString(1);
                    }
                    if (reader.IsDBNull(2) == false)
                    {
                        ViewBag.Cust_Term = reader.GetString(2);
                    }

                }
                cn.Close();
            }
        }

        return View();
    }



}

