using OgmentoAPI.Domain.Authorization.Abstractions.Models;

namespace OgmentoAPI.Domain.Authorization.Abstractions.Dto
{
    public static class UserDto
    {
        public static UserDetailsDto ToDto(this UserModel user)
        {

            UserDetailsDto dto = new UserDetailsDto
            {
                UserName = user.UserName,
                UserRole = user.UserRole,
                EmailId = user.Email,
				KioskName=user.KioskName,
                UserUId = user.UserUid,
                City = user.City,
                PhoneNumber = user.PhoneNumber,
                ValidityDays = user.ValidityDays,
                SalesCenters = user.SalesCenters,
            };

            return dto;

        }

        public static UserModel ToModel(this UserDetailsDto dto, int userId)
        {
            UserModel userModel = new UserModel()
            {
                UserId = userId,
                UserName = dto.UserName,
                UserRole = dto.UserRole,
                Email = dto.EmailId,
                UserUid = dto.UserUId,
                City = dto.City,
                PhoneNumber = dto.PhoneNumber,
                ValidityDays = dto.ValidityDays,
                SalesCenters = dto.SalesCenters
            };

            return userModel;

        }

        public static List<UserDetailsDto> ToDto(this List<UserModel> userModels)
        {
            List<UserDetailsDto> userDetails = userModels.Select(x =>
             x.ToDto()
            ).ToList();
            return userDetails;

        }

        public static UserModel ToModel(this AddUserDto dto, int userId)
        {
            UserModel userModel = new UserModel()
            {
                UserId = userId,
                UserName = dto.UserName,
                UserRole = dto.UserRole,
                Email = dto.EmailId,
                UserUid = dto.UserUId,
                City = dto.City,
                PhoneNumber = dto.PhoneNumber,
                ValidityDays = dto.ValidityDays,
                SalesCenters = dto.SalesCenters,
                Password = dto.Password
            };

            return userModel;

        }


    }
}


