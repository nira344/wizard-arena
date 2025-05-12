using UnityEngine;
using UnityEngine.UI;

public class SkillEquipButton : MonoBehaviour
{
    public Image iconImage;
    public GameObject selectedPanel;

    private Skill equippedSkill;
    private SkillMenuManager menuManager;

    void Start()
    {
        menuManager = FindObjectOfType<SkillMenuManager>();
        UnequipSkill();
    }

    public void OnClick()
    {
        menuManager.SelectEquipSlot(this);
    }

    public void EquipSkill(Skill skill)
    {
        equippedSkill = skill;
        iconImage.sprite = skill.icon;
        iconImage.enabled = true;
    }

    public void UnequipSkill()
    {
        equippedSkill = null;
        iconImage.sprite = null;
        iconImage.enabled = false;
    }

    public bool HasSkill() => equippedSkill != null;
    public Skill GetSkill() => equippedSkill;

    public void SetSelected(bool selected)
    {
        selectedPanel.SetActive(selected);
    }
}
