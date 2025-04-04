// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.TestStatus
// Assembly: ICSharpCode.SharpZipLib, Version=0.85.5.452, Culture=neutral, PublicKeyToken=1b03e6acf1164f73
// MVID: 6C7CFC05-3661-46A0-98AB-FC6F81793937
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ICSharpCode.SharpZipLib.dll

#nullable disable
namespace ICSharpCode.SharpZipLib.Zip
{
  public class TestStatus
  {
    private ZipFile file_;
    private ZipEntry entry_;
    private bool entryValid_;
    private int errorCount_;
    private long bytesTested_;
    private TestOperation operation_;

    public TestStatus(ZipFile file) => this.file_ = file;

    public TestOperation Operation => this.operation_;

    public ZipFile File => this.file_;

    public ZipEntry Entry => this.entry_;

    public int ErrorCount => this.errorCount_;

    public long BytesTested => this.bytesTested_;

    public bool EntryValid => this.entryValid_;

    internal void AddError()
    {
      ++this.errorCount_;
      this.entryValid_ = false;
    }

    internal void SetOperation(TestOperation operation) => this.operation_ = operation;

    internal void SetEntry(ZipEntry entry)
    {
      this.entry_ = entry;
      this.entryValid_ = true;
      this.bytesTested_ = 0L;
    }

    internal void SetBytesTested(long value) => this.bytesTested_ = value;
  }
}
