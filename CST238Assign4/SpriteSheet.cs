// SpriteSheetExample
// by Pete Myers
// OIT, Spring 2017

using System;
using System.Drawing;

namespace CST238Assign4
{
    public class SpriteSheet
    {
        // properties of sprite sheet
        private int rowCount;
        private int columnCount;
        private int frameWidth;
        private int frameHeight;
        private Image sheet;
        private int frameCount;

        // current state of animation
        private int currentFrame;
        private Rectangle currentFrameRect;

        public SpriteSheet(Image sheet, int frameCount, int rowCount, int columnCount, int frameWidth, int frameHeight)
        {
            this.sheet = sheet;
            this.frameCount = frameCount;
            this.rowCount = rowCount;
            this.columnCount = columnCount;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;

            currentFrame = 0;
            currentFrameRect = CreateFrameRectangle();
        }

        public void DrawCurrentFrame(Graphics g, Rectangle dest)
        {
            g.DrawImage(sheet, dest, currentFrameRect, GraphicsUnit.Pixel);
        }

        public void SetCurrentFrame(int frame)
        {
            // SetCurrentFrame() method allows you to choose which frame you want to show next

            if (frame < 0 || frame >= frameCount)
                throw new Exception("Invalid frame index");

            currentFrame = frame;
            currentFrameRect = CreateFrameRectangle();
        }

        public void NextFrame()
        {
            // NextFrame() method advances automatically to the next frame

            currentFrame = (currentFrame + 1) % frameCount;
            currentFrameRect = CreateFrameRectangle();
        }

        private Rectangle CreateFrameRectangle()
        {
            // find the current frame in the spritesheet
            return new Rectangle(
                (currentFrame % columnCount) * frameWidth,
                (currentFrame / columnCount) * frameHeight,
                frameWidth,
                frameHeight);
        }

        public bool Done => (currentFrame == 0);
    }
}
