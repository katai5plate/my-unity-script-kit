using UnityEngine;

namespace Managers {
  public class StoreManager : Mon, ILife {
    public static StoreManager Instance { get; private set; }
    readonly Types.StoreData shared = new();
    public static Types.StoreData Shared => Instance.shared;

    void ILife.Awake() {
      if (Instance == null) {
        Instance = this;
        DontDestroyOnLoad(gameObject);
      }
      else {
        Destroy(gameObject);
      }
      Application.targetFrameRate = 60;
      QualitySettings.vSyncCount = 0;
    }

    void ILife.Start() {
      shared.session.status.hagar.Set(
        "姉御", "よくない？",
        new(){
          Help.Palette.Red,
          Help.Palette.PinkLight,
          Help.Palette.GreenYellow,
        },
        (
          (0, "ローズ (SB)"),
          (1, "パンラズナ (AW)"),
          (1, " "),
          (2, "レモンミント (FM)"),
          (2, " ")
        )
      );
      shared.session.work.shisha.breath.Reset(null);
    }

    void ILife.Update() {
      shared.session.status.sober.Set(
        0.5f + Mathf.Sin(Time.frameCount * Time.fixedDeltaTime / 8f) * 0.5f,
        0.25f + Mathf.Sin(Time.frameCount * Time.fixedDeltaTime / 128f) * 0.25f
      );
      shared.session.status.frast.Set(
        0.5f + Mathf.Sin(Time.frameCount * Time.fixedDeltaTime / 8f) * 0.5f,
        Mathf.Clamp01(Mathf.Sin(Time.frameCount * Time.fixedDeltaTime / 48f) * 0.5f)
      );
      shared.session.status.vital.Set(
        0.5f + Mathf.Sin(Time.frameCount * Time.fixedDeltaTime / 8f) * 0.5f,
        0.25f + Mathf.Sin(Time.frameCount * Time.fixedDeltaTime / 128f) * 0.25f
      );
      shared.session.time =
        90f + Mathf.Sin(Time.frameCount * Time.fixedDeltaTime / 16f) * 90f;
      shared.playerStatus.money =
        Mathf.FloorToInt(Time.frameCount);

      shared.session.work.shisha.breath.Update();
    }
  }
}