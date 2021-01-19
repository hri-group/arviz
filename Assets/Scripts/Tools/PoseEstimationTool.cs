using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class PoseEstimationTool : BaseInputHandler, IMixedRealityPointerHandler
{
    public GameObject PrefabToSpawn;
    const int POSITION_STATE = 0;
    const int ORIENTATION_STATE = 1;
    private int state = 0;
    private GameObject arrow_clone;

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        switch (state)
        {
            case POSITION_STATE:
                if (PrefabToSpawn != null)
                {
                    var result = eventData.Pointer.Result;
                    arrow_clone = Instantiate(PrefabToSpawn, Vector3.zero, Quaternion.identity);
                    arrow_clone.GetComponent<ArrowManipulation>().SetArrow(Vector3.zero, Vector3.forward);
                    arrow_clone.transform.localPosition = result.Details.Point;
                    arrow_clone.transform.parent = transform;
                    // Using BoundingBox to perform rotation on the arrow
                    // However, this would make the arrow's centre of rotation at the middle of the arrow, not the tail of the arrow
                    // TODO: look for a way to make it 
                    arrow_clone.AddComponent<BoundingBox>();
                    arrow_clone.GetComponent<BoundingBox>().ShowRotationHandleForX = false;
                    arrow_clone.GetComponent<BoundingBox>().ShowRotationHandleForY = true;
                    arrow_clone.GetComponent<BoundingBox>().ShowRotationHandleForZ = false;
                    Debug.Log("Im placed");
                }
                state = ORIENTATION_STATE;
                break;
            case ORIENTATION_STATE:
                Destroy(arrow_clone);
                state = POSITION_STATE;
                break;
        }
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {

    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
        
    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
    }

    protected override void RegisterHandlers()
    {
    }

    protected override void UnregisterHandlers()
    {
    }
}
