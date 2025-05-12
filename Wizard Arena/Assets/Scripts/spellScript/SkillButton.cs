using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public Skill skill;
    public Image iconImage;
    public GameObject lockedOverlay;
    public GameObject selectedPanel;

    public string abilityName;
    public bool isUnlocked = false;

    private SkillMenuManager menuManager;

    void Start()
    {
        menuManager = FindFirstObjectByType<SkillMenuManager>();
        iconImage.sprite = skill.icon;
        UpdateVisual();
    }

    public void UnlockSkill()
    {
        skill.isUnlocked = true;
        isUnlocked = true;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        lockedOverlay.SetActive(!isUnlocked);
    }

    public void OnClick()
    {
        menuManager.SelectSkill(this);
    }

    public void SetSelected(bool isSelected)
    {
        selectedPanel.SetActive(isSelected);
    }
}
