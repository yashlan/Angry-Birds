using System.Collections.Generic;
using UnityEngine;
using Yashlan.util;

namespace Yashlan.audio
{
    public class AudioManager : SingletonBehaviour<AudioManager>
    {
        public const string BIRD_DIE_SFX    = "bird-dead";
        public const string BIRD_HIT_SFX    = "bird-hit";
        public const string BIRD_LAUNCH_SFX = "bird-launch";
        public const string ENEMY_DIE_SFX   = "enemy-dead";
        public const string GAME_WIN_SFX    = "game-win";
        public const string GAME_LOSE_SFX   = "game-lose";

        [Header("BGM")]
        [SerializeField]
        private AudioClip _bgmSound;
        [SerializeField]
        [Range(0,1)]
        private float _bgmVolume;

        [Header("SFX")]
        [SerializeField]
        private AudioSource _audioSource;
        [SerializeField]
        private List<AudioClip> _audioClips;

        private AudioSource _bgmSource;

        void Start()
        {
            _bgmSource = gameObject.AddComponent<AudioSource>();
            _bgmSource.clip = _bgmSound;
            _bgmSource.loop = true;
            _bgmSource.volume = _bgmVolume;
            _bgmSource.Play();
        }

        public void StopBGM() => _bgmSource.Stop();

        public void PlaySFX(string name)
        {
            AudioClip sfx = _audioClips.Find(s => s.name == name);

            if (sfx == null) 
                Debug.LogError("AnjayException : no AudioClip with name : " + name);
            else
                _audioSource.PlayOneShot(sfx);
        }
    }
}
