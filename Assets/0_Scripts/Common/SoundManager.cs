using UnityEngine;

namespace Common
{
    public class SoundManager : SingletonMonoBehaviour<SoundManager>
    {
        [SerializeField] private AudioSource _audioSource;

        public AudioSource AudioSource => _audioSource;

        public static void PlaySfx(ClipType type)
        {
            var clip = SoundData.Instance.GetSfxClip(type);
            if (clip == null)
            {
                return;
            }
            
            Instance._audioSource.PlayOneShot(clip);
        }

        public static void PlayBgm(BgmType type)
        {
            var clip = SoundData.Instance.GetBgmClip(type);
            if (clip == null)
            {
                return;
            } 
            
            Instance._audioSource.clip = clip;
            Instance._audioSource.Play();
        }
    }
}
