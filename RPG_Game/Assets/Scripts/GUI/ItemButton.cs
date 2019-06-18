using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemButton : MonoBehaviour, IPointerDownHandler
{
    private Item item; 
    private int amount;
    private int price;
    private bool selected;
    
    public void setItem(Item value) {
        item = value;
        setView();
    }

    public void setView() {
        Text itemName = transform.Find("ItemName").GetComponent<Text>();
        Text itemPrice = transform.Find("ItemPrice").GetComponent<Text>(); 

        itemName.text = item.getName();
        itemPrice.text =  string.Concat(item.getPrice().ToString(), " G");       
    }

    public void selectItem() {
        if(!selected) {
            GameObject panel = GameObject.Find("Panel");
            ShopManager shopManager = panel.GetComponent<ShopManager>();           
            GetComponent<Image>().color = Color.grey;
            shopManager.showDescription(gameObject, item);
            selected = true;
        }
    }

    public void deselectItem() {
        if(selected) {
            GameObject panel = GameObject.Find("Panel");
            ShopManager shopManager = panel.GetComponent<ShopManager>();
            GetComponent<Image>().color = new Color(194, 194, 194, 255);
            selected = false;
        }
    }

    public void addItem() {
        if(amount <= 100) {
            amount++;
            GameObject panel = GameObject.Find("Panel");
            ShopManager shopManager = panel.GetComponent<ShopManager>();
            shopManager.addItem(item);
            Text itemAmount = transform.Find("ItemAmount").GetComponent<Text>();  
            itemAmount.text = string.Concat("x ", amount.ToString());
        }
    }
    
    public void removeItem() {
        if(amount >= 1) {
            amount--;
            GameObject panel = GameObject.Find("Panel");
            ShopManager shopManager = panel.GetComponent<ShopManager>();
            shopManager.removeItem(item);
            Text itemAmount = transform.Find("ItemAmount").GetComponent<Text>();  
            itemAmount.text = string.Concat("x ", amount.ToString());
        }
    }

    public void OnPointerDown (PointerEventData eventData) {
        selectItem();
    }

    // Start is called before the first frame update
    void Start()
    {
        amount = 0;
        selected = false;
        GetComponent<Image>().color = new Color(194, 194, 194, 255);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
