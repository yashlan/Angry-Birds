using UnityEngine;
using Yashlan.enemy;

namespace Yashlan.util
{
    public class Destroyer : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D collision)
        {
            string tag = collision.gameObject.tag;
            if (tag == "Bird" || tag == "Enemy" || tag == "Obstacle")
            {
                if(collision.GetComponent<Enemy>() != null)
                {
                    var enemy = collision.GetComponent<Enemy>();
                    enemy.IsHit = true;
                }
                
                Destroy(collision.gameObject);
            }
        }
    }
}

