﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebShopBackOffice.WebShopService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="WebShopService.IWebShopService", CallbackContract=typeof(WebShopBackOffice.WebShopService.IWebShopServiceCallback))]
    public interface IWebShopService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebShopService/GetName", ReplyAction="http://tempuri.org/IWebShopService/GetNameResponse")]
        string GetName();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebShopService/GetName", ReplyAction="http://tempuri.org/IWebShopService/GetNameResponse")]
        System.Threading.Tasks.Task<string> GetNameAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebShopService/GetProductInfo", ReplyAction="http://tempuri.org/IWebShopService/GetProductInfoResponse")]
        string GetProductInfo(int productId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebShopService/GetProductInfo", ReplyAction="http://tempuri.org/IWebShopService/GetProductInfoResponse")]
        System.Threading.Tasks.Task<string> GetProductInfoAsync(int productId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebShopService/BuyProduct", ReplyAction="http://tempuri.org/IWebShopService/BuyProductResponse")]
        bool BuyProduct(int productId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebShopService/BuyProduct", ReplyAction="http://tempuri.org/IWebShopService/BuyProductResponse")]
        System.Threading.Tasks.Task<bool> BuyProductAsync(int productId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebShopService/GetProductList", ReplyAction="http://tempuri.org/IWebShopService/GetProductListResponse")]
        WebShopInterface.Product[] GetProductList();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebShopService/GetProductList", ReplyAction="http://tempuri.org/IWebShopService/GetProductListResponse")]
        System.Threading.Tasks.Task<WebShopInterface.Product[]> GetProductListAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebShopService/GetOrderList", ReplyAction="http://tempuri.org/IWebShopService/GetOrderListResponse")]
        WebShopInterface.Order[] GetOrderList();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebShopService/GetOrderList", ReplyAction="http://tempuri.org/IWebShopService/GetOrderListResponse")]
        System.Threading.Tasks.Task<WebShopInterface.Order[]> GetOrderListAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebShopService/RefreshProductStock", ReplyAction="http://tempuri.org/IWebShopService/RefreshProductStockResponse")]
        int RefreshProductStock(int productId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebShopService/RefreshProductStock", ReplyAction="http://tempuri.org/IWebShopService/RefreshProductStockResponse")]
        System.Threading.Tasks.Task<int> RefreshProductStockAsync(int productId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebShopService/ShipOrder", ReplyAction="http://tempuri.org/IWebShopService/ShipOrderResponse")]
        bool ShipOrder(WebShopInterface.Order order);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebShopService/ShipOrder", ReplyAction="http://tempuri.org/IWebShopService/ShipOrderResponse")]
        System.Threading.Tasks.Task<bool> ShipOrderAsync(WebShopInterface.Order order);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebShopService/SubscribeToNewOrders", ReplyAction="http://tempuri.org/IWebShopService/SubscribeToNewOrdersResponse")]
        void SubscribeToNewOrders();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebShopService/SubscribeToNewOrders", ReplyAction="http://tempuri.org/IWebShopService/SubscribeToNewOrdersResponse")]
        System.Threading.Tasks.Task SubscribeToNewOrdersAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IWebShopServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IWebShopService/productShipped")]
        void productShipped(WebShopInterface.Order order);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IWebShopService/productStockChanged")]
        void productStockChanged(int productId, int stock);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IWebShopService/newOrder")]
        void newOrder(WebShopInterface.Order order);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IWebShopServiceChannel : WebShopBackOffice.WebShopService.IWebShopService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WebShopServiceClient : System.ServiceModel.DuplexClientBase<WebShopBackOffice.WebShopService.IWebShopService>, WebShopBackOffice.WebShopService.IWebShopService {
        
        public WebShopServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public WebShopServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public WebShopServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public WebShopServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public WebShopServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public string GetName() {
            return base.Channel.GetName();
        }
        
        public System.Threading.Tasks.Task<string> GetNameAsync() {
            return base.Channel.GetNameAsync();
        }
        
        public string GetProductInfo(int productId) {
            return base.Channel.GetProductInfo(productId);
        }
        
        public System.Threading.Tasks.Task<string> GetProductInfoAsync(int productId) {
            return base.Channel.GetProductInfoAsync(productId);
        }
        
        public bool BuyProduct(int productId) {
            return base.Channel.BuyProduct(productId);
        }
        
        public System.Threading.Tasks.Task<bool> BuyProductAsync(int productId) {
            return base.Channel.BuyProductAsync(productId);
        }
        
        public WebShopInterface.Product[] GetProductList() {
            return base.Channel.GetProductList();
        }
        
        public System.Threading.Tasks.Task<WebShopInterface.Product[]> GetProductListAsync() {
            return base.Channel.GetProductListAsync();
        }
        
        public WebShopInterface.Order[] GetOrderList() {
            return base.Channel.GetOrderList();
        }
        
        public System.Threading.Tasks.Task<WebShopInterface.Order[]> GetOrderListAsync() {
            return base.Channel.GetOrderListAsync();
        }
        
        public int RefreshProductStock(int productId) {
            return base.Channel.RefreshProductStock(productId);
        }
        
        public System.Threading.Tasks.Task<int> RefreshProductStockAsync(int productId) {
            return base.Channel.RefreshProductStockAsync(productId);
        }
        
        public bool ShipOrder(WebShopInterface.Order order) {
            return base.Channel.ShipOrder(order);
        }
        
        public System.Threading.Tasks.Task<bool> ShipOrderAsync(WebShopInterface.Order order) {
            return base.Channel.ShipOrderAsync(order);
        }
        
        public void SubscribeToNewOrders() {
            base.Channel.SubscribeToNewOrders();
        }
        
        public System.Threading.Tasks.Task SubscribeToNewOrdersAsync() {
            return base.Channel.SubscribeToNewOrdersAsync();
        }
    }
}
