using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SkillMenuManager : MonoBehaviour
{
    [Header("Description UI")]
    public Image descriptionImage;
    public TextMeshProUGUI descriptionNameText;
    public TextMeshProUGUI descriptionText;

    [Header("Equip Slots")]
    public SkillEquipButton equipSlotE;
    public SkillEquipButton equipSlotR;
    public SkillEquipButton mobilitySlot;

    [Header("UI Elements")]
    public GameObject skillPanel;
    public TextMeshProUGUI skillPointText;

    [Header("Skill Buttons")]
    public List<SkillButton> skillButtons = new List<SkillButton>();

    private SkillButton selectedButton;
    private SkillEquipButton selectedEquipSlot;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            skillPanel.SetActive(!skillPanel.activeSelf);

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (selectedButton != null && !selectedButton.isUnlocked)
            {
                TryUnlockSkill();
            }
            else if (selectedButton != null && selectedButton.isUnlocked)
            {
                TryEquipSkill(selectedButton.skill);
            }
            else if (selectedEquipSlot != null)
            {
                selectedEquipSlot.UnequipSkill();
                ClearDescription();
            }
        }

        // Always display current skill points
        skillPointText.text = "Skill Points: " + SkillManager.Instance.skillPoints;
    }

    public void SelectSkill(SkillButton button)
    {
        selectedButton = button;
        selectedEquipSlot = null;

        foreach (SkillButton b in skillButtons)
            b.SetSelected(b == selectedButton);

        equipSlotE.SetSelected(false);
        equipSlotR.SetSelected(false);
        mobilitySlot.SetSelected(false);

        // Update Description
        descriptionImage.sprite = button.skill.icon;
        descriptionNameText.text = button.skill.skillName;
        descriptionText.text = button.skill.description;
    }

    private void TryUnlockSkill()
    {
        if (selectedButton == null)
        {
            Debug.LogWarning("No selected button when trying to unlock.");
            return;
        }

        if (selectedButton.isUnlocked)
        {
            Debug.Log("Skill already unlocked.");
            return;
        }

        if (SkillManager.Instance.SpendSkillPoint())
        {
            selectedButton.UnlockSkill();
            Debug.Log("Unlocked: " + selectedButton.abilityName);
        }
        else
        {
            Debug.LogWarning("Not enough skill points to unlock skill.");
        }
    }

    private void TryEquipSkill(Skill skill)
    {
        if (skill.isMobility)
        {
            if (!mobilitySlot.HasSkill())
                mobilitySlot.EquipSkill(skill);
        }
        else
        {
            if (!equipSlotE.HasSkill())
                equipSlotE.EquipSkill(skill);
            else if (!equipSlotR.HasSkill())
                equipSlotR.EquipSkill(skill);
        }
    }

    public void SelectEquipSlot(SkillEquipButton slot)
    {
        selectedEquipSlot = slot;
        selectedButton = null;

        foreach (SkillButton b in skillButtons)
            b.SetSelected(false);

        equipSlotE.SetSelected(equipSlotE == slot);
        equipSlotR.SetSelected(equipSlotR == slot);
        mobilitySlot.SetSelected(mobilitySlot == slot);

        if (slot.HasSkill())
        {
            Skill s = slot.GetSkill();
            descriptionImage.sprite = s.icon;
            descriptionNameText.text = s.skillName;
            descriptionText.text = s.description;
        }
        else
        {
            ClearDescription();
        }
    }

    private void ClearDescription()
    {
        descriptionImage.sprite = null;
        descriptionNameText.text = "";
        descriptionText.text = "";
    }
}
