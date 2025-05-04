using UnityEngine;

public class IceShardGrimoire : MonoBehaviour, IUsableItem
{
    public Spell iceShardSpell;

    public void Use(GameObject user)
    {
        SpellMenuManager.Instance.UnlockSpell(iceShardSpell);
        Debug.Log("IceShard spell unlocked!");
    }
}
