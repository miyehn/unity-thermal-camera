using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[ExecuteInEditMode]
public class ThermalCameraScript : MonoBehaviour {

	public Shader TVPostProcessing;
	public Shader TVSurfaceReplacement;

	public Color coolColor;
	public Color midColor;
	public Color warmColor;

	public float environmentTemperature = 0.4f;

	bool TV = false; // Thermal Vision
	Camera cam;

	Material TVPostProcessingMaterial = null;
	Material SkyboxMaterialCached = null;
	Material SkyboxMaterialReplacement = null;

	void Awake() {
		Assert.IsTrue(TVPostProcessing != null);
		Assert.IsTrue(TVSurfaceReplacement != null);
		TVPostProcessingMaterial = new Material(TVPostProcessing);
		SkyboxMaterialCached = RenderSettings.skybox;
		SkyboxMaterialReplacement = new Material(TVSurfaceReplacement);
		Assert.IsTrue(SkyboxMaterialCached != null);

		cam = GetComponent<Camera>();
		Assert.IsTrue(cam != null);
	}

	void Update() {
		if (Input.GetKeyDown("space")) {
			TV = !TV;

			if (TV) {
				RenderSettings.skybox = SkyboxMaterialReplacement;
				cam.SetReplacementShader(TVSurfaceReplacement, "RenderType");

			} else {
				RenderSettings.skybox = SkyboxMaterialCached;
				cam.ResetReplacementShader();
			}
		}
	}

	void OnRenderImage(RenderTexture src, RenderTexture dst) {

		Shader.SetGlobalFloat("_EnvironmentTemperature", environmentTemperature);

		if (false && TV) {
			TVPostProcessingMaterial.SetColor("coolColor", coolColor);
			TVPostProcessingMaterial.SetColor("midColor", midColor);
			TVPostProcessingMaterial.SetColor("warmColor", warmColor);
			Graphics.Blit(src, dst, TVPostProcessingMaterial);
		} else {
			Graphics.Blit(src, dst);
		}
	}

}
