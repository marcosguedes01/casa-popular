using CasaPopularLib.Enums;
using CasaPopularLib.Handlers.FamilyGetPoints;
using CasaPopularLib.Handlers.PeopleGetEligible;
using CasaPopularLib.Models;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CasaPopularLibTests
{
    public class EligiblePeopleTest
    {
        /// <summary>
        /// Valida a listagem de famílias elegíveis
        /// </summary>
        [Fact]
        public async Task ShouldBelistEligibleFamilies()
        {
            var families = new List<Family>() {
                new Family {
                    Id = Guid.NewGuid(),
                    FullName = "Matheus Silva",
                    People = new List<FamilyPerson> {
                        new FamilyPerson {
                            Age = 20,
                            Salary = 3500,
                            Kinship = KinshipType.CLAIMANT,
                        },
                        new FamilyPerson {
                            Age = 18,
                            Salary = 1200,
                            Kinship = KinshipType.SPOUSE,
                        },
                        new FamilyPerson {
                            Age = 16,
                            Kinship = KinshipType.CHILDREN,
                        },
                        new FamilyPerson {
                            Age = 15,
                            Kinship = KinshipType.CHILDREN,
                        },
                        new FamilyPerson {
                            Age = 12,
                            Kinship = KinshipType.CHILDREN,
                        },
                    }
                },
                new Family {
                    Id = Guid.NewGuid(),
                    FullName = "Amaro Peixoto",
                    People = new List<FamilyPerson> {
                        new FamilyPerson {
                            Age = 20,
                            Salary = 3500,
                            Kinship = KinshipType.CLAIMANT,
                        },
                    }
                },
                new Family {
                    Id = Guid.NewGuid(),
                    FullName = "João Rodrigues",
                    People = new List<FamilyPerson> {
                        new FamilyPerson {
                            Age = 20,
                            Salary = 900,
                            Kinship = KinshipType.CLAIMANT,
                        }
                    } 
                },
                new Family {
                    Id = Guid.NewGuid(),
                    FullName = "Cassia Andrade",
                    People = new List<FamilyPerson> {
                        new FamilyPerson {
                            Age = 20,
                            Salary = 3500,
                            Kinship = KinshipType.CLAIMANT,
                        },
                        new FamilyPerson {
                            Age = 16,
                            Kinship = KinshipType.CHILDREN,
                        },
                    } 
                },
                new Family{
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
                }
            };

            FamilyGetPointsHandler getPointsHandler = new FamilyGetPointsHandler();
            var mediator = new Mock<IMediator>();
            mediator
                .Setup(m => m.Send(It.IsAny<FamilyGetPointsQuery>(), It.IsAny<CancellationToken>()))
                .Returns(async (FamilyGetPointsQuery q, CancellationToken token) => await getPointsHandler.Handle(new FamilyGetPointsQuery { Family = q.Family }, token));


            var eligibleQuery = new EligiblePeopleQuery { Families = families };

            EligiblePeopleHandler eligibleHandler = new EligiblePeopleHandler(mediator.Object);

            var result = await eligibleHandler.Handle(eligibleQuery, new CancellationToken());

            var expectedList = result.OrderByDescending(x => x.Points);

            Assert.NotEmpty(result);
            Assert.Equal(families.Count(), result.Count());
            Assert.True(expectedList.SequenceEqual(result));
        }

        /// <summary>
        /// Valida a existência de mesmo id na validação dos elegíveis
        /// Requisito: cada família pode pontuar uma única vez por critério, além de poder atender todos os critérios ou nenhum deles
        /// Como não foi estabelecido critério de seleção, no caso de duplicidade, optei por invalidar a lista, caso haja o mesmo id na coleção.
        /// </summary>
        [Fact]
        public async Task ShouldBeRejectBecauseHasSameId()
        {
            var sameIdTest = Guid.NewGuid();

            var people = new List<Family>() {
                new Family {
                    Id = sameIdTest,
                    FullName = "Matheus Silva",
                    People = new List<FamilyPerson> {
                        new FamilyPerson {
                            Age = 20,
                            Salary = 3500,
                            Kinship = KinshipType.CLAIMANT,
                        },
                        new FamilyPerson {
                            Age = 18,
                            Salary = 1200,
                            Kinship = KinshipType.SPOUSE,
                        },
                        new FamilyPerson {
                            Age = 16,
                            Kinship = KinshipType.CHILDREN,
                        },
                        new FamilyPerson {
                            Age = 15,
                            Kinship = KinshipType.CHILDREN,
                        },
                        new FamilyPerson {
                            Age = 12,
                            Kinship = KinshipType.CHILDREN,
                        },
                    }
                },
                new Family {
                    Id = sameIdTest,
                    FullName = "Matheus Silva",
                    People = new List<FamilyPerson> {
                        new FamilyPerson {
                            Age = 20,
                            Salary = 3500,
                            Kinship = KinshipType.CLAIMANT,
                        },
                    }
                },
                new Family {
                    Id = Guid.NewGuid(),
                    FullName = "João Rodrigues",
                    People = new List<FamilyPerson> {
                        new FamilyPerson {
                            Age = 20,
                            Salary = 900,
                            Kinship = KinshipType.CLAIMANT,
                        }
                    }
                },
            };

            FamilyGetPointsHandler getPointsHandler = new FamilyGetPointsHandler();
            var mediator = new Mock<IMediator>();
            mediator
                .Setup(m => m.Send(It.IsAny<FamilyGetPointsQuery>(), It.IsAny<CancellationToken>()))
                .Returns(async (FamilyGetPointsQuery q, CancellationToken token) => await getPointsHandler.Handle(new FamilyGetPointsQuery { Family = q.Family }, token));


            var eligibleQuery = new EligiblePeopleQuery { Families = people };

            EligiblePeopleHandler eligibleHandler = new EligiblePeopleHandler(mediator.Object);

            var result = await eligibleHandler.Handle(eligibleQuery, new CancellationToken());

            Assert.Empty(result);
        }
    }
}