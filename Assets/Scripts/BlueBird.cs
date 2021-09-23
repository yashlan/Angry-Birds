using System.Collections.Generic;
using UnityEngine;
using Yashlan.util;

namespace Yashlan.bird
{
    public class BlueBird : Bird
    {
        [SerializeField]
        private float _boostForce;
        [SerializeField]
        private bool _hasSpawned = false;

        private List<BlueBird> _clone = new List<BlueBird>();

        private bool _initBird = false;

        private void Spawn()
        {
            if (State == BirdState.Thrown && !_hasSpawned)
            {
                _hasSpawned = true;
                Instantiate(SmokeEffect, transform.position, Quaternion.identity);
                for (int i = 0; i < 2; i++)
                {
                    var clone = Instantiate(gameObject, transform.position, Quaternion.identity);
                    clone.gameObject.SetActive(false);
                    _clone.Add(clone.GetComponent<BlueBird>());
                }
            }
        }
        public override void OnTap() => Spawn();

        void Update()
        {
            if (_hasSpawned && _clone.Count > 0 && !_initBird)
            {
                foreach (var bird in _clone)
                {
                    if (bird == null) return;
                    bird.gameObject.SetActive(true);
                    bird.State = BirdState.Thrown;
                    bird.GetComponent<CircleCollider2D>().enabled = true;
                }

                this.CustomInvoke(() => Init(_clone), 0.05f);
            }
        }

        private void Init(List<BlueBird> birds)
        {
            if (!_initBird)
            {
                foreach (var bird in birds)
                {
                    if (bird == null) return;
                    var rb = bird.GetComponent<Rigidbody2D>();
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    rb.AddForce(Rigidbody.velocity * _boostForce);
                }
                
                var pos_0 = new Vector3(transform.position.x, transform.position.y +  1.5f, transform.position.z);
                var pos_1 = new Vector3(transform.position.x, transform.position.y + -1.5f, transform.position.z);
                    
                birds[0].transform.position = pos_0;
                birds[1].transform.position = pos_1;  
        
                Instantiate(SmokeEffect, birds[0].transform.position, Quaternion.identity);
                Instantiate(SmokeEffect, birds[1].transform.position, Quaternion.identity);

                _initBird = true;
            }
        }
    }
}
