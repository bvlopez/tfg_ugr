using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    private Item selectedItem;
    private GameObject selectedItemButton;
    private GameObject equipItemButton;
    private GameObject useItemButton;
    private GameObject removeItemButton;
    private Player player;
    private List<GameObject> itemsPrefabs;
    private GameObject descriptionView;    
    [SerializeField]
    private GameObject weaponButtonPrefab;
    [SerializeField]
    private GameObject accessoryButtonPrefab;
    [SerializeField]
    private GameObject potionButtonPrefab;
    [SerializeField]
    private GameObject recoveryStoneButtonPrefab;
    [SerializeField]
    private List<Sprite> factionsIcons;
    [SerializeField]
    private Image factionIcon;
    GameObject scrollView;
    private GameObject inventoryButton;
    private GameObject questsButton;
    private GameObject skillsButton;
    private GameObject inventoryTab;
    private GameObject questsTab;
    private GameObject skillsTab;
    private GameManager gameManager;

    // Selecciona el objeto
    public void selectItem(GameObject button, Item item) {
        if(selectedItemButton != null) {
            selectedItemButton.GetComponent<InventoryItemButton>().deselectItem();
        }
        selectedItem = item;
        selectedItemButton = button;
        setButtonsView();
    }

    // Usa el objeto seleccionado
    public void useItem() {
        if(selectedItem != null) {
            if(selectedItem.canUse()) {
                Stat stat = selectedItem.getTargetStat(0);
                // Si puede recuperar y la vida esta a 0
                if(selectedItem.canRecover() && (player.getStat(Stat.CurrentHealth) <= 0)) {
                    player.useItem(selectedItem);
                    int nextAmount = selectedItemButton.GetComponent<InventoryItemButton>().getAmount();
                    selectedItemButton.GetComponent<InventoryItemButton>().removeItem();
                    if((nextAmount - 1) <= 0) {
                        selectedItem = null;
                        selectedItemButton.SetActive(false);
                    }
                    resetItemsView();
                    resetDescription();
                }
                // Comprueba que la vida o el mana no este al maximo                
                else if((stat == Stat.CurrentHealth) && (player.getStat(Stat.CurrentHealth) < player.getStat(Stat.MaxHealth))) {
                    // Comprueba que la vida no este a 0 y que no pueda recuperar
                    if((player.getStat(Stat.CurrentHealth) > 0) && !selectedItem.canRecover()) {
                        player.useItem(selectedItem);
                        int nextAmount = selectedItemButton.GetComponent<InventoryItemButton>().getAmount();
                        selectedItemButton.GetComponent<InventoryItemButton>().removeItem();
                        if((nextAmount - 1) <= 0) {
                            selectedItem = null;
                            selectedItemButton.SetActive(false);
                        }
                        resetItemsView();
                        resetDescription();
                    }
                }
                else if((stat == Stat.CurrentManaPoints) && (player.getStat(Stat.CurrentManaPoints) < player.getStat(Stat.ManaPoints))) {
                    player.useItem(selectedItem);
                    int nextAmount = selectedItemButton.GetComponent<InventoryItemButton>().getAmount();
                    selectedItemButton.GetComponent<InventoryItemButton>().removeItem();
                    if((nextAmount - 1) <= 0) {
                        selectedItem = null;
                        selectedItemButton.SetActive(false);
                    }
                    resetItemsView();
                    resetDescription();
                }
            }
        }
    }

    // Equipa el objeto seleccionado
    public void equipItem() {
        if(selectedItem != null) {
            if(selectedItem.canEquip()) {                
                player.equipItem(selectedItem);
                int nextAmount = selectedItemButton.GetComponent<InventoryItemButton>().getAmount();                
                selectedItemButton.GetComponent<InventoryItemButton>().removeItem();
                if((nextAmount - 1) <= 0) {
                    selectedItem = null;
                    selectedItemButton.SetActive(false);
                }
                resetItemsView();
                resetDescription();
            }
        }
    }

    // Elimina un objeto seleccionado
    public void removeItem() {
        if(selectedItem != null) {                          
            player.removeItem(selectedItem);
            int nextAmount = selectedItemButton.GetComponent<InventoryItemButton>().getAmount();                
            selectedItemButton.GetComponent<InventoryItemButton>().removeItem();
            if((nextAmount - 1) <= 0) {
                selectedItem = null;
                selectedItemButton.SetActive(false);
            }
            resetItemsView();
            resetDescription();       
        }
    }

    // Selecciona el objeto y muestra la descripcion
    public void showDescription(GameObject button, Item item) {
        selectItem(button, item);
        Text descriptionText = descriptionView.GetComponent<Text>();
        descriptionText.text = item.getDescription();    
    }

    // Actualiza la informacion en pantalla
    public void setPlayerData() {
        gameManager.updatePlayer();
        // Espera a que los datos del jugador esten actualizados
        StartCoroutine(gameManager.waitPlayerUpdate("", setData));          
    }

    public void setData(string value) {               
        GameObject.Find("PlayerNameText").GetComponent<Text>().text = string.Concat("Nombre: ", player.getName());
        GameObject.Find("PlayerLevelText").GetComponent<Text>().text = string.Concat("Nivel: ", player.getLevel().ToString());
        if(player.getJob() == Job.Warrior) {
            GameObject.Find("PlayerJobText").GetComponent<Text>().text = "Clase: Guerrero";
        }
        else if(player.getJob() == Job.Wizard) {
            GameObject.Find("PlayerJobText").GetComponent<Text>().text = "Clase: Mago";
        }
        else if(player.getJob() == Job.Swordsman) {
            GameObject.Find("PlayerJobText").GetComponent<Text>().text = "Clase: Espadachín";
        }
        GameObject.Find("MaxPVText").GetComponent<Text>().text = string.Concat("PV Máximos: ", player.getStat(Stat.MaxHealth));
        GameObject.Find("CurrentPVText").GetComponent<Text>().text = string.Concat("PV Actuales: ", player.getStat(Stat.CurrentHealth));
        GameObject.Find("MaxMPText").GetComponent<Text>().text = string.Concat("PM Máximos: ", player.getStat(Stat.ManaPoints));
        GameObject.Find("CurrentMPText").GetComponent<Text>().text = string.Concat("PM Actuales: ", player.getStat(Stat.CurrentManaPoints));
        GameObject.Find("StrengthText").GetComponent<Text>().text = string.Concat("Fuerza: ", player.getStat(Stat.Strength));
        GameObject.Find("IntelligenceText").GetComponent<Text>().text = string.Concat("Inteligencia: ", player.getStat(Stat.Intelligence));
        GameObject.Find("DexteryText").GetComponent<Text>().text = string.Concat("Destreza: ", player.getStat(Stat.Dextery));
        GameObject.Find("MoneyText").GetComponent<Text>().text = string.Concat("Dinero: ", player.getMoney().ToString());
        GameObject.Find("BaseDamageText").GetComponent<Text>().text = string.Concat("Daño Base: ", player.getStat(Stat.BaseDamage));
        if(player.getAccessory() == null) {
            GameObject.Find("EquippedAccessoryText").GetComponent<Text>().text = "Accesorio:";
        }
        else {
            GameObject.Find("EquippedAccessoryText").GetComponent<Text>().text = string.Concat("Accesorio: ", player.getAccessory().getName());
        }
        if(player.getWeapon() == null) {
            GameObject.Find("EquippedWeaponText").GetComponent<Text>().text = "Arma:";
        }
        else {
            GameObject.Find("EquippedWeaponText").GetComponent<Text>().text = string.Concat("Arma: ", player.getWeapon().getName());
        }
        if(player.getFaction() == Faction.Engineering) {
            factionIcon.sprite = factionsIcons[0];
        }
        else if(player.getFaction() == Faction.Science) {
            factionIcon.sprite = factionsIcons[2];
        }
        else {
            factionIcon.sprite = factionsIcons[1];
        }
        
        setItemsView();
        setButtonsView();
    }

    // Borra los prefabs de los objetos
    public void resetItemsView() {
        for(int i = 0; i < itemsPrefabs.Count; i++) {
            itemsPrefabs[i].SetActive(false);
            Destroy(itemsPrefabs[i]);
        }
        itemsPrefabs.Clear();
        resetDescription();
        setPlayerData();
    }

    // Borra la descripcion
    public void resetDescription() {
        Text descriptionText = descriptionView.GetComponent<Text>();
        descriptionText.text = "";  
    }

    // Muestra los prefabs de los objetos
    public void setItemsView() {
        // Falta hacer que se separe por tipos
        List<Item> inventory = player.getInventory();        
        for(int i = 0; i < inventory.Count; i++) {          
            if(inventory[i].isAccessoryItem()) {
                itemsPrefabs.Add((GameObject)Instantiate(accessoryButtonPrefab, new Vector3(0, 358 - i*84, 0), Quaternion.identity));
            }
            else if(inventory[i].isWeaponItem()) {
                itemsPrefabs.Add((GameObject)Instantiate(weaponButtonPrefab, new Vector3(0, 358 - i*84, 0), Quaternion.identity));
            }
            else if(inventory[i].canRecover()) {
                itemsPrefabs.Add((GameObject)Instantiate(recoveryStoneButtonPrefab, new Vector3(0, 358 - i*84, 0), Quaternion.identity));
            }
            else if(!inventory[i].canRecover() && inventory[i].canUse()) {
                itemsPrefabs.Add((GameObject)Instantiate(potionButtonPrefab, new Vector3(0, 358 - i*84, 0), Quaternion.identity));
            }                             
            itemsPrefabs[i].transform.SetParent(scrollView.transform, false);
            itemsPrefabs[i].GetComponent<InventoryItemButton>().setItem(inventory[i]);
        }         
    }

    // Alterna los botones del inventario
    public void setButtonsView() {
        if(selectedItem == null) {
            equipItemButton.SetActive(false);
            useItemButton.SetActive(false);
            removeItemButton.SetActive(false);
        }
        else if(selectedItem.canEquip()) {
            equipItemButton.SetActive(true);
            useItemButton.SetActive(false);
            removeItemButton.SetActive(true);          
        }
        else if(selectedItem.canUse()) {
            equipItemButton.SetActive(false);
            useItemButton.SetActive(true);
            removeItemButton.SetActive(true);
        }
    }

    public void changeTab(int value) {        
        if(value == 1) {
            inventoryTab.SetActive(true);
            resetItemsView();
            skillsTab.SetActive(false);
            questsTab.SetActive(false);
            inventoryButton.GetComponent<Image>().color = new Color32(92, 92, 92, 125);
            questsButton.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
            skillsButton.GetComponent<Image>().color = new Color32(255, 255, 255, 0);           
        }
        else if(value == 2) {
            inventoryTab.SetActive(false);
            skillsTab.SetActive(false);
            questsTab.SetActive(true);
            inventoryButton.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
            questsButton.GetComponent<Image>().color = new Color32(92, 92, 92, 125);
            skillsButton.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        }
        else if(value == 3) {
            inventoryTab.SetActive(false);
            skillsTab.SetActive(true);
            questsTab.SetActive(false);
            inventoryButton.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
            questsButton.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
            skillsButton.GetComponent<Image>().color = new Color32(92, 92, 92, 125);
            skillsTab.GetComponent<MenuSkillsManager>().resetSkillsView();
            skillsTab.GetComponent<MenuSkillsManager>().setSkillsView(player);
        }
    }

    public void closeMenu() {
        SceneManager.LoadScene("WorldScene");
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        player = gameManager.getPlayer();

        equipItemButton =  GameObject.Find("EquipItemButton");
        useItemButton =  GameObject.Find("UseItemButton");
        removeItemButton =  GameObject.Find("RemoveItemButton");
        descriptionView = GameObject.Find("ItemDataDialog");
        scrollView = GameObject.Find("ContentItems");
        inventoryButton = GameObject.Find("InventoryButton");
        questsButton = GameObject.Find("QuestsButton");
        skillsButton = GameObject.Find("SkillsButton");
        inventoryTab = GameObject.Find("InventoryTab");
        skillsTab = GameObject.Find("SkillsTab");
        questsTab = GameObject.Find("QuestsTab");
        itemsPrefabs = new List<GameObject>();   

        skillsTab.GetComponent<MenuSkillsManager>().resetSkillsView();
        skillsTab.GetComponent<MenuSkillsManager>().setSkillsView(player);
        inventoryTab.SetActive(true);
        skillsTab.SetActive(false);
        questsTab.SetActive(false);

        inventoryButton.GetComponent<Image>().color = new Color32(92, 92, 92, 125);
        setPlayerData();
        setButtonsView();
    }
    
    // Update is called once per frame
    void Update()
    {
      
    }
}
