using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public Vector3 RotationSpeed;

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(RotationSpeed * Time.deltaTime);        
    }
}
