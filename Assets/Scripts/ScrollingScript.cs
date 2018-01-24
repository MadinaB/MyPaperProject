using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScrollingScript : MonoBehaviour
{
	
	public Vector2 speed = new Vector2(10, 10);
	public Vector2 direction = new Vector2(-1, 0);

	public bool isLinkedToCamera = false;
	public bool isLooping = false;
	private List<SpriteRenderer> backgroundPart;
	private Vector2 repeatableSize;
	void Start()
	{
		
		if (isLooping)
		{
			backgroundPart = new List<SpriteRenderer>();
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				SpriteRenderer r = child.GetComponent<SpriteRenderer>();
				if (r != null)
				{
					backgroundPart.Add(r);
				}
			}
			if (backgroundPart.Count == 0)
			{
				Debug.LogError("Nothing to scroll!");
			}
			backgroundPart = backgroundPart.OrderBy(t => t.transform.position.x * (-1 * direction.x)).ThenBy(t => t.transform.position.y * (-1 * direction.y)).ToList();
			var first = backgroundPart.First();
			var last = backgroundPart.Last();
			repeatableSize = new Vector2(
				Mathf.Abs(last.transform.position.x - first.transform.position.x),
				Mathf.Abs(last.transform.position.y - first.transform.position.y)
			);
		}
	}
	void Update()
	{
		Vector3 movement = new Vector3(
			speed.x * direction.x,
			speed.y * direction.y,
			0);

		movement *= Time.deltaTime;
		transform.Translate(movement);

		if (isLinkedToCamera)
		{
			Camera.main.transform.Translate(movement);
		}

		if (isLooping)
		{
			
			var dist = (transform.position - Camera.main.transform.position).z;
			float leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
			float rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;

			var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).y;
			var bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, dist)).y;

			Vector3 exitBorder = Vector3.zero;
			Vector3 entryBorder = Vector3.zero;

			if (direction.x < 0)
			{
				exitBorder.x = leftBorder;
				entryBorder.x = rightBorder;
			}
			else if (direction.x > 0)
			{
				exitBorder.x = rightBorder;
				entryBorder.x = leftBorder;
			}

			if (direction.y < 0)
			{
				exitBorder.y = bottomBorder;
				entryBorder.y = topBorder;
			}
			else if (direction.y > 0)
			{
				exitBorder.y = topBorder;
				entryBorder.y = bottomBorder;
			}

			SpriteRenderer firstChild = backgroundPart.FirstOrDefault();

			if (firstChild != null)
			{
				bool checkVisible = false;

				if (direction.x != 0)
				{
					if ((direction.x < 0 && (firstChild.transform.position.x < exitBorder.x))
						|| (direction.x > 0 && (firstChild.transform.position.x > exitBorder.x)))
					{
						checkVisible = true;
					}
				}
				if (direction.y != 0)
				{
					if ((direction.y < 0 && (firstChild.transform.position.y < exitBorder.y))
						|| (direction.y > 0 && (firstChild.transform.position.y > exitBorder.y)))
					{
						checkVisible = true;
					}
				}
				if (checkVisible)
				{
					
					if (firstChild.IsVisibleFrom(Camera.main) == false)
					{
						firstChild.transform.position = new Vector3(
							firstChild.transform.position.x + ((repeatableSize.x + firstChild.bounds.size.x) * -1 * direction.x),
							firstChild.transform.position.y + ((repeatableSize.y + firstChild.bounds.size.y) * -1 * direction.y),
							firstChild.transform.position.z
						);
						backgroundPart.Remove(firstChild);
						backgroundPart.Add(firstChild);
					}
				}
			}

		}
	}
}