using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Help {
  /// <summary>非同期処理・コルーチン関連</summary>
  public class Asyncs {
    static IEnumerator WaitAndDoCoroutine(Action action, int frames) {
      for (int i = 0; i < frames; i++) yield return null;
      action?.Invoke();
    }
    /// <summary>指定フレーム数、処理を遅延させる</summary>
    public static void Delay(MonoBehaviour mono, int frames, Action action) =>
      mono.StartCoroutine(WaitAndDoCoroutine(action, frames));
    /// <summary>次のフレームまで、処理を遅延させる<br/>
    /// （１フレームでキャンバス描画完了が確約される）</summary>
    public static void Next(MonoBehaviour mono, Action action) =>
      Delay(mono, 1, action);
    /// <summary>
    /// 非同期処理の合間にコードを実行する
    /// <code>
    /// await Enumes.CutIn(
    ///   500, // ms
    ///   () => AsyncFunc(),
    ///   () => CutInFunc()
    /// );</code></summary>
    public static async Task CutIn(int ms, Func<Task> main, Action cutIn) {
      var task = main();
      await Task.Delay(ms);
      cutIn();
      await task;
    }
    /// <summary>条件が合うまで先に進ませない</summary>
    public static async Task Observe(Func<bool> cond) {
      while (!cond()) await Task.Yield();
    }

    class CoroutineRunner : MonoBehaviour {
      private static CoroutineRunner instance;
      public static CoroutineRunner Instance {
        get {
          if (instance == null) {
            var obj = new GameObject("[CoroutineRunner]");
            instance = obj.AddComponent<CoroutineRunner>();
            DontDestroyOnLoad(obj);
          }
          return instance;
        }
      }
    }
    static IEnumerator RunCoroutine(IEnumerator coroutine, TaskCompletionSource<bool> tcs) {
      while (coroutine.MoveNext()) yield return coroutine.Current;
      tcs.SetResult(true);
    }
    static IEnumerator RunAsyncCoroutine(Func<Task> asyncFunction, Action onComplete) {
      Task task = asyncFunction();
      Exception exception = null;
      while (!task.IsCompleted) yield return null;
      if (task.IsFaulted) {
        exception = task.Exception;
        Debug.LogError(exception);
      }
      onComplete?.Invoke();
    }
    /// <summary>コルーチン関数を非同期関数のように実行する</summary>
    public static async Task FromCoroutine(IEnumerator coroutine) {
      var tcs = new TaskCompletionSource<bool>();
      CoroutineRunner.Instance.StartCoroutine(RunCoroutine(coroutine, tcs));
      await tcs.Task;
    }
    /// <summary>非同期関数を同期的に使用する</summary>
    public static void RunSync(Func<Task> asyncFunction, Action onComplete = null) {
      CoroutineRunner.Instance.StartCoroutine(RunAsyncCoroutine(asyncFunction, onComplete));
    }
    /// <summary>コルーチン関数が終了したらコールバックを呼ぶ</summary>
    public static void RunCoroutine(IEnumerator coroutine, Action onComplete = null) =>
      RunSync(() => FromCoroutine(coroutine), onComplete);
  }
}