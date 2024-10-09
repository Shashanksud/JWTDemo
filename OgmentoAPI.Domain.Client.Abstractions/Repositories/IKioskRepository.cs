using OgmentoAPI.Domain.Client.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgmentoAPI.Domain.Client.Abstractions.Repositories
{
    public interface IKioskRepository
    {
        List<KioskModel> GetKioskDetails();
        int? UpdateKioskDetails(string kioskName, int salesCenterId);
		bool DeleteKioskByName(string kioskName);
		List<KioskModel> GetKioskDetails(List<int> salesCenterIds);
	}
}
