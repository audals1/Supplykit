using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScore : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_myScore;

    // Update is called once per frame
    void Update()
    {
        m_myScore.text = GameManager.Instance.score.ToString();        
    }
}
