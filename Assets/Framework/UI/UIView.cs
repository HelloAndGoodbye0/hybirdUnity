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
    /// ��������
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
    /// �򿪶���ǰ�ص�
    /// </summary>
    protected abstract void OnWillOpen(object param = null);
    /// <summary>
    /// �򿪶�����ص�
    /// </summary>
    protected abstract void OnDidOpen(object param = null);
    /// <summary>
    /// �رն���ǰ�ص�
    /// </summary>
    protected abstract void OnWillClose();
    /// <summary>
    /// �رն�����ص�
    /// </summary>
    protected abstract void OnDidClose();

    /// <summary>
    /// �򿪶���������չʵ�֣�
    /// </summary>
    protected virtual void PlayOpenAnimation(Action action = null)
    {
        if (_popType != PopType.None)
        {
            //�������Ͳ��Ŷ���
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
    /// �رն���������չʵ�֣�
    /// </summary>
    protected virtual void PlayCloseAnimation(Action action = null)
    {
        if (_popType != PopType.None)
        {
            //�������Ͳ��Ŷ���
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