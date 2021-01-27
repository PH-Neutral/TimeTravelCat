using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeScreen : MonoBehaviour {
    float Alpha {
        get { return _canvasGroup.alpha; }
        set { _canvasGroup.alpha = Mathf.Clamp01(value); }
    }
    [SerializeField] float startAlpha;
    CanvasGroup _canvasGroup;

    private void Awake() {
        _canvasGroup = GetComponent<CanvasGroup>();
        BlockRaycast(startAlpha != 0);
    }

    public IEnumerator FadeIn(float duration) => Fade(0, duration, true, false);
    public IEnumerator FadeOut(float duration) => Fade(1, duration, true, true);
    public IEnumerator Fade(float endAlpha, float duration, bool startBlockRaycasts, bool endBlockRaycasts) {
        //Debug.Log("Start Fade");
        BlockRaycast(startBlockRaycasts);
        GameManager.Instance.GamePaused = startBlockRaycasts;
        yield return FadeCanvasGroup(Alpha, endAlpha, duration);
        BlockRaycast(endBlockRaycasts);
        GameManager.Instance.GamePaused = endBlockRaycasts;
        //Debug.Log("End Fade");
    }
    IEnumerator FadeCanvasGroup(float startAlpha, float endAlpha, float duration) {
        startAlpha = Mathf.Clamp01(startAlpha);
        endAlpha = Mathf.Clamp01(endAlpha);
        while(Alpha != endAlpha) {
            Alpha = _canvasGroup.alpha + Time.unscaledDeltaTime * Mathf.Sign(endAlpha - startAlpha) / duration;
            yield return new WaitForEndOfFrame();
        }
    }

    void BlockRaycast(bool block) {
        _canvasGroup.blocksRaycasts = block;
    }
}