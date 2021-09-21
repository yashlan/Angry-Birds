using UnityEngine;

namespace Yashlan.util
{
    public class AutoDestroy : MonoBehaviour
    {
        void Start() => Destroy(gameObject, 1f);
    }
}

