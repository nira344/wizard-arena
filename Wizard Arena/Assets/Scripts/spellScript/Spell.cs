using UnityEngine;

[CreateAssetMenu(menuName = "Spells/New Spell")]
public class Spell : ScriptableObject
{
    public string spellName;
    public Sprite spellIcon;
    public string description;
    public bool isMobilitySpell;
}
