# Tide h4x for Privacy Challenge
<p>
  <a href="https://tide.org/licenses_tcosl-1-0-en">
    <img src="https://img.shields.io/badge/license-TCOS-green.svg" alt="license">
  </a>
</p>
The concept for the challenge is quite simple: The details of 1 Bitcoin were stored in the simplest website setup: on a database record behind a web server. Anyone logging in to that website using the correct username and password will see those details. All typical defences around that set up were removed. No firewalls, no added security. It should be easy work for any hacker to crack that. The twist: Tide's unique protection mechanism was used on the data and the website authentication. Supposedly, even if one cracks the whole thing, it would be impractical to crack the authentication or extract the data. Anyone who does, the bitcoin is theirs, and Tide is back to the drawing board. This project is an open-source code repository for the challenge and provides ability to recreate the entire environment locally.

### Terminology

Below are concepts that are important to understand within the context of the Tide h4x challenge.

**Seeker** - Any organization or business seeking to engage with or collect consumer data.

**Vendor** - Any consumer-facing organization or business that collects, manages and stores consumer data.

**Consumer** - Any individual natural person that has a uniquely identified representation or data footprint (usually in the form of a user account or identity) in a Vendor’s database.

**Smart Contract** - a computer protocol intended to digitally facilitate, verify, or enforce the negotiation or performance of a contract.

**ORK** - Orchestrated Recluder of Keys - The Tide Protocol nodes

**Key Pair** - consist of a Secret Key (SK) and Public Key (PK)

## Architecture

