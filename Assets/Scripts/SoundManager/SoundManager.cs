using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource _soundEffectSource;
    [SerializeField] private AudioClip _buttonClickSound;
    [SerializeField] private AudioClip _chipSound;
    [SerializeField] private AudioClip _wheelSound;
    [SerializeField] private AudioClip _ballHitSound;
    [SerializeField] private AudioClip _winSound;

    public void PlayWheelSound()
    {
        _soundEffectSource.clip = _wheelSound;
        _soundEffectSource.Play();
    }

    public void PlayBallHitSound()
    {
        _soundEffectSource.clip = _ballHitSound;
        _soundEffectSource.Play();
    }

    public void PlayWinSound()
    {
        _soundEffectSource.PlayOneShot(_winSound);
    }

    public void PlayButtonClickSound()
    {
        _soundEffectSource.PlayOneShot(_buttonClickSound);
    }

    public void PlayChipSound()
    {
        _soundEffectSource.PlayOneShot(_chipSound);
    }
}