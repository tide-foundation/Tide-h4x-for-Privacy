# Tide h4x for Privacy Challenge

The concept for the challenge is quite simple: The details of 1 Bitcoin were stored in the simplest website setup: on a database record behind a web server. Anyone logging in to that website using the correct username and password will see those details. All typical defences around that set up were removed. No firewalls, no added security. It should be easy work for any hacker to crack that. The twist: Tide's unique protection mechanism was used on the data and the website authentication. Supposedly, even if one cracks the whole thing, it would be impractical to crack the authentication or extract the data. Anyone who does, the bitcoin is theirs, and Tide is back to the drawing board. This project is an open-source code repository for the challenge and provides ability to recreate the entire environment locally.

### Terminology

Below are concepts that are important to understand within the context of the Tide h4x challenge.

**Seeker** - Any organization or business seeking to engage with or collect consumer data.

**Vendor** - Any consumer-facing organization or business that collects, manages and stores consumer data.

**Consumer** - Any individual natural person that has a uniquely identified representation or data footprint (usually in the form of a user account or identity) in a Vendor’s database.

**Smart Contract** - a computer protocol intended to digitally facilitate, verify, or enforce the negotiation or performance of a contract.

**ORK** - Orchestrated Recluder of Keys

## Architecture

![alt text](https://github.com/tide-foundation/Tide-h4x-for-Privacy/blob/master/Tide%20h4x%20Architecture.png "Architecture Diagram")

## Flow Diagram

![alt text](https://github.com/tide-foundation/Tide-h4x-for-Privacy/blob/master/Tide%20h4x%20Workflow.png "Flow Diagram")

### Components

1. **cryptide library** - Tide encryption library
1. **h4x frontend Website** - website for this h4x challenge.
1. **h4x Tide SDK API** - API for the h4x challenge.
1. **h4x SQL Database** - Database for the h4x challenge.
1. **ORK Nodes** - ORK nodes running in Local.

## Installation

### Prerequisite

1. [.NET Core 2.2 SDK](https://dotnet.microsoft.com/download/dotnet-core/2.2 ".net Core 2.2 Download")
1. [Node.js](https://nodejs.org/en/download/ "node.js Download")
1. [Cleos](https://developers.eos.io/eosio-nodeos/v1.2.0/docs/cleos-overview "Cleos")
1. [SQL Express](https://www.microsoft.com/en-au/sql-server/sql-server-editions-express "SQL Express")

### Project Setup

1. Run `Git Clone https://github.com/tide-foundation/Tide-h4x-for-Privacy`
1. Run `Raziel.Ork\dotnet restore`
1. Run `Raziel.Library\dotnet restore`
1. Run `Raziel.Vendor\dotnet restore`
1. Run `Raziel.Generator\dotnet restore`
1. Run `Raziel.Front\npm install`
1. Run `Raziel.Creator\npm install`

### Deployment

#### EOS

In this challenge we used an EOS Environment.

1. Generate a keypair for your master account and each of your ork nodes using cleos. Run `cleos create key --to-console`.
1. Import the private keys into your cleos wallet by running `cleos wallet import --private-key YOUR_PRIVATE_KEY`.
1. Using your master account create a new eos account for each of your ORKS by running `cleos create account YOURMASTERACCOUNT YOURORKACCOUNT YOURORKACCOUNT_PUBLIC_KEY`.
1. Fund the master account with enough RAM to run the challenge. 10-15 should be sufficient. Example: `cleos system buyram YOURMASTERACCOUNT YOURMASTERACCOUNT "15 EOS"`.
1. Compile the onboarding contract using `eosio-cpp -abigen -o onboarding.wasm onboarding.cpp` and push it to the contract holder account using `cleos set contract YOURMASTERACCOUNT ./onboarding -p YOURMASTERACCOUNT @active`.

#### Miscellaneous

1. Build the Raziel.Generator project and run it once for each ORK Node. You can generate them all at once by indluding the optional parameter, example: `./Raziel.Generator 12`. Save the generated credentials as we will be using them for the ORK environmental variables.
1. Run the Raziel.Creator and create an account to use for the challenge. Ensure you write down the credentials used in it's creation.

#### Vendor

1. Open a cli in the Raziel.Vendor project folder and run `dotnet ef migrations database update` to push the structure to the database. Ensure you have your connection settings set in appsettings before executing.
1. Publish the project to your endpoint.

#### ORKs

1. Open Raziel.Ork and Publish to your endpoints. This can be done easily via right clicking the project in Visual Studio -> publish or via powershell using `dotnet publish Raziel.Ork.csproj /p:PublishProfile=$profile /p:Password=$pass’`.
1. Open each web service and edit the environmental variables to align with the blockchain you’re using, the account created in EOS setup and the keys created in ‘Miscellaneous’ step 1 (the public key, private key and password).
1. Restart the web apps for the changes to take effect.
1. Test their viability by visiting yourorkendpoint.com/discover. It should give you a json object similar to this:

```
{
    "success": true,
    "content": {
        "account": "yourorkaccount",
        "url": "https://yourorkendpoint.net",
        "publicKey": "ALdwxVN4QlL9DSySq65hwkStfpSuuwz__EXAMPLE_PUBLIC_KEY__oD8PpStPQ0BXqHQd6vV5bjf79NAD9LGzQLujEXg=="
    },
    "error": null
}
```

### Environment Setup

Environmental variables are explained below:

#### ORK nodes

```
{
  "Settings": {
    "Account": "Blockchain account identifier",
    "PublicKey": "elGamal public key",
    "PrivateKey": " elGamal private key ",
    "Onboarding": "Authentication smart contract",
    "EosPrivateKey": "Blockchain account private key",
    "Password": "AES encryption passphrase",
    "Key": "Key used for generation of junk data",
    "BlockchainChainId": "Blockchain Chain Id",
    "BlockchainEndpoint": "Blockchain API Endpoint",
    "UsersTable": "Users table in the authentication contract",
    "FragmentsTable": " Fragment table in the authentication contract "

  }
}
```

#### Vendor

```
{
  "VendorSettings": {
    "Connection": "Database endpoint",
    "Password": "JWT token password"
  }
}
```

#### Frontend Setup

1. Open Raziel.Front\src\assets\js\config.js. Populate the ORK node endpoints array. The generation is looped as the endpoints follow a strict naming convention but this can be easily changed.
1. Open the .env.production file and set your vendor endpoint there.

#### Running the Frontend Website

1. In the Raziel.Front\ folder, open a console and run `npm run build`. This will build a production instance.
1. Publish the results to your web app endpoint. An easy way to do this is via FTP. Alternatively you can run it locally by running `npm run serve` and open the browser to the endpoint shown.
1. Login using your credentials from ‘Miscellaneous’ step 2

### Social

[Tide Telegram group](https://t.me/TideFoundation)

[Tide Subreddit channel](https://www.reddit.com/r/TideFoundation)
