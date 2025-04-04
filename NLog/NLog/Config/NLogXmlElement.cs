// Decompiled with JetBrains decompiler
// Type: NLog.Config.NLogXmlElement
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

#nullable disable
namespace NLog.Config
{
  internal class NLogXmlElement
  {
    private readonly List<string> _parsingErrors;

    public NLogXmlElement(string inputUri)
      : this()
    {
      using (XmlReader reader = XmlReader.Create(inputUri))
      {
        int content = (int) reader.MoveToContent();
        this.Parse(reader);
      }
    }

    public NLogXmlElement(XmlReader reader)
      : this()
    {
      this.Parse(reader);
    }

    private NLogXmlElement()
    {
      this.AttributeValues = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      this.Children = (IList<NLogXmlElement>) new List<NLogXmlElement>();
      this._parsingErrors = new List<string>();
    }

    public string LocalName { get; private set; }

    public Dictionary<string, string> AttributeValues { get; private set; }

    public IList<NLogXmlElement> Children { get; private set; }

    public string Value { get; private set; }

    public IEnumerable<NLogXmlElement> Elements(string elementName)
    {
      List<NLogXmlElement> nlogXmlElementList = new List<NLogXmlElement>();
      foreach (NLogXmlElement child in (IEnumerable<NLogXmlElement>) this.Children)
      {
        if (child.LocalName.Equals(elementName, StringComparison.OrdinalIgnoreCase))
          nlogXmlElementList.Add(child);
      }
      return (IEnumerable<NLogXmlElement>) nlogXmlElementList;
    }

    public string GetRequiredAttribute(string attributeName)
    {
      return this.GetOptionalAttribute(attributeName, (string) null) ?? throw new NLogConfigurationException("Expected " + attributeName + " on <" + this.LocalName + " />");
    }

    public bool GetOptionalBooleanAttribute(string attributeName, bool defaultValue)
    {
      string str;
      return !this.AttributeValues.TryGetValue(attributeName, out str) ? defaultValue : Convert.ToBoolean(str, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public bool? GetOptionalBooleanAttribute(string attributeName, bool? defaultValue)
    {
      string str;
      if (!this.AttributeValues.TryGetValue(attributeName, out str))
        return defaultValue;
      return StringHelpers.IsNullOrWhiteSpace(str) ? new bool?() : new bool?(Convert.ToBoolean(str, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public string GetOptionalAttribute(string attributeName, string defaultValue)
    {
      string optionalAttribute;
      if (!this.AttributeValues.TryGetValue(attributeName, out optionalAttribute))
        optionalAttribute = defaultValue;
      return optionalAttribute;
    }

    public void AssertName(params string[] allowedNames)
    {
      foreach (string allowedName in allowedNames)
      {
        if (this.LocalName.Equals(allowedName, StringComparison.OrdinalIgnoreCase))
          return;
      }
      throw new InvalidOperationException("Assertion failed. Expected element name '" + string.Join("|", allowedNames) + "', actual: '" + this.LocalName + "'.");
    }

    public IEnumerable<string> GetParsingErrors()
    {
      foreach (string parsingError in this._parsingErrors)
        yield return parsingError;
      foreach (NLogXmlElement child in (IEnumerable<NLogXmlElement>) this.Children)
      {
        foreach (string parsingError in child.GetParsingErrors())
          yield return parsingError;
      }
    }

    private void Parse(XmlReader reader)
    {
      if (reader.MoveToFirstAttribute())
      {
        do
        {
          if (!this.AttributeValues.ContainsKey(reader.LocalName))
            this.AttributeValues.Add(reader.LocalName, reader.Value);
          else
            this._parsingErrors.Add(string.Format("Duplicate attribute detected. Attribute name: [{0}]. Duplicate value:[{1}], Current value:[{2}]", (object) reader.LocalName, (object) reader.Value, (object) this.AttributeValues[reader.LocalName]));
        }
        while (reader.MoveToNextAttribute());
        reader.MoveToElement();
      }
      this.LocalName = reader.LocalName;
      if (reader.IsEmptyElement)
        return;
      while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.CDATA || reader.NodeType == XmlNodeType.Text)
          this.Value += reader.Value;
        else if (reader.NodeType == XmlNodeType.Element)
          this.Children.Add(new NLogXmlElement(reader));
      }
    }
  }
}
