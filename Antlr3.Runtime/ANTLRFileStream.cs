// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.ANTLRFileStream
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;
using System.IO;
using System.Security.Permissions;
using System.Text;

#nullable disable
namespace Antlr.Runtime
{
  [Serializable]
  [FileIOPermission(SecurityAction.Demand, Unrestricted = true)]
  public class ANTLRFileStream : ANTLRStringStream
  {
    protected string fileName;

    public ANTLRFileStream(string fileName)
      : this(fileName, (Encoding) null)
    {
    }

    public ANTLRFileStream(string fileName, Encoding encoding)
    {
      this.fileName = fileName;
      this.Load(fileName, encoding);
    }

    public virtual void Load(string fileName, Encoding encoding)
    {
      if (fileName == null)
        return;
      this.data = (encoding != null ? File.ReadAllText(fileName, encoding) : File.ReadAllText(fileName)).ToCharArray();
      this.n = this.data.Length;
    }

    public override string SourceName => this.fileName;
  }
}
