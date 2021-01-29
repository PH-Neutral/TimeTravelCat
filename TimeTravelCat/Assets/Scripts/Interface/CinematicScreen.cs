using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CinematicScreen : FadeScreen {
    [SerializeField] AudioClip audioClip = null;
    [SerializeField] float durationIfNoAudio = 0f;
    [SerializeField] Sprite[] cinematicImages = null;
    [SerializeField] int[] cinematicDurationWeights = null;
    float[] cinematicDurations;
    Image screenImg;

    protected override void Awake() {
        base.Awake();
        screenImg = GetComponentInChildren<Image>();
        // make sure we have the correct number of weights
        int[] weights = new int[cinematicImages.Length];
        int totalWeight = 0;
        int weight;
        for(int i = 0; i < weights.Length; i++) {
            weight = 1;
            if(i < cinematicDurationWeights.Length) {
                weight = cinematicDurationWeights[i];
            }
            weights[i] = weight;
            //Debug.Log("weights[" + i + "] = " + weights[i]);
            totalWeight += weight;
        }
        // calculate the correct duration for each image based on audioClip length
        cinematicDurations = new float[weights.Length];
        float referenceDuration = audioClip != null ? audioClip.length : durationIfNoAudio;
        //Debug.Log("referenceDuration = " + referenceDuration + "; totalWeight = " + totalWeight);
        for (int i=0; i<cinematicDurations.Length; i++) {
            cinematicDurations[i] = referenceDuration * (weights[i] / (float)totalWeight);
            //Debug.Log(referenceDuration + " * (" + weights[i] + " / " + totalWeight + ") = " + cinematicDurations[i]);
        }
        /*Debug.Log("weights: " + weights.Length + ", images: " + cinematicImages.Length 
            + ", durationWeights: " + cinematicDurationWeights.Length + ", durations: " + cinematicDurations.Length);*/
    }

    public IEnumerator PlayCinematic() {
        //Debug.Log("Sound starts playing");
        AudioSource source = SoundManager.Instance.PlayClip(audioClip, false);
        float timer;
        for (int i=0; i<cinematicImages.Length; i++) {
            screenImg.sprite = cinematicImages[i];
            timer = cinematicDurations[i];
            while (timer > 0) {
                timer -= Time.unscaledDeltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
        while (source.isPlaying) {
            yield return new WaitForEndOfFrame();
        }
        //Debug.Log("Sound stops playing");
        yield return FadeIn(1f);
        //gameObject.SetActive(false);
    }
}