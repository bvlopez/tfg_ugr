using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleItemManager : MonoBehaviour
{
    
    private Item selectedItem;
    private GameObject selectedItemButton;
    private List<GameObject> itemsPrefabs;
    private GameObject descriptionView;    
    [SerializeField]
    private GameObject potionButtonPrefab;
    [SerializeField]
    private GameObject playerGameObject;    
    private Player player;
    GameObject scrollView;

    // Start is called before the first frame update
    void Start()
    {
        player = playerGameObject.GetComponent<CharacterController>().getPlayer();
        descriptionView = GameObject.Find("ItemDataDialog");
        scrollView = GameObject.Find("ContentItems");
        itemsPrefabs = new List<GameObject>();
        setItemsView();
    }

    public void selectItem(GameObject button, Item item) {
        if(selectedItemButton != null) {
            selectedItemButton.GetComponent<BattleItemButton>().deselectItem();
        }
        selectedItem = item;
        selectedItemButton = button;
    }

    public void showDescription(GameObject button, Item item) {
        selectItem(button, item);
        Text descriptionText = descriptionView.GetComponent<Text>();
        descriptionText.text = item.getDescription();    
    }

    // Borra los prefabs de los objetos
    public void resetItemsView() {
        for(int i = 0; i < itemsPrefabs.Count; i++) {
            itemsPrefabs[i].SetActive(false);
            Destroy(itemsPrefabs[i]);
        }
        itemsPrefabs.Clear();
        resetDescription();
    }

    // Borra la descripcion
    public void resetDescription() {
        Text descriptionText = descriptionView.GetComponent<Text>();
        descriptionText.text = "";  
    }

    public void setItemsView() {
        // Falta hacer que se separe por tipos
        List<Item> inventory = player.getInventory();
        int j = 0;        
        for(int i = 0; i < inventory.Count; i++) {              
            if(!inventory[i].canRecover() && inventory[i].canUse()) {
                itemsPrefabs.Add((GameObject)Instantiate(potionButtonPrefab, new Vector3(0, 358 - i*84, 0), Quaternion.identity));
                itemsPrefabs[j].transform.SetParent(scrollView.transform, false);
                itemsPrefabs[j].GetComponent<BattleItemButton>().setItem(inventory[i]);
                j++;
            }                                      
        }         
    }

    public Item getSelectedItem() {
        return selectedItem;
    }

    public void resetItemList() {
        resetItemsView();
        resetDescription();
        setItemsView();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
