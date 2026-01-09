using System.Collections.Generic;
using UnityEngine;

public class BFSSolver
{
    // 최저 횟수만 out 매개변수 (int)로 반환하는 TrySolve 패턴
    public bool TrySolve(CellColor[,] initialColors, CellColor targetColor, out int minMoves)
    {
        // minMoves 초기화 (경로 찾기 실패 시 값)
        minMoves = -1;
        
        // 1. 초기 상태 설정
        var startState = new BoardState
        {
            Colors = (CellColor[,])initialColors.Clone(),
            Depth = 0
        };
        
        var queue = new Queue<BoardState>(); 
        queue.Enqueue(startState);
        
        var visited = new HashSet<BoardState>(); 
        visited.Add(startState);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            
            // 2. 목표 도달 확인
            // 보드 전체가 단색이고 그 색상이 목표 색상과 같으면 성공
            if (SolverHelpers.IsBoardMonochromatic(current.Colors) && current.Colors[0, 0] == targetColor)
            {
                // ★★★ out 매개변수에 최저 횟수(Depth) 할당 ★★★
                minMoves = current.Depth;
                return true; // 성공 시 true 반환
            }
            
            // 3. 다음 상태 생성
            var nextMoveColors = SolverHelpers.GetAdjacentColors(current.Colors); 
            
            if (current.Colors[0, 0] != targetColor)
            {
                // 보드 전체 셀 개수
                const int TotalCells = SolverHelpers.Rows * SolverHelpers.Cols;
            
                // 목표 색상으로 칠했을 때 플러드 영역이 보드 전체를 덮을 수 있는지 체크
                var tempColors = (CellColor[,])current.Colors.Clone();
                SolverHelpers.FloodFillState(tempColors, targetColor);
            
                var finalRegion = SolverHelpers.GetFloodRegionCells(tempColors);
            
                if (finalRegion.Count == TotalCells)
                {
                    // 목표 색상으로 마지막 이동이 가능하다면 후보에 추가 (중복 방지)
                    nextMoveColors.Add(targetColor);
                }
            }
            
            foreach (var color in nextMoveColors)
            {
                // 상태 복제
                var nextColorsArray = (CellColor[,])current.Colors.Clone();
                
                // Flood Fill로 상태 변경
                SolverHelpers.FloodFillState(nextColorsArray, color);
                
                var nextState = new BoardState 
                {
                    Colors = nextColorsArray,
                    Depth = current.Depth + 1,
                    Parent = current, // Parent는 경로 복원에 필요 없으나 구조 유지를 위해 남김
                    LastMoveColor = color 
                };
                
                // 4. 이미 방문하지 않은 상태일 경우에만 큐에 추가
                if (!visited.Contains(nextState))
                {
                    visited.Add(nextState);
                    queue.Enqueue(nextState);
                }
            }
        }
        
        // 5. 해답을 찾지 못한 경우
        minMoves = -1; 
        return false; // 실패 시 false 반환
    }
}
