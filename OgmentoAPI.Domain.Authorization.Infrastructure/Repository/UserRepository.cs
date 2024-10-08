using OgmentoAPI.Domain.Authorization.Abstractions.DataContext;
using OgmentoAPI.Domain.Authorization.Abstractions.Enums;
using OgmentoAPI.Domain.Authorization.Abstractions.Models;
using OgmentoAPI.Domain.Authorization.Abstractions.Repository;
using OgmentoAPI.Web.DataContext;

namespace OgmentoAPI.Domain.Authorization.Infrastructure.Repository
{
    public class UserRepository : IAuthorizationRepository
    {
        private readonly AuthorizationDbContext _context;
        public UserRepository(AuthorizationDbContext context)
        {
            _context = context;
        }
        public RolesMaster GetUserRole(int userId)
        {
            UserRoles roleID = _context.UsersMaster.Find(userId).RoleId;
            RolesMaster roleMaster = _context.RolesMaster.FirstOrDefault(x => x.RoleId == roleID);
            return roleMaster;
        }
        public string GetRoleName(int userId)
        {
            var roleMaster = GetUserRole(userId);
            return roleMaster.RoleName;
        }
        public UserModel GetUserByID(int userId)
        {
            return (from UM in _context.UsersMaster
                    where UM.UserId == userId
                    select new UserModel
                    {
                        UserId=UM.UserId,
                        UserUid = UM.UserUid,
                        UserName = UM.UserName,
                        Email = UM.Email,
                        PhoneNumber = UM.PhoneNumber,
                        //UserRole=UM.UserRole,
                        City=UM.City,

                        ValidityDays = UM.ValidityDays,
                    }).FirstOrDefault();
        }
        public UsersMaster GetUserDetail(LoginModel login)
        {
            return _context.UsersMaster.FirstOrDefault(c => c.Email == login.Email && c.Password == login.Password);
        }

        public List<UserModel> GetUserDetails()
        {
            return  _context.UsersMaster.Select(UM=> new UserModel
                     {
                        UserId = UM.UserId,
                        UserUid = UM.UserUid,
                        UserName = UM.UserName,
                        Email = UM.Email,
                        PhoneNumber = UM.PhoneNumber,
                        City = UM.City,
                        ValidityDays = UM.ValidityDays,
                    }).ToList();
        }


        public int? UpdateUser(UserModel user)
        {
            UsersMaster userMaster = _context.UsersMaster.FirstOrDefault(x => x.UserUid == user.UserUid);
            if (user == null)
            {
                return null;
            }
            userMaster.UpdatedOn = DateTime.UtcNow;
            userMaster.UserName = user.UserName;
            userMaster.Email = user.Email;
            userMaster.PhoneNumber = user.PhoneNumber;
            userMaster.ValidityDays = user.ValidityDays;
            userMaster.City = user.City;

            var role = _context.RolesMaster.First(x => x.RoleName == user.UserRole);
            userMaster.RoleId = role.RoleId;

            _context.Update(userMaster);
            return _context.SaveChanges();
        }
        public int? AddUser(UserModel user)
        {
            UsersMaster userMaster = new UsersMaster()
            {
              UserUid = Guid.NewGuid(),
                UpdatedOn = DateTime.UtcNow,
                CreatedOn = DateTime.UtcNow,
                UserName = user.UserName,
                Password = user.Password,
                Email = user.Email,
                City = user.City,
                ValidityDays = user.ValidityDays,
                PhoneNumber = user.PhoneNumber,
                CountryId = 1
            };
            RolesMaster role = _context.RolesMaster.First(x => x.RoleName == user.UserRole);
            userMaster.RoleId = role.RoleId;
            var entity = _context.UsersMaster.Add(userMaster);
            _context.SaveChanges();
            return entity.Entity.UserId;
        }
        public bool DeleteUserDetails(Guid userUId)
        {
            int noOfRowsDeleted = 0;
            UsersMaster usersMaster = _context.UsersMaster.FirstOrDefault(user => user.UserUid == userUId);
            if (usersMaster != null)
            {
                _context.UsersMaster.Remove(usersMaster);
                noOfRowsDeleted = _context.SaveChanges();

            }
            if (noOfRowsDeleted > 0)
            {
                return true;
            }
            return false;
        }

    }
}



