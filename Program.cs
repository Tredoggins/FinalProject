using System;
using System.Collections.Generic;
using FinalProject.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    class Program
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Info("Program Started");
            try
            {
                string choice;
                var db = new NorthwindContext();
                do
                {
                    Console.WriteLine("Select:");
                    Console.WriteLine("1) Add Product");
                    Console.WriteLine("2) Edit Product");
                    Console.WriteLine("3) Display Product(s)");
                    Console.WriteLine("4) Add Category");
                    Console.WriteLine("5) Edit Category");
                    Console.WriteLine("6) Display Categories");
                    Console.WriteLine("7) Delete Product");
                    Console.WriteLine("8) Delete Category");
                    Console.WriteLine("Anything Else to Quit");
                    choice = Console.ReadLine();
                    Console.Clear();
                    logger.Info("User Choice: " + choice);
                    if (choice == "1")
                    {
                        Product product = new Product();
                        Console.WriteLine("Product Name: ");
                        product.ProductName = Console.ReadLine();
                        Console.WriteLine("Supplier ID: ");
                        product.SupplierID = int.Parse(Console.ReadLine());
                        Console.WriteLine("Category ID: ");
                        product.CategoryID = int.Parse(Console.ReadLine());
                        Console.WriteLine("Quantity Per Unit: ");
                        product.QuantityPerUnit = Console.ReadLine();
                        Console.WriteLine("Unit Price: $");
                        product.UnitPrice = decimal.Parse(Console.ReadLine());
                        Console.WriteLine("Units In Stock: ");
                        product.UnitsInStock = Int16.Parse(Console.ReadLine());
                        Console.WriteLine("Units On Order: ");
                        product.UnitsOnOrder = Int16.Parse(Console.ReadLine());
                        Console.WriteLine("Reorder Level: ");
                        product.ReorderLevel = Int16.Parse(Console.ReadLine());
                        Console.WriteLine("Discontinued(y/n): ");
                        string dis = Console.ReadLine();
                        if ((dis[0] + "").ToLower() == "y")
                        {
                            product.Discontinued = true;
                        }
                        else
                        {
                            product.Discontinued = false;
                        }
                        db.AddProduct(product);
                    }
                    else if(choice == "2")
                    {
                        Product product = new Product();
                        var query = db.Products.OrderBy(p => p.ProductID);
                        foreach(Product p in query)
                        {
                            Console.WriteLine(p.ProductID + " - " + p.ProductName);
                        }
                        try
                        {
                            Console.WriteLine("Product ID: ");
                            product.ProductID = int.Parse(Console.ReadLine());
                            Console.WriteLine("New Product Name: ");
                            product.ProductName = Console.ReadLine();
                            Console.WriteLine("New Supplier ID: ");
                            string user = Console.ReadLine();
                            product.SupplierID = (user.Length > 0 ? int.Parse(user) : -1);
                            Console.WriteLine("New Category ID: ");
                            user = Console.ReadLine();
                            product.CategoryID = (user.Length > 0 ? int.Parse(user) : -1);
                            Console.WriteLine("New Quantity Per Unit: ");
                            product.QuantityPerUnit = Console.ReadLine();
                            Console.WriteLine("New Unit Price: $");
                            user = Console.ReadLine();
                            product.UnitPrice = (user.Length > 0 ? decimal.Parse(user) : -1);
                            Console.WriteLine("Units In Stock: ");
                            user = Console.ReadLine();
                            product.UnitsInStock = (user.Length > 0 ? Int16.Parse(user) : Int16.Parse("-1"));
                            Console.WriteLine("New Units On Order: ");
                            user = Console.ReadLine();
                            product.UnitsOnOrder = (user.Length > 0 ? Int16.Parse(user) : Int16.Parse("-1"));
                            Console.WriteLine("New Reorder Level: ");
                            user = Console.ReadLine();
                            product.ReorderLevel = (user.Length > 0 ? Int16.Parse(user) : Int16.Parse("-1"));
                            Console.WriteLine("Discontinued(y/n): ");
                            string dis = Console.ReadLine();
                            if ((dis[0] + "").ToLower() == "y")
                            {
                                product.Discontinued = true;
                            }
                            else
                            {
                                product.Discontinued = false;
                            }
                            db.UpdateProduct(product);
                            logger.Info($"Updated Product {product.ProductName} - ID: {product.ProductID}");
                        }
                        catch
                        {
                            logger.Error("That was not a valid input");
                        }
                    }
                    else if(choice == "3")
                    {
                        string productDisplay = "0";
                        Console.WriteLine("Select:");
                        Console.WriteLine("1) Display All Products");
                        Console.WriteLine("2) Display Products Based On Criteria");
                        Console.WriteLine("Anything Else to Cancel");
                        productDisplay = Console.ReadLine();
                        Console.Clear();
                        logger.Info("User Entered " + productDisplay);
                        var query = db.Products.OrderBy(p => p.ProductName);
                        if(productDisplay == "2")
                        {
                            Console.WriteLine("Select Criteria:");
                            Console.WriteLine("1) ProductID");
                            Console.WriteLine("2) ProductName");
                            Console.WriteLine("3) SupplierID");
                            Console.WriteLine("4) CategoryID");
                            Console.WriteLine("5) UnitPrice");
                            Console.WriteLine("6) Discontinued");
                            Console.WriteLine("Anything Else to Cancel");
                            productDisplay = Console.ReadLine();
                            Console.Clear();
                            logger.Info("User Entered " + productDisplay);
                            if (productDisplay == "1")
                            {
                                Console.WriteLine("One Product ID to Display:");
                                List<int> ids=new List<int>();
                                string next="";
                                do
                                {
                                    Console.WriteLine("Enter s to Stop Entering IDs");
                                    next = Console.ReadLine();
                                    logger.Info("User Entered: " + next);
                                    try
                                    {
                                        int id = int.Parse(next);
                                        if(id>0&&id<=db.Products.Max(p => p.ProductID))
                                        {
                                            ids.Add(id);
                                            Console.WriteLine("Another ID:");
                                        }
                                        else
                                        {
                                            next = "s";
                                        }
                                    }
                                    catch
                                    {
                                        next = "s";
                                    }
                                } while (next!="s");
                                if (ids.Count > 0)
                                {
                                    query = db.Products.Where(p => p.ProductID == 0).OrderBy(p => p.ProductID);
                                    foreach (int id in ids)
                                    {
                                        query = query.Union(db.Products.Where(p => p.ProductID == id).OrderBy(p => p.ProductID)).OrderBy(p => p.ProductID);
                                    }
                                }
                                else
                                {
                                    productDisplay = "";
                                }
                            }
                            else if (productDisplay == "2")
                            {
                                Console.WriteLine("One Product Name to Display:");
                                List<string> names = new List<string>();
                                string next = "";
                                do
                                {
                                    Console.WriteLine("Enter s to Stop Entering Names");
                                    next = Console.ReadLine();
                                    logger.Info("User Entered: " + next);
                                    if (next != "s")
                                    {
                                        names.Add(next);
                                    }
                                    
                                } while (next!="s");
                                if (names.Count > 0)
                                {
                                    query = db.Products.Where(p => p.ProductName == "").OrderBy(p => p.ProductName);
                                    foreach (string name in names)
                                    {
                                        query = query.Union(db.Products.Where(p => p.ProductName == name).OrderBy(p => p.ProductName)).OrderBy(p => p.ProductName);
                                    }
                                }
                                else
                                {
                                    productDisplay = "";
                                }
                            }
                            else if (productDisplay == "3")
                            {
                                Console.WriteLine("One Product from a Supplier ID to Display:");
                                List<int> ids = new List<int>();
                                string next = "";
                                do
                                {
                                    Console.WriteLine("Enter s to Stop Entering IDs");
                                    next = Console.ReadLine();
                                    logger.Info("User Entered: " + next);
                                    try
                                    {
                                        int id = int.Parse(next);
                                        if (id > 0 && id <= db.Products.Max(p => p.ProductID))
                                        {
                                            ids.Add(id);
                                            Console.WriteLine("Another ID:");
                                        }
                                        else
                                        {
                                            next = "s";
                                        }
                                    }
                                    catch
                                    {
                                        next = "s";
                                    }
                                } while (next != "s");
                                if (ids.Count > 0)
                                {
                                    query = db.Products.Where(p => p.SupplierID == 0).OrderBy(p => p.SupplierID);
                                    foreach (int id in ids)
                                    {
                                        query = query.Union(db.Products.Where(p => p.SupplierID == id).OrderBy(p => p.SupplierID)).OrderBy(p => p.SupplierID);
                                    }
                                }
                                else
                                {
                                    productDisplay = "";
                                }
                            }
                            else if (productDisplay == "4")
                            {
                                Console.WriteLine("One Product from a Category ID to Display:");
                                List<int> ids = new List<int>();
                                string next = "";
                                do
                                {
                                    Console.WriteLine("Enter s to Stop Entering IDs");
                                    next = Console.ReadLine();
                                    logger.Info("User Entered: " + next);
                                    try
                                    {
                                        int id = int.Parse(next);
                                        if (id > 0 && id <= db.Products.Max(p => p.ProductID))
                                        {
                                            ids.Add(id);
                                            Console.WriteLine("Another ID:");
                                        }
                                        else
                                        {
                                            next = "s";
                                        }
                                    }
                                    catch
                                    {
                                        next = "s";
                                    }
                                } while (next != "s");
                                if (ids.Count > 0)
                                {
                                    query = db.Products.Where(p => p.CategoryID == 0).OrderBy(p => p.CategoryID);
                                    foreach (int id in ids)
                                    {
                                        query = query.Union(db.Products.Where(p => p.CategoryID == id).OrderBy(p => p.CategoryID)).OrderBy(p => p.CategoryID);
                                    }
                                }
                                else
                                {
                                    productDisplay = "";
                                }
                            }
                            else if (productDisplay == "5")
                            {
                                decimal min = 0;
                                decimal max = 0;
                                try
                                {
                                    Console.WriteLine("Minimum Unit Price: $");
                                    min = decimal.Parse(Console.ReadLine());
                                    try
                                    {
                                        Console.WriteLine("Maximum Unit Price: $");
                                        max = decimal.Parse(Console.ReadLine());
                                        query = db.Products.Where(p => p.UnitPrice >= min && p.UnitPrice <= max).OrderBy(p => p.UnitPrice);
                                    }
                                    catch
                                    {
                                        logger.Error("That is not a valid Maximum Unit Price");
                                        productDisplay = "";
                                    }
                                }
                                catch
                                {
                                    logger.Error("That is not a valid Minimum Unit Price");
                                    productDisplay = "";
                                }
                            }
                            else if (productDisplay == "6")
                            {
                                Console.WriteLine("Discontinued(y) or Not(n)");
                                Console.WriteLine("Anything else to cancel");
                                string dis = Console.ReadLine();
                                if ((dis[0] + "").ToLower() == "y")
                                {
                                    query = db.Products.Where(p => p.Discontinued).OrderBy(p => p.ProductID);
                                }
                                else if ((dis[0] + "").ToLower() == "n")
                                {
                                    query = db.Products.Where(p => !p.Discontinued).OrderBy(p => p.ProductID);
                                }
                                else
                                {
                                    productDisplay = "";
                                }
                            }
                            else
                            {
                                productDisplay = "";
                            }
                        }
                        if (productDisplay.Length > 0)
                        {
                            Console.WriteLine($"{query.Count()} Products Returned");
                            foreach (var item in query)
                            {
                                if (!item.Discontinued)
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                }
                                Console.WriteLine(item.ProductName);
                            }
                            Console.ResetColor();
                        }
                        else
                        {
                            logger.Info("Product Display Canceled");
                        }
                    }
                    else if(choice == "4")
                    {
                        Category category = new Category();
                        Console.WriteLine("Category Name: ");
                        category.CategoryName = Console.ReadLine();
                        Console.WriteLine("Category Description: ");
                        category.Description = Console.ReadLine();
                        db.AddCategory(category);
                    }
                    else if (choice == "5")
                    {
                        Category category = new Category();
                        var query = db.Categories.OrderBy(c => c.CategoryID);
                        foreach(Category c in query)
                        {
                            Console.WriteLine(c.CategoryID + " - " + c.CategoryName);
                        }
                        try
                        {
                            Console.WriteLine("Category ID: ");
                            category.CategoryID = int.Parse(Console.ReadLine());
                            Console.WriteLine("Category Name: ");
                            category.CategoryName = Console.ReadLine();
                            Console.WriteLine("Category Description: ");
                            category.Description = Console.ReadLine();
                            db.UpdateCategory(category);
                            logger.Info($"Updated Category {category.CategoryName} - ID: {category.CategoryID}");
                        }
                        catch
                        {
                            logger.Error("That is not a valid input");
                        }
                    }
                    else if (choice == "6")
                    {
                        string categoryDisplay = "0";
                        Console.WriteLine("1) Display All Categories");
                        Console.WriteLine("2) Display All Categories and Their Valid Products");
                        Console.WriteLine("3) Display Categories Based on Criteria");
                        Console.WriteLine("4) Display Categories Based on Criteria and Their Valid Products");
                        Console.WriteLine("Anything Else to Cancel");
                        categoryDisplay = Console.ReadLine();
                        Console.Clear();
                        logger.Info("User Entered " + categoryDisplay);
                        var query = db.Categories.OrderBy(c => c.CategoryName);
                        if (categoryDisplay == "1")
                        {
                            Console.WriteLine($"{query.Count()} Categories Returned");
                            foreach (Category c in query)
                            {
                                Console.WriteLine(c.CategoryName);
                                Console.WriteLine("    " + c.Description);
                            }
                        }
                        else if (categoryDisplay == "2")
                        {
                            Console.WriteLine($"{query.Count()} Categories Returned");
                            var newDB = new NorthwindContext();
                            var pQuery=newDB.Products.OrderBy(p => p.ProductName);
                            foreach (Category c in query)
                            {
                                Console.WriteLine(c.CategoryName);
                                pQuery = newDB.Products.Where(p => p.CategoryID == c.CategoryID).OrderBy(p => p.ProductName);
                                Console.WriteLine($"{pQuery.Count()} Products Returned");
                                foreach (Product p in pQuery)
                                {
                                    Console.WriteLine("    " + p.ProductName);
                                }
                            }
                        }
                        else if (categoryDisplay == "3")
                        {
                            Console.WriteLine("Select Which Criteria To Use:");
                            Console.WriteLine("1) Category ID");
                            Console.WriteLine("2) Category Name");
                            Console.WriteLine("Anything Else To Cancel");
                            categoryDisplay = Console.ReadLine();
                            Console.Clear();
                            logger.Info("User Entered " + categoryDisplay);
                            if (categoryDisplay == "1")
                            {
                                Console.WriteLine("One Category ID to Display:");
                                List<int> ids = new List<int>();
                                string next = "";
                                do
                                {
                                    Console.WriteLine("Enter s to Stop Entering IDs");
                                    next = Console.ReadLine();
                                    logger.Info("User Entered: " + next);
                                    try
                                    {
                                        int id = int.Parse(next);
                                        if (id > 0 && id <= db.Categories.Max(c => c.CategoryID))
                                        {
                                            ids.Add(id);
                                            Console.WriteLine("Another ID:");
                                        }
                                        else
                                        {
                                            next = "s";
                                        }
                                    }
                                    catch
                                    {
                                        next = "s";
                                    }
                                } while (next != "s");
                                if (ids.Count > 0)
                                {
                                    query = db.Categories.Where(c => c.CategoryID == 0).OrderBy(c => c.CategoryID);
                                    foreach (int id in ids)
                                    {
                                        query = query.Union(db.Categories.Where(c => c.CategoryID == id).OrderBy(c => c.CategoryID)).OrderBy(c => c.CategoryID);
                                    }
                                }
                                else
                                {
                                    categoryDisplay = "";
                                }
                            }
                            else if (categoryDisplay == "2")
                            {
                                Console.WriteLine("One Category Name to Display:");
                                List<string> names = new List<string>();
                                string next = "";
                                do
                                {
                                    Console.WriteLine("Enter s to Stop Entering IDs");
                                    next = Console.ReadLine();
                                    logger.Info("User Entered: " + next);
                                    names.Add(next);
                                } while (next != "s");
                                if (names.Count > 0)
                                {
                                    query = db.Categories.Where(c => c.CategoryName == "").OrderBy(c => c.CategoryName);
                                    foreach (string name in names)
                                    {
                                        query = query.Union(db.Categories.Where(c => c.CategoryName == name).OrderBy(c => c.CategoryName)).OrderBy(c => c.CategoryName);
                                    }
                                }
                                else
                                {
                                    categoryDisplay = "";
                                }
                            }
                            Console.WriteLine($"{query.Count()} Categories Returned");
                            foreach (Category c in query)
                            {
                                Console.WriteLine(c.CategoryName);
                                Console.WriteLine("    " + c.Description);
                            }
                        }
                        else if (categoryDisplay == "4")
                        {
                            Console.WriteLine("Select Which Criteria To Use:");
                            Console.WriteLine("1) Category ID");
                            Console.WriteLine("2) Category Name");
                            Console.WriteLine("Anything Else To Cancel");
                            categoryDisplay = Console.ReadLine();
                            Console.Clear();
                            logger.Info("User Entered " + categoryDisplay);
                            if (categoryDisplay == "1")
                            {
                                Console.WriteLine("One Category ID to Display:");
                                List<int> ids = new List<int>();
                                string next = "";
                                do
                                {
                                    Console.WriteLine("Enter s to Stop Entering IDs");
                                    next = Console.ReadLine();
                                    logger.Info("User Entered: " + next);
                                    try
                                    {
                                        int id = int.Parse(next);
                                        if (id > 0 && id <= db.Categories.Max(c => c.CategoryID))
                                        {
                                            ids.Add(id);
                                            Console.WriteLine("Another ID:");
                                        }
                                        else
                                        {
                                            next = "s";
                                        }
                                    }
                                    catch
                                    {
                                        next = "s";
                                    }
                                } while (next != "s");
                                if (ids.Count > 0)
                                {
                                    query = db.Categories.Where(c => c.CategoryID == 0).OrderBy(c => c.CategoryID);
                                    foreach (int id in ids)
                                    {
                                        query = query.Union(db.Categories.Where(c => c.CategoryID == id).OrderBy(c => c.CategoryID)).OrderBy(c => c.CategoryID);
                                    }
                                }
                                else
                                {
                                    categoryDisplay = "";
                                }
                            }
                            else if (categoryDisplay == "2")
                            {
                                Console.WriteLine("One Category Name to Display:");
                                List<string> names = new List<string>();
                                string next = "";
                                do
                                {
                                    Console.WriteLine("Enter s to Stop Entering IDs");
                                    next = Console.ReadLine();
                                    logger.Info("User Entered: " + next);
                                    names.Add(next);
                                } while (next != "s");
                                if (names.Count > 0)
                                {
                                    query = db.Categories.Where(c => c.CategoryName == "").OrderBy(c => c.CategoryName);
                                    foreach (string name in names)
                                    {
                                        query = query.Union(db.Categories.Where(c => c.CategoryName == name).OrderBy(c => c.CategoryName)).OrderBy(c => c.CategoryName);
                                    }
                                }
                                else
                                {
                                    categoryDisplay = "";
                                }
                            }
                            if (categoryDisplay.Length > 0)
                            {
                                Console.WriteLine($"{query.Count()} Categories Returned");
                                var newDB = new NorthwindContext();
                                var pQuery = newDB.Products.OrderBy(p => p.ProductName);
                                foreach (Category c in query)
                                {
                                    Console.WriteLine(c.CategoryName);
                                    pQuery = newDB.Products.Where(p => p.CategoryID == c.CategoryID).OrderBy(p => p.ProductName);
                                    Console.WriteLine($"{pQuery.Count()} Products Returned");
                                    foreach (Product p in pQuery)
                                    {
                                        if (!p.Discontinued)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Green;
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                        }
                                        Console.WriteLine("    " + p.ProductName);
                                    }
                                    Console.ResetColor();
                                }
                            }
                            else
                            {
                                logger.Info("Category Display Cancelled");
                            }
                        }
                    }
                    else if (choice == "7")
                    {
                        var products = db.Products.OrderBy(p => p.ProductID);
                        Console.WriteLine("Select Which Product To Delete");
                        foreach(var p in products)
                        {
                            Console.WriteLine(p.ProductID + " - " + p.ProductName);
                        }
                        Console.WriteLine("C to Cancel");
                        string delete = Console.ReadLine();
                        Console.Clear();
                        logger.Info("User Entered " + delete);
                        try
                        {
                            int id = int.Parse(delete);
                            if(id>0&&id<=products.Max(p => p.ProductID))
                            {
                                Product product = db.Products.Find(id);
                                if (product != null)
                                {
                                    Console.WriteLine($"Are You Sure You Want To Delete The Product ID: {id} Named {product.ProductName}?\nThis Will Delete It Forever, And Delete All Orders Associated With It(Y/N).");
                                    string answer = Console.ReadLine();
                                    Console.Clear();
                                    logger.Info("User Entered " + answer);
                                    if (answer.ToLower()[0] + "" == "y")
                                    {
                                        db.RemoveProduct(id);
                                        logger.Info($"Deleted Product ID: {id} Named {product.ProductName}");
                                    }
                                    else
                                    {
                                        logger.Info("Delete Cancelled");
                                    }
                                }
                                else
                                {
                                    logger.Error($"Product ID {id} Does Not Exist");
                                }
                            }
                            else
                            {
                                logger.Error($"Product ID {id} Does Not Exist");
                            }
                        }
                        catch
                        {
                            logger.Error($"{delete} Is not a valid Input");
                        }
                    }
                    else if (choice == "8")
                    {
                        var categories = db.Categories.OrderBy(c => c.CategoryID);
                        Console.WriteLine("Select Which Category To Delete");
                        foreach (var c in categories)
                        {
                            Console.WriteLine(c.CategoryID + " - " + c.CategoryName);
                        }
                        Console.WriteLine("C to Cancel");
                        string delete = Console.ReadLine();
                        Console.Clear();
                        logger.Info("User Entered " + delete);
                        try
                        {
                            int id = int.Parse(delete);
                            if (id > 0 && id <= categories.Max(c => c.CategoryID))
                            {
                                Category category = db.Categories.Find(id);
                                if (category != null)
                                {
                                    Console.WriteLine($"Are You Sure You Want To Delete The Category ID: {id} Named {category.CategoryName}?\nThis Will Delete It Forever, And Delete All Products Associated With It, As Well as Any Orders Associated With The Products Deleted(Y/N).");
                                    string answer = Console.ReadLine();
                                    Console.Clear();
                                    logger.Info("User Entered " + answer);
                                    if (answer.ToLower()[0] + "" == "y")
                                    {
                                        db.RemoveProduct(id);
                                        logger.Info($"Deleted Category ID: {id} Named {category.CategoryName}");
                                    }
                                    else
                                    {
                                        logger.Info("Delete Cancelled");
                                    }
                                }
                                else
                                {
                                    logger.Error($"Category ID {id} Does Not Exist");
                                }
                            }
                            else
                            {
                                logger.Error($"Category ID {id} Does Not Exist");
                            }
                        }
                        catch
                        {
                            logger.Error($"{delete} Is not a valid Input");
                        }
                    }
                } while (choice == "1"||choice == "2"||choice == "3"||choice == "4"||choice == "5"||choice == "6"||choice == "7"||choice == "8");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message+" "+ex.InnerException);
            }
            logger.Info("Program Ended");
        }
    }
}
