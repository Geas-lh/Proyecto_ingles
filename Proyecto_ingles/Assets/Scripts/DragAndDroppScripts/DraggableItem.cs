using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string itemID;
    public Image iconImage;
    public TextMeshProUGUI labelText;

    private Transform originalParent;
    private CanvasGroup canvasGroup;

    public void Setup(string id, Sprite icon, string label)
    {
        itemID = id;
        if (iconImage != null) iconImage.sprite = icon;
        if (labelText != null) labelText.text = label;
    }

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root); // mover arriba para que no quede oculto
        if (canvasGroup != null) canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent);
        if (canvasGroup != null) canvasGroup.blocksRaycasts = true;
    }

    public void ReturnToOrigin()
    {
        transform.SetParent(originalParent);
        transform.localPosition = Vector3.zero;
    }
}
