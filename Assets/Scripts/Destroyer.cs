using UnityEngine;

namespace Yashlan.util
{
    public class Destroyer : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D collision)
        {
            string tag = collision.gameObject.tag;
            if (tag == "Bird" || tag == "Enemy" || tag == "Obstacle")
            {
                Destroy(collision.gameObject);
            }
        }
    }
}

