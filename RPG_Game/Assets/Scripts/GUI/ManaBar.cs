using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    private Slider manaBar;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        manaBar = GetComponent<Slider>();
        gameManager = GameManager.instance;
        manaBar.maxValue = gameManager.getPlayer().getStat(Stat.ManaPoints);
        manaBar.value = gameManager.getPlayer().getStat(Stat.CurrentManaPoints);
    }    

    public void setMaxHealth(int value) {
        manaBar.maxValue = gameManager.getPlayer().getStat(Stat.ManaPoints);
    }

    public void setCurrentHealth(int value) {
        manaBar.value = gameManager.getPlayer().getStat(Stat.CurrentManaPoints);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
