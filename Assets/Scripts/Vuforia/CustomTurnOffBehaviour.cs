/*===============================================================================
Copyright (c) 2017-2020 PTC Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other
countries.
===============================================================================*/
using UnityEngine;
using Vuforia;

public class CustomTurnOffBehaviour : MonoBehaviour
{
    #region PUBLIC_MEMBERS
    public enum TurnOffRendering
    {
        PlayModeAndDevice,
        PlayModeOnly,
        Neither
    }

    public TurnOffRendering turnOffRendering = TurnOffRendering.PlayModeAndDevice;
    #endregion //PUBLIC_MEMBERS

    private bool DestroyTrackableBehaviourMeshAndRenderer
    {
        get
        {
            // Mesh and Renderer will be destroyed if "PlayModeAndDevice" is selected or if we're running in PlayMode
            // and only if the "Neither" option isn't set. Setting "Neither" will keep the Mesh and Renderer.
            return
                (turnOffRendering != TurnOffRendering.Neither &&
                ((turnOffRendering == TurnOffRendering.PlayModeAndDevice) || Application.isEditor)
                );
        }
    }

    #region UNITY_MONOBEHAVIOUR_METHODS

    void Awake()
    {
        if (VuforiaRuntimeUtilities.IsVuforiaEnabled() && DestroyTrackableBehaviourMeshAndRenderer)
        {
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            MeshFilter meshFilter = GetComponent<MeshFilter>();

            if (meshRenderer)
                Destroy(meshRenderer);
            if (meshFilter)
                Destroy(meshFilter);
        }
    }

    #endregion // UNITY_MONOBEHAVIOUR_METHODS
}