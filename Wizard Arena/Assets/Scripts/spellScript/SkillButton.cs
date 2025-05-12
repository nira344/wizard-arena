using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public Skill skill;
    public Image iconImage;
    public GameObject lockedOverlay;

    public GameObject selectedPanel;
    public SkillMenuManager menuManager;

    public string abilityName;
    public bool isUnlocked = false;

    void Start()
    {
        menuManager = FindObjectOfType<SkillMenuManager>();
        iconImage.sprite = skill.icon;
        UpdateVisual();
    }

    public void UnlockSkill()
    {
        skill.isUnlocked = true;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        lockedOverlay.SetActive(!skill.isUnlocked);
    }

    public void OnClick()
    {
        menuManager.SelectSkill(this); // notify the manager
    }

    public void SetSelected(bool isSelected)
    {
        selectedPanel.SetActive(isSelected);
    }
}
