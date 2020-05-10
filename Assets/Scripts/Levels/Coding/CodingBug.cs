using System;
using UnityEngine;

namespace Levels.Coding
{
    public class CodingBug : MonoBehaviour
    {
        public MovementState movementState = MovementState.Run;
        public float speed = 5f;
        public float ultraSpeed = 10f;
        public float equalTolerance = 0.01f;
        public float threatRadius = 2f;
        public float staticDuration = 5f;
        public Vector2 _generationLimit;
        public Transform playerTransform;
    
        private static int ANIM_PARAM_DEATH = Animator.StringToHash("Death");
        private static Array ENUM_VALUES = Enum.GetValues(typeof(MovementState));
    
        private Animator _animator;
        private Vector2 _targetPosition;
        private Vector2 _newPosition;
        private float _staticStartTime;
        private float _staticEndTime;
        private bool _isThreatened;

        public enum MovementState
        {
            Run,
            Static,
            StaticAware,
            Taunt,
            UltraRun
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();

            InitMovementState(movementState);
        }
    
        private void Update()
        {
            switch (movementState)
            {
                case MovementState.Run:
                    _newPosition =  Vector2.MoveTowards(transform.position, 
                        _targetPosition, speed * Time.deltaTime);
                    Move();
                    break;
                case MovementState.Static:
                    if (Time.time > _staticEndTime)
                    {
                        InitMovementState(MovementState.UltraRun);
                    }
                    break;
                case MovementState.StaticAware:
                    break;
                case MovementState.Taunt:
                    _newPosition =  Vector2.MoveTowards(transform.position, 
                        _targetPosition, speed * Time.deltaTime);
                    Move();
                    break;
                case MovementState.UltraRun:
                    _newPosition =  Vector2.MoveTowards(transform.position, 
                        _targetPosition, ultraSpeed * Time.deltaTime);
                    Move();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (_isThreatened || movementState == MovementState.StaticAware
                || movementState == MovementState.Taunt)
            {
                CheckIfNearby();
            }
        }

        private void Move()
        {
            transform.position = _newPosition;

            if (Vector2.Distance(_targetPosition, _newPosition) < equalTolerance)
            {
                InitMovementState();
            }
        }

        private void CheckIfNearby()
        {
            if (Vector2.Distance(playerTransform.position, transform.position) < threatRadius)
            {
                InitMovementState(MovementState.UltraRun);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            TopDownCharacter2D character = other.gameObject.GetComponent<TopDownCharacter2D>();
            if (character != null)
            {
                character.playerData.AddBugKill();
                _animator.SetTrigger(ANIM_PARAM_DEATH);
            }
        }

        public void SetData(Vector2 generationLimit, Transform playerTransform, bool isThreatened)
        {
            _isThreatened = isThreatened;
            _generationLimit = generationLimit;
            this.playerTransform = playerTransform;
            
            InitMovementState();
        }

        private void InitMovementState()
        {
            int index = UnityEngine.Random.Range(0, ENUM_VALUES.Length);
            movementState = (MovementState)ENUM_VALUES.GetValue(index);
            InitMovementState(movementState);
        }

        private void InitMovementState(MovementState newMovementState)
        {
            movementState = newMovementState;
        
            switch (movementState)
            {
                case MovementState.Run:
                case MovementState.UltraRun:
                    _targetPosition = new Vector2(
                        UnityEngine.Random.Range(-_generationLimit.x, _generationLimit.x),
                        UnityEngine.Random.Range(-_generationLimit.y, _generationLimit.y)
                    );
                    break;
                case MovementState.Static:
                    _staticEndTime = Time.time + UnityEngine.Random.value * 
                        staticDuration;
                    break;
                case MovementState.StaticAware:
                    break;
                case MovementState.Taunt:
                    _targetPosition = playerTransform.position;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
