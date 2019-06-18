using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill")]
public class Skill : ScriptableObject
{
    [SerializeField]
    private int id;
    [SerializeField]
    private string skillName;
    [SerializeField]
    private SkillType skillType;
    [SerializeField]
    private int value;
    [SerializeField]
    private bool multiTarget;
    [SerializeField]
    private Stat targetStat;
    [SerializeField]
    private int manaCost;
    [SerializeField]
    private int activeTurns;
    [SerializeField]
    private string description;

    /* public void use(CharacterController characterController) {
        if(isDamageSkill) {

        }
        else if(isBuffSkill) {
            
        }
        else if(isDebuffSkill) {
            
        }
    } */

    public bool canMultiTarget() {
        return multiTarget;
    }

    public Stat getTargetStat() {
        return targetStat;
    }

    public int getManaCost() {
        return manaCost;
    }

    public int getValue() {
        return value;
    }   

    public int getActiveTurns() {
        return activeTurns;
    }

    public string getName() {
        return skillName;
    }

    public string getDescription() {
        return description;
    }

    public int getId() {
        return id;
    }

    public SkillType getSkillType() {
        return skillType;
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
