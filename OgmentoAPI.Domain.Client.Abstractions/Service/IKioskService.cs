using OgmentoAPI.Domain.Client.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgmentoAPI.Domain.Client.Abstractions.Service
{
    public interface IKioskService
    {
        List<KioskModel> GetKioskDetails();
        List<KioskModel> GetKioskDetails(List<int> salesCenterIds);
        int? UpdateKioskDetails(string kioskName, Guid salesCenterId);

        bool DeleteKioskByName(string kioskName);
    }

}
