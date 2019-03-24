using System;

namespace CommonLib
{
    // Члены предложения.
    // Переименуйте по желанию 
    [Flags]
    [Serializable]
    public enum SentenceMembers
    {
        Subject     = 0b000001,
        Predicate   = 0b000010,
        Definition  = 0b000100,
        Addition    = 0b010000,
        Circumstance= 0b100000,
    }
}