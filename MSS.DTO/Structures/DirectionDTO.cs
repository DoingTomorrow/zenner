// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Structures.DirectionDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.Core.Model.MSSClient;

#nullable disable
namespace MSS.DTO.Structures
{
  public class DirectionDTO
  {
    public int Id { get; set; }

    public string Direction { get; set; }

    public DirectionsEnum DirectionEnum { get; set; }
  }
}
