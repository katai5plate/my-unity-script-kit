using UnityEngine;
using UnityEngine.UI;

namespace UIPrefab {
  public class SpriteList : ListBase<Sprite>, ILife {
    Image image;
    bool isLoop = false;
    float time = 0f;
    float intervalChange = 1f;

    void ILife.Awake() {
      image = GetComponent<Image>();
    }

    public override void Next() {
      base.Next();
      image.sprite = Current;
    }
    public override void Prev() {
      base.Prev();
      image.sprite = Current;
    }
    public void ResetLoop() => this.index = 0;
    public void StartLoop(float interval, bool reset = true) {
      if (reset) ResetLoop();
      intervalChange = interval;
      time = 0f;
      isLoop = true;
    }
    public void StartLoopWhenStopped(float interval, bool reset = true) {
      if (isLoop) return;
      StartLoop(interval, reset);
    }
    public void StopLoop() => isLoop = false;
    public void StopLoopWhenStarted() {
      if (!isLoop) return;
      StopLoop();
    }
    void ILife.Update() {
      if (isLoop) {
        time += Time.deltaTime;
        if (time >= intervalChange) {
          time -= intervalChange;
          Next();
        }
      }
    }
  }
}