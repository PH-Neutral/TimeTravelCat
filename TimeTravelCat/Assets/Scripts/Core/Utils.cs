﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public static class Utils {
    static int fingerID = -1;

    public static bool HasBeenClickedOn(this Collider2D col, int layer) {
        #if !UNITY_EDITOR
            fingerID = 0; 
        #endif
        if(EventSystem.current.IsPointerOverGameObject(fingerID))    // is the touch on the GUI
        {
            // GUI Action
            return false;
        }
        int layers = (1 << layer) | (1 << LayerMask.NameToLayer("UI"));
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero, 15f, layers);
        for (int i=0; i<hits.Length; i++) {
            //Debug.Log("collided with " + hits[i].collider?.name);
            if (hits[i].collider != null && hits[i].collider == col) {
                return true;
            }
        }
        return false;
    }

    public static bool RaycastCollided(int layer) {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero, 15f, 1<<layer);
        for(int i = 0; i < hits.Length; i++) {
            //Debug.Log("collided with " + hits[i].collider?.name);
            if(hits[i].collider != null) {
                //Debug.Log("hits[i].collider: " + hits[i].collider.name);
                return true;
            }
        }
        return false;
    }

    #region Time

    public static IEnumerator WaitForUnscaledSeconds(float seconds) {
        float timer = seconds;
        while (timer > 0) {
            timer -= Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    #endregion

    #region Events

    public static bool HasBeenSet(this UnityEvent uEvent) {
        for (int i=0; i<uEvent.GetPersistentEventCount(); i++) {
            if (uEvent.GetPersistentTarget(i) != null) {
                return true;
            }
        }
        return false;
    }

    #endregion

    #region GUI

    public static void AdaptToParent(this Image img, RectTransform parent) {
        Vector2 slotSize = parent.rect.size;
        Vector2 spriteSize = img.sprite.bounds.extents;
        float aspect = spriteSize.x / spriteSize.y; // = width / height
        float offset;
        RectTransform rt = img.rectTransform;
        //Debug.Log("slotSize: " + slotSize.ToString() + "; spriteSize: " + spriteSize + "; aspect: " + aspect);
        //Debug.Log("BEFORE - rt: " + rt.offsetMin + ", " + rt.offsetMax);
        rt.SetStretchZero();
        //Debug.Log("STRETCH - rt: " + rt.offsetMin + ", " + rt.offsetMax);
        if(aspect < 1) {
            offset = slotSize.x * 0.5f * aspect;
            rt.SetRight(offset);
            rt.SetLeft(offset);
        } else if(aspect > 1) {
            offset = slotSize.y * 0.5f / aspect;
            rt.SetTop(offset);
            rt.SetBottom(offset);
        }
        //Debug.Log("AFTER - rt: " + rt.offsetMin + ", " + rt.offsetMax);
    }

    public static void SetLeft(this RectTransform rt, float left) {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(this RectTransform rt, float right) {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(this RectTransform rt, float top) {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(this RectTransform rt, float bottom) {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }

    public static void SetStretchZero(this RectTransform rt) {
        rt.offsetMin = new Vector2(0, 0);
        rt.offsetMax = new Vector2(0, 0);
    }

    #endregion
}