using Flight_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flight_Tracker.Contracts
{
    public interface ICustomerRepository : IRepositoryBase<Customer>
    {
        Customer GetCustomer(int? customerId);
        List<Customer> GetCustomer(string userId);
        void CreateCustomer(Customer customer);
        void EditCustomer(Customer customer);
        void DeleteCustomer(int customerId);
    }
}
