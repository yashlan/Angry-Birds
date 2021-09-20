using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yashlan.bird;

namespace Yashlan.trail
{
    public class TrailController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _trail;

        private Bird _targetBird;
        private List<GameObject> _trails;

        void Start()
        {
            _trails = new List<GameObject>();
        }

        public void SetBird(Bird bird)
        {
            _targetBird = bird;

            for (int i = 0; i < _trails.Count; i++)
            {
                Destroy(_trails[i].gameObject);
            }

            _trails.Clear();
        }

        public IEnumerator SpawnTrail()
        {
            _trails.Add(Instantiate(_trail, _targetBird.transform.position, Quaternion.identity));

            yield return new WaitForSeconds(0.1f);

            if (_targetBird != null && _targetBird.State != Bird.BirdState.HitSomething)
            {
                StartCoroutine(SpawnTrail());
            }
        }
    }
}
