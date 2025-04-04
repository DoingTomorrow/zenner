// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.XmlHelper
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Packaging;
using OfficeOpenXml.Style;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace OfficeOpenXml
{
  public abstract class XmlHelper
  {
    private string[] _schemaNodeOrder;

    internal XmlHelper(XmlNamespaceManager nameSpaceManager)
    {
      this.TopNode = (XmlNode) null;
      this.NameSpaceManager = nameSpaceManager;
    }

    internal XmlHelper(XmlNamespaceManager nameSpaceManager, XmlNode topNode)
    {
      this.TopNode = topNode;
      this.NameSpaceManager = nameSpaceManager;
    }

    internal XmlNamespaceManager NameSpaceManager { get; set; }

    internal XmlNode TopNode { get; set; }

    internal string[] SchemaNodeOrder
    {
      get => this._schemaNodeOrder;
      set => this._schemaNodeOrder = value;
    }

    internal XmlNode CreateNode(string path)
    {
      return path == "" ? this.TopNode : this.CreateNode(path, false);
    }

    internal XmlNode CreateNode(string path, bool insertFirst)
    {
      XmlNode node = this.TopNode;
      XmlNode refChild = (XmlNode) null;
      string str1 = path;
      char[] chArray = new char[1]{ '/' };
      foreach (string str2 in str1.Split(chArray))
      {
        XmlNode newChild = node.SelectSingleNode(str2, this.NameSpaceManager);
        if (newChild == null)
        {
          string[] strArray = str2.Split(':');
          if (this.SchemaNodeOrder != null && str2[0] != '@')
          {
            insertFirst = false;
            refChild = this.GetPrependNode(str2, node);
          }
          string prefix;
          string namespaceURI;
          string str3;
          if (strArray.Length > 1)
          {
            prefix = strArray[0];
            if (prefix[0] == '@')
              prefix = prefix.Substring(1, prefix.Length - 1);
            namespaceURI = this.NameSpaceManager.LookupNamespace(prefix);
            str3 = strArray[1];
          }
          else
          {
            prefix = "";
            namespaceURI = "";
            str3 = strArray[0];
          }
          if (str2.StartsWith("@"))
          {
            XmlAttribute attribute = node.OwnerDocument.CreateAttribute(str2.Substring(1, str2.Length - 1), namespaceURI);
            node.Attributes.Append(attribute);
          }
          else
          {
            newChild = !(prefix == "") ? (prefix == "" || node.OwnerDocument != null && node.OwnerDocument.DocumentElement != null && node.OwnerDocument.DocumentElement.NamespaceURI == namespaceURI && node.OwnerDocument.DocumentElement.Prefix == "" ? (XmlNode) node.OwnerDocument.CreateElement(str3, namespaceURI) : (XmlNode) node.OwnerDocument.CreateElement(prefix, str3, namespaceURI)) : (XmlNode) node.OwnerDocument.CreateElement(str3, namespaceURI);
            if (refChild != null)
            {
              node.InsertBefore(newChild, refChild);
              refChild = (XmlNode) null;
            }
            else if (insertFirst)
              node.PrependChild(newChild);
            else
              node.AppendChild(newChild);
          }
        }
        node = newChild;
      }
      return node;
    }

    internal XmlNode CreateComplexNode(string path)
    {
      return this.CreateComplexNode(this.TopNode, path, XmlHelper.eNodeInsertOrder.SchemaOrder, (XmlNode) null);
    }

    internal XmlNode CreateComplexNode(XmlNode topNode, string path)
    {
      return this.CreateComplexNode(topNode, path, XmlHelper.eNodeInsertOrder.SchemaOrder, (XmlNode) null);
    }

    internal XmlNode CreateComplexNode(
      XmlNode topNode,
      string path,
      XmlHelper.eNodeInsertOrder nodeInsertOrder,
      XmlNode referenceNode)
    {
      if (path == null || path == string.Empty)
        return topNode;
      XmlNode node = topNode;
      string empty1 = string.Empty;
      string str1 = path;
      char[] chArray = new char[1]{ '/' };
      foreach (string xpath in str1.Split(chArray))
      {
        if (xpath.Length > 0)
        {
          if (xpath.StartsWith("@"))
          {
            string[] strArray = xpath.Split('=');
            string name = strArray[0].Substring(1, strArray[0].Length - 1);
            string str2 = (string) null;
            if (strArray.Length > 1)
              str2 = strArray[1].Replace("'", "").Replace("\"", "");
            XmlAttribute namedItem = (XmlAttribute) node.Attributes.GetNamedItem(name);
            if (str2 == string.Empty)
            {
              if (namedItem != null)
                node.Attributes.Remove(namedItem);
            }
            else
            {
              if (namedItem == null)
              {
                XmlAttribute attribute = node.OwnerDocument.CreateAttribute(name);
                node.Attributes.Append(attribute);
              }
              if (str2 != null)
                node.Attributes[name].Value = str2;
            }
          }
          else
          {
            XmlNode newChild = node.SelectSingleNode(xpath, this.NameSpaceManager);
            if (newChild == null)
            {
              string[] strArray = xpath.Split(':');
              empty1 = string.Empty;
              string empty2;
              string namespaceURI;
              string str3;
              if (strArray.Length > 1)
              {
                empty2 = strArray[0];
                namespaceURI = this.NameSpaceManager.LookupNamespace(empty2);
                str3 = strArray[1];
              }
              else
              {
                empty2 = string.Empty;
                namespaceURI = string.Empty;
                str3 = strArray[0];
              }
              if (str3.IndexOf("[") > 0)
                str3 = str3.Substring(0, str3.IndexOf("["));
              newChild = !(empty2 == string.Empty) ? (node.OwnerDocument == null || node.OwnerDocument.DocumentElement == null || !(node.OwnerDocument.DocumentElement.NamespaceURI == namespaceURI) || !(node.OwnerDocument.DocumentElement.Prefix == string.Empty) ? (XmlNode) node.OwnerDocument.CreateElement(empty2, str3, namespaceURI) : (XmlNode) node.OwnerDocument.CreateElement(str3, namespaceURI)) : (XmlNode) node.OwnerDocument.CreateElement(str3, namespaceURI);
              if (nodeInsertOrder == XmlHelper.eNodeInsertOrder.SchemaOrder)
              {
                if (this.SchemaNodeOrder == null || this.SchemaNodeOrder.Length == 0)
                {
                  nodeInsertOrder = XmlHelper.eNodeInsertOrder.Last;
                }
                else
                {
                  referenceNode = this.GetPrependNode(str3, node);
                  nodeInsertOrder = referenceNode == null ? XmlHelper.eNodeInsertOrder.Last : XmlHelper.eNodeInsertOrder.Before;
                }
              }
              switch (nodeInsertOrder)
              {
                case XmlHelper.eNodeInsertOrder.First:
                  node.PrependChild(newChild);
                  break;
                case XmlHelper.eNodeInsertOrder.Last:
                  node.AppendChild(newChild);
                  break;
                case XmlHelper.eNodeInsertOrder.After:
                  node.InsertAfter(newChild, referenceNode);
                  referenceNode = (XmlNode) null;
                  break;
                case XmlHelper.eNodeInsertOrder.Before:
                  node.InsertBefore(newChild, referenceNode);
                  referenceNode = (XmlNode) null;
                  break;
              }
            }
            node = newChild;
          }
        }
      }
      return node;
    }

    private XmlNode GetPrependNode(string nodeName, XmlNode node)
    {
      int nodePos1 = this.GetNodePos(nodeName);
      if (nodePos1 < 0)
        return (XmlNode) null;
      XmlNode prependNode = (XmlNode) null;
      foreach (XmlNode childNode in node.ChildNodes)
      {
        int nodePos2 = this.GetNodePos(childNode.Name);
        if (nodePos2 > -1 && nodePos2 > nodePos1)
        {
          prependNode = childNode;
          break;
        }
      }
      return prependNode;
    }

    private int GetNodePos(string nodeName)
    {
      int num = nodeName.IndexOf(":");
      if (num > 0)
        nodeName = nodeName.Substring(num + 1, nodeName.Length - (num + 1));
      for (int nodePos = 0; nodePos < this._schemaNodeOrder.Length; ++nodePos)
      {
        if (nodeName == this._schemaNodeOrder[nodePos])
          return nodePos;
      }
      return -1;
    }

    internal void DeleteAllNode(string path)
    {
      string[] strArray = path.Split('/');
      XmlNode xmlNode = this.TopNode;
      foreach (string xpath in strArray)
      {
        xmlNode = xmlNode.SelectSingleNode(xpath, this.NameSpaceManager);
        if (xmlNode == null)
          break;
        if (xmlNode is XmlAttribute)
          (xmlNode as XmlAttribute).OwnerElement.Attributes.Remove(xmlNode as XmlAttribute);
        else
          xmlNode.ParentNode.RemoveChild(xmlNode);
      }
    }

    internal void DeleteNode(string path)
    {
      XmlNode oldChild = this.TopNode.SelectSingleNode(path, this.NameSpaceManager);
      if (oldChild == null)
        return;
      if (oldChild is XmlAttribute)
      {
        XmlAttribute node = (XmlAttribute) oldChild;
        node.OwnerElement.Attributes.Remove(node);
      }
      else
        oldChild.ParentNode.RemoveChild(oldChild);
    }

    internal void DeleteTopNode() => this.TopNode.ParentNode.RemoveChild(this.TopNode);

    internal void SetXmlNodeString(string path, string value)
    {
      this.SetXmlNodeString(this.TopNode, path, value, false, false);
    }

    internal void SetXmlNodeString(string path, string value, bool removeIfBlank)
    {
      this.SetXmlNodeString(this.TopNode, path, value, removeIfBlank, false);
    }

    internal void SetXmlNodeString(XmlNode node, string path, string value)
    {
      this.SetXmlNodeString(node, path, value, false, false);
    }

    internal void SetXmlNodeString(XmlNode node, string path, string value, bool removeIfBlank)
    {
      this.SetXmlNodeString(node, path, value, removeIfBlank, false);
    }

    internal void SetXmlNodeString(
      XmlNode node,
      string path,
      string value,
      bool removeIfBlank,
      bool insertFirst)
    {
      if (node == null)
        return;
      if (value == "" && removeIfBlank)
      {
        this.DeleteAllNode(path);
      }
      else
      {
        XmlNode xmlNode = node.SelectSingleNode(path, this.NameSpaceManager);
        if (xmlNode == null)
        {
          this.CreateNode(path, insertFirst);
          xmlNode = node.SelectSingleNode(path, this.NameSpaceManager);
        }
        xmlNode.InnerText = value;
      }
    }

    internal void SetXmlNodeBool(string path, bool value)
    {
      this.SetXmlNodeString(this.TopNode, path, value ? "1" : "0", false, false);
    }

    internal void SetXmlNodeBool(string path, bool value, bool removeIf)
    {
      if (value == removeIf)
      {
        XmlNode oldChild = this.TopNode.SelectSingleNode(path, this.NameSpaceManager);
        if (oldChild == null)
          return;
        if (oldChild is XmlAttribute)
        {
          XmlElement ownerElement = (oldChild as XmlAttribute).OwnerElement;
          ownerElement.ParentNode.RemoveChild((XmlNode) ownerElement);
        }
        else
          this.TopNode.RemoveChild(oldChild);
      }
      else
        this.SetXmlNodeString(this.TopNode, path, value ? "1" : "0", false, false);
    }

    internal bool ExistNode(string path)
    {
      return this.TopNode != null && this.TopNode.SelectSingleNode(path, this.NameSpaceManager) != null;
    }

    internal bool? GetXmlNodeBoolNullable(string path)
    {
      return string.IsNullOrEmpty(this.GetXmlNodeString(path)) ? new bool?() : new bool?(this.GetXmlNodeBool(path));
    }

    internal bool GetXmlNodeBool(string path) => this.GetXmlNodeBool(path, false);

    internal bool GetXmlNodeBool(string path, bool blankValue)
    {
      switch (this.GetXmlNodeString(path))
      {
        case "1":
        case "-1":
        case "True":
          return true;
        case "":
          return blankValue;
        default:
          return false;
      }
    }

    internal int GetXmlNodeInt(string path)
    {
      int result;
      return int.TryParse(this.GetXmlNodeString(path), out result) ? result : int.MinValue;
    }

    internal int? GetXmlNodeIntNull(string path)
    {
      string xmlNodeString = this.GetXmlNodeString(path);
      int result;
      return xmlNodeString != "" && int.TryParse(xmlNodeString, out result) ? new int?(result) : new int?();
    }

    internal Decimal GetXmlNodeDecimal(string path)
    {
      Decimal result;
      return Decimal.TryParse(this.GetXmlNodeString(path), NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result) ? result : 0M;
    }

    internal double? GetXmlNodeDoubleNull(string path)
    {
      string xmlNodeString = this.GetXmlNodeString(path);
      if (xmlNodeString == "")
        return new double?();
      double result;
      return double.TryParse(xmlNodeString, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result) ? new double?(result) : new double?();
    }

    internal double GetXmlNodeDouble(string path)
    {
      string xmlNodeString = this.GetXmlNodeString(path);
      double result;
      return xmlNodeString == "" || !double.TryParse(xmlNodeString, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result) ? double.NaN : result;
    }

    internal string GetXmlNodeString(XmlNode node, string path)
    {
      if (node == null)
        return "";
      XmlNode xmlNode = node.SelectSingleNode(path, this.NameSpaceManager);
      if (xmlNode == null)
        return "";
      if (xmlNode.NodeType != XmlNodeType.Attribute)
        return xmlNode.InnerText;
      return xmlNode.Value == null ? "" : xmlNode.Value;
    }

    internal string GetXmlNodeString(string path) => this.GetXmlNodeString(this.TopNode, path);

    internal static Uri GetNewUri(ZipPackage package, string sUri)
    {
      return XmlHelper.GetNewUri(package, sUri, 1);
    }

    internal static Uri GetNewUri(ZipPackage package, string sUri, int id)
    {
      Uri partUri;
      do
      {
        partUri = new Uri(string.Format(sUri, (object) id++), UriKind.Relative);
      }
      while (package.PartExists(partUri));
      return partUri;
    }

    internal void InserAfter(XmlNode parentNode, string beforeNodes, XmlNode newNode)
    {
      string str = beforeNodes;
      char[] chArray = new char[1]{ ',' };
      foreach (string xpath in str.Split(chArray))
      {
        XmlNode refChild = parentNode.SelectSingleNode(xpath, this.NameSpaceManager);
        if (refChild != null)
        {
          parentNode.InsertAfter(newNode, refChild);
          return;
        }
      }
      parentNode.InsertAfter(newNode, (XmlNode) null);
    }

    internal static void LoadXmlSafe(XmlDocument xmlDoc, Stream stream)
    {
      XmlReader reader = XmlReader.Create(stream, new XmlReaderSettings()
      {
        ProhibitDtd = true
      });
      xmlDoc.Load(reader);
    }

    internal static void LoadXmlSafe(XmlDocument xmlDoc, string xml, Encoding encoding)
    {
      MemoryStream memoryStream = new MemoryStream(encoding.GetBytes(xml));
      XmlHelper.LoadXmlSafe(xmlDoc, (Stream) memoryStream);
    }

    internal delegate int ChangedEventHandler(StyleBase sender, StyleChangeEventArgs e);

    internal enum eNodeInsertOrder
    {
      First,
      Last,
      After,
      Before,
      SchemaOrder,
    }
  }
}
