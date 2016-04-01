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

namespace WebShopClient
{
    public partial class MainWindow : Form
    {
        WebShopServiceClient shop;
        Product selectedProduct;

        public MainWindow()
        {
            InitializeComponent();
            shop = new WebShopServiceClient();

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

            if(selectedProduct.Stock != -1)
            {
                selectedProduct.Stock = shop.RefreshProductStock(selectedProduct.ProductId);
                inputInStockLabel.Text = selectedProduct.Stock.ToString();
            }

            MessageBox.Show("Product purchased");
        }
    }
}
