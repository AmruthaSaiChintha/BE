// OtpService.cs
using AadharVerify.Models;
using System;
using System.Linq;

namespace AadharVerify.Services
{
    public class OtpService
    {
        private readonly UserDataDbContext _dbContext;

        public OtpService(UserDataDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public string GenerateOtp(string phoneNumber)
        {
            using (var cryptoServiceProvider = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[4];
                cryptoServiceProvider.GetBytes(randomNumber);
                int otpNumber = BitConverter.ToInt32(randomNumber, 0) % 900000 + 100000; // Ensure a 6-digit positive OTP
                string otp = otpNumber.ToString("D6");

                // Save the OTP record in the database
                SaveOtpRecord(new Phone
                {
                    PhoneNumber = phoneNumber,
                    OTP = otp
                });

                return otp;
            }
        }

        public void SaveOtpRecord(Phone phone)
        {
            _dbContext.Phonenumber.Add(phone);
            _dbContext.SaveChanges();
        }

        public bool VerifyOtp(string phoneNumber, string enteredOtp)
        {
            Phone otpRecord = _dbContext.Phonenumber
                .Where(record => record.PhoneNumber == phoneNumber)
                .OrderByDescending(record => record.Id)
                .FirstOrDefault();

            return otpRecord != null && string.Equals(otpRecord.OTP, enteredOtp, StringComparison.Ordinal);
        }
    }
}
