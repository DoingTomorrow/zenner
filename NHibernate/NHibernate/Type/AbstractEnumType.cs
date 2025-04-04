// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.AbstractEnumType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlTypes;
using System;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public abstract class AbstractEnumType : 
    PrimitiveType,
    IDiscriminatorType,
    IIdentifierType,
    IType,
    ICacheAssembler,
    ILiteralType
  {
    private readonly object defaultValue;
    private readonly System.Type enumType;

    protected AbstractEnumType(SqlType sqlType, System.Type enumType)
      : base(sqlType)
    {
      this.enumType = enumType.IsEnum ? enumType : throw new MappingException(enumType.Name + " did not inherit from System.Enum");
      this.defaultValue = Enum.GetValues(enumType).GetValue(0);
    }

    public override System.Type ReturnedClass => this.enumType;

    public object StringToObject(string xml) => Enum.Parse(this.enumType, xml);

    public override object FromStringValue(string xml) => this.StringToObject(xml);

    public override System.Type PrimitiveClass => this.enumType;

    public override object DefaultValue => this.defaultValue;
  }
}
