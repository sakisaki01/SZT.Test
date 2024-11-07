

namespace SZT.Test.Services;

public interface IPeakStorage
{
    int[] FindPeak(int[] data, double amplitudeThreshold, int minDistance);

    double CalculatePeakArea(double[] data, int peakIndex, double threshold = 0.1, int maxWidth = 20);
}
