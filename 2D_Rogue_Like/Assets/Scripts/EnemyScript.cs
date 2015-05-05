using UnityEngine;
using System.Collections;

public class EnemyScript : MovingObject 
{
	public int playerDamage;

	private Animator animator;
	private Transform target;
	private bool skipMove;

	public AudioClip enemyAttack1;
	public AudioClip enemyAttack2;

	protected override void Start () 
	{
		GameManagerScript.instance.AddEnemyToList (this);

		animator = GetComponent<Animator>();
		target = GameObject.FindGameObjectWithTag("Player").transform;
		base.Start ();
	}
	
	protected override void AttemptMove<T>(int xDir, int yDir)
	{
		// skipMove: 두턴에 한 번만 움직인다.
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

		AttemptMove<Player>(xDir, yDir);
	}

	protected override void OnCantMove<T> (T component)
	{
		Player hitPlayer = component as Player;

		animator.SetTrigger ("enemyAttack");

		hitPlayer.LoseFood (playerDamage);

		SoundManagerScript.instance.RandomizeSfx(enemyAttack1, enemyAttack2);
	}
}
