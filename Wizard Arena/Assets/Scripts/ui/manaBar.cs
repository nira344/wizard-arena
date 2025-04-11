using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
	private int maxMana;
	private int currentMana;

	private RectTransform rt;

    private void Start()
    {
        rt = gameObject.GetComponent<RectTransform>();
	}

    public void SetMaxMana(int mana)
	{
		maxMana = mana;
		SetMana(mana);
	}

    public void SetMana(int mana)
	{
		currentMana = mana;
		rt.localScale = new Vector3 (5.0f * currentMana / maxMana, rt.localScale.y, rt.localScale.z);
	}

}
