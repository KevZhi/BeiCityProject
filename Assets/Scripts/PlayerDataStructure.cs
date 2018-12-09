[System.Serializable]
public class PlayerDataStructure {

    public string SavedText;
    public string CurScene;

    public PlayerDataStructure(string curScene)
    {
        this.CurScene = curScene;
    }

}
