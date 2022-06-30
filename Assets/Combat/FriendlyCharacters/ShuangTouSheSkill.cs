using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuangTouSheSkill:MonoBehaviour {
	public int buff;
	int everBuff = 0;
	int whatIsCreating = 0;
	public Sprite[] skillScripts;
	public GameObject prefab;

	GameObject[] anima = new GameObject[5];
	float animaTime = 0;
	bool isCreating = false;
	Vector2 position;
	[field: SerializeField] public GameObject Thunder { get; protected set; }
	void Start() {
	}

	void creat() {
		position.x=this.transform.position.x-0.6f;
		position.y=this.transform.position.y+0.38f-((float)(everBuff))*0.2f;
		anima[everBuff]=Instantiate(prefab,position,Quaternion.identity);
		anima[everBuff].transform.parent=this.transform;
		anima[everBuff].GetComponent<SpriteRenderer>().sprite=skillScripts[0];
		whatIsCreating=everBuff;
		animaTime=0.5f;
		isCreating=true;
	}


	private void FixedUpdate() {
		if(isCreating)
			animaTime-=Time.deltaTime;
	}

	// Update is called once per frame
	void Update() {
		//Debug.Log(buff);
		if(animaTime<=0&&isCreating) //生成新东西
		{
			anima[whatIsCreating].GetComponent<SpriteRenderer>().sprite=skillScripts[1];
			isCreating=false;
		}

		if(buff<everBuff)//掉buff了
		{
			whatIsCreating--;
			Destroy(anima[buff]);//掉一层显示
			Debug.Log(123);
		} else if(buff>everBuff) {
			creat(); //加一层buff
		}
		everBuff=buff;
	}
}
