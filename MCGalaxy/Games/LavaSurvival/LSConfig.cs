﻿/*
    Copyright 2011 MCForge
        
    Dual-licensed under the Educational Community License, Version 2.0 and
    the GNU General Public License, Version 3 (the "Licenses"); you may
    not use this file except in compliance with the Licenses. You may
    obtain a copy of the Licenses at
    
    http://www.opensource.org/licenses/ecl2.php
    http://www.gnu.org/licenses/gpl-3.0.html
    
    Unless required by applicable law or agreed to in writing,
    software distributed under the Licenses are distributed on an "AS IS"
    BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
    or implied. See the Licenses for the specific language governing
    permissions and limitations under the Licenses.
 */
using System;
using System.IO;
using System.Timers;
using MCGalaxy.Commands;
using MCGalaxy.Maths;
using BlockID = System.UInt16;
using MCGalaxy.Config;
using System.Collections.Generic;

namespace MCGalaxy.Games {
    
    public sealed class LSConfig {
        [ConfigBool("start-on-startup", null, false)]
        public bool StartImmediately;
        [ConfigInt("lives", null, 3, 0)]
        public int MaxLives = 3;
        [ConfigStringList("maps", null)]
        public List<string> Maps = new List<string>();
        
        static ConfigElement[] cfg;
        const string propsFile = "properties/lavasurvival.properties";
        
        public void Save() {
            if (cfg == null) cfg = ConfigElement.GetAll(typeof(LSConfig));
            ConfigElement.SerialiseSimple(cfg, propsFile, this);
        }
        
        public void Load() {
            if (cfg == null) cfg = ConfigElement.GetAll(typeof(LSConfig));
            ConfigElement.ParseFile(cfg, "Lava survival", propsFile, this);
        }
    }
    
    public sealed class LSMapConfig {
        [ConfigInt("fast-chance", null, 0, 0, 100)]
        public int FastChance;
        [ConfigInt("killer-chance", null, 100, 0, 100)]
        public int KillerChance = 100;
        [ConfigInt("destroy-chance", null, 0, 0, 100)]
        public int DestroyChance;
        [ConfigInt("water-chance", null, 0, 0, 100)]
        public int WaterChance;
        [ConfigInt("layer-chance", null, 0, 0, 100)]
        public int LayerChance;
        
        [ConfigInt("layer-height", null, 3, 0)]
        public int LayerHeight = 3;
        [ConfigInt("layer-count", null, 10, 0)]
        public int LayerCount = 10;
        
        [ConfigReal("layer-interval", null, 2, 0)]
        public float LayerIntervalMins = 2;
        [ConfigReal("round-time", null, 15, 0)]
        public float RoundTimeMins = 15;
        [ConfigReal("flood-time", null, 5, 0)]
        public float FloodTimeMins = 5;
        
        [ConfigVec3("block-flood", null)] public Vec3U16 FloodPos;
        [ConfigVec3("block-layer", null)] public Vec3U16 LayerPos;
        [ConfigVec3("safe-zone-min", null)] public Vec3U16 SafeZoneMin;
        [ConfigVec3("safe-zone-max", null)] public Vec3U16 SafeZoneMax;
        
        
        const string propsDir = "properties/lavasurvival/";
        static string Path(string map) { return propsDir + map + ".properties"; }
        static ConfigElement[] cfg;
        
        public void Load(string map) {
            if (cfg == null) cfg = ConfigElement.GetAll(typeof(LSMapConfig));
            ConfigElement.ParseFile(cfg, "Lava survival map", Path(map), this);
        }
        
        public void Save(string map) {
            if (!Directory.Exists(propsDir)) Directory.CreateDirectory(propsDir);
            
            if (cfg == null) cfg = ConfigElement.GetAll(typeof(LSMapConfig));
            ConfigElement.SerialiseSimple(cfg, Path(map), this);
        }
        
        public void SetDefaults(Level lvl) {
            ushort x = (ushort)(lvl.Width / 2), y = (ushort)(lvl.Height / 2), z = (ushort)(lvl.Length / 2);
            FloodPos = new Vec3U16(x, (ushort)(lvl.Height - 1), z);
            LayerPos = new Vec3U16(0, y                       , 0);
            
            SafeZoneMin = new Vec3U16((ushort)(x - 3), y,               (ushort)(z - 3));
            SafeZoneMax = new Vec3U16((ushort)(x + 3), (ushort)(y + 4), (ushort)(z + 3));
        }
    }
}
