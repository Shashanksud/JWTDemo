﻿using OgmentoAPI.Domain.Authorization.Abstraction.DataContext;
using OgmentoAPI.Domain.Client.Abstractions.DataContext;

namespace OgmentoAPI.Domain.Authorization.Abstraction.DataContext
{
    public partial class UsersMaster
    {
        public UsersMaster()
        {
            RefreshToken = new HashSet<RefreshToken>();
            UserRoles = new HashSet<UserRoles>();
            UserSalesCenter= new HashSet<SalesCenterUserMapping>();
            
        }

        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int? ValidityDays {  get; set; }
        public virtual ICollection<RefreshToken> RefreshToken { get; set; }
        public virtual ICollection<UserRoles> UserRoles { get; set; }
        public virtual ICollection<SalesCenterUserMapping> UserSalesCenter { get; set; }
    }
}
