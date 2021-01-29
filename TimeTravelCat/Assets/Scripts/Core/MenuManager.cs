using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    public static MenuManager Instance;

    [SerializeField] bool isMainMenu = false;
    // mainMenu
    [SerializeField] GameObject animSelected = null;
    [SerializeField] Button firstSelected = null;

    // game
    [SerializeField] GameObject panelPause = null;
    [SerializeField] GameObject panelDialog = null;
    Text dialogText = null;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        if (!isMainMenu) {
            dialogText = panelDialog.GetComponentInChildren<Text>();
        } else {
            //selectedAnim
        }
    }

    private void Start() {
        if (isMainMenu) {
            Select(firstSelected);
            SoundManager.Instance.PlayMenuMusic(true);
        }
    }

    #region MainMenuScene

    public void LoadMainMenuScene() {
        SceneManager.LoadScene(0);
    }

    public void LoadGameScene() {
        SceneManager.LoadScene(1);
    }

    public void QuitGame() {
        Application.Quit(0);
    }

    public void Select(Button btn) {
        btn.Select();
    }

    public void SetSelected(Button btn) {
        animSelected.SetActive(true);
        animSelected.transform.position = btn.transform.position;
    }

    public void SetDeselected() {
        animSelected.SetActive(false);
    }

    #endregion

    #region GameScene

    public void OpenDialogBubble(string text) {
        dialogText.text = text;
        panelDialog.SetActive(true);
    }

    public void CloseDialogBubble() {
        panelDialog.SetActive(false);
    }

    public void OpenPauseMenu() {
        GameManager.Instance.GamePaused = true;
        panelPause.SetActive(true);
    }

    public void ClosePauseMenu() {
        GameManager.Instance.GamePaused = false;
        panelPause.SetActive(false);
    }

    #endregion
}