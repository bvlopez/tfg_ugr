using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuSkillButton : MonoBehaviour, IPointerDownHandler
{

    private Skill skill;
    private GameObject panel;
    private MenuSkillsManager menuSkillsManager;
    private bool selected;

    public void setSkill(Skill value) {
        skill = value;
        setView();
    }

    // Selecciona la habilidad
    public void selectSkill() {
        if(!selected) {                 
            GetComponent<Image>().color = Color.grey;
            menuSkillsManager.showDescription(gameObject, skill);
            selected = true;
        }
    }

    public void deselectSkill() {
        if(selected) {          
            GetComponent<Image>().color = new Color(194, 194, 194, 255);
            selected = false;
        }   
    }

    public void setView() {
        Text skillName = transform.Find("SkillName").GetComponent<Text>();     
        skillName.text = skill.getName();     
        Text skillCost = transform.Find("SkillCost").GetComponent<Text>();     
        skillCost.text = string.Concat(skill.getManaCost().ToString(), " PM");
        Text skillTurns = transform.Find("SkillTurns").GetComponent<Text>();     
        skillTurns.text = string.Concat(skill.getActiveTurns().ToString(), " Turnos");
        if(skill.canMultiTarget()) {
            Text skilltargets = transform.Find("SkillTargets").GetComponent<Text>();     
            skilltargets.text = "Multiples objetivos";     
        }
        else {
            Text skilltargets = transform.Find("SkillTargets").GetComponent<Text>();     
            skilltargets.text = "";   
        }
    }

    public void OnPointerDown (PointerEventData eventData) {
        selectSkill();
    }

    void Start()
    {   
        panel = GameObject.Find("SkillsTab");
        menuSkillsManager = panel.GetComponent<MenuSkillsManager>();
        selected = false;
        GetComponent<Image>().color = new Color(194, 194, 194, 255);
    }
}