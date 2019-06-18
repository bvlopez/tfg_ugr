using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSkillsManager : MonoBehaviour
{

    private GameObject selectedSkillButton;
    private Skill selectedSkill;
    private GameObject descriptionView;
    private List<GameObject> skillsPrefabs;
    [SerializeField]
    private GameObject prefab;
    private GameObject scrollView;

    public void selectSkill(GameObject button, Skill skill) {
        if(selectedSkillButton != null) {
            selectedSkillButton.GetComponent<MenuSkillButton>().deselectSkill();
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
            skillsPrefabs[i].GetComponent<MenuSkillButton>().setSkill(skills[i]);
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

    // Start is called before the first frame update
    void Awake()
    {
        scrollView = GameObject.Find("ContentSkills");
        descriptionView = GameObject.Find("SkillDataDialog");     
        skillsPrefabs = new List<GameObject>();               
    }
  
    // Update is called once per frame
    void Update()
    {
        
    }
}
