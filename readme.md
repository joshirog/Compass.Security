# Compass Security

NET 5 &amp; PostgreSQL

> https://compass-security.herokuapp.com

---

* Clean Arquitecture with CQRS 
* Mediatr
* Openiddict
* Send In Blue Email
* Firebase Storage
* Lazy Cache
* Google reCatpcha v3
* Google oAuth
* Facebook oAuth
* Twitter oAuth
* LinkedIn oAuth
* Windows Account oAuth

## Migrations
~~~
* dotnet ef database update -s Compass.Security.Web -p Compass.Security.Infrastructure

* dotnet ef database update 0 -s Compass.Security.Web -p Compass.Security.Infrastructure
* dotnet ef migrations remove -s Compass.Security.Web -p Compass.Security.Infrastructure

* dotnet ef migrations add CreateInitialScheme -s Compass.Security.Web -p Compass.Security.Infrastructure -o Persistences/Migrations
~~~

## OpenID

> https://compass-security.herokuapp.com/.well-known/openid-configuration