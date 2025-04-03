// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.GMM.MeasureUnitsHelper
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Core.Model.Meters;
using MSS.DTO.Meters;
using MSS.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#nullable disable
namespace MSS.Business.Modules.GMM
{
  public static class MeasureUnitsHelper
  {
    public static IEnumerable<MeasureUnitDTO> GetMeasureUnits(
      IRepository<MeasureUnit> measureUnitsRepository)
    {
      ObservableCollection<MeasureUnitDTO> source = new ObservableCollection<MeasureUnitDTO>();
      foreach (MeasureUnit measureUnit in measureUnitsRepository.GetAll().OrderBy<MeasureUnit, string>((Func<MeasureUnit, string>) (mu => mu.Code)).ToList<MeasureUnit>())
      {
        MeasureUnit currentMeasureUnit = measureUnit;
        char[] charArray = currentMeasureUnit.Code.ToCharArray();
        if (charArray.Length != 0 && charArray[0] == '˚')
        {
          charArray[0] = '°';
          currentMeasureUnit.Code = string.Concat<char>((IEnumerable<char>) charArray);
        }
        if (charArray.Length > 1 && charArray[1] == '3')
        {
          charArray[1] = '\u00B3';
          currentMeasureUnit.Code = string.Concat<char>((IEnumerable<char>) charArray);
        }
        if (source.FirstOrDefault<MeasureUnitDTO>((Func<MeasureUnitDTO, bool>) (item => item.Code == currentMeasureUnit.Code)) == null)
          source.Add(new MeasureUnitDTO()
          {
            Id = currentMeasureUnit.Id,
            Code = currentMeasureUnit.Code,
            Name = currentMeasureUnit.Code
          });
      }
      return (IEnumerable<MeasureUnitDTO>) source.OrderBy<MeasureUnitDTO, string>((Func<MeasureUnitDTO, string>) (x => x.Name)).ToList<MeasureUnitDTO>();
    }
  }
}
