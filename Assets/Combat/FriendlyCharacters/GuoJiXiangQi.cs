using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{

	public class GuoJiXiangQi : EntityFriendly
	{

		float times = 0;
		public float skillTimes;
		EntityBase Third;
		EntityFriendly temp;
		Vector2 position;
		protected override void Start()
		{
			base.Start();
			EntityBase.UpdateStats += EntityBase_UpdateStats;
		}

		
        protected override void FixedUpdate()
        {           
			base.FixedUpdate();
			times -= Time.deltaTime;
			if(times<0 && this.positionInTeam == 2 && Third)
            {
				
				this.positionInTeam = 1;
				(Third as EntityFriendly).positionInTeam = 2;
				
				position = this.transform.position;
				this.transform.position = Third.transform.position;
				Third.transform.position = position;
				
				friendlyList[this.positionInTeam] = this;
				friendlyList[(Third as EntityFriendly).positionInTeam] = Third as EntityFriendly;

			}
        }
        protected override void OnDestroy()
		{
			base.OnDestroy();
			EntityBase.UpdateStats -= EntityBase_UpdateStats;

		}
		private void EntityBase_UpdateStats(object _sender)
		{
			//判断是否响应
			EntityBase sender = _sender as EntityBase;
			if (sender != this) return;

			foreach(var i in entities)
            {
				if(i is EntityFriendly && (i as EntityFriendly).positionInTeam == 2)
                {
					Third = i;
					if ((float)Third.hp <= 0.3*(float)Third.maxHp)
                    {
						position = this.transform.position;
						this.transform.position = Third.transform.position;
						Third.transform.position = position;

						this.positionInTeam = 2;
						(Third as EntityFriendly).positionInTeam = 1;
						times = skillTimes;

						friendlyList[this.positionInTeam] = this;
						friendlyList[(Third as EntityFriendly).positionInTeam] = Third as EntityFriendly;
						
						Third.Heal((int)(0.2 * Third.maxHp));
						Debug.Log(Third.hp);
					}
                }
            }
			//准备相应
			//注意用+= 不要用*=或=7

		}
	}

}