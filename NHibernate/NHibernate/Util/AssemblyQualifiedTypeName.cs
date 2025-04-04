// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.AssemblyQualifiedTypeName
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Util
{
  public class AssemblyQualifiedTypeName
  {
    private readonly string type;
    private readonly string assembly;
    private readonly int hashCode;

    public AssemblyQualifiedTypeName(string type, string assembly)
    {
      this.type = type != null ? type : throw new ArgumentNullException(nameof (type));
      this.assembly = assembly;
      this.hashCode = type.GetHashCode() * 397 ^ (assembly != null ? assembly.GetHashCode() : 0);
    }

    public string Type => this.type;

    public string Assembly => this.assembly;

    public override bool Equals(object obj) => this.Equals(obj as AssemblyQualifiedTypeName);

    public override string ToString()
    {
      return this.assembly == null ? this.type : this.type + ", " + this.assembly;
    }

    public bool Equals(AssemblyQualifiedTypeName obj)
    {
      if (obj == null)
        return false;
      if (object.ReferenceEquals((object) this, (object) obj))
        return true;
      return object.Equals((object) obj.type, (object) this.type) && object.Equals((object) obj.assembly, (object) this.assembly);
    }

    public override int GetHashCode() => this.hashCode;
  }
}
