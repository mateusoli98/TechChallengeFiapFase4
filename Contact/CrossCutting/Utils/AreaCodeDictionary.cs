namespace CrossCutting.Utils;

/// <summary>
/// Classe auxiliar temporária até que a extensão do projeto seja confirmada.
/// Se houver necessidade de uma relação interna no BD então essa classe será substituida por uma entidade e um script de update no banco
/// </summary>
public static class AreaCodeDictionary
{
    private static readonly Dictionary<string, int[]> _stateToAreaCodes = new()
    {
        ["AC"] = [68],
        ["AL"] = [82],
        ["AM"] = [92, 97],
        ["AP"] = [96],
        ["BA"] = [71, 73, 74, 75, 77],
        ["CE"] = [85, 88],
        ["DF"] = [61],
        ["ES"] = [27, 28],
        ["GO"] = [62, 64],
        ["MA"] = [98, 99],
        ["MG"] = [31, 32, 33, 34, 35, 37, 38],
        ["MS"] = [67],
        ["MT"] = [65, 66],
        ["PA"] = [91, 93, 94],
        ["PB"] = [83],
        ["PE"] = [81, 87],
        ["PI"] = [86, 89],
        ["PR"] = [41, 42, 43, 44, 45, 46],
        ["RJ"] = [21, 22, 24],
        ["RN"] = [84],
        ["RO"] = [69],
        ["RR"] = [95],
        ["RS"] = [51, 53, 54, 55],
        ["SC"] = [47, 48, 49],
        ["SE"] = [79],
        ["SP"] = [11, 12, 13, 14, 15, 16, 17, 18, 19],
        ["TO"] = [63]
    };

    public static int[] GetAreaCodesByState(string state)
    {
        if (_stateToAreaCodes.TryGetValue(state, out var areaCodes))
        {
            return areaCodes;
        }

        throw new ArgumentException($"Invalid state: {state}");
    }

    public static bool IsValidAreaCodeForState(int areaCode, string state)
    {
        if (_stateToAreaCodes.TryGetValue(state, out var areaCodes))
        {
            return Array.Exists(areaCodes, code => code == areaCode);
        }

        return false;
    }

    public static string GetStateByAreaCode(short areaCode)
    {
        foreach (var stateEntry in _stateToAreaCodes)
        {
            var state = stateEntry.Key;
            var areaCodes = stateEntry.Value;
            if (Array.Exists(areaCodes, code => code == areaCode))
            {
                return state;
            }
        }

        return "Invalid AreaCode";
    }
}
