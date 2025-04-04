// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.LocationCoordinates
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  public class LocationCoordinates
  {
    internal LocationCoordinates()
    {
    }

    internal LocationCoordinates(double latitude, double longitude)
    {
      this.Longitude = longitude;
      this.Latitude = latitude;
    }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    internal bool IsEmpty() => this.Latitude == 0.0 && this.Longitude == 0.0;

    internal bool IsValid()
    {
      return this.Latitude >= -90.0 && this.Latitude <= 90.0 && this.Longitude >= -180.0 && this.Longitude <= 180.0;
    }

    internal LocationCoordinates Copy()
    {
      return new LocationCoordinates()
      {
        Latitude = this.Latitude,
        Longitude = this.Longitude
      };
    }

    public override string ToString()
    {
      return string.Format("Lat: {0:0.#}, Long: {1:0.#}", (object) this.Latitude, (object) this.Longitude);
    }
  }
}
