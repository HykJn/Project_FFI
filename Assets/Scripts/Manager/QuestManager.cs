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

    public static Quest FirstQuest = new Quest(ActionID.Harvest, "ù ��Ȯ", "ù �۹��� ��Ȯ�غ���!", 1, 300);
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
        return new Quest(ActionID.Plant, "�۹� �ɱ�" + level_Plant, "�۹��� �ɾ��!", goal, reward);
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
        return new Quest(ActionID.PlantCarrot, "��� �ɱ� " + level_PlantCarrot, "����� �ɾ��!", goal, reward);
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
        return new Quest(ActionID.PlantEggplant, "���� �ɱ� " + level_PlantEggplant, "������ �ɾ��!", goal, reward);
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
        return new Quest(ActionID.Harvest, "�۹� ��Ȯ�ϱ� " + level_Harvest, "�۹��� ��Ȯ�غ���!", goal, reward);
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
        return new Quest(ActionID.HarvestCarrot, "��� ��Ȯ�ϱ� " + level_HarvestCarrot, "����� ��Ȯ�غ���!", goal, reward);
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
        return new Quest(ActionID.HarvestEggplant, "���� ��Ȯ�ϱ� " + level_HarvestEggplant, "������ ��Ȯ�غ���!", goal, reward);
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
        return new Quest(ActionID.GetAnimalProduct, "��깰 ��� " + level_GetAnimalProduct, "��깰�� ����!", goal, reward);
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
        return new Quest(ActionID.GetEgg, "��� ��� " + level_GetEgg, "����� ����!", goal, reward);
    }
    #endregion
}
