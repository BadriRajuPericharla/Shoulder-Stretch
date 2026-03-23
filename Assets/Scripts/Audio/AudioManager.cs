using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]private AudioSource audioSource;
    [SerializeField]private AudioClip GunShot;
    [SerializeField]private AudioClip ButtonClick;
    [SerializeField]private AudioClip ZombieDead;
    [SerializeField]private AudioClip PlayerDead;
    [SerializeField]private AudioClip WavesSound;
    [SerializeField] private AudioClip playerHurtSound;
    public void PlayGunShot()
    {
        audioSource.PlayOneShot(GunShot);
    }
    public void PlayButtonClick()
    {
        audioSource.PlayOneShot(ButtonClick);
    }
    public void PlayZombieDead()
    {
        audioSource.PlayOneShot(ZombieDead);
    }
    public void PlayPlayerDead()
    {
        audioSource.PlayOneShot(PlayerDead);
    }
    public void PlayWavesSound()
    {
        audioSource.PlayOneShot(WavesSound);
    }

    public void PlayPlayerDamageTakenSound()
    {
        audioSource.PlayOneShot(playerHurtSound);
    }

}
