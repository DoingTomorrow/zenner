// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.SfxGenerationException
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

#nullable disable
namespace Ionic.Zip
{
  [Guid("ebc25cf6-9120-4283-b972-0e5520d00008")]
  [Serializable]
  public class SfxGenerationException : ZipException
  {
    public SfxGenerationException()
    {
    }

    public SfxGenerationException(string message)
      : base(message)
    {
    }

    protected SfxGenerationException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
