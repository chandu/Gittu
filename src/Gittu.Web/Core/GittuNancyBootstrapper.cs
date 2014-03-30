﻿using Gittu.Web.Domain;
using Gittu.Web.Domain.Entities.Mapping;
using Gittu.Web.Security;
using Gittu.Web.Services;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.TinyIoc;

namespace Gittu.Web.Core
{
	public class GittuNancyBootstrapper:DefaultNancyBootstrapper
	{
		protected override void ConfigureConventions(NancyConventions nancyConventions)
		{
			base.ConfigureConventions(nancyConventions);
			nancyConventions.StaticContentsConventions.Add(
				StaticContentConventionBuilder.AddDirectory("public", @"public")
				);
			nancyConventions.StaticContentsConventions.Add(
				StaticContentConventionBuilder.AddDirectory("app", @"public/app")
				);
		}

		protected override void ConfigureApplicationContainer(TinyIoCContainer container)
		{
			container.Register<IHasher, SHA256Hasher>();
			container.Register<IMailService, DummyMailService>();
			base.ConfigureApplicationContainer(container);
		}

		protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
		{
			container.Register<IAuthenticationService, DefaultAuthenticationService>();
			container.Register<IEntityMappingsConfigurator, EntityMappingsConfigurator>();
			
			container.Register<IRegistrationService, DefaultRegistrationService>();
			container
				.Register<IGittuContext>((c, p) => new GittuContext("GittuDB"));
			container
				.Register<IUnitOfWork>((c, p) => new GittuUnitOfWork("GittuDB"));
			base.ConfigureRequestContainer(container, context);
		}
	}
}