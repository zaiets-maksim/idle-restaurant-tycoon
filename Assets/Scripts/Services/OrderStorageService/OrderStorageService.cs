using System;
using System.Collections;
using System.Collections.Generic;
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

        // for customer
        public void NewOrder(Order order)
        {
            Debug.Log($"new order: {order.DishTypeId}");

            _orders.Enqueue(order);
            OnNewOrderReceived?.Invoke(order);
        }

        // call in chef
        public void Cooked(Order order)
        {
            Debug.Log($"order cooked: {order.DishTypeId}");
            _ordersForServing.Enqueue(order);
            OnOrderCooked?.Invoke(order); // subscribe in waiter
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