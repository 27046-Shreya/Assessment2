using System.Globalization;

namespace MusicNoteApplication;

public class Program
{
    public static void Main()
    {
        Dictionary<string, MusicNote> musicNotes = new Dictionary<string, MusicNote>()
        {
            {"C", new MusicNote(261.63, "C", 2000) },
            {"C#", new MusicNote(277.18, "C#", 2000) },
            {"D", new MusicNote(293.66, "D", 2000) },
            {"D#", new MusicNote(311.13, "D#", 2000) },
            {"E", new MusicNote(329.63, "E", 2000) },
            {"F", new MusicNote(349.23, "F", 2000) },
            {"F#", new MusicNote(369.99, "F#", 2000) },
            {"G", new MusicNote(329.00, "G", 2000) },
            {"G#", new MusicNote(415.30, "G#", 2000) },
            {"A", new MusicNote(440.00, "A", 2000) },
            {"A#", new MusicNote(466.16, "A#", 2000) },
            {"B", new MusicNote(493.88, "B", 2000) },
        };


        string inputSequence;

        while(true)
        {
            Console.Clear();
            Console.Write("Enter Music Sequence or esc to exit application: ");
            inputSequence = Console.ReadLine() ?? string.Empty;
            if (string.Equals(inputSequence, "esc", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            if (string.IsNullOrEmpty(inputSequence))
            {
                Console.WriteLine("Input cannot be null or empty");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();

            }
            else if(inputSequence.Where(x => char.IsDigit(x)).Any())
            {
                Console.WriteLine("Numeric notes not supported");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
            else
            {
                PlayMusic(inputSequence, musicNotes);

                Console.WriteLine("Press any key after enjoying the music....");
                Console.ReadKey();
            }
        }

        
    }

    public static void PlayMusic(string inputSequence, Dictionary<string, MusicNote> musicNotes)
    {
        string[] musicSequence = inputSequence.Split(' ');
        // To maintain the sequence, List and Queue can also be used. Preferring string array as it can iterated but not enumerated. Iteration is faster and simpler that enumeration. 
        Console.WriteLine("Listen..........");


        for (int i = 0; i < musicSequence.Length; i++)
        {
            string note = musicSequence[i];
            if (musicNotes.ContainsKey(note))
            {
                Console.Beep((int)musicNotes[note].Frequency, musicNotes[note].Duration);
            }
            else
            {
                Console.WriteLine($"{note} note found!!");
                Thread.Sleep(2000);
            }
        }
    }

}
