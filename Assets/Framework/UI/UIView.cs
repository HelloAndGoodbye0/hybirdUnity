using UnityEngine;
using System.Threading.Tasks;
using System;

public abstract class UIView : MonoBehaviour
{
    protected virtual UILayer layer => UILayer.UI;
    public UILayer Layer => layer;

    /// <summary>
    /// Animation
    /// </summary>
    [SerializeField]
    protected Animation _animation = null;

    /// <summary>
    /// 弹出类型
    /// </summary>
    [SerializeField]
    protected PopType _popType = PopType.None;



    public void Open(object param = null)
    {
        OnWillOpen(param);
        gameObject.SetActive(true);
        PlayOpenAnimation(() => {
            OnDidOpen(param);
        });
       
    }

    public void Close()
    {
        OnWillClose();
        PlayCloseAnimation(()=>{
            gameObject.SetActive(false);
            OnDidClose();
        });
       
       
    }

    public virtual void Refresh(object param = null) { }

    /// <summary>
    /// 打开动画前回调
    /// </summary>
    protected abstract void OnWillOpen(object param = null);
    /// <summary>
    /// 打开动画后回调
    /// </summary>
    protected abstract void OnDidOpen(object param = null);
    /// <summary>
    /// 关闭动画前回调
    /// </summary>
    protected abstract void OnWillClose();
    /// <summary>
    /// 关闭动画后回调
    /// </summary>
    protected abstract void OnDidClose();

    /// <summary>
    /// 打开动画（可扩展实现）
    /// </summary>
    protected virtual void PlayOpenAnimation(Action action = null)
    {
        if (_popType != PopType.None)
        {
            //根据类型播放动画
            if (_animation != null && AnimationConfig.Config.TryGetValue(_popType, out AnimationItem item))
            {
                Utils.playAnimation(_animation, item.Show, () => {
                    action?.Invoke();
                });
            }
            else
            {
                LogerUtils.LogWarning($"UIView: {_popType} PlayOpenAnimation not found or Animation component is missing.");
                action?.Invoke();
            }
        }
        else
        {
            action?.Invoke();
        }
            
    }

    /// <summary>
    /// 关闭动画（可扩展实现）
    /// </summary>
    protected virtual void PlayCloseAnimation(Action action = null)
    {
        if (_popType != PopType.None)
        {
            //根据类型播放动画
            if (_animation != null && AnimationConfig.Config.TryGetValue(_popType, out AnimationItem item))
            {
                Utils.playAnimation(_animation, item.Hide, () => {
                    action?.Invoke();
                });
            }
            else
            {
                action?.Invoke();
                LogerUtils.LogWarning($"UIView: {_popType} animation PlayCloseAnimation not found or Animation component is missing.");
            }
        }
        else
        {
            action?.Invoke();
        }
    }
}