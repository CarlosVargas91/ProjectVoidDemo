using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFullCameraController : MonoBehaviour
{
    private float startSize;
    private Camera cam;
    [SerializeField] MapCameraController mapCam;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float maxZoom;
    [SerializeField] private float minZoom;
    [SerializeField] private float moveModifier = 1f;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();

        startSize = cam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f).normalized * cam.orthographicSize * Time.unscaledDeltaTime * moveModifier;
        if (Input.GetKey(KeyCode.E) || Input.GetButton("Fire1"))
        {
            cam.orthographicSize -= zoomSpeed * Time.unscaledDeltaTime;
        }

        if (Input.GetKey(KeyCode.Q) || Input.GetButton("Action"))
        {
            cam.orthographicSize += zoomSpeed * Time.unscaledDeltaTime;
        }

        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
    }

    private void OnEnable()
    {
        transform.position = mapCam.transform.position;
    }
}
