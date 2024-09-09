using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Monster", menuName = "Assets/Monster")]
public class MonsterData : ScriptableObject
{
    public Animator animator;
    public bool isRange;
    public int hp;

    public float speed, jumpPower, highJumpPower;
    public float jumpTime;
}
