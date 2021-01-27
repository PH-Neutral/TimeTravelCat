using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Utils {

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

    #region RectTransform

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