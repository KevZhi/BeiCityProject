
[System.Serializable]
public class EventDataStructure
{
    public int event001;
    public int event002;
    public int event003;

    public int desk01observed;
    public int desk02observed;
    public int desk03observed;

    public int mR02s0actived;
    public int mR02s1actived;
    public int wR03s1actived;
    public int sR02s1actived;
    public int wR03s2actived;
    public int sR05s1actived;
    public int wR02s1actived;
    public int sR07s1actived;

    public int event002canSubmit;

    public int sR07Helped;

    public EventDataStructure(
        int event001,
        int event002,
        int event003,
       
        int desk01observed,
        int desk02observed,
        int desk03observed,
       
        int mR02s0actived,
        int mR02s1actived,
        int wR03s1actived,
        int sR02s1actived,
        int wR03s2actived,
        int sR05s1actived,
        int wR02s1actived,
        int sR07s1actived,
       
        int event002canSubmit,
       
        int sR07Helped)
    {
        this.event001 = event001;
        this.event002 = event002;
        this.event003 = event003;

        this.desk01observed = desk01observed;
        this.desk02observed = desk02observed;
        this.desk03observed = desk03observed;

        this.mR02s0actived = mR02s0actived;
        this.mR02s1actived = mR02s1actived;
        this.wR03s1actived = wR03s1actived;
        this.sR02s1actived = sR02s1actived;
        this.wR03s2actived = wR03s2actived;
        this.sR05s1actived = sR05s1actived;
        this.wR02s1actived = wR02s1actived;
        this.sR07s1actived = sR07s1actived;

        this.event002canSubmit = event002canSubmit;

        this.sR07Helped = sR07Helped;
    }
}
