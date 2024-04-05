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
            foreach (Pickup pickup in Game.Instance.EntityManager.AllEntities.OfType<Pickup>().Where(x => x.IsActive))
            {
                float distance = Vector2fDistance(this.GetPosition(), pickup.GetPosition());

                if (distance <= pickupRadius)
                {
                    if (base.CheckCollision(pickup))
                    {
                        switch (pickup)
                        {
                            case Gem gem:
                                HandleGemPickup(gem);
                                break;
                            case Magnet magnet:
                                HandleMagnetPickup(magnet);
                                break;
                            // Add cases for other Pickup types as necessary
                            default:
                                pickup.PickItUpInt(); // Fallback for pickups without specific handling
                                break;
                        }
                    }
                }
            }
        }

        private void HandleGemPickup(Gem gem)
        {
            int xpAmount = gem.PickItUpInt(); // Or any specific logic for Gems
            ProcessXP(xpAmount);
        }

        private void HandleMagnetPickup(Magnet magnet)
        {
            magnet.PickItUp(); // Or any specific logic for Magnets
                               // Perhaps Magnets don't give XP but have another effect
        }

        private void ProcessXP(int xpAmount)
        {
            while (xpAmount > 0)
            {
                if (XP + xpAmount >= NeededXP)
                {
                    xpAmount -= (NeededXP - XP);
                    XP = 0;
                    LevelUp(1);
                }
                else
                {
                    XP += xpAmount;
                    xpAmount = 0;
                }
            }
        }


        private float Vector2fDistance(Vector2f point1, Vector2f point2)
        {
            return (float)Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2));
        }

        public Vector2i PreviousChunkIndex { get; private set; }

        private float pickupRadius = 100f; // 100 pixels as an example, adjust based on your game's scale


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

            base.SetPosition(GetPosition() + movement);

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

            base.Update(this, deltaTime);

        }

        public override void Draw(RenderTexture renderTexture, float deltaTime)
        {
            base.Draw(renderTexture, deltaTime);
        }

        private void UpdatePlayerAbilities(float deltaTime)
        {
            // Filter out abilities that are not FireballAbility and execute their logic
            foreach (var ability in Abilities)
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
