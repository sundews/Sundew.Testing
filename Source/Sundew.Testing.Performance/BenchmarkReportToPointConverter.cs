namespace Sundew.Testing.Performance;

using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Toolchains.Results;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Sundew.Base.Collections;
using Sundew.Base.Primitives.Computation;

public static class BenchmarkReportToPointConverter
{
    public static R<IEnumerable<PointData>, IEnumerable<ExecuteResult>> GetPointResults(Summary[] summaries, DateTime dateTime)
    {
        var allOrFailed = summaries.SelectMany(summary => summary.Reports.Select(report => (summary, report))).AllOrFailed(x =>
        {
            var (summary, report) = x;
            if (report.Success)
            {
                var point = PointData.Measurement(report.BenchmarkCase.Descriptor.DisplayInfo);
                point = point.Tag("Runtime", report.BenchmarkCase.Job.Id)
                    .Tag("CPU", summary.HostEnvironmentInfo.CpuInfo.Value.ProcessorName)
                    .Tag("Configuration", summary.HostEnvironmentInfo.Configuration);
                var allocated = report.Metrics.FirstOrDefault(x => x.Value.Descriptor.DisplayName == "Allocated");
                if (!Equals(allocated, default))
                {
                    point = point.Field("Allocated", allocated.Value.Value);
                }

                point.Field("Mean", report.ResultStatistics!.Mean)
                    .Field("StdDev", report.ResultStatistics.StandardDeviation)
                    .Field("StdErr", report.ResultStatistics.StandardError)
                    .Timestamp(dateTime, WritePrecision.Ns);
                return Item.Pass<PointData, IReadOnlyList<ExecuteResult>>(point);
            }

            return Item.Fail(report.ExecuteResults);
        });

        return allOrFailed.With<IEnumerable<PointData>, IEnumerable<ExecuteResult>>(
            x => x.Items,
            x => x.GetErrors().SelectMany(x => x));
    }
}