namespace MusicNoteApplication;

public class MusicNote
{
    public MusicNote(double frequency, string name, int duration)
    {
        this.Frequency = frequency;
        this.Name = name;
        this.Duration = duration;
    }
    public double Frequency { get; set; }
    public string Name { get; set; }

    public int Duration {  get; set; }
}
