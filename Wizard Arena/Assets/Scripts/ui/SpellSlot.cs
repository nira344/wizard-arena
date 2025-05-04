using UnityEngine;
using UnityEngine.UI;

public class SpellSlot : MonoBehaviour
{
    public Image spellImage;
    public GameObject selectedPanel;

    private Spell storedSpell;

    public void ConfigureSlot(Spell spell)
    {
        storedSpell = spell;
        spellImage.sprite = spell.spellIcon;
        spellImage.enabled = true;
    }

    public void SelectSlot()
    {
        selectedPanel.SetActive(true);
        SpellMenuManager.Instance.ShowDescription(storedSpell);
    }

    public void DeselectSlot()
    {
        selectedPanel.SetActive(false);
    }

    public void Equip()
    {
        if (storedSpell != null)
        {
            SpellMenuManager.Instance.EquipSpell(storedSpell);
        }
    }

    public Spell GetStoredSpell() => storedSpell;
}
