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
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			EntityBase.UpdateStats -= EntityBase_UpdateStats;
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