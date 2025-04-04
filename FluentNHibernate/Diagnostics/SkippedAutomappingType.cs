// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Diagnostics.SkippedAutomappingType
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.Diagnostics
{
  public class SkippedAutomappingType
  {
    public Type Type { get; set; }

    public string Reason { get; set; }

    public bool Equals(SkippedAutomappingType other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return object.Equals((object) other.Type, (object) this.Type) && object.Equals((object) other.Reason, (object) this.Reason);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return obj.GetType() == typeof (SkippedAutomappingType) && this.Equals((SkippedAutomappingType) obj);
    }

    public override int GetHashCode()
    {
      return (this.Type != null ? this.Type.GetHashCode() : 0) * 397 ^ (this.Reason != null ? this.Reason.GetHashCode() : 0);
    }
  }
}
