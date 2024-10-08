using Microsoft.EntityFrameworkCore;
using OgmentoAPI.Domain.Client.Abstractions.DataContext;
using OgmentoAPI.Domain.Client.Abstractions.Models;
using OgmentoAPI.Domain.Client.Abstractions.Repositories;
using System.Linq.Expressions;

namespace OgmentoAPI.Domain.Client.Infrastructure.Repository
{
    public class SalesCenterRepository : ISalesCenterRepository
    {
        private readonly ClientDBContext _context;
        public SalesCenterRepository(ClientDBContext Context)
        {
            _context = Context;
        }
        public IEnumerable<int> GetSalesCenterIds(Expression<Func<SalesCenterUserMapping, bool>> predicate)
        {
            var salesCenterIds = _context.SalesCenterUserMapping
                .AsNoTracking()
                .Where(predicate)
                .Select(mapping => mapping.SalesCenterId)
                .ToList();

            return salesCenterIds;
        }
        public IEnumerable<SalesCenter> GetSalesCenter(Expression<Func<SalesCenterUserMapping, bool>> predicate)
        {
            var salesCenterIds = GetSalesCenterIds(predicate);
            return _context.SalesCenter
                .AsNoTracking()
                .Where(x => salesCenterIds.Contains(x.ID))
                .ToList();
        }

        public List<SalesCenterModel> GetSalesCenterDetails()
        {
            List<SalesCenter> allDetail = _context.SalesCenter.ToList();

            List<SalesCenterModel> salesCenterModel = allDetail.Select(x => new SalesCenterModel
            {
                City = x.City,
                CountryId = x.CountryId,
                SalesCenterName = x.SalesCenterName,
                SalesCenterUid = x.SalesCenterUid,
                SalesCenterId = x.ID,


            }).ToList();
            return salesCenterModel;


        }

        public int? UpdateSalesCentersForUser(int userId, List<Guid> guids)
        {
            List<int> salesCenterIds = _context.SalesCenter.Where(x => guids.Contains(x.SalesCenterUid)).
                Select(y => y.ID).ToList();

            List<SalesCenterUserMapping> userIdsToBeDeleted = _context.SalesCenterUserMapping.Where(x => x.UserId == userId).ToList();

            _context.SalesCenterUserMapping.RemoveRange(userIdsToBeDeleted);
            List<SalesCenterUserMapping> userMappings = GetSalesCenterUserMappingDetails(userId, salesCenterIds);
            _context.SalesCenterUserMapping.AddRange(userMappings);
            return _context.SaveChanges();

        }

        private static List<SalesCenterUserMapping> GetSalesCenterUserMappingDetails(int userId, List<int> salesCenterIds)
        {
            return salesCenterIds.Select(x => new SalesCenterUserMapping
            {
                UserId = userId,
                SalesCenterId = x
            }).ToList();
        }

        public int? UpdateMainSalesCenter(SalesCenterModel salesCenterModel)
        {
			SalesCenter salesCenter = GetSalesCenterDetail(salesCenterModel.SalesCenterUid);


			if (salesCenter == null)
            {
                return null;
            }
            salesCenter.SalesCenterName = salesCenterModel.SalesCenterName;
            salesCenter.CountryId = salesCenterModel.CountryId;
            salesCenter.City = salesCenterModel.City;
            _context.SalesCenter.Update(salesCenter);
            return _context.SaveChanges();

        }

        public int? AddSalesCenter(SalesCenterModel salesCenterModel)
        {
            bool isExists = _context.SalesCenter.Any(x => x.SalesCenterName == salesCenterModel.SalesCenterName
      && x.City == salesCenterModel.City
      && x.CountryId == salesCenterModel.CountryId);
            if (!isExists)
            {
                SalesCenter salesCenter = new SalesCenter()
                {
                    SalesCenterUid = Guid.NewGuid(),
                    SalesCenterName = salesCenterModel.SalesCenterName,
                    City = salesCenterModel.City,
                    CountryId = salesCenterModel.CountryId,
                };
                _context.SalesCenter.Add(salesCenter);
                return _context.SaveChanges();
            }
            return null;

        }


        public int? DeleteSalesCenter(Guid salesCenterUid)
        {
            SalesCenter salesCenter =GetSalesCenterDetail(salesCenterUid);

            if (salesCenter == null)
            {
                return null;
            }

            _context.Remove(salesCenter);
            return _context.SaveChanges();
        }

        public int GetUserSalesCenterMappingId(Guid salesCenterUid)
        {
            int salesCenterId = GetSalesCenterDetail(salesCenterUid).ID;
            return _context.SalesCenterUserMapping.Count(x => x.SalesCenterId == salesCenterId);
        }

        public SalesCenter GetSalesCenterDetail(Guid salesCenterUid)
        {
            return _context.SalesCenter.AsNoTracking().FirstOrDefault(x => x.SalesCenterUid == salesCenterUid);
        }
    }
}
