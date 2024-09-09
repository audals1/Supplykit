using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    #region Filed
    public enum MonsterState
    {
        Idle,
        Chase,
        Attack,
        Range,
        Damage,
        Die,
        Max
    }
    MonsterState m_monsterState;
    Vector3 m_dir;
    [SerializeField]
    float m_speed;
    float m_detectDist;
    float m_attackDist;
    float m_sqrAttackDist;
    float m_sqrDetectDist;
    Rigidbody2D m_rigidbody;
    [SerializeField] BoxCollider2D m_attArea;
    [SerializeField] Transform m_follow;
    [SerializeField ]PlayerController m_target;
    Animator m_animator;
    SpriteRenderer m_render;
    public Status m_status;


    public void SetTarget(PlayerController player)
    {
        m_target = player;
    }
    void SetState(MonsterState state)
    {
        m_monsterState = state;
    }

    void InitMonster()
    {
        m_status = new Status(1,2f,1f);
        m_attackDist = 2.5f;
        m_detectDist = 10f;
        m_sqrAttackDist = Mathf.Pow(m_attackDist, 2f);
        m_sqrDetectDist = Mathf.Pow(m_detectDist, 2f);
    }
    #endregion
    void MonsterAI()
    {
        switch (m_monsterState)
        {
            case MonsterState.Idle:
                if (CheckArea(m_target.transform.position, m_sqrAttackDist))
                {
                    //Attack();
                }
                break;
            case MonsterState.Chase:
                break;
            case MonsterState.Attack:

                break;
            case MonsterState.Range:
                break;
            case MonsterState.Damage:
                break;
        }
    }

    public void SetDamage()
    {
        float damage = m_status.m_attack - m_status.m_defence;
        m_status.m_hp -= (int)damage;
        if(m_status.m_hp <= 0)
        {
            SetDie();
            GameManager.Instance.score += 200; 
        }
    }
    public void Attack()
    {
        m_animator.SetTrigger("AttackMel");
        SetState(MonsterState.Attack);
        m_target.SetDamage();
        
    }
    bool CheckArea(Vector3 target, float area) //공격가능거리 도달 체크
    {
        var dist = target - transform.position;
        if (Mathf.Approximately(dist.sqrMagnitude, area) || dist.sqrMagnitude < area)
        {
            return true;
        }
        return false;
    }
    void SetDie()
    {
      m_animator.SetTrigger("Die");
      Destroy(gameObject,1f);
    }
    
    void MoveToTarget()
    {
        
        float dir = m_target.transform.position.x - transform.position.x;
        dir = (dir < 0) ? -1 : 1;
        if(dir == -1)
        {
            m_render.flipX = false;
        }
        else
        {
            m_render.flipX = true;
        }
        transform.Translate(new Vector2(dir, 0) * m_speed * Time.deltaTime);
    }
    // Start is called before the first frame update
    void Awake()
    {
        m_render = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        InitMonster();
    }
    void Start()
    {
        SetTarget(m_target);    
    }
    // Update is called once per frame
    void Update()
    {
        MoveToTarget();
        MonsterAI();
        
    }
}
