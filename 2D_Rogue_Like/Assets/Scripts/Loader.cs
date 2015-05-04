using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour 
{
	public GameObject gameManagerPrefab;

	// Use this for initialization
	void Awake ()
	{
		// GameManager 싱글턴 인스턴스가 아직 null 이면 gameManager 를 Instantiate
		// 	여기서 gameManager 는 UnityEngine 을 통해 gamaManager prefab 과 연결되어 있다.
		// 		즉, Instantiate 를 통해 gameManager prefab 이 GameObject 가 되는 것.
		if (GameManager.instance == null)
		{
			Instantiate(gameManagerPrefab);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
//		if (Input.GetKey ("up"))
//		{
		//			Destroy(gameManagerPrefab);
//			Instantiate (gameManager);
//		}
	}
}
