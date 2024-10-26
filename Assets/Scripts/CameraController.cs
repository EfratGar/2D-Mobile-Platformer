using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Room camera
    [SerializeField] private float speed;
    private float currentPositionX;
    private Vector3 velocity = Vector3.zero;

    //Follow character
    [SerializeField] private Transform character;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    private float lookAhead;


    private void Update()
    {
        //Room camera
        //transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPositionX, transform.position.y, transform.position.z), ref velocity, speed);

        //Follow character
        transform.position = new Vector3(character.position.x + lookAhead, transform.position.y, transform.position.z);
        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * character.localScale.x), Time.deltaTime * cameraSpeed);
    }

    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPositionX = _newRoom.position.x;
    }


}
