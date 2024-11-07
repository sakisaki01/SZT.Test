
namespace SZT.Test.Services;

public class PeakStorage : IPeakStorage
{
    public  double CalculatePeakArea(double[] data, int peakIndex, double threshold = 0.1, int maxWidth = 20)
    {
        // 检查给定的波峰索引是否在有效范围内
        if (peakIndex <= 0 || peakIndex >= data.Length - 1)
        {
            throw new ArgumentOutOfRangeException("Invalid peak index.");
        }

        // 找到波峰的左右边界
        int leftIndex = peakIndex;
        int rightIndex = peakIndex;

        // 向左扩展边界，加入阈值判断并限制最大距离
        int leftLimit = Math.Max(0, peakIndex - maxWidth); // 限制左边界的最小位置
        while (leftIndex > leftLimit && data[leftIndex] >= data[leftIndex - 1] - threshold)
        {
            leftIndex--;
        }
        Console.WriteLine($"左边界索引: {leftIndex}");

        // 向右扩展边界，加入阈值判断并限制最大距离
        int rightLimit = Math.Min(data.Length - 1, peakIndex + maxWidth); // 限制右边界的最大位置
        while (rightIndex < rightLimit && data[rightIndex] >= data[rightIndex + 1] - threshold)
        {
            rightIndex++;
        }
        Console.WriteLine($"右边界索引: {rightIndex}");

        // 计算波峰的面积（使用梯形法）
        double area = 0.0;
        for (int i = leftIndex; i < rightIndex; i++)
        {
            area += (data[i] + data[i + 1]) / 2.0;
        }

        return area;
    }

    /// <summary>
    /// 寻峰算法
    /// </summary>
    /// <param name="data">传入数据</param>
    /// <param name="amplitudeThreshold"> 幅度阈值</param>
    /// <param name="minDistance"> 距离阈值</param>
    /// <returns></returns>
    public int[] FindPeak(int[] data, double amplitudeThreshold, int minDistance)
    {
        int[] diff = new int[data.Length - 1];
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
