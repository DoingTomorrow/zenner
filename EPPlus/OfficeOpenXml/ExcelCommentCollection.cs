// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelCommentCollection
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Packaging;
using OfficeOpenXml.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

#nullable disable
namespace OfficeOpenXml
{
  public class ExcelCommentCollection : IEnumerable, IDisposable
  {
    internal RangeCollection _comments;

    internal ExcelCommentCollection(ExcelPackage pck, ExcelWorksheet ws, XmlNamespaceManager ns)
    {
      this.CommentXml = new XmlDocument();
      this.CommentXml.PreserveWhitespace = false;
      this.NameSpaceManager = ns;
      this.Worksheet = ws;
      this.CreateXml(pck);
      this.AddCommentsFromXml();
    }

    private void CreateXml(ExcelPackage pck)
    {
      ZipPackageRelationshipCollection relationshipsByType = this.Worksheet.Part.GetRelationshipsByType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/comments");
      bool flag = false;
      this.CommentXml = new XmlDocument();
      foreach (ZipPackageRelationship packageRelationship in relationshipsByType)
      {
        this.Uri = UriHelper.ResolvePartUri(packageRelationship.SourceUri, packageRelationship.TargetUri);
        this.Part = pck.Package.GetPart(this.Uri);
        XmlHelper.LoadXmlSafe(this.CommentXml, (Stream) this.Part.GetStream());
        this.RelId = packageRelationship.Id;
        flag = true;
      }
      if (flag)
        return;
      this.CommentXml.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\" ?><comments xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\"><authors /><commentList /></comments>");
      this.Uri = (Uri) null;
    }

    private void AddCommentsFromXml()
    {
      List<IRangeID> cells = new List<IRangeID>();
      foreach (XmlElement selectNode in this.CommentXml.SelectNodes("//d:commentList/d:comment", this.NameSpaceManager))
      {
        ExcelComment excelComment = new ExcelComment(this.NameSpaceManager, (XmlNode) selectNode, new ExcelRangeBase(this.Worksheet, selectNode.GetAttribute("ref")));
        cells.Add((IRangeID) excelComment);
      }
      this._comments = new RangeCollection(cells);
    }

    public XmlDocument CommentXml { get; set; }

    internal Uri Uri { get; set; }

    internal string RelId { get; set; }

    internal XmlNamespaceManager NameSpaceManager { get; set; }

    internal ZipPackagePart Part { get; set; }

    public ExcelWorksheet Worksheet { get; set; }

    public int Count => this._comments.Count;

    public ExcelComment this[int Index]
    {
      get
      {
        if (Index < 0 || Index >= this._comments.Count)
          throw new ArgumentOutOfRangeException("Comment index out of range");
        return this._comments[Index] as ExcelComment;
      }
    }

    public ExcelComment this[ExcelCellAddress cell]
    {
      get
      {
        ulong cellId = ExcelCellBase.GetCellID(this.Worksheet.SheetID, cell.Row, cell.Column);
        return this._comments.IndexOf(cellId) >= 0 ? this._comments[cellId] as ExcelComment : (ExcelComment) null;
      }
    }

    public ExcelComment Add(ExcelRangeBase cell, string Text, string author)
    {
      XmlElement element = this.CommentXml.CreateElement("comment", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
      int num = this._comments.IndexOf(ExcelCellBase.GetCellID(this.Worksheet.SheetID, cell._fromRow, cell._fromCol));
      if (num < 0 && ~num < this._comments.Count)
      {
        ExcelComment comment = this._comments[~num] as ExcelComment;
        comment._commentHelper.TopNode.ParentNode.InsertBefore((XmlNode) element, comment._commentHelper.TopNode);
      }
      else
        this.CommentXml.SelectSingleNode("d:comments/d:commentList", this.NameSpaceManager).AppendChild((XmlNode) element);
      element.SetAttribute("ref", cell.Start.Address);
      ExcelComment cell1 = new ExcelComment(this.NameSpaceManager, (XmlNode) element, cell);
      cell1.RichText.Add(Text);
      if (author != "")
        cell1.Author = author;
      this._comments.Add((IRangeID) cell1);
      return cell1;
    }

    public void Remove(ExcelComment comment)
    {
      ulong cellId = ExcelCellBase.GetCellID(this.Worksheet.SheetID, comment.Range._fromRow, comment.Range._fromCol);
      int Index = this._comments.IndexOf(cellId);
      if (Index < 0 || comment != this._comments[Index])
        throw new ArgumentException("Comment does not exist in the worksheet");
      comment.TopNode.ParentNode.RemoveChild(comment.TopNode);
      comment._commentHelper.TopNode.ParentNode.RemoveChild(comment._commentHelper.TopNode);
      this.Worksheet.VmlDrawingsComments._drawings.Delete(cellId);
      this._comments.Delete(cellId);
    }

    void IDisposable.Dispose()
    {
      if (this._comments == null)
        return;
      ((IDisposable) this._comments).Dispose();
    }

    public void RemoveAt(int Index) => this.Remove(this[Index]);

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._comments;
  }
}
