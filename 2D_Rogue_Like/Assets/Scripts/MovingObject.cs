using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour 
{
	public float moveTime = 0.1f;
	public LayerMask blockingLayer;

	private BoxCollider2D boxColider;
	private Rigidbody2D rb2D;
	private float inverseMoveTime;


	// Use this for initialization
	protected virtual void Start ()
	{
		boxColider = GetComponent<BoxCollider2D>();
		rb2D = GetComponent<Rigidbody2D>();
		inverseMoveTime = 1f / moveTime;
	}

	protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
	{
		Vector2 start = transform.position;
		Vector2 end = start + new Vector2(xDir, yDir);

		boxColider.enabled = false;
		hit = Physics2D.Linecast (start, end, blockingLayer);
		boxColider.enabled = true;

		if (hit.transform == null)
		{
			Debug.Log ("StartCoroutine");
			StartCoroutine(SmoothMovement(end));
		}

		return false;
	}

	protected virtual void AttemptMove<T> (int xDir, int yDir)
		where T : Component
	{
		RaycastHit2D hit;
		bool canMove = Move (xDir, yDir, out hit);

		if (hit.transform == null)
		{
			return;
		}

		T hitComponent = hit.transform.GetComponent<T>();

		if (!canMove && hitComponent != null)
		{
			OnCantMove(hitComponent);
		}
	}

	protected IEnumerator SmoothMovement (Vector3 end)
	{
		float sqrRemainingDistance = (gameObject.transform.position - end).sqrMagnitude;
		int i = 0;
		while (sqrRemainingDistance > float.Epsilon)
		{
			Debug.Log ("Smooth inner loop " + ++i);
			Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
			rb2D.MovePosition(newPosition);
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			yield return null;
		}
	}

	protected abstract void OnCantMove<T>(T component)
		where T : Component;
}
