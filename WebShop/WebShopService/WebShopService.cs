using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WebShopService
{
    public class WebShopService : IWebShopService
    {
        static Dictionary<int, Product> products;
        static int productIdCounter;
        static string name;

        public WebShopService()
        {
            if (products == null)
            {
                products = new Dictionary<int, Product>();
                productIdCounter = 1000000;
                name = "Linux Server Shop - Y2252";

                AddNewProduct("Speedup", "Detect and fix performance problems in your linux distribution", 34.99, -1, false);
                AddNewProduct("Virus Removal", "Removes all virusses from your linux distribution", 99.99, -1, false);
                AddNewProduct("Low-end Desktop", "i13-229300 8GB L8 Cache [32 physical cores] @ 4.33 GHz | 1TB Ram DDR55-5600 | uEthernet-1Tb/s | FastNix 30.021", 299.99, 1000, false);
                AddNewProduct("Mid-range Desktop", "i15-88440 32GB L8 Cache [64 physical cores] @ 18.66 GHz | 16TB Ram DDR55-15000 | uEthernet-1Tb/s | FastNix 30.021", 1299.99, 500, false);
                AddNewProduct("High-end Desktop", "i17-97780 128GB L8 Cache [256 physical cores] @ 64.01 GHz | 256TB Ram DDR55-37400 | uEthernet-1Tb/s | FastNix 30.021", 5299.99, 33, false);
                AddNewProduct("Server", "uXeon-778X3 1TB L16 Cache [1024 physical cores] @ 88.74 GHz | 8192TB Ram DDR55-66600 | 16x pEthernet-10Tb/s | FastNixServer 12.844.15.5", 15799.99, 7, true);
            }
        }

        public Product AddNewProduct(Product product)
        {
            Product newProduct = new Product(GetNewProductId(), product.Name, product.Description, product.Price, product.Stock, product.OnSale);
            products.Add(newProduct.ProductId, newProduct);
            return newProduct;
        }

        public Product AddNewProduct(string name, string description, double price, int stock, bool onSale)
        {
            Product newProduct = new Product(GetNewProductId(), name, description, price, stock, onSale);
            products.Add(newProduct.ProductId, newProduct);
            return newProduct;
        }

        private int GetNewProductId()
        {
            return ++productIdCounter;
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

            if (product.Stock != -1)
            {
                --product.Stock;
            }

            return true;
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
    }
}
