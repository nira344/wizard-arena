using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SpellMenuManager : MonoBehaviour
{
    public static SpellMenuManager Instance;

    public GameObject spellMenuUI;
    public SpellSlot[] spellSlots;
    public EquipSlot[] equipSlots; // 0 and 1 for normal spells
    public EquipSlot mobilitySlot;
    public GameObject SpellPanel;

    public Image descriptionImage;
    public TMP_Text descriptionText;
    public TMP_Text descriptionNameText;

    private List<Spell> unlockedSpells = new List<Spell>();
    private bool isMenuOpen = false;
    public bool playerInZone = false;

    void Awake()
    {
        Instance = this;

        if (SpellPanel == null)
            Debug.LogError("SpellMenuManager: SpellPanel is not assigned in the Inspector!");
    }

    void Update()
    {
        if (playerInZone && Input.GetKeyDown(KeyCode.F))
        {
            if (isMenuOpen)
                CloseMenu();
            else
                OpenMenu();
        }
    }

    public void OpenMenu()
    {
        isMenuOpen = true;
        Time.timeScale = 0f;
        spellMenuUI.SetActive(true);
        SpellPanel.SetActive(true);

        foreach (Transform child in SpellPanel.transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void CloseMenu()
    {
        isMenuOpen = false;
        Time.timeScale = 1f;
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

    void UpdateSlots()
    {
        for (int i = 0; i < spellSlots.Length; i++)
        {
            if (i < unlockedSpells.Count)
                spellSlots[i].ConfigureSlot(unlockedSpells[i]);
            else
                spellSlots[i].gameObject.SetActive(false);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SpellMenuManager.Instance.playerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SpellMenuManager.Instance.playerInZone = false;

            if (Time.timeScale == 0f) // menu open
            {
                SpellMenuManager.Instance.CloseMenu();
            }
        }
    }
}
