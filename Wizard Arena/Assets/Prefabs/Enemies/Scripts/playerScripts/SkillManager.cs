using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;
    public int skillPoints = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    public void AddSkillPoint()
    {
        skillPoints++;
        Debug.Log("Skill Point Gained! Total: " + skillPoints);
    }

    public bool SpendSkillPoint()
    {
        if (skillPoints > 0)
        {
            skillPoints--;
            return true;
        }

        return false;
    }
}
