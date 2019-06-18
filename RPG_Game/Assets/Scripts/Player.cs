using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    private int id;
    [SerializeField]
    private string name;
    [SerializeField]
    private int level;
    [SerializeField]
    private Stats stats;
    [SerializeField]
    private List<Item> inventory;
    [SerializeField]
    private List<Skill> skills;
    [SerializeField]
    private int money;
    [SerializeField]
    private Job job;
    [SerializeField]
    private Item accessory;
    [SerializeField]
    private Item weapon;
    [SerializeField]
    private int experience;
    [SerializeField]
    private int currentExperience;
    [SerializeField]
    private Faction faction;
    private GameManager gameManager;

    public Player() {
        inventory = new List<Item>();
        skills = new List<Skill>();
        stats = new Stats();
        gameManager = GameManager.instance;
    }

    public string getName() {
        return name;
    }

    public int getLevel() {
        return level;
    }

    public Stats getStats() {
        return stats;
    }

    public Faction getFaction() {
        return faction;
    }

    public List<Item> getInventory() {
        return inventory;
    }

    public int getMoney() {
        return money;
    }

    public Job getJob() {
        return job;
    }

    public Item getAccessory() {
        return accessory;
    }

    public Item getWeapon() {
        return weapon;
    }

    public int getExperience() {
        return experience;
    }

    public int getCurrentExperience() {
        return currentExperience;
    }

    public int getStat(Stat value) {
        return stats.getStat(value);
    }

    public List<Skill> getSkills() {
        return skills;
    }

    public void setName(string value) {
        name = value;
    }

    public void setLevel(int value) {
        level = value;
    }

    public void levelUp() {
        if(currentExperience >= experience) {
            level += 1;
            currentExperience = currentExperience - experience;
            experience = experience * level;
        }
    }

    public void setStats(Stats value) {
        stats = value;
    }

    public void setStat(Stat stat, int value) {
        stats.setStat(stat, value);
    }

    public void setExperience(int value) {
        experience = value;
    }

    public void setCurrentExperience(int value) {
        currentExperience = value;
    }

    public void setAccessory(Item value) {
        if(value.isAccessoryItem()) {
            if(accessory != null) {
                accessory.discard(this);
                inventory.Add(accessory);
            }            
            inventory.Remove(value);            
            accessory = value;    
            accessory.use(this);            
        }
    }

    public void setWeapon(Item value) {
        if(value.isWeaponItem()) {
            if(weapon != null) {
                weapon.discard(this);
                inventory.Add(weapon);
            }            
            inventory.Remove(value);            
            weapon = value;          
            weapon.use(this);             
        }
    }

    public void equipItem(Item value) {
        if(value.isAccessoryItem()) {   
            Item tmp = value;       
            setAccessory(value);
            gameManager.deleteItem(tmp);
            gameManager.sendEquipItems();            
            gameManager.sendPlayerAttributes();    
        }
        else if(value.isWeaponItem()) { 
           Item tmp = value;             
            setWeapon(value);  
            gameManager.deleteItem(tmp);  
            gameManager.sendEquipItems();               
            gameManager.sendPlayerAttributes();              
        }
    }

    public void useItem(Item value) {
       if(value.canUse()) {
            value.use(this);    
            gameManager.deleteItem(value);       
            inventory.Remove(value);   
            gameManager.sendPlayerAttributes();          
       }
    }

    public void buyItems(List<Item> values) {        
        for(int i = 0; i < values.Count; i++) {
            inventory.Add(values[i]);
            gameManager.sendItem(values[i]);
            money = money - values[i].getPrice();
        }
        gameManager.sendPlayerAttributes();        
    }

    public void removeItem(Item value) {
        gameManager.deleteItem(value);
        inventory.Remove(value);
    }

    public void setJob(Job value) {
        job = value;
    }

    public void setSkills(List<Skill> value) {
        skills = value;
    }

    public void setInventory(List<Item>  value) {
        inventory = value;
    }

    public void setFaction(Faction value) {
        faction = value;
    }

    public void setMoney(int value) {
        money = value;
    }

    public void clearInventory() {
        inventory.Clear();
    }

    public void setId(int value) {
        id = value;
    }

    public int getId() {
        return id;
    }

    /*
    public void getResponse(JSONObject json) {

        GameManager gameManager = GameManager.instance;        
        // Se actualiza el inventario
        JSONObject array_items = json.GetField("inventory");
        inventory.Clear();
		foreach(JSONObject j in array_items.list) {
			inventory.Add(gameManager.getItemCatalog().getItemByName(j.GetField("item_name").str));				
		}

        // Se actualizan las habilidades
        JSONObject array_skills = json.GetField("skills");
        skills.Clear();
		foreach(JSONObject j in array_skills.list) {
			skills.Add(gameManager.getSkillCatalog().getSkillByName(j.GetField("skill_name").str));				
		}

        weapon = gameManager.getItemCatalog().getItemByName(json.GetField("weapon").str);
        accessory = gameManager.getItemCatalog().getItemByName(json.GetField("accessory").str);
        name = json.GetField("name").str;

        // Se actualiza la clase
        if(json.GetField("class").str == "wizard") {
            job = Job.Wizard;
        }
        else if (json.GetField("class").str == "warrior") {
            job = Job.Warrior;
        }
        else {
            job = Job.Swordsman;
        }

        // Se actualiza la faccion
        if(json.GetField("faction").str == "science") {
            faction = Faction.Science;
        }
        else if (json.GetField("faction").str == "engineering") {
            faction = Faction.Engineering;
        }
        else {
            faction = Faction.ArtsAndHumanities;
        }

        level = int.Parse(json.GetField("player_level").str);
        experience = int.Parse(json.GetField("experience").str);
        currentExperience = int.Parse(json.GetField("current_experience").str);
        money = int.Parse(json.GetField("money").str);

        // Se actualizan las estadisticas principales
        stats.setStat(Stat.Strength, int.Parse(json.GetField("strength").str));
        stats.setStat(Stat.BaseDamage, int.Parse(json.GetField("base_damage").str));
        stats.setStat(Stat.MaxHealth, int.Parse(json.GetField("max_health").str));
        stats.setStat(Stat.CurrentHealth, int.Parse(json.GetField("current_health").str));
        stats.setStat(Stat.ManaPoints, int.Parse(json.GetField("mana").str));
        stats.setStat(Stat.CurrentManaPoints, int.Parse(json.GetField("current_mana").str));
        stats.setStat(Stat.Dextery, int.Parse(json.GetField("dextery").str));
        stats.setStat(Stat.Intelligence, int.Parse(json.GetField("intelligence").str));
        
    }*/

}
