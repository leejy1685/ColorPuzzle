using Firebase.Analytics;

public class ClearInfo
{
    private int _stageId;
    private float _clearTime;
    private int _usedChance;
    private int _retryCount;

    public ClearInfo(int stageId, float clearTime, int usedChance, int retryCount)
    {
        _stageId = stageId;
        _clearTime = clearTime;
        _usedChance = usedChance;
        _retryCount = retryCount;
    }
    
    public Parameter[] Parameters => new Parameter[]
    {
        new Parameter("StageId", _stageId),
        new Parameter("ClearTime", _clearTime),
        new Parameter("ChanceUsed", _usedChance),
        new Parameter("TryCount", _retryCount)
    };
    
}
