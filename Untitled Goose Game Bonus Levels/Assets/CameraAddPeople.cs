using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAddPeople : MonoBehaviour
{
    SphereCollider sphereCollider;
    public CinemachineTargetGroup _targetGroup;
    
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
            //targetGroup.AddMember(other.gameObject.transform,1,0);
            AddTarget(other.gameObject.transform);
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
            //targetGroup.RemoveMember(other.gameObject.transform);
            RemoveTarget(other.gameObject.transform);
            Debug.Log("Remove member");
        }
    }

    //[SerializeField]
    //CinemachineTargetGroup _targetGroup;

    [SerializeField]
    float _targetEase = 0.15f;

    public void AddTarget(Transform target)
    {
        if (_targetGroup != null)
        {
            if (_targetGroup.FindMember(target) == -1)
            {
                _targetGroup.AddMember(target, 0, 1);
                StartCoroutine(easeInMember(target));
            }
        }
    }

    IEnumerator easeInMember(Transform target)
    {
        int index = _targetGroup.FindMember(target);
        CinemachineTargetGroup.Target t = _targetGroup.m_Targets[index];
        while (t.weight < 0.66f)
        {
            t.weight = Mathf.MoveTowards(t.weight, 0.66f, _targetEase * Time.smoothDeltaTime);
            index = _targetGroup.FindMember(target);
            if (index >= 0)
            {
                _targetGroup.m_Targets[index] = t;
            }
            yield return new WaitForSeconds(0.01f);
        }
        t.weight = 0.66f;
    }

    public void RemoveTarget(Transform target)
    {
        if (_targetGroup != null)
        {
            if (_targetGroup.FindMember(target) != -1)
            {
                StartCoroutine(easeOutMember(target));
            }
        }
    }

    IEnumerator easeOutMember(Transform target)
    {
        int index = _targetGroup.FindMember(target);
        CinemachineTargetGroup.Target t = _targetGroup.m_Targets[index];
        while (t.weight > 0f)
        {
            t.weight = Mathf.MoveTowards(t.weight, 0, _targetEase * Time.smoothDeltaTime);
            index = _targetGroup.FindMember(target);
            if (index >= 0)
            {
                _targetGroup.m_Targets[index] = t;
            }
            yield return new WaitForSeconds(0.01f);
        }
        t.weight = 0;
        _targetGroup.RemoveMember(target);
        //target.gameObject.SetActive(false);
    }

}
