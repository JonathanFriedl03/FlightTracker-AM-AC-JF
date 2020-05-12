using Flight_Tracker.Contracts;
using Flight_Tracker.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flight_Tracker
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private ApplicationDbContext _context;
        private ICustomerRepository _customer;
        public ICustomerRepository Customer 
        { 
            get 
            { 
                if (_customer == null) 
                { 
                    _customer = new CustomerRepository(_context); 
                } 
                return _customer; 
            } 
        }
        public RepositoryWrapper(ApplicationDbContext context) 
        { 
            _context = context; 
        }
        public void Save() 
        { 
            _context.SaveChanges(); 
        }
    }
}
