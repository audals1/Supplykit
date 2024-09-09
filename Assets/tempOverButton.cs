using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempOverButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameManager.Instance.GameOver();
    }
}
