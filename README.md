# Tide h4x for Privacy Challenge

Welcome to the Tide h4x Challenge. 

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
1. **h4x frontend Website** -  website for this h4x challenge.  
1. **h4x Tide SDK API** - API for the h4x challenge.  
1. **h4x SQL Database** - Database for the h4x challenge. 
1. **ORK Nodes**  - ORK nodes running in Azure, AWS and Google. 

## Installation
### Prerequisite
1. [.NET Core 2.2 SDK](https://dotnet.microsoft.com/download/dotnet-core/2.2 ".net Core 2.2 Download")
1. [Node.js](https://nodejs.org/en/download/ "node.js Download")
1. [Cleos](https://developers.eos.io/eosio-nodeos/v1.2.0/docs/cleos-overview "Cleos")

### Project Setup
1. Git Clone
1. Run ```Raziel.Ork\dotnet restore``` 
1. Run ```Raziel.Library\dotnet restore```
1. Run ```Raziel.Vendor\dotnet restore```
1. Run ```Raziel.Front\npm install```

### Environment Setup
Set your environmental variables for each node. Variables explained below:

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
#### EOS  Environment
In this challenge we used an EOS Environment. 

1. Generate a keypair, an easy way to do this is by using scatter or https://nadejde.github.io/eos-token-sale/
1. Create accounts. Once for the contract holder and one account for every ork you plan to use.
1. Fund the accounts and seed the contract holder account with enough ram using cleos. Example: ```cleos -u http://jungle2.cryptolions.io:80 system buyram buyeraccount contractholderaccount "5 EOS"```
1. Compile the onboarding contract using ```eosio-cpp -abigen -o onboarding.wasm onboarding.cpp``` and push it to the contract holder account using ```cleos -u http://jungle2.cryptolions.io:80 set contract contractholderaccount ./onboarding -p contractholderaccount @active``` 

### Deploying the Environment

#### Deploying the ORKs
1. Publish your selected number of orks to the cloud (AWS, Google, Azure) and take note of the endpoints for these will be required when setting up the frontend.

#### Deploying the Vendor 
1. After the vendor connection settings have been filled in, open a console in the root folder and run ```dotnet ef migrations database update``` to push the structure to the database. 
1. Publish the vendor to the cloud. 

#### Deploying the Smart Contract
1. In the *\Raziel.Front*, open *config.js* 
1. Populate the ORK nodes endpoint array. The generation is looped as the endpoints follow a strict naming convention. 
1. Open the .env production file and set your vendor endpoint there.
1. For EOS, compile the onboarding smart contract and push it to your master EOS account.  Also included are multiple interfaces for your chosen blockchain. 

#### Running the Frontend Website
1. In the frontend folder, open a console and run ```npm run build```. This will build a production instance. 
1. Publish the results to your web app endpoint. An easy way to do this is via FTP. Alternatively you can run it locally by running ```npm run serve``` and open the browser to the endpoint shown.


### Social
[Tide Telegram group](https://t.me/TideFoundation)

[Tide Reddit group](https://www.reddit.com/r/TideFoundation)

