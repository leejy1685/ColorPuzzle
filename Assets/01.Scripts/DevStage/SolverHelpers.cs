using System.Collections.Generic;

// 2. 헬퍼 함수 (Flood Fill, 인접 색상 등)
public static class SolverHelpers
{
    public const int Rows = 8; 
    public const int Cols = 10;
    
    // 인접 방향 (제공된 _direction 배열과 동일)
    private static readonly int[] dr = { 1, -1, 0, 0 };
    private static readonly int[] dc = { 0, 0, 1, -1 };
    
    // BFS Flood Fill: 상태 배열(CellColor[,])을 직접 변경
    public static void FloodFillState(CellColor[,] colors, CellColor newColor)
    {
        int startR = 0;
        int startC = 0;
        CellColor currentFloodColor = colors[startR, startC]; // 현재 플러드 영역의 색상 (ColD)

        // 1. 선택된 색상이 현재 플러드 영역의 색상과 같으면 아무 일도 일어나지 않음
        if (currentFloodColor == newColor) return; 

        var queue = new Queue<(int r, int c)>();
        queue.Enqueue((startR, startC));

        // 시작 셀의 색상을 새 색상으로 즉시 변경합니다.
        colors[startR, startC] = newColor; 
        
        while (queue.Count > 0)
        {
            var (r, c) = queue.Dequeue();

            for (int i = 0; i < dr.Length; i++)
            {
                int nr = r + dr[i];
                int nc = c + dc[i];

                // 2. 경계 확인 및 '이전 플러드 영역의 색상'과 같은지 확인
                if (nr >= 0 && nr < Rows && nc >= 0 && nc < Cols && 
                    colors[nr, nc] == currentFloodColor) // 
                {
                    // 이전 플러드 영역의 셀을 새 색상으로 칠하고 큐에 추가
                    colors[nr, nc] = newColor; 
                    queue.Enqueue((nr, nc));
                }
            }
        }
    }

    // 플러드 영역에 속한 셀 좌표들을 반환 (인접 색상 탐색을 위한 보조 함수)
    public static HashSet<(int r, int c)> GetFloodRegionCells(CellColor[,] colors)
    {
        // (0, 0)의 현재 색상을 기준으로 Flood Fill을 수행하여 영역을 식별합니다.
        var region = new HashSet<(int r, int c)>();
        var queue = new Queue<(int r, int c)>();
    
        // 시작 셀의 색상
        CellColor floodColor = colors[0, 0];
    
        queue.Enqueue((0, 0));
        region.Add((0, 0)); // 시작 셀 포함

        while (queue.Count > 0)
        {
            var (r, c) = queue.Dequeue();
        
            for (int i = 0; i < dr.Length; i++)
            {
                int nr = r + dr[i];
                int nc = c + dc[i];

                // 1. 경계 확인
                if (nr >= 0 && nr < Rows && nc >= 0 && nc < Cols)
                {
                    // 2. 색상 일치 확인 && 3. 이미 영역에 포함되었는지 확인
                    if (colors[nr, nc] == floodColor && !region.Contains((nr, nc)))
                    {
                        region.Add((nr, nc));
                        queue.Enqueue((nr, nc));
                    }
                }
            }
        }
        return region;
    }
    
    // 플러드 영역과 인접한 유효한 다음 이동 색상 목록을 반환
    public static HashSet<CellColor> GetAdjacentColors(CellColor[,] colors)
    {
        var adjColors = new HashSet<CellColor>();
        var floodRegion = GetFloodRegionCells(colors);
        CellColor floodColor = colors[0, 0];
        
        foreach (var (r, c) in floodRegion)
        {
            for (int i = 0; i < dr.Length; i++)
            {
                int nr = r + dr[i];
                int nc = c + dc[i];

                if (nr >= 0 && nr < Rows && nc >= 0 && nc < Cols)
                {
                    CellColor neighborColor = colors[nr, nc];
                    if (neighborColor != floodColor && neighborColor != CellColor.None)
                    {
                        adjColors.Add(neighborColor);
                    }
                }
            }
        }
        return adjColors;
    }
    
    // 목표 상태 확인 (보드 전체가 단색인지)
    public static bool IsBoardMonochromatic(CellColor[,] colors)
    {
        CellColor firstColor = colors[0, 0];
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Cols; c++)
            {
                if (colors[r, c] != firstColor) return false;
            }
        }
        return true;
    }
}