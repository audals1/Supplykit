using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [SerializeField]
    GameObject m_monsterPrefab;
    [SerializeField]
    GameObject[] m_spawnPoints;
    int m_spawnMonsterCount; //몬스터 등장 수
    int m_gameLv; //난이도
    [SerializeField]
    PlayerController player;
    void InitializeLv()
    {
        m_gameLv = 1;
    }
    void LvCul()
    {
        int lv = GameManager.Instance.score / 200;
        m_gameLv = lv;
        int count = 4 + (m_gameLv * 4);
        m_spawnMonsterCount = count;
    }
    public void CreateMonster()
    {
        for (int i = 0; i < m_spawnMonsterCount; i++)
        {
            var obj = Instantiate(m_monsterPrefab);
            var mon = obj.GetComponent<Monster>();
            mon.transform.position = m_spawnPoints[Random.Range(1,101) < 20 ? 0 : Random.Range(1,3)].transform.position;
            mon.SetTarget(player.transform);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        LvCul();
        CreateMonster();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
