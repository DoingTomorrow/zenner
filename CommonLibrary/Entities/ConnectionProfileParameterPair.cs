// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.Entities.ConnectionProfileParameterPair
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

#nullable disable
namespace ZENNER.CommonLibrary.Entities
{
  public class ConnectionProfileParameterPair
  {
    public ConnectionProfileParameter ParameterName;
    public string ParameterValue;

    public ConnectionProfileParameterPair(
      ConnectionProfileParameter parameterName,
      string parameterValue)
    {
      this.ParameterName = parameterName;
      this.ParameterValue = parameterValue;
    }

    public override string ToString()
    {
      return this.ParameterValue == null ? this.ParameterName.ToString() : this.ParameterName.ToString() + "=" + this.ParameterValue;
    }
  }
}
