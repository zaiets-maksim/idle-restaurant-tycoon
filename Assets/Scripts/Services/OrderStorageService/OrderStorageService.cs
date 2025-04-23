using System;
using System.Collections.Generic;
using System.Text;
using Characters.Customers;
using UnityEngine;

namespace Services.OrderStorageService
{
    public class OrderStorageService : IOrderStorageService
    {
        public event Action<Order> OnOrderCooked;
        public event Action<Order> OnNewOrderReceived;
        
        private readonly Queue<Order> _orders = new();
        private readonly Queue<Order> _ordersForServing = new();

        
        public bool HasOrders() => _orders.Count > 0;
        
        public bool HasOrdersForServe() => _ordersForServing.Count > 0;
        
        public Order GetOrder() => _orders.Dequeue();

        public Order GetOrderForServe() => _ordersForServing.Dequeue();
        
        public void NewOrder(Order order)
        {
            // Debug.Log($"new order: {order.DishTypeId}");

            _orders.Enqueue(order);
            
            Debug.Log($"<<OnNewOrderReceived>> {order.DishTypeId}");
            Output();
            
            OnNewOrderReceived?.Invoke(order);
        }
        
        public void Cooked(Order order)
        {
            // Debug.Log($"order cooked: {order.DishTypeId}");
            // _orders.Dequeue();
            
            _ordersForServing.Enqueue(order);
            
            Output();
            Debug.Log($"<<OnOrderCooked>> {order.DishTypeId}");
            
            OnOrderCooked?.Invoke(order);
        }

        private void Output()
        {
            Debug.Log("--------------------------------------------");
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("ORDERS: ");
            foreach (var order in _orders) 
                stringBuilder.Append($"{order.DishTypeId} ");

            Debug.Log(stringBuilder);

            stringBuilder = new StringBuilder();
            stringBuilder.Append("FOR SERVING: ");
            foreach (var order in _ordersForServing) 
                stringBuilder.Append($"{order.DishTypeId} ");
            
            Debug.Log(stringBuilder);
            Debug.Log("--------------------------------------------");
        }
    }
    
    public class Order
    {
        public DishTypeId DishTypeId;
        public Customer Customer;

        public Order(DishTypeId dishTypeId, Customer customer)
        {
            DishTypeId = dishTypeId;
            Customer = customer;
        }
    }

    public interface IOrderStorageService
    {
        event Action<Order> OnNewOrderReceived;
        event Action<Order> OnOrderCooked;
        bool HasOrders();
        bool HasOrdersForServe();
        Order GetOrder();
        Order GetOrderForServe();
        void NewOrder(Order order);
        void Cooked(Order order);
    }
}