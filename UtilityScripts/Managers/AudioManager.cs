using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // different sources for FX and main music
    public GameObject FXAudioPlayer, musicAudioPlayer;
    Dictionary<AudioSource, string> activeSoundSources = new();
    public bool isMuted = false;

    // singleton
    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StopAll()
    {
        foreach (var source in activeSoundSources)
        {
            if (source.Key != null)
            {
                FadeOut(0.1f, source.Key);
            }
        }
    }

    public void OnClickPlaySound(AudioClip sound)
    {
        PlaySound(sound);
    }

    public void OnClickStartMusic(AudioClip music)
    {
        PlayMusic(music, true);
    }

    public void PlaySound(AudioClip sound, bool loop = false, Vector2 volumeRange = default(Vector2), Vector2 pitchRange = default(Vector2))
    {
        GameObject audioGameObject = Instantiate(FXAudioPlayer, transform);
        AudioSource source = audioGameObject.GetComponent<AudioSource>();
        source.loop = loop;
        source.clip = sound;
        source.pitch *= 1 + UnityEngine.Random.Range(pitchRange.x, pitchRange.y);
        source.volume *= 1 + UnityEngine.Random.Range(volumeRange.x, volumeRange.y);
        source.PlayDelayed(0.1f);
        activeSoundSources.Add(source, "fx");
        if (!loop)
            StartCoroutine(DestroyOnceFinished(source, sound.length));
    }

    public void PlayMusic(AudioClip music, bool loop, Vector2 pitchRange = default(Vector2))
    {
        // if another music source is playing, destroy that first
        if (activeSoundSources.ContainsValue("music"))
        {
            // Find the element in the dictionary with the value "music"
            foreach (var entry in activeSoundSources)
            {
                if (entry.Value == "music" && entry.Key != null)
                {
                    // Remove it and destroy its key through the FadeOut method
                    FadeOut(0.1f, entry.Key);
                    break;
                }
            }
        }

        GameObject audioGameObject = Instantiate(musicAudioPlayer, transform);
        AudioSource source = audioGameObject.GetComponent<AudioSource>();
        source.loop = loop;
        source.clip = music;
        source.pitch *= 1 + UnityEngine.Random.Range(pitchRange.x, pitchRange.y);
        activeSoundSources.Add(source, "music");
        source.PlayDelayed(0.1f);
        if (!loop)
            StartCoroutine(DestroyOnceFinished(source, music.length));
    }

    private IEnumerator DestroyOnceFinished(AudioSource source, float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        if (source != null)
        {
            FadeOut(0.1f, source);
        }
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0 : 1;
    }

    public void FadeOut(float fadeDuration, AudioSource source)
    {
        if (source != null)
        {
            StartCoroutine(FadeOutCoroutine(fadeDuration, source));
        }
    }

    private IEnumerator FadeOutCoroutine(float fadeDuration, AudioSource source)
    {
        if (source != null)
        {
            float startVolume = source.volume;
            while (source.volume > 0)
            {
                source.volume -= startVolume * Time.deltaTime / fadeDuration;
                yield return null;
            }

            source.volume = 0;
            source.Stop();
            Destroy(source.gameObject);

            activeSoundSources.Remove(source);
        }

    }
}