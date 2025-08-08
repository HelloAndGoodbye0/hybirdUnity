using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Utils
{
    /// <summary>
    /// ��Ӱ�ť����¼�
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
    /// ���Ŷ����������ʱ�ص�
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

        // �Ƴ�֮ǰ��ӵ����� AnimationEvent��ֻ�Ƴ��Զ���ģ�����Ӱ��ԭ���¼���
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

            // ͨ�� MonoBehaviour ����ص�
            AnimationEventProxy.Attach(animation.gameObject, onFinish);
        }

        animation.Play(clipName);
    }
}

// �¼�������
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

    // ������ public������ AnimationEvent �� functionName ƥ��
    public void __OnAnimationFinish()
    {
        _onFinish?.Invoke();
        _onFinish = null;
    }
}
