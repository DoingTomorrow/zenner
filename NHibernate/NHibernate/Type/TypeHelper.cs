// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.TypeHelper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Intercept;
using NHibernate.Properties;
using NHibernate.Tuple;
using System;
using System.Collections;

#nullable disable
namespace NHibernate.Type
{
  public static class TypeHelper
  {
    public static void DeepCopy(
      object[] values,
      IType[] types,
      bool[] copy,
      object[] target,
      ISessionImplementor session)
    {
      for (int index = 0; index < types.Length; ++index)
      {
        if (copy[index])
          target[index] = object.Equals(LazyPropertyInitializer.UnfetchedProperty, values[index]) || object.Equals(BackrefPropertyAccessor.Unknown, values[index]) ? values[index] : types[index].DeepCopy(values[index], session.EntityMode, session.Factory);
      }
    }

    public static void BeforeAssemble(
      object[] row,
      ICacheAssembler[] types,
      ISessionImplementor session)
    {
      for (int index = 0; index < types.Length; ++index)
      {
        if (!object.Equals(LazyPropertyInitializer.UnfetchedProperty, row[index]) && !object.Equals(BackrefPropertyAccessor.Unknown, row[index]))
          types[index].BeforeAssemble(row[index], session);
      }
    }

    public static object[] Assemble(
      object[] row,
      ICacheAssembler[] types,
      ISessionImplementor session,
      object owner)
    {
      object[] objArray = new object[row.Length];
      for (int index = 0; index < row.Length; ++index)
        objArray[index] = object.Equals(LazyPropertyInitializer.UnfetchedProperty, row[index]) || object.Equals(BackrefPropertyAccessor.Unknown, row[index]) ? row[index] : types[index].Assemble(row[index], session, owner);
      return objArray;
    }

    public static object[] Disassemble(
      object[] row,
      ICacheAssembler[] types,
      bool[] nonCacheable,
      ISessionImplementor session,
      object owner)
    {
      object[] objArray = new object[row.Length];
      for (int index = 0; index < row.Length; ++index)
        objArray[index] = nonCacheable == null || !nonCacheable[index] ? (object.Equals(LazyPropertyInitializer.UnfetchedProperty, row[index]) || object.Equals(BackrefPropertyAccessor.Unknown, row[index]) ? row[index] : types[index].Disassemble(row[index], session, owner)) : LazyPropertyInitializer.UnfetchedProperty;
      return objArray;
    }

    public static object[] Replace(
      object[] original,
      object[] target,
      IType[] types,
      ISessionImplementor session,
      object owner,
      IDictionary copiedAlready)
    {
      object[] objArray = new object[original.Length];
      for (int index = 0; index < original.Length; ++index)
        objArray[index] = object.Equals(LazyPropertyInitializer.UnfetchedProperty, original[index]) || object.Equals(BackrefPropertyAccessor.Unknown, original[index]) ? target[index] : types[index].Replace(original[index], target[index], session, owner, copiedAlready);
      return objArray;
    }

    public static object[] Replace(
      object[] original,
      object[] target,
      IType[] types,
      ISessionImplementor session,
      object owner,
      IDictionary copyCache,
      ForeignKeyDirection foreignKeyDirection)
    {
      object[] objArray = new object[original.Length];
      for (int index = 0; index < types.Length; ++index)
        objArray[index] = object.Equals(LazyPropertyInitializer.UnfetchedProperty, original[index]) || object.Equals(BackrefPropertyAccessor.Unknown, original[index]) ? target[index] : types[index].Replace(original[index], target[index], session, owner, copyCache, foreignKeyDirection);
      return objArray;
    }

    public static object[] ReplaceAssociations(
      object[] original,
      object[] target,
      IType[] types,
      ISessionImplementor session,
      object owner,
      IDictionary copyCache,
      ForeignKeyDirection foreignKeyDirection)
    {
      object[] objArray = new object[original.Length];
      for (int index = 0; index < types.Length; ++index)
      {
        if (object.Equals(LazyPropertyInitializer.UnfetchedProperty, original[index]) || object.Equals(BackrefPropertyAccessor.Unknown, original[index]))
          objArray[index] = target[index];
        else if (types[index].IsComponentType)
        {
          IAbstractComponentType type = (IAbstractComponentType) types[index];
          IType[] subtypes = type.Subtypes;
          object[] values = TypeHelper.ReplaceAssociations(original[index] == null ? new object[subtypes.Length] : type.GetPropertyValues(original[index], session), target[index] == null ? new object[subtypes.Length] : type.GetPropertyValues(target[index], session), subtypes, session, (object) null, copyCache, foreignKeyDirection);
          if (!type.IsAnyType && target[index] != null)
            type.SetPropertyValues(target[index], values, session.EntityMode);
          objArray[index] = target[index];
        }
        else
          objArray[index] = types[index].IsAssociationType ? types[index].Replace(original[index], target[index], session, owner, copyCache, foreignKeyDirection) : target[index];
      }
      return objArray;
    }

    public static int[] FindDirty(
      StandardProperty[] properties,
      object[] currentState,
      object[] previousState,
      bool[][] includeColumns,
      bool anyUninitializedProperties,
      ISessionImplementor session)
    {
      int[] sourceArray = (int[]) null;
      int length1 = 0;
      int length2 = properties.Length;
      for (int i = 0; i < length2; ++i)
      {
        if (TypeHelper.Dirty(properties, currentState, previousState, includeColumns, anyUninitializedProperties, session, i))
        {
          if (sourceArray == null)
            sourceArray = new int[length2];
          sourceArray[length1++] = i;
        }
      }
      if (length1 == 0)
        return (int[]) null;
      int[] destinationArray = new int[length1];
      Array.Copy((Array) sourceArray, 0, (Array) destinationArray, 0, length1);
      return destinationArray;
    }

    private static bool Dirty(
      StandardProperty[] properties,
      object[] currentState,
      object[] previousState,
      bool[][] includeColumns,
      bool anyUninitializedProperties,
      ISessionImplementor session,
      int i)
    {
      if (object.Equals(LazyPropertyInitializer.UnfetchedProperty, currentState[i]))
        return false;
      if (object.Equals(LazyPropertyInitializer.UnfetchedProperty, previousState[i]))
        return true;
      return properties[i].IsDirtyCheckable(anyUninitializedProperties) && properties[i].Type.IsDirty(previousState[i], currentState[i], includeColumns[i], session);
    }

    public static int[] FindModified(
      StandardProperty[] properties,
      object[] currentState,
      object[] previousState,
      bool[][] includeColumns,
      bool anyUninitializedProperties,
      ISessionImplementor session)
    {
      int[] sourceArray = (int[]) null;
      int length1 = 0;
      int length2 = properties.Length;
      for (int index = 0; index < length2; ++index)
      {
        if (!object.Equals(LazyPropertyInitializer.UnfetchedProperty, currentState[index]) && properties[index].IsDirtyCheckable(anyUninitializedProperties) && properties[index].Type.IsModified(previousState[index], currentState[index], includeColumns[index], session))
        {
          if (sourceArray == null)
            sourceArray = new int[length2];
          sourceArray[length1++] = index;
        }
      }
      if (length1 == 0)
        return (int[]) null;
      int[] destinationArray = new int[length1];
      Array.Copy((Array) sourceArray, 0, (Array) destinationArray, 0, length1);
      return destinationArray;
    }
  }
}
