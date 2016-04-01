using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebShopBackOffice.WebShopService;
using WebShopInterface;

namespace WebShopBackOffice
{
    public partial class MainWindow : Form, IWebShopServiceCallback
    {
        WebShopServiceClient shop;
        InstanceContext instanceContext;

        public MainWindow()
        {
            InitializeComponent();

            instanceContext = new InstanceContext(this);
            shop = new WebShopServiceClient(instanceContext);
            shop.SubscribeToNewOrders();
            
            var orders = shop.GetOrderList();

            foreach(Order order in orders)
            {
                ordersList.Items.Add(order);
            }
        }

        public void newOrder(Order order)
        {
            ordersList.Items.Add(order);
        }

        public void productShipped(Order order)
        {
            
        }

        public void productStockChanged(int productId, int stock)
        {
            
        }

        private void shipBtn_Click(object sender, EventArgs e)
        {
            Order order = ordersList.SelectedItem as Order;
            if(order != null && shop.ShipOrder(order))
            {
                ordersList.Items.Remove(order);
                MessageBox.Show("Order shipped");
            }
        }
    }
}
