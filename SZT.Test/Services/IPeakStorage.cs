

namespace SZT.Test.Services;

public interface IPeakStorage
{
    int[] FindPeak(double[] data, double amplitudeThreshold, int minDistance);

    double CalculatePeakArea(double[] data, int peakIndex);
}
