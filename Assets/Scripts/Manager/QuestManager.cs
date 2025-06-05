using System;
using System.Collections.Generic;
using UnityEngine;

public static class QuestManager
{
    #region ==========Fields==========
    public static Quest[] Quests => quests.ToArray();
    private static List<Quest> quests = new List<Quest>();
    public static Action<ActionID, int> OnQuestProceed;

    public static readonly Color Color_NotRead = new Color(0.5f, 0.5f, 0.5f, 1f);
    public static readonly Color Color_Read = new Color(1f, 1f, 1f, 1f);
    public static readonly Color Color_Clear = new Color(0.5f, 1f, 0.5f, 1f);

    //Tool
    private static int level_UseTool = 0;
    private static int level_CutTree = 0;
    private static int level_Plow = 0;
    private static int level_Watering = 0;

    //Plant
    private static int level_Plant = 0;
    private static int level_PlantCarrot = 0;
    private static int level_PlantEggplant = 0;

    //Harvest
    private static int level_Harvest = 0;
    private static int level_HarvestCarrot = 0;
    private static int level_HarvestEggplant = 0;

    //Animal
    private static int level_FeedAnimal = 0;
    private static int level_FeedChicken = 0;
    private static int level_FeedCow = 0;

    private static int level_GetAnimalProduct = 0;
    private static int level_GetEgg = 0;
    private static int level_GetMilk = 0;

    public static Quest FirstQuest = new Quest(ActionID.Harvest, "첫 수확", "첫 작물을 수확해보자!", 1, 300);
    #endregion

    #region ==========Methods==========
    public static void AddQuest(Quest quest)
    {
        if (quest == null) return;
        quests.Add(quest);
    }

    public static void RemoveQuest(Quest quest)
    {
        if (quest == null) return;
        quest.OnClear();
        quests.Remove(quest);
    }

    public static Quest AutoQuestGenerator(ActionID action)
    {
        return action switch
        {
            ActionID.Plant => PlantQuest(),
            ActionID.PlantCarrot => PlantCarrotQuest(),
            ActionID.PlantEggplant => PlantEggplantQuest(),
            ActionID.Harvest => HarvestQuest(),
            ActionID.HarvestCarrot => HarvestCarrotQuest(),
            ActionID.HarvestEggplant => HarvestEggplantQuest(),
            ActionID.GetAnimalProduct => GetAnimalProductQuest(),
            ActionID.GetEgg => GetEggQuest(),
            _ => null,

        };
    }

    private static Quest PlantQuest()
    {
        int goal;
        int reward;
        switch (level_Plant)
        {
            case 0: goal = 30; reward = 300; break;
            case 1: goal = 50; reward = 500; break;
            case 2: goal = 100; reward = 1000; break;
            case 3: goal = 250; reward = 2000; break;
            case 4: goal = 500; reward = 5000; break;
            default: return null;
        }
        level_Plant++;
        return new Quest(ActionID.Plant, "작물 심기" + level_Plant, "작물을 심어보자!", goal, reward);
    }

    private static Quest PlantCarrotQuest()
    {
        int goal;
        int reward;
        switch (level_PlantCarrot)
        {
            case 0: goal = 10; reward = 300; break;
            case 1: goal = 20; reward = 500; break;
            case 2: goal = 50; reward = 1000; break;
            case 3: goal = 100; reward = 2000; break;
            case 4: goal = 200; reward = 5000; break;
            default: return null;
        }
        level_PlantCarrot++;
        return new Quest(ActionID.PlantCarrot, "당근 심기 " + level_PlantCarrot, "당근을 심어보자!", goal, reward);
    }

    private static Quest PlantEggplantQuest()
    {
        int goal;
        int reward;
        switch (level_PlantEggplant)
        {
            case 0: goal = 10; reward = 300; break;
            case 1: goal = 20; reward = 500; break;
            case 2: goal = 50; reward = 1000; break;
            case 3: goal = 100; reward = 2000; break;
            case 4: goal = 200; reward = 5000; break;
            default: return null;
        }
        level_PlantEggplant++;
        return new Quest(ActionID.PlantEggplant, "가지 심기 " + level_PlantEggplant, "가지를 심어보자!", goal, reward);
    }

    private static Quest HarvestQuest()
    {
        int goal;
        int reward;
        switch (level_Harvest)
        {
            case 0: goal = 30; reward = 300; break;
            case 1: goal = 50; reward = 500; break;
            case 2: goal = 100; reward = 1000; break;
            case 3: goal = 250; reward = 2000; break;
            case 4: goal = 500; reward = 5000; break;
            default: return null;
        }
        level_Harvest++;
        return new Quest(ActionID.Harvest, "작물 수확하기 " + level_Harvest, "작물을 수확해보자!", goal, reward);
    }

    private static Quest HarvestCarrotQuest()
    {
        int goal;
        int reward;
        switch (level_HarvestCarrot)
        {
            case 0: goal = 10; reward = 300; break;
            case 1: goal = 20; reward = 500; break;
            case 2: goal = 50; reward = 1000; break;
            case 3: goal = 100; reward = 2000; break;
            case 4: goal = 200; reward = 5000; break;
            default: return null;
        }
        level_HarvestCarrot++;
        return new Quest(ActionID.HarvestCarrot, "당근 수확하기 " + level_HarvestCarrot, "당근을 수확해보자!", goal, reward);
    }

    private static Quest HarvestEggplantQuest()
    {
        int goal;
        int reward;
        switch (level_HarvestEggplant)
        {
            case 0: goal = 10; reward = 300; break;
            case 1: goal = 20; reward = 500; break;
            case 2: goal = 50; reward = 1000; break;
            case 3: goal = 100; reward = 2000; break;
            case 4: goal = 200; reward = 5000; break;
            default: return null;
        }
        level_HarvestEggplant++;
        return new Quest(ActionID.HarvestEggplant, "가지 수확하기 " + level_HarvestEggplant, "가지를 수확해보자!", goal, reward);
    }

    private static Quest GetAnimalProductQuest()
    {
        int goal;
        int reward;
        switch (level_GetAnimalProduct)
        {
            case 0: goal = 30; reward = 300; break;
            case 1: goal = 50; reward = 500; break;
            case 2: goal = 100; reward = 1000; break;
            case 3: goal = 250; reward = 2000; break;
            case 4: goal = 500; reward = 5000; break;
            default: return null;
        }
        level_GetAnimalProduct++;
        return new Quest(ActionID.GetAnimalProduct, "축산물 얻기 " + level_GetAnimalProduct, "축산물을 얻어보자!", goal, reward);
    }

    private static Quest GetEggQuest()
    {
        int goal;
        int reward;
        switch (level_GetEgg)
        {
            case 0: goal = 10; reward = 300; break;
            case 1: goal = 20; reward = 500; break;
            case 2: goal = 50; reward = 1000; break;
            case 3: goal = 100; reward = 2000; break;
            case 4: goal = 200; reward = 5000; break;
            default: return null;
        }
        level_GetEgg++;
        return new Quest(ActionID.GetEgg, "계란 얻기 " + level_GetEgg, "계란을 얻어보자!", goal, reward);
    }
    #endregion
}
