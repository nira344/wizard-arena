using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells/Spell")]
public class Spell : ScriptableObject
{
    public SpellType spellType;
    public string spellName;
    public Sprite spellIcon;
    public string description;
    public bool isMobilitySpell;
    public GameObject projectilePrefab;
    public GameObject prefab;
    public float cooldown = 1f;
    public float manaCost = 10f;

    public enum SpellType
    {
        Fireball,
        IceShard,
        ShadowDash
    }
}
