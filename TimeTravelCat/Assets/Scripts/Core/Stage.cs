﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour {
    [SerializeField] AudioClip backgroundMusic = null;
    Stage nextStage = null;

    public void Next(Stage nextStage) {
        this.nextStage = nextStage;
        StartCoroutine(nameof(RoutineNext));
    }
    IEnumerator RoutineNext() {
        GameManager.Instance.StopStageThings();
        yield return RoutineLeave();
        nextStage.gameObject.SetActive(true);
        nextStage.Enter();
        gameObject.SetActive(false);
    }

    public void Enter() {
        //Debug.Log("Enter Stage (start coroutine)");
        StartCoroutine(nameof(RoutineEnter));
    }
    public IEnumerator RoutineEnter() => RoutineEnter(GameManager.Instance.stageFadeDuration);
    public IEnumerator RoutineEnter(float fadeDuration) {
        //Debug.Log("Enter Stage");
        if (GameManager.Instance.blackScreen.gameObject.activeInHierarchy) {
            yield return GameManager.Instance.blackScreen.FadeIn(fadeDuration);
        }
        GameManager.Instance.actualStage = this;
        SoundManager.Instance.PlayMusic(backgroundMusic, true);
    }

    public void Leave() {
        StartCoroutine(nameof(RoutineLeave));
    }
    public IEnumerator RoutineLeave() => RoutineLeave(GameManager.Instance.stageFadeDuration);
    public IEnumerator RoutineLeave(float fadeDuration) {
        if(GameManager.Instance.blackScreen.gameObject.activeInHierarchy) {
            yield return GameManager.Instance.blackScreen.FadeOut(fadeDuration);
        }
    }
}