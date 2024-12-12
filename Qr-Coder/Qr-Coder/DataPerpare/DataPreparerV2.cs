namespace Qr_Coder;

public static class DataPreparerV2
{
    public static (bool[], int) Prepare(string text, Correction correction)
    {
        var encoded = AlphanumericEncoder.Encode(text);
        var (version, needsBitsLength) = VersionDeterminer.Determine(encoded.Length, correction);
        Console.WriteLine($"Version is: {version}. Bits length: {encoded.Length}, max in version: {needsBitsLength}");
        var workingFields = WorkingFieldsDeterminer.GetWorkingFields(text.Length, version);
        var prettied = DataPrettier.Pretty(encoded, workingFields, needsBitsLength);
        var blocks = BlockSplitter.Split(prettied.ToByteArray(), correction, version);
        var correctionBlocks = blocks
            .Select(e => Rid_Salomon.GetCorrectionBlock(e, correction, version))
            .ToArray();
        var resultBits =blocks.ByInternalIndexesOrder()
            .Concat(correctionBlocks.ByInternalIndexesOrder())
            .ToBoolArray();
        
        return (resultBits, version);
    }

    
}