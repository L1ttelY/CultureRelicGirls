using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Direction {

	public const int right = 0;
	public const int up = 1;
	public const int left = 2;
	public const int down = 3; 

	public static Vector2 GetVector(int x) {
		x%=4;
		switch(x) {
		case right: return Vector2.right;
		case left: return Vector2.left;
		case up: return Vector2.up;
		case down: return Vector2.down;
		default: return Vector2.zero;
		}
	}
	public static Vector2Int GetVectorInt(int x) {
		x%=4;
		switch(x) {
		case right: return Vector2Int.right;
		case left: return Vector2Int.left;
		case up: return Vector2Int.up;
		case down: return Vector2Int.down;
		default: return Vector2Int.zero;
		}
	}

	public static int GetX(int x) {
		x%=4;
		switch(x) {
		case right: return 1;
		case up: return 0;
		case left: return -1;
		case down: return 0;
		default: return 0;
		}
	}
	public static int GetY(int x) {
		x%=4;
		switch(x) {
		case right: return 0;
		case up: return 1;
		case left: return 0;
		case down: return -1;
		default: return 0;
		}
	}

	public static int Reverse(int x) {
		x%=4;
		switch(x) {
		case right: return left;
		case up: return down;
		case left: return right;
		case down: return up;
		default: return -1;
		}
	}

	public static int random { get { return Random.Range(0,4); } }

}
