using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GreyPanel : MonoBehaviour
{
    public RectTransform rectTransform;
    public TMP_Text text;

    /// <summary>
    /// Set Position and Size of the Panel.
    /// </summary>
    /// <param name="offset">bottom left corner of the panel</param>
    /// <param name="size"></param>
    public void Initiate(Vector2 offset, Vector2 size)
    {
        rectTransform.localPosition = new Vector3(offset.x, offset.y, 0);
        rectTransform.sizeDelta = size;
    }
}
