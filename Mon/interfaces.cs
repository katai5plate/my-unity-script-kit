/// <summary>Mon で入力判定を使えるようにする</summary>
public interface IAct {
  void On(Inputs.InputActions.GameActions act) { }
}

/// <summary>Mon 専用ライフサイクルメソッド</summary>
public interface ILife {
  void Awake() { }
  void OnEnable() { }
  void OnDisable() { }
  void Start() { }
  void Update() { }
  void FixedUpdate() { }
  void OnDestroy() { }
  void OnDrawGizmos() { }
}