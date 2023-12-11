using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Draw : MonoBehaviour
{
    [SerializeField] Camera Cam = null;
    [SerializeField] LineRenderer trailPrefab = null;
    [SerializeField] private Transform _parent;
    private LineRenderer currentTrail;
    private List<Vector3> points = new List<Vector3>();

    void Start()
    {
        if (!Cam)
        {
            Cam = Camera.main;
        }

        currentTrail = Instantiate(trailPrefab);
        currentTrail.transform.SetParent(transform, true);
    }

    void Update()
    {
        Touch[] touches = Input.touches;

        foreach (Touch touch in touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                CreateNewLine();
                AddPoint(touch.position);
            }
            if (touch.phase == TouchPhase.Moved)
            {
                AddPoint(touch.position);
            }
            if (touch.phase == TouchPhase.Ended)
            {
                UpdateLinePosition();
                int pointCount = currentTrail.positionCount;
                Vector3[] linePositions = new Vector3[pointCount];
                currentTrail.GetPositions(linePositions);
                Player.Singleton.ChangePositions(linePositions);
                points.Clear();
                currentTrail.positionCount = 0;
            }
        }

        // Обновляем позицию линии в каждом кадре
        UpdateLinePosition();
    }

    private void CreateNewLine()
    {
        points.Clear();

    }

    private void UpdateLinePosition()
    {
            for (int i = 0; i < currentTrail.positionCount; i++)
            {
                currentTrail.SetPosition(i,new Vector3(currentTrail.GetPosition(i).x, currentTrail.GetPosition(i).y, _parent.position.z-2.5f ));
            }
    }

    private void UpdateLinePoints()
    {
        if (currentTrail != null && points.Count > 1)
        {
            currentTrail.positionCount = points.Count;
            currentTrail.SetPositions(points.ToArray());
        }
    }

    private void AddPoint(Vector2 screenPosition)
    {
        var ray = Cam.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Writeable"))
            {
                Vector3 localPoint = currentTrail.transform.InverseTransformPoint(hit.point);
                points.Add(localPoint);
                points.Add(localPoint);
                points.Add(localPoint);
                UpdateLinePoints();
            }
        }
    }
}
