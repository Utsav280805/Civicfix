using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivicFix.Services
{
    interface IOTPService
    {
        Task<bool> GenerateAndSendOTPAsync(string mobileNumber);
        bool VerifyOTP(string mobileNumber, string enteredOTP);
    }
}
