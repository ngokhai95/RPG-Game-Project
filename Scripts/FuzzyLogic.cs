using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FLS;
using FLS.Rules;
using System.Linq;

public class FuzzyLogic
{

    public double KillerFuzzy(double killAmount, double killQuest)
    {
        //Killer Fuzzy Set
        LinguisticVariable killer = new LinguisticVariable("Killer");
        var novice = killer.MembershipFunctions.AddTriangle("Novice", 0, 3, 3);
        var advance = killer.MembershipFunctions.AddTriangle("Advance", 2, 5, 5);
        var proficient = killer.MembershipFunctions.AddTriangle("Proficient", 4, 8, 8);
        var expert = killer.MembershipFunctions.AddTrapezoid("Expert", 7, 8, 10, 10);

        //Support Fuzzy Set
        LinguisticVariable amount = new LinguisticVariable("Amount");
        var low = amount.MembershipFunctions.AddTriangle("Low", 1, 50, 50);
        var medium = amount.MembershipFunctions.AddTriangle("Medium", 40, 80, 80);
        var high = amount.MembershipFunctions.AddTrapezoid("High", 70, 80, 1000, 1000);

        LinguisticVariable quest = new LinguisticVariable("Quest");
        var little = quest.MembershipFunctions.AddTriangle("Little", 1, 3, 3);
        var average = quest.MembershipFunctions.AddTriangle("Average", 2, 5, 5);
        var lot = quest.MembershipFunctions.AddTrapezoid("Lot", 7, 8, 10, 10);

        //time
        /*LinguisticVariable time = new LinguisticVariable("Time");
        var slow = kill.MembershipFunctions.AddTrapezoid("Slow", 0, 0.006, 0.008, 0.008);
        var normal = kill.MembershipFunctions.AddTriangle("Normal", 0.007 , 0.16, 0.16);
        var fast = kill.MembershipFunctions.AddTriangle("Fast", 0.15, 1, 1);*/ 

        IFuzzyEngine fuzzyEngine = new FuzzyEngineFactory().Create(new MoMDefuzzification());


        var rule1 = Rule.If(amount.Is(low).And(quest.Is(little))).Then(killer.Is(novice));
        var rule2 = Rule.If(amount.Is(low).And(quest.Is(average))).Then(killer.Is(advance));
        var rule3 = Rule.If(amount.Is(low).And(quest.Is(lot))).Then(killer.Is(advance));
        var rule4 = Rule.If(amount.Is(medium).And(quest.Is(little))).Then(killer.Is(novice));
        var rule5 = Rule.If(amount.Is(medium).And(quest.Is(average))).Then(killer.Is(advance));
        var rule6 = Rule.If(amount.Is(medium).And(quest.Is(lot))).Then(killer.Is(proficient));
        var rule7 = Rule.If(amount.Is(high).And(quest.Is(little))).Then(killer.Is(advance));
        var rule8 = Rule.If(amount.Is(high).And(quest.Is(average))).Then(killer.Is(proficient));
        var rule9 = Rule.If(amount.Is(high).And(quest.Is(lot))).Then(killer.Is(expert));

        fuzzyEngine.Rules.Add(rule1, rule2, rule3, rule4, rule5, rule6, rule7, rule8, rule9);

        var result = fuzzyEngine.Defuzzify(new { amount = killAmount, quest = killQuest });


        return result;
        
    }

