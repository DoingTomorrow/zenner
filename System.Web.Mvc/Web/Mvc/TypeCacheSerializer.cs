// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.TypeCacheSerializer
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc.Properties;
using System.Xml;

#nullable disable
namespace System.Web.Mvc
{
  internal sealed class TypeCacheSerializer
  {
    private static readonly Guid _mvcVersionId = typeof (TypeCacheSerializer).Module.ModuleVersionId;

    private DateTime CurrentDate => this.CurrentDateOverride ?? DateTime.Now;

    internal DateTime? CurrentDateOverride { get; set; }

    public List<Type> DeserializeTypes(TextReader input)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.Load(input);
      XmlElement documentElement = xmlDocument.DocumentElement;
      if (new Guid(documentElement.Attributes["mvcVersionId"].Value) != TypeCacheSerializer._mvcVersionId)
        return (List<Type>) null;
      List<Type> typeList = new List<Type>();
      foreach (XmlNode childNode1 in documentElement.ChildNodes)
      {
        Assembly assembly = Assembly.Load(childNode1.Attributes["name"].Value);
        foreach (XmlNode childNode2 in childNode1.ChildNodes)
        {
          Guid guid = new Guid(childNode2.Attributes["versionId"].Value);
          foreach (XmlNode childNode3 in childNode2.ChildNodes)
          {
            string innerText = childNode3.InnerText;
            Type type = assembly.GetType(innerText);
            if (type == (Type) null || type.Module.ModuleVersionId != guid)
              return (List<Type>) null;
            typeList.Add(type);
          }
        }
      }
      return typeList;
    }

    public void SerializeTypes(IEnumerable<Type> types, TextWriter output)
    {
      IEnumerable<IGrouping<Assembly, IGrouping<Module, Type>>> groupings = types.GroupBy<Type, Module>((Func<Type, Module>) (type => type.Module)).GroupBy<IGrouping<Module, Type>, Assembly>((Func<IGrouping<Module, Type>, Assembly>) (groupedByModule => groupedByModule.Key.Assembly));
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.AppendChild((XmlNode) xmlDocument.CreateComment(MvcResources.TypeCache_DoNotModify));
      XmlElement element1 = xmlDocument.CreateElement("typeCache");
      xmlDocument.AppendChild((XmlNode) element1);
      element1.SetAttribute("lastModified", this.CurrentDate.ToString());
      element1.SetAttribute("mvcVersionId", TypeCacheSerializer._mvcVersionId.ToString());
      foreach (IGrouping<Assembly, IGrouping<Module, Type>> grouping1 in groupings)
      {
        XmlElement element2 = xmlDocument.CreateElement("assembly");
        element1.AppendChild((XmlNode) element2);
        element2.SetAttribute("name", grouping1.Key.FullName);
        foreach (IGrouping<Module, Type> grouping2 in (IEnumerable<IGrouping<Module, Type>>) grouping1)
        {
          XmlElement element3 = xmlDocument.CreateElement("module");
          element2.AppendChild((XmlNode) element3);
          element3.SetAttribute("versionId", grouping2.Key.ModuleVersionId.ToString());
          foreach (Type type in (IEnumerable<Type>) grouping2)
          {
            XmlElement element4 = xmlDocument.CreateElement("type");
            element3.AppendChild((XmlNode) element4);
            element4.AppendChild((XmlNode) xmlDocument.CreateTextNode(type.FullName));
          }
        }
      }
      xmlDocument.Save(output);
    }
  }
}
