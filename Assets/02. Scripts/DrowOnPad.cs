using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrowOnPad : MonoBehaviour
{
    public LineRenderer drawedLine;
    public float inputSensitivity = 0.01f;
    public float lineWidth = 5f;

    RectTransform canvas;    

    private LineRenderer curLine;
    private List<LineRenderer> lines = new();
    private List<Vector3> points = new List<Vector3>();

    Vector3[] corners = new Vector3[4];
    Vector3 bottomLeft;
    Vector3 topRight;

    private void Awake()
    {
        canvas = GetComponent<RectTransform>();

        canvas.GetWorldCorners(corners);
        bottomLeft = corners[0];
        topRight = corners[2];
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 입력 시작
            StartDrawing();
        }
        else if(Input.GetMouseButton(0))
        {
            // 마우스 누르는 중
            Draw();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            // 마우스 뗐을 때
            EndDrawing();
        }
    }

    void StartDrawing()
    {
        Vector3 mousePos = Input.mousePosition;
        // 거리 조절
        mousePos.z = canvas.position.z - 5;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        // 스케치패드 범위를 나갔을 경우 return
        if (!IsOnSketchpad(worldPos)) return;

        // 새 라인 생성
        curLine = Instantiate(drawedLine);
        // 부모와 회전 맞추기
        curLine.transform.SetParent(canvas, true);
        curLine.startWidth = lineWidth;
        curLine.endWidth = lineWidth;
        curLine.positionCount = 0;
        points.Clear();
        lines.Add(curLine);
    }

    void Draw()
    {
        // 마우스 위치->타겟 이미지 위의 로컬 좌표로 변환
        Vector3 mousePos = Input.mousePosition;
        // 거리 조절
        mousePos.z = canvas.position.z - 5;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        if (!IsOnSketchpad(worldPos)) return;

        // 현재 저장된 점이 없거나 마지막 점으로부터 일정 이상 떨어진 거리에 찍혔을 경우에만 그리기
        if (points.Count == 0 ||
            Vector2.Distance(points[points.Count - 1], worldPos) > inputSensitivity
            ) 
        {
            if (curLine == null) StartDrawing();

            points.Add(worldPos);
            curLine.positionCount = points.Count;
            curLine.SetPositions(points.ToArray());
        }
    }

    void EndDrawing()
    {
        curLine = null;
    }

    bool IsOnSketchpad(Vector3 inputPos)
    {
        return bottomLeft.x <= inputPos.x
            && topRight.x >= inputPos.x
            && bottomLeft.y <= inputPos.y
            && topRight.y >= inputPos.y;
    }

    public void SaveImage()
    {
        

    }
}
