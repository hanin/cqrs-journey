﻿// ==============================================================================================================
// Microsoft patterns & practices
// CQRS Journey project
// ==============================================================================================================
// ©2012 Microsoft. All rights reserved. Certain content used with permission from contributors
// http://cqrsjourney.github.com/contributors/members
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance 
// with the License. You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software distributed under the License is 
// distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and limitations under the License.
// ==============================================================================================================

namespace Registration.Tests.ConferenceViewModelGeneratorFixture
{
    using System;
    using System.Linq;
    using Conference;
    using Registration.Handlers;
    using Registration.ReadModel;
    using Registration.ReadModel.Implementation;
    using Xunit;

    public class given_a_database : IDisposable
    {
        protected string dbName = Guid.NewGuid().ToString();
        protected ConferenceViewModelGenerator sut;

        public given_a_database()
        {
            using (var context = new ConferenceRegistrationDbContext(dbName))
            {
                if (context.Database.Exists())
                    context.Database.Delete();

                context.Database.Create();
            }

            this.sut = new ConferenceViewModelGenerator(() => new ConferenceRegistrationDbContext(dbName));
        }

        public void Dispose()
        {
            using (var context = new ConferenceRegistrationDbContext(dbName))
            {
                if (context.Database.Exists())
                    context.Database.Delete();
            }
        }
    }

    public class given_no_conference : given_a_database
    {
        [Fact]
        public void when_conference_created_then_alias_dto_populated()
        {
            var conferenceId = Guid.NewGuid();

            this.sut.Handle(new ConferenceCreated
            {
                Name = "name",
                Description = "description",
                Slug = "test",
                Owner = new Owner
                {
                    Name = "owner",
                    Email = "owner@email.com",
                },
                SourceId = conferenceId,
                StartDate = DateTime.UtcNow.Date,
                EndDate = DateTime.UtcNow.Date,
            });

            using (var context = new ConferenceRegistrationDbContext(dbName))
            {
                var dto = context.Find<ConferenceAliasDTO>(conferenceId);

                Assert.NotNull(dto);
                Assert.Equal("name", dto.Name);
                Assert.Equal("test", dto.Code);
            }

        }

        [Fact]
        public void when_conference_created_then_description_dto_populated()
        {
            var conferenceId = Guid.NewGuid();

            this.sut.Handle(new ConferenceCreated
            {
                Name = "name",
                Description = "description",
                Slug = "test",
                Owner = new Owner
                {
                    Name = "owner",
                    Email = "owner@email.com",
                },
                SourceId = conferenceId,
                StartDate = DateTime.UtcNow.Date,
                EndDate = DateTime.UtcNow.Date,
            });

            using (var context = new ConferenceRegistrationDbContext(dbName))
            {
                var dto = context.Find<ConferenceDescriptionDTO>(conferenceId);

                Assert.NotNull(dto);
                Assert.Equal("name", dto.Name);
                Assert.Equal("description", dto.Description);
                Assert.Equal("test", dto.Code);
            }

        }

        [Fact]
        public void when_conference_created_then_conference_dto_populated()
        {
            var conferenceId = Guid.NewGuid();

            this.sut.Handle(new ConferenceCreated
            {
                Name = "name",
                Description = "description",
                Slug = "test",
                Owner = new Owner
                {
                    Name = "owner",
                    Email = "owner@email.com",
                },
                SourceId = conferenceId,
                StartDate = DateTime.UtcNow.Date,
                EndDate = DateTime.UtcNow.Date,
            });

            using (var context = new ConferenceRegistrationDbContext(dbName))
            {
                var dto = context.Find<ConferenceDTO>(conferenceId);

                Assert.NotNull(dto);
                Assert.Equal("name", dto.Name);
                Assert.Equal("description", dto.Description);
                Assert.Equal("test", dto.Code);
                Assert.Equal(0, dto.Seats.Count);
            }
        }
    }

    public class given_existing_conference : given_a_database
    {
        private Guid conferenceId = Guid.NewGuid();

        public given_existing_conference()
        {
            this.sut.Handle(new ConferenceCreated
            {
                SourceId = conferenceId,
                Name = "name",
                Description = "description",
                Slug = "test",
                Owner = new Owner
                {
                    Name = "owner",
                    Email = "owner@email.com",
                },
                StartDate = DateTime.UtcNow.Date,
                EndDate = DateTime.UtcNow.Date,
            });
        }

