# ACME's draw landing page

It's a landing page for ACME corporation. Handle with caution: it might explode.

## How to make it work

Requisites:

- .NET. Command `dotnet run` needs to work on your machine.
- Node.js and NPM. Command `npm` needs to work on your machine.

### Setting up the database

A file `database.db` is provided in the repository. It contains an empty database ready to be used. Copy it to a location in your computer.

Modify the configuration file for the website project, which is located in `/src/Acme.DrawLanding.Website/appsettings.json`, and write the location of the file you just copied. For example:

```json
{
    "ConnectionStrings": {
        "DefaultConnection": "Data Source=D:\\database.db;"
    }
}
```

### Changing the encryption key

The application uses an encryption key to encrypt sensitive data, such as passwords, in the database. This key can be modified in the configuration file. It needs to be a 256 bits key, written in base64 in the configuration file:

```json
{
    "EncryptionKey": "WgEIdue6PRk2Ksv4qBIGucNN1Eep2F+/B9PaestJA5s=",
}
```

A sample key is provided, but should be changed **before creating any user**. Once the first user has been created, the key should not be changed, since the application provides no means to update already encrypted data with the new key.

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
