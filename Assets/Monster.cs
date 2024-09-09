using System.Collections;
using System.Collections.Generic;
using Actor;
using Common;
using UnityEngine;
using Game.StageMap;

public class Monster : MonoBehaviour
{
    [System.Serializable]
    public enum MonsterState
    {
        Idle,
        Walk,
        Attack,
        Duck,
        Jump,
        Death,
        Max,
    }
    public MonsterData _data; //temperate!!!! �ݵ�� �����

    public Animator m_animator;
    public SpriteRenderer m_sprite;
    public Collider2D capsule, rangeCol, meleeCol;
    public Bullet bp;
    public Transform firePosition;
    public GameObject foo;

    private int _hp;

    public int HP
    {
        get => _hp;
        set
        {
            _hp = value;
            if (_hp <= 0)
            {
                if (foo != null)
                {
                    Instantiate(foo, transform.position, Quaternion.identity);
                }

                Destroy(gameObject);
            }
        }
    }

    [SerializeField]
    private MonsterState m_state;
    [SerializeField]
    private Transform m_target;
    private float m_speed;
    private float vy = 0;
    private float g, jumpPower, highJumpPower;
    private bool isGround = true;
    private bool isRange;
    private float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Initiate(_data);
    }

    public Vector3 CharacterPosition
    {
        get
        {
            var pos = m_target.transform.position;
            pos.y += 1.5f;
            return pos;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        switch (m_state)
        {
            case MonsterState.Idle:
                if (timer < 1.0f) break;
                // ���� ������ ����
                if (isRange && rangeCol.OverlapPoint(CharacterPosition))
                {
                    //����
                    AttackRange();
                }
                else if (!isRange && meleeCol.OverlapPoint(CharacterPosition))
                {
                    //����
                    AttackMelee();
                }
                else
                {
                    // ���� ������ �̵�
                    SetState(MonsterState.Walk);
                }
                break;
            case MonsterState.Duck:
                if (timer < 1.2f) break;
                SetState(MonsterState.Idle);
                break;
            case MonsterState.Walk:
                if(timer > 2.5f)
                {
                    SetState(MonsterState.Idle);
                    break;
                }
                float dir;
                if(m_target != null)
                {
                    dir = CharacterPosition.x - transform.position.x;
                }
                else
                {
                    dir = Random.Range(-1, 1);
                }
                dir = dir < 0 ? -1 : 1;
                if (dir < 0) m_sprite.flipX = true;
                if (dir >= 0) m_sprite.flipX = false;
                transform.Translate(dir * m_speed * Vector3.right * Time.deltaTime);
                if (!isGround)
                {
                    //transform.Translate(vy * Vector3.up);
                    vy -= -g * Time.deltaTime;
                    break;
                }
                Collider2D col = Physics2D.OverlapBox(transform.position + dir * new Vector3(2, 1), new Vector3(1, 1, 0), 0f);
                if(col != null && col.CompareTag("BackGround") && transform.position.y < 6)
                {
                    HighJump();
                    SetState(MonsterState.Jump);
                    break;
                }
                col = Physics2D.OverlapBox(transform.position + dir * new Vector3(2, 0), new Vector3(1, 1, 0), 0f);
                if(col != null && col.CompareTag("BackGround")){
                    Jump();
                    break;
                }
                break;
            case MonsterState.Jump:
                //transform.Translate(vy * Vector3.up);
                vy -= -g * Time.deltaTime;
                break;
            case MonsterState.Attack:
                if (timer < 0.6f) break;
                if (isRange) SetState(MonsterState.Duck);
                else SetState(MonsterState.Walk);
                break;
            case MonsterState.Death:
            default:
                break;
        }
    }

    public void Initiate(MonsterData data)
    {
        SetState(MonsterState.Idle);
        this.m_target = GameManager.Instance.Character.transform;
        this.m_speed = data.speed;
        this.jumpPower = data.jumpPower;
        this.highJumpPower = data.highJumpPower;
        this.isRange = data.isRange;
        this.g = -(2f * data.jumpPower) / Mathf.Pow(data.jumpTime / 2f, 2f);
        HP = data.hp * 10;
    }
    public void SetTarget(Transform player)
    {
        m_target = player;
    }
    public void SetState(MonsterState state)
    {
        m_state = state;
        if (m_state == MonsterState.Idle)
        {
            m_animator.Play("Idle");
        }
        else if (m_state == MonsterState.Walk)
        {
            m_animator.Play("Walk");
        }
        else if (m_state == MonsterState.Duck)
        {
            m_animator.Play("Duck");
        }
        else if(m_state == MonsterState.Jump)
        {
            m_animator.Play("Jump");
        }
        else if(m_state == MonsterState.Attack)
        {
            m_animator.Play("Jump");
        }
        else if(m_state == MonsterState.Death)
        {
            m_animator.Play("Death");
        }
        timer = 0f;
    }

    private void Jump()
    {
        isGround = false;
        vy = Mathf.Sqrt(2f * jumpPower * -g);
    }

    private void HighJump()
    {
        isGround = false;
        vy = Mathf.Sqrt(2f * highJumpPower * -g);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BackGround"))
        {
            if(!isGround && vy < 0)
            {
                SetState(MonsterState.Idle);
                isGround = true;
                vy = 0;
            }
        }
    }

    private void AttackRange()
    {
        SetState(MonsterState.Attack);
        var bullet = Instantiate(bp, firePosition.position, Quaternion.identity);
        float dir = CharacterPosition.x - transform.position.x;

        Vector3 direction = default;
        if (dir < 0)
        {
            direction = Vector3.left;
            m_sprite.flipX = true;
        }

        if (dir >= 0)
        {
            direction = Vector3.right;
            m_sprite.flipX = false;
        }

        bullet.transform.right = direction;
        bullet.Initialize(15, 1, false);
        SoundManager.PlaySfx(ClipType.Zap);
    }

    private void AttackMelee()
    {
        SetState(MonsterState.Attack);
        var character = m_target.GetComponent<Character>();

        if (character != null)
        {
            character.HP--;
            character.UpdateHP();
        }
    }
}
