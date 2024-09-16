using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; //�t�@�C������ׂ̈ɕK�v
using Newtonsoft.Json; //json���g���ׂɕK�v

public class RankingManager : MonoBehaviour
{
    [SerializeField]
    private bool _clearRankingData = false;
    public static RankingManager Instance;
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
    [System.Serializable] //���̃N���X���V���A���C�Y��(�ۑ��o������)�ɏo����
    public class PlayerScore
    {
        public string playerName;
        public float score;
    }
    private List<PlayerScore> rankingList = new List<PlayerScore>();
    public List<PlayerScore> RankingList => rankingList;
    private string filePath; //�ۑ���
    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "ranking.json"); //json�̕ۑ���t�@�C���p�X�����Ă�����
        if (_clearRankingData) ClearRankingData();
        LoadRanking(); //�N�����ɕۑ�����Ă���ranking�����[�h
    }
    public void AddScore(string playerName, float score)
    {
        PlayerScore newScore = new PlayerScore { playerName = playerName, score = score };
        rankingList.Add(newScore); //�ǉ����Ă�
        rankingList.Sort((a, b) => a.score.CompareTo(b.score)); //�����\�[�g
        if (rankingList.Count > 3)
        {
            rankingList.RemoveAt(3);
        }
        SaveRanking();
    }
    /// <summary>�����L���O�f�[�^�������\�b�h</summary>
    private void ClearRankingData()
    {
        Debug.Log("�����L���O�f�[�^����!!!!!");
        rankingList.Clear();
        List<PlayerScore> empty = new List<PlayerScore>();
        string json = JsonConvert.SerializeObject(empty, Formatting.Indented); //Formatting.Indented �� json�t�@�C���ɉ��s�Ƃ�����ēǂ݂₷�����Ă����
        File.WriteAllText(filePath, json);
        SaveRanking();
    }
    ///<summary> �����L���O�f�[�^�Z�[�u</summary>
    public void SaveRanking()
    {
        string jsonData = JsonConvert.SerializeObject(rankingList, Formatting.Indented); //���X�g��json�`���ɕϊ�����
        File.WriteAllText(filePath, jsonData); //�t�@�C����json�f�[�^����������
        Debug.Log("ranking���ۑ�����܂����I");
    }
    /// <summary> �����L���O�f�[�^�ǂݍ���</summary>
    public void LoadRanking()
    {
        if (!File.Exists(filePath)) return; //�t�@�C�������݂��Ȃ��ꍇ��return
        string jsonData = File.ReadAllText(filePath); //�ǂݍ���
        rankingList = JsonConvert.DeserializeObject<List<PlayerScore>>(jsonData); //Json�f�[�^�����X�g�ɕϊ�����rankingList�ɓ����
        Debug.Log("ranking�����[�h����܂����I");
    }
    /// <summary>�����L���O�f�[�^��n��</summary>
    /// <param name="rank"></param>
    /// <returns>�����L���O�f�[�^</returns>
    public PlayerScore GetRankingData(int rank)
    {
        return rankingList[rank];
    }
}
