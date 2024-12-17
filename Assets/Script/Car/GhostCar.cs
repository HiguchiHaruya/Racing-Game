using System.Collections.Generic;
using UnityEngine;

public class GhostCar : MonoBehaviour
{
    public static GhostCar Instance;
    private List<GhostFrame> _ghostFrames;
    private int _currentFrameIndex = 0;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// ゴースト車クラスのリストに記録した動きを入れる
    /// </summary>
    /// <param name="ghostFrames"></param>
    public void SetGhostData(List<GhostFrame> ghostFrames)
    {
        _ghostFrames = ghostFrames;
        _currentFrameIndex = 0;
    }

    void Update()
    {
        if (_ghostFrames == null || _currentFrameIndex >= _ghostFrames.Count) return;
        
        GhostFrame frame = _ghostFrames[_currentFrameIndex];
        transform.position = frame.Position;
        transform.rotation = frame.Rotation;

        _currentFrameIndex++;
    }
}
