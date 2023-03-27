using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public enum Sound
    {
        BuildingPlaced,
        BuildingDamaged,
        BuildingDestroyed,
        EnemyDie,
        EnemyHit,
        GameOver,
    }

    private AudioSource audioSource;
    private Dictionary<Sound, AudioClip> soundAuddioClipDictionary;
    private float volume = .5f;

    private void Awake()
    {
        Instance = this;

        audioSource = GetComponent<AudioSource>();

        volume = PlayerPrefs.GetFloat("soundVolume", .5f);
        soundAuddioClipDictionary = new Dictionary<Sound, AudioClip>();

        foreach (Sound sound in System.Enum.GetValues(typeof(Sound)))
        {
            soundAuddioClipDictionary[sound] = Resources.Load<AudioClip>(sound.ToString());
        }
    }
    public void PlaySound(Sound sound)
    {
        audioSource.PlayOneShot(soundAuddioClipDictionary[sound], volume);
    }
    public void InceraseVolume()
    {
        volume += .1f;
        volume = Mathf.Clamp01(volume);
        //https://docs.unity3d.com/kr/530/ScriptReference/Mathf.Clamp01.html
        //Mathf.Clamp01() 0이하면 0으로 1이상이면 1로
        PlayerPrefs.SetFloat("soundVolume", volume);
    }
    public void DecreaseVolume()
    {
        volume -= .1f;
        volume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("soundVolume", volume);
    }
    public float GetVolume()
    {
        return volume;
    }
}
