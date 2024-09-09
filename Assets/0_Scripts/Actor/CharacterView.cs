using System;
using System.Collections.Generic;
using UnityEngine;

namespace Actor
{
    public enum PlayerState
    {
        Idle,
        Walk,
        Jump,
        Crouch,
        Attack,
        CrouchAttack,
    }

    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Animator _animator;

        private readonly Dictionary<PlayerState, int> _triggerHashes = new();

        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public Animator Animator => _animator;

        private void Awake()
        {
            _triggerHashes.Add(PlayerState.Idle, Animator.StringToHash("Idle"));
            _triggerHashes.Add(PlayerState.Walk, Animator.StringToHash("Walk"));
            _triggerHashes.Add(PlayerState.Jump, Animator.StringToHash("Jump"));
            _triggerHashes.Add(PlayerState.Crouch, Animator.StringToHash("Crouch"));
            _triggerHashes.Add(PlayerState.Attack, Animator.StringToHash("Attack"));
            _triggerHashes.Add(PlayerState.CrouchAttack, Animator.StringToHash("CrouchAttack"));
        }

        public void SetState(PlayerState state)
        {
            foreach (var triggerHash in _triggerHashes)
            {
                _animator.ResetTrigger(triggerHash.Value);
            }

            _animator.SetTrigger(_triggerHashes[state]);
        }
    }
}
