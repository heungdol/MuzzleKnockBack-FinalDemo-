using UnityEngine;
//using System.Collections;
using System.Collections.Generic;
using System;

//[RequireComponent(typeof(FieldLevelManager))]
public class FieldLevelGenerator : MonoBehaviour {
	public int fixedLengthWidthOfLevel = 10;
	public int fixedLengthHeightOfLevel = 10;
	public int fixedLengthOfDoor = 3;

	public int numberOfVerticalLevels;
	public int numberOfHorizontalLevels;

	public int maxNumberOf22Levels;
	public int maxNumberOf21Levels;
	public int maxNumberOf12Levels;
	public int minNumberOf11Levels = 10;

	[Range(0, 100)]
	public int blockFillPercent;

	public GameObject blockPrefab;

	public GameObject framePivotPrefab;
	public GameObject frameWallPrefab;
	public GameObject backgroundPrefab;
	public GameObject levelPrefab;

	public bool directlyBoss;

	//private BossBattle bossBattle;

	void Awake () {
		CheckNumberOfLevels ();
		GenerateLevels ();
	}

	void CheckNumberOfLevels () {
		int totalLevels = numberOfHorizontalLevels * numberOfVerticalLevels;

		if (minNumberOf11Levels > totalLevels)
			minNumberOf11Levels = (int)(totalLevels * 0.5f);

		while (totalLevels < maxNumberOf22Levels * 4 + (maxNumberOf12Levels + maxNumberOf21Levels) * 2 + minNumberOf11Levels * 1) {
			int whatSubLevel = UnityEngine.Random.Range (0, 3);
			switch (whatSubLevel) {
			case 0:
				maxNumberOf22Levels--;
				break;
			case 1:
				maxNumberOf21Levels--;
				break;
			case 2:
				maxNumberOf12Levels--;
				break;
			}
		}
	}

