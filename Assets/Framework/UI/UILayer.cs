using System.Collections.Generic;

/// <summary>
/// UILayer 分层
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
    Scale,      //缩放
    Right_Show, //右侧弹出

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
    // 可选字段：远程包名（允许为 null）
    // 使用自动实现的属性，默认值为 null
    public string Bundle { get; set; }

    // 必填字段：窗口层级（使用枚举类型）
    // 设置为只读属性，强制通过构造函数初始化
    public UILayer Layer { get; }

    // 必填字段：预制资源相对路径
    // 设置为只读属性，强制通过构造函数初始化
    public string Prefab { get; }

    /// <summary>
    /// 构造函数（强制初始化必填字段）
    /// </summary>
    /// <param name="layer">窗口层级（必填）</param>
    /// <param name="prefab">预制资源路径（必填）</param>
    /// <param name="bundle">远程包名（可选）</param>
    public UIConfig(UILayer layer, string prefab, string bundle = null)
    {
        Layer = layer;
        Prefab = prefab;
        Bundle = bundle;
    }

}