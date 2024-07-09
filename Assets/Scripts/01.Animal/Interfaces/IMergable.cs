public interface IMergable
{
    public int Grade { get; set; }
    public AnimalType Type { get; set; }
    public int MergeId { get; set; }
    public bool Merge(AnimalWork animal);
}