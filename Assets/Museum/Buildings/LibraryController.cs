using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {
	public class LibraryController:BuildingWithCharacterInteractionBase {

		public static LibraryController instance { get; private set; }
		protected override void Start() {
			base.Start();
			instance=this;
		}

	}

}