using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace NameCore
{

    [Serializable]
    public class GameData
    {
        public PlayerResources resources = new();
        public GameSetting settings = new();
        public PlayerProgress progress = new();
    }
    [Serializable]
    public class PlayerResources 
    {
        public List<ResourceEntry> entries = new();

        public int Get(ResoucrType type)
        {
            return entries.Find(e => e.type == type)?.amount ?? 0;

        }

        public void Set(ResoucrType type, int amount)
        {
            var entry = entries.Find(e => e.type == type);
            if (entry != null)
                entry.amount += amount;
            else
                entries.Add(new ResourceEntry { type = type, amount = amount });

            DataManager.Instance.SaveData();
        }

        public void Add(ResoucrType type, int delta)
        {
            Set(type,Get(type)+ delta);
        }
    }

    [Serializable]
    public class GameSetting
    {
        public float mussicVolume = 0.5f;
        public float soundVolume = 1f;
        public bool vibration = true;
    }
    [Serializable]
    public class PlayerProgress
    {
        public int currentLevel = 0;
        public List<LevelData> levels = new();
        public int GetStarForLevel(int level)
        {
            return levels.Find(l => l.levelID == level)?.stars ?? 0;
        }

        public void CompleteLevel(int levelId, int starEarned)
        {
            var progress = DataManager.Instance.Data.progress;

            // Tìm hoặc tạo mới tiến trình cho level hiện tại
            var current = progress.levels.Find(l => l.levelID == levelId);
            if (current == null)
            {
                current = new LevelData
                {
                    levelID = levelId,
                    stars = starEarned,
                    isUnlocked = true
                };
                progress.levels.Add(current);
            }
            else
            {
                // Cập nhật số sao nếu cao hơn
                current.stars = Mathf.Max(current.stars, starEarned);
            }

            // Mở khóa level tiếp theo
            var next = progress.levels.Find(l => l.levelID == levelId + 1);
            if (next == null)
            {
                progress.levels.Add(new LevelData
                {
                    levelID = levelId + 1,
                    isUnlocked = true,
                    stars = 0
                });
            }
            else
            {
                next.isUnlocked = true;
            }

            // Cập nhật level hiện tại
            progress.currentLevel = levelId;

            DataManager.Instance.SaveData();
        }
        public bool IsLevelUnlocked(int levelId)
        {
            var level = DataManager.Instance.Data.progress.levels.Find(l => l.levelID == levelId);
            return level != null && level.isUnlocked;
        }
    }
    [Serializable]
    public class LevelData
    {
        public int levelID;
        public int stars;
        public bool isUnlocked;
    }

    [Serializable]

    public class ResourceEntry
    {
        public ResoucrType type;
        public int amount;
    }
    public enum ResoucrType
    {
        coin,
        diamod,
        tiket,
        energi,
        ruby,
        key,
        star,
        heart,
        exp,
        daylyToken,
    }
}

