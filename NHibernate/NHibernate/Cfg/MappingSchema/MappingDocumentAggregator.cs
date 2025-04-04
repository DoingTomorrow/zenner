// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.MappingDocumentAggregator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

#nullable disable
namespace NHibernate.Cfg.MappingSchema
{
  public class MappingDocumentAggregator
  {
    private readonly IAssemblyResourceFilter defaultFilter;
    private readonly System.Collections.Generic.List<HbmMapping> documents = new System.Collections.Generic.List<HbmMapping>();
    private readonly IMappingDocumentParser parser;

    public MappingDocumentAggregator()
      : this((IMappingDocumentParser) new MappingDocumentParser(), (IAssemblyResourceFilter) new EndsWithHbmXmlFilter())
    {
    }

    public MappingDocumentAggregator(
      IMappingDocumentParser parser,
      IAssemblyResourceFilter defaultFilter)
    {
      if (parser == null)
        throw new ArgumentNullException(nameof (parser));
      if (defaultFilter == null)
        throw new ArgumentNullException(nameof (defaultFilter));
      this.parser = parser;
      this.defaultFilter = defaultFilter;
    }

    public void Add(HbmMapping document)
    {
      if (document == null)
        throw new ArgumentNullException(nameof (document));
      this.documents.Add(document);
    }

    public void Add(Stream stream) => this.Add(this.parser.Parse(stream));

    public void Add(Assembly assembly, string resourceName)
    {
      if (assembly == null)
        throw new ArgumentNullException(nameof (assembly));
      using (Stream manifestResourceStream = assembly.GetManifestResourceStream(resourceName))
        this.Add(manifestResourceStream);
    }

    public void Add(Assembly assembly, IAssemblyResourceFilter filter)
    {
      if (assembly == null)
        throw new ArgumentNullException(nameof (assembly));
      if (filter == null)
        throw new ArgumentNullException(nameof (filter));
      foreach (string manifestResourceName in assembly.GetManifestResourceNames())
      {
        if (this.defaultFilter.ShouldParse(manifestResourceName))
          this.Add(assembly, manifestResourceName);
      }
    }

    public void Add(Assembly assembly) => this.Add(assembly, this.defaultFilter);

    public void Add(FileInfo file)
    {
      if (file == null)
        throw new ArgumentNullException(nameof (file));
      using (FileStream fileStream = file.OpenRead())
        this.Add((Stream) fileStream);
    }

    public void Add(string fileName) => this.Add(new FileInfo(fileName));

    public IList<HbmMapping> List() => (IList<HbmMapping>) this.documents.ToArray();
  }
}
