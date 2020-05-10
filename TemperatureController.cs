using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureController : MonoBehaviour {

	[Tooltip("Temperature of this object relative to environment.\nBecause of edge darkening, 0 actually looks slightly cooler than the environment.\nAlso note that objects with this script use a different thermal vision replacement shader.")]
	public float temperature = 0.08f;

	[HideInInspector] public string cachedMaterialTag;
	[HideInInspector] public Color cachedColor;

}
