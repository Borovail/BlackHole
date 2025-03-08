using Assets.Scripts;
using UnityEngine;

public class CameraScroll : Singleton<CameraScroll>
{
    public Camera Cam;
    public float ZoomSpeed = 5f;
    public float MinSize = 1f;
    public float CurrentMaxSize=10f;
    public float SecondStageMax = 120f;


    private void Start()
    {
        if (Cam == null)
            Cam = Camera.main;

        Cam.orthographicSize = 10;
    }

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;

        if (Input.GetKey(KeyCode.UpArrow))
            scroll += ZoomSpeed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.DownArrow))
            scroll -= ZoomSpeed * Time.deltaTime;

        if (scroll != 0)
        {
            Cam.orthographicSize -= scroll;
            Cam.orthographicSize = Mathf.Clamp(Cam.orthographicSize, MinSize, CurrentMaxSize);
        }
    }
}
