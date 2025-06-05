using UnityEngine;

public static class AchievementManager
{
    //TODO: Level design
    //TODO: Set achievement from player
    #region ==========Categorize==========
    public const ushort ACHIEVEMENT_GOLDS = 0x0001;
    public const ushort ACHIEVEMENT_CROP = 0x0002;
    public const ushort ACHIEVEMENT_TOOL = 0x0003;
    #endregion
    #region ==========Structure==========
    public struct Achievement
    {
        public readonly ushort Category { get; }
        public readonly AchievementID AchievementID { get; }
        public readonly ushort Stage => (ushort)(stage + 1);
        public uint Value
        {
            readonly get => value;
            set
            {
                this.value = value;
                if (value >= threshold[stage])
                {
                    stage = (ushort)Mathf.Clamp(stage + 1, 0, threshold.Length - 1);
                }
            }
        }

        private readonly uint[] threshold;
        private ushort stage;
        private uint value;

        public Achievement(ushort category, AchievementID achievementID, uint[] threshold)
        {
            Category = category;
            AchievementID = achievementID;
            stage = 0;
            value = 0;
            this.threshold = threshold;
        }
    }
    #endregion

    #region ==========Levels==========
    //Golds
    public static Achievement EarnedTotalGolds { get; } = new(ACHIEVEMENT_GOLDS, AchievementID.EarnedTotalGolds, new uint[] { 1000, 5000, 10000, 20000, 50000 });
    public static Achievement EarnedDailyGolds { get; } = new(ACHIEVEMENT_GOLDS, AchievementID.EarnedDailyGolds, new uint[] { 100, 500, 1000, 2000, 5000 });
    public static Achievement EarnedFarmaingGolds { get; } = new(ACHIEVEMENT_GOLDS, AchievementID.EarnedFarmingGolds, new uint[] { 100, 500, 1000, 2000, 5000 });
    public static Achievement EarnedAnimalGolds { get; } = new(ACHIEVEMENT_GOLDS, AchievementID.EarnedAnimalGolds, new uint[] { 100, 500, 1000, 2000, 5000 });
    public static Achievement EarnedMiscGolds { get; } = new(ACHIEVEMENT_GOLDS, AchievementID.EarnedMiscGolds, new uint[] { 100, 500, 1000, 2000, 5000 });
    public static Achievement CurrentGolds { get; } = new(ACHIEVEMENT_GOLDS, AchievementID.CurrentGolds, new uint[] { 1000, 5000, 10000, 20000, 50000 });
    public static Achievement SpentGolds { get; } = new(ACHIEVEMENT_GOLDS, AchievementID.SpentGolds, new uint[] { 100, 500, 1000, 2000, 5000 });

    //Crop
    public static Achievement HarvestedTotalCrop { get; } = new(ACHIEVEMENT_CROP, AchievementID.HarvestedTotalCrop, new uint[] { 100, 500, 1000, 2000, 5000 });
    public static Achievement HarvestedCarrot { get; } = new(ACHIEVEMENT_CROP, AchievementID.HarvestedCarrot, new uint[] { 100, 500, 1000, 2000, 5000 });
    public static Achievement HarvestedEggplant { get; } = new(ACHIEVEMENT_CROP, AchievementID.HarvestedEggplant, new uint[] { 100, 500, 1000, 2000, 5000 });

    //Tool
    public static Achievement UsedTool { get; } = new(ACHIEVEMENT_TOOL, AchievementID.UsedTool, new uint[] { 100, 500, 1000, 2000, 5000 });
    public static Achievement UsedAxe { get; } = new(ACHIEVEMENT_TOOL, AchievementID.UsedAxe, new uint[] { 100, 500, 1000, 2000, 5000 });
    public static Achievement UsedHoe { get; } = new(ACHIEVEMENT_TOOL, AchievementID.UsedHoe, new uint[] { 100, 500, 1000, 2000, 5000 });
    public static Achievement UsedWateringCan { get; } = new(ACHIEVEMENT_TOOL, AchievementID.UsedWateringCan, new uint[] { 100, 500, 1000, 2000, 5000 });
    public static Achievement FilledWateringCan { get; } = new(ACHIEVEMENT_TOOL, AchievementID.FilledWateringCan, new uint[] { 100, 500, 1000, 2000, 5000 });

    #endregion

    #region ==========Unity Methods==========

    #endregion

    #region ==========Methods==========

    #endregion
}
