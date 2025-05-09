﻿// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ZipContainer
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using Ionic.Zlib;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Ionic.Zip
{
  internal class ZipContainer
  {
    private ZipFile _zf;
    private ZipOutputStream _zos;
    private ZipInputStream _zis;

    public ZipContainer(object o)
    {
      this._zf = o as ZipFile;
      this._zos = o as ZipOutputStream;
      this._zis = o as ZipInputStream;
    }

    public ZipFile ZipFile => this._zf;

    public ZipOutputStream ZipOutputStream => this._zos;

    public string Name
    {
      get
      {
        if (this._zf != null)
          return this._zf.Name;
        if (this._zis != null)
          throw new NotSupportedException();
        return this._zos.Name;
      }
    }

    public string Password
    {
      get
      {
        if (this._zf != null)
          return this._zf._Password;
        return this._zis != null ? this._zis._Password : this._zos._password;
      }
    }

    public Zip64Option Zip64
    {
      get
      {
        if (this._zf != null)
          return this._zf._zip64;
        if (this._zis != null)
          throw new NotSupportedException();
        return this._zos._zip64;
      }
    }

    public int BufferSize
    {
      get
      {
        if (this._zf != null)
          return this._zf.BufferSize;
        if (this._zis != null)
          throw new NotSupportedException();
        return 0;
      }
    }

    public ParallelDeflateOutputStream ParallelDeflater
    {
      get
      {
        if (this._zf != null)
          return this._zf.ParallelDeflater;
        return this._zis != null ? (ParallelDeflateOutputStream) null : this._zos.ParallelDeflater;
      }
      set
      {
        if (this._zf != null)
        {
          this._zf.ParallelDeflater = value;
        }
        else
        {
          if (this._zos == null)
            return;
          this._zos.ParallelDeflater = value;
        }
      }
    }

    public long ParallelDeflateThreshold
    {
      get
      {
        return this._zf != null ? this._zf.ParallelDeflateThreshold : this._zos.ParallelDeflateThreshold;
      }
    }

    public int ParallelDeflateMaxBufferPairs
    {
      get
      {
        return this._zf != null ? this._zf.ParallelDeflateMaxBufferPairs : this._zos.ParallelDeflateMaxBufferPairs;
      }
    }

    public int CodecBufferSize
    {
      get
      {
        if (this._zf != null)
          return this._zf.CodecBufferSize;
        return this._zis != null ? this._zis.CodecBufferSize : this._zos.CodecBufferSize;
      }
    }

    public CompressionStrategy Strategy
    {
      get => this._zf != null ? this._zf.Strategy : this._zos.Strategy;
    }

    public Zip64Option UseZip64WhenSaving
    {
      get => this._zf != null ? this._zf.UseZip64WhenSaving : this._zos.EnableZip64;
    }

    public Encoding AlternateEncoding
    {
      get
      {
        if (this._zf != null)
          return this._zf.AlternateEncoding;
        return this._zos != null ? this._zos.AlternateEncoding : (Encoding) null;
      }
    }

    public Encoding DefaultEncoding
    {
      get
      {
        if (this._zf != null)
          return ZipFile.DefaultEncoding;
        return this._zos != null ? ZipOutputStream.DefaultEncoding : (Encoding) null;
      }
    }

    public ZipOption AlternateEncodingUsage
    {
      get
      {
        if (this._zf != null)
          return this._zf.AlternateEncodingUsage;
        return this._zos != null ? this._zos.AlternateEncodingUsage : ZipOption.Default;
      }
    }

    public Stream ReadStream => this._zf != null ? this._zf.ReadStream : this._zis.ReadStream;
  }
}
