﻿using System.Linq;
using System;
using System.Reflection;
using NLog;

namespace NzbDrone.Core.Datastore
{
    public class MigrationsHelper
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();


        public static void Run(string connectionString, bool trace)
        {
            SqlCeProxy.EnsureDatabase(connectionString);

            logger.Trace("Preparing to run database migration");

            try
            {
                Migrator.Migrator migrator;
                if (trace)
                {
                    migrator = new Migrator.Migrator("sqlserverce", connectionString, Assembly.GetAssembly(typeof(MigrationsHelper)), true, new MigrationLogger());
                }
                else
                {
                    migrator = new Migrator.Migrator("sqlserverce", connectionString, Assembly.GetAssembly(typeof(MigrationsHelper)));
                }

                migrator.MigrateToLastVersion();
                logger.Info("Database migration completed");


            }
            catch (Exception e)
            {
                logger.FatalException("An error has occurred while migrating database", e);
                throw;
            }
        }



        public static string GetIndexName(string tableName, params string[] columns)
        {
            return String.Format("IX_{0}_{1}", tableName, String.Join("_", columns));
        }
    }




}