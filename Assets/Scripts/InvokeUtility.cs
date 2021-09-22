using System.Collections;
using UnityEngine;

namespace Yashlan.util
{
    public static class InvokeUtility
    {
        public static void CustomInvoke(this MonoBehaviour mb, System.Action action, float delay)
        {
            mb.StartCoroutine(InvokeRoutine(action, delay));
        }
        private static IEnumerator InvokeRoutine(System.Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action();
        }
    }
}