	private void GenerateLevels () {
		int gridHeight = numberOfVerticalLevels * 2 + 1;
		int gridWidth = numberOfHorizontalLevels * 2 + 1;

		int[,] gridInts = new int[gridHeight, gridWidth];	// include pivots and walls
		// 0: pivot or wall, 1: opened wall, 2: space, 3: checked space, 4: special space

		for (int y = 0; y < gridInts.GetLength (0); y++) {
			for (int x = 0; x < gridInts.GetLength (1); x++) {
				if (x == 0 || x == gridInts.GetLength (1) - 1 || y == 0 || y == gridInts.GetLength (0) - 1 || x % 2 == 0 || y % 2 == 0) {
					gridInts [y, x] = 0;
				} else {
					gridInts [y, x] = 2;
				}
			}
		}

		// kill and hunt
		int currentX, currentY;
		do { currentX = UnityEngine.Random.Range (1, gridInts.GetLength(1)-1);
		} while (currentX % 2 == 0);
		do { currentY = UnityEngine.Random.Range (1, gridInts.GetLength(0)-1);
		} while (currentY % 2 == 0);

		do {
			gridInts [currentY, currentX] = 3;

			bool goToTop = true;
			bool goToRight = true;
			bool goToBottom = true;
			bool goToLeft = true;

			int goToDirection = UnityEngine.Random.Range (0, 4);
			int goToDirectionDelta = 1 + 2 * UnityEngine.Random.Range (0, 2);	// only 1 or 3

			bool determinedGoTo = false;

			while ((goToTop || goToRight || goToBottom || goToLeft) && (!determinedGoTo)) {
				switch (goToDirection) {
				case 0:		// top
					if (currentY + 2 < gridInts.GetLength (0) - 1 && gridInts [currentY + 2, currentX] == 2) {
						gridInts [currentY + 1, currentX] = 1;

						currentY += 2;
						determinedGoTo = true;
					} else {
						goToTop = false;
					}
					break;
				case 1:		// right
					if (currentX + 2 < gridInts.GetLength (1) - 1 && gridInts [currentY, currentX + 2] == 2) {
						gridInts [currentY, currentX + 1] = 1;

						currentX += 2;
						determinedGoTo = true;
					} else {
						goToRight = false;
					}
					break;
				case 2:		// bottom
					if (currentY - 2 > 0 && gridInts [currentY - 2, currentX] == 2) {
						gridInts [currentY - 1, currentX] = 1;

						currentY -= 2;
						determinedGoTo = true;
					} else {
						goToBottom = false;
					}
					break;
				case 3:		// left
					if (currentX - 2 > 0 && gridInts [currentY, currentX - 2] == 2) {
						gridInts [currentY, currentX - 1] = 1;

						currentX -= 2;
						determinedGoTo = true;
					} else {
						goToLeft = false;
					}
					break;
				}

				goToDirection += goToDirectionDelta;
				goToDirection %= 4;
			}

			if (determinedGoTo) 
				continue;

			int newGoToDirection = UnityEngine.Random.Range(0, 4);
			int newGoToDirectionDelta = 1 + 2 * UnityEngine.Random.Range(0, 2);
			
			for (int y = 1; y < gridHeight - 1 && !determinedGoTo; y += 2) {
				for (int x = 1; x < gridWidth - 1 && !determinedGoTo; x += 2) {
					if (gridInts [y, x] == 2) {
						for (int c = 0; c < 4 && !determinedGoTo; c++) {
							switch (newGoToDirection) {
							case 0 :
								if (y + 2 < gridHeight - 1 && gridInts [y + 2, x] == 3) {
									currentX = x;
									currentY = y;
									determinedGoTo = true;

									gridInts[y + 1, x] = 1;
								}
								break;
							case 1 :
								if (x + 2 < gridWidth - 1 && gridInts [y, x + 2] == 3) {
									currentX = x;
									currentY = y;
									determinedGoTo = true;

									gridInts[y, x + 1] = 1;
								}
								break;
							case 2 :
								if (y - 2 > 0 && gridInts [y - 2, x] == 3) {
									currentX = x;
									currentY = y;
									determinedGoTo = true;

									gridInts[y - 1, x] = 1;
								}
								break;
							case 3 :
								if (x - 2 > 0 && gridInts [y, x - 2] == 3) {
									currentX = x;
									currentY = y;
									determinedGoTo = true;

									gridInts[y, x - 1] = 1;
								}
								break;

							}
							newGoToDirection += newGoToDirectionDelta;
							newGoToDirection %= 4;
						}
					}
				}
			}

			if (determinedGoTo == false)
				break;

		} while (true);

		// make more space for 22 21 12 levels
		int currentNumberOf22Levels = 0;
		int currentNumberOf21Levels = 0;
		int currentNumberOf12Levels = 0;

		List<Vector2> _22LevelPivots = new List<Vector2>();
		List<Vector2> _21LevelPivots = new List<Vector2>();
		List<Vector2> _12LevelPivots = new List<Vector2>();
		List<Vector2> _11LevelPivots = new List<Vector2>();

		int i = 0;
		while (i < 100 && currentNumberOf22Levels < maxNumberOf22Levels) {
			int x = UnityEngine.Random.Range (1, gridWidth - 1 - 2);
			int y = UnityEngine.Random.Range (1, gridHeight - 1 - 2);

			if (gridInts [y, x] == 3 &&
			    gridInts [y, x + 2] == 3 &&
			    gridInts [y + 2, x] == 3 &&
			    gridInts [y + 2, x + 2] == 3) {

				gridInts [y, x] = 22;
				gridInts [y, x+1] = 22;
				gridInts [y, x+2] = 22;

				gridInts [y+1, x] = 22;
				gridInts [y+1, x+1] = 22;
				gridInts [y+1, x+2] = 22;

				gridInts [y+2, x] = 22;
				gridInts [y+2, x+1] = 22;
				gridInts [y+2, x+2] = 22;

				_22LevelPivots.Add(new Vector2 (x + 1, y + 1));
				currentNumberOf22Levels++;
			}

			i++;
		}

		i = 0;
		while (i < 1000 && currentNumberOf21Levels < maxNumberOf21Levels) {
			int x = UnityEngine.Random.Range (1, gridWidth - 1);
			int y = UnityEngine.Random.Range (1, gridHeight - 1 - 2);

			if (gridInts [y, x] == 3 &&
				gridInts [y + 1, x] == 1 &&
				gridInts [y + 2, x] == 3) {

				gridInts [y, x] = 21;
				gridInts [y+1, x] = 21;
				gridInts [y+2, x] = 21;

				_21LevelPivots.Add(new Vector2 (x, y + 1));
				currentNumberOf21Levels++;
			}

			i++;
		}

		i = 0;
		while (i < 1000 && currentNumberOf12Levels < maxNumberOf12Levels) {
			int x = UnityEngine.Random.Range (1, gridWidth - 1 - 2);
			int y = UnityEngine.Random.Range (1, gridHeight - 1);

			if (gridInts [y, x] == 3 &&
				gridInts [y, x + 1] == 1 &&
				gridInts [y, x + 2] == 3) {

				gridInts [y, x] = 12;
				gridInts [y, x+1] = 12;
				gridInts [y, x+2] = 12;

				_12LevelPivots.Add(new Vector2 (x + 1, y));
				currentNumberOf12Levels++;
			}

			i++;
		}

		for (int y = 1; y < gridInts.GetLength (0)-1; y += 2) {
			for (int x = 1; x < gridInts.GetLength (1)-1; x += 2) {
				if (gridInts [y, x] == 3) {
					gridInts [y, x] = 11;

					_11LevelPivots.Add (new Vector2 (x, y));
				}
			}
		}

		// test code
		GameObject test = new GameObject ();
		test.name = "TestBlocks";
		test.transform.position = new Vector3 ((int)(gridInts.GetLength (1) / 2), (int)(gridInts.GetLength (0) / 2), 0);

		for (int y = 0; y < gridInts.GetLength (0); y++) {
			for (int x = 0; x < gridInts.GetLength (1); x++) {
				GameObject block = Instantiate (blockPrefab, new Vector3 (x, y, 0), Quaternion.identity);
				block.gameObject.name = "block [" + x + ", " + y + "]";	// [x, y]
				block.gameObject.transform.SetParent(test.transform);
				switch (gridInts [y, x]) {
				case 0:
					block.GetComponent<MeshRenderer> ().material.color = Color.black;
					break;
				case 1:
					block.GetComponent<MeshRenderer> ().material.color = Color.red;
					break;
				case 4:
					block.GetComponent<MeshRenderer> ().material.color = Color.blue;
					break;
				default :
					block.GetComponent<MeshRenderer> ().material.color = Color.white;
					break;
				}
			}
		}

		test.SetActive (false);
		GameObject levelParent = new GameObject ();
		levelParent.name = "FieldLevel";

		// generate field level frame
		GameObject levelFrameParent = GenerateFrame (gridInts, gridHeight, gridWidth);
		levelFrameParent.transform.SetParent (levelParent.transform);

		// generate background
		GameObject levelBackgroundParent22 = GenerateBackground (_22LevelPivots, LEVELTYPE._22, gridInts, gridHeight, gridWidth);
		GameObject levelBackgroundParent21 = GenerateBackground (_21LevelPivots, LEVELTYPE._21, gridInts, gridHeight, gridWidth);
		GameObject levelBackgroundParent12 = GenerateBackground (_12LevelPivots, LEVELTYPE._12, gridInts, gridHeight, gridWidth);
		GameObject levelBackgroundParent11 = GenerateBackground (_11LevelPivots, LEVELTYPE._11, gridInts, gridHeight, gridWidth);

		levelBackgroundParent22.transform.SetParent (levelParent.transform);
		levelBackgroundParent21.transform.SetParent (levelParent.transform);
		levelBackgroundParent12.transform.SetParent (levelParent.transform);
		levelBackgroundParent11.transform.SetParent (levelParent.transform);

		if (!directlyBoss) {
			GameObject levelSlotParentSpecial11 = GenerateSpecial11Level (_11LevelPivots, gridInts, fixedLengthHeightOfLevel, fixedLengthWidthOfLevel);
			levelSlotParentSpecial11.transform.SetParent (levelParent.transform);
		}

		GameObject levelSlotParent22 = GenerateLevel (_22LevelPivots, LEVELTYPE._22, gridInts, fixedLengthHeightOfLevel, fixedLengthWidthOfLevel);
		GameObject levelSlotParent21 = GenerateLevel (_21LevelPivots, LEVELTYPE._21, gridInts, fixedLengthHeightOfLevel, fixedLengthWidthOfLevel);
		GameObject levelSlotParent12 = GenerateLevel (_12LevelPivots, LEVELTYPE._12, gridInts, fixedLengthHeightOfLevel, fixedLengthWidthOfLevel);
		GameObject levelSlotParent11 = GenerateLevel (_11LevelPivots, LEVELTYPE._11, gridInts, fixedLengthHeightOfLevel, fixedLengthWidthOfLevel);

		levelSlotParent22.transform.SetParent (levelParent.transform);
		levelSlotParent21.transform.SetParent (levelParent.transform);
		levelSlotParent12.transform.SetParent (levelParent.transform);
		levelSlotParent11.transform.SetParent (levelParent.transform);
	}

