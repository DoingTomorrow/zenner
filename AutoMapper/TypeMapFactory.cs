// Decompiled with JetBrains decompiler
// Type: AutoMapper.TypeMapFactory
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using AutoMapper.Impl;
using AutoMapper.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

#nullable disable
namespace AutoMapper
{
  public class TypeMapFactory : ITypeMapFactory
  {
    private static readonly IDictionaryFactory DictionaryFactory = PlatformAdapter.Resolve<IDictionaryFactory>();
    private static readonly AutoMapper.Internal.IDictionary<Type, TypeInfo> _typeInfos = TypeMapFactory.DictionaryFactory.CreateDictionary<Type, TypeInfo>();

    public TypeMap CreateTypeMap(
      Type sourceType,
      Type destinationType,
      IMappingOptions options,
      MemberList memberList)
    {
      TypeInfo typeInfo1 = TypeMapFactory.GetTypeInfo(sourceType, options.SourceExtensionMethods);
      TypeInfo typeInfo2 = TypeMapFactory.GetTypeInfo(destinationType, (IEnumerable<MethodInfo>) new MethodInfo[0]);
      TypeMap typeMap = new TypeMap(typeInfo1, typeInfo2, memberList);
      foreach (MemberInfo publicWriteAccessor in typeInfo2.GetPublicWriteAccessors())
      {
        LinkedList<MemberInfo> linkedList = new LinkedList<MemberInfo>();
        if (this.MapDestinationPropertyToSource(linkedList, typeInfo1, publicWriteAccessor.Name, options))
        {
          IEnumerable<IMemberGetter> source = linkedList.Select<MemberInfo, IMemberGetter>((Func<MemberInfo, IMemberGetter>) (mi => mi.ToMemberGetter()));
          IMemberAccessor memberAccessor = publicWriteAccessor.ToMemberAccessor();
          typeMap.AddPropertyMap(memberAccessor, source.Cast<IValueResolver>());
        }
      }
      if (!destinationType.IsAbstract && destinationType.IsClass)
      {
        foreach (ConstructorInfo destCtor in (IEnumerable<ConstructorInfo>) typeInfo2.GetConstructors().OrderByDescending<ConstructorInfo, int>((Func<ConstructorInfo, int>) (ci => ci.GetParameters().Length)))
        {
          if (this.MapDestinationCtorToSource(typeMap, destCtor, typeInfo1, options))
            break;
        }
      }
      return typeMap;
    }

    private bool MapDestinationCtorToSource(
      TypeMap typeMap,
      ConstructorInfo destCtor,
      TypeInfo sourceTypeInfo,
      IMappingOptions options)
    {
      List<ConstructorParameterMap> parameters1 = new List<ConstructorParameterMap>();
      ParameterInfo[] parameters2 = destCtor.GetParameters();
      if (parameters2.Length == 0 || !options.ConstructorMappingEnabled)
        return false;
      foreach (ParameterInfo parameter in parameters2)
      {
        LinkedList<MemberInfo> linkedList = new LinkedList<MemberInfo>();
        if (!this.MapDestinationPropertyToSource(linkedList, sourceTypeInfo, parameter.Name, options))
          return false;
        IEnumerable<IMemberGetter> source = linkedList.Select<MemberInfo, IMemberGetter>((Func<MemberInfo, IMemberGetter>) (mi => mi.ToMemberGetter()));
        ConstructorParameterMap constructorParameterMap = new ConstructorParameterMap(parameter, source.ToArray<IMemberGetter>());
        parameters1.Add(constructorParameterMap);
      }
      typeMap.AddConstructorMap(destCtor, (IEnumerable<ConstructorParameterMap>) parameters1);
      return true;
    }

    private static TypeInfo GetTypeInfo(Type type, IEnumerable<MethodInfo> extensionMethodsToSearch)
    {
      return TypeMapFactory._typeInfos.GetOrAdd(type, (Func<Type, TypeInfo>) (t => new TypeInfo(type, extensionMethodsToSearch)));
    }

    private bool MapDestinationPropertyToSource(
      LinkedList<MemberInfo> resolvers,
      TypeInfo sourceType,
      string nameToSearch,
      IMappingOptions mappingOptions)
    {
      if (string.IsNullOrEmpty(nameToSearch))
        return true;
      IEnumerable<MemberInfo> publicReadAccessors = sourceType.GetPublicReadAccessors();
      IEnumerable<MethodInfo> publicNoArgMethods = sourceType.GetPublicNoArgMethods();
      IEnumerable<MethodInfo> extensionMethods = sourceType.GetPublicNoArgExtensionMethods();
      MemberInfo typeMember1 = TypeMapFactory.FindTypeMember(publicReadAccessors, publicNoArgMethods, extensionMethods, nameToSearch, mappingOptions);
      bool source = (object) typeMember1 != null;
      if (source)
      {
        resolvers.AddLast(typeMember1);
      }
      else
      {
        string[] array = mappingOptions.DestinationMemberNamingConvention.SplittingExpression.Matches(nameToSearch).Cast<Match>().Select<Match, string>((Func<Match, string>) (m => m.Value)).ToArray<string>();
        for (int i = 1; i <= array.Length && !source; ++i)
        {
          TypeMapFactory.NameSnippet nameSnippet = this.CreateNameSnippet((IEnumerable<string>) array, i, mappingOptions);
          MemberInfo typeMember2 = TypeMapFactory.FindTypeMember(publicReadAccessors, publicNoArgMethods, extensionMethods, nameSnippet.First, mappingOptions);
          if ((object) typeMember2 != null)
          {
            resolvers.AddLast(typeMember2);
            source = this.MapDestinationPropertyToSource(resolvers, TypeMapFactory.GetTypeInfo(typeMember2.GetMemberType(), mappingOptions.SourceExtensionMethods), nameSnippet.Second, mappingOptions);
            if (!source)
              resolvers.RemoveLast();
          }
        }
      }
      return source;
    }

