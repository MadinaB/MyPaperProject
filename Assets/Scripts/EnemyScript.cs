using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
	private WeaponScript weapon;

	void Awake()
	{
		weapon = GetComponent<WeaponScript>();
	}

	void Update()
	{
		if (weapon != null && weapon.CanAttack)
		{
			weapon.Attack(true);
		}
	}
}