	GameObject GenerateFrame (int[,] gridInts, int gridHeight, int gridWidth) {
		GameObject levelframeParent = new GameObject ();
		levelframeParent.name = "FieldLevelFrames";
		levelframeParent.transform.position = new Vector3 (fixedLengthWidthOfLevel * (int)(gridInts.GetLength (1) / 2) * 0.5f, fixedLengthHeightOfLevel * (int)(gridInts.GetLength (0) / 2) * 0.5f, 0);

		for (int y = 0; y < gridInts.GetLength (0); y++) {
			for (int x = 0; x < gridInts.GetLength (1); x++) {
				bool xIsOdd = (x % 2 == 1) ? true : false;
				bool yIsOdd = (y % 2 == 1) ? true : false;
				GameObject a = null;
				if (gridInts [y, x] == 0) {
					if (!xIsOdd && !yIsOdd) {
						a = Instantiate (framePivotPrefab, new Vector3 ((int)(x / 2) * fixedLengthWidthOfLevel, (int)(y / 2) * fixedLengthHeightOfLevel, 0), Quaternion.identity) as GameObject;
						a.name = "PIV";

						int intT, intR, intB, intL;

						if (y + 1 < gridInts.GetLength (0)) 
							intT = gridInts [y + 1, x];
						else 
							intT = -1;

						if (x + 1 < gridInts.GetLength (1)) 
							intR = gridInts [y, x + 1];
						else 
							intR = -1;

						if (y - 1 >= 0) 
							intB = gridInts [y - 1, x];
						else 
							intB = -1;

						if (x - 1 >= 0) 
							intL = gridInts [y, x - 1];
						else 
							intL = -1;

						a.GetComponent<FieldLevelFramePivot> ().SetInfo (intT, intR, intB, intL);

					} else if (xIsOdd && !yIsOdd) {	// horizontal
						a = Instantiate (frameWallPrefab
							, new Vector3 ((x / 2.0f) * fixedLengthWidthOfLevel, (y / 2.0f) * fixedLengthHeightOfLevel, 0)
							, Quaternion.identity);
						a.GetComponent<FieldLevelFrameWall> ().SetInfo (gridWidth, gridHeight, x, y, fixedLengthWidthOfLevel-1, fixedLengthOfDoor, WALLTYPE.HORIZONTAL0);
						a.name = "HW";
					} else if (!xIsOdd && yIsOdd) {
						a = Instantiate (frameWallPrefab
							, new Vector3 ((x / 2.0f) * fixedLengthWidthOfLevel, (y / 2.0f) * fixedLengthHeightOfLevel, 0)
							, Quaternion.identity);
						a.GetComponent<FieldLevelFrameWall> ().SetInfo (gridWidth, gridHeight, x, y, fixedLengthHeightOfLevel-1, fixedLengthOfDoor, WALLTYPE.VERTICAL0);
						a.name = "VW";
					}

					if (a != null) {
						a.gameObject.transform.SetParent (levelframeParent.transform);
						a.name += "\t [" + x + ", " + y + "]";
					}

				} else if (gridInts [y, x] == 1) {
					if (xIsOdd && !yIsOdd) {
						a = Instantiate (frameWallPrefab, new Vector3 ((x / 2.0f) * fixedLengthWidthOfLevel, (y / 2.0f) * fixedLengthHeightOfLevel, 0), Quaternion.identity) as GameObject;
						a.GetComponent<FieldLevelFrameWall> ().SetInfo (gridWidth, gridHeight, x, y, fixedLengthWidthOfLevel-1, fixedLengthOfDoor, WALLTYPE.HORIZONTAL1);
						a.name = "HW";
					} else if (!xIsOdd && yIsOdd) {
						a = Instantiate (frameWallPrefab, new Vector3 ((x / 2.0f) * fixedLengthWidthOfLevel, (y / 2.0f) * fixedLengthHeightOfLevel, 0), Quaternion.identity) as GameObject;
						a.GetComponent<FieldLevelFrameWall> ().SetInfo (gridWidth, gridHeight, x, y, fixedLengthHeightOfLevel-1, fixedLengthOfDoor, WALLTYPE.VERTICAL1);
						a.name = "VW";
					}

					if (a != null) {
						a.gameObject.transform.SetParent (levelframeParent.transform);
						a.name += "\t [" + x + ", " + y + "]";
					}
				}
			}
		}

		return levelframeParent;
	}

