using UnityEngine;
using UnityEngine.Events;
using Yashlan.audio;
using Yashlan.manage;

namespace Yashlan.enemy 
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private GameObject _destroyEffect;
        [SerializeField]
        private float _health;

        private bool _isHit = false;

        public UnityAction<GameObject> OnEnemyDestroyed = delegate { };

        public bool IsHit 
        { 
            get => _isHit; 
            set => _isHit = value; 
        }

        void OnDestroy()
        {
            if (_isHit)
            {
                Instantiate(_destroyEffect, transform.position, Quaternion.identity);
                OnEnemyDestroyed(gameObject);
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if(GameManager.Instance.GameState == GameState.Start)
            {
                if (collision.gameObject.GetComponent<Rigidbody2D>() == null) return;

                var tag = collision.gameObject.tag;

                if (tag == "Bird")
                {
                    _isHit = true;
                    AudioManager.Instance.PlaySFX(AudioManager.ENEMY_DIE_SFX);
                    Destroy(gameObject);
                }
                else if (tag == "Obstacle")
                {
                    float damage = collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 10;
                    _health -= damage;

                    GameManager.Instance.AddScore((int)damage);

                    if (_health <= 0)
                    {
                        _isHit = true;
                        AudioManager.Instance.PlaySFX(AudioManager.ENEMY_DIE_SFX);
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
