using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraFollow : MonoBehaviour
{
    public Transform objFollow;
    public Vector3 offset;

    private void LateUpdate()
    {
        if (!objFollow)
            return;
        transform.position = objFollow.position + offset;
    }

    public void SetObjToFollow(Transform transformTofollow)
    {
        objFollow = transformTofollow;
    }
}
