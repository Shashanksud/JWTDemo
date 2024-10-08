using OgmentoAPI.Domain.Authorization.Abstractions.DataContext;
using OgmentoAPI.Domain.Authorization.Abstractions.Models;
using OgmentoAPI.Domain.Authorization.Abstractions.Repository;
using OgmentoAPI.Domain.Authorization.Abstractions.Services;
using OgmentoAPI.Domain.Client.Abstractions.DataContext;
using OgmentoAPI.Domain.Client.Abstractions.Models;
using OgmentoAPI.Domain.Client.Abstractions.Service;

namespace OgmentoAPI.Domain.Authorization.Services
{
    public class UserService : IUserService
    {
        private readonly IAuthorizationRepository _context;
        private readonly ISalesCenterService _salesCenterService;
        public UserService(IAuthorizationRepository context, ISalesCenterService salesCenterService)
        {
            _context = context;
            _salesCenterService = salesCenterService;
        }

        public UserModel GetUserDetail(int userId)
        {
            UserModel user = new UserModel();

            try
            {
                user = _context.GetUserByID(userId);
                string userRole = _context.GetRoleName(userId);
                List<SalesCenter> salesCenterNames = _salesCenterService.GetSalesCenterForUser(userId).ToList();

                Dictionary<Guid, string> salesCenterDictionary = salesCenterNames.ToDictionary(sc => sc.SalesCenterUid, sc => sc.SalesCenterName);

                if (user != null)
                {
                    user.UserRole = userRole;
                    user.SalesCenters = salesCenterDictionary;
                    return user;
                }
                else
                {
                    return user;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<UserModel> GetUserDetails()
        {
            try
            {
                List<UserModel> userModels = _context.GetUserDetails();
                userModels.ForEach(userModel =>
                {
                    string userRole = _context.GetRoleName(userModel.UserId);
					List<SalesCenter> salesCenterList = _salesCenterService.GetSalesCenterForUser(userModel.UserId).ToList();

					Dictionary<Guid, string> salesCenterDictionary = salesCenterList.ToDictionary(sc => sc.SalesCenterUid, sc => sc.SalesCenterName);
                    userModel.UserRole = userRole;
                    userModel.SalesCenters = salesCenterDictionary;
                });
                return userModels;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int? UpdateUser(UserModel user)
        {
            try
            {
                List<Guid> guidList = new List<Guid>(user.SalesCenters.Keys);
                _salesCenterService.UpdateSalesCenters(user.UserId, guidList);
                return _context.UpdateUser(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int? AddUser(UserModel user)
        {
            try
            {
                int? userId = _context.AddUser(user);
                List<Guid> guidList = new List<Guid>(user.SalesCenters.Keys);
                if (userId.HasValue)
                {
                    _salesCenterService.UpdateSalesCenters(userId.Value, guidList);

                }
                return userId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteUser(Guid userUId)
        {

            //Todo need to delete salescenter for user, userroles
            return _context.DeleteUserDetails(userUId);
        }
    }
}

