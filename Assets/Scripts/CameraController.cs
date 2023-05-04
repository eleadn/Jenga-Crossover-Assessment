using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI infoBox;
    [SerializeField]
    float distanceFromFocus;
    [SerializeField]
    float rotationSpeed;
    [SerializeField]
    float rotationThreshold;

    Vector3 focus;

    float yRotation;
    float xRotation;

    public void SetFocus(Vector3 newFocus)
    {
        focus = newFocus;
        transform.position = new Vector3(newFocus.x, newFocus.y, -distanceFromFocus);
        transform.LookAt(newFocus);
        transform.RotateAround(focus, Vector3.up, xRotation);
        transform.RotateAround(focus, transform.right, yRotation);
    }

    void Rotate()
    {
        Vector2 speed = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y")).normalized * rotationSpeed * Time.deltaTime;

        if (yRotation + speed.y > rotationThreshold)
            speed.y = rotationThreshold - yRotation;
        else if (yRotation + speed.y < -rotationThreshold)
            speed.y = -rotationThreshold - yRotation;

        transform.RotateAround(focus, Vector3.up, speed.x);
        transform.RotateAround(focus, transform.right, speed.y);

        yRotation += speed.y;
        xRotation += speed.x;
    }

    void OpenInfoBox()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(mouseRay, out hit))
        {
            GameObject collider = hit.collider.gameObject;
            if (collider.layer != LayerMask.NameToLayer("JengaPiece"))
                return;
            int index = collider.transform.GetSiblingIndex() - 1;
            infoBox.transform.parent.gameObject.SetActive(true);
            infoBox.text = collider.transform.parent.GetComponent<Tower>().GetInfos(index);
            infoBox.transform.parent.position = Input.mousePosition;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
            Rotate();
        if (Input.GetMouseButtonDown(1))
            OpenInfoBox();
    }
}
