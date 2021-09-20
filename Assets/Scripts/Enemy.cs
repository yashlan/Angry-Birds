using UnityEngine;
using UnityEngine.Events;

namespace Yashlan.enemy 
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private float _health;

        public UnityAction<GameObject> OnEnemyDestroyed = delegate { };

        private bool _isHit = false;

        void OnDestroy()
        {
            if (_isHit)
            {
                OnEnemyDestroyed(gameObject);
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<Rigidbody2D>() == null) return;

            if (collision.gameObject.tag == "Bird")
            {
                _isHit = true;
                Destroy(gameObject);
            }
            else if (collision.gameObject.tag == "Obstacle")
            {
                float damage = collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 10;
                _health -= damage;

                if (_health <= 0)
                {
                    _isHit = true;
                    Destroy(gameObject);
                }
            }
        }
    }
}
