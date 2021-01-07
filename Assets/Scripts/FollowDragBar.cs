using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowDragBar : MonoBehaviour
{
    private GameObject dragbar;

    void Start()
    {
        dragbar = GameObject.Find("Dragbar");
    }

    void Update()
    {
        gameObject.transform.position = dragbar.transform.position - new Vector3(0, 0.2f, 0);
    }
}
