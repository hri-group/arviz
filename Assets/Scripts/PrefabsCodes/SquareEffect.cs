using UnityEngine;

public class SquareEffect : MonoBehaviour
{
    [SerializeField]
    private bool squareEffect = false;
    void LateUpdate()
    {
        if (squareEffect)
        {
            transform.LookAt(Camera.main.transform);
        }
    }
    public void SetSquareEffect(bool arg)
    {
        squareEffect = arg;
    }
}
