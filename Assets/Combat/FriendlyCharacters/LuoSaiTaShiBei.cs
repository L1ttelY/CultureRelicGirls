using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class LuoSaiTaShiBei:EntityFriendly {



		int buffNumber = 0;
		public float addAttactPerBuff = 6.0f;
		string[] area = { "Europen","America","Asia" };
		public GameObject attPre;

		[SerializeField] AudioClip buffSound;

		protected override void Start() {
			base.Start();
			EntityBase.UpdateStats+=EntityBase_UpdateStats;


		}

		bool inited;
		protected override void Update() {
			base.Update();

			if(!inited) {
				for(int j = 0;j<3;j++) {
					foreach(var i in friendlyList) {
						Debug.Log($"{name} : {area[j]} -> {i.gameObject.tag}");
						if(i.gameObject.tag==area[j]) {
							buffNumber+=1;
							break;
						}
					}
				}
				Debug.Log(buffNumber);

				if(buffNumber>0) {
					AudioController.PlayAudio(buffSound,transform.position);
					GameObject a = Instantiate(attPre,this.transform.position,Quaternion.identity);
					a.transform.parent=this.transform;//������Ч

				}
				inited=true;
			}
		}

		protected override void OnDestroy() {
			EntityBase.UpdateStats-=EntityBase_UpdateStats;
			base.OnDestroy();
		}
		private void EntityBase_UpdateStats(object _sender) {
			//�ж��Ƿ���Ӧ
			EntityFriendly sender = _sender as EntityFriendly;
			if(sender==null) return;

			//׼����Ӧ
			//ע����+= ��Ҫ��*=��=
			if(buffNumber==1) sender.powerBuff+=0.05f;
			else if(buffNumber==2) sender.powerBuff+=0.25f;

		}
	}

}