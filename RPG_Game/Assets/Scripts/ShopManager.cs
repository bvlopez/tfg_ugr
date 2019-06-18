using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{

    private List<Item> purchase;
    private int price;
    private GameObject priceView;
    private GameObject descriptionView;
    private GameObject activeButton;
    [SerializeField]
    private GameObject weaponButtonPrefab;
    [SerializeField]
    private GameObject accessoryButtonPrefab;
    [SerializeField]
    private GameObject potionButtonPrefab;
    [SerializeField]
    private GameObject recoveryStoneButtonPrefab;
    private List<GameObject> itemsPrefabs;
    private Player player;
    private GameManager gameManager;
    private List<Item> items;
        
    public void addItem(Item item) {
        purchase.Add(item);
        price += item.getPrice();
        Text priceText = priceView.GetComponent<Text>();
        priceText.text = string.Concat(price.ToString(), " G");
    }

    public void removeItem(Item item) {
        purchase.Remove(item);
        price -= item.getPrice();
        Text priceText = priceView.GetComponent<Text>();
        priceText.text = string.Concat(price.ToString(), " G");
    }

    public void showDescription(GameObject button, Item item) { 
        if(activeButton != null) {            
            activeButton.GetComponent<ItemButton>().deselectItem();
        }
        activeButton = button;
        Text descriptionText = descriptionView.GetComponent<Text>();
        descriptionText.text = item.getDescription();    
    }

    public void resetView() {
        GameObject.Find("Money").GetComponent<Text>().text =  string.Concat(player.getMoney().ToString(), " G");
        Text priceText = priceView.GetComponent<Text>();
        priceText.text = string.Concat(price.ToString(), " G");
    }

    public void buyItems() {
        if(player.getMoney() >= price) {
            player.buyItems(purchase);
            purchase.Clear();
            price = 0;
            gameManager.updatePlayer();
            // Espera a que los datos del jugador esten actualizados
            StartCoroutine(gameManager.waitPlayerUpdate("", startReset)); 
            resetView();
        }        
    }

    public void startReset(string value) {
        resetView();
    }

    public void closeShop() {
        SceneManager.LoadScene("WorldScene");
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        player = gameManager.getPlayer();
        items = gameManager.getCurrentShopItems();

        GameObject scrollView = GameObject.Find("Content");
        purchase = new List<Item>();
        itemsPrefabs = new List<GameObject>();

        for(int i = 0; i < items.Count; i++) {
            if(items[i].getItemType() == ItemType.Weapon) {
                itemsPrefabs.Add((GameObject)Instantiate(weaponButtonPrefab, new Vector3(0, 358 - i*84, 0), Quaternion.identity));
            }
            else if(items[i].getItemType() == ItemType.Potion) {
                itemsPrefabs.Add((GameObject)Instantiate(potionButtonPrefab, new Vector3(0, 358 - i*84, 0), Quaternion.identity));
            }
            else if(items[i].getItemType() == ItemType.RecoveryStone) {
                itemsPrefabs.Add((GameObject)Instantiate(recoveryStoneButtonPrefab, new Vector3(0, 358 - i*84, 0), Quaternion.identity));
            }
            else {
                itemsPrefabs.Add((GameObject)Instantiate(accessoryButtonPrefab, new Vector3(0, 358 - i*84, 0), Quaternion.identity));
            }
            itemsPrefabs[i].transform.SetParent(scrollView.transform, false);
            itemsPrefabs[i].GetComponent<ItemButton>().setItem(items[i]);
        }    

        price = 0;
        priceView = GameObject.Find("PurchaseMoney");
        descriptionView = GameObject.Find("ShopDataDialog");
        resetView();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
