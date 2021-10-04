using System;
using System.Collections.Generic;
using Compass.Security.Domain.Commons.Enums;
using Compass.Security.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compass.Security.Infrastructure.Persistences.Seeders
{
    public class BlacklistSeeder: IEntityTypeConfiguration<Blacklist>
    {
        public void Configure(EntityTypeBuilder<Blacklist> builder)
        {
            var date = new DateTime(2012, 6, 15);
            
            builder.HasData(new List<Blacklist>
            {
                #region seed

                new()
                {
                    Id = Guid.NewGuid(), 
                    Type = Enum.GetName(typeof(BlackListTypeEnum), BlackListTypeEnum.Password),
                    Status = Enum.GetName(typeof(StatusEnum), StatusEnum.Active),
                    Value = "Secret2020$$",
                    CreatedBy = "migrations",
                    CreatedAt = date
                }
                
                #endregion
            });
        }
    }
}