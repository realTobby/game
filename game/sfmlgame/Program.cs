// See https://aka.ms/new-console-template for more information
using SFML.Audio;
using sfmlgame;
using sfmlgame.Managers;

class Program
{
    public static SoundManager SoundManager;

    public static Music backgroundMusic = new Music("Assets/BGM/Venus.wav");

    static void Main(string[] args)
    {
        SoundManager = new SoundManager();

        backgroundMusic.Play();

        Game game = Game.Instance;
        game.Run();

        backgroundMusic.Stop();

        var windowSize = game.GetWindow().Size;
        Console.WriteLine("Final Window Size: ");
        Console.WriteLine(windowSize.ToString());

        Console.ReadLine();
    }
}

