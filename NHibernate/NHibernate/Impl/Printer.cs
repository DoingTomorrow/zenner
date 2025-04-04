// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.Printer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Metadata;
using NHibernate.Type;
using NHibernate.Util;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Impl
{
  public sealed class Printer
  {
    private readonly ISessionFactoryImplementor _factory;
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (Printer));

    public string ToString(object entity, EntityMode entityMode)
    {
      IClassMetadata classMetadata = this._factory.GetClassMetadata(entity.GetType());
      if (classMetadata == null)
        return entity.GetType().FullName;
      IDictionary<string, string> dictionary = (IDictionary<string, string>) new Dictionary<string, string>();
      if (classMetadata.HasIdentifierProperty)
        dictionary[classMetadata.IdentifierPropertyName] = classMetadata.IdentifierType.ToLoggableString(classMetadata.GetIdentifier(entity, entityMode), this._factory);
      IType[] propertyTypes = classMetadata.PropertyTypes;
      string[] propertyNames = classMetadata.PropertyNames;
      object[] propertyValues = classMetadata.GetPropertyValues(entity, entityMode);
      for (int index = 0; index < propertyTypes.Length; ++index)
        dictionary[propertyNames[index]] = propertyTypes[index].ToLoggableString(propertyValues[index], this._factory);
      return classMetadata.EntityName + CollectionPrinter.ToString(dictionary);
    }

    public string ToString(IType[] types, object[] values)
    {
      List<string> stringList = new List<string>(types.Length);
      for (int index = 0; index < types.Length; ++index)
      {
        if (types[index] != null)
          stringList.Add(types[index].ToLoggableString(values[index], this._factory));
      }
      return CollectionPrinter.ToString((IList) stringList);
    }

    public string ToString(IDictionary<string, TypedValue> namedTypedValues)
    {
      IDictionary<string, string> dictionary = (IDictionary<string, string>) new Dictionary<string, string>(namedTypedValues.Count);
      foreach (KeyValuePair<string, TypedValue> namedTypedValue in (IEnumerable<KeyValuePair<string, TypedValue>>) namedTypedValues)
      {
        TypedValue typedValue = namedTypedValue.Value;
        dictionary[namedTypedValue.Key] = typedValue.Type.ToLoggableString(typedValue.Value, this._factory);
      }
      return CollectionPrinter.ToString(dictionary);
    }

    public void ToString(IEnumerator enumerator, EntityMode entityMode)
    {
      if (!Printer.log.IsDebugEnabled || !enumerator.MoveNext())
        return;
      Printer.log.Debug((object) "listing entities:");
      int num = 0;
      while (num++ <= 20)
      {
        Printer.log.Debug((object) this.ToString(enumerator.Current, entityMode));
        if (!enumerator.MoveNext())
          return;
      }
      Printer.log.Debug((object) "more......");
    }

    public Printer(ISessionFactoryImplementor factory) => this._factory = factory;
  }
}
