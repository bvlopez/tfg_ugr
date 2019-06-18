using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GuildQuestButton : MonoBehaviour, IPointerDownHandler
{
    private Quest quest;
    private bool selected;

    public void setQuest(Quest value) {
        quest = value;
        setView();
    }

    public void setView() {
        Text questName = transform.Find("QuestName").GetComponent<Text>();
        Text questStatus = transform.Find("QuestStatus").GetComponent<Text>();
        Text questProgress = transform.Find("QuestProgress").GetComponent<Text>();
        Text questExperience = transform.Find("QuestExperience").GetComponent<Text>();
        Text questMoney = transform.Find("QuestMoney").GetComponent<Text>();

        questName.text = quest.getName();
        questStatus.text = quest.getStatus();
        questProgress.text = quest.getProgress().ToString() + "/" + quest.getNumber();
        questExperience.text = quest.getExperience().ToString() + " PE";
        questMoney.text = quest.getMoney().ToString() + " G";     
    }

    public void selectQuest() {
        if(!selected) {
            GameObject guildManagerGameObject = GameObject.Find("GuildManager");
            GuildManager guildManager = guildManagerGameObject.GetComponent<GuildManager>();           
            GetComponent<Image>().color = Color.grey;
            guildManager.showDescription(gameObject, quest);
            selected = true;
        }
    }

    public void deselectQuest() {
        if(selected) {
            GetComponent<Image>().color = new Color(194, 194, 194, 255);
            selected = false;
        }
    }

    public Quest getQuest() {
        return quest;
    }

    // Start is called before the first frame update
    void Start()
    {
        selected = false;
        GetComponent<Image>().color = new Color(194, 194, 194, 255);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown (PointerEventData eventData) {
        selectQuest();
    }
}
