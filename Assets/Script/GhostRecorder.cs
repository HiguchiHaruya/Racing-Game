using System.Collections.Generic;
using UnityEngine;

public class GhostRecorder : MonoBehaviour
{
    private List<GhostFrame> _ghostFrames = new List<GhostFrame>();
    private bool _isRecording = true;

    void Update()
    {
        if (_isRecording)
        {
            RecordFrame();
        }
    }

    private void RecordFrame()
    {
        GhostFrame frame = new GhostFrame
        {
            Position = transform.position,
            Rotation = transform.rotation
        };
        _ghostFrames.Add(frame);
    }

    public List<GhostFrame> GetGhostData()
    {
        return new List<GhostFrame>(_ghostFrames);
    }

    public void StopRecording()
    {
        _isRecording = false;
    }

    public void StartRecording()
    {
        _ghostFrames.Clear();
        _isRecording = true;
    }
}

[System.Serializable]
public struct GhostFrame
{
    public Vector3 Position;
    public Quaternion Rotation;
}
