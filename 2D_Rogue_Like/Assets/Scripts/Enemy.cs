using UnityEngine;
using System.Collections;

public class Enemy : MovingObject 
{
	public int playerDamage;

	private Animator animator;
	private Transform target;
	private bool skipMove;

	protected override void Start () 
	{
		animator = GetComponent<Animator>();
		target = GameObject.FindGameObjectWithTag("Player").transform;
		base.Start ();
	}
	
	protected override void AttempMove<T>(int xDir, int yDir)
	{
		if (skipMove)
		{
			skipMove = false;
			return;
		}

		base.AttemptMove<T>(xDir, yDir);

		skipMove = true;
	}

	public void MoveEnemy()
	{
		int xDir = 0;
		int yDir = 0;

		if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)	// in the same column
		{
			yDir = target.position.y > transform.position.y ? 1 : -1;
		}
		else
			xDir = target.position.x > transform.position.x ? 1 : -1;
		AttempMove(xDir, yDir);
	}

	protected override void OnCantMove<T> (T component)
	{
		Player hitPlayer = component as Player;

		hitPlayer.LoseFood (playerDamage);

	}
}
