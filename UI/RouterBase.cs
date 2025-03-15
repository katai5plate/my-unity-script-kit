using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UIPrefab {
  // public class RouterBase<T> : ListBase<T> where T : Mon, ILife {
  public class RouterBase<T> : ListBase<T>, ILife where T : Mon, ILife {
    readonly List<MonoBehaviour> instantiateds = new();

    void Activate(bool isAwake = false) {
      if (isAwake) {
        foreach (var el in items.Select((value, index) => new { value, index })) {
          el.value.gameObject.SetActive(true);
          var instantiated = Instantiate(el.value, transform);
          instantiated.name = el.value.name;
          instantiateds.Add(instantiated);
        }
      }
      foreach (var el in instantiateds.Select((value, index) => new { value, index }))
        el.value.gameObject.SetActive(el.index == index);
    }

    void ILife.Awake() => Activate(true);
    // 'RouterBase<T>.ILife.Awake()': containing type does not implement interface 'ILife'

    public override void Next() {
      base.Next();
      Activate();
    }
    public override void Prev() {
      base.Prev();
      Activate();
    }
    public void Pick(T target) {
      var result = items.FindIndex((el) => el.name == nameof(target));
      if (result == -1) throw new System.Exception($"その名前はルーティングできません: {name}");
      index = result;
      Activate();
    }
  }
}