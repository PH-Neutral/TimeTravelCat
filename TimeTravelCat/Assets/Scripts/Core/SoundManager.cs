using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour {
    public static SoundManager Instance;

    [SerializeField] AudioClip menuMusic = null, cinematicMusic = null;
    [SerializeField] AudioClip cinematicDialog = null;
    [SerializeField] AudioClip buttonSound = null, victorySound = null;
    AudioSource source;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);

        source = GetComponent<AudioSource>();
    }

    #region SpecificSounds

    public AudioSource PlayMenuMusic(bool loop) {
        if (menuMusic != null) {
            return PlayClip(menuMusic, loop);
        }
        return null;
    }

    #endregion

    public AudioSource PlayClip(AudioClip clip, bool loop = false) {
        source.clip = clip;
        source.loop = loop;
        source.Play();
        return source;
    }

    public void StopClip() {
        source.Stop();
    }

    public void ChangeVolume(Slider slider) => ChangeVolume(slider.value);
    public void ChangeVolume(float volume) {
        source.volume = volume;
    }
}