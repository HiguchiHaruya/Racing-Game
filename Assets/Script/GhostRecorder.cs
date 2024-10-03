using System.Collections.Generic;
using UnityEngine;

public class GhostRecorder : MonoBehaviour
{
    public static GhostRecorder Instance;
    ///  <summary>�v���C���[�Ԃ̓������L�^���邽�߂̃��X�g</summary>
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
    /// frame�ɋL�^���ă��X�g�ɒǉ�
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
    /// �Ăяo������_recordGhostFrames��Ԃ�
    /// </summary>
    /// <returns>�v���C���[�̎Ԃ̓����������Ă郊�X�g</returns>
    public List<GhostFrame> GetGhostData()
    {
        return new List<GhostFrame>(_recordGhostFrames); //_recordGhostFrames��V�������X�g�Ƃ��ăR�s�[����return����
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
