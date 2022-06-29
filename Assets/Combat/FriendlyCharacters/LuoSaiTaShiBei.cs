using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{

	public class LuoSaiTaShiBei : EntityFriendly
	{

		int buffNumber = 1;
		public float addAttactPerBuff = 6.0f;
		string[] area= { "Europen","America","Asia" };
		public GameObject attPre;
       
		protected override void Start()
		{
			base.Start();
			EntityBase.UpdateStats += EntityBase_UpdateStats;

			for (int j = 0; j < 3; j++) {
				foreach (var i in entities)
				{
					if (i is EntityFriendly && i.tag == area[j])
				    {
						buffNumber += 1;
						Debug.Log(area[j]);
						break;
					}
				}
			}
			GameObject a = Instantiate(attPre, this.transform.position, Quaternion.identity);
			a.transform.parent = this.transform;//������Ч

		}

		protected override void OnDestroy() {
			EntityBase.UpdateStats-=EntityBase_UpdateStats;
			base.OnDestroy();
		}
		private void EntityBase_UpdateStats(object _sender)
		{
			//�ж��Ƿ���Ӧ
			EntityBase sender = _sender as EntityBase;
			if (sender != this) return;

			//׼����Ӧ
			//ע����+= ��Ҫ��*=��=
			powerBuff += (buffNumber * addAttactPerBuff)/attackBasePower;
		}
	}

}