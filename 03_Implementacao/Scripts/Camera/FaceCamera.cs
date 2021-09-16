using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    [SerializeField] private bool BillboardX = true;
    [SerializeField] private bool BillboardY = true;
    [SerializeField] private bool BillboardZ = true;
    [SerializeField] private float OffsetToCamera;
    [SerializeField] private Transform cameraPos;
    private Vector3 _localStartPosition;

    // Use this for initialization
    private void Start()
    {
        GameObject cam = GameObject.FindWithTag("MainCamera");
        cameraPos = cam.transform;
        gameObject.GetComponent<Canvas>().worldCamera = cam.GetComponent<Camera>();
        _localStartPosition = transform.localPosition;
    }

    // Update is called once per frame
    private void Update()
    {
        Quaternion rotation = cameraPos.rotation;
        transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
        if(!BillboardX || !BillboardY || !BillboardZ)
            transform.rotation = Quaternion.Euler(BillboardX ? transform.rotation.eulerAngles.x : 0f, BillboardY ? transform.rotation.eulerAngles.y : 0f, BillboardZ ? transform.rotation.eulerAngles.z : 0f);
        transform.localPosition = _localStartPosition;
        transform.position += transform.rotation * Vector3.forward * OffsetToCamera;
    }
}
