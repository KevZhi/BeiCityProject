[System.Serializable]
public class PlayerDataStructure {

    public string SavedText;
    public string CurScene;

    public PlayerDataStructure(string curScene,string savedText)
    {
        this.CurScene = curScene;
        this.SavedText = savedText;
    }

}
