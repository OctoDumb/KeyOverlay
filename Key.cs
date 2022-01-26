using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace KeyOverlay {
    class Key
    {
        private string name;

        private RenderTexture texture;
        public Texture Texture {
            get {
                return texture.Texture;
            }
        }

        public readonly Texture Pressed;

        private Keyboard.Key key;
        private int counter;

        public int Counter {
            get {
                return counter;
            }
        }

        private Config config;

        private bool isPressed = false;

        private List<KeyHold> holds = new List<KeyHold>();

        public Key(string name, string k, Config config)
        {
            this.name = name;
            this.config = config;

            Shape shape;
            if(config.Key.Rounded)
            {
                shape = new RoundedRectangle(
                    config.Key.Size - config.Key.BorderSize * 2, 
                    config.Key.Size - config.Key.BorderSize * 2, 
                    config.Key.Radius
                );
            }
            else
            {
                shape = new RectangleShape(
                    new Vector2f(
                        config.Key.Size - config.Key.BorderSize * 2, 
                        config.Key.Size - config.Key.BorderSize * 2
                    )
                );
            }
            
            shape.FillColor = new Color(0, 0, 0, 0);
            shape.OutlineColor = config.Key.Border;
            shape.OutlineThickness = config.Key.BorderSize;
            shape.Position = new Vector2f(
                config.Key.BorderSize,
                config.Key.BorderSize
            );

            Font font = new Font(config.Key.Font);
            Text text = new Text(name, font);
            text.CharacterSize = config.Key.TextSize;
            text.FillColor = config.Key.Text;
            FloatRect textBounds = text.GetGlobalBounds();
            text.Position = new Vector2f(
                (config.Key.Size - textBounds.Width) / 2 - textBounds.Left,
                (config.Key.Size - textBounds.Height) / 2 - textBounds.Top
            );

            texture = new RenderTexture((uint)Math.Ceiling(config.Key.Size), (uint)Math.Ceiling(config.Key.Size));
            texture.Clear(Color.Transparent);
            texture.Draw(shape);
            texture.Draw(text);

            var pressed = new RenderTexture((uint)Math.Ceiling(config.Key.Size), (uint)Math.Ceiling(config.Key.Size));
            pressed.Clear(Color.Transparent);
            shape.FillColor = config.Key.Fill;
            shape.OutlineThickness = 0;
            pressed.Draw(shape);

            Pressed = new Texture(pressed.Texture);

            if(!Enum.TryParse(k, out key))
            {
                throw new ArgumentException($"Invalid key: {k}");
            }
        }

        public Vector2f Draw(RenderTexture t, Vector2f offset)
        {
            var timeNow = Util.GetMillisecondsNow();
            holds = holds.Where((hold) => hold.End > (timeNow - config.Key.Hold.Time)).ToList();

            Sprite sprite;
            if(IsPressed)
            {
                sprite = new Sprite(Pressed);
                sprite.Position = offset;
                t.Draw(sprite);
            }
            sprite = new Sprite(Texture);
            sprite.Position = offset;
            t.Draw(sprite);

            foreach (var hold in holds)
            {
                if(hold.Length == 0) continue;
                var holdShape = new RectangleShape(new Vector2f(
                    config.Key.Hold.Size,
                    hold.Length / config.Key.Hold.Time * (config.Window.Height - offset.Y)
                ));

                holdShape.FillColor = config.Key.Hold.Color;
                holdShape.Position = offset + new Vector2f(
                    (config.Key.Size - config.Key.Hold.Size) / 2,
                    config.Key.Size + (float)(timeNow - hold.End) / config.Key.Hold.Time * (config.Window.Height - offset.Y)
                    // (hold.Start - timeNow) / config.Key.Hold.Time * offset.Y
                );
                t.Draw(holdShape);
            }

            return offset + new Vector2f(texture.Size.X + config.Key.Margin, 0);
        }

        public bool IsPressed
        {
            get {
                var isPressedNow = Keyboard.IsKeyPressed(key);
                if(isPressedNow && !isPressed)
                {
                    counter++;
                    holds.Add(new KeyHold());
                }
                else if(isPressed && !isPressedNow)
                {
                    holds.Last().Stop();
                }
                isPressed = isPressedNow;
                return isPressed;
            }
        }

        private class KeyHold
        {
            public long Start;
            private long end = 0;
            public long End {
                get {
                    return end == 0 ? Util.GetMillisecondsNow() : end;
                }
            }

            public float Length {
                get {
                    return End - Start;
                }
            }

            public KeyHold()
            {
                Start = Util.GetMillisecondsNow();
            }

            public void Stop()
            {
                if(end > 0) return;
                end = Util.GetMillisecondsNow();
            }
        }
    }
}
