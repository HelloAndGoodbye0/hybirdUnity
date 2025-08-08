using System.Collections.Generic;

/// <summary>
/// UILayer �ֲ�
/// </summary>
public enum UILayer
{
    /// <summary>
    /// Game
    /// </summary>
    Game = 0,
    /// <summary>
    /// UI
    /// </summary>
    UI,
    /// <summary>
    ///PopUp
    /// </summary>
    PopUp,
    /// <summary>
    /// Dialog
    /// </summary>
    Dialog,
    /// <summary>
    /// System
    /// </summary>
    System,
    /// <summary>
    /// Notify
    /// </summary>
    Notify,
    /// <summary>
    /// Guide
    /// </summary>
    Guide

}


public enum PopType { 
    None= 0,
    Scale,      //����
    Right_Show, //�Ҳ൯��

}


public class AnimationItem
{
    public string Show { get; set; }
    public string Hide { get; set; }
}

public static class AnimationConfig
{
    public static readonly Dictionary<PopType, AnimationItem> Config = new()
    {
        { PopType.Scale, new AnimationItem { Show = "view_show", Hide = "view_dismiss" } },
        { PopType.Right_Show, new AnimationItem { Show = "view_right_show", Hide = "view_right_dismiss" } }
    };
}



public class UIConfig
{
    // ��ѡ�ֶΣ�Զ�̰���������Ϊ null��
    // ʹ���Զ�ʵ�ֵ����ԣ�Ĭ��ֵΪ null
    public string Bundle { get; set; }

    // �����ֶΣ����ڲ㼶��ʹ��ö�����ͣ�
    // ����Ϊֻ�����ԣ�ǿ��ͨ�����캯����ʼ��
    public UILayer Layer { get; }

    // �����ֶΣ�Ԥ����Դ���·��
    // ����Ϊֻ�����ԣ�ǿ��ͨ�����캯����ʼ��
    public string Prefab { get; }

    /// <summary>
    /// ���캯����ǿ�Ƴ�ʼ�������ֶΣ�
    /// </summary>
    /// <param name="layer">���ڲ㼶�����</param>
    /// <param name="prefab">Ԥ����Դ·�������</param>
    /// <param name="bundle">Զ�̰�������ѡ��</param>
    public UIConfig(UILayer layer, string prefab, string bundle = null)
    {
        Layer = layer;
        Prefab = prefab;
        Bundle = bundle;
    }

}