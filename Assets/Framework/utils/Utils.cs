using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Utils
{
    /// <summary>
    /// 添加按钮点击事件
    /// </summary>
    /// <param name="button"></param>
    /// <param name="action"></param>
    public static void AddBtnClickListener(Button button, Action<Button> action)
    {
        if (button == null) return;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => {
            action(button);
        });
    }

    /// <summary>
    /// 播放动画并在完成时回调
    /// </summary>
    /// <param name="animation"></param>
    /// <param name="clipName"></param>
    /// <param name="onFinish"></param>
    public static void playAnimation(Animation animation, string clipName, Action onFinish = null)
    {
        if (animation == null || string.IsNullOrEmpty(clipName))
            return;

        AnimationClip clip = animation.GetClip(clipName);
        if (clip == null)
            return;

        // 移除之前添加的所有 AnimationEvent（只移除自定义的，避免影响原有事件）
        var events = new List<AnimationEvent>(clip.events);
        events.RemoveAll(e => e.functionName == "__OnAnimationFinish");
        clip.events = events.ToArray();

        if (onFinish != null)
        {
            AnimationEvent animationEvent = new AnimationEvent
            {
                functionName = "__OnAnimationFinish",
                time = clip.length
            };
            clip.AddEvent(animationEvent);

            // 通过 MonoBehaviour 代理回调
            AnimationEventProxy.Attach(animation.gameObject, onFinish);
        }

        animation.Play(clipName);
    }
}

// 事件代理类
public class AnimationEventProxy : MonoBehaviour
{
    private Action _onFinish;

    public static void Attach(GameObject go, Action onFinish)
    {
        var proxy = go.GetComponent<AnimationEventProxy>();
        if (proxy == null)
            proxy = go.AddComponent<AnimationEventProxy>();
        proxy._onFinish = onFinish;
    }

    // 必须是 public，且与 AnimationEvent 的 functionName 匹配
    public void __OnAnimationFinish()
    {
        _onFinish?.Invoke();
        _onFinish = null;
    }
}