    public double AchieverFuzzy(double questFinished, double avgDifficulty)
    {
        //Achiever Characteristic Fuzzy Set

        LinguisticVariable amount = new LinguisticVariable("Amount");
        var low = amount.MembershipFunctions.AddTriangle("Low", 1, 10, 10);
        var medium = amount.MembershipFunctions.AddTriangle("Medium", 8, 16, 16);
        var high = amount.MembershipFunctions.AddTrapezoid("High", 15, 30, 100, 100);

        LinguisticVariable difficulty = new LinguisticVariable("Difficulty");
        var easy = amount.MembershipFunctions.AddTriangle("Low", 1, 2, 2);
        var normal = amount.MembershipFunctions.AddTriangle("Medium", 2, 3, 3);
        var hard = amount.MembershipFunctions.AddTrapezoid("High", 3, 4, 5, 5);

        LinguisticVariable achiever = new LinguisticVariable("Achiever");
        var novice = achiever.MembershipFunctions.AddTriangle("Novice", 0, 3, 3);
        var advance = achiever.MembershipFunctions.AddTriangle("Advance", 2, 5, 5);
        var proficient = achiever.MembershipFunctions.AddTriangle("Proficient", 4, 8, 8);
        var expert = achiever.MembershipFunctions.AddTrapezoid("Expert", 7, 8, 10, 10);

        IFuzzyEngine fuzzyEngine = new FuzzyEngineFactory().Create(new MoMDefuzzification());

        var rule1 = Rule.If(amount.Is(low).And(difficulty.Is(easy))).Then(achiever.Is(novice));
        var rule2 = Rule.If(amount.Is(low).And(difficulty.Is(normal))).Then(achiever.Is(advance));
        var rule3 = Rule.If(amount.Is(low).And(difficulty.Is(hard))).Then(achiever.Is(advance));
        var rule4 = Rule.If(amount.Is(medium).And(difficulty.Is(easy))).Then(achiever.Is(novice));
        var rule5 = Rule.If(amount.Is(medium).And(difficulty.Is(normal))).Then(achiever.Is(advance));
        var rule6 = Rule.If(amount.Is(medium).And(difficulty.Is(hard))).Then(achiever.Is(proficient));
        var rule7 = Rule.If(amount.Is(high).And(difficulty.Is(easy))).Then(achiever.Is(advance));
        var rule8 = Rule.If(amount.Is(high).And(difficulty.Is(normal))).Then(achiever.Is(proficient));
        var rule9 = Rule.If(amount.Is(high).And(difficulty.Is(hard))).Then(achiever.Is(expert));

        fuzzyEngine.Rules.Add(rule1, rule2, rule3, rule4, rule5, rule6, rule7, rule8, rule9);

        var result = fuzzyEngine.Defuzzify(new { amount = questFinished, difficulty = avgDifficulty });

        return result;
    }

    public double ExplorerFuzzy(double placesTravel, double exploreQuest)
    {
        //Explorer Characteristic Fuzzy Set

        LinguisticVariable travel = new LinguisticVariable("Travel");
        var low = travel.MembershipFunctions.AddTriangle("Low", 1, 10, 10);
        var medium = travel.MembershipFunctions.AddTriangle("Medium", 8, 16, 16);
        var high = travel.MembershipFunctions.AddTrapezoid("High", 15, 30, 100, 100);

        LinguisticVariable explore = new LinguisticVariable("Explore");
        var little = explore.MembershipFunctions.AddTriangle("Little", 1, 3, 3);
        var average = explore.MembershipFunctions.AddTriangle("Average", 2, 5, 5);
        var lot = explore.MembershipFunctions.AddTrapezoid("Lot", 7, 8, 10, 10);

        LinguisticVariable explorer = new LinguisticVariable("Achiever");
        var novice = explorer.MembershipFunctions.AddTriangle("Novice", 0, 3, 3);
        var advance = explorer.MembershipFunctions.AddTriangle("Advance", 2, 5, 5);
        var proficient = explorer.MembershipFunctions.AddTriangle("Proficient", 4, 8, 8);
        var expert = explorer.MembershipFunctions.AddTrapezoid("Expert", 7, 8, 10, 10);

        IFuzzyEngine fuzzyEngine = new FuzzyEngineFactory().Create(new MoMDefuzzification());

        var rule1 = Rule.If(travel.Is(low).And(explore.Is(little))).Then(explorer.Is(novice));
        var rule2 = Rule.If(travel.Is(low).And(explore.Is(average))).Then(explorer.Is(advance));
        var rule3 = Rule.If(travel.Is(low).And(explore.Is(lot))).Then(explorer.Is(advance));
        var rule4 = Rule.If(travel.Is(medium).And(explore.Is(little))).Then(explorer.Is(novice));
        var rule5 = Rule.If(travel.Is(medium).And(explore.Is(average))).Then(explorer.Is(advance));
        var rule6 = Rule.If(travel.Is(medium).And(explore.Is(lot))).Then(explorer.Is(proficient));
        var rule7 = Rule.If(travel.Is(high).And(explore.Is(little))).Then(explorer.Is(advance));
        var rule8 = Rule.If(travel.Is(high).And(explore.Is(average))).Then(explorer.Is(proficient));
        var rule9 = Rule.If(travel.Is(high).And(explore.Is(lot))).Then(explorer.Is(expert));

        fuzzyEngine.Rules.Add(rule1, rule2, rule3, rule4, rule5, rule6, rule7, rule8, rule9);

        var result = fuzzyEngine.Defuzzify(new { travel = placesTravel, explore = exploreQuest });

        return result;
    }

