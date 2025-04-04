// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Meta.TypeFormatEventArgs
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using System;

#nullable disable
namespace ProtoBuf.Meta
{
  public class TypeFormatEventArgs : EventArgs
  {
    private Type type;
    private string formattedName;
    private readonly bool typeFixed;

    public Type Type
    {
      get => this.type;
      set
      {
        if (!(this.type != value))
          return;
        if (this.typeFixed)
          throw new InvalidOperationException("The type is fixed and cannot be changed");
        this.type = value;
      }
    }

    public string FormattedName
    {
      get => this.formattedName;
      set
      {
        if (!(this.formattedName != value))
          return;
        if (!this.typeFixed)
          throw new InvalidOperationException("The formatted-name is fixed and cannot be changed");
        this.formattedName = value;
      }
    }

    internal TypeFormatEventArgs(string formattedName)
    {
      this.formattedName = !Helpers.IsNullOrEmpty(formattedName) ? formattedName : throw new ArgumentNullException(nameof (formattedName));
    }

    internal TypeFormatEventArgs(Type type)
    {
      this.type = !(type == (Type) null) ? type : throw new ArgumentNullException(nameof (type));
      this.typeFixed = true;
    }
  }
}
