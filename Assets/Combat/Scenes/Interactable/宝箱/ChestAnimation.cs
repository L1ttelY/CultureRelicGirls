using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {
  public class ChestAnimation:MonoBehaviour {

    [SerializeField] Sprite[] spriteSequence;
    [SerializeField] float timePerFrame;
    [SerializeField] string message;

    int imageIndex;
    bool started;


		private void Update() {
			//if(!started)
		}

	}
}