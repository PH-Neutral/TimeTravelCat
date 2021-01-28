using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour {
    public Slot slot {
        get; set;
    }
    public Interactable Item {
        get; private set;
    }

    Image img;

    private void Awake() {
        img = GetComponent<Image>();
    }

    public void FromInteractable(Interactable item) {
        Item = item;
        img.sprite = item.sprite;
        img.color = item.color;
        img.AdaptToParent(slot.transform as RectTransform);
        Item.MakeVisible(false);
        Item.MakeInteractable(false);
    }

    public void ToInteractable(Vector3 position) {
        Item.transform.position = position;
        Item.MakeVisible(true);
        Item.MakeInteractable(false);
    }
}