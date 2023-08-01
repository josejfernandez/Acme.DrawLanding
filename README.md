# ACME's draw landing page

It's a landing page for ACME corporation. Handle with caution: it might explode.

## How to make it work

Requisites:

- .NET. Command `dotnet run` needs to work on your machine.
- Node.js and NPM. Command `npm` needs to work on your machine.

### Seting up the database

A file `database.db` is provided in the repository. It contains an empty database ready to be used. Copy it to a location in your computer.

Modify the configuration file for the website project, which is located in `/src/Acme.DrawLanding.Website/appsettings.json`, and write the location of the file you just copied. For example:

```json
{
    "ConnectionStrings": {
        "DefaultConnection": "Data Source=D:\\database.db;"
    }
}
```

### Building the frontend

Open a shell in the directory of the website project, which is located in `/src/Acme.DrawLanding.Website`, and run the following commands:

```shell
npm install
npm run build
```

### Running the backend

Open a shell in the directory of the website project, which is located in `/src/Acme.DrawLanding.Website`, and run the following commands:

```shell
dotnet run
```

Open http://localhost:5076 or https://localhost:7126 in your browser and voilà, a wild landing page appeared.

### What do to now

Find your way through the UI to:

1. Create a user. An endpoint to do this has been exposed. Username and password can be anything.
2. Access `Admin UI`. Expect the explosion at this point.
3. Just a flesh wound / login form. Enter the credentials of the user you just created.
4. Go to `View serial numbers` and find the form to create serial numbers.
5. Create serial numbers to your heart's content.
6. Pretend you are many people and enter a few serial numbers yourself.
