/*==============================================================================
Copyright (c) 2020 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using Vuforia;

/// <summary>
/// A custom handler that registers to the TrackableBehaviour callbacks
///
/// Changes made to this file could be overwritten when upgrading the Vuforia version.
/// When implementing custom event handler behavior, consider inheriting from this class instead.
/// </summary>
public class TrackableStatusEventHandler : DefaultTrackableEventHandler
{
    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour.StatusInfo m_PreviousStatusInfo;
    protected TrackableBehaviour.StatusInfo m_NewStatusInfo;

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region PRIVATE_MEMBER_VARIABLES

    private bool TrackingStatusIsTracked
    {
        get
        {
            return
                (this.m_NewStatus == TrackableBehaviour.Status.DETECTED ||
                 this.m_NewStatus == TrackableBehaviour.Status.TRACKED ||
                 this.m_NewStatus == TrackableBehaviour.Status.EXTENDED_TRACKED);
        }
    }

    private bool TrackingStatusIsNotTracked
    {
        get
        {
            return
                this.m_PreviousStatus == TrackableBehaviour.Status.TRACKED &&
                (this.m_NewStatus == TrackableBehaviour.Status.NO_POSE ||
                 this.m_NewStatus == TrackableBehaviour.Status.LIMITED);
        }
    }

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region UNITY_MONOBEHAVIOUR_METHODS

    protected virtual new void Start()
    {
        this.mTrackableBehaviour = GetComponent<TrackableBehaviour>();

        if (this.mTrackableBehaviour)
        {
            this.mTrackableBehaviour.RegisterOnTrackableStatusChanged(OnTrackableStatusChanged);
            this.mTrackableBehaviour.RegisterOnTrackableStatusInfoChanged(OnTrackableStatusInfoChanged);
        }
    }

    protected virtual new void OnDestroy()
    {
        if (this.mTrackableBehaviour)
        {
            this.mTrackableBehaviour.UnregisterOnTrackableStatusChanged(OnTrackableStatusChanged);
            this.mTrackableBehaviour.UnregisterOnTrackableStatusInfoChanged(OnTrackableStatusInfoChanged);
        }
    }

    #endregion // UNITY_MONOBEHAVIOUR_METHODS


    #region PRIVATE_METHODS

    void OnTrackableStatusChanged(TrackableBehaviour.StatusChangeResult statusChangeResult)
    {
        this.m_PreviousStatus = statusChangeResult.PreviousStatus;
        this.m_NewStatus = statusChangeResult.NewStatus;

        Debug.LogFormat("Trackable {0} {1} -- {2}",
            mTrackableBehaviour.TrackableName,
            mTrackableBehaviour.CurrentStatus,
            mTrackableBehaviour.CurrentStatusInfo);

        HandleTrackableStatusChanged();
    }

    void OnTrackableStatusInfoChanged(TrackableBehaviour.StatusInfoChangeResult statusInfoChangeResult)
    {
        this.m_PreviousStatusInfo = statusInfoChangeResult.PreviousStatusInfo;
        this.m_NewStatusInfo = statusInfoChangeResult.NewStatusInfo;

        Debug.LogFormat("Trackable {0} {1} -- {2}",
            mTrackableBehaviour.TrackableName,
            mTrackableBehaviour.CurrentStatus,
            mTrackableBehaviour.CurrentStatusInfo);

        HandleTrackableStatusChanged();
    }

    #endregion // PRIVATE_METHODS


    #region PROTECTED_METHODS

    protected new void HandleTrackableStatusChanged()
    {
        if (this.TrackingStatusIsTracked)
        {
            OnTrackingFound();
        }
        else if (this.TrackingStatusIsNotTracked)
        {
            OnTrackingLost();
        }
        else
        {
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    #endregion //PROTECTED_METHODS

}
