using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempStartButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameManager.Instance.GameStart();
    }
}
