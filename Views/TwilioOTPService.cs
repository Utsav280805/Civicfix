using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivicFix.Services;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace CivicFix.Views
{
    class TwilioOTPService : IOTPService
    {
        private static readonly Random random = new Random();
        private static readonly Dictionary<string, (string otp, DateTime expiry)> otpStore = new Dictionary<string, (string, DateTime)>();

        private const string AccountSid = "ACe242d2e9d495b4b1d2e511466ef40ca4";
        private const string AuthToken = "62e5d48ec21913bd40eee61e4514f3e7";
        private const int OTP_LENGTH = 6;
        private const int OTP_EXPIRY_MINUTES = 2;

        public TwilioOTPService()
        {
            TwilioClient.Init(AccountSid, AuthToken);
        }

        public async Task<bool> GenerateAndSendOTPAsync(string mobileNumber)
        {
            string otp = random.Next((int)Math.Pow(10, OTP_LENGTH - 1), (int)Math.Pow(10, OTP_LENGTH)).ToString();
            otpStore[mobileNumber] = (otp, DateTime.UtcNow.AddMinutes(OTP_EXPIRY_MINUTES));

            try
            {
                var accountSid = "ACe242d2e9d495b4b1d2e511466ef40ca4";
                var authToken = "62e5d48ec21913bd40eee61e4514f3e7";
                TwilioClient.Init(accountSid, authToken);
                var messageOptions = new CreateMessageOptions(
                  new PhoneNumber($"+91{mobileNumber}"));
                messageOptions.MessagingServiceSid = "MG4fddc5138ad053f65c8bf8cf38c39a39";
                messageOptions.Body = $"Hello  : this is Your One Time passWord :{otp}";
                var message = MessageResource.Create(messageOptions);
                Console.WriteLine(message.Body);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending OTP: {ex.Message}");
                return false;
            }
        }

        public bool VerifyOTP(string mobileNumber, string enteredOTP)
        {
            if (otpStore.TryGetValue(mobileNumber, out var otpData))
            {
                if (DateTime.UtcNow > otpData.expiry)
                {
                    Console.WriteLine("OTP expired.");
                    return false;
                }

                if (otpData.otp == enteredOTP)
                {
                    Console.WriteLine("OTP verified successfully.");
                    otpStore.Remove(mobileNumber); // Remove OTP after verification
                    return true;
                }
                else
                {
                    Console.WriteLine("Invalid OTP.");
                    return false;
                }
            }
            Console.WriteLine("No OTP found for this number.");
            return false;
        }
    }
}
