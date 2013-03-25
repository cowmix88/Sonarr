﻿using System.Reflection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using NzbDrone.Core.Datastore.Migration.Sqlite;

namespace NzbDrone.Core.Datastore.Migration.Framework
{
    public interface IMigrationController
    {
        void MigrateToLatest(string connectionString, MigrationType migrationType);
    }

    public class MigrationController : IMigrationController
    {
        private readonly IAnnouncer _announcer;

        public MigrationController(IAnnouncer announcer)
        {
            _announcer = announcer;
        }

        public void MigrateToLatest(string connectionString, MigrationType migrationType)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var migrationContext = new RunnerContext(_announcer)
            {
                Namespace = "NzbDrone.Core.Datastore.Migration",
                ApplicationContext = migrationType
            };

            var options = new MigrationOptions { PreviewOnly = false, Timeout = 60 };
            var factory = new MonoSqliteProcessorFactory();
            var processor = factory.Create(connectionString, _announcer, options);
            var runner = new MigrationRunner(assembly, migrationContext, processor);
            runner.MigrateUp(true);
        }
    }
}