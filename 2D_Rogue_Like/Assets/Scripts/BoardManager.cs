using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Random = UnityEngine.Random;

// 레벨에 맞춰 보드를 랜덤 생성하는 클래스
public class BoardManager : MonoBehaviour
{
	[Serializable]
	public class Count
	{
		public int minimum;
		public int maximum;

		public Count (int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}

	#region 에디터에서 연결
	public int columns = 8;
	public int rows = 8;

	public Count wallCount = new Count (5, 9);
	public Count foodCount = new Count (1, 5);
	public GameObject exit;
	public GameObject[] floorTiles;
	public GameObject[] outerWallTiles;
	public GameObject[] wallTiles;
	public GameObject[] foodTiles;
	public GameObject[] enemyTiles;
	#endregion

	private Transform boardHolder;		// 게임판 GameObject 의 Transform의 reference
	private List<Vector3> gridPositions = new List<Vector3>();		// GameObject 생성을 위한 gridPosition 리스트. 사용된 위치는 리스트에서 제거.


	public void SetupScene(int level)
	{
		BoardSetup();
		InitializeGridPositions();
		LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
		LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
		
		int enemyCount = (int)Mathf.Log(level, 2f);
		LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);

		Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
	}

	#region private interface
	void BoardSetup()
	{
		boardHolder = (new GameObject ("Board")).transform;

		for (int x = -1; x < columns + 1; ++x) 
		{
			for (int y = -1; y < rows + 1; ++y) 
			{
				GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];
				if (x == -1 || x == columns || y == -1 || y == rows)
				{
					toInstantiate = outerWallTiles[Random.Range (0, outerWallTiles.Length)];
				}

				GameObject instance = (GameObject)(Instantiate (toInstantiate, new Vector3(x, y, 0f), Quaternion.identity));
				instance.transform.SetParent (boardHolder);
			}
		}
	}

	// gridPosition 설정.
	void InitializeGridPositions()
	{
		gridPositions.Clear ();
		for (int x = 1; x < columns - 1; ++x) 
		{
			for (int y = 1; y < rows - 1; ++y)
			{
				gridPositions.Add (new Vector3 (x, y, 0f));
			}
		}
	}

	void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
	{
		int objectCount = Random.Range(minimum, maximum + 1);
		
		for (int i = 0; i < objectCount; ++i)
		{
			Vector3 randomPosition = RandomPosition();
			GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
			Instantiate(tileChoice, randomPosition, Quaternion.identity);
		}
	}

	Vector3 RandomPosition()
	{
		int randomIndex = Random.Range(0, gridPositions.Count);
		Vector3 randomPosition = gridPositions[randomIndex];
		gridPositions.RemoveAt(randomIndex);
		return randomPosition;
	}
	#endregion
}
