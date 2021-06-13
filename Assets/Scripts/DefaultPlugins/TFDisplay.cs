using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TFDisplay : MonoBehaviour
{
    [SerializeField]
    GameObject tfDisplayPrefab;
    [SerializeField]
    GameObject arrowPrefab;
    [SerializeField]
    GameObject checkboxPrefab;
    [SerializeField]
    GameObject refMenuPanel;
    TFListener tfListener;
    List<GameObject> publishedTFTree = new List<GameObject>();
    List<GameObject> renderedTFFrames = new List<GameObject>();
    List<GameObject> renderedArrows = new List<GameObject>();
    List<GameObject> checkboxList = new List<GameObject>();
    Transform parentFrame;
    GameObject targetVisual;
    GameObject targetArrow;
    GameObject usedVisual;
    GameObject usedArrow;
    
    string visualSuffix = "_tf";
    string arrowSuffix = "_arrow";
    WaitForSeconds updateInterval = new WaitForSeconds(0.05f);
    WaitForSeconds updateMenu = new WaitForSeconds(5f);

    private void OnEnable()
    {
        tfListener = GameObject.Find("TFListener").GetComponent<TFListener>();
        StartCoroutine(TFFramesRender());
        StartCoroutine(PopulateTFMenu());
    }
    IEnumerator TFFramesRender()
    {
        while (true)
        {
            publishedTFTree = tfListener.GetTFTree();
            if (publishedTFTree is null)
            {
                Debug.LogWarning("TFTree is updating or not yet instantiated");
            }
            else
            {
                foreach (GameObject frame in publishedTFTree)
                {
                    if (frame != null)
                    {
                        // TF Visual and name
                        // Look up if the frame has already been made
                        targetVisual = renderedTFFrames.Find(res => res.name == frame.name + visualSuffix);
                        if (targetVisual is null)
                        {
                            // Create new frame at the origin of the TFTree if yet created
                            targetVisual = Instantiate(tfDisplayPrefab, Vector3.zero, Quaternion.identity);
                            targetVisual.name = frame.name + visualSuffix;
                            targetVisual.transform.parent = transform;
                            targetVisual.transform.localPosition = Vector3.zero;
                            targetVisual.transform.localRotation = Quaternion.identity;
                            targetVisual.tag = "TF";
                            // Update name visual
                            targetVisual.transform.GetChild(1).GetComponent<TextMeshPro>().text = frame.name;
                            renderedTFFrames.Add(targetVisual);
                        }
                        // Look up if the frame is active
                        if (!targetVisual.activeInHierarchy)
                        {
                            targetVisual.SetActive(true);
                        }
                        // Place the visual properly
                        parentFrame = frame.transform.parent;
                        targetVisual.transform.parent = parentFrame;
                        targetVisual.transform.localPosition = frame.transform.localPosition;
                        targetVisual.transform.localRotation = frame.transform.localRotation;
                        targetVisual.transform.parent = transform;
                        // Arrow
                        // Look up if the arrow has already been made
                        targetArrow = renderedArrows.Find(res => res.name == frame.name + arrowSuffix);
                        if (targetArrow is null)
                        {
                            // Create new arrow at the origin of the TFTree if yet created
                            targetArrow = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity);
                            targetArrow.name = frame.name + arrowSuffix;
                            targetArrow.transform.parent = transform;
                            targetArrow.transform.localPosition = Vector3.zero;
                            targetArrow.transform.localRotation = Quaternion.identity;
                            targetArrow.tag = "TFArrow";
                            targetArrow.GetComponent<ArrowManipulation>().SetColor(Color.yellow, Color.magenta);
                            renderedArrows.Add(targetArrow);
                        }
                        // Look up if the arrow is active
                        if (!targetArrow.activeInHierarchy)
                        {
                            targetArrow.SetActive(true);
                        }
                        // Position Arrow
                        targetArrow.transform.parent = parentFrame;
                        targetArrow.GetComponent<ArrowManipulation>().SetArrow(frame.transform.localPosition, Vector3.zero);
                        targetArrow.transform.parent = transform;
                    }
                }
            }
            // Loop through to check for unused one
            foreach (GameObject visual in renderedTFFrames)
            {
                usedVisual = publishedTFTree.Find(res => res.name + visualSuffix == visual.name);
                if (usedVisual is null)
                {
                    visual.SetActive(false);
                }
            }
            // renderedTFFrames.RemoveAll(visual => visual == null);
            foreach (GameObject arrow in renderedArrows)
            {

                usedArrow = publishedTFTree.Find(res => res.name + arrowSuffix == arrow.name);
                if (usedArrow is null)
                {
                    arrow.SetActive(false);
                }
            }
            // renderedArrows.RemoveAll(arrow => arrow == null);
            yield return updateInterval;
        }
    }


    IEnumerator PopulateTFMenu()
    {
        while (true)
        {

            float offset = -0.2263f;

            // wait for tree to be populated
            yield return new WaitForSeconds(2f);
            
            foreach (GameObject createdCheckbox in checkboxList)
            {
                Destroy(createdCheckbox);
                // Scale size of backplate to match number of entries
                refMenuPanel.transform.Find("BackPlate").transform.position -= new UnityEngine.Vector3(0f, -1f, 0) * 0.06f / 2;
                refMenuPanel.transform.Find("BackPlate").transform.localScale -= new UnityEngine.Vector3(0f, 1f, 0) * 0.06f;
            }
            checkboxList.Clear();
            for (int i = 0; i < publishedTFTree.Count; i++)
            {
                var checkbox = Instantiate(checkboxPrefab, transform.position, transform.rotation);

                checkbox.transform.parent = refMenuPanel.transform;
                checkbox.transform.localPosition = new UnityEngine.Vector3(-0.2364f, offset, -0.0172f);
                checkbox.transform.localRotation = UnityEngine.Quaternion.identity;
                checkbox.GetComponent<Interactable>().AddReceiver<TFButtonReceiver>();
                checkbox.transform.Find("ButtonContent").transform.Find("Label").GetComponent<TextMesh>().text = publishedTFTree[i].name;
                checkbox.name = publishedTFTree[i].name + "_checkbox";
                
                if (renderedTFFrames.Find(res => res.name == publishedTFTree[i].name + visualSuffix).transform.GetChild(0).gameObject.activeInHierarchy)
                {
                    checkbox.GetComponent<Interactable>().IsToggled = true;
                }
                
                checkboxList.Add(checkbox);
                // next checkbox offset lower
                offset -= 0.06f;

                // Scale size of backplate to match number of entries
                refMenuPanel.transform.Find("BackPlate").transform.position += new UnityEngine.Vector3(0f, -1f, 0) * 0.06f / 2;
                refMenuPanel.transform.Find("BackPlate").transform.localScale += new UnityEngine.Vector3(0f, 1f, 0) * 0.06f;
            }
            yield return updateMenu;
        }
    }
}

