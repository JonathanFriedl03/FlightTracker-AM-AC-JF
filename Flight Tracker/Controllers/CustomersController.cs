﻿  
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
            DataInfo info = new DataInfo();
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customerToDisplay = _repo.Customer.GetCustomer(userId);
            if (customerToDisplay == null)
            {
                return RedirectToAction("Create");
            }
            FlightInfo flight = new FlightInfo();
            CustomerFlightInfoViewModel customerFlight = new CustomerFlightInfoViewModel();
            if (flightNumber != null)
            {
                flightNumber = RemoveSpaces(flightNumber);
                info = await _flightService.GetArrivalInfo(flightNumber);
                if (info.data.Length != 0)
                {
                    flight.CustomerId = customerToDisplay.Id;
                    flight.FlightNumber = flightNumber;
                    _repo.Flight.CreateFlight(flight);
                    await _context.SaveChangesAsync();
                }
            }
            var flights = _repo.Flight.GetFlights(customerToDisplay.Id);
            ViewBag.FlightNum = flightNumber;
            ViewBag.Check = flight.FlightNumber;
            if(flights.Count != 0)
            {
                if (flights[flights.Count - 1].Airport == null || searchFlight != null)
                {
                    info = await _flightService.GetArrivalInfo(flights[flights.Count - 1].FlightNumber);
                }
            }

            ViewBag.Flights = info.data;
            customerFlight.Customer = customerToDisplay;
            customerFlight.Flights = flights;
            if (searchFlight != null)
            {
                return await SetFlightInfo(info, customerToDisplay, flights[flights.Count - 1], Convert.ToInt32(searchFlight));
            }
            return View(customerFlight);
        }
        public string RemoveSpaces(string flightNum)
        {
            string formattedString = flightNum.Replace(" ", String.Empty);
            return formattedString;
        }
        public async Task<IActionResult> SetFlightInfo(DataInfo info, Customer customer, FlightInfo flightInfo, int index)
        {
            TimeSpan hours = new TimeSpan(5, 0, 0);
            flightInfo.Airport = info.data[index].departure.airport;
            flightInfo.ArrivalAirport = info.data[index].arrival.airport;
            flightInfo.Airline = info.data[index].airline.name;
            flightInfo.FlightStatus = info.data[index].flight_status;
            flightInfo.Gate = info.data[index].departure.gate;
            flightInfo.Delay = info.data[index].departure.delay;
            flightInfo.AirportCode = info.data[index].departure.iata;
            flightInfo.ArrivalAirportCode = info.data[index].arrival.iata;
            flightInfo.EstimatedDeparture = info.data[index].departure.scheduled.Value.Add(hours);
            if (info.data[index].departure.actual.HasValue)
            {
                flightInfo.ActualDeparture = info.data[index].departure.actual.Value.Add(hours);
            }
            flightInfo.EstimatedArrival = info.data[index].arrival.scheduled.Value.Add(hours);
            if (info.data[index].arrival.actual.HasValue)
            {
                flightInfo.ActualArrival = info.data[index].arrival.actual.Value.Add(hours);
            }
            
            _repo.Flight.EditFlight(flightInfo);
            _repo.Customer.EditCustomer(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customerToDisplay = _repo.Customer.GetCustomer(userId);
            if (customerToDisplay == null)
            {
                return NotFound();
            }
            return View(customerToDisplay);
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
            _repo.Customer.EditCustomer(customer);
            _context.SaveChangesAsync();
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customerToDisplay = _repo.Customer.GetCustomer(userId);
            if (customerToDisplay == null)
            {
                return NotFound();
            }
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", customerToDisplay.IdentityUserId);
            return View(customerToDisplay);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Customer customer)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var customerToEdit = _repo.Customer.GetCustomer(userId);
                    customerToEdit.FirstName = customer.FirstName;
                    customerToEdit.LastName = customer.LastName;
                    customerToEdit.StreetAddress = customer.StreetAddress;
                    customerToEdit.City = customer.City;
                    customerToEdit.State = customer.State;
                    customerToEdit.ZipCode = customer.ZipCode;
                    customerToEdit.IdentityUserId = userId;
                    _repo.Customer.EditCustomer(customerToEdit);
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
        public async Task<IActionResult> Delete()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customerToDisplay = _repo.Customer.GetCustomer(userId);
            if (customerToDisplay== null)
            {
                return NotFound();
            }

            return View(customerToDisplay);
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
            if (time != null)
            {
                flight.SelectedArrivalTime = time;
            }
            _repo.Customer.EditCustomer(customer);
            _repo.Flight.EditFlight(flight);
            await _context.SaveChangesAsync();
            customerFlight.Customer = customer;
            customerFlight.FlightInfo = flight;
            Airport airport = await _tsaWaitTimesService.GetWaitTimes(flight.AirportCode);
            string firstTimeSlot = airport.estimated_hourly_times[0].timeslot;
            char firstChar = firstTimeSlot[0];
            int firstTime = Convert.ToInt32(Char.GetNumericValue(firstChar));
            double? transitMins;
            bool withinTime;
            if (flight.SelectedArrivalTime.HasValue || time.HasValue)
            {
                if (flight.SelectedArrivalTime.Value.Hours < firstTime || flight.SelectedArrivalTime.Value.Hours > airport.estimated_hourly_times.Length)
                {
                    withinTime = true;
                    ViewBag.TimeCheck = withinTime;
                    return View(customerFlight);
                }
                for (int i = firstTime; i < airport.estimated_hourly_times.Length; i++)
                {
                    TimeSpan start = new TimeSpan(i, 0, 0);
                    TimeSpan end = new TimeSpan((i + 1), 0, 0);
                    if ((flight.SelectedArrivalTime >= start) && (flight.SelectedArrivalTime <= end))
                    {
                        flight.TSAWaitTimeOnArrival = airport.estimated_hourly_times[i].waittime;
                        break;
                    }
                }
                _repo.Flight.EditFlight(flight);
                await _context.SaveChangesAsync();
                EpochTimeConverter(flight);
                TravelInfo travelInfo = await _directions.GetDirections(customer, flight);
                SetDirectionsInfo(travelInfo, customer);
                transitMins = customer.duration + flight.TSAWaitTimeOnArrival;
                ViewBag.TransitTime = transitMins;
                DateTime leaveTime = ConvertTime(transitMins, flight);
                ViewBag.LeaveTimeData = leaveTime;
                string dateToDisplay = leaveTime.ToShortDateString();
                string timeToDisplay = leaveTime.ToString("hh:mm:ss tt");
                ViewBag.LeaveTime = $"{dateToDisplay} {timeToDisplay}";
            }
            return View(customerFlight);
        }
        public void EpochTimeConverter(FlightInfo flight)
        {
            DateTimeOffset date = flight.EstimatedDeparture.Value.Date;
            DateTimeOffset timeOffset = date.Add(flight.SelectedArrivalTime.Value);
            flight.EpochTime = timeOffset.ToUnixTimeSeconds();
            _repo.Flight.EditFlight(flight);
            _context.SaveChangesAsync();
        }
        public DateTime ConvertTime(double? mins, FlightInfo flight)
        {
            TimeSpan conTime = TimeSpan.FromMinutes(mins.Value);
            TimeSpan arrivalTime = new TimeSpan(flight.SelectedArrivalTime.Value.Hours, flight.SelectedArrivalTime.Value.Minutes, flight.SelectedArrivalTime.Value.Seconds);
            DateTime flightDate = flight.EstimatedDeparture.Value.Date;
            DateTime time = flightDate.Add(arrivalTime);
            DateTime value = time.Subtract(conTime);
            return value;
        }
        public IActionResult DeleteFlight(int flightId)
        {
            _repo.Flight.DeleteFlight(flightId);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
    
}

       
      


