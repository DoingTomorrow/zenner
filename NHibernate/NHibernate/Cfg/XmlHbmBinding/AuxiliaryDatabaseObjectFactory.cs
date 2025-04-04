// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.XmlHbmBinding.AuxiliaryDatabaseObjectFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping;
using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Cfg.XmlHbmBinding
{
  internal class AuxiliaryDatabaseObjectFactory
  {
    public static IAuxiliaryDatabaseObject Create(
      Mappings mappings,
      HbmDatabaseObject databaseObjectSchema)
    {
      return !databaseObjectSchema.HasDefinition() ? AuxiliaryDatabaseObjectFactory.CreateSimpleObject(databaseObjectSchema) : AuxiliaryDatabaseObjectFactory.CreateCustomObject(mappings, databaseObjectSchema);
    }

    private static IAuxiliaryDatabaseObject CreateSimpleObject(
      HbmDatabaseObject databaseObjectSchema)
    {
      IAuxiliaryDatabaseObject simpleObject = (IAuxiliaryDatabaseObject) new SimpleAuxiliaryDatabaseObject(databaseObjectSchema.FindCreateText(), databaseObjectSchema.FindDropText());
      foreach (string dialectScopeName in (IEnumerable<string>) databaseObjectSchema.FindDialectScopeNames())
        simpleObject.AddDialectScope(dialectScopeName);
      return simpleObject;
    }

    private static IAuxiliaryDatabaseObject CreateCustomObject(
      Mappings mappings,
      HbmDatabaseObject databaseObjectSchema)
    {
      HbmDefinition definition = databaseObjectSchema.FindDefinition();
      string type = definition.@class;
      try
      {
        IAuxiliaryDatabaseObject instance = (IAuxiliaryDatabaseObject) NHibernate.Cfg.Environment.BytecodeProvider.ObjectsFactory.CreateInstance(ReflectHelper.ClassForName(TypeNameParser.Parse(type, mappings.DefaultNamespace, mappings.DefaultAssembly).ToString()));
        foreach (string dialectScopeName in (IEnumerable<string>) databaseObjectSchema.FindDialectScopeNames())
          instance.AddDialectScope(dialectScopeName);
        instance.SetParameterValues(definition.FindParameterValues());
        return instance;
      }
      catch (TypeLoadException ex)
      {
        throw new MappingException(string.Format("Could not locate custom database object class [{0}].", (object) type), (Exception) ex);
      }
      catch (Exception ex)
      {
        throw new MappingException(string.Format("Could not instantiate custom database object class [{0}].", (object) type), ex);
      }
    }
  }
}
