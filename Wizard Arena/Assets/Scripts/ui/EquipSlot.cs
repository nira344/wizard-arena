using UnityEngine;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour
{
    public Image spellImage;

    public void SetSpell(Spell spell)
    {
        spellImage.sprite = spell.spellIcon;
        spellImage.enabled = true;
    }
}