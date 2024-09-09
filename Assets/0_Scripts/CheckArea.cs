using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckArea : MonoBehaviour
{
    [SerializeField]
    MonsterController m_monster;
    
    IEnumerator Coroutin_Attack()
    {
        yield return new WaitForSeconds(1f);
        m_monster.Attack();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine("Coroutin_Attack");
        }
    }
}
