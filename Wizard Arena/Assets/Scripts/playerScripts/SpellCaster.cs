using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    void Update()
    {
        // Cast primary spell (E or Fire1)
        if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Fire1"))
        {
            Cast(SpellMenuManager.Instance.equippedPrimarySpell);
        }

        // Cast secondary spell (R or Fire2)
        if (Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("Fire2"))
        {
            Cast(SpellMenuManager.Instance.equippedSecondarySpell);
        }

        // Cast mobility spell (Left Shift)
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Cast(SpellMenuManager.Instance.equippedMobilitySpell);
        }
    }

    void Cast(Spell spell)
    {
        if (spell == null)
        {
            Debug.Log("No spell equipped!");
            return;
        }

        // Replace this with your actual casting logic
        Debug.Log("Casting: " + spell.spellName);

        if (spell.projectilePrefab != null)
        {
            Instantiate(spell.projectilePrefab, transform.position + transform.forward, transform.rotation);
        }
    }
}
