using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{

	public class TianChongYunJian : EntityFriendly
	{

		[field: SerializeField] public GameObject Thunder { get; protected set; }//序列化字段，让其暴露
		public int SkillDamage = 50;
		public float skillTime = 1.2f;
		public float Height = 20;
		float coolTime = 3;
		bool isSkill = false;
		float duringSkill = 0;
		Vector2 vec;
		DamageModel starDamage;


		protected override void Start()
		{
			base.Start();
			EntityBase.UpdateStats += EntityBase_UpdateStats;
			EntityBase.DamageEvent += EntityBase_DamageEvent;
			starDamage = GetDamage();
			starDamage.amount = 0;
			starDamage.knockback = GetDamage().knockback / 3;
			vec.x = 1;vec.y = 0;
		}

		private void EntityBase_DamageEvent(object sender, DamageModel e)
		{
			if (coolTime > skillTime && (sender as EntityBase).hp<=0 )
			{
				coolTime = 0;
				Vector2 position;
				position.x = this.transform.position.x+6; position.y = Height;
				starDamage.amount = 0;
				starDamage.knockback = 0;

				ProjectilePool.Create(Thunder, position, vec, this , true, starDamage);
				isSkill = true;
				duringSkill = 0.85f;

			}
		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			coolTime += Time.deltaTime;
			if (isSkill)
			{
				duringSkill -= Time.deltaTime;
				if (duringSkill <= 0)
				{
					isSkill = false;
					foreach (var i in entities)
					{
						if (i is EntityEnemy && (i.transform.position.x - this.transform.position.x < 12))
						{
							starDamage.amount = SkillDamage;
							starDamage.knockback = GetDamage().knockback / 3;
							i.Damage(starDamage);
						}
					}
				}
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			EntityBase.UpdateStats -= EntityBase_UpdateStats;
			EntityBase.DamageEvent -= EntityBase_DamageEvent;

		}
		private void EntityBase_UpdateStats(object _sender)
		{
			//判断是否响应
			EntityBase sender = _sender as EntityBase;
			if (sender != this) return;
			//准备相应
			//注意用+= 不要用*=或=

		}
	}

}