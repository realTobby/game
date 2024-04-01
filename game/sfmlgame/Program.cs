// See https://aka.ms/new-console-template for more information
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
    }
}

