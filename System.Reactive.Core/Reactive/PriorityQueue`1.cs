// Decompiled with JetBrains decompiler
// Type: System.Reactive.PriorityQueue`1
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace System.Reactive
{
  internal class PriorityQueue<T> where T : IComparable<T>
  {
    private static long _count = long.MinValue;
    private PriorityQueue<T>.IndexedItem[] _items;
    private int _size;

    public PriorityQueue()
      : this(16)
    {
    }

    public PriorityQueue(int capacity)
    {
      this._items = new PriorityQueue<T>.IndexedItem[capacity];
      this._size = 0;
    }

    private bool IsHigherPriority(int left, int right)
    {
      return this._items[left].CompareTo(this._items[right]) < 0;
    }

    private void Percolate(int index)
    {
      if (index >= this._size || index < 0)
        return;
      int index1 = (index - 1) / 2;
      if (index1 < 0 || index1 == index || !this.IsHigherPriority(index, index1))
        return;
      PriorityQueue<T>.IndexedItem indexedItem = this._items[index];
      this._items[index] = this._items[index1];
      this._items[index1] = indexedItem;
      this.Percolate(index1);
    }

    private void Heapify() => this.Heapify(0);

    private void Heapify(int index)
    {
      if (index >= this._size || index < 0)
        return;
      int left1 = 2 * index + 1;
      int left2 = 2 * index + 2;
      int index1 = index;
      if (left1 < this._size && this.IsHigherPriority(left1, index1))
        index1 = left1;
      if (left2 < this._size && this.IsHigherPriority(left2, index1))
        index1 = left2;
      if (index1 == index)
        return;
      PriorityQueue<T>.IndexedItem indexedItem = this._items[index];
      this._items[index] = this._items[index1];
      this._items[index1] = indexedItem;
      this.Heapify(index1);
    }

    public int Count => this._size;

    public T Peek()
    {
      if (this._size == 0)
        throw new InvalidOperationException(Strings_Core.HEAP_EMPTY);
      return this._items[0].Value;
    }

    private void RemoveAt(int index)
    {
      this._items[index] = this._items[--this._size];
      this._items[this._size] = new PriorityQueue<T>.IndexedItem();
      this.Heapify();
      if (this._size >= this._items.Length / 4)
        return;
      PriorityQueue<T>.IndexedItem[] items1 = this._items;
      this._items = new PriorityQueue<T>.IndexedItem[this._items.Length / 2];
      PriorityQueue<T>.IndexedItem[] items2 = this._items;
      int size = this._size;
      Array.Copy((Array) items1, 0, (Array) items2, 0, size);
    }

    public T Dequeue()
    {
      T obj = this.Peek();
      this.RemoveAt(0);
      return obj;
    }

    public void Enqueue(T item)
    {
      if (this._size >= this._items.Length)
      {
        PriorityQueue<T>.IndexedItem[] items = this._items;
        this._items = new PriorityQueue<T>.IndexedItem[this._items.Length * 2];
        Array.Copy((Array) items, (Array) this._items, items.Length);
      }
      int index = this._size++;
      this._items[index] = new PriorityQueue<T>.IndexedItem()
      {
        Value = item,
        Id = Interlocked.Increment(ref PriorityQueue<T>._count)
      };
      this.Percolate(index);
    }

    public bool Remove(T item)
    {
      for (int index = 0; index < this._size; ++index)
      {
        if (EqualityComparer<T>.Default.Equals(this._items[index].Value, item))
        {
          this.RemoveAt(index);
          return true;
        }
      }
      return false;
    }

    private struct IndexedItem : IComparable<PriorityQueue<T>.IndexedItem>
    {
      public T Value;
      public long Id;

      public int CompareTo(PriorityQueue<T>.IndexedItem other)
      {
        int num = this.Value.CompareTo(other.Value);
        if (num == 0)
          num = this.Id.CompareTo(other.Id);
        return num;
      }
    }
  }
}
