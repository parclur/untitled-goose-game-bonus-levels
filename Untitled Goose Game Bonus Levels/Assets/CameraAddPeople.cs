using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAddPeople : MonoBehaviour
{
    SphereCollider sphereCollider;
    public CinemachineTargetGroup targetGroup;

    // Start is called before the first frame update
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("On trigger enter");
        if (other.CompareTag("NPC"))
        {
            targetGroup.AddMember(other.gameObject.transform,1,0);
            Debug.Log("Add member");
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("On trigger exit");
        if (other.CompareTag("NPC"))
        {
            //targetGroup.RemoveMember(other.gameObject.transform);
            //Mathf.Lerp
            targetGroup.RemoveMember(other.gameObject.transform);
            Debug.Log("Remove member");
        }
    }
}
