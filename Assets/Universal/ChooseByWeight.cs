using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChooseByWeight {

	public delegate float FloatElementCallback(int a);
	public static int Work(FloatElementCallback list,int amount) {
		float total = 0;
		for(int i = 0;i<amount;i++) {
			total+=list(i);
		}
		float randomFactor = Random.Range(0,total);

		for(int i = 0;i<amount;i++) {
			randomFactor-=list(i);
			if(randomFactor<=0) return i;
		}
		return amount-1;

	}

}
