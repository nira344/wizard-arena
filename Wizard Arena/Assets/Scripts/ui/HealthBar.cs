﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	public int maxHealth;
	public int currentHealth;

	private RectTransform rt;

    private void Start()
    {
        rt = gameObject.GetComponent<RectTransform>();
	}

    public void SetMaxHealth(int health)
	{
		Debug.Log("max hp set");
		maxHealth = health;
		SetHealth(health);
	}

    public void SetHealth(int health)
	{
		currentHealth = health;
		rt.localScale = new Vector3((5.0f * currentHealth / maxHealth), rt.localScale.y, rt.localScale.z);
	}

}
