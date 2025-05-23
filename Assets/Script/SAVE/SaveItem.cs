using System;

[Serializable]
public class SavedItem
{
    public string part;      // ItemPartType 이름
    public int itemId;    // 데이터베이스 조회용 ID
    public int level;     // 강화 레벨
    public string statType;  // stat 종류 문자열
}
