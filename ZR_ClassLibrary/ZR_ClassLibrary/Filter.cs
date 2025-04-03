// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.Filter
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

#nullable disable
namespace ZR_ClassLibrary
{
  public sealed class Filter
  {
    public int FilterId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string DisplayName
    {
      get => string.Format("{0} {1}", (object) this.FilterId, (object) this.Name).Trim();
    }

    public override string ToString() => this.DisplayName;

    public static string Empty => " ";
  }
}
