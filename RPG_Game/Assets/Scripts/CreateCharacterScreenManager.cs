using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateCharacterScreenManager : MonoBehaviour
{

    private int activeJob;
    private int activeFaction;
    private int name;

    // Start is called before the first frame update
    void Start()
    {
        activeJob = 0;
        activeFaction = 0;
    }

    public int getActiveJob() {
        return activeJob;
    }

    public int getActiveFaction() {
        return activeFaction;
    }
    
    public void setActiveJob(int value) {
        if(value != activeJob) {
            if(activeJob == 1) {
                GameObject warriorButton = GameObject.Find("WarriorButton");
                WarriorButton warriorJob = warriorButton.GetComponent<WarriorButton>();
                warriorJob.deselectJob();
            }
            else if(activeJob == 2) {
                GameObject wizardButton = GameObject.Find("WizardButton");
                WizardButton wizardJob = wizardButton.GetComponent<WizardButton>();
                wizardJob.deselectJob();
            }
            else if(activeJob == 3) {
                GameObject swordsmanButton = GameObject.Find("SwordsmanButton");
                SwordsmanButton swordsmanJob = swordsmanButton.GetComponent<SwordsmanButton>();
                swordsmanJob.deselectJob();
            }
            activeJob = value;
        }
    }

    public void setActiveFaction(int value) {
        if(value != activeFaction) {
            if(activeFaction == 1) {
                GameObject artsButton = GameObject.Find("ArtsFactionButton");
                ArtsFactionButton artsFaction = artsButton.GetComponent<ArtsFactionButton>();
                artsFaction.deselectFaction();
            }
            else if(activeFaction == 2) {
                GameObject scienceButton = GameObject.Find("ScienceFactionButton");
                ScienceFactionButton scienceFaction = scienceButton.GetComponent<ScienceFactionButton>();
                scienceFaction.deselectFaction();
            }
            else if(activeFaction == 3) {
                GameObject engineeringButton = GameObject.Find("EngineeringFactionButton");
                EngineeringFactionButton engineeringFaction = engineeringButton.GetComponent<EngineeringFactionButton>();
                engineeringFaction.deselectFaction();
            }
            activeFaction = value;
        }
    }

    public void createCharacter() {
        string playerName = GameObject.Find("NewName").GetComponent<InputField>().text;
        if((activeFaction > 0) && (activeJob > 0) && (playerName != null)) {
            if(playerName.Length > 0) {
                GameManager gameManager = GameManager.instance;
                JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
                Faction faction;
                if(activeFaction == 1) {
                    faction = Faction.ArtsAndHumanities;
                    json.AddField("faction", "artsandhumanities");
                }
                else if(activeFaction == 2) {
                    faction = Faction.Science;
                    json.AddField("faction", "science");
                }
                else {
                    faction = Faction.Engineering;
                    json.AddField("faction", "engineering");
                }
                Job job;
                if(activeJob == 1) {
                    job = Job.Warrior;
                    json.AddField("class", "warrior");
                }
                else if(activeJob == 2) {
                    job = Job.Wizard;
                    json.AddField("class", "wizard");
                }
                else {
                    job = Job.Swordsman;
                    json.AddField("class", "swordsman");
                }
                json.AddField("name", playerName);     
                json.AddField("email", gameManager.getUser().getEmail());
                json.AddField("user_password", gameManager.getUser().getPassword());                      
                StartCoroutine(gameManager.getServerConnection().postRequest(json, "create_player", getResponse));        
            }
        }
    }

    public void getResponse(JSONObject json) {
        GameManager gameManager = GameManager.instance;       
        gameManager.setPlayer(); 
        StartCoroutine(gameManager.waitPlayerUpdate("WorldScene", gameManager.changeScene));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
