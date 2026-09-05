using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeatPacking : Minigame
{
    public override string Name => "Pack";

    [SerializeField]
    private LineRenderer line;
    [SerializeField]
    private LineRenderer targetLine;
    [SerializeField]
    private LayerMask canvasLayerMask;
    [SerializeField]
    private float minLineSegmentLengthSqr;
    [SerializeField]
    private float maxDistanceFromTargetSqr;
    [SerializeField]
    private float maxDistanceFromVertexSqr;
    [SerializeField]
    private Sprite cursorSprite;
    [SerializeField]
    private Vector2 cursorHotspot;

    [Header("Tip")]
    [SerializeField]
    private GameObject tableHint;

    private bool pressed = false;
    private HashSet<Vector3> completedVertices = new HashSet<Vector3>();

    private static Texture2D scaledCursor;

    protected override void Start()
    {
        base.Start();
        
        Reset();
        targetLine.gameObject.SetActive(false);
        line.gameObject.SetActive(false);

        if (scaledCursor == null)
        {
            scaledCursor = ScaleTexture(cursorSprite.texture, 32, 32);
        }
    }

    public override void StartMinigame()
    {
        base.StartMinigame();

        InputAdapter.look.performed += OnMouseMove;
        InputAdapter.interact.started += OnMousePress;
        InputAdapter.interact.canceled += OnMouseRelease;

        targetLine.gameObject.SetActive(true);
        line.gameObject.SetActive(true);
        tableHint.gameObject.SetActive(true);

        Reset();

        Cursor.SetCursor(scaledCursor, cursorHotspot, CursorMode.Auto);
    }

    public override void EndMinigame(bool win)
    {
        base.EndMinigame(win);

        InputAdapter.look.performed -= OnMouseMove;
        InputAdapter.interact.started -= OnMousePress;
        InputAdapter.interact.canceled -= OnMouseRelease;

        Reset();

        targetLine.gameObject.SetActive(false);
        line.gameObject.SetActive(false);
        tableHint.gameObject.SetActive(false);

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    private void OnMouseMove(InputAction.CallbackContext context)
    {
        if (pressed)
        {
            Vector3 cameraPos = Mouse.current.position.ReadValue();
            cameraPos.z = 20;
            Vector3 raycastTarget = Camera.main.ScreenToWorldPoint(cameraPos);
            Vector3 raycastRoot = Camera.main.transform.position;

            if (Physics.Raycast(raycastRoot, raycastTarget - raycastRoot, out RaycastHit hitInfo, 20, canvasLayerMask))
            {
                Vector3 hitInLocalSpace = line.transform.InverseTransformPoint(hitInfo.point);
                if (line.positionCount > 0 && 
                    (line.GetPosition(line.positionCount - 1) - hitInLocalSpace).sqrMagnitude < minLineSegmentLengthSqr)
                {
                    return;
                }

                float distance = DistanceToLineRenderer(hitInLocalSpace, targetLine, out Vector3 _, out Vector3 vertex);
                //Debug.Log($"Distance to line: {distance}, position: {hitInLocalSpace}, closestVertex: {vertex}");
                if (distance > maxDistanceFromTargetSqr)
                {
                    Reset();
                    return;
                }

                if (line.positionCount > 0)
                {
                    if (!completedVertices.Contains(vertex) &&
                        (vertex - hitInLocalSpace).sqrMagnitude < maxDistanceFromVertexSqr)
                    {
                        completedVertices.Add(vertex);
                        Debug.Log($"Vertex completed: {vertex}. Total: {completedVertices.Count}");
                    }
                }
                
                line.positionCount++;
                line.SetPosition(line.positionCount - 1, hitInLocalSpace);

                if (completedVertices.Count == targetLine.positionCount && 
                    (line.GetPosition(0) - line.GetPosition(line.positionCount - 1)).sqrMagnitude < maxDistanceFromTargetSqr)
                {
                    EndMinigame(true);
                }
            }
        }
    }

    private void OnMouseRelease(InputAction.CallbackContext context)
    {
        pressed = false;
        Reset();
    }

    private void OnMousePress(InputAction.CallbackContext context)
    {
        pressed = true;
    }

    private void Reset()
    {
        line.positionCount = 0;
        completedVertices.Clear();
    }

    /// <summary>
    /// Returns the shortest distance from a point to the closest point 
    /// on any segment of the LineRenderer's polyline.
    /// </summary>
    public static float DistanceToLineRenderer(Vector3 point, LineRenderer line, out Vector3 closestPoint, out Vector3 closestVertex)
    {
        int count = line.positionCount;
        closestPoint = Vector3.zero;
        closestVertex = Vector3.zero;
        float minDistSqr = float.MaxValue;

        if (count == 0)
        {
            return float.MaxValue;
        }

        if (count == 1)
        {
            closestPoint = line.GetPosition(0);
            closestVertex = line.GetPosition(0);
            return Vector3.Distance(point, point);
        }

        Vector3 a, b, candidate;
        float distSqr;
        bool aCloser;
        for (int i = 0; i < count - 1; i++)
        {
            a = line.GetPosition(i);
            b = line.GetPosition(i + 1);

            candidate = ClosestPointOnSegment(point, a, b, out aCloser);
            distSqr = (point - candidate).sqrMagnitude;

            if (distSqr < minDistSqr)
            {
                minDistSqr = distSqr;
                closestPoint = candidate;
                closestVertex = aCloser ? a : b;
            }
        }

        a = line.GetPosition(line.positionCount - 1);
        b = line.GetPosition(0);

        candidate = ClosestPointOnSegment(point, a, b, out aCloser);
        distSqr = (point - candidate).sqrMagnitude;

        if (distSqr < minDistSqr)
        {
            minDistSqr = distSqr;
            closestPoint = candidate;
            closestVertex = aCloser ? a : b;
        }

        return minDistSqr;
    }

    /// <summary>
    /// Returns the closest point on segment ab to point p.
    /// </summary>
    public static Vector3 ClosestPointOnSegment(Vector3 p, Vector3 a, Vector3 b, out bool aCloser)
    {
        Vector3 ab = b - a;
        float sqrLen = ab.sqrMagnitude;
        aCloser = true;

        // Degenerate segment (a == b)
        if (sqrLen < Mathf.Epsilon)
            return a;

        float t = Vector3.Dot(p - a, ab) / sqrLen;
        t = Mathf.Clamp01(t); // clamp to the segment, not the infinite line
        aCloser = t < 0.5f;

        return a + ab * t;
    }
}
