// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.ImportPart
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using System;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class ImportPart
  {
    private readonly AttributeStore attributes = new AttributeStore();

    public ImportPart(Type importType)
    {
      this.attributes.Set("Class", 0, (object) new TypeReference(importType));
    }

    public void As(string alternativeName)
    {
      this.attributes.Set("Rename", 2, (object) alternativeName);
    }

    internal ImportMapping GetImportMapping() => new ImportMapping(this.attributes.Clone());
  }
}
