using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterManager : MonoBehaviour
{
    [SerializeField]
    private GameObject filterPanel;
    private bool playersActivated;
    private bool bossesActivated;
    private bool shopsActivated;
    private bool guildsActivated;

    void Awake() {
        filterPanel.SetActive(false);
        playersActivated = true;
        bossesActivated = true;
        shopsActivated = true;
        guildsActivated = true;
    }

    public void openFilterPanel() {
        filterPanel.SetActive(true);
    }

    public void closeFilterPanel() {
        filterPanel.SetActive(false);
    }

    public void changePlayersFilter() {
        playersActivated = !playersActivated;
    }

    public void changeBossesFilter() {
        bossesActivated = !bossesActivated;
    }

    public void changeShopsFilter() {
        shopsActivated = !shopsActivated;
    }

    public void changeGuildsFilter() {
        guildsActivated = !guildsActivated;
    }

    public bool canShowPlayers() {
        return playersActivated;
    }

    public bool canShowGuilds() {
        return guildsActivated;
    }

    public bool canShowShops() {
        return shopsActivated;
    }

    public bool canShowBosses() {
        return bossesActivated;
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
