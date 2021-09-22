using UnityEngine;
using Yashlan.manage;

namespace Yashlan.wood
{
    public class Wood : MonoBehaviour
    {
        [SerializeField]
        private GameObject _destroyEffect;

        private float _health = 30f;

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (GameManager.Instance.GameState == GameState.Start)
            {
                if (collision.gameObject.GetComponent<Rigidbody2D>() == null) return;

                float damage = collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 10;
                _health -= damage;

                GameManager.Instance.AddScore((int)damage);

                if (_health <= 0)
                {
                    Instantiate(_destroyEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
            }
        }
    }

}
