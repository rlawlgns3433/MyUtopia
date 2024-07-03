public interface IMergable
{
    public int Grade { get; set; }
    public AnimalType Type { get; set; }
    public Animal Merge(IMergable animal);
}