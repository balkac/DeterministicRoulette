using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class BetDataEditor : EditorWindow
{
    private const string FolderPath = "Assets/BetData";
    private const string ConditionFolderPath = "Assets/BetConditions";

    [MenuItem("Tools/Generate American Roulette Bets")]
    public static void ShowWindow()
    {
        GetWindow(typeof(BetDataEditor), false, "Bet Data Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Generate All American Roulette Bets", EditorStyles.boldLabel);

        if (GUILayout.Button("Generate Bet Data"))
        {
            GenerateBetData();
        }
    }

    private static void GenerateBetData()
    {
        if (!Directory.Exists(FolderPath))
        {
            Directory.CreateDirectory(FolderPath);
        }

        if (!Directory.Exists(ConditionFolderPath))
        {
            Directory.CreateDirectory(ConditionFolderPath);
        }

        foreach (BetType betType in System.Enum.GetValues(typeof(BetType)))
        {
            string betTypeFolderPath = Path.Combine(FolderPath, betType.ToString());
            if (!Directory.Exists(betTypeFolderPath))
            {
                Directory.CreateDirectory(betTypeFolderPath);
            }

            if (IsInsideBet(betType))
            {
                int[][] betCombinations = GetCombinationsForBet(betType);
                foreach (var numbers in betCombinations)
                {
                    BetCondition_NumberExists conditionNumberExists = GetOrCreateCondition(betType, numbers);
                    CreateBetData(betType, numbers, conditionNumberExists, betTypeFolderPath);
                }
            }
            else
            {
                int[] numbers = GetNumbersForBet(betType);
                BetCondition_NumberExists conditionNumberExists = GetOrCreateCondition(betType, numbers);
                CreateBetData(betType, numbers, conditionNumberExists, betTypeFolderPath);
            }
        }
    }

    private static bool IsInsideBet(BetType betType)
    {
        return betType == BetType.Straight || betType == BetType.Split || betType == BetType.Street ||
               betType == BetType.Corner || betType == BetType.SixLine;
    }

    private static void CreateBetData(BetType betType, int[] numbers, BetCondition_NumberExists conditionNumberExists, string folderPath)
    {
        string assetPath;
        if (IsInsideBet(betType))
        {
            assetPath = Path.Combine(folderPath, $"BetData_{betType}_{string.Join("_", numbers)}.asset");
        }
        else
        {
            assetPath = Path.Combine(folderPath, $"BetData_{betType}.asset");
        }

        if (File.Exists(assetPath))
        {
            Debug.LogWarning($"BetData for {betType} {string.Join(",", numbers)} already exists.");
            return;
        }

        BetData betData = ScriptableObject.CreateInstance<BetData>();
        betData.BetType = betType;
        betData.PayoutMultiplier = GetPayoutMultiplier(betType);
        betData.Numbers = numbers;
        betData.BetCondition = conditionNumberExists;

        EditorUtility.SetDirty(betData);
        AssetDatabase.CreateAsset(betData, assetPath);
        AssetDatabase.SaveAssets();
        Debug.Log($"Created BetData for {betType} {string.Join(",", numbers)}");
    }

    private static BetCondition_NumberExists GetOrCreateCondition(BetType betType, int[] numbers)
    {
        string conditionPath =
            Path.Combine(ConditionFolderPath, $"BetCondition_NumberExists.asset");
        BetCondition_NumberExists conditionNumberExists = AssetDatabase.LoadAssetAtPath<BetCondition_NumberExists>(conditionPath);

        if (conditionNumberExists != null)
        {
            return conditionNumberExists;
        }

        conditionNumberExists = ScriptableObject.CreateInstance<BetCondition_NumberExists>();

        AssetDatabase.CreateAsset(conditionNumberExists, conditionPath);
        AssetDatabase.SaveAssets();
        Debug.Log($"Created Condition for {betType} {string.Join(",", numbers)}");

        return conditionNumberExists;
    }

    private static float GetPayoutMultiplier(BetType betType)
    {
        return betType switch
        {
            BetType.Straight => 35f,
            BetType.Split => 17f,
            BetType.Street => 11f,
            BetType.Corner => 8f,
            BetType.SixLine => 5f,
            BetType.Red => 1f,
            BetType.Black => 1f,
            BetType.Even => 1f,
            BetType.Odd => 1f,
            BetType.FirstDozen => 2f,
            BetType.SecondDozen => 2f,
            BetType.ThirdDozen => 2f,
            BetType.FirstColumn => 2f,
            BetType.SecondColumn => 2f,
            BetType.ThirdColumn => 2f,
            BetType.High => 1f,
            BetType.Low => 1f,
            _ => 0f,
        };
    }

    private static int[][] GetCombinationsForBet(BetType betType)
    {
        List<int[]> combinations = new List<int[]>();
        int[] numbers = GetAllRouletteNumbers();

        switch (betType)
        {
            case BetType.Straight:
                foreach (var num in numbers) combinations.Add(new int[] { num });
                break;
            case BetType.Split:
                for (int i = 0; i < numbers.Length; i++)
                {
                    if ((i + 1) % 3 != 0)
                        combinations.Add(new int[] { numbers[i], numbers[i + 1] });
                    if (i < numbers.Length - 3)
                        combinations.Add(new int[] { numbers[i], numbers[i + 3] });
                }

                break;
            case BetType.Street:
                for (int i = 0; i < numbers.Length - 2; i += 3)
                    combinations.Add(new int[] { numbers[i], numbers[i + 1], numbers[i + 2] });
                break;
            case BetType.Corner:
                for (int i = 0; i < numbers.Length - 4; i++)
                {
                    if ((i + 1) % 3 != 0 && i < numbers.Length - 3)
                        combinations.Add(new int[] { numbers[i], numbers[i + 1], numbers[i + 3], numbers[i + 4] });
                }

                break;
            case BetType.SixLine:
                for (int i = 0; i < numbers.Length - 5; i += 3)
                    combinations.Add(new int[]
                        { numbers[i], numbers[i + 1], numbers[i + 2], numbers[i + 3], numbers[i + 4], numbers[i + 5] });
                break;
        }

        return combinations.ToArray();
    }

    private static int[] GetAllRouletteNumbers()
    {
        return new int[]
        {
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29,
            30, 31, 32, 33, 34, 35, 36
        };
    }

    private static int[] GetNumbersForBet(BetType betType)
    {
        return betType switch
        {
            BetType.Red => new int[] { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 },
            BetType.Black => new int[] { 2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35 },
            BetType.Even => new int[] { 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32, 34, 36 },
            BetType.Odd => new int[] { 1, 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 23, 25, 27, 29, 31, 33, 35 },
            BetType.FirstDozen => new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 },
            BetType.SecondDozen => new int[] { 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 },
            BetType.ThirdDozen => new int[] { 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36 },
            BetType.FirstColumn => new int[] { 1, 4, 7, 10, 13, 16, 19, 22, 25, 28, 31, 34 },
            BetType.SecondColumn => new int[] { 2, 5, 8, 11, 14, 17, 20, 23, 26, 29, 32, 35 },
            BetType.ThirdColumn => new int[] { 3, 6, 9, 12, 15, 18, 21, 24, 27, 30, 33, 36 },
            BetType.High => new int[] { 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36 },
            BetType.Low => new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 },
            _ => new int[0],
        };
    }
}