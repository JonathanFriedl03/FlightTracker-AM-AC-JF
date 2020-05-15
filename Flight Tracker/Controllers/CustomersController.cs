  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Flight_Tracker.Data;
using Flight_Tracker.Models;
using Microsoft.AspNetCore.Authorization;
using Flight_Tracker.Services;

using Flight_Tracker.Contracts;
using System.Security.Claims;
using System.Globalization;


namespace Flight_Tracker.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITSAWaitTimesService _tsaWaitTimesService;
        private readonly DirectionService _directions;
        private IRepositoryWrapper _repo;
        public FlightService _flightService;

        public CustomersController(ApplicationDbContext context,
            DirectionService directions,
            IRepositoryWrapper repo,
            FlightService flightService,
            ITSAWaitTimesService tsaWaitTimesService)
        {
            _tsaWaitTimesService = tsaWaitTimesService;
            _directions = directions;
            _tsaWaitTimesService = tsaWaitTimesService;
            _context = context;
            _repo = repo;
            _flightService = flightService;
        }
        // GET: Customers
        public async Task<IActionResult> Index(string flightNumber, string searchFlight)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customerToDisplay = _repo.Customer.GetCustomer(userId);
            if (customerToDisplay == null)
            {
                return RedirectToAction("Create");
            }
            var flights = _repo.Flight.GetFlights(customerToDisplay.Id);
            FlightInfo flight = new FlightInfo();
            CustomerFlightInfoViewModel customerFlight = new CustomerFlightInfoViewModel();
           
            if (flightNumber != null)
            {
                flight.CustomerId = customerToDisplay.Id;
                flight.FlightNumber = flightNumber;
                _repo.Flight.CreateFlight(flight);
                await _context.SaveChangesAsync();
            }       
            ViewBag.Check = flight.FlightNumber;
            DataInfo info = new DataInfo();
            if (flightNumber != null && flight.FlightNumber !=null)
            {
                info = await _flightService.GetArrivalInfo(flight);
            }
            else if(flights.Count != 0)
            {
                info = await _flightService.GetArrivalInfo(flights[flights.Count - 1]);
            }
            ViewBag.Flights = info.data;
            customerFlight.Customer = customerToDisplay;
            customerFlight.Flights = flights;
            if (searchFlight != null)
            {               
                return await SetFlightInfo(info, customerToDisplay,flights[flights.Count-1], Convert.ToInt32(searchFlight));
            }
            return View(customerFlight);
        }
        public async Task<IActionResult> SetFlightInfo(DataInfo info, Customer customer, FlightInfo flightInfo, int index)
        {
            
            flightInfo.Airport = info.data[index].departure.airport;
            flightInfo.ArrivalAirport = info.data[index].arrival.airport;
            flightInfo.Airline = info.data[index].airline.name;
            flightInfo.FlightStatus = info.data[index].flight_status;
            flightInfo.Gate = info.data[index].departure.gate;
            flightInfo.Delay = info.data[index].departure.delay;
            flightInfo.AirportCode = info.data[index].departure.iata;
            flightInfo.ArrivalAirportCode = info.data[index].arrival.iata;
            flightInfo.EstimatedDeparture = info.data[index].departure.scheduled;
            flightInfo.ActualDeparture = info.data[index].departure.actual;
            flightInfo.EstimatedArrival = info.data[index].arrival.scheduled;
            flightInfo.ActualArrival = info.data[index].arrival.actual;
            customer.TSAWaitTimeOnArrival = null;
            DateTimeOffset? dateTime = flightInfo.EstimatedDeparture;
            customer.EpochTime = dateTime.Value.ToUnixTimeSeconds();
            TravelInfo travelInfo = await _directions.GetDirections(customer, flightInfo);
             SetDirectionsInfo(travelInfo, customer);
            _repo.Flight.EditFlight(flightInfo);
            _repo.Customer.EditCustomer(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var customer = _repo.Customer.GetCustomer(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            Customer customer = new Customer();
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View(customer);
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,FlightDate,FlightNumber,StreetAddress,City,State,ZipCode,Latitude,Longitude,Airport,FlightStatus,Gate,Delay,EstimatedDeparture,ActualDeparture,EstimatedArrival,ActualArrival,UserName,Email,IdentityUserId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                customer.IdentityUserId = userId;

                //make directions api call
                _repo.Customer.CreateCustomer(customer);            
                await _context.SaveChangesAsync();

            }
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", customer.IdentityUserId);
            return RedirectToAction(nameof(Index)); ;
        }

        public void SetDirectionsInfo(TravelInfo travelInfo, Customer customer)
        {
            customer.duration = travelInfo.routes[0].legs[0].duration_in_traffic.value;
            customer.duration = customer.duration / 60;
            customer.distance = travelInfo.routes[0].legs[0].distance.value;
            customer.endLatitude = travelInfo.routes[0].legs[0].end_location.lat;
            customer.endLongitude = travelInfo.routes[0].legs[0].end_location.lng;
            customer.startLatitude = travelInfo.routes[0].legs[0].start_location.lat;
            customer.startLongitude = travelInfo.routes[0].legs[0].start_location.lng;
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = _repo.Customer.GetCustomer(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", customer.IdentityUserId);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,FlightNumber,StreetAddress,City,State,ZipCode,Latitude,Longitude,Airport,FlightStatus,Gate,Delay,EstimatedDeparture,ActualDeparture,EstimatedArrival,ActualArrival,UserName,Email,IdentityUserId")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    customer.IdentityUserId = userId;
                    _repo.Customer.EditCustomer(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", customer.IdentityUserId);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = _repo.Customer.GetCustomer(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _repo.Customer.DeleteCustomer(id);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            var customer = _repo.Customer.GetCustomer(id);
            if (customer != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<IActionResult> FlightInfo(TimeSpan? time, int flightId)
        {    
            CustomerFlightInfoViewModel customerFlight = new CustomerFlightInfoViewModel();
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _repo.Customer.GetCustomer(userId);
            if (flightId != 0)
            {
                customer.flightId = flightId;
            }
            var flight = _repo.Flight.GetFlight(customer.flightId);
            customer.SelectedArrivalTime = time;
            _repo.Customer.EditCustomer(customer);
            await _context.SaveChangesAsync();
            customerFlight.Customer = customer;
            customerFlight.FlightInfo = flight;
            if (customer.SelectedArrivalTime == null)
            {
                return View(customerFlight);
            }
            else if (customer.TSAWaitTimeOnArrival == null)
            {               
                Airport airport = await _tsaWaitTimesService.GetWaitTimes(flight.AirportCode);
                for (int i = 0; i < 24; i++)
                {
                    TimeSpan start = new TimeSpan(i, 0, 0);
                    TimeSpan end = new TimeSpan((i + 1), 0, 0);
                    if ((time > start) && (time <= end))
                    {
                        customer.TSAWaitTimeOnArrival = airport.estimated_hourly_times[i].waittime;
                        break;
                    }
                }
                _repo.Customer.EditCustomer(customer);
                await _context.SaveChangesAsync();
                ViewBag.TransitTime = customer.duration + customer.TSAWaitTimeOnArrival;
                return View(customerFlight);
            }
            else
            {
                ViewBag.TransitTime = customer.duration + customer.TSAWaitTimeOnArrival;
                return View(customerFlight);
            }
        }
       
       
    }
}


