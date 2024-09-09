using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIHp : MonoBehaviour
{
    [SerializeField] Image m_hp1;
    [SerializeField] Image m_hp2;
    [SerializeField] Image m_hp3;
    [SerializeField] Image m_hp4;
    [SerializeField] Image m_hp5;
    [SerializeField] PlayerController m_player;
    public int m_damage;
    // Update is called once per frame
    void Update()
    {
        m_damage = m_player.m_damageCount;
        switch (m_player.m_damageCount)
        {
            case 1:
                m_hp1.fillAmount = 0;
                break;
            case 2:
                m_hp2.fillAmount = 0;
                break;
            case 3:
                m_hp3.fillAmount = 0;
                break;
            case 4:
                m_hp4.fillAmount = 0;
                break;
            case 5:
                m_hp5.fillAmount = 0;
                break;
            default:
                break;
        }
    }
}
