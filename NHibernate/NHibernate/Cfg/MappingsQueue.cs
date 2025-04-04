// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingsQueue
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace NHibernate.Cfg
{
  public class MappingsQueue
  {
    private readonly Queue availableEntries = new Queue();
    private readonly ISet<string> processedClassNames = (ISet<string>) new HashedSet<string>();
    private readonly List<MappingsQueueEntry> unavailableEntries = new List<MappingsQueueEntry>();

    public void AddDocument(NamedXmlDocument document)
    {
      this.AddEntry(new MappingsQueueEntry(document, (IEnumerable<ClassExtractor.ClassEntry>) ClassExtractor.GetClassEntries(document.Document)));
    }

    public NamedXmlDocument GetNextAvailableResource()
    {
      if (this.availableEntries.Count == 0)
        return (NamedXmlDocument) null;
      MappingsQueueEntry mappingsQueueEntry = (MappingsQueueEntry) this.availableEntries.Dequeue();
      this.AddProcessedClassNames(mappingsQueueEntry.ContainedClassNames);
      return mappingsQueueEntry.Document;
    }

    public void CheckNoUnavailableEntries()
    {
      if (this.unavailableEntries.Count > 0)
        throw new MappingException(MappingsQueue.FormatExceptionMessage((IEnumerable<MappingsQueueEntry>) this.unavailableEntries));
    }

    private void AddProcessedClassNames(ICollection<string> classNames)
    {
      this.processedClassNames.AddAll(classNames);
      if (classNames.Count <= 0)
        return;
      this.ProcessUnavailableEntries();
    }

    private void AddEntry(MappingsQueueEntry re)
    {
      if (this.CanProcess(re))
        this.availableEntries.Enqueue((object) re);
      else
        this.unavailableEntries.Add(re);
    }

    private void ProcessUnavailableEntries()
    {
      MappingsQueueEntry availableResourceEntry;
      while ((availableResourceEntry = this.FindAvailableResourceEntry()) != null)
      {
        this.availableEntries.Enqueue((object) availableResourceEntry);
        this.unavailableEntries.Remove(availableResourceEntry);
      }
    }

    private MappingsQueueEntry FindAvailableResourceEntry()
    {
      return this.unavailableEntries.FirstOrDefault<MappingsQueueEntry>(new Func<MappingsQueueEntry, bool>(this.CanProcess));
    }

    private bool CanProcess(MappingsQueueEntry ce)
    {
      return ce.RequiredClassNames.All<MappingsQueueEntry.RequiredEntityName>((Func<MappingsQueueEntry.RequiredEntityName, bool>) (c => this.processedClassNames.Contains(c.FullClassName) || this.processedClassNames.Contains(c.EntityName)));
    }

    private static string FormatExceptionMessage(IEnumerable<MappingsQueueEntry> resourceEntries)
    {
      StringBuilder stringBuilder = new StringBuilder(500);
      stringBuilder.Append("These classes referenced by 'extends' were not found:");
      foreach (MappingsQueueEntry.RequiredEntityName requiredEntityName in resourceEntries.SelectMany<MappingsQueueEntry, MappingsQueueEntry.RequiredEntityName>((Func<MappingsQueueEntry, IEnumerable<MappingsQueueEntry.RequiredEntityName>>) (resourceEntry => (IEnumerable<MappingsQueueEntry.RequiredEntityName>) resourceEntry.RequiredClassNames)))
        stringBuilder.Append('\n').Append((object) requiredEntityName);
      return stringBuilder.ToString();
    }
  }
}
