using UnityEngine;

namespace Yashlan.bird
{
    public class YellowBird : Bird
    {
        [SerializeField]
        private TrailRenderer _trail;
        [SerializeField]
        private float _boostForce;
        [SerializeField]
        private bool _hasBoost = false;

        private void Boost()
        {
            if (State == BirdState.Thrown && !_hasBoost)
            {
                _trail.enabled = true;
                Instantiate(SmokeEffect, transform.position, Quaternion.identity);
                Rigidbody.AddForce(Rigidbody.velocity * _boostForce);
                _hasBoost = true;
            }
        }

        public override void OnTap() => Boost();
    }
}
