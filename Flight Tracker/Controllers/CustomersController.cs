  
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
        public async Task<IActionResult> Index(string flightNumber, string flightDate)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _repo.Customer.GetCustomer(userId);
            ViewData["FlightNumber"] = flightNumber;
            ViewData["FlightDate"] = flightDate;
            if (flightNumber != null && flightDate != null)
            {
                customer.FlightNumber = flightNumber.Trim();
                customer.FlightDate = flightDate;
                _repo.Customer.EditCustomer(customer);
                await _context.SaveChangesAsync();
            }
            if (customer == null)
            {
                return RedirectToAction("Create");
            }
            ViewBag.Check = customer.FlightNumber;
            DataInfo info = new DataInfo();
            List<string> flightData = new List<string>();
            if (customer.FlightNumber != null)
            {
                info = await _flightService.GetArrivalInfo(customer);

                for (int i = 0; i < info.data.Length; i++)
                {
                    string flight = info.data[i].departure.airport + " " + info.data[i].departure.scheduled;
                    flightData.Add(flight);
                }
            }
            SelectList selectFlights = new SelectList(flightData);
            ViewData["Flights"] = selectFlights;
            //await SetFlightInfo(info, customer[0]);
            return View(customer);
        }
        public async Task SetFlightInfo(DataInfo info, Customer customer)
        {
            List<SelectListItem> flights = null;
            for (int i = 0; i < info.data.Length; i++)
            {
                var newFlight = new SelectListItem() { Text = info.data[i].departure.airport, Value = info.data[i].departure.airport };
                flights.Add(newFlight);

                //if (info.data[i].flight_date == customer.FlightDate) 
                //{
                //    customer.EstimatedDeparture = info.data[i].departure.estimated;
                //    customer.Airport = info.data[i].departure.airport;
                //    _repo.Customer.EditCustomer(customer);
                //    await _context.SaveChangesAsync();
                //}
            }
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
                TravelInfo travelInfo = await _directions.GetDirections(customer);
                
                await SetDirectionsInfo(travelInfo, customer);
                
                await _context.SaveChangesAsync();

            }
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", customer.IdentityUserId);
            return RedirectToAction(nameof(Index)); ;
        }

        public async Task SetDirectionsInfo(TravelInfo travelInfo, Customer customer)
        {

            customer.duration = travelInfo.routes[0].legs[0].duration.value;
            customer.duration = customer.duration / 60;
            customer.distance = travelInfo.routes[0].legs[0].distance.value;
            customer.endLatitude = travelInfo.routes[0].legs[0].end_location.lat;
            customer.endLongitude = travelInfo.routes[0].legs[0].end_location.lng;
            customer.startLatitude = travelInfo.routes[0].legs[0].start_location.lat;
            customer.startLongitude = travelInfo.routes[0].legs[0].start_location.lng;
            _repo.Customer.EditCustomer(customer);

            await _context.SaveChangesAsync();
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
    }
}


