using System.Collections.Generic;

/// <summary>
/// UILayer ·Ö²ã
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
    Scale,      //Ëõ·Å
    Right_Show, //ÓÒ²àµ¯³ö

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