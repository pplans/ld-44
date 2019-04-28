using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class PostProcess : MonoBehaviour
{
	Camera AttachedCamera;
	public Shader DrawSimple;
	public Shader Post_Outline;
	Camera TempCam;
	Material Post_Mat;

	Material OutlineMaskMat;
	private RenderTexture TempRT = null;

	private CommandBuffer m_outlineCommandBuffer;

	void Start()
	{
		AttachedCamera = GetComponent<Camera>();
		TempCam = new GameObject().AddComponent<Camera>();
		TempCam.enabled = false;
		Post_Mat = new Material(Post_Outline);
		OutlineMaskMat = new Material(DrawSimple);
	}

	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		//set up a temporary camera
		/*TempCam.CopyFrom(AttachedCamera);
		TempCam.clearFlags = CameraClearFlags.Color;
		TempCam.backgroundColor = Color.black;

		//cull any layer that isn't the outline
		// Voodoo, without temporary variable, the value is -1...
		int OutlineLayerMask = 1 << LayerMask.NameToLayer("Outline");
		TempCam.cullingMask = OutlineLayerMask;

		//make the temporary rendertexture
		if (TempRT==null)
			TempRT = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.R8);

		//put it to video memory
		TempRT.Create();

		//set the camera's target texture when rendering
		TempCam.targetTexture = TempRT;

		//render all objects this camera can render, but with our custom shader.
		TempCam.RenderWithShader(DrawSimple, "");
		Post_Mat.SetTexture("_SceneTex", source);
		//copy the temporary RT to the final image
		Graphics.Blit(TempRT, destination, Post_Mat);

		//release the temporary RT
		TempRT.Release();*/

		//
		// create new command buffer
		/*m_outlineCommandBuffer = new CommandBuffer();
		m_outlineCommandBuffer.name = "Outline map buffer";

		// create render texture for glow map
		int tempID = Shader.PropertyToID("_Temp1");
		m_outlineCommandBuffer.GetTemporaryRT(tempID, -1, -1, 24, FilterMode.Bilinear);
		// add command to draw stuff to this texture
		m_outlineCommandBuffer.SetRenderTarget(tempID);

		// clear render texture before drawing to it each frame!!
		m_outlineCommandBuffer.ClearRenderTarget(true, true, Color.black);
		// draw all glow objects to the render texture
		foreach (GameObject o in OutlineManager.instance.m_GlowObjs)
		{
			Renderer r = o.GetComponent<Renderer>();
			if (r)
				m_outlineCommandBuffer.DrawRenderer(r, OutlineMaskMat);
		}// set render texture as globally accessible 'glow map' texture
		m_outlineCommandBuffer.SetGlobalTexture("_OutlineMap", tempID);

		// add this command buffer to the pipeline
		AttachedCamera.AddCommandBuffer(CameraEvent.BeforeLighting, m_outlineCommandBuffer);*/
	}

}