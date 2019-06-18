using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{

    private int id;
    private string name;
    private int experience;
    private int money;
    private string description;
    private int progress;
    private int number;
    private string enemyName;
    private string status;

    public Quest(int newId, string newName, string newDescription, int newExperience, int newMoney, int newProgress, int newNumber, string newEnemyName, string newStatus) {
        id = newId;
        name = newName;
        description = newDescription;
        experience = newExperience;
        money = newMoney;
        progress = newProgress;
        number = newNumber;
        enemyName = newEnemyName;
        status = newStatus;
    }

    public int getId() {
        return id;
    }

    public string getName() {
        return name;
    }

    public int getExperience() {
        return experience;
    }

    public int getMoney() {
        return money;
    }

    public string getDescription() {
        return description;
    }

    public int getProgress() {
        return progress;
    }

    public int getNumber() {
        return number;
    }

    public string getEnemyName() {
        return enemyName;
    }

    public string getStatus() {
        if(progress >= number) {
            return "Lista para entregar";
        }
        else {
            return status;
        }
    }

    public bool isComplete() {
        if(progress >= number) {
            return true;
        }
        else {
            return false;
        }
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
