﻿namespace PropertySalesAPI.Models.ViewModels
{
    public class RegisterUserRequest
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public long ContactNumber { get; set; }
        public string Address { get; set; }
        public long Pincode { get; set; }
        public long AadhaarCard { get; set; }
    }
}
