using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FLS;
using FLS.Rules;

public class FuzzyLogic
{

    public void BattleLogic(double inputKill, double inputTime)
    {

        LinguisticVariable kill = new LinguisticVariable("Kill");
        var low = kill.MembershipFunctions.AddTriangle("Low", 0, 50, 50);
        var medium = kill.MembershipFunctions.AddTriangle("Medium", 40, 80, 80);
        var high = kill.MembershipFunctions.AddTrapezoid("High", 70, 80, 1000, 1000);

        LinguisticVariable time = new LinguisticVariable("Time");
        var slow = kill.MembershipFunctions.AddTrapezoid("Slow", 0, 0.006, 0.008, 0.008);
        var normal = kill.MembershipFunctions.AddTriangle("Normal", 0.007 , 0.16, 0.16);
        var fast = kill.MembershipFunctions.AddTriangle("Fast", 0.15, 1, 1);

        /*LinguisticVariable explore = new LinguisticVariable("Explore");
        novice = explore.MembershipFunctions.AddTrapezoid("Novice", 0, 0, 5, 10);
        advance = explore.MembershipFunctions.AddTriangle("Advance", 10, 15, 20);
        expert = explore.MembershipFunctions.AddTrapezoid("Expert", 30, 40, 50, 50);

        LinguisticVariable achieve = new LinguisticVariable("Achieve");
        novice = achieve.MembershipFunctions.AddTrapezoid("Novice", 0, 0, 5, 10);
        advance = achieve.MembershipFunctions.AddTriangle("Advance", 10, 15, 20);
        expert = achieve.MembershipFunctions.AddTrapezoid("Expert", 30, 40, 50, 50);

        LinguisticVariable socialize = new LinguisticVariable("Socialize");
        novice = socialize.MembershipFunctions.AddTrapezoid("Novice", 0, 0, 5, 10);
        advance = socialize.MembershipFunctions.AddTriangle("Advance", 10, 15, 20);
        expert = socialize.MembershipFunctions.AddTrapezoid("Expert", 30, 40, 50, 50);*/

        LinguisticVariable killer = new LinguisticVariable("Killer");
        var novice = killer.MembershipFunctions.AddTriangle("Novice", 0, 3, 3);
        var advance = killer.MembershipFunctions.AddTriangle("Advance", 2, 5, 5);
        var proficient = killer.MembershipFunctions.AddTriangle("Proficient", 4, 8, 8);
        var expert = killer.MembershipFunctions.AddTrapezoid("Expert", 7, 8, 10, 10);

        IFuzzyEngine fuzzyEngine = new FuzzyEngineFactory().Create(new MoMDefuzzification());


        var rule1 = Rule.If(kill.Is(low).And(time.Is(slow))).Then(killer.Is(novice));
        var rule2 = Rule.If(kill.Is(low).And(time.Is(normal))).Then(killer.Is(advance));
        var rule3 = Rule.If(kill.Is(low).And(time.Is(fast))).Then(killer.Is(advance));
        var rule4 = Rule.If(kill.Is(medium).And(time.Is(slow))).Then(killer.Is(novice));
        var rule5 = Rule.If(kill.Is(medium).And(time.Is(normal))).Then(killer.Is(advance));
        var rule6 = Rule.If(kill.Is(medium).And(time.Is(fast))).Then(killer.Is(proficient));
        var rule7 = Rule.If(kill.Is(high).And(time.Is(slow))).Then(killer.Is(advance));
        var rule8 = Rule.If(kill.Is(high).And(time.Is(normal))).Then(killer.Is(proficient));
        var rule9 = Rule.If(kill.Is(high).And(time.Is(fast))).Then(killer.Is(expert));

        fuzzyEngine.Rules.Add(rule1, rule2, rule3, rule4, rule5, rule6, rule7, rule8, rule9);

        var result = fuzzyEngine.Defuzzify(new { kill = inputKill, time = inputTime });
       

        Debug.Log(result);
        
    }

}
