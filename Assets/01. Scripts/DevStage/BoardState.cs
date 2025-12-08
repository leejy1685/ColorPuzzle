using System;

public class BoardState : IEquatable<BoardState>
{
    public CellColor[,] Colors; 
    public int Depth; 
    public BoardState Parent; 
    public CellColor LastMoveColor; 

    // ★★★ 1. 해시 코드를 캐시할 필드 추가 ★★★
    private int _cachedHashCode =  int.MinValue;

    // 상태 비교를 위한 키 생성 (배열 순회를 한 번만 수행)
    private int GetArrayHashCode(CellColor[,] colors)
    {
        // BoardState.GetHashCode()가 처음 호출될 때 한 번만 계산합니다.
        if (_cachedHashCode != int.MinValue) return _cachedHashCode;
        
        // SolverHelpers의 상수를 사용 (Board 의존성 제거)
        const int Rows = SolverHelpers.Rows; 
        const int Cols = SolverHelpers.Cols;

        unchecked 
        {
            int hash = 17;
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    hash = hash * 23 + (int)colors[r, c]; 
                }
            }
            _cachedHashCode = hash;
            return hash;
        }
    }

    // ★★★ 2. GetHashCode 오버라이드 (캐시 사용) ★★★
    public override int GetHashCode() => GetArrayHashCode(this.Colors);
    
    // 3. Equals 구현 (문자열 비교 대신 배열 내용 직접 비교)
    public bool Equals(BoardState other) 
    {
        if (other == null) return false;
        
        // 빠른 실패: 해시 코드가 다르면 상태도 다름
        if (this.GetHashCode() != other.GetHashCode()) return false; 
        
        // 느린 비교: 해시 코드가 같으면 배열 내용 전체 비교
        return AreColorsEqual(this.Colors, other.Colors);
    }

    // 4. 배열 내용 비교 헬퍼 함수 (SolverHelpers.Rows/Cols 사용)
    private static bool AreColorsEqual(CellColor[,] c1, CellColor[,] c2)
    {
        const int Rows = SolverHelpers.Rows;
        const int Cols = SolverHelpers.Cols;
        
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Cols; c++)
            {
                if (c1[r, c] != c2[r, c]) return false;
            }
        }
        return true;
    }
}