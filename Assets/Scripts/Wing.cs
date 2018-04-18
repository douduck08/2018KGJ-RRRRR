using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wing : MonoBehaviour {

	void Update () {
		this.transform.Rotate (0f, 360f * Time.deltaTime, 0f);
	}
}
