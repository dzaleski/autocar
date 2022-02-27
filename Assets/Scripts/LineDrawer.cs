using UnityEngine;

public class LineDrawer
{
    private LineRenderer _lineRenderer;
    private float _lineSize;

    public LineDrawer(Transform parent, float lineSize = 0.03f)
    {
        var lineObj = new GameObject("Sensor");
        lineObj.transform.parent = parent;
        _lineRenderer = lineObj.AddComponent<LineRenderer>();
        _lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));
        _lineSize = lineSize;
    }

    public void DrawLineInGameView(Vector3 start, Vector3 end, Color color)
    {
        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;

        _lineRenderer.startWidth = _lineSize;
        _lineRenderer.endWidth = _lineSize;

        _lineRenderer.positionCount = 2;

        _lineRenderer.SetPosition(0, start);
        _lineRenderer.SetPosition(1, end);
    }

    public void Destroy()
    {
        if (_lineRenderer != null)
        {
            Object.Destroy(_lineRenderer.gameObject);
        }
    }
}