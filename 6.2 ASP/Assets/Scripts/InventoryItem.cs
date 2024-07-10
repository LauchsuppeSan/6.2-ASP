using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IPointerExitHandler
{
    public ItemData itemData;

    Image itemImage;

    InventoryHighlight highlight;

    private void Start()
    {
        itemImage = GetComponent<Image>();
    }

    public int HEIGHT
    {
        get
        {
            if (rotated == false)
            {
                return itemData.height;
            }
            return itemData.width;
        }
    }

    public int WIDTH
    {
        get
        {
            if (rotated == false)
            {
                return itemData.width;
            }
            return itemData.height;
        }
    }

    public int onGridPositionX;
    public int onGridPositionY;

    public bool rotated = false;

    private void Update()
    {

    }
    internal void Rotate()
    {
        rotated = !rotated;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0, 0, rotated == true ? 90f : 0f);
    }

    internal void Set(ItemData itemData)
    {
        this.itemData = itemData;

        GetComponent<Image>().sprite = itemData.itemIcon;

        Vector2 size = new Vector2();
        size.x = itemData.width * ItemGrid.tileSizeWidth;
        size.y = itemData.height * ItemGrid.tileSizeHeight;
        GetComponent<RectTransform>().sizeDelta = size;
    }

    public void SetRaycastTarget(bool rayOn)
    {
        if (rayOn)
        {
             itemImage.raycastTarget = true;
            Debug.Log("ello");
        }
        else if (!rayOn)
        {
            itemImage.raycastTarget = false;
        }
    }

    private void OnPointerExit()
    {
        highlight.SetColor(Color.white);
        Debug.Log("huh");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlight.SetColor(Color.red);
        Debug.Log("workkk");
    }
}