![alt text](https://github.com/tide-foundation/Tide-h4x-for-Privacy/blob/master/Tide%20h4x%20Architecture.png "Architecture Diagram")

## Flow Diagram

![alt text](https://github.com/tide-foundation/Tide-h4x-for-Privacy/blob/master/Tide%20h4x%20Workflow.png "Flow Diagram")

### Components

1. **Cryptide Library** - Tide encryption library
1. **h4x frontend Website** - website for this h4x challenge.
1. **h4x Tide SDK API** - API for the h4x challenge.
1. **h4x SQL Database** - Database for the h4x challenge.
1. **ORK Nodes** - Decentralized ORK nodes.

## Installation

This guide assists you to replicate the entire environment using EOS jungle testnet (free), local deployment with 3 ORK nodes.
![alt text](https://github.com/tide-foundation/Tide-h4x-for-Privacy/blob/master/Tide%20h4x%20Local.png "Local Setup")

### Prerequisite

1. [.NET Core 2.2 Build apps - SDK](https://dotnet.microsoft.com/download/dotnet-core/2.2 ".net Core 2.2 Download")
1. [Node.js - LTS](https://nodejs.org/en/download/ "node.js Download")
1. [Cleos](https://developers.eos.io/eosio-nodeos/v1.2.0/docs/cleos-overview "Cleos")
1. [SQL Express](https://www.microsoft.com/en-au/sql-server/sql-server-editions-express "SQL Express")
1. Clone Repository 
   Using Windows Powershell
   ```
   wget "https://github.com/tide-foundation/Tide-h4x-for-Privacy/archive/master.zip" -outfile "h4x.zip"
   Expand-Archive "h4x.zip" -Force
   ``` 
   <details>
   <summary>Linux</summary>
      
      ```
      git clone https://github.com/tide-foundation/Tide-h4x-for-Privacy
      ```
   </details>
   

#### Installing Cleos in Windows 10 
There are two options when running EOS in a Windows environment: Docker or Linux for Windows.  Provided below are the steps in running Cleos using Linux/Ubuntu for Windows 10.  

1. Get Ubuntu from Windows Store [Ubuntu Windows 10](https://www.microsoft.com/en-au/p/ubuntu/9nblggh4msv6?activetab=pivot:overviewtab "Ubuntu Windows 10")
1. Install Ubuntu from the Windows Store. Ignore the following error: 
   ```
   See https://aka.ms/wslinstall for details.
   Error: 0x8007007e
   Press any key to continue...
   ```
   Open PowerShell as Administrator and run the following command.  
   ```
   Enable-WindowsOptionalFeature -Online -FeatureName Microsoft-Windows-Subsystem-Linux
   ```
1. Install Ubuntu and Login.
1. Download and install EOS binaries 
   ```
   wget "https://github.com/tide-foundation/Tide-h4x-for-Privacy/archive/master.zip" -outfile "h4x.zip"
   Expand-Archive "h4x.zip" -Force -DestinationPath "C:\code"
   ```
### Deployment

#### EOS

This deployment utilizes EOS "jungle" testnet environment.

1. Create EOS Wallet `cleos wallet create --to-console`.  This will create a *default* wallet.  Take note of the **WALLET_PASSWORD**
1. Generate a keypair for your master account `cleos create key --to-console`.  Take note of the generated **PK_MASTER SK_MASTER** keypair.
1. Import the private keys into your cleos wallet by running `cleos wallet import --private-key SK_MASTER`.
1. Navigate to Jungle Testnet for the Account Creation [Jungle Test Net](https://monitor.jungletestnet.io/#account "Jungle"). 
1. Create a Jungle Testnet account by *Create Account*.  Use the generated PK_MASTER for the Owner Public Key and Active Public Key field.  This will be your **MASTER_ACCOUNT**
1. The MASTER_ACCOUNT will need some RAM delegated to it for the smartcontract and transaction processing. Use the *faucet* on the [Jungle Test Net](https://monitor.jungletestnet.io/#faucet "Jungle") to give your **MASTER_ACCOUNT** some EOS.  Your main account should get 100 EOS.   
1. In cleos, run the command `cleos -u http://jungle2.cryptolions.io:80 system buyram MASTER_ACCOUNT MASTER_ACCOUNT "15 EOS"`. The -u parameter is telling cleos to run this command using the jungle testnet.  
   <details>
   <summary>Unlock Wallet</summary>
   Should the wallet lock run the following command
   
   ```
   cleos wallet unlock --password WALLET_PASSWORD
   ```
1. Generate a keypair for each of the 3 ORK nodes using cleos. Take a note of the 3 created keys ORK 1 **PK_ORK1 SK_ORK1**, ORK 2 **PK_ORK2 SK_ORK2**, ORK 3 **PK_ORK3 SK_ORK3**. 
1. Using your Master Account create a new eos account for each of the 3 ORK nodes by running 
   ```
   cleos -u http://jungle2.cryptolions.io:80 create account MASTER_ACCOUNT ORK1_ACCOUNT PK_ORK1
   cleos -u http://jungle2.cryptolions.io:80 create account MASTER_ACCOUNT ORK2_ACCOUNT PK_ORK2
   cleos -u http://jungle2.cryptolions.io:80 create account MASTER_ACCOUNT ORK3_ACCOUNT PK_ORK3
   ```

1. Move the cli working folder to the location of the onboarding contract provided when pulling the repo (./src/Raziel-Contracts/onboarding)
1. Compile the onboarding contract using `eosio-cpp -abigen -o onboarding.wasm onboarding.cpp` 
1. Go up a folder (../Raziel-Contracts/) and run the command `cleos -u http://jungle2.cryptolions.io:80 set contract MASTER_ACCOUNT ./onboarding -p MASTER_ACCOUNT @active`.


#### Miscellaneous

1. Run `Raziel.Generator\dotnet restore`.
1. Build the Raziel.Generator project and run it once for each ORK Node. You can generate them all at once by indluding the optional parameter, example: `./Raziel.Generator 12`. Save the generated credentials as we will be using them for the ORK environmental variables.
1. Run `Raziel.Creator\npm install`.
1. Open Index.html inside the Razial.Creator root folder and create an account to use for the challenge. Ensure you write down the credentials used in it's creation.

#### Vendor

1. Run `Raziel.Vendor\dotnet restore`.
1. Open a shell in the Raziel.Vendor project folder and run `dotnet ef migrations database update` to push the structure to the database. Ensure you have your connection settings set in appsettings before executing.
1. Publish the project to your endpoint.

#### ORKs

1. Run `Raziel.Ork\dotnet restore`.
1. Run 3 instances of Raziel.Ork, each with their own environmental variables set.
1. Open each web service and edit the environmental variables to align with the EOS jungle testnet, the account created in EOS setup and the keys created in ‘Miscellaneous’ step 1 (the public key, private key and password).
1. Restart the web apps for the changes to take effect.
1. Test their viability by visiting localhost:orkport/discover. It should give you a json object similar to this:

```json
{
    "success": true,
    "content": {
        "account": "yourorkaccount",
        "url": "https://localhost:orkport",
        "publicKey": "ALdwxVNySq65hwkStfpSuuwz__EXAMPLE_PUBLIC_KEY__oD8PpStPQ0BXqHQd69NAD9LGzQLujEXg=="
    },
    "error": null
}
```

### Environment Setup

Environmental variables are explained below:

#### ORK nodes

```json
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

```json
{
  "VendorSettings": {
    "Connection": "Database endpoint",
    "Password": "JWT token password"
  }
}
```

#### Frontend Setup

1. Run `Raziel.Front\npm install`.
1. Open Raziel.Front\src\assets\js\config.js. Populate the ORK node endpoints array. The generation is looped as the endpoints follow a strict naming convention but this can be easily changed.
1. Open the .env.production file and set your vendor endpoint there.

#### Running the Frontend Website

1. In the Raziel.Front\ folder, open a console and run `npm run serve` and open the browser to the endpoint shown.
1. Login using your credentials from ‘Miscellaneous’ step 2

### Social

[Tide Telegram group](https://t.me/TideFoundation)

[Tide Subreddit channel](https://www.reddit.com/r/TideFoundation)
