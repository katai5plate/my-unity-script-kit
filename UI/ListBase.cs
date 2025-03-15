using System.Collections.Generic;

namespace UIPrefab {
  public class ListBase<T> : Mon, ILife {
    public int index = 0;
    public List<T> items;
    public T Current => items[index];
    public virtual void Next() =>
      index = index >= items.Count - 1 ? 0 : index + 1;
    public virtual void Prev() =>
      index = index <= 0 ? items.Count - 1 : index - 1;
  }
}