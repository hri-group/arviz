using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;

public class ArvizUtils : MonoBehaviour
{
    private GameObject pointerPos;
    [SerializeField]
    private Vector3 ARMarkerTransform;
    // Start is called before the first frame update
    void Start()
    {
        pointerPos = new GameObject("pointerPos");
        CoreServices.InputSystem?.FocusProvider?.SubscribeToPrimaryPointerChanged(OnPrimaryPointerChanged, true);
    }
    private void OnDestroy()
    {
        Destroy(pointerPos);
    }
    private void OnPrimaryPointerChanged(IMixedRealityPointer oldPointer, IMixedRealityPointer newPointer)
    {
        if (pointerPos != null)
        {
            if (newPointer != null)
            {
                Transform parentTransform = newPointer.BaseCursor?.GameObjectReference?.transform;

                // If there's no cursor try using the controller pointer transform instead
                if (parentTransform == null)
                {
                    var controllerPointer = newPointer as BaseControllerPointer;
                    parentTransform = controllerPointer?.transform;
                }

                if (parentTransform != null)
                {
                    pointerPos.transform.SetParent(parentTransform, false);
                    pointerPos.SetActive(true);
                    pointerPos.transform.position = parentTransform.position;
                    return;
                }
            }
            pointerPos.SetActive(false);
            pointerPos.transform.SetParent(null, false);
        }
    }
    // One-shot recalibration
    public void Calibrate()
    {
        GameObject.Find("ImageTarget").GetComponent<CustomDefaultTrackableEventHandler>().recalibrate = true;
        GameObject.Find("ImageTarget").GetComponent<CustomDefaultTrackableEventHandler>().reActivate();
    }
    public void CancelTool()
    {

    }
}
