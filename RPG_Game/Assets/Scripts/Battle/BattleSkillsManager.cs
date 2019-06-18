using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSkillsManager : MonoBehaviour
{

    private GameObject selectedSkillButton;
    private Skill selectedSkill;
    private GameObject descriptionView;
    private List<GameObject> skillsPrefabs;
    [SerializeField]
    private GameObject prefab;
    private GameObject scrollView;

    void Awake()
    {
        scrollView = GameObject.Find("ContentSkills");
        descriptionView = GameObject.Find("SkillDataDialog");     
        skillsPrefabs = new List<GameObject>();               
    }

    public void selectSkill(GameObject button, Skill skill) {
        if(selectedSkillButton != null) {
            selectedSkillButton.GetComponent<BattleSkillButton>().deselectSkill();
        }
        selectedSkill = skill;
        selectedSkillButton = button;
    }

    public void showDescription(GameObject button, Skill skill) {
        selectSkill(button, skill);
        Text descriptionText = descriptionView.GetComponent<Text>();
        descriptionText.text = skill.getDescription();
    }

    public void setSkillsView(Player player) {
        List<Skill> skills = player.getSkills();        
        for(int i = 0; i < skills.Count; i++) {       
            skillsPrefabs.Add((GameObject)Instantiate(prefab, new Vector3(0, 358 - i*130, 0), Quaternion.identity));                    
            skillsPrefabs[i].transform.SetParent(scrollView.transform, false);
            skillsPrefabs[i].GetComponent<BattleSkillButton>().setSkill(skills[i]);
        }         
    }

    public void resetSkillsView() {
        for(int i = 0; i < skillsPrefabs.Count; i++) {
            skillsPrefabs[i].SetActive(false);
            Destroy(skillsPrefabs[i]);
        }
        skillsPrefabs.Clear();
        Text descriptionText = descriptionView.GetComponent<Text>();
        descriptionText.text = "";
    }

    public Skill getSelectedSkill() {
        return selectedSkill;
    }
}
