using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item")]
public class Item : ScriptableObject
{
    [SerializeField]
    private int id;
    [SerializeField]
    private string itemName;
    [SerializeField]
    private ItemType itemType;    
    [SerializeField]
    private List<int> values;
    [SerializeField]
    private List<Stat> targetStats;
  
    [SerializeField]    
    private int price;
    [SerializeField]
    private Sprite icon; 
    [SerializeField]
    private string description; 

    /*
    public void useInCombat(PlayerController playerController) {
        if(isUsable && !isRecoveryStone) {

        }
    }
    */

    // Usa un objeto del inventario del jugador desde el menu
    public void use(Player player) {
        if(canUse()) {
            // Se usa la piedra de recuperacion
            if(canRecover()) {
                if(player.getStat(Stat.CurrentHealth) <= 0) {
                    player.setStat(Stat.CurrentHealth, values[0]);
                }
            }
            // Se usan las pociones
            else {
                if(player.getStat(Stat.CurrentHealth) > 0) {                    
                    // Pocion de salud
                    if(targetStats[0] == Stat.CurrentHealth) {
                        // En caso de que no sobrepase la vida maxima
                        if((player.getStat(targetStats[0]) + values[0]) <= player.getStat(Stat.MaxHealth)) {
                            player.setStat(targetStats[0], player.getStat(targetStats[0]) + values[0]);
                        }
                        // Si sobrepasa la vida maxima con la pocion la vida actual se pone al maximo
                        else {
                            player.setStat(targetStats[0], player.getStat(Stat.MaxHealth));
                        }
                    }
                    // Pocion de mana
                    else if(targetStats[0] == Stat.CurrentManaPoints) {
                        // En caso de que no sobrepase el mana maximo
                        if((player.getStat(targetStats[0]) + values[0]) <= player.getStat(Stat.ManaPoints)) {
                            player.setStat(targetStats[0], player.getStat(targetStats[0]) + values[0]);
                        }
                        // Si sobrepasa el mana maximo con la pocion el mana actual se pone al maximo
                        else {
                            player.setStat(targetStats[0], player.getStat(Stat.ManaPoints));
                        }
                    }
                    
                }
            }
        }
        else if(canEquip()) {
            for(int i = 0; i < values.Count; i++) {
                player.setStat(targetStats[i], player.getStat(targetStats[i]) + values[i]);
            }
        }
    }

    public void discard(Player player) {
        for(int i = 0; i < values.Count; i++) {
            player.setStat(targetStats[i], player.getStat(targetStats[i]) - values[i]);
        }
    }

    public int getId() {
        return id;
    }

    public string getDescription() {
        return description;
    }

    public string getName() {
        return itemName;
    }

    public bool canEquip() {
         if(itemType == ItemType.Accessory || itemType == ItemType.Weapon) {
            return true;
        }
        else {
            return false;
        }
    }

    public bool canUse() {
        if(itemType == ItemType.Potion || itemType == ItemType.RecoveryStone) {
            return true;
        }
        else {
            return false;
        }
    }

    public bool canRecover() {
        if(itemType == ItemType.RecoveryStone) {
            return true;
        }
        else {
            return false;
        }
    }

    public List<int> getAllValues() {
        return values;
    }

    public List<Stat> getAllTargetStats() {
        return targetStats;
    }

    public int getValue(int i) {
        return values[i];
    }

    public Stat getTargetStat(int i) {
        return targetStats[i];
    }

    public int getNumberOfStats() {
        return targetStats.Count;
    }

    public int getPrice() {
        return price;
    }

    public Sprite getIcon() {
        return icon;
    }

    public bool isAccessoryItem() {
        if(itemType == ItemType.Accessory) {
            return true;
        }
        else {
            return false;
        }
    }

    public bool isWeaponItem() {
        if(itemType == ItemType.Weapon) {
            return true;
        }
        else {
            return false;
        }
    }

    public ItemType getItemType() {
        return itemType;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
