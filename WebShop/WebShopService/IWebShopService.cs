using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WebShopService
{
    [ServiceContract]
    public interface IWebShopService
    {
        [OperationContract]
        string GetName();

        [OperationContract]
        string GetProductInfo(int productId);

        [OperationContract]
        bool BuyProduct(int productId);

        [OperationContract]
        List<Product> GetProductList();

        [OperationContract]
        int RefreshProductStock(int productId);
    }

    [DataContract]
    public class Product
    {
        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public string Name { get; set; }

        public string Description { get; set; }

        [DataMember]
        public double Price { get; set; }

        [DataMember]
        public int Stock { get; set; }

        public bool OnSale { get; set; }

        public Product(int productId, string name, string description, double price, int stock, bool onSale)
        {
            ProductId = productId;
            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
            OnSale = onSale;
        }

        public override string ToString()
        {
            return Name + " (" + ProductId.ToString() + ")";
        }
    }
}
