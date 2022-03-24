using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    public AnimationCurve animationCurve;
    public BoardGroup boardGroup;

    private float groupSize;
    private float margin = 1.1f;
    private Camera camera;

    private Board currentBoard;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        groupSize = Initializator.Instance.GroupSize;
        camera = Camera.main;
    }

    private void Update()
    {
        if (currentBoard == null) return;
        if (!currentBoard.IsDisabled) return;
        if (!GameManager.Instance.HideBoards) return;

        SetCameraToStartPosition();
    }

    public void SetCameraToStartPosition()
    {
        float columns = Mathf.Ceil(Mathf.Sqrt(groupSize));
        float rows = Mathf.Ceil(groupSize / columns);
        var (width, center) = boardGroup.GetGridWidthAndCenter(rows, columns);
        SetCameraPos(width, center);
    }

    public void MoveToBoard(Board board)
    {
        var size = board.GetBoardSize();
        var center = board.GetBoardCenter();
        SetCameraPos(size.horizontal, center);
        currentBoard = board;
    }

    private void SetCameraPos(float width, Vector3 center)
    {
        float maxExtent = width / 4;
        float minDistance = (maxExtent * margin) / Mathf.Sin(Mathf.Deg2Rad * camera.fieldOfView / 2.0f); ;
        var cameraPos = center - transform.forward * minDistance;

        LeanTween.cancel(gameObject);
        LeanTween.move(gameObject, cameraPos, 3f).setEase(animationCurve);
        LeanTween.delayedCall(1f, () => camera.nearClipPlane = minDistance - maxExtent);
    }
}