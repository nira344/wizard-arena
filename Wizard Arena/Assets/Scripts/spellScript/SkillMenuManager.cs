using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class SkillMenuManager : MonoBehaviour
{
    public GameObject skillPanel;
    public List<SkillButton> skillButtons;
    public TextMeshProUGUI skillPointText; // drag in the SkillPointText object
    public int skillPoints = 1;
    private SkillButton selectedButton;

    private SkillButton selectedSkillButton;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            skillPanel.SetActive(!skillPanel.activeSelf);

        if (Input.GetKeyDown(KeyCode.F) && selectedButton != null && !selectedButton.isUnlocked)
        {
            TryUnlockSkill();
        }

        skillPointText.text = "Skill Points: " + skillPoints;
    }

    public void SelectSkill(SkillButton button)
    {
        selectedButton = button;

        foreach (SkillButton b in skillButtons)
        {
            b.SetSelected(b == selectedButton); // only highlight the selected one
        }
    }

    private void TryUnlockSkill()
    {
        if (skillPoints > 0)
        {
            skillPoints--;
            selectedButton.isUnlocked = true;

            Debug.Log("Unlocked: " + selectedButton.abilityName);

            // Optional: update the button visuals here (e.g., remove dark overlay)
        }
    }
}
