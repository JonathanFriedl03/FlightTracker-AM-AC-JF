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
        private IFlightRepository _flight;
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
        public IFlightRepository Flight
        {
            get
            {
                if(_flight == null)
                {
                    _flight = new FlightRepository(_context);
                }
                return _flight;
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