        [Fact]
        public void when_conference_updated_then_alias_dto_populated()
        {
            this.sut.Handle(new ConferenceUpdated
            {
                Name = "newname",
                Description = "newdescription",
                Slug = "newtest",
                Owner = new Owner
                {
                    Name = "owner",
                    Email = "owner@email.com",
                },
                SourceId = conferenceId,
                StartDate = DateTime.UtcNow.Date,
                EndDate = DateTime.UtcNow.Date,
            });

            using (var context = new ConferenceRegistrationDbContext(dbName))
            {
                var dto = context.Find<ConferenceAliasDTO>(conferenceId);

                Assert.NotNull(dto);
                Assert.Equal("newname", dto.Name);
                Assert.Equal("newtest", dto.Code);
            }

        }

        [Fact]
        public void when_conference_updated_then_description_dto_populated()
        {
            this.sut.Handle(new ConferenceUpdated
            {
                Name = "newname",
                Description = "newdescription",
                Slug = "newtest",
                Owner = new Owner
                {
                    Name = "owner",
                    Email = "owner@email.com",
                },
                SourceId = conferenceId,
                StartDate = DateTime.UtcNow.Date,
                EndDate = DateTime.UtcNow.Date,
            });

            using (var context = new ConferenceRegistrationDbContext(dbName))
            {
                var dto = context.Find<ConferenceDescriptionDTO>(conferenceId);

                Assert.NotNull(dto);
                Assert.Equal("newname", dto.Name);
                Assert.Equal("newdescription", dto.Description);
                Assert.Equal("newtest", dto.Code);
            }

        }

        [Fact]
        public void when_conference_updated_then_conference_dto_populated()
        {
            this.sut.Handle(new ConferenceUpdated
            {
                Name = "newname",
                Description = "newdescription",
                Slug = "newtest",
                Owner = new Owner
                {
                    Name = "owner",
                    Email = "owner@email.com",
                },
                SourceId = conferenceId,
                StartDate = DateTime.UtcNow.Date,
                EndDate = DateTime.UtcNow.Date,
            });

            using (var context = new ConferenceRegistrationDbContext(dbName))
            {
                var dto = context.Find<ConferenceDTO>(conferenceId);

                Assert.NotNull(dto);
                Assert.Equal("newname", dto.Name);
                Assert.Equal("newdescription", dto.Description);
                Assert.Equal("newtest", dto.Code);
                Assert.Equal(0, dto.Seats.Count);
            }
        }

        [Fact]
        public void when_seat_created_then_adds_seat_to_conference_dto()
        {
            var seatId = Guid.NewGuid();

            this.sut.Handle(new SeatCreated
            {
                ConferenceId = conferenceId,
                SourceId = seatId,
                Name = "seat",
                Description = "description",
                Price = 200,
            });

            using (var context = new ConferenceRegistrationDbContext(dbName))
            {
                var dto = context.Set<ConferenceDTO>()
                    .Where(x => x.Id == conferenceId)
                    .SelectMany(x => x.Seats)
                    .FirstOrDefault(x => x.Id == seatId);

                Assert.NotNull(dto);
                Assert.Equal("seat", dto.Name);
                Assert.Equal("description", dto.Description);
                Assert.Equal(200, dto.Price);
            }

        }

        [Fact]
        public void when_seat_updated_then_updates_seat_on_conference_dto()
        {
            var seatId = Guid.NewGuid();

            this.sut.Handle(new SeatCreated
            {
                ConferenceId = conferenceId,
                SourceId = seatId,
                Name = "seat",
                Description = "description",
                Price = 200,
            });

            this.sut.Handle(new SeatUpdated
            {
                ConferenceId = conferenceId,
                SourceId = seatId,
                Name = "newseat",
                Description = "newdescription",
                Price = 100,
            });

            using (var context = new ConferenceRegistrationDbContext(dbName))
            {
                var dto = context.Set<ConferenceDTO>()
                    .Where(x => x.Id == conferenceId)
                    .SelectMany(x => x.Seats)
                    .FirstOrDefault(x => x.Id == seatId);

                Assert.NotNull(dto);
                Assert.Equal("newseat", dto.Name);
                Assert.Equal("newdescription", dto.Description);
                Assert.Equal(100, dto.Price);
            }
        }
    }
}