    private static MemberInfo FindTypeMember(
      IEnumerable<MemberInfo> modelProperties,
      IEnumerable<MethodInfo> getMethods,
      IEnumerable<MethodInfo> getExtensionMethods,
      string nameToSearch,
      IMappingOptions mappingOptions)
    {
      MemberInfo typeMember1 = modelProperties.FirstOrDefault<MemberInfo>((Func<MemberInfo, bool>) (prop => TypeMapFactory.NameMatches(prop.Name, nameToSearch, mappingOptions)));
      if ((object) typeMember1 != null)
        return typeMember1;
      MethodInfo typeMember2 = getMethods.FirstOrDefault<MethodInfo>((Func<MethodInfo, bool>) (m => TypeMapFactory.NameMatches(m.Name, nameToSearch, mappingOptions)));
      if ((object) typeMember2 != null)
        return (MemberInfo) typeMember2;
      MethodInfo methodInfo = getExtensionMethods.FirstOrDefault<MethodInfo>((Func<MethodInfo, bool>) (m => TypeMapFactory.NameMatches(m.Name, nameToSearch, mappingOptions)));
      return (object) methodInfo != null ? (MemberInfo) methodInfo : (MemberInfo) null;
    }

    private static bool NameMatches(
      string memberName,
      string nameToMatch,
      IMappingOptions mappingOptions)
    {
      IEnumerable<string> source = TypeMapFactory.PossibleNames(memberName, mappingOptions.Aliases, mappingOptions.Prefixes, mappingOptions.Postfixes);
      IEnumerable<string> possibleDestNames = TypeMapFactory.PossibleNames(nameToMatch, mappingOptions.Aliases, mappingOptions.DestinationPrefixes, mappingOptions.DestinationPostfixes);
      return source.SelectMany((Func<string, IEnumerable<string>>) (sourceName => possibleDestNames), (sourceName, destName) => new
      {
        sourceName = sourceName,
        destName = destName
      }).Any(pair => string.Compare(pair.sourceName, pair.destName, StringComparison.OrdinalIgnoreCase) == 0);
    }

    private static IEnumerable<string> PossibleNames(
      string memberName,
      IEnumerable<AliasedMember> aliases,
      IEnumerable<string> prefixes,
      IEnumerable<string> postfixes)
    {
      if (!string.IsNullOrEmpty(memberName))
      {
        yield return memberName;
        foreach (AliasedMember alias in aliases.Where<AliasedMember>((Func<AliasedMember, bool>) (alias => string.Equals(memberName, alias.Member, StringComparison.Ordinal))))
          yield return alias.Alias;
        foreach (string prefix in prefixes.Where<string>((Func<string, bool>) (prefix => memberName.StartsWith(prefix, StringComparison.Ordinal))))
        {
          string withoutPrefix = memberName.Substring(prefix.Length);
          yield return withoutPrefix;
          foreach (string postfix in postfixes.Where<string>((Func<string, bool>) (postfix => withoutPrefix.EndsWith(postfix, StringComparison.Ordinal))))
            yield return withoutPrefix.Remove(withoutPrefix.Length - postfix.Length);
        }
        foreach (string postfix in postfixes.Where<string>((Func<string, bool>) (postfix => memberName.EndsWith(postfix, StringComparison.Ordinal))))
          yield return memberName.Remove(memberName.Length - postfix.Length);
      }
    }

    private TypeMapFactory.NameSnippet CreateNameSnippet(
      IEnumerable<string> matches,
      int i,
      IMappingOptions mappingOptions)
    {
      return new TypeMapFactory.NameSnippet()
      {
        First = string.Join(mappingOptions.SourceMemberNamingConvention.SeparatorCharacter, matches.Take<string>(i).ToArray<string>()),
        Second = string.Join(mappingOptions.SourceMemberNamingConvention.SeparatorCharacter, matches.Skip<string>(i).ToArray<string>())
      };
    }

    private class NameSnippet
    {
      public string First { get; set; }

      public string Second { get; set; }
    }
  }
}
