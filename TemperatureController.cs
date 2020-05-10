using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureController : MonoBehaviour {

	public float temperature = 0.3f;
	[HideInInspector] public string cachedMaterialTag;
	[HideInInspector] public Color cachedColor;

}
