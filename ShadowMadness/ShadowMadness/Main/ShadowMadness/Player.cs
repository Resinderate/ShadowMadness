using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GDLibrary;
using Microsoft.Xna.Framework;
using GDLibrary.Managers;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace ShadowMadness
{
    
    public class Player : ModularAnimatedSprite
    {
        private float moveAmount;

        private Vector2 velocity;
        private float acceleration;
        private Vector2 direction;
        private float gravity;
        private float jumpPower;
        private float springJumpPower;
        private bool hasKey;
        private int coins;
       
        private bool invulnerable;
        private int invulnerableTimer;

        private Vector2 levelStartPos;

        int health;

        private Vector2 oldPos;

        private List<Block> destructablesRemoved;
        private List<Rectangle> shadowsRemoved;
        private List<Block> keysTaken;

        private SoundEffectInstance jump;
        private SoundEffectInstance pickup;
        private SoundEffectInstance hurt;
        private SoundEffectInstance endGame;

        private bool gameOver;

        private float peakHeight;

        public Line walkingLine;

        private bool lineContact;
        private Line lastLineContact;

        #region PROPERTIES
        public bool GAMEOVER
        {
            get
            {
                return gameOver;
            }
        }
        public int HEALTH
        {
            get
            {
                return health;
            }
        }
        public int COINS
        {
            get
            {
                return coins;
            }
        }
        public bool HASKEY
        {
            get
            {
                return hasKey;
            }
        }
        #endregion

        public Player(string name, List<AnimatedTextureData> animationsList,
                    SpritePresentationInfo spritePresentationInfo,
                    SpritePositionInfo spritePositionInfo,
                    int frameRate)
            : base(name, animationsList,
                    spritePresentationInfo,
                    spritePositionInfo,
                    frameRate)
        {
            this.moveAmount = 0;
            this.velocity = Vector2.Zero;
            this.acceleration = 0.0015f;
            this.direction = Vector2.Zero;
            this.gravity = 0.0025f;
            this.jumpPower = 1.6f;
            this.springJumpPower = 2f;
            this.oldPos = new Vector2(spritePositionInfo.TRANSLATIONX, spritePositionInfo.TRANSLATIONY);
            this.hasKey = false;
            this.coins = 0;
            this.health = 4; // 3 health? 3 hearts?
            this.invulnerable = false;
            this.invulnerableTimer = 0;

            this.levelStartPos = spritePositionInfo.TRANSLATION;

            this.destructablesRemoved = new List<Block>();
            this.shadowsRemoved = new List<Rectangle>();
            this.keysTaken = new List<Block>();

            this.jump = SpriteManager.GAME.SOUNDMANAGER.getEffectInstance("jump");
            this.pickup = SpriteManager.GAME.SOUNDMANAGER.getEffectInstance("pickup");
            this.hurt = SpriteManager.GAME.SOUNDMANAGER.getEffectInstance("hurt");
            this.endGame = SpriteManager.GAME.SOUNDMANAGER.getEffectInstance("endGame");

            this.peakHeight = spritePositionInfo.TRANSLATIONY;

            this.walkingLine = new Line(0, POSITIONINFO.BOUNDS.Top, 0, POSITIONINFO.BOUNDS.Bottom);
            System.Diagnostics.Debug.WriteLine(walkingLine.start + " : " + walkingLine.end);
            
        }

        public override void Update(GameTime gameTime)
        {
            handleInput(gameTime);
            processDeceleration();
            processGravity(gameTime);
            checkCollision(gameTime);
            //moveIfOnALine(gameTime);
            keepInsideLevelBounds();
            stopMovingIfBlocked();
            updateAnimationDirections();
            handleAnimations();
            manageInvulnrableTimer(gameTime);
            checkForDeath();
            updatePeakHeight();
            //System.Diagnostics.Debug.WriteLine(peakHeight);
            checkLineContact();
            updateWalkingLine();

            base.Update(gameTime);
        }

        

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        private void updateWalkingLine()
        {
            this.walkingLine.start.X = POSITIONINFO.TRANSLATIONX + (POSITIONINFO.BOUNDSWIDTH / 2);
            this.walkingLine.start.Y = POSITIONINFO.BOUNDS.Top;

            this.walkingLine.end.X = POSITIONINFO.TRANSLATIONX + (POSITIONINFO.BOUNDSWIDTH / 2);
            this.walkingLine.end.Y = POSITIONINFO.BOUNDS.Bottom;
        }

        protected override void checkCollision(GameTime gameTime)
        {
            if (lineContact)
            {
                
                Vector2 movement = velocity * gameTime.ElapsedGameTime.Milliseconds;

                //new - old
                Line contactLine = lastLineContact;
                
                this.TranslateBy(movement.X, 0);
                this.TranslateTo(POSITIONINFO.TRANSLATIONX, contactLine.findPointOfX(POSITIONINFO.TRANSLATIONX + (POSITIONINFO.BOUNDSWIDTH / 2)).Y - POSITIONINFO.BOUNDSHEIGHT);
            }
            else
            {
                checkMovementCollision(gameTime);
                checkDestuctableCollision(gameTime);
                checkSpikeCollision();
                checkExitsCollision();
                checkKeyCollision();
            }
            
            //checkSpringCollision();
            //checkExitsCollision();
            //checkKeyCollision();
            //checkCoinCollision();
            //checkSpikeCollision();
            //checkEnemiesCollision();
        }

        private void moveIfOnALine(GameTime gameTime)
        {
            

        }

        protected override void handleInput(GameTime gameTime)
        {
            //System.Diagnostics.Debug.WriteLine(velocity);
            this.moveAmount = acceleration * gameTime.ElapsedGameTime.Milliseconds;

            
            if (SpriteManager.GAME.KEYBOARDMANAGER.isKeyDown(Keys.D))
                velocity.X += moveAmount;
 

            if (SpriteManager.GAME.KEYBOARDMANAGER.isKeyDown(Keys.A))                    
                velocity.X -= moveAmount;


            if (SpriteManager.GAME.KEYBOARDMANAGER.isKeyDown(Keys.S))
            {
                if(lineContact)
                    velocity *= Vector2.UnitX;
                peakHeight += 20;
            }
            

            if (SpriteManager.GAME.KEYBOARDMANAGER.isFirstKeyPress(Keys.Space) && (canJump() || lineContact))
            {
                lineContact = false;
                velocity += -Vector2.UnitY * jumpPower;
                this.jump.Play();
            }   
        }

        private void handleAnimations()
        {
            //standing still
            if ((velocity.X < 0.05f && velocity.X > -0.05f && velocity.Y > -0.1f && velocity.Y < 0.1f) || (velocity.X < 0.05f && velocity.X > -0.05f && lineContact))
            {
                if (CURRENTANIMATION != 3)
                    changePlayerAnimation(3);
            }
            else
            {
                ////Jump stuff
                //if (!canJump()) //in the air
                //{
                //    if (CURRENTANIMATION != 2)
                //        this.changePlayerAnimation(2); //Jumping
                //}
                //else  //should be checking if you can jump ie. on the floor, (dont need to check this else if actually, implied from previous)
                //{
                //    //walking
                //    if (CURRENTANIMATION != 0)
                //        changePlayerAnimation(0);
                //}
                if (CURRENTANIMATION != 0)
                    changePlayerAnimation(0);
            }
        }

        

        private void checkMovementCollision(GameTime gameTime)
        {
           

            //previous position before the movement.

            oldPos = new Vector2(this.POSITIONINFO.TRANSLATIONX, this.POSITIONINFO.TRANSLATIONY);
            Vector2 destination = new Vector2(oldPos.X, oldPos.Y);
            destination += velocity * gameTime.ElapsedGameTime.Milliseconds;

            Vector2 finalDestination = whereCanIGetTo(oldPos, destination, this.POSITIONINFO.BOUNDS);

            TranslateTo(finalDestination.X, finalDestination.Y);
        }

        private void checkSpringCollision()
        {
            List<Block> springs = SpriteManager.GAME.SPRINGS;

            for (int i = 0; i < springs.Count; i++)
            {
                //System.Diagnostics.Debug.WriteLine(springs[i].POSITIONINFO.TRANSLATIONY);
                if (Collision.IntersectsNonAA(this, springs[i]).X != -1)
                {
                    //springs[i].changeAnimation(1);
                    //hit a spring
                    peakHeight = POSITIONINFO.TRANSLATIONY;
                    velocity.Y = 0;
                    velocity += -Vector2.UnitY * springJumpPower;
                    jump.Play();
                }
            }
        }

        private void checkExitsCollision()
        {
            List<Block> exits = SpriteManager.GAME.EXITS;

            for (int i = 0; i < exits.Count; i++)
            {
                //System.Diagnostics.Debug.WriteLine(springs[i].POSITIONINFO.TRANSLATIONY);
                if (Collision.IntersectsNonAA(this, exits[i]).X != -1 && hasKey)
                {
                    if (!gameOver)
                    {
                        gameOver = true;
                        endGame.Play();
                        SpriteManager.GAME.endGame();
                    }
                }
            }
        }

        private void checkKeyCollision()
        {
            List<Block> key = SpriteManager.GAME.KEY;

            for (int i = 0; i < key.Count; i++)
            {
                if (Collision.IntersectsNonAA(this, key[i]).X != -1)
                {
                    this.hasKey = true;
                    this.keysTaken.Add(key[i]);
                    SpriteManager.GAME.SPRITEMANAGER.Remove(key[i]);
                    key.RemoveAt(i);
                    pickup.Play();
                }
            }
        }

        private void checkSpikeCollision()
        {
            if (!invulnerable)
            {
                List<Block> spikes = SpriteManager.GAME.SPIKES;

                for (int i = 0; i < spikes.Count; i++)
                {
                    if (Collision.IntersectsNonAA(this, spikes[i]).X != -1)
                    {
                        //move the player away somehow
                        velocity.Y = -1;
                        //remove a health
                        this.health--;
                        //make invulerable
                        this.invulnerable = true;
                        this.invulnerableTimer = 0;
                        hurt.Play();
                    }
                }
            }
        }

        private void checkEnemiesCollision()
        {
            if (!invulnerable)
            {
                List<Enemy> enemies = SpriteManager.GAME.ENEMIES;

                for (int i = 0; i < enemies.Count; i++)
                {
                    if (Collision.IntersectsNonAA(this, enemies[i]).X != -1)
                    {
                        //move the player away somehow
                        velocity *= -1;
                        //remove a health
                        this.health--;
                        //make invulerable
                        this.invulnerable = true;
                        this.invulnerableTimer = 0;
                        hurt.Play();
                    }
                }
            }
        }

        private void checkDestuctableCollision(GameTime gameTime)
        {
            if (!invulnerable)
            {
                List<Block> destructable = SpriteManager.GAME.DESTRUCTABLE;

                for (int i = 0; i < destructable.Count; i++)
                {
                     if (peakHeight + POSITIONINFO.BOUNDSHEIGHT < destructable[i].POSITIONINFO.BOUNDS.Top)
                    {
                        if (Collision.Intersects(this, destructable[i]))
                        {
                            //Console.Write("Health = " + destructable[i].HEALTH);

                            invulnerableTimer += gameTime.ElapsedGameTime.Milliseconds;
                            if (invulnerableTimer > 1000)
                            {
                                destructable[i].destructionFrame++;
                                destructable[i].PRESENTATIONINFO.changeSourceRectangle(destructable[i].destructionFrame, 0, 60, 60);
                                destructable[i].loseHealth();
                                //Console.Write("Health = " + destructable[i].HEALTH);

                                if (destructable[i].HEALTH == 0)
                                {

                                    List<Rectangle> shadows = SpriteManager.GAME.SHADOWCASTERS;
                                    for (int s = 0; s < shadows.Count; s++)
                                    {
                                        if (shadows[s].X == destructable[i].POSITIONINFO.TRANSLATIONX && shadows[s].Y == destructable[i].POSITIONINFO.TRANSLATIONY)
                                        {
                                            this.shadowsRemoved.Add(shadows[s]);
                                            shadows.Remove(shadows[s]);
                                            //keep track of anything you remove.
                                            
                                        }
                                    }
                                    destructable[i].destructionFrame = 0;
                                    destructable[i].PRESENTATIONINFO.changeSourceRectangle(destructable[i].destructionFrame, 0, 60, 60);
                                    destructable[i].HEALTH = 5;
                                    destructablesRemoved.Add(destructable[i]);
                                    SpriteManager.GAME.SPRITEMANAGER.Remove(destructable[i]);
                                    SpriteManager.GAME.DESTRUCTABLE.Remove(destructable[i]);
                                   
                                }
                                invulnerableTimer = 0;

                            }
                        }

                    }
                }
            }
        }

        private void manageInvulnrableTimer(GameTime gameTime)
        {
            this.invulnerableTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (invulnerableTimer > 1000)
            {
                invulnerable = false;
            }

        }

        private void processDeceleration()
        {
            //can play with these values, main contribution for capping speed.
            velocity *= new Vector2(.90f, .93f);

            if (velocity.X < .001 && velocity.X > -.001)
                velocity.X = 0;
            if (velocity.Y < .001 && velocity.Y > -.001)
                velocity.Y = 0;


        }

        private void processGravity(GameTime gameTime)
        {
            velocity.Y += gravity * gameTime.ElapsedGameTime.Milliseconds;
        }

        private Vector2 whereCanIGetTo(Vector2 originalPosition, Vector2 destination, Rectangle boundingRectangle)
        {

            Vector2 movementToTry = destination - originalPosition;
            Vector2 furthestAvailableLocationSoFar = originalPosition;
            int numberOfStepsToBreakMovementInto = (int)(movementToTry.Length() * 2) + 1;
            Vector2 oneStep = movementToTry / numberOfStepsToBreakMovementInto;

            for (int i = 1; i <= numberOfStepsToBreakMovementInto; i++)
            {
                Vector2 positionToTry = originalPosition + oneStep * i;
                SpriteShell newBoundary =
                    createRectangleAtPosition(positionToTry, boundingRectangle.Width, boundingRectangle.Height);
                Vector2 newPos = positionToTry;
                newPos.X += POSITIONINFO.BOUNDSWIDTH / 2;
                SpriteShell newWalkingLine =
                    createRectangleAtPosition(newPos, 1, POSITIONINFO.BOUNDSHEIGHT);
                if (!intersectsAnyBlock(newBoundary) && !intersectsAnyLines(newWalkingLine.bounds)) 
                { 
                    furthestAvailableLocationSoFar = positionToTry; 
                }
                else
                {
                    bool isDiagonalMove = movementToTry.X != 0 && movementToTry.Y != 0;
                    if (isDiagonalMove)
                    {
                        int stepsLeft = numberOfStepsToBreakMovementInto - (i - 1);

                        Vector2 remainingHorizontalMovement = oneStep.X * Vector2.UnitX * stepsLeft;
                        Vector2 finalPositionIfMovingHorizontally = furthestAvailableLocationSoFar + remainingHorizontalMovement;
                        furthestAvailableLocationSoFar =
                            whereCanIGetTo(furthestAvailableLocationSoFar, finalPositionIfMovingHorizontally, boundingRectangle);

                        Vector2 remainingVerticalMovement = oneStep.Y * Vector2.UnitY * stepsLeft;
                        Vector2 finalPositionIfMovingVertically = furthestAvailableLocationSoFar + remainingVerticalMovement;
                        furthestAvailableLocationSoFar =
                            whereCanIGetTo(furthestAvailableLocationSoFar, finalPositionIfMovingVertically, boundingRectangle);
                    }
                    break;
                }
            }
            
            return furthestAvailableLocationSoFar;
        }

        private SpriteShell createRectangleAtPosition(Vector2 positionToTry, int width, int height)
        {
            Rectangle temp = new Rectangle((int)positionToTry.X, (int)positionToTry.Y, width, height);

            return new SpriteShell(Collision.getSectorNumber(temp), temp);
        }

        private Line createLineAtPos(Vector2 positionToTry, float endY)
        {
            Line temp = new Line(positionToTry, new Vector2(positionToTry.X, positionToTry.Y + endY));

            return temp;
        }

        //Only rectangle collision?
        private bool intersectsAnyBlock(SpriteShell rectangleToCheck)
        {
            List<Block> collidables = SpriteManager.GAME.COLLIDABLES;
            List<Block> solidCollidables = SpriteManager.GAME.SOLIDCOLLIDABLES;
            List<Block> destructables = SpriteManager.GAME.DESTRUCTABLE;
            //System.Diagnostics.Debug.WriteLine(walkingLine.start + " : " + walkingLine.end);
            for (int i = 0; i < collidables.Count; i++)
            {
                if (peakHeight + POSITIONINFO.BOUNDSHEIGHT < collidables[i].POSITIONINFO.BOUNDS.Top)
                {
                    if (Collision.Intersects(rectangleToCheck.bounds, Line.getTopLine(collidables[i].POSITIONINFO.BOUNDS)))
                        return true;
                }
            }

            for (int i = 0; i < destructables.Count; i++)
            {

                if (Collision.Intersects(rectangleToCheck, destructables[i]))
                    return true;

            }

            //if you leave out the peak high stuff, it just performs a rectangle collision.
            //the thing passed in is a SpriteShell (with a bsp sector # etc) so we cover BSP aswel.
            for (int i = 0; i < solidCollidables.Count; i++)
            {
                if(Collision.Intersects(rectangleToCheck, solidCollidables[i]))
                    return true;
            }
            
            
            return false;
        }

        private bool intersectsAnyLines(Rectangle rectangleToCheck)
        {
            //added -1
            
            for (int i = 0; i < SpriteManager.GAME.shadowCastManager.contactLines.Count; i++)
            {
                if (peakHeight + POSITIONINFO.BOUNDSHEIGHT < SpriteManager.GAME.shadowCastManager.contactLines[i].findPointOfX(POSITIONINFO.TRANSLATIONX + (POSITIONINFO.BOUNDSWIDTH / 2)).Y)
                {
                    if (Collision.Intersects(rectangleToCheck, SpriteManager.GAME.shadowCastManager.contactLines[i]))
                    {
                        lastLineContact = SpriteManager.GAME.shadowCastManager.contactLines[i];
                        return true;
                    }
                }
            }
            return false;
        }

        private bool canJump()
        {
            Rectangle onePixelLower = this.POSITIONINFO.BOUNDS;
            //fixes a bug. Seems to act like it is 1px wider than it should be
            onePixelLower.Width -= 1;
            onePixelLower.Offset(0, 1);

            Rectangle walkingRect = new Rectangle((int)walkingLine.start.X, (int)walkingLine.start.Y, 1, POSITIONINFO.BOUNDSHEIGHT + 1);

            bool blocksCheck = intersectsAnyBlock(new SpriteShell(Collision.getSectorNumber(onePixelLower), onePixelLower));
            bool lineCheck = intersectsAnyLines(walkingRect);
            
            //if either can jump, you can jump.
            if (!blocksCheck && !lineCheck)
                return false;
            else
                return true;
        }

        private void checkLineContact()
        {
            SpriteShell newWalkingLine =
                    createRectangleAtPosition(new Vector2(POSITIONINFO.TRANSLATIONX + (POSITIONINFO.BOUNDSWIDTH / 2), POSITIONINFO.TRANSLATIONY + 1), 1, POSITIONINFO.BOUNDSHEIGHT);
            lineContact = intersectsAnyLines(newWalkingLine.bounds);

            if (lineContact)
                peakHeight = POSITIONINFO.TRANSLATIONY -5;
        }

        private void stopMovingIfBlocked()
        {
            Vector2 lastMovement = this.POSITIONINFO.TRANSLATION - oldPos;
            if (lastMovement.X == 0) { velocity *= Vector2.UnitY; }
            if (lastMovement.Y == 0) { velocity *= Vector2.UnitX; }
        }

        private void keepInsideLevelBounds()
        {
            //assuming the player isnt big enough to hit both sides of a level, (right / left) or (top / bottom)
            if (this.POSITIONINFO.TRANSLATIONX > GameData.LEVEL_WIDTH - this.POSITIONINFO.BOUNDSWIDTH)                  //Right side
                this.TranslateTo(GameData.LEVEL_WIDTH - this.POSITIONINFO.BOUNDSWIDTH, this.POSITIONINFO.TRANSLATIONY);
            else if (this.POSITIONINFO.TRANSLATIONX < 0)                                                                //Left side
                this.TranslateTo(0, this.POSITIONINFO.TRANSLATIONY);
            
            if (this.POSITIONINFO.TRANSLATIONY > GameData.LEVEL_HEIGHT - this.POSITIONINFO.BOUNDSHEIGHT)                 //Bottom
                this.TranslateTo(this.POSITIONINFO.TRANSLATIONX, GameData.LEVEL_HEIGHT - this.POSITIONINFO.BOUNDSHEIGHT);
            else if (this.POSITIONINFO.TRANSLATIONY < 0)
                this.TranslateTo(this.POSITIONINFO.TRANSLATIONX, 0);                                                    //Top
        }

        private void updateAnimationDirections()
        {
            if (velocity.X > 0)
                this.PRESENTATIONINFO.SPRITEEFFECTS = SpriteEffects.None;
            else if (velocity.X < 0)
                this.PRESENTATIONINFO.SPRITEEFFECTS = SpriteEffects.FlipHorizontally;
        }

        //small wrapper to fix a bug
        private void changePlayerAnimation(int index)
        {
            //TranslateBy(0, -1);
            changeAnimation(index);
        }

        public void resetGame()
        {
            resetPlayer();
            resetLevel();
            SpriteManager.GAME.BACKGROUNDMUSIC = SpriteManager.GAME.SOUNDMANAGER.getEffectInstance("backgroundMusic");
        }

        private void resetPlayer()
        {
            this.TranslateTo(levelStartPos.X, levelStartPos.Y);
            this.health = 4;
            this.hasKey = false;
            this.coins = 0;
            this.velocity = Vector2.Zero;
        }

        private void resetLevel()
        {
            List<Block> keys = SpriteManager.GAME.KEY;
            List<Block> coins = SpriteManager.GAME.COINS;

            for (int i = 0; i < keysTaken.Count; i++)
            {
                keys.Add(keysTaken[i]);
                SpriteManager.GAME.SPRITEMANAGER.Add(keysTaken[i]);
            }

            //put all the blocks back.
            for (int i = 0; i < destructablesRemoved.Count; i++)
            {
                SpriteManager.GAME.DESTRUCTABLE.Add(destructablesRemoved[i]);
                SpriteManager.GAME.SPRITEMANAGER.Add(destructablesRemoved[i]);
            }

            //List<Rectangle> shadows = SpriteManager.GAME.SHADOWCASTERS;
            for (int i = 0; i < shadowsRemoved.Count; i++)
            {
                SpriteManager.GAME.addShadow(shadowsRemoved[i]);
            }

            //make sure all the blocks are on full health.
            for (int i = 0; i < SpriteManager.GAME.DESTRUCTABLE.Count; i++)
            {
                SpriteManager.GAME.DESTRUCTABLE[i].HEALTH = 5;
                SpriteManager.GAME.DESTRUCTABLE[i].destructionFrame = 0;
                SpriteManager.GAME.DESTRUCTABLE[i].PRESENTATIONINFO.changeSourceRectangle(SpriteManager.GAME.DESTRUCTABLE[i].destructionFrame, 0, 60, 60);
            }

            shadowsRemoved.Clear();
            destructablesRemoved.Clear();
            //keysTaken.Clear();
           
            gameOver = false;
        }

        private void checkForDeath()
        {
            if (this.health <= 0)
            {
                //play some death sound
                resetGame();
                //SpriteManager.GAME.BACKGROUNDMUSIC.Play();
            }
        }

        private void updatePeakHeight()
        {
            if (canJump())
            {
                peakHeight = POSITIONINFO.TRANSLATIONY-5; //made a hacky fix here.
            }
            else
            {
                if (POSITIONINFO.TRANSLATIONY < peakHeight)
                    peakHeight = POSITIONINFO.TRANSLATIONY;
            }
        }
    }
}
