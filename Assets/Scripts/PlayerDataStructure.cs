[System.Serializable]
public class PlayerDataStructure {

    public string SavedText;
    public string CurScene;

    //public int Level;
    //public int Exp;
    public int SkillPoint;

    public int ShamLV;
    public int ShamEXP;
    public int PassiveLV;
    public int PassiveEXP;
    public int RebelLV;
    public int RebelEXP;
    public int SelfishLV;
    public int SelfishEXP;
    public int EvilLV;
    public int EvilEXP;

    public PlayerDataStructure(
        string curScene,
        int skillPoint,
        string savedText,
    int shamLV,
    int shamEXP,
    int passiveLV,
    int passiveEXP,
    int rebelLV,
    int rebelEXP,
    int selfishLV,
    int selfishEXP,
    int evilLV,
    int evilEXP
        )
    {

        this.CurScene = curScene;

        this.SkillPoint = skillPoint;
        this.SavedText = savedText;

        this.ShamLV = shamLV;
        this.ShamEXP = shamEXP;
        this.PassiveLV = passiveLV;
        this.PassiveEXP = passiveEXP;
        this.RebelLV = rebelLV;
        this.RebelEXP = rebelEXP;
        this.SelfishLV = selfishLV;
        this.SelfishEXP = selfishEXP;
        this.EvilLV = evilLV;
        this.EvilEXP = evilEXP;
    }

}
