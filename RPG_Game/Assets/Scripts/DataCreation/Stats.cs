using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats
{

    [SerializeField]
    private int baseDamage;
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int currentHealth;
    [SerializeField]
    private int manaPoints;
    [SerializeField]
    private int currentManaPoints;
    [SerializeField]
    private int strength;
    [SerializeField]
    private int dextery;
    [SerializeField]
    private int intelligence;

    public Stats() {
        
    }

    public Stats(int newBaseDamage, int newMaxHealth, int newCurrentHealth, int newManaPoints, int newCurrentManaPoints, int newStrength, int newDextery, int newIntelligence) {
        baseDamage = newBaseDamage;
        maxHealth = newMaxHealth;
        currentHealth = newCurrentHealth;
        manaPoints = newManaPoints;
        currentManaPoints = newCurrentManaPoints;
        strength = newStrength;
        dextery = newDextery;
        intelligence = newIntelligence;
    }

    public int getMaxHealth() {
        return maxHealth;
    }

    public int getCurrentHealth() {
        return currentHealth;
    }

    public int getManaPoints() {
        return manaPoints;
    }

    public int getCurrentManaPoints() {
        return currentManaPoints;
    }

    public int getStrength() {
        return strength;
    }

    public int getDextery() {
        return dextery;
    }

    public int getIntelligence() {
        return intelligence;
    }

    public int getBaseDamage() {
        return baseDamage;
    }

    public void setMaxHealth(int value) {
        maxHealth = value;
    }

    public void setCurrentHealth(int value) {
        currentHealth = value;
    }

    public void setManaPoints(int value) {
        manaPoints = value;
    }

    public void setCurrentManaPoints(int value) {
        currentManaPoints = value;
    }

    public void setStrength(int value) {
        strength = value;
    }

    public void setDextery(int value) {
        dextery = value;
    }

    public void setIntelligence(int value) {
        intelligence = value;
    }

    public void setBaseDamage(int value) {
        baseDamage = value;
    }

    public void setStat(Stat stat, int value) {
        if(stat == Stat.BaseDamage) {
            setBaseDamage(value);
        }
        else if(stat == Stat.MaxHealth) {
            setMaxHealth(value);
        }
        else if(stat == Stat.CurrentHealth) {
            setCurrentHealth(value);
        }
        else if(stat == Stat.ManaPoints) {
            setManaPoints(value);
        }
        else if(stat == Stat.CurrentManaPoints) {
            setCurrentManaPoints(value);
        }
        else if(stat == Stat.Strength) {
            setStrength(value);
        }
        else if(stat == Stat.Dextery) {
            setDextery(value);
        }
        else if(stat == Stat.Intelligence) {
            setIntelligence(value);
        }
    }

    public int getStat(Stat stat) {
        if(stat == Stat.BaseDamage) {
            return baseDamage;
        }
        else if(stat == Stat.MaxHealth) {
            return maxHealth;
        }
        else if(stat == Stat.CurrentHealth) {
            return currentHealth;
        }
        else if(stat == Stat.ManaPoints) {
            return manaPoints;
        }
        else if(stat == Stat.CurrentManaPoints) {
            return currentManaPoints;
        }
        else if(stat == Stat.Strength) {
            return strength;
        }
        else if(stat == Stat.Dextery) {
            return dextery;
        }
        else if(stat == Stat.Intelligence) {
            return intelligence;
        }
        else {
            return 0;
        }
    }
}
