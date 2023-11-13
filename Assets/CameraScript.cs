using UnityEngine;

public class CameraScript : MonoBehaviour
{

    private float dist;
    private Vector3 MouseStart;
    private float zoomSpeed = 0.5f;


    void Update()
    {
        //zoom in and out
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0 && Camera.main.orthographicSize > 1.4 || scroll < 0 && Camera.main.orthographicSize < 50)
        {
            Camera.main.orthographicSize -= scroll * zoomSpeed * Camera.main.orthographicSize;
        }


        if (Input.GetMouseButtonDown(2))
        {
            MouseStart = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
            MouseStart = Camera.main.ScreenToWorldPoint(MouseStart);
            MouseStart.z = transform.position.z;

        }
        else if (Input.GetMouseButton(2))
        {
            var MouseMove = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
            MouseMove = Camera.main.ScreenToWorldPoint(MouseMove);
            MouseMove.z = transform.position.z;
            transform.position = transform.position - (MouseMove - MouseStart);
        }
    }

}



