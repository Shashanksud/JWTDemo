using OgmentoAPI.Domain.Client.Abstractions.DataContext;
using OgmentoAPI.Domain.Client.Abstractions.Models;
using OgmentoAPI.Domain.Client.Abstractions.Repositories;
using OgmentoAPI.Domain.Client.Abstractions.Service;
using System.Linq.Expressions;

namespace OgmentoAPI.Domain.Client.Services
{
    public class SalesCenterService : ISalesCenterService
    {
        private readonly ISalesCenterRepository _salesCenterRepository;
        public SalesCenterService(ISalesCenterRepository salesCenterRepository)
        {
            _salesCenterRepository = salesCenterRepository;
        }
        public IEnumerable<SalesCenter> GetSalesCenterForUser(int Id)
        {
            Expression<Func<SalesCenterUserMapping, bool>> predicate = (mapping => mapping.UserId == Id);
            return _salesCenterRepository.GetSalesCenter(predicate);
        }
        public List<SalesCenterModel> GetAllSalesCenters()
        {
            IEnumerable<SalesCenterModel> salesCenters = _salesCenterRepository.GetSalesCenterDetails();
            return salesCenters.ToList();

        }

        public int? UpdateSalesCenters(int userId, List<Guid> guids)
        {

            try
            {
                return _salesCenterRepository.UpdateSalesCentersForUser(userId, guids);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int? UpdateMainSalesCenter(SalesCenterModel salesCenterModel)
        {
            try
            {
                return _salesCenterRepository.UpdateMainSalesCenter(salesCenterModel);
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public int? AddSalesCenter(SalesCenterModel salesCenterModel)
        {
            try
            {
                return _salesCenterRepository.AddSalesCenter(salesCenterModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int? DeleteSalesCenter(Guid salesCenterUid)
        {
            try
            {
           int salesCenterUserCount=  _salesCenterRepository.GetUserSalesCenterMappingId(salesCenterUid);
                if(salesCenterUserCount>0)
                {
                    throw new Exception("CannotDelete");
                }
                return _salesCenterRepository.DeleteSalesCenter(salesCenterUid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SalesCenter GetSalesCenterDetail(Guid salesCenterUid)
        {
            return _salesCenterRepository.GetSalesCenterDetail(salesCenterUid);
        }
        //public int GetUserSalesCenterMappingId(Guid salesCenterUid)
        //{
        //    {
        //        try
        //        {
        //         return   _salesCenterRepository.GetUserSalesCenterMappingId(salesCenterUid);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //}
    }
}