// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.XmlNode
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System;
using System.Xml;

#nullable disable
namespace Microsoft.Synchronization.ClientServices
{
  public class XmlNode
  {
    private XmlNodeType nodeType;
    private StringHandle localName;
    private ValueHandle value;
    private bool hasValue;
    private bool canGetAttribute;
    private bool canMoveToElement;
    private ReadState readState;
    private XmlAttributeTextNode attributeTextNode;
    private bool exitScope;
    private int depthDelta;
    private bool skipValue;
    private bool hasContent;
    private bool isEmptyElement;
    private char quoteChar;
    private bool isAtomicValue;

    public bool HasValue => this.hasValue;

    public ReadState ReadState => this.readState;

    public StringHandle LocalName => this.localName;

    public bool CanGetAttribute => this.canGetAttribute;

    public bool CanMoveToElement => this.canMoveToElement;

    public XmlAttributeTextNode AttributeText => this.attributeTextNode;

    public bool SkipValue => this.skipValue;

    public ValueHandle Value => this.value;

    public int DepthDelta => this.depthDelta;

    public bool HasContent => this.hasContent;

    public XmlNodeType NodeType
    {
      get => this.nodeType;
      set => this.nodeType = value;
    }

    public bool IsAtomicValue
    {
      get => this.isAtomicValue;
      set => this.isAtomicValue = value;
    }

    public bool ExitScope
    {
      get => this.exitScope;
      set => this.exitScope = value;
    }

    public bool IsEmptyElement
    {
      get => this.isEmptyElement;
      set => this.isEmptyElement = value;
    }

    public char QuoteChar
    {
      get => this.quoteChar;
      set => this.quoteChar = value;
    }

    public string ValueAsString => this.Value.GetString();

    protected XmlNode(
      XmlNodeType nodeType,
      StringHandle localName,
      ValueHandle value,
      XmlNode.XmlNodeFlags nodeFlags,
      ReadState readState,
      XmlAttributeTextNode attributeTextNode,
      int depthDelta)
    {
      this.nodeType = nodeType;
      this.localName = localName;
      this.value = value;
      this.hasValue = (nodeFlags & XmlNode.XmlNodeFlags.HasValue) != 0;
      this.canGetAttribute = (nodeFlags & XmlNode.XmlNodeFlags.CanGetAttribute) != 0;
      this.canMoveToElement = (nodeFlags & XmlNode.XmlNodeFlags.CanMoveToElement) != 0;
      this.IsAtomicValue = (nodeFlags & XmlNode.XmlNodeFlags.AtomicValue) != 0;
      this.skipValue = (nodeFlags & XmlNode.XmlNodeFlags.SkipValue) != 0;
      this.hasContent = (nodeFlags & XmlNode.XmlNodeFlags.HasContent) != 0;
      this.readState = readState;
      this.attributeTextNode = attributeTextNode;
      this.exitScope = nodeType == XmlNodeType.EndElement;
      this.depthDelta = depthDelta;
      this.isEmptyElement = false;
      this.quoteChar = '"';
    }

    public bool IsLocalName(string name) => this.LocalName == name;

    public bool IsNamespaceUri(string iNs) => false;

    public bool IsLocalNameAndNamespaceUri(string name, string iNs) => false;

    public bool IsPrefixAndLocalName(string prefix, string slocalName) => false;

    [Flags]
    protected enum XmlNodeFlags
    {
      None = 0,
      CanGetAttribute = 1,
      CanMoveToElement = 2,
      HasValue = 4,
      AtomicValue = 8,
      SkipValue = 16, // 0x00000010
      HasContent = 32, // 0x00000020
    }
  }
}
