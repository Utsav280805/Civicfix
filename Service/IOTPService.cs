using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Service
{
    
        interface IOTPService
        {
            Task<bool> GenerateAndSendOTPAsync(string mobileNumber);
            bool VerifyOTP(string mobileNumber, string enteredOTP);
        }
    
}
