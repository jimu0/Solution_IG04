

using System.Collections.Generic;
using System.Linq;

namespace Mycelia;

internal class GameMode : ISim
{
    //private World world = new();
    internal readonly World world = new();
    
    internal readonly Totem[] Totems = new Totem[81];
    internal readonly Card[] Cards = new Card[56];
    internal readonly Unit[] Units = new Unit[137];
    internal Player player => Totems[0] as Player ?? new Player();
    
    /*
    //private static readonly int[] TotemClassAIds = { 0 };
    //private static readonly int[] TotemClassBIds = {8,17,27,36,45,54,63,72};
    //private static readonly int[] TotemClassCIds = Enumerable.Range(1, 80).Except(TotemClassBIds).ToArray();
    //private static readonly int[][] TotemClassIds = { TotemClassAIds, TotemClassBIds, TotemClassCIds };
    */
    
    
    private readonly Pulse mainPulse = new(); //主脉搏系统
    private readonly Energy energy = new(600);
    
    internal GameMode(GameModeSettings settings)
    {
        world.InitDefaultTiles();
        //构建totem结构
        for (int i = 0; i < Totems.Length; i++)
        {
            int totenTableId = i + 1 + 1000000000;
            Totems[i] = new Totem
            {
                id = totenTableId,
                position = Config.GetUnitConfig(totenTableId).position,
                orientation = Config.GetUnitConfig(totenTableId).orientation,
                scale =  Config.GetUnitConfig(totenTableId).scale
            };
        }
        for (int j = 0; j < 9; j++)
        {
            for (int i = 0; i < 9; i++)
            {
                var n = 9 * j + i;
                var b = 9 * j + 8;
                Totem t = Totems[n];
                if (n == 0)
                {
                    t.level = 0;
                    t.parent = Totems[8];
                    for (int k = 0; k < 8; k++)
                    {
                        t.targets[k] = Totems[9 * k + 8];
                    }
                }
                else if (n == b)
                {
                    t.level = 1;
                    t.parent = Totems[0];
                    for (int k = 0; k < 8; k++)
                    {
                        t.targets[k] = Totems[b - 8 + k];
                    }
                }
                else
                {
                    t.level = 2;
                    t.parent = Totems[b];
                }
            }
        }
        //指定玩家单位
        //player = Totems[0] as Player ?? new Player();
        
        //遍历测试unit
        //Unit unit = new();
        //if (settings.unitsCount <= 0) return;
        //for (int i = 0; i < settings.unitsCount; i++) Units.Add(unit);
        int index = 0;
        foreach (Totem t in Totems) Units[index++] = t;
        foreach (Card t in Cards) Units[index++] = t;
        Units[0] = GetPlayer();
    }

    internal Player GetPlayer()
    {
        return Totems[0] as Player ?? new Player();
    }
    
    
    
    public SimPhase Phase => SimPhase.Step;

    public void OnSimStart(in Input input, ref WorldState state)
    {
        mainPulse.Start(WTime.fixedDt, Totems[0]);
        world.CopyToState(ref state);
    }

    public void OnSimStep(in Input input, ref WorldState state)
    {
        
        mainPulse.Step(WTime.fixedDt, energy);

        List<Totem> totems = new();
        foreach (ScheduledNode node in mainPulse.queue)
        {
            if (node.Node is Totem totem) totems.Add(totem);
        }
        string idString = string.Join(",", totems.Select(item => item?.id));
        state.tick = (int)energy.Remaining;
        state.debugText = $"测试：时间:{mainPulse.time}),待运行Totem数量:({mainPulse.queue.Count}),待运行内容：({idString})";
        
    }
}





