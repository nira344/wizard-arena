using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SpellMenuManager : MonoBehaviour
{
    public static SpellMenuManager Instance;

    public Spell equippedPrimarySpell;
    public Spell equippedSecondarySpell;
    public Spell equippedMobilitySpell;

    [Header("UI References")]
    public GameObject spellMenuUI;
    public GameObject SpellPanel;
    public SpellSlot[] spellSlots;
    public EquipSlot[] equipSlots; // 0 and 1 for normal spells
    public EquipSlot mobilitySlot;
    public Image descriptionImage;
    public TMP_Text descriptionText;
    public TMP_Text descriptionNameText;

    private List<Spell> unlockedSpells = new List<Spell>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void OpenMenu()
    {
        spellMenuUI.SetActive(true);
        SpellPanel.SetActive(true);
        UpdateSlots();

        foreach (Transform child in SpellPanel.transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void CloseMenu()
    {
        spellMenuUI.SetActive(false);
        SpellPanel.SetActive(false);
    }

    public void UnlockSpell(Spell spell)
    {
        if (!unlockedSpells.Contains(spell))
        {
            unlockedSpells.Add(spell);
        }
    }

    private void UpdateSlots()
    {
        for (int i = 0; i < spellSlots.Length; i++)
        {
            if (i < unlockedSpells.Count)
            {
                spellSlots[i].ConfigureSlot(unlockedSpells[i]);
                spellSlots[i].gameObject.SetActive(true);
            }
            else
            {
                spellSlots[i].gameObject.SetActive(false);
            }
        }
    }

    public void ShowDescription(Spell spell)
    {
        descriptionNameText.text = spell.spellName;
        descriptionImage.sprite = spell.spellIcon;
        descriptionText.text = spell.description;
    }

    public void EquipSpell(Spell spell)
    {
        if (spell.isMobilitySpell)
        {
            equippedMobilitySpell = spell;
            mobilitySlot.SetSpell(spell);
        }
        else
        {
            if (equippedPrimarySpell == null)
            {
                equippedPrimarySpell = spell;
                equipSlots[0].SetSpell(spell);
            }
            else
            {
                equippedSecondarySpell = spell;
                equipSlots[1].SetSpell(spell);
            }
        }
    }

    public Spell GetSpell(Spell.SpellType type)
    {
        if (type == Spell.SpellType.Fireball || type == Spell.SpellType.IceShard)
            return equippedPrimarySpell;
        if (type == Spell.SpellType.ShadowDash)
            return equippedMobilitySpell;

        return null;
    }
}
