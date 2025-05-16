using UnityEngine;

public class GrimoireItem : MonoBehaviour, IUsableItem
{
    public void Use(GameObject player)
    {
        SkillManager.Instance.AddSkillPoint();
        Debug.Log("Grimoire consumed! Gained 1 skill point.");
        Debug.Log("SkillManager instance: " + SkillManager.Instance.GetInstanceID());

    }

}
