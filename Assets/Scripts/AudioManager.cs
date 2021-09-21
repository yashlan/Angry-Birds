using System.Collections.Generic;
using UnityEngine;
using Yashlan.util;

namespace Yashlan.audio
{
    public class AudioManager : SingletonBehaviour<AudioManager>
    {
        public const string UNLOCKED_SLOT_SFX = "unlocked-slot";
        public const string DROP_TOWER_SFX = "drop-tower";
        public const string HIT_ENEMY_SFX = "hit-enemy";
        public const string HIT_TOWER_SFX = "hit-enemy";
        public const string ENEMY_DIE_SFX = "enemy-die";
        public const string BIRD_HIT_SFX = "bird hit oof";
        public const string BIRD_LAUNCH_SFX = "bird launch";
        public const string GAME_WIN_SFX = "game-win";
        public const string GAME_LOSE_SFX = "game-lose";

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
            if (sfx == null) return;

            _audioSource.PlayOneShot(sfx);
        }
    }
}
