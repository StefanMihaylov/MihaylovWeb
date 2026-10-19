using System;
using System.Collections.Generic;
using System.Linq;
using Mihaylov.Api.Site.Contracts.Helpers.Models;
using Mihaylov.Api.Site.Data.Media.Interfaces;

namespace Mihaylov.Api.Site.Data.Media.Services;

public class AlglibHelper : IAlglibHelper
{
    public void GetDuplucatesHash(List<DuplicateModel> duplicateList, IEnumerable<MediaInfoModel> files, double minDistance)
    {
        var vectorSize = 64;
        var filesData = files.Where(f => f.PerceptualHash.HasValue).ToList();

        var hashIdDict = new Dictionary<int, MediaInfoModel>();

        double[,] data = new double[filesData.Count, vectorSize];
        var tags = new int[filesData.Count];

        for (int n = 0; n < filesData.Count; n++)
        {
            var file = filesData[n];

            tags[n] = n;
            hashIdDict.Add(n, file);

            double[] vector = ULongToDoubleVector(file.PerceptualHash.Value, vectorSize);

            for (int d = 0; d < vector.Length; d++)
            {
                data[n, d] = vector[d];
            }
        }

        alglib.kdtreebuildtagged(data, tags, data.GetLength(0), data.GetLength(1), 0, 2, out alglib.kdtree tree);

        var duplicateFiles = new HashSet<string>();
        foreach (var duplicate in duplicateList)
        {
            string key = GetDuplicateKey(duplicate.List.Select(f => f.FileId), 0);
            duplicateFiles.Add(key);
        }

        var radius = Math.Sqrt(minDistance * vectorSize / 100.0);

        for (int n = 0; n < tags.Length; n++)
        {
            var tag = tags[n];

            var vector = new double[vectorSize];
            for (int v = 0; v < vector.Length; v++)
            {
                vector[v] = data[n, v];
            }

            int neighborsCount = alglib.kdtreequeryrnnu(tree, vector, radius, true);

            var nearestNeighbors = GetNeighbours(tree, tag);
            if (nearestNeighbors.Count == 0)
            {
                continue;
            }

            var current = hashIdDict[tag];

            foreach (var neighborData in nearestNeighbors)
            {
                var neighbours = new List<MediaInfoModel>()
                    {
                        current,
                        hashIdDict[neighborData.Tag]
                    };

                var key = GetDuplicateKey(neighbours.Select(n => n.FileId), neighborData.Distance);
                if (duplicateFiles.Contains(key))
                {
                    continue;
                }

                duplicateFiles.Add(key);

                var checksums = neighbours.Select(n => n.Checksum).Distinct().ToList();
                var hashes = neighbours.Select(n => n.PerceptualHash).Distinct().ToList();

                duplicateList.Add(new DuplicateModel()
                {
                    Checksum = checksums.Count == 1 ? checksums.First() : null,
                    Hash = hashes.Count == 1 ? hashes.First() : null,
                    Similarity = 100 - neighborData.Distance,
                    List = neighbours,
                    Count = neighbours.Count()
                });
            }
        }
    }

    private static string GetDuplicateKey(IEnumerable<Guid> fileIds, double distance)
    {
        return $"{string.Join('_', fileIds.OrderBy(d => d))}_{distance}";
    }

    private List<(int Tag, double Distance)> GetNeighbours(alglib.kdtree tree, int tag, double? minDistance = null)
    {
        var currentTags = new int[0];
        var currentDistance = new double[0];

        alglib.kdtreequeryresultsdistances(tree, ref currentDistance);
        alglib.kdtreequeryresultstags(tree, ref currentTags);

        var nearestNeighbors = new List<(int Tag, double Distance)>();
        for (int i = 0; i < currentTags.Length; i++)
        {
            if (currentTags[i] == tag)
            {
                continue;
            }

            var bitcount = Math.Round(currentDistance[i] * currentDistance[i]);
            var distance = bitcount * 100 / 64.0;

            if (minDistance.HasValue)
            {
                if (distance > minDistance.Value)
                {
                    continue;
                }
            }

            nearestNeighbors.Add((currentTags[i], distance));
        }

        return nearestNeighbors;
    }

    private double[] ULongToDoubleVector(ulong? value, int vectorLength)
    {
        if (!value.HasValue)
        {
            return null;
        }

        var vector = new double[vectorLength];
        for (int i = 0; i < vectorLength; i++)
        {
            // (value >> i) & 1 isolates the i-th bit
            vector[i] = ((value.Value >> i) & 1) == 1 ? 1.0 : 0.0;
        }

        return vector;
    }
}
