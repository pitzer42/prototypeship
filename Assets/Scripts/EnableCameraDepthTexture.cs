using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class EnableCameraDepthTexture : MonoBehaviour {

  private Camera cam;

  void Start () {
    cam = GetComponent<Camera>();
    cam.depthTextureMode = DepthTextureMode.Depth;
  }

}
