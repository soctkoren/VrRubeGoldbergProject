using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMenu : MonoBehaviour {

    public GameObject teleporter;
    public GameObject metal;
    public GameObject trampoline;
    public GameObject wood;
    public GameObject displayTeleporter;
    public GameObject displayMetal;
    public GameObject displayTrampoline;
    public GameObject displayWood;
    public List<GameObject> menuItems;
    public GameObject selectedItemText;
    public GameObject display;
    public int trampolineLimit;
    public int woodLimit;
    public int metalLimit;
    public int teleporterLimit;
    private int trampolineSpanwed;
    private int woodSpawned;
    private int metalSpanwed;
    private int teleporterSpanwed;
    private GameObject selectedItem;
    private bool finishedMoving;
    private int currentIndex;


    public OVRInput.Controller rightHand;
 
    void Start () {
        currentIndex = 0;
        menuItems = new List<GameObject>();
        menuItems.Add(teleporter);
        menuItems.Add(metal);
        menuItems.Add(trampoline);
        menuItems.Add(wood);
        selectedItem = menuItems[currentIndex];
        setSelectedItemText(selectedItem);
        finishedMoving = true;
        trampolineSpanwed = 0;
        woodSpawned = 0;
        metalSpanwed = 0;
        teleporterSpanwed = 0;
}

    void Update () {
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) >= 0.5f)
        {
            display.SetActive(true);
            if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).y >= 0.5f && finishedMoving)
            {
                if (currentIndex == 3)
                {
                    currentIndex = 0;
                    selectedItem = menuItems[currentIndex];
                    setSelectedItemText(selectedItem);
                    finishedMoving = false;
                }
                else
                {
                    currentIndex += 1;
                    selectedItem = menuItems[currentIndex];
                    setSelectedItemText(selectedItem);
                    finishedMoving = false;
                }
            }
            else if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).y <= -0.5f && finishedMoving)
            {
                if (currentIndex == 0)
                {
                    currentIndex = 3;
                    selectedItem = menuItems[currentIndex];
                    setSelectedItemText(selectedItem);
                    finishedMoving = false;
                }
                else
                {
                    currentIndex -= 1;
                    selectedItem = menuItems[currentIndex];
                    setSelectedItemText(selectedItem);
                    finishedMoving = false;
                }
            }
            else if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).y == 0.0f && finishedMoving == false)
            {
                finishedMoving = true;
            }
            // SPAWN ITEMS
            else if (OVRInput.GetUp(OVRInput.Button.Two, OVRInput.Controller.RTouch))
            {
                Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.5f);
                if (isValidToSpawn(selectedItem)) {
                    GameObject newItem = (GameObject)Instantiate(selectedItem, newPosition, transform.rotation);
                }
            }

        }
        else {
            display.SetActive(false);
        }
    }

    bool isValidToSpawn(GameObject selectedItem)
    {
        if (selectedItem.transform.name == "Fan_Body")
        {
            if (teleporterSpanwed < teleporterLimit)
            {
                teleporterSpanwed += 1;
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (selectedItem.transform.name == "Metal_Plank_WE")
        {
            if (metalSpanwed < metalLimit)
            {
                metalSpanwed += 1;
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (selectedItem.transform.name == "Trampoline")
        {
            if (trampolineSpanwed < trampolineLimit)
            {
                trampolineSpanwed += 1;
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (selectedItem.transform.name == "Wood_Plank")
        {
            if (woodSpawned < woodLimit)
            {
                woodSpawned += 1;
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    void setSelectedItemText(GameObject selectedItem) 
    {
        TextMesh text = selectedItemText.GetComponent<TextMesh>();
        Debug.Log(selectedItem.transform.name);
        if (selectedItem.transform.name == "Fan_Body")
        {
            text.text = "Teleporter: throw ball into \n fan and the ball \n will teleport to x mark";
            displayTeleporter.SetActive(true);
            displayMetal.SetActive(false);
            displayTrampoline.SetActive(false);
            displayWood.SetActive(false);
        }
        else if (selectedItem.transform.name == "Metal_Plank_WE")
        {
            text.text = "Rails: The ball will roll \n bown the rails";
            displayTeleporter.SetActive(false);
            displayMetal.SetActive(true);
            displayTrampoline.SetActive(false);
            displayWood.SetActive(false);
        }
        else if (selectedItem.transform.name == "Trampoline")
        {
            text.text = "Trampoline: Will bounch  \n the ball";
            displayTeleporter.SetActive(false);
            displayMetal.SetActive(false);
            displayTrampoline.SetActive(true);
            displayWood.SetActive(false);
        }
        else if (selectedItem.transform.name == "Wood_Plank")
        {
            text.text = "Wood: used to block  \n the ball";
            displayTeleporter.SetActive(false);
            displayMetal.SetActive(false);
            displayTrampoline.SetActive(false);
            displayWood.SetActive(true);
        }
    }
}
