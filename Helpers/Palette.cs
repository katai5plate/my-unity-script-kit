using UnityEngine;

namespace Help {
  /// <summary>カラーセット</summary>
  public class Palette {
    public static Color FromRGB(float r, float g, float b) {
      return new Color(r / 255, g / 255, b / 255);
    }
    public static Color GreenDark => FromRGB(44, 234, 139);
    public static Color GreenLight => FromRGB(99, 255, 99);
    public static Color GreenYellow => FromRGB(171, 255, 87);
    public static Color Lemon => FromRGB(255, 255, 99);
    public static Color BlueDark => FromRGB(44, 139, 234);
    public static Color BlueLight => FromRGB(87, 171, 255);
    public static Color CyanDark => FromRGB(0, 224, 224);
    public static Color CyanLight => FromRGB(99, 255, 255);
    public static Color PurpleDark => FromRGB(139, 44, 234);
    public static Color PurpleLight => FromRGB(171, 87, 255);
    public static Color PinkDark => FromRGB(255, 99, 255);
    public static Color PinkLight => FromRGB(255, 168, 255);
    public static Color Brown => FromRGB(159, 111, 63);
    public static Color Orrange => FromRGB(255, 122, 0);
    public static Color OrrangePale => FromRGB(255, 191, 127);
    public static Color Red => FromRGB(255, 99, 99);

    public static Color TheBlack => FromRGB(0, 0, 0);
    public static Color TheWhite => FromRGB(255, 255, 255);
    public static Color TheYellow => FromRGB(255, 255, 0);

    public static Color Alpha(Color color, float a) =>
      new(color.r, color.g, color.b, a);
  }
}