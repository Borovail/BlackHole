using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    public Camera cam;
    public float zoomSpeed = 5f;
    public float minSize = 1f;
    public float maxSize = 120f;

    void Start()
    {
        if (cam == null)
            cam = Camera.main;

        cam.orthographicSize = 10;
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        if (Input.GetKey(KeyCode.UpArrow))
            scroll += zoomSpeed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.DownArrow))
            scroll -= zoomSpeed * Time.deltaTime;

        if (scroll != 0)
        {
            cam.orthographicSize -= scroll;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minSize, maxSize);
        }
    }
}
