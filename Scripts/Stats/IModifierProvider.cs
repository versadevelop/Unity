using System.Collections.Generic;

namespace Tears_Of_Void.Stats
{
    public interface IModifierProvider
    {
        IEnumerable<float> GetAdditiveModifier(Stat stat);
    }
}