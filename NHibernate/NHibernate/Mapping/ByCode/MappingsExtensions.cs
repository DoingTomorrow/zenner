// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.MappingsExtensions
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public static class MappingsExtensions
  {
    public static void WriteAllXmlMapping(this IEnumerable<HbmMapping> mappings)
    {
      if (mappings == null)
        throw new ArgumentNullException(nameof (mappings));
      string path1 = MappingsExtensions.ArrangeMappingsFolderPath();
      foreach (HbmMapping mapping in mappings)
      {
        string fileName = MappingsExtensions.GetFileName(mapping);
        string contents = MappingsExtensions.Serialize(mapping);
        File.WriteAllText(Path.Combine(path1, fileName), contents);
      }
    }

    public static string AsString(this HbmMapping mappings)
    {
      return mappings != null ? MappingsExtensions.Serialize(mappings) : throw new ArgumentNullException(nameof (mappings));
    }

    private static string ArrangeMappingsFolderPath()
    {
      string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
      string relativeSearchPath = AppDomain.CurrentDomain.RelativeSearchPath;
      string path = Path.Combine(relativeSearchPath != null ? Path.Combine(baseDirectory, relativeSearchPath) : baseDirectory, "Mappings");
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      else
        System.Array.ForEach<string>(Directory.GetFiles(path), new Action<string>(File.Delete));
      return path;
    }

    private static string GetFileName(HbmMapping hbmMapping)
    {
      string str = "MyMapping";
      HbmClass hbmClass = ((IEnumerable<HbmClass>) hbmMapping.RootClasses).FirstOrDefault<HbmClass>();
      if (hbmClass != null)
        str = hbmClass.Name;
      HbmSubclass hbmSubclass = ((IEnumerable<HbmSubclass>) hbmMapping.SubClasses).FirstOrDefault<HbmSubclass>();
      if (hbmSubclass != null)
        str = hbmSubclass.Name;
      HbmJoinedSubclass hbmJoinedSubclass = ((IEnumerable<HbmJoinedSubclass>) hbmMapping.JoinedSubclasses).FirstOrDefault<HbmJoinedSubclass>();
      if (hbmJoinedSubclass != null)
        str = hbmJoinedSubclass.Name;
      HbmUnionSubclass hbmUnionSubclass = ((IEnumerable<HbmUnionSubclass>) hbmMapping.UnionSubclasses).FirstOrDefault<HbmUnionSubclass>();
      if (hbmUnionSubclass != null)
        str = hbmUnionSubclass.Name;
      return str + ".hbm.xml";
    }

    private static string Serialize(HbmMapping hbmElement)
    {
      XmlWriterSettings settings = new XmlWriterSettings()
      {
        Indent = true
      };
      using (MemoryStream output = new MemoryStream(2048))
      {
        using (XmlWriter xmlWriter = XmlWriter.Create((Stream) output, settings))
          new XmlSerializer(typeof (HbmMapping)).Serialize(xmlWriter, (object) hbmElement);
        output.Position = 0L;
        using (StreamReader streamReader = new StreamReader((Stream) output))
          return streamReader.ReadToEnd();
      }
    }
  }
}
