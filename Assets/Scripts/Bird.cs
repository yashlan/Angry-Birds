using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Yashlan.audio;

namespace Yashlan.bird
{
    public class Bird : MonoBehaviour
    {
        public enum BirdState { Idle, Thrown, HitSomething }
        [SerializeField]
        private BirdState _state;
        [SerializeField]
        private GameObject _smokeEffect;
        [SerializeField]
        private Rigidbody2D _rigidBody;
        [SerializeField]
        private CircleCollider2D _circleCollider2D;

        private float _minVelocity = 0.05f;
        private bool _flagDestroy = false;

        public UnityAction OnBirdDestroyed = delegate { };
        public UnityAction<Bird> OnBirdShot = delegate { };
        public BirdState State
        {
            get => _state;
            set => _state = value;
        }
        public Rigidbody2D Rigidbody => _rigidBody;
        public GameObject SmokeEffect => _smokeEffect;

        void Start()
        {
            _rigidBody.bodyType = RigidbodyType2D.Kinematic;
            _circleCollider2D.enabled = false;
            _state = BirdState.Idle;
        }

        void FixedUpdate()
        {
            if (_state == BirdState.Idle && _rigidBody.velocity.sqrMagnitude >= _minVelocity)
            {
                _state = BirdState.Thrown;
            }

            if ((_state == BirdState.Thrown || _state == BirdState.HitSomething) &&
                _rigidBody.velocity.sqrMagnitude < _minVelocity && !_flagDestroy)
            {
                _flagDestroy = true;
                StartCoroutine(DestroyAfter(2));
            }
        }

        void OnDestroy()
        {
            if (_state == BirdState.Thrown || _state == BirdState.HitSomething)
            {
                AudioManager.Instance.PlaySFX(AudioManager.BIRD_DIE_SFX);
                Instantiate(_smokeEffect, transform.position, Quaternion.identity);
                OnBirdDestroyed();
            }
        }

        void OnCollisionEnter2D(Collision2D collision) 
        {
            _state = BirdState.HitSomething;
            AudioManager.Instance.PlaySFX(AudioManager.BIRD_HIT_SFX);
        }

        private IEnumerator DestroyAfter(float second)
        {
            yield return new WaitForSeconds(second);
            if(_state == BirdState.HitSomething) OnHitSomething();
            Destroy(gameObject);
        }

        public void MoveTo(Vector2 target, GameObject parent)
        {
            gameObject.transform.SetParent(parent.transform);
            gameObject.transform.position = target;
        }

        public void Shoot(Vector2 velocity, float distance, float speed)
        {
            AudioManager.Instance.PlaySFX(AudioManager.BIRD_LAUNCH_SFX);
            _circleCollider2D.enabled = true;
            _rigidBody.bodyType = RigidbodyType2D.Dynamic;
            _rigidBody.velocity = velocity * speed * distance;
            OnBirdShot(this);
        }

        public virtual void OnTap() { /*Do nothing*/ }

        public virtual void OnHitSomething() { /*Do nothing*/ }

    }
}
