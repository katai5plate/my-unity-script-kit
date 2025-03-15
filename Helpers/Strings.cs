using System.Text.RegularExpressions;

namespace Help {
  /// <summary>文字列操作系</summary>
  public class Strings {
    public static int Long(string text) {
      int totalLength = 0;
      foreach (char c in text)
        totalLength +=
          Regex.IsMatch(c.ToString(), @"[^\u0000-\u007F]") ? 2 : 1;
      return totalLength;
    }
  }
}