	GameObject GenerateBackground (List<Vector2> pivots, LEVELTYPE levelType, int[,] gridInts, int gridHeight, int gridWidth) {
		GameObject levelBackgroundParent = new GameObject ();
		levelBackgroundParent.name = levelType.ToString() + "FieldLevelBackgrounds";
		levelBackgroundParent.transform.position = new Vector3 (fixedLengthWidthOfLevel * (int)(gridInts.GetLength (1) / 2) * 0.5f, fixedLengthHeightOfLevel * (int)(gridInts.GetLength (0) / 2) * 0.5f, 0);

		for (int i = 0; i < pivots.Count; i++) {
			Vector3 realPosition = new Vector3 (pivots[i].x * fixedLengthWidthOfLevel * 0.5f, pivots[i].y * fixedLengthHeightOfLevel * 0.5f, 0);

			GameObject levelBackground = Instantiate (backgroundPrefab, realPosition, Quaternion.identity) as GameObject;
			levelBackground.name = "BGS\t[" + pivots[i].x +", " + pivots[i].y + "]";
			levelBackground.transform.SetParent (levelBackgroundParent.transform);
			levelBackground.GetComponent<FieldLevelBackground> ().SetInfo (fixedLengthHeightOfLevel, fixedLengthWidthOfLevel, levelType);
		}

		return levelBackgroundParent;
	}

