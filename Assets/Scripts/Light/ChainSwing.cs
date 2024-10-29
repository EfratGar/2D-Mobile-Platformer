using UnityEngine;

public class ChainSwing : MonoBehaviour
{
    [SerializeField] private float swingAngle = 15f;
    [SerializeField] private float swingSpeed = 2f;

    private void Update()
    {
        float angle = Mathf.Sin(Time.time * swingSpeed) * swingAngle; 
        transform.rotation = Quaternion.Euler(0, 0, angle); 
    }
}
