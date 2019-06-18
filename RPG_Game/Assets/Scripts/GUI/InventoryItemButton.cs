using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItemButton : MonoBehaviour, IPointerDownHandler
{
    private Item item; 
    private int amount;
    private bool selected;
    private GameObject panel;
    private MenuManager menuManager;
    
    public void setItem(Item value) {
        item = value;
        amount++;
        setView();
    }
        
    public void setView() {
        Text itemName = transform.Find("ItemName").GetComponent<Text>();     
        itemName.text = item.getName();       
        Text itemAmount = transform.Find("ItemAmount").GetComponent<Text>();     
        itemAmount.text = "x 1";        
    }

    public int getAmount() {
        return amount;
    }

    public void removeItem() {
        if((amount - 1) >= 0) {
            amount--;
            setView();
        }
    }

    public void selectItem() {
        if(!selected) {                 
            GetComponent<Image>().color = Color.grey;
            menuManager.showDescription(gameObject, item);
            selected = true;
        }
    }

    public void deselectItem() {
        if(selected) {          
            GetComponent<Image>().color = new Color(194, 194, 194, 255);
            selected = false;
        }
    } 
   
    public void OnPointerDown (PointerEventData eventData) {
        selectItem();
    }

    // Start is called before the first frame update
    void Start()
    {   
        panel = GameObject.Find("Panel");
        menuManager = panel.GetComponent<MenuManager>();
        selected = false;
        GetComponent<Image>().color = new Color(194, 194, 194, 255);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
