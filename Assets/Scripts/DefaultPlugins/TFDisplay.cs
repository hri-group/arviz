using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TFDisplay : MonoBehaviour
{
    [SerializeField]
    GameObject tfDisplayPrefab;
    [SerializeField]
    GameObject arrowPrefab;
    TFListener tfListener;
    List<GameObject> publishedTFTree;
    List<GameObject> renderedTFFrames;
    List<GameObject> renderedArrows;
    Transform parentFrame;
    GameObject targetVisual;
    GameObject targetArrow;
    GameObject usedVisual;
    GameObject usedArrow;
    string visualSuffix = "_tf";
    string arrowSuffix = "_arrow";
    WaitForSeconds updateInterval = new WaitForSeconds(0.05f);
    RosSharp.RosBridgeClient.MessageTypes.Visualization.Marker arrowMarker;

    // Start is called before the first frame update
    void Start()
    {
        publishedTFTree = new List<GameObject>();
        renderedTFFrames = new List<GameObject>();
        renderedArrows = new List<GameObject>();
        tfListener = GameObject.Find("TFListener").GetComponent<TFListener>();
        StartCoroutine(TFFramesRender());
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
                        // Update name visual
                        targetVisual.transform.GetChild(1).GetComponent<TextMeshPro>().text = frame.name;
                        renderedTFFrames.Add(targetVisual);
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
                    if(targetArrow is null)
                    {
                        // Create new arrow at the origin of the TFTree if yet created
                        targetArrow = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity);
                        targetArrow.name = frame.name + arrowSuffix;
                        targetArrow.transform.parent = transform;
                        targetArrow.transform.localPosition = Vector3.zero;
                        targetArrow.transform.localRotation = Quaternion.identity;
                        targetArrow.GetComponent<ArrowManipulation>().SetColor(Color.yellow, Color.magenta);
                        renderedArrows.Add(targetArrow);
                    }
                    // Position Arrow
                    targetArrow.transform.parent = parentFrame;
                    targetArrow.GetComponent<ArrowManipulation>().SetArrow(frame.transform.localPosition, Vector3.zero);
                    targetArrow.transform.parent = transform;
                }
            }
            // Loop through to check for unused one
            foreach (GameObject visual in renderedTFFrames)
            {
                usedVisual = publishedTFTree.Find(res => res.name + visualSuffix == visual.name);
                if (usedVisual is null)
                {
                    Destroy(visual);
                }
            }
            renderedTFFrames.RemoveAll(visual => visual == null);
            foreach (GameObject arrow in renderedArrows)
            {
                usedArrow = publishedTFTree.Find(res => res.name + arrowSuffix == arrow.name);
                if (usedArrow is null)
                {
                    Destroy(arrow);
                }
            }
            renderedArrows.RemoveAll(arrow => arrow == null);
            yield return updateInterval;
        }
    }

    /*
    IEnumerator populateMenu()
    {
        float offset = -0.2263f;

        // wait for tree to be populated
        yield return new WaitForSeconds(2f);

        // sanity check
        while (tfTree.Count == 0)
        {
            yield return null;
        }

        for (int i = 0; i < tfTree.Count; i++)
        {
            var checkbox = Instantiate(checkbox_prefab, transform.position, transform.rotation);

            checkbox.transform.parent = GameObject.Find("TFMenuPanel").transform;
            checkbox.transform.localPosition = new UnityEngine.Vector3(-0.2364f, offset, -0.0172f);
            checkbox.transform.localRotation = UnityEngine.Quaternion.identity;

            checkbox.transform.Find("ButtonContent").transform.Find("Label").GetComponent<TextMesh>().text = tfTree[i].name;
            checkbox.name = tfTree[i].name + "_checkbox";

            // next checkbox offset lower
            offset -= 0.06f;

            // Scale size of backplate to match number of entries
            GameObject.Find("BackPlate").transform.position += new UnityEngine.Vector3(0f, -1f, 0) * 0.06f / 2;
            GameObject.Find("BackPlate").transform.localScale += new UnityEngine.Vector3(0f, 1f, 0) * 0.06f;
        }
    }
    */

}
