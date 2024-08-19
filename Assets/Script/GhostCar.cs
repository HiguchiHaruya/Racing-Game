using System.Collections.Generic;
using UnityEngine;

public class GhostCar : MonoBehaviour
{
    private List<GhostFrame> _ghostFrames;
    private int _currentFrameIndex = 0;

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
