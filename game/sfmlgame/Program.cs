// See https://aka.ms/new-console-template for more information
using SFML.Audio;
using sfmlgame;
using sfmlgame.Managers;

class Program
{
    public static SoundManager SoundManager;

    

    static void Main(string[] args)
    {
        SoundManager = new SoundManager();

        Game game = Game.Instance;
        game.Run();

        var windowSize = game.GetWindow().Size;
        Console.WriteLine("Final Window Size: ");
        Console.WriteLine(windowSize.ToString());

        Console.ReadLine();
    }
}

