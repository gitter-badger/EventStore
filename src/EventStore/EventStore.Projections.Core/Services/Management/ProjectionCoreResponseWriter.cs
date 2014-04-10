﻿using EventStore.Core.Bus;
using EventStore.Projections.Core.Messages;
using EventStore.Projections.Core.Services.Processing;
using Newtonsoft.Json;

namespace EventStore.Projections.Core.Services.Management
{
    public sealed class ProjectionCoreResponseWriter
        : IHandle<CoreProjectionManagementMessage.Faulted>,
            IHandle<CoreProjectionManagementMessage.Prepared>,
            IHandle<CoreProjectionManagementMessage.SlaveProjectionReaderAssigned>,
            IHandle<CoreProjectionManagementMessage.Started>,
            IHandle<CoreProjectionManagementMessage.StatisticsReport>,
            IHandle<CoreProjectionManagementMessage.Stopped>,
            IHandle<CoreProjectionManagementMessage.StateReport>,
            IHandle<CoreProjectionManagementMessage.ResultReport>
    {
        private readonly IResponseWriter _writer;

        public ProjectionCoreResponseWriter(IResponseWriter responseWriter)
        {
            _writer = responseWriter;
        }

        public void Handle(CoreProjectionManagementMessage.Faulted message)
        {
            var command = new ProjectionCoreResponseWriter.Faulted
            {
                Id = message.ProjectionId.ToString("N"),
                FaultedReason = message.FaultedReason,
            };
            _writer.PublishCommand("$faulted", command);
        }

        public void Handle(CoreProjectionManagementMessage.Prepared message)
        {
            var command = new ProjectionCoreResponseWriter.Prepared
            {
                Id = message.ProjectionId.ToString("N"),
                SourceDefinition = message.SourceDefinition,
            };
            _writer.PublishCommand("$prepared", command);
        }

        public void Handle(CoreProjectionManagementMessage.SlaveProjectionReaderAssigned message)
        {
            var command = new ProjectionCoreResponseWriter.SlaveProjectionReaderAssigned 
            {
                Id = message.ProjectionId.ToString("N"),
                ReaderId = message.ReaderId.ToString("N"),
                SubscriptionId = message.SubscriptionId.ToString("N"),
            };
            _writer.PublishCommand("$slave-projection-reader-assigned", command);
        }

        public void Handle(CoreProjectionManagementMessage.Started message)
        {
            var command = new ProjectionCoreResponseWriter.Started
            {
                Id = message.ProjectionId.ToString("N"),
            };
            _writer.PublishCommand("$started", command);
        }

        public void Handle(CoreProjectionManagementMessage.StatisticsReport message)
        {
            var command = new ProjectionCoreResponseWriter.StatisticsReport
            {
                Id = message.ProjectionId.ToString("N"),
                Statistcs = message.Statistics
            };
            _writer.PublishCommand("$statistics-report", command);
        }

        public void Handle(CoreProjectionManagementMessage.Stopped message)
        {
            var command = new ProjectionCoreResponseWriter.Stopped
            {
                Id = message.ProjectionId.ToString("N"),
                Completed = message.Completed,
            };
            _writer.PublishCommand("$stopped", command);
        }

        public void Handle(CoreProjectionManagementMessage.StateReport message)
        {
            var command = new ProjectionCoreResponseWriter.StateReport
            {
                Id = message.ProjectionId.ToString("N"),
                State = message.State,
                CorrelationId = message.CorrelationId.ToString("N"),
                Position = message.Position,
                Partition = message.Partition
            };
            _writer.PublishCommand("$state", command);
        }

        public void Handle(CoreProjectionManagementMessage.ResultReport message)
        {
            var command = new ProjectionCoreResponseWriter.ResultReport
            {
                Id = message.ProjectionId.ToString("N"),
                Result = message.Result,
                CorrelationId = message.CorrelationId.ToString("N"),
                Position = message.Position,
                Partition = message.Partition
            };
            _writer.PublishCommand("$result", command);
        }

        public class Faulted
        {
            public string Id { get; set; }
            public string FaultedReason { get; set; }
        }

        public class Prepared
        {
            public string Id { get; set; }
            public ProjectionSourceDefinition SourceDefinition { get; set; }
        }

        public class SlaveProjectionReaderAssigned
        {
            public string Id { get; set; }
            public string ReaderId { get; set; }
            public string SubscriptionId { get; set; }
        }

        public class Started
        {
            public string Id { get; set; }
        }

        public class StatisticsReport
        {
            public string Id { get; set; }
            public ProjectionStatistics Statistcs { get; set; }
        }

        public class Stopped
        {
            public string Id { get; set; }
            public bool Completed { get; set; }
        }

        public class StateReport
        {
            public string Id { get; set; }
            public string CorrelationId { get; set; }
            public string State { get; set; }
            public string Partition { get; set; }

            [JsonConverter(typeof(CheckpointTagJsonConverter))]
            public CheckpointTag Position { get; set; }
        }

        public class ResultReport
        {
            public string Id { get; set; }
            public string CorrelationId { get; set; }
            public string Result { get; set; }
            public string Partition { get; set; }

            [JsonConverter(typeof(CheckpointTagJsonConverter))]
            public CheckpointTag Position { get; set; }
        }

        public class ProjectionWorkerStarted
        {
            public string Id { get; set; }
        }


    }
}