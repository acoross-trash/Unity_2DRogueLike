using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MovingObject 
{
	public int wallDamage = 1;
	public int pointsPerFood = 10;
	public int pointsPerSoda = 20;
	public float restartLevelDelay = 1f;
	public Text foodText;
	public AudioClip moveSound1;
	public AudioClip moveSound2;
	public AudioClip eatSound1;
	public AudioClip eatSound2;
	public AudioClip drinkSound1;
	public AudioClip drinkSound2;
	public AudioClip gameOverSound;

	private Animator animator;
	private int food;

	// Use this for initialization
	protected override void Start () 
	{
		animator = GetComponent<Animator>();
		food = GameManagerScript.instance.playerFoodPoints;

		foodText.text = "Food: " + food;

		base.Start ();
	}

	private void OnDisable()
	{
		GameManagerScript.instance.playerFoodPoints = food;

	}

	void Update () 
	{
		if (!GameManagerScript.instance.playersTurn) return;

		int horizontal = 0;
		int vertical = 0;

		horizontal = (int) Input.GetAxisRaw ("Horizontal");
		vertical = (int) Input.GetAxisRaw ("Vertical");

		if (horizontal != 0)
		{
			vertical = 0;
		}

		if (vertical != 0 || horizontal != 0)
		{
			AttemptMove<Wall>(horizontal, vertical);
		}
	}

	public void Move(int horizontal, int vertical)
	{
		AttemptMove<Wall>(horizontal, vertical);
	}

	protected override void AttemptMove<T> (int xDir, int yDir)
	{
		--food;
		foodText.text = "Food: " + food;

		base.AttemptMove<T>(xDir, yDir);

		RaycastHit2D hit;
		if (Move (xDir, yDir, out hit))
		{
			print("movesound");
			SoundManagerScript.instance.RandomizeSfx(moveSound1, moveSound2);
		}

		CheckIfGameOver();

		GameManagerScript.instance.playersTurn = false;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Exit")
		{
			Invoke ("Restart", restartLevelDelay);
			enabled = false;
		}
		else if (other.tag == "Food")
		{
			food += pointsPerFood;
			foodText.text = "+" + pointsPerFood + " Food: " + food;
			SoundManagerScript.instance.RandomizeSfx(eatSound1, eatSound2);
			other.gameObject.SetActive(false);

		}
		else if (other.tag == "Soda")
		{
			food += pointsPerSoda;
			foodText.text = "+" + pointsPerSoda + " Food: " + food;
			SoundManagerScript.instance.RandomizeSfx(drinkSound1, drinkSound2);
			other.gameObject.SetActive(false);
		}

	}

	protected override void OnCantMove<T>(T component)
	{
		Wall hitWall = component as Wall;
		hitWall.DamageWall(wallDamage);

		animator.SetTrigger("playerChop");
	}

	private void Restart()
	{
		Application.LoadLevel(Application.loadedLevel);
	}

	public void LoseFood(int loss)
	{
		animator.SetTrigger ("playerHit");
		food -= loss;
		foodText.text = "-" + loss + " Food: " + food;
		CheckIfGameOver();
	}

	private void CheckIfGameOver()
	{
		if (food <= 0)
		{
			SoundManagerScript.instance.PlaySingle(gameOverSound);
			SoundManagerScript.instance.musicSource.Stop();
			GameManagerScript.instance.GameOver ();
		}
	}
}
