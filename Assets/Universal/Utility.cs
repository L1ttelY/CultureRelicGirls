using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Void();
public delegate void EventHandler(object sender);

public struct LoadTextEventArgs {
	public string language;
	public LoadTextEventArgs(string _language) {
		language=_language;
	}
}

public static class Utility {

	public static string GenerateNameFromGameObject(GameObject from) {
		System.Text.StringBuilder result = new System.Text.StringBuilder();
		for(Transform i = from.transform;i;i=i.parent) {
			result.Append("--");
			result.Append(i.gameObject.name);
		}
		result.Append("--");
		result.Append(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
		return result.ToString();
	}

	public static Rect GetWorldRect(this RectTransform rectTransform) {
		Vector3[] corners = new Vector3[4];
		rectTransform.GetWorldCorners(corners);
		// Get the bottom left corner.
		Vector3 position = corners[0];

		Vector2 size = new Vector2(
				rectTransform.lossyScale.x*rectTransform.rect.size.x,
				rectTransform.lossyScale.y*rectTransform.rect.size.y);

		return new Rect(position,size);
	}
	public static Rect GetLocalRect(this RectTransform rectTransform) {
		Vector3[] corners = new Vector3[4];
		rectTransform.GetLocalCorners(corners);
		// Get the bottom left corner.
		Vector3 position = corners[0];

		Vector2 size = new Vector2(
				rectTransform.lossyScale.x*rectTransform.rect.size.x,
				rectTransform.lossyScale.y*rectTransform.rect.size.y);

		return new Rect(position,size);
	}

	//调用NonAlloc函数时使用的缓存
	public static RaycastHit2D[] raycastBuffer = new RaycastHit2D[100];
	public static Collider2D[] colliderBuffer = new Collider2D[100];

	//常量


	//判断程序是否在调试环境下运行
	public static bool debug {
		get {
#if UNITY_EDITOR
			return true;
#else
			return false;
#endif
		}
	}

	//用矢量进行复数乘法
	public static Vector2 Product(Vector2 a,Vector2 b) {
		return new Vector2(a.x*b.x-a.y*b.y,a.x*b.y+a.y*b.x);
	}

	//用于寻找未激活的对象
	public static GameObject Find(string name) {
		GameObject result = GameObject.Find(name);
		if(result!=null) return result;
		var all = Resources.FindObjectsOfTypeAll<GameObject>();
		foreach(var i in all) {
			if(i.name==name) {
				result=i;
				break;
			}
		}
		return result;
	}

	public static void SetActive(GameObject obj,bool value) {
		obj.SetActive(value);
	}

	public static readonly System.Reflection.BindingFlags allMethods =
		System.Reflection.BindingFlags.NonPublic|
		System.Reflection.BindingFlags.Public|
		System.Reflection.BindingFlags.Static|
		System.Reflection.BindingFlags.Instance;

	/// <summary>
	/// 检测一个方法是否被重载, 若有同名方法则不应使用此方法
	/// </summary>
	/// <param name="testedObject">被检测的对象</param>
	/// <param name="testedMethod">被检测的方法名</param>
	/// <param name="baseType">被检测的方法被定义的基类</param>
	/// <returns></returns>
	public static bool MethodOverriden(object testedObject,string testedMethod,System.Type baseType) {
		var foundMethod = testedObject.GetType().GetMethod(testedMethod,allMethods);
		if(foundMethod==null) Debug.Log("METHOD NOT FOUND");
		if(foundMethod==null) return false;
		return foundMethod.DeclaringType!=baseType;
	}

	//数学运算
	public static bool Chance(float chance) {
		return Random.Range(0f,1f)<chance;
	}
	public static void Swap<T>(ref T a,ref T b) {
		T temp = a;
		a=b;
		b=temp;
	}

	public static float Cross(Vector2 a,Vector2 b) {
		return a.x*b.y-a.y*b.x;
	}

	public static float Taxicab(Vector2 a,Vector2 b) {
		return Mathf.Abs(a.x-b.x)+Mathf.Abs(a.y-b.y);
	}
	public static int Taxicab(Vector2Int a,Vector2Int b) {
		return Mathf.Abs(a.x-b.x)+Mathf.Abs(a.y-b.y);
	}

	//视线检测
	public static bool IfLineOfSight(Vector2 position,Vector2 target) {
		bool result = true;
		Vector2 direction = target-position;
		int cnt = Physics2D.CircleCastNonAlloc(position,0.2f,direction,raycastBuffer,direction.magnitude);
		for(int i = 0;i<cnt;i++)
			if(!raycastBuffer[i].collider.isTrigger&&raycastBuffer[i].collider.tag=="Wall") result=false;
		return result;
	}

	//绘制
	public static void GizmosDrawSquare(Vector2 start,Vector2 end,Color color) {
		Color col = Gizmos.color;
		Gizmos.color=color;
		Gizmos.DrawWireCube((start+end)/2,end-start);
		Gizmos.color=col;
	}

	//用于记录一些始终存在于游戏中的对象
	static void LoadStaticObjects() {
		staticParent=GameObject.Find("Statics");
		dynamicParent=GameObject.Find("Dynamics");
	}

	public static GameObject staticParent;
	public static GameObject dynamicParent;

	//静止帧
	const float maxWaitTime = 0.1f;
	static float timeWaitedThisFrame;
	public static void WaitSync(float time) {
		if(timeWaitedThisFrame>=maxWaitTime) return;
		timeWaitedThisFrame+=time;
		if(timeWaitedThisFrame>maxWaitTime) time-=timeWaitedThisFrame-maxWaitTime;
		float timeTarget = Time.realtimeSinceStartup+time;
		while(Time.realtimeSinceStartup<timeTarget) ;
	}

	public static void WaitSyncNoLimit(float time) {
		float timeTarget = Time.realtimeSinceStartup+time;
		while(Time.realtimeSinceStartup<timeTarget) ;
	}

}