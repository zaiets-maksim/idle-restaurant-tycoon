using System.Collections.Generic;
using Characters.Customers;

namespace Services.ActiveCustomersRegistry
{
    public class ActiveCustomersRegistry : IActiveCustomersRegistry
    {
        public List<Customer> Customers { get; } = new();
    
        public void Add(Customer customer) => Customers.Add(customer);

        public void Remove(Customer customer) => Customers.Remove(customer);
    }

    public interface IActiveCustomersRegistry
    {
        List<Customer> Customers { get; }

        void Add(Customer customer);
        void Remove(Customer customer);
    }
}