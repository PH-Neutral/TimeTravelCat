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
    //[SerializeField] Sprite spriteBackground = null;
    [SerializeField] Sprite spriteCenter = null, spriteArm = null;
    [SerializeField] SpriteMask prefabMask = null;
    //[SerializeField] Sprite[] spriteIcons = null;
    [SerializeField] SpriteRenderer prefabIcon = null;
    [SerializeField] Action[] actions = null;
    SpriteRenderer background;
    SpriteRenderer[] icons;
    Animator[] iconAnims;
    SpriteMask center;
    Transform tMasks, tIcons;
    CircleCollider2D col;
    Interactable _item;
    int hoveredButton = -1;
    bool firstFrame = true;
    bool closeWheel = false;

    private void Awake() {
        col = GetComponent<CircleCollider2D>();
        background = transform.GetComponentInChildren<SpriteRenderer>();
        tMasks = transform.GetChild(1);
        tIcons = transform.GetChild(2);

        //InitializeGraphics();
    }

    void InitializeGraphics() {
        //background.sprite = spriteBackground;
        //background.color = backgroundColor;

        center = Instantiate(prefabMask, tMasks);
        center.sprite = spriteCenter;

        actions = Item.AvailableActions;
        icons = new SpriteRenderer[actions.Length];
        iconAnims = new Animator[actions.Length];
        for (int i = 0; i < actions.Length; i++) {
            SpriteMask arm = Instantiate(prefabMask, transform.position, Quaternion.Euler(-Vector3.forward * (i + 0.5f) * ButtonAngle), tMasks);
            arm.sprite = spriteArm;
            Vector3 iconVector = Vector3.up * (FullRadius + CenterRadius) * 0.5f;
            icons[i] = Instantiate(prefabIcon, Quaternion.Euler(-Vector3.forward * i * ButtonAngle) * iconVector + transform.position, transform.rotation, tIcons);
            //Debug.Log("icons[i] : " + icons[i]);
            //Debug.Log("actions[i] : " + actions[i] + "(" + (int)actions[i] + ")");
            //Debug.Log("spriteIcons.Length : " + spriteIcons.Length);
            iconAnims[i] = icons[i].GetComponent<Animator>();
            iconAnims[i].SetInteger("IconType", (int)actions[i]);
            //Debug.Log("Action: " + actions[i] + " (" + (int)actions[i] + ")");
        }
    }

    private void Update() {
        if (GameManager.Instance.GamePaused) { return; }
        if (closeWheel) {
            Destroy(gameObject);
        }
        CheckMouseOver();
        if (Input.GetMouseButtonDown(1)) {
            CloseNext();
        } else if(Input.GetMouseButtonDown(0) && !firstFrame) {
            //Debug.LogError("hoveredButton = " + hoveredButton);
            if (hoveredButton < 0) {
                //CloseNext();
            } else {
                ChooseAction();
            }
            CloseNext();
        }
        firstFrame = false;
    }

    void ChooseAction() {
        Action action = GetActionFromIndex(hoveredButton);
        Item.OnAction(action);
        //Debug.Log("Action: " + action.ToString());
    }

    void CloseNext() {
        closeWheel = true;
        Destroy(gameObject);
    }

    void HoverButton() {
        int iAction = 0;
        if (hoveredButton >= 0) {
            iAction = (int)actions[hoveredButton];
        }
        //icons[index].color = hovered ? Color.red : Color.white;
        for (int i=0; i<iconAnims.Length; i++) {
            Vector3 scale = Vector3.one;
            if (i == hoveredButton) {
                scale *= 1.5f;
            }
            icons[i].transform.localScale = scale;
            iconAnims[i].SetInteger("Hovered", iAction);
        }
        
    }

    int GetButtonIndex(Vector3 mousePosition) {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePos.z = 0;
        Vector3 rVector = mousePos - transform.position;
        //Debug.Log(CenterRadius + " < " + rVector.magnitude + " < " + FullRadius);
        //Debug.DrawLine(transform.position, mousePos, Color.green, 0.5f);
        //Debug.Log("rVector.magnitude = " + rVector.magnitude);
        if (rVector.magnitude < CenterRadius || rVector.magnitude > FullRadius) { return -1; }
        float angle = (Vector3.SignedAngle(Vector3.up, rVector, -Vector3.forward) + 360f + ButtonAngle * 0.5f) % 360;
        return GetButtonIndexFromAngle(angle);
        //Debug.Log("Angle = " + angle + "; index: " + buttonIndex);
        
    }

    Action GetActionFromIndex(int index) {
        if(index >= 0 && index < actions.Length) {
            //Debug.Log("index: " + index + " => Action: " + actions[index]);
            return actions[index];
        }
        return Action.None;
    }

    int GetButtonIndexFromAngle(float angle) {
        return (int)(angle / ButtonAngle);
    }

    private void CheckMouseOver() {
        if (col.HasBeenClickedOn(LayerMask.NameToLayer("GameUI"))) {
            int buttonIndex = GetButtonIndex(Input.mousePosition);
            if(buttonIndex != hoveredButton) {
                hoveredButton = buttonIndex;
                HoverButton();
                SoundManager.Instance.PlayButtonSound();
            }
        } else if (hoveredButton != -1) {
            hoveredButton = -1;
            HoverButton();
        }
    }
}