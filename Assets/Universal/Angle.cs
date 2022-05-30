using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *表示角度的结构体
 *val为角度,保持在0~360内
 */
public struct Angle {
	private float val;
	public float degree {
		get { return val; }
		set { val=value; Update(); }
	}
	public float radian {
		get { return val*Mathf.Deg2Rad; }
		set { val=value*Mathf.Rad2Deg; Update(); }
	}
	public Quaternion quaternion {
		get { return Quaternion.Euler(0,0,degree); }
		set { val=value.eulerAngles.z; }
	}
	public Vector2 vector {
		get { return new Vector2(cos,sin); }
		set { this=new Angle(vector); }
	}
	public int direction {
		get {
			if(IfBetween((Angle)315,(Angle)45)) return Direction.right;
			if(IfBetween((Angle)45,(Angle)135)) return Direction.up;
			if(IfBetween((Angle)135,(Angle)225)) return Direction.left;
			if(IfBetween((Angle)225,(Angle)315)) return Direction.down;
			return 0;
		}
	}

	public static Angle Degree(float degree) {
		Angle result = new Angle();
		result.degree=degree;
		return result;
	}
	public static Angle Radian(float radian) {
		Angle result = new Angle();
		result.radian=radian;
		return result;
	}
	public static Angle Vector(Vector2 vector) {
		return new Angle(vector);
	}
	public static Angle FromQuaternion(Quaternion quaternion) {
		Angle result = new Angle();
		result.quaternion=quaternion;
		return result;
	}

	public Angle(Vector2 vector) {
		val=vector.y>0 ? Vector2.Angle(Vector2.right,vector) : 360-Vector2.Angle(Vector2.right,vector);
		Update();
	}
	public Angle(float deg) {
		val=deg;
		Update();
	}
	public Angle(Quaternion _quaternion) {
		val=0;
		quaternion=_quaternion;
		Update();
	}

	public static Angle right { get { return Degree(0); } }
	public static Angle up { get { return Degree(90); } }
	public static Angle left { get { return Degree(180); } }
	public static Angle down { get { return Degree(270); } }
	public static Angle epsilon { get { return Degree(Mathf.Epsilon); } }
	public static Angle random{ get{ return Degree(Random.Range(0,360)); } }

	private void Update() {
		while(val<0) val+=360;
		while(val>360) val-=360;
	}

	public float sin { get { return Mathf.Sin(radian); } }
	public float cos { get { return Mathf.Cos(radian); } }
	/// <summary>
	/// 判断一个角度是否在从a顺时针转到b的区间内
	/// </summary>
	public bool IfBetween(Angle a,Angle b) {
		float deg1 = a.degree;
		float deg2 = b.degree;
		float deg = degree;
		while(deg2<deg1) deg2+=360;
		while(deg<deg1) deg+=360;
		return deg<=deg2;
	}

	/*
	public static Vector2 operator *(Vector2 a,Angle b) {
		return Utility.Product(a,b.vector);
	}
	public static Vector2 operator *(Angle b,Vector2 a) {
		return Utility.Product(a,b.vector);
	}
	public static Vector2 operator /(Vector2 a,Angle b) {
		b.degree=-b.degree;
		return Utility.Product(a,b.vector);
	}
	*/

	public static Angle operator +(Angle left,Angle right) {
		float deg1 = left.degree;
		float deg2 = right.degree;
		return Degree(deg1+deg2);
	}
	public static Angle operator -(Angle left,Angle right) {
		float deg1 = left.degree;
		float deg2 = right.degree;
		return Degree(deg1-deg2);
	}

	public static bool operator ==(Angle left,Angle right) { return left.IfBetween(right-epsilon,right+epsilon); }
	public static bool operator !=(Angle left,Angle right) { return !(left==right);	}


	public static explicit operator float(Angle angle) { return angle.degree; }
	public static explicit operator Quaternion(Angle angle) { return angle.quaternion; }
	public static explicit operator Vector2(Angle angle) { return angle.vector; }

	public static explicit operator Angle(float degree) { return new Angle(degree); }
	public static explicit operator Angle(Quaternion quaternion) { return new Angle(quaternion); }
	public static explicit operator Angle(Vector2 vector) { return new Angle(vector); }

	public void RotateTowards(Angle target,float maxStep) {
		if(Mathf.Abs(target.degree-degree)<=maxStep) degree=target.degree;
		else if(target.IfBetween(this,this+left)) degree+=maxStep;
		else degree-=maxStep;
	}

}
