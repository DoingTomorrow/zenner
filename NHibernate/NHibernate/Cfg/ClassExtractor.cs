// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.ClassExtractor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using NHibernate.Util;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Cfg
{
  public class ClassExtractor
  {
    public static ICollection<ClassExtractor.ClassEntry> GetClassEntries(HbmMapping document)
    {
      HashSet<ClassExtractor.ClassEntry> classEntries = new HashSet<ClassExtractor.ClassEntry>();
      string assembly = document.assembly;
      string @namespace = document.@namespace;
      classEntries.UnionWith(ClassExtractor.GetRootClassesEntries(assembly, @namespace, (IEnumerable<HbmClass>) document.RootClasses));
      classEntries.UnionWith(ClassExtractor.GetSubclassesEntries(assembly, @namespace, (string) null, (IEnumerable<HbmSubclass>) document.SubClasses));
      classEntries.UnionWith(ClassExtractor.GetJoinedSubclassesEntries(assembly, @namespace, (string) null, (IEnumerable<HbmJoinedSubclass>) document.JoinedSubclasses));
      classEntries.UnionWith(ClassExtractor.GetUnionSubclassesEntries(assembly, @namespace, (string) null, (IEnumerable<HbmUnionSubclass>) document.UnionSubclasses));
      return (ICollection<ClassExtractor.ClassEntry>) classEntries;
    }

    private static IEnumerable<ClassExtractor.ClassEntry> GetRootClassesEntries(
      string assembly,
      string @namespace,
      IEnumerable<HbmClass> rootClasses)
    {
      foreach (HbmClass rootClass in rootClasses)
      {
        string entityName = rootClass.EntityName;
        yield return new ClassExtractor.ClassEntry((string) null, rootClass.Name, entityName, assembly, @namespace);
        foreach (ClassExtractor.ClassEntry classEntry in ClassExtractor.GetSubclassesEntries(assembly, @namespace, entityName, rootClass.Subclasses))
          yield return classEntry;
        foreach (ClassExtractor.ClassEntry classEntry in ClassExtractor.GetJoinedSubclassesEntries(assembly, @namespace, entityName, rootClass.JoinedSubclasses))
          yield return classEntry;
        foreach (ClassExtractor.ClassEntry classEntry in ClassExtractor.GetUnionSubclassesEntries(assembly, @namespace, entityName, rootClass.UnionSubclasses))
          yield return classEntry;
      }
    }

    private static IEnumerable<ClassExtractor.ClassEntry> GetSubclassesEntries(
      string assembly,
      string @namespace,
      string defaultExtends,
      IEnumerable<HbmSubclass> hbmSubclasses)
    {
      foreach (HbmSubclass subclass in hbmSubclasses)
      {
        string extends = subclass.extends ?? defaultExtends;
        yield return new ClassExtractor.ClassEntry(extends, subclass.Name, subclass.EntityName, assembly, @namespace);
        foreach (ClassExtractor.ClassEntry classEntry in ClassExtractor.GetSubclassesEntries(assembly, @namespace, subclass.EntityName, subclass.Subclasses))
          yield return classEntry;
      }
    }

    private static IEnumerable<ClassExtractor.ClassEntry> GetJoinedSubclassesEntries(
      string assembly,
      string @namespace,
      string defaultExtends,
      IEnumerable<HbmJoinedSubclass> hbmJoinedSubclasses)
    {
      foreach (HbmJoinedSubclass subclass in hbmJoinedSubclasses)
      {
        string extends = subclass.extends ?? defaultExtends;
        yield return new ClassExtractor.ClassEntry(extends, subclass.Name, subclass.EntityName, assembly, @namespace);
        foreach (ClassExtractor.ClassEntry classEntry in ClassExtractor.GetJoinedSubclassesEntries(assembly, @namespace, subclass.EntityName, subclass.JoinedSubclasses))
          yield return classEntry;
      }
    }

    private static IEnumerable<ClassExtractor.ClassEntry> GetUnionSubclassesEntries(
      string assembly,
      string @namespace,
      string defaultExtends,
      IEnumerable<HbmUnionSubclass> hbmUnionSubclasses)
    {
      foreach (HbmUnionSubclass subclass in hbmUnionSubclasses)
      {
        string extends = subclass.extends ?? defaultExtends;
        yield return new ClassExtractor.ClassEntry(extends, subclass.Name, subclass.EntityName, assembly, @namespace);
        foreach (ClassExtractor.ClassEntry classEntry in ClassExtractor.GetUnionSubclassesEntries(assembly, @namespace, subclass.EntityName, subclass.UnionSubclasses))
          yield return classEntry;
      }
    }

    public class ClassEntry
    {
      private readonly string entityName;
      private readonly string extendsEntityName;
      private readonly AssemblyQualifiedTypeName fullExtends;
      private readonly AssemblyQualifiedTypeName fullClassName;
      private readonly int hashCode;

      public ClassEntry(
        string extends,
        string className,
        string entityName,
        string assembly,
        string @namespace)
      {
        this.fullExtends = string.IsNullOrEmpty(extends) ? (AssemblyQualifiedTypeName) null : TypeNameParser.Parse(extends, @namespace, assembly);
        this.fullClassName = string.IsNullOrEmpty(className) ? (AssemblyQualifiedTypeName) null : TypeNameParser.Parse(className, @namespace, assembly);
        this.entityName = entityName;
        this.extendsEntityName = string.IsNullOrEmpty(extends) ? (string) null : extends;
        this.hashCode = entityName != null ? entityName.GetHashCode() : 0;
        this.hashCode = this.hashCode * 397 ^ (this.fullExtends != null ? this.fullExtends.GetHashCode() : 0);
        this.hashCode = this.hashCode * 397 ^ (this.fullClassName != null ? this.fullClassName.GetHashCode() : 0);
      }

      public AssemblyQualifiedTypeName FullExtends => this.fullExtends;

      public AssemblyQualifiedTypeName FullClassName => this.fullClassName;

      public string EntityName => this.entityName;

      public string ExtendsEntityName => this.extendsEntityName;

      public override bool Equals(object obj) => this.Equals(obj as ClassExtractor.ClassEntry);

      public bool Equals(ClassExtractor.ClassEntry obj)
      {
        if (obj == null)
          return false;
        if (object.ReferenceEquals((object) this, (object) obj))
          return true;
        return object.Equals((object) obj.entityName, (object) this.entityName) && object.Equals((object) obj.fullExtends, (object) this.fullExtends) && object.Equals((object) obj.fullClassName, (object) this.fullClassName);
      }

      public override int GetHashCode() => this.hashCode;
    }
  }
}
