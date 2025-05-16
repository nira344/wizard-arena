using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skills/Skill")]
public class Skill : ScriptableObject
{
    public string skillName;         // Name of the skill
    public string description;       // Description shown in the UI
    public Sprite icon;              // Skill icon for UI display
    public bool isMobility;          // True if this is a mobility skill
    public bool isUnlocked = false;  // Whether this skill is unlocked (persisted in SkillButton)
}
