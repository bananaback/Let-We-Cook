﻿using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetWeCook.Data.Configurations
{
	public class DietaryPreferenceEntityTypeConfiguration : IEntityTypeConfiguration<DietaryPreference>
	{
		public void Configure(EntityTypeBuilder<DietaryPreference> builder)
		{
			builder.ToTable("dietary_preference");

			builder.HasKey(dp => dp.Id);

			builder.Property(dp => dp.Id)
				.HasColumnName("id");

			builder.Property(dp => dp.Value)
				.HasColumnName("value");
		}
	}
}