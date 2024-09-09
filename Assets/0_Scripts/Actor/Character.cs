using System;
using Common;
using Game;
using Game.StageMap;
using NaughtyAttributes;
using UnityEngine;

namespace Actor
{
    public partial class Character : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _jumpHeight;
        [SerializeField] private float _jumpDuration;
        [SerializeField] private float _highJumpHeight;
        [SerializeField] private Transform _view;
        [SerializeField] private CharacterView _characterView;
        
        [SerializeField] private GroundChecker _groundChecker;

        [SerializeField] private Rigidbody2D _rigidbody;

        [ShowIf(nameof(IsRuntime))]
        [SerializeField] private float _velocity;

        [ShowIf(nameof(IsRuntime))]
        [SerializeField] private bool _isGrounded;

        public bool IsRuntime => Application.isPlaying;

        private float _standardWidth;
        private bool _isCrouching;

        private int _hp = 3;

        public int HP
        {
            get => _hp;
            set
            {
                int prev = _hp;
                _hp = value;

                if (prev > _hp)
                {
                    SoundManager.PlaySfx(ClipType.Damaged);
                }
                
                UpdateHP();
            }
        } 
        
        private void Awake()
        {
            _groundChecker.Initialize(this);
            _standardWidth = _characterView.SpriteRenderer.bounds.size.x;
            
            _hand.DestroyAllChildren();
        }

        private void Start()
        {
            UpdateAmmoCount();
            UpdateHP();
        }

        public void UpdateHP()
        {
            StageManager.Instance.UpdateHP(HP);
            if (HP <= 0)
            {
                GameManager.Instance.GameOver();
            }
        }
        
        private void Update()
        {
            float gravity = -(2f * _jumpHeight) / Mathf.Pow(_jumpDuration / 2f, 2f);

            if (_isGrounded)
            {
                MapManager.Instance.SetEnabledPlatformColliders(true);
            }

            _isCrouching = false;
            if (_isGrounded && _velocity <= 0)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    if (Input.GetKey(KeyCode.DownArrow))
                    {
                        SoundManager.PlaySfx(ClipType.Jump);
                        MapManager.Instance.SetEnabledPlatformColliders(false);
                        _groundChecker.ForceGroundCheck();
                    }
                    else
                    {
                        SoundManager.PlaySfx(ClipType.RealJump);
                        float jumpVelocity = Mathf.Sqrt(2f * _highJumpHeight * -gravity);
                        _velocity = jumpVelocity;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.X))
                {
                    if (Input.GetKey(KeyCode.DownArrow))
                    {
                        SoundManager.PlaySfx(ClipType.Jump);
                        MapManager.Instance.SetEnabledPlatformColliders(false);
                        _groundChecker.ForceGroundCheck();
                    }
                    else
                    {
                        SoundManager.PlaySfx(ClipType.RealJump);
                        float jumpVelocity = -gravity * _jumpDuration / 2f;
                        _velocity = jumpVelocity;
                    }
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    _isCrouching = true;
                }
            }
            else
            {
                _velocity += gravity * Time.deltaTime;
            }

            Vector2 movement = default;
            if (Input.GetKey(KeyCode.RightArrow)) movement.x += 1f;
            if (Input.GetKey(KeyCode.LeftArrow)) movement.x -= 1f;

            if (movement.x != 0)
            {
                _view.rotation = Quaternion.Euler(0, movement.x > 0 ? 0 : 180f, 0);
            }

            if (_isCrouching) movement.x = 0;

            var position = _rigidbody.position;
            position += movement * (_moveSpeed * Time.deltaTime);
            position.y += _velocity * Time.deltaTime;

            float minX = CameraController.Instance.GetBounds().min.x + _standardWidth / 2f;
            position.x = Mathf.Max(minX, position.x);
            _rigidbody.MovePosition(position);

            var state = PlayerState.Walk;
            if (!_isGrounded)
            {
                state = PlayerState.Jump;
            }
            else
            {
                if (_isCrouching) state = PlayerState.Crouch;
                else if (movement.x == 0) state = PlayerState.Idle;
            }

            bool doNotTrigger = state == PlayerState.Walk;

            if (Input.GetKeyDown(KeyCode.Z))
            {
                TryAttack(false, doNotTrigger);
            }
            else if (Input.GetKey(KeyCode.Z))
            {
                TryAttack(true, doNotTrigger);
            }

            _characterView.SetState(state);
        }

        public void OnEnterGround()
        {
            Debug.Log("Enter Ground");
            _isGrounded = true;
            if (_velocity <= 0)
            {
                _velocity = 0;
            }
        }

        public void OnExitGround()
        {
            Debug.Log("Exit Ground");
            _isGrounded = false;
        }
    }
}
