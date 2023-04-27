namespace Hotspot.Tools.Code
{
    public interface IGenerator
    {
        string GenerateCourtesy(int n);
        string GenerateEightDigit(int n);
        string GenerateSevenDigit(int n);
        string GenerateTenDigit(int n);
        string GenerateTwelveDigit(int n);
        string GenerateNineDigit(int n);
    }
}