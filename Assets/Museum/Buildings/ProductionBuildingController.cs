using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerData;

namespace Museum {

	public class ProductionBuildingController:BuildingControllerBase {

		[SerializeField] float[] productionPerMinute;
		[SerializeField] int[] capacity;
		[SerializeField] bool targetSentienceMatter;

		[SerializeField] SpriteRenderer displaySpriteRenderer;

		DataInt targetData;

		System.TimeSpan FullProductionTime(int level) {
			float totalMinutes = capacity[level]/productionPerMinute[level];
			return new System.TimeSpan((long)(System.TimeSpan.TicksPerMinute*(double)totalMinutes));
		}

		public override void OnClick(CameraFocus.CancelFocus cancelFocus) {

			if(saveData.extraProgression.progressionAmount>0.1f&&saveData.extraStatus.value>0) {

				targetData.value+=Mathf.FloorToInt(saveData.extraProgression.progressionAmount*capacity[saveData.level.value]);
				saveData.extraProgression.SetProgression(FullProductionTime(saveData.level.value),0);
				cancelFocus.doCancel=true;
				spriteRenderer.material=normalMaterial;

			} else base.OnClick(cancelFocus);
		}

		protected override void Start() {
			base.Start();
			targetData=targetSentienceMatter ? PlayerDataRoot.instance.sentienceMatter : PlayerDataRoot.instance.printingMaterial;
		}

		protected override void FixedUpdate() {
			base.FixedUpdate();

			if(saveData.levelUpStatus.value!=0||saveData.level.value<=0) {
				saveData.extraStatus.value=0;
			} else {
				if(saveData.extraStatus.value==0) {
					saveData.extraStatus.value=1;
					saveData.extraProgression.SetProgression(FullProductionTime(saveData.level.value),0);
				}
			}

			if(saveData.extraStatus.value==0) {
				displaySpriteRenderer.color=Color.clear;
			} else {
				displaySpriteRenderer.color=new Color(1,1,1,saveData.extraProgression.progressionAmount);
			}

		}

	}

}