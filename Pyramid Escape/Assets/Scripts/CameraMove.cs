using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private GameObject target;
    // Update is called once per frame
    void Update()
    {
        var position = target.transform.position;
        gameObject.transform.position = new Vector3(position.x, position.y, -10);
    }
}
