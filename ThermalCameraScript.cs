using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

public class ThermalCameraScript : MonoBehaviour {

	[Header("Resource References")]
	public Shader TVPostProcessing;
	public Shader TVSurfaceReplacement; // cool replacement material

	[Header("Palette")]
	public int useTexture = 1;
	public Texture2D paletteTex;
	public Color coolColor;
	public Color midColor;
	public Color warmColor;

	[Header("Parameters")]
	public float environmentTemperature = 0.2f;

	//--------------------------------
	
	bool TV = false; // Thermal Vision
	Camera cam;

	Material TVPostProcessingMaterial = null;
	Material SkyboxMaterialCached = null;
	Material SkyboxMaterialReplacement = null;

	void Awake() {
		SkyboxMaterialCached = RenderSettings.skybox;
		TVPostProcessingMaterial = new Material(TVPostProcessing);
		SkyboxMaterialReplacement = new Material(TVSurfaceReplacement);

		cam = GetComponent<Camera>();
	}

	void Update() {
		List<TemperatureController> TCs = GetAllTemperatureControllers();

		if (Input.GetKeyDown("space")) {
			TV = !TV;

			if (TV) {
				// replace skybox material (since replacement shade doesn't seem to affect it)
				RenderSettings.skybox = SkyboxMaterialReplacement;

				// replace material tags and color for objects with explicit temperature control
				foreach (TemperatureController TC in TCs) {
					Renderer R = TC.gameObject.GetComponent<Renderer>();
					if (R==null) continue;
					TC.cachedMaterialTag = R.material.GetTag("RenderType", false);
					TC.cachedColor = R.material.color;
					TC.gameObject.GetComponent<Renderer>().material.SetOverrideTag("RenderType", "Temperature");
				}

				// everything else
				cam.SetReplacementShader(TVSurfaceReplacement, "RenderType");

			} else {
				// restore skybox material
				RenderSettings.skybox = SkyboxMaterialCached;

				// restore temperature-controlled object tags and color
				foreach (TemperatureController TC in TCs) {
					Renderer R = TC.gameObject.GetComponent<Renderer>();
					if (R==null) continue;
					TC.gameObject.GetComponent<Renderer>().material.SetOverrideTag("RenderType", TC.cachedMaterialTag);
					TC.gameObject.GetComponent<Renderer>().material.color = TC.cachedColor;
				}

				// everything else
				cam.ResetReplacementShader();
			}
		}

		if (TV) {
			foreach (TemperatureController TC in TCs) {
				TC.gameObject.GetComponent<Renderer>().material.color = new Color(TC.temperature, 0, 0, 0);
			}
		}
	}

	void OnRenderImage(RenderTexture src, RenderTexture dst) {

		Shader.SetGlobalFloat("_EnvironmentTemperature", environmentTemperature);

		if (TV) {
			if (useTexture==1 || useTexture==2) {
				TVPostProcessingMaterial.SetInt("useTexture", useTexture);
				TVPostProcessingMaterial.SetTexture("_PaletteTex", paletteTex);

			} else {
				TVPostProcessingMaterial.SetInt("useTexture", 0);
				TVPostProcessingMaterial.SetColor("coolColor", coolColor);
				TVPostProcessingMaterial.SetColor("midColor", midColor);
				TVPostProcessingMaterial.SetColor("warmColor", warmColor);
			}
			Graphics.Blit(src, dst, TVPostProcessingMaterial);

		} else {
			Graphics.Blit(src, dst);
		}
	}
	
	List<TemperatureController> GetAllTemperatureControllers() {
		List<TemperatureController> TCs = new List<TemperatureController>();
		foreach(TemperatureController TC in Resources.FindObjectsOfTypeAll(typeof(TemperatureController)) as TemperatureController[]) {
			if (!EditorUtility.IsPersistent(TC.transform.root.gameObject) && 
					!(TC.hideFlags == HideFlags.NotEditable || TC.hideFlags == HideFlags.HideAndDontSave)) {
				TCs.Add(TC);
			}
		}
		return TCs;
	}

}
