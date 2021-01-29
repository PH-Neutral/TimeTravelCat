using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour {
    public static SoundManager Instance;

    [SerializeField] AudioClip menuMusic = null;
    [SerializeField] AudioClip buttonSound = null;
    AudioSource source;
    float volumeDialog = 1, volumeSound = 1;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        source = GetComponent<AudioSource>();
    }

    #region SpecificSounds

    public AudioSource PlayMenuMusic(bool loop) {
        if (menuMusic != null) {
            return PlayClip(menuMusic, loop);
        }
        return null;
    }

    public void PlayButtonSound() {
        PlaySound(buttonSound);
    }

    #endregion

    public void PlayMusic(AudioClip clip, bool loop) {
        PlayClip(clip, loop);
    }

    public void PlayDialog(AudioClip clip) {
        PlayClipOnce(clip, volumeDialog);
    }

    public void PlaySound(AudioClip clip) {
        PlayClipOnce(clip, volumeSound);
    }

    public AudioSource PlayClip(AudioClip clip, bool loop = false) {
        source.clip = clip;
        source.loop = loop;
        source.Play();
        return source;
    }

    void PlayClipOnce(AudioClip clip, float volumeScale) {
        source.PlayOneShot(clip, volumeScale);
    }

    public void StopClip() {
        source.Stop();
    }

    public void ChangeVolumeDialog(Slider slider) => ChangeVolumeDialog(slider.value);
    public void ChangeVolumeDialog(float volume) {
        volumeDialog = volume;
    }

    public void ChangeVolumeSound(Slider slider) => ChangeVolumeSound(slider.value);
    public void ChangeVolumeSound(float volume) {
        volumeSound = volume;
    }

    public void ChangeVolumeMain(Slider slider) => ChangeVolumeMain(slider.value);
    public void ChangeVolumeMain(float volume) {
        source.volume = volume;
    }
}