// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.StructuresManagement.LocationManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using AutoMapper;
using MSS.Core.Model.MSSClient;
using MSS.Core.Model.Structures;
using MSS.DTO.Clients;
using MSS.DTO.Structures;
using MSS.Interfaces;
using MSS.Utils.Utils;
using Ninject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#nullable disable
namespace MSS.Business.Modules.StructuresManagement
{
  public class LocationManager
  {
    private readonly IRepository<Location> _locationRepository;
    private readonly IRepository<Scenario> _scenarioRepository;
    private readonly IRepositoryFactory _repositoryFactory;

    [Inject]
    public LocationManager(IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this._locationRepository = repositoryFactory.GetRepository<Location>();
      this._scenarioRepository = repositoryFactory.GetRepository<Scenario>();
      Mapper.CreateMap<LocationDTO, Location>();
    }

    public IEnumerable<Scenario> GetScenarios()
    {
      return (IEnumerable<Scenario>) this._scenarioRepository.GetAll().OrderBy<Scenario, int>((Func<Scenario, int>) (x => x.Code));
    }

    public IEnumerable<EnumObj> GetGenerations()
    {
      ObservableCollection<EnumObj> generations = new ObservableCollection<EnumObj>();
      IEnumerable<GenerationEnum> source1 = Enum.GetValues(typeof (GenerationEnum)).Cast<GenerationEnum>();
      if (!(source1 is GenerationEnum[] generationEnumArray))
        generationEnumArray = source1.ToArray<GenerationEnum>();
      GenerationEnum[] source2 = generationEnumArray;
      if (((IEnumerable<GenerationEnum>) source2).Count<GenerationEnum>() != 0)
      {
        for (int index = 0; index < ((IEnumerable<GenerationEnum>) source2).Count<GenerationEnum>(); ++index)
        {
          GenerationEnum generationEnum = source2[index];
          EnumObj enumObj = new EnumObj()
          {
            IdEnum = index + 2,
            StatusFromObj = generationEnum.GetStringValue()
          };
          generations.Add(enumObj);
        }
      }
      return (IEnumerable<EnumObj>) generations;
    }

    public IEnumerable<EnumObj> GetScales()
    {
      ObservableCollection<EnumObj> scales = new ObservableCollection<EnumObj>();
      IEnumerable<ScaleEnum> source1 = Enum.GetValues(typeof (ScaleEnum)).Cast<ScaleEnum>();
      if (!(source1 is ScaleEnum[] scaleEnumArray))
        scaleEnumArray = source1.ToArray<ScaleEnum>();
      ScaleEnum[] source2 = scaleEnumArray;
      if (((IEnumerable<ScaleEnum>) source2).Count<ScaleEnum>() != 0)
      {
        for (int index = 0; index < ((IEnumerable<ScaleEnum>) source2).Count<ScaleEnum>(); ++index)
        {
          ScaleEnum scaleEnum = source2[index];
          EnumObj enumObj = new EnumObj()
          {
            IdEnum = index,
            StatusFromObj = scaleEnum.GetStringValue()
          };
          scales.Add(enumObj);
        }
      }
      return (IEnumerable<EnumObj>) scales;
    }
  }
}
