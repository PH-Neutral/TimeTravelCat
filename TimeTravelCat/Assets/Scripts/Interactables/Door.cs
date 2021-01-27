using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable {/*/
    [SerializeField] Stage nextStage;

    protected override void Awake() {
        base.Awake();
        _canBeUsed = true;
    }

    protected override void OnUse() {
        // go to another stage
        GameManager.Instance.LoadStage(nextStage);
    }//*/
}