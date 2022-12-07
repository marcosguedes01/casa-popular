using CasaPopularLib.Enums;
using CasaPopularLib.Handlers.FamilyGetPoints;
using CasaPopularLib.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CasaPopularLibTests
{
    public class TotalFamilyIncomeTest
    {
        /// <summary>
        /// Renda total da família até 900 reais - valendo 5 pontos;
        /// </summary>
        [Fact]
        public async Task TotalFamilyIncome900ShouldBe5PointsAsync()
        {
            var family = new Family
            {
                Id = Guid.NewGuid(),
                FullName = "João Rodrigues",
                People = new List<FamilyPerson> {
                    new FamilyPerson {
                        Age = 20,
                        Salary = 900,
                        Kinship = KinshipType.CLAIMANT,
                    }
                }
            };

            var query = new FamilyGetPointsQuery { Family = family };

            FamilyGetPointsHandler handler = new FamilyGetPointsHandler();

            var expectedResult = 5;
            var result = await handler.Handle(query, new CancellationToken());

            Assert.True(result.IsValidToAnalyse);
            Assert.Equal(expectedResult, result.Points);
        }

        /// <summary>
        /// Renda total da família de 901 à 1500 reais - valendo 3 pontos;
        /// </summary>
        [Fact]
        public async Task TotalFamilyIncomeBetween901And1500ShouldBe3PointsAsync()
        {
            var family = new Family
            {
                Id = Guid.NewGuid(),
                FullName = "Amanda Alencar",
                People = new List<FamilyPerson> {
                    new FamilyPerson {
                        Age = 20,
                        Salary = 600,
                        Kinship = KinshipType.CLAIMANT,
                    },
                    new FamilyPerson {
                        Age = 18,
                        Salary = 800,
                        Kinship = KinshipType.SPOUSE,
                    }
                }
            };

            var query = new FamilyGetPointsQuery { Family = family };

            FamilyGetPointsHandler handler = new FamilyGetPointsHandler();

            var expectedResult = 3;
            var result = await handler.Handle(query, new CancellationToken());

            Assert.True(result.IsValidToAnalyse);
            Assert.Equal(expectedResult, result.Points);
        }

        /// <summary>
        /// Famílias com 3 ou mais dependentes  (lembrando que dependentes maiores de 18 anos não contam) - valendo 3 pontos;
        /// </summary>
        [Fact]
        public async Task FamilyWithThreeOrMoreDependentsShouldBe3PointsAsync()
        {
            var family = new Family
            {
                Id = Guid.NewGuid(),
                FullName = "Matheus Silva",
                People = new List<FamilyPerson> {
                    new FamilyPerson {
                        Age = 20,
                        Salary = 3500,
                        Kinship = KinshipType.CLAIMANT
                    },
                    new FamilyPerson {
                        Age = 18,
                        Salary = 1200,
                        Kinship = KinshipType.SPOUSE
                    },
                    new FamilyPerson {
                        Age = 16,
                        Kinship = KinshipType.CHILDREN
                    },
                    new FamilyPerson {
                        Age = 15,
                        Kinship = KinshipType.CHILDREN
                    },
                    new FamilyPerson {
                        Age = 12,
                        Kinship = KinshipType.CHILDREN
                    },
                }
            };

            var query = new FamilyGetPointsQuery { Family = family };

            FamilyGetPointsHandler handler = new FamilyGetPointsHandler();

            var expectedResult = 3;
            var result = await handler.Handle(query, new CancellationToken());

            Assert.True(result.IsValidToAnalyse);
            Assert.Equal(expectedResult, result.Points);
        }

        /// <summary>
        /// Famílias com 1 ou 2 dependentes  (lembrando que dependentes maiores de 18 anos não contam) - valendo 2 pontos.
        /// </summary>
        [Fact]
        public async Task FamilyWithOneOrTwoDependentsShouldBe2PointsAsync()
        {
            var family = new Family
            {
                Id = Guid.NewGuid(),
                FullName = "Cassia Andrade",
                People = new List<FamilyPerson> {
                    new FamilyPerson {
                        Age = 20,
                        Salary = 3500,
                        Kinship = KinshipType.CLAIMANT
                    },
                    new FamilyPerson {
                        Age = 16,
                        Kinship = KinshipType.CHILDREN
                    },
                }
            };

            var query = new FamilyGetPointsQuery { Family = family };

            FamilyGetPointsHandler handler = new FamilyGetPointsHandler();

            var expectedResult = 2;
            var result = await handler.Handle(query, new CancellationToken());

            Assert.True(result.IsValidToAnalyse);
            Assert.Equal(expectedResult, result.Points);
        }

        /// <summary>
        /// Teste para casos que não se enquadram nas regras anteriores
        /// </summary>
        [Fact]
        public async Task ShouldBe0PointsAsync()
        {
            var family = new Family
            {
                Id = Guid.NewGuid(),
                FullName = "Amaro Peixoto",
                People = new List<FamilyPerson> {
                    new FamilyPerson {
                        Age = 20,
                        Salary = 3500,
                        Kinship = KinshipType.CLAIMANT
                    },
                }
            };

            var query = new FamilyGetPointsQuery { Family = family };

            FamilyGetPointsHandler handler = new FamilyGetPointsHandler();

            var expectedResult = 0;
            var result = await handler.Handle(query, new CancellationToken());

            Assert.True(result.IsValidToAnalyse);
            Assert.Equal(expectedResult, result.Points);
        }
    }
}