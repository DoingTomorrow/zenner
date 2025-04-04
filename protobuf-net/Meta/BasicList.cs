// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Meta.BasicList
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using System;
using System.Collections;

#nullable disable
namespace ProtoBuf.Meta
{
  internal class BasicList : IEnumerable
  {
    private static readonly BasicList.Node nil = new BasicList.Node((object[]) null, 0);
    protected BasicList.Node head = BasicList.nil;

    public void CopyTo(Array array, int offset) => this.head.CopyTo(array, offset);

    public int Add(object value) => (this.head = this.head.Append(value)).Length - 1;

    public object this[int index] => this.head[index];

    public void Trim() => this.head = this.head.Trim();

    public int Count => this.head.Length;

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) new BasicList.NodeEnumerator(this.head);
    }

    public BasicList.NodeEnumerator GetEnumerator() => new BasicList.NodeEnumerator(this.head);

    internal int IndexOf(BasicList.MatchPredicate predicate, object ctx)
    {
      return this.head.IndexOf(predicate, ctx);
    }

    internal int IndexOfString(string value) => this.head.IndexOfString(value);

    internal int IndexOfReference(object instance) => this.head.IndexOfReference(instance);

    internal bool Contains(object value)
    {
      foreach (object objA in this)
      {
        if (object.Equals(objA, value))
          return true;
      }
      return false;
    }

    internal static BasicList GetContiguousGroups(int[] keys, object[] values)
    {
      if (keys == null)
        throw new ArgumentNullException(nameof (keys));
      if (values == null)
        throw new ArgumentNullException(nameof (values));
      if (values.Length < keys.Length)
        throw new ArgumentException("Not all keys are covered by values", nameof (values));
      BasicList contiguousGroups = new BasicList();
      BasicList.Group group = (BasicList.Group) null;
      for (int index = 0; index < keys.Length; ++index)
      {
        if (index == 0 || keys[index] != keys[index - 1])
          group = (BasicList.Group) null;
        if (group == null)
        {
          group = new BasicList.Group(keys[index]);
          contiguousGroups.Add((object) group);
        }
        group.Items.Add(values[index]);
      }
      return contiguousGroups;
    }

    public struct NodeEnumerator : IEnumerator
    {
      private int position;
      private readonly BasicList.Node node;

      internal NodeEnumerator(BasicList.Node node)
      {
        this.position = -1;
        this.node = node;
      }

      void IEnumerator.Reset() => this.position = -1;

      public object Current => this.node[this.position];

      public bool MoveNext()
      {
        int length = this.node.Length;
        return this.position <= length && ++this.position < length;
      }
    }

    internal sealed class Node
    {
      private readonly object[] data;
      private int length;

      public object this[int index]
      {
        get
        {
          return index >= 0 && index < this.length ? this.data[index] : throw new ArgumentOutOfRangeException(nameof (index));
        }
        set
        {
          if (index < 0 || index >= this.length)
            throw new ArgumentOutOfRangeException(nameof (index));
          this.data[index] = value;
        }
      }

      public int Length => this.length;

      internal Node(object[] data, int length)
      {
        this.data = data;
        this.length = length;
      }

      public void RemoveLastWithMutate()
      {
        if (this.length == 0)
          throw new InvalidOperationException();
        --this.length;
      }

      public BasicList.Node Append(object value)
      {
        int length = this.length + 1;
        object[] objArray;
        if (this.data == null)
          objArray = new object[10];
        else if (this.length == this.data.Length)
        {
          objArray = new object[this.data.Length * 2];
          Array.Copy((Array) this.data, (Array) objArray, this.length);
        }
        else
          objArray = this.data;
        objArray[this.length] = value;
        return new BasicList.Node(objArray, length);
      }

      public BasicList.Node Trim()
      {
        if (this.length == 0 || this.length == this.data.Length)
          return this;
        object[] objArray = new object[this.length];
        Array.Copy((Array) this.data, (Array) objArray, this.length);
        return new BasicList.Node(objArray, this.length);
      }

      internal int IndexOfString(string value)
      {
        for (int index = 0; index < this.length; ++index)
        {
          if (value == (string) this.data[index])
            return index;
        }
        return -1;
      }

      internal int IndexOfReference(object instance)
      {
        for (int index = 0; index < this.length; ++index)
        {
          if (instance == this.data[index])
            return index;
        }
        return -1;
      }

      internal int IndexOf(BasicList.MatchPredicate predicate, object ctx)
      {
        for (int index = 0; index < this.length; ++index)
        {
          if (predicate(this.data[index], ctx))
            return index;
        }
        return -1;
      }

      internal void CopyTo(Array array, int offset)
      {
        if (this.length <= 0)
          return;
        Array.Copy((Array) this.data, 0, array, offset, this.length);
      }

      internal void Clear()
      {
        if (this.data != null)
          Array.Clear((Array) this.data, 0, this.data.Length);
        this.length = 0;
      }
    }

    internal delegate bool MatchPredicate(object value, object ctx);

    internal sealed class Group
    {
      public readonly int First;
      public readonly BasicList Items;

      public Group(int first)
      {
        this.First = first;
        this.Items = new BasicList();
      }
    }
  }
}
