using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sfmlgame.World;
using sfmlgame.Abilities;
using System.Numerics;
using sfmlgame.Managers;
using sfmlgame.Entities.Pickups;
using sfmlgame.UI;

namespace sfmlgame.Entities
{
    public class Player : Entity
    {
        private WorldManager world; // Reference to the World object

        //public Sprite Sprite;
        private float speed = 75f;

        public Vector2i CurrentChunkIndex { get; private set; }

        public List<Ability> Abilities { get; private set; } = new List<Ability>();

        public int XP = 0;
        public int NeededXP = 4;

        public int Level = 1;

        AbilityFactory abilityFactory;

        public Player(Texture texture, Vector2f position, WorldManager world) : base("Entities/priestess","priestess", 5, position)
        {
            this.world = world; // Store the World reference

            abilityFactory = new AbilityFactory();

            

            CanCheckCollision = true;
        }

        

        public void LevelUp(int levels)
        {
            

            Level += levels;

            Game.Instance.MainPowerUpMenu.OpenWindow();
            
            //var newAbility = abilityFactory.CreateRandomAbility(this);

            //Abilities.Add(newAbility);

            SoundManager.Instance.PlayLevelUp();
        }

        private void CheckCollisionWithPickups()
        {
            foreach (Gem gem in Game.Instance.EntityManager.AllEntities.OfType<Gem>().ToList().Where(x => x.IsActive))
            {
                if (base.CheckCollision(gem))
                {
                    int xpAmount = gem.Pickup();

                    while (xpAmount > 0)
                    {

                        if (XP + xpAmount >= NeededXP)
                        {
                            xpAmount -= (NeededXP - XP);
                            XP = 0;
                            LevelUp(1);

                            //powerupMenu.OpenWindow();
                        }
                        else
                        {
                            XP += xpAmount;
                            xpAmount = 0;
                            
                        }
                    }
                    
                }
            }
        }

        public Vector2i PreviousChunkIndex { get; private set; }

        public void Update(float deltaTime)
        {
            Vector2f movement = new Vector2f();
            if (Keyboard.IsKeyPressed(Keyboard.Key.W)) movement.Y -= speed * deltaTime;
            if (Keyboard.IsKeyPressed(Keyboard.Key.S)) movement.Y += speed * deltaTime;
            if (Keyboard.IsKeyPressed(Keyboard.Key.A)) movement.X -= speed * deltaTime;
            if (Keyboard.IsKeyPressed(Keyboard.Key.D)) movement.X += speed * deltaTime;

            if(Keyboard.IsKeyPressed(Keyboard.Key.G))
            {
                Game.Instance.Debug = !Game.Instance.Debug;
            }

            SetPosition(GetPosition() + movement);

            if (world == null) return;

            PreviousChunkIndex = CurrentChunkIndex;
            CurrentChunkIndex = world.CalculateChunkIndex(GetPosition());

            // Check if the player has moved to a new chunk
            if (CurrentChunkIndex != PreviousChunkIndex)
            {
                world.ManageChunks(GetPosition());
            }

            UpdatePlayerAbilities(deltaTime);

            CheckCollisionWithPickups();

            //CheckXP();

        }

        public override void Draw(RenderTexture renderTexture, float deltaTime)
        {
            base.Draw(renderTexture, deltaTime);


        }

        private void UpdatePlayerAbilities(float deltaTime)
        {
            foreach (Ability ability in Abilities)
            {
                ability.Update();

                if (ability.abilityClock.ElapsedTime.AsSeconds() >= ability.Cooldown)
                {
                    ability.Activate();
                    ability.LastActivatedTime = ability.abilityClock.Restart().AsSeconds();
                }
            }
        }

        public override void ResetFromPool(Vector2f position)
        {
            // this hopefully never happens, thank you :) -Player
            throw new NotImplementedException();
        }
    }
}
