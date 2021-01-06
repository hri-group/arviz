using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelControl : MonoBehaviour
{
    private TFDisplay tfdisp;

    // Start is called before the first frame update
    void Start()
    {
        tfdisp = GetComponent<TFDisplay>();

        StartCoroutine(GetTree());
    }

    IEnumerator GetTree()
    {
        yield return new WaitForSeconds(1.5f);

        Debug.Log("Show: " + tfdisp.tf_tree[0]);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
