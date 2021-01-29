using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    /*public bool GamePaused {
        get { return Time.timeScale == 0; }
        set { Time.timeScale = value ? 0 : 1; }
    }*/
    public bool GamePaused {
        get { return _isPaused; }
        set { _isPaused = value; }
    }
    public Stage actualStage {
        get; set;
    }
    public SlotItem prefabSlotItem = null;
    public ActionWheel prefabActionWheel = null;
    public CinematicScreen prefabCinematicStart = null;
    public CinematicScreen prefabCinematicEnd = null;
    public CinematicScreen prefabCinematicCredit = null;
    public FadeScreen blackScreen;
    public float stageFadeDuration;
    public float dialogMinDuration;

    [SerializeField] Stage[] stages = null;
    [SerializeField] Stage startingStage = null;
    GameObject canvas;
    bool _isPaused = false;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        canvas = FindObjectOfType<Canvas>().gameObject;
    }

    private void Start() {
        StartGame();
    }

    private void Update() {
    }

    public void StartGame() {
        //Debug.Log("Start Game with " + startingStage?.name);
        //Debug.Log(cinematic.name + " has finished instantiating.");
        StartCoroutine(nameof(PlayStartCinematic));
    }

    public void WinGame() {
        StartCoroutine(nameof(RoutineWinGame));
    }

    IEnumerator RoutineWinGame() {
        yield return PlayEndCinematic();
        //yield return PlayCreditCinematic();
        MenuManager.Instance.LoadMainMenuScene();
    }

    IEnumerator PlayStartCinematic() {
        CinematicScreen cinematic = Instantiate(prefabCinematicStart, canvas.transform);
        yield return cinematic.PlayCinematic();
        yield return Utils.WaitForUnscaledSeconds(0.5f);
        Destroy(cinematic.gameObject);
        yield return startingStage.RoutineEnter(stageFadeDuration);
    }

    IEnumerator PlayEndCinematic() {
        yield return actualStage.RoutineLeave(stageFadeDuration);
        yield return Utils.WaitForUnscaledSeconds(0.5f);
        CinematicScreen cinematic = Instantiate(prefabCinematicEnd, canvas.transform);
        yield return cinematic.PlayCinematic();
        Destroy(cinematic.gameObject);
    }

    IEnumerator PlayCreditCinematic() {
        CinematicScreen cinematic = Instantiate(prefabCinematicEnd, canvas.transform);
        yield return cinematic.PlayCinematic();
    }

    public void LoadStage(Stage stage) {
        actualStage.Next(stage);
    }

    public void StartDialogue(Action action, InteractableType targetType, InteractableType otherType, bool success) {
        //Debug.Log("Action: " + action + ", Target: " + targetType + ", Other: " + otherType + ", Success: " + success);
        try {
            string dialog = LanguageManager.Instance.GetText(action, targetType, otherType);
            //Debug.Log(dialog);
            MenuManager.Instance.OpenDialogBubble(dialog);
            float duration = dialogMinDuration + dialog.Length * 0.05f;
            CancelInvoke(nameof(EndDialog));
            Invoke(nameof(EndDialog), duration);
        } catch(TextNotFoundException) {
            // bypass dialog if text is not found
        }
    }

    public void StopStageThings() {
        // stop dialogs
        CancelInvoke(nameof(EndDialog));
        EndDialog();
        // stop music
    }

    void EndDialog() {
        MenuManager.Instance.CloseDialogBubble();
    }
}
