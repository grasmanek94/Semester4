using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WebShopInterface;

namespace WebShopService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
                     ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class WebShopService : IWebShopService
    {
        enum SubscriptionModel
        {
            Default,
            NewOrders
        };

        enum Nothing
        {
            Nothing
        };

        static ConcurrentDictionary<int, Product> products;
        static int productIdCounter;
        static string name;
        static ConcurrentDictionary<IWebShopCallback, SubscriptionModel> callbacks;
        static ConcurrentDictionary<DateTime, Order> orders;

        IWebShopCallback current_callback;

        public WebShopService()
        {
            if (products == null)
            {
                products = new ConcurrentDictionary<int, Product>();
                callbacks = new ConcurrentDictionary<IWebShopCallback, SubscriptionModel>();
                orders = new ConcurrentDictionary<DateTime, Order>();

                productIdCounter = 1000000;
                name = "Linux Server Shop - Y2252";

                AddNewProduct("Speedup", "Detect and fix performance problems in your linux distribution", 34.99, -1, false);
                AddNewProduct("Virus Removal", "Removes all virusses from your linux distribution", 99.99, -1, false);
                AddNewProduct("Low-end Desktop", "i13-229300 8GB L8 Cache [32 physical cores] @ 4.33 GHz\r\n1TB Ram DDR55-5600\r\nuEthernet-1Tb/s\r\nFastNix 30.021", 299.99, 1000, false);
                AddNewProduct("Mid-range Desktop", "i15-88440 32GB L8 Cache [64 physical cores] @ 18.66 GHz\r\n16TB Ram DDR55-15000\r\nuEthernet-1Tb/s\r\nFastNix 30.021", 1299.99, 500, false);
                AddNewProduct("High-end Desktop", "i17-97780 128GB L8 Cache [256 physical cores] @ 64.01 GHz\r\n256TB Ram DDR55-37400\r\nuEthernet-1Tb/s\r\nFastNix 30.021", 5299.99, 33, false);
                AddNewProduct("Server", "uXeon-778X3 1TB L16 Cache [1024 physical cores] @ 88.74 GHz\r\n8192TB Ram DDR55-66600\r\n16x pEthernet-10Tb/s\r\nFastNixServer 12.844.15.5", 15799.99, 7, true);
            }
            current_callback = OperationContext.Current.GetCallbackChannel<IWebShopCallback>();

            callbacks.TryAdd(current_callback, 0);
        }

        public void CallbackStockChangeToAllClients(int productId, int stock)
        {
            foreach (var callback in callbacks)
            {
                if (current_callback != callback.Key)
                {
                    callback.Key.productStockChanged(productId, stock);
                }
            }
        }

        public void CallbackShipmentToAllClients(Order order)
        {
            if(callbacks.ContainsKey(order.Callback))
            {
                order.Callback.productShipped(order);
            }
        }

        public void CallbackOrderToAllSubscribers(Order order)
        {
            foreach (var callback in callbacks)
            {
                if (current_callback != callback.Key && callback.Value == SubscriptionModel.NewOrders)
                {
                    callback.Key.newOrder(order);
                }
            }
        }

        ~WebShopService()
        {
            SubscriptionModel x;
            callbacks.TryRemove(current_callback, out x);
        }

        public Product AddNewProduct(Product product)
        {
            Product newProduct = new Product(GetNewProductId(), product.Name, product.Description, product.Price, product.Stock, product.OnSale);
            products.TryAdd(newProduct.ProductId, newProduct);
            return newProduct;
        }

        public Product AddNewProduct(string name, string description, double price, int stock, bool onSale)
        {
            Product newProduct = new Product(GetNewProductId(), name, description, price, stock, onSale);
            products.TryAdd(newProduct.ProductId, newProduct);
            return newProduct;
        }

        private int GetNewProductId()
        {
            return System.Threading.Interlocked.Increment(ref productIdCounter);
        }

        public bool BuyProduct(int productId)
        {
            if (!products.ContainsKey(productId))
            {
                return false;
            }

            Product product = products[productId];
            if (product.Stock == 0)
            {
                return false;
            }

            Order order = new Order();
            order.Callback = current_callback;
            order.ProductId = product.ProductId;
            order.Name = product.Name;
            order.Moment = DateTime.Now;

            orders.TryAdd(order.Moment, order);
            CallbackOrderToAllSubscribers(order);

            if (product.Stock != -1)
            {
                int stock = --product.Stock;
                CallbackStockChangeToAllClients(product.ProductId, stock);
            }

            return true;
        }

        public void SubscribeToNewOrders()
        {
            callbacks[current_callback] = SubscriptionModel.NewOrders;
        }

        public List<Order> GetOrderList()
        {
            return orders.Values.ToList();
        }

        public string GetName()
        {
            return name;
        }

        public string GetProductInfo(int productId)
        {
            if (!products.ContainsKey(productId))
            {
                return null;
            }

            return products[productId].Description;
        }

        public List<Product> GetProductList()
        {
            return products.Values.ToList();
        }

        public int RefreshProductStock(int productId)
        {
            if (!products.ContainsKey(productId))
            {
                return 0;
            }

            return products[productId].Stock;
        }

        public bool ShipOrder(Order order)
        {
            if(!orders.ContainsKey(order.Moment))
            {
                return false;
            }

            Order x;
            orders.TryRemove(order.Moment, out x);
            CallbackShipmentToAllClients(x);

            return true;
        }
    }
}
