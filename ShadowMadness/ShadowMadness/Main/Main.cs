using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GDLibrary.Managers;
using GDLibrary;
using GDLibrary.Utilities;
using System.Collections.Generic;
using CGPLibrary.Sound;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Primitive2D;
using System;

namespace ShadowMadness
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Main : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;
        //public BasicEffect basicEffect;

        private SpriteBatch spriteBatch;
        private SpriteManager spriteManager;
        private static TextureManager textureManager;
        private KeyboardManager keyboardManager;
        private MouseManager mouseManager;
        private MenuManager menuManager;
        private SoundManager soundManager;

        private Camera2D camera2D;

        private UIManager uiManager;

        private CloudFactory cloudFactory;

        private Rectangle levelBounds;
        private Rectangle screenBounds;

        public ShadowCastManager shadowCastManager;
        private int level=0;
        
        private List<Block> collidables;        //platforms to pass through and land on
        private List<Block> nonCollidables;     
        private List<Block> exits;
        private List<Block> springs;
        private List<Block> spikes;
        private List<Block> coins;
        private List<Block> key;
        private List<Block> destructables;
        private List<Block> solidCollidables;   //walls etc

        //the grouped rectangles based on the collidables etc. (anything you want to cast a shadow).
        private List<Rectangle> shadowCasters;

        public List<List<Vector2>> shadows;
        public Draw2D draw2D;
        public Color shadowColor;

        //Testing
        //public Line lineTest;
        

        Texture2D blank;
        public String currentLevel;
        //Enemies
        private List<Enemy> enemies;

        private Player player;

        //Sound
        SoundEffectInstance backgroundMusic;


        #region PROPERTIES
        public List<Rectangle> SHADOWCASTERS
        {
            get
            {
                return shadowCasters;
            }
            set
            {
                shadowCasters = value;
            }
        }
        public List<Block> SOLIDCOLLIDABLES
        {
            get
            {
                return solidCollidables;
            }
        }
        public SoundManager SOUNDMANAGER
        {
            get
            {
                return soundManager;
            }
        }
        public SoundEffectInstance BACKGROUNDMUSIC
        {
            get
            {
                return backgroundMusic;
            }
            set
            {
                backgroundMusic = value;
            }
        }
        public List<Enemy> ENEMIES
        {
            get
            {
                return enemies;
            }
        }
        public List<Block> SPIKES
        {
            get
            {
                return spikes;
            }
        }
        public List<Block> COINS
        {
            get
            {
                return coins;
            }
        }
        public List<Block> KEY
        {
            get
            {
                return key;
            }
        }
        public List<Block> EXITS
        {
            get
            {
                return exits;
            }
        }
        public List<Block> SPRINGS
        {
            get
            {
                return springs;
            }
        }
        public Player PLAYER
        {
            get
            {
                return player;
            }
        }
        public List<Block> COLLIDABLES
        {
            get
            {
                return collidables;
            }
        }
        public List<Block> DESTRUCTABLE
        {
            get
            {
                return destructables;

            }
        }
        public List<Block> NONCOLLIDABLES
        {
            get
            {
                return nonCollidables;
            }
        }
        public Camera2D CAMERA2D
        {
            get
            {
                return camera2D;
            }
        }

        //public Camera2DPathManager PATHMANAGER
        //{
        //    get
        //    {
        //        return pathManager;
        //    }
        //}

        public SpriteBatch SPRITEBATCH
        {
            get
            {
                return spriteBatch;
            }
        }
        //public EnemySprite ENEMYSPRITE
        //{
        //    get
        //    {
        //        return enemySprite;
        //    }
        //}

        public Rectangle SCREENBOUNDS
        {
            get
            {
                return screenBounds;
            }
        }
        public Rectangle LEVELBOUNDS
        {
            get
            {
                return levelBounds;
            }
        }
        //public Rectangle INFLATEDSCREENBOUNDS
        //{
        //    get
        //    {
        //        return inflatedScreenBounds;
        //    }
        //}

        public MenuManager MENUMANAGER
        {
            get
            {
                return menuManager;
            }
        }

        public MouseManager MOUSEMANAGER
        {
            get
            {
                return mouseManager;
            }
        }

        public SpriteManager SPRITEMANAGER
        {
            get
            {
                return spriteManager;
            }
        }
        public TextureManager TEXTUREMANAGER
        {
            get
            {
                return textureManager;
            }
        }
        public KeyboardManager KEYBOARDMANAGER
        {
            get
            {
                return keyboardManager;
            }
        }
        #endregion

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            level++;
            currentLevel = "Levels/Level" + level + ".txt";
            setScreenProperties();

            Collision.setCollisionSectors(new BSPCollisionSectorLayout(4, 4, this.LEVELBOUNDS));

            shadows = new List<List<Vector2>>();
            this.IsMouseVisible = false;
            graphics.PreferMultiSampling = true;
            //shadowColor = new Color(0, 0, 0, 120);

            loadManagers();

            loadTextures();

            //loadLevel();

            loadLevelSprites("", currentLevel, 60, 60);
           // loadLevelSprites("", "forground.txt", 60, 60);

            loadFonts();

            loadCamera();

            setUpTestingStuff();

            loadSoundData();

            //loadEnemiesData();

            //addEnemies();

            loadPlayerData();

            addPlayer();

            loadCloudFactory();

            loadUIData();
            addUI();



            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            draw2D = new Draw2D(this, spriteBatch);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            showHideMenu();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //updateUI();
            Vector2 mouse = shadowCastManager.shadowCastingPosition;

            shadows.Clear();

            //using the nonCollidables atm
            for (int i = 0; i < shadowCasters.Count; i++)
            {
                shadows.Add(Util.shadowFactory(shadowCasters[i], mouse, GameData.WINDOW_SIZE));
            }


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        public void addShadow(Rectangle r)
        {
            shadowCasters.Add(r);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(101, 58, 58)); //Color of the background.

            drawTestingStuff();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        //think can get rid of this.
        private void setUpTestingStuff()
        {

            shadowCastManager = new ShadowCastManager(this);
            Components.Add(shadowCastManager);
            

            //blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            //blank.SetData(new[] { Color.White });
            
        }

        public void drawTestingStuff()
        {
            //spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearClamp,
            //        DepthStencilState.Default, RasterizerState.CullNone,
            //        null, CAMERA2D.MATRIX);
            ////Line.DrawLine(spriteBatch, blank, 2, new Color(0, 0, 0, 120), lineTest.start, lineTest.end);
            ////Line.DrawLine(spriteBatch, blank, 1, Color.Red, player.walkingLine.start, player.walkingLine.end);

            ////contact lines
            ////for (int i = 0; i < shadowCastManager.contactLines.Count; i++)
            ////{
            ////    Line.DrawLine(spriteBatch, blank, 1, Color.Red, shadowCastManager.contactLines[i].start, shadowCastManager.contactLines[i].end);
            ////}
            //spriteBatch.End();
        }

        private void setScreenProperties()
        {
            int[] levelLenght = getFileLength("", currentLevel);
            graphics.PreferredBackBufferWidth = (int)GameData.WINDOW_SIZE.X;
            graphics.PreferredBackBufferHeight = (int)GameData.WINDOW_SIZE.Y;
            graphics.ApplyChanges();

            this.levelBounds = new Rectangle(0, 0,
                levelLenght[1]*60,
                    levelLenght[0]*60);

            this.screenBounds = new Rectangle(0, 0, GameData.WINDOW_WIDTH, GameData.WINDOW_HEIGHT);
        }

        private void loadManagers()
        {
            this.spriteManager = new SpriteManager(this);
            Components.Add(spriteManager);

            textureManager = new TextureManager(this);

            this.keyboardManager = new KeyboardManager(this);

            Components.Add(keyboardManager);

            //string[] strMenuTextures = {};

            //need to implement fonts etc for menu.
            //this.menuManager = new MenuManager(this, strMenuTextures,
            //    "Assets\\Fonts\\MenuFont", new Integer2(10, 10));

            this.mouseManager = new MouseManager(this, true);
            Components.Add(mouseManager);

            string[] menuTextures = {"Assets\\HUD\\pause_texture"};

            this.menuManager = new MenuManager(this, menuTextures, "Assets\\Fonts\\MenuFont", new Integer2(0, 0));

            this.soundManager = new SoundManager();
        }

        private void loadTextures()
        {
            //load textures

            
            textureManager.Add("items_spritesheet", "Assets\\Items\\items_spritesheet");
            textureManager.Add("key", "Assets\\Items\\Key");
            textureManager.Add("keySheet", "Assets\\Items\\keySheet");
            textureManager.Add("tileSheet", "Assets\\Tiles\\tileSheet");
            textureManager.Add("grassTiles", "Assets\\Tiles\\grassTiles");
            textureManager.Add("cloudsheet", "Assets\\Tiles\\cloudsheet");
            textureManager.Add("spikes", "Assets\\Tiles\\spikes");
            textureManager.Add("door", "Assets\\Tiles\\door");
            textureManager.Add("spritesheet", "Assets\\Player\\spritesheet");
            textureManager.Add("HealthBar", "Assets\\HUD\\HealthBar");
            textureManager.Add("destructableTile", "Assets\\Tiles\\destructableTile");
        }

        private void loadFonts()
        {
            //fonts
        }

        private void loadCamera()
        {
            int[] levelLenght = getFileLength("", currentLevel);
            //nmcg - camera path manager - step 1
            this.camera2D = new Camera2D(this,
                new Vector2(this.GraphicsDevice.Viewport.Width / 2, levelLenght[0]*60), 0, 1);
            //nmcg - game camera - step 3
            Components.Add(camera2D);
        }

        private void loadSoundData()
        {
            
            soundManager.add(new SoundEffectInfo(this, "backgroundMusic", "Assets\\Sounds\\backgroundMusic", 1, 0, 0.5f, true));
            soundManager.add(new SoundEffectInfo(this, "jump", "Assets\\Sounds\\jump", 0.1f, 0, 0.5f, false));
            soundManager.add(new SoundEffectInfo(this, "pickup", "Assets\\Sounds\\pickup", 0.3f, 0, 0.5f, false));
            soundManager.add(new SoundEffectInfo(this, "hurt", "Assets\\Sounds\\hurt", 1, 0, 0.5f, false));
            soundManager.add(new SoundEffectInfo(this, "endGame", "Assets\\Sounds\\endGame", 0.2f, 0, 0.5f, false));

            backgroundMusic = soundManager.getEffectInstance("backgroundMusic");
            backgroundMusic.Play();
        }

        private void loadCloudFactory()
        {

            List<Rectangle> cloudSources = new List<Rectangle>();
            
            cloudSources.Add(new Rectangle(0, 0, 129, 71));
            cloudSources.Add(new Rectangle(0, 72, 129, 71));
            cloudSources.Add(new Rectangle(0, 144, 129, 71));

            this.cloudFactory = new CloudFactory(this, cloudSources, .05f, 15, 
                new Rectangle(0, 0, GameData.LEVEL_WIDTH, GameData.LEVEL_HEIGHT));

            Components.Add(cloudFactory);
        }
        private void loadPlayerData()
        {
            List<Rectangle> walkSources = new List<Rectangle>();
            walkSources.Add(new Rectangle(0, 0, 40, 30));
            walkSources.Add(new Rectangle(40, 0, 40, 30));
            walkSources.Add(new Rectangle(80, 0, 40, 30));
            walkSources.Add(new Rectangle(120, 0, 40, 30));
            walkSources.Add(new Rectangle(160, 0, 40, 30));


            //List<Rectangle> duckSources = new List<Rectangle>();
            //duckSources.Add(new Rectangle(365, 98, 69, 71));

            //List<Rectangle> jumpSources = new List<Rectangle>();
            //jumpSources.Add(new Rectangle(438, 93, 67, 94));

            List<Rectangle> standSources = new List<Rectangle>();
            standSources.Add(new Rectangle(0, 0, 40, 30));

            textureManager.Add("playerWalk", textureManager.Get("spritesheet").TEXTURE, walkSources);
            //textureManager.Add("playerDuck", textureManager.Get("player_spritesheet").TEXTURE, duckSources);
            //textureManager.Add("playerJump", textureManager.Get("player_spritesheet").TEXTURE, jumpSources);
            textureManager.Add("playerStand", textureManager.Get("spritesheet").TEXTURE, standSources);
        }

        private void addPlayer()
        {
            List<AnimatedTextureData> playerAnimationList = new List<AnimatedTextureData>();
            
            playerAnimationList.Add((AnimatedTextureData)textureManager.Get("playerWalk"));
            playerAnimationList.Add((AnimatedTextureData)textureManager.Get("playerStand"));
            
   


            //0 - walk
            //1 - duck
            //2 - jump
            //3 - stand
            int[] levelLenght = getFileLength("", currentLevel);
            Player player = new Player("player", playerAnimationList,
                new SpritePresentationInfo(new Rectangle(0, 196, 66, 92), 0.2f), //uses innitial one I think?
                new SpritePositionInfo(new Vector2(160, (levelLenght[0]*60)-160), playerAnimationList[0].FRAMEWIDTH, playerAnimationList[0].FRAMEHEIGHT),
                30);
            //2175
            player.changeAnimation(3);

            this.player = player;

            this.spriteManager.Add(player);

            System.Diagnostics.Debug.WriteLine(player.walkingLine.slope);
        }

        private int[] getFileLength(string path, string name)
        {

            path += name;
            String[] fileData = System.IO.File.ReadAllLines(path);
            String[] lineData = null;
            int[] levelLength= new int[2];
            int rowCount = fileData.Length, colCount = 0;
            lineData = fileData[0].Split(',');
            colCount = lineData.Length;
            GameData.LEVEL_WIDTH = colCount*60;
            GameData.LEVEL_HEIGHT = rowCount*60;
            levelLength[0] = rowCount;
            levelLength[1] = colCount;
            return levelLength;
        }

        private void loadEnemiesData()
        {
            //fly
            List<Rectangle> flySources = new List<Rectangle>();
            flySources.Add(new Rectangle(0, 32, 72, 36));
            flySources.Add(new Rectangle(0, 0, 75, 31));
            textureManager.Add("flyFlying", textureManager.Get("enemies_spritesheet").TEXTURE, flySources);

            List<Rectangle> snailSources = new List<Rectangle>();
            snailSources.Add(new Rectangle(143, 34, 54, 31));
            snailSources.Add(new Rectangle(67, 87, 57, 31));
            textureManager.Add("snailWalking", textureManager.Get("enemies_spritesheet").TEXTURE, snailSources);

            List<Rectangle> slimeSources = new List<Rectangle>();
            slimeSources.Add(new Rectangle(52, 125, 50, 28));
            slimeSources.Add(new Rectangle(0, 125, 51, 26));
            textureManager.Add("slimeWalking", textureManager.Get("enemies_spritesheet").TEXTURE, slimeSources);
        }

        private void addEnemies()
        {
            this.enemies = new List<Enemy>();

            //addEnemy(1, new Vector2(550, 2000), new Vector2(200, 2000), 70);
            //add more enemies here.
            addEnemy(2, new Vector2(365, 599), new Vector2(145, 599), 110);
            addEnemy(3, new Vector2(490, 1724), new Vector2(210, 1724), 140);
        }

        private void addEnemy(int type, Vector2 startPos, Vector2 endPos, int speed)
        {
            float depth = 0.25f;
            //if it is a valid type
            if (type > 0 && type < 4) // the amount of enemy types, think 3 atm
            {
                List<AnimatedTextureData> animationList = new List<AnimatedTextureData>();
                if (type == 1)
                {
                    animationList.Add((AnimatedTextureData)textureManager.Get("flyFlying"));

                    Enemy enemy = new Enemy(startPos, endPos, speed, "enemyFly",
                                            animationList,
                                            new SpritePresentationInfo(new Rectangle(0, 32, 72, 36), depth),
                                            new SpritePositionInfo(startPos, animationList[0].FRAMEWIDTH, animationList[0].FRAMEHEIGHT),
                                            5); //animation rate
                    enemies.Add(enemy);
                    spriteManager.Add(enemy);
                }
                else if (type == 2)
                {
                    //snail?
                    animationList.Add((AnimatedTextureData)textureManager.Get("snailWalking"));

                    Enemy enemy = new Enemy(startPos, endPos, speed, "enemySnail",
                                            animationList,
                                            new SpritePresentationInfo(new Rectangle(143, 34, 54, 31), depth),
                                            new SpritePositionInfo(startPos, animationList[0].FRAMEWIDTH, animationList[0].FRAMEHEIGHT),
                                            5); //animation rate
                    enemies.Add(enemy);
                    spriteManager.Add(enemy);
                }
                else if (type == 3)
                {
                    //slime?
                    animationList.Add((AnimatedTextureData)textureManager.Get("slimeWalking"));

                    Enemy enemy = new Enemy(startPos, endPos, speed, "enemySlime",
                                            animationList,
                                            new SpritePresentationInfo(new Rectangle(52, 125, 50, 28), depth),
                                            new SpritePositionInfo(startPos, animationList[0].FRAMEWIDTH, animationList[0].FRAMEHEIGHT),
                                            5); //animation rate
                    enemies.Add(enemy);
                    spriteManager.Add(enemy);
                }                
            }
        }

        private void loadUIData()
        {
            //heart(s)
           


            List<Rectangle> healthBar = new List<Rectangle>();
            healthBar.Add(new Rectangle(0, 0, 128, 26));
           

            //animated texture datas
            textureManager.Add("healthBar", textureManager.Get("HealthBar").TEXTURE, healthBar);
          

            //key

            List<Rectangle> key = new List<Rectangle>();
            key.Add(new Rectangle(0, 0, 58, 58));
            textureManager.Add("keySheet", textureManager.Get("keySheet").TEXTURE, key);
           

         

          

            
        }



        private void addUI()
        {
            uiManager = new UIManager(this);
            Components.Add(uiManager);
        }

        private void updateUI()
        {
            uiManager.updateHealth(player.HEALTH);
            uiManager.updateKey(player.HASKEY);
        }


   

        private void loadLevelSprites(string path, string name, int tileWidth, int tileHeight)
        {
            //call multiple times for each level element
            List<Block>[] blocks = LevelManager.load(path, name, tileWidth, tileHeight, textureManager);
            this.shadowCasters = LevelManager.loadShadowGrid(path, name, tileWidth, tileHeight);


            //used for organized collision groups.
            this.collidables = blocks[0];
            this.nonCollidables = blocks[1];
            this.exits = blocks[2];
            this.springs = blocks[3];
            this.spikes = blocks[4];
            this.key = blocks[5];
            this.coins = blocks[6];
            this.destructables = blocks[7];
            this.solidCollidables = blocks[8];


            //add the collidables
            for (int i = 0; i < collidables.Count; i++)
            {
                this.spriteManager.Add(collidables[i]);
            }
            //non collidables
            for (int i = 0; i < nonCollidables.Count; i++)
            {
                this.spriteManager.Add(nonCollidables[i]);
            }
            //exits
            for (int i = 0; i < exits.Count; i++)
            {
                this.spriteManager.Add(exits[i]);
            }
            //springs
            for (int i = 0; i < springs.Count; i++)
            {
                this.spriteManager.Add(springs[i]);
            }
            //spikes
            for (int i = 0; i < spikes.Count; i++)
            {
                this.spriteManager.Add(spikes[i]);
            }
            //key(s)
            for (int i = 0; i < key.Count; i++)
            {
                this.spriteManager.Add(key[i]);
            }
            //stars
            for (int i = 0; i < coins.Count; i++)
            {
                this.spriteManager.Add(coins[i]);
            }
            //destructables
            for (int i = 0; i < destructables.Count; i++)
            {
                this.spriteManager.Add(destructables[i]);
            }

            for (int i = 0; i < solidCollidables.Count; i++)
            {
                this.spriteManager.Add(solidCollidables[i]);
            }
        }

        private void showHideMenu()
        {
            if (KEYBOARDMANAGER.isFirstKeyPress(Keys.Escape))
            {
                if (!SPRITEMANAGER.PAUSE)
                {
                    backgroundMusic.Pause();
                    SPRITEMANAGER.PAUSE = true;
                    Components.Add(menuManager);
                }
                else if(!player.GAMEOVER)
                {
                    backgroundMusic.Play();
                    SPRITEMANAGER.PAUSE = false;
                    Components.Remove(menuManager);
                }
            }
        }

        public void endGame()
        {
            //BACKGROUNDMUSIC = soundManager.getEffectInstance("backgroundMusic");
            BACKGROUNDMUSIC.Pause();
            SPRITEMANAGER.PAUSE = true;
            Components.Add(SpriteManager.GAME.MENUMANAGER);
            MENUMANAGER.showEndGameScreen();

            
        }
    }
}
