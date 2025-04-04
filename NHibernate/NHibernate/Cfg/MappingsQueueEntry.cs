// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingsQueueEntry
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections.Generic;

#nullable disable
namespace NHibernate.Cfg
{
  public class MappingsQueueEntry
  {
    private readonly HashSet<string> containedClassNames;
    private readonly NamedXmlDocument document;
    private readonly HashSet<MappingsQueueEntry.RequiredEntityName> requiredClassNames;

    public MappingsQueueEntry(
      NamedXmlDocument document,
      IEnumerable<ClassExtractor.ClassEntry> classEntries)
    {
      this.document = document;
      this.containedClassNames = MappingsQueueEntry.GetClassNames(classEntries);
      this.requiredClassNames = MappingsQueueEntry.GetRequiredClassNames(classEntries, (ICollection<string>) this.containedClassNames);
    }

    public NamedXmlDocument Document => this.document;

    public ICollection<MappingsQueueEntry.RequiredEntityName> RequiredClassNames
    {
      get => (ICollection<MappingsQueueEntry.RequiredEntityName>) this.requiredClassNames;
    }

    public ICollection<string> ContainedClassNames
    {
      get => (ICollection<string>) this.containedClassNames;
    }

    private static HashSet<string> GetClassNames(
      IEnumerable<ClassExtractor.ClassEntry> classEntries)
    {
      HashSet<string> classNames = new HashSet<string>();
      foreach (ClassExtractor.ClassEntry classEntry in classEntries)
      {
        if (classEntry.EntityName != null)
          classNames.Add(classEntry.EntityName);
        else if (classEntry.FullClassName != null)
          classNames.Add(classEntry.FullClassName.Type);
      }
      return classNames;
    }

    private static HashSet<MappingsQueueEntry.RequiredEntityName> GetRequiredClassNames(
      IEnumerable<ClassExtractor.ClassEntry> classEntries,
      ICollection<string> containedNames)
    {
      HashSet<MappingsQueueEntry.RequiredEntityName> requiredClassNames = new HashSet<MappingsQueueEntry.RequiredEntityName>();
      foreach (ClassExtractor.ClassEntry classEntry in classEntries)
      {
        if (classEntry.ExtendsEntityName != null && !containedNames.Contains(classEntry.FullExtends.Type) && !containedNames.Contains(classEntry.ExtendsEntityName))
          requiredClassNames.Add(new MappingsQueueEntry.RequiredEntityName(classEntry.ExtendsEntityName, classEntry.FullExtends.Type));
      }
      return requiredClassNames;
    }

    public class RequiredEntityName
    {
      public RequiredEntityName(string entityName, string fullClassName)
      {
        this.EntityName = entityName;
        this.FullClassName = fullClassName;
      }

      public string EntityName { get; private set; }

      public string FullClassName { get; private set; }

      public bool Equals(MappingsQueueEntry.RequiredEntityName obj)
      {
        if (obj == null)
          return false;
        if (object.ReferenceEquals((object) this, (object) obj))
          return true;
        return object.Equals((object) obj.EntityName, (object) this.EntityName) && object.Equals((object) obj.FullClassName, (object) this.FullClassName);
      }

      public override bool Equals(object obj)
      {
        switch (obj)
        {
          case null:
            return false;
          case string str when str.Equals(this.EntityName) || str.Equals(this.FullClassName):
            return true;
          case MappingsQueueEntry.RequiredEntityName requiredEntityName:
            return false;
          default:
            return this.Equals(requiredEntityName);
        }
      }

      public override int GetHashCode()
      {
        return (this.EntityName != null ? this.EntityName.GetHashCode() : 0) * 397 ^ (this.FullClassName != null ? this.FullClassName.GetHashCode() : 0);
      }

      public override string ToString()
      {
        return string.Format("FullName:{0} - Name:{1}", (object) (this.FullClassName ?? "<null>"), (object) (this.EntityName ?? "<null>"));
      }
    }
  }
}
