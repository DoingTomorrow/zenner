﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XmlDeclarationWrapper
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System.Xml;

#nullable disable
namespace Newtonsoft.Json.Converters
{
  internal class XmlDeclarationWrapper : XmlNodeWrapper, IXmlDeclaration, IXmlNode
  {
    private readonly XmlDeclaration _declaration;

    public XmlDeclarationWrapper(XmlDeclaration declaration)
      : base((XmlNode) declaration)
    {
      this._declaration = declaration;
    }

    public string Version => this._declaration.Version;

    public string Encoding
    {
      get => this._declaration.Encoding;
      set => this._declaration.Encoding = value;
    }

    public string Standalone
    {
      get => this._declaration.Standalone;
      set => this._declaration.Standalone = value;
    }
  }
}
