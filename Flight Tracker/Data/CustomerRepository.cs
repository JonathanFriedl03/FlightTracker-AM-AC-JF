using Flight_Tracker.Contracts;
using Flight_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            FindByCondition(c => c.Id.Equals(customerId)).SingleOrDefault();
        //public Customer GetCustomer(string userId) =>
           //FindByCondition(c => c.IdentityUserId.Equals(userId)).SingleOrDefault();
        public void CreateCustomer(Customer customer) => Create(customer);
        public void EditCustomer(Customer customer)
        {
            var customerToUpdate = FindByCondition(c => c.Id.Equals(customer.Id)).SingleOrDefault();
            if (customerToUpdate != null)
            {
                customerToUpdate = customer;
                Update(customer);
            }
        }
        public void DeleteCustomer(int customerId)
        {
            var customerToDelete = FindByCondition(c => c.Id.Equals(customerId)).SingleOrDefault();
            Delete(customerToDelete);
        }
    }
}
