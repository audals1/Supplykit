using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    GameObject m_ExplosionPrefab;
    Vector3 m_dir = Vector3.right;
    [SerializeField]
    float m_speed = 15f;
    float m_amount = 0f;
    SpriteRenderer m_spriteRenderer;
    Rigidbody2D m_rigidbody;
    Vector3 m_prevPos;
    [SerializeField]
    MonsterController m_monster;
    string filter;

    public void SetBullet(Vector3 Position, Vector3 dir, string filter)
    {
        transform.position = Position;
        m_dir = dir;
        if (dir == Vector3.left)
        {
            m_spriteRenderer.flipY = true;
        }
        else
        {
            m_spriteRenderer.flipY = false;
        }
        m_rigidbody.AddForce(m_dir * m_speed, ForceMode2D.Impulse);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer(filter))
        {
            Debug.Log("hit");
            var monster = collision.transform.GetComponent<MonsterController>();
            monster.SetDamage();
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        m_amount = m_speed * Time.deltaTime;
        m_prevPos = transform.position;
        transform.position += m_dir * m_amount;
    }
}
