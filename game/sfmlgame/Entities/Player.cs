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
using static System.Net.Mime.MediaTypeNames;
using sfmlgame.Scenes;
using sfmlgame.Framework;

namespace sfmlgame.Entities
{
    public class Player : Entity
    {
        public PlayerStats Stats;

        private WorldManager world; // Reference to the World object

        //public Sprite Sprite;

        

        public Vector2i CurrentChunkIndex { get; private set; }

        public List<Ability> Abilities { get; private set; } = new List<Ability>();



        AbilityFactory abilityFactory;

        public UI_ProgressBar HPBar;

        private Dictionary<Keyboard.Key, bool> previousKeysPressed = new Dictionary<Keyboard.Key, bool>();

        

        public Player(Texture texture, Vector2f position, WorldManager world) : base("Entities/priestess", "priestess", 5, position)
        {
            Stats = new PlayerStats();

            Stats.MaxHP = 10;
            Stats.CurrentHP = 9;

            HPBar = new UI_ProgressBar(new Vector2f(GetPosition().X, GetPosition().Y), new UIBinding<int>(() => Stats.CurrentHP), new UIBinding<int>(() => Stats.MaxHP), new Vector2f(32, 8), Color.Green, this);
            //Game.Instance.UIManager.AddComponent(hpBar);
            

            this.world = world;
            abilityFactory = new AbilityFactory();
            CanCheckCollision = true;

            // Initialize key state tracking
            foreach (Keyboard.Key key in Enum.GetValues(typeof(Keyboard.Key)))
            {
                previousKeysPressed[key] = false;
            }
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
            Stats.ProcessXP(xpAmount);
        }

        private void HandleMagnetPickup(Magnet magnet)
        {
            magnet.PickItUp(); // Or any specific logic for Magnets
                               // Perhaps Magnets don't give XP but have another effect
        }

        


        private float Vector2fDistance(Vector2f point1, Vector2f point2)
        {
            return (float)Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2));
        }

        public Vector2i PreviousChunkIndex { get; private set; }

        private float pickupRadius = 100f; // 100 pixels as an example, adjust based on your game's scale

        // Add this field at the class level to keep track of whether the key was previously pressed
        private bool previousGKeyPressed = false;

        public void Update(float deltaTime)
        {
            HandleKeyboardInput(deltaTime); // Handle player input and movement

            Vector2f worldPosition = GetWorldPosition(); // Get the updated world position after movement
            Vector2i newChunkIndex = world.CalculateChunkIndex(worldPosition);

            if (newChunkIndex != CurrentChunkIndex)
            {
                // Correctly position the player within the new chunk center
                SetPosition(world.AdjustPlayerPositionForChunkTransition(GetPosition(), CurrentChunkIndex, newChunkIndex));
                CurrentChunkIndex = newChunkIndex; // Update the current chunk index
                world.ManageChunks(newChunkIndex); // Manage chunk loading/unloading
            }

            UpdatePlayerAbilities(deltaTime); // Update abilities based on cooldowns and usage
            CheckCollisionWithPickups(); // Check for and handle collisions with pickups
            base.Update(this, deltaTime); // Call base update (if any additional logic is needed there)

            HPBar.Update(deltaTime); // Update the player's health bar UI
        }


        private Vector2f CalculateMovement(float deltaTime)
        {
            Vector2f movement = new Vector2f();
            if (Keyboard.IsKeyPressed(Keyboard.Key.W)) movement.Y -= Stats.MovementSpeed * deltaTime;
            if (Keyboard.IsKeyPressed(Keyboard.Key.S)) movement.Y += Stats.MovementSpeed * deltaTime;
            if (Keyboard.IsKeyPressed(Keyboard.Key.A)) movement.X -= Stats.MovementSpeed * deltaTime;
            if (Keyboard.IsKeyPressed(Keyboard.Key.D)) movement.X += Stats.MovementSpeed * deltaTime;
            return movement;
        }

        // This function calculates the player's position in world space
        private Vector2f GetWorldPosition()
        {
            return GetPosition();  // Directly return the position since it's always relative to the current chunk
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

        private void HandleKeyboardInput(float deltaTime)
        {
            Vector2f movement = CalculateMovement(deltaTime);
            SetPosition(movement + GetPosition());  // Update player's position based on input

            // Additional command checks (like toggling debug mode, etc.)
            CheckSingleKeyPress(Keyboard.Key.Escape, () => Game.Instance.SceneTransition(new MainMenuScene()));
            CheckSingleKeyPress(Keyboard.Key.C, () => Game.Instance.SceneManager.CallGameSceneForOpenPlayerInfo());
            CheckSingleKeyPress(Keyboard.Key.G, () => { Game.Instance.Debug = !Game.Instance.Debug; });
        }


        private void CheckSingleKeyPress(Keyboard.Key key, Action action)
        {
            bool currentlyPressed = Keyboard.IsKeyPressed(key);
            if (currentlyPressed && previousKeysPressed.ContainsKey(key) && !previousKeysPressed[key])
            {
                action.Invoke();
            }
            previousKeysPressed[key] = currentlyPressed; // Update the state in the dictionary
        }




    }
}
