using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Status
{
    public int m_hp;
    public int m_hpMax;
    public float m_attack;
    public float m_defence;

    public Status(int hp, float attack, float defence)
    {
        m_hp = m_hpMax = hp;
        m_attack = attack;
        m_defence = defence;
    }
}
