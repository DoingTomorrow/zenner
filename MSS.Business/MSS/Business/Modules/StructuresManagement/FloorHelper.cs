// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.StructuresManagement.FloorHelper
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.Utils;
using MSS.Core.Model.MSSClient;
using MSS.DTO.Structures;
using MSS.Utils.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#nullable disable
namespace MSS.Business.Modules.StructuresManagement
{
  public static class FloorHelper
  {
    public static IEnumerable<FloorNameDTO> GetFloorNames()
    {
      ObservableCollection<FloorNameDTO> floorNames = new ObservableCollection<FloorNameDTO>();
      Dictionary<string, string> enumElements = EnumHelper.GetEnumElements<FloorNamesEnum>();
      int num = 1;
      foreach (KeyValuePair<string, string> keyValuePair in enumElements)
      {
        switch ((FloorNamesEnum) Enum.Parse(typeof (FloorNamesEnum), keyValuePair.Key, true))
        {
          case FloorNamesEnum.BM:
            floorNames.Add(new FloorNameDTO()
            {
              Id = num,
              FloorName = FloorNamesEnum.BM.GetStringValue(),
              FloorNameEnum = FloorNamesEnum.BM
            });
            break;
          case FloorNamesEnum.GF:
            floorNames.Add(new FloorNameDTO()
            {
              Id = num,
              FloorName = FloorNamesEnum.GF.GetStringValue(),
              FloorNameEnum = FloorNamesEnum.GF
            });
            break;
          case FloorNamesEnum.UF:
            floorNames.Add(new FloorNameDTO()
            {
              Id = num,
              FloorName = FloorNamesEnum.UF.GetStringValue(),
              FloorNameEnum = FloorNamesEnum.UF
            });
            break;
          case FloorNamesEnum.TF:
            floorNames.Add(new FloorNameDTO()
            {
              Id = num,
              FloorName = FloorNamesEnum.TF.GetStringValue(),
              FloorNameEnum = FloorNamesEnum.TF
            });
            break;
        }
        ++num;
      }
      return (IEnumerable<FloorNameDTO>) floorNames;
    }

    public static IEnumerable<DirectionDTO> GetDirections()
    {
      ObservableCollection<DirectionDTO> directions = new ObservableCollection<DirectionDTO>();
      Dictionary<string, string> enumElements = EnumHelper.GetEnumElements<DirectionsEnum>();
      int num = 1;
      foreach (KeyValuePair<string, string> keyValuePair in enumElements)
      {
        switch ((DirectionsEnum) Enum.Parse(typeof (DirectionsEnum), keyValuePair.Key, true))
        {
          case DirectionsEnum.Left:
            directions.Add(new DirectionDTO()
            {
              Id = num,
              Direction = DirectionsEnum.Left.GetStringValue(),
              DirectionEnum = DirectionsEnum.Left
            });
            break;
          case DirectionsEnum.Middle:
            directions.Add(new DirectionDTO()
            {
              Id = num,
              Direction = DirectionsEnum.Middle.GetStringValue(),
              DirectionEnum = DirectionsEnum.Middle
            });
            break;
          case DirectionsEnum.Right:
            directions.Add(new DirectionDTO()
            {
              Id = num,
              Direction = DirectionsEnum.Right.GetStringValue(),
              DirectionEnum = DirectionsEnum.Right
            });
            break;
        }
        ++num;
      }
      return (IEnumerable<DirectionDTO>) directions;
    }
  }
}
