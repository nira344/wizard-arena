using UnityEngine;

public class ShadowDashGrimoire : MonoBehaviour, IUsableItem
{
    public Spell shadowDashSpell;

    public void Use(GameObject user)
    {
        SpellMenuManager.Instance.UnlockSpell(shadowDashSpell);
        Debug.Log("Shadow Dash spell unlocked!");
    }
}
