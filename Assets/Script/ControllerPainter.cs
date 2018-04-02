using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Es.InkPainter.Sample
{
    public class ControllerPainter : MonoBehaviour
    {
        /// <summary>
        /// Types of methods used to paint.
        /// </summary>
        /// 

        private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
        private SteamVR_TrackedObject controller;
        private SteamVR_Controller.Device device;

        private bool isHold = false;

        [System.Serializable]
        private enum UseMethodType
        {
            RaycastHitInfo,
            WorldPoint,
            NearestSurfacePoint,
            DirectUV,
        }

        [SerializeField]
        private Brush brush;

        [SerializeField]
        private UseMethodType useMethodType = UseMethodType.RaycastHitInfo;

        [SerializeField]
        bool erase = false;


        private void Start()
        {
            controller = GetComponent<SteamVR_TrackedObject>();
        }

        private void Update()
        {
            //var device = SteamVR_Controller.Input(8);
            //Debug.Log("IsPressed = " + device.GetPress(SteamVR_Controller.ButtonMask.Trigger));

            device = SteamVR_Controller.Input((int)controller.index);

            if (device.GetPressDown(triggerButton))
            {
                isHold = true;
            }

            if (device.GetPressUp(triggerButton))
            {
                isHold = false;
            }

            if (isHold)
            {
                var mousePos = Input.mousePosition;
                mousePos.z = 0f;
                //var ray = Camera.main.ScreenPointToRay(mousePos);
                Ray ray = new Ray(transform.position, transform.forward);


                Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
                bool success = true;
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    var paintObject = hitInfo.transform.GetComponent<InkCanvas>();
                    if (paintObject != null)
                        switch (useMethodType)
                        {
                            case UseMethodType.RaycastHitInfo:
                                success = erase ? paintObject.Erase(brush, hitInfo) : paintObject.Paint(brush, hitInfo);
                                break;

                            case UseMethodType.WorldPoint:
                                success = erase ? paintObject.Erase(brush, hitInfo.point) : paintObject.Paint(brush, hitInfo.point);
                                break;

                            case UseMethodType.NearestSurfacePoint:
                                success = erase ? paintObject.EraseNearestTriangleSurface(brush, hitInfo.point) : paintObject.PaintNearestTriangleSurface(brush, hitInfo.point);
                                break;

                            case UseMethodType.DirectUV:
                                if (!(hitInfo.collider is MeshCollider))
                                    Debug.LogWarning("Raycast may be unexpected if you do not use MeshCollider.");
                                success = erase ? paintObject.EraseUVDirect(brush, hitInfo.textureCoord) : paintObject.PaintUVDirect(brush, hitInfo.textureCoord);
                                break;
                        }
                    if (!success)
                        Debug.LogError("Failed to paint.");
                }
            }
        }

        public void OnGUI()
        {
            if (GUILayout.Button("Reset"))
            {
                foreach (var canvas in FindObjectsOfType<InkCanvas>())
                    canvas.ResetPaint();
            }
        }
    }
}