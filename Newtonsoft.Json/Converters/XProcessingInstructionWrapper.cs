// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XProcessingInstructionWrapper
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System.Xml.Linq;

#nullable disable
namespace Newtonsoft.Json.Converters
{
  internal class XProcessingInstructionWrapper(XProcessingInstruction processingInstruction) : 
    XObjectWrapper((XObject) processingInstruction)
  {
    private XProcessingInstruction ProcessingInstruction
    {
      get => (XProcessingInstruction) this.WrappedNode;
    }

    public override string LocalName => this.ProcessingInstruction.Target;

    public override string Value
    {
      get => this.ProcessingInstruction.Data;
      set => this.ProcessingInstruction.Data = value;
    }
  }
}