	GameObject GenerateLevel (List<Vector2> pivots, LEVELTYPE levelType, int[,] gridInts, int height, int width) {
		GameObject levelParent = new GameObject ();
		levelParent.name = levelType.ToString() + "FieldLevels";
		levelParent.transform.position = new Vector3 (fixedLengthWidthOfLevel * (int)(gridInts.GetLength (1) / 2) * 0.5f, fixedLengthHeightOfLevel * (int)(gridInts.GetLength (0) / 2) * 0.5f, 0);

		for (int i = 0; i < pivots.Count; i++) {
			Vector3 realPosition = new Vector3 (pivots[i].x * fixedLengthWidthOfLevel * 0.5f, pivots[i].y * fixedLengthHeightOfLevel * 0.5f, 0);

			GameObject level = Instantiate (levelPrefab, realPosition, Quaternion.identity) as GameObject;
			level.name = "FieldLevel\t[" + pivots[i].x + ", " + pivots[i].y + "]";
			level.transform.SetParent (levelParent.transform);
			level.GetComponent<FieldLevelController> ().SetInfo (levelType, height, width, fixedLengthOfDoor, blockFillPercent, 0);
		}

		return levelParent;
	}

	GameObject GenerateSpecial11Level (List<Vector2> _11pivots, int[,] gridInts, int height, int width) {
		GameObject levelParent = new GameObject ();
		levelParent.name = "_11FieldSpecialLevels";
		levelParent.transform.position = new Vector3 (fixedLengthWidthOfLevel * (int)(gridInts.GetLength (1) / 2) * 0.5f, fixedLengthHeightOfLevel * (int)(gridInts.GetLength (0) / 2) * 0.5f, 0);

		int i = 0;
		Vector3 realPosition = Vector3.zero;
		GameObject level = null;

		// ========== Start ==========
		i = UnityEngine.Random.Range (0, _11pivots.Count);
		realPosition = new Vector3 (_11pivots[i].x * fixedLengthWidthOfLevel * 0.5f, _11pivots[i].y * fixedLengthHeightOfLevel * 0.5f, 0);

		level = Instantiate (levelPrefab, realPosition, Quaternion.identity) as GameObject;
		level.name = "FieldSpecialLevel\t[" + _11pivots[i].x + ", " + _11pivots[i].y + "]";
		level.transform.SetParent (levelParent.transform);
		level.GetComponent<FieldLevelController> ().SetInfo (LEVELTYPE._11, height, width, fixedLengthOfDoor, blockFillPercent, 1);

		_11pivots.RemoveAt (i);

		// ========== Boss ==========
		i = UnityEngine.Random.Range (0, _11pivots.Count);
		realPosition = new Vector3 (_11pivots[i].x * fixedLengthWidthOfLevel * 0.5f, _11pivots[i].y * fixedLengthHeightOfLevel * 0.5f, 0);

		level = Instantiate (levelPrefab, realPosition, Quaternion.identity) as GameObject;
		level.name = "FieldSpecialLevel\t[" + _11pivots[i].x + ", " + _11pivots[i].y + "]";
		level.transform.SetParent (levelParent.transform);
		level.GetComponent<FieldLevelController> ().SetInfo (LEVELTYPE._11, height, width, fixedLengthOfDoor, blockFillPercent, 2);

		_11pivots.RemoveAt (i);

		// ========== Life Vending ==========
		for (int a = 0; a < 1; a++) {
			i = UnityEngine.Random.Range (0, _11pivots.Count);
			realPosition = new Vector3 (_11pivots[i].x * fixedLengthWidthOfLevel * 0.5f, _11pivots[i].y * fixedLengthHeightOfLevel * 0.5f, 0);

			level = Instantiate (levelPrefab, realPosition, Quaternion.identity) as GameObject;
			level.name = "FieldSpecialLevel\t[" + _11pivots[i].x + ", " + _11pivots[i].y + "]";
			level.transform.SetParent (levelParent.transform);
			level.GetComponent<FieldLevelController> ().SetInfo (LEVELTYPE._11, height, width, fixedLengthOfDoor, blockFillPercent, 4);

			_11pivots.RemoveAt (i);
		}

		// ========== Star Vending ==========
		for (int a = 0; a < 1; a++) {
			i = UnityEngine.Random.Range (0, _11pivots.Count);
			realPosition = new Vector3 (_11pivots [i].x * fixedLengthWidthOfLevel * 0.5f, _11pivots [i].y * fixedLengthHeightOfLevel * 0.5f, 0);

			level = Instantiate (levelPrefab, realPosition, Quaternion.identity) as GameObject;
			level.name = "FieldSpecialLevel\t[" + _11pivots [i].x + ", " + _11pivots [i].y + "]";
			level.transform.SetParent (levelParent.transform);
			level.GetComponent<FieldLevelController> ().SetInfo (LEVELTYPE._11, height, width, fixedLengthOfDoor, blockFillPercent, 5);

			_11pivots.RemoveAt (i);
		}

		return levelParent;
	}
}
