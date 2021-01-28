using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour {
    Stage nextStage = null;

    public void Next(Stage nextStage) {
        this.nextStage = nextStage;
        StartCoroutine(nameof(RoutineNext));
    }
    IEnumerator RoutineNext() {
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
        yield return GameManager.Instance.blackScreen.FadeIn(fadeDuration);
        GameManager.Instance.actualStage = this;
    }

    public void Leave() {
        StartCoroutine(nameof(RoutineLeave));
    }
    IEnumerator RoutineLeave() {
        yield return GameManager.Instance.blackScreen.FadeOut(GameManager.Instance.stageFadeDuration);
    }
}