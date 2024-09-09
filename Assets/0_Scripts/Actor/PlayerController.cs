using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Filed
    Vector2 m_dir; //플레이어 방향
    [SerializeField]
    float m_speed; //이동속도
    public float m_smallJumpPow;
    [SerializeField]
    Transform m_DuckPos;
    [SerializeField]
    Transform m_firePos;//발사체 생성위치
    Animator m_animator;
    [SerializeField]
    Rigidbody2D m_rigidbody;
    [SerializeField]
    GameObject m_bulletPrefab;
    public int m_bulletCount;
    public int m_MaxBulletCount = 50;
    public int m_damageCount = 0;
    public Status m_status;
    bool m_isSitdown;
    bool m_isGruounded;
    bool m_isfall;
    bool m_bulletCheck;
    #endregion

    void InitStatus()
    {
        m_status = new Status(5, 2f, 1f);
    }
    public void SetDamage()
    {
        float damage = m_status.m_attack - m_status.m_defence;
        m_status.m_hp -= (int)damage;
        m_damageCount++;
        if (m_status.m_hp <= 0)
        {
            Debug.Log("die");
        }
    }
    public void BulletCheke()
    {
        if(m_bulletCount < m_MaxBulletCount)
        {
            m_bulletCheck = true;
        }
        else
        {
            m_bulletCheck = false;
        }
    }
    void MotionControl()
    {
        BulletCheke();
        float move = Input.GetAxis("Horizontal");
        if (m_bulletCheck)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                var obj = Instantiate(m_bulletPrefab);
                var bullet = obj.GetComponent<BulletController>();
                if (!m_isSitdown)
                {
                    bullet.SetBullet(m_firePos.position, transform.eulerAngles.y == 0f ? Vector3.right : Vector3.left, "Enemy");
                }
                else
                {
                    bullet.SetBullet(m_DuckPos.position, transform.eulerAngles.y == 0f ? Vector3.right : Vector3.left, "Enemy");
                }
                m_bulletCount++;
            }
        }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    m_dir = Vector3.right;
                    m_animator.SetTrigger("Walk");
                    transform.eulerAngles = new Vector3(0f, 0f, 0f);
                    m_isSitdown = false;

                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    m_dir = Vector3.left;
                    m_animator.SetTrigger("Walk");
                    transform.eulerAngles = new Vector3(0f, 180f, 0f);
                    m_isSitdown = false;
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    m_animator.SetTrigger("Duck");
                    m_isSitdown = true;
                }

                if (Input.GetKeyDown(KeyCode.C))
                {
                    m_smallJumpPow = 25f;
                    m_dir = Vector3.up;
                    m_animator.SetTrigger("SmallJump");
                    m_rigidbody.AddForce(Vector2.up * m_smallJumpPow, ForceMode2D.Impulse);
                    m_isSitdown = false;
                }
                if (Input.GetKeyDown(KeyCode.X))
                {
                    m_smallJumpPow = 40f;
                    m_dir = Vector3.up;
                    m_animator.SetTrigger("SmallJump");
                    m_rigidbody.AddForce(Vector2.up * m_smallJumpPow, ForceMode2D.Impulse);
                    m_isSitdown = false;
                }


                if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.DownArrow))
                {
                    m_animator.SetTrigger("Idle");
                    m_isSitdown = false;
                    m_dir = Vector3.zero;
                }
                if (!m_isGruounded)
                {
                    if (m_rigidbody.velocity.y < 0f)
                    {
                        if (!m_isfall)
                        {
                            m_animator.SetInteger("JumpState", 2);
                            m_isfall = true;
                        }
                    }
                }
                m_rigidbody.velocity = new Vector2(move * m_speed, m_rigidbody.velocity.y);
            
        

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("BackGround"))
        {
            if(m_dir != Vector2.zero)
            {
                m_isGruounded = true;
                m_isfall = false;
                m_animator.SetInteger("JumpState", 0);
                m_animator.SetBool("IsMove", true);
            }
            else
            {
                m_animator.SetInteger("JumpState", 0);
                m_animator.SetBool("IsMove", false);
            }
            
            
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("BackGround"))
        {
            m_isGruounded = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        InitStatus();
        m_damageCount = 0;
    }
    
    void Update()
    {
        MotionControl();

    }
    

}
