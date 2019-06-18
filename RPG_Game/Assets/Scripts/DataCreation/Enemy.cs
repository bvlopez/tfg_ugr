using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy")]
public class Enemy : ScriptableObject
{
    [SerializeField]
    private int id;
    [SerializeField]
    private string enemyName;      
    [SerializeField]
    private Stats stats;
    [SerializeField]
    private bool isBoss;
    [SerializeField]
    private GameObject enemySprite;

    public int getId() {
        return id;
    }

    public string getName() {
        return enemyName;
    }   

    public bool isEnemyBoss() {
        return isBoss;
    }

    public GameObject getEnemySprite() {
        return enemySprite;
    }

    public int getStat(Stat value) {
        return stats.getStat(value);
    }

    public void setStat(Stat stat, int value) {
        stats.setStat(stat, value);
    }

    public Stats getStats() {
        return stats;
    }

    public void setName(string value) {
        name = value;
    }

    public void adjustAttributes(int playerLevel) {
        stats.setMaxHealth(stats.getMaxHealth() + playerLevel*2);
        stats.setCurrentHealth(stats.getCurrentHealth() + playerLevel*2);
        stats.setManaPoints(stats.getManaPoints() + playerLevel*2);
        stats.setCurrentManaPoints(stats.getCurrentManaPoints() + playerLevel*2);
        stats.setStrength(stats.getStrength() + playerLevel*2);
        stats.setDextery(stats.getDextery() + playerLevel*2);
        stats.setIntelligence(stats.getIntelligence() + playerLevel*2);
        stats.setBaseDamage(stats.getBaseDamage() + playerLevel*2);
    }
}
