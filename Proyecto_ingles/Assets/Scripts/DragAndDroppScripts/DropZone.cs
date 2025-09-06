using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DropZone : MonoBehaviour, IDropHandler
{
    public string expectedItemID;
    public TextMeshProUGUI targetNameText;

    public void SetExpected(string itemID)
    {
        expectedItemID = itemID;
        if (targetNameText != null)
            targetNameText.text = itemID;
    }

    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem di = eventData.pointerDrag.GetComponent<DraggableItem>();
        if (di != null)
        {
            if (di.itemID == expectedItemID)
            {
                // ✅ Correcto
                GameManager.Instance.OnCorrectPlacement(di.itemID, di.gameObject);
            }
            else
            {
                // ❌ Incorrecto
                di.ReturnToOrigin();
                GameManager.Instance.RegisterFail();
            }
        }
    }
}
