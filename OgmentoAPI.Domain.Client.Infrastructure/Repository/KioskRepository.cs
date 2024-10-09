using OgmentoAPI.Domain.Client.Abstractions.DataContext;
using OgmentoAPI.Domain.Client.Abstractions.Dto;
using OgmentoAPI.Domain.Client.Abstractions.Models;
using OgmentoAPI.Domain.Client.Abstractions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgmentoAPI.Domain.Client.Infrastructure.Repository
{
	public class KioskRepository : IKioskRepository
	{
		private readonly ClientDBContext _context;

		public KioskRepository(ClientDBContext context)
		{
			_context = context;
		}



		public List<KioskModel> GetKioskDetails()
		{
			List<Kiosk> allDetail = _context.Kiosk.ToList();

			List<KioskModel> kioskModels = GetKiosk(allDetail);
			return kioskModels;
		}

		public List<KioskModel> GetKioskDetails(List<int> salesCenterIds)
		{
			List<Kiosk> allDetail = _context.Kiosk.Where(x => salesCenterIds.Contains(x.SalesCenterId)).ToList();

			List<KioskModel> kioskModels = GetKiosk(allDetail);
			return kioskModels;

		}

		public int? UpdateKioskDetails(string kioskName, int salesCenterId)
		{
			Kiosk kiosk = _context.Kiosk.FirstOrDefault(x => x.KioskName == kioskName);
			kiosk.SalesCenterId = salesCenterId;

			_context.Update(kiosk);
			return _context.SaveChanges();

		}
		public bool DeleteKioskByName(string kioskName)
		{
			int noOfRowsDeleted = 0;
			Kiosk? kiosk = _context.Kiosk.FirstOrDefault(x => x.KioskName == kioskName);
			if (kiosk == null)
			{
				_context.Kiosk.Remove(kiosk);
				noOfRowsDeleted = _context.SaveChanges();
			}
			if (noOfRowsDeleted > 0)
			{
				return true;
			}
			return false;
		}

		public List<KioskModel> GetKiosk(List<Kiosk> kiosk)
		{

			List<KioskModel> kioskModels = kiosk.Select(x => new KioskModel
			{
				KioskName = x.KioskName,
				SalesCenterId = x.SalesCenterId,
				ID = x.ID,
				IsActive = x.IsActive,
				IsDeleted = x.IsDeleted,

			}).ToList();
			return kioskModels;
		}

	}
}
