using Jint.Parser.Ast;
using OpenData.Basketball.AbaLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Utilities
{
    public static class TypeConversionExstention
    {
        public static decimal? ConvertToDecimal(this double? value)
        {
            if(value == null)
            {
                return null;
            }
            return Math.Round((decimal)value, 2);
        }
    }
}
