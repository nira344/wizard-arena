using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SpellMenuManager : MonoBehaviour
{
    public static SpellMenuManager Instance;

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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (SpellPanel == null || spellMenuUI == null)
        {
            Debug.LogError("SpellMenuManager: SpellPanel or spellMenuUI is not assigned!");
        }
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

    public void UnlockSpell(Spell newSpell)
    {
        if (!unlockedSpells.Contains(newSpell))
        {
            unlockedSpells.Add(newSpell);
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
            mobilitySlot.SetSpell(spell);
        }
        else
        {
            if (!equipSlots[0].spellImage.enabled)
                equipSlots[0].SetSpell(spell);
            else
                equipSlots[1].SetSpell(spell);
        }
    }
}
