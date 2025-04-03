// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.Entities.Location
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;

#nullable disable
namespace ZENNER.CommonLibrary.Entities
{
  [Serializable]
  public class Location
  {
    public int ID { get; set; }

    public string Country { get; set; }

    public string Region { get; set; }

    public string City { get; set; }

    public string Zip { get; set; }

    public string Street { get; set; }

    public string Floor { get; set; }

    public string HouseNumber { get; set; }

    public string RoomNumber { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string Description { get; set; }

    public string Name
    {
      get
      {
        return string.Format("{0} {1} {2} {3} {4} {5} {6}", (object) this.Country, (object) this.Region, (object) this.City, (object) this.Zip, (object) this.Street, (object) this.Floor, (object) this.HouseNumber);
      }
    }
  }
}