    public double SocializerFuzzy(double activity, double interaction)
    {
        //Socializer Characteristic Fuzzy Set

        LinguisticVariable amount = new LinguisticVariable("Amount");
        var low = amount.MembershipFunctions.AddTriangle("Low", 1, 10, 10);
        var medium = amount.MembershipFunctions.AddTriangle("Medium", 8, 16, 16);
        var high = amount.MembershipFunctions.AddTrapezoid("High", 15, 30, 100, 100);

        LinguisticVariable interact = new LinguisticVariable("Interact");
        var little = interact.MembershipFunctions.AddTriangle("Little", 1, 3, 3);
        var average = interact.MembershipFunctions.AddTriangle("Average", 2, 5, 5);
        var lot = interact.MembershipFunctions.AddTrapezoid("Lot", 7, 8, 10, 10);

        LinguisticVariable socializer = new LinguisticVariable("Socializer");
        var novice = socializer.MembershipFunctions.AddTriangle("Novice", 0, 3, 3);
        var advance = socializer.MembershipFunctions.AddTriangle("Advance", 2, 5, 5);
        var proficient = socializer.MembershipFunctions.AddTriangle("Proficient", 4, 8, 8);
        var expert = socializer.MembershipFunctions.AddTrapezoid("Expert", 7, 8, 10, 10);

        IFuzzyEngine fuzzyEngine = new FuzzyEngineFactory().Create(new MoMDefuzzification());

        var rule1 = Rule.If(amount.Is(low).And(interact.Is(little))).Then(socializer.Is(novice));
        var rule2 = Rule.If(amount.Is(low).And(interact.Is(average))).Then(socializer.Is(advance));
        var rule3 = Rule.If(amount.Is(low).And(interact.Is(lot))).Then(socializer.Is(advance));
        var rule4 = Rule.If(amount.Is(medium).And(interact.Is(little))).Then(socializer.Is(novice));
        var rule5 = Rule.If(amount.Is(medium).And(interact.Is(average))).Then(socializer.Is(advance));
        var rule6 = Rule.If(amount.Is(medium).And(interact.Is(lot))).Then(socializer.Is(proficient));
        var rule7 = Rule.If(amount.Is(high).And(interact.Is(little))).Then(socializer.Is(advance));
        var rule8 = Rule.If(amount.Is(high).And(interact.Is(average))).Then(socializer.Is(proficient));
        var rule9 = Rule.If(amount.Is(high).And(interact.Is(lot))).Then(socializer.Is(expert));

        fuzzyEngine.Rules.Add(rule1, rule2, rule3, rule4, rule5, rule6, rule7, rule8, rule9);

        var result = fuzzyEngine.Defuzzify(new { amount = activity, interact = interaction });

        return result;
    }

    public void Characterize(double killer, double achiever, double explorer, double socializer)
    {
        if (killer == achiever && achiever == explorer && explorer == socializer)
        {
            Debug.Log("Player is Balance");
        }
        else
        {
            double total = killer + achiever + explorer + socializer;
            double[] characteristic = { killer, achiever, explorer, socializer };
            double maxValue = characteristic.Max();
            int maxIndex = characteristic.ToList().IndexOf(maxValue);
            double percentage = ((maxValue / total) * 100);
            switch (maxIndex)
            {
                case 0:
                    Debug.Log("Player is " + GetExpertiseLevel(maxValue) + " Killer at " + percentage + "%");
                    break;
                case 1:
                    Debug.Log("Player is " + GetExpertiseLevel(maxValue) + " Achiever at " + percentage + "%");
                    break;
                case 2:
                    Debug.Log("Player is " + GetExpertiseLevel(maxValue) + " Explorer at " + percentage + "%");
                    break;
                case 3:
                    Debug.Log("Player is " + GetExpertiseLevel(maxValue) + " Socializer at " + percentage + "%");
                    break;
            }
        }
    }

    private string GetExpertiseLevel(double score)
    {
        if(score <= 3)
        {
            return "Novice";
        }
        else if(score <= 5)
        {
            return "Advance";
        }
        else if (score <= 8)
        {
            return "Proficient";
        }
        else
        {
            return "Expert";
        }
    }

}
