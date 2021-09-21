using UnityEngine;

namespace Yashlan.bird
{
    public class BrownBird : Bird
    {
        [SerializeField]
        private float _explosionForce;
        [SerializeField]
        private float _radius;
        [SerializeField]
        private float _upliftModifer;
        [SerializeField]
        private bool _hasExploded = false;
        [SerializeField]
        private bool _showRadius = false;

        public override void OnHitSomething() => Exploded();

        private void Exploded()
        {
            if (!_hasExploded)
            {
                _hasExploded = true;

                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _radius);

                foreach (Collider2D coll in colliders)
                {
                    if (coll.GetComponent<Rigidbody2D>())
                        AddExplosionForce(coll.GetComponent<Rigidbody2D>(), _explosionForce, transform.position, _radius, _upliftModifer);
                }
            }
        }

        private void AddExplosionForce(Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius, float upliftModifier = 0)
        {
            var dir = (body.transform.position - explosionPosition);
            float wearoff = 1 - (dir.magnitude / explosionRadius);
            Vector3 baseForce = dir.normalized * explosionForce * wearoff;
            baseForce.z = 0;
            body.AddForce(baseForce);

            if (_upliftModifer != 0)
            {
                float upliftWearoff = 1 - upliftModifier / explosionRadius;
                Vector3 upliftForce = Vector2.up * explosionForce * upliftWearoff;
                upliftForce.z = 0;
                body.AddForce(upliftForce);
            }
        }

        void OnDrawGizmos()
        {
            if (_showRadius)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, _radius);
            }
        }
    }
}
