using AbakTools.Core.Domain.Product.Projections;
using AbakTools.Core.Domain.Product.Repositories;
using AbakTools.Core.Domain.Product.Specifications;
using AbakTools.Core.Infrastructure.PrestaShop.Repositories;
using Bukimedia.PrestaSharp.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace AbakTools.Core.Infrastructure.PrestaShop.Services.Implementations
{
    class UnitSynchronizeService : IUnitSynchronizeService
    {
        private readonly ILogger _logger;
        private readonly IUnitRepository _unitRepository;
        private readonly IPsUnitRepository _psUnitRepository;

        public UnitSynchronizeService(ILogger<UnitSynchronizeService> logger, IUnitRepository unitRepository, IPsUnitRepository psUnitRepository)
            => (_logger, _unitRepository, _psUnitRepository) = (logger, unitRepository, psUnitRepository);

        public IEnumerable<int> GetUnitsIdsToSynchonise(long stampFrom, long stampTo)
        {
            return _unitRepository.GetList(UnitsToExportSpecification.Of(stampFrom, stampTo), UnitIdProjection.Create());
        }

        public unit Synchronize(int unitId)
        {
            var unit = _unitRepository.Get(unitId);
            return Synchronize(unit);
        }

        public unit Synchronize(Domain.Product.UnitEntity unit)
        {
            var psUnit = GetPsUnit(unit.WebId);

            if (psUnit == null)
            {
                psUnit = Insert(unit);
            }
            else
            {
                psUnit = Update(unit, psUnit);
            }

            unit.WebId = (int)psUnit.id;
            _unitRepository.SaveOrUpdate(unit);

            return psUnit;
        }

        private unit Insert(Domain.Product.UnitEntity unit)
        {
            _logger.LogDebug($"Insert unit {unit.Name} (IsDefault:{unit.Default}, Multiplier:{unit.Multiplier})");
            var psUnit = new unit();
            return UpdateCore(unit, psUnit);
        }

        private unit Update(Domain.Product.UnitEntity unit, unit psUnit)
        {
            _logger.LogDebug($"Update unit {unit.Name} (IsDefault:{unit.Default}, Multiplier:{unit.Multiplier})");
            return UpdateCore(unit, psUnit);
        }

        private unit UpdateCore(Domain.Product.UnitEntity unit, unit psUnit)
        {
            psUnit.is_default = unit.Default ? 1 : 0;
            psUnit.multiplier = unit.Multiplier;
            psUnit.name = unit.Name;

            return _psUnitRepository.SaveOrUpdate(psUnit);
        }

        private unit GetPsUnit(int? webId)
        {
            if (webId.HasValue)
            {
                return _psUnitRepository.Get((long)webId.Value);
            }

            return null;
        }
    }
}
