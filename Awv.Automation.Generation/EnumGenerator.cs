using System;
using System.Linq;

namespace Awv.Automation.Generation
{
    public class EnumGenerator<TEnumType> : PredefinedGenerator<TEnumType> where TEnumType : Enum
    {
        public EnumGenerator()
            : base(Enum.GetValues(typeof(TEnumType)).Cast<TEnumType>())
        {
        }
    }
}
