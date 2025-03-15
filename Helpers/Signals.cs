using System;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Help {
  /// <summary>UniRx などの通信系</summary>
  public class Signals {
    public static void Post<T>(T message) {
      MessageBroker.Default.Publish(message);
    }
    public static void Receive<T>(Component addTo, Action<T> subscribe) {
      MessageBroker.Default
        .Receive<T>()
        .Subscribe(subscribe)
        .AddTo(addTo);
    }
    public class WithTCS {
      public TaskCompletionSource<bool> TCS { get; set; } = new();
    }
    public static Task WaitTCS(WithTCS message) =>
      message.TCS.Task;
    public static void DoneTCS(WithTCS command) =>
      command.TCS.SetResult(true);
  }
}