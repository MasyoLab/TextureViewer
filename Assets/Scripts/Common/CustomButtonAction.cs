using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CustomButtonAction : Button {

    public override void OnPointerClick(PointerEventData eventData) {
        base.OnPointerClick(eventData);
    }

    public override void OnPointerDown(PointerEventData eventData) {
        base.OnPointerDown(eventData);
        transform.DOScale(0.95f, 0.24f).SetEase(Ease.OutCubic);
    }

    public override void OnPointerUp(PointerEventData eventData) {
        base.OnPointerUp(eventData);
        transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);
    }
}
