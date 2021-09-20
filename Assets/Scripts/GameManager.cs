using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yashlan.bird;
using Yashlan.trail;
using Yashlan.enemy;
using Yashlan.shoot;

namespace Yashlan.manage
{
    public enum GameState
    {
        Ready,
        Start,
        Win,
        Lose
    }

    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private GameState _gameState;
        [SerializeField]
        private BoxCollider2D _tapCollider;
        [SerializeField]
        private SlingShooter _slingShooter;
        [SerializeField]
        private TrailController _trailController;
        [SerializeField]
        private bool _isGameEnded = false;
        [SerializeField]
        private List<Bird> _birds;
        [SerializeField]
        private List<Enemy> _enemies;

        private Bird _shotBird;

        void Start()
        {
            for (int i = 0; i < _birds.Count; i++)
            {
                _birds[i].OnBirdDestroyed += ChangeBird;
                _birds[i].OnBirdShot += AssignTrail;
            }

            for (int i = 0; i < _enemies.Count; i++)
                _enemies[i].OnEnemyDestroyed += CheckGameEnd;

            _tapCollider.enabled = false;
            _slingShooter.InitiateBird(_birds[0]);
            _shotBird = _birds[0];
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
        }

        void OnMouseUp()
        {
            if (_shotBird != null) _shotBird.OnTap();
        }

        private void AssignTrail(Bird bird)
        {
            _trailController.SetBird(bird);
            StartCoroutine(_trailController.SpawnTrail());
            _tapCollider.enabled = true;
        }

        private void ChangeBird()
        {
            _tapCollider.enabled = false;

            if (_isGameEnded) return;

            _birds.RemoveAt(0);

            if (_birds.Count > 0)
            {
                _slingShooter.InitiateBird(_birds[0]);
                _shotBird = _birds[0];
            }
        }

        private void CheckGameEnd(GameObject destroyedEnemy)
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                if (_enemies[i].gameObject == destroyedEnemy)
                {
                    _enemies.RemoveAt(i);
                    break;
                }
            }

            if (_enemies.Count == 0)
            {
                _isGameEnded = true;
            }
        }

        void OnDestroy()
        {
            for (int i = 0; i < _birds.Count; i++)
            {
                _birds[i].OnBirdDestroyed -= ChangeBird;
                _birds[i].OnBirdShot -= AssignTrail;
            }
        }
    }
}
