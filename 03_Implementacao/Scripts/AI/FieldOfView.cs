using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FieldOfView : MonoBehaviour
{
    [SerializeField] public float viewRadius;
    [SerializeField] [Range(0, 360)] public float viewAngle;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private Transform rayOrigin;
    [HideInInspector] public List<Transform> visibleTargets = new List<Transform>();

    private void Start()
    {
        StartCoroutine(nameof(FindTargetsWithDelay), 0.1f);
    }

    private IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    private void FindVisibleTargets()
    {
        visibleTargets.Clear();

        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        foreach (Collider t in targetsInViewRadius)
        {
            Transform target = t.transform;
            Vector3 targetPos = target.position + Vector3.up * 1.4f;
            Vector3 dirToTarget = (targetPos - rayOrigin.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(rayOrigin.position, dirToTarget, distToTarget, obstacleMask))
                    visibleTargets.Add(target);
            }
        }
    }

    public Vector3 DirFromAngle(float angle, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
            angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}