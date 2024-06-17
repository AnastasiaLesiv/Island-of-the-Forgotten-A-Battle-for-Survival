using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotInventory : MonoBehaviour
{

    public Transform quickslotParent;
    public InventoryManager inventoryManager;
    public int currentQuickslotID = 0;
    public Sprite selectedSprite;
    public Sprite notSelectedSprite;
    public TMP_Text healthText;

    // Update is called once per frame
    void Update()
    {
        float mw = Input.GetAxis("Mouse ScrollWheel");

        if (mw > 0.1)
        {
            quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = notSelectedSprite;
            
            if (currentQuickslotID >= quickslotParent.childCount-1)
            {
                currentQuickslotID = 0;
            }
            else
            {
                currentQuickslotID++;
            }
            quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = selectedSprite;


        }
        if (mw < -0.1)
        {
            
            quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = notSelectedSprite;
            
            if (currentQuickslotID <= 0)
            {
                currentQuickslotID = quickslotParent.childCount-1;
            }
            else
            {
                
                currentQuickslotID--;
            }

            quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = selectedSprite;
      
            
        }

        for(int i = 0; i < quickslotParent.childCount; i++)
        {
           
            if (Input.GetKeyDown((i + 1).ToString())) {
              
                if (currentQuickslotID == i)
                {
                   
                    if (quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite == notSelectedSprite)
                    {
                        quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = selectedSprite;
                    }
                    else
                    {
                        quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = notSelectedSprite;
                    }
                }
                else
                {
                    quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = notSelectedSprite;
                    currentQuickslotID = i;
                    quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = selectedSprite;
                }
            }
        }
       
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().item != null)
            {
                if (quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().item.isConsumeable && !inventoryManager.isOpened && quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite == selectedSprite)
                {
                    ChangeCharacteristics();

                    if (quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().amount <= 1)
                    {
                        quickslotParent.GetChild(currentQuickslotID).GetComponentInChildren<DragAndDropItem>().NullifySlotData();
                    }
                    else
                    {
                        quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().amount--;
                        quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().itemAmountText.text = quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().amount.ToString();
                    }
                }
            }
        }
    }

    private void ChangeCharacteristics()
    {
        if(int.Parse(healthText.text) + quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().item.changeHealth <= 100)
        {
            float newHealth = int.Parse(healthText.text) + quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().item.changeHealth;
            healthText.text = newHealth.ToString();
        }
       else
        {
            healthText.text = "100";
        }
    }
}
