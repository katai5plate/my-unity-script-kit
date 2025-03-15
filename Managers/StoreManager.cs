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
      //
    }

    void ILife.Update() {
      //
    }
  }
}