// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.TopologicalSorter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Loader
{
  internal class TopologicalSorter
  {
    private readonly int[] _vertices;
    private readonly int[,] _matrix;
    private int _numVerts;
    private readonly int[] _sortedArray;

    public TopologicalSorter(int size)
    {
      this._vertices = new int[size];
      this._matrix = new int[size, size];
      this._numVerts = 0;
      for (int index1 = 0; index1 < size; ++index1)
      {
        for (int index2 = 0; index2 < size; ++index2)
          this._matrix[index1, index2] = 0;
      }
      this._sortedArray = new int[size];
    }

    public int AddVertex(int vertex)
    {
      this._vertices[this._numVerts++] = vertex;
      return this._numVerts - 1;
    }

    public void AddEdge(int start, int end) => this._matrix[start, end] = 1;

    public int[] Sort()
    {
      while (this._numVerts > 0)
      {
        int delVert = this.noSuccessors();
        this._sortedArray[this._numVerts - 1] = delVert != -1 ? this._vertices[delVert] : throw new Exception("Graph has cycles");
        this.deleteVertex(delVert);
      }
      return this._sortedArray;
    }

    private int noSuccessors()
    {
      for (int index1 = 0; index1 < this._numVerts; ++index1)
      {
        bool flag = false;
        for (int index2 = 0; index2 < this._numVerts; ++index2)
        {
          if (this._matrix[index1, index2] > 0)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return index1;
      }
      return -1;
    }

    private void deleteVertex(int delVert)
    {
      if (delVert != this._numVerts - 1)
      {
        for (int index = delVert; index < this._numVerts - 1; ++index)
          this._vertices[index] = this._vertices[index + 1];
        for (int row = delVert; row < this._numVerts - 1; ++row)
          this.moveRowUp(row, this._numVerts);
        for (int col = delVert; col < this._numVerts - 1; ++col)
          this.moveColLeft(col, this._numVerts - 1);
      }
      --this._numVerts;
    }

    private void moveRowUp(int row, int length)
    {
      for (int index = 0; index < length; ++index)
        this._matrix[row, index] = this._matrix[row + 1, index];
    }

    private void moveColLeft(int col, int length)
    {
      for (int index = 0; index < length; ++index)
        this._matrix[index, col] = this._matrix[index, col + 1];
    }
  }
}
