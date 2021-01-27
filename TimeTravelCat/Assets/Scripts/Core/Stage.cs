using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour {
    Stage nextStage = null;

    public void Next(Stage nextStage) {
        this.nextStage = nextStage;
        StartCoroutine("RoutineNext");
    }
    IEnumerator RoutineNext() {
        yield return RoutineLeave();
        nextStage.gameObject.SetActive(true);
        nextStage.Enter();
        gameObject.SetActive(false);
    }

    public void Enter() {
        //Debug.Log("Enter Stage (start coroutine)");
        StartCoroutine("RoutineEnter");
    }
    IEnumerator RoutineEnter() {
        //Debug.Log("Enter Stage");
        yield return GameManager.Instance.blackScreen.FadeIn(GameManager.Instance.stageFadeDuration);
        GameManager.Instance.actualStage = this;
    }

    public void Leave() {
        StartCoroutine("RoutineLeave");
    }
    IEnumerator RoutineLeave() {
        yield return GameManager.Instance.blackScreen.FadeOut(GameManager.Instance.stageFadeDuration);
    }
}