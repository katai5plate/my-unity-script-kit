using UnityEngine;
using System.Collections;
using Managers;
using Help;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UIPrefab {
  public class Proxy : Mon, ILife {
    public GameObject prefab;
    public Color fill = Palette.Alpha(Palette.TheWhite, 0.1f);
    public Color stroke = Palette.TheWhite;
    public bool topMode = false;

    public static T Find<T>(Transform parent) where T : Component {
      string targetName = typeof(T).Name;
      Transform targetTransform = parent.Find(targetName);
      if (targetTransform != null) {
        T component = targetTransform.GetComponent<T>();
        if (component != null) return component;
        else throw new System.Exception($"{targetName} が見つからない");
      }
      throw new System.Exception($"{targetName} が見つからない");
    }

    public static T FindRecursive<T>(Transform parent) where T : Component {
      string targetName = typeof(T).Name;
      if (parent.name == targetName) {
        T component = parent.GetComponent<T>();
        if (component != null) return component;
      }
      foreach (Transform child in parent) {
        T result = FindRecursive<T>(child);
        if (result != null) return result;
      }
      throw new System.Exception($"{targetName} が見つからない");
    }

    IEnumerator Setup() {
      if (topMode) {
        // マネージャーが準備されるまで待つ(都度追加)
        while (
          InputManager.Instance == null ||
          StoreManager.Instance == null
        ) yield return null;
        Debug.Log(InputManager.Instance);
        StoreManager.Shared.system.managerIsReady = true;
      }
      else {
        // トップが準備完了するまで待つ
        while (
          StoreManager.Shared.system.managerIsReady != true
        ) yield return null;
      }
      // 初期処理
      if (prefab != null) {
        GameObject instantiated;
        instantiated = Instantiate(prefab, transform.parent);
        instantiated.transform.SetSiblingIndex(transform.GetSiblingIndex());
        instantiated.transform.localScale = transform.localScale;
        instantiated.name = prefab.name;

        RectTransform source = GetComponent<RectTransform>();
        RectTransform target = instantiated.GetComponent<RectTransform>();
        if (source != null && target != null) {
          target.anchorMin = source.anchorMin;
          target.anchorMax = source.anchorMax;
          target.anchoredPosition = source.anchoredPosition;
          target.sizeDelta = source.sizeDelta;
          target.pivot = source.pivot;
          target.localRotation = source.localRotation;
          target.localScale = source.localScale;
        }

        Destroy(gameObject);
      }
    }

    void ILife.Awake() => StartCoroutine(Setup());

#if UNITY_EDITOR
    void ILife.OnDrawGizmos() {
      RectTransform rt = GetComponent<RectTransform>();
      if (rt == null) return;
      Vector3[] corners = new Vector3[4];
      rt.GetWorldCorners(corners);
      Handles.color = fill;
      Handles.DrawAAConvexPolygon(corners);
      Handles.color = stroke;
      Handles.DrawAAPolyLine(corners);
      Handles.DrawLine(corners[3], corners[0]);
    }
    void CopyRectTransform() {
      if (prefab == null) return;
      RectTransform target = GetComponent<RectTransform>();
      if (target == null) return;

      GameObject tempInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
      RectTransform tempRect = tempInstance.GetComponent<RectTransform>();
      if (tempRect == null) {
        DestroyImmediate(tempInstance);
        return;
      }

      Undo.RegisterCreatedObjectUndo(tempInstance, "Copy RectTransform From Proxy Button");
      tempRect.SetParent(target.parent, false);
      tempRect.localPosition = target.localPosition;
      tempRect.localRotation = target.localRotation;
      tempRect.localScale = target.localScale;
      tempRect.SetSiblingIndex(target.GetSiblingIndex());

      Undo.RecordObject(target, "Copy RectTransform From Proxy Button");

      target.anchorMin = tempRect.anchorMin;
      target.anchorMax = tempRect.anchorMax;
      target.anchoredPosition = tempRect.anchoredPosition;
      target.sizeDelta = tempRect.sizeDelta;
      target.pivot = tempRect.pivot;
      target.localRotation = tempRect.localRotation;
      target.localScale = tempRect.localScale;

      EditorUtility.SetDirty(target);

      Undo.DestroyObjectImmediate(tempInstance);
    }
    [CustomEditor(typeof(Proxy))]
    class OptimizeRectEditor : Editor {
      public override void OnInspectorGUI() {
        DrawDefaultInspector();
        Proxy proxy = (Proxy)target;
        if (GUILayout.Button("サイズを同期")) {
          proxy.CopyRectTransform();
        }
      }
    }
#endif
  }
}