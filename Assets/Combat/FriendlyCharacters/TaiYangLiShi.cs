using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{

	public class TaiYangLiShi : EntityFriendly
	{
      
		public float SpeedDebuff = 0.5f;
		public float SkillTime = 1.1f;
		public GameObject slowPre;
		object enermy;
		float times=0;
		bool isAnima;
		float animaTime = 0;
		protected override void Start()
		{
			base.Start();
			EntityBase.UpdateStats += EntityBase_UpdateStats;
            EntityBase.DamageEvent += EntityBase_DamageEvent;
		}

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
			times -= Time.deltaTime; //��ʱ��
			
			if (isAnima) //�Ŷ����Ĺ����У�animaTime���٣���¼�������˶��
				animaTime -= Time.deltaTime;
        }

        private void EntityBase_DamageEvent(object sender, DamageModel e)
        {
			enermy = sender ; //��¼���Ҵ����
			times = SkillTime; //��¼���ٵ�ʱ��
			isAnima = true; //��¼��ʼ�Ŷ���
		}

        protected override void OnDestroy() {
			EntityBase.UpdateStats-=EntityBase_UpdateStats;
			EntityBase.DamageEvent-=EntityBase_DamageEvent;
			base.OnDestroy();
		}

		private void EntityBase_UpdateStats(object _sender)
		{
			//�ж��Ƿ���Ӧ
			EntityBase sender = _sender as EntityBase;
			if (sender!=this) return;
			if (times > 0)
				(enermy as EntityBase).speedBuff -= SpeedDebuff;
			
			if(isAnima && animaTime <=0) //���ڷŶ�������animaTime����0����
            {
				animaTime = SkillTime; //����animaTime����Ϊ�˷�ֹ�ظ�ˢ����Ч��
				//���ż�����Ч
				GameObject a = Instantiate(slowPre, (enermy as EntityBase).transform.position, Quaternion.identity);
                a.transform.parent = (enermy as EntityBase).transform;
            }
        }
	}


}