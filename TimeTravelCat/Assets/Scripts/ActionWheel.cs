using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionWheel : MonoBehaviour {
    public Interactable Item {
        get { return _item; }
        set {
            _item = value;
            InitializeGraphics();
        }
    }

    float FullRadius {
        get { return background.bounds.extents.x; }
    }
    float CenterRadius {
        get { return center.bounds.extents.x; }
    }
    float ButtonAngle {
        get { return 360f / (float)actions.Length; }
    }
    [SerializeField] Sprite spriteBackground;
    [SerializeField] Sprite spriteCenter, spriteArm;
    [SerializeField] SpriteMask prefabMask;
    [SerializeField] Color backgroundColor;
    [SerializeField] Sprite[] spriteIcons;
    [SerializeField] SpriteRenderer prefabIcon;
    [SerializeField] Action[] actions;
    SpriteRenderer background;
    SpriteRenderer[] icons;
    SpriteMask center;
    Transform tMasks, tIcons;
    Interactable _item;

    int hoveredButton = -1;
    bool firstFrame = true;

    private void Awake() {
        background = transform.GetComponentInChildren<SpriteRenderer>();
        tMasks = transform.GetChild(1);
        tIcons = transform.GetChild(2);

        //InitializeGraphics();
    }

    void InitializeGraphics() {
        background.sprite = spriteBackground;
        background.color = backgroundColor;

        center = Instantiate(prefabMask, tMasks);
        center.sprite = spriteCenter;

        actions = Item.AvailableActions;
        icons = new SpriteRenderer[actions.Length];
        for (int i = 0; i < actions.Length; i++) {
            SpriteMask arm = Instantiate(prefabMask, transform.position, Quaternion.Euler(-Vector3.forward * (i + 0.5f) * ButtonAngle), tMasks);
            arm.sprite = spriteArm;
            Vector3 iconVector = Vector3.up * (FullRadius + CenterRadius) * 0.5f;
            icons[i] = Instantiate(prefabIcon, Quaternion.Euler(-Vector3.forward * i * ButtonAngle) * iconVector + transform.position, transform.rotation, tIcons);
            //Debug.Log("icons[i] : " + icons[i]);
            //Debug.Log("actions[i] : " + actions[i] + "(" + (int)actions[i] + ")");
            //Debug.Log("spriteIcons.Length : " + spriteIcons.Length);
            icons[i].sprite = spriteIcons[(int)actions[i] - 1];
        }
    }

    private void Update() {
        if (Input.GetMouseButtonDown(1)) {
            Close();
        } else if(Input.GetMouseButtonDown(0) && !firstFrame) {
            //Debug.LogError("hoveredButton = " + hoveredButton);
            if (hoveredButton < 0) {
                Close();
            } else {
                ChooseAction();
            }
        }
        firstFrame = false;
    }

    void ChooseAction() {
        Action action = GetActionFromIndex(hoveredButton);
        if(Item.OnAction(action)) {
        }
        Close();
        //Debug.Log("Action: " + action.ToString());
    }

    void Close() {
        Item.wheelExists = false;
        Destroy(gameObject);
    }

    void HoverButton(int index, bool hovered = true) {
        if (index < 0) { return; }
        icons[index].color = hovered ? Color.red : Color.white;
    }

    int GetButtonIndex(Vector3 mousePosition) {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePos.z = 0;
        Vector3 rVector = mousePos - transform.position;
        //Debug.DrawLine(transform.position, mousePos, Color.green, 0.5f);
        //Debug.Log("rVector.magnitude = " + rVector.magnitude);
        if (rVector.magnitude < CenterRadius || rVector.magnitude > FullRadius) { return -1; }
        float angle = (Vector3.SignedAngle(Vector3.up, rVector, -Vector3.forward) + 360f + ButtonAngle * 0.5f) % 360;
        return GetButtonIndexFromAngle(angle);
        //Debug.Log("Angle = " + angle + "; index: " + buttonIndex);
        
    }

    Action GetActionFromIndex(int index) {
        if(index >= 0 && index < actions.Length) {
            return actions[index];
        }
        return Action.None;
    }

    int GetButtonIndexFromAngle(float angle) {
        return (int)(angle / ButtonAngle);
    }

    private void OnMouseOver() {
        int buttonIndex = GetButtonIndex(Input.mousePosition);
        if (buttonIndex != hoveredButton) {
            HoverButton(hoveredButton, false);
            HoverButton(buttonIndex, true);
            hoveredButton = buttonIndex;
        }
    }

    private void OnMouseExit() {
        HoverButton(hoveredButton, false);
        hoveredButton = -1;
    }
}