using System;
using System.IO;
using Newtonsoft.Json;
using SFML.Graphics;

namespace KeyOverlay
{
    struct WindowConfig
    {
        [JsonProperty("width")]
        public uint Width;
        [JsonProperty("height")]
        public uint Height;
        [JsonProperty("transparent")]
        public bool Transparent;
        [JsonProperty("fps")]
        public uint FPS;
    }

    struct HoldKeyConfig
    {
        [JsonProperty("color")]
        public Color Color;
        [JsonProperty("size")]
        public float Size;
        [JsonProperty("time")]
        public uint Time;
    }

    struct KeyConfig
    {
        [JsonProperty("border")]
        public Color Border;
        [JsonProperty("background")]
        public Color Background;
        [JsonProperty("text")]
        public Color Text;
        [JsonProperty("fill")]
        public Color Fill;
        [JsonProperty("font")]
        public string Font;
        [JsonProperty("border_size")]
        public float BorderSize;
        [JsonProperty("size")]
        public float Size;
        [JsonProperty("margin")]
        public float Margin;
        [JsonProperty("text_size")]
        public uint TextSize;
        [JsonProperty("rounded")]
        public bool Rounded;
        [JsonProperty("radius")]
        public float Radius;
        [JsonProperty("hold")]
        public HoldKeyConfig Hold;
    }

    struct KeysConfig
    {
        [JsonProperty("name")]
        public string Name;
        [JsonProperty("key")]
        public string Key;
    }

    struct FadeConfig
    {
        [JsonProperty("size")]
        public float Size;
        [JsonProperty("side")]
        public uint Side;
    }

    struct ConfigData
    {
        [JsonProperty("window")]
        public WindowConfig Window;
        [JsonProperty("key")]
        public KeyConfig Key;
        [JsonProperty("keys")]
        public KeysConfig[] Keys;
        [JsonProperty("fade")]
        public FadeConfig Fade;
    }

    class Config
    {
        private ConfigData data;

        public WindowConfig Window {
            get {
                return data.Window;
            }
        }

        public KeyConfig Key {
            get {
                return data.Key;
            }
        }

        public KeysConfig[] Keys {
            get {
                return data.Keys;
            }
        }

        public FadeConfig Fade {
            get {
                return data.Fade;
            }
        }

        public Config() {}

        public void LoadConfig()
        {
            string text = File.ReadAllText(@"config.json");
            data = JsonConvert.DeserializeObject<ConfigData>(text, new ColorConverter());
        }
    }
}