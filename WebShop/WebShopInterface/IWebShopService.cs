using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WebShopInterface
{
    [ServiceContract(CallbackContract = typeof(IWebShopCallback))]
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
        List<Order> GetOrderList();

        [OperationContract]
        int RefreshProductStock(int productId);

        [OperationContract]
        bool ShipOrder(Order order);

        [OperationContract]
        void SubscribeToNewOrders();
    }

    public interface IWebShopCallback
    {
        [OperationContract(IsOneWay = true)]
        void productShipped(Order order);

        [OperationContract(IsOneWay = true)]
        void productStockChanged(int productId, int stock);

        [OperationContract(IsOneWay = true)]
        void newOrder(Order order);
    }

    [DataContract]
    public class Order
    {
        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public DateTime Moment { get; set; }

        public IWebShopCallback Callback { get; set; }

        public override string ToString()
        {
            return Name + " (" + ProductId.ToString() + ") @ " + Moment.ToString();
        }
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
