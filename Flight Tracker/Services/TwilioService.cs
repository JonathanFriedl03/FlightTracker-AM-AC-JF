using Flight_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Flight_Tracker.Services
{
    public class TwilioService
    {
        public void SendTextMessage(Contact contact)
        {
            string accountSid = APIKeys.TwilioAccountSid;
            string authToken = APIKeys.TwilioAuthToken;

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: "Join Earth's mightiest heroes. Like Kevin Bacon.",
                from: new Twilio.Types.PhoneNumber(APIKeys.TwilioPhoneNumber),
                to: new Twilio.Types.PhoneNumber(contact.PhoneNumber)
            );
            Console.WriteLine(message.Sid);
        }

    }
}
