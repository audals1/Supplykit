using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIBullet : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_currentBullet;
    [SerializeField]
    TextMeshProUGUI m_maxBullet;
    [SerializeField]
    PlayerController m_player;
    void Update()
    {
        m_currentBullet.text = m_player.m_bulletCount.ToString();
    }
}
