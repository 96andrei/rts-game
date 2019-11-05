using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiController
{
    GameObject selectboxObject;
    GameObject canvas;
    RectTransform rectTransform;
    Vector2 anchor;
    UiControllerHelper controllerHelper;
    public UiControllerHelper ControllerHelper { get { return controllerHelper; } }

    public UiController()
    {
        controllerHelper = GameObject.FindObjectOfType<UiControllerHelper>();
        selectboxObject = controllerHelper.SelectBoxObject;
        rectTransform = controllerHelper.RectTransform;
        canvas = controllerHelper.Canvas;
        ClearBox();
    }

    public void OnMovement(Vector3 point)
    {
        if (controllerHelper == null)
            return;

        controllerHelper.MoveMarker.transform.position = point + Vector3.up * 0.1f;
        controllerHelper.MoveMarker.SetActive(true);
        controllerHelper.AttackMarker.SetActive(false);
    }

    public void OnGather(Vector3 point)
    {
        OnMovement(point);
    }

    public void OnAttack(Entity target)
    {
        if (controllerHelper == null)
            return;

        //controllerHelper.MoveMarker.transform.position = target.transform.position;// + Vector3.up * 0.1f;
        controllerHelper.MoveMarker.SetActive(false);
        controllerHelper.AttackMarker.transform.position = target.Halo.transform.position;
        controllerHelper.AttackMarker.SetActive(true);
    }

    public void StartSelectBox(Vector3 anchor)
    {
        if (controllerHelper != null)
            controllerHelper.MoveMarker.SetActive(false);
        selectboxObject.SetActive(true);

        this.anchor = ScreenToLocal(anchor);

        rectTransform.localPosition = new Vector3(this.anchor.x, this.anchor.y, rectTransform.localPosition.z);
    }

    public void DragSelectBox(Vector3 outer)
    {
        Vector2 rectPos = ScreenToLocal(outer);
        Vector2 newSize = new Vector2();
        Vector3 newPosition = new Vector3(anchor.x, anchor.y, rectTransform.localPosition.z);

        if (rectPos.x - anchor.x < 0)
        {
            // move location
            newSize.x = anchor.x - rectPos.x;
            newPosition.x = rectPos.x;
        }
        else
        {
            newSize.x = rectPos.x - anchor.x;
        }

        if (rectPos.y - anchor.y < 0)
        {
            // move location
            newSize.y = anchor.y - rectPos.y;
            newPosition.y = rectPos.y;
        }
        else
        {
            newSize.y = rectPos.y - anchor.y;
        }

        rectTransform.sizeDelta = newSize;
        rectTransform.localPosition = newPosition;
    }

    private Vector2 ScreenToLocal(Vector3 point)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, point, null, out pos);
        return pos;
    }

    public void ClearAll()
    {
        ClearBox();
    }

    public void ClearBox()
    {
        selectboxObject.SetActive(false);
        rectTransform.sizeDelta = new Vector2(0, 0);
    }
}