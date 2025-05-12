using UnityEngine;

[CreateAssetMenu(menuName = "Skill Tree/Skill")]
public class Skill : ScriptableObject
{
    public string skillName;
    public Sprite icon;
    public bool isUnlocked = false;
}
