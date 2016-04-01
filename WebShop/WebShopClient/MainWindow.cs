using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebShopClient.WebShopService;
using System.ServiceModel;
using WebShopInterface;

namespace WebShopClient
{
    public partial class MainWindow : Form, IWebShopServiceCallback
    {
        WebShopServiceClient shop;
        Product selectedProduct;
        InstanceContext instanceContext;

        public MainWindow()
        {
            InitializeComponent();

            instanceContext = new InstanceContext(this);
            shop = new WebShopServiceClient(instanceContext);
        
            shopNameLabel.Text = "You are now shopping at: " + shop.GetName();

            productsView.DataSource = new BindingList<Product>(shop.GetProductList());
            productsView.RowEnter += ProductsView_CurrentCellChanged;
        }

        private void ProductsView_CurrentCellChanged(object sender, EventArgs e)
        {
            selectedProduct = null;
            if (productsView.SelectedRows.Count > 0)
            {
                selectedProduct = productsView.SelectedRows[0].DataBoundItem as Product;
                if (selectedProduct != null)
                {
                    inputProductNameLabel.Text = selectedProduct.Name;
                    inputPriceLabel.Text = "€" + selectedProduct.Price.ToString();
                    inputInStockLabel.Text = selectedProduct.Stock.ToString();
                    inputDescriptionText.Text = shop.GetProductInfo(selectedProduct.ProductId);
                }
            }
        }

        private void buyBtn_Click(object sender, EventArgs e)
        {
            if(selectedProduct == null)
            {
                MessageBox.Show("Select a product");
                return;
            }

            if(!shop.BuyProduct(selectedProduct.ProductId))
            {
                MessageBox.Show("This product is sold out");
                return;
            }

            selectedProduct.Stock = shop.RefreshProductStock(selectedProduct.ProductId);
            inputInStockLabel.Text = selectedProduct.Stock.ToString();

            MessageBox.Show("Product purchased");
        }

        public void productShipped(Order order)
        {
            MessageBox.Show("Product " + order.Name + " (" + order.ProductId.ToString() + ") has shipped at " + order.Moment.ToString());
        }

        public void productStockChanged(int productId, int stock)
        {
            foreach (DataGridViewRow row in productsView.Rows)
            {
                Product p = row.DataBoundItem as Product;
                if (p != null && p.ProductId == productId)
                {
                    p.Stock = stock;
                    if (p == selectedProduct)
                    {
                        inputInStockLabel.Text = p.Stock.ToString();
                    }
                    break;
                }
            }
        }

        public void newOrder(Order order)
        {
            //not needed for client
        }
    }
}
