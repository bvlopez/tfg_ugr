using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    private Slider healthBar;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<Slider>();
        gameManager = GameManager.instance;
        healthBar.maxValue = gameManager.getPlayer().getStat(Stat.MaxHealth);
        healthBar.value = gameManager.getPlayer().getStat(Stat.CurrentHealth);
    }    

    public void setMaxHealth() {
        healthBar.maxValue = gameManager.getPlayer().getStat(Stat.MaxHealth);
    }

    public void setCurrentHealth() {
        healthBar.value = gameManager.getPlayer().getStat(Stat.CurrentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
