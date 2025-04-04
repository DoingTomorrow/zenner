// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.EnumStringType`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class EnumStringType<T> : EnumStringType
  {
    private readonly string typeName;

    public EnumStringType()
      : base(typeof (T))
    {
      System.Type type = this.GetType();
      this.typeName = type.FullName + ", " + type.Assembly.GetName().Name;
    }

    public override string Name => this.typeName;
  }
}
