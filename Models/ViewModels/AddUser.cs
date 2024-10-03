using System.ComponentModel.DataAnnotations;

namespace PropertySales.Models.ViewModels
{
    public class AddUser
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string Pincode { get; set; }
        public long AdhaarCard { get; set; }
    }
}
