using AbakTools.Core.Domain.Product;
using Bukimedia.PrestaSharp.Entities;
using System.Collections.Generic;

namespace AbakTools.Core.Infrastructure.PrestaShop.Services
{
    public interface IUnitSynchronizeService
    {
        IEnumerable<int> GetUnitsIdsToSynchonise(long stampFrom, long stampTo);
        unit Synchronize(int unitId);
        unit Synchronize(UnitEntity unitEntity);
    }
}
