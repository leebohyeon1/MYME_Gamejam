using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnMouseEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(transform.localScale * 1.3f, 0.25f).SetEase(Ease.InQuad);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(new Vector3(1,1,1),0.25f).SetEase(Ease.InQuad);
    }
}
