using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;

namespace UIPrefab {
  public class ChoiceList : ListBase<Image>, ILife {
    [NonSerialized] public List<TMP_Text> texts = new();

    public class Panel {
      public Image background;
      public TMP_Text text;
    }
    List<Panel> choiceItems;
    public List<Panel> Items => choiceItems;
    public Panel CurrentPanel => Items[index];

    void Validation() {
      foreach (var item in items) {
        var text = item.GetComponentInChildren<TMP_Text>();
        if (text == null) throw new Exception("TMP_Textが含まれていない");
        else texts.Add(text);
      }
      choiceItems = items.Select((_, i) => new Panel() {
        background = items[i],
        text = texts[i]
      }).ToList();
    }
    void ILife.Awake() => Validation();
  }
}