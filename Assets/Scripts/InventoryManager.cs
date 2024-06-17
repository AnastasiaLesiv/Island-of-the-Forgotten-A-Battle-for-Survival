using System.Collections;
using System.Collections.Generic;
using UnityEditor.TestTools.CodeCoverage;
using UnityEngine;
using UnityEngine.Serialization;

public class InventoryManager : MonoBehaviour
{
    [FormerlySerializedAs("UIPanel")] public GameObject UIBG;
    public Transform inventoryPanel;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public bool isOpened;
    public float reachDistance = 3f;
    private Camera mainCamera;

    public MonoBehaviour playerScript;
    // Start is called before the first frame update
    private void Awake()
    {
        UIBG.SetActive(true);
    }
    void Start()
    {
        mainCamera = Camera.main;

        // Перевірка і ініціалізація слотів
        for (int i = 0; i < inventoryPanel.childCount; i++)
        {
            var slot = inventoryPanel.GetChild(i).GetComponent<InventorySlot>();
            if (slot != null)
            {
                
                slots.Add(slot);
            }
        }

        UIBG.SetActive(false);
        inventoryPanel.gameObject.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isOpened = !isOpened;
            if (isOpened)
            {
                if (playerScript != null)
                {
                    playerScript.enabled = false;
                }
                UIBG.SetActive(true);
                inventoryPanel.gameObject.SetActive(true);
            }
            else
            {
                if (playerScript != null)
                {
                    playerScript.enabled = true;
                }
                UIBG.SetActive(false);
                inventoryPanel.gameObject.SetActive(false);
            }
        }
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(Physics.Raycast(ray, out hit, reachDistance))
            {
                if (hit.collider.gameObject.GetComponent<Item>() != null)
                {
                    AddItem(hit.collider.gameObject.GetComponent<Item>().item,
                    hit.collider.gameObject.GetComponent<Item>().amount);
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }
    private void AddItem(ItemScriptableObject _item, int _amount)
    {

        foreach (InventorySlot slot in slots)
        {
            
            if (slot.item == _item && !slot.isEmpty)
            {
                if ((slot.amount + _amount) <= _item.maximumAmount)
                {
                    slot.amount += _amount;
                    slot.itemAmountText.text = slot.amount.ToString();
                    return;
                }

                break;
            }
        }

        foreach (InventorySlot slot in slots)
        {
            if (slot.isEmpty)
            {
                slot.item = _item;
                slot.amount = _amount;
                slot.isEmpty = false;
                slot.SetIcon(_item.icon);
                slot.itemAmountText.text = _amount.ToString();
                break;
            }
        }
    }

}
