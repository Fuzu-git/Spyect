using DG.Tweening;
using UnityEngine;

public class ChangeSize : MonoBehaviour
{
    public Vector3 end;
    public float lerpTime;

    // Start is called before the first frame update
    void Start()
    {
        end = transform.localScale;
        transform.localScale = Vector3.one;
        transform.DOScale(end, lerpTime).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
