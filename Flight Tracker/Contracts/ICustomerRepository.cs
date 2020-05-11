using Flight_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flight_Tracker.Contracts
{
    public interface ICustomerRepository : IRepositoryBase<Customer>
    {
        Customer GetCustomer(int customerId); 
        void CreateCustomer(Customer customer);
    }
}
