using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestsTabManager : MonoBehaviour
{

    private GameObject activeButton;
    [SerializeField]
    private GameObject descriptionView;
    private List<GameObject> questsPrefabs;
    [SerializeField]
    private GameObject questButtonPrefab;
    private GameManager gameManager;

    public void showDescription(GameObject button, Quest quest) { 
        if(activeButton != null) {            
            activeButton.GetComponent<MenuQuestButton>().deselectQuest();
        }
        activeButton = button;
        Text descriptionText = descriptionView.GetComponent<Text>();       
        descriptionText.text = quest.getDescription();    
    }

    public void resetView() {
        Text descriptionText = descriptionView.GetComponent<Text>();
        descriptionText.text = "";
        //questsPrefabs.Clear();
    }   

    // Obtiene las misiones
    public void getQuests() {
        JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
        json.AddField("email", gameManager.getUser().getEmail());
        json.AddField("user_password", gameManager.getUser().getPassword());
        StartCoroutine(gameManager.getServerConnection().postRequest(json, "accepted_quests", getQuestsResponse));
    }

    public void getQuestsResponse(JSONObject json) {
        for(int j = 0; j < questsPrefabs.Count; j++) {
            questsPrefabs[j].SetActive(false);
            Destroy(questsPrefabs[j]);
        }
        questsPrefabs.Clear();
        GameObject scrollView = GameObject.Find("Content");

        JSONObject array_quests = json.GetField("quests");
        int i = 0;
        foreach(JSONObject j in array_quests.list) {
            int id = int.Parse(j.GetField("id").str);
            string title = j.GetField("title").str;              
            string description = j.GetField("description").str;
            int number = int.Parse(j.GetField("number").str);
            int experience = int.Parse(j.GetField("experience").str);
            int money = int.Parse(j.GetField("money").str);
            int progress = int.Parse(j.GetField("progress").str);
            string status = ""; 
            if(progress < 0) {
                status = "No aceptada";
                progress = 0;
            }
            else if(progress >= 0 && progress < number) {
                status = "Aceptada";
            }
            else if(progress >= number) {
                status = "Lista para entregar";
            }     
            questsPrefabs.Add((GameObject)Instantiate(questButtonPrefab, new Vector3(0, 358 - i*84, 0), Quaternion.identity));
            questsPrefabs[i].transform.SetParent(scrollView.transform, false);
            questsPrefabs[i].GetComponent<MenuQuestButton>().setQuest(new Quest(id, title, description, experience, money, progress, number, "", status));
            i++;   
        }
    }

    public void startReset(string value) {
        resetView();
    }

    // Start is called before the first frame update
    void Start()
    {    
        gameManager = GameManager.instance;
        GameObject scrollView = GameObject.Find("Content");
        questsPrefabs = new List<GameObject>();
        getQuests();      
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
