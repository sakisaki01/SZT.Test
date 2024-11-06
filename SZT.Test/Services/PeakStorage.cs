
namespace SZT.Test.Services;

public class PeakStorage : IPeakStorage
{
    public double CalculatePeakArea(double[] data, int peakIndex)
    {
        // 检查给定的波峰索引是否在有效范围内
        if (peakIndex <= 0 || peakIndex >= data.Length - 1)
        {
            throw new ArgumentOutOfRangeException("Invalid peak index.");
        }

        // 找到波峰的左右边界
        int leftIndex = peakIndex;
        int rightIndex = peakIndex;

        // 向左扩展边界
        while (leftIndex > 0 && data[leftIndex] >= data[leftIndex - 1])
        {
            leftIndex--;
        }
        Console.WriteLine(leftIndex);

        // 向右扩展边界
        while (rightIndex < data.Length - 1 && data[rightIndex] >= data[rightIndex + 1])
        {
            rightIndex++;
        }
        Console.WriteLine(rightIndex);

        // 计算波峰的面积（使用梯形法）
        double area = 0.0;
        for (int i = leftIndex; i < rightIndex; i++)
        {
            area += (data[i] + data[i + 1]) / 2.0; // 梯形法
        }

        return area;
    }

    public int[] FindPeak(double[] data, double amplitudeThreshold, int minDistance)
    {
        double[] diff = new double[data.Length - 1];
        for (int i = 0; i < diff.Length; i++)
        {
            diff[i] = data[i + 1] - data[i];
        }
        int[] sign = new int[diff.Length];
        for (int i = 0; i < sign.Length; i++)
        {
            if (diff[i] > 0) sign[i] = 1;
            else if (diff[i] == 0) sign[i] = 0;
            else sign[i] = -1;
        }
        for (int i = sign.Length - 1; i >= 0; i--)
        {
            if (sign[i] == 0 && i == sign.Length - 1)
            {
                sign[i] = 1;
            }
            else if (sign[i] == 0)
            {
                if (sign[i + 1] >= 0)
                {
                    sign[i] = 1;
                }
                else
                {
                    sign[i] = -1;
                }
            }
        }
        List<int> result = new List<int>();
        for (int i = 0; i != sign.Length - 1; i++)
        {


            if (sign[i + 1] - sign[i] == -2)
            {
                // 检查幅度阈值
                if (Math.Abs(data[i + 1] - data[i]) >= amplitudeThreshold)
                {
                    result.Add(i + 1);
                }
            }

        }

        // 处理距离阈值
        List<int> filteredResult = new List<int>();
        for (int i = 0; i < result.Count; i++)
        {
            if (i == 0 || result[i] - result[i - 1] >= minDistance)
            {
                filteredResult.Add(result[i]);
            }
        }
        return filteredResult.ToArray();
    }
}
