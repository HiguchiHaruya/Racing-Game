using System.Collections.Generic;
using UnityEngine;

public class GhostRecorder : MonoBehaviour
{
    public static GhostRecorder Instance;
    ///  <summary>プレイヤー車の動きを記録するためのリスト</summary>
    private List<GhostFrame> _recordGhostFrames = new List<GhostFrame>();
    private bool _isRecording = true;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (_isRecording)
        {
            RecordFrame();
        }
    }
    /// <summary>
    /// frameに記録してリストに追加
    /// </summary>
    private void RecordFrame()
    {
        GhostFrame frame = new GhostFrame 
        {
            Position = transform.position,
            Rotation = transform.rotation
        };
        _recordGhostFrames.Add(frame);
    }
    /// <summary>
    /// 呼び出し元に_recordGhostFramesを返す
    /// </summary>
    /// <returns>プレイヤーの車の動きが入ってるリスト</returns>
    public List<GhostFrame> GetGhostData()
    {
        return new List<GhostFrame>(_recordGhostFrames); //_recordGhostFramesを新しいリストとしてコピーしてreturnする
    }

    public void StopRecording()
    {
        _isRecording = false;
    }

    public void StartRecording()
    {
        _recordGhostFrames.Clear();
        _isRecording = true;
    }
}

[System.Serializable]
public struct GhostFrame 
{
    public Vector3 Position;
    public Quaternion Rotation;
}
