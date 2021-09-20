using UnityEngine;

namespace Yashlan.bird
{
    public class YellowBird : Bird
    {
        [SerializeField]
        private float _boostForce;
        [SerializeField]
        private bool _hasBoost = false;

        public void Boost()
        {
            if (State == BirdState.Thrown && !_hasBoost)
            {
                Rigidbody.AddForce(Rigidbody.velocity * _boostForce);
                _hasBoost = true;
            }
        }

        public override void OnTap() => Boost();
    }
}
