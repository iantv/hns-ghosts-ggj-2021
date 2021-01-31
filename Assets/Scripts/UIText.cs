using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIText : MonoBehaviour
{
    public void OnPointerEnter()
    {
        this.gameObject.GetComponent<Text>().fontSize += 5;
    }

    public void OnPointerExit()
    {
        this.gameObject.GetComponent<Text>().fontSize -= 5;
    }
}
