using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillCatalog")]
public class SkillCatalog : ScriptableObject
{
    [SerializeField]
    private List<Skill> skills;

    public Skill getSkillByName(string name) {
        Skill value = skills.Find(skill => skill.getName().ToUpper() == name.ToUpper());
        return value;
    }

    public Skill getSkillById(int id) {
        Skill value = skills.Find(skill => skill.getId() == id);
        return value;
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
