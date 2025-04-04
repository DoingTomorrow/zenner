// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.UnsavedValueFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Properties;
using NHibernate.Type;
using System;
using System.Reflection;

#nullable disable
namespace NHibernate.Engine
{
  public sealed class UnsavedValueFactory
  {
    private static readonly object[] NoParameters = new object[0];

    private UnsavedValueFactory()
    {
    }

    private static object Instantiate(ConstructorInfo constructor)
    {
      try
      {
        return constructor.Invoke(UnsavedValueFactory.NoParameters);
      }
      catch (Exception ex)
      {
        throw new InstantiationException("could not instantiate test object: ", ex, constructor.DeclaringType);
      }
    }

    public static IdentifierValue GetUnsavedIdentifierValue(
      string unsavedValue,
      IGetter identifierGetter,
      IType identifierType,
      ConstructorInfo constructor)
    {
      if (unsavedValue == null)
      {
        if (identifierGetter != null && constructor != null)
          return new IdentifierValue(identifierGetter.Get(UnsavedValueFactory.Instantiate(constructor)));
        PrimitiveType primitiveType = identifierType as PrimitiveType;
        return identifierGetter != null && primitiveType != null ? new IdentifierValue(primitiveType.DefaultValue) : IdentifierValue.SaveNull;
      }
      if ("null" == unsavedValue)
        return IdentifierValue.SaveNull;
      if ("undefined" == unsavedValue)
        return IdentifierValue.Undefined;
      if ("none" == unsavedValue)
        return IdentifierValue.SaveNone;
      if ("any" == unsavedValue)
        return IdentifierValue.SaveAny;
      try
      {
        return new IdentifierValue(((IIdentifierType) identifierType).StringToObject(unsavedValue));
      }
      catch (InvalidCastException ex)
      {
        throw new MappingException("Bad identifier type: " + identifierType.Name, (Exception) ex);
      }
      catch (Exception ex)
      {
        throw new MappingException("Could not parse identifier unsaved-value: " + unsavedValue, ex);
      }
    }

    public static VersionValue GetUnsavedVersionValue(
      string versionUnsavedValue,
      IGetter versionGetter,
      IVersionType versionType,
      ConstructorInfo constructor)
    {
      if (versionUnsavedValue == null)
      {
        if (constructor == null)
          return VersionValue.VersionUndefined;
        object y = versionGetter.Get(UnsavedValueFactory.Instantiate(constructor));
        if (y != null && y.GetType().IsValueType)
          return new VersionValue(y);
        return !versionType.IsEqual(versionType.Seed((ISessionImplementor) null), y) ? new VersionValue(y) : VersionValue.VersionUndefined;
      }
      if ("undefined" == versionUnsavedValue)
        return VersionValue.VersionUndefined;
      if ("null" == versionUnsavedValue)
        return VersionValue.VersionSaveNull;
      if ("negative" == versionUnsavedValue)
        return VersionValue.VersionNegative;
      try
      {
        return new VersionValue(versionType.FromStringValue(versionUnsavedValue));
      }
      catch (InvalidCastException ex)
      {
        throw new MappingException("Bad version type: " + versionType.Name, (Exception) ex);
      }
      catch (Exception ex)
      {
        throw new MappingException("Could not parse version unsaved-value: " + versionUnsavedValue, ex);
      }
    }
  }
}
