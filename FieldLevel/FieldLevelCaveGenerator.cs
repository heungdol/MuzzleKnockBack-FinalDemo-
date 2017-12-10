using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FieldLevelCaveGenerator : MonoBehaviour {

	private int numberOfHeight = 6;
	private int numberOfWidth = 8;

	private int numberOfHeightForPixel;
	private int numberOfWidthForPixel;

	private int lengthOfDoor = 3;

	private int lengthOfDoorForPixel;

	private int numberOfPixPerOne = 2;
	private int numberOfPixSide = 1;

	//private int levelTypeInt;	// 0, 1: Start, 2: Boss, 3: Vending
	private LEVELTYPE levelType;

	private int[,] blockInts = new int[1, 1];
	private int[,] goldPieceInts = new int[1, 1];

	[Range(0, 100)]
	private int fillPercent = 50;
	[Range(0, 100)]
	private int goldPieceFillPercent = 10;

	public GameObject cavePixelPrefab;
	//public GameObject caveGoldBlockPrefab;
	public GameObject[] caveGoldPiecePrefab;

	public void SetInfo (LEVELTYPE levelType, int height, int width, /*int type,*/ int fill, int door) {
		//this.levelTypeInt = type;
		this.fillPercent = fill;
		this.lengthOfDoor = door;
		this.levelType = levelType;

		switch (levelType) {
		case LEVELTYPE._11:
			this.numberOfHeight = height - 1;
			this.numberOfWidth = width - 1;

			fillPercent = 30;
			goldPieceFillPercent = 20;
			break;
		case LEVELTYPE._12:
			this.numberOfHeight = height - 1;
			this.numberOfWidth = (width - 1) * 2 + 1;

			fillPercent = 70;
			goldPieceFillPercent = 40;
			break;
		case LEVELTYPE._21:
			this.numberOfHeight = (height - 1) * 2 + 1;
			this.numberOfWidth = width - 1;

			fillPercent = 70;
			goldPieceFillPercent = 40;
			break;
		case LEVELTYPE._22:
			this.numberOfHeight = (height - 1) * 2 + 1;
			this.numberOfWidth = (width - 1) * 2 + 1;

			fillPercent = 50;
			goldPieceFillPercent = 30;
			break;
		}
	}

	void Start () {
		this.numberOfHeightForPixel = this.numberOfHeight * this.numberOfPixPerOne;
		this.numberOfWidthForPixel = this.numberOfWidth * this.numberOfPixPerOne;
		this.lengthOfDoorForPixel = (lengthOfDoor + 1) * numberOfPixPerOne;

		//goldBlockPivots = new List<Vector2> ();

		GenerateCavePixel ();

		for (int i = 0; i < 3; i++) {
			SmoothCavePixel (blockInts);
		}

		GenerateGoldBlock ();

		for (int i = 0; i < 2; i++) {
			SmoothGoldPiece (blockInts, goldPieceInts);
		}

		InstantitateCavePixel();
		SetScale ();

		InstantitateGoldBlock ();
	}

	void GenerateCavePixel () {
		blockInts = new int[numberOfHeightForPixel, numberOfWidthForPixel];
		string seed = (UnityEngine.Random.Range(0, 10000)).ToString();
		System.Random pseudoRandom = new System.Random (seed.GetHashCode ());

		// set	
		for (int y = 0; y < blockInts.GetLength(0); y++) {
			for (int x = 0; x < blockInts.GetLength(1); x++) {
				int indexXStart0 	= 0;
				int indexXEnd0 		= 0;
				int indexXStart1 	= 0;
				int indexXEnd1 		= 0;
				int indexYStart0 	= 0;
				int indexYEnd0 		= 0;
				int indexYStart1 	= 1;
				int indexYEnd1 		= 1;
				switch (levelType) {
				case LEVELTYPE._11:
					indexXStart0 	= (numberOfWidthForPixel / 2) - (lengthOfDoorForPixel / 2);
					indexXEnd0 		= (numberOfWidthForPixel / 2) + (lengthOfDoorForPixel / 2);
					indexYStart0 	= (numberOfHeightForPixel / 2) - (lengthOfDoorForPixel / 2);
					indexYEnd0 		= (numberOfHeightForPixel / 2) + (lengthOfDoorForPixel / 2);

					if (x < numberOfPixSide || x > numberOfWidthForPixel - numberOfPixSide - 1 || y < numberOfPixSide|| y > numberOfHeightForPixel - numberOfPixSide - 1) {
						if ((x < indexXStart0 || x > indexXEnd0) && (y < indexYStart0 || y > indexYEnd0))
							blockInts [y, x] = 1;
						else 
							blockInts [y, x] = 0;
					} else {
						blockInts [y, x] = (pseudoRandom.Next (0, 100) < fillPercent) ? 1 : 0;
					}
					break;

				case LEVELTYPE._21:
					indexXStart0 	= (numberOfWidthForPixel / 2) - (lengthOfDoorForPixel / 2);
					indexXEnd0 		= (numberOfWidthForPixel / 2) + (lengthOfDoorForPixel / 2);
					indexYStart0 	= (numberOfHeightForPixel / 4) - (lengthOfDoorForPixel / 2);
					indexYEnd0 		= (numberOfHeightForPixel / 4) + (lengthOfDoorForPixel / 2);
					indexYStart1 	= (numberOfHeightForPixel * 3 / 4) - (lengthOfDoorForPixel / 2);
					indexYEnd1 		= (numberOfHeightForPixel * 3 / 4) + (lengthOfDoorForPixel / 2);
					if (x < numberOfPixSide || x > numberOfWidthForPixel - numberOfPixSide - 1 || y < numberOfPixSide|| y > numberOfHeightForPixel - numberOfPixSide - 1) {
						if ((x < indexXStart0 || x > indexXEnd0) && (y < indexYStart0 || y > indexYEnd0) && (y < indexYStart1 || y > indexYEnd1))
							blockInts [y, x] = 1;
						else 
							blockInts [y, x] = 0;
					} else {
						blockInts [y, x] = (pseudoRandom.Next (0, 100) < fillPercent) ? 1 : 0;
					}
					break;

				case LEVELTYPE._12:
					indexXStart0 	= (numberOfWidthForPixel / 4) - (lengthOfDoorForPixel / 2);
					indexXEnd0 		= (numberOfWidthForPixel / 4) + (lengthOfDoorForPixel / 2);
					indexXStart1 	= (numberOfWidthForPixel * 3 / 4) - (lengthOfDoorForPixel / 2);
					indexXEnd1 		= (numberOfWidthForPixel * 3 / 4) + (lengthOfDoorForPixel / 2);
					indexYStart0 	= (numberOfHeightForPixel / 2) - (lengthOfDoorForPixel / 2);
					indexYEnd0 		= (numberOfHeightForPixel / 2) + (lengthOfDoorForPixel / 2);
					if (x < numberOfPixSide || x > numberOfWidthForPixel - numberOfPixSide - 1 || y < numberOfPixSide|| y > numberOfHeightForPixel - numberOfPixSide - 1) {
						if ((x < indexXStart0 || x > indexXEnd0) && (y < indexYStart0 || y > indexYEnd0) && (x < indexXStart1 || x > indexXEnd1))
							blockInts [y, x] = 1;
						else 
							blockInts [y, x] = 0;
					} else {
						blockInts [y, x] = (pseudoRandom.Next (0, 100) < fillPercent) ? 1 : 0;
					}
					break;

				case LEVELTYPE._22:
					indexXStart0 	= (numberOfWidthForPixel / 4) - (lengthOfDoorForPixel / 2);
					indexXEnd0 		= (numberOfWidthForPixel / 4) + (lengthOfDoorForPixel / 2);
					indexXStart1 	= (numberOfWidthForPixel * 3 / 4) - (lengthOfDoorForPixel / 2);
					indexXEnd1 		= (numberOfWidthForPixel * 3 / 4) + (lengthOfDoorForPixel / 2);
					indexYStart0 	= (numberOfHeightForPixel / 4) - (lengthOfDoorForPixel / 2);
					indexYEnd0 		= (numberOfHeightForPixel / 4) + (lengthOfDoorForPixel / 2);
					indexYStart1 	= (numberOfHeightForPixel * 3 / 4) - (lengthOfDoorForPixel / 2);
					indexYEnd1 		= (numberOfHeightForPixel * 3 / 4) + (lengthOfDoorForPixel / 2);
					if (x < numberOfPixSide || x > numberOfWidthForPixel - numberOfPixSide - 1 || y < numberOfPixSide|| y > numberOfHeightForPixel - numberOfPixSide - 1) {
						if ((x < indexXStart0 || x > indexXEnd0) && (y < indexYStart0 || y > indexYEnd0) && (x < indexXStart1 || x > indexXEnd1) && (y < indexYStart1 || y > indexYEnd1))
							blockInts [y, x] = 1;
						else 
							blockInts [y, x] = 0;
					} else {
						blockInts [y, x] = (pseudoRandom.Next (0, 100) < fillPercent) ? 1 : 0;
					}
					break;
				}
			}
		}
	}

	void SmoothCavePixel (int[,] ints) {
		for (int y = numberOfPixSide; y < ints.GetLength(0) - numberOfPixSide; y++) {
			for (int x = numberOfPixSide; x < ints.GetLength(1) - numberOfPixSide; x++) {
				if (GetSurroundBlockCount (ints, y, x) > 4) {
					ints [y, x] = 1;
				} else {
					ints [y, x] = 0;
				}
			}
		}
	}

	void SmoothGoldPiece (int[,] bInts, int[,] gInts) {
		for (int y = 0; y < gInts.GetLength(0); y++) {
			for (int x = 0; x < gInts.GetLength(1); x++) {
				if (gInts [y, x] == 1 && GetSurroundBlockCount (bInts, y, x) == 8 && GetSurroundBlockCount (gInts, y, x) > 1) {
					gInts [y, x] = 1;
				} else {
					gInts [y, x] = 0;
				}
			}
		}
	}

	void InstantitateCavePixel () {
		for (int y = 0; y < blockInts.GetLength(0); y++) {
			for (int x = 0; x < blockInts.GetLength(1); x++) {
				if (blockInts [y, x] == 1 || blockInts [y, x] == 2) {
					GameObject block = Instantiate (cavePixelPrefab, transform.position, Quaternion.identity);
					//block.transform.SetParent (blockParent.gameObject.transform);
					block.transform.SetParent (gameObject.transform);
					block.transform.localPosition 
					= new Vector3 ((blockInts.GetLength(1) / -2.0f) + 0.5f + x * 1.0f
						, (blockInts.GetLength(0) / -2.0f) + 0.5f + y * 1.0f
						, 0);
				}
			}
		}
	}

	int GetSurroundBlockCount (int[,] ints, int gridY, int gridX) {
		int count = 0;
		for (int y = gridY - 1; y <= gridY + 1; y++) {
			for (int x = gridX - 1; x <= gridX + 1; x++) {
				if (x >= 0 && x < ints.GetLength(1) && y >= 0 && y < ints.GetLength(0)) {
					if (x != gridX || y != gridY)
						count += ints [y, x];					
				} else {
					count++;
				}
			}
		}

		return count;
	}

	void SetScale () {
		Vector3 scale = Vector3.one * (1.0f / numberOfPixPerOne);
		gameObject.transform.localScale = scale;
	}

	void GenerateGoldBlock () {
		goldPieceInts = new int[blockInts.GetLength (0), blockInts.GetLength (1)];
		string seed = (UnityEngine.Random.Range(0, 10000)).ToString();
		System.Random pseudoRandom = new System.Random (seed.GetHashCode ());

		for (int y = numberOfPixPerOne/2; y < blockInts.GetLength (0) - numberOfPixPerOne/2; y++) {
			for (int x = numberOfPixPerOne/2; x < blockInts.GetLength (1) - numberOfPixPerOne/2; x++) {
				if (blockInts [y, x] == 1) {
					goldPieceInts [y, x] = (pseudoRandom.Next (0, 100) < goldPieceFillPercent) ? 1 : 0;
				}
			}
		}

	}

	void InstantitateGoldBlock () {
		GameObject goldBlockParent = new GameObject ();
		goldBlockParent.name = "FieldLevelGoldBlocks";
		goldBlockParent.transform.SetParent (gameObject.transform.parent.transform);
		goldBlockParent.transform.localPosition = Vector3.zero;

		for (int y = 0; y < goldPieceInts.GetLength (0); y++) {
			for (int x = 0; x < goldPieceInts.GetLength (1); x++) {
				if (goldPieceInts [y, x] == 1) {
					Vector3 goldBlockPosition = new Vector3 (-numberOfWidth/2.0f + x * 1.0f / numberOfPixPerOne, -numberOfHeight/2.0f + y * 1.0f / numberOfPixPerOne, 0);

					int goldInt = UnityEngine.Random.Range (0, 20);
					if (goldInt == 0) {
						goldInt = 0;
					} else if (1 <= goldInt && goldInt < 10) {
						goldInt = 1;
					} else if (10 <= goldInt && goldInt < 20) {
						goldInt = 2;
					}
						
					GameObject goldBlock = Instantiate (caveGoldPiecePrefab[goldInt], Vector3.zero, Quaternion.identity) as GameObject;
					goldBlock.name = "FieldLevelGoldBlock\t[" + x + ", " + y + "]";
					goldBlock.transform.SetParent (goldBlockParent.gameObject.transform);
					goldBlock.transform.localPosition = goldBlockPosition;
				}
			}
		}
	}
}
