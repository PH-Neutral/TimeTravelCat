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
    public FadeScreen blackScreen;
    public float stageFadeDuration;

    [SerializeField] Stage startingStage;
    [SerializeField] KeyCode keyPause = KeyCode.Escape;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        StartGame();
    }

    private void Update() {
        if (Input.GetKeyDown(keyPause)) {
            // pause
        }
    }

    void StartGame() {
        Debug.Log("Start Game with " + startingStage?.name);
        startingStage.Enter();
    }

    public void LoadStage(Stage stage) {
        actualStage.Next(stage);
    }

    void TogglePauseScreen() {

    }
}
