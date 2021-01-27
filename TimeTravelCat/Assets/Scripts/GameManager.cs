using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    public SlotItem prefabSlotItem = null;
    public ActionWheel prefabActionWheel = null;
    public GameObject actualStage;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }
}
