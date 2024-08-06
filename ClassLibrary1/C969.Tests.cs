using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace C969.Tests
{
    [TestFixture]
    public class AppointmentTests
    {
        private string connectionString;

        [SetUp]
        public void Setup()
        {
            connectionString = ConfigurationManager.ConnectionStrings["localdb"]?.ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("The connection string 'localdb' could not be found.");
            }
        }

        [Test]
        public void TestDatabaseConnection()
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                Assert.DoesNotThrow(() => con.Open(), "Database connection should open without throwing an exception.");
            }
        }

        [Test]
        public void TestIsTimeConflicted_NoConflict()
        {
            // Arrange
            var start = new DateTime(2024, 07, 25, 10, 0, 0);
            var end = new DateTime(2024, 07, 25, 11, 0, 0);

            // Act
            var isConflicted = CheckTimeConflict(start, end);

            // Assert
            Assert.That(isConflicted, Is.False, "There should be no conflict with the existing appointments.");
        }

        [Test]
        public void TestIsTimeConflicted_WithConflict()
        {
            // Arrange
            var start = new DateTime(2024, 07, 23, 09, 0, 0);
            var end = new DateTime(2024, 07, 23, 10, 0, 0);

            // Act
            var isConflicted = CheckTimeConflict(start, end);

            // Assert
            Assert.That(isConflicted, Is.True, "There should be a conflict with an existing appointment.");
        }

        private bool CheckTimeConflict(DateTime start, DateTime end)
        {
            var conflictFound = false;

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var query = "SELECT COUNT(*) FROM appointment WHERE (start < @end AND end > @start)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@start", start);
                    command.Parameters.AddWithValue("@end", end);

                    var count = Convert.ToInt32(command.ExecuteScalar());

                    if (count > 0)
                    {
                        conflictFound = true;
                    }
                }
            }

            return conflictFound;
        }
    }
}
