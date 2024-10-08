using Newtonsoft.Json;


namespace OgmentoAPI.Domain.Authorization.Abstractions.Models
{
    public class UserModel
    {

        public int UserId { get; set; }
        public Guid UserUid { get; set; }
        

        public string UserName { get; set; }
      
        public string Email { get; set; }
      
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

        public string UserRole { get; set; }
		public string City { get; set; }

		public string  KioskName { get; set; }
		public Dictionary<Guid, string> SalesCenters { get; set; }
        public int? ValidityDays {  get; set; }
       
    
    }
}
