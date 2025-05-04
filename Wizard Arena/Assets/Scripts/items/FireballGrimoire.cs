public class FireballGrimoire : MonoBehaviour, IUsableItem
{
    public Spell fireballSpell;

    public void Use(GameObject user)
    {
        if (fireballSpell == null)
        {
            Debug.LogError("Fireball spell reference is null!");
            return; // Exit early if the spell is null
        }

        if (SpellMenuManager.Instance == null)
        {
            Debug.LogError("SpellMenuManager instance is not initialized!");
            return; // Exit early if the instance is not initialized
        }

        Debug.Log("Unlocking Fireball spell...");
        SpellMenuManager.Instance.UnlockSpell(fireballSpell);
        Debug.Log("Fireball spell unlocked!");
    }
}