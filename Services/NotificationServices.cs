using FirebaseAdmin.Messaging;
using packagesentinel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace packagesentinel.Services
{
    public class NotificationServices
    {

        public async Task<bool> NotifyUserAsync(string token, string title, string body)
        {
            try
            {
                var message = new Message()
                {
                    Token = token,
                    Notification = new FirebaseAdmin.Messaging.Notification()
                    {
                        Title = title,
                        Body = body
                    }
                };

                // Send a message to the device corresponding to the provided registration token.
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                // Response is a message ID string.

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }
    }
}
