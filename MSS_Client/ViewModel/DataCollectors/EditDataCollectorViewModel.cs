// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.DataCollectors.EditDataCollectorViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using AutoMapper;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.UsersManagement;
using MSS.DTO.Minomat;
using MSS.DTO.Structures;
using MSS.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace MSS_Client.ViewModel.DataCollectors
{
  public class EditDataCollectorViewModel : DataCollectorViewModelBase
  {
    private IRepositoryFactory _repositoryFactory;

    public EditDataCollectorViewModel(IRepositoryFactory repositoryFactory, MinomatDTO minomat)
      : base(repositoryFactory)
    {
      Mapper.CreateMap<MSS.Core.Model.DataCollectors.Minomat, MinomatDTO>();
      Mapper.CreateMap<MinomatDTO, MSS.Core.Model.DataCollectors.Minomat>();
      this.IsInMasterPool = minomat.IsInMasterPool;
      this.selectedMinomat = Mapper.Map<MinomatDTO, MSS.Core.Model.DataCollectors.Minomat>(minomat);
      this._repositoryFactory = repositoryFactory;
      this.MasterRadioId = minomat.RadioId;
      this.StartDate = minomat.StartDate;
      this.EndDate = minomat.EndDate;
      if (minomat.Provider != null)
      {
        Provider byId = this._repositoryFactory.GetRepository<Provider>().GetById((object) minomat.Provider.Id);
        if (byId != null)
          this.SelectedProvider = byId;
      }
      this.Status = (StatusMinomatEnum) Enum.ToObject(typeof (StatusMinomatEnum), minomat.idEnumStatus);
      this.Registered = minomat.Registered;
      this.NotRegistered = !minomat.Registered;
      this.Challenge = minomat.Challenge;
      this.GsmId = minomat.GsmId;
      this.Polling = new int?(minomat.Polling);
      this.HostAndPort = minomat.HostAndPort;
      this.Url = minomat.Url;
      this.DateAppended = minomat.CreatedOn;
      Guid createBy;
      if (Guid.TryParse(minomat.CreatedBy, out createBy))
      {
        User user = this._repositoryFactory.GetRepository<User>().FirstOrDefault((Expression<Func<User, bool>>) (x => x.Id == createBy));
        if (user != null)
          this.AppendedBy = string.Format("{0} {1}", (object) user.FirstName, (object) user.LastName);
      }
      else
        this.AppendedBy = minomat.CreatedBy;
      this.IsInMasterPool = minomat.IsInMasterPool;
      this.IsMaster = minomat.IsMaster;
      this.SimPin = minomat.SimPin;
      this.AccessPoint = minomat.AccessPoint;
      this.UserId = minomat.UserId;
      this.UserPassword = minomat.UserPassword;
      this.SessionKey = minomat.SessionKey;
      this.SelectedCountry = minomat.Country != null ? this._repositoryFactory.GetRepository<Country>().GetById((object) minomat.Country.Id) : (Country) null;
      this.SelectedScenario = minomat.Scenario != null ? this.GetListofScenarios.FirstOrDefault<ScenarioDTO>((Func<ScenarioDTO, bool>) (x => x.Id == minomat.Scenario.Id)) : (ScenarioDTO) null;
      this.SimCardNumber = minomat.SimCardNumber;
    }
  }
}
