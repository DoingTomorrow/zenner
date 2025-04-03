// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.UsersManagement.UserSettingsManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Core.Model.Meters;
using MSS.DTO.Users;
using MSS.Interfaces;
using NHibernate;
using System;
using System.Collections.Generic;

#nullable disable
namespace MSS.Business.Modules.UsersManagement
{
  public class UserSettingsManager
  {
    private readonly ISession _nhSession;
    private readonly IRepository<UserDeviceTypeSettings> _userDeviceTypeSettingsRepository;
    private readonly IRepository<MeasureUnit> _measureUnitRepository;
    private readonly IRepositoryFactory _repositoryFactory;

    public UserSettingsManager(IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this._nhSession = repositoryFactory.GetSession();
      this._userDeviceTypeSettingsRepository = repositoryFactory.GetRepository<UserDeviceTypeSettings>();
      this._measureUnitRepository = repositoryFactory.GetRepository<MeasureUnit>();
    }

    public void CreateUserDeviceTypesSettings(List<UserDeviceTypeSettingsDTO> udtsList)
    {
      if (udtsList == null)
        return;
      try
      {
        this._nhSession.FlushMode = FlushMode.Commit;
        this._nhSession.BeginTransaction();
        foreach (UserDeviceTypeSettingsDTO udts in udtsList)
          this._userDeviceTypeSettingsRepository.TransactionalInsert(new UserDeviceTypeSettings()
          {
            User = udts.User,
            DeviceType = udts.DeviceType,
            DecimalPlaces = udts.DecimalPlaces,
            DisplayUnit = this._measureUnitRepository.GetById((object) udts.DisplayUnitId)
          });
        this._nhSession.Transaction.Commit();
      }
      catch (Exception ex)
      {
        this._nhSession.Transaction.Rollback();
        throw;
      }
    }

    public void UpdateUserDeviceTypesSettings(List<UserDeviceTypeSettingsDTO> udtsList)
    {
      if (udtsList == null)
        return;
      try
      {
        this._nhSession.FlushMode = FlushMode.Commit;
        this._nhSession.BeginTransaction();
        foreach (UserDeviceTypeSettingsDTO udts in udtsList)
        {
          UserDeviceTypeSettings byId = this._userDeviceTypeSettingsRepository.GetById((object) udts.Id);
          if (byId != null)
          {
            byId.DecimalPlaces = (double?) udts?.DecimalPlaces;
            byId.DisplayUnit = this._measureUnitRepository.GetById((object) udts.DisplayUnitId);
            this._userDeviceTypeSettingsRepository.TransactionalUpdate(byId);
          }
        }
        this._nhSession.Transaction.Commit();
      }
      catch (Exception ex)
      {
        this._nhSession.Transaction.Rollback();
        throw;
      }
    }
  }
}
