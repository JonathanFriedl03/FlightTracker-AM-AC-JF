using Flight_Tracker.Contracts;
using Flight_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flight_Tracker.Data
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {

        }
        public Customer GetCustomer(int customerId) =>
            FindByCondition(c =>
            c.Id.Equals(customerId)).SingleOrDefault();
        public void CreateCustomer(Customer customer) => Create(customer);
    }
}
