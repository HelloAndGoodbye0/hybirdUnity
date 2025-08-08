using UnityEngine;
using System.Threading.Tasks;

public abstract class UIView : MonoBehaviour
{
    protected virtual UILayer layer => UILayer.UI;


    public UILayer Layer => layer;

    public async Task Open(object param = null)
    {
        OnWillOpen(param);
        gameObject.SetActive(true);
        await PlayOpenAnimation();
        OnDidOpen(param);
    }

    public async Task Close()
    {
        OnWillClose();
        await PlayCloseAnimation();
        gameObject.SetActive(false);
        OnDidClose();
    }

    public virtual void Refresh(object param = null) { }

    /// <summary>
    /// 打开动画前回调
    /// </summary>
    protected virtual void OnWillOpen(object param = null) { }
    /// <summary>
    /// 打开动画后回调
    /// </summary>
    protected virtual void OnDidOpen(object param = null) { }
    /// <summary>
    /// 关闭动画前回调
    /// </summary>
    protected virtual void OnWillClose() { }
    /// <summary>
    /// 关闭动画后回调
    /// </summary>
    protected virtual void OnDidClose() { }

    /// <summary>
    /// 打开动画（可扩展实现）
    /// </summary>
    protected virtual Task PlayOpenAnimation()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// 关闭动画（可扩展实现）
    /// </summary>
    protected virtual Task PlayCloseAnimation()
    {
        return Task.CompletedTask;
    }
}