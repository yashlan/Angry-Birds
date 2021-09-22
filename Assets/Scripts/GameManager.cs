using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Yashlan.bird;
using Yashlan.trail;
using Yashlan.enemy;
using Yashlan.shoot;
using Yashlan.util;
using Yashlan.audio;
using Yashlan.data;

namespace Yashlan.manage
{
    public enum GameState
    {
        Ready,
        Start,
        Win,
        Lose
    }

    public class GameManager : SingletonBehaviour<GameManager>
    {
        [Header("state")]
        [SerializeField]
        private GameState _gameState;

        [Header("UI")]
        [SerializeField]
        private Canvas _canvas;
        [SerializeField]
        private GameObject _panelInfoLevel;
        [SerializeField]
        private GameObject _panelInfoGame;
        [SerializeField]
        private GameObject _buttonNextLevel;
        [SerializeField]
        private Text _textScore;
        [SerializeField]
        private Text _textGameState;
        [SerializeField]
        private Text _textLevelGame;

        [Header("component")]
        [SerializeField]
        private BoxCollider2D _tapCollider;
        [SerializeField]
        private SlingShooter _slingShooter;
        [SerializeField]
        private TrailController _trailController;
        [SerializeField]
        private List<Bird> _birds;
        [SerializeField]
        private List<Enemy> _enemies;

        private Bird _shotBird;

        private int _score
        {
            get => ScoreData.Score;
            set => ScoreData.Score = value;
        }

        public GameState GameState => _gameState;

        #region onClickEvent
        public void RestartGameOnClick() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        public void GotoNextLevelOnClick() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        #endregion

        private IEnumerator InitStartGame()
        {
            _canvas.gameObject.SetActive(true);
            _panelInfoLevel.SetActive(true);
            _textLevelGame.text = SceneManager.GetActiveScene().name;
            yield return new WaitForSeconds(2f);
            _panelInfoLevel.SetActive(false);
            _gameState = GameState.Start;
            yield break;
        }

        void Start()
        {
            StartCoroutine(InitStartGame());

            for (int i = 0; i < _birds.Count; i++)
            {
                _birds[i].OnBirdDestroyed += ChangeBird;
                _birds[i].OnBirdShot += AssignTrail;
            }

            for (int i = 0; i < _enemies.Count; i++)
            {
                _enemies[i].OnEnemyDestroyed += DestroyEnemy;
            }

            _tapCollider.enabled = false;
            _slingShooter.InitiateBird(_birds[0]);
            _shotBird = _birds[0];

            _textScore.text = "Score : " + _score.ToString("000000000");
        }

        public void AddScore(int amount) 
        {
            _score += amount;
            _textScore.text = "Score : " + _score.ToString("000000000");
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
            if (_gameState == GameState.Start)
            {
                if (_birds == null) return;

                _tapCollider.enabled = false;

                _birds.RemoveAt(0);

                if (_birds.Count > 0)
                {
                    _slingShooter.InitiateBird(_birds[0]);
                    _shotBird = _birds[0];
                }

                Invoke(nameof(CheckGameEnd), 2f);
            }
        }

        private void DestroyEnemy(GameObject destroyedEnemy)
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                if (_enemies[i].gameObject == destroyedEnemy)
                {
                    _enemies.RemoveAt(i);
                    break;
                }
            }
        }

        private void CheckGameEnd()
        {
            if (_enemies.Count == 0)
                this.CustomInvoke(() => SetGameState(GameState.Win), 1f);
            else if (_enemies.Count > 0 && _birds.Count == 0)
                this.CustomInvoke(() => SetGameState(GameState.Lose), 1f);
        }

        private void SetGameState(GameState gameState)
        {
            _gameState = gameState;
            var win = gameState == GameState.Win;
            if(win) SetReward();
            _textGameState.text = win ? "You Win!" : "You Lose!";
            _panelInfoGame.SetActive(true);
            _buttonNextLevel.SetActive(win ? true : false);
            AudioManager.Instance.PlaySFX(win ? AudioManager.GAME_WIN_SFX : AudioManager.GAME_LOSE_SFX);
        }

        private void SetReward()
        {
            if(_birds.Count == 0) AddScore(1000);
            if(_birds.Count == 1) AddScore(5000);
            if(_birds.Count >= 2) AddScore(10000);
        }
    }
}

