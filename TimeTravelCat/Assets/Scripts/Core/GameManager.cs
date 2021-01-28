using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    public bool GamePaused {
        get { return Time.timeScale == 0; }
        set { Time.timeScale = value ? 0 : 1; }
    }
    public Stage actualStage {
        get; set;
    }
    public SlotItem prefabSlotItem = null;
    public ActionWheel prefabActionWheel = null;
    public CinematicScreen cinematicScreen;
    public FadeScreen blackScreen;
    public float stageFadeDuration;
    public float dialogMinDuration;

    [SerializeField] Stage[] stages = null;
    [SerializeField] Stage startingStage = null;
    [SerializeField] KeyCode keyPause = KeyCode.Escape;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        StartCoroutine(nameof(StartGame));
    }

    private void Update() {
        if (GamePaused) { return; }
        if (Input.GetKeyDown(keyPause)) {
        }
    }

    IEnumerator StartGame() {
        //Debug.Log("Start Game with " + startingStage?.name);
        yield return cinematicScreen.PlayCinematic();
        yield return Utils.WaitForUnscaledSeconds(0.5f);
        yield return startingStage.RoutineEnter(stageFadeDuration);
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

    void StopStageThings() {
        CancelInvoke(nameof(EndDialog));
        EndDialog();

    }

    void InitStages() {
        for (int i=0; i<stages.Length; i++) {
            //
        }
    }

    void EndDialog() {
        MenuManager.Instance.CloseDialogBubble();
    }
}
