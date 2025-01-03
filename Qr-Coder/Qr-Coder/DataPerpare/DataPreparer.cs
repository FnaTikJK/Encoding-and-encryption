﻿namespace Qr_Coder;

public static class DataPreparer
{
    public static (bool[], int) Prepare(byte[] data, Correction correction)
    {
        var version = GetVersion(data, correction);
        var workingFields = GetWorkingFields(data, version);
        var toEncode = workingFields
            .Concat(data.ToBoolArray())
            .ToArray();
        toEncode = toEncode
            .Concat(new bool[toEncode.Length % 8])
            .ToArray();
        var filled = FillToVersionSize(toEncode, version, correction).ToByteArray();
        var blocks = BlockSplitter.Split(filled, correction, version);
        var correctionBlocks = blocks
            .Select(block => ReedSalomon.GetCorrectionBlock(block, correction, version))
            .ToArray();
        var bits = blocks.ByInternalIndexesOrder()
            .Concat(correctionBlocks.ByInternalIndexesOrder())
            .ToBoolArray();

        return (bits, version);
    }

    private static bool[] FillToVersionSize(bool[] toEncode, int version, Correction correction)
    {
        var bitsToAdd = ("00010001" + "11101100").ToBoolArray();
        var needToAddCount = BitsPerVersionAndCorrection[correction][version - 1];
        return toEncode
            .Concat(Enumerable.Range(0, needToAddCount)
                .Select((_, ind) => bitsToAdd[ind % bitsToAdd.Length]))
            .ToArray();
    }

    private static bool[] GetWorkingFields(byte[] data, int version)
    {
        var encodingTypeField = "0100"; // байтовое кодирование
        var encodingBitsLength = version <= 9 ? 8 : 16; // TODO: check data length Добавление служебных полей
        var dataCountField = Convert.ToString(data.Length, 2).PadLeft(encodingBitsLength, '0');
        return (encodingTypeField + dataCountField).ToBoolArray();
    }

    public static int GetVersion(byte[] data, Correction correction)
    {
        var bitsCount = data.Length * 8;
        var versions = BitsPerVersionAndCorrection[correction];
        for (int i = 0; i < versions.Length; i++)
        {
            if (versions[i] > bitsCount)
                return i + 1;
        }

        throw new Exception($"data is too large. BitsCount: {bitsCount}");
    }

    private static Dictionary<Correction, int[]> BitsPerVersionAndCorrection = new()
    {
        {
            Correction.L,
            new[]
            {
                152, 272, 440, 640, 864, 1088, 1248, 1552, 1856, 2192, 2592, 2960, 3424, 3688, 4184, 4712, 5176, 5768,
                6360, 6888, 7456, 8048, 8752, 9392, 10208, 10960, 11744, 12248, 13048, 13880, 14744, 15640, 16568,
                17528, 18448, 19472, 20528, 21616, 22496, 23648
            }
        },
        {
            Correction.M,
            new[]
            {
                128, 224, 352, 512, 688, 864, 992, 1232, 1456, 1728, 2032, 2320, 2672, 2920, 3320, 3624, 4056, 4504,
                5016, 5352, 5712, 6256, 6880, 7312, 8000, 8496, 9024, 9544, 10136, 10984, 11640, 12328, 13048, 13800,
                14496, 15312, 15936, 16816, 17728, 18672
            }
        },
        {
            Correction.H,
            new[]
            {
                104, 176, 272, 384, 496, 608, 704, 880, 1056, 1232, 1440, 1648, 1952, 2088, 2360, 2600, 2936, 3176,
                3560, 3880, 4096, 4544, 4912, 5312, 5744, 6032, 6464, 6968, 7288, 7880, 8264, 8920, 9368, 9848, 10288,
                10832, 11408, 12016, 12656, 13328
            }
        },
        {
            Correction.Q,
            new[]
            {
                72, 128, 208, 288, 368, 480, 528, 688, 800, 976, 1120, 1264, 1440, 1576, 1784, 2024, 2264, 2504, 2728,
                3080, 3248, 3536, 3712, 4112, 4304, 4768, 5024, 5288, 5608, 5960, 6344, 6760, 7208, 7688, 7888, 8432,
                8768, 9136, 9776, 10208
            }
        },
    };
}