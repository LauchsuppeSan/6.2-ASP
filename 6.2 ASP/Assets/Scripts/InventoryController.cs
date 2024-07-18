using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [HideInInspector]
    private ItemGrid selectedItemGrid; //public
    private bool dropBoxFilled = false;

    InventoryItem selectedItem;
    InventoryItem overlapItem;
    InventoryItem itemToHighlight;
    RectTransform rectTransform;

    [SerializeField] List<ItemData> items;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform canvasTransform;
    [SerializeField] ItemGrid dropBox;

    InventoryHighlight inventoryHighlight;

    Vector2Int oldPosition;

    public ItemGrid SelectedItemGrid
    {
        get => selectedItemGrid;
        set
        {
            selectedItemGrid = value;
            inventoryHighlight.SetParent(value);
        }
    }

    private void Awake()
    {
        inventoryHighlight = GetComponent<InventoryHighlight>();
    }
   
    private void Update()
    {
        //Debug.Log(selectedItemGrid.GetTileGridPosition(Input.mousePosition));

        ItemIconDrag();

        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    CreateRandomItem();
        //}
        Debug.Log(dropBoxFilled);

        if (!dropBoxFilled)
        {
            InsertRandomItem();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateItem();
        }

        if (selectedItemGrid == null)
        {
            inventoryHighlight.Show(false);
            return;
        }

        HandleHighlight();

        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonPress();
        }
    }

    private void RotateItem()
    {
        if (selectedItem == null) return;

        selectedItem.Rotate();
    }

    private void InsertRandomItem()
    {
        selectedItemGrid = dropBox;
        //if (selectedItemGrid == null) return;

        for (int i = 0; i < 25; i++)
        {
            CreateRandomItem();
            InventoryItem itemToInsert = selectedItem;
            selectedItem = null;
            InsertItem(itemToInsert);
        }
        dropBoxFilled = true;
    }

    private void InsertItem(InventoryItem itemToInsert)
    {
        Vector2Int? posOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert);

        if (posOnGrid == null) return;

        selectedItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }

    /// <summary>
    /// Handles highlight
    /// </summary>
    private void HandleHighlight()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();

        if (oldPosition == positionOnGrid) return;

        oldPosition = positionOnGrid;

        if (selectedItem == null)
        {
            itemToHighlight = selectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);

            if (itemToHighlight != null)
            {
                inventoryHighlight.Show(true);
                inventoryHighlight.SetSize(itemToHighlight);
                //inventoryHighlight.SetParent(selectedItemGrid);
                inventoryHighlight.SetPosition(selectedItemGrid, itemToHighlight);
            }
            else
            {
                inventoryHighlight.Show(false);
            }
        }
        else
        {
            inventoryHighlight.Show(selectedItemGrid.BoundaryCheck(positionOnGrid.x, positionOnGrid.y, selectedItem.WIDTH, selectedItem.HEIGHT));
            inventoryHighlight.SetSize(selectedItem);
            //inventoryHighlight.SetParent(selectedItemGrid);
            inventoryHighlight.SetPosition(selectedItemGrid, selectedItem, positionOnGrid.x, positionOnGrid.y);
        }
    }

    /// <summary>
    /// Creates a random item
    /// </summary>
    private void CreateRandomItem()
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        selectedItem = inventoryItem; //newly created item being set as currently held

        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);
        rectTransform.SetAsLastSibling();

        int selectedItemID = UnityEngine.Random.Range(0, items.Count); //selects random item out of item list
        inventoryItem.Set(items[selectedItemID]);
    }

    /// <summary>
    /// What happens when the left mouse button is pressed on the grid
    /// </summary>
    private void LeftMouseButtonPress()
    {
        //Debug.Log(selectedItemGrid.GetTileGridPosition(Input.mousePosition));
        Vector2Int tileGridPosition = GetTileGridPosition();

        if (selectedItem == null)
        {
            PickUpItem(tileGridPosition);
        }
        else
        {
            PlaceItem(tileGridPosition);
        }
    }

    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Input.mousePosition;

        if (selectedItem != null)
        {
            position.x -= (selectedItem.WIDTH - 1) * ItemGrid.tileSizeWidth / 2;
            position.y += (selectedItem.HEIGHT - 1) * ItemGrid.tileSizeHeight / 2;
        }

        return selectedItemGrid.GetTileGridPosition(position);
    }

    /// <summary>
    /// checks for an overlapitem, assignes it and nullyfies it
    /// </summary>
    /// <param name="tileGridPosition"></param>
    private void PlaceItem(Vector2Int tileGridPosition)
    {
        bool complete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem); //checks if item can be placed

        //Debug.Log("complete " + complete);

        if (!complete)
        {
            //Debug.Log("youre a tad closer");
        }
        if (complete) //if item can be placed
        {
            selectedItem = null; //item currently held is null

            if (overlapItem != null) //if there is an overlapitem
            {
                selectedItem = overlapItem; //makes the overlapitem currently held item
                overlapItem = null; //overlapitem is null
                rectTransform = selectedItem.GetComponent<RectTransform>();
                rectTransform.SetAsLastSibling();
            }
        }

    }

    private void PickUpItem(Vector2Int tileGridPosition)
    {
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        if (selectedItem != null)
        {
            rectTransform = selectedItem.GetComponent<RectTransform>();
            rectTransform.SetParent(canvasTransform);
        }
    }

    /// <summary>
    /// Drags the icon/sprite of the currently held item
    /// </summary>
    private void ItemIconDrag()
    {
        if (selectedItem != null)
        {
            rectTransform.position = Input.mousePosition;
        }
    }
}
