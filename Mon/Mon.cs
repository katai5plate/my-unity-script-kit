using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;

public class Mon : MonoBehaviour {
  List<Component> nullchecks;
  ILife lifeThis;

  // ライフサイクル

  void GetLife() {
    if (lifeThis != null) return;
    if (this is ILife life) lifeThis = life;
  }
  void Awake() {
    GetLife();
    lifeThis?.Awake();
  }
  void Start() => lifeThis?.Start();
  void Update() => lifeThis?.Update();
  void FixedUpdate() => lifeThis?.FixedUpdate();
  void OnEnable() {
    if (this is IAct _)
      InputManager.Instance.Register(this);
    lifeThis?.OnEnable();
  }
  void OnDisable() => lifeThis?.OnDisable();
  void OnDestroy() => lifeThis?.OnDestroy();
  void OnDrawGizmos() {
    GetLife();
    lifeThis?.OnDrawGizmos();
  }

  // 追加機能

  /// <summary>
  /// 非同期処理中にプレイモードが終了し、
  /// 全コンポーネントが一括NULL化した場合、
  /// MissingReferenceExceptionが発生することがある。
  /// これの抑制のため、事前に監視するコンポーネントを登録する。
  /// 実際に防止するには、非同期処理中に呼び出される可能性のある関数に
  /// 以下の記述を追加すること。
  /// <code>if (Aborted()) return;</code></summary>
  protected void AbortCheckRegister(params Component[] components) {
    if (nullchecks != null)
      throw new System.Exception($"{nameof(AbortCheckRegister)} を実行できるのは１回だけ");
    nullchecks = components.ToList();
  }
  /// <summary>
  /// 参照コンポーネントがすべてNULLになっているか確認する。<br/>
  /// <b>事前に AbortCheckRegister を実行すること。</b>
  /// </summary>
  protected bool Aborted() {
    if (nullchecks == null)
      throw new System.Exception($"{nameof(AbortCheckRegister)} しろ");
    return nullchecks.All(nc => nc == null);
  }
}