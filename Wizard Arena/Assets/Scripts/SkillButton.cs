using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public Skill skill;
    public string abilityName;
    public GameObject selectedPanel;

    public bool isUnlocked = false;

    public void SetSelected(bool selected)
    {
        if (selectedPanel != null)
            selectedPanel.SetActive(selected);
    }

    public void UnlockSkill()
    {
        isUnlocked = true;
        GetComponent<Image>().color = Color.white; // Example: change color to indicate unlock
    }

    public void OnClick()
    {
        FindFirstObjectByType<SkillMenuManager>().SelectSkill(this);
    }
}
