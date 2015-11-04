using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;


namespace KeysToInsanity
{
    public class Animation
    {
        List<AnimationFrame> frames = new List<AnimationFrame>();
        TimeSpan timeIntoAnimation;

        TimeSpan Duration
        {
            get
            {
                double totalSeconds = 0;
                foreach (var frame in frames)
                {
                    totalSeconds += frame.Duration.TotalSeconds;
                }

                return TimeSpan.FromSeconds(totalSeconds);
            }
        }

        public void AddFrame(Rectangle rectangle, TimeSpan duration)
        {
            AnimationFrame newFrame = new AnimationFrame()
            {
                SourceRectangle = rectangle,
                Duration = duration
            };

            frames.Add(newFrame);
        }

        public void AddUniformStrip(Rectangle stripRectangle, Point frameSize, TimeSpan frameDuration)
        {
            int size = stripRectangle.Width / frameSize.X;
            for(int i = 0; i < size; i++)
            {
                frames.Add(new AnimationFrame()
                {
                    SourceRectangle = new Rectangle((frameSize.X * i) + stripRectangle.X, stripRectangle.Y, frameSize.X, frameSize.Y),
                    Duration = frameDuration
                });
            }
        }

        public void AddUniformHeightStrip(Rectangle stripRectangle, int[] widths, TimeSpan frameDuration)
        {
            for(int i = 0; i < widths.Count(); i++)
            {
                frames.Add(new AnimationFrame()
                {
                    SourceRectangle = new Rectangle((widths[i] * i) + stripRectangle.X, stripRectangle.Y, widths[i], stripRectangle.Height),
                    Duration = frameDuration
                });
            }
        }

        public void Update(GameTime gameTime)
        {
            double secondsIntoAnimation =
                timeIntoAnimation.TotalSeconds + gameTime.ElapsedGameTime.TotalSeconds;

            double remainder = secondsIntoAnimation % Duration.TotalSeconds;

            timeIntoAnimation = TimeSpan.FromSeconds(remainder);
        }

        public Rectangle CurrentRectangle
        {
            get
            {
                AnimationFrame currentFrame = null;

                //See if we can find the frame
                TimeSpan accumulatedTime = new TimeSpan();
                for(int i = 0; i < frames.Count; i++)
                {
                    if (accumulatedTime + frames[i].Duration >= timeIntoAnimation)
                    {
                        currentFrame = frames[i];
                        break;
                    }
                    else
                    {
                        accumulatedTime += frames[i].Duration;
                    }
                }
                /*If no frame was found, then we ty the last frame,
             in case if timeIntoDuration exceeds Duration*/
                if (currentFrame == null)
                {
                    currentFrame = frames.LastOrDefault();
                }
                if (currentFrame != null)
                {
                    return currentFrame.SourceRectangle;
                }
                else
                {
                    return Rectangle.Empty;
                }
            }
        }
    }
}
