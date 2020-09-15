# eShop
<div class="row">
    <div class="col-8">
        <p>
            Eshop is an ecommerce application for customers and administrators.
            The front-end is developed in *Angular 9* and *Ionic 5*, and the back-end is developed in *ASP.NET Core 3.0*. The back-end includes unitary and integration tests.
        </p>
      </div>
    <div class="col-4">
        <p>
            <img src="https://user-images.githubusercontent.com/53798204/93257005-85cd3b00-f79c-11ea-939c-a8833bae37de.png" width="300" title="eShop">
        </p>
  </div>
</div>

##Front-end
To run the Ionic application: **ionic serve**
To run the Angular application: **ng serve**
#Back-end
To migrate the data base:
** dotnet ef database drop --force **
** dotnet ef migrations add InitialCreate **
** dotnet ef database update **
