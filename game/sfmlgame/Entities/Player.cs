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

namespace sfmlgame.Entities
{
    public class Player : Entity
    {
        private WorldManager world; // Reference to the World object

        //public Sprite Sprite;

        public Action OnPlayerLevelUp;

        public Vector2i CurrentChunkIndex { get; private set; }

        public List<Ability> Abilities { get; private set; } = new List<Ability>();

        public int XP = 0;
        public int NeededXP = 4;

        public int Level = 1;

        AbilityFactory abilityFactory;

        // TODO: STAT MACHINE - DAMAGE ; DEFENSE; CRIT; AND STUFF LIKE THAT, KEEP IT SEPERATED, KEEP IT SIMPLE STUPID
        public int Damage = 0;

        public float Speed = 75f;



        private Dictionary<Keyboard.Key, bool> previousKeysPressed = new Dictionary<Keyboard.Key, bool>();

        public Player(Texture texture, Vector2f position, WorldManager world) : base("Entities/priestess", "priestess", 5, position)
        {
            this.world = world;
            abilityFactory = new AbilityFactory();
            CanCheckCollision = true;

            // Initialize key state tracking
            foreach (Keyboard.Key key in Enum.GetValues(typeof(Keyboard.Key)))
            {
                previousKeysPressed[key] = false;
            }
        }



        public void LevelUp(int levels)
        {
            Damage += 1;

            Speed += 1.25f;

            Level += levels;

            NeededXP = NeededXP + 5;

            OnPlayerLevelUp?.Invoke();

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

        // Add this field at the class level to keep track of whether the key was previously pressed
        private bool previousGKeyPressed = false;

        public void Update(float deltaTime)
        {
            HandleKeyboardInput(deltaTime);

            // Other updates like chunk management and collision checks
            PreviousChunkIndex = CurrentChunkIndex;
            CurrentChunkIndex = world.CalculateChunkIndex(GetPosition());
            if (CurrentChunkIndex != PreviousChunkIndex)
            {
                world.ManageChunks(GetPosition());
                world.UpdateTrapsForCurrentChunk(CurrentChunkIndex);
            }

            UpdatePlayerAbilities(deltaTime);
            CheckCollisionWithPickups();
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

        private void HandleKeyboardInput(float deltaTime)
        {
            Vector2f movement = new Vector2f();

            if (Keyboard.IsKeyPressed(Keyboard.Key.W)) movement.Y -= Speed * deltaTime;
            if (Keyboard.IsKeyPressed(Keyboard.Key.S)) movement.Y += Speed * deltaTime;
            if (Keyboard.IsKeyPressed(Keyboard.Key.A)) movement.X -= Speed * deltaTime;
            if (Keyboard.IsKeyPressed(Keyboard.Key.D)) movement.X += Speed * deltaTime;

            SetPosition(GetPosition() + movement);

            // Specific key checks for non-movement commands
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
