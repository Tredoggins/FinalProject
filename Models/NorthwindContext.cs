using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    class NorthwindContext : DbContext
    {
        public NorthwindContext() : base("name=NorthwindContext") { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public void AddProduct(Product product)
        {
            this.Products.Add(product);
            this.SaveChanges();
        }
        public void UpdateProduct(Product newProduct)
        {
            Product product = this.Products.Find(newProduct.ProductID);
            if (newProduct.ProductName.Length>0)
            {
                product.ProductName = newProduct.ProductName;
            }
            if (newProduct.SupplierID > 0)
            {
                product.SupplierID = newProduct.SupplierID;
            }
            if (newProduct.CategoryID > 0)
            {
                product.CategoryID = newProduct.CategoryID;
            }
            if (newProduct.QuantityPerUnit.Length>0)
            {
                product.QuantityPerUnit = newProduct.QuantityPerUnit;
            }
            if (newProduct.UnitPrice > 0)
            {
                product.UnitPrice = newProduct.UnitPrice;
            }
            if (newProduct.UnitsInStock > 0)
            {
                product.UnitsInStock = newProduct.UnitsInStock;
            }
            if (newProduct.UnitsOnOrder > 0)
            {
                product.UnitsOnOrder = newProduct.UnitsOnOrder;
            }
            if (newProduct.ReorderLevel > 0)
            {
                product.ReorderLevel = newProduct.ReorderLevel;
            }
            product.Discontinued = newProduct.Discontinued;
            this.SaveChanges();
        }
        public void RemoveProduct(int productID)
        {
            var product = this.Products.Find(productID);
            var ods = this.OrderDetails.Where(od => od.ProductID == productID);
            foreach(var od in ods)
            {
                var order = this.Orders.Find(od.OrderID);
                this.OrderDetails.Remove(od);
                if (order != null)
                {
                    this.Orders.Remove(order);
                }
            }
            this.Products.Remove(product);
            this.SaveChanges();
        }
        public void AddCategory(Category category)
        {
            this.Categories.Add(category);
            this.SaveChanges();
        }
        public void UpdateCategory(Category newCategory)
        {
            Category category = this.Categories.Find(newCategory.CategoryID);
            if (newCategory.CategoryName.Length>0)
            {
                category.CategoryName = newCategory.CategoryName;
            }
            if (newCategory.Description.Length>0)
            {
                category.Description = newCategory.Description;
            }
            this.SaveChanges();
        }
        public void RemoveCategory(int categoryID)
        {
            var products=this.Products.Where(p => p.CategoryID == categoryID);
            foreach(var p in products)
            {
                this.RemoveProduct(p.ProductID);
            }
            this.Categories.Remove(this.Categories.Find(categoryID));
            this.SaveChanges();
        }
    }
}
