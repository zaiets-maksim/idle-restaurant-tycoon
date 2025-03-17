using System;
using System.Collections;
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
        
        private Queue<Order> _orders = new();
        private Queue<Order> _ordersForServing = new();

        public bool HasOrders() => _orders.Count > 0;
        public bool HasOrdersForServe() => _ordersForServing.Count > 0;
        
        public Order GetOrder() => _orders.Peek();
        
        public Order GetOrderForServe() => _ordersForServing.Peek();

        // for customer
        public void NewOrder(Order order)
        {
            // Debug.Log($"new order: {order.DishTypeId}");

            _orders.Enqueue(order);
            OnNewOrderReceived?.Invoke(order);

            Output();
        }

        // call in chef
        public void Cooked(Order order)
        {
            // Debug.Log($"order cooked: {order.DishTypeId}");
            _orders.Dequeue();
            
            _ordersForServing.Enqueue(order);
            OnOrderCooked?.Invoke(order); // subscribe in waiter

            Output();
        }

        public void Served(Order order)
        {
            _ordersForServing.Dequeue();
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
        void Served(Order order);
    }
}