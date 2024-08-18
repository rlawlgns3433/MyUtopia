using TMPro;

public class PopulationText : Observer
{
    private static readonly string format = "{0} / {1}";
    public Floor floor;
    public TextMeshProUGUI textPopulation;

    private void OnEnable()
    {
        textPopulation.text = string.Format(format, floor.animals.Count, floor.FloorStat.Max_Population);
    }

    public override void Notify(Subject subject)
    {
        textPopulation.text = string.Format(format, floor.animals.Count, floor.FloorStat.Max_Population);
    }
}
