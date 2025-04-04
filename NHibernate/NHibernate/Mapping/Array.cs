// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.Array
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;
using NHibernate.Util;
using System;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public class Array(PersistentClass owner) : List(owner)
  {
    private System.Type elementClass;
    private string elementClassName;

    public System.Type ElementClass
    {
      get
      {
        if (this.elementClass == null)
        {
          if (this.elementClassName == null)
          {
            IType type = this.Element.Type;
            this.elementClass = this.IsPrimitiveArray ? ((PrimitiveType) type).PrimitiveClass : type.ReturnedClass;
          }
          else
          {
            try
            {
              this.elementClass = ReflectHelper.ClassForName(this.elementClassName);
            }
            catch (Exception ex)
            {
              throw new MappingException(ex);
            }
          }
        }
        return this.elementClass;
      }
    }

    public override CollectionType DefaultCollectionType
    {
      get
      {
        return TypeFactory.Array(this.Role, this.ReferencedPropertyName, this.Embedded, this.ElementClass);
      }
    }

    public override bool IsArray => true;

    public string ElementClassName
    {
      get => this.elementClassName;
      set
      {
        if ((this.elementClassName != null || value == null) && (this.elementClassName == null || this.elementClassName.Equals(value)))
          return;
        this.elementClassName = value;
        this.elementClass = (System.Type) null;
      }
    }
  }
}
