using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Flight_Tracker.Data;
using Flight_Tracker.Models;
using System.Security.Claims;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Flight_Tracker.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Contacts
        public IActionResult Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _context.Customers.Where(u => u.IdentityUserId == userId).SingleOrDefault();
            customer.Contacts = _context.Contacts.Where(c => c.UserId == customer.Id).ToList();
            return View(customer);
        }

        // GET: Contacts/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Customers, "Id", "Id");
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,PhoneNumber,UserId")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var customer = _context.Customers.Where(c => c.IdentityUserId == userId).SingleOrDefault();
                contact.UserId = customer.Id;
                contact.PhoneNumber = StandardizePhoneNumber(contact.PhoneNumber);
                const string accountSid = "ACee69b1a1fadbf6d8443320d75f3c3094";
                const string authToken = "630bda7a204c3ab6e65c7bd9c2825114";

                TwilioClient.Init(accountSid, authToken);

                var message = MessageResource.Create(
                    body: "Join Earth's mightiest heroes. Like Kevin Bacon.",
                    from: new Twilio.Types.PhoneNumber("+12057402655"),
                    to: new Twilio.Types.PhoneNumber("+" + contact.PhoneNumber)
                );
                Console.WriteLine(message.Sid);
                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Customers, "Id", "Id", contact.UserId);
            return View(contact);
        }

        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Customers, "Id", "Id", contact.UserId);
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,PhoneNumber,UserId")] Contact contact)
        {
            if (id != contact.Id)
            {
                return NotFound();
            }
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _context.Customers.Where(c => c.IdentityUserId == userId).SingleOrDefault();
            contact.UserId = customer.Id;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Contacts.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.Id))
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
            ViewData["UserId"] = new SelectList(_context.Customers, "Id", "Id", contact.UserId);
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }
        private string StandardizePhoneNumber(string phoneNumber)
        {
            var contactNumber = "";
            phoneNumber.ToCharArray();
            for(int i = 0; i < phoneNumber.Length; i++)
            {
                if (phoneNumber[i] == '-' || phoneNumber[i] == '(' || phoneNumber[i] == ')' || phoneNumber[i] == ' ')
                {
                    
                }
                else
                {
                    contactNumber += phoneNumber[i];
                }
            }

            return contactNumber;
        }
    }
}
