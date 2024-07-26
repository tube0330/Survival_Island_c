using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DataInfo;

public class InventoryDrop : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
            InventoryDrag.draggingItem.transform.SetParent(transform, false);

        Item item = InventoryDrag.draggingItem.GetComponent<itemInfo>().C_item;

        GameManager.G_Instance.AddItem(item);
    }
}
