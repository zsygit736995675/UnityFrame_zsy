using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DoTween工具
/// </summary>
public class DoTweenUtils : MonoBehaviour {


    public static void DoHideAnim(GameObject obj, GameObject canClickBtn = null)
    {
        if (canClickBtn != null)
        {
            canClickBtn.SetActive(false);
        }
        obj.transform.DOScale(new Vector3(0.4f, 0.4f, 0.4f), 0.5f);
        obj.transform.DOLocalJump(new Vector3(-900f, 0f), 100, 4, 2f, false).OnComplete(() =>
        {
            obj.SetActive(false);
            obj.transform.localPosition = new Vector3(0f, 0f, 0f);
            obj.transform.localScale = new Vector3(1, 1, 1);
            if (canClickBtn != null)
            {
                canClickBtn.SetActive(true);
            }
        });
    }

    public static void DoEnterAnim(GameObject obj)
    {
        obj.transform.localPosition = new Vector3(900f, 0, 0);
        obj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        obj.SetActive(true);
        obj.transform.DOScale(new Vector3(1f, 1f, 1f), 1f);
        obj.transform.DOLocalJump(new Vector3(0f, 0f), 100, 4, 1.5f, false);
    }

    public static void DoEnterSaleAnim(GameObject go, float time = -1f)
    {
        go.SetActive(true);
        go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        if (time == -1) time = 0.4f;
        go.transform.DOScale(new Vector3(1, 1, 1), time);
    }

    public static void DoHideSaleAnim(GameObject go, float time = 0.4f)
    {
        go.transform.DOScale(new Vector3(0f, 0f, 0f), time);
    }

    public static void DoEnlargeSaleAnim(GameObject go, float offset, float duration)
    {
        go.transform.DOScale(new Vector3(offset, offset, offset), 0.4f);
    }

    public static void DoDownToUpAnim(GameObject go)
    {
        go.transform.localPosition = new Vector3(0, 1320f, 0);
        go.transform.DOLocalMoveY(0, 0.4f);
    }

    public static void DoUpToDownAnim(GameObject go)
    {
        go.transform.localPosition = new Vector3(0, -1320f, 0);
        go.transform.DOLocalMoveY(0, 0.4f);
    }

    public static void DoLeftUPtoPosAnim(GameObject go, Vector3 pos, float timer = -1f)
    {
        go.transform.localPosition = new Vector3(-1000f, 1500f, 0);
        if (timer == -1f) timer = 0.4f;
        go.transform.DOLocalMoveY(pos.y, timer);
        go.transform.DOLocalMoveX(pos.x, timer);
    }

    public static void DoDownHideAnim(GameObject go)
    {
        go.transform.localPosition = new Vector3(0, -1320f, 0);
    }


    public static void DoLeftToRightAnim(GameObject go)
    {
        go.transform.localPosition = new Vector3(-900f, 0, 0);
        go.transform.DOLocalMoveX(0, 0.4f);

    }

    public static void DoRotateAnim(GameObject go, float pos)
    {
        Vector3 v = new Vector3(0, 0, pos);
        go.transform.DOLocalRotate(v, 0.4f);
    }

    public static void DoRotateYAnim(GameObject go, float angle, float timer = -1)
    {
        if (timer == -1) timer = 0.4f;
        go.transform.DOLocalRotate(Vector3.up * angle, timer);
    }

    public static void DoDownToUpAnim(GameObject go, float endy)
    {
        go.transform.DOLocalMoveY(endy, 0.4f);

    }

    public static void DoToUpAnim(GameObject go, float endy, float time = 0.4f, float starty = 0)
    {
        go.SetActive(true);
        go.transform.localPosition = new Vector3(go.transform.localPosition.x, starty, go.transform.localPosition.z);
        go.transform.DOLocalMoveY(endy, time);
    }

    public static void DoMovePosAnim(GameObject go, float pos)
    {
        go.transform.DOLocalMoveX(pos, 0.4f);
    }

    public static void DoRightToLeftAnim(GameObject go)
    {

        go.transform.localPosition = new Vector3(900, 0, 0);
        go.SetActive(true);
        go.transform.DOLocalMoveX(0, 0.4f);
    }
    public static void DoFanzhuanAnim(GameObject go)
    {
        go.transform.DOFlip();
    }

}
