using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [SerializeField]
    private Button button = null;

    void Start()
    {
        Utils.AddClickListener(button, (btn) => {
            LogerUtils.Log($"btnName:{btn.name}");
        });
    }


}
