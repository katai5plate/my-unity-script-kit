using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers {
  public class InputManager : Mon, ILife {
    public static InputManager Instance { get; private set; }

    Inputs.InputActions ia;
    readonly Dictionary<string, Action<Inputs.InputActions.GameActions>> listeners = new();
    readonly Dictionary<string, MonoBehaviour> listenerOwners = new();
    static readonly Queue<KeyValuePair<string, MonoBehaviour>> pendingListeners = new();

    void ILife.Awake() {
      if (Instance == null) {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        ia = new Inputs.InputActions();
      }
      else Destroy(gameObject);
      RegisterPendingListeners();
    }

    void ILife.OnEnable() {
      ia.Enable();
      foreach (var act in ia.Game.Get()) {
        act.performed += ctx => InvokeListeners(ia.Game);
        act.started += ctx => InvokeListeners(ia.Game);
        act.canceled += ctx => InvokeListeners(ia.Game);
      }
    }

    void ILife.OnDisable() => ia.Disable();

    public void Register(MonoBehaviour owner) {
      var key = owner.GetType().Name;

      if (Instance != null && Instance == this) {
        if (owner is IAct actOwner && !listeners.ContainsKey(key)) {
          void defaultCallback(Inputs.InputActions.GameActions gameActions) {
            if (owner != null && owner.gameObject.activeSelf)
              actOwner.On(gameActions);
          }
          listeners.Add(key, defaultCallback);
          listenerOwners.Add(key, owner);
          owner.StartCoroutine(AutoRemoveOnDisable(owner));
        }
      }
      else pendingListeners.Enqueue(new KeyValuePair<string, MonoBehaviour>(key, owner));
    }

    System.Collections.IEnumerator AutoRemoveOnDisable(MonoBehaviour owner) {
      while (owner != null && owner.isActiveAndEnabled) yield return null;
      if (owner != null) Remove(owner.GetType().Name);
    }

    void Remove(string key) {
      if (listeners.ContainsKey(key)) {
        listeners.Remove(key);
        listenerOwners.Remove(key);
      }
    }

    void InvokeListeners(Inputs.InputActions.GameActions gameActions) {
      foreach (var listener in listeners.Values)
        listener?.Invoke(gameActions);
    }

    void RegisterPendingListeners() {
      while (pendingListeners.Count > 0) {
        var listener = pendingListeners.Dequeue();
        Register(listener.Value);
      }
    }
  }
}
