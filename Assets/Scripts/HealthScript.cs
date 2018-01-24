using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
	public int hp = 1;
	public bool isEnemy = true;

	public void Damage(int damageCount)
	{
		hp -= damageCount;

		if (hp <= 0)
		{
			// Dead!
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		// Is this a shot?
		ShotScript shot = otherCollider.gameObject.GetComponent<ShotScript>();
		if (shot != null)
		{
			if (shot.isEnemyShot != isEnemy)  // Avoid friendly fire
			{
				Damage(shot.damage);
				Destroy(shot.gameObject);    // Remember to always target the game object, otherwise you will just remove the script
			}
		}
	}
}