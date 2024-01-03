using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReduxJWTLogin.Models;

namespace ReduxJWTLogin.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{

		}

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);


			builder.Entity<IdentityRole>()
				.HasData(

					new IdentityRole { Name = "User", NormalizedName = "USER" },
					new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" }

					);





    }
	}

	
}

