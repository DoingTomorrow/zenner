// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.StructuresManagement.TenantManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Core.Model.MSSClient;
using MSS.Core.Model.Structures;
using MSS.Interfaces;
using MSS.Localisation;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace MSS.Business.Modules.StructuresManagement
{
  public class TenantManager
  {
    private readonly IRepository<Location> _locationRepository;
    private readonly IRepository<Scenario> _scenarioRepository;
    private readonly IRepository<Tenant> _tenantRepository;
    private readonly IRepositoryFactory _repositoryFactory;

    [Inject]
    public TenantManager(IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this._tenantRepository = repositoryFactory.GetRepository<Tenant>();
      this._locationRepository = repositoryFactory.GetRepository<Location>();
      this._scenarioRepository = repositoryFactory.GetRepository<Scenario>();
    }

    public IEnumerable<string> GetFloorNames()
    {
      return (IEnumerable<string>) ((IEnumerable<string>) Enum.GetNames(typeof (FloorNamesEnum))).ToList<string>();
    }

    public IEnumerable<string> GetDirections()
    {
      return (IEnumerable<string>) ((IEnumerable<string>) Enum.GetNames(typeof (DirectionsEnum))).ToList<string>();
    }

    public void CreateTenant(Tenant tenantToBeAdded, Guid locationGuid)
    {
      this._tenantRepository.Insert(tenantToBeAdded);
    }

    public void EditTenant(Tenant tenantToBeEdited)
    {
      this._tenantRepository.Update(tenantToBeEdited);
    }

    public void DeleteTenant(Guid tenantGuid)
    {
      this._tenantRepository.Delete(this._tenantRepository.GetById((object) tenantGuid));
    }

    public IEnumerable<Tenant> GetTenants()
    {
      return (IEnumerable<Tenant>) this._tenantRepository.GetAll();
    }

    private LocationManager GetLocationManagerInstance()
    {
      return new LocationManager(this._repositoryFactory);
    }

    public bool ValidateTenantNr(
      int tenantNr,
      List<int> localStructureTenantNrs,
      Guid selectedTenantGuid,
      out ICollection<string> validationErrors)
    {
      validationErrors = (ICollection<string>) new List<string>();
      if (localStructureTenantNrs.Contains(tenantNr))
        validationErrors.Add(Resources.MSS_Client_Structures_TenantNrExisting);
      return validationErrors.Count == 0;
    }
  }
}
