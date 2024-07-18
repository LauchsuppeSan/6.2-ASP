using System;
using UnityEngine;
using TMPro;

public class ItemGrid : MonoBehaviour
{
    public const float tileSizeWidth = 32;
    public const float tileSizeHeight = 32;
    private bool availableSpace/* = true*/;
    private bool allClear/* = true*/;

    InventoryItem[,] inventoryItemSlot;

    RectTransform rectTransform;

    [SerializeField] int gridSizeWidth;
    [SerializeField] int gridSizeHeight;
    [SerializeField] TMP_InputField iFWidth;

    //[SerializeField] GameObject inventoryItemPrefab;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(gridSizeWidth, gridSizeHeight);

        //iFWidth = GetComponent<TMP_InputField>();

        //InventoryItem inventoryItem = Instantiate(inventoryItemPrefab).GetComponent<InventoryItem>();
        //PlaceItem(inventoryItem, 3, 2);
    }
    private void Update()
    {
        Debug.Log("available spac3 " + availableSpace);
        Debug.Log("allClear " + allClear);
    }

    public void ChangeGridWidth(string input)
    {
        if (Convert.ToInt32(input) < 2 || Convert.ToInt32(input) > 30)
        {
            //textfield
        }
        if (Convert.ToInt32(input) < gridSizeWidth && Convert.ToInt32(input) >= 2)
        {
            Debug.Log("ello");

            //if (inventoryItemSlot == null || IsNullOrEmpty(inventoryItemSlot)
            //{
            //    gridSizeWidth = Convert.ToInt32(input);
            //    InitChanged(gridSizeWidth, gridSizeHeight);
            //}
            //else if (inventoryItemSlot != null)
            //{
            //    Debug.Log("Awe man!");
            //}
            //availableSpace = true;
            for (int x = 0; x < gridSizeWidth; x++)
            {
                for (int y = 0; y < gridSizeHeight; y++)
                {
                    if (inventoryItemSlot[x, y] != null)
                    {
                        Debug.Log("Check available space FALSE");
                        iFWidth.text = Convert.ToString(gridSizeWidth);
                        availableSpace = false;
                        return;
                    }
                    //else
                    //{
                    //    availableSpace = true;
                    //}
                }
            }
            allClear = true;
            if (!availableSpace && allClear)
            {
                //availableSpace = true;
                gridSizeWidth = Convert.ToInt32(input);
                InitChanged(gridSizeWidth, gridSizeHeight);
            }
        }
        if (Convert.ToInt32(input) <= 30 && Convert.ToInt32(input) >= gridSizeWidth)
        {
            gridSizeWidth = Convert.ToInt32(input);
            InitChanged(gridSizeWidth, gridSizeHeight);
        }
    }
    public void ChangeGridHeight(string input)
    {
        if (Convert.ToInt32(input) <= 17 && Convert.ToInt32(input) >= 2)
        {
            gridSizeHeight = Convert.ToInt32(input);
            InitChanged(gridSizeWidth, gridSizeHeight);
        }
    }

    /// <summary>
    /// Initiating Inventory Grid
    /// </summary>
    /// <param name="width">width of grid</param>
    /// <param name="height">height of grid</param>
    private void Init(int width, int height)
    {
        inventoryItemSlot = new InventoryItem[width, height];
        Vector2 size = new Vector2(width * tileSizeWidth, height * tileSizeHeight);
        rectTransform.sizeDelta = size;
    }

    /// <summary>
    /// Initating changed Grid after user input
    /// </summary>
    /// <param name="width">new grid width</param>
    /// <param name="height">new grid height</param>
    private void InitChanged(int width, int height)
    {
        //InventoryItem[,] tempItems = inventoryItemSlot;

        //if (tempItems.GetLength(1) > height || tempItems.GetLength(0) > width)
        //{
        //    Debug.Log("FUCK YEAH!");
        //    //return;
        //}

        inventoryItemSlot = new InventoryItem[width, height];
        //Array.Copy(tempItems, 0, inventoryItemSlot, 0, tempItems.Length);

        Vector2 size = new Vector2(width * tileSizeWidth, height * tileSizeHeight);
        rectTransform.sizeDelta = size;
    }

    Vector2 positionOnTheGrid = new Vector2();
    Vector2Int tileGridPosition = new Vector2Int();

    /// <summary>
    /// Mouse position
    /// </summary>
    /// <param name="mousePosition"></param>
    /// <returns></returns>
    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = rectTransform.position.y - mousePosition.y;

        tileGridPosition.x = (int)(positionOnTheGrid.x / tileSizeWidth);
        tileGridPosition.y = (int)(positionOnTheGrid.y / tileSizeHeight);

        return tileGridPosition;
    }

    public bool PlaceItem(InventoryItem inventoryItem, int posX, int posY, ref InventoryItem overlapItem)
    {
        if (BoundaryCheck(posX, posY, inventoryItem.WIDTH, inventoryItem.HEIGHT) == false) //Boundary check is true
        {
            //Debug.Log("Place item FALSE");
            return false;
        }
        // if (BoundaryCheck(posX, posY, inventoryItem.WIDTH, inventoryItem.HEIGHT) == true)
        //{
        //    Debug.Log("Boundary check TRUE");

        //}

        if (OverlapCheck(posX, posY, inventoryItem.WIDTH, inventoryItem.HEIGHT, ref overlapItem) == false)
        {
            //Debug.Log("place item FALSE");
            overlapItem = null;

            return false;
        }
        if (OverlapCheck(posX, posY, inventoryItem.WIDTH, inventoryItem.HEIGHT, ref overlapItem) == true)
        {
            //Debug.Log("Overlap check TRUE");

        }

        if (overlapItem != null)
        {
            CleanGridReference(overlapItem);
        }

        PlaceItem(inventoryItem, posX, posY);

        //Debug.Log("Place Item TRUE");
        return true;
    }

    /// <summary>
    /// Places Item
    /// </summary>
    /// <param name="inventoryItem">currently held item</param>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    public void PlaceItem(InventoryItem inventoryItem, int posX, int posY)
    {
        RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);

        for (int x = 0; x < inventoryItem.WIDTH; x++) //Cycling through width and height of item to make sure enough tiles are obtained for the item
        {
            for (int y = 0; y < inventoryItem.HEIGHT; y++)
            {
                inventoryItemSlot[posX + x, posY + y] = inventoryItem;
            }
        }

        inventoryItem.onGridPositionX = posX; //OnGridPosition being assigned
        inventoryItem.onGridPositionY = posY;
        Vector2 position = CalculatePositionOnGrid(inventoryItem, posX, posY);

        rectTransform.localPosition = position;
    }

    /// <summary>
    /// Calculates correct position on grid
    /// </summary>
    /// <param name="inventoryItem">Item wanting to be placed</param>
    /// <param name="posX">position on grid</param>
    /// <param name="posY">position on grid</param>
    /// <returns></returns>
    public Vector2 CalculatePositionOnGrid(InventoryItem inventoryItem, int posX, int posY)
    {
        Vector2 position = new Vector2();
        position.x = posX * tileSizeWidth + tileSizeWidth * inventoryItem.WIDTH / 2;
        position.y = -(posY * tileSizeHeight + tileSizeHeight * inventoryItem.HEIGHT / 2);
        return position;
    }

    /// <summary>
    /// Checks if there is already an item in that slot, if yes it assignes it to overlapitem
    /// </summary>
    /// <param name="posX">x position we want to place our item in</param>
    /// <param name="posY">y position we want to place our item in</param>
    /// <param name="width">width of the item being held/param>
    /// <param name="height">height of the item being held</param>
    /// <param name="overlapItem">overlapping item will be assigned to this</param>
    /// <returns>if true we can place item, if false we are overlapping with 2 items</returns>
    private bool OverlapCheck(int posX, int posY, int width, int height, ref InventoryItem overlapItem)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (inventoryItemSlot[posX + x, posY + y] != null)
                {
                    //Debug.Log("it aint null");
                    if (overlapItem == null)
                    {
                        overlapItem = inventoryItemSlot[posX + x, posY + y]; //Overlapitem has been assigned to the item we are overlapping with
                        //Debug.Log("Overlap TRUE");
                    }
                    else if (overlapItem != inventoryItemSlot[posX + x, posY + y])
                    {
                        //Debug.Log("Overlap FALSE");

                        return false; //There is 2 items we are overlapping with so we cant place the item
                    }
                }
                else if (inventoryItemSlot[posX + x, posY + y] == null)
                {
                    //Debug.Log("It do be null");
                }
            }
        }
        //Debug.Log("Nothing to overlap with");

        return true;
    }

    /// <summary>
    /// Checks grid for available space
    /// </summary>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    private bool CheckAvailableSpace(int posX, int posY, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (inventoryItemSlot[posX + x, posY + y] != null)
                {
                    //Debug.Log("Check available space FALSE");
                    return false;
                }
            }
        }
        //Debug.Log("Check available space TRUE");

        return true;
    }

    /// <summary>
    /// Picks item back up from the grid
    /// </summary>
    /// <param name="x">x position of the item on the grid</param>
    /// <param name="y">y position of the item on the grid</param>
    /// <returns></returns>
    public InventoryItem PickUpItem(int x, int y)
    {
        InventoryItem toReturn = inventoryItemSlot[x, y];

        if (toReturn == null) { return null; } //Throws error because its trying to interact with data where there is none

        CleanGridReference(toReturn);

        return toReturn;
    }

    /// <summary>
    /// the slot of the picked up item is being cleared of that item
    /// </summary>
    /// <param name="item">Item that is being picked up</param>
    private void CleanGridReference(InventoryItem item)
    {
        for (int ix = 0; ix < item.WIDTH; ix++)
        {
            for (int iy = 0; iy < item.HEIGHT; iy++)
            {
                inventoryItemSlot[item.onGridPositionX + ix, item.onGridPositionY + iy] = null;
            }
        }
    }

    /// <summary>
    /// Checks if position x and y falls inside the boundaries of the grid
    /// </summary>
    /// <param name="posX">x position you want to place item in</param>
    /// <param name="posY">y position you want to place item in</param>
    /// <returns></returns>
    bool PositionCheck(int posX, int posY)
    {
        if (posX < 0 || posY < 0)
        {
            //Debug.Log("Position check FALSE");
            return false; //outside the boundary
        }

        if (posX >= gridSizeWidth || posY >= gridSizeHeight)
        {
            //Debug.Log("Position check FALSE");

            return false; //outside the boundary
        }

        //Debug.Log("Position check TRUE");

        return true;
    }

    /// <summary>
    /// Checks if whole item is inside the grid
    /// </summary>
    /// <param name="posX">x position on the grid</param>
    /// <param name="posY">y position on the grid</param>
    /// <param name="width">width of the item</param>
    /// <param name="height">height of the item</param>
    /// <returns></returns>
    public bool BoundaryCheck(int posX, int posY, int width, int height)
    {
        if (PositionCheck(posX, posY) == false)
        {
            //Debug.Log("Boundary check FALSE");
            return false;
        }

        posX += width - 1;
        posY += height - 1;

        if (PositionCheck(posX, posY) == false)
        {
            //Debug.Log("Boundary check FALSE");

            return false;
        }

        //Debug.Log("Boundary check TRUE");

        return true;
    }

    /// <summary>
    /// Gets item on grid
    /// </summary>
    /// <param name="x">x position of item</param>
    /// <param name="y">y position of item</param>
    /// <returns></returns>
    internal InventoryItem GetItem(int x, int y)
    {
        return inventoryItemSlot[x, y];
    }

    internal Vector2Int? FindSpaceForObject(InventoryItem itemToInsert)
    {
        int height = gridSizeHeight - itemToInsert.HEIGHT + 1;
        int width = gridSizeWidth - itemToInsert.WIDTH + 1;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (CheckAvailableSpace(x, y, itemToInsert.WIDTH, itemToInsert.HEIGHT) == true)
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return null;
    }
}
