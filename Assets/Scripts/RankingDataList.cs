using System;

public class RankDataList
{
    public RankData[] dataList;
}

[System.Serializable]
public class RankData : IComparable
{
    public string name;
    public int score;


    public RankData(string name, int score)
    {
        this.name = name;
        this.score = score;
    }

    public RankData()
    {
        name = "NoName";
        score = 0;
    }

    public int CompareTo(object obj)
    {
        RankData other = obj as RankData;
        return other.score.CompareTo(score);
    }
}
