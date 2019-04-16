namespace NaiveGrainGrowSimulation
{
    enum GrainGrow
    {
        Random,
        Even,
        Click,
        Radius
    }

    enum NeighborhoodType
    {
        Moore,
        VonNeumann,
        HeksagonalLeft,
        HeksagonalRight,
        HeksagonalRandom,
        PentagonalRandom
    }

    enum EdgeCondition
    {
        NonPeriodic,
        Periodic
    }
}