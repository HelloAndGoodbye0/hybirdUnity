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
    /// �򿪶���ǰ�ص�
    /// </summary>
    protected virtual void OnWillOpen(object param = null) { }
    /// <summary>
    /// �򿪶�����ص�
    /// </summary>
    protected virtual void OnDidOpen(object param = null) { }
    /// <summary>
    /// �رն���ǰ�ص�
    /// </summary>
    protected virtual void OnWillClose() { }
    /// <summary>
    /// �رն�����ص�
    /// </summary>
    protected virtual void OnDidClose() { }

    /// <summary>
    /// �򿪶���������չʵ�֣�
    /// </summary>
    protected virtual Task PlayOpenAnimation()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// �رն���������չʵ�֣�
    /// </summary>
    protected virtual Task PlayCloseAnimation()
    {
        return Task.CompletedTask;
    }
}