using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable {
    [SerializeField] GameObject stage;

    protected override void OnUse() {
        // go to another stage
        GameManager.Instance.actualStage.SetActive(false);
        stage.SetActive(true);
    }
}