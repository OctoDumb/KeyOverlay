using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace KeyOverlay
{
    class App
    {
        private Config config = new Config();
        private bool reinit = false;

        private RenderWindow window;
        private RenderTexture texture;
        private Shader shader;
        private RenderStates states;

        private Vector2i grabbedOffset;
        private bool grabbed = false;

        private List<Key> keys = new List<Key>();

        public App() { }

        private void load()
        {
            config.LoadConfig();

            window = new RenderWindow(new VideoMode(config.Window.Width, config.Window.Height), "KeyOverlay", config.Window.Transparent ? Styles.None : (Styles.Titlebar | Styles.Close));
            window.SetFramerateLimit(config.Window.FPS);
            window.Closed += OnClose;
            window.KeyPressed += OnKeyPress;
            window.MouseButtonPressed += OnMousePress;
            window.MouseButtonReleased += OnMouseRelease;
            window.MouseMoved += OnMouseMove;

            keys.Clear();

            foreach(KeysConfig key in config.Keys)
            {
                var _key = new Key(key.Name, key.Key, config);
                keys.Add(_key);
            }

            if(config.Window.Transparent)
            {
                // https://gist.github.com/Alia5/5d8c48941d1f73c1ef14967a5ffe33d5
                SetWindowLong(window.SystemHandle, -16, 0x80000000 | 0x10000000);
                MARGINS margins = new MARGINS { cyTopHeight = -1 };
                DwmExtendFrameIntoClientArea(window.SystemHandle, margins);
            }

            texture = new RenderTexture(config.Window.Width, config.Window.Height);

            string shaderText = File.ReadAllText("./shader.frag");
            shader = Shader.FromString(null, null, shaderText);
            shader.SetUniform("resolution", new Vector2f(config.Window.Width, config.Window.Height));
            shader.SetUniform("size", config.Fade.Size);
            shader.SetUniform("side", config.Fade.Side);

            states = new RenderStates(shader);
        }

        private void createKeys()
        {

        }

        private void OnClose(object sender, EventArgs e)
        {
            window.Close();
        }

        private void OnKeyPress(object sender, KeyEventArgs e)
        {
            switch(e.Code)
            {
                case Keyboard.Key.Escape:
                    window.Close();
                    break;
                case Keyboard.Key.R:
                    window.Close();
                    reinit = true;
                    break;
                default:
                    //
                    break;
            }
        }

        private void OnMousePress(object sender, MouseButtonEventArgs e)
        {
            if(e.Button == Mouse.Button.Left)
            {
                grabbedOffset = window.Position - Mouse.GetPosition();
                grabbed = true;
            }
        }

        private void OnMouseRelease(object sender, MouseButtonEventArgs e)
        {
            if(e.Button == Mouse.Button.Left)
                grabbed = false;
        }

        private void OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            if(grabbed)
                window.Position = Mouse.GetPosition() + grabbedOffset;
        }

        public void Run()
        {
            reinit = false;

            load();

            while(window.IsOpen && !reinit)
            {
                texture.Clear(Color.Transparent);

                Vector2f offset = new Vector2f(
                    (config.Window.Width - config.Key.Size * keys.Count - config.Key.Margin * (keys.Count - 1)) / 2,
                    20 + config.Key.Size
                );

                foreach(Key key in keys)
                {
                    offset = key.Draw(texture, offset);
                }

                texture.Display();

                var t = texture.Texture;
                shader.SetUniform("texture", t);
                var sprite = new Sprite(t);

                window.Clear(Color.Transparent);
                window.DispatchEvents();

                window.Draw(sprite, states);
                window.Display();
            }

            if(reinit)
                Run();
        }

        #region Imports

        struct MARGINS
        {
            public int cxLeftWidth;
            public int cxRightWidth;
            public int cyTopHeight;
            public int cyBottomHeight;
        }

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hwnd, int index, uint newlong);
        [DllImport("Dwmapi.dll")]
        static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, MARGINS margins);

        #endregion
    }
}