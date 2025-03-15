using UnityEngine;

namespace Debugs {
  public class Term {
    public static void Log(params object[] messages) =>
      Debug.Log(string.Join(" ", messages));
  }
